// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.UnderwritingConditionListAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class UnderwritingConditionListAccessor
  {
    private const string tableName = "UnderwritingConditionList�";
    private const string doctableName = "UnderwritingConditionDocumentList�";

    private UnderwritingConditionListAccessor()
    {
    }

    public static ConditionTrackingSetup GetUnderwritingConditionList()
    {
      return ClientContext.GetCurrent().Cache.Get<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.Underwriting), new Func<ConditionTrackingSetup>(UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb), CacheSetting.Low);
    }

    public static void MigrateUnderwritingConditionsToDb()
    {
      if (UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb().Count != 0)
        return;
      ConditionTrackingSetup trackingSetupFromXml = ConditionConfiguration.GetConditionTrackingSetupFromXml(ConditionType.Underwriting);
      if (trackingSetupFromXml.Count <= 0)
        return;
      UnderwritingConditionListAccessor.xSaveUnderwritingConditionListToDb(trackingSetupFromXml);
      ClientContext.GetCurrent().Cache.Remove(ConditionConfiguration.GetCacheName(ConditionType.Underwriting));
    }

    public static SellConditionTrackingSetup GetSellConditionList()
    {
      UnderwritingConditionTrackingSetup conditionTrackingSetup = (UnderwritingConditionTrackingSetup) ClientContext.GetCurrent().Cache.Get<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.Underwriting), new Func<ConditionTrackingSetup>(UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb), CacheSetting.Low);
      SellConditionTrackingSetup sellConditionList = new SellConditionTrackingSetup();
      foreach (UnderwritingConditionTemplate conditionTemplate1 in conditionTrackingSetup.ToArray())
      {
        SellConditionTemplate conditionTemplate2 = new SellConditionTemplate(conditionTemplate1.Guid);
        conditionTemplate2.Name = conditionTemplate1.Name;
        conditionTemplate2.Description = conditionTemplate1.Description;
        conditionTemplate2.DaysTillDue = conditionTemplate1.DaysTillDue;
        conditionTemplate2.Category = conditionTemplate1.Category;
        conditionTemplate2.ForRoleID = conditionTemplate1.ForRoleID;
        conditionTemplate2.AllowToClear = conditionTemplate1.AllowToClear;
        conditionTemplate2.PriorTo = conditionTemplate1.PriorTo;
        conditionTemplate2.IsInternal = conditionTemplate1.IsInternal;
        conditionTemplate2.IsExternal = conditionTemplate1.IsExternal;
        conditionTemplate2.TPOCondDocType = conditionTemplate1.TPOCondDocType;
        conditionTemplate2.TPOCondDocGuid = conditionTemplate1.TPOCondDocGuid;
        conditionTemplate2.SetDocumentIds(conditionTemplate1.GetDocumentIDs());
      }
      return sellConditionList;
    }

    public static void AddUnderwritingCondition(
      UnderwritingConditionTemplate underwritingCondTrackingSetup,
      string userName)
    {
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.Underwriting), (Action) (() => UnderwritingConditionListAccessor.AddUnderwritingConditionToDb(underwritingCondTrackingSetup, userName)), new Func<ConditionTrackingSetup>(UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb), CacheSetting.Low);
    }

    public static bool UpdateUnderwritingCondition(
      UnderwritingConditionTemplate underwritingCondTrackingSetup,
      string userName)
    {
      bool result = false;
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.Underwriting), (Action) (() => result = UnderwritingConditionListAccessor.UpdateUnderwritingConditionToDb(underwritingCondTrackingSetup, userName)), new Func<ConditionTrackingSetup>(UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb), CacheSetting.Low);
      return result;
    }

    public static void DeleteUnderwritingCondition(Guid[] conditionGuids)
    {
      if (conditionGuids == null || conditionGuids.Length < 1)
        return;
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.Underwriting), (Action) (() => UnderwritingConditionListAccessor.DeleteUnderwritingConditionFromDb(conditionGuids)), new Func<ConditionTrackingSetup>(UnderwritingConditionListAccessor.GetUnderwritingConditionListFromDb), CacheSetting.Low);
    }

    private static ConditionTrackingSetup GetUnderwritingConditionListFromDb()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      UnderwritingConditionTrackingSetup conditionListFromDb = new UnderwritingConditionTrackingSetup();
      dbQueryBuilder.AppendLine("Select * from UnderwritingConditionList order by [Name] Asc");
      dbQueryBuilder.AppendLine("select ucd.Guid, ucd.DocumentsGUID from UnderwritingConditionDocumentList ucd inner join UnderwritingConditionList ucl on ucl.Guid = ucd.Guid");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default);
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows == null || dataSet.Tables[0].Rows.Count == 0)
        return (ConditionTrackingSetup) conditionListFromDb;
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      foreach (DataRow row1 in (InternalDataCollectionBase) table1.Rows)
      {
        UnderwritingConditionTemplate template = new UnderwritingConditionTemplate(row1["Guid"]?.ToString());
        template.Name = row1["Name"]?.ToString();
        template.Description = row1["Description"]?.ToString();
        int result1;
        int.TryParse(row1["DaysTillDue"]?.ToString(), out result1);
        template.DaysTillDue = result1;
        template.Category = row1["Category"]?.ToString();
        int result2;
        if (row1["RoleID"]?.ToString() == "")
          result2 = -1;
        else
          int.TryParse(row1["RoleID"]?.ToString(), out result2);
        template.ForRoleID = result2;
        bool result3;
        bool.TryParse(row1["AllowToClear"]?.ToString(), out result3);
        template.AllowToClear = result3;
        template.PriorTo = row1["PriorTo"]?.ToString();
        bool result4;
        bool.TryParse(row1["IsInternal"]?.ToString(), out result4);
        template.IsInternal = result4;
        bool result5;
        bool.TryParse(row1["IsExternal"]?.ToString(), out result5);
        template.IsExternal = result5;
        template.TPOCondDocType = row1["TPOCondDocType"]?.ToString();
        template.TPOCondDocGuid = row1["TPOCondDocGuid"]?.ToString();
        IEnumerable<DataRow> source = (IEnumerable<DataRow>) table2.Select("Guid ='" + row1["Guid"]?.ToString() + "'");
        if (source.Any<DataRow>())
          template.SetDocumentIds(source.Select<DataRow, string>((System.Func<DataRow, string>) (row => row["DocumentsGUID"].ToString())).ToArray<string>());
        conditionListFromDb.Add((ConditionTemplate) template);
        if (DateTime.TryParse(row1["LastModifiedDateTime"]?.ToString(), out DateTime _))
          template.LastModifiedDateTime = (DateTime) row1["LastModifiedDateTime"];
      }
      return (ConditionTrackingSetup) conditionListFromDb;
    }

    private static void AddUnderwritingConditionToDb(
      UnderwritingConditionTemplate condition,
      string userName)
    {
      if (condition == null)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str = condition.ForRoleID < 0 ? (string) null : SQL.Encode((object) condition.ForRoleID);
        dbQueryBuilder.AppendLine("Insert into UnderwritingConditionList (Guid, Name, Description, DaysTillDue, Category, RoleID, AllowToClear, PriorTo, IsInternal, IsExternal, TPOCondDocType, TPOCondDocGuid, LastModifiedBy) Values(" + SQL.Encode((object) condition.Guid) + ", " + SQL.Encode((object) condition.Name) + ", " + SQL.Encode((object) condition.Description) + ", " + SQL.Encode((object) condition.DaysTillDue) + ", " + SQL.Encode((object) condition.Category) + ", " + str + ", " + SQL.EncodeBooleanFull(condition.AllowToClear) + ", " + SQL.Encode((object) condition.PriorTo) + ", " + SQL.EncodeBooleanFull(condition.IsInternal) + ", " + SQL.EncodeBooleanFull(condition.IsExternal) + ", " + SQL.Encode((object) condition.TPOCondDocType) + ", " + SQL.Encode((object) condition.TPOCondDocGuid) + ", " + SQL.Encode((object) userName) + ")");
        foreach (string documentId in condition.GetDocumentIDs())
          dbQueryBuilder.AppendLine("Insert into UnderwritingConditionDocumentList (Guid, DocumentsGUID) Values(" + SQL.Encode((object) condition.Guid) + ", " + SQL.Encode((object) documentId) + ")");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (UnderwritingConditionListAccessor), ex);
      }
    }

    private static bool UpdateUnderwritingConditionToDb(
      UnderwritingConditionTemplate condition,
      string userName)
    {
      if (condition == null)
        return false;
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Append("update UnderwritingConditionList set Name = " + SQL.Encode((object) condition.Name) + ", Description = " + SQL.Encode((object) condition.Description) + ", DaysTillDue = " + SQL.Encode((object) condition.DaysTillDue) + ", category = " + SQL.Encode((object) condition.Category) + ", RoleID = " + SQL.Encode((object) condition.ForRoleID) + ",  AllowToClear = " + SQL.EncodeBooleanFull(condition.AllowToClear) + ", PriorTo = " + SQL.Encode((object) condition.PriorTo) + ", IsInternal = " + SQL.EncodeBooleanFull(condition.IsInternal) + ",  IsExternal = " + SQL.EncodeBooleanFull(condition.IsExternal) + ", TPOCondDocType = " + SQL.Encode((object) condition.TPOCondDocType) + " , TPOCondDocGuid = " + SQL.Encode((object) condition.TPOCondDocGuid) + ", LastModifiedDateTime = getdate(), LastModifiedBy = " + SQL.Encode((object) userName) + " where guid = " + SQL.Encode((object) condition.Guid) + " and LastModifiedDateTime = '" + condition.LastModifiedDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "' If @@ROWCOUNT = 1 Begin");
        string[] documentIds = condition.GetDocumentIDs();
        if (documentIds.Length != 0)
        {
          dbQueryBuilder.Append(" WITH CTE_UWDocs AS (SELECT * FROM UnderwritingConditionDocumentList WHERE Guid = " + SQL.Encode((object) condition.Guid) + ") MERGE INTO CTE_UWDocs TGT USING(select * from (values");
          for (int index = 0; index < documentIds.Length; ++index)
            dbQueryBuilder.Append("(" + SQL.Encode((object) condition.Guid) + "," + SQL.Encode((object) documentIds[index]) + "),");
          dbQueryBuilder.Remove(dbQueryBuilder.Length - 1, 1);
          dbQueryBuilder.Append(") a (Guid, DocumentsGUID)) AS SRC ON SRC.Guid = TGT.Guid AND SRC.DocumentsGUID = TGT.DocumentsGUID WHEN NOT MATCHED BY TARGET THEN INSERT([Guid], DocumentsGUID) VALUES(SRC.[Guid], SRC.DocumentsGUID) WHEN NOT MATCHED BY SOURCE THEN DELETE;");
        }
        else
          dbQueryBuilder.Append(" Delete from UnderwritingConditionDocumentList where Guid = " + SQL.Encode((object) condition.Guid) ?? "");
        dbQueryBuilder.Append(" End Else begin raiserror('Update Failed - This condition was modified by another user. Reopen condition and try again', 15, 1) end");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      catch (ServerDataException ex)
      {
        Err.Reraise(nameof (UnderwritingConditionListAccessor), ex.InnerException);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (UnderwritingConditionListAccessor), ex);
      }
      return true;
    }

    private static void DeleteUnderwritingConditionFromDb(Guid[] conditionGuids)
    {
      if (conditionGuids != null && conditionGuids.Length == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str = string.Join(",", (IEnumerable<string>) ((IEnumerable<string>) string.Join<Guid>(",", (IEnumerable<Guid>) conditionGuids).Split(',')).Select<string, string>((System.Func<string, string>) (x => string.Format("'{0}'", (object) x))).ToList<string>());
        dbQueryBuilder.AppendLine("Delete UnderwritingConditionDocumentList where Guid IN(" + str + ");");
        dbQueryBuilder.AppendLine(" Delete UnderwritingConditionList where Guid IN(" + str + ");");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (UnderwritingConditionListAccessor), ex);
      }
    }

    private static void xSaveUnderwritingConditionListToDb(
      ConditionTrackingSetup underwritingCondTrackingSetup)
    {
      if (underwritingCondTrackingSetup.Count > 0)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder1.AppendLine("Delete UnderwritingConditionDocumentList");
        dbQueryBuilder1.AppendLine("Delete UnderwritingConditionList");
        try
        {
          XmlDocument xmlDocument = new XmlDocument();
          using (BinaryObject binaryObject = new BinaryObject((IXmlSerializable) underwritingCondTrackingSetup))
          {
            string xml = Encoding.UTF8.GetString(binaryObject.GetBytes());
            xmlDocument.LoadXml(xml);
          }
          foreach (XmlNode selectNode in xmlDocument.SelectNodes("objdata/element/element[element[@name='Guid']]"))
          {
            string innerText1 = selectNode.SelectSingleNode("element[@name='Guid']").InnerText;
            string str = selectNode.SelectSingleNode("element[@name='Name']").InnerText;
            if (str.Length > 256)
              str = str.Substring(0, 256);
            string innerText2 = selectNode.SelectSingleNode("element[@name='Description']").InnerText;
            string innerText3 = selectNode.SelectSingleNode("element[@name='DaysTillDue']").InnerText;
            XmlNodeList childNodes = selectNode.SelectSingleNode("element[@name='Documents']").ChildNodes;
            string innerText4 = selectNode.SelectSingleNode("element[@name='Category']").InnerText;
            string innerText5 = selectNode.SelectSingleNode("element[@name='ForRoleID']").InnerText;
            string innerText6 = selectNode.SelectSingleNode("element[@name='AllowToClear']").InnerText;
            string innerText7 = selectNode.SelectSingleNode("element[@name='PriorTo']").InnerText;
            string innerText8 = selectNode.SelectSingleNode("element[@name='IsInternal']").InnerText;
            string innerText9 = selectNode.SelectSingleNode("element[@name='IsExternal']").InnerText;
            string innerText10 = selectNode.SelectSingleNode("element[@name='TPOCondDocType']").InnerText;
            string innerText11 = selectNode.SelectSingleNode("element[@name='TPOCondDocGuid']").InnerText;
            if (int.Parse(innerText5) < 0)
            {
              dbQueryBuilder1.AppendLine("Insert into UnderwritingConditionList (Guid, Name, Description, DaysTillDue, Category, AllowToClear, PriorTo, IsInternal, IsExternal, TPOCondDocType, TPOCondDocGuid) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) str) + ", " + SQL.Encode((object) innerText2) + ", " + SQL.Encode((object) innerText3) + ", " + SQL.Encode((object) innerText4) + ", " + SQL.Encode((object) innerText6) + ", " + SQL.Encode((object) innerText7) + ", " + SQL.Encode((object) innerText8) + ", " + SQL.Encode((object) innerText9) + ", " + SQL.Encode((object) innerText10) + ", " + SQL.Encode((object) innerText11) + ")");
            }
            else
            {
              dbQueryBuilder2.AppendLine("Select count(*) from Roles where RoleID=" + SQL.Encode((object) innerText5));
              if (Convert.ToInt32(dbQueryBuilder2.ExecuteScalar()) > 0)
                dbQueryBuilder1.AppendLine("Insert into UnderwritingConditionList (Guid, Name, Description, DaysTillDue, Category, RoleID, AllowToClear, PriorTo, IsInternal, IsExternal, TPOCondDocType, TPOCondDocGuid) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) str) + ", " + SQL.Encode((object) innerText2) + ", " + SQL.Encode((object) innerText3) + ", " + SQL.Encode((object) innerText4) + ", " + SQL.Encode((object) innerText5) + ", " + SQL.Encode((object) innerText6) + ", " + SQL.Encode((object) innerText7) + ", " + SQL.Encode((object) innerText8) + ", " + SQL.Encode((object) innerText9) + ", " + SQL.Encode((object) innerText10) + ", " + SQL.Encode((object) innerText11) + ")");
              else
                dbQueryBuilder1.AppendLine("Insert into UnderwritingConditionList (Guid, Name, Description, DaysTillDue, Category, AllowToClear, PriorTo, IsInternal, IsExternal, TPOCondDocType, TPOCondDocGuid) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) str) + ", " + SQL.Encode((object) innerText2) + ", " + SQL.Encode((object) innerText3) + ", " + SQL.Encode((object) innerText4) + ", " + SQL.Encode((object) innerText6) + ", " + SQL.Encode((object) innerText7) + ", " + SQL.Encode((object) innerText8) + ", " + SQL.Encode((object) innerText9) + ", " + SQL.Encode((object) innerText10) + ", " + SQL.Encode((object) innerText11) + ")");
              dbQueryBuilder2.Reset();
            }
            for (int i = 0; i < childNodes.Count; ++i)
            {
              string innerText12 = childNodes[i].InnerText;
              dbQueryBuilder1.AppendLine("Insert into UnderwritingConditionDocumentList (Guid, DocumentsGUID) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) innerText12) + ")");
            }
          }
          dbQueryBuilder1.ExecuteNonQuery(DbTransactionType.Default);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (UnderwritingConditionListAccessor), ex);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("Delete UnderwritingConditionDocumentList");
        dbQueryBuilder.AppendLine("Delete UnderwritingConditionList");
        dbQueryBuilder.Execute();
      }
    }
  }
}
