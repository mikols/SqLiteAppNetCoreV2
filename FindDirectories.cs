using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SqLiteAppNetCoreV2
{
    public class FindDirectories
    {
        public static void FindDirs(List<string> rootPaths)
        {
            foreach (var dir in rootPaths)
            {
                if (Directory.Exists(dir))
                    FindDir(dir);
                else
                    Console.WriteLine($"Cannot find directory: {dir}");

            }
        }

        public static void FindDir(string rootPath)
        {
            Console.WriteLine($"Selected Path: {rootPath}");

            // Get an enumerable collection of subdirectories
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);
            var subdirectories = rootDir.EnumerateDirectories("*", SearchOption.AllDirectories);
            List<string> textsToFind = new List<string> { "obj", "debug" };

            // Print the count and names of subdirectories
            Console.WriteLine($"Number of subdirectories: {subdirectories.Count()}");
            List<TitleDataModel> titleList = new List<TitleDataModel>();
            foreach (var directory in subdirectories)
            {
                // Console.WriteLine(directory);
                 if (!textsToFind.Any(text => directory.FullName.Contains(text, StringComparison.OrdinalIgnoreCase)))
                {
                    var dirSize = GetDirectorySize(directory);
                    Console.WriteLine(directory.FullName + ": (" + dirSize.ToString() + ")");
                    titleList.Add(new TitleDataModel { Name = directory.Name, Parent = directory.FullName, Size = dirSize });
                }
            }
            if (titleList.Count > 0)
                SqliteDB.InsertRowTitle(titleList);
        }

        public static long GetDirectorySize(DirectoryInfo dir)
        {
            // Get the size of all files in the directory
            long size = dir.EnumerateFiles().Sum(file => file.Length);

            // Recursively get the size of all subdirectories
            size += dir.EnumerateDirectories().Sum(subDir => GetDirectorySize(subDir));

            return size;
        }
    }
}    
