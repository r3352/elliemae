// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CDOHelpDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CDOHelpDialog : Form
  {
    private Label labMessage;
    private Button btnOK;
    private Button btnCancel;
    private Button btnHelp;
    private System.ComponentModel.Container components;

    public CDOHelpDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labMessage = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnHelp = new Button();
      this.SuspendLayout();
      this.labMessage.Location = new Point(24, 16);
      this.labMessage.Name = "labMessage";
      this.labMessage.Size = new Size(448, 80);
      this.labMessage.TabIndex = 0;
      this.labMessage.Text = "Encompass has detected a missing Microsoft Office component. If you choose to continue, some email addresses may not be imported.\n\n- Click OK to continue anyway.\n- Click Cancel to stop importing.\n- Click Help for steps to install the required component.";
      this.btnOK.Anchor = AnchorStyles.Bottom;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(128, 104);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnCancel.Anchor = AnchorStyles.Bottom;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(208, 104);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnHelp.Anchor = AnchorStyles.Bottom;
      this.btnHelp.Location = new Point(288, 104);
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = new Size(72, 24);
      this.btnHelp.TabIndex = 3;
      this.btnHelp.Text = "&Help";
      this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(490, 135);
      this.Controls.Add((Control) this.btnHelp);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.labMessage);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CDOHelpDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      this.ResumeLayout(false);
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
      int num = (int) new CDOHelpStepDialog().ShowDialog();
    }
  }
}
