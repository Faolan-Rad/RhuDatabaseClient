using System;
using System.Collections.Generic;
using System.Text;

using RhuDBShared;

namespace ServerDB
{
	public interface IDBSet {

	}
	public class DBSet<T>:DBObject, IDBSet where T : DBElement
	{
		public List<T> Elements;

		public T AddNewElement(T element) {
			element.Initialize(DBRoot);
			element.SnowFlake = SnowFlakeManager.BuildSnowFlake(((ServerDB)DBRoot).ServerID);
			element.LastEditEpoch = element.SnowFlake.GetEpoch();
			((ServerDB)DBRoot).AddNewElement(element);
			return element;
		}
	}
}
