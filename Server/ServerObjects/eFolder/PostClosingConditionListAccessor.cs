// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.PostClosingConditionListAccessor
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
  public class PostClosingConditionListAccessor
  {
    private const string tableName = "PostClosingConditionList�";
    private const string doctableName = "PostClosingConditionDocumentList�";

    private PostClosingConditionListAccessor()
    {
    }

    public static ConditionTrackingSetup GetPostClosingConditionList()
    {
      return ClientContext.GetCurrent().Cache.Get<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.PostClosing), new Func<ConditionTrackingSetup>(PostClosingConditionListAccessor.GetPostClosingConditionListFromDb), CacheSetting.Low);
    }

    public static void MigratePostClosingConditionsToDb()
    {
      if (PostClosingConditionListAccessor.GetPostClosingConditionListFromDb().Count != 0)
        return;
      ConditionTrackingSetup trackingSetupFromXml = ConditionConfiguration.GetConditionTrackingSetupFromXml(ConditionType.PostClosing);
      if (trackingSetupFromXml.Count <= 0)
        return;
      PostClosingConditionListAccessor.xSavePostClosingConditionToDb(trackingSetupFromXml);
      ClientContext.GetCurrent().Cache.Remove(ConditionConfiguration.GetCacheName(ConditionType.PostClosing));
    }

    public static bool UpdatePostClosingCondition(
      PostClosingConditionTemplate postClosingConditionTemplate,
      string userName)
    {
      bool result = false;
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.PostClosing), (Action) (() => result = PostClosingConditionListAccessor.UpdatePostClosingConditionToDb(postClosingConditionTemplate, userName)), new Func<ConditionTrackingSetup>(PostClosingConditionListAccessor.GetPostClosingConditionListFromDb), CacheSetting.Low);
      return result;
    }

    public static void AddPostClosingCondition(
      PostClosingConditionTemplate postClosingConditionTemplate,
      string userName)
    {
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.PostClosing), (Action) (() => PostClosingConditionListAccessor.AddPostClosingConditionToDb(postClosingConditionTemplate, userName)), new Func<ConditionTrackingSetup>(PostClosingConditionListAccessor.GetPostClosingConditionListFromDb), CacheSetting.Low);
    }

    public static void DeletePostClosingCondition(Guid[] conditionGuids)
    {
      ClientContext.GetCurrent().Cache.Put<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(ConditionType.PostClosing), (Action) (() => PostClosingConditionListAccessor.DeletePostClosingConditionFromDb(conditionGuids)), new Func<ConditionTrackingSetup>(PostClosingConditionListAccessor.GetPostClosingConditionListFromDb), CacheSetting.Low);
    }

    private static void xSavePostClosingConditionToDb(
      ConditionTrackingSetup postClosingCondTrackingSetup)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete PostClosingConditionDocumentList");
      dbQueryBuilder.AppendLine("Delete PostClosingConditionList");
      if (postClosingCondTrackingSetup.Count <= 0)
        return;
      XmlDocument xmlDocument = new XmlDocument();
      using (BinaryObject binaryObject = new BinaryObject((IXmlSerializable) postClosingCondTrackingSetup))
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
        string innerText4 = selectNode.SelectSingleNode("element[@name='Source']").InnerText;
        string innerText5 = selectNode.SelectSingleNode("element[@name='Recipient']").InnerText;
        string innerText6 = selectNode.SelectSingleNode("element[@name='IsInternal']").InnerText;
        string innerText7 = selectNode.SelectSingleNode("element[@name='IsExternal']").InnerText;
        dbQueryBuilder.AppendLine("Insert into PostClosingConditionList (Guid, Name, Description, DaysTillDue, Source, Recipient, IsInternal, IsExternal) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) str) + ", " + SQL.Encode((object) innerText2) + ", " + SQL.Encode((object) innerText3) + ", " + SQL.Encode((object) innerText4) + ", " + SQL.Encode((object) innerText5) + ", " + SQL.Encode((object) innerText6) + ", " + SQL.Encode((object) innerText7) + ")");
        XmlNodeList childNodes = selectNode.SelectSingleNode("element[@name='Documents']").ChildNodes;
        for (int i = 0; i < childNodes.Count; ++i)
        {
          string innerText8 = childNodes[i].InnerText;
          dbQueryBuilder.AppendLine("Insert into PostClosingConditionDocumentList (GUID, DocumentsGUID) Values(" + SQL.Encode((object) innerText1) + ", " + SQL.Encode((object) innerText8) + ")");
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static ConditionTrackingSetup GetPostClosingConditionListFromDb()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      PostClosingConditionTrackingSetup conditionListFromDb = new PostClosingConditionTrackingSetup();
      dbQueryBuilder.AppendLine("Select * from PostClosingConditionList order by [Name] Asc");
      dbQueryBuilder.AppendLine("select ucd.Guid, ucd.DocumentsGUID from PostClosingConditionDocumentList ucd inner join PostClosingConditionList ucl on ucl.Guid = ucd.Guid");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default);
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows == null || dataSet.Tables[0].Rows.Count == 0)
        return (ConditionTrackingSetup) conditionListFromDb;
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      foreach (DataRow row1 in (InternalDataCollectionBase) table1.Rows)
      {
        PostClosingConditionTemplate template = new PostClosingConditionTemplate(row1["Guid"]?.ToString());
        template.Name = row1["Name"]?.ToString();
        template.Description = row1["Description"]?.ToString();
        int result1;
        int.TryParse(row1["DaysTillDue"]?.ToString(), out result1);
        template.DaysTillDue = result1;
        template.Source = row1["Source"]?.ToString();
        template.Recipient = row1["Recipient"]?.ToString();
        bool result2;
        bool.TryParse(row1["IsInternal"]?.ToString(), out result2);
        template.IsInternal = result2;
        bool result3;
        bool.TryParse(row1["IsExternal"]?.ToString(), out result3);
        template.IsExternal = result3;
        IEnumerable<DataRow> source = (IEnumerable<DataRow>) table2.Select("Guid ='" + row1["Guid"]?.ToString() + "'");
        if (source.Any<DataRow>())
          template.SetDocumentIds(source.Select<DataRow, string>((System.Func<DataRow, string>) (row => row["DocumentsGUID"].ToString())).ToArray<string>());
        conditionListFromDb.Add((ConditionTemplate) template);
        if (DateTime.TryParse(row1["LastModifiedDateTime"]?.ToString(), out DateTime _))
          template.LastModifiedDateTime = (DateTime) row1["LastModifiedDateTime"];
      }
      return (ConditionTrackingSetup) conditionListFromDb;
    }

    private static bool UpdatePostClosingConditionToDb(
      PostClosingConditionTemplate condition,
      string userName)
    {
      if (condition == null)
        return false;
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Append("update PostClosingConditionList set Name = " + SQL.Encode((object) condition.Name) + ", Description = " + SQL.Encode((object) condition.Description) + ", DaysTillDue = " + SQL.Encode((object) condition.DaysTillDue) + ", Source = " + SQL.Encode((object) condition.Source) + ", Recipient = " + SQL.Encode((object) condition.Recipient) + ", IsInternal = " + SQL.EncodeBooleanFull(condition.IsInternal) + ", IsExternal = " + SQL.EncodeBooleanFull(condition.IsExternal) + ", LastModifiedDateTime = getdate(), LastModifiedBy = " + SQL.Encode((object) userName) + " where guid = " + SQL.Encode((object) condition.Guid) + " and LastModifiedDateTime = '" + condition.LastModifiedDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "' If @@ROWCOUNT = 1 Begin");
        string[] documentIds = condition.GetDocumentIDs();
        if (documentIds.Length != 0)
        {
          dbQueryBuilder.Append(" WITH CTE_UWDocs AS (SELECT * FROM PostClosingConditionDocumentList WHERE Guid = " + SQL.Encode((object) condition.Guid) + ") MERGE INTO CTE_UWDocs TGT USING(select * from (values");
          for (int index = 0; index < documentIds.Length; ++index)
            dbQueryBuilder.Append("(" + SQL.Encode((object) condition.Guid) + "," + SQL.Encode((object) documentIds[index]) + "),");
          dbQueryBuilder.Remove(dbQueryBuilder.Length - 1, 1);
          dbQueryBuilder.Append(") a (Guid, DocumentsGUID)) AS SRC ON SRC.Guid = TGT.Guid AND SRC.DocumentsGUID = TGT.DocumentsGUID WHEN NOT MATCHED BY TARGET THEN INSERT([Guid], DocumentsGUID) VALUES(SRC.[Guid], SRC.DocumentsGUID) WHEN NOT MATCHED BY SOURCE THEN DELETE;");
        }
        else
          dbQueryBuilder.Append(" Delete from PostClosingConditionDocumentList where Guid = " + SQL.Encode((object) condition.Guid) ?? "");
        dbQueryBuilder.Append(" End Else begin raiserror('Update Failed - This condition was modified by another user. Reopen condition and try again', 15, 1) end");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      catch (ServerDataException ex)
      {
        Err.Reraise(nameof (PostClosingConditionListAccessor), ex.InnerException);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PostClosingConditionListAccessor), ex);
      }
      return true;
    }

    private static void AddPostClosingConditionToDb(
      PostClosingConditionTemplate condition,
      string userName)
    {
      if (condition == null)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Insert into PostClosingConditionList (Guid, Name, Description, DaysTillDue, Source, Recipient, IsInternal, IsExternal, LastModifiedBy) Values(" + SQL.Encode((object) condition.Guid) + ", " + SQL.Encode((object) condition.Name) + ", " + SQL.Encode((object) condition.Description) + ", " + SQL.Encode((object) condition.DaysTillDue) + ", " + SQL.Encode((object) condition.Source) + ", " + SQL.Encode((object) condition.Recipient) + ", " + SQL.EncodeBooleanFull(condition.IsInternal) + ", " + SQL.EncodeBooleanFull(condition.IsExternal) + ", " + SQL.Encode((object) userName) + ")");
        foreach (string documentId in condition.GetDocumentIDs())
          dbQueryBuilder.AppendLine("Insert into PostClosingConditionDocumentList (Guid, DocumentsGUID) Values(" + SQL.Encode((object) condition.Guid) + ", " + SQL.Encode((object) documentId) + ")");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PostClosingConditionListAccessor), ex);
      }
    }

    private static void DeletePostClosingConditionFromDb(Guid[] conditionGuids)
    {
      if (conditionGuids == null || conditionGuids.Length < 1)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str = string.Join(",", (IEnumerable<string>) ((IEnumerable<string>) string.Join<Guid>(",", (IEnumerable<Guid>) conditionGuids).Split(',')).Select<string, string>((System.Func<string, string>) (x => string.Format("'{0}'", (object) x))).ToList<string>());
        dbQueryBuilder.AppendLine("Delete PostClosingConditionDocumentList where Guid IN(" + str + ");");
        dbQueryBuilder.AppendLine(" Delete PostClosingConditionList where Guid IN(" + str + ");");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PostClosingConditionListAccessor), ex);
      }
    }
  }
}
