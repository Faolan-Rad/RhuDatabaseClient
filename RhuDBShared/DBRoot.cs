using System;
using System.Collections.Generic;
using System.Text;

namespace RhuDBShared
{
	public abstract class DBRoot
	{
		public Dictionary<ulong, DBElement> Elements;

		public abstract T GetElement<T>(ulong snowFlake) where T : DBElement;
	}
}
