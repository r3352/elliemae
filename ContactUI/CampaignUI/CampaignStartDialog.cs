// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignStartDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignStartDialog : Form
  {
    private CampaignStartDialog.SelectionTypes userSelection = CampaignStartDialog.SelectionTypes.Cancel;
    private System.ComponentModel.Container components;
    private Label lblText1;
    private Label lblText2;
    private Label lblText3a;
    private Label lblText7a;
    private Label lblText3b;
    private Label lblText7b;
    private Label lblText3c;
    private Label lblText7c;
    private Panel pnlFooter;
    private Label lblText4;
    private Label lblText5;
    private Label lblText6;
    private Label lblText8;
    private Label lblText9;
    private Label lblText10;
    private Button btnCancel;
    private Button btnStartCampaign;
    private Label lblSeparator;

    public CampaignStartDialog.SelectionTypes UserSelection => this.userSelection;

    public CampaignStartDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnStartCampaign_Click(object sender, EventArgs e)
    {
      this.userSelection = CampaignStartDialog.SelectionTypes.StartCampaign;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.userSelection = CampaignStartDialog.SelectionTypes.Cancel;
      this.Close();
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnStartCampaign = new Button();
      this.lblText1 = new Label();
      this.lblText2 = new Label();
      this.lblText3a = new Label();
      this.lblText7a = new Label();
      this.lblText3b = new Label();
      this.lblText7b = new Label();
      this.lblText3c = new Label();
      this.lblText7c = new Label();
      this.pnlFooter = new Panel();
      this.lblSeparator = new Label();
      this.lblText4 = new Label();
      this.lblText5 = new Label();
      this.lblText6 = new Label();
      this.lblText8 = new Label();
      this.lblText9 = new Label();
      this.lblText10 = new Label();
      this.pnlFooter.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(312, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(144, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Do Not Start Campaign";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnStartCampaign.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnStartCampaign.Location = new Point(208, 9);
      this.btnStartCampaign.Name = "btnStartCampaign";
      this.btnStartCampaign.Size = new Size(96, 23);
      this.btnStartCampaign.TabIndex = 1;
      this.btnStartCampaign.Text = "&Start Campaign";
      this.btnStartCampaign.Click += new EventHandler(this.btnStartCampaign_Click);
      this.lblText1.AutoSize = true;
      this.lblText1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblText1.Location = new Point(8, 8);
      this.lblText1.Name = "lblText1";
      this.lblText1.Size = new Size(131, 13);
      this.lblText1.TabIndex = 2;
      this.lblText1.Text = "Important Information:";
      this.lblText1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText2.AutoSize = true;
      this.lblText2.Location = new Point(8, 32);
      this.lblText2.Name = "lblText2";
      this.lblText2.Size = new Size(293, 13);
      this.lblText2.TabIndex = 3;
      this.lblText2.Text = "Confirm your campaign settings before starting the campaign.";
      this.lblText2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText3a.AutoSize = true;
      this.lblText3a.Location = new Point(32, 64);
      this.lblText3a.Name = "lblText3a";
      this.lblText3a.Size = new Size(159, 13);
      this.lblText3a.TabIndex = 4;
      this.lblText3a.Text = "After you start the campaign you";
      this.lblText3a.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText7a.AutoSize = true;
      this.lblText7a.Location = new Point(32, 160);
      this.lblText7a.Name = "lblText7a";
      this.lblText7a.Size = new Size(159, 13);
      this.lblText7a.TabIndex = 5;
      this.lblText7a.Text = "After you start the campaign you";
      this.lblText7a.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText3b.AutoSize = true;
      this.lblText3b.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblText3b.Location = new Point(187, 64);
      this.lblText3b.Name = "lblText3b";
      this.lblText3b.Size = new Size(46, 13);
      this.lblText3b.TabIndex = 6;
      this.lblText3b.Text = "cannot";
      this.lblText3b.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText7b.AutoSize = true;
      this.lblText7b.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblText7b.Location = new Point(187, 160);
      this.lblText7b.Name = "lblText7b";
      this.lblText7b.Size = new Size(28, 13);
      this.lblText7b.TabIndex = 7;
      this.lblText7b.Text = "can";
      this.lblText7b.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText3c.AutoSize = true;
      this.lblText3c.Location = new Point(229, 64);
      this.lblText3c.Margin = new Padding(0);
      this.lblText3c.Name = "lblText3c";
      this.lblText3c.Size = new Size(46, 13);
      this.lblText3c.TabIndex = 8;
      this.lblText3c.Text = "change:";
      this.lblText3c.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText7c.AutoSize = true;
      this.lblText7c.Location = new Point(211, 160);
      this.lblText7c.Name = "lblText7c";
      this.lblText7c.Size = new Size(46, 13);
      this.lblText7c.TabIndex = 9;
      this.lblText7c.Text = "change:";
      this.lblText7c.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlFooter.Controls.Add((Control) this.lblSeparator);
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Controls.Add((Control) this.btnStartCampaign);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 286);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(464, 40);
      this.pnlFooter.TabIndex = 10;
      this.lblSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(8, 0);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(448, 1);
      this.lblSeparator.TabIndex = 2;
      this.lblText4.AutoSize = true;
      this.lblText4.Location = new Point(56, 88);
      this.lblText4.Name = "lblText4";
      this.lblText4.Size = new Size(85, 13);
      this.lblText4.TabIndex = 11;
      this.lblText4.Text = "Campaign Name";
      this.lblText4.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText5.AutoSize = true;
      this.lblText5.Location = new Point(56, 108);
      this.lblText5.Name = "lblText5";
      this.lblText5.Size = new Size(60, 13);
      this.lblText5.TabIndex = 12;
      this.lblText5.Text = "Description";
      this.lblText5.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText6.AutoSize = true;
      this.lblText6.Location = new Point(56, 128);
      this.lblText6.Name = "lblText6";
      this.lblText6.Size = new Size(184, 13);
      this.lblText6.TabIndex = 13;
      this.lblText6.Text = "Steps, activities or activity documents";
      this.lblText6.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText8.AutoSize = true;
      this.lblText8.Location = new Point(56, 184);
      this.lblText8.Name = "lblText8";
      this.lblText8.Size = new Size(160, 13);
      this.lblText8.TabIndex = 14;
      this.lblText8.Text = "Manually Add/Remove contacts";
      this.lblText8.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText9.AutoSize = true;
      this.lblText9.Location = new Point(56, 204);
      this.lblText9.Name = "lblText9";
      this.lblText9.Size = new Size(111, 13);
      this.lblText9.TabIndex = 15;
      this.lblText9.Text = "Edit the Search Query";
      this.lblText9.TextAlign = ContentAlignment.MiddleLeft;
      this.lblText10.AutoSize = true;
      this.lblText10.Location = new Point(56, 224);
      this.lblText10.Name = "lblText10";
      this.lblText10.Size = new Size(307, 13);
      this.lblText10.TabIndex = 16;
      this.lblText10.Text = "Query Frequency (if contacts are added/removed automatically)";
      this.lblText10.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(464, 326);
      this.Controls.Add((Control) this.lblText10);
      this.Controls.Add((Control) this.lblText9);
      this.Controls.Add((Control) this.lblText8);
      this.Controls.Add((Control) this.lblText6);
      this.Controls.Add((Control) this.lblText5);
      this.Controls.Add((Control) this.lblText4);
      this.Controls.Add((Control) this.pnlFooter);
      this.Controls.Add((Control) this.lblText7c);
      this.Controls.Add((Control) this.lblText3c);
      this.Controls.Add((Control) this.lblText7b);
      this.Controls.Add((Control) this.lblText3b);
      this.Controls.Add((Control) this.lblText7a);
      this.Controls.Add((Control) this.lblText3a);
      this.Controls.Add((Control) this.lblText2);
      this.Controls.Add((Control) this.lblText1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignStartDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Start Campaign";
      this.pnlFooter.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum SelectionTypes
    {
      StartCampaign,
      Cancel,
    }
  }
}
