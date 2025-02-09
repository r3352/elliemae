// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.CustomLetterPanel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class CustomLetterPanel : UserControl
  {
    private const string className = "CustomLetterPanel";
    private static readonly string sw = Tracing.SwCustomLetters;
    private System.ComponentModel.Container components;
    private FSExplorer fsExplorer1;
    private string localCustomLetterDir;
    private IMainScreen mainScreen;
    private Sessions.Session session;
    private CustomLetterIFSExplorer customLetterIFSExplorer;

    public CustomLetterPanel(Sessions.Session session, bool showPublicOnly)
      : this(session, (IMainScreen) null, showPublicOnly)
    {
    }

    public CustomLetterPanel(Sessions.Session session, IMainScreen mainScreen)
      : this(session, mainScreen, false)
    {
    }

    public CustomLetterPanel(Sessions.Session session, IMainScreen mainScreen, bool showPublicOnly)
    {
      this.session = session;
      this.mainScreen = mainScreen;
      this.localCustomLetterDir = SystemSettings.LocalCustomLetterDir;
      this.InitializeComponent();
      this.customLetterIFSExplorer = new CustomLetterIFSExplorer(this.session, this.mainScreen);
      this.customLetterIFSExplorer.CustomFormDetailChanged += new EventHandler(this.fsExplorer1_EntryChanged);
      this.fsExplorer1.HasPublicRight = UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CustomPrintForms);
      FileSystemEntry publicRoot1 = FileSystemEntry.PublicRoot;
      FileSystemEntry publicRoot2;
      try
      {
        publicRoot2 = FileSystemEntry.Parse(this.session.GetPrivateProfileString(nameof (CustomLetterPanel), "LastFolderViewed") ?? "");
        if (!this.customLetterIFSExplorer.EntryExists(publicRoot2))
          publicRoot2 = FileSystemEntry.PublicRoot;
      }
      catch
      {
        publicRoot2 = FileSystemEntry.PublicRoot;
      }
      this.fsExplorer1.FileType = FSExplorer.FileTypes.CustomForms;
      this.fsExplorer1.SetupForCustomForm();
      this.fsExplorer1.SetProperties(false, false, false, false);
      if (!showPublicOnly)
        showPublicOnly = !((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_CustomPrintForms);
      if (showPublicOnly && !publicRoot2.IsPublic)
        publicRoot2 = FileSystemEntry.PublicRoot;
      this.fsExplorer1.FolderChanged += new EventHandler(this.fsExplorer1_EntryChanged);
      this.fsExplorer1.AfterFileCopied += new EventHandler(this.fsExplorer1_AfterFileCopied);
      this.fsExplorer1.AfterFileRenamed += new EventHandler(this.fsExplorer1_AfterFileRenamed);
      this.fsExplorer1.AfterFileMoved += new EventHandler(this.fsExplorer1_AfterFileRenamed);
      this.fsExplorer1.AfterFileDeleted += new EventHandler(this.fsExplorer1_AfterFileDeleted);
      this.fsExplorer1.Init((IFSExplorerBase) this.customLetterIFSExplorer, publicRoot2, showPublicOnly);
    }

    private void fsExplorer1_EntryChanged(object sender, EventArgs e)
    {
      CustomFormDetail[] customFormDetails = this.session.ConfigurationManager.GetCustomFormDetails();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.fsExplorer1.GVItems)
      {
        gvItem.SubItems[1].Text = "";
        FileSystemEntry tag = (FileSystemEntry) gvItem.Tag;
        if (tag.Type == FileSystemEntry.Types.File)
        {
          gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) ForBorrowerType.All);
          foreach (CustomFormDetail customFormDetail in customFormDetails)
          {
            if (customFormDetail.Source == tag.ToString())
              gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) customFormDetail.IntendedFor);
          }
        }
      }
    }

    private void fsExplorer1_AfterFileDeleted(object sender, EventArgs e)
    {
      if (sender == null || !(sender is FileSystemEntry))
        return;
      FileSystemEntry fileSystemEntry = (FileSystemEntry) sender;
      if (fileSystemEntry.Type != FileSystemEntry.Types.File)
        return;
      this.session.ConfigurationManager.DeleteCustomFormDetail(fileSystemEntry.ToString());
    }

    private void fsExplorer1_AfterFileRenamed(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) sender;
      if (fileSystemEntryArray[0].Type != FileSystemEntry.Types.File)
        return;
      this.session.ConfigurationManager.RenameCustomFormDetailSource(fileSystemEntryArray[0].ToString(), fileSystemEntryArray[1].ToString());
    }

    private void fsExplorer1_AfterFileCopied(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) sender;
      if (fileSystemEntryArray[0].Type != FileSystemEntry.Types.File || fileSystemEntryArray[1].Type != FileSystemEntry.Types.File)
        return;
      FileSystemEntry fileSystemEntry1 = fileSystemEntryArray[0];
      FileSystemEntry fileSystemEntry2 = fileSystemEntryArray[1];
      this.session.ConfigurationManager.CopyCustomFormDetail(fileSystemEntry1.ToString(), fileSystemEntry2.ToString());
      this.fsExplorer1_EntryChanged(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.fsExplorer1 = new FSExplorer(this.session);
      this.SuspendLayout();
      this.fsExplorer1.Dock = DockStyle.Fill;
      this.fsExplorer1.HasPublicRight = true;
      this.fsExplorer1.Location = new Point(0, 0);
      this.fsExplorer1.Name = "fsExplorer1";
      this.fsExplorer1.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsExplorer1.Size = new Size(693, 452);
      this.fsExplorer1.TabIndex = 8;
      this.Controls.Add((Control) this.fsExplorer1);
      this.Name = nameof (CustomLetterPanel);
      this.Size = new Size(693, 452);
      this.Leave += new EventHandler(this.CustomLetterPanel_Leave);
      this.ResumeLayout(false);
    }

    private void CustomLetterPanel_Leave(object sender, EventArgs e)
    {
      try
      {
        this.session.WritePrivateProfileString(nameof (CustomLetterPanel), "LastFolderViewed", this.fsExplorer1.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    public FileSystemEntry SelectedFolder
    {
      get => this.fsExplorer1.CurrentFolder;
      set => this.fsExplorer1.SetFolder(value);
    }

    public void SetSelectedItems(string[] fileTypePathList)
    {
      this.fsExplorer1.MakeItemSelected(AclFileType.PrintGroups, fileTypePathList);
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.fsExplorer1.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
      }
    }
  }
}
