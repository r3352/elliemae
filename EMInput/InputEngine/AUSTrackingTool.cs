// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AUSTrackingTool
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DeepLinking;
using EllieMae.EMLite.Common.DeepLinking.Context;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AUSTrackingTool : UserControl, IRefreshContents, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private LoanData loan;
    private InputFormInfo mainFormInfo = new InputFormInfo("ATR_TrackingDetails", "ATR_TrackingDetails");
    private const string MAINFORMNAME = "AUS Details";
    private const int MAINFORMNAMEWIDTH = 600;
    private const int MAINFORMNAMEHEIGHT = 750;
    private AUSTrackingHistoryLog selectedTrackingLog;
    internal AusLoanDeltaDialogEvents LoanDeltaDialogEvents;
    private GridView _selectedTabGridView;
    private System.Windows.Forms.Button _compareButton;
    private IContainer components;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox6;
    private System.Windows.Forms.TextBox textBox5;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Panel panelForm;
    private GroupContainer gpcHistoryList;
    internal GridView gridViewUnderwriting;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.TextBox textBox13;
    private System.Windows.Forms.TextBox textBox23;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.TextBox textBox24;
    private System.Windows.Forms.TextBox textBox25;
    private System.Windows.Forms.TextBox textBox26;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Label label29;
    private System.Windows.Forms.TextBox textBox27;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.TextBox textBox36;
    private System.Windows.Forms.Label label41;
    private System.Windows.Forms.Button btnCopy;
    internal StandardIconButton stdIconView;
    internal StandardIconButton stdIconEdit;
    internal StandardIconButton stdIconNew;
    private ToolTip toolTip1;
    private System.Windows.Forms.TextBox textBox9;
    private System.Windows.Forms.TextBox textBox7;
    private System.Windows.Forms.TextBox textBox8;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox11;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox textBox10;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button btnRequestUnderwriting;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnCompareLPAReports;
    private System.Windows.Forms.Label labelCompareText;
    internal TabControl tabControl_history;
    private TabPage tabPageUnderwriting;
    private TabPage tabPageLoanQualityAdvisor;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label labelLQACompareText;
    private System.Windows.Forms.Button btnCompareLQAReports;
    internal GridView gridViewLQAResults;
    private System.Windows.Forms.Button btnOpenUwCenterInWeb;

    public GVSelectedItemCollection SelectedhistoryCollection { get; internal set; }

    public AUSTrackingTool(Sessions.Session session, LoanData loan)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.InitForm();
      this.LoanDeltaDialogEvents = new AusLoanDeltaDialogEvents(this);
    }

    private void InitForm()
    {
      this.gridViewUnderwriting.BeginUpdate();
      this.gridViewUnderwriting.Items.Clear();
      this.gridViewLQAResults.BeginUpdate();
      this.gridViewLQAResults.Items.Clear();
      AUSTrackingHistoryList trackingHistoryList = this.loan.GetAUSTrackingHistoryList();
      if (trackingHistoryList != null && trackingHistoryList.HistoryCount > 0)
      {
        for (int i = 0; i < trackingHistoryList.HistoryCount; ++i)
        {
          if (trackingHistoryList.GetHistoryAt(i).SubmissionType == "LQA")
            this.gridViewLQAResults.Items.Add(this.BuildSnapshotItemLqa(trackingHistoryList.GetHistoryAt(i), false));
          else
            this.gridViewUnderwriting.Items.Add(this.BuildSnapshotItem(trackingHistoryList.GetHistoryAt(i), false));
        }
      }
      this.RefreshRecordCount();
      this.gridViewUnderwriting.EndUpdate();
      this.gridViewUnderwriting.AllowMultiselect = true;
      this.btnCompareLPAReports.Enabled = false;
      this.gridViewLQAResults.EndUpdate();
      this.gridViewLQAResults.AllowMultiselect = true;
      this.btnCompareLQAReports.Enabled = false;
      this._selectedTabGridView = this.gridViewUnderwriting;
      this._compareButton = this.btnCompareLPAReports;
      this.ApplySecurity();
      this.GridViewSelectedIndexChanged(this.gridViewUnderwriting, this.btnCompareLPAReports);
      this.GridViewSelectedIndexChanged(this.gridViewLQAResults, this.btnCompareLQAReports);
      this.RefreshLatestSubmissionSection();
      try
      {
        ResourceManager resources = new ResourceManager(typeof (MIPDialog));
        new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), this.session).SetButtonAccessMode(this.btnCopy, "copytotransmittalsummary");
      }
      catch (Exception ex)
      {
      }
    }

    private void ApplySecurity()
    {
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      FeaturesAclManager aclManager1 = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.stdIconNew.Visible = this.tabControl_history.SelectedTab == this.tabPageUnderwriting && aclManager1.GetUserApplicationRight(AclFeature.ToolsTab_AUSTrackingManual);
      FeatureConfigsAclManager aclManager2 = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.btnOpenUwCenterInWeb.Visible = this.session.EncompassEdition != EncompassEdition.Broker && aclManager2.GetUserApplicationRight(AclFeature.PlatForm_Access) > 0 && aclManager1.GetUserApplicationRight(AclFeature.LOConnectTab_UnderwritingCenter);
    }

    private void stdIconView_Click(object sender, EventArgs e)
    {
      if (this.tabControl_history.SelectedTab == this.tabPageLoanQualityAdvisor)
        this.ViewHistoryRecord(this.gridViewLQAResults);
      else
        this.ViewHistoryRecord(this.gridViewUnderwriting);
    }

    private void ViewHistoryRecord(GridView historyGridView)
    {
      if (historyGridView.SelectedItems.Count < 1)
        return;
      AUSTrackingHistoryLog tag = (AUSTrackingHistoryLog) historyGridView.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The history is invalid!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        tag.DataValues.ReadOnly = true;
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) tag.DataValues, "AUS Details", this.mainFormInfo, 600, 750, FieldSource.CurrentLoan, "", this.session))
        {
          entryPopupDialog.SetFieldsReadonly();
          int num2 = (int) entryPopupDialog.ShowDialog((IWin32Window) this);
          for (int index = 0; index < historyGridView.Items.Count; ++index)
          {
            if (string.Compare(((AUSTrackingHistoryLog) historyGridView.Items[index].Tag).HistoryID, tag.HistoryID, true) == 0)
            {
              historyGridView.Items[index].Selected = true;
              historyGridView.EnsureVisible(index);
              break;
            }
          }
        }
        tag.DataValues.ReadOnly = false;
      }
    }

    private void stdIconNew_Click(object sender, EventArgs e)
    {
      this.selectedTrackingLog = new AUSTrackingHistoryLog(Guid.NewGuid().ToString());
      this.selectedTrackingLog.CopyLoanToLog(this.loan, true);
      this.selectedTrackingLog.SubmissionDate = Utils.ConvertDateTimeToEst(DateTime.Now).ToString("MM/dd/yyyy");
      this.selectedTrackingLog.SubmissionTime = Utils.ConvertDateTimeToEst(DateTime.Now).ToString("hh:mm:ss tt");
      this.selectedTrackingLog.RecordType = "Manual";
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.selectedTrackingLog.DataValues, "AUS Details", this.mainFormInfo, 600, 750, FieldSource.CurrentLoan, "", this.session))
      {
        entryPopupDialog.InputConfirmationIsRequired();
        entryPopupDialog.OkClicked += new EventHandler(this.quickPage_OkClicked);
        if (entryPopupDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (this.selectedTrackingLog.SubmissionType == "LQA")
        {
          this.gridViewLQAResults.SelectedItems.Clear();
          this.selectedTrackingLog.Date = this.session.SessionObjects.Session.ServerTime;
          if (!this.loan.AddAUSTrackingHistory(this.selectedTrackingLog))
            return;
          this.gridViewLQAResults.Items.Insert(0, this.BuildSnapshotItemLqa(this.selectedTrackingLog, true));
          this.tabControl_history.SelectTab(1);
        }
        else
        {
          this.gridViewUnderwriting.SelectedItems.Clear();
          this.selectedTrackingLog.Date = DateTime.UtcNow;
          if (!this.loan.AddAUSTrackingHistory(this.selectedTrackingLog))
            return;
          this.gridViewUnderwriting.Items.Insert(0, this.BuildSnapshotItem(this.selectedTrackingLog, true));
          this.RefreshRecordCount();
          this.RefreshLatestSubmissionSection();
        }
      }
    }

    private void quickPage_OkClicked(object sender, EventArgs e)
    {
      QuickEntryPopupDialog entryPopupDialog = (QuickEntryPopupDialog) sender;
      if (entryPopupDialog == null || this.selectedTrackingLog == null)
        return;
      if (this.selectedTrackingLog.GetField("AUS.X1") == string.Empty)
      {
        entryPopupDialog.DataIsValid = false;
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please select Underwriting Risk Assess Type!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.selectedTrackingLog.GetField("AUS.X3") == string.Empty)
      {
        entryPopupDialog.DataIsValid = false;
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please enter date/time to Submission Date!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        entryPopupDialog.DataIsValid = true;
    }

    private void stdIconEdit_Click(object sender, EventArgs e)
    {
      AUSTrackingHistoryLog tag = (AUSTrackingHistoryLog) this.gridViewUnderwriting.SelectedItems[0].Tag;
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) tag.DataValues, "AUS Details", this.mainFormInfo, 600, 750, FieldSource.CurrentLoan, "", this.session))
      {
        entryPopupDialog.InputConfirmationIsRequired();
        if (entryPopupDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        for (int index = 0; index < this.gridViewUnderwriting.Items.Count; ++index)
        {
          if (string.Compare(((AUSTrackingHistoryLog) this.gridViewUnderwriting.Items[index].Tag).HistoryID, tag.HistoryID, true) == 0)
          {
            this.loan.UpdateAUSTrackingHistory(tag);
            this.updateSnapshotItem(this.gridViewUnderwriting.Items[index], tag);
            this.gridViewUnderwriting.Items[index].Selected = true;
            this.gridViewUnderwriting.EnsureVisible(index);
            this.RefreshLatestSubmissionSection();
            break;
          }
        }
      }
    }

    private void updateSnapshotItem(GVItem selectedItem, AUSTrackingHistoryLog rec)
    {
      selectedItem.Text = rec.Date.ToString("MM/dd/yyyy hh:mm:ss tt");
      selectedItem.SubItems[1].Text = rec.RecordType;
      selectedItem.SubItems[2].Text = rec.SubmissionDateTime;
      selectedItem.SubItems[3].Text = rec.SubmissionTypeToString;
      selectedItem.SubItems[4].Text = rec.Recommendation;
      selectedItem.SubItems[5].Text = rec.SubmittedBy;
      selectedItem.SubItems[6].Text = rec.AUSVersion;
      selectedItem.SubItems[7].Text = rec.SubmissionNumber.ToString();
      selectedItem.Tag = (object) rec;
    }

    private void textField_Enter(object sender, EventArgs e)
    {
      string fieldId = ((System.Windows.Forms.Control) sender).Tag.ToString();
      if (!((fieldId ?? "") != string.Empty))
        return;
      this.DisplayFieldId(fieldId);
    }

    private void DisplayFieldId(string fieldId)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(fieldId);
    }

    private void RefreshLatestSubmissionSection()
    {
      this.PopulateFieldValues(this.groupContainer1.Controls);
      this.btnCopy.Enabled = this.gridViewUnderwriting.Items.Count > 0;
    }

    private void PopulateFieldValues(System.Windows.Forms.Control.ControlCollection cs)
    {
      foreach (System.Windows.Forms.Control c in (ArrangedElementCollection) cs)
      {
        if (!(c is System.Windows.Forms.TextBox))
          this.PopulateFieldValues(c.Controls);
        else if (c.Tag != null)
        {
          string str1 = c.Tag.ToString();
          if (!(str1 == string.Empty))
          {
            c.Text = this.loan.GetField(str1);
            if (this.toolTip1.GetToolTip(c) == str1)
            {
              string str2 = FieldHelp.GetText(str1) ?? "";
              this.toolTip1.SetToolTip(c, str1 + (str2.Trim() != string.Empty ? ": " : "") + str2);
            }
          }
        }
      }
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      if (this.gridViewUnderwriting.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is no AUS Tracking data.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy AUS Tracking data to Transmittal Summary?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
          return;
        this.loan.Calculator.FormCalculation("copyaustoloan", (string) null, (string) null);
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      }
    }

    private void RefreshRecordCount()
    {
      this.gpcHistoryList.Text = "Underwriting Decision History (" + (object) this.gridViewUnderwriting.Items.Count + ")";
    }

    public string GetHelpTargetName() => "AUS Tracking";

    public void RefreshContents() => this.InitForm();

    public void RefreshLoanContents() => this.InitForm();

    private void btnRequestUnderwriting_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEPass>().ProcessURL(Epass.AU.Url, true, true);
    }

    internal void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl_history.SelectedIndex == 0)
      {
        this.ApplySecurity();
        this.stdIconEdit.Visible = true;
        this._selectedTabGridView = this.gridViewUnderwriting;
        this._compareButton = this.btnCompareLPAReports;
        this.stdIconView.Enabled = this.gridViewUnderwriting.SelectedItems.Count >= 1;
      }
      else
      {
        this.stdIconNew.Visible = false;
        this.stdIconEdit.Visible = false;
        this._selectedTabGridView = this.gridViewLQAResults;
        this._compareButton = this.btnCompareLQAReports;
        this.stdIconView.Enabled = this.gridViewLQAResults.SelectedItems.Count >= 1;
      }
    }

    private void CompareButtonReports_Click(object sender, EventArgs e)
    {
      try
      {
        this.GetSelectedGridViewAndCompareButton();
        this.CompareReports(this._selectedTabGridView);
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Error.ToString(), "AUSTrackingTool.cs.btnCompareLPAReports_Click", ex.ToString());
      }
    }

    private void GetSelectedGridViewAndCompareButton()
    {
      this._selectedTabGridView = this.tabControl_history.SelectedIndex == 0 ? this.gridViewUnderwriting : this.gridViewLQAResults;
      this._compareButton = this.tabControl_history.SelectedIndex == 0 ? this.btnCompareLPAReports : this.btnCompareLQAReports;
    }

    private void GridViewDoubleClick(object sender, EventArgs e)
    {
      this.GetSelectedGridViewAndCompareButton();
      this.GridViewResultsDoubleClick(this._selectedTabGridView);
    }

    private void GridViewSelectedIndexChanged(object sender, EventArgs e)
    {
      this.GetSelectedGridViewAndCompareButton();
      this.GridViewSelectedIndexChanged(this._selectedTabGridView, this._compareButton);
    }

    private GVItem BuildSnapshotItem(AUSTrackingHistoryLog rec, bool selected)
    {
      return new GVItem(Utils.ConvertDateTimeToEst(rec.Date).ToString("MM/dd/yyyy hh:mm:ss tt"))
      {
        SubItems = {
          (object) rec.RecordType,
          (object) rec.SubmissionDateTime,
          (object) rec.SubmissionTypeToString,
          (object) rec.Recommendation,
          (object) rec.SubmittedBy,
          (object) rec.AUSVersion,
          (object) rec.SubmissionNumber.ToString()
        },
        Tag = (object) rec,
        Selected = selected
      };
    }

    private GVItem BuildSnapshotItemLqa(AUSTrackingHistoryLog rec, bool selected)
    {
      return new GVItem(rec.SubmissionDateTime)
      {
        SubItems = {
          (object) "LQA",
          (object) rec.SubmissionTypeToString,
          (object) rec.Recommendation,
          (object) rec.SubmittedBy
        },
        Tag = (object) rec,
        Selected = selected
      };
    }

    internal static void BtnCompareReportsEnabled(System.Windows.Forms.Button btnCompare, GridView resultsGridView)
    {
      try
      {
        btnCompare.Enabled = false;
        if (resultsGridView != null && resultsGridView.SelectedItems != null && resultsGridView.SelectedItems.Count != 2)
          return;
        if (new List<string>()
        {
          resultsGridView.SelectedItems[0].SubItems["gvType"].Text,
          resultsGridView.SelectedItems[1].SubItems["gvType"].Text
        }.Any<string>((Func<string, bool>) (type => !string.Equals(type, "Freddie Mac", StringComparison.OrdinalIgnoreCase))))
          return;
        btnCompare.Enabled = true;
      }
      catch (Exception ex)
      {
        btnCompare.Enabled = false;
        Tracing.Log(true, TraceLevel.Error.ToString(), "AUSTrackingTool.cs.BtnCompareReportsEnabled", ex.ToString());
      }
    }

    private void GridViewResultsDoubleClick(GridView gridViewResults)
    {
      if (gridViewResults.SelectedItems == null || gridViewResults.SelectedItems.Count == 0)
        return;
      if (gridViewResults.SelectedItems[0].SubItems[1].Text == "Manual" && this.stdIconEdit.Enabled)
        this.stdIconEdit_Click((object) null, (EventArgs) null);
      else
        this.ViewHistoryRecord(gridViewResults);
    }

    private void CompareReports(GridView gridViewLqaResults)
    {
      this.SelectedhistoryCollection = gridViewLqaResults.SelectedItems;
      this.LoanDeltaDialogEvents.ShowLoanDataDeltaDifferences();
    }

    private void GridViewSelectedIndexChanged(GridView gridViewResults, System.Windows.Forms.Button btnCompareLqaReports)
    {
      this.stdIconView.Enabled = gridViewResults.SelectedItems.Count == 1;
      this.stdIconEdit.Enabled = gridViewResults.SelectedItems.Count == 1 && gridViewResults.SelectedItems[0].SubItems[1].Text == "Manual";
      AUSTrackingTool.BtnCompareReportsEnabled(btnCompareLqaReports, gridViewResults);
    }

    private async void btnOpenUwCenterInWeb_ClickAsync(object sender, EventArgs e)
    {
      await DeepLinkLauncher.LaunchWebAppInBrowserAsync(DeepLinkType.UnderwriterCenter, (IDeepLinkContext) new AUSTrackingToolContext());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.panelForm = new System.Windows.Forms.Panel();
      this.gpcHistoryList = new GroupContainer();
      this.tabControl_history = new TabControl();
      this.tabPageUnderwriting = new TabPage();
      this.panel1 = new System.Windows.Forms.Panel();
      this.labelCompareText = new System.Windows.Forms.Label();
      this.btnCompareLPAReports = new System.Windows.Forms.Button();
      this.gridViewUnderwriting = new GridView();
      this.tabPageLoanQualityAdvisor = new TabPage();
      this.panel2 = new System.Windows.Forms.Panel();
      this.labelLQACompareText = new System.Windows.Forms.Label();
      this.btnCompareLQAReports = new System.Windows.Forms.Button();
      this.gridViewLQAResults = new GridView();
      this.stdIconEdit = new StandardIconButton();
      this.stdIconNew = new StandardIconButton();
      this.stdIconView = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.btnRequestUnderwriting = new System.Windows.Forms.Button();
      this.textBox11 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.textBox10 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.textBox24 = new System.Windows.Forms.TextBox();
      this.textBox9 = new System.Windows.Forms.TextBox();
      this.label27 = new System.Windows.Forms.Label();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.textBox8 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.btnCopy = new System.Windows.Forms.Button();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.textBox5 = new System.Windows.Forms.TextBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.textBox36 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label41 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.textBox13 = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.textBox23 = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
      this.textBox27 = new System.Windows.Forms.TextBox();
      this.label29 = new System.Windows.Forms.Label();
      this.textBox25 = new System.Windows.Forms.TextBox();
      this.label28 = new System.Windows.Forms.Label();
      this.textBox26 = new System.Windows.Forms.TextBox();
      this.toolTip1 = new ToolTip(this.components);
      this.btnOpenUwCenterInWeb = new System.Windows.Forms.Button();
      this.panelForm.SuspendLayout();
      this.gpcHistoryList.SuspendLayout();
      this.tabControl_history.SuspendLayout();
      this.tabPageUnderwriting.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tabPageLoanQualityAdvisor.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.stdIconEdit).BeginInit();
      ((ISupportInitialize) this.stdIconNew).BeginInit();
      ((ISupportInitialize) this.stdIconView).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.panelForm.Controls.Add((System.Windows.Forms.Control) this.gpcHistoryList);
      this.panelForm.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.panelForm.Dock = DockStyle.Left;
      this.panelForm.Location = new Point(0, 0);
      this.panelForm.Name = "panelForm";
      this.panelForm.Size = new Size(884, 412);
      this.panelForm.TabIndex = 2;
      this.gpcHistoryList.BackColor = Color.White;
      this.gpcHistoryList.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gpcHistoryList.Controls.Add((System.Windows.Forms.Control) this.tabControl_history);
      this.gpcHistoryList.Controls.Add((System.Windows.Forms.Control) this.stdIconEdit);
      this.gpcHistoryList.Controls.Add((System.Windows.Forms.Control) this.stdIconNew);
      this.gpcHistoryList.Controls.Add((System.Windows.Forms.Control) this.stdIconView);
      this.gpcHistoryList.Dock = DockStyle.Fill;
      this.gpcHistoryList.HeaderForeColor = SystemColors.ControlText;
      this.gpcHistoryList.Location = new Point(0, 179);
      this.gpcHistoryList.Name = "gpcHistoryList";
      this.gpcHistoryList.Size = new Size(884, 233);
      this.gpcHistoryList.TabIndex = 2;
      this.gpcHistoryList.Text = "Underwriting Decision History (#)";
      this.tabControl_history.Controls.Add((System.Windows.Forms.Control) this.tabPageUnderwriting);
      this.tabControl_history.Controls.Add((System.Windows.Forms.Control) this.tabPageLoanQualityAdvisor);
      this.tabControl_history.Dock = DockStyle.Fill;
      this.tabControl_history.Location = new Point(1, 25);
      this.tabControl_history.Name = "tabControl_history";
      this.tabControl_history.SelectedIndex = 0;
      this.tabControl_history.Size = new Size(882, 207);
      this.tabControl_history.TabIndex = 48;
      this.tabControl_history.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabPageUnderwriting.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.tabPageUnderwriting.Location = new Point(4, 22);
      this.tabPageUnderwriting.Name = "tabPageUnderwriting";
      this.tabPageUnderwriting.Padding = new Padding(3);
      this.tabPageUnderwriting.Size = new Size(874, 181);
      this.tabPageUnderwriting.TabIndex = 0;
      this.tabPageUnderwriting.Text = "Underwriting";
      this.tabPageUnderwriting.UseVisualStyleBackColor = true;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.labelCompareText);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnCompareLPAReports);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.gridViewUnderwriting);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(868, 175);
      this.panel1.TabIndex = 47;
      this.labelCompareText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.labelCompareText.AutoSize = true;
      this.labelCompareText.Location = new Point(306, 148);
      this.labelCompareText.Name = "labelCompareText";
      this.labelCompareText.Size = new Size(367, 13);
      this.labelCompareText.TabIndex = 373;
      this.labelCompareText.Text = "Select two reports with a Type of 'Freddie Mac' to view any data differences.";
      this.btnCompareLPAReports.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCompareLPAReports.Location = new Point(688, 144);
      this.btnCompareLPAReports.Name = "btnCompareLPAReports";
      this.btnCompareLPAReports.Size = new Size(175, 21);
      this.btnCompareLPAReports.TabIndex = 372;
      this.btnCompareLPAReports.Text = "Compare Reports";
      this.btnCompareLPAReports.UseVisualStyleBackColor = true;
      this.btnCompareLPAReports.Click += new EventHandler(this.CompareButtonReports_Click);
      this.gridViewUnderwriting.AllowMultiselect = false;
      this.gridViewUnderwriting.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gridViewUnderwriting.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvRecDate";
      gvColumn1.Text = "Record Date/Time";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvRecType";
      gvColumn2.Text = "Record Type";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvSubmissionDate";
      gvColumn3.Text = "Submission Date/Time";
      gvColumn3.Width = 140;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvType";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvRec";
      gvColumn5.Text = "Recommendation";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvBy";
      gvColumn6.Text = "By";
      gvColumn6.Width = 120;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "gvVersion";
      gvColumn7.Text = "AUS Version #";
      gvColumn7.Width = 90;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "gvNumber";
      gvColumn8.Text = "Submission Number";
      gvColumn8.Width = 120;
      this.gridViewUnderwriting.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gridViewUnderwriting.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewUnderwriting.Location = new Point(0, 0);
      this.gridViewUnderwriting.Name = "gridViewUnderwriting";
      this.gridViewUnderwriting.Size = new Size(868, 135);
      this.gridViewUnderwriting.SortIconVisible = false;
      this.gridViewUnderwriting.SortOption = GVSortOption.None;
      this.gridViewUnderwriting.TabIndex = 43;
      this.gridViewUnderwriting.SelectedIndexChanged += new EventHandler(this.GridViewSelectedIndexChanged);
      this.gridViewUnderwriting.DoubleClick += new EventHandler(this.GridViewDoubleClick);
      this.tabPageLoanQualityAdvisor.Controls.Add((System.Windows.Forms.Control) this.panel2);
      this.tabPageLoanQualityAdvisor.Location = new Point(4, 22);
      this.tabPageLoanQualityAdvisor.Name = "tabPageLoanQualityAdvisor";
      this.tabPageLoanQualityAdvisor.Padding = new Padding(3);
      this.tabPageLoanQualityAdvisor.Size = new Size(874, 181);
      this.tabPageLoanQualityAdvisor.TabIndex = 1;
      this.tabPageLoanQualityAdvisor.Text = "Loan Quality Advisor History";
      this.tabPageLoanQualityAdvisor.UseVisualStyleBackColor = true;
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.labelLQACompareText);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.btnCompareLQAReports);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.gridViewLQAResults);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(868, 175);
      this.panel2.TabIndex = 48;
      this.labelLQACompareText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.labelLQACompareText.AutoSize = true;
      this.labelLQACompareText.Location = new Point(306, 148);
      this.labelLQACompareText.Name = "labelLQACompareText";
      this.labelLQACompareText.Size = new Size(367, 13);
      this.labelLQACompareText.TabIndex = 373;
      this.labelLQACompareText.Text = "Select two reports with a Type of 'Freddie Mac' to view any data differences.";
      this.btnCompareLQAReports.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCompareLQAReports.Location = new Point(688, 144);
      this.btnCompareLQAReports.Name = "btnCompareLQAReports";
      this.btnCompareLQAReports.Size = new Size(175, 21);
      this.btnCompareLQAReports.TabIndex = 372;
      this.btnCompareLQAReports.Text = "Compare Reports";
      this.btnCompareLQAReports.UseVisualStyleBackColor = true;
      this.btnCompareLQAReports.Click += new EventHandler(this.CompareButtonReports_Click);
      this.gridViewLQAResults.AllowMultiselect = false;
      this.gridViewLQAResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gridViewLQAResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "gvSubmissionDate";
      gvColumn9.Text = "Submission Date/Time";
      gvColumn9.Width = 200;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "gvRecType";
      gvColumn10.Text = "Record Type";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "gvType";
      gvColumn11.Text = "Type";
      gvColumn11.Width = 120;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "gvRec";
      gvColumn12.Text = "Purchase Eligibility";
      gvColumn12.Width = 325;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "gvBy";
      gvColumn13.Text = "By";
      gvColumn13.Width = 120;
      this.gridViewLQAResults.Columns.AddRange(new GVColumn[5]
      {
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gridViewLQAResults.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewLQAResults.Location = new Point(0, 0);
      this.gridViewLQAResults.Name = "gridViewLQAResults";
      this.gridViewLQAResults.Size = new Size(868, 135);
      this.gridViewLQAResults.SortIconVisible = false;
      this.gridViewLQAResults.SortOption = GVSortOption.None;
      this.gridViewLQAResults.TabIndex = 374;
      this.gridViewLQAResults.SelectedIndexChanged += new EventHandler(this.GridViewSelectedIndexChanged);
      this.gridViewLQAResults.DoubleClick += new EventHandler(this.GridViewDoubleClick);
      this.stdIconEdit.BackColor = Color.Transparent;
      this.stdIconEdit.Location = new Point(837, 4);
      this.stdIconEdit.MouseDownImage = (System.Drawing.Image) null;
      this.stdIconEdit.Name = "stdIconEdit";
      this.stdIconEdit.Size = new Size(16, 16);
      this.stdIconEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconEdit.TabIndex = 46;
      this.stdIconEdit.TabStop = false;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.stdIconEdit, "Edit History");
      this.stdIconEdit.Click += new EventHandler(this.stdIconEdit_Click);
      this.stdIconNew.BackColor = Color.Transparent;
      this.stdIconNew.Location = new Point(815, 4);
      this.stdIconNew.MouseDownImage = (System.Drawing.Image) null;
      this.stdIconNew.Name = "stdIconNew";
      this.stdIconNew.Size = new Size(16, 16);
      this.stdIconNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconNew.TabIndex = 45;
      this.stdIconNew.TabStop = false;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.stdIconNew, "New History");
      this.stdIconNew.Click += new EventHandler(this.stdIconNew_Click);
      this.stdIconView.BackColor = Color.Transparent;
      this.stdIconView.Location = new Point(859, 4);
      this.stdIconView.MouseDownImage = (System.Drawing.Image) null;
      this.stdIconView.Name = "stdIconView";
      this.stdIconView.Size = new Size(16, 16);
      this.stdIconView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.stdIconView.TabIndex = 44;
      this.stdIconView.TabStop = false;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.stdIconView, "View History");
      this.stdIconView.Click += new EventHandler(this.stdIconView_Click);
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnOpenUwCenterInWeb);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnRequestUnderwriting);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox11);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox10);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox24);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox9);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label27);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox7);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox8);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnCopy);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox6);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox5);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox1);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox4);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox36);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label41);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox13);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label18);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox3);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label17);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox23);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label30);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label26);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox27);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label29);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox25);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label28);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.textBox26);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(884, 179);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "AUS Tracking";
      this.btnRequestUnderwriting.Location = new Point(425, 3);
      this.btnRequestUnderwriting.Name = "btnRequestUnderwriting";
      this.btnRequestUnderwriting.Size = new Size(130, 22);
      this.btnRequestUnderwriting.TabIndex = 380;
      this.btnRequestUnderwriting.Text = "Request Underwriting";
      this.btnRequestUnderwriting.UseVisualStyleBackColor = true;
      this.btnRequestUnderwriting.Click += new EventHandler(this.btnRequestUnderwriting_Click);
      this.textBox11.Location = new Point(444, 101);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(138, 20);
      this.textBox11.TabIndex = 379;
      this.textBox11.TabStop = false;
      this.textBox11.Tag = (object) "AUSF.X10";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox11, "AUSF.X10");
      this.textBox11.Enter += new EventHandler(this.textField_Enter);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(319, 104);
      this.label10.Name = "label10";
      this.label10.Size = new Size(108, 13);
      this.label10.TabIndex = 378;
      this.label10.Text = "First Submission Time";
      this.textBox10.Location = new Point(444, 57);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(138, 20);
      this.textBox10.TabIndex = 377;
      this.textBox10.TabStop = false;
      this.textBox10.Tag = (object) "AUSF.X8";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox10, "AUSF.X8");
      this.textBox10.Enter += new EventHandler(this.textField_Enter);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(319, 60);
      this.label5.Name = "label5";
      this.label5.Size = new Size(86, 13);
      this.label5.TabIndex = 376;
      this.label5.Text = "Submission Time";
      this.textBox24.Location = new Point(737, 35);
      this.textBox24.Name = "textBox24";
      this.textBox24.ReadOnly = true;
      this.textBox24.Size = new Size(138, 20);
      this.textBox24.TabIndex = 360;
      this.textBox24.TabStop = false;
      this.textBox24.Tag = (object) "AUSF.X17";
      this.textBox24.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox24, "AUSF.X17");
      this.textBox24.Enter += new EventHandler(this.textField_Enter);
      this.textBox9.Location = new Point(165, 35);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(138, 20);
      this.textBox9.TabIndex = 375;
      this.textBox9.TabStop = false;
      this.textBox9.Tag = (object) "AUSF.X1";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox9, "AUSF.X1");
      this.textBox9.Enter += new EventHandler(this.textField_Enter);
      this.label27.AutoSize = true;
      this.label27.Location = new Point(601, 38);
      this.label27.Name = "label27";
      this.label27.Size = new Size(74, 13);
      this.label27.TabIndex = 102;
      this.label27.Text = "Housing Ratio";
      this.textBox7.Location = new Point(444, 35);
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.Size = new Size(138, 20);
      this.textBox7.TabIndex = 373;
      this.textBox7.TabStop = false;
      this.textBox7.Tag = (object) "AUSF.X7";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox7, "AUSF.X7");
      this.textBox7.Enter += new EventHandler(this.textField_Enter);
      this.textBox8.Location = new Point(444, 79);
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new Size(138, 20);
      this.textBox8.TabIndex = 374;
      this.textBox8.TabStop = false;
      this.textBox8.Tag = (object) "AUSF.X9";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox8, "AUSF.X9");
      this.textBox8.Enter += new EventHandler(this.textField_Enter);
      this.textBox2.Location = new Point(165, 79);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new Size(138, 20);
      this.textBox2.TabIndex = 372;
      this.textBox2.TabStop = false;
      this.textBox2.Tag = (object) "AUSF.X3";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox2, "AUSF.X3");
      this.textBox2.Enter += new EventHandler(this.textField_Enter);
      this.btnOpenUwCenterInWeb.Location = new Point(735, 2);
      this.btnOpenUwCenterInWeb.Name = "btnOpenUwCenterInWeb";
      this.btnOpenUwCenterInWeb.Size = new Size(142, 22);
      this.btnOpenUwCenterInWeb.TabIndex = 381;
      this.btnOpenUwCenterInWeb.Text = "Open Underwriting Center";
      this.btnOpenUwCenterInWeb.UseVisualStyleBackColor = true;
      this.btnOpenUwCenterInWeb.Visible = false;
      this.btnOpenUwCenterInWeb.Click += new EventHandler(this.btnOpenUwCenterInWeb_ClickAsync);
      this.btnCopy.Location = new Point(565, 2);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(160, 22);
      this.btnCopy.TabIndex = 371;
      this.btnCopy.Text = "Copy to Transmittal Summary";
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
      this.textBox6.Location = new Point(165, 123);
      this.textBox6.Name = "textBox6";
      this.textBox6.ReadOnly = true;
      this.textBox6.Size = new Size(138, 20);
      this.textBox6.TabIndex = 100;
      this.textBox6.TabStop = false;
      this.textBox6.Tag = (object) "AUSF.X5";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox6, "AUSF.X5");
      this.textBox6.Enter += new EventHandler(this.textField_Enter);
      this.textBox5.Location = new Point(444, 145);
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new Size(138, 20);
      this.textBox5.TabIndex = 90;
      this.textBox5.TabStop = false;
      this.textBox5.Tag = (object) "AUSF.X12";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox5, "AUSF.X12");
      this.textBox5.Enter += new EventHandler(this.textField_Enter);
      this.textBox1.Location = new Point(444, 123);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(138, 20);
      this.textBox1.TabIndex = 50;
      this.textBox1.TabStop = false;
      this.textBox1.Tag = (object) "AUSF.X11";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox1, "AUSF.X11");
      this.textBox1.Enter += new EventHandler(this.textField_Enter);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(319, 82);
      this.label4.Name = "label4";
      this.label4.Size = new Size(108, 13);
      this.label4.TabIndex = 49;
      this.label4.Text = "First Submission Date";
      this.textBox4.Location = new Point(165, 145);
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.Size = new Size(138, 20);
      this.textBox4.TabIndex = 80;
      this.textBox4.TabStop = false;
      this.textBox4.Tag = (object) "AUSF.X6";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox4, "AUSF.X6");
      this.textBox4.Enter += new EventHandler(this.textField_Enter);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(319, 148);
      this.label8.Name = "label8";
      this.label8.Size = new Size(77, 13);
      this.label8.TabIndex = 56;
      this.label8.Text = "AUS Version #";
      this.textBox36.Location = new Point(737, 57);
      this.textBox36.Name = "textBox36";
      this.textBox36.ReadOnly = true;
      this.textBox36.Size = new Size(138, 20);
      this.textBox36.TabIndex = 370;
      this.textBox36.TabStop = false;
      this.textBox36.Tag = (object) "AUSF.X18";
      this.textBox36.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox36, "AUSF.X18");
      this.textBox36.Enter += new EventHandler(this.textField_Enter);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(319, 38);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 13);
      this.label3.TabIndex = 47;
      this.label3.Text = "Submission Date";
      this.label41.AutoSize = true;
      this.label41.Location = new Point(601, 60);
      this.label41.Name = "label41";
      this.label41.Size = new Size(103, 13);
      this.label41.TabIndex = 120;
      this.label41.Text = "Total Expense Ratio";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 126);
      this.label9.Name = "label9";
      this.label9.Size = new Size(99, 13);
      this.label9.TabIndex = 57;
      this.label9.Text = "Doc Class (Freddie)";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 148);
      this.label7.Name = "label7";
      this.label7.Size = new Size(69, 13);
      this.label7.TabIndex = 55;
      this.label7.Text = "Submitted By";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 82);
      this.label2.Name = "label2";
      this.label2.Size = new Size(115, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "AUS Recommendation";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 38);
      this.label1.Name = "label1";
      this.label1.Size = new Size(153, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Underwriting Risk Assess Type";
      this.textBox13.Location = new Point(165, 57);
      this.textBox13.Name = "textBox13";
      this.textBox13.ReadOnly = true;
      this.textBox13.Size = new Size(138, 20);
      this.textBox13.TabIndex = 110;
      this.textBox13.TabStop = false;
      this.textBox13.Tag = (object) "AUSF.X2";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox13, "AUSF.X2");
      this.textBox13.Enter += new EventHandler(this.textField_Enter);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(50, 60);
      this.label18.Name = "label18";
      this.label18.Size = new Size(108, 13);
      this.label18.TabIndex = 61;
      this.label18.Text = "If \"Other\" Description";
      this.textBox3.Location = new Point(165, 101);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(138, 20);
      this.textBox3.TabIndex = 70;
      this.textBox3.TabStop = false;
      this.textBox3.Tag = (object) "AUSF.X4";
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox3, "AUSF.X4");
      this.textBox3.Enter += new EventHandler(this.textField_Enter);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 104);
      this.label6.Name = "label6";
      this.label6.Size = new Size(148, 13);
      this.label6.TabIndex = 53;
      this.label6.Text = "DU Case ID / LPA AUS Key#";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(319, 126);
      this.label17.Name = "label17";
      this.label17.Size = new Size(100, 13);
      this.label17.TabIndex = 63;
      this.label17.Text = "Submission Number";
      this.textBox23.Location = new Point(737, 123);
      this.textBox23.Name = "textBox23";
      this.textBox23.ReadOnly = true;
      this.textBox23.Size = new Size(138, 20);
      this.textBox23.TabIndex = 320;
      this.textBox23.TabStop = false;
      this.textBox23.Tag = (object) "AUSF.X13";
      this.textBox23.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox23, "AUSF.X13");
      this.textBox23.Enter += new EventHandler(this.textField_Enter);
      this.label30.AutoSize = true;
      this.label30.Location = new Point(601, 82);
      this.label30.Name = "label30";
      this.label30.Size = new Size(109, 13);
      this.label30.TabIndex = 98;
      this.label30.Text = "Total Monthly Income";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(601, 126);
      this.label26.Name = "label26";
      this.label26.Size = new Size(117, 13);
      this.label26.TabIndex = 106;
      this.label26.Text = "Total Housing Payment";
      this.textBox27.Location = new Point(737, 79);
      this.textBox27.Name = "textBox27";
      this.textBox27.ReadOnly = true;
      this.textBox27.Size = new Size(138, 20);
      this.textBox27.TabIndex = 330;
      this.textBox27.TabStop = false;
      this.textBox27.Tag = (object) "AUSF.X14";
      this.textBox27.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox27, "AUSF.X14");
      this.textBox27.Enter += new EventHandler(this.textField_Enter);
      this.label29.AutoSize = true;
      this.label29.Location = new Point(601, 148);
      this.label29.Name = "label29";
      this.label29.Size = new Size(96, 13);
      this.label29.TabIndex = 100;
      this.label29.Text = "Total Liquid Assets";
      this.textBox25.Location = new Point(737, 101);
      this.textBox25.Name = "textBox25";
      this.textBox25.ReadOnly = true;
      this.textBox25.Size = new Size(138, 20);
      this.textBox25.TabIndex = 350;
      this.textBox25.TabStop = false;
      this.textBox25.Tag = (object) "AUSF.X16";
      this.textBox25.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox25, "AUSF.X16");
      this.textBox25.Enter += new EventHandler(this.textField_Enter);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(601, 104);
      this.label28.Name = "label28";
      this.label28.Size = new Size(97, 13);
      this.label28.TabIndex = 101;
      this.label28.Text = "Total Monthly Debt";
      this.textBox26.Location = new Point(737, 145);
      this.textBox26.Name = "textBox26";
      this.textBox26.ReadOnly = true;
      this.textBox26.Size = new Size(138, 20);
      this.textBox26.TabIndex = 340;
      this.textBox26.TabStop = false;
      this.textBox26.Tag = (object) "AUSF.X15";
      this.textBox26.TextAlign = HorizontalAlignment.Right;
      this.toolTip1.SetToolTip((System.Windows.Forms.Control) this.textBox26, "AUSF.X15");
      this.textBox26.Enter += new EventHandler(this.textField_Enter);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((System.Windows.Forms.Control) this.panelForm);
      this.Name = nameof (AUSTrackingTool);
      this.Size = new Size(935, 412);
      this.panelForm.ResumeLayout(false);
      this.gpcHistoryList.ResumeLayout(false);
      this.tabControl_history.ResumeLayout(false);
      this.tabPageUnderwriting.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tabPageLoanQualityAdvisor.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.stdIconEdit).EndInit();
      ((ISupportInitialize) this.stdIconNew).EndInit();
      ((ISupportInitialize) this.stdIconView).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
