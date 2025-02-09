// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.AlertEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class AlertEditor : Form
  {
    private Dictionary<WorksheetInfo, string> alertMsgsToUpdate;
    private FieldSettings fieldSettings;
    private AlertConfig currentAlert;
    private FieldDefinition triggerFieldDef;
    private Sessions.Session session;
    private bool initFlag;
    private IContainer components;
    private Button btnReset;
    private Button btnSave;
    private Label lblAlertName;
    private TextBox txtName;
    private TextBox txtMessage;
    private Label lblMessage;
    private GroupContainer grpFields;
    private GroupContainer grpDataCompletionFields;
    private GridView gvFields;
    private GridView gvDataCompletionFields;
    private Panel pnlName;
    private Panel pnlMessage;
    private Panel pnlTrigger;
    private Label label7;
    private TextBox txtTrigger;
    private Panel pnlButtons;
    private FlowLayoutPanel pnlFieldButtons;
    private FlowLayoutPanel pnlDataCompletionFieldButtons;
    private StandardIconButton btnRemoveField;
    private StandardIconButton btnAddField;
    private StandardIconButton btnRemoveDataCompletionField;
    private StandardIconButton btnAddDataCompletionField;
    private ToolTip toolTip1;
    private StandardIconButton btnTriggerLookup;
    private Label label1;
    private TextBox txtAlertDateFieldID;
    private SizableTextBox txtAdjustDays;
    private ComboBox cboAdjustDayType;
    private Label label10;
    private CheckBox chkAdjustTrigger;
    private ComboBox cboAdjustDir;
    private Panel pnlTriggerAdj;
    private TextBox txtAlertDateFieldDesc;
    private Panel pnlOptions;
    private BorderPanel pnlCustomTriggerGroup;
    private GradientPanel pnlCustomType;
    private RadioButton radActivateBoth;
    private RadioButton radActivateCondition;
    private RadioButton radActivateDate;
    private Label label5;
    private BorderPanel pnlCustomDate;
    private Panel pnlCustomTriggerSpacer;
    private CheckBox chkAllowToClear;
    private AdvancedSearchControl ctlCustomCondition;
    private PanelEx pnlNotification;
    private Button btnRecipient;
    private RadioButton radNotificationDisabled;
    private RadioButton radNotificationEnabled;
    private Label label3;
    private PanelEx pnlAlertDays;
    private Label lblDaysBefore;
    private TextBox txtDaysBefore;
    private Label label4;
    private PanelEx pnlAlert;
    private Button btnMilestone;
    private Label label2;
    private RadioButton radAlertDisabled;
    private RadioButton radAlertEnabled;
    private Panel pnlTILOptions;
    private RadioButton radTILToleranceAll;
    private RadioButton radTILToleranceFixed;
    private Label label8;
    private BorderPanel pnlOptionGroup;
    private Panel pnlCustomClearAlert;
    private Panel pnlTriggerFields;
    private Panel pnlOptionSpacer;
    private RadioButton radTILAPRAll;
    private RadioButton radTILAPRIncrease;
    private Label label6;
    private Panel panel1;
    private Panel panel2;
    private CheckBox chkDeclineConsent;
    private RadioButton rdoFileStartedDate;
    private RadioButton rdoApplicationDate;
    private Label label9;

    public AlertEditor(AlertConfig alert, FieldSettings fieldSettings, Sessions.Session session)
    {
      this.session = session;
      this.currentAlert = !(alert is AlertConfigWithDataCompletionFields) ? (AlertConfig) alert.Clone() : (AlertConfig) ((AlertConfigWithDataCompletionFields) alert).Clone();
      this.fieldSettings = fieldSettings;
      this.InitializeComponent();
      this.init();
      if (this.currentAlert.AlertID == 51)
      {
        this.panel2.Visible = true;
        IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Alert");
        bool flag = (bool) serverSettings[(object) "Alert.USEAPPLICATIONDATE"];
        this.rdoApplicationDate.Checked = flag;
        this.rdoFileStartedDate.Checked = !flag;
        this.chkDeclineConsent.Checked = (bool) serverSettings[(object) "Alert.DONOTSHOWDECLINECONSENT"];
      }
      else
        this.panel2.Visible = false;
      if (this.currentAlert.AlertID < 0)
        this.radAlertEnabled.Checked = true;
      TextBoxFormatter.Attach(this.txtDaysBefore, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.txtAdjustDays.TextBox, TextBoxContentRule.NonNegativeInteger);
    }

    public AlertConfig GetAlert() => this.currentAlert;

    private void init()
    {
      this.txtName.Text = this.currentAlert.Definition.Name;
      this.txtName.ReadOnly = this.currentAlert.Definition.Category != AlertCategory.Custom;
      this.txtMessage.Text = this.currentAlert.Message;
      this.pnlMessage.Visible = this.currentAlert.Definition.SupportsCustomMessage;
      if (this.currentAlert.Definition.Category == AlertCategory.Workflow)
        this.txtTrigger.Text = (this.currentAlert.Definition as WorkflowAlert).TriggerDescription;
      else
        this.pnlTrigger.Visible = false;
      if (this.currentAlert.Definition.Category == AlertCategory.Regulation)
      {
        this.loadTriggerFields(this.currentAlert);
        if (this.currentAlert is AlertConfigWithDataCompletionFields)
          this.loadDataCompletionFields(this.currentAlert as AlertConfigWithDataCompletionFields);
      }
      else
        this.pnlTriggerFields.Visible = false;
      if (this.currentAlert.Definition.Category == AlertCategory.Custom)
      {
        this.loadCustomTriggerData(this.currentAlert);
      }
      else
      {
        this.pnlCustomClearAlert.Visible = false;
        this.pnlCustomTriggerGroup.Visible = false;
      }
      this.radAlertEnabled.Checked = this.currentAlert.AlertEnabled;
      this.radAlertDisabled.Checked = !this.currentAlert.AlertEnabled;
      if (this.currentAlert.AlertID == 21)
        this.loadTILRedisclosureOptions();
      if (this.currentAlert.AlertID == 49)
        this.loadREGZCDRedisclosureOptions();
      if (this.currentAlert.AlertID != 21 && this.currentAlert.AlertID != 49)
        this.pnlTILOptions.Visible = false;
      if (this.currentAlert.AlertTiming == AlertTiming.DaysBefore)
        this.txtDaysBefore.Text = this.currentAlert.DaysBefore.ToString();
      if (this.currentAlert.Definition.Category != AlertCategory.Custom)
        this.pnlAlertDays.Visible = this.currentAlert.AlertTiming == AlertTiming.DaysBefore;
      if (this.currentAlert.Definition is StandardAlert)
        this.lblDaysBefore.Text = this.getDaysBeforeText((StandardAlertID) this.currentAlert.AlertID);
      if (this.currentAlert.Definition.NotificationType == AlertNotificationType.None)
      {
        this.pnlNotification.Visible = false;
      }
      else
      {
        this.radNotificationEnabled.Checked = this.currentAlert.NotificationEnabled;
        this.radNotificationDisabled.Checked = !this.currentAlert.NotificationEnabled;
        this.btnRecipient.Visible = this.currentAlert.Definition.NotificationType == AlertNotificationType.Configurable;
      }
      this.resizeForm();
      this.btnSave.Enabled = false;
      this.initFlag = true;
    }

    private void loadTILRedisclosureOptions()
    {
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Compliance");
      this.radTILToleranceAll.Checked = (bool) serverSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs"];
      if (!this.radTILToleranceAll.Checked)
        this.radTILToleranceFixed.Checked = true;
      this.radTILAPRIncrease.Checked = (bool) serverSettings[(object) "Compliance.SuppressNegativeAPRAlert"];
      if (this.radTILAPRIncrease.Checked)
        return;
      this.radTILAPRAll.Checked = true;
    }

    private void saveTILRedisclosureOptions()
    {
      this.session.ServerManager.UpdateServerSetting("Compliance.ApplyFixedAPRToleranceToARMs", (object) this.radTILToleranceAll.Checked);
      this.session.ServerManager.UpdateServerSetting("Compliance.SuppressNegativeAPRAlert", (object) this.radTILAPRIncrease.Checked);
    }

    private void saveESignConsentOptions()
    {
      this.session.ServerManager.UpdateServerSetting("Alert.DONOTSHOWDECLINECONSENT", (object) this.chkDeclineConsent.Checked);
      this.session.ServerManager.UpdateServerSetting("Alert.USEAPPLICATIONDATE", (object) this.rdoApplicationDate.Checked);
    }

    private void loadREGZCDRedisclosureOptions()
    {
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Compliance");
      this.radTILToleranceAll.Checked = (bool) serverSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs2015"];
      if (!this.radTILToleranceAll.Checked)
        this.radTILToleranceFixed.Checked = true;
      this.radTILAPRIncrease.Checked = (bool) serverSettings[(object) "Compliance.SuppressNegativeAPRAlert2015"];
      if (this.radTILAPRIncrease.Checked)
        return;
      this.radTILAPRAll.Checked = true;
    }

    private void saveREGZCDRedisclosureOptions()
    {
      this.session.ServerManager.UpdateServerSetting("Compliance.ApplyFixedAPRToleranceToARMs2015", (object) this.radTILToleranceAll.Checked);
      this.session.ServerManager.UpdateServerSetting("Compliance.SuppressNegativeAPRAlert2015", (object) this.radTILAPRIncrease.Checked);
    }

    private void resizeForm()
    {
      this.PerformLayout();
      this.ClientSize = new Size(this.ClientSize.Width, this.pnlButtons.Bottom + 1);
    }

    private string getDaysBeforeText(StandardAlertID configId)
    {
      switch (configId)
      {
        case StandardAlertID.ConversationFollowUp:
          return "days before the follow-up is due";
        case StandardAlertID.DocumentExpected:
          return "days before the document is expected";
        case StandardAlertID.EscrowDisbursementDue:
          return "days before the disbursement is due";
        case StandardAlertID.TaskFollowUp:
          return "days before the follow-up is due";
        case StandardAlertID.MilestoneExpected:
          return "days before the milestone is expected";
        case StandardAlertID.PaymentPastDue:
          return "days before the late payment date";
        case StandardAlertID.PrintMailPaymentStatement:
          return "days before the printing/mailing is due";
        case StandardAlertID.RateLockExpired:
          return "days before the lock expires";
        case StandardAlertID.ShippingExpected:
          return "days before the shipping is due";
        case StandardAlertID.RegistrationExpired:
          return "days before the registration expires";
        case StandardAlertID.PostClosingConditionExpected:
        case StandardAlertID.PreliminaryConditionExpected:
        case StandardAlertID.UnderwritingConditionExpected:
          return "days before the condition is expected";
        case StandardAlertID.DocumentExpired:
          return "days before the document expires";
        case StandardAlertID.TaskDue:
          return "days before the task is expected";
        case StandardAlertID.InitialDisclosures:
          return "days before the Initial Disclosure Due Date";
        case StandardAlertID.GFEExpires:
          return "days before the expiration date";
        case StandardAlertID.RediscloseGFEChangedCircumstances:
          return "days before the Redisclosure Due Date";
        case StandardAlertID.LoanEstimatesExpires:
          return "days before the expiration date";
        case StandardAlertID.RediscloseLEChangedCircumstances:
          return "days before the Redisclosure Due Date";
        case StandardAlertID.RediscloseCDChangedCircumstances:
          return "days before the Redisclosure Due Date";
        default:
          return "days prior to alert date";
      }
    }

    private void loadCustomTriggerData(AlertConfig alertConfig)
    {
      CustomAlert definition = alertConfig.Definition as CustomAlert;
      this.chkAdjustTrigger.Checked = false;
      this.cboAdjustDayType.SelectedIndex = 0;
      this.cboAdjustDir.SelectedIndex = 0;
      this.lblDaysBefore.Text = "days before trigger event";
      this.chkAllowToClear.Checked = definition.AllowToClear;
      if (alertConfig.TriggerFieldList.Count > 0 && (definition.ConditionXml ?? "") != "")
        this.radActivateBoth.Checked = true;
      else if (alertConfig.TriggerFieldList.Count > 0)
        this.radActivateDate.Checked = true;
      else if ((definition.ConditionXml ?? "") != "")
        this.radActivateCondition.Checked = true;
      else
        this.radActivateDate.Checked = true;
      if (alertConfig.TriggerFieldList.Count > 0)
        this.triggerFieldDef = EncompassFields.GetField(alertConfig.TriggerFieldList[0], this.fieldSettings);
      this.setTriggerFieldStatus();
      if (this.triggerFieldDef != null)
      {
        this.chkAdjustTrigger.Checked = definition.DateAdjustment != 0;
        this.txtAdjustDays.Text = Math.Abs(definition.DateAdjustment).ToString();
        ClientCommonUtils.PopulateDropdown(this.cboAdjustDayType, (object) definition.AdjustmentDayType.ToString(), false);
        this.cboAdjustDir.SelectedIndex = definition.DateAdjustment >= 0 ? 0 : 1;
      }
      this.ctlCustomCondition.FieldDefs = (ReportFieldDefs) LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.BasicLoanDataFields);
      this.ctlCustomCondition.AllowDatabaseFieldsOnly = false;
      this.ctlCustomCondition.AllowVirtualFields = true;
      this.ctlCustomCondition.AllowDynamicOperators = false;
      if (!((definition.ConditionXml ?? "") != ""))
        return;
      this.ctlCustomCondition.SetCurrentFilter(FieldFilterList.Parse(definition.ConditionXml));
    }

    private void loadTriggerFields(AlertConfig alertConfig)
    {
      this.populateTriggerFields(alertConfig);
    }

    private void populateTriggerFields(AlertConfig alertConfig)
    {
      RegulationAlert definition = alertConfig.Definition as RegulationAlert;
      this.setToleranceColumns(AlertEditor.canShowToleranceColumn(definition.TriggerFields), this.gvFields);
      this.populateTriggerFieldsGridView(definition.TriggerFields);
      this.populateCustomTriggerFieldsGridView(alertConfig.TriggerFieldList, definition.AllowUserDefinedTriggerFields);
      this.refreshFieldGroupTitle();
    }

    private void populateTriggerFieldsGridView(AlertTriggerFieldCollection triggerFields)
    {
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) triggerFields)
        this.gvFields.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = triggerField.FieldID
            },
            [1] = {
              Text = EncompassFields.GetDescription(triggerField.FieldID, this.fieldSettings)
            },
            [2] = {
              Text = triggerField.Tolerance ?? ""
            }
          },
          Tag = (object) triggerField
        });
    }

    private void populateCustomTriggerFieldsGridView(
      List<string> fieldList,
      bool allowUserDefinedTriggerFields)
    {
      this.pnlFieldButtons.Visible = allowUserDefinedTriggerFields;
      if (!allowUserDefinedTriggerFields)
        return;
      foreach (string field in fieldList)
        this.gvFields.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = field
            },
            [1] = {
              Text = EncompassFields.GetDescription(field, this.fieldSettings)
            }
          },
          Tag = (object) field
        });
    }

    private static bool canShowToleranceColumn(AlertTriggerFieldCollection triggerFields)
    {
      bool flag = false;
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) triggerFields)
      {
        if ((triggerField.Tolerance ?? "") != "")
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void setToleranceColumns(bool showTolerance, GridView gv)
    {
      if (showTolerance)
        return;
      GVColumn columnByName = gv.Columns.GetColumnByName("colTolerance");
      gv.Columns.GetColumnByName("colDescription").Width += columnByName.Width;
      gv.Columns.Remove(columnByName);
    }

    private bool validateAlert()
    {
      if (this.currentAlert.Definition.Category == AlertCategory.Custom)
      {
        if (this.txtName.Text.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must provide a Name for this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (!this.radActivateCondition.Checked)
        {
          if (this.triggerFieldDef == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a date field to be used at the date of the Alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (this.chkAdjustTrigger.Checked && !Utils.IsInt((object) this.txtAdjustDays.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The number of days specified for the date adjustment is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
        }
        if (!this.radActivateDate.Checked && this.ctlCustomCondition.FilterCount == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "At least one condition must be specified for this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (string.Compare(this.currentAlert.Definition.Name, this.txtName.Text, true) != 0 && this.session.AlertManager.GetAlertConfigByName(this.txtName.Text) != null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "An alert with the name '" + this.txtName.Text + "' already exists.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      if (this.currentAlert.Definition.SupportsCustomMessage && this.txtMessage.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must provide a Message to be displayed for this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!this.pnlAlertDays.Visible || Utils.IsInt((object) this.txtDaysBefore.Text.Trim()))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter the number of days prior to the trigger date that the alert should appear.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void saveAlert()
    {
      if (this.currentAlert.Definition.Category == AlertCategory.Custom)
        this.saveCustomAlertSettings();
      if (this.currentAlert.Definition.SupportsCustomMessage)
        this.currentAlert.Message = this.txtMessage.Text.Trim();
      this.currentAlert.AlertEnabled = this.radAlertEnabled.Checked;
      if (this.currentAlert.AlertTiming == AlertTiming.DaysBefore)
        this.currentAlert.DaysBefore = Utils.ParseInt((object) this.txtDaysBefore.Text.Trim());
      if (this.currentAlert.Definition.NotificationType != AlertNotificationType.None)
        this.currentAlert.NotificationEnabled = this.radNotificationEnabled.Checked;
      if ((this.currentAlert.Definition is RegulationAlert definition ? (definition.AllowUserDefinedTriggerFields ? 1 : 0) : 0) != 0)
      {
        this.currentAlert.TriggerFieldList.Clear();
        this.currentAlert.TriggerFieldList.AddRange((IEnumerable<string>) this.getCustomTriggerFieldList());
      }
      if (this.currentAlert.AlertID == 0)
        this.adjustMilestonesForMilestoneFinishedAlert();
      if (this.currentAlert.AlertID == 51)
        this.saveESignConsentOptions();
      this.currentAlert = this.session.AlertManager.GetAlertConfig(this.session.AlertManager.UpdateAlertConfig(this.currentAlert));
      if (this.currentAlert.AlertID == 21)
        this.saveTILRedisclosureOptions();
      if (this.currentAlert.AlertID == 49)
        this.saveREGZCDRedisclosureOptions();
      if (this.alertMsgsToUpdate == null)
        return;
      ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).SetOrUpdateMsWorksheetAlertMessages(this.alertMsgsToUpdate);
    }

    private void adjustMilestonesForMilestoneFinishedAlert()
    {
      foreach (EllieMae.EMLite.Workflow.Milestone activeMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList())
      {
        if (activeMilestones.RoleID <= 0)
          this.currentAlert.MilestoneGuidList.Remove(activeMilestones.MilestoneID);
      }
    }

    private void saveCustomAlertSettings()
    {
      CustomAlert definition = this.currentAlert.Definition as CustomAlert;
      definition.Name = this.txtName.Text.Trim();
      this.currentAlert.TriggerFieldList.Clear();
      definition.AllowToClear = this.chkAllowToClear.Checked;
      if (!this.radActivateCondition.Checked)
      {
        this.currentAlert.TriggerFieldList.Add(this.triggerFieldDef.FieldID);
        if (this.chkAdjustTrigger.Checked)
        {
          definition.DateAdjustment = int.Parse(this.txtAdjustDays.Text) * (this.cboAdjustDir.SelectedIndex == 0 ? 1 : -1);
          definition.AdjustmentDayType = (DayType) Enum.Parse(typeof (DayType), this.cboAdjustDayType.Text, true);
        }
        else
        {
          definition.DateAdjustment = 0;
          definition.AdjustmentDayType = DayType.Calendar;
        }
      }
      definition.ConditionXml = (string) null;
      if (this.radActivateDate.Checked)
        return;
      FieldFilterList currentFilter = this.ctlCustomCondition.GetCurrentFilter();
      if (currentFilter.Count == 0)
        return;
      definition.ConditionXml = currentFilter.ToXML();
    }

    private string[] getCustomTriggerFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
      {
        if (gvItem.Tag is string)
          stringList.Add(gvItem.Tag.ToString());
      }
      return stringList.ToArray();
    }

    private void alertOption_CheckedChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnMilestone.Enabled = this.radAlertEnabled.Checked;
      this.txtDaysBefore.Enabled = this.radAlertEnabled.Checked;
      this.pnlNotification.Enabled = this.radAlertEnabled.Checked;
      this.currentAlert.AlertEnabled = this.radAlertEnabled.Checked;
      if (this.radAlertEnabled.Checked)
        this.populateMilestoneSettings();
      else
        this.radNotificationDisabled.Checked = true;
    }

    private void populateMilestoneSettings()
    {
      if (this.currentAlert.MilestoneGuidList.Count != 0)
        return;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in this.session.StartupInfo.Milestones)
        this.currentAlert.MilestoneGuidList.Add(milestone.MilestoneID);
    }

    private void btnMilestone_Click(object sender, EventArgs e)
    {
      using (MilestoneSelectionDialog milestoneSelectionDialog = new MilestoneSelectionDialog(this.session, this.currentAlert))
      {
        if (milestoneSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || !milestoneSelectionDialog.IsDirty)
          return;
        this.btnSave.Enabled = true;
        this.currentAlert.MilestoneGuidList.Clear();
        this.currentAlert.MilestoneGuidList.AddRange((IEnumerable<string>) milestoneSelectionDialog.GetSelectedMilestones());
        this.alertMsgsToUpdate = milestoneSelectionDialog.AlertMsgsToUpdate;
        if (this.currentAlert.MilestoneGuidList.Count != 0)
          return;
        this.radAlertDisabled.Checked = true;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateAlert())
        return;
      this.saveAlert();
      this.DialogResult = DialogResult.OK;
    }

    private void radNotificationEnabled_CheckedChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
      this.btnRecipient.Enabled = this.radNotificationEnabled.Checked;
    }

    private void btnRecipient_Click(object sender, EventArgs e)
    {
      using (RecipientSelectionDialog recipientSelectionDialog = new RecipientSelectionDialog(this.currentAlert))
      {
        if (recipientSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.btnSave.Enabled = true;
      }
    }

    private void txtDaysBefore_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        if (selectedItem.Tag is AlertTriggerField)
        {
          flag = true;
          break;
        }
      }
      this.btnRemoveField.Enabled = this.gvFields.SelectedItems.Count > 0 && !flag;
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.btnSave.Enabled = true;

    private void btnAddField_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, "Add Trigger Fields", AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowHiddenFields))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.onMoreFieldsAdded);
        DialogResult dialogResult = addFields.ShowDialog((IWin32Window) this);
        addFields.OnAddMoreButtonClick -= new EventHandler(this.onMoreFieldsAdded);
        if (dialogResult != DialogResult.OK)
          return;
        this.addCustomTriggerFieldsGridView(addFields.SelectedFieldIDs);
      }
    }

    private void onMoreFieldsAdded(object sender, EventArgs e)
    {
      this.addCustomTriggerFieldsGridView(((AddFields) sender).SelectedFieldIDs);
    }

    private void addCustomTriggerFieldsGridView(string[] fieldIds)
    {
      if (this.currentAlert.AlertID == 43)
        fieldIds = this.getAlert43CustomTriggerFields(fieldIds);
      foreach (string fieldId in fieldIds)
      {
        if (!this.existsTriggerFieldGridView(fieldId))
        {
          this.gvFields.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Text = fieldId
              },
              [1] = {
                Text = EncompassFields.GetDescription(fieldId, this.fieldSettings)
              }
            },
            Tag = (object) fieldId
          });
          this.btnSave.Enabled = true;
        }
      }
      this.refreshFieldGroupTitle();
    }

    private string[] getAlert43CustomTriggerFields(string[] fieldIds)
    {
      List<string> stringList = new List<string>();
      foreach (string fieldId in fieldIds)
      {
        if (this.isSnapshotField(fieldId))
          stringList.Add(fieldId);
      }
      if (stringList.Count < fieldIds.Length)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Key Pricing Fields must be on the lock request form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return stringList.ToArray();
    }

    private void refreshFieldGroupTitle()
    {
      this.grpFields.Text = "Field Triggers (" + (object) this.gvFields.Items.Count + ")";
    }

    private bool existsTriggerFieldGridView(string fieldId)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
      {
        bool flag = false;
        if (gvItem.Tag is string)
          flag = string.Compare(gvItem.Tag.ToString(), fieldId, true) == 0;
        else if ((object) (gvItem.Tag as AlertTriggerField) != null)
          flag = string.Compare(((AlertTriggerField) gvItem.Tag).FieldID, fieldId, true) == 0;
        if (flag)
          return true;
      }
      return false;
    }

    private bool isSnapshotField(string fieldID) => LockRequestLog.SnapshotFields.Contains(fieldID);

    private void btnRemoveField_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the selected fields?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        if (selectedItem.Tag is string)
          this.gvFields.Items.Remove(selectedItem);
      }
      this.btnSave.Enabled = true;
      this.refreshFieldGroupTitle();
    }

    private void chkAdjustTrigger_CheckedChanged(object sender, EventArgs e)
    {
      this.txtAdjustDays.Enabled = this.chkAdjustTrigger.Checked;
      this.cboAdjustDayType.Enabled = this.chkAdjustTrigger.Checked;
      this.cboAdjustDir.Enabled = this.chkAdjustTrigger.Checked;
      this.btnSave.Enabled = true;
    }

    private void btnTriggerLookup_Click(object sender, EventArgs e)
    {
      bool flag = true;
      while (flag)
      {
        using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) null, true, string.Empty, true, true))
        {
          if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
            break;
          FieldDefinition field = EncompassFields.GetField(ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1], this.fieldSettings, true);
          if (!field.IsDateValued())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The selected field is not valid in this context. You must select a date field for this alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            this.triggerFieldDef = field;
            this.setTriggerFieldStatus();
            this.btnSave.Enabled = true;
            break;
          }
        }
      }
    }

    private void btnTriggerClear_Click(object sender, EventArgs e)
    {
      this.triggerFieldDef = (FieldDefinition) null;
      this.setTriggerFieldStatus();
    }

    private void setTriggerFieldStatus()
    {
      if (this.triggerFieldDef == null)
      {
        this.txtAlertDateFieldID.Text = "";
        this.txtAlertDateFieldDesc.Text = "";
      }
      else
      {
        this.txtAlertDateFieldID.Text = this.triggerFieldDef.FieldID;
        this.txtAlertDateFieldDesc.Text = this.triggerFieldDef.Description;
      }
    }

    private void txtAlertDateFieldID_Validating(object sender, CancelEventArgs e)
    {
      string str = this.txtAlertDateFieldID.Text.Trim();
      if (str == "")
      {
        this.triggerFieldDef = (FieldDefinition) null;
        this.setTriggerFieldStatus();
      }
      else
      {
        if (this.triggerFieldDef != null && string.Compare(this.triggerFieldDef.FieldID, str, true) == 0)
          return;
        FieldDefinition field = EncompassFields.GetField(str, this.fieldSettings, true);
        if (field == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + str + "' is not a valid field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.Cancel = true;
        }
        else if (!field.IsDateValued())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The field '" + str + "' (" + field.Description + ") is not a date. You must select a date field for the alert date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.Cancel = true;
        }
        else
        {
          this.triggerFieldDef = field;
          this.setTriggerFieldStatus();
          this.onFieldValueChanged(sender, (EventArgs) e);
        }
      }
    }

    private void onCustomAlertActivationChanged(object sender, EventArgs e)
    {
      bool flag1 = this.radActivateDate.Checked || this.radActivateBoth.Checked;
      bool flag2 = this.radActivateCondition.Checked || this.radActivateBoth.Checked;
      this.pnlCustomDate.Visible = flag1;
      this.ctlCustomCondition.Visible = flag2;
      this.pnlAlertDays.Visible = flag1;
      this.chkAllowToClear.Enabled = flag2;
      if (!flag2)
        this.chkAllowToClear.Checked = true;
      this.btnSave.Enabled = true;
      this.resizeForm();
    }

    private void AlertEditor_Shown(object sender, EventArgs e) => this.resizeForm();

    private void ctlCustomCondition_DataChange(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void onAPRToleranceChanged(object sender, EventArgs e)
    {
      if (this.radTILToleranceAll.Checked)
      {
        this.gvFields.Items[0].SubItems[2].Text = "0.125%";
        if (this.txtMessage.Text == "REG-Z TIL must be re-disclosed when the Current APR differs from the Disclosed APR by more than 0.125%. (0.25% for ARM loans)")
          this.txtMessage.Text = "REG-Z TIL must be re-disclosed when the Current APR differs from the Disclosed APR by more than 0.125%.";
      }
      else
      {
        this.gvFields.Items[0].SubItems[2].Text = "0.125/0.25%";
        if (this.txtMessage.Text == "REG-Z TIL must be re-disclosed when the Current APR differs from the Disclosed APR by more than 0.125%.")
          this.txtMessage.Text = "REG-Z TIL must be re-disclosed when the Current APR differs from the Disclosed APR by more than 0.125%. (0.25% for ARM loans)";
      }
      this.onFieldValueChanged(sender, e);
    }

    private void rdoApplicationDate_CheckedChanged(object sender, EventArgs e)
    {
      if (this.currentAlert.Definition is RegulationAlert definition && definition.TriggerFields != null)
      {
        AlertTriggerField alertTriggerField1 = definition.TriggerFields.Find((Predicate<AlertTriggerField>) (a => a.FieldID == "3142"));
        AlertTriggerField alertTriggerField2 = definition.TriggerFields.Find((Predicate<AlertTriggerField>) (a => a.FieldID == "MS.START"));
        if (this.rdoApplicationDate.Checked)
        {
          if (alertTriggerField1 == null && alertTriggerField2 != null)
          {
            AlertTriggerField alertTriggerField3 = new AlertTriggerField("3142");
            definition.TriggerFields.Remove(alertTriggerField2);
            definition.TriggerFields.Insert(0, alertTriggerField3);
          }
        }
        else if (alertTriggerField1 != null && alertTriggerField2 == null)
        {
          AlertTriggerField alertTriggerField4 = new AlertTriggerField("MS.START");
          definition.TriggerFields.Remove(alertTriggerField1);
          definition.TriggerFields.Insert(0, alertTriggerField4);
        }
        if (this.initFlag)
        {
          this.gvFields.Items.Clear();
          foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) definition.TriggerFields)
            this.gvFields.Items.Add(new GVItem()
            {
              SubItems = {
                [0] = {
                  Text = triggerField.FieldID
                },
                [1] = {
                  Text = EncompassFields.GetDescription(triggerField.FieldID, this.fieldSettings)
                },
                [2] = {
                  Text = triggerField.Tolerance ?? ""
                }
              },
              Tag = (object) triggerField
            });
          this.pnlFieldButtons.Visible = definition.AllowUserDefinedTriggerFields;
          if (definition.AllowUserDefinedTriggerFields)
          {
            foreach (string triggerField in this.currentAlert.TriggerFieldList)
              this.gvFields.Items.Add(new GVItem()
              {
                SubItems = {
                  [0] = {
                    Text = triggerField
                  },
                  [1] = {
                    Text = EncompassFields.GetDescription(triggerField, this.fieldSettings)
                  }
                },
                Tag = (object) triggerField
              });
          }
        }
      }
      this.btnSave.Enabled = true;
    }

    private void chkDeclineConsent_CheckedChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private AlertConfigWithDataCompletionFields _alertConfigWithDataCompletionFields
    {
      get => this.currentAlert as AlertConfigWithDataCompletionFields;
    }

    private void loadDataCompletionFields(AlertConfigWithDataCompletionFields alertConfig)
    {
      bool flag = alertConfig.Definition is RegulationAlertWithDataCompletionFields definition && definition.AllowCustomDataCompletionFields;
      this.setToleranceColumns(false, this.gvDataCompletionFields);
      this.populateDataCompletionFieldsGridView(alertConfig.StandardDataCompletionActiveFields);
      this.pnlDataCompletionFieldButtons.Visible = flag;
      if (flag)
        this.populateDataCompletionFieldsGridView(alertConfig.CustomDataCompletionFields);
      this.refreshDataCompletionFieldGroupTitle();
      this.grpDataCompletionFields.Visible = true;
    }

    private void addDataCompletionFields(string[] fieldIds)
    {
      if (fieldIds == null || fieldIds.Length == 0)
        return;
      foreach (string fieldId in fieldIds)
      {
        if (!this.existsTriggerFieldGridView(fieldId))
        {
          AlertDataCompletionField field = (AlertDataCompletionField) null;
          AlertConfigWithDataCompletionFields completionFields = this._alertConfigWithDataCompletionFields;
          if ((completionFields != null ? (completionFields.AddField(fieldId, out field) ? 1 : 0) : 0) != 0)
            this.populateDataCompletionFieldsGridViewItem(field);
        }
      }
    }

    private void removeDataCompletionFields()
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the selected fields?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      foreach (GVItem selectedItem in this.gvDataCompletionFields.SelectedItems)
      {
        if (selectedItem.Tag is AlertDataCompletionField)
        {
          AlertDataCompletionField tag = selectedItem.Tag as AlertDataCompletionField;
          AlertConfigWithDataCompletionFields completionFields = this._alertConfigWithDataCompletionFields;
          if ((completionFields != null ? (completionFields.RemoveField(tag) ? 1 : 0) : 0) != 0)
            this.gvDataCompletionFields.Items.Remove(selectedItem);
        }
      }
    }

    private void populateDataCompletionFieldsGridView(AlertDataCompletionFieldCollection fields)
    {
      foreach (AlertDataCompletionField field in (List<AlertDataCompletionField>) fields)
        this.populateDataCompletionFieldsGridViewItem(field);
    }

    private void populateDataCompletionFieldsGridViewItem(AlertDataCompletionField field)
    {
      this.gvDataCompletionFields.Items.Add(new GVItem()
      {
        SubItems = {
          [0] = {
            Text = field.FieldID
          },
          [1] = {
            Text = EncompassFields.GetDescription(field.FieldID, this.fieldSettings)
          }
        },
        Tag = (object) field
      });
    }

    private void refreshDataCompletionFieldGroupTitle()
    {
      this.grpDataCompletionFields.Text = "Fields Needed for Completion (" + (object) this.gvDataCompletionFields.Items.Count + ")";
    }

    private void onMoreCustomDataCompletionFieldsAdded(object sender, EventArgs e)
    {
      this.addDataCompletionFields(((AddFields) sender).SelectedFieldIDs);
      this.refreshDataCompletionFieldGroupTitle();
      this.btnSave.Enabled = true;
    }

    private void btnAddDataCompletionField_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, "Add Fields Needed for Completion", AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowHiddenFields))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.onMoreCustomDataCompletionFieldsAdded);
        DialogResult dialogResult = addFields.ShowDialog((IWin32Window) this);
        addFields.OnAddMoreButtonClick -= new EventHandler(this.onMoreCustomDataCompletionFieldsAdded);
        if (dialogResult != DialogResult.OK)
          return;
        this.addDataCompletionFields(addFields.SelectedFieldIDs);
        this.refreshDataCompletionFieldGroupTitle();
        this.btnSave.Enabled = true;
      }
    }

    private void btnRemoveDataCompletionField_Click(object sender, EventArgs e)
    {
      this.removeDataCompletionFields();
      this.refreshDataCompletionFieldGroupTitle();
      this.btnSave.Enabled = true;
    }

    private void gvDataCompletionFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (GVItem selectedItem in this.gvDataCompletionFields.SelectedItems)
      {
        if ((selectedItem.Tag is AlertDataCompletionField tag ? (tag.ReadOnly ? 1 : 0) : 0) != 0)
        {
          flag = true;
          break;
        }
      }
      this.btnRemoveDataCompletionField.Enabled = !flag && this.gvDataCompletionFields.SelectedItems.Count > 0;
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
      this.btnReset = new Button();
      this.btnSave = new Button();
      this.lblAlertName = new Label();
      this.txtName = new TextBox();
      this.txtMessage = new TextBox();
      this.lblMessage = new Label();
      this.grpFields = new GroupContainer();
      this.pnlFieldButtons = new FlowLayoutPanel();
      this.btnRemoveField = new StandardIconButton();
      this.btnAddField = new StandardIconButton();
      this.gvFields = new GridView();
      this.grpDataCompletionFields = new GroupContainer();
      this.pnlDataCompletionFieldButtons = new FlowLayoutPanel();
      this.btnRemoveDataCompletionField = new StandardIconButton();
      this.btnAddDataCompletionField = new StandardIconButton();
      this.gvDataCompletionFields = new GridView();
      this.pnlName = new Panel();
      this.pnlMessage = new Panel();
      this.txtAlertDateFieldDesc = new TextBox();
      this.pnlTriggerAdj = new Panel();
      this.txtAdjustDays = new SizableTextBox();
      this.cboAdjustDayType = new ComboBox();
      this.cboAdjustDir = new ComboBox();
      this.label10 = new Label();
      this.chkAdjustTrigger = new CheckBox();
      this.btnTriggerLookup = new StandardIconButton();
      this.label1 = new Label();
      this.txtAlertDateFieldID = new TextBox();
      this.pnlTrigger = new Panel();
      this.label7 = new Label();
      this.txtTrigger = new TextBox();
      this.pnlButtons = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlOptions = new Panel();
      this.pnlOptionGroup = new BorderPanel();
      this.pnlOptionSpacer = new Panel();
      this.pnlAlertDays = new PanelEx();
      this.lblDaysBefore = new Label();
      this.txtDaysBefore = new TextBox();
      this.label4 = new Label();
      this.pnlNotification = new PanelEx();
      this.btnRecipient = new Button();
      this.radNotificationDisabled = new RadioButton();
      this.radNotificationEnabled = new RadioButton();
      this.label3 = new Label();
      this.pnlAlert = new PanelEx();
      this.btnMilestone = new Button();
      this.label2 = new Label();
      this.radAlertDisabled = new RadioButton();
      this.radAlertEnabled = new RadioButton();
      this.pnlTILOptions = new Panel();
      this.panel1 = new Panel();
      this.radTILToleranceAll = new RadioButton();
      this.radTILToleranceFixed = new RadioButton();
      this.radTILAPRAll = new RadioButton();
      this.radTILAPRIncrease = new RadioButton();
      this.label6 = new Label();
      this.label8 = new Label();
      this.pnlCustomClearAlert = new Panel();
      this.chkAllowToClear = new CheckBox();
      this.pnlTriggerFields = new Panel();
      this.pnlCustomTriggerGroup = new BorderPanel();
      this.pnlCustomTriggerSpacer = new Panel();
      this.ctlCustomCondition = new AdvancedSearchControl();
      this.pnlCustomDate = new BorderPanel();
      this.pnlCustomType = new GradientPanel();
      this.radActivateBoth = new RadioButton();
      this.radActivateCondition = new RadioButton();
      this.radActivateDate = new RadioButton();
      this.label5 = new Label();
      this.panel2 = new Panel();
      this.chkDeclineConsent = new CheckBox();
      this.rdoFileStartedDate = new RadioButton();
      this.rdoApplicationDate = new RadioButton();
      this.label9 = new Label();
      this.grpFields.SuspendLayout();
      this.pnlFieldButtons.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveField).BeginInit();
      ((ISupportInitialize) this.btnAddField).BeginInit();
      this.grpDataCompletionFields.SuspendLayout();
      this.pnlDataCompletionFieldButtons.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDataCompletionField).BeginInit();
      ((ISupportInitialize) this.btnAddDataCompletionField).BeginInit();
      this.pnlName.SuspendLayout();
      this.pnlMessage.SuspendLayout();
      this.pnlTriggerAdj.SuspendLayout();
      ((ISupportInitialize) this.btnTriggerLookup).BeginInit();
      this.pnlTrigger.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.pnlOptions.SuspendLayout();
      this.pnlOptionGroup.SuspendLayout();
      this.pnlAlertDays.SuspendLayout();
      this.pnlNotification.SuspendLayout();
      this.pnlAlert.SuspendLayout();
      this.pnlTILOptions.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlCustomClearAlert.SuspendLayout();
      this.pnlTriggerFields.SuspendLayout();
      this.pnlCustomTriggerGroup.SuspendLayout();
      this.pnlCustomDate.SuspendLayout();
      this.pnlCustomType.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnReset.DialogResult = DialogResult.Cancel;
      this.btnReset.Location = new Point(460, 13);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(75, 22);
      this.btnReset.TabIndex = 7;
      this.btnReset.Text = "Cancel";
      this.btnReset.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(377, 13);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.lblAlertName.AutoSize = true;
      this.lblAlertName.Location = new Point(9, 10);
      this.lblAlertName.Name = "lblAlertName";
      this.lblAlertName.Size = new Size(60, 14);
      this.lblAlertName.TabIndex = 10;
      this.lblAlertName.Text = "Alert Name";
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(10, 25);
      this.txtName.MaxLength = 50;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(525, 20);
      this.txtName.TabIndex = 11;
      this.txtName.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMessage.Location = new Point(10, 22);
      this.txtMessage.MaxLength = 500;
      this.txtMessage.Multiline = true;
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.ScrollBars = ScrollBars.Vertical;
      this.txtMessage.Size = new Size(525, 52);
      this.txtMessage.TabIndex = 13;
      this.txtMessage.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.lblMessage.AutoSize = true;
      this.lblMessage.BackColor = Color.Transparent;
      this.lblMessage.Location = new Point(9, 7);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(51, 14);
      this.lblMessage.TabIndex = 12;
      this.lblMessage.Text = "Message";
      this.grpFields.Controls.Add((Control) this.pnlFieldButtons);
      this.grpFields.Controls.Add((Control) this.gvFields);
      this.grpFields.Dock = DockStyle.Top;
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(0, 0);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(525, 152);
      this.grpFields.TabIndex = 14;
      this.grpFields.Text = "Trigger Fields";
      this.pnlFieldButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlFieldButtons.BackColor = Color.Transparent;
      this.pnlFieldButtons.Controls.Add((Control) this.btnRemoveField);
      this.pnlFieldButtons.Controls.Add((Control) this.btnAddField);
      this.pnlFieldButtons.FlowDirection = FlowDirection.RightToLeft;
      this.pnlFieldButtons.Location = new Point(461, 2);
      this.pnlFieldButtons.Name = "pnlFieldButtons";
      this.pnlFieldButtons.Size = new Size(58, 22);
      this.pnlFieldButtons.TabIndex = 1;
      this.btnRemoveField.BackColor = Color.Transparent;
      this.btnRemoveField.Enabled = false;
      this.btnRemoveField.Location = new Point(42, 3);
      this.btnRemoveField.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveField.MouseDownImage = (Image) null;
      this.btnRemoveField.Name = "btnRemoveField";
      this.btnRemoveField.Size = new Size(16, 16);
      this.btnRemoveField.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveField.TabIndex = 0;
      this.btnRemoveField.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveField, "Remove Fields");
      this.btnRemoveField.Click += new EventHandler(this.btnRemoveField_Click);
      this.btnAddField.BackColor = Color.Transparent;
      this.btnAddField.Location = new Point(21, 3);
      this.btnAddField.Margin = new Padding(5, 3, 0, 3);
      this.btnAddField.MouseDownImage = (Image) null;
      this.btnAddField.Name = "btnAddField";
      this.btnAddField.Size = new Size(16, 16);
      this.btnAddField.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddField.TabIndex = 1;
      this.btnAddField.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddField, "Add Fields");
      this.btnAddField.Click += new EventHandler(this.btnAddField_Click);
      this.gvFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colFieldID";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDescription";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colTolerance";
      gvColumn3.Text = "Tolerance/Value";
      gvColumn3.Width = 100;
      this.gvFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFields.Location = new Point(1, 26);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(523, 125);
      this.gvFields.TabIndex = 0;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.grpDataCompletionFields.Controls.Add((Control) this.pnlDataCompletionFieldButtons);
      this.grpDataCompletionFields.Controls.Add((Control) this.gvDataCompletionFields);
      this.grpDataCompletionFields.Dock = DockStyle.Top;
      this.grpDataCompletionFields.HeaderForeColor = SystemColors.ControlText;
      this.grpDataCompletionFields.Location = new Point(0, 152);
      this.grpDataCompletionFields.Name = "grpDataCompletionFields";
      this.grpDataCompletionFields.Size = new Size(525, 148);
      this.grpDataCompletionFields.TabIndex = 15;
      this.grpDataCompletionFields.Text = "Fields Needed for Completion";
      this.grpDataCompletionFields.Visible = false;
      this.pnlDataCompletionFieldButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlDataCompletionFieldButtons.BackColor = Color.Transparent;
      this.pnlDataCompletionFieldButtons.Controls.Add((Control) this.btnRemoveDataCompletionField);
      this.pnlDataCompletionFieldButtons.Controls.Add((Control) this.btnAddDataCompletionField);
      this.pnlDataCompletionFieldButtons.FlowDirection = FlowDirection.RightToLeft;
      this.pnlDataCompletionFieldButtons.Location = new Point(461, 2);
      this.pnlDataCompletionFieldButtons.Name = "pnlDataCompletionFieldButtons";
      this.pnlDataCompletionFieldButtons.Size = new Size(58, 22);
      this.pnlDataCompletionFieldButtons.TabIndex = 1;
      this.btnRemoveDataCompletionField.BackColor = Color.Transparent;
      this.btnRemoveDataCompletionField.Enabled = false;
      this.btnRemoveDataCompletionField.Location = new Point(42, 3);
      this.btnRemoveDataCompletionField.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveDataCompletionField.MouseDownImage = (Image) null;
      this.btnRemoveDataCompletionField.Name = "btnRemoveDataCompletionField";
      this.btnRemoveDataCompletionField.Size = new Size(16, 16);
      this.btnRemoveDataCompletionField.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDataCompletionField.TabIndex = 0;
      this.btnRemoveDataCompletionField.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveDataCompletionField, "Remove Fields");
      this.btnRemoveDataCompletionField.Click += new EventHandler(this.btnRemoveDataCompletionField_Click);
      this.btnAddDataCompletionField.BackColor = Color.Transparent;
      this.btnAddDataCompletionField.Location = new Point(21, 3);
      this.btnAddDataCompletionField.Margin = new Padding(5, 3, 0, 3);
      this.btnAddDataCompletionField.MouseDownImage = (Image) null;
      this.btnAddDataCompletionField.Name = "btnAddDataCompletionField";
      this.btnAddDataCompletionField.Size = new Size(16, 16);
      this.btnAddDataCompletionField.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDataCompletionField.TabIndex = 1;
      this.btnAddDataCompletionField.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddDataCompletionField, "Add Fields");
      this.btnAddDataCompletionField.Click += new EventHandler(this.btnAddDataCompletionField_Click);
      this.gvDataCompletionFields.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colFieldID";
      gvColumn4.Text = "Field ID";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colDescription";
      gvColumn5.Text = "Description";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colTolerance";
      gvColumn6.Text = "Tolerance/Value";
      gvColumn6.Width = 100;
      this.gvDataCompletionFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvDataCompletionFields.Dock = DockStyle.Fill;
      this.gvDataCompletionFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDataCompletionFields.Location = new Point(1, 26);
      this.gvDataCompletionFields.Name = "gvDataCompletionFields";
      this.gvDataCompletionFields.Size = new Size(523, 121);
      this.gvDataCompletionFields.TabIndex = 0;
      this.gvDataCompletionFields.SelectedIndexChanged += new EventHandler(this.gvDataCompletionFields_SelectedIndexChanged);
      this.pnlName.Controls.Add((Control) this.lblAlertName);
      this.pnlName.Controls.Add((Control) this.txtName);
      this.pnlName.Dock = DockStyle.Top;
      this.pnlName.Location = new Point(0, 0);
      this.pnlName.Name = "pnlName";
      this.pnlName.Size = new Size(545, 45);
      this.pnlName.TabIndex = 1;
      this.pnlMessage.Controls.Add((Control) this.lblMessage);
      this.pnlMessage.Controls.Add((Control) this.txtMessage);
      this.pnlMessage.Dock = DockStyle.Top;
      this.pnlMessage.Location = new Point(0, 122);
      this.pnlMessage.Name = "pnlMessage";
      this.pnlMessage.Size = new Size(545, 74);
      this.pnlMessage.TabIndex = 2;
      this.txtAlertDateFieldDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAlertDateFieldDesc.Location = new Point(206, 10);
      this.txtAlertDateFieldDesc.Name = "txtAlertDateFieldDesc";
      this.txtAlertDateFieldDesc.ReadOnly = true;
      this.txtAlertDateFieldDesc.Size = new Size(288, 20);
      this.txtAlertDateFieldDesc.TabIndex = 27;
      this.txtAlertDateFieldDesc.TabStop = false;
      this.pnlTriggerAdj.Controls.Add((Control) this.txtAdjustDays);
      this.pnlTriggerAdj.Controls.Add((Control) this.cboAdjustDayType);
      this.pnlTriggerAdj.Controls.Add((Control) this.cboAdjustDir);
      this.pnlTriggerAdj.Controls.Add((Control) this.label10);
      this.pnlTriggerAdj.Controls.Add((Control) this.chkAdjustTrigger);
      this.pnlTriggerAdj.Location = new Point(10, 33);
      this.pnlTriggerAdj.Name = "pnlTriggerAdj";
      this.pnlTriggerAdj.Size = new Size(419, 43);
      this.pnlTriggerAdj.TabIndex = 2;
      this.txtAdjustDays.BackColor = SystemColors.Window;
      this.txtAdjustDays.BorderColor = Color.Black;
      this.txtAdjustDays.Enabled = false;
      this.txtAdjustDays.Location = new Point(107, 21);
      this.txtAdjustDays.MaxLength = 3;
      this.txtAdjustDays.Name = "txtAdjustDays";
      this.txtAdjustDays.Size = new Size(30, 22);
      this.txtAdjustDays.TabIndex = 2;
      this.txtAdjustDays.ChangeCommitted += new EventHandler(this.onFieldValueChanged);
      this.cboAdjustDayType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAdjustDayType.Enabled = false;
      this.cboAdjustDayType.FormattingEnabled = true;
      this.cboAdjustDayType.Items.AddRange(new object[3]
      {
        (object) "Calendar",
        (object) "Postal",
        (object) "Business"
      });
      this.cboAdjustDayType.Location = new Point(139, 21);
      this.cboAdjustDayType.Name = "cboAdjustDayType";
      this.cboAdjustDayType.Size = new Size(105, 22);
      this.cboAdjustDayType.TabIndex = 3;
      this.cboAdjustDayType.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.cboAdjustDir.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAdjustDir.Enabled = false;
      this.cboAdjustDir.FormattingEnabled = true;
      this.cboAdjustDir.Items.AddRange(new object[2]
      {
        (object) "Add",
        (object) "Subtract"
      });
      this.cboAdjustDir.Location = new Point(18, 21);
      this.cboAdjustDir.Name = "cboAdjustDir";
      this.cboAdjustDir.Size = new Size(88, 22);
      this.cboAdjustDir.TabIndex = 4;
      this.cboAdjustDir.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(247, 25);
      this.label10.Name = "label10";
      this.label10.Size = new Size(123, 14);
      this.label10.TabIndex = 22;
      this.label10.Text = "days to the Trigger Date";
      this.chkAdjustTrigger.AutoSize = true;
      this.chkAdjustTrigger.Location = new Point(0, 2);
      this.chkAdjustTrigger.Name = "chkAdjustTrigger";
      this.chkAdjustTrigger.Size = new Size(182, 18);
      this.chkAdjustTrigger.TabIndex = 1;
      this.chkAdjustTrigger.Text = "Apply calculation to Trigger Date";
      this.chkAdjustTrigger.UseVisualStyleBackColor = true;
      this.chkAdjustTrigger.CheckedChanged += new EventHandler(this.chkAdjustTrigger_CheckedChanged);
      this.btnTriggerLookup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTriggerLookup.BackColor = Color.Transparent;
      this.btnTriggerLookup.Location = new Point(498, 12);
      this.btnTriggerLookup.MouseDownImage = (Image) null;
      this.btnTriggerLookup.Name = "btnTriggerLookup";
      this.btnTriggerLookup.Size = new Size(16, 16);
      this.btnTriggerLookup.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnTriggerLookup.TabIndex = 16;
      this.btnTriggerLookup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnTriggerLookup, "Find Field");
      this.btnTriggerLookup.Click += new EventHandler(this.btnTriggerLookup_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(103, 14);
      this.label1.TabIndex = 14;
      this.label1.Text = "Trigger Date Field ID";
      this.txtAlertDateFieldID.Location = new Point(115, 10);
      this.txtAlertDateFieldID.MaxLength = 200;
      this.txtAlertDateFieldID.Name = "txtAlertDateFieldID";
      this.txtAlertDateFieldID.Size = new Size(88, 20);
      this.txtAlertDateFieldID.TabIndex = 1;
      this.txtAlertDateFieldID.Validating += new CancelEventHandler(this.txtAlertDateFieldID_Validating);
      this.pnlTrigger.Controls.Add((Control) this.label7);
      this.pnlTrigger.Controls.Add((Control) this.txtTrigger);
      this.pnlTrigger.Dock = DockStyle.Top;
      this.pnlTrigger.Location = new Point(0, 196);
      this.pnlTrigger.Name = "pnlTrigger";
      this.pnlTrigger.Size = new Size(545, 42);
      this.pnlTrigger.TabIndex = 3;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 7);
      this.label7.Name = "label7";
      this.label7.Size = new Size(66, 14);
      this.label7.TabIndex = 12;
      this.label7.Text = "Trigger Field";
      this.txtTrigger.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTrigger.Location = new Point(10, 22);
      this.txtTrigger.Name = "txtTrigger";
      this.txtTrigger.ReadOnly = true;
      this.txtTrigger.Size = new Size(525, 20);
      this.txtTrigger.TabIndex = 13;
      this.pnlButtons.Controls.Add((Control) this.btnSave);
      this.pnlButtons.Controls.Add((Control) this.btnReset);
      this.pnlButtons.Dock = DockStyle.Top;
      this.pnlButtons.Location = new Point(0, 1085);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(545, 44);
      this.pnlButtons.TabIndex = 21;
      this.pnlOptions.AutoSize = true;
      this.pnlOptions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlOptions.Controls.Add((Control) this.pnlOptionGroup);
      this.pnlOptions.Controls.Add((Control) this.pnlTriggerFields);
      this.pnlOptions.Controls.Add((Control) this.pnlCustomTriggerGroup);
      this.pnlOptions.Dock = DockStyle.Top;
      this.pnlOptions.Location = new Point(0, 238);
      this.pnlOptions.Name = "pnlOptions";
      this.pnlOptions.Padding = new Padding(10, 10, 10, 0);
      this.pnlOptions.Size = new Size(545, 847);
      this.pnlOptions.TabIndex = 23;
      this.pnlOptionGroup.AutoSize = true;
      this.pnlOptionGroup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlOptionGroup.Controls.Add((Control) this.pnlOptionSpacer);
      this.pnlOptionGroup.Controls.Add((Control) this.pnlAlertDays);
      this.pnlOptionGroup.Controls.Add((Control) this.pnlNotification);
      this.pnlOptionGroup.Controls.Add((Control) this.pnlAlert);
      this.pnlOptionGroup.Controls.Add((Control) this.pnlTILOptions);
      this.pnlOptionGroup.Controls.Add((Control) this.pnlCustomClearAlert);
      this.pnlOptionGroup.Dock = DockStyle.Top;
      this.pnlOptionGroup.Location = new Point(10, 576);
      this.pnlOptionGroup.Name = "pnlOptionGroup";
      this.pnlOptionGroup.Size = new Size(525, 271);
      this.pnlOptionGroup.TabIndex = 1;
      this.pnlOptionSpacer.BackColor = Color.Transparent;
      this.pnlOptionSpacer.Dock = DockStyle.Top;
      this.pnlOptionSpacer.Location = new Point(1, 262);
      this.pnlOptionSpacer.Name = "pnlOptionSpacer";
      this.pnlOptionSpacer.Size = new Size(523, 10);
      this.pnlOptionSpacer.TabIndex = 28;
      this.pnlAlertDays.Controls.Add((Control) this.lblDaysBefore);
      this.pnlAlertDays.Controls.Add((Control) this.txtDaysBefore);
      this.pnlAlertDays.Controls.Add((Control) this.label4);
      this.pnlAlertDays.Dock = DockStyle.Top;
      this.pnlAlertDays.Location = new Point(1, 232);
      this.pnlAlertDays.Name = "pnlAlertDays";
      this.pnlAlertDays.Size = new Size(523, 30);
      this.pnlAlertDays.TabIndex = 24;
      this.lblDaysBefore.AutoSize = true;
      this.lblDaysBefore.Location = new Point(216, 13);
      this.lblDaysBefore.Name = "lblDaysBefore";
      this.lblDaysBefore.Size = new Size(78, 14);
      this.lblDaysBefore.TabIndex = 2;
      this.lblDaysBefore.Text = "days before ...";
      this.txtDaysBefore.Location = new Point(155, 10);
      this.txtDaysBefore.Name = "txtDaysBefore";
      this.txtDaysBefore.Size = new Size(57, 20);
      this.txtDaysBefore.TabIndex = 5;
      this.txtDaysBefore.TextChanged += new EventHandler(this.txtDaysBefore_TextChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 13);
      this.label4.Name = "label4";
      this.label4.Size = new Size(67, 14);
      this.label4.TabIndex = 4;
      this.label4.Text = "Show Alert?";
      this.pnlNotification.Controls.Add((Control) this.btnRecipient);
      this.pnlNotification.Controls.Add((Control) this.radNotificationDisabled);
      this.pnlNotification.Controls.Add((Control) this.radNotificationEnabled);
      this.pnlNotification.Controls.Add((Control) this.label3);
      this.pnlNotification.Dock = DockStyle.Top;
      this.pnlNotification.Location = new Point(1, 187);
      this.pnlNotification.Name = "pnlNotification";
      this.pnlNotification.Size = new Size(523, 45);
      this.pnlNotification.TabIndex = 25;
      this.btnRecipient.Location = new Point(206, 9);
      this.btnRecipient.Name = "btnRecipient";
      this.btnRecipient.Size = new Size(106, 22);
      this.btnRecipient.TabIndex = 4;
      this.btnRecipient.Text = "Select Recipients";
      this.btnRecipient.UseVisualStyleBackColor = true;
      this.btnRecipient.Click += new EventHandler(this.btnRecipient_Click);
      this.radNotificationDisabled.AutoSize = true;
      this.radNotificationDisabled.Location = new Point(155, 30);
      this.radNotificationDisabled.Name = "radNotificationDisabled";
      this.radNotificationDisabled.Size = new Size(38, 18);
      this.radNotificationDisabled.TabIndex = 2;
      this.radNotificationDisabled.TabStop = true;
      this.radNotificationDisabled.Text = "No";
      this.radNotificationDisabled.UseVisualStyleBackColor = true;
      this.radNotificationEnabled.AutoSize = true;
      this.radNotificationEnabled.Location = new Point(155, 11);
      this.radNotificationEnabled.Name = "radNotificationEnabled";
      this.radNotificationEnabled.Size = new Size(44, 18);
      this.radNotificationEnabled.TabIndex = 1;
      this.radNotificationEnabled.TabStop = true;
      this.radNotificationEnabled.Text = "Yes";
      this.radNotificationEnabled.UseVisualStyleBackColor = true;
      this.radNotificationEnabled.CheckedChanged += new EventHandler(this.radNotificationEnabled_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(139, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Enable Pop-Up Notification?";
      this.pnlAlert.Controls.Add((Control) this.btnMilestone);
      this.pnlAlert.Controls.Add((Control) this.label2);
      this.pnlAlert.Controls.Add((Control) this.radAlertDisabled);
      this.pnlAlert.Controls.Add((Control) this.radAlertEnabled);
      this.pnlAlert.Dock = DockStyle.Top;
      this.pnlAlert.Location = new Point(1, 142);
      this.pnlAlert.Name = "pnlAlert";
      this.pnlAlert.Size = new Size(523, 45);
      this.pnlAlert.TabIndex = 23;
      this.btnMilestone.Location = new Point(206, 9);
      this.btnMilestone.Name = "btnMilestone";
      this.btnMilestone.Size = new Size(106, 22);
      this.btnMilestone.TabIndex = 3;
      this.btnMilestone.Text = "Select Milestones";
      this.btnMilestone.UseVisualStyleBackColor = true;
      this.btnMilestone.Click += new EventHandler(this.btnMilestone_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(70, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Enable Alert?";
      this.radAlertDisabled.AutoSize = true;
      this.radAlertDisabled.Location = new Point(155, 30);
      this.radAlertDisabled.Name = "radAlertDisabled";
      this.radAlertDisabled.Size = new Size(38, 18);
      this.radAlertDisabled.TabIndex = 1;
      this.radAlertDisabled.TabStop = true;
      this.radAlertDisabled.Text = "No";
      this.radAlertDisabled.UseVisualStyleBackColor = true;
      this.radAlertDisabled.CheckedChanged += new EventHandler(this.alertOption_CheckedChanged);
      this.radAlertEnabled.AutoSize = true;
      this.radAlertEnabled.Location = new Point(155, 11);
      this.radAlertEnabled.Name = "radAlertEnabled";
      this.radAlertEnabled.Size = new Size(44, 18);
      this.radAlertEnabled.TabIndex = 2;
      this.radAlertEnabled.TabStop = true;
      this.radAlertEnabled.Text = "Yes";
      this.radAlertEnabled.UseVisualStyleBackColor = true;
      this.radAlertEnabled.CheckedChanged += new EventHandler(this.alertOption_CheckedChanged);
      this.pnlTILOptions.Controls.Add((Control) this.panel1);
      this.pnlTILOptions.Controls.Add((Control) this.radTILAPRAll);
      this.pnlTILOptions.Controls.Add((Control) this.radTILAPRIncrease);
      this.pnlTILOptions.Controls.Add((Control) this.label6);
      this.pnlTILOptions.Controls.Add((Control) this.label8);
      this.pnlTILOptions.Dock = DockStyle.Top;
      this.pnlTILOptions.Location = new Point(1, 24);
      this.pnlTILOptions.Name = "pnlTILOptions";
      this.pnlTILOptions.Size = new Size(523, 118);
      this.pnlTILOptions.TabIndex = 26;
      this.panel1.Controls.Add((Control) this.radTILToleranceAll);
      this.panel1.Controls.Add((Control) this.radTILToleranceFixed);
      this.panel1.Location = new Point(155, 7);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(292, 39);
      this.panel1.TabIndex = 6;
      this.radTILToleranceAll.AutoSize = true;
      this.radTILToleranceAll.Location = new Point(0, 0);
      this.radTILToleranceAll.Name = "radTILToleranceAll";
      this.radTILToleranceAll.Size = new Size(121, 18);
      this.radTILToleranceAll.TabIndex = 1;
      this.radTILToleranceAll.TabStop = true;
      this.radTILToleranceAll.Text = "0.125% for all loans";
      this.radTILToleranceAll.UseVisualStyleBackColor = true;
      this.radTILToleranceAll.CheckedChanged += new EventHandler(this.onAPRToleranceChanged);
      this.radTILToleranceFixed.AutoSize = true;
      this.radTILToleranceFixed.Location = new Point(0, 19);
      this.radTILToleranceFixed.Name = "radTILToleranceFixed";
      this.radTILToleranceFixed.Size = new Size(289, 18);
      this.radTILToleranceFixed.TabIndex = 2;
      this.radTILToleranceFixed.TabStop = true;
      this.radTILToleranceFixed.Text = "0.125% for regular loans and 0.25% for irregular loans";
      this.radTILToleranceFixed.UseVisualStyleBackColor = true;
      this.radTILToleranceFixed.CheckedChanged += new EventHandler(this.onAPRToleranceChanged);
      this.radTILAPRAll.CheckAlign = ContentAlignment.TopLeft;
      this.radTILAPRAll.Location = new Point(155, 52);
      this.radTILAPRAll.Name = "radTILAPRAll";
      this.radTILAPRAll.Size = new Size(289, 32);
      this.radTILAPRAll.TabIndex = 3;
      this.radTILAPRAll.TabStop = true;
      this.radTILAPRAll.Text = "Alert when APR increases or decreases by more than the APR tolerance.";
      this.radTILAPRAll.TextAlign = ContentAlignment.TopLeft;
      this.radTILAPRAll.UseVisualStyleBackColor = true;
      this.radTILAPRAll.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radTILAPRIncrease.CheckAlign = ContentAlignment.TopLeft;
      this.radTILAPRIncrease.Location = new Point(155, 86);
      this.radTILAPRIncrease.Name = "radTILAPRIncrease";
      this.radTILAPRIncrease.Size = new Size(289, 32);
      this.radTILAPRIncrease.TabIndex = 4;
      this.radTILAPRIncrease.TabStop = true;
      this.radTILAPRIncrease.Text = "Alert only when APR increases by more than the APR tolerance.";
      this.radTILAPRIncrease.TextAlign = ContentAlignment.TopLeft;
      this.radTILAPRIncrease.UseVisualStyleBackColor = true;
      this.radTILAPRIncrease.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 55);
      this.label6.Name = "label6";
      this.label6.Size = new Size(64, 14);
      this.label6.TabIndex = 5;
      this.label6.Text = "Alert Option";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 11);
      this.label8.Name = "label8";
      this.label8.Size = new Size(78, 14);
      this.label8.TabIndex = 1;
      this.label8.Text = "APR Tolerance";
      this.pnlCustomClearAlert.Controls.Add((Control) this.chkAllowToClear);
      this.pnlCustomClearAlert.Dock = DockStyle.Top;
      this.pnlCustomClearAlert.Location = new Point(1, 1);
      this.pnlCustomClearAlert.Name = "pnlCustomClearAlert";
      this.pnlCustomClearAlert.Size = new Size(523, 23);
      this.pnlCustomClearAlert.TabIndex = 27;
      this.chkAllowToClear.AutoSize = true;
      this.chkAllowToClear.BackColor = Color.Transparent;
      this.chkAllowToClear.Location = new Point(10, 8);
      this.chkAllowToClear.Name = "chkAllowToClear";
      this.chkAllowToClear.Size = new Size(148, 18);
      this.chkAllowToClear.TabIndex = 0;
      this.chkAllowToClear.Text = "Provide Clear Alert button";
      this.chkAllowToClear.UseVisualStyleBackColor = false;
      this.chkAllowToClear.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.pnlTriggerFields.AutoSize = true;
      this.pnlTriggerFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlTriggerFields.Controls.Add((Control) this.grpDataCompletionFields);
      this.pnlTriggerFields.Controls.Add((Control) this.grpFields);
      this.pnlTriggerFields.Dock = DockStyle.Top;
      this.pnlTriggerFields.Location = new Point(10, 276);
      this.pnlTriggerFields.Name = "pnlTriggerFields";
      this.pnlTriggerFields.Size = new Size(525, 300);
      this.pnlTriggerFields.TabIndex = 30;
      this.pnlCustomTriggerGroup.AutoSize = true;
      this.pnlCustomTriggerGroup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlCustomTriggerGroup.Borders = AnchorStyles.None;
      this.pnlCustomTriggerGroup.Controls.Add((Control) this.pnlCustomTriggerSpacer);
      this.pnlCustomTriggerGroup.Controls.Add((Control) this.ctlCustomCondition);
      this.pnlCustomTriggerGroup.Controls.Add((Control) this.pnlCustomDate);
      this.pnlCustomTriggerGroup.Controls.Add((Control) this.pnlCustomType);
      this.pnlCustomTriggerGroup.Dock = DockStyle.Top;
      this.pnlCustomTriggerGroup.Location = new Point(10, 10);
      this.pnlCustomTriggerGroup.Name = "pnlCustomTriggerGroup";
      this.pnlCustomTriggerGroup.Size = new Size(525, 266);
      this.pnlCustomTriggerGroup.TabIndex = 0;
      this.pnlCustomTriggerSpacer.Dock = DockStyle.Top;
      this.pnlCustomTriggerSpacer.Location = new Point(0, 256);
      this.pnlCustomTriggerSpacer.Name = "pnlCustomTriggerSpacer";
      this.pnlCustomTriggerSpacer.Size = new Size(525, 10);
      this.pnlCustomTriggerSpacer.TabIndex = 7;
      this.ctlCustomCondition.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlCustomCondition.DDMSetting = false;
      this.ctlCustomCondition.Dock = DockStyle.Top;
      this.ctlCustomCondition.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlCustomCondition.Location = new Point(0, 118);
      this.ctlCustomCondition.Name = "ctlCustomCondition";
      this.ctlCustomCondition.Size = new Size(525, 138);
      this.ctlCustomCondition.TabIndex = 6;
      this.ctlCustomCondition.Title = "Conditions";
      this.ctlCustomCondition.DataChange += new EventHandler(this.ctlCustomCondition_DataChange);
      this.pnlCustomDate.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCustomDate.Controls.Add((Control) this.txtAlertDateFieldID);
      this.pnlCustomDate.Controls.Add((Control) this.txtAlertDateFieldDesc);
      this.pnlCustomDate.Controls.Add((Control) this.label1);
      this.pnlCustomDate.Controls.Add((Control) this.btnTriggerLookup);
      this.pnlCustomDate.Controls.Add((Control) this.pnlTriggerAdj);
      this.pnlCustomDate.Dock = DockStyle.Top;
      this.pnlCustomDate.Location = new Point(0, 32);
      this.pnlCustomDate.Name = "pnlCustomDate";
      this.pnlCustomDate.Size = new Size(525, 86);
      this.pnlCustomDate.TabIndex = 1;
      this.pnlCustomType.Controls.Add((Control) this.radActivateBoth);
      this.pnlCustomType.Controls.Add((Control) this.radActivateCondition);
      this.pnlCustomType.Controls.Add((Control) this.radActivateDate);
      this.pnlCustomType.Controls.Add((Control) this.label5);
      this.pnlCustomType.Dock = DockStyle.Top;
      this.pnlCustomType.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlCustomType.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlCustomType.Location = new Point(0, 0);
      this.pnlCustomType.Name = "pnlCustomType";
      this.pnlCustomType.Size = new Size(525, 32);
      this.pnlCustomType.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlCustomType.TabIndex = 0;
      this.radActivateBoth.AutoSize = true;
      this.radActivateBoth.BackColor = Color.Transparent;
      this.radActivateBoth.Location = new Point(207, 7);
      this.radActivateBoth.Name = "radActivateBoth";
      this.radActivateBoth.Size = new Size(129, 18);
      this.radActivateBoth.TabIndex = 21;
      this.radActivateBoth.TabStop = true;
      this.radActivateBoth.Text = "Date Field && Condition";
      this.radActivateBoth.UseVisualStyleBackColor = false;
      this.radActivateBoth.CheckedChanged += new EventHandler(this.onCustomAlertActivationChanged);
      this.radActivateCondition.AutoSize = true;
      this.radActivateCondition.BackColor = Color.Transparent;
      this.radActivateCondition.Location = new Point(135, 7);
      this.radActivateCondition.Name = "radActivateCondition";
      this.radActivateCondition.Size = new Size(69, 18);
      this.radActivateCondition.TabIndex = 20;
      this.radActivateCondition.TabStop = true;
      this.radActivateCondition.Text = "Condition";
      this.radActivateCondition.UseVisualStyleBackColor = false;
      this.radActivateCondition.CheckedChanged += new EventHandler(this.onCustomAlertActivationChanged);
      this.radActivateDate.AutoSize = true;
      this.radActivateDate.BackColor = Color.Transparent;
      this.radActivateDate.Location = new Point(60, 7);
      this.radActivateDate.Name = "radActivateDate";
      this.radActivateDate.Size = new Size(72, 18);
      this.radActivateDate.TabIndex = 18;
      this.radActivateDate.TabStop = true;
      this.radActivateDate.Text = "Date Field";
      this.radActivateDate.UseVisualStyleBackColor = false;
      this.radActivateDate.CheckedChanged += new EventHandler(this.onCustomAlertActivationChanged);
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(7, 9);
      this.label5.Name = "label5";
      this.label5.Size = new Size(47, 14);
      this.label5.TabIndex = 19;
      this.label5.Text = "Trigger";
      this.panel2.Controls.Add((Control) this.chkDeclineConsent);
      this.panel2.Controls.Add((Control) this.rdoFileStartedDate);
      this.panel2.Controls.Add((Control) this.rdoApplicationDate);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 45);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(545, 77);
      this.panel2.TabIndex = 24;
      this.chkDeclineConsent.AutoSize = true;
      this.chkDeclineConsent.Location = new Point(13, 61);
      this.chkDeclineConsent.Name = "chkDeclineConsent";
      this.chkDeclineConsent.Size = new Size(454, 18);
      this.chkDeclineConsent.TabIndex = 3;
      this.chkDeclineConsent.Text = "Do not show alert if borrower/co-borrower/non-borrowing owner has declined consent.";
      this.chkDeclineConsent.UseVisualStyleBackColor = true;
      this.chkDeclineConsent.CheckedChanged += new EventHandler(this.chkDeclineConsent_CheckedChanged);
      this.rdoFileStartedDate.AutoSize = true;
      this.rdoFileStartedDate.Location = new Point(35, 41);
      this.rdoFileStartedDate.Name = "rdoFileStartedDate";
      this.rdoFileStartedDate.Size = new Size(166, 18);
      this.rdoFileStartedDate.TabIndex = 2;
      this.rdoFileStartedDate.TabStop = true;
      this.rdoFileStartedDate.Text = "File Started Date (MS.START)";
      this.rdoFileStartedDate.UseVisualStyleBackColor = true;
      this.rdoFileStartedDate.CheckedChanged += new EventHandler(this.rdoApplicationDate_CheckedChanged);
      this.rdoApplicationDate.AutoSize = true;
      this.rdoApplicationDate.Location = new Point(35, 24);
      this.rdoApplicationDate.Name = "rdoApplicationDate";
      this.rdoApplicationDate.Size = new Size(175, 18);
      this.rdoApplicationDate.TabIndex = 1;
      this.rdoApplicationDate.TabStop = true;
      this.rdoApplicationDate.Text = "Application Date (Field ID 3142)";
      this.rdoApplicationDate.UseVisualStyleBackColor = true;
      this.rdoApplicationDate.CheckedChanged += new EventHandler(this.rdoApplicationDate_CheckedChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(10, 7);
      this.label9.Name = "label9";
      this.label9.Size = new Size(120, 14);
      this.label9.TabIndex = 0;
      this.label9.Text = "Choose the trigger date";
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnReset;
      this.ClientSize = new Size(545, 749);
      this.Controls.Add((Control) this.pnlButtons);
      this.Controls.Add((Control) this.pnlOptions);
      this.Controls.Add((Control) this.pnlTrigger);
      this.Controls.Add((Control) this.pnlMessage);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.pnlName);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AlertEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Alert Settings";
      this.Shown += new EventHandler(this.AlertEditor_Shown);
      this.grpFields.ResumeLayout(false);
      this.pnlFieldButtons.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveField).EndInit();
      ((ISupportInitialize) this.btnAddField).EndInit();
      this.grpDataCompletionFields.ResumeLayout(false);
      this.pnlDataCompletionFieldButtons.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDataCompletionField).EndInit();
      ((ISupportInitialize) this.btnAddDataCompletionField).EndInit();
      this.pnlName.ResumeLayout(false);
      this.pnlName.PerformLayout();
      this.pnlMessage.ResumeLayout(false);
      this.pnlMessage.PerformLayout();
      this.pnlTriggerAdj.ResumeLayout(false);
      this.pnlTriggerAdj.PerformLayout();
      ((ISupportInitialize) this.btnTriggerLookup).EndInit();
      this.pnlTrigger.ResumeLayout(false);
      this.pnlTrigger.PerformLayout();
      this.pnlButtons.ResumeLayout(false);
      this.pnlOptions.ResumeLayout(false);
      this.pnlOptions.PerformLayout();
      this.pnlOptionGroup.ResumeLayout(false);
      this.pnlAlertDays.ResumeLayout(false);
      this.pnlAlertDays.PerformLayout();
      this.pnlNotification.ResumeLayout(false);
      this.pnlNotification.PerformLayout();
      this.pnlAlert.ResumeLayout(false);
      this.pnlAlert.PerformLayout();
      this.pnlTILOptions.ResumeLayout(false);
      this.pnlTILOptions.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlCustomClearAlert.ResumeLayout(false);
      this.pnlCustomClearAlert.PerformLayout();
      this.pnlTriggerFields.ResumeLayout(false);
      this.pnlCustomTriggerGroup.ResumeLayout(false);
      this.pnlCustomDate.ResumeLayout(false);
      this.pnlCustomDate.PerformLayout();
      this.pnlCustomType.ResumeLayout(false);
      this.pnlCustomType.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
