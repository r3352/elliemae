// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompensationList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompensationList : SettingsUserControl
  {
    private ImageList imageList;
    private Sessions.Session session;
    private LoanCompDefaultPlan loanCompDefaultPlan;
    private FeaturesAclManager aclMgr;
    private string containerHeader = "LO Compensation Plans";
    private GridView listView;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDuplicate;
    private IContainer components;
    private Panel panelDefault;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelList;
    private GroupContainer grpcDefault;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private ComboBox cmbRounding;
    private TextBox txtTriggerField;
    private TextBox textMinTerm;
    private ComboBox cmbType;
    private ComboBox cboPaidBy;
    private CheckBox chkDefaultInvestment;
    private CheckBox chkDefaultHELOC;
    private StandardIconButton stdIconBtnReset;
    private StandardIconButton stdIconBtnSave;
    private StandardIconButton stdIconBtnExport;

    public LOCompensationList(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer)
    {
    }

    public LOCompensationList(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.aclMgr = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      this.InitializeComponent();
      this.imageList = new ImageList();
      this.imageList.Images.Add(Image.FromFile(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.ImageRelDir, "check-mark-green.png"), SystemSettings.LocalAppDir)));
      this.listView.ImageList = this.imageList;
      this.initForm();
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void initForm()
    {
      this.Reset();
      this.listView.Items.Clear();
      this.listView.BeginUpdate();
      List<LoanCompPlan> allCompPlans = this.session.ConfigurationManager.GetAllCompPlans(false, 0);
      if (allCompPlans != null)
      {
        foreach (LoanCompPlan lcp in allCompPlans)
          this.listView.Items.Add(this.buildGVItem(lcp, false));
      }
      this.listView.Sort(2, SortOrder.Ascending);
      this.listView.EndUpdate();
      this.listView_SelectedIndexChanged((object) null, (EventArgs) null);
      this.refreshListViewHeader();
    }

    private GVItem buildGVItem(LoanCompPlan lcp, bool selected)
    {
      GVItem gvItem = new GVItem((object) lcp.Id);
      gvItem.SubItems.Add((object) lcp.Name);
      gvItem.SubItems.Add((object) lcp.Description);
      gvItem.SubItems.Add(lcp.Status ? (object) "Yes" : (object) "No");
      gvItem.SubItems.Add(!(lcp.ActivationDate != DateTime.MaxValue) || !(lcp.ActivationDate != DateTime.MinValue) ? (object) "" : (object) lcp.ActivationDate.ToString("MM/dd/yyyy"));
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems.Add((object) "");
      if (lcp.PlanType == LoanCompPlanType.LoanOfficer || lcp.PlanType == LoanCompPlanType.Both)
      {
        gvItem.SubItems[5].ImageIndex = 0;
        gvItem.SubItems[5].Text = " ";
      }
      else
        gvItem.SubItems[5].ImageIndex = -1;
      if (lcp.PlanType == LoanCompPlanType.Broker || lcp.PlanType == LoanCompPlanType.Both)
      {
        gvItem.SubItems[6].ImageIndex = 0;
        gvItem.SubItems[6].Text = " ";
      }
      else
        gvItem.SubItems[6].ImageIndex = -1;
      gvItem.Selected = selected;
      gvItem.Tag = (object) lcp;
      return gvItem;
    }

    private void refreshListViewHeader()
    {
      this.gContainer.Text = this.containerHeader + " (" + (object) this.listView.Items.Count + ")";
      this.stdIconBtnExport.Enabled = this.listView.Items.Count > 0;
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      if (this.IsDirty)
      {
        if (Utils.Dialog((IWin32Window) this, "The Default Plan Settings have been changed. Would you like to save it before creating a new LO Compensation?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK || !this.saveDefaultSettings())
          return;
        this.setDirtyFlag(false);
        this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = false;
      }
      using (LOCompGroupDialog loCompGroupDialog = new LOCompGroupDialog(this.session, this.loanCompDefaultPlan, (LoanCompPlan) null))
      {
        if (loCompGroupDialog.ShowDialog() != DialogResult.OK)
          return;
        this.listView.SelectedItems.Clear();
        this.listView.Items.Add(this.buildGVItem(loCompGroupDialog.CurrentLoanCompPlans, true));
        this.refreshListViewHeader();
      }
    }

    private void editSelectedItem()
    {
      if (this.IsDirty)
      {
        if (Utils.Dialog((IWin32Window) this, "The Default Plan Settings have been changed. Would you like to save it before creating a new LO Compensation?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK)
          return;
        this.Save();
      }
      LoanCompPlan tag = (LoanCompPlan) this.listView.SelectedItems[0].Tag;
      using (LOCompGroupDialog loCompGroupDialog = new LOCompGroupDialog(this.session, this.loanCompDefaultPlan, tag))
      {
        if (loCompGroupDialog.ShowDialog() != DialogResult.OK)
          return;
        this.listView.SelectedItems[0].Text = loCompGroupDialog.CurrentLoanCompPlans.Id.ToString();
        this.listView.SelectedItems[0].SubItems[1].Text = loCompGroupDialog.CurrentLoanCompPlans.Name;
        this.listView.SelectedItems[0].SubItems[2].Text = loCompGroupDialog.CurrentLoanCompPlans.Description;
        this.listView.SelectedItems[0].SubItems[3].Text = loCompGroupDialog.CurrentLoanCompPlans.Status ? "Yes" : "No";
        this.listView.SelectedItems[0].SubItems[4].Text = !(loCompGroupDialog.CurrentLoanCompPlans.ActivationDate != DateTime.MaxValue) || !(loCompGroupDialog.CurrentLoanCompPlans.ActivationDate != DateTime.MinValue) ? "" : loCompGroupDialog.CurrentLoanCompPlans.ActivationDate.ToString("MM/dd/yyyy");
        this.listView.SelectedItems[0].SubItems[5].Text = "";
        this.listView.SelectedItems[0].SubItems[6].Text = "";
        if (loCompGroupDialog.CurrentLoanCompPlans.PlanType == LoanCompPlanType.LoanOfficer || loCompGroupDialog.CurrentLoanCompPlans.PlanType == LoanCompPlanType.Both)
        {
          this.listView.SelectedItems[0].SubItems[5].ImageIndex = 0;
          this.listView.SelectedItems[0].SubItems[5].Text = " ";
        }
        else
          this.listView.SelectedItems[0].SubItems[5].ImageIndex = -1;
        if (loCompGroupDialog.CurrentLoanCompPlans.PlanType == LoanCompPlanType.Broker || loCompGroupDialog.CurrentLoanCompPlans.PlanType == LoanCompPlanType.Both)
        {
          this.listView.SelectedItems[0].SubItems[6].ImageIndex = 0;
          this.listView.SelectedItems[0].SubItems[5].Text = " ";
        }
        else
          this.listView.SelectedItems[0].SubItems[6].ImageIndex = -1;
        this.listView.SelectedItems[0].Tag = (object) tag;
        this.refreshListViewHeader();
      }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "The selected LO Comp Plans will be deleted permanently. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      int index1 = this.listView.SelectedItems[0].Index;
      List<int> intList1 = new List<int>();
      foreach (GVItem selectedItem in this.listView.SelectedItems)
      {
        LoanCompPlan tag = (LoanCompPlan) selectedItem.Tag;
        intList1.Add(tag.Id);
      }
      try
      {
        List<int> intList2 = Session.ConfigurationManager.RemoveCompPlans(intList1.ToArray());
        string str = string.Empty;
        for (int index2 = this.listView.SelectedItems.Count - 1; index2 >= 0; --index2)
        {
          LoanCompPlan tag = (LoanCompPlan) this.listView.SelectedItems[index2].Tag;
          if (intList2 != null && intList2.Contains(tag.Id))
            this.listView.Items.Remove(this.listView.SelectedItems[index2]);
          else
            str = str + (str != string.Empty ? "," : "") + "\"" + tag.Name + "\"";
        }
        if (intList2.Count != intList1.Count)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You cannot delete the following Comp plans that are associated with either an organization, broker or user currently:\r\n\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.refreshListViewHeader();
        if (this.listView.Items.Count == 0)
          return;
        this.listView.SelectedItems.Clear();
        if (this.listView.Items.Count < index1 + 1)
          this.listView.Items[this.listView.Items.Count - 1].Selected = true;
        else
          this.listView.Items[index1].Selected = true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void listView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDuplicate.Enabled = this.listView.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count > 0 && (this.session.UserInfo.IsAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LOCompensation));
    }

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void stdIconBtnDuplicate_Click(object sender, EventArgs e)
    {
      LoanCompPlan lcp = (LoanCompPlan) ((LoanCompPlan) this.listView.SelectedItems[0].Tag).Clone();
      lcp.Status = false;
      using (LOCompGroupDialog loCompGroupDialog = new LOCompGroupDialog(this.session, this.loanCompDefaultPlan, lcp))
      {
        if (loCompGroupDialog.ShowDialog() != DialogResult.OK)
          return;
        this.listView.SelectedItems.Clear();
        this.listView.Items.Add(this.buildGVItem(loCompGroupDialog.CurrentLoanCompPlans, true));
        this.refreshListViewHeader();
      }
    }

    private void listView_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void textMinTerm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? Any changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.Reset();
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e) => this.Save();

    public override void Reset()
    {
      this.loanCompDefaultPlan = Session.ConfigurationManager.GetDefaultLoanCompPlan();
      this.cmbType.SelectedIndex = this.loanCompDefaultPlan.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (this.loanCompDefaultPlan.PlanType == LoanCompPlanType.Broker ? 2 : (this.loanCompDefaultPlan.PlanType == LoanCompPlanType.Both ? 3 : 0));
      this.txtTriggerField.Text = this.loanCompDefaultPlan.TriggerField;
      this.textMinTerm.Text = this.loanCompDefaultPlan.MinTermDays == 0 ? "" : this.loanCompDefaultPlan.MinTermDays.ToString();
      this.cmbRounding.SelectedIndex = this.loanCompDefaultPlan.RoundingMethod > 1 ? 1 : 0;
      this.chkDefaultHELOC.Checked = (this.loanCompDefaultPlan.LoansExempt & 1) == 1;
      this.chkDefaultInvestment.Checked = (this.loanCompDefaultPlan.LoansExempt & 4) == 4;
      this.cboPaidBy.SelectedIndex = this.loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Lender ? 0 : (this.loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Borrower ? 1 : (this.loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Exempt ? 2 : 0));
      this.setDirtyFlag(false);
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = false;
    }

    public override void Save()
    {
      if (!this.saveDefaultSettings())
        return;
      this.setDirtyFlag(false);
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = false;
    }

    private bool saveDefaultSettings()
    {
      if (string.IsNullOrEmpty(this.txtTriggerField.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Trigger Basis field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtTriggerField.Focus();
        return false;
      }
      FieldDefinition fieldDefinition = !this.txtTriggerField.Text.ToUpper().StartsWith("CX.") ? EncompassFields.GetField(this.txtTriggerField.Text) : EncompassFields.GetField(this.txtTriggerField.Text, this.session.LoanManager.GetFieldSettings());
      if (fieldDefinition == null || !fieldDefinition.IsDateValued())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Trigger Basis field is not a date field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtTriggerField.Focus();
        return false;
      }
      if (this.textMinTerm.Text != string.Empty && Utils.ParseInt((object) this.textMinTerm.Text) > 36500)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Minimum Term # Days field cannot be greater than 36500.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textMinTerm.Focus();
        return false;
      }
      if (this.loanCompDefaultPlan == null)
        this.loanCompDefaultPlan = new LoanCompDefaultPlan();
      this.loanCompDefaultPlan.PlanType = (LoanCompPlanType) Enum.ToObject(typeof (LoanCompPlanType), this.cmbType.SelectedIndex);
      this.loanCompDefaultPlan.TriggerField = this.txtTriggerField.Text.Trim();
      this.loanCompDefaultPlan.MinTermDays = this.textMinTerm.Text.Trim() != "" ? Utils.ParseInt((object) this.textMinTerm.Text.Trim()) : 0;
      this.loanCompDefaultPlan.RoundingMethod = this.cmbRounding.SelectedIndex == 1 ? 2 : 1;
      this.loanCompDefaultPlan.LoansExempt = (this.chkDefaultHELOC.Checked ? 1 : 0) | 0 | (this.chkDefaultInvestment.Checked ? 4 : 0);
      this.loanCompDefaultPlan.PaidBy = (LoanCompPaidBy) Enum.ToObject(typeof (LoanCompPaidBy), this.cboPaidBy.SelectedIndex + 1);
      try
      {
        this.session.ConfigurationManager.SetLoanCompDefaultPlan(this.loanCompDefaultPlan);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The default plan settings cannot be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        return false;
      }
      return true;
    }

    private void defaultFields_Changed(object sender, EventArgs e)
    {
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void stdIconBtnExport_Click(object sender, EventArgs e)
    {
      bool flag = false;
      using (LOCompExportOptionForm exportOptionForm = new LOCompExportOptionForm())
      {
        if (exportOptionForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        flag = exportOptionForm.SortedByPlan;
      }
      if (flag)
        this.exportAndSortedByPlan();
      else
        this.exportAndSortedByLO();
    }

    private void exportAndSortedByPlan()
    {
      Cursor.Current = Cursors.WaitCursor;
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddHeaderColumn("Plan Name", "@");
        excelHandler.AddHeaderColumn("Description", "@");
        excelHandler.AddHeaderColumn("Status", "@");
        excelHandler.AddHeaderColumn("LO/Broker", "@");
        excelHandler.AddHeaderColumn("Min # Days", "0");
        excelHandler.AddHeaderColumn("Activation Date", "m/d/yyyy");
        excelHandler.AddHeaderColumn("Rounding Type", "@");
        excelHandler.AddHeaderColumn("Loan Amount %", "0.00000");
        excelHandler.AddHeaderColumn("% of", "@");
        excelHandler.AddHeaderColumn("Additional $ Amount", "0.00");
        excelHandler.AddHeaderColumn("Minimum $", "0.00");
        excelHandler.AddHeaderColumn("Maximum $", "0.00");
        for (int nItemIndex = 0; nItemIndex < this.listView.Items.Count; ++nItemIndex)
        {
          LoanCompPlan tag = (LoanCompPlan) this.listView.Items[nItemIndex].Tag;
          string[] data = new string[12]
          {
            tag.Name,
            tag.Description,
            tag.Status ? "Activate" : "",
            tag.PlanType == LoanCompPlanType.LoanOfficer ? "Loan Officer" : (tag.PlanType == LoanCompPlanType.Broker ? "Broker" : (tag.PlanType == LoanCompPlanType.Both ? "Both" : "")),
            tag.MinTermDays.ToString("0"),
            !(tag.ActivationDate != DateTime.MinValue) || !(tag.ActivationDate != DateTime.MaxValue) ? "" : tag.ActivationDate.ToString("MM/dd/yyyy"),
            tag.RoundingMethod > 1 ? "To Nearest $" : "",
            tag.PercentAmt.ToString("N5"),
            tag.PercentAmtBase == 2 ? "Base Loan" : (tag.PercentAmtBase == 1 ? "Total Loan" : ""),
            tag.DollarAmount.ToString("N2"),
            tag.MinDollarAmount.ToString("N2"),
            tag.MaxDollarAmount.ToString("N2")
          };
          excelHandler.AddDataRow(data);
        }
        excelHandler.Export(true);
      }
      Cursor.Current = Cursors.Default;
    }

    private void exportAndSortedByLO()
    {
      List<LoanCompHistoryList> historyforAllOriginators;
      try
      {
        historyforAllOriginators = this.session.ConfigurationManager.GetComPlanHistoryforAllOriginators();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot get LO Compensation list due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      if (historyforAllOriginators == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Cannot get LO Compensation list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Hashtable hashtable = new Hashtable();
        for (int nItemIndex = 0; nItemIndex < this.listView.Items.Count; ++nItemIndex)
        {
          LoanCompPlan tag = (LoanCompPlan) this.listView.Items[nItemIndex].Tag;
          hashtable.Add((object) tag.Id, (object) tag);
        }
        string[] data = new string[15];
        Cursor.Current = Cursors.WaitCursor;
        using (CursorActivator.Wait())
        {
          ExcelHandler excelHandler = new ExcelHandler();
          excelHandler.AddHeaderColumn("Originator's Name", "@");
          excelHandler.AddHeaderColumn("Plan Name", "@");
          excelHandler.AddHeaderColumn("Start Date", "m/d/yyyy");
          excelHandler.AddHeaderColumn("End Date", "m/d/yyyy");
          excelHandler.AddHeaderColumn("Description", "@");
          excelHandler.AddHeaderColumn("Status", "@");
          excelHandler.AddHeaderColumn("LO/Broker", "@");
          excelHandler.AddHeaderColumn("Min # Days", "0");
          excelHandler.AddHeaderColumn("Activation Date", "m/d/yyyy");
          excelHandler.AddHeaderColumn("Rounding Type", "@");
          excelHandler.AddHeaderColumn("Loan Amount %", "0.00000");
          excelHandler.AddHeaderColumn("% of", "@");
          excelHandler.AddHeaderColumn("Additional $ Amount", "0.00");
          excelHandler.AddHeaderColumn("Minimum $", "0.00");
          excelHandler.AddHeaderColumn("Maximum $", "0.00");
          for (int index1 = 0; index1 < historyforAllOriginators.Count; ++index1)
          {
            LoanCompHistoryList loanCompHistoryList = historyforAllOriginators[index1];
            if (index1 > 0)
            {
              for (int index2 = 0; index2 < data.Length; ++index2)
                data[index2] = string.Empty;
              excelHandler.AddDataRow(data);
            }
            for (int i = 0; i < loanCompHistoryList.Count; ++i)
            {
              LoanCompHistory historyAt = loanCompHistoryList.GetHistoryAt(i);
              LoanCompPlan loanCompPlan = (LoanCompPlan) null;
              if (hashtable.ContainsKey((object) historyAt.CompPlanId))
                loanCompPlan = (LoanCompPlan) hashtable[(object) historyAt.CompPlanId];
              data[0] = i > 0 ? "" : loanCompHistoryList.UserName;
              data[1] = historyAt.PlanName;
              string[] strArray1 = data;
              DateTime dateTime = historyAt.StartDate;
              DateTime date1 = dateTime.Date;
              dateTime = DateTime.MinValue;
              DateTime date2 = dateTime.Date;
              string str1;
              if (date1 != date2)
              {
                dateTime = historyAt.StartDate;
                DateTime date3 = dateTime.Date;
                dateTime = DateTime.MaxValue;
                DateTime date4 = dateTime.Date;
                if (date3 != date4)
                {
                  dateTime = historyAt.StartDate;
                  str1 = dateTime.ToString("MM/dd/yyyy");
                  goto label_22;
                }
              }
              str1 = "";
label_22:
              strArray1[2] = str1;
              string[] strArray2 = data;
              dateTime = historyAt.EndDate;
              DateTime date5 = dateTime.Date;
              dateTime = DateTime.MinValue;
              DateTime date6 = dateTime.Date;
              string str2;
              if (date5 != date6)
              {
                dateTime = historyAt.EndDate;
                DateTime date7 = dateTime.Date;
                dateTime = DateTime.MaxValue;
                DateTime date8 = dateTime.Date;
                if (date7 != date8)
                {
                  dateTime = historyAt.EndDate;
                  str2 = dateTime.ToString("MM/dd/yyyy");
                  goto label_26;
                }
              }
              str2 = "";
label_26:
              strArray2[3] = str2;
              if (loanCompPlan == null)
              {
                data[4] = string.Empty;
                data[5] = string.Empty;
                data[6] = string.Empty;
                data[8] = string.Empty;
              }
              else
              {
                data[4] = loanCompPlan.Description;
                data[5] = loanCompPlan.Status ? "Activate" : "";
                data[6] = loanCompPlan.PlanType == LoanCompPlanType.LoanOfficer ? "Loan Officer" : (loanCompPlan.PlanType == LoanCompPlanType.Broker ? "Broker" : (loanCompPlan.PlanType == LoanCompPlanType.Both ? "Both" : ""));
                string[] strArray3 = data;
                string str3;
                if (!(loanCompPlan.ActivationDate != DateTime.MinValue) || !(loanCompPlan.ActivationDate != DateTime.MaxValue))
                {
                  str3 = "";
                }
                else
                {
                  dateTime = loanCompPlan.ActivationDate;
                  str3 = dateTime.ToString("MM/dd/yyyy");
                }
                strArray3[8] = str3;
              }
              data[7] = historyAt.MinTermDays.ToString("0");
              data[9] = historyAt.RoundingMethod > 1 ? "To Nearest $" : "";
              data[10] = historyAt.PercentAmt.ToString("N5");
              data[11] = historyAt.PercentAmtBase == 2 ? "Base Loan" : (historyAt.PercentAmtBase == 1 ? "Total Loan" : "");
              data[12] = historyAt.DollarAmount.ToString("N2");
              data[13] = historyAt.MinDollarAmount.ToString("N2");
              data[14] = historyAt.MaxDollarAmount.ToString("N2");
              excelHandler.AddDataRow(data);
            }
          }
          excelHandler.Export(true);
        }
        Cursor.Current = Cursors.Default;
      }
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
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnSave = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnExport = new StandardIconButton();
      this.panelDefault = new Panel();
      this.grpcDefault = new GroupContainer();
      this.cboPaidBy = new ComboBox();
      this.chkDefaultInvestment = new CheckBox();
      this.chkDefaultHELOC = new CheckBox();
      this.cmbType = new ComboBox();
      this.textMinTerm = new TextBox();
      this.txtTriggerField = new TextBox();
      this.cmbRounding = new ComboBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelList = new Panel();
      this.gContainer = new GroupContainer();
      this.listView = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnExport).BeginInit();
      this.panelDefault.SuspendLayout();
      this.grpcDefault.SuspendLayout();
      this.panelList.SuspendLayout();
      this.gContainer.SuspendLayout();
      this.SuspendLayout();
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(888, 4);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(840, 4);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 10;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(912, 4);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 9;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(934, 3);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 35;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(912, 3);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 34;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.stdIconBtnSave_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(864, 4);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 13;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.stdIconBtnDuplicate_Click);
      this.stdIconBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnExport.BackColor = Color.Transparent;
      this.stdIconBtnExport.Location = new Point(934, 4);
      this.stdIconBtnExport.MouseDownImage = (Image) null;
      this.stdIconBtnExport.Name = "stdIconBtnExport";
      this.stdIconBtnExport.Size = new Size(16, 16);
      this.stdIconBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdIconBtnExport.TabIndex = 14;
      this.stdIconBtnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnExport, "Export");
      this.stdIconBtnExport.Click += new EventHandler(this.stdIconBtnExport_Click);
      this.panelDefault.Controls.Add((Control) this.grpcDefault);
      this.panelDefault.Dock = DockStyle.Bottom;
      this.panelDefault.Location = new Point(0, 537);
      this.panelDefault.Name = "panelDefault";
      this.panelDefault.Size = new Size(957, 116);
      this.panelDefault.TabIndex = 9;
      this.grpcDefault.Controls.Add((Control) this.stdIconBtnReset);
      this.grpcDefault.Controls.Add((Control) this.stdIconBtnSave);
      this.grpcDefault.Controls.Add((Control) this.cboPaidBy);
      this.grpcDefault.Controls.Add((Control) this.chkDefaultInvestment);
      this.grpcDefault.Controls.Add((Control) this.chkDefaultHELOC);
      this.grpcDefault.Controls.Add((Control) this.cmbType);
      this.grpcDefault.Controls.Add((Control) this.textMinTerm);
      this.grpcDefault.Controls.Add((Control) this.txtTriggerField);
      this.grpcDefault.Controls.Add((Control) this.cmbRounding);
      this.grpcDefault.Controls.Add((Control) this.label6);
      this.grpcDefault.Controls.Add((Control) this.label5);
      this.grpcDefault.Controls.Add((Control) this.label4);
      this.grpcDefault.Controls.Add((Control) this.label3);
      this.grpcDefault.Controls.Add((Control) this.label2);
      this.grpcDefault.Controls.Add((Control) this.label1);
      this.grpcDefault.Dock = DockStyle.Fill;
      this.grpcDefault.HeaderForeColor = SystemColors.ControlText;
      this.grpcDefault.Location = new Point(0, 0);
      this.grpcDefault.Name = "grpcDefault";
      this.grpcDefault.Size = new Size(957, 116);
      this.grpcDefault.TabIndex = 0;
      this.grpcDefault.Text = "Default Plan Settings";
      this.cboPaidBy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPaidBy.FormattingEnabled = true;
      this.cboPaidBy.Items.AddRange(new object[3]
      {
        (object) "Lender Paid",
        (object) "Borrower Paid",
        (object) "Exempt"
      });
      this.cboPaidBy.Location = new Point(562, 84);
      this.cboPaidBy.Name = "cboPaidBy";
      this.cboPaidBy.Size = new Size(115, 21);
      this.cboPaidBy.TabIndex = 33;
      this.cboPaidBy.SelectedIndexChanged += new EventHandler(this.defaultFields_Changed);
      this.chkDefaultInvestment.AutoSize = true;
      this.chkDefaultInvestment.Location = new Point(630, 39);
      this.chkDefaultInvestment.Name = "chkDefaultInvestment";
      this.chkDefaultInvestment.Size = new Size(120, 17);
      this.chkDefaultInvestment.TabIndex = 32;
      this.chkDefaultInvestment.Text = "Investment Property\r\n";
      this.chkDefaultInvestment.UseVisualStyleBackColor = true;
      this.chkDefaultInvestment.CheckedChanged += new EventHandler(this.defaultFields_Changed);
      this.chkDefaultHELOC.AutoSize = true;
      this.chkDefaultHELOC.Location = new Point(562, 39);
      this.chkDefaultHELOC.Name = "chkDefaultHELOC";
      this.chkDefaultHELOC.Size = new Size(62, 17);
      this.chkDefaultHELOC.TabIndex = 30;
      this.chkDefaultHELOC.Text = "HELOC";
      this.chkDefaultHELOC.UseVisualStyleBackColor = true;
      this.chkDefaultHELOC.CheckedChanged += new EventHandler(this.defaultFields_Changed);
      this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Loan Officer",
        (object) "Broker",
        (object) "Both"
      });
      this.cmbType.Location = new Point(157, 37);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(103, 21);
      this.cmbType.TabIndex = 29;
      this.cmbType.SelectedIndexChanged += new EventHandler(this.defaultFields_Changed);
      this.textMinTerm.Location = new Point(157, 84);
      this.textMinTerm.MaxLength = 5;
      this.textMinTerm.Name = "textMinTerm";
      this.textMinTerm.Size = new Size(103, 20);
      this.textMinTerm.TabIndex = 28;
      this.textMinTerm.TextAlign = HorizontalAlignment.Right;
      this.textMinTerm.TextChanged += new EventHandler(this.defaultFields_Changed);
      this.textMinTerm.KeyPress += new KeyPressEventHandler(this.textMinTerm_KeyPress);
      this.txtTriggerField.Location = new Point(157, 61);
      this.txtTriggerField.Name = "txtTriggerField";
      this.txtTriggerField.Size = new Size(103, 20);
      this.txtTriggerField.TabIndex = 27;
      this.txtTriggerField.TextChanged += new EventHandler(this.defaultFields_Changed);
      this.cmbRounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRounding.FormattingEnabled = true;
      this.cmbRounding.Items.AddRange(new object[2]
      {
        (object) "",
        (object) "To Nearest $"
      });
      this.cmbRounding.Location = new Point(562, 61);
      this.cmbRounding.Name = "cmbRounding";
      this.cmbRounding.Size = new Size(115, 21);
      this.cmbRounding.TabIndex = 20;
      this.cmbRounding.SelectedIndexChanged += new EventHandler(this.defaultFields_Changed);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(373, 87);
      this.label6.Name = "label6";
      this.label6.Size = new Size(175, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Lender Paid/Borrower Paid/Exempt";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(373, 40);
      this.label5.Name = "label5";
      this.label5.Size = new Size(101, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Loans to be Exempt";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(373, 64);
      this.label4.Name = "label4";
      this.label4.Size = new Size(53, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Rounding";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 87);
      this.label3.Name = "label3";
      this.label3.Size = new Size(112, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Minimum Term # Days";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 64);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Trigger Basis";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(131, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Officer/Broker Value";
      this.panelList.Controls.Add((Control) this.gContainer);
      this.panelList.Dock = DockStyle.Fill;
      this.panelList.Location = new Point(0, 0);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(957, 530);
      this.panelList.TabIndex = 11;
      this.gContainer.Controls.Add((Control) this.stdIconBtnExport);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listView);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(957, 530);
      this.gContainer.TabIndex = 8;
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "id";
      gvColumn1.Text = "ID";
      gvColumn1.Width = 0;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 75;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Description";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Active";
      gvColumn4.Text = "Active";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ActivationDate";
      gvColumn5.Text = "Activation Date";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "LoanOfficer";
      gvColumn6.Text = "Loan Officer";
      gvColumn6.Width = 120;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Broker";
      gvColumn7.Text = "Broker";
      gvColumn7.Width = 100;
      this.listView.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(955, 503);
      this.listView.TabIndex = 7;
      this.listView.ItemDoubleClick += new GVItemEventHandler(this.listView_ItemDoubleClick);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelDefault;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 530);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 10;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.Controls.Add((Control) this.panelList);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelDefault);
      this.Name = nameof (LOCompensationList);
      this.Size = new Size(957, 653);
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnExport).EndInit();
      this.panelDefault.ResumeLayout(false);
      this.grpcDefault.ResumeLayout(false);
      this.grpcDefault.PerformLayout();
      this.panelList.ResumeLayout(false);
      this.gContainer.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
