using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderPerformanceTest
{
	class Program
	{
		public static List<T> GetRandom<T>(List<T> list, int count)
		{
			var r = new Random();
			var result = new List<T>();
			for (int i = 0; i < count; i++)
			{
				result.Add(list[r.Next(0, list.Count-1)]);
			}
			return result;
		} 

		static void Main(string[] args)
		{
			var t = new Stopwatch();
			
			FolderManager.Instance.LoadGuids();
			var rs = GetRandom(FolderManager.Instance.FolderGuids, 300);
			var gs = FolderManager.Instance.FolderGuids.Where(x=>rs.Contains(x)).ToList();
            var files = new List<string>();
			t.Start();
			foreach (var guid in gs)
			{
				File.Open(FolderManager.Instance.LocalDirectory + guid + "\\logo.png", FileMode.Open);
			}
			t.Stop();
			Console.WriteLine(t.ElapsedMilliseconds);
			Console.ReadKey();
		}
	}
}
