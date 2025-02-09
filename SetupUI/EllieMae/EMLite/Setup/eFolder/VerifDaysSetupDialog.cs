// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.VerifDaysSetupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class VerifDaysSetupDialog : Form
  {
    private VerifDays verifDays;
    private string verifID;
    private IContainer components;
    private TextBox txtName;
    private Label lblDaysTillExpire;
    private Label lblDaysTillDue;
    private Label lblName;
    private TextBox txtDaysTillExpire;
    private TextBox txtDaysTillDue;
    private Button btnOK;
    private Button btnCancel;

    public VerifDaysSetupDialog(VerifDays verifDays, string verifID)
    {
      this.InitializeComponent();
      this.verifDays = verifDays;
      this.verifID = verifID;
      this.txtName.Text = verifID;
      int recvDays = this.verifDays.GetRecvDays(verifID);
      if (recvDays > 0)
        this.txtDaysTillDue.Text = recvDays.ToString();
      int expDays = this.verifDays.GetExpDays(verifID);
      if (expDays > 0)
        this.txtDaysTillExpire.Text = expDays.ToString();
      TextBoxFormatter.Attach(this.txtDaysTillDue, TextBoxContentRule.Integer);
      TextBoxFormatter.Attach(this.txtDaysTillExpire, TextBoxContentRule.Integer);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.verifDays.SetRecvDays(this.verifID, Utils.ParseInt((object) this.txtDaysTillDue.Text, 0));
      this.verifDays.SetExpDays(this.verifID, Utils.ParseInt((object) this.txtDaysTillExpire.Text, 0));
      Session.SaveSystemSettings((object) this.verifDays);
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtName = new TextBox();
      this.lblDaysTillExpire = new Label();
      this.lblDaysTillDue = new Label();
      this.lblName = new Label();
      this.txtDaysTillExpire = new TextBox();
      this.txtDaysTillDue = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.txtName.Location = new Point(104, 12);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(212, 20);
      this.txtName.TabIndex = 1;
      this.txtName.TabStop = false;
      this.lblDaysTillExpire.AutoSize = true;
      this.lblDaysTillExpire.Location = new Point(12, 64);
      this.lblDaysTillExpire.Name = "lblDaysTillExpire";
      this.lblDaysTillExpire.Size = new Size(77, 14);
      this.lblDaysTillExpire.TabIndex = 4;
      this.lblDaysTillExpire.Text = "Days to Expire";
      this.lblDaysTillDue.AutoSize = true;
      this.lblDaysTillDue.Location = new Point(12, 40);
      this.lblDaysTillDue.Name = "lblDaysTillDue";
      this.lblDaysTillDue.Size = new Size(86, 14);
      this.lblDaysTillDue.TabIndex = 2;
      this.lblDaysTillDue.Text = "Days to Receive";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(12, 16);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 14);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name";
      this.txtDaysTillExpire.Location = new Point(104, 60);
      this.txtDaysTillExpire.MaxLength = 3;
      this.txtDaysTillExpire.Name = "txtDaysTillExpire";
      this.txtDaysTillExpire.Size = new Size(80, 20);
      this.txtDaysTillExpire.TabIndex = 5;
      this.txtDaysTillDue.Location = new Point(104, 36);
      this.txtDaysTillDue.MaxLength = 3;
      this.txtDaysTillDue.Name = "txtDaysTillDue";
      this.txtDaysTillDue.Size = new Size(80, 20);
      this.txtDaysTillDue.TabIndex = 3;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(159, 108);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(240, 108);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(328, 143);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblDaysTillExpire);
      this.Controls.Add((Control) this.lblDaysTillDue);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.txtDaysTillExpire);
      this.Controls.Add((Control) this.txtDaysTillDue);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (VerifDaysSetupDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Verifications";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
