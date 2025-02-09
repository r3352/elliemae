// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactUpdateQuestionDlg
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactUpdateQuestionDlg : Form
  {
    private Button btnYes;
    private Label label1;
    private Button btnYesAll;
    private Button btnNo;
    private Button btnNoAll;
    private System.ComponentModel.Container components;

    public ContactUpdateQuestionDlg(bool batchProcessing)
    {
      this.InitializeComponent();
      if (batchProcessing)
        return;
      this.btnNoAll.Visible = false;
      this.btnYesAll.Visible = false;
      this.btnYes.Location = this.btnNo.Location;
      this.btnNo.Location = this.btnNoAll.Location;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnYes = new Button();
      this.label1 = new Label();
      this.btnYesAll = new Button();
      this.btnNo = new Button();
      this.btnNoAll = new Button();
      this.SuspendLayout();
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(120, 60);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(72, 24);
      this.btnYes.TabIndex = 0;
      this.btnYes.Text = "Yes";
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(344, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = "You have completed this loan. Would you like to update Borrower Contacts with information from this loan?";
      this.btnYesAll.DialogResult = DialogResult.OK;
      this.btnYesAll.Location = new Point(40, 60);
      this.btnYesAll.Name = "btnYesAll";
      this.btnYesAll.Size = new Size(72, 24);
      this.btnYesAll.TabIndex = 2;
      this.btnYesAll.Text = "Yes to All";
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(200, 60);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(72, 24);
      this.btnNo.TabIndex = 3;
      this.btnNo.Text = "No";
      this.btnNoAll.DialogResult = DialogResult.Cancel;
      this.btnNoAll.Location = new Point(280, 60);
      this.btnNoAll.Name = "btnNoAll";
      this.btnNoAll.Size = new Size(72, 24);
      this.btnNoAll.TabIndex = 4;
      this.btnNoAll.Text = "No to All";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(362, 99);
      this.Controls.Add((Control) this.btnNoAll);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYesAll);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnYes);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (ContactUpdateQuestionDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass";
      this.ResumeLayout(false);
    }
  }
}
