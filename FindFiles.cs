using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FindDupes
{
    public static void Find()
    {
        string rootDirectory = @"C:\Your\Directory\Path";
        var duplicates = FindDuplicateFiles(rootDirectory);

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

    static List<List<string>> FindDuplicateFiles(string rootDirectory)
    {
        var files = Directory.GetFiles(rootDirectory, "*.*", SearchOption.AllDirectories);
        var fileGroups = files.GroupBy(f => new { Size = new FileInfo(f).Length, Name = Path.GetFileName(f), Extension = Path.GetExtension(f) })
                              .Where(g => g.Count() > 1)
                              .Select(g => g.ToList())
                              .ToList();

        return fileGroups;
    }
}

