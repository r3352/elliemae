// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnlockLoanDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class UnlockLoanDialog : Form
  {
    private Label lblMsg;
    private Button btnUnlock;
    private Button btnReadonly;
    private Button btnCancel;
    private System.ComponentModel.Container components;

    public UnlockLoanDialog(LockInfo lockInfo)
    {
      this.InitializeComponent();
      string str = "";
      if (lockInfo.LockedFor == LoanInfo.LockReason.OpenForWork)
        str = "locked";
      else if (lockInfo.LockedFor == LoanInfo.LockReason.Downloaded)
        str = "downloaded";
      else if (lockInfo.LockedFor == LoanInfo.LockReason.NotLocked)
      {
        this.DialogResult = DialogResult.No;
        this.Close();
      }
      this.lblMsg.Text = "This loan was " + str + " by user '" + lockInfo.LockedBy + "' on " + lockInfo.LockedSince.ToString("MM/dd/yyyy HH:mm") + ". Do you want to unlock the loan before opening, or open the loan in read-only mode?";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMsg = new Label();
      this.btnUnlock = new Button();
      this.btnReadonly = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblMsg.Location = new Point(16, 8);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(376, 40);
      this.lblMsg.TabIndex = 0;
      this.btnUnlock.DialogResult = DialogResult.Yes;
      this.btnUnlock.Location = new Point(72, 56);
      this.btnUnlock.Name = "btnUnlock";
      this.btnUnlock.TabIndex = 1;
      this.btnUnlock.Text = "Unlock";
      this.btnReadonly.DialogResult = DialogResult.No;
      this.btnReadonly.Location = new Point(160, 56);
      this.btnReadonly.Name = "btnReadonly";
      this.btnReadonly.TabIndex = 2;
      this.btnReadonly.Text = "Read Only";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(248, 56);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnUnlock;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(400, 86);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnReadonly);
      this.Controls.Add((Control) this.btnUnlock);
      this.Controls.Add((Control) this.lblMsg);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UnlockLoanDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Unlock loan?";
      this.ResumeLayout(false);
    }
  }
}
