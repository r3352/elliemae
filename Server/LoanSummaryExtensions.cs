// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanSummaryExtensions
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LoanSummaryExtensions
  {
    public static void AddToSummary(LoanSummaryExtension loanSummaryExtension)
    {
    }

    public static void UpdateSummary(LoanSummaryExtension loanSummaryExtension)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("DECLARE @TradeId int");
      if (!string.IsNullOrEmpty(loanSummaryExtension.CorrespondentTradeGuid))
        stringBuilder.AppendLine("SELECT @TradeId = TradeId FROM trades WHERE Guid = '" + loanSummaryExtension.CorrespondentTradeGuid + "'");
      else
        stringBuilder.AppendLine("SET @TradeId = null");
      stringBuilder.AppendLine("IF NOT EXISTS(SELECT 1 FROM LoanSummaryExtension WHERE Guid = '" + loanSummaryExtension.Guid + "')");
      stringBuilder.AppendLine("     INSERT INTO LoanSummaryExtension(Guid, CorrespondentTradeID, LoanNumber, LoanAmount, PurchaseDate, ReceivedDate, RejectedDate, DeliveryType, TPOCompanyID, SubmittedForReviewDate, PurchaseSuspenseDate,PurchaseApprovalDate, ClearedForPurchaseDate, CancelledDate, VoidedDate, WithdrawalRequestedDate, PurchasedPrinciple, LenderCaseIdentifier)  ");
      stringBuilder.AppendLine("     VALUES ( '" + loanSummaryExtension.Guid + "', @TradeId, ");
      stringBuilder.AppendLine("'" + loanSummaryExtension.LoanNumber + "', ");
      stringBuilder.AppendLine(loanSummaryExtension.LoanAmount.ToString() + ", ");
      if (loanSummaryExtension.PurchaseDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.PurchaseDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.ReceivedDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.ReceivedDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.RejectedDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.RejectedDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.DeliveryType != CorrespondentMasterDeliveryType.None)
        stringBuilder.AppendLine(((int) loanSummaryExtension.DeliveryType).ToString() + ", ");
      else
        stringBuilder.AppendLine("null,");
      stringBuilder.AppendLine("'" + loanSummaryExtension.TpoCompanyId + "', ");
      if (loanSummaryExtension.SubmittedForReviewDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.SubmittedForReviewDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.PurchaseSuspenseDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.PurchaseSuspenseDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.PurchaseApprovalDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.PurchaseApprovalDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.ClearedForPurchaseDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.ClearedForPurchaseDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.CancelledDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.CancelledDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.VoidedDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.VoidedDate + "', ");
      else
        stringBuilder.AppendLine("NULL, ");
      if (loanSummaryExtension.WithdrawalRequestedDate != DateTime.MinValue)
        stringBuilder.AppendLine("'" + (object) loanSummaryExtension.WithdrawalRequestedDate + "',");
      else
        stringBuilder.AppendLine("NULL, ");
      stringBuilder.AppendLine("'" + (object) loanSummaryExtension.PurchasedPrinciple + "',");
      if (!string.IsNullOrEmpty(loanSummaryExtension.LenderCaseIdentifier))
        stringBuilder.AppendLine("'" + loanSummaryExtension.LenderCaseIdentifier.Replace("'", "\\''") + "')");
      else
        stringBuilder.AppendLine("NULL)");
      stringBuilder.AppendLine("ELSE");
      stringBuilder.AppendLine("     UPDATE LoanSummaryExtension");
      stringBuilder.AppendLine("     SET LoanNumber = '" + loanSummaryExtension.LoanNumber + "', ");
      stringBuilder.AppendLine("     CorrespondentTradeId = @TradeId, ");
      stringBuilder.AppendLine("     LoanAmount = " + (object) loanSummaryExtension.LoanAmount + ", ");
      if (loanSummaryExtension.PurchaseDate != DateTime.MinValue)
        stringBuilder.AppendLine("     PurchaseDate = '" + loanSummaryExtension.PurchaseDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     PurchaseDate = NULL, ");
      if (loanSummaryExtension.ReceivedDate != DateTime.MinValue)
        stringBuilder.AppendLine("     ReceivedDate = '" + loanSummaryExtension.ReceivedDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     ReceivedDate = NULL, ");
      if (loanSummaryExtension.RejectedDate != DateTime.MinValue)
        stringBuilder.AppendLine("     RejectedDate = '" + loanSummaryExtension.RejectedDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     RejectedDate = NULL, ");
      if (loanSummaryExtension.DeliveryType != CorrespondentMasterDeliveryType.None)
        stringBuilder.AppendLine("     DeliveryType = " + (object) (int) loanSummaryExtension.DeliveryType + ", ");
      else
        stringBuilder.AppendLine("     DeliveryType = null, ");
      stringBuilder.AppendLine("     TPOCompanyID = '" + loanSummaryExtension.TpoCompanyId + "', ");
      if (loanSummaryExtension.SubmittedForReviewDate != DateTime.MinValue)
        stringBuilder.AppendLine("     SubmittedForReviewDate = '" + loanSummaryExtension.SubmittedForReviewDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     SubmittedForReviewDate = null, ");
      if (loanSummaryExtension.PurchaseSuspenseDate != DateTime.MinValue)
        stringBuilder.AppendLine("     PurchaseSuspenseDate = '" + loanSummaryExtension.PurchaseSuspenseDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     PurchaseSuspenseDate = null, ");
      if (loanSummaryExtension.PurchaseApprovalDate != DateTime.MinValue)
        stringBuilder.AppendLine("     PurchaseApprovalDate = '" + loanSummaryExtension.PurchaseApprovalDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     PurchaseApprovalDate = null, ");
      if (loanSummaryExtension.ClearedForPurchaseDate != DateTime.MinValue)
        stringBuilder.AppendLine("     ClearedForPurchaseDate = '" + loanSummaryExtension.ClearedForPurchaseDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     ClearedForPurchaseDate = null, ");
      if (loanSummaryExtension.CancelledDate != DateTime.MinValue)
        stringBuilder.AppendLine("     CancelledDate = '" + loanSummaryExtension.CancelledDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     CancelledDate = null, ");
      if (loanSummaryExtension.VoidedDate != DateTime.MinValue)
        stringBuilder.AppendLine("     VoidedDate = '" + loanSummaryExtension.VoidedDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     VoidedDate = null, ");
      if (loanSummaryExtension.WithdrawalRequestedDate != DateTime.MinValue)
        stringBuilder.AppendLine("     WithdrawalRequestedDate = '" + loanSummaryExtension.WithdrawalRequestedDate.ToString() + "', ");
      else
        stringBuilder.AppendLine("     WithdrawalRequestedDate = null, ");
      stringBuilder.AppendLine("     PurchasedPrinciple = " + (object) loanSummaryExtension.PurchasedPrinciple + ", ");
      if (!string.IsNullOrEmpty(loanSummaryExtension.LenderCaseIdentifier))
        stringBuilder.AppendLine("     LenderCaseIdentifier = '" + loanSummaryExtension.LenderCaseIdentifier.Replace("'", "\\''").ToString() + "' ");
      else
        stringBuilder.AppendLine("     LenderCaseIdentifier = null ");
      stringBuilder.AppendLine("     WHERE Guid = '" + loanSummaryExtension.Guid + "'");
      dbQueryBuilder.Append(stringBuilder.ToString());
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteSummary(string loanGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("IF EXISTS(SELECT 1 FROM LoanSummaryExtension WHERE Guid = '" + loanGuid + "')");
      stringBuilder.AppendLine("DELETE FROM LoanSummaryExtension ");
      stringBuilder.AppendLine("WHERE Guid = '" + loanGuid + "'");
      dbQueryBuilder.Append(stringBuilder.ToString());
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanSummaryExtension GetSummary(string loanGuid) => new LoanSummaryExtension();

    public static List<LoanSummaryExtension> GetSummariesByTradeId(int tradeId)
    {
      List<LoanSummaryExtension> summariesByTradeId = new List<LoanSummaryExtension>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("Select LoanSummaryExtension.*, LoanSummary.WithdrawnDate from LoanSummaryExtension inner join LoanSummary on LoanSummary.Guid = LoanSummaryExtension.Guid where CorrespondentTradeId = " + (object) tradeId);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          summariesByTradeId.Add(new LoanSummaryExtension()
          {
            Guid = row["Guid"].ToString(),
            LoanNumber = row["LoanNumber"].ToString(),
            CorrespondentTradeId = SQL.DecodeInt(row["CorrespondentTradeId"]),
            CorrespondentLoanStatus = SQL.DecodeInt(row["CorrespondentLoanStatus"]),
            LoanAmount = SQL.DecodeDecimal(row["LoanAmount"]),
            PurchaseDate = SQL.DecodeDateTime(row["PurchaseDate"]),
            ReceivedDate = SQL.DecodeDateTime(row["ReceivedDate"]),
            RejectedDate = SQL.DecodeDateTime(row["RejectedDate"]),
            DeliveryType = (CorrespondentMasterDeliveryType) SQL.DecodeInt(row["DeliveryType"]),
            TpoCompanyId = SQL.DecodeString(row["TPOCompanyID"]),
            SubmittedForReviewDate = SQL.DecodeDateTime(row["SubmittedForReviewDate"]),
            PurchaseSuspenseDate = SQL.DecodeDateTime(row["PurchaseSuspenseDate"]),
            PurchaseApprovalDate = SQL.DecodeDateTime(row["PurchaseApprovalDate"]),
            ClearedForPurchaseDate = SQL.DecodeDateTime(row["ClearedForPurchaseDate"]),
            CancelledDate = SQL.DecodeDateTime(row["CancelledDate"]),
            VoidedDate = SQL.DecodeDateTime(row["VoidedDate"]),
            WithdrawnDate = SQL.DecodeDateTime(row["WithdrawnDate"]),
            PurchasedPrinciple = SQL.DecodeDecimal(row["PurchasedPrinciple"]),
            LenderCaseIdentifier = row["LenderCaseIdentifier"].ToString()
          });
      }
      return summariesByTradeId;
    }

    public static void UpdateSummeryExtension(LoanSummaryExtension extension)
    {
      string tradeGuid = extension.TradeGuid;
      string investorStatus = extension.InvestorStatus;
      DateTime investorStatusDate = extension.InvestorStatusDate;
      if (tradeGuid == "")
      {
        LoanTrades.SetTradeAssignmentStatus(extension.Guid, tradeGuid, LoanTradeStatus.Unassigned, investorStatusDate);
      }
      else
      {
        switch (investorStatus)
        {
          case "Purchased":
            LoanTrades.SetTradeAssignmentStatus(extension.Guid, tradeGuid, LoanTradeStatus.Purchased, investorStatusDate);
            break;
          case "Shipped":
            LoanTrades.SetTradeAssignmentStatus(extension.Guid, tradeGuid, LoanTradeStatus.Shipped, investorStatusDate);
            break;
          default:
            LoanTrades.SetTradeAssignmentStatus(extension.Guid, tradeGuid, LoanTradeStatus.Assigned, investorStatusDate);
            break;
        }
      }
      string correspondentTradeGuid = extension.CorrespondentTradeGuid;
      CorrespondentTrades.SetTradeAssignmentStatus(extension.Guid, correspondentTradeGuid, correspondentTradeGuid == "" ? CorrespondentTradeLoanStatus.Unassigned : CorrespondentTradeLoanStatus.Assigned, investorStatusDate);
      if (string.IsNullOrEmpty(extension.TpoCompanyId))
        return;
      LoanSummaryExtensions.UpdateSummary(extension);
    }
  }
}
