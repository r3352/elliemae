// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TPOCompanyContactsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class TPOCompanyContactsAccessor
  {
    public static IList<ExternalOrgLenderContact> GetGlobalLenderContacts(int? externalOrgId = null)
    {
      IList<ExternalOrgLenderContact> globalLenderContacts = (IList<ExternalOrgLenderContact>) new List<ExternalOrgLenderContact>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT eolc.ContactId, eolc.ExternalOrgID, eolc.UserId, eolc.WholesaleChannel, eolc.DelegatedChannel, eolc.NonDelegatedChannel, eolc.UserId, 0 as Hide, CASE WHEN eolc.UserId IS null THEN eolc.Name ELSE (usr.FirstLastName) END AS Name, CASE WHEN eolc.UserId IS null THEN eolc.Title ELSE (usr.jobtitle) END AS Title, CASE WHEN eolc.UserId IS null THEN eolc.Phone ELSE (usr.phone) END AS Phone, CASE WHEN eolc.UserId IS null THEN eolc.Email ELSE (usr.email) END AS Email, eolc.DisplayOrder FROM ExternalOrgLenderContacts eolc LEFT JOIN users usr ON eolc.UserID = usr.userid WHERE eolc.ExternalOrgID IS NULL ORDER BY DisplayOrder";
      if (externalOrgId.HasValue)
        text = "SELECT eolc.ContactId, eolc.ExternalOrgID, eolc.UserId, eolc.WholesaleChannel, eolc.DelegatedChannel, eolc.NonDelegatedChannel, eolc.UserId, eoclc.Hide, CASE WHEN eolc.UserId IS null THEN eolc.Name ELSE (usr.FirstLastName) END AS Name, CASE WHEN eolc.UserId IS null THEN eolc.Title ELSE (usr.jobtitle) END AS Title, CASE WHEN eolc.UserId IS null THEN eolc.Phone ELSE (usr.phone) END AS Phone, CASE WHEN eolc.UserId IS null THEN eolc.Email ELSE (usr.email) END AS Email, CASE WHEN eoclc.DisplayOrder IS NOT null THEN  eoclc.DisplayOrder ELSE eolc.DisplayOrder END AS DisplayOrder FROM ExternalOrgLenderContacts eolc LEFT JOIN users usr ON eolc.UserID = usr.userid LEFT JOIN ExternalOrgCompanyLenderContacts eoclc ON eolc.ContactId = eoclc.ContactId AND eoclc.ContactSource = 0 WHERE eolc.ExternalOrgID IS NULL ORDER BY DisplayOrder";
      dbQueryBuilder.Append(text);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
        return globalLenderContacts;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        int int32_1 = Convert.ToInt32(row["ContactId"]);
        string str1 = Convert.ToString(row["UserId"]);
        string str2 = Convert.ToString(row["Name"]);
        string str3 = Convert.ToString(row["Title"]);
        string str4 = Convert.ToString(row["Phone"]);
        string str5 = Convert.ToString(row["Email"]);
        bool boolean1 = Convert.ToBoolean(row["WholesaleChannel"]);
        bool boolean2 = Convert.ToBoolean(row["DelegatedChannel"]);
        bool boolean3 = Convert.ToBoolean(row["NonDelegatedChannel"]);
        bool flag = !row.IsNull("Hide") && Convert.ToBoolean(row["Hide"]);
        int int32_2 = Convert.ToInt32(row["DisplayOrder"]);
        if (str3.Length > 30)
          str3 = str3.Substring(0, 30);
        ExternalOrgLenderContact orgLenderContact = new ExternalOrgLenderContact()
        {
          ContactID = int32_1,
          ExternalOrgID = externalOrgId,
          UserID = str1,
          Name = str2,
          Title = str3,
          Phone = str4,
          Email = str5,
          isWholesaleChannelEnabled = boolean1,
          isDelegatedChannelEnabled = boolean2,
          isNonDelegatedChannelEnabled = boolean3,
          DisplayOrder = int32_2,
          Source = ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts,
          isHidden = flag
        };
        globalLenderContacts.Add(orgLenderContact);
      }
      return globalLenderContacts;
    }

    public static IList<ExternalOrgLenderContact> GetLenderContacts(int externalOrgId)
    {
      IList<ExternalOrgLenderContact> lenderContacts = (IList<ExternalOrgLenderContact>) new List<ExternalOrgLenderContact>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT eolc.ContactId, eolc.ExternalOrgID, eolc.UserId, eolc.WholesaleChannel, eolc.DelegatedChannel, eolc.NonDelegatedChannel, eolc.UserId, eoclc.Hide, CASE WHEN eolc.UserId IS null THEN eolc.Name ELSE (usr.FirstLastName) END AS Name, CASE WHEN eolc.UserId IS null THEN eolc.Title ELSE (usr.jobtitle) END AS Title, CASE WHEN eolc.UserId IS null THEN eolc.Phone ELSE (usr.phone) END AS Phone, CASE WHEN eolc.UserId IS null THEN eolc.Email ELSE (usr.email) END AS Email, CASE WHEN eoclc.DisplayOrder IS NOT null THEN  eoclc.DisplayOrder ELSE eolc.DisplayOrder END AS DisplayOrder FROM ExternalOrgLenderContacts eolc LEFT JOIN users usr ON eolc.UserID = usr.userid LEFT JOIN ExternalOrgCompanyLenderContacts eoclc ON eolc.ContactId = eoclc.ContactId AND eoclc.ContactSource = 0 " + string.Format("WHERE eolc.ExternalOrgID IS null OR eolc.ExternalOrgID = {0} ", (object) SQL.Encode((object) externalOrgId)) + "ORDER BY DisplayOrder";
      dbQueryBuilder.Append(text);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
        return lenderContacts;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        int int32_1 = Convert.ToInt32(row["ContactId"]);
        string str1 = Convert.ToString(row["UserId"]);
        string str2 = Convert.ToString(row["Name"]);
        string str3 = Convert.ToString(row["Title"]);
        string str4 = Convert.ToString(row["Phone"]);
        string str5 = Convert.ToString(row["Email"]);
        bool boolean1 = Convert.ToBoolean(row["WholesaleChannel"]);
        bool boolean2 = Convert.ToBoolean(row["DelegatedChannel"]);
        bool boolean3 = Convert.ToBoolean(row["NonDelegatedChannel"]);
        bool flag = !row.IsNull("Hide") && Convert.ToBoolean(row["Hide"]);
        int int32_2 = Convert.ToInt32(row["DisplayOrder"]);
        if (str3.Length > 30)
          str3 = str3.Substring(0, 30);
        ExternalOrgLenderContact orgLenderContact = new ExternalOrgLenderContact()
        {
          ContactID = int32_1,
          ExternalOrgID = new int?(externalOrgId),
          UserID = str1,
          Name = str2,
          Title = str3,
          Phone = str4,
          Email = str5,
          isWholesaleChannelEnabled = boolean1,
          isDelegatedChannelEnabled = boolean2,
          isNonDelegatedChannelEnabled = boolean3,
          DisplayOrder = int32_2,
          Source = ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts,
          isHidden = flag
        };
        lenderContacts.Add(orgLenderContact);
      }
      return lenderContacts;
    }

    public static void UpdateLenderContact(ExternalOrgLenderContact contact)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string str1 = contact.ExternalOrgID.HasValue ? SQL.Encode((object) contact.ExternalOrgID) : "NULL";
        string str2 = string.IsNullOrWhiteSpace(contact.UserID) ? "NULL" : SQL.Encode((object) contact.UserID);
        stringBuilder.AppendLine("UPDATE ExternalOrgLenderContacts");
        stringBuilder.AppendLine("     SET ExternalOrgID = " + str1 + ", ");
        stringBuilder.AppendLine("     UserId = " + str2 + ", ");
        if (string.IsNullOrWhiteSpace(contact.UserID))
        {
          stringBuilder.AppendLine("     Name = '" + SQL.EncodeString(contact.Name, false) + "', ");
          stringBuilder.AppendLine("     Title = '" + SQL.EncodeString(contact.Title, false) + "', ");
          stringBuilder.AppendLine("     Phone = '" + SQL.EncodeString(contact.Phone, false) + "', ");
          stringBuilder.AppendLine("     Email = '" + SQL.EncodeString(contact.Email, false) + "', ");
        }
        stringBuilder.AppendLine("     WholesaleChannel = " + (contact.isWholesaleChannelEnabled ? "1" : "0") + ", ");
        stringBuilder.AppendLine("     DelegatedChannel = " + (contact.isDelegatedChannelEnabled ? "1" : "0") + ", ");
        stringBuilder.AppendLine("     NonDelegatedChannel = " + (contact.isNonDelegatedChannelEnabled ? "1" : "0") + ", ");
        stringBuilder.AppendLine("     DisplayOrder = " + (object) contact.DisplayOrder);
        stringBuilder.AppendLine("     WHERE ContactId = " + (object) contact.ContactID);
        stringBuilder.AppendLine(" select @@rowcount");
        dbQueryBuilder.Append(stringBuilder.ToString());
        if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) <= 0)
          throw new Exception(string.Format("No entry for ContactId, \"{0}\"", (object) contact.ContactID));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("No entry for ContactId, \"{0}\"", (object) contact.ContactID));
      }
    }

    public static void UpdateLenderContacts(ExternalOrgLenderContact[] contacts)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        foreach (ExternalOrgLenderContact contact in contacts)
        {
          string str1 = contact.ExternalOrgID.HasValue ? SQL.Encode((object) contact.ExternalOrgID) : "NULL";
          string str2 = string.IsNullOrWhiteSpace(contact.UserID) ? "NULL" : SQL.Encode((object) contact.UserID);
          stringBuilder.AppendLine("UPDATE ExternalOrgLenderContacts");
          stringBuilder.AppendLine("     SET ExternalOrgID = " + str1 + ", ");
          stringBuilder.AppendLine("     UserId = " + str2 + ", ");
          if (string.IsNullOrWhiteSpace(contact.UserID))
          {
            stringBuilder.AppendLine("     Name = '" + SQL.EncodeString(contact.Name, false) + "', ");
            stringBuilder.AppendLine("     Title = '" + SQL.EncodeString(contact.Title, false) + "', ");
            stringBuilder.AppendLine("     Phone = '" + SQL.EncodeString(contact.Phone, false) + "', ");
            stringBuilder.AppendLine("     Email = '" + SQL.EncodeString(contact.Email, false) + "', ");
          }
          stringBuilder.AppendLine("     WholesaleChannel = " + (contact.isWholesaleChannelEnabled ? "1" : "0") + ", ");
          stringBuilder.AppendLine("     DelegatedChannel = " + (contact.isDelegatedChannelEnabled ? "1" : "0") + ", ");
          stringBuilder.AppendLine("     NonDelegatedChannel = " + (contact.isNonDelegatedChannelEnabled ? "1" : "0") + ", ");
          stringBuilder.AppendLine("     DisplayOrder = " + (object) contact.DisplayOrder);
          stringBuilder.AppendLine("     WHERE ContactId = " + (object) contact.ContactID);
        }
        stringBuilder.AppendLine(" select @@rowcount");
        dbQueryBuilder.Append(stringBuilder.ToString());
        if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) < 1)
          throw new Exception("Error updating contacts");
      }
      catch (Exception ex)
      {
        throw new Exception("Error updating contacts", ex);
      }
    }

    public static string AddLenderContactQuery(ExternalOrgLenderContact contact, string rootOrgId)
    {
      string str1 = rootOrgId == null ? "@oid" : rootOrgId;
      string str2;
      if (string.IsNullOrWhiteSpace(contact.UserID))
        str2 = "INSERT INTO [ExternalOrgLenderContacts] ([ExternalOrgID],[UserId], [Name], [Title], [Phone],[Email], [WholesaleChannel], [DelegatedChannel], [NonDelegatedChannel], [DisplayOrder]) VALUES (" + str1 + "," + SQL.EncodeString(contact.UserID) + "," + SQL.EncodeString(contact.Name) + "," + SQL.EncodeString(contact.Title) + "," + SQL.EncodeString(contact.Phone) + "," + SQL.EncodeString(contact.Email) + "," + SQL.EncodeFlag(contact.isWholesaleChannelEnabled) + "," + SQL.EncodeFlag(contact.isDelegatedChannelEnabled) + "," + SQL.EncodeFlag(contact.isNonDelegatedChannelEnabled) + "," + SQL.Encode((object) (contact.ExternalOrgID.HasValue ? -1 : contact.DisplayOrder)) + ")";
      else
        str2 = "INSERT INTO [ExternalOrgLenderContacts] ([ExternalOrgID],[UserId], [WholesaleChannel], [DelegatedChannel], [NonDelegatedChannel], [DisplayOrder]) VALUES (" + str1 + "," + SQL.EncodeString(contact.UserID) + "," + SQL.EncodeFlag(contact.isWholesaleChannelEnabled) + "," + SQL.EncodeFlag(contact.isDelegatedChannelEnabled) + "," + SQL.EncodeFlag(contact.isNonDelegatedChannelEnabled) + "," + SQL.Encode((object) contact.DisplayOrder) + ")";
      return str2;
    }

    public static int AddLenderContact(ExternalOrgLenderContact contact)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@ContactId", "int");
      string rootOrgId = contact.ExternalOrgID.HasValue ? SQL.Encode((object) contact.ExternalOrgID) : (string) null;
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) rootOrgId);
      dbQueryBuilder.AppendLine(TPOCompanyContactsAccessor.AddLenderContactQuery(contact, rootOrgId));
      dbQueryBuilder.SelectIdentity("@ContactId");
      dbQueryBuilder.Select("@ContactId");
      try
      {
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        throw new Exception("TPOCompanyContactsDbAccessor: Cannot insert value to table 'ExternalOrgLenderContacts' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void DeleteLenderContact(int contactId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        stringBuilder.AppendLine("DELETE FROM ExternalOrgLenderContacts WHERE ContactId = " + (object) contactId);
        stringBuilder.AppendLine(" select @@rowcount");
        stringBuilder.AppendLine("DELETE FROM ExternalOrgCompanyLenderContacts WHERE ContactSource = 0 AND ContactId = " + (object) contactId);
        dbQueryBuilder.Append(stringBuilder.ToString());
        if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) <= 0)
          throw new Exception(string.Format("No entry for ContactId, {0}", (object) contactId));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("No entry for ContactId, {0}", (object) contactId));
      }
    }

    public static List<ExternalOrgLenderContact> GetTPOCompanyLenderContacts(int externalOrgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      Dictionary<string, Tuple<ExternalOrgLenderContact, int>> dictionary1 = new Dictionary<string, Tuple<ExternalOrgLenderContact, int>>();
      List<Tuple<ExternalOrgLenderContact, int>> tupleList1 = new List<Tuple<ExternalOrgLenderContact, int>>();
      stringBuilder.AppendLine("(select salesReps.salesRepId as ContactId, eoclc.ExternalOrgID, salesreps.userid, salesReps.DelegatedChannel, salesReps.NonDelegatedChannel, salesReps.WholesaleChannel,salesReps.jobtitle as Title,");
      stringBuilder.AppendLine("salesReps.Name, salesReps.phone, salesReps.email, ");
      stringBuilder.AppendLine("case when eoclc.DisplayOrder is null then (case when salesReps.isPrimarySales = 1 then -3 else -2 end) else eoclc.DisplayOrder end as DisplayOrder, ");
      stringBuilder.AppendLine("eoclc.Hide, salesReps.isPrimarySales, salesReps.ContactSource, ");
      stringBuilder.AppendLine("case when salesReps.isPrimarySales = 1 then -3 else -2 end as GlobalDisplayOrder from ");
      stringBuilder.AppendLine("(SELECT distinct salesRepId, esr.DelegatedChannel, esr.NonDelegatedChannel, esr.WholesaleChannel, esr.externalOrgID, u.UserID, u.jobtitle, u.phone,");
      stringBuilder.AppendLine("u.FirstLastName as Name, u.email, case when eod.PrimarySalesRepUserId = u.userid then 1 else 0 end as isPrimarySales, 1 as ContactSource FROM [ExternalOrgSalesReps] esr ");
      stringBuilder.AppendLine("inner join users u on u.userid = esr.UserID and u.status = 0 left join [ExternalOriginatorManagement] eom on eom.oid = esr.externalOrgID inner join ExternalOrgDetail eod on eod.externalOrgID = eom.oid ");
      stringBuilder.AppendLine("where esr.externalOrgID = " + SQL.Encode((object) externalOrgId) + " OR esr.externalOrgID in (select descendent from [ExternalOrgDescendents] where oid = " + (object) externalOrgId + ")) as salesReps ");
      stringBuilder.AppendLine("left join ExternalOrgCompanyLenderContacts eoclc on salesReps.salesRepId = eoclc.ContactId and eoclc.ExternalOrgID = " + (object) externalOrgId + " and eoclc.ContactSource = 1) ");
      stringBuilder.AppendLine("union ");
      stringBuilder.AppendLine("(select eolc.ContactId, eolc.ExternalOrgID, u.userid, eolc.DelegatedChannel, eolc.NonDelegatedChannel, eolc.WholesaleChannel,");
      stringBuilder.AppendLine("case when eolc.UserId is null then eolc.Title else u.jobtitle end as Title,");
      stringBuilder.AppendLine("case when eolc.UserId is null then eolc.Name else u.FirstLastName end as Name,");
      stringBuilder.AppendLine("case when eolc.UserId is null then eolc.Phone else (u.phone) end as phone, ");
      stringBuilder.AppendLine("case when eolc.UserId is null then eolc.Email else (u.email) end as email,");
      stringBuilder.AppendLine("case when eoclc.DisplayOrder is null then (case when eolc.ExternalOrgID is null then -1 else 0 end) else eoclc.DisplayOrder end as DisplayOrder,");
      stringBuilder.AppendLine("eoclc.Hide, 0 as isPrimarySales, 0 as ContactSource, eolc.DisplayOrder as GlobalDisplayOrder");
      stringBuilder.AppendLine("from ExternalOrgLenderContacts eolc ");
      stringBuilder.AppendLine("left join ExternalOrgCompanyLenderContacts eoclc on eoclc.ContactId = eolc.ContactId and eoclc.ExternalOrgID = " + SQL.Encode((object) externalOrgId));
      stringBuilder.AppendLine(" left join users u on eolc.UserID = u.userid  ");
      stringBuilder.AppendLine("where (eoclc.ContactSource = 0 or eoclc.ContactSource is null) AND (eolc.ExternalOrgID is null OR eolc.ExternalOrgID = " + SQL.Encode((object) externalOrgId) + " )) order by DisplayOrder asc, GlobalDisplayOrder asc, Name asc, Title asc");
      dbQueryBuilder.Append(stringBuilder.ToString());
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        string str = SQL.DecodeString(dataRow["Title"], "");
        if (str.Length > 30)
          str = str.Substring(0, 30);
        ExternalOrgLenderContact orgLenderContact = new ExternalOrgLenderContact()
        {
          UserID = SQL.DecodeString(dataRow["userid"], ""),
          ContactID = Convert.ToInt32(dataRow["ContactId"]),
          isDelegatedChannelEnabled = Convert.ToBoolean(dataRow["DelegatedChannel"]),
          isNonDelegatedChannelEnabled = Convert.ToBoolean(dataRow["NonDelegatedChannel"]),
          isWholesaleChannelEnabled = Convert.ToBoolean(dataRow["WholesaleChannel"]),
          Source = (ExternalOrgCompanyContactSourceTable) Convert.ToInt32(dataRow["ContactSource"]),
          Email = SQL.DecodeString(dataRow["email"], ""),
          isHidden = SQL.DecodeBoolean(dataRow["Hide"], false),
          Name = SQL.DecodeString(dataRow["Name"], ""),
          Phone = SQL.DecodeString(dataRow["phone"], ""),
          isPrimarySalesRep = Convert.ToBoolean(dataRow["isPrimarySales"]),
          Title = str,
          DisplayOrder = SQL.DecodeInt(dataRow["displayOrder"]),
          ExternalOrgID = SQL.DecodeInt(dataRow["ExternalOrgId"], -1) == -1 ? new int?() : new int?(SQL.DecodeInt(dataRow["ExternalOrgId"]))
        };
        int num = SQL.DecodeInt(dataRow["GlobalDisplayOrder"]);
        Tuple<ExternalOrgLenderContact, int> tuple = new Tuple<ExternalOrgLenderContact, int>(orgLenderContact, num);
        if (orgLenderContact.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps)
        {
          if (string.IsNullOrWhiteSpace(orgLenderContact.UserID) || !dictionary1.ContainsKey(orgLenderContact.UserID))
          {
            dictionary1.Add(orgLenderContact.UserID, tuple);
            tupleList1.Add(tuple);
          }
          else if (dictionary1.ContainsKey(orgLenderContact.UserID))
          {
            dictionary1[orgLenderContact.UserID].Item1.isWholesaleChannelEnabled |= orgLenderContact.isWholesaleChannelEnabled;
            dictionary1[orgLenderContact.UserID].Item1.isNonDelegatedChannelEnabled |= orgLenderContact.isNonDelegatedChannelEnabled;
            dictionary1[orgLenderContact.UserID].Item1.isDelegatedChannelEnabled |= orgLenderContact.isDelegatedChannelEnabled;
            dictionary1[orgLenderContact.UserID].Item1.isHidden |= orgLenderContact.isHidden;
            dictionary1[orgLenderContact.UserID].Item1.isPrimarySalesRep |= orgLenderContact.isPrimarySalesRep;
            if (dictionary1[orgLenderContact.UserID].Item1.DisplayOrder < 0 && orgLenderContact.DisplayOrder >= 0 || dictionary1[orgLenderContact.UserID].Item1.DisplayOrder > orgLenderContact.DisplayOrder)
              dictionary1[orgLenderContact.UserID].Item1.DisplayOrder = orgLenderContact.DisplayOrder;
            if (dictionary1[orgLenderContact.UserID].Item2 < 0 && orgLenderContact.DisplayOrder >= 0 || dictionary1[orgLenderContact.UserID].Item2 > num)
              dictionary1[orgLenderContact.UserID] = new Tuple<ExternalOrgLenderContact, int>(dictionary1[orgLenderContact.UserID].Item1, num);
          }
        }
        else
          tupleList1.Add(tuple);
      }
      if (tupleList1 != null && tupleList1.Count > 1)
      {
        bool flag1 = false;
        bool flag2 = false;
        Dictionary<int, ExternalOrgLenderContact> dictionary2 = new Dictionary<int, ExternalOrgLenderContact>();
        List<Tuple<ExternalOrgLenderContact, int>> tupleList2 = new List<Tuple<ExternalOrgLenderContact, int>>();
        List<Tuple<ExternalOrgLenderContact, int>> tupleList3 = new List<Tuple<ExternalOrgLenderContact, int>>();
        List<ExternalOrgLenderContact> companyLenderContacts = new List<ExternalOrgLenderContact>();
        foreach (Tuple<ExternalOrgLenderContact, int> tuple in tupleList1)
        {
          if (tuple.Item1.DisplayOrder <= 0)
          {
            flag1 = true;
            tupleList3.Add(tuple);
          }
          else
          {
            flag2 = true;
            tupleList2.Add(tuple);
          }
          companyLenderContacts.Add(tuple.Item1);
        }
        if (!flag1 || !flag2)
          return companyLenderContacts;
        int num1 = 0;
        int num2 = 0;
        int num3 = -1;
        for (int index = 0; index < tupleList2.Count; ++index)
        {
          Tuple<ExternalOrgLenderContact, int> tuple = tupleList2[index];
          if (tuple.Item1.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps)
          {
            num2 = index;
            if (tuple.Item1.isPrimarySalesRep)
              num1 = index;
          }
          if (num3 == -1 && tuple.Item1.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts)
            num3 = index;
        }
        if (num3 == -1)
          num3 = tupleList2.Count;
        foreach (Tuple<ExternalOrgLenderContact, int> tuple in tupleList3)
        {
          if (tuple.Item1.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps)
          {
            if (tuple.Item1.isPrimarySalesRep)
            {
              tupleList2.Insert(++num1, tuple);
              if (num2 >= num1)
                ++num2;
              if (num3 >= num1)
                ++num3;
            }
            else
            {
              tupleList2.Insert(++num2, tuple);
              if (num1 >= num2)
                ++num1;
              if (num3 >= num2)
                ++num3;
            }
          }
          else
          {
            bool flag3 = false;
            if (tuple.Item2 >= 0)
            {
              for (int index = num3; index < tupleList2.Count; ++index)
              {
                if (tupleList2[index].Item2 > tuple.Item2)
                {
                  flag3 = true;
                  tupleList2.Insert(index, tuple);
                  if (num1 >= index)
                    ++num1;
                  if (num2 >= index)
                  {
                    ++num2;
                    break;
                  }
                  break;
                }
              }
            }
            if (!flag3)
              tupleList2.Add(tuple);
          }
        }
        List<ExternalOrgLenderContact> contacts = new List<ExternalOrgLenderContact>();
        for (int index = 0; index < tupleList2.Count; ++index)
        {
          Tuple<ExternalOrgLenderContact, int> tuple = tupleList2[index];
          tuple.Item1.DisplayOrder = index + 1;
          contacts.Add(tuple.Item1);
        }
        TPOCompanyContactsAccessor.UpdateTPOCompanyLenderContacts(externalOrgId, (IList<ExternalOrgLenderContact>) contacts);
        return contacts;
      }
      if (tupleList1 == null || tupleList1.Count != 1)
        return new List<ExternalOrgLenderContact>();
      return new List<ExternalOrgLenderContact>()
      {
        tupleList1[0].Item1
      };
    }

    public static void UpdateTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("IF NOT EXISTS(SELECT 1 FROM ExternalOrgCompanyLenderContacts where ExternalOrgId = " + (object) externalOrgId + " and ContactId = " + (object) contactID + " and ContactSource = " + (object) (int) source + ")");
        stringBuilder.AppendLine("BEGIN");
        stringBuilder.AppendLine("insert into ExternalOrgCompanyLenderContacts(ExternalOrgId, ContactSource, ContactId, Hide, DisplayOrder) VALUES(" + SQL.Encode((object) externalOrgId) + "," + (object) (int) source + ", " + (object) contactID + "," + (object) hide + "," + (object) displayOrder + ")");
        stringBuilder.AppendLine("END");
        stringBuilder.AppendLine("ELSE");
        stringBuilder.AppendLine("BEGIN");
        stringBuilder.AppendLine("UPDATE ExternalOrgCompanyLenderContacts SET Hide = " + (object) hide + ", DisplayOrder = " + (object) displayOrder + " WHERE ExternalOrgId = " + (object) externalOrgId + " and ContactId = " + (object) contactID + " and ContactSource = " + (object) (int) source);
        stringBuilder.AppendLine("END");
        dbQueryBuilder.Append(stringBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise("TPOCompanyLenderContactAccessor", ex);
      }
    }

    public static void UpdateTPOCompanyLenderContacts(
      int externalOrgId,
      IList<ExternalOrgLenderContact> contacts)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (ExternalOrgLenderContact contact in (IEnumerable<ExternalOrgLenderContact>) contacts)
        {
          stringBuilder.AppendLine("IF NOT EXISTS (SELECT 1 FROM ExternalOrgCompanyLenderContacts where ExternalOrgId = " + SQL.Encode((object) externalOrgId) + " and ContactId = " + (object) contact.ContactID + " and ContactSource = " + (object) (int) contact.Source + ")");
          stringBuilder.AppendLine("BEGIN");
          stringBuilder.AppendLine("insert into ExternalOrgCompanyLenderContacts (ExternalOrgId, ContactSource, ContactId, Hide, DisplayOrder) VALUES(" + SQL.Encode((object) externalOrgId) + ", " + (object) (int) contact.Source + ", " + (object) contact.ContactID + ", " + (contact.isHidden ? (object) "1" : (object) "0") + ", " + (object) contact.DisplayOrder + ")");
          stringBuilder.AppendLine("END");
          stringBuilder.AppendLine("ELSE");
          stringBuilder.AppendLine("BEGIN");
          stringBuilder.AppendLine("UPDATE ExternalOrgCompanyLenderContacts SET Hide = " + (contact.isHidden ? (object) "1" : (object) "0") + ", DisplayOrder = " + (object) contact.DisplayOrder + " WHERE ExternalOrgId = " + SQL.Encode((object) externalOrgId) + " and ContactId = " + (object) contact.ContactID + " and ContactSource = " + (object) (int) contact.Source);
          stringBuilder.AppendLine("END");
        }
        dbQueryBuilder.Append(stringBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise("TPOCompanyLenderContactAccessor", ex);
      }
    }

    public static string AddTPOCompanyLenderContactQuery(
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder,
      string rootOrgId)
    {
      return "INSERT INTO [ExternalOrgCompanyLenderContacts] ([ExternalOrgID],[ContactSource], [ContactID], [Hide], [DisplayOrder]) VALUES (" + (rootOrgId == null ? "@oid" : rootOrgId) + "," + SQL.Encode((object) (int) source) + ",@ContactId, " + SQL.Encode((object) hide) + "," + SQL.Encode((object) displayOrder) + ")";
    }

    public static void AddTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@ContactId", "int");
      dbQueryBuilder.SelectVar("@ContactId", (object) contactID);
      dbQueryBuilder.AppendLine(TPOCompanyContactsAccessor.AddTPOCompanyLenderContactQuery(source, hide, displayOrder, externalOrgId.ToString()));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw new Exception("TPOCompanyContactsDbAccessor: Cannot insert value to table 'ExternalOrgCompanyLenderContacts' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static Dictionary<string, string> GetTpoLoanOfficer(string orgId, int realWorldRoleId)
    {
      Dictionary<string, string> tpoLoanOfficer = new Dictionary<string, string>();
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine(" select distinct  eu.ContactID,eu.First_name,eu.Middle_name,eu.Last_name ");
        dbQueryBuilder.AppendLine(" from ExternalUsers  eu inner join ExternalOrgDetail eod on eu.externalOrgID=eod.externalOrgID inner join ExternalOriginatorManagement eom on eod.externalOrgID=eom.oid inner join   UserPersona u on eu.ContactID=u.userid  ");
        dbQueryBuilder.AppendLine(" inner join Personas p on u.personaID=p.personaID");
        dbQueryBuilder.AppendLine(" inner join RolePersonas rp on p.personaID=rp.personaID ");
        dbQueryBuilder.AppendLine(" inner join RolesMapping rm on rp.roleID=rm.roleID");
        dbQueryBuilder.AppendLine(" where eom.ExternalID=" + SQL.Encode((object) orgId));
        dbQueryBuilder.AppendLine(" and  rm.realWorldRoleID=  " + SQL.Encode((object) realWorldRoleId));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          tpoLoanOfficer.Add(dataRow["ContactID"].ToString(), dataRow["First_name"].ToString() + " " + dataRow["Middle_name"].ToString() + " " + dataRow["Last_name"].ToString() + " (" + dataRow["ContactID"].ToString() + ")");
        return tpoLoanOfficer;
      }
      catch (Exception ex)
      {
        Err.Reraise("TPOCompanyLenderContactAccessor", ex);
        return tpoLoanOfficer;
      }
    }
  }
}
