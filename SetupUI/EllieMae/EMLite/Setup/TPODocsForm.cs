// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPODocsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TPODocsForm : UserControl, IOnlineHelpTarget
  {
    private const string className = "AllTPOUserPanel";
    private static string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private SetUpContainer setupContainer;
    private IContainer components;
    private IOrganizationManager rOrg = Session.OrganizationManager;
    private ToolTip toolTip1;
    private TabControlEx tabControlDocsAll;
    private TabPageEx tabPageActiveDoc;
    private TabPageEx tabPageArchiveDocs;
    private EditActiveDocumentControl activeDocumentControl;
    private ArchiveDocumentList archiveDocumentControl;

    public TPODocsForm(Sessions.Session session, SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      this.InitializeComponent();
      this.activeDocumentControl = new EditActiveDocumentControl(session);
      this.tabPageActiveDoc.Controls.Add((Control) this.activeDocumentControl);
      this.archiveDocumentControl = new ArchiveDocumentList(session);
      this.tabPageArchiveDocs.Controls.Add((Control) this.archiveDocumentControl);
      this.archiveDocumentControl.Dock = DockStyle.Fill;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTip1 = new ToolTip(this.components);
      this.tabControlDocsAll = new TabControlEx();
      this.tabPageActiveDoc = new TabPageEx();
      this.tabPageArchiveDocs = new TabPageEx();
      this.tabControlDocsAll.SuspendLayout();
      this.SuspendLayout();
      this.tabControlDocsAll.Dock = DockStyle.Fill;
      this.tabControlDocsAll.Location = new Point(0, 0);
      this.tabControlDocsAll.Name = "tabControlDocsAll";
      this.tabControlDocsAll.Size = new Size(855, 357);
      this.tabControlDocsAll.TabHeight = 20;
      this.tabControlDocsAll.TabIndex = 24;
      this.tabControlDocsAll.TabPages.Add(this.tabPageActiveDoc);
      this.tabControlDocsAll.TabPages.Add(this.tabPageArchiveDocs);
      this.tabControlDocsAll.Text = "tabControlEx1";
      this.tabControlDocsAll.SelectedIndexChanged += new EventHandler(this.tabControlDocsAll_SelectedIndexChanged);
      this.tabPageActiveDoc.BackColor = Color.Transparent;
      this.tabPageActiveDoc.Location = new Point(1, 23);
      this.tabPageActiveDoc.Name = "tabPageActiveDoc";
      this.tabPageActiveDoc.TabIndex = 23;
      this.tabPageActiveDoc.TabWidth = 150;
      this.tabPageActiveDoc.Text = "TPO Docs";
      this.tabPageActiveDoc.Value = (object) "TPO Docs";
      this.tabPageArchiveDocs.BackColor = Color.Transparent;
      this.tabPageArchiveDocs.Location = new Point(0, 0);
      this.tabPageArchiveDocs.Name = "tabPageArchiveDocs";
      this.tabPageArchiveDocs.TabIndex = 0;
      this.tabPageArchiveDocs.TabWidth = 150;
      this.tabPageArchiveDocs.Text = "Archived Docs";
      this.tabPageArchiveDocs.Value = (object) "Archived Docs";
      this.Controls.Add((Control) this.tabControlDocsAll);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TPODocsForm);
      this.Size = new Size(855, 357);
      this.tabControlDocsAll.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void tabControlDocsAll_SelectedIndexChanged(object sender, EventArgs e)
    {
      TabPageEx selectedPage = this.tabControlDocsAll.SelectedPage;
      this.tabControlDocsAll.SelectedIndexChanged -= new EventHandler(this.tabControlDocsAll_SelectedIndexChanged);
      if (selectedPage == this.tabPageActiveDoc)
        this.activeDocumentControl.RefreshData();
      else if (selectedPage == this.tabPageArchiveDocs)
        this.archiveDocumentControl.RefreshData();
      this.tabControlDocsAll.SelectedIndexChanged += new EventHandler(this.tabControlDocsAll_SelectedIndexChanged);
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO WebCenter Docs";
  }
}
