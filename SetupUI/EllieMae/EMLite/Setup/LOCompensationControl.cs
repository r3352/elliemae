// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompensationControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompensationControl : SettingsUserControl
  {
    private LOCompensationSetting setting;
    private IContainer components;
    private Label label1;
    private ComboBox cboRule;
    private GroupContainer grpFields;
    private CheckBox chkUser1;
    private CheckBox chkCompensation;
    private CheckBox chkBroker;
    private CheckBox chkUnderwriting;
    private CheckBox chkProcessing;
    private CheckBox chkApplication;
    private CheckBox chkLoanOrigination;
    private CheckBox chkUser4;
    private CheckBox chkUser3;
    private CheckBox chkUser2;
    private CheckBox chkUser5;
    private GroupContainer groupContainer2;
    private Label label2;
    private CheckBox chkOriginationPoints;
    private Label label3;
    private RadioButton rdo3rdParty;
    private RadioButton rdoBorrower;
    private Panel panelSellerPaid;
    private CheckBox chkUser12;
    private CheckBox chkUser11;
    private CheckBox chkUser10;
    private CheckBox chkUser9;
    private CheckBox chkUser8;
    private CheckBox chkUser7;
    private CheckBox chkUser6;

    public LOCompensationControl(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.Reset();
    }

    public override void Reset()
    {
      this.chkLoanOrigination.Checked = this.chkApplication.Checked = this.chkProcessing.Checked = false;
      this.chkUnderwriting.Checked = this.chkUser1.Checked = this.chkUser2.Checked = false;
      this.chkUser3.Checked = this.chkUser4.Checked = this.chkUser5.Checked = false;
      this.setting = new LOCompensationSetting(Session.ConfigurationManager.GetCompanySetting("LOCompensation", "Rule"));
      switch (this.setting.LOAction)
      {
        case LOCompensationSetting.LOActions.Fix:
          this.cboRule.SelectedIndex = 0;
          break;
        case LOCompensationSetting.LOActions.WarningOnly:
          this.cboRule.SelectedIndex = 1;
          break;
        case LOCompensationSetting.LOActions.NoAction:
          this.cboRule.SelectedIndex = 2;
          break;
      }
      this.chkLoanOrigination.Checked = this.setting.IsLineItemEnabled("a");
      this.chkApplication.Checked = this.setting.IsLineItemEnabled("b");
      this.chkProcessing.Checked = this.setting.IsLineItemEnabled("c");
      this.chkUnderwriting.Checked = this.setting.IsLineItemEnabled("d");
      this.chkUser1.Checked = this.setting.IsLineItemEnabled("g");
      this.chkUser2.Checked = this.setting.IsLineItemEnabled("h");
      this.chkUser3.Checked = this.setting.IsLineItemEnabled("i");
      this.chkUser4.Checked = this.setting.IsLineItemEnabled("j");
      this.chkUser5.Checked = this.setting.IsLineItemEnabled("k");
      this.chkUser6.Checked = this.setting.IsLineItemEnabled("l");
      this.chkUser7.Checked = this.setting.IsLineItemEnabled("m");
      this.chkUser8.Checked = this.setting.IsLineItemEnabled("n");
      this.chkUser9.Checked = this.setting.IsLineItemEnabled("o");
      this.chkUser10.Checked = this.setting.IsLineItemEnabled("p");
      this.chkUser11.Checked = this.setting.IsLineItemEnabled("q");
      this.chkUser12.Checked = this.setting.IsLineItemEnabled("r");
      this.chkOriginationPoints.Checked = this.setting.IsLineItemEnabled("s");
      this.rdoBorrower.Checked = this.setting.UseSellerForBorrower;
      this.rdo3rdParty.Checked = !this.setting.UseSellerForBorrower;
      this.cboRule_SelectedIndexChanged((object) null, (EventArgs) null);
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      string empty = string.Empty;
      int selectedIndex = this.cboRule.SelectedIndex;
      string str = selectedIndex != 2 ? string.Concat((object) (selectedIndex + 1)) : "0";
      for (int index = 1; index <= 16; ++index)
      {
        switch (index)
        {
          case 1:
            str += this.chkLoanOrigination.Checked ? ",a" : "";
            break;
          case 2:
            str += this.chkApplication.Checked ? ",b" : "";
            break;
          case 3:
            str += this.chkProcessing.Checked ? ",c" : "";
            break;
          case 4:
            str += this.chkUnderwriting.Checked ? ",d" : "";
            break;
          case 5:
            str += this.chkUser1.Checked ? ",g" : "";
            break;
          case 6:
            str += this.chkUser2.Checked ? ",h" : "";
            break;
          case 7:
            str += this.chkUser3.Checked ? ",i" : "";
            break;
          case 8:
            str += this.chkUser4.Checked ? ",j" : "";
            break;
          case 9:
            str += this.chkUser5.Checked ? ",k" : "";
            break;
          case 10:
            str += this.chkUser6.Checked ? ",l" : "";
            break;
          case 11:
            str += this.chkUser7.Checked ? ",m" : "";
            break;
          case 12:
            str += this.chkUser8.Checked ? ",n" : "";
            break;
          case 13:
            str += this.chkUser9.Checked ? ",o" : "";
            break;
          case 14:
            str += this.chkUser10.Checked ? ",p" : "";
            break;
          case 15:
            str += this.chkUser11.Checked ? ",q" : "";
            break;
          case 16:
            str += this.chkUser12.Checked ? ",r" : "";
            break;
          case 17:
            str += this.chkOriginationPoints.Checked ? ",s" : "";
            break;
        }
      }
      if (this.rdoBorrower.Checked)
        str += ",3b";
      Session.ConfigurationManager.SetCompanySetting("LOCompensation", "Rule", str);
      this.setDirtyFlag(false);
    }

    private void cboRule_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.grpFields.Visible = this.panelSellerPaid.Visible = this.cboRule.SelectedIndex == 0 || this.cboRule.SelectedIndex == 1;
      this.setDirtyFlag(true);
    }

    private void compensationField_Changed(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void rdoBorrower_Click(object sender, EventArgs e)
    {
      if (this.rdoBorrower.Checked && Utils.Dialog((IWin32Window) this, "According to Regulation Z, 12 CFR 1026.36 (d)(2), Loan Originator Compensation must come directly from one source. If the Loan Originator is compensated directly by the Consumer, the Loan Originator cannot be compensated by any other person/entity. You are electing to treat Loan Originator compensation paid by the SELLER as if it were paid by the CONSUMER (BORROWER). If you continue, you may have loans in violation of Regulation Z, 12 CFR 1026.36 (d)(2). Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
      {
        this.rdoBorrower.Checked = false;
        this.rdo3rdParty.Checked = true;
      }
      if ((!this.setting.UseSellerForBorrower || !this.rdo3rdParty.Checked) && (this.setting.UseSellerForBorrower || !this.rdoBorrower.Checked))
        return;
      this.setDirtyFlag(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.cboRule = new ComboBox();
      this.grpFields = new GroupContainer();
      this.chkUser12 = new CheckBox();
      this.chkUser11 = new CheckBox();
      this.chkUser10 = new CheckBox();
      this.chkUser9 = new CheckBox();
      this.chkUser8 = new CheckBox();
      this.chkUser7 = new CheckBox();
      this.chkUser6 = new CheckBox();
      this.chkOriginationPoints = new CheckBox();
      this.chkUser5 = new CheckBox();
      this.chkUser4 = new CheckBox();
      this.chkUser3 = new CheckBox();
      this.chkUser2 = new CheckBox();
      this.chkUser1 = new CheckBox();
      this.chkCompensation = new CheckBox();
      this.chkBroker = new CheckBox();
      this.chkUnderwriting = new CheckBox();
      this.chkProcessing = new CheckBox();
      this.chkApplication = new CheckBox();
      this.chkLoanOrigination = new CheckBox();
      this.groupContainer2 = new GroupContainer();
      this.panelSellerPaid = new Panel();
      this.rdo3rdParty = new RadioButton();
      this.label3 = new Label();
      this.rdoBorrower = new RadioButton();
      this.label2 = new Label();
      this.grpFields.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.panelSellerPaid.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 67);
      this.label1.Name = "label1";
      this.label1.Size = new Size(107, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select Control Option";
      this.cboRule.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRule.FormattingEnabled = true;
      this.cboRule.Items.AddRange(new object[3]
      {
        (object) "Enforce compliance to the LO Compensation paid-by rule",
        (object) "Display warning when all compensation fields are not compliant",
        (object) "Do not enforce compliance or display a warning"
      });
      this.cboRule.Location = new Point(129, 64);
      this.cboRule.Name = "cboRule";
      this.cboRule.Size = new Size(465, 21);
      this.cboRule.TabIndex = 1;
      this.cboRule.SelectedIndexChanged += new EventHandler(this.cboRule_SelectedIndexChanged);
      this.grpFields.Controls.Add((Control) this.chkUser12);
      this.grpFields.Controls.Add((Control) this.chkUser11);
      this.grpFields.Controls.Add((Control) this.chkUser10);
      this.grpFields.Controls.Add((Control) this.chkUser9);
      this.grpFields.Controls.Add((Control) this.chkUser8);
      this.grpFields.Controls.Add((Control) this.chkUser7);
      this.grpFields.Controls.Add((Control) this.chkUser6);
      this.grpFields.Controls.Add((Control) this.chkOriginationPoints);
      this.grpFields.Controls.Add((Control) this.chkUser5);
      this.grpFields.Controls.Add((Control) this.chkUser4);
      this.grpFields.Controls.Add((Control) this.chkUser3);
      this.grpFields.Controls.Add((Control) this.chkUser2);
      this.grpFields.Controls.Add((Control) this.chkUser1);
      this.grpFields.Controls.Add((Control) this.chkCompensation);
      this.grpFields.Controls.Add((Control) this.chkBroker);
      this.grpFields.Controls.Add((Control) this.chkUnderwriting);
      this.grpFields.Controls.Add((Control) this.chkProcessing);
      this.grpFields.Controls.Add((Control) this.chkApplication);
      this.grpFields.Controls.Add((Control) this.chkLoanOrigination);
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(19, 91);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(575, 411);
      this.grpFields.TabIndex = 2;
      this.grpFields.Text = "Select the Compensation Fields from the Itemization";
      this.chkUser12.AutoSize = true;
      this.chkUser12.Location = new Point(15, 382);
      this.chkUser12.Name = "chkUser12";
      this.chkUser12.Size = new Size(133, 17);
      this.chkUser12.TabIndex = 20;
      this.chkUser12.Text = "801 r. User Defined 12";
      this.chkUser12.UseVisualStyleBackColor = true;
      this.chkUser12.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser11.AutoSize = true;
      this.chkUser11.Location = new Point(15, 362);
      this.chkUser11.Name = "chkUser11";
      this.chkUser11.Size = new Size(136, 17);
      this.chkUser11.TabIndex = 19;
      this.chkUser11.Text = "801 q. User Defined 11";
      this.chkUser11.UseVisualStyleBackColor = true;
      this.chkUser11.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser10.AutoSize = true;
      this.chkUser10.Location = new Point(15, 342);
      this.chkUser10.Name = "chkUser10";
      this.chkUser10.Size = new Size(136, 17);
      this.chkUser10.TabIndex = 18;
      this.chkUser10.Text = "801 p. User Defined 10";
      this.chkUser10.UseVisualStyleBackColor = true;
      this.chkUser10.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser9.AutoSize = true;
      this.chkUser9.Location = new Point(15, 322);
      this.chkUser9.Name = "chkUser9";
      this.chkUser9.Size = new Size(130, 17);
      this.chkUser9.TabIndex = 17;
      this.chkUser9.Text = "801 o. User Defined 9";
      this.chkUser9.UseVisualStyleBackColor = true;
      this.chkUser9.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser8.AutoSize = true;
      this.chkUser8.Location = new Point(15, 302);
      this.chkUser8.Name = "chkUser8";
      this.chkUser8.Size = new Size(130, 17);
      this.chkUser8.TabIndex = 16;
      this.chkUser8.Text = "801 n. User Defined 8";
      this.chkUser8.UseVisualStyleBackColor = true;
      this.chkUser8.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser7.AutoSize = true;
      this.chkUser7.Location = new Point(15, 282);
      this.chkUser7.Name = "chkUser7";
      this.chkUser7.Size = new Size(132, 17);
      this.chkUser7.TabIndex = 15;
      this.chkUser7.Text = "801 m. User Defined 7";
      this.chkUser7.UseVisualStyleBackColor = true;
      this.chkUser7.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser6.AutoSize = true;
      this.chkUser6.Location = new Point(15, 262);
      this.chkUser6.Name = "chkUser6";
      this.chkUser6.Size = new Size(126, 17);
      this.chkUser6.TabIndex = 14;
      this.chkUser6.Text = "801 l. User Defined 6";
      this.chkUser6.UseVisualStyleBackColor = true;
      this.chkUser6.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkOriginationPoints.AutoSize = true;
      this.chkOriginationPoints.Location = new Point(382, 131);
      this.chkOriginationPoints.Name = "chkOriginationPoints";
      this.chkOriginationPoints.Size = new Size(132, 17);
      this.chkOriginationPoints.TabIndex = 23;
      this.chkOriginationPoints.Text = "802. Origination Points";
      this.chkOriginationPoints.UseVisualStyleBackColor = true;
      this.chkOriginationPoints.Visible = false;
      this.chkOriginationPoints.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser5.AutoSize = true;
      this.chkUser5.Location = new Point(15, 242);
      this.chkUser5.Name = "chkUser5";
      this.chkUser5.Size = new Size(130, 17);
      this.chkUser5.TabIndex = 13;
      this.chkUser5.Text = "801 k. User Defined 5";
      this.chkUser5.UseVisualStyleBackColor = true;
      this.chkUser5.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser4.AutoSize = true;
      this.chkUser4.Location = new Point(15, 222);
      this.chkUser4.Name = "chkUser4";
      this.chkUser4.Size = new Size(126, 17);
      this.chkUser4.TabIndex = 12;
      this.chkUser4.Text = "801 j. User Defined 4";
      this.chkUser4.UseVisualStyleBackColor = true;
      this.chkUser4.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser3.AutoSize = true;
      this.chkUser3.Location = new Point(15, 202);
      this.chkUser3.Name = "chkUser3";
      this.chkUser3.Size = new Size(126, 17);
      this.chkUser3.TabIndex = 11;
      this.chkUser3.Text = "801 i. User Defined 3";
      this.chkUser3.UseVisualStyleBackColor = true;
      this.chkUser3.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser2.AutoSize = true;
      this.chkUser2.Location = new Point(15, 182);
      this.chkUser2.Name = "chkUser2";
      this.chkUser2.Size = new Size(130, 17);
      this.chkUser2.TabIndex = 10;
      this.chkUser2.Text = "801 h. User Defined 2";
      this.chkUser2.UseVisualStyleBackColor = true;
      this.chkUser2.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkUser1.AutoSize = true;
      this.chkUser1.Location = new Point(15, 162);
      this.chkUser1.Name = "chkUser1";
      this.chkUser1.Size = new Size(130, 17);
      this.chkUser1.TabIndex = 9;
      this.chkUser1.Text = "801 g. User Defined 1";
      this.chkUser1.UseVisualStyleBackColor = true;
      this.chkUser1.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkCompensation.AutoSize = true;
      this.chkCompensation.Checked = true;
      this.chkCompensation.CheckState = CheckState.Checked;
      this.chkCompensation.Enabled = false;
      this.chkCompensation.Location = new Point(15, 142);
      this.chkCompensation.Name = "chkCompensation";
      this.chkCompensation.Size = new Size(157, 17);
      this.chkCompensation.TabIndex = 8;
      this.chkCompensation.TabStop = false;
      this.chkCompensation.Text = "801 f. Broker Compensation";
      this.chkCompensation.UseVisualStyleBackColor = true;
      this.chkBroker.AutoSize = true;
      this.chkBroker.Checked = true;
      this.chkBroker.CheckState = CheckState.Checked;
      this.chkBroker.Enabled = false;
      this.chkBroker.Location = new Point(15, 122);
      this.chkBroker.Name = "chkBroker";
      this.chkBroker.Size = new Size(116, 17);
      this.chkBroker.TabIndex = 7;
      this.chkBroker.TabStop = false;
      this.chkBroker.Text = "801 e. Broker Fees";
      this.chkBroker.UseVisualStyleBackColor = true;
      this.chkUnderwriting.AutoSize = true;
      this.chkUnderwriting.Location = new Point(15, 102);
      this.chkUnderwriting.Name = "chkUnderwriting";
      this.chkUnderwriting.Size = new Size(144, 17);
      this.chkUnderwriting.TabIndex = 6;
      this.chkUnderwriting.Text = "801 d. Underwriting Fees";
      this.chkUnderwriting.UseVisualStyleBackColor = true;
      this.chkUnderwriting.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkProcessing.AutoSize = true;
      this.chkProcessing.Location = new Point(15, 82);
      this.chkProcessing.Name = "chkProcessing";
      this.chkProcessing.Size = new Size(137, 17);
      this.chkProcessing.TabIndex = 5;
      this.chkProcessing.Text = "801 c. Processing Fees";
      this.chkProcessing.UseVisualStyleBackColor = true;
      this.chkProcessing.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkApplication.AutoSize = true;
      this.chkApplication.Location = new Point(15, 62);
      this.chkApplication.Name = "chkApplication";
      this.chkApplication.Size = new Size(137, 17);
      this.chkApplication.TabIndex = 4;
      this.chkApplication.Text = "801 b. Application Fees";
      this.chkApplication.UseVisualStyleBackColor = true;
      this.chkApplication.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.chkLoanOrigination.AutoSize = true;
      this.chkLoanOrigination.Location = new Point(15, 42);
      this.chkLoanOrigination.Name = "chkLoanOrigination";
      this.chkLoanOrigination.Size = new Size(157, 17);
      this.chkLoanOrigination.TabIndex = 3;
      this.chkLoanOrigination.Text = "801 a. Loan Origination Fee";
      this.chkLoanOrigination.UseVisualStyleBackColor = true;
      this.chkLoanOrigination.CheckedChanged += new EventHandler(this.compensationField_Changed);
      this.groupContainer2.Controls.Add((Control) this.panelSellerPaid);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.grpFields);
      this.groupContainer2.Controls.Add((Control) this.cboRule);
      this.groupContainer2.Controls.Add((Control) this.label1);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(706, 553);
      this.groupContainer2.TabIndex = 3;
      this.groupContainer2.Text = "LO Compensation Rule Control";
      this.panelSellerPaid.Controls.Add((Control) this.rdo3rdParty);
      this.panelSellerPaid.Controls.Add((Control) this.label3);
      this.panelSellerPaid.Controls.Add((Control) this.rdoBorrower);
      this.panelSellerPaid.Location = new Point(20, 507);
      this.panelSellerPaid.Name = "panelSellerPaid";
      this.panelSellerPaid.Size = new Size(575, 35);
      this.panelSellerPaid.TabIndex = 21;
      this.rdo3rdParty.AutoSize = true;
      this.rdo3rdParty.Checked = true;
      this.rdo3rdParty.Location = new Point(302, 5);
      this.rdo3rdParty.Name = "rdo3rdParty";
      this.rdo3rdParty.Size = new Size(76, 17);
      this.rdo3rdParty.TabIndex = 22;
      this.rdo3rdParty.TabStop = true;
      this.rdo3rdParty.Text = "Third-Party";
      this.rdo3rdParty.UseVisualStyleBackColor = true;
      this.rdo3rdParty.Click += new EventHandler(this.rdoBorrower_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(198, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Treat seller-paid broker compensation as";
      this.rdoBorrower.AutoSize = true;
      this.rdoBorrower.Location = new Point(222, 5);
      this.rdoBorrower.Name = "rdoBorrower";
      this.rdoBorrower.Size = new Size(67, 17);
      this.rdoBorrower.TabIndex = 21;
      this.rdoBorrower.Text = "Borrower";
      this.rdoBorrower.UseVisualStyleBackColor = true;
      this.rdoBorrower.Click += new EventHandler(this.rdoBorrower_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(16, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(627, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "To be in compliance, if any compensation fields are paid by borrower, all other compensation fields must be paid by borrower as well.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Name = nameof (LOCompensationControl);
      this.Size = new Size(706, 553);
      this.grpFields.ResumeLayout(false);
      this.grpFields.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.panelSellerPaid.ResumeLayout(false);
      this.panelSellerPaid.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
