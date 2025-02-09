// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EDisclosurePackage
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EDisclosurePackage : IXmlSerializable
  {
    private bool enabled;
    private bool atApplication;
    private bool threeDay;
    private bool atLock;
    private bool approval;
    private bool includeGFE;
    private bool includeTIL;
    private bool includeLE;
    private PackageRequirementType requirementType;
    private StandardAlertID requiredAlert;
    private string[] requiredFields;
    private string requiredMilestone;
    private Dictionary<MilestoneTemplate, string> updatedExceptionsList;
    private EdisclosurePackageType packageType;

    public EDisclosurePackage()
    {
    }

    public EDisclosurePackage(XmlSerializationInfo info)
    {
      this.enabled = info.GetBoolean(nameof (Enabled));
      this.atApplication = info.GetBoolean(nameof (AtApplication));
      this.threeDay = info.GetBoolean(nameof (ThreeDay));
      this.atLock = info.GetBoolean(nameof (AtLock));
      this.approval = info.GetBoolean(nameof (Approval));
      this.includeGFE = info.GetBoolean(nameof (IncludeGFE));
      this.includeTIL = info.GetBoolean(nameof (IncludeTIL));
      this.includeLE = info.GetBoolean(nameof (IncludeLE), false);
      this.requirementType = info.GetEnum<PackageRequirementType>(nameof (RequirementType));
      this.requiredAlert = info.GetEnum<StandardAlertID>(nameof (RequiredAlert));
      this.requiredMilestone = info.GetString(nameof (RequiredMilestone));
      XmlList<string> xmlList = (XmlList<string>) info.GetValue(nameof (RequiredFields), typeof (XmlList<string>), (object) null);
      if (xmlList != null)
        this.requiredFields = xmlList.ToArray();
      else
        this.requiredFields = (string[]) null;
    }

    public EdisclosurePackageType PackageType
    {
      get => this.packageType;
      set => this.packageType = value;
    }

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public bool AtApplication
    {
      get => this.atApplication;
      set => this.atApplication = value;
    }

    public bool ThreeDay
    {
      get => this.threeDay;
      set => this.threeDay = value;
    }

    public bool AtLock
    {
      get => this.atLock;
      set => this.atLock = value;
    }

    public bool Approval
    {
      get => this.approval;
      set => this.approval = value;
    }

    public bool IncludeGFE
    {
      get => this.includeGFE;
      set => this.includeGFE = value;
    }

    public bool IncludeTIL
    {
      get => this.includeTIL;
      set => this.includeTIL = value;
    }

    public bool IncludeLE
    {
      get => this.includeLE;
      set => this.includeLE = value;
    }

    public PackageRequirementType RequirementType
    {
      get => this.requirementType;
      set => this.requirementType = value;
    }

    public StandardAlertID RequiredAlert
    {
      get => this.requiredAlert;
      set => this.requiredAlert = value;
    }

    public string[] RequiredFields
    {
      get => this.requiredFields;
      set => this.requiredFields = value;
    }

    public string RequiredMilestone
    {
      get => this.requiredMilestone;
      set => this.requiredMilestone = value;
    }

    public Dictionary<MilestoneTemplate, string> UpdatedExceptionsList
    {
      get => this.updatedExceptionsList;
      set => this.updatedExceptionsList = value;
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Enabled", (object) this.enabled);
      info.AddValue("AtApplication", (object) this.atApplication);
      info.AddValue("ThreeDay", (object) this.threeDay);
      info.AddValue("AtLock", (object) this.atLock);
      info.AddValue("Approval", (object) this.approval);
      info.AddValue("IncludeGFE", (object) this.includeGFE);
      info.AddValue("IncludeTIL", (object) this.includeTIL);
      info.AddValue("IncludeLE", (object) this.includeLE);
      info.AddValue("RequirementType", (object) this.requirementType);
      info.AddValue("RequiredAlert", (object) this.requiredAlert);
      info.AddValue("RequiredMilestone", (object) this.requiredMilestone);
      if (this.requiredFields != null)
        info.AddValue("RequiredFields", (object) new XmlList<string>((IEnumerable<string>) this.requiredFields));
      else
        info.AddValue("RequiredFields", (object) null);
    }

    public bool Evaluate(LoanData loanData)
    {
      return this.Evaluate(loanData, (MilestoneTemplate) null, (string) null);
    }

    internal bool Evaluate(
      LoanData loanData,
      MilestoneTemplate template,
      string milestoneExceptionID)
    {
      if (!this.enabled)
        return false;
      switch (this.requirementType)
      {
        case PackageRequirementType.Alert:
          PipelineInfo.Alert alert = (PipelineInfo.Alert) null;
          switch (this.requiredAlert)
          {
            case StandardAlertID.RediscloseTILRateChange:
              alert = RegulationAlerts.GetRediscloseTILAlertRateChange(loanData);
              break;
            case StandardAlertID.RediscloseGFERateLocked:
              alert = RegulationAlerts.GetRediscloseGFERateLockAlert(loanData) ?? RegulationAlerts.GetRediscloseLERateLockAlert(loanData);
              break;
            case StandardAlertID.RediscloseGFEChangedCircumstances:
              alert = RegulationAlerts.GetRediscloseGFEChangedCircumstanceAlert(loanData) ?? RegulationAlerts.GetRediscloseLEChangedCircumstanceAlert(loanData);
              break;
          }
          return alert != null;
        case PackageRequirementType.Fields:
          if (this.requiredFields != null)
          {
            foreach (string requiredField in this.requiredFields)
            {
              if (loanData.GetSimpleField(requiredField) == string.Empty)
                return false;
            }
          }
          return true;
        case PackageRequirementType.Milestone:
          string requiredMilestone = this.requiredMilestone;
          if (template != null && template.EDisclosureMilestoneSettings != null)
          {
            foreach (KeyValuePair<string, string> milestoneSetting in template.EDisclosureMilestoneSettings)
            {
              if (milestoneExceptionID == milestoneSetting.Key)
                requiredMilestone = milestoneSetting.Value;
            }
          }
          MilestoneLog milestoneLog = (MilestoneLog) null;
          if (requiredMilestone != null)
            milestoneLog = loanData.GetLogList().GetMilestoneByID(requiredMilestone);
          return milestoneLog == null || milestoneLog.Done;
        default:
          return true;
      }
    }
  }
}
