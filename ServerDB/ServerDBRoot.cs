using System;
using System.Collections.Generic;
using System.Text;

using RhuDBShared;

namespace ServerDB
{
	public class ServerDBRoot : DBObject
	{
		public void BuildSets() {
			foreach (var member in GetType().GetFields()) {
				if (!member.IsInitOnly) {
					continue;
				}
				if (typeof(IDBSet).IsAssignableFrom(member.FieldType)) { 
					var newData = (DBObject)Activator.CreateInstance(member.FieldType);
					newData.Initialize(DBRoot);
					member.SetValue(this, newData);
				}
			}
		}
	}
}
