// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CustomFieldsEditorDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CustomFieldsEditorDialog : Form
  {
    private CustomFieldsEditor fieldEditor;
    private Button btnOK;
    private Button btnCancel;
    private Panel pnlContent;
    private System.ComponentModel.Container components;

    public CustomFieldsEditorDialog()
    {
      this.InitializeComponent();
      this.fieldEditor = new CustomFieldsEditor(Session.DefaultInstance, false);
      this.fieldEditor.Dock = DockStyle.Fill;
      this.pnlContent.Controls.Add((Control) this.fieldEditor);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.pnlContent = new Panel();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(694, 538);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(88, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "Close";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(613, 538);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Visible = false;
      this.pnlContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlContent.Location = new Point(0, 0);
      this.pnlContent.Name = "pnlContent";
      this.pnlContent.Size = new Size(794, 532);
      this.pnlContent.TabIndex = 2;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(794, 568);
      this.Controls.Add((Control) this.pnlContent);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomFieldsEditorDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Loan Custom Field Editor";
      this.Closing += new CancelEventHandler(this.CustomFieldsEditorDialog_Closing);
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void CustomFieldsEditorDialog_Closing(object sender, CancelEventArgs e)
    {
    }
  }
}
