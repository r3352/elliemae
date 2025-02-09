// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AddLogDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AddLogDialog : Form
  {
    private RadioButton conRadio;
    private RadioButton genRadio;
    private FeaturesAclManager aclMgr;
    private DialogButtons dlgButtons;
    private Label label1;
    private System.ComponentModel.Container components;
    public const int Conversation = 1;
    public const int Task = 2;
    private int type;

    public AddLogDialog(LoanContentAccess loanContentAccess)
    {
      this.InitializeComponent();
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if ((loanContentAccess & LoanContentAccess.ConversationLog) != LoanContentAccess.ConversationLog && loanContentAccess != LoanContentAccess.FullAccess || !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ConversationLog))
      {
        this.conRadio.Checked = false;
        this.conRadio.Enabled = false;
      }
      if ((loanContentAccess & LoanContentAccess.Task) != LoanContentAccess.Task && loanContentAccess != LoanContentAccess.FullAccess || !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Task_Add))
      {
        this.genRadio.Checked = false;
        this.genRadio.Enabled = false;
      }
      if (this.genRadio.Enabled && !this.conRadio.Enabled)
        this.genRadio.Checked = true;
      else if (!this.genRadio.Enabled && this.conRadio.Enabled)
        this.conRadio.Checked = true;
      if (this.genRadio.Enabled || this.conRadio.Enabled)
        this.dlgButtons.OKButton.Enabled = true;
      else
        this.dlgButtons.OKButton.Enabled = false;
    }

    public bool Editable => this.genRadio.Enabled || this.conRadio.Enabled;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.conRadio = new RadioButton();
      this.genRadio = new RadioButton();
      this.dlgButtons = new DialogButtons();
      this.label1 = new Label();
      this.SuspendLayout();
      this.conRadio.Location = new Point(38, 37);
      this.conRadio.Name = "conRadio";
      this.conRadio.Size = new Size(121, 20);
      this.conRadio.TabIndex = 1;
      this.conRadio.Text = "Conversation Log";
      this.genRadio.Location = new Point(38, 57);
      this.genRadio.Name = "genRadio";
      this.genRadio.Size = new Size(121, 20);
      this.genRadio.TabIndex = 2;
      this.genRadio.Text = "Task";
      this.dlgButtons.ButtonAlignment = HorizontalAlignment.Center;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 100);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(198, 44);
      this.dlgButtons.TabIndex = 3;
      this.dlgButtons.OK += new EventHandler(this.okBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(173, 14);
      this.label1.TabIndex = 4;
      this.label1.Text = "Select the type of log entry to add:";
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(198, 144);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.genRadio);
      this.Controls.Add((Control) this.conRadio);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddLogDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Log Entry";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public int SelectedType => this.type;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.conRadio.Checked)
        this.type = 1;
      else if (this.genRadio.Checked)
      {
        this.type = 2;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select Conversation or Task to add.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.DialogResult = DialogResult.OK;
    }
  }
}
