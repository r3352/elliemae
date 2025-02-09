// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpPipeline
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Requests.Pipeline;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.MainUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpPipeline : OperationBase, IOpPipeline, IOperation, IDisposable
  {
    private const string className = "OpPipeline";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private string MustSelectLoan = "You must first select a loan from the list.";
    private string MustSelectLoans = "You must first select one or more loans from the list.";

    public OpSimpleResponse CreateNewLoan(OpSimpleRequest request)
    {
      OpSimpleResponse newLoan = new OpSimpleResponse();
      LoanFolderRuleManager bpmManager = (LoanFolderRuleManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.LoanFolder);
      LoanFolderInfo loanFolderInfo = new LoanFolderInfo(request.CommandContext.Session.WorkingFolder);
      if (!bpmManager.GetRule(loanFolderInfo.Name).CanOriginateLoans)
      {
        newLoan.ErrorCode = ErrorCodes.NotAuthorizedToCreateLoan;
        newLoan.ErrorMessage = "You are not authorized to originate loans in the current folder.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, newLoan.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return newLoan;
      }
      Session.Application.GetService<ILoanConsole>().StartNewLoan(loanFolderInfo.Name, true);
      return newLoan;
    }

    public OpSimpleResponse OpenLoanMailbox(OpSimpleRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      Session.Application.GetService<IEncompassApplication>().OpenLoanMailbox();
      return opSimpleResponse;
    }

    public OpFieldDialogResponse SelectField(OpSimpleRequest request)
    {
      OpFieldDialogResponse fieldDialogResponse = new OpFieldDialogResponse();
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector((ReportFieldDefs) MainScreen.Instance.PipelineBrowser.FieldDefs, true, true))
      {
        DialogResult dialogResult = reportFieldSelector.ShowDialog(request.CommandContext.SourceWindow);
        fieldDialogResponse.DialogResult = reportFieldSelector.DialogResult.ToString();
        if (dialogResult != DialogResult.OK)
          return fieldDialogResponse;
        fieldDialogResponse.FieldId = reportFieldSelector.SelectedField.FieldID;
      }
      return fieldDialogResponse;
    }

    public OpSimpleResponse RebuildLoan(OpRebuildLoanRequest request)
    {
      OpFieldDialogResponse fieldDialogResponse = new OpFieldDialogResponse();
      try
      {
        using (CursorActivator.Wait())
          request.CommandContext.Session.LoanManager.RebuildLoan(request.LoanFolder, request.LoanName, DatabaseToRebuild.Both);
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The loan has been rebuilt successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "RebuildLoan Exceptions", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (RebuildLoan), 106);
        fieldDialogResponse.ErrorCode = ErrorCodes.RebuildLoanFailed;
        fieldDialogResponse.ErrorMessage = "The attempt to rebuild this loan has failed due to error (" + ex.Message + "). View the Encompass Server log file for additional information.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, fieldDialogResponse.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return (OpSimpleResponse) fieldDialogResponse;
    }

    public OpSimpleResponse OpenLoan(OpSimpleRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      return !this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoan) ? response : this.OpenLoan(request, response, MainScreen.Instance.PipelineBrowser.ThinPipelineInfos[0].LoanGuid.ToString("B"));
    }

    public OpSimpleResponse OpenLoan(
      OpSimpleRequest request,
      OpSimpleResponse response,
      string guid)
    {
      try
      {
        request.CommandContext.Session.Application.GetService<ILoanConsole>().OpenLoan(guid);
      }
      catch (Exception ex)
      {
        response.ErrorCode = ErrorCodes.CannotOpenLoan;
        response.ErrorMessage = "Error opening loan: " + (object) ex;
        Tracing.Log(OpPipeline.sw, nameof (OpPipeline), TraceLevel.Error, response.ErrorMessage);
      }
      return response;
    }

    public OpSimpleResponse ProcessEPassUrl(OpProcessEPassUrlRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoan))
        return response;
      OpSimpleResponse opSimpleResponse = this.OpenLoan((OpSimpleRequest) request);
      if (opSimpleResponse.ErrorCode != ErrorCodes.None)
        return opSimpleResponse;
      Session.Application.GetService<IEPass>().ProcessURL(request.Url);
      return opSimpleResponse;
    }

    public OpSimpleResponse OpenLoanForm(OpOpenLoanFormRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoan))
        return response;
      OpSimpleResponse opSimpleResponse = this.OpenLoan((OpSimpleRequest) request);
      if (opSimpleResponse.ErrorCode != ErrorCodes.None)
        return opSimpleResponse;
      Session.Application.GetService<ILoanEditor>().OpenForm(request.LoanFormName);
      return opSimpleResponse;
    }

    public OpSimpleResponse ShowLockConfirmation(OpSimpleRequest request)
    {
      OpSimpleResponse response1 = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response1, this.MustSelectLoan))
        return response1;
      OpSimpleResponse response2 = this.OpenLoan(request);
      if (response2.ErrorCode != ErrorCodes.None)
        return response2;
      LockConfirmLog lockConfirmation = this.GetLoanData(request, response2).GetLogList().GetCurrentLockConfirmation();
      Session.Application.GetService<ILoanEditor>().OpenLogRecord((LogRecordBase) lockConfirmation);
      return response2;
    }

    private LoanData GetLoanData(OpSimpleRequest request, OpSimpleResponse response)
    {
      using (CursorActivator.Wait())
      {
        using (ILoan loan = request.CommandContext.Session.LoanManager.OpenLoan(MainScreen.Instance.PipelineBrowser.ThinPipelineInfos[0].LoanGuid.ToString("B")))
        {
          if (loan != null)
            return loan.GetLoanData(false);
          MetricsFactory.IncrementErrorCounter((Exception) null, "The loan has been deleted or is no longer accessible", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (GetLoanData), 200);
          response.ErrorCode = ErrorCodes.LoanNotAccessible;
          response.ErrorMessage = "The loan has been deleted or is no longer accessible";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return (LoanData) null;
        }
      }
    }

    public OpSimpleResponse StartConversation(OpStartConversationRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoan))
        return response;
      OpSimpleResponse opSimpleResponse = this.OpenLoan((OpSimpleRequest) request);
      if (opSimpleResponse.ErrorCode != ErrorCodes.None)
        return opSimpleResponse;
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserID)
      {
        IsEmail = request.IsEmail,
        Phone = request.IsEmail ? string.Empty : request.ContactInfo,
        Email = request.IsEmail ? request.ContactInfo : string.Empty,
        Name = !request.IsBorrower ? Session.LoanData.GetField("68") + " " + Session.LoanData.GetField("69") : Session.LoanData.GetField("36") + " " + Session.LoanData.GetField("37")
      });
      return opSimpleResponse;
    }

    public OpSimpleResponse ExportToExcel(OpExportRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (MainScreen.Instance.PipelineBrowser.CurrentView == null)
      {
        response.ErrorCode = ErrorCodes.ViewNotSet;
        response.ErrorMessage = "Pipeline View is not set.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
        return response;
      }
      TableLayout layout = MainScreen.Instance.PipelineBrowser.CurrentView.Layout;
      if (layout.ColumnCount > ExcelHandler.GetMaximumColumnCount())
      {
        response.ErrorCode = ErrorCodes.MaxColCountExceeded;
        response.ErrorMessage = "You pipeline cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
        return response;
      }
      if (MainScreen.Instance.PipelineBrowser.ThinPipelineInfos.Length > ExcelHandler.GetMaximumRowCount() - 1)
      {
        response.ErrorCode = ErrorCodes.MaxRowCountExceeded;
        response.ErrorMessage = "You pipeline cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
        return response;
      }
      List<string> guids = this.GetGuids(request.ExportAll, (OpSimpleRequest) request, response);
      if (response.ErrorCode != ErrorCodes.None)
        return response;
      PipelineInfo[] pipeline = request.CommandContext.Session.LoanManager.GetPipeline(guids.ToArray(), layout.ColumnTags, PipelineData.Fields, MainScreen.Instance.PipelineBrowser.Flag);
      if (pipeline == null || pipeline.Length == 0)
      {
        MetricsFactory.IncrementErrorCounter((Exception) null, "Data not found", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (ExportToExcel), 283);
        response.ErrorCode = ErrorCodes.DataNotFound;
        response.ErrorMessage = "Data not found";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
        return response;
      }
      ExcelHandler excelHandler = new ExcelHandler();
      IEnumerator enumerator1 = (IEnumerator) layout.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        TableLayout.Column current = (TableLayout.Column) enumerator1.Current;
        excelHandler.AddHeaderColumn(current.Title, (ReportFieldDef) MainScreen.Instance.PipelineBrowser.FieldDefs.GetFieldByCriterionName(current.Tag));
      }
      List<List<string>> dataList = new List<List<string>>();
      foreach (PipelineInfo pinfo in pipeline)
      {
        List<string> stringList = new List<string>();
        IEnumerator enumerator2 = (IEnumerator) layout.GetEnumerator();
        while (enumerator2.MoveNext())
        {
          TableLayout.Column current = (TableLayout.Column) enumerator2.Current;
          string str = pinfo.Info[(object) current.Tag] == null ? string.Empty : pinfo.Info[(object) current.Tag].ToString();
          LoanReportFieldDef fieldByCriterionName = MainScreen.Instance.PipelineBrowser.FieldDefs.GetFieldByCriterionName(current.Tag);
          if (fieldByCriterionName != null)
            str = fieldByCriterionName.ToDisplayElement(current.Tag, pinfo, (Control) null).ToString();
          stringList.Add(str);
        }
        dataList.Add(stringList);
      }
      excelHandler.AddDataTable(dataList);
      excelHandler.CreateExcel();
      return response;
    }

    private List<string> GetGuids(
      bool exportAll,
      OpSimpleRequest request,
      OpSimpleResponse response)
    {
      List<string> guids = new List<string>();
      if (exportAll)
      {
        if (MainScreen.Instance.PipelineBrowser.CurrentView == null)
        {
          response.ErrorCode = ErrorCodes.ViewNotSet;
          response.ErrorMessage = "Pipeline View is not set.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
          return (List<string>) null;
        }
        foreach (PipelineInfo pipelineInfo in new EnumerableCursor(MainScreen.Instance.PipelineBrowser.PipelineCursor, false))
          guids.Add(pipelineInfo.GUID);
      }
      else
        guids = MainScreen.Instance.PipelineBrowser.Guids;
      if (guids.Count != 0)
        return guids;
      response.ErrorCode = ErrorCodes.NoLoanSelected;
      response.ErrorMessage = "Please select a loan to export.";
      int num1 = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return (List<string>) null;
    }

    private PipelineView getPipelineViewForPersonaPipelineView(PersonaPipelineView personaView)
    {
      PipelineView personaPipelineView = new PipelineView(personaView.ToString());
      personaPipelineView.LoanFolder = personaView.LoanFolders;
      personaPipelineView.LoanOwnership = personaView.Ownership;
      personaPipelineView.Filter = personaView.Filter;
      personaPipelineView.PersonaName = personaView.PersonaName;
      TableLayout tableLayout = new TableLayout();
      foreach (PersonaPipelineViewColumn column in personaView.Columns)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) MainScreen.Instance.PipelineBrowser.FieldDefs.GetFieldByCriterionName(column.ColumnDBName);
        if (fieldByCriterionName != null)
        {
          TableLayout.Column tableLayoutColumn = fieldByCriterionName.ToTableLayoutColumn();
          tableLayoutColumn.SortOrder = column.SortOrder;
          if (column.Width >= 0)
            tableLayoutColumn.Width = column.Width;
          tableLayout.AddColumn(tableLayoutColumn);
        }
      }
      personaPipelineView.Layout = tableLayout;
      return personaPipelineView;
    }

    public OpSimpleResponse PrintForms(OpSimpleRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoans))
        return response;
      using (FormSelectorDialog formSelectorDialog = new FormSelectorDialog(request.CommandContext.Session, Array.ConvertAll<ThinPipelineInfo, string>(MainScreen.Instance.PipelineBrowser.ThinPipelineInfos, new Converter<ThinPipelineInfo, string>(OpPipeline.ThinPipelineInfoGuidToString))))
      {
        int num = (int) formSelectorDialog.ShowDialog(request.CommandContext.SourceWindow);
      }
      return response;
    }

    public static string ThinPipelineInfoGuidToString(ThinPipelineInfo thinPipelineInfo)
    {
      return thinPipelineInfo.LoanGuid.ToString("B");
    }

    public OpSimpleResponse SetThinPipelineInfos(OpThinPipelineInfosRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      MainScreen.Instance.PipelineBrowser.ThinPipelineInfos = request.ThinPipelineInfos;
      MainForm.Instance.PipelineSetMenuItemsStates();
      return opSimpleResponse;
    }

    public OpSimpleResponse SetPipelineView(OpPipelineViewRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      if (!MainScreen.Instance.PipelineBrowser.SetSelectedPipelineView(request.PipelineViewName, request.PersonaName))
      {
        opSimpleResponse.ErrorCode = ErrorCodes.ViewNotFound;
        opSimpleResponse.ErrorMessage = "View not found.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, opSimpleResponse.ErrorMessage);
      }
      return opSimpleResponse;
    }

    public OpSimpleResponse TransferLoans(OpSimpleRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoans))
        return response;
      string fileList = "";
      string guidList = "";
      foreach (ThinPipelineInfo thinPipelineInfo in MainScreen.Instance.PipelineBrowser.ThinPipelineInfos)
      {
        if (fileList == "")
        {
          fileList = thinPipelineInfo.LoanName;
          guidList = thinPipelineInfo.LoanGuid.ToString("B");
        }
        else
        {
          fileList = fileList + "|" + thinPipelineInfo.LoanName;
          guidList = guidList + "|" + thinPipelineInfo.LoanGuid.ToString("B");
        }
      }
      using (TransferDialog transferDialog = new TransferDialog(fileList, guidList))
      {
        int num = (int) transferDialog.ShowDialog(request.CommandContext.SourceWindow);
      }
      return response;
    }

    public OpSimpleResponse NotifyUsers(OpSimpleRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      if (!this.LoansSelected((RequestBase) request, (ResponseBase) response, this.MustSelectLoans))
        return response;
      List<LoanDisplayInfo> loanInfo = new List<LoanDisplayInfo>();
      foreach (ThinPipelineInfo thinPipelineInfo in MainScreen.Instance.PipelineBrowser.ThinPipelineInfos)
      {
        LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo();
        loanDisplayInfo.LoanNumber = thinPipelineInfo.LoanNumber;
        loanDisplayInfo.LoanGuid = thinPipelineInfo.LoanGuid;
        if (thinPipelineInfo.BorrowerName != null)
          loanDisplayInfo.BorrowerName = thinPipelineInfo.BorrowerName;
        loanDisplayInfo.LoanAmount = thinPipelineInfo.LoanAmount;
        loanInfo.Add(loanDisplayInfo);
      }
      using (NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(loanInfo))
      {
        int num = (int) notifyUsersDialog.ShowDialog(request.CommandContext.SourceWindow);
      }
      return response;
    }

    public OpSimpleResponse DuplicateLoan(OpDuplicateLoanRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        LoanDataMgr sourceMgr = LoanDataMgr.OpenLoan(request.CommandContext.Session.SessionObjects, MainScreen.Instance.PipelineBrowser.ThinPipelineInfos[0].LoanGuid.ToString("B"), false);
        if (request.IsPiggyback)
        {
          if (sourceMgr.IsLoanLocked())
          {
            response.ErrorCode = ErrorCodes.LoanLocked;
            response.ErrorMessage = "The selected loan is locked by another user. Linking to a Piggyback requires full access rights to the selected loan.";
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return response;
          }
          if (sourceMgr.LinkedLoan != null && Utils.Dialog(request.CommandContext.SourceWindow, "This loan currently is linking to another loan. Do you want to remove current link?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          {
            response.ErrorCode = ErrorCodes.LoanLinked;
            response.ErrorMessage = "This loan currently is linking to another loan. Do you want to remove current link?";
            return response;
          }
        }
        LoanData loanData = sourceMgr.LoanData;
        if (sourceMgr.LoanName.Replace(".", "") == "")
        {
          response.ErrorCode = ErrorCodes.InvalidName;
          response.ErrorMessage = "An error occurred while attempting to copy the loan '" + loanData.ToPipelineInfo().LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return response;
        }
        if (request.IsSecond)
          loanData.ChangeLoanToSecond();
        LoanDataMgr loanDataMgr = LoanDataMgr.CopyLoan(Session.SessionObjects, loanData, request.DestinationFolder, sourceMgr.LoanName, request.TemplateToUse);
        LoanServiceManager.DuplicateLoan(loanDataMgr, request.TemplateToUse);
        LoanServiceManager.DuplicateLoan(loanDataMgr);
        string empty = string.Empty;
        string guid;
        if (request.IsPiggyback)
        {
          this.linkToPiggybackLoan((OpSimpleRequest) request, response, sourceMgr, loanDataMgr);
          if (response.ErrorCode != ErrorCodes.None)
            return response;
          guid = loanDataMgr.LoanData.GUID;
        }
        else
        {
          sourceMgr.Close();
          loanDataMgr.Save(false);
          guid = loanDataMgr.LoanData.GUID;
        }
        sourceMgr.Close();
        loanDataMgr.Close();
        if (Utils.Dialog(request.CommandContext.SourceWindow, "The loan has been successfully duplicated. Would you like to open the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.OpenLoan((OpSimpleRequest) request, response, guid);
        return response;
      }
      catch (Exception ex)
      {
        Tracing.Log(OpPipeline.sw, TraceLevel.Error, nameof (OpPipeline), "Error duplicating loan: " + (object) ex);
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "Error duplicating loan: " + ex.Message);
        return response;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private OpSimpleResponse linkToPiggybackLoan(
      OpSimpleRequest request,
      OpSimpleResponse response,
      LoanDataMgr sourceMgr,
      LoanDataMgr targetMgr)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        sourceMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The selected loan cannot be linked", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (linkToPiggybackLoan), 605);
        response.ErrorCode = ErrorCodes.LoanCannotBeLinked;
        response.ErrorMessage = "The selected loan cannot be linked due to this error: " + ex.Message;
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return response;
      }
      if (sourceMgr.LinkedLoan != null)
      {
        try
        {
          sourceMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", "");
          try
          {
            sourceMgr.LinkedLoan.Save();
            sourceMgr.Unlink();
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "PiggybackInfoCannotBeRemoved", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (linkToPiggybackLoan), 625);
            response.ErrorCode = ErrorCodes.PiggybackInfoCannotBeRemoved;
            response.ErrorMessage = "The existing piggyback loan information in source file cannot be removed due to this error: " + ex.Message;
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
            return response;
          }
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "PiggybackInfoCannotBeRemoved", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (linkToPiggybackLoan), 634);
          response.ErrorCode = ErrorCodes.PiggybackInfoCannotBeRemoved;
          response.ErrorMessage = "The existing piggyback loan information in source file cannot be removed due to this error: " + ex.Message;
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
          return response;
        }
      }
      targetMgr.LoanData.SetCurrentField("LINKGUID", sourceMgr.LoanData.GUID);
      Tracing.Log(OpPipeline.sw, TraceLevel.Info, nameof (OpPipeline), "trying to link the newly created linked loan.");
      sourceMgr.LoanData.SetCurrentField("LINKGUID", targetMgr.LoanData.GUID);
      try
      {
        targetMgr.LinkTo(sourceMgr);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "Can not link the new Piggyback loan", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpPipeline.cs", nameof (linkToPiggybackLoan), 654);
        response.ErrorCode = ErrorCodes.LoanCannotBeLinked;
        response.ErrorMessage = "Can not link the new Piggyback loan.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(OpPipeline.sw, TraceLevel.Info, nameof (OpPipeline), "trying to link the newly created linked loan. Error: " + ex.Message);
        return response;
      }
      if (sourceMgr.LoanData.Calculator != null)
      {
        sourceMgr.LoanData.Calculator.SkipLockRequestSync = true;
        if (sourceMgr.LoanData.LinkedData != null && sourceMgr.LoanData.LinkedData.Calculator != null)
          sourceMgr.LoanData.LinkedData.Calculator.SkipLockRequestSync = true;
      }
      sourceMgr.LoanData.SyncPiggyBackFiles((string[]) null, true, true, (string) null, (string) null, false);
      MetricsFactory.IncrementCounter("LoanSaveIncCounter", (SFxTag) new SFxInternalTag());
      using (MetricsFactory.GetIncrementalTimer("LoanSaveIncTimer", (SFxTag) new SFxInternalTag()))
        sourceMgr.SaveLoan(false, (ILoanMilestoneTemplateOrchestrator) null, false);
      sourceMgr.ReleaseExclusiveALock();
      Cursor.Current = Cursors.Default;
      return response;
    }

    private bool LoansSelected(RequestBase request, ResponseBase response, string errorMessage)
    {
      if (MainScreen.Instance.PipelineBrowser.ThinPipelineInfos.Length != 0)
        return true;
      int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      response.ErrorMessage = errorMessage;
      response.ErrorCode = ErrorCodes.NoLoanSelected;
      return false;
    }

    public OpDialogResponse eFolderExport(OpExportRequest request)
    {
      OpDialogResponse response = new OpDialogResponse();
      try
      {
        if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        {
          response.ErrorCode = ErrorCodes.ModuleNotAvailable;
          response.ErrorMessage = "EDM Module is not available for current user";
          return response;
        }
        List<string> guids = this.GetGuids(request.ExportAll, (OpSimpleRequest) request, (OpSimpleResponse) response);
        if (response.ErrorCode != ErrorCodes.None)
          return response;
        if (Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate, FileSystemEntry.PublicRoot).Length == 0)
        {
          response.ErrorCode = ErrorCodes.NoDocumentExportTemplateSetUp;
          response.ErrorMessage = "At least one Document Export Template must be set up before you can export documents.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
          return response;
        }
        string companySetting = Session.ConfigurationManager.GetCompanySetting("ExportTemplate", "Default");
        FileSystemEntry defaultEntry;
        try
        {
          defaultEntry = FileSystemEntry.Parse(companySetting);
        }
        catch (Exception ex)
        {
          response.ErrorCode = ErrorCodes.NoDefaultDocumentExportTemplateDesignated;
          response.ErrorMessage = "One Document Export Template must be designated as Default before you can export documents.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
          Tracing.Log(OpPipeline.sw, nameof (OpPipeline), TraceLevel.Error, "Error retrieving Default Export Template: " + (object) ex);
          return response;
        }
        if (BackgroundAttachmentDialog.IsProcessing(guids.ToArray()))
        {
          response.ErrorCode = ErrorCodes.FilesNotAttachedYet;
          response.ErrorMessage = "Selected loans have files waiting to be attached.\r\nYou cannot export until the files have been attached.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
          return response;
        }
        using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(LoanDataMgr.OpenLoan(Session.SessionObjects, guids[0], false), guids.ToArray(), defaultEntry))
        {
          DialogResult dialogResult = selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          response.DialogResult = dialogResult.ToString();
          if (dialogResult == DialogResult.Cancel)
            return response;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(OpPipeline.sw, nameof (OpPipeline), TraceLevel.Error, "Error exporting documents: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      return response;
    }

    public OpSimpleResponse ExportLEFPipeline(OpExportRequest request)
    {
      OpSimpleResponse response = new OpSimpleResponse();
      using (CursorActivator.Wait())
      {
        List<string> guids = this.GetGuids(request.ExportAll, (OpSimpleRequest) request, response);
        if (response.ErrorCode != ErrorCodes.None)
          return response;
        new ExportLEFData().Export(Session.LoanDataMgr, guids.ToArray());
      }
      return response;
    }

    public OpSimpleResponse GenerateNMLSReport(OpSimpleRequest request)
    {
      OpSimpleResponse nmlsReport = new OpSimpleResponse();
      if (!NMLSReportForm.ValidateRequiredFields(NMLSReportType.Standard))
      {
        nmlsReport.ErrorCode = ErrorCodes.FieldValidationFailed;
        nmlsReport.ErrorMessage = "Field Validation failed.";
        return nmlsReport;
      }
      using (NMLSReportForm nmlsReportForm = new NMLSReportForm())
      {
        int num = (int) nmlsReportForm.ShowDialog(request.CommandContext.SourceWindow);
      }
      return nmlsReport;
    }

    public OpSimpleResponse GenerateNCMLDReport(OpSimpleRequest request)
    {
      OpSimpleResponse ncmldReport = new OpSimpleResponse();
      if (!NCMLDReportForm.ValidateRequiredFields())
      {
        ncmldReport.ErrorCode = ErrorCodes.FieldValidationFailed;
        ncmldReport.ErrorMessage = "Field Validation failed.";
        return ncmldReport;
      }
      using (NCMLDReportForm ncmldReportForm = new NCMLDReportForm())
      {
        int num = (int) ncmldReportForm.ShowDialog(request.CommandContext.SourceWindow);
      }
      return ncmldReport;
    }

    public OpSimpleResponse CreateAppointment(OpContactGuidRequest request)
    {
      OpSimpleResponse appointment = new OpSimpleResponse();
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(request.ContactGuid.ToString("B"));
      if (borrower == null)
      {
        appointment.ErrorCode = ErrorCodes.ContactRecordNotFound;
        appointment.ErrorMessage = "The linked contact record could not be found and may have been deleted. You will need to re-link the borrower to the correct contact to use this feature.";
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, appointment.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return appointment;
      }
      ContactInfo contactInfo = new ContactInfo(borrower.FullName, borrower.ContactID.ToString(), CategoryType.Borrower);
      Session.Application.GetService<ICalendar>().AddAppointment(new ContactInfo[1]
      {
        contactInfo
      });
      return appointment;
    }

    public OpSimpleResponse ImportLoans(OpSimpleRequest request)
    {
      MainScreen.Instance.PipelineBrowser.ImportLoans();
      return new OpSimpleResponse();
    }

    public OpDialogResponse InvestorStandardExportPipeline(OpServiceRequest request)
    {
      ServiceSetting serviceSetting = MainScreen.Instance.PipelineBrowser.GetServiceSetting(request.CategoryName, request.DataServiceId);
      serviceSetting.Tag = request.ExportAll ? (object) "All" : (object) "Selected";
      OpDialogResponse response = new OpDialogResponse();
      using (CursorActivator.Wait())
      {
        List<string> guids = this.GetGuids(serviceSetting.Tag.Equals((object) "All"), (OpSimpleRequest) request, (OpSimpleResponse) response);
        if (response.ErrorCode != ErrorCodes.None)
          return response;
        if (Session.LoanData != null)
        {
          response.ErrorCode = ErrorCodes.LoanFilesOpen;
          response.ErrorMessage = "Please exit all loan files before exporting.";
          int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return response;
        }
        bool flag = false;
        ILoanServices service = Session.Application.GetService<ILoanServices>();
        if (serviceSetting.UseStandardValidationGrid)
        {
          if (MainScreen.Instance.PipelineBrowser.CurrentView == null)
          {
            response.ErrorCode = ErrorCodes.ViewNotSet;
            response.ErrorMessage = "Pipeline View is not set.";
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, response.ErrorMessage);
            return response;
          }
          TableLayout layout = MainScreen.Instance.PipelineBrowser.CurrentView.Layout;
          InvestorExportDialog.Instance.Initialize(MainScreen.Instance.PipelineBrowser.FieldDefs, guids.ToArray(), layout, serviceSetting);
          if (!InvestorExportDialog.PassDataCheck && DialogResult.Cancel == InvestorExportDialog.Instance.ShowDialog(request.CommandContext.SourceWindow))
          {
            response.DialogResult = DialogResult.Cancel.ToString();
            return response;
          }
          flag = service.ExportServiceExportData(Session.LoanDataMgr, serviceSetting, guids.ToArray());
        }
        else
          flag = service.ExportServiceProcessLoans(Session.LoanDataMgr, serviceSetting, guids.ToArray());
        return response;
      }
    }

    public OpDialogResponse ExportFannieMaeFormattedFile(OpSimpleRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      try
      {
        string path1 = string.Empty;
        string str1 = string.Empty;
        using (ExportToLocal exportToLocal = new ExportToLocal())
        {
          DialogResult dialogResult;
          if ((dialogResult = exportToLocal.ShowDialog((IWin32Window) Session.MainScreen)) != DialogResult.OK)
          {
            opDialogResponse.DialogResult = dialogResult.ToString();
            return opDialogResponse;
          }
          path1 = exportToLocal.folder;
          str1 = exportToLocal.fileName;
        }
        if (str1.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || str1 == string.Empty)
        {
          opDialogResponse.ErrorCode = ErrorCodes.InvalidFileName;
          opDialogResponse.ErrorMessage = "Invalid file name.";
          int num = (int) MessageBox.Show(opDialogResponse.ErrorMessage, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return opDialogResponse;
        }
        string str2 = str1 + ".fnm";
        if (!Directory.Exists(path1))
        {
          opDialogResponse.ErrorCode = ErrorCodes.InvalidFolderPath;
          opDialogResponse.ErrorMessage = "Invalid folder path.";
          int num = (int) MessageBox.Show(opDialogResponse.ErrorMessage, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return opDialogResponse;
        }
        foreach (ThinPipelineInfo thinPipelineInfo in MainScreen.Instance.PipelineBrowser.ThinPipelineInfos)
        {
          LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo()
          {
            LoanNumber = thinPipelineInfo.LoanNumber,
            LoanGuid = thinPipelineInfo.LoanGuid
          };
          LoanDataMgr LoanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, thinPipelineInfo.LoanGuid.ToString("B"), false);
          LoanData loanData = LoanDataMgr.LoanData;
          if (!this.Validate("FNMA32", true, loanData))
          {
            opDialogResponse.ErrorCode = ErrorCodes.ValidationFailed;
            opDialogResponse.ErrorMessage = "Validation failed.";
            return opDialogResponse;
          }
          string s = this.Export(LoanDataMgr, loanData, "FNMA32", false);
          if (s != "")
          {
            string path2 = path1 + "\\" + str2;
            if (File.Exists(path2))
            {
              DialogResult dialogResult = MessageBox.Show("File already exists. Would you like to replace it?", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
              if (dialogResult == DialogResult.Cancel)
              {
                opDialogResponse.DialogResult = dialogResult.ToString();
                return opDialogResponse;
              }
            }
            FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            int num = (int) MessageBox.Show("File exported. The file " + path2 + " has been created.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return opDialogResponse;
    }

    public bool Validate(string format, bool allowContinue, LoanData loanData)
    {
      try
      {
        string empty = string.Empty;
        string str = !(format == "LEF") ? SystemSettings.EpassDataDir + "Export.Validate.dll" : SystemSettings.EpassDataDir + "Export.LEF.dll";
        if (!File.Exists(str))
        {
          str = AssemblyResolver.GetResourceFileFolderPath("EMN\\Export.Validate.dll");
          if (!File.Exists(str))
            return false;
        }
        string fullName1 = AssemblyName.GetAssemblyName(str).FullName;
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == fullName1)
            assembly1 = assembly2;
        }
        if (assembly1 == (Assembly) null)
        {
          FileStream fileStream = File.OpenRead(str);
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, numArray.Length);
          fileStream.Close();
          assembly1 = Assembly.Load(numArray);
        }
        string fullName2 = typeof (IValidate).FullName;
        string typeName = (string) null;
        foreach (System.Type type in assembly1.GetTypes())
        {
          if (type.GetInterface(fullName2) != (System.Type) null)
            typeName = type.FullName;
        }
        IValidate instance = (IValidate) assembly1.CreateInstance(typeName);
        instance.Bam = (EllieMae.EMLite.Export.IBam) new EllieMae.EMLite.Export.Bam(loanData);
        return instance.ValidateData(format, allowContinue);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public string Export(
      LoanDataMgr LoanDataMgr,
      LoanData loanData,
      string format,
      bool currentBorPairOnly)
    {
      try
      {
        ExportData exportData = new ExportData(LoanDataMgr, loanData);
        if (string.Compare(format, "EDRS", true) == 0)
          return loanData.ToXml(true);
        string str1 = OpPipeline.GetExportAssemblyPath(format);
        Tracing.Log(Tracing.SwImportExport, "ExportData", TraceLevel.Verbose, "Export assembly file: " + str1);
        if (!File.Exists(str1))
        {
          str1 = AssemblyResolver.GetResourceFileFolderPath("EMN\\Export.Fannie32.dll");
          if (!File.Exists(str1))
            return string.Empty;
        }
        string fullName1 = AssemblyName.GetAssemblyName(str1).FullName;
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == fullName1)
            assembly1 = assembly2;
        }
        if (assembly1 == (Assembly) null)
        {
          FileStream fileStream = File.OpenRead(str1);
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, numArray.Length);
          fileStream.Close();
          assembly1 = Assembly.Load(numArray);
        }
        string fullName2 = typeof (IExport).FullName;
        string typeName = (string) null;
        foreach (System.Type type in assembly1.GetTypes())
        {
          if (type.GetInterface(fullName2) != (System.Type) null)
            typeName = type.FullName;
        }
        IExport instance = (IExport) assembly1.CreateInstance(typeName);
        instance.Bam = (EllieMae.EMLite.Export.IBam) new EllieMae.EMLite.Export.Bam(loanData);
        string empty = string.Empty;
        string str2;
        if (currentBorPairOnly)
        {
          MethodInfo method = instance.GetType().GetMethod("ExportData", new System.Type[1]
          {
            typeof (bool)
          });
          str2 = !(method == (MethodInfo) null) ? string.Concat(method.Invoke((object) instance, new object[1]
          {
            (object) true
          })) : throw new NotSupportedException("The export format '" + format + "' does not support export of the current borrower pair only");
        }
        else
          str2 = this.exportDataMethodInvoke(instance);
        return str2;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return "";
      }
    }

    protected string exportDataMethodInvoke(IExport export) => export.ExportData();

    public static string GetExportAssemblyPath(string format)
    {
      if (string.Compare(format, "EDRS", true) == 0)
        return (string) null;
      switch (format.ToUpper())
      {
        case "EMXML":
          format = "EMXML10";
          break;
        case "FNMA30":
          format = "FANNIE30";
          break;
        case "FNMA32":
          format = "FANNIE32";
          break;
        case "FANNIE":
          format = "LOANDELIVERY";
          break;
        case "FREDDIE":
          format = "LOANDELIVERY";
          break;
        case "GINNIE":
          format = "GnmaPdd12";
          break;
      }
      return SystemSettings.EpassDataDir + "Export." + format + ".dll";
    }
  }
}
