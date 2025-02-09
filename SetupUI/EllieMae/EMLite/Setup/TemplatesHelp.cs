// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplatesHelp
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
  public class TemplatesHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem3Summary;
    private Label labSubitem3Title;
    private Label labSubitem6Summary;
    private Label labSubitem6Title;
    private IContainer components;
    private SetUpContainer setUpContainer;

    public TemplatesHelp(SetUpContainer setUpContainer)
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
      this.labSubitem3Summary = new Label();
      this.labSubitem3Title = new Label();
      this.labSubitem6Summary = new Label();
      this.labSubitem6Title = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(388, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Templates";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(388, 32);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Templates options to reduce data entry time and improve accuracy by creating templates of default data and frequently-used loan information.";
      this.labSubitem3Summary.Location = new Point(16, 172);
      this.labSubitem3Summary.Name = "labSubitem3Summary";
      this.labSubitem3Summary.Size = new Size(184, 32);
      this.labSubitem3Summary.TabIndex = 7;
      this.labSubitem3Summary.Text = "Create custom templates for forms, letters, and other documents.";
      this.labSubitem3Title.Cursor = Cursors.Hand;
      this.labSubitem3Title.Location = new Point(16, 152);
      this.labSubitem3Title.Name = "labSubitem3Title";
      this.labSubitem3Title.Size = new Size(184, 20);
      this.labSubitem3Title.TabIndex = 6;
      this.labSubitem3Title.Text = "Custom Forms";
      this.labSubitem3Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem6Summary.Location = new Point(16, 96);
      this.labSubitem6Summary.Name = "labSubitem6Summary";
      this.labSubitem6Summary.Size = new Size(184, 44);
      this.labSubitem6Summary.TabIndex = 13;
      this.labSubitem6Summary.Text = "Create templates of loan information, including default data on loan forms.";
      this.labSubitem6Title.Cursor = Cursors.Hand;
      this.labSubitem6Title.Location = new Point(16, 76);
      this.labSubitem6Title.Name = "labSubitem6Title";
      this.labSubitem6Title.Size = new Size(184, 20);
      this.labSubitem6Title.TabIndex = 12;
      this.labSubitem6Title.Text = "Loan Template Setup";
      this.labSubitem6Title.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.labSubitem6Summary);
      this.Controls.Add((Control) this.labSubitem6Title);
      this.Controls.Add((Control) this.labSubitem3Summary);
      this.Controls.Add((Control) this.labSubitem3Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (TemplatesHelp);
      this.Size = new Size(545, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "labSubitem3Title":
          nodeText = "Custom Forms";
          break;
        case "labSubitem6Title":
          nodeText = "Loan Template Setup";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
