// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplateIFSExplorer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemplateIFSExplorer : IFSExplorerBase
  {
    private static readonly string sw = Tracing.SwInputEngine;
    private const string className = "TemplateIFSExplorer";
    private EllieMae.EMLite.ClientServer.TemplateSettingsType tpType;
    private bool configureTemplate = true;
    private Sessions.Session session;
    private TemplateIFSExplorer.RootType currRoot = TemplateIFSExplorer.RootType.PublicOnly;
    private FileSystemEntry[] entries;
    private TemplateIFSExplorer.FileTypes fileType;
    private FileSystemEntry _currentFolder = FileSystemEntry.PublicRoot;
    private FileSystemEntry[] currFSEntries = new FileSystemEntry[0];
    private FSExplorer.RESPAFilter respaMode;

    public event EventHandler AdditionalFieldButtonClicked;

    public event EventHandler DefaultLoanTemplateChanged;

    public event EventHandler SelectedCurrentFile;

    private FileSystemEntry currentFolder
    {
      get => this._currentFolder;
      set => this._currentFolder = value;
    }

    public TemplateIFSExplorer(
      Sessions.Session session,
      EllieMae.EMLite.ClientServer.TemplateSettingsType tpType,
      bool configureTemplate)
    {
      this.session = session;
      this.tpType = tpType;
      this.configureTemplate = configureTemplate;
    }

    public TemplateIFSExplorer(Sessions.Session session, EllieMae.EMLite.ClientServer.TemplateSettingsType tpType)
    {
      this.session = session;
      this.tpType = tpType;
    }

    private bool hasSecurityPublicRights()
    {
      return this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.LoanProgram);
    }

    public TemplateIFSExplorer.FileTypes FileType
    {
      set => this.fileType = value;
    }

    public FileSystemEntry[] CurrentFileSystemEntries => this.currFSEntries;

    public void Init(FileSystemEntry defaultFolder, bool publicOnly)
    {
      this.init(defaultFolder, publicOnly);
    }

    private void init(FileSystemEntry defaultFolder, bool publicOnly)
    {
      this.currRoot = defaultFolder != null || publicOnly ? (!publicOnly ? (defaultFolder.Owner == null ? TemplateIFSExplorer.RootType.Public : TemplateIFSExplorer.RootType.Private) : TemplateIFSExplorer.RootType.PublicOnly) : TemplateIFSExplorer.RootType.Top;
      this.changeFolder(this.normalizeFolder(defaultFolder.Path, this.currRoot), this.currRoot);
    }

    private string normalizeFolder(string folder, TemplateIFSExplorer.RootType rootType)
    {
      if (folder == null || folder.Length == 0)
        return "\\";
      while (folder.IndexOf("\\\\") >= 0)
        folder = folder.Replace("\\\\", "\\");
      if (folder.EndsWith("\\"))
        folder = folder.Substring(0, folder.Length - 1);
      if (!folder.StartsWith("\\"))
        folder = "\\" + folder;
      return folder;
    }

    private void changeFolder(string folder, TemplateIFSExplorer.RootType rootType)
    {
      string empty = string.Empty;
      if (rootType == TemplateIFSExplorer.RootType.Top)
      {
        this.entries = new FileSystemEntry[2];
        this.entries[0] = new FileSystemEntry("\\Personal\\", FileSystemEntry.Types.Folder, this.session.UserID);
        this.entries[1] = new FileSystemEntry("\\Public\\", FileSystemEntry.Types.Folder, (string) null);
        this.currentFolder = FileSystemEntry.PublicRoot;
      }
      else
      {
        FileSystemEntry fileSystemEntry = new FileSystemEntry(folder, FileSystemEntry.Types.Folder, this.isRootTypePublic(rootType) ? (string) null : this.session.UserID);
        this.entries = this.currFSEntries;
        if (fileSystemEntry.IsPublic && (this.entries == null || this.entries.Length == 0 || this.currentFolder.Path == fileSystemEntry.Path || this.currentFolder.ParentFolder != null && this.currentFolder.ParentFolder.Path == fileSystemEntry.Path || !this.currentFolder.IsPublic) && fileSystemEntry.ParentFolder != null)
          this.entries = this.GetFileSystemEntries(fileSystemEntry.ParentFolder);
        if (this.entries != null && fileSystemEntry.IsPublic)
        {
          foreach (FileSystemEntry entry in this.entries)
          {
            if (fileSystemEntry.Path == entry.Path)
            {
              fileSystemEntry = entry;
              break;
            }
          }
        }
        if (fileSystemEntry.Path == "\\" && fileSystemEntry.IsPublic)
        {
          fileSystemEntry = this.setFakeFolderProperty(fileSystemEntry);
          if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
            fileSystemEntry.Access = AclResourceAccess.ReadWrite;
        }
        this.entries = this.GetFileSystemEntries(fileSystemEntry);
        if (this.entries == null)
          return;
        this.currentFolder = fileSystemEntry;
      }
      if (this.currRoot != TemplateIFSExplorer.RootType.PublicOnly)
        this.currRoot = rootType;
      if (this.entries == null)
        this.currFSEntries = new FileSystemEntry[0];
      else
        this.currFSEntries = this.entries;
    }

    private bool isRootTypePublic(TemplateIFSExplorer.RootType type)
    {
      return type != TemplateIFSExplorer.RootType.Private;
    }

    private bool isRootPublic => this.isRootTypePublic(this.currRoot);

    private FileSystemEntry setFakeFolderProperty(FileSystemEntry newFolder)
    {
      AclFileType fileType = AclFileType.LoanProgram;
      string uri = "";
      switch (this.fileType)
      {
        case TemplateIFSExplorer.FileTypes.PrintGroups:
          fileType = AclFileType.PrintGroups;
          uri = "Public:\\Public Forms Groups\\";
          break;
        case TemplateIFSExplorer.FileTypes.CustomForms:
          fileType = AclFileType.CustomPrintForms;
          uri = "Public:\\Public Custom Forms\\";
          break;
        case TemplateIFSExplorer.FileTypes.Reports:
          fileType = AclFileType.Reports;
          uri = "Public:\\Public Reports\\";
          break;
        case TemplateIFSExplorer.FileTypes.LoanTemplates:
          fileType = AclFileType.LoanTemplate;
          uri = "Public:\\Public Loan Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.DataTemplates:
          fileType = AclFileType.MiscData;
          uri = "Public:\\Public Data Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.LoanPrograms:
          fileType = AclFileType.LoanProgram;
          uri = "Public:\\Public Loan Programs\\";
          break;
        case TemplateIFSExplorer.FileTypes.ClosingCosts:
          fileType = AclFileType.ClosingCost;
          uri = "Public:\\Public Closing Cost Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.DocumentSets:
          fileType = AclFileType.DocumentSet;
          uri = "Public:\\Public Document Sets\\";
          break;
        case TemplateIFSExplorer.FileTypes.FormLists:
          fileType = AclFileType.FormList;
          uri = "Public:\\Public Form Lists\\";
          break;
        case TemplateIFSExplorer.FileTypes.CampaignTemplates:
          fileType = AclFileType.CampaignTemplate;
          uri = "Public:\\Public Campaign Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.DashboardTemplate:
          fileType = AclFileType.DashboardTemplate;
          uri = "Public:\\Public Dashboard Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.DashboardViewTemplate:
          fileType = AclFileType.DashboardViewTemplate;
          uri = "Public:\\Public DashboardView Templates\\";
          break;
        case TemplateIFSExplorer.FileTypes.TaskSets:
          fileType = AclFileType.TaskSet;
          uri = "Public:\\Public Task Sets\\";
          break;
        case TemplateIFSExplorer.FileTypes.SettlementServiceProviders:
          fileType = AclFileType.SettlementServiceProviders;
          uri = "Public:\\Public Settlement Service Providers\\";
          break;
        case TemplateIFSExplorer.FileTypes.AffiliatedBusinessArrangements:
          fileType = AclFileType.AffiliatedBusinessArrangements;
          uri = "Public:\\Public Affiliates\\";
          break;
      }
      if (uri == "")
        return newFolder;
      AclResourceAccess fileFolderAccess = this.session.AclGroupManager.GetUserFileFolderAccess(fileType, FileSystemEntry.Parse(uri));
      if (fileFolderAccess == AclResourceAccess.ReadWrite)
        newFolder.Access = fileFolderAccess;
      return newFolder;
    }

    private AclFileType GetAclFileType()
    {
      AclFileType aclFileType = AclFileType.LoanProgram;
      switch (this.fileType)
      {
        case TemplateIFSExplorer.FileTypes.PrintGroups:
          aclFileType = AclFileType.PrintGroups;
          break;
        case TemplateIFSExplorer.FileTypes.CustomForms:
          aclFileType = AclFileType.CustomPrintForms;
          break;
        case TemplateIFSExplorer.FileTypes.Reports:
          aclFileType = AclFileType.Reports;
          break;
        case TemplateIFSExplorer.FileTypes.LoanTemplates:
          aclFileType = AclFileType.LoanTemplate;
          break;
        case TemplateIFSExplorer.FileTypes.DataTemplates:
          aclFileType = AclFileType.MiscData;
          break;
        case TemplateIFSExplorer.FileTypes.LoanPrograms:
          aclFileType = AclFileType.LoanProgram;
          break;
        case TemplateIFSExplorer.FileTypes.ClosingCosts:
          aclFileType = AclFileType.ClosingCost;
          break;
        case TemplateIFSExplorer.FileTypes.DocumentSets:
          aclFileType = AclFileType.DocumentSet;
          break;
        case TemplateIFSExplorer.FileTypes.FormLists:
          aclFileType = AclFileType.FormList;
          break;
        case TemplateIFSExplorer.FileTypes.CampaignTemplates:
          aclFileType = AclFileType.CampaignTemplate;
          break;
        case TemplateIFSExplorer.FileTypes.DashboardTemplate:
          aclFileType = AclFileType.DashboardTemplate;
          break;
        case TemplateIFSExplorer.FileTypes.DashboardViewTemplate:
          aclFileType = AclFileType.DashboardViewTemplate;
          break;
        case TemplateIFSExplorer.FileTypes.TaskSets:
          aclFileType = AclFileType.TaskSet;
          break;
        case TemplateIFSExplorer.FileTypes.SettlementServiceProviders:
          aclFileType = AclFileType.SettlementServiceProviders;
          break;
        case TemplateIFSExplorer.FileTypes.AffiliatedBusinessArrangements:
          aclFileType = AclFileType.SettlementServiceProviders;
          break;
      }
      return aclFileType;
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentEntry)
    {
      try
      {
        FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetFilteredTemplateDirEntries(this.tpType, parentEntry);
        if (this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice || this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate)
        {
          foreach (FileSystemEntry fileSystemEntry in templateDirEntries)
            fileSystemEntry.Access = AclResourceAccess.ReadWrite;
        }
        return templateDirEntries;
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
    }

    public string GetDisplayName(FileSystemEntry entry, bool displayExtension)
    {
      return entry.Type == FileSystemEntry.Types.Folder || displayExtension ? entry.Name : this.removeFileExtension(entry.Name);
    }

    private string removeFileExtension(string filename)
    {
      int length = filename.LastIndexOf('.');
      return length > 0 ? filename.Substring(0, length) : filename;
    }

    public int GetFileImageIcon()
    {
      return TemplateIFSExplorer.FileTypes.PrintGroups == this.fileType ? (TemplateIFSExplorer.RootType.Public == this.currRoot ? 3 : 4) : (TemplateIFSExplorer.FileTypes.Reports == this.fileType ? 5 : 1);
    }

    public override void CreateFolder(FileSystemEntry entry)
    {
      this.session.ConfigurationManager.CreateTemplateSettingsFolder(this.tpType, entry);
    }

    public override void DeleteEntry(FileSystemEntry entry)
    {
      if (this.currRoot == TemplateIFSExplorer.RootType.Top)
        return;
      this.session.ConfigurationManager.DeleteTemplateSettingsObject(this.tpType, entry);
      this.currFSEntries = this.GetFileSystemEntries(this.currentFolder);
      if (this.tpType != EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate && this.tpType != EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder)
        return;
      this.onDefaultTemplateChanged();
    }

    public FileSystemEntry Rename(FileSystemEntry source, string oldName, string newName)
    {
      string oldName1 = "Public:" + source.Path;
      string newName1 = oldName.IndexOf(".") <= 0 ? this.ConstructFilePath(newName, source.Type, source.ParentFolder.Path, "") : this.ConstructFilePath(newName, source.Type, source.ParentFolder.Path, oldName.Substring(oldName.IndexOf(".")));
      if (string.Compare(oldName, newName, false) == 0)
        return (FileSystemEntry) null;
      FileSystemEntry targetEntry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, newName), source.Type, source.Owner);
      targetEntry.Access = source.Access;
      try
      {
        this.MoveEntry(source, targetEntry);
      }
      catch (Exception ex)
      {
        Tracing.Log(TemplateIFSExplorer.sw, nameof (TemplateIFSExplorer), TraceLevel.Error, ex.ToString());
        return (FileSystemEntry) null;
      }
      targetEntry.Properties = source.Properties;
      ArrayList arrayList = new ArrayList((ICollection) this.currFSEntries);
      arrayList.Remove((object) source);
      arrayList.Add((object) targetEntry);
      this.currFSEntries = (FileSystemEntry[]) arrayList.ToArray(typeof (FileSystemEntry));
      if (this.currentFolder.IsPublic)
      {
        if (source.Type == FileSystemEntry.Types.Folder)
          this.session.AclGroupManager.UpdateFileResource(oldName1, newName1, (int) this.GetAclFileType(), true);
        else
          this.session.AclGroupManager.UpdateFileResource(oldName1, newName1, (int) this.GetAclFileType(), false);
      }
      return targetEntry;
    }

    private string ConstructFilePath(
      string name,
      FileSystemEntry.Types type,
      string path,
      string extension)
    {
      string str1 = "Public:" + path;
      string str2;
      if (type == FileSystemEntry.Types.Folder)
      {
        str2 = str1 + name + "\\";
      }
      else
      {
        str2 = str1 + name;
        if (extension != "")
          str2 += extension;
      }
      return str2;
    }

    public override void MoveEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic || !target.IsPublic || !this.containsXRefs())
        this.session.ConfigurationManager.MoveTemplateSettingsObject(this.tpType, source, target);
      else if (source.Type == FileSystemEntry.Types.Folder)
        this.moveFolder(source, target);
      else
        this.moveFile(source, target);
      if (this.tpType != EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate && this.tpType != EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder)
        return;
      this.onDefaultTemplateChanged();
    }

    private bool containsXRefs()
    {
      return this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram || this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate;
    }

    private bool moveFolder(FileSystemEntry source, FileSystemEntry target)
    {
      FileSystemEntry[] fileSystemEntries = this.GetFileSystemEntries(source);
      this.CreateFolder(target);
      bool flag = true;
      for (int index = 0; index < fileSystemEntries.Length; ++index)
        flag = fileSystemEntries[index].Type != FileSystemEntry.Types.File ? this.moveFolder(fileSystemEntries[index], new FileSystemEntry(target.Path, fileSystemEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner)) & flag : this.moveFile(fileSystemEntries[index], target) & flag;
      if (flag)
        this.DeleteEntry(source);
      return flag;
    }

    private bool moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (!this.checkForPersonalContents(source))
        return false;
      this.session.ConfigurationManager.MoveTemplateSettingsObject(this.tpType, source, target);
      return true;
    }

    public bool DuplicateEntry(FileSystemEntry source)
    {
      if (this.currRoot == TemplateIFSExplorer.RootType.Top || source == null)
        return false;
      FileSystemEntry currentFolder = this.currentFolder;
      FileSystemEntry fileSystemEntry = new FileSystemEntry(currentFolder.Path, source.Name, source.Type, currentFolder.Owner);
      if (source.Equals((object) fileSystemEntry))
      {
        FileSystemEntry entry = new FileSystemEntry(fileSystemEntry.ParentFolder.Path + "Copy of " + fileSystemEntry.Name, fileSystemEntry.Type, fileSystemEntry.Owner);
        int num = 2;
        while (this.EntryExistsOfAnyType(entry))
        {
          entry = new FileSystemEntry(fileSystemEntry.ParentFolder.Path + "Copy of (" + (object) num + ") " + fileSystemEntry.Name, fileSystemEntry.Type, fileSystemEntry.Owner);
          ++num;
        }
        fileSystemEntry = entry;
      }
      if (this.EntryExistsOfAnyType(fileSystemEntry) || source.Type == FileSystemEntry.Types.Folder && fileSystemEntry.ToString().ToLower().IndexOf(source.ToString().ToLower()) >= 0)
        return false;
      try
      {
        this.CopyEntry(source, fileSystemEntry);
      }
      catch (Exception ex)
      {
        Tracing.Log(TemplateIFSExplorer.sw, nameof (TemplateIFSExplorer), TraceLevel.Error, ex.ToString());
      }
      this.changeFolder(this.currentFolder.Path, this.currRoot);
      return true;
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      RespaVersions respaVersion = RespaVersions.Respa2015;
      if (this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate)
        respaVersion = RespaVersions.Respa2010;
      if (source.Type == FileSystemEntry.Types.File && this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost)
      {
        using (CreateNewTemplateDialog newTemplateDialog = new CreateNewTemplateDialog(false))
        {
          if (newTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          respaVersion = newTemplateDialog.Use2015 ? RespaVersions.Respa2015 : (newTemplateDialog.Use2010 ? RespaVersions.Respa2010 : RespaVersions.Respa2009);
        }
      }
      if (source.IsPublic || !target.IsPublic || !this.containsXRefs())
        this.session.ConfigurationManager.CopyTemplateSettingsObject(this.tpType, source, target, respaVersion);
      else if (source.Type == FileSystemEntry.Types.Folder)
        this.copyFolder(source, target, respaVersion);
      else
        this.copyFile(source, target, respaVersion);
    }

    private void copyFolder(
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion)
    {
      FileSystemEntry[] fileSystemEntries = this.GetFileSystemEntries(source);
      this.CreateFolder(target);
      for (int index = 0; index < fileSystemEntries.Length; ++index)
      {
        if (fileSystemEntries[index].Type == FileSystemEntry.Types.File)
          this.copyFile(fileSystemEntries[index], target, respaVersion);
        else
          this.copyFolder(fileSystemEntries[index], new FileSystemEntry(target.Path, fileSystemEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner), respaVersion);
      }
    }

    public bool copyFile(
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion)
    {
      if (!this.checkForPersonalContents(source))
        return false;
      this.session.ConfigurationManager.CopyTemplateSettingsObject(this.tpType, source, target, respaVersion);
      return true;
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExists(this.tpType, entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(this.tpType, entry);
    }

    public override void OpenFile(FileSystemEntry entry, GVItem lvItem)
    {
      if (!this.EntryExists(entry))
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.configureTemplate && this.SelectedCurrentFile != null)
      {
        this.SelectedCurrentFile((object) null, (EventArgs) null);
      }
      else
      {
        string str1 = (string) null;
        switch (this.tpType)
        {
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
            str1 = this.editLoanProgram(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
            str1 = this.editClosingCost(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
            str1 = this.editDataTemplate(entry, lvItem);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
            str1 = this.editFormList(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
            str1 = this.editDocumentSet(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
            str1 = this.editLoanTemplate(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder:
            str1 = this.editStackingOrderSet(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice:
            str1 = this.editPurchaseAdviceSet(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate:
            str1 = this.editFundingTemplate(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
            str1 = this.editTaskSet(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.ConditionalLetter:
            str1 = this.editConditionalLetter(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
            str1 = this.editSettlementServiceProviders(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanDuplicationTemplate:
            str1 = this.editLoanDuplicationTemplate(entry);
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
            str1 = this.editAffiliatedBusinessArrangements(entry);
            break;
        }
        if (str1 == null)
          return;
        entry.Properties[(object) "Description"] = (object) str1;
        string str2 = str1.Replace("\r\n", "  ");
        if (lvItem.SubItems.Count > 2)
          lvItem.SubItems[2].Text = str2;
        else
          lvItem.SubItems[1].Text = str2;
      }
    }

    public FileSystemEntry AddEntry(bool displayExtension)
    {
      if (this.currRoot == TemplateIFSExplorer.RootType.Top)
        return (FileSystemEntry) null;
      string newDocBaseName = this.NewDocBaseName;
      string newDocExtension = displayExtension ? "" : this.NewDocExtension;
      FileSystemEntry entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, newDocBaseName + newDocExtension), FileSystemEntry.Types.File, this.isRootPublic ? (string) null : this.session.UserID);
      int num = 2;
      while (this.EntryExistsOfAnyType(entry))
      {
        entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, newDocBaseName + " (" + (object) num + ")" + newDocExtension), FileSystemEntry.Types.File, this.isRootPublic ? (string) null : this.session.UserID);
        ++num;
      }
      if (!this.CreateNew(entry))
        return entry;
      this.currFSEntries = this.GetFileSystemEntries(this.currentFolder);
      foreach (FileSystemEntry currFsEntry in this.currFSEntries)
      {
        if (currFsEntry.Path == entry.Path)
        {
          entry = currFsEntry;
          break;
        }
      }
      return entry;
    }

    public override bool CreateNew(FileSystemEntry entry)
    {
      Cursor.Current = Cursors.WaitCursor;
      switch (this.tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          LoanProgram data1 = new LoanProgram();
          data1.TemplateName = entry.Name;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data1);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          ClosingCost data2 = (ClosingCost) null;
          using (CreateNewTemplateDialog newTemplateDialog = new CreateNewTemplateDialog(false))
          {
            if (newTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return false;
            data2 = new ClosingCost();
            if (newTemplateDialog.Use2015)
              data2.RESPAVersion = "2015";
            else if (newTemplateDialog.Use2010)
            {
              data2.RESPAVersion = "2010";
              data2.For2010GFE = true;
            }
            else
            {
              data2.RESPAVersion = "2009";
              data2.For2010GFE = false;
            }
          }
          data2.TemplateName = entry.Name;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data2);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          DataTemplate data3 = new DataTemplate();
          data3.TemplateName = entry.Name;
          data3.RESPAVersion = "";
          data3.SetField("PAYMENTTABLE.USEACTUALPAYMENTCHANGE", "N");
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data3);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          FormTemplate data4 = new FormTemplate();
          data4.AddForm("DTNAME", entry.Name);
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data4);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) new DocumentSetTemplate()
          {
            TemplateName = entry.Name
          });
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          LoanTemplate data5 = new LoanTemplate();
          data5.TemplateName = entry.Name;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data5);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder:
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) new StackingOrderSetTemplate()
          {
            TemplateName = entry.Name
          });
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice:
          PurchaseAdviceTemplate data6 = new PurchaseAdviceTemplate();
          data6.TemplateName = entry.Name;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data6);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate:
          FundingTemplate data7 = (FundingTemplate) null;
          if (this.respaMode == FSExplorer.RESPAFilter.All)
          {
            using (CreateNewTemplateDialog newTemplateDialog = new CreateNewTemplateDialog(true))
            {
              if (newTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return false;
              data7 = new FundingTemplate();
              data7.For2010GFE = newTemplateDialog.Use2010;
              data7.RESPAVersion = newTemplateDialog.Use2015 ? "2015" : (newTemplateDialog.Use2010 ? "2010" : "2009");
            }
          }
          else
          {
            data7 = new FundingTemplate();
            if (this.respaMode == FSExplorer.RESPAFilter.Respa2015)
            {
              data7.RESPAVersion = "2015";
              data7.For2010GFE = false;
            }
            else if (this.respaMode == FSExplorer.RESPAFilter.Respa2010)
            {
              data7.RESPAVersion = "2010";
              data7.For2010GFE = true;
            }
            else
            {
              data7.RESPAVersion = "2009";
              data7.For2010GFE = false;
            }
          }
          data7.TemplateName = entry.Name;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data7);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) new TaskSetTemplate()
          {
            TemplateName = entry.Name
          });
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ConditionalLetter:
          ConditionalLetterPrintOption data8 = new ConditionalLetterPrintOption();
          data8.SetCurrentField("DTNAME", entry.Name);
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data8);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          SettlementServiceTemplate data9 = new SettlementServiceTemplate();
          data9.SetCurrentField("DTNAME", entry.Name);
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data9);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanDuplicationTemplate:
          LoanDuplicationTemplate data10 = new LoanDuplicationTemplate();
          data10.SetCurrentField("DTNAME", entry.Name);
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data10);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate:
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) new DocumentExportTemplate()
          {
            TemplateName = entry.Name
          });
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          AffiliateTemplate data11 = new AffiliateTemplate();
          data11.SetCurrentField("DTNAME", entry.Name);
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) data11);
          break;
      }
      Cursor.Current = Cursors.Default;
      return true;
    }

    private string editLoanProgram(FileSystemEntry entry)
    {
      LoanProgram templateSettings = (LoanProgram) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
        return (string) null;
      string description1 = templateSettings.Description;
      Cursor.Current = Cursors.WaitCursor;
      using (LoanProgramDialog loanProgramDialog = new LoanProgramDialog(this.session, templateSettings, entry.IsPublic))
      {
        if (loanProgramDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      return (string) null;
    }

    private string editClosingCost(FileSystemEntry entry)
    {
      ClosingCost templateSettings = (ClosingCost) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      Cursor.Current = Cursors.WaitCursor;
      TemplateDialog templateDialog = !entry.IsPublic ? new TemplateDialog((FieldDataTemplate) templateSettings, this.tpType, entry.IsPublic, this.session) : (entry.Access != AclResourceAccess.ReadOnly ? new TemplateDialog((FieldDataTemplate) templateSettings, this.tpType, entry.IsPublic, this.session) : new TemplateDialog((FieldDataTemplate) templateSettings, this.tpType, entry.IsPublic, true, this.session));
      try
      {
        if (templateSettings.RESPAVersion == "2015")
          templateDialog.LoadForm("Closing Cost", "REGZGFE_2015");
        else if (templateSettings.For2010GFE || templateSettings.RESPAVersion == "2010")
          templateDialog.LoadForm("Closing Cost", "REGZGFE_2010");
        else
          templateDialog.LoadForm("Closing Cost", "CCOSTPROG");
        if (templateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        templateDialog.Dispose();
      }
      return (string) null;
    }

    private string editDocumentSet(FileSystemEntry entry)
    {
      DocumentSetTemplate templateSettings = (DocumentSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      DocumentSetTemplateDialog setTemplateDialog = !entry.IsPublic ? new DocumentSetTemplateDialog(templateSettings, this.session) : (entry.Access != AclResourceAccess.ReadOnly ? new DocumentSetTemplateDialog(templateSettings, this.session) : new DocumentSetTemplateDialog(templateSettings, true, this.session));
      try
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        setTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editTaskSet(FileSystemEntry entry)
    {
      TaskSetTemplate templateSettings = (TaskSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      TaskSetTemplateDialog setTemplateDialog = !entry.IsPublic ? new TaskSetTemplateDialog(templateSettings, this.session) : (entry.Access != AclResourceAccess.ReadOnly ? new TaskSetTemplateDialog(templateSettings, this.session) : new TaskSetTemplateDialog(templateSettings, true, this.session));
      try
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        setTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editFormList(FileSystemEntry entry)
    {
      FormTemplate templateSettings = (FormTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      FormListDialog formListDialog = !entry.IsPublic ? new FormListDialog(templateSettings, this.session) : (entry.Access != AclResourceAccess.ReadOnly ? new FormListDialog(templateSettings, this.session) : new FormListDialog(templateSettings, true));
      try
      {
        if (formListDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        formListDialog.Dispose();
      }
      return (string) null;
    }

    private string editSettlementServiceProviders(FileSystemEntry entry)
    {
      SettlementServiceTemplate templateSettings = (SettlementServiceTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      SettlementServiceTemplateDialog serviceTemplateDialog = !entry.IsPublic ? new SettlementServiceTemplateDialog(templateSettings, entry.IsPublic, false, this.session) : new SettlementServiceTemplateDialog(templateSettings, entry.IsPublic, entry.Access == AclResourceAccess.ReadOnly, this.session);
      try
      {
        if (serviceTemplateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        serviceTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editAffiliatedBusinessArrangements(FileSystemEntry entry)
    {
      AffiliateTemplate templateSettings = (AffiliateTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      AffiliateTemplateDialog affiliateTemplateDialog = !entry.IsPublic ? new AffiliateTemplateDialog(templateSettings, entry.IsPublic, false, this.session) : new AffiliateTemplateDialog(templateSettings, entry.IsPublic, entry.Access == AclResourceAccess.ReadOnly, this.session);
      try
      {
        if (affiliateTemplateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        affiliateTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editLoanTemplate(FileSystemEntry entry)
    {
      LoanTemplate templateSettings = (LoanTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      LoanTemplateDialog loanTemplateDialog = !entry.IsPublic ? new LoanTemplateDialog(this.session, templateSettings, entry.IsPublic) : (entry.Access != AclResourceAccess.ReadOnly ? new LoanTemplateDialog(this.session, templateSettings, entry.IsPublic) : new LoanTemplateDialog(this.session, templateSettings, entry.IsPublic, true));
      try
      {
        if (loanTemplateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        loanTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editDataTemplate(FileSystemEntry entry, GVItem lvItem)
    {
      DataTemplate templateSettings = (DataTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      DataTemplateDialog dataTemplateDialog = !entry.IsPublic ? new DataTemplateDialog(this.session, templateSettings, entry.IsPublic) : (entry.Access != AclResourceAccess.ReadOnly ? new DataTemplateDialog(this.session, templateSettings, entry.IsPublic) : new DataTemplateDialog(this.session, templateSettings, true, entry.IsPublic));
      try
      {
        if (dataTemplateDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          entry.Properties[(object) "RESPAVERSION"] = (object) templateSettings.RESPAVersion;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          lvItem.SubItems[1].Text = this.GetRESPA(entry) == "2009" ? "Old" : this.GetRESPA(entry);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        dataTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editFundingTemplate(FileSystemEntry entry)
    {
      FundingTemplate templateSettings = (FundingTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      TemplateDialog templateDialog = new TemplateDialog((FieldDataTemplate) templateSettings, EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate, true, !aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_SecondarySetup), this.session);
      try
      {
        if (templateSettings.RESPAVersion == "2015")
          templateDialog.LoadForm("Funding Template", "FundingTemplate2015");
        else if (templateSettings.RESPAVersion == "2010" || templateSettings.For2010GFE)
          templateDialog.LoadForm("Funding Template", "FundingTemplate2010");
        else
          templateDialog.LoadForm("Funding Template", "FundingTemplateForm");
        if (templateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        templateDialog.Dispose();
      }
      return (string) null;
    }

    private string editPurchaseAdviceSet(FileSystemEntry entry)
    {
      PurchaseAdviceTemplate templateSettings = (PurchaseAdviceTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      templateSettings.TemplateName = entry.Name;
      string description1 = templateSettings.Description;
      using (PurchaseAdviceTemplateSetup adviceTemplateSetup = new PurchaseAdviceTemplateSetup(templateSettings, this.session))
      {
        try
        {
          if (adviceTemplateSetup.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
          {
            string description2 = templateSettings.Description;
            this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
            if (description2 != description1)
              return description2;
          }
        }
        finally
        {
          adviceTemplateSetup.Dispose();
        }
      }
      return (string) null;
    }

    private string editConditionalLetter(FileSystemEntry entry)
    {
      ConditionalLetterPrintOption templateSettings = (ConditionalLetterPrintOption) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      using (ConditionalLetterSetupForm conditionalLetterSetupForm = new ConditionalLetterSetupForm(this.session, templateSettings))
      {
        if (conditionalLetterSetupForm.ShowDialog((IWin32Window) null) == DialogResult.OK)
        {
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          return "";
        }
      }
      return (string) null;
    }

    private string editStackingOrderSet(FileSystemEntry entry)
    {
      StackingOrderSetTemplate templateSettings = (StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      string description1 = templateSettings.Description;
      StackingOrderSetTemplateDialog setTemplateDialog = new StackingOrderSetTemplateDialog(templateSettings);
      try
      {
        if (setTemplateDialog.ShowDialog() == DialogResult.OK)
        {
          string description2 = templateSettings.Description;
          this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          if (description2 != description1)
            return description2;
        }
      }
      finally
      {
        setTemplateDialog.Dispose();
      }
      return (string) null;
    }

    private string editLoanDuplicationTemplate(FileSystemEntry entry)
    {
      LoanDuplicationTemplate templateSettings = (LoanDuplicationTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.tpType, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + entry.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
      templateSettings.TemplateName = entry.Name;
      string description1 = templateSettings.Description;
      using (LoanDuplicationTemplateForm duplicationTemplateForm = new LoanDuplicationTemplateForm(this.session, templateSettings))
      {
        try
        {
          duplicationTemplateForm.AdditionalFieldButtonClicked += new EventHandler(this.setting_AdditionalFieldButtonClicked);
          if (duplicationTemplateForm.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
          {
            string description2 = templateSettings.Description;
            this.session.ConfigurationManager.SaveTemplateSettings(this.tpType, entry, (BinaryObject) (BinaryConvertibleObject) templateSettings);
            if (description2 != description1)
              return description2;
          }
        }
        finally
        {
          duplicationTemplateForm.Dispose();
        }
      }
      return (string) null;
    }

    private void setting_AdditionalFieldButtonClicked(object sender, EventArgs e)
    {
      if (this.AdditionalFieldButtonClicked == null)
        return;
      this.AdditionalFieldButtonClicked(sender, e);
    }

    public override string NewDocBaseName
    {
      get
      {
        switch (this.tpType)
        {
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
            return "New Loan Program";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
            return "New Closing Cost";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
            return "New Data Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
            return "New Form List";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
            return "New Document Set";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
            return "New Loan Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder:
            return "New Stacking Order Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice:
            return "New Purchase Advice Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate:
            return "New Funding Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.ConditionalLetter:
            return "New Condition Form";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanDuplicationTemplate:
            return "New Loan Duplication Template";
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate:
            return "New Document Export Template";
          default:
            return "New Template";
        }
      }
    }

    public override string GetDescription(FileSystemEntry entry)
    {
      string description = string.Concat(entry.Properties[(object) "Description"]);
      if (description != string.Empty)
        description = description.Replace("\r\n", "  ");
      return description;
    }

    public override string GetRESPA(FileSystemEntry entry)
    {
      if (this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData && entry.Properties.ContainsKey((object) "RESPAVERSION") && string.Concat(entry.Properties[(object) "RESPAVERSION"]) == "")
        return "";
      if (entry.Properties.ContainsKey((object) "RESPAVERSION") && string.Concat(entry.Properties[(object) "RESPAVERSION"]) != "")
        return string.Concat(entry.Properties[(object) "RESPAVERSION"]);
      return entry.Properties.ContainsKey((object) "For2010GFE") && entry.Properties[(object) "For2010GFE"].ToString() == "Yes" ? "2010" : "2009";
    }

    public override void Import(FileSystemEntry entry)
    {
      switch (this.tpType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          this.importLoanProgram(entry);
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          this.importClosingCost(entry);
          break;
      }
    }

    private void importLoanProgram(FileSystemEntry entry)
    {
      int num = (int) new ImportPointTemplates("LP").ShowDialog();
    }

    private void importClosingCost(FileSystemEntry entry)
    {
      int num = (int) new ImportPointTemplates("CC").ShowDialog();
    }

    private bool checkForPersonalContents(FileSystemEntry source)
    {
      FileSystemEntry[] templateSettingsXrefs = this.session.ConfigurationManager.GetTemplateSettingsXRefs(this.tpType, source);
      if (templateSettingsXrefs.Length == 0)
        return true;
      if (this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram)
        return this.checkForLPPersonalContents(source, templateSettingsXrefs);
      return this.tpType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate && this.checkForLTPersonalContents(source, templateSettingsXrefs);
    }

    private bool checkForLPPersonalContents(FileSystemEntry source, FileSystemEntry[] xrefs)
    {
      if (xrefs[0].IsPublic)
        return true;
      return Utils.Dialog((IWin32Window) Form.ActiveForm, "The Loan Program '" + source.Name + "' references the personal Closing Cost template '" + xrefs[0].Name + "'. Would you like to copy this Loan Program and have the Closing Cost information removed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private bool checkForLTPersonalContents(FileSystemEntry source, FileSystemEntry[] xrefs)
    {
      bool flag = false;
      for (int index = 0; index < xrefs.Length && !flag; ++index)
      {
        if (!xrefs[index].IsPublic)
          flag = true;
      }
      return !flag || Utils.Dialog((IWin32Window) Form.ActiveForm, "The Loan Template '" + source.Name + "' references other personal templates. Would you like to copy this Loan Template and have the personal template information removed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private bool isPersonalTemplate(LoanTemplate lt, string fieldName)
    {
      string field = lt.GetField(fieldName);
      return !(field.Trim() == "") && !FileSystemEntry.Parse(field, this.session.UserID).IsPublic;
    }

    private void onDefaultTemplateChanged()
    {
      if (this.DefaultLoanTemplateChanged == null)
        return;
      this.DefaultLoanTemplateChanged((object) this, EventArgs.Empty);
    }

    public FSExplorer.RESPAFilter RespaMode
    {
      set => this.respaMode = value;
    }

    public enum FileTypes
    {
      Ignore,
      PrintGroups,
      CustomForms,
      CustomLetters,
      Reports,
      LoanTemplates,
      DataTemplates,
      LoanPrograms,
      ClosingCosts,
      DocumentSets,
      FormLists,
      StackingOrderSets,
      CampaignTemplates,
      PurchaseAdvice,
      FundingTemplate,
      DashboardTemplate,
      DashboardViewTemplate,
      TaskSets,
      ConditionalLetter,
      SettlementServiceProviders,
      LoanDuplicationTemplate,
      AffiliatedBusinessArrangements,
    }

    private enum RootType
    {
      Top,
      Private,
      Public,
      PublicOnly,
    }
  }
}
