using System;

using RhuDBShared;

namespace TestModel
{
    public class User: DBElement
    {
		public string UserName { get; set; }
		public string ProfileUrl { get; set; }
    }

	public class World : DBElement
	{
		public string WorldName { get; set; }

		public readonly DBRef<User> Owner;

	}
}
