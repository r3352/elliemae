// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplatePanelExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemplatePanelExplorer : UserControl
  {
    private FSExplorer tempExplorer;
    private System.ComponentModel.Container components;
    private Sessions.Session session;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType tpType;

    public string[] SelectedTemplatePaths
    {
      get
      {
        if (this.SelectedFileSystemEntries.Count == 0)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (FileSystemEntry selectedFileSystemEntry in this.SelectedFileSystemEntries)
          stringList.Add(selectedFileSystemEntry.ToString());
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        foreach (GVItem gvItem in this.tempExplorer.GVItems.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(((FileSystemEntry) item.Tag).ToString()))))
          gvItem.Selected = true;
      }
    }

    public TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType tpType, Sessions.Session session)
      : this(tpType, session, false)
    {
    }

    public TemplatePanelExplorer(
      EllieMae.EMLite.ClientServer.TemplateSettingsType tpType,
      Sessions.Session session,
      bool showPublicOnly)
    {
      this.tpType = tpType;
      this.session = session;
      this.InitializeComponent();
      bool menuItemImportVisible = false;
      string section = "";
      switch (tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          section = "LoanProgramTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          section = "ClosingCostTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          section = "DataTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          section = "FormListTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          section = "DocumentSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          section = "LoanTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          section = "TaskSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          section = "SettlementServiceTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          section = "AffiliateTemplate";
          break;
      }
      string uri = this.session.GetPrivateProfileString(section, "LastFolderViewed") ?? "";
      TemplateIFSExplorer ifsExplorer = new TemplateIFSExplorer(this.session, tpType);
      switch (tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          this.tempExplorer.FileType = FSExplorer.FileTypes.LoanPrograms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          this.tempExplorer.FileType = FSExplorer.FileTypes.ClosingCosts;
          this.tempExplorer.AddNewHUDColumn();
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          this.tempExplorer.FileType = FSExplorer.FileTypes.DataTemplates;
          this.tempExplorer.AddNewHUDColumn();
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          this.tempExplorer.FileType = FSExplorer.FileTypes.FormLists;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          this.tempExplorer.FileType = FSExplorer.FileTypes.DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          this.tempExplorer.FileType = FSExplorer.FileTypes.LoanTemplates;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          this.tempExplorer.FileType = FSExplorer.FileTypes.TaskSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          this.tempExplorer.FileType = FSExplorer.FileTypes.SettlementServiceProviders;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          this.tempExplorer.FileType = FSExplorer.FileTypes.AffiliatedBusinessArrangements;
          break;
      }
      FileSystemEntry publicRoot1 = FileSystemEntry.PublicRoot;
      this.tempExplorer.SetProperties(false, menuItemImportVisible, false, (int) tpType, true);
      FileSystemEntry publicRoot2;
      try
      {
        publicRoot2 = FileSystemEntry.Parse(uri);
        if (!ifsExplorer.EntryExists(publicRoot2))
          publicRoot2 = FileSystemEntry.PublicRoot;
      }
      catch
      {
        publicRoot2 = FileSystemEntry.PublicRoot;
      }
      if (!showPublicOnly && (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.checkPersonalRight()))
        showPublicOnly = false;
      if (showPublicOnly && !publicRoot2.IsPublic)
        publicRoot2 = FileSystemEntry.PublicRoot;
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, publicRoot2, showPublicOnly);
    }

    public FileSystemEntry SelectedFolder
    {
      get => this.tempExplorer.CurrentFolder;
      set => this.tempExplorer.SetFolder(value);
    }

    public void SetSelectedItems(AclFileType type, string[] fileTypePathList)
    {
      this.tempExplorer.MakeItemSelected(type, fileTypePathList);
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.tempExplorer.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
      }
    }

    private bool checkPersonalRight()
    {
      if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        return true;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      AclFeature feature = AclFeature.SettingsTab_Personal_LoanPrograms;
      switch (this.tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          feature = AclFeature.SettingsTab_Personal_LoanPrograms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          feature = AclFeature.SettingsTab_Personal_ClosingCosts;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          feature = AclFeature.SettingsTab_Personal_MiscDataTemplates;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          feature = AclFeature.SettingsTab_Personal_InputFormSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          feature = AclFeature.SettingsTab_Personal_DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          feature = AclFeature.SettingsTab_Personal_LoanTemplateSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          feature = AclFeature.SettingsTab_Personal_TaskSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          feature = AclFeature.SettingsTab_Personal_SettlementServiceProvider;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          feature = AclFeature.SettingsTab_Personal_Affiliate;
          break;
      }
      return aclManager.GetUserApplicationRight(feature);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tempExplorer = new FSExplorer(this.session);
      this.SuspendLayout();
      this.tempExplorer.Dock = DockStyle.Fill;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(0, 0);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(584, 448);
      this.tempExplorer.TabIndex = 0;
      this.Controls.Add((Control) this.tempExplorer);
      this.Name = nameof (TemplatePanelExplorer);
      this.Size = new Size(584, 448);
      this.Leave += new EventHandler(this.TemplatePanelExplorer_Leave);
      this.ResumeLayout(false);
    }

    private void TemplatePanelExplorer_Leave(object sender, EventArgs e)
    {
      string section = "";
      switch (this.tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          section = "LoanProgramTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          section = "ClosingCostTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          section = "DataTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          section = "FormListTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          section = "DocumentSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          section = "LoanTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          section = "TaskSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          section = "SettlementServiceTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          section = "AffiliateTemplate";
          break;
      }
      try
      {
        this.session.WritePrivateProfileString(section, "LastFolderViewed", this.tempExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }
  }
}
