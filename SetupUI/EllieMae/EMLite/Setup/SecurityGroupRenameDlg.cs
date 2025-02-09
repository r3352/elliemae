// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecurityGroupRenameDlg
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
  public class SecurityGroupRenameDlg : Form
  {
    private Label label1;
    private TextBox textBoxSecurityGroupName;
    private Button btnOK;
    private Button button1;
    private System.ComponentModel.Container components;

    public string SecurityGroupName => this.textBoxSecurityGroupName.Text;

    public SecurityGroupRenameDlg() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textBoxSecurityGroupName = new TextBox();
      this.btnOK = new Button();
      this.button1 = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(344, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Please enter the new group name.";
      this.textBoxSecurityGroupName.Location = new Point(12, 44);
      this.textBoxSecurityGroupName.Name = "textBoxSecurityGroupName";
      this.textBoxSecurityGroupName.Size = new Size(344, 20);
      this.textBoxSecurityGroupName.TabIndex = 1;
      this.textBoxSecurityGroupName.Text = "";
      this.btnOK.Location = new Point(228, 80);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(60, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.button1.DialogResult = DialogResult.Cancel;
      this.button1.Location = new Point(296, 80);
      this.button1.Name = "button1";
      this.button1.Size = new Size(60, 24);
      this.button1.TabIndex = 3;
      this.button1.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(374, 115);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.textBoxSecurityGroupName);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (SecurityGroupRenameDlg);
      this.ShowInTaskbar = false;
      this.Text = "Group Name";
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (Session.AclGroupManager.GroupNameExists(this.SecurityGroupName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The group name already exists. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }
  }
}
