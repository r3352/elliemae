// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EDisclosureChannel
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EDisclosureChannel : IXmlSerializable
  {
    private bool isBroker;
    private bool isLender;
    private bool isInfoOnly;
    private ControlOptionType initialControl;
    private ControlOptionType redisclosureControl;
    private EDisclosurePackage allLoans;
    private EDisclosurePackage conditionalApplication;
    private EDisclosurePackage conditionalThreeDay;
    private EDisclosurePackage conditionalLock;
    private EDisclosurePackage conditionalApproval;
    private EDisclosurePackage changedAPR;
    private EDisclosurePackage changedCircumstance;
    private EDisclosurePackage changedLock;

    public EDisclosureChannel()
    {
      this.isBroker = true;
      this.isLender = true;
      this.isInfoOnly = false;
      this.initialControl = ControlOptionType.AllLoans;
      this.redisclosureControl = ControlOptionType.AllLoans;
      this.allLoans = new EDisclosurePackage();
      this.allLoans.Enabled = true;
      this.allLoans.AtApplication = true;
      this.allLoans.ThreeDay = true;
      this.allLoans.AtLock = true;
      this.allLoans.Approval = true;
      this.allLoans.IncludeGFE = true;
      this.allLoans.IncludeTIL = true;
      this.allLoans.RequirementType = PackageRequirementType.None;
      this.conditionalApplication = new EDisclosurePackage();
      this.conditionalApplication.Enabled = true;
      this.conditionalApplication.AtApplication = true;
      this.conditionalApplication.ThreeDay = false;
      this.conditionalApplication.AtLock = false;
      this.conditionalApplication.Approval = false;
      this.conditionalApplication.IncludeGFE = true;
      this.conditionalApplication.IncludeTIL = true;
      this.conditionalApplication.RequirementType = PackageRequirementType.Milestone;
      this.conditionalApplication.RequiredMilestone = CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Started);
      this.conditionalThreeDay = new EDisclosurePackage();
      this.conditionalThreeDay.Enabled = true;
      this.conditionalThreeDay.AtApplication = false;
      this.conditionalThreeDay.ThreeDay = true;
      this.conditionalThreeDay.AtLock = false;
      this.conditionalThreeDay.Approval = false;
      this.conditionalThreeDay.IncludeGFE = true;
      this.conditionalThreeDay.IncludeTIL = true;
      this.conditionalThreeDay.RequirementType = PackageRequirementType.Milestone;
      this.conditionalThreeDay.RequiredMilestone = CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Started);
      this.conditionalLock = new EDisclosurePackage();
      this.conditionalLock.Enabled = true;
      this.conditionalLock.AtApplication = false;
      this.conditionalLock.ThreeDay = false;
      this.conditionalLock.AtLock = true;
      this.conditionalLock.Approval = false;
      this.conditionalLock.IncludeGFE = true;
      this.conditionalLock.IncludeTIL = true;
      this.conditionalLock.RequirementType = PackageRequirementType.Milestone;
      this.conditionalLock.RequiredMilestone = CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Started);
      this.conditionalApproval = new EDisclosurePackage();
      this.conditionalApproval.Enabled = true;
      this.conditionalApproval.AtApplication = false;
      this.conditionalApproval.ThreeDay = false;
      this.conditionalApproval.AtLock = false;
      this.conditionalApproval.Approval = true;
      this.conditionalApproval.IncludeGFE = true;
      this.conditionalApproval.IncludeTIL = true;
      this.conditionalApproval.RequirementType = PackageRequirementType.Milestone;
      this.conditionalApproval.RequiredMilestone = CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Started);
      this.changedAPR = new EDisclosurePackage();
      this.changedAPR.Enabled = true;
      this.changedAPR.AtApplication = true;
      this.changedAPR.ThreeDay = true;
      this.changedAPR.AtLock = true;
      this.changedAPR.Approval = true;
      this.changedAPR.IncludeGFE = true;
      this.changedAPR.IncludeTIL = true;
      this.changedAPR.RequirementType = PackageRequirementType.Alert;
      this.changedAPR.RequiredAlert = StandardAlertID.RediscloseTILRateChange;
      this.changedCircumstance = new EDisclosurePackage();
      this.changedCircumstance.Enabled = true;
      this.changedCircumstance.AtApplication = true;
      this.changedCircumstance.ThreeDay = true;
      this.changedCircumstance.AtLock = true;
      this.changedCircumstance.Approval = true;
      this.changedCircumstance.IncludeGFE = true;
      this.changedCircumstance.IncludeTIL = true;
      this.changedCircumstance.RequirementType = PackageRequirementType.Alert;
      this.changedCircumstance.RequiredAlert = StandardAlertID.RediscloseGFEChangedCircumstances;
      this.changedLock = new EDisclosurePackage();
      this.changedLock.Enabled = true;
      this.changedLock.AtApplication = true;
      this.changedLock.ThreeDay = true;
      this.changedLock.AtLock = true;
      this.changedLock.Approval = true;
      this.changedLock.IncludeGFE = true;
      this.changedLock.IncludeTIL = true;
      this.changedLock.RequirementType = PackageRequirementType.Alert;
      this.changedLock.RequiredAlert = StandardAlertID.RediscloseGFERateLocked;
    }

    public EDisclosureChannel(XmlSerializationInfo info)
    {
      this.isBroker = info.GetBoolean(nameof (IsBroker));
      this.isLender = info.GetBoolean(nameof (IsLender));
      this.isInfoOnly = info.GetBoolean(nameof (IsInformationalOnly), false);
      this.initialControl = info.GetEnum<ControlOptionType>(nameof (InitialControl));
      this.redisclosureControl = info.GetEnum<ControlOptionType>(nameof (RedisclosureControl));
      this.allLoans = (EDisclosurePackage) info.GetValue(nameof (AllLoans), typeof (EDisclosurePackage));
      this.conditionalApplication = (EDisclosurePackage) info.GetValue(nameof (ConditionalApplication), typeof (EDisclosurePackage));
      this.conditionalThreeDay = (EDisclosurePackage) info.GetValue(nameof (ConditionalThreeDay), typeof (EDisclosurePackage));
      this.conditionalLock = (EDisclosurePackage) info.GetValue(nameof (ConditionalLock), typeof (EDisclosurePackage));
      this.conditionalApproval = (EDisclosurePackage) info.GetValue(nameof (ConditionalApproval), typeof (EDisclosurePackage));
      this.changedAPR = (EDisclosurePackage) info.GetValue(nameof (ChangedAPR), typeof (EDisclosurePackage));
      this.changedCircumstance = (EDisclosurePackage) info.GetValue(nameof (ChangedCircumstance), typeof (EDisclosurePackage));
      this.changedLock = (EDisclosurePackage) info.GetValue(nameof (ChangedLock), typeof (EDisclosurePackage));
    }

    public bool IsBroker
    {
      get => this.isBroker;
      set => this.isBroker = value;
    }

    public bool IsLender
    {
      get => this.isLender;
      set => this.isLender = value;
    }

    public bool IsInformationalOnly
    {
      get => this.isInfoOnly;
      set => this.isInfoOnly = value;
    }

    public ControlOptionType InitialControl
    {
      get => this.initialControl;
      set => this.initialControl = value;
    }

    public ControlOptionType RedisclosureControl
    {
      get => this.redisclosureControl;
      set => this.redisclosureControl = value;
    }

    public EDisclosurePackage AllLoans
    {
      get => this.allLoans;
      set => this.allLoans = value;
    }

    public EDisclosurePackage ConditionalApplication
    {
      get => this.conditionalApplication;
      set => this.conditionalApplication = value;
    }

    public EDisclosurePackage ConditionalThreeDay
    {
      get => this.conditionalThreeDay;
      set => this.conditionalThreeDay = value;
    }

    public EDisclosurePackage ConditionalLock
    {
      get => this.conditionalLock;
      set => this.conditionalLock = value;
    }

    public EDisclosurePackage ConditionalApproval
    {
      get => this.conditionalApproval;
      set => this.conditionalApproval = value;
    }

    public EDisclosurePackage ChangedAPR
    {
      get => this.changedAPR;
      set => this.changedAPR = value;
    }

    public EDisclosurePackage ChangedCircumstance
    {
      get => this.changedCircumstance;
      set => this.changedCircumstance = value;
    }

    public EDisclosurePackage ChangedLock
    {
      get => this.changedLock;
      set => this.changedLock = value;
    }

    public ControlOptionType GetControlOption(LoanData loanData)
    {
      switch (this.redisclosureControl)
      {
        case ControlOptionType.User:
          if (RegulationAlerts.GetRediscloseTILAlertRateChange(loanData) != null || RegulationAlerts.GetRediscloseGFEChangedCircumstanceAlert(loanData) != null || RegulationAlerts.GetRediscloseGFERateLockAlert(loanData) != null || RegulationAlerts.GetRediscloseLEChangedCircumstanceAlert(loanData) != null || RegulationAlerts.GetRediscloseLERateLockAlert(loanData) != null)
            return this.redisclosureControl;
          break;
        case ControlOptionType.AllLoans:
          if (this.changedAPR.Evaluate(loanData) || this.changedCircumstance.Evaluate(loanData) || this.changedLock.Evaluate(loanData))
            return this.redisclosureControl;
          break;
      }
      return this.initialControl;
    }

    internal EDisclosurePackage[] GetPackages(
      LoanData loanData,
      MilestoneTemplate template,
      string channelID)
    {
      List<EDisclosurePackage> edisclosurePackageList = new List<EDisclosurePackage>();
      if (this.redisclosureControl == ControlOptionType.AllLoans)
      {
        if (this.changedAPR.Evaluate(loanData))
          edisclosurePackageList.Add(this.changedAPR);
        if (this.changedCircumstance.Evaluate(loanData))
          edisclosurePackageList.Add(this.changedCircumstance);
        if (this.changedLock.Evaluate(loanData))
          edisclosurePackageList.Add(this.changedLock);
        if (edisclosurePackageList.Count > 0)
          return edisclosurePackageList.ToArray();
      }
      switch (this.initialControl)
      {
        case ControlOptionType.AllLoans:
          edisclosurePackageList.Add(this.allLoans);
          break;
        case ControlOptionType.Conditional:
          if (this.conditionalApplication.Evaluate(loanData, template, channelID + "_AtApplication"))
            edisclosurePackageList.Add(this.conditionalApplication);
          if (this.conditionalThreeDay.Evaluate(loanData, template, channelID + "_ThreeDay"))
            edisclosurePackageList.Add(this.conditionalThreeDay);
          if (this.conditionalLock.Evaluate(loanData, template, channelID + "_AtLock"))
            edisclosurePackageList.Add(this.conditionalLock);
          if (this.conditionalApproval.Evaluate(loanData, template, channelID + "_Approval"))
          {
            edisclosurePackageList.Add(this.conditionalApproval);
            break;
          }
          break;
      }
      return edisclosurePackageList.ToArray();
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("IsBroker", (object) this.isBroker);
      info.AddValue("IsLender", (object) this.isLender);
      info.AddValue("IsInformationalOnly", (object) this.isInfoOnly);
      info.AddValue("InitialControl", (object) this.initialControl);
      info.AddValue("RedisclosureControl", (object) this.redisclosureControl);
      info.AddValue("AllLoans", (object) this.allLoans);
      info.AddValue("ConditionalApplication", (object) this.conditionalApplication);
      info.AddValue("ConditionalThreeDay", (object) this.conditionalThreeDay);
      info.AddValue("ConditionalLock", (object) this.conditionalLock);
      info.AddValue("ConditionalApproval", (object) this.conditionalApproval);
      info.AddValue("ChangedAPR", (object) this.changedAPR);
      info.AddValue("ChangedCircumstance", (object) this.changedCircumstance);
      info.AddValue("ChangedLock", (object) this.changedLock);
    }
  }
}
