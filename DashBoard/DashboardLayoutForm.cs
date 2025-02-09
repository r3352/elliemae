// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardLayoutForm
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardLayoutForm : Form
  {
    private const string dashboardCategory = "Dashboard";
    private const string reservedViewName = "Dashboard.ReservedViewName";
    private const string SECTION_DASHBOARD = "Dashboard";
    private const string DEFAULT_VIEW_ID = "Dashboard.DefaultViewId";
    private const int LAYOUT_COUNT = 18;
    private const int SNAPSHOT_COUNT = 9;
    private PictureBox[] picLayouts;
    private Label[] lblSnapshots;
    private TextBox[] txtSnapshots;
    private PictureBox[] picSnapshots;
    private ComboBox[] cboTimeFrames;
    private PictureBox picSelectedLayout;
    private DashboardViewList viewList;
    private List<int> mostRecentViewIds = new List<int>();
    private DashboardLayoutCollection dashboardLayouts = DashboardLayoutCollection.GetDashboardLayoutCollection();
    private IDictionary dashboardSettings;
    private DashboardLayoutForm.Action action;
    private DashboardView dashboardView;
    private int currentDefaultViewId;
    private bool isViewModified;
    private IContainer components;
    private Label lblName;
    private TextBox txtName;
    private Label lblHorizontal1;
    private Label lblHorizontal2;
    private Button btnCancel;
    private Button btnSave;
    private Label lblSelectLayout;
    private Label lblSelectSnapshot;
    private PictureBox picLayout1;
    private PictureBox picLayout2;
    private PictureBox picLayout3;
    private PictureBox picLayout4;
    private PictureBox picLayout6;
    private PictureBox picLayout5;
    private PictureBox picLayout7;
    private PictureBox picLayout9;
    private PictureBox picLayout8;
    private PictureBox picLayout10;
    private PictureBox picLayout12;
    private PictureBox picLayout11;
    private PictureBox picLayout15;
    private PictureBox picLayout14;
    private PictureBox picLayout13;
    private PictureBox picLayout18;
    private Label lblVertical1;
    private Label lblSnapshot1;
    private Label lblSnapshot2;
    private Label lblSnapshot3;
    private Label lblSnapshot4;
    private Label lblSnapshot5;
    private Label lblSnapshot6;
    private Label lblSnapshot7;
    private Label lblSnapshot8;
    private Label lblSnapshot9;
    private TextBox txtSnapshot1;
    private TextBox txtSnapshot2;
    private TextBox txtSnapshot3;
    private TextBox txtSnapshot4;
    private TextBox txtSnapshot5;
    private TextBox txtSnapshot6;
    private TextBox txtSnapshot7;
    private TextBox txtSnapshot8;
    private TextBox txtSnapshot9;
    private PictureBox picSnapshot1;
    private PictureBox picSnapshot2;
    private PictureBox picSnapshot3;
    private PictureBox picSnapshot4;
    private PictureBox picSnapshot5;
    private PictureBox picSnapshot6;
    private PictureBox picSnapshot7;
    private PictureBox picSnapshot8;
    private PictureBox picSnapshot9;
    private PictureBox picLayout16;
    private PictureBox picLayout17;
    private Panel pnlLayout18;
    private Panel pnlLayout1;
    private Panel pnlLayout2;
    private Panel pnlLayout3;
    private Panel pnlLayout17;
    private Panel pnlLayout4;
    private Panel pnlLayout5;
    private Panel pnlLayout6;
    private Panel pnlLayout7;
    private Panel pnlLayout8;
    private Panel pnlLayout9;
    private Panel pnlLayout10;
    private Panel pnlLayout11;
    private Panel pnlLayout12;
    private Panel pnlLayout13;
    private Panel pnlLayout14;
    private Panel pnlLayout15;
    private Panel pnlLayout16;
    private ImageList imglstLayouts;
    private ImageList imgLstSearch;
    private CheckBox chkDefaultView;
    private Label lblStartFrom;
    private ViewSelectionControl ctlViewSelection;
    private ToolTip tipToolTip;
    private Panel pnlSelectLayout;
    private Panel pnlSelectSnapshot;
    protected ComboBox cboTimeFrame2;
    protected ComboBox cboTimeFrame1;
    protected ComboBox cboTimeFrame9;
    protected ComboBox cboTimeFrame8;
    protected ComboBox cboTimeFrame7;
    protected ComboBox cboTimeFrame6;
    protected ComboBox cboTimeFrame5;
    protected ComboBox cboTimeFrame4;
    protected ComboBox cboTimeFrame3;

    public DashboardView DashboardView => this.dashboardView;

    public DashboardLayoutForm(DashboardViewList viewList, List<int> mostRecentViewIds)
    {
      this.viewList = viewList;
      this.mostRecentViewIds = mostRecentViewIds;
      this.InitializeComponent();
      this.initialize();
    }

    public void CreateDashboardView()
    {
      this.action = DashboardLayoutForm.Action.CREATE_VIEW;
      this.Text = "Create Dashboard Layout";
      this.dashboardView = DashboardView.NewDashboardView();
      this.dashboardView.UserId = Session.UserID;
      this.dashboardView.ViewName = string.Empty;
      this.txtName.Focus();
      this.updateUIFromView((DashboardView) null);
      this.setViewModified(true);
    }

    public void EditDashboardView(DashboardView dashboardView)
    {
      this.action = DashboardLayoutForm.Action.EDIT_VIEW;
      this.Text = "Edit Dashboard Layout";
      this.dashboardView = dashboardView;
      this.txtName.ReadOnly = true;
      if (dashboardView.IsViewReadOnly)
        this.setReadOnlyMode();
      this.updateUIFromView((DashboardView) null);
      this.setViewModified(false);
    }

    private void initialize()
    {
      this.ctlViewSelection.RefreshSelections(this.viewList, this.mostRecentViewIds);
      this.ctlViewSelection.SetSelectedViewId(-1);
      this.currentDefaultViewId = 0;
      try
      {
        this.currentDefaultViewId = Convert.ToInt32(Session.GetPrivateProfileString("Dashboard", "Dashboard.DefaultViewId"));
      }
      catch (Exception ex)
      {
      }
      this.dashboardLayouts = DashboardLayoutCollection.GetDashboardLayoutCollection();
      this.picLayouts = new PictureBox[18];
      this.picLayouts[0] = this.picLayout1;
      this.picLayouts[1] = this.picLayout2;
      this.picLayouts[2] = this.picLayout3;
      this.picLayouts[3] = this.picLayout4;
      this.picLayouts[4] = this.picLayout5;
      this.picLayouts[5] = this.picLayout6;
      this.picLayouts[6] = this.picLayout7;
      this.picLayouts[7] = this.picLayout8;
      this.picLayouts[8] = this.picLayout9;
      this.picLayouts[9] = this.picLayout10;
      this.picLayouts[10] = this.picLayout11;
      this.picLayouts[11] = this.picLayout12;
      this.picLayouts[12] = this.picLayout13;
      this.picLayouts[13] = this.picLayout14;
      this.picLayouts[14] = this.picLayout15;
      this.picLayouts[15] = this.picLayout16;
      this.picLayouts[16] = this.picLayout17;
      this.picLayouts[17] = this.picLayout18;
      this.lblSnapshots = new Label[9];
      this.lblSnapshots[0] = this.lblSnapshot1;
      this.lblSnapshots[1] = this.lblSnapshot2;
      this.lblSnapshots[2] = this.lblSnapshot3;
      this.lblSnapshots[3] = this.lblSnapshot4;
      this.lblSnapshots[4] = this.lblSnapshot5;
      this.lblSnapshots[5] = this.lblSnapshot6;
      this.lblSnapshots[6] = this.lblSnapshot7;
      this.lblSnapshots[7] = this.lblSnapshot8;
      this.lblSnapshots[8] = this.lblSnapshot9;
      this.txtSnapshots = new TextBox[9];
      this.txtSnapshots[0] = this.txtSnapshot1;
      this.txtSnapshots[1] = this.txtSnapshot2;
      this.txtSnapshots[2] = this.txtSnapshot3;
      this.txtSnapshots[3] = this.txtSnapshot4;
      this.txtSnapshots[4] = this.txtSnapshot5;
      this.txtSnapshots[5] = this.txtSnapshot6;
      this.txtSnapshots[6] = this.txtSnapshot7;
      this.txtSnapshots[7] = this.txtSnapshot8;
      this.txtSnapshots[8] = this.txtSnapshot9;
      this.picSnapshots = new PictureBox[9];
      this.picSnapshots[0] = this.picSnapshot1;
      this.picSnapshots[1] = this.picSnapshot2;
      this.picSnapshots[2] = this.picSnapshot3;
      this.picSnapshots[3] = this.picSnapshot4;
      this.picSnapshots[4] = this.picSnapshot5;
      this.picSnapshots[5] = this.picSnapshot6;
      this.picSnapshots[6] = this.picSnapshot7;
      this.picSnapshots[7] = this.picSnapshot8;
      this.picSnapshots[8] = this.picSnapshot9;
      this.cboTimeFrames = new ComboBox[9];
      this.cboTimeFrames[0] = this.cboTimeFrame1;
      this.cboTimeFrames[1] = this.cboTimeFrame2;
      this.cboTimeFrames[2] = this.cboTimeFrame3;
      this.cboTimeFrames[3] = this.cboTimeFrame4;
      this.cboTimeFrames[4] = this.cboTimeFrame5;
      this.cboTimeFrames[5] = this.cboTimeFrame6;
      this.cboTimeFrames[6] = this.cboTimeFrame7;
      this.cboTimeFrames[7] = this.cboTimeFrame8;
      this.cboTimeFrames[8] = this.cboTimeFrame9;
    }

    private void setReadOnlyMode()
    {
      this.ctlViewSelection.Enabled = false;
      this.pnlSelectLayout.Enabled = false;
      this.pnlSelectSnapshot.Enabled = false;
      foreach (PictureBox picSnapshot in this.picSnapshots)
        picSnapshot.Image = this.imgLstSearch.Images["picSearchDisabled"];
    }

    private void updateUIFromView(DashboardView startFromView)
    {
      if (string.Empty != this.dashboardView.ViewName)
        this.txtName.Text = this.dashboardView.ViewName;
      DashboardView dashboardView = startFromView == null ? this.dashboardView : startFromView;
      this.picLayout_Click((object) this.picLayouts[dashboardView.LayoutId - 1], EventArgs.Empty);
      for (int index = 0; index < this.txtSnapshots.Length; ++index)
      {
        if (index < dashboardView.ReportCollection.Count)
        {
          DashboardLayoutForm.SnapshotTag tag = new DashboardLayoutForm.SnapshotTag(dashboardView.ReportCollection[index].ReportName, dashboardView.ReportCollection[index].DashboardTemplatePath, dashboardView.ReportCollection[index].ReportParameters);
          this.txtSnapshots[index].Tag = (object) tag;
          this.txtSnapshots[index].Text = tag.ReportName;
          this.updateCboFromView(tag, this.cboTimeFrames[index]);
        }
        else
        {
          this.txtSnapshots[index].Tag = (object) null;
          this.txtSnapshots[index].Text = string.Empty;
        }
      }
      if (DashboardLayoutForm.Action.EDIT_VIEW != this.action || this.currentDefaultViewId != this.dashboardView.ViewId)
        return;
      this.chkDefaultView.Checked = true;
    }

    private void updateCboFromView(DashboardLayoutForm.SnapshotTag tag, ComboBox control)
    {
      DashboardTemplate dashboardTemplate = new DashboardIFSExplorer().LoadDashboardTemplate(FileSystemEntry.Parse(tag.TemplatePath));
      control.SelectedIndexChanged -= new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      switch (dashboardTemplate.ChartType)
      {
        case DashboardChartType.BarChart:
        case DashboardChartType.LoanTable:
        case DashboardChartType.UserTable:
          control.DataSource = SelectionOptions.TimeFrameOptions.Clone();
          break;
        case DashboardChartType.TrendChart:
          control.DataSource = SelectionOptions.TimePeriodOptions.Clone();
          break;
      }
      control.DisplayMember = "Name";
      control.ValueMember = "Id";
      int num = 0;
      if (1 == tag.ReportParameters.Length && tag.ReportParameters[0] != "")
      {
        num = int.Parse(tag.ReportParameters[0]);
      }
      else
      {
        switch (dashboardTemplate.ChartType)
        {
          case DashboardChartType.BarChart:
          case DashboardChartType.LoanTable:
          case DashboardChartType.UserTable:
            num = 9;
            break;
          case DashboardChartType.TrendChart:
            num = 4;
            break;
        }
      }
      control.SelectedValue = (object) num;
      control.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
    }

    private bool updateViewFromUI()
    {
      string viewName = this.txtName.Text.Trim();
      if (this.action == DashboardLayoutForm.Action.CREATE_VIEW)
      {
        if (string.Empty == viewName)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please specify a name for this Dashboard View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtName.Focus();
          return false;
        }
        if (this.isReservedName(viewName))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "'" + viewName + "' is a reserved name. Please specify another name for this Dashboard View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtName.Focus();
          return false;
        }
      }
      DashboardLayout dashboardLayout = this.dashboardLayouts.Find(this.picSelectedLayout.Tag.ToString());
      DashboardReport[] dashboardReportArray = new DashboardReport[dashboardLayout.LayoutBlocks.Length];
      for (int index = 0; index < dashboardLayout.LayoutBlocks.Length; ++index)
      {
        if (!(this.txtSnapshots[index].Tag is DashboardLayoutForm.SnapshotTag tag))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please specify a snapshot for each layout position.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtSnapshots[index].Focus();
          return false;
        }
        DashboardReport dashboardReport = DashboardReport.NewDashboardReport();
        dashboardReport.LayoutBlockNumber = index + 1;
        dashboardReport.DashboardTemplatePath = tag.TemplatePath;
        dashboardReport.ReportParameters = tag.ReportParameters;
        dashboardReportArray[index] = dashboardReport;
      }
      this.dashboardView.ViewName = viewName;
      this.dashboardView.DashboardLayout = dashboardLayout;
      this.dashboardView.ReportCollection.Clear();
      foreach (DashboardReport dashboardReport in dashboardReportArray)
        this.dashboardView.ReportCollection.Add(dashboardReport);
      return true;
    }

    private void setViewModified(bool value)
    {
      this.isViewModified = value;
      this.btnSave.Enabled = value;
    }

    private bool isReservedName(string viewName)
    {
      if (this.dashboardSettings == null)
        this.dashboardSettings = Session.ServerManager.GetServerSettings("Dashboard");
      foreach (object key in (IEnumerable) this.dashboardSettings.Keys)
      {
        if (key.ToString().StartsWith("Dashboard.ReservedViewName", StringComparison.OrdinalIgnoreCase) && string.Equals(this.dashboardSettings[key].ToString(), viewName, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    private bool saveView()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.dashboardView = (DashboardView) this.dashboardView.Save();
      }
      catch (Exception ex)
      {
        if (0 <= ex.Message.IndexOf("Violation of UNIQUE KEY constraint 'UK_DashboardView_ViewName'"))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The name you specified already exists.\nPlease specify a different name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtName.Focus();
          return false;
        }
        throw;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      return true;
    }

    private void setDefaultView()
    {
      if (this.chkDefaultView.Checked && this.currentDefaultViewId != this.dashboardView.ViewId)
      {
        Session.WritePrivateProfileString("Dashboard", "Dashboard.DefaultViewId", this.dashboardView.ViewId.ToString());
      }
      else
      {
        if (this.chkDefaultView.Checked || this.currentDefaultViewId != this.dashboardView.ViewId)
          return;
        Session.WritePrivateProfileString("Dashboard", "Dashboard.DefaultViewId", string.Empty);
      }
    }

    private void ctlViewSelection_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
    {
      this.updateUIFromView(this.viewList.Find(e.ViewId));
      this.setViewModified(true);
    }

    private void picLayout_Click(object sender, EventArgs e)
    {
      if (this.picSelectedLayout != null)
      {
        this.picSelectedLayout.Parent.BackColor = SystemColors.Control;
        this.picSelectedLayout.Image = this.imglstLayouts.Images[this.picSelectedLayout.Tag.ToString()];
      }
      this.picSelectedLayout = (PictureBox) sender;
      int length = this.dashboardLayouts.Find(this.picSelectedLayout.Tag.ToString()).LayoutBlocks.Length;
      for (int index = 0; index < 9; ++index)
      {
        bool flag = index < length;
        this.lblSnapshots[index].Visible = flag;
        this.txtSnapshots[index].Visible = flag;
        this.picSnapshots[index].Visible = flag;
        this.cboTimeFrames[index].Visible = flag;
        if (flag && this.txtSnapshots[index].Text == "")
          this.cboTimeFrames[index].Enabled = false;
      }
      this.picSelectedLayout.Parent.BackColor = Color.FromArgb((int) byte.MaxValue, 156, 0);
      this.picSelectedLayout.Image = this.imglstLayouts.Images[this.picSelectedLayout.Tag.ToString() + "Ovr"];
      this.setViewModified(true);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.isViewModified && this.dashboardView.IsViewReadWrite && (!this.updateViewFromUI() || !this.saveView()))
        return;
      this.setDefaultView();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void picLayout_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string key = pictureBox.Tag.ToString() + "Ovr";
      pictureBox.Image = this.imglstLayouts.Images[key];
    }

    private void picLayout_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string key = pictureBox.Tag.ToString();
      if (!(key != this.picSelectedLayout.Tag.ToString()))
        return;
      pictureBox.Image = this.imglstLayouts.Images[key];
    }

    private void picSnapshot_Click(object sender, EventArgs e)
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.SelectTemplate))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog())
          return;
        DashboardTemplate selectedTemplate = templateFormDialog.SelectedTemplate;
        if (selectedTemplate == null)
          return;
        FileSystemEntry selectedFileSystemEntry = templateFormDialog.SelectedFileSystemEntry;
        DashboardLayoutForm.SnapshotTag tag = new DashboardLayoutForm.SnapshotTag(selectedTemplate.TemplateName, selectedFileSystemEntry.ToString(), new string[0]);
        int int32 = Convert.ToInt32(((Control) sender).Tag);
        this.txtSnapshots[int32].Tag = (object) tag;
        this.txtSnapshots[int32].Text = tag.ReportName;
        this.cboTimeFrames[int32].Enabled = true;
        this.updateCboFromView(tag, this.cboTimeFrames[int32]);
        this.setViewModified(true);
      }
    }

    private void picSnapshot_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgLstSearch.Images["picSearchMouseOver"];
    }

    private void picSnapshot_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgLstSearch.Images["picSearch"];
    }

    private void chkDefaultView_Click(object sender, EventArgs e) => this.btnSave.Enabled = true;

    private void cboTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(((Control) sender).Tag);
      DashboardLayoutForm.SnapshotTag tag = (DashboardLayoutForm.SnapshotTag) this.txtSnapshots[int32].Tag;
      if (tag == null)
        return;
      tag.ReportParameters = new string[1]
      {
        string.Concat(((ListControl) sender).SelectedValue)
      };
      this.txtSnapshots[int32].Tag = (object) tag;
      this.setViewModified(true);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DashboardLayoutForm));
      this.lblName = new Label();
      this.txtName = new TextBox();
      this.lblHorizontal1 = new Label();
      this.lblHorizontal2 = new Label();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.lblSelectLayout = new Label();
      this.lblSelectSnapshot = new Label();
      this.picLayout1 = new PictureBox();
      this.picLayout2 = new PictureBox();
      this.picLayout3 = new PictureBox();
      this.picLayout4 = new PictureBox();
      this.picLayout6 = new PictureBox();
      this.picLayout5 = new PictureBox();
      this.picLayout7 = new PictureBox();
      this.picLayout9 = new PictureBox();
      this.picLayout8 = new PictureBox();
      this.picLayout10 = new PictureBox();
      this.picLayout12 = new PictureBox();
      this.picLayout11 = new PictureBox();
      this.picLayout15 = new PictureBox();
      this.picLayout14 = new PictureBox();
      this.picLayout13 = new PictureBox();
      this.picLayout18 = new PictureBox();
      this.lblVertical1 = new Label();
      this.lblSnapshot1 = new Label();
      this.lblSnapshot2 = new Label();
      this.lblSnapshot3 = new Label();
      this.lblSnapshot4 = new Label();
      this.lblSnapshot5 = new Label();
      this.lblSnapshot6 = new Label();
      this.lblSnapshot7 = new Label();
      this.lblSnapshot8 = new Label();
      this.lblSnapshot9 = new Label();
      this.txtSnapshot1 = new TextBox();
      this.txtSnapshot2 = new TextBox();
      this.txtSnapshot3 = new TextBox();
      this.txtSnapshot4 = new TextBox();
      this.txtSnapshot5 = new TextBox();
      this.txtSnapshot6 = new TextBox();
      this.txtSnapshot7 = new TextBox();
      this.txtSnapshot8 = new TextBox();
      this.txtSnapshot9 = new TextBox();
      this.picSnapshot1 = new PictureBox();
      this.picSnapshot2 = new PictureBox();
      this.picSnapshot3 = new PictureBox();
      this.picSnapshot4 = new PictureBox();
      this.picSnapshot5 = new PictureBox();
      this.picSnapshot6 = new PictureBox();
      this.picSnapshot7 = new PictureBox();
      this.picSnapshot8 = new PictureBox();
      this.picSnapshot9 = new PictureBox();
      this.picLayout16 = new PictureBox();
      this.picLayout17 = new PictureBox();
      this.pnlLayout18 = new Panel();
      this.pnlLayout1 = new Panel();
      this.pnlLayout2 = new Panel();
      this.pnlLayout3 = new Panel();
      this.pnlLayout17 = new Panel();
      this.pnlLayout4 = new Panel();
      this.pnlLayout5 = new Panel();
      this.pnlLayout6 = new Panel();
      this.pnlLayout7 = new Panel();
      this.pnlLayout8 = new Panel();
      this.pnlLayout9 = new Panel();
      this.pnlLayout10 = new Panel();
      this.pnlLayout11 = new Panel();
      this.pnlLayout12 = new Panel();
      this.pnlLayout13 = new Panel();
      this.pnlLayout14 = new Panel();
      this.pnlLayout15 = new Panel();
      this.pnlLayout16 = new Panel();
      this.imglstLayouts = new ImageList(this.components);
      this.imgLstSearch = new ImageList(this.components);
      this.chkDefaultView = new CheckBox();
      this.lblStartFrom = new Label();
      this.tipToolTip = new ToolTip(this.components);
      this.pnlSelectLayout = new Panel();
      this.pnlSelectSnapshot = new Panel();
      this.cboTimeFrame9 = new ComboBox();
      this.cboTimeFrame8 = new ComboBox();
      this.cboTimeFrame7 = new ComboBox();
      this.cboTimeFrame6 = new ComboBox();
      this.cboTimeFrame5 = new ComboBox();
      this.cboTimeFrame4 = new ComboBox();
      this.cboTimeFrame3 = new ComboBox();
      this.cboTimeFrame2 = new ComboBox();
      this.cboTimeFrame1 = new ComboBox();
      this.ctlViewSelection = new ViewSelectionControl();
      ((ISupportInitialize) this.picLayout1).BeginInit();
      ((ISupportInitialize) this.picLayout2).BeginInit();
      ((ISupportInitialize) this.picLayout3).BeginInit();
      ((ISupportInitialize) this.picLayout4).BeginInit();
      ((ISupportInitialize) this.picLayout6).BeginInit();
      ((ISupportInitialize) this.picLayout5).BeginInit();
      ((ISupportInitialize) this.picLayout7).BeginInit();
      ((ISupportInitialize) this.picLayout9).BeginInit();
      ((ISupportInitialize) this.picLayout8).BeginInit();
      ((ISupportInitialize) this.picLayout10).BeginInit();
      ((ISupportInitialize) this.picLayout12).BeginInit();
      ((ISupportInitialize) this.picLayout11).BeginInit();
      ((ISupportInitialize) this.picLayout15).BeginInit();
      ((ISupportInitialize) this.picLayout14).BeginInit();
      ((ISupportInitialize) this.picLayout13).BeginInit();
      ((ISupportInitialize) this.picLayout18).BeginInit();
      ((ISupportInitialize) this.picSnapshot1).BeginInit();
      ((ISupportInitialize) this.picSnapshot2).BeginInit();
      ((ISupportInitialize) this.picSnapshot3).BeginInit();
      ((ISupportInitialize) this.picSnapshot4).BeginInit();
      ((ISupportInitialize) this.picSnapshot5).BeginInit();
      ((ISupportInitialize) this.picSnapshot6).BeginInit();
      ((ISupportInitialize) this.picSnapshot7).BeginInit();
      ((ISupportInitialize) this.picSnapshot8).BeginInit();
      ((ISupportInitialize) this.picSnapshot9).BeginInit();
      ((ISupportInitialize) this.picLayout16).BeginInit();
      ((ISupportInitialize) this.picLayout17).BeginInit();
      this.pnlLayout18.SuspendLayout();
      this.pnlLayout1.SuspendLayout();
      this.pnlLayout2.SuspendLayout();
      this.pnlLayout3.SuspendLayout();
      this.pnlLayout17.SuspendLayout();
      this.pnlLayout4.SuspendLayout();
      this.pnlLayout5.SuspendLayout();
      this.pnlLayout6.SuspendLayout();
      this.pnlLayout7.SuspendLayout();
      this.pnlLayout8.SuspendLayout();
      this.pnlLayout9.SuspendLayout();
      this.pnlLayout10.SuspendLayout();
      this.pnlLayout11.SuspendLayout();
      this.pnlLayout12.SuspendLayout();
      this.pnlLayout13.SuspendLayout();
      this.pnlLayout14.SuspendLayout();
      this.pnlLayout15.SuspendLayout();
      this.pnlLayout16.SuspendLayout();
      this.pnlSelectLayout.SuspendLayout();
      this.pnlSelectSnapshot.SuspendLayout();
      this.SuspendLayout();
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(13, 17);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(38, 13);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name:";
      this.lblName.TextAlign = ContentAlignment.MiddleLeft;
      this.txtName.Location = new Point(57, 13);
      this.txtName.MaxLength = 50;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(238, 20);
      this.txtName.TabIndex = 0;
      this.lblHorizontal1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHorizontal1.BorderStyle = BorderStyle.FixedSingle;
      this.lblHorizontal1.Location = new Point(12, 44);
      this.lblHorizontal1.Name = "lblHorizontal1";
      this.lblHorizontal1.Size = new Size(621, 1);
      this.lblHorizontal1.TabIndex = 2;
      this.lblHorizontal1.Text = "label1";
      this.lblHorizontal2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHorizontal2.BorderStyle = BorderStyle.FixedSingle;
      this.lblHorizontal2.Location = new Point(13, 602);
      this.lblHorizontal2.Name = "lblHorizontal2";
      this.lblHorizontal2.Size = new Size(620, 1);
      this.lblHorizontal2.TabIndex = 3;
      this.lblHorizontal2.Text = "label2";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(557, 617);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(476, 617);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.lblSelectLayout.AutoSize = true;
      this.lblSelectLayout.Location = new Point(0, 0);
      this.lblSelectLayout.Name = "lblSelectLayout";
      this.lblSelectLayout.Size = new Size(87, 13);
      this.lblSelectLayout.TabIndex = 7;
      this.lblSelectLayout.Text = "1. Select Layout:";
      this.lblSelectLayout.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSelectSnapshot.AutoSize = true;
      this.lblSelectSnapshot.Location = new Point(0, 0);
      this.lblSelectSnapshot.Name = "lblSelectSnapshot";
      this.lblSelectSnapshot.Size = new Size(188, 13);
      this.lblSelectSnapshot.TabIndex = 8;
      this.lblSelectSnapshot.Text = "2. Select a Snapshot for each module:";
      this.lblSelectSnapshot.TextAlign = ContentAlignment.MiddleLeft;
      this.picLayout1.BackColor = SystemColors.Control;
      this.picLayout1.Image = (Image) componentResourceManager.GetObject("picLayout1.Image");
      this.picLayout1.Location = new Point(2, 2);
      this.picLayout1.Name = "picLayout1";
      this.picLayout1.Size = new Size(84, 60);
      this.picLayout1.TabIndex = 34;
      this.picLayout1.TabStop = false;
      this.picLayout1.Tag = (object) "C1R1";
      this.picLayout1.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout1.Click += new EventHandler(this.picLayout_Click);
      this.picLayout1.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout2.BackColor = SystemColors.Control;
      this.picLayout2.Image = (Image) componentResourceManager.GetObject("picLayout2.Image");
      this.picLayout2.Location = new Point(2, 2);
      this.picLayout2.Name = "picLayout2";
      this.picLayout2.Size = new Size(84, 60);
      this.picLayout2.TabIndex = 35;
      this.picLayout2.TabStop = false;
      this.picLayout2.Tag = (object) "C1R2";
      this.picLayout2.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout2.Click += new EventHandler(this.picLayout_Click);
      this.picLayout2.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout3.BackColor = SystemColors.Control;
      this.picLayout3.Image = (Image) componentResourceManager.GetObject("picLayout3.Image");
      this.picLayout3.Location = new Point(2, 2);
      this.picLayout3.Name = "picLayout3";
      this.picLayout3.Size = new Size(84, 60);
      this.picLayout3.TabIndex = 38;
      this.picLayout3.TabStop = false;
      this.picLayout3.Tag = (object) "C2R1R1";
      this.picLayout3.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout3.Click += new EventHandler(this.picLayout_Click);
      this.picLayout3.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout4.BackColor = SystemColors.Control;
      this.picLayout4.Image = (Image) componentResourceManager.GetObject("picLayout4.Image");
      this.picLayout4.Location = new Point(2, 2);
      this.picLayout4.Name = "picLayout4";
      this.picLayout4.Size = new Size(84, 60);
      this.picLayout4.TabIndex = 39;
      this.picLayout4.TabStop = false;
      this.picLayout4.Tag = (object) "C2R2R2";
      this.picLayout4.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout4.Click += new EventHandler(this.picLayout_Click);
      this.picLayout4.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout6.BackColor = SystemColors.Control;
      this.picLayout6.Image = (Image) componentResourceManager.GetObject("picLayout6.Image");
      this.picLayout6.Location = new Point(2, 2);
      this.picLayout6.Name = "picLayout6";
      this.picLayout6.Size = new Size(84, 60);
      this.picLayout6.TabIndex = 46;
      this.picLayout6.TabStop = false;
      this.picLayout6.Tag = (object) "C2R2R2W6";
      this.picLayout6.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout6.Click += new EventHandler(this.picLayout_Click);
      this.picLayout6.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout5.BackColor = SystemColors.Control;
      this.picLayout5.Image = (Image) componentResourceManager.GetObject("picLayout5.Image");
      this.picLayout5.Location = new Point(2, 2);
      this.picLayout5.Name = "picLayout5";
      this.picLayout5.Size = new Size(84, 60);
      this.picLayout5.TabIndex = 45;
      this.picLayout5.TabStop = false;
      this.picLayout5.Tag = (object) "C2R2R2W4";
      this.picLayout5.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout5.Click += new EventHandler(this.picLayout_Click);
      this.picLayout5.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout7.BackColor = SystemColors.Control;
      this.picLayout7.Image = (Image) componentResourceManager.GetObject("picLayout7.Image");
      this.picLayout7.Location = new Point(2, 2);
      this.picLayout7.Name = "picLayout7";
      this.picLayout7.Size = new Size(84, 60);
      this.picLayout7.TabIndex = 52;
      this.picLayout7.TabStop = false;
      this.picLayout7.Tag = (object) "C2R3R2";
      this.picLayout7.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout7.Click += new EventHandler(this.picLayout_Click);
      this.picLayout7.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout9.BackColor = SystemColors.Control;
      this.picLayout9.Image = (Image) componentResourceManager.GetObject("picLayout9.Image");
      this.picLayout9.Location = new Point(2, 2);
      this.picLayout9.Name = "picLayout9";
      this.picLayout9.Size = new Size(84, 60);
      this.picLayout9.TabIndex = 51;
      this.picLayout9.TabStop = false;
      this.picLayout9.Tag = (object) "C2R3R2W6";
      this.picLayout9.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout9.Click += new EventHandler(this.picLayout_Click);
      this.picLayout9.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout8.BackColor = SystemColors.Control;
      this.picLayout8.Image = (Image) componentResourceManager.GetObject("picLayout8.Image");
      this.picLayout8.Location = new Point(2, 2);
      this.picLayout8.Name = "picLayout8";
      this.picLayout8.Size = new Size(84, 60);
      this.picLayout8.TabIndex = 50;
      this.picLayout8.TabStop = false;
      this.picLayout8.Tag = (object) "C2R3R2W4";
      this.picLayout8.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout8.Click += new EventHandler(this.picLayout_Click);
      this.picLayout8.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout10.BackColor = SystemColors.Control;
      this.picLayout10.Image = (Image) componentResourceManager.GetObject("picLayout10.Image");
      this.picLayout10.Location = new Point(2, 2);
      this.picLayout10.Name = "picLayout10";
      this.picLayout10.Size = new Size(84, 60);
      this.picLayout10.TabIndex = 55;
      this.picLayout10.TabStop = false;
      this.picLayout10.Tag = (object) "C2R2R3";
      this.picLayout10.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout10.Click += new EventHandler(this.picLayout_Click);
      this.picLayout10.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout12.BackColor = SystemColors.Control;
      this.picLayout12.Image = (Image) componentResourceManager.GetObject("picLayout12.Image");
      this.picLayout12.Location = new Point(2, 2);
      this.picLayout12.Name = "picLayout12";
      this.picLayout12.Size = new Size(84, 60);
      this.picLayout12.TabIndex = 54;
      this.picLayout12.TabStop = false;
      this.picLayout12.Tag = (object) "C2R2R3W6";
      this.picLayout12.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout12.Click += new EventHandler(this.picLayout_Click);
      this.picLayout12.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout11.BackColor = SystemColors.Control;
      this.picLayout11.Image = (Image) componentResourceManager.GetObject("picLayout11.Image");
      this.picLayout11.Location = new Point(2, 2);
      this.picLayout11.Name = "picLayout11";
      this.picLayout11.Size = new Size(84, 60);
      this.picLayout11.TabIndex = 53;
      this.picLayout11.TabStop = false;
      this.picLayout11.Tag = (object) "C2R2R3W4";
      this.picLayout11.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout11.Click += new EventHandler(this.picLayout_Click);
      this.picLayout11.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout15.BackColor = SystemColors.Control;
      this.picLayout15.Image = (Image) componentResourceManager.GetObject("picLayout15.Image");
      this.picLayout15.Location = new Point(2, 2);
      this.picLayout15.Name = "picLayout15";
      this.picLayout15.Size = new Size(84, 60);
      this.picLayout15.TabIndex = 58;
      this.picLayout15.TabStop = false;
      this.picLayout15.Tag = (object) "C2R3R3W6";
      this.picLayout15.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout15.Click += new EventHandler(this.picLayout_Click);
      this.picLayout15.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout14.BackColor = SystemColors.Control;
      this.picLayout14.Image = (Image) componentResourceManager.GetObject("picLayout14.Image");
      this.picLayout14.Location = new Point(2, 2);
      this.picLayout14.Name = "picLayout14";
      this.picLayout14.Size = new Size(84, 60);
      this.picLayout14.TabIndex = 57;
      this.picLayout14.TabStop = false;
      this.picLayout14.Tag = (object) "C2R3R3W4";
      this.picLayout14.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout14.Click += new EventHandler(this.picLayout_Click);
      this.picLayout14.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout13.BackColor = SystemColors.Control;
      this.picLayout13.Image = (Image) componentResourceManager.GetObject("picLayout13.Image");
      this.picLayout13.Location = new Point(2, 2);
      this.picLayout13.Name = "picLayout13";
      this.picLayout13.Size = new Size(84, 60);
      this.picLayout13.TabIndex = 56;
      this.picLayout13.TabStop = false;
      this.picLayout13.Tag = (object) "C2R3R3";
      this.picLayout13.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout13.Click += new EventHandler(this.picLayout_Click);
      this.picLayout13.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout18.BackColor = SystemColors.Control;
      this.picLayout18.Image = (Image) componentResourceManager.GetObject("picLayout18.Image");
      this.picLayout18.ImageLocation = "";
      this.picLayout18.Location = new Point(2, 2);
      this.picLayout18.Name = "picLayout18";
      this.picLayout18.Size = new Size(84, 60);
      this.picLayout18.TabIndex = 59;
      this.picLayout18.TabStop = false;
      this.picLayout18.Tag = (object) "C3R3R3R3";
      this.picLayout18.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout18.Click += new EventHandler(this.picLayout_Click);
      this.picLayout18.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.lblVertical1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblVertical1.BorderStyle = BorderStyle.FixedSingle;
      this.lblVertical1.Location = new Point(315, 62);
      this.lblVertical1.Name = "lblVertical1";
      this.lblVertical1.Size = new Size(1, 524);
      this.lblVertical1.TabIndex = 60;
      this.lblSnapshot1.AutoSize = true;
      this.lblSnapshot1.Location = new Point(1, 31);
      this.lblSnapshot1.Name = "lblSnapshot1";
      this.lblSnapshot1.Size = new Size(64, 13);
      this.lblSnapshot1.TabIndex = 19;
      this.lblSnapshot1.Text = "Snapshot 1:";
      this.lblSnapshot1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot2.AutoSize = true;
      this.lblSnapshot2.Location = new Point(1, 85);
      this.lblSnapshot2.Name = "lblSnapshot2";
      this.lblSnapshot2.Size = new Size(64, 13);
      this.lblSnapshot2.TabIndex = 20;
      this.lblSnapshot2.Text = "Snapshot 2:";
      this.lblSnapshot2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot3.AutoSize = true;
      this.lblSnapshot3.Location = new Point(1, 138);
      this.lblSnapshot3.Name = "lblSnapshot3";
      this.lblSnapshot3.Size = new Size(64, 13);
      this.lblSnapshot3.TabIndex = 21;
      this.lblSnapshot3.Text = "Snapshot 3:";
      this.lblSnapshot3.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot4.AutoSize = true;
      this.lblSnapshot4.Location = new Point(1, 191);
      this.lblSnapshot4.Name = "lblSnapshot4";
      this.lblSnapshot4.Size = new Size(64, 13);
      this.lblSnapshot4.TabIndex = 22;
      this.lblSnapshot4.Text = "Snapshot 4:";
      this.lblSnapshot4.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot5.AutoSize = true;
      this.lblSnapshot5.Location = new Point(1, 244);
      this.lblSnapshot5.Name = "lblSnapshot5";
      this.lblSnapshot5.Size = new Size(64, 13);
      this.lblSnapshot5.TabIndex = 23;
      this.lblSnapshot5.Text = "Snapshot 5:";
      this.lblSnapshot5.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot6.AutoSize = true;
      this.lblSnapshot6.Location = new Point(1, 297);
      this.lblSnapshot6.Name = "lblSnapshot6";
      this.lblSnapshot6.Size = new Size(64, 13);
      this.lblSnapshot6.TabIndex = 24;
      this.lblSnapshot6.Text = "Snapshot 6:";
      this.lblSnapshot6.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot7.AutoSize = true;
      this.lblSnapshot7.Location = new Point(1, 350);
      this.lblSnapshot7.Name = "lblSnapshot7";
      this.lblSnapshot7.Size = new Size(64, 13);
      this.lblSnapshot7.TabIndex = 25;
      this.lblSnapshot7.Text = "Snapshot 7:";
      this.lblSnapshot7.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot8.AutoSize = true;
      this.lblSnapshot8.Location = new Point(1, 403);
      this.lblSnapshot8.Name = "lblSnapshot8";
      this.lblSnapshot8.Size = new Size(64, 13);
      this.lblSnapshot8.TabIndex = 26;
      this.lblSnapshot8.Text = "Snapshot 8:";
      this.lblSnapshot8.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot9.AutoSize = true;
      this.lblSnapshot9.Location = new Point(1, 456);
      this.lblSnapshot9.Name = "lblSnapshot9";
      this.lblSnapshot9.Size = new Size(64, 13);
      this.lblSnapshot9.TabIndex = 27;
      this.lblSnapshot9.Text = "Snapshot 9:";
      this.lblSnapshot9.TextAlign = ContentAlignment.MiddleLeft;
      this.txtSnapshot1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot1.Location = new Point(72, 27);
      this.txtSnapshot1.Name = "txtSnapshot1";
      this.txtSnapshot1.ReadOnly = true;
      this.txtSnapshot1.Size = new Size(199, 20);
      this.txtSnapshot1.TabIndex = 70;
      this.txtSnapshot1.Tag = (object) "";
      this.txtSnapshot2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot2.Location = new Point(72, 80);
      this.txtSnapshot2.Name = "txtSnapshot2";
      this.txtSnapshot2.ReadOnly = true;
      this.txtSnapshot2.Size = new Size(199, 20);
      this.txtSnapshot2.TabIndex = 71;
      this.txtSnapshot2.Tag = (object) "";
      this.txtSnapshot3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot3.Location = new Point(72, 133);
      this.txtSnapshot3.Name = "txtSnapshot3";
      this.txtSnapshot3.ReadOnly = true;
      this.txtSnapshot3.Size = new Size(199, 20);
      this.txtSnapshot3.TabIndex = 72;
      this.txtSnapshot3.Tag = (object) "";
      this.txtSnapshot4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot4.Location = new Point(72, 186);
      this.txtSnapshot4.Name = "txtSnapshot4";
      this.txtSnapshot4.ReadOnly = true;
      this.txtSnapshot4.Size = new Size(199, 20);
      this.txtSnapshot4.TabIndex = 73;
      this.txtSnapshot4.Tag = (object) "";
      this.txtSnapshot5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot5.Location = new Point(72, 239);
      this.txtSnapshot5.Name = "txtSnapshot5";
      this.txtSnapshot5.ReadOnly = true;
      this.txtSnapshot5.Size = new Size(199, 20);
      this.txtSnapshot5.TabIndex = 74;
      this.txtSnapshot5.Tag = (object) "";
      this.txtSnapshot6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot6.Location = new Point(72, 292);
      this.txtSnapshot6.Name = "txtSnapshot6";
      this.txtSnapshot6.ReadOnly = true;
      this.txtSnapshot6.Size = new Size(199, 20);
      this.txtSnapshot6.TabIndex = 75;
      this.txtSnapshot6.Tag = (object) "";
      this.txtSnapshot7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot7.Location = new Point(72, 345);
      this.txtSnapshot7.Name = "txtSnapshot7";
      this.txtSnapshot7.ReadOnly = true;
      this.txtSnapshot7.Size = new Size(199, 20);
      this.txtSnapshot7.TabIndex = 76;
      this.txtSnapshot7.Tag = (object) "";
      this.txtSnapshot8.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot8.Location = new Point(72, 398);
      this.txtSnapshot8.Name = "txtSnapshot8";
      this.txtSnapshot8.ReadOnly = true;
      this.txtSnapshot8.Size = new Size(199, 20);
      this.txtSnapshot8.TabIndex = 77;
      this.txtSnapshot8.Tag = (object) "";
      this.txtSnapshot9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot9.Location = new Point(72, 451);
      this.txtSnapshot9.Name = "txtSnapshot9";
      this.txtSnapshot9.ReadOnly = true;
      this.txtSnapshot9.Size = new Size(199, 20);
      this.txtSnapshot9.TabIndex = 78;
      this.txtSnapshot9.Tag = (object) "";
      this.picSnapshot1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot1.Image = (Image) componentResourceManager.GetObject("picSnapshot1.Image");
      this.picSnapshot1.Location = new Point(277, 29);
      this.picSnapshot1.Name = "picSnapshot1";
      this.picSnapshot1.Size = new Size(16, 16);
      this.picSnapshot1.TabIndex = 416;
      this.picSnapshot1.TabStop = false;
      this.picSnapshot1.Tag = (object) "0";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot1, "Select...");
      this.picSnapshot1.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot1.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot1.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot2.Image = (Image) componentResourceManager.GetObject("picSnapshot2.Image");
      this.picSnapshot2.Location = new Point(277, 82);
      this.picSnapshot2.Name = "picSnapshot2";
      this.picSnapshot2.Size = new Size(16, 16);
      this.picSnapshot2.TabIndex = 417;
      this.picSnapshot2.TabStop = false;
      this.picSnapshot2.Tag = (object) "1";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot2, "Select...");
      this.picSnapshot2.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot2.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot2.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot3.Image = (Image) componentResourceManager.GetObject("picSnapshot3.Image");
      this.picSnapshot3.Location = new Point(277, 135);
      this.picSnapshot3.Name = "picSnapshot3";
      this.picSnapshot3.Size = new Size(16, 16);
      this.picSnapshot3.TabIndex = 418;
      this.picSnapshot3.TabStop = false;
      this.picSnapshot3.Tag = (object) "2";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot3, "Select...");
      this.picSnapshot3.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot3.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot3.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot4.Image = (Image) componentResourceManager.GetObject("picSnapshot4.Image");
      this.picSnapshot4.Location = new Point(277, 188);
      this.picSnapshot4.Name = "picSnapshot4";
      this.picSnapshot4.Size = new Size(16, 16);
      this.picSnapshot4.TabIndex = 419;
      this.picSnapshot4.TabStop = false;
      this.picSnapshot4.Tag = (object) "3";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot4, "Select...");
      this.picSnapshot4.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot4.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot4.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot5.Image = (Image) componentResourceManager.GetObject("picSnapshot5.Image");
      this.picSnapshot5.Location = new Point(277, 241);
      this.picSnapshot5.Name = "picSnapshot5";
      this.picSnapshot5.Size = new Size(16, 16);
      this.picSnapshot5.TabIndex = 420;
      this.picSnapshot5.TabStop = false;
      this.picSnapshot5.Tag = (object) "4";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot5, "Select...");
      this.picSnapshot5.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot5.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot5.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot6.Image = (Image) componentResourceManager.GetObject("picSnapshot6.Image");
      this.picSnapshot6.Location = new Point(277, 294);
      this.picSnapshot6.Name = "picSnapshot6";
      this.picSnapshot6.Size = new Size(16, 16);
      this.picSnapshot6.TabIndex = 421;
      this.picSnapshot6.TabStop = false;
      this.picSnapshot6.Tag = (object) "5";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot6, "Select...");
      this.picSnapshot6.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot6.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot6.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot7.Image = (Image) componentResourceManager.GetObject("picSnapshot7.Image");
      this.picSnapshot7.Location = new Point(277, 347);
      this.picSnapshot7.Name = "picSnapshot7";
      this.picSnapshot7.Size = new Size(16, 16);
      this.picSnapshot7.TabIndex = 422;
      this.picSnapshot7.TabStop = false;
      this.picSnapshot7.Tag = (object) "6";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot7, "Select...");
      this.picSnapshot7.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot7.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot7.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot8.Image = (Image) componentResourceManager.GetObject("picSnapshot8.Image");
      this.picSnapshot8.Location = new Point(277, 400);
      this.picSnapshot8.Name = "picSnapshot8";
      this.picSnapshot8.Size = new Size(16, 16);
      this.picSnapshot8.TabIndex = 423;
      this.picSnapshot8.TabStop = false;
      this.picSnapshot8.Tag = (object) "7";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot8, "Select...");
      this.picSnapshot8.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot8.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot8.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot9.Image = (Image) componentResourceManager.GetObject("picSnapshot9.Image");
      this.picSnapshot9.Location = new Point(277, 453);
      this.picSnapshot9.Name = "picSnapshot9";
      this.picSnapshot9.Size = new Size(16, 16);
      this.picSnapshot9.TabIndex = 424;
      this.picSnapshot9.TabStop = false;
      this.picSnapshot9.Tag = (object) "8";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot9, "Select...");
      this.picSnapshot9.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.picSnapshot9.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot9.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picLayout16.BackColor = SystemColors.Control;
      this.picLayout16.Image = (Image) componentResourceManager.GetObject("picLayout16.Image");
      this.picLayout16.Location = new Point(2, 2);
      this.picLayout16.Name = "picLayout16";
      this.picLayout16.Size = new Size(84, 60);
      this.picLayout16.TabIndex = 425;
      this.picLayout16.TabStop = false;
      this.picLayout16.Tag = (object) "C3R2R2R2";
      this.picLayout16.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout16.Click += new EventHandler(this.picLayout_Click);
      this.picLayout16.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout17.BackColor = SystemColors.Control;
      this.picLayout17.Image = (Image) componentResourceManager.GetObject("picLayout17.Image");
      this.picLayout17.InitialImage = (Image) componentResourceManager.GetObject("picLayout17.InitialImage");
      this.picLayout17.Location = new Point(2, 2);
      this.picLayout17.Name = "picLayout17";
      this.picLayout17.Size = new Size(84, 60);
      this.picLayout17.TabIndex = 426;
      this.picLayout17.TabStop = false;
      this.picLayout17.Tag = (object) "R2C2C3";
      this.picLayout17.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.picLayout17.Click += new EventHandler(this.picLayout_Click);
      this.picLayout17.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.pnlLayout18.BackColor = SystemColors.Control;
      this.pnlLayout18.Controls.Add((Control) this.picLayout18);
      this.pnlLayout18.Location = new Point(188, 377);
      this.pnlLayout18.Name = "pnlLayout18";
      this.pnlLayout18.Size = new Size(88, 64);
      this.pnlLayout18.TabIndex = 18;
      this.pnlLayout18.Tag = (object) "";
      this.pnlLayout1.BackColor = SystemColors.Control;
      this.pnlLayout1.Controls.Add((Control) this.picLayout1);
      this.pnlLayout1.Location = new Point(0, 27);
      this.pnlLayout1.Name = "pnlLayout1";
      this.pnlLayout1.Size = new Size(88, 64);
      this.pnlLayout1.TabIndex = 1;
      this.pnlLayout1.Tag = (object) "";
      this.pnlLayout2.BackColor = SystemColors.Control;
      this.pnlLayout2.Controls.Add((Control) this.picLayout2);
      this.pnlLayout2.Location = new Point(94, 27);
      this.pnlLayout2.Name = "pnlLayout2";
      this.pnlLayout2.Size = new Size(88, 64);
      this.pnlLayout2.TabIndex = 2;
      this.pnlLayout2.Tag = (object) "";
      this.pnlLayout3.BackColor = SystemColors.Control;
      this.pnlLayout3.Controls.Add((Control) this.picLayout3);
      this.pnlLayout3.Location = new Point(188, 27);
      this.pnlLayout3.Name = "pnlLayout3";
      this.pnlLayout3.Size = new Size(88, 64);
      this.pnlLayout3.TabIndex = 3;
      this.pnlLayout3.Tag = (object) "";
      this.pnlLayout17.Controls.Add((Control) this.picLayout17);
      this.pnlLayout17.Location = new Point(94, 377);
      this.pnlLayout17.Name = "pnlLayout17";
      this.pnlLayout17.Size = new Size(88, 64);
      this.pnlLayout17.TabIndex = 17;
      this.pnlLayout4.Controls.Add((Control) this.picLayout4);
      this.pnlLayout4.Location = new Point(0, 97);
      this.pnlLayout4.Name = "pnlLayout4";
      this.pnlLayout4.Size = new Size(88, 64);
      this.pnlLayout4.TabIndex = 4;
      this.pnlLayout4.Tag = (object) "";
      this.pnlLayout5.Controls.Add((Control) this.picLayout5);
      this.pnlLayout5.Location = new Point(94, 97);
      this.pnlLayout5.Name = "pnlLayout5";
      this.pnlLayout5.Size = new Size(88, 64);
      this.pnlLayout5.TabIndex = 5;
      this.pnlLayout5.Tag = (object) "";
      this.pnlLayout6.Controls.Add((Control) this.picLayout6);
      this.pnlLayout6.Location = new Point(188, 97);
      this.pnlLayout6.Name = "pnlLayout6";
      this.pnlLayout6.Size = new Size(88, 64);
      this.pnlLayout6.TabIndex = 6;
      this.pnlLayout6.Tag = (object) "";
      this.pnlLayout7.Controls.Add((Control) this.picLayout7);
      this.pnlLayout7.Location = new Point(0, 167);
      this.pnlLayout7.Name = "pnlLayout7";
      this.pnlLayout7.Size = new Size(88, 64);
      this.pnlLayout7.TabIndex = 7;
      this.pnlLayout8.Controls.Add((Control) this.picLayout8);
      this.pnlLayout8.Location = new Point(94, 167);
      this.pnlLayout8.Name = "pnlLayout8";
      this.pnlLayout8.Size = new Size(88, 64);
      this.pnlLayout8.TabIndex = 8;
      this.pnlLayout9.Controls.Add((Control) this.picLayout9);
      this.pnlLayout9.Location = new Point(188, 167);
      this.pnlLayout9.Name = "pnlLayout9";
      this.pnlLayout9.Size = new Size(88, 64);
      this.pnlLayout9.TabIndex = 9;
      this.pnlLayout10.Controls.Add((Control) this.picLayout10);
      this.pnlLayout10.Location = new Point(0, 237);
      this.pnlLayout10.Name = "pnlLayout10";
      this.pnlLayout10.Size = new Size(88, 64);
      this.pnlLayout10.TabIndex = 10;
      this.pnlLayout11.Controls.Add((Control) this.picLayout11);
      this.pnlLayout11.Location = new Point(94, 237);
      this.pnlLayout11.Name = "pnlLayout11";
      this.pnlLayout11.Size = new Size(88, 64);
      this.pnlLayout11.TabIndex = 11;
      this.pnlLayout12.Controls.Add((Control) this.picLayout12);
      this.pnlLayout12.Location = new Point(188, 237);
      this.pnlLayout12.Name = "pnlLayout12";
      this.pnlLayout12.Size = new Size(88, 64);
      this.pnlLayout12.TabIndex = 12;
      this.pnlLayout13.Controls.Add((Control) this.picLayout13);
      this.pnlLayout13.Location = new Point(0, 307);
      this.pnlLayout13.Name = "pnlLayout13";
      this.pnlLayout13.Size = new Size(88, 64);
      this.pnlLayout13.TabIndex = 13;
      this.pnlLayout14.Controls.Add((Control) this.picLayout14);
      this.pnlLayout14.Location = new Point(94, 307);
      this.pnlLayout14.Name = "pnlLayout14";
      this.pnlLayout14.Size = new Size(88, 64);
      this.pnlLayout14.TabIndex = 14;
      this.pnlLayout15.Controls.Add((Control) this.picLayout15);
      this.pnlLayout15.Location = new Point(188, 307);
      this.pnlLayout15.Name = "pnlLayout15";
      this.pnlLayout15.Size = new Size(88, 64);
      this.pnlLayout15.TabIndex = 15;
      this.pnlLayout16.Controls.Add((Control) this.picLayout16);
      this.pnlLayout16.Location = new Point(0, 377);
      this.pnlLayout16.Name = "pnlLayout16";
      this.pnlLayout16.Size = new Size(88, 64);
      this.pnlLayout16.TabIndex = 16;
      this.imglstLayouts.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imglstLayouts.ImageStream");
      this.imglstLayouts.TransparentColor = Color.Transparent;
      this.imglstLayouts.Images.SetKeyName(0, "C1R1");
      this.imglstLayouts.Images.SetKeyName(1, "C1R2");
      this.imglstLayouts.Images.SetKeyName(2, "C2R1R1");
      this.imglstLayouts.Images.SetKeyName(3, "C2R2R2");
      this.imglstLayouts.Images.SetKeyName(4, "C2R2R2W4");
      this.imglstLayouts.Images.SetKeyName(5, "C2R2R2W6");
      this.imglstLayouts.Images.SetKeyName(6, "C2R3R2");
      this.imglstLayouts.Images.SetKeyName(7, "C2R3R2W4");
      this.imglstLayouts.Images.SetKeyName(8, "C2R3R2W6");
      this.imglstLayouts.Images.SetKeyName(9, "C2R2R3");
      this.imglstLayouts.Images.SetKeyName(10, "C2R2R3W4");
      this.imglstLayouts.Images.SetKeyName(11, "C2R2R3W6");
      this.imglstLayouts.Images.SetKeyName(12, "C2R3R3");
      this.imglstLayouts.Images.SetKeyName(13, "C2R3R3W4");
      this.imglstLayouts.Images.SetKeyName(14, "C2R3R3W6");
      this.imglstLayouts.Images.SetKeyName(15, "C3R2R2R2");
      this.imglstLayouts.Images.SetKeyName(16, "R2C2C3");
      this.imglstLayouts.Images.SetKeyName(17, "C3R3R3R3");
      this.imglstLayouts.Images.SetKeyName(18, "C1R1Ovr");
      this.imglstLayouts.Images.SetKeyName(19, "C1R2Ovr");
      this.imglstLayouts.Images.SetKeyName(20, "C2R1R1Ovr");
      this.imglstLayouts.Images.SetKeyName(21, "C2R2R2Ovr");
      this.imglstLayouts.Images.SetKeyName(22, "C2R2R2W4Ovr");
      this.imglstLayouts.Images.SetKeyName(23, "C2R2R2W6Ovr");
      this.imglstLayouts.Images.SetKeyName(24, "C2R3R2Ovr");
      this.imglstLayouts.Images.SetKeyName(25, "C2R3R2W4Ovr");
      this.imglstLayouts.Images.SetKeyName(26, "C2R3R2W6Ovr");
      this.imglstLayouts.Images.SetKeyName(27, "C2R2R3Ovr");
      this.imglstLayouts.Images.SetKeyName(28, "C2R2R3W4Ovr");
      this.imglstLayouts.Images.SetKeyName(29, "C2R2R3W6Ovr");
      this.imglstLayouts.Images.SetKeyName(30, "C2R3R3Ovr");
      this.imglstLayouts.Images.SetKeyName(31, "C2R3R3W4Ovr");
      this.imglstLayouts.Images.SetKeyName(32, "C2R3R3W6Ovr");
      this.imglstLayouts.Images.SetKeyName(33, "C3R2R2R2Ovr");
      this.imglstLayouts.Images.SetKeyName(34, "R2C2C3Ovr");
      this.imglstLayouts.Images.SetKeyName(35, "C3R3R3R3Ovr");
      this.imgLstSearch.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgLstSearch.ImageStream");
      this.imgLstSearch.TransparentColor = Color.Transparent;
      this.imgLstSearch.Images.SetKeyName(0, "picSearch");
      this.imgLstSearch.Images.SetKeyName(1, "picSearchDisabled");
      this.imgLstSearch.Images.SetKeyName(2, "picSearchMouseOver");
      this.chkDefaultView.AutoSize = true;
      this.chkDefaultView.Location = new Point(333, 580);
      this.chkDefaultView.Name = "chkDefaultView";
      this.chkDefaultView.Size = new Size(119, 17);
      this.chkDefaultView.TabIndex = 425;
      this.chkDefaultView.Text = "Set as Default View";
      this.chkDefaultView.UseVisualStyleBackColor = true;
      this.chkDefaultView.Click += new EventHandler(this.chkDefaultView_Click);
      this.lblStartFrom.AutoSize = true;
      this.lblStartFrom.Location = new Point(312, 17);
      this.lblStartFrom.Name = "lblStartFrom";
      this.lblStartFrom.Size = new Size(58, 13);
      this.lblStartFrom.TabIndex = 427;
      this.lblStartFrom.Text = "Start From:";
      this.lblStartFrom.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout3);
      this.pnlSelectLayout.Controls.Add((Control) this.lblSelectLayout);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout18);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout1);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout16);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout2);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout15);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout17);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout14);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout4);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout13);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout5);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout12);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout6);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout11);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout7);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout10);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout8);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout9);
      this.pnlSelectLayout.Location = new Point(16, 62);
      this.pnlSelectLayout.Name = "pnlSelectLayout";
      this.pnlSelectLayout.Size = new Size(276, 441);
      this.pnlSelectLayout.TabIndex = 429;
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSelectSnapshot);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot7);
      this.pnlSelectSnapshot.Location = new Point(333, 62);
      this.pnlSelectSnapshot.Name = "pnlSelectSnapshot";
      this.pnlSelectSnapshot.Size = new Size(293, 503);
      this.pnlSelectSnapshot.TabIndex = 430;
      this.cboTimeFrame9.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame9.FormattingEnabled = true;
      this.cboTimeFrame9.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame9.Location = new Point(72, 477);
      this.cboTimeFrame9.Name = "cboTimeFrame9";
      this.cboTimeFrame9.Size = new Size(199, 21);
      this.cboTimeFrame9.TabIndex = 433;
      this.cboTimeFrame9.Tag = (object) "8";
      this.cboTimeFrame9.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame8.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame8.FormattingEnabled = true;
      this.cboTimeFrame8.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame8.Location = new Point(72, 424);
      this.cboTimeFrame8.Name = "cboTimeFrame8";
      this.cboTimeFrame8.Size = new Size(199, 21);
      this.cboTimeFrame8.TabIndex = 432;
      this.cboTimeFrame8.Tag = (object) "7";
      this.cboTimeFrame8.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame7.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame7.FormattingEnabled = true;
      this.cboTimeFrame7.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame7.Location = new Point(72, 371);
      this.cboTimeFrame7.Name = "cboTimeFrame7";
      this.cboTimeFrame7.Size = new Size(199, 21);
      this.cboTimeFrame7.TabIndex = 431;
      this.cboTimeFrame7.Tag = (object) "6";
      this.cboTimeFrame7.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame6.FormattingEnabled = true;
      this.cboTimeFrame6.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame6.Location = new Point(72, 318);
      this.cboTimeFrame6.Name = "cboTimeFrame6";
      this.cboTimeFrame6.Size = new Size(199, 21);
      this.cboTimeFrame6.TabIndex = 430;
      this.cboTimeFrame6.Tag = (object) "5";
      this.cboTimeFrame6.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame5.FormattingEnabled = true;
      this.cboTimeFrame5.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame5.Location = new Point(72, 265);
      this.cboTimeFrame5.Name = "cboTimeFrame5";
      this.cboTimeFrame5.Size = new Size(199, 21);
      this.cboTimeFrame5.TabIndex = 429;
      this.cboTimeFrame5.Tag = (object) "4";
      this.cboTimeFrame5.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame4.FormattingEnabled = true;
      this.cboTimeFrame4.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame4.Location = new Point(72, 212);
      this.cboTimeFrame4.Name = "cboTimeFrame4";
      this.cboTimeFrame4.Size = new Size(199, 21);
      this.cboTimeFrame4.TabIndex = 428;
      this.cboTimeFrame4.Tag = (object) "3";
      this.cboTimeFrame4.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame3.FormattingEnabled = true;
      this.cboTimeFrame3.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame3.Location = new Point(72, 159);
      this.cboTimeFrame3.Name = "cboTimeFrame3";
      this.cboTimeFrame3.Size = new Size(199, 21);
      this.cboTimeFrame3.TabIndex = 427;
      this.cboTimeFrame3.Tag = (object) "2";
      this.cboTimeFrame3.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame2.FormattingEnabled = true;
      this.cboTimeFrame2.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame2.Location = new Point(72, 106);
      this.cboTimeFrame2.Name = "cboTimeFrame2";
      this.cboTimeFrame2.Size = new Size(199, 21);
      this.cboTimeFrame2.TabIndex = 426;
      this.cboTimeFrame2.Tag = (object) "1";
      this.cboTimeFrame2.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame1.FormattingEnabled = true;
      this.cboTimeFrame1.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame1.Location = new Point(72, 53);
      this.cboTimeFrame1.Name = "cboTimeFrame1";
      this.cboTimeFrame1.Size = new Size(199, 21);
      this.cboTimeFrame1.TabIndex = 425;
      this.cboTimeFrame1.Tag = (object) "0";
      this.cboTimeFrame1.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.ctlViewSelection.Location = new Point(376, 13);
      this.ctlViewSelection.Name = "ctlViewSelection";
      this.ctlViewSelection.Size = new Size(160, 21);
      this.ctlViewSelection.TabIndex = 428;
      this.ctlViewSelection.SelectionChangedEvent += new ViewSelectionControl.SelectionChangedEventHandler(this.ctlViewSelection_SelectionChangedEvent);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(645, 652);
      this.Controls.Add((Control) this.pnlSelectSnapshot);
      this.Controls.Add((Control) this.pnlSelectLayout);
      this.Controls.Add((Control) this.ctlViewSelection);
      this.Controls.Add((Control) this.lblStartFrom);
      this.Controls.Add((Control) this.chkDefaultView);
      this.Controls.Add((Control) this.lblVertical1);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblHorizontal2);
      this.Controls.Add((Control) this.lblHorizontal1);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DashboardLayoutForm);
      this.Text = "Edit Dashboard Layout";
      ((ISupportInitialize) this.picLayout1).EndInit();
      ((ISupportInitialize) this.picLayout2).EndInit();
      ((ISupportInitialize) this.picLayout3).EndInit();
      ((ISupportInitialize) this.picLayout4).EndInit();
      ((ISupportInitialize) this.picLayout6).EndInit();
      ((ISupportInitialize) this.picLayout5).EndInit();
      ((ISupportInitialize) this.picLayout7).EndInit();
      ((ISupportInitialize) this.picLayout9).EndInit();
      ((ISupportInitialize) this.picLayout8).EndInit();
      ((ISupportInitialize) this.picLayout10).EndInit();
      ((ISupportInitialize) this.picLayout12).EndInit();
      ((ISupportInitialize) this.picLayout11).EndInit();
      ((ISupportInitialize) this.picLayout15).EndInit();
      ((ISupportInitialize) this.picLayout14).EndInit();
      ((ISupportInitialize) this.picLayout13).EndInit();
      ((ISupportInitialize) this.picLayout18).EndInit();
      ((ISupportInitialize) this.picSnapshot1).EndInit();
      ((ISupportInitialize) this.picSnapshot2).EndInit();
      ((ISupportInitialize) this.picSnapshot3).EndInit();
      ((ISupportInitialize) this.picSnapshot4).EndInit();
      ((ISupportInitialize) this.picSnapshot5).EndInit();
      ((ISupportInitialize) this.picSnapshot6).EndInit();
      ((ISupportInitialize) this.picSnapshot7).EndInit();
      ((ISupportInitialize) this.picSnapshot8).EndInit();
      ((ISupportInitialize) this.picSnapshot9).EndInit();
      ((ISupportInitialize) this.picLayout16).EndInit();
      ((ISupportInitialize) this.picLayout17).EndInit();
      this.pnlLayout18.ResumeLayout(false);
      this.pnlLayout1.ResumeLayout(false);
      this.pnlLayout2.ResumeLayout(false);
      this.pnlLayout3.ResumeLayout(false);
      this.pnlLayout17.ResumeLayout(false);
      this.pnlLayout4.ResumeLayout(false);
      this.pnlLayout5.ResumeLayout(false);
      this.pnlLayout6.ResumeLayout(false);
      this.pnlLayout7.ResumeLayout(false);
      this.pnlLayout8.ResumeLayout(false);
      this.pnlLayout9.ResumeLayout(false);
      this.pnlLayout10.ResumeLayout(false);
      this.pnlLayout11.ResumeLayout(false);
      this.pnlLayout12.ResumeLayout(false);
      this.pnlLayout13.ResumeLayout(false);
      this.pnlLayout14.ResumeLayout(false);
      this.pnlLayout15.ResumeLayout(false);
      this.pnlLayout16.ResumeLayout(false);
      this.pnlSelectLayout.ResumeLayout(false);
      this.pnlSelectLayout.PerformLayout();
      this.pnlSelectSnapshot.ResumeLayout(false);
      this.pnlSelectSnapshot.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum Action
    {
      CREATE_VIEW,
      EDIT_VIEW,
    }

    private class SnapshotTag
    {
      public string ReportName = string.Empty;
      public string TemplatePath = string.Empty;
      public string[] ReportParameters = new string[0];

      private SnapshotTag()
      {
      }

      public SnapshotTag(string reportName, string templatePath, string[] reportParameters)
      {
        this.ReportName = reportName;
        this.TemplatePath = templatePath;
        this.ReportParameters = reportParameters;
      }
    }
  }
}
