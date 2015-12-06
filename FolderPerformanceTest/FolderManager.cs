using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderPerformanceTest
{
	public static class Extensions
	{
		public static string Last(this string source, int tailLength)
		{
			if (tailLength >= source.Length)
			{
				return source;
			}
			return source.Substring(source.Length - tailLength);
		}
	}

	public sealed class FolderManager
	{
		public List<Guid> FolderGuids { get; set; }
		private static volatile FolderManager instance;
		private static readonly object syncRoot = new object();

		public readonly string LocalDirectory = @"C:\Users\ignac_000\Documents\visual studio 2015\Projects\FolderPerformanceTest\Folders\";
		public readonly string bg = "bg.png";
		public readonly string logo = "logo.png";
		private readonly string original = "Original";

		private FolderManager()
		{
		}

		public static FolderManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new FolderManager();
					}
				}

				return instance;
			}
		}

		public void LoadGuids()
		{
			var dirs = Directory.GetDirectories(LocalDirectory).ToList();
			dirs.Remove(LocalDirectory + original);
            this.FolderGuids = dirs.Select(x=>new Guid(x.Last(32+4))).ToList();
		}

		public void CopyDirectory(int copies)
		{
			for (var i = 0; i < copies; i++)
			{
				DirectoryCopy(LocalDirectory + original, LocalDirectory + Guid.NewGuid(), true);
			}
		}

		private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();

			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		public void CreateFolders(int count)
		{
			var guids = new List<Guid>();
			for (var i = 0; i < count; i++)
			{
				var guid = Guid.NewGuid();
				guids.Add(guid);
				var path = LocalDirectory + guid;
				Directory.CreateDirectory(path);
			}
			FolderGuids = guids;
			LoadGuids();
		}
	}
}