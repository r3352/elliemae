// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AddGroupDialog
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AddGroupDialog : Form
  {
    private string groupName = "";
    private TextBox nameBox;
    private Button cancelBtn;
    private Button okBtn;
    private Label label1;
    private System.ComponentModel.Container components;

    public AddGroupDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.nameBox = new TextBox();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.nameBox.Location = new Point(12, 40);
      this.nameBox.Name = "nameBox";
      this.nameBox.Size = new Size(224, 20);
      this.nameBox.TabIndex = 0;
      this.nameBox.KeyPress += new KeyPressEventHandler(this.nameBox_KeyPress);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(160, 72);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 2;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(84, 72);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 1;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(236, 16);
      this.label1.TabIndex = 7;
      this.label1.Text = "Enter the name of the group and click OK.";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(250, 104);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.nameBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddGroupDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Group";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.groupName = this.nameBox.Text.Trim();
      if (this.groupName == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a group name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nameBox.Focus();
      }
      else if (Session.MessengerListManager.ContainsGroup(this.groupName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A group with the name " + this.groupName + " already exists. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.nameBox.Focus();
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.nameBox.Text.Length < 64 || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    public string GroupName => this.groupName;
  }
}
