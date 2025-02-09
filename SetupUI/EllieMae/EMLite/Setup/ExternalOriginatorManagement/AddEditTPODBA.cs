// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddEditTPODBA
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddEditTPODBA : Form
  {
    private IContainer components;
    private Label lblDBAName;
    private Label lblReq;
    private TextBox txtDBAName;
    private CheckBox chkSetDefualt;
    private Button btnOK;
    private Button btnCancel;

    public AddEditTPODBA(ExternalOrgDBAName dba)
    {
      this.InitializeComponent();
      if (dba == null)
      {
        this.Text = "Add DBA";
      }
      else
      {
        this.Text = "Edit DBA";
        this.txtDBAName.Text = dba.Name;
        this.chkSetDefualt.Checked = dba.SetAsDefault;
      }
      this.txtDBAName_TextChanged((object) null, (EventArgs) null);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtDBAName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public string DBAName => this.txtDBAName.Text.Trim();

    public bool SetAsDefault => this.chkSetDefualt.Checked;

    private void txtDBAName_TextChanged(object sender, EventArgs e)
    {
      if (this.txtDBAName.Text.Trim() == "")
        this.btnOK.Enabled = false;
      else
        this.btnOK.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditTPODBA));
      this.lblDBAName = new Label();
      this.lblReq = new Label();
      this.txtDBAName = new TextBox();
      this.chkSetDefualt = new CheckBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblDBAName.AutoSize = true;
      this.lblDBAName.Location = new Point(22, 23);
      this.lblDBAName.Name = "lblDBAName";
      this.lblDBAName.Size = new Size(60, 13);
      this.lblDBAName.TabIndex = 0;
      this.lblDBAName.Text = "DBA Name";
      this.lblReq.AutoSize = true;
      this.lblReq.BackColor = Color.Transparent;
      this.lblReq.ForeColor = Color.Red;
      this.lblReq.Location = new Point(77, 23);
      this.lblReq.Name = "lblReq";
      this.lblReq.Size = new Size(11, 13);
      this.lblReq.TabIndex = 20;
      this.lblReq.Text = "*";
      this.txtDBAName.Location = new Point(95, 23);
      this.txtDBAName.MaxLength = 256;
      this.txtDBAName.Name = "txtDBAName";
      this.txtDBAName.Size = new Size(318, 20);
      this.txtDBAName.TabIndex = 21;
      this.txtDBAName.TextChanged += new EventHandler(this.txtDBAName_TextChanged);
      this.chkSetDefualt.AutoSize = true;
      this.chkSetDefualt.Location = new Point(95, 58);
      this.chkSetDefualt.Name = "chkSetDefualt";
      this.chkSetDefualt.Size = new Size(91, 17);
      this.chkSetDefualt.TabIndex = 22;
      this.chkSetDefualt.Text = "Set as default";
      this.chkSetDefualt.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(259, 89);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 23;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(349, 89);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 24;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(440, 124);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.chkSetDefualt);
      this.Controls.Add((Control) this.txtDBAName);
      this.Controls.Add((Control) this.lblReq);
      this.Controls.Add((Control) this.lblDBAName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditTPODBA);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add DBA";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
