// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FileRightsHelp
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
  public class FileRightsHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem1Title;
    private Label labSubitem1Summary;
    private Label labSubitem2Summary;
    private Label labSubitem2Title;
    private IContainer components;
    private SetUpContainer setUpContainer;

    public FileRightsHelp(SetUpContainer setUpContainer)
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
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(480, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "File Rights";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(480, 16);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the File Rights options to assign file rights to other users and to unlock loan files.";
      this.labSubitem1Title.Cursor = Cursors.Hand;
      this.labSubitem1Title.Location = new Point(16, 56);
      this.labSubitem1Title.Name = "labSubitem1Title";
      this.labSubitem1Title.Size = new Size(168, 16);
      this.labSubitem1Title.TabIndex = 2;
      this.labSubitem1Title.Text = "Assignment of Rights";
      this.labSubitem1Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem1Summary.Location = new Point(16, 72);
      this.labSubitem1Summary.Name = "labSubitem1Summary";
      this.labSubitem1Summary.Size = new Size(168, 48);
      this.labSubitem1Summary.TabIndex = 3;
      this.labSubitem1Summary.Text = "Assign and revoke Read/Write and Full rights access to your loans.";
      this.labSubitem2Summary.Location = new Point(16, 144);
      this.labSubitem2Summary.Name = "labSubitem2Summary";
      this.labSubitem2Summary.Size = new Size(168, 100);
      this.labSubitem2Summary.TabIndex = 5;
      this.labSubitem2Summary.Text = "Unlock a read-only file. When a loan file is downloaded from the server or opened by another user, the access rights of the file are changed to read-only until it is unlocked.";
      this.labSubitem2Title.Cursor = Cursors.Hand;
      this.labSubitem2Title.Location = new Point(16, 128);
      this.labSubitem2Title.Name = "labSubitem2Title";
      this.labSubitem2Title.Size = new Size(168, 16);
      this.labSubitem2Title.TabIndex = 4;
      this.labSubitem2Title.Text = "Unlock Loan Files";
      this.labSubitem2Title.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.labSubitem2Summary);
      this.Controls.Add((Control) this.labSubitem2Title);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Controls.Add((Control) this.labSubitem1Summary);
      this.Controls.Add((Control) this.labSubitem1Title);
      this.Name = nameof (FileRightsHelp);
      this.Size = new Size(473, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "labSubitem1Title":
          nodeText = "Assignment of Rights";
          break;
        case "labSubitem2Title":
          nodeText = "Unlock Loan Files";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
