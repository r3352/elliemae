// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AlertPanelWithDataCompletionFields
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AlertPanelWithDataCompletionFields : AlertPanelBase
  {
    private IContainer components;
    protected FlowLayoutPanel flowLayoutPanel3;
    protected Button btnGoToField2;
    protected GridView gvAdditionalFields;
    protected GroupContainer grpAdditionalFields;
    protected Label lblDataCompletionDate;
    protected Panel flpDataCompletionControls;
    protected TextBox txtDataCompletionDate;

    public AlertPanelWithDataCompletionFields()
    {
      this.InitializeComponent();
      this.flowLayoutPanel2.Location = new Point(this.Width - this.flowLayoutPanel2.Width - 5, this.flowLayoutPanel2.Location.Y);
      this.flowLayoutPanel3.Location = new Point(this.Width - this.flowLayoutPanel3.Width - 5, this.flowLayoutPanel3.Location.Y);
    }

    protected override void PopulateTriggerFields() => base.PopulateTriggerFields();

    protected override void ConfigureTriggerFieldList()
    {
      this.gvAdditionalFields.Items.Clear();
      if (!(this.AlertConfig is AlertConfigWithDataCompletionFields))
        return;
      foreach (string dataCompletionField in ((AlertConfigWithDataCompletionFields) this.AlertConfig).DataCompletionFields)
      {
        GVItem triggerFieldItem = this.CreateTriggerFieldItem(dataCompletionField);
        if (triggerFieldItem != null)
        {
          triggerFieldItem.Tag = (object) dataCompletionField;
          this.gvAdditionalFields.Items.Add(triggerFieldItem);
        }
      }
    }

    private void gvAdditionalFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnGoToField2.Enabled = this.gvAdditionalFields.SelectedItems.Count == 1;
    }

    private void btnGoToField2_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      if (this.gvAdditionalFields.SelectedItems[0].Tag is string)
        empty = this.gvAdditionalFields.SelectedItems[0].Tag.ToString();
      this.GoToField(empty);
    }

    public void SetDataCompletionDateControls(string lableText, DateTime dataCompletionDate)
    {
      this.lblDataCompletionDate.Text = lableText;
      if (dataCompletionDate != DateTime.MinValue)
        this.txtDataCompletionDate.Text = dataCompletionDate.ToString("MM/dd/yyyy");
      else
        this.txtDataCompletionDate.Text = string.Empty;
      this.AdjustControlPositions(0);
    }

    protected override void AdjustControlPositions(int width = 0)
    {
      base.AdjustControlPositions(Math.Max(width, this.lblDataCompletionDate.Width));
      this.lblDataCompletionDate.Left = this.lblAlertDate.Left - 1;
      this.txtDataCompletionDate.Left = this.txtAlertDate.Left - 1;
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
      this.gvAdditionalFields = new GridView();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.btnGoToField2 = new Button();
      this.grpAdditionalFields = new GroupContainer();
      this.lblDataCompletionDate = new Label();
      this.flpDataCompletionControls = new Panel();
      this.txtDataCompletionDate = new TextBox();
      this.grpFields.SuspendLayout();
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.grpAdditionalFields.SuspendLayout();
      this.flpDataCompletionControls.SuspendLayout();
      this.SuspendLayout();
      this.grpFields.Dock = DockStyle.Top;
      this.grpFields.Size = new Size(1005, 184);
      this.gvFields.Size = new Size(1003, 157);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Controls.Add((Control) this.flpDataCompletionControls);
      this.grpDetails.Size = new Size(1005, 146);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblAlertName, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtAlertName, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblDescription, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtMessage, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblAlertDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtAlertDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.pnlAltDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.flpDataCompletionControls, 0);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(925, 38);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(925, 20);
      this.pnlFields.Controls.Add((Control) this.grpAdditionalFields);
      this.pnlFields.Location = new Point(0, 146);
      this.pnlFields.Size = new Size(1005, 382);
      this.pnlFields.Controls.SetChildIndex((Control) this.grpFields, 0);
      this.pnlFields.Controls.SetChildIndex((Control) this.grpAdditionalFields, 0);
      this.gvAdditionalFields.AllowMultiselect = false;
      this.gvAdditionalFields.BorderStyle = BorderStyle.None;
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
      this.gvAdditionalFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvAdditionalFields.Dock = DockStyle.Fill;
      this.gvAdditionalFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAdditionalFields.Location = new Point(1, 26);
      this.gvAdditionalFields.Name = "gvAdditionalFields";
      this.gvAdditionalFields.Size = new Size(1003, 171);
      this.gvAdditionalFields.TabIndex = 2;
      this.gvAdditionalFields.SelectedIndexChanged += new EventHandler(this.gvAdditionalFields_SelectedIndexChanged);
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.btnGoToField2);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(193, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(803, 22);
      this.flowLayoutPanel3.TabIndex = 1;
      this.btnGoToField2.Enabled = false;
      this.btnGoToField2.Location = new Point(726, 0);
      this.btnGoToField2.Margin = new Padding(3, 0, 2, 0);
      this.btnGoToField2.Name = "btnGoToField2";
      this.btnGoToField2.Size = new Size(75, 22);
      this.btnGoToField2.TabIndex = 0;
      this.btnGoToField2.Text = "G&o to Field";
      this.btnGoToField2.UseVisualStyleBackColor = true;
      this.btnGoToField2.Click += new EventHandler(this.btnGoToField2_Click);
      this.grpAdditionalFields.Controls.Add((Control) this.gvAdditionalFields);
      this.grpAdditionalFields.Controls.Add((Control) this.flowLayoutPanel3);
      this.grpAdditionalFields.Dock = DockStyle.Fill;
      this.grpAdditionalFields.HeaderForeColor = SystemColors.ControlText;
      this.grpAdditionalFields.Location = new Point(0, 184);
      this.grpAdditionalFields.Name = "grpAdditionalFields";
      this.grpAdditionalFields.Size = new Size(1005, 198);
      this.grpAdditionalFields.TabIndex = 2;
      this.grpAdditionalFields.Text = "Fields Needed for Completion";
      this.lblDataCompletionDate.AutoSize = true;
      this.lblDataCompletionDate.Location = new Point(3, 4);
      this.lblDataCompletionDate.Name = "lblDataCompletionDate";
      this.lblDataCompletionDate.Size = new Size(109, 14);
      this.lblDataCompletionDate.TabIndex = 10;
      this.lblDataCompletionDate.Text = "Data Completion Date";
      this.flpDataCompletionControls.BackColor = Color.Transparent;
      this.flpDataCompletionControls.Controls.Add((Control) this.lblDataCompletionDate);
      this.flpDataCompletionControls.Controls.Add((Control) this.txtDataCompletionDate);
      this.flpDataCompletionControls.Dock = DockStyle.Bottom;
      this.flpDataCompletionControls.Location = new Point(1, 116);
      this.flpDataCompletionControls.Margin = new Padding(0);
      this.flpDataCompletionControls.Name = "flpDataCompletionControls";
      this.flpDataCompletionControls.Size = new Size(1003, 30);
      this.flpDataCompletionControls.TabIndex = 9;
      this.txtDataCompletionDate.Location = new Point(118, 1);
      this.txtDataCompletionDate.Name = "txtDataCompletionDate";
      this.txtDataCompletionDate.ReadOnly = true;
      this.txtDataCompletionDate.Size = new Size(137, 20);
      this.txtDataCompletionDate.TabIndex = 11;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoSize = true;
      this.Name = nameof (AlertPanelWithDataCompletionFields);
      this.Size = new Size(1005, 528);
      this.grpFields.ResumeLayout(false);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.pnlFields.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      this.grpAdditionalFields.ResumeLayout(false);
      this.flpDataCompletionControls.ResumeLayout(false);
      this.flpDataCompletionControls.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
