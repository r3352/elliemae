// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.CustomLetterIFSExplorer
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class CustomLetterIFSExplorer : IFSExplorerBase
  {
    private const string className = "CustomLetterIFSExplorer";
    private static readonly string sw = Tracing.SwCustomLetters;
    private bool noAccessCheck;
    private IMainScreen mainScreen;
    private string localCustomLetterDir = SystemSettings.LocalCustomLetterDir;
    private CustomLetterType letterType = CustomLetterType.Generic;
    private Sessions.Session session;

    public event EventHandler CustomFormDetailChanged;

    public CustomLetterIFSExplorer(Sessions.Session session, EllieMae.EMLite.ContactUI.ContactType contactType)
    {
      this.session = session;
      this.mainScreen = (IMainScreen) null;
      if (contactType == EllieMae.EMLite.ContactUI.ContactType.BizPartner)
        this.letterType = CustomLetterType.BizPartner;
      else
        this.letterType = CustomLetterType.Borrower;
    }

    public CustomLetterIFSExplorer(Sessions.Session session, IMainScreen mainScreen)
    {
      this.session = session;
      this.mainScreen = mainScreen;
    }

    public CustomLetterIFSExplorer(
      Sessions.Session session,
      IMainScreen mainScreen,
      bool noAccessCheck)
    {
      this.session = session;
      this.mainScreen = mainScreen;
      this.noAccessCheck = noAccessCheck;
    }

    public CustomLetterIFSExplorer(
      Sessions.Session session,
      EllieMae.EMLite.ContactUI.ContactType contactType,
      bool noAccessCheck)
      : this(session, contactType)
    {
      this.noAccessCheck = noAccessCheck;
    }

    public override void Import(FileSystemEntry folder)
    {
      ImportDialog importDialog = new ImportDialog();
      if (importDialog.ShowDialog() != DialogResult.OK)
        return;
      string[] sourceFile = importDialog.SourceFile;
      string sourcePath = importDialog.SourcePath;
      bool keepField = importDialog.KeepField;
      for (int index = 0; index < sourceFile.Length; ++index)
        this.ImportForm(folder, sourcePath, sourceFile[index], keepField);
    }

    public void ImportForm(
      FileSystemEntry folder,
      string sourcepath,
      string sourcefile,
      bool keepField)
    {
      this.ImportForm(folder, sourcepath, sourcefile, keepField, false);
    }

    public bool ImportForm(
      FileSystemEntry folder,
      string sourcepath,
      string sourcefile,
      bool keepField,
      bool skipDialog)
    {
      if (!File.Exists(sourcepath + "\\" + sourcefile))
      {
        if (skipDialog)
        {
          Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "The '" + sourcepath + "\\" + sourcefile + "' cannot be found.\r\n");
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "The '" + sourcepath + "\\" + sourcefile + "' cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      FileSystemEntry fileSystemEntry = new FileSystemEntry(folder.Path, sourcefile, FileSystemEntry.Types.File, folder.Owner);
      if (this.session.ConfigurationManager.CustomLetterObjectExistsOfAnyType(this.letterType, fileSystemEntry))
      {
        DialogResult dialogResult;
        if (skipDialog)
        {
          if (Utils.Dialog((IWin32Window) null, "The Custom Form '" + fileSystemEntry.Name + "' already exists. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          {
            Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "The Custom Form '" + sourcepath + "\\" + sourcefile + "' was not imported because a file with the same name already exists and user chose not to overwrite it.\r\n");
            return false;
          }
          dialogResult = DialogResult.No;
        }
        else
          dialogResult = Utils.Dialog((IWin32Window) null, "A file or folder with the name '" + fileSystemEntry.Name + "' already exists. Would you like to use different name?\r\n Choose Yes to rename, No to overwrite, and Cancel to not import.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        if (dialogResult != DialogResult.Yes && dialogResult != DialogResult.No)
          return false;
        if (dialogResult == DialogResult.Yes)
        {
          NewLetter newLetter = new NewLetter(this.letterType, fileSystemEntry);
          if (newLetter.ShowDialog() != DialogResult.OK)
            return false;
          fileSystemEntry = newLetter.LetterEntry;
        }
      }
      string str1 = this.letterType != CustomLetterType.Borrower ? (this.letterType != CustomLetterType.BizPartner ? SystemSettings.LocalCustomLetterDir : SystemSettings.LocalBizLetterDir) : SystemSettings.LocalBorLetterDir;
      string str2 = SystemUtil.CombinePath(str1, fileSystemEntry.GetEncodedPath());
      Directory.CreateDirectory(Path.GetDirectoryName(str2));
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        File.Copy(sourcepath + "\\" + sourcefile, str2, true);
        File.SetAttributes(str2, FileAttributes.Normal);
      }
      catch (Exception ex)
      {
        if (!skipDialog)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + sourcefile + " cannot be imported because you don't have access rights to '" + str1 + "' folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "ErrorCode: " + ex.Message);
        return false;
      }
      if (!keepField)
      {
        WordHandler wordHandler1 = (WordHandler) null;
        WordHandler wordHandler2;
        try
        {
          wordHandler2 = new WordHandler();
        }
        catch (Exception ex)
        {
          if (!skipDialog)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Error occurred in MailMerge.");
          Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), ex.StackTrace);
          wordHandler1?.ShutDown();
          return false;
        }
        if (!wordHandler2.ImportLetter(str2))
        {
          if (skipDialog)
          {
            Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "The '" + sourcefile + " cannot be imported because existing fields cannot be removed.\r\n");
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The '" + sourcefile + " cannot be imported because existing fields cannot be removed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return false;
        }
        Thread.Sleep(5000);
      }
      try
      {
        using (BinaryObject data = new BinaryObject(str2))
          this.session.ConfigurationManager.SaveCustomLetter(this.letterType, fileSystemEntry, data);
      }
      catch (Exception ex)
      {
        if (skipDialog)
        {
          Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Encompass client cannot save custom letter.  Esception: " + ex.Message + "\r\n");
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "Encompass client cannot save custom letter." + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      File.Delete(str2);
      Cursor.Current = Cursors.Default;
      return true;
    }

    public FileSystemEntry ImportForm(string sourceFilePath, FileSystemEntry fsTargetFile)
    {
      if (!File.Exists(sourceFilePath))
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The file '" + sourceFilePath + "' could not be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry) null;
      }
      while (this.session.ConfigurationManager.CustomLetterObjectExistsOfAnyType(this.letterType, fsTargetFile))
        fsTargetFile = new FileSystemEntry(this.getNextCopyToFileName(fsTargetFile.Path), fsTargetFile.Type, fsTargetFile.Owner);
      try
      {
        using (BinaryObject data = new BinaryObject(sourceFilePath))
          this.session.ConfigurationManager.SaveCustomLetter(this.letterType, fsTargetFile, data);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The file '" + sourceFilePath + "' could not be imported.\r\nError message follows: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry) null;
      }
      return fsTargetFile;
    }

    private string getNextCopyToFileName(string path)
    {
      string directoryName = Path.GetDirectoryName(path);
      string fileName = Path.GetFileName(path);
      Match match1 = new Regex("^Copy (\\((\\d+)\\)) of ", RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(fileName);
      if (match1.Success)
      {
        string oldValue = match1.Value;
        string newValue = "Copy (" + (int.Parse(match1.Groups[2].Value) + 1).ToString() + ") of ";
        string path2 = fileName.Replace(oldValue, newValue);
        return Path.Combine(directoryName, path2);
      }
      Match match2 = new Regex("^Copy of ", RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(fileName);
      if (!match2.Success)
        return Path.Combine(directoryName, "Copy of " + fileName);
      string oldValue1 = match2.Value;
      string newValue1 = "Copy (2) of ";
      string path2_1 = fileName.Replace(oldValue1, newValue1);
      return Path.Combine(directoryName, path2_1);
    }

    public override string NewDocBaseName
    {
      get
      {
        if (this.letterType == CustomLetterType.Borrower)
          return "New Borrower Letter";
        return this.letterType == CustomLetterType.BizPartner ? "New Business Partner Letter" : "New Custom Form";
      }
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry entry)
    {
      try
      {
        return !this.noAccessCheck ? this.session.ConfigurationManager.GetFilteredCustomLetterDirEntries(this.letterType, entry) : this.session.ConfigurationManager.GetCustomLetterDirEntries(this.letterType, entry);
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
    }

    public override void DeleteEntry(FileSystemEntry entry)
    {
      this.session.ConfigurationManager.DeleteCustomLetterObject(this.letterType, entry);
    }

    public override void MoveEntry(FileSystemEntry source, FileSystemEntry target)
    {
      this.session.ConfigurationManager.MoveCustomLetterObject(this.letterType, source, target);
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      this.session.ConfigurationManager.CopyCustomLetterObject(this.letterType, source, target);
    }

    public override void CreateFolder(FileSystemEntry entry)
    {
      this.session.ConfigurationManager.CreateCustomLetterFolder(this.letterType, entry);
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.CustomLetterObjectExists(this.letterType, entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.CustomLetterObjectExistsOfAnyType(this.letterType, entry);
    }

    public override string NewDocExtension => ".DOC";

    public override bool CreateNew(FileSystemEntry entry)
    {
      string str = SystemUtil.CombinePath(this.localCustomLetterDir, entry.GetEncodedPath());
      if (!Directory.Exists(Path.GetDirectoryName(str)))
        Directory.CreateDirectory(Path.GetDirectoryName(str));
      BinaryObject data = (BinaryObject) null;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        WordHandler wordHandler = new WordHandler();
        wordHandler.CreateEmptyDoc(str);
        wordHandler.ShutDown();
        data = new BinaryObject(str);
        this.session.ConfigurationManager.NewCustomLetter(this.letterType, entry, data);
      }
      catch (Exception ex)
      {
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Error" + ex.Message);
      }
      data?.Dispose();
      Cursor.Current = Cursors.Default;
      return true;
    }

    public override void OpenFile(FileSystemEntry entry) => this.OpenFile(entry, (GVItem) null);

    public override void OpenFile(FileSystemEntry entry, GVItem lvItem)
    {
      string str = SystemUtil.CombinePath(this.localCustomLetterDir, entry.GetEncodedPath());
      string directoryName = Path.GetDirectoryName(str);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      try
      {
        BinaryObject customLetter = this.session.ConfigurationManager.GetCustomLetter(this.letterType, entry);
        if (customLetter == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + entry.Name + "' custom letter cannot be found or no longer exists.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          customLetter.Write(str);
          this.editLetter(entry, str, false, lvItem);
        }
      }
      catch (ServerFileIOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Encompass server cannot access the file " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (LocalFileIOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Encompass client cannot access the file " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (FileDownloadException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Custom letter cannot be downloaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void editLetter(
      FileSystemEntry entry,
      string localFileName,
      bool bNewFile,
      GVItem lvItem)
    {
      Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Verbose, nameof (CustomLetterIFSExplorer), "Edit Template " + localFileName);
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.letterType != CustomLetterType.Generic)
        {
          ContactFieldDialog contactFieldDialog = new ContactFieldDialog(this.session, localFileName, entry, this.letterType);
          if (contactFieldDialog.InitializeFieldList(bNewFile))
            return;
          contactFieldDialog.Close();
        }
        else
        {
          InsertFieldDialog insertFieldDialog = new InsertFieldDialog(localFileName, lvItem, (CustomLetterPanel) null, this.session.MainScreen, this.session);
          insertFieldDialog.CustomFormDetailChanged += new EventHandler(this.insertFieldDialog_CustomFormDetailChanged);
          if (insertFieldDialog.InitializeFieldList(bNewFile))
            return;
          insertFieldDialog.Close();
          this.session.Application.Enable();
        }
      }
      catch (COMException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot edit the selected form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "ErrorCode" + (object) ex.ErrorCode);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "TagetSite" + (object) ex.TargetSite);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Message" + ex.Message);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Source" + ex.Source);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "StackTrace" + ex.StackTrace);
        this.session.Application.Enable();
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Application could not Merge Template", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Message" + ex.Message);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "Source" + ex.Source);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), "TargetSite" + (object) ex.TargetSite);
        this.session.Application.Enable();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This letter could not be edited.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(CustomLetterIFSExplorer.sw, TraceLevel.Error, nameof (CustomLetterIFSExplorer), ex.Message + " " + ex.Source);
        this.session.Application.Enable();
      }
    }

    private void insertFieldDialog_CustomFormDetailChanged(object sender, EventArgs e)
    {
      if (this.CustomFormDetailChanged == null)
        return;
      this.CustomFormDetailChanged((object) this, e);
    }
  }
}
