
using System.ComponentModel;


namespace SqLiteAppNetCoreV2
{
    public enum MenuChoices
    {        
        [Description("Create Database")]
        CreateSqliteDatabase,
        
        [Description("Insert Title Test Data")]
        InsertTitleData,

        [Description("Delete All Title Data")]
        DeleteTitleData,

        [Description("Add Directories - Enter file paths (type 'done' to finish):")]
        AddDirectories,

        [Description("Find Directories - and Insert")]
        FindDirectories,
        
        [Description("Read titledata")]
        ReadTitles,
        
        [Description("Find titledata")]
        FindTitles,
        
        [Description("Parse Date")]
        ParseDate,

        [Description("Exit")]
        Exit,

        [Description("Unknown")]
        Unknown

    }

}