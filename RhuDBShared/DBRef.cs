using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhuDBShared
{
	public interface IDBRef {
		ulong TargetSnowFlake { get; set; }
	}

	public class DBRef<T> : DBObject, IDBRef where T : DBElement
	{
		public T Target { get; private set; }

		public ulong TargetSnowFlake
		{
			get => Target?.SnowFlake ?? 0;
			set => Target = DBRoot.GetElement<T>(value);
		}

	}
}
