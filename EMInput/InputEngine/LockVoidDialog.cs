// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockVoidDialog
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
  public class LockVoidDialog : Form
  {
    private LockVoidRequestForm lockCancelForm;
    private IContainer components;
    private Panel pnlLockVoidRequest;
    private Panel panel1;
    private Button btnSubmit;
    private Button btnCancel;

    public LockVoidDialog(Sessions.Session session, LoanData loanData)
    {
      this.InitializeComponent();
      this.lockCancelForm = new LockVoidRequestForm(session, loanData);
      this.lockCancelForm.Dock = DockStyle.Fill;
      this.pnlLockVoidRequest.Controls.Add((Control) this.lockCancelForm);
    }

    private void btnVoidRequest_Click(object sender, EventArgs e)
    {
      if (!this.lockCancelForm.RequestLockVoid(true))
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
      this.pnlLockVoidRequest = new Panel();
      this.panel1 = new Panel();
      this.btnSubmit = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlLockVoidRequest.Dock = DockStyle.Fill;
      this.pnlLockVoidRequest.Location = new Point(0, 0);
      this.pnlLockVoidRequest.Name = "pnlLockVoidRequest";
      this.pnlLockVoidRequest.Size = new Size(820, 539);
      this.pnlLockVoidRequest.TabIndex = 0;
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
      this.btnSubmit.Text = "Void Lock";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnVoidRequest_Click);
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
      this.Controls.Add((Control) this.pnlLockVoidRequest);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LockVoidDialog);
      this.Text = "Void";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
