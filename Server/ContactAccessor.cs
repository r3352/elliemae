// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ContactAccessor
  {
    private const string className = "ContactAccessor�";
    private const string BUSINESS_CUSTOM_FIELD_PAGE_NAMES = "BusinessCustomFieldPageNames�";
    private const string BUSINESS_CUSTOM_FIELDS = "BusinessCustomFields�";
    private const string BUSINESS_CUSTOM_FIELD_OPTIONS = "BusinessCustomFieldOptions�";
    private const string xmlFileRelativePath_BusinessCustomFields = "Users\\BizCustomFields.xml�";
    private const string BORROWER_CUSTOM_FIELD_PAGE_NAMES = "BorrowerCustomFieldPageNames�";
    private const string BORROWER_CUSTOM_FIELDS = "BorrowerCustomFields�";
    private const string BORROWER_CUSTOM_FIELD_OPTIONS = "BorrowerCustomFieldOptions�";
    private const string xmlFileRelativePath_BorrowerCustomFields = "Users\\BorCustomFields.xml�";
    private const string BORROWER_CONTACT_STATUS = "BorrowerContactStatus�";

    public static ContactCustomFieldInfoCollection GetBusinessCustomFields()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM BusinessCustomFieldPageNames");
      dbQueryBuilder.AppendLine("SELECT * FROM BusinessCustomFields");
      dbQueryBuilder.AppendLine("SELECT * FROM BusinessCustomFieldOptions");
      ContactCustomFieldInfoCollection customFields = ContactAccessor.getBusinessCustomFieldsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
      if (customFields == null)
      {
        ClientContext current = ClientContext.GetCurrent();
        using (current.Cache.Lock(nameof (ContactAccessor), LockType.ReadOnly))
          customFields = BizPartnerCustomFields.DeserializeResource(current);
        if (customFields == null || customFields.Items.Length == 0)
          return customFields;
        ContactAccessor.populateBusinessCustomFieldsDatabase(customFields);
        customFields = ContactAccessor.GetBusinessCustomFields();
      }
      return customFields;
    }

    private static ContactCustomFieldInfoCollection getBusinessCustomFieldsFromDataSet(
      DataSet dsBusinessCustomFields)
    {
      if (dsBusinessCustomFields == null || dsBusinessCustomFields.Tables[0].Rows.Count == 0 || dsBusinessCustomFields.Tables[1].Rows.Count == 0)
        return (ContactCustomFieldInfoCollection) null;
      DataRelation relation = dsBusinessCustomFields.Relations.Add("BusinessCustomFieldOption", dsBusinessCustomFields.Tables[1].Columns["FieldId"], dsBusinessCustomFields.Tables[2].Columns["FieldId"]);
      ContactCustomFieldInfoCollection fieldsFromDataSet = new ContactCustomFieldInfoCollection();
      fieldsFromDataSet.Page1Name = Convert.ToString(dsBusinessCustomFields.Tables[0].Rows[0]["Page1Name"]);
      fieldsFromDataSet.Page2Name = Convert.ToString(dsBusinessCustomFields.Tables[0].Rows[0]["Page2Name"]);
      fieldsFromDataSet.Page3Name = Convert.ToString(dsBusinessCustomFields.Tables[0].Rows[0]["Page3Name"]);
      fieldsFromDataSet.Page4Name = Convert.ToString(dsBusinessCustomFields.Tables[0].Rows[0]["Page4Name"]);
      fieldsFromDataSet.Page5Name = Convert.ToString(dsBusinessCustomFields.Tables[0].Rows[0]["Page5Name"]);
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = new ContactCustomFieldInfo[dsBusinessCustomFields.Tables[1].Rows.Count];
      for (int index = 0; index < dsBusinessCustomFields.Tables[1].Rows.Count; ++index)
      {
        List<string> stringList = new List<string>();
        foreach (DataRow childRow in dsBusinessCustomFields.Tables[1].Rows[index].GetChildRows(relation))
          stringList.Add(SQL.DecodeString(childRow["OptionValue"]));
        ContactCustomFieldInfo contactCustomFieldInfo = new ContactCustomFieldInfo(SQL.DecodeInt(dsBusinessCustomFields.Tables[1].Rows[index]["LabelId"]), SQL.DecodeString(dsBusinessCustomFields.Tables[1].Rows[index]["OwnerId"]), SQL.DecodeString(dsBusinessCustomFields.Tables[1].Rows[index]["Label"]), (FieldFormat) SQL.DecodeInt(dsBusinessCustomFields.Tables[1].Rows[index]["FieldType"]), SQL.DecodeString(dsBusinessCustomFields.Tables[1].Rows[index]["LoanFieldId"]), SQL.DecodeBoolean(dsBusinessCustomFields.Tables[1].Rows[index]["TwoWayTransfer"]), stringList.ToArray());
        contactCustomFieldInfoArray.SetValue((object) contactCustomFieldInfo, index);
      }
      fieldsFromDataSet.Items = contactCustomFieldInfoArray;
      return fieldsFromDataSet;
    }

    private static void populateBusinessCustomFieldsDatabase(
      ContactCustomFieldInfoCollection customFields)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("BusinessCustomFieldPageNames");
      DbTableInfo table2 = DbAccessManager.GetTable("BusinessCustomFields");
      DbValueList values = new DbValueList();
      values.Add("Page1Name", (object) SQL.DecodeString((object) customFields.Page1Name));
      values.Add("Page2Name", (object) SQL.DecodeString((object) customFields.Page2Name));
      values.Add("Page3Name", (object) SQL.DecodeString((object) customFields.Page3Name));
      values.Add("Page4Name", (object) SQL.DecodeString((object) customFields.Page4Name));
      values.Add("Page5Name", (object) SQL.DecodeString((object) customFields.Page5Name));
      dbQueryBuilder.DeleteFrom(table1);
      dbQueryBuilder.InsertInto(table1, values, true, false);
      dbQueryBuilder.Declare("@FieldId", "INT");
      dbQueryBuilder.DeleteFrom(table2);
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFields.Items)
      {
        dbQueryBuilder.InsertInto(table2, new DbValueList()
        {
          {
            "LabelId",
            (object) contactCustomFieldInfo.LabelID
          },
          {
            "OwnerId",
            (object) contactCustomFieldInfo.OwnerID
          },
          {
            "Label",
            (object) contactCustomFieldInfo.Label
          },
          {
            "FieldType",
            (object) (int) contactCustomFieldInfo.FieldType
          },
          {
            "LoanFieldId",
            (object) contactCustomFieldInfo.LoanFieldId
          },
          {
            "TwoWayTransfer",
            (object) (contactCustomFieldInfo.TwoWayTransfer ? 1 : 0)
          }
        }, true, false);
        dbQueryBuilder.SelectIdentity("@FieldId");
        foreach (string fieldOption in contactCustomFieldInfo.FieldOptions)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([FieldId], [OptionValue])", (object) "BusinessCustomFieldOptions"));
          dbQueryBuilder.AppendLine(string.Format("VALUES(@FieldId, {0})", (object) SQL.EncodeString(fieldOption)));
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void SetBusinessCustomFields(
      ContactCustomFieldInfoCollection customFieldsCollection)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (ContactAccessor)))
      {
        current.Cache.Remove(nameof (ContactAccessor));
        ContactAccessor.populateBusinessCustomFieldsDatabase(customFieldsCollection);
        XmlDataStore.Serialize((object) customFieldsCollection, Path.Combine(current.Settings.AppDataDir, "Users\\BizCustomFields.xml"));
        current.Cache.Put(nameof (ContactAccessor), (object) customFieldsCollection, CacheSetting.Low);
      }
    }

    public static ContactCustomFieldInfoCollection GetBorrowerCustomFields()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM BorrowerCustomFieldPageNames");
      dbQueryBuilder.AppendLine("SELECT * FROM BorrowerCustomFields");
      dbQueryBuilder.AppendLine("SELECT * FROM BorrowerCustomFieldOptions");
      ContactCustomFieldInfoCollection customFields = ContactAccessor.getBorrowerCustomFieldsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
      if (customFields == null)
      {
        ClientContext current = ClientContext.GetCurrent();
        using (current.Cache.Lock(nameof (ContactAccessor), LockType.ReadOnly))
          customFields = BorrowerCustomFields.DeserializeResource(current);
        if (customFields == null)
          return customFields;
        ContactAccessor.populateBorrowerCustomFieldsDatabase(customFields);
        customFields = ContactAccessor.GetBorrowerCustomFields();
      }
      return customFields;
    }

    private static ContactCustomFieldInfoCollection getBorrowerCustomFieldsFromDataSet(
      DataSet dsBorrowerCustomFields)
    {
      if (dsBorrowerCustomFields == null || dsBorrowerCustomFields.Tables[0].Rows.Count == 0 && dsBorrowerCustomFields.Tables[1].Rows.Count == 0)
        return (ContactCustomFieldInfoCollection) null;
      DataRelation relation = dsBorrowerCustomFields.Relations.Add("BorrowerCustomFieldOption", dsBorrowerCustomFields.Tables[1].Columns["FieldId"], dsBorrowerCustomFields.Tables[2].Columns["FieldId"]);
      ContactCustomFieldInfoCollection fieldsFromDataSet = new ContactCustomFieldInfoCollection();
      if (dsBorrowerCustomFields.Tables[0].Rows.Count > 0)
      {
        fieldsFromDataSet.Page1Name = Convert.ToString(dsBorrowerCustomFields.Tables[0].Rows[0]["Page1Name"]);
        fieldsFromDataSet.Page2Name = Convert.ToString(dsBorrowerCustomFields.Tables[0].Rows[0]["Page2Name"]);
        fieldsFromDataSet.Page3Name = Convert.ToString(dsBorrowerCustomFields.Tables[0].Rows[0]["Page3Name"]);
        fieldsFromDataSet.Page4Name = Convert.ToString(dsBorrowerCustomFields.Tables[0].Rows[0]["Page4Name"]);
        fieldsFromDataSet.Page5Name = Convert.ToString(dsBorrowerCustomFields.Tables[0].Rows[0]["Page5Name"]);
      }
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = new ContactCustomFieldInfo[dsBorrowerCustomFields.Tables[1].Rows.Count];
      for (int index = 0; index < dsBorrowerCustomFields.Tables[1].Rows.Count; ++index)
      {
        List<string> stringList = new List<string>();
        foreach (DataRow childRow in dsBorrowerCustomFields.Tables[1].Rows[index].GetChildRows(relation))
          stringList.Add(SQL.DecodeString(childRow["OptionValue"]));
        ContactCustomFieldInfo contactCustomFieldInfo = new ContactCustomFieldInfo(SQL.DecodeInt(dsBorrowerCustomFields.Tables[1].Rows[index]["LabelId"]), SQL.DecodeString(dsBorrowerCustomFields.Tables[1].Rows[index]["OwnerId"]), SQL.DecodeString(dsBorrowerCustomFields.Tables[1].Rows[index]["Label"]), (FieldFormat) SQL.DecodeInt(dsBorrowerCustomFields.Tables[1].Rows[index]["FieldType"]), SQL.DecodeString(dsBorrowerCustomFields.Tables[1].Rows[index]["LoanFieldId"]), SQL.DecodeBoolean(dsBorrowerCustomFields.Tables[1].Rows[index]["TwoWayTransfer"]), stringList.ToArray());
        contactCustomFieldInfoArray.SetValue((object) contactCustomFieldInfo, index);
      }
      fieldsFromDataSet.Items = contactCustomFieldInfoArray;
      return fieldsFromDataSet;
    }

    private static void populateBorrowerCustomFieldsDatabase(
      ContactCustomFieldInfoCollection customFields)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("BorrowerCustomFieldPageNames");
      DbTableInfo table2 = DbAccessManager.GetTable("BorrowerCustomFields");
      DbValueList values = new DbValueList();
      values.Add("Page1Name", (object) SQL.DecodeString((object) customFields.Page1Name));
      values.Add("Page2Name", (object) SQL.DecodeString((object) customFields.Page2Name));
      values.Add("Page3Name", (object) SQL.DecodeString((object) customFields.Page3Name));
      values.Add("Page4Name", (object) SQL.DecodeString((object) customFields.Page4Name));
      values.Add("Page5Name", (object) SQL.DecodeString((object) customFields.Page5Name));
      dbQueryBuilder.DeleteFrom(table1);
      dbQueryBuilder.InsertInto(table1, values, true, false);
      dbQueryBuilder.Declare("@FieldId", "INT");
      dbQueryBuilder.DeleteFrom(table2);
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFields.Items)
      {
        dbQueryBuilder.InsertInto(table2, new DbValueList()
        {
          {
            "LabelId",
            (object) contactCustomFieldInfo.LabelID
          },
          {
            "OwnerId",
            (object) contactCustomFieldInfo.OwnerID
          },
          {
            "Label",
            (object) contactCustomFieldInfo.Label
          },
          {
            "FieldType",
            (object) (int) contactCustomFieldInfo.FieldType
          },
          {
            "LoanFieldId",
            (object) contactCustomFieldInfo.LoanFieldId
          },
          {
            "TwoWayTransfer",
            (object) (contactCustomFieldInfo.TwoWayTransfer ? 1 : 0)
          }
        }, true, false);
        dbQueryBuilder.SelectIdentity("@FieldId");
        foreach (string fieldOption in contactCustomFieldInfo.FieldOptions)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([FieldId], [OptionValue])", (object) "BorrowerCustomFieldOptions"));
          dbQueryBuilder.AppendLine(string.Format("VALUES(@FieldId, {0})", (object) SQL.EncodeString(fieldOption)));
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void SetBorrowerCustomFields(
      ContactCustomFieldInfoCollection customFieldsCollection)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (ContactAccessor)))
      {
        current.Cache.Remove(nameof (ContactAccessor));
        ContactAccessor.populateBorrowerCustomFieldsDatabase(customFieldsCollection);
        XmlDataStore.Serialize((object) customFieldsCollection, current.Settings.GetDataFilePath("Users\\BorCustomFields.xml"));
        current.Cache.Put(nameof (ContactAccessor), (object) customFieldsCollection, CacheSetting.Low);
      }
    }

    public static BorrowerStatus GetBorrowerContactStatusList()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM BorrowerContactStatus");
      BorrowerStatus contactStatusList1 = ContactAccessor.getBorrowerContactStatusFromDataSet(dbQueryBuilder.ExecuteTableQuery());
      if (contactStatusList1 == null)
      {
        BorrowerStatus contactStatusList2 = BorrowerStatusStore.Get();
        if (contactStatusList2 == null || contactStatusList2.Items.Length == 0)
          return contactStatusList2;
        ContactAccessor.populateBorrowerContactStatusDatabase(contactStatusList2.Items);
        contactStatusList1 = ContactAccessor.GetBorrowerContactStatusList();
      }
      return contactStatusList1;
    }

    private static BorrowerStatus getBorrowerContactStatusFromDataSet(
      DataTable dtBorrowerContactStatus)
    {
      if (dtBorrowerContactStatus == null || dtBorrowerContactStatus.Rows.Count == 0)
        return (BorrowerStatus) null;
      BorrowerStatus statusFromDataSet = new BorrowerStatus();
      BorrowerStatusItem[] borrowerStatusItemArray = new BorrowerStatusItem[dtBorrowerContactStatus.Rows.Count];
      for (int index = 0; index < dtBorrowerContactStatus.Rows.Count; ++index)
      {
        BorrowerStatusItem borrowerStatusItem = new BorrowerStatusItem(SQL.DecodeString(dtBorrowerContactStatus.Rows[index]["Name"]), SQL.DecodeInt(dtBorrowerContactStatus.Rows[index]["StatusIndex"]));
        borrowerStatusItemArray.SetValue((object) borrowerStatusItem, index);
      }
      statusFromDataSet.Items = borrowerStatusItemArray;
      return statusFromDataSet;
    }

    private static void populateBorrowerContactStatusDatabase(BorrowerStatusItem[] statusItems)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("BorrowerContactStatus");
      dbQueryBuilder.DeleteFrom(table);
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      foreach (BorrowerStatusItem statusItem in statusItems)
        ContactAccessor.insertBorrowerStatusItemtoDatabase(statusItem, "");
    }

    private static void insertBorrowerStatusItemtoDatabase(BorrowerStatusItem item, string oldName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("BorrowerContactStatus");
      DbValueList values = new DbValueList();
      values.Add("Name", (object) item.name);
      values.Add("StatusIndex", (object) item.index);
      if (!string.IsNullOrEmpty(oldName))
        dbQueryBuilder.Update(table, values, new DbValue("Name", (object) oldName));
      else
        dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void CreateBorrowerStatus(BorrowerStatusItem item)
    {
      ContactAccessor.insertBorrowerStatusItemtoDatabase(item, "");
      BorrowerStatusStore.Create(item);
    }

    public static void SetBorrowerStatus(BorrowerStatusItem[] items)
    {
      ContactAccessor.populateBorrowerContactStatusDatabase(items);
      BorrowerStatusStore.Set(items);
    }

    public static void UpdateBorrowerStatusItem(int index, BorrowerStatusItem item, string oldName)
    {
      ContactAccessor.insertBorrowerStatusItemtoDatabase(item, oldName);
      BorrowerStatusStore.Update(index, item);
    }
  }
}
