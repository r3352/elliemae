// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates
{
  public class SCTLoanBatchUpdates
  {
    public EventHandler AsynchronousProcessCompleted;
    public EventHandler LoanProcessCompleted;
    private AsynchronousProcess asynchronousProcess;
    private IProgressFeedback iProgFeedback;
    private SCTLoanUpdateCondition loanUpdateCondition;
    private List<Tuple<string, string>> updatedFieldIDs = new List<Tuple<string, string>>()
    {
      new Tuple<string, string>("761", "4527"),
      new Tuple<string, string>("432", "4528"),
      new Tuple<string, string>("762", "4529"),
      new Tuple<string, string>("2400", "4532")
    };
    private StreamWriter logWriter;
    private static string SUCCEEDWORD = "Processed";
    public static string ErrorFilePath = (string) null;
    public static string PendingListFile_AssignNew = Path.GetTempPath() + "SCTLoanBatchUpdate_PendingList_New.txt";
    public static string CompletedListFile_AssignNew = Path.GetTempPath() + "SCTLoanBatchUpdate_CompletedList_New.txt";
    private static string _logFilePath = Path.Combine(Environment.GetEnvironmentVariable("TMP"), "SCTUpdateTool.log");
    private DialogResult processingResult = DialogResult.Abort;
    private bool isCancelled;

    public SCTLoanBatchUpdates(
      SCTLoanUpdateCondition loanUpdateCondition,
      IProgressFeedback iProgFeedback)
    {
      this.loanUpdateCondition = loanUpdateCondition;
      this.iProgFeedback = iProgFeedback;
      this.asynchronousProcess = new AsynchronousProcess(this.updateLoans);
    }

    public void Run()
    {
      this.isCancelled = false;
      Thread thread = new Thread(new ThreadStart(this.runProcess));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
    }

    private void runProcess()
    {
      try
      {
        this.processingResult = this.asynchronousProcess((object) this.loanUpdateCondition, this.iProgFeedback);
        if (this.processingResult != DialogResult.None)
          return;
        this.processingResult = DialogResult.Abort;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        this.processingResult = DialogResult.Abort;
      }
      finally
      {
        if (this.AsynchronousProcessCompleted != null)
          this.AsynchronousProcessCompleted((object) this, new EventArgs());
      }
    }

    private DialogResult updateLoans(object state, IProgressFeedback feedback)
    {
      if (feedback != null)
        feedback.Status = "Searching Loans...";
      this.loanUpdateCondition = (SCTLoanUpdateCondition) state;
      try
      {
        Session.SessionObjects.LoanManager.GetLoanConfigurationInfo();
        if (!this.loanUpdateCondition.ProcessPendingList)
        {
          try
          {
            if (File.Exists(this.getPendingFile()))
              File.Delete(this.getPendingFile());
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "SCT Loan Batch Update Tool can not delete old pending list file \"" + this.getPendingFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return DialogResult.None;
          }
          try
          {
            if (File.Exists(this.getCompletedListFile()))
              File.Delete(this.getCompletedListFile());
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "SCT Loan Batch Update Tool can not delete file \"" + this.getCompletedListFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return DialogResult.None;
          }
          try
          {
            if (File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile()))
              File.Delete(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile());
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "SCT Loan Batch Update Tool can not delete log file \"" + EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return DialogResult.None;
          }
          if (EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath != null)
          {
            if (File.Exists(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath))
            {
              try
              {
                File.Delete(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath);
              }
              catch (Exception ex)
              {
                int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not delete error log file \"" + EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return DialogResult.None;
              }
            }
          }
          EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath = (string) null;
          if (this.parseCSV(feedback) != DialogResult.OK)
            return DialogResult.None;
        }
        try
        {
          this.logWriter = new StreamWriter(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.getLogFile(), true, Encoding.ASCII);
          this.logWriter.AutoFlush = true;
          this.addLogMsg("***** Process start time: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
        }
        catch (Exception ex)
        {
        }
        string pendingFile = this.getPendingFile();
        if (!File.Exists(pendingFile))
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Cannot find pending list file \"" + pendingFile + "\" in your temp folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        string[] strArray1 = (string[]) null;
        try
        {
          using (StreamReader streamReader = new StreamReader(pendingFile, Encoding.ASCII))
            strArray1 = streamReader.ReadToEnd().Split(new char[2]
            {
              '\n',
              '\r'
            }, StringSplitOptions.RemoveEmptyEntries);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Cannot read pending list file \"" + pendingFile + "\" in your temp folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        if (strArray1 == null || strArray1.Length == 0)
        {
          try
          {
            File.Delete(pendingFile);
          }
          catch (Exception ex)
          {
          }
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "The pending list file \"" + pendingFile + "\" in your temp folder is blank. Please try it again with new process.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        if (feedback != null && !feedback.ResetCounter(strArray1.Length))
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Progress status cannot be set.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.Abort;
        }
        if (feedback != null)
          feedback.Status = "Processing Loans...";
        string completedListFile = this.getCompletedListFile();
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
        if (File.Exists(completedListFile))
        {
          try
          {
            using (StreamReader streamReader = new StreamReader(completedListFile, Encoding.ASCII))
            {
              string str;
              while ((str = streamReader.ReadLine()) != null)
              {
                string key = str.Split(':')[0].Trim();
                if (!string.IsNullOrWhiteSpace(key) && key.StartsWith("{") && key.EndsWith("}"))
                  dictionary[key] = false;
              }
            }
          }
          catch (Exception ex)
          {
          }
        }
        int num1 = 0;
        List<string> stringList1 = (List<string>) null;
        List<string> stringList2 = (List<string>) null;
        using (StreamWriter streamWriter = new StreamWriter(completedListFile, true, Encoding.ASCII))
        {
          streamWriter.AutoFlush = true;
          for (int index1 = 0; index1 < strArray1.Length; ++index1)
          {
            if (this.isCancelled)
            {
              try
              {
                streamWriter.Close();
                this.addLogMsg("***** Process aborted at: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
                if (this.logWriter != null)
                  this.logWriter.Close();
              }
              catch (Exception ex)
              {
              }
              EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.PendingFile = pendingFile;
              return DialogResult.Abort;
            }
            string str1 = "Processing " + (object) (index1 + 1) + " / " + (object) strArray1.Length + "...";
            if (feedback != null)
              feedback.Details = str1;
            if (feedback != null && !feedback.Increment(1))
              return DialogResult.Abort;
            string[] strArray2 = new string[4]
            {
              strArray1[index1],
              null,
              null,
              null
            };
            if (dictionary.ContainsKey(strArray2[0]))
            {
              strArray2[3] = "Loan already processed.";
              if (this.LoanProcessCompleted != null)
                this.LoanProcessCompleted((object) strArray2, new EventArgs());
              this.addLogMsg((object[]) strArray2, false);
            }
            else
            {
              LoanDataMgr loanDataMgr;
              try
              {
                if (feedback != null)
                  feedback.Details = str1 + strArray1[index1].ToString() + " : loading loan...";
                loanDataMgr = LoanDataMgr.GetLoanDataMgr(Session.SessionObjects, Session.SessionObjects.LoanManager.OpenLoan(strArray1[index1].ToString()).GetLoanData(false), false, true);
              }
              catch (Exception ex)
              {
                strArray2[3] = "LoanDataMgr object cannot be initialized or loan is not found!";
                if (this.LoanProcessCompleted != null)
                  this.LoanProcessCompleted((object) strArray2, new EventArgs());
                bool isError = true;
                this.addLogMsg((object[]) strArray2, isError);
                ++num1;
                continue;
              }
              try
              {
                LoanData loanData = loanDataMgr.LoanData;
                strArray2[1] = loanData.GetField("364");
                strArray2[2] = loanData.GetField("36") + " " + loanData.GetField("37");
                if (loanDataMgr.LoanData.GetField("2626") != "Correspondent")
                {
                  strArray2[3] = "Loan is not a Correspondent loan.";
                  bool isError = true;
                  this.addLogMsg((object[]) strArray2, isError);
                  if (this.LoanProcessCompleted != null)
                    this.LoanProcessCompleted((object) strArray2, new EventArgs());
                  loanData.Close();
                  loanDataMgr.Close();
                  continue;
                }
                List<string> stringList3 = new List<string>();
                for (int index2 = 0; index2 < this.updatedFieldIDs.Count; ++index2)
                {
                  string field = loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index2].Item2);
                  if (field != "//" && field != "" && (!(this.updatedFieldIDs[index2].Item2 == "4532") || !(field == "N")))
                    stringList3.Add(this.updatedFieldIDs[index2].Item2);
                }
                if (stringList3.Count > 0)
                {
                  if (stringList3.Count == 1)
                  {
                    strArray2[3] = "Loan has already has data in target field: \"" + stringList3[0] + "\".";
                  }
                  else
                  {
                    strArray2[3] = "Loan has already has data in target fields: ";
                    for (int index3 = 0; index3 < stringList3.Count; ++index3)
                    {
                      if (index3 > 0)
                      {
                        // ISSUE: explicit reference operation
                        ^ref strArray2[3] += ", ";
                      }
                      if (index3 == stringList3.Count - 1)
                      {
                        // ISSUE: explicit reference operation
                        ^ref strArray2[3] += "and ";
                      }
                      ref string local = ref strArray2[3];
                      local = local + "\"" + stringList3[index3] + "\"";
                    }
                    // ISSUE: explicit reference operation
                    ^ref strArray2[3] += ".";
                  }
                  bool isError = true;
                  this.addLogMsg((object[]) strArray2, isError);
                  if (this.LoanProcessCompleted != null)
                    this.LoanProcessCompleted((object) strArray2, new EventArgs());
                  loanData.Close();
                  loanDataMgr.Close();
                  continue;
                }
                bool flag = false;
                LockInfo lockInfo = (LockInfo) null;
                LockInfo[] currentLocks = loanDataMgr.GetCurrentLocks();
                if (currentLocks != null && currentLocks.Length != 0)
                {
                  for (int index4 = 0; index4 < currentLocks.Length; ++index4)
                  {
                    if (currentLocks[index4] != null && string.Compare(currentLocks[index4].LockedBy, Session.UserID, true) != 0)
                    {
                      flag = true;
                      lockInfo = currentLocks[index4];
                      break;
                    }
                  }
                }
                if (flag)
                {
                  strArray2[3] = "Loan is locked by user \"" + lockInfo.LockedByFirstName + " " + lockInfo.LockedByLastName + "\"!";
                  loanData.Close();
                  loanDataMgr.Close();
                  if (this.LoanProcessCompleted != null)
                    this.LoanProcessCompleted((object) strArray2, new EventArgs());
                  bool isError = true;
                  this.addLogMsg((object[]) strArray2, isError);
                  ++num1;
                  continue;
                }
                if (feedback != null)
                  feedback.Details = str1 + strArray1[index1].ToString() + " : locking loan...";
                loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
                stringList1 = new List<string>();
                stringList2 = new List<string>();
                for (int index5 = 0; index5 < this.updatedFieldIDs.Count; ++index5)
                {
                  string field1 = loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index5].Item1);
                  if (!loanDataMgr.LoanData.IsLocked(this.updatedFieldIDs[index5].Item2))
                  {
                    stringList1.Add(loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index5].Item2));
                    loanDataMgr.LoanData.SetField(this.updatedFieldIDs[index5].Item2, field1);
                    string field2 = loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index5].Item2);
                    stringList2.Add(field2);
                  }
                }
                if (feedback != null)
                  feedback.Details = str1 + strArray1[index1].ToString() + " : saving loan...";
                loanDataMgr.LoanData.SetCurrentField("CALCREQUIRED", "");
                loanDataMgr.Save(false, false);
                if (feedback != null)
                  feedback.Details = str1 + strArray1[index1].ToString() + " : unlocking loan...";
                loanDataMgr.Unlock();
                loanDataMgr.LoanData.Close();
                if (feedback != null)
                  feedback.Details = str1 + strArray1[index1].ToString() + " : loan is " + EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD.ToLower() + ".";
                strArray2[3] = EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD;
                dictionary.Add(strArray2[0], true);
                streamWriter.WriteLine(strArray2[0].ToString() + " : " + loanDataMgr.LoanFolder + " : " + (strArray2[1] != null ? strArray2[1].ToString() : "") + " : " + (strArray2[2] != null ? strArray2[2].ToString().Replace(":", "") : "") + " : " + strArray2[3].ToString());
              }
              catch (Exception ex)
              {
                if (strArray2 == null)
                  strArray2 = new string[4]
                  {
                    strArray1[index1],
                    null,
                    null,
                    null
                  };
                strArray2[3] = "Error - " + ex.Message;
                ++num1;
              }
              if (this.LoanProcessCompleted != null)
                this.LoanProcessCompleted((object) strArray2, new EventArgs());
              this.addLogMsg((object[]) strArray2, false);
              if (strArray2 != null && strArray2.Length >= 4 && string.Compare(strArray2[3].ToString(), EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD, true) == 0)
              {
                string str2 = "";
                for (int index6 = 0; index6 < this.updatedFieldIDs.Count; ++index6)
                  str2 = str2 + (str2 != "" ? " ~ " : "") + this.updatedFieldIDs[index6].Item2 + ":" + stringList1[index6] + "|" + stringList2[index6];
                this.addLogMsg("Detail - " + str2);
              }
            }
          }
          streamWriter.Close();
        }
        EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.PendingFile = (string) null;
        if (num1 == 0)
        {
          try
          {
            File.Delete(pendingFile);
          }
          catch (Exception ex)
          {
          }
        }
        try
        {
          this.addLogMsg("***** Process completion time: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
          if (this.logWriter != null)
            this.logWriter.Close();
        }
        catch (Exception ex)
        {
        }
      }
      finally
      {
        this.loanUpdateCondition = (SCTLoanUpdateCondition) null;
      }
      return DialogResult.OK;
    }

    private void addLogMsg(object[] loanBasicInfo, bool isError)
    {
      if (isError)
      {
        if (EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath == null)
        {
          EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath = Path.Combine(Path.GetTempPath(), "SCTLoanBatchUpdate_ErrorList_New.txt");
          using (StreamWriter streamWriter = new StreamWriter(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath, false, Encoding.ASCII))
            streamWriter.WriteLine("\"GUID\",\"Loan #\",\"Borrower\",\"Result\"");
        }
        using (StreamWriter streamWriter = new StreamWriter(EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.ErrorFilePath, true, Encoding.ASCII))
        {
          for (int index = 0; index < loanBasicInfo.Length; ++index)
          {
            if (index > 0)
              streamWriter.Write(", ");
            streamWriter.Write(string.Format("\"{0}\"", loanBasicInfo[index]));
          }
          streamWriter.WriteLine();
        }
      }
      this.addLogMsg(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + " : " + loanBasicInfo[0].ToString() + " - " + (loanBasicInfo[3].ToString() == EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD ? "(" + EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD + ")  " : "(Error)    ") + (loanBasicInfo[1] != null ? loanBasicInfo[1].ToString() : "") + " : " + (loanBasicInfo[2] != null ? loanBasicInfo[2].ToString().Replace(":", "") : "") + (loanBasicInfo[3].ToString() != EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.SUCCEEDWORD ? " * " + loanBasicInfo[3].ToString() : ""));
    }

    private void addLogMsg(string msg)
    {
      if (this.logWriter == null)
        return;
      this.logWriter.WriteLine(msg);
    }

    private DialogResult parseCSV(IProgressFeedback feedback)
    {
      string[][] strArray = (string[][]) null;
      int index1 = -1;
      using (CsvParser csvParser = new CsvParser((TextReader) new StreamReader(this.loanUpdateCondition.CSVFile, Encoding.Default), true))
      {
        strArray = csvParser.RemainingRows();
        if (strArray != null)
        {
          if (strArray.Length > 1)
          {
            if (strArray[0] != null)
            {
              if (strArray[1] != null)
              {
                for (int index2 = 0; index2 < strArray[0].Length; ++index2)
                {
                  if ((string.Compare(strArray[0][index2], "Loan Info Loan ID", true) == 0 || string.Compare(strArray[0][index2], "GUID", true) == 0) && (strArray[1][index2] ?? "") != null && strArray[1][index2].StartsWith("{") && strArray[1][index2].EndsWith("}") && strArray[1][index2].Length == 38)
                  {
                    index1 = index2;
                    break;
                  }
                }
              }
            }
          }
        }
      }
      if (index1 == -1)
      {
        string text = "File must be in CSV format and provided csv file must contain Loan GUID in order to update loan data.";
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.None;
      }
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(this.getPendingFile(), false, Encoding.ASCII))
        {
          for (int index3 = 1; index3 < strArray.Length; ++index3)
            streamWriter.WriteLine(strArray[index3][index1]);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "You do not have access rights to write Process Pending List file to Windows temporary folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.None;
      }
      return DialogResult.OK;
    }

    public static string PendingFile { get; private set; }

    public string getPendingFile() => EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.PendingListFile_AssignNew;

    private string getCompletedListFile() => EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates.CompletedListFile_AssignNew;

    public static string getLogFile() => EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates.SCTLoanBatchUpdates._logFilePath;

    public DialogResult ProcessingResult => this.processingResult;

    public bool IsCancelled
    {
      set => this.isCancelled = value;
    }
  }
}
