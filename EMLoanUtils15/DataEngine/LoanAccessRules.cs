// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanAccessRules
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Collections;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanAccessRules : ILoanAccessRules
  {
    private LoanData loan;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    private int[] protectedRoles;

    public LoanAccessRules(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData loan)
    {
      if (sessionObjects == null)
        throw new ArgumentNullException(nameof (sessionObjects));
      if (configInfo == null)
        throw new ArgumentNullException(nameof (configInfo));
      this.loan = loan != null ? loan : throw new ArgumentNullException(nameof (loan));
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
      this.protectedRoles = this.getProtectedRoles();
      this.Refresh();
    }

    public void Refresh()
    {
    }

    public UserInfo GetUserInfo() => this.sessionObjects.UserInfo;

    public bool IsLogEntryAccessible(LogRecordBase logEntry)
    {
      if (this.isPrivilegedUser())
        return true;
      switch (logEntry)
      {
        case DocumentLog _:
          return this.checkAccessibility((DocumentLog) logEntry);
        case PreliminaryConditionLog _:
          return this.checkAccessibility((PreliminaryConditionLog) logEntry);
        default:
          return true;
      }
    }

    private bool checkAccessibility(DocumentLog documentLog)
    {
      bool flag = false;
      LoanContentAccess contentAccess = this.loan.ContentAccess;
      if ((contentAccess & (LoanContentAccess.DocumentTracking | LoanContentAccess.DocTrackingViewOnly | LoanContentAccess.DocTrackingPartial)) != LoanContentAccess.None)
        flag = true;
      else if (contentAccess == LoanContentAccess.None)
        flag = true;
      return flag && documentLog.IsAccessibleToAnyRole(this.GetUsersEffectiveRoles());
    }

    public bool AllowFullAccess()
    {
      foreach (LoanAssociateLog allLoanAssociate in this.loan.GetLogList().GetAllLoanAssociates())
      {
        if (allLoanAssociate.LoanAssociateAccess && (allLoanAssociate.LoanAssociateType == LoanAssociateType.User && string.Compare(allLoanAssociate.LoanAssociateID, this.sessionObjects.UserID, true) == 0 || allLoanAssociate.LoanAssociateType == LoanAssociateType.Group && this.IsUserAssignedToAclGroup(Utils.ParseInt((object) allLoanAssociate.LoanAssociateID))))
          return true;
      }
      return false;
    }

    public bool IsLogEntryEditable(LogRecordBase logEntry)
    {
      if (this.isPrivilegedUser())
        return true;
      if (!this.IsLogEntryAccessible(logEntry))
        return false;
      switch (logEntry)
      {
        case DocumentLog _:
          return this.IsLogEntryProtected(logEntry) ? this.hasFeatureAccess(AclFeature.eFolder_PD_EditDoc) : this.hasFeatureAccess(AclFeature.eFolder_UD_EditDoc);
        case PreliminaryConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_PreliminaryCondition);
        case UnderwritingConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_UW_NewEditImpDel);
        case PostClosingConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
        default:
          return true;
      }
    }

    public bool IsLogEntryDeletable(LogRecordBase logEntry)
    {
      if (this.isPrivilegedUser())
        return true;
      if (!this.IsLogEntryAccessible(logEntry))
        return false;
      switch (logEntry)
      {
        case DocumentLog _:
          DocumentLog documentLog = (DocumentLog) logEntry;
          if (documentLog.IsEmploymentVerification || documentLog.IsObligationVerification || documentLog.IsIncomeVerification || documentLog.IsAssetVerification)
            return this.sessionObjects.UserInfo.IsSuperAdministrator();
          return this.IsLogEntryProtected(logEntry) ? this.hasFeatureAccess(AclFeature.eFolder_PD_DeleteDoc) : this.hasFeatureAccess(AclFeature.eFolder_UD_DeleteDoc);
        case PreliminaryConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_PreliminaryCondition);
        case UnderwritingConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_UW_NewEditImpDel);
        case PostClosingConditionLog _:
          return this.hasFeatureAccess(AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
        default:
          return true;
      }
    }

    public bool IsLogEntryProtected(LogRecordBase logEntry)
    {
      return logEntry is DocumentLog && !logEntry.IsNew && ((DocumentLog) logEntry).IsAccessibleToAnyRole(this.protectedRoles);
    }

    public bool IsAllowedToClearCondition(ConditionLog logEntry)
    {
      if (this.isPrivilegedUser() || this.hasFeatureAccess(AclFeature.eFolder_Conditions_UW_Status_Cleared))
        return true;
      if (logEntry.ConditionType == ConditionType.Underwriting)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) logEntry;
        if (underwritingConditionLog.AllowToClear && underwritingConditionLog.ForRoleID >= 0)
          return this.IsUserInEffectiveRole(underwritingConditionLog.ForRoleID);
      }
      return false;
    }

    public bool CanUpdateEnhancedConditionTrackingStatus(StatusTrackingDefinition trackDef)
    {
      if (this.isPrivilegedUser())
        return true;
      bool result = false;
      bool.TryParse(this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "ENHANCEDCONDNAPPLYACCESSROLE"), out result);
      foreach (int allowedRole in trackDef.AllowedRoles)
      {
        if (result)
        {
          if (this.IsUserInDefinedRole(allowedRole))
            return true;
        }
        else if (this.IsUserInEffectiveRole(allowedRole))
          return true;
      }
      return false;
    }

    public bool IsUserInDefinedRole(int roleId)
    {
      foreach (int usersDefinedRole in this.GetUsersDefinedRoles())
      {
        if (usersDefinedRole == roleId)
          return true;
      }
      return false;
    }

    public int[] GetDefaultRoleAccessForDocument(DocumentLog logEntry)
    {
      IntegerList integerList = new IntegerList(true);
      int[] values = this.GetUsersAssignedRoles(false);
      if (values.Length == 0)
      {
        bool flag = false;
        if (this.sessionObjects.StartupInfo.PolicySettings.Contains((object) "Policies.ApplyDefinedAccessRoles"))
          flag = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.ApplyDefinedAccessRoles"];
        if (flag)
          values = this.GetUsersDefinedRoles();
        if (values.Length == 0)
          values = new int[1]{ RoleInfo.Others.RoleID };
      }
      integerList.AddRange((IEnumerable) values);
      foreach (int roleId in values)
      {
        DocumentDefaultAccessRuleInfo documentAccessRule = this.getDefaultDocumentAccessRule(roleId);
        if (documentAccessRule != null)
          integerList.AddRange((IEnumerable) documentAccessRule.RolesAllowedAccess);
      }
      return integerList.ToArray();
    }

    public bool IsAlertApplicableToUser(LogAlert logAlert)
    {
      return this.IsUserInAssignedRole(logAlert.RoleId);
    }

    public PipelineInfo.Alert[] GetUsersPipelineAlerts()
    {
      PipelineInfo pipelineInfo = this.loan.ToPipelineInfo();
      pipelineInfo.UpdateAlerts(this.sessionObjects.UserInfo, this.configInfo.UserAclGroups, this.configInfo.AlertSetupData);
      return pipelineInfo.Alerts;
    }

    private int[] getUsersAclGroupIDs()
    {
      int[] usersAclGroupIds = new int[this.configInfo.UserAclGroups.Length];
      for (int index = 0; index < usersAclGroupIds.Length; ++index)
        usersAclGroupIds[index] = this.configInfo.UserAclGroups[index].ID;
      return usersAclGroupIds;
    }

    private bool isPrivilegedUser()
    {
      return this.sessionObjects.UserInfo.IsSuperAdministrator() || this.sessionObjects.UserID == "(trusted)";
    }

    private bool hasFeatureAccess(AclFeature feature)
    {
      if (this.sessionObjects.UserInfo.IsSuperAdministrator())
        return true;
      return this.sessionObjects.StartupInfo.UserAclFeatureRights.Contains((object) feature) && (bool) this.sessionObjects.StartupInfo.UserAclFeatureRights[(object) feature];
    }

    public bool IsUserInEffectiveRole(int roleId)
    {
      foreach (int usersEffectiveRole in this.GetUsersEffectiveRoles())
      {
        if (usersEffectiveRole == roleId)
          return true;
      }
      return false;
    }

    public bool IsUserInAssignedRole(int roleId)
    {
      foreach (int usersAssignedRole in this.GetUsersAssignedRoles())
      {
        if (usersAssignedRole == roleId)
          return true;
      }
      return false;
    }

    public int[] GetUsersAssignedRoles() => this.GetUsersAssignedRoles(true);

    public int[] GetUsersAssignedRoles(bool includeFileStarter)
    {
      List<int> intList = new List<int>();
      foreach (LoanAssociateLog allLoanAssociate in this.loan.GetLogList().GetAllLoanAssociates())
      {
        if (allLoanAssociate.RoleID >= RoleInfo.FileStarter.RoleID && (includeFileStarter || allLoanAssociate.RoleID != RoleInfo.FileStarter.RoleID))
        {
          if (allLoanAssociate.LoanAssociateType == LoanAssociateType.User && string.Compare(allLoanAssociate.LoanAssociateID, this.sessionObjects.UserID, true) == 0)
            intList.Add(allLoanAssociate.RoleID);
          else if (allLoanAssociate.LoanAssociateType == LoanAssociateType.Group && this.IsUserAssignedToAclGroup(Utils.ParseInt((object) allLoanAssociate.LoanAssociateID)))
            intList.Add(allLoanAssociate.RoleID);
        }
      }
      return intList.ToArray();
    }

    public bool IsUserAssignedToAclGroup(int groupId)
    {
      foreach (AclGroup userAclGroup in this.configInfo.UserAclGroups)
      {
        if (userAclGroup.ID == groupId)
          return true;
      }
      return false;
    }

    private DocumentDefaultAccessRuleInfo getDefaultDocumentAccessRule(int roleId)
    {
      foreach (DocumentDefaultAccessRuleInfo documentAccessRule in this.configInfo.DefaultDocumentAccessRules)
      {
        if (documentAccessRule.RoleAddedBy == roleId)
          return documentAccessRule;
      }
      return (DocumentDefaultAccessRuleInfo) null;
    }

    public int[] GetUsersDefinedRoles()
    {
      List<int> intList1 = new List<int>();
      foreach (Persona userPersona in this.sessionObjects.UserInfo.UserPersonas)
      {
        if (!intList1.Contains(userPersona.ID))
          intList1.Add(userPersona.ID);
      }
      List<int> intList2 = new List<int>();
      foreach (RoleInfo allRole in this.configInfo.AllRoles)
      {
        foreach (int num in intList1)
        {
          if (Array.IndexOf<int>(allRole.PersonaIDs, num) >= 0 && !intList2.Contains(allRole.RoleID))
            intList2.Add(allRole.RoleID);
        }
      }
      return intList2.ToArray();
    }

    public int[] GetUsersEffectiveRoles()
    {
      int[] usersAssignedRoles = this.GetUsersAssignedRoles(false);
      if (usersAssignedRoles.Length != 0)
        return usersAssignedRoles;
      List<int> intList = new List<int>();
      intList.Add(RoleInfo.Others.RoleID);
      bool flag = false;
      if (this.sessionObjects.StartupInfo.PolicySettings.Contains((object) "Policies.ApplyDefinedAccessRoles"))
        flag = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.ApplyDefinedAccessRoles"];
      if (!flag && this.loan.GetLogList().GetAssignedAssociates(false).Length == 0)
        flag = true;
      if (flag)
      {
        int[] usersDefinedRoles = this.GetUsersDefinedRoles();
        intList.AddRange((IEnumerable<int>) usersDefinedRoles);
      }
      return intList.ToArray();
    }

    private int[] getProtectedRoles()
    {
      ArrayList arrayList = new ArrayList();
      foreach (RoleInfo allRole in this.configInfo.AllRoles)
      {
        if (allRole.Protected)
          arrayList.Add((object) allRole.RoleID);
      }
      return (int[]) arrayList.ToArray(typeof (int));
    }

    private bool checkAccessibility(PreliminaryConditionLog cond)
    {
      if (!cond.UnderwriterAccess && this.configInfo.UserRoleMappings != null)
      {
        foreach (RolesMappingInfo userRoleMapping in this.configInfo.UserRoleMappings)
        {
          if (userRoleMapping.RealWorldRoleID == RealWorldRoleID.Underwriter)
            return false;
        }
      }
      return true;
    }

    public bool IsGFEEditable() => this.loan.Calculator.IsSyncGFERequired;
  }
}
