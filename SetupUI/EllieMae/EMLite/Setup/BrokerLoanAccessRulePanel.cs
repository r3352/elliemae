// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BrokerLoanAccessRulePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BrokerLoanAccessRulePanel : SettingsUserControl
  {
    private Persona loPersona;
    private Persona lpPersona;
    private Persona[] allPersonas;
    private LoanAccessRuleInfo loRule;
    private LoanAccessRuleInfo lpRule;
    private IContainer components;
    private SplitContainer splitContainer1;
    private GroupContainer groupContainer1;
    private GradientPanel gradientPanel1;
    private GroupContainer groupContainer2;
    private Label label1;
    private GradientPanel gradientPanel2;
    private Label label2;
    private CheckBox chkLOConv;
    private CheckBox chkLOTasks;
    private CheckBox chkLOProfit;
    private RadioButton radLOPartialAccess;
    private CheckBox chkLOeFolder;
    private RadioButton radLOFullAccess;
    private CheckBox chkLPConv;
    private CheckBox chkLPTasks;
    private CheckBox chkLPProfit;
    private RadioButton radLPPartialAccess;
    private CheckBox chkLPeFolder;
    private RadioButton radLPFullAccess;
    private CheckBox chkLODisclosures;
    private CheckBox chkLPDisclosures;
    private CheckBox chkApplyToLO;
    private RadioButton radLOViewOnly;
    private RadioButton radLPViewOnly;
    private ComboBox ddlLODocumentTab;
    private ComboBox ddlLPDocumentTab;

    public BrokerLoanAccessRulePanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.loadRules();
      this.Reset();
    }

    private void loadRules()
    {
      this.allPersonas = Session.PersonaManager.GetAllPersonas();
      foreach (Persona allPersona in this.allPersonas)
      {
        if (allPersona.Name == "Loan Officer")
          this.loPersona = allPersona;
        else if (allPersona.Name == "Loan Processor")
          this.lpPersona = allPersona;
      }
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess);
      BizRuleInfo[] allRules = bpmManager.GetAllRules();
      if (this.migrateBusinessRules(allRules))
        allRules = bpmManager.GetAllRules();
      foreach (LoanAccessRuleInfo loanAccessRuleInfo in allRules)
      {
        if (loanAccessRuleInfo.Condition == BizRule.Condition.CurrentLoanAssocMS)
        {
          if (loanAccessRuleInfo.RoleID == 1)
            this.loRule = loanAccessRuleInfo;
          else if (loanAccessRuleInfo.RoleID == 5)
            this.lpRule = loanAccessRuleInfo;
          else if (loanAccessRuleInfo.MilestoneID == CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Processing))
            this.loRule = loanAccessRuleInfo;
          else if (loanAccessRuleInfo.MilestoneID == CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Approved))
            this.lpRule = loanAccessRuleInfo;
        }
      }
      if (this.loRule == null)
        throw new Exception("Cannot load LO loan access rule");
      if (this.lpRule == null)
        throw new Exception("Cannot load LP loan access rule");
    }

    private bool migrateBusinessRules(BizRuleInfo[] rules)
    {
      LoanAccessRuleInfo loanAccessRuleInfo1 = (LoanAccessRuleInfo) null;
      LoanAccessRuleInfo loanAccessRuleInfo2 = (LoanAccessRuleInfo) null;
      foreach (LoanAccessRuleInfo rule in rules)
      {
        if (rule.Condition == BizRule.Condition.FinishedMilestone)
        {
          if (rule.MilestoneID == CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Processing))
            loanAccessRuleInfo1 = rule;
          else if (rule.MilestoneID == CoreMilestoneIDEnumUtil.GetMilestoneID(CoreMilestoneID.Approved))
            loanAccessRuleInfo2 = rule;
        }
      }
      if (loanAccessRuleInfo1 == null && loanAccessRuleInfo2 == null)
        return false;
      LoanAccessBpmManager bpmManager1 = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess);
      WorkflowManager bpmManager2 = (WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow);
      RoleInfo roleInfo1 = (RoleInfo) null;
      RoleInfo roleInfo2 = (RoleInfo) null;
      foreach (RoleInfo allRoleFunction in bpmManager2.GetAllRoleFunctions())
      {
        if (string.Compare(allRoleFunction.Name, "Loan Processor", true) == 0)
          roleInfo1 = allRoleFunction;
        else if (string.Compare(allRoleFunction.Name, "Closer", true) == 0)
          roleInfo2 = allRoleFunction;
      }
      if (roleInfo1 == null)
        throw new Exception("Loan Processor role cannot be found.");
      if (roleInfo2 == null)
        throw new Exception("Closer role cannot be found.");
      int num1;
      if (loanAccessRuleInfo1 != null)
      {
        LoanAccessRuleInfo rule = new LoanAccessRuleInfo(loanAccessRuleInfo1.RuleName);
        rule.Condition = BizRule.Condition.CurrentLoanAssocMS;
        rule.ConditionState = 2.ToString();
        LoanAccessRuleInfo loanAccessRuleInfo3 = rule;
        num1 = roleInfo1.ID;
        string str = num1.ToString();
        loanAccessRuleInfo3.ConditionState2 = str;
        rule.LoanAccessRights = loanAccessRuleInfo1.LoanAccessRights;
        LoanAccessRuleInfo newRule = (LoanAccessRuleInfo) bpmManager1.CreateNewRule((BizRuleInfo) rule);
        int num2 = (int) bpmManager1.ActivateRule(newRule.RuleID);
        bpmManager1.DeleteRule(loanAccessRuleInfo1.RuleID);
      }
      if (loanAccessRuleInfo2 != null)
      {
        LoanAccessRuleInfo rule = new LoanAccessRuleInfo(loanAccessRuleInfo2.RuleName);
        rule.Condition = BizRule.Condition.CurrentLoanAssocMS;
        LoanAccessRuleInfo loanAccessRuleInfo4 = rule;
        num1 = 4;
        string str1 = num1.ToString();
        loanAccessRuleInfo4.ConditionState = str1;
        LoanAccessRuleInfo loanAccessRuleInfo5 = rule;
        num1 = roleInfo2.ID;
        string str2 = num1.ToString();
        loanAccessRuleInfo5.ConditionState2 = str2;
        rule.LoanAccessRights = loanAccessRuleInfo2.LoanAccessRights;
        LoanAccessRuleInfo newRule = (LoanAccessRuleInfo) bpmManager1.CreateNewRule((BizRuleInfo) rule);
        int num3 = (int) bpmManager1.ActivateRule(newRule.RuleID);
        bpmManager1.DeleteRule(loanAccessRuleInfo2.RuleID);
      }
      return true;
    }

    public override void Reset()
    {
      BizRule.LoanAccessRight accessRight1 = (BizRule.LoanAccessRight) this.loRule.GetLoanAccessRights().GetAccessRight(this.loPersona.ID);
      if (accessRight1 == BizRule.LoanAccessRight.EditAll)
      {
        this.radLOFullAccess.Checked = true;
      }
      else
      {
        this.radLOViewOnly.Checked = true;
        if ((accessRight1 & BizRule.LoanAccessRight.DocTracking) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLOeFolder.Checked = true;
          this.ddlLODocumentTab.SelectedIndex = 1;
        }
        else if ((accessRight1 & BizRule.LoanAccessRight.DocTrackingRequestRetrieveService) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLOeFolder.Checked = true;
          this.ddlLODocumentTab.SelectedIndex = 0;
        }
        if ((accessRight1 & BizRule.LoanAccessRight.ConversationLog) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLOConv.Checked = true;
        }
        if ((accessRight1 & BizRule.LoanAccessRight.Task) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLOTasks.Checked = true;
        }
        if ((accessRight1 & BizRule.LoanAccessRight.ProfitMgmt) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLOProfit.Checked = true;
        }
        if ((accessRight1 & BizRule.LoanAccessRight.DisclosureTracking) > (BizRule.LoanAccessRight) 0)
        {
          this.radLOPartialAccess.Checked = true;
          this.chkLODisclosures.Checked = true;
        }
      }
      BizRule.LoanAccessRight accessRight2 = (BizRule.LoanAccessRight) this.lpRule.GetLoanAccessRights().GetAccessRight(this.lpPersona.ID);
      if (accessRight2 == BizRule.LoanAccessRight.EditAll)
      {
        this.radLPFullAccess.Checked = true;
      }
      else
      {
        this.radLPViewOnly.Checked = true;
        if ((accessRight2 & BizRule.LoanAccessRight.DocTracking) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPeFolder.Checked = true;
          this.ddlLPDocumentTab.SelectedIndex = 1;
        }
        else if ((accessRight2 & BizRule.LoanAccessRight.DocTrackingRequestRetrieveService) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPeFolder.Checked = true;
          this.ddlLPDocumentTab.SelectedIndex = 0;
        }
        if ((accessRight2 & BizRule.LoanAccessRight.ConversationLog) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPConv.Checked = true;
        }
        if ((accessRight2 & BizRule.LoanAccessRight.Task) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPTasks.Checked = true;
        }
        if ((accessRight2 & BizRule.LoanAccessRight.ProfitMgmt) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPProfit.Checked = true;
        }
        if ((accessRight2 & BizRule.LoanAccessRight.DisclosureTracking) > (BizRule.LoanAccessRight) 0)
        {
          this.radLPPartialAccess.Checked = true;
          this.chkLPDisclosures.Checked = true;
        }
      }
      this.chkApplyToLO.Checked = Session.ConfigurationManager.GetCompanySetting("Broker", "ApplyLPRightsToLO") == "1";
      base.Reset();
    }

    private void radLOPartialAccess_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radLOPartialAccess.Checked)
      {
        this.chkLOeFolder.Checked = false;
        this.chkLOConv.Checked = false;
        this.chkLOTasks.Checked = false;
        this.chkLOProfit.Checked = false;
        this.chkLODisclosures.Checked = false;
      }
      this.chkLOeFolder.Enabled = this.radLOPartialAccess.Checked;
      this.chkLOConv.Enabled = this.radLOPartialAccess.Checked;
      this.chkLOTasks.Enabled = this.radLOPartialAccess.Checked;
      this.chkLOProfit.Enabled = this.radLOPartialAccess.Checked;
      this.chkLODisclosures.Enabled = this.radLOPartialAccess.Checked;
      this.setDirtyFlag(true);
    }

    private void radLPPartialAccess_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radLPPartialAccess.Checked)
      {
        this.chkLPeFolder.Checked = false;
        this.chkLPConv.Checked = false;
        this.chkLPTasks.Checked = false;
        this.chkLPProfit.Checked = false;
        this.chkLPDisclosures.Checked = false;
      }
      this.chkLPeFolder.Enabled = this.radLPPartialAccess.Checked;
      this.chkLPConv.Enabled = this.radLPPartialAccess.Checked;
      this.chkLPTasks.Enabled = this.radLPPartialAccess.Checked;
      this.chkLPProfit.Enabled = this.radLPPartialAccess.Checked;
      this.chkLPDisclosures.Enabled = this.radLPPartialAccess.Checked;
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess);
      BizRule.LoanAccessRight accessRight1 = BizRule.LoanAccessRight.ViewAllOnly;
      if (this.radLOFullAccess.Checked)
        accessRight1 = BizRule.LoanAccessRight.EditAll;
      else if (this.radLOPartialAccess.Checked)
      {
        if (this.chkLOeFolder.Checked)
        {
          if (this.ddlLODocumentTab.SelectedIndex == 1)
            accessRight1 |= BizRule.LoanAccessRight.DocTracking;
          else
            accessRight1 |= BizRule.LoanAccessRight.DocTrackingRequestRetrieveService | BizRule.LoanAccessRight.DocTrackingRetrieveServiceCurrent | BizRule.LoanAccessRight.DocTrackingPartial;
        }
        if (this.chkLOConv.Checked)
          accessRight1 |= BizRule.LoanAccessRight.ConversationLog;
        if (this.chkLOTasks.Checked)
          accessRight1 |= BizRule.LoanAccessRight.Task;
        if (this.chkLOProfit.Checked)
          accessRight1 |= BizRule.LoanAccessRight.ProfitMgmt;
        if (this.chkLODisclosures.Checked)
          accessRight1 |= BizRule.LoanAccessRight.DisclosureTracking;
        if (accessRight1 == BizRule.LoanAccessRight.ViewAllOnly)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Since no partial access option has been selected for the Loan Officer, View Only access will be granted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      List<PersonaLoanAccessRight> personaLoanAccessRightList1 = new List<PersonaLoanAccessRight>();
      personaLoanAccessRightList1.Add(new PersonaLoanAccessRight(this.loPersona.ID, (int) accessRight1, new string[0]));
      foreach (Persona allPersona in this.allPersonas)
      {
        if (allPersona.ID != this.loPersona.ID && allPersona.ID != Persona.SuperAdministrator.ID)
          personaLoanAccessRightList1.Add(new PersonaLoanAccessRight(allPersona.ID, 16777215, new string[0]));
      }
      this.loRule.LoanAccessRights = personaLoanAccessRightList1.ToArray();
      this.loRule = bpmManager.UpdateBrokerRuleMilestoneSetting(this.loRule);
      bpmManager.UpdateRule((BizRuleInfo) this.loRule);
      BizRule.LoanAccessRight accessRight2 = BizRule.LoanAccessRight.ViewAllOnly;
      if (this.radLPFullAccess.Checked)
        accessRight2 = BizRule.LoanAccessRight.EditAll;
      else if (this.radLPPartialAccess.Checked)
      {
        if (this.chkLPeFolder.Checked)
        {
          if (this.ddlLPDocumentTab.SelectedIndex == 1)
            accessRight2 |= BizRule.LoanAccessRight.DocTracking;
          else
            accessRight2 |= BizRule.LoanAccessRight.DocTrackingRequestRetrieveService | BizRule.LoanAccessRight.DocTrackingRetrieveServiceCurrent | BizRule.LoanAccessRight.DocTrackingPartial;
        }
        if (this.chkLPConv.Checked)
          accessRight2 |= BizRule.LoanAccessRight.ConversationLog;
        if (this.chkLPTasks.Checked)
          accessRight2 |= BizRule.LoanAccessRight.Task;
        if (this.chkLPProfit.Checked)
          accessRight2 |= BizRule.LoanAccessRight.ProfitMgmt;
        if (this.chkLPDisclosures.Checked)
          accessRight2 |= BizRule.LoanAccessRight.DisclosureTracking;
        if (accessRight2 == BizRule.LoanAccessRight.ViewAllOnly)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Since no partial access option has been selected for the Loan Processor, View Only access will be granted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      List<PersonaLoanAccessRight> personaLoanAccessRightList2 = new List<PersonaLoanAccessRight>();
      if (this.chkApplyToLO.Checked)
        personaLoanAccessRightList2.Add(new PersonaLoanAccessRight(this.loPersona.ID, (int) accessRight2, new string[0]));
      else
        personaLoanAccessRightList2.Add(new PersonaLoanAccessRight(this.loPersona.ID, (int) accessRight1, new string[0]));
      personaLoanAccessRightList2.Add(new PersonaLoanAccessRight(this.lpPersona.ID, (int) accessRight2, new string[0]));
      foreach (Persona allPersona in this.allPersonas)
      {
        if (allPersona.ID != this.loPersona.ID && allPersona.ID != this.lpPersona.ID && allPersona.ID != Persona.SuperAdministrator.ID)
          personaLoanAccessRightList2.Add(new PersonaLoanAccessRight(allPersona.ID, 16777215, new string[0]));
      }
      this.lpRule.LoanAccessRights = personaLoanAccessRightList2.ToArray();
      this.lpRule = bpmManager.UpdateBrokerRuleMilestoneSetting(this.lpRule);
      bpmManager.UpdateRule((BizRuleInfo) this.lpRule);
      Session.ConfigurationManager.SetCompanySetting("Broker", "ApplyLPRightsToLO", this.chkApplyToLO.Checked ? "1" : "0");
      base.Save();
      if (accessRight1 == BizRule.LoanAccessRight.ViewAllOnly)
        this.radLOViewOnly.Checked = true;
      if (accessRight2 != BizRule.LoanAccessRight.ViewAllOnly)
        return;
      this.radLPViewOnly.Checked = true;
    }

    private void onAccessOptionChecked(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      if (sender == this.chkLOeFolder)
      {
        this.ddlLODocumentTab.Enabled = this.chkLOeFolder.Checked;
        this.ddlLODocumentTab.SelectedIndex = 0;
      }
      else
      {
        if (sender != this.chkLPeFolder)
          return;
        this.ddlLPDocumentTab.Enabled = this.chkLPeFolder.Checked;
        this.ddlLPDocumentTab.SelectedIndex = 0;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.splitContainer1 = new SplitContainer();
      this.groupContainer1 = new GroupContainer();
      this.radLOViewOnly = new RadioButton();
      this.chkLODisclosures = new CheckBox();
      this.chkLOConv = new CheckBox();
      this.chkLOTasks = new CheckBox();
      this.chkLOProfit = new CheckBox();
      this.radLOPartialAccess = new RadioButton();
      this.chkLOeFolder = new CheckBox();
      this.radLOFullAccess = new RadioButton();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.radLPViewOnly = new RadioButton();
      this.chkApplyToLO = new CheckBox();
      this.chkLPDisclosures = new CheckBox();
      this.chkLPConv = new CheckBox();
      this.chkLPTasks = new CheckBox();
      this.chkLPProfit = new CheckBox();
      this.radLPPartialAccess = new RadioButton();
      this.chkLPeFolder = new CheckBox();
      this.radLPFullAccess = new RadioButton();
      this.gradientPanel2 = new GradientPanel();
      this.label2 = new Label();
      this.ddlLODocumentTab = new ComboBox();
      this.ddlLPDocumentTab = new ComboBox();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.groupContainer1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.groupContainer2);
      this.splitContainer1.Size = new Size(786, 515);
      this.splitContainer1.SplitterDistance = 390;
      this.splitContainer1.TabIndex = 0;
      this.groupContainer1.Controls.Add((Control) this.ddlLODocumentTab);
      this.groupContainer1.Controls.Add((Control) this.radLOViewOnly);
      this.groupContainer1.Controls.Add((Control) this.chkLODisclosures);
      this.groupContainer1.Controls.Add((Control) this.chkLOConv);
      this.groupContainer1.Controls.Add((Control) this.chkLOTasks);
      this.groupContainer1.Controls.Add((Control) this.chkLOProfit);
      this.groupContainer1.Controls.Add((Control) this.radLOPartialAccess);
      this.groupContainer1.Controls.Add((Control) this.chkLOeFolder);
      this.groupContainer1.Controls.Add((Control) this.radLOFullAccess);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(390, 515);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Loan Officer";
      this.radLOViewOnly.AutoSize = true;
      this.radLOViewOnly.Location = new Point(14, 256);
      this.radLOViewOnly.Name = "radLOViewOnly";
      this.radLOViewOnly.Size = new Size(76, 18);
      this.radLOViewOnly.TabIndex = 8;
      this.radLOViewOnly.Text = "View Only";
      this.radLOViewOnly.UseVisualStyleBackColor = true;
      this.radLOViewOnly.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLODisclosures.AutoSize = true;
      this.chkLODisclosures.Location = new Point(32, 228);
      this.chkLODisclosures.Name = "chkLODisclosures";
      this.chkLODisclosures.Size = new Size(121, 18);
      this.chkLODisclosures.TabIndex = 7;
      this.chkLODisclosures.Text = "Disclosure Tracking";
      this.chkLODisclosures.UseVisualStyleBackColor = true;
      this.chkLODisclosures.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLOConv.AutoSize = true;
      this.chkLOConv.Location = new Point(32, 168);
      this.chkLOConv.Name = "chkLOConv";
      this.chkLOConv.Size = new Size(111, 18);
      this.chkLOConv.TabIndex = 4;
      this.chkLOConv.Text = "Conversation Log";
      this.chkLOConv.UseVisualStyleBackColor = true;
      this.chkLOConv.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLOTasks.AutoSize = true;
      this.chkLOTasks.Location = new Point(32, 187);
      this.chkLOTasks.Name = "chkLOTasks";
      this.chkLOTasks.Size = new Size(55, 18);
      this.chkLOTasks.TabIndex = 5;
      this.chkLOTasks.Text = "Tasks";
      this.chkLOTasks.UseVisualStyleBackColor = true;
      this.chkLOTasks.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLOProfit.AutoSize = true;
      this.chkLOProfit.Location = new Point(32, 208);
      this.chkLOProfit.Name = "chkLOProfit";
      this.chkLOProfit.Size = new Size(115, 18);
      this.chkLOProfit.TabIndex = 6;
      this.chkLOProfit.Text = "Profit Management";
      this.chkLOProfit.UseVisualStyleBackColor = true;
      this.chkLOProfit.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.radLOPartialAccess.AutoSize = true;
      this.radLOPartialAccess.Checked = true;
      this.radLOPartialAccess.Location = new Point(14, 96);
      this.radLOPartialAccess.Name = "radLOPartialAccess";
      this.radLOPartialAccess.Size = new Size(95, 18);
      this.radLOPartialAccess.TabIndex = 2;
      this.radLOPartialAccess.TabStop = true;
      this.radLOPartialAccess.Text = "Partial Access";
      this.radLOPartialAccess.UseVisualStyleBackColor = true;
      this.radLOPartialAccess.CheckedChanged += new EventHandler(this.radLOPartialAccess_CheckedChanged);
      this.chkLOeFolder.AutoSize = true;
      this.chkLOeFolder.Location = new Point(32, 120);
      this.chkLOeFolder.Name = "chkLOeFolder";
      this.chkLOeFolder.Size = new Size(140, 18);
      this.chkLOeFolder.TabIndex = 3;
      this.chkLOeFolder.Text = "eFolder Documents Tab";
      this.chkLOeFolder.UseVisualStyleBackColor = true;
      this.chkLOeFolder.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.radLOFullAccess.AutoSize = true;
      this.radLOFullAccess.Location = new Point(14, 68);
      this.radLOFullAccess.Name = "radLOFullAccess";
      this.radLOFullAccess.Size = new Size(82, 18);
      this.radLOFullAccess.TabIndex = 1;
      this.radLOFullAccess.Text = "Full Access";
      this.radLOFullAccess.UseVisualStyleBackColor = true;
      this.radLOFullAccess.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(388, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(302, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "After loans are sent to a processor, Loan Officers can have:";
      this.groupContainer2.Controls.Add((Control) this.ddlLPDocumentTab);
      this.groupContainer2.Controls.Add((Control) this.radLPViewOnly);
      this.groupContainer2.Controls.Add((Control) this.chkApplyToLO);
      this.groupContainer2.Controls.Add((Control) this.chkLPDisclosures);
      this.groupContainer2.Controls.Add((Control) this.chkLPConv);
      this.groupContainer2.Controls.Add((Control) this.chkLPTasks);
      this.groupContainer2.Controls.Add((Control) this.chkLPProfit);
      this.groupContainer2.Controls.Add((Control) this.radLPPartialAccess);
      this.groupContainer2.Controls.Add((Control) this.chkLPeFolder);
      this.groupContainer2.Controls.Add((Control) this.radLPFullAccess);
      this.groupContainer2.Controls.Add((Control) this.gradientPanel2);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(392, 515);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Loan Processor";
      this.radLPViewOnly.AutoSize = true;
      this.radLPViewOnly.Location = new Point(16, 256);
      this.radLPViewOnly.Name = "radLPViewOnly";
      this.radLPViewOnly.Size = new Size(76, 18);
      this.radLPViewOnly.TabIndex = 8;
      this.radLPViewOnly.Text = "View Only";
      this.radLPViewOnly.UseVisualStyleBackColor = true;
      this.radLPViewOnly.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkApplyToLO.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkApplyToLO.CheckAlign = ContentAlignment.TopLeft;
      this.chkApplyToLO.Location = new Point(16, 299);
      this.chkApplyToLO.Name = "chkApplyToLO";
      this.chkApplyToLO.Size = new Size(364, 36);
      this.chkApplyToLO.TabIndex = 9;
      this.chkApplyToLO.Text = "Apply these settings to Loan Officers in addition to Loan Processors after the loan is sent to a closer";
      this.chkApplyToLO.TextAlign = ContentAlignment.TopLeft;
      this.chkApplyToLO.UseVisualStyleBackColor = true;
      this.chkApplyToLO.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLPDisclosures.AutoSize = true;
      this.chkLPDisclosures.Location = new Point(34, 228);
      this.chkLPDisclosures.Name = "chkLPDisclosures";
      this.chkLPDisclosures.Size = new Size(121, 18);
      this.chkLPDisclosures.TabIndex = 7;
      this.chkLPDisclosures.Text = "Disclosure Tracking";
      this.chkLPDisclosures.UseVisualStyleBackColor = true;
      this.chkLPDisclosures.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLPConv.AutoSize = true;
      this.chkLPConv.Location = new Point(34, 168);
      this.chkLPConv.Name = "chkLPConv";
      this.chkLPConv.Size = new Size(111, 18);
      this.chkLPConv.TabIndex = 4;
      this.chkLPConv.Text = "Conversation Log";
      this.chkLPConv.UseVisualStyleBackColor = true;
      this.chkLPConv.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLPTasks.AutoSize = true;
      this.chkLPTasks.Location = new Point(34, 187);
      this.chkLPTasks.Name = "chkLPTasks";
      this.chkLPTasks.Size = new Size(55, 18);
      this.chkLPTasks.TabIndex = 5;
      this.chkLPTasks.Text = "Tasks";
      this.chkLPTasks.UseVisualStyleBackColor = true;
      this.chkLPTasks.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.chkLPProfit.AutoSize = true;
      this.chkLPProfit.Location = new Point(34, 208);
      this.chkLPProfit.Name = "chkLPProfit";
      this.chkLPProfit.Size = new Size(115, 18);
      this.chkLPProfit.TabIndex = 6;
      this.chkLPProfit.Text = "Profit Management";
      this.chkLPProfit.UseVisualStyleBackColor = true;
      this.chkLPProfit.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.radLPPartialAccess.AutoSize = true;
      this.radLPPartialAccess.Checked = true;
      this.radLPPartialAccess.Location = new Point(16, 96);
      this.radLPPartialAccess.Name = "radLPPartialAccess";
      this.radLPPartialAccess.Size = new Size(95, 18);
      this.radLPPartialAccess.TabIndex = 2;
      this.radLPPartialAccess.TabStop = true;
      this.radLPPartialAccess.Text = "Partial Access";
      this.radLPPartialAccess.UseVisualStyleBackColor = true;
      this.radLPPartialAccess.CheckedChanged += new EventHandler(this.radLPPartialAccess_CheckedChanged);
      this.chkLPeFolder.AutoSize = true;
      this.chkLPeFolder.Location = new Point(34, 120);
      this.chkLPeFolder.Name = "chkLPeFolder";
      this.chkLPeFolder.Size = new Size(140, 18);
      this.chkLPeFolder.TabIndex = 3;
      this.chkLPeFolder.Text = "eFolder Documents Tab";
      this.chkLPeFolder.UseVisualStyleBackColor = true;
      this.chkLPeFolder.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.radLPFullAccess.AutoSize = true;
      this.radLPFullAccess.Location = new Point(16, 68);
      this.radLPFullAccess.Name = "radLPFullAccess";
      this.radLPFullAccess.Size = new Size(82, 18);
      this.radLPFullAccess.TabIndex = 1;
      this.radLPFullAccess.TabStop = true;
      this.radLPFullAccess.Text = "Full Access";
      this.radLPFullAccess.UseVisualStyleBackColor = true;
      this.radLPFullAccess.CheckedChanged += new EventHandler(this.onAccessOptionChecked);
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 26);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(390, 25);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel2.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(298, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "After loans are sent to a closer, Loan Processors can have:";
      this.ddlLODocumentTab.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ddlLODocumentTab.Enabled = false;
      this.ddlLODocumentTab.FormattingEnabled = true;
      this.ddlLODocumentTab.Items.AddRange(new object[2]
      {
        (object) "Request/Retrieve ICE Mortgage Technology Network Service Only",
        (object) "Edit"
      });
      this.ddlLODocumentTab.Location = new Point(50, 140);
      this.ddlLODocumentTab.Name = "ddlLODocumentTab";
      this.ddlLODocumentTab.Size = new Size(370, 22);
      this.ddlLODocumentTab.SelectedIndexChanged += new EventHandler(this.onAccessOptionChecked);
      this.ddlLODocumentTab.TabIndex = 9;
      this.ddlLPDocumentTab.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ddlLPDocumentTab.Enabled = false;
      this.ddlLPDocumentTab.FormattingEnabled = true;
      this.ddlLPDocumentTab.Items.AddRange(new object[2]
      {
        (object) "Request/Retrieve ICE Mortgage Technology Network Service Only",
        (object) "Edit"
      });
      this.ddlLPDocumentTab.Location = new Point(50, 140);
      this.ddlLPDocumentTab.Name = "ddlLPDocumentTab";
      this.ddlLPDocumentTab.Size = new Size(370, 22);
      this.ddlLPDocumentTab.SelectedIndexChanged += new EventHandler(this.onAccessOptionChecked);
      this.ddlLPDocumentTab.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.splitContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BrokerLoanAccessRulePanel);
      this.Size = new Size(856, 515);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
