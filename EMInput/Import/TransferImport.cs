// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.TransferImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class TransferImport
  {
    private const string className = "TransferImport";
    private static readonly string sw = Tracing.SwImportExport;
    private string zipFile = string.Empty;
    private bool applyToAll;
    private bool overwriteAll;
    private bool cancelAll;
    private int fileCount;
    private ILoan loan;
    private string tempFolderRoot = Path.Combine(SystemSettings.TempFolderRoot, "(EMTransfer)");

    public TransferImport(string zipFile, string targetFolder)
    {
      if (!(zipFile != string.Empty))
        return;
      this.zipFile = zipFile;
      int num1 = this.ImportMessageList(targetFolder);
      try
      {
        Directory.Delete(this.tempFolderRoot, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferImport.sw, TraceLevel.Error, nameof (TransferImport), "Cannot delete '" + this.tempFolderRoot + "' folder. " + ex.Message);
      }
      if (num1 == 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "The one loan file you selected has been successfully added.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (num1 > 1)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The " + num1.ToString() + " loan files have been successfully added.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (num1 != 0)
          return;
        int num4 = (int) Utils.Dialog((IWin32Window) null, "No loan files were added.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private int ImportMessageList(string baseFolder)
    {
      if (!File.Exists(this.zipFile))
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "The program was unable to locate the '" + this.zipFile + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      if (!this.UnzipFileToTempFolder(this.tempFolderRoot))
        return -1;
      FileInfo[] files = new DirectoryInfo(this.tempFolderRoot).GetFiles();
      this.fileCount = files.Length;
      int num1 = 0;
      for (int index = 0; index < files.Length; ++index)
      {
        string upper = files[index].Name.ToUpper();
        int length = upper.IndexOf(".ZIP");
        string localFile = string.Empty;
        if (length > -1)
          localFile = upper.Substring(0, length);
        if (!(localFile == string.Empty) && this.AddFileToLoanFolder(baseFolder, localFile, this.tempFolderRoot))
          ++num1;
      }
      return num1;
    }

    private bool AddFileToLoanFolder(string targetFolder, string localFile, string tempDir)
    {
      Tracing.Log(TransferImport.sw, nameof (TransferImport), TraceLevel.Info, "AddFileToLoanFolder: targetFolder(" + targetFolder + ") starts at" + DateTime.Now.ToString());
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int length = localFile.IndexOf("(");
      string loanName1;
      string guid;
      if (length == -1)
      {
        loanName1 = "Transfer";
        guid = localFile;
      }
      else
      {
        guid = localFile.Substring(0, length);
        int num = localFile.IndexOf(")");
        loanName1 = num != -1 ? localFile.Substring(length + 1, num - length - 1) : "Transfer";
      }
      string loanName2 = (string) null;
      bool flag1 = true;
      bool cancel = false;
      LoanInfo.Right right = LoanInfo.Right.FullRight;
      LoanIdentity loanIdentity = Session.LoanManager.GetLoanIdentity(guid);
      LockInfo lockInfo = (LockInfo) null;
      bool flag2;
      if (loanIdentity != (LoanIdentity) null)
      {
        flag2 = false;
        loanName2 = loanIdentity.LoanName;
        try
        {
          this.loan = Session.LoanManager.OpenLoan(guid);
          right = this.loan.GetRights();
          lockInfo = this.loan.GetLockInfo();
          this.loan.Close();
        }
        catch (SecurityException ex)
        {
          right = LoanInfo.Right.NoRight;
        }
      }
      else
        flag2 = true;
      if (!flag2)
      {
        if (right == LoanInfo.Right.NoRight)
        {
          if (Utils.Dialog((IWin32Window) Session.MainScreen, "You do not have the necessary rights to overwrite the '" + loanName2 + "' loan file. Would you like to import it as a new loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            return false;
          flag1 = false;
        }
        else if (lockInfo.LockedFor != LoanInfo.LockReason.NotLocked)
        {
          if (Utils.Dialog((IWin32Window) Session.MainScreen, "Someone is currently working on the loan file (" + loanName2 + ") so it cannot be overwritten. Would you like to import it as a new loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            return false;
          flag1 = false;
        }
        else
        {
          if (!this.applyToAll)
          {
            flag1 = this.overwriteExistingLoan(ref cancel, loanName2);
          }
          else
          {
            cancel = this.cancelAll;
            flag1 = this.overwriteAll;
          }
          if (cancel)
            return false;
        }
      }
      try
      {
        string str1 = tempDir + "\\" + localFile + ".ZIP";
        string str2 = tempDir + "\\" + guid;
        string path = str2 + "\\loan.em";
        FileCompressor.Instance.Unzip(str1, str2);
        if (File.Exists(path))
        {
          using (BinaryObject binaryObject = new BinaryObject(path))
          {
            if (flag2)
              this.loan = Session.LoanManager.Import(targetFolder, loanName1, binaryObject, DuplicateLoanAction.Rename);
            else if (!flag1)
            {
              this.loan = Session.LoanManager.ImportNew(targetFolder, loanName1, binaryObject, DuplicateLoanAction.Rename, true);
            }
            else
            {
              this.loan = Session.LoanManager.OpenLoan(guid, LoanInfo.LockReason.Downloaded);
              this.loan.Import(binaryObject, targetFolder);
            }
          }
          bool flag3 = false;
          bool flag4 = false;
          bool flag5 = false;
          foreach (LoanProperty loanPropertySetting in this.loan.GetLoanPropertySettings())
          {
            if (string.Equals(loanPropertySetting.Category, "LoanStorage", StringComparison.OrdinalIgnoreCase) && string.Equals(loanPropertySetting.Attribute, "SupportingData", StringComparison.OrdinalIgnoreCase))
            {
              if (string.Equals(loanPropertySetting.Value, "SkyDrive", StringComparison.OrdinalIgnoreCase))
              {
                flag3 = true;
                break;
              }
              if (string.Equals(loanPropertySetting.Value, "SkyDriveLite", StringComparison.OrdinalIgnoreCase))
              {
                flag4 = true;
                break;
              }
              if (string.Equals(loanPropertySetting.Value, "SkyDriveClassic", StringComparison.OrdinalIgnoreCase))
              {
                flag5 = true;
                break;
              }
              break;
            }
          }
          SkyDriveStreamingClient driveStreamingClient = (SkyDriveStreamingClient) null;
          if (flag3 | flag4 | flag5)
            driveStreamingClient = new SkyDriveStreamingClient(this.loan, Session.StartupInfo);
          foreach (string file in Directory.GetFiles(str2))
          {
            string fileName = Path.GetFileName(file);
            if (fileName.ToLower() != "loan.em".ToLower())
            {
              using (BinaryObject data = new BinaryObject(file))
              {
                Tracing.Log(TransferImport.sw, nameof (TransferImport), TraceLevel.Info, "AddFileToLoanFolder: importFile(" + file + ")", data != null ? data.Length : 0L);
                if (flag3 | flag4 | flag5 && !Utils.IsCIFsOnlyFile(fileName))
                  Task.WaitAll((Task) driveStreamingClient.SaveSupportingData(fileName, data));
                else
                  this.loan.SaveSupportingDataOnCIFs(fileName, data);
              }
            }
          }
          this.loan.Unlock();
          LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, this.loan.Guid, false);
          loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
          loanDataMgr.LoanData.LoanNumber = "";
          loanDataMgr.SetMilestoneTemplateOnNew("", (Hashtable) null, true);
          LoanData loanData = loanDataMgr.LoanData;
          MilestoneLog milestone = loanData.GetLogList().GetMilestone("Started");
          UserInfo userInfo = Session.UserInfo;
          try
          {
            EncompassDocs.SetDocEngine((IHtmlInput) loanData, "New_Encompass_Docs_Solution");
          }
          catch (Exception ex)
          {
          }
          if ((flag2 || milestone.LoanAssociateID == "" || !flag2 && !flag1) && userInfo != (UserInfo) null)
            milestone.SetLoanAssociate(userInfo);
          try
          {
            loanDataMgr.SetLoanNumber("");
          }
          catch (MissingPrerequisiteException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "An error occurred while attempting to import this loan: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          loanDataMgr.SaveWithoutAuditRecord(loanData);
          loanDataMgr.Close();
        }
        Directory.Delete(str2, true);
        File.Delete(str1);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferImport.sw, TraceLevel.Error, nameof (TransferImport), "Cannot add file to loan folder. " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "An error occurred while attempting to import this loan: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Tracing.Log(TransferImport.sw, nameof (TransferImport), TraceLevel.Info, "AddFileToLoanFolder: targetFolder(" + targetFolder + ") ends at" + DateTime.Now.ToString());
      return true;
    }

    private bool overwriteExistingLoan(ref bool cancel, string loanName)
    {
      string caption = "Loan Conflict";
      string text = "There is a writable copy of the loan file (" + loanName + ") on the server.  The copy might have been modified.  Do you want to overwrite it?";
      string[] options = new string[2]
      {
        "Overwrite",
        "Import as a new loan"
      };
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank) && !aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl))
        options = new string[1]{ "Overwrite" };
      bool showCheckBox = this.fileCount > 1;
      TransferOptionDialog transferOptionDialog = new TransferOptionDialog(caption, text, options, 0, showCheckBox);
      DialogResult dialogResult = transferOptionDialog.ShowDialog();
      if (transferOptionDialog.AppyToAll)
        this.applyToAll = true;
      if (dialogResult == DialogResult.Cancel)
      {
        if (this.applyToAll)
          this.cancelAll = true;
        cancel = true;
        return true;
      }
      if (transferOptionDialog.SelectedOption == 1)
      {
        if (this.applyToAll)
          this.overwriteAll = false;
        return false;
      }
      if (this.applyToAll)
        this.overwriteAll = true;
      return true;
    }

    private bool UnzipFileToTempFolder(string tempFolder)
    {
      try
      {
        if (Directory.Exists(tempFolder))
          Directory.Delete(tempFolder, true);
        Directory.CreateDirectory(tempFolder);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferImport.sw, TraceLevel.Error, nameof (TransferImport), "Cannot create '" + tempFolder + "' folder. " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "Due to network problem, the '" + this.zipFile + " cannot be imported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        FileCompressor.Instance.Unzip(this.zipFile, tempFolder);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferImport.sw, TraceLevel.Error, nameof (TransferImport), "Cannot unzip '" + this.zipFile + "' file to '" + tempFolder + "' folder. " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "Due to network problem, the '" + this.zipFile + " cannot be imported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return true;
    }
  }
}
