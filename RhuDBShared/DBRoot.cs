using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace RhuDBShared
{
	public abstract class DBRoot
	{
		public Dictionary<ulong, DBElement> Elements;

		public abstract T GetElement<T>(ulong snowFlake) where T : DBElement;

		public abstract void ElementUpdated(DBElement dBElement);

		public virtual string Serialize(DBElement dBElement) {
			return JsonConvert.SerializeObject(dBElement);
		}
	}
}
