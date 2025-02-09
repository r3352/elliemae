// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CustomFormDetailsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class CustomFormDetailsAccessor
  {
    public static CustomFormDetail[] GetCustomFormDetails()
    {
      List<CustomFormDetail> customFormDetailList = new List<CustomFormDetail>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Source, IntendedForBorrowerTypeID FROM CustomFormDetails");
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteTableQuery().Rows)
      {
        ForBorrowerType intendedFor = (ForBorrowerType) row["IntendedForBorrowerTypeID"];
        customFormDetailList.Add(new CustomFormDetail(row["Source"].ToString(), intendedFor));
      }
      return customFormDetailList.ToArray();
    }

    public static void SaveCustomFormDetail(CustomFormDetail formDetail)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = formDetail.Source.Replace("'", "''");
      dbQueryBuilder.AppendLine("exec CustomFormDetail_Save '" + str + "', " + (object) (int) formDetail.IntendedFor + ";");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void RenameSource(string currentSource, string newSource)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      currentSource = currentSource.Replace("'", "''");
      newSource = newSource.Replace("'", "''");
      dbQueryBuilder.AppendLine("UPDATE CustomFormDetails SET source = '" + newSource + "' WHERE source = '" + currentSource + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void CopyCustomFormDetail(string currentSource, string targetSource)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      currentSource = currentSource.Replace("'", "''");
      targetSource = targetSource.Replace("'", "''");
      dbQueryBuilder.AppendLine("exec CustomFormDetail_Copy '" + currentSource + "', '" + targetSource + "';");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteCustomFormDetail(string source)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      source = source.Replace("'", "''");
      dbQueryBuilder.AppendLine("DELETE CustomFormDetails WHERE source = '" + source + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
