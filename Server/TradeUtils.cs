// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TradeUtils
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class TradeUtils
  {
    internal static void DeleteTrade(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("Trades");
      DbValue key = new DbValue("TradeID", (object) tradeId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    internal static void setBasicTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update Trades set status = " + (object) (int) status + " where TradeID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) tradeIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    internal static DbValueList CreateTradesValueList(TradeInfoObj info)
    {
      DbValueList tradesValueList = new DbValueList();
      tradesValueList.Add("Guid", (object) info.Guid);
      tradesValueList.Add("Name", (object) info.Name);
      tradesValueList.Add("Status", (object) (int) info.Status);
      tradesValueList.Add("Locked", (object) info.Locked, (IDbEncoder) DbEncoding.Flag);
      tradesValueList.Add("TradeType", (object) (int) info.TradeType);
      tradesValueList.Add("ContractID", (object) info.ContractID, (IDbEncoder) DbEncoding.MinusOneAsNull);
      tradesValueList.Add("TradeAmount", (object) info.TradeAmount);
      tradesValueList.Add("CommitmentType", (object) info.CommitmentType);
      tradesValueList.Add("CommitmentDate", (object) info.CommitmentDate);
      tradesValueList.Add("DealerName", (object) info.DealerName);
      tradesValueList.Add("PairOffFee", (object) info.PairOffFee);
      tradesValueList.Add("PairOffAmount", (object) info.PairOffAmount);
      tradesValueList.Add("Tolerance", (object) info.Tolerance);
      tradesValueList.Add("TradeDescription", (object) info.TradeDescription);
      tradesValueList.Add("InvestorName", (object) info.InvestorName);
      tradesValueList.Add("InvestorDeliveryDate", (object) info.InvestorDeliveryDate);
      tradesValueList.Add("EarlyDeliveryDate", (object) info.EarlyDeliveryDate);
      tradesValueList.Add("TargetDeliveryDate", (object) info.TargetDeliveryDate);
      tradesValueList.Add("ShipmentDate", (object) info.ShipmentDate);
      tradesValueList.Add("PurchaseDate", (object) info.PurchaseDate);
      if (info is MbsPoolInfo)
        tradesValueList.Add("OpenAmount", (object) ((MbsPoolInfo) info).TBAOpenAmount);
      else
        tradesValueList.Add("OpenAmount", (object) info.OpenAmount);
      tradesValueList.Add("PairOffGainLoss", (object) info.TotalPairOffGainLoss);
      tradesValueList.Add("LastModified", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      tradesValueList.Add("PendingBy", (object) info.PendingBy);
      return tradesValueList;
    }

    internal static DbValueList CreateTradeObjectsValueList(TradeInfoObj info)
    {
      return new DbValueList()
      {
        {
          "FilterQueryXml",
          info.Filter == null ? (object) (string) null : (object) info.Filter.ToXml()
        },
        {
          "SRPTableXml",
          info.SRPTable == null ? (object) (string) null : (object) info.SRPTable.ToXml()
        },
        {
          "InvestorXml",
          info.Investor == null ? (object) (string) null : (object) info.Investor.ToXml()
        },
        {
          "PairOffsXml",
          info.PairOffs == null ? (object) (string) null : (object) info.PairOffs.ToXml()
        },
        {
          "PricingXml",
          info.Pricing == null ? (object) (string) null : (object) info.Pricing.ToXml()
        },
        {
          "AdjustmentsXml",
          info.PriceAdjustments == null ? (object) (string) null : (object) info.PriceAdjustments.ToXml()
        },
        {
          "DealerXml",
          info.Dealer == null ? (object) (string) null : (object) info.Dealer.ToXml()
        },
        {
          "AssigneeXml",
          info.Assignee == null ? (object) (string) null : (object) info.Assignee.ToXml()
        },
        {
          "Notes",
          (object) info.Notes
        }
      };
    }

    internal static DbQueryBuilder generateTradeQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortFields,
      QueryEngine queryEngine,
      bool isExternalOrganization)
    {
      DbQueryBuilder tradeQuerySql = new DbQueryBuilder();
      tradeQuerySql.AppendLine("select " + fieldList + " ");
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      tradeQuerySql.Append(queryEngine.GetTableSelectionClause(fields, filter, sortFields, true, true, isExternalOrganization));
      tradeQuerySql.Append(queryEngine.GetOrderByClause(sortFields));
      return tradeQuerySql;
    }

    public static CorrespondentMasterDeliveryType GetDeliveryTypeEnum(string deliveryTypeString)
    {
      if (deliveryTypeString.IndexOf("Individual", StringComparison.CurrentCultureIgnoreCase) >= 0)
      {
        if (deliveryTypeString.IndexOf("Mandatory", StringComparison.CurrentCultureIgnoreCase) >= 0)
          return CorrespondentMasterDeliveryType.IndividualMandatory;
        if (deliveryTypeString.IndexOf("Best", StringComparison.CurrentCultureIgnoreCase) >= 0)
          return CorrespondentMasterDeliveryType.IndividualBestEfforts;
      }
      if (deliveryTypeString.IndexOf("AOT", StringComparison.CurrentCultureIgnoreCase) >= 0 && deliveryTypeString.IndexOf("BULK", StringComparison.CurrentCultureIgnoreCase) < 0)
        return CorrespondentMasterDeliveryType.AOT;
      if (deliveryTypeString.IndexOf("BULK", StringComparison.CurrentCultureIgnoreCase) >= 0 && deliveryTypeString.IndexOf("AOT", StringComparison.CurrentCultureIgnoreCase) < 0)
        return CorrespondentMasterDeliveryType.Bulk;
      if (deliveryTypeString.IndexOf("BULK", StringComparison.CurrentCultureIgnoreCase) >= 0 && deliveryTypeString.IndexOf("AOT", StringComparison.CurrentCultureIgnoreCase) >= 0)
        return CorrespondentMasterDeliveryType.BulkAOT;
      if (deliveryTypeString.IndexOf("Forward", StringComparison.CurrentCultureIgnoreCase) >= 0)
        return CorrespondentMasterDeliveryType.Forwards;
      return deliveryTypeString.IndexOf("Live", StringComparison.CurrentCultureIgnoreCase) >= 0 ? CorrespondentMasterDeliveryType.LiveTrade : CorrespondentMasterDeliveryType.None;
    }

    public static TradeStatus GetTradeStatus(string status)
    {
      try
      {
        return (TradeStatus) Enum.Parse(typeof (TradeStatus), status);
      }
      catch (Exception ex)
      {
        return TradeStatus.None;
      }
    }
  }
}
