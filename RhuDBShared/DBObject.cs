using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhuDBShared
{
    public class DBObject
	{
		public DBRoot DBRoot { get; private set; }
		public virtual void Initialize(DBRoot dBRoot) {
			DBRoot = dBRoot;
		}
    }
}
