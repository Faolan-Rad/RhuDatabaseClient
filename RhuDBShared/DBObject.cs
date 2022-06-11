using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace RhuDBShared
{
    public class DBObject
	{
		[JsonIgnore]
		public DBRoot DBRoot { get; private set; }
		public virtual void Initialize(DBRoot dBRoot) {
			DBRoot = dBRoot;
		}
    }
}
