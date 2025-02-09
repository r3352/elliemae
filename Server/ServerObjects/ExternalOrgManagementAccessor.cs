// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.ExternalOrgManagementAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using Elli.Common.Extensions;
using Elli.Common.Security;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Common;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.Calculator;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class ExternalOrgManagementAccessor
  {
    private const string className = "ExternalOrgManagementAccessor�";
    private const int tpoCustomFieldType = 1;
    private const string EXT_ORG_ID = "@oid�";
    private const string TABLE_NAME_EXTERNALORGDBANAMES = "ExternalOrgDBANames�";
    private const string COLUMN_INHERITCOMPANYDETAILS = "InheritCompanyDetails�";
    private const string COLUMN_INHERITPARENTRATESHEET = "InheritParentRateSheet�";
    private const string COLUMN_INHERITPARENTPRODUCTPRICING = "InheritParentProductPricing�";
    private const string COLUMN_INHERITPARENTAPPROVALSTATUS = "InheritParentApprovalStatus�";
    private const string COLUMN_INHERITPARENTBIZINFORMATION = "InheritParentBizInformation�";
    private const string TPO_CUSTOMFIELDSETTINGS_KEY = "TPOCustomfieldSettings�";
    private const string EXTERNAL_USER_CACHE_KEY = "UserStore_�";
    private static HashSet<string> TABLE_FLAGS = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "InheritCompanyDetails",
      "InheritParentRateSheet",
      "InheritParentProductPricing",
      "InheritParentApprovalStatus",
      "InheritParentBizInformation"
    };
    private static readonly TimeSpan mutexTimeout = TimeSpan.FromSeconds(30.0);
    private static List<string> tpoTables = new List<string>((IEnumerable<string>) new string[16]
    {
      "ExternalOriginatorManagement",
      "ExternalOrgDetail",
      "ExternalOrgAttachment",
      "ExternalOrgContact",
      "ExternalOrgDescendents",
      "ExternalOrgLoanTypes",
      "ExternalOrgLoanTypesChannels",
      "ExternalOrgNotes",
      "ExternalOrgSalesReps",
      "ExternalOrgSelectedURL",
      "ExternalOrgStateLicensing",
      "ExternalOrgURLs",
      "ExternalUserRole",
      "ExternalUsers",
      "ExternalUserStateLicensing",
      "ExternalUserURLs"
    });

    public static List<ExternalOriginatorManagementData>[] GetAllExternalOrganizations()
    {
      return new List<ExternalOriginatorManagementData>[2]
      {
        ExternalOrgManagementAccessor.GetAllExternalOrganizations(true),
        ExternalOrgManagementAccessor.GetAllExternalOrganizations(false)
      };
    }

    public static List<ExternalOriginatorManagementData> GetAllExternalOrganizations(bool forLender)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("SELECT oid FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "]");
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizations(forLender, stringBuilder.ToString());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalOrganizations: Cannot fetch all records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetAllExternalParentOrganizations(
      bool forLender)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> parentOrganizations = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] a left join [ExternalOrgDetail] b  on a.oid = b.externalOrgID where parent = 0 ORDER BY OrganizationName ");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalOriginatorManagementData result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          if (!forLender)
            result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, row);
          parentOrganizations.Add(result);
        }
        return parentOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalParentOrganizations: Cannot fetch all records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetExternalOrganization(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData result = (ExternalOriginatorManagementData) null;
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + "] where oid = " + (object) oid + " ORDER BY [HierarchyPath], [Depth] ");
      try
      {
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        if (dataRowCollection1.Count > 0)
          result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection1[0]);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgDetail] where [externalOrgID] = " + (object) oid);
        DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
        if (dataRowCollection2.Count > 0)
          result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, dataRowCollection2[0]);
        return result;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganization: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationsWithoutExtension(
      string currentUserID,
      string externalOrgID)
    {
      List<int> oids = new List<int>();
      if (ExternalOrgManagementAccessor.isCurrentUserTPOSalesRep(currentUserID))
      {
        ArrayList andOrgBySalesRep = ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRep(currentUserID);
        return andOrgBySalesRep != null && andOrgBySalesRep.Count > 3 ? ExternalOrgManagementAccessor.GetExternalOrganizations(false, (List<int>) andOrgBySalesRep[3]) : (List<ExternalOriginatorManagementData>) null;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT b.oid FROM [ExternalOriginatorManagement] b INNER JOIN [ExternalOrgDetail] ON b.oid = [externalOrgID] WHERE [OrganizationType] = 0");
      if (!string.IsNullOrEmpty(externalOrgID))
        dbQueryBuilder.Append(" AND b.[ExternalID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrgID));
      dbQueryBuilder.Append(" ORDER BY [HierarchyPath], [Depth] ");
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          oids.Add(Utils.ParseInt((object) string.Concat(dataRow["oid"])));
        return ExternalOrgManagementAccessor.GetExternalOrganizations(false, oids);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalOrganizations: Cannot fetch all records from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static Hashtable GetExternalOrganizationsAvailableCommitment(int[] oids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Hashtable availableCommitment = new Hashtable();
      dbQueryBuilder.AppendLine("SELECT * FROM FN_GetTpoCommitments() where oid in (" + string.Join<int>(",", (IEnumerable<int>) oids) + ")");
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          availableCommitment.Add((object) int.Parse(EllieMae.EMLite.DataAccess.SQL.Encode(dataRow["oid"])), (object) double.Parse(EllieMae.EMLite.DataAccess.SQL.Encode(dataRow["Mand_Available"])));
        return availableCommitment;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationsCommitmentDetails: Cannot fetch record from FN_GetTpoCommitments function.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetOldExternalOrganizations()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> externalOrganizations = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalOrganizations.Add(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row));
        return externalOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOldExternalOrganizations: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetOldTPOExternalOrganizations()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> externalOrganizations = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement where contactType = 1");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalOrganizations.Add(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row));
        return externalOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOldTPOExternalOrganizations: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetOldExternalOrganizationByDBAName(
      string dbaName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement where CompanyDBAName = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(dbaName) + " and ExternalID = ''");
      try
      {
        IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            return ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow((DataRow) enumerator.Current);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        return new ExternalOriginatorManagementData();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOldExternalOrganizationByTPOID: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetOldExternalOrganizationByTPOID(string tpoid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement where externalid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(tpoid) + " ORDER BY [HierarchyPath], [Depth] ");
      try
      {
        IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            return ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow((DataRow) enumerator.Current);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        return new ExternalOriginatorManagementData();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOldExternalOrganizationByTPOID: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    [PgReady]
    public static List<ExternalOriginatorManagementData> GetExternalOrganizationByTPOID(
      string tpoId,
      bool includeDisabled = false)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        List<ExternalOriginatorManagementData> organizationByTpoid = new List<ExternalOriginatorManagementData>();
        pgDbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement INNER JOIN [ExternalOrgDetail] on externalOrgID = oid\r\n                             WHERE externalid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(tpoId));
        if (!includeDisabled)
          pgDbQueryBuilder.AppendLine(" AND DisableLogin=0 ");
        pgDbQueryBuilder.AppendLine(" ORDER BY [HierarchyPath], [Depth] ");
        try
        {
          foreach (DataRow row in (InternalDataCollectionBase) pgDbQueryBuilder.Execute())
          {
            ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
            organizationByTpoid.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
          }
          return organizationByTpoid;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationByTPOID: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        List<ExternalOriginatorManagementData> organizationByTpoid = new List<ExternalOriginatorManagementData>();
        dbQueryBuilder.AppendLine("SELECT * FROM ExternalOriginatorManagement INNER JOIN [ExternalOrgDetail] on externalOrgID = oid\r\n                             WHERE externalid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(tpoId));
        if (!includeDisabled)
          dbQueryBuilder.AppendLine(" AND DisableLogin=0 ");
        dbQueryBuilder.AppendLine(" ORDER BY [HierarchyPath], [Depth] ");
        try
        {
          foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
            organizationByTpoid.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
          }
          return organizationByTpoid;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationByTPOID: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
        }
      }
    }

    public static int GetExternalRootOrgIdFromOrgChart()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM org_chart WHERE org_name = 'Third Party Originators' AND org_type = 'External'");
      try
      {
        object obj = dbQueryBuilder.ExecuteScalar();
        return obj != null ? Convert.ToInt32(obj) : 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalRootOrgFromOrgChart: Cannot fetch id records from org_chart table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetRootOrganisation(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData result = (ExternalOriginatorManagementData) null;
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOriginatorManagement] a inner join [ExternalOrgDescendents] b  on a.oid = b.oid where a.parent = 0 and b.descendent = " + (object) oid);
      int num = -1;
      try
      {
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        if (dataRowCollection1.Count > 0)
        {
          result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection1[0]);
          num = Convert.ToInt32(dataRowCollection1[0][nameof (oid)]);
        }
        else
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("SELECT * FROM  [ExternalOriginatorManagement] where oid = " + (object) oid + " ORDER BY [HierarchyPath], [Depth] ");
          DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
          if (dataRowCollection2.Count > 0)
            result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection2[0]);
        }
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgDetail] where [externalOrgID] = " + (object) num);
        DataRowCollection dataRowCollection3 = dbQueryBuilder.Execute();
        if (dataRowCollection3.Count > 0)
          result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, dataRowCollection3[0]);
        return result;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganization: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetRootBranch(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData result = (ExternalOriginatorManagementData) null;
      dbQueryBuilder.AppendLine("SELECT a.*, b.* FROM [ExternalOriginatorManagement] a INNER JOIN [ExternalOrgDescendents] b ON a.oid = b.oid INNER JOIN [ExternalOrgDetail] c ON a.oid = c.externalOrgID WHERE c.OrganizationType = 2 AND b.descendent = " + (object) oid);
      int num = -1;
      try
      {
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        if (dataRowCollection1.Count > 0)
        {
          result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection1[0]);
          num = Convert.ToInt32(dataRowCollection1[0][nameof (oid)]);
        }
        else
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("SELECT * FROM  [ExternalOriginatorManagement] where oid = " + (object) oid + " ORDER BY [HierarchyPath], [Depth] ");
          DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
          if (dataRowCollection2.Count > 0)
            result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection2[0]);
        }
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgDetail] where [externalOrgID] = " + (object) num);
        DataRowCollection dataRowCollection3 = dbQueryBuilder.Execute();
        if (dataRowCollection3.Count > 0)
          result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, dataRowCollection3[0]);
        return result;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganization: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetCompanyOrganizations(int oid)
    {
      ExternalOriginatorManagementData rootOrganisation = ExternalOrgManagementAccessor.GetRootOrganisation(false, oid);
      if (rootOrganisation == null)
        return new List<ExternalOriginatorManagementData>();
      List<int> organizationDesendents = ExternalOrgManagementAccessor.GetExternalOrganizationDesendents(rootOrganisation.oid);
      organizationDesendents.Add(rootOrganisation.oid);
      return ExternalOrgManagementAccessor.GetExternalOrganizations(false, organizationDesendents.ToList<int>());
    }

    public static Dictionary<string, List<ExternalOriginatorManagementData>> GetCompanyAncestors(
      int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, List<ExternalOriginatorManagementData>> companyAncestors = new Dictionary<string, List<ExternalOriginatorManagementData>>();
      List<int> source = new List<int>();
      source.Add(oid);
      dbQueryBuilder.AppendLine("SELECT [Ancestor] FROM [ExternalOrgAncestors] where orgID = " + (object) oid);
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          source.Add(Utils.ParseInt((object) string.Concat(dataRow["Ancestor"])));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetCompanyAncestors: Cannot fetch record from ExternalOrgAncestors table.\r\n" + ex.Message);
      }
      companyAncestors.Add("CompanyOrgs", ExternalOrgManagementAccessor.GetCompanyOrganizations(oid));
      companyAncestors.Add("CompanyAncestors", ExternalOrgManagementAccessor.GetExternalOrganizations(false, source.ToList<int>()));
      return companyAncestors;
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      string oidFilter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> externalOrganizations = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] a left join [ExternalOrgDetail] b  on a.oid = b.externalOrgID where oid in (" + oidFilter + ") ORDER BY a.[HierarchyPath], a.[Depth] ");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalOriginatorManagementData result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          if (!forLender)
            result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, row);
          externalOrganizations.Add(result);
        }
        return externalOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizations: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static HashSet<int> GetValidExternalOrganizationIds(HashSet<int> oidList)
    {
      string str = string.Join<int>(",", (IEnumerable<int>) oidList);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [ExternalOriginatorManagement] where oid in (" + str + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      HashSet<int> externalOrganizationIds = new HashSet<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        externalOrganizationIds.Add((int) dataRowCollection[index]["oid"]);
      return externalOrganizationIds;
    }

    public static List<ExternalOrgManagementDataCount> GetAllExternalOrganizationsByParent(
      int? parentOrgId,
      bool isRecursive = false,
      int? start = 0,
      int? limit = 0,
      ViewType? view = ViewType.Summary,
      ExternalOriginatorOrgType[] orgTypes = null,
      string tpoId = null)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOrgManagementDataCount> organizationsByParent = new List<ExternalOrgManagementDataCount>();
      dbQueryBuilder.AppendLine("WITH cte AS (");
      if (!parentOrgId.HasValue)
      {
        if (isRecursive)
        {
          if (tpoId == null)
            dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom");
          else
            dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.ExternalID = '" + tpoId + "'");
        }
        else if (tpoId == null)
          dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.parent = 0");
        else
          dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.parent = 0 AND eom.ExternalID = '" + tpoId + "'");
      }
      else if (!isRecursive)
      {
        if (tpoId == null)
          dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN [ExternalOriginatorManagement] ep ON eom.parent = ep.oid AND ep.oid = " + (object) parentOrgId);
        else
          dbQueryBuilder.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN [ExternalOriginatorManagement] ep ON eom.parent = ep.oid AND ep.oid = " + (object) parentOrgId + " AND eom.ExternalID = '" + tpoId + "'");
      }
      else if (tpoId == null)
        dbQueryBuilder.Append("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE Parent = " + (object) parentOrgId + " UNION ALL SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN cte c ON eom.Parent = c.oid ");
      else
        dbQueryBuilder.Append("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE Parent = " + (object) parentOrgId + " AND eom.ExternalID = '" + tpoId + "' UNION ALL SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN cte c ON eom.Parent = c.oid ");
      dbQueryBuilder.AppendLine("), cco AS (SELECT eu.externalOrgId, count(*) AS numContacts FROM ExternalUsers eu INNER JOIN cte ON cte.oid = eu.externalOrgID GROUP BY eu.externalOrgID");
      dbQueryBuilder.AppendLine("), cch AS ( SELECT cte.oid, count(1) AS numChildren FROM cte INNER JOIN ExternalOriginatorManagement eom2 ON cte.oid = eom2.Parent GROUP BY cte.oid)");
      if (limit.HasValue)
      {
        int? nullable = limit;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          dbQueryBuilder.AppendLine("SELECT TOP " + (object) limit + " * FROM (");
      }
      if (start.HasValue)
      {
        int? nullable = start;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          dbQueryBuilder.AppendLine("SELECT * FROM (");
      }
      dbQueryBuilder.AppendLine("SELECT cte.*, org.*, CASE WHEN cco.numContacts is NULL THEN 0 ELSE cco.numContacts END AS countContacts, CASE WHEN cch.numChildren is null THEN 0 ELSE cch.numChildren END AS countChildren ");
      dbQueryBuilder.AppendLine(", CASE WHEN e.OrganizationName is NULL THEN '' ELSE e.OrganizationName END AS parentOrgName");
      if (start.HasValue)
      {
        int? nullable = start;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          dbQueryBuilder.AppendLine(", ROW_NUMBER() OVER(ORDER BY cte.OrganizationName) AS row");
      }
      dbQueryBuilder.AppendLine("FROM cte INNER JOIN [ExternalOrgDetail] org ON oid = org.[externalOrgID] LEFT OUTER JOIN cco ON cco.externalOrgId = org.[externalOrgID] LEFT OUTER JOIN cch ON cch.oid = org.[externalOrgID]");
      dbQueryBuilder.AppendLine("LEFT OUTER JOIN ExternalOriginatorManagement e ON cte.parent = e.oid");
      dbQueryBuilder.AppendLine("WHERE 0 = 0");
      if (orgTypes != null && orgTypes.Length != 0)
        dbQueryBuilder.AppendLine("AND org.OrganizationType IN (" + string.Join<int>(",", (IEnumerable<int>) ((IEnumerable<ExternalOriginatorOrgType>) orgTypes).Select<ExternalOriginatorOrgType, int>((System.Func<ExternalOriginatorOrgType, int>) (x => (int) x)).ToArray<int>()) + ")");
      if (start.HasValue)
      {
        int? nullable = start;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          dbQueryBuilder.AppendLine(") us WHERE row >= " + (object) start);
      }
      if (limit.HasValue)
      {
        int? nullable = limit;
        int num = 0;
        if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          dbQueryBuilder.AppendLine(") ext");
      }
      dbQueryBuilder.AppendLine("ORDER BY OrganizationName");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalOrgManagementDataCount countFromDatarow = ExternalOrgManagementAccessor.getExternalOrgManagementCountFromDatarow(row);
          countFromDatarow.DataDetails = ExternalOrgManagementAccessor.addExternalOrgDetail(countFromDatarow.DataDetails, row);
          organizationsByParent.Add(countFromDatarow);
        }
        return organizationsByParent;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizations: Cannot fetch record for parentOrgId=" + (object) parentOrgId + ", isRecursive=" + isRecursive.ToString() + ", orgType=" + (object) orgTypes + ".\r\n" + ex.Message);
      }
    }

    public static ExternalOrgManagementDataCount GetExternalOrganizationDataWithCountByOid(int orgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOrgManagementDataCount managementDataCount = new ExternalOrgManagementDataCount();
      dbQueryBuilder.AppendLine("Declare @oid int; set @oid = " + (object) orgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalOrganizationDataWithCountByOidQuery());
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (ExternalOrgManagementDataCount) null;
        ExternalOrgManagementDataCount countFromDatarow = ExternalOrgManagementAccessor.getExternalOrgManagementCountFromDatarow(dataRowCollection[0]);
        countFromDatarow.DataDetails = ExternalOrgManagementAccessor.addExternalOrgDetail(countFromDatarow.DataDetails, dataRowCollection[0]);
        return countFromDatarow;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizations: Cannot fetch record for orgId=" + (object) orgId + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      List<int> oids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> externalOrganizations = new List<ExternalOriginatorManagementData>();
      string idList = "";
      if (!oids.Any<int>())
        return new List<ExternalOriginatorManagementData>();
      oids.ForEach((Action<int>) (x =>
      {
        if (idList == "")
          idList = string.Concat((object) x);
        else
          idList = idList + ", " + (object) x;
      }));
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] a left join [ExternalOrgDetail] b  on a.oid = b.externalOrgID where oid in (" + idList + ") ORDER BY a.[HierarchyPath], a.[Depth] ");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalOriginatorManagementData result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          if (string.IsNullOrEmpty(result.CompanyDBAName))
          {
            ExternalOrgDBAName defaultDbaName = ExternalOrgManagementAccessor.GetDefaultDBAName(result.oid);
            result.CompanyDBAName = defaultDbaName == null ? "" : defaultDbaName.Name;
          }
          if (!forLender)
            result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, row);
          externalOrganizations.Add(result);
        }
        return externalOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizations: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      ExternalOriginatorEntityType entityType,
      string legalName,
      string dbaName,
      string cityName,
      string stateName,
      bool exactMatch,
      string sortedColumnName,
      bool sortedDescending,
      string currentUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> intList1 = new List<int>();
      string str = string.Empty;
      if (!forLender && currentUserID != null && string.Compare(currentUserID, "admin", true) != 0)
      {
        ArrayList andOrgBySalesRep = ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRep(currentUserID);
        if (andOrgBySalesRep != null && andOrgBySalesRep.Count > 3)
        {
          List<int> intList2 = (List<int>) andOrgBySalesRep[3];
          if (intList2.Count > 0)
          {
            for (int index = 0; index < intList2.Count; ++index)
              str = str + (str != string.Empty ? (object) "," : (object) "") + (object) intList2[index];
            dbQueryBuilder.AppendLine("Select descendent from ExternalOrgDescendents where oid in (" + str + ")");
            DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              {
                int num = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["descendent"]);
                if (!intList2.Contains(num))
                  intList2.Add(num);
              }
            }
            str = string.Empty;
            for (int index = 0; index < intList2.Count; ++index)
              str = str + (str != string.Empty ? (object) "," : (object) "") + (object) intList2[index];
          }
        }
      }
      List<ExternalOriginatorManagementData> externalOrganizations = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("SELECT org.* FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] org");
      if (!forLender)
        dbQueryBuilder.AppendLine(" INNER JOIN [ExternalOrgDetail] orgDetails on org.oid = orgDetails.externalOrgID WHERE orgDetails.OrganizationType in (0, 2)");
      else
        dbQueryBuilder.AppendLine(" WHERE 1 = 1");
      if (!string.IsNullOrEmpty(str))
        dbQueryBuilder.AppendLine(" AND [oid] in (" + str + ")");
      switch (entityType)
      {
        case ExternalOriginatorEntityType.Broker:
          dbQueryBuilder.AppendLine(" AND " + (exactMatch ? "[EntityType] = 1" : "([EntityType] = 1 OR [EntityType] = 3)"));
          break;
        case ExternalOriginatorEntityType.Correspondent:
          dbQueryBuilder.AppendLine(" AND " + (exactMatch ? "[EntityType] = 1" : "([EntityType] = 1 OR [EntityType] = 2)"));
          break;
        case ExternalOriginatorEntityType.Both:
          dbQueryBuilder.AppendLine(" AND " + (exactMatch ? "[EntityType] = 3" : "([EntityType] = 1 OR [EntityType] = 2 OR [EntityType] = 3)"));
          break;
      }
      if ((legalName ?? "") != string.Empty)
        dbQueryBuilder.AppendLine(" AND [CompanyLegalName] " + (exactMatch ? " = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) legalName) : " LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ("%" + legalName + "%"))));
      if ((dbaName ?? "") != string.Empty)
        dbQueryBuilder.AppendLine(" AND [CompanyDBAName] " + (exactMatch ? " = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) dbaName) : " LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ("%" + dbaName + "%"))));
      if ((cityName ?? "") != string.Empty)
        dbQueryBuilder.AppendLine(" AND [City] " + (exactMatch ? " = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) cityName) : " LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ("%" + cityName + "%"))));
      if ((stateName ?? "") != string.Empty)
        dbQueryBuilder.AppendLine(" AND [State] " + (exactMatch ? " = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) stateName) : " LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ("%" + stateName + "%"))));
      if ((sortedColumnName ?? "") != string.Empty)
        dbQueryBuilder.AppendLine(" ORDER BY [" + sortedColumnName + "] " + (sortedDescending ? "DESC" : "ASC"));
      else
        dbQueryBuilder.AppendLine(" ORDER BY [HierarchyPath], [Depth] ASC");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalOrganizations.Add(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row));
        return externalOrganizations;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch certain records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static IEnumerable<ExternalOrgWrapper> GetExternalOrganizations(
      out int totalCount,
      int? parentOrgId = null,
      bool isRecursive = false,
      int? start = 0,
      int? limit = 0,
      ExternalOriginatorOrgType[] orgTypes = null,
      string tpoId = null)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<int, ExternalOrgWrapper> dictionary = new Dictionary<int, ExternalOrgWrapper>();
      dbQueryBuilder1.AppendLine("WITH cte AS (");
      if (!parentOrgId.HasValue)
      {
        if (isRecursive)
        {
          if (tpoId == null)
            dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom");
          else
            dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoId));
        }
        else if (tpoId == null)
          dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.parent = 0");
        else
          dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.parent = 0 AND eom.ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoId));
      }
      else if (!isRecursive)
      {
        if (tpoId == null)
          dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN [ExternalOriginatorManagement] ep ON eom.parent = ep.oid AND ep.oid = " + (object) parentOrgId);
        else
          dbQueryBuilder1.AppendLine("SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN [ExternalOriginatorManagement] ep ON eom.parent = ep.oid AND ep.oid = " + (object) parentOrgId + " AND eom.ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoId));
      }
      else if (tpoId == null)
        dbQueryBuilder1.Append("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE Parent = " + (object) parentOrgId + " UNION ALL SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN cte c ON eom.Parent = c.oid ");
      else
        dbQueryBuilder1.Append("SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE Parent = " + (object) parentOrgId + " AND eom.ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoId) + " UNION ALL SELECT eom.* FROM [ExternalOriginatorManagement] eom JOIN cte c ON eom.Parent = c.oid ");
      dbQueryBuilder1.AppendLine("),");
      dbQueryBuilder1.AppendLine("TempView AS (");
      dbQueryBuilder1.AppendLine("SELECT cte.*, org.*");
      dbQueryBuilder1.AppendLine(", CASE WHEN e.OrganizationName is NULL THEN '' ELSE e.OrganizationName END AS parentOrgName");
      dbQueryBuilder1.AppendLine("FROM cte INNER JOIN [ExternalOrgDetail] org ON oid = org.[externalOrgID]");
      dbQueryBuilder1.AppendLine("LEFT OUTER JOIN ExternalOriginatorManagement e ON cte.parent = e.oid");
      dbQueryBuilder1.AppendLine("WHERE 0 = 0");
      if (orgTypes != null && orgTypes.Length != 0)
        dbQueryBuilder1.AppendLine("AND org.OrganizationType IN (" + string.Join<int>(",", (IEnumerable<int>) ((IEnumerable<ExternalOriginatorOrgType>) orgTypes).Select<ExternalOriginatorOrgType, int>((System.Func<ExternalOriginatorOrgType, int>) (x => (int) x)).ToArray<int>()) + ")");
      dbQueryBuilder1.AppendLine(")");
      try
      {
        DataRowCollection dataRowCollection = (DataRowCollection) null;
        bool flag = start.HasValue && limit.HasValue;
        if (flag)
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
          List<SortColumn> sortColumnList = new List<SortColumn>()
          {
            new SortColumn("OrganizationName", System.Data.SqlClient.SortOrder.Ascending)
          };
          DataTable paginatedRecords = dbQueryBuilder2.GetCTEPaginatedRecords(dbQueryBuilder1.ToString(), start.Value + 1, start.Value + limit.Value, sortColumnList);
          if (paginatedRecords != null && paginatedRecords.Rows != null)
            dataRowCollection = paginatedRecords.Rows;
        }
        else
        {
          dbQueryBuilder1.AppendLine("select * from TempView");
          dataRowCollection = dbQueryBuilder1.Execute();
        }
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          totalCount = !flag ? dataRowCollection.Count : EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0]["TotalRowCount"]);
          foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
          {
            ExternalOrgManagementDataCount countFromDatarow = ExternalOrgManagementAccessor.getExternalOrgManagementCountFromDatarow(row);
            countFromDatarow.DataDetails = ExternalOrgManagementAccessor.addExternalOrgDetail(countFromDatarow.DataDetails, row);
            dictionary[countFromDatarow.DataDetails.oid] = new ExternalOrgWrapper()
            {
              ExternalManagementDataCount = countFromDatarow
            };
          }
          if (dictionary.Any<KeyValuePair<int, ExternalOrgWrapper>>())
            ExternalOrgManagementAccessor.GetExternalOrgsAdditionalDetails(dictionary);
        }
        else
          totalCount = 0;
        return (IEnumerable<ExternalOrgWrapper>) dictionary.Values;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizations: Cannot fetch record for parentOrgId=" + (object) parentOrgId + ", isRecursive=" + isRecursive.ToString() + ", orgType=" + (object) orgTypes + ".\r\n" + ex.Message);
      }
    }

    private static void GetExternalOrgsAdditionalDetails(Dictionary<int, ExternalOrgWrapper> orgs)
    {
      string strParentOrgIds = string.Join<int>(",", (IEnumerable<int>) orgs.Keys);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.GetExternalOrgsAdditionalDataQuery(strParentOrgIds));
      DataSet ds = dbQueryBuilder.ExecuteSetQuery();
      if (ds == null || ds.Tables.Count <= 0)
        return;
      ExternalOrgManagementAccessor.GatherExternalOrgsData(ds, orgs);
    }

    private static string GetExternalOrgsAdditionalDataQuery(string strParentOrgIds)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(ExternalOrgManagementAccessor.GetExternalOrgsChildrenEntityQuery(strParentOrgIds));
      return stringBuilder.ToString();
    }

    private static string GetExternalOrgsChildrenEntityQuery(string strParentOrgIds)
    {
      return "Select a.oid,a.Parent,b.OrganizationType,a.OrganizationName from [ExternalOriginatorManagement] a left join [ExternalOrgDetail] b  on a.oid = b.externalOrgID  where Parent in (" + strParentOrgIds + ")";
    }

    private static void GatherExternalOrgsData(DataSet ds, Dictionary<int, ExternalOrgWrapper> orgs)
    {
      if (ds.Tables.Count <= 0 || ds.Tables[0].Rows == null)
        return;
      ExternalOrgManagementAccessor.GatherChildrenOrgs(ds.Tables[0].Rows, orgs);
    }

    private static void GatherChildrenOrgs(
      DataRowCollection rows,
      Dictionary<int, ExternalOrgWrapper> orgs)
    {
      foreach (DataRow row in (InternalDataCollectionBase) rows)
      {
        ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
        originatorManagementData.oid = Convert.ToInt32(row["oid"]);
        originatorManagementData.Parent = Convert.ToInt32(row["Parent"]);
        originatorManagementData.OrganizationName = Convert.ToString(row["OrganizationName"]);
        originatorManagementData.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["OrganizationType"]);
        if (orgs[originatorManagementData.Parent].ChildOrganizations == null)
          orgs[originatorManagementData.Parent].ChildOrganizations = new List<ExternalOriginatorManagementData>();
        orgs[originatorManagementData.Parent].ChildOrganizations.Add(originatorManagementData);
      }
    }

    public static List<int> GetExternalOrganizationDesendents(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> organizationDesendents = new List<int>();
      dbQueryBuilder.AppendLine("Select descendent from ExternalOrgDescendents where oid = " + (object) oid);
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return organizationDesendents;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          organizationDesendents.Add(Utils.ParseInt((object) string.Concat(row[0])));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDescendents.\r\n" + ex.Message);
      }
      return organizationDesendents;
    }

    public static List<int> GetExternalOrganizationDesendents(int oid, bool excludeDisabled)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> organizationDesendents = new List<int>();
      if (excludeDisabled)
        dbQueryBuilder.AppendLine("Select des.descendent from ExternalOrgDescendents as des  inner join ExternalOrgDetail  as eod on  des.descendent=eod.externalOrgID where eod.DisableLogin = 0 and des.oid = " + (object) oid);
      else
        dbQueryBuilder.AppendLine("Select descendent from ExternalOrgDescendents where oid = " + (object) oid);
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return organizationDesendents;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          organizationDesendents.Add(Utils.ParseInt((object) string.Concat(row[0])));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDescendents.\r\n" + ex.Message);
      }
      return organizationDesendents;
    }

    public static List<string> GetExternalOrganizationDesendentsTPOID(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> organizationDesendentsTpoid = new List<string>();
      dbQueryBuilder.AppendLine("select externalID from ExternalOriginatorManagement where oid in (Select descendent from ExternalOrgDescendents where oid = " + (object) oid + ")");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return organizationDesendentsTpoid;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          organizationDesendentsTpoid.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString((object) string.Concat(row[0])));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDescendents.\r\n" + ex.Message);
      }
      return organizationDesendentsTpoid;
    }

    public static List<int> GetExternalOrganizationParents(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> organizationParents = new List<int>();
      dbQueryBuilder.AppendLine("select oid from ExternalOrgDescendents where descendent = " + (object) oid + " order by oid");
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          organizationParents.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow[0]));
        return organizationParents;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDescendents.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRep(
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> oids = new List<int>();
      dbQueryBuilder.AppendLine("Select distinct externalOrgID from ExternalOrgSalesReps where UserID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalOriginatorManagementData>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          oids.Add(Utils.ParseInt((object) string.Concat(row[0])));
        return ExternalOrgManagementAccessor.GetExternalOrganizations(false, oids);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgSalesReps.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRepWithPrimarySalesRep(
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> oids = new List<int>();
      dbQueryBuilder.AppendLine("Select distinct externalOrgID from ExternalOrgSalesReps where UserID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid) + "union select distinct externalOrgid from [ExternalOrgDetail] where [PrimarySalesRepUserId] =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalOriginatorManagementData>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          oids.Add(Utils.ParseInt((object) string.Concat(row[0])));
        return ExternalOrgManagementAccessor.GetExternalOrganizations(false, oids);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgSalesReps.\r\n" + ex.Message);
      }
    }

    public static int GetExternalOrganizationCountForManagerID(string managerID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> intList = new List<int>();
      dbQueryBuilder.AppendLine("Select count(*) from [ExternalOrgDetail] where ManagerUserID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(managerID));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count == 0 ? 0 : EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0][0]);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDetail.\r\n" + ex.Message);
      }
    }

    public static Dictionary<string, int> GetExternalOrganizationCountForManagerIDs(
      string[] managerIDs)
    {
      Dictionary<string, int> countForManagerIds = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ManagerUserID, count(*) OrgCount from [ExternalOrgDetail]");
      dbQueryBuilder.AppendLine("where ManagerUserID in (" + string.Join(",", EllieMae.EMLite.DataAccess.SQL.EncodeString(managerIDs, true)) + ")");
      dbQueryBuilder.AppendLine("group by ManagerUserID");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return countForManagerIds;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          countForManagerIds.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["ManagerUserID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["OrgCount"]));
        return countForManagerIds;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDetail.\r\n" + ex.Message);
      }
    }

    public static List<int> GetExternalOrganizationsForManagerID(string managerID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> organizationsForManagerId = new List<int>();
      dbQueryBuilder.AppendLine("Select externalorgID from [ExternalOrgDetail] where ManagerUserID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(managerID));
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          organizationsForManagerId.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["externalorgID"]));
        return organizationsForManagerId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalOrgDetail.\r\n" + ex.Message);
      }
    }

    public static void UpdateOrgTypeAndTpoID(List<int> children, int orgType, string TpoID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (int child in children)
      {
        dbQueryBuilder.AppendLine("Update [ExternalOriginatorManagement] set ExternalID = '" + TpoID + "' where oid = " + (object) child);
        dbQueryBuilder.AppendLine("Update [ExternalOrgDetail] set [OrganizationType] = " + (object) orgType + " where [externalOrgID] = " + (object) child);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationsUsingCompPlan(
      int loCompID)
    {
      List<ExternalOriginatorManagementData> organizationsUsingCompPlan = new List<ExternalOriginatorManagementData>();
      for (int index = 1; index <= 2; ++index)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT DISTINCT org.oid, org.* FROM [" + (index == 2 ? "ExternalLenders" : "ExternalOriginatorManagement") + "] org");
        dbQueryBuilder.AppendLine("  INNER JOIN [" + (index == 2 ? "Ext_LenderCompPlans" : "Ext_CompPlans") + "] orgPlan ON org.oid = orgPlan.oid");
        dbQueryBuilder.AppendLine(" WHERE orgPlan.compplanid = " + (object) loCompID);
        try
        {
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          if (dataSet != null)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
              organizationsUsingCompPlan.Add(managementFromDatarow);
            }
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationsUsingCompPlan: Cannot fetch records from " + (index == 2 ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
        }
      }
      return organizationsUsingCompPlan;
    }

    public static ExternalOriginatorManagementData GetByoid(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE [oid] = (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + ")");
      try
      {
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        if (dataRowCollection1 == null || dataRowCollection1.Count <= 0)
          return (ExternalOriginatorManagementData) null;
        ExternalOriginatorManagementData result = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection1[0]);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgDetail] where [externalOrgID] = " + (object) oid);
        DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
        if (dataRowCollection2.Count > 0)
          result = ExternalOrgManagementAccessor.addExternalOrgDetail(result, dataRowCollection2[0]);
        return result;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetByoid: Cannot fetch records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static Dictionary<string, string> GetOrgTree(int organizationId)
    {
      Dictionary<string, string> orgTree = new Dictionary<string, string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      dbQueryBuilder.AppendLine(";WITH ret AS(\tSELECT oid, HierarchyPath FROM ExternalOriginatorManagement WHERE\toid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) organizationId));
      dbQueryBuilder.AppendLine("\t\t\t\tUNION ALL");
      dbQueryBuilder.AppendLine("\t\t\t\tSELECT\tt.oid, t.HierarchyPath FROM\tExternalOriginatorManagement t INNER JOIN ret r ON t.Parent = r.oid");
      dbQueryBuilder.AppendLine(")");
      dbQueryBuilder.AppendLine("SELECT  * ");
      dbQueryBuilder.AppendLine("FROM    ret ");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              orgTree.Add(Convert.ToString(dataRow["oid"]), Convert.ToString(dataRow["HierarchyPath"]));
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOrgTree: Cannot fetch records from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
      return orgTree;
    }

    public static Dictionary<string, string> GetOrgTree(int organizationId, bool excludeDisabled)
    {
      Dictionary<string, string> orgTree = new Dictionary<string, string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      dbQueryBuilder.AppendLine(";WITH ret AS(\tSELECT oid, HierarchyPath FROM ExternalOriginatorManagement WHERE\toid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) organizationId));
      dbQueryBuilder.AppendLine("\t\t\t\tUNION ALL");
      if (excludeDisabled)
        dbQueryBuilder.AppendLine("\tSELECT\tt.oid, t.HierarchyPath FROM\tExternalOriginatorManagement t INNER JOIN ret r ON t.Parent = r.oid inner join ExternalOrgDetail erd on t.oid=erd.externalOrgID and  erd.DisableLogin=0");
      else
        dbQueryBuilder.AppendLine("\tSELECT\tt.oid, t.HierarchyPath FROM\tExternalOriginatorManagement t INNER JOIN ret r ON t.Parent = r.oid");
      dbQueryBuilder.AppendLine(")");
      dbQueryBuilder.AppendLine("SELECT  * ");
      dbQueryBuilder.AppendLine("FROM    ret ");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              orgTree.Add(Convert.ToString(dataRow["oid"]), Convert.ToString(dataRow["HierarchyPath"]));
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOrgTree: Cannot fetch records from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
      return orgTree;
    }

    public static List<ExternalOriginatorManagementData> GetContactByName(
      bool forLender,
      string ContactName,
      bool searchLegalName)
    {
      List<ExternalOriginatorManagementData> contactByName = new List<ExternalOriginatorManagementData>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE " + (searchLegalName ? "[CompanyLegalName]" : "[CompanyDBAName]") + " in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) ContactName) + ")");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          contactByName.Add(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row));
        return contactByName;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetContactByName: Cannot fetch records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfOrgNameExists(bool forLender, string orgName, int externalOrgId)
    {
      List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (externalOrgId == 0)
        dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE [OrganizationName] in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgName) + ")");
      else
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOriginatorManagement] WHERE [OrganizationName] in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgName) + ") AND  ([oid] = " + (object) externalOrgId + " OR [oid]  in (SELECT oid FROM ExternalOrgDescendents WHERE descendent = " + (object) externalOrgId + ")  OR [oid]  in (SELECT descendent FROM ExternalOrgDescendents WHERE oid = " + (object) externalOrgId + "))");
      try
      {
        return dbQueryBuilder.Execute().Count > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfOrgNameExists: can not Check if there is any other company other than selcted company with given org name.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfOrgExists(bool forLender, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 1 FROM [" + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + "] where oid = " + (object) externalOrgId);
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfOrgExists: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfOrgExistsWithTpoIdAndSalesRep(string TPOId, string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRepQuery(ExternalOrgManagementAccessor.GetUserIdFromExtOrgQuery(userId, 0), TPOId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfOrgExistsWithTpoIdAndSalesRep: Cannot fetch record from ExternalOrgAncestors & ExternalOrgSalesReps table.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfOrgExistsWithTpoId(bool forLender, string TPOId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 1 FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] where parent = 0 AND ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) TPOId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfParentOrgExists: Cannot fetch record from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData GetExternalCompanyByTPOID(
      bool forLender,
      string TPOId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE [ExternalID] =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) TPOId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          IEnumerator enumerator = dataRowCollection.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
              return ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow((DataRow) enumerator.Current);
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        }
        return (ExternalOriginatorManagementData) null;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalCompanyByTPOID: Cannot fetch records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<long> GetAllTpoID()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<long> allTpoId = new List<long>();
      dbQueryBuilder.AppendLine("SELECT ExternalID FROM ExternalOriginatorManagement");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (List<long>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          if (dataRow["ExternalID"].ToString() != "")
            allTpoId.Add(long.Parse(Convert.ToString(dataRow["ExternalID"])));
        }
        return allTpoId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllTpoID: Cannot fetch records from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static List<string> GetAllTpoIDAsString()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> allTpoIdAsString = new List<string>();
      dbQueryBuilder.AppendLine("SELECT ExternalID FROM ExternalOriginatorManagement where ExternalID is not null AND ExternalID != '' ");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (List<string>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          allTpoIdAsString.Add(Convert.ToString(dataRow["ExternalID"]));
        return allTpoIdAsString;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllTpoIDAsString: Cannot fetch records. \r\n" + ex.Message);
      }
    }

    public static List<string> GetAllLoginEmailID(string contactID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> allLoginEmailId = new List<string>();
      dbQueryBuilder.AppendLine("SELECT [Login_email] FROM [ExternalUsers] where external_userid <> '" + contactID + "' and status = 0");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (List<string>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          if (dataRow["Login_email"].ToString() != "")
            allLoginEmailId.Add(Convert.ToString(dataRow["Login_email"]));
        }
        return allLoginEmailId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllLoginEmailID: Cannot fetch records from ExternalUsers table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetAllContactsByLoginEmailId(
      string loginEmailID,
      string externalUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> contactsByLoginEmailId = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] where ExternalUsers.status = 0 AND ExternalUsers.Login_email = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loginEmailID) + " AND external_userid <> " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserId));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            contactsByLoginEmailId.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return contactsByLoginEmailId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllContactsByLoginEmailId: Cannot fetch records from ExternalUsers table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfoURL> GetExternalUserInfoUrlsByContactIds(
      string externalUserIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfoURL> urlsByContactIds = new List<ExternalUserInfoURL>();
      dbQueryBuilder.AppendLine("SELECT distinct urlID,external_userid FROM [ExternalUserURLs]  where external_userid in ( '" + externalUserIds + "')");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (List<ExternalUserInfoURL>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalUserInfoURL externalUserInfoUrl = new ExternalUserInfoURL()
          {
            URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["urlID"]),
            ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["external_userid"])
          };
          urlsByContactIds.Add(externalUserInfoUrl);
        }
        return urlsByContactIds;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfoUrlsByContactIds: Cannot fetch a record from [ExternalUserInfoURL] table.\r\n" + ex.Message);
      }
    }

    public static void DisableContactsLogin(string externalUserIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "UPDATE [ExternalUsers] SET [status] = 1 WHERE external_userid in ( '" + externalUserIds + "')";
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<long> GetAllContactID()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<long> allContactId = new List<long>();
      dbQueryBuilder.AppendLine("SELECT ContactID FROM [ExternalUsers]");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (List<long>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          if (dataRow["ContactID"].ToString() != "")
            allContactId.Add(long.Parse(Convert.ToString(dataRow["ContactID"])));
        }
        return allContactId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllContactID: Cannot fetch records from ExternalUsers table.\r\n" + ex.Message);
      }
    }

    public static LoanCompHistoryList GetCompPlansByoid(bool forLender, int oid)
    {
      return LOCompAccessor.GetComPlanHistoryforOrg(oid, true, forLender);
    }

    public static int GetOidByTPOId(bool forLender, string ExternalID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE [ContactType] = 1 AND [ExternalID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalID));
      try
      {
        object obj = dbQueryBuilder.ExecuteScalar();
        return obj != null ? Convert.ToInt32(obj) : 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOidByTPOId: Cannot fetch id records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static int GetOidByBusinessId(bool forLender, int Id)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [" + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + "] WHERE [ContactType] = 2 AND [ExternalID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) string.Concat((object) Id)));
      try
      {
        object obj = dbQueryBuilder.ExecuteScalar();
        return obj != null ? Convert.ToInt32(obj) : 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOidByBusinessId: Cannot fetch id records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<int> GetOidByParentId(bool forLender, int parentId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> oidByParentId = new List<int>();
      dbQueryBuilder.AppendLine("SELECT oid FROM [" + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + "] WHERE [Parent] =" + (object) parentId);
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          oidByParentId.Add(Convert.ToInt32(dataRow["oid"]));
        return oidByParentId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOidByParentId: Cannot fetch id records from " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static List<HierarchySummary> GetHierarchy(bool forLender)
    {
      return ExternalOrgManagementAccessor.GetHierarchy(forLender, -1);
    }

    public static List<HierarchySummary> GetHierarchy(bool forLender, int parentId)
    {
      List<HierarchySummary> hierarchy = new List<HierarchySummary>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid, ExternalID, Parent, OrganizationName,CompanyLegalName,CompanyDBAName, Depth, HierarchyPath FROM " + (forLender ? "[ExternalLenders]" : "[ExternalOriginatorManagement]") + (parentId == -1 ? " ORDER BY Parent ASC" : " Where Parent = " + (object) parentId));
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          hierarchy.Add(ExternalOrgManagementAccessor.getHierarchySummaryFromDatarow(row));
        return hierarchy;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetHierarchy: Cannot fetch HierarchySummary records from " + (forLender ? "[ExternalLenders]" : "[ExternalOriginatorManagement]") + " table.\r\n" + ex.Message);
      }
    }

    public static HierarchySummary GetOrgDetails(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid, ExternalID, Parent,OrganizationName,CompanyDBAName,CompanyLegalName, Depth, HierarchyPath FROM " + (forLender ? (object) "[ExternalLenders]" : (object) "[ExternalOriginatorManagement]") + " Where oid = " + (object) oid);
      try
      {
        return ExternalOrgManagementAccessor.getHierarchySummaryFromDatarow(dbQueryBuilder.Execute()[0]);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetOrgDetails: Cannot fetch HierarchySummary records from " + (forLender ? "[ExternalLenders]" : "[ExternalOriginatorManagement]") + " table.\r\n" + ex.Message);
      }
    }

    public static BranchExtLicensing GetExtLicenseDetails(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT AllowLoansWithIssues, MsgUploadNonApprovedLoans, InheritParentLicense, LicenseLenderType, LicenseHomeState, LicenseOptOut, LicenseStatutoryMaryland, LicenseStatutoryMaryland2, LicenseStatutoryKansas,ATRQMSmallCreditor,ATRQMExemptCreditor FROM [ExternalOrgDetail] Where externalOrgID = " + (object) oid);
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return (BranchExtLicensing) null;
        BranchExtLicensing licensingFromDatarow = ExternalOrgManagementAccessor.getBranchExtLicensingFromDatarow(dataRowCollection[0]);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT State, LicenseType, LicenseApproved, LicenseExempt, LicenseNumber,IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate, SortIndex FROM [ExternalOrgStateLicensing] Where externalOrgID = " + (object) oid + " order by externalOrgID, state, sortIndex");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          licensingFromDatarow.AddStateLicenseExtType(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, true));
        return licensingFromDatarow;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExtLicenseDetails: Cannot fetch BranchExtLicensing records from [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    public static BranchExtLicensing GetExportLicensesDetails(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@FinalOid", "INT");
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ExternalOrgDetail");
      dbQueryBuilder.IfExists(table, new DbValueList()
      {
        new DbValue("externalOrgID", (object) oid),
        new DbValue("InheritParentLicense", (object) 1)
      });
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine(string.Format("SELECT @FinalOid = Parent FROM [ExternalOriginatorManagement] WHERE Oid = {0}", (object) oid));
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine(string.Format("SELECT @FinalOid = Oid FROM [ExternalOriginatorManagement] WHERE Oid = {0}", (object) oid));
      dbQueryBuilder.End();
      dbQueryBuilder.AppendLine("SELECT State, LicenseType, LicenseApproved, LicenseExempt, LicenseNumber,IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate, SortIndex FROM [ExternalOrgStateLicensing] Where externalOrgID = @FinalOid AND LicenseApproved = 1 order by externalOrgID, state, sortIndex");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return (BranchExtLicensing) null;
        BranchExtLicensing exportLicensesDetails = new BranchExtLicensing();
        if (dataRowCollection != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
            exportLicensesDetails.AddStateLicenseExtType(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
        }
        return exportLicensesDetails;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExportLicensesDetails: Cannot fetch BranchExtLicensing records from [ExternalOrgStateLicensing] table.\r\n" + ex.Message);
      }
    }

    public static BranchExtLicensing GetExtLicenseDetails(string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        BranchExtLicensing extLicenseDetails = new BranchExtLicensing();
        dbQueryBuilder.AppendLine("SELECT State, LicenseApproved, LicenseExempt, LicenseNumber, IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate FROM [ExternalUserStateLicensing] Where [external_userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserID) + " order by external_userid, state");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
            extLicenseDetails.AddStateLicenseExtType(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
        }
        return extLicenseDetails;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExtLicenseDetails: Cannot fetch BranchExtLicensing records from [ExternalUserStateLicensing] table.\r\n" + ex.Message);
      }
    }

    public static string GetExtUserNotes(string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("SELECT Note FROM [ExternalUsers] Where [external_userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null ? EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["Note"]) : "";
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExtUserNotes: Cannot fetch BranchExtLicensing records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    public static List<object> GetExternalAdditionalDetails(int oid)
    {
      try
      {
        return new List<object>()
        {
          (object) ExternalOrgManagementAccessor.GetExtLicenseDetails(oid),
          (object) ExternalOrgManagementAccessor.GetExternalOrganizationLoanTypes(oid),
          (object) ExternalOrgManagementAccessor.GetExternalOrganizationPrimarySalesRepUserInfos(oid),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Current Company Status"),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Current Contact Status"),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Company Rating"),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Attachment Category"),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Price Group"),
          (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("ICE PPE Rate Sheet")
        };
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAdditionalDetails: Cannot fetch all additional details records for external organization. Error:" + ex.Message);
      }
    }

    public static Dictionary<ExternalOriginatorOrgSetting, object> GetExternalAdditionalDetails(
      int oid,
      List<ExternalOriginatorOrgSetting> requested)
    {
      try
      {
        ExternalOriginatorManagementData originatorManagementData = (ExternalOriginatorManagementData) null;
        Dictionary<ExternalOriginatorOrgSetting, object> additionalDetails = new Dictionary<ExternalOriginatorOrgSetting, object>();
        if (requested.Contains(ExternalOriginatorOrgSetting.AssignableSalesReps))
          additionalDetails.Add(ExternalOriginatorOrgSetting.AssignableSalesReps, (object) ExternalOrgManagementAccessor.GetExternalOrganizationPrimarySalesRepUserInfos(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.AttachmentCategory))
          additionalDetails.Add(ExternalOriginatorOrgSetting.AttachmentCategory, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Attachment Category"));
        if (requested.Contains(ExternalOriginatorOrgSetting.CompanyRating))
          additionalDetails.Add(ExternalOriginatorOrgSetting.CompanyRating, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Company Rating"));
        if (requested.Contains(ExternalOriginatorOrgSetting.CompanyStatus))
          additionalDetails.Add(ExternalOriginatorOrgSetting.CompanyStatus, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Current Company Status"));
        if (requested.Contains(ExternalOriginatorOrgSetting.ContactStatus))
          additionalDetails.Add(ExternalOriginatorOrgSetting.ContactStatus, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Current Contact Status"));
        if (requested.Contains(ExternalOriginatorOrgSetting.License))
          additionalDetails.Add(ExternalOriginatorOrgSetting.License, (object) ExternalOrgManagementAccessor.GetExtLicenseDetails(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.LoanTypes))
          additionalDetails.Add(ExternalOriginatorOrgSetting.LoanTypes, (object) ExternalOrgManagementAccessor.GetExternalOrganizationLoanTypes(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.PriceGroup))
          additionalDetails.Add(ExternalOriginatorOrgSetting.PriceGroup, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Price Group"));
        if (requested.Contains(ExternalOriginatorOrgSetting.UrlList))
          additionalDetails.Add(ExternalOriginatorOrgSetting.UrlList, (object) ExternalOrgManagementAccessor.GetSelectedOrgUrls(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.PrimaryManager))
        {
          originatorManagementData = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid);
          additionalDetails.Add(ExternalOriginatorOrgSetting.PrimaryManager, (object) ExternalOrgManagementAccessor.GetExternalUserInfo(originatorManagementData.Manager));
        }
        if (requested.Contains(ExternalOriginatorOrgSetting.Note))
          additionalDetails.Add(ExternalOriginatorOrgSetting.Note, (object) ExternalOrgManagementAccessor.GetExternalOrganizationNotes(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.Attachment))
          additionalDetails.Add(ExternalOriginatorOrgSetting.Attachment, (object) ExternalOrgManagementAccessor.GetExternalAttachmentsByOid(oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.LOComp))
          additionalDetails.Add(ExternalOriginatorOrgSetting.LOComp, (object) LOCompAccessor.GetAllCompPlans(true, 2));
        if (requested.Contains(ExternalOriginatorOrgSetting.LoCompHistory))
          additionalDetails.Add(ExternalOriginatorOrgSetting.LoCompHistory, (object) ExternalOrgManagementAccessor.GetCompPlansByoid(false, oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg))
        {
          List<object> objectList = new List<object>();
          if (originatorManagementData == null)
            originatorManagementData = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid);
          objectList.Add(originatorManagementData == null || string.IsNullOrWhiteSpace(originatorManagementData.PrimarySalesRepUserId) ? (object) ExternalOrgManagementAccessor.GetPrimarySalesRep(oid) : (object) originatorManagementData.PrimarySalesRepUserId);
          objectList.Add((object) ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCurrentOrg(oid));
          objectList.Add(originatorManagementData == null || !(originatorManagementData.PrimarySalesRepAssignedDate != DateTime.MinValue) ? (object) string.Empty : (object) originatorManagementData.PrimarySalesRepAssignedDate.ToString());
          additionalDetails.Add(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg, (object) objectList);
        }
        return additionalDetails;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAdditionalDetails: Cannot fetch all additional details records for external organization. Error:" + ex.Message);
      }
    }

    private static IEnumerable<UserInfo> GetExternalOrganizationPrimarySalesRepUserInfos(int oid)
    {
      IEnumerable<UserInfo> salesRepUserInfos = (IEnumerable<UserInfo>) new List<UserInfo>();
      ExternalOriginatorManagementData externalOrganization = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid);
      if (externalOrganization == null)
        return salesRepUserInfos;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select userid from ExternalOrgSalesReps OSR where externalorgID in (select distinct oid from ExternalOriginatorManagement where ExternalID = '" + externalOrganization.ExternalID + "')");
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select sales_rep_userid as userid from ExternalUsers where externalOrgID in (select distinct oid from ExternalOriginatorManagement EOM where ExternalID = '" + externalOrganization.ExternalID + "')");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<string> stringList = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        stringList.Add(string.Concat(row[0]));
      if (stringList.Count > 0)
        salesRepUserInfos = (IEnumerable<UserInfo>) User.GetUsers(stringList.ToArray());
      return salesRepUserInfos;
    }

    public static List<object> GetExternalAdditionalDetails(string externalUserID)
    {
      try
      {
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(externalUserID);
        return new List<object>()
        {
          (object) ExternalOrgManagementAccessor.GetExtLicenseDetails(externalUserID),
          (object) ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserID),
          (object) ExternalOrgManagementAccessor.GetSelectedOrgUrls(externalUserInfo.ExternalOrgID)
        };
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAdditionalDetails: Cannot fetch all additional details records for external organization. Error:" + ex.Message);
      }
    }

    public static Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> GetExternalAdditionalDetails(
      string externalUserID,
      List<ExternalUserInfo.ExternalUserInfoSetting> requested)
    {
      try
      {
        Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> additionalDetails = new Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object>();
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.License))
          additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.License, (object) ExternalOrgManagementAccessor.GetExtLicenseDetails(externalUserID));
        ExternalUserInfo externalUserInfo = (ExternalUserInfo) null;
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls) || requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser) || requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.SalesRep))
          externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(externalUserID);
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls))
          additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls, (object) new List<object>()
          {
            (object) ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserID),
            (object) ExternalOrgManagementAccessor.GetSelectedOrgUrls(externalUserInfo.ExternalOrgID)
          });
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser))
        {
          if (externalUserInfo.UpdatedByExternal && externalUserInfo.UpdatedBy != "")
            additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser, (object) ExternalOrgManagementAccessor.GetExternalUserInfoByContactId(externalUserInfo.UpdatedBy));
          else
            additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser, (object) null);
        }
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.AccessibleUserList))
          additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.AccessibleUserList, (object) ExternalOrgManagementAccessor.GetAccessibleExternalUserInfoList(externalUserID));
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.ContactStatus))
          additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.ContactStatus, (object) ExternalOrgManagementAccessor.GetExternalOrgSettingsByName("Current Contact Status"));
        if (requested.Contains(ExternalUserInfo.ExternalUserInfoSetting.SalesRep))
          additionalDetails.Add(ExternalUserInfo.ExternalUserInfoSetting.SalesRep, (object) User.GetUserById(externalUserInfo.SalesRepID));
        return additionalDetails;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAdditionalDetails: Cannot fetch all additional details records for external organization. Error:" + ex.Message);
      }
    }

    public static bool GetUseParentInfoValue(bool forLender, int oid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT UseParentInfo FROM " + (forLender ? (object) "[ExternalLenders]" : (object) "[ExternalOriginatorManagement]") + " Where oid = " + (object) oid);
        return Convert.ToBoolean(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetUseParentInfoValue: Cannot fetch HierarchySummary records from " + (forLender ? "[ExternalLenders]" : "[ExternalOriginatorManagement]") + " table.\r\n" + ex.Message);
      }
    }

    private static HierarchySummary getHierarchySummaryFromDatarow(DataRow row)
    {
      return new HierarchySummary()
      {
        oid = Convert.ToInt32(row["oid"]),
        Parent = Convert.ToInt32(row["Parent"]),
        OrganizationName = Convert.ToString(row["OrganizationName"]),
        CompanyDBAName = Convert.ToString(row["CompanyDBAName"]),
        CompanyLegalName = Convert.ToString(row["CompanyLegalName"]),
        Depth = Convert.ToInt32(row["Depth"]),
        HierarchyPath = Convert.ToString(row["HierarchyPath"]),
        ExternalID = string.Concat(row["ExternalID"])
      };
    }

    private static BranchExtLicensing getBranchExtLicensingFromDatarow(DataRow row)
    {
      int int32 = row["AllowLoansWithIssues"] != DBNull.Value ? Convert.ToInt32(row["AllowLoansWithIssues"]) : 0;
      string msgUploadNonApprovedLoans = Convert.ToString(row["MsgUploadNonApprovedLoans"]);
      bool useParentInfo = row["InheritParentLicense"] != DBNull.Value && Convert.ToBoolean(row["InheritParentLicense"]);
      string lenderType = Convert.ToString(row["LicenseLenderType"]);
      string homeState = Convert.ToString(row["LicenseHomeState"]);
      string optOut = Convert.ToString(row["LicenseOptOut"]);
      bool statutoryElectionInKansas = row["LicenseStatutoryKansas"] != DBNull.Value && Convert.ToBoolean(row["LicenseStatutoryKansas"]);
      bool statutoryElectionInMaryland = row["LicenseStatutoryMaryland"] != DBNull.Value && Convert.ToBoolean(row["LicenseStatutoryMaryland"]);
      string statutoryElectionInMaryland2 = Convert.ToString(row["LicenseStatutoryMaryland2"]);
      BranchLicensing.ATRSmallCreditors atrSmallCreditor = row["ATRQMSmallCreditor"] != DBNull.Value ? (BranchLicensing.ATRSmallCreditors) Convert.ToInt32(row["ATRQMSmallCreditor"]) : BranchLicensing.ATRSmallCreditors.None;
      BranchLicensing.ATRExemptCreditors atrExemptCreditor = row["ATRQMExemptCreditor"] != DBNull.Value ? (BranchLicensing.ATRExemptCreditors) Convert.ToInt32(row["ATRQMExemptCreditor"]) : BranchLicensing.ATRExemptCreditors.None;
      return new BranchExtLicensing(useParentInfo, int32, msgUploadNonApprovedLoans, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, (List<StateLicenseExtType>) null, false, atrSmallCreditor, atrExemptCreditor);
    }

    private static StateLicenseExtType getStateLicenseExtTypeFromDatarow(DataRow row, bool org)
    {
      string stateAbbrevation = Convert.ToString(row["State"]);
      string licenseType = row.Table.Columns.Contains("LicenseType") ? Convert.ToString(row["LicenseType"]) : "";
      bool boolean1 = Convert.ToBoolean(row["LicenseApproved"]);
      bool boolean2 = Convert.ToBoolean(row["LicenseExempt"]);
      string licenseNo = Convert.ToString(row["LicenseNumber"]);
      DateTime issueDate = row["IssueDate"] != DBNull.Value ? Convert.ToDateTime(row["IssueDate"]) : DateTime.MinValue;
      DateTime startDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : DateTime.MinValue;
      DateTime endDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : DateTime.MinValue;
      string licenseStatus = Convert.ToString(row["Status"]);
      DateTime statusDate = row["StatusDate"] != DBNull.Value ? Convert.ToDateTime(row["StatusDate"]) : DateTime.MinValue;
      DateTime lastChecked = row["LastCheckedDate"] != DBNull.Value ? Convert.ToDateTime(row["LastCheckedDate"]) : DateTime.MinValue;
      if (!org)
        return new StateLicenseExtType(stateAbbrevation, licenseType, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, boolean1, boolean2, lastChecked);
      int int32 = row["sortIndex"] != DBNull.Value ? Convert.ToInt32(row["SortIndex"]) : 0;
      return new StateLicenseExtType(stateAbbrevation, licenseType, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, boolean1, boolean2, lastChecked, int32);
    }

    [PgReady]
    private static ExternalOriginatorManagementData getExternalOriginatorManagementFromDatarow(
      DataRow row)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        ExternalOriginatorManagementData managementFromDatarow = new ExternalOriginatorManagementData();
        managementFromDatarow.oid = Convert.ToInt32(row["oid"]);
        managementFromDatarow.contactType = (ExternalOriginatorContactType) Convert.ToInt32(row["ContactType"]);
        managementFromDatarow.Parent = Convert.ToInt32(row["Parent"]);
        managementFromDatarow.ExternalID = Convert.ToString(row["ExternalID"]);
        managementFromDatarow.CompanyDBAName = Convert.ToString(row["CompanyDBAName"]);
        managementFromDatarow.CompanyLegalName = Convert.ToString(row["CompanyLegalName"]);
        managementFromDatarow.OrganizationName = Convert.ToString(row["OrganizationName"]);
        if (row.Table.Columns.Contains("OrganizationType"))
          managementFromDatarow.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["OrganizationType"]);
        if (row["VisibleOnTPOWCSite"] != DBNull.Value)
          managementFromDatarow.VisibleOnTPOWCSite = new bool?(EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["VisibleOnTPOWCSite"]));
        managementFromDatarow.entityType = (ExternalOriginatorEntityType) Convert.ToInt32(row["EntityType"]);
        managementFromDatarow.Address = Convert.ToString(row["Address"]);
        managementFromDatarow.City = Convert.ToString(row["City"]);
        managementFromDatarow.State = Convert.ToString(row["State"]);
        managementFromDatarow.Zip = Convert.ToString(row["Zip"]);
        managementFromDatarow.UseParentInfo = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["UseParentInfo"]);
        managementFromDatarow.Depth = Convert.ToInt32(row["Depth"]);
        managementFromDatarow.HierarchyPath = Convert.ToString(row["HierarchyPath"]);
        if (row.Table.Columns.Contains("UnderwritingType"))
          managementFromDatarow.UnderwritingType = (ExternalOriginatorUnderwritingType) Convert.ToInt32(row["UnderwritingType"]);
        if (row.Table.Columns.Contains("GenerateDisclosures"))
          managementFromDatarow.GenerateDisclosures = row["GenerateDisclosures"].Equals((object) DBNull.Value) ? ManageFeeLEDisclosures.DisableFeeManagement : (ManageFeeLEDisclosures) Convert.ToInt32(row["GenerateDisclosures"]);
        return managementFromDatarow;
      }
      ExternalOriginatorManagementData managementFromDatarow1 = new ExternalOriginatorManagementData();
      managementFromDatarow1.oid = Convert.ToInt32(row["oid"]);
      managementFromDatarow1.contactType = (ExternalOriginatorContactType) Convert.ToInt32(row["ContactType"]);
      managementFromDatarow1.Parent = Convert.ToInt32(row["Parent"]);
      managementFromDatarow1.ExternalID = Convert.ToString(row["ExternalID"]);
      managementFromDatarow1.CompanyDBAName = Convert.ToString(row["CompanyDBAName"]);
      managementFromDatarow1.CompanyLegalName = Convert.ToString(row["CompanyLegalName"]);
      managementFromDatarow1.OrganizationName = Convert.ToString(row["OrganizationName"]);
      if (row.Table.Columns.Contains("OrganizationType"))
        managementFromDatarow1.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["OrganizationType"]);
      if (row["VisibleOnTPOWCSite"] != DBNull.Value)
        managementFromDatarow1.VisibleOnTPOWCSite = new bool?(EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["VisibleOnTPOWCSite"]));
      managementFromDatarow1.entityType = (ExternalOriginatorEntityType) Convert.ToInt32(row["EntityType"]);
      managementFromDatarow1.Address = Convert.ToString(row["Address"]);
      managementFromDatarow1.City = Convert.ToString(row["City"]);
      managementFromDatarow1.State = Convert.ToString(row["State"]);
      managementFromDatarow1.Zip = Convert.ToString(row["Zip"]);
      managementFromDatarow1.UseParentInfo = (bool) row["UseParentInfo"];
      managementFromDatarow1.Depth = Convert.ToInt32(row["Depth"]);
      managementFromDatarow1.HierarchyPath = Convert.ToString(row["HierarchyPath"]);
      if (row.Table.Columns.Contains("UnderwritingType"))
        managementFromDatarow1.UnderwritingType = (ExternalOriginatorUnderwritingType) Convert.ToInt32(row["UnderwritingType"]);
      if (row.Table.Columns.Contains("GenerateDisclosures"))
        managementFromDatarow1.GenerateDisclosures = row["GenerateDisclosures"].Equals((object) DBNull.Value) ? ManageFeeLEDisclosures.DisableFeeManagement : (ManageFeeLEDisclosures) Convert.ToInt32(row["GenerateDisclosures"]);
      return managementFromDatarow1;
    }

    private static ExternalOrgManagementDataCount getExternalOrgManagementCountFromDatarow(
      DataRow row)
    {
      ExternalOrgManagementDataCount countFromDatarow = new ExternalOrgManagementDataCount();
      countFromDatarow.DataDetails = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
      if (row.Table.Columns.Contains("countChildren"))
        countFromDatarow.ImmediateChildOrgCount = Convert.ToInt32(row["countChildren"]);
      countFromDatarow.ParentOrgName = Convert.ToString(row["parentOrgName"]);
      if (row.Table.Columns.Contains("countContacts"))
        countFromDatarow.OrgContactCount = Convert.ToInt32(row["countContacts"]);
      countFromDatarow.DataDetails.PhoneNumber = Convert.ToString(row["Phone"]);
      countFromDatarow.DataDetails.FaxNumber = Convert.ToString(row["Fax"]);
      countFromDatarow.DataDetails.Email = Convert.ToString(row["Email"]);
      countFromDatarow.DataDetails.Website = Convert.ToString(row["WebUrl"]);
      countFromDatarow.DataDetails.Manager = Convert.ToString(row["ManagerUserID"]);
      countFromDatarow.DataDetails.NmlsId = Convert.ToString(row["NMLSID"]);
      if (row.Table.Columns.Contains("VisibleOnTPOWCSite") && row["VisibleOnTPOWCSite"] != DBNull.Value)
        countFromDatarow.DataDetails.VisibleOnTPOWCSite = new bool?(EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["VisibleOnTPOWCSite"]));
      if (row["LastLoanSubmittedDate"] != DBNull.Value)
        countFromDatarow.DataDetails.LastLoanSubmitted = Convert.ToDateTime(row["LastLoanSubmittedDate"]);
      if (row["DisableLogin"] != DBNull.Value)
        countFromDatarow.DataDetails.DisabledLogin = Convert.ToBoolean(row["DisableLogin"]);
      if (row["MultiFactorAuthentication"] != DBNull.Value)
        countFromDatarow.DataDetails.MultiFactorAuthentication = Convert.ToBoolean(row["MultiFactorAuthentication"]);
      countFromDatarow.DataDetails.OrgID = Convert.ToString(row["OrganizationID"]);
      countFromDatarow.DataDetails.OwnerName = Convert.ToString(row["OwnerName"]);
      return countFromDatarow;
    }

    private static ExternalOriginatorManagementData addExternalOrgDetail(
      ExternalOriginatorManagementData result,
      DataRow row)
    {
      if (row["OrganizationType"] == DBNull.Value)
        return result;
      result.OrgID = Convert.ToString(row["OrganizationID"]);
      result.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["OrganizationType"]);
      if (row["DisableLogin"] != DBNull.Value)
        result.DisabledLogin = Convert.ToBoolean(row["DisableLogin"]);
      if (row["NoAfterHourWires"] != DBNull.Value)
        result.NoAfterHourWires = Convert.ToBoolean(row["NoAfterHourWires"]);
      if (row["MultiFactorAuthentication"] != DBNull.Value)
        result.MultiFactorAuthentication = Convert.ToBoolean(row["MultiFactorAuthentication"]);
      if (row["InheritCompanyDetails"] != DBNull.Value)
        result.UseParentInfoForCompanyDetails = Convert.ToBoolean(row["InheritCompanyDetails"]);
      result.OldExternalID = Convert.ToString(row["OldExternalID"]);
      result.OwnerName = Convert.ToString(row["OwnerName"]);
      result.PhoneNumber = Convert.ToString(row["Phone"]);
      result.FaxNumber = Convert.ToString(row["Fax"]);
      result.Email = Convert.ToString(row["Email"]);
      result.Website = Convert.ToString(row["WebUrl"]);
      result.Manager = Convert.ToString(row["ManagerUserID"]);
      result.Timezone = Convert.ToString(row["Timezone"]);
      result.BillingAddress = Convert.ToString(row["BillingAddress"]);
      result.BillingCity = Convert.ToString(row["BillingCity"]);
      result.BillingState = Convert.ToString(row["BillingState"]);
      result.BillingZip = Convert.ToString(row["BillingZip"]);
      if (row["LastLoanSubmittedDate"] != DBNull.Value)
        result.LastLoanSubmitted = Convert.ToDateTime(row["LastLoanSubmittedDate"]);
      if (row["CanAcceptFirstPayments"] != DBNull.Value)
        result.CanAcceptFirstPayments = Convert.ToBoolean(row["CanAcceptFirstPayments"]);
      result.UseParentInfoForRateLock = Convert.ToBoolean(row["InheritParentRateSheet"]);
      result.EmailForRateSheet = Convert.ToString(row["RateSheetEmail"]);
      result.FaxForRateSheet = Convert.ToString(row["RateSheetFax"]);
      result.EmailForLockInfo = Convert.ToString(row["LockInfoEmail"]);
      result.FaxForLockInfo = Convert.ToString(row["LockInfoFax"]);
      result.UseParentInfoForEPPS = Convert.ToBoolean(row["InheritParentProductPricing"]);
      result.EPPSUserName = Convert.ToString(row["EPPSUserName"]);
      result.EPPSCompModel = Convert.ToString(row["EPPSCompModel"]);
      result.EPPSPriceGroup = Convert.ToString(row["EPPSPriceGroup"]);
      result.EPPSPriceGroupBroker = Convert.ToString(row["EPPSPriceGroupBroker"]);
      result.EPPSPriceGroupDelegated = Convert.ToString(row["EPPSPriceGroupDel"]);
      result.EPPSPriceGroupNonDelegated = Convert.ToString(row["EPPSPriceGroupNonDel"]);
      result.EPPSRateSheet = Convert.ToString(row["EPPSRateSheet"]);
      result.PMLUserName = row["PMLUserName"] == DBNull.Value ? "" : Convert.ToString(row["PMLUserName"]);
      result.PMLPassword = row["PMLPassword"] == DBNull.Value || !(row["PMLPassword"].ToString() != "") ? "" : Convert.ToString(row["PMLPassword"]);
      result.PMLCustomerCode = row["PMLCustomerCode"] == DBNull.Value ? "" : Convert.ToString(row["PMLCustomerCode"]);
      result.UseParentInfoForApprovalStatus = Convert.ToBoolean(row["InheritParentApprovalStatus"]);
      if (row["CurrentApprovalStatus"] != DBNull.Value)
        result.CurrentStatus = Convert.ToInt32(row["CurrentApprovalStatus"]);
      result.AddToWatchlist = Convert.ToBoolean(row["AddToApprovalWatchlist"]);
      if (row["CurrentApprovalStatusDate"] != DBNull.Value)
        result.CurrentStatusDate = Convert.ToDateTime(row["CurrentApprovalStatusDate"]);
      if (row["ApprovedDate"] != DBNull.Value)
        result.ApprovedDate = Convert.ToDateTime(row["ApprovedDate"]);
      if (row["ApplicationDate"] != DBNull.Value)
        result.ApplicationDate = Convert.ToDateTime(row["ApplicationDate"]);
      result.CompanyRating = Convert.ToInt32(row["CompanyRating"]);
      result.UseParentInfoForBusinessInfo = Convert.ToBoolean(row["InheritParentBizInformation"]);
      result.Incorporated = Convert.ToBoolean(row["Incorporated"]);
      result.StateIncorp = Convert.ToString(row["StateIncorporated"]);
      if (row["DateIncorporated"] != DBNull.Value)
        result.DateOfIncorporation = Convert.ToDateTime(row["DateIncorporated"]);
      result.TypeOfEntity = Convert.ToInt32(row["BizEntityType"]);
      result.OtherEntityDescription = Convert.ToString(row["BizOtherEntityDesc"]);
      result.TaxID = Convert.ToString(row["TaxID"]);
      result.UseSSNFormat = Convert.ToBoolean(row["UseSSNFormat"]);
      result.NmlsId = Convert.ToString(row["NMLSID"]);
      result.FinancialsPeriod = Convert.ToString(row["FinancialPeriod"]);
      if (row["FinancialLastUpdated"] != DBNull.Value)
        result.FinancialsLastUpdate = Convert.ToDateTime(row["FinancialLastUpdated"]);
      if (row["CompanyNetWorth"] != DBNull.Value)
        result.CompanyNetWorth = new Decimal?(Convert.ToDecimal(row["CompanyNetWorth"]));
      if (row["EOExpirationDate"] != DBNull.Value)
        result.EOExpirationDate = Convert.ToDateTime(row["EOExpirationDate"]);
      result.EOCompany = Convert.ToString(row["EOCompany"]);
      result.EOPolicyNumber = Convert.ToString(row["EOPolicyNumber"]);
      result.MERSOriginatingORGID = Convert.ToString(row["MERSOriginatorOrgID"]);
      result.DUSponsored = Convert.ToInt32(row["DUSponsored"]);
      result.CanFundInOwnName = Convert.ToInt32(row["CanFundInOwnName"]);
      result.CanCloseInOwnName = Convert.ToInt32(row["CanCloseInOwnName"]);
      result.PrimarySalesRepUserId = string.Concat(row["PrimarySalesRepUserId"]);
      if (row["PrimarySalesRepAssignedDate"] != DBNull.Value)
        result.PrimarySalesRepAssignedDate = (DateTime) row["PrimarySalesRepAssignedDate"];
      result.InheritWebCenterSetup = Convert.ToBoolean(row["InheritWebCenterSetup"]);
      result.InheritCustomFields = Convert.ToBoolean(row["InheritCustomFields"]);
      if (row["InheritDBANames"] != DBNull.Value)
        result.InheritDBANames = Convert.ToBoolean(row["InheritDBANames"]);
      if (row["InheritWarehouses"] != DBNull.Value)
        result.InheritWarehouses = Convert.ToBoolean(row["InheritWarehouses"]);
      if (row["InheritParentLicense"] != DBNull.Value)
        result.InheritParentLicense = Convert.ToBoolean(row["InheritParentLicense"]);
      result.IsTestAccount = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["isTestAccount"]);
      result.CommitmentUseBestEffort = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentBestEffort"]);
      result.CommitmentUseBestEffortLimited = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentBestEffortLimited"]);
      result.MaxCommitmentAuthority = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["CommitmentMaxAuthority"]);
      result.CommitmentMandatory = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentMandatory"]);
      result.MaxCommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["CommitmentMaxAmount"]);
      result.IsCommitmentDeliveryIndividual = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryIndividual"]);
      result.IsCommitmentDeliveryBulk = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryBulk"]);
      result.IsCommitmentDeliveryAOT = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryAOT"]);
      result.IsCommitmentDeliveryBulkAOT = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryBulkAOT"]);
      result.IsCommitmentDeliveryLiveTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryLiveTrade"]);
      result.IsCommitmentDeliveryCoIssue = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryCoIssue"]);
      result.IsCommitmentDeliveryForward = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryForward"]);
      result.CommitmentPolicy = (ExternalOriginatorCommitmentPolicy) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CommitmentExceedPolicy"]);
      if (row.Table.Columns.Contains("CommitmentExceedTradePolicy") && row["CommitmentExceedTradePolicy"] != DBNull.Value)
        result.CommitmentTradePolicy = (ExternalOriginatorCommitmentTradePolicy) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CommitmentExceedTradePolicy"]);
      if (row["CommitmentMessage"] != DBNull.Value)
        result.CommitmentMessage = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CommitmentMessage"]);
      result.TradeMgmtEnableTPOTradeManagementForTPOClient = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtEnableTPOTradeMgmtForTPOClient"]);
      result.TradeMgmtUseCompanyTPOTradeManagementSettings = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtUseCompanyTPOTradeMgmtSettings"]);
      result.TradeMgmtViewCorrespondentTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtViewCorrespondentTrade"]);
      result.TradeMgmtViewCorrespondentMasterCommitment = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtViewCorrespondentMasterCommitment"]);
      result.TradeMgmtLoanEligibilityToCorrespondentTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtLoanEligibilityToCorrespondentTrade"]);
      result.TradeMgmtEPPSLoanProgramEligibilityPricing = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtEPPSLoanProgramEligibilityPricing"]);
      result.TradeMgmtLoanAssignmentToCorrespondentTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtLoanAssignmentToCorrespondentTrade"]);
      result.TradeMgmtLoanDeletionFromCorrespondentTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtLoanDeletionFromCorrespondentTrade"]);
      result.TradeMgmtRequestPairOff = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtRequestPairOff"]);
      result.LPASponsored = !row.Table.Columns.Contains("LPASponsored") || row["LPASponsored"] == DBNull.Value ? 0 : Convert.ToInt32(row["LPASponsored"]);
      result.LPASponsorTPONumber = !row.Table.Columns.Contains("LPASponsorTPONumber") || row["LPASponsorTPONumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["LPASponsorTPONumber"]);
      result.LPASponsorLPAPassword = !row.Table.Columns.Contains("LPASponsorLPAPassword") || row["LPASponsorLPAPassword"] == DBNull.Value ? string.Empty : Convert.ToString(row["LPASponsorLPAPassword"]);
      result.TradeMgmtReceiveCommitmentConfirmation = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["TradeMgmtReceiveCommitmentConfirmation"]);
      result.LEINumber = !row.Table.Columns.Contains("LEINumber") || row["LEINumber"] == DBNull.Value ? string.Empty : Convert.ToString(row["LEINumber"]);
      result.BestEffortDailyVolumeLimit = !row.Table.Columns.Contains("BestEffortDailyVolumeLimit") || row["BestEffortDailyVolumeLimit"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortDailyVolumeLimit"]);
      result.BestEffortTolerencePolicy = !row.Table.Columns.Contains("BestEffortTolerencePolicy") || row["BestEffortTolerencePolicy"] == DBNull.Value ? ExternalOriginatorCommitmentTolerancePolicy.NoPolicy : (ExternalOriginatorCommitmentTolerancePolicy) Convert.ToInt32(row["BestEffortTolerencePolicy"]);
      result.BestEffortTolerancePct = !row.Table.Columns.Contains("BestEffortTolerancePct") || row["BestEffortTolerancePct"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortTolerancePct"]);
      result.BestEffortToleranceAmt = !row.Table.Columns.Contains("BestEffortToleranceAmt") || row["BestEffortToleranceAmt"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortToleranceAmt"]);
      result.BestEfforDailyLimitPolicy = !row.Table.Columns.Contains("BestEfforDailyLimitPolicy") || row["BestEfforDailyLimitPolicy"] == DBNull.Value ? ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock : (ExternalOriginatorBestEffortDailyLimitPolicy) Convert.ToInt32(row["BestEfforDailyLimitPolicy"]);
      result.DailyLimitWarningMsg = !row.Table.Columns.Contains("DailyLimitWarningMsg") || row["DailyLimitWarningMsg"] == DBNull.Value ? (string) null : Convert.ToString(row["DailyLimitWarningMsg"]);
      result.MandatoryTolerencePolicy = !row.Table.Columns.Contains("MandatoryTolerencePolicy") || row["MandatoryTolerencePolicy"] == DBNull.Value ? ExternalOriginatorCommitmentTolerancePolicy.NoPolicy : (ExternalOriginatorCommitmentTolerancePolicy) Convert.ToInt32(row["MandatoryTolerencePolicy"]);
      if (row.Table.Columns.Contains("MandatoryTolerancePct") && row["MandatoryTolerancePct"] != DBNull.Value)
        result.MandatoryTolerancePct = Convert.ToDecimal(row["MandatoryTolerancePct"]);
      else
        result.BestEffortTolerancePct = 0M;
      result.MandatoryToleranceAmt = !row.Table.Columns.Contains("MandatoryToleranceAmt") || row["MandatoryToleranceAmt"] == DBNull.Value ? 0M : Convert.ToDecimal(row["MandatoryToleranceAmt"]);
      if (row.Table.Columns.Contains("NoAfterHourWires") && row["NoAfterHourWires"] != DBNull.Value)
        result.NoAfterHourWires = Convert.ToBoolean(row["NoAfterHourWires"]);
      if (row["GlobalOrSpecificTPO"] != DBNull.Value)
        result.GlobalOrSpecificTPO = Convert.ToBoolean(row["GlobalOrSpecificTPO"]);
      if (row.Table.Columns.Contains("ResetLimitForRatesheetId") && row["ResetLimitForRatesheetId"] != DBNull.Value)
        result.ResetLimitForRatesheetId = Convert.ToBoolean(row["ResetLimitForRatesheetId"]);
      return result;
    }

    public static int ImportExternalContact(
      ExternalOriginatorManagementData newExternalContact)
    {
      int num = ExternalOrgManagementAccessor.AddExternalContacts(false, newExternalContact, (LoanCompHistoryList) null);
      if (num != -1)
      {
        bool flag = true;
        if (newExternalContact.Parent > 0)
        {
          IEnumerable<UserInfo> salesRepUserInfos = ExternalOrgManagementAccessor.GetExternalOrganizationPrimarySalesRepUserInfos(newExternalContact.Parent);
          if (salesRepUserInfos != null)
          {
            List<UserInfo> list = salesRepUserInfos.ToList<UserInfo>();
            if (list != null && list.FindIndex((Predicate<UserInfo>) (x => x.Userid == newExternalContact.PrimarySalesRepUserId)) > -1)
              flag = false;
          }
        }
        if (flag)
          ExternalOrgManagementAccessor.AddExternalOrganizationSalesReps(new ExternalOrgSalesRep[1]
          {
            new ExternalOrgSalesRep(0, num, newExternalContact.PrimarySalesRepUserId, "", "", "", "", "", "")
          });
        if (newExternalContact.Parent > 0)
        {
          List<ExternalOrgURL> selectedOrgUrls = ExternalOrgManagementAccessor.GetSelectedOrgUrls(newExternalContact.Parent);
          if (selectedOrgUrls != null)
            ExternalOrgManagementAccessor.UpdateExternalOrganizationSelectedURLs(num, selectedOrgUrls, newExternalContact.Parent);
        }
      }
      return num;
    }

    public static int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact)
    {
      return ExternalOrgManagementAccessor.AddExternalContacts(forLender, newExternalContact, (LoanCompHistoryList) null);
    }

    public static string addExternalOrgToInternalOrgTableQuery(
      ExternalOriginatorManagementData newExternalContact)
    {
      return "INSERT INTO org_chart (parent,org_name,description,company_name,address1,city,state,zip,phone,fax,nmls_code,depth,org_type) VALUES (" + (object) ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(newExternalContact.Parent, false) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrganizationName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrganizationName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrganizationName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Address) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.City) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.State) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Zip) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PhoneNumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FaxNumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.NmlsId) + "," + (object) newExternalContact.Depth + ",'External')";
    }

    private static int addExternalOrgToInternalOrgTable(
      ExternalOriginatorManagementData newExternalContact)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.addExternalOrgToInternalOrgTable: Creating SQL commands for table org_chart.");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.addExternalOrgToInternalOrgTableQuery(newExternalContact));
      dbQueryBuilder.SelectIdentity("@oid");
      dbQueryBuilder.Select("@oid");
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
      return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
    }

    public static void updateInternalOrgFromExternalOrgTable(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE org_chart SET [org_name] = extOrg.OrganizationName, [description] = extOrg.OrganizationName, [company_name] = extOrg.OrganizationName, [address1] = extOrg.Address, [city] = extOrg.City, [state] = extOrg.State, [zip] = extOrg.Zip, [phone] = orgDetails.Phone, [fax] = orgDetails.Fax, [nmls_code] = orgDetails.NMLSID, [depth] = extOrg.Depth  FROM org_chart org inner join ExternalOriginatorManagement extOrg  ON  extOrg.org_chart_id = org.oid  inner join ExternalOrgDetail orgDetails  ON orgDetails.externalOrgID = extOrg.oid  WHERE extOrg.oid = " + (object) oid;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("updateInternalOrgFromExternalOrgTable: Cannot update record in org_chart table.\r\n" + ex.Message);
      }
    }

    public static int GetOrgIdForExternalOrgID(int externalOrgId)
    {
      return ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(externalOrgId, false);
    }

    private static int getOrgChartIdForExternalOid(int ExternalOID, bool excludeRoot)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT org_chart_id FROM [ExternalOriginatorManagement] WHERE [oid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalOID));
      try
      {
        object obj1 = dbQueryBuilder.ExecuteScalar();
        if (obj1 != null)
          return Convert.ToInt32(obj1);
        if (excludeRoot)
          return -1;
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT oid FROM org_chart WHERE org_name = 'Third Party Originators' AND org_type = 'External'");
        object obj2 = dbQueryBuilder.ExecuteScalar();
        return obj2 != null ? Convert.ToInt32(obj2) : 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("getOrgChartIdForExternalOid: Cannot fetch id records from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static string InsertExternalOriginatorManagementQuery(
      ExternalOriginatorManagementData newExternalContact)
    {
      return "INSERT INTO ExternalOriginatorManagement ([ContactType], [Parent], [ExternalID],[OrganizationName], [CompanyDBAName], [CompanyLegalName], [EntityType], [Address], [City], [State], [Zip], [UseParentInfo], [Depth], [HierarchyPath], [VisibleOnTPOWCSite], [UnderwritingType], [GenerateDisclosures], [org_chart_id]) VALUES (" + (object) (newExternalContact.contactType == ExternalOriginatorContactType.TPO ? 1 : (newExternalContact.contactType == ExternalOriginatorContactType.BusinessContacts ? 2 : (newExternalContact.contactType == ExternalOriginatorContactType.FreeEntry ? 3 : 0))) + "," + (object) newExternalContact.Parent + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.ExternalID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrganizationName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyDBAName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyLegalName) + "," + (object) (newExternalContact.entityType == ExternalOriginatorEntityType.Broker ? 1 : (newExternalContact.entityType == ExternalOriginatorEntityType.Correspondent ? 2 : (newExternalContact.entityType == ExternalOriginatorEntityType.Both ? 3 : 0))) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Address) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.City) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.State) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Zip) + "," + (newExternalContact.UseParentInfo ? (object) "'True'" : (object) "'False'") + "," + (object) newExternalContact.Depth + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.HierarchyPath) + "," + (newExternalContact.OrganizationType == ExternalOriginatorOrgType.Company ? (object) "null" : (EllieMae.EMLite.DataAccess.SQL.DecodeBoolean((object) newExternalContact.VisibleOnTPOWCSite) ? (object) "'True'" : (object) "'False'")) + "," + (object) (newExternalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Delegated ? 1 : (newExternalContact.UnderwritingType == ExternalOriginatorUnderwritingType.NonDelegated ? 2 : (newExternalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Both ? 3 : 0))) + "," + (object) (newExternalContact.GenerateDisclosures == ManageFeeLEDisclosures.RequestLEAndDisclosures ? 1 : (newExternalContact.GenerateDisclosures == ManageFeeLEDisclosures.GenerateLE ? 2 : (newExternalContact.GenerateDisclosures == ManageFeeLEDisclosures.GenerateLEAndDisclosures ? 3 : 0))) + ",@orgChartId)";
    }

    public static int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      LoanCompHistoryList loanCompHistoryList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@orgChartId", "int");
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalContacts: Creating SQL commands for table " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + ".");
      if (!forLender)
      {
        int internalOrgTable = ExternalOrgManagementAccessor.addExternalOrgToInternalOrgTable(newExternalContact);
        dbQueryBuilder.SelectVar("@orgChartId", (object) internalOrgTable);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExternalOriginatorManagementQuery(newExternalContact));
      }
      else
        dbQueryBuilder.AppendLine("INSERT INTO ExternalLenders ([ContactType], [Parent], [ExternalID],[OrganizationName], [CompanyDBAName], [CompanyLegalName], [EntityType], [Address], [City], [State], [Zip], [UseParentInfo], [Depth], [HierarchyPath]) VALUES (" + (object) (newExternalContact.contactType == ExternalOriginatorContactType.TPO ? 1 : (newExternalContact.contactType == ExternalOriginatorContactType.BusinessContacts ? 2 : (newExternalContact.contactType == ExternalOriginatorContactType.FreeEntry ? 3 : 0))) + "," + (object) newExternalContact.Parent + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.ExternalID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrganizationName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyDBAName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyLegalName) + "," + (object) (newExternalContact.entityType == ExternalOriginatorEntityType.Broker ? 1 : (newExternalContact.entityType == ExternalOriginatorEntityType.Correspondent ? 2 : (newExternalContact.entityType == ExternalOriginatorEntityType.Both ? 3 : 0))) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Address) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.City) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.State) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Zip) + "," + (newExternalContact.UseParentInfo ? (object) "'True'" : (object) "'False'") + "," + (object) newExternalContact.Depth + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.HierarchyPath) + ")");
      dbQueryBuilder.SelectIdentity("@orgChartId");
      dbQueryBuilder.Select("@orgChartId");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        int orgId = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        if (!forLender)
          ExternalOrgManagementAccessor.insertOrgDetails(newExternalContact, orgId);
        newExternalContact.oid = orgId;
        if (loanCompHistoryList != null)
          LOCompAccessor.CreateHistoryCompPlans(loanCompHistoryList, orgId, (string) null, true, forLender);
        if (newExternalContact.OrganizationType == ExternalOriginatorOrgType.Company && !forLender)
        {
          ExternalOrgManagementAccessor.AssignAllDefaultDocumentsToTOPOrg(newExternalContact.oid);
          ExternalOrgManagementAccessor.SetDefaultFeeManagementListByChannel(newExternalContact.oid, newExternalContact.entityType);
        }
        if (!forLender)
        {
          ExternalOrgManagementAccessor.UpdateInheritDBANameSetting(newExternalContact.oid, newExternalContact.OrganizationType != 0);
          ExternalOrgManagementAccessor.UpdateInheritWarehouses(newExternalContact.oid, newExternalContact.OrganizationType != 0);
          ExternalOrgManagementAccessor.saveLoanTypeForOrg(newExternalContact);
        }
        return orgId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalContacts: Cannot update " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table due to the following problem:\r\n" + ex.Message);
      }
    }

    public static int CreateExternalOrganization(
      Dictionary<string, object> newExternalContactDictionary)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.CreateExternalOrganizationQuery(newExternalContactDictionary, cmdParams));
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        DataSet dataSet = new EllieMae.EMLite.Server.DbAccessManager().ExecuteSetQuery((IDbCommand) sqlCmd);
        return dataSet != null && dataSet.Tables.Count > 0 ? Utils.ParseInt((object) dataSet.Tables[0].Rows[0]["OrganizationId"].ToString()) : -1;
      }
    }

    public static int CreateExternalOrganization(
      Dictionary<string, object> newExternalContactDictionary,
      out ExternalOrgWrapper externalOrgWrapper)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.CreateExternalOrganizationQuery(newExternalContactDictionary, cmdParams));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.GetExternalOrganizationQuery());
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        DataSet ds = new EllieMae.EMLite.Server.DbAccessManager().ExecuteSetQuery((IDbCommand) sqlCmd);
        if (ds != null && ds.Tables.Count > 0)
        {
          int orgId = Utils.ParseInt((object) ds.Tables[0].Rows[0]["OrganizationId"].ToString());
          ds.Tables.RemoveAt(0);
          externalOrgWrapper = ExternalOrgManagementAccessor.GatherExternalOrgData(ds, orgId);
          return orgId;
        }
        externalOrgWrapper = (ExternalOrgWrapper) null;
        return -1;
      }
    }

    private static string CreateExternalOrganizationQuery(
      Dictionary<string, object> newExternalContactDictionary,
      List<SqlParameter> cmdParams)
    {
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      ExternalOriginatorManagementData newExternalContact1 = newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.BasicInfo.ToString()) ? newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.BasicInfo.ToString()] as ExternalOriginatorManagementData : throw new Exception("BasicInfo is required.");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.Declare("@orgChartId", "int");
      dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.addExternalOrgToInternalOrgTableQuery(newExternalContact1));
      dbQueryBuilder1.SelectIdentity("@orgChartId");
      dbQueryBuilder1.Declare("@oid", "int");
      dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertExternalOriginatorManagementQuery(newExternalContact1));
      dbQueryBuilder1.SelectIdentity("@oid");
      dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.insertOrgDetailsQuery(newExternalContact1));
      dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdatePMLPasswordQuery(newExternalContact1.PMLPassword));
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoComp.ToString()) && newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoComp.ToString()] != null)
      {
        foreach (LoanCompHistory plan in newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoComp.ToString()] as List<LoanCompHistory>)
          dbQueryBuilder1.AppendLine(LOCompAccessor.InsertLoCompHistoryQuery("Ext_CompPlans", "[oid]", (string) null, plan));
      }
      if (newExternalContact1.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.AssignAllDefaultDocumentsToTOPOrgQuery());
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.SetDefaultFeeManagementListByChannelQuery(newExternalContact1.entityType));
      }
      if (newExternalContact1.OrganizationType != ExternalOriginatorOrgType.Company && newExternalContact1.InheritDBANames)
      {
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateInheritDBANameSettingQuery(newExternalContact1.InheritDBANames));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InheritDBANameQuery());
      }
      else if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba.ToString()))
      {
        int sortIndex = !(newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba.ToString()] is List<ExternalOrgDBAName> newExternalContact2) || newExternalContact2.Count <= 0 ? 0 : newExternalContact2[0].SortIndex;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertDbaQuery((IEnumerable<ExternalOrgDBAName>) newExternalContact2, sortIndex, false));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse.ToString()))
      {
        if (newExternalContact1.OrganizationType != ExternalOriginatorOrgType.Company)
        {
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateInheritWarehousesSettingQuery(newExternalContact1.InheritWarehouses));
          if (newExternalContact1.InheritWarehouses)
            dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InheritWarehousesQuery());
        }
        if (!newExternalContact1.InheritWarehouses && newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse.ToString()] is List<ExternalOrgWarehouse> newExternalContact3 && newExternalContact3.Any<ExternalOrgWarehouse>())
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpsertExtOrgWarehousesQuery((IEnumerable<ExternalOrgWarehouse>) newExternalContact3, cmdParams));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.SalesRepAe.ToString()))
      {
        foreach (ExternalOrgSalesRep newRep in newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.SalesRepAe.ToString()] as List<ExternalOrgSalesRep>)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.AddExternalOrganizationSalesRepQuery(newRep));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Notes.ToString()))
      {
        List<ExternalOrgNote> newExternalContact4 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Notes.ToString()] as List<ExternalOrgNote>;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.AddExternalOrganizationNotesQuery(newExternalContact4));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpocSetup.ToString()))
      {
        List<ExternalOrgURL> newExternalContact5 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpocSetup.ToString()] as List<ExternalOrgURL>;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrganizationSelectedURLsQuery(newExternalContact5));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpoContacts.ToString()))
      {
        foreach (ExternalOrgContact tpoContact in newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpoContacts.ToString()] as List<ExternalOrgContact>)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.AddExternalOrgTpoContactQuery(tpoContact));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.LenderContacts.ToString()))
      {
        List<ExternalOrgLenderContact> newExternalContact6 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.LenderContacts.ToString()] as List<ExternalOrgLenderContact>;
        dbQueryBuilder1.Declare("@ContactId", "int");
        foreach (ExternalOrgLenderContact contact in newExternalContact6)
        {
          string str = ExternalOrgManagementAccessor.GetRootParentOrgIDforOrg(newExternalContact1.Parent).ToString();
          if (contact.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts)
          {
            dbQueryBuilder1.AppendLine(TPOCompanyContactsAccessor.AddLenderContactQuery(contact, newExternalContact1.Parent == 0 ? (string) null : str));
            dbQueryBuilder1.SelectIdentity("@ContactId");
            dbQueryBuilder1.AppendLine(TPOCompanyContactsAccessor.AddTPOCompanyLenderContactQuery(contact.Source, contact.isHidden ? 1 : 0, contact.DisplayOrder, newExternalContact1.Parent == 0 ? (string) null : str));
          }
          else if (contact.Source == ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps && contact.isHidden)
          {
            dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.GetSalesRepIdByUserIdQuery(contact.UserID, newExternalContact1.Parent == 0 ? (string) null : str));
            dbQueryBuilder1.AppendLine(TPOCompanyContactsAccessor.AddTPOCompanyLenderContactQuery(contact.Source, contact.isHidden ? 1 : 0, contact.DisplayOrder, newExternalContact1.Parent == 0 ? (string) null : str));
          }
        }
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.TradeManagement.ToString()))
      {
        ExternalOriginatorManagementData newExternalContact7 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.TradeManagement.ToString()] as ExternalOriginatorManagementData;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateTradeManagementSettingsForExternalOrgQuery(newExternalContact7));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.CustomFields.ToString()))
      {
        ExternalOrgCustomFields newExternalContact8 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.CustomFields.ToString()] as ExternalOrgCustomFields;
        bool? useParentInfo = newExternalContact8.UseParentInfo;
        int num;
        if (useParentInfo.HasValue)
        {
          useParentInfo = newExternalContact8.UseParentInfo;
          if (useParentInfo.Value)
          {
            num = 1;
            goto label_52;
          }
        }
        num = 0;
label_52:
        bool inheritCustomFields = num != 0;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateInheritCustomFieldsFlagQuery(inheritCustomFields));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertCustomFieldValuesQuery(newExternalContact8.CustomFields.ToArray()));
      }
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.License.ToString()) && newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.License.ToString()] != null)
      {
        BranchExtLicensing newExternalContact9 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.License.ToString()] as BranchExtLicensing;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateExtOrgLicenseQuery(newExternalContact9));
        if (newExternalContact9.StateLicenseExtTypes.Any<StateLicenseExtType>())
        {
          foreach (StateLicenseExtType stateLicenseExtType in newExternalContact9.StateLicenseExtTypes)
            dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertExtOrgLicensingQuery(stateLicenseExtType));
        }
      }
      if (!newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoanCriteria.ToString()))
      {
        ExternalOrgLoanTypes loanTypes = new ExternalOrgLoanTypes();
        loanTypes.Broker = loanTypes.CorrespondentDelegated = loanTypes.CorrespondentNonDelegated = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType()
        {
          LoanTypes = 0,
          LoanPurpose = 0,
          AllowLoansWithIssues = 2
        };
        loanTypes.Underwriting = 1;
        loanTypes.UseParentInfoFhaVa = false;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrgLoanTypesQuery(loanTypes, loanTypes.UseParentInfoFhaVa));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(0, loanTypes.Broker));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(1, loanTypes.CorrespondentDelegated));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(2, loanTypes.CorrespondentNonDelegated));
      }
      else
      {
        ExternalOrgLoanTypes newExternalContact10 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoanCriteria.ToString()] as ExternalOrgLoanTypes;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrgLoanTypesQuery(newExternalContact10, newExternalContact10.UseParentInfoFhaVa));
        if (newExternalContact10.Broker != null)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(0, newExternalContact10.Broker));
        if (newExternalContact10.CorrespondentDelegated != null)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(1, newExternalContact10.CorrespondentDelegated));
        if (newExternalContact10.CorrespondentNonDelegated != null)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(2, newExternalContact10.CorrespondentNonDelegated));
      }
      ExternalOrgManagementAccessor.ExternalOrganizationEntities organizationEntities;
      if (newExternalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Fees.ToString()) && newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Fees.ToString()] != null)
      {
        Dictionary<string, object> newExternalContact11 = newExternalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Fees.ToString()] as Dictionary<string, object>;
        Dictionary<string, object> dictionary1 = newExternalContact11;
        organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.FeeDetails;
        string key1 = organizationEntities.ToString();
        if (dictionary1.ContainsKey(key1))
        {
          Dictionary<string, object> dictionary2 = newExternalContact11;
          organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.FeeDetails;
          string key2 = organizationEntities.ToString();
          foreach (ExternalFeeManagement feeManagement in dictionary2[key2] as List<ExternalFeeManagement>)
            dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertFeeManagementSettingsQuery(feeManagement));
        }
        Dictionary<string, object> dictionary3 = newExternalContact11;
        organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.LateFeeSettings;
        string key3 = organizationEntities.ToString();
        if (dictionary3.ContainsKey(key3))
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
          Dictionary<string, object> dictionary4 = newExternalContact11;
          organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.LateFeeSettings;
          string key4 = organizationEntities.ToString();
          string text = ExternalOrgManagementAccessor.InsertLateFeeSettingsQuery(dictionary4[key4] as ExternalLateFeeSettings);
          dbQueryBuilder2.AppendLine(text);
        }
      }
      Dictionary<string, object> dictionary5 = newExternalContactDictionary;
      organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.Commitments;
      string key5 = organizationEntities.ToString();
      if (dictionary5.ContainsKey(key5))
      {
        Dictionary<string, object> dictionary6 = newExternalContactDictionary;
        organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.Commitments;
        string key6 = organizationEntities.ToString();
        ExternalOriginatorManagementData externalContact = dictionary6[key6] as ExternalOriginatorManagementData;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateCommitmentsSettingsForExternalOrgQuery(externalContact));
      }
      Dictionary<string, object> dictionary7 = newExternalContactDictionary;
      organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.Onrp;
      string key7 = organizationEntities.ToString();
      if (dictionary7.ContainsKey(key7))
      {
        Dictionary<string, object> dictionary8 = newExternalContactDictionary;
        organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.Onrp;
        string key8 = organizationEntities.ToString();
        if (dictionary8[key8] != null)
        {
          Dictionary<string, object> dictionary9 = newExternalContactDictionary;
          organizationEntities = ExternalOrgManagementAccessor.ExternalOrganizationEntities.Onrp;
          string key9 = organizationEntities.ToString();
          ExternalOrgOnrpSettings onrpSettings = dictionary9[key9] as ExternalOrgOnrpSettings;
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InsertOnrpSettingsQuery(onrpSettings));
        }
      }
      dbQueryBuilder1.AppendLine("select @oid as OrganizationId");
      return dbQueryBuilder1.ToString();
    }

    public static void UpdateExternalOrganization(
      Dictionary<string, object> externalContactDictionary,
      out ExternalOrgWrapper externalOrgWrapper,
      bool returnOrgDetails)
    {
      externalOrgWrapper = (ExternalOrgWrapper) null;
      ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
      ExternalOriginatorManagementData externalContact1 = externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.BasicInfo.ToString()) ? externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.BasicInfo.ToString()] as ExternalOriginatorManagementData : throw new Exception("BasicInfo is required.");
      ExternalOrgManagementAccessor.UpdateExternalContact(false, externalContact1, (LoanCompHistoryList) null);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      dbQueryBuilder1.Declare("@oid", "int");
      dbQueryBuilder1.AppendLine("set @oid = " + (object) externalContact1.oid);
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba.ToString()))
      {
        List<ExternalOrgDBAName> externalContact2 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba.ToString()] as List<ExternalOrgDBAName>;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateInheritDBANameSettingQuery(externalContact1.InheritDBANames));
        if (externalContact1.InheritDBANames)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InheritDBANameQuery());
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpsertExtOrgDbasQuery(externalContact1.oid, (IEnumerable<ExternalOrgDBAName>) externalContact2, cmdParams, useParentInfo: externalContact1.InheritDBANames));
      }
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse.ToString()))
      {
        List<ExternalOrgWarehouse> externalContact3 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse.ToString()] as List<ExternalOrgWarehouse>;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateInheritWarehousesSettingQuery(externalContact1.InheritWarehouses));
        if (externalContact1.InheritWarehouses)
        {
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InheritWarehousesQuery());
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.SyncInheritChildWarehouseQuery());
        }
        else if (externalContact3 != null && externalContact3.Any<ExternalOrgWarehouse>())
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpsertExtOrgWarehousesQuery((IEnumerable<ExternalOrgWarehouse>) externalContact3, cmdParams));
      }
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.Commitments.ToString()))
      {
        ExternalOriginatorManagementData externalContact4 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.Commitments.ToString()] as ExternalOriginatorManagementData;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateCommitmentsSettingsForExternalOrgQuery(externalContact4));
      }
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoanCriteria.ToString()))
      {
        ExternalOrgLoanTypes externalContact5 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.LoanCriteria.ToString()] as ExternalOrgLoanTypes;
        if (externalContact5.UseParentInfoFhaVa)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.SyncInheritChildLoanChannelQuery());
        else
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateExternalOrganizationLoanTypesQuery(externalContact1.oid, externalContact5));
      }
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.CustomFields.ToString()))
      {
        ExternalOrgCustomFields externalContact6 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.CustomFields.ToString()] as ExternalOrgCustomFields;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
        bool? useParentInfo = externalContact6.UseParentInfo;
        string text1 = ExternalOrgManagementAccessor.DeleteCustomFieldsQuery(useParentInfo.GetValueOrDefault());
        dbQueryBuilder2.AppendLine(text1);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = dbQueryBuilder1;
        useParentInfo = externalContact6.UseParentInfo;
        string text2 = ExternalOrgManagementAccessor.UpdateInheritCustomFieldsFlagQuery(useParentInfo.GetValueOrDefault());
        dbQueryBuilder3.AppendLine(text2);
        useParentInfo = externalContact6.UseParentInfo;
        if (useParentInfo.GetValueOrDefault())
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.InheritCustomFieldsQuery());
        List<TpoCustomFields> fields = externalContact6.Fields;
        if ((fields != null ? (fields.Any<TpoCustomFields>() ? 1 : 0) : 0) != 0)
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateExternalOrganizationCustomFields(externalContact1.oid, externalContact6, cmdParams));
      }
      if (externalContactDictionary.ContainsKey(ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpocSetup.ToString()))
      {
        List<ExternalOrgURL> externalContact7 = externalContactDictionary[ExternalOrgManagementAccessor.ExternalOrganizationEntities.TpocSetup.ToString()] as List<ExternalOrgURL>;
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpdateIsTestAccountQuery(externalContact1.IsTestAccount));
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.UpsertExternalOrgURLsQuery(externalContact1.oid, (IEnumerable<ExternalOrgURL>) externalContact7, cmdParams, new bool?(false), new bool?(true)));
      }
      if (returnOrgDetails)
        dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.GetExternalOrganizationQuery());
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder1.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        if (returnOrgDetails)
        {
          DataSet ds = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd);
          externalOrgWrapper = ExternalOrgManagementAccessor.GatherExternalOrgData(ds, externalContact1.oid);
        }
        else
          dbAccessManager.ExecuteNonQuery((IDbCommand) sqlCmd);
      }
    }

    private static void saveLoanTypeForOrg(
      ExternalOriginatorManagementData newExternalContact)
    {
      if (newExternalContact.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        ExternalOrgLoanTypes loanTypes = new ExternalOrgLoanTypes();
        loanTypes.Broker = loanTypes.CorrespondentDelegated = loanTypes.CorrespondentNonDelegated = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType()
        {
          LoanTypes = 0,
          LoanPurpose = 0,
          AllowLoansWithIssues = 2
        };
        loanTypes.Underwriting = 1;
        ExternalOrgManagementAccessor.UpdateExternalOrganizationLoanTypes(newExternalContact.oid, loanTypes);
      }
      else
      {
        ExternalOrgLoanTypes organizationLoanTypes = ExternalOrgManagementAccessor.GetExternalOrganizationLoanTypes(newExternalContact.Parent);
        organizationLoanTypes.UseParentInfoFhaVa = true;
        ExternalOrgManagementAccessor.UpdateExternalOrganizationLoanTypes(newExternalContact.oid, organizationLoanTypes);
      }
    }

    public static void UpdateExternalContact(
      bool forLender,
      ExternalOriginatorManagementData externalContact)
    {
      ExternalOrgManagementAccessor.UpdateExternalContact(forLender, externalContact, (LoanCompHistoryList) null);
    }

    private static string UpdateTradeManagementSettingsForExternalOrgQuery(
      ExternalOriginatorManagementData externalContact)
    {
      return "UPDATE ExternalOrgDetail SET [TradeMgmtEnableTPOTradeMgmtForTPOClient] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtEnableTPOTradeManagementForTPOClient) + ",[TradeMgmtUseCompanyTPOTradeMgmtSettings] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtUseCompanyTPOTradeManagementSettings) + ",[TradeMgmtViewCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtViewCorrespondentTrade) + ",[TradeMgmtViewCorrespondentMasterCommitment] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtViewCorrespondentMasterCommitment) + ",[TradeMgmtLoanEligibilityToCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanEligibilityToCorrespondentTrade) + ",[TradeMgmtEPPSLoanProgramEligibilityPricing] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtEPPSLoanProgramEligibilityPricing) + ",[TradeMgmtLoanAssignmentToCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanAssignmentToCorrespondentTrade) + ",[TradeMgmtLoanDeletionFromCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanDeletionFromCorrespondentTrade) + ",[TradeMgmtRequestPairOff] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtRequestPairOff) + ",[TradeMgmtReceiveCommitmentConfirmation] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtReceiveCommitmentConfirmation) + " WHERE externalOrgID = @oid";
    }

    public static void UpdateExternalContact(
      bool forLender,
      ExternalOriginatorManagementData externalContact,
      LoanCompHistoryList loanCompHistoryList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (!forLender)
          ExternalOrgManagementAccessor.updateOrgDetails(externalContact);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalContact: Cannot update record in ExternalOrgDetail table.\r\n" + ex.Message);
      }
      try
      {
        string str1 = "UPDATE " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " SET [parent] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Parent) + ",[ExternalID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.ExternalID) + ",[OrganizationName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OrganizationName) + ",[CompanyDBAName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyDBAName) + ",[CompanyLegalName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyLegalName) + ",";
        if (!forLender)
          str1 = str1 + "[UnderwritingType] = " + (object) (int) externalContact.UnderwritingType + "," + "[GenerateDisclosures] = " + (object) (int) externalContact.GenerateDisclosures + ",";
        string str2 = Convert.ToString(EllieMae.EMLite.DataAccess.SQL.DecodeBoolean((object) externalContact.VisibleOnTPOWCSite) ? 1 : 0);
        if (externalContact.OrganizationType == ExternalOriginatorOrgType.Company)
          str2 = "null";
        string text1 = str1 + "[entityType] = " + (object) (int) externalContact.entityType + ",[Address] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Address) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.City) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.State) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Zip) + ",[UseParentInfo] = " + (object) (externalContact.UseParentInfo ? 1 : 0) + ",[Depth] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Depth) + ",[HierarchyPath] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.HierarchyPath) + ",[VisibleOnTPOWCSite] = " + str2 + " WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.oid);
        dbQueryBuilder1.AppendLine(text1);
        dbQueryBuilder1.ExecuteNonQuery();
        if (!forLender)
          ExternalOrgManagementAccessor.updateInternalOrgFromExternalOrgTable(externalContact.oid);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.AppendLine("SELECT  [descendent],[InheritCompanyDetails] FROM [ExternalOrgDescendents] extOrgdesc INNER JOIN [ExternalOrgDetail] extOrgDet on extOrgDet.externalOrgID = extOrgdesc.descendent where extOrgdesc.oid = " + (object) externalContact.oid + " AND extOrgDet.InheritCompanyDetails = 1 AND (0 not in ((SELECT [InheritCompanyDetails] FROM [ExternalOrgDetail] extOrgDet1 INNER JOIN [ExternalOrgDescendents] extOrgdesc1 ON extOrgDet1.externalOrgID = extOrgdesc1.oid  where extOrgdesc1.descendent = extOrgdesc.descendent and oid > " + (object) externalContact.oid + ") UNION (SELECT [InheritCompanyDetails] FROM [ExternalOrgDetail] extOrgDet2  INNER JOIN [ExternalOriginatorManagement] eom on eom.oid = extOrgDet2.externalOrgID  Where externalOrgID = extOrgdesc.descendent and [InheritCompanyDetails] = 1 and eom.parent = " + (object) externalContact.oid + "))) ");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder2.Execute())
        {
          if (Convert.ToBoolean(dataRow["InheritCompanyDetails"]))
          {
            dbQueryBuilder2.Reset();
            string str3 = "UPDATE " + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + " SET [CompanyDBAName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyDBAName) + ",[CompanyLegalName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyLegalName) + ",[entityType] = " + (object) (int) externalContact.entityType + ",";
            if (!forLender)
              str3 = str3 + "[UnderwritingType] = " + (object) (int) externalContact.UnderwritingType + "," + "[GenerateDisclosures] = " + (object) (int) externalContact.GenerateDisclosures + ",";
            string text2 = str3 + "[Address] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Address) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.City) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.State) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Zip) + " WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) Convert.ToInt32(dataRow["descendent"]));
            dbQueryBuilder2.AppendLine(text2);
            dbQueryBuilder2.ExecuteNonQuery();
            if (!forLender)
              ExternalOrgManagementAccessor.updateInternalOrgFromExternalOrgTable(Convert.ToInt32(dataRow["descendent"]));
          }
          dbQueryBuilder2.Reset();
          string text3 = "Update ExternalUsers SET [Address1] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Address) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.City) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.State) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Zip) + " Where [externalOrgID] =  " + EllieMae.EMLite.DataAccess.SQL.Encode((object) Convert.ToInt32(dataRow["descendent"])) + " and [UseCompanyAddress] = 1";
          dbQueryBuilder2.AppendLine(text3);
          dbQueryBuilder2.ExecuteNonQuery();
        }
        dbQueryBuilder2.Reset();
        string text4 = "Update ExternalUsers SET [Address1] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Address) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.City) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.State) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Zip) + " Where [externalOrgID] =  " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.oid) + " and [UseCompanyAddress] = 1";
        dbQueryBuilder2.AppendLine(text4);
        dbQueryBuilder2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalContact: Cannot update record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
      try
      {
        if (loanCompHistoryList == null)
          return;
        LOCompAccessor.CreateHistoryCompPlansForOrg(loanCompHistoryList, externalContact.oid, true, forLender);
        ExternalOrgManagementAccessor.updateExternalChildrenPlans(loanCompHistoryList, externalContact.oid, false, forLender);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalContact: Cannot create comp plans for " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + ".\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalOrgLOCompPlans(
      bool forLender,
      int oid,
      LoanCompHistoryList loanCompHistoryList)
    {
      try
      {
        LOCompAccessor.CreateHistoryCompPlansForOrg(loanCompHistoryList, oid, true, forLender);
        ExternalOrgManagementAccessor.updateExternalChildrenPlans(loanCompHistoryList, oid, loanCompHistoryList.UncheckParentInfo, forLender);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgLOCompPlans: Cannot create comp plans for " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + ".\r\n" + ex.Message);
      }
    }

    private static string insertOrgDetailsQuery(
      ExternalOriginatorManagementData newExternalContact)
    {
      bool autoAssignOrgId = newExternalContact.AutoAssignOrgID;
      string str = "UPDATE [company_settings] WITH (ROWLOCK) SET @orgid = [value] = CAST(CAST([value] AS bigint) + 1 AS nvarchar(25)) WHERE category = 'POLICIES' and attribute = 'NextOrgIdNumber'";
      return "DECLARE @orgid nvarchar(MAX) = " + (autoAssignOrgId ? (object) ("NULL;" + str) : (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OrgID)) + ";INSERT INTO ExternalOrgDetail ([externalOrgID],[OrganizationID],[OrganizationType],[DisableLogin],[MultiFactorAuthentication],[InheritCompanyDetails],[OldExternalID],[OwnerName],[Phone],[Fax],[Email],[WebUrl],[ManagerUserID],[LastLoanSubmittedDate],[InheritParentRateSheet],[RateSheetEmail],[RateSheetFax],[LockInfoEmail],[LockInfoFax],[InheritParentProductPricing],[EPPSUserName],[EPPSCompModel],[EPPSPriceGroup],[EPPSPriceGroupBroker],[EPPSPriceGroupDel],[EPPSPriceGroupNonDel],[EPPSRateSheet],[PMLUserName],[PMLCustomerCode],[InheritParentApprovalStatus],[CurrentApprovalStatus],[AddToApprovalWatchlist],[CurrentApprovalStatusDate],[ApprovedDate],[ApplicationDate],[CompanyRating],[InheritParentBizInformation],[Incorporated],[StateIncorporated],[DateIncorporated],[BizEntityType],[BizOtherEntityDesc],[TaxID],[UseSSNFormat],[NMLSID],[FinancialPeriod],[FinancialLastUpdated],[CompanyNetWorth],[EOExpirationDate],[EOCompany],[EOPolicyNumber],[MERSOriginatorOrgID],[DUSponsored],[CanFundInOwnName],[CanCloseInOwnName],[InheritParentLicense],[PrimarySalesRepUserId],[CommitmentBestEffort],[CommitmentBestEffortLimited],[CommitmentMaxAuthority],[CommitmentMandatory],[CommitmentMaxAmount],[CommitmentDeliveryIndividual],[CommitmentDeliveryBulk],[CommitmentDeliveryAOT],[CommitmentDeliveryBulkAOT],[CommitmentDeliveryLiveTrade],[CommitmentDeliveryCoIssue],[CommitmentDeliveryForward],[CommitmentExceedPolicy],[CommitmentExceedTradePolicy],[CommitmentMessage],[LPASponsored],[LPASponsorTPONumber],[LPASponsorLPAPassword],[LEINumber],[NoAfterHourWires],[CanAcceptFirstPayments],[PrimarySalesRepAssignedDate],[Timezone],[BillingAddress],[BillingCity],[BillingState],[BillingZip],[GlobalOrSpecificTPO], [IsTestAccount]) VALUES (@oid,CAST(@orgid AS VARCHAR(25))," + (object) (newExternalContact.OrganizationType == ExternalOriginatorOrgType.Company ? 0 : (newExternalContact.OrganizationType == ExternalOriginatorOrgType.CompanyExtension ? 1 : (newExternalContact.OrganizationType == ExternalOriginatorOrgType.Branch ? 2 : (newExternalContact.OrganizationType == ExternalOriginatorOrgType.BranchExtension ? 3 : 0)))) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.DisabledLogin ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.MultiFactorAuthentication ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseParentInfoForCompanyDetails ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OldExternalID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OwnerName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PhoneNumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FaxNumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Email) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Website) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.Manager) + "," + (newExternalContact.LastLoanSubmitted != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.LastLoanSubmitted) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseParentInfoForRateLock ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EmailForRateSheet) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FaxForRateSheet) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EmailForLockInfo) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FaxForLockInfo) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseParentInfoForEPPS ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSUserName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSCompModel) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSPriceGroup) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSPriceGroupBroker) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSPriceGroupDelegated) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSPriceGroupNonDelegated) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EPPSRateSheet) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PMLUserName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PMLCustomerCode) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseParentInfoForApprovalStatus ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CurrentStatus) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.AddToWatchlist ? 1 : 0)) + "," + (newExternalContact.CurrentStatusDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CurrentStatusDate) : (object) "NULL") + "," + (newExternalContact.ApprovedDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.ApprovedDate) : (object) "NULL") + "," + (newExternalContact.ApplicationDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.ApplicationDate) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyRating) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseParentInfoForBusinessInfo ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.Incorporated ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.StateIncorp) + "," + (newExternalContact.DateOfIncorporation != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.DateOfIncorporation) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.TypeOfEntity) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.OtherEntityDescription) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.TaxID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.UseSSNFormat ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.NmlsId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FinancialsPeriod) + "," + (newExternalContact.FinancialsLastUpdate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.FinancialsLastUpdate) : (object) "NULL") + "," + (newExternalContact.CompanyNetWorth.HasValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CompanyNetWorth) : (object) "NULL") + "," + (newExternalContact.EOExpirationDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EOExpirationDate) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EOCompany) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.EOPolicyNumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.MERSOriginatingORGID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.DUSponsored) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CanFundInOwnName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.CanCloseInOwnName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.OrganizationType != ExternalOriginatorOrgType.Company ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PrimarySalesRepUserId) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.CommitmentUseBestEffort) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.CommitmentUseBestEffortLimited) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.MaxCommitmentAuthority) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.CommitmentMandatory) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.MaxCommitmentAmount) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryIndividual) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryBulk) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryAOT) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryBulkAOT) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryLiveTrade) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryCoIssue) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsCommitmentDeliveryForward) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) newExternalContact.CommitmentPolicy) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) newExternalContact.CommitmentTradePolicy) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.CommitmentMessage) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.LPASponsored) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.LPASponsorTPONumber) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.LPASponsorLPAPassword) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.LEINumber) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.NoAfterHourWires ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.CanAcceptFirstPayments ? 1 : 0)) + "," + (newExternalContact.PrimarySalesRepAssignedDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalContact.PrimarySalesRepAssignedDate) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.Timezone) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.BillingAddress) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.BillingCity) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.BillingState) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalContact.BillingZip) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (newExternalContact.GlobalOrSpecificTPO ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalContact.IsTestAccount) + ")";
    }

    private static void insertOrgDetails(
      ExternalOriginatorManagementData newExternalContact,
      int orgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) orgId);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.insertOrgDetailsQuery(newExternalContact));
        dbQueryBuilder.ExecuteScalar();
        ExternalOrgManagementAccessor.UpdatePMLPassword(newExternalContact.PMLPassword, orgId);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("addOrgDetails: Cannot update table ExternalOrgDetail.\r\n" + ex.Message);
      }
    }

    private static string UpdateCommitmentsSettingsForExternalOrgQuery(
      ExternalOriginatorManagementData externalContact)
    {
      return "UPDATE ExternalOrgDetail SET [CommitmentBestEffort] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentUseBestEffort) + ",[CommitmentBestEffortLimited] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentUseBestEffortLimited) + ",[CommitmentMaxAuthority] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MaxCommitmentAuthority) + ",[CommitmentMandatory] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentMandatory) + ",[CommitmentMaxAmount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MaxCommitmentAmount) + ",[CommitmentDeliveryIndividual] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryIndividual) + ",[CommitmentDeliveryBulk] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryBulk) + ",[CommitmentDeliveryAOT] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryAOT) + ",[CommitmentDeliveryBulkAOT] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryBulkAOT) + ",[CommitmentDeliveryLiveTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryLiveTrade) + ",[CommitmentDeliveryCoIssue] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryCoIssue) + ",[CommitmentDeliveryForward] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryForward) + ",[CommitmentExceedPolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) externalContact.CommitmentPolicy) + ",[CommitmentExceedTradePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) externalContact.CommitmentTradePolicy) + ",[CommitmentMessage] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.CommitmentMessage) + ",[BestEffortDailyVolumeLimit] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortDailyVolumeLimit) + ",[BestEffortTolerencePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.BestEffortTolerencePolicy) + ",[BestEffortTolerancePct] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortTolerancePct) + ",[BestEffortToleranceAmt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortToleranceAmt) + ",[BestEfforDailyLimitPolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.BestEfforDailyLimitPolicy) + ",[DailyLimitWarningMsg] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.DailyLimitWarningMsg) + ",[MandatoryTolerencePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.MandatoryTolerencePolicy) + ",[MandatoryTolerancePct] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MandatoryTolerancePct) + ",[MandatoryToleranceAmt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MandatoryToleranceAmt) + ",[ResetLimitForRatesheetId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.ResetLimitForRatesheetId) + " WHERE externalOrgID = @oid";
    }

    public static ExternalOriginatorManagementData GetExtOrgMinimalDetails(int extOrgId)
    {
      ExternalOriginatorManagementData orgMinimalDetails = (ExternalOriginatorManagementData) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid, ExternalID, parent, OrganizationType FROM [ExternalOriginatorManagement] INNER JOIN [ExternalOrgDetail] ON oid = externalOrgID WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) extOrgId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count > 0)
      {
        orgMinimalDetails = new ExternalOriginatorManagementData();
        orgMinimalDetails.oid = Convert.ToInt32(dataRowCollection[0]["oid"]);
        orgMinimalDetails.ExternalID = Convert.ToString(dataRowCollection[0]["ExternalID"]);
        orgMinimalDetails.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0]["OrganizationType"]);
      }
      return orgMinimalDetails;
    }

    public static CommitmentsCalculatorResponseModel GetCommitmentsCalculations(int extOrgId)
    {
      if (extOrgId <= 0)
        return (CommitmentsCalculatorResponseModel) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("EXEC GetExtOrgCommitmentCalculations {0}", (object) extOrgId));
      DataSet dataSet = (DataSet) null;
      try
      {
        using (SqlCommand sqlCmd = new SqlCommand())
        {
          sqlCmd.CommandText = dbQueryBuilder.ToString();
          dataSet = new EllieMae.EMLite.Server.DbAccessManager().ExecuteSetQuery((IDbCommand) sqlCmd);
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception(string.Format("GetCommitmentsCalculations: Cannot perform calculations for extOrgId {0} .\r\n {1}", (object) extOrgId, (object) ex.Message));
      }
      return dataSet != null && dataSet.Tables.Count > 0 ? ExternalOrgManagementAccessor.GetCommitmentsFromDataTables(dataSet.Tables) : (CommitmentsCalculatorResponseModel) null;
    }

    private static CommitmentsCalculatorResponseModel GetCommitmentsFromDataTables(
      DataTableCollection tables)
    {
      CommitmentsCalculatorResponseModel commitmentsFromDataTables = (CommitmentsCalculatorResponseModel) null;
      if (tables[0].Rows.Count > 0)
      {
        commitmentsFromDataTables = new CommitmentsCalculatorResponseModel();
        DataRow row = tables[0].Rows[0];
        commitmentsFromDataTables.BestEfforts = new CommitmentsCalculatorBaseResponseModel();
        commitmentsFromDataTables.BestEfforts.OutstandingCommitments = new AmountModel();
        commitmentsFromDataTables.BestEfforts.OutstandingCommitments.Amount = new long?(Convert.ToInt64(row["OutstandingCommitments"]));
        if (row["OutstandingCommitmentsPercentage"] != DBNull.Value)
          commitmentsFromDataTables.BestEfforts.OutstandingCommitments.Percentage = new Decimal?(Convert.ToDecimal(row["OutstandingCommitmentsPercentage"]));
        if (row["AvailableAmount"] != DBNull.Value)
        {
          commitmentsFromDataTables.BestEfforts.AvailableAmount = new AmountModel();
          commitmentsFromDataTables.BestEfforts.AvailableAmount.Amount = new long?(Convert.ToInt64(row["AvailableAmount"]));
        }
        if (row["AvailableAmountPercentage"] != DBNull.Value)
          commitmentsFromDataTables.BestEfforts.AvailableAmount.Percentage = new Decimal?(Convert.ToDecimal(row["AvailableAmountPercentage"]));
      }
      if (tables[1].Rows.Count > 0)
      {
        if (commitmentsFromDataTables == null)
          commitmentsFromDataTables = new CommitmentsCalculatorResponseModel();
        DataRow row = tables[1].Rows[0];
        commitmentsFromDataTables.Mandatory = new MandatoryCommitmentsCalculatorResponseModel();
        commitmentsFromDataTables.Mandatory.OutstandingCommitments = new AmountModel();
        commitmentsFromDataTables.Mandatory.OutstandingCommitments.Amount = new long?(Convert.ToInt64(row["OutstandingCommitments"]));
        commitmentsFromDataTables.Mandatory.OutstandingCommitments.Percentage = new Decimal?(Convert.ToDecimal(row["OutstandingCommitmentsPercentage"]));
        commitmentsFromDataTables.Mandatory.AvailableAmount = new AmountModel();
        commitmentsFromDataTables.Mandatory.AvailableAmount.Amount = new long?(Convert.ToInt64(row["AvailableAmount"]));
        commitmentsFromDataTables.Mandatory.AvailableAmount.Percentage = new Decimal?(Convert.ToDecimal(row["AvailableAmountPercentage"]));
      }
      if (tables[2].Rows.Count > 0 && commitmentsFromDataTables.Mandatory != null)
      {
        commitmentsFromDataTables.Mandatory.DeliveryTypes = new CommitmentsDeliveryTypeCalculatorResponseModel();
        foreach (DataRow row in (InternalDataCollectionBase) tables[2].Rows)
        {
          CorrespondentMasterDeliveryType result;
          if (Enum.TryParse<CorrespondentMasterDeliveryType>(row["DeliveryType"].ToString(), out result))
          {
            AmountModel amountModel = new AmountModel();
            amountModel.Amount = new long?(Convert.ToInt64(row["Amount"]));
            amountModel.Percentage = new Decimal?((Decimal) Convert.ToInt64(row["Percentage"]));
            switch (result)
            {
              case CorrespondentMasterDeliveryType.AOT:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.Aot = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.Forwards:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.Forward = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.IndividualMandatory:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.Individual = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.LiveTrade:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.DirectTrade = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.Bulk:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.Bulk = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.BulkAOT:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.BulkAot = amountModel;
                continue;
              case CorrespondentMasterDeliveryType.CoIssue:
                commitmentsFromDataTables.Mandatory.DeliveryTypes.CoIssue = amountModel;
                continue;
              default:
                continue;
            }
          }
        }
      }
      return commitmentsFromDataTables;
    }

    private static void updateOrgDetails(ExternalOriginatorManagementData externalContact)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from ExternalOrgDetail WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.oid));
      if (dbQueryBuilder.Execute().Count == 0)
      {
        ExternalOrgManagementAccessor.insertOrgDetails(externalContact, externalContact.oid);
      }
      else
      {
        dbQueryBuilder.Reset();
        string text = "UPDATE ExternalOrgDetail SET [OrganizationID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OrgID) + ",[OrganizationType] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) externalContact.OrganizationType) + ",[DisableLogin] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.DisabledLogin ? 1 : 0)) + ",[MultiFactorAuthentication] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.MultiFactorAuthentication ? 1 : 0)) + ",[InheritCompanyDetails] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseParentInfoForCompanyDetails ? 1 : 0)) + ",[OldExternalID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OldExternalID) + ",[OwnerName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OwnerName) + ",[Phone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PhoneNumber) + ",[Fax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxNumber) + ",[Email] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Email) + ",[WebUrl] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Website) + ",[ManagerUserID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Manager) + ",[LastLoanSubmittedDate] = " + (externalContact.LastLoanSubmitted != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LastLoanSubmitted) : "NULL") + ",[InheritParentRateSheet] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseParentInfoForRateLock ? 1 : 0)) + ",[RateSheetEmail] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EmailForRateSheet) + ",[RateSheetFax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxForRateSheet) + ",[LockInfoEmail] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EmailForLockInfo) + ",[LockInfoFax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxForLockInfo) + ",[InheritParentProductPricing] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseParentInfoForEPPS ? 1 : 0)) + ",[EPPSUserName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSUserName) + ",[EPPSCompModel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSCompModel) + ",[EPPSPriceGroup] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroup) + ",[EPPSPriceGroupBroker] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupBroker) + ",[EPPSPriceGroupDel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupDelegated) + ",[EPPSPriceGroupNonDel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupNonDelegated) + ",[EPPSRateSheet] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSRateSheet) + ",[PMLUserName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PMLUserName) + ",[PMLCustomerCode] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PMLCustomerCode) + ",[InheritParentApprovalStatus] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseParentInfoForApprovalStatus ? 1 : 0)) + ",[CurrentApprovalStatus] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CurrentStatus) + ",[AddToApprovalWatchlist] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.AddToWatchlist ? 1 : 0)) + ",[CurrentApprovalStatusDate] = " + (externalContact.CurrentStatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CurrentStatusDate) : "NULL") + ",[ApprovedDate] = " + (externalContact.ApprovedDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.ApprovedDate) : "NULL") + ",[ApplicationDate] = " + (externalContact.ApplicationDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.ApplicationDate) : "NULL") + ",[CompanyRating] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyRating) + ",[InheritParentBizInformation] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseParentInfoForBusinessInfo ? 1 : 0)) + ",[Incorporated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.Incorporated ? 1 : 0)) + ",[StateIncorporated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.StateIncorp) + ",[DateIncorporated] = " + (externalContact.DateOfIncorporation != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.DateOfIncorporation) : "NULL") + ",[BizEntityType] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.TypeOfEntity) + ",[BizOtherEntityDesc] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OtherEntityDescription) + ",[TaxID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.TaxID) + ",[UseSSNFormat] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseSSNFormat ? 1 : 0)) + ",[NMLSID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.NmlsId) + ",[FinancialPeriod] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FinancialsPeriod) + ",[FinancialLastUpdated] = " + (externalContact.FinancialsLastUpdate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FinancialsLastUpdate) : "NULL") + ",[CompanyNetWorth] = " + (externalContact.CompanyNetWorth.HasValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyNetWorth) : "NULL") + ",[EOExpirationDate] = " + (externalContact.EOExpirationDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOExpirationDate) : "NULL") + ",[EOCompany] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOCompany) + ",[EOPolicyNumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOPolicyNumber) + ",[MERSOriginatorOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MERSOriginatingORGID) + ",[DUSponsored] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.DUSponsored) + ",[CanFundInOwnName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CanFundInOwnName) + ",[CanCloseInOwnName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CanCloseInOwnName) + ",[PrimarySalesRepUserId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PrimarySalesRepUserId) + ",[CommitmentBestEffort] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentUseBestEffort) + ",[CommitmentBestEffortLimited] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentUseBestEffortLimited) + ",[CommitmentMaxAuthority] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MaxCommitmentAuthority) + ",[CommitmentMandatory] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.CommitmentMandatory) + ",[CommitmentMaxAmount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MaxCommitmentAmount) + ",[CommitmentDeliveryIndividual] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryIndividual) + ",[CommitmentDeliveryBulk] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryBulk) + ",[CommitmentDeliveryAOT] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryAOT) + ",[CommitmentDeliveryBulkAOT] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryBulkAOT) + ",[CommitmentDeliveryLiveTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryLiveTrade) + ",[CommitmentDeliveryCoIssue] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryCoIssue) + ",[CommitmentDeliveryForward] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.IsCommitmentDeliveryForward) + ",[CommitmentExceedPolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) externalContact.CommitmentPolicy) + ",[CommitmentExceedTradePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) externalContact.CommitmentTradePolicy) + ",[CommitmentMessage] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.CommitmentMessage) + ",[TradeMgmtEnableTPOTradeMgmtForTPOClient] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtEnableTPOTradeManagementForTPOClient) + ",[TradeMgmtUseCompanyTPOTradeMgmtSettings] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtUseCompanyTPOTradeManagementSettings) + ",[TradeMgmtViewCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtViewCorrespondentTrade) + ",[TradeMgmtViewCorrespondentMasterCommitment] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtViewCorrespondentMasterCommitment) + ",[TradeMgmtLoanEligibilityToCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanEligibilityToCorrespondentTrade) + ",[TradeMgmtEPPSLoanProgramEligibilityPricing] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtEPPSLoanProgramEligibilityPricing) + ",[TradeMgmtLoanAssignmentToCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanAssignmentToCorrespondentTrade) + ",[TradeMgmtLoanDeletionFromCorrespondentTrade] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtLoanDeletionFromCorrespondentTrade) + ",[TradeMgmtRequestPairOff] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtRequestPairOff) + ",[TradeMgmtReceiveCommitmentConfirmation] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.TradeMgmtReceiveCommitmentConfirmation) + ",[LPASponsored] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LPASponsored) + ",[LPASponsorTPONumber] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.LPASponsorTPONumber) + ",[LPASponsorLPAPassword] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.LPASponsorLPAPassword) + ",[LEINumber] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.LEINumber) + ",[BestEffortDailyVolumeLimit] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortDailyVolumeLimit) + ",[BestEffortTolerencePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.BestEffortTolerencePolicy) + ",[BestEffortTolerancePct] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortTolerancePct) + ",[BestEffortToleranceAmt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BestEffortToleranceAmt) + ",[BestEfforDailyLimitPolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.BestEfforDailyLimitPolicy) + ",[DailyLimitWarningMsg] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.DailyLimitWarningMsg) + ",[MandatoryTolerencePolicy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (byte) externalContact.MandatoryTolerencePolicy) + ",[MandatoryTolerancePct] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MandatoryTolerancePct) + ",[MandatoryToleranceAmt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MandatoryToleranceAmt) + ",[NoAfterHourWires] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.NoAfterHourWires ? 1 : 0)) + ",[CanAcceptFirstPayments] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.CanAcceptFirstPayments ? 1 : 0)) + ",[PrimarySalesRepAssignedDate] = " + (externalContact.PrimarySalesRepAssignedDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PrimarySalesRepAssignedDate) : "NULL") + ",[BillingAddress] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingAddress) + ",[BillingCity] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingCity) + ",[BillingState] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingState) + ",[BillingZip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingZip) + ",[Timezone] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalContact.Timezone) + ",[ResetLimitForRatesheetId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalContact.ResetLimitForRatesheetId) + " WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.oid);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdatePMLPassword(externalContact.PMLPassword, externalContact.oid);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT [externalOrgID], [parent], InheritCompanyDetails, InheritParentRateSheet, InheritParentProductPricing, InheritParentApprovalStatus, InheritParentBizInformation\r\n                FROM ExternalOrgDetail, ExternalOriginatorManagement \r\n                WHERE (InheritCompanyDetails = 1 OR InheritParentRateSheet = 1 OR InheritParentProductPricing = 1 OR InheritParentApprovalStatus = 1 OR InheritParentBizInformation = 1) \r\n                    AND [oid] = [externalOrgID] \r\n                    AND [externalOrgID] IN (SELECT descendent from ExternalOrgDescendents where oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.oid) + ")");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        dbQueryBuilder.Reset();
        Dictionary<string, Dictionary<int, int>> dictionary1 = new Dictionary<string, Dictionary<int, int>>();
        Dictionary<string, List<int>> dictionary2 = new Dictionary<string, List<int>>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          int int32_1 = Convert.ToInt32(row["externalOrgID"]);
          int int32_2 = Convert.ToInt32(row["parent"]);
          foreach (string str in ExternalOrgManagementAccessor.TABLE_FLAGS)
          {
            if (Convert.ToBoolean(row[str]))
            {
              if (!dictionary1.ContainsKey(str))
                dictionary1.Add(str, new Dictionary<int, int>());
              dictionary1[str].Add(int32_1, int32_2);
            }
          }
        }
        foreach (KeyValuePair<string, Dictionary<int, int>> keyValuePair1 in dictionary1)
        {
          List<int> source = new List<int>();
          foreach (KeyValuePair<int, int> keyValuePair2 in keyValuePair1.Value)
          {
            if (keyValuePair1.Value.ContainsKey(keyValuePair2.Value) || keyValuePair2.Value == externalContact.oid)
              source.Add(keyValuePair2.Key);
          }
          if (source.Any<int>())
            dictionary2.Add(keyValuePair1.Key, source);
        }
        List<int> source1;
        if (dictionary2.TryGetValue("InheritCompanyDetails", out source1) && source1.Any<int>())
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgDetail SET [OrganizationID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OrgID) + ",[DisableLogin] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.DisabledLogin ? 1 : 0)) + ",[NoAfterHourWires] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.NoAfterHourWires ? 1 : 0)) + ",[MultiFactorAuthentication] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.MultiFactorAuthentication ? 1 : 0)) + ",[OwnerName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OwnerName) + ",[Phone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PhoneNumber) + ",[Fax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxNumber) + ",[Email] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Email) + ",[WebUrl] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Website) + ",[ManagerUserID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.Manager) + ",[LastLoanSubmittedDate] = " + (externalContact.LastLoanSubmitted != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LastLoanSubmitted) : "NULL") + ",[LPASponsored] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LPASponsored) + ",[LPASponsorTPONumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LPASponsorTPONumber) + ",[LPASponsorLPAPassword] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LPASponsorLPAPassword) + ",[BillingAddress] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingAddress) + ",[BillingCity] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingCity) + ",[BillingState] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingState) + ",[BillingZip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.BillingZip) + ",[CanAcceptFirstPayments] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.CanAcceptFirstPayments ? 1 : 0)) + " WHERE externalOrgID IN (" + string.Join(",", source1.Select<int, string>((System.Func<int, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x)))) + ")");
        if (dictionary2.TryGetValue("InheritParentRateSheet", out source1) && source1.Any<int>())
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgDetail SET [RateSheetEmail] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EmailForRateSheet) + ",[RateSheetFax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxForRateSheet) + ",[LockInfoEmail] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EmailForLockInfo) + ",[LockInfoFax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FaxForLockInfo) + " WHERE externalOrgID IN (" + string.Join(",", source1.Select<int, string>((System.Func<int, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x)))) + ")");
        if (dictionary2.TryGetValue("InheritParentProductPricing", out source1) && source1.Any<int>())
        {
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgDetail SET [EPPSUserName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSUserName) + ",[EPPSCompModel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSCompModel) + ",[EPPSPriceGroup] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroup) + ",[EPPSPriceGroupBroker] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupBroker) + ",[EPPSPriceGroupDel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupDelegated) + ",[EPPSPriceGroupNonDel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSPriceGroupNonDelegated) + ",[EPPSRateSheet] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EPPSRateSheet) + ",[PMLUserName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PMLUserName) + ",[PMLCustomerCode] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.PMLCustomerCode) + " WHERE externalOrgID IN (" + string.Join(",", source1.Select<int, string>((System.Func<int, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x)))) + ")");
          foreach (int externalOrgId in source1)
            ExternalOrgManagementAccessor.UpdatePMLPassword(externalContact.PMLPassword, externalOrgId);
        }
        if (dictionary2.TryGetValue("InheritParentApprovalStatus", out source1) && source1.Any<int>())
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgDetail SET [CurrentApprovalStatus] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CurrentStatus) + ",[AddToApprovalWatchlist] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.AddToWatchlist ? 1 : 0)) + ",[CurrentApprovalStatusDate] = " + (externalContact.CurrentStatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CurrentStatusDate) : "NULL") + ",[ApprovedDate] = " + (externalContact.ApprovedDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.ApprovedDate) : "NULL") + ",[ApplicationDate] = " + (externalContact.ApplicationDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.ApplicationDate) : "NULL") + ",[CompanyRating] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyRating) + " WHERE externalOrgID IN (" + string.Join(",", source1.Select<int, string>((System.Func<int, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x)))) + ")");
        if (dictionary2.TryGetValue("InheritParentBizInformation", out source1) && source1.Any<int>())
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgDetail SET [Incorporated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.Incorporated ? 1 : 0)) + ",[StateIncorporated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.StateIncorp) + ",[DateIncorporated] = " + (externalContact.DateOfIncorporation != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.DateOfIncorporation) : "NULL") + ",[BizEntityType] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.TypeOfEntity) + ",[BizOtherEntityDesc] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.OtherEntityDescription) + ",[TaxID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.TaxID) + ",[UseSSNFormat] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalContact.UseSSNFormat ? 1 : 0)) + ",[NMLSID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.NmlsId) + ",[FinancialPeriod] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FinancialsPeriod) + ",[FinancialLastUpdated] = " + (externalContact.FinancialsLastUpdate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.FinancialsLastUpdate) : "NULL") + ",[CompanyNetWorth] = " + (externalContact.CompanyNetWorth.HasValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CompanyNetWorth) : "NULL") + ",[EOExpirationDate] = " + (externalContact.EOExpirationDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOExpirationDate) : "NULL") + ",[EOCompany] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOCompany) + ",[EOPolicyNumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.EOPolicyNumber) + ",[MERSOriginatorOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.MERSOriginatingORGID) + ",[DUSponsored] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.DUSponsored) + ",[CanFundInOwnName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CanFundInOwnName) + ",[CanCloseInOwnName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.CanCloseInOwnName) + ",[LEINumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalContact.LEINumber) + " WHERE externalOrgID IN (" + string.Join(",", source1.Select<int, string>((System.Func<int, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x)))) + ")");
        if (dbQueryBuilder.Length <= 0)
          return;
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void UpdatePMLPassword(string PMLPassword, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) externalOrgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdatePMLPasswordQuery(PMLPassword));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static string UpdatePMLPasswordQuery(string PMLPassword)
    {
      return PMLPassword == null || !(PMLPassword != "") ? "UPDATE ExternalOrgDetail SET [PMLPassword] = NULL WHERE externalOrgID = @oid" : "UPDATE ExternalOrgDetail SET [PMLPassword] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) PMLPassword) + " WHERE externalOrgID = @oid";
    }

    public static void UpdateExternalLicence(BranchExtLicensing license, int oid)
    {
      List<int> parentInfoLicense = ExternalOrgManagementAccessor.GetExternalOrgsUsingParentInfoLicense(oid);
      bool flag = true;
      foreach (int oid1 in parentInfoLicense)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("Select * from ExternalOrgDetail where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
        if (dbQueryBuilder.Execute().Count == 0)
          ExternalOrgManagementAccessor.insertOrgDetails(ExternalOrgManagementAccessor.GetByoid(false, oid), oid);
        flag = oid != oid1 || license.UseParentInfo;
        dbQueryBuilder.Reset();
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) oid);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateExtOrgLicenseQuery(license));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("delete from ExternalOrgStateLicensing WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
        dbQueryBuilder.Execute();
        dbQueryBuilder.Reset();
        if (license.StateLicenseExtTypes.Count > 0)
        {
          foreach (StateLicenseExtType stateLicenseExtType in license.StateLicenseExtTypes)
          {
            dbQueryBuilder.AppendLine("select * from ExternalOrgStateLicensing WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and State = '" + stateLicenseExtType.StateAbbrevation + "' and LicenseType = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(stateLicenseExtType.LicenseType));
            if (dbQueryBuilder.Execute().Count == 0)
              ExternalOrgManagementAccessor.InsertLicensing(stateLicenseExtType, oid1);
            else
              ExternalOrgManagementAccessor.UpdateLicensing(stateLicenseExtType, oid1);
            dbQueryBuilder.Reset();
          }
        }
        else
          ExternalOrgManagementAccessor.deleteLicensing(oid1);
      }
    }

    private static string UpdateExtOrgLicenseQuery(BranchExtLicensing license)
    {
      return "UPDATE ExternalOrgDetail SET [InheritParentLicense] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (license.UseParentInfo ? 1 : 0)) + ",[AllowLoansWithIssues] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.AllowLoansWithIssues) + ",[MsgUploadNonApprovedLoans]  = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.MsgUploadNonApprovedLoans) + ",[LicenseLenderType] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LenderType) + ",[LicenseHomeState] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.HomeState) + ",[LicenseOptOut] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.OptOut) + ",[LicenseStatutoryMaryland] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (license.StatutoryElectionInMaryland ? 1 : 0)) + ",[LicenseStatutoryMaryland2] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.StatutoryElectionInMaryland2) + ",[LicenseStatutoryKansas] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (license.StatutoryElectionInKansas ? 1 : 0)) + ",[ATRQMSmallCreditor]  = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) license.ATRSmallCreditor) + ",[ATRQMExemptCreditor] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) license.ATRExemptCreditor) + " where externalOrgID = @oid";
    }

    private static string InsertExtOrgLicensingQuery(StateLicenseExtType stateLicense)
    {
      return "INSERT INTO ExternalOrgStateLicensing VALUES ( @oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.StateAbbrevation) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.LicenseType) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (stateLicense.Approved ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (stateLicense.Exempt ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.LicenseNo) + "," + (stateLicense.IssueDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.IssueDate) : "NULL") + "," + (stateLicense.StartDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.StartDate) : "NULL") + "," + (stateLicense.EndDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.EndDate) : "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.LicenseStatus) + "," + (stateLicense.StatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.StatusDate) : "NULL") + "," + (stateLicense.LastChecked != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.LastChecked) : "NULL") + ", " + (stateLicense.SortIndex != 0 ? EllieMae.EMLite.DataAccess.SQL.Encode((object) stateLicense.SortIndex) : "NULL") + ")";
    }

    private static List<int> GetExternalOrgsUsingParentInfoLicense(int oid)
    {
      try
      {
        List<int> parentInfoLicense = new List<int>();
        parentInfoLicense.Add(oid);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT [descendent] FROM [ExternalOrgDescendents] extOrgdesc INNER JOIN [ExternalOrgDetail] extOrgDet on extOrgDet.externalOrgID = extOrgdesc.descendent where extOrgDet.InheritParentLicense = 1 AND extOrgdesc.oid = " + (object) oid);
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          parentInfoLicense.Add(Convert.ToInt32(dataRow["descendent"]));
        return parentInfoLicense;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgsUsingParentInfoLicense: Error getting external orgs using parent info for license.\r\n" + ex.Message);
      }
    }

    private static void InsertLicensing(StateLicenseExtType license, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) oid);
      try
      {
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExtOrgLicensingQuery(license));
        dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("insertLicensing: Cannot update table ExternalOrgStateLicensing.\r\n" + ex.Message);
      }
    }

    private static void UpdateLicensing(StateLicenseExtType license, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE ExternalOrgStateLicensing SET [State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.StateAbbrevation) + ",[LicenseType] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LicenseType) + ",[LicenseApproved] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (license.Approved ? 1 : 0)) + ",[LicenseExempt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (license.Exempt ? 1 : 0)) + ",[LicenseNumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LicenseNo) + ",[IssueDate] = " + (license.IssueDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.IssueDate) : "NULL") + ",[StartDate] = " + (license.StartDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.StartDate) : "NULL") + ",[EndDate] = " + (license.EndDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.EndDate) : "NULL") + ",[Status] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LicenseStatus) + ",[StatusDate] = " + (license.StatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.StatusDate) : "NULL") + ",[LastCheckedDate] = " + (license.LastChecked != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LastChecked) : "NULL") + ", [SortIndex] = " + (license.SortIndex != 0 ? EllieMae.EMLite.DataAccess.SQL.Encode((object) license.SortIndex) : "NULL") + " WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " and State = '" + license.StateAbbrevation + "' and LicenseType = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) license.LicenseType);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("updateLicensing: Cannot update table ExternalOrgStateLicensing.\r\n" + ex.Message);
      }
    }

    private static void deleteLicensing(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM ExternalOrgStateLicensing WHERE externalOrgID=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("deleteLicensing: Cannot delete state licenses.\r\n" + ex.Message);
      }
    }

    private static void InsertUserLicensing(StateLicenseExtType license, string externalUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertUserLicensingQuery(license, externalUserId));
        dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("insertUserLicensing: Cannot insert in table ExternalUserStateLicensing.\r\n" + ex.Message);
      }
    }

    private static void UpdateUserLicensing(StateLicenseExtType license, string externalUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateUserLicensingQuery(license, externalUserId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("updateUserLicensing: Cannot update table ExternalUserStateLicensing.\r\n" + ex.Message);
      }
    }

    private static void updateExternalChildrenPlans(
      LoanCompHistoryList parentHistoryList,
      int oid,
      bool uncheckParentInfo,
      bool forLender)
    {
      TraceLog.WriteVerbose(nameof (ExternalOrgManagementAccessor), "updateExternalChildrenPlans: Update child branch for LO Comp");
      try
      {
        int[] ofExternalCompany = ExternalOrgManagementAccessor.getDescendentsOfExternalCompany(oid, forLender, true);
        if (uncheckParentInfo)
        {
          if (ofExternalCompany == null || ofExternalCompany.Length == 0)
            return;
          string str = string.Empty;
          for (int index = 0; index < ofExternalCompany.Length; ++index)
            str = (str != string.Empty ? (object) "," : (object) "").ToString() + (object) ofExternalCompany[index];
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("UPDATE " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " SET [UseParentInfo] = 0 WHERE oid IN (" + str + ")");
          dbQueryBuilder.ExecuteNonQuery();
          for (int index = 0; index < ofExternalCompany.Length; ++index)
            ExternalOrgManagementAccessor.updateExternalChildrenPlans((LoanCompHistoryList) null, ofExternalCompany[index], uncheckParentInfo, forLender);
        }
        else
        {
          if (ofExternalCompany == null || ofExternalCompany.Length == 0)
            return;
          DateTime today;
          List<LoanCompHistory> loanCompHistoryList1;
          if (parentHistoryList == null)
          {
            loanCompHistoryList1 = (List<LoanCompHistory>) null;
          }
          else
          {
            LoanCompHistoryList loanCompHistoryList2 = parentHistoryList;
            today = DateTime.Today;
            DateTime date = today.Date;
            loanCompHistoryList1 = loanCompHistoryList2.GetCurrentAndFuturePlans(date);
          }
          List<LoanCompHistory> loanCompHistoryList3 = loanCompHistoryList1;
          for (int index = 0; index < ofExternalCompany.Length; ++index)
          {
            LoanCompHistoryList planHistoryforOrg = LOCompAccessor.GetComPlanHistoryforOrg(ofExternalCompany[index], true, forLender);
            if (planHistoryforOrg != null)
            {
              planHistoryforOrg.SetID(ofExternalCompany[index].ToString());
              planHistoryforOrg.UseParentInfo = true;
              LoanCompHistoryList loanCompHistoryList4 = planHistoryforOrg;
              List<LoanCompHistory> parentPlans = loanCompHistoryList3;
              today = DateTime.Today;
              DateTime date = today.Date;
              loanCompHistoryList4.AddParentPlans(parentPlans, date);
              LOCompAccessor.CreateHistoryCompPlansForOrg(planHistoryforOrg, ofExternalCompany[index], true, forLender);
              ExternalOrgManagementAccessor.updateExternalChildrenPlans(planHistoryforOrg, ofExternalCompany[index], uncheckParentInfo, forLender);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
    }

    private static int[] getDescendentsOfExternalCompany(
      int oid,
      bool forLender,
      bool includeUseParentInfoOnly)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT oid FROM [" + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + "] WHERE [Parent] = " + (object) oid);
      if (includeUseParentInfoOnly)
        dbQueryBuilder.Append(" AND [UseParentInfo] = 'True'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (int[]) null;
      List<int> intList = new List<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        intList.Add((int) dataRowCollection[index][0]);
      return intList?.ToArray();
    }

    public static void MoveExternalCompany(bool forLender, HierarchySummary summary)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (summary.oid == summary.Parent)
      {
        int num = (int) MessageBox.Show("You can't assign a company to itself.");
      }
      else
      {
        try
        {
          string text = "UPDATE " + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + " SET [parent] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) summary.Parent) + ",[UseParentInfo] = " + (object) 0 + ",[Depth] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) summary.Depth) + ",[HierarchyPath] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) summary.HierarchyPath) + " WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) summary.oid);
          dbQueryBuilder.AppendLine(text);
          dbQueryBuilder.ExecuteNonQuery();
          ExternalOrgManagementAccessor.runHierarchyMove(forLender, summary.oid, summary.Depth, summary.HierarchyPath);
          ExternalOrgManagementAccessor.updateExternalChildrenPlans((LoanCompHistoryList) null, summary.oid, true, forLender);
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("MoveExternalCompany: Cannot update record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
        }
      }
    }

    public static void MoveExternalUser(List<ExternalUserInfo> extUserList, int oId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        ExternalOriginatorManagementData externalOrganization = ExternalOrgManagementAccessor.GetExternalOrganization(false, oId);
        string str = string.Join("','", extUserList.Select<ExternalUserInfo, string>((System.Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>());
        dbQueryBuilder.AppendLine("UPDATE [users] SET org_id = " + (object) ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(oId, false) + " where userid in (SELECT [ContactID] FROM [ExternalUsers] WHERE external_userid in ( '" + str + "'));");
        dbQueryBuilder.Append("update  [ExternalUsers] set [externalOrgId] ='" + (object) oId + "', [UpdatedBy] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(extUserList[0].UpdatedBy) + " , [UpdatedDTTM] = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ",  [UpdatedByExternal] =" + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(extUserList[0].UpdatedByExternal) + ", [Address1] = case when [UseCompanyAddress] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.Address) + "  else [Address1]  end, [City] = case when [UseCompanyAddress] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.City) + " else [City] end, [State] = case when [UseCompanyAddress] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.State) + " else [State] end, [Zip] = case when [UseCompanyAddress] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.Zip) + " else [Zip] end, [Rate_sheet_email] = case when [InheritParentRateSheet] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.EmailForRateSheet) + " else [Rate_sheet_email] end, [Rate_sheet_fax] = case when [InheritParentRateSheet] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.FaxForRateSheet) + " else [Rate_sheet_fax] end, [Lock_info_email] = case when [InheritParentRateSheet] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.EmailForLockInfo) + " else [Lock_info_email] end,[Lock_info_fax] = case when [InheritParentRateSheet] =1 then " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.FaxForLockInfo) + " else [Lock_info_fax] end where [external_userid] in ( '" + str + "')");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
    }

    private static void runHierarchyMove(bool forLender, int oid, int depth, string hierarchy)
    {
      string text = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        List<int> oidByParentId = ExternalOrgManagementAccessor.GetOidByParentId(forLender, oid);
        if (oidByParentId.Count <= 0)
          return;
        foreach (int oid1 in oidByParentId)
        {
          ExternalOriginatorManagementData byoid = ExternalOrgManagementAccessor.GetByoid(forLender, oid1);
          int depth1 = depth + 1;
          string hierarchy1 = hierarchy + "\\" + byoid.CompanyLegalName;
          text = text + "UPDATE " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " SET [Depth] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) depth1) + ",[HierarchyPath] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) hierarchy1) + " WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1);
          ExternalOrgManagementAccessor.runHierarchyMove(forLender, oid1, depth1, hierarchy1);
        }
        if (!(text != ""))
          return;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("runHierarchyMove: Cannot update ExternalOriginatorManagement record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static void DeleteExternalContact(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int num = -1;
      try
      {
        if (!forLender)
        {
          num = ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(oid, true);
          dbQueryBuilder.AppendLine("Delete from [ExternalOrgSelectedURL] where [externalOrgID] =  " + (object) oid);
        }
        dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "Ext_LenderCompPlans" : (object) "Ext_CompPlans") + " WHERE oid = " + (object) oid);
        dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + " WHERE oid = " + (object) oid);
        if (!forLender)
        {
          dbQueryBuilder.AppendLine("Delete from [org_chart] where [oid] = " + (object) num);
          dbQueryBuilder.AppendLine("Delete from [ExternalOrgLenderContacts] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
          dbQueryBuilder.AppendLine("Delete from [ExternalOrgCompanyLenderContacts] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
        }
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.hierarchyDelete(forLender, oid);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalContact: Cannot delete record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    private static void hierarchyDelete(bool forLender, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int num = -1;
      try
      {
        List<int> oidByParentId = ExternalOrgManagementAccessor.GetOidByParentId(forLender, oid);
        if (oidByParentId.Count <= 0)
          return;
        foreach (int oid1 in oidByParentId)
        {
          if (!forLender)
          {
            num = ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(oid, true);
            dbQueryBuilder.AppendLine("Delete from [ExternalOrgSelectedURL] where [externalOrgID] =  " + (object) oid1);
          }
          dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "Ext_LenderCompPlans" : (object) "Ext_CompPlans") + " WHERE oid = " + (object) oid1);
          dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + " WHERE oid = " + (object) oid1);
          if (!forLender)
            dbQueryBuilder.AppendLine("Delete from [org_chart] where [oid] = " + (object) num);
          ExternalOrgManagementAccessor.hierarchyDelete(forLender, oid1);
        }
        if (!(dbQueryBuilder.ToString() != ""))
          return;
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("hierarchyDelete: Cannot update record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static void DeleteExternalCompPlans(bool forLender, int oid, int CompPlanId)
    {
      ExternalOrgManagementAccessor.DeleteExternalCompPlans((forLender ? 1 : 0) != 0, oid, new int[1]
      {
        CompPlanId
      });
    }

    public static void DeleteExternalCompPlans(bool forLender, int oid, int[] CompPlanId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "Ext_LenderCompPlans" : (object) "Ext_CompPlans") + " WHERE oid = " + (object) oid + "AND CompPlanId IN (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) CompPlanId) + ")");
        ExternalOrgManagementAccessor.hierarchyDeleteExtCompPlans(forLender, oid, CompPlanId);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalCompPlans: Cannot delete record in " + (forLender ? "Ext_LenderCompPlans" : "Ext_CompPlans") + " table.\r\n" + ex.Message);
      }
    }

    private static void hierarchyDeleteExtCompPlans(bool forLender, int oid, int[] CompPlanId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        List<int> oidByParentId = ExternalOrgManagementAccessor.GetOidByParentId(forLender, oid);
        if (oidByParentId.Count <= 0)
          return;
        foreach (int oid1 in oidByParentId)
        {
          if (ExternalOrgManagementAccessor.GetUseParentInfoValue(forLender, oid1))
          {
            dbQueryBuilder.AppendLine("DELETE from " + (forLender ? (object) "Ext_LenderCompPlans" : (object) "Ext_CompPlans") + " WHERE oid = " + (object) oid1 + "AND CompPlanId IN (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) CompPlanId) + ")");
            ExternalOrgManagementAccessor.hierarchyDeleteExtCompPlans(forLender, oid1, CompPlanId);
          }
        }
        if (!(dbQueryBuilder.ToString() != ""))
          return;
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("hierarchyDeleteExtCompPlans: Cannot delete record in " + (forLender ? "Ext_LenderCompPlans" : "Ext_CompPlans") + " table.\r\n" + ex.Message);
      }
    }

    public static LoanCompPlan GetCurrentComPlanforBrokerByID(
      bool forLender,
      string brokerID,
      DateTime triggerDateTime)
    {
      LoanCompHistoryList planHistoryforUser = LOCompAccessor.GetComPlanHistoryforUser(brokerID, true, forLender);
      if (planHistoryforUser == null)
        return (LoanCompPlan) null;
      LoanCompHistory currentPlan = planHistoryforUser.GetCurrentPlan(triggerDateTime.Date);
      return currentPlan == null ? (LoanCompPlan) null : LOCompAccessor.GetLoanCompPlanByID(currentPlan.CompPlanId);
    }

    public static LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(
      string TPOWebID,
      DateTime triggerDateTime)
    {
      return ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByTPOWebID(false, TPOWebID, triggerDateTime);
    }

    public static LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(
      bool forLender,
      string TPOWebID,
      DateTime triggerDateTime)
    {
      int oidByTpoId = ExternalOrgManagementAccessor.GetOidByTPOId(forLender, TPOWebID);
      return oidByTpoId == 0 ? (LoanCompPlan) null : ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByID(forLender, string.Concat((object) oidByTpoId), triggerDateTime);
    }

    public static LoanCompPlan GetCurrentComPlanforBrokerByName(
      bool forLender,
      string brokerName,
      DateTime triggerDateTime)
    {
      List<ExternalOriginatorManagementData> contactByName = ExternalOrgManagementAccessor.GetContactByName(forLender, brokerName, true);
      return contactByName == null || contactByName.Count == 0 ? (LoanCompPlan) null : ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByID(forLender, contactByName[0].oid.ToString(), triggerDateTime);
    }

    public static int AddTPOContact(
      bool forLender,
      string id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      ExternalOriginatorEntityType entityType,
      string address,
      string city,
      string state,
      string zip,
      int parent,
      int depth,
      string hierarchyPath)
    {
      return ExternalOrgManagementAccessor.AddExternalContacts(forLender, new ExternalOriginatorManagementData()
      {
        oid = -1,
        contactType = ExternalOriginatorContactType.TPO,
        Parent = parent,
        ExternalID = id,
        OrganizationName = organizationName,
        CompanyDBAName = companyDBAName,
        CompanyLegalName = companyLegalName,
        entityType = entityType,
        Address = address,
        City = city,
        State = state,
        Zip = zip,
        UseParentInfo = false,
        Depth = depth,
        HierarchyPath = hierarchyPath
      });
    }

    public static void UpdateTPOContact(
      bool forLender,
      string id,
      ExternalOriginatorManagementData externalContact,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      ExternalOriginatorManagementData externalContact1 = externalContact;
      externalContact1.oid = Utils.ParseInt((object) id);
      externalContact1.contactType = ExternalOriginatorContactType.TPO;
      externalContact1.Parent = parent;
      externalContact1.UseParentInfo = useParentInfo;
      externalContact1.Depth = depth;
      externalContact1.HierarchyPath = hierarchyPath;
      ExternalOrgManagementAccessor.UpdateExternalContact(forLender, externalContact1, loanCompHistoryList);
    }

    public static void OverwriteTPOContact(
      bool forLender,
      int oid,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      int parent,
      int depth,
      string hierarchyPath)
    {
      ExternalOriginatorManagementData byoid = ExternalOrgManagementAccessor.GetByoid(forLender, oid);
      bool flag = false;
      if (byoid.UseParentInfo && parent != byoid.Parent)
        flag = true;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [" + (forLender ? (object) "ExternalLenders" : (object) "ExternalOriginatorManagement") + "] SET [parent] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parent) + ",[OrganizationName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) organizationName) + ",[CompanyDBAName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyDBAName) + ",[CompanyLegalName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyLegalName) + "," + (entityType == ExternalOriginatorEntityType.None ? (object) "" : (object) ("[entityType] = " + (object) (int) entityType + ",")) + "[Address] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) address) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) city) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) state) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) zip) + ",[Depth] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) depth) + "," + (flag ? (object) "[UseParentInfo] = 0," : (object) "") + "[HierarchyPath] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) hierarchyPath) + " WHERE oid = " + (object) oid;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot overwrite ExternalOriginatorManagement record in " + (forLender ? "ExternalLenders" : "ExternalOriginatorManagement") + " table.\r\n" + ex.Message);
      }
    }

    public static void UpdateLOCompUseParentInfo(bool useParentInfo, int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [ExternalOriginatorManagement] SET [UseParentInfo] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(useParentInfo) + " WHERE oid = " + (object) oid;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot overwrite UseParentInfo record in ExternalOriginatorManagemen table.\r\n" + ex.Message);
      }
    }

    public static ExternalOriginatorManagementData[] QueryExternalOrganizations(
      QueryCriterion[] criteria)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Append("select distinct org.* from [ExternalOriginatorManagement] org ");
        dbQueryBuilder.AppendLine(" where (1 = 1)");
        for (int index = 0; index < criteria.Length; ++index)
          dbQueryBuilder.AppendLine("   and (" + criteria[index].ToSQLClause() + ")");
        List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
            originatorManagementDataList.Add(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row));
        }
        return originatorManagementDataList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("QueryExternalOrganizations: Cannot query record in ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static bool DoesTpoContactExists(
      ExternalUserInfo user,
      int destOid,
      List<int> urlsTobeAdded)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalUserURL[] externalUserInfoUrLs1 = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(user.ExternalUserID);
      dbQueryBuilder.Append("select [Login_email],[external_userid] from [ExternalUsers] where [externalOrgID] =  " + (object) destOid);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          if (Convert.ToString(dataRow["Login_email"]) == user.EmailForLogin)
          {
            ExternalUserURL[] externalUserInfoUrLs2 = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(Convert.ToString(dataRow["external_userid"]));
            if (!((IEnumerable<ExternalUserURL>) externalUserInfoUrLs2).Any<ExternalUserURL>() && !((IEnumerable<ExternalUserURL>) externalUserInfoUrLs1).Any<ExternalUserURL>())
              return true;
            foreach (int num in urlsTobeAdded)
            {
              int d = num;
              if (((IEnumerable<ExternalUserURL>) externalUserInfoUrLs2).Where<ExternalUserURL>((System.Func<ExternalUserURL, bool>) (a => a.URLID == d)).FirstOrDefault<ExternalUserURL>() != null)
                return true;
            }
            foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs1)
            {
              ExternalUserURL url = externalUserUrl;
              if (((IEnumerable<ExternalUserURL>) externalUserInfoUrLs2).Where<ExternalUserURL>((System.Func<ExternalUserURL, bool>) (a => a.URLID == url.URLID)).FirstOrDefault<ExternalUserURL>() != null)
                return true;
            }
          }
        }
      }
      return false;
    }

    public static int AddBusinessContact(
      bool forLender,
      int BusinessID,
      string CompanyDBAName,
      string CompanyLegalName,
      string Address,
      string City,
      string State,
      string Zip,
      int parent,
      int depth,
      string hierarchyPath)
    {
      return ExternalOrgManagementAccessor.AddExternalContacts(forLender, new ExternalOriginatorManagementData()
      {
        oid = -1,
        contactType = ExternalOriginatorContactType.BusinessContacts,
        Parent = parent,
        ExternalID = BusinessID.ToString(),
        CompanyDBAName = CompanyDBAName,
        CompanyLegalName = CompanyLegalName,
        entityType = ExternalOriginatorEntityType.None,
        Address = Address,
        City = City,
        State = State,
        Zip = Zip,
        UseParentInfo = false,
        Depth = depth,
        HierarchyPath = hierarchyPath
      });
    }

    public static void UpdateBusinessContact(
      bool forLender,
      int ID,
      string OrganizationName,
      string CompanyDBAName,
      string CompanyLegalName,
      string Address,
      string City,
      string State,
      string Zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      ExternalOrgManagementAccessor.UpdateExternalContact(forLender, new ExternalOriginatorManagementData()
      {
        oid = ID,
        contactType = ExternalOriginatorContactType.BusinessContacts,
        Parent = parent,
        ExternalID = ID.ToString(),
        CompanyDBAName = CompanyDBAName,
        CompanyLegalName = CompanyLegalName,
        OrganizationName = OrganizationName,
        entityType = entityType,
        Address = Address,
        City = City,
        State = State,
        Zip = Zip,
        UseParentInfo = useParentInfo,
        Depth = depth,
        HierarchyPath = hierarchyPath
      }, loanCompHistoryList);
    }

    public static int AddManualContact(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      bool UseParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      ExternalOriginatorManagementData newExternalContact1 = newExternalContact;
      newExternalContact1.oid = -1;
      if (newExternalContact1.contactType == (ExternalOriginatorContactType) 0)
        newExternalContact1.contactType = ExternalOriginatorContactType.FreeEntry;
      newExternalContact1.Parent = parent;
      newExternalContact1.UseParentInfo = UseParentInfo;
      newExternalContact1.Depth = depth;
      newExternalContact1.HierarchyPath = hierarchyPath;
      return ExternalOrgManagementAccessor.AddExternalContacts(forLender, newExternalContact1, loanCompHistoryList);
    }

    public static void UpdateManualContact(
      bool forLender,
      int ID,
      string OrganizationName,
      string CompanyDBAName,
      string CompanyLegalName,
      string Address,
      string City,
      string State,
      string Zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      ExternalOrgManagementAccessor.UpdateExternalContact(forLender, new ExternalOriginatorManagementData()
      {
        oid = ID,
        contactType = ExternalOriginatorContactType.FreeEntry,
        Parent = parent,
        ExternalID = ID.ToString(),
        CompanyDBAName = CompanyDBAName,
        CompanyLegalName = CompanyLegalName,
        OrganizationName = OrganizationName,
        entityType = entityType,
        Address = Address,
        City = City,
        State = State,
        Zip = Zip,
        UseParentInfo = useParentInfo,
        Depth = depth,
        HierarchyPath = hierarchyPath
      }, loanCompHistoryList);
    }

    public static ExternalOrgNotes GetExternalOrganizationNotes(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgNotes] WHERE [externalOrgID] = " + (object) oid + " ORDER BY [AddedDateTime]");
      try
      {
        ExternalOrgNotes organizationNotes = new ExternalOrgNotes();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            organizationNotes.AddNotes(new ExternalOrgNote()
            {
              NoteID = Convert.ToInt32(dataRow["notesID"]),
              ExternalCompanyID = Convert.ToInt32(dataRow["externalOrgID"]),
              AddedDateTime = Convert.ToDateTime(dataRow["AddedDateTime"]),
              WhoAdded = Convert.ToString(dataRow["UserID"]),
              NotesDetails = dataRow["NotesDetails"] != null ? Convert.ToString(dataRow["NotesDetails"]) : ""
            });
        }
        return organizationNotes;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationNotes: Cannot fetch all records from [ExternalOrgNotes] table.\r\n" + ex.Message);
      }
    }

    public static int AddExternalOrganizationNotes(ExternalOrgNote newExtOrgNote)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@notesID", "int");
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) newExtOrgNote.ExternalCompanyID);
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalOrganizationNotes: Creating SQL commands for table ExternalOrgNotes.");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.AddExternalOrganizationNotesQuery(new List<ExternalOrgNote>()
      {
        newExtOrgNote
      }));
      dbQueryBuilder.SelectIdentity("@notesID");
      dbQueryBuilder.Select("@notesID");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrganizationNotes: Cannot insert notes to table 'ExternalOrgNotes' due to the following problem:\r\n" + ex.Message);
      }
    }

    private static string AddExternalOrganizationNotesQuery(List<ExternalOrgNote> newExtOrgNotes)
    {
      string str1 = "";
      foreach (ExternalOrgNote newExtOrgNote in newExtOrgNotes)
      {
        string str2 = "INSERT INTO [ExternalOrgNotes] ([externalOrgID], [AddedDateTime], [UserID], [NotesDetails]) VALUES (@oid, '" + newExtOrgNote.AddedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt") + "'," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExtOrgNote.WhoAdded) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExtOrgNote.NotesDetails) + ")";
        str1 += str2;
      }
      return str1;
    }

    public static bool DeleteExternalOrganizationNotes(int oid, ExternalOrgNotes deletedExtOrgNotes)
    {
      string str = string.Empty;
      for (int i = 0; i < deletedExtOrgNotes.Count; ++i)
        str = str + (str != string.Empty ? (object) "," : (object) "") + (object) deletedExtOrgNotes.GetNotesAt(i).NoteID;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE from [ExternalOrgNotes] WHERE externalOrgID = " + (object) oid + " AND notesID in (" + str + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrganizationNotes: Cannot delete record in [ExternalOrgNotes] due to this error: " + ex.Message);
      }
    }

    public static bool DeleteExternalOrganizationNotes(int oid, List<int> notesID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE from [ExternalOrgNotes] WHERE externalOrgID = " + (object) oid + " AND notesID in (" + string.Join<int>(",", (IEnumerable<int>) notesID) + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrganizationNotes: Cannot delete record in [ExternalOrgNotes] due to this error: " + ex.Message);
      }
    }

    public static ExternalOrgLoanTypes GetExternalOrganizationLoanTypes(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgLoanTypes] WHERE [externalOrgID] = " + (object) oid);
      try
      {
        ExternalOrgLoanTypes organizationLoanTypes = new ExternalOrgLoanTypes();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          IEnumerator enumerator = dataRowCollection.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
            {
              DataRow current = (DataRow) enumerator.Current;
              organizationLoanTypes.ExternalOrgID = Convert.ToInt32(current["externalOrgID"]);
              organizationLoanTypes.UseParentInfoFhaVa = Convert.ToBoolean(current["InheritParentFHAVA"]);
              organizationLoanTypes.FHAId = Convert.ToString(current["FHAID"]);
              organizationLoanTypes.FHASonsorId = Convert.ToString(current["FHASponsorID"]);
              organizationLoanTypes.FHAStatus = Convert.ToString(current["FHAStatus"]);
              organizationLoanTypes.FHACompareRatio = Convert.ToDecimal(current["FHACompareRatio"]);
              organizationLoanTypes.FHAApprovedDate = current["FHAApprovedDate"] != DBNull.Value ? Convert.ToDateTime(current["FHAApprovedDate"]) : DateTime.MinValue;
              organizationLoanTypes.FHAExpirationDate = current["FHAExpirationDate"] != DBNull.Value ? Convert.ToDateTime(current["FHAExpirationDate"]) : DateTime.MinValue;
              organizationLoanTypes.VAId = Convert.ToString(current["VAID"]);
              organizationLoanTypes.VAStatus = Convert.ToString(current["VAStatus"]);
              organizationLoanTypes.VAApprovedDate = current["VAApprovedDate"] != DBNull.Value ? Convert.ToDateTime(current["VAApprovedDate"]) : DateTime.MinValue;
              organizationLoanTypes.VAExpirationDate = current["VAExpirationDate"] != DBNull.Value ? Convert.ToDateTime(current["VAExpirationDate"]) : DateTime.MinValue;
              organizationLoanTypes.Underwriting = current["Underwriting"] == DBNull.Value || current["Underwriting"].ToString() == "-1" ? 1 : Convert.ToInt32(current["Underwriting"]);
              organizationLoanTypes.AdvancedCode = Convert.ToString(current["AdvancedCode"]);
              organizationLoanTypes.AdvancedCodeXml = Convert.ToString(current["AdvancedCodeXml"]);
              organizationLoanTypes.Broker = ExternalOrgManagementAccessor.getExternalOrganizationLoanTypesChannels(oid, 0);
              organizationLoanTypes.CorrespondentDelegated = ExternalOrgManagementAccessor.getExternalOrganizationLoanTypesChannels(oid, 1);
              organizationLoanTypes.CorrespondentNonDelegated = ExternalOrgManagementAccessor.getExternalOrganizationLoanTypesChannels(oid, 2);
              organizationLoanTypes.FHADirectEndorsement = Convert.ToString(current["FHADirectEndorsement"]);
              organizationLoanTypes.VASponsorID = Convert.ToString(current["VASponsorID"]);
              organizationLoanTypes.FHMLCApproved = Convert.ToBoolean(current["FHMLCApproved"] != DBNull.Value ? current["FHMLCApproved"] : (object) 0);
              organizationLoanTypes.FNMAApproved = Convert.ToBoolean(current["FNMAApproved"] != DBNull.Value ? current["FNMAApproved"] : (object) 0);
              organizationLoanTypes.FannieMaeID = Convert.ToString(current["FannieMaeID"]);
              organizationLoanTypes.FreddieMacID = Convert.ToString(current["FreddieMacID"]);
              organizationLoanTypes.AUSMethod = Convert.ToString(current["AUSMethod"]);
            }
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        }
        return organizationLoanTypes;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationLoanTypes: Cannot fetch Loan Types Info from [ExternalOrgLoanTypes] table.\r\n" + ex.Message);
      }
    }

    private static ExternalOrgLoanTypes.ExternalOrgChannelLoanType getExternalOrganizationLoanTypesChannels(
      int oid,
      int channel)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgLoanTypesChannels] WHERE [externalOrgID] = " + (object) oid + " and [ChannelType] = " + (object) channel);
      try
      {
        ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanTypesChannels = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          IEnumerator enumerator = dataRowCollection.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
            {
              DataRow current = (DataRow) enumerator.Current;
              loanTypesChannels.LoanTypes = Convert.ToInt32(current["LoanTypes"]);
              loanTypesChannels.LoanPurpose = Convert.ToInt32(current["LoanPurposes"]);
              loanTypesChannels.AllowLoansWithIssues = Convert.ToInt32(current["AllowLoansWithIssues"]);
              loanTypesChannels.MsgUploadNonApprovedLoans = Convert.ToString(current["MsgUploadNonApprovedLoans"]);
            }
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        }
        return loanTypesChannels;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("getExternalOrganizationLoanTypesChannels: Cannot fetch Loan Types Info from [getExternalOrganizationLoanTypesChannels] table.\r\n" + ex.Message);
      }
    }

    private static string UpdateExternalOrganizationCustomFields(
      int oid,
      ExternalOrgCustomFields externalOrgCustomFields,
      List<SqlParameter> cmdParams)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DataTable fieldsUpsertDataTable = ExternalOrgManagementAccessor.GetCustomFieldsUpsertDataTable(oid, externalOrgCustomFields.Fields);
      dbQueryBuilder.AppendLine("EXEC UpsertExtOrgCustomFields @CustomFields");
      cmdParams.Add(new SqlParameter("@CustomFields", (object) fieldsUpsertDataTable)
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_CustomFields"
      });
      return dbQueryBuilder.ToString();
    }

    private static DataTable GetCustomFieldsUpsertDataTable(
      int orgId,
      List<TpoCustomFields> customFields)
    {
      DataTable fieldsUpsertDataTable = new DataTable();
      fieldsUpsertDataTable.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("FieldId", typeof (int)),
        new DataColumn("ExternalOrgID", typeof (int)),
        new DataColumn("OwnerID", typeof (string)),
        new DataColumn("FieldValue", typeof (string))
      });
      foreach (TpoCustomFields customField in customFields)
      {
        DataRow row = fieldsUpsertDataTable.NewRow();
        row["FieldId"] = (object) customField.CustomFieldId;
        row["ExternalOrgID"] = (object) orgId;
        row["OwnerID"] = (object) customField.OwnerID;
        row["FieldValue"] = (object) customField.FieldValue;
        fieldsUpsertDataTable.Rows.Add(row);
      }
      return fieldsUpsertDataTable;
    }

    public static string UpdateExternalOrganizationLoanTypesQuery(
      int oid,
      ExternalOrgLoanTypes loanTypes)
    {
      List<int> parentInfoLoanType = ExternalOrgManagementAccessor.GetExternalOrgsUsingParentInfoLoanType(oid);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (int oid1 in parentInfoLoanType)
      {
        bool useParentInfo = oid != oid1 || loanTypes.UseParentInfoFhaVa;
        try
        {
          dbQueryBuilder.AppendLine("DELETE from [ExternalOrgLoanTypes] WHERE [externalOrgID] = " + (object) oid1);
          TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
          dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrgLoanTypesQuery(loanTypes, useParentInfo));
          if (loanTypes.Broker != null)
          {
            dbQueryBuilder.AppendLine("If Exists(select 1 from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 0 + ")");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.updateLoanTypeChannelQuery(oid1, 0, loanTypes.Broker));
            dbQueryBuilder.AppendLine("end else begin");
            dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(0, loanTypes.Broker));
            dbQueryBuilder.AppendLine("end");
          }
          if (loanTypes.CorrespondentDelegated != null)
          {
            dbQueryBuilder.AppendLine("If Exists(select 1 from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 1 + ")");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.updateLoanTypeChannelQuery(oid1, 1, loanTypes.CorrespondentDelegated));
            dbQueryBuilder.AppendLine("end else begin");
            dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(1, loanTypes.CorrespondentDelegated));
            dbQueryBuilder.AppendLine("end");
          }
          if (loanTypes.CorrespondentNonDelegated != null)
          {
            dbQueryBuilder.AppendLine("If Exists(select 1 from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 2 + ")");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.updateLoanTypeChannelQuery(oid1, 2, loanTypes.CorrespondentNonDelegated));
            dbQueryBuilder.AppendLine("end else begin");
            dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(2, loanTypes.CorrespondentNonDelegated));
            dbQueryBuilder.AppendLine("end");
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("UpdateExternalOrganizationLoanTypesQuery: Cannot update record from [ExternalOrgLoanTypes] due to this error: " + ex.Message);
        }
      }
      dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      return dbQueryBuilder.ToString();
    }

    public static bool UpdateExternalOrganizationLoanTypes(int oid, ExternalOrgLoanTypes loanTypes)
    {
      List<int> parentInfoLoanType = ExternalOrgManagementAccessor.GetExternalOrgsUsingParentInfoLoanType(oid);
      foreach (int oid1 in parentInfoLoanType)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        bool useParentInfo = oid != oid1 || loanTypes.UseParentInfoFhaVa;
        try
        {
          dbQueryBuilder.AppendLine("DELETE from [ExternalOrgLoanTypes] WHERE [externalOrgID] = " + (object) oid1);
          TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
          dbQueryBuilder.Declare("@oid", "int");
          dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
          dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrgLoanTypesQuery(loanTypes, useParentInfo));
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
          if (loanTypes.Broker != null)
          {
            dbQueryBuilder.AppendLine("select * from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 0);
            if (dbQueryBuilder.Execute().Count == 0)
            {
              dbQueryBuilder.Declare("@oid", "int");
              dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
              dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(0, loanTypes.Broker));
              dbQueryBuilder.ExecuteNonQuery();
            }
            else
              ExternalOrgManagementAccessor.updateLoanTypeChannel(oid1, 0, loanTypes.Broker);
            dbQueryBuilder.Reset();
          }
          if (loanTypes.CorrespondentDelegated != null)
          {
            dbQueryBuilder.AppendLine("select * from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 1);
            if (dbQueryBuilder.Execute().Count == 0)
            {
              dbQueryBuilder.Declare("@oid", "int");
              dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
              dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(1, loanTypes.CorrespondentDelegated));
              dbQueryBuilder.ExecuteNonQuery();
            }
            else
              ExternalOrgManagementAccessor.updateLoanTypeChannel(oid1, 1, loanTypes.CorrespondentDelegated);
            dbQueryBuilder.Reset();
          }
          if (loanTypes.CorrespondentNonDelegated != null)
          {
            dbQueryBuilder.AppendLine("select * from [ExternalOrgLoanTypesChannels] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1) + " and [ChannelType] = " + (object) 2);
            if (dbQueryBuilder.Execute().Count == 0)
            {
              dbQueryBuilder.Declare("@oid", "int");
              dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid1));
              dbQueryBuilder.Append(ExternalOrgManagementAccessor.InsertLoanTypeChannelQuery(2, loanTypes.CorrespondentNonDelegated));
              dbQueryBuilder.ExecuteNonQuery();
            }
            else
              ExternalOrgManagementAccessor.updateLoanTypeChannel(oid1, 2, loanTypes.CorrespondentNonDelegated);
            dbQueryBuilder.Reset();
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("UpdateExternalOrganizationLoanTypes: Cannot update record from [ExternalOrgLoanTypes] due to this error: " + ex.Message);
        }
      }
      return true;
    }

    private static string InsertLoanTypeChannelQuery(
      int channelType,
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanType)
    {
      return "INSERT INTO [ExternalOrgLoanTypesChannels] ([externalOrgID], [ChannelType], [LoanTypes], [LoanPurposes], [AllowLoansWithIssues], [MsgUploadNonApprovedLoans]) VALUES (@oid," + EllieMae.EMLite.DataAccess.SQL.Encode((object) channelType) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.LoanTypes) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.LoanPurpose) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.AllowLoansWithIssues) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.MsgUploadNonApprovedLoans) + ")";
    }

    private static string InsertExternalOrgLoanTypesQuery(
      ExternalOrgLoanTypes loanTypes,
      bool useParentInfo)
    {
      return "INSERT INTO [ExternalOrgLoanTypes] ([externalOrgID],[InheritParentFHAVA], [FHAID], [FHASponsorID], [FHAStatus], [FHACompareRatio], [FHAApprovedDate], [FHAExpirationDate], [VAID], [VAStatus], [VAApprovedDate], [VAExpirationDate], [Underwriting], [AdvancedCode], [AdvancedCodeXml], [FHADirectEndorsement], [VASponsorID], [FHMLCApproved], [FNMAApproved], [FannieMaeID], [FreddieMacID], [AUSMethod]  ) VALUES (@oid," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (useParentInfo ? 1 : 0)) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FHAId) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FHASonsorId) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FHAStatus) + ", " + (object) loanTypes.FHACompareRatio + ", " + (loanTypes.FHAApprovedDate != DateTime.MinValue ? (object) ("'" + loanTypes.FHAApprovedDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "'") : (object) "NULL") + ", " + (loanTypes.FHAExpirationDate != DateTime.MinValue ? (object) ("'" + loanTypes.FHAExpirationDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "'") : (object) "NULL") + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.VAId) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.VAStatus) + ", " + (loanTypes.VAApprovedDate != DateTime.MinValue ? (object) ("'" + loanTypes.VAApprovedDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "'") : (object) "NULL") + ", " + (loanTypes.VAExpirationDate != DateTime.MinValue ? (object) ("'" + loanTypes.VAExpirationDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "'") : (object) "NULL") + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.Underwriting) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.AdvancedCode) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.AdvancedCodeXml) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FHADirectEndorsement) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.VASponsorID) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (loanTypes.FHMLCApproved ? 1 : 0)) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (loanTypes.FNMAApproved ? 1 : 0)) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FannieMaeID) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.FreddieMacID) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanTypes.AUSMethod) + ")";
    }

    private static void updateLoanTypeChannel(
      int oid,
      int channelType,
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(ExternalOrgManagementAccessor.updateLoanTypeChannelQuery(oid, channelType, loanType));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static string updateLoanTypeChannelQuery(
      int oid,
      int channelType,
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(" UPDATE [ExternalOrgLoanTypesChannels] SET ");
      dbQueryBuilder.AppendLine("[LoanTypes] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.LoanTypes) + ",[LoanPurposes] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.LoanPurpose) + ",[AllowLoansWithIssues] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.AllowLoansWithIssues) + ",[MsgUploadNonApprovedLoans] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanType.MsgUploadNonApprovedLoans) + " WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " and [ChannelType] = " + (object) channelType);
      return dbQueryBuilder.ToString();
    }

    public static List<int> GetExternalOrgsUsingParentInfoLoanType(int oid)
    {
      try
      {
        List<int> parentInfoLoanType = new List<int>();
        parentInfoLoanType.Add(oid);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT [descendent] FROM [ExternalOrgDescendents] eod INNER JOIN [ExternalOrgLoanTypes] eol on eol.externalOrgID = eod.descendent where eol.InheritParentFHAVA = 1 AND eod.oid = " + (object) oid + " AND (0 not in ((SELECT [InheritParentFHAVA] FROM [ExternalOrgLoanTypes] eol1 INNER JOIN [ExternalOrgDescendents] eod1 ON eol1.externalOrgID = eod1.oid  where eod1.descendent = eod.descendent  and oid > " + (object) oid + ") UNION (SELECT [InheritParentFHAVA] FROM [ExternalOrgLoanTypes] eol2  INNER JOIN [ExternalOriginatorManagement] eom on eom.oid = eol2.externalOrgID  Where externalOrgID = eod.descendent and [InheritParentFHAVA] = 1 and eom.parent = " + (object) oid + " )))");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          parentInfoLoanType.Add(Convert.ToInt32(dataRow["descendent"]));
        return parentInfoLoanType;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgsUsingParentInfoLoanType: Error getting external orgs using parent info for loan types.\r\n" + ex.Message);
      }
    }

    public static FieldRuleInfo GetExternalUnderwritingConditions(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT externalOrgID,Underwriting,AdvancedCode, AdvancedCodeXml from ExternalOrgLoanTypes where externalOrgID = " + (object) oid);
      IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
        {
          DataRow current = (DataRow) enumerator.Current;
          FieldRuleInfo underwritingConditions = new FieldRuleInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(current["externalOrgID"]), "TPOUnderwriting");
          underwritingConditions.Condition = BizRule.Condition.AdvancedCoding;
          underwritingConditions.ConditionState = underwritingConditions.Condition == BizRule.Condition.AdvancedCoding ? string.Concat(current["advancedCode"]) : string.Concat(current["conditionState"]);
          underwritingConditions.AdvancedCodeXML = string.Concat(current["advancedCodeXml"]);
          return underwritingConditions;
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return (FieldRuleInfo) null;
    }

    public static List<ExternalOrgAttachments> GetExternalAttachmentsByOid(int oid)
    {
      try
      {
        List<ExternalOrgAttachments> attachmentsByOid = new List<ExternalOrgAttachments>();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [ExternalOrgAttachment] where [externalOrgID] = " + (object) oid);
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          attachmentsByOid.Add(ExternalOrgManagementAccessor.dataRowToExternalOrgAttachments(r));
        return attachmentsByOid;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAttachmentsByOid: Error getting external attachments.\r\n" + ex.Message);
      }
    }

    public static List<string> GetExternalAttachmentFileNames()
    {
      try
      {
        List<string> attachmentFileNames = new List<string>();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select FileName from ExternalOrgAttachment");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          attachmentFileNames.Add(Convert.ToString(dataRow["FileName"]));
        return attachmentFileNames;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetExternalAttachments: Error setting external attachments.\r\n" + ex.Message);
      }
    }

    public static bool GetExternalAttachmentIsExpired(int oid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select DaysToExpire from ExternalOrgAttachment where [externalOrgID] = " + (object) oid);
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          if (dataRow["DaysToExpire"] != DBNull.Value && Convert.ToInt32(dataRow["DaysToExpire"]) < 0)
            return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalAttachmentIsExpired: Error setting external attachments.\r\n" + ex.Message);
      }
    }

    public static void InsertExternalAttachment(ExternalOrgAttachments attachment)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("INSERT INTO ExternalOrgAttachment VALUES ('" + (object) attachment.Guid + "' ," + (object) attachment.ExternalOrgID + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.FileName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.Description) + "," + (attachment.DateAdded != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.DateAdded) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.Category) + "," + (attachment.FileDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.FileDate) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.UserWhoAdded) + ", " + (attachment.ExpirationDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.ExpirationDate) : (object) "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.RealFileName) + ")");
        dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("InsertExternalAttachment: Cannot insert in table ExternalOrgAttachment.\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalAttachment(ExternalOrgAttachments attachment)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE ExternalOrgAttachment SET [externalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.ExternalOrgID) + ",[FileName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.FileName) + ",[Description] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.Description) + ",[DateAdded] = " + (attachment.DateAdded != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.DateAdded) : (object) "NULL") + ",[Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.Category) + ",[FileDate] = " + (attachment.FileDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.FileDate) : (object) "NULL") + ",[UserWhoAdded] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.UserWhoAdded) + ",[ExpirationDate] = " + (attachment.ExpirationDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.ExpirationDate) : (object) "NULL") + ",[RealFileName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) attachment.RealFileName) + "WHERE Guid = '" + (object) attachment.Guid + "'";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalAttachment: Cannot update table ExternalOrgAttachment.\r\n" + ex.Message);
      }
    }

    public static void DeleteExternalAttachment(Guid guid, FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "Delete from ExternalOrgAttachment  WHERE Guid = '" + (object) guid + "'";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalAttachments"), entry.GetEncodedPath()), MutexAccess.Read))
        {
          if (!dataFile.Exists)
            return;
          dataFile.Delete();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalAttachment: Cannot delete from table ExternalOrgAttachment.\r\n" + ex.Message);
      }
    }

    public static DataFile AddAttachment(FileSystemEntry entry, BinaryObject data)
    {
      if (data == null)
        data = new BinaryObject(new byte[0]);
      string dataFolderPath = ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalAttachments");
      using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, entry.GetEncodedPath()), MutexAccess.Read))
      {
        if (dataFile.Exists)
          dataFile.Delete();
      }
      using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, entry.GetEncodedPath()), MutexAccess.Read))
      {
        dataFile.CreateNew(data);
        return dataFile;
      }
    }

    public static BinaryObject ReadAttachment(string fileName)
    {
      try
      {
        string dataFolderPath = ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalAttachments");
        FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + fileName, FileSystemEntry.Types.File, (string) null);
        using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, fileSystemEntry.GetEncodedPath()), MutexAccess.Read))
          return dataFile.Exists ? dataFile.GetData() : throw new Exception("The '" + fileSystemEntry.Name + "' attachment cannot be found or no longer exists.\r\n");
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception(ex.Message);
      }
    }

    public static DataFile CheckOut(string path, MutexAccess access)
    {
      if ((path ?? "") == "")
      {
        TraceLog.WriteVerbose(nameof (ExternalOrgManagementAccessor), "path is empty");
        Err.Raise(TraceLevel.Warning, nameof (ExternalOrgManagementAccessor), new ServerException("Invalid file path"));
      }
      DateTime now = DateTime.Now;
      SafeMutex innerLock = new SafeMutex((IClientContext) ClientContext.GetCurrent(), path, access);
      if (!innerLock.WaitOne(ExternalOrgManagementAccessor.mutexTimeout))
        Err.Raise(nameof (ExternalOrgManagementAccessor), new ServerException("Timeout waiting to obtain mutex on file '" + path + "'"));
      try
      {
        DataFile dataFile = new DataFile(path, innerLock);
        TimeSpan timeSpan = DateTime.Now - now;
        TraceLog.WriteVerbose("FileStore", "File '" + path + "' checked out with access '" + (object) access + "' in " + timeSpan.TotalMilliseconds.ToString("0") + " ms");
        return dataFile;
      }
      catch (Exception ex)
      {
        innerLock.ReleaseMutex();
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
        return (DataFile) null;
      }
    }

    private static ExternalOrgAttachments dataRowToExternalOrgAttachments(DataRow r)
    {
      return new ExternalOrgAttachments(Guid.Parse(Convert.ToString(r["Guid"])), Convert.ToInt32(r["externalOrgID"]), Convert.ToString(r["FileName"]), Convert.ToString(r["Description"]), r["DateAdded"] != DBNull.Value ? Convert.ToDateTime(r["DateAdded"]) : DateTime.MinValue, Convert.ToInt32(r["Category"]), r["FileDate"] != DBNull.Value ? Convert.ToDateTime(r["FileDate"]) : DateTime.MinValue, Convert.ToString(r["UserWhoAdded"]), r["ExpirationDate"] != DBNull.Value ? Convert.ToDateTime(r["ExpirationDate"]) : DateTime.MinValue, r["DaysToExpire"] != DBNull.Value ? Convert.ToInt32(r["DaysToExpire"]) : 0, Convert.ToString(r["RealFileName"]));
    }

    public static List<ExternalOrgSalesRep> GetExternalOrgSalesRepsByLoanId(string loanId)
    {
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.GetExternalOrgSalesRepsByLoanId: Creating SQL commands for table ExternalOrgSalesReps.");
      try
      {
        List<ExternalOrgSalesRep> salesRepsByLoanId = new List<ExternalOrgSalesRep>();
        if (!string.IsNullOrEmpty(loanId))
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("SELECT DISTINCT esr.salesRepId, esr.externalOrgID, esr.UserID, usr.personaAccessComments AS Title, eom.CompanyLegalName, eom.OrganizationName, usr.email, usr.phone, usr.FirstLastName, esr.WholesaleChannel, esr.DelegatedChannel, esr.NonDelegatedChannel, CASE WHEN esr.UserID = eod.PrimarySalesRepUserId THEN 'true' ELSE 'false' END AS isPrimarySalesRep FROM [ExternalOrgSalesReps] esr INNER JOIN [ExternalOriginatorManagement] eom ON eom.oid = esr.externalOrgID INNER JOIN [users] usr ON usr.userid = esr.UserID INNER JOIN [LoanSummary] ls ON ls.TPOCompanyID = eom.ExternalID INNER JOIN [ExternalOrgDetail] eod ON eod.externalOrgID = eom.oid WHERE ls.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanId));
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            {
              string title = Convert.ToString(dataRow["Title"]);
              if (title.Length > 30)
                title = title.Substring(0, 30);
              salesRepsByLoanId.Add(new ExternalOrgSalesRep(Convert.ToInt32(dataRow["salesRepId"]), Convert.ToInt32(dataRow["externalOrgID"]), Convert.ToString(dataRow["UserID"]), Convert.ToString(dataRow["CompanyLegalName"]), Convert.ToString(dataRow["OrganizationName"]), Convert.ToString(dataRow["FirstLastName"]), title, Convert.ToString(dataRow["phone"]), Convert.ToString(dataRow["email"]), Convert.ToBoolean(dataRow["isPrimarySalesRep"]), Convert.ToBoolean(dataRow["WholesaleChannel"]), Convert.ToBoolean(dataRow["DelegatedChannel"]), Convert.ToBoolean(dataRow["NonDelegatedChannel"])));
            }
          }
        }
        return salesRepsByLoanId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSalesRepsForLoan: Error getting external sales reps for the loan.\r\n" + ex.Message);
      }
    }

    public static List<string> GetxternalOrgSalesRepsByExternalUserId(string externalUserId)
    {
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.GetxternalOrgSalesRepsByExternalUserId: Creating SQL commands for view AEAccessibleExternalUsers.");
      try
      {
        List<string> stringList = new List<string>();
        if (!string.IsNullOrEmpty(externalUserId))
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("SELECT DISTINCT AEID FROM [AEAccessibleExternalUsers] WHERE [TPOContactID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserId));
          DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
          if (dataTable != null && dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              stringList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["AEID"]));
          }
        }
        return stringList;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetxternalOrgSalesRepsByExternalUserId: Error getting external sales reps for external user id.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCurrentOrg(int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @oid int; set @oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalOrgSalesRepsForCurrentOrgQuery());
      try
      {
        return ExternalOrgManagementAccessor.populateExternalOrgSalesRepsForCurrentOrg(dbQueryBuilder.Execute());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSalesRepsForCurrentOrg: Error getting external sales reps for the current organization.\r\n" + ex.Message);
      }
    }

    private static List<ExternalOrgSalesRep> populateExternalOrgSalesRepsForCurrentOrg(
      DataRowCollection rows)
    {
      List<ExternalOrgSalesRep> externalOrgSalesRepList = new List<ExternalOrgSalesRep>();
      if (rows != null && rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) rows)
        {
          string title = Convert.ToString(row["Title"]);
          if (title.Length > 30)
            title = title.Substring(0, 30);
          externalOrgSalesRepList.Add(new ExternalOrgSalesRep(Convert.ToInt32(row["salesRepId"]), Convert.ToInt32(row["externalOrgID"]), Convert.ToString(row["UserID"]), Convert.ToString(row["CompanyLegalName"]), Convert.ToString(row["OrganizationName"]), Convert.ToString(row["FirstLastName"]), title, Convert.ToString(row["phone"]), Convert.ToString(row["email"]), Convert.ToBoolean(row["isPrimarySalesRep"]), Convert.ToBoolean(row["WholesaleChannel"]), Convert.ToBoolean(row["DelegatedChannel"]), Convert.ToBoolean(row["NonDelegatedChannel"])));
        }
      }
      return externalOrgSalesRepList;
    }

    private static string getExternalOrgSalesRepsForCurrentOrgQuery()
    {
      return "SELECT distinct esr.salesRepId, esr.externalOrgID, esr.UserID, usr.jobtitle AS Title, eom.CompanyLegalName, eom.OrganizationName, usr.email, usr.phone, usr.FirstLastName, esr.WholesaleChannel, esr.DelegatedChannel, esr.NonDelegatedChannel, CASE WHEN esr.UserID = eod.PrimarySalesRepUserId THEN 'true' ELSE 'false' END AS isPrimarySalesRep FROM [ExternalOrgSalesReps] esr inner join [ExternalOriginatorManagement] eom on eom.oid = esr.externalOrgID INNER JOIN [users] usr ON usr.userid = esr.UserID INNER JOIN [ExternalOrgDetail] eod ON eod.externalOrgID = eom.oid where esr.externalOrgID =@oid OR esr.externalOrgID in (select oid from [ExternalOrgDescendents] where descendent =@oid or oid=@oid)";
    }

    public static List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCompany(int companyOrgId)
    {
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCompany: Creating SQL commands for table ExternalOrgSalesReps.");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "Declare @companyOid int; set @companyOid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyOrgId);
      dbQueryBuilder1.AppendLine(text);
      dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.getExternalorgSalesRepsForCompanyQuery());
      try
      {
        List<ExternalOrgSalesRep> salesRepsForCompany = ExternalOrgManagementAccessor.populateExternalorgSalesRepsForCompany(dbQueryBuilder1.Execute());
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.AppendLine(text);
        dbQueryBuilder2.AppendLine(ExternalOrgManagementAccessor.getSalesRepsAssignedToUserQuery());
        DataRowCollection rows = dbQueryBuilder2.Execute();
        salesRepsForCompany.AddRange((IEnumerable<ExternalOrgSalesRep>) ExternalOrgManagementAccessor.populateSalesRepsAssignedToUser(rows));
        return salesRepsForCompany;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSalesRepsForCompany: Error getting external sales reps for the current organization.\r\n" + ex.Message);
      }
    }

    private static List<ExternalOrgSalesRep> populateSalesRepsAssignedToUser(DataRowCollection rows)
    {
      List<ExternalOrgSalesRep> user = new List<ExternalOrgSalesRep>();
      if (rows != null && rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) rows)
          user.Add(new ExternalOrgSalesRep(0, Convert.ToInt32(row["externalOrgID"]), Convert.ToString(row["Sales_rep_userid"]), "", "", "", "", "", ""));
      }
      return user;
    }

    private static List<ExternalOrgSalesRep> populateExternalorgSalesRepsForCompany(
      DataRowCollection rows)
    {
      List<ExternalOrgSalesRep> externalOrgSalesRepList = new List<ExternalOrgSalesRep>();
      if (rows != null && rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) rows)
        {
          string title = Convert.ToString(row["Title"]);
          if (title.Length > 30)
            title = title.Substring(0, 30);
          externalOrgSalesRepList.Add(new ExternalOrgSalesRep(Convert.ToInt32(row["salesRepId"]), Convert.ToInt32(row["externalOrgID"]), Convert.ToString(row["UserID"]), Convert.ToString(row["CompanyLegalName"]), Convert.ToString(row["OrganizationName"]), Convert.ToString(row["FirstLastName"]), title, Convert.ToString(row["phone"]), Convert.ToString(row["email"]), Convert.ToBoolean(row["isPrimarySalesRep"]), Convert.ToBoolean(row["WholesaleChannel"]), Convert.ToBoolean(row["DelegatedChannel"]), Convert.ToBoolean(row["NonDelegatedChannel"])));
        }
      }
      return externalOrgSalesRepList;
    }

    private static string getSalesRepsAssignedToUserQuery()
    {
      return "SELECT distinct eu.externalOrgID,Sales_rep_userid FROM [ExternalUsers] eu inner join [ExternalOriginatorManagement] eom on eom.oid = eu.externalOrgID where eu.externalOrgID = @companyOid OR eu.externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =@companyOid) or eu.externalOrgID = @companyOid";
    }

    private static string getIsInheritWarehouseQuery()
    {
      return "SELECT [InheritWarehouses] FROM [ExternalOrgDetail] where [ExternalOrgID] = @oid";
    }

    private static string getWarehousesQuery()
    {
      return "SELECT * FROM ExternalOrgWarehouse warehouse inner join ExternalBanks bank on warehouse.BankID = bank.BankID inner join ExternalOriginatorManagement mgr on warehouse.ExternalOrgID = mgr.oid inner join ExternalOrgDetail on ExternalOrgDetail.externalOrgID = mgr.oid where warehouse.ExternalOrgID = @oid";
    }

    private static string getCommitmentsQuery()
    {
      return "SELECT CommitmentBestEffort, CommitmentBestEffortLimited,CommitmentMaxAuthority,BestEffortDailyVolumeLimit,BestEffortTolerencePolicy,BestEffortToleranceAmt,BestEffortTolerancePct,BestEfforDailyLimitPolicy,DailyLimitWarningMsg,CommitmentMandatory,CommitmentMaxAmount,MandatoryTolerencePolicy,MandatoryTolerancePct,MandatoryToleranceAmt,CommitmentDeliveryIndividual,CommitmentDeliveryBulk,CommitmentDeliveryAOT,CommitmentDeliveryBulkAOT,CommitmentDeliveryLiveTrade,CommitmentDeliveryCoIssue,CommitmentDeliveryForward,CommitmentExceedPolicy,CommitmentMessage,ResetLimitForRatesheetId,CommitmentExceedTradePolicy from externalOrgDetail where @ParentId = 0 AND externalOrgID = @oid";
    }

    private static string getCustomFieldsDetailsQuery()
    {
      return "Select FieldID, OwnerID, FieldValue, Label, FieldType, LoanFieldId  from [TpoCustomField] tcf Inner Join[CustomFields] cf on tcf.FieldID = cf.LabelID where OrgId = CASE WHEN @InheritCustomfields = 1 THEN @ParentId ELSE @oid END";
    }

    private static string getTpoConnectSetupQuery()
    {
      return "SELECT * FROM [ExternalOrgURLs] a inner join[ExternalOrgSelectedURL] b on a.[UrlID] = b.[urlID]  WHERE b.[externalOrgID] = @oid ORDER BY[AddedDateTime]";
    }

    private static string getExternalorgSalesRepsForCompanyQuery()
    {
      return "SELECT distinct salesRepId,esr.externalOrgID,esr.UserID,CompanyLegalName,OrganizationName, usr.FirstLastName, usr.jobtitle AS Title, usr.phone, usr.email, CASE WHEN esr.UserID = eod.PrimarySalesRepUserId THEN 'true' ELSE 'false' END AS isPrimarySalesRep, esr.WholesaleChannel, esr.DelegatedChannel, esr.NonDelegatedChannel FROM [ExternalOrgSalesReps] esr inner join [ExternalOriginatorManagement] eom on eom.oid = esr.externalOrgID INNER JOIN [users] usr ON usr.userid = esr.UserID INNER JOIN [ExternalOrgDetail] eod ON eod.externalOrgID = eom.oid where esr.externalOrgID =@companyOid OR esr.externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =@companyOid) or esr.externalOrgID = @companyOid";
    }

    private static string getDBADetailsQuery()
    {
      return "Select * from ExternalOrgDBANames where[ExternalOrgID] = @oid order by sortIndex asc";
    }

    private static string getLoanCriteriaQuery()
    {
      return "Select * from [ExternalOrgLoanTypes] where[ExternalOrgID] = @oid";
    }

    private static string getLoanTypesChannelsQuery()
    {
      return "Select * from [ExternalOrgLoanTypesChannels] where[ExternalOrgID] = @oid";
    }

    public static List<ExternalOrgLenderContact> GetExternalOrgSalesRepContactsForCompany(
      int companyOrgId)
    {
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.GetExternalOrgSalesRepContactsForCompany: Creating SQL commands for table ExternalOrgSalesReps.");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select sr.salesRepId, sr.DelegatedChannel, sr.NonDelegatedChannel, sr.WholesaleChannel, sr.Title, sr.UserID, sr.Name, sr.phone, sr.email, sr.externalOrgID, sr.isPrimarySales, eoclc.Hide, eoclc.DisplayOrder FROM (SELECT DISTINCT salesRepId, esr.DelegatedChannel, esr.NonDelegatedChannel, esr.WholesaleChannel, esr.externalOrgID, u.UserID, u.jobtitle AS Title, u.phone, u.FirstLastName as Name, u.email, CASE WHEN eod.PrimarySalesRepUserId = u.userid THEN 1 ELSE 0 END AS isPrimarySales FROM [ExternalOrgSalesReps] esr INNER JOIN users u ON u.userid = esr.UserID INNER JOIN ExternalOriginatorManagement eom ON eom.oid = esr.externalOrgID INNER JOIN ExternalOrgDetail eod ON eod.externalOrgID = eom.oid WHERE esr.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyOrgId) + " OR esr.externalOrgID IN (SELECT descendent FROM [ExternalOrgDescendents] WHERE oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyOrgId) + " OR esr.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyOrgId) + ")) AS sr LEFT JOIN ExternalOrgCompanyLenderContacts eoclc ON eoclc.ContactId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) companyOrgId) + " AND sr.salesRepId = eoclc.ContactId AND eoclc.ContactSource = 1 ");
      try
      {
        Dictionary<string, ExternalOrgLenderContact> dictionary = new Dictionary<string, ExternalOrgLenderContact>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            string str = Convert.ToString(dataRow["Title"]);
            if (str.Length > 30)
              str = str.Substring(0, 30);
            ExternalOrgLenderContact orgLenderContact = new ExternalOrgLenderContact()
            {
              ContactID = Convert.ToInt32(dataRow["salesRepId"]),
              ExternalOrgID = new int?(companyOrgId),
              UserID = Convert.ToString(dataRow["UserID"]),
              Name = Convert.ToString(dataRow["Name"]),
              Title = str,
              Phone = Convert.ToString(dataRow["phone"]),
              Email = Convert.ToString(dataRow["email"]),
              isWholesaleChannelEnabled = Convert.ToBoolean(dataRow["WholesaleChannel"]),
              isDelegatedChannelEnabled = Convert.ToBoolean(dataRow["DelegatedChannel"]),
              isNonDelegatedChannelEnabled = Convert.ToBoolean(dataRow["NonDelegatedChannel"]),
              DisplayOrder = dataRow.IsNull("DisplayOrder") ? -1 : Convert.ToInt32(dataRow["DisplayOrder"]),
              Source = ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps,
              isHidden = !dataRow.IsNull("Hide") && Convert.ToBoolean(dataRow["Hide"]),
              isPrimarySalesRep = !dataRow.IsNull("isPrimarySales") && Convert.ToBoolean(dataRow["isPrimarySales"])
            };
            if (string.IsNullOrWhiteSpace(orgLenderContact.UserID) || !dictionary.ContainsKey(orgLenderContact.UserID))
              dictionary.Add(orgLenderContact.UserID, orgLenderContact);
            else if (dictionary.ContainsKey(orgLenderContact.UserID))
            {
              dictionary[orgLenderContact.UserID].isWholesaleChannelEnabled |= orgLenderContact.isWholesaleChannelEnabled;
              dictionary[orgLenderContact.UserID].isNonDelegatedChannelEnabled |= orgLenderContact.isNonDelegatedChannelEnabled;
              dictionary[orgLenderContact.UserID].isDelegatedChannelEnabled |= orgLenderContact.isDelegatedChannelEnabled;
              dictionary[orgLenderContact.UserID].isHidden |= orgLenderContact.isHidden;
              dictionary[orgLenderContact.UserID].isPrimarySalesRep |= orgLenderContact.isPrimarySalesRep;
            }
          }
        }
        return dictionary.Values.ToList<ExternalOrgLenderContact>();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSalesRepsForCompany: Error getting external sales reps for the current organization.\r\n" + ex.Message);
      }
    }

    public static bool AddExternalOrganizationSalesReps(ExternalOrgSalesRep[] newReps)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalOrganizationNotes: Creating SQL commands for table ExternalOrgSalesReps.");
      for (int index = 0; index < newReps.Length; ++index)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.Declare("@oid", "int");
        dbQueryBuilder2.SelectVar("@oid", (object) newReps[index].externalOrgId);
        dbQueryBuilder2.AppendLine("Select * from  [ExternalOrgSalesReps] where [externalOrgID] = @oid and [UserID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newReps[index].userId));
        if (dbQueryBuilder2.Execute().Count == 0)
        {
          dbQueryBuilder1.Declare("@oid", "int");
          dbQueryBuilder1.SelectVar("@oid", (object) newReps[index].externalOrgId);
          dbQueryBuilder1.AppendLine(ExternalOrgManagementAccessor.AddExternalOrganizationSalesRepQuery(newReps[index]));
        }
      }
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder1.ToString());
        if (dbQueryBuilder1.ToString() != "")
          dbQueryBuilder1.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrganizationSalesReps: Cannot insert record to table 'ExternalOrgSalesReps' due to the following problem:\r\n" + ex.Message);
      }
    }

    private static string AddExternalOrganizationSalesRepQuery(ExternalOrgSalesRep newRep)
    {
      string[] strArray = new string[9];
      strArray[0] = "INSERT INTO [ExternalOrgSalesReps] ([externalOrgID], [UserID], [WholesaleChannel], [DelegatedChannel], [NonDelegatedChannel]) VALUES (@oid,";
      strArray[1] = EllieMae.EMLite.DataAccess.SQL.Encode((object) newRep.userId);
      strArray[2] = ",";
      int num = newRep.isWholesaleChannelEnabled ? 1 : 0;
      strArray[3] = num.ToString();
      strArray[4] = ", ";
      num = newRep.isDelegatedChannelEnabled ? 1 : 0;
      strArray[5] = num.ToString();
      strArray[6] = ", ";
      num = newRep.isNonDelegatedChannelEnabled ? 1 : 0;
      strArray[7] = num.ToString();
      strArray[8] = ")";
      return string.Concat(strArray);
    }

    public static bool UpdateExternalOrganizationSalesRep(ExternalOrgSalesRep salesRep)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        stringBuilder.AppendLine("UPDATE ExternalOrgSalesReps");
        stringBuilder.AppendLine("     SET ExternalOrgID = " + (object) salesRep.externalOrgId + ", ");
        stringBuilder.AppendLine("     UserID = '" + EllieMae.EMLite.DataAccess.SQL.EncodeString(salesRep.userId, false) + "', ");
        stringBuilder.AppendLine("     WholesaleChannel = " + (salesRep.isWholesaleChannelEnabled ? (object) "1" : (object) "0").ToString() + ", ");
        stringBuilder.AppendLine("     DelegatedChannel = " + (salesRep.isDelegatedChannelEnabled ? (object) "1" : (object) "0").ToString() + ", ");
        stringBuilder.AppendLine("     NonDelegatedChannel = " + (salesRep.isNonDelegatedChannelEnabled ? (object) "1" : (object) "0").ToString());
        stringBuilder.AppendLine("     WHERE salesRepId = " + (object) salesRep.salesRepId);
        stringBuilder.AppendLine(" select @@rowcount");
        dbQueryBuilder.Append(stringBuilder.ToString());
        if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) <= 0)
          throw new Exception(string.Format("No entry for salesRep, \"{0}\"", (object) salesRep.salesRepId));
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrganizationSalesReps: Cannot insert record to table 'ExternalOrgSalesReps' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static bool DeleteExternalOrganizationSalesReps(int oid, string[] userIDs)
    {
      string str = string.Empty;
      for (int index = 0; index < userIDs.Length; ++index)
        str = str + (str != string.Empty ? "," : "") + "'" + userIDs[index] + "'";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE from [ExternalOrgSalesReps] WHERE [externalOrgID] = " + (object) oid + " AND [UserID] in (" + str + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrganizationSalesReps: Cannot delete record from [ExternalOrgSalesReps] due to this error: " + ex.Message);
      }
    }

    public static string SetSalesRepAsPrimaryQuery(
      string userId,
      DateTime primarySalesRepAssignedDate = default (DateTime))
    {
      return "UPDATE [ExternalOrgDetail] SET PrimarySalesRepUserId =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId) + ",[PrimarySalesRepAssignedDate] = " + (primarySalesRepAssignedDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) primarySalesRepAssignedDate) : "NULL") + " WHERE externalOrgId =@oid";
    }

    public static void SetSalesRepAsPrimary(
      string userId,
      int externalOrgId,
      DateTime primarySalesRepAssignedDate = default (DateTime))
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgId);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SetSalesRepAsPrimaryQuery(userId, primarySalesRepAssignedDate));
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetSalesRepAsPrimary: Cannot set as primary rep due to this error: " + ex.Message);
      }
    }

    public static string GetPrimarySalesRep(int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT PrimarySalesRepUserId FROM [ExternalOrgDetail] WHERE externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0 && dataRowCollection[0]["PrimarySalesRepUserId"] != DBNull.Value ? Convert.ToString(dataRowCollection[0]["PrimarySalesRepUserId"]) : (string) null;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetPrimarySalesRep: Error getting primary sales rep.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfSalesRepHasAnyContacts(string userId, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (externalOrgId == -1)
        dbQueryBuilder1.AppendLine("SELECT COUNT(*) FROM [ExternalUsers]  WHERE Sales_rep_userid= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId));
      else
        dbQueryBuilder1.AppendLine("SELECT COUNT(*) FROM [ExternalUsers]  WHERE Sales_rep_userid= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId) + " AND (externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + " OR externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + "))");
      try
      {
        DataRowCollection dataRowCollection1 = dbQueryBuilder1.Execute();
        if (dataRowCollection1 != null && dataRowCollection1.Count > 0 && Convert.ToInt32(dataRowCollection1[0][0]) > 0)
          return true;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (externalOrgId == -1)
          dbQueryBuilder2.AppendLine("SELECT COUNT(*) FROM [ExternalOrgDetail]  WHERE PrimarySalesRepUserId= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId));
        else
          dbQueryBuilder2.AppendLine("SELECT COUNT(*) FROM [ExternalOrgDetail]  WHERE PrimarySalesRepUserId= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId) + " AND (externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + "))");
        DataRowCollection dataRowCollection2 = dbQueryBuilder2.Execute();
        return dataRowCollection2 != null && dataRowCollection2.Count > 0 && Convert.ToInt32(dataRowCollection2[0][0]) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetPrimarySalesRep: Error getting primary sales rep.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfSalesRepIsPrimary(string userId, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (externalOrgId == -1)
          dbQueryBuilder2.AppendLine("SELECT COUNT(*) FROM [ExternalOrgDetail]  WHERE PrimarySalesRepUserId= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId));
        else
          dbQueryBuilder2.AppendLine("SELECT COUNT(*) FROM [ExternalOrgDetail]  WHERE PrimarySalesRepUserId= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId) + " AND (externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + "))");
        DataRowCollection dataRowCollection = dbQueryBuilder2.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0 && Convert.ToInt32(dataRowCollection[0][0]) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfSalesRepIsPrimary: Error getting if sales rep is primary for any sub organization.\r\n" + ex.Message);
      }
    }

    public static void ChangeSalesRepForContacts(
      string existingSalesRepUserId,
      string newSalesRepUserId,
      int externalOrgId,
      DateTime primarySalesRepAeAssignedDate = default (DateTime))
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder1.AppendLine("UPDATE [ExternalUsers] SET Sales_rep_userid =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(newSalesRepUserId) + " WHERE Sales_rep_userid= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(existingSalesRepUserId) + " AND (externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + " OR externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + "))");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder1.ToString());
        dbQueryBuilder1.ExecuteNonQuery(DbTransactionType.Serialized);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.AppendLine("UPDATE [ExternalOrgDetail] SET PrimarySalesRepUserId =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(newSalesRepUserId) + ",[PrimarySalesRepAssignedDate] = " + (primarySalesRepAeAssignedDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) primarySalesRepAeAssignedDate) : "NULL") + " WHERE PrimarySalesRepUserId= " + EllieMae.EMLite.DataAccess.SQL.EncodeString(existingSalesRepUserId) + " AND (externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + " OR externalOrgID in (select descendent from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + "))");
        dbQueryBuilder2.ExecuteNonQuery(DbTransactionType.Serialized);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("ChangeSalesRepForContacts: Cannot change sales reps for the contacts due to this error: " + ex.Message);
      }
    }

    private static string GetSalesRepIdByUserIdQuery(string userId, string rootOrgId)
    {
      string str = rootOrgId ?? "@oid";
      return "select @ContactId = salesRepId from ExternalOrgSalesReps where userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " and externalOrgID = " + str;
    }

    public static List<ExternalSettingType> GetExternalOrgSettingTypes()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT settingTypeId,settingTypeKey,settingTypeText FROM [ExternalOrgSettingTypes] WHERE isActive = 1");
      try
      {
        List<ExternalSettingType> externalOrgSettingTypes = new List<ExternalSettingType>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            externalOrgSettingTypes.Add(new ExternalSettingType(Convert.ToInt32(dataRow["settingTypeId"]), Convert.ToString(dataRow["settingTypeKey"]), Convert.ToString(dataRow["settingTypeText"])));
        }
        return externalOrgSettingTypes;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSettingTypes: Error getting external setting types.\r\n" + ex.Message);
      }
    }

    public static List<ExternalSettingValue> GetExternalOrgSettingsByType(int settingTypeId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT settingId,settingTypeId,settingCode, settingValue,SortId FROM [ExternalOrgSettingValues] WHERE settingTypeId=" + (object) settingTypeId + " ORDER BY SortId ");
      try
      {
        List<ExternalSettingValue> orgSettingsByType = new List<ExternalSettingValue>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            orgSettingsByType.Add(new ExternalSettingValue(Convert.ToInt32(dataRow["settingId"]), Convert.ToInt32(dataRow[nameof (settingTypeId)]), Convert.ToString(dataRow["settingCode"]), Convert.ToString(dataRow["settingValue"]), Convert.ToInt32(dataRow["SortId"])));
        }
        return orgSettingsByType;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSettingsByType: Error getting external setting values.\r\n" + ex.Message);
      }
    }

    public static List<ExternalSettingValue> GetExternalOrgSettingsByName(string settingName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT settingId,esv.settingTypeId,settingCode,settingValue,SortId FROM [ExternalOrgSettingValues] esv inner join [ExternalOrgSettingTypes] est on esv.settingTypeId = est.settingTypeId   WHERE est.IsActive= 1 AND est.settingTypeText= '" + settingName + "' ORDER BY SortId ");
      try
      {
        List<ExternalSettingValue> orgSettingsByName = new List<ExternalSettingValue>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            orgSettingsByName.Add(new ExternalSettingValue(Convert.ToInt32(dataRow["settingId"]), Convert.ToInt32(dataRow["settingTypeId"]), Convert.ToString(dataRow["settingCode"]), Convert.ToString(dataRow["settingValue"]), Convert.ToInt32(dataRow["SortId"])));
        }
        return orgSettingsByName;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSettingsByName: Error getting external setting values.\r\n" + ex.Message);
      }
    }

    public static ExternalSettingValue GetExternalOrgSettingsByID(int settingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgSettingValues]  WHERE settingId= " + (object) settingId);
      try
      {
        ExternalSettingValue externalOrgSettingsById = (ExternalSettingValue) null;
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          externalOrgSettingsById = new ExternalSettingValue(Convert.ToInt32(dataRowCollection[0][nameof (settingId)]), Convert.ToInt32(dataRowCollection[0]["settingTypeId"]), Convert.ToString(dataRowCollection[0]["settingCode"]), Convert.ToString(dataRowCollection[0]["settingValue"]), Convert.ToInt32(dataRowCollection[0]["SortId"]));
        return externalOrgSettingsById;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSettingsByID: Error getting external setting values.\r\n" + ex.Message);
      }
    }

    public static Dictionary<string, List<ExternalSettingValue>> GetExternalOrgSettings()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT settingId,esv.settingTypeId,est.settingTypeText,settingCode,settingValue,SortId FROM [ExternalOrgSettingValues] esv inner join [ExternalOrgSettingTypes] est on esv.settingTypeId = est.settingTypeId WHERE est.IsActive = 1 ORDER BY SortId ");
      try
      {
        Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = new Dictionary<string, List<ExternalSettingValue>>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            if (externalOrgSettings.ContainsKey(Convert.ToString(dataRow["settingTypeText"])))
            {
              externalOrgSettings[Convert.ToString(dataRow["settingTypeText"])].Add(new ExternalSettingValue(Convert.ToInt32(dataRow["settingId"]), Convert.ToInt32(dataRow["settingTypeId"]), Convert.ToString(dataRow["settingCode"]), Convert.ToString(dataRow["settingValue"]), Convert.ToInt32(dataRow["SortId"])));
            }
            else
            {
              externalOrgSettings[Convert.ToString(dataRow["settingTypeText"])] = new List<ExternalSettingValue>();
              externalOrgSettings[Convert.ToString(dataRow["settingTypeText"])].Add(new ExternalSettingValue(Convert.ToInt32(dataRow["settingId"]), Convert.ToInt32(dataRow["settingTypeId"]), Convert.ToString(dataRow["settingCode"]), Convert.ToString(dataRow["settingValue"]), Convert.ToInt32(dataRow["SortId"])));
            }
          }
        }
        return externalOrgSettings;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgSettings: Error getting external setting values.\r\n" + ex.Message);
      }
    }

    public static int AddExternalOrgSettingValue(ExternalSettingValue externalSettingValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@settingId", "int");
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalOrgSettingValue: Creating SQL commands for table ExternalSettingValues.");
      dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgSettingValues] ([settingTypeId],[SettingCode], [SettingValue], [SortId]) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingTypeId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingCode) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingValue) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.sortId) + ")");
      dbQueryBuilder.SelectIdentity("@settingId");
      dbQueryBuilder.Select("@settingId");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrgSettingValue: Cannot insert setting value to table 'ExternalSettingValues' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static bool DeleteExternalOrgSettingValues(string settingIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Append("select * from [ExternalOrgSettingValues] where settingId in(" + settingIds + ")");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        List<ExternalSettingValue> externalSettingValueList = new List<ExternalSettingValue>();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            ExternalSettingValue externalSettingValue = new ExternalSettingValue(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["settingId"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["settingTypeid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SettingCode"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SettingValue"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["sortId"]));
            externalSettingValueList.Add(externalSettingValue);
          }
        }
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("DELETE from [ExternalOrgSettingValues] WHERE settingId in (" + settingIds + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        foreach (ExternalSettingValue externalSettingValue in externalSettingValueList)
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("UPDATE [ExternalOrgDetail] SET [EPPSPriceGroup] = '' where [EPPSPriceGroup] =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalSettingValue.settingCode));
          dbQueryBuilder.ExecuteNonQuery();
        }
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrgSettingValues: Cannot delete record in [ExternalOrgSettingValues] due to this error: " + ex.Message);
      }
    }

    public static void UpdateExternalOrgSettingValue(ExternalSettingValue externalSettingValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Append("select * from [ExternalOrgSettingValues] where settingId =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingId));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        string str = "";
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            str = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SettingCode"]);
        }
        string text = "UPDATE [ExternalOrgSettingValues] SET [SettingCode] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingCode) + ",[SettingValue] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingValue) + " WHERE settingId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalSettingValue.settingId);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        if (!(externalSettingValue.settingCode != string.Empty) || !(str != string.Empty))
          return;
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append("UPDATE [ExternalOrgDetail] SET [EPPSPriceGroup] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalSettingValue.settingCode) + "where [EPPSPriceGroup] =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(str));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgSettingValue: Cannot update record in ExternalSettingValues table.\r\n" + ex.Message);
      }
    }

    public static void ChangeSettingSortId(
      ExternalSettingValue oldSetting,
      ExternalSettingValue newSetting)
    {
      List<string> stringList = new List<string>();
      List<int> intList = new List<int>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (oldSetting.sortId > newSetting.sortId)
      {
        dbQueryBuilder.AppendLine("SELECT settingId, SortId from ExternalOrgSettingValues WHERE SettingTypeId = " + (object) oldSetting.settingTypeId + "AND SortId BETWEEN " + (object) newSetting.sortId + " AND " + (object) oldSetting.sortId + " ORDER BY SortId");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          stringList.Add(string.Concat(dataRow["settingId"]));
          intList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["SortId"]));
        }
        dbQueryBuilder.Reset();
        for (int index = 0; index < stringList.Count - 1; ++index)
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgSettingValues SET SortId = " + (object) intList[index + 1] + " WHERE settingId = '" + stringList[index] + "'");
        dbQueryBuilder.AppendLine("UPDATE ExternalOrgSettingValues SET SortId = " + (object) intList[0] + " WHERE settingId = '" + stringList[stringList.Count - 1] + "'");
      }
      else
      {
        if (newSetting.sortId <= oldSetting.sortId)
          return;
        dbQueryBuilder.AppendLine("SELECT settingId, SortId from ExternalOrgSettingValues WHERE  SettingTypeId = " + (object) oldSetting.settingTypeId + "AND SortId BETWEEN " + (object) oldSetting.sortId + " AND " + (object) newSetting.sortId + " ORDER BY SortId");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          stringList.Add(string.Concat(dataRow["settingId"]));
          intList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["SortId"]));
        }
        dbQueryBuilder.Reset();
        for (int index = 0; index < stringList.Count - 1; ++index)
          dbQueryBuilder.AppendLine("UPDATE ExternalOrgSettingValues SET SortId = " + (object) intList[index] + " WHERE settingId = '" + stringList[index + 1] + "'");
        dbQueryBuilder.AppendLine("UPDATE ExternalOrgSettingValues SET SortId = " + (object) intList[intList.Count - 1] + " WHERE settingId = '" + stringList[0] + "'");
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationsBySettingId(
      int settingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> oids = new List<int>();
      dbQueryBuilder.AppendLine("Select distinct externalOrgID from ExternalOrgDetail where CurrentApprovalStatus = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " OR CompanyRating = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalOriginatorManagementData>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          oids.Add(Utils.ParseInt((object) string.Concat(row[0])));
        return ExternalOrgManagementAccessor.GetExternalOrganizations(false, oids);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from GetExternalOrganizationsBySettingId.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalContactsBySettingId(int settingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> contactsBySettingId = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] where Approval_status = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            contactsBySettingId.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return contactsBySettingId;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from GetExternalContactsBySettingId.\r\n" + ex.Message);
      }
    }

    public static void AssignNewSettingValueToExternalOrg(
      int settingId,
      int settingTypeId,
      string oids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = (string) null;
      try
      {
        switch (settingTypeId)
        {
          case 1:
            text = "UPDATE [ExternalOrgDetail] SET [CurrentApprovalStatus] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " WHERE externalOrgID in (" + oids + ")";
            break;
          case 3:
            text = "UPDATE [ExternalOrgDetail] SET [CompanyRating] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " WHERE externalOrgID in (" + oids + ")";
            break;
          case 5:
            text = "UPDATE [ExternalOrgDetail] SET [EPPSPriceGroup] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " WHERE externalOrgID in (" + oids + ")";
            break;
          case 7:
            text = "UPDATE [ExternalOrgDetail] SET [EPPSRateSheet] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " WHERE externalOrgID in (" + oids + ")";
            break;
        }
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignNewSettingValueToExternalOrg: Cannot update record in ExternalSettingValues table.\r\n" + ex.Message);
      }
    }

    public static void AssignNewSettingValueToContact(int settingId, string contactIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [ExternalUsers] SET [Approval_status] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId) + " WHERE externalOrgID in (" + contactIds + ")";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignNewSettingValueToExternalOrg: Cannot update record in ExternalSettingValues table.\r\n" + ex.Message);
      }
    }

    public static void AssignNewSettingValueToAttachments(int settingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [ExternalOrgAttachment] SET [Category] = -1 WHERE Category = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignNewSettingValueToAttachment: Cannot update record in ExternalOrgAttachment table.\r\n" + ex.Message);
      }
    }

    public static bool CheckIfAttachmentWithCategoryExists(int settingId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT COUNT(*) FROM [ExternalOrgAttachment]  WHERE Category= " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0 && Convert.ToInt32(dataRowCollection[0][0]) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfAttachmentWithCategoryExists: Error checking if attachment with category exists.\r\n" + ex.Message);
      }
    }

    [PgReady]
    public static ExternalOrgURL[] GetExternalOrganizationURLs()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgURLs] ORDER BY [AddedDateTime]");
        try
        {
          List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
          DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              externalOrgUrlList.Add(new ExternalOrgURL()
              {
                URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["urlID"]),
                ExternalOrgID = -1,
                URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["URL"]),
                DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["AddedDateTime"]),
                isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isDeleted"]),
                siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SiteId"]),
                TPOAdminLinkAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["TPOAdminLinkAccess"])
              });
          }
          return externalOrgUrlList.ToArray();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgURLs]  ORDER BY [AddedDateTime]");
        try
        {
          List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              externalOrgUrlList.Add(new ExternalOrgURL()
              {
                URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["urlID"]),
                ExternalOrgID = -1,
                URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["URL"]),
                DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["AddedDateTime"]),
                isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isDeleted"]),
                siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SiteId"]),
                TPOAdminLinkAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["TPOAdminLinkAccess"])
              });
          }
          return externalOrgUrlList.ToArray();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
    }

    public static ExternalOrgURL GetExternalOrganizationURLbySiteID(string siteID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgURLs]  where SiteID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) siteID));
      try
      {
        List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          IEnumerator enumerator = dataRowCollection.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
            {
              DataRow current = (DataRow) enumerator.Current;
              return new ExternalOrgURL()
              {
                URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(current["urlID"]),
                ExternalOrgID = -1,
                URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(current["URL"]),
                DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(current["AddedDateTime"]),
                isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(current["isDeleted"]),
                siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(current["SiteId"]),
                TPOAdminLinkAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(current["TPOAdminLinkAccess"])
              };
            }
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        }
        return (ExternalOrgURL) null;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationURLbySiteID: Cannot fetch a record from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOrgURL> GetSelectedOrgUrls(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getTpoConnectSetupQuery());
      try
      {
        List<ExternalOrgURL> source = new List<ExternalOrgURL>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow rows in (InternalDataCollectionBase) dataRowCollection)
            source.Add(ExternalOrgManagementAccessor.getExternalTpoConnectFromDatarow(rows));
        }
        return source.ToList<ExternalOrgURL>();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOrgURL> GetExternalOrganizationURLBySalesRep(string userId)
    {
      List<ExternalOrgURL> organizationUrlBySalesRep = new List<ExternalOrgURL>();
      if (!string.IsNullOrEmpty(userId))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgURLs] extOrgURLs INNER JOIN [ExternalOrgSelectedURL] extOrgSelURL ON extOrgURLs.[UrlID] = extOrgSelURL.[urlID] INNER JOIN [ExternalOrgSalesReps] extOrgSalesReps ON extOrgSelURL.[externalOrgID] = extOrgSalesReps.[externalOrgID] WHERE extOrgSalesReps.[UserID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userId));
        try
        {
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null)
          {
            if (dataRowCollection.Count > 0)
            {
              foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
                organizationUrlBySalesRep.Add(new ExternalOrgURL()
                {
                  URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["urlID"]),
                  ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["externalOrgID"]),
                  URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["URL"]),
                  DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["AddedDateTime"]),
                  isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isDeleted"]),
                  EntityType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["UrlEntityType"]),
                  siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SiteId"]),
                  TPOAdminLinkAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["TPOAdminLinkAccess"])
                });
            }
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalOrganizationURLBySalesRep: Cannot fetch records from [ExternalOrgURLs] table for salesRep '" + userId + "'.\r\n" + ex.Message);
        }
      }
      return organizationUrlBySalesRep;
    }

    public static void UpdateExternalOrganizationSelectedURLs(
      int oid,
      List<ExternalOrgURL> orgUrl,
      int root)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int[] array = orgUrl.Select<ExternalOrgURL, int>((System.Func<ExternalOrgURL, int>) (a => a.URLID)).ToArray<int>();
      string str = string.Join<int>(",", (IEnumerable<int>) array);
      if (((IEnumerable<int>) array).Any<int>())
        dbQueryBuilder.AppendLine("select * from [ExternalOrgSelectedURL] where [externalOrgID] = " + (object) oid + " and [UrlID] not in (" + str + ")");
      else
        dbQueryBuilder.AppendLine("select * from [ExternalOrgSelectedURL] where [externalOrgID] = " + (object) oid);
      List<int> intList = new List<int>();
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          intList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["urlId"]));
      }
      dbQueryBuilder.Reset();
      if (((IEnumerable<int>) array).Any<int>())
        dbQueryBuilder.AppendLine("Delete [ExternalOrgSelectedURL] where [externalOrgID] = " + (object) oid + " and [UrlID] not in (" + str + ")");
      else
        dbQueryBuilder.AppendLine("Delete [ExternalOrgSelectedURL] where [externalOrgID] = " + (object) oid);
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) oid);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExternalOrganizationSelectedURLsQuery(orgUrl));
        dbQueryBuilder.ExecuteNonQuery();
        if (intList == null || !intList.Any<int>())
          return;
        ExternalOrgManagementAccessor.DeleteFromExternalOrgURLs(intList, oid, root);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrganizationSelectedURLs: Cannot update records from [ExternalOrgSelectedURL] table.\r\n" + ex.Message);
      }
    }

    private static string InsertExternalOrganizationSelectedURLsQuery(List<ExternalOrgURL> orgUrl)
    {
      string str1 = "";
      foreach (ExternalOrgURL externalOrgUrl in orgUrl)
      {
        string str2 = "if not exists (SELECT * FROM [ExternalOrgSelectedURL] WHERE [externalOrgID] = @oid and [UrlID] = " + (object) externalOrgUrl.URLID + ")begin Insert into [ExternalOrgSelectedURL] (externalOrgID, UrlID,[UrlEntityType]) values (@oid, " + (object) externalOrgUrl.URLID + "," + (object) externalOrgUrl.EntityType + ") end else Begin Update  [ExternalOrgSelectedURL] set [UrlEntityType] = " + (object) externalOrgUrl.EntityType + " where externalOrgID = @oid and UrlID=" + (object) externalOrgUrl.URLID + " end ";
        str1 += str2;
      }
      return str1;
    }

    public static void DeleteFromExternalOrgURLs(List<int> UrlsTobeDeleted, int oid, int root)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = string.Join<int>(",", (IEnumerable<int>) UrlsTobeDeleted);
      dbQueryBuilder.AppendLine("select * from [ExternalOrgSelectedURL] a inner join [ExternalOrgURLs] b on a.Urlid = b.urlid where  b.[UrlID] in (" + str + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        return;
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("Delete [ExternalOrgURLs] where  [UrlID] in (" + str + ") and isDeleted = 1");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static IEnumerable<ExternalOrgURL> UpsertExternalOrgURLs(
      int extOrgId,
      IEnumerable<ExternalOrgURL> orgUrlList,
      bool returnData,
      bool isUpdate)
    {
      if (extOrgId <= 0 || orgUrlList != null && !orgUrlList.Any<ExternalOrgURL>())
        return Enumerable.Empty<ExternalOrgURL>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) extOrgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpsertExternalOrgURLsQuery(extOrgId, orgUrlList, cmdParams, new bool?(true), new bool?(isUpdate)));
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        DataSet dataSet = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd);
        List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          externalOrgUrlList.Add(ExternalOrgManagementAccessor.getExternalURLFromDatarow(row, returnData));
        return (IEnumerable<ExternalOrgURL>) externalOrgUrlList;
      }
    }

    private static string UpsertExternalOrgURLsQuery(
      int extOrgId,
      IEnumerable<ExternalOrgURL> orgUrlList,
      List<SqlParameter> cmdParams,
      bool? returnData = null,
      bool? isUpdate = null)
    {
      if ((orgUrlList != null ? (!orgUrlList.Any<ExternalOrgURL>() ? 1 : 0) : 1) != 0)
        return string.Empty;
      cmdParams.Add(new SqlParameter("@ExternalOrgURLs", (object) ExternalOrgManagementAccessor.GetExternalOrgURLsUpsertDataTable(extOrgId, orgUrlList))
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_ExternalOrgURLs"
      });
      if (returnData.HasValue)
        cmdParams.Add(new SqlParameter("@ReturnExternalOrgURLData", (object) returnData.Value));
      else
        cmdParams.Add(new SqlParameter("@ReturnExternalOrgURLData", (object) DBNull.Value));
      cmdParams.Add(new SqlParameter("@SyncChildExternalOrgURLs", (object) isUpdate));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("EXEC UpsertExternalOrgURLs @ExternalOrgURLs, @oid, @ReturnExternalOrgURLData, @SyncChildExternalOrgURLs");
      return dbQueryBuilder.ToString();
    }

    private static DataTable GetExternalOrgURLsUpsertDataTable(
      int extOrgId,
      IEnumerable<ExternalOrgURL> orgUrlList)
    {
      DataTable lsUpsertDataTable = new DataTable();
      lsUpsertDataTable.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("Id", typeof (int)),
        new DataColumn("URLID", typeof (int)),
        new DataColumn("ExternalOrgID", typeof (int)),
        new DataColumn("EntityType", typeof (int))
      });
      foreach (ExternalOrgURL orgUrl in orgUrlList)
      {
        DataRow row = lsUpsertDataTable.NewRow();
        row["Id"] = (object) orgUrl.Id;
        row["URLID"] = (object) orgUrl.URLID;
        row["ExternalOrgID"] = (object) extOrgId;
        row["EntityType"] = (object) orgUrl.EntityType;
        lsUpsertDataTable.Rows.Add(row);
      }
      return lsUpsertDataTable;
    }

    private static ExternalOrgURL getExternalURLFromDatarow(DataRow row, bool populateDetails = true)
    {
      ExternalOrgURL externalUrlFromDatarow = new ExternalOrgURL();
      externalUrlFromDatarow.Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : -1;
      if (populateDetails)
      {
        externalUrlFromDatarow.Id = Convert.ToInt32(row["Id"]);
        externalUrlFromDatarow.ExternalOrgID = Convert.ToInt32(row["externalOrgID"]);
        externalUrlFromDatarow.EntityType = Convert.ToInt32(row["UrlEntityType"]);
        externalUrlFromDatarow.URLID = Convert.ToInt32(row["UrlId"]);
        externalUrlFromDatarow.siteId = Convert.ToString(row["SiteId"]);
        externalUrlFromDatarow.URL = Convert.ToString(row["URL"]);
      }
      return externalUrlFromDatarow;
    }

    public static void DeleteTPOCSetUpDetails(int orgId, IEnumerable<int> idList)
    {
      if (idList != null && !idList.Any<int>())
        return;
      string ids = string.Join<int>(",", idList);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.AppendLine("set @oid = " + (object) orgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SyncDeleteTPOCSetUpDetailsQuery(ids));
      dbQueryBuilder.ExecuteSetQuery();
    }

    private static string SyncDeleteTPOCSetUpDetailsQuery(string ids)
    {
      return "EXEC [DeleteExtOrgChildOrgURLs] @oid,'" + ids + "'" ?? "";
    }

    private static string UpdateIsTestAccountQuery(bool isTestAccount)
    {
      return "UPDATE [ExternalOrgDetail] SET [IsTestAccount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (isTestAccount ? 1 : 0)) + " where externalOrgID = @oid";
    }

    public static void UpdateExternalOrganizationURL(ExternalOrgURL externalOrgUrl)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [ExternalOrgURLs] set [URL] = '" + externalOrgUrl.URL + "' where [siteid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrgUrl.siteId));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrganizationURL: Cannot update records in [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalOrganizationURLTPOAccessStatus(ExternalOrgURL externalOrgUrl)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [ExternalOrgURLs] set [TPOAdminLinkAccess] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(externalOrgUrl.TPOAdminLinkAccess) + " where [siteid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrgUrl.siteId));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrganizationURLTPOAccessStatus: Cannot update records in [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static void UpdateInheritWebCenterSetupFlag(int oid, bool inheritWebCenterSetup)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [ExternalOrgDetail] set [InheritWebCenterSetup] = '" + EllieMae.EMLite.DataAccess.SQL.Encode((object) (inheritWebCenterSetup ? 1 : 0)) + "' where [externalOrgID] = " + (object) oid);
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateInheritWebCenterSetupFlag: Cannot update InheritWebCenterSetup in [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    private static string UpdateInheritCustomFieldsFlagQuery(bool inheritCustomFields)
    {
      return "Update [ExternalOrgDetail] set [InheritCustomFields] = '" + EllieMae.EMLite.DataAccess.SQL.Encode((object) (inheritCustomFields ? 1 : 0)) + "' where [externalOrgID] = @oid";
    }

    private static string DeleteCustomFieldsQuery(bool inheritCustomFields)
    {
      return "IF (select InheritCustomfields from ExternalOrgDetail where externalOrgID = @oid) != " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (inheritCustomFields ? 1 : 0)) + "  BEGIN  DELETE FROM [TpoCustomField] where OrgID = @oid  END ";
    }

    private static string InheritCustomFieldsQuery()
    {
      return "DELETE FROM [TpoCustomField] where OrgID = @oid INSERT INTO [TpoCustomField] ([OrgID],[FieldID],[OwnerID],[FieldValue]) (SELECT @oid, FieldID, OwnerID, FieldValue from  [TpoCustomField] where OrgID = (select parent from ExternalOriginatorManagement where oid = @oid ))";
    }

    public static void UpdateInheritCustomFieldsFlag(int oid, bool inheritCustomFields)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) oid);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateInheritCustomFieldsFlagQuery(inheritCustomFields));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateInheritCustomFieldsFlag: Cannot update InheritCustomFields in [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    public static ExternalOrgURL AddExternalOrganizationURL(string siteId, string url)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("if not exists (SELECT * FROM [ExternalOrgURLs] WHERE [siteId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteId) + ")");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("Insert into [ExternalOrgURLs] (siteId, URL) values (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteId) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(url) + ")");
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.ExecuteNonQuery();
        return ((IEnumerable<ExternalOrgURL>) ExternalOrgManagementAccessor.GetExternalOrganizationURLs()).FirstOrDefault<ExternalOrgURL>((System.Func<ExternalOrgURL, bool>) (x => x.URL == url));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrganizationURL: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static bool DeleteExternalOrganizationURL(string SiteId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("Select * from  [ExternalOrgURLs] a inner join [ExternalOrgSelectedURL] b on a.UrlId = b.Urlid where a.SiteId =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(SiteId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      dbQueryBuilder.Reset();
      if (dataRowCollection != null & dataRowCollection.Count > 0)
        dbQueryBuilder.AppendLine("update [ExternalOrgURLs] set isDeleted = 1 where [SiteId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(SiteId));
      else
        dbQueryBuilder.AppendLine("Delete from [ExternalOrgURLs] where [SiteId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(SiteId));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] QueryExternalUsers(QueryCriterion[] criteria)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Append("select distinct Contact.* from [ExternalUsers] Contact ");
        dbQueryBuilder.AppendLine(" where (1 = 1)");
        for (int index = 0; index < criteria.Length; ++index)
          dbQueryBuilder.AppendLine("   and (" + criteria[index].ToSQLClause() + ")");
        List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
            externalUserInfoList.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r));
        }
        return externalUserInfoList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("QueryExternalUsers: Cannot query record in ExternalUsers table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] GetExternalUserInfos(int externalOrgID, bool isKeyContact)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]  WHERE [externalOrgID] = " + (object) externalOrgID);
      if (isKeyContact)
        dbQueryBuilder.AppendLine(" and isKeyContact = 0");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
        foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
        {
          DataRow row = dataRow;
          ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>());
          List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("SELECT State, LicenseApproved, LicenseExempt, LicenseNumber,IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate FROM [ExternalUserStateLicensing] Where external_userid = '" + externalUserInfo.ExternalUserID + "'");
          foreach (DataRow row1 in (InternalDataCollectionBase) dbQueryBuilder.Execute())
            stateLicenseExtTypeList.Add(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row1, false));
          externalUserInfo.Licenses = stateLicenseExtTypeList;
          externalUserInfoList.Add(externalUserInfo);
        }
        return externalUserInfoList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfos: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] GetExternalUserInfosSummary(
      int externalOrgID,
      bool isKeyContact,
      bool getPersona = false)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, List<Persona>> dictionary = (Dictionary<string, List<Persona>>) null;
      if (getPersona)
      {
        try
        {
          dbQueryBuilder.AppendLine("SELECT eu.ContactID, p.* from Externalusers eu join UserPersona up on eu.ContactID = up.userid join Personas p on p.personaID = up.personaID where externalOrgID = " + (object) externalOrgID);
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            dictionary = new Dictionary<string, List<Persona>>();
            foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
            {
              string key = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ContactID"]);
              if (!dictionary.ContainsKey(key))
                dictionary[key] = new List<Persona>();
              dictionary[key].Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfoUserPersona(r));
            }
          }
          dbQueryBuilder.Reset();
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
        }
      }
      List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT eu.*, em.State as OrgState, em.City as OrgCity, em.Zip as OrgZip, em.Address as OrgAddress FROM [ExternalUsers] eu, ExternalOriginatorManagement em WHERE eu.externalOrgID = em.oid and eu.[externalOrgID] = " + (object) externalOrgID);
      if (isKeyContact)
        dbQueryBuilder.AppendLine(" and isKeyContact = 0");
      try
      {
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r);
          if (externalUserInfo.UseCompanyAddress)
          {
            externalUserInfo.State = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgState"]);
            externalUserInfo.City = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgCity"]);
            externalUserInfo.Zipcode = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgZip"]);
            externalUserInfo.Address = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgAddress"]);
          }
          if (dictionary != null && dictionary.ContainsKey(externalUserInfo.ContactID))
            externalUserInfo.UserPersonas = dictionary[externalUserInfo.ContactID].ToArray();
          externalUserInfoList.Add(externalUserInfo);
        }
        return externalUserInfoList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfosSummary: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] GetAllExternalUserInfos(string tpoID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
      if (tpoID == "")
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]");
      else
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] eu  where eu.externalOrgID in (select oid from ExternalOriginatorManagement where ExternalID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(tpoID) + ")");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null && rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>());
            externalUserInfoList.Add(externalUserInfo);
          }
        }
        return externalUserInfoList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalUserInfos: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message, ex);
      }
    }

    public static ExternalUserInfo[] GetAllExternalUserInfos(int oid)
    {
      return ExternalOrgManagementAccessor.getAllExternalUserInfos(oid, false);
    }

    public static ExternalUserInfo[] getAllExternalUserInfos(int oid, bool branchExcluded)
    {
      bool flag = false;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      if (branchExcluded)
      {
        if (oid == -1)
          dbQueryBuilder.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType FROM [ExternalUsers] eu inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid WHERE org.OrganizationType <= 1");
        else
          dbQueryBuilder.AppendLine("SELECT eu.*,  em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType FROM [ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID where (eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (select [descendent] from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      }
      else if (oid == -1)
      {
        dbQueryBuilder.AppendLine("SELECT eu.[contactID], eu.[external_userid],eu.[Last_name],eu.[First_name],eu.[Title],eu.[State],eu.[externalOrgID],eu.[Status],eu.[Email],eu.[Phone],eu.[Roles],eu.[Login_email], eu.[Last_Login],eu.[Approval_status],eu.[UseCompanyAddress], em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType FROM [ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID");
        flag = true;
      }
      else
        dbQueryBuilder.AppendLine("SELECT eu.*,  em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType FROM [ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID where (eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (select [descendent] from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      try
      {
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalUserInfo externalUserInfo = flag ? ExternalOrgManagementAccessor.databaseRowToExternalUserInfoAllTPO(r) : ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r);
          if (externalUserInfo.UseCompanyAddress)
            externalUserInfo.State = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgState"]);
          if (!dictionary.ContainsKey(externalUserInfo.ExternalUserID))
            dictionary.Add(externalUserInfo.ExternalUserID, externalUserInfo);
        }
        return dictionary.Values.ToArray<ExternalUserInfo>();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalUserInfos: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] getAllExternalUserInfos(
      int oid,
      bool visibleInTpowcSiteOnly,
      bool isRecursive,
      bool includeLicenseInfo,
      List<string> roleTypeList = null)
    {
      bool flag = false;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      if (!isRecursive)
      {
        if (oid == -1)
          dbQueryBuilder.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType, (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList FROM [ExternalUsers] eu inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid WHERE org.OrganizationType <= 1");
        else
          dbQueryBuilder.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType , (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList FROM [ExternalUsers] eu inner join\t[ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid  WHERE eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      }
      else if (oid == -1)
      {
        dbQueryBuilder.AppendLine("SELECT eu.[contactID], eu.[external_userid],eu.[Last_name],eu.[First_name],eu.[Title],eu.[State],eu.[externalOrgID],eu.[Status],eu.[Email],eu.[Phone],eu.[Roles],eu.[Login_email], eu.[Last_Login],eu.[Approval_status],eu.[UseCompanyAddress], em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType, (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList  FROM [ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID where 1 = 1");
        flag = true;
      }
      else
        dbQueryBuilder.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType\r\n                                , (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList\r\n                                FROM[ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid\r\n                                inner join[ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID\r\n                                where (eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (select[descendent] from[ExternalOrgDescendents] where oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      if (visibleInTpowcSiteOnly)
        dbQueryBuilder.Append(" and (org.OrganizationType in (0, 2) or em.VisibleOnTPOWCSite = 1) ");
      try
      {
        Dictionary<int, List<string>> personaRoleMappings = WorkflowBpmDbAccessor.GetAllPersonaRoleMappings();
        Persona[] allPersonas = PersonaAccessor.GetAllPersonas();
        if (roleTypeList != null && roleTypeList.Any<string>() && personaRoleMappings != null && personaRoleMappings.Any<KeyValuePair<int, List<string>>>())
        {
          IEnumerable<int> ints = personaRoleMappings.Where<KeyValuePair<int, List<string>>>((System.Func<KeyValuePair<int, List<string>>, bool>) (x => x.Value.Any<string>((System.Func<string, bool>) (r => roleTypeList.Contains(r))))).Select<KeyValuePair<int, List<string>>, int>((System.Func<KeyValuePair<int, List<string>>, int>) (x => x.Key));
          if (!ints.Any<int>())
            return dictionary.Values.ToArray<ExternalUserInfo>();
          dbQueryBuilder.Append(" and exists (select 1 from UserPersona up1 where up1.userid = eu.ContactID and up1.personaID in (" + string.Join<int>(",", ints) + "))");
        }
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalUserInfo externalUserInfo = flag ? ExternalOrgManagementAccessor.databaseRowToExternalUserInfoAllTPO(r) : ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r);
          if (externalUserInfo.UseCompanyAddress)
            externalUserInfo.State = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgState"]);
          string str1 = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["personasList"]);
          if (personaRoleMappings != null && personaRoleMappings.Any<KeyValuePair<int, List<string>>>())
          {
            HashSet<string> roleTypes = new HashSet<string>();
            List<Persona> source = new List<Persona>();
            if (!string.IsNullOrEmpty(str1))
            {
              string str2 = str1;
              char[] chArray = new char[1]{ ',' };
              foreach (string s in str2.Split(chArray))
              {
                int personaId = 0;
                if (int.TryParse(s, out personaId))
                {
                  source.Add(Array.Find<Persona>(allPersonas, (Predicate<Persona>) (p => p.ID == personaId)));
                  if (personaRoleMappings.ContainsKey(personaId) && personaRoleMappings[personaId] != null && personaRoleMappings[personaId].Any<string>())
                    personaRoleMappings[personaId].ForEach((Action<string>) (x => roleTypes.Add(x)));
                }
              }
              if (roleTypes.Any<string>())
                externalUserInfo.RoleTypes = roleTypes.ToList<string>();
              if (source.Any<Persona>())
                externalUserInfo.UserPersonas = source.ToArray();
            }
          }
          if (includeLicenseInfo)
          {
            DataTable userLicenseData = ExternalOrgManagementAccessor.GetUserLicenseData(oid, externalUserInfo.ExternalUserID);
            if (userLicenseData != null && userLicenseData.Rows.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) userLicenseData.Rows)
                externalUserInfo.Licenses.Add(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
            }
          }
          if (!dictionary.ContainsKey(externalUserInfo.ExternalUserID))
            dictionary.Add(externalUserInfo.ExternalUserID, externalUserInfo);
        }
        return dictionary.Values.ToArray<ExternalUserInfo>();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalUserInfos: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo[] getAllExternalUserInfos(
      int oid,
      string tpoId,
      bool visibleInTpowcSiteOnly,
      bool isRecursive,
      bool includeLicenseInfo,
      out int totalCount,
      List<string> roleNames = null,
      bool authorizedTradersOnly = false,
      int start = -1,
      int limit = 0)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      totalCount = 0;
      Dictionary<string, ExternalUserInfo> dictionary1 = new Dictionary<string, ExternalUserInfo>();
      if (oid == -1 && !string.IsNullOrEmpty(tpoId))
        dbQueryBuilder1.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType , (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList FROM [ExternalUsers] eu inner join\t[ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid  WHERE em.ExternalID = '" + tpoId + "'");
      else if (!isRecursive)
        dbQueryBuilder1.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType , (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList FROM [ExternalUsers] eu inner join\t[ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid  WHERE eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      else
        dbQueryBuilder1.AppendLine("SELECT eu.*, em.State as OrgState, em.VisibleOnTPOWCSite, org.OrganizationType\r\n                        , (stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) personasList\r\n                        FROM[ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid\r\n                        inner join[ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID\r\n                        where (eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (select[descendent] from[ExternalOrgDescendents] where oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      if (visibleInTpowcSiteOnly)
        dbQueryBuilder1.Append(" and (org.OrganizationType in (0, 2) or em.VisibleOnTPOWCSite = 1) ");
      try
      {
        if (authorizedTradersOnly)
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = oid != -1 || string.IsNullOrEmpty(tpoId) ? (isRecursive ? ExternalOrgManagementAccessor.GetAllAuthorizedDealersContactIdsQuery(-1, true) : ExternalOrgManagementAccessor.GetAllAuthorizedDealersContactIdsQuery(oid, true)) : ExternalOrgManagementAccessor.GetAllAuthorizedDealersContactIdsQuery(-1, true);
          if (dbQueryBuilder3 == null || !(dbQueryBuilder3.ToString() != string.Empty))
            return dictionary1.Values.ToArray<ExternalUserInfo>();
          dbQueryBuilder1.Append(" and (eu.ContactID in ( " + (object) dbQueryBuilder3 + "))");
        }
        Dictionary<int, List<string>> personaRoleMappings = WorkflowBpmDbAccessor.GetAllPersonaRoleMappings();
        Persona[] allPersonas = PersonaAccessor.GetAllPersonas();
        if (roleNames != null && roleNames.Any<string>() && personaRoleMappings != null && personaRoleMappings.Any<KeyValuePair<int, List<string>>>())
        {
          IEnumerable<int> ints = personaRoleMappings.Where<KeyValuePair<int, List<string>>>((System.Func<KeyValuePair<int, List<string>>, bool>) (x => x.Value.Any<string>((System.Func<string, bool>) (r => roleNames.Contains(r))))).Select<KeyValuePair<int, List<string>>, int>((System.Func<KeyValuePair<int, List<string>>, int>) (x => x.Key));
          if (!ints.Any<int>())
            return dictionary1.Values.ToArray<ExternalUserInfo>();
          dbQueryBuilder1.Append(" and exists (select 1 from UserPersona up1 where up1.userid = eu.ContactID and up1.personaID in (" + string.Join<int>(",", ints) + "))");
        }
        DataRowCollection rows = (DataRowCollection) null;
        bool flag = start >= 0 && limit > 0;
        if (flag)
        {
          DataTable paginatedRecords = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder1.ToString(), start + 1, start + limit, (List<SortColumn>) null);
          if (paginatedRecords != null && paginatedRecords.Rows != null)
            rows = paginatedRecords.Rows;
        }
        else
          rows = dbQueryBuilder1.Execute();
        if (rows != null && rows.Count > 0)
        {
          totalCount = !flag ? rows.Count : EllieMae.EMLite.DataAccess.SQL.DecodeInt(rows[0]["TotalRowCount"]);
          Dictionary<string, UserInfo> dictionary2 = ((IEnumerable<UserInfo>) User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows))).ToDictionary<UserInfo, string, UserInfo>((System.Func<UserInfo, string>) (u => u.Userid), (System.Func<UserInfo, UserInfo>) (u => u));
          foreach (DataRow r in (InternalDataCollectionBase) rows)
          {
            UserInfo userInfo;
            if (!dictionary2.TryGetValue(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]), out userInfo))
              userInfo = (UserInfo) null;
            ExternalUserInfo externalUserInfo1 = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, userInfo);
            if (externalUserInfo1.UseCompanyAddress)
              externalUserInfo1.State = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["OrgState"]);
            string str1 = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["personasList"]);
            if (personaRoleMappings != null && personaRoleMappings.Any<KeyValuePair<int, List<string>>>())
            {
              HashSet<string> roleTypes = new HashSet<string>();
              List<Persona> source = new List<Persona>();
              if (!string.IsNullOrEmpty(str1))
              {
                string str2 = str1;
                char[] chArray = new char[1]{ ',' };
                foreach (string s in str2.Split(chArray))
                {
                  int personaId = 0;
                  if (int.TryParse(s, out personaId))
                  {
                    source.Add(Array.Find<Persona>(allPersonas, (Predicate<Persona>) (p => p.ID == personaId)));
                    if (personaRoleMappings.ContainsKey(personaId) && personaRoleMappings[personaId] != null && personaRoleMappings[personaId].Any<string>())
                      personaRoleMappings[personaId].ForEach((Action<string>) (x => roleTypes.Add(x)));
                  }
                }
                if (roleTypes.Any<string>())
                  externalUserInfo1.RoleTypes = roleTypes.ToList<string>();
                if (source.Any<Persona>())
                  externalUserInfo1.UserPersonas = source.ToArray();
              }
            }
            if (includeLicenseInfo)
            {
              DataTable dataTable = oid != -1 || string.IsNullOrEmpty(tpoId) ? ExternalOrgManagementAccessor.GetUserLicenseData(oid, externalUserInfo1.ExternalUserID) : ExternalOrgManagementAccessor.GetUserLicenseData(tpoId, externalUserInfo1.ExternalUserID);
              if (dataTable != null && dataTable.Rows.Count > 0)
              {
                foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
                  externalUserInfo1.Licenses.Add(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
              }
            }
            ExternalUserInfo externalUserInfo2 = externalUserInfo1;
            ExternalUserURL[] externalUserInfoUrLs = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserInfo1.ExternalUserID);
            List<ExternalUserURL> list = externalUserInfoUrLs != null ? ((IEnumerable<ExternalUserURL>) externalUserInfoUrLs).ToList<ExternalUserURL>() : (List<ExternalUserURL>) null;
            externalUserInfo2.SiteURLs = list;
            if (!dictionary1.ContainsKey(externalUserInfo1.ExternalUserID))
              dictionary1.Add(externalUserInfo1.ExternalUserID, externalUserInfo1);
          }
        }
        Dictionary<string, Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>> extUsersRolesAndRoleMappings = WorkflowBpmDbAccessor.GetUsersRolesAndRoleMappings(dictionary1.Select<KeyValuePair<string, ExternalUserInfo>, string>((System.Func<KeyValuePair<string, ExternalUserInfo>, string>) (x => x.Value.ContactID)).ToList<string>());
        dictionary1.ForEach<KeyValuePair<string, ExternalUserInfo>>((Action<KeyValuePair<string, ExternalUserInfo>>) (x =>
        {
          if (!extUsersRolesAndRoleMappings.ContainsKey(x.Value.ContactID))
            return;
          x.Value.RoleSummaries = extUsersRolesAndRoleMappings[x.Value.ContactID].Item1?.ToArray();
          x.Value.RoleMappings = extUsersRolesAndRoleMappings[x.Value.ContactID].Item2?.ToArray();
        }));
        return dictionary1.Values.ToArray<ExternalUserInfo>();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalUserInfos: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    private static DataTable GetUserLicenseData(int oid, string externalUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT  eu.external_userid, eusl.State, eusl.LicenseApproved, eusl.LicenseExempt, eusl.LicenseNumber,  eusl.IssueDate, eusl.StartDate, eusl.EndDate, eusl.Status, eusl.StatusDate, eusl.LastCheckedDate  FROM [ExternalUsers] eu inner join  ExternalOriginatorManagement em on eu.externalOrgID = em.oid  inner join [ExternalUserStateLicensing] eusl on eu.external_userid = eusl.external_userid  where(eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (select [descendent] from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      dbQueryBuilder.AppendLine(" and eu.external_userid = '" + externalUserId + "'");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    private static DataTable GetUserLicenseData(string tpoId, string externalUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT  eu.external_userid, eusl.State, eusl.LicenseApproved, eusl.LicenseExempt, eusl.LicenseNumber,  eusl.IssueDate, eusl.StartDate, eusl.EndDate, eusl.Status, eusl.StatusDate, eusl.LastCheckedDate  FROM [ExternalUsers] eu inner join  ExternalOriginatorManagement em on eu.externalOrgID = em.oid  inner join [ExternalUserStateLicensing] eusl on eu.external_userid = eusl.external_userid  where(em.externalID = '" + tpoId + "')");
      dbQueryBuilder.AppendLine(" and eu.external_userid = '" + externalUserId + "'");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    public static List<ExternalUserInfo> GetExternalUserInfosForOrganization(
      int oid,
      bool isRecusrive)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> infosForOrganization = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT eu.ContactID,  em.oid, em.OrganizationName, em.Parent, em.VisibleOnTPOWCSite, org.OrganizationType, eu.First_name, eu.Last_name, eu.Middle_name\r\n                             FROM [ExternalUsers] eu inner join ExternalOriginatorManagement em on eu.externalOrgID = em.oid\r\n                                inner join [ExternalOrgDetail] org on eu.externalOrgID = org.externalOrgID\r\n                             WHERE 1 = 1 ");
      if (!isRecusrive)
        dbQueryBuilder.Append("AND eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      else
        dbQueryBuilder.Append("AND (eu.externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " OR eu.externalOrgID in (SELECT [descendent] from [ExternalOrgDescendents] where oid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + "))");
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          ExternalUserInfo externalUserInfo = new ExternalUserInfo();
          externalUserInfo.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["ContactID"]);
          externalUserInfo.ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow[nameof (oid)]);
          externalUserInfo.OrgName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["OrganizationName"]);
          externalUserInfo.RootExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["Parent"]);
          externalUserInfo.OrgVisibleOnTpowcSite = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["VisibleOnTPOWCSite"]);
          externalUserInfo.OrganizationType = (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["OrganizationType"]);
          externalUserInfo.FirstName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["First_name"]);
          externalUserInfo.LastName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Last_name"]);
          externalUserInfo.MiddleName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Middle_name"]);
          infosForOrganization.Add(externalUserInfo);
        }
        return infosForOrganization;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfosForOrganization: Cannot fetch records from [ExternalUsers] table.\r\n" + ex.Message);
      }
    }

    private static string[] getContactIdList(DataRowCollection rows)
    {
      string[] contactIdList = new string[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        contactIdList[index] = EllieMae.EMLite.DataAccess.SQL.DecodeString(rows[index]["contactID"]);
      return contactIdList;
    }

    public static int GetRootParentOrgIDforOrg(int orgId)
    {
      int parentOrgIdforOrg = 0;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DECLARE @OrgId int =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgId) + ";WITH RootPrentCTE AS\r\n                            (\r\n                             SELECT oid,Parent\r\n                             FROM [ExternalOriginatorManagement] eom WHERE oid= @OrgId\r\n                             UNION ALL\r\n                             SELECT eom.oid,eom.Parent\r\n                             FROM RootPrentCTE c INNER JOIN [ExternalOriginatorManagement] eom ON eom.oid= c.Parent\r\n                             )\r\n                             SELECT oid FROM RootPrentCTE where Parent=0");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        parentOrgIdforOrg = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0]["oid"]);
      return parentOrgIdforOrg;
    }

    public static ExternalUserInfo GetExternalUserInfo(string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalUserInfo externalUserInfo = (ExternalUserInfo) null;
      List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
      List<string> stringList = new List<string>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]  WHERE [external_userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalUserID));
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null && rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(rows[0], users[0]);
        }
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT State, LicenseApproved, LicenseExempt, LicenseNumber,IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate FROM [ExternalUserStateLicensing] Where external_userid = '" + externalUserID + "'");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          stateLicenseExtTypeList.Add(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
        if ((UserInfo) externalUserInfo != (UserInfo) null)
        {
          externalUserInfo.RootExternalOrgID = ExternalOrgManagementAccessor.GetRootParentOrgIDforOrg(externalUserInfo.ExternalOrgID);
          externalUserInfo.Licenses = stateLicenseExtTypeList;
          if (externalUserInfo.UseCompanyAddress || externalUserInfo.UseParentInfoForRateLock)
          {
            ExternalOriginatorManagementData externalOrganization = ExternalOrgManagementAccessor.GetExternalOrganization(false, externalUserInfo.ExternalOrgID);
            if (externalUserInfo.UseCompanyAddress)
            {
              externalUserInfo.Address = externalOrganization.Address;
              externalUserInfo.State = externalOrganization.State;
              externalUserInfo.Zipcode = externalOrganization.Zip;
              externalUserInfo.City = externalOrganization.City;
            }
            if (externalUserInfo.UseParentInfoForRateLock)
            {
              externalUserInfo.EmailForRateSheet = externalOrganization.EmailForRateSheet;
              externalUserInfo.EmailForLockInfo = externalOrganization.EmailForLockInfo;
              externalUserInfo.FaxForRateSheet = externalOrganization.FaxForRateSheet;
              externalUserInfo.FaxForLockInfo = externalOrganization.FaxForLockInfo;
            }
          }
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine(" SELECT r.roleName FROM UserPersona up inner join RolePersonas rp on up.personaID=rp.personaID ");
          dbQueryBuilder.AppendLine(" inner join Roles r on r.roleID=rp.roleID inner join  RolesMapping rm on rm.roleID=rp.roleId where up.userid='" + externalUserInfo.ContactID + "'");
          dbQueryBuilder.AppendLine(WorkflowBpmDbAccessor.GetUserEligibleRolesAndRoleMappingsQuery(new List<string>()
          {
            externalUserInfo.ContactID
          }));
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          if (dataSet != null)
          {
            if (dataSet.Tables[0].Rows.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
              {
                string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["roleName"]);
                if (!stringList.Contains(str))
                  stringList.Add(str);
              }
              externalUserInfo.RoleTypes = stringList;
            }
            if (dataSet.Tables[1].Rows.Count > 0)
            {
              List<RoleSummaryInfo> roleSummaryInfoList = new List<RoleSummaryInfo>();
              List<RoleMappingsDetails> roleMappingsDetailsList = new List<RoleMappingsDetails>();
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
              {
                roleSummaryInfoList.Add(WorkflowBpmDbAccessor.DataRowToRoleSummaryInfo(row));
                if (!Convert.IsDBNull(row["realWorldRoleID"]))
                  roleMappingsDetailsList.Add(WorkflowBpmDbAccessor.DataRowToRoleMappingDetails(row));
              }
              externalUserInfo.RoleMappings = roleMappingsDetailsList.ToArray();
              externalUserInfo.RoleSummaries = roleSummaryInfoList.ToArray();
            }
          }
          externalUserInfo.SiteURLs = ((IEnumerable<ExternalUserURL>) ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserID)).ToList<ExternalUserURL>();
          externalUserInfo.Groups = ExternalOrgManagementAccessor.GetExternalUserGroups(externalUserInfo.ContactID);
        }
        return externalUserInfo;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfo: Cannot fetch record from [ExternalUsers] table for user " + externalUserID + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoList(List<string> externalUserIDs)
    {
      string str = string.Join("','", (IEnumerable<string>) externalUserIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
      List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] WHERE [external_userid] in ( '" + str + "' )");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            externalUserInfoList.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return externalUserInfoList;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfo: Cannot fetch record from [ExternalUsers] table for user " + str + ".\r\n" + ex.Message);
      }
    }

    private static List<ExternalUserInfo> getExternalUserDetails(DataSet dsExternalUserDetails)
    {
      List<ExternalUserInfo> externalUserDetails = new List<ExternalUserInfo>();
      if (dsExternalUserDetails != null && dsExternalUserDetails.Tables.Count > 0)
      {
        dsExternalUserDetails.Relations.Add(dsExternalUserDetails.Tables[0].Columns["ContactId"], dsExternalUserDetails.Tables[1].Columns["userid"]);
        dsExternalUserDetails.Relations.Add(dsExternalUserDetails.Tables[0].Columns["external_userid"], dsExternalUserDetails.Tables[2].Columns["external_userid"]);
        dsExternalUserDetails.Relations.Add(dsExternalUserDetails.Tables[0].Columns["external_userid"], dsExternalUserDetails.Tables[3].Columns["external_userid"]);
        dsExternalUserDetails.Relations.Add(dsExternalUserDetails.Tables[0].Columns["ContactId"], dsExternalUserDetails.Tables[4].Columns["userid"]);
        dsExternalUserDetails.Relations.Add(dsExternalUserDetails.Tables[0].Columns["ContactId"], dsExternalUserDetails.Tables[5].Columns["userid"]);
        foreach (DataRow row in (InternalDataCollectionBase) dsExternalUserDetails.Tables[0].Rows)
        {
          ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row);
          externalUserInfo.AccessMode = (UserInfo.AccessModeEnum) row["userAccessMode"];
          externalUserInfo.PeerView = (UserInfo.UserPeerView) row["userPeerView"];
          externalUserInfo.UserPersonas = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[1].Select("userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["ContactId"]), true))).Select<DataRow, Persona>((System.Func<DataRow, Persona>) (u => PersonaAccessor.GetPersona(Convert.ToInt32(u["personaID"])))).ToArray<Persona>();
          externalUserInfo.Licenses = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[2].Select("external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["external_userid"]), true))).Select<DataRow, StateLicenseExtType>((System.Func<DataRow, StateLicenseExtType>) (u => ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(u, false))).ToList<StateLicenseExtType>();
          externalUserInfo.SiteURLs = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[3].Select("external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["external_userid"]), true))).Select<DataRow, ExternalUserURL>((System.Func<DataRow, ExternalUserURL>) (u => ExternalOrgManagementAccessor.databaseRowToExternalUserURL(u))).ToList<ExternalUserURL>();
          externalUserInfo.Groups = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[4].Select("userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["ContactId"]), true))).Select<DataRow, AclGroup>((System.Func<DataRow, AclGroup>) (u => ExternalOrgManagementAccessor.databaseRowToAclGroup(u))).ToList<AclGroup>();
          externalUserInfo.RoleSummaries = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[5].Select("userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["ContactId"]), true))).Select<DataRow, RoleSummaryInfo>((System.Func<DataRow, RoleSummaryInfo>) (u => WorkflowBpmDbAccessor.DataRowToRoleSummaryInfo(u))).ToArray<RoleSummaryInfo>();
          externalUserInfo.RoleMappings = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[5].Select("userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["ContactId"]), true) + " and realWorldRoleID is not null")).Select<DataRow, RoleMappingsDetails>((System.Func<DataRow, RoleMappingsDetails>) (u => WorkflowBpmDbAccessor.DataRowToRoleMappingDetails(u))).ToArray<RoleMappingsDetails>();
          externalUserInfo.RoleTypes = ((IEnumerable<DataRow>) dsExternalUserDetails.Tables[6].Select("userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(Convert.ToString(row["ContactId"]), true))).Select<DataRow, string>((System.Func<DataRow, string>) (u => Convert.ToString(u["roleName"]))).ToList<string>();
          externalUserDetails.Add(externalUserInfo);
        }
      }
      return externalUserDetails;
    }

    private static AclGroup databaseRowToAclGroup(DataRow r)
    {
      bool viewSubordContacts = false;
      if (r["viewSubordContacts"] != DBNull.Value)
        viewSubordContacts = (byte) r["viewSubordContacts"] == (byte) 1;
      AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
      if (r["contactAccess"] != DBNull.Value)
        contactAccess = (byte) r["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
      return new AclGroup((int) r["groupID"], (string) r["groupName"], viewSubordContacts, contactAccess, (int) r["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModifiedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LastModifiedBy"]));
    }

    public static List<string> GetExternalUserIds(string contactIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT external_userid from [AEAccessibleExternalUsers] aeu JOIN [ExternalUsers] eu ON aeu.TPOContactID = eu.ContactID where aeu.AEID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) contactIds));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      List<string> externalUserIds = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        externalUserIds.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["external_userid"]));
      return externalUserIds;
    }

    private static List<string> getExternalUserContactIDs(List<string> externalUserIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> externalUserContactIds = new List<string>();
      if (externalUserIDs == null || !externalUserIDs.Any<string>())
        return externalUserContactIds;
      string str = string.Join("','", (IEnumerable<string>) externalUserIDs);
      dbQueryBuilder.AppendLine("SELECT ContactID FROM [ExternalUsers] eu WHERE eu.external_userid IN ('" + str + "')");
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteSetQuery().Tables[0].Rows)
        externalUserContactIds.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContactID"]));
      return externalUserContactIds;
    }

    private static string getExternalUserContactID(string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (string.IsNullOrWhiteSpace(externalUserID))
        return string.Empty;
      dbQueryBuilder.AppendLine("SELECT ContactID FROM [ExternalUsers] eu WHERE eu.external_userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserID));
      return Convert.ToString(dbQueryBuilder.ExecuteScalar());
    }

    public static ExternalUserInfo GetExternalUserInfoByContactId(string contactId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
      List<string> stringList = new List<string>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] WHERE [contactId] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(contactId));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (ExternalUserInfo) null;
        ExternalUserInfo externalUserInfo1 = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(dataRowCollection[0], User.GetUserById(contactId));
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT State, LicenseApproved, LicenseExempt, LicenseNumber,IssueDate, StartDate, EndDate, Status, StatusDate, LastCheckedDate FROM [ExternalUserStateLicensing] Where external_userid = '" + externalUserInfo1.ExternalUserID + "'");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          stateLicenseExtTypeList.Add(ExternalOrgManagementAccessor.getStateLicenseExtTypeFromDatarow(row, false));
        externalUserInfo1.Licenses = stateLicenseExtTypeList;
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(" SELECT r.roleName FROM UserPersona up inner join RolePersonas rp on up.personaID=rp.personaID ");
        dbQueryBuilder.AppendLine(" inner join Roles r on r.roleID=rp.roleID inner join  RolesMapping rm on rm.roleID=rp.roleId where up.userid='" + externalUserInfo1.ContactID + "'");
        dbQueryBuilder.AppendLine(WorkflowBpmDbAccessor.GetUserEligibleRolesAndRoleMappingsQuery(new List<string>()
        {
          externalUserInfo1.ContactID
        }));
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet != null)
        {
          if (dataSet.Tables[0].Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["roleName"]);
              if (!stringList.Contains(str))
                stringList.Add(str);
            }
            externalUserInfo1.RoleTypes = stringList;
          }
          if (dataSet.Tables[1].Rows.Count > 0)
          {
            List<RoleSummaryInfo> roleSummaryInfoList = new List<RoleSummaryInfo>();
            List<RoleMappingsDetails> roleMappingsDetailsList = new List<RoleMappingsDetails>();
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
            {
              roleSummaryInfoList.Add(WorkflowBpmDbAccessor.DataRowToRoleSummaryInfo(row));
              if (!Convert.IsDBNull(row["realWorldRoleID"]))
                roleMappingsDetailsList.Add(WorkflowBpmDbAccessor.DataRowToRoleMappingDetails(row));
            }
          }
        }
        ExternalUserInfo externalUserInfo2 = externalUserInfo1;
        ExternalUserURL[] externalUserInfoUrLs = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserInfo1.ExternalUserID);
        List<ExternalUserURL> list = externalUserInfoUrLs != null ? ((IEnumerable<ExternalUserURL>) externalUserInfoUrLs).ToList<ExternalUserURL>() : (List<ExternalUserURL>) null;
        externalUserInfo2.SiteURLs = list;
        externalUserInfo1.Groups = ExternalOrgManagementAccessor.GetExternalUserGroups(externalUserInfo1.ContactID);
        return externalUserInfo1;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
        return (ExternalUserInfo) null;
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoList(List<string> contactIDs, int urlID)
    {
      List<string[]> strArrayList = new List<string[]>();
      while (contactIDs.Count > 1000)
      {
        string[] array = new string[1000];
        contactIDs.CopyTo(0, array, 0, 1000);
        strArrayList.Add(array);
        contactIDs.RemoveRange(0, 1000);
      }
      if (contactIDs.Count > 0)
        strArrayList.Add(contactIDs.ToArray());
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      foreach (string[] strArray in strArrayList.ToArray())
      {
        string str = string.Join("','", strArray);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers] U inner join [ExternalUserURLs] EUUrl on U.external_userid = EUUrl.external_userid  WHERE [contactId] in ( '" + str + "' ) and EUUrl.urlID = " + (object) urlID);
        try
        {
          DataRowCollection rows = dbQueryBuilder.Execute();
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          if (rows != null)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
            {
              DataRow row = dataRow;
              ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row);
              if (!dictionary.ContainsKey(externalUserInfo.ExternalUserID))
                dictionary.Add(externalUserInfo.ExternalUserID, ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
            }
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalUserInfoList: Cannot fetch record from [ExternalUsers] table for user " + str + ".\r\n" + ex.Message);
        }
      }
      return dictionary.Values.ToList<ExternalUserInfo>();
    }

    public static Persona[] GetExternalUserInfoUserPersonas(string contactID)
    {
      Persona[] infoUserPersonas = (Persona[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT p.* FROM [UserPersona] up INNER JOIN [Personas] p ON up.personaID = p.personaID WHERE up.[userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(contactID));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            infoUserPersonas = new Persona[dataRowCollection.Count];
            for (int index = 0; index < dataRowCollection.Count; ++index)
              infoUserPersonas[index] = ExternalOrgManagementAccessor.databaseRowToExternalUserInfoUserPersona(dataRowCollection[index]);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
      return infoUserPersonas;
    }

    public static Persona[] GetTPOMappedPersonasByRealWorldRoleID(int realWorldRoleID)
    {
      Persona[] byRealWorldRoleId = (Persona[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Personas.* FROM Personas inner join RolePersonas on personas.personaID = RolePersonas.personaID ");
      dbQueryBuilder.AppendLine("where RolePersonas.roleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID = " + realWorldRoleID.ToString() + "))");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            byRealWorldRoleId = new Persona[dataRowCollection.Count];
            for (int index = 0; index < dataRowCollection.Count; ++index)
              byRealWorldRoleId[index] = ExternalOrgManagementAccessor.databaseRowToExternalUserInfoUserPersona(dataRowCollection[index]);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
      return byRealWorldRoleId;
    }

    public static Persona[] GetTPOPrimarySalesRepMappedPersonas()
    {
      return ExternalOrgManagementAccessor.GetTPOMappedPersonasByRealWorldRoleID(7);
    }

    public static Persona[] GetTPOLoanOfficerMappedPersonas()
    {
      return ExternalOrgManagementAccessor.GetTPOMappedPersonasByRealWorldRoleID(8);
    }

    public static Persona[] GetTPOLoanProcessorMappedPersonas()
    {
      return ExternalOrgManagementAccessor.GetTPOMappedPersonasByRealWorldRoleID(9);
    }

    private static DataTable constructTPOWCAEView()
    {
      return new DataTable()
      {
        Columns = {
          "ID",
          "ContactID",
          "FirstName",
          "LastName",
          "Email",
          "Login_email",
          "Phone",
          "ContactOrgID",
          "CompanyID",
          "CompanyExternalID",
          "CompanyName",
          "BranchID",
          "BranchExternalID",
          "BranchName"
        }
      };
    }

    private static void addTPOWCAEViewRow(DataTable dt, DataRow dr)
    {
      if (((IEnumerable<DataRow>) dt.Select("ID = '" + dr["external_userid"] + "'")).Count<DataRow>() > 0)
        return;
      DataRow row = dt.NewRow();
      row["ID"] = dr["external_userid"];
      row["ContactID"] = dr["ContactID"];
      row["FirstName"] = dr["First_name"];
      row["LastName"] = dr["Last_name"];
      row["Email"] = dr["Email"];
      row["Login_email"] = dr["Login_email"];
      row["Phone"] = dr["Phone"];
      row["ContactOrgID"] = dr["externalOrgID"];
      dt.Rows.Add(row);
    }

    private static Dictionary<string, List<string>> getTPOWCAEOrgView(List<string> distinctOrgIDs)
    {
      Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      distinctOrgIDs.ForEach((Action<string>) (x =>
      {
        if (result.ContainsKey(x))
          return;
        try
        {
          sql.Reset();
          sql.AppendLine("Select * from FN_TPOWCOrgInfo(" + x + ") order by depth");
          DataRowCollection dataRowCollection = sql.Execute();
          if (dataRowCollection != null || dataRowCollection.Count > 0)
          {
            DataRow dataRow1 = dataRowCollection[0];
            DataRow dataRow2 = (DataRow) null;
            if (dataRowCollection.Count > 1)
              dataRow2 = dataRowCollection[1];
            List<string> stringList = new List<string>();
            stringList.Add(string.Concat(dataRow1["oid"]));
            stringList.Add(string.Concat(dataRow1["OrganizationName"]));
            stringList.Add(string.Concat(dataRow1["ExternalID"]));
            if (dataRow2 != null && string.Concat(dataRow2["OrganizationType"]) == "2")
            {
              stringList.Add(string.Concat(dataRow2["oid"]));
              stringList.Add(string.Concat(dataRow2["OrganizationName"]));
              stringList.Add(string.Concat(dataRow2["ExternalID"]));
            }
            else
              stringList.AddRange((IEnumerable<string>) new string[3]
              {
                "",
                "",
                ""
              });
            result.Add(x, stringList);
          }
          else
            result.Add(x, new List<string>((IEnumerable<string>) new string[6]
            {
              "",
              "",
              "",
              "",
              "",
              ""
            }));
        }
        catch
        {
          result.Add(x, new List<string>((IEnumerable<string>) new string[6]
          {
            "",
            "",
            "",
            "",
            "",
            ""
          }));
        }
      }));
      return result;
    }

    public static ArrayList GetTPOWCAEView(
      string userid,
      int orgId,
      int urlID,
      QueryCriterion[] searchCriteria = null,
      bool isAdmin = false)
    {
      DataTable dt = ExternalOrgManagementAccessor.constructTPOWCAEView();
      List<string> distinctOrgIDs = new List<string>();
      List<string> stringList = new List<string>();
      List<string[]> strArrayList = new List<string[]>();
      if (!isAdmin)
      {
        ArrayList andOrgBySalesRep = ExternalOrgManagementAccessor.GetExternalAndInternalUserAndOrgBySalesRep(userid, orgId);
        if (andOrgBySalesRep != null)
          stringList = (List<string>) andOrgBySalesRep[4];
      }
      else
        stringList = ExternalOrgManagementAccessor.GetAllExternalContactIds();
      while (stringList.Count > 1000)
      {
        string[] array = new string[1000];
        stringList.CopyTo(0, array, 0, 1000);
        strArrayList.Add(array);
        stringList.RemoveRange(0, 1000);
      }
      if (stringList.Count > 0)
        strArrayList.Add(stringList.ToArray());
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      foreach (string[] strArray in strArrayList.ToArray())
      {
        string str = string.Join("','", strArray);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT U.external_userid, U.ContactID, U.First_name, U.Last_name, U.Email, U.Login_email, U.Phone, U.externalOrgID FROM [ExternalUsers] U inner join [ExternalUserURLs] EUUrl on U.external_userid = EUUrl.external_userid  WHERE [contactId] in ( '" + str + "' ) and EUUrl.urlID = " + (object) urlID);
        if (searchCriteria != null)
        {
          for (int index = 0; index < searchCriteria.Length; ++index)
            dbQueryBuilder.AppendLine("   and (" + searchCriteria[index].ToSQLClause() + ")");
        }
        try
        {
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null)
          {
            foreach (DataRow dr in (InternalDataCollectionBase) dataRowCollection)
            {
              ExternalOrgManagementAccessor.addTPOWCAEViewRow(dt, dr);
              if (string.Concat(dr["externalOrgID"]) != "" && !distinctOrgIDs.Contains(string.Concat(dr["externalOrgID"])))
                distinctOrgIDs.Add(string.Concat(dr["externalOrgID"]));
            }
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalUserInfoList: Cannot fetch record from [ExternalUsers] table for user " + str + ".\r\n" + ex.Message);
        }
      }
      Dictionary<string, List<string>> tpowcaeOrgView = ExternalOrgManagementAccessor.getTPOWCAEOrgView(distinctOrgIDs);
      return new ArrayList()
      {
        (object) dt,
        (object) tpowcaeOrgView
      };
    }

    public static ArrayList ConstructTpoAeView(
      string salesRepId,
      int orgId,
      string siteId,
      QueryCriterion[] searchCriteria = null,
      bool isAdmin = false,
      bool canAccessGlobalTpoUsers = false,
      int start = 0,
      int limit = 0,
      bool includePersonaDetails = false)
    {
      ExternalOrgManagementAccessor.constructTPOWCAEView();
      List<string> distinctOrgIDs = new List<string>();
      List<string> stringList = new List<string>();
      List<string[]> strArrayList = new List<string[]>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT DISTINCT eu.external_UserId AS ID, eu.contactID AS ContactID, eu.last_name AS LastName, eu.Middle_name, eu.first_name AS FirstName, eou.IsDeleted, eu.externalOrgID AS ContactOrgID , eu.Email , eu.Login_email , eu.Phone, eu.status,");
      if (includePersonaDetails)
        dbQueryBuilder.AppendLine("(stuff((select ',' + cast(personaID as varchar) from userpersona up where up.userid = eu.ContactID for xml path('')), 1, 1, '')) ");
      else
        dbQueryBuilder.AppendLine(" '' ");
      dbQueryBuilder.AppendLine(" personasList FROM ExternalUsers eu JOIN ExternalUserURLs euur ON eu.external_userid = euur.external_userid");
      dbQueryBuilder.AppendLine("JOIN ExternalOrgURLs eou ON euur.urlID = eou.urlID ");
      dbQueryBuilder.AppendLine("LEFT OUTER JOIN AEAccessibleExternalUsers aeu ON eu.external_userid = aeu.UserID WHERE eu.status = 0 AND eou.siteID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) siteId));
      if (!(isAdmin | canAccessGlobalTpoUsers))
        dbQueryBuilder.AppendLine("AND (aeu.aeID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) salesRepId) + " and aeu.aeId is not null)");
      if (searchCriteria != null)
      {
        for (int index = 0; index < searchCriteria.Length; ++index)
          dbQueryBuilder.AppendLine("AND (" + searchCriteria[index].ToSQLClause() + ")");
      }
      DataTable dataTable;
      try
      {
        if (start <= 0 && limit <= 0)
        {
          dataTable = dbQueryBuilder.ExecuteTableQuery();
        }
        else
        {
          int startRecordNumber = start <= 0 ? 1 : start + 1;
          int endRecordNumber = limit <= 0 ? int.MaxValue : startRecordNumber + limit - 1;
          dataTable = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), startRecordNumber, endRecordNumber, (List<SortColumn>) null);
        }
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              if (string.Concat(row["ContactOrgID"]) != "" && !distinctOrgIDs.Contains(string.Concat(row["ContactOrgID"])))
                distinctOrgIDs.Add(string.Concat(row["ContactOrgID"]));
            }
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfoList: Error fetching record for salesrep.\r\n" + ex.Message);
      }
      Dictionary<string, List<string>> tpowcaeOrgView = ExternalOrgManagementAccessor.getTPOWCAEOrgView(distinctOrgIDs);
      return new ArrayList()
      {
        (object) dataTable,
        (object) tpowcaeOrgView
      };
    }

    public static List<ExternalUserInfo> GetExternalUserInfoListByContactId(List<string> contactIDs)
    {
      List<string[]> strArrayList = new List<string[]>();
      while (contactIDs.Count > 1000)
      {
        string[] array = new string[1000];
        contactIDs.CopyTo(0, array, 0, 1000);
        strArrayList.Add(array);
        contactIDs.RemoveRange(0, 1000);
      }
      if (contactIDs.Count > 0)
        strArrayList.Add(contactIDs.ToArray());
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, ExternalUserInfo> dictionary = new Dictionary<string, ExternalUserInfo>();
      foreach (string[] strArray in strArrayList.ToArray())
      {
        string str = string.Join("','", strArray);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]  WHERE [contactId] in ( '" + str + "' )");
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUserURLs] a inner join [ExternalOrgURLs] b  on a.[urlID] = b.[urlID] WHERE a.[external_userid] in (SELECT external_userid FROM [ExternalUsers]  WHERE [contactId] in ( '" + str + "' )) ORDER BY a.[AddedDateTime]");
        try
        {
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default);
          if (dataSet != null)
          {
            List<ExternalUserURL> source1 = new List<ExternalUserURL>();
            if (dataSet.Tables[1] != null)
            {
              foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
                source1.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserURL(row));
            }
            UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
            foreach (DataRow row1 in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              DataRow row = row1;
              ExternalUserInfo externalUserInfo1 = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row);
              if (!dictionary.ContainsKey(externalUserInfo1.ExternalUserID))
              {
                ExternalUserInfo extUser = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>());
                ExternalUserInfo externalUserInfo2 = extUser;
                IEnumerable<ExternalUserURL> source2 = source1.Where<ExternalUserURL>((System.Func<ExternalUserURL, bool>) (x => x.ExternalUserID == extUser.ExternalUserID));
                List<ExternalUserURL> list = source2 != null ? source2.ToList<ExternalUserURL>() : (List<ExternalUserURL>) null;
                externalUserInfo2.SiteURLs = list;
                dictionary.Add(externalUserInfo1.ExternalUserID, extUser);
              }
            }
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetExternalUserInfoListByContactId: Cannot fetch record from [ExternalUsers] table for user " + str + ".\r\n" + ex.Message);
        }
      }
      Dictionary<string, Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>> extUsersRolesAndRoleMappings = WorkflowBpmDbAccessor.GetUsersRolesAndRoleMappings(dictionary.Select<KeyValuePair<string, ExternalUserInfo>, string>((System.Func<KeyValuePair<string, ExternalUserInfo>, string>) (x => x.Value.ContactID)).ToList<string>());
      dictionary.ForEach<KeyValuePair<string, ExternalUserInfo>>((Action<KeyValuePair<string, ExternalUserInfo>>) (x =>
      {
        if (!extUsersRolesAndRoleMappings.ContainsKey(x.Value.ContactID))
          return;
        x.Value.RoleSummaries = extUsersRolesAndRoleMappings[x.Value.ContactID].Item1?.ToArray();
        x.Value.RoleMappings = extUsersRolesAndRoleMappings[x.Value.ContactID].Item2?.ToArray();
      }));
      return dictionary.Values.ToList<ExternalUserInfo>();
    }

    public static List<ExternalUserInfo> GetLoginExternalUserInfoFromEmail(string loginEmail)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> userInfoFromEmail = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT ContactID, external_userid, externalOrgId, status, password FROM [ExternalUsers]");
      dbQueryBuilder.AppendLine("WHERE login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            ExternalUserInfo externalUserInfo1 = new ExternalUserInfo();
            externalUserInfo1.DisabledLogin = (UserInfo.UserStatusEnum) dataRow["Status"] == UserInfo.UserStatusEnum.Disabled;
            externalUserInfo1.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["contactID"]);
            externalUserInfo1.ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["external_userid"]);
            externalUserInfo1.ExternalOrgID = (int) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["externalOrgId"]);
            externalUserInfo1.Password = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["password"]);
            ExternalUserInfo externalUserInfo2 = externalUserInfo1;
            userInfoFromEmail.Add(externalUserInfo2);
          }
        }
        return userInfoFromEmail;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetLoginExternalUserInfoFromEmail: Cannot fetch record from [ExternalUsers] table for user " + loginEmail + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoFromEmail(string loginEmail)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> userInfoFromEmail = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]  WHERE login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null && rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            userInfoFromEmail.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return userInfoFromEmail;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfoFromEmail: Cannot fetch record from [ExternalUsers] table for user " + loginEmail + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoFromEmailandURLID(
      string loginEmail,
      string URLID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> fromEmailandUrlid = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("select * from ExternalUsers eu inner join ExternalUserURLs euu on eu.external_userID = euu.external_userID where eu.login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
      dbQueryBuilder.AppendLine(" and euu.urlID in (select urlID from [ExternalOrgURLs] where siteID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(URLID));
      dbQueryBuilder.AppendLine(") order by eu.status ASC");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null && rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            fromEmailandUrlid.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return fromEmailandUrlid;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfoFromEmailandURLID: Cannot fetch record from [ExternalUsers] table for user " + loginEmail + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserURL> GetExternalUserURLs(
      string loginEmail,
      List<string> siteIds,
      string excludeExtUserID = null,
      bool activeUsersOnly = true)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = (string) null;
      if (siteIds != null && siteIds.Count > 0)
        str = string.Join(",", EllieMae.EMLite.DataAccess.SQL.EncodeString(siteIds.ToArray(), true));
      dbQueryBuilder.AppendLine("select euu.urlID, euu.external_userid, euu.AddedDateTime, eou.IsDeleted, eou.URL, eou.SiteId");
      dbQueryBuilder.AppendLine("from ExternalUsers eu");
      dbQueryBuilder.AppendLine("inner join ExternalUserURLs euu on eu.external_userID = euu.external_userID");
      dbQueryBuilder.AppendLine("inner join ExternalOrgURLs eou on eou.[urlID] = euu.[urlID]");
      dbQueryBuilder.AppendLine("where eu.login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
      if (!string.IsNullOrWhiteSpace(str))
        dbQueryBuilder.AppendLine("and eou.SiteId in (" + str + ")");
      if (!string.IsNullOrWhiteSpace(excludeExtUserID))
        dbQueryBuilder.AppendLine("and eu.external_userid <> " + EllieMae.EMLite.DataAccess.SQL.EncodeString(excludeExtUserID));
      if (activeUsersOnly)
        dbQueryBuilder.AppendLine("and eu.status = 0");
      try
      {
        List<ExternalUserURL> externalUserUrLs = new List<ExternalUserURL>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
            externalUserUrLs.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserURL(r));
        }
        return externalUserUrLs;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserURLs: Cannot fetch record from [ExternalUserURLs] table for user with loging email" + loginEmail + ".\r\n" + ex.Message);
      }
    }

    private static ExternalUserURL databaseRowToExternalUserURL(DataRow r)
    {
      return new ExternalUserURL()
      {
        URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["urlID"]),
        ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["external_userid"]),
        DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["AddedDateTime"]),
        isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsDeleted"]),
        URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["URL"]),
        siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SiteId"])
      };
    }

    public static bool CompareExternalUserPassword(string external_userId, string password)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @password varbinary(255)");
        dbQueryBuilder.AppendLine("declare @contactID varchar(16)");
        dbQueryBuilder.AppendLine("select @contactID = [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(external_userId));
        dbQueryBuilder.AppendLine("select @password = password from [UserCredentials] where userid = @contactID");
        dbQueryBuilder.AppendLine("if (@password is NULL)");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("    select PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalOrgManagementAccessor.getCaseSensitivePassword(password)) + ", password, 0) as PwdResult from [ExternalUsers] where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(external_userId));
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    select -1 as PwdResult, @password as HashedPassword");
        DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
        int num = (int) dataRow["PwdResult"];
        PasswordEncryptor passwordEncryptor = new PasswordEncryptor();
        if (num >= 0)
        {
          if (num == 0)
            return false;
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("declare @contactID varchar(16)");
          dbQueryBuilder.AppendLine("select @contactID = [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(external_userId));
          byte[] numArray = passwordEncryptor.Hash(password);
          dbQueryBuilder.AppendLine("Insert into [UserCredentials] values (@contactID, @passwordHash)");
          DbCommandParameter[] parameters = new DbCommandParameter[1]
          {
            new DbCommandParameter("@passwordHash", (object) numArray, DbType.Binary)
          };
          dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default, parameters);
          return true;
        }
        byte[] hashedPassword = (byte[]) dataRow["HashedPassword"];
        return passwordEncryptor.Compare(password, hashedPassword);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CompareExternalUserPassword: Cannot fetch record from [ExternalUsers] table for user " + external_userId + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoFromEmailandURLID(
      string loginEmail,
      string URLID,
      string password)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> fromEmailandUrlid = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("select *, PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalOrgManagementAccessor.getCaseSensitivePassword(password)) + ", eu.password, 0) as PwdComp, uc.password as credentials from ExternalUsers eu ");
      dbQueryBuilder.AppendLine("inner join ExternalUserURLs euu on eu.external_userID = euu.external_userID left outer join userCredentials uc on uc.userid = eu.contactID where eu.login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
      dbQueryBuilder.AppendLine(" and euu.urlID in (select urlID from [ExternalOrgURLs] where siteID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(URLID));
      dbQueryBuilder.AppendLine(") order by eu.status ASC");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows != null && rows.Count > 0)
        {
          PasswordEncryptor passwordEncryptor = new PasswordEncryptor();
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            if (!Convert.IsDBNull(row["credentials"]))
            {
              if (passwordEncryptor.Compare(password, (byte[]) row["credentials"]))
                fromEmailandUrlid.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
            }
            else if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["PwdComp"]) == 1)
            {
              fromEmailandUrlid.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>()));
              string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["external_userid"]);
              byte[] numArray = passwordEncryptor.Hash(password);
              dbQueryBuilder.Reset();
              dbQueryBuilder.AppendLine("declare @contactID varchar(16)");
              dbQueryBuilder.AppendLine("select @contactID = [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(str));
              dbQueryBuilder.AppendLine("if not exists (select * from [UserCredentials] where userid = @contactID)");
              dbQueryBuilder.AppendLine("    Insert into [UserCredentials] values (@contactID, @passwordHash)");
              DbCommandParameter[] parameters = new DbCommandParameter[1]
              {
                new DbCommandParameter("@passwordHash", (object) numArray, DbType.Binary)
              };
              dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default, parameters);
            }
          }
        }
        return fromEmailandUrlid;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserInfoFromEmailandURLID: Cannot fetch record from [ExternalUsers] table for user " + loginEmail + ".\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetExternalUserInfoBySalesRep(string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> userInfoBySalesRep = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUsers]  where Sales_rep_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            userInfoBySalesRep.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return userInfoBySalesRep;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetAllCompanyManagers(int extID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> allCompanyManagers = new List<ExternalUserInfo>();
      ExternalOriginatorManagementData rootOrganisation = ExternalOrgManagementAccessor.GetRootOrganisation(false, extID);
      if (rootOrganisation == null)
        return new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT descendent FROM [ExternalOrgDescendents] WHERE [oid] = " + (object) rootOrganisation.oid);
      List<int> values = new List<int>();
      values.Add(rootOrganisation.oid);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        values.Add(Convert.ToInt32(dataRow["descendent"]));
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("Select * from ExternalUsers where Roles & 4 = 4 and externalOrgID in (" + string.Join<int>(",", (IEnumerable<int>) values) + ")");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            allCompanyManagers.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return allCompanyManagers;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
    }

    private static List<string> GetAllExternalContactIds()
    {
      ExternalUserInfo[] externalUserInfos = ExternalOrgManagementAccessor.GetAllExternalUserInfos(string.Empty);
      return externalUserInfos != null && ((IEnumerable<ExternalUserInfo>) externalUserInfos).Any<ExternalUserInfo>() ? ((IEnumerable<ExternalUserInfo>) externalUserInfos).Select<ExternalUserInfo, string>((System.Func<ExternalUserInfo, string>) (item => item.ContactID.ToString())).ToList<string>() : (List<string>) null;
    }

    public static List<string> GetAllExternalUsers()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> allExternalUsers = new List<string>();
      dbQueryBuilder.AppendLine("SELECT external_userId FROM [ExternalUsers]");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            if (row["external_userId"] != null)
              allExternalUsers.Add(row["external_userId"].ToString());
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
      return allExternalUsers;
    }

    private static ExternalUserInfo databaseRowToExternalUserInfo(DataRow r)
    {
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      externalUserInfo.ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["external_userid"]);
      externalUserInfo.UseCompanyAddress = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UseCompanyAddress"]);
      externalUserInfo.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]);
      externalUserInfo.LastName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Last_name"], (object) "");
      externalUserInfo.FirstName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["First_name"], (object) "");
      externalUserInfo.MiddleName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Middle_name"], (object) "");
      externalUserInfo.Suffix = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Suffix_name"], (object) "");
      externalUserInfo.Title = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Title"], (object) "");
      externalUserInfo.Address = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Address1"], (object) "");
      externalUserInfo.City = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["City"], (object) "");
      externalUserInfo.State = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["State"], (object) "");
      externalUserInfo.Zipcode = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Zip"], (object) "");
      externalUserInfo.ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOrgID"]);
      externalUserInfo.Phone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Phone"], (object) "");
      externalUserInfo.PasswordChangedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Password_changed"]);
      externalUserInfo.CellPhone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Cell_phone"], (object) "");
      externalUserInfo.Fax = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Fax"], (object) "");
      externalUserInfo.Email = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Email"], (object) "");
      externalUserInfo.NmlsID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NMLSOriginatorID"]);
      externalUserInfo.SSN = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SSN"]);
      externalUserInfo.UseParentInfoForRateLock = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["InheritParentRateSheet"], false);
      externalUserInfo.EmailForRateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Rate_sheet_email"]);
      externalUserInfo.FaxForRateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Rate_sheet_fax"]);
      externalUserInfo.EmailForLockInfo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Lock_info_email"]);
      externalUserInfo.FaxForLockInfo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Lock_info_fax"]);
      externalUserInfo.DisabledLogin = (UserInfo.UserStatusEnum) r["Status"] == UserInfo.UserStatusEnum.Disabled;
      externalUserInfo.EmailForLogin = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Login_email"]);
      externalUserInfo.WelcomeEmailDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Welcome_Email_Date"]);
      externalUserInfo.WelcomeEmailUserName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Welcome_Email_By"]);
      externalUserInfo.ApprovalCurrentStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Approval_status"]);
      externalUserInfo.AddToWatchlist = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Approval_status_watchlist"]);
      externalUserInfo.ApprovalCurrentStatusDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Approval_status_date"]);
      externalUserInfo.ApprovalDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Approval_date"]);
      externalUserInfo.RequirePasswordChange = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Require_pwd_change"]);
      externalUserInfo.LastLogin = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Last_Login"], DateTime.MinValue);
      externalUserInfo.SalesRepID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Sales_rep_userid"]);
      externalUserInfo.Roles = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Roles"]);
      externalUserInfo.Notes = r.Table.Columns.Contains("Note") ? EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Note"]) : "";
      externalUserInfo.UpdatedDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["UpdatedDTTM"]);
      externalUserInfo.UpdatedByExternal = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UpdatedByExternal"]);
      externalUserInfo.UpdatedBy = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UpdatedBy"]);
      externalUserInfo.NMLSCurrent = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["nmlscurrent"]);
      externalUserInfo.OrgVisibleOnTpowcSite = r.Table.Columns.Contains("VisibleOnTPOWCSite") && EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["VisibleOnTPOWCSite"]);
      externalUserInfo.OrganizationType = r.Table.Columns.Contains("OrganizationType") ? (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["OrganizationType"]) : ExternalOriginatorOrgType.Company;
      externalUserInfo.createdDate = new DateTime?(r.Table.Columns.Contains("CreatedDate") ? EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["CreatedDate"], DateTime.MinValue) : DateTime.MinValue);
      externalUserInfo.createdBy = r.Table.Columns.Contains("CreatedBy") ? EllieMae.EMLite.DataAccess.SQL.DecodeString(r["CreatedBy"], (string) null) : (string) null;
      externalUserInfo.lastModifiedDate = new DateTime?(r.Table.Columns.Contains("LastModifiedDate") ? EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["LastModifiedDate"], DateTime.MinValue) : DateTime.MinValue);
      return externalUserInfo;
    }

    private static ExternalUserInfo databaseRowToExternalUserInfo(DataRow r, UserInfo userInfo)
    {
      ExternalUserInfo externalUserInfo = new ExternalUserInfo(userInfo);
      externalUserInfo.ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["external_userid"]);
      externalUserInfo.UseCompanyAddress = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UseCompanyAddress"]);
      externalUserInfo.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]);
      externalUserInfo.LastName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Last_name"], (object) "");
      externalUserInfo.FirstName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["First_name"], (object) "");
      externalUserInfo.MiddleName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Middle_name"], (object) "");
      externalUserInfo.Suffix = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Suffix_name"], (object) "");
      externalUserInfo.Title = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Title"], (object) "");
      externalUserInfo.Address = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Address1"], (object) "");
      externalUserInfo.City = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["City"], (object) "");
      externalUserInfo.State = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["State"], (object) "");
      externalUserInfo.Zipcode = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Zip"], (object) "");
      externalUserInfo.ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOrgID"]);
      externalUserInfo.Phone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Phone"], (object) "");
      externalUserInfo.PasswordChangedDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Password_changed"]);
      externalUserInfo.CellPhone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Cell_phone"], (object) "");
      externalUserInfo.Fax = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Fax"], (object) "");
      externalUserInfo.Email = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Email"], (object) "");
      externalUserInfo.NmlsID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["NMLSOriginatorID"]);
      externalUserInfo.SSN = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["SSN"]);
      externalUserInfo.UseParentInfoForRateLock = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["InheritParentRateSheet"], false);
      externalUserInfo.EmailForRateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Rate_sheet_email"]);
      externalUserInfo.FaxForRateSheet = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Rate_sheet_fax"]);
      externalUserInfo.EmailForLockInfo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Lock_info_email"]);
      externalUserInfo.FaxForLockInfo = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Lock_info_fax"]);
      externalUserInfo.DisabledLogin = (UserInfo.UserStatusEnum) r["Status"] == UserInfo.UserStatusEnum.Disabled;
      externalUserInfo.EmailForLogin = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Login_email"]);
      externalUserInfo.WelcomeEmailDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Welcome_Email_Date"]);
      externalUserInfo.WelcomeEmailUserName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Welcome_Email_By"]);
      externalUserInfo.ApprovalCurrentStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Approval_status"]);
      externalUserInfo.AddToWatchlist = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Approval_status_watchlist"]);
      externalUserInfo.ApprovalCurrentStatusDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Approval_status_date"]);
      externalUserInfo.ApprovalDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Approval_date"]);
      externalUserInfo.RequirePasswordChange = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["Require_pwd_change"]);
      externalUserInfo.LastLogin = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Last_Login"], DateTime.MinValue);
      externalUserInfo.SalesRepID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Sales_rep_userid"]);
      externalUserInfo.Roles = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Roles"]);
      externalUserInfo.Notes = r.Table.Columns.Contains("Note") ? EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Note"]) : "";
      externalUserInfo.UpdatedDateTime = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["UpdatedDTTM"]);
      externalUserInfo.UpdatedByExternal = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UpdatedByExternal"]);
      externalUserInfo.UpdatedBy = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["UpdatedBy"]);
      externalUserInfo.NMLSCurrent = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["nmlscurrent"]);
      externalUserInfo.OrgVisibleOnTpowcSite = r.Table.Columns.Contains("VisibleOnTPOWCSite") && EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["VisibleOnTPOWCSite"]);
      externalUserInfo.OrganizationType = r.Table.Columns.Contains("OrganizationType") ? (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["OrganizationType"]) : ExternalOriginatorOrgType.Company;
      return externalUserInfo;
    }

    private static ExternalUserInfo databaseRowToExternalUserInfoAllTPO(DataRow r)
    {
      ExternalUserInfo externalUserInfoAllTpo = new ExternalUserInfo();
      externalUserInfoAllTpo.ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOrgID"]);
      externalUserInfoAllTpo.ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["external_userid"]);
      externalUserInfoAllTpo.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]);
      externalUserInfoAllTpo.LastName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Last_name"], (object) "");
      externalUserInfoAllTpo.FirstName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["First_name"], (object) "");
      externalUserInfoAllTpo.Title = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Title"], (object) "");
      externalUserInfoAllTpo.Phone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Phone"], (object) "");
      externalUserInfoAllTpo.State = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["State"], (object) "");
      externalUserInfoAllTpo.Email = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Email"], (object) "");
      externalUserInfoAllTpo.DisabledLogin = (UserInfo.UserStatusEnum) r["Status"] == UserInfo.UserStatusEnum.Disabled;
      externalUserInfoAllTpo.ApprovalCurrentStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Approval_status"]);
      externalUserInfoAllTpo.LastLogin = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Last_Login"], DateTime.MinValue);
      externalUserInfoAllTpo.Roles = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Roles"]);
      externalUserInfoAllTpo.EmailForLogin = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Login_email"]);
      externalUserInfoAllTpo.UseCompanyAddress = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UseCompanyAddress"]);
      externalUserInfoAllTpo.OrgVisibleOnTpowcSite = r.Table.Columns.Contains("VisibleOnTPOWCSite") && EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["VisibleOnTPOWCSite"]);
      externalUserInfoAllTpo.OrganizationType = r.Table.Columns.Contains("OrganizationType") ? (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["OrganizationType"]) : ExternalOriginatorOrgType.Company;
      return externalUserInfoAllTpo;
    }

    private static ExternalUserInfo databaseRowToExternalUserInfoAllTPO(
      DataRow r,
      UserInfo userInfo)
    {
      ExternalUserInfo externalUserInfoAllTpo = new ExternalUserInfo(userInfo);
      externalUserInfoAllTpo.ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ExternalOrgID"]);
      externalUserInfoAllTpo.ExternalUserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["external_userid"]);
      externalUserInfoAllTpo.ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]);
      externalUserInfoAllTpo.LastName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Last_name"], (object) "");
      externalUserInfoAllTpo.FirstName = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["First_name"], (object) "");
      externalUserInfoAllTpo.Title = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Title"], (object) "");
      externalUserInfoAllTpo.Phone = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Phone"], (object) "");
      externalUserInfoAllTpo.State = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["State"], (object) "");
      externalUserInfoAllTpo.Email = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["Email"], (object) "");
      externalUserInfoAllTpo.DisabledLogin = (UserInfo.UserStatusEnum) r["Status"] == UserInfo.UserStatusEnum.Disabled;
      externalUserInfoAllTpo.ApprovalCurrentStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Approval_status"]);
      externalUserInfoAllTpo.LastLogin = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["Last_Login"], DateTime.MinValue);
      externalUserInfoAllTpo.Roles = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Roles"]);
      externalUserInfoAllTpo.EmailForLogin = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["Login_email"]);
      externalUserInfoAllTpo.UseCompanyAddress = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["UseCompanyAddress"]);
      externalUserInfoAllTpo.OrgVisibleOnTpowcSite = r.Table.Columns.Contains("VisibleOnTPOWCSite") && EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["VisibleOnTPOWCSite"]);
      externalUserInfoAllTpo.OrganizationType = r.Table.Columns.Contains("OrganizationType") ? (ExternalOriginatorOrgType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["OrganizationType"]) : ExternalOriginatorOrgType.Company;
      return externalUserInfoAllTpo;
    }

    private static Persona databaseRowToExternalUserInfoUserPersona(DataRow r)
    {
      return new Persona((int) r["personaID"], EllieMae.EMLite.DataAccess.SQL.DecodeString(r["personaName"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["aclFeaturesDefault"]), (int) r["displayOrder"], EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["isInternal"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["isExternal"]));
    }

    public static ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense)
    {
      return ExternalOrgManagementAccessor.SaveExternalUserInfo(newExternalUserInfo, cleanUpStateLicenseBeforeUpdateLicense, false);
    }

    public static ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      bool setDefaultURL)
    {
      return ExternalOrgManagementAccessor.SaveExternalUserInfo(newExternalUserInfo, cleanUpStateLicenseBeforeUpdateLicense, setDefaultURL, (int[]) null);
    }

    private static string InsertExternalUserQuery(ExternalUserInfo newExternalUserInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("INSERT INTO [users] ([userid],[org_id],[persona],[last_name],[first_name],[middle_name],[suffix_name], [status],[email],[phone],[password_changed],[require_pwd_change],[fax], [cell_phone],[nmlsOriginatorID],[user_type], [peerView], [access_mode], [jobtitle],[CreatedDate],[CreatedBy],[LastModifiedDate],[LastModifiedBy])  VALUES (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID) + ",@orgChartId, '0'," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.LastName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FirstName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.MiddleName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Suffix) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.DisabledLogin) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Email) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Phone) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Today) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.RequirePasswordChange) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Fax) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.CellPhone) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.NmlsID) + ",'External'," + (object) (int) newExternalUserInfo.PeerView + "," + (object) (int) newExternalUserInfo.AccessMode + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Title) + ",GETUTCDATE()," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.UpdatedBy) + ",GETUTCDATE()," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.UpdatedBy) + ");");
      dbQueryBuilder.AppendLine("INSERT INTO [ExternalUsers] ([external_userid],[contactID], [Last_name], [First_name], [Middle_name], [Suffix_name], [Title], [Address1], [City], [State], [Zip], [ExternalOrgID], [Phone], [Cell_phone],[Password_changed] , [Fax], [Email], [NMLSOriginatorID], [SSN], [InheritParentRateSheet], [Rate_sheet_email], [Rate_sheet_fax], [Lock_info_email], [Lock_info_fax], [Status], [Login_email], [Welcome_Email_Date], [Welcome_Email_By], [Approval_status], [Approval_status_watchlist], [Approval_status_date], [Approval_date], [Require_pwd_change], [Sales_rep_userid], [Roles], [Note], [UpdatedBy], [UpdatedByExternal],[nmlscurrent], [UseCompanyAddress]) VALUES (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.LastName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FirstName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.MiddleName) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Suffix) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Title) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Address) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.City) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.State) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Zipcode) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ExternalOrgID) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Phone) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.CellPhone) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.PasswordChangedDate) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Fax) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Email) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.NmlsID) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.SSN) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UseParentInfoForRateLock) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForRateSheet) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FaxForRateSheet) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForLockInfo) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FaxForLockInfo) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.DisabledLogin) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForLogin) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.WelcomeEmailDate) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.WelcomeEmailUserName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ApprovalCurrentStatus) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.AddToWatchlist) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.ApprovalCurrentStatusDate) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.ApprovalDate) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.RequirePasswordChange) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.SalesRepID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.Roles) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.Notes) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.UpdatedBy) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UpdatedByExternal) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.NMLSCurrent) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UseCompanyAddress) + ")");
      return dbQueryBuilder.ToString();
    }

    private static DataSet GetExternalUsersUpsertDataSet(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      DataSet usersUpsertDataSet = new DataSet();
      DataTable dtTable1 = new DataTable();
      dtTable1.Columns.AddRange(new DataColumn[41]
      {
        new DataColumn("external_userid", typeof (string)),
        new DataColumn("ContactID", typeof (string)),
        new DataColumn("Last_name", typeof (string)),
        new DataColumn("Middle_name", typeof (string)),
        new DataColumn("Suffix_name", typeof (string)),
        new DataColumn("First_name", typeof (string)),
        new DataColumn("Title", typeof (string)),
        new DataColumn("Address1", typeof (string)),
        new DataColumn("City", typeof (string)),
        new DataColumn("State", typeof (string)),
        new DataColumn("Zip", typeof (string)),
        new DataColumn("externalOrgID", typeof (int)),
        new DataColumn("Status", typeof (int)),
        new DataColumn("Email", typeof (string)),
        new DataColumn("Phone", typeof (string)),
        new DataColumn("Require_pwd_change", typeof (bool)),
        new DataColumn("Fax", typeof (string)),
        new DataColumn("Cell_phone", typeof (string)),
        new DataColumn("NMLSOriginatorID", typeof (string)),
        new DataColumn("SSN", typeof (string)),
        new DataColumn("InheritParentRateSheet", typeof (bool)),
        new DataColumn("Rate_sheet_email", typeof (string)),
        new DataColumn("Rate_sheet_fax", typeof (string)),
        new DataColumn("Lock_info_email", typeof (string)),
        new DataColumn("Lock_info_fax", typeof (string)),
        new DataColumn("Login_email", typeof (string)),
        new DataColumn("Approval_status", typeof (int)),
        new DataColumn("Approval_status_watchlist", typeof (bool)),
        new DataColumn("Approval_status_date", typeof (DateTime)),
        new DataColumn("Approval_date", typeof (DateTime)),
        new DataColumn("Sales_rep_userid", typeof (string)),
        new DataColumn("Roles", typeof (int)),
        new DataColumn("Note", typeof (string)),
        new DataColumn("NMLSCurrent", typeof (int)),
        new DataColumn("UpdatedBy", typeof (string)),
        new DataColumn("UpdatedByExternal", typeof (bool)),
        new DataColumn("UseCompanyAddress", typeof (bool)),
        new DataColumn("PeerView", typeof (int)),
        new DataColumn("Access_mode", typeof (int)),
        new DataColumn("CreatedBy", typeof (string)),
        new DataColumn("LastModifiedBy", typeof (string))
      });
      DataTable dttblUserPersonas = new DataTable();
      dttblUserPersonas.Columns.AddRange(new DataColumn[2]
      {
        new DataColumn("id_1", typeof (int)),
        new DataColumn("id_2", typeof (string))
      });
      DataTable dttblExternalUserURLs = new DataTable();
      dttblExternalUserURLs.Columns.AddRange(new DataColumn[2]
      {
        new DataColumn("id_1", typeof (int)),
        new DataColumn("id_2", typeof (string))
      });
      DataTable dtTable2 = new DataTable();
      dtTable2.Columns.AddRange(new DataColumn[11]
      {
        new DataColumn("external_userid", typeof (string)),
        new DataColumn("State", typeof (string)),
        new DataColumn("LicenseApproved", typeof (bool)),
        new DataColumn("LicenseExempt", typeof (bool)),
        new DataColumn("LicenseNumber", typeof (string)),
        new DataColumn("IssueDate", typeof (DateTime)),
        new DataColumn("StartDate", typeof (DateTime)),
        new DataColumn("EndDate", typeof (DateTime)),
        new DataColumn("Status", typeof (string)),
        new DataColumn("StatusDate", typeof (DateTime)),
        new DataColumn("LastCheckedDate", typeof (DateTime))
      });
      DataTable dataTable = new DataTable();
      dataTable.Columns.AddRange(new DataColumn[2]
      {
        new DataColumn("Guid", typeof (string)),
        new DataColumn("StringValue", typeof (string))
      });
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ExternalUsers");
      foreach (Tuple<ExternalUserInfo, int[]> extUserInfo1 in extUserInfos)
      {
        ExternalUserInfo extUserInfo = extUserInfo1.Item1;
        if (string.IsNullOrEmpty(extUserInfo.ExternalUserID))
          extUserInfo.ExternalUserID = Guid.NewGuid().ToString();
        DbValueList values1 = new DbValueList();
        values1.Add("external_userid", (object) extUserInfo.ExternalUserID);
        values1.Add("ContactID", (object) extUserInfo.ContactID);
        values1.Add("Last_name", (object) extUserInfo.LastName);
        values1.Add("Middle_name", (object) extUserInfo.MiddleName);
        values1.Add("Suffix_name", (object) extUserInfo.Suffix);
        values1.Add("First_name", (object) extUserInfo.FirstName);
        values1.Add("Title", (object) extUserInfo.Title);
        values1.Add("Address1", (object) extUserInfo.Address);
        values1.Add("City", (object) extUserInfo.City);
        values1.Add("State", (object) extUserInfo.State);
        values1.Add("Zip", (object) extUserInfo.Zipcode);
        values1.Add("externalOrgID", (object) extUserInfo.ExternalOrgID);
        values1.Add("Status", (object) extUserInfo.DisabledLogin);
        values1.Add("Email", (object) extUserInfo.Email);
        values1.Add("Phone", (object) extUserInfo.Phone);
        values1.Add("Require_pwd_change", (object) extUserInfo.RequirePasswordChange);
        values1.Add("Fax", (object) extUserInfo.Fax);
        values1.Add("Cell_phone", (object) extUserInfo.CellPhone);
        values1.Add("NMLSOriginatorID", (object) extUserInfo.NmlsID);
        values1.Add("SSN", (object) extUserInfo.SSN);
        values1.Add("InheritParentRateSheet", (object) extUserInfo.UseParentInfoForRateLock);
        values1.Add("Rate_sheet_email", (object) extUserInfo.EmailForRateSheet);
        values1.Add("Rate_sheet_fax", (object) extUserInfo.FaxForRateSheet);
        values1.Add("Lock_info_email", (object) extUserInfo.EmailForLockInfo);
        values1.Add("Lock_info_fax", (object) extUserInfo.FaxForLockInfo);
        values1.Add("Login_email", (object) extUserInfo.EmailForLogin);
        values1.Add("Approval_status", (object) extUserInfo.ApprovalCurrentStatus);
        values1.Add("Approval_status_watchlist", (object) extUserInfo.AddToWatchlist);
        if (extUserInfo.ApprovalCurrentStatusDate != DateTime.MinValue)
          values1.Add("Approval_status_date", (object) extUserInfo.ApprovalCurrentStatusDate);
        else
          values1.Add("Approval_status_date", (object) DBNull.Value);
        if (extUserInfo.ApprovalDate != DateTime.MinValue)
          values1.Add("Approval_date", (object) extUserInfo.ApprovalDate);
        else
          values1.Add("Approval_date", (object) DBNull.Value);
        values1.Add("Sales_rep_userid", (object) extUserInfo.SalesRepID);
        values1.Add("Roles", (object) extUserInfo.Roles);
        values1.Add("Note", (object) extUserInfo.Notes);
        values1.Add("NMLSCurrent", (object) extUserInfo.NMLSCurrent);
        values1.Add("UpdatedBy", (object) extUserInfo.UpdatedBy);
        values1.Add("UpdatedByExternal", (object) extUserInfo.UpdatedByExternal);
        values1.Add("UseCompanyAddress", (object) extUserInfo.UseCompanyAddress);
        values1.Add("PeerView", (object) extUserInfo.PeerView);
        values1.Add("Access_mode", (object) extUserInfo.AccessMode);
        if (extUserInfo.Userid == null)
          values1.Add("CreatedBy", (object) extUserInfo.createdBy);
        else if (!string.IsNullOrEmpty(extUserInfo.createdBy))
          values1.Add("CreatedBy", (object) extUserInfo.createdBy);
        else
          values1.Add("CreatedBy", (object) DBNull.Value);
        values1.Add("LastModifiedBy", (object) extUserInfo.UpdatedBy);
        sql.InsertIntoDataTable(dtTable1, table1, values1);
        ((IEnumerable<Persona>) extUserInfo.UserPersonas).ToList<Persona>().ForEach((Action<Persona>) (p => dttblUserPersonas.Rows.Add((object) p.ID, (object) extUserInfo.ContactID)));
        if (extUserInfo1.Item2 != null && ((IEnumerable<int>) extUserInfo1.Item2).Count<int>() > 0)
          ((IEnumerable<int>) extUserInfo1.Item2).ToList<int>().ForEach((Action<int>) (u => dttblExternalUserURLs.Rows.Add((object) u, (object) extUserInfo.ExternalUserID)));
        else
          dttblExternalUserURLs.Rows.Add(new object[2]
          {
            null,
            (object) extUserInfo.ExternalUserID
          });
        if (extUserInfo.Licenses != null && extUserInfo.Licenses.Count > 0)
        {
          DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ExternalUserStateLicensing");
          foreach (StateLicenseExtType license in extUserInfo.Licenses)
          {
            DbValueList values2 = new DbValueList();
            values2.Add("external_userid", (object) extUserInfo.ExternalUserID);
            values2.Add("State", (object) license.StateAbbrevation);
            values2.Add("LicenseApproved", (object) (license.Approved ? 1 : 0));
            values2.Add("LicenseExempt", (object) (license.Exempt ? 1 : 0));
            values2.Add("LicenseNumber", (object) license.LicenseNo);
            if (license.IssueDate != DateTime.MinValue)
              values2.Add("IssueDate", (object) license.IssueDate);
            else
              values2.Add("IssueDate", (object) DBNull.Value);
            if (license.StartDate != DateTime.MinValue)
              values2.Add("StartDate", (object) license.StartDate);
            else
              values2.Add("StartDate", (object) DBNull.Value);
            if (license.EndDate != DateTime.MinValue)
              values2.Add("EndDate", (object) license.EndDate);
            else
              values2.Add("EndDate", (object) DBNull.Value);
            values2.Add("Status", (object) license.LicenseStatus);
            if (license.StatusDate != DateTime.MinValue)
              values2.Add("StatusDate", (object) license.StatusDate);
            else
              values2.Add("StatusDate", (object) DBNull.Value);
            if (license.LastChecked != DateTime.MinValue)
              values2.Add("LastCheckedDate", (object) license.LastChecked);
            else
              values2.Add("LastCheckedDate", (object) DBNull.Value);
            sql.InsertIntoDataTable(dtTable2, table2, values2);
          }
        }
        dataTable.Rows.Add((object) extUserInfo.ExternalUserID, (object) extUserInfo.ContactID);
      }
      usersUpsertDataSet.Tables.AddRange(new DataTable[5]
      {
        dtTable1,
        dttblUserPersonas,
        dttblExternalUserURLs,
        dtTable2,
        dataTable
      });
      return usersUpsertDataSet;
    }

    private static string UpdateExternalUserQuery(ExternalUserInfo newExternalUserInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("UPDATE [users] SET userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID) + ",org_id = @orgChartId, last_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.LastName) + ", first_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FirstName) + ", middle_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.MiddleName) + ", suffix_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Suffix) + ", status = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.DisabledLogin) + ", email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Email) + ", phone = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Phone) + ", fax = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Fax) + ", failed_login_attempts = " + (object) newExternalUserInfo.failed_login_attempts + ", cell_phone = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.CellPhone) + ", nmlsOriginatorID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.NmlsID) + ", peerView  = " + (object) (int) newExternalUserInfo.PeerView + ", access_mode = " + (object) (int) newExternalUserInfo.AccessMode + ", jobtitle = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Title) + ", LastModifiedBy = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.UpdatedBy) + ", LastModifiedDate = GETUTCDATE() where userid = (SELECT [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID) + ");");
      dbQueryBuilder.AppendLine("Update [ExternalUsers] Set ContactID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID) + ", Last_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.LastName) + ", First_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FirstName) + ", Middle_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.MiddleName) + ", Suffix_name = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Suffix) + ", Title = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Title) + ", Address1 = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Address) + ", City = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.City) + ", State = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.State) + ", Zip = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Zipcode) + ", ExternalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ExternalOrgID) + ", Phone = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Phone) + ", password_changed = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.PasswordChangedDate) + ", Cell_phone = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.CellPhone) + ", Fax = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Fax) + ", Email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.Email) + ", NMLSOriginatorID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.NmlsID) + ", SSN = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.SSN) + ", InheritParentRateSheet = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UseParentInfoForRateLock) + ", Rate_sheet_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForRateSheet) + ", Rate_sheet_fax = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FaxForRateSheet) + ", Lock_info_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForLockInfo) + ", Lock_info_fax = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.FaxForLockInfo) + ", Status = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.DisabledLogin) + ", Login_email = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.EmailForLogin) + ", Welcome_Email_Date = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.WelcomeEmailDate) + ", Welcome_Email_By = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.WelcomeEmailUserName) + ", Approval_status = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ApprovalCurrentStatus) + ", Approval_status_watchlist = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.AddToWatchlist) + ", Approval_status_date = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.ApprovalCurrentStatusDate) + ", Approval_date = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.ApprovalDate) + ", require_pwd_change = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.RequirePasswordChange) + ", Sales_rep_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.SalesRepID) + ", Roles =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.Roles) + ", Note =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.Notes) + ", UpdatedBy =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.UpdatedBy) + ", UpdatedByExternal =" + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UpdatedByExternal) + ", UpdatedDttm = GETDATE(),nmlscurrent =" + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.NMLSCurrent) + ", UseCompanyAddress = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(newExternalUserInfo.UseCompanyAddress) + " where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID));
      return dbQueryBuilder.ToString();
    }

    private static string InsertUserLicensingQuery(StateLicenseExtType state, string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("INSERT INTO ExternalUserStateLicensing VALUES ('" + externalUserID + "'," + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StateAbbrevation) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (state.Approved ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (state.Exempt ? 1 : 0)) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LicenseNo) + "," + (state.IssueDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.IssueDate) : "NULL") + "," + (state.StartDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StartDate) : "NULL") + "," + (state.EndDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.EndDate) : "NULL") + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LicenseStatus) + "," + (state.StatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StatusDate) : "NULL") + "," + (state.LastChecked != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LastChecked) : "NULL") + ")");
      return dbQueryBuilder.ToString();
    }

    private static string UpdateUserLicensingQuery(StateLicenseExtType state, string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("UPDATE ExternalUserStateLicensing SET [State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StateAbbrevation) + ",[LicenseApproved] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (state.Approved ? 1 : 0)) + ",[LicenseExempt] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (state.Exempt ? 1 : 0)) + ",[LicenseNumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LicenseNo) + ",[IssueDate] = " + (state.IssueDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.IssueDate) : "NULL") + ",[StartDate] = " + (state.StartDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StartDate) : "NULL") + ",[EndDate] = " + (state.EndDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.EndDate) : "NULL") + ",[Status] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LicenseStatus) + ",[StatusDate] = " + (state.StatusDate != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.StatusDate) : "NULL") + ",[LastCheckedDate] = " + (state.LastChecked != DateTime.MinValue ? EllieMae.EMLite.DataAccess.SQL.Encode((object) state.LastChecked) : "NULL") + " WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserID) + " and State = '" + state.StateAbbrevation + "'");
      return dbQueryBuilder.ToString();
    }

    private static string SaveExternalUserPersonaQuery(ExternalUserInfo newExternalUserInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (newExternalUserInfo.UserPersonas != null)
      {
        dbQueryBuilder.AppendLine("DELETE FROM [UserPersona] WHERE [userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID));
        foreach (Persona userPersona in newExternalUserInfo.UserPersonas)
          dbQueryBuilder.AppendLine("INSERT INTO [UserPersona] ([userid],[personaID]) VALUES (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ContactID) + "," + (object) userPersona.ID + ")");
      }
      return dbQueryBuilder.ToString();
    }

    public static ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      bool setDefaultURL,
      int[] siteURLs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string eventType = string.IsNullOrEmpty(newExternalUserInfo.ExternalUserID) ? "create" : "update";
      if (string.IsNullOrEmpty(newExternalUserInfo.ExternalUserID))
        newExternalUserInfo.ExternalUserID = Guid.NewGuid().ToString();
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalUserInfo: Creating SQL commands for table ExternalUsers.");
      dbQueryBuilder.Declare("@orgChartId", "INT");
      dbQueryBuilder.SelectVar("@orgChartId", (object) ExternalOrgManagementAccessor.getOrgChartIdForExternalOid(newExternalUserInfo.ExternalOrgID, false));
      dbQueryBuilder.AppendLine("if not exists (select 1 from [ExternalUsers] where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID) + ")");
      dbQueryBuilder.AppendLine("Begin");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertExternalUserQuery(newExternalUserInfo));
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("else");
      dbQueryBuilder.AppendLine("Begin");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateExternalUserQuery(newExternalUserInfo));
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SaveExternalUserPersonaQuery(newExternalUserInfo));
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfo: Cannot Create new External User due to the following issue:\r\n" + ex.Message);
      }
      if (cleanUpStateLicenseBeforeUpdateLicense)
        dbQueryBuilder.AppendLine("DELETE FROM [ExternalUserStateLicensing] WHERE [external_userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ExternalUserID));
      foreach (StateLicenseExtType license in newExternalUserInfo.Licenses)
      {
        string text = ExternalOrgManagementAccessor.UpdateUserLicensingQuery(license, newExternalUserInfo.ExternalUserID);
        dbQueryBuilder.AppendLine("if not exists (select 1 from ExternalUserStateLicensing WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newExternalUserInfo.ExternalUserID) + " and State = '" + license.StateAbbrevation + "')");
        dbQueryBuilder.AppendLine("Begin");
        dbQueryBuilder.AppendLine("Begin Try");
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertUserLicensingQuery(license, newExternalUserInfo.ExternalUserID));
        dbQueryBuilder.AppendLine("End Try\n");
        dbQueryBuilder.AppendLine("Begin Catch");
        dbQueryBuilder.AppendLine("if (error_number() = '2627')");
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.AppendLine("End Catch");
        dbQueryBuilder.AppendLine("End");
        dbQueryBuilder.AppendLine("else\n");
        dbQueryBuilder.AppendLine("Begin\n");
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.AppendLine("End\n");
      }
      try
      {
        if (!string.IsNullOrWhiteSpace(dbQueryBuilder.ToString()))
        {
          TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfo: External User has been created but Cannot insert User's State Licenses due to the following issue:\r\n" + ex.Message);
      }
      if (setDefaultURL)
      {
        ExternalUserURL[] externalUserInfoUrLs = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(newExternalUserInfo.ExternalUserID);
        if (externalUserInfoUrLs == null || externalUserInfoUrLs.Length == 0)
        {
          List<ExternalOrgURL> selectedOrgUrls = ExternalOrgManagementAccessor.GetSelectedOrgUrls(newExternalUserInfo.ExternalOrgID);
          if (selectedOrgUrls != null && selectedOrgUrls.Count == 1)
            dbQueryBuilder.Append(ExternalOrgManagementAccessor.buildSiteURLs(newExternalUserInfo.ExternalUserID, new int[1]
            {
              selectedOrgUrls[0].URLID
            }).ToString());
        }
      }
      try
      {
        if (!string.IsNullOrWhiteSpace(dbQueryBuilder.ToString()))
        {
          TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfo: External User has been created but Cannot insert User's Default URL due to the following issue:\r\n" + ex.Message);
      }
      if (siteURLs != null && siteURLs.Length != 0)
        dbQueryBuilder.Append(ExternalOrgManagementAccessor.buildSiteURLs(newExternalUserInfo.ExternalUserID, siteURLs).ToString());
      try
      {
        if (!string.IsNullOrWhiteSpace(dbQueryBuilder.ToString()))
        {
          TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfo: External User has been created but Cannot insert User's Site URLs due to the following issue:\r\n" + ex.Message);
      }
      ExternalOrgManagementAccessor.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
      {
        newExternalUserInfo.ContactID
      }, UserWebhookMessage.UserType.ExternalUsers, eventType, newExternalUserInfo.UpdatedBy);
      try
      {
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(newExternalUserInfo.ExternalUserID);
        ExternalOrgManagementAccessor.removeExternalUserCache(externalUserInfo.ContactID);
        return externalUserInfo;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static List<ExternalUserInfo> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool isCreate)
    {
      List<ExternalUserInfo> updatedExtUsers;
      ExternalOrgManagementAccessor.UpsertExternalUserInfos(extUserInfos, true, out updatedExtUsers, isCreate);
      return updatedExtUsers;
    }

    public static List<string> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool returnUpdatedResult,
      out List<ExternalUserInfo> updatedExtUsers,
      bool isCreate)
    {
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.UpsertExternalUsersInfos: Creating SQL commands for table ExternalUsers.");
      updatedExtUsers = (List<ExternalUserInfo>) null;
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      List<SqlParameter> sqlParameterList = new List<SqlParameter>();
      DataSet usersUpsertDataSet = ExternalOrgManagementAccessor.GetExternalUsersUpsertDataSet(extUserInfos, sql);
      SqlParameter sqlParameter1 = new SqlParameter("@externalUsers", (object) usersUpsertDataSet.Tables[0])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_ExternalUsers"
      };
      sql.AppendLine("EXEC UpsertExternalUsers @externalUsers");
      sqlParameterList.Add(sqlParameter1);
      SqlParameter sqlParameter2 = new SqlParameter("@externalUserPersonas", (object) usersUpsertDataSet.Tables[1])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_CompositeIds"
      };
      sql.AppendLine("EXEC UpsertExternalUserPersonas @externalUserPersonas");
      sqlParameterList.Add(sqlParameter2);
      SqlParameter sqlParameter3 = new SqlParameter("@externalUserURLs", (object) usersUpsertDataSet.Tables[2])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_CompositeIds"
      };
      sql.AppendLine("EXEC UpsertExternalUserURLs @externalUserURLs");
      sqlParameterList.Add(sqlParameter3);
      if (usersUpsertDataSet.Tables[3].Rows.Count > 0)
      {
        SqlParameter sqlParameter4 = new SqlParameter("@externalUserStateLicensing", (object) usersUpsertDataSet.Tables[3])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_ExternalUserStateLicensing"
        };
        sql.AppendLine("EXEC UpsertExternalUserStateLicensing @externalUserStateLicensing");
        sqlParameterList.Add(sqlParameter4);
      }
      if (returnUpdatedResult)
      {
        SqlParameter sqlParameter5 = new SqlParameter("@externalUserIds", (object) usersUpsertDataSet.Tables[4])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_GuidStringPair"
        };
        sql.AppendLine("EXEC [GetExternalUserDetails] @externalUserIds");
        sqlParameterList.Add(sqlParameter5);
      }
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), sql.ToString());
        using (SqlCommand sqlCmd = new SqlCommand())
        {
          sqlCmd.CommandText = sql.ToString();
          sqlCmd.Parameters.AddRange(sqlParameterList.ToArray());
          DataSet dsExternalUserDetails = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd);
          if (returnUpdatedResult)
            updatedExtUsers = ExternalOrgManagementAccessor.getExternalUserDetails(dsExternalUserDetails);
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpsertExternalUsersInfos: Cannot Create new External Users due to the following issue:\r\n" + ex.Message);
      }
      finally
      {
        sql.Reset();
      }
      string eventType = isCreate ? "create" : "update";
      IEnumerable<string> strings = extUserInfos.Select<Tuple<ExternalUserInfo, int[]>, string>((System.Func<Tuple<ExternalUserInfo, int[]>, string>) (e => e.Item1.ContactID));
      ExternalOrgManagementAccessor.PublishUserKafkaEvent(strings, UserWebhookMessage.UserType.ExternalUsers, eventType, extUserInfos.FirstOrDefault<Tuple<ExternalUserInfo, int[]>>().Item1.UpdatedBy);
      List<string> list = strings.ToList<string>();
      ExternalOrgManagementAccessor.removeExternalUserCache(list);
      return list;
    }

    private static bool PublishUserKafkaEvent(
      IEnumerable<string> contactIds,
      UserWebhookMessage.UserType userType,
      string eventType,
      string loggedInUser)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool flag = false;
      try
      {
        UserWebhookEvent queueEvent = new UserWebhookEvent(current.InstanceName, loggedInUser, EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.UtcNow);
        queueEvent.CreateUserMessage(queueEvent.StandardMessage.EntityId, ClientContext.CurrentRequest.CorrelationId, current.InstanceName, loggedInUser, eventType, userType, current.ClientID, contactIds);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ExternalOrgManagementAccessor), string.Format("Exception publishing userEvent to kafka for userId - {0}. Exception details {1}", (object) loggedInUser, (object) ex.StackTrace));
      }
      return flag;
    }

    public static bool DeleteExternalUserInfo(
      int externalOrgID,
      ExternalUserInfo deletedExternalUserInfo,
      UserInfo userInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM [UserPersona] WHERE [userid] = (SELECT [ContactID] FROM [ExternalUsers] WHERE  externalOrgID = " + (object) externalOrgID + " AND external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(deletedExternalUserInfo.ExternalUserID) + ")");
        dbQueryBuilder.AppendLine("DELETE from [ExternalUsers] WHERE externalOrgID = " + (object) externalOrgID + " AND external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(deletedExternalUserInfo.ExternalUserID));
        dbQueryBuilder.AppendLine("DELETE FROM [users] WHERE [userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(deletedExternalUserInfo.ContactID));
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
        {
          deletedExternalUserInfo.Userid
        }, UserWebhookMessage.UserType.ExternalUsers, "delete", userInfo.Userid);
        ExternalOrgManagementAccessor.removeExternalUserCache(deletedExternalUserInfo.ContactID);
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalUserInfo: Cannot delete record in [ExternalUsers] due to this error: " + ex.Message);
      }
    }

    public static bool DeleteExternalUserInfos(
      int externalOrgID,
      List<ExternalUserInfo> extUsers,
      UserInfo userInfo)
    {
      if (!extUsers.Any<ExternalUserInfo>())
        throw new ArgumentException("At leat one user should be provided in the list.", nameof (extUsers));
      if (extUsers.Count > 100)
        throw new ArgumentException("A maximum of 100 users can be deleted at a time.", nameof (extUsers));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str = string.Join(",", extUsers.Select<ExternalUserInfo, string>((System.Func<ExternalUserInfo, string>) (x => EllieMae.EMLite.DataAccess.SQL.EncodeString(x.ExternalUserID)))) ?? "";
        dbQueryBuilder.AppendLine("DECLARE @contactIds TABLE (contactId varchar(16) PRIMARY KEY)");
        dbQueryBuilder.AppendLine("INSERT INTO @contactIds");
        dbQueryBuilder.AppendLine(string.Format("SELECT [ContactID] FROM [ExternalUsers] WHERE [externalOrgID] = {0} AND [external_userid] IN ({1})", (object) externalOrgID, (object) str));
        dbQueryBuilder.AppendLine("DELETE FROM [UserPersona] WHERE [userid] IN (SELECT contactId FROM @contactIds)");
        dbQueryBuilder.AppendLine("DELETE FROM [ExternalUsers] WHERE [ContactID] IN (SELECT contactId FROM @contactIds)");
        dbQueryBuilder.AppendLine("DELETE FROM [users] WHERE [userid] IN (SELECT contactId FROM @contactIds)");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.PublishUserKafkaEvent(extUsers.Select<ExternalUserInfo, string>((System.Func<ExternalUserInfo, string>) (x => x.ContactID)), UserWebhookMessage.UserType.ExternalUsers, "delete", userInfo.Userid);
        ExternalOrgManagementAccessor.removeExternalUserCache(extUsers);
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalUserInfos: Cannot delete records in [ExternalUsers] due to this error: " + ex.Message);
      }
    }

    public static bool UpdateExternalUserLastLogin(ExternalUserInfo newExternalUserInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("Update [ExternalUsers] Set Last_Login =" + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(newExternalUserInfo.LastLogin) + " where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID));
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder1.ToString());
        dbQueryBuilder1.ExecuteNonQuery();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.AppendLine("Update [users] Set failed_login_attempts =" + (object) newExternalUserInfo.failed_login_attempts + " where userid = (SELECT [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(newExternalUserInfo.ExternalUserID) + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder2.ToString());
        dbQueryBuilder2.ExecuteNonQuery();
        ExternalOrgManagementAccessor.removeExternalUserCache(newExternalUserInfo.ContactID);
        return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalUserLastLogin: Cannot update last Login to table 'ExternalUsers' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo ResetExternalUserInfoPassword(
      string externaluserID,
      string newPassword,
      DateTime date,
      string updatedBy,
      bool requirePasswordChange)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Update [users] Set " + (requirePasswordChange ? "require_pwd_change = 1, " : "require_pwd_change = 0, ") + "password_changed = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(date) + ", Password = PwdEncrypt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalOrgManagementAccessor.getCaseSensitivePassword(newPassword)) + ") where userid = (SELECT [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externaluserID) + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Update [ExternalUsers] Set " + (requirePasswordChange ? "Require_pwd_change = 1, " : "Require_pwd_change = 0, ") + "Password_changed = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(date) + ", Password = PwdEncrypt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) ExternalOrgManagementAccessor.getCaseSensitivePassword(newPassword)) + ") ,UpdatedBy =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) updatedBy) + ", UpdatedByExternal =" + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(false) + ", UpdatedDttm =" + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + "where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externaluserID));
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        byte[] numArray = new PasswordEncryptor().Hash(newPassword);
        dbQueryBuilder.AppendLine("declare @contactID varchar(16)");
        dbQueryBuilder.AppendLine("select @contactID = [ContactID] FROM [ExternalUsers] WHERE external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externaluserID));
        dbQueryBuilder.AppendLine("if exists (select * from [UserCredentials] where userid = @contactID)");
        dbQueryBuilder.AppendLine("    update [UserCredentials] set password = @passwordHash where userid = @contactID");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    insert into [UserCredentials] values (@contactID, @passwordHash)");
        DbCommandParameter[] parameters = new DbCommandParameter[1]
        {
          new DbCommandParameter("@passwordHash", (object) numArray, DbType.Binary)
        };
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default, parameters);
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(externaluserID);
        ExternalOrgManagementAccessor.removeExternalUserCache(externalUserInfo.ContactID);
        return externalUserInfo;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("ResetExternalUserInfoPassword: Cannot update password to table 'ExternalUsers' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void SendWelcomeEmailUserInfo(
      string externaluserID,
      DateTime date,
      string userName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [ExternalUsers] Set [Welcome_Email_Date] = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", [Welcome_Email_By] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userName) + " where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externaluserID));
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SendWelcomeEmailUserInfo: Cannot update welcome email info to table 'ExternalUsers' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static ExternalUserInfo ResetExternalUserInfoUpdatedDate(
      string externaluserID,
      DateTime date,
      string updatedBy,
      bool updatedByExternal)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [ExternalUsers] Set UpdatedBy =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) updatedBy) + ", UpdatedByExternal =" + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(updatedByExternal) + ", UpdatedDttm =" + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(date) + "where external_userid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externaluserID));
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(externaluserID);
        ExternalOrgManagementAccessor.removeExternalUserCache(externalUserInfo.ContactID);
        return externalUserInfo;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("ResetExternalUserInfoPassword: Cannot update password to table 'ExternalUsers' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static ExternalUserURL[] GetExternalUserInfoURLs(string externalUserID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUserURLs] a inner join [ExternalOrgURLs] b  on a.[urlID] = b.[urlID] WHERE a.[external_userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalUserID) + " ORDER BY a.[AddedDateTime]");
      try
      {
        List<ExternalUserURL> externalUserUrlList = new List<ExternalUserURL>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
            externalUserUrlList.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserURL(r));
        }
        return externalUserUrlList.ToArray();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static List<AclGroup> GetExternalUserGroups(string contactId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT agm.UserID, ag.* FROM AclGroups ag INNER JOIN AclGroupMembers agm ON ag.GroupID = agm.GroupID WHERE agm.UserID  = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(contactId) + " ORDER BY ag.groupID");
      try
      {
        List<AclGroup> externalUserGroups = new List<AclGroup>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
            externalUserGroups.Add(ExternalOrgManagementAccessor.databaseRowToAclGroup(r));
        }
        return externalUserGroups;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalUserGroups: Cannot fetch all records from [AclGroups] table.\r\n" + ex.Message);
      }
    }

    public static HashSet<string> GetMultipleExternalUserInfoURLs(string externalUserIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalUserURLs] a inner join [ExternalOrgURLs] b  on a.[urlID] = b.[urlID] WHERE a.[external_userid] in  ('" + externalUserIds + "') ORDER BY a.[AddedDateTime]");
      try
      {
        HashSet<string> externalUserInfoUrLs = new HashSet<string>();
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            externalUserInfoUrLs.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["external_userid"]));
        }
        return externalUserInfoUrLs;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetMultipleExternalUserInfoURLs: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static string GetUrlLink(int urlID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select URL from ExternalOrgURLs where urlID = " + (object) urlID);
      return dbQueryBuilder.Execute()[0]["URL"].ToString();
    }

    public static bool DuplicateExternalUserLoginEmail(int orgID, string loginEmail)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Select 1 from [ExternalUsers] WHERE [externalOrgID] = " + (object) orgID + " and [Login_email] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loginEmail));
        if (dbQueryBuilder.Execute().Count > 0)
          return true;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfoURLs: Cannot fetch all records from [ExternalUserURLs] table.\r\n" + ex.Message);
      }
      return false;
    }

    public static void SaveExternalUserInfoURLs(string externalUserID, int[] urlIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = ExternalOrgManagementAccessor.buildSiteURLs(externalUserID, urlIDs);
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.removeExternalUserCache(ExternalOrgManagementAccessor.getExternalUserContactID(externalUserID));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SaveExternalUserInfoURLs: Cannot fetch all records from [ExternalUserURLs] table.\r\n" + ex.Message);
      }
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder buildSiteURLs(
      string externalUserID,
      int[] urlIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      sql.AppendLine("Delete [ExternalUserURLs] WHERE [external_userid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalUserID));
      ((IEnumerable<int>) urlIDs).ToList<int>().ForEach((Action<int>) (x => sql.AppendLine("Insert into [ExternalUserURLs] (urlID, external_userid) values(" + (object) x + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalUserID) + ")")));
      return sql;
    }

    private static void removeExternalUserCache(string contactID)
    {
      ClientContext.GetCurrent().Cache.Remove("UserStore_" + contactID);
    }

    private static void removeExternalUserCache(List<ExternalUserInfo> externalUsers)
    {
      ClientContext ctx = ClientContext.GetCurrent();
      externalUsers.ForEach((Action<ExternalUserInfo>) (externalUser => ctx.Cache.Remove("UserStore_" + externalUser.ContactID)));
    }

    private static void removeExternalUserCache(List<string> contactIDs)
    {
      ClientContext ctx = ClientContext.GetCurrent();
      contactIDs.ForEach((Action<string>) (contactID => ctx.Cache.Remove("UserStore_" + contactID)));
    }

    public static List<ExternalOrgContact> GetExternalOrgContacts(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOrgContact> externalOrgContacts = new List<ExternalOrgContact>();
      dbQueryBuilder.AppendLine("SELECT externalOrgContactID, Null as external_userid, externalOrgID, name, title, email, phone FROM [ExternalOrgContact] WHERE [externalOrgID] = " + (object) externalOrgID);
      dbQueryBuilder.AppendLine("UNION");
      dbQueryBuilder.AppendLine("SELECT -1 as externalOrgContactID, external_userid, externalOrgID, first_name + ' ' + last_name as name, title, email, phone FROM [ExternalUsers] WHERE [isKeyContact] = 1 and [externalOrgID] = " + (object) externalOrgID);
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalOrgContacts.Add(ExternalOrgManagementAccessor.getExternalOrgContactFromDatarow(row));
        return externalOrgContacts;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgContacts: Cannot fetch records from [ExternalUsers] and [ExternalOrgContact] table.\r\n" + ex.Message);
      }
    }

    private static ExternalOrgContact getExternalOrgContactFromDatarow(DataRow row)
    {
      ExternalOrgContact contactFromDatarow = new ExternalOrgContact();
      int int32 = Convert.ToInt32(row["externalOrgContactID"]);
      if (int32 != -1)
        contactFromDatarow.ExternalOrgContactID = int32;
      else
        contactFromDatarow.ExternalUserID = Convert.ToString(row["external_userid"]);
      contactFromDatarow.Type = int32 != -1 ? ExternalOriginatorContactType.FreeEntry : ExternalOriginatorContactType.TPO;
      contactFromDatarow.ExternalOrgID = Convert.ToInt32(row["externalOrgID"]);
      contactFromDatarow.Name = Convert.ToString(row["name"]);
      contactFromDatarow.Title = Convert.ToString(row["title"]);
      contactFromDatarow.Email = Convert.ToString(row["email"]);
      contactFromDatarow.Phone = Convert.ToString(row["phone"]);
      return contactFromDatarow;
    }

    private static string AddExternalOrgTpoContactQuery(ExternalOrgContact tpoContact)
    {
      return "INSERT INTO [ExternalOrgContact] ([externalOrgID], [name], [title], [email], [phone]) VALUES (@oid," + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoContact.Name) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoContact.Title) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoContact.Email) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) tpoContact.Phone) + ")";
    }

    public static int AddExternalOrgManualContact(ExternalOrgContact obj)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@externalOrgContactID", "int");
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) obj.ExternalOrgID);
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalOrgManualContact: Creating SQL commands for table ExternalOrgContact.");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.AddExternalOrgTpoContactQuery(obj));
      dbQueryBuilder.SelectIdentity("@externalOrgContactID");
      dbQueryBuilder.Select("@externalOrgContactID");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalOrgManualContact: Cannot insert manual contact to table 'ExternalOrgContact' due to the following problem:\r\n" + ex.Message);
      }
    }

    public static ExternalOrgWrapper GetExternalOrganization(int orgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @oid int; set @oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgId));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.GetExternalOrganizationQuery());
      DataSet ds = dbQueryBuilder.ExecuteSetQuery();
      return ds != null && ds.Tables.Count > 0 ? ExternalOrgManagementAccessor.GatherExternalOrgData(ds, orgId) : (ExternalOrgWrapper) null;
    }

    private static string GetExternalOrganizationQuery()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalOrganizationDataWithCountByOidQuery());
      dbQueryBuilder.AppendLine("Declare @companyOid int;");
      dbQueryBuilder.AppendLine("SELECT @companyOid=eom.oid FROM [ExternalOriginatorManagement] eom inner join [ExternalOrgDescendents] eod on eom.oid = eod.oid where eom.parent = 0 and eod.descendent = @oid");
      dbQueryBuilder.AppendLine("set @companyOid = isnull(@companyOid, @oid)");
      dbQueryBuilder.Declare("@ParentId", "INT");
      dbQueryBuilder.AppendLine("select @ParentId = parent from ExternalOriginatorManagement where oid = @oid");
      dbQueryBuilder.Declare("@InheritCustomfields", "INT");
      dbQueryBuilder.AppendLine("select @InheritCustomfields = InheritCustomfields from ExternalOrgDetail where externalOrgID = @oid");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getDBADetailsQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalOrgSalesRepsForCurrentOrgQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalorgSalesRepsForCompanyQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getSalesRepsAssignedToUserQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getWarehousesQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getCommitmentsQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getLoanCriteriaQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getLoanTypesChannelsQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getCustomFieldsDetailsQuery());
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getTpoConnectSetupQuery());
      return dbQueryBuilder.ToString();
    }

    private static ExternalOrgWrapper GatherExternalOrgData(DataSet ds, int orgId)
    {
      int count = ds.Tables.Count;
      if (count <= 0 || ds.Tables[0].Rows.Count <= 0)
        return (ExternalOrgWrapper) null;
      ExternalOrgWrapper externalOrgWrapper1 = new ExternalOrgWrapper();
      DataRow[] array = Enumerable.Cast<DataRow>(ds.Tables[0].Rows).ToArray<DataRow>();
      externalOrgWrapper1.ExternalManagementDataCount = ExternalOrgManagementAccessor.getExternalOrgManagementCountFromDatarow(array[0]);
      externalOrgWrapper1.ExternalManagementDataCount.DataDetails = ExternalOrgManagementAccessor.addExternalOrgDetail(externalOrgWrapper1.ExternalManagementDataCount.DataDetails, array[0]);
      List<ExternalOrgDBAName> externalOrgDbaNameList1 = new List<ExternalOrgDBAName>();
      if (count > 1 && ds.Tables[1].Rows.Count > 0)
      {
        List<ExternalOrgDBAName> externalOrgDbaNameList2 = ExternalOrgManagementAccessor.populateExternalOrgDBA(ds.Tables[1].Rows);
        externalOrgWrapper1.DBADetails = externalOrgDbaNameList2;
      }
      List<ExternalOrgSalesRepPlatform> source1 = new List<ExternalOrgSalesRepPlatform>();
      if (count > 2 && ds.Tables[2].Rows.Count > 0)
      {
        List<ExternalOrgSalesRep> source2 = ExternalOrgManagementAccessor.populateExternalOrgSalesRepsForCurrentOrg(ds.Tables[2].Rows);
        List<ExternalOrgSalesRep> list = source2.Where<ExternalOrgSalesRep>((System.Func<ExternalOrgSalesRep, bool>) (t => t.externalOrgId == orgId)).ToList<ExternalOrgSalesRep>();
        list.AddRange(source2.SkipWhile<ExternalOrgSalesRep>((System.Func<ExternalOrgSalesRep, bool>) (t => t.externalOrgId == orgId)));
        foreach (ExternalOrgSalesRep externalOrgSalesRep in list)
        {
          ExternalOrgSalesRep salesRep = externalOrgSalesRep;
          ExternalOrgSalesRepPlatform salesRepPlatform = source1.FirstOrDefault<ExternalOrgSalesRepPlatform>((System.Func<ExternalOrgSalesRepPlatform, bool>) (w => string.Equals(w.userId, salesRep.userId, StringComparison.InvariantCultureIgnoreCase)));
          if (salesRepPlatform == null)
          {
            salesRepPlatform = new ExternalOrgSalesRepPlatform(salesRep);
            salesRepPlatform.isPrimarySalesRep = string.Equals(salesRep.userId, externalOrgWrapper1.ExternalManagementDataCount.DataDetails.PrimarySalesRepUserId, StringComparison.InvariantCultureIgnoreCase);
            source1.Add(salesRepPlatform);
          }
          salesRepPlatform.Organizations.Add(new EntityRef()
          {
            EntityId = salesRep.externalOrgId.ToString(),
            EntityName = salesRep.organizationName
          });
          salesRepPlatform.RecordType.Add("CurrentOrg");
          salesRepPlatform.RecordType.Add("Company");
        }
      }
      if (count > 3 && ds.Tables[3].Rows.Count > 0)
      {
        foreach (ExternalOrgSalesRep externalOrgSalesRep in ExternalOrgManagementAccessor.populateExternalorgSalesRepsForCompany(ds.Tables[3].Rows))
        {
          ExternalOrgSalesRep salesRep = externalOrgSalesRep;
          ExternalOrgSalesRepPlatform salesRepPlatform = source1.FirstOrDefault<ExternalOrgSalesRepPlatform>((System.Func<ExternalOrgSalesRepPlatform, bool>) (w => string.Equals(w.userId, salesRep.userId, StringComparison.InvariantCultureIgnoreCase)));
          if (salesRepPlatform == null)
          {
            salesRepPlatform = new ExternalOrgSalesRepPlatform(salesRep);
            salesRepPlatform.isPrimarySalesRep = false;
            source1.Add(salesRepPlatform);
          }
          salesRepPlatform.Organizations.Add(new EntityRef()
          {
            EntityId = salesRep.externalOrgId.ToString(),
            EntityName = salesRep.organizationName
          });
          salesRepPlatform.RecordType.Add("Company");
        }
      }
      if (count > 4 && ds.Tables[4].Rows.Count > 0)
      {
        foreach (ExternalOrgSalesRep externalOrgSalesRep in ExternalOrgManagementAccessor.populateSalesRepsAssignedToUser(ds.Tables[4].Rows))
        {
          ExternalOrgSalesRep salesRep = externalOrgSalesRep;
          ExternalOrgSalesRepPlatform salesRepPlatform = source1.FirstOrDefault<ExternalOrgSalesRepPlatform>((System.Func<ExternalOrgSalesRepPlatform, bool>) (w => string.Equals(w.userId, salesRep.userId, StringComparison.InvariantCultureIgnoreCase)));
          if (salesRepPlatform == null)
          {
            salesRepPlatform = new ExternalOrgSalesRepPlatform(salesRep);
            source1.Add(salesRepPlatform);
          }
          if (salesRepPlatform.Organizations.Count <= 0)
          {
            salesRepPlatform.IsAssignedToUser = true;
            salesRepPlatform.RecordType.Add("Company");
          }
        }
      }
      externalOrgWrapper1.SalesReps = source1;
      List<ExternalOrgWarehouse> externalOrgWarehouseList = new List<ExternalOrgWarehouse>();
      if (count > 5)
      {
        DataRowCollection rows = ds.Tables[5].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) ds.Tables[5].Rows)
            externalOrgWarehouseList.Add(ExternalOrgManagementAccessor.getExternalWarehouseFromDatarow(row));
        }
      }
      externalOrgWrapper1.WareHousesDetails = externalOrgWarehouseList;
      ExternalOrgWrapper externalOrgWrapper2 = externalOrgWrapper1;
      ExternalOriginatorManagementData originatorManagementData;
      if (count > 6)
      {
        DataRowCollection rows = ds.Tables[6].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
        {
          originatorManagementData = ExternalOrgManagementAccessor.getExternalCommitmentsFromDatarow(ds.Tables[6].Rows[0]);
          goto label_42;
        }
      }
      originatorManagementData = new ExternalOriginatorManagementData();
label_42:
      externalOrgWrapper2.CommitmentsDetails = originatorManagementData;
      ExternalOrgLoanTypes externalOrgLoanTypes = new ExternalOrgLoanTypes();
      if (count > 7)
      {
        DataRowCollection rows = ds.Tables[7].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
          externalOrgLoanTypes = ExternalOrgManagementAccessor.PopulateExternalOrgLoanCriteria(ds.Tables[7].Rows[0]);
      }
      if (count > 8)
      {
        DataRowCollection rows = ds.Tables[8].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
        {
          List<ExternalOrgLoanTypes.ExternalOrgChannelLoanType> source3 = ExternalOrgManagementAccessor.PopulateLoanTypeChannels(ds.Tables[8].Rows);
          externalOrgLoanTypes.Broker = source3.Where<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>((System.Func<ExternalOrgLoanTypes.ExternalOrgChannelLoanType, bool>) (a => a.ChannelType == 0)).FirstOrDefault<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>();
          externalOrgLoanTypes.CorrespondentDelegated = source3.Where<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>((System.Func<ExternalOrgLoanTypes.ExternalOrgChannelLoanType, bool>) (a => a.ChannelType == 1)).FirstOrDefault<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>();
          externalOrgLoanTypes.CorrespondentNonDelegated = source3.Where<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>((System.Func<ExternalOrgLoanTypes.ExternalOrgChannelLoanType, bool>) (a => a.ChannelType == 2)).FirstOrDefault<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>();
        }
      }
      externalOrgWrapper1.LoanCriteria = externalOrgLoanTypes;
      ExternalOrgCustomFields externalOrgCustomFields = new ExternalOrgCustomFields();
      if (count > 9)
      {
        DataRowCollection rows = ds.Tables[9].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
          externalOrgCustomFields.Fields = ExternalOrgManagementAccessor.GetTpoCustomFields(ds.Tables[9].Rows).Fields;
      }
      externalOrgWrapper1.CustomFieldsDetails = externalOrgCustomFields;
      List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
      if (count > 10)
      {
        DataRowCollection rows = ds.Tables[10].Rows;
        if ((rows != null ? (rows.Count > 0 ? 1 : 0) : 0) != 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) ds.Tables[10].Rows)
            externalOrgUrlList.Add(ExternalOrgManagementAccessor.getExternalTpoConnectFromDatarow(row));
        }
      }
      externalOrgWrapper1.TpoConnectSetup = externalOrgUrlList;
      return externalOrgWrapper1;
    }

    private static List<ExternalOrgDBAName> populateExternalOrgDBA(DataRowCollection rows)
    {
      List<ExternalOrgDBAName> externalOrgDbaNameList = new List<ExternalOrgDBAName>();
      if (rows != null && rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) rows)
          externalOrgDbaNameList.Add(new ExternalOrgDBAName(Convert.ToInt32(row["ExternalOrgID"]), Convert.ToString(row["Name"]), Convert.ToBoolean(row["SetAsDefault"]))
          {
            DBAID = Convert.ToInt32(row["DBAID"]),
            SortIndex = Convert.ToInt32(row["SortIndex"])
          });
      }
      return externalOrgDbaNameList;
    }

    private static ExternalOrgLoanTypes PopulateExternalOrgLoanCriteria(DataRow dataRow)
    {
      return new ExternalOrgLoanTypes()
      {
        ExternalOrgID = Convert.ToInt32(dataRow["externalOrgID"]),
        UseParentInfoFhaVa = Convert.ToBoolean(dataRow["InheritParentFHAVA"]),
        FHAId = Convert.ToString(dataRow["FHAID"]),
        FHASonsorId = Convert.ToString(dataRow["FHASponsorID"]),
        FHAStatus = Convert.ToString(dataRow["FHAStatus"]),
        FHACompareRatio = Convert.ToDecimal(dataRow["FHACompareRatio"]),
        FHAApprovedDate = dataRow["FHAApprovedDate"] != DBNull.Value ? Convert.ToDateTime(dataRow["FHAApprovedDate"]) : DateTime.MinValue,
        FHAExpirationDate = dataRow["FHAExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dataRow["FHAExpirationDate"]) : DateTime.MinValue,
        VAId = Convert.ToString(dataRow["VAID"]),
        VAStatus = Convert.ToString(dataRow["VAStatus"]),
        VAApprovedDate = dataRow["VAApprovedDate"] != DBNull.Value ? Convert.ToDateTime(dataRow["VAApprovedDate"]) : DateTime.MinValue,
        VAExpirationDate = dataRow["VAExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dataRow["VAExpirationDate"]) : DateTime.MinValue,
        Underwriting = dataRow["Underwriting"] == DBNull.Value || dataRow["Underwriting"].ToString() == "-1" ? 1 : Convert.ToInt32(dataRow["Underwriting"]),
        AdvancedCode = Convert.ToString(dataRow["AdvancedCode"]),
        AdvancedCodeXml = Convert.ToString(dataRow["AdvancedCodeXml"]),
        FHADirectEndorsement = Convert.ToString(dataRow["FHADirectEndorsement"]),
        VASponsorID = Convert.ToString(dataRow["VASponsorID"]),
        FHMLCApproved = Convert.ToBoolean(dataRow["FHMLCApproved"] != DBNull.Value ? dataRow["FHMLCApproved"] : (object) 0),
        FNMAApproved = Convert.ToBoolean(dataRow["FNMAApproved"] != DBNull.Value ? dataRow["FNMAApproved"] : (object) 0),
        FannieMaeID = Convert.ToString(dataRow["FannieMaeID"]),
        FreddieMacID = Convert.ToString(dataRow["FreddieMacID"]),
        AUSMethod = Convert.ToString(dataRow["AUSMethod"])
      };
    }

    private static List<ExternalOrgLoanTypes.ExternalOrgChannelLoanType> PopulateLoanTypeChannels(
      DataRowCollection dataRows)
    {
      List<ExternalOrgLoanTypes.ExternalOrgChannelLoanType> orgChannelLoanTypeList = new List<ExternalOrgLoanTypes.ExternalOrgChannelLoanType>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRows)
        orgChannelLoanTypeList.Add(new ExternalOrgLoanTypes.ExternalOrgChannelLoanType()
        {
          LoanTypes = Convert.ToInt32(dataRow["LoanTypes"]),
          LoanPurpose = Convert.ToInt32(dataRow["LoanPurposes"]),
          AllowLoansWithIssues = Convert.ToInt32(dataRow["AllowLoansWithIssues"]),
          MsgUploadNonApprovedLoans = Convert.ToString(dataRow["MsgUploadNonApprovedLoans"]),
          ChannelType = Convert.ToInt32(dataRow["ChannelType"])
        });
      return orgChannelLoanTypeList;
    }

    private static string getExternalOrganizationDataWithCountByOidQuery()
    {
      return "\r\n            ;WITH cte AS \r\n            (\r\n                SELECT eom.* FROM [ExternalOriginatorManagement] eom WHERE eom.oid = @oid\r\n            ), \r\n            cco AS \r\n            (\r\n                SELECT eu.externalOrgId, count(*) AS numContacts FROM ExternalUsers eu INNER JOIN cte ON cte.oid = eu.externalOrgID GROUP BY eu.externalOrgID\r\n            ), \r\n            cch AS \r\n            ( \r\n                SELECT cte.oid, count(1) AS numChildren FROM cte INNER JOIN ExternalOriginatorManagement eom2 ON cte.oid = eom2.Parent GROUP BY cte.oid\r\n            ) \r\n            SELECT cte.*, org.*, \r\n            CASE WHEN cco.numContacts is NULL THEN 0 ELSE cco.numContacts END AS countContacts, CASE WHEN cch.numChildren is null THEN 0 ELSE cch.numChildren END AS countChildren \r\n            , CASE WHEN e.OrganizationName is NULL THEN '' ELSE e.OrganizationName END AS parentOrgName FROM cte \r\n            INNER JOIN [ExternalOrgDetail] org ON oid = org.[externalOrgID] \r\n            LEFT OUTER JOIN cco ON cco.externalOrgId = org.[externalOrgID] LEFT OUTER JOIN cch ON cch.oid = org.[externalOrgID] \r\n            LEFT OUTER JOIN ExternalOriginatorManagement e ON cte.parent = e.oid \r\n            ORDER BY OrganizationName";
    }

    private static string getExternalOrganisationByParentQuery()
    {
      return "select * from [ExternalOriginatorManagement] inner join [ExternalOrgDetail] on oid = [externalOrgID] where parent =@oid";
    }

    public static bool UpdateExternalOrgManualContact(ExternalOrgContact obj)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [ExternalOrgContact] Set[name] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) obj.Name) + ",[title] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) obj.Title) + ",[email] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) obj.Email) + ",[phone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) obj.Phone) + " WHERE externalOrgContactID = " + (object) obj.ExternalOrgContactID;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgManualContact: Cannot update record in ExternalOrgContact table.\r\n" + ex.Message);
      }
      return true;
    }

    public static void UpdateExternalOrgHeirarchyPath(int oid, string heirarchyPath)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE [ExternalOriginatorManagement] Set[HierarchyPath] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(heirarchyPath) + " WHERE oid = " + (object) oid;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgHeirarchyPath: Cannot update record in ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
    }

    public static bool DeleteExternalOrgContact(List<ExternalOrgContact> externalOrgContact)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str1 = string.Empty;
        string str2 = string.Empty;
        foreach (ExternalOrgContact externalOrgContact1 in externalOrgContact)
        {
          if (externalOrgContact1.Type == ExternalOriginatorContactType.FreeEntry)
            str1 = str1 + (str1 != string.Empty ? (object) "," : (object) "") + (object) externalOrgContact1.ExternalOrgContactID;
          else if (externalOrgContact1.Type == ExternalOriginatorContactType.TPO)
            str2 = str2 + (str2 != string.Empty ? "," : "") + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrgContact1.ExternalUserID);
        }
        if (!str1.Equals(""))
          dbQueryBuilder.AppendLine("DELETE from [ExternalOrgContact] WHERE externalOrgContactID in (" + str1 + ")");
        if (!str2.Equals(""))
          dbQueryBuilder.AppendLine("UPDATE [ExternalUsers] Set [isKeyContact] = 0 WHERE external_userid in (" + str2 + ")");
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrgContact: Cannot delete record in ExternalOrgContact or ExternalUsers due to this error: " + ex.Message);
      }
      return true;
    }

    public static bool AddTpoUserToExtOrgContact(string[] external_userid, int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str1 = string.Empty;
        foreach (string str2 in external_userid)
          str1 = str1 + (str1 != string.Empty ? "," : "") + EllieMae.EMLite.DataAccess.SQL.EncodeString(str2);
        string text = "UPDATE [ExternalUsers] Set[isKeyContact] = 1 WHERE external_userid in (" + str1 + ") and externalOrgID = " + (object) externalOrgID;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddTpoUserToExtOrgContact: Cannot update record in ExternalUsers table.\r\n" + ex.Message);
      }
      return true;
    }

    public static ContactCustomFieldInfoCollection GetCustomFieldInfo()
    {
      return ClientContext.GetCurrent()?.Cache?.Get<ContactCustomFieldInfoCollection>("TPOCustomfieldSettings", (Func<ContactCustomFieldInfoCollection>) (() =>
      {
        ContactCustomFieldInfoCollection customFieldInfo = new ContactCustomFieldInfoCollection();
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.Append("select * from [CustomPages] where type = " + (object) 1);
          DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
          if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
          {
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
            {
              if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PageID"]) == 1)
                customFieldInfo.Page1Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PageName"]);
              else if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PageID"]) == 2)
                customFieldInfo.Page2Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PageName"]);
              else if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PageID"]) == 3)
                customFieldInfo.Page3Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PageName"]);
              else if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PageID"]) == 4)
                customFieldInfo.Page4Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PageName"]);
              else if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PageID"]) == 5)
                customFieldInfo.Page5Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PageName"]);
            }
          }
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("select * from [CustomFields] where type = " + (object) 1);
          DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          List<int> intList = new List<int>();
          Dictionary<int, ContactCustomFieldInfo> dictionary = new Dictionary<int, ContactCustomFieldInfo>();
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
          {
            ContactCustomFieldInfo contactCustomFieldInfo = new ContactCustomFieldInfo();
            contactCustomFieldInfo.LabelID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["LabelID"]);
            contactCustomFieldInfo.Label = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Label"]);
            FieldFormat fieldFormat = (FieldFormat) Enum.Parse(typeof (FieldFormat), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["FieldType"]), true);
            contactCustomFieldInfo.FieldType = fieldFormat;
            contactCustomFieldInfo.LoanFieldId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["LoanFieldId"]);
            int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["CustomFieldID"]);
            if (fieldFormat == FieldFormat.DROPDOWN || fieldFormat == FieldFormat.DROPDOWNLIST)
              intList.Add(key);
            dictionary.Add(key, contactCustomFieldInfo);
          }
          if (intList.Count > 0)
          {
            string str1 = string.Join<int>(",", (IEnumerable<int>) intList.ToArray());
            dbQueryBuilder.Reset();
            dbQueryBuilder.Append("select * from [CustomFieldOptions] where CustomFieldID in (" + str1 + ") order by CustomFieldID");
            DataRowCollection dataRowCollection3 = dbQueryBuilder.Execute();
            int key = 0;
            List<string> stringList = new List<string>();
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection3)
            {
              string str2 = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["OptionName"]);
              int num = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["CustomFieldID"]);
              if (key == 0 || key == num)
              {
                stringList.Add(str2);
              }
              else
              {
                if (dictionary.ContainsKey(key))
                {
                  dictionary[key].FieldOptions = stringList.ToArray();
                  stringList.Clear();
                }
                stringList.Add(str2);
              }
              key = num;
            }
            if (dictionary.ContainsKey(key))
            {
              dictionary[key].FieldOptions = stringList.ToArray();
              stringList.Clear();
            }
          }
          customFieldInfo.Items = dictionary.Values.ToArray<ContactCustomFieldInfo>();
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
        }
        return customFieldInfo;
      }));
    }

    public static void UpdateCustomFieldInfo(
      ContactCustomFieldInfoCollection customFields,
      ArrayList invalidFieldIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(customFields.GetType());
        StringWriter stringWriter = new StringWriter();
        xmlSerializer.Serialize((TextWriter) stringWriter, (object) customFields);
        string str1 = stringWriter.ToString().Replace("'", "''");
        string str2 = string.Empty;
        for (int index = 0; index < invalidFieldIds.Count; ++index)
          str2 = str2 + (str2 != string.Empty ? (object) "," : (object) "") + invalidFieldIds[index];
        dbQueryBuilder.AppendLine("exec UpdateCustomFields  '" + str1 + "','" + str2 + "'," + (object) 1);
        dbQueryBuilder.Execute();
        FieldSearchRule fieldSearchRule = new FieldSearchRule(ExternalOrgManagementAccessor.GetCustomFieldInfo(), FieldSearchRuleType.TPOCustomFields);
        if (fieldSearchRule.FieldSearchFields.Count > 0)
          FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
        else
          FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.TPOCustomFields);
        ClientContext.GetCurrent()?.Cache?.Remove("TPOCustomfieldSettings");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
    }

    private static string InsertCustomFieldValuesQuery(ContactCustomField[] fields)
    {
      string str1 = "";
      foreach (ContactCustomField field in fields)
      {
        string str2 = "INSERT INTO [TpoCustomField] ([OrgID],[FieldID],[OwnerID],[FieldValue]) VALUES (@oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) field.FieldID) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) field.OwnerID) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) field.FieldValue) + ")";
        str1 += str2;
      }
      return str1;
    }

    public static void UpdateCustomFieldValues(int orgID, ContactCustomField[] fields)
    {
      try
      {
        if (fields.Length == 0)
          return;
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TpoCustomField");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        for (int index = 0; index < fields.Length; ++index)
        {
          DbValueList dbValueList = new DbValueList();
          dbValueList.Add("OrgID", (object) orgID);
          dbValueList.Add("FieldID", (object) fields[index].FieldID);
          dbQueryBuilder.DeleteFrom(table, dbValueList);
          dbValueList.Add("FieldValue", (object) fields[index].FieldValue);
          dbValueList.Add("OwnerID", (object) fields[index].OwnerID);
          dbQueryBuilder.InsertInto(table, dbValueList, true, false);
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
    }

    public static ContactCustomField[] GetCustomFieldValues(int orgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("select * from [TpoCustomField] where OrgId = " + (object) orgID);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ContactCustomField[] customFieldValues = new ContactCustomField[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        customFieldValues[index] = ExternalOrgManagementAccessor.dataRowToCustomField(dataRowCollection[index]);
      return customFieldValues;
    }

    public static ExternalOrgCustomFields GetTpoCustomFields(int orgID)
    {
      ExternalOrgCustomFields tpoCustomFields = new ExternalOrgCustomFields();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("Select FieldID, OwnerID, FieldValue, Label, FieldType, LoanFieldId from [TpoCustomField] tcf ");
      dbQueryBuilder.Append("Inner Join [CustomFields] cf on tcf.FieldID = cf.LabelID where OrgId = " + (object) orgID);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        List<TpoCustomFields> tpoCustomFieldsList = new List<TpoCustomFields>();
        for (int index = 0; index < dataRowCollection.Count; ++index)
          tpoCustomFieldsList.Add(ExternalOrgManagementAccessor.dataRowMapToCustomField(dataRowCollection[index]));
        tpoCustomFields.Fields = tpoCustomFieldsList;
      }
      return tpoCustomFields;
    }

    public static ExternalOrgCustomFields GetTpoCustomFields(DataRowCollection dataRows)
    {
      ExternalOrgCustomFields tpoCustomFields = new ExternalOrgCustomFields();
      if (dataRows != null && dataRows.Count > 0)
      {
        List<TpoCustomFields> tpoCustomFieldsList = new List<TpoCustomFields>();
        for (int index = 0; index < dataRows.Count; ++index)
          tpoCustomFieldsList.Add(ExternalOrgManagementAccessor.dataRowMapToCustomField(dataRows[index]));
        tpoCustomFields.Fields = tpoCustomFieldsList;
      }
      return tpoCustomFields;
    }

    public static ExternalOrgURL getExternalTpoConnectFromDatarow(DataRow rows)
    {
      try
      {
        return new ExternalOrgURL()
        {
          URLID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(rows["urlID"]),
          ExternalOrgID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(rows["externalOrgID"]),
          URL = EllieMae.EMLite.DataAccess.SQL.DecodeString(rows["URL"]),
          DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(rows["AddedDateTime"]),
          isDeleted = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(rows["isDeleted"]),
          EntityType = EllieMae.EMLite.DataAccess.SQL.DecodeInt(rows["UrlEntityType"]),
          siteId = EllieMae.EMLite.DataAccess.SQL.DecodeString(rows["SiteId"]),
          TPOAdminLinkAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(rows["TPOAdminLinkAccess"]),
          Id = EllieMae.EMLite.DataAccess.SQL.DecodeInt(rows["ExternalOrgSelectedURLID"])
        };
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("getExternalTpoConnectFromDatarow: Cannot fetch all records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    private static TpoCustomFields dataRowMapToCustomField(DataRow row)
    {
      return new TpoCustomFields()
      {
        CustomFieldId = EllieMae.EMLite.DataAccess.SQL.Decode(row["FieldID"], (object) string.Empty).ToString(),
        OwnerID = EllieMae.EMLite.DataAccess.SQL.Decode(row["OwnerID"], (object) string.Empty).ToString(),
        FieldValue = EllieMae.EMLite.DataAccess.SQL.Decode(row["FieldValue"], (object) string.Empty).ToString(),
        FieldName = EllieMae.EMLite.DataAccess.SQL.Decode(row["Label"], (object) string.Empty).ToString(),
        FieldType = EllieMae.EMLite.DataAccess.SQL.Decode(row["FieldType"], (object) string.Empty).ToString(),
        LoanFieldId = EllieMae.EMLite.DataAccess.SQL.Decode(row["LoanFieldId"], (object) string.Empty).ToString()
      };
    }

    private static ContactCustomField dataRowToCustomField(DataRow row)
    {
      return new ContactCustomField((int) row["OrgId"], (int) row["FieldID"], EllieMae.EMLite.DataAccess.SQL.Decode(row["OwnerID"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(row["FieldValue"], (object) "").ToString());
    }

    public static Dictionary<string, string> IsTpoOnWatchList(
      string companyId,
      string branchId,
      string lnProcessorId,
      string lnOfficerId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      try
      {
        companyId = companyId != string.Empty ? companyId : "-1";
        branchId = branchId != string.Empty ? branchId : "-1";
        lnProcessorId = lnProcessorId != string.Empty ? lnProcessorId : "-1";
        lnOfficerId = lnOfficerId != string.Empty ? lnOfficerId : "-1";
        string empty = string.Empty;
        dbQueryBuilder.AppendLine("select count(*) from [ExternalOrgDetail] WHERE [externalOrgID] in ");
        dbQueryBuilder.AppendLine("(select [oid] from [ExternalOriginatorManagement] where ExternalID in ('" + companyId + "', '" + branchId + "')) and AddToApprovalWatchlist = 1");
        int int32_1 = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("select count(*) from [ExternalUsers] WHERE [ContactID] in ('" + lnProcessorId + "', '" + lnOfficerId + "') and Approval_status_watchlist = 1");
        int int32_2 = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
        if (int32_1 > 0 && int32_2 > 0)
          empty = WatchListReasonType.WatchListReason.Both.ToString();
        else if (int32_1 > 0)
          empty = WatchListReasonType.WatchListReason.Company.ToString();
        else if (int32_2 > 0)
          empty = WatchListReasonType.WatchListReason.User.ToString();
        if (empty != string.Empty)
        {
          dictionary.Add("Flag", "Y");
          dictionary.Add("Reason", empty);
        }
        else
        {
          dictionary.Add("Flag", "N");
          dictionary.Add("Reason", empty);
        }
        return dictionary;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        return dictionary;
      }
    }

    public static List<string> GetAEAccessibleExternalUsers(string[] userids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> accessibleExternalUsers = new List<string>();
      string userList = "";
      ((IEnumerable<string>) userids).ToList<string>().ForEach((Action<string>) (x =>
      {
        if (userList == "")
          userList = EllieMae.EMLite.DataAccess.SQL.EncodeString(x);
        else
          userList = userList + "," + EllieMae.EMLite.DataAccess.SQL.EncodeString(x);
      }));
      dbQueryBuilder.AppendLine("Select distinct TPOContactID from [AEAccessibleExternalUsers] where [AEID] in (" + userList + ")");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOContactID"]);
          accessibleExternalUsers.Add(str);
        }
      }
      dbQueryBuilder.Reset();
      return accessibleExternalUsers;
    }

    public static ArrayList GetExternalAndInternalUserAndOrgBySalesRep(string userid, int orgId)
    {
      return ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRepWithSubQuery(ExternalOrgManagementAccessor.GetUserIdFromExtOrgQuery(userid, orgId));
    }

    public static ArrayList GetExternalUserAndOrgBySalesRepWithSubQuery(string useridQuery)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
      Dictionary<string, ExternalOriginatorManagementData> dictionary1 = new Dictionary<string, ExternalOriginatorManagementData>();
      ArrayList salesRepWithSubQuery = new ArrayList();
      Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
      try
      {
        dbQueryBuilder.AppendLine("Select ContactID, externalorgid from ExternalUsers where Sales_rep_userid in (" + useridQuery + ")");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContactID"]);
              stringList2.Add(str);
              int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["externalorgid"]);
              if (!dictionary2.ContainsKey(key))
                dictionary2.Add(key, "");
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Select distinct externalorgid from ExternalOrgSalesReps where UserID in (" + useridQuery + ")");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["externalorgid"]);
              intList3.Add(key);
              if (!dictionary2.ContainsKey(key))
                dictionary2.Add(key, "");
            }
          }
        }
      }
      catch
      {
      }
      List<int> list = dictionary2.Keys.ToList<int>();
      try
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Select distinct TPOContactID from [AEAccessibleExternalUsers] where [AEID] in (" + useridQuery + ")");
        DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable1 != null && dataTable1.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
          {
            string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOContactID"]);
            stringList1.Add(str);
          }
        }
        dbQueryBuilder.Reset();
        if (list.Count > 0)
        {
          dbQueryBuilder.Append(ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRepQuery(useridQuery));
          DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
          if (dataTable2 != null && dataTable2.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
            {
              if (!dictionary1.ContainsKey(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ExternalID"])))
              {
                ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
                originatorManagementData.ExternalID = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ExternalID"]);
                originatorManagementData.CompanyDBAName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CompanyDBAName"]);
                originatorManagementData.CompanyLegalName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CompanyLegalName"]);
                originatorManagementData.OrganizationName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["OrganizationName"]);
                originatorManagementData.oid = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["orgID"]);
                dictionary1.Add(originatorManagementData.ExternalID, originatorManagementData);
                intList2.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Ancestor"]));
              }
            }
          }
          originatorManagementDataList = dictionary1.Values.ToList<ExternalOriginatorManagementData>();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
      }
      salesRepWithSubQuery.Add((object) stringList2);
      salesRepWithSubQuery.Add((object) originatorManagementDataList);
      salesRepWithSubQuery.Add((object) intList3);
      salesRepWithSubQuery.Add((object) intList2);
      salesRepWithSubQuery.Add((object) stringList1);
      return salesRepWithSubQuery;
    }

    public static ArrayList GetExternalUserAndOrgBySalesRep(string userids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
      Dictionary<string, ExternalOriginatorManagementData> dictionary1 = new Dictionary<string, ExternalOriginatorManagementData>();
      ArrayList andOrgBySalesRep = new ArrayList();
      Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
      if (!userids.Contains(","))
        userids = EllieMae.EMLite.DataAccess.SQL.EncodeString(userids);
      try
      {
        dbQueryBuilder.AppendLine("Select ContactID, externalorgid from ExternalUsers where Sales_rep_userid in (" + userids + ")");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ContactID"]);
              stringList2.Add(str);
              int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["externalorgid"]);
              if (!dictionary2.ContainsKey(key))
                dictionary2.Add(key, "");
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Select distinct externalorgid from ExternalOrgSalesReps where UserID in (" + userids + ")");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            {
              int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["externalorgid"]);
              intList3.Add(key);
              if (!dictionary2.ContainsKey(key))
                dictionary2.Add(key, "");
            }
          }
        }
      }
      catch
      {
      }
      List<int> list = dictionary2.Keys.ToList<int>();
      try
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Select distinct TPOContactID from [AEAccessibleExternalUsers] where [AEID] in (" + userids + ")");
        DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable1 != null && dataTable1.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
          {
            string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOContactID"]);
            stringList1.Add(str);
          }
        }
        dbQueryBuilder.Reset();
        if (list.Count > 0)
        {
          dbQueryBuilder.AppendLine("SELECT Ancestor, ExternalID, CompanyLegalName, CompanyDBAName, OrganizationName, orgID FROM ExternalOrgAncestors where depth = 1 and ");
          dbQueryBuilder.Append("orgid in (");
          dbQueryBuilder.AppendLine("Select externalorgid from ExternalUsers where Sales_rep_userid in (" + userids + ")");
          dbQueryBuilder.AppendLine(" Union ");
          dbQueryBuilder.AppendLine("Select externalorgid from ExternalOrgSalesReps where UserID in (" + userids + ")");
          dbQueryBuilder.Append(" ) ");
          dbQueryBuilder.AppendLine("group by ExternalID,CompanyLegalName, CompanyDBAName, orgID, Ancestor, OrganizationName order by OrganizationName");
          DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
          if (dataTable2 != null && dataTable2.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
            {
              if (!dictionary1.ContainsKey(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ExternalID"])))
              {
                ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
                originatorManagementData.ExternalID = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["ExternalID"]);
                originatorManagementData.CompanyDBAName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CompanyDBAName"]);
                originatorManagementData.CompanyLegalName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CompanyLegalName"]);
                originatorManagementData.OrganizationName = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["OrganizationName"]);
                originatorManagementData.oid = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["orgID"]);
                dictionary1.Add(originatorManagementData.ExternalID, originatorManagementData);
                intList2.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Ancestor"]));
              }
            }
          }
          originatorManagementDataList = dictionary1.Values.ToList<ExternalOriginatorManagementData>();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
      }
      andOrgBySalesRep.Add((object) stringList2);
      andOrgBySalesRep.Add((object) originatorManagementDataList);
      andOrgBySalesRep.Add((object) intList3);
      andOrgBySalesRep.Add((object) intList2);
      andOrgBySalesRep.Add((object) stringList1);
      return andOrgBySalesRep;
    }

    private static string GetExternalUserAndOrgBySalesRepQuery(
      string useridQuery,
      string externalId = null)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Ancestor, ExternalID, CompanyLegalName, CompanyDBAName, OrganizationName, orgID FROM ExternalOrgAncestors where depth = 1 and ");
      dbQueryBuilder.Append("orgid in (");
      dbQueryBuilder.AppendLine("Select externalorgid from ExternalUsers where Sales_rep_userid in (" + useridQuery + ")");
      dbQueryBuilder.AppendLine(" Union ");
      dbQueryBuilder.AppendLine("Select externalorgid from ExternalOrgSalesReps where UserID in (" + useridQuery + ")");
      dbQueryBuilder.Append(" ) ");
      if (!string.IsNullOrWhiteSpace(externalId))
        dbQueryBuilder.AppendLine(" and ExternalID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalId));
      dbQueryBuilder.AppendLine("group by ExternalID,CompanyLegalName, CompanyDBAName, orgID, Ancestor, OrganizationName order by OrganizationName");
      return dbQueryBuilder.ToString();
    }

    private static string GetUserIdFromExtOrgQuery(string userid, int orgId)
    {
      return "select userid from users where org_id in (Select descendent from org_descendents where oid = " + (object) orgId + ") or userid =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid);
    }

    private static string getCaseSensitivePassword(string password)
    {
      int length = password.Length;
      password += "\\~@\\";
      for (int index = 0; index < length; ++index)
        password += char.IsLower(password[index]) ? "0" : "1";
      return password;
    }

    public static bool ReassignSalesRep(
      List<string> extUserId,
      List<int> extOrgId,
      string salesRepId,
      string currSalesRepId)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (extOrgId != null && extOrgId.Any<int>())
        {
          foreach (int parentOid in extOrgId)
          {
            dbQueryBuilder.AppendLine("Select *  from [ExternalOrgSalesReps] WHERE [UserID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(salesRepId) + " and [externalOrgID] = " + (object) parentOid);
            DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
            if (dataRowCollection1.Count > 0)
            {
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append("delete from [ExternalOrgSalesReps] WHERE [UserID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(currSalesRepId) + " and [externalOrgID] = " + (object) parentOid);
              dbQueryBuilder.ExecuteNonQuery();
            }
            else
            {
              dbQueryBuilder.Reset();
              string.Join<int>(",", (IEnumerable<int>) extOrgId.ToArray());
              dbQueryBuilder.Append("update  [ExternalOrgSalesReps] set [UserID] ='" + salesRepId + "' where [externalOrgID] = ( " + (object) parentOid + ") and [UserID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(currSalesRepId));
              dbQueryBuilder.ExecuteNonQuery();
            }
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("Select [PrimarySalesRepUserId]  from [ExternalOrgDetail] WHERE  [externalOrgID] = " + (object) parentOid);
            dataRowCollection1.Clear();
            DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
            {
              if (EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PrimarySalesRepUserId"]) == currSalesRepId)
              {
                dbQueryBuilder.Reset();
                dbQueryBuilder.Append("update  [ExternalOrgDetail] set [PrimarySalesRepUserId] =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(salesRepId) + " where [externalOrgID] =" + (object) parentOid);
                dbQueryBuilder.ExecuteNonQuery();
                ExternalOrgManagementAccessor.HandleChildPrimarySalesRep(parentOid, EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["PrimarySalesRepUserId"]), salesRepId);
              }
            }
            dataRowCollection2.Clear();
            dbQueryBuilder.Reset();
          }
        }
        dbQueryBuilder.Reset();
        if (extUserId != null && extUserId.Any<string>())
        {
          string str = string.Join("','", extUserId.ToArray());
          dbQueryBuilder.Append("update  [ExternalUsers] set [Sales_rep_userid] ='" + salesRepId + "' where [external_userid] in ( '" + str + "')");
          dbQueryBuilder.ExecuteNonQuery();
          ExternalOrgManagementAccessor.removeExternalUserCache(ExternalOrgManagementAccessor.getExternalUserContactIDs(extUserId));
        }
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
        return false;
      }
    }

    public static void HandleChildPrimarySalesRep(
      int parentOid,
      string primarySalesRepParent,
      string newSalesRepId)
    {
      List<ExternalOriginatorManagementData> organisationByParent = ExternalOrgManagementAccessor.GetExternalOrganisationByParent(parentOid);
      List<ExternalOriginatorManagementData> originatorManagementDataList1 = new List<ExternalOriginatorManagementData>();
      List<ExternalOriginatorManagementData> originatorManagementDataList2 = new List<ExternalOriginatorManagementData>();
      if (organisationByParent == null && !organisationByParent.Any<ExternalOriginatorManagementData>())
        return;
      List<ExternalOriginatorManagementData> list1 = organisationByParent.Where<ExternalOriginatorManagementData>((System.Func<ExternalOriginatorManagementData, bool>) (a => a.PrimarySalesRepUserId == primarySalesRepParent)).ToList<ExternalOriginatorManagementData>();
      List<ExternalOriginatorManagementData> list2 = organisationByParent.Where<ExternalOriginatorManagementData>((System.Func<ExternalOriginatorManagementData, bool>) (a => a.PrimarySalesRepUserId != primarySalesRepParent)).ToList<ExternalOriginatorManagementData>();
      ExternalOrgManagementAccessor.InsertPrimarySalesRepforChild(list1);
      foreach (ExternalOriginatorManagementData originatorManagementData in list2)
        ExternalOrgManagementAccessor.HandleChildPrimarySalesRep(originatorManagementData.oid, primarySalesRepParent, newSalesRepId);
    }

    public static void InsertPrimarySalesRepforChild(List<ExternalOriginatorManagementData> orgList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (ExternalOriginatorManagementData org in orgList)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append("select * from [ExternalOrgSalesReps] where [externalOrgID] = " + (object) org.oid + " and userId =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) org.PrimarySalesRepUserId));
        if (dbQueryBuilder.Execute().Count == 0)
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("insert into [ExternalOrgSalesReps] ([externalOrgID],userId) values (" + (object) org.oid + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) org.PrimarySalesRepUserId) + ")");
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganisationByParent(int oid)
    {
      List<ExternalOriginatorManagementData> organisationByParent = new List<ExternalOriginatorManagementData>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @oid int; set @oid = " + (object) oid);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getExternalOrganisationByParentQuery());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          organisationByParent.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
        }
      }
      return organisationByParent;
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganisationByParentRecursive(
      int oid,
      bool includeDisabled = false)
    {
      List<ExternalOriginatorManagementData> byParentRecursive = new List<ExternalOriginatorManagementData>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("DECLARE @ParentId int =" + (object) oid + "\r\n                                ;WITH cte AS\r\n                                 (\r\n                                  SELECT eom.*\r\n                                  FROM [ExternalOriginatorManagement] eom\r\n                                  WHERE oid = @ParentId\r\n                                  UNION ALL\r\n                                  SELECT eom.*\r\n                                  FROM [ExternalOriginatorManagement] eom JOIN cte c ON eom.Parent = c.oid\r\n                                  ) SELECT *, org.OrganizationType FROM cte  INNER JOIN [ExternalOrgDetail] org ON oid = org.[externalOrgID] ");
      if (!includeDisabled)
        dbQueryBuilder.Append(" where org.DisableLogin=0");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          byParentRecursive.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
        }
      }
      return byParentRecursive;
    }

    public static void UpdateExternalOrgIsTestAccount(int oid, bool isTestAccount)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) oid);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateIsTestAccountQuery(isTestAccount));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgIsTestAccount: Cannot update  [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalOrgManager(int oid, string manager)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [ExternalOrgDetail] set [ManagerUserID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) manager) + " where [externalOrgID] = " + (object) oid);
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgManager: Cannot update  [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalOrgLastLoanSubmittedDate(
      int oid,
      DateTime LastLoanSubmittedDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [ExternalOrgDetail] set [LastLoanSubmittedDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) LastLoanSubmittedDate) + " where [externalOrgID] = " + (object) oid);
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgLastLoanSubmittedDate: Cannot update  [ExternalOrgDetail] table.\r\n" + ex.Message);
      }
    }

    public static List<object> GetTPOInformationToolSettings(
      string companyExteralID,
      string companyName,
      string branchExteralID,
      string BranchName,
      string siteID,
      string currentUserID)
    {
      bool flag1 = ExternalOrgManagementAccessor.isCurrentUserTPOSalesRep(currentUserID);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOriginatorManagement] a INNER JOIN [ExternalOrgDetail] b on b.externalOrgID = a.oid WHERE a.externalid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(companyExteralID));
      dbQueryBuilder.Append(" AND b.[OrganizationType] = 0");
      dbQueryBuilder.Append(" ORDER BY a.[HierarchyPath], a.[Depth] ASC");
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
            originatorManagementData1 = ExternalOrgManagementAccessor.addExternalOrgDetail(ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(dataRowCollection[0]), dataRowCollection[0]);
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrganizationByTPOID: Cannot fetch record from ExternalOriginatorManagement table.\r\n" + ex.Message);
      }
      List<object> informationToolSettings = new List<object>();
      if (originatorManagementData1 != null)
      {
        List<string> stringList1 = new List<string>();
        informationToolSettings.Add((object) originatorManagementData1);
        ExternalLateFeeSettings orgLateFeeSettings = ExternalOrgManagementAccessor.GetExternalOrgLateFeeSettings(originatorManagementData1.oid, true);
        informationToolSettings.Add((object) orgLateFeeSettings);
        Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = ExternalOrgManagementAccessor.GetExternalOrgSettings();
        if (externalOrgSettings != null && externalOrgSettings.ContainsKey("Company Rating"))
          informationToolSettings.Add((object) externalOrgSettings["Company Rating"]);
        List<UserInfo> userInfoList = new List<UserInfo>();
        if (!string.IsNullOrEmpty(originatorManagementData1.PrimarySalesRepUserId))
          stringList1.Add(originatorManagementData1.PrimarySalesRepUserId);
        if (!string.IsNullOrEmpty(originatorManagementData1.Manager))
        {
          ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(originatorManagementData1.Manager);
          if ((UserInfo) externalUserInfo != (UserInfo) null)
            informationToolSettings.Add((object) externalUserInfo);
        }
        bool flag2 = flag1;
        if (flag1)
        {
          List<ExternalOrgSalesRep> repsForCurrentOrg = ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCurrentOrg(originatorManagementData1.oid);
          if (repsForCurrentOrg != null && repsForCurrentOrg.Count > 0 && repsForCurrentOrg.FindIndex((Predicate<ExternalOrgSalesRep>) (x => x.userId == currentUserID)) >= 0)
            flag2 = false;
        }
        List<ExternalOriginatorManagementData> originatorManagementDataList1 = ExternalOrgManagementAccessor.GetChildExternalOrganizationByType(originatorManagementData1.oid, new List<int>()
        {
          2
        });
        if (originatorManagementDataList1 != null && originatorManagementDataList1.Count > 0)
        {
          if (flag2)
          {
            List<ExternalOriginatorManagementData> originatorManagementDataList2 = new List<ExternalOriginatorManagementData>();
            foreach (ExternalOriginatorManagementData originatorManagementData2 in originatorManagementDataList1)
            {
              ExternalOriginatorManagementData o = originatorManagementData2;
              List<ExternalOrgSalesRep> salesRepsForCompany = ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCompany(o.oid);
              if (salesRepsForCompany != null && salesRepsForCompany.Count != 0 && salesRepsForCompany.FindIndex((Predicate<ExternalOrgSalesRep>) (x => x.userId == currentUserID)) >= 0 && originatorManagementDataList2.FindIndex((Predicate<ExternalOriginatorManagementData>) (y => y.oid == o.oid)) == -1)
                originatorManagementDataList2.Add(o);
            }
            originatorManagementDataList1 = originatorManagementDataList2;
          }
          informationToolSettings.Add((object) originatorManagementDataList1);
        }
        ExternalOriginatorManagementData originatorManagementData3 = (ExternalOriginatorManagementData) null;
        if (!string.IsNullOrEmpty(branchExteralID) || !string.IsNullOrEmpty(BranchName))
        {
          foreach (ExternalOriginatorManagementData originatorManagementData4 in originatorManagementDataList1)
          {
            if ((string.IsNullOrEmpty(branchExteralID) || string.Compare(originatorManagementData4.ExternalID, branchExteralID, true) == 0) && (string.IsNullOrEmpty(BranchName) || string.Compare(originatorManagementData4.OrganizationName, BranchName, true) == 0))
            {
              informationToolSettings.Add((object) new ExternalOriginatorManagementData[1]
              {
                originatorManagementData4
              });
              if (!string.IsNullOrEmpty(originatorManagementData4.PrimarySalesRepUserId) && !stringList1.Contains(originatorManagementData4.PrimarySalesRepUserId))
                stringList1.Add(originatorManagementData4.PrimarySalesRepUserId);
              if (!string.IsNullOrEmpty(originatorManagementData4.Manager))
              {
                ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(originatorManagementData4.Manager);
                if ((UserInfo) externalUserInfo != (UserInfo) null)
                  informationToolSettings.Add((object) externalUserInfo);
              }
              originatorManagementData3 = originatorManagementData4;
              break;
            }
          }
        }
        int oid1 = string.IsNullOrEmpty(branchExteralID) && string.IsNullOrEmpty(BranchName) || originatorManagementData3 == null ? originatorManagementData1.oid : originatorManagementData3.oid;
        ExternalOriginatorManagementData externalOrganization1 = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid1);
        List<int> organizationDesendents = ExternalOrgManagementAccessor.GetExternalOrganizationDesendents(oid1);
        List<int> intList = new List<int>();
        intList.Add(oid1);
        foreach (int oid2 in organizationDesendents)
        {
          ExternalOriginatorManagementData externalOrganization2 = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid2);
          if (externalOrganization1.OrganizationType == ExternalOriginatorOrgType.Company && externalOrganization2.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
            intList.Add(oid2);
          else if (externalOrganization1.OrganizationType == ExternalOriginatorOrgType.Branch && externalOrganization2.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
            intList.Add(oid2);
        }
        List<ExternalOrgURL> selectedOrgUrls1 = ExternalOrgManagementAccessor.GetSelectedOrgUrls(originatorManagementData1.oid);
        List<ExternalOrgURL> source = new List<ExternalOrgURL>();
        int num = -1;
        foreach (int oid3 in intList)
        {
          List<ExternalOrgURL> selectedOrgUrls2 = ExternalOrgManagementAccessor.GetSelectedOrgUrls(oid3);
          source.AddRange((IEnumerable<ExternalOrgURL>) selectedOrgUrls2);
        }
        if (source != null && source.Count > 0)
        {
          informationToolSettings.Add((object) source.GroupBy<ExternalOrgURL, string>((System.Func<ExternalOrgURL, string>) (a => a.siteId)).Select<IGrouping<string, ExternalOrgURL>, ExternalOrgURL>((System.Func<IGrouping<string, ExternalOrgURL>, ExternalOrgURL>) (g => g.First<ExternalOrgURL>())).ToList<ExternalOrgURL>());
          if (!string.IsNullOrEmpty(siteID) && source.FindIndex((Predicate<ExternalOrgURL>) (x => x.siteId == siteID)) >= 0)
            num = source.Find((Predicate<ExternalOrgURL>) (x => x.siteId == siteID)).URLID;
        }
        ExternalUserInfo[] externalUserInfoArray = string.IsNullOrEmpty(branchExteralID) ? ExternalOrgManagementAccessor.getAllExternalUserInfos(oid1, true) : ExternalOrgManagementAccessor.GetAllExternalUserInfos(oid1);
        if (originatorManagementData3 != null & flag2)
        {
          List<ExternalOrgSalesRep> repsForCurrentOrg = ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCurrentOrg(originatorManagementData3.oid);
          if (repsForCurrentOrg != null && repsForCurrentOrg.Count > 0 && repsForCurrentOrg.FindIndex((Predicate<ExternalOrgSalesRep>) (x => x.userId == currentUserID)) > -1)
            flag2 = false;
        }
        if (flag2 & flag1 && externalUserInfoArray != null && externalUserInfoArray.Length != 0)
        {
          ArrayList andOrgBySalesRep = ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRep(currentUserID);
          List<string> stringList2 = andOrgBySalesRep == null || andOrgBySalesRep.Count <= 4 ? (List<string>) null : (List<string>) andOrgBySalesRep[4];
          List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
          foreach (ExternalUserInfo externalUserInfo in externalUserInfoArray)
          {
            if (string.Compare(externalUserInfo.SalesRepID, currentUserID, true) == 0 || stringList2 != null && stringList2.Contains(externalUserInfo.ContactID))
              externalUserInfoList.Add(externalUserInfo);
          }
          externalUserInfoArray = externalUserInfoList.ToArray();
          andOrgBySalesRep?.Clear();
          stringList2?.Clear();
        }
        if (source != null && source.Count > 0 && externalUserInfoArray != null && externalUserInfoArray.Length != 0 && num > -1)
        {
          List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
          foreach (ExternalUserInfo externalUserInfo in externalUserInfoArray)
          {
            ExternalUserURL[] externalUserInfoUrLs = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserInfo.ExternalUserID);
            if (externalUserInfoUrLs != null && externalUserInfoUrLs.Length != 0)
            {
              bool flag3 = false;
              foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs)
              {
                if (externalUserUrl.URLID == num)
                {
                  externalUserInfoList.Add(externalUserInfo);
                  break;
                }
                if (flag3)
                  break;
              }
            }
          }
          externalUserInfoArray = externalUserInfoList.ToArray();
        }
        if (externalUserInfoArray != null && externalUserInfoArray.Length != 0)
        {
          foreach (ExternalUserInfo externalUserInfo in externalUserInfoArray)
          {
            if (!string.IsNullOrEmpty(externalUserInfo.SalesRepID) && !stringList1.Contains(externalUserInfo.SalesRepID))
            {
              stringList1.Add(externalUserInfo.SalesRepID);
              ExternalUserURL[] externalUserInfoUrLs = ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserInfo.ExternalUserID);
              if (externalUserInfoUrLs != null && selectedOrgUrls1 != null)
              {
                foreach (ExternalUserURL externalUserUrl in externalUserInfoUrLs)
                {
                  ExternalUserURL url = externalUserUrl;
                  if (source.FindIndex((Predicate<ExternalOrgURL>) (x => x.URLID == url.URLID)) == -1 && selectedOrgUrls1.FindIndex((Predicate<ExternalOrgURL>) (x => x.URLID == url.URLID)) != -1)
                    source.Add(selectedOrgUrls1.Find((Predicate<ExternalOrgURL>) (x => x.URLID == url.URLID)));
                }
              }
            }
          }
        }
        if (stringList1.Count > 0)
        {
          UserInfo[] users = User.GetUsers(stringList1.ToArray());
          if (users != null)
            userInfoList.AddRange((IEnumerable<UserInfo>) users);
        }
        informationToolSettings.Add((object) userInfoList);
        informationToolSettings.Add((object) externalUserInfoArray);
      }
      return informationToolSettings;
    }

    public static List<UserInfo[]> GetAllSalesRepUsers(string currentUserID)
    {
      List<UserInfo> userInfoList1 = new List<UserInfo>();
      List<UserInfo> userInfoList2 = new List<UserInfo>();
      UserInfo[] allUsers = User.GetAllUsers(currentUserID);
      userInfoList1.AddRange((IEnumerable<UserInfo>) allUsers);
      for (int index = userInfoList1.Count - 1; index >= 0; --index)
      {
        if (!FeaturesAclDbAccessor.CheckPermission(AclFeature.ExternalSettings_ContactSalesRep, userInfoList1[index]))
        {
          userInfoList2.Add(userInfoList1[index]);
          userInfoList1.RemoveAt(index);
        }
      }
      return new List<UserInfo[]>()
      {
        userInfoList1.ToArray(),
        userInfoList2.ToArray()
      };
    }

    private static bool isCurrentUserTPOSalesRep(string currentUserID)
    {
      if (string.IsNullOrEmpty(currentUserID))
        return false;
      try
      {
        UserInfo userById = User.GetUserById(currentUserID);
        return !(userById == (UserInfo) null) && !userById.IsAdministrator() && !userById.IsSuperAdministrator() && !userById.IsTopLevelAdministrator() && FeaturesAclDbAccessor.CheckPermission(AclFeature.ExternalSettings_ContactSalesRep, userById);
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationBranches(
      bool forLender,
      int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> organizationBranches = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.Reset();
      dbQueryBuilder.Append("Select * from [ExternalOriginatorManagement] b  inner join [ExternalOrgDetail] on  b.oid = [externalOrgID]  where b.oid in ( Select descendent from ExternalOrgDescendents where oid = " + (object) oid + ") or b.oid = " + (object) oid + " and OrganizationType = 2");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          organizationBranches.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
        }
      }
      return organizationBranches;
    }

    public static List<ExternalOriginatorManagementData> GetExternalOrganizationBranchesBySite(
      bool forLender,
      int oid,
      string siteID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> organizationBranchesBySite = new List<ExternalOriginatorManagementData>();
      dbQueryBuilder.Reset();
      dbQueryBuilder.Append("Select * from [ExternalOriginatorManagement] b  inner join [ExternalOrgDetail] on  b.oid = [externalOrgID]  where (b.oid in ( Select descendent from ExternalOrgDescendents where oid = " + (object) oid + ") or b.oid = " + (object) oid + ") and OrganizationType = 2 and b.oid in (select externalOrgID from [ExternalOrgSelectedURL] c inner join [ExternalOrgURLs] d on c.urlid = d.urlid where d.siteid =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteID) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          organizationBranchesBySite.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
        }
      }
      return organizationBranchesBySite;
    }

    public static List<ExternalOriginatorManagementData> GetChildExternalOrganizationByType(
      int oid,
      List<int> orgType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalOriginatorManagementData> organizationByType = new List<ExternalOriginatorManagementData>();
      List<int> organizationDesendents = ExternalOrgManagementAccessor.GetExternalOrganizationDesendents(oid);
      DataRowCollection dataRowCollection = (DataRowCollection) null;
      if (organizationDesendents.Any<int>() && orgType.Any<int>())
      {
        string str1 = string.Join<int>(",", (IEnumerable<int>) organizationDesendents);
        string str2 = string.Join<int>(",", (IEnumerable<int>) orgType);
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append("Select * from [ExternalOriginatorManagement] b  inner join [ExternalOrgDetail] on  b.oid = [externalOrgID]  where b.oid in (" + str1 + ") and OrganizationType in (" + str2 + ")");
        dataRowCollection = dbQueryBuilder.Execute();
      }
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ExternalOriginatorManagementData managementFromDatarow = ExternalOrgManagementAccessor.getExternalOriginatorManagementFromDatarow(row);
          organizationByType.Add(ExternalOrgManagementAccessor.addExternalOrgDetail(managementFromDatarow, row));
        }
      }
      return organizationByType;
    }

    public static List<ExternalUserInfo> GetAccessibleExternalUserInfos(string userid)
    {
      List<ExternalUserInfo> externalUserInfos = new List<ExternalUserInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @userID varchar(38)");
      dbQueryBuilder.AppendLine("declare @managerOrgID int");
      dbQueryBuilder.AppendLine("declare @externalOrgID int");
      dbQueryBuilder.AppendLine("set @managerOrgID = -1");
      dbQueryBuilder.AppendLine("set @externalOrgID = -1");
      dbQueryBuilder.AppendLine("set @userID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid));
      dbQueryBuilder.AppendLine("if exists (select * from ExternalOrgDetail org where org.ManagerUserID = @userID)");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("select top 1 @managerOrgID = externalOrgID from ExternalOrgDetail org inner join ExternalOriginatorManagement eOrg on org.externalOrgID = eOrg.oid where org.ManagerUserID = @userID order by eOrg.depth");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("select top 1 @externalOrgID = oid from ExternalOriginatorManagement eOrg where externalID in( select externalID from ExternalOriginatorManagement a inner join ExternalUsers eUser on a.oid = eUser.externalOrgID where external_userid = @userID) order by eOrg.depth");
      dbQueryBuilder.AppendLine("select top 1 @externalOrgID = oid");
      dbQueryBuilder.AppendLine("from ExternalOriginatorManagement where oid in (@managerOrgID, @externalOrgID) order by depth");
      dbQueryBuilder.AppendLine("select * ");
      dbQueryBuilder.AppendLine("from ExternalUsers");
      dbQueryBuilder.AppendLine("where externalOrgID in (select descendent from ExternalOrgDescendents where oid = @externalOrgID) or externalOrgID = @externalOrgID");
      try
      {
        DataRowCollection rows = dbQueryBuilder.Execute();
        if (rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(rows));
          foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
          {
            DataRow row = dataRow;
            ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>());
            externalUserInfos.Add(externalUserInfo);
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAccessibleExternalUserInfos:" + ex.Message);
      }
      return externalUserInfos;
    }

    public static DataRowCollection GetAccessibleExternalUsersLOLPPrimarySalesRep(string userid)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Personas.personaID INTO #PrimarySalesRepMappedPersonas FROM Personas inner join RolePersonas on personas.personaID = RolePersonas.personaID");
      dbQueryBuilder.AppendLine("where RolePersonas.roleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID = 7))");
      dbQueryBuilder.AppendLine("SELECT Personas.personaID INTO #LOMappedPersonas FROM Personas inner join RolePersonas on personas.personaID = RolePersonas.personaID ");
      dbQueryBuilder.AppendLine("where RolePersonas.roleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID = 8))");
      dbQueryBuilder.AppendLine("SELECT Personas.personaID INTO #LPMappedPersonas FROM Personas inner join RolePersonas on personas.personaID = RolePersonas.personaID");
      dbQueryBuilder.AppendLine("where RolePersonas.roleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID = 9))");
      dbQueryBuilder.AppendLine("DECLARE @externalOrgID INT");
      dbQueryBuilder.AppendLine("SET @externalOrgID = -1");
      dbQueryBuilder.AppendLine("declare @userID varchar(38)");
      dbQueryBuilder.AppendLine("set @userID = (select TOP 1 external_userid from ExternalUsers where ContactID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid) + ")");
      dbQueryBuilder.AppendLine("SELECT @externalOrgID = externalOrgID  FROM ExternalUsers WHERE external_userid =@userID");
      dbQueryBuilder.AppendLine("SELECT ExternalUsers.ContactID,ExternalUsers.Last_name, ExternalUsers.Middle_name, ExternalUsers.Suffix_name, ExternalUsers.First_name, ExternalUsers.Title, ExternalUsers.externalOrgID,");
      dbQueryBuilder.AppendLine("ExternalOriginatorManagement.ExternalID,ExternalOriginatorManagement.OrganizationName ,");
      dbQueryBuilder.AppendLine("CASE (select COUNT(*) from UserPersona where userid = ContactID and (UserPersona.personaID IN (select personaID from #LOMappedPersonas)))");
      dbQueryBuilder.AppendLine("WHEN 0 THEN 0");
      dbQueryBuilder.AppendLine("ELSE 1");
      dbQueryBuilder.AppendLine("END AS LO ");
      dbQueryBuilder.AppendLine(",CASE (select COUNT(*) from UserPersona where userid = ContactID and (UserPersona.personaID IN (select personaID from #LPMappedPersonas)))");
      dbQueryBuilder.AppendLine("WHEN 0 THEN 0 ");
      dbQueryBuilder.AppendLine("ELSE 1 ");
      dbQueryBuilder.AppendLine("END AS LP ");
      dbQueryBuilder.AppendLine(",CASE (select COUNT(*) from UserPersona where userid = ContactID and (UserPersona.personaID IN (select personaID from #PrimarySalesRepMappedPersonas))) ");
      dbQueryBuilder.AppendLine("WHEN 0 THEN 0 ");
      dbQueryBuilder.AppendLine("ELSE 1 ");
      dbQueryBuilder.AppendLine("END AS PrimarySalesRep");
      dbQueryBuilder.AppendLine("FROM\tExternalUsers INNER JOIN ExternalOriginatorManagement on ExternalUsers.externalOrgID = ExternalOriginatorManagement.oid ");
      dbQueryBuilder.AppendLine("where\tExternalUsers.externalOrgID in ");
      dbQueryBuilder.AppendLine("( ");
      dbQueryBuilder.AppendLine("select\tExternalOriginatorManagement.oid ");
      dbQueryBuilder.AppendLine("from\tExternalOriginatorManagement inner join [ExternalOrgDetail] ");
      dbQueryBuilder.AppendLine("on\t\tExternalOriginatorManagement.oid = [ExternalOrgDetail].externalOrgID ");
      dbQueryBuilder.AppendLine("where\tExternalOriginatorManagement.oid = @externalOrgID AND [ExternalOrgDetail].OrganizationType IN (0,2)");
      dbQueryBuilder.AppendLine("UNION");
      dbQueryBuilder.AppendLine("select\tExternalOriginatorManagement.oid ");
      dbQueryBuilder.AppendLine("from\tExternalOriginatorManagement inner join [ExternalOrgDetail]");
      dbQueryBuilder.AppendLine("on\t\tExternalOriginatorManagement.oid = [ExternalOrgDetail].externalOrgID ");
      dbQueryBuilder.AppendLine("where\tExternalOriginatorManagement.oid = @externalOrgID AND ExternalOriginatorManagement.VisibleOnTPOWCSite =1 ");
      dbQueryBuilder.AppendLine("        AND [ExternalOrgDetail].OrganizationType IN (1,3)");
      dbQueryBuilder.AppendLine("UNION");
      dbQueryBuilder.AppendLine("select\tExternalOriginatorManagement.oid");
      dbQueryBuilder.AppendLine("from\tExternalOriginatorManagement inner join [ExternalOrgDetail]");
      dbQueryBuilder.AppendLine("ON [ExternalOrgDetail].externalOrgID = ExternalOriginatorManagement.oid");
      dbQueryBuilder.AppendLine("where ExternalOriginatorManagement.oid IN");
      dbQueryBuilder.AppendLine("(SELECT descendent FROM ExternalOrgDescendents WHERE oid = @externalOrgID) AND [ExternalOrgDetail].OrganizationType IN (0,2)");
      dbQueryBuilder.AppendLine("UNION");
      dbQueryBuilder.AppendLine("select\tExternalOriginatorManagement.oid ");
      dbQueryBuilder.AppendLine("from\tExternalOriginatorManagement inner join [ExternalOrgDetail]");
      dbQueryBuilder.AppendLine("ON [ExternalOrgDetail].externalOrgID = ExternalOriginatorManagement.oid");
      dbQueryBuilder.AppendLine("where ExternalOriginatorManagement.oid IN");
      dbQueryBuilder.AppendLine("(SELECT descendent FROM ExternalOrgDescendents ");
      dbQueryBuilder.AppendLine("\t\t\t\t   WHERE oid = @externalOrgID)AND ExternalOriginatorManagement.VisibleOnTPOWCSite = 1 AND [ExternalOrgDetail].OrganizationType IN (1,3)");
      dbQueryBuilder.AppendLine(") ");
      try
      {
        return dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAccessibleExternalUserInfos:" + ex.Message);
      }
    }

    public static List<object> GetAccessibleExternalUserInfoList(string userid)
    {
      List<object> externalUserInfoList1 = new List<object>();
      List<ExternalUserInfo> externalUserInfoList2 = new List<ExternalUserInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @userID varchar(38)");
      dbQueryBuilder.AppendLine("declare @managerOrgID int");
      dbQueryBuilder.AppendLine("declare @externalOrgID int");
      dbQueryBuilder.AppendLine("set @managerOrgID = -1");
      dbQueryBuilder.AppendLine("set @externalOrgID = -1");
      dbQueryBuilder.AppendLine("set @userID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(userid));
      dbQueryBuilder.AppendLine("if exists (select * from ExternalOrgDetail org where org.ManagerUserID = @userID)");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("select top 1 @managerOrgID = externalOrgID from ExternalOrgDetail org inner join ExternalOriginatorManagement eOrg on org.externalOrgID = eOrg.oid where org.ManagerUserID = @userID order by eOrg.depth");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("select top 1 @externalOrgID = oid from ExternalOriginatorManagement eOrg where externalID in( select externalID from ExternalOriginatorManagement a inner join ExternalUsers eUser on a.oid = eUser.externalOrgID where external_userid = @userID) order by eOrg.depth");
      dbQueryBuilder.AppendLine("select top 1 @externalOrgID = oid");
      dbQueryBuilder.AppendLine("from ExternalOriginatorManagement where oid in (@managerOrgID, @externalOrgID) order by depth");
      dbQueryBuilder.AppendLine("select * ");
      dbQueryBuilder.AppendLine("from ExternalUsers");
      dbQueryBuilder.AppendLine("where externalOrgID in (select descendent from ExternalOrgDescendents where oid = @externalOrgID) or externalOrgID = @externalOrgID");
      dbQueryBuilder.AppendLine("select a.external_userid, a.ContactID, c.urlid, c.siteid ");
      dbQueryBuilder.AppendLine("from ExternalUsers a inner join ExternalUserURLs b on a.external_userid = b.external_userid inner join externalorgurls c on b.urlid = c.urlid");
      dbQueryBuilder.AppendLine("where a.externalOrgID in (select descendent from ExternalOrgDescendents where oid = @externalOrgID) or externalOrgID = @externalOrgID");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row1 in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow row = row1;
            ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(row, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"]))).FirstOrDefault<UserInfo>());
            externalUserInfoList2.Add(externalUserInfo);
          }
        }
        Dictionary<string, List<string>> dictionary1 = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
        List<object> list1 = Enumerable.Cast<DataRow>(dataSet.Tables[1].Rows).Select<DataRow, object>((System.Func<DataRow, object>) (r => r["siteid"])).Distinct<object>().ToList<object>();
        List<object> list2 = Enumerable.Cast<DataRow>(dataSet.Tables[1].Rows).Select<DataRow, object>((System.Func<DataRow, object>) (r => r["urlid"])).Distinct<object>().ToList<object>();
        foreach (string str in list1)
        {
          string xx = str;
          dictionary1.Add(xx, Enumerable.Cast<DataRow>(dataSet.Tables[1].Rows).Where<DataRow>((System.Func<DataRow, bool>) (r => string.Concat(r["siteid"]) == xx)).Select<DataRow, string>((System.Func<DataRow, string>) (r => string.Concat(r["external_userid"]))).Distinct<string>().ToList<string>());
        }
        foreach (int num in list2)
        {
          int yy = num;
          dictionary2.Add(string.Concat((object) yy), Enumerable.Cast<DataRow>(dataSet.Tables[1].Rows).Where<DataRow>((System.Func<DataRow, bool>) (r => string.Concat(r["urlid"]) == string.Concat((object) yy))).Select<DataRow, string>((System.Func<DataRow, string>) (r => string.Concat(r["external_userid"]))).Distinct<string>().ToList<string>());
        }
        externalUserInfoList1.Add((object) externalUserInfoList2);
        externalUserInfoList1.Add((object) dictionary1);
        externalUserInfoList1.Add((object) dictionary2);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAccessibleExternalUserInfos:" + ex.Message);
      }
      return externalUserInfoList1;
    }

    public static ExternalOrgURL AssociateExternalOrganisationUrl(
      int oid,
      string siteId,
      int EntityType)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        ExternalOriginatorManagementData externalOrganization = ExternalOrgManagementAccessor.GetExternalOrganization(false, oid);
        dbQueryBuilder.Append("Select * from [ExternalOrgURLs] where siteid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteId));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          int num = -1;
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
            num = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["UrlId"]);
          if (Convert.ToInt32((object) externalOrganization.entityType) == EntityType)
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("if not exists(Select * from [ExternalOrgURLs] a inner join [ExternalOrgSelectedURL] b on a.Urlid = b.urlid where a.Siteid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteId) + " and b.[externalOrgID] = " + (object) oid + ")");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine("Insert into [ExternalOrgSelectedURL] (urlId,externalOrgID,UrlEntityType) values (" + (object) num + "," + (object) oid + "," + (object) EntityType + ")");
            dbQueryBuilder.AppendLine("end");
          }
          dbQueryBuilder.ExecuteNonQuery();
        }
        return ExternalOrgManagementAccessor.GetSelectedOrgUrls(oid).FirstOrDefault<ExternalOrgURL>((System.Func<ExternalOrgURL, bool>) (a => a.siteId == siteId));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssociateExternalOrganisationUrl:" + ex.Message);
      }
    }

    public static void DeleteExternalOrgSelectedUrl(int oid, int urlid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from ExternalOrgSelectedURL where externalOrgID=" + (object) oid + " and urlId =" + (object) urlid);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrgSelectedUrl:" + ex.Message);
      }
    }

    public static List<object> GetAllExternalOrganizationNames()
    {
      List<ExternalOriginatorManagementData> parentOrganizations = ExternalOrgManagementAccessor.GetAllExternalParentOrganizations(false);
      List<KeyValuePair<string, string>> keyValuePairList1 = new List<KeyValuePair<string, string>>();
      List<KeyValuePair<string, int>> keyValuePairList2 = new List<KeyValuePair<string, int>>();
      if (parentOrganizations != null && parentOrganizations.Count > 0)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in parentOrganizations)
        {
          keyValuePairList1.Add(new KeyValuePair<string, string>(originatorManagementData.OrganizationName.ToLower(), originatorManagementData.HierarchyPath));
          keyValuePairList2.Add(new KeyValuePair<string, int>(originatorManagementData.OrganizationName.ToLower(), originatorManagementData.oid));
        }
      }
      List<long> allTpoId = ExternalOrgManagementAccessor.GetAllTpoID();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT OrganizationName, HierarchyPath FROM [ExternalOriginatorManagement] ORDER BY OrganizationName");
      string empty = string.Empty;
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          string str1 = string.Concat(dataRow["OrganizationName"]);
          stringList1.Add(str1.ToLower());
          string str2 = string.Concat(dataRow["HierarchyPath"]);
          stringList2.Add(str2.ToLower());
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllExternalOrganizationNames: Cannot fetch all OrganizationNames from [ExternalOriginatorManagement] table.\r\n" + ex.Message);
      }
      return new List<object>()
      {
        (object) keyValuePairList1,
        (object) allTpoId,
        (object) keyValuePairList2,
        (object) stringList1,
        (object) stringList2
      };
    }

    [PgReady]
    public static bool CheckIfAnyTPOSiteExists()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        try
        {
          pgDbQueryBuilder.AppendLine("SELECT COUNT(1) FROM [ExternalOrgURLs] WHERE TPOAdminLinkAccess = 1");
          return Utils.ParseInt(pgDbQueryBuilder.ExecuteScalar()) > 0;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("CheckIfAnyTPOSiteExists: Cannot fetch records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        try
        {
          dbQueryBuilder.AppendLine("SELECT COUNT(1) FROM [ExternalOrgURLs] WHERE TPOAdminLinkAccess = 1");
          return Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > 0;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("CheckIfAnyTPOSiteExists: Cannot fetch records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
    }

    [PgReady]
    public static bool CheckIfTPOWebCenterProvisioned()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        try
        {
          pgDbQueryBuilder.AppendLine("SELECT COUNT(1) FROM [ExternalOrgURLs] WHERE TPOAdminLinkAccess = 0");
          return Utils.ParseInt(pgDbQueryBuilder.ExecuteScalar()) > 0;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("CheckIfAnyTPOSiteExists: Cannot fetch records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        try
        {
          dbQueryBuilder.AppendLine("SELECT COUNT(1) FROM [ExternalOrgURLs] WHERE TPOAdminLinkAccess = 0");
          return Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > 0;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("CheckIfAnyTPOSiteExists: Cannot fetch records from [ExternalOrgURLs] table.\r\n" + ex.Message);
        }
      }
    }

    public static bool CheckIfNewTPOSiteExists(string siteID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("SELECT COUNT(1) FROM [ExternalOrgURLs] WHERE [SiteId] =" + EllieMae.EMLite.DataAccess.SQL.EncodeString(siteID) + " AND TPOAdminLinkAccess = 1");
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfNewTPOSiteExists: Cannot fetch records from [ExternalOrgURLs] table.\r\n" + ex.Message);
      }
    }

    public static List<object> GetTPOForClosingVendorInformation(string tpoOrgID, string tpoLOID)
    {
      List<ExternalOriginatorManagementData> organizationByTpoid = tpoOrgID != string.Empty ? ExternalOrgManagementAccessor.GetExternalOrganizationByTPOID(tpoOrgID) : (List<ExternalOriginatorManagementData>) null;
      ExternalOriginatorManagementData originatorManagementData = organizationByTpoid == null || organizationByTpoid.Count <= 0 ? (ExternalOriginatorManagementData) null : organizationByTpoid[0];
      BranchExtLicensing extLicenseDetails = originatorManagementData != null ? ExternalOrgManagementAccessor.GetExtLicenseDetails(originatorManagementData.oid) : (BranchExtLicensing) null;
      ExternalUserInfo userInfoByContactId = tpoLOID != string.Empty ? ExternalOrgManagementAccessor.GetExternalUserInfoByContactId(tpoLOID) : (ExternalUserInfo) null;
      OrgInfo orgInfo = (OrgInfo) null;
      try
      {
        if ((UserInfo) userInfoByContactId != (UserInfo) null)
        {
          if (userInfoByContactId.SalesRepID != string.Empty)
          {
            UserInfo userById = User.GetUserById(userInfoByContactId.SalesRepID);
            if (userById != (UserInfo) null)
              orgInfo = OrganizationStore.GetOrganizationForClosingVendorInformation(userById.OrgId);
          }
        }
      }
      catch (Exception ex)
      {
      }
      return new List<object>()
      {
        (object) originatorManagementData,
        (object) extLicenseDetails,
        (object) orgInfo
      };
    }

    public static List<ExternalUserInfo> GetAllLOLPUsers()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> allLolpUsers = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("Select * from ExternalUsers where (Roles & 2 = 2  or Roles & 1 = 1)");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] externalTpoUsers = User.GetExternalTPOUsers();
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            allLolpUsers.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) externalTpoUsers).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return allLolpUsers;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetAllTPOLOLPUsers(List<int> personaIds)
    {
      if (personaIds == null || personaIds.Count == 0)
        return new List<ExternalUserInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<ExternalUserInfo> allTpololpUsers = new List<ExternalUserInfo>();
      dbQueryBuilder.AppendLine("SELECT EU.* FROM [ExternalUsers] EU ");
      dbQueryBuilder.AppendLine("INNER JOIN [UserPersona] UP ON EU.ContactId = UP.userid ");
      dbQueryBuilder.AppendLine("WHERE UP.personaID in (" + string.Join<int>(",", (IEnumerable<int>) personaIds) + ")");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            allTpololpUsers.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return allTpololpUsers;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
    }

    public static List<ExternalUserInfo> GetAllAuthorizedDealers(
      int externalOrgID,
      bool onlyContactIds = false)
    {
      List<ExternalUserInfo> authorizedDealers = new List<ExternalUserInfo>();
      try
      {
        DataSet dataSet = ExternalOrgManagementAccessor.GetAllAuthorizedDealersContactIdsQuery(externalOrgID, onlyContactIds).ExecuteSetQuery();
        if (dataSet == null)
          return new List<ExternalUserInfo>();
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          if (onlyContactIds)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
              authorizedDealers.Add(new ExternalUserInfo()
              {
                ContactID = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactID"])
              });
            return authorizedDealers;
          }
          UserInfo[] users = User.GetUsers(ExternalOrgManagementAccessor.getContactIdList(dataSet.Tables[0].Rows));
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            DataRow r = row;
            authorizedDealers.Add(ExternalOrgManagementAccessor.databaseRowToExternalUserInfo(r, ((IEnumerable<UserInfo>) users).Where<UserInfo>((System.Func<UserInfo, bool>) (u => u.Userid == EllieMae.EMLite.DataAccess.SQL.DecodeString(r["contactID"]))).FirstOrDefault<UserInfo>()));
          }
        }
        return authorizedDealers;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from ExternalUsers.\r\n" + ex.Message);
      }
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder GetAllAuthorizedDealersContactIdsQuery(
      int externalOrgID,
      bool onlyContactIds = false)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dealersContactIdsQuery = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (onlyContactIds)
        dealersContactIdsQuery.AppendLine("(SELECT EU.ContactId FROM [ExternalUsers] EU ");
      else
        dealersContactIdsQuery.AppendLine("(SELECT EU.* FROM [ExternalUsers] EU ");
      dealersContactIdsQuery.AppendLine("INNER JOIN [UserPersona] UP ON EU.ContactId = UP.userid ");
      dealersContactIdsQuery.AppendLine("INNER JOIN [Acl_Features] AF on UP.personaID = AF.personaID ");
      dealersContactIdsQuery.AppendLine("WHERE AF.access = 1 AND AF.featureID = " + (object) 2016);
      if (externalOrgID != -1)
        dealersContactIdsQuery.AppendLine("AND externalOrgID = " + (object) externalOrgID);
      dealersContactIdsQuery.AppendLine("EXCEPT");
      if (onlyContactIds)
        dealersContactIdsQuery.AppendLine("SELECT EU.ContactId FROM [ExternalUsers] EU ");
      else
        dealersContactIdsQuery.AppendLine("SELECT EU.* FROM [ExternalUsers] EU ");
      dealersContactIdsQuery.AppendLine("LEFT JOIN Acl_Features_User AF ON EU.ContactId = AF.userid");
      dealersContactIdsQuery.AppendLine("WHERE AF.access = 0 AND AF.featureID = " + (object) 2016);
      if (externalOrgID != -1)
        dealersContactIdsQuery.AppendLine("AND externalOrgID = " + (object) externalOrgID);
      dealersContactIdsQuery.AppendLine(")");
      dealersContactIdsQuery.AppendLine("UNION");
      if (onlyContactIds)
        dealersContactIdsQuery.AppendLine("SELECT EU.ContactId FROM [ExternalUsers] EU ");
      else
        dealersContactIdsQuery.AppendLine("SELECT EU.* FROM [ExternalUsers] EU ");
      dealersContactIdsQuery.AppendLine("LEFT JOIN Acl_Features_User AF ON EU.ContactId = AF.userid");
      dealersContactIdsQuery.AppendLine("WHERE AF.access = 1 AND AF.featureID = " + (object) 2016);
      if (externalOrgID != -1)
        dealersContactIdsQuery.AppendLine("AND externalOrgID = " + (object) externalOrgID);
      return dealersContactIdsQuery;
    }

    public static HashSet<string> CheckIfTPOUsersHaveLoansAssigned(List<string> contactIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str = string.Join("'),('", (IEnumerable<string>) contactIds);
        HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        dbQueryBuilder.AppendLine("Create table #ContactIdsTemp (contactId varchar(50))");
        dbQueryBuilder.AppendLine("Insert into #ContactIdsTemp values ('" + str + "')");
        dbQueryBuilder.AppendLine("Select contactId from #ContactIdsTemp temp where exists (select 1 from Loansummary ls where temp.contactId =  ls.TPOLOID or temp.contactId = ls.TPOLPID)");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return stringSet;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          stringSet.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["contactId"]));
        return stringSet;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("CheckIfTPOUserLoansExists: Cannot fetch records from [LoanSummary] table.\r\n" + ex.Message);
      }
    }

    public static bool ReassignTPOLoans(
      ExternalUserInfo oldUser,
      ExternalUserInfo newUser,
      UserInfo currentUser,
      bool isTPOMVP)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> loanGuids = new List<string>();
      dbQueryBuilder.AppendLine("Select * from LoanSummary where TPOLOID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oldUser.ContactID) + " or TPOLPID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oldUser.ContactID));
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return false;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          stringList1.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
          if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLOID"]).Equals(oldUser.ContactID))
            stringList2.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
          if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLPID"]).Equals(oldUser.ContactID))
            stringList3.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
          if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLOID"]).Equals(oldUser.ContactID) && EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLPID"]).Equals(oldUser.ContactID))
            loanGuids.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("Cannot fetch records from LoanSummary.\r\n" + ex.Message);
      }
      if (stringList1.Count > 0)
      {
        ExternalOriginatorManagementData currentOrg = ExternalOrgManagementAccessor.GetExternalOrganization(false, newUser.ExternalOrgID);
        ExternalOriginatorManagementData rootOrg;
        if (currentOrg.Parent != 0)
        {
          rootOrg = ExternalOrgManagementAccessor.GetRootOrganisation(false, newUser.ExternalOrgID);
        }
        else
        {
          rootOrg = currentOrg;
          currentOrg = (ExternalOriginatorManagementData) null;
        }
        Dictionary<string, string> fields = new Dictionary<string, string>();
        if (oldUser.ExternalUserID.Equals(newUser.ExternalUserID) && stringList2.Count > 0 && TPOUtils.IsLoanOfficer(newUser.Roles))
        {
          ExternalOrgManagementAccessor.PopulateCompanyAndBranchInfo(fields, currentOrg, rootOrg);
          ExternalOrgManagementAccessor.PopulateLOUser(fields, oldUser);
          ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList2, currentUser, fields);
          ExternalOrgManagementAccessor.UpdateTPOLoanAssociate(stringList2, oldUser, RealWorldRoleID.TPOLoanOfficer);
        }
        else if (!oldUser.ExternalUserID.Equals(newUser.ExternalUserID))
        {
          if (stringList2.Count > 0 && TPOUtils.IsLoanOfficer(newUser.Roles) | isTPOMVP)
          {
            ExternalOrgManagementAccessor.PopulateCompanyAndBranchInfo(fields, currentOrg, rootOrg);
            ExternalOrgManagementAccessor.PopulateLOUser(fields, newUser);
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList2, currentUser, fields);
            ExternalOrgManagementAccessor.UpdateTPOLoanAssociate(stringList2, newUser, RealWorldRoleID.TPOLoanOfficer);
            fields.Clear();
          }
          if (stringList3.Count > 0 && TPOUtils.IsLoanProcessor(newUser.Roles) | isTPOMVP)
          {
            ExternalOrgManagementAccessor.PopulateLPUser(fields, newUser);
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList3, currentUser, fields);
            ExternalOrgManagementAccessor.UpdateTPOLoanAssociate(stringList3, newUser, RealWorldRoleID.TPOLoanProcessor);
            fields.Clear();
          }
          if (loanGuids.Count > 0 && !TPOUtils.IsLoanProcessor(newUser.Roles) | isTPOMVP)
          {
            ExternalOrgManagementAccessor.PopulateBlankLPUser(fields);
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(loanGuids, currentUser, fields);
            ExternalOrgManagementAccessor.UpdateTPOLoanAssociateWithBlankData(stringList3, RealWorldRoleID.TPOLoanProcessor);
            fields.Clear();
          }
        }
      }
      return true;
    }

    public static bool ReassignTPOLoanSalesRep(
      List<int> orgList,
      List<string> userList,
      UserInfo currentUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<string> source = new List<string>();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      foreach (string user in userList)
      {
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(user);
        dbQueryBuilder.AppendLine("Select * from LoanSummary where TPOLOID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserInfo.ContactID) + " or TPOLPID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalUserInfo.ContactID));
        try
        {
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          if (dataSet != null)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              source.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
              if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLOID"]).Equals(externalUserInfo.ContactID))
                stringList1.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
              if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLPID"]).Equals(externalUserInfo.ContactID))
                stringList2.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
              if (EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLOID"]).Equals(externalUserInfo.ContactID) && EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TPOLPID"]).Equals(externalUserInfo.ContactID))
                stringList4.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
            }
            dbQueryBuilder.Reset();
          }
          else
            continue;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("Cannot fetch records from LoanSummary.\r\n" + ex.Message);
        }
        Dictionary<string, string> fields = new Dictionary<string, string>();
        if (source.Any<string>())
        {
          if (stringList1.Any<string>() && TPOUtils.IsLoanOfficer(externalUserInfo.Roles))
          {
            ExternalOrgManagementAccessor.PopulateLOUser(fields, externalUserInfo);
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList1, currentUser, fields);
          }
          if (stringList2.Any<string>() && TPOUtils.IsLoanProcessor(externalUserInfo.Roles))
          {
            ExternalOrgManagementAccessor.PopulateLPUser(fields, externalUserInfo);
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList2, currentUser, fields);
          }
        }
        source.Clear();
        stringList1.Clear();
        stringList2.Clear();
        stringList4.Clear();
        fields.Clear();
      }
      foreach (int org in orgList)
      {
        Dictionary<string, string> fields = new Dictionary<string, string>();
        ExternalOriginatorManagementData externalOrganization = ExternalOrgManagementAccessor.GetExternalOrganization(false, org);
        if (externalOrganization.Parent != 0)
        {
          dbQueryBuilder.AppendLine("Select * from LoanSummary where [TPOBranchID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.ExternalID));
          UserInfo userById = User.GetUserById(externalOrganization.PrimarySalesRepUserId);
          if (userById != (UserInfo) null)
          {
            fields.Add("TPO.X54", userById.FirstName + " " + userById.LastName);
            fields.Add("TPO.X55", userById.Userid);
          }
          else
          {
            fields.Add("TPO.X54", "");
            fields.Add("TPO.X55", "");
          }
        }
        else
        {
          dbQueryBuilder.AppendLine("Select * from LoanSummary where [TPOCompanyID] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(externalOrganization.ExternalID));
          UserInfo userById = User.GetUserById(externalOrganization.PrimarySalesRepUserId);
          if (userById != (UserInfo) null)
          {
            fields.Add("TPO.X30", userById.FirstName + " " + userById.LastName);
            fields.Add("TPO.X31", userById.Userid);
          }
          else
          {
            fields.Add("TPO.X30", "");
            fields.Add("TPO.X31", "");
          }
        }
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            stringList3.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]));
          if (stringList3.Any<string>())
            ExternalOrgManagementAccessor.TPOLoansBatchUpdate(stringList3, currentUser, fields);
          dbQueryBuilder.Reset();
          stringList3.Clear();
          fields.Clear();
        }
      }
      return true;
    }

    private static bool TPOLoansBatchUpdate(
      List<string> loanGuids,
      UserInfo currentUser,
      Dictionary<string, string> fields)
    {
      LoanSetBatch loanSetBatch = new LoanSetBatch();
      loanSetBatch.LoanGuids.AddRange((IEnumerable<string>) loanGuids.ToArray());
      foreach (KeyValuePair<string, string> field in fields)
      {
        LoanBatchField loanBatchField = new LoanBatchField(field.Key, (object) field.Value);
        loanSetBatch.Fields.Add(loanBatchField);
      }
      LoanBatchUpdateAccessor.SubmitBatch((LoanBatch) loanSetBatch, currentUser, true);
      return true;
    }

    private static void PopulateCompanyAndBranchInfo(
      Dictionary<string, string> fields,
      ExternalOriginatorManagementData currentOrg,
      ExternalOriginatorManagementData rootOrg)
    {
      Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = ExternalOrgManagementAccessor.GetExternalOrgSettings();
      if (rootOrg != null)
      {
        ExternalOrgDBAName defaultDbaName = ExternalOrgManagementAccessor.GetDefaultDBAName(rootOrg.oid);
        fields.Add("TPO.X15", rootOrg.ExternalID);
        fields.Add("TPO.X14", rootOrg.OrganizationName);
        fields.Add("TPO.X16", rootOrg.OrgID);
        fields.Add("TPO.X17", rootOrg.CompanyLegalName);
        fields.Add("TPO.X24", defaultDbaName != null ? defaultDbaName.Name : "");
        fields.Add("TPO.X18", rootOrg.Address);
        fields.Add("TPO.X19", rootOrg.City);
        fields.Add("TPO.X20", rootOrg.State);
        fields.Add("TPO.X21", rootOrg.Zip);
        fields.Add("TPO.X22", rootOrg.PhoneNumber);
        fields.Add("TPO.X23", rootOrg.FaxNumber);
        if (externalOrgSettings != null && externalOrgSettings.ContainsKey("Company Rating"))
          fields.Add("TPO.X27", ExternalOrgManagementAccessor.translateRating(rootOrg.CompanyRating, externalOrgSettings["Company Rating"]));
        else
          fields.Add("TPO.X27", "");
        ExternalUserInfo externalUserInfo = ExternalOrgManagementAccessor.GetExternalUserInfo(rootOrg.Manager);
        if ((UserInfo) externalUserInfo != (UserInfo) null)
        {
          fields.Add("TPO.X28", externalUserInfo.FirstName + " " + externalUserInfo.LastName);
          fields.Add("TPO.X29", externalUserInfo.Email);
        }
        else
        {
          fields.Add("TPO.X28", "");
          fields.Add("TPO.X29", "");
        }
        UserInfo userById = User.GetUserById(rootOrg.PrimarySalesRepUserId);
        if (userById != (UserInfo) null)
        {
          fields.Add("TPO.X30", userById.FirstName + " " + userById.LastName);
          fields.Add("TPO.X31", userById.Userid);
        }
        else
        {
          fields.Add("TPO.X30", "");
          fields.Add("TPO.X31", "");
        }
      }
      if (currentOrg != null && (currentOrg.OrganizationType == ExternalOriginatorOrgType.BranchExtension || currentOrg.OrganizationType == ExternalOriginatorOrgType.CompanyExtension))
        currentOrg = (ExternalOriginatorManagementData) null;
      ExternalOrgDBAName externalOrgDbaName = (ExternalOrgDBAName) null;
      if (currentOrg != null)
        externalOrgDbaName = ExternalOrgManagementAccessor.GetDefaultDBAName(currentOrg.oid);
      fields.Add("TPO.X39", currentOrg != null ? currentOrg.ExternalID : "");
      fields.Add("TPO.X38", currentOrg != null ? currentOrg.OrganizationName : "");
      fields.Add("TPO.X40", currentOrg != null ? currentOrg.OrgID : "");
      fields.Add("TPO.X41", currentOrg != null ? currentOrg.CompanyLegalName : "");
      fields.Add("TPO.X42", currentOrg != null ? currentOrg.Address : "");
      fields.Add("TPO.X43", currentOrg != null ? currentOrg.City : "");
      fields.Add("TPO.X44", currentOrg != null ? currentOrg.State : "");
      fields.Add("TPO.X45", currentOrg != null ? currentOrg.Zip : "");
      fields.Add("TPO.X46", currentOrg != null ? currentOrg.PhoneNumber : "");
      fields.Add("TPO.X47", currentOrg != null ? currentOrg.FaxNumber : "");
      fields.Add("TPO.X48", externalOrgDbaName != null ? externalOrgDbaName.Name : "");
      if (externalOrgSettings != null && externalOrgSettings.ContainsKey("Company Rating") && currentOrg != null)
        fields.Add("TPO.X51", ExternalOrgManagementAccessor.translateRating(currentOrg.CompanyRating, externalOrgSettings["Company Rating"]));
      else
        fields.Add("TPO.X51", "");
      if (currentOrg == null)
        return;
      ExternalUserInfo externalUserInfo1 = ExternalOrgManagementAccessor.GetExternalUserInfo(currentOrg.Manager);
      if ((UserInfo) externalUserInfo1 != (UserInfo) null)
      {
        fields.Add("TPO.X52", externalUserInfo1.FirstName + " " + externalUserInfo1.LastName);
        fields.Add("TPO.X53", externalUserInfo1.Email);
      }
      else
      {
        fields.Add("TPO.X52", "");
        fields.Add("TPO.X53", "");
      }
      UserInfo userById1 = User.GetUserById(currentOrg.PrimarySalesRepUserId);
      if (userById1 != (UserInfo) null)
      {
        fields.Add("TPO.X54", userById1.FirstName + " " + userById1.LastName);
        fields.Add("TPO.X55", userById1.Userid);
      }
      else
      {
        fields.Add("TPO.X54", "");
        fields.Add("TPO.X55", "");
      }
    }

    private static string translateRating(int ratingID, List<ExternalSettingValue> tpoRatings)
    {
      if (tpoRatings == null || tpoRatings.Count == 0 || ratingID == -1)
        return string.Empty;
      ExternalSettingValue externalSettingValue = tpoRatings.Find((Predicate<ExternalSettingValue>) (x => x.settingId == ratingID));
      return externalSettingValue == null ? string.Empty : externalSettingValue.settingValue;
    }

    private static void UpdateTPOLoanAssociate(
      List<string> listOfLoanGuids,
      ExternalUserInfo userInfo,
      RealWorldRoleID realWorldRoleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = string.Join("','", (IEnumerable<string>) listOfLoanGuids);
      dbQueryBuilder.AppendLine("UPDATE [LoanAssociates] SET [Name] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.FullName) + ",[UserID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.ContactID) + ",[Email] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Email) + ",[Phone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Phone) + ",[Fax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Fax) + " WHERE Guid in ('" + str + "') AND RoleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID =" + (object) (int) realWorldRoleID + "))");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateTPOLoanAssociate: Cannot update TPO Loan Officer Loan Associate.\r\n" + ex.Message);
      }
    }

    private static void UpdateTPOLoanAssociateWithBlankData(
      List<string> listOfLoanGuids,
      RealWorldRoleID realWorldRoleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = string.Join("','", (IEnumerable<string>) listOfLoanGuids);
      dbQueryBuilder.AppendLine("UPDATE [LoanAssociates] SET [Name] = NULL,[UserID] = NULL,[Email] = NULL,[Phone] = NULL,[Fax] = NULL WHERE Guid in ('" + str + "') AND RoleID = (SELECT TOP(1) Roles.roleID FROM Roles WHERE Roles.roleID IN (SELECT roleID FROM RolesMapping WHERE RolesMapping.realWorldRoleID =" + (object) (int) realWorldRoleID + "))");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateTPOLoanAssociate: Cannot update TPO Loan Officer Loan Associate.\r\n" + ex.Message);
      }
    }

    private static void PopulateLOUser(Dictionary<string, string> fields, ExternalUserInfo userInfo)
    {
      fields.Add("TPO.X61", userInfo.FirstName + " " + (userInfo.MiddleName != string.Empty ? userInfo.MiddleName + " " : "") + userInfo.LastName + (userInfo.LastName != string.Empty ? " " + userInfo.Suffix : ""));
      fields.Add("TPO.X62", userInfo.ContactID);
      fields.Add("TPO.X63", userInfo.Email);
      fields.Add("TPO.X64", userInfo.DisabledLogin ? "Disabled" : "Enabled");
      fields.Add("TPO.X65", userInfo.Phone);
      fields.Add("TPO.X66", userInfo.Fax);
      fields.Add("TPO.X67", userInfo.CellPhone);
      fields.Add("TPO.X68", userInfo.Address);
      fields.Add("TPO.X69", userInfo.City);
      fields.Add("TPO.X70", userInfo.State);
      fields.Add("TPO.X71", userInfo.Zipcode);
      fields.Add("TPO.X72", userInfo.Notes);
      UserInfo userById = User.GetUserById(userInfo.SalesRepID);
      if (userById != (UserInfo) null)
      {
        fields.Add("TPO.X56", userById.FirstName + " " + userById.LastName);
        fields.Add("TPO.X57", userById.Userid);
      }
      else
      {
        fields.Add("TPO.X56", "");
        fields.Add("TPO.X57", "");
      }
    }

    private static void PopulateLPUser(Dictionary<string, string> fields, ExternalUserInfo userInfo)
    {
      fields.Add("TPO.X74", userInfo.FirstName + " " + (userInfo.MiddleName != string.Empty ? userInfo.MiddleName + " " : "") + userInfo.LastName + (userInfo.LastName != string.Empty ? " " + userInfo.Suffix : ""));
      fields.Add("TPO.X75", userInfo.ContactID);
      fields.Add("TPO.X76", userInfo.Email);
      fields.Add("TPO.X77", userInfo.DisabledLogin ? "Disabled" : "Enabled");
      fields.Add("TPO.X78", userInfo.Phone);
      fields.Add("TPO.X79", userInfo.Fax);
      fields.Add("TPO.X80", userInfo.CellPhone);
      fields.Add("TPO.X81", userInfo.Address);
      fields.Add("TPO.X82", userInfo.City);
      fields.Add("TPO.X83", userInfo.State);
      fields.Add("TPO.X84", userInfo.Zipcode);
      fields.Add("TPO.X85", userInfo.Notes);
      UserInfo userById = User.GetUserById(userInfo.SalesRepID);
      if (userById != (UserInfo) null)
      {
        fields.Add("TPO.X58", userById.FirstName + " " + userById.LastName);
        fields.Add("TPO.X59", userById.Userid);
      }
      else
      {
        fields.Add("TPO.X58", "");
        fields.Add("TPO.X59", "");
      }
    }

    private static void PopulateBlankLPUser(Dictionary<string, string> fields)
    {
      fields.Add("TPO.X74", "");
      fields.Add("TPO.X75", "");
      fields.Add("TPO.X76", "");
      fields.Add("TPO.X77", "");
      fields.Add("TPO.X78", "");
      fields.Add("TPO.X79", "");
      fields.Add("TPO.X80", "");
      fields.Add("TPO.X81", "");
      fields.Add("TPO.X82", "");
      fields.Add("TPO.X83", "");
      fields.Add("TPO.X84", "");
      fields.Add("TPO.X85", "");
      fields.Add("TPO.X58", "");
      fields.Add("TPO.X59", "");
    }

    public static List<List<HierarchySummary>> SearchOrganization(string type, string keyword)
    {
      List<List<HierarchySummary>> hierarchySummaryListList = new List<List<HierarchySummary>>();
      List<HierarchySummary> hierarchySummaryList = new List<HierarchySummary>();
      List<HierarchySummary> source = new List<HierarchySummary>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string idList = "";
      if (type.Equals("Name"))
        dbQueryBuilder.AppendLine("SELECT oid, ExternalID, Parent, OrganizationName,CompanyLegalName,CompanyDBAName, Depth, HierarchyPath FROM [ExternalOriginatorManagement] Where OrganizationName like '%" + EllieMae.EMLite.DataAccess.SQL.Escape(keyword) + "%'");
      else
        dbQueryBuilder.AppendLine("SELECT oid, ExternalID, Parent, OrganizationName,CompanyLegalName,CompanyDBAName, Depth, HierarchyPath FROM [ExternalOriginatorManagement], [ExternalOrgDetail] Where OrganizationID like '%" + EllieMae.EMLite.DataAccess.SQL.Escape(keyword) + "%'  and oid = externalOrgID");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          source.Add(ExternalOrgManagementAccessor.getHierarchySummaryFromDatarow(row));
        source.ForEach((Action<HierarchySummary>) (x =>
        {
          if (idList == "")
            idList = string.Concat((object) x.oid);
          else
            idList = idList + ", " + (object) x.oid;
        }));
        if (source.Any<HierarchySummary>())
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("SELECT a.oid oid, ExternalID, Parent, OrganizationName,CompanyLegalName,CompanyDBAName, Depth, HierarchyPath FROM [ExternalOriginatorManagement] a inner join [ExternalOrgDescendents] b  on a.oid = b.oid where a.parent = 0 and b.descendent in (" + idList + ")");
          foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
            hierarchySummaryList.Add(ExternalOrgManagementAccessor.getHierarchySummaryFromDatarow(row));
        }
        hierarchySummaryListList.Add(hierarchySummaryList);
        hierarchySummaryListList.Add(source);
        return hierarchySummaryListList;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetHierarchy: Cannot fetch HierarchySummary records from [ExternalOriginatorManagement] table.\r\n" + ex.Message);
      }
    }

    public static bool IsMfaEnabledForExternalOrg(int extOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      dbQueryBuilder.AppendLine("SELECT 1 FROM ExternalOrgDetail WHERE ExternalOrgId = @externalOrgId AND MultiFactorAuthentication = 1");
      SqlParameter sqlParameter = new SqlParameter("@externalOrgId", (object) extOrgId);
      try
      {
        using (SqlCommand sqlCmd = new SqlCommand())
        {
          sqlCmd.CommandText = dbQueryBuilder.ToString();
          sqlCmd.Parameters.Add(sqlParameter);
          return dbAccessManager.ExecuteScalar((IDbCommand) sqlCmd) != null;
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("IsMfaEnabled: Cannot fetch record from ExternalOrgDetail table for extOrgId " + (object) extOrgId + ".\r\n" + ex.Message);
      }
    }

    public static List<DocumentSettingInfo> GetExternalDocuments(
      int externalOrgID,
      int channel,
      int status)
    {
      try
      {
        List<DocumentSettingInfo> externalDocuments = new List<DocumentSettingInfo>();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        List<string> guids = new List<string>();
        if (channel == -1 && status == -1)
          dbQueryBuilder.AppendLine("select * from [ExternalDocuments] where IsArchive = 0 ORDER BY [DateAdded] desc");
        else if (channel == -1)
          dbQueryBuilder.AppendLine("select * from [ExternalDocuments] where [status] = " + (object) status + " and IsArchive = 0 ORDER BY [DateAdded] desc");
        else if (status == -1)
          dbQueryBuilder.AppendLine("select * from [ExternalDocuments] where ([channel] = 3 OR [channel] = " + (object) channel + ") and IsArchive = 0 ORDER BY [DateAdded]");
        else
          dbQueryBuilder.AppendLine("select * from [ExternalDocuments] where ([channel] = 3 OR [channel] = " + (object) channel + ") and [status] = " + (object) status + " and IsArchive = 0 ORDER BY [DateAdded] desc");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          guids.Add(Convert.ToString(dataRow["Guid"]));
        Dictionary<string, int> assignedTpOsCount = ExternalOrgManagementAccessor.GetDocAssignedTPOsCount(guids);
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        {
          DocumentSettingInfo externalDocument = ExternalOrgManagementAccessor.dataRowToExternalDocument(r);
          externalDocument.AssignCount = !assignedTpOsCount.ContainsKey(Convert.ToString(r["Guid"])) ? 0 : assignedTpOsCount[Convert.ToString(r["Guid"])];
          externalDocuments.Add(externalDocument);
        }
        return externalDocuments;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalDocuments: Error getting external documents.\r\n" + ex.Message);
      }
    }

    public static List<DocumentSettingInfo> GetExternalDocumentsForOrgAssignment(int externalOrgID)
    {
      try
      {
        List<DocumentSettingInfo> forOrgAssignment = new List<DocumentSettingInfo>();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [ExternalDocuments] where IsArchive = 0 and ActiveChecked = 1 and Guid not in (select Guid from [ExternalOrgDefaultDocuments] where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " )  ORDER BY [DateAdded] desc");
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          DocumentSettingInfo externalDocument = ExternalOrgManagementAccessor.dataRowToExternalDocument(r);
          externalDocument.IsDefault = true;
          forOrgAssignment.Add(externalDocument);
        }
        return forOrgAssignment;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalDocuments: Error getting external documents.\r\n" + ex.Message);
      }
    }

    public static Dictionary<int, List<DocumentSettingInfo>> GetExternalOrgDocuments(
      int externalOrgID,
      int channel,
      int status,
      bool disableGlobalDocs)
    {
      try
      {
        Dictionary<int, List<DocumentSettingInfo>> externalOrgDocuments = new Dictionary<int, List<DocumentSettingInfo>>();
        string text1 = "SELECT ed.Guid,edd.externalOrgID,AddedBy,ed.ActiveChecked as DefaultActive,edd.ActiveChecked,FileName,DisplayName,Category,Channel,DateAdded,StartDate,EndDate,ed.Status as DefaultStatus,AvailbleAllTPO,IsArchive,FileSize,VaultFileId,edd.SortId,edd.Status,1 as IsDefault FROM [ExternalDocuments] ed inner join [ExternalOrgDefaultDocuments] edd on ed.Guid = edd.Guid ";
        string text2 = "SELECT Guid,externalOrgID,AddedBy,0,ActiveChecked,FileName,DisplayName,Category,Channel,DateAdded,StartDate,EndDate,0,0,IsArchive,FileSize,VaultFileId,SortId,Status,0 as IsDefault FROM [ExternalOrgDocuments]";
        string str1 = " WHERE ed.ActiveChecked = 1 AND ed.IsArchive = 0 and edd.externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
        string str2 = " WHERE IsArchive = 0 and externalOrgID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        string text3;
        string text4;
        if (channel == -1 && status == -1)
        {
          text3 = str1 ?? "";
          text4 = str2 ?? "";
        }
        else if (channel == -1)
        {
          text3 = str1 + " AND edd.Status = " + (object) status;
          text4 = str2 + " AND Status = " + (object) status;
        }
        else if (status == -1)
        {
          text3 = str1 + " AND ([channel] = 3 OR [channel] = " + (object) channel + ")";
          text4 = str2 + " AND ([channel] = 3 OR [channel] = " + (object) channel + ")";
        }
        else
        {
          text3 = str1 + " AND ([channel] = 3 OR [channel] = " + (object) channel + ") and edd.Status = " + (object) status;
          text4 = str2 + " AND ([channel] = 3 OR [channel] = " + (object) channel + ") and [Status] = " + (object) status;
        }
        if (!disableGlobalDocs)
        {
          dbQueryBuilder.AppendLine(text1);
          dbQueryBuilder.AppendLine(text3);
          dbQueryBuilder.AppendLine(" UNION ");
        }
        dbQueryBuilder.AppendLine(text2);
        dbQueryBuilder.AppendLine(text4);
        dbQueryBuilder.AppendLine(" ORDER BY [category] desc,[SortId]");
        foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          if (externalOrgDocuments.ContainsKey(EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Category"])))
          {
            externalOrgDocuments[EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Category"])].Add(ExternalOrgManagementAccessor.dataRowToExternalDocument(r));
          }
          else
          {
            externalOrgDocuments[EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Category"])] = new List<DocumentSettingInfo>();
            externalOrgDocuments[EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Category"])].Add(ExternalOrgManagementAccessor.dataRowToExternalDocument(r));
          }
        }
        return externalOrgDocuments;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgDocuments: Error getting external org documents.\r\n" + ex.Message);
      }
    }

    public static List<DocumentSettingInfo> GetAllArchiveDocuments(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<DocumentSettingInfo> archiveDocuments = new List<DocumentSettingInfo>();
      if (externalOrgID == -1)
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalDocuments] where IsArchive = 1 ORDER BY [DateAdded] desc");
      else
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgDocuments] where IsArchive = 1 AND externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " ORDER BY [DateAdded] desc");
      try
      {
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet == null)
          return archiveDocuments;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          DocumentSettingInfo externalDocument = ExternalOrgManagementAccessor.dataRowToExternalDocument(row);
          archiveDocuments.Add(externalDocument);
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetAllArchiveDocuments : Cannot fetch all archive Documents from [ExternalDocuments] or [ExternalOrgDocuments]  table.\r\n" + ex.Message);
      }
      return archiveDocuments;
    }

    public static void UnArchiveDocuments(int externalOrgID, List<string> guids)
    {
      string str = string.Join("','", (IEnumerable<string>) guids);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (externalOrgID == -1)
        dbQueryBuilder.AppendLine("update  [ExternalDocuments] set IsArchive = 0 where Guid in ('" + str + "')");
      else
        dbQueryBuilder.AppendLine("update  [ExternalOrgDocuments] set IsArchive = 0 where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND Guid in ('" + str + "')");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UnArchiveDocuments: Cannot unarchive Documents from [ExternalDocuments] or [ExternalOrgDocuments]  table.\r\n" + ex.Message);
      }
    }

    public static void ArchiveDocuments(int externalOrgID, List<string> guids)
    {
      string str = string.Join("','", (IEnumerable<string>) guids);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (externalOrgID == -1)
        dbQueryBuilder.AppendLine("update  [ExternalDocuments] set IsArchive = 1 where Guid in ('" + str + "')");
      else
        dbQueryBuilder.AppendLine("update  [ExternalOrgDocuments] set IsArchive = 1 where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND Guid in ('" + str + "')");
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("ArchiveDocuments: Cannot archive Documents from [ExternalDocuments] or [ExternalOrgDocuments]  table.\r\n" + ex.Message);
      }
    }

    public static void DeleteDocument(int externalOrgID, Guid guid, FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text;
        if (externalOrgID == -1)
          text = "Delete from ExternalDocuments WHERE Guid = '" + (object) guid + "'";
        else
          text = "Delete from ExternalOrgDocuments WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND Guid = '" + (object) guid + "'";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalDocuments"), entry.GetEncodedPath()), MutexAccess.Read))
        {
          if (!dataFile.Exists)
            return;
          dataFile.Delete();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteDocument: Cannot delete from table ExternalDocuments or ExternalOrgDocuments .\r\n" + ex.Message);
      }
    }

    private static DocumentSettingInfo dataRowToExternalDocument(DataRow r)
    {
      DocumentSettingInfo externalDocument = new DocumentSettingInfo();
      externalDocument.Active = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["ActiveChecked"]);
      externalDocument.AvailbleAllTPO = r.Table.Columns.Contains("AvailbleAllTPO") && EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AvailbleAllTPO"]);
      externalDocument.Category = EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Category"]);
      externalDocument.Channel = (ExternalOriginatorEntityType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Channel"]);
      externalDocument.DateAdded = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["DateAdded"]);
      externalDocument.DisplayName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["DisplayName"]);
      externalDocument.EndDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["EndDate"]);
      externalDocument.AddedBy = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["AddedBy"]);
      externalDocument.FileName = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FileName"]);
      externalDocument.Guid = Guid.Parse(Convert.ToString(r["Guid"]));
      externalDocument.StartDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["StartDate"]);
      externalDocument.Status = (ExternalOriginatorStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["Status"]);
      externalDocument.FileSize = EllieMae.EMLite.DataAccess.SQL.DecodeString(r["FileSize"]);
      object obj = EllieMae.EMLite.DataAccess.SQL.Decode(r["VaultFileId"]);
      externalDocument.VaultFileId = obj == null ? (string) null : (string) obj;
      externalDocument.ExternalOrgId = !r.Table.Columns.Contains("externalOrgID") ? -1 : EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["externalOrgID"]);
      externalDocument.SortId = !r.Table.Columns.Contains("SortId") ? -1 : EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["SortId"]);
      externalDocument.IsDefault = r.Table.Columns.Contains("IsDefault") && EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["IsDefault"]) == 1;
      if (r.Table.Columns.Contains("DefaultStatus"))
      {
        externalDocument.DefaultStatus = (ExternalOriginatorStatus) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["DefaultStatus"]);
        if (externalDocument.IsDefault)
          externalDocument.Status = !externalDocument.Active ? ExternalOriginatorStatus.NotActive : externalDocument.DefaultStatus;
      }
      else
        externalDocument.DefaultStatus = ExternalOriginatorStatus.Active;
      externalDocument.DefaultActive = !r.Table.Columns.Contains("DefaultActive") || EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["DefaultActive"]);
      return externalDocument;
    }

    public static void AddDocument(
      int externalOrgID,
      DocumentSettingInfo document,
      bool isTopOfCategory)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (externalOrgID == -1)
        {
          dbQueryBuilder.AppendLine("INSERT INTO [ExternalDocuments] ([Guid],[AddedBy],[ActiveChecked], [FileName],[DisplayName],[Category],[Channel],[DateAdded],[StartDate],[EndDate],[AvailbleAllTPO],[FileSize],[VaultFileId],[IsArchive]) VALUES (");
          dbQueryBuilder.AppendLine("'" + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Guid) + "', ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.AddedBy) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.Active ? 1 : 0)) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.FileName) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DisplayName) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Category) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) document.Channel) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DateAdded) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.StartDate) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.EndDate) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.AvailbleAllTPO ? 1 : 0)) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.FileSize) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.VaultFileId) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.IsArchive ? 1 : 0)) + ")");
        }
        else
        {
          if (isTopOfCategory)
          {
            ExternalOrgManagementAccessor.MoveDocumentSortOrder(document.ExternalOrgId, document, 1);
            document.SortId = 1;
          }
          else
            document.SortId = ExternalOrgManagementAccessor.GetDocumentMaxSortId(document.ExternalOrgId, document) + 1;
          dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDocuments] ([Guid],[externalOrgID],[AddedBy],[ActiveChecked], [FileName],[DisplayName],[Category],[Channel],[DateAdded],[StartDate],[EndDate],[FileSize],[VaultFileId],[IsArchive],[SortId]) VALUES (");
          dbQueryBuilder.AppendLine("'" + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Guid) + "', ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.AddedBy) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.Active ? 1 : 0)) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.FileName) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DisplayName) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Category) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) document.Channel) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DateAdded) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.StartDate) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.EndDate) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.FileSize) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.VaultFileId) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.IsArchive ? 1 : 0)) + ", ");
          dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.SortId) + ")");
        }
        dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddDocument: Cannot insert in table ExternalDocuments or ExternalOrgDocuments.\r\n" + ex.Message);
      }
    }

    public static void UpdateDocument(int externalOrgID, DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text;
        if (externalOrgID == -1)
          text = "UPDATE ExternalDocuments SET [DisplayName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DisplayName) + ",[Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Category) + ",[Channel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) document.Channel) + ",[StartDate] = " + (document.StartDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) document.StartDate) : (object) "NULL") + ",[EndDate] = " + (document.EndDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) document.EndDate) : (object) "NULL") + ",[VaultFileId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.VaultFileId) + ",[AvailbleAllTPO] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.AvailbleAllTPO ? 1 : 0)) + " WHERE Guid = '" + (object) document.Guid + "'";
        else
          text = "UPDATE ExternalOrgDocuments SET [DisplayName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.DisplayName) + ",[Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Category) + ",[Channel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) document.Channel) + ",[StartDate] = " + (document.StartDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) document.StartDate) : (object) "NULL") + ",[EndDate] = " + (document.EndDate != DateTime.MinValue ? (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) document.EndDate) : (object) "NULL") + ",[VaultFileId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.VaultFileId) + ",[SortId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.SortId) + " WHERE Guid = '" + (object) document.Guid + "'";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateDocument: Cannot update table ExternalDocuments.\r\n" + ex.Message);
      }
    }

    public static void MoveDocumentSortOrder(
      int externalOrgId,
      DocumentSettingInfo document,
      int startSortId)
    {
      if (startSortId == -1)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [ExternalOrgDocuments] set sortid = sortid +1  where externalOrgID =" + (object) externalOrgId + " and [Category] = " + (object) document.Category + "  and sortId >= " + (object) startSortId);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("update a set a.sortid = a.sortid + 1 from [ExternalOrgDefaultDocuments] a join [ExternalDocuments] b on a.guid = b.guid where externalOrgID =" + (object) externalOrgId + " and [Category] = " + (object) document.Category + " and sortid >= " + (object) startSortId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int GetDocumentSortId(int externalOrgId, DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("select sortid from ExternalOrgDefaultDocuments  where externalOrgID =" + (object) externalOrgId + " and guid = '" + (object) document.Guid + "'");
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery();
      int documentSortId;
      if (dataSet1 != null && dataSet1.Tables[0].Rows.Count > 0)
      {
        documentSortId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataSet1.Tables[0].Rows[0]["sortId"]);
      }
      else
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append("select sortid from ExternalOrgDocuments  where externalOrgID =" + (object) externalOrgId + " and [Category] = " + (object) document.Category + " and guid = '" + (object) document.Guid + "'");
        DataSet dataSet2 = dbQueryBuilder.ExecuteSetQuery();
        documentSortId = dataSet2 == null || dataSet2.Tables[0].Rows.Count <= 0 ? 1 : EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataSet2.Tables[0].Rows[0]["sortId"]);
      }
      return documentSortId;
    }

    public static void AssignDocumentsToTposByRelatedDoc(
      List<int> externalOrgIds,
      DocumentSettingInfo document,
      DocumentSettingInfo relatedDocument,
      bool IsTopOfCategoryorDoc)
    {
      foreach (int externalOrgId in externalOrgIds)
      {
        int startSortId;
        if (relatedDocument == null & IsTopOfCategoryorDoc)
        {
          startSortId = 1;
          document.SortId = 1;
        }
        else if (relatedDocument != null & IsTopOfCategoryorDoc)
        {
          int documentSortId = ExternalOrgManagementAccessor.GetDocumentSortId(externalOrgId, relatedDocument);
          startSortId = documentSortId;
          document.SortId = documentSortId;
        }
        else if (relatedDocument != null && !IsTopOfCategoryorDoc)
        {
          int documentSortId = ExternalOrgManagementAccessor.GetDocumentSortId(externalOrgId, relatedDocument);
          startSortId = documentSortId + 1;
          document.SortId = documentSortId + 1;
        }
        else
        {
          startSortId = -1;
          document.SortId = -1;
        }
        ExternalOrgManagementAccessor.MoveDocumentSortOrder(externalOrgId, document, startSortId);
      }
      ExternalOrgManagementAccessor.AssignDocumentToTpos(externalOrgIds, document);
    }

    public static int GetDocumentMaxSortId(int externalOrgid, DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int val1 = 0;
      dbQueryBuilder.Append("Select max(sortid) as maxSortId from [ExternalOrgDefaultDocuments] a join [ExternalDocuments] b on a.guid = b.guid where externalOrgID =" + (object) externalOrgid + " and [Category] = " + (object) document.Category);
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet1 != null && dataSet1.Tables[0].Rows.Count > 0)
        val1 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataSet1.Tables[0].Rows[0]["maxSortId"]);
      dbQueryBuilder.Reset();
      dbQueryBuilder.Append("select max(sortid) as maxSortId from ExternalOrgDocuments  where externalOrgID =" + (object) externalOrgid + " and [Category] = " + (object) document.Category);
      DataSet dataSet2 = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet2 != null && dataSet2.Tables[0].Rows.Count > 0)
        val1 = Math.Max(val1, EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataSet2.Tables[0].Rows[0]["maxSortId"]));
      return val1;
    }

    public static void AssignDocumentToTpos(List<int> externalOrgIds, DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (int externalOrgId in externalOrgIds)
      {
        if (document.SortId == -1)
          document.SortId = ExternalOrgManagementAccessor.GetDocumentMaxSortId(externalOrgId, document) + 1;
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDefaultDocuments] ([Guid],[externalOrgID],[ActiveChecked],[SortId]) VALUES (");
        dbQueryBuilder.AppendLine("'" + (object) document.Guid + "' , ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + ",");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.Active ? 1 : 0)) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.SortId) + ")");
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
    }

    public static void UpdateSortId(
      int newId,
      Guid guid,
      int externalOrgId,
      bool isExternalOrgDocuments)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (isExternalOrgDocuments)
        dbQueryBuilder.AppendLine("update [ExternalOrgDocuments] set sortid = " + (object) newId + " where guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(guid.ToString()) + " and externalOrgID =" + (object) externalOrgId);
      else
        dbQueryBuilder.AppendLine("update [ExternalOrgDefaultDocuments] set sortid = " + (object) newId + " where guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(guid.ToString()) + " and externalOrgID =" + (object) externalOrgId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<int> GetExternalOrgsByDocumentGuid(Guid guid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> orgsByDocumentGuid = new List<int>();
      dbQueryBuilder.AppendLine("Select [externalOrgID] from [ExternalOrgDefaultDocuments]  where [Guid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(guid.ToString()));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return orgsByDocumentGuid;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        orgsByDocumentGuid.Add(EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["externalOrgID"]));
      return orgsByDocumentGuid;
    }

    public static Dictionary<string, bool> GetDocAssignedTPOs(Guid guid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, bool> docAssignedTpOs = new Dictionary<string, bool>();
      dbQueryBuilder.AppendLine("Select edd.externalOrgID,eom.OrganizationName,edd.ActiveChecked from [ExternalOrgDefaultDocuments] edd inner join [ExternalOriginatorManagement] eom  on edd.externalOrgID = eom.oid where [Guid] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(guid.ToString()) + " Order by [OrganizationName]");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return docAssignedTpOs;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        if (!docAssignedTpOs.ContainsKey(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["OrganizationName"])))
          docAssignedTpOs.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["OrganizationName"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["ActiveChecked"]));
      }
      return docAssignedTpOs;
    }

    private static Dictionary<string, int> GetDocAssignedTPOsCount(List<string> guids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = string.Join("','", (IEnumerable<string>) guids);
      Dictionary<string, int> assignedTpOsCount = new Dictionary<string, int>();
      dbQueryBuilder.AppendLine("Select [Guid], Count(*) as DocCount from [ExternalOrgDefaultDocuments] edd inner join [ExternalOriginatorManagement] eom   on edd.externalOrgID = eom.oid  group by [Guid] having [Guid] in ('" + str + "')");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return assignedTpOsCount;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        if (!assignedTpOsCount.ContainsKey(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"])))
          assignedTpOsCount.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["DocCount"]));
      }
      return assignedTpOsCount;
    }

    public static void AssignDefaultDocumentToAll(DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        foreach (ExternalOriginatorManagementData parentOrganization in ExternalOrgManagementAccessor.GetAllExternalParentOrganizations(false))
          ExternalOrgManagementAccessor.MoveDocumentSortOrder(parentOrganization.oid, document, 1);
        string text = "INSERT INTO [ExternalOrgDefaultDocuments] ([Guid],[externalOrgID],[ActiveChecked],[SortId]) SELECT '" + (object) document.Guid + "',[oid]," + EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.Active ? 1 : 0)) + ",1 FROM [ExternalOriginatorManagement] WHERE [Parent] = 0 and oid not in (select [externalOrgID] from [ExternalOrgDefaultDocuments]  where [Guid] ='" + (object) document.Guid + "') ";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignDefaultDocumentToAll: Cannot insert into table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void RemoveDefaultDocumentFromAll(DocumentSettingInfo document)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "DELETE FROM [ExternalOrgDefaultDocuments] WHERE [Guid] = '" + (object) document.Guid + "'";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("RemoveDefaultDocumentFromAll: Cannot delete from table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void RemoveAssignedDocFromTPO(string guid, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "DELETE FROM [ExternalOrgDefaultDocuments] WHERE [Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " AND externalOrgId=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("RemoveAssignedDocFromTPO: Cannot delete from table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void RemoveAssignedDocFromTPOs(string guid, List<int> externalOrgIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "DELETE FROM [ExternalOrgDefaultDocuments] WHERE [Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + " AND externalOrgId IN (" + string.Join(",", externalOrgIds.Select<int, string>((System.Func<int, string>) (e => e.ToString())).ToArray<string>()) + ")";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("RemoveAssignedDocFromTPOs: Cannot delete from table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    private static string AssignAllDefaultDocumentsToTOPOrgQuery()
    {
      return "INSERT INTO [ExternalOrgDefaultDocuments] ([Guid],[externalOrgID],[ActiveChecked],[SortId]) SELECT [Guid],@oid,ActiveChecked,1FROM [ExternalDocuments] WHERE [AvailbleAllTPO] = 1";
    }

    public static void AssignAllDefaultDocumentsToTOPOrg(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.AssignAllDefaultDocumentsToTOPOrgQuery());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignAllDefaultDocumentsToTOPOrg: Cannot insert into table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void AssignDocumentToOrg(DocumentSettingInfo document, bool isTopOfCategory)
    {
      if (isTopOfCategory)
      {
        ExternalOrgManagementAccessor.MoveDocumentSortOrder(document.ExternalOrgId, document, 1);
        document.SortId = 1;
      }
      else
        document.SortId = ExternalOrgManagementAccessor.GetDocumentMaxSortId(document.ExternalOrgId, document) + 1;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDefaultDocuments] ([Guid],[externalOrgID],[ActiveChecked],[SortId]) VALUES (");
        dbQueryBuilder.AppendLine("'" + EllieMae.EMLite.DataAccess.SQL.Encode((object) document.Guid) + "', ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.ExternalOrgId) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (document.Active ? 1 : 0)) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) document.SortId) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AssignDocumentToOrg: Cannot insert into table ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void UpdateActiveStatus(
      int externalOrgID,
      bool activeChecked,
      bool isDefault,
      Guid guid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (externalOrgID == -1)
        {
          string text1 = "UPDATE [ExternalDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE Guid = '" + (object) guid + "'";
          dbQueryBuilder.AppendLine(text1);
          string text2 = "UPDATE [ExternalOrgDefaultDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE Guid = '" + (object) guid + "'";
          dbQueryBuilder.AppendLine(text2);
        }
        else
        {
          string text;
          if (isDefault)
            text = "UPDATE [ExternalOrgDefaultDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE Guid = '" + (object) guid + "' AND externalOrgID=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
          else
            text = "UPDATE [ExternalOrgDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE Guid = '" + (object) guid + "' AND externalOrgID=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
          dbQueryBuilder.AppendLine(text);
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateActiveStatus: Cannot update table ExternalDocuments.\r\n" + ex.Message);
      }
    }

    public static void UpdateActiveStatusAllDocsInCategory(
      int externalOrgID,
      int category,
      bool activeChecked)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (externalOrgID == -1)
          return;
        string text1 = "UPDATE [ExternalOrgDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE Category = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) category) + " AND externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
        dbQueryBuilder1.AppendLine(text1);
        dbQueryBuilder1.ExecuteNonQuery();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        string text2 = "UPDATE [ExternalOrgDefaultDocuments] SET [ActiveChecked] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (activeChecked ? 1 : 0)) + " WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND Guid in (Select Guid from [ExternalDocuments] WHERE Category = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) category) + ")";
        dbQueryBuilder2.AppendLine(text2);
        dbQueryBuilder2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateActiveStatusAllDocsInCategory: Cannot update table ExternalOrgDocuments and ExternalOrgDefaultDocuments.\r\n" + ex.Message);
      }
    }

    public static void UpdateDocumentCategory(int oldCategory, int newCategory)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text1 = "UPDATE ExternalDocuments SET [Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newCategory) + " WHERE [Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oldCategory);
        dbQueryBuilder.AppendLine(text1);
        string text2 = "UPDATE ExternalOrgDocuments SET [Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newCategory) + " WHERE [Category] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oldCategory);
        dbQueryBuilder.AppendLine(text2);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateDocumentCategory: Cannot update table ExternalDocuments.\r\n" + ex.Message);
      }
    }

    public static void SwapDocumentSortIds(
      int externalOrgID,
      DocumentSettingInfo firstDoc,
      DocumentSettingInfo secondDoc)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text1;
        if (secondDoc.IsDefault)
          text1 = "UPDATE ExternalOrgDefaultDocuments SET [SortId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) firstDoc.SortId) + " WHERE externalOrgID= " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND  [Guid] = '" + (object) secondDoc.Guid + "'";
        else
          text1 = "UPDATE ExternalOrgDocuments SET [SortId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) firstDoc.SortId) + " WHERE externalOrgID= " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND  [Guid] = '" + (object) secondDoc.Guid + "'";
        dbQueryBuilder1.AppendLine(text1);
        dbQueryBuilder1.ExecuteNonQuery();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        string text2;
        if (firstDoc.IsDefault)
          text2 = "UPDATE ExternalOrgDefaultDocuments SET [SortId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) secondDoc.SortId) + " WHERE externalOrgID=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND  [Guid] = '" + (object) firstDoc.Guid + "'";
        else
          text2 = "UPDATE ExternalOrgDocuments SET [SortId] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) secondDoc.SortId) + " WHERE externalOrgID=" + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " AND  [Guid] = '" + (object) firstDoc.Guid + "'";
        dbQueryBuilder2.AppendLine(text2);
        dbQueryBuilder2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SwapDocumentSortIds: Cannot update table ExternalOrgDocuments.\r\n" + ex.Message);
      }
    }

    public static DataFile CreateDocumentInDataFolder(FileSystemEntry entry, BinaryObject data)
    {
      if (data == null)
        data = new BinaryObject(new byte[0]);
      string dataFolderPath = ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalDocuments");
      using (DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, entry.GetEncodedPath()), MutexAccess.Read))
      {
        if (dataFile.Exists)
          dataFile.Delete();
      }
      using (DataFile documentInDataFolder = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, entry.GetEncodedPath()), MutexAccess.Read))
      {
        documentInDataFolder.CreateNew(data);
        return documentInDataFolder;
      }
    }

    public static BinaryObject ReadDocumentFromDataFolder(string fileName)
    {
      try
      {
        string dataFolderPath = ClientContext.GetCurrent().Settings.GetDataFolderPath("ExternalDocuments");
        FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + fileName, FileSystemEntry.Types.File, (string) null);
        DataFile dataFile = ExternalOrgManagementAccessor.CheckOut(SystemUtil.CombinePath(dataFolderPath, fileSystemEntry.GetEncodedPath()), MutexAccess.Read);
        return dataFile.Exists ? dataFile.GetData() : throw new Exception("The '" + fileSystemEntry.Name + "' document cannot be found or no longer exists.\r\n");
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception(ex.Message);
      }
    }

    public static XmlDocument BackupTPOData()
    {
      XmlDocument xmlDocument = (XmlDocument) null;
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOrgManagementAccessor.tpoTables.ForEach((Action<string>) (x => sql.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(x))));
      DataSet dataSet = sql.ExecuteSetQuery();
      if (dataSet != null)
      {
        for (int index = 0; index < ExternalOrgManagementAccessor.tpoTables.Count; ++index)
          dataSet.Tables[index].TableName = ExternalOrgManagementAccessor.tpoTables[index];
        ExternalOrgManagementAccessor.addEmptyElementsToDataset(dataSet);
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());
      }
      return xmlDocument;
    }

    private static void addEmptyElementsToDataset(DataSet dataSet)
    {
      foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
      {
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          for (int index = 0; index < row.ItemArray.Length; ++index)
          {
            if (row.ItemArray[index] == DBNull.Value)
              row[index] = !(table.Columns[index].DataType == typeof (DateTime)) ? (table.Columns[index].DataType == typeof (int) || table.Columns[index].DataType == typeof (Decimal) ? (object) -9999 : (!(table.Columns[index].DataType == typeof (bool)) ? (!(table.Columns[index].ColumnName.ToLower() == "password") ? (!(table.Columns[index].ColumnName.ToLower() == "pmlpassword") ? (object) "DBNull" : (object) 50) : (object) 50) : (object) false)) : (object) DateTime.MinValue;
          }
        }
      }
    }

    public static bool RestoreTPOData(string sourceData)
    {
      bool flag = true;
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalOrgManagementAccessor.tpoTables.ForEach((Action<string>) (x => sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(x))));
      sql.ExecuteNonQuery();
      StringReader reader = new StringReader(sourceData);
      DataSet dataSet = new DataSet();
      int num = (int) dataSet.ReadXml((TextReader) reader);
      using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ClientContext.GetCurrent().Settings.ConnectionString, SqlBulkCopyOptions.KeepIdentity))
      {
        foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
        {
          if (ExternalOrgManagementAccessor.tpoTables.Contains(table.TableName))
          {
            foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
            {
              for (int index = 0; index < row.ItemArray.Length; ++index)
              {
                switch (string.Concat(row[index]))
                {
                  case "0001-01-01T00:00:00-08:00":
                    row[index] = (object) DBNull.Value;
                    break;
                  case "-9999":
                  case "DBNull":
                    row[index] = (object) DBNull.Value;
                    break;
                }
                if (table.TableName == "ExternalUsers" && table.Columns[index].ColumnName == "Password")
                  row[index] = (object) DBNull.Value;
              }
            }
            try
            {
              sqlBulkCopy.ColumnMappings.Clear();
              foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
              {
                if (column.ColumnName != "DaysToExpire")
                  sqlBulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
              }
              sqlBulkCopy.DestinationTableName = table.TableName;
              sqlBulkCopy.WriteToServer(table);
            }
            catch (Exception ex)
            {
              ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
            }
          }
        }
        sqlBulkCopy.Close();
      }
      return flag;
    }

    public static void RebuildExternalOrgs()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("exec RebuildExternalOrgs");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExternalOrgManagementAccessor), ex);
      }
    }

    public static List<ExternalFeeManagement> GetFeeManagement(int externalOrgID)
    {
      List<ExternalFeeManagement> feeManagement = new List<ExternalFeeManagement>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOrgFeeManagement where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          feeManagement.Add(ExternalOrgManagementAccessor.dataRowToExternalFeeManagement(r));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetFeeManagement: Cannot fetch all records from ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
      return feeManagement;
    }

    public static List<ExternalFeeManagement> GetFeeManagementListByChannel(
      ExternalOriginatorEntityType channel)
    {
      List<int> values = new List<int>();
      values.Add(3);
      if (channel == ExternalOriginatorEntityType.Both)
        values.AddRange((IEnumerable<int>) new int[2]
        {
          1,
          2
        });
      else
        values.Add((int) channel);
      List<ExternalFeeManagement> managementListByChannel = new List<ExternalFeeManagement>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOrgFeeManagement where [ExternalOrgID] = -1 and [Status] in (1,2) and [Channel] in (" + string.Join<int>(",", (IEnumerable<int>) values) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          managementListByChannel.Add(ExternalOrgManagementAccessor.dataRowToExternalFeeManagement(r));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetFeeManagementListByChannel: Cannot fetch all records from ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
      return managementListByChannel;
    }

    [PgReady]
    public static ExternalLateFeeSettings GetGlobalLateFeeSettings()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgLateFeeSettings] where [ExternalOrgID] = -1");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        try
        {
          return dataRowCollection.Count > 0 ? ExternalOrgManagementAccessor.dataRowToExternalLateFeeSettings(dataRowCollection[0]) : (ExternalLateFeeSettings) null;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetGlobalLateFeeSettings: Cannot fetch all records from ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgLateFeeSettings] where [ExternalOrgID] = -1");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        try
        {
          return dataRowCollection.Count > 0 ? ExternalOrgManagementAccessor.dataRowToExternalLateFeeSettings(dataRowCollection[0]) : (ExternalLateFeeSettings) null;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("GetGlobalLateFeeSettings: Cannot fetch all records from ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
        }
      }
    }

    public static ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      int externalOrgID,
      bool getGlobalIfNotFound)
    {
      if (getGlobalIfNotFound && ExternalOrgManagementAccessor.GetGlobalOrSpecificTPOSetting(externalOrgID) == 0)
        return ExternalOrgManagementAccessor.GetGlobalLateFeeSettings();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgLateFeeSettings] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count > 0)
          return ExternalOrgManagementAccessor.dataRowToExternalLateFeeSettings(dataRowCollection[0]);
        return getGlobalIfNotFound ? ExternalOrgManagementAccessor.GetGlobalLateFeeSettings() : (ExternalLateFeeSettings) null;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgLateFeeSettings: Cannot fetch the ExternalLateFeeSettings records from ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
      }
    }

    public static ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      string externalTPOCompanyID,
      bool getGlobalIfNotFound)
    {
      List<ExternalOriginatorManagementData> organizationByTpoid = ExternalOrgManagementAccessor.GetExternalOrganizationByTPOID(externalTPOCompanyID);
      if (organizationByTpoid != null && organizationByTpoid.Count > 0)
        return ExternalOrgManagementAccessor.GetExternalOrgLateFeeSettings(organizationByTpoid[0].oid, getGlobalIfNotFound);
      return getGlobalIfNotFound ? ExternalOrgManagementAccessor.GetGlobalLateFeeSettings() : (ExternalLateFeeSettings) null;
    }

    public static int GetGlobalOrSpecificTPOSetting(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT GlobalOrSpecificTPO FROM [ExternalOrgDetail] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        return dataRowCollection.Count > 0 ? Convert.ToInt32(dataRowCollection[0]["GlobalOrSpecificTPO"]) : 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetGlobalOrSpecificTPOSetting: Cannot fetch all records from ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    public static void InsertFeeManagementSettings(
      ExternalFeeManagement feeManagement,
      int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertFeeManagementSettingsQuery(feeManagement));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("InsertFeeManagementSettings: Cannot insert record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    private static string InsertFeeManagementSettingsQuery(ExternalFeeManagement feeManagement)
    {
      return "INSERT INTO [ExternalOrgFeeManagement] ([ExternalOrgID],[FeeName],[Description], [Code],[Channel],[StartDate],[EndDate],[Condition],[AdvancedCode],[AdvancedCodeXml],[FeePercent],[FeeAmount],[FeeBasedOn],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]) VALUES ( @oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeName) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Description) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Code) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) feeManagement.Channel) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.StartDate) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.EndDate) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Condition) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.AdvancedCode) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.AdvancedCodeXml) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeePercent) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeAmount) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeBasedOn) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.CreatedBy) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.DateCreated) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.UpdatedBy) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.DateUpdated) + ")";
    }

    public static void UpdateFeeManagementSettings(ExternalFeeManagement feeManagement)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("UPDATE [ExternalOrgFeeManagement] SET ");
        dbQueryBuilder.AppendLine("[FeeName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeName) + ", ");
        dbQueryBuilder.AppendLine("[Description] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Description) + ", ");
        dbQueryBuilder.AppendLine("[Code] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Code) + ", ");
        dbQueryBuilder.AppendLine("[Channel] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) feeManagement.Channel) + ", ");
        dbQueryBuilder.AppendLine("[StartDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.StartDate) + ", ");
        dbQueryBuilder.AppendLine("[EndDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.EndDate) + ", ");
        dbQueryBuilder.AppendLine("[Condition] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.Condition) + ", ");
        dbQueryBuilder.AppendLine("[AdvancedCode] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.AdvancedCode) + ", ");
        dbQueryBuilder.AppendLine("[AdvancedCodeXml] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.AdvancedCodeXml) + ", ");
        dbQueryBuilder.AppendLine("[FeePercent] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeePercent) + ", ");
        dbQueryBuilder.AppendLine("[FeeAmount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeAmount) + ", ");
        dbQueryBuilder.AppendLine("[FeeBasedOn] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeBasedOn) + ", ");
        dbQueryBuilder.AppendLine("[CreatedBy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.CreatedBy) + ", ");
        dbQueryBuilder.AppendLine("[DateCreated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.DateCreated) + ", ");
        dbQueryBuilder.AppendLine("[UpdatedBy] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.UpdatedBy) + ", ");
        dbQueryBuilder.AppendLine("[DateUpdated] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.DateUpdated) + "where [FeeManagementID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) feeManagement.FeeManagementID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateFeeManagementSettings: Cannot update record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    private static string SetDefaultFeeManagementListByChannelQuery(
      ExternalOriginatorEntityType channel)
    {
      List<int> values = new List<int>();
      values.Add(3);
      if (channel == ExternalOriginatorEntityType.Both)
        values.AddRange((IEnumerable<int>) new int[2]
        {
          1,
          2
        });
      else
        values.Add((int) channel);
      return "INSERT INTO [ExternalOrgFeeManagement] ([ExternalOrgID],[FeeName],[Description], [Code],[Channel],[StartDate],[EndDate],[Condition],[AdvancedCode],[AdvancedCodeXml],[FeePercent],[FeeAmount],[FeeBasedOn],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]) Select @oid, [FeeName],[Description], [Code],[Channel],[StartDate],[EndDate],[Condition],[AdvancedCode],[AdvancedCodeXml],[FeePercent],[FeeAmount],[FeeBasedOn],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated] from ExternalOrgFeeManagement where [ExternalOrgID] = -1 and [Status] in (1,2) and [Channel] in (" + string.Join<int>(",", (IEnumerable<int>) values) + ")";
    }

    public static void SetDefaultFeeManagementListByChannel(
      int externalOrgID,
      ExternalOriginatorEntityType channel)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SetDefaultFeeManagementListByChannelQuery(channel));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetDefaultFeeManagementListByChannel: Cannot insert record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    public static void SetSelectedFeeManagementList(
      int externalOrgID,
      List<ExternalFeeManagement> fees)
    {
      List<int> values = new List<int>();
      foreach (ExternalFeeManagement fee in fees)
        values.Add(fee.FeeManagementID);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgFeeManagement] ([ExternalOrgID],[FeeName],[Description], [Code],[Channel],[StartDate],[EndDate],[Condition],[AdvancedCode],[AdvancedCodeXml],[FeePercent],[FeeAmount],[FeeBasedOn],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated]) ");
        dbQueryBuilder.AppendLine("Select " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + ", [FeeName],[Description], [Code],[Channel],[StartDate],[EndDate],[Condition],[AdvancedCode],[AdvancedCodeXml],[FeePercent],[FeeAmount],[FeeBasedOn],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated] from ");
        dbQueryBuilder.AppendLine("ExternalOrgFeeManagement where [ExternalOrgID] = -1 and [FeeManagementID] in (" + string.Join<int>(",", (IEnumerable<int>) values) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetDefaultFeeManagementListByChannel: Cannot insert record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    public static void DeleteFeeManagementSettings(List<int> feeManagementIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM [ExternalOrgFeeManagement] WHERE FeeManagementID in (" + string.Join<int>(",", (IEnumerable<int>) feeManagementIDs) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteFeeManagementSettings: Cannot delete record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    public static void DeleteTPOFeeManagementSettings(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM [ExternalOrgFeeManagement] WHERE externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteTPOFeeManagementSetting: Cannot delete record in ExternalOrgFeeManagement table.\r\n" + ex.Message);
      }
    }

    public static void InsertLateFeeSettings(
      ExternalLateFeeSettings lateFeeSettings,
      int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertLateFeeSettingsQuery(lateFeeSettings));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("InsertLateFeeSettings: Cannot insert record in ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
      }
    }

    private static string InsertLateFeeSettingsQuery(ExternalLateFeeSettings lateFeeSettings)
    {
      return "INSERT INTO [ExternalOrgLateFeeSettings] ([ExternalOrgID],[GracePeriodDays],[GracePeriodCalendar],[GracePeriodStarts],[GracePeriodLaterOf],[OtherDate],[StartOnWeekend],[IncludeDay],[FeeHandledAs],[LateFee],[LateFeeBasedOn],[Amount],[CalculateAs],[MaxLateDays], [DayCleared], [DayClearedOtherDate]) VALUES ( @oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodDays) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodCalendar) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodStarts) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodLaterOf) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.OtherDate) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.StartOnWeekend) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.IncludeDay) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.FeeHandledAs) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFee) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFeeBasedOn) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.Amount) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.CalculateAs) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.MaxLateDays) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayCleared) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayClearedOtherDate) + ")";
    }

    public static void UpdateOrgLateFeeSettings(
      ExternalLateFeeSettings lateFeeSettings,
      int externalOrgID)
    {
      if (ExternalOrgManagementAccessor.GetExternalOrgLateFeeSettings(externalOrgID, false) == null)
      {
        ExternalOrgManagementAccessor.InsertLateFeeSettings(lateFeeSettings, externalOrgID);
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        try
        {
          dbQueryBuilder.AppendLine("UPDATE [ExternalOrgLateFeeSettings] SET ");
          dbQueryBuilder.AppendLine("[GracePeriodDays] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodDays) + ", ");
          dbQueryBuilder.AppendLine("[GracePeriodCalendar] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodCalendar) + ", ");
          dbQueryBuilder.AppendLine("[GracePeriodStarts] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodStarts) + ", ");
          dbQueryBuilder.AppendLine("[GracePeriodLaterOf] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodLaterOf) + ", ");
          dbQueryBuilder.AppendLine("[OtherDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.OtherDate) + ", ");
          dbQueryBuilder.AppendLine("[StartOnWeekend] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.StartOnWeekend) + ", ");
          dbQueryBuilder.AppendLine("[IncludeDay] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.IncludeDay) + ", ");
          dbQueryBuilder.AppendLine("[FeeHandledAs] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.FeeHandledAs) + ", ");
          dbQueryBuilder.AppendLine("[LateFee] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFee) + ", ");
          dbQueryBuilder.AppendLine("[LateFeeBasedOn] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFeeBasedOn) + ", ");
          dbQueryBuilder.AppendLine("[Amount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.Amount) + ", ");
          dbQueryBuilder.AppendLine("[CalculateAs] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.CalculateAs) + ", ");
          dbQueryBuilder.AppendLine("[MaxLateDays] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.MaxLateDays) + ", ");
          dbQueryBuilder.AppendLine("[DayCleared] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayCleared) + ", ");
          dbQueryBuilder.AppendLine("[DayClearedOtherDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayClearedOtherDate) + "where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("UpdateOrgLateFeeSettings: Cannot update record in ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
        }
      }
    }

    public static void UpdateGlobalLateFeeSettings(ExternalLateFeeSettings lateFeeSettings)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("UPDATE [ExternalOrgLateFeeSettings] SET ");
        dbQueryBuilder.AppendLine("[GracePeriodDays] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodDays) + ", ");
        dbQueryBuilder.AppendLine("[GracePeriodCalendar] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodCalendar) + ", ");
        dbQueryBuilder.AppendLine("[GracePeriodStarts] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodStarts) + ", ");
        dbQueryBuilder.AppendLine("[GracePeriodLaterOf] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.GracePeriodLaterOf) + ", ");
        dbQueryBuilder.AppendLine("[OtherDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.OtherDate) + ", ");
        dbQueryBuilder.AppendLine("[StartOnWeekend] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.StartOnWeekend) + ", ");
        dbQueryBuilder.AppendLine("[IncludeDay] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.IncludeDay) + ", ");
        dbQueryBuilder.AppendLine("[FeeHandledAs] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.FeeHandledAs) + ", ");
        dbQueryBuilder.AppendLine("[LateFee] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFee) + ", ");
        dbQueryBuilder.AppendLine("[LateFeeBasedOn] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.LateFeeBasedOn) + ", ");
        dbQueryBuilder.AppendLine("[Amount] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.Amount) + ", ");
        dbQueryBuilder.AppendLine("[CalculateAs] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.CalculateAs) + ", ");
        dbQueryBuilder.AppendLine("[MaxLateDays] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.MaxLateDays) + ", ");
        dbQueryBuilder.AppendLine("[DayCleared] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayCleared) + ", ");
        dbQueryBuilder.AppendLine("[DayClearedOtherDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettings.DayClearedOtherDate) + "where externalOrgID = -1");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateGlobalLateFeeSettings: Cannot update record in ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
      }
    }

    public static void DeleteLateFeeSettings(int lateFeeSettingID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM [ExternalOrgLateFeeSettings] WHERE externalOrgID <> -1 and lateFeeSettingID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lateFeeSettingID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteLateFeeSettings: Cannot delete record in ExternalOrgLateFeeSettings table.\r\n" + ex.Message);
      }
    }

    public static void UpdateGlobalOrSpecificTPOSetting(int externalOrgID, int globalOrSpecificTPO)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("UPDATE [ExternalOrgDetail] SET ");
        dbQueryBuilder.AppendLine("[GlobalOrSpecificTPO] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) globalOrSpecificTPO) + "where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateGlobalOrSpecificTPOSetting: Cannot update record in ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    private static ExternalFeeManagement dataRowToExternalFeeManagement(DataRow r)
    {
      return new ExternalFeeManagement()
      {
        FeeManagementID = Convert.ToInt32(r["FeeManagementID"]),
        ExternalOrgID = Convert.ToInt32(r["ExternalOrgID"]),
        FeeName = Convert.ToString(r["FeeName"]),
        Description = Convert.ToString(r["Description"]),
        Code = Convert.ToString(r["Code"]),
        Channel = (ExternalOriginatorEntityType) Convert.ToInt32(r["Channel"]),
        StartDate = r["StartDate"] != DBNull.Value ? Convert.ToDateTime(r["StartDate"]) : DateTime.MinValue,
        EndDate = r["EndDate"] != DBNull.Value ? Convert.ToDateTime(r["EndDate"]) : DateTime.MinValue,
        Condition = Convert.ToInt32(r["Condition"]),
        AdvancedCode = Convert.ToString(r["AdvancedCode"]),
        AdvancedCodeXml = Convert.ToString(r["AdvancedCodeXml"]),
        FeePercent = r["FeePercent"] != DBNull.Value ? Convert.ToDouble(r["FeePercent"]) : 0.0,
        FeeAmount = r["FeeAmount"] != DBNull.Value ? Convert.ToDouble(r["FeeAmount"]) : 0.0,
        FeeBasedOn = r["FeeBasedOn"] != DBNull.Value ? Convert.ToInt32(r["FeeBasedOn"]) : 0,
        CreatedBy = Convert.ToString(r["CreatedBy"]),
        UpdatedBy = Convert.ToString(r["UpdatedBy"]),
        DateCreated = r["DateCreated"] != DBNull.Value ? Convert.ToDateTime(r["DateCreated"]) : DateTime.MinValue,
        DateUpdated = r["DateUpdated"] != DBNull.Value ? Convert.ToDateTime(r["DateUpdated"]) : DateTime.MinValue,
        Status = (ExternalOriginatorStatus) Convert.ToInt32(r["Status"])
      };
    }

    [PgReady]
    private static ExternalLateFeeSettings dataRowToExternalLateFeeSettings(DataRow r)
    {
      return new ExternalLateFeeSettings()
      {
        LateFeeSettingID = Convert.ToInt32(r["LateFeeSettingID"]),
        ExternalOrgID = Convert.ToInt32(r["ExternalOrgID"]),
        GracePeriodDays = Convert.ToInt32(r["GracePeriodDays"]),
        GracePeriodCalendar = Convert.ToInt32(r["GracePeriodCalendar"]),
        GracePeriodStarts = Convert.ToInt32(r["GracePeriodStarts"]),
        GracePeriodLaterOf = Convert.ToInt32(r["GracePeriodLaterOf"]),
        StartOnWeekend = Convert.ToInt32(r["StartOnWeekend"]),
        IncludeDay = Convert.ToInt32(r["IncludeDay"]),
        OtherDate = Convert.ToString(r["OtherDate"]),
        FeeHandledAs = Convert.ToInt32(r["FeeHandledAs"]),
        LateFee = Convert.ToDouble(r["LateFee"]),
        LateFeeBasedOn = Convert.ToInt32(r["LateFeeBasedOn"]),
        Amount = Convert.ToDouble(r["Amount"]),
        CalculateAs = Convert.ToInt32(r["CalculateAs"]),
        MaxLateDays = Convert.ToInt32(r["MaxLateDays"]),
        DayCleared = r["DayCleared"] != DBNull.Value ? Convert.ToInt32(r["DayCleared"]) : 0,
        DayClearedOtherDate = Convert.ToString(r["DayClearedOtherDate"])
      };
    }

    public static List<ExternalOrgDBAName> GetDBANames(int externalOrgID)
    {
      List<ExternalOrgDBAName> dbaNames = new List<ExternalOrgDBAName>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @oid int; set @oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getDBADetailsQuery());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          dbaNames.Add(ExternalOrgManagementAccessor.dataRowToExternalOrgDBAName(r));
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetDBANames: Cannot fetch all records from ExternalOrgDBANames table.\r\n" + ex.Message);
      }
      return dbaNames;
    }

    public static ExternalOrgDBAName GetDefaultDBAName(int externalOrgID)
    {
      ExternalOrgDBAName externalOrgDbaName = new ExternalOrgDBAName();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalOrgDBANames where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " and [SetAsDefault] = 1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        IEnumerator enumerator = dataRowCollection.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            return ExternalOrgManagementAccessor.dataRowToExternalOrgDBAName((DataRow) enumerator.Current);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetDBANames: Cannot fetch all records from ExternalOrgDBANames table.\r\n" + ex.Message);
      }
      return (ExternalOrgDBAName) null;
    }

    public static bool GetInheritDBANameSetting(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT [InheritDBANames] FROM [ExternalOrgDetail] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        return dataRowCollection.Count > 0 && dataRowCollection[0]["InheritDBANames"] != DBNull.Value && Convert.ToInt32(dataRowCollection[0]["InheritDBANames"]) == 1;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetGlobalOrSpecificTPOSetting: Cannot fetch all records from ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    public static int InsertDBANames(ExternalOrgDBAName name, int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.MaxSortIndexQuery(externalOrgID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int sortIndex = 0;
      if (dataRowCollection.Count > 0)
      {
        DataRow dataRow = dataRowCollection[0];
        if (dataRow["sortindex"] != DBNull.Value)
          sortIndex = Utils.ParseInt((object) Convert.ToInt32(dataRow["sortindex"]));
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.Declare("@DBAID", "int");
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertDbaQuery((IEnumerable<ExternalOrgDBAName>) new List<ExternalOrgDBAName>()
      {
        name
      }, sortIndex, true, orgId: externalOrgID));
      dbQueryBuilder.SelectIdentity("@DBAID");
      dbQueryBuilder.Select("@DBAID");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        int num = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        List<int> intList = new List<int>();
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.ChildExtOrgQuery(externalOrgID));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          intList.Add(Convert.ToInt32(dataRow["ExternalOrgID"]));
        foreach (int externalOrgID1 in intList)
          ExternalOrgManagementAccessor.InsertDBANames(name, externalOrgID1);
        return num;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("InsertDBANames: Cannot insert in ExternalOrgDBANames table.\r\n" + ex.Message);
      }
    }

    public static IEnumerable<ExternalOrgDBAName> UpsertExtOrgDbas(
      int orgId,
      IEnumerable<ExternalOrgDBAName> dbaList,
      bool returnDetails)
    {
      if (orgId <= 0 || dbaList == null || dbaList.Count<ExternalOrgDBAName>() <= 0)
        return Enumerable.Empty<ExternalOrgDBAName>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.AppendLine(string.Format("SET @oid = {0}", (object) orgId));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpsertExtOrgDbasQuery(orgId, dbaList, cmdParams, true));
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        DataSet dataSet = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd);
        List<ExternalOrgDBAName> externalOrgDbaNameList = new List<ExternalOrgDBAName>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          externalOrgDbaNameList.Add(ExternalOrgManagementAccessor.dataRowToExternalOrgDBAName(row, returnDetails));
        return (IEnumerable<ExternalOrgDBAName>) externalOrgDbaNameList;
      }
    }

    public static void ReorderExternalOrgDbas(Dictionary<int, int> dbas, int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SetDBANamesSortIndexQuery(dbas, externalOrgID));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SyncChildOrgDbasWithParentQuery(externalOrgID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static string UpsertExtOrgDbasQuery(
      int orgId,
      IEnumerable<ExternalOrgDBAName> dbaList,
      List<SqlParameter> cmdParams,
      bool returnData = false,
      bool useParentInfo = false)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (!useParentInfo && dbaList != null && dbaList.Any<ExternalOrgDBAName>())
      {
        if (dbaList.Any<ExternalOrgDBAName>((System.Func<ExternalOrgDBAName, bool>) (x => x.SetAsDefault)))
          dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateSetDefaultForExisingDbasQuery(orgId, false));
        DataTable dbaUpsertDataSet = ExternalOrgManagementAccessor.GetDBAUpsertDataSet(orgId, dbaList);
        dbQueryBuilder.AppendLine("EXEC UpsertExtOrgDbas @DBA, @returnExtOrgDbaData");
        cmdParams.Add(new SqlParameter("@DBA", (object) dbaUpsertDataSet)
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_Dba"
        });
        cmdParams.Add(new SqlParameter("@returnExtOrgDbaData", (object) returnData));
      }
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SyncChildOrgDbasWithParentQuery(orgId));
      return dbQueryBuilder.ToString();
    }

    private static DataTable GetDBAUpsertDataSet(int orgId, IEnumerable<ExternalOrgDBAName> dbaList)
    {
      DataTable dbaUpsertDataSet = new DataTable();
      dbaUpsertDataSet.Columns.AddRange(new DataColumn[5]
      {
        new DataColumn("DBAID", typeof (int)),
        new DataColumn("ExternalOrgID", typeof (int)),
        new DataColumn("Name", typeof (string)),
        new DataColumn("SetAsDefault", typeof (int)),
        new DataColumn("SortIndex", typeof (int))
      });
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.MaxSortIndexQuery(orgId));
      int num = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      if (num == -1)
        num = 0;
      foreach (ExternalOrgDBAName dba in dbaList)
      {
        ++num;
        DataRow row = dbaUpsertDataSet.NewRow();
        row["DBAID"] = (object) dba.DBAID;
        row["ExternalOrgID"] = (object) orgId;
        row["Name"] = (object) dba.Name;
        row["SetAsDefault"] = (object) dba.SetAsDefault;
        row["SortIndex"] = (object) num;
        dbaUpsertDataSet.Rows.Add(row);
      }
      return dbaUpsertDataSet;
    }

    public static IEnumerable<ExternalOrgWarehouse> UpsertExtOrgWarehouses(
      int extOrgId,
      IEnumerable<ExternalOrgWarehouse> warehouseList,
      bool returnData)
    {
      if (extOrgId <= 0 || warehouseList != null && !warehouseList.Any<ExternalOrgWarehouse>())
        return Enumerable.Empty<ExternalOrgWarehouse>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<SqlParameter> cmdParams = new List<SqlParameter>();
      EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.SelectVar("@oid", (object) extOrgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpsertExtOrgWarehousesQuery(warehouseList, cmdParams, new bool?(returnData)));
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        sqlCmd.CommandText = dbQueryBuilder.ToString();
        sqlCmd.Parameters.AddRange(cmdParams.ToArray());
        DataTable dtWarehouseDetails = dbAccessManager.ExecuteTableQuery((IDbCommand) sqlCmd);
        return ExternalOrgManagementAccessor.GetWarehouseDetails(returnData, dtWarehouseDetails);
      }
    }

    private static IEnumerable<ExternalOrgWarehouse> GetWarehouseDetails(
      bool returnDetails,
      DataTable dtWarehouseDetails)
    {
      if (dtWarehouseDetails == null || dtWarehouseDetails.Rows.Count <= 0)
        return Enumerable.Empty<ExternalOrgWarehouse>();
      List<ExternalOrgWarehouse> warehouseDetails = new List<ExternalOrgWarehouse>();
      if (returnDetails)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dtWarehouseDetails.Rows)
          warehouseDetails.Add(ExternalOrgManagementAccessor.getExternalWarehouseFromDatarow(row));
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) dtWarehouseDetails.Rows)
          warehouseDetails.Add(new ExternalOrgWarehouse()
          {
            WarehouseID = Convert.ToInt32(row["WarehouseID"])
          });
      }
      return (IEnumerable<ExternalOrgWarehouse>) warehouseDetails;
    }

    private static string UpsertExtOrgWarehousesQuery(
      IEnumerable<ExternalOrgWarehouse> warehouseList,
      List<SqlParameter> cmdParams,
      bool? returnData = null,
      bool syncChildWarehouse = true)
    {
      if ((warehouseList != null ? (!warehouseList.Any<ExternalOrgWarehouse>() ? 1 : 0) : 1) != 0)
        return string.Empty;
      cmdParams.Add(new SqlParameter("@ExtOrgWareHouses", (object) ExternalOrgManagementAccessor.GetWarehouseUpsertDataTable(warehouseList))
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_ExtOrgWareHouse"
      });
      if (returnData.HasValue)
        cmdParams.Add(new SqlParameter("@ReturnExtOrgWareHouseData", (object) returnData.Value));
      else
        cmdParams.Add(new SqlParameter("@ReturnExtOrgWareHouseData", (object) DBNull.Value));
      cmdParams.Add(new SqlParameter("@SyncChildWarehouse", (object) syncChildWarehouse));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("EXEC UpsertExtOrgWareHouses @ExtOrgWareHouses, @oid, @ReturnExtOrgWareHouseData, @SyncChildWarehouse");
      return dbQueryBuilder.ToString();
    }

    private static DataTable GetWarehouseUpsertDataTable(
      IEnumerable<ExternalOrgWarehouse> warehouseList)
    {
      DataTable warehouseUpsertDataTable = new DataTable();
      warehouseUpsertDataTable.Columns.AddRange(new DataColumn[20]
      {
        new DataColumn("WarehouseID", typeof (int)),
        new DataColumn("BankID", typeof (int)),
        new DataColumn("UseBankContact", typeof (int)),
        new DataColumn("WarehouseContactName", typeof (string)),
        new DataColumn("WarehouseContactEmail", typeof (string)),
        new DataColumn("WarehouseContactPhone", typeof (string)),
        new DataColumn("WarehouseContactFax", typeof (string)),
        new DataColumn("Notes", typeof (string)),
        new DataColumn("AcctNumber", typeof (string)),
        new DataColumn("Description", typeof (string)),
        new DataColumn("Expiration", typeof (DateTime)),
        new DataColumn("SelfFunder", typeof (int)),
        new DataColumn("BaileeReq", typeof (int)),
        new DataColumn("TriParty", typeof (int)),
        new DataColumn("Approved", typeof (string)),
        new DataColumn("AcctName", typeof (string)),
        new DataColumn("FurtherCreditAcctName", typeof (string)),
        new DataColumn("FurtherCreditAcctNum", typeof (string)),
        new DataColumn("TimeZone", typeof (string)),
        new DataColumn("StatusDate", typeof (DateTime))
      });
      foreach (ExternalOrgWarehouse warehouse in warehouseList)
      {
        DataRow row = warehouseUpsertDataTable.NewRow();
        row["WarehouseID"] = (object) warehouse.WarehouseID;
        row["BankID"] = (object) warehouse.BankID;
        row["UseBankContact"] = (object) warehouse.UseBankContact;
        row["WarehouseContactName"] = (object) warehouse.ContactName;
        row["WarehouseContactEmail"] = (object) warehouse.ContactEmail;
        row["WarehouseContactPhone"] = (object) warehouse.ContactPhone;
        row["WarehouseContactFax"] = (object) warehouse.ContactFax;
        row["Notes"] = (object) warehouse.Notes;
        row["AcctNumber"] = (object) warehouse.AcctNumber;
        row["Description"] = (object) warehouse.Description;
        row["Expiration"] = !(warehouse.Expiration != DateTime.MinValue) || !(warehouse.Expiration != DateTime.MaxValue) ? (object) DBNull.Value : (object) warehouse.Expiration;
        row["SelfFunder"] = (object) warehouse.SelfFunder;
        row["BaileeReq"] = (object) warehouse.BaileeReq;
        row["TriParty"] = (object) warehouse.TriParty;
        row["Approved"] = (object) warehouse.Approved;
        row["AcctName"] = (object) warehouse.AcctName;
        row["FurtherCreditAcctName"] = (object) warehouse.CreditAcctName;
        row["FurtherCreditAcctNum"] = (object) warehouse.CreditAcctNumber;
        row["TimeZone"] = (object) warehouse.TimeZone;
        row["StatusDate"] = !(warehouse.StatusDate != DateTime.MinValue) || !(warehouse.StatusDate != DateTime.MaxValue) ? (object) DBNull.Value : (object) warehouse.StatusDate;
        warehouseUpsertDataTable.Rows.Add(row);
      }
      return warehouseUpsertDataTable;
    }

    public static void DeleteExtOrgDbas(int orgId, IEnumerable<int> dbaIdList)
    {
      if (dbaIdList == null || dbaIdList.Count<int>() <= 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.DeleteDbaByIdQuery(dbaIdList));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SyncChildOrgDbasWithParentQuery(orgId));
      dbQueryBuilder.ExecuteSetQuery();
    }

    private static string ChildExtOrgQuery(int externalOrgID)
    {
      return "SELECT [externalOrgID] FROM externalOrgDetail WHERE [InheritDBANames] = 1 AND [externalOrgID] IN (SELECT descendent FROM ExternalOrgDescendents WHERE oid = " + (object) externalOrgID + ")";
    }

    private static string ChildExtOrgQuery(
      int externalOrgID,
      ExternalOrgManagementAccessor.ExternalOrganizationEntities entity)
    {
      return "SELECT [externalOrgID], [parent] FROM ExternalOrgDetail, ExternalOriginatorManagement WHERE " + ExternalOrgManagementAccessor.GetDbFieldName(entity) + " = 1 AND [oid] = [externalOrgID] AND [externalOrgID] IN (SELECT descendent from ExternalOrgDescendents where oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + ")";
    }

    private static string GetDbFieldName(
      ExternalOrgManagementAccessor.ExternalOrganizationEntities entity)
    {
      if (entity == ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba)
        return "[InheritDBANames]";
      return entity == ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse ? "[InheritWarehouses]" : string.Empty;
    }

    private static string MaxSortIndexQuery(int externalOrgID)
    {
      return "SELECT max(SortIndex) as sortindex from [ExternalOrgDBANames] where externalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID);
    }

    private static string InsertDbaQuery(
      IEnumerable<ExternalOrgDBAName> dbaList,
      int sortIndex,
      bool isEncompass,
      bool outputDetails = false,
      int orgId = 0)
    {
      if ((dbaList != null ? (!dbaList.Any<ExternalOrgDBAName>() ? 1 : 0) : 1) != 0)
        return string.Empty;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (isEncompass)
      {
        ExternalOrgDBAName externalOrgDbaName = dbaList.ElementAt<ExternalOrgDBAName>(0);
        dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDBANames] ([ExternalOrgID],[Name],[SetAsDefault],[SortIndex]) VALUES (");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) orgId) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgDbaName.Name) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (externalOrgDbaName.SetAsDefault ? 1 : 0)) + ", ");
        dbQueryBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) (sortIndex == 0 ? 0 : sortIndex + 1)) + ")");
      }
      else
      {
        int num = 0;
        List<string> values = new List<string>();
        foreach (ExternalOrgDBAName dba in dbaList)
        {
          if (num % 1000 == 0)
          {
            if (num > 0)
            {
              dbQueryBuilder.AppendLine(string.Join("," + Environment.NewLine, (IEnumerable<string>) values));
              values.Clear();
            }
            dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDBANames] ([ExternalOrgID],[Name],[SetAsDefault],[SortIndex]) " + (outputDetails ? "OUTPUT INSERTED.*" : string.Empty) + " VALUES ");
          }
          values.Add("(@oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) dba.Name) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (dba.SetAsDefault ? 1 : 0)) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) sortIndex) + ")");
          ++sortIndex;
          ++num;
        }
        if (values.Count > 0)
          dbQueryBuilder.AppendLine(string.Join("," + Environment.NewLine, (IEnumerable<string>) values));
      }
      return dbQueryBuilder.ToString();
    }

    private static string UpdateSetDefaultForExisingDbasQuery(int orgId, bool isEncompass)
    {
      return !isEncompass ? "UPDATE [ExternalOrgDBANames] SET [SetAsDefault] = 0 where [ExternalOrgID] = @oid AND [SetAsDefault] = 1" : "UPDATE [ExternalOrgDBANames] SET [SetAsDefault] = 0 where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgId);
    }

    private static string SyncChildOrgDbasWithParentQuery(int orgId)
    {
      if (orgId <= 0)
        return string.Empty;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.ChildExtOrgQuery(orgId, ExternalOrgManagementAccessor.ExternalOrganizationEntities.Dba));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        dictionary.Add(Convert.ToInt32(dataRow["ExternalOrgID"]), Convert.ToInt32(dataRow["parent"]));
      return !dictionary.Any<KeyValuePair<int, int>>() ? string.Empty : ExternalOrgManagementAccessor.InsertDbas(orgId, dictionary);
    }

    private static string InsertDbas(int parentOrgId, Dictionary<int, int> childOrgIds)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<int, int> childOrgId in childOrgIds)
      {
        if (childOrgIds.ContainsKey(childOrgId.Value) || childOrgId.Value == parentOrgId)
          stringBuilder.AppendLine(ExternalOrgManagementAccessor.InsertDbaInChildExtOrgQuery(parentOrgId, childOrgId.Key));
      }
      return stringBuilder.ToString();
    }

    private static string InsertDbasInChildOrgQuery(int orgId, IEnumerable<int> childOrgIdList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (int childOrgId in childOrgIdList)
        stringBuilder.AppendLine(ExternalOrgManagementAccessor.InsertDbaInChildExtOrgQuery(orgId, childOrgId));
      return stringBuilder.ToString();
    }

    private static string InsertDbaInChildExtOrgQuery(int orgId, int childOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM [ExternalOrgDBANames] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) childOrgId));
      dbQueryBuilder.AppendLine("INSERT INTO [ExternalOrgDBANames] ([ExternalOrgID],[Name],[SetAsDefault],[SortIndex]) (");
      dbQueryBuilder.AppendLine("SELECT " + EllieMae.EMLite.DataAccess.SQL.Encode((object) childOrgId) + ", name, setasDefault,sortIndex from  [ExternalOrgDBANames] where externalOrgId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) orgId) + " )");
      return dbQueryBuilder.ToString();
    }

    private static string DeleteDbaByIdQuery(IEnumerable<int> ids)
    {
      return "DELETE FROM [ExternalOrgDBANames] OUTPUT DELETED.DBAID where DBAID in (" + string.Join<int>(",", ids) + ")";
    }

    public static string InsertDBANamesQuery(List<ExternalOrgDBAName> dbaNames)
    {
      string str1 = "";
      foreach (ExternalOrgDBAName dbaName in dbaNames)
      {
        string str2 = "INSERT INTO [ExternalOrgDBANames] ([ExternalOrgID],[Name],[SetAsDefault],[SortIndex]) VALUES (@oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) dbaName.Name) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (dbaName.SetAsDefault ? 1 : 0)) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) dbaName.SortIndex) + ")";
        str1 += str2;
      }
      return str1;
    }

    public static void UpdateDBANames(ExternalOrgDBAName name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (name.DBAID != 0)
        {
          dbQueryBuilder.AppendLine("UPDATE [ExternalOrgDBANames] SET ");
          dbQueryBuilder.AppendLine("[Name] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name.Name) + "where [DBAID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name.DBAID));
        }
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenDBANameSetting(name.ExternalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateDBANames: Cannot update ExternalOrgDBANames table.\r\n" + ex.Message);
      }
    }

    public static void SetDBANameAsDefault(int externalOrgID, int DBAID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateSetDefaultForExisingDbasQuery(externalOrgID, true));
        dbQueryBuilder.AppendLine("UPDATE [ExternalOrgDBANames] SET [SetAsDefault] = 1 where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID) + " and [DBAID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DBAID));
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenDBANameSetting(externalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetDBANameAsDefault: Cannot update ExternalOrgDBANames table.\r\n" + ex.Message);
      }
    }

    public static void SetDBANamesSortIndex(Dictionary<int, int> dbas, int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SetDBANamesSortIndexQuery(dbas, externalOrgID));
        if (dbQueryBuilder.ToString() != "")
          dbQueryBuilder.ExecuteNonQuery();
        else if (dbas.Count == 0)
        {
          dbQueryBuilder.AppendLine("DELETE FROM  [ExternalOrgDBANames] WHERE [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
          dbQueryBuilder.ExecuteNonQuery();
        }
        ExternalOrgManagementAccessor.UpdateAllChildrenDBANameSetting(externalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("SetDBANamesSortIndex: Cannot update ExternalOrgDBANames table.\r\n" + ex.Message);
      }
    }

    public static void DeleteDBANames(List<ExternalOrgDBAName> names)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        List<int> ids = new List<int>();
        foreach (ExternalOrgDBAName name in names)
        {
          if (name.DBAID != 0)
            ids.Add(name.DBAID);
        }
        if (ids.Count == 0)
          return;
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.DeleteDbaByIdQuery((IEnumerable<int>) ids));
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenDBANameSetting(names[0].ExternalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteDBANames: Cannot delete from ExternalOrgDBANames table.\r\n" + ex.Message);
      }
    }

    private static string SetDBANamesSortIndexQuery(Dictionary<int, int> dbas, int externalOrgID)
    {
      if ((dbas != null ? (!dbas.Any<KeyValuePair<int, int>>() ? 1 : 0) : 1) != 0)
        return string.Empty;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ExternalOrgDBANames");
      DbValueList values = new DbValueList();
      DbValueList keys = new DbValueList();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (KeyValuePair<int, int> dba in dbas)
      {
        values.Clear();
        keys.Clear();
        values.Add("SortIndex", (object) dba.Key);
        keys.Add("DBAID", (object) dba.Value);
        keys.Add("ExternalOrgID", (object) externalOrgID);
        dbQueryBuilder.Update(table, values, keys);
      }
      return dbQueryBuilder.ToString();
    }

    private static string UpdateInheritDBANameSettingQuery(bool useParentInfo)
    {
      return "UPDATE [ExternalOrgDetail] SET [InheritDBANames] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (useParentInfo ? 1 : 0)) + " where externalOrgID = @oid";
    }

    private static string InheritDBANameQuery()
    {
      return "DELETE FROM [ExternalOrgDBANames] where [ExternalOrgID] = @oid INSERT INTO [ExternalOrgDBANames] ([ExternalOrgID],[Name],[SetAsDefault],[SortIndex]) (SELECT @oid, name, setasDefault,sortIndex from  [ExternalOrgDBANames] where externalOrgId = (select parent from ExternalOriginatorManagement where oid = @oid ))";
    }

    public static void UpdateInheritDBANameSetting(int externalOrgID, bool useParentInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateInheritDBANameSettingQuery(useParentInfo));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InheritDBANameQuery());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateInheritDBANameSetting: Cannot update record in ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    public static void UpdateAllChildrenDBANameSetting(int parent)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        List<int> childOrgIdList = new List<int>();
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.ChildExtOrgQuery(parent));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          childOrgIdList.Add(Convert.ToInt32(dataRow["ExternalOrgID"]));
        dbQueryBuilder.Reset();
        if (childOrgIdList.Count == 0)
          return;
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertDbasInChildOrgQuery(parent, (IEnumerable<int>) childOrgIdList));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateInheritDBANameSetting: Cannot update record in ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    private static ExternalOrgDBAName dataRowToExternalOrgDBAName(DataRow r, bool populateDetails = true)
    {
      ExternalOrgDBAName externalOrgDbaName = new ExternalOrgDBAName();
      externalOrgDbaName.DBAID = r["DBAID"] != DBNull.Value ? Convert.ToInt32(r["DBAID"]) : -1;
      if (populateDetails)
      {
        externalOrgDbaName.ExternalOrgID = Convert.ToInt32(r["ExternalOrgID"]);
        externalOrgDbaName.Name = Convert.ToString(r["Name"]);
        externalOrgDbaName.SetAsDefault = r["SetAsDefault"] != DBNull.Value && Convert.ToInt32(r["SetAsDefault"]) != 0;
        externalOrgDbaName.SortIndex = r["SortIndex"] != DBNull.Value ? Convert.ToInt32(r["SortIndex"]) : 0;
      }
      return externalOrgDbaName;
    }

    private static ExternalBank getExternalBankFromDatarow(DataRow row)
    {
      return new ExternalBank()
      {
        BankID = Convert.ToInt32(row["BankID"]),
        BankName = Convert.ToString(row["BankName"]),
        Address = Convert.ToString(row["Address"]),
        Address1 = Convert.ToString(row["Address1"]),
        City = Convert.ToString(row["City"]),
        State = Convert.ToString(row["State"]),
        Zip = Convert.ToString(row["Zip"]),
        ContactName = Convert.ToString(row["ContactName"]),
        ContactPhone = Convert.ToString(row["ContactPhone"]),
        ContactEmail = Convert.ToString(row["ContactEmail"]),
        ContactFax = Convert.ToString(row["ContactFax"]),
        ABANumber = Convert.ToString(row["ABANumber"]),
        DateAdded = Convert.ToDateTime(row["DateAdded"]),
        TimeZone = Convert.ToString(row["TimeZone"])
      };
    }

    public static List<HierarchySummary> GetBankHierarchy()
    {
      List<HierarchySummary> bankHierarchy = new List<HierarchySummary>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT BankID, BankName FROM ExternalBanks");
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          bankHierarchy.Add(new HierarchySummary()
          {
            oid = Convert.ToInt32(dataRow["BankID"]),
            OrganizationName = Convert.ToString(dataRow["BankName"])
          });
        return bankHierarchy;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetBankHierarchy: Cannot fetch HierarchySummary records from ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalBank> GetExternalBanks()
    {
      List<ExternalBank> externalBanks = new List<ExternalBank>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalBanks");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalBanks.Add(ExternalOrgManagementAccessor.getExternalBankFromDatarow(row));
        return externalBanks;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalBankByName: Cannot fetch records from ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalBank> GetPaginatedExternalBanks(
      int offset,
      int recordCount,
      out int totalRecords)
    {
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandText = "GetExternalBanks";
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand sqlCmd = sqlCommand;
      SqlParameterCollection parameters1 = sqlCmd.Parameters;
      SqlParameter sqlParameter1 = new SqlParameter();
      sqlParameter1.ParameterName = "StartRecordNumber";
      sqlParameter1.Value = (object) offset;
      parameters1.Add(sqlParameter1);
      SqlParameterCollection parameters2 = sqlCmd.Parameters;
      SqlParameter sqlParameter2 = new SqlParameter();
      sqlParameter2.ParameterName = "RecordCount";
      sqlParameter2.Value = (object) recordCount;
      parameters2.Add(sqlParameter2);
      SqlParameter sqlParameter3 = new SqlParameter();
      sqlParameter3.ParameterName = "TotalNumberOfRecords";
      sqlParameter3.Value = (object) -1;
      sqlParameter3.Direction = ParameterDirection.Output;
      SqlParameter sqlParameter4 = sqlParameter3;
      sqlCmd.Parameters.Add(sqlParameter4);
      DataTable dataTable = new EllieMae.EMLite.Server.DbAccessManager().ExecuteTableQuery((IDbCommand) sqlCmd, DbTransactionType.Default);
      totalRecords = sqlParameter4.Value == DBNull.Value ? -1 : Convert.ToInt32(sqlParameter4.Value);
      if (totalRecords <= 0)
        return new List<ExternalBank>(0);
      List<ExternalBank> paginatedExternalBanks = new List<ExternalBank>(dataTable.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        paginatedExternalBanks.Add(ExternalOrgManagementAccessor.getExternalBankFromDatarow(row));
      return paginatedExternalBanks;
    }

    public static List<ExternalBank> GetSelectedExternalBanks(int[] bankIds)
    {
      List<ExternalBank> selectedExternalBanks = new List<ExternalBank>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalBanks where BankID in (" + string.Join<int>(",", (IEnumerable<int>) bankIds) + ")");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          selectedExternalBanks.Add(ExternalOrgManagementAccessor.getExternalBankFromDatarow(row));
        return selectedExternalBanks;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetSelectedExternalBanks: Cannot fetch records from ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalBank> GetExternalBankByName(string bankName)
    {
      List<ExternalBank> externalBankByName = new List<ExternalBank>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalBanks WHERE [BankName] in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) bankName) + ")");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalBankByName.Add(ExternalOrgManagementAccessor.getExternalBankFromDatarow(row));
        return externalBankByName;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalBankByName: Cannot fetch records from ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static ExternalBank GetExternalBankById(int id)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExternalBank externalBank = new ExternalBank();
      dbQueryBuilder.AppendLine("SELECT * FROM ExternalBanks WHERE [BankID] = (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) id) + ")");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0 ? ExternalOrgManagementAccessor.getExternalBankFromDatarow(dataRowCollection[0]) : (ExternalBank) null;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalBankById: Cannot fetch records from ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static int AddExternalBank(ExternalBank bank)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), "ExternalOrgManagementAccessor.AddExternalBank: Creating SQL commands for table ExternalBanks.");
      dbQueryBuilder.AppendLine("INSERT INTO ExternalBanks (BankName, Address, Address1, City, State, Zip, ContactName, ContactEmail, ContactPhone, ContactFax, ABANumber, DateAdded, TimeZone) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.BankName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Address) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Address1) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.City) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.State) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Zip) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactName) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactEmail) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactPhone) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactFax) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ABANumber) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.TimeZone) + ")");
      dbQueryBuilder.SelectIdentity("@oid");
      dbQueryBuilder.Select("@oid");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalBank: Cannot update ExternalBanks table due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void UpdateExternalBank(int id, ExternalBank bank)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text1 = "UPDATE ExternalBanks SET [BankName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.BankName) + ",[Address] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Address) + ",[Address1] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Address1) + ",[City] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.City) + ",[State] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.State) + ",[Zip] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.Zip) + ",[ContactName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactName) + ",[ContactEmail] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactEmail) + ",[ContactPhone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactPhone) + ",[ContactFax] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ContactFax) + ",[TimeZone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.TimeZone) + ",[ABANumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.ABANumber) + " WHERE BankID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id);
        dbQueryBuilder.AppendLine(text1);
        dbQueryBuilder.ExecuteNonQuery();
        string empty = string.Empty;
        string text2 = "UPDATE ExternalOrgWarehouse SET [TimeZone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) bank.TimeZone) + " WHERE BankID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id);
        dbQueryBuilder.AppendLine(text2);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalBank: Cannot update record in ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static void DeleteExternalBank(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Delete from [ExternalBanks] where [BankID] =  " + (object) oid);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalBank: Cannot delete record in ExternalBanks table.\r\n" + ex.Message);
      }
    }

    public static bool AnyWarehousesUsingThisBank(int oid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("select count(*) from [ExternalOrgWarehouse] where [BankID] =  " + (object) oid);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection != null && dataRowCollection.Count > 0 && Convert.ToInt32(dataRowCollection[0][0]) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AnyWarehousesUsingThisBank: Cannot select record in ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    private static ExternalOrgWarehouse getExternalWarehouseFromDatarow(DataRow row)
    {
      return new ExternalOrgWarehouse()
      {
        WarehouseID = Convert.ToInt32(row["WarehouseID"]),
        ExternalOrgID = Convert.ToInt32(row["ExternalOrgID"]),
        BankID = Convert.ToInt32(row["BankID"]),
        BankName = Convert.ToString(row["BankName"]),
        Address = Convert.ToString(row["Address"]),
        Address1 = Convert.ToString(row["Address1"]),
        City = Convert.ToString(row["City"]),
        State = Convert.ToString(row["State"]),
        Zip = Convert.ToString(row["Zip"]),
        ABANumber = Convert.ToString(row["ABANumber"]),
        DateAdded = Convert.ToDateTime(row["DateAdded"]),
        UseBankContact = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["UseBankContact"]),
        BankContactName = Convert.ToString(row["ContactName"]),
        BankContactPhone = Convert.ToString(row["ContactPhone"]),
        BankContactFax = Convert.ToString(row["ContactFax"]),
        BankContactEmail = Convert.ToString(row["ContactEmail"]),
        ContactName = Convert.ToString(row["WarehouseContactName"]),
        ContactPhone = Convert.ToString(row["WarehouseContactPhone"]),
        ContactFax = Convert.ToString(row["WarehouseContactFax"]),
        ContactEmail = Convert.ToString(row["WarehouseContactEmail"]),
        Notes = Convert.ToString(row["Notes"]),
        AcctNumber = Convert.ToString(row["AcctNumber"]),
        Description = Convert.ToString(row["Description"]),
        Expiration = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["Expiration"]),
        SelfFunder = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["SelfFunder"]),
        BaileeReq = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["BaileeReq"]),
        TriParty = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TriParty"]),
        TimeZone = Convert.ToString(row["TimeZone"]),
        OrgName = Convert.ToString(row["OrganizationName"]),
        OrgType = Convert.ToInt32(row["OrganizationType"]),
        Approved = string.Equals(Convert.ToString(row["Approved"]), "true", StringComparison.CurrentCultureIgnoreCase),
        AcctName = Convert.ToString(row["AcctName"]),
        CreditAcctName = Convert.ToString(row["FurtherCreditAcctName"]),
        CreditAcctNumber = Convert.ToString(row["FurtherCreditAcctNum"]),
        StatusDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(row["StatusDate"])
      };
    }

    public static List<ExternalOrgWarehouse> GetExternalOrgWarehouses(int oid)
    {
      List<ExternalOrgWarehouse> externalOrgWarehouses = new List<ExternalOrgWarehouse>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @oid int");
      dbQueryBuilder.AppendLine("set @oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getWarehousesQuery());
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          externalOrgWarehouses.Add(ExternalOrgManagementAccessor.getExternalWarehouseFromDatarow(row));
        return externalOrgWarehouses;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgWarehouses: Cannot fetch records from ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    public static List<ExternalOrgWarehouse> GetExternalOrgWarehousesbyCompanies(int oid)
    {
      List<ExternalOrgWarehouse> warehousesbyCompanies = new List<ExternalOrgWarehouse>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select orgID into #temp from ExternalOrgAncestors ans inner join ExternalOriginatorManagement mgr on ans.orgID = mgr.oid where Ancestor =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) oid) + " order by HierarchyPath");
      dbQueryBuilder.AppendLine("select * FROM ExternalOrgWarehouse warehouse inner join ExternalBanks bank on warehouse.BankID = bank.BankID inner join ExternalOriginatorManagement mgr on warehouse.ExternalOrgID = mgr.oid inner join ExternalOrgDetail on ExternalOrgDetail.externalOrgID = mgr.oid inner join #temp on mgr.oid = #temp.orgID order by HierarchyPath, BankName");
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          warehousesbyCompanies.Add(ExternalOrgManagementAccessor.getExternalWarehouseFromDatarow(row));
        return warehousesbyCompanies;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgWarehousesbyCompanies: Cannot fetch records from ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    public static string AddExternalOrgWarehousesQuery(ExternalOrgWarehouse warehouse)
    {
      return "INSERT INTO ExternalOrgWarehouse (BankID, ExternalOrgID) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.BankID) + ", @oid )";
    }

    public static ExternalOrgWarehouse AddExternalOrgWarehouse(ExternalOrgWarehouse warehouse)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      List<ExternalOrgWarehouse> externalOrgWarehouseList = new List<ExternalOrgWarehouse>();
      dbQueryBuilder.AppendLine("INSERT INTO ExternalOrgWarehouse (BankID, ExternalOrgID,TimeZone) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.BankID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.ExternalOrgID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.TimeZone) + ")");
      dbQueryBuilder.SelectIdentity("@oid");
      dbQueryBuilder.Select("@oid");
      try
      {
        TraceLog.WriteInfo(nameof (ExternalOrgManagementAccessor), dbQueryBuilder.ToString());
        int num = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        warehouse.WarehouseID = num;
        dbQueryBuilder.Reset();
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        dbQueryBuilder.AppendLine("select [externalOrgID], [parent] from ExternalOrgDetail, ExternalOriginatorManagement where [InheritWarehouses] = 1 and [oid] = [externalOrgID] and [externalOrgID] in (SELECT descendent from ExternalOrgDescendents where oid = " + (object) warehouse.ExternalOrgID + ")");
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          dictionary.Add(Convert.ToInt32(dataRow["ExternalOrgID"]), Convert.ToInt32(dataRow["parent"]));
        dbQueryBuilder.Reset();
        foreach (KeyValuePair<int, int> keyValuePair in dictionary)
        {
          if (dictionary.ContainsKey(keyValuePair.Value) || keyValuePair.Value == warehouse.ExternalOrgID)
            dbQueryBuilder.AppendLine("INSERT INTO ExternalOrgWarehouse (BankID, ExternalOrgID, TimeZone) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.BankID) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) keyValuePair.Key) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.TimeZone) + ")");
        }
        if (!dbQueryBuilder.ToString().Equals(""))
          dbQueryBuilder.ExecuteNonQuery();
        return warehouse;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("AddExternalBank: Cannot Add to ExternalOrgWarehouse table due to the following problem:\r\n" + ex.Message);
      }
    }

    public static string UpdateExternalOrgWarehouseQuery(ExternalOrgWarehouse warehouse)
    {
      string str1 = warehouse.UseBankContact ? EllieMae.EMLite.DataAccess.SQL.Encode((object) "") : EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.ContactName);
      string str2 = warehouse.UseBankContact ? EllieMae.EMLite.DataAccess.SQL.Encode((object) "") : EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.ContactEmail);
      string str3 = warehouse.UseBankContact ? EllieMae.EMLite.DataAccess.SQL.Encode((object) "") : EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.ContactPhone);
      string str4 = warehouse.UseBankContact ? EllieMae.EMLite.DataAccess.SQL.Encode((object) "") : EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.ContactFax);
      return "UPDATE ExternalOrgWarehouse SET [UseBankContact] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(warehouse.UseBankContact) + ",[WarehouseContactName] = " + str1 + ",[WarehouseContactEmail] = " + str2 + ",[WarehouseContactPhone] = " + str3 + ",[WarehouseContactFax] = " + str4 + ",[Notes] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.Notes) + ",[AcctNumber] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.AcctNumber) + ",[Description] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.Description) + ",[Expiration] = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(warehouse.Expiration) + ",[SelfFunder] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.SelfFunder) + ",[BaileeReq] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.BaileeReq) + ",[TriParty] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.TriParty) + ",[TimeZone] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.TimeZone) + ",[AcctName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.AcctName) + ",[FurtherCreditAcctName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.CreditAcctName) + ",[FurtherCreditAcctNum] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.CreditAcctNumber) + ",[Approved] = " + EllieMae.EMLite.DataAccess.SQL.Encode(warehouse.Approved ? (object) "true" : (object) "false") + ",[StatusDate] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) warehouse.StatusDate) + " WHERE WarehouseID = @WarehouseID";
    }

    public static void UpdateExternalOrgWarehouse(int id, ExternalOrgWarehouse warehouse)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@WarehouseID", "int");
        dbQueryBuilder.SelectVar("@WarehouseID", (object) id);
        string text = ExternalOrgManagementAccessor.UpdateExternalOrgWarehouseQuery(warehouse);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenWarehouses(warehouse.ExternalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateExternalOrgWarehouse: Cannot update record in ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    public static void DeleteExternalOrgWarehouse(int id, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Delete from [ExternalOrgWarehouse] where [WarehouseID] =  " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id));
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenWarehouses(externalOrgId);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("DeleteExternalOrgWarehouse: Cannot delete record in ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    public static void UpdateAllChildrenWarehouses(int parent)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        Dictionary<int, int> childOrgIds = new Dictionary<int, int>();
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.ChildExtOrgQuery(parent, ExternalOrgManagementAccessor.ExternalOrganizationEntities.Warehouse));
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          childOrgIds.Add(Convert.ToInt32(dataRow["ExternalOrgID"]), Convert.ToInt32(dataRow[nameof (parent)]));
        dbQueryBuilder.Reset();
        if (childOrgIds.Count == 0)
          return;
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertWarehousesInChildOrgQuery(parent, childOrgIds));
        if (dbQueryBuilder.ToString().Equals(""))
          return;
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateAllChildrenWarehouses: Cannot update record in ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    private static string UpdateInheritWarehousesSettingQuery(bool useParentInfo)
    {
      return "UPDATE [ExternalOrgDetail] SET [InheritWarehouses] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (useParentInfo ? 1 : 0)) + " where externalOrgID = @oid";
    }

    private static string SyncInheritChildWarehouseQuery()
    {
      return "EXEC SyncExtOrgChildOrgWarehouses @oid";
    }

    private static string InheritWarehousesQuery()
    {
      return "DELETE FROM [ExternalOrgWarehouse] where [ExternalOrgID] =  @oid INSERT INTO [ExternalOrgWarehouse] ([ExternalOrgID], [BankID], [UseBankContact], [WarehouseContactName], [WarehouseContactEmail], [WarehouseContactPhone], [WarehouseContactFax], [Notes], [AcctNumber], [Description], [Expiration], [SelfFunder], [BaileeReq], [TriParty], Approved, AcctName, FurtherCreditAcctName, FurtherCreditAcctNum, TimeZone, [StatusDate]) (SELECT @oid, BankID, UseBankContact, WarehouseContactName, WarehouseContactEmail, WarehouseContactPhone, WarehouseContactFax, Notes, AcctNumber, Description, Expiration, SelfFunder, BaileeReq, TriParty, Approved, AcctName, FurtherCreditAcctName, FurtherCreditAcctNum, TimeZone, StatusDate from  [ExternalOrgWarehouse] where externalOrgId = (select parent from ExternalOriginatorManagement where oid = @oid ))";
    }

    private static string SyncInheritChildLoanChannelQuery()
    {
      return "EXEC SyncExtOrgLoanTypesChannel @oid";
    }

    public static void UpdateInheritWarehouses(int externalOrgID, bool useParentInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.UpdateInheritWarehousesSettingQuery(useParentInfo));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgID);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InheritWarehousesQuery());
        dbQueryBuilder.ExecuteNonQuery();
        ExternalOrgManagementAccessor.UpdateAllChildrenWarehouses(externalOrgID);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("UpdateInheritWarehouses: Cannot update record in ExternalOrgWarehouse table.\r\n" + ex.Message);
      }
    }

    public static bool GetInheritWarehouses(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @oid int");
      dbQueryBuilder.AppendLine("set @oid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.getIsInheritWarehouseQuery());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        return dataRowCollection.Count > 0 && dataRowCollection[0]["InheritWarehouses"] != DBNull.Value && Convert.ToInt32(dataRowCollection[0]["InheritWarehouses"]) == 1;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetInheritWarehouses: Cannot fetch all records from ExternalOrgDetail table.\r\n" + ex.Message);
      }
    }

    public static void DeleteExtOrgWarehouses(int orgId, IEnumerable<int> warehouseIdList)
    {
      if (!warehouseIdList.Any<int>())
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@oid", "int");
      dbQueryBuilder.AppendLine("set @oid = " + (object) orgId);
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.DeleteWarehouseByIdQuery(warehouseIdList));
      dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.SyncInheritChildWarehouseQuery());
      dbQueryBuilder.ExecuteSetQuery();
    }

    private static string DeleteWarehouseByIdQuery(IEnumerable<int> ids)
    {
      return "DELETE FROM [ExternalOrgWarehouse] OUTPUT DELETED.WarehouseID where WarehouseID in (" + string.Join<int>(",", ids) + ")";
    }

    private static string InsertWarehousesInChildOrgQuery(
      int parentOrgId,
      Dictionary<int, int> childOrgIds)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<int, int> childOrgId in childOrgIds)
      {
        if (childOrgIds.ContainsKey(childOrgId.Value) || childOrgId.Value == parentOrgId)
        {
          stringBuilder.AppendLine("DELETE FROM [ExternalOrgWarehouse] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) childOrgId.Key));
          stringBuilder.AppendLine("INSERT INTO [ExternalOrgWarehouse] ([ExternalOrgID], [BankID], [UseBankContact], [WarehouseContactName], [WarehouseContactEmail], [WarehouseContactPhone], [WarehouseContactFax], [Notes], [AcctNumber], [Description], [Expiration], [SelfFunder], [BaileeReq], [TriParty], [TimeZone], [Approved], [AcctName], [FurtherCreditAcctName], [FurtherCreditAcctNum], [StatusDate]) (");
          stringBuilder.AppendLine("SELECT " + (object) childOrgId.Key + ", BankID, UseBankContact, WarehouseContactName, WarehouseContactEmail, WarehouseContactPhone, WarehouseContactFax, Notes, AcctNumber, Description, Expiration, SelfFunder, BaileeReq, TriParty, TimeZone, Approved, AcctName, FurtherCreditAcctName, FurtherCreditAcctNum, [StatusDate] from  [ExternalOrgWarehouse] where externalOrgId = " + (object) parentOrgId + " )");
        }
      }
      return stringBuilder.ToString();
    }

    private static ExternalOriginatorManagementData getExternalCommitmentsFromDatarow(DataRow row)
    {
      ExternalOriginatorManagementData commitmentsFromDatarow = new ExternalOriginatorManagementData();
      commitmentsFromDatarow.CommitmentUseBestEffort = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentBestEffort"]);
      commitmentsFromDatarow.CommitmentUseBestEffortLimited = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentBestEffortLimited"]);
      commitmentsFromDatarow.MaxCommitmentAuthority = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["CommitmentMaxAuthority"]);
      commitmentsFromDatarow.CommitmentMandatory = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentMandatory"]);
      commitmentsFromDatarow.MaxCommitmentAmount = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(row["CommitmentMaxAmount"]);
      commitmentsFromDatarow.IsCommitmentDeliveryIndividual = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryIndividual"]);
      commitmentsFromDatarow.IsCommitmentDeliveryBulk = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryBulk"]);
      commitmentsFromDatarow.IsCommitmentDeliveryAOT = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryAOT"]);
      commitmentsFromDatarow.IsCommitmentDeliveryBulkAOT = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryBulkAOT"]);
      commitmentsFromDatarow.IsCommitmentDeliveryLiveTrade = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryLiveTrade"]);
      commitmentsFromDatarow.IsCommitmentDeliveryCoIssue = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryCoIssue"]);
      commitmentsFromDatarow.IsCommitmentDeliveryForward = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(row["CommitmentDeliveryForward"]);
      commitmentsFromDatarow.CommitmentPolicy = (ExternalOriginatorCommitmentPolicy) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CommitmentExceedPolicy"]);
      if (row.Table.Columns.Contains("CommitmentExceedTradePolicy") && row["CommitmentExceedTradePolicy"] != DBNull.Value)
        commitmentsFromDatarow.CommitmentTradePolicy = (ExternalOriginatorCommitmentTradePolicy) EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["CommitmentExceedTradePolicy"]);
      if (row["CommitmentMessage"] != DBNull.Value)
        commitmentsFromDatarow.CommitmentMessage = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["CommitmentMessage"]);
      commitmentsFromDatarow.BestEffortDailyVolumeLimit = !row.Table.Columns.Contains("BestEffortDailyVolumeLimit") || row["BestEffortDailyVolumeLimit"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortDailyVolumeLimit"]);
      commitmentsFromDatarow.BestEffortTolerencePolicy = !row.Table.Columns.Contains("BestEffortTolerencePolicy") || row["BestEffortTolerencePolicy"] == DBNull.Value ? ExternalOriginatorCommitmentTolerancePolicy.NoPolicy : (ExternalOriginatorCommitmentTolerancePolicy) Convert.ToInt32(row["BestEffortTolerencePolicy"]);
      commitmentsFromDatarow.BestEffortTolerancePct = !row.Table.Columns.Contains("BestEffortTolerancePct") || row["BestEffortTolerancePct"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortTolerancePct"]);
      commitmentsFromDatarow.BestEffortToleranceAmt = !row.Table.Columns.Contains("BestEffortToleranceAmt") || row["BestEffortToleranceAmt"] == DBNull.Value ? 0M : Convert.ToDecimal(row["BestEffortToleranceAmt"]);
      commitmentsFromDatarow.BestEfforDailyLimitPolicy = !row.Table.Columns.Contains("BestEfforDailyLimitPolicy") || row["BestEfforDailyLimitPolicy"] == DBNull.Value ? ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock : (ExternalOriginatorBestEffortDailyLimitPolicy) Convert.ToInt32(row["BestEfforDailyLimitPolicy"]);
      commitmentsFromDatarow.DailyLimitWarningMsg = !row.Table.Columns.Contains("DailyLimitWarningMsg") || row["DailyLimitWarningMsg"] == DBNull.Value ? (string) null : Convert.ToString(row["DailyLimitWarningMsg"]);
      commitmentsFromDatarow.MandatoryTolerencePolicy = !row.Table.Columns.Contains("MandatoryTolerencePolicy") || row["MandatoryTolerencePolicy"] == DBNull.Value ? ExternalOriginatorCommitmentTolerancePolicy.NoPolicy : (ExternalOriginatorCommitmentTolerancePolicy) Convert.ToInt32(row["MandatoryTolerencePolicy"]);
      if (row.Table.Columns.Contains("MandatoryTolerancePct") && row["MandatoryTolerancePct"] != DBNull.Value)
        commitmentsFromDatarow.MandatoryTolerancePct = Convert.ToDecimal(row["MandatoryTolerancePct"]);
      else
        commitmentsFromDatarow.BestEffortTolerancePct = 0M;
      commitmentsFromDatarow.MandatoryToleranceAmt = !row.Table.Columns.Contains("MandatoryToleranceAmt") || row["MandatoryToleranceAmt"] == DBNull.Value ? 0M : Convert.ToDecimal(row["MandatoryToleranceAmt"]);
      if (row.Table.Columns.Contains("ResetLimitForRatesheetId") && row["ResetLimitForRatesheetId"] != DBNull.Value)
        commitmentsFromDatarow.ResetLimitForRatesheetId = Convert.ToBoolean(row["ResetLimitForRatesheetId"]);
      return commitmentsFromDatarow;
    }

    public static Dictionary<CorrespondentMasterDeliveryType, Decimal> GetNonAllocatedOutstandingCommitments(
      string externalID)
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT [LoanAmount], [DeliveryType] FROM LoanSummaryExtension where [TPOCompanyID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalID) + "  and [CorrespondentTradeId] is null and deliveryType is not null and not (receivedDate is not null and rejectedDate is null)");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      try
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          CorrespondentMasterDeliveryType key = (CorrespondentMasterDeliveryType) Enum.Parse(typeof (CorrespondentMasterDeliveryType), string.Concat(dataRow["DeliveryType"]));
          if (outstandingCommitments.ContainsKey(key))
            outstandingCommitments[key] += Convert.ToDecimal(dataRow["LoanAmount"]);
          else
            outstandingCommitments.Add(key, Convert.ToDecimal(dataRow["LoanAmount"]));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetNonAllocatedOutstandingCommitments: Cannot fetch all records from LoanSummary table.\r\n" + ex.Message);
      }
      return outstandingCommitments;
    }

    public static bool IsInOutstandingCommitments(string loanGuid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM LoanSummaryExtension where [GUID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid) + " and deliveryType is not null");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count > 0;
    }

    public static Dictionary<CorrespondentTradeCommitmentType, Decimal> GetCommitmentAvailableAmounts(
      ExternalOriginatorManagementData orgData,
      ICorrespondentTradeManager correspondentTradeManager)
    {
      Dictionary<CorrespondentTradeCommitmentType, Decimal> availableAmounts = new Dictionary<CorrespondentTradeCommitmentType, Decimal>();
      Dictionary<CorrespondentMasterDeliveryType, Decimal> dictionary = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments = correspondentTradeManager.GetOutStandingCommitments(orgData.oid);
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments = ExternalOrgManagementAccessor.GetNonAllocatedOutstandingCommitments(orgData.ExternalID);
      foreach (CorrespondentMasterDeliveryType key in (CorrespondentMasterDeliveryType[]) Enum.GetValues(typeof (CorrespondentMasterDeliveryType)))
      {
        Decimal num = 0M;
        if (standingCommitments.ContainsKey(key))
          num += standingCommitments[key];
        if (outstandingCommitments.ContainsKey(key))
          num += outstandingCommitments[key];
        if (dictionary.ContainsKey(key))
          dictionary[key] = num;
        else
          dictionary.Add(key, num);
      }
      if (orgData.CommitmentUseBestEffort)
      {
        if (!availableAmounts.ContainsKey(CorrespondentTradeCommitmentType.BestEfforts))
          availableAmounts.Add(CorrespondentTradeCommitmentType.BestEfforts, 0M);
        if (!orgData.CommitmentUseBestEffortLimited)
          availableAmounts[CorrespondentTradeCommitmentType.BestEfforts] = Decimal.MaxValue;
        else if (dictionary.ContainsKey(CorrespondentMasterDeliveryType.IndividualBestEfforts))
          availableAmounts[CorrespondentTradeCommitmentType.BestEfforts] = orgData.MaxCommitmentAuthority - dictionary[CorrespondentMasterDeliveryType.IndividualBestEfforts];
      }
      if (!orgData.CommitmentMandatory)
        return availableAmounts;
      if (!availableAmounts.ContainsKey(CorrespondentTradeCommitmentType.Mandatory))
        availableAmounts.Add(CorrespondentTradeCommitmentType.Mandatory, 0M);
      Decimal num1 = 0M;
      foreach (KeyValuePair<CorrespondentMasterDeliveryType, Decimal> keyValuePair in dictionary)
      {
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.IndividualMandatory && orgData.IsCommitmentDeliveryIndividual)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.AOT && orgData.IsCommitmentDeliveryAOT)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.Bulk && orgData.IsCommitmentDeliveryBulk)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.BulkAOT && orgData.IsCommitmentDeliveryBulkAOT)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.CoIssue && orgData.IsCommitmentDeliveryCoIssue)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.Forwards && orgData.IsCommitmentDeliveryForward)
          num1 += keyValuePair.Value;
        if (keyValuePair.Key == CorrespondentMasterDeliveryType.LiveTrade && orgData.IsCommitmentDeliveryLiveTrade)
          num1 += keyValuePair.Value;
      }
      availableAmounts[CorrespondentTradeCommitmentType.Mandatory] = orgData.MaxCommitmentAmount - num1;
      return availableAmounts;
    }

    public static ExternalOrgOnrpSettings GetExternalOrgOnrpSettings(int externalOrgID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgONRP] where [ExternalOrgID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgID));
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count > 0 ? ExternalOrgManagementAccessor.dataRowToExternalOrgOnrpSettings(dataRowCollection[0]) : new ExternalOrgOnrpSettings();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgOnrpSettings: Cannot fetch the ExternalOrgOnrpSettings records from ExternalOrgONRP table.\r\n" + ex.Message);
      }
    }

    public static ExternalOrgOnrpSettings GetExternalOrgOnrpSettingsByTPOId(string tpoId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [ExternalOrgONRP] orgONRP inner join ExternalOriginatorManagement org on orgONRP.ExternalOrgID = org.oid ");
      dbQueryBuilder.AppendLine("where org.ExternalID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(tpoId) + " order by org.HierarchyPath, org.Depth");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection.Count > 0 ? ExternalOrgManagementAccessor.dataRowToExternalOrgOnrpSettings(dataRowCollection[0]) : new ExternalOrgOnrpSettings();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("GetExternalOrgOnrpSettingsByTPOId: Cannot fetch the ExternalOrgOnrpSettings records from ExternalOrgONRP table.\r\n" + ex.Message);
      }
    }

    public static void InsertOnrpSettings(ExternalOrgOnrpSettings onrpSettings, int externalOrgId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.Declare("@oid", "int");
        dbQueryBuilder.SelectVar("@oid", (object) externalOrgId);
        dbQueryBuilder.AppendLine(ExternalOrgManagementAccessor.InsertOnrpSettingsQuery(onrpSettings));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
        throw new Exception("InsertOnrpSettings: Cannot insert record in ExternalOrgONRP table.\r\n" + ex.Message);
      }
    }

    public static string InsertOnrpSettingsQuery(ExternalOrgOnrpSettings onrpSettings)
    {
      return "INSERT INTO [ExternalOrgONRP] ([ExternalOrgID], [BrokerDollarLimit], [BrokerEnabled], [BrokerEndTime], [BrokerNoLimit],[BrokerSpecifyTimes], [BrokerTolerance], [BrokerUseChannelDefault], [BrokerWHCoverage],[CorrespondentDollarLimit], [CorrespondentONRPCancelledExpiredLocks], [CorrespondentEnabled], [CorrespondentEndTime], [CorrespondentNoLimit], [CorrespondentSpecifyTimes],[CorrespondentTolerance], [CorrespondentUseChannelDefault], [CorrespondentWHCoverage], [BrokerSatEnabled], [BrokerSunEnabled], [BrokerSatEndTime], [BrokerSunEndTime],[CorrespondentSatEnabled], [CorrespondentSunEnabled], [CorrespondentSatEndTime], [CorrespondentSunEndTime]) VALUES (@oid, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Broker.DollarLimit) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPEndTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.MaximumLimit) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(!onrpSettings.Broker.ContinuousCoverage) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Broker.Tolerance) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.UseChannelDefault) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.WeekendHolidayCoverage) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Correspondent.DollarLimit) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.AllowONRPForCancelledExpiredLocks) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPEndTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.MaximumLimit) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(!onrpSettings.Correspondent.ContinuousCoverage) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Correspondent.Tolerance) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.UseChannelDefault) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.WeekendHolidayCoverage) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableSatONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableSunONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPSatEndTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPSunEndTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableSatONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableSunONRP) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPSatEndTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPSunEndTime) + ")";
    }

    public static void UpdateOnrpSettings(
      ExternalOrgOnrpSettings onrpSettings,
      int externalOrgId,
      ExternalOrgOnrpSettings onrpSettingsOld)
    {
      if (onrpSettings.ONRPID < 0)
      {
        ExternalOrgManagementAccessor.InsertOnrpSettings(onrpSettings, externalOrgId);
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        try
        {
          dbQueryBuilder.AppendLine("UPDATE [ExternalOrgONRP] SET ");
          dbQueryBuilder.AppendLine("[BrokerEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableONRP) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableONRP) + ",");
          dbQueryBuilder.AppendLine("[BrokerDollarLimit] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Broker.DollarLimit) + ", ");
          dbQueryBuilder.AppendLine("[BrokerEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPEndTime) + ", ");
          dbQueryBuilder.AppendLine("[BrokerNoLimit] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.MaximumLimit) + ", ");
          dbQueryBuilder.AppendLine("[BrokerSpecifyTimes] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(!onrpSettings.Broker.ContinuousCoverage) + ", ");
          dbQueryBuilder.AppendLine("[BrokerTolerance] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Broker.Tolerance) + ", ");
          dbQueryBuilder.AppendLine("[BrokerUseChannelDefault] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.UseChannelDefault) + ", ");
          dbQueryBuilder.AppendLine("[BrokerSatEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableSatONRP) + ", ");
          dbQueryBuilder.AppendLine("[BrokerSunEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.EnableSunONRP) + ", ");
          dbQueryBuilder.AppendLine("[BrokerWHCoverage] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Broker.WeekendHolidayCoverage) + ", ");
          dbQueryBuilder.AppendLine("[BrokerSatEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPSatEndTime) + ", ");
          dbQueryBuilder.AppendLine("[BrokerSunEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Broker.ONRPSunEndTime) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentONRPCancelledExpiredLocks] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.AllowONRPForCancelledExpiredLocks) + ",");
          dbQueryBuilder.AppendLine("[CorrespondentDollarLimit] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Correspondent.DollarLimit) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPEndTime) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentNoLimit] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.MaximumLimit) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentSpecifyTimes] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(!onrpSettings.Correspondent.ContinuousCoverage) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentTolerance] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) onrpSettings.Correspondent.Tolerance) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentUseChannelDefault] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.UseChannelDefault) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentSatEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableSatONRP) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentSunEnabled] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.EnableSunONRP) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentWHCoverage] = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(onrpSettings.Correspondent.WeekendHolidayCoverage) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentSatEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPSatEndTime) + ", ");
          dbQueryBuilder.AppendLine("[CorrespondentSunEndTime] = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(onrpSettings.Correspondent.ONRPSunEndTime));
          dbQueryBuilder.AppendLine(" where ExternalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId));
          bool flag1 = ExternalOrgManagementAccessor.HasONRPUpdate(onrpSettings, onrpSettingsOld, true);
          bool flag2 = ExternalOrgManagementAccessor.HasONRPUpdate(onrpSettings, onrpSettingsOld, false);
          if (flag1 & flag2)
          {
            dbQueryBuilder.AppendLine("DELETE FROM OnrpPeriodAccruedAmount ");
            dbQueryBuilder.AppendLine("WHERE ExternalCompanyId = ");
            dbQueryBuilder.AppendLine("(SELECT top 1 externalID FROM ExternalOriginatorManagement c INNER join ExternalOrgONRP o ON c.oid = o.ExternalOrgID ");
            dbQueryBuilder.AppendLine("WHERE o.ExternalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + ")");
          }
          else if (flag1)
          {
            dbQueryBuilder.AppendLine("DELETE FROM OnrpPeriodAccruedAmount ");
            dbQueryBuilder.AppendLine("WHERE ExternalCompanyId = ");
            dbQueryBuilder.AppendLine("(SELECT top 1 externalID FROM ExternalOriginatorManagement c INNER join ExternalOrgONRP o ON c.oid = o.ExternalOrgID ");
            dbQueryBuilder.AppendLine("WHERE o.ExternalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + ") ");
            dbQueryBuilder.AppendLine("AND EntityChannel = '2' ");
          }
          else if (flag2)
          {
            dbQueryBuilder.AppendLine("DELETE FROM OnrpPeriodAccruedAmount ");
            dbQueryBuilder.AppendLine("WHERE ExternalCompanyId = ");
            dbQueryBuilder.AppendLine("(SELECT top 1 externalID FROM ExternalOriginatorManagement c INNER join ExternalOrgONRP o ON c.oid = o.ExternalOrgID ");
            dbQueryBuilder.AppendLine("WHERE o.ExternalOrgID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) externalOrgId) + ") ");
            dbQueryBuilder.AppendLine("AND EntityChannel = '4' ");
          }
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (ExternalOrgManagementAccessor), ex);
          throw new Exception("UpdateOnrpSettings: Cannot update record in ExternalOrgONRP table.\r\n" + ex.Message);
        }
      }
    }

    private static bool HasONRPUpdate(
      ExternalOrgOnrpSettings newOnrp,
      ExternalOrgOnrpSettings oldOnrp,
      bool isWholesale)
    {
      return isWholesale ? oldOnrp == null || newOnrp.Broker.DollarLimit != oldOnrp.Broker.DollarLimit || newOnrp.Broker.EnableONRP != oldOnrp.Broker.EnableONRP || newOnrp.Broker.ONRPEndTime != oldOnrp.Broker.ONRPEndTime || newOnrp.Broker.MaximumLimit != oldOnrp.Broker.MaximumLimit || newOnrp.Broker.ContinuousCoverage != oldOnrp.Broker.ContinuousCoverage || newOnrp.Broker.Tolerance != oldOnrp.Broker.Tolerance || newOnrp.Broker.WeekendHolidayCoverage != oldOnrp.Broker.WeekendHolidayCoverage || newOnrp.Broker.EnableSatONRP != oldOnrp.Broker.EnableSatONRP || newOnrp.Broker.EnableSunONRP != oldOnrp.Broker.EnableSunONRP || newOnrp.Broker.ONRPSatEndTime != oldOnrp.Broker.ONRPSatEndTime || newOnrp.Broker.ONRPSunEndTime != oldOnrp.Broker.ONRPSunEndTime : oldOnrp == null || newOnrp.Correspondent.DollarLimit != oldOnrp.Correspondent.DollarLimit || newOnrp.Correspondent.EnableONRP != oldOnrp.Correspondent.EnableONRP || newOnrp.Correspondent.ONRPEndTime != oldOnrp.Correspondent.ONRPEndTime || newOnrp.Correspondent.MaximumLimit != oldOnrp.Correspondent.MaximumLimit || newOnrp.Correspondent.ContinuousCoverage != oldOnrp.Correspondent.ContinuousCoverage || newOnrp.Correspondent.Tolerance != oldOnrp.Correspondent.Tolerance || newOnrp.Correspondent.WeekendHolidayCoverage != oldOnrp.Correspondent.WeekendHolidayCoverage || newOnrp.Correspondent.EnableSatONRP != oldOnrp.Correspondent.EnableSatONRP || newOnrp.Correspondent.EnableSunONRP != oldOnrp.Correspondent.EnableSunONRP || newOnrp.Correspondent.ONRPSatEndTime != oldOnrp.Correspondent.ONRPSatEndTime || newOnrp.Correspondent.ONRPSunEndTime != oldOnrp.Correspondent.ONRPSunEndTime;
    }

    private static ExternalOrgOnrpSettings dataRowToExternalOrgOnrpSettings(DataRow dr)
    {
      return new ExternalOrgOnrpSettings()
      {
        Broker = {
          DollarLimit = Utils.ParseDouble(dr["BrokerDollarLimit"]),
          EnableONRP = Utils.ParseBoolean(dr["BrokerEnabled"]),
          ONRPEndTime = dr["BrokerEndTime"].ToString(),
          MaximumLimit = Utils.ParseBoolean(dr["BrokerNoLimit"]),
          ContinuousCoverage = !Utils.ParseBoolean(dr["BrokerSpecifyTimes"]),
          Tolerance = Utils.ParseInt(dr["BrokerTolerance"]),
          UseChannelDefault = Utils.ParseBoolean(dr["BrokerUseChannelDefault"]),
          WeekendHolidayCoverage = Utils.ParseBoolean(dr["BrokerWHCoverage"]),
          EnableSatONRP = Utils.ParseBoolean(dr["BrokerSatEnabled"]),
          EnableSunONRP = Utils.ParseBoolean(dr["BrokerSunEnabled"]),
          ONRPSatEndTime = dr["BrokerSatEndTime"].ToString(),
          ONRPSunEndTime = dr["BrokerSunEndTime"].ToString()
        },
        Correspondent = {
          DollarLimit = Utils.ParseDouble(dr["CorrespondentDollarLimit"]),
          EnableONRP = Utils.ParseBoolean(dr["CorrespondentEnabled"]),
          AllowONRPForCancelledExpiredLocks = Utils.ParseBoolean(dr["CorrespondentONRPCancelledExpiredLocks"]),
          ONRPEndTime = dr["CorrespondentEndTime"].ToString(),
          MaximumLimit = Utils.ParseBoolean(dr["CorrespondentNoLimit"]),
          ContinuousCoverage = !Utils.ParseBoolean(dr["CorrespondentSpecifyTimes"]),
          Tolerance = Utils.ParseInt(dr["CorrespondentTolerance"]),
          UseChannelDefault = Utils.ParseBoolean(dr["CorrespondentUseChannelDefault"]),
          WeekendHolidayCoverage = Utils.ParseBoolean(dr["CorrespondentWHCoverage"]),
          EnableSatONRP = Utils.ParseBoolean(dr["CorrespondentSatEnabled"]),
          EnableSunONRP = Utils.ParseBoolean(dr["CorrespondentSunEnabled"]),
          ONRPSatEndTime = dr["CorrespondentSatEndTime"].ToString(),
          ONRPSunEndTime = dr["CorrespondentSunEndTime"].ToString()
        },
        ONRPID = Utils.ParseInt(dr["OnrpID"])
      };
    }

    public static void UpdateBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("where ExternalCompanyId = '" + entityId + "' and LockDate = '" + lockDate.Date.ToShortDateString() + "' ");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("IF EXISTS(SELECT 1 FROM ExternalOrgDailyBesteffortLocks ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine(") ");
      dbQueryBuilder2.AppendLine("BEGIN ");
      dbQueryBuilder2.AppendLine("    UPDATE ExternalOrgDailyBesteffortLocks ");
      dbQueryBuilder2.AppendLine("    SET AccruedAmount = AccruedAmount + " + (object) loanAmount + " ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine("END ");
      dbQueryBuilder2.AppendLine("ELSE");
      dbQueryBuilder2.AppendLine("BEGIN");
      dbQueryBuilder2.AppendLine("    INSERT INTO ExternalOrgDailyBesteffortLocks (ExternalCompanyId,LockDate,AccruedAmount) values(");
      dbQueryBuilder2.AppendLine("'" + entityId + "',");
      dbQueryBuilder2.AppendLine("'" + lockDate.Date.ToShortDateString() + "', ");
      dbQueryBuilder2.AppendLine(loanAmount.ToString() + ")");
      dbQueryBuilder2.AppendLine("END ");
      dbQueryBuilder2.Execute();
    }

    public static void UpdateBestEffortDailyLimit(
      string entityid,
      DateTime lockDate,
      double loanAmount,
      string loanGuid,
      string rateSheetId)
    {
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandText = "UpdateBestEffortAccruedAmount";
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand sqlCmd = sqlCommand;
      sqlCmd.Parameters.AddWithValue("@loanGuid", (object) loanGuid);
      sqlCmd.Parameters.AddWithValue("@externalCompanyId", (object) entityid);
      sqlCmd.Parameters.AddWithValue("@rateSheetId", (object) rateSheetId);
      sqlCmd.Parameters.AddWithValue("@lockDate", (object) lockDate);
      sqlCmd.Parameters.AddWithValue("@loanAmount", (object) loanAmount);
      new EllieMae.EMLite.Server.DbAccessManager().ExecuteNonQuery((IDbCommand) sqlCmd);
    }

    public static double GetBestEffortDailyLimit(string entityId, DateTime lockDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("where ExternalCompanyId = '" + entityId + "' and LockDate = '" + lockDate.Date.ToShortDateString() + "' ");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("SELECT AccruedAmount FROM ExternalOrgDailyBesteffortLocks ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder2.Execute();
      return dataRowCollection.Count > 0 ? Convert.ToDouble(dataRowCollection[0]["AccruedAmount"]) : 0.0;
    }

    public static double GetBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      string rateSheetId,
      string loanGuid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("SELECT ISNULL(SUM(ISNULL(locks.AccruedAmount, 0) - ISNULL(loans.LoanAmount, 0)), 0) AS AccruedAmount  FROM ExternalOrgDailyBesteffortLocks locks");
      dbQueryBuilder1.AppendLine("LEFT OUTER JOIN ExternalOrgDailyBestEffortLoans loans ON locks.Id = loans.Id");
      dbQueryBuilder1.AppendLine("AND loans.LoanGUID = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loanGuid));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("WHERE locks.ExternalCompanyId = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(entityId) + " AND locks.LockDate = '" + lockDate.Date.ToShortDateString() + "'");
      if (!string.IsNullOrWhiteSpace(rateSheetId))
        dbQueryBuilder2.AppendLine("AND locks.RateSheetId = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(rateSheetId));
      dbQueryBuilder1.Append(dbQueryBuilder2.ToString());
      return Convert.ToDouble(dbQueryBuilder1.ExecuteScalar());
    }

    public static void ResetBestEffortDailyLimit(
      string entityid,
      DateTime lockDate,
      double loanAmount,
      string loanGuid)
    {
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandText = "ResetBestEffortAccruedAmount";
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand sqlCmd = sqlCommand;
      sqlCmd.Parameters.AddWithValue("@loanGuid", (object) loanGuid);
      sqlCmd.Parameters.AddWithValue("@externalCompanyId", (object) entityid);
      sqlCmd.Parameters.AddWithValue("@lockDate", (object) lockDate);
      sqlCmd.Parameters.AddWithValue("@snapshotLoanAmount", (object) loanAmount);
      new EllieMae.EMLite.Server.DbAccessManager().ExecuteNonQuery((IDbCommand) sqlCmd);
    }

    public enum ExternalOrganizationEntities
    {
      All,
      BasicInfo,
      Dba,
      License,
      LoanCriteria,
      TpoContacts,
      SalesRepAe,
      LenderContacts,
      Warehouse,
      Fees,
      LoComp,
      Commitments,
      TradeManagement,
      Onrp,
      Notes,
      TpocSetup,
      TpoDocs,
      Attachments,
      CustomFields,
      FeeDetails,
      LateFeeSettings,
    }
  }
}
