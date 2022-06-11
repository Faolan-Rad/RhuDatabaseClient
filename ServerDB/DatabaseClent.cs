using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using RhuDBShared;

namespace ServerDB
{
	public class DatabaseClent
	{
		public IClientConnection clientConection;

		public HashSet<ulong> WatchGroup = new HashSet<ulong>();

		public void NotifyUpdate(DBElement dBElement) {
			if (WatchGroup.Contains(dBElement.SnowFlake)) {
				var updateMsg = new UpdateDataCall {
					Target = dBElement.SnowFlake
				};
				clientConection.SendData(JsonConvert.SerializeObject(updateMsg));
			}
		}
	}
}
