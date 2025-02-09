// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.InsertOtherDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  internal class InsertOtherDialog : Form
  {
    private Label label1;
    private Button btnCancel;
    private Button btnOK;
    private System.ComponentModel.Container components;
    private TextBox idTxt;
    private string fieldID;

    public InsertOtherDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.idTxt = new TextBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Field ID:";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(213, 44);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Location = new Point(132, 44);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.idTxt.Location = new Point(68, 12);
      this.idTxt.Name = "idTxt";
      this.idTxt.Size = new Size(220, 20);
      this.idTxt.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(318, 80);
      this.Controls.Add((Control) this.idTxt);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InsertOtherDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Insert Other";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.fieldID = this.idTxt.Text.Trim();
      if (this.fieldID == string.Empty || this.fieldID == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a field ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    internal string FieldID => this.fieldID;
  }
}
