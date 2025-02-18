<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <DataGrid Name="dataGrid" AutoGenerateColumns="False" MouseRightButtonUp="DataGrid_MouseRightButtonUp">
            <DataGrid.Columns>
                <DataGridTextColumn Header="File Path" Binding="{Binding FilePath}" />
            </DataGrid.Columns>
        </DataGrid>
        <ContextMenu x:Key="DataGridContextMenu">
            <MenuItem Header="Delete File" Click="DeleteFile_Click" />
        </ContextMenu>
    </Grid>
</Window>

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Assuming you have a collection of file paths to bind to the DataGrid
            dataGrid.ItemsSource = GetFilePaths();
        }

        private void DataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                ContextMenu contextMenu = this.FindResource("DataGridContextMenu") as ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = dataGrid;
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var filePath = (dataGrid.SelectedItem as YourDataType).FilePath;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    MessageBox.Show("File deleted successfully.");
                    // Refresh the DataGrid or remove the item from the source collection
                }
                else
                {
                    MessageBox.Show("File not found.");
                }
            }
        }

        private IEnumerable<YourDataType> GetFilePaths()
        {
            // Replace with your actual data retrieval logic
            return new List<YourDataType>
            {
                new YourDataType { FilePath = @"C:\path\to\file1.txt" },
                new YourDataType { FilePath = @"C:\path\to\file2.txt" }
            };
        }
    }

    public class YourDataType
    {
        public string FilePath { get; set; }
    }
}


----

Ver 2:

-----

<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <DataGrid Name="dataGrid" AutoGenerateColumns="False" MouseRightButtonUp="DataGrid_MouseRightButtonUp">
            <DataGrid.Columns>
                <DataGridTextColumn Header="File Path" Binding="{Binding FilePath}" />
            </DataGrid.Columns>
        </DataGrid>
        <ContextMenu x:Key="DataGridContextMenu">
            <MenuItem Header="Delete File" Click="DeleteFile_Click" />
        </ContextMenu>
    </Grid>
</Window>
---
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        private List<YourDataType> filePaths;

        public MainWindow()
        {
            InitializeComponent();
            filePaths = GetFilePaths().ToList();
            dataGrid.ItemsSource = filePaths;
        }

        private void DataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                ContextMenu contextMenu = this.FindResource("DataGridContextMenu") as ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = dataGrid;
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var selectedItem = dataGrid.SelectedItem as YourDataType;
                var filePath = selectedItem.FilePath;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the file: {filePath}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        MessageBox.Show("File deleted successfully.");
                        filePaths.Remove(selectedItem);
                        dataGrid.ItemsSource = null;
                        dataGrid.ItemsSource = filePaths;
                    }
                    else
                    {
                        MessageBox.Show("File not found.");
                    }
                }
            }
        }

        private IEnumerable<YourDataType> GetFilePaths()
        {
            // Replace with your actual data retrieval logic
            return new List<YourDataType>
            {
                new YourDataType { FilePath = @"C:\path\to\file1.txt" },
                new YourDataType { FilePath = @"C:\path\to\file2.txt" }
            };
        }
    }

    public class YourDataType
    {
        public string FilePath { get; set; }
    }
}

