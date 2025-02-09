// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QuickButtonsControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QuickButtonsControl : UserControl
  {
    private Button btnView;
    private Button btnOrder;
    private Button btnAudit;
    private System.ComponentModel.Container components;
    private LoanScreen screen;
    private IMainScreen mainScreen;
    private FlowLayoutPanel pnlButtons;
    private Button btnStatusTaxReturns;
    private Button btnOrderTaxReturns;
    private ILoanEditor editor;
    private Button btnEClose;
    private LoanDataMgr currentLoan;

    public QuickButtonsControl(IMainScreen mainScreen, LoanScreen screen, InputFormInfo form)
    {
      this.screen = screen;
      this.mainScreen = mainScreen;
      this.editor = Session.Application.GetService<ILoanEditor>();
      this.InitializeComponent();
      this.loadPage(form);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnView = new Button();
      this.btnOrder = new Button();
      this.btnAudit = new Button();
      this.pnlButtons = new FlowLayoutPanel();
      this.btnEClose = new Button();
      this.btnStatusTaxReturns = new Button();
      this.btnOrderTaxReturns = new Button();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.btnView.Anchor = AnchorStyles.Right;
      this.btnView.BackColor = SystemColors.Control;
      this.btnView.Location = new Point(397, 0);
      this.btnView.Margin = new Padding(1, 0, 0, 0);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(55, 22);
      this.btnView.TabIndex = 5;
      this.btnView.TabStop = false;
      this.btnView.Text = "View";
      this.btnView.UseVisualStyleBackColor = true;
      this.btnView.Click += new EventHandler(this.quickButton_Click);
      this.btnOrder.Anchor = AnchorStyles.Right;
      this.btnOrder.BackColor = SystemColors.Control;
      this.btnOrder.Location = new Point(241, 0);
      this.btnOrder.Margin = new Padding(1, 0, 0, 0);
      this.btnOrder.Name = "btnOrder";
      this.btnOrder.Size = new Size(77, 22);
      this.btnOrder.TabIndex = 4;
      this.btnOrder.TabStop = false;
      this.btnOrder.Text = "Order Docs";
      this.btnOrder.UseVisualStyleBackColor = true;
      this.btnOrder.Click += new EventHandler(this.quickButton_Click);
      this.btnAudit.Anchor = AnchorStyles.Right;
      this.btnAudit.BackColor = SystemColors.Control;
      this.btnAudit.Location = new Point(185, 0);
      this.btnAudit.Margin = new Padding(1, 0, 0, 0);
      this.btnAudit.Name = "btnAudit";
      this.btnAudit.Size = new Size(55, 22);
      this.btnAudit.TabIndex = 3;
      this.btnAudit.TabStop = false;
      this.btnAudit.Text = "Audit";
      this.btnAudit.UseVisualStyleBackColor = true;
      this.btnAudit.Click += new EventHandler(this.quickButton_Click);
      this.pnlButtons.Anchor = AnchorStyles.Right;
      this.pnlButtons.Controls.Add((Control) this.btnView);
      this.pnlButtons.Controls.Add((Control) this.btnEClose);
      this.pnlButtons.Controls.Add((Control) this.btnOrder);
      this.pnlButtons.Controls.Add((Control) this.btnAudit);
      this.pnlButtons.Controls.Add((Control) this.btnStatusTaxReturns);
      this.pnlButtons.Controls.Add((Control) this.btnOrderTaxReturns);
      this.pnlButtons.FlowDirection = FlowDirection.RightToLeft;
      this.pnlButtons.Location = new Point(4, 12);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(464, 22);
      this.pnlButtons.TabIndex = 0;
      this.btnEClose.Anchor = AnchorStyles.Right;
      this.btnEClose.BackColor = SystemColors.Control;
      this.btnEClose.Location = new Point(319, 0);
      this.btnEClose.Margin = new Padding(1, 0, 0, 0);
      this.btnEClose.Name = "btnEClose";
      this.btnEClose.Size = new Size(77, 22);
      this.btnEClose.TabIndex = 6;
      this.btnEClose.TabStop = false;
      this.btnEClose.Text = "eClose";
      this.btnEClose.UseVisualStyleBackColor = true;
      this.btnEClose.Click += new EventHandler(this.quickButton_Click);
      this.btnStatusTaxReturns.Anchor = AnchorStyles.Right;
      this.btnStatusTaxReturns.BackColor = SystemColors.Control;
      this.btnStatusTaxReturns.Location = new Point(99, 0);
      this.btnStatusTaxReturns.Margin = new Padding(1, 0, 0, 0);
      this.btnStatusTaxReturns.Name = "btnStatusTaxReturns";
      this.btnStatusTaxReturns.Size = new Size(85, 22);
      this.btnStatusTaxReturns.TabIndex = 2;
      this.btnStatusTaxReturns.TabStop = false;
      this.btnStatusTaxReturns.Text = "Check Status";
      this.btnStatusTaxReturns.UseVisualStyleBackColor = true;
      this.btnStatusTaxReturns.Click += new EventHandler(this.quickButton_Click);
      this.btnOrderTaxReturns.Anchor = AnchorStyles.Right;
      this.btnOrderTaxReturns.BackColor = SystemColors.Control;
      this.btnOrderTaxReturns.Location = new Point(7, 0);
      this.btnOrderTaxReturns.Margin = new Padding(1, 0, 0, 0);
      this.btnOrderTaxReturns.Name = "btnOrderTaxReturns";
      this.btnOrderTaxReturns.Size = new Size(95, 22);
      this.btnOrderTaxReturns.TabIndex = 1;
      this.btnOrderTaxReturns.TabStop = false;
      this.btnOrderTaxReturns.Text = "Order Transcript";
      this.btnOrderTaxReturns.UseVisualStyleBackColor = true;
      this.btnOrderTaxReturns.Click += new EventHandler(this.quickButton_Click);
      this.Controls.Add((Control) this.pnlButtons);
      this.Name = nameof (QuickButtonsControl);
      this.Size = new Size(459, 44);
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private async void loadPage(InputFormInfo form)
    {
      QuickButtonsControl quickButtonsControl = this;
      quickButtonsControl.screen.LoadForm(form);
      quickButtonsControl.screen.SetTitleOnly(form.Name);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      if (form.FormID.ToLower() == "tax4506t" || form.FormID.ToLower() == "tax4506tclassic")
      {
        if (!service.IsTaxServiceInstalled)
        {
          quickButtonsControl.btnOrderTaxReturns.Visible = false;
          quickButtonsControl.btnStatusTaxReturns.Visible = false;
        }
        quickButtonsControl.btnAudit.Visible = false;
        quickButtonsControl.btnOrder.Visible = false;
        quickButtonsControl.btnView.Visible = false;
        quickButtonsControl.btnEClose.Visible = false;
      }
      else
      {
        quickButtonsControl.btnOrderTaxReturns.Visible = false;
        quickButtonsControl.btnStatusTaxReturns.Visible = false;
        quickButtonsControl.btnEClose.Visible = Session.ACL.IsAuthorizedForFeature(AclFeature.LoanTab_EMClosingDocs_OrderDocsDigitalClosing);
      }
      quickButtonsControl.applyFieldAccessRights();
      if (Session.LoanDataMgr != quickButtonsControl.currentLoan)
      {
        if (quickButtonsControl.currentLoan != null)
          quickButtonsControl.currentLoan.AccessRightsChanged -= new EventHandler(quickButtonsControl.onAccessRightsChanged);
        quickButtonsControl.currentLoan = Session.LoanDataMgr;
        if (quickButtonsControl.currentLoan != null)
          quickButtonsControl.currentLoan.AccessRightsChanged += new EventHandler(quickButtonsControl.onAccessRightsChanged);
      }
      if (quickButtonsControl.currentLoan.LoanData == null || quickButtonsControl.currentLoan.LoanData.LinkSyncType != LinkSyncType.ConstructionLinked)
        return;
      quickButtonsControl.btnOrder.Enabled = false;
      quickButtonsControl.btnEClose.Enabled = false;
    }

    private void onAccessRightsChanged(object sender, EventArgs e) => this.applyFieldAccessRights();

    private void applyFieldAccessRights()
    {
      this.setButtonAccess(this.btnAudit, "REGZ-TIL");
      this.setButtonAccess(this.btnOrder, "REGZ-TIL");
      this.setButtonAccess(this.btnView, "REGZ-TIL");
      this.setButtonAccess(this.btnEClose, "REGZ-TIL");
      this.setButtonAccess(this.btnOrderTaxReturns, "TAX4506");
      this.setButtonAccess(this.btnStatusTaxReturns, "TAX4506");
    }

    private void setButtonAccess(Button button, string prefix)
    {
      button.Enabled = true;
      switch (Session.LoanDataMgr.GetFieldAccessRights("Button_" + prefix + button.Text))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    private void updateBusinessRule()
    {
      try
      {
        if (this.currentLoan.LoanData == null || this.currentLoan.LoanData.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    private void checkSendEDisclosuresAccess(Button btn)
    {
      if (!(bool) Session.DefaultInstance.StartupInfo.PolicySettings[(object) "Policies.VALIDATESYSRULEPRIORORDERINGDOC"])
        return;
      FieldRulesBpmManager bpmManager1 = (FieldRulesBpmManager) Session.DefaultInstance.BPM.GetBpmManager(BpmCategory.FieldRules);
      FieldAccessBpmManager bpmManager2 = (FieldAccessBpmManager) Session.DefaultInstance.BPM.GetBpmManager(BpmCategory.FieldAccess);
      bool flag1 = false;
      bool flag2 = false;
      if (btn == this.btnAudit)
      {
        flag1 = ((IEnumerable<BizRuleInfo>) bpmManager1.GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_REGZ-TILAUDIT")));
        flag2 = ((IEnumerable<BizRuleInfo>) bpmManager2.GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID.ToUpper() == "BUTTON_REGZ-TILAUDIT"));
      }
      else if (btn == this.btnOrder)
      {
        flag1 = ((IEnumerable<BizRuleInfo>) bpmManager1.GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_REGZ-TILORDER DOCS")));
        flag2 = ((IEnumerable<BizRuleInfo>) bpmManager2.GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID.ToUpper() == "BUTTON_REGZ-TILORDER DOCS"));
      }
      else if (btn == this.btnEClose)
      {
        flag1 = ((IEnumerable<BizRuleInfo>) bpmManager1.GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_REGZ-TILECLOSE")));
        flag2 = ((IEnumerable<BizRuleInfo>) bpmManager2.GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID.ToUpper() == "BUTTON_REGZ-TILECLOSE"));
      }
      else if (btn == this.btnView)
      {
        flag1 = ((IEnumerable<BizRuleInfo>) bpmManager1.GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_REGZ-TILVIEW")));
        flag2 = ((IEnumerable<BizRuleInfo>) bpmManager2.GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID.ToUpper() == "BUTTON_REGZ-TILVIEW"));
      }
      if (!(flag1 | flag2) || !this.currentLoan.LoanData.Dirty)
        return;
      if (Utils.Dialog((IWin32Window) this, "You must save the loan before you can add a Disclosure Tracking entry. Would you like to save the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
      {
        WaitDialog waitDialog = new WaitDialog("Encompass is currently checking validation rules to determine if there are known discrepancies before Documents can be drawn", (WaitCallback) null, (object) null);
        waitDialog.Show((IWin32Window) this);
        waitDialog.Top = this.screen.Top + this.screen.Height / 2 - waitDialog.Height / 2;
        waitDialog.Left = this.screen.Left + this.screen.Width / 2 - waitDialog.Width / 2;
        if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false))
        {
          waitDialog.Close();
          return;
        }
        this.updateBusinessRule();
        waitDialog.Close();
      }
      this.applyFieldAccessRights();
    }

    public static bool ValidateButtonFieldDataEntryRule(string buttonEventID, LoanData loanData)
    {
      if (!string.IsNullOrEmpty(buttonEventID) && loanData != null)
      {
        if (loanData.Validator != null)
        {
          try
          {
            loanData.Validator.Validate(buttonEventID, buttonEventID);
          }
          catch (MissingPrerequisiteException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, ex.Message);
            return false;
          }
          catch (ValidationException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, ex.Message);
            return false;
          }
          return true;
        }
      }
      return true;
    }

    private void quickButton_Click(object sender, EventArgs e)
    {
      IEPass service1 = Session.Application.GetService<IEPass>();
      ILoanServices service2 = Session.Application.GetService<ILoanServices>();
      bool policySetting = (bool) Session.DefaultInstance.StartupInfo.PolicySettings[(object) "Policies.VALIDATESYSRULEPRIORORDERINGDOC"];
      switch (((Control) sender).Name)
      {
        case "btnAudit":
          this.checkSendEDisclosuresAccess(this.btnAudit);
          if (policySetting && (!this.btnAudit.Visible || !this.btnAudit.Enabled))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          QuickButtonsControl.TriggerCalcAll();
          if (!this.editor.ShowRegulationAlerts() || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILAudit", this.currentLoan.LoanData))
            break;
          service1.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;AUDIT", false);
          this.editor.RefreshContents("1885");
          break;
        case "btnOrder":
          this.checkSendEDisclosuresAccess(this.btnOrder);
          if (policySetting && (!this.btnOrder.Visible || !this.btnOrder.Enabled))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          QuickButtonsControl.TriggerCalcAll();
          if (!this.editor.ShowRegulationAlertsOrderDoc() || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILOrder Docs", this.currentLoan.LoanData))
            break;
          service1.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;PROCESS", false);
          this.editor.RefreshContents();
          break;
        case "btnView":
          this.checkSendEDisclosuresAccess(this.btnView);
          if (policySetting && (!this.btnView.Visible || !this.btnView.Enabled))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          if (!QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILView", this.currentLoan.LoanData))
            break;
          service1.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;VIEWDOCS", false);
          break;
        case "btnOrderTaxReturns":
          QuickButtonsControl.TriggerCalcAll();
          service2.OrderTaxReturns();
          break;
        case "btnStatusTaxReturns":
          service2.CheckTaxReturnStatus();
          break;
        case "btnEClose":
          this.checkSendEDisclosuresAccess(this.btnEClose);
          if (policySetting && (!this.btnEClose.Visible || !this.btnEClose.Enabled))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          if (!QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILeClose", this.currentLoan.LoanData))
            break;
          IEFolder service3 = Session.Application.GetService<IEFolder>();
          this.btnEClose.Click -= new EventHandler(this.quickButton_Click);
          service3.LaunchEClose(Session.LoanDataMgr);
          this.btnEClose.Click += new EventHandler(this.quickButton_Click);
          break;
      }
    }

    public static void TriggerCalcAll()
    {
      try
      {
        Session.LoanDataMgr.LoanData.Calculator.CalcOnDemand();
        bool skipLockRequestSync = Session.LoanDataMgr.LoanData.Calculator.SkipLockRequestSync;
        Session.LoanDataMgr.LoanData.Calculator.SkipLockRequestSync = true;
        Session.LoanDataMgr.LoanData.Calculator.CalculateAll(false);
        Session.LoanDataMgr.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
      }
      catch (Exception ex)
      {
      }
    }

    public static bool UseQuickPanel(string formId)
    {
      switch (formId.ToLower())
      {
        case "altlender":
        case "borvesting":
        case "closingconditions":
        case "hud1pg1":
        case "hud1pg2":
        case "propertyinfo":
        case "regz50closer":
        case "regzcd":
        case "tax4506t":
        case "tax4506tclassic":
          return true;
        default:
          return false;
      }
    }

    private bool ensurePlanCodeSelected()
    {
      if (Session.LoanData.GetSimpleField("1881") == "")
        ((IFormScreen) this.editor.GetFormScreen()).ExecAction("plancode");
      return Session.LoanData.GetSimpleField("1881") != "";
    }
  }
}
