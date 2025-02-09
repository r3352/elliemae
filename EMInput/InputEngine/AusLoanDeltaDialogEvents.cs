// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AusLoanDeltaDialogEvents
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanDataDelta;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AusLoanDeltaDialogEvents
  {
    private readonly AUSTrackingTool _ausTrackingTool;
    private LoanDeltaDialog _loanDataDeltaDialog;
    public static List<string> NonDataFieldList = new List<string>()
    {
      "AUS.X124",
      "AUS.X4",
      "AUS.X5",
      "AUS.X116",
      "AUS.X131",
      "AUS.X174",
      "AUS.X170",
      "AUS.X9",
      "AUS.X168",
      "AUS.X173",
      "AUS.X165",
      "AUS.X8",
      "AUS.X169",
      "AUS.EFOLDERGUID",
      "AUS.X3",
      "AUS.X114"
    };

    public static Dictionary<string, string> AusFieldMapIdDictionary { get; set; }

    public string FirstColumnDate { get; set; }

    public string SecondColumnDate { get; set; }

    public List<AUSTrackingHistoryLog> FirsthistoryLog { get; set; }

    public List<AUSTrackingHistoryLog> SecondhistoryLog { get; set; }

    internal AusLoanDeltaDialogEvents(AUSTrackingTool ausTrackingTool)
    {
      this._ausTrackingTool = ausTrackingTool;
    }

    internal void ShowLoanDataDeltaDifferences()
    {
      if (this.ActivateExistingLoanDeltaDialog())
        return;
      this._ausTrackingTool.Cursor = Cursors.WaitCursor;
      try
      {
        Application.DoEvents();
        string windowTitle = "Reports Comparison";
        Func<List<LoanDeltaItem>> getData = new Func<List<LoanDeltaItem>>(this.GetDataDeltaDifferences);
        this.GetDatesForColumnHeaders();
        this._loanDataDeltaDialog = new LoanDeltaDialog(getData, windowTitle, new Func<List<string>>(this.GetDatesForColumnHeadersFunc));
        this._loanDataDeltaDialog.FormClosing += new FormClosingEventHandler(this.ClearLoanDeltaDialog);
        if (this._loanDataDeltaDialog.NumberOfDiffs > 0)
          this._loanDataDeltaDialog.Show();
        else
          this.ClearLoanDeltaDialog();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) Form.ActiveForm, string.Format("One or both of the selected history records contains invalid data. Data compare window cannot display with invalid data."), "Loan Data Delta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        Tracing.Log(true, TraceLevel.Error.ToString(), "AusLoanDeltaDialogEvents.cs.ShowLoanDataDeltaDifferences", ex.ToString());
        this.ClearLoanDeltaDialog();
      }
      finally
      {
        this._ausTrackingTool.Cursor = Cursors.Default;
      }
    }

    private void GetDatesForColumnHeaders()
    {
      if (this.GetSelectedRows())
        return;
      this.FirstColumnDate = this.FirsthistoryLog.First<AUSTrackingHistoryLog>().SubmissionDateTime;
      this.SecondColumnDate = this.SecondhistoryLog.First<AUSTrackingHistoryLog>().SubmissionDateTime;
    }

    private List<string> GetDatesForColumnHeadersFunc()
    {
      List<string> columnHeadersFunc = new List<string>();
      if (this.GetSelectedRows())
        return columnHeadersFunc;
      columnHeadersFunc.Add(this.FirsthistoryLog.First<AUSTrackingHistoryLog>().SubmissionDateTime);
      columnHeadersFunc.Add(this.SecondhistoryLog.First<AUSTrackingHistoryLog>().SubmissionDateTime);
      return columnHeadersFunc;
    }

    private bool GetSelectedRows()
    {
      GVSelectedItemCollection selectedhistoryCollection = this._ausTrackingTool.SelectedhistoryCollection;
      if (selectedhistoryCollection == null)
        return true;
      this.FirsthistoryLog = selectedhistoryCollection.Select<GVItem, AUSTrackingHistoryLog>((Func<GVItem, AUSTrackingHistoryLog>) (selected => (AUSTrackingHistoryLog) this._ausTrackingTool.SelectedhistoryCollection[0].Tag)).ToList<AUSTrackingHistoryLog>();
      this.SecondhistoryLog = selectedhistoryCollection.Select<GVItem, AUSTrackingHistoryLog>((Func<GVItem, AUSTrackingHistoryLog>) (selected => (AUSTrackingHistoryLog) this._ausTrackingTool.SelectedhistoryCollection[1].Tag)).ToList<AUSTrackingHistoryLog>();
      return false;
    }

    private void ClearLoanDeltaDialog() => this._loanDataDeltaDialog = (LoanDeltaDialog) null;

    private void ClearLoanDeltaDialog(object sender, FormClosingEventArgs e)
    {
      this.ClearLoanDeltaDialog();
    }

    private List<LoanDeltaItem> GetDataDeltaDifferences()
    {
      List<LoanDeltaItem> deltaDifferences = new List<LoanDeltaItem>();
      try
      {
        if (this.GetSelectedRows())
          return deltaDifferences;
        AUSTrackingHistoryLog trackingHistoryLog1 = this.FirsthistoryLog.FirstOrDefault<AUSTrackingHistoryLog>();
        if (trackingHistoryLog1 == null)
          return deltaDifferences;
        AUSTrackingHistoryLog trackingHistoryLog2 = this.FirsthistoryLog.FirstOrDefault<AUSTrackingHistoryLog>();
        if (trackingHistoryLog2 == null)
          return deltaDifferences;
        string[] assignedFieldIds1 = trackingHistoryLog1.DataValues.GetAssignedFieldIDs();
        string[] assignedFieldIds2 = trackingHistoryLog2.DataValues.GetAssignedFieldIDs();
        foreach (string id in ((IEnumerable<string>) assignedFieldIds1).Count<string>() > ((IEnumerable<string>) assignedFieldIds2).Count<string>() ? assignedFieldIds1 : assignedFieldIds2)
        {
          if (!AusLoanDeltaDialogEvents.NonDataFieldList.Contains(id))
          {
            LoanDeltaItem loanDeltaItem1 = new LoanDeltaItem();
            loanDeltaItem1.FieldId = id;
            LoanDeltaItem loanDeltaItem2 = loanDeltaItem1;
            loanDeltaItem2.FieldDescription = this.FirsthistoryLog.First<AUSTrackingHistoryLog>().DataValues.GetFieldDefinition(loanDeltaItem2.FieldId).Description;
            loanDeltaItem2.Value = this.FirsthistoryLog.First<AUSTrackingHistoryLog>().DataValues.GetField(id);
            AUSTrackingHistoryLog trackingHistoryLog3 = this.SecondhistoryLog.FirstOrDefault<AUSTrackingHistoryLog>();
            if (trackingHistoryLog3 != null)
              loanDeltaItem2.SnapshotValue = trackingHistoryLog3.GetField(id);
            if (!string.Equals(loanDeltaItem2.Value, loanDeltaItem2.SnapshotValue))
              deltaDifferences.Add(loanDeltaItem2);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), "AUSTrackingTool.cs.GetDataDeltaDifferences", ex.ToString());
      }
      return deltaDifferences;
    }

    private bool ActivateExistingLoanDeltaDialog()
    {
      if (this._loanDataDeltaDialog != null)
      {
        if (this._loanDataDeltaDialog.WindowState == FormWindowState.Minimized)
          this._loanDataDeltaDialog.WindowState = FormWindowState.Normal;
        this._loanDataDeltaDialog.Activate();
        return true;
      }
      foreach (Form form in ((IEnumerable<Form>) Application.OpenForms.Cast<Form>().ToArray<Form>()).Where<Form>((Func<Form, bool>) (f => f.Name == "LoanDataDeltaDialog")))
        form.Close();
      return false;
    }
  }
}
