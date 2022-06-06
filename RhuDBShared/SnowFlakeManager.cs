using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RhuDBShared
{
	public static class SnowFlakeManager
	{
		public static DateTime GetTimeOfCreation(this ulong snowFlake) {
			var epoch = snowFlake >> 22;
			return START_EPOCH.AddMilliseconds(epoch);
		}
		public static ulong GetEpoch(this ulong snowFlake) {
			var epoch = snowFlake >> 22;
			return epoch;
		}


		public static DateTime START_EPOCH = new DateTime(2020, 1, 1);

		public static ulong GetCurrentEpoch() {
			var t = DateTime.UtcNow - START_EPOCH;
			var millisecondsSinceEpoch = (ulong)t.TotalMilliseconds;
			return millisecondsSinceEpoch;
		}

		[ThreadStatic]
		public static uint CurrentID = 0;
		[ThreadStatic]
		public static byte ThreadID = 0;
		public static Semaphore semaphore = new Semaphore(1, 1);
		public static byte CurrentThreadID = 0;
		public static ulong BuildSnowFlake(byte serverId){
			var incrument = CurrentID;
			CurrentID++;
			if(CurrentID == 0xFFFul) {
				CurrentID = 0;
			}
			if(ThreadID == 0) {
				semaphore.WaitOne();
				CurrentThreadID++;
				if (CurrentThreadID == 0xFF) {
					CurrentThreadID = 0;
				}
				ThreadID = CurrentThreadID;
				semaphore.Release();
			}
			var serverIdByte = (serverId & 0xFul) << 18;
			var ThreadId = (ThreadID & 0xFFul) << 12;	
			var offset = GetCurrentEpoch() << 22;
			return offset | serverIdByte | ThreadId | incrument;
		}
	}
}
