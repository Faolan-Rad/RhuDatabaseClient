using System;

using RhuDBShared;

namespace ClientDB
{
	public class ClientDB: DBRoot
	{
		public Uri ServerRoot { get; set; }

		public ClientDB(Uri ServerUri) {
			ServerRoot = ServerUri;
		}

		public override T GetElement<T>(ulong snowFlake) {
			throw new NotImplementedException();
		}
	}
}
