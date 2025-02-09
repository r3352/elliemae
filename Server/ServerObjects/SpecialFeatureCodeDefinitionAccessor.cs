// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SpecialFeatureCodeDefinitionAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class SpecialFeatureCodeDefinitionAccessor
  {
    protected internal const string DbTableName = "SpecialFeatureCodes�";
    protected internal static readonly string[] DbColumnNames = new string[5]
    {
      "ID",
      "Code",
      "Description",
      "Comment",
      "Source"
    };
    protected internal static readonly string DbColumnNamesCsv = string.Join(", ", ((IEnumerable<string>) SpecialFeatureCodeDefinitionAccessor.DbColumnNames).Select<string, string>((System.Func<string, string>) (c => "[" + c + "]")));
    protected internal static readonly string DbParameterNames = string.Join(", ", ((IEnumerable<string>) SpecialFeatureCodeDefinitionAccessor.DbColumnNames).Select<string, string>((System.Func<string, string>) (c => "@" + c.ToLowerInvariant())));
    protected internal static readonly string DbColumnAssignments = string.Join(", ", ((IEnumerable<string>) SpecialFeatureCodeDefinitionAccessor.DbColumnNames).Select<string, string>((System.Func<string, string>) (c => c + " = @" + c.ToLowerInvariant())));

    public bool Create(SpecialFeatureCodeDefinition code)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("INSERT INTO [SpecialFeatureCodes] (" + SpecialFeatureCodeDefinitionAccessor.DbColumnNamesCsv + ", [Status]) VALUES (" + SpecialFeatureCodeDefinitionAccessor.DbParameterNames + ", 0)");
      DbCommandParameter[] commandParameters = this.GetCommandParameters(code);
      return 1 == (int) dbQueryBuilder.ExecuteNonQueryWithRowCount(commandParameters);
    }

    public bool SetStatus(
      SpecialFeatureCodeDefinition code,
      SpecialFeatureCodeDefinitionStatus status = SpecialFeatureCodeDefinitionStatus.Active)
    {
      int num1 = 0;
      int num2 = 1;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (SpecialFeatureCodeDefinitionStatus.Active == status)
      {
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET [Status] = CASE [ID] WHEN @id THEN {1} ELSE {2} END", (object) "SpecialFeatureCodes", (object) num2, (object) num1));
        dbQueryBuilder.AppendLine("WHERE [Code] = @code AND [Source] = @source;");
        dbQueryBuilder.AppendLine(string.Format("SELECT 1 FROM [{0}] WHERE [ID] = @id AND [Status] = {1};", (object) "SpecialFeatureCodes", (object) num2));
      }
      else
      {
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET [Status] = {1} WHERE [ID] = @id", (object) "SpecialFeatureCodes", (object) num1));
        dbQueryBuilder.AppendLine(string.Format("SELECT 1 FROM [{0}] WHERE [ID] = @id AND [Status] = {1};", (object) "SpecialFeatureCodes", (object) num1));
      }
      DbCommandParameter[] commandParameters = this.GetCommandParameters(code);
      bool flag = dbQueryBuilder.ExecuteRowQuery(commandParameters) != null;
      if (flag)
        code.Status = status;
      return flag;
    }

    public IList<SpecialFeatureCodeDefinition> GetAll(bool activeOnly = false)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("SELECT " + SpecialFeatureCodeDefinitionAccessor.DbColumnNamesCsv + ", [Status] FROM [SpecialFeatureCodes]");
      if (activeOnly)
        dbQueryBuilder.Append(" WHERE [STATUS] = 1");
      return (IList<SpecialFeatureCodeDefinition>) dbQueryBuilder.Execute().OfType<DataRow>().Select<DataRow, SpecialFeatureCodeDefinition>(new System.Func<DataRow, SpecialFeatureCodeDefinition>(this.ParseDataRow)).ToList<SpecialFeatureCodeDefinition>();
    }

    public bool Update(SpecialFeatureCodeDefinition code)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("UPDATE [SpecialFeatureCodes] SET " + SpecialFeatureCodeDefinitionAccessor.DbColumnAssignments + " WHERE [ID] = @id");
      DbCommandParameter[] commandParameters = this.GetCommandParameters(code);
      return 1 == (int) dbQueryBuilder.ExecuteNonQueryWithRowCount(commandParameters);
    }

    public bool Delete(SpecialFeatureCodeDefinition code)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("DELETE FROM [SpecialFeatureCodes] WHERE [ID] = @id");
      DbCommandParameter[] parameters = new DbCommandParameter[1]
      {
        this.GetPkCommandParameter(code.ID)
      };
      return 1 == (int) dbQueryBuilder.ExecuteNonQueryWithRowCount(parameters);
    }

    protected internal DbCommandParameter GetPkCommandParameter(string id)
    {
      return new DbCommandParameter(nameof (id), (object) id, DbType.String);
    }

    protected internal DbCommandParameter[] GetCommandParameters(SpecialFeatureCodeDefinition code)
    {
      return new DbCommandParameter[5]
      {
        this.GetPkCommandParameter(code.ID),
        new DbCommandParameter(nameof (code), (object) code.Code, DbType.String),
        new DbCommandParameter("description", (object) code.Description, DbType.String),
        new DbCommandParameter("comment", (object) code.Comment, DbType.String),
        new DbCommandParameter("source", (object) code.Source, DbType.String)
      };
    }

    protected internal SpecialFeatureCodeDefinition ParseDataRow(DataRow row)
    {
      string column1 = row.GetColumn<string>("ID");
      string column2 = row.GetColumn<string>("Code");
      string column3 = row.GetColumn<string>("Source");
      string column4 = row.GetColumn<string>("Description");
      string column5 = row.GetColumn<string>("Comment");
      SpecialFeatureCodeDefinitionStatus result;
      Enum.TryParse<SpecialFeatureCodeDefinitionStatus>(row["Status"].ToString(), out result);
      return new SpecialFeatureCodeDefinition(column2, column3, column4, column5)
      {
        ID = column1,
        Status = result
      };
    }

    public class UnitTest
    {
      private static System.Func<SpecialFeatureCodeDefinition, bool> Compare(
        SpecialFeatureCodeDefinition code)
      {
        return (System.Func<SpecialFeatureCodeDefinition, bool>) (d => d.ID == code.ID);
      }

      public static void Run()
      {
        bool flag = false;
        SpecialFeatureCodeDefinition code1 = new SpecialFeatureCodeDefinition("__TEST__", "FannieMae", "This is a test record that should be immediately deleted", "About to create");
        SpecialFeatureCodeDefinitionAccessor definitionAccessor = new SpecialFeatureCodeDefinitionAccessor();
        IList<SpecialFeatureCodeDefinition> all = definitionAccessor.GetAll();
        flag = definitionAccessor.Create(code1);
        all = definitionAccessor.GetAll();
        code1.Comment = "About to update";
        flag = definitionAccessor.Update(code1);
        all = definitionAccessor.GetAll();
        flag = definitionAccessor.SetStatus(code1);
        all = definitionAccessor.GetAll();
        SpecialFeatureCodeDefinition code2 = code1.Clone();
        code2.Comment = "Cloned definition";
        flag = definitionAccessor.Create(code2);
        flag = definitionAccessor.SetStatus(code2);
        all = definitionAccessor.GetAll();
        flag = definitionAccessor.SetStatus(code2, SpecialFeatureCodeDefinitionStatus.None);
        all = definitionAccessor.GetAll();
        code1.Comment = "About to delete";
        flag = definitionAccessor.Delete(code1);
        all = definitionAccessor.GetAll();
        flag = definitionAccessor.Delete(code2);
      }
    }
  }
}
