
using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;


namespace SqLiteAppNetCoreV2
{

    public class SqliteDB
    {
        public static string sqliteDBFile ="./mySqliteDbTest.db";


        public static void CreateSqliteDbTable()
        {
           // Create Connection
            Console.WriteLine($"Create Connection to {sqliteDBFile}" );
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                // Create Table
                Console.WriteLine("Create Table");
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Actor(
                        Name TEXT NOT NULL, 
                        Size Long)
                        ";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Title(
                        Name TEXT NOT NULL, 
                        Parent TEXT NOT NULL, 
                        Size Long)
                        ";
                tableCmd.ExecuteNonQuery();
            }
        }

        public static void TestInsertDataIntoSqliteDbTable()
        {
           // Create Connection
             Console.WriteLine($"Create Connection to {sqliteDBFile}" );
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                 // Insert Some Records
                using (var transaction = connection.BeginTransaction())
                {
                    Console.WriteLine("Insert Some Records");
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO Actor Values ( 'Nisse', 100)";
                    insertCmd.ExecuteNonQuery();
                    insertCmd.CommandText = "INSERT INTO Title Values ('Name', 'Parent', 20)";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public static void InsertRowActor(string name, long size)
        {
           // Create Connection
            Console.WriteLine("Create Connection");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    Console.WriteLine($"INSERT INTO Actor Values ( '{name}', {size})");
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = $"INSERT INTO Actor Values ( '{name}', {size})";
                    insertCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public static void InsertRowTitle(List<TitleDataModel> dataList)
        {
           // Create Connection
            Console.WriteLine("Create Connection");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO Title (Name, Parent, Size) VALUES ($valueName, $valueParent, $valueSize)";
                        
                    var paramName = insertCmd.CreateParameter();
                    paramName.ParameterName = "$valueName";
                    insertCmd.Parameters.Add(paramName);

                    var paramParent = insertCmd.CreateParameter();
                    paramParent.ParameterName = "$valueParent";
                    insertCmd.Parameters.Add(paramParent);

                    var paramSize = insertCmd.CreateParameter();
                    paramSize.ParameterName = "$valueSize";
                    insertCmd.Parameters.Add(paramSize);

                    foreach (var data in dataList)
                    {
                        paramName.Value = data.Name;
                        paramParent.Value = data.Parent;
                        paramSize.Value = data.Size;
                        insertCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }


        public static void ReadDataFromSqliteDbTable()
        {

            Console.WriteLine("ReadDataIntoSqliteDbTable");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                // Read Records
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT * FROM Title";
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = reader.GetString(1);
                        Console.WriteLine(result);
                    }
                }
                Console.WriteLine("Read Records");
            }
        }

        public static void DeleteTitleTableData()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = sqliteDBFile;

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Title";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} rows deleted.");
                }
            }
        }

         public static void FindSubstring(string substring)
         {
             var connectionStringBuilder = new SqliteConnectionStringBuilder();
             connectionStringBuilder.DataSource = sqliteDBFile;

             using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
             {
                 connection.Open();

                 string query = "SELECT * FROM TITLE WHERE Name LIKE @substring";

                 using (var command = new SqliteCommand(query, connection))
                 {
                     command.Parameters.AddWithValue("@substring", "%" + substring + "%");

                     using (var reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             // Process each row
                             Console.WriteLine(reader["Name"]);
                         }
                     }
                 }
            }
        }

    }

}