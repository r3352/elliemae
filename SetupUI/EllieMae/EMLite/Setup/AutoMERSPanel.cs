// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoMERSPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoMERSPanel : SettingsUserControl
  {
    private Label label2;
    private Label label1;
    private Label label;
    private TextBox orgIDTxt;
    private CheckBox autoChk;
    private TextBox nextNumTxt;
    private Button btnBranchMERS;
    private bool initAutoGeneration;
    private string initCompanyId;
    private string initNextNum;
    private System.ComponentModel.Container components;
    private GroupContainer groupContainer1;

    public AutoMERSPanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.orgIDTxt.MaxLength = 7;
      this.nextNumTxt.MaxLength = 10;
      this.Reset();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.label1 = new Label();
      this.nextNumTxt = new TextBox();
      this.label = new Label();
      this.orgIDTxt = new TextBox();
      this.autoChk = new CheckBox();
      this.btnBranchMERS = new Button();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label2.Location = new Point(7, 88);
      this.label2.Name = "label2";
      this.label2.Size = new Size(89, 20);
      this.label2.TabIndex = 16;
      this.label2.Text = "Next Number";
      this.label1.Location = new Point(7, 36);
      this.label1.Name = "label1";
      this.label1.Size = new Size(368, 16);
      this.label1.TabIndex = 15;
      this.label1.Text = "Enter the 7 digit Organization ID provided to you by MERS";
      this.label1.TextAlign = ContentAlignment.BottomLeft;
      this.nextNumTxt.Location = new Point(102, 85);
      this.nextNumTxt.Name = "nextNumTxt";
      this.nextNumTxt.Size = new Size(199, 20);
      this.nextNumTxt.TabIndex = 2;
      this.nextNumTxt.TextChanged += new EventHandler(this.nextNumTxt_TextChanged);
      this.nextNumTxt.KeyPress += new KeyPressEventHandler(this.nextNumTxt_keyPress);
      this.label.Location = new Point(7, 63);
      this.label.Name = "label";
      this.label.Size = new Size(89, 20);
      this.label.TabIndex = 13;
      this.label.Text = "Organization ID";
      this.orgIDTxt.Location = new Point(102, 60);
      this.orgIDTxt.Name = "orgIDTxt";
      this.orgIDTxt.Size = new Size(199, 20);
      this.orgIDTxt.TabIndex = 1;
      this.orgIDTxt.TextChanged += new EventHandler(this.orgIDTxt_TextChanged);
      this.orgIDTxt.KeyPress += new KeyPressEventHandler(this.orgIDTxt_keyPress);
      this.autoChk.Location = new Point(102, 111);
      this.autoChk.Name = "autoChk";
      this.autoChk.Size = new Size(199, 24);
      this.autoChk.TabIndex = 3;
      this.autoChk.Text = "Auto Create MERS MIN Numbers";
      this.autoChk.CheckedChanged += new EventHandler(this.autoChk_CheckedChanged);
      this.btnBranchMERS.Location = new Point(345, 2);
      this.btnBranchMERS.Name = "btnBranchMERS";
      this.btnBranchMERS.Size = new Size(130, 22);
      this.btnBranchMERS.TabIndex = 4;
      this.btnBranchMERS.Text = "MERS MIN Numbering";
      this.btnBranchMERS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBranchMERS.Click += new EventHandler(this.btnBranchMERS_Click);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.nextNumTxt);
      this.groupContainer1.Controls.Add((Control) this.orgIDTxt);
      this.groupContainer1.Controls.Add((Control) this.label);
      this.groupContainer1.Controls.Add((Control) this.autoChk);
      this.groupContainer1.Controls.Add((Control) this.btnBranchMERS);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(480, 237);
      this.groupContainer1.TabIndex = 20;
      this.groupContainer1.Text = "MERS MIN Numbering";
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AutoMERSPanel);
      this.Size = new Size(480, 237);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void btnBranchMERS_Click(object sender, EventArgs e)
    {
      int num = (int) new BranchMERSMINDialog().ShowDialog((IWin32Window) this);
    }

    private void keypress(object sender, KeyPressEventArgs e, int length)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        this.setDirtyFlag(true);
      else if (char.IsDigit(e.KeyChar))
      {
        if (((Control) sender).Text.Length >= length)
          return;
        this.setDirtyFlag(true);
      }
      else
        e.Handled = true;
    }

    private void orgIDTxt_keyPress(object sender, KeyPressEventArgs e)
    {
      this.keypress(sender, e, 7);
    }

    private void nextNumTxt_keyPress(object sender, KeyPressEventArgs e)
    {
      this.keypress(sender, e, 10);
    }

    public override void Save()
    {
      if (!this.IsDirty)
        this.Dispose();
      else if ((this.nextNumTxt.Text.Trim() != string.Empty || this.autoChk.Checked) && this.orgIDTxt.Text.Length < 7)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Organization ID must be seven (" + (object) 7 + ") digits.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if ((this.orgIDTxt.Text.Trim() != string.Empty || this.autoChk.Checked) && this.nextNumTxt.Text.Length < 10)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The 'Next Number' must be ten (" + (object) 10 + ") digits.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Session.ConfigurationManager.UpdateMersNumberingInfo(new MersNumberingInfo(this.autoChk.Checked, this.orgIDTxt.Text, this.nextNumTxt.Text));
        this.Reset();
      }
    }

    public override void Reset()
    {
      MersNumberingInfo mersNumberingInfo = Session.ConfigurationManager.GetMersNumberingInfo();
      this.initAutoGeneration = mersNumberingInfo.AutoGenerate;
      this.initCompanyId = mersNumberingInfo.CompanyID.Trim();
      this.initNextNum = mersNumberingInfo.NextNumber.Trim();
      this.autoChk.Checked = this.initAutoGeneration;
      this.orgIDTxt.Text = this.initCompanyId;
      this.nextNumTxt.Text = this.initNextNum;
      this.setDirtyFlag(false);
    }

    private void autoChk_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void orgIDTxt_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void nextNumTxt_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);
  }
}
