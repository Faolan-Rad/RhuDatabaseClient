using System;
using System.Collections.Generic;
using System.Text;

namespace RhuDB
{
	public interface IServerClientLink
	{
		string RequestData(ulong snowFlake);
		ulong MarkLoaded(ulong snowFlake);

		Action<string> ReservedData { get; set; }

	}
}
