using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace RhuDBShared
{
	public interface IDBRef {
		ulong TargetSnowFlake { get; set; }
	}

	public class DBRef<T> : DBObject, IDBRef where T : DBElement
	{
		[JsonIgnore]
		public T Target
		{
			get {
				if ((_target?.Updated??true) || _target.SnowFlake != TargetSnowFlake) {
					_target = TargetSnowFlake == 0 ? null : DBRoot.GetElement<T>(TargetSnowFlake);
				}
				return _target;
			}
		}
		[JsonIgnore]
		private T _target;

		public ulong TargetSnowFlake
		{
			get;
			set;
		}

	}
}
