using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace RhuDBShared
{
	public static class BDElementExtension
	{
		public static T UpdateCheck<T>(this T element) where T : DBElement {
			return !element.Updated ? element : element.DBRoot.GetElement<T>(element.SnowFlake);
		}
	}
	public class DBElement: DBObject
	{
		
		public ulong LastEditEpoch { get; set; }

		public ulong SnowFlake { get; set; }

		[JsonIgnore]
		public DateTime CreationDate => SnowFlake.GetTimeOfCreation();
		[JsonIgnore]
		public bool Updated { get; set; }

		public string GetNetworkedData() {
			return DBRoot.Serialize(this);
		}

		public unsafe override void Initialize(DBRoot dBRoot) {
			base.Initialize(dBRoot);
			foreach (var member in GetType().GetFields()) {
				if (typeof(IDBRef).IsAssignableFrom(member.FieldType)) {
					((DBObject)member.GetValue(this))?.Initialize(DBRoot);
				}
			}
		}
		public void UpdateData() {
			LastEditEpoch = SnowFlakeManager.GetCurrentEpoch();
			DBRoot.ElementUpdated(this);
		}

		public void BuildRefs() {
			foreach (var member in GetType().GetFields()) {
				if (typeof(IDBRef).IsAssignableFrom(member.FieldType)) {
					var newData = (DBObject)Activator.CreateInstance(member.FieldType);
					newData.Initialize(DBRoot);
					member.SetValue(this, newData);
				}
			}
		}
	}
}
