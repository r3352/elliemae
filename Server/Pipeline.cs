// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Pipeline
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Interface;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.PipelineEngine;
using EllieMae.EMLite.Trading;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class Pipeline
  {
    private const int MaxGuidsPerQuery = 100;
    private static string className = nameof (Pipeline);
    public const string LoanSummaryFieldPrefix = "Loan�";
    public const string PipelineFieldPrefix = "Pipeline�";
    private static readonly int DefaultServiceSessionRefreshInterval = 10;

    static Pipeline()
    {
      int result;
      if (!int.TryParse(ConfigurationManager.AppSettings["Session.TimeoutMinutes"], out result))
        return;
      Pipeline.DefaultServiceSessionRefreshInterval = result < 5 ? 0 : result - 5;
    }

    public static PipelineInfo GetPipelineInfo(
      UserInfo userInfo,
      string guid,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return Pipeline.GetPipelineInfo(userInfo, guid, fields, dataToInclude, isExternalOrganization, 0);
    }

    public static PipelineInfo GetPipelineInfo(
      UserInfo userInfo,
      string guid,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead)
    {
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate(userInfo, new string[1]
      {
        guid
      }, fields, dataToInclude, (isExternalOrganization ? 1 : 0) != 0, sqlRead);
      return pipelineInfoArray.Length == 0 ? (PipelineInfo) null : pipelineInfoArray[0];
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.Generate(userInfo, guids, fields, dataToInclude, isExternalOrganization, 0, treatNotFoundFieldAsNull, tradeType, excludeArchivedLoans);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      int[] xRefIds,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      int maxGuidsPerQuery = 1000,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.Generate(userInfo, xRefIds, fields, dataToInclude, isExternalOrganization, 0, treatNotFoundFieldAsNull, tradeType, maxGuidsPerQuery, excludeArchivedLoans);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      bool excludeArchivedLoans = false)
    {
      if (guids.Length == 0)
        return new PipelineInfo[0];
      ArrayList arrayList = new ArrayList();
      for (int sourceIndex = 0; sourceIndex < guids.Length; sourceIndex += 100)
      {
        int length = Math.Min(100, guids.Length - sourceIndex);
        string[] strArray = new string[length];
        Array.Copy((Array) guids, sourceIndex, (Array) strArray, 0, length);
        arrayList.AddRange((ICollection) Pipeline.generateFromIDList(userInfo, strArray, fields, dataToInclude, isExternalOrganization, sqlRead, treatNotFoundFieldAsNull, tradeType, excludeArchivedLoans));
      }
      return (PipelineInfo[]) arrayList.ToArray(typeof (PipelineInfo));
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      int[] xRefIds,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      int maxGuidsPerQuery = 1000,
      bool excludeArchivedLoans = false)
    {
      if (xRefIds.Length == 0)
        return new PipelineInfo[0];
      string[] strArray = fields;
      if (!((IEnumerable<string>) fields).Contains<string>("Loan.XRefId"))
      {
        strArray = new string[fields.Length + 1];
        Array.Copy((Array) fields, (Array) strArray, fields.Length);
        strArray[fields.Length] = "Loan.XRefId";
      }
      ArrayList arrayList = new ArrayList();
      for (int sourceIndex = 0; sourceIndex < xRefIds.Length; sourceIndex += maxGuidsPerQuery)
      {
        int length = Math.Min(maxGuidsPerQuery, xRefIds.Length - sourceIndex);
        int[] numArray = new int[length];
        Array.Copy((Array) xRefIds, sourceIndex, (Array) numArray, 0, length);
        arrayList.AddRange((ICollection) Pipeline.generateFromIDList(userInfo, numArray, strArray, dataToInclude, isExternalOrganization, sqlRead, treatNotFoundFieldAsNull, tradeType, excludeArchivedLoans));
      }
      return (PipelineInfo[]) arrayList.ToArray(typeof (PipelineInfo));
    }

    private static PipelineInfo[] generateFromIDList(
      UserInfo userInfo,
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None)
    {
      return Pipeline.generateFromIDList(userInfo, guids, fields, dataToInclude, isExternalOrganization, 0, treatNotFoundFieldAsNull, tradeType);
    }

    private static PipelineInfo[] generateFromIDList(
      UserInfo userInfo,
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      bool excludeArchivedLoans = false)
    {
      LoanFieldTranslator fieldTranslator = new LoanFieldTranslator()
      {
        TranslateNotFoundFieldAsNull = treatNotFoundFieldAsNull
      };
      PipelineInfo[] pipelineInfoArray = Pipeline.generate(userInfo, LoanInfo.Right.Access, (string[]) null, (string) null, guids, fields, dataToInclude, (QueryCriterion) null, (SortField[]) null, (ICriterionTranslator) fieldTranslator, isExternalOrganization, sqlRead, tradeType, excludeArchivedLoans: excludeArchivedLoans);
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < pipelineInfoArray.Length; ++index)
      {
        string guid = pipelineInfoArray[index].GUID;
        insensitiveHashtable[(object) guid] = (object) pipelineInfoArray[index];
      }
      PipelineInfo[] fromIdList = new PipelineInfo[guids.Length];
      for (int index = 0; index < guids.Length; ++index)
      {
        if (insensitiveHashtable.ContainsKey((object) guids[index]))
        {
          fromIdList[index] = (PipelineInfo) insensitiveHashtable[(object) guids[index]];
        }
        else
        {
          TraceLog.WriteWarning(Pipeline.className, "Can not find pipeline info for loan guid " + guids[index]);
          fromIdList[index] = (PipelineInfo) null;
        }
      }
      return fromIdList;
    }

    private static PipelineInfo[] generateFromIDList(
      UserInfo userInfo,
      int[] xRefIds,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead,
      bool treatNotFoundFieldAsNull = false,
      TradeType tradeType = TradeType.None,
      bool excludeArchivedLoans = false)
    {
      LoanFieldTranslator fieldTranslator = new LoanFieldTranslator()
      {
        TranslateNotFoundFieldAsNull = treatNotFoundFieldAsNull
      };
      string identitySelectionQuery = string.Format("select LoanGuid from LoanXRef where XRefId in ({0})", (object) string.Join<int>(",", (IEnumerable<int>) xRefIds));
      PipelineInfo[] pipelineInfoArray = Pipeline.generate(userInfo, LoanInfo.Right.Access, (string[]) null, identitySelectionQuery, (string[]) null, fields, dataToInclude, (QueryCriterion) null, (SortField[]) null, (ICriterionTranslator) fieldTranslator, isExternalOrganization, sqlRead, tradeType, excludeArchivedLoans: excludeArchivedLoans);
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < pipelineInfoArray.Length; ++index)
      {
        int xrefId = pipelineInfoArray[index].Identity.XrefId;
        insensitiveHashtable[(object) xrefId] = (object) pipelineInfoArray[index];
      }
      PipelineInfo[] fromIdList = new PipelineInfo[xRefIds.Length];
      for (int index = 0; index < xRefIds.Length; ++index)
      {
        if (insensitiveHashtable.ContainsKey((object) xRefIds[index]))
        {
          fromIdList[index] = (PipelineInfo) insensitiveHashtable[(object) xRefIds[index]];
        }
        else
        {
          TraceLog.WriteWarning(Pipeline.className, "Can not find pipeline info for loan xRefId " + (object) xRefIds[index]);
          fromIdList[index] = (PipelineInfo) null;
        }
      }
      return fromIdList;
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      bool isExternalOrganization,
      TradeType tradeType)
    {
      return Pipeline.Generate(userInfo, (string) null, rights, fields, dataToInclude, filter, sortFields, isExternalOrganization, tradeType);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      bool isExternalOrganization,
      int sqlRead = 0)
    {
      return Pipeline.Generate(userInfo, (string) null, rights, fields, dataToInclude, filter, sortFields, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return Pipeline.Generate(userInfo, folderName, rights, fields, dataToInclude, filter, (SortField[]) null, isExternalOrganization);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      bool isExternalOrganization,
      TradeType tradeType = TradeType.None,
      int sqlRead = 0)
    {
      return Pipeline.Generate(userInfo, folderName, rights, fields, dataToInclude, filter, sortFields, isExternalOrganization, sqlRead, tradeType);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      bool isExternalOrganization,
      int sqlRead,
      TradeType tradeType = TradeType.None)
    {
      return Pipeline.Generate(userInfo, folderName, rights, fields, dataToInclude, filter, sortFields, (ICriterionTranslator) null, isExternalOrganization, sqlRead, tradeType);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string[] folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      TradeType tradeType = TradeType.None,
      int? maxCount = null,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.Generate(userInfo, folderName, rights, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, 0, tradeType, maxCount, excludeArchivedLoans);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      int sqlRead,
      TradeType tradeType = TradeType.None,
      int? maxCount = null)
    {
      string[] strArray;
      if (!string.IsNullOrWhiteSpace(folderName))
        strArray = new string[1]{ folderName };
      else
        strArray = (string[]) null;
      string[] loanFolders = strArray;
      return Pipeline.generate(userInfo, rights, loanFolders, (string) null, (string[]) null, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, sqlRead, tradeType, maxCount);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string[] folderNames,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      int sqlRead,
      TradeType tradeType = TradeType.None,
      int? maxCount = null,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.generate(userInfo, rights, folderNames, (string) null, (string[]) null, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, sqlRead, tradeType, maxCount, excludeArchivedLoans: excludeArchivedLoans);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string[] folderNames,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      int sqlRead,
      bool isGlobalSearch = false,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.generate(userInfo, rights, folderNames, (string) null, (string[]) null, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, sqlRead, isGlobalSearch: isGlobalSearch, excludeArchivedLoans: excludeArchivedLoans);
    }

    public static PipelineInfo[] Generate(
      UserInfo user,
      LoanInfo.Right rights,
      string identitySelectionQuery,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead = 0,
      QueryCriterion queryCriterion = null,
      SortField[] sortFields = null)
    {
      return Pipeline.generate(user, rights, (string[]) null, identitySelectionQuery, (string[]) null, fields, dataToInclude, queryCriterion, sortFields, (ICriterionTranslator) null, isExternalOrganization, sqlRead);
    }

    public static PipelineInfo[] Generate(
      string identitySelectionQuery,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      TradeType tradeType)
    {
      return Pipeline.generate((UserInfo) null, LoanInfo.Right.Access, (string) null, identitySelectionQuery, (string[]) null, fields, dataToInclude, (QueryCriterion) null, (SortField[]) null, (ICriterionTranslator) null, isExternalOrganization, tradeType);
    }

    public static PipelineInfo[] Generate(
      UserInfo user,
      string identitySelectionQuery,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      TradeType tradeType,
      int sqlRead)
    {
      return Pipeline.generate(user, LoanInfo.Right.Access, (string) null, identitySelectionQuery, (string[]) null, fields, dataToInclude, (QueryCriterion) null, (SortField[]) null, (ICriterionTranslator) null, isExternalOrganization, tradeType, sqlRead);
    }

    public static PipelineInfo[] GenerateWithPagination(
      UserInfo userInfo,
      LoanInfo.Right rights,
      string[] loanFolders,
      string[] guidList,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      CalculateTotalCountEnum calculateTotalCountType,
      out int totalCount,
      int start = 0,
      int limit = 0,
      int? maxLimit = null,
      bool isExternalOrganization = false,
      bool excludeArchivedLoans = true)
    {
      if (guidList != null)
        guidList = ((IEnumerable<string>) guidList).Where<string>((System.Func<string, bool>) (x => !string.IsNullOrWhiteSpace(x))).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase).ToArray<string>();
      if (calculateTotalCountType == CalculateTotalCountEnum.CountOnly)
        return Pipeline.generate(userInfo, rights, loanFolders, (string) null, guidList, new string[0], PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, isExternalOrganization, 0, out totalCount, calculateTotalCount: CalculateTotalCountEnum.CountOnly, excludeArchivedLoans: excludeArchivedLoans);
      PipelinePagination pipelinePagination;
      if (start < 0 || limit <= 0)
        pipelinePagination = (PipelinePagination) null;
      else
        pipelinePagination = new PipelinePagination()
        {
          Start = start,
          Limit = limit
        };
      PipelinePagination paginationInfo = pipelinePagination;
      PipelineInfo[] withPagination = (PipelineInfo[]) null;
      int countInfo = -1;
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      try
      {
        ClientContext context = ClientContext.GetCurrent();
        IDataCache requestCache = ClientContext.CurrentRequest?.RequestCache;
        string correlationId = ClientContext.CurrentRequest?.CorrelationId;
        Guid? transactionId = (Guid?) ClientContext.CurrentRequest?.TransactionId;
        Task task = (Task) null;
        if (calculateTotalCountType != CalculateTotalCountEnum.NoCount)
          task = Task.Run((Action) (() =>
          {
            using (context.MakeCurrent(requestCache, correlationId, transactionId, new bool?()))
              Pipeline.generate(userInfo, rights, loanFolders, (string) null, guidList, new string[0], PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, isExternalOrganization, 0, out countInfo, calculateTotalCount: CalculateTotalCountEnum.CountOnly, excludeArchivedLoans: excludeArchivedLoans);
          }), cancellationTokenSource.Token);
        Stopwatch stopwatch = Stopwatch.StartNew();
        withPagination = Pipeline.generate(userInfo, rights, loanFolders, (string) null, guidList, fields, dataToInclude, filter, sortFields, (ICriterionTranslator) null, isExternalOrganization, 0, maxCount: maxLimit, paginationInfo: paginationInfo, excludeArchivedLoans: excludeArchivedLoans);
        stopwatch.Stop();
        if (task != null && !task.IsCompleted)
        {
          if (calculateTotalCountType == CalculateTotalCountEnum.WaitForCount)
            task.Wait();
          else if (stopwatch.ElapsedMilliseconds > 3000L || !task.Wait((int) (3000L - stopwatch.ElapsedMilliseconds)))
            cancellationTokenSource.Cancel();
        }
        totalCount = countInfo;
      }
      finally
      {
        cancellationTokenSource?.Dispose();
      }
      return withPagination;
    }

    private static PipelineInfo[] generate(
      UserInfo user,
      LoanInfo.Right accessRights,
      string loanFolder,
      string identitySelectionQuery,
      string[] guidList,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      TradeType tradeType = TradeType.None,
      int sqlRead = 0,
      int? maxCount = null)
    {
      string[] strArray;
      if (!string.IsNullOrEmpty(loanFolder))
        strArray = new string[1]{ loanFolder };
      else
        strArray = (string[]) null;
      string[] loanFolders = strArray;
      return Pipeline.generate(user, accessRights, loanFolders, identitySelectionQuery, guidList, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, sqlRead, tradeType, maxCount);
    }

    private static PipelineInfo[] generate(
      UserInfo user,
      LoanInfo.Right accessRights,
      string[] loanFolders,
      string identitySelectionQuery,
      string[] guidList,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      int sqlRead,
      TradeType tradeType = TradeType.None,
      int? maxCount = null,
      PipelinePagination paginationInfo = null,
      bool isGlobalSearch = false,
      bool excludeArchivedLoans = false)
    {
      return Pipeline.generate(user, accessRights, loanFolders, identitySelectionQuery, guidList, fields, dataToInclude, filter, sortFields, fieldTranslator, isExternalOrganization, sqlRead, out int _, tradeType, maxCount, paginationInfo, isGlobalSearch, excludeArchivedLoans: excludeArchivedLoans);
    }

    private static PipelineInfo[] generate(
      UserInfo user,
      LoanInfo.Right accessRights,
      string[] loanFolders,
      string identitySelectionQuery,
      string[] guidList,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization,
      int sqlRead,
      out int totalCount,
      TradeType tradeType = TradeType.None,
      int? maxCount = null,
      PipelinePagination paginationInfo = null,
      bool isGlobalSearch = false,
      CalculateTotalCountEnum calculateTotalCount = CalculateTotalCountEnum.None,
      bool excludeArchivedLoans = false)
    {
      string companySetting1 = Company.GetCompanySetting("POLICIES", "ApplyUserAccessFiltering");
      bool flag1 = string.IsNullOrEmpty(companySetting1) || Convert.ToBoolean(companySetting1);
      bool flag2 = false;
      string companySetting2 = Company.GetCompanySetting("POLICIES", "USEGETMYLOANSFORMYLOANS");
      if (!string.IsNullOrEmpty(companySetting2))
        flag2 = Convert.ToBoolean(companySetting2);
      loanFolders = loanFolders == null || !((IEnumerable<string>) loanFolders).Contains<string>("<All Folders>") ? loanFolders : (string[]) null;
      TraceLog.WriteInfo(Pipeline.className, "Generate Pipeline Query with Global flag : " + isGlobalSearch.ToString());
      PipelineParameters parameters = new PipelineParameters()
      {
        User = user,
        AccessRights = accessRights,
        LoanFolders = loanFolders,
        IdentitySelectionQuery = identitySelectionQuery,
        GuidList = guidList,
        Fields = fields,
        DataToInclude = dataToInclude,
        Filter = filter,
        SortFields = sortFields,
        FieldTranslator = fieldTranslator,
        IsExternalOrganization = isExternalOrganization,
        SqlRead = sqlRead,
        TradeType = tradeType,
        MaxCount = maxCount,
        PaginationInfo = paginationInfo,
        ApplyUserAccessFiltering = flag1,
        UseGetLoansForMyLoans = flag2,
        IsGlobalSearch = isGlobalSearch,
        CalculateCountOnly = calculateTotalCount,
        excludeArchivedLoans = excludeArchivedLoans
      };
      return new PipelineEngineFactory(ClientContext.GetCurrent()).CreateInstance().GeneratePipeline(parameters, out totalCount);
    }

    public static QueryCriterion CreateCombinedFilterCriterion(
      string[] loanFolders,
      QueryCriterion filter)
    {
      if (loanFolders == null)
        return filter;
      if (loanFolders.Length == 1)
      {
        string loanFolder = loanFolders[0];
        if (loanFolders == null)
          return filter;
        return filter == null ? (QueryCriterion) new StringValueCriterion("Loan.LoanFolder", loanFolder, StringMatchType.CaseInsensitive) : filter.And((QueryCriterion) new StringValueCriterion("Loan.LoanFolder", loanFolder));
      }
      return !((IEnumerable<string>) loanFolders).Any<string>((System.Func<string, bool>) (folder => !string.IsNullOrWhiteSpace(folder))) ? filter : (QueryCriterion) new StringValueCriterion("Loan.LoanFolder", EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) loanFolders), StringMatchType.MultiValue);
    }

    public static PipelineInfo[] GenerateInvisibleContactLoans(string[] noAccessXrefId)
    {
      if (noAccessXrefId == null || noAccessXrefId.Length == 0)
        return new PipelineInfo[0];
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      dbQueryBuilder.Append("Select Loan.Guid, Loan.DateFileOpened, Loan.LoanNumber, Loan.CurrentMilestoneName, Loan.IsArchived from LoanSummary Loan  where xrefId in (" + string.Join(", ", noAccessXrefId) + ")");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          pipelineInfoList.Add(new PipelineInfo(new Hashtable()
          {
            {
              (object) "Loan.DataFileOpened",
              (object) string.Concat(row["DateFileOpened"])
            },
            {
              (object) "Loan.LoanNumber",
              (object) string.Concat(row["LoanNumber"])
            },
            {
              (object) "Loan.CurrentMilestoneName",
              (object) string.Concat(row["CurrentMilestoneName"])
            },
            {
              (object) "Loan.Guid",
              (object) string.Concat(row["Guid"])
            },
            {
              (object) "Loan.IsArchived",
              (object) string.Concat(row["IsArchived"])
            }
          }, (PipelineInfo.Borrower[]) null, (PipelineInfo.Alert[]) null, (PipelineInfo.LoanAssociateInfo[]) null, (LockInfo) null, (Hashtable) null));
      }
      return pipelineInfoList.ToArray();
    }

    public static string EncodeParseableGuidList(string[] guids)
    {
      return EllieMae.EMLite.DataAccess.SQL.Encode((object) string.Join("", guids));
    }

    public static string EncodeParseableGuidList(string[] guids, string separator)
    {
      return EllieMae.EMLite.DataAccess.SQL.Encode((object) string.Join(separator, guids));
    }

    public static string EncodeGuidList(string[] guids)
    {
      return string.Join(", ", (IEnumerable<string>) ((IEnumerable<string>) guids).Select<string, string>((System.Func<string, string>) (guid => EllieMae.EMLite.DataAccess.SQL.EncodeString(guid))).ToList<string>());
    }

    public static LoanIdentity[] GenerateIdentities(
      UserInfo user,
      LoanInfo.Right rights,
      QueryCriterion filter,
      ICriterionTranslator fieldTranslator,
      bool isExternalOrganization)
    {
      string[] fields = new string[4]
      {
        "Loan.Guid",
        "Loan.LoanFolder",
        "Loan.LoanName",
        "Loan.XrefId"
      };
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate(user, (string[]) null, rights, fields, PipelineData.Fields, filter, (SortField[]) null, fieldTranslator, isExternalOrganization);
      LoanIdentity[] identities = new LoanIdentity[pipelineInfoArray.Length];
      for (int index = 0; index < pipelineInfoArray.Length; ++index)
        identities[index] = pipelineInfoArray[index].Identity;
      return identities;
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return Pipeline.Generate(userInfo, roleID, userID, folderName, fields, dataToInclude, LoanInfo.Right.FullRight, isExternalOrganization);
    }

    public static PipelineInfo[] Generate(
      UserInfo userInfo,
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      LoanInfo.Right loanRight,
      bool isExternalOrganization,
      string externalOrgID = null,
      int sqlRead = 0,
      QueryCriterion queryCriterion = null,
      SortField[] sortFields = null)
    {
      string identitySelectionQuery = !(userID != "") ? "select distinct LS.Guid from LoanSummary LS inner join LoanAssociates LA on LS.Guid = LA.Guid where IsNull(LA.UserID, '') != '' " : "select distinct LS.Guid from LoanSummary LS inner join LoanAssociates LA on LS.Guid = LA.Guid where LA.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID);
      if (roleID != "")
        identitySelectionQuery = identitySelectionQuery + " and LA.RoleID = " + roleID;
      if ((folderName ?? "") != "")
        identitySelectionQuery = identitySelectionQuery + " and LS.LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName);
      if (isExternalOrganization && !string.IsNullOrEmpty(externalOrgID))
      {
        string str = identitySelectionQuery + " AND ((LS.[TPOLOID] IS NOT NULL AND [TPOLOID] <> '') OR (LS.[TPOLPID] IS NOT NULL AND [TPOLPID] <> ''))";
        identitySelectionQuery = !(externalOrgID != "-1") ? str + " AND (LS.[TPOCompanyID] IS NOT NULL AND [TPOCompanyID] <> '')" : str + " AND (LS.[TPOCompanyID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrgID) + ")";
      }
      return Pipeline.Generate(userInfo, loanRight, identitySelectionQuery, fields, dataToInclude, isExternalOrganization, sqlRead, queryCriterion, sortFields);
    }

    public static string GetUserVisibleIDQuery(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      bool isExternalOrganization)
    {
      return Pipeline.GetUserVisibleIDQuery(userInfo, (string) null, accessRights, isExternalOrganization);
    }

    public static string GetUserVisibleIDQuery(
      UserInfo userInfo,
      string folderName,
      LoanInfo.Right minAccessRights,
      bool isExternalOrganization)
    {
      UserInfo userInfo1 = userInfo;
      string[] loanFolders;
      if (!string.IsNullOrWhiteSpace(folderName))
        loanFolders = new string[1]{ folderName };
      else
        loanFolders = (string[]) null;
      int minAccessRights1 = (int) minAccessRights;
      int num = isExternalOrganization ? 1 : 0;
      string filterJoinClause = Pipeline.GetUserVisibleIDQueryFilterJoinClause(userInfo1, loanFolders, (LoanInfo.Right) minAccessRights1, num != 0, (QueryCriterion) null);
      return filterJoinClause == "" ? "" : ("select Loan.Guid from LoanSummary Loan" + Environment.NewLine + filterJoinClause).Trim();
    }

    public static string GetUserVisibleIDQueryFilterJoinClause(
      UserInfo userInfo,
      string[] loanFolders,
      LoanInfo.Right minAccessRights,
      bool isExternalOrganization,
      QueryCriterion filter)
    {
      if (userInfo == (UserInfo) null || userInfo.IsSuperAdministrator())
      {
        StringBuilder stringBuilder = new StringBuilder("");
        if (loanFolders != null && loanFolders.Length != 0)
          stringBuilder.Append("  inner join #loanFolders _vid_fol on _vid_fol.Name = Loan.LoanFolder ");
        return stringBuilder.ToString();
      }
      bool flag = ((IEnumerable<AclGroup>) AclGroupAccessor.GetGroupsOfUser(userInfo.Userid)).Any<AclGroup>((System.Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
      if (isExternalOrganization)
      {
        Hashtable hashtable = FeaturesAclDbAccessor.CheckPermissions(new AclFeature[2]
        {
          AclFeature.ExternalSettings_ContactSalesRep,
          AclFeature.ExternalSettings_OrganizationSettings
        }, userInfo);
        if (flag || !(bool) hashtable[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) hashtable[(object) AclFeature.ExternalSettings_OrganizationSettings])
        {
          StringBuilder stringBuilder = new StringBuilder("");
          if (loanFolders != null && loanFolders.Length != 0)
            stringBuilder.Append("  inner join #loanFolders _vid_fol on _vid_fol.Name = Loan.LoanFolder ");
          return stringBuilder.ToString();
        }
      }
      string pipelineQueryMethod = ClientContext.GetCurrent().Settings.PipelineQueryMethod;
      if (!isExternalOrganization)
      {
        if (minAccessRights == LoanInfo.Right.FullRight)
          return loanFolders == null ? "  inner join FN_GetUsersAssignableLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid" : "  inner join FN_GetUsersAssignableLoansByFolders(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
        string companySetting = Company.GetCompanySetting("migration", "AccessibleLoanOwnerOrgs");
        switch (pipelineQueryMethod)
        {
          case "inline":
            return "  inner join " + (string.IsNullOrWhiteSpace(companySetting) ? "FN_GetUsersAccessibleLoansInlineByFolders" : "FN_GetUsersAccessibleLoansInlineByFolders_org") + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          case "table":
            return "  inner join " + (string.IsNullOrWhiteSpace(companySetting) ? "FN_GetUsersAccessibleLoansTableByFolders" : "FN_GetUsersAccessibleLoansTableByFolders_org") + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          default:
            return Pipeline.GetUserAccessFilterMethod(userInfo, filter, companySetting, loanFolders);
        }
      }
      else
      {
        string str = userInfo.Userid;
        if (filter != null)
        {
          string sqlClause = filter.ToSQLClause();
          if (sqlClause != null && !sqlClause.Contains("[Loan].[TPOLOID]"))
            str = userInfo.PeerView != UserInfo.UserPeerView.Disabled ? Pipeline.getUserList(userInfo.OrgId) : userInfo.Userid;
        }
        else
          str = userInfo.PeerView != UserInfo.UserPeerView.Disabled ? Pipeline.getUserList(userInfo.OrgId) : userInfo.Userid;
        if (loanFolders == null)
        {
          switch (pipelineQueryMethod)
          {
            case "inline":
              return "  inner join FN_GetAEAccessibleLoansInline('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
            case "table":
              return "  inner join FN_GetAEAccessibleLoansTable('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
            default:
              return "  inner join FN_GetAEAccessibleLoans('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
          }
        }
        else
        {
          switch (pipelineQueryMethod)
          {
            case "inline":
              return "  inner join FN_GetAEAccessibleLoansInlineByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
            case "table":
              return "  inner join FN_GetAEAccessibleLoansTableByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
            default:
              return "  inner join FN_GetAEAccessibleLoansByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          }
        }
      }
    }

    public static string GetUserVisibleIDQueryFilterJoinClause(
      UserInfo userInfo,
      string[] loanFolders,
      LoanInfo.Right minAccessRights,
      bool isExternalOrganization,
      QueryCriterion filter,
      bool isGlobalSearch = false,
      bool isOptFlow = false)
    {
      if (userInfo == (UserInfo) null || userInfo.IsSuperAdministrator())
      {
        StringBuilder stringBuilder = new StringBuilder("");
        if (loanFolders != null && loanFolders.Length != 0)
          stringBuilder.Append("  inner join #loanFolders _vid_fol on _vid_fol.Name = Loan.LoanFolder ");
        return stringBuilder.ToString();
      }
      bool flag = ((IEnumerable<AclGroup>) AclGroupAccessor.GetGroupsOfUser(userInfo.Userid)).Any<AclGroup>((System.Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
      if (isExternalOrganization)
      {
        Hashtable hashtable = FeaturesAclDbAccessor.CheckPermissions(new AclFeature[2]
        {
          AclFeature.ExternalSettings_ContactSalesRep,
          AclFeature.ExternalSettings_OrganizationSettings
        }, userInfo);
        if (flag || !(bool) hashtable[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) hashtable[(object) AclFeature.ExternalSettings_OrganizationSettings])
        {
          StringBuilder stringBuilder = new StringBuilder("");
          if (loanFolders != null && loanFolders.Length != 0)
            stringBuilder.Append("  inner join #loanFolders _vid_fol on _vid_fol.Name = Loan.LoanFolder ");
          return stringBuilder.ToString();
        }
      }
      string pipelineQueryMethod = ClientContext.GetCurrent().Settings.PipelineQueryMethod;
      if (!isExternalOrganization)
      {
        if (minAccessRights == LoanInfo.Right.FullRight)
          return loanFolders == null ? "  inner join FN_GetUsersAssignableLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid" : "  inner join FN_GetUsersAssignableLoansByFolders(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
        string companySetting = Company.GetCompanySetting("migration", "AccessibleLoanOwnerOrgs");
        switch (pipelineQueryMethod)
        {
          case "inline":
            return "  inner join " + (string.IsNullOrWhiteSpace(companySetting) ? "FN_GetUsersAccessibleLoansInlineByFolders" : "FN_GetUsersAccessibleLoansInlineByFolders_org") + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          case "table":
            return "  inner join " + (string.IsNullOrWhiteSpace(companySetting) ? "FN_GetUsersAccessibleLoansTableByFolders" : "FN_GetUsersAccessibleLoansTableByFolders_org") + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          default:
            return Pipeline.GetUserAccessFilterMethod(userInfo, filter, companySetting, loanFolders, isGlobalSearch, isOptFlow);
        }
      }
      else
      {
        string str = userInfo.Userid;
        if (filter != null)
        {
          string sqlClause = filter.ToSQLClause();
          if (sqlClause != null && !sqlClause.Contains("[Loan].[TPOLOID]"))
            str = userInfo.PeerView != UserInfo.UserPeerView.Disabled ? Pipeline.getUserList(userInfo.OrgId) : userInfo.Userid;
        }
        else
          str = userInfo.PeerView != UserInfo.UserPeerView.Disabled ? Pipeline.getUserList(userInfo.OrgId) : userInfo.Userid;
        if (loanFolders == null)
        {
          switch (pipelineQueryMethod)
          {
            case "inline":
              return "  inner join FN_GetAEAccessibleLoansInline('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
            case "table":
              return "  inner join FN_GetAEAccessibleLoansTable('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
            default:
              return "  inner join FN_GetAEAccessibleLoans('" + str + "', null) _vid_ual on _vid_ual.Guid = Loan.Guid";
          }
        }
        else
        {
          switch (pipelineQueryMethod)
          {
            case "inline":
              return "  inner join FN_GetAEAccessibleLoansInlineByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
            case "table":
              return "  inner join FN_GetAEAccessibleLoansTableByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
            default:
              return "  inner join FN_GetAEAccessibleLoansByFolders('" + str + "', @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid";
          }
        }
      }
    }

    private static bool IsFilterUserCurrentUser(UserInfo userInfo, QueryCriterion filter)
    {
      string empty = string.Empty;
      switch (filter)
      {
        case StringValueCriterion _ when !Pipeline.IsSameUser(userInfo, filter):
          return false;
        case BinaryOperation _:
          foreach (QueryCriterion criterion1 in ((BinaryOperation) filter).Criteria)
          {
            switch (criterion1)
            {
              case StringValueCriterion _ when !Pipeline.IsSameUser(userInfo, criterion1):
                return false;
              case BinaryOperation _:
                foreach (QueryCriterion criterion2 in ((BinaryOperation) criterion1).Criteria)
                {
                  if (!Pipeline.IsFilterUserCurrentUser(userInfo, criterion2))
                    return false;
                }
                break;
            }
          }
          break;
      }
      return true;
    }

    private static bool IsSameUser(UserInfo userInfo, QueryCriterion criterion)
    {
      return !("LoanAssociateUser.UserID" == ((FieldValueCriterion) criterion).FieldName) || userInfo.Userid == ((StringValueCriterion) criterion).Value && ((StringValueCriterion) criterion).MatchType == StringMatchType.Exact;
    }

    private static string GetUserGuid(string userId, bool isOptflow)
    {
      string companySetting = Company.GetCompanySetting("POLICIES", "OPTPIPELINEQUERY");
      if (isOptflow && string.Equals(companySetting, "True", StringComparison.OrdinalIgnoreCase))
        return "inner join UserLoans ul on ul.LoanXRef = Loan.XRefID -- NewOPTFLOWinner join AccessibleLoanOwners alo on ul.UserID = alo.OwnerID inner join AclGroupLoanFolderAccess aglfa on Loan.LoanFolder = aglfa.FolderName inner join AclGroupMembers agmu on agmu.GroupID = aglfa.GroupID and agmu.UserID = alo.UserID ";
      return "  inner join ( select distinct ls.Guid\r\n    from (\r\n    select distinct ls.Guid, ls.LoanFolder\r\n    from LoanSummary ls \r\n    inner join UserLoans ul on ul.LoanXRef = ls.XRefID\r\n    inner join AccessibleLoanOwners alo on ul.UserID = alo.OwnerID\r\n    where\r\n    alo.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + "\r\n    and exists (select 1 \r\n                from #loanFolders lf\r\n    where lf.[name] = ls.LoanFolder)) ls\r\n    inner join AclGroupLoanFolderAccess aglfa on ls.LoanFolder = aglfa.FolderName\r\n    inner join AclGroupMembers agmu on agmu.GroupID = aglfa.GroupID\r\n    where aglfa.Access = 1 \r\n    and agmu.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + ") _vid_ual on _vid_ual.Guid = Loan.Guid";
    }

    private static string GetUserAccessFilterMethod(
      UserInfo userInfo,
      QueryCriterion filter,
      string useOrg,
      string[] loanFolders)
    {
      bool flag = false;
      string companySetting = Company.GetCompanySetting("POLICIES", "USEGETMYLOANSFORMYLOANS");
      if (!string.IsNullOrEmpty(companySetting))
        flag = filter != null && Convert.ToBoolean(companySetting) && Convert.ToBoolean(companySetting) && filter.UsesField("LoanAssociateUser.UserId") && Pipeline.IsFilterUserCurrentUser(userInfo, filter);
      return loanFolders != null ? (!flag ? Pipeline.GetUserGuid(userInfo.Userid, false) : "  inner join FN_GetMyLoansByFolders(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid") : (!flag ? "  inner join " + (string.IsNullOrWhiteSpace(useOrg) ? Pipeline.GetUserAccessibleLoans(userInfo.Userid) : "FN_GetUsersAccessibleLoans_org (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", null)") + " _vid_ual on _vid_ual.Guid = Loan.Guid" : "  inner join FN_GetMyLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid");
    }

    private static string GetUserAccessibleLoans(string userID)
    {
      return "( \r\n select distinct ils.Guid\r\nfrom (\r\nselect distinct ils.Guid, ils.LoanFolder\r\nfrom LoanSummary ils \r\ninner join UserLoans ul on ul.LoanXRef = ils.XRefID\r\ninner join AccessibleLoanOwners alo on ul.UserID = alo.OwnerID\r\n --newaccessibleflow Left Outer join AllLoanFolders ilf ON ils.LoanFolder = ilf.folderName  AND ilf.archive != 'Y' \r\nwhere\r\nalo.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID) + ") ils\r\ninner join AclGroupLoanFolderAccess aglfa on ils.LoanFolder = aglfa.FolderName\r\ninner join AclGroupMembers agmu on agmu.GroupID = aglfa.GroupID\r\nwhere aglfa.Access = 1 \r\nand agmu.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID) + ")";
    }

    private static string GetUserAccessFilterMethod(
      UserInfo userInfo,
      QueryCriterion filter,
      string useOrg,
      string[] loanFolders,
      bool isGlobalSearch = false,
      bool isOptflow = false)
    {
      bool flag = false;
      string companySetting = Company.GetCompanySetting("POLICIES", "USEGETMYLOANSFORMYLOANS");
      if (!string.IsNullOrEmpty(companySetting))
        flag = filter != null && Convert.ToBoolean(companySetting) && Convert.ToBoolean(companySetting) && filter.UsesField("LoanAssociateUser.UserId") && Pipeline.IsFilterUserCurrentUser(userInfo, filter);
      return loanFolders != null ? (!flag ? Pipeline.GetUserGuid(userInfo.Userid, isOptflow) : "  inner join FN_GetMyLoansByFolders(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", @loanFolders) _vid_ual on _vid_ual.Guid = Loan.Guid") : (!flag ? Pipeline.getAccessesibleFolder(useOrg, userInfo.Userid, isGlobalSearch) : "  inner join FN_GetMyLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid");
    }

    private static string getAccessesibleFolder(string useOrg, string userid, bool isGlobalSearch)
    {
      if (!(string.Compare(Company.GetCompanySetting("POLICIES", "UsePiplineOptimization"), "True", true) == 0 | isGlobalSearch))
        return "  inner join " + (string.IsNullOrWhiteSpace(useOrg) ? Pipeline.GetUserAccessibleLoans(userid) : "FN_GetUsersAccessibleLoans_org (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid) + ", null)") + " _vid_ual on _vid_ual.Guid = Loan.Guid";
      return !string.IsNullOrWhiteSpace(useOrg) ? "  inner join FN_GetUsersAccessibleLoans_org(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid" : "  inner join #usrloanFolders lf on  lf.Name = Loan.loanfolder inner join ( select ul.LoanXRef from UserLoans ul inner join AccessibleLoanOwners alo on ul.UserID = alo.OwnerID where alo.UserID = '" + userid + "' group by ul.LoanXRef ) ul on ul.LoanXRef = Loan.xrefid";
    }

    private static string getUserList(int orgId)
    {
      UserInfo[] underOrganization = User.GetUsersUnderOrganization(orgId);
      List<string> values = new List<string>();
      for (int index = 0; index < underOrganization.Length; ++index)
      {
        UserInfo userInfo = underOrganization[index];
        values.Add(userInfo.Userid.Replace("'", "''"));
      }
      return string.Join(",", (IEnumerable<string>) values);
    }

    private static int[] getGroupIDsOfUser(string userId)
    {
      AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userId);
      int[] groupIdsOfUser = new int[groupsOfUser.Length];
      for (int index = 0; index < groupsOfUser.Length; ++index)
        groupIdsOfUser[index] = groupsOfUser[index].ID;
      return groupIdsOfUser;
    }

    public static LoanInfo.Right GetEffectiveLoanAccessRights(string guid, UserInfo userInfo)
    {
      return Pipeline.GetEffectiveLoanAccessRights(new string[1]
      {
        guid
      }, userInfo)[0];
    }

    public static LoanInfo.Right[] GetEffectiveLoanAccessRights(string[] guids, UserInfo userInfo)
    {
      LoanInfo.Right[] source = new LoanInfo.Right[guids.Length];
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      bool flag1 = UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas);
      ClientContext current = ClientContext.GetCurrent();
      for (int index = 0; index < source.Length; ++index)
      {
        source[index] = flag1 ? LoanInfo.Right.FullRight : LoanInfo.Right.NoRight;
        insensitiveHashtable[(object) guids[index]] = (object) index;
      }
      if (flag1)
        return source;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = string.Join("", guids);
      DataRowCollection dataRowCollection1;
      if (Company.GetCompanySetting("FEATURE", "EnableOptLoanAccessRights").ToLower() == "true")
      {
        dbQueryBuilder.AppendLine("");
        dbQueryBuilder.AppendLine("GetEffectiveLoanAccessRights_Opt");
        dataRowCollection1 = dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Default, new DbValueList()
        {
          {
            "@userId",
            (object) userInfo.Userid
          },
          {
            "@guid_str",
            (object) str
          }
        });
      }
      else
      {
        bool flag2 = ((IEnumerable<AclGroup>) AclGroupAccessor.GetGroupsOfUser(userInfo.Userid)).Any<AclGroup>((System.Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
        if (!current.IsTPOClient.HasValue)
        {
          dbQueryBuilder.AppendLine("select top 1 * from ExternalOrgDetail");
          DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
          dbQueryBuilder.Reset();
          current.IsTPOClient = new bool?(dataRowCollection2 != null && dataRowCollection2.Count > 0);
        }
        bool? isTpoClient = current.IsTPOClient;
        bool flag3 = true;
        if (isTpoClient.GetValueOrDefault() == flag3 & isTpoClient.HasValue && ((IEnumerable<LoanInfo.Right>) source).Contains<LoanInfo.Right>(LoanInfo.Right.NoRight) && userInfo.UserType != "External")
        {
          Hashtable hashtable = FeaturesAclDbAccessor.CheckPermissions(new AclFeature[2]
          {
            AclFeature.ExternalSettings_ContactSalesRep,
            AclFeature.ExternalSettings_OrganizationSettings
          }, userInfo);
          int num = userInfo.OrgId;
          if (flag2 || !(bool) hashtable[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) hashtable[(object) AclFeature.ExternalSettings_OrganizationSettings])
            num = 0;
          string pipelineQueryMethod = current.Settings.PipelineQueryMethod;
          dbQueryBuilder.AppendLine("");
          dbQueryBuilder.AppendLine("-- Create the GUID list table");
          dbQueryBuilder.AppendLine("declare @ae_loan_guids table ( guid varchar(38) primary key)");
          dbQueryBuilder.AppendLine("insert into @ae_loan_guids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(guids) + ")");
          dbQueryBuilder.AppendLine("");
          switch (pipelineQueryMethod)
          {
            case "inline":
              dbQueryBuilder.AppendLine("select _vid_ual.guid from FN_GetAEAccessibleLoansInlineOpt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) num) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DBNull.Value) + ") _vid_ual inner join @ae_loan_guids ae_loan on _vid_ual.Guid = ae_loan.guid");
              break;
            case "table":
              dbQueryBuilder.AppendLine("select _vid_ual.guid from FN_GetAEAccessibleLoansTableOpt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) num) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DBNull.Value) + ") _vid_ual inner join @ae_loan_guids ae_loan on _vid_ual.Guid = ae_loan.guid");
              break;
            default:
              dbQueryBuilder.AppendLine("select _vid_ual.guid from FN_GetAEAccessibleLoansOpt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) num) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DBNull.Value) + ") _vid_ual inner join @ae_loan_guids ae_loan on _vid_ual.Guid = ae_loan.guid");
              break;
          }
          DataRowCollection dataRowCollection3 = dbQueryBuilder.Execute(DbTransactionType.Snapshot);
          dbQueryBuilder.Reset();
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection3)
          {
            int index = (int) insensitiveHashtable[dataRow["guid"]];
            if (source[index] == LoanInfo.Right.NoRight)
              source[index] = LoanInfo.Right.Access;
          }
        }
        string companySetting = Company.GetCompanySetting("migration", "AccessibleLoanOwnerOrgs");
        EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid);
        if (!string.IsNullOrWhiteSpace(companySetting))
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("");
          dbQueryBuilder.AppendLine("GetEffectiveLoanAccessRights_Org");
          dataRowCollection1 = dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, new DbValueList()
          {
            {
              "@userId",
              (object) userInfo.Userid
            },
            {
              "@guid_str",
              (object) str
            }
          });
        }
        else
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("");
          dbQueryBuilder.AppendLine(nameof (GetEffectiveLoanAccessRights));
          dataRowCollection1 = dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, new DbValueList()
          {
            {
              "@userId",
              (object) userInfo.Userid
            },
            {
              "@guid_str",
              (object) str
            }
          });
        }
      }
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
      {
        int num = (int) dataRow["rights"];
        LoanInfo.Right right = num == 0 ? LoanInfo.Right.Read : (LoanInfo.Right) num;
        if (source[(int) insensitiveHashtable[dataRow["guid"]]] == LoanInfo.Right.NoRight)
          source[(int) insensitiveHashtable[dataRow["guid"]]] = right;
      }
      return source;
    }

    public static void Rebuild(
      UserInfo currentUser,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild,
      string sessionId)
    {
      ClientContext current = ClientContext.GetCurrent();
      CustomLevelLog customLevelLog1 = new CustomLevelLog("EVENT");
      customLevelLog1.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      customLevelLog1.Src = "Pipeline:Rebuild\\Start";
      customLevelLog1.Message = "Full Rebuild of all loan folders" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'";
      CustomLevelLog log1 = customLevelLog1;
      log1.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.Start);
      DiagUtility.DefaultLogger.Write<CustomLevelLog>(log1);
      Err.WriteApplicationEvent("Full Rebuild of all loan folders" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2000, EventLogEntryCategory.None);
      bool flag = false;
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings();
        LoanAlertMonitor loanAlertMonitor = LoanConfiguration.GetLoanAlertMonitor();
        TraceLog.WriteVerbose(Pipeline.className, "About to build custom field calculators.");
        using (CustomFieldCalculators calcs = new CustomFieldCalculators(loanSettings.FieldSettings.CustomFields))
        {
          TraceLog.WriteVerbose(Pipeline.className, "Completed build of custom field calculators. Calculations found = " + (object) calcs.Count);
          Pipeline.rebuildInternal(calcs, currentUser, loanSettings, loanAlertMonitor, feedback, dbToRebuild, false, sessionId);
        }
        feedback?.SetFeedback("Freeing unused space...", "", -1);
        Loan.RemoveOrphanedLoanData();
        flag = true;
      }
      catch (Exception ex)
      {
        CustomLevelLog log2 = new CustomLevelLog("EVENT");
        log2.Level = Encompass.Diagnostics.Logging.LogLevel.ERROR.Force();
        log2.Src = "Pipeline:Rebuild\\FAIL";
        log2.Message = "Unhandled exception occured while rebuilding pipeline 3";
        log2.Error = new LogErrorData(ex);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log2);
      }
      finally
      {
        CustomLevelLog customLevelLog2 = new CustomLevelLog("EVENT");
        customLevelLog2.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
        customLevelLog2.Src = "Pipeline:Rebuild\\Stop";
        customLevelLog2.Message = "Full Rebuild of all loan folders " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds";
        CustomLevelLog log3 = customLevelLog2;
        log3.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.End);
        log3.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) stopwatch.ElapsedMilliseconds);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log3);
        Err.WriteApplicationEvent("Full Rebuild of all loan folders " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2000, EventLogEntryCategory.None);
      }
    }

    private static void rebuildInternal(
      CustomFieldCalculators calcs,
      UserInfo currentUser,
      ILoanSettings loanSettings,
      LoanAlertMonitor alertMonitor,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild,
      bool isExternalOrganization,
      string sessionId)
    {
      foreach (string folderName in !(currentUser == (UserInfo) null) ? LoanFolder.GetAllLoanFolderNames(true, currentUser) : LoanFolder.GetAllLoanFolderNames(true))
      {
        if (feedback != null && feedback.Cancel)
        {
          TraceLog.WriteInfo(Pipeline.className, "Rebuild pipeline operation aborted by user.");
          return;
        }
        try
        {
          Pipeline.rebuildInternal(folderName, calcs, currentUser, loanSettings, alertMonitor, feedback, dbToRebuild, isExternalOrganization, sessionId);
        }
        catch
        {
        }
      }
      string[] namesFromDatabase = LoanFolder.GetAllLoanFolderNamesFromDatabase(true);
      if (feedback != null && namesFromDatabase.Length != 0)
      {
        if (!feedback.ResetCounter(namesFromDatabase.Length))
          return;
        feedback.SetFeedback("Cleaning loan folders...", "", -1);
      }
      for (int index = 0; index < namesFromDatabase.Length; ++index)
      {
        LoanFolder loanFolder = new LoanFolder(namesFromDatabase[index]);
        if (feedback != null && !feedback.SetFeedback((string) null, "Processing folder '" + loanFolder.Name + "'...", index))
          break;
        if (!loanFolder.Exists)
        {
          foreach (LoanIdentity content in loanFolder.GetContents())
          {
            using (Loan loan = LoanStore.CheckOut(content.Guid))
            {
              loan.Unlock();
              loan.Delete(true, currentUser, isExternalOrganization);
            }
          }
        }
      }
    }

    public static void Rebuild(
      string folderName,
      UserInfo currentUser,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild,
      bool isExternalOrganization,
      string sessionId)
    {
      ClientContext current = ClientContext.GetCurrent();
      CustomLevelLog customLevelLog1 = new CustomLevelLog("EVENT");
      customLevelLog1.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      customLevelLog1.Src = "Pipeline:Rebuild\\Start";
      customLevelLog1.Message = "Full Rebuild of folder '" + folderName + "'" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'";
      CustomLevelLog log1 = customLevelLog1;
      log1.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.Start);
      DiagUtility.DefaultLogger.Write<CustomLevelLog>(log1);
      Err.WriteApplicationEvent("Full Rebuild of folder '" + folderName + "'" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2001, EventLogEntryCategory.None);
      Stopwatch stopwatch = Stopwatch.StartNew();
      bool flag = false;
      try
      {
        CustomFieldsInfo loanCustomFields = SystemConfiguration.GetLoanCustomFields();
        ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings();
        LoanAlertMonitor loanAlertMonitor = LoanConfiguration.GetLoanAlertMonitor();
        TraceLog.WriteVerbose(Pipeline.className, "About to build custom field calculators.");
        using (CustomFieldCalculators calcs = new CustomFieldCalculators(loanCustomFields))
        {
          TraceLog.WriteVerbose(Pipeline.className, "Completed build of custom field calculators. Calculations found = " + (object) calcs.Count);
          Pipeline.rebuildInternal(folderName, calcs, currentUser, loanSettings, loanAlertMonitor, feedback, dbToRebuild, isExternalOrganization, sessionId);
        }
        flag = true;
      }
      finally
      {
        CustomLevelLog customLevelLog2 = new CustomLevelLog("EVENT");
        customLevelLog2.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
        customLevelLog2.Src = "Pipeline:Rebuild\\Stop";
        customLevelLog2.Message = "Full Rebuild of folder '" + folderName + "' " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds";
        CustomLevelLog log2 = customLevelLog2;
        log2.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.End);
        log2.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) stopwatch.ElapsedMilliseconds);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log2);
        Err.WriteApplicationEvent("Full Rebuild of folder '" + folderName + "' " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2001, EventLogEntryCategory.None);
      }
    }

    private static void rebuildInternal(
      string folderName,
      CustomFieldCalculators calcs,
      UserInfo currentUser,
      ILoanSettings loanSettings,
      LoanAlertMonitor alertMonitor,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild,
      bool isExternalOrganization,
      string sessionId)
    {
      ClientContext context = ClientContext.GetCurrent();
      IDataCache requestCache = ClientContext.CurrentRequest?.RequestCache;
      string correlationId = ClientContext.CurrentRequest?.CorrelationId;
      Guid? transactionId = (Guid?) ClientContext.CurrentRequest?.TransactionId;
      try
      {
        TraceLog.WriteVerbose(Pipeline.className, "Rebuilding loan folder '" + folderName + "'...");
        LoanFolder fol = new LoanFolder(folderName);
        if (feedback != null)
        {
          if (!feedback.ResetCounter(1))
            return;
          feedback.SetFeedback("Scanning contents of \"" + fol.Name + "\"...", "", 0);
        }
        string[] loanDirs = fol.GetLoanDirectoriesFromDisk();
        if (feedback != null && loanDirs.Length != 0)
        {
          if (!feedback.ResetCounter(loanDirs.Length))
            return;
          feedback.Status = "Rebuilding \"" + fol.Name + "\"...";
        }
        int num1 = loanDirs.Length / 10;
        if (num1 == 0)
          num1 = 1;
        int num2 = Utils.ParseInt(EnConfigurationSettings.GlobalSettings["MaxProgressInterval", (object) "50"]);
        if (num2 > 0 && num1 > num2)
          num1 = num2;
        int num3 = Utils.ParseInt((object) Company.GetCompanySetting("FEATURE", "RebuildPipelineThreadCount"), 1);
        int num4 = Utils.ParseInt(context.Settings.GetServerSetting("FEATURE.RebuildPipelineMaxThreadCount"), 1);
        if (num3 > num4)
          num3 = num4;
        ILogger logger = DiagUtility.LogManager.GetLogger("PipelineRebuild");
        logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, Pipeline.className, string.Format("Rebuilding pipeline started for loan folder '{0}' having '{1}' loans with '{2:N0}' threads.", (object) folderName, (object) loanDirs.Length, (object) num3));
        Stopwatch stopwatch = Stopwatch.StartNew();
        DateTime lastRefreshed = DateTime.MinValue;
        if (num3 > 1)
        {
          try
          {
            int i = 0;
            string[] source = loanDirs;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = num3;
            Action<string> body = (Action<string>) (loanDir =>
            {
              Interlocked.Increment(ref i);
              if (feedback != null && !feedback.SetFeedback((string) null, string.Format("Reading loan information ({0}/{1})...", (object) i, (object) loanDirs.Length.ToString("#,##0")), i))
                return;
              if (DateTime.Now - lastRefreshed > TimeSpan.FromMinutes((double) Pipeline.DefaultServiceSessionRefreshInterval))
              {
                context.Sessions.GetServiceSessionInfo(sessionId, true);
                lastRefreshed = DateTime.Now;
              }
              using (context.MakeCurrent(requestCache, correlationId, transactionId, new bool?()))
                Pipeline.rebuildLoanInternal(folderName, loanDir, calcs, currentUser, loanSettings, alertMonitor, dbToRebuild, false, sessionId);
            });
            Parallel.ForEach<string>((IEnumerable<string>) source, parallelOptions, body);
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(Pipeline.className, "Parallel rebuilding pipeline failed.");
          }
        }
        else
        {
          for (int index = 0; index < loanDirs.Length; ++index)
          {
            if (feedback != null && index % num1 == 0)
            {
              if (!feedback.SetFeedback((string) null, "Reading loan information (" + index.ToString("#,##0") + "/" + loanDirs.Length.ToString("#,##0") + ")...", index))
                return;
            }
            string loanName = loanDirs[index];
            try
            {
              if (DateTime.Now - lastRefreshed > TimeSpan.FromMinutes((double) Pipeline.DefaultServiceSessionRefreshInterval))
              {
                context.Sessions.GetServiceSessionInfo(sessionId, true);
                lastRefreshed = DateTime.Now;
              }
              Pipeline.rebuildLoanInternal(folderName, loanName, calcs, currentUser, loanSettings, alertMonitor, dbToRebuild, false, sessionId);
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(Pipeline.className, "Error rebuilding loan " + loanName + ": " + (object) ex);
            }
          }
        }
        LoanIdentity[] contents = fol.GetContents();
        if (feedback != null && contents.Length != 0)
        {
          if (!feedback.ResetCounter(contents.Length))
            return;
          feedback.SetFeedback("Validating \"" + fol.Name + "\"...", "", -1);
        }
        if (num3 > 1)
        {
          try
          {
            LoanIdentity[] source = contents;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = num3;
            Action<LoanIdentity> body = (Action<LoanIdentity>) (loanIdentity =>
            {
              using (context.MakeCurrent(requestCache, correlationId, transactionId, new bool?()))
              {
                if (fol.LoanFilesExist(loanIdentity.LoanName))
                  return;
                using (Loan loan = LoanStore.CheckOut(loanIdentity.Guid))
                {
                  loan.Unlock();
                  loan.Delete(true, currentUser, isExternalOrganization);
                }
              }
            });
            Parallel.ForEach<LoanIdentity>((IEnumerable<LoanIdentity>) source, parallelOptions, body);
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(Pipeline.className, "Parallel rebuilding pipeline failed." + (object) ex);
          }
        }
        else
        {
          for (int index = 0; index < contents.Length; ++index)
          {
            if (!fol.LoanFilesExist(contents[index].LoanName))
            {
              using (Loan loan = LoanStore.CheckOut(contents[index].Guid))
              {
                loan.Unlock();
                loan.Delete(true, currentUser, isExternalOrganization);
              }
            }
          }
        }
        Pipeline.createLoanFolderEntry(folderName);
        logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, Pipeline.className, string.Format("Rebuilding pipeline completed for loan folder '{0}' with '{1:N0}' threads and elapsed time of '{2}'.", (object) folderName, (object) num3, (object) stopwatch.Elapsed));
      }
      finally
      {
        if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(context) > 0)
          LoanNameFolderGenerator.ResetLoanCount(folderName);
      }
    }

    private static void createLoanFolderEntry(string folderName)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @folderName varchar(128)");
        dbQueryBuilder.AppendLine("set @folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
        dbQueryBuilder.AppendLine("if not exists (select folderName from LoanFolder where folderName = @folderName)\r\nbegin\r\n    insert into LoanFolder (folderName, folderType)\r\n    select @folderName, 0\r\nend");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.WriteApplicationEvent("Error creating entry in LoanFolder table for the folder '" + folderName + "'. This does not stop rebuilding pipeline folders.", EventLogEntryType.Warning, 2002, EventLogEntryCategory.None);
      }
    }

    public static void RebuildLoan(
      string folderName,
      string loanName,
      UserInfo currentUser,
      DatabaseToRebuild dbToRebuild,
      string sessionId)
    {
      ClientContext current = ClientContext.GetCurrent();
      CustomLevelLog customLevelLog1 = new CustomLevelLog("EVENT");
      customLevelLog1.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      customLevelLog1.Src = "Loan:Rebuild\\Start";
      customLevelLog1.Message = "Full Rebuild of loan file '" + folderName + "\\" + loanName + "'" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'";
      CustomLevelLog log1 = customLevelLog1;
      log1.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.Start);
      DiagUtility.DefaultLogger.Write<CustomLevelLog>(log1);
      Err.WriteApplicationEvent("Full Rebuild of loan file '" + folderName + "\\" + loanName + "'" + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2002, EventLogEntryCategory.None);
      Stopwatch stopwatch = Stopwatch.StartNew();
      bool flag = false;
      try
      {
        ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings();
        LoanAlertMonitor loanAlertMonitor = LoanConfiguration.GetLoanAlertMonitor();
        using (CustomFieldCalculators calcs = new CustomFieldCalculators(loanSettings.FieldSettings.CustomFields))
          Pipeline.rebuildLoanInternal(folderName, loanName, calcs, currentUser, loanSettings, loanAlertMonitor, dbToRebuild, false, sessionId);
        flag = true;
      }
      finally
      {
        CustomLevelLog customLevelLog2 = new CustomLevelLog("EVENT");
        customLevelLog2.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
        customLevelLog2.Src = "Loan:Rebuild\\Stop";
        customLevelLog2.Message = "Rebuild of loan file '" + folderName + "\\" + loanName + "' " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds";
        CustomLevelLog log2 = customLevelLog2;
        log2.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.End);
        log2.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) stopwatch.ElapsedMilliseconds);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log2);
        Err.WriteApplicationEvent("Rebuild of loan file '" + folderName + "\\" + loanName + "' " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2002, EventLogEntryCategory.None);
      }
    }

    private static void rebuildLoanInternal(
      string folderName,
      string loanName,
      CustomFieldCalculators calcs,
      UserInfo currentUser,
      ILoanSettings loanSettings,
      LoanAlertMonitor alertMonitor,
      DatabaseToRebuild dbToRebuild,
      bool isExternalOrganization,
      string sessionId)
    {
      try
      {
        TraceLog.WriteVerbose(Pipeline.className, "Rebuilding loan '" + folderName + "\\" + loanName + "'...");
        LoanFolder loanFolder = new LoanFolder(folderName);
        LoanData loanData = loanFolder.ReadLoanData(loanName, loanSettings, loanFolder: loanFolder.Name);
        CustomCodeContextDataProvider contextDataProvider = new CustomCodeContextDataProvider(ClientContext.GetCurrent());
        if (loanData == null)
          Err.Raise(TraceLevel.Warning, Pipeline.className, (ServerException) new ObjectNotFoundException("LoanData file missing or invalid", ObjectType.Loan, (object) new LoanIdentity(folderName, loanName)));
        using (Loan loan1 = LoanStore.CheckOut(loanData.GUID))
        {
          if (currentUser != (UserInfo) null)
          {
            DateTime now = DateTime.Now;
            using (CustomCalculationContext context = new CustomCalculationContext(currentUser, loanData, (IServerDataProvider) contextDataProvider))
              calcs.InvokeAll(context);
            TimeSpan timeSpan = DateTime.Now - now;
            TraceLog.WriteVerbose(Pipeline.className, "Custom calculations re-evaluated for loan '" + folderName + "/" + loanName + "' in " + timeSpan.TotalMilliseconds.ToString("0") + "ms");
          }
          if (currentUser != (UserInfo) null)
          {
            DateTime now = DateTime.Now;
            alertMonitor.ActivateAlerts(loanData, currentUser);
            TimeSpan timeSpan = DateTime.Now - now;
            TraceLog.WriteVerbose(Pipeline.className, "Custom alerts re-evaluated for loan '" + folderName + "/" + loanName + "' in " + timeSpan.TotalMilliseconds.ToString("0") + "ms");
          }
          LoanIdentity loanIdentity = Loan.LookupIdentity(folderName, loanName);
          if (loanIdentity != (LoanIdentity) null && string.Compare(loanIdentity.Guid, loanData.GUID, true) != 0)
          {
            using (Loan loan2 = LoanStore.CheckOut(loanIdentity.Guid))
            {
              if (loan2.Exists)
                loan2.Delete(true, currentUser, isExternalOrganization);
            }
          }
          loanData.AttachAlertMonitor((IAlertMonitor) alertMonitor);
          if (dbToRebuild != DatabaseToRebuild.Internal && dbToRebuild != DatabaseToRebuild.Both)
            return;
          if (!loan1.Exists)
          {
            loan1.IsRebuildFlow = true;
            loan1.CreateNew(folderName, loanName, new LoanServerInfo(loanData.GUID), loanData, currentUser, true, sessionId);
          }
          else
          {
            loan1.ForceRebuild = true;
            loan1.LastModified = loan1.LastModified;
            loan1.ChangeIdentity(new LoanIdentity(folderName, loanName, loan1.Identity.Guid, loan1.Identity.XrefId));
            if (LoanLockAccessor.IsLoanLockDbEnabled)
            {
              LoanLockAccessor.updateLock(new LockInfo(loan1.Identity.Guid, currentUser.Userid, (string) null, (string) null, sessionId, "", LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.Exclusive));
              loan1.CheckIn(loanData, false, currentUser, dbToRebuild != 0, false, isExternalOrganization, sessionId);
              LoanLockAccessor.removeLock(loan1.Identity.Guid, sessionId, currentUser.Userid);
            }
            else
              loan1.CheckIn(loanData, false, currentUser, dbToRebuild != 0, false, isExternalOrganization, sessionId);
          }
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(Pipeline.className, "Rebuilding loan '" + folderName + "\\" + loanName + "' failed..." + (object) ex);
      }
    }

    public static void RebuildIndex(string[] fieldNames, IServerProgressFeedback feedback)
    {
      ClientContext current = ClientContext.GetCurrent();
      Err.WriteApplicationEvent("RebuildIndex started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2003, EventLogEntryCategory.None);
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        DbTableInfo table = DbAccessManager.GetTable("LoanSummary");
        foreach (LoanFolder allLoanFolder in LoanFolder.GetAllLoanFolders())
        {
          LoanIdentity[] contents = allLoanFolder.GetContents();
          int num1 = 0;
          int num2 = num1 / 20;
          if (num2 > 100)
            num2 = 100;
          if (num2 == 0)
            num2 = 1;
          feedback.ResetCounter(contents.Length);
          feedback.SetFeedback("Re-indexing folder '" + allLoanFolder.Name + "'...", "", 0);
          foreach (LoanIdentity loanIdentity in contents)
          {
            if (feedback != null && num1 % num2 == 0)
            {
              if (!feedback.SetFeedback((string) null, "Completed " + (object) num1 + " of " + (object) contents.Length + " loans", num1))
              {
                TraceLog.WriteInfo(Pipeline.className, "Rebuild index cancelled by user.");
                return;
              }
            }
            try
            {
              using (Loan loan = LoanStore.CheckOut(loanIdentity.Guid))
              {
                PipelineInfo pipelineInfo = loan.LoanData.ToPipelineInfo();
                DbValue key1 = new DbValue("Guid", (object) loanIdentity.Guid);
                DbValueList values = new DbValueList();
                if (fieldNames != null)
                {
                  foreach (string fieldName in fieldNames)
                    values.Add(fieldName, pipelineInfo.Info[(object) fieldName]);
                }
                else
                {
                  foreach (string key2 in (IEnumerable) pipelineInfo.Info.Keys)
                    values.Add(key2, pipelineInfo.Info[(object) key2]);
                }
                DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
                dbQueryBuilder.Update(table, values, key1);
                dbQueryBuilder.ExecuteNonQuery();
                TraceLog.WriteInfo(Pipeline.className, "Successfully reindexed loan " + (object) loanIdentity);
              }
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(Pipeline.className, "Error rebuilding index data for loan " + (object) loanIdentity + ": " + (object) ex);
            }
            ++num1;
          }
        }
      }
      finally
      {
        Err.WriteApplicationEvent("RebuildIndex completed for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2003, EventLogEntryCategory.None);
      }
    }

    public static void UpdateExtendedLoanSummary(IServerProgressFeedback feedback)
    {
      ClientContext current = ClientContext.GetCurrent();
      Err.WriteApplicationEvent("UpdateExtendedLoanSummary started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2003, EventLogEntryCategory.None);
      Stopwatch stopwatch = Stopwatch.StartNew();
      try
      {
        DbTableInfo table = DbAccessManager.GetTable("LoanSummaryExtension");
        DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
        DbValueList dbValueList = new DbValueList();
        dbQueryBuilder1.SelectFrom(table, new string[2]
        {
          "Guid",
          "LoanNumber"
        });
        DataRowCollection dataRowCollection = dbQueryBuilder1.Execute();
        int num1 = 0;
        int num2 = dataRowCollection.Count / 20;
        if (num2 > 100)
          num2 = 100;
        if (num2 == 0)
          num2 = 1;
        feedback.ResetCounter(dataRowCollection.Count);
        feedback.SetFeedback("Updating Extended Loan Summary", "", 0);
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          string guid = (string) dataRow["Guid"];
          string str = (string) dataRow["LoanNumber"];
          if (feedback != null && num1 % num2 == 0)
          {
            if (!feedback.SetFeedback((string) null, "Completed " + (object) num1 + " of " + (object) dataRowCollection.Count + " loans", num1))
            {
              TraceLog.WriteInfo(Pipeline.className, "Updating Extended Loan Summary table  cancelled by user.");
              break;
            }
          }
          try
          {
            using (Loan loan = LoanStore.CheckOut(guid))
            {
              DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
              DbValueList values = new DbValueList();
              DbValue key = new DbValue("Guid", (object) guid);
              values.Add("SubmittedForReviewDate", (object) loan.LoanData.GetField("4119"));
              values.Add("PurchaseSuspenseDate", (object) loan.LoanData.GetField("3918"));
              values.Add("PurchaseApprovalDate", (object) loan.LoanData.GetField("3920"));
              values.Add("ClearedForPurchaseDate", (object) loan.LoanData.GetField("3921 "));
              values.Add("CancelledDate", (object) loan.LoanData.GetField("4207"));
              values.Add("VoidedDate", (object) loan.LoanData.GetField("4208"));
              values.Add("WithdrawalRequestedDate", (object) loan.LoanData.GetField("4242"));
              dbQueryBuilder2.Update(table, values, key);
              dbQueryBuilder2.ExecuteNonQuery();
              TraceLog.WriteInfo(Pipeline.className, "Successfully updated loan " + str);
            }
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(Pipeline.className, "Error updating Extended Loan Summary table" + str + ": " + (object) ex);
          }
          ++num1;
        }
      }
      finally
      {
        Err.WriteApplicationEvent("Updating Extended Loan Summary table '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2003, EventLogEntryCategory.None);
      }
    }

    private static string getSetRDBLastUpdated(bool useERDB)
    {
      string str = useERDB ? "ERDBLastUpdated" : "RDBLastUpdated";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @RDBLastUpdated varchar(64)");
      dbQueryBuilder.AppendLine("set @RDBLastUpdated = (select [value] from [company_settings] where [category] = 'ReportingDB' and [attribute] = '" + str + "')");
      dbQueryBuilder.AppendLine("if @RDBLastUpdated is null");
      dbQueryBuilder.AppendLine("    insert into [company_settings] ([category], [attribute], [value]) values ('ReportingDB', '" + str + "', CONVERT(varchar(64), getdate(), 121))");
      dbQueryBuilder.AppendLine("select @RDBLastUpdated");
      object obj = dbQueryBuilder.ExecuteScalar();
      return obj == DBNull.Value ? (string) null : (string) obj;
    }

    private static void deleteRDBLastUpdated(bool useERDB)
    {
      string str = useERDB ? "ERDBLastUpdated" : "RDBLastUpdated";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [company_settings] where [category] = 'ReportingDB' and [attribute] = '" + str + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      UserInfo currentUser,
      IServerProgressFeedback2 feedback)
    {
      Pipeline.RebuildReportingDb(useERDB, pendingFieldsOnly, currentUser, feedback, true);
    }

    public static void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      UserInfo currentUser,
      IServerProgressFeedback2 feedback,
      bool updateAllLoans)
    {
      ClientContext current = ClientContext.GetCurrent();
      CustomLevelLog customLevelLog1 = new CustomLevelLog("EVENT");
      customLevelLog1.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      customLevelLog1.Src = "RDB:Rebuild\\Start";
      customLevelLog1.Message = "Reporting DB Rebuild of " + (pendingFieldsOnly ? "pending fields only" : "all fields") + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'";
      CustomLevelLog log1 = customLevelLog1;
      log1.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.Start);
      DiagUtility.DefaultLogger.Write<CustomLevelLog>(log1);
      Err.WriteApplicationEvent("Reporting DB Rebuild of " + (pendingFieldsOnly ? "pending fields only" : "all fields") + (currentUser == (UserInfo) null ? "" : " by user '" + currentUser.Userid + "'") + " started for client instance '" + current.InstanceName + "'", EventLogEntryType.Information, 2004, EventLogEntryCategory.None);
      Stopwatch stopwatch = Stopwatch.StartNew();
      bool flag = false;
      try
      {
        LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList(useERDB);
        LoanXDBField[] fields = pendingFieldsOnly ? loanXdbTableList.GetFieldsRequiringRebuild() : loanXdbTableList.GetAllFields();
        if (fields.Length == 0)
          return;
        CustomFieldsInfo loanCustomFields = SystemConfiguration.GetLoanCustomFields();
        CustomCodeContextDataProvider serviceContext = new CustomCodeContextDataProvider(ClientContext.GetCurrent());
        CustomFieldCalculators calcs = (CustomFieldCalculators) null;
        foreach (LoanXDBField loanXdbField in fields)
        {
          if (CustomFieldInfo.IsCustomFieldID(loanXdbField.FieldID))
          {
            CustomFieldInfo field = loanCustomFields.GetField(loanXdbField.FieldID);
            if (field != null && (field.Calculation ?? "") != "")
            {
              calcs = new CustomFieldCalculators(loanCustomFields);
              break;
            }
          }
        }
        try
        {
          string rdbLastUpdated = Pipeline.getSetRDBLastUpdated(useERDB);
          if (updateAllLoans)
            rdbLastUpdated = (string) null;
          foreach (LoanFolder allLoanFolder in LoanFolder.GetAllLoanFolders())
          {
            LoanIdentity[] contents = allLoanFolder.GetContents(useERDB, rdbLastUpdated);
            if (feedback.NumberOfThreads == 1)
            {
              if (!Pipeline.rebuildReportingDb(0, current, allLoanFolder.Name, useERDB, fields, calcs, serviceContext, currentUser, feedback, contents, 0, contents.Length - 1))
                return;
            }
            else
            {
              int num1 = contents.Length / feedback.NumberOfThreads;
              int num2 = contents.Length % feedback.NumberOfThreads;
              int startIdx = 0;
              int num3 = startIdx + num1 - 1;
              int num4 = num2;
              int num5 = num4 - 1;
              int num6 = num4 > 0 ? 1 : 0;
              int endIdx = num3 + num6;
              Thread[] threadArray = new Thread[feedback.NumberOfThreads];
              Pipeline.RebuildReportingDbParam[] reportingDbParamArray = new Pipeline.RebuildReportingDbParam[feedback.NumberOfThreads];
              for (int threadIdx = 0; threadIdx < feedback.NumberOfThreads; ++threadIdx)
              {
                if (endIdx < startIdx)
                {
                  threadArray[threadIdx] = (Thread) null;
                  reportingDbParamArray[threadIdx] = (Pipeline.RebuildReportingDbParam) null;
                }
                else
                {
                  threadArray[threadIdx] = new Thread(new ParameterizedThreadStart(Pipeline.rebuildReportingDbThreadStart));
                  reportingDbParamArray[threadIdx] = new Pipeline.RebuildReportingDbParam(threadIdx, current, allLoanFolder.Name, useERDB, fields, calcs, serviceContext, currentUser, feedback, contents, startIdx, endIdx);
                  threadArray[threadIdx].Start((object) reportingDbParamArray[threadIdx]);
                  startIdx = endIdx + 1;
                  endIdx = startIdx + num1 - 1 + (num5-- > 0 ? 1 : 0);
                }
              }
              for (int index = 0; index < feedback.NumberOfThreads; ++index)
              {
                if (threadArray[index] != null)
                  threadArray[index].Join();
              }
              for (int index = 0; index < feedback.NumberOfThreads; ++index)
              {
                if (reportingDbParamArray[index] != null && !reportingDbParamArray[index].ReturnValue)
                  return;
              }
            }
          }
          Pipeline.deleteRDBLastUpdated(useERDB);
          LoanXDBStore.ClearPendingFields(useERDB, currentUser.Userid);
        }
        catch (Exception ex)
        {
          CustomLevelLog log2 = new CustomLevelLog("EVENT");
          log2.Level = Encompass.Diagnostics.Logging.LogLevel.ERROR.Force();
          log2.Src = "RDB:Rebuild\\FAIL";
          log2.Message = "Unhandled exception occured while rebuilding pipeline 1";
          log2.Error = new LogErrorData(ex);
          DiagUtility.DefaultLogger.Write<CustomLevelLog>(log2);
        }
        finally
        {
          calcs?.Dispose();
        }
        flag = true;
      }
      catch (Exception ex)
      {
        CustomLevelLog log3 = new CustomLevelLog("EVENT");
        log3.Level = Encompass.Diagnostics.Logging.LogLevel.ERROR.Force();
        log3.Src = "RDB:Rebuild\\FAIL";
        log3.Message = "Unhandled exception occured while rebuilding pipeline 2";
        log3.Error = new LogErrorData(ex);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log3);
      }
      finally
      {
        CustomLevelLog customLevelLog2 = new CustomLevelLog("EVENT");
        customLevelLog2.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
        customLevelLog2.Src = "RDB:Rebuild\\Stop";
        customLevelLog2.Message = "Reporting DB Rebuild " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds";
        CustomLevelLog log4 = customLevelLog2;
        log4.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, LogEventType.End);
        log4.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (double) stopwatch.ElapsedMilliseconds);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log4);
        Err.WriteApplicationEvent("Reporting DB Rebuild " + (flag ? "completed successfully" : "failed") + " for client instance '" + current.InstanceName + "' in " + stopwatch.Elapsed.TotalSeconds.ToString("0") + " seconds", EventLogEntryType.Information, 2004, EventLogEntryCategory.None);
      }
    }

    private static void rebuildReportingDbThreadStart(object obj)
    {
      try
      {
        Pipeline.RebuildReportingDbParam reportingDbParam = obj as Pipeline.RebuildReportingDbParam;
        reportingDbParam.ReturnValue = Pipeline.rebuildReportingDb(reportingDbParam.ThreadIdx, reportingDbParam.Context, reportingDbParam.FolderName, reportingDbParam.UseERDB, reportingDbParam.Fields, reportingDbParam.Calcs, reportingDbParam.ServiceContext, reportingDbParam.CurrentUser, reportingDbParam.Feedback, reportingDbParam.LoanIds, reportingDbParam.StartIdx, reportingDbParam.EndIdx);
      }
      catch (Exception ex)
      {
        CustomLevelLog log = new CustomLevelLog("EVENT");
        log.Level = Encompass.Diagnostics.Logging.LogLevel.ERROR.Force();
        log.Src = "RDB:Rebuild\\FAIL";
        log.Message = "Unhandled exception occured while rebuilding pipeline on background thread";
        log.Error = new LogErrorData(ex);
        DiagUtility.DefaultLogger.Write<CustomLevelLog>(log);
      }
    }

    private static bool rebuildReportingDb(
      int threadIdx,
      ClientContext context,
      string folderName,
      bool useERDB,
      LoanXDBField[] fields,
      CustomFieldCalculators calcs,
      CustomCodeContextDataProvider serviceContext,
      UserInfo currentUser,
      IServerProgressFeedback2 feedback,
      LoanIdentity[] loanIds,
      int startIdx,
      int endIdx)
    {
      if (context != null)
      {
        using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          int num1 = 0;
          int maxValue = endIdx - startIdx + 1;
          int num2 = maxValue / 20;
          if (num2 > 100)
            num2 = 100;
          if (num2 == 0)
            num2 = 1;
          if (feedback != null)
          {
            feedback.ResetCounter(threadIdx, maxValue);
            feedback.SetFeedback(threadIdx, "Update Reporting DB Records  '" + folderName + "'...", "", 0);
          }
          for (int index = startIdx; index <= endIdx; ++index)
          {
            LoanIdentity loanId = loanIds[index];
            if (feedback != null && num1 % num2 == 0)
            {
              if (!feedback.SetFeedback(threadIdx, (string) null, "Completed " + (object) num1 + " of " + (object) maxValue + " loans", num1))
              {
                TraceLog.WriteInfo(Pipeline.className, "Rebuild cancelled by user.");
                return false;
              }
            }
            try
            {
              using (Loan loan = LoanStore.CheckOut(loanId.Guid))
              {
                loan.LoanData.AttachSnapshotProvider((ILoanSnapshotProvider) new LoanSnapshotProvider(loan));
                loan.RefreshReportingData(useERDB, fields, calcs, serviceContext, currentUser);
              }
              TraceLog.WriteInfo(Pipeline.className, "Successfully updated reporting data for loan " + (object) loanId);
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(Pipeline.className, "Error rebuilding index data for loan " + (object) loanId + ": " + (object) ex);
            }
            ++num1;
          }
        }
      }
      return true;
    }

    public static void LoanReassign(
      int index,
      PipelineInfo pipeLine,
      string userID,
      int roleID,
      ISession session,
      IServerProgressFeedback feedback)
    {
      UserInfo userInfo = session.GetUserInfo();
      string sessionId = session.SessionID;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      LoanInfo.Right loanAccessRights = Pipeline.GetEffectiveLoanAccessRights(pipeLine.GUID, userInfo);
      feedback.ResetCounter(6);
      feedback.Details = index.ToString() + "_In Progress";
      feedback.Status = "Processing";
      if (!pipeLine.LockInfo.IsLocked)
      {
        try
        {
          using (Loan loan1 = LoanStore.CheckOut(pipeLine.GUID))
          {
            if (!loan1.Locked)
            {
              try
              {
                using (Loan loan2 = LoanStore.CheckOut(pipeLine.GUID))
                {
                  try
                  {
                    if (loan2.Exists)
                    {
                      if ((loanAccessRights & LoanInfo.Right.Access) != LoanInfo.Right.NoRight)
                      {
                        bool flag1 = false;
                        bool flag2 = false;
                        bool flag3 = false;
                        bool flag4 = false;
                        if (LoanLockAccessor.IsLoanLockDbEnabled)
                        {
                          LoanLockAccessor.updateLock(new LockInfo(loan2.Identity.Guid, userInfo.Userid, (string) null, (string) null, sessionId, "", LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.Exclusive));
                        }
                        else
                        {
                          loan2.Lock(new LockInfo(pipeLine.GUID, userInfo.Userid, userInfo.FirstName, userInfo.LastName, sessionId, "", LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.Exclusive));
                          loan2.CheckIn(true, userInfo, false, sessionId);
                        }
                        feedback.Increment(1);
                        LoanData loanData = loan2.LoanData;
                        loanData.Parse();
                        feedback.Increment(1);
                        LogList logList = loanData.GetLogList();
                        UserInfo userById = User.GetUserById(userID);
                        string str = string.Empty;
                        OrgInfo avaliableOrganization = OrganizationStore.GetFirstAvaliableOrganization(userById.OrgId);
                        if (avaliableOrganization != null)
                          str = avaliableOrganization.CompanyFax;
                        bool flag5 = false;
                        MilestoneLog[] allMilestones = logList.GetAllMilestones();
                        RolesMappingInfo[] usersRoleMapping = WorkflowBpmDbAccessor.GetUsersRoleMapping(userID);
                        if (usersRoleMapping != null)
                        {
                          List<int> intList = new List<int>();
                          foreach (RolesMappingInfo rolesMappingInfo in usersRoleMapping)
                          {
                            intList.Clear();
                            intList.AddRange((IEnumerable<int>) rolesMappingInfo.RoleIDList);
                            if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanOfficer && intList.Contains(roleID))
                              flag1 = true;
                            if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanProcessor && intList.Contains(roleID))
                              flag2 = true;
                            if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanCloser && intList.Contains(roleID))
                              flag3 = true;
                            if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.Underwriter && intList.Contains(roleID))
                              flag4 = true;
                          }
                        }
                        for (int index1 = 0; index1 < allMilestones.Length; ++index1)
                        {
                          if (allMilestones[index1].RoleID == roleID && allMilestones[index1].LoanAssociateID != "" && index1 > 0)
                          {
                            allMilestones[index1].SetLoanAssociate(userById);
                            if ((userById.Fax ?? "") == string.Empty)
                              allMilestones[index1].LoanAssociateFax = str;
                            flag5 = true;
                          }
                        }
                        if (flag5 & flag1)
                        {
                          flag5 = true;
                          loanData.SetField("LOID", userID);
                          loanData.SetField("317", userById.FullName);
                          loanData.SetField("4508", userById.JobTitle);
                          loanData.SetField("1407", userById.Email);
                          loanData.SetField("1406", userById.Phone);
                          loanData.SetField("2411", userById.Fax);
                          loanData.SetField("2854", userById.CellPhone);
                          loanData.SetField("1612", userById.FullName);
                          if (loanData.GetField("1825") == "2020")
                          {
                            loanData.SetField("URLA.X170", userById != (UserInfo) null ? userById.FirstName : "");
                            loanData.SetField("URLA.X171", userById != (UserInfo) null ? userById.MiddleName : "");
                            loanData.SetField("URLA.X172", userById != (UserInfo) null ? userById.LastName : "");
                            loanData.SetField("URLA.X173", userById != (UserInfo) null ? userById.SuffixName : "");
                          }
                          loanData.SetField("3239", userById.Userid);
                          loanData.SetField("3238", userById.NMLSOriginatorID);
                          loanData.SetField("1823", userById.Phone);
                          loanData.SetField("3968", userById.Email);
                          loanData.SetField("2306", userById.lo_license);
                          if (loanData.GetField("2626") == "Banked - Retail")
                          {
                            loanData.SetField("LCP.X21", userById.FullName);
                            loanData.SetField("LE3.X4", userById.FullName);
                          }
                          else if (loanData.GetField("2626") == "Brokered")
                          {
                            loanData.SetField("LCP.X21", userById.FullName);
                            loanData.SetField("LE3.X4", "");
                          }
                          else if (loanData.GetField("2626") == "Banked - Wholesale")
                            loanData.SetField("LE3.X4", "");
                          loanData.SetField("1256", userById.FullName);
                        }
                        if (flag5 & flag2)
                        {
                          flag5 = true;
                          loanData.SetField("LPID", userID);
                          loanData.SetField("362", userById.FullName);
                          loanData.SetField("4509", userById.JobTitle);
                          loanData.SetField("1409", userById.Email);
                          loanData.SetField("1408", userById.Phone);
                          loanData.SetField("2412", userById.Fax);
                          loanData.SetField("2855", userById.CellPhone);
                        }
                        if (flag5 & flag3)
                        {
                          flag5 = true;
                          loanData.SetField("CLID", userID);
                          loanData.SetField("1855", userById.FullName);
                          loanData.SetField("1857", userById.Email);
                          loanData.SetField("1856", userById.Phone);
                          loanData.SetField("2413", userById.Fax);
                          loanData.SetField("2856", userById.CellPhone);
                        }
                        if (flag5 & flag4)
                        {
                          flag5 = true;
                          loanData.SetField("UWID", userID);
                          loanData.SetField("2574", userById.FullName);
                          loanData.SetField("2576", userById.Email);
                          loanData.SetField("2575", userById.Phone);
                          loanData.SetField("2577", userById.Fax);
                        }
                        MilestoneFreeRoleLog[] milestoneFreeRoles = logList.GetAllMilestoneFreeRoles();
                        if (milestoneFreeRoles != null)
                        {
                          foreach (MilestoneFreeRoleLog milestoneFreeRoleLog in milestoneFreeRoles)
                          {
                            if (milestoneFreeRoleLog.RoleID == roleID)
                            {
                              if (milestoneFreeRoleLog.LoanAssociateID != "")
                              {
                                milestoneFreeRoleLog.SetLoanAssociate(userById);
                                if ((userById.Fax ?? "") == string.Empty)
                                  milestoneFreeRoleLog.LoanAssociateFax = str;
                                flag5 = true;
                                break;
                              }
                              break;
                            }
                          }
                        }
                        feedback.Increment(1);
                        if (LoanLockAccessor.IsLoanLockDbEnabled)
                        {
                          loan2.CheckIn(userInfo, false, sessionId);
                          LoanLockAccessor.removeLock(pipeLine.GUID, sessionId, userInfo.Userid);
                        }
                        else
                        {
                          loan2.Unlock();
                          loan2.CheckIn(userInfo, false, sessionId);
                        }
                        loan2.Dispose();
                        feedback.Increment(1);
                        if (flag5)
                        {
                          feedback.Details = index.ToString() + "_Success";
                          feedback.Status = "Success";
                        }
                        else
                        {
                          feedback.Details = index.ToString() + "_No user with this specific role has been assigned to this loan record.  No update has been made.";
                          feedback.Status = "Warning";
                        }
                      }
                      else
                      {
                        feedback.Details = index.ToString() + "_Loan does not have Reassignment Right";
                        feedback.Status = "Fail";
                      }
                    }
                    else
                    {
                      feedback.Details = index.ToString() + "_Loan does not exist";
                      feedback.Status = "Fail";
                    }
                  }
                  catch (Exception ex)
                  {
                    TraceLog.WriteError(Pipeline.className, "Error reassigning loan " + loan2.Identity.Guid + ": " + (object) ex);
                    if (LoanLockAccessor.IsLoanLockDbEnabled)
                      LoanLockAccessor.removeLock(pipeLine.GUID, sessionId, userInfo.Userid);
                    else
                      loan2.Unlock();
                    loan2.Dispose();
                    feedback.Details = index.ToString() + "_Exception encountered while updating loan record";
                    feedback.Status = "Fail";
                  }
                }
              }
              catch (Exception ex)
              {
                TraceLog.WriteError(Pipeline.className, "Error reassigning loan " + loan1.Identity.Guid + ": " + (object) ex);
                feedback.Details = index.ToString() + "_Exception encountered while updating loan record";
                feedback.Status = "Fail";
              }
            }
            else
            {
              feedback.Details = index.ToString() + "_Loan is currently locked by session (" + loan1.LockedBySessions[0] + "). No reassignment has been performed.";
              feedback.Status = "Warning";
            }
          }
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(Pipeline.className, "Error updating loan file " + pipeLine.GUID + ": " + (object) ex);
          feedback.Details = index.ToString() + "_Exception encountered while updating loan file";
          feedback.Status = "Fail";
        }
      }
      else
      {
        feedback.Details = index.ToString() + "_Loan is currently locked by user (" + pipeLine.LockInfo.LockedBy + "). No reassignment has been performed.";
        feedback.Status = "Warning";
      }
    }

    public static void ClearReportingDatabase(IServerProgressFeedback feedback)
    {
      string[] tableNameList = LoanXDBStore.GetTableNameList();
      if (tableNameList == null)
        return;
      if (feedback != null && tableNameList.Length != 0)
      {
        if (!feedback.ResetCounter(tableNameList.Length))
          return;
        feedback.Status = "Clearing all records in reporting databases";
      }
      for (int index = 0; index < tableNameList.Length; ++index)
      {
        if (feedback != null)
          feedback.Details = "Clearing \"" + tableNameList[index] + "\" records...";
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("DELETE FROM " + tableNameList[index]);
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteWarning(Pipeline.className, "Can't erase records in reporting database " + tableNameList[index] + ". Error: " + ex.Message);
        }
        if (feedback != null && !feedback.Increment(1))
          break;
      }
    }

    public static bool IsExclusiveFilter(QueryCriterion cri) => cri != null && cri.IsExclusive();

    private class RebuildReportingDbParam
    {
      public readonly int ThreadIdx;
      public readonly ClientContext Context;
      public readonly string FolderName;
      public readonly bool UseERDB;
      public readonly LoanXDBField[] Fields;
      public readonly CustomFieldCalculators Calcs;
      public readonly CustomCodeContextDataProvider ServiceContext;
      public readonly UserInfo CurrentUser;
      public readonly IServerProgressFeedback2 Feedback;
      public readonly LoanIdentity[] LoanIds;
      public readonly int StartIdx;
      public readonly int EndIdx;
      public bool ReturnValue;

      public RebuildReportingDbParam(
        int threadIdx,
        ClientContext context,
        string folderName,
        bool useERDB,
        LoanXDBField[] fields,
        CustomFieldCalculators calcs,
        CustomCodeContextDataProvider serviceContext,
        UserInfo currentUser,
        IServerProgressFeedback2 feedback,
        LoanIdentity[] loanIds,
        int startIdx,
        int endIdx)
      {
        this.ThreadIdx = threadIdx;
        this.Context = context;
        this.FolderName = folderName;
        this.UseERDB = useERDB;
        this.Fields = fields;
        this.Calcs = calcs;
        this.ServiceContext = serviceContext;
        this.CurrentUser = currentUser;
        this.Feedback = feedback;
        this.LoanIds = loanIds;
        this.StartIdx = startIdx;
        this.EndIdx = endIdx;
      }
    }
  }
}
