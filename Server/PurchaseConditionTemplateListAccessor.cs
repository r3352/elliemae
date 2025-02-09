// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.PurchaseConditionTemplateListAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class PurchaseConditionTemplateListAccessor
  {
    public static ConditionTrackingSetup GetPurchaseConditionTemplateList()
    {
      List<PurchaseConditionTemplate> conditionTemplateList1 = PurchaseConditionTemplateListAccessor.SearchForPurchaseConditionTemplates((string) null);
      if (conditionTemplateList1 == null || conditionTemplateList1.Count <= 0)
        return (ConditionTrackingSetup) null;
      PurchaseConditionTemplateList conditionTemplateList2 = new PurchaseConditionTemplateList();
      foreach (PurchaseConditionTemplate template in conditionTemplateList1)
        conditionTemplateList2.Add((ConditionTemplate) template);
      return (ConditionTrackingSetup) conditionTemplateList2;
    }

    public static string AddPurchaseConditionTemplate(
      PurchaseConditionTemplate purchaseConditionTemplate)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      StringBuilder stringBuilder1 = new StringBuilder();
      string number = purchaseConditionTemplate.Number;
      if (!string.IsNullOrEmpty(number))
      {
        stringBuilder1.AppendLine("SELECT 1 FROM PurchaseConditionList WHERE Number = '" + number + "'");
        dbQueryBuilder1.Append(stringBuilder1.ToString());
        DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
          throw new Exception(string.Format("Condition Number \"{0}\" already exists as \"{0}\". Purchase Condition template has not been added.", (object) number));
      }
      string guid = purchaseConditionTemplate.Guid;
      if (string.IsNullOrWhiteSpace(guid))
        guid = Guid.NewGuid().ToString();
      stringBuilder1.AppendLine("SELECT 1 FROM PurchaseConditionList WHERE Guid = '" + guid + "'");
      dbQueryBuilder1.Append(stringBuilder1.ToString());
      try
      {
        DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
        if (dataSet != null)
        {
          if (dataSet.Tables.Count > 0)
          {
            if (dataSet.Tables[0].Rows.Count > 0)
              throw new Exception(string.Format("Duplicate entry for guid, \"{0}\"", (object) guid));
          }
        }
      }
      catch
      {
      }
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendLine("INSERT INTO PurchaseConditionList (Guid, Number, Name, Description, Category, [Sub-Category], RoleID, AllowToClear, PriorTo, IsInternal, IsExternal)  ");
      stringBuilder2.AppendLine("     VALUES ( '" + guid + "', ");
      stringBuilder2.AppendLine("     '" + purchaseConditionTemplate.Number + "', ");
      stringBuilder2.AppendLine("     '" + purchaseConditionTemplate.Name + "', ");
      stringBuilder2.AppendLine("     '" + purchaseConditionTemplate.Description + "', ");
      stringBuilder2.AppendLine("     " + purchaseConditionTemplate.Category.ToString() + ", ");
      stringBuilder2.AppendLine("     " + purchaseConditionTemplate.Subcategory.ToString() + ", ");
      stringBuilder2.AppendLine("     " + purchaseConditionTemplate.RoleID.ToString() + ", ");
      if (purchaseConditionTemplate.AllowToClear)
        stringBuilder2.AppendLine("     'True', ");
      else
        stringBuilder2.AppendLine("     'False', ");
      stringBuilder2.AppendLine("     '" + purchaseConditionTemplate.PriorTo + "', ");
      if (purchaseConditionTemplate.IsInternal)
        stringBuilder2.AppendLine("     'True', ");
      else
        stringBuilder2.AppendLine("     'False', ");
      if (purchaseConditionTemplate.IsExternal)
        stringBuilder2.AppendLine("     'True')");
      else
        stringBuilder2.AppendLine("     'False')");
      dbQueryBuilder2.Append(stringBuilder2.ToString());
      dbQueryBuilder2.ExecuteNonQuery();
      DbQueryBuilder dbQueryBuilder3 = new DbQueryBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder3.AppendLine("IF EXISTS(SELECT 1 FROM PurchaseConditionDocumentList WHERE Guid = '" + guid + "')");
      stringBuilder3.AppendLine("     DELETE FROM PurchaseConditionDocumentList WHERE Guid = '" + guid + "'");
      foreach (string documentId in purchaseConditionTemplate.GetDocumentIDs())
        stringBuilder3.AppendLine("INSERT INTO PurchaseConditionDocumentList (Guid, DocumentsGUID) VALUES ('" + guid + "', '" + documentId + "')");
      dbQueryBuilder3.Append(stringBuilder3.ToString());
      dbQueryBuilder3.ExecuteNonQuery();
      return guid;
    }

    public static void UpdatePurchaseConditionTemplate(
      PurchaseConditionTemplate purchaseConditionTemplate)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("SELECT 1 FROM PurchaseConditionList WHERE Guid = '" + purchaseConditionTemplate.Guid + "'");
      dbQueryBuilder1.Append(stringBuilder1.ToString());
      try
      {
        DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
        if (dataSet != null && dataSet.Tables.Count > 0)
        {
          if (dataSet.Tables[0].Rows.Count > 0)
            goto label_5;
        }
        throw new Exception(string.Format("No entry for guid, \"{0}\"", (object) purchaseConditionTemplate.Guid));
      }
      catch
      {
        throw new Exception(string.Format("No entry for guid, \"{0}\"", (object) purchaseConditionTemplate.Guid));
      }
