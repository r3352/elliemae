// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CorrespondentMasters
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class CorrespondentMasters
  {
    private static string className = nameof (CorrespondentMasters);

    public static CorrespondentMasterInfo GetCorrespondentMasterByTradeId(int tradeId)
    {
      CorrespondentMasterInfo[] tradeIdFromDatabase = CorrespondentMasters.getCorrespondentMastersByTradeIdFromDatabase(string.Concat((object) tradeId));
      if (tradeIdFromDatabase != null && tradeIdFromDatabase.Length == 0)
        return (CorrespondentMasterInfo) null;
      return tradeIdFromDatabase?[0];
    }

    private static CorrespondentMasterInfo[] getCorrespondentMastersByTradeIdFromDatabase(
      string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select CorrespondentMasterID from CorrespondentMaster";
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      string text1 = "\r\n\t\t\t\tselect\r\n\t\t\t\t\tCM.CorrespondentMasterID, CM.GUID,\r\n\t\t\t\t\tCM.ContractNumber,\r\n\t\t\t\t\tCM.Status, CM.CommitmentType,\r\n\t\t\t\t\tCM.CommitmentAmount, CM.MasterEffectiveDate,\r\n\t\t\t\t\tCM.MasterExpirationDate,\r\n\t\t\t\t\tCM.ExternalOriginatorManagementID, CM.RateSheet,\r\n\t\t\t\t\tCM.CompanyName,\r\n\t\t\t\t\tCM.ExternalID, CM.OrganizationID,\r\n\t\t\t\t\tCM.Notes,\r\n\t\t\t\t\t--(select count(*) from CorrespondentMasterLoan CML where CML.CorrespondentMasterID=CM.CorrespondentMasterID) as LoanCount, -- TODO CMC: incorporate trade loans\r\n\t\t\t\t\t0 as CompletionPercentage,  -- TODO CMC: calculate in db\r\n\t\t\t\t\tCM.AvailableAmount\r\n\t\t\t\tfrom CorrespondentMaster CM\r\n\t\t\t\twhere CM.TradeID in (!!!CRITERIA!!!)\r\n\t\t\t\torder by CM.ContractNumber\r\n\t\t\t".Replace("!!!CRITERIA!!!", criteria);
      dbQueryBuilder1.Append(text1);
      string text2 = "select cdm.* from CorrespondentMasterDeliveryMethod cdm inner join CorrespondentMaster cm on cm.CorrespondentMasterId = cdm.CorrespondentMasterId where cm.TradeID in (!!!CRITERIA!!!)".Replace("!!!CRITERIA!!!", criteria);
      dbQueryBuilder1.Append(text2);
      DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      CorrespondentMasterInfo[] tradeIdFromDatabase = new CorrespondentMasterInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        CorrespondentMasterInfo correspondentMasterInfo = CorrespondentMasters.dataRowToCorrespondentMasterInfo(table1.Rows[index], table2, (DataTable) null);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("select CommitmentBestEffortLimited from ExternalOrgDetail e ");
        stringBuilder.AppendLine(" inner join CorrespondentMaster cm on cm.ExternalOriginatorManagementID = e.externalOrgID ");
        stringBuilder.AppendLine("where cm.TradeID = " + (object) correspondentMasterInfo.TradeID);
        DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
        dbQueryBuilder2.Append(stringBuilder.ToString());
        DataTable table3 = dbQueryBuilder2.ExecuteSetQuery().Tables[0];
        bool isCommitmentUseBestEffortLimited = correspondentMasterInfo.CommitmentType == MasterCommitmentType.BestEfforts;
        if (table3 != null && table3.Rows != null && table3.Rows.Count > 0)
          isCommitmentUseBestEffortLimited = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(table3.Rows[0]["CommitmentBestEffortLimited"]);
        CorrespondentTradeInfo[] tradeInfosByMasterId = CorrespondentTrades.GetTradeInfosByMasterId(correspondentMasterInfo.ID);
        if (tradeInfosByMasterId != null && tradeInfosByMasterId.Length != 0)
          correspondentMasterInfo.TradesInfo = tradeInfosByMasterId;
        correspondentMasterInfo.AvailableAmount = CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(correspondentMasterInfo.CommitmentAmount, isCommitmentUseBestEffortLimited, ((IEnumerable<CorrespondentTradeInfo>) tradeInfosByMasterId).ToList<CorrespondentTradeInfo>());
        tradeIdFromDatabase[index] = correspondentMasterInfo;
      }
      return tradeIdFromDatabase;
    }

    public static CorrespondentMasterInfo GetCorrespondentMaster(int correspondentMasterId)
    {
      CorrespondentMasterInfo[] mastersFromDatabase = CorrespondentMasters.getCorrespondentMastersFromDatabase(string.Concat((object) correspondentMasterId));
      return mastersFromDatabase.Length == 0 ? (CorrespondentMasterInfo) null : mastersFromDatabase[0];
    }

    private static CorrespondentMasterInfo[] getCorrespondentMastersFromDatabase(string criteria)
    {
      if ((criteria ?? "") == "")
        criteria = "select CorrespondentMasterID from CorrespondentMaster";
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      string text1 = "\r\n\t\t\t\tselect\r\n\t\t\t\t\tCM.CorrespondentMasterID, CM.GUID,\r\n\t\t\t\t\tCM.ContractNumber,\r\n\t\t\t\t\tCM.Status, CM.CommitmentType,\r\n\t\t\t\t\tCM.CommitmentAmount, CM.MasterEffectiveDate,\r\n\t\t\t\t\tCM.MasterExpirationDate,\r\n\t\t\t\t\tCM.ExternalOriginatorManagementID, CM.RateSheet,\r\n\t\t\t\t\tCM.CompanyName,\r\n\t\t\t\t\tCM.ExternalID, CM.OrganizationID,\r\n\t\t\t\t\tCM.Notes,\r\n\t\t\t\t\t--(select count(*) from CorrespondentMasterLoan CML where CML.CorrespondentMasterID=CM.CorrespondentMasterID) as LoanCount, -- TODO CMC: incorporate trade loans\r\n\t\t\t\t\t0 as CompletionPercentage,  -- TODO CMC: calculate in db\r\n\t\t\t\t\tCM.AvailableAmount\r\n\t\t\t\tfrom CorrespondentMaster CM\r\n\t\t\t\twhere CM.CorrespondentMasterID in (!!!CRITERIA!!!)\r\n\t\t\t\torder by CM.ContractNumber\r\n\t\t\t".Replace("!!!CRITERIA!!!", criteria);
      dbQueryBuilder1.Append(text1);
      string text2 = "select * from CorrespondentMasterDeliveryMethod where CorrespondentMasterID in (!!!CRITERIA!!!)".Replace("!!!CRITERIA!!!", criteria);
      dbQueryBuilder1.Append(text2);
      DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      CorrespondentMasterInfo[] mastersFromDatabase = new CorrespondentMasterInfo[table1.Rows.Count];
      for (int index = 0; index < table1.Rows.Count; ++index)
      {
        CorrespondentMasterInfo correspondentMasterInfo = CorrespondentMasters.dataRowToCorrespondentMasterInfo(table1.Rows[index], table2, (DataTable) null);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("select CommitmentBestEffortLimited from ExternalOrgDetail e ");
        stringBuilder.AppendLine(" inner join CorrespondentMaster cm on cm.ExternalOriginatorManagementID = e.externalOrgID ");
        stringBuilder.AppendLine("where cm.CorrespondentMasterID = " + (object) correspondentMasterInfo.ID);
        DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
        dbQueryBuilder2.Append(stringBuilder.ToString());
        DataTable table3 = dbQueryBuilder2.ExecuteSetQuery().Tables[0];
        bool isCommitmentUseBestEffortLimited = correspondentMasterInfo.CommitmentType == MasterCommitmentType.BestEfforts;
        if (table3 != null && table3.Rows != null && table3.Rows.Count > 0)
          isCommitmentUseBestEffortLimited = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(table3.Rows[0]["CommitmentBestEffortLimited"]);
        correspondentMasterInfo.AvailableAmount = CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(correspondentMasterInfo.CommitmentAmount, isCommitmentUseBestEffortLimited, ((IEnumerable<CorrespondentTradeInfo>) CorrespondentTrades.GetTradeInfosByMasterId(correspondentMasterInfo.ID)).ToList<CorrespondentTradeInfo>());
        mastersFromDatabase[index] = correspondentMasterInfo;
      }
      return mastersFromDatabase;
    }

    public static CorrespondentMasterInfo GetCorrespondentMasterSummaryByMasterNumber(
      string contractNumber)
    {
      CorrespondentMasterInfo summaryByMasterNumber = (CorrespondentMasterInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("Select * from CorrespondentMaster where ContractNumber like '" + EllieMae.EMLite.DataAccess.SQL.Escape(contractNumber) + "'");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        summaryByMasterNumber = CorrespondentMasters.dataRowToCorrespondentMasterInfo(dataSet.Tables[0].Rows[0], (DataTable) null, (DataTable) null);
      return summaryByMasterNumber;
    }

    public static CorrespondentMasterInfo[] GetActiveCorrespondentMasterByTpo(
      int tpoId,
      IList<MasterCommitmentType> commitmentTypes,
      DateTime expireDateTime)
    {
      string str = string.Join<int>(",", commitmentTypes.Select<MasterCommitmentType, int>((System.Func<MasterCommitmentType, int>) (t => (int) t)));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Select CorrespondentMasterID from CorrespondentMaster ");
      stringBuilder.Append("where Status <> 1 ");
      stringBuilder.Append("and (MasterExpirationDate is null or MasterExpirationDate is not null and MasterExpirationDate >= '" + expireDateTime.ToShortDateString() + "') ");
      stringBuilder.Append("and ExternalOriginatorManagementID = " + (object) tpoId + " ");
      if (!string.IsNullOrEmpty(str.Trim()))
        stringBuilder.Append("and CommitmentType in (" + str + ")");
      return CorrespondentMasters.getCorrespondentMastersFromDatabase(stringBuilder.ToString());
    }

    public static void SetCorrespondentMasterStatus(
      int[] commitmentIds,
      MasterCommitmentStatus status,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update CorrespondentMaster set status = " + (object) (int) status + " where CorrespondentMasterID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) commitmentIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
      foreach (CorrespondentMasterInfo correspondentMaster in CorrespondentMasters.GetCorrespondentMaster(commitmentIds))
        CorrespondentMasters.AddCorrespondentMasterHistoryItem(new CorrespondentMasterHistoryItem(correspondentMaster, status == MasterCommitmentStatus.Archived ? CorrespondentMasterHistoryAction.CorrespondentMasterArchived : CorrespondentMasterHistoryAction.CorrespondentMasterActivated, currentUser));
    }

    private static CorrespondentMasterViewModel getCorrespondentMasterViewModelFromDatabase(
      CorrespondentMasterViewModelID viewModelId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "\r\n\t\t\t\tselect\r\n\t\t\t\t\tCM.CorrespondentMasterID,\r\n\t\t\t\t\tCMDM.CorrespondentMasterDeliveryMethodID,\r\n\t\t\t\t\tCM.GUID,\r\n\t\t\t\t\tCM.ContractNumber,\r\n\t\t\t\t\tCM.Status,\r\n\t\t\t\t\tCM.CommitmentType,\r\n\t\t\t\t\tCM.CommitmentAmount,\r\n\t\t\t\t\tCM.MasterEffectiveDate,\r\n\t\t\t\t\tCM.MasterExpirationDate,\r\n\t\t\t\t\tCM.ExternalOriginatorManagementID,\r\n\t\t\t\t\tCM.RateSheet,\r\n\t\t\t\t\tCM.CompanyName,\r\n\t\t\t\t\tCM.ExternalID,\r\n\t\t\t\t\tCM.OrganizationID,\r\n\t\t\t\t\tCMDM.DeliveryType,\r\n\t\t\t\t\tCMDM.EffectiveDate,\r\n\t\t\t\t\tCMDM.ExpirationDate,\r\n\t\t\t\t\tCMDM.DeliveryDays,\r\n\t\t\t\t\tCMDM.Tolerance,    \r\n\t\t\t\t\t--(select count(*) from CorrespondentMasterLoan CML where CML.CorrespondentMasterID=CM.CorrespondentMasterID) as LoanCount,\r\n\t\t\t\t\t0 as TradeCount, \r\n\t\t\t\t\t0 as PercentComplete,\r\n\t\t\t\t\tCM.AvailableAmount,\r\n\t\t\t\t\tCM.LastModified\r\n\t\t\t\tfrom \r\n\t\t\t\t\tCorrespondentMaster CM\r\n\t\t\t\t\tleft outer join CorrespondentMasterDeliveryMethod CMDM\r\n\t\t\t\t\t\ton CM.CorrespondentMasterID = CMDM.CorrespondentMasterID\r\n\t\t\t\twhere\r\n\t\t\t\t\t!!CorrespondentMasterIDCriteria!!\r\n\t\t\t\t\tand !!CorrespondentMasterDeliveryMethodIDCritiera!!\r\n\t\t\t\torder by\r\n\t\t\t\t\tCM.ContractNumber\r\n\t\t\t".Replace("!!CorrespondentMasterIDCriteria!!", "CM.CorrespondentMasterID = " + (object) viewModelId.CorrespondentMasterID);
      string text = viewModelId.CorrespondentMasterDeliveryMethodID.HasValue ? str.Replace("!!CorrespondentMasterDeliveryMethodIDCritiera!!", "CMDM.CorrespondentMasterDeliveryMethodID = " + (object) viewModelId.CorrespondentMasterDeliveryMethodID) : str.Replace("!!CorrespondentMasterDeliveryMethodIDCritiera!!", "CMDM.CorrespondentMasterDeliveryMethodID is null");
      dbQueryBuilder.Append(text);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("select TradeID, TradeAmount, PairOffAmount, Status, CommitmentType, DeliveryType from CorrespondentTradeDetails ");
      stringBuilder.AppendLine("where CorrespondentMasterID = " + (object) viewModelId.CorrespondentMasterID);
      stringBuilder.AppendLine("select * from LoanSummaryExtension l ");
      stringBuilder.AppendLine(" inner join CorrespondentTrades ct on l.CorrespondentTradeId = ct.TradeID ");
      stringBuilder.AppendLine("where ct.CorrespondentMasterID = " + (object) viewModelId.CorrespondentMasterID);
      stringBuilder.AppendLine("select CommitmentBestEffortLimited from ExternalOrgDetail e ");
      stringBuilder.AppendLine(" inner join CorrespondentMaster cm on cm.ExternalOriginatorManagementID = e.externalOrgID ");
      stringBuilder.AppendLine("where cm.CorrespondentMasterID = " + (object) viewModelId.CorrespondentMasterID);
      dbQueryBuilder.Append(stringBuilder.ToString());
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      DataTable table3 = dataSet.Tables[2];
      DataTable table4 = dataSet.Tables[3];
      return table1 == null || table1 != null && table1.Rows != null && table1.Rows.Count == 0 ? (CorrespondentMasterViewModel) null : CorrespondentMasters.dataRowToCorrespondentMasterViewModel(table1.Rows[0], table2, table3, table4);
    }

    private static CorrespondentMasterViewModel dataRowToCorrespondentMasterViewModel(
      DataRow r,
      DataTable tradeTable,
      DataTable loanTable,
      DataTable tpoTable)
    {
      Decimal num1 = 0M;
      try
      {
        num1 = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["PercentComplete"]);
      }
      catch
      {
      }
      CorrespondentMasterViewModel correspondentMasterViewModel1 = new CorrespondentMasterViewModel();
      correspondentMasterViewModel1.CorrespondentMasterID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"]);
      correspondentMasterViewModel1.CorrespondentMasterDeliveryMethodID = r["CorrespondentMasterDeliveryMethodID"] == DBNull.Value ? new int?() : (int?) r["CorrespondentMasterDeliveryMethodID"];
      correspondentMasterViewModel1.GUID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GUID"]);
      correspondentMasterViewModel1.ContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      correspondentMasterViewModel1.CommitmentType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CommitmentType"]).ToString();
      correspondentMasterViewModel1.Status = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]);
      correspondentMasterViewModel1.CommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CommitmentAmount"]);
      correspondentMasterViewModel1.MasterEffectiveDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MasterEffectiveDate"]);
      correspondentMasterViewModel1.MasterExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MasterExpirationDate"]);
      correspondentMasterViewModel1.TpoId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ExternalID"]);
      correspondentMasterViewModel1.CompanyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CompanyName"]);
      correspondentMasterViewModel1.OrganizationId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrganizationID"]);
      correspondentMasterViewModel1.RateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RateSheet"]);
      int num2 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DeliveryType"]);
      correspondentMasterViewModel1.DeliveryType = num2.ToString();
      correspondentMasterViewModel1.DeliveryDays = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DeliveryDays"]);
      correspondentMasterViewModel1.EffectiveDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EffectiveDate"]);
      correspondentMasterViewModel1.ExpirationDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["ExpirationDate"]);
      correspondentMasterViewModel1.ExternalOriginatorManagementID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOriginatorManagementID"]);
      correspondentMasterViewModel1.Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["Tolerance"]);
      correspondentMasterViewModel1.AvailableAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AvailableAmount"]);
      CorrespondentMasterViewModel correspondentMasterViewModel2 = correspondentMasterViewModel1;
      string commitmentType = correspondentMasterViewModel2.CommitmentType;
      num2 = 1;
      string str = num2.ToString();
      bool isCommitmentUseBestEffortLimited = commitmentType == str;
      if (tpoTable != null && tpoTable.Rows != null && tpoTable.Rows.Count > 0)
        isCommitmentUseBestEffortLimited = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(tpoTable.Rows[0]["CommitmentBestEffortLimited"]);
      if (tradeTable != null && tradeTable.Rows != null && tradeTable.Rows.Count > 0)
      {
        List<CorrespondentTradeInfo> trades = new List<CorrespondentTradeInfo>();
        foreach (DataRow row in (InternalDataCollectionBase) tradeTable.Rows)
        {
          CorrespondentTradeInfo correspondentTradeInfo1 = new CorrespondentTradeInfo();
          correspondentTradeInfo1.CommitmentType = (CorrespondentTradeCommitmentType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CommitmentType"]);
          correspondentTradeInfo1.DeliveryType = (CorrespondentMasterDeliveryType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["DeliveryType"]);
          correspondentTradeInfo1.TradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["TradeAmount"]);
          correspondentTradeInfo1.PairOffAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["PairOffAmount"]);
          correspondentTradeInfo1.Status = (TradeStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Status"]);
          CorrespondentTradeInfo correspondentTradeInfo2 = correspondentTradeInfo1;
          List<LoanSummaryExtension> summaryExtensionList = new List<LoanSummaryExtension>();
          if (loanTable != null && loanTable.Rows != null && loanTable.Rows.Count > 0)
          {
            foreach (DataRow dataRow in loanTable.Select("CorrespondentTradeId = " + row["TradeID"]))
            {
              LoanSummaryExtension summaryExtension = new LoanSummaryExtension()
              {
                LoanAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(dataRow["LoanAmount"]),
                ReceivedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["ReceivedDate"]),
                RejectedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["RejectedDate"])
              };
              summaryExtensionList.Add(summaryExtension);
            }
          }
          correspondentTradeInfo2.AssignedLoanList = summaryExtensionList;
          trades.Add(correspondentTradeInfo2);
        }
        correspondentMasterViewModel2.AvailableAmount = CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(correspondentMasterViewModel2.CommitmentAmount, isCommitmentUseBestEffortLimited, trades);
      }
      return correspondentMasterViewModel2;
    }

    public static CorrespondentMasterViewModel[] GetCorrespondentMasterViewModels(
      CorrespondentMasterViewModelID[] viewModelIds)
    {
      if (viewModelIds == null || viewModelIds.Length == 0)
        return new CorrespondentMasterViewModel[0];
      List<CorrespondentMasterViewModel> correspondentMasterViewModelList = new List<CorrespondentMasterViewModel>();
      foreach (CorrespondentMasterViewModelID viewModelId in viewModelIds)
        correspondentMasterViewModelList.Add(CorrespondentMasters.getCorrespondentMasterViewModelFromDatabase(viewModelId));
      return correspondentMasterViewModelList.ToArray();
    }

    private static CorrespondentMasterInfo dataRowToCorrespondentMasterInfo(
      DataRow r,
      DataTable deliveryMethodTable,
      DataTable statusTable)
    {
      CorrespondentMasterInfo correspondentMasterInfo1 = new CorrespondentMasterInfo();
      correspondentMasterInfo1.ID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"], -1);
      correspondentMasterInfo1.Guid = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["GUID"]);
      correspondentMasterInfo1.Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContractNumber"]);
      correspondentMasterInfo1.CommitmentType = (MasterCommitmentType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CommitmentType"]);
      correspondentMasterInfo1.Status = (MasterCommitmentStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]);
      correspondentMasterInfo1.CommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["CommitmentAmount"]);
      correspondentMasterInfo1.MasterEffectiveDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MasterEffectiveDate"]);
      correspondentMasterInfo1.MasterExpirationDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["MasterExpirationDate"]);
      correspondentMasterInfo1.ExternalOriginatorManagementID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOriginatorManagementID"], -1);
      correspondentMasterInfo1.RateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["RateSheet"]);
      correspondentMasterInfo1.CompanyName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CompanyName"]);
      correspondentMasterInfo1.TpoId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ExternalID"]);
      correspondentMasterInfo1.OrganizationId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrganizationID"]);
      correspondentMasterInfo1.Notes = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Notes"]);
      correspondentMasterInfo1.AvailableAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(r["AvailableAmount"]);
      CorrespondentMasterInfo correspondentMasterInfo2 = correspondentMasterInfo1;
      if (deliveryMethodTable == null)
        return correspondentMasterInfo2;
      foreach (DataRow dataRow in deliveryMethodTable.Select("CorrespondentMasterID = " + r["CorrespondentMasterID"]))
        correspondentMasterInfo2.DeliveryInfos.Add(new MasterCommitmentDeliveryInfo()
        {
          ID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["CorrespondentMasterDeliveryMethodID"]),
          Type = (CorrespondentMasterDeliveryType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["DeliveryType"]),
          EffectiveDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["EffectiveDate"]),
          ExpireDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["ExpirationDate"]),
          DeliveryDays = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["DeliveryDays"]),
          Tolerance = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(dataRow["Tolerance"])
        });
      return correspondentMasterInfo2;
    }

    private static Dictionary<CorrespondentMasterLoanStatus, int> dataRowsToPendingLoanCounts(
      IEnumerable rows)
    {
      return new Dictionary<CorrespondentMasterLoanStatus, int>();
    }

    public static int CreateCorrespondentMaster(
      CorrespondentMasterInfo master,
      UserInfo currentUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("CorrespondentMaster");
      DbTableInfo table2 = DbAccessManager.GetTable("CorrespondentMasterDeliveryMethod");
      dbQueryBuilder.Declare("@correspondentMasterId", "int");
      dbQueryBuilder.InsertInto(table1, CorrespondentMasters.createCorrespondentMasterValueList(master, true), true, false);
      dbQueryBuilder.SelectIdentity("@correspondentMasterId");
      DbValue dbValue = new DbValue("CorrespondentMasterID", (object) "@correspondentMasterId", (IDbEncoder) DbEncoding.None);
      List<MasterCommitmentDeliveryInfo> deliveryInfos = master.DeliveryInfos;
      for (int index = 0; index < deliveryInfos.Count; ++index)
      {
        DbValueList masterDelivery = CorrespondentMasters.createMasterDelivery(deliveryInfos[index]);
        masterDelivery.Add(dbValue);
        dbQueryBuilder.InsertInto(table2, masterDelivery, true, false);
      }
      dbQueryBuilder.AppendLine("INSERT INTO Trades (Guid, Name, Status, Locked, TradeType, TradeAmount, OpenAmount, LastModified, Tolerance, PairOffFee, PairOffAmount, CommitmentType, TradeDescription ) ");
      dbQueryBuilder.AppendLine("SELECT GUID, ContractNumber, 1, 0, 5, CommitmentAmount, AvailableAmount, LastModified, 0, 0.00, 0.00, '', '' FROM CorrespondentMaster WHERE CorrespondentMasterID = @correspondentMasterId");
      dbQueryBuilder.AppendLine("UPDATE CorrespondentMaster SET TradeId = @@IDENTITY WHERE CorrespondentMasterID = @correspondentMasterId");
      dbQueryBuilder.Select("@correspondentMasterId");
      int correspondentMasterId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      master = CorrespondentMasters.GetCorrespondentMaster(correspondentMasterId);
      CorrespondentMasters.AddCorrespondentMasterHistoryItem(new CorrespondentMasterHistoryItem(master, CorrespondentMasterHistoryAction.CorrespondentMasterCreated, currentUser));
      if (master.Status != MasterCommitmentStatus.Active)
        CorrespondentMasters.AddCorrespondentMasterHistoryItem(new CorrespondentMasterHistoryItem(master, master.Status, currentUser));
      return correspondentMasterId;
    }

    private static DbValueList createCorrespondentMasterValueList(
      CorrespondentMasterInfo master,
      bool isCreateNew)
    {
      DbValueList correspondentMasterValueList = new DbValueList();
      if (isCreateNew)
      {
        correspondentMasterValueList.Add("ContractNumber", (object) master.Name);
        correspondentMasterValueList.Add("GUID", (object) Guid.NewGuid().ToString());
        correspondentMasterValueList.Add("CommitmentType", (object) (int) master.CommitmentType);
        correspondentMasterValueList.Add("ExternalOriginatorManagementID", (object) master.ExternalOriginatorManagementID);
        correspondentMasterValueList.Add("CompanyName", (object) master.CompanyName);
        correspondentMasterValueList.Add("OrganizationID", (object) master.OrganizationId);
        correspondentMasterValueList.Add("ExternalID", (object) master.TpoId);
      }
      correspondentMasterValueList.Add("Status", (object) (int) master.Status);
      correspondentMasterValueList.Add("CommitmentAmount", (object) master.CommitmentAmount);
      correspondentMasterValueList.Add("MasterEffectiveDate", (object) master.MasterEffectiveDateTime);
      correspondentMasterValueList.Add("MasterExpirationDate", (object) master.MasterExpirationDateTime);
      correspondentMasterValueList.Add("RateSheet", (object) master.RateSheet);
      correspondentMasterValueList.Add("Notes", (object) master.Notes);
      correspondentMasterValueList.Add("AvailableAmount", (object) master.AvailableAmount);
      correspondentMasterValueList.Add("LastModified", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
      return correspondentMasterValueList;
    }

    private static DbValueList createMasterDelivery(MasterCommitmentDeliveryInfo info)
    {
      return new DbValueList()
      {
        {
          "DeliveryType",
          (object) (int) info.Type
        },
        {
          "EffectiveDate",
          (object) info.EffectiveDateTime
        },
        {
          "ExpirationDate",
          (object) info.ExpireDateTime
        },
        {
          "DeliveryDays",
          (object) info.DeliveryDays
        },
        {
          "Tolerance",
          (object) info.Tolerance
        }
      };
    }

    public static List<CorrespondentMasterViewModelID> QueryCorrespondentMasterIds(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      StringBuilder stringBuilder = new StringBuilder(CorrespondentMasters.getQueryCorrespondentMasterIdsSql(user, criteria, sortOrder, isExternalOrganization));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<CorrespondentMasterViewModelID> masterViewModelIdList = new List<CorrespondentMasterViewModelID>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        masterViewModelIdList.Add(new CorrespondentMasterViewModelID()
        {
          CorrespondentMasterID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["CorrespondentMasterID"]),
          CorrespondentMasterDeliveryMethodID = dataRow["CorrespondentMasterDeliveryMethodID"] == DBNull.Value ? new int?() : (int?) dataRow["CorrespondentMasterDeliveryMethodID"]
        });
      return masterViewModelIdList;
    }

    private static string getQueryCorrespondentMasterIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization)
    {
      try
      {
        string fieldList = "CorrespondentMaster.CorrespondentMasterID, CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID";
        CorrespondentMasterQuery correspondentMasterQuery = new CorrespondentMasterQuery(user);
        return CorrespondentMasters.generateQuerySql(fieldList, user, criteria, sortOrder, (QueryEngine) correspondentMasterQuery, isExternalOrganization).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(CorrespondentMasters.className, ex);
        return (string) null;
      }
    }

    private static DbQueryBuilder generateQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      SortField[] sortFields,
      QueryEngine queryEngine,
      bool isExternalOrganization)
    {
      DbQueryBuilder querySql = new DbQueryBuilder();
      querySql.AppendLine("select " + fieldList + " ");
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion queryCriterion = QueryCriterion.Join(criteria, BinaryOperator.And);
      querySql.Append(queryEngine.GetTableSelectionClause(fields, queryCriterion, sortFields, false, true, isExternalOrganization));
      querySql.Append(queryEngine.GetFilterClause(queryCriterion));
      querySql.Append(queryEngine.GetOrderByClause(sortFields));
      return querySql;
    }

    public static void UpdateCorrespondentMaster(
      CorrespondentMasterInfo master,
      UserInfo currentUser)
    {
      CorrespondentMasterInfo correspondentMaster = CorrespondentMasters.GetCorrespondentMaster(master.ID);
      if (correspondentMaster == null)
        throw new ObjectNotFoundException("Correspondent Master Commitment not found", ObjectType.Trade, (object) master.ID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("CorrespondentMaster");
      DbTableInfo table2 = DbAccessManager.GetTable("CorrespondentMasterDeliveryMethod");
      DbValue key = new DbValue("CorrespondentMasterID", (object) master.ID);
      dbQueryBuilder.DeleteFrom(table2, key);
      dbQueryBuilder.Update(table1, CorrespondentMasters.createCorrespondentMasterValueList(master, false), key);
      List<MasterCommitmentDeliveryInfo> deliveryInfos = master.DeliveryInfos;
      for (int index = 0; index < deliveryInfos.Count; ++index)
      {
        DbValueList masterDelivery = CorrespondentMasters.createMasterDelivery(deliveryInfos[index]);
        masterDelivery.Add(key);
        dbQueryBuilder.InsertInto(table2, masterDelivery, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
      if (master.Status == correspondentMaster.Status)
        return;
      CorrespondentMasters.AddCorrespondentMasterHistoryItem(new CorrespondentMasterHistoryItem(master, master.Status, currentUser));
    }

    public static bool DeleteCorrespondentMaster(int masterId, UserInfo userInfo)
    {
      CorrespondentMasters.GetCorrespondentMaster(masterId);
      if (CorrespondentTrades.GetTradeInfosByMasterId(masterId).Length != 0)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("CorrespondentMaster");
      DbTableInfo table2 = DbAccessManager.GetTable("CorrespondentMasterDeliveryMethod");
      DbValue key = new DbValue("CorrespondentMasterID", (object) masterId);
      dbQueryBuilder.DeleteFrom(table2, key);
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.ExecuteNonQuery();
      return true;
    }

    public static CorrespondentMasterInfo[] GetCorrespondentMaster(int[] masterIds)
    {
      return CorrespondentMasters.getCorrespondentMastersFromDatabase(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) masterIds));
    }

    public static CorrespondentMasterInfo GetCorrespondentMasterForTrade(int tradeId)
    {
      CorrespondentMasterInfo[] mastersFromDatabase = CorrespondentMasters.getCorrespondentMastersFromDatabase("select CorrespondentMasterID from CorrespondentTrades where TradeID = " + (object) tradeId);
      return mastersFromDatabase != null && ((IEnumerable<CorrespondentMasterInfo>) mastersFromDatabase).Count<CorrespondentMasterInfo>() > 0 ? mastersFromDatabase[0] : (CorrespondentMasterInfo) null;
    }

    public static CorrespondentMasterHistoryItem[] GetCorrespondentMasterHistory(
      int correspondentMasterId)
    {
      return CorrespondentMasters.getCorrespondentMasterHistoryFromDatabase("select CorrespondentMasterHistoryID from CorrespondentMasterHistory where CorrespondentMasterID = " + (object) correspondentMasterId);
    }

    private static CorrespondentMasterHistoryItem[] getCorrespondentMasterHistoryFromDatabase(
      string selectionQuery)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "\r\n\t\t\t\tselect CorrespondentMasterHistory.*,\r\n\t\t\t\t\tLoanSummary.LoanNumber, CorrespondentMaster.ContractNumber as CorrespondentMasterName\r\n\t\t\t\tfrom CorrespondentMasterHistory\r\n\t\t\t\t\tinner join CorrespondentMaster on CorrespondentMasterHistory.CorrespondentMasterID = CorrespondentMaster.CorrespondentMasterID\r\n\t\t\t\t\tleft outer join LoanSummary on CorrespondentMasterHistory.LoanGuid = LoanSummary.Guid ";
      dbQueryBuilder.Append(text);
      if ((selectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where CorrespondentMasterHistory.CorrespondentMasterHistoryID in (" + selectionQuery + ")");
      return CorrespondentMasters.dataRowsToCorrespondentMasterHistoryItems((IEnumerable) dbQueryBuilder.Execute());
    }

    private static CorrespondentMasterHistoryItem[] dataRowsToCorrespondentMasterHistoryItems(
      IEnumerable rows)
    {
      List<CorrespondentMasterHistoryItem> masterHistoryItemList = new List<CorrespondentMasterHistoryItem>();
      foreach (DataRow row in rows)
        masterHistoryItemList.Add(CorrespondentMasters.dataRowToHistoryItem(row));
      return masterHistoryItemList.ToArray();
    }

    private static CorrespondentMasterHistoryItem dataRowToHistoryItem(DataRow r)
    {
      return new CorrespondentMasterHistoryItem(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterHistoryID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["CorrespondentMasterID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), (CorrespondentMasterHistoryAction) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Action"], 0), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Timestamp"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UserID"]), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["DataXml"]));
    }

    public static int AddCorrespondentMasterHistoryItem(CorrespondentMasterHistoryItem item)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("CorrespondentMasterHistory");
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "CorrespondentMasterID",
          (object) item.CorrespondentMasterID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "LoanGuid",
          (object) item.LoanGuid,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "UserID",
          (object) item.UserID
        },
        {
          "Action",
          (object) (int) item.Action
        },
        {
          "Status",
          (object) item.Status,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "Timestamp",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "DataXml",
          (object) item.Data.ToXml()
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity();
      item.CorrespondentMasterHistoryID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      return item.CorrespondentMasterHistoryID;
    }

    public static bool IsAllLoanPurcahsedInMaster(int[] masterIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("select count(*) from CorrespondentMaster cmc ");
      stringBuilder.AppendLine("inner join CorrespondentTrades ct on cmc.CorrespondentMasterID = ct.CorrespondentMasterID ");
      stringBuilder.AppendLine("inner join LoanSummaryExtension lse on lse.CorrespondentTradeId = ct.TradeID ");
      stringBuilder.AppendLine("where cmc.CorrespondentMasterID in (");
      stringBuilder.AppendLine(string.Join<int>(",", (IEnumerable<int>) masterIds));
      stringBuilder.AppendLine(") and lse.purchaseDate is null");
      dbQueryBuilder.Append(stringBuilder.ToString());
      return int.Parse(dbQueryBuilder.ExecuteScalar().ToString()) == 0;
    }

    public static bool AreTradesInCorrespondentMasters()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("select count(*) from CorrespondentTrades ");
      stringBuilder.AppendLine("where CorrespondentMasterId is not null");
      dbQueryBuilder.Append(stringBuilder.ToString());
      return int.Parse(dbQueryBuilder.ExecuteScalar().ToString()) > 0;
    }

    public static CorrespondentMasterSummaryInfo GetCorrespondentMasterSummary(int id)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("select      CorrespondentTrades.CorrespondentMasterID,");
      stringBuilder.AppendLine("count(CorrespondentTrades.TradeID) as TotalTrades,");
      stringBuilder.AppendLine("sum(CorrespondentTradeDetails.GainLossAmount) as GainLossAmount,");
      stringBuilder.AppendLine("SUM(CorrespondentTradeDetails.TradeAmount) as TotalTradeAmount,");
      stringBuilder.AppendLine("sum(CorrespondentTradeSummary.LoanCount) as LoanCount,");
      stringBuilder.AppendLine("sum(CorrespondentTradeSummary.TotalAmount) as TotalAmount,");
      stringBuilder.AppendLine("sum((case Trades.TradeAmount when 0 Then isnull(Trades.PairOffAmount, 0) Else Trades.TradeAmount - isnull(CorrespondentTradeSummary.TotalAmount,0) + isnull(Trades.PairOffAmount, 0) End)) as OpenAmount ");
      stringBuilder.AppendLine("from CorrespondentTrades ");
      stringBuilder.AppendLine("inner join Trades on CorrespondentTrades.TradeID = Trades.TradeID ");
      stringBuilder.AppendLine("inner join CorrespondentTradeDetails on CorrespondentTradeDetails.TradeID = CorrespondentTrades.TradeId ");
      stringBuilder.AppendLine("left join ");
      stringBuilder.AppendLine("(select TradeAssignment.TradeID, count(LoanSummary.Guid) as LoanCount, Sum(IsNull(TradeAssignment.Profit, 0)) as TotalProfit, ");
      stringBuilder.AppendLine("Sum(convert(money, TotalLoanAmount)) as TotalAmount ");
      stringBuilder.AppendLine("from TradeAssignment inner join LoanSummary on TradeAssignment.LoanGuid = LoanSummary.Guid ");
      stringBuilder.AppendLine("where TradeAssignment.Status > 1 ");
      stringBuilder.AppendLine("group by TradeAssignment.TradeID ");
      stringBuilder.AppendLine(") CorrespondentTradeSummary on CorrespondentTradeSummary.TradeID = CorrespondentTrades.TradeID ");
      stringBuilder.AppendLine("group by CorrespondentTrades.CorrespondentMasterID ");
      stringBuilder.AppendLine("having CorrespondentTrades.CorrespondentMasterID  = ");
      stringBuilder.AppendLine(id.ToString());
      dbQueryBuilder.Append(stringBuilder.ToString());
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      CorrespondentMasterSummaryInfo correspondentMasterSummary = new CorrespondentMasterSummaryInfo();
      DataTable table = dataSet.Tables[0];
      correspondentMasterSummary.ID = id;
      if (table.Rows.Count == 1)
      {
        correspondentMasterSummary.TotalTrades = EllieMae.EMLite.DataAccess.SQL.DecodeInt(table.Rows[0]["TotalTrades"]);
        correspondentMasterSummary.GainLossAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table.Rows[0]["GainLossAmount"]);
        correspondentMasterSummary.TotalAssignedLoanAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table.Rows[0]["TotalAmount"]);
        correspondentMasterSummary.TotalTradeAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(table.Rows[0]["TotalTradeAmount"]);
      }
      return correspondentMasterSummary;
    }

    public static CorrespondentMasterInfo[] GetCorrespondentMastersByTPOID(string oId)
    {
      CorrespondentMasterInfo[] mastersFromDatabase = CorrespondentMasters.getCorrespondentMastersFromDatabase("select CorrespondentMasterID from CorrespondentMaster where ExternalID = '" + oId + "'");
      return mastersFromDatabase.Length == 0 ? (CorrespondentMasterInfo[]) null : mastersFromDatabase;
    }

    public static CorrespondentMasterInfo[] GetCorrespondentMastersByOrgID(string oId)
    {
      CorrespondentMasterInfo[] mastersFromDatabase = CorrespondentMasters.getCorrespondentMastersFromDatabase("select CorrespondentMasterID from CorrespondentMaster where OrganizationID = '" + oId + "'");
      return mastersFromDatabase.Length == 0 ? (CorrespondentMasterInfo[]) null : mastersFromDatabase;
    }

    public static CorrespondentMasterInfo GetCorrespondentMastersByContractNumber(string number)
    {
      CorrespondentMasterInfo[] mastersFromDatabase = CorrespondentMasters.getCorrespondentMastersFromDatabase("select CorrespondentMasterID from CorrespondentMaster where ContractNumber = '" + number + "'");
      return mastersFromDatabase.Length == 0 ? (CorrespondentMasterInfo) null : mastersFromDatabase[0];
    }

    private static CorrespondentMasterInfo[] GetCorrespondentMasterByCriteria(string sqlCriteria)
    {
      return CorrespondentMasters.getCorrespondentMastersFromDatabase(sqlCriteria);
    }

    public static string GetNextAutoCreateMasterName()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select TheNumber from FN_GetNextCorrespondentMasterCommitmentNumber()");
      string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(dbQueryBuilder.ExecuteScalar());
      return str.Equals("10000000000") ? "" : str;
    }
  }
}
