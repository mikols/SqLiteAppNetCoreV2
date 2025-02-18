using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class FindDupes
{
    public static void Find(string path)
    {
        string rootDirectory = path;
        var duplicates = _FindDuplicateFiles(rootDirectory);

        foreach (var duplicate in duplicates)
        {
            Console.WriteLine($"Duplicate files found:");
            foreach (var file in duplicate)
            {
                Console.WriteLine(file);
            }
            Console.WriteLine();
        }
    }

    private static List<List<string>> _FindDuplicateFiles(string rootDirectory)
    {
        Console.WriteLine($"Search Directory: " + rootDirectory);

        var files = Directory.GetFiles(rootDirectory, "*.mp4", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
        var fileGroups = files.GroupBy(f => new { Size = new FileInfo(f).Length, Name = Path.GetFileName(f), Extension = Path.GetExtension(f) })
                              .Where(g => g.Count() > 1)
                              .Select(g => g.ToList())
                              .ToList();

        return fileGroups;
    }

    public static void FindDuplicateFiles(string rootPath)
    {
        Console.WriteLine($"Rootpath: {rootPath}");
        var allowedExtensions = new HashSet<string> { ".dll", ".exe", ".mp4" };

        // string rootPath = @"C:\Your\Directory\Path";
        var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()));

        // foreach (var file in files)
        // {
        //     Console.WriteLine(file);
        // }

        var fileGroups = files
            .Select(file => new FileInfo(file))
            .GroupBy(file => file.Length)
            .Where(group => group.Count() > 1);

        foreach (var group in fileGroups)
        {
            Console.WriteLine($"Filesize: {group.Key} bytes");
            foreach (var file in group)
            {
                Console.WriteLine(file.FullName);
            }
            Console.WriteLine();
        }
    }

    public static void ScanForDuplicateFiles(string rootPath)
    {
        var allowedExtensions = new HashSet<string> { ".mp4", ".dll" };
        var fileGroups = new ConcurrentDictionary<long, List<FileInfo>>();

        var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()));

        Parallel.ForEach(files, file =>
        {
            var fileInfo = new FileInfo(file);
            fileGroups.AddOrUpdate(fileInfo.Length,
                new List<FileInfo> { fileInfo },
                (key, existingList) =>
                {
                    existingList.Add(fileInfo);
                    return existingList;
                });
        });

        var duplicateFiles = new List<FileDetail>();

        foreach (var group in fileGroups.Where(g => g.Value.Count > 1))
        {
            foreach (var file in group.Value)
            {
                duplicateFiles.Add(new FileDetail
                {
                    FileSize = group.Key,
                    FilePath = file.FullName,
                    FileName = file.Name
                });
            }
        }

        // FILELISTVIEW IN XAML
        // ---- >  FileListView.ItemsSource = duplicateFiles;
        foreach (var f in duplicateFiles)
        {
            Console.WriteLine($"FileName: {f.FileName} Filesize: {f.FileSize} - Filepath: {f.FilePath}");    
        }
    }


/*

    static void Main()
    {
        string rootPath = @"C:\Your\Directory\Path";
        var allowedExtensions = new HashSet<string> { ".txt", ".doc" };
        var fileGroups = new ConcurrentDictionary<long, List<FileInfo>>();

        var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()));

        Parallel.ForEach(files, file =>
        {
            var fileInfo = new FileInfo(file);
            fileGroups.AddOrUpdate(fileInfo.Length,
                new List<FileInfo> { fileInfo },
                (key, existingList) =>
                {
                    existingList.Add(fileInfo);
                    return existingList;
                });
        });

        foreach (var group in fileGroups.Where(g => g.Value.Count > 1))
        {
            Console.WriteLine($"Filesize: {group.Key} bytes");
            foreach (var file in group.Value)
            {
                Console.WriteLine(file.FullName);
            }
            Console.WriteLine();
        }
    }


*/


}



