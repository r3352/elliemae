// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanSetupHelp
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
  public class LoanSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem3Summary;
    private Label labSubitem3Title;
    private Label labSubitem4Summary;
    private Label labSubitem4Title;
    private Label labSubitem5Summary;
    private Label labSubitem5Title;
    private Label labSubitem8Summary;
    private Label labSubitem8Title;
    private IContainer components;
    private Label labSubitem11Summary;
    private Label labSubitem11Title;
    private SetUpContainer setUpContainer;

    public LoanSetupHelp(SetUpContainer setUpContainer)
    {
      this.setUpContainer = setUpContainer;
      this.InitializeComponent();
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.labSubitem3Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem3Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem3Title.BackColor = this.defaultBackColor;
      this.labSubitem3Summary.BackColor = this.defaultBackColor;
      this.labSubitem4Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem4Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem4Title.BackColor = this.defaultBackColor;
      this.labSubitem4Summary.BackColor = this.defaultBackColor;
      this.labSubitem5Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem5Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem5Title.BackColor = this.defaultBackColor;
      this.labSubitem5Summary.BackColor = this.defaultBackColor;
      this.labSubitem8Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem8Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem8Title.BackColor = this.defaultBackColor;
      this.labSubitem8Summary.BackColor = this.defaultBackColor;
      this.labSubitem11Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem11Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem11Title.BackColor = this.defaultBackColor;
      this.labSubitem11Summary.BackColor = this.defaultBackColor;
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
      this.labSubitem3Summary = new Label();
      this.labSubitem3Title = new Label();
      this.labSubitem4Summary = new Label();
      this.labSubitem4Title = new Label();
      this.labSubitem5Summary = new Label();
      this.labSubitem5Title = new Label();
      this.labSubitem8Summary = new Label();
      this.labSubitem8Title = new Label();
      this.labSubitem11Summary = new Label();
      this.labSubitem11Title = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(492, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Loan Setup";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(492, 36);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Loan Setup options to create loan folders, set up loan numbering systems, configure default GFE output forms and create default RESPA information.";
      this.labSubitem3Summary.Location = new Point(20, 92);
      this.labSubitem3Summary.Name = "labSubitem3Summary";
      this.labSubitem3Summary.Size = new Size(168, 36);
      this.labSubitem3Summary.TabIndex = 7;
      this.labSubitem3Summary.Text = "Create loan folders to organize loans into groups.";
      this.labSubitem3Title.Cursor = Cursors.Hand;
      this.labSubitem3Title.Location = new Point(20, 76);
      this.labSubitem3Title.Name = "labSubitem3Title";
      this.labSubitem3Title.Size = new Size(168, 16);
      this.labSubitem3Title.TabIndex = 6;
      this.labSubitem3Title.Text = "Loan Folders";
      this.labSubitem3Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem4Summary.Location = new Point(20, 164);
      this.labSubitem4Summary.Name = "labSubitem4Summary";
      this.labSubitem4Summary.Size = new Size(168, 36);
      this.labSubitem4Summary.TabIndex = 9;
      this.labSubitem4Summary.Text = "Define the assignment of loan numbers to loans.";
      this.labSubitem4Title.Cursor = Cursors.Hand;
      this.labSubitem4Title.Location = new Point(20, 148);
      this.labSubitem4Title.Name = "labSubitem4Title";
      this.labSubitem4Title.Size = new Size(168, 16);
      this.labSubitem4Title.TabIndex = 8;
      this.labSubitem4Title.Text = "Auto Loan Numbering";
      this.labSubitem4Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem5Summary.Location = new Point(20, 236);
      this.labSubitem5Summary.Name = "labSubitem5Summary";
      this.labSubitem5Summary.Size = new Size(168, 36);
      this.labSubitem5Summary.TabIndex = 11;
      this.labSubitem5Summary.Text = "Define the assignment of MIN numbers to loans.";
      this.labSubitem5Title.Cursor = Cursors.Hand;
      this.labSubitem5Title.Location = new Point(20, 220);
      this.labSubitem5Title.Name = "labSubitem5Title";
      this.labSubitem5Title.Size = new Size(168, 16);
      this.labSubitem5Title.TabIndex = 10;
      this.labSubitem5Title.Text = "Auto MERS MIN Numbering";
      this.labSubitem5Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem8Summary.Location = new Point(208, 92);
      this.labSubitem8Summary.Name = "labSubitem8Summary";
      this.labSubitem8Summary.Size = new Size(168, 72);
      this.labSubitem8Summary.TabIndex = 17;
      this.labSubitem8Summary.Text = "Create default RESPA information. When you start a new loan, the data from the template is placed on the RESPA Servicing Disclosure.";
      this.labSubitem8Title.Cursor = Cursors.Hand;
      this.labSubitem8Title.Location = new Point(208, 76);
      this.labSubitem8Title.Name = "labSubitem8Title";
      this.labSubitem8Title.Size = new Size(168, 16);
      this.labSubitem8Title.TabIndex = 16;
      this.labSubitem8Title.Text = "RESPA";
      this.labSubitem8Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem11Summary.Location = new Point(20, 308);
      this.labSubitem11Summary.Name = "labSubitem11Summary";
      this.labSubitem11Summary.Size = new Size(168, 40);
      this.labSubitem11Summary.TabIndex = 23;
      this.labSubitem11Summary.Text = "Select the default GFE output forms to print.";
      this.labSubitem11Title.Cursor = Cursors.Hand;
      this.labSubitem11Title.Location = new Point(20, 292);
      this.labSubitem11Title.Name = "labSubitem11Title";
      this.labSubitem11Title.Size = new Size(168, 16);
      this.labSubitem11Title.TabIndex = 22;
      this.labSubitem11Title.Text = "GFE Print";
      this.labSubitem11Title.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.labSubitem11Summary);
      this.Controls.Add((Control) this.labSubitem11Title);
      this.Controls.Add((Control) this.labSubitem8Summary);
      this.Controls.Add((Control) this.labSubitem8Title);
      this.Controls.Add((Control) this.labSubitem5Summary);
      this.Controls.Add((Control) this.labSubitem5Title);
      this.Controls.Add((Control) this.labSubitem4Summary);
      this.Controls.Add((Control) this.labSubitem4Title);
      this.Controls.Add((Control) this.labSubitem3Summary);
      this.Controls.Add((Control) this.labSubitem3Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (LoanSetupHelp);
      this.Size = new Size(763, 604);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "labSubitem3Title":
          nodeText = "Loan Folders";
          break;
        case "labSubitem4Title":
          nodeText = "Auto Loan Numbering";
          break;
        case "labSubitem5Title":
          nodeText = "Auto MERS MIN Numbering";
          break;
        case "labSubitem8Title":
          nodeText = "RESPA";
          break;
        case "labSubitem11Title":
          nodeText = "GFE Print";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
