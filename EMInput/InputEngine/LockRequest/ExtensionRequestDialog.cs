// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockRequest.ExtensionRequestDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.LockRequest
{
  public class ExtensionRequestDialog : Form
  {
    private ExtensionRequestForm extensionRequestForm;
    private Sessions.Session session;
    public string TradeExtensionInfo = string.Empty;
    private IContainer components;
    private Panel pnlLockExtensionRequest;
    private Panel panel1;
    private Button btnRequestExtension;
    private Button btnCancel;

    public ExtensionRequestDialog(DateTime dtExpDate, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.extensionRequestForm = new ExtensionRequestForm(dtExpDate, this.session);
      this.extensionRequestForm.Dock = DockStyle.Fill;
      this.pnlLockExtensionRequest.Controls.Add((Control) this.extensionRequestForm);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnRequestExtension_Click(object sender, EventArgs e)
    {
      if (!this.extensionRequestForm.requestTradeExtension())
        return;
      this.TradeExtensionInfo = this.extensionRequestForm.TradeExtensionInfo;
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
      this.pnlLockExtensionRequest = new Panel();
      this.panel1 = new Panel();
      this.btnRequestExtension = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlLockExtensionRequest.Dock = DockStyle.Fill;
      this.pnlLockExtensionRequest.Location = new Point(0, 0);
      this.pnlLockExtensionRequest.Name = "pnlLockExtensionRequest";
      this.pnlLockExtensionRequest.Size = new Size(500, 279);
      this.pnlLockExtensionRequest.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.btnRequestExtension);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 279);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(500, 46);
      this.panel1.TabIndex = 1;
      this.btnRequestExtension.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRequestExtension.Location = new Point(292, 11);
      this.btnRequestExtension.Name = "btnRequestExtension";
      this.btnRequestExtension.Size = new Size(111, 23);
      this.btnRequestExtension.TabIndex = 0;
      this.btnRequestExtension.Text = "Request Extension";
      this.btnRequestExtension.UseVisualStyleBackColor = true;
      this.btnRequestExtension.Click += new EventHandler(this.btnRequestExtension_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(413, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(500, 325);
      this.Controls.Add((Control) this.pnlLockExtensionRequest);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExtensionRequestDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Extension Request";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
