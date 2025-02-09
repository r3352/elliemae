// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SSOOrgConfirmationDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SSOOrgConfirmationDialog : Form, IHelp
  {
    private const string className = "SSOOrgConfirmationDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private GroupBox groupBox1;
    private Button btnCancel;
    private Button btnOk;
    private System.ComponentModel.Container components;
    public bool ApplyToAll;
    private RadioButton rbApplyToNonCustomUser;
    private RadioButton rbApplyAll;
    private Label label1;
    private Sessions.Session session;

    public SSOOrgConfirmationDialog(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.rbApplyToNonCustomUser = new RadioButton();
      this.rbApplyAll = new RadioButton();
      this.label1 = new Label();
      this.SuspendLayout();
      this.groupBox1.Location = new Point(14, 102);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(324, 10);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnCancel.Location = new Point(263, 125);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnOk.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnOk.Location = new Point(181, 125);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 24);
      this.btnOk.TabIndex = 5;
      this.btnOk.Text = "&OK";
      this.btnOk.Click += new EventHandler(this.okBtn_Click);
      this.rbApplyToNonCustomUser.AutoSize = true;
      this.rbApplyToNonCustomUser.Checked = true;
      this.rbApplyToNonCustomUser.Location = new Point(14, 42);
      this.rbApplyToNonCustomUser.Name = "rbApplyToNonCustomUser";
      this.rbApplyToNonCustomUser.Size = new Size(331, 17);
      this.rbApplyToNonCustomUser.TabIndex = 10;
      this.rbApplyToNonCustomUser.TabStop = true;
      this.rbApplyToNonCustomUser.Text = "Apply only to users that are linked with Organization SSO settings";
      this.rbApplyToNonCustomUser.UseVisualStyleBackColor = true;
      this.rbApplyToNonCustomUser.CheckedChanged += new EventHandler(this.rbApplyToNonCustomUser_CheckedChanged);
      this.rbApplyAll.AutoSize = true;
      this.rbApplyAll.Location = new Point(14, 70);
      this.rbApplyAll.Name = "rbApplyAll";
      this.rbApplyAll.Size = new Size(104, 17);
      this.rbApplyAll.TabIndex = 11;
      this.rbApplyAll.Text = "Apply to all users";
      this.rbApplyAll.UseVisualStyleBackColor = true;
      this.rbApplyAll.CheckedChanged += new EventHandler(this.rbApplyToNonCustomUser_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(315, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Please select which users should inherit the parent SSO settings :";
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(353, 157);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.rbApplyAll);
      this.Controls.Add((Control) this.rbApplyToNonCustomUser);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.groupBox1);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SSOOrgConfirmationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
    }

    private void rbApplyToNonCustomUser_CheckedChanged(object sender, EventArgs e)
    {
      this.ApplyToAll = this.rbApplyAll.Checked;
    }
  }
}
