// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.GoodFaithFeeVarianceCureLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class GoodFaithFeeVarianceCureLogWS : UserControl, IOnlineHelpTarget
  {
    private GoodFaithFeeVarianceCureLog goodFaithFeeVarianceCureLog;
    private IContainer components;
    private GroupContainer gcGFFVC;
    private GroupContainer grpFields;
    protected GridView gvFields;
    protected GroupContainer grpDetails;
    protected TextBox txtAlertDate;
    protected Label lblAlertDate;
    protected TextBox txtDescription;
    protected Label lblDescription;
    protected TextBox txtAlertName;
    protected Label lblAlertName;
    private FlowLayoutPanel flpControls;
    private GroupContainer gcResolution;
    protected TextBox txtAppliedCureAmount;
    protected Label label1;
    protected Label label4;
    protected TextBox txtResolvedBy;
    protected Label label3;
    protected TextBox txtDate;
    protected Label label2;
    protected TextBox txtComments;
    protected TextBox txtTotalVariance;
    protected Label label5;

    public GoodFaithFeeVarianceCureLogWS(GoodFaithFeeVarianceCureLog log)
    {
      this.goodFaithFeeVarianceCureLog = log;
      this.InitializeComponent();
      this.refreshLogDetails();
    }

    public GoodFaithFeeVarianceCureLogWS() => this.InitializeComponent();

    private void refreshLogDetails()
    {
      this.ConfigureTriggerFieldList();
      this.txtDate.Text = this.goodFaithFeeVarianceCureLog.Date.ToString("MM/dd/yyyy");
      this.txtResolvedBy.Text = this.goodFaithFeeVarianceCureLog.ResolvedById;
      this.txtAppliedCureAmount.Text = this.goodFaithFeeVarianceCureLog.AppliedCureAmount;
      this.txtComments.Text = this.goodFaithFeeVarianceCureLog.CureComments;
      this.txtAlertName.Text = "Good Faith Fee Variance Violated";
      this.txtAlertDate.Text = this.goodFaithFeeVarianceCureLog.AlertDate.ToString("MM/dd/yyyy");
      this.txtTotalVariance.Text = this.goodFaithFeeVarianceCureLog.AppliedCureAmount;
      this.gvFields.Items.Clear();
      foreach (GFFVAlertTriggerField triggerField in this.goodFaithFeeVarianceCureLog.TriggerFieldList)
        this.gvFields.Items.Add(this.createGVItem(triggerField));
    }

    private void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Field ID", 150);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Initial LE $", 130);
      this.gvFields.Columns.Add("Baseline", 80);
      this.gvFields.Columns.Add("Disclosed $", 100);
      this.gvFields.Columns.Add("Itemization $", 100);
      this.gvFields.Columns.Add("Variance $", 100);
      this.gvFields.Columns.Add("Variance Limit", 100);
    }

    private GVItem createGVItem(GFFVAlertTriggerField field)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Text = field.FieldId;
      gvItem.SubItems[1].Text = field.Description;
      gvItem.SubItems[2].Text = field.InitialLEValue;
      gvItem.SubItems[3].Text = field.Baseline;
      gvItem.SubItems[4].Text = field.DisclosedValue;
      gvItem.SubItems[5].Text = field.ItemizationValue;
      gvItem.SubItems[6].Text = field.VarianceValue;
      gvItem.SubItems[7].Text = field.VarianceLimit;
      gvItem.ForeColor = EncompassColors.Alert2;
      if (field.VarianceValue != "")
      {
        for (int nItemIndex = 0; nItemIndex < gvItem.SubItems.Count; ++nItemIndex)
          gvItem.SubItems[nItemIndex].Font = new Font(this.lblAlertDate.Font, FontStyle.Bold);
      }
      return gvItem;
    }

    public string GetHelpTargetName() => nameof (GoodFaithFeeVarianceCureLogWS);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcGFFVC = new GroupContainer();
      this.gcResolution = new GroupContainer();
      this.txtComments = new TextBox();
      this.label4 = new Label();
      this.txtResolvedBy = new TextBox();
      this.label3 = new Label();
      this.txtDate = new TextBox();
      this.label2 = new Label();
      this.txtAppliedCureAmount = new TextBox();
      this.label1 = new Label();
      this.grpFields = new GroupContainer();
      this.gvFields = new GridView();
      this.grpDetails = new GroupContainer();
      this.txtTotalVariance = new TextBox();
      this.label5 = new Label();
      this.txtAlertDate = new TextBox();
      this.lblAlertDate = new Label();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.txtAlertName = new TextBox();
      this.lblAlertName = new Label();
      this.flpControls = new FlowLayoutPanel();
      this.gcGFFVC.SuspendLayout();
      this.gcResolution.SuspendLayout();
      this.grpFields.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gcGFFVC.Controls.Add((Control) this.gcResolution);
      this.gcGFFVC.Controls.Add((Control) this.grpFields);
      this.gcGFFVC.Controls.Add((Control) this.grpDetails);
      this.gcGFFVC.Dock = DockStyle.Fill;
      this.gcGFFVC.HeaderForeColor = SystemColors.ControlText;
      this.gcGFFVC.Location = new Point(0, 0);
      this.gcGFFVC.Name = "gcGFFVC";
      this.gcGFFVC.Size = new Size(1025, 632);
      this.gcGFFVC.TabIndex = 0;
      this.gcGFFVC.Text = "Good Faith Fee Variance Cured";
      this.gcResolution.Controls.Add((Control) this.txtComments);
      this.gcResolution.Controls.Add((Control) this.label4);
      this.gcResolution.Controls.Add((Control) this.txtResolvedBy);
      this.gcResolution.Controls.Add((Control) this.label3);
      this.gcResolution.Controls.Add((Control) this.txtDate);
      this.gcResolution.Controls.Add((Control) this.label2);
      this.gcResolution.Controls.Add((Control) this.txtAppliedCureAmount);
      this.gcResolution.Controls.Add((Control) this.label1);
      this.gcResolution.Dock = DockStyle.Top;
      this.gcResolution.HeaderForeColor = SystemColors.ControlText;
      this.gcResolution.Location = new Point(1, 26);
      this.gcResolution.Name = "gcResolution";
      this.gcResolution.Size = new Size(1023, 157);
      this.gcResolution.TabIndex = 4;
      this.gcResolution.Text = "Resolution";
      this.txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComments.Location = new Point(409, 36);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ReadOnly = true;
      this.txtComments.ScrollBars = ScrollBars.Vertical;
      this.txtComments.Size = new Size(509, 104);
      this.txtComments.TabIndex = 9;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(340, 40);
      this.label4.Name = "label4";
      this.label4.Size = new Size(56, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Comments";
      this.txtResolvedBy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtResolvedBy.Enabled = false;
      this.txtResolvedBy.Location = new Point(125, 80);
      this.txtResolvedBy.Name = "txtResolvedBy";
      this.txtResolvedBy.ReadOnly = true;
      this.txtResolvedBy.Size = new Size(149, 20);
      this.txtResolvedBy.TabIndex = 7;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 84);
      this.label3.Name = "label3";
      this.label3.Size = new Size(67, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Resolved By";
      this.txtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDate.Enabled = false;
      this.txtDate.Location = new Point(125, 58);
      this.txtDate.Name = "txtDate";
      this.txtDate.ReadOnly = true;
      this.txtDate.Size = new Size(149, 20);
      this.txtDate.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 62);
      this.label2.Name = "label2";
      this.label2.Size = new Size(30, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Date";
      this.txtAppliedCureAmount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAppliedCureAmount.Enabled = false;
      this.txtAppliedCureAmount.Location = new Point(125, 36);
      this.txtAppliedCureAmount.Name = "txtAppliedCureAmount";
      this.txtAppliedCureAmount.ReadOnly = true;
      this.txtAppliedCureAmount.Size = new Size(149, 20);
      this.txtAppliedCureAmount.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(106, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Applied Cure Amount";
      this.grpFields.Controls.Add((Control) this.gvFields);
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(1, 309);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(1023, 319);
      this.grpFields.TabIndex = 3;
      this.grpFields.Text = "Trigger Fields";
      this.gvFields.AllowMultiselect = false;
      this.gvFields.BorderStyle = BorderStyle.None;
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFields.Location = new Point(1, 26);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(1021, 292);
      this.gvFields.TabIndex = 2;
      this.grpDetails.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpDetails.Controls.Add((Control) this.txtTotalVariance);
      this.grpDetails.Controls.Add((Control) this.label5);
      this.grpDetails.Controls.Add((Control) this.txtAlertDate);
      this.grpDetails.Controls.Add((Control) this.lblAlertDate);
      this.grpDetails.Controls.Add((Control) this.txtDescription);
      this.grpDetails.Controls.Add((Control) this.lblDescription);
      this.grpDetails.Controls.Add((Control) this.txtAlertName);
      this.grpDetails.Controls.Add((Control) this.lblAlertName);
      this.grpDetails.Controls.Add((Control) this.flpControls);
      this.grpDetails.ForeColor = SystemColors.ControlText;
      this.grpDetails.HeaderForeColor = Color.Black;
      this.grpDetails.Location = new Point(1, 182);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(1023, 129);
      this.grpDetails.TabIndex = 2;
      this.grpDetails.Text = "Alert Details";
      this.txtTotalVariance.Enabled = false;
      this.txtTotalVariance.Location = new Point(345, 96);
      this.txtTotalVariance.Name = "txtTotalVariance";
      this.txtTotalVariance.ReadOnly = true;
      this.txtTotalVariance.Size = new Size(129, 20);
      this.txtTotalVariance.TabIndex = 10;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(263, 100);
      this.label5.Name = "label5";
      this.label5.Size = new Size(76, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Total Variance";
      this.txtAlertDate.Enabled = false;
      this.txtAlertDate.Location = new Point(75, 96);
      this.txtAlertDate.Name = "txtAlertDate";
      this.txtAlertDate.ReadOnly = true;
      this.txtAlertDate.Size = new Size(137, 20);
      this.txtAlertDate.TabIndex = 6;
      this.lblAlertDate.AutoSize = true;
      this.lblAlertDate.Location = new Point(7, 100);
      this.lblAlertDate.Name = "lblAlertDate";
      this.lblAlertDate.Size = new Size(54, 13);
      this.lblAlertDate.TabIndex = 5;
      this.lblAlertDate.Text = "Alert Date";
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Enabled = false;
      this.txtDescription.Location = new Point(75, 58);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(938, 36);
      this.txtDescription.TabIndex = 4;
      this.txtDescription.Text = "Good Faith Fee Variance limit is violated. Correct fees or address the fee variance at closing or within 30 calendar days after settlement.";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(7, 62);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Description";
      this.txtAlertName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAlertName.Enabled = false;
      this.txtAlertName.Location = new Point(75, 36);
      this.txtAlertName.Name = "txtAlertName";
      this.txtAlertName.ReadOnly = true;
      this.txtAlertName.Size = new Size(580, 20);
      this.txtAlertName.TabIndex = 2;
      this.lblAlertName.AutoSize = true;
      this.lblAlertName.Location = new Point(7, 40);
      this.lblAlertName.Name = "lblAlertName";
      this.lblAlertName.Size = new Size(59, 13);
      this.lblAlertName.TabIndex = 1;
      this.lblAlertName.Text = "Alert Name";
      this.flpControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpControls.BackColor = Color.Transparent;
      this.flpControls.FlowDirection = FlowDirection.RightToLeft;
      this.flpControls.Location = new Point(160, 2);
      this.flpControls.Name = "flpControls";
      this.flpControls.Size = new Size(859, 22);
      this.flpControls.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcGFFVC);
      this.Name = nameof (GoodFaithFeeVarianceCureLogWS);
      this.Size = new Size(1025, 632);
      this.gcGFFVC.ResumeLayout(false);
      this.gcResolution.ResumeLayout(false);
      this.gcResolution.PerformLayout();
      this.grpFields.ResumeLayout(false);
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
