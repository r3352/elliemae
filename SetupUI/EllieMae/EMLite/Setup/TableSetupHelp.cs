// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TableSetupHelp
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TableSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label lblSummary1;
    private Label lblTitle1;
    private Label lblSummary2;
    private Label lblTitle2;
    private Label lblSummary3;
    private Label lblTitle3;
    private Label lblSummary4;
    private Label lblTitle4;
    private IContainer components;
    private SetUpContainer setUpContainer;

    public TableSetupHelp(SetUpContainer setUpContainer)
    {
      this.InitializeComponent();
      this.setUpContainer = setUpContainer;
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.lblTitle1.Font = this.defaultSubItemTitleFont;
      this.lblTitle1.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblTitle1.BackColor = this.defaultBackColor;
      this.lblSummary1.BackColor = this.defaultBackColor;
      this.lblTitle2.Font = this.defaultSubItemTitleFont;
      this.lblTitle2.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblTitle2.BackColor = this.defaultBackColor;
      this.lblSummary2.BackColor = this.defaultBackColor;
      this.lblTitle3.Font = this.defaultSubItemTitleFont;
      this.lblTitle3.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblTitle3.BackColor = this.defaultBackColor;
      this.lblSummary3.BackColor = this.defaultBackColor;
      this.lblTitle4.Font = this.defaultSubItemTitleFont;
      this.lblTitle4.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblTitle4.BackColor = this.defaultBackColor;
      this.lblSummary4.BackColor = this.defaultBackColor;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labItemTitle = new Label();
      this.labItemSummary = new Label();
      this.lblSummary1 = new Label();
      this.lblTitle1 = new Label();
      this.lblSummary2 = new Label();
      this.lblTitle2 = new Label();
      this.lblSummary3 = new Label();
      this.lblTitle3 = new Label();
      this.lblSummary4 = new Label();
      this.lblTitle4 = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(496, 24);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Table List";
      this.labItemSummary.Location = new Point(8, 32);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(496, 44);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Table List options to create escrow and title tables for the companies with which you do business. Once tables are created, you can select them from the Good Faith Estimate or California MLDS. Encompass calculates the fee and places it in the form.";
      this.lblSummary1.Location = new Point(16, 104);
      this.lblSummary1.Name = "lblSummary1";
      this.lblSummary1.Size = new Size(168, 44);
      this.lblSummary1.TabIndex = 23;
      this.lblSummary1.Text = "Create tables of escrow fees for the companies with which you do business.";
      this.lblTitle1.Cursor = Cursors.Hand;
      this.lblTitle1.Location = new Point(16, 88);
      this.lblTitle1.Name = "lblTitle1";
      this.lblTitle1.Size = new Size(168, 16);
      this.lblTitle1.TabIndex = 22;
      this.lblTitle1.Text = "Escrow Fee (Purchase)";
      this.lblTitle1.Click += new EventHandler(this.labelHeader_Click);
      this.lblSummary2.Location = new Point(16, 172);
      this.lblSummary2.Name = "lblSummary2";
      this.lblSummary2.Size = new Size(168, 44);
      this.lblSummary2.TabIndex = 25;
      this.lblSummary2.Text = "Create tables of escrow fees for the companies with which you do business.";
      this.lblTitle2.Cursor = Cursors.Hand;
      this.lblTitle2.Location = new Point(16, 156);
      this.lblTitle2.Name = "lblTitle2";
      this.lblTitle2.Size = new Size(168, 16);
      this.lblTitle2.TabIndex = 24;
      this.lblTitle2.Text = "Escrow Fee (Refinance)";
      this.lblTitle2.Click += new EventHandler(this.labelHeader_Click);
      this.lblSummary3.Location = new Point(16, 240);
      this.lblSummary3.Name = "lblSummary3";
      this.lblSummary3.Size = new Size(168, 44);
      this.lblSummary3.TabIndex = 27;
      this.lblSummary3.Text = "Create tables of title fees for the companies with which you do business.";
      this.lblTitle3.Cursor = Cursors.Hand;
      this.lblTitle3.Location = new Point(16, 224);
      this.lblTitle3.Name = "lblTitle3";
      this.lblTitle3.Size = new Size(168, 16);
      this.lblTitle3.TabIndex = 26;
      this.lblTitle3.Text = "Title Fee (Purchase)";
      this.lblTitle3.Click += new EventHandler(this.labelHeader_Click);
      this.lblSummary4.Location = new Point(16, 308);
      this.lblSummary4.Name = "lblSummary4";
      this.lblSummary4.Size = new Size(168, 44);
      this.lblSummary4.TabIndex = 29;
      this.lblSummary4.Text = "Create tables of title fees for the companies with which you do business.";
      this.lblTitle4.Cursor = Cursors.Hand;
      this.lblTitle4.Location = new Point(16, 292);
      this.lblTitle4.Name = "lblTitle4";
      this.lblTitle4.Size = new Size(168, 16);
      this.lblTitle4.TabIndex = 28;
      this.lblTitle4.Text = "Title Fee (Refinance)";
      this.lblTitle4.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.lblSummary4);
      this.Controls.Add((Control) this.lblTitle4);
      this.Controls.Add((Control) this.lblSummary3);
      this.Controls.Add((Control) this.lblTitle3);
      this.Controls.Add((Control) this.lblSummary2);
      this.Controls.Add((Control) this.lblTitle2);
      this.Controls.Add((Control) this.lblSummary1);
      this.Controls.Add((Control) this.lblTitle1);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (TableSetupHelp);
      this.Size = new Size(521, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      string nodeText = "";
      string name = label.Name;
      if (name == "lblTitle1" || name == "lblTitle2" || name == "lblTitle3" || name == "lblTitle4")
        nodeText = label.Text;
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
