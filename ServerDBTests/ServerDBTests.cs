using RhuDB;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ServerDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestModel;
using RhuDBShared;

namespace RhuDB.Tests
{
	public class TestServerModel : ServerDBRoot
	{
		public readonly DBSet<User> Users;

		public readonly DBSet<World> Worlds;
	}

	[TestClass()]
	public class ServerDBTests
	{
		public ServerDB<TestServerModel> BuildDB() {
			var db = new ServerDB<TestServerModel>();
			db.Initialize();
			return db;
		}

		public (ServerDB<TestServerModel>,World,World) BuildTestDB() {
			var db = BuildDB();
			var newowner = db.ServerData.Users.AddNewElement(new User { UserName = "jeffBoy0" });
			var OwnerTwo = db.ServerData.Users.AddNewElement(new User { UserName = "jeffBoy312" });
			var world = db.ServerData.Worlds.AddNewElement(new World { WorldName = "jeffsWorld" });
			world.Owner.TargetSnowFlake = newowner.SnowFlake;
			var worldTwo = db.ServerData.Worlds.AddNewElement(new World { WorldName = "jeffssefWorld" });
			worldTwo.Owner.TargetSnowFlake = OwnerTwo.SnowFlake;
			return (db,world,worldTwo);
		}
		[TestMethod]
		public void TestBuildDB() {
			BuildDB();
		}
		[TestMethod]
		public void TestAddDataDB() {
			var database = BuildDB();
			var newElement = database.ServerData.Users.AddNewElement(new User { UserName = "jeff" });
			if (newElement.SnowFlake == 0) {
				throw new Exception("SnowFlake is null");
			}
		}
		[TestMethod]
		public void TestAddAndFindDataDB() {
			var database = BuildDB();
			var newElement = database.ServerData.Users.AddNewElement(new User { UserName = "jeff" });
			if (newElement.SnowFlake == 0) {
				throw new Exception("SnowFlake is null");
			}
			var element = database.GetElement<User>(newElement.SnowFlake);
			Assert.AreEqual(element, newElement);
		}

		[TestMethod]
		public void TestRef() {
			var database = BuildDB();
			var user = database.ServerData.Users.AddNewElement(new User { UserName = "jeff" });
			var world = database.ServerData.Worlds.AddNewElement(new World { WorldName = "jeffsWorld" });
			world.Owner.TargetSnowFlake = user.SnowFlake;
			Assert.AreEqual(world.Owner.Target, user);
		}

		[TestMethod]
		public void TestSerlizer() {
			var database = BuildDB();
			var data = database.Serialize(new User { UserName = "jeff" });
			Assert.AreEqual("{\"UserName\":\"jeff\",\"ProfileUrl\":null,\"LastEditEpoch\":0,\"SnowFlake\":0}", data);
		}

		[TestMethod]
		public void TestSerlizerRefs() {
			var database = BuildDB();
			var user = database.ServerData.Users.AddNewElement(new User { UserName = "jeff" });
			var world = database.ServerData.Worlds.AddNewElement(new World { WorldName = "jeffsWorld" });
			world.Owner.TargetSnowFlake = user.SnowFlake;
			Console.WriteLine(database.Serialize(world));
		}

		public class FakeClient : IServerClientLink,IClientConnection
		{
			public ServerDB<TestServerModel> ServerDB;

			public FakeClient(ServerDB<TestServerModel> serverDB) {
				ServerDB = serverDB;
			}

			public DatabaseClent DatabaseClent { get; set; }
			public Action<string> ReservedData { get; set; }

			public event Action OnLostConnection;

			public ulong MarkLoaded(ulong snowFlake) {
				return ServerDB.MarkLoaded(DatabaseClent, snowFlake);
			}

			public string RequestData(ulong snowFlake) {
				return ServerDB.RequestData(DatabaseClent,snowFlake);
			}

			public void SendData(string data) {
				ReservedData.Invoke(data);
			}
		}

		public ClientDB BuildTestClient(FakeClient fakeClient) {
			var newCLient = new ClientDB(fakeClient);
			return newCLient;
		}

		[TestMethod]
		public void RunClientTestOne() {
			var (database,worldone,worldtwo) = BuildTestDB();
			var fakeclient = new FakeClient(database);
			var fakeClientDB = new ClientDB(fakeclient);
			database.RegisterClient(fakeclient);
			var world = fakeClientDB.GetElement<World>(worldone.SnowFlake);
			Assert.IsNotNull(world);
			Assert.IsNotNull(world.Owner);
			Assert.IsNotNull(world.Owner.Target);
			Assert.AreEqual(worldone.Owner.Target.UserName, world.Owner.Target.UserName);
		}

		[TestMethod]
		public void RunClientTestUpdate() {
			var (database, worldone, worldtwo) = BuildTestDB();
			var fakeclient = new FakeClient(database);
			var fakeClientDB = new ClientDB(fakeclient);
			database.RegisterClient(fakeclient);
			var world = fakeClientDB.GetElement<World>(worldone.SnowFlake);
			worldone.Owner.Target.UserName = "ThisIsMyNewUserName";
			worldone.Owner.Target.UpdateData();
			Assert.IsNotNull(world);
			Assert.IsNotNull(world.Owner);
			Assert.IsNotNull(world.Owner.Target);
			Assert.AreEqual(worldone.Owner.Target.UserName, world.Owner.Target.UserName);
			worldone.Owner.Target.UserName = "ThisIsMyNewUserName Yep i changedIt again";
			worldone.Owner.Target.UpdateData();
			Assert.IsNotNull(world);
			Assert.IsNotNull(world.Owner);
			Assert.IsNotNull(world.Owner.Target);
			Assert.AreEqual(worldone.Owner.Target.UserName, world.Owner.Target.UserName);
		}
		[TestMethod]
		public void RunClientTestUpdateAtRoot() {
			var (database, worldone, worldtwo) = BuildTestDB();
			var fakeclient = new FakeClient(database);
			var fakeClientDB = new ClientDB(fakeclient);
			database.RegisterClient(fakeclient);
			var world = fakeClientDB.GetElement<World>(worldone.SnowFlake);
			worldone.WorldName = "NewWorldName";
			worldone.UpdateData();
			world = world.UpdateCheck();
			Assert.AreEqual(worldone.WorldName, world.WorldName);
			worldone.WorldName = "NewWorld";
			worldone.UpdateData();
			world = world.UpdateCheck();
			Assert.AreEqual(worldone.WorldName, world.WorldName);
			worldone.WorldName = "NewWorldBLaBla";
			worldone.UpdateData();
			world = world.UpdateCheck();
			Assert.AreEqual(worldone.WorldName, world.WorldName);
		}
	}
}