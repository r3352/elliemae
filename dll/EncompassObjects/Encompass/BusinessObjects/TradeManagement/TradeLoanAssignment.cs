// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.TradeLoanAssignment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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
  public class TradeLoanAssignment
  {
    [Field("TradeAssignment.Status")]
    [Description("Trade Assignment Status")]
    public string TradeAssignmentStatus { get; set; }

    [Field("TradeAssignment.StatusDate")]
    [Description("Trade Assignment Status Date")]
    public DateTime TradeAssignmentStatusDate { get; set; }

    [Field("TradeAssignment.TradeID")]
    [Description("Trade Assignment ID")]
    public DateTime TradeAssignementId { get; set; }

    [Field("Loan.LoanNumber")]
    [Description("Loan Number")]
    public string LoanNumber { get; set; }

    [Field("Loan.TotalBuyPrice")]
    [Description("Loan Trade Total Buy Price")]
    public Decimal TotalBuyPrice { get; set; }

    [Field("Loan.TotalSellPrice")]
    [Description("Loan Trade Total Sell Price")]
    public Decimal TotalSellPrice { get; set; }

    [Field("Loan.NetProfit")]
    [Description("Gain/Loss")]
    public Decimal NetProfit { get; set; }

    [Field("Loan.LoanProgram")]
    [Description("Loan Program")]
    public string LoanProgram { get; set; }

    [Field("Loan.CurrentMilestoneName")]
    [Description("Last Finished Milestone")]
    public string CurrentMilestoneName { get; set; }

    [Field("Loan.TotalLoanAmount")]
    [Description("Loan Amount")]
    public Decimal TotalLoanAmount { get; set; }

    [Field("Loan.LoanAmount")]
    [Description("Loan Amount")]
    public Decimal LoanAmount { get; set; }

    [Field("Loan.LoanRate")]
    [Description("Note Rate")]
    public Decimal NoteRate { get; set; }

    [Field("Loan.Term")]
    [Description("Term")]
    public int Term { get; set; }

    [Field("Loan.LTV")]
    [Description("LTV")]
    public Decimal Ltv { get; set; }

    [Field("Loan.CLTV")]
    [Description("CLTV")]
    public Decimal Cltv { get; set; }

    [Field("Loan.DTITop")]
    [Description("DTI Top")]
    public Decimal DtiTop { get; set; }

    [Field("Loan.DTIBottom")]
    [Description("DTI Bottom")]
    public Decimal DtiBottom { get; set; }

    [Field("Loan.CreditScore")]
    [Description("FICO")]
    public int Fico { get; set; }

    [Field("Loan.OccupancyStatus")]
    [Description("Occupancy Type")]
    public string OccupancyType { get; set; }

    [Field("Loan.PropertyType")]
    [Description("Property Type")]
    public string PropertyType { get; set; }

    [Field("Loan.State")]
    [Description("State")]
    public string State { get; set; }

    [Field("Loan.LockExpirationDate")]
    [Description("Lock Expiration Date")]
    public DateTime LockExpirationDate { get; set; }

    [Field("Loan.BorrowerLastName")]
    [Description("Borrower Last Name")]
    public string BorrowerLastName { get; set; }

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
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"], false, -1) == ((TradeBase) trade).TradeID)
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
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name], false, 0M), (object[]) null);
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
                str = ((Enum) (object) (LoanTradeStatus) pipelineInfo.Info[(object) "TradeAssignment.Status"]).ToDescription();
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
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"], false, -1) == ((TradeBase) trade).TradeID)
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
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name], false, 0M), (object[]) null);
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
              if (Utils.ParseInt(pipelineInfo.Info[(object) "TradeAssignment.TradeID"], false, -1) == ((TradeBase) trade).TradeID)
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
                  property.SetValue((object) this, (object) Utils.ParseDecimal(pipelineInfo.Info[(object) name], false, 0M), (object[]) null);
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
