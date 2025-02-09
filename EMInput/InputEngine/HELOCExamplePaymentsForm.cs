// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HELOCExamplePaymentsForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HELOCExamplePaymentsForm : Form
  {
    private List<List<string[]>> helocPayments;
    private IContainer components;
    private Button btnExport;
    private Panel panelWorstBest;
    private RadioButton radioMaximum;
    private RadioButton rdoMinimum;
    private RadioButton rdoHistorical;
    private RadioButton radioMaximum_fullLoanAmt;
    private GridView lvwSchedule;
    private Button okBtn;
    private CheckBox chkAlternateSchedule;

    public HELOCExamplePaymentsForm(List<List<string[]>> helocPayments, string fHHIX4)
    {
      this.helocPayments = helocPayments;
      this.InitializeComponent();
      if (this.helocPayments != null && this.helocPayments.Count > 3 && this.helocPayments[3] != null)
        this.radioMaximum_fullLoanAmt.Enabled = true;
      else
        this.radioMaximum_fullLoanAmt.Enabled = false;
      this.chkAlternateSchedule.Checked = fHHIX4 == "Y";
      this.scheduleOption_Changed((object) this.rdoHistorical, (EventArgs) null);
    }

    private void scheduleOption_Changed(object sender, EventArgs e)
    {
      RadioButton radioButton = (RadioButton) sender;
      if (!radioButton.Checked)
        return;
      if (radioButton.Name == "rdoHistorical")
        this.loadSchedule(this.helocPayments == null || this.helocPayments.Count <= 0 ? (List<string[]>) null : this.helocPayments[0]);
      else if (radioButton.Name == "rdoMinimum")
        this.loadSchedule(this.helocPayments == null || this.helocPayments.Count <= 1 ? (List<string[]>) null : this.helocPayments[1]);
      else if (radioButton.Name == "radioMaximum")
        this.loadSchedule(this.helocPayments == null || this.helocPayments.Count <= 2 ? (List<string[]>) null : this.helocPayments[2]);
      else if (radioButton.Name == "radioMaximum_fullLoanAmt")
        this.loadSchedule(this.helocPayments == null || this.helocPayments.Count <= 3 ? (List<string[]>) null : this.helocPayments[3]);
      if (radioButton.Name == "rdoHistorical")
        this.chkAlternateSchedule.Visible = true;
      else
        this.chkAlternateSchedule.Visible = false;
    }

    private void loadSchedule(List<string[]> schedules)
    {
      this.lvwSchedule.Items.Clear();
      if (schedules == null)
        return;
      this.lvwSchedule.BeginUpdate();
      for (int index1 = 0; index1 < schedules.Count; ++index1)
      {
        GVItem gvItem = new GVItem(string.Concat((object) (index1 + 1)));
        for (int index2 = 0; index2 < schedules[index1].Length; ++index2)
          gvItem.SubItems.Add((object) schedules[index1][index2]);
        this.lvwSchedule.Items.Add(gvItem);
      }
      this.lvwSchedule.EndUpdate();
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.btnExport = new Button();
      this.panelWorstBest = new Panel();
      this.radioMaximum = new RadioButton();
      this.rdoMinimum = new RadioButton();
      this.radioMaximum_fullLoanAmt = new RadioButton();
      this.rdoHistorical = new RadioButton();
      this.lvwSchedule = new GridView();
      this.okBtn = new Button();
      this.chkAlternateSchedule = new CheckBox();
      this.panelWorstBest.SuspendLayout();
      this.SuspendLayout();
      this.btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnExport.Location = new Point(569, 535);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 2;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Visible = false;
      this.panelWorstBest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.panelWorstBest.Controls.Add((Control) this.radioMaximum);
      this.panelWorstBest.Controls.Add((Control) this.rdoMinimum);
      this.panelWorstBest.Controls.Add((Control) this.radioMaximum_fullLoanAmt);
      this.panelWorstBest.Controls.Add((Control) this.rdoHistorical);
      this.panelWorstBest.Location = new Point(228, 12);
      this.panelWorstBest.Name = "panelWorstBest";
      this.panelWorstBest.Size = new Size(500, 22);
      this.panelWorstBest.TabIndex = 53;
      this.radioMaximum.AutoSize = true;
      this.radioMaximum.Location = new Point(245, 3);
      this.radioMaximum.Name = "radioMaximum";
      this.radioMaximum.Size = new Size(117, 17);
      this.radioMaximum.TabIndex = 2;
      this.radioMaximum.Text = "Maximum Schedule";
      this.radioMaximum.CheckedChanged += new EventHandler(this.scheduleOption_Changed);
      this.rdoMinimum.AutoSize = true;
      this.rdoMinimum.Location = new Point(125, 3);
      this.rdoMinimum.Name = "rdoMinimum";
      this.rdoMinimum.Size = new Size(114, 17);
      this.rdoMinimum.TabIndex = 1;
      this.rdoMinimum.Text = "Minimum Schedule";
      this.rdoMinimum.CheckedChanged += new EventHandler(this.scheduleOption_Changed);
      this.radioMaximum_fullLoanAmt.AutoSize = true;
      this.radioMaximum_fullLoanAmt.Location = new Point(368, 3);
      this.radioMaximum_fullLoanAmt.Name = "radioMaximum_fullLoanAmt";
      this.radioMaximum_fullLoanAmt.Size = new Size(128, 17);
      this.radioMaximum_fullLoanAmt.TabIndex = 3;
      this.radioMaximum_fullLoanAmt.TabStop = true;
      this.radioMaximum_fullLoanAmt.Text = "Worst-Case Schedule";
      this.radioMaximum_fullLoanAmt.CheckedChanged += new EventHandler(this.scheduleOption_Changed);
      this.rdoHistorical.AutoSize = true;
      this.rdoHistorical.Checked = true;
      this.rdoHistorical.Location = new Point(3, 3);
      this.rdoHistorical.Name = "rdoHistorical";
      this.rdoHistorical.Size = new Size(116, 17);
      this.rdoHistorical.TabIndex = 0;
      this.rdoHistorical.TabStop = true;
      this.rdoHistorical.Text = "Historical Schedule";
      this.rdoHistorical.CheckedChanged += new EventHandler(this.scheduleOption_Changed);
      this.lvwSchedule.AllowMultiselect = false;
      this.lvwSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColPayNo";
      gvColumn1.Text = "Pmt#";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColPayDate";
      gvColumn2.SortMethod = GVSortMethod.Date;
      gvColumn2.Text = "Payment Date";
      gvColumn2.Width = 90;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColRate";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Rate";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 70;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColPayment";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Payment";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 90;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColPrincipal";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Principal";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 90;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColInterest";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Interest";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 90;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColBalance";
      gvColumn7.SortMethod = GVSortMethod.Numeric;
      gvColumn7.Text = "Balance";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 107;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColPeroidType";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "Period Type";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 120;
      this.lvwSchedule.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.lvwSchedule.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwSchedule.Location = new Point(13, 36);
      this.lvwSchedule.Name = "lvwSchedule";
      this.lvwSchedule.Size = new Size(709, 483);
      this.lvwSchedule.SortIconVisible = false;
      this.lvwSchedule.SortOption = GVSortOption.None;
      this.lvwSchedule.TabIndex = 0;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(650, 535);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 3;
      this.okBtn.Text = "&Close";
      this.chkAlternateSchedule.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAlternateSchedule.Enabled = false;
      this.chkAlternateSchedule.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkAlternateSchedule.Location = new Point(13, 535);
      this.chkAlternateSchedule.Name = "chkAlternateSchedule";
      this.chkAlternateSchedule.Size = new Size(134, 17);
      this.chkAlternateSchedule.TabIndex = 1;
      this.chkAlternateSchedule.Text = "Alternate Schedule";
      this.chkAlternateSchedule.UseVisualStyleBackColor = true;
      this.chkAlternateSchedule.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(737, 564);
      this.Controls.Add((Control) this.chkAlternateSchedule);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.lvwSchedule);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.panelWorstBest);
      this.MinimizeBox = false;
      this.Name = nameof (HELOCExamplePaymentsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "HELOC Example Payment Schedules";
      this.panelWorstBest.ResumeLayout(false);
      this.panelWorstBest.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
