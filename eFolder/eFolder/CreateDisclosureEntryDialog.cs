// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.CreateDisclosureEntryDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.eFolder.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class CreateDisclosureEntryDialog : Form
  {
    private IContainer components;
    private Label label2;
    private PictureBox pictureBox1;
    private Button btnNo;
    private Button btnYes;
    private Label label1;

    public CreateDisclosureEntryDialog() => this.InitializeComponent();

    private void btnYes_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Yes;

    private void btnNo_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.No;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.btnNo = new Button();
      this.btnYes = new Button();
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(52, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(189, 14);
      this.label2.TabIndex = 9;
      this.label2.Text = "Do you want to create an entry now?";
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(294, 82);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 7;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(213, 82);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 6;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(52, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(317, 48);
      this.label1.TabIndex = 5;
      this.label1.Text = "If one or more of the selected disclosures is being sent to the borrower, you must create a Disclosure History entry on the Disclosure Tracking tool.";
      this.pictureBox1.Image = (Image) Resources.compliance_48x48_flag;
      this.pictureBox1.Location = new Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(47, 50);
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(373, 109);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (CreateDisclosureEntryDialog);
      this.Text = "Create Disclosure Entry?";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
