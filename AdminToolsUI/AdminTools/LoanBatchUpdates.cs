// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanBatchUpdates
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer.Query;
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
namespace EllieMae.EMLite.AdminTools
{
  public class LoanBatchUpdates
  {
    public static string PendingListFile_AssignNew = Path.GetTempPath() + "LoanBatchUpdate_PendingList_New.txt";
    public static string PendingListFile_Options = Path.GetTempPath() + "LoanBatchUpdate_PendingList_Options.txt";
    public static string CompletedListFile_AssignNew = Path.GetTempPath() + "LoanBatchUpdate_CompletedList_New.txt";
    public EventHandler AsynchronousProcessCompleted;
    public EventHandler LoanProcessCompleted;
    private AsynchronousProcess asynchronousProcess;
    private IProgressFeedback iProgFeedback;
    private LoanUpdateCondition loanUpdateCondition;
    private List<string> updatedFieldIDs = new List<string>()
    {
      "HMDA.X13",
      "HMDA.X29",
      "HMDA.X31",
      "HMDA.X35",
      "HMDA.X59",
      "HMDA.X71",
      "HMDA.X77",
      "HMDA.X78",
      "HMDA.X79",
      "HMDA.X80",
      "HMDA.X83",
      "HMDA.X84",
      "HMDA.X85",
      "HMDA.X100",
      "HMDA.X111",
      "HMDA.X112",
      "HMDA.X116",
      "HMDA.X118",
      "HMDA.X120",
      "HMDA.X114",
      "HMDA.X115"
    };
    private StreamWriter logWriter;
    private static string SUCCEEDWORD = "Processed";
    private DialogResult processingResult = DialogResult.Abort;
    private bool isCancelled;

