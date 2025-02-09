// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsRptJobQueue
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsRptJobQueue : SettingsUserControl
  {
    private GroupContainer gcCurrentLogins;
    private GridView reportQueueView;
    private IOrganizationManager orgManager;
    private StandardIconButton siBtnExcel;
    private new SetUpContainer setupContainer;
    private Sessions.Session session;
    private TextBox searchTxtBx;
    private Button clear_btn;
    private Button search_btn;
    private StandardIconButton stdIconBtnNewJob;
    private StandardIconButton stdIconBtnDeleteJob;
    private StandardIconButton stdIconBtnRefresh;
    private ComboBox cmbReportTypes;
    private ComboBox cmbUsers;
    private PageListNavigator navigator;
    private FeaturesAclManager aclMgr;
    private SettingsRptJobInfo[] jobinfo;
    private Label lblReportName;
    private Label lblUser;
    private Label lblRequestType;
    private Label label1;
    private PanelEx panelEx1;
    private bool suspendEvents = true;
    private bool canUserAccessReportByOtherUser = true;

    public SettingsRptJobQueue(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ViewOtherSettings_Report))
        this.canUserAccessReportByOtherUser = false;
      this.InitializeComponent();
      if (!this.DesignMode)
      {
        this.orgManager = Session.OrganizationManager;
        this.populateReportTypesCombo();
        this.populateUsersCombo();
        this.suspendEvents = false;
        this.reportQueueView.Sort(0, SortOrder.Descending);
        this.suspendEvents = true;
        this.refresh();
      }
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ViewNewSettings_Report))
        this.stdIconBtnNewJob.Enabled = false;
      if (this.aclMgr.GetUserApplicationRight(AclFeature.DeleteSettings_Report))
        return;
      this.stdIconBtnDeleteJob.Enabled = false;
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.gcCurrentLogins = new GroupContainer();
      this.reportQueueView = new GridView();
      this.panelEx1 = new PanelEx();
      this.lblReportName = new Label();
      this.search_btn = new Button();
      this.clear_btn = new Button();
      this.lblUser = new Label();
      this.searchTxtBx = new TextBox();
      this.lblRequestType = new Label();
      this.cmbUsers = new ComboBox();
      this.cmbReportTypes = new ComboBox();
      this.label1 = new Label();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.stdIconBtnDeleteJob = new StandardIconButton();
      this.stdIconBtnNewJob = new StandardIconButton();
      this.siBtnExcel = new StandardIconButton();
      this.navigator = new PageListNavigator();
      this.gcCurrentLogins.SuspendLayout();
      this.panelEx1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteJob).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewJob).BeginInit();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      this.SuspendLayout();
      this.gcCurrentLogins.Controls.Add((Control) this.reportQueueView);
      this.gcCurrentLogins.Controls.Add((Control) this.panelEx1);
      this.gcCurrentLogins.Controls.Add((Control) this.label1);
      this.gcCurrentLogins.Controls.Add((Control) this.stdIconBtnRefresh);
      this.gcCurrentLogins.Controls.Add((Control) this.stdIconBtnDeleteJob);
      this.gcCurrentLogins.Controls.Add((Control) this.stdIconBtnNewJob);
      this.gcCurrentLogins.Controls.Add((Control) this.siBtnExcel);
      this.gcCurrentLogins.Controls.Add((Control) this.navigator);
      this.gcCurrentLogins.Dock = DockStyle.Fill;
      this.gcCurrentLogins.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcCurrentLogins.HeaderForeColor = SystemColors.ControlText;
      this.gcCurrentLogins.Location = new Point(0, 0);
      this.gcCurrentLogins.Name = "gcCurrentLogins";
      this.gcCurrentLogins.Size = new Size(1144, 673);
      this.gcCurrentLogins.TabIndex = 12;
      this.gcCurrentLogins.Paint += new PaintEventHandler(this.gcCurrentLogins_Paint);
      this.reportQueueView.AllowColumnReorder = true;
      this.reportQueueView.AllowDrop = true;
      this.reportQueueView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Tag = (object) "ReportID";
      gvColumn1.Text = "Report ID";
      gvColumn1.Width = 99;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "ReportName";
      gvColumn2.Text = "Report Name";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.DateTime;
      gvColumn3.Tag = (object) "CreateDate";
      gvColumn3.Text = "Request Date";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "JobType";
      gvColumn4.Text = "Request Type";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Tag = (object) "CreatedBy";
      gvColumn5.Text = "Requested By";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "StatusInfo";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 150;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Tag = (object) "Message";
      gvColumn7.Text = "Action";
      gvColumn7.Width = 500;
      this.reportQueueView.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.reportQueueView.Cursor = Cursors.Default;
      this.reportQueueView.Dock = DockStyle.Fill;
      this.reportQueueView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.reportQueueView.Location = new Point(1, 86);
      this.reportQueueView.Name = "reportQueueView";
      this.reportQueueView.Size = new Size(1142, 562);
      this.reportQueueView.SortOption = GVSortOption.Owner;
      this.reportQueueView.TabIndex = 0;
      this.reportQueueView.SortItems += new GVColumnSortEventHandler(this.reportQueueView_SortItems);
      this.reportQueueView.Click += new EventHandler(this.reportQueueView_Click);
      this.panelEx1.Controls.Add((Control) this.lblReportName);
      this.panelEx1.Controls.Add((Control) this.search_btn);
      this.panelEx1.Controls.Add((Control) this.clear_btn);
      this.panelEx1.Controls.Add((Control) this.lblUser);
      this.panelEx1.Controls.Add((Control) this.searchTxtBx);
      this.panelEx1.Controls.Add((Control) this.lblRequestType);
      this.panelEx1.Controls.Add((Control) this.cmbUsers);
      this.panelEx1.Controls.Add((Control) this.cmbReportTypes);
      this.panelEx1.Dock = DockStyle.Top;
      this.panelEx1.Location = new Point(1, 26);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(1142, 60);
      this.panelEx1.TabIndex = 30;
      this.lblReportName.AutoSize = true;
      this.lblReportName.Location = new Point(3, 11);
      this.lblReportName.Name = "lblReportName";
      this.lblReportName.Size = new Size(69, 14);
      this.lblReportName.TabIndex = 25;
      this.lblReportName.Text = "Report Name";
      this.search_btn.BackColor = SystemColors.Control;
      this.search_btn.Location = new Point(564, 26);
      this.search_btn.Name = "search_btn";
      this.search_btn.Size = new Size(75, 22);
      this.search_btn.TabIndex = 14;
      this.search_btn.Text = "Search";
      this.search_btn.UseVisualStyleBackColor = false;
      this.search_btn.Click += new EventHandler(this.search_btn_Click);
      this.clear_btn.BackColor = SystemColors.Control;
      this.clear_btn.Location = new Point(645, 26);
      this.clear_btn.Name = "clear_btn";
      this.clear_btn.Size = new Size(75, 22);
      this.clear_btn.TabIndex = 15;
      this.clear_btn.Text = "Clear";
      this.clear_btn.UseVisualStyleBackColor = false;
      this.clear_btn.Click += new EventHandler(this.clear_btn_Click);
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new Point(375, 9);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(75, 14);
      this.lblUser.TabIndex = 27;
      this.lblUser.Text = "Requested By";
      this.searchTxtBx.Location = new Point(6, 28);
      this.searchTxtBx.Name = "searchTxtBx";
      this.searchTxtBx.Size = new Size(180, 20);
      this.searchTxtBx.TabIndex = 11;
      this.lblRequestType.AutoSize = true;
      this.lblRequestType.Location = new Point(189, 9);
      this.lblRequestType.Name = "lblRequestType";
      this.lblRequestType.Size = new Size(73, 14);
      this.lblRequestType.TabIndex = 26;
      this.lblRequestType.Text = "Request Type";
      this.cmbUsers.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbUsers.FormattingEnabled = true;
      this.cmbUsers.Location = new Point(378, 28);
      this.cmbUsers.Name = "cmbUsers";
      this.cmbUsers.Size = new Size(180, 22);
      this.cmbUsers.TabIndex = 13;
      this.cmbReportTypes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbReportTypes.FormattingEnabled = true;
      this.cmbReportTypes.Location = new Point(192, 28);
      this.cmbReportTypes.Name = "cmbReportTypes";
      this.cmbReportTypes.Size = new Size(180, 22);
      this.cmbReportTypes.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(111, 14);
      this.label1.TabIndex = 28;
      this.label1.Text = "Search for Reports";
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(1046, 4);
      this.stdIconBtnRefresh.MouseDownImage = (Image) null;
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 16);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 22;
      this.stdIconBtnRefresh.TabStop = false;
      this.stdIconBtnRefresh.Click += new EventHandler(this.stdIconBtnRefresh_Click);
      this.stdIconBtnDeleteJob.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteJob.BackColor = Color.Transparent;
      this.stdIconBtnDeleteJob.Location = new Point(1090, 4);
      this.stdIconBtnDeleteJob.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteJob.Name = "stdIconBtnDeleteJob";
      this.stdIconBtnDeleteJob.Size = new Size(16, 17);
      this.stdIconBtnDeleteJob.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteJob.TabIndex = 20;
      this.stdIconBtnDeleteJob.TabStop = false;
      this.stdIconBtnDeleteJob.Click += new EventHandler(this.stdIconBtnDeleteJob_Click);
      this.stdIconBtnNewJob.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewJob.BackColor = Color.Transparent;
      this.stdIconBtnNewJob.Location = new Point(1068, 4);
      this.stdIconBtnNewJob.MouseDownImage = (Image) null;
      this.stdIconBtnNewJob.Name = "stdIconBtnNewJob";
      this.stdIconBtnNewJob.Size = new Size(16, 17);
      this.stdIconBtnNewJob.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewJob.TabIndex = 19;
      this.stdIconBtnNewJob.TabStop = false;
      this.stdIconBtnNewJob.Click += new EventHandler(this.stdIconBtnNewOrg_Click);
      this.siBtnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Location = new Point(960, 4);
      this.siBtnExcel.Margin = new Padding(4);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(21, 20);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 7;
      this.siBtnExcel.TabStop = false;
      this.siBtnExcel.Visible = false;
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.navigator.BackColor = Color.Transparent;
      this.navigator.Dock = DockStyle.Bottom;
      this.navigator.Font = new Font("Arial", 8f);
      this.navigator.Location = new Point(1, 648);
      this.navigator.Name = "navigator";
      this.navigator.NumberOfItems = 0;
      this.navigator.Size = new Size(1142, 24);
      this.navigator.TabIndex = 5;
      this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navigator_PageChangedEvent);
      this.Controls.Add((Control) this.gcCurrentLogins);
      this.Name = nameof (SettingsRptJobQueue);
      this.Size = new Size(1144, 673);
      this.gcCurrentLogins.ResumeLayout(false);
      this.gcCurrentLogins.PerformLayout();
      this.panelEx1.ResumeLayout(false);
      this.panelEx1.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteJob).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewJob).EndInit();
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.ResumeLayout(false);
    }

    public void refreshJobQueue() => this.refresh();

    public Sessions.Session curSession => this.session;

    private void reportQueueView_SortItems(object source, GVColumnSortEventArgs e)
    {
      this.refresh(e.ColumnSorts);
    }

    private void refresh(GVColumnSort[] sortOrder = null)
    {
      if (!this.suspendEvents)
        return;
      this.reportQueueView.Items.Clear();
      sortOrder = sortOrder == null ? this.reportQueueView.Columns.GetSortOrder() : sortOrder;
      try
      {
        string currentSortFields = this.getCurrentSortFields(sortOrder);
        int total = 0;
        this.jobinfo = Session.ReportManager.GetFilteredSettingsRptJobs(this.searchTxtBx.Text.ToString().Trim(), ((KeyValuePair<string, string>) this.cmbReportTypes.SelectedItem).Key, ((KeyValuePair<string, string>) this.cmbUsers.SelectedItem).Key, currentSortFields, this.navigator.CurrentPageItemIndex == -1 ? 0 : this.navigator.CurrentPageItemIndex, out total, this.navigator.ItemsPerPage);
        this.navigator.NumberOfItems = total;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Settings Report Job Status can not be retrieved due to the following reason: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private string getCurrentSortFields(GVColumnSort[] sortOrder)
    {
      return this.getSortFieldsForColumnSort(sortOrder);
    }

    private string getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      string fieldsForColumnSort = "";
      GVColumnSort[] gvColumnSortArray = sortOrder;
      int index = 0;
      if (index < gvColumnSortArray.Length)
      {
        GVColumnSort gvColumnSort = gvColumnSortArray[index];
        fieldsForColumnSort = (string) this.reportQueueView.Columns[gvColumnSort.Column].Tag + (gvColumnSort.SortOrder == SortOrder.Ascending ? " asc" : " desc");
      }
      return fieldsForColumnSort;
    }

    private void displayCurrentPage()
    {
      this.reportQueueView.Items.Clear();
      for (int index = 0; index < this.jobinfo.Length && this.jobinfo[index] != null; ++index)
      {
        string str = Session.UserInfo.ToString() + " (" + Session.UserID + ")";
        GVItem gvItem = new GVItem(this.jobinfo[index].ReportID);
        gvItem.SubItems.Add((object) this.jobinfo[index].ReportName.ToString());
        DateTime localTime = Convert.ToDateTime(this.jobinfo[index].CreateDate).ToLocalTime();
        gvItem.SubItems.Add((object) localTime.ToString());
        gvItem.SubItems.Add((object) (this.jobinfo[index].Type.ToString() + " Report"));
        gvItem.SubItems.Add((object) this.jobinfo[index].CreatedBy);
        gvItem.SubItems.Add((object) this.jobinfo[index].Status);
        if (this.jobinfo[index].Status.ToString().Equals("Submitted") || this.jobinfo[index].Status.ToString().Equals("InProgress") || this.jobinfo[index].Status.ToString().Equals("Canceling"))
        {
          bool userCreatedReport = false;
          if (this.jobinfo[index].CreatedBy == str)
            userCreatedReport = true;
          SettingsRptCancel settingsRptCancel = new SettingsRptCancel(this.jobinfo[index].ReportID, this.jobinfo[index].Status, userCreatedReport);
          gvItem.SubItems.Add((object) settingsRptCancel);
        }
        else
        {
          SettingsRptAction settingsRptAction = new SettingsRptAction(this.jobinfo[index].Type, this.jobinfo[index].ReportID, this.jobinfo[index].ReportName, Session.UserID, localTime, this.jobinfo[index].Status);
          gvItem.SubItems.Add((object) settingsRptAction);
        }
        this.reportQueueView.Items.Add(gvItem);
      }
    }

    private void reportQueueView_Click(object sender, EventArgs e)
    {
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.refresh();

    private void releaseEventHandlers()
    {
    }

    private void stdIconBtnRefresh_Click(object sender, EventArgs e)
    {
      this.refresh();
      SaveConfirmScreen.Show(this.setupContainer == null ? (IWin32Window) this : (IWin32Window) this.setupContainer, "Data refreshed.");
    }

    private void siBtnExcel_Click(object sender, EventArgs e)
    {
      XmlDocument xmlDocument = new XmlDocument();
      GVSelectedItemCollection selectedItems = this.reportQueueView.SelectedItems;
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        string reportID = selectedItems[index].ToString();
        Session.ReportManager.GetSettingsRptXML(Session.UserID, this.jobinfo[index]);
        new SettingsReportXLS(reportID).Save(EnConfigurationSettings.GlobalSettings.AppTempDirectory + "\\SettingsReport_" + reportID + "_" + (object) DateTime.Now.Ticks + ".xlsx", true);
      }
    }

    private void gcCurrentLogins_Paint(object sender, PaintEventArgs e)
    {
    }

    public override void Reset()
    {
      this.refresh();
      SaveConfirmScreen.Show(this.setupContainer == null ? (IWin32Window) this : (IWin32Window) this.setupContainer, "Data refreshed.");
    }

    private void stdIconBtnNewOrg_Click(object sender, EventArgs e)
    {
      using (GenSettingsRptDlg genSettingsRptDlg = new GenSettingsRptDlg(this.session, Session.UserID))
      {
        int num1 = (int) genSettingsRptDlg.ShowDialog((IWin32Window) this);
        if (genSettingsRptDlg.DialogResult == DialogResult.OK)
        {
          try
          {
            this.resetControls();
            this.refresh();
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "A new report was not created. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        genSettingsRptDlg.Dispose();
      }
    }

    private void populateReportTypesCombo()
    {
      this.cmbReportTypes.Items.Clear();
      this.cmbReportTypes.Items.Add((object) new KeyValuePair<string, string>("All", "All"));
      foreach (SettingsRptJobInfo.jobType jobType in Enum.GetValues(typeof (SettingsRptJobInfo.jobType)))
        this.cmbReportTypes.Items.Add((object) new KeyValuePair<string, string>(Convert.ToString((int) jobType), jobType.ToString() + "Report"));
      this.cmbReportTypes.DisplayMember = "Value";
      this.cmbReportTypes.ValueMember = "Key";
      this.cmbReportTypes.SelectedIndex = 0;
    }

    private void populateUsersCombo()
    {
      UserInfo[] array = ((IEnumerable<UserInfo>) this.session.OrganizationManager.GetAllUsers()).OrderBy<UserInfo, string>((Func<UserInfo, string>) (user => user.FirstName)).ToArray<UserInfo>();
      this.cmbUsers.Items.Clear();
      this.cmbUsers.Items.Add((object) new KeyValuePair<string, string>("All", "<All Users>"));
      foreach (UserInfo userInfo in array)
        this.cmbUsers.Items.Add((object) new KeyValuePair<string, string>("(" + userInfo.Userid + ")", string.Format("{0} {1}", (object) userInfo.FirstName, (object) userInfo.LastName)));
      this.cmbUsers.DisplayMember = "Value";
      this.cmbUsers.ValueMember = "Key";
      if (!this.canUserAccessReportByOtherUser)
      {
        this.cmbUsers.SelectedIndex = this.cmbUsers.Items.IndexOf((object) new KeyValuePair<string, string>("(" + this.session.UserInfo.Userid + ")", string.Format("{0} {1}", (object) this.session.UserInfo.FirstName, (object) this.session.UserInfo.LastName)));
        this.cmbUsers.Enabled = false;
      }
      else
        this.cmbUsers.SelectedIndex = 0;
    }

    private void search_btn_Click(object sender, EventArgs e)
    {
      this.suspendEvents = false;
      this.resetNavigation();
      this.reportQueueView.Sort(0, SortOrder.Descending);
      this.suspendEvents = true;
      this.refresh();
    }

    private void navigator_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.reportQueueView.Items.Count == 0)
        this.displayCurrentPage();
      else
        this.refresh();
    }

    private void clear_btn_Click(object sender, EventArgs e)
    {
      this.suspendEvents = false;
      this.resetControls();
      this.reportQueueView.Sort(0, SortOrder.Descending);
      this.suspendEvents = true;
      this.refresh();
    }

    private void resetNavigation()
    {
      this.navigator.ClearSelection();
      this.reportQueueView.ClearSort();
    }

    private void resetControls()
    {
      this.navigator.ClearSelection();
      this.searchTxtBx.Text = string.Empty;
      this.cmbReportTypes.SelectedIndex = 0;
      if (!this.canUserAccessReportByOtherUser)
      {
        this.cmbUsers.SelectedIndex = this.cmbUsers.Items.IndexOf((object) new KeyValuePair<string, string>("(" + this.session.UserInfo.Userid + ")", string.Format("{0} {1}", (object) this.session.UserInfo.FirstName, (object) this.session.UserInfo.LastName)));
        this.cmbUsers.Enabled = false;
      }
      else
        this.cmbUsers.SelectedIndex = 0;
      this.reportQueueView.ClearSort();
    }

    private void stdIconBtnDeleteJob_Click(object sender, EventArgs e)
    {
      GVSelectedItemCollection selectedItems = this.reportQueueView.SelectedItems;
      bool flag = false;
      if (selectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "No reports have been selected for deletion.  Please select reports to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show((IWin32Window) this, "Are you sure you want to delete the selected reports?  These reports will no longer be available for download. ", "Delete Selected Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        for (int index = 0; index < selectedItems.Count; ++index)
        {
          string reportID = selectedItems[index].ToString();
          SettingsRptJobInfo.jobStatus status = Session.ReportManager.GetSettingsRptInfo(reportID).Status;
          if (status.Equals((object) SettingsRptJobInfo.jobStatus.InProgress) || status.Equals((object) SettingsRptJobInfo.jobStatus.Submitted))
            flag = true;
          else
            Session.ReportManager.DeleteSettingsRptJobs(reportID, this.session.UserID);
        }
        if (flag)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The selected reports to delete consist of reports that have just been submitted or still in progress.  Only reports that have been completed, canceled or failed will get deleted.  ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.refresh();
      }
    }
  }
}
