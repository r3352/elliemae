// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockCancellationDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LockCancellationDialog : Form
  {
    private LockCancellationRequestForm lockCancelForm;
    private LockCancellationDialog.CancellationActionType actionType;
    private IContainer components;
    private Panel pnlLockCancellationRequest;
    private Panel panel1;
    private Button btnSubmit;
    private Button btnCancel;

    public LockCancellationDialog(
      Sessions.Session session,
      LoanData loanData,
      LockCancellationDialog.CancellationActionType actionType)
    {
      this.actionType = actionType;
      this.InitializeComponent();
      if (actionType != LockCancellationDialog.CancellationActionType.RequestCancellation)
      {
        if (actionType == LockCancellationDialog.CancellationActionType.Cancel)
          ;
        this.Text = "Cancellation";
        this.btnSubmit.Text = "Cancel Lock";
      }
      else
      {
        this.Text = "Cancellation Request";
        this.btnSubmit.Text = "Request Cancellation";
      }
      this.lockCancelForm = new LockCancellationRequestForm(session, loanData, actionType);
      this.lockCancelForm.Dock = DockStyle.Fill;
      this.pnlLockCancellationRequest.Controls.Add((Control) this.lockCancelForm);
    }

    private void btnCancellationRequest_Click(object sender, EventArgs e)
    {
      if (!this.lockCancelForm.RequestLockCancellation(true))
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlLockCancellationRequest = new Panel();
      this.panel1 = new Panel();
      this.btnSubmit = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlLockCancellationRequest.Dock = DockStyle.Fill;
      this.pnlLockCancellationRequest.Location = new Point(0, 0);
      this.pnlLockCancellationRequest.Name = "pnlLockCancellationRequest";
      this.pnlLockCancellationRequest.Size = new Size(820, 539);
      this.pnlLockCancellationRequest.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.btnSubmit);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 539);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(820, 46);
      this.panel1.TabIndex = 1;
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(600, 11);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(128, 23);
      this.btnSubmit.TabIndex = 1;
      this.btnSubmit.Text = "Request Cancellation";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnCancellationRequest_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(733, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(820, 585);
      this.Controls.Add((Control) this.pnlLockCancellationRequest);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LockCancellationDialog);
      this.Text = "Cancellation Request";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum CancellationActionType
    {
      RequestCancellation,
      Cancel,
    }
  }
}