    public LoanBatchUpdates(
      LoanUpdateCondition loanUpdateCondition,
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
      LoanUpdateCondition loanUpdateCondition = (LoanUpdateCondition) state;
      HMDAProfile hmdaProfile = (HMDAProfile) null;
      HMDAInformation hmdaInformation = (HMDAInformation) null;
      Session.SessionObjects.LoanManager.GetLoanConfigurationInfo();
      if (this.loanUpdateCondition.UpdateOption != LoanUpdateCondition.UpdateOptions.UpdateOnly)
      {
        hmdaProfile = Session.SessionObjects.ConfigurationManager.GetHMDAProfileById(loanUpdateCondition.NewProfileID);
        if (hmdaProfile == null)
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not read HMDA Profile \"" + (object) loanUpdateCondition.NewProfileID + "\" in Encompass setting. ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        hmdaInformation = new HMDAInformation(hmdaProfile.HMDAProfileSetting);
      }
      else if (loanUpdateCondition.HmdaProfilesLookup.Count == 0)
      {
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not read all HMDA Profiles in Encompass setting. ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.None;
      }
      if (!loanUpdateCondition.ProcessPendingList)
      {
        try
        {
          if (File.Exists(this.getPendingFile()))
            File.Delete(this.getPendingFile());
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not delete old pending list file \"" + this.getPendingFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        try
        {
          if (File.Exists(this.getCompletedListFile()))
            File.Delete(this.getCompletedListFile());
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not delete file \"" + this.getCompletedListFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        try
        {
          if (File.Exists(this.getLogFile()))
            File.Delete(this.getLogFile());
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Loan Batch Update Tool can not delete log file \"" + this.getLogFile() + "\". " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return DialogResult.None;
        }
        if (this.parseCSV(feedback) != DialogResult.OK)
          return DialogResult.None;
      }
      try
      {
        this.logWriter = new StreamWriter(this.getLogFile(), true, Encoding.ASCII);
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
      string[] strArray = (string[]) null;
      try
      {
        using (FileStream fileStream = new FileStream(pendingFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          {
            strArray = streamReader.ReadToEnd().Split('|');
            streamReader.Close();
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Cannot read pending list file \"" + pendingFile + "\" in your temp folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.None;
      }
      if (strArray == null || strArray.Length == 0)
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
      if (feedback != null && !feedback.ResetCounter(strArray.Length))
      {
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "Progress status cannot be set.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
      if (feedback != null)
        feedback.Status = "Processing Loans...";
      string completedListFile = this.getCompletedListFile();
      string str1 = "";
      if (File.Exists(completedListFile))
      {
        try
        {
          using (FileStream fileStream = new FileStream(completedListFile, FileMode.Open, FileAccess.Read, FileShare.Read))
          {
            using (StreamReader streamReader = new StreamReader((Stream) fileStream))
            {
              str1 = streamReader.ReadToEnd();
              streamReader.Close();
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
      using (FileStream fileStream = new FileStream(completedListFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          streamWriter.AutoFlush = true;
          for (int index1 = 0; index1 < strArray.Length; ++index1)
          {
            string str2 = "Processing " + (object) (index1 + 1) + " / " + (object) strArray.Length + "...";
            if (feedback != null)
              feedback.Details = str2;
            if (feedback != null && !feedback.Increment(1))
              return DialogResult.Abort;
            object[] objArray = new object[4]
            {
              (object) strArray[index1].ToString(),
              null,
              null,
              null
            };
            if (!(str1 != "") || str1.IndexOf(strArray[index1].ToString()) <= -1)
            {
              LoanDataMgr loanDataMgr;
              try
              {
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : loading loan...";
                loanDataMgr = LoanDataMgr.GetLoanDataMgr(Session.SessionObjects, Session.SessionObjects.LoanManager.OpenLoan(strArray[index1].ToString()).GetLoanData(false), false, true);
              }
              catch (Exception ex)
              {
                objArray[3] = (object) "LoanDataMgr object cannot be initialized or loan is not found!";
                if (this.LoanProcessCompleted != null)
                  this.LoanProcessCompleted((object) objArray, new EventArgs());
                this.addLogMsg(objArray);
                ++num1;
                continue;
              }
              try
              {
                LoanData loanData1 = loanDataMgr.LoanData;
                objArray[1] = (object) loanData1.GetField("364");
                objArray[2] = (object) (loanData1.GetField("36") + " " + loanData1.GetField("37"));
                if (this.loanUpdateCondition.UpdateOption == LoanUpdateCondition.UpdateOptions.UpdateOnly)
                {
                  int key = Utils.ParseInt((object) loanData1.GetField("HMDA.X100"));
                  if (key != -1 && loanUpdateCondition.HmdaProfilesLookup.ContainsKey(key))
                  {
                    hmdaProfile = loanUpdateCondition.HmdaProfilesLookup[key];
                    hmdaInformation = new HMDAInformation(hmdaProfile.HMDAProfileSetting);
                  }
                  else
                  {
                    objArray[3] = key != -1 ? (object) ("HMDA Profile ID \"" + (object) key + "\" is not in setting!") : (object) "HMDA Profile is not assigned in loan!";
                    loanData1.Close();
                    loanDataMgr.Close();
                    if (this.LoanProcessCompleted != null)
                      this.LoanProcessCompleted((object) objArray, new EventArgs());
                    this.addLogMsg(objArray);
                    continue;
                  }
                }
                bool flag = false;
                LockInfo lockInfo = (LockInfo) null;
                LockInfo[] currentLocks = loanDataMgr.GetCurrentLocks();
                if (currentLocks != null && currentLocks.Length != 0)
                {
                  for (int index2 = 0; index2 < currentLocks.Length; ++index2)
                  {
                    if (currentLocks[index2] != null && string.Compare(currentLocks[index2].LockedBy, Session.UserID, true) != 0)
                    {
                      flag = true;
                      lockInfo = currentLocks[index2];
                      break;
                    }
                  }
                }
                if (flag)
                {
                  objArray[3] = (object) ("Loan is locked by user \"" + lockInfo.LockedByFirstName + " " + lockInfo.LockedByLastName + "\"!");
                  loanData1.Close();
                  loanDataMgr.Close();
                  if (this.LoanProcessCompleted != null)
                    this.LoanProcessCompleted((object) objArray, new EventArgs());
                  this.addLogMsg(objArray);
                  ++num1;
                  continue;
                }
                stringList1 = new List<string>();
                for (int index3 = 0; index3 < this.updatedFieldIDs.Count; ++index3)
                  stringList1.Add(loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index3]));
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : locking loan...";
                string simpleField = loanDataMgr.LoanData.GetSimpleField("749");
                loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
                loanDataMgr.LoanData.Settings.HMDAInfo = hmdaInformation;
                HMDAInformation hmdaInfo = loanDataMgr.LoanData.Settings.HMDAInfo;
                int num2 = hmdaProfile.HMDAProfileID;
                string str3 = num2.ToString();
                hmdaInfo.HMDAProfileID = str3;
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : updating loan...";
                if (this.loanUpdateCondition.UpdateOption != LoanUpdateCondition.UpdateOptions.UpdateOnly)
                  loanDataMgr.LoanData.SetField("HMDA.X100", string.Concat((object) loanUpdateCondition.NewProfileID));
                if (loanDataMgr.LoanData.Settings != null && loanDataMgr.LoanData.Settings.HMDAInfo != null && loanDataMgr.LoanData.Settings.HMDAInfo.HMDAApplicationDate != null && loanDataMgr.LoanData.Settings.HMDAInfo.HMDAApplicationDate != "")
                  loanDataMgr.LoanData.Calculator.FormCalculation(loanDataMgr.LoanData.Settings.HMDAInfo.HMDAApplicationDate);
                if (StandardFields.GetField("HMDA.X113") != null)
                  loanDataMgr.LoanData.Calculator.FormCalculation("RECALCULATEHMDA");
                loanDataMgr.LoanData.Calculator.FormCalculation("UPDATEHMDA2018");
                loanDataMgr.LoanData.SetField("HMDA.X29", "");
                loanDataMgr.LoanData.Calculator.FormCalculation("1393");
                loanDataMgr.LoanData.Calculator.FormCalculation("4457");
                loanDataMgr.LoanData.SetField("749", simpleField);
                if (simpleField != "" && simpleField != "//")
                {
                  LoanData loanData2 = loanDataMgr.LoanData;
                  num2 = Utils.ParseDate((object) simpleField, false).Year;
                  string val = num2.ToString();
                  loanData2.SetCurrentField("HMDA.X27", val);
                }
                if (loanDataMgr.LoanData.Settings.HMDAInfo.HMDAApplicationDate == "749" && !loanDataMgr.LoanData.IsLocked("HMDA.X29"))
                  loanDataMgr.LoanData.SetCurrentField("HMDA.X29", simpleField == "//" ? "" : simpleField);
                if (!loanDataMgr.LoanData.IsLocked("HMDA.X29"))
                {
                  string field = loanDataMgr.LoanData.GetField("HMDA.X29");
                  string val = this.reformatDate(field);
                  if (field != val)
                    loanDataMgr.LoanData.SetField("HMDA.X29", val);
                }
                loanDataMgr.LoanData.Calculator.FormCalculation("1177");
                loanDataMgr.LoanData.Calculator.FormCalculation("HMDA.X56");
                loanDataMgr.LoanData.Calculator.FormCalculation("4492");
                loanDataMgr.LoanData.SetCurrentField("HMDA.X114", loanDataMgr.LoanData.GetField("1659"));
                loanDataMgr.LoanData.SetCurrentField("HMDA.X115", loanDataMgr.LoanData.GetField("NEWHUD.X6"));
                BorrowerPair[] borrowerPairs = loanDataMgr.LoanData.GetBorrowerPairs();
                for (int index4 = 0; index4 < borrowerPairs.Length; ++index4)
                {
                  loanDataMgr.LoanData.SetBorrowerPair(borrowerPairs[index4]);
                  if (loanDataMgr.LoanData.GetField("HMDA.X116") != "Exempt")
                    loanDataMgr.LoanData.SetCurrentField("HMDA.X116", loanDataMgr.LoanData.GetField("4174"));
                  if (loanDataMgr.LoanData.GetField("HMDA.X117") != "Exempt")
                    loanDataMgr.LoanData.SetCurrentField("HMDA.X117", loanDataMgr.LoanData.GetField("4175"));
                  if (loanDataMgr.LoanData.GetField("HMDA.X118") != "Exempt")
                    loanDataMgr.LoanData.SetCurrentField("HMDA.X118", loanDataMgr.LoanData.GetField("4177"));
                  if (loanDataMgr.LoanData.GetField("HMDA.X119") != "Exempt")
                    loanDataMgr.LoanData.SetCurrentField("HMDA.X119", loanDataMgr.LoanData.GetField("4178"));
                }
                loanDataMgr.LoanData.SetBorrowerPair(borrowerPairs[0]);
                loanDataMgr.LoanData.SetCurrentField("HMDA.X120", loanDataMgr.LoanData.GetField("HMDA.X109"));
                loanDataMgr.LoanData.Calculator.FormCalculation("HMDACREDITSCOREDECISION");
                loanDataMgr.LoanData.SetCurrentField("CALCREQUIRED", "");
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : saving loan...";
                loanDataMgr.Save(false, false);
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : unlocking loan...";
                loanDataMgr.Unlock();
                stringList2 = new List<string>();
                for (int index5 = 0; index5 < this.updatedFieldIDs.Count; ++index5)
                  stringList2.Add(loanDataMgr.LoanData.GetField(this.updatedFieldIDs[index5]));
                loanDataMgr.LoanData.Close();
                if (feedback != null)
                  feedback.Details = str2 + strArray[index1].ToString() + " : loan is " + LoanBatchUpdates.SUCCEEDWORD.ToLower() + ".";
                objArray[3] = (object) LoanBatchUpdates.SUCCEEDWORD;
                streamWriter.WriteLine(loanDataMgr.LoanFolder + " : " + objArray[0].ToString() + " - " + (objArray[1] != null ? objArray[1].ToString() : "") + " : " + (objArray[2] != null ? objArray[2].ToString().Replace(":", "") : "") + " : " + objArray[3].ToString());
              }
              catch (Exception ex)
              {
                if (objArray == null)
                  objArray = new object[4]
                  {
                    (object) strArray[index1].ToString(),
                    null,
                    null,
                    null
                  };
                objArray[3] = (object) ("Error - " + ex.Message);
                ++num1;
              }
              if (this.LoanProcessCompleted != null)
                this.LoanProcessCompleted((object) objArray, new EventArgs());
              this.addLogMsg(objArray);
              if (objArray != null && objArray.Length >= 4 && string.Compare(objArray[3].ToString(), LoanBatchUpdates.SUCCEEDWORD, true) == 0)
              {
                string str4 = "";
                for (int index6 = 0; index6 < this.updatedFieldIDs.Count; ++index6)
                  str4 = str4 + (str4 != "" ? " ~ " : "") + this.updatedFieldIDs[index6] + ":" + stringList1[index6] + "|" + stringList2[index6];
                this.addLogMsg("Detail - " + str4);
              }
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
                return DialogResult.Abort;
              }
            }
          }
          streamWriter.Close();
        }
      }
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
      return DialogResult.OK;
    }

    private void addLogMsg(object[] loanBasicInfo)
    {
      this.addLogMsg(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + " : " + loanBasicInfo[0].ToString() + " - " + (loanBasicInfo[3].ToString() == LoanBatchUpdates.SUCCEEDWORD ? "(" + LoanBatchUpdates.SUCCEEDWORD + ")  " : "(Error)    ") + (loanBasicInfo[1] != null ? loanBasicInfo[1].ToString() : "") + " : " + (loanBasicInfo[2] != null ? loanBasicInfo[2].ToString().Replace(":", "") : "") + (loanBasicInfo[3].ToString() != LoanBatchUpdates.SUCCEEDWORD ? " * " + loanBasicInfo[3].ToString() : ""));
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
      int num1 = -1;
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
                for (int index = 0; index < strArray[0].Length; ++index)
                {
                  if ((string.Compare(strArray[0][index], "Loan Info Loan ID", true) == 0 || string.Compare(strArray[0][index], "GUID", true) == 0) && (strArray[1][index] ?? "") != null && strArray[1][index].StartsWith("{") && strArray[1][index].EndsWith("}") && strArray[1][index].Length == 38)
                  {
                    num1 = index;
                    break;
                  }
                }
              }
            }
          }
        }
      }
      if (num1 == -1)
      {
        string text = "File must be in CSV format and provided csv file must contain Loan GUID in order to update loan data.";
        int num2 = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.None;
      }
      int[] numArray = new int[6];
      numArray[0] = num1;
      int index1 = 1;
      for (int index2 = 0; index2 < 7; ++index2)
      {
        if (index2 != num1)
        {
          numArray[index1] = index2 < strArray[0].Length ? index2 : -1;
          ++index1;
          if (index1 >= 6)
            break;
        }
      }
      List<string> stringList1 = new List<string>();
      for (int index3 = 0; index3 < numArray.Length; ++index3)
      {
        if (numArray[index3] > -1)
          stringList1.Add(strArray[0][numArray[index3]]);
        else
          stringList1.Add("Undefined");
      }
      QueryResult queryResult = new QueryResult(stringList1.ToArray());
      for (int index4 = 1; index4 < strArray.Length; ++index4)
      {
        if (!(strArray[index4][numArray[0]] == "") && strArray[index4][numArray[0]].StartsWith("{") && strArray[index4][numArray[0]].EndsWith("}") && strArray[index4][numArray[0]].Length == 38)
        {
          List<string> stringList2 = new List<string>();
          for (int index5 = 0; index5 < numArray.Length; ++index5)
          {
            if (numArray[index5] > -1)
              stringList2.Add(strArray[index4][numArray[index5]]);
            else
              stringList2.Add("");
          }
          queryResult.AddRow((object[]) stringList2.ToArray());
        }
      }
      return !this.createPendingList(queryResult, queryResult.Columns.ToArray(), feedback) ? DialogResult.None : DialogResult.OK;
    }

    private bool createPendingList(
      QueryResult queryResult,
      string[] selectedFormColumnNames,
      IProgressFeedback feedback)
    {
      if (feedback != null)
        feedback.Details = "Creating process list...";
      string s = "";
      int recordCount = queryResult.RecordCount;
      for (int index = 0; index < recordCount; ++index)
      {
        object[] row = queryResult.GetRow(index);
        s = s + (s != "" ? "|" : "") + row[0].ToString();
      }
      try
      {
        using (FileStream fileStream = new FileStream(this.getPendingFile(), FileMode.Create, FileAccess.Write, FileShare.None))
        {
          byte[] bytes = Encoding.ASCII.GetBytes(s);
          fileStream.Write(bytes, 0, bytes.Length);
          fileStream.Close();
        }
        using (FileStream fileStream = new FileStream(LoanBatchUpdates.PendingListFile_Options, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          byte[] bytes = Encoding.ASCII.GetBytes((this.loanUpdateCondition.UpdateOption == LoanUpdateCondition.UpdateOptions.AssignNewHMDA ? "1" : "3") + "|" + (this.loanUpdateCondition.UpdateOption == LoanUpdateCondition.UpdateOptions.AssignNewHMDA ? string.Concat((object) this.loanUpdateCondition.NewProfileID) : ""));
          fileStream.Write(bytes, 0, bytes.Length);
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog(LoanUpdateToolMainForm.MainScreen, "You do not have access rights to write Process Pending List file to Windows temporary folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return true;
    }

    private string getPendingFile() => LoanBatchUpdates.PendingListFile_AssignNew;

    private string getCompletedListFile() => LoanBatchUpdates.CompletedListFile_AssignNew;

    private string getLogFile()
    {
      return Path.Combine(Environment.GetEnvironmentVariable("TMP"), "HMDAUpdateTool.log");
    }

    public DialogResult ProcessingResult => this.processingResult;

    public bool IsCancelled
    {
      set => this.isCancelled = value;
    }

    private string reformatDate(string originalDate)
    {
      string[] strArray;
      if (originalDate.IndexOf("/") > -1)
      {
        strArray = originalDate.Split('/');
      }
      else
      {
        if (originalDate.IndexOf("-") <= -1)
          return originalDate;
        strArray = originalDate.Split('-');
      }
      if (strArray.Length != 3)
        return originalDate;
      try
      {
        string str1;
        string str2;
        string str3;
        if (strArray[0].Length == 4)
        {
          str1 = strArray[0];
          if (Utils.ParseInt((object) strArray[1]) > 12)
          {
            str2 = strArray[2];
            str3 = strArray[1];
          }
          else
          {
            str2 = strArray[1];
            str3 = strArray[2];
          }
        }
        else
        {
          if (strArray[2].Length != 4)
            return originalDate;
          str1 = strArray[2];
          if (Utils.ParseInt((object) strArray[0]) > 12)
          {
            str2 = strArray[1];
            str3 = strArray[0];
          }
          else
          {
            str2 = strArray[0];
            str3 = strArray[1];
          }
        }
        return str2 + "/" + str3 + "/" + str1;
      }
      catch (Exception ex)
      {
        return originalDate;
      }
    }
  }
}
