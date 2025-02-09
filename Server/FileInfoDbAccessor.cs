// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FileInfoDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class FileInfoDbAccessor
  {
    private const string FileInfoTable = "FileInfo�";
    private static readonly DbTableInfo InfoTable = DbAccessManager.GetTable("FileInfo");

    public static DataRowCollection GetFileInfo(string[] filePaths, int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from FileInfo");
      dbQueryBuilder.AppendLine("where FilePath in (" + SQL.Encode((object) filePaths) + ") and FileType =" + SQL.Encode((object) fileType));
      return dbQueryBuilder.Execute();
    }

    public static DataRowCollection GetFileInfoByFileIds(int[] fileIds, int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from FileInfo");
      dbQueryBuilder.AppendLine("where FileID in (" + SQL.Encode((object) fileIds) + ") and FileType =" + SQL.Encode((object) fileType));
      return dbQueryBuilder.Execute();
    }

    public static void InsertFileInfo(
      long fileSize,
      string checkSum,
      string filePath,
      int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList values = new DbValueList()
      {
        {
          "FilePath",
          (object) filePath
        },
        {
          "FileType",
          (object) fileType
        },
        {
          "FileChecksum",
          (object) checkSum
        },
        {
          "FileSize",
          (object) fileSize
        }
      };
      dbQueryBuilder.AppendLine("if not exists (select * from [FileInfo] where FilePath =" + SQL.Encode((object) filePath) + "and FileType =" + SQL.Encode((object) fileType) + ")");
      dbQueryBuilder.InsertInto(FileInfoDbAccessor.InfoTable, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateFileInfo(
      long fileSize,
      string checkSum,
      string filePath,
      int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList values = new DbValueList()
      {
        {
          "FileChecksum",
          (object) checkSum
        },
        {
          "FileSize",
          (object) fileSize
        }
      };
      dbQueryBuilder.Update(FileInfoDbAccessor.InfoTable, values, new DbValueList()
      {
        {
          "FilePath",
          (object) filePath
        },
        {
          "FileType",
          (object) fileType
        }
      });
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteFileInfo(string filePath, int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(FileInfoDbAccessor.InfoTable, new DbValueList()
      {
        {
          "FilePath",
          (object) filePath
        },
        {
          "FileType",
          (object) fileType
        }
      });
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ChangeFilePath(
      long fileSize,
      string checkSum,
      string sourcePath,
      string targetPath,
      int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList values = new DbValueList()
      {
        {
          "FilePath",
          (object) targetPath
        },
        {
          "FileType",
          (object) fileType
        },
        {
          "FileChecksum",
          (object) checkSum
        },
        {
          "FileSize",
          (object) fileSize
        }
      };
      dbQueryBuilder.DeleteFrom(FileInfoDbAccessor.InfoTable, new DbValueList()
      {
        {
          "FilePath",
          (object) sourcePath
        },
        {
          "FileType",
          (object) fileType
        }
      });
      dbQueryBuilder.InsertInto(FileInfoDbAccessor.InfoTable, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void CopyFile(
      long fileSize,
      string checkSum,
      string sourcePath,
      string targetPath,
      int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList values1 = new DbValueList()
      {
        {
          "FilePath",
          (object) sourcePath
        },
        {
          "FileType",
          (object) fileType
        },
        {
          "FileChecksum",
          (object) checkSum
        },
        {
          "FileSize",
          (object) fileSize
        }
      };
      DbValueList values2 = new DbValueList()
      {
        {
          "FilePath",
          (object) targetPath
        },
        {
          "FileType",
          (object) fileType
        },
        {
          "FileChecksum",
          (object) checkSum
        },
        {
          "FileSize",
          (object) fileSize
        }
      };
      dbQueryBuilder.Declare("@FileChecksum", "varchar(255)");
      dbQueryBuilder.Declare("@FileSize", "bigint");
      dbQueryBuilder.AppendLine("select @FileChecksum = FileChecksum, @FileSize = FileSize from [FileInfo] where FilePath = " + SQL.Encode((object) sourcePath) + " and FileType = " + SQL.Encode((object) fileType));
      dbQueryBuilder.AppendLine("if (@FileChecksum is NOT NULL) and (@FileSize is NOT NULL)");
      dbQueryBuilder.AppendLine("insert into [FileInfo] values (" + SQL.Encode((object) targetPath) + ", " + SQL.Encode((object) fileType) + ", @FileChecksum, @FileSize)");
      dbQueryBuilder.AppendLine("else");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.InsertInto(FileInfoDbAccessor.InfoTable, values1, true, false);
      dbQueryBuilder.InsertInto(FileInfoDbAccessor.InfoTable, values2, true, false);
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
