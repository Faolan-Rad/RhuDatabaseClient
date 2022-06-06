using Microsoft.VisualStudio.TestTools.UnitTesting;

using RhuDBShared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhuDBShared.Tests
{
	[TestClass()]
	public class SnowFlakeManagerTests
	{

		[TestMethod()]
		public void ThreadedSnowFlakeTest() {
			var amountOfValues = 4000;
			var answers = new ulong[amountOfValues];
			Parallel.For(0, amountOfValues, (value) => answers[value] = SnowFlakeManager.BuildSnowFlake(10));
			var hashSet = new HashSet<ulong>();
			for (var i = 0; i < answers.Length; i++) {
				if (!hashSet.Add(answers[i])) {
					var count = hashSet.Count;
					throw new Exception($"Value Same {count}");
				}
			}
		}
		[TestMethod()]
		public void SingleSnowFlakeTest() {
			var amountOfValues = 4000;
			var answers = new ulong[amountOfValues];
			for (var i = 0; i < amountOfValues; i++) {
				answers[i] = SnowFlakeManager.BuildSnowFlake(10);
			}
			var hashSet = new HashSet<ulong>();
			for (var i = 0; i < answers.Length; i++) {
				if (!hashSet.Add(answers[i])) {
					var count = hashSet.Count;
					throw new Exception($"Value Same Value:{answers[i]} Count:{count}");
				}
			}
		}

		[TestMethod()]
		public void CreationTimeTest() {
			var currentDateTime = DateTime.UtcNow;
			var test = SnowFlakeManager.BuildSnowFlake(10);
			Assert.AreEqual(currentDateTime.ToLongDateString(), test.GetTimeOfCreation().ToLongDateString());
		}
	}
}