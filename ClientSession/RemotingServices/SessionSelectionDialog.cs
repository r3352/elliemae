// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.SessionSelectionDialog
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class SessionSelectionDialog : Form
  {
    private SessionInfo[] sessions;
    private Label label1;
    private ComboBox cmbBoxSessions;
    private Button btnOK;
    private System.ComponentModel.Container components;

    public SessionSelectionDialog(string userID, SessionInfo[] sessions)
    {
      this.InitializeComponent();
      this.label1.Text = "There are multiple users logged on as '" + userID + "'. Select a user session to talk to.";
      this.sessions = sessions;
      for (int index = 0; index < sessions.Length; ++index)
        this.cmbBoxSessions.Items.Add((object) sessions[index].SessionID);
      this.cmbBoxSessions.SelectedIndex = 0;
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
      this.cmbBoxSessions = new ComboBox();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(316, 37);
      this.label1.TabIndex = 0;
      this.label1.Text = "There are multiple peopel log on as 'userid'.  Select a user session to talk to.";
      this.cmbBoxSessions.Location = new Point(8, 48);
      this.cmbBoxSessions.Name = "cmbBoxSessions";
      this.cmbBoxSessions.Size = new Size(235, 21);
      this.cmbBoxSessions.TabIndex = 1;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(249, 48);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(329, 80);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.cmbBoxSessions);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SessionSelectionDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Instant Messenger";
      this.ResumeLayout(false);
    }

    public SessionInfo SelectedSessionInfo
    {
      get
      {
        return this.sessions == null ? (SessionInfo) null : this.sessions[this.cmbBoxSessions.SelectedIndex];
      }
    }
  }
}
