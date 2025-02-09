// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactLoans
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ContactLoans
  {
    private const string className = "ContactLoans�";

    private ContactLoans()
    {
    }

    public static int AddLoan(ContactLoanInfo info)
    {
      DbValueList values = new DbValueList();
      values.Add("BorrowerID", (object) info.BorrowerID);
      values.Add("LoanStatus", (object) info.LoanStatus);
      values.Add("AppraisedValue", (object) info.AppraisedValue);
      values.Add("LoanAmount", (object) info.LoanAmount);
      values.Add("InterestRate", (object) info.InterestRate);
      values.Add("Term", (object) info.Term);
      values.Add("Purpose", (object) LoanPurposeEnumUtil.ValueToNameInLoan(info.Purpose));
      values.Add("DownPayment", (object) info.DownPayment);
      values.Add("LTV", (object) info.LTV);
      values.Add("Amortization", (object) AmortizationTypeEnumUtil.ValueToNameInLoan(info.Amortization));
      values.Add("DateCompleted", (object) info.DateCompleted);
      values.Add("LoanType", (object) LoanTypeEnumUtil.ValueToNameInLoan(info.LoanType));
      values.Add("LienPosition", (object) LienEnumUtil.ValueToNameInLoan(info.LienPosition));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable("Loan"), values, true, false);
      dbQueryBuilder.SelectIdentity();
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static ContactLoanInfo GetLoan(int loanId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Loan"), new DbValue("LoanID", (object) loanId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ContactLoanInfo) null : ContactLoans.dataRowToContactLoanInfo(dataRowCollection[0]);
    }

    public static ContactLoanInfo[] GetBorrowerLoans(int borrowerId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Loan"), new DbValue("BorrowerID", (object) borrowerId));
      return ContactLoans.rowCollectionToLoanInfos(dbQueryBuilder.Execute());
    }

    public static ContactLoanInfo GetLastClosedLoanForContact(
      int contactId,
      ContactType contactType)
    {
      try
      {
        string str = contactType == ContactType.BizPartner ? "BizPartnerHistory" : "BorrowerHistory";
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from Loan where");
        dbQueryBuilder.AppendLine("   LoanID = (select Top 1 Loan.LoanID from Loan, " + str + " History");
        dbQueryBuilder.AppendLine("                 where  (History.LoanID = Loan.LoanID)");
        dbQueryBuilder.AppendLine("                        and (History.ContactID = " + (object) contactId + ")");
        dbQueryBuilder.AppendLine("                        and (History.EventType = 'Completed Loan')");
        dbQueryBuilder.AppendLine("                        order by Loan.DateCompleted desc)");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count == 0 ? (ContactLoanInfo) null : ContactLoans.dataRowToContactLoanInfo(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactLoans), ex);
        return (ContactLoanInfo) null;
      }
    }

    private static ContactLoanInfo dataRowToContactLoanInfo(DataRow r)
    {
      return new ContactLoanInfo((int) r["LoanID"], (int) SQL.Decode(r["BorrowerID"], (object) -1), string.Concat(r["LoanStatus"]), Convert.ToDecimal(SQL.Decode(r["AppraisedValue"], (object) 0)), Convert.ToDecimal(SQL.Decode(r["LoanAmount"], (object) 0)), Convert.ToDecimal(SQL.Decode(r["InterestRate"], (object) 0)), (int) SQL.Decode(r["Term"], (object) 0), LoanPurposeEnumUtil.NameInLoanToValue(string.Concat(r["Purpose"])), Convert.ToDecimal(SQL.Decode(r["DownPayment"], (object) 0)), Convert.ToDecimal(SQL.Decode(r["LTV"], (object) 0)), AmortizationTypeEnumUtil.NameInLoanToValue(string.Concat(r["Amortization"])), (DateTime) SQL.Decode(r["DateCompleted"], (object) DateTime.MinValue), LoanTypeEnumUtil.NameInLoanToValue(string.Concat(r["LoanType"])), LienEnumUtil.NameInLoanToValue(string.Concat(r["LienPosition"])));
    }

    private static ContactLoanInfo[] rowCollectionToLoanInfos(DataRowCollection rows)
    {
      ContactLoanInfo[] loanInfos = new ContactLoanInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        loanInfos[index] = ContactLoans.dataRowToContactLoanInfo(rows[index]);
      return loanInfos;
    }
  }
}
