// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolderPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ePass;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class eFolderPage : Form, IPersonaSecurityPage
  {
    private ConditionsPage conditionsPage;
    private UnprotectedDocsPage unprotectedDocsPage;
    private ProtectedDocsPage protectedDocsPage;
    private UnassignedFilesPage unassignedFilesPage;
    private AccessToDocumentTabPage accessToDocTabPage;
    private OtherPage otherPage;
    private bool bIsUserSetup;
    private int personaId = -1;
    private string userId;
    private Persona[] personas;
    private Sessions.Session session;
    private PipelineConfiguration pipelineConfiguration;
    private IContainer components;
    private Panel pnlLeft;
    private Panel pnlRight;
    private Panel pnlConditions;
    private Panel pnlProtectedDoc;
    private Panel pnlUnassignedFiles;
    private Panel pnlUnprotectedDocs;
    private Panel pnlBottom;
    private Panel pnlMiddle;
    private Splitter splitter5;
    private Splitter splitter1;
    private Panel pnlGeneral;
    private Panel pnlTop;
    private Splitter splitter2;
    private Splitter splitter3;

    public eFolderPage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.pipelineConfiguration = pipelineConfiguration;
      this.init(false, (string) null, (Persona[]) null, personaId, dirtyFlagChanged);
    }

    public eFolderPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.pipelineConfiguration = pipelineConfiguration;
      this.init(true, userId, personas, -1, dirtyFlagChanged);
    }

    private void init(
      bool isUserSetup,
      string userId,
      Persona[] personas,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      if (isUserSetup)
      {
        this.bIsUserSetup = isUserSetup;
        this.userId = userId;
        this.personas = personas;
        this.accessToDocTabPage = new AccessToDocumentTabPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration);
        this.conditionsPage = new ConditionsPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration);
        this.unprotectedDocsPage = new UnprotectedDocsPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.protectedDocsPage = new ProtectedDocsPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.unassignedFilesPage = new UnassignedFilesPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.otherPage = new OtherPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
      }
      else
      {
        this.personaId = personaId;
        this.accessToDocTabPage = new AccessToDocumentTabPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration);
        this.conditionsPage = new ConditionsPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration);
        this.unprotectedDocsPage = new UnprotectedDocsPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.protectedDocsPage = new ProtectedDocsPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.unassignedFilesPage = new UnassignedFilesPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
        this.otherPage = new OtherPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration, this.accessToDocTabPage);
      }
      this.accessToDocTabPage.TopLevel = false;
      this.accessToDocTabPage.ShowGroupContainer = false;
      this.accessToDocTabPage.Visible = true;
      this.accessToDocTabPage.Dock = DockStyle.Fill;
      this.pnlTop.Controls.Add((Control) this.accessToDocTabPage);
      this.conditionsPage.TopLevel = false;
      this.conditionsPage.Visible = true;
      this.conditionsPage.Dock = DockStyle.Fill;
      this.pnlConditions.Controls.Add((Control) this.conditionsPage);
      this.unprotectedDocsPage.TopLevel = false;
      this.unprotectedDocsPage.Visible = true;
      this.unprotectedDocsPage.Dock = DockStyle.Fill;
      this.pnlUnprotectedDocs.Controls.Add((Control) this.unprotectedDocsPage);
      this.protectedDocsPage.TopLevel = false;
      this.protectedDocsPage.Visible = true;
      this.protectedDocsPage.Dock = DockStyle.Fill;
      this.pnlProtectedDoc.Controls.Add((Control) this.protectedDocsPage);
      this.unassignedFilesPage.TopLevel = false;
      this.unassignedFilesPage.Visible = true;
      this.unassignedFilesPage.Dock = DockStyle.Fill;
      this.pnlUnassignedFiles.Controls.Add((Control) this.unassignedFilesPage);
      this.otherPage.TopLevel = false;
      this.otherPage.Visible = true;
      this.otherPage.Dock = DockStyle.Fill;
      this.pnlGeneral.Controls.Add((Control) this.otherPage);
      this.conditionsPage.BackColor = EncompassColors.Neutral3;
    }

    public void ResetData()
    {
      this.accessToDocTabPage.ResetData();
      this.conditionsPage.ResetData();
      this.unprotectedDocsPage.ResetData();
      this.protectedDocsPage.ResetData();
      this.unassignedFilesPage.ResetData();
      this.otherPage.ResetData();
    }

    public bool NeedToSaveData()
    {
      return this.accessToDocTabPage.NeedToSaveData() || this.conditionsPage.NeedToSaveData() || this.unprotectedDocsPage.NeedToSaveData() || this.protectedDocsPage.NeedToSaveData() || this.unassignedFilesPage.NeedToSaveData() || this.otherPage.NeedToSaveData();
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.accessToDocTabPage.MakeReadOnly(makeReadOnly);
      this.conditionsPage.MakeReadOnly(makeReadOnly);
      this.unprotectedDocsPage.MakeReadOnly(makeReadOnly);
      this.protectedDocsPage.MakeReadOnly(makeReadOnly);
      this.unassignedFilesPage.MakeReadOnly(makeReadOnly);
      this.otherPage.MakeReadOnly(makeReadOnly);
    }

    public void SaveData()
    {
      this.accessToDocTabPage.SaveData();
      this.conditionsPage.SaveData();
      this.unprotectedDocsPage.SaveData();
      this.protectedDocsPage.SaveData();
      this.unassignedFilesPage.SaveData();
      this.otherPage.SaveData();
    }

    public virtual void SetPersona(int personaId)
    {
      if (this.personaId != personaId)
      {
        this.personaId = personaId;
        this.ResetData();
      }
      this.accessToDocTabPage.SetPersona(personaId);
      this.conditionsPage.SetPersona(personaId);
      this.unprotectedDocsPage.SetPersona(personaId);
      this.protectedDocsPage.SetPersona(personaId);
      this.unassignedFilesPage.SetPersona(personaId);
      this.otherPage.SetPersona(personaId);
      this.ResetData();
    }

    private void eFolderPage_SizeChanged(object sender, EventArgs e)
    {
      this.pnlLeft.Size = new Size(this.Width / 2, this.pnlMiddle.Height);
      Panel pnlGeneral = this.pnlGeneral;
      Panel pnlUnassignedFiles = this.pnlUnassignedFiles;
      Size size1 = new Size(this.Width / 2, this.pnlMiddle.Height / 2);
      Size size2 = size1;
      pnlUnassignedFiles.Size = size2;
      Size size3 = size1;
      pnlGeneral.Size = size3;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlLeft = new Panel();
      this.pnlUnassignedFiles = new Panel();
      this.pnlUnprotectedDocs = new Panel();
      this.pnlRight = new Panel();
      this.pnlGeneral = new Panel();
      this.pnlProtectedDoc = new Panel();
      this.pnlConditions = new Panel();
      this.pnlBottom = new Panel();
      this.pnlMiddle = new Panel();
      this.splitter1 = new Splitter();
      this.splitter5 = new Splitter();
      this.pnlTop = new Panel();
      this.splitter2 = new Splitter();
      this.splitter3 = new Splitter();
      this.pnlLeft.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.pnlMiddle.SuspendLayout();
      this.SuspendLayout();
      this.pnlLeft.Controls.Add((Control) this.pnlUnprotectedDocs);
      this.pnlLeft.Controls.Add((Control) this.splitter2);
      this.pnlLeft.Controls.Add((Control) this.pnlGeneral);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(323, 420);
      this.pnlLeft.TabIndex = 0;
      this.pnlUnassignedFiles.Dock = DockStyle.Top;
      this.pnlUnassignedFiles.Location = new Point(0, 0);
      this.pnlUnassignedFiles.Name = "pnlUnassignedFiles";
      this.pnlUnassignedFiles.Size = new Size(322, 219);
      this.pnlUnassignedFiles.TabIndex = 4;
      this.pnlUnprotectedDocs.Dock = DockStyle.Fill;
      this.pnlUnprotectedDocs.Location = new Point(0, 0);
      this.pnlUnprotectedDocs.Name = "pnlUnprotectedDocs";
      this.pnlUnprotectedDocs.Size = new Size(323, 420);
      this.pnlUnprotectedDocs.TabIndex = 0;
      this.pnlRight.Controls.Add((Control) this.pnlProtectedDoc);
      this.pnlRight.Controls.Add((Control) this.splitter3);
      this.pnlRight.Controls.Add((Control) this.pnlUnassignedFiles);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(326, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(322, 420);
      this.pnlRight.TabIndex = 2;
      this.pnlGeneral.Dock = DockStyle.Top;
      this.pnlGeneral.Location = new Point(0, 0);
      this.pnlGeneral.Name = "pnlGeneral";
      this.pnlGeneral.Size = new Size(323, 219);
      this.pnlGeneral.TabIndex = 2;
      this.pnlProtectedDoc.Dock = DockStyle.Fill;
      this.pnlProtectedDoc.Location = new Point(0, 0);
      this.pnlProtectedDoc.Name = "pnlProtectedDoc";
      this.pnlProtectedDoc.Size = new Size(322, 420);
      this.pnlProtectedDoc.TabIndex = 0;
      this.pnlConditions.Dock = DockStyle.Fill;
      this.pnlConditions.Location = new Point(0, 0);
      this.pnlConditions.Name = "pnlConditions";
      this.pnlConditions.Size = new Size(648, 125);
      this.pnlConditions.TabIndex = 2;
      this.pnlBottom.Controls.Add((Control) this.pnlConditions);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 464);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(648, 125);
      this.pnlBottom.TabIndex = 3;
      this.pnlMiddle.Controls.Add((Control) this.pnlRight);
      this.pnlMiddle.Controls.Add((Control) this.splitter1);
      this.pnlMiddle.Controls.Add((Control) this.pnlLeft);
      this.pnlMiddle.Dock = DockStyle.Fill;
      this.pnlMiddle.Location = new Point(0, 41);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Size = new Size(648, 420);
      this.pnlMiddle.TabIndex = 4;
      this.splitter1.Location = new Point(323, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 420);
      this.splitter1.TabIndex = 3;
      this.splitter1.TabStop = false;
      this.splitter5.Dock = DockStyle.Bottom;
      this.splitter5.Location = new Point(0, 461);
      this.splitter5.Name = "splitter5";
      this.splitter5.Size = new Size(648, 3);
      this.splitter5.TabIndex = 5;
      this.splitter5.TabStop = false;
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Padding = new Padding(0, 10, 0, 0);
      this.pnlTop.Size = new Size(648, 41);
      this.pnlTop.TabIndex = 6;
      this.splitter2.Dock = DockStyle.Top;
      this.splitter2.Location = new Point(0, 219);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(323, 3);
      this.splitter2.TabIndex = 3;
      this.splitter2.TabStop = false;
      this.splitter3.Dock = DockStyle.Top;
      this.splitter3.Location = new Point(0, 219);
      this.splitter3.Name = "splitter3";
      this.splitter3.Size = new Size(322, 3);
      this.splitter3.TabIndex = 5;
      this.splitter3.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(648, 589);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.pnlTop);
      this.Controls.Add((Control) this.splitter5);
      this.Controls.Add((Control) this.pnlBottom);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (eFolderPage);
      this.Text = nameof (eFolderPage);
      this.SizeChanged += new EventHandler(this.eFolderPage_SizeChanged);
      this.pnlLeft.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlBottom.ResumeLayout(false);
      this.pnlMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
