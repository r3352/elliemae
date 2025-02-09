// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.FeaturesAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class FeaturesAclDbAccessor
  {
    private const string className = "FeaturesAclDbAccessor�";
    private const string tableName = "[Acl_Features]�";
    private const string tableName_User = "[Acl_Features_User]�";
    private const string featuresDefaultCacheName = "AclFeaturesDefault�";

    private FeaturesAclDbAccessor()
    {
    }

    [PgReady]
    private static DataRowCollection GetUserAclFeaturesFromDb(AclFeature[] features, string userid)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select featureID, access from [Acl_Features_User] where userid = @userid and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
        DbCommandParameter parameter = new DbCommandParameter(nameof (userid), (object) userid.TrimEnd(), DbType.AnsiString);
        return pgDbQueryBuilder.Execute(parameter);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_Features_User] where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.Execute();
    }

    [PgReady]
    public static Hashtable GetPermissions(AclFeature[] features, string userid)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        if (features.Length == 0)
          return new Hashtable();
        DataRowCollection aclFeaturesFromDb = FeaturesAclDbAccessor.GetUserAclFeaturesFromDb(features, userid);
        Hashtable permissions = new Hashtable();
        for (int index = 0; index < aclFeaturesFromDb.Count; ++index)
          permissions.Add(Enum.ToObject(typeof (AclFeature), (int) aclFeaturesFromDb[index]["featureID"]), (object) (AclTriState) (SQL.DecodeInt(aclFeaturesFromDb[index]["access"]) == 1 ? 1 : 0));
        return permissions;
      }
      if (features.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_Features_User] where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions1 = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions1.Add(Enum.ToObject(typeof (AclFeature), (int) dataRowCollection[index]["featureID"]), (object) (AclTriState) ((byte) dataRowCollection[index]["access"] == (byte) 1 ? 1 : 0));
      return permissions1;
    }

    [PgReady]
    private static DataRowCollection GetAclFeaturesFromDb(AclFeature[] features, int personaID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select featureID, access from [Acl_Features] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
        return pgDbQueryBuilder.Execute();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_Features] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.Execute();
    }

    [PgReady]
    public static Hashtable GetPermissions(AclFeature[] features, int personaID)
    {
      Hashtable permissions = new Hashtable();
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        if (features.Length == 0)
          return new Hashtable();
        DataRowCollection aclFeaturesFromDb = FeaturesAclDbAccessor.GetAclFeaturesFromDb(features, personaID);
        for (int index = 0; index < aclFeaturesFromDb.Count; ++index)
          permissions.Add((object) (AclFeature) aclFeaturesFromDb[index]["featureID"], (object) (SQL.DecodeInt(aclFeaturesFromDb[index]["access"]) == 1));
        bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaID);
        for (int index = 0; index < features.Length; ++index)
        {
          if (!permissions.Contains((object) features[index]))
          {
            if (features[index] == AclFeature.LoanMgmt_Duplicate_Blank || features[index] == AclFeature.SettingsTab_Company_DiagnosticMode || features[index] == AclFeature.ConsumerConnect_Admin || features[index] == AclFeature.ConsumerConnect_Contributor || features[index] == AclFeature.TPOAdministrationTab_AccessToWholesale || features[index] == AclFeature.TPOAdministrationTab_RegisterWholesaleLoanNode || features[index] == AclFeature.TPOAdministrationTab_AccessToCorrespondent || features[index] == AclFeature.TPOAdministrationTab_RegisterCorrespondentLoanNode || features[index] == AclFeature.SettingsTab_Personal_MyProfilePhoto || features[index] == AclFeature.TPOAdministrationTab_submitForPurchase || features[index] == AclFeature.TPOAdministrationTab_ViewPurchaseAdvice || features[index] == AclFeature.SettingsTab_HMDASetup || features[index] == AclFeature.LoanMgmt_HMDAGenerateHMDALAR || features[index] == AclFeature.LoanMgmt_HMDAOrderHMDAServices || features[index] == AclFeature.SettingsTab_HMDAAddLEI || features[index] == AclFeature.SettingsTab_HMDAEditLEI || features[index] == AclFeature.SettingsTab_HMDARemoveLEI || features[index] == AclFeature.SettingsTab_DynamicDataManagement || features[index] == AclFeature.SettingsTab_FeeRules || features[index] == AclFeature.SettingsTab_FieldRules || features[index] == AclFeature.SettingsTab_DataTables || features[index] == AclFeature.SettingsTab_FeeGroups || features[index] == AclFeature.SettingsTab_DataPopulationTiming || features[index] == AclFeature.SettingsTab_AutomatedDisclosures || features[index] == AclFeature.SettingsTab_AutomatedEConsent || features[index] == AclFeature.SettingsTab_OrgUserAddLEI || features[index] == AclFeature.SettingsTab_OrgUserEditLEI || features[index] == AclFeature.SettingsTab_LoanActionCompletion || features[index] == AclFeature.SettingsTab_PersonaAccesstoLoanActions || features[index] == AclFeature.SettingsTab_LoanErrorInformation || features[index] == AclFeature.SettingsTab_InsightsSetup || features[index] == AclFeature.ThinThick_AnalysisTool_Access || features[index] == AclFeature.LoanMgmt_SearchArchiveFolders || features[index] == AclFeature.ExternalSettings_TPOGlobalLenderContact || features[index] == AclFeature.ExternalSettings_SalesReps || features[index] == AclFeature.ExternalSettings_EditSalesReps || features[index] == AclFeature.ExternalSettings_LenderContacts || features[index] == AclFeature.ExternalSettings_EditLenderContacts || features[index] == AclFeature.ExternalSettings_ONRP || features[index] == AclFeature.ExternalSettings_EditONRP || features[index] == AclFeature.ExternalSettings_TradeMgmt || features[index] == AclFeature.ExternalSettings_EditTradeMgmt || features[index] == AclFeature.LoanMgmt_SearchArchiveFolders || features[index] == AclFeature.eFolder_Conditions_SellCondTab || features[index] == AclFeature.eFolder_Conditions_SellCond_AddEditDel || features[index] == AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond || features[index] == AclFeature.ExternalSettings_AuthorizedTrader || features[index] == AclFeature.eFolder_Conditions_SellCond_ImportAllDeliveryCond || features[index] == AclFeature.eFolder_Conditions_SellCond_DeliverConditionResponse || features[index] == AclFeature.eFolder_Conditions_SellCond_ConditionDeliveryStatus || features[index] == AclFeature.TradeTab_SecurityTrades || features[index] == AclFeature.TradeTab_EditSecurityTrades || features[index] == AclFeature.TradeTab_LoanSearch || features[index] == AclFeature.TradeTab_EditLoanSearch || features[index] == AclFeature.TradeTab_LoanTrades || features[index] == AclFeature.TradeTab_EditLoanTrades || features[index] == AclFeature.TradeTab_MasterCommitments || features[index] == AclFeature.TradeTab_EditMasterContracts || features[index] == AclFeature.TradeTab_MBSPools || features[index] == AclFeature.TradeTab_EditMBSPools || features[index] == AclFeature.TradeTab_GSECommitments || features[index] == AclFeature.TradeTab_EditGSECommitments || features[index] == AclFeature.TradeTab_CorrespondentTrades || features[index] == AclFeature.TradeTab_EditCorrespondentTrades || features[index] == AclFeature.TradeTab_CorrespondentMasters || features[index] == AclFeature.TradeTab_EditCorrespondentMasters || features[index] == AclFeature.TradeTab_BidTapeManagement || features[index] == AclFeature.TradeTab_EditBidTapeManagement || features[index] == AclFeature.SettingsTab_LockComparisonToolFields || features[index] == AclFeature.ToolsTab_LockComparisonTool || features[index] == AclFeature.ToolsTab_ValidatePricing || features[index] == AclFeature.LOConnectTab_MakePrimary || features[index] == AclFeature.SettingsTab_PartnerSetup || features[index] == AclFeature.LOConnectTab_TaskPipelineOption || features[index] == AclFeature.LoanMgmt_AccessToArchiveLoans || features[index] == AclFeature.LoanMgmt_AccessToArchiveFolders || features[index] == AclFeature.LOConnectTab_SchedulerEventTool || features[index] == AclFeature.LOConnectTab_AppraisalManagementTool)
              permissions[(object) features[index]] = (object) false;
            else if (features[index] == AclFeature.LOConnectTab_AllServices || features[index] == AclFeature.LOConnectTab_Services || features[index] == AclFeature.LoanMgmt_CreatePipelineViews || features[index] == AclFeature.LOConnectTab_PipelineDefault || features[index] == AclFeature.LOConnectTab_LoanPipelineOption)
              permissions[(object) features[index]] = (object) true;
            else
              permissions[(object) features[index]] = (object) aclFeaturesDefault;
          }
        }
        return permissions;
      }
      if (features.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_Features] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions.Add((object) (AclFeature) dataRowCollection[index]["featureID"], (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
      bool aclFeaturesDefault1 = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaID);
      for (int index = 0; index < features.Length; ++index)
      {
        if (!permissions.Contains((object) features[index]))
        {
          if (features[index] == AclFeature.LoanMgmt_Duplicate_Blank || features[index] == AclFeature.SettingsTab_Company_DiagnosticMode || features[index] == AclFeature.ConsumerConnect_Admin || features[index] == AclFeature.ConsumerConnect_Contributor || features[index] == AclFeature.TPOAdministrationTab_AccessToWholesale || features[index] == AclFeature.TPOAdministrationTab_RegisterWholesaleLoanNode || features[index] == AclFeature.TPOAdministrationTab_AccessToCorrespondent || features[index] == AclFeature.TPOAdministrationTab_RegisterCorrespondentLoanNode || features[index] == AclFeature.SettingsTab_Personal_MyProfilePhoto || features[index] == AclFeature.TPOAdministrationTab_submitForPurchase || features[index] == AclFeature.TPOAdministrationTab_ViewPurchaseAdvice || features[index] == AclFeature.SettingsTab_HMDASetup || features[index] == AclFeature.LoanMgmt_HMDAGenerateHMDALAR || features[index] == AclFeature.LoanMgmt_HMDAOrderHMDAServices || features[index] == AclFeature.SettingsTab_HMDAAddLEI || features[index] == AclFeature.SettingsTab_HMDAEditLEI || features[index] == AclFeature.SettingsTab_HMDARemoveLEI || features[index] == AclFeature.SettingsTab_DynamicDataManagement || features[index] == AclFeature.SettingsTab_FeeRules || features[index] == AclFeature.SettingsTab_FieldRules || features[index] == AclFeature.SettingsTab_DataTables || features[index] == AclFeature.SettingsTab_FeeGroups || features[index] == AclFeature.SettingsTab_DataPopulationTiming || features[index] == AclFeature.SettingsTab_AutomatedDisclosures || features[index] == AclFeature.SettingsTab_AutomatedEConsent || features[index] == AclFeature.SettingsTab_OrgUserAddLEI || features[index] == AclFeature.SettingsTab_OrgUserEditLEI || features[index] == AclFeature.SettingsTab_LoanActionCompletion || features[index] == AclFeature.SettingsTab_PersonaAccesstoLoanActions || features[index] == AclFeature.SettingsTab_LoanErrorInformation || features[index] == AclFeature.SettingsTab_InsightsSetup || features[index] == AclFeature.ThinThick_AnalysisTool_Access || features[index] == AclFeature.LoanMgmt_SearchArchiveFolders || features[index] == AclFeature.ExternalSettings_TPOGlobalLenderContact || features[index] == AclFeature.ExternalSettings_SalesReps || features[index] == AclFeature.ExternalSettings_EditSalesReps || features[index] == AclFeature.ExternalSettings_LenderContacts || features[index] == AclFeature.ExternalSettings_EditLenderContacts || features[index] == AclFeature.ExternalSettings_ONRP || features[index] == AclFeature.ExternalSettings_EditONRP || features[index] == AclFeature.ExternalSettings_TradeMgmt || features[index] == AclFeature.ExternalSettings_EditTradeMgmt || features[index] == AclFeature.LoanMgmt_SearchArchiveFolders || features[index] == AclFeature.eFolder_Conditions_SellCondTab || features[index] == AclFeature.eFolder_Conditions_SellCond_AddEditDel || features[index] == AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond || features[index] == AclFeature.SettingsTab_UnlockTrade || features[index] == AclFeature.ExternalSettings_AuthorizedTrader || features[index] == AclFeature.eFolder_Conditions_SellCond_ImportAllDeliveryCond || features[index] == AclFeature.eFolder_Conditions_SellCond_DeliverConditionResponse || features[index] == AclFeature.eFolder_Conditions_SellCond_ConditionDeliveryStatus || features[index] == AclFeature.TradeTab_SecurityTrades || features[index] == AclFeature.TradeTab_EditSecurityTrades || features[index] == AclFeature.TradeTab_LoanSearch || features[index] == AclFeature.TradeTab_EditLoanSearch || features[index] == AclFeature.TradeTab_LoanTrades || features[index] == AclFeature.TradeTab_EditLoanTrades || features[index] == AclFeature.TradeTab_MasterCommitments || features[index] == AclFeature.TradeTab_EditMasterContracts || features[index] == AclFeature.TradeTab_MBSPools || features[index] == AclFeature.TradeTab_EditMBSPools || features[index] == AclFeature.TradeTab_GSECommitments || features[index] == AclFeature.TradeTab_EditGSECommitments || features[index] == AclFeature.TradeTab_CorrespondentTrades || features[index] == AclFeature.TradeTab_EditCorrespondentTrades || features[index] == AclFeature.TradeTab_CorrespondentMasters || features[index] == AclFeature.TradeTab_EditCorrespondentMasters || features[index] == AclFeature.TradeTab_BidTapeManagement || features[index] == AclFeature.TradeTab_EditBidTapeManagement || features[index] == AclFeature.SettingsTab_LockComparisonToolFields || features[index] == AclFeature.ToolsTab_LockComparisonTool || features[index] == AclFeature.ToolsTab_ValidatePricing || features[index] == AclFeature.LOConnectTab_MakePrimary || features[index] == AclFeature.SettingsTab_PartnerSetup || features[index] == AclFeature.LOConnectTab_TaskPipelineOption || features[index] == AclFeature.LoanMgmt_AccessToArchiveLoans || features[index] == AclFeature.LoanMgmt_AccessToArchiveFolders)
            permissions[(object) features[index]] = (object) false;
          else if (features[index] == AclFeature.eFolder_Conditions_PreliminaryCondTab_ImportCond)
            permissions[(object) features[index]] = permissions[(object) AclFeature.eFolder_Conditions_PreliminaryCondition];
          else if (features[index] == AclFeature.eFolder_Conditions_UnderwritingCond_ImportCond)
            permissions[(object) features[index]] = permissions[(object) AclFeature.eFolder_Conditions_UnderWritingCondTab];
          else if (features[index] == AclFeature.eFolder_Conditions_PostClosingCondition_ImportCond)
            permissions[(object) features[index]] = permissions[(object) AclFeature.eFolder_Conditions_PostClosingCondTab];
          else if (features[index] == AclFeature.ToolsTab_ImportShippingDetails)
          {
            if (permissions.Contains((object) AclFeature.ToolsTab_DocumentTracking))
              permissions[(object) features[index]] = permissions[(object) AclFeature.ToolsTab_DocumentTracking];
          }
          else if (features[index] == AclFeature.LOConnectTab_AllServices || features[index] == AclFeature.LOConnectTab_Services || features[index] == AclFeature.LoanMgmt_CreatePipelineViews || features[index] == AclFeature.LOConnectTab_PipelineDefault || features[index] == AclFeature.LOConnectTab_LoanPipelineOption)
            permissions[(object) features[index]] = (object) true;
          else
            permissions[(object) features[index]] = (object) aclFeaturesDefault1;
        }
      }
      return permissions;
    }

    public static Hashtable GetPermissionsForPersonaAndUserId(
      AclFeature[] features,
      int[] personaIDs,
      string userId = null)
    {
      if (features.Length == 0)
        return new Hashtable();
      if (personaIDs == null || personaIDs.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features));
      for (int index = 0; index < personaIDs.Length; ++index)
        dbQueryBuilder.AppendFormat("select featureID, access, 1 as ispersona from {0} where personaID = {1} and featureID in ({2}) UNION ", (object) "[Acl_Features]", (object) personaIDs[index], (object) str);
      if (!string.IsNullOrEmpty(userId))
        dbQueryBuilder.AppendLine(Environment.NewLine + "select featureID, access, 0 as ispersona from [Acl_Features_User] where userid = " + SQL.EncodeString(userId) + " and featureID in (" + str + ")");
      else
        dbQueryBuilder.Remove(dbQueryBuilder.Length - " UNION ".Length, " UNION ".Length);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        AclFeature key = (AclFeature) dataRowCollection[index]["featureID"];
        if ((int) dataRowCollection[index]["ispersona"] == 1)
        {
          if (!hashtable1.ContainsKey((object) key))
            hashtable1.Add((object) key, (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
          else
            hashtable1[(object) key] = (object) (bool) ((byte) dataRowCollection[index]["access"] == (byte) 1 ? 1 : ((bool) hashtable1[(object) key] ? 1 : 0));
        }
        else
          hashtable2.Add((object) key, (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
      }
      Hashtable hashtable3 = new Hashtable();
      for (int index1 = 0; index1 < personaIDs.Length; ++index1)
      {
        bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIDs[index1]);
        for (int index2 = 0; index2 < features.Length; ++index2)
        {
          if (!hashtable1.ContainsKey((object) features[index2]))
          {
            if (!hashtable3.ContainsKey((object) features[index2]))
              hashtable3.Add((object) features[index2], (object) aclFeaturesDefault);
            else
              hashtable3[(object) features[index2]] = (object) (bool) (aclFeaturesDefault ? 1 : ((bool) hashtable3[(object) features[index2]] ? 1 : 0));
          }
        }
      }
      foreach (DictionaryEntry dictionaryEntry in hashtable3)
        hashtable1.Add(dictionaryEntry.Key, dictionaryEntry.Value);
      Hashtable personaAndUserId = new Hashtable();
      for (int index = 0; index < features.Length; ++index)
        personaAndUserId.Add((object) features[index], (object) false);
      for (int index = 0; index < features.Length; ++index)
        personaAndUserId[(object) features[index]] = (object) (bool) ((bool) personaAndUserId[(object) features[index]] ? 1 : ((bool) hashtable1[(object) features[index]] ? 1 : 0));
      if (hashtable2.Count > 0)
      {
        foreach (AclFeature key in (IEnumerable) hashtable2.Keys)
          personaAndUserId[(object) key] = (object) (bool) hashtable2[(object) key];
      }
      return personaAndUserId;
    }

    public static void SetPermission(AclFeature feature, string userid, AclTriState access)
    {
      FeaturesAclDbAccessor.SetPermissions(new Hashtable()
      {
        [(object) feature] = (object) access
      }, userid);
    }

    public static void SetPermission(AclFeature feature, int personaID, AclTriState access)
    {
      FeaturesAclDbAccessor.SetPermissions(new Hashtable()
      {
        [(object) feature] = (object) access
      }, personaID);
    }

    public static void SetPermissions(Hashtable featureAccesses, string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclFeature key in (IEnumerable) featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_Features_User] where featureID = " + (object) (int) key + " and userid = " + SQL.Encode((object) userid));
        AclTriState featureAccess = (AclTriState) featureAccesses[(object) key];
        if (featureAccess != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_Features_User] (featureID, userid, access) values (" + (object) (int) key + ", " + SQL.Encode((object) userid) + ", " + (object) (int) featureAccess + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(Hashtable featureAccesses, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclFeature key in (IEnumerable) featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_Features] where featureID = " + (object) (int) key + " and personaId = " + (object) personaID);
        AclTriState featureAccess = (AclTriState) featureAccesses[(object) key];
        if (featureAccess != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_Features] (featureID, personaID, access) values (" + (object) (int) key + ", " + (object) personaID + ", " + (object) (int) featureAccess + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DisablePermission(AclFeature feature)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [Acl_Features] set access = 0 where featureID = " + (object) (int) feature);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLFeatures(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_Features] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_Features] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static AclTriState GetPermission(AclFeature feature, string userId)
    {
      Hashtable permissions = FeaturesAclDbAccessor.GetPermissions(new AclFeature[1]
      {
        feature
      }, userId);
      return !permissions.Contains((object) feature) ? AclTriState.Unspecified : (AclTriState) permissions[(object) feature];
    }

    public static bool GetPermission(AclFeature feature, int personaId)
    {
      Hashtable permissions = FeaturesAclDbAccessor.GetPermissions(new AclFeature[1]
      {
        feature
      }, personaId);
      return permissions.Contains((object) feature) && (bool) permissions[(object) feature];
    }

    public static Hashtable GetPermissions(AclFeature[] features, Persona[] personas)
    {
      int[] personaIDs = new int[personas.Length];
      for (int index = 0; index < personaIDs.Length; ++index)
        personaIDs[index] = personas[index].ID;
      return FeaturesAclDbAccessor.GetPermissions(features, personaIDs);
    }

    public static Hashtable GetPermissions(AclFeature[] features, int[] personaIDs)
    {
      Hashtable permissions1 = new Hashtable();
      for (int index = 0; index < features.Length; ++index)
        permissions1.Add((object) features[index], (object) false);
      if (personaIDs != null && personaIDs.Length != 0)
      {
        for (int index1 = 0; index1 < personaIDs.Length; ++index1)
        {
          Hashtable permissions2 = FeaturesAclDbAccessor.GetPermissions(features, personaIDs[index1]);
          for (int index2 = 0; index2 < features.Length; ++index2)
            permissions1[(object) features[index2]] = (object) (bool) ((bool) permissions1[(object) features[index2]] ? 1 : ((bool) permissions2[(object) features[index2]] ? 1 : 0));
        }
      }
      return permissions1;
    }

    public static bool CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      if (userInfo.Userid != "(trusted)" && Company.GetCurrentEdition() == EncompassEdition.Broker)
      {
        AclTriState defaultBrokerAccess = FeatureSets.GetDefaultBrokerAccess(feature);
        if (defaultBrokerAccess != AclTriState.Unspecified)
          return defaultBrokerAccess == AclTriState.True;
      }
      AclTriState permission = FeaturesAclDbAccessor.GetPermission(feature, userInfo.Userid);
      if (permission != AclTriState.Unspecified)
        return permission == AclTriState.True;
      for (int index = 0; index < userInfo.UserPersonas.Length; ++index)
      {
        if (FeaturesAclDbAccessor.GetPermission(feature, userInfo.UserPersonas[index].ID))
          return true;
      }
      return false;
    }

    public static Hashtable CheckPermissions(AclFeature[] features, UserInfo userInfo)
    {
      Hashtable permissions1 = FeaturesAclDbAccessor.GetPermissions(features, AclUtils.GetPersonaIDs(userInfo.UserPersonas));
      Hashtable permissions2 = FeaturesAclDbAccessor.GetPermissions(features, userInfo.Userid);
      foreach (AclFeature key in (IEnumerable) permissions2.Keys)
        permissions1[(object) key] = (object) ((AclTriState) permissions2[(object) key] == AclTriState.True);
      if (userInfo.Userid != "(trusted)" && Company.GetCurrentEdition() == EncompassEdition.Broker)
      {
        foreach (AclFeature feature in features)
        {
          AclTriState defaultBrokerAccess = FeatureSets.GetDefaultBrokerAccess(feature);
          if (defaultBrokerAccess != AclTriState.Unspecified)
            permissions1[(object) feature] = (object) (defaultBrokerAccess == AclTriState.True);
        }
      }
      return permissions1;
    }

    public static string[] GetPersonaListByFeature(AclFeature[] features, AclTriState access)
    {
      DataTable personaRecordByFeature = FeaturesAclDbAccessor.getPersonaRecordByFeature(features, access);
      string[] personaListByFeature = new string[personaRecordByFeature.Rows.Count];
      for (int index = 0; index < personaRecordByFeature.Rows.Count; ++index)
        personaListByFeature[index] = string.Concat(personaRecordByFeature.Rows[index][0]);
      return personaListByFeature;
    }

    public static string[] GetUserListByFeature(AclFeature[] features, AclTriState access)
    {
      DataTable userRecordByFeature = FeaturesAclDbAccessor.getUserRecordByFeature(features, access);
      string[] userListByFeature = new string[userRecordByFeature.Rows.Count];
      for (int index = 0; index < userRecordByFeature.Rows.Count; ++index)
        userListByFeature[index] = string.Concat(userRecordByFeature.Rows[index][0]);
      return userListByFeature;
    }

    public static bool GetAclFeaturesDefault(int[] personaIDs)
    {
      bool aclFeaturesDefault = false;
      for (int index = 0; index < personaIDs.Length; ++index)
      {
        aclFeaturesDefault = aclFeaturesDefault || FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIDs[index]);
        if (aclFeaturesDefault)
          break;
      }
      return aclFeaturesDefault;
    }

    public static bool GetAclFeaturesDefault(int personaID)
    {
      return PersonaAccessor.GetPersonaAclFeaturesDefault(personaID);
    }

    private static DataTable getPersonaRecordByFeature(AclFeature[] features, AclTriState access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct personaID from [Acl_Features] where access = " + (object) (int) access + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    private static DataTable getUserRecordByFeature(AclFeature[] features, AclTriState access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct userid from [Acl_Features_User] where access = " + (object) (int) access + " and featureID in (" + SQL.EncodeArray((Array) FeaturesAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    private static int[] getFeatureIDs(AclFeature[] features)
    {
      int[] featureIds = new int[features.Length];
      for (int index = 0; index < features.Length; ++index)
        featureIds[index] = (int) features[index];
      return featureIds;
    }
  }
}
