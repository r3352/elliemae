// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BorrowerOpportunity
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class BorrowerOpportunity
  {
    private const string className = "BorrowerOpportunity�";

    public static void UpdateBorrowerOpportunity(Opportunity item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Update(DbAccessManager.GetTable("Opportunity"), BorrowerOpportunity.createDbValueList(item), new DbValue("OpportunityID", (object) item.OpportunityID));
      dbQueryBuilder.Update(DbAccessManager.GetTable("Borrower"), BorrowerOpportunity.createLastModifiedDbValueList(), new DbValue("ContactID", (object) item.ContactID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList createLastModifiedDbValueList()
    {
      return new DbValueList()
      {
        {
          "LastModified",
          (object) DateTime.Now
        }
      };
    }

    private static DbValueList createDbValueList(Opportunity item)
    {
      return new DbValueList()
      {
        {
          "ContactID",
          (object) item.ContactID
        },
        {
          "LoanAmount",
          (object) item.LoanAmount
        },
        {
          "Purpose",
          (object) LoanPurposeEnumUtil.ValueToName(item.Purpose)
        },
        {
          "PurposeOther",
          (object) item.PurposeOther
        },
        {
          "LoanType",
          (object) LoanTypeEnumUtil.ValueToName(item.LoanType)
        },
        {
          "TypeOther",
          (object) item.TypeOther
        },
        {
          "EstimatedCreditScore",
          (object) item.EstimatedCreditScore
        },
        {
          "Term",
          (object) item.Term
        },
        {
          "Amortization",
          (object) AmortizationTypeEnumUtil.ValueToName(item.Amortization)
        },
        {
          "DownPayment",
          (object) item.DownPayment
        },
        {
          "PropertyAddress",
          (object) item.PropertyAddress.Street1
        },
        {
          "PropertyCity",
          (object) item.PropertyAddress.City
        },
        {
          "PropertyState",
          (object) item.PropertyAddress.State
        },
        {
          "PropertyZip",
          (object) item.PropertyAddress.Zip
        },
        {
          "PropertyUse",
          (object) PropertyUseEnumUtil.ValueToName(item.PropUse)
        },
        {
          "PropertyType",
          (object) PropertyTypeEnumUtil.ValueToName(item.PropType)
        },
        {
          "PropertyValue",
          (object) item.PropertyValue
        },
        {
          "PurchaseDate",
          (object) item.PurchaseDate,
          (IDbEncoder) DbEncoding.ShortDateTime
        },
        {
          "MortgageBalance",
          (object) item.MortgageBalance
        },
        {
          "MortgageRate",
          (object) item.MortgageRate
        },
        {
          "HousingPayment",
          (object) item.HousingPayment
        },
        {
          "NonHousingPayment",
          (object) item.NonhousingPayment
        },
        {
          "CreditRating",
          (object) item.CreditRating
        },
        {
          "Bankruptcy",
          (object) item.IsBankruptcy,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "Employment",
          (object) EmploymentStatusEnumUtil.ValueToName(item.Employment)
        },
        {
          "CashOut",
          (object) item.CashOut
        }
      };
    }

    public static void DeleteBorrowerOpportunity(int itemId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("if exists (select 1 from Opportunity, Borrower where Opportunity.ContactID = Borrower.ContactID and OpportunityID = " + (object) itemId + ")");
      dbQueryBuilder.AppendLine("   update Borrower set LastModified = " + SQL.EncodeDateTime(DateTime.Now) + " where ContactID = (select ContactID from Opportunity where OpportunityID = " + (object) itemId + ")");
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Opportunity"), new DbValue("OpportunityID", (object) itemId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteOpportunityByBorrowerId(int contactId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Opportunity"), new DbValue("ContactID", (object) contactId));
      dbQueryBuilder.Update(DbAccessManager.GetTable("Borrower"), BorrowerOpportunity.createLastModifiedDbValueList(), new DbValue("ContactID", (object) contactId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int CreateBorrowerOpportunity(Opportunity item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable("Opportunity"), BorrowerOpportunity.createDbValueList(item), true, false);
      dbQueryBuilder.SelectIdentity();
      dbQueryBuilder.Update(DbAccessManager.GetTable("Borrower"), BorrowerOpportunity.createLastModifiedDbValueList(), new DbValue("ContactID", (object) item.ContactID));
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static Opportunity GetBorrowerOpportunity(int opportunityId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Opportunity"), new DbValue("OpportunityID", (object) opportunityId));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count == 0 ? (Opportunity) null : BorrowerOpportunity.dataRowToBorrowerOpportunity(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerOpportunity), ex);
        return (Opportunity) null;
      }
    }

    public static Opportunity GetOpportunityByBorrowerId(int borrowerId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Opportunity"), new DbValue("ContactID", (object) borrowerId));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count == 0 ? (Opportunity) null : BorrowerOpportunity.dataRowToBorrowerOpportunity(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerOpportunity), ex);
        return (Opportunity) null;
      }
    }

    private static Opportunity dataRowToBorrowerOpportunity(DataRow row)
    {
      return new Opportunity((int) row["OpportunityID"], (int) SQL.Decode(row["ContactID"], (object) -1), Convert.ToDecimal(SQL.Decode(row["LoanAmount"], (object) 0)), LoanPurposeEnumUtil.NameToValue(SQL.Decode(row["Purpose"], (object) "").ToString().Trim()), SQL.Decode(row["PurposeOther"], (object) "").ToString().Trim(), (int) SQL.Decode(row["Term"], (object) -1), AmortizationTypeEnumUtil.NameToValue(SQL.Decode(row["Amortization"], (object) "").ToString().Trim()), Convert.ToDecimal(SQL.Decode(row["DownPayment"], (object) 0)), new Address(SQL.Decode(row["PropertyAddress"], (object) "").ToString().Trim(), string.Empty, SQL.Decode(row["PropertyCity"], (object) "").ToString().Trim(), SQL.Decode(row["PropertyState"], (object) "").ToString().Trim(), SQL.Decode(row["PropertyZip"], (object) "").ToString().Trim()), PropertyUseEnumUtil.NameToValue(SQL.Decode(row["PropertyUse"], (object) "").ToString().Trim()), PropertyTypeEnumUtil.NameToValue(SQL.Decode(row["PropertyType"], (object) "").ToString().Trim()), Convert.ToDecimal(SQL.Decode(row["PropertyValue"], (object) 0)), (DateTime) SQL.Decode(row["PurchaseDate"], (object) DateTime.MinValue), Convert.ToDecimal(SQL.Decode(row["MortgageBalance"], (object) 0)), Convert.ToDecimal(SQL.Decode(row["MortgageRate"], (object) 0)), Convert.ToDecimal(SQL.Decode(row["HousingPayment"], (object) 0)), Convert.ToDecimal(SQL.Decode(row["NonHousingPayment"], (object) 0)), SQL.Decode(row["CreditRating"], (object) "").ToString().Trim(), (bool) SQL.Decode(row["Bankruptcy"], (object) false), EmploymentStatusEnumUtil.NameToValue(SQL.Decode(row["Employment"], (object) "").ToString().Trim()), Convert.ToDecimal(SQL.Decode(row["CashOut"], (object) 0)), LoanTypeEnumUtil.NameToValue(SQL.Decode(row["LoanType"], (object) "").ToString().Trim()), SQL.Decode(row["TypeOther"], (object) "").ToString().Trim(), SQL.Decode(row["EstimatedCreditScore"], (object) "").ToString().Trim());
    }
  }
}
