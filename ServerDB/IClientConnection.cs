using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDB
{
	public interface IClientConnection
	{
		DatabaseClent DatabaseClent { get; set; }
		void SendData(string data);

		event Action OnLostConnection;
	}
}
