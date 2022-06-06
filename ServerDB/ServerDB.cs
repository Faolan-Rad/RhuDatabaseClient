using System;
using System.Collections.Generic;

using RhuDBShared;

namespace ServerDB
{
	public abstract class ServerDB : DBRoot
	{
		public byte ServerID;
		public override T GetElement<T>(ulong snowFlake) {
			try {
				return (T)Elements[snowFlake];
			}
			catch {
				return default;
			}
		}
		public abstract ServerDBRoot RootData { get; }

		public void AddNewElement(DBElement element) {
			element.BuildRefs();
			Elements.Add(element.SnowFlake,element);
		}
	}

	public class ServerDB<T>:ServerDB where T : ServerDBRoot, new()
	{
		public T ServerData { get; private set; }

		public override ServerDBRoot RootData => ServerData;

		public void Initialize()  {
			Elements = new Dictionary<ulong, DBElement>();
			ServerData = new T();
			RootData.Initialize(this);
			RootData.BuildSets();
		}
	}
}
