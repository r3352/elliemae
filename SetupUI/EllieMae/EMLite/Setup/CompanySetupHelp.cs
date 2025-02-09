// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanySetupHelp
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CompanySetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label labSubitem1Title;
    private Label labSubitem1Summary;
    private Label labSubitem7Summary;
    private Label labSubitem7Title;
    private IContainer components;
    private Panel panel1;
    private GroupBox groupBox1;
    private SetUpContainer setUpContainer;

    public CompanySetupHelp(SetUpContainer setUpContainer)
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
      this.labSubitem1Summary.Text = "Set up company information that displays on reports and forms, a company email signature, and licensing data for your company and its branches.";
      this.labSubitem7Title.Font = this.defaultSubItemTitleFont;
      this.labSubitem7Title.ForeColor = this.defaultSubItemTitleForeColor;
      this.labSubitem7Title.BackColor = this.defaultBackColor;
      this.labSubitem7Summary.BackColor = this.defaultBackColor;
      if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
        return;
      this.labSubitem7Title.Visible = false;
      this.labSubitem7Summary.Visible = false;
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
      this.labSubitem7Summary = new Label();
      this.labSubitem7Title = new Label();
      this.panel1 = new Panel();
      this.groupBox1 = new GroupBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.labItemTitle.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labItemTitle.ForeColor = Color.Black;
      this.labItemTitle.Location = new Point(0, 15);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(492, 17);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "Company Setup";
      this.labItemTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.labItemSummary.Dock = DockStyle.Top;
      this.labItemSummary.Location = new Point(0, 56);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(752, 36);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the Company Setup options to manage basic information for your company and change the company password.";
      this.labSubitem1Title.Cursor = Cursors.Hand;
      this.labSubitem1Title.Location = new Point(4, 124);
      this.labSubitem1Title.Name = "labSubitem1Title";
      this.labSubitem1Title.Size = new Size(168, 16);
      this.labSubitem1Title.TabIndex = 2;
      this.labSubitem1Title.Text = "Company Information";
      this.labSubitem1Title.Click += new EventHandler(this.labelHeader_Click);
      this.labSubitem1Summary.Location = new Point(4, 144);
      this.labSubitem1Summary.Name = "labSubitem1Summary";
      this.labSubitem1Summary.Size = new Size(728, 16);
      this.labSubitem1Summary.TabIndex = 3;
      this.labSubitem7Summary.Location = new Point(4, 192);
      this.labSubitem7Summary.Name = "labSubitem7Summary";
      this.labSubitem7Summary.Size = new Size(668, 16);
      this.labSubitem7Summary.TabIndex = 15;
      this.labSubitem7Summary.Text = "Make changes to the company password created by the system administrator during initial installation of Encompass.";
      this.labSubitem7Title.Cursor = Cursors.Hand;
      this.labSubitem7Title.Location = new Point(4, 172);
      this.labSubitem7Title.Name = "labSubitem7Title";
      this.labSubitem7Title.Size = new Size(168, 16);
      this.labSubitem7Title.TabIndex = 14;
      this.labSubitem7Title.Text = "Company Password";
      this.labSubitem7Title.Click += new EventHandler(this.labelHeader_Click);
      this.panel1.Controls.Add((Control) this.groupBox1);
      this.panel1.Controls.Add((Control) this.labItemTitle);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(752, 56);
      this.panel1.TabIndex = 16;
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Location = new Point(0, 42);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(899, 4);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.labSubitem1Summary);
      this.Controls.Add((Control) this.labSubitem1Title);
      this.Controls.Add((Control) this.labSubitem7Summary);
      this.Controls.Add((Control) this.labSubitem7Title);
      this.Name = nameof (CompanySetupHelp);
      this.Size = new Size(752, 569);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "labSubitem1Title":
          nodeText = "Company Information";
          break;
        case "labSubitem7Title":
          nodeText = "Company Password";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}
