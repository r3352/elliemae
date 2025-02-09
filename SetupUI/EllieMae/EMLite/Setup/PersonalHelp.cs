// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonalHelp
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
  public class PersonalHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem1Title;
    private Label labSubitem1Summary;
    private Label labSubitem2Summary;
    private Label labSubitem2Title;
    private IContainer components;
    private Label labSubitem3Summary;
    private Label labSubitem3Title;
    private Label labSubitem5Summary;
    private Label labSubitem5Title;
    private SetUpContainer setUpContainer;

    public PersonalHelp(SetUpContainer setUpContainer)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PersonalHelp));
      this.labItemTitle = new Label();
      this.labItemSummary = new Label();
      this.labSubitem1Title = new Label();
      this.labSubitem1Summary = new Label();
      this.labSubitem2Summary = new Label();
      this.labSubitem2Title = new Label();
      this.labSubitem3Summary = new Label();
      this.labSubitem3Title = new Label();
      this.labSubitem5Summary = new Label();
      this.labSubitem5Title = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(460, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Personal Settings";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(460, 16);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Personal Settings options to manage your user profile and default file contacts.";
      this.labSubitem1Title.Cursor = Cursors.Hand;
      this.labSubitem1Title.Location = new Point(16, 56);
      this.labSubitem1Title.Name = "labSubitem1Title";
      this.labSubitem1Title.Size = new Size(168, 16);
      this.labSubitem1Title.TabIndex = 2;
      this.labSubitem1Title.Text = "Profile";
      this.labSubitem1Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem1Summary.Location = new Point(16, 72);
      this.labSubitem1Summary.Name = "labSubitem1Summary";
      this.labSubitem1Summary.Size = new Size(168, 108);
      this.labSubitem1Summary.TabIndex = 3;
      this.labSubitem1Summary.Text = componentResourceManager.GetString("labSubitem1Summary.Text");
      this.labSubitem2Summary.Location = new Point(16, 204);
      this.labSubitem2Summary.Name = "labSubitem2Summary";
      this.labSubitem2Summary.Size = new Size(172, 108);
      this.labSubitem2Summary.TabIndex = 5;
      this.labSubitem2Summary.Text = componentResourceManager.GetString("labSubitem2Summary.Text");
      this.labSubitem2Title.Cursor = Cursors.Hand;
      this.labSubitem2Title.Location = new Point(16, 188);
      this.labSubitem2Title.Name = "labSubitem2Title";
      this.labSubitem2Title.Size = new Size(172, 16);
      this.labSubitem2Title.TabIndex = 4;
      this.labSubitem2Title.Text = "Default Providers";
      this.labSubitem2Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem3Summary.Location = new Point(16, 336);
      this.labSubitem3Summary.Name = "labSubitem3Summary";
      this.labSubitem3Summary.Size = new Size(172, 44);
      this.labSubitem3Summary.TabIndex = 9;
      this.labSubitem3Summary.Text = "Configure the default options and triggers for the Status Online feature.";
      this.labSubitem3Title.Cursor = Cursors.Hand;
      this.labSubitem3Title.Location = new Point(16, 320);
      this.labSubitem3Title.Name = "labSubitem3Title";
      this.labSubitem3Title.Size = new Size(172, 16);
      this.labSubitem3Title.TabIndex = 8;
      this.labSubitem3Title.Text = "Status Online Configuration";
      this.labSubitem3Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem5Summary.Location = new Point(204, 72);
      this.labSubitem5Summary.Name = "labSubitem5Summary";
      this.labSubitem5Summary.Size = new Size(172, 44);
      this.labSubitem5Summary.TabIndex = 13;
      this.labSubitem5Summary.Text = "Configure and modify your autosave settings on this computer.";
      this.labSubitem5Title.Cursor = Cursors.Hand;
      this.labSubitem5Title.Location = new Point(204, 56);
      this.labSubitem5Title.Name = "labSubitem5Title";
      this.labSubitem5Title.Size = new Size(172, 16);
      this.labSubitem5Title.TabIndex = 12;
      this.labSubitem5Title.Text = "Autosave Configuration";
      this.labSubitem5Title.Click += new EventHandler(this.labelHeader_Click);
      this.ClientSize = new Size(497, 424);
      this.Controls.Add((Control) this.labSubitem5Summary);
      this.Controls.Add((Control) this.labSubitem5Title);
      this.Controls.Add((Control) this.labSubitem3Summary);
      this.Controls.Add((Control) this.labSubitem3Title);
      this.Controls.Add((Control) this.labSubitem2Summary);
      this.Controls.Add((Control) this.labSubitem2Title);
      this.Controls.Add((Control) this.labSubitem1Summary);
      this.Controls.Add((Control) this.labSubitem1Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (PersonalHelp);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      string nodeText = "";
      string name = label.Name;
      if (name == "labSubitem1Title" || name == "labSubitem2Title" || name == "labSubitem3Title" || name == "labSubitem4Title" || name == "labSubitem5Title")
        nodeText = label.Text;
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