label_5:
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendLine("UPDATE PurchaseConditionList");
      stringBuilder2.AppendLine("     SET Number = '" + purchaseConditionTemplate.Number + "', ");
      stringBuilder2.AppendLine("     Name = '" + purchaseConditionTemplate.Name + "', ");
      stringBuilder2.AppendLine("     Description = '" + purchaseConditionTemplate.Description + "', ");
      stringBuilder2.AppendLine("     Category = " + purchaseConditionTemplate.Category.ToString() + ", ");
      stringBuilder2.AppendLine("     [Sub-Category] = " + purchaseConditionTemplate.Subcategory.ToString() + ", ");
      stringBuilder2.AppendLine("     RoleID = " + purchaseConditionTemplate.RoleID.ToString() + ", ");
      if (purchaseConditionTemplate.AllowToClear)
        stringBuilder2.AppendLine("     AllowToClear = 'True', ");
      else
        stringBuilder2.AppendLine("     AllowToClear = 'False', ");
      stringBuilder2.AppendLine("     PriorTo = '" + purchaseConditionTemplate.PriorTo + "', ");
      if (purchaseConditionTemplate.IsInternal)
        stringBuilder2.AppendLine("     IsInternal = 'True', ");
      else
        stringBuilder2.AppendLine("     IsInternal = 'False', ");
      if (purchaseConditionTemplate.IsExternal)
        stringBuilder2.AppendLine("     IsExternal = 'True'");
      else
        stringBuilder2.AppendLine("     IsExternal = 'False'");
      stringBuilder2.AppendLine("     WHERE Guid = '" + purchaseConditionTemplate.Guid + "'");
      dbQueryBuilder2.Append(stringBuilder2.ToString());
      dbQueryBuilder2.ExecuteNonQuery();
      DbQueryBuilder dbQueryBuilder3 = new DbQueryBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder3.AppendLine("IF EXISTS(SELECT 1 FROM PurchaseConditionDocumentList WHERE Guid = '" + purchaseConditionTemplate.Guid + "')");
      stringBuilder3.AppendLine("     DELETE FROM PurchaseConditionDocumentList WHERE Guid = '" + purchaseConditionTemplate.Guid + "'");
      foreach (string documentId in purchaseConditionTemplate.GetDocumentIDs())
        stringBuilder3.AppendLine("INSERT INTO PurchaseConditionDocumentList (Guid, DocumentsGUID) VALUES ('" + purchaseConditionTemplate.Guid + "', '" + documentId + "')");
      dbQueryBuilder3.Append(stringBuilder3.ToString());
      dbQueryBuilder3.ExecuteNonQuery();
    }

    public static void DeletePurchaseConditionTemplate(string guid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("IF EXISTS(SELECT 1 FROM PurchaseConditionDocumentList WHERE Guid = '" + guid + "')");
      stringBuilder.AppendLine("     DELETE FROM PurchaseConditionDocumentList WHERE Guid = '" + guid + "'");
      stringBuilder.AppendLine("IF EXISTS(SELECT 1 FROM PurchaseConditionList WHERE Guid = '" + guid + "')");
      stringBuilder.AppendLine("     DELETE FROM PurchaseConditionList WHERE Guid = '" + guid + "'");
      dbQueryBuilder.Append(stringBuilder.ToString());
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static PurchaseConditionTemplate GetPurchaseConditionTemplate(string guid)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Append("Select * from PurchaseConditionList where Guid = '" + guid + "'");
      DataSet dataSet1 = dbQueryBuilder1.ExecuteSetQuery();
      if (dataSet1 != null && dataSet1.Tables.Count > 0 && dataSet1.Tables[0].Rows.Count > 0)
      {
        IEnumerator enumerator = dataSet1.Tables[0].Rows.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            PurchaseConditionTemplate conditionTemplate1 = new PurchaseConditionTemplate(current["Guid"].ToString());
            conditionTemplate1.Number = current["Number"].ToString();
            conditionTemplate1.Name = current["Name"].ToString();
            conditionTemplate1.Description = current["Description"].ToString();
            conditionTemplate1.Category = current["Category"].ToString();
            conditionTemplate1.Subcategory = SQL.DecodeInt(current["Sub-Category"]);
            conditionTemplate1.RoleID = SQL.DecodeInt(current["RoleID"]);
            conditionTemplate1.AllowToClear = SQL.DecodeBoolean(current["AllowToClear"]);
            conditionTemplate1.PriorTo = current["PriorTo"].ToString();
            conditionTemplate1.IsInternal = SQL.DecodeBoolean(current["IsInternal"]);
            conditionTemplate1.IsExternal = SQL.DecodeBoolean(current["IsExternal"]);
            PurchaseConditionTemplate conditionTemplate2 = conditionTemplate1;
            DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
            dbQueryBuilder2.Append("Select pcdl.Guid, pcdl.DocumentsGUID, dt.Name from PurchaseConditionDocumentList pcdl \r\n                                 inner join DocumentTemplates dt on pcdl.DocumentsGUID = dt.Guid\r\n                                 where pcdl.Guid = '" + conditionTemplate2.Guid + "'");
            DataSet dataSet2 = dbQueryBuilder2.ExecuteSetQuery();
            if (dataSet2 != null && dataSet2.Tables.Count > 0 && dataSet2.Tables[0].Rows.Count > 0)
            {
              List<string> stringList = new List<string>();
              conditionTemplate2.Documents = new List<DocumentTemplate>();
              foreach (DataRow row in (InternalDataCollectionBase) dataSet2.Tables[0].Rows)
              {
                string guid1 = row["DocumentsGUID"].ToString();
                string name = row["Name"].ToString();
                stringList.Add(guid1);
                conditionTemplate2.Documents.Add(new DocumentTemplate(guid1, name));
              }
              if (stringList.Count > 0)
                conditionTemplate2.SetDocumentIds(stringList.ToArray());
            }
            return conditionTemplate2;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      return (PurchaseConditionTemplate) null;
    }

    public static List<PurchaseConditionTemplate> SearchForPurchaseConditionTemplates(
      string conditions)
    {
      List<PurchaseConditionTemplate> conditionTemplateList = new List<PurchaseConditionTemplate>();
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      if (string.IsNullOrWhiteSpace(conditions))
        dbQueryBuilder1.Append("Select * from PurchaseConditionList");
      else
        dbQueryBuilder1.Append("Select * from PurchaseConditionList where " + conditions);
      DataSet dataSet1 = dbQueryBuilder1.ExecuteSetQuery();
      if (dataSet1 != null && dataSet1.Tables.Count > 0 && dataSet1.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row1 in (InternalDataCollectionBase) dataSet1.Tables[0].Rows)
        {
          PurchaseConditionTemplate conditionTemplate1 = new PurchaseConditionTemplate(row1["Guid"].ToString());
          conditionTemplate1.Number = row1["Number"].ToString();
          conditionTemplate1.Name = row1["Name"].ToString();
          conditionTemplate1.Description = row1["Description"].ToString();
          conditionTemplate1.Category = row1["Category"].ToString();
          conditionTemplate1.Subcategory = SQL.DecodeInt(row1["Sub-Category"]);
          conditionTemplate1.RoleID = SQL.DecodeInt(row1["RoleID"]);
          conditionTemplate1.AllowToClear = SQL.DecodeBoolean(row1["AllowToClear"]);
          conditionTemplate1.PriorTo = row1["PriorTo"].ToString();
          conditionTemplate1.IsInternal = SQL.DecodeBoolean(row1["IsInternal"]);
          conditionTemplate1.IsExternal = SQL.DecodeBoolean(row1["IsExternal"]);
          PurchaseConditionTemplate conditionTemplate2 = conditionTemplate1;
          conditionTemplateList.Add(conditionTemplate2);
          DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
          dbQueryBuilder2.Append("Select pcdl.Guid, pcdl.DocumentsGUID, dt.Name from PurchaseConditionDocumentList pcdl \r\n                                 inner join DocumentTemplates dt on pcdl.DocumentsGUID = dt.Guid\r\n                                 where pcdl.Guid = '" + conditionTemplate2.Guid + "'");
          DataSet dataSet2 = dbQueryBuilder2.ExecuteSetQuery();
          if (dataSet2 != null && dataSet2.Tables.Count > 0 && dataSet2.Tables[0].Rows.Count > 0)
          {
            List<string> stringList = new List<string>();
            conditionTemplate2.Documents = new List<DocumentTemplate>();
            foreach (DataRow row2 in (InternalDataCollectionBase) dataSet2.Tables[0].Rows)
            {
              string guid = row2["DocumentsGUID"].ToString();
              string name = row2["Name"].ToString();
              stringList.Add(guid);
              conditionTemplate2.Documents.Add(new DocumentTemplate(guid, name));
            }
            if (stringList.Count > 0)
              conditionTemplate2.SetDocumentIds(stringList.ToArray());
          }
        }
      }
      return conditionTemplateList;
    }
  }
}
