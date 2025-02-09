// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Alerts
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.MSWorksheet;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class Alerts : SettingsUserControl
  {
    private List<EllieMae.EMLite.Workflow.Milestone> availableMilestones;
    private bool allowMultiSelect;
    private Sessions.Session currentSession;
    private FieldSettings fieldSettings;
    private IContainer components;
    private GroupContainer gcAlerts;
    private StandardIconButton stdIconBtnEdit;
    private ToolTip toolTip1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem editToolStripMenuItem;
    private GridView gvAlerts;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton stdIconBtnAdd;
    private StandardIconButton stdIconBtnDelete;

    public Alerts(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer, false)
    {
    }

    public Alerts(Sessions.Session session, SetUpContainer setupContainer, bool allowMultiSelect)
      : base(setupContainer)
    {
      this.currentSession = session;
      this.allowMultiSelect = allowMultiSelect;
      this.setupContainer = setupContainer;
      this.fieldSettings = this.currentSession.LoanManager.GetFieldSettings();
      this.InitializeComponent();
      this.gvAlerts.AllowMultiselect = allowMultiSelect;
      this.availableMilestones = SetupUtil.GetMilestones(this.currentSession, false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      this.init();
    }

    private void init()
    {
      this.gvAlerts.Items.Clear();
      if (this.currentSession.EncompassEdition != EncompassEdition.Banker)
        this.stdIconBtnAdd.Visible = false;
      AlertConfig[] alertConfigList = this.currentSession.AlertManager.GetAlertConfigList();
      Array.Sort<AlertConfig>(alertConfigList, new Comparison<AlertConfig>(this.compareAlertConfigs));
      foreach (AlertConfig alertConfig in alertConfigList)
      {
        if (alertConfig.Definition.AppliesToEdition(this.currentSession.EncompassEdition))
        {
          GVItem gvItem = new GVItem();
          this.populateGVItemFromAlertConfig(gvItem, alertConfig);
          this.gvAlerts.Items.Add(gvItem);
        }
      }
      this.gvAlerts_SelectedIndexChanged((object) this, (EventArgs) null);
      this.gcAlerts.Text = "Alerts (" + (object) this.gvAlerts.Items.Count + ")";
    }

    private void populateGVItemFromAlertConfig(GVItem item, AlertConfig alertConfig)
    {
      item.SubItems[0].Text = alertConfig.Definition.Name;
      item.SubItems[1].Text = this.getAlertTriggerDescription(alertConfig);
      item.SubItems[2].Text = this.getMilestoneList(alertConfig.AlertEnabled, alertConfig.MilestoneGuidList);
      item.SubItems[3].Text = this.getDaysBeforeText(alertConfig);
      item.SubItems[4].Text = alertConfig.Definition.NotificationType != AlertNotificationType.None ? (alertConfig.NotificationEnabled ? "Yes" : "No") : "N/A";
      switch (alertConfig.AlertID)
      {
        case 43:
        case 64:
        case 65:
        case 66:
          item.SubItems[5].Text = "";
          break;
        default:
          item.SubItems[5].Text = alertConfig.Definition.Category != AlertCategory.Regulation ? (alertConfig.Definition.Category != AlertCategory.Custom ? "" : "Custom") : "Compliance" + this.getRegulationName(alertConfig.AlertID);
          break;
      }
      item.Tag = (object) alertConfig;
    }

    public string[] SelectedAlerts
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAlerts.Items)
        {
          AlertConfig tag = (AlertConfig) gvItem.Tag;
          if (gvItem.Selected)
          {
            if (tag.AlertID < 1000)
            {
              stringList.Add("<<" + tag.Definition.Name + ">>");
              stringList.Add(tag.Definition.Name);
            }
            else
              stringList.Add(tag.Definition.Name);
          }
        }
        return stringList.ToArray();
      }
      set
      {
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAlerts.Items)
          gvItem.Selected = stringList.Contains(((AlertConfig) gvItem.Tag).Definition.Name) || stringList.Contains("<<" + ((AlertConfig) gvItem.Tag).Definition.Name + ">>");
      }
    }

    private string getRegulationName(int alertId)
    {
      switch ((StandardAlertID) alertId)
      {
        case StandardAlertID.RediscloseTILRateChange:
        case StandardAlertID.ClosingDateViolation:
          return "  (MDIA, Jul 2009)";
        default:
          if (!EllieMae.EMLite.DataEngine.Alerts.IsRegulationAlert(alertId))
            return "";
          switch (alertId)
          {
            case 22:
              return "  (RESPA-TILA, Oct 2015 & Jan 2010)";
            case 45:
            case 46:
            case 47:
            case 48:
            case 49:
            case 50:
            case 51:
            case 63:
            case 72:
            case 73:
              return "  (RESPA-TILA, Oct 2015)";
            default:
              return "  (RESPA, Jan 2010)";
          }
      }
    }

    private int compareAlertConfigs(AlertConfig a, AlertConfig b)
    {
      return a.Definition.CompareTo(b.Definition);
    }

    private string getAlertTriggerDescription(AlertConfig alertConfig)
    {
      if (alertConfig.Definition is WorkflowAlert)
        return ((WorkflowAlert) alertConfig.Definition).TriggerDescription;
      return alertConfig.Definition is RegulationAlert ? this.getRegulationFieldListDescription(alertConfig) : this.getCustomAlertFieldListDescription(alertConfig);
    }

    private string getCustomAlertFieldListDescription(AlertConfig alertConfig)
    {
      List<string> stringList = new List<string>();
      foreach (string triggerField in alertConfig.TriggerFieldList)
        stringList.Add(this.getFieldDescription(triggerField) + " (" + triggerField + ")");
      return string.Join(", ", stringList.ToArray());
    }

    private string getRegulationFieldListDescription(AlertConfig alertConfig)
    {
      List<string> stringList = new List<string>();
      RegulationAlert definition = (RegulationAlert) alertConfig.Definition;
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) definition.TriggerFields)
        stringList.Add(this.getFieldDescription(triggerField.FieldID) + " (" + triggerField.FieldID + ")");
      if (definition.AllowUserDefinedTriggerFields)
      {
        foreach (string triggerField in alertConfig.TriggerFieldList)
          stringList.Add(this.getFieldDescription(triggerField) + " (" + triggerField + ")");
      }
      return string.Join(", ", stringList.ToArray());
    }

    private string getFieldDescription(string fieldId)
    {
      string fieldDescription = EncompassFields.GetDescription(fieldId, this.fieldSettings);
      if ((fieldDescription ?? "") == "")
        fieldDescription = "<Unknown Field>";
      return fieldDescription;
    }

    private string getDaysBeforeText(AlertConfig config)
    {
      return config.AlertTiming == AlertTiming.Immediate ? "N/A - Immediate" : config.DaysBefore.ToString() + " days before";
    }

    private string getMilestoneList(bool displayOnPipeline, List<string> milestoneGuidList)
    {
      string str = "";
      bool flag = true;
      string milestoneList;
      if (!displayOnPipeline)
      {
        milestoneList = "No";
      }
      else
      {
        foreach (EllieMae.EMLite.Workflow.Milestone availableMilestone in this.availableMilestones)
        {
          if (milestoneGuidList.Contains(availableMilestone.MilestoneID))
            str = !(str == "") ? str + ", " + availableMilestone.Name : availableMilestone.Name;
          else if (availableMilestone.MilestoneID != 7.ToString())
            flag = false;
        }
        if (flag)
          str = "<All Milestones>";
        milestoneList = !(str != "") ? "Yes (<No Milestone Selected>)" : "Yes (" + str + ")";
      }
      return milestoneList;
    }

    private void gvAlerts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void onEditAlert(object source, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      GVItem selectedItem = this.gvAlerts.SelectedItems[0];
      using (AlertEditor alertEditor = new AlertEditor((AlertConfig) selectedItem.Tag, this.fieldSettings, this.currentSession))
      {
        if (alertEditor.ShowDialog((IWin32Window) this.setupContainer) != DialogResult.OK)
          return;
        this.populateGVItemFromAlertConfig(selectedItem, alertEditor.GetAlert());
      }
    }

    private void gvAlerts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.gvAlerts.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.gvAlerts.SelectedItems.Count == 1 && ((AlertConfig) this.gvAlerts.SelectedItems[0].Tag).Definition is CustomAlert;
      this.editToolStripMenuItem.Enabled = this.gvAlerts.SelectedItems.Count == 1;
    }

    public void stdIconBtnAdd_Click(object sender, EventArgs e)
    {
      using (AlertEditor alertEditor = new AlertEditor(new AlertConfig(), this.fieldSettings, this.currentSession))
      {
        if (alertEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int alertId = alertEditor.GetAlert().AlertID;
        this.init();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAlerts.Items)
        {
          if (((AlertConfig) gvItem.Tag).AlertID == alertId)
            gvItem.Selected = true;
        }
      }
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      AlertConfig tag = (AlertConfig) this.gvAlerts.SelectedItems[0].Tag;
      if (Utils.Dialog((IWin32Window) this, "The custom alert '" + tag.Definition.Name + "' will be permanently deleted and the alert will be removed from any loans in which it is currently active." + Environment.NewLine + Environment.NewLine + "Are you sure you want to delete this alert?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      using (CursorActivator.Wait())
      {
        this.currentSession.AlertManager.DeleteAlertConfig(tag);
        this.init();
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.gcAlerts = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnAdd = new StandardIconButton();
      this.gvAlerts = new GridView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.editToolStripMenuItem = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.gcAlerts.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.gcAlerts.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcAlerts.Controls.Add((Control) this.gvAlerts);
      this.gcAlerts.Dock = DockStyle.Fill;
      this.gcAlerts.HeaderForeColor = SystemColors.ControlText;
      this.gcAlerts.Location = new Point(0, 0);
      this.gcAlerts.Name = "gcAlerts";
      this.gcAlerts.Size = new Size(785, 452);
      this.gcAlerts.TabIndex = 2;
      this.gcAlerts.Text = "Alerts (16)";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnAdd);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(706, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(74, 22);
      this.flowLayoutPanel1.TabIndex = 2;
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(58, 3);
      this.stdIconBtnEdit.Margin = new Padding(5, 3, 0, 3);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 0;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.onEditAlert);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Enabled = false;
      this.stdIconBtnDelete.Location = new Point(37, 3);
      this.stdIconBtnDelete.Margin = new Padding(5, 3, 0, 3);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 2;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Alert");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(16, 3);
      this.stdIconBtnAdd.Margin = new Padding(5, 3, 0, 3);
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 1;
      this.stdIconBtnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnAdd, "Add Alert");
      this.stdIconBtnAdd.Click += new EventHandler(this.stdIconBtnAdd_Click);
      this.gvAlerts.AllowColumnReorder = true;
      this.gvAlerts.AllowMultiselect = false;
      this.gvAlerts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 220;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colTriggerField";
      gvColumn2.Text = "Trigger Field";
      gvColumn2.Width = 235;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colEnableAlert";
      gvColumn3.Text = "Enable Alert?";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colShowAlert";
      gvColumn4.Text = "Show Alert?";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colNotification";
      gvColumn5.Text = "Notification?";
      gvColumn5.Width = 75;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colAlertType";
      gvColumn6.Text = "Alert Type";
      gvColumn6.Width = 180;
      this.gvAlerts.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvAlerts.ContextMenuStrip = this.contextMenuStrip1;
      this.gvAlerts.Dock = DockStyle.Fill;
      this.gvAlerts.Location = new Point(1, 26);
      this.gvAlerts.Name = "gvAlerts";
      this.gvAlerts.Size = new Size(783, 425);
      this.gvAlerts.TabIndex = 1;
      this.gvAlerts.SelectedIndexChanged += new EventHandler(this.gvAlerts_SelectedIndexChanged);
      this.gvAlerts.ItemDoubleClick += new GVItemEventHandler(this.gvAlerts_ItemDoubleClick);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.editToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(104, 26);
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new Size(103, 22);
      this.editToolStripMenuItem.Text = "Edit";
      this.editToolStripMenuItem.Click += new EventHandler(this.onEditAlert);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcAlerts);
      this.Name = nameof (Alerts);
      this.Size = new Size(785, 452);
      this.gcAlerts.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
