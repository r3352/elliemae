// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.GoodFaithViolationResolutionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class GoodFaithViolationResolutionDialog : Form
  {
    private string _requiredCureAmount;
    private IContainer components;
    private DatePicker dpDateResolved;
    private Label lblDateResolved;
    private TextBox txtReason;
    private DialogButtons dlgButtons;
    private TextBox txtAppliedCureAmount;
    private Label lblAppliedCureAmount;
    private Label lblRequiredCureAmount;
    private TextBox tbxRequiredCureAmount;
    private FieldLockButton lockBtnRequiredCureAmount;
    private Label lblResolvedBy;
    private Label lblComments;
    private TextBox tbxResolvedBy;
    private Label label1;
    private TextBox txtLenderCureAmount;
    private Label label2;
    private TextBox txtPOC;

    public GoodFaithViolationResolutionDialog()
    {
      this.InitializeComponent();
      if (Session.LoanData != null)
      {
        this.txtAppliedCureAmount.Text = Session.LoanData.GetField("FV.X366");
        this.txtLenderCureAmount.Text = Session.LoanData.GetField("FV.X396");
        this.txtPOC.Text = Session.LoanData.GetField("FV.X397");
      }
      else
      {
        this.txtAppliedCureAmount.Text = "";
        this.txtLenderCureAmount.Text = "";
      }
      bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_CureToleranceAlert);
      bool flag1 = true;
      bool flag2 = true;
      if (Session.LoanDataMgr != null)
      {
        Hashtable fieldAccessList = Session.LoanDataMgr.GetFieldAccessList();
        string str = "LOCKBUTTON_FV.X348";
        if (fieldAccessList != null && fieldAccessList.ContainsKey((object) str))
        {
          switch (Session.LoanDataMgr.GetFieldAccessRights(str))
          {
            case BizRule.FieldAccessRight.Hide:
              flag2 = false;
              break;
            case BizRule.FieldAccessRight.ViewOnly:
              flag1 = false;
              break;
          }
        }
      }
      this.lockBtnRequiredCureAmount.Enabled = applicationRight & flag1;
      this.lockBtnRequiredCureAmount.Visible = flag2;
      if (Session.LoanData != null)
      {
        this.lockBtnRequiredCureAmount.Locked = Session.LoanData.IsLocked("FV.X348");
        this.tbxRequiredCureAmount.ReadOnly = !this.lockBtnRequiredCureAmount.Locked;
        DateTime result;
        if (!string.IsNullOrWhiteSpace(Session.LoanData.GetField("3171")) && DateTime.TryParse(Session.LoanData.GetField("3171"), out result))
          this.dpDateResolved.Value = result;
      }
      this._requiredCureAmount = this.tbxRequiredCureAmount.Text = Session.LoanData != null ? Session.LoanData.GetField("FV.X348") : string.Empty;
      this.tbxResolvedBy.Text = Session.LoanData != null ? Session.LoanData.GetField("3173") : string.Empty;
      this.txtReason.Text = Session.LoanData != null ? Session.LoanData.GetField("3172") : string.Empty;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.txtReason.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first provide an explanation of how the Good Faith Fee Variance violation was resolved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.dpDateResolved.Value == DateTime.MinValue)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must provide the date on which the violation was resolved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.txtAppliedCureAmount.Text.Trim() == "")
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "You must first provide Applied Cure Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.txtLenderCureAmount.Text.Trim() == "")
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "You must first provide Cure Applied to Lender Credit Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.lockBtnRequiredCureAmount.Locked)
          Session.LoanData.RemoveCurrentLock("FV.X348");
        if (this._requiredCureAmount != this.tbxRequiredCureAmount.Text.Trim())
          Session.LoanData.SetField("FV.X348", this.tbxRequiredCureAmount.Text.Trim());
        string field = Session.LoanData.GetField("3171");
        string val = this.dpDateResolved.Value.ToString("MM/dd/yyyy");
        Session.LoanData.SetField("3171", val);
        if (field == val)
          Session.LoanData.TriggerCalculation("3171", val);
        Session.LoanData.SetField("3172", this.txtReason.Text.Trim());
        Session.LoanData.SetField("3173", Session.UserID);
        Session.LoanData.SetField("FV.X396", this.txtLenderCureAmount.Text.Trim());
        this.DialogResult = DialogResult.OK;
      }
    }

    private void CureAmount_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void lockBtnRequiredCureAmount_Click(object sender, EventArgs e)
    {
      this.lockBtnRequiredCureAmount.Locked = !this.lockBtnRequiredCureAmount.Locked;
      this.tbxRequiredCureAmount.ReadOnly = !this.lockBtnRequiredCureAmount.Locked;
      if (this.lockBtnRequiredCureAmount.Locked)
        this.tbxRequiredCureAmount.Text = string.Empty;
      else
        this.tbxRequiredCureAmount.Text = Session.LoanData != null ? Session.LoanData.GetField("FV.X348") : string.Empty;
    }

    private void txtLenderCureAmount_Leave(object sender, EventArgs e)
    {
      if (Session.LoanData == null)
        return;
      this.txtAppliedCureAmount.Text = (Utils.ParseDouble((object) this.txtLenderCureAmount.Text) + Utils.ParseDouble((object) this.txtPOC.Text)).ToString("0.00");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dpDateResolved = new DatePicker();
      this.lblDateResolved = new Label();
      this.txtReason = new TextBox();
      this.dlgButtons = new DialogButtons();
      this.txtAppliedCureAmount = new TextBox();
      this.lblAppliedCureAmount = new Label();
      this.lblRequiredCureAmount = new Label();
      this.tbxRequiredCureAmount = new TextBox();
      this.lockBtnRequiredCureAmount = new FieldLockButton();
      this.lblResolvedBy = new Label();
      this.lblComments = new Label();
      this.tbxResolvedBy = new TextBox();
      this.label1 = new Label();
      this.txtLenderCureAmount = new TextBox();
      this.label2 = new Label();
      this.txtPOC = new TextBox();
      this.SuspendLayout();
      this.dpDateResolved.BackColor = SystemColors.Window;
      this.dpDateResolved.Location = new Point(174, 105);
      this.dpDateResolved.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpDateResolved.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpDateResolved.Name = "dpDateResolved";
      this.dpDateResolved.Size = new Size(173, 21);
      this.dpDateResolved.TabIndex = 7;
      this.dpDateResolved.ToolTip = "";
      this.dpDateResolved.Value = new DateTime(0L);
      this.lblDateResolved.AutoSize = true;
      this.lblDateResolved.Location = new Point(17, 109);
      this.lblDateResolved.Name = "lblDateResolved";
      this.lblDateResolved.Size = new Size(30, 13);
      this.lblDateResolved.TabIndex = 6;
      this.lblDateResolved.Text = "Date";
      this.txtReason.Location = new Point(20, 172);
      this.txtReason.Multiline = true;
      this.txtReason.Name = "txtReason";
      this.txtReason.ScrollBars = ScrollBars.Vertical;
      this.txtReason.Size = new Size(362, 99);
      this.txtReason.TabIndex = 11;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 291);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(411, 44);
      this.dlgButtons.TabIndex = 12;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.txtAppliedCureAmount.Enabled = false;
      this.txtAppliedCureAmount.Location = new Point(174, 33);
      this.txtAppliedCureAmount.Name = "txtAppliedCureAmount";
      this.txtAppliedCureAmount.Size = new Size(173, 20);
      this.txtAppliedCureAmount.TabIndex = 5;
      this.txtAppliedCureAmount.KeyPress += new KeyPressEventHandler(this.CureAmount_KeyPress);
      this.lblAppliedCureAmount.AutoSize = true;
      this.lblAppliedCureAmount.Location = new Point(17, 36);
      this.lblAppliedCureAmount.Name = "lblAppliedCureAmount";
      this.lblAppliedCureAmount.Size = new Size(106, 13);
      this.lblAppliedCureAmount.TabIndex = 4;
      this.lblAppliedCureAmount.Text = "Applied Cure Amount";
      this.lblRequiredCureAmount.AutoSize = true;
      this.lblRequiredCureAmount.Location = new Point(17, 14);
      this.lblRequiredCureAmount.Name = "lblRequiredCureAmount";
      this.lblRequiredCureAmount.Size = new Size(114, 13);
      this.lblRequiredCureAmount.TabIndex = 1;
      this.lblRequiredCureAmount.Text = "Required Cure Amount";
      this.tbxRequiredCureAmount.Location = new Point(174, 11);
      this.tbxRequiredCureAmount.Name = "tbxRequiredCureAmount";
      this.tbxRequiredCureAmount.ReadOnly = true;
      this.tbxRequiredCureAmount.Size = new Size(173, 20);
      this.tbxRequiredCureAmount.TabIndex = 3;
      this.tbxRequiredCureAmount.KeyPress += new KeyPressEventHandler(this.CureAmount_KeyPress);
      this.lockBtnRequiredCureAmount.Location = new Point(152, 13);
      this.lockBtnRequiredCureAmount.MaximumSize = new Size(16, 16);
      this.lockBtnRequiredCureAmount.MinimumSize = new Size(16, 16);
      this.lockBtnRequiredCureAmount.Name = "lockBtnRequiredCureAmount";
      this.lockBtnRequiredCureAmount.Size = new Size(16, 16);
      this.lockBtnRequiredCureAmount.TabIndex = 2;
      this.lockBtnRequiredCureAmount.Click += new EventHandler(this.lockBtnRequiredCureAmount_Click);
      this.lblResolvedBy.AutoSize = true;
      this.lblResolvedBy.Location = new Point(17, 134);
      this.lblResolvedBy.Name = "lblResolvedBy";
      this.lblResolvedBy.Size = new Size(66, 13);
      this.lblResolvedBy.TabIndex = 8;
      this.lblResolvedBy.Text = "Resolved by";
      this.lblComments.AutoSize = true;
      this.lblComments.Location = new Point(17, 156);
      this.lblComments.Name = "lblComments";
      this.lblComments.Size = new Size(56, 13);
      this.lblComments.TabIndex = 10;
      this.lblComments.Text = "Comments";
      this.tbxResolvedBy.Location = new Point(174, 131);
      this.tbxResolvedBy.Name = "tbxResolvedBy";
      this.tbxResolvedBy.ReadOnly = true;
      this.tbxResolvedBy.Size = new Size(173, 20);
      this.tbxResolvedBy.TabIndex = 9;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(17, 85);
      this.label1.Name = "label1";
      this.label1.Size = new Size(144, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "Cure applied to Lender Credit";
      this.txtLenderCureAmount.Location = new Point(174, 82);
      this.txtLenderCureAmount.Name = "txtLenderCureAmount";
      this.txtLenderCureAmount.Size = new Size(173, 20);
      this.txtLenderCureAmount.TabIndex = 14;
      this.txtLenderCureAmount.Leave += new EventHandler(this.txtLenderCureAmount_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(17, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(152, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "Cure applied to Principal (POC)";
      this.txtPOC.Enabled = false;
      this.txtPOC.Location = new Point(174, 57);
      this.txtPOC.Name = "txtPOC";
      this.txtPOC.Size = new Size(173, 20);
      this.txtPOC.TabIndex = 16;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(411, 335);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtPOC);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtLenderCureAmount);
      this.Controls.Add((Control) this.tbxResolvedBy);
      this.Controls.Add((Control) this.lblComments);
      this.Controls.Add((Control) this.lblResolvedBy);
      this.Controls.Add((Control) this.lockBtnRequiredCureAmount);
      this.Controls.Add((Control) this.tbxRequiredCureAmount);
      this.Controls.Add((Control) this.lblRequiredCureAmount);
      this.Controls.Add((Control) this.lblAppliedCureAmount);
      this.Controls.Add((Control) this.txtAppliedCureAmount);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.dpDateResolved);
      this.Controls.Add((Control) this.lblDateResolved);
      this.Controls.Add((Control) this.txtReason);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (GoodFaithViolationResolutionDialog);
      this.Text = "Good Faith Fee Variance Resolution";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
