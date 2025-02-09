// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplateSetupHelp
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
  public class LoanTemplateSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem1Title;
    private Label labSubitem1Summary;
    private Label labSubitem2Summary;
    private Label labSubitem2Title;
    private IContainer components;
    private Label labSubitem5Summary;
    private Label labSubitem5Title;
    private Label labSubitem4Summary;
    private Label labSubitem4Title;
    private Label labSubitem6Summary;
    private Label labSubitem6Title;
    private Label labSubitem3Summary;
    private Label labSubitem3Title;
    private SetUpContainer setUpContainer;

    public LoanTemplateSetupHelp(SetUpContainer setUpContainer)
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
      this.labSubitem6Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem6Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem6Title.BackColor = this.defaultBackColor;
      this.labSubitem6Summary.BackColor = this.defaultBackColor;
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
      this.labSubitem2Summary = new Label();
      this.labSubitem2Title = new Label();
      this.labSubitem5Summary = new Label();
      this.labSubitem5Title = new Label();
      this.labSubitem4Summary = new Label();
      this.labSubitem4Title = new Label();
      this.labSubitem6Summary = new Label();
      this.labSubitem6Title = new Label();
      this.labSubitem3Summary = new Label();
      this.labSubitem3Title = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(128, 24);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Loan Template Setup";
      this.labItemSummary.Location = new Point(8, 32);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(400, 32);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Loan Template Setup options to create templates of loan information, including default data on loan forms.";
      this.labSubitem1Title.Cursor = Cursors.Hand;
      this.labSubitem1Title.Location = new Point(16, 76);
      this.labSubitem1Title.Name = "labSubitem1Title";
      this.labSubitem1Title.Size = new Size(184, 16);
      this.labSubitem1Title.TabIndex = 2;
      this.labSubitem1Title.Text = "Loan Templates";
      this.labSubitem1Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem1Summary.Location = new Point(16, 92);
      this.labSubitem1Summary.Name = "labSubitem1Summary";
      this.labSubitem1Summary.Size = new Size(184, 44);
      this.labSubitem1Summary.TabIndex = 3;
      this.labSubitem1Summary.Text = "Create templates of loan data and documents for frequently-used loan scenarios.";
      this.labSubitem2Summary.Location = new Point(220, 92);
      this.labSubitem2Summary.Name = "labSubitem2Summary";
      this.labSubitem2Summary.Size = new Size(168, 32);
      this.labSubitem2Summary.TabIndex = 5;
      this.labSubitem2Summary.Text = "Create templates of default loan data.";
      this.labSubitem2Title.Cursor = Cursors.Hand;
      this.labSubitem2Title.Location = new Point(220, 76);
      this.labSubitem2Title.Name = "labSubitem2Title";
      this.labSubitem2Title.Size = new Size(168, 16);
      this.labSubitem2Title.TabIndex = 4;
      this.labSubitem2Title.Text = "Misc. Data Templates";
      this.labSubitem2Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem5Summary.Location = new Point(16, 252);
      this.labSubitem5Summary.Name = "labSubitem5Summary";
      this.labSubitem5Summary.Size = new Size(184, 48);
      this.labSubitem5Summary.TabIndex = 13;
      this.labSubitem5Summary.Text = "Create sets (templates) of forms to display in the input forms list on the loan workspace.";
      this.labSubitem5Title.Cursor = Cursors.Hand;
      this.labSubitem5Title.Location = new Point(16, 232);
      this.labSubitem5Title.Name = "labSubitem5Title";
      this.labSubitem5Title.Size = new Size(184, 20);
      this.labSubitem5Title.TabIndex = 12;
      this.labSubitem5Title.Text = "Input Form Lists";
      this.labSubitem5Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem4Summary.Location = new Point(16, 164);
      this.labSubitem4Summary.Name = "labSubitem4Summary";
      this.labSubitem4Summary.Size = new Size(184, 60);
      this.labSubitem4Summary.TabIndex = 15;
      this.labSubitem4Summary.Text = "Create sets (templates) of documents to meet the requirements of various loan scenarios or particular lenders.";
      this.labSubitem4Title.Cursor = Cursors.Hand;
      this.labSubitem4Title.Location = new Point(16, 144);
      this.labSubitem4Title.Name = "labSubitem4Title";
      this.labSubitem4Title.Size = new Size(184, 20);
      this.labSubitem4Title.TabIndex = 14;
      this.labSubitem4Title.Text = "Document Sets";
      this.labSubitem4Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem6Summary.Location = new Point(220, 236);
      this.labSubitem6Summary.Name = "labSubitem6Summary";
      this.labSubitem6Summary.Size = new Size(168, 60);
      this.labSubitem6Summary.TabIndex = 19;
      this.labSubitem6Summary.Text = "Create template of predefined values that appear primarily on the Good Faith Estimate and the 1003 application.";
      this.labSubitem6Title.Cursor = Cursors.Hand;
      this.labSubitem6Title.Location = new Point(220, 216);
      this.labSubitem6Title.Name = "labSubitem6Title";
      this.labSubitem6Title.Size = new Size(168, 20);
      this.labSubitem6Title.TabIndex = 18;
      this.labSubitem6Title.Text = "Closing Costs";
      this.labSubitem6Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem3Summary.Location = new Point(220, 152);
      this.labSubitem3Summary.Name = "labSubitem3Summary";
      this.labSubitem3Summary.Size = new Size(168, 56);
      this.labSubitem3Summary.TabIndex = 17;
      this.labSubitem3Summary.Text = "Create template of predefined values that appear primarily on the Truth-In-Lending Disclosure and the 1003 application.";
      this.labSubitem3Title.Cursor = Cursors.Hand;
      this.labSubitem3Title.Location = new Point(220, 132);
      this.labSubitem3Title.Name = "labSubitem3Title";
      this.labSubitem3Title.Size = new Size(168, 20);
      this.labSubitem3Title.TabIndex = 16;
      this.labSubitem3Title.Text = "Loan Programs";
      this.labSubitem3Title.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.labSubitem6Summary);
      this.Controls.Add((Control) this.labSubitem6Title);
      this.Controls.Add((Control) this.labSubitem3Summary);
      this.Controls.Add((Control) this.labSubitem3Title);
      this.Controls.Add((Control) this.labSubitem4Summary);
      this.Controls.Add((Control) this.labSubitem4Title);
      this.Controls.Add((Control) this.labSubitem5Summary);
      this.Controls.Add((Control) this.labSubitem5Title);
      this.Controls.Add((Control) this.labSubitem2Summary);
      this.Controls.Add((Control) this.labSubitem2Title);
      this.Controls.Add((Control) this.labSubitem1Summary);
      this.Controls.Add((Control) this.labSubitem1Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (LoanTemplateSetupHelp);
      this.Size = new Size(545, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      string nodeText = "";
      string name = label.Name;
      if (name == "labSubitem1Title" || name == "labSubitem2Title" || name == "labSubitem3Title" || name == "labSubitem4Title" || name == "labSubitem5Title" || name == "labSubitem6Title")
        nodeText = label.Text;
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
