// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LoanReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LoanReportFieldDefs : ReportFieldDefs
  {
    private const string className = "LoanReportFieldDefs";
    private static Hashtable loanReportFieldDefsCollection = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private LoanReportFieldDefs basicFields;
    private static Dictionary<string, List<EllieMae.EMLite.Workflow.Milestone>> sessionMsSetup = new Dictionary<string, List<EllieMae.EMLite.Workflow.Milestone>>();

    public static LoanReportFieldDefs GetLoanReportFieldDefs(Sessions.Session session)
    {
      lock (LoanReportFieldDefs.loanReportFieldDefsCollection)
      {
        if (!LoanReportFieldDefs.loanReportFieldDefsCollection.ContainsKey((object) session.SessionID))
          LoanReportFieldDefs.loanReportFieldDefsCollection.Add((object) session.SessionID, (object) new LoanReportFieldDefs(session));
        return (LoanReportFieldDefs) LoanReportFieldDefs.loanReportFieldDefsCollection[(object) session.SessionID];
      }
    }

    private void init()
    {
      this.basicFields = new LoanReportFieldDefs(this.session, "ReportMap.xml");
      this.basicFields.Add((ReportFieldDef) LoanReportFieldDef.RateLockFieldSelector());
      this.basicFields.Add((ReportFieldDef) LoanReportFieldDef.InterimFieldSelector());
      this.basicFields.Add((ReportFieldDef) LoanReportFieldDef.GFEDisclosedFieldSelector());
    }

    public static LoanReportFieldDefs BasicFields
    {
      get => LoanReportFieldDefs.GetLoanReportFieldDefs(Session.DefaultInstance).BasicFieldsI;
    }

    public LoanReportFieldDefs BasicFieldsI => this.basicFields;

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new LoanReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      return (ReportFieldDef) new LoanReportFieldDef(fieldDef);
    }

    public override string GetFieldPrefix() => "";

    public LoanReportFieldDefs(Sessions.Session session)
      : base(session)
    {
      this.init();
    }

    public LoanReportFieldDefs(LoanReportFieldDefs fieldDefs)
      : base((ReportFieldDefs) fieldDefs)
    {
      this.basicFields = fieldDefs.basicFields;
    }

    private LoanReportFieldDefs(Sessions.Session session, string fileName)
      : base(session, fileName)
    {
    }

    public LoanReportFieldDef this[int index] => (LoanReportFieldDef) this.fieldDefs[index];

    public LoanReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return (LoanReportFieldDef) base.GetFieldByCriterionName(dbname);
    }

    public LoanReportFieldDef GetFieldByID(string fieldId)
    {
      return (LoanReportFieldDef) base.GetFieldByID(fieldId);
    }

    public override void PopulateDynamicOptionList(ReportFieldDef fieldDef)
    {
      if (fieldDef.DisplayType == FieldDisplayType.Milestone)
        this.populateMilestoneOptionList(fieldDef);
      else
        base.PopulateDynamicOptionList(fieldDef);
    }

    private void populateMilestoneOptionList(ReportFieldDef fieldDef)
    {
      if (!this.session.IsConnected)
        return;
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList = (List<EllieMae.EMLite.Workflow.Milestone>) null;
      lock (LoanReportFieldDefs.sessionMsSetup)
      {
        if (!LoanReportFieldDefs.sessionMsSetup.ContainsKey(this.session.SessionID))
          LoanReportFieldDefs.sessionMsSetup[this.session.SessionID] = this.session.StartupInfo.Milestones;
        milestoneList = LoanReportFieldDefs.sessionMsSetup[this.session.SessionID];
      }
      fieldDef.FieldDefinition.Options.Clear();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneList)
        fieldDef.FieldDefinition.Options.AddOption(milestone.Name, milestone.Name);
      fieldDef.FieldDefinition.Options.RequireValueFromList = true;
      fieldDef.FieldType = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList;
    }

    public LoanReportFieldDefs ExtractFields(LoanReportFieldFlags flags)
    {
      LoanReportFieldDefs fields = new LoanReportFieldDefs(this.session);
      if ((flags & LoanReportFieldFlags.AllFields) == (LoanReportFieldFlags) 0)
        flags |= LoanReportFieldFlags.AllFields;
      Bitmask bitmask = new Bitmask((object) flags);
      foreach (LoanReportFieldDef fieldDef in this.fieldDefs)
      {
        bool flag1 = false;
        bool flag2 = LoanXDBAuditField.IsAuditFieldID(fieldDef.FieldID);
        bool flag3 = fieldDef.FieldDefinition is CustomField;
        if (!fieldDef.Selectable)
          flag1 = false;
        else if (this.BasicFieldsI.GetFieldByID(fieldDef.FieldID) != null)
          flag1 = bitmask.Contains((object) LoanReportFieldFlags.IncludeBasicFields);
        else if (flag3)
          flag1 = bitmask.Contains((object) LoanReportFieldFlags.IncludeCustomFields);
        else if (flag2)
          flag1 = bitmask.Contains((object) LoanReportFieldFlags.IncludeAuditFields);
        else if (fieldDef.IsDatabaseField)
          flag1 = bitmask.Contains((object) LoanReportFieldFlags.IncludeReportingFields);
        if (flag1 && (flag2 || fieldDef.IsLoanDataField || !bitmask.Contains((object) LoanReportFieldFlags.LoanDataFieldsOnly)) && (fieldDef.IsDatabaseField || !bitmask.Contains((object) LoanReportFieldFlags.DatabaseFieldsOnly)))
          fields.Add((ReportFieldDef) fieldDef);
      }
      return fields;
    }

    public LoanReportFieldDefs Clone() => new LoanReportFieldDefs(this);

    public static LoanReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      LoanReportFieldFlags flags)
    {
      return LoanReportFieldDefs.GetFieldDefs(session, false, flags);
    }

    public LoanReportFieldDefs GetFieldDefsI(LoanReportFieldFlags flags)
    {
      return this.GetFieldDefsI(false, flags);
    }

    public static LoanReportFieldDefs GetFieldDefs(LoanReportFieldFlags flags, bool applySecurity)
    {
      return LoanReportFieldDefs.GetLoanReportFieldDefs(Session.DefaultInstance).GetFieldDefsI(false, flags, applySecurity);
    }

    public LoanReportFieldDefs GetFieldDefsI(LoanReportFieldFlags flags, bool applySecurity)
    {
      return this.GetFieldDefsI(flags, applySecurity, Session.DefaultInstance);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      LoanReportFieldFlags flags,
      bool applySecurity,
      Sessions.Session session)
    {
      return this.GetFieldDefsI(false, flags, applySecurity, session);
    }

    public static LoanReportFieldDefs GetFieldDefs(
      SessionObjects sessionObjects,
      LoanReportFieldFlags flags,
      Dictionary<string, AclTriState> accessList,
      bool applySecurity)
    {
      return LoanReportFieldDefs.GetLoanReportFieldDefs(Session.DefaultInstance).GetFieldDefsI(sessionObjects, flags, accessList, applySecurity);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      SessionObjects sessionObjects,
      LoanReportFieldFlags flags,
      Dictionary<string, AclTriState> accessList,
      bool applySecurity)
    {
      return this.GetFieldDefsI(false, sessionObjects, flags, accessList, applySecurity);
    }

    public static LoanReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      bool useERDB,
      LoanReportFieldFlags flags)
    {
      return LoanReportFieldDefs.GetLoanReportFieldDefs(session).GetFieldDefsI(useERDB, flags, session);
    }

    public LoanReportFieldDefs GetFieldDefsI(bool useERDB, LoanReportFieldFlags flags)
    {
      return this.GetFieldDefsI(useERDB, flags, Session.DefaultInstance);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      bool useERDB,
      LoanReportFieldFlags flags,
      Sessions.Session session)
    {
      Dictionary<string, AclTriState> userFieldIdAccess = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetUserFieldIDAccess(this.session.UserID, this.session.UserInfo.UserPersonas, true);
      bool applySecurity = true;
      if (this.session.UserInfo.IsSuperAdministrator())
        applySecurity = false;
      return this.GetFieldDefsI(useERDB, this.session.SessionObjects, flags, userFieldIdAccess, applySecurity, session);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      bool useERDB,
      LoanReportFieldFlags flags,
      bool applySecurity)
    {
      return this.GetFieldDefsI(useERDB, flags, applySecurity, Session.DefaultInstance);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      bool useERDB,
      LoanReportFieldFlags flags,
      bool applySecurity,
      Sessions.Session session)
    {
      Dictionary<string, AclTriState> userFieldIdAccess = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetUserFieldIDAccess(this.session.UserID, this.session.UserInfo.UserPersonas);
      if (this.session.UserInfo.IsSuperAdministrator())
        applySecurity = false;
      return this.GetFieldDefsI(useERDB, this.session.SessionObjects, flags, userFieldIdAccess, applySecurity, session);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      bool useERDB,
      SessionObjects sessionObjects,
      LoanReportFieldFlags flags,
      Dictionary<string, AclTriState> accessList,
      bool applySecurity)
    {
      return this.GetFieldDefsI(useERDB, sessionObjects, flags, accessList, applySecurity, Session.DefaultInstance);
    }

    public LoanReportFieldDefs GetFieldDefsI(
      bool useERDB,
      SessionObjects sessionObjects,
      LoanReportFieldFlags flags,
      Dictionary<string, AclTriState> accessList,
      bool applySecurity,
      Sessions.Session session)
    {
      if ((flags & LoanReportFieldFlags.AllFields) == (LoanReportFieldFlags) 0)
        flags |= LoanReportFieldFlags.AllFields;
      Bitmask bitmask = new Bitmask((object) flags);
      LoanReportFieldDefs fieldDefsI = new LoanReportFieldDefs(this.session);
      FieldSettings fieldSettings = sessionObjects.LoanManager.GetFieldSettings();
      AlertConfig[] alertConfigList = sessionObjects.AlertManager.GetAlertConfigList();
      List<LoanExternalFieldConfig> fieldsDefination = sessionObjects.LoanExternalFieldManager.GetAllLoanExternalFieldsDefination();
      if (bitmask.Contains((object) LoanReportFieldFlags.IncludeBasicFields))
      {
        foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.BasicFieldsI)
        {
          if ((fieldDef.IsLoanDataField || !bitmask.Contains((object) LoanReportFieldFlags.LoanDataFieldsOnly)) && (fieldDef.IsDatabaseField || !bitmask.Contains((object) LoanReportFieldFlags.DatabaseFieldsOnly)) && (!(fieldDef.IsDatabaseField & applySecurity) || accessList.ContainsKey(fieldDef.FieldID) && accessList[fieldDef.FieldID] == AclTriState.True) && fieldDef.FieldDefinition.AppliesToEdition(sessionObjects.ServerLicense.Edition))
            fieldDefsI.Add((ReportFieldDef) fieldDef);
        }
        if (!bitmask.Contains((object) LoanReportFieldFlags.LoanDataFieldsOnly))
        {
          foreach (AlertConfig alertConfig in alertConfigList)
          {
            if (alertConfig.Definition.AppliesToEdition(sessionObjects.ServerLicense.Edition))
            {
              FieldDefinition fieldDef = (FieldDefinition) ReportingFieldDefinition.FromAlertDefinition(alertConfig.Definition);
              if (!applySecurity || accessList.ContainsKey(fieldDef.FieldID) && accessList[fieldDef.FieldID] == AclTriState.True)
                fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef("Alerts", fieldDef, fieldDef.FieldID, FieldDisplayType.Alert));
            }
          }
        }
      }
      if (bitmask.Contains((object) LoanReportFieldFlags.IncludeCustomFields) && !bitmask.Contains((object) LoanReportFieldFlags.DatabaseFieldsOnly))
      {
        foreach (CustomFieldInfo customField in fieldSettings.CustomFields)
        {
          if (!customField.IsEmpty())
            fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(customField));
        }
      }
      if (bitmask.Contains((object) LoanReportFieldFlags.IncludeReportingFields))
      {
        LoanXDBTableList loanXdbTableList = sessionObjects.LoanManager.GetLoanXDBTableList(useERDB);
        if (loanXdbTableList == null)
        {
          Tracing.Log(true, "Error", nameof (LoanReportFieldDefs), "Unable to get reporting database field definitions.");
          return (LoanReportFieldDefs) null;
        }
        foreach (LoanXDBField xdbField in (IEnumerable) loanXdbTableList.GetFields().Values)
        {
          FieldDefinition field = EncompassFields.GetField(xdbField.FieldID, fieldSettings, true);
          string idWithCoMortgagor = xdbField.FieldIDWithCoMortgagor;
          if (bitmask.Contains((object) LoanReportFieldFlags.IncludeReportingFields))
          {
            if (field != null)
            {
              if (field.AppliesToEdition(sessionObjects.ServerLicense.Edition))
              {
                if (applySecurity)
                {
                  if (accessList.ContainsKey(idWithCoMortgagor) && accessList[idWithCoMortgagor] == AclTriState.True)
                    fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(field, xdbField));
                }
                else
                  fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(field, xdbField));
              }
            }
            else if (!bitmask.Contains((object) LoanReportFieldFlags.LoanDataFieldsOnly))
            {
              if (applySecurity)
              {
                if (accessList.ContainsKey(idWithCoMortgagor) && accessList[idWithCoMortgagor] == AclTriState.True)
                  fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(xdbField));
              }
              else
                fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(xdbField));
            }
          }
          if (bitmask.Contains((object) LoanReportFieldFlags.IncludeAuditFields) && xdbField.Auditable)
          {
            foreach (LoanXDBAuditField auditTrailField in xdbField.GetAuditTrailFields())
            {
              if (applySecurity)
              {
                if (accessList.ContainsKey(auditTrailField.ReportingCriterionName) && accessList[auditTrailField.ReportingCriterionName] == AclTriState.True)
                  fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(auditTrailField));
              }
              else
                fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef(auditTrailField));
            }
          }
        }
      }
      foreach (LoanExternalFieldConfig loanExternalFieldData in fieldsDefination)
      {
        FieldDefinition fieldDef = (FieldDefinition) ReportingFieldDefinition.FromLoamExternalDataDefinition(loanExternalFieldData);
        fieldDefsI.Add((ReportFieldDef) new LoanReportFieldDef("Loan", fieldDef, loanExternalFieldData.FieldTypeTable + "." + fieldDef.FieldID, FieldDisplayType.LoanMetaData));
      }
      return fieldDefsI;
    }

    public static LoanReportFieldDef[] GetAuditTrailFieldDefinitions()
    {
      return LoanReportFieldDefs.GetLoanReportFieldDefs(Session.DefaultInstance).GetAuditTrailFieldDefinitionsI();
    }

    public LoanReportFieldDef[] GetAuditTrailFieldDefinitionsI()
    {
      LoanXDBTableList loanXdbTableList = this.session.SessionObjects.LoanManager.GetLoanXDBTableList(false);
      List<LoanReportFieldDef> loanReportFieldDefList = new List<LoanReportFieldDef>();
      Dictionary<string, AclTriState> userFieldIdAccess = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetUserFieldIDAccess(this.session.UserID, this.session.UserInfo.UserPersonas);
      foreach (LoanXDBField loanXdbField in (IEnumerable) loanXdbTableList.GetFields().Values)
      {
        if (loanXdbField.Auditable)
        {
          foreach (LoanXDBAuditField auditTrailField in loanXdbField.GetAuditTrailFields())
          {
            if (this.session.UserInfo.IsSuperAdministrator() || userFieldIdAccess.ContainsKey(auditTrailField.ReportingCriterionName) && userFieldIdAccess[auditTrailField.ReportingCriterionName] == AclTriState.True)
              loanReportFieldDefList.Add(new LoanReportFieldDef(auditTrailField));
          }
        }
      }
      return loanReportFieldDefList.ToArray();
    }

    private LoanReportFieldDef[] createLoanTeamMemberFieldsDefs()
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      List<LoanReportFieldDef> loanReportFieldDefList = new List<LoanReportFieldDef>();
      foreach (RoleInfo roleInfo in allRoleFunctions)
      {
        LoanReportFieldDef loanReportFieldDef = new LoanReportFieldDef("Loan Team Member", EncompassFields.GetField("LoanTeamMember.Name." + roleInfo.Name), "LoanTeamMember.FullName." + roleInfo.Name, FieldDisplayType.LoanAssociate, new string[3]
        {
          "LoanTeamMember.AssociateType." + roleInfo.Name,
          "LoanTeamMember.UserID." + roleInfo.Name,
          "LoanTeamMember.GroupID." + roleInfo.Name
        });
        loanReportFieldDefList.Add(loanReportFieldDef);
      }
      return loanReportFieldDefList.ToArray();
    }
  }
}
