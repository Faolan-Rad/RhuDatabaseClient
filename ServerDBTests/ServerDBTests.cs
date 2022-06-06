using Microsoft.VisualStudio.TestTools.UnitTesting;

using RhuDB;

using ServerDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestModel;

namespace RhuDB.Tests
{
	public class TestServerModel: ServerDBRoot
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
		[TestMethod]
		public void TestBuildDB() {
			BuildDB();
		}
		[TestMethod]
		public void TestAddDataDB() {
			var database = BuildDB();
			var newElement = database.ServerData.Users.AddNewElement(new User { UserName = "jeff" });
			if(newElement.SnowFlake == 0) {
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
	}
}