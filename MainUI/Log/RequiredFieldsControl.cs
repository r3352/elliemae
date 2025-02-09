// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RequiredFieldsControl
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RequiredFieldsControl : UserControl
  {
    private MilestoneLog msToCheck;
    private FieldMilestonePair[] reqFields;
    private LoanData loan;
    private EllieMae.EMLite.Workflow.Milestone msInfo;
    private FieldQuickEditor quickEditor;
    private Hashtable loanMilestones;
    private bool displayGoTo;
    private Hashtable allowedForms;
    private InputFormList systemFormList;
    private FeaturesAclManager aclMgr;
    private InputFormInfo fieldSummaryForm;
    private Sessions.Session session;
    private static RequiredFieldsControl.CacheState cachedDisplayGoTo;
    private IContainer components;
    private System.Windows.Forms.Button btnSummary;
    private ToolTip toolTipField;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.Button btnGoTo;

    public RequiredFieldsControl(
      Sessions.Session session,
      MilestoneLog msToCheck,
      EllieMae.EMLite.Workflow.Milestone msWorkInfo,
      Hashtable loanMilestones)
    {
      this.session = session;
      this.msToCheck = msToCheck;
      this.msInfo = msWorkInfo;
      if (this.msInfo != null)
        this.fieldSummaryForm = Session.FormManager.GetFormInfo(this.msInfo.SummaryFormID);
      this.loan = Session.LoanData;
      this.loanMilestones = loanMilestones;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (RequiredFieldsControl.cachedDisplayGoTo == RequiredFieldsControl.CacheState.NotCached)
        RequiredFieldsControl.cachedDisplayGoTo = !(Session.ConfigurationManager.GetCompanySetting("RequiredFields", "Display") != "1") ? RequiredFieldsControl.CacheState.False : RequiredFieldsControl.CacheState.True;
      this.displayGoTo = RequiredFieldsControl.cachedDisplayGoTo == RequiredFieldsControl.CacheState.True;
      this.quickEditor = new FieldQuickEditor(this.session, this.loan, FieldQuickEditorMode.Other, this.displayGoTo);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.quickEditor);
    }

    public void RefreshFieldList(FieldMilestonePair[] reqFields, bool includePrevious)
    {
      this.reqFields = reqFields;
      if (this.reqFields == null || this.reqFields.Length == 0)
      {
        this.btnGoTo.Visible = false;
      }
      else
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        List<string> stringList = new List<string>();
        for (int index = 0; index < this.reqFields.Length; ++index)
        {
          string key = this.reqFields[index].MilestoneID.ToString();
          if (this.loanMilestones.ContainsKey((object) key) && (!(this.msToCheck.Stage != this.loanMilestones[(object) key].ToString()) || includePrevious))
            stringList.Add(this.reqFields[index].FieldID);
        }
        this.quickEditor.RefreshFieldList(stringList.ToArray(), true);
        if (this.quickEditor.ShowGoToField)
          return;
        this.btnGoTo.Visible = false;
      }
    }

    public bool VerifyRequiredFields()
    {
      if (this.reqFields == null || this.reqFields.Length == 0)
        return true;
      string empty = string.Empty;
      for (int index = 0; index < this.reqFields.Length; ++index)
      {
        if (EncompassFields.GetField(this.reqFields[index].FieldID, this.loan.Settings.FieldSettings) != null)
        {
          string field = this.loan.GetField(this.reqFields[index].FieldID);
          if (field == string.Empty || field == "//")
            return false;
        }
      }
      return true;
    }

    private void btnSummary_Click(object sender, EventArgs e)
    {
      if (this.msInfo == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Milestone Worksheet Information is not available.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.fieldSummaryForm == (InputFormInfo) null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "There is no Field Summary input screen for this milestone.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.allowedForms == null)
        {
          this.systemFormList = new InputFormList(Session.SessionObjects);
          this.allowedForms = CollectionsUtil.CreateCaseInsensitiveHashtable();
          InputFormInfo[] formList = this.systemFormList.GetFormList("All");
          for (int index = 0; index < formList.Length; ++index)
          {
            if (!this.allowedForms.ContainsKey((object) formList[index].FormID.ToLower()))
              this.allowedForms.Add((object) formList[index].FormID.ToLower(), (object) formList[index]);
          }
        }
        if (this.aclMgr == null)
          this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
        InputFormInfo formByName = this.systemFormList.GetFormByName(this.fieldSummaryForm.Name);
        if (formByName.Category == InputFormCategory.Tool)
        {
          if (string.Compare(this.fieldSummaryForm.FormID, "FUNDINGWORKSHEET", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FundingWS) || string.Compare(this.fieldSummaryForm.FormID, "CASHINFO", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_CashToClose) || string.Compare(this.fieldSummaryForm.FormID, "FILECONTACTS", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FileContacts) || string.Compare(this.fieldSummaryForm.FormID, "FUNDBALANCINGWORKSHEET", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FundingBalWS) || string.Compare(this.fieldSummaryForm.FormID, "LOLOCKREQUEST", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LockRequestForm) || string.Compare(this.fieldSummaryForm.FormID, "PREQUAL", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Prequal) || string.Compare(this.fieldSummaryForm.FormID, "PROFITMANAGEMENT", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ProfitMngt) || string.Compare(this.fieldSummaryForm.FormID, "PURCHASEADVICE", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_PurchaseAdviceForm) || string.Compare(this.fieldSummaryForm.FormID, "RENTOWN", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_RentOwn) || string.Compare(this.fieldSummaryForm.FormID, "SHIPPINGDETAIL", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ShippingDetail) || string.Compare(this.fieldSummaryForm.FormID, "TRUSTACCOUNT", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_TrustAccount) || string.Compare(this.fieldSummaryForm.FormID, "UNDERWRITERSUMMARY", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_UnderwriterSummary) || string.Compare(this.fieldSummaryForm.FormID, "WORKSHEET", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DebtConsolidation) || string.Compare(this.fieldSummaryForm.FormID, "LOANCOMP", true) == 0 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LoanComparison))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access '" + this.fieldSummaryForm.Name + "' input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else if (!this.allowedForms.ContainsKey((object) this.fieldSummaryForm.FormID))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access '" + this.fieldSummaryForm.Name + "' input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, this.fieldSummaryForm.Name, formByName, 800, 560, FieldSource.CurrentLoan, "", this.session))
        {
          int num5 = (int) entryPopupDialog.ShowDialog((IWin32Window) Session.MainForm);
        }
      }
    }

    private void btnGoTo_Click(object sender, EventArgs e)
    {
      BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
      if (!businessRuleCheck.HasRequirement(this.loan, this.msToCheck))
        return;
      RequirementDialog.Instance.InitForm(this.loan, this.msToCheck, this.msToCheck, businessRuleCheck.RequiredFields);
      if (!RequirementDialog.Instance.Visible)
      {
        RequirementDialog.Instance.Owner = Session.MainForm;
        RequirementDialog.Instance.Show();
      }
      if (RequirementDialog.Instance.WindowState == FormWindowState.Minimized)
        RequirementDialog.Instance.WindowState = FormWindowState.Normal;
      RequirementDialog.Instance.Activate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnSummary = new System.Windows.Forms.Button();
      this.toolTipField = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.btnGoTo = new System.Windows.Forms.Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnSummary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSummary.Location = new Point(333, 1);
      this.btnSummary.Name = "btnSummary";
      this.btnSummary.Size = new Size(95, 22);
      this.btnSummary.TabIndex = 3;
      this.btnSummary.Text = "Field Summary";
      this.btnSummary.UseVisualStyleBackColor = true;
      this.btnSummary.Click += new EventHandler(this.btnSummary_Click);
      this.groupContainer1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnGoTo);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnSummary);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(433, 292);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Required Fields";
      this.btnGoTo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGoTo.Location = new Point(247, 1);
      this.btnGoTo.Name = "btnGoTo";
      this.btnGoTo.Size = new Size(83, 22);
      this.btnGoTo.TabIndex = 4;
      this.btnGoTo.Text = "Go to Fields";
      this.btnGoTo.UseVisualStyleBackColor = true;
      this.btnGoTo.Click += new EventHandler(this.btnGoTo_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.Name = nameof (RequiredFieldsControl);
      this.Size = new Size(433, 292);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private enum CacheState
    {
      NotCached,
      True,
      False,
    }
  }
}
