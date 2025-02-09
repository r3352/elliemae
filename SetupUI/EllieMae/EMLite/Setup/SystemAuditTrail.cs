// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SystemAuditTrail
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SystemAuditTrail : UserControl
  {
    private bool purgingDaysDirtyFlag;
    private EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType selectedAction;
    private Sessions.Session session;
    private string[] ddmSubCategoryOptions = new string[6]
    {
      "Fee Rule",
      "Field Rule",
      "Fee Scenario",
      "Field Scenario",
      "Data Table",
      "Global DDM Settings"
    };
    private IContainer components;
    private ComboBox cmbCategory;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private TextBox txtUserID;
    private ComboBox cmbAction;
    private DateTimePicker dttmDate1;
    private Label label5;
    private DateTimePicker dttmDate2;
    private TextBox txtObject;
    private Label lblObject;
    private Button btnSearch;
    private GridView gvRecords;
    private GroupContainer groupContainer1;
    private GroupContainer gcAuditTrails;
    private StandardIconButton siBtnExcel;
    private StandardIconButton siBtnSearchUser;
    private StandardIconButton siBtnRefresh;
    private GroupContainer groupContainer3;
    private Label label6;
    private StandardIconButton siBtnRefreshSetting;
    private StandardIconButton stBtnSave;
    private TextBox txtPurgingDays;
    private ComboBox cmbActionTime;
    private Label label7;
    private ToolTip toolTip1;
    private Label lblSubCategory;
    private ComboBox cmbSubCategory;

    public SystemAuditTrail(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initialPageData();
      this.initialSettingData();
      this.setPurgingDaysDirtyFlag(false);
    }

    private void initialPageData()
    {
      if (this.session.EncompassEdition == EncompassEdition.Broker)
      {
        this.cmbCategory.Items.Remove((object) "Business Contact Group");
        this.cmbCategory.Items.Remove((object) "Business Rule");
      }
      if (!Utils.ParseBoolean(Session.ServerManager.GetServerSetting("CLIENT.ALLOWDDM", false)) || !this.session.IsBankerEdition())
        this.cmbCategory.Items.Remove((object) "Dynamic Data Management");
      if (!this.session.UserInfo.IsTopLevelAdministrator())
      {
        this.siBtnRefreshSetting.Enabled = false;
        this.stBtnSave.Enabled = false;
        this.txtPurgingDays.ReadOnly = true;
      }
      this.cmbCategory.SelectedIndex = 0;
      this.groupContainer1.Text = "Search";
      this.groupContainer3.Text = "Automatic Audit Record Purge Setting";
      this.txtUserID.Text = "";
      this.txtObject.Text = "";
      this.cmbActionTime.SelectedIndex = 0;
      this.gcAuditTrails.Text = "Result (0)";
    }

    private void hideDate2(bool hide)
    {
      if (hide && this.dttmDate2.Visible)
      {
        this.dttmDate2.Visible = false;
        this.label5.Visible = false;
        StandardIconButton siBtnRefresh = this.siBtnRefresh;
        Point location = this.siBtnRefresh.Location;
        int x1 = location.X;
        location = this.btnSearch.Location;
        int x2 = location.X;
        location = this.label5.Location;
        int x3 = location.X;
        int num = x2 - x3;
        int x4 = x1 - num;
        location = this.siBtnRefresh.Location;
        int y1 = location.Y;
        Point point1 = new Point(x4, y1);
        siBtnRefresh.Location = point1;
        Button btnSearch = this.btnSearch;
        location = this.label5.Location;
        int x5 = location.X;
        location = this.btnSearch.Location;
        int y2 = location.Y;
        Point point2 = new Point(x5, y2);
        btnSearch.Location = point2;
      }
      else
      {
        if (hide)
          return;
        this.dttmDate2.Visible = true;
        this.label5.Visible = true;
        Button btnSearch = this.btnSearch;
        Point location = this.dttmDate2.Location;
        int x = location.X + this.dttmDate2.Width + 5;
        location = this.dttmDate2.Location;
        int y = location.Y;
        Point point3 = new Point(x, y);
        btnSearch.Location = point3;
        StandardIconButton siBtnRefresh = this.siBtnRefresh;
        location = this.btnSearch.Location;
        Point point4 = new Point(location.X + this.btnSearch.Width + 5, this.siBtnRefresh.Location.Y);
        siBtnRefresh.Location = point4;
      }
    }

    private void initialSettingData()
    {
      this.txtPurgingDays.Text = string.Concat(this.session.ServerManager.GetServerSetting("Policies.SettingsAuditTrailPurge"));
    }

    private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cmbAction.Items.Clear();
      this.cmbAction.Enabled = this.lblObject.Visible = this.txtObject.Visible = true;
      this.lblObject.Left = 366;
      this.txtObject.Left = 369;
      this.lblSubCategory.Visible = this.cmbSubCategory.Visible = false;
      switch (this.cmbCategory.Text)
      {
        case "Business Contact Group":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "Business Contact Group Name";
          break;
        case "Business Rule":
          this.cmbAction.Items.AddRange((object[]) new string[5]
          {
            "Created",
            "Modified",
            "Deleted",
            "Activated",
            "Deactivated"
          });
          this.lblObject.Text = "Business Rule Name";
          break;
        case "Dynamic Data Management":
          this.cmbSubCategory.Items.Clear();
          this.lblSubCategory.Visible = this.cmbSubCategory.Visible = true;
          this.lblObject.Left = 524;
          this.txtObject.Left = 527;
          this.cmbSubCategory.Items.AddRange((object[]) this.ddmSubCategoryOptions);
          this.cmbSubCategory.SelectedIndex = 0;
          this.lblObject.Text = "Setting / Rule Name";
          break;
        case "Failed Login Attempt":
          this.cmbAction.Items.AddRange((object[]) new string[6]
          {
            "User Not Found",
            "Password Mismatch",
            "User Disabled",
            "User Locked",
            "Persona Not Found",
            "IP Blocked"
          });
          this.lblObject.Text = "IP Address";
          break;
        case "HMDA Profile":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "HMDA Profile Name";
          break;
        case "Loan File":
          this.cmbAction.Items.AddRange((object[]) new string[7]
          {
            "Created",
            "Imported",
            "Modified",
            "Deleted",
            "Permanently Deleted",
            "Restored",
            "Moved"
          });
          this.lblObject.Text = "Borrower Last Name";
          break;
        case "Loan Template/Resource":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "Template Name";
          break;
        case "Persona":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "Persona Name";
          break;
        case "User Account":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "User Name";
          break;
        case "User Group":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          this.lblObject.Text = "User Group Name";
          break;
        case "User Login":
        case "User Logout":
        case "User Password Change Forced":
        case "User Password Changed":
          this.lblObject.Text = "User Name";
          this.cmbAction.Enabled = false;
          break;
        default:
          this.cmbAction.Enabled = false;
          this.lblObject.Text = "NA";
          break;
      }
      if (this.cmbAction.Items.Count <= 0)
        return;
      this.cmbAction.SelectedIndex = 0;
    }

    private void btnUserList_Click(object sender, EventArgs e)
    {
      ContactAssignment contactAssignment = new ContactAssignment(this.session, "");
      if (contactAssignment.ShowDialog() != DialogResult.OK)
        return;
      this.txtUserID.Text = contactAssignment.SelectedUser.Userid;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.gvRecords.Items.Clear();
      this.gvRecords.Columns.Clear();
      this.selectedAction = this.getSelectedActionType();
      if (this.selectedAction == EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.None)
        return;
      string userID = this.txtUserID.Text.Trim();
      string objectID = "";
      string objectName = "";
      switch (this.selectedAction)
      {
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted:
          objectID = this.txtObject.Text.Trim();
          break;
        default:
          objectName = this.txtObject.Text.Trim();
          break;
      }
      DateTime startTime = DateTime.MinValue;
      DateTime minValue = DateTime.MinValue;
      DateTime endTime;
      switch (this.cmbActionTime.Text.ToLower())
      {
        case "is":
          startTime = this.dttmDate1.Value;
          endTime = this.dttmDate1.Value;
          break;
        case "after":
          startTime = this.dttmDate1.Value.AddSeconds(1.0);
          endTime = DateTime.MaxValue;
          break;
        case "before":
          endTime = this.dttmDate1.Value.AddSeconds(-1.0);
          break;
        case "between":
          startTime = this.dttmDate1.Value;
          endTime = this.dttmDate2.Value;
          break;
        default:
          startTime = DateTime.MinValue;
          endTime = DateTime.MaxValue;
          break;
      }
      SystemAuditRecord[] auditRecord = this.session.GetAuditRecord(userID, this.selectedAction, startTime, endTime, objectID, objectName);
      if (auditRecord == null || auditRecord.Length == 0)
      {
        this.gcAuditTrails.Text = "Result (0)";
        this.siBtnExcel.Enabled = false;
      }
      else
      {
        this.siBtnExcel.Enabled = true;
        this.gcAuditTrails.Text = "Result (" + (object) auditRecord.Length + ")";
        this.loadLsvEx(auditRecord);
      }
    }

    private void loadLsvEx(SystemAuditRecord[] searchResult)
    {
      if (searchResult == null || searchResult.Length == 0)
        return;
      this.gcAuditTrails.Text = "Result (" + (object) searchResult.Length + ")";
      string[] headersText = (string[]) null;
      switch (this.selectedAction)
      {
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanImported:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanPermanentDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanRestored:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanMoved:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserDeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChangeForced:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChanged:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogin:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.SSOUserLogin:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogout:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaDeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupDeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupDeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeactivated:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserNotFound:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPasswordMismatch:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserDisabled:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginLoginDisabled:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserLocked:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPersonaNotFound:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.IPBlocked:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.SSOFailedUserLogin:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataPopulationTimingModified:
          headersText = searchResult[0].ColumnHeaders;
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDACreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDAModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDADeleted:
          headersText = searchResult[0].ColumnHeaders;
          break;
      }
      this.createColumnHeader(headersText);
      switch (this.selectedAction)
      {
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanImported:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanPermanentDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanRestored:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanMoved:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserDeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChangeForced:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChanged:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogin:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogout:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaDeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupDeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupDeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeactivated:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserNotFound:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPasswordMismatch:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserDisabled:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginLoginDisabled:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserLocked:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPersonaNotFound:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.IPBlocked:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioActivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeactivated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableCreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableDeleted:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataPopulationTimingModified:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDACreated:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDAModified:
        case EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDADeleted:
          foreach (SystemAuditRecord systemAuditRecord in searchResult)
            this.gvRecords.Items.Add(new GVItem(systemAuditRecord.ColumnContents));
          break;
      }
    }

    private void createColumnHeader(string[] headersText)
    {
      if (headersText == null || headersText.Length == 0)
        return;
      foreach (string strColumnName in headersText)
      {
        GVColumn gvColumn = this.gvRecords.Columns.Add(strColumnName, 150);
        if (strColumnName == "DateTime")
          gvColumn.SortMethod = GVSortMethod.DateTime;
      }
    }

    private EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType getSelectedActionType()
    {
      EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.None;
      switch (this.cmbCategory.Text)
      {
        case "Business Contact Group":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessContactGroupDeleted;
              break;
          }
          break;
        case "Business Rule":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeleted;
              break;
            case "Activated":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleActivated;
              break;
            case "Deactivated":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.BusinessRuleDeactivated;
              break;
          }
          break;
        case "Dynamic Data Management":
          switch (this.cmbAction.Text)
          {
            case "Created":
              switch (this.cmbSubCategory.Text)
              {
                case "Fee Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleCreated;
                  break;
                case "Field Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleCreated;
                  break;
                case "Fee Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioCreated;
                  break;
                case "Field Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioCreated;
                  break;
                case "Data Table":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableCreated;
                  break;
              }
              break;
            case "Modified":
              switch (this.cmbSubCategory.Text)
              {
                case "Fee Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleModified;
                  break;
                case "Field Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleModified;
                  break;
                case "Fee Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioModified;
                  break;
                case "Field Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioModified;
                  break;
                case "Data Table":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableModified;
                  break;
                case "Global DDM Settings":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataPopulationTimingModified;
                  break;
              }
              break;
            case "Deleted":
              switch (this.cmbSubCategory.Text)
              {
                case "Fee Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeleted;
                  break;
                case "Field Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeleted;
                  break;
                case "Fee Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeleted;
                  break;
                case "Field Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeleted;
                  break;
                case "Data Table":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.DataTableDeleted;
                  break;
              }
              break;
            case "Activated":
              switch (this.cmbSubCategory.Text)
              {
                case "Fee Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleActivated;
                  break;
                case "Field Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleActivated;
                  break;
                case "Fee Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioActivated;
                  break;
                case "Field Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioActivated;
                  break;
              }
              break;
            case "Deactivated":
              switch (this.cmbSubCategory.Text)
              {
                case "Fee Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeRuleDeactivated;
                  break;
                case "Field Rule":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldRuleDeactivated;
                  break;
                case "Fee Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FeeScenarioDeactivated;
                  break;
                case "Field Scenario":
                  selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FieldScenarioDeactivated;
                  break;
              }
              break;
          }
          break;
        case "Failed Login Attempt":
          switch (this.cmbAction.Text)
          {
            case "IP Blocked":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.IPBlocked;
              break;
            case "Login Disabled":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginLoginDisabled;
              break;
            case "Password Mismatch":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPasswordMismatch;
              break;
            case "Persona Not Found":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginPersonaNotFound;
              break;
            case "User Disabled":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserDisabled;
              break;
            case "User Locked":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserLocked;
              break;
            case "User Not Found":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.FailedLoginUserNotFound;
              break;
          }
          break;
        case "HMDA Profile":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDACreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDAModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDADeleted;
              break;
          }
          break;
        case "Loan File":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanCreated;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanDeleted;
              break;
            case "Imported":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanImported;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanModified;
              break;
            case "Moved":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanMoved;
              break;
            case "Permanently Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanPermanentDeleted;
              break;
            case "Restored":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.LoanRestored;
              break;
          }
          break;
        case "Loan Template/Resource":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted;
              break;
          }
          break;
        case "Persona":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.PersonaDeleted;
              break;
          }
          break;
        case "User Account":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserDeleted;
              break;
          }
          break;
        case "User Group":
          switch (this.cmbAction.Text)
          {
            case "Created":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupCreated;
              break;
            case "Modified":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupModified;
              break;
            case "Deleted":
              selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupDeleted;
              break;
          }
          break;
        case "User Login":
          selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogin;
          break;
        case "User Logout":
          selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserLogout;
          break;
        case "User Password Change Forced":
          selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChangeForced;
          break;
        case "User Password Changed":
          selectedActionType = EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserPasswordChanged;
          break;
      }
      return selectedActionType;
    }

    private void dttmFromPicker_ValueChanged(object sender, EventArgs e)
    {
      if (this.dttmDate1.Value > this.dttmDate2.Value)
        this.dttmDate2.Value = this.dttmDate1.Value;
      this.dttmDate2.MinDate = this.dttmDate1.Value;
    }

    private void dttmToPicker_ValueChanged(object sender, EventArgs e)
    {
      if (this.dttmDate2.Value < this.dttmDate1.Value)
        this.dttmDate1.Value = this.dttmDate2.Value;
      this.dttmDate1.MaxDate = this.dttmDate2.Value;
    }

    private void siBtnRefresh_Click(object sender, EventArgs e)
    {
      this.gvRecords.Items.Clear();
      this.gvRecords.Columns.Clear();
      this.initialPageData();
    }

    private void siBtnExcel_Click(object sender, EventArgs e)
    {
      if (this.gvRecords.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one audit record to export.");
      }
      else
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn column in this.gvRecords.Columns)
          excelHandler.AddHeaderColumn(column);
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.gvRecords.SelectedItems)
        {
          stringList.Clear();
          foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) selectedItem.SubItems)
            stringList.Add(subItem.Text);
          excelHandler.AddDataRow(stringList.ToArray());
        }
        excelHandler.CreateExcel();
      }
    }

    private void siBtnRefreshSetting_Click(object sender, EventArgs e)
    {
      this.initialSettingData();
      this.setPurgingDaysDirtyFlag(false);
    }

    private void stBtnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateSetting())
        return;
      this.session.ServerManager.UpdateServerSetting("Policies.SettingsAuditTrailPurge", (object) this.txtPurgingDays.Text.Trim());
      this.setPurgingDaysDirtyFlag(false);
    }

    private bool validateSetting()
    {
      bool flag = true;
      if (!Utils.IsInt((object) this.txtPurgingDays.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid input.  Number of days must be an integer.");
        this.initialSettingData();
        flag = false;
      }
      else if (int.Parse(this.txtPurgingDays.Text.Trim()) < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid input.  Number of days must be greater than 0.");
        this.initialSettingData();
        flag = false;
      }
      else if (int.Parse(this.txtPurgingDays.Text.Trim()) > 999)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid input.  Number of days must be less than 999.");
        this.initialSettingData();
        flag = false;
      }
      return flag;
    }

    private void cmbActionTime_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbActionTime.Text == "")
      {
        this.hideDate2(true);
        this.dttmDate1.CustomFormat = "' '";
        this.dttmDate1.Enabled = false;
      }
      else if (this.cmbActionTime.Text != "Between")
      {
        this.dttmDate1.CustomFormat = "MM/dd/yyyy HH:mm:ss";
        this.hideDate2(true);
        this.dttmDate1.Enabled = true;
      }
      else
      {
        this.hideDate2(false);
        this.dttmDate1.Enabled = true;
        this.dttmDate1.CustomFormat = "MM/dd/yyyy HH:mm:ss";
      }
    }

    private void dttmDate1_CloseUp(object sender, EventArgs e)
    {
      this.dttmDate1.CustomFormat = "MM/dd/yyyy HH:mm:ss";
    }

    private void txtPurgingDays_TextChanged(object sender, EventArgs e)
    {
      if (this.txtPurgingDays.Text != "" && !Utils.IsInt((object) this.txtPurgingDays.Text))
        this.txtPurgingDays.Text = "";
      this.setPurgingDaysDirtyFlag(true);
    }

    private void setPurgingDaysDirtyFlag(bool val)
    {
      this.purgingDaysDirtyFlag = val;
      this.stBtnSave.Enabled = this.siBtnRefreshSetting.Enabled = val;
    }

    private void cmbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.cmbAction.Text;
      this.cmbAction.Items.Clear();
      switch (this.cmbSubCategory.Text)
      {
        case "Fee Rule":
          this.cmbAction.Items.AddRange((object[]) new string[5]
          {
            "Created",
            "Modified",
            "Deleted",
            "Activated",
            "Deactivated"
          });
          break;
        case "Field Rule":
          this.cmbAction.Items.AddRange((object[]) new string[5]
          {
            "Created",
            "Modified",
            "Deleted",
            "Activated",
            "Deactivated"
          });
          break;
        case "Fee Scenario":
          this.cmbAction.Items.AddRange((object[]) new string[5]
          {
            "Created",
            "Modified",
            "Deleted",
            "Activated",
            "Deactivated"
          });
          break;
        case "Field Scenario":
          this.cmbAction.Items.AddRange((object[]) new string[5]
          {
            "Created",
            "Modified",
            "Deleted",
            "Activated",
            "Deactivated"
          });
          break;
        case "Data Table":
          this.cmbAction.Items.AddRange((object[]) new string[3]
          {
            "Created",
            "Modified",
            "Deleted"
          });
          break;
        case "Global DDM Settings":
          this.cmbAction.Items.AddRange((object[]) new string[1]
          {
            "Modified"
          });
          break;
      }
      if (!this.cmbAction.Items.Contains((object) text))
        return;
      this.cmbAction.SelectedIndex = this.cmbAction.FindStringExact(text);
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
      this.gcAuditTrails = new GroupContainer();
      this.siBtnExcel = new StandardIconButton();
      this.gvRecords = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.lblSubCategory = new Label();
      this.cmbSubCategory = new ComboBox();
      this.cmbActionTime = new ComboBox();
      this.siBtnRefresh = new StandardIconButton();
      this.siBtnSearchUser = new StandardIconButton();
      this.btnSearch = new Button();
      this.label1 = new Label();
      this.txtObject = new TextBox();
      this.label2 = new Label();
      this.lblObject = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.cmbCategory = new ComboBox();
      this.dttmDate2 = new DateTimePicker();
      this.cmbAction = new ComboBox();
      this.dttmDate1 = new DateTimePicker();
      this.txtUserID = new TextBox();
      this.groupContainer3 = new GroupContainer();
      this.label7 = new Label();
      this.siBtnRefreshSetting = new StandardIconButton();
      this.stBtnSave = new StandardIconButton();
      this.txtPurgingDays = new TextBox();
      this.label6 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.gcAuditTrails.SuspendLayout();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.siBtnRefresh).BeginInit();
      ((ISupportInitialize) this.siBtnSearchUser).BeginInit();
      this.groupContainer3.SuspendLayout();
      ((ISupportInitialize) this.siBtnRefreshSetting).BeginInit();
      ((ISupportInitialize) this.stBtnSave).BeginInit();
      this.SuspendLayout();
      this.gcAuditTrails.Controls.Add((Control) this.siBtnExcel);
      this.gcAuditTrails.Controls.Add((Control) this.gvRecords);
      this.gcAuditTrails.Dock = DockStyle.Fill;
      this.gcAuditTrails.Font = new Font("Arial", 8.25f);
      this.gcAuditTrails.HeaderForeColor = SystemColors.ControlText;
      this.gcAuditTrails.Location = new Point(0, 124);
      this.gcAuditTrails.Name = "gcAuditTrails";
      this.gcAuditTrails.Size = new Size(820, 460);
      this.gcAuditTrails.TabIndex = 4;
      this.siBtnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Enabled = false;
      this.siBtnExcel.Location = new Point(799, 5);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 6;
      this.siBtnExcel.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnExcel, "Export to Excel");
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.gvRecords.BorderStyle = BorderStyle.None;
      this.gvRecords.Dock = DockStyle.Fill;
      this.gvRecords.Font = new Font("Arial", 8.25f);
      this.gvRecords.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRecords.Location = new Point(1, 26);
      this.gvRecords.Name = "gvRecords";
      this.gvRecords.Size = new Size(818, 433);
      this.gvRecords.TabIndex = 2;
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.lblSubCategory);
      this.groupContainer1.Controls.Add((Control) this.cmbSubCategory);
      this.groupContainer1.Controls.Add((Control) this.cmbActionTime);
      this.groupContainer1.Controls.Add((Control) this.siBtnRefresh);
      this.groupContainer1.Controls.Add((Control) this.siBtnSearchUser);
      this.groupContainer1.Controls.Add((Control) this.btnSearch);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.txtObject);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.lblObject);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.cmbCategory);
      this.groupContainer1.Controls.Add((Control) this.dttmDate2);
      this.groupContainer1.Controls.Add((Control) this.cmbAction);
      this.groupContainer1.Controls.Add((Control) this.dttmDate1);
      this.groupContainer1.Controls.Add((Control) this.txtUserID);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(820, 124);
      this.groupContainer1.TabIndex = 3;
      this.lblSubCategory.AutoSize = true;
      this.lblSubCategory.Font = new Font("Arial", 8.25f);
      this.lblSubCategory.Location = new Point(366, 33);
      this.lblSubCategory.Name = "lblSubCategory";
      this.lblSubCategory.Size = new Size(74, 14);
      this.lblSubCategory.TabIndex = 21;
      this.lblSubCategory.Text = "Sub-Category";
      this.cmbSubCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSubCategory.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbSubCategory.FormattingEnabled = true;
      this.cmbSubCategory.Location = new Point(369, 50);
      this.cmbSubCategory.Name = "cmbSubCategory";
      this.cmbSubCategory.Size = new Size(136, 22);
      this.cmbSubCategory.TabIndex = 20;
      this.cmbSubCategory.SelectedIndexChanged += new EventHandler(this.cmbSubCategory_SelectedIndexChanged);
      this.cmbActionTime.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbActionTime.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbActionTime.FormattingEnabled = true;
      this.cmbActionTime.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Is",
        (object) "After",
        (object) "Before",
        (object) "Between"
      });
      this.cmbActionTime.Location = new Point(213, 93);
      this.cmbActionTime.Name = "cmbActionTime";
      this.cmbActionTime.Size = new Size(136, 22);
      this.cmbActionTime.TabIndex = 19;
      this.cmbActionTime.SelectedIndexChanged += new EventHandler(this.cmbActionTime_SelectedIndexChanged);
      this.siBtnRefresh.BackColor = Color.Transparent;
      this.siBtnRefresh.Location = new Point(738, 96);
      this.siBtnRefresh.MouseDownImage = (Image) null;
      this.siBtnRefresh.Name = "siBtnRefresh";
      this.siBtnRefresh.Size = new Size(16, 16);
      this.siBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.siBtnRefresh.TabIndex = 17;
      this.siBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnRefresh, "Reset");
      this.siBtnRefresh.Click += new EventHandler(this.siBtnRefresh_Click);
      this.siBtnSearchUser.BackColor = Color.Transparent;
      this.siBtnSearchUser.Location = new Point(172, 94);
      this.siBtnSearchUser.MouseDownImage = (Image) null;
      this.siBtnSearchUser.Name = "siBtnSearchUser";
      this.siBtnSearchUser.Size = new Size(16, 16);
      this.siBtnSearchUser.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.siBtnSearchUser.TabIndex = 16;
      this.siBtnSearchUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnSearchUser, "Find");
      this.siBtnSearchUser.Click += new EventHandler(this.btnUserList_Click);
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.Location = new Point(661, 93);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(75, 21);
      this.btnSearch.TabIndex = 15;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f);
      this.label1.Location = new Point(7, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(51, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Category";
      this.txtObject.Font = new Font("Arial", 8.25f);
      this.txtObject.Location = new Point(527, 51);
      this.txtObject.Name = "txtObject";
      this.txtObject.Size = new Size(129, 20);
      this.txtObject.TabIndex = 14;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(210, 33);
      this.label2.Name = "label2";
      this.label2.Size = new Size(38, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Action";
      this.lblObject.AutoSize = true;
      this.lblObject.Font = new Font("Arial", 8.25f);
      this.lblObject.Location = new Point(524, 33);
      this.lblObject.Name = "lblObject";
      this.lblObject.Size = new Size(70, 14);
      this.lblObject.TabIndex = 13;
      this.lblObject.Text = "General Field";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f);
      this.label3.Location = new Point(7, 77);
      this.label3.Name = "label3";
      this.label3.Size = new Size(84, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Action Taken by";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f);
      this.label4.Location = new Point(210, 77);
      this.label4.Name = "label4";
      this.label4.Size = new Size(63, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Action Time";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f);
      this.label5.Location = new Point(500, 97);
      this.label5.Name = "label5";
      this.label5.Size = new Size(25, 14);
      this.label5.TabIndex = 10;
      this.label5.Text = "and";
      this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCategory.Font = new Font("Arial", 8.25f);
      this.cmbCategory.FormattingEnabled = true;
      this.cmbCategory.Items.AddRange(new object[14]
      {
        (object) "Business Contact Group",
        (object) "Business Rule",
        (object) "Dynamic Data Management",
        (object) "Failed Login Attempt",
        (object) "HMDA Profile",
        (object) "Loan File",
        (object) "Loan Template/Resource",
        (object) "Persona",
        (object) "User Account",
        (object) "User Group",
        (object) "User Login",
        (object) "User Logout",
        (object) "User Password Change Forced",
        (object) "User Password Changed"
      });
      this.cmbCategory.Location = new Point(10, 49);
      this.cmbCategory.Name = "cmbCategory";
      this.cmbCategory.Size = new Size(180, 22);
      this.cmbCategory.TabIndex = 4;
      this.cmbCategory.SelectedIndexChanged += new EventHandler(this.cmbCategory_SelectedIndexChanged);
      this.dttmDate2.CalendarFont = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dttmDate2.CalendarMonthBackground = Color.White;
      this.dttmDate2.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dttmDate2.CustomFormat = "MM/dd/yyyy HH:mm:ss";
      this.dttmDate2.Font = new Font("Arial", 8.25f);
      this.dttmDate2.Format = DateTimePickerFormat.Custom;
      this.dttmDate2.Location = new Point(527, 94);
      this.dttmDate2.Name = "dttmDate2";
      this.dttmDate2.Size = new Size(129, 20);
      this.dttmDate2.TabIndex = 9;
      this.dttmDate2.ValueChanged += new EventHandler(this.dttmToPicker_ValueChanged);
      this.cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAction.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbAction.FormattingEnabled = true;
      this.cmbAction.Location = new Point(213, 49);
      this.cmbAction.Name = "cmbAction";
      this.cmbAction.Size = new Size(136, 22);
      this.cmbAction.TabIndex = 5;
      this.dttmDate1.CalendarFont = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dttmDate1.CalendarMonthBackground = Color.White;
      this.dttmDate1.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dttmDate1.CustomFormat = "MM/dd/yyyy HH:mm:ss";
      this.dttmDate1.Format = DateTimePickerFormat.Custom;
      this.dttmDate1.Location = new Point(369, 94);
      this.dttmDate1.Name = "dttmDate1";
      this.dttmDate1.Size = new Size(129, 20);
      this.dttmDate1.TabIndex = 8;
      this.dttmDate1.CloseUp += new EventHandler(this.dttmDate1_CloseUp);
      this.dttmDate1.ValueChanged += new EventHandler(this.dttmFromPicker_ValueChanged);
      this.txtUserID.Font = new Font("Arial", 8.25f);
      this.txtUserID.Location = new Point(10, 93);
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.Size = new Size(159, 20);
      this.txtUserID.TabIndex = 6;
      this.groupContainer3.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer3.Controls.Add((Control) this.label7);
      this.groupContainer3.Controls.Add((Control) this.siBtnRefreshSetting);
      this.groupContainer3.Controls.Add((Control) this.stBtnSave);
      this.groupContainer3.Controls.Add((Control) this.txtPurgingDays);
      this.groupContainer3.Controls.Add((Control) this.label6);
      this.groupContainer3.Dock = DockStyle.Bottom;
      this.groupContainer3.Font = new Font("Arial", 8.25f);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 584);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(820, 61);
      this.groupContainer3.TabIndex = 5;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f);
      this.label7.Location = new Point(243, 36);
      this.label7.Name = "label7";
      this.label7.Size = new Size(31, 14);
      this.label7.TabIndex = 4;
      this.label7.Text = "days";
      this.siBtnRefreshSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnRefreshSetting.BackColor = Color.Transparent;
      this.siBtnRefreshSetting.Location = new Point(799, 5);
      this.siBtnRefreshSetting.MouseDownImage = (Image) null;
      this.siBtnRefreshSetting.Name = "siBtnRefreshSetting";
      this.siBtnRefreshSetting.Size = new Size(16, 16);
      this.siBtnRefreshSetting.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.siBtnRefreshSetting.TabIndex = 3;
      this.siBtnRefreshSetting.TabStop = false;
      this.siBtnRefreshSetting.Click += new EventHandler(this.siBtnRefreshSetting_Click);
      this.stBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stBtnSave.BackColor = Color.Transparent;
      this.stBtnSave.Location = new Point(778, 5);
      this.stBtnSave.MouseDownImage = (Image) null;
      this.stBtnSave.Name = "stBtnSave";
      this.stBtnSave.Size = new Size(16, 16);
      this.stBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stBtnSave.TabIndex = 2;
      this.stBtnSave.TabStop = false;
      this.stBtnSave.Click += new EventHandler(this.stBtnSave_Click);
      this.txtPurgingDays.Location = new Point(200, 33);
      this.txtPurgingDays.MaxLength = 3;
      this.txtPurgingDays.Name = "txtPurgingDays";
      this.txtPurgingDays.Size = new Size(41, 20);
      this.txtPurgingDays.TabIndex = 1;
      this.txtPurgingDays.TextChanged += new EventHandler(this.txtPurgingDays_TextChanged);
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f);
      this.label6.Location = new Point(10, 36);
      this.label6.Name = "label6";
      this.label6.Size = new Size(190, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Purge audit records permanently after";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcAuditTrails);
      this.Controls.Add((Control) this.groupContainer3);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (SystemAuditTrail);
      this.Size = new Size(820, 645);
      this.gcAuditTrails.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.siBtnRefresh).EndInit();
      ((ISupportInitialize) this.siBtnSearchUser).EndInit();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      ((ISupportInitialize) this.siBtnRefreshSetting).EndInit();
      ((ISupportInitialize) this.stBtnSave).EndInit();
      this.ResumeLayout(false);
    }
  }
}
