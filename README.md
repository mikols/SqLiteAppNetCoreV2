# SqLiteAppNetCoreV2

To display the results in a WPF ListView, you can bind the data to the ListView control. Here’s how you can modify the previous code to show the duplicate files in a WPF ListView:

Create a WPF Application: Start by creating a new WPF application in Visual Studio.
Define the ListView in XAML:
Add a ListView control to your MainWindow.xaml and define the columns using GridView.
XML

<Window x:Class="DuplicateFileFinder.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Duplicate File Finder" Height="450" Width="800">
    <Grid>
        <ListView Name="FileListView" Margin="10">
            <ListView.View>
                <GridView>
                    <GridViewColumn Header="Filesize" DisplayMemberBinding="{Binding FileSize}" Width="100"/>
                    <GridViewColumn Header="File Path" DisplayMemberBinding="{Binding FilePath}" Width="600"/>
                </GridView>
            </ListView.View>
        </ListView>
    </Grid>
</Window>


------------------


AI-generated code. Review and use carefully. More info on FAQ.
Create a Data Model:
Define a class to represent the file information.


------------------


C#

public class FileDetail
{
    public long FileSize { get; set; }
    public string FilePath { get; set; }
}

------------------

AI-generated code. Review and use carefully. More info on FAQ.
Update the Code-Behind:
Modify the MainWindow.xaml.cs to scan for files and bind the results to the ListView.
C#

------------------


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DuplicateFileFinder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ScanForDuplicateFiles();
        }

        private void ScanForDuplicateFiles()
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

            var duplicateFiles = new List<FileDetail>();

            foreach (var group in fileGroups.Where(g => g.Value.Count > 1))
            {
                foreach (var file in group.Value)
                {
                    duplicateFiles.Add(new FileDetail
                    {
                        FileSize = group.Key,
                        FilePath = file.FullName
                    });
                }
            }

            FileListView.ItemsSource = duplicateFiles;
        }
    }
}

Explanation:
XAML Definition: The ListView is defined with two columns: Filesize and File Path.
Data Model: The FileDetail class represents the file information.
Code-Behind: The ScanForDuplicateFiles method scans for duplicate files and binds the results to the ListView.
This setup will display the duplicate files in a WPF ListView, showing their size and full path. 


