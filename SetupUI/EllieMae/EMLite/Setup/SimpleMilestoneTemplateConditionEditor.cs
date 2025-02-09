// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SimpleMilestoneTemplateConditionEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SimpleMilestoneTemplateConditionEditor : UserControl
  {
    private CheckBox[] channelCheckboxes;
    private CheckBox[] loanTypeCheckboxes;
    private CheckBox[] loanPurposeCheckboxes;
    private IContainer components;
    private GroupContainer grpSimple;
    private Panel pnlBasic;
    private BorderPanel pnlLoanPurposes;
    private TextBox txtOtherLoanPurpose;
    private CheckBox chkOtherLoanPurpose;
    private CheckBox chkContructionPerm;
    private CheckBox chkConstruction;
    private CheckBox chkNoCashOutRefi;
    private CheckBox chkCashOutRefi;
    private CheckBox chkPurchase;
    private CheckBox chkLoanPurpose;
    private BorderPanel pnlLoanTypes;
    private TextBox txtOtherLoanType;
    private CheckBox chkHELOC;
    private CheckBox chkOtherLoanType;
    private CheckBox chkUSDA;
    private CheckBox chkVA;
    private CheckBox chkFHA;
    private CheckBox chkConventional;
    private CheckBox chkLoanType;
    private BorderPanel pnlChannels;
    private CheckBox chkCorrespondent;
    private CheckBox chkBrokered;
    private CheckBox chkBankedWholesale;
    private CheckBox chkBankedRetail;
    private CheckBox chkChannel;
    private Label label2;

    public SimpleMilestoneTemplateConditionEditor()
    {
      this.InitializeComponent();
      this.channelCheckboxes = new CheckBox[4]
      {
        this.chkBankedRetail,
        this.chkBankedWholesale,
        this.chkBrokered,
        this.chkCorrespondent
      };
      this.loanTypeCheckboxes = new CheckBox[6]
      {
        this.chkConventional,
        this.chkFHA,
        this.chkVA,
        this.chkUSDA,
        this.chkOtherLoanType,
        this.chkHELOC
      };
      this.loanPurposeCheckboxes = new CheckBox[6]
      {
        this.chkPurchase,
        this.chkCashOutRefi,
        this.chkNoCashOutRefi,
        this.chkConstruction,
        this.chkContructionPerm,
        this.chkOtherLoanPurpose
      };
      this.chkChannel.Checked = false;
      this.chkLoanType.Checked = false;
      this.chkLoanPurpose.Checked = false;
    }

    public void SetTemplateCondition(MilestoneTemplateSimpleCondition condition)
    {
      this.chkChannel.Checked = condition.LoanChannels.Count > 0;
      foreach (CheckBox channelCheckbox in this.channelCheckboxes)
        channelCheckbox.Checked = condition.LoanChannels.Contains(channelCheckbox.Tag.ToString());
      this.chkLoanType.Checked = condition.LoanTypes.Count > 0;
      foreach (CheckBox loanTypeCheckbox in this.loanTypeCheckboxes)
        loanTypeCheckbox.Checked = condition.LoanTypes.Contains(loanTypeCheckbox.Tag.ToString());
      this.chkLoanPurpose.Checked = condition.LoanPurposes.Count > 0;
      foreach (CheckBox loanPurposeCheckbox in this.loanPurposeCheckboxes)
        loanPurposeCheckbox.Checked = condition.LoanPurposes.Contains(loanPurposeCheckbox.Tag.ToString());
    }

    public MilestoneTemplateSimpleCondition GetTemplateCondition()
    {
      MilestoneTemplateSimpleCondition templateCondition = new MilestoneTemplateSimpleCondition();
      if (this.chkChannel.Checked)
      {
        foreach (CheckBox channelCheckbox in this.channelCheckboxes)
        {
          if (channelCheckbox.Checked)
            templateCondition.LoanChannels.Add(channelCheckbox.Tag.ToString());
        }
      }
      if (this.chkLoanPurpose.Checked)
      {
        foreach (CheckBox loanPurposeCheckbox in this.loanPurposeCheckboxes)
        {
          if (loanPurposeCheckbox.Checked)
            templateCondition.LoanPurposes.Add(loanPurposeCheckbox.Tag.ToString());
        }
      }
      if (this.chkLoanType.Checked)
      {
        foreach (CheckBox loanTypeCheckbox in this.loanTypeCheckboxes)
        {
          if (loanTypeCheckbox.Checked)
            templateCondition.LoanTypes.Add(loanTypeCheckbox.Tag.ToString());
        }
      }
      return templateCondition;
    }

    private void chkChannel_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chkChannel.Checked)
      {
        foreach (CheckBox channelCheckbox in this.channelCheckboxes)
          channelCheckbox.Checked = false;
      }
      this.pnlChannels.Enabled = this.chkChannel.Checked;
    }

    private void chkLoanType_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chkLoanType.Checked)
      {
        foreach (CheckBox loanTypeCheckbox in this.loanTypeCheckboxes)
          loanTypeCheckbox.Checked = false;
      }
      this.pnlLoanTypes.Enabled = this.chkLoanType.Checked;
    }

    private void chkLoanPurpose_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chkLoanPurpose.Checked)
      {
        foreach (CheckBox loanPurposeCheckbox in this.loanPurposeCheckboxes)
          loanPurposeCheckbox.Checked = false;
      }
      this.pnlLoanPurposes.Enabled = this.chkLoanPurpose.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpSimple = new GroupContainer();
      this.pnlBasic = new Panel();
      this.pnlLoanPurposes = new BorderPanel();
      this.txtOtherLoanPurpose = new TextBox();
      this.chkOtherLoanPurpose = new CheckBox();
      this.chkContructionPerm = new CheckBox();
      this.chkConstruction = new CheckBox();
      this.chkNoCashOutRefi = new CheckBox();
      this.chkCashOutRefi = new CheckBox();
      this.chkPurchase = new CheckBox();
      this.chkLoanPurpose = new CheckBox();
      this.pnlLoanTypes = new BorderPanel();
      this.txtOtherLoanType = new TextBox();
      this.chkHELOC = new CheckBox();
      this.chkOtherLoanType = new CheckBox();
      this.chkUSDA = new CheckBox();
      this.chkVA = new CheckBox();
      this.chkFHA = new CheckBox();
      this.chkConventional = new CheckBox();
      this.chkLoanType = new CheckBox();
      this.pnlChannels = new BorderPanel();
      this.chkCorrespondent = new CheckBox();
      this.chkBrokered = new CheckBox();
      this.chkBankedWholesale = new CheckBox();
      this.chkBankedRetail = new CheckBox();
      this.chkChannel = new CheckBox();
      this.label2 = new Label();
      this.grpSimple.SuspendLayout();
      this.pnlBasic.SuspendLayout();
      this.pnlLoanPurposes.SuspendLayout();
      this.pnlLoanTypes.SuspendLayout();
      this.pnlChannels.SuspendLayout();
      this.SuspendLayout();
      this.grpSimple.Controls.Add((Control) this.pnlBasic);
      this.grpSimple.Controls.Add((Control) this.label2);
      this.grpSimple.Dock = DockStyle.Fill;
      this.grpSimple.HeaderForeColor = SystemColors.ControlText;
      this.grpSimple.Location = new Point(0, 0);
      this.grpSimple.Name = "grpSimple";
      this.grpSimple.Size = new Size(561, 198);
      this.grpSimple.TabIndex = 4;
      this.grpSimple.Text = "Condition";
      this.pnlBasic.Controls.Add((Control) this.pnlLoanPurposes);
      this.pnlBasic.Controls.Add((Control) this.chkLoanPurpose);
      this.pnlBasic.Controls.Add((Control) this.pnlLoanTypes);
      this.pnlBasic.Controls.Add((Control) this.chkLoanType);
      this.pnlBasic.Controls.Add((Control) this.pnlChannels);
      this.pnlBasic.Controls.Add((Control) this.chkChannel);
      this.pnlBasic.Dock = DockStyle.Fill;
      this.pnlBasic.Location = new Point(1, 26);
      this.pnlBasic.Name = "pnlBasic";
      this.pnlBasic.Size = new Size(559, 171);
      this.pnlBasic.TabIndex = 0;
      this.pnlLoanPurposes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlLoanPurposes.BackColor = Color.White;
      this.pnlLoanPurposes.Controls.Add((Control) this.txtOtherLoanPurpose);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkOtherLoanPurpose);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkContructionPerm);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkConstruction);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkNoCashOutRefi);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkCashOutRefi);
      this.pnlLoanPurposes.Controls.Add((Control) this.chkPurchase);
      this.pnlLoanPurposes.Location = new Point(376, 33);
      this.pnlLoanPurposes.Name = "pnlLoanPurposes";
      this.pnlLoanPurposes.Size = new Size(173, 128);
      this.pnlLoanPurposes.TabIndex = 5;
      this.txtOtherLoanPurpose.Enabled = false;
      this.txtOtherLoanPurpose.Location = new Point(68, 97);
      this.txtOtherLoanPurpose.Name = "txtOtherLoanPurpose";
      this.txtOtherLoanPurpose.Size = new Size(70, 20);
      this.txtOtherLoanPurpose.TabIndex = 8;
      this.chkOtherLoanPurpose.AutoSize = true;
      this.chkOtherLoanPurpose.Location = new Point(11, 99);
      this.chkOtherLoanPurpose.Name = "chkOtherLoanPurpose";
      this.chkOtherLoanPurpose.Size = new Size(58, 17);
      this.chkOtherLoanPurpose.TabIndex = 6;
      this.chkOtherLoanPurpose.Tag = (object) "Other";
      this.chkOtherLoanPurpose.Text = "Other -";
      this.chkOtherLoanPurpose.UseVisualStyleBackColor = true;
      this.chkContructionPerm.AutoSize = true;
      this.chkContructionPerm.Location = new Point(11, 81);
      this.chkContructionPerm.Name = "chkContructionPerm";
      this.chkContructionPerm.Size = new Size(118, 17);
      this.chkContructionPerm.TabIndex = 5;
      this.chkContructionPerm.Tag = (object) "ConstructionToPermanent";
      this.chkContructionPerm.Text = "Construction - Perm";
      this.chkContructionPerm.UseVisualStyleBackColor = true;
      this.chkConstruction.AutoSize = true;
      this.chkConstruction.Location = new Point(11, 63);
      this.chkConstruction.Name = "chkConstruction";
      this.chkConstruction.Size = new Size(85, 17);
      this.chkConstruction.TabIndex = 4;
      this.chkConstruction.Tag = (object) "ConstructionOnly";
      this.chkConstruction.Text = "Construction";
      this.chkConstruction.UseVisualStyleBackColor = true;
      this.chkNoCashOutRefi.AutoSize = true;
      this.chkNoCashOutRefi.Location = new Point(11, 45);
      this.chkNoCashOutRefi.Name = "chkNoCashOutRefi";
      this.chkNoCashOutRefi.Size = new Size(109, 17);
      this.chkNoCashOutRefi.TabIndex = 3;
      this.chkNoCashOutRefi.Tag = (object) "NoCash-Out Refinance";
      this.chkNoCashOutRefi.Text = "No Cash-Out Refi";
      this.chkNoCashOutRefi.UseVisualStyleBackColor = true;
      this.chkCashOutRefi.AutoSize = true;
      this.chkCashOutRefi.Location = new Point(11, 27);
      this.chkCashOutRefi.Name = "chkCashOutRefi";
      this.chkCashOutRefi.Size = new Size(92, 17);
      this.chkCashOutRefi.TabIndex = 2;
      this.chkCashOutRefi.Tag = (object) "Cash-Out Refinance";
      this.chkCashOutRefi.Text = "Cash-Out Refi";
      this.chkCashOutRefi.UseVisualStyleBackColor = true;
      this.chkPurchase.AutoSize = true;
      this.chkPurchase.Location = new Point(11, 9);
      this.chkPurchase.Name = "chkPurchase";
      this.chkPurchase.Size = new Size(71, 17);
      this.chkPurchase.TabIndex = 1;
      this.chkPurchase.Tag = (object) "Purchase";
      this.chkPurchase.Text = "Purchase";
      this.chkPurchase.UseVisualStyleBackColor = true;
      this.chkLoanPurpose.AutoSize = true;
      this.chkLoanPurpose.Checked = true;
      this.chkLoanPurpose.CheckState = CheckState.Checked;
      this.chkLoanPurpose.Location = new Point(376, 8);
      this.chkLoanPurpose.Name = "chkLoanPurpose";
      this.chkLoanPurpose.Size = new Size(92, 17);
      this.chkLoanPurpose.TabIndex = 4;
      this.chkLoanPurpose.Text = "Loan Purpose";
      this.chkLoanPurpose.UseVisualStyleBackColor = true;
      this.chkLoanPurpose.CheckedChanged += new EventHandler(this.chkLoanPurpose_CheckedChanged);
      this.pnlLoanTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlLoanTypes.BackColor = Color.White;
      this.pnlLoanTypes.Controls.Add((Control) this.txtOtherLoanType);
      this.pnlLoanTypes.Controls.Add((Control) this.chkHELOC);
      this.pnlLoanTypes.Controls.Add((Control) this.chkOtherLoanType);
      this.pnlLoanTypes.Controls.Add((Control) this.chkUSDA);
      this.pnlLoanTypes.Controls.Add((Control) this.chkVA);
      this.pnlLoanTypes.Controls.Add((Control) this.chkFHA);
      this.pnlLoanTypes.Controls.Add((Control) this.chkConventional);
      this.pnlLoanTypes.Location = new Point(193, 33);
      this.pnlLoanTypes.Name = "pnlLoanTypes";
      this.pnlLoanTypes.Size = new Size(173, 128);
      this.pnlLoanTypes.TabIndex = 3;
      this.txtOtherLoanType.Enabled = false;
      this.txtOtherLoanType.Location = new Point(68, 79);
      this.txtOtherLoanType.Name = "txtOtherLoanType";
      this.txtOtherLoanType.Size = new Size(70, 20);
      this.txtOtherLoanType.TabIndex = 7;
      this.chkHELOC.AutoSize = true;
      this.chkHELOC.Location = new Point(11, 99);
      this.chkHELOC.Name = "chkHELOC";
      this.chkHELOC.Size = new Size(62, 17);
      this.chkHELOC.TabIndex = 6;
      this.chkHELOC.Tag = (object) "HELOC";
      this.chkHELOC.Text = "HELOC";
      this.chkHELOC.UseVisualStyleBackColor = true;
      this.chkOtherLoanType.AutoSize = true;
      this.chkOtherLoanType.Location = new Point(11, 81);
      this.chkOtherLoanType.Name = "chkOtherLoanType";
      this.chkOtherLoanType.Size = new Size(58, 17);
      this.chkOtherLoanType.TabIndex = 5;
      this.chkOtherLoanType.Tag = (object) "Other";
      this.chkOtherLoanType.Text = "Other -";
      this.chkOtherLoanType.UseVisualStyleBackColor = true;
      this.chkUSDA.AutoSize = true;
      this.chkUSDA.Location = new Point(11, 63);
      this.chkUSDA.Name = "chkUSDA";
      this.chkUSDA.Size = new Size(82, 17);
      this.chkUSDA.TabIndex = 4;
      this.chkUSDA.Tag = (object) "FarmersHomeAdministration";
      this.chkUSDA.Text = "USDA-RHS";
      this.chkUSDA.UseVisualStyleBackColor = true;
      this.chkVA.AutoSize = true;
      this.chkVA.Location = new Point(11, 45);
      this.chkVA.Name = "chkVA";
      this.chkVA.Size = new Size(40, 17);
      this.chkVA.TabIndex = 3;
      this.chkVA.Tag = (object) "VA";
      this.chkVA.Text = "VA";
      this.chkVA.UseVisualStyleBackColor = true;
      this.chkFHA.AutoSize = true;
      this.chkFHA.Location = new Point(11, 27);
      this.chkFHA.Name = "chkFHA";
      this.chkFHA.Size = new Size(47, 17);
      this.chkFHA.TabIndex = 2;
      this.chkFHA.Tag = (object) "FHA";
      this.chkFHA.Text = "FHA";
      this.chkFHA.UseVisualStyleBackColor = true;
      this.chkConventional.AutoSize = true;
      this.chkConventional.Location = new Point(11, 9);
      this.chkConventional.Name = "chkConventional";
      this.chkConventional.Size = new Size(88, 17);
      this.chkConventional.TabIndex = 1;
      this.chkConventional.Tag = (object) "Conventional";
      this.chkConventional.Text = "Conventional";
      this.chkConventional.UseVisualStyleBackColor = true;
      this.chkLoanType.AutoSize = true;
      this.chkLoanType.Checked = true;
      this.chkLoanType.CheckState = CheckState.Checked;
      this.chkLoanType.Location = new Point(193, 8);
      this.chkLoanType.Name = "chkLoanType";
      this.chkLoanType.Size = new Size(77, 17);
      this.chkLoanType.TabIndex = 2;
      this.chkLoanType.Text = "Loan Type";
      this.chkLoanType.UseVisualStyleBackColor = true;
      this.chkLoanType.CheckedChanged += new EventHandler(this.chkLoanType_CheckedChanged);
      this.pnlChannels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlChannels.BackColor = Color.White;
      this.pnlChannels.Controls.Add((Control) this.chkCorrespondent);
      this.pnlChannels.Controls.Add((Control) this.chkBrokered);
      this.pnlChannels.Controls.Add((Control) this.chkBankedWholesale);
      this.pnlChannels.Controls.Add((Control) this.chkBankedRetail);
      this.pnlChannels.Location = new Point(10, 33);
      this.pnlChannels.Name = "pnlChannels";
      this.pnlChannels.Size = new Size(173, 128);
      this.pnlChannels.TabIndex = 1;
      this.chkCorrespondent.AutoSize = true;
      this.chkCorrespondent.Location = new Point(11, 63);
      this.chkCorrespondent.Name = "chkCorrespondent";
      this.chkCorrespondent.Size = new Size(95, 17);
      this.chkCorrespondent.TabIndex = 4;
      this.chkCorrespondent.Tag = (object) "Correspondent";
      this.chkCorrespondent.Text = "Correspondent";
      this.chkCorrespondent.UseVisualStyleBackColor = true;
      this.chkBrokered.AutoSize = true;
      this.chkBrokered.Location = new Point(11, 45);
      this.chkBrokered.Name = "chkBrokered";
      this.chkBrokered.Size = new Size(69, 17);
      this.chkBrokered.TabIndex = 3;
      this.chkBrokered.Tag = (object) "Brokered";
      this.chkBrokered.Text = "Brokered";
      this.chkBrokered.UseVisualStyleBackColor = true;
      this.chkBankedWholesale.AutoSize = true;
      this.chkBankedWholesale.Location = new Point(11, 27);
      this.chkBankedWholesale.Name = "chkBankedWholesale";
      this.chkBankedWholesale.Size = new Size(122, 17);
      this.chkBankedWholesale.TabIndex = 2;
      this.chkBankedWholesale.Tag = (object) "Banked - Wholesale";
      this.chkBankedWholesale.Text = "Banked - Wholesale";
      this.chkBankedWholesale.UseVisualStyleBackColor = true;
      this.chkBankedRetail.AutoSize = true;
      this.chkBankedRetail.Location = new Point(11, 9);
      this.chkBankedRetail.Name = "chkBankedRetail";
      this.chkBankedRetail.Size = new Size(99, 17);
      this.chkBankedRetail.TabIndex = 1;
      this.chkBankedRetail.Tag = (object) "Banked - Retail";
      this.chkBankedRetail.Text = "Banked - Retail";
      this.chkBankedRetail.UseVisualStyleBackColor = true;
      this.chkChannel.AutoSize = true;
      this.chkChannel.Checked = true;
      this.chkChannel.CheckState = CheckState.Checked;
      this.chkChannel.Location = new Point(10, 8);
      this.chkChannel.Name = "chkChannel";
      this.chkChannel.Size = new Size(65, 17);
      this.chkChannel.TabIndex = 0;
      this.chkChannel.Text = "Channel";
      this.chkChannel.UseVisualStyleBackColor = true;
      this.chkChannel.CheckedChanged += new EventHandler(this.chkChannel_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Condition";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpSimple);
      this.Name = nameof (SimpleMilestoneTemplateConditionEditor);
      this.Size = new Size(561, 198);
      this.grpSimple.ResumeLayout(false);
      this.grpSimple.PerformLayout();
      this.pnlBasic.ResumeLayout(false);
      this.pnlBasic.PerformLayout();
      this.pnlLoanPurposes.ResumeLayout(false);
      this.pnlLoanPurposes.PerformLayout();
      this.pnlLoanTypes.ResumeLayout(false);
      this.pnlLoanTypes.PerformLayout();
      this.pnlChannels.ResumeLayout(false);
      this.pnlChannels.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
