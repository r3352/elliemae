// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactSetupHelp
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
  public class ContactSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem1Title;
    private Label labSubitem1Summary;
    private Label labSubitem3Summary;
    private Label labSubitem3Title;
    private Label labSubitem2Summary;
    private Label labSubitem2Title;
    private Label labSubitem4Summary;
    private Label labSubitem4Title;
    private Label labSubitem5Summary;
    private Label labSubitem5Title;
    private IContainer components;
    private SetUpContainer setUpContainer;

    public ContactSetupHelp(SetUpContainer setUpContainer)
    {
      this.setUpContainer = setUpContainer;
      this.InitializeComponent();
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.labSubitem1Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem1Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem1Title.BackColor = this.defaultBackColor;
      this.labSubitem1Summary.BackColor = this.defaultBackColor;
      this.labSubitem2Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem2Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem2Title.BackColor = this.defaultBackColor;
      this.labSubitem2Summary.BackColor = this.defaultBackColor;
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
      this.labSubitem1Title = new Label();
      this.labSubitem1Summary = new Label();
      this.labSubitem3Summary = new Label();
      this.labSubitem3Title = new Label();
      this.labSubitem2Summary = new Label();
      this.labSubitem2Title = new Label();
      this.labSubitem4Summary = new Label();
      this.labSubitem4Title = new Label();
      this.labSubitem5Summary = new Label();
      this.labSubitem5Title = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(480, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Contacts Setup";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(480, 16);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Contacts Setup options to configure and customize the Contacts module.";
      this.labSubitem1Title.Cursor = Cursors.Hand;
      this.labSubitem1Title.Location = new Point(16, 56);
      this.labSubitem1Title.Name = "labSubitem1Title";
      this.labSubitem1Title.Size = new Size(168, 20);
      this.labSubitem1Title.TabIndex = 2;
      this.labSubitem1Title.Text = "Contact Access Rights";
      this.labSubitem1Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem1Summary.Location = new Point(16, 76);
      this.labSubitem1Summary.Name = "labSubitem1Summary";
      this.labSubitem1Summary.Size = new Size(168, 36);
      this.labSubitem1Summary.TabIndex = 3;
      this.labSubitem1Summary.Text = "Control access to borrower contacts.";
      this.labSubitem3Summary.Location = new Point(212, 76);
      this.labSubitem3Summary.Name = "labSubitem3Summary";
      this.labSubitem3Summary.Size = new Size(168, 36);
      this.labSubitem3Summary.TabIndex = 13;
      this.labSubitem3Summary.Text = "Create global custom fields for business contacts.";
      this.labSubitem3Title.Cursor = Cursors.Hand;
      this.labSubitem3Title.Location = new Point(212, 56);
      this.labSubitem3Title.Name = "labSubitem3Title";
      this.labSubitem3Title.Size = new Size(168, 20);
      this.labSubitem3Title.TabIndex = 12;
      this.labSubitem3Title.Text = "Business Custom Fields";
      this.labSubitem3Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem2Summary.Location = new Point(16, 144);
      this.labSubitem2Summary.Name = "labSubitem2Summary";
      this.labSubitem2Summary.Size = new Size(168, 36);
      this.labSubitem2Summary.TabIndex = 11;
      this.labSubitem2Summary.Text = "Create global custom fields for borrower contacts.";
      this.labSubitem2Title.Cursor = Cursors.Hand;
      this.labSubitem2Title.Location = new Point(16, 124);
      this.labSubitem2Title.Name = "labSubitem2Title";
      this.labSubitem2Title.Size = new Size(168, 20);
      this.labSubitem2Title.TabIndex = 10;
      this.labSubitem2Title.Text = "Borrower Custom Fields";
      this.labSubitem2Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem4Summary.Location = new Point(212, 144);
      this.labSubitem4Summary.Name = "labSubitem4Summary";
      this.labSubitem4Summary.Size = new Size(168, 36);
      this.labSubitem4Summary.TabIndex = 15;
      this.labSubitem4Summary.Text = "Create categories for business contacts.";
      this.labSubitem4Title.Cursor = Cursors.Hand;
      this.labSubitem4Title.Location = new Point(212, 124);
      this.labSubitem4Title.Name = "labSubitem4Title";
      this.labSubitem4Title.Size = new Size(168, 20);
      this.labSubitem4Title.TabIndex = 14;
      this.labSubitem4Title.Text = "Business Categories";
      this.labSubitem4Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem5Summary.Location = new Point(16, 212);
      this.labSubitem5Summary.Name = "labSubitem5Summary";
      this.labSubitem5Summary.Size = new Size(168, 48);
      this.labSubitem5Summary.TabIndex = 17;
      this.labSubitem5Summary.Text = "Create a predefined list of custom statuses for borrower contacts.";
      this.labSubitem5Title.Cursor = Cursors.Hand;
      this.labSubitem5Title.Location = new Point(16, 192);
      this.labSubitem5Title.Name = "labSubitem5Title";
      this.labSubitem5Title.Size = new Size(168, 20);
      this.labSubitem5Title.TabIndex = 16;
      this.labSubitem5Title.Text = "Borrower Contact Status";
      this.labSubitem5Title.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.labSubitem5Summary);
      this.Controls.Add((Control) this.labSubitem5Title);
      this.Controls.Add((Control) this.labSubitem4Summary);
      this.Controls.Add((Control) this.labSubitem4Title);
      this.Controls.Add((Control) this.labSubitem3Summary);
      this.Controls.Add((Control) this.labSubitem3Title);
      this.Controls.Add((Control) this.labSubitem2Summary);
      this.Controls.Add((Control) this.labSubitem2Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Controls.Add((Control) this.labSubitem1Summary);
      this.Controls.Add((Control) this.labSubitem1Title);
      this.Name = nameof (ContactSetupHelp);
      this.Size = new Size(543, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "labSubitem1Title":
          nodeText = "Contact Access Rights";
          break;
        case "labSubitem2Title":
          nodeText = "Borrower Custom Fields";
          break;
        case "labSubitem3Title":
          nodeText = "Business Custom Fields";
          break;
        case "labSubitem4Title":
          nodeText = "Business Categories";
          break;
        case "labSubitem5Title":
          nodeText = "Borrower Contact Status";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
