// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompHistoryControl
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
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompHistoryControl : UserControl
  {
    private Sessions.Session session;
    private string parentID = string.Empty;
    private string currentID = string.Empty;
    private bool forUser;
    private bool forExternal;
    private bool forExternalLender;
    private LoanCompHistoryList loanCompHistoryList;
    private LoanCompHistoryList originalloanCompHistoryList;
    private bool uncheckParentInfo;
    private IContainer components;
    private GroupContainer grpHistory;
    private GridView gridViewHistory;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private CheckBox chkUseParent;
    private StandardIconButton stdIconBtnZoom;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnPrint;

    public event EventHandler UseParentInfoClicked;

    public event EventHandler HistorySelectedIndexChanged;

    public event EventHandler AssignPlanButtonClicked;

    public event EventHandler DeletePlanButtonClicked;

    public LOCompHistoryControl(Sessions.Session session, bool forUser, bool forExternal)
      : this(session, forUser, forExternal, false)
    {
    }

    public LOCompHistoryControl(
      Sessions.Session session,
      bool forUser,
      bool forExternal,
      bool forExternalLender)
    {
      this.session = session;
      this.forUser = forUser;
      this.forExternal = forExternal;
      this.forExternalLender = forExternalLender;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
    }

    public void RefreshData(
      LoanCompHistoryList loanCompHistoryList,
      string parentID,
      string currentID)
    {
      this.loanCompHistoryList = loanCompHistoryList;
      this.originalloanCompHistoryList = (LoanCompHistoryList) loanCompHistoryList.Clone();
      this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
      this.chkUseParent.Checked = this.loanCompHistoryList.UseParentInfo;
      if (parentID == currentID || this.forExternal && parentID == "0")
        this.chkUseParent.Visible = false;
      this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
      this.parentID = parentID;
      this.currentID = currentID;
      this.gridViewHistory.Items.Clear();
      this.RefreshHistoryList(this.loanCompHistoryList.GetCurrentPlan(DateTime.Today.Date));
    }

    public void RefreshHistoryList(LoanCompHistory newlyAdded)
    {
      this.gridViewHistory.BeginUpdate();
      this.gridViewHistory.Items.Clear();
      for (int i = 0; i < this.loanCompHistoryList.Count; ++i)
      {
        LoanCompHistory historyAt = this.loanCompHistoryList.GetHistoryAt(i);
        GVItem gvItem = new GVItem(historyAt.PlanName);
        GVSubItemCollection subItems1 = gvItem.SubItems;
        DateTime dateTime = historyAt.StartDate;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.MinValue;
        DateTime date2 = dateTime.Date;
        string str1;
        if (!(date1 != date2))
        {
          str1 = "";
        }
        else
        {
          dateTime = historyAt.StartDate;
          dateTime = dateTime.Date;
          str1 = dateTime.ToString("MM/dd/yyyy");
        }
        subItems1.Add((object) str1);
        GVSubItemCollection subItems2 = gvItem.SubItems;
        dateTime = historyAt.EndDate;
        DateTime date3 = dateTime.Date;
        dateTime = DateTime.MinValue;
        DateTime date4 = dateTime.Date;
        string str2;
        if (date3 != date4)
        {
          dateTime = historyAt.EndDate;
          DateTime date5 = dateTime.Date;
          dateTime = DateTime.MaxValue;
          DateTime date6 = dateTime.Date;
          if (date5 != date6)
          {
            dateTime = historyAt.EndDate;
            dateTime = dateTime.Date;
            str2 = dateTime.ToString("MM/dd/yyyy");
            goto label_8;
          }
        }
        str2 = "";
label_8:
        subItems2.Add((object) str2);
        if (newlyAdded != null)
        {
          dateTime = newlyAdded.StartDate;
          DateTime date7 = dateTime.Date;
          dateTime = historyAt.StartDate;
          DateTime date8 = dateTime.Date;
          if (date7 == date8)
          {
            gvItem.Selected = true;
            goto label_12;
          }
        }
        gvItem.Selected = false;
label_12:
        gvItem.Tag = (object) historyAt;
        dateTime = historyAt.EndDate;
        DateTime date9 = dateTime.Date;
        dateTime = DateTime.Today;
        DateTime date10 = dateTime.Date;
        gvItem.ForeColor = !(date9 < date10) ? Color.Green : Color.Red;
        this.gridViewHistory.Items.Add(gvItem);
      }
      this.gridViewHistory.Sort(1, SortOrder.Descending);
      this.grpHistory.Text = "Assigned Compensation Plans (" + (object) this.gridViewHistory.Items.Count + ")";
      this.gridViewHistory.EndUpdate();
      this.gridViewHistory_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void gridViewHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.gridViewHistory.SelectedItems.Count > 0;
      this.stdIconBtnZoom.Enabled = this.gridViewHistory.SelectedItems.Count == 1;
      this.stdIconBtnPrint.Enabled = this.gridViewHistory.Items.Count > 0;
      if (this.gridViewHistory.SelectedItems == null || this.gridViewHistory.SelectedItems.Count != 1)
      {
        if (this.HistorySelectedIndexChanged == null)
          return;
        this.HistorySelectedIndexChanged((object) null, EventArgs.Empty);
      }
      else
      {
        if (this.HistorySelectedIndexChanged == null)
          return;
        this.HistorySelectedIndexChanged((object) (LoanCompHistory) this.gridViewHistory.SelectedItems[0].Tag, EventArgs.Empty);
      }
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (AssignLOCompDialog assignLoCompDialog = new AssignLOCompDialog(this.session, this.currentID))
      {
        if (this.forExternal)
        {
          if (!assignLoCompDialog.RefreshForm(this.loanCompHistoryList, 2))
            return;
        }
        else if (!assignLoCompDialog.RefreshForm(this.loanCompHistoryList))
          return;
        if (assignLoCompDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loanCompHistoryList.UseParentInfo = false;
        this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
        this.chkUseParent.Checked = false;
        this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
        this.loanCompHistoryList.AddHistory(assignLoCompDialog.NewLoanCompHistory);
        this.loanCompHistoryList.SortPlans(true);
        this.RefreshHistoryList(assignLoCompDialog.NewLoanCompHistory);
        if (this.AssignPlanButtonClicked == null)
          return;
        this.AssignPlanButtonClicked((object) assignLoCompDialog.NewLoanCompHistory, EventArgs.Empty);
      }
    }

    public bool UncheckParentInfo => this.uncheckParentInfo;

    public bool DataValidation(bool users)
    {
      string str = "Do you want to update all ";
      if (users)
        str += "users and ";
      string text = str + "child organizations associated with this Branch?";
      this.originalloanCompHistoryList.SortPlans(true);
      this.loanCompHistoryList.SortPlans(true);
      if (this.originalloanCompHistoryList.Count != this.loanCompHistoryList.Count || this.originalloanCompHistoryList.UseParentInfo != this.loanCompHistoryList.UseParentInfo)
      {
        DialogResult dialogResult = Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
        if (dialogResult == DialogResult.Cancel)
          return false;
        this.uncheckParentInfo = dialogResult != DialogResult.Yes;
        return true;
      }
      for (int i = 0; i < this.originalloanCompHistoryList.Count; ++i)
      {
        LoanCompHistory historyAt1 = this.loanCompHistoryList.GetHistoryAt(i);
        LoanCompHistory historyAt2 = this.originalloanCompHistoryList.GetHistoryAt(i);
        if (!(historyAt1.PlanName != historyAt2.PlanName))
        {
          DateTime dateTime = historyAt1.StartDate;
          DateTime date1 = dateTime.Date;
          dateTime = historyAt2.StartDate;
          DateTime date2 = dateTime.Date;
          if (!(date1 != date2))
          {
            dateTime = historyAt1.EndDate;
            DateTime date3 = dateTime.Date;
            dateTime = historyAt2.EndDate;
            DateTime date4 = dateTime.Date;
            if (!(date3 != date4))
              continue;
          }
        }
        DialogResult dialogResult = Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
        if (dialogResult == DialogResult.Cancel)
          return false;
        this.uncheckParentInfo = dialogResult != DialogResult.Yes;
        break;
      }
      return true;
    }

    public void SetReadOnly(bool readOnly)
    {
      this.stdIconBtnNew.Enabled = this.stdIconBtnDelete.Enabled = this.chkUseParent.Enabled = this.stdIconBtnZoom.Enabled = !readOnly;
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      List<GVItem> list = this.gridViewHistory.SelectedItems.ToList<GVItem>();
      int num1 = 0;
      for (int index = 0; index < list.Count; ++index)
      {
        if (((LoanCompHistory) list[index].Tag).StartDate.Date <= Session.ServerTime)
          ++num1;
      }
      if (num1 > 0)
      {
        if (num1 == list.Count)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete history plan or current plan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (Utils.Dialog((IWin32Window) this, "There " + (num1 > 1 ? "are " + (object) num1 : "is a") + " history plan" + (num1 > 1 ? "s" : "") + " that you cannot delete. Do you want to continue to delete other future plans?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
          return;
      }
      else if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected LO comp plan(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      for (int index = 0; index < list.Count; ++index)
      {
        LoanCompHistory tag = (LoanCompHistory) list[index].Tag;
        if (!(tag.StartDate.Date <= DateTime.Today.Date))
        {
          try
          {
            this.loanCompHistoryList.RemoveHistory(tag);
          }
          catch (Exception ex)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            continue;
          }
          this.gridViewHistory.Items.Remove(list[index]);
        }
      }
      this.loanCompHistoryList.UseParentInfo = false;
      this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
      this.chkUseParent.Checked = false;
      this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
      this.loanCompHistoryList.SortPlans(true);
      this.RefreshHistoryList((LoanCompHistory) null);
      if (this.DeletePlanButtonClicked == null)
        return;
      this.DeletePlanButtonClicked((object) null, EventArgs.Empty);
    }

    public void AutoSetGrid() => this.grpHistory_SizeChanged((object) null, (EventArgs) null);

    public bool GetUseParentInfo() => this.chkUseParent.Checked;

    public void DisableButtons(bool value)
    {
      this.stdIconBtnNew.Enabled = this.chkUseParent.Enabled = value;
      if (value)
        return;
      this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
      this.chkUseParent.Checked = value;
      this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
    }

    private void chkUseParent_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseParent.Checked)
      {
        LoanCompHistoryList loanCompHistoryList1 = (LoanCompHistoryList) null;
        if (this.forExternal)
        {
          loanCompHistoryList1 = this.session.ConfigurationManager.GetCompPlansByoid(this.forExternalLender, Utils.ParseInt((object) this.parentID));
        }
        else
        {
          OrgInfo organization = this.session.OrganizationManager.GetOrganization(Utils.ParseInt((object) this.parentID));
          if (organization != null)
            loanCompHistoryList1 = organization.LOCompHistoryList;
        }
        if (loanCompHistoryList1 != null)
        {
          this.loanCompHistoryList.GetCurrentPlan(this.session.SessionObjects.Session.ServerTime);
          List<LoanCompHistory> currentAndFuturePlans = loanCompHistoryList1.GetCurrentAndFuturePlans(this.session.SessionObjects.Session.ServerTime);
          if (this.loanCompHistoryList.GetFuturePlans(this.session.SessionObjects.Session.ServerTime).Count > 0 && Utils.Dialog((IWin32Window) this, "By using Parent Info, all LO Comp Plans assigned to this " + (this.forUser ? "user" : "branch") + " that are scheduled to go into effect on a future date will be unassigned. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          {
            this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
            this.chkUseParent.Checked = false;
            this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
            return;
          }
          if (currentAndFuturePlans != null)
          {
            LoanCompHistoryList loanCompHistoryList2 = this.loanCompHistoryList;
            List<LoanCompHistory> parentPlans = currentAndFuturePlans;
            DateTime dateTime = this.session.SessionObjects.Session.ServerTime;
            DateTime date1 = dateTime.Date;
            loanCompHistoryList2.AddParentPlans(parentPlans, date1);
            if (currentAndFuturePlans == null)
              return;
            dateTime = this.session.SessionObjects.Session.ServerTime;
            dateTime = dateTime.Date;
            dateTime = dateTime.AddDays(1.0);
            DateTime date2 = dateTime.Date;
            LoanCompHistory currentPlan;
            if (currentAndFuturePlans.Count > 0)
            {
              currentPlan = this.loanCompHistoryList.GetCurrentPlan(date2);
            }
            else
            {
              LoanCompHistoryList loanCompHistoryList3 = this.loanCompHistoryList;
              dateTime = this.session.SessionObjects.Session.ServerTime;
              DateTime date3 = dateTime.Date;
              currentPlan = loanCompHistoryList3.GetCurrentPlan(date3);
            }
            this.loanCompHistoryList.UseParentInfo = true;
            this.RefreshHistoryList(currentPlan ?? (currentAndFuturePlans == null || currentAndFuturePlans.Count <= 0 ? (LoanCompHistory) null : currentAndFuturePlans[0]));
            if (this.UseParentInfoClicked != null)
              this.UseParentInfoClicked((object) (currentPlan ?? (currentAndFuturePlans == null || currentAndFuturePlans.Count <= 0 ? (LoanCompHistory) null : currentAndFuturePlans[0])), EventArgs.Empty);
            if (currentAndFuturePlans.Count > 0)
            {
              string str1;
              if (currentPlan == null)
              {
                dateTime = currentAndFuturePlans[0].StartDate;
                str1 = dateTime.ToString("MM/dd/yyyy");
              }
              else
              {
                dateTime = currentPlan.StartDate;
                str1 = dateTime.ToString("MM/dd/yyyy");
              }
              string str2 = date2.ToString("MM/dd/yyyy");
              string str3;
              if (!(str1 == str2))
              {
                if (currentPlan == null)
                {
                  dateTime = currentAndFuturePlans[0].StartDate;
                  str3 = dateTime.ToString("MM/dd/yyyy");
                }
                else
                {
                  dateTime = currentPlan.StartDate;
                  str3 = dateTime.ToString("MM/dd/yyyy");
                }
              }
              else
                str3 = "tomorrow";
              string str4 = str3;
              int num = (int) Utils.Dialog((IWin32Window) this, "The parent branch’s LO Comp Plan '" + (currentPlan != null ? currentPlan.PlanName : currentAndFuturePlans[0].PlanName) + "' will go into effect " + str4 + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Currently there are no LO Comp Plans assigned to the parent branch. Plans assigned to the parent in the future will then be applied to this " + (this.forUser ? "user" : "branch") + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        this.chkUseParent.CheckedChanged -= new EventHandler(this.chkUseParent_CheckedChanged);
        this.loanCompHistoryList.UseParentInfo = true;
        this.chkUseParent.Checked = true;
        this.chkUseParent.CheckedChanged += new EventHandler(this.chkUseParent_CheckedChanged);
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Currently there are no LO Comp Plans assigned to the parent branch. Plans assigned to the parent in the future will then be applied to this " + (this.forUser ? "user" : "branch") + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.loanCompHistoryList.UseParentInfo = false;
        LoanCompHistory currentPlan = this.loanCompHistoryList.GetCurrentPlan(this.session.SessionObjects.Session.ServerTime.Date);
        if (this.UseParentInfoClicked != null)
          this.UseParentInfoClicked((object) currentPlan, EventArgs.Empty);
        this.gridViewHistory_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void stdIconBtnZoom_Click(object sender, EventArgs e)
    {
      LoanCompHistory tag = (LoanCompHistory) this.gridViewHistory.SelectedItems[0].Tag;
      using (LOCompDetailDialog compDetailDialog = new LOCompDetailDialog(this.session.ConfigurationManager.GetLoanCompPlanByID(tag.CompPlanId), Session.ConfigurationManager.GetDefaultLoanCompPlan(), tag))
      {
        int num = (int) compDetailDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void grpHistory_SizeChanged(object sender, EventArgs e)
    {
      this.gridViewHistory.Columns[0].Width = (int) ((double) this.gridViewHistory.Width * 0.64);
      this.gridViewHistory.Columns[1].Width = (int) ((double) this.gridViewHistory.Width * 0.18);
    }

    private void stdIconBtnPrint_Click(object sender, EventArgs e)
    {
      LoanCompDefaultPlan defaultLoanCompPlan = this.session.ConfigurationManager.GetDefaultLoanCompPlan();
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddHeaderColumn("Plan Name", "@");
        excelHandler.AddHeaderColumn("% Amount", "0.00000");
        excelHandler.AddHeaderColumn("$ Amount", "0.00");
        excelHandler.AddHeaderColumn("Minimum $", "0.00");
        excelHandler.AddHeaderColumn("Maximum $", "0.00");
        excelHandler.AddHeaderColumn("Start Date", "m/d/yyyy");
        excelHandler.AddHeaderColumn("Trigger Basis ", "@");
        excelHandler.AddHeaderColumn("Min # Days", "0");
        excelHandler.AddHeaderColumn("Loan Amount Type", "@");
        excelHandler.AddHeaderColumn("Rounding Type", "@");
        excelHandler.AddHeaderColumn("End Date", "m/d/yyyy");
        for (int nItemIndex = 0; nItemIndex < this.gridViewHistory.Items.Count; ++nItemIndex)
        {
          LoanCompHistory tag = (LoanCompHistory) this.gridViewHistory.Items[nItemIndex].Tag;
          string[] data = new string[11];
          data[0] = tag.PlanName;
          string[] strArray1 = data;
          Decimal num = tag.PercentAmt;
          string str1 = num.ToString("N5");
          strArray1[1] = str1;
          string[] strArray2 = data;
          num = tag.DollarAmount;
          string str2 = num.ToString("N2");
          strArray2[2] = str2;
          string[] strArray3 = data;
          num = tag.MinDollarAmount;
          string str3 = num.ToString("N2");
          strArray3[3] = str3;
          string[] strArray4 = data;
          num = tag.MaxDollarAmount;
          string str4 = num.ToString("N2");
          strArray4[4] = str4;
          string[] strArray5 = data;
          DateTime dateTime = tag.StartDate;
          string str5 = dateTime.ToString("MM/dd/yyyy");
          strArray5[5] = str5;
          data[6] = defaultLoanCompPlan.TriggerField;
          data[7] = tag.MinTermDays.ToString("0");
          data[8] = tag.PercentAmtBaseToUIStr;
          data[9] = tag.RoundingMethodToUIStr;
          string[] strArray6 = data;
          dateTime = tag.EndDate;
          DateTime date1 = dateTime.Date;
          dateTime = DateTime.MinValue;
          DateTime date2 = dateTime.Date;
          string str6;
          if (!(date1 == date2))
          {
            dateTime = tag.EndDate;
            DateTime date3 = dateTime.Date;
            dateTime = DateTime.MaxValue;
            DateTime date4 = dateTime.Date;
            if (!(date3 == date4))
            {
              dateTime = tag.EndDate;
              dateTime = dateTime.Date;
              str6 = dateTime.ToString("MM/dd/yyyy");
              goto label_6;
            }
          }
          str6 = "";
label_6:
          strArray6[10] = str6;
          excelHandler.AddDataRow(data);
        }
        excelHandler.Export(true);
      }
    }

    public void DisableControls()
    {
      this.stdIconBtnDelete.Visible = this.stdIconBtnNew.Visible = false;
      this.stdIconBtnZoom.Left = this.stdIconBtnDelete.Left;
      this.disableControl(this.Controls);
      if (this.gridViewHistory.Items.Count > 0)
      {
        this.gridViewHistory.SelectedIndexChanged -= new EventHandler(this.gridViewHistory_SelectedIndexChanged);
        this.stdIconBtnZoom.Enabled = true;
        this.stdIconBtnPrint.Enabled = true;
        this.gridViewHistory.Items[0].Selected = true;
        this.gridViewHistory.SelectedIndexChanged += new EventHandler(this.gridViewHistory_SelectedIndexChanged);
      }
      else
        this.stdIconBtnZoom.Visible = this.stdIconBtnPrint.Visible = false;
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
      }
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
      this.grpHistory = new GroupContainer();
      this.stdIconBtnPrint = new StandardIconButton();
      this.stdIconBtnZoom = new StandardIconButton();
      this.chkUseParent = new CheckBox();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gridViewHistory = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.grpHistory.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnPrint).BeginInit();
      ((ISupportInitialize) this.stdIconBtnZoom).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.grpHistory.Controls.Add((Control) this.stdIconBtnPrint);
      this.grpHistory.Controls.Add((Control) this.stdIconBtnZoom);
      this.grpHistory.Controls.Add((Control) this.chkUseParent);
      this.grpHistory.Controls.Add((Control) this.stdIconBtnNew);
      this.grpHistory.Controls.Add((Control) this.stdIconBtnDelete);
      this.grpHistory.Controls.Add((Control) this.gridViewHistory);
      this.grpHistory.Dock = DockStyle.Fill;
      this.grpHistory.HeaderForeColor = SystemColors.ControlText;
      this.grpHistory.Location = new Point(0, 0);
      this.grpHistory.Name = "grpHistory";
      this.grpHistory.Size = new Size(464, 507);
      this.grpHistory.TabIndex = 0;
      this.grpHistory.Text = "Assigned Compensation Plans";
      this.grpHistory.SizeChanged += new EventHandler(this.grpHistory_SizeChanged);
      this.stdIconBtnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnPrint.BackColor = Color.Transparent;
      this.stdIconBtnPrint.Location = new Point(441, 5);
      this.stdIconBtnPrint.MouseDownImage = (Image) null;
      this.stdIconBtnPrint.Name = "stdIconBtnPrint";
      this.stdIconBtnPrint.Size = new Size(16, 16);
      this.stdIconBtnPrint.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdIconBtnPrint.TabIndex = 15;
      this.stdIconBtnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnPrint, "Export Comp Plan Assignment History ");
      this.stdIconBtnPrint.Click += new EventHandler(this.stdIconBtnPrint_Click);
      this.stdIconBtnZoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnZoom.BackColor = Color.Transparent;
      this.stdIconBtnZoom.Location = new Point(399, 5);
      this.stdIconBtnZoom.MouseDownImage = (Image) null;
      this.stdIconBtnZoom.Name = "stdIconBtnZoom";
      this.stdIconBtnZoom.Size = new Size(16, 16);
      this.stdIconBtnZoom.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.stdIconBtnZoom.TabIndex = 14;
      this.stdIconBtnZoom.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnZoom, "View Comp Plan Details");
      this.stdIconBtnZoom.Click += new EventHandler(this.stdIconBtnZoom_Click);
      this.chkUseParent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParent.AutoSize = true;
      this.chkUseParent.BackColor = Color.Transparent;
      this.chkUseParent.Location = new Point(272, 5);
      this.chkUseParent.Name = "chkUseParent";
      this.chkUseParent.Size = new Size(100, 17);
      this.chkUseParent.TabIndex = 1;
      this.chkUseParent.Text = "Use Parent Info";
      this.chkUseParent.UseVisualStyleBackColor = false;
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(378, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 12;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "Add Comp Plan");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(420, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 11;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Comp Plan");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.gridViewHistory.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnPlanName";
      gvColumn1.Text = "Plan Name";
      gvColumn1.Width = 298;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnStartDate";
      gvColumn2.SortMethod = GVSortMethod.Date;
      gvColumn2.Text = "Start Date";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 82;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnEndDate";
      gvColumn3.SortMethod = GVSortMethod.Date;
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "End Date";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 82;
      this.gridViewHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewHistory.Dock = DockStyle.Fill;
      this.gridViewHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewHistory.Location = new Point(1, 26);
      this.gridViewHistory.Name = "gridViewHistory";
      this.gridViewHistory.Size = new Size(462, 480);
      this.gridViewHistory.TabIndex = 1;
      this.gridViewHistory.TabStop = false;
      this.gridViewHistory.SelectedIndexChanged += new EventHandler(this.gridViewHistory_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpHistory);
      this.Name = nameof (LOCompHistoryControl);
      this.Size = new Size(464, 507);
      this.grpHistory.ResumeLayout(false);
      this.grpHistory.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnPrint).EndInit();
      ((ISupportInitialize) this.stdIconBtnZoom).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
