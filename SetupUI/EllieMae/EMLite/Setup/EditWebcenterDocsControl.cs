// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditWebcenterDocsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditWebcenterDocsControl : UserControl
  {
    private const string className = "EditWebcenterDocsControl";
    private static string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private EditCompanyActiveDocControl companyActiveDocumentControl;
    private EditCompanyArchiveDocControl companyArchiveDocumentControl;
    private IContainer components;
    private GroupContainer gcAll;
    private Panel panelHeader;
    private Label lblTPOWCDocsHeader;
    private TabControlEx tabControlExAll;
    private TabPageEx tabPageExActive;
    private TabPageEx tabPageExArchived;

    public EditWebcenterDocsControl(
      Sessions.Session session,
      int orgID,
      int companyOrgId,
      bool isTPOTool)
    {
      this.session = session;
      this.InitializeComponent();
      if (companyOrgId == -1 || companyOrgId == 0)
        companyOrgId = orgID;
      if (companyOrgId != orgID)
        this.lblTPOWCDocsHeader.Text = "This TPO can view the Active documents below on your TPO Connect site. These documents can be managed within the highest company organization folder for this TPO.";
      this.companyActiveDocumentControl = new EditCompanyActiveDocControl(session, orgID, companyOrgId, isTPOTool, this.tabControlExAll.Parent.Text);
      this.tabPageExActive.Controls.Add((Control) this.companyActiveDocumentControl);
      this.companyArchiveDocumentControl = new EditCompanyArchiveDocControl(session, orgID, companyOrgId, isTPOTool, this.tabControlExAll.Parent.Text);
      this.tabPageExArchived.Controls.Add((Control) this.companyArchiveDocumentControl);
      this.companyArchiveDocumentControl.Dock = DockStyle.Fill;
      this.companyActiveDocumentControl.Dock = DockStyle.Fill;
      this.Dock = DockStyle.Fill;
    }

    public void DisableControls()
    {
      this.companyActiveDocumentControl.DisableControls();
      this.companyArchiveDocumentControl.DisableControls();
    }

    private void tabControlExAll_SelectedIndexChanged(object sender, EventArgs e)
    {
      TabPageEx selectedPage = this.tabControlExAll.SelectedPage;
      this.tabControlExAll.SelectedIndexChanged -= new EventHandler(this.tabControlExAll_SelectedIndexChanged);
      if (selectedPage == this.tabPageExActive)
        this.companyActiveDocumentControl.RefreshData();
      else if (selectedPage == this.tabPageExArchived)
        this.companyArchiveDocumentControl.RefreshData();
      this.tabControlExAll.SelectedIndexChanged += new EventHandler(this.tabControlExAll_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditWebcenterDocsControl));
      this.gcAll = new GroupContainer();
      this.tabControlExAll = new TabControlEx();
      this.tabPageExActive = new TabPageEx();
      this.tabPageExArchived = new TabPageEx();
      this.panelHeader = new Panel();
      this.lblTPOWCDocsHeader = new Label();
      this.gcAll.SuspendLayout();
      this.tabControlExAll.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.gcAll.Controls.Add((Control) this.tabControlExAll);
      this.gcAll.Controls.Add((Control) this.panelHeader);
      this.gcAll.Dock = DockStyle.Fill;
      this.gcAll.HeaderForeColor = SystemColors.ControlText;
      this.gcAll.Location = new Point(5, 5);
      this.gcAll.Margin = new Padding(0);
      this.gcAll.Name = "gcAll";
      this.gcAll.Size = new Size(862, 610);
      this.gcAll.TabIndex = 12;
      this.gcAll.Text = "TPO Docs";
      this.tabControlExAll.Dock = DockStyle.Fill;
      this.tabControlExAll.Location = new Point(1, 68);
      this.tabControlExAll.Margin = new Padding(0);
      this.tabControlExAll.Name = "tabControlExAll";
      this.tabControlExAll.Size = new Size(860, 541);
      this.tabControlExAll.TabHeight = 20;
      this.tabControlExAll.TabIndex = 1;
      this.tabControlExAll.TabPages.Add(this.tabPageExActive);
      this.tabControlExAll.TabPages.Add(this.tabPageExArchived);
      this.tabControlExAll.Text = "tabControlEx1";
      this.tabControlExAll.SelectedIndexChanged += new EventHandler(this.tabControlExAll_SelectedIndexChanged);
      this.tabPageExActive.BackColor = Color.Transparent;
      this.tabPageExActive.Location = new Point(1, 23);
      this.tabPageExActive.Name = "tabPageExActive";
      this.tabPageExActive.TabIndex = 0;
      this.tabPageExActive.TabWidth = 100;
      this.tabPageExActive.Text = "TPO Docs";
      this.tabPageExActive.Value = (object) "TPO Docs";
      this.tabPageExArchived.BackColor = Color.Transparent;
      this.tabPageExArchived.Location = new Point(0, 0);
      this.tabPageExArchived.Name = "tabPageExArchived";
      this.tabPageExArchived.TabIndex = 0;
      this.tabPageExArchived.TabWidth = 100;
      this.tabPageExArchived.Text = "Archived Docs";
      this.tabPageExArchived.Value = (object) "Archived Docs";
      this.panelHeader.Controls.Add((Control) this.lblTPOWCDocsHeader);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 42);
      this.panelHeader.TabIndex = 0;
      this.lblTPOWCDocsHeader.Location = new Point(-1, 4);
      this.lblTPOWCDocsHeader.Name = "lblTPOWCDocsHeader";
      this.lblTPOWCDocsHeader.Padding = new Padding(5, 0, 0, 0);
      this.lblTPOWCDocsHeader.Size = new Size(860, 36);
      this.lblTPOWCDocsHeader.TabIndex = 36;
      this.lblTPOWCDocsHeader.Text = componentResourceManager.GetString("lblTPOWCDocsHeader.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditWebcenterDocsControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.gcAll.ResumeLayout(false);
      this.tabControlExAll.ResumeLayout(false);
      this.panelHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
