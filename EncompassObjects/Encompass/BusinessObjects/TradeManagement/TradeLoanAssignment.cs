// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents a loan assignment in trade management.</summary>
  /// <remarks>When assign a loan to a correspondent trade, this object will be created to carry the details of loan assignment.
  /// </remarks>
  public class TradeLoanAssignment
  {
    /// <summary>Gets and sets trade assignment status</summary>
    [Field("TradeAssignment.Status")]
    [Description("Trade Assignment Status")]
    public string TradeAssignmentStatus { get; set; }

    /// <summary>Gets and sets trade assignment status date</summary>
    [Field("TradeAssignment.StatusDate")]
    [Description("Trade Assignment Status Date")]
    public DateTime TradeAssignmentStatusDate { get; set; }

    /// <summary>Gets and sets the identifier of trade</summary>
    [Field("TradeAssignment.TradeID")]
    [Description("Trade Assignment ID")]
    public DateTime TradeAssignementId { get; set; }

    /// <summary>Gets and sets the loan number</summary>
    [Field("Loan.LoanNumber")]
    [Description("Loan Number")]
    public string LoanNumber { get; set; }

    /// <summary>Gets and sets total buy price of a loan</summary>
    [Field("Loan.TotalBuyPrice")]
    [Description("Loan Trade Total Buy Price")]
    public Decimal TotalBuyPrice { get; set; }

    /// <summary>Gets and sets total sell price of a loan</summary>
    [Field("Loan.TotalSellPrice")]
    [Description("Loan Trade Total Sell Price")]
    public Decimal TotalSellPrice { get; set; }

    /// <summary>Gets and sets gain loss amount of a loan</summary>
    [Field("Loan.NetProfit")]
    [Description("Gain/Loss")]
    public Decimal NetProfit { get; set; }

    /// <summary>Gets and sets loan program of a loan</summary>
    [Field("Loan.LoanProgram")]
    [Description("Loan Program")]
    public string LoanProgram { get; set; }

    /// <summary>Gets and sets lastest finished milestone of a loan</summary>
    [Field("Loan.CurrentMilestoneName")]
    [Description("Last Finished Milestone")]
    public string CurrentMilestoneName { get; set; }

    /// <summary>Gets and sets total loan amount of a loan</summary>
    [Field("Loan.TotalLoanAmount")]
    [Description("Loan Amount")]
    public Decimal TotalLoanAmount { get; set; }

    /// <summary>Gets and sets loan amount of a loan</summary>
    [Field("Loan.LoanAmount")]
    [Description("Loan Amount")]
    public Decimal LoanAmount { get; set; }

    /// <summary>Gets and sets note rate of a loan</summary>
    [Field("Loan.LoanRate")]
    [Description("Note Rate")]
    public Decimal NoteRate { get; set; }

    /// <summary>Gets and sets term of a loan</summary>
    [Field("Loan.Term")]
    [Description("Term")]
    public int Term { get; set; }

    /// <summary>Gets and sets LTV of a loan</summary>
    [Field("Loan.LTV")]
    [Description("LTV")]
    public Decimal Ltv { get; set; }

    /// <summary>Gets and sets CLTV of a loan</summary>
    [Field("Loan.CLTV")]
    [Description("CLTV")]
    public Decimal Cltv { get; set; }

    /// <summary>Gets and sets DTI Top of a loan</summary>
    [Field("Loan.DTITop")]
    [Description("DTI Top")]
    public Decimal DtiTop { get; set; }

    /// <summary>Gets and sets DTI Bottom of a loan</summary>
    [Field("Loan.DTIBottom")]
    [Description("DTI Bottom")]
    public Decimal DtiBottom { get; set; }

    /// <summary>Gets and sets FICO of a loan</summary>
    [Field("Loan.CreditScore")]
    [Description("FICO")]
    public int Fico { get; set; }

    /// <summary>Gets and sets occupancy type of a loan</summary>
    [Field("Loan.OccupancyStatus")]
    [Description("Occupancy Type")]
    public string OccupancyType { get; set; }

    /// <summary>Gets and sets property type of a loan</summary>
    [Field("Loan.PropertyType")]
    [Description("Property Type")]
    public string PropertyType { get; set; }

    /// <summary>Gets and sets state of the loan assignment</summary>
    [Field("Loan.State")]
    [Description("State")]
    public string State { get; set; }

    /// <summary>Gets and sets lock expiration date of a loan</summary>
    [Field("Loan.LockExpirationDate")]
    [Description("Lock Expiration Date")]
    public DateTime LockExpirationDate { get; set; }

    /// <summary>Gets and sets borrower last name of a loan</summary>
    [Field("Loan.BorrowerLastName")]
    [Description("Borrower Last Name")]
    public string BorrowerLastName { get; set; }

    /// <summary>Constructor of TradeLoanAssignment</summary>
    /// <param name="trade"></param>
    /// <param name="pipelineInfo"></param>
    internal TradeLoanAssignment(CorrespondentTradeInfo trade, PipelineInfo pipelineInfo)
    {
      foreach (PropertyInfo property in typeof (TradeLoanAssignment).GetProperties())
      {
        object obj = ((IEnumerable<object>) property.GetCustomAttributes(false)).FirstOrDefault<object>((Func<object, bool>) (a => a.GetType() == typeof (FieldAttribute)));
        if (obj != null)
        {
          string name = ((FieldAttribute) obj).Name;
          switch (name)
          {
            case "TradeAssignment.Status":
              string empty = string.Empty;
              string str;
              if (pipelineInfo.Info[(object) "TradeAssignment.Status"] == null)
              {
                str = "Removed from Correspondent Trade - Pending";
              }
              else
              {
                str = ((CorrespondentTradeLoanStatus) pipelineInfo.Info[(object) "TradeAssignment.Status"]).ToDescription();
                if (pipelineInfo.TradeAssignments != null && ((IEnumerable<PipelineInfo.TradeInfo>) pipelineInfo.TradeAssignments).Count<PipelineInfo.TradeInfo>() > 0 && (pipelineInfo.TradeAssignments[0].PendingStatus == 2 || pipelineInfo.TradeAssignments[0].PendingStatus == 3 || pipelineInfo.TradeAssignments[0].PendingStatus == 4))
                  str += " - Pending";
              }
              this.TradeAssignmentStatus = str;
              continue;
            case "Loan.TotalBuyPrice":
              this.TotalBuyPrice = CorrespondentTradeCalculation.CalculatePriceIndex(pipelineInfo, trade);
              continue;
            case "Loan.NetProfit":
              this.NetProfit = CorrespondentTradeCalculation.CalculateProfit(pipelineInfo, trade, 0M);
              continue;
            case "TradeAssignment.StatusDate":
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"]) == trade.TradeID)
              {
                this.TradeAssignmentStatusDate = (DateTime) pipelineInfo.Info[(object) "TradeAssignment.StatusDate"];
                continue;
              }
              continue;
            default:
              if (property.CanWrite)
              {
                if (property.PropertyType == typeof (string))
                {
                  property.SetValue((object) this, (object) (pipelineInfo.Info[(object) name] as string), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (DateTime))
                {
                  property.SetValue((object) this, (object) Utils.ParseDate(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (Decimal))
                {
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (int))
                {
                  property.SetValue((object) this, (object) Utils.ParseInt(pipelineInfo.Info[(object) name], 0), (object[]) null);
                  continue;
                }
                continue;
              }
              continue;
          }
        }
      }
    }

    /// <summary>Get field name for all properties in this class</summary>
    /// <returns>List of field names</returns>
    internal static List<string> GetFieldList()
    {
      List<string> fieldList = new List<string>();
      foreach (MemberInfo property in typeof (TradeLoanAssignment).GetProperties())
      {
        object obj = ((IEnumerable<object>) property.GetCustomAttributes(false)).FirstOrDefault<object>((Func<object, bool>) (a => a.GetType() == typeof (FieldAttribute)));
        if (obj != null)
          fieldList.Add(((FieldAttribute) obj).Name);
      }
      fieldList.Add("Trade.TradeType");
      fieldList.Add("Guid");
      return fieldList;
    }

    /// <summary>Constructor of TradeLoanAssignment</summary>
    /// <param name="trade"></param>
    /// <param name="pipelineInfo"></param>
    internal TradeLoanAssignment(LoanTradeInfo trade, PipelineInfo pipelineInfo)
    {
      foreach (PropertyInfo property in typeof (TradeLoanAssignment).GetProperties())
      {
        object obj = ((IEnumerable<object>) property.GetCustomAttributes(false)).FirstOrDefault<object>((Func<object, bool>) (a => a.GetType() == typeof (FieldAttribute)));
        if (obj != null)
        {
          string name = ((FieldAttribute) obj).Name;
          switch (name)
          {
            case "TradeAssignment.Status":
              string empty = string.Empty;
              string str;
              if (pipelineInfo.Info[(object) "TradeAssignment.Status"] == null)
              {
                str = "Removed from Loan Trade - Pending";
              }
              else
              {
                str = ((LoanTradeStatus) pipelineInfo.Info[(object) "TradeAssignment.Status"]).ToDescription();
                if (pipelineInfo.TradeAssignments != null && ((IEnumerable<PipelineInfo.TradeInfo>) pipelineInfo.TradeAssignments).Count<PipelineInfo.TradeInfo>() > 0 && (pipelineInfo.TradeAssignments[0].PendingStatus == 2 || pipelineInfo.TradeAssignments[0].PendingStatus == 3 || pipelineInfo.TradeAssignments[0].PendingStatus == 4))
                  str += " - Pending";
              }
              this.TradeAssignmentStatus = str;
              continue;
            case "Loan.TotalSellPrice":
              this.TotalSellPrice = trade.CalculatePriceIndex(pipelineInfo);
              continue;
            case "Loan.NetProfit":
              this.NetProfit = trade.CalculateProfit(pipelineInfo, 0M);
              continue;
            case "TradeAssignment.StatusDate":
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"]) == trade.TradeID)
              {
                this.TradeAssignmentStatusDate = (DateTime) pipelineInfo.Info[(object) "TradeAssignment.StatusDate"];
                continue;
              }
              continue;
            default:
              if (property.CanWrite)
              {
                if (property.PropertyType == typeof (string))
                {
                  property.SetValue((object) this, (object) (pipelineInfo.Info[(object) name] as string), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (DateTime))
                {
                  property.SetValue((object) this, (object) Utils.ParseDate(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (Decimal))
                {
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (int))
                {
                  property.SetValue((object) this, (object) Utils.ParseInt(pipelineInfo.Info[(object) name], 0), (object[]) null);
                  continue;
                }
                continue;
              }
              continue;
          }
        }
      }
    }

    /// <summary>Constructor of TradeLoanAssignment</summary>
    /// <param name="trade"></param>
    /// <param name="pipelineInfo"></param>
    internal TradeLoanAssignment(MbsPoolInfo trade, PipelineInfo pipelineInfo)
    {
      foreach (PropertyInfo property in typeof (TradeLoanAssignment).GetProperties())
      {
        object obj = ((IEnumerable<object>) property.GetCustomAttributes(false)).FirstOrDefault<object>((Func<object, bool>) (a => a.GetType() == typeof (FieldAttribute)));
        if (obj != null)
        {
          string name = ((FieldAttribute) obj).Name;
          switch (name)
          {
            case "TradeAssignment.Status":
              string empty = string.Empty;
              string str;
              if (pipelineInfo.Info[(object) "TradeAssignment.Status"] == null)
              {
                str = "Removed from MBS Pool - Pending";
              }
              else
              {
                str = ((CorrespondentTradeLoanStatus) pipelineInfo.Info[(object) "TradeAssignment.Status"]).ToDescription();
                if (pipelineInfo.TradeAssignments != null && ((IEnumerable<PipelineInfo.TradeInfo>) pipelineInfo.TradeAssignments).Count<PipelineInfo.TradeInfo>() > 0 && (pipelineInfo.TradeAssignments[0].PendingStatus == 2 || pipelineInfo.TradeAssignments[0].PendingStatus == 3 || pipelineInfo.TradeAssignments[0].PendingStatus == 4))
                  str += " - Pending";
              }
              this.TradeAssignmentStatus = str;
              continue;
            case "Loan.TotalSellPrice":
              this.TotalSellPrice = trade.CalculatePriceIndex(pipelineInfo, trade.WeightedAvgPrice);
              continue;
            case "Loan.NetProfit":
              this.NetProfit = trade.CalculateProfit(pipelineInfo, trade.WeightedAvgPrice);
              continue;
            case "TradeAssignment.StatusDate":
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"]) == trade.TradeID)
              {
                this.TradeAssignmentStatusDate = (DateTime) pipelineInfo.Info[(object) "TradeAssignment.StatusDate"];
                continue;
              }
              continue;
            default:
              if (property.CanWrite)
              {
                if (property.PropertyType == typeof (string))
                {
                  property.SetValue((object) this, (object) (pipelineInfo.Info[(object) name] as string), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (DateTime))
                {
                  property.SetValue((object) this, (object) Utils.ParseDate(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (Decimal))
                {
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name]), (object[]) null);
                  continue;
                }
                if (property.PropertyType == typeof (int))
                {
                  property.SetValue((object) this, (object) Utils.ParseInt(pipelineInfo.Info[(object) name], 0), (object[]) null);
                  continue;
                }
                continue;
              }
              continue;
          }
        }
      }
    }
  }
}
