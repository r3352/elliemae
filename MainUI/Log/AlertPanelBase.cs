// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AlertPanelBase
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AlertPanelBase : UserControl
  {
    private PipelineInfo.Alert alert;
    private AlertConfig alertConfig;
    private Hashtable hiddenFields;
    private IContainer components;
    protected FlowLayoutPanel flpControls;
    protected Button btnClear;
    protected GroupContainer grpFields;
    protected FlowLayoutPanel flowLayoutPanel2;
    protected Button btnGoToField;
    protected GridView gvFields;
    protected Panel pnlAltDate;
    protected GroupContainer grpDetails;
    protected TextBox txtAlertDate;
    protected TextBox txtAltDate;
    protected Label lblAltDate;
    protected Label lblAlertDate;
    protected TextBox txtMessage;
    protected Label lblDescription;
    protected TextBox txtAlertName;
    protected Label lblAlertName;
    protected Panel pnlFields;

    public event EventHandler ClearAlert;

    public event EventHandler FieldSelectedIndexChanged;

    public AlertPanelBase()
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.hiddenFields = Session.LoanDataMgr.GetHiddenFields();
    }

    public PipelineInfo.Alert Alert => this.alert;

    public AlertConfig AlertConfig => this.alertConfig;

    public virtual bool AllowClearAlert => false;

    public string AlertDateCaption
    {
      get => this.lblAlertDate.Text;
      set
      {
        this.lblAlertDate.Text = value;
        this.AdjustControlPositions();
      }
    }

    protected void SetAlertDate(DateTime alertDate)
    {
      if (alertDate != DateTime.MinValue)
        this.txtAlertDate.Text = alertDate.ToString("MM/dd/yyyy");
      else
        this.txtAlertDate.Text = "N/A";
    }

    protected void ShowAlternateDate(string caption, DateTime date)
    {
      this.pnlAltDate.Visible = true;
      this.lblAltDate.Text = caption;
      if (date != DateTime.MinValue)
        this.txtAltDate.Text = date.ToString("MM/dd/yyyy");
      else
        this.txtAltDate.Text = "N/A";
      this.AdjustControlPositions();
    }

    public void SetAlert(PipelineInfo.Alert alert)
    {
      this.alert = alert;
      this.alertConfig = Session.LoanDataMgr.SystemConfiguration.AlertSetupData.GetAlertConfig(alert.AlertID);
      this.RefreshAlert();
    }

    protected void AddHeaderControl(Control c) => this.flpControls.Controls.Add(c);

    protected void AddTriggerFieldsControl(Control c)
    {
      if (this.flowLayoutPanel2.Contains((Control) this.btnGoToField))
        this.flowLayoutPanel2.Controls.Remove((Control) this.btnGoToField);
      this.flowLayoutPanel2.Controls.Add(c);
    }

    protected virtual void RefreshAlert()
    {
      this.grpDetails.Text = this.alert.AlertMessage;
      this.txtAlertName.Text = this.alert.AlertMessage;
      this.txtAlertDate.Text = this.alert.Date.ToString("MM/dd/yyyy");
      this.btnClear.Visible = this.AllowClearAlert;
      if (this.alertConfig == null)
        return;
      this.txtAlertName.Text = this.alertConfig.Definition.Name;
      this.txtMessage.Text = this.alertConfig.Message;
      this.ConfigureTriggerFieldList();
      this.PopulateTriggerFields();
    }

    protected virtual void ConfigureTriggerFieldList()
    {
    }

    protected virtual void PopulateTriggerFields()
    {
      this.gvFields.Items.Clear();
      if (this.alertConfig.Definition is RegulationAlert)
        this.populateRegulationTriggerFields(this.alertConfig.Definition as RegulationAlert);
      foreach (string triggerField in this.alertConfig.TriggerFieldList)
      {
        GVItem triggerFieldItem = this.CreateTriggerFieldItem(triggerField);
        if (triggerFieldItem != null)
        {
          if (this.alertConfig.AlertID == 43 && this.pricingFieldHighlight(triggerField))
            triggerFieldItem.ForeColor = Color.Red;
          this.gvFields.Items.Add(triggerFieldItem);
        }
      }
    }

    private void populateRegulationTriggerFields(RegulationAlert alertDef)
    {
      foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) alertDef.TriggerFields)
      {
        GVItem triggerFieldItem = this.CreateTriggerFieldItem(triggerField.FieldID);
        if (triggerFieldItem != null)
        {
          triggerFieldItem.Tag = (object) triggerField;
          if (alertDef.AlertID == 43 && this.pricingFieldHighlight(triggerField.FieldID))
            triggerFieldItem.ForeColor = Color.Red;
          this.gvFields.Items.Add(triggerFieldItem);
        }
      }
    }

    private bool pricingFieldHighlight(string fieldID)
    {
      LoanData loanData = Session.LoanDataMgr.LoanData;
      LockRequestLog lastConfirmedLock = loanData.GetLogList().GetLastConfirmedLock();
      if (lastConfirmedLock == null)
        return false;
      Hashtable lockRequestSnapshot = lastConfirmedLock.GetLockRequestSnapshot();
      if (lockRequestSnapshot.Contains((object) fieldID))
      {
        if (lockRequestSnapshot[(object) fieldID].ToString() != loanData.GetField(fieldID))
          return true;
      }
      else if (loanData.GetField(fieldID) != "")
        return true;
      return false;
    }

    protected virtual GVItem CreateTriggerFieldItem(string fieldID)
    {
      GVItem triggerFieldItem = new GVItem();
      string fieldId = this.checkCollectionFieldID(fieldID);
      triggerFieldItem.SubItems[0].Text = fieldId;
      triggerFieldItem.SubItems[1].Text = EncompassFields.GetDescription(fieldID, Session.LoanDataMgr.SystemConfiguration.LoanSettings.FieldSettings);
      this.handleSubItemForValue(triggerFieldItem.SubItems[2], fieldId);
      return triggerFieldItem;
    }

    private void btnClear_Click(object sender, EventArgs e) => this.OnClearAlert(EventArgs.Empty);

    protected virtual void OnClearAlert(EventArgs e)
    {
      if (this.ClearAlert == null)
        return;
      this.ClearAlert((object) this, e);
    }

    protected override void OnLoad(EventArgs e)
    {
      this.AdjustControlPositions();
      base.OnLoad(e);
    }

    private void btnGoToField_Click(object sender, EventArgs e)
    {
      this.GoToField(this.checkCollectionFieldID(!(this.gvFields.SelectedItems[0].Tag is GFFVAlertTriggerField) ? (!(this.gvFields.SelectedItems[0].Tag is AlertTriggerField) ? (!(this.gvFields.SelectedItems[0].Tag is string) ? this.gvFields.SelectedItems[0].SubItems[0].Text : this.gvFields.SelectedItems[0].Tag.ToString()) : ((AlertTriggerField) this.gvFields.SelectedItems[0].Tag).FieldID) : ((GFFVAlertTriggerField) this.gvFields.SelectedItems[0].Tag).FieldId));
    }

    protected virtual void GoToField(string fieldId)
    {
      Session.Application.GetService<ILoanEditor>().GoToField(fieldId);
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnGoToField.Enabled = this.gvFields.SelectedItems.Count == 1;
      if (this.FieldSelectedIndexChanged == null)
        return;
      object sender1 = (object) null;
      if (this.gvFields.SelectedItems.Count > 0)
        sender1 = !(this.gvFields.SelectedItems[0].Tag is GFFVAlertTriggerField) ? (!(this.gvFields.SelectedItems[0].Tag is AlertTriggerField) ? (!(this.gvFields.SelectedItems[0].Tag is string) ? (object) this.gvFields.SelectedItems[0].SubItems[0].Text : (object) this.gvFields.SelectedItems[0].Tag.ToString()) : (object) (AlertTriggerField) this.gvFields.SelectedItems[0].Tag) : (object) (GFFVAlertTriggerField) this.gvFields.SelectedItems[0].Tag;
      this.FieldSelectedIndexChanged(sender1, e);
    }

    protected virtual void AdjustControlPositions(int additionalWidth = 0)
    {
      this.txtAlertName.Left = this.txtMessage.Left = this.txtAlertDate.Left = Math.Max(Math.Max(Math.Max(this.lblAlertName.Width, this.lblDescription.Width), this.lblAlertDate.Width), additionalWidth) + 10;
      TextBox txtAlertName = this.txtAlertName;
      TextBox txtMessage = this.txtMessage;
      int val2 = this.grpDetails.ClientRectangle.Width - this.txtAlertName.Left - 9;
      int num1;
      int num2 = num1 = Math.Max(0, val2);
      txtMessage.Width = num1;
      int num3 = num2;
      txtAlertName.Width = num3;
      this.txtAltDate.Left = this.lblAltDate.Right + 9;
      this.pnlAltDate.Width = this.txtAltDate.Right;
      this.pnlAltDate.Left = this.txtAlertDate.Right + 10;
      this.flpControls.Left = this.Width - this.flpControls.Width - 3;
    }

    protected void SetGroupHeaderText(string headerText) => this.grpFields.Text = headerText;

    private void grpDetails_Resize(object sender, EventArgs e) => this.AdjustControlPositions();

    private string checkCollectionFieldID(string inputFieldId)
    {
      string str = inputFieldId;
      if (str.Length >= 4 && Regex.IsMatch(str.Substring(0, 4), "[A-Za-z][A-Za-z]00"))
      {
        int result = 0;
        if (int.TryParse(this.alert.Status, out result) && result > 0)
          str = str.Substring(0, 2) + result.ToString("00") + str.Substring(4);
      }
      return str;
    }

    private void handleSubItemForValue(GVSubItem gvSubItem, string fieldId)
    {
      if (this.isHiddenField(fieldId))
      {
        gvSubItem.Text = "*";
        gvSubItem.BackColor = SystemColors.InactiveCaption;
      }
      else
        gvSubItem.Text = Session.LoanDataMgr.LoanData.GetField(fieldId);
    }

    private bool isHiddenField(string fieldId)
    {
      return this.hiddenFields != null && this.hiddenFields.Count != 0 && this.hiddenFields.ContainsKey((object) fieldId);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.pnlFields = new Panel();
      this.grpFields = new GroupContainer();
      this.gvFields = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnGoToField = new Button();
      this.grpDetails = new GroupContainer();
      this.pnlAltDate = new Panel();
      this.txtAltDate = new TextBox();
      this.lblAltDate = new Label();
      this.txtAlertDate = new TextBox();
      this.lblAlertDate = new Label();
      this.txtMessage = new TextBox();
      this.lblDescription = new Label();
      this.txtAlertName = new TextBox();
      this.lblAlertName = new Label();
      this.flpControls = new FlowLayoutPanel();
      this.btnClear = new Button();
      this.pnlFields.SuspendLayout();
      this.grpFields.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.pnlAltDate.SuspendLayout();
      this.flpControls.SuspendLayout();
      this.SuspendLayout();
      this.pnlFields.Controls.Add((Control) this.grpFields);
      this.pnlFields.Dock = DockStyle.Fill;
      this.pnlFields.Location = new Point(0, 126);
      this.pnlFields.Name = "pnlFields";
      this.pnlFields.Size = new Size(970, 442);
      this.pnlFields.TabIndex = 1;
      this.grpFields.Controls.Add((Control) this.gvFields);
      this.grpFields.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpFields.Dock = DockStyle.Fill;
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(0, 0);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(970, 442);
      this.grpFields.TabIndex = 1;
      this.grpFields.Text = "Trigger Fields";
      this.gvFields.AllowMultiselect = false;
      this.gvFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colFieldID";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDescription";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colValue";
      gvColumn3.Text = "Value";
      gvColumn3.Width = 150;
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
      this.gvFields.Size = new Size(968, 415);
      this.gvFields.TabIndex = 2;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnGoToField);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(116, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(850, 22);
      this.flowLayoutPanel2.TabIndex = 1;
      this.btnGoToField.Enabled = false;
      this.btnGoToField.Location = new Point(773, 0);
      this.btnGoToField.Margin = new Padding(3, 0, 2, 0);
      this.btnGoToField.Name = "btnGoToField";
      this.btnGoToField.Size = new Size(75, 22);
      this.btnGoToField.TabIndex = 0;
      this.btnGoToField.Text = "&Go to Field";
      this.btnGoToField.UseVisualStyleBackColor = true;
      this.btnGoToField.Click += new EventHandler(this.btnGoToField_Click);
      this.grpDetails.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpDetails.Controls.Add((Control) this.pnlAltDate);
      this.grpDetails.Controls.Add((Control) this.txtAlertDate);
      this.grpDetails.Controls.Add((Control) this.lblAlertDate);
      this.grpDetails.Controls.Add((Control) this.txtMessage);
      this.grpDetails.Controls.Add((Control) this.lblDescription);
      this.grpDetails.Controls.Add((Control) this.txtAlertName);
      this.grpDetails.Controls.Add((Control) this.lblAlertName);
      this.grpDetails.Controls.Add((Control) this.flpControls);
      this.grpDetails.Dock = DockStyle.Top;
      this.grpDetails.ForeColor = SystemColors.ControlText;
      this.grpDetails.HeaderForeColor = Color.FromArgb(238, 0, 0);
      this.grpDetails.Location = new Point(0, 0);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(970, 126);
      this.grpDetails.TabIndex = 0;
      this.grpDetails.Text = "<Alert Title>";
      this.grpDetails.Resize += new EventHandler(this.grpDetails_Resize);
      this.pnlAltDate.Controls.Add((Control) this.txtAltDate);
      this.pnlAltDate.Controls.Add((Control) this.lblAltDate);
      this.pnlAltDate.Location = new Point(242, 96);
      this.pnlAltDate.Name = "pnlAltDate";
      this.pnlAltDate.Size = new Size(210, 21);
      this.pnlAltDate.TabIndex = 7;
      this.pnlAltDate.Visible = false;
      this.txtAltDate.Location = new Point(66, 0);
      this.txtAltDate.Name = "txtAltDate";
      this.txtAltDate.ReadOnly = true;
      this.txtAltDate.Size = new Size(137, 20);
      this.txtAltDate.TabIndex = 8;
      this.lblAltDate.AutoSize = true;
      this.lblAltDate.Location = new Point(-2, 4);
      this.lblAltDate.Name = "lblAltDate";
      this.lblAltDate.Size = new Size(55, 14);
      this.lblAltDate.TabIndex = 7;
      this.lblAltDate.Text = "Alert Date";
      this.txtAlertDate.Location = new Point(75, 96);
      this.txtAlertDate.Name = "txtAlertDate";
      this.txtAlertDate.ReadOnly = true;
      this.txtAlertDate.Size = new Size(137, 20);
      this.txtAlertDate.TabIndex = 6;
      this.lblAlertDate.AutoSize = true;
      this.lblAlertDate.Location = new Point(7, 100);
      this.lblAlertDate.Name = "lblAlertDate";
      this.lblAlertDate.Size = new Size(55, 14);
      this.lblAlertDate.TabIndex = 5;
      this.lblAlertDate.Text = "Alert Date";
      this.txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMessage.Location = new Point(75, 58);
      this.txtMessage.Multiline = true;
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.ReadOnly = true;
      this.txtMessage.ScrollBars = ScrollBars.Vertical;
      this.txtMessage.Size = new Size(885, 36);
      this.txtMessage.TabIndex = 4;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(7, 62);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Description";
      this.txtAlertName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAlertName.Location = new Point(75, 36);
      this.txtAlertName.Name = "txtAlertName";
      this.txtAlertName.ReadOnly = true;
      this.txtAlertName.Size = new Size(527, 20);
      this.txtAlertName.TabIndex = 2;
      this.lblAlertName.AutoSize = true;
      this.lblAlertName.Location = new Point(7, 40);
      this.lblAlertName.Name = "lblAlertName";
      this.lblAlertName.Size = new Size(60, 14);
      this.lblAlertName.TabIndex = 1;
      this.lblAlertName.Text = "Alert Name";
      this.flpControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpControls.BackColor = Color.Transparent;
      this.flpControls.Controls.Add((Control) this.btnClear);
      this.flpControls.FlowDirection = FlowDirection.RightToLeft;
      this.flpControls.Location = new Point(160, 2);
      this.flpControls.Name = "flpControls";
      this.flpControls.Size = new Size(806, 22);
      this.flpControls.TabIndex = 0;
      this.btnClear.Location = new Point(729, 0);
      this.btnClear.Margin = new Padding(3, 0, 2, 0);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 22);
      this.btnClear.TabIndex = 0;
      this.btnClear.Text = "&Clear Alert";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlFields);
      this.Controls.Add((Control) this.grpDetails);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AlertPanelBase);
      this.Size = new Size(970, 568);
      this.pnlFields.ResumeLayout(false);
      this.grpFields.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.flpControls.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
