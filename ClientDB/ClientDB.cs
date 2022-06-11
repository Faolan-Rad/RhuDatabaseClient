using System;

using Newtonsoft.Json;

using RhuDBShared;

namespace RhuDB
{
	public class ClientDB: DBRoot
	{
		public IServerClientLink ServerRoot { get; set; }

		public JsonSerializerSettings jsonSerializer;

		public void LoadDefaultJsonSerializer() {
			jsonSerializer = new JsonSerializerSettings {
				ObjectCreationHandling = ObjectCreationHandling.Replace
			};
		}

		private void OnResive(string data) {
			var msg = JsonConvert.DeserializeObject<UpdateDataCall>(data);
			Elements[msg.Target].Updated = true;
			Elements.Remove(msg.Target);
		}

		public ClientDB(IServerClientLink ServerLink) {
			LoadDefaultJsonSerializer();
			ServerLink.ReservedData = OnResive;
			ServerRoot = ServerLink;
			Elements = new System.Collections.Generic.Dictionary<ulong, DBElement>();
		}

		public ClientDB(IServerClientLink ServerUri, System.Collections.Generic.Dictionary<ulong, DBElement> cache) {
			LoadDefaultJsonSerializer();
			ServerRoot = ServerUri;
			Elements = cache;
			foreach (var item in Elements.Keys) {
				var lastUpdate = ServerRoot.MarkLoaded(item);
				if(lastUpdate != Elements[item].LastEditEpoch) {
					Elements.Remove(item);
				}
			}
		}


		public override T GetElement<T>(ulong snowFlake) {
			if (Elements.ContainsKey(snowFlake)) {
				return ((T)Elements[snowFlake]).UpdateCheck();
			}
			else {
				var newdata = ServerRoot.RequestData(snowFlake);
				var returndata = JsonConvert.DeserializeObject<T>(newdata, jsonSerializer);
				returndata.Initialize(this);
				Elements.Add(snowFlake, returndata);
				return returndata;
			}
		}

		public override void ElementUpdated(DBElement dBElement) {
			throw new Exception("Client can not updateData");
		}
	}
}
