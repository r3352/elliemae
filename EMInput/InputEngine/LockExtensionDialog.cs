// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockExtensionDialog
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
  public class LockExtensionDialog : Form
  {
    private LockExtensionRequestForm lockRequestForm;
    private IContainer components;
    private Panel pnlLockExtensionRequest;
    private Panel panel1;
    private Button btnExtensionRequest;
    private Button btnCancel;

    public LockExtensionDialog(
      Sessions.Session session,
      LoanData loanData,
      LockExtensionUtils settings,
      int newExtNumber)
    {
      this.InitializeComponent();
      this.lockRequestForm = new LockExtensionRequestForm(session, loanData, settings, newExtNumber);
      this.lockRequestForm.Dock = DockStyle.Fill;
      this.pnlLockExtensionRequest.Controls.Add((Control) this.lockRequestForm);
      this.Text = "Extension Request #" + newExtNumber.ToString();
    }

    private void btnExtensionRequest_Click(object sender, EventArgs e)
    {
      if (!this.lockRequestForm.RequestLockExtension(true))
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
      this.pnlLockExtensionRequest = new Panel();
      this.panel1 = new Panel();
      this.btnExtensionRequest = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlLockExtensionRequest.Dock = DockStyle.Fill;
      this.pnlLockExtensionRequest.Location = new Point(0, 0);
      this.pnlLockExtensionRequest.Name = "pnlLockExtensionRequest";
      this.pnlLockExtensionRequest.Size = new Size(820, 539);
      this.pnlLockExtensionRequest.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.btnExtensionRequest);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 539);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(820, 46);
      this.panel1.TabIndex = 1;
      this.btnExtensionRequest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnExtensionRequest.Location = new Point(617, 11);
      this.btnExtensionRequest.Name = "btnExtensionRequest";
      this.btnExtensionRequest.Size = new Size(111, 23);
      this.btnExtensionRequest.TabIndex = 1;
      this.btnExtensionRequest.Text = "Request Extension";
      this.btnExtensionRequest.UseVisualStyleBackColor = true;
      this.btnExtensionRequest.Click += new EventHandler(this.btnExtensionRequest_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(733, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(820, 585);
      this.Controls.Add((Control) this.pnlLockExtensionRequest);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LockExtensionDialog);
      this.Text = "Extension Request";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
