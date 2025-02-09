// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ComplianceAlertMessage
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ComplianceAlertMessage : Form
  {
    private IContainer components;
    private PictureBox pictureBox1;
    private Label lblMessage;
    private Label lblTitle;
    private Button btnOK;

    public ComplianceAlertMessage(string title, string description)
    {
      this.InitializeComponent();
      this.lblTitle.Text = title;
      this.lblMessage.Text = description;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMessage = new Label();
      this.lblTitle = new Label();
      this.pictureBox1 = new PictureBox();
      this.btnOK = new Button();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblMessage.AutoEllipsis = true;
      this.lblMessage.Location = new Point(64, 28);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(272, 44);
      this.lblMessage.TabIndex = 4;
      this.lblMessage.Text = "<Alert Description Here>";
      this.lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTitle.AutoEllipsis = true;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(64, 10);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(272, 15);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "<Alert Title Here>";
      this.pictureBox1.Image = (Image) Resources.compliance_48x48_flag;
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(48, 48);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(261, 75);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(348, 103);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.lblTitle);
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ComplianceAlertMessage);
      this.Text = "Regulation Alert";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
