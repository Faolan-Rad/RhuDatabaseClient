using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhuDBShared
{
    public class DBElement: DBObject
	{
		public ulong LastEditEpoch { get; set; }

		public ulong SnowFlake { get; set; }

		public DateTime CreationDate => SnowFlake.GetTimeOfCreation();

		public unsafe override void Initialize(DBRoot dBRoot) {
			base.Initialize(dBRoot);
			foreach (var item in GetType().GetProperties()) {
				
			}
		}

		public void BuildRefs() {
			foreach (var member in GetType().GetFields()) {
				if (!member.IsInitOnly) {
					continue;
				}
				if (typeof(IDBRef).IsAssignableFrom(member.FieldType)) {
					var newData = (DBObject)Activator.CreateInstance(member.FieldType);
					newData.Initialize(DBRoot);
					member.SetValue(this, newData);
				}
			}
		}
	}
}
