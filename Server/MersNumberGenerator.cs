// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MersNumberGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class MersNumberGenerator
  {
    private const string className = "MersNumberGenerator�";
    public static readonly int CompanyIdLength = 7;
    public static readonly int NextNumberLength = 10;

    private MersNumberGenerator()
    {
    }

    public static string FormatMersNumber(string mersNumber, string sep)
    {
      string str1 = mersNumber;
      int startIndex1 = mersNumber.Length - 5;
      string str2 = str1.Insert(startIndex1, sep);
      int startIndex2 = startIndex1 - 5;
      string str3 = str2.Insert(startIndex2, sep);
      int startIndex3 = startIndex2 - 4;
      return str3.Insert(startIndex3, sep).Insert(startIndex3 - 3, sep);
    }

    public static string FormatMersNumber(string mersNumber)
    {
      return MersNumberGenerator.FormatMersNumber(mersNumber, "-");
    }

    public static string GetNextMersNumber(OrgInfo orgInfo)
    {
      return MersNumberGenerator.GetNextMersNumber(false, orgInfo);
    }

    public static string GetNextMersNumber(bool alwaysGenerate, OrgInfo sourceOrg)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @autoGen char(1)");
        dbQueryBuilder.AppendLine("declare @valueStr varchar(" + (object) 10 + ")");
        dbQueryBuilder.AppendLine("declare @value decimal(" + (object) 11 + " ,0)");
        dbQueryBuilder.AppendLine("declare @nextValueStr varchar(" + (object) 10 + ")");
        dbQueryBuilder.AppendLine("declare @useBranchNumbering bit");
        dbQueryBuilder.AppendLine("declare @branchCode varchar(10)");
        dbQueryBuilder.AppendLine("select @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("select @branchCode = ''");
        if (sourceOrg != null && sourceOrg.MERSMINCode != "")
          dbQueryBuilder.AppendLine("select @useBranchNumbering = 1, @branchCode = mersmin_code from BranchMERSNumber with (rowlock, xlock) where (enabled = 1) and (next_number is not null) and (mersmin_code = " + SQL.Encode((object) sourceOrg.MERSMINCode) + ")");
        dbQueryBuilder.AppendLine("select @autoGen = AutoGeneration from MersNumber with (tablockx)");
        dbQueryBuilder.AppendLine("if @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("    select @valueStr = NextNumber from MersNumber with (tablockx)");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    select @valueStr = next_number from BranchMERSNumber with (rowlock, xlock) where mersmin_code = @branchCode");
        if (!alwaysGenerate)
        {
          dbQueryBuilder.AppendLine("if (@autoGen <> '0')");
          dbQueryBuilder.AppendLine("begin");
        }
        dbQueryBuilder.AppendLine("   select @value = convert(decimal(" + (object) 11 + ", 0), @valueStr)");
        dbQueryBuilder.AppendLine("   select @nextValueStr = substring(replace(str(@value + 1, " + (object) 11 + ", 0), ' ', '0'), 2, " + (object) 10 + ")");
        dbQueryBuilder.AppendLine("if @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("    update MersNumber set NextNumber = @nextValueStr");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    update BranchMERSNumber set next_number = @nextValueStr where mersmin_code = @branchCode");
        if (!alwaysGenerate)
          dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.AppendLine("if @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("    select AutoGeneration, CompanyId, @valueStr as NextNumber from MersNumber");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    select AutoGeneration, CompanyId = @branchCode, @valueStr as NextNumber from MersNumber");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.Serialized);
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteWarning(nameof (MersNumberGenerator), "No Mers Numbering data found in database.");
          return "";
        }
        DataRow dataRow = dataRowCollection[0];
        if (!alwaysGenerate && dataRow["AutoGeneration"].ToString() == "0")
          return "";
        string minSum = dataRow["CompanyId"].ToString() + dataRow["NextNumber"].ToString();
        string checksum = MersNumberGenerator.generateChecksum(minSum);
        string nextMersNumber = minSum + checksum;
        TraceLog.WriteVerbose(nameof (MersNumberGenerator), "Generated MERS number \"" + nextMersNumber + "\"");
        return nextMersNumber;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MersNumberGenerator), ex);
        return (string) null;
      }
    }

    public static MersNumberingInfo GetMersNumberingInfo()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("MersNumber"));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Error, nameof (MersNumberGenerator), (ServerException) new ServerDataException("Error in MersNumber table: no rows returned"));
        DataRow dataRow = dataRowCollection[0];
        return new MersNumberingInfo(dataRow["AutoGeneration"].ToString() == "1", dataRow["CompanyId"].ToString(), dataRow["NextNumber"].ToString());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MersNumberGenerator), ex);
        return (MersNumberingInfo) null;
      }
    }

    public static void SetMersNumberingInfo(MersNumberingInfo info)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("AutoGeneration", (object) info.AutoGenerate, (IDbEncoder) DbEncoding.Flag);
        values.Add("CompanyId", (object) info.CompanyID);
        values.Add("NextNumber", (object) info.NextNumber);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable("MersNumber"), values, new DbValueList());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MersNumberGenerator), ex);
      }
    }

    public static BranchMERSMINNumberingInfo[] GetAllBranchMERSMinNumberingInfo(bool onlyInUse)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (onlyInUse)
      {
        dbQueryBuilder.AppendLine("select distinct oc.mersmin_code, IsNull(bln.enabled, 0) as enabled, bln.next_number from org_chart oc left outer join BranchMERSNumber bln on oc.mersmin_code = bln.mersmin_code where IsNull(oc.mersmin_code, '') <> ''");
      }
      else
      {
        dbQueryBuilder.AppendLine("select distinct * from");
        dbQueryBuilder.AppendLine("\t(select oc.mersmin_code, IsNull(bln.enabled, 0) as enabled, bln.next_number from org_chart oc left outer join BranchMERSNumber bln on oc.mersmin_code = bln.mersmin_code where IsNull(oc.mersmin_code, '') <> '') t1");
        dbQueryBuilder.AppendLine("\tunion");
        dbQueryBuilder.AppendLine("\t(select mersmin_code, enabled, next_number from BranchMERSNumber)");
      }
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      BranchMERSMINNumberingInfo[] minNumberingInfo = new BranchMERSMINNumberingInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        minNumberingInfo[index] = MersNumberGenerator.dataRowToBranchMERSNumberingInfo(dataRowCollection[index]);
      return minNumberingInfo;
    }

    public static BranchMERSMINNumberingInfo GetBranchMERSNumberingInfo(string mersminCode)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("BranchMERSNumber"), new DbValue("mersmin_code", (object) mersminCode));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (BranchMERSMINNumberingInfo) null : MersNumberGenerator.dataRowToBranchMERSNumberingInfo(dataRowCollection[0]);
    }

    public static void SaveBranchMERSNumberingInfo(BranchMERSMINNumberingInfo info)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("BranchMERSNumber");
      dbQueryBuilder.DeleteFrom(table, new DbValue("mersmin_code", (object) info.MERSMINCode));
      dbQueryBuilder.InsertInto(table, MersNumberGenerator.createDbValueList(info), true, false);
      dbQueryBuilder.Execute();
    }

    private static DbValueList createDbValueList(BranchMERSMINNumberingInfo info)
    {
      return new DbValueList()
      {
        {
          "mersmin_code",
          (object) info.MERSMINCode
        },
        {
          "enabled",
          (object) info.Enabled,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "next_number",
          (object) info.NextNumber
        }
      };
    }

    private static BranchMERSMINNumberingInfo dataRowToBranchMERSNumberingInfo(DataRow r)
    {
      return new BranchMERSMINNumberingInfo(r["mersmin_code"].ToString(), (bool) r["enabled"], (string) SQL.Decode(r["next_number"], (object) ""));
    }

    private static string generateChecksum(string minSum)
    {
      string empty1 = string.Empty;
      for (int startIndex = 0; startIndex < minSum.Length; startIndex += 2)
        empty1 += minSum.Substring(startIndex, 1);
      string empty2 = string.Empty;
      for (int startIndex = 1; startIndex < minSum.Length; startIndex += 2)
        empty2 += minSum.Substring(startIndex, 1);
      string str = Convert.ToString(2 * MersNumberGenerator.intValue(empty1));
      int num1 = 0;
      for (int startIndex = 0; startIndex < str.Length; ++startIndex)
        num1 += MersNumberGenerator.intValue(str.Substring(startIndex, 1));
      int num2 = 0;
      for (int startIndex = 0; startIndex < empty2.Length; ++startIndex)
        num2 += MersNumberGenerator.intValue(empty2.Substring(startIndex, 1));
      string strValue = Convert.ToString(num1 + num2);
      return (!(strValue.Substring(strValue.Length - 1, 1) == "0") ? (strValue.Length != 1 ? MersNumberGenerator.intValue(strValue) + 10 - MersNumberGenerator.intValue(strValue.Substring(strValue.Length - 1, 1)) - MersNumberGenerator.intValue(strValue) : 10 - MersNumberGenerator.intValue(strValue)) : 0).ToString();
    }

    private static int intValue(string strValue)
    {
      return Convert.ToInt32(strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue));
    }
  }
}
