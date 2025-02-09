// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormGroupPanel
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintFormGroupPanel : UserControl
  {
    private const string PROFILEID = "PrintFormGroups";
    private Sessions.Session session;
    private SetUpContainer setupContainer;
    private IContainer components;
    private FSExplorer formifsExplorer;

    public PrintFormGroupPanel(Sessions.Session session, SetUpContainer setupContainer)
      : this(session, setupContainer, false)
    {
    }

    public PrintFormGroupPanel(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool showPublicOnly)
    {
      this.session = session;
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.initForm(showPublicOnly);
    }

    private void initForm(bool showPublicOnly)
    {
      this.formifsExplorer.FileType = FSExplorer.FileTypes.PrintGroups;
      this.formifsExplorer.HasPublicRight = this.getPublicRight(FSExplorer.FileTypes.PrintGroups);
      this.formifsExplorer.SetProperties(false, false, false, 22, false);
      FSExplorer formifsExplorer = this.formifsExplorer;
      Size renameButtonSize = this.formifsExplorer.RenameButtonSize;
      int width = renameButtonSize.Width;
      renameButtonSize = this.formifsExplorer.RenameButtonSize;
      int height = renameButtonSize.Height - 2;
      Size size = new Size(width, height);
      formifsExplorer.RenameButtonSize = size;
      PrintFormGroupIFSExplorer ifsExplorer = new PrintFormGroupIFSExplorer(this.session);
      FileSystemEntry publicRoot;
      try
      {
        publicRoot = FileSystemEntry.Parse(this.session.GetPrivateProfileString("PrintFormGroups", "LastFolderViewed") ?? "");
        if (!ifsExplorer.EntryExists(publicRoot))
          publicRoot = FileSystemEntry.PublicRoot;
      }
      catch (Exception ex)
      {
        publicRoot = FileSystemEntry.PublicRoot;
      }
      if (!showPublicOnly)
        showPublicOnly = !this.getPrivateRight(FSExplorer.FileTypes.PrintGroups);
      if (showPublicOnly && !publicRoot.IsPublic)
        publicRoot = FileSystemEntry.PublicRoot;
      this.formifsExplorer.Init((IFSExplorerBase) ifsExplorer, publicRoot, showPublicOnly);
    }

    private bool getPrivateRight(FSExplorer.FileTypes type)
    {
      AclFeature feature = AclFeature.SettingsTab_Personal_CustomPrintForms;
      switch (type)
      {
        case FSExplorer.FileTypes.PrintGroups:
          feature = AclFeature.SettingsTab_Personal_PrintGroups;
          break;
        case FSExplorer.FileTypes.CustomForms:
          feature = AclFeature.SettingsTab_Personal_CustomPrintForms;
          break;
      }
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(feature);
    }

    private bool getPublicRight(FSExplorer.FileTypes type)
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return true;
      AclFileType fileType = AclFileType.PrintGroups;
      switch (type)
      {
        case FSExplorer.FileTypes.PrintGroups:
          fileType = AclFileType.PrintGroups;
          break;
        case FSExplorer.FileTypes.CustomForms:
          fileType = AclFileType.CustomPrintForms;
          break;
      }
      return this.session.AclGroupManager.CheckPublicAccessPermission(fileType);
    }

    private void PrintFormGroupPanel_Leave(object sender, EventArgs e)
    {
      try
      {
        this.session.WritePrivateProfileString("PrintFormGroups", "LastFolderViewed", this.formifsExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    public FileSystemEntry SelectedFolder
    {
      get => this.formifsExplorer.CurrentFolder;
      set => this.formifsExplorer.SetFolder(value);
    }

    public void SetSelectedItems(string[] fileTypePathList)
    {
      this.formifsExplorer.MakeItemSelected(AclFileType.PrintGroups, fileTypePathList);
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.formifsExplorer.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.formifsExplorer = new FSExplorer(this.session);
      this.SuspendLayout();
      this.formifsExplorer.Dock = DockStyle.Fill;
      this.formifsExplorer.FolderComboSelectedIndex = -1;
      this.formifsExplorer.HasPublicRight = true;
      this.formifsExplorer.Location = new Point(0, 0);
      this.formifsExplorer.Name = "formifsExplorer";
      this.formifsExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.formifsExplorer.Size = new Size(679, 448);
      this.formifsExplorer.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.formifsExplorer);
      this.Name = nameof (PrintFormGroupPanel);
      this.Size = new Size(679, 448);
      this.Leave += new EventHandler(this.PrintFormGroupPanel_Leave);
      this.ResumeLayout(false);
    }
  }
}
