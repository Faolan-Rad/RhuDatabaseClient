using System;
using System.Collections.Generic;

using RhuDBShared;

namespace ServerDB
{
	public abstract class ServerDB : DBRoot
	{
		public byte ServerID;
		public override T GetElement<T>(ulong snowFlake) {
			try {
				return (T)Elements[snowFlake];
			}
			catch {
				return default;
			}
		}
		public abstract ServerDBRoot RootData { get; }

		public void AddNewElement(DBElement element) {
			element.BuildRefs();
			Elements.Add(element.SnowFlake,element);
		}

		public string RequestData(DatabaseClent DatabaseClent,ulong snowFlake) {
			DatabaseClent.WatchGroup.Add(snowFlake);
			return Serialize(Elements[snowFlake]);
		}
		public ulong MarkLoaded(DatabaseClent DatabaseClent, ulong snowFlake) {
			DatabaseClent.WatchGroup.Add(snowFlake);
			return Elements[snowFlake].LastEditEpoch;
		}
	}

	public class ServerDB<T>:ServerDB where T : ServerDBRoot, new()
	{
		public List<DatabaseClent> databaseClents = new List<DatabaseClent>();

		public T ServerData { get; private set; }

		public void RegisterClient(IClientConnection clientConnection) {
			var newclient = new DatabaseClent {
				clientConection = clientConnection,
			};
			clientConnection.DatabaseClent = newclient;
			databaseClents.Add(newclient);
			clientConnection.OnLostConnection += () => databaseClents.Remove(newclient);
		}

		public override ServerDBRoot RootData => ServerData;

		public override void ElementUpdated(DBElement dBElement) {
			foreach (var item in databaseClents) {
				item.NotifyUpdate(dBElement);
			}
		}
		public void Initialize()  {
			Elements = new Dictionary<ulong, DBElement>();
			ServerData = new T();
			RootData.Initialize(this);
			RootData.BuildSets();
		}
	}
}
