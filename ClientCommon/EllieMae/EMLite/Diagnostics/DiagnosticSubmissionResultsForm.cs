// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DiagnosticSubmissionResultsForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public class DiagnosticSubmissionResultsForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Label label1;
    private Label label2;
    private TextBox txtPackageID;

    public DiagnosticSubmissionResultsForm(string pkgId)
    {
      this.InitializeComponent();
      this.txtPackageID.Text = pkgId;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtPackageID = new TextBox();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.OK;
      this.btnCancel.Location = new Point(304, 111);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(368, 49);
      this.label1.TabIndex = 7;
      this.label1.Text = "Your diagnostic data has been successfully submitted to ICE Mortgage Technology. When contacting Customer Support, you should reference the following identifier to help us quickly locate and analyze your data.";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 71);
      this.label2.Name = "label2";
      this.label2.Size = new Size(71, 14);
      this.label2.TabIndex = 10;
      this.label2.Text = "Submission #";
      this.txtPackageID.Location = new Point(83, 67);
      this.txtPackageID.Name = "txtPackageID";
      this.txtPackageID.ReadOnly = true;
      this.txtPackageID.Size = new Size(200, 20);
      this.txtPackageID.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(388, 142);
      this.Controls.Add((Control) this.txtPackageID);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (DiagnosticSubmissionResultsForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Diagnostics Submission";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
