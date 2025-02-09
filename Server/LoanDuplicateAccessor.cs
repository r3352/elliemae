// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanDuplicateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  public sealed class LoanDuplicateAccessor
  {
    private const string className = "LoanDuplicateAccessor�";
    public static readonly int MaxLength = 18;

    private LoanDuplicateAccessor()
    {
    }

    public static List<LoanDuplicateChecker> GetLoanDuplicateInfo(
      string guid,
      string firstName,
      string lastName,
      string ssn,
      string homePhone,
      string cellPhone,
      string email,
      string workEmail,
      Address address,
      string loanFolder)
    {
      if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(homePhone) && string.IsNullOrWhiteSpace(cellPhone) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(workEmail) && string.IsNullOrWhiteSpace(ssn))
      {
        if (address == null)
          return new List<LoanDuplicateChecker>();
        if (string.IsNullOrWhiteSpace(address.City) && string.IsNullOrWhiteSpace(address.State) && string.IsNullOrWhiteSpace(address.Street1) && string.IsNullOrWhiteSpace(address.Street2) && string.IsNullOrWhiteSpace(address.Zip))
          return new List<LoanDuplicateChecker>();
      }
      if (string.IsNullOrWhiteSpace(loanFolder))
        loanFolder = "''My Pipeline''";
      firstName = firstName.Replace("'", "''");
      lastName = lastName.Replace("'", "''");
      email = email.Replace("'", "''");
      workEmail = workEmail.Replace("'", "''");
      address.Street1 = address.Street1.Replace("'", "''");
      address.Street2 = address.Street2.Replace("'", "''");
      address.City = address.City.Replace("'", "''");
      string str = string.IsNullOrEmpty(ssn) ? string.Empty : SQL.EncodeToSHA1(ssn);
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("exec GetDupLoanCheckValues  " + SQL.Encode((object) guid) + ", " + SQL.Encode((object) address.Street1) + ", " + SQL.Encode((object) address.Street2) + ", " + SQL.Encode((object) address.City) + ", " + SQL.Encode((object) address.State) + ", " + SQL.Encode((object) address.Zip) + ", " + SQL.Encode((object) firstName) + ", " + SQL.Encode((object) lastName) + ", '" + loanFolder + "', " + SQL.Encode((object) homePhone) + ", " + SQL.Encode((object) cellPhone) + ", " + SQL.Encode((object) email) + ", " + SQL.Encode((object) workEmail) + ", '" + str + "'");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return new List<LoanDuplicateChecker>();
        List<LoanDuplicateChecker> source = new List<LoanDuplicateChecker>();
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        {
          LoanDuplicateChecker check = LoanDuplicateAccessor.dataRowToLoanDuplicateChecker(r);
          if (source.FirstOrDefault<LoanDuplicateChecker>((System.Func<LoanDuplicateChecker, bool>) (item => item.GUID == check.GUID)) == null)
            source.Add(check);
        }
        return source;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicateAccessor), ex);
        return (List<LoanDuplicateChecker>) null;
      }
    }

    public static void saveDuplicate(string loanGuid, string duplicateGuid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select duplicatesGuid from DuplicateLoansTracker where LoanGuid =  '" + loanGuid + "'");
        if (dbQueryBuilder.Execute().Count == 0)
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("insert into DuplicateLoansTracker values ('" + loanGuid + "' ,'" + duplicateGuid + "')");
        }
        else
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("update DuplicateLoansTracker set duplicatesGuid = '" + duplicateGuid + "' where LoanGuid = '" + loanGuid + "'");
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicateAccessor), ex);
      }
    }

    public static string GetDuplicates(string loanGuid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select duplicatesGuid from DuplicateLoansTracker where LoanGuid =  '" + loanGuid + "'");
        return dbQueryBuilder.Execute()[0]["DuplicatesGuid"].ToString();
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private static LoanDuplicateChecker dataRowToLoanDuplicateChecker(DataRow r)
    {
      Address subjectProperty = new Address(r["Address1"].ToString(), r["Address2"].ToString(), r["City"].ToString(), r["State"].ToString(), r["Zip"].ToString());
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select UserID from loanAssociates la where Guid = '" + r["Guid"].ToString() + "' and MilestoneID = '1'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      string fileStarter = dataRowCollection.Count > 0 ? dataRowCollection[0]["UserID"].ToString() : "";
      return new LoanDuplicateChecker(new Guid(r["Guid"].ToString()), r["firstName"].ToString(), r["LastName"].ToString(), r["workphone"].ToString(), r["homephone"].ToString(), r["cellphone"].ToString(), r["email"].ToString(), r["workEmail"].ToString(), r["ssn"].ToString(), subjectProperty, LoanTypeEnumUtil.NameToValue(r["loanType"].ToString()), SQL.DecodeDateTime(r["DateCreated"]), fileStarter, r["LoanOfficerID"].ToString(), r["LoanFolder"].ToString(), r["LoanAmount"].ToString().Trim() != "" ? Convert.ToInt64(r["LoanAmount"]) : long.MinValue, (LoanStatusMap.LoanStatus) SQL.DecodeInt(r["loanStatus"]));
    }

    private static string getXmlFilePath(string userid, string loanFolder, string loanName)
    {
      ClientContext current = ClientContext.GetCurrent();
      return loanFolder == null || loanName == null ? (string) null : current.Settings.GetLoansFilePath(loanFolder + "\\" + loanName + "\\DuplicateLoanCheck.xml", false);
    }

    public static DuplicateScreenSetting GetDuplicateScreenSetting(
      string userid,
      string loanFolder,
      string loanName)
    {
      string xmlFilePath = LoanDuplicateAccessor.getXmlFilePath(userid, loanFolder, loanName);
      return xmlFilePath == null ? (DuplicateScreenSetting) XmlDataStore.CreateNew(typeof (DuplicateScreenSetting)) : (DuplicateScreenSetting) XmlDataStore.Deserialize(typeof (DuplicateScreenSetting), xmlFilePath);
    }

    public static void SaveDuplicateScreenSetting(
      string userid,
      string loanFolder,
      string loanName,
      DuplicateScreenSetting setting)
    {
      string xmlFilePath = LoanDuplicateAccessor.getXmlFilePath(userid, loanFolder, loanName);
      XmlDataStore.Serialize((object) setting, xmlFilePath);
    }
  }
}
