// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.SelectSignatureTypeDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class SelectSignatureTypeDialog : Form
  {
    public string signatureType;
    private IContainer components;
    private RadioButton rdoESigned;
    private RadioButton rdoWetSigned;
    private Button btnOK;
    private RadioButton rdoOther;
    private Button btnCancel;
    private Label lblText;

    public SelectSignatureTypeDialog(string CDType)
    {
      this.InitializeComponent();
      this.lblText.Text = this.lblText.Text.Replace(" $CDType", CDType.Replace("Closing Disclosure", string.Empty));
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoESigned.Checked)
        this.signatureType = "esigned";
      else if (this.rdoWetSigned.Checked)
        this.signatureType = "wetsigned";
      else if (this.rdoOther.Checked)
        this.signatureType = "other";
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void rdoESigned_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rdoESigned.Checked)
        return;
      this.btnOK.Enabled = true;
    }

    private void rdoWetSigned_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rdoWetSigned.Checked)
        return;
      this.btnOK.Enabled = true;
    }

    private void rdoOther_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rdoOther.Checked)
        return;
      this.btnOK.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoESigned = new RadioButton();
      this.rdoWetSigned = new RadioButton();
      this.rdoOther = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblText = new Label();
      this.SuspendLayout();
      this.rdoESigned.AutoSize = true;
      this.rdoESigned.Location = new Point(42, 60);
      this.rdoESigned.Name = "rdoESigned";
      this.rdoESigned.Size = new Size(92, 20);
      this.rdoESigned.TabIndex = 0;
      this.rdoESigned.Text = "eSignable";
      this.rdoESigned.UseVisualStyleBackColor = true;
      this.rdoESigned.CheckedChanged += new EventHandler(this.rdoESigned_CheckedChanged);
      this.rdoWetSigned.AutoSize = true;
      this.rdoWetSigned.Location = new Point(42, 92);
      this.rdoWetSigned.Name = "rdoWetSigned";
      this.rdoWetSigned.Size = new Size(116, 20);
      this.rdoWetSigned.TabIndex = 1;
      this.rdoWetSigned.Text = "Wet Sign only";
      this.rdoWetSigned.UseVisualStyleBackColor = true;
      this.rdoWetSigned.CheckedChanged += new EventHandler(this.rdoWetSigned_CheckedChanged);
      this.rdoOther.AutoSize = true;
      this.rdoOther.Location = new Point(42, 125);
      this.rdoOther.Name = "rdoOther";
      this.rdoOther.Size = new Size(65, 20);
      this.rdoOther.TabIndex = 4;
      this.rdoOther.Text = "Other";
      this.rdoOther.UseVisualStyleBackColor = true;
      this.rdoOther.CheckedChanged += new EventHandler(this.rdoOther_CheckedChanged);
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(211, 177);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(96, 29);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(331, 177);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(101, 29);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblText.AutoSize = true;
      this.lblText.Location = new Point(39, 23);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(392, 16);
      this.lblText.TabIndex = 5;
      this.lblText.Text = "Select a Signature Type for the Final CD $CDType document";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(478, 231);
      this.Controls.Add((Control) this.lblText);
      this.Controls.Add((Control) this.rdoOther);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoWetSigned);
      this.Controls.Add((Control) this.rdoESigned);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectSignatureTypeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Signature Type";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
