// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.GoodFaithFeeVarianceViolationAlertFeeDetailPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class GoodFaithFeeVarianceViolationAlertFeeDetailPanel : UserControl
  {
    private GoodFaithFeeVarianceViolationAlertPanel goodFaithAlertPanel;
    private LoanData loan;
    private int selectedRecordIndex = -1;
    private string selectedAlertField;
    private string selectedDescription = "";
    private bool isNewLEFlow = true;
    private string[] Reasons = new string[13]
    {
      "Changed Circumstance - Settlement Charges",
      "Changed Circumstance - Eligibility",
      "Revisions requested by the Consumer",
      "Interest Rate dependent charges (Rate Lock)",
      "Expiration (Intent to Proceed received after 10 business days)",
      "Delayed Settlement on Construction Loans",
      "Change in APR",
      "Change in Loan Product",
      "Prepayment Penalty Added",
      "24-hour Advanced Preview",
      "Tolerance Cure",
      "Clerical Error Correction",
      "Other"
    };
    private string[] leCocReason = new string[7]
    {
      "Changed Circumstance - Settlement Charges",
      "Changed Circumstance - Eligibility",
      "Revisions requested by the Consumer",
      "Interest Rate dependent charges (Rate Lock)",
      "Expiration (Intent to Proceed received after 10 business days)",
      "Delayed Settlement on Construction Loans",
      "Other"
    };
    private string[] cdCocReason = new string[11]
    {
      "Change in APR",
      "Change in Loan Product",
      "Prepayment Penalty Added",
      "Changed Circumstance - Settlement Charges",
      "Changed Circumstance - Eligibility",
      "Revisions requested by the Consumer",
      "Interest Rate dependent charges (Rate Lock)",
      "24-hour Advanced Preview",
      "Tolerance Cure",
      "Clerical Error Correction",
      "Other"
    };
    private IStatusDisplay statusDisplay;
    private bool forCD;
    private bool useFeeDisclosureLevelIndicator = true;
    private IContainer components;
    private Panel panelFeeDetail;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelStandartAlertUI;
    private ComboBox cboReason;
    private Label label6;
    private TextBox txtComments;
    private Label label5;
    private DatePicker dateRevisedDate;
    private DatePicker dateChangedCircumstance;
    private TextBox txtDescription;
    private Label label4;
    private Label labelRevisedDueDate;
    private Label label2;
    private GroupContainer groupContainerFee;
    private ToolTip toolTip1;
    private StandardIconButton btnSelect;
    private TextBox cboReasonOther;
    private Label applyToLabel;
    private ComboBox applyLECDCombo;
    private FlowLayoutPanel flowLayoutPanel2;

    public GoodFaithFeeVarianceViolationAlertFeeDetailPanel(PipelineInfo.Alert alert)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.goodFaithAlertPanel = new GoodFaithFeeVarianceViolationAlertPanel(alert);
      this.goodFaithAlertPanel.FieldSelectedIndexChanged += new EventHandler(this.goodFaithAlertPanel_FieldSelectedIndexChanged);
      this.panelStandartAlertUI.Controls.Add((Control) this.goodFaithAlertPanel);
      this.loan = Session.LoanDataMgr.LoanData;
      string field = this.loan.GetField("4462");
      IDisclosureTracking2015Log idisclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.All);
      this.forCD = !(field == "") || idisclosureTracking2015Log == null ? field == "CD" : idisclosureTracking2015Log.DisclosedForCD;
      if (!this.loan.Use2015RESPA || !Session.StartupInfo.EnableCoC || this.loan.GetField("4461") != "Y")
      {
        this.useFeeDisclosureLevelIndicator = false;
        this.setControls(false);
      }
      else
      {
        this.loan.Calculator.GetGFFVarianceAlertDetails();
        this.isNewLEFlow = this.loan.Calculator.UseNewCompliance(19.2M);
        if (this.isNewLEFlow)
          this.cdCocReason = new string[5]
          {
            "Changed Circumstance - Settlement Charges",
            "Changed Circumstance - Eligibility",
            "Revisions requested by the Consumer",
            "Interest Rate dependent charges (Rate Lock)",
            "Other"
          };
        this.cboReason.Items.Add((object) "");
        this.cboReason.Items.AddRange(this.forCD ? (object[]) this.cdCocReason : (object[]) this.leCocReason);
        this.applyLECDCombo.SelectedIndexChanged -= new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
        this.applyLECDCombo.SelectedItem = this.forCD ? (object) "CD" : (object) "LE";
        if (this.goodFaithAlertPanel.fieldCount > 1)
          this.goodFaithAlertPanel.SelectFirstItem();
        this.applyLECDCombo.SelectedIndexChanged += new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
      }
    }

    private void goodFaithAlertPanel_FieldSelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.useFeeDisclosureLevelIndicator)
        return;
      GFFVAlertTriggerField alertTriggerField1 = (GFFVAlertTriggerField) null;
      string fieldID = (string) null;
      string description = (string) null;
      switch (sender)
      {
        case GFFVAlertTriggerField _:
          GFFVAlertTriggerField alertTriggerField2 = (GFFVAlertTriggerField) sender;
          fieldID = alertTriggerField2.FieldId;
          description = alertTriggerField2.Description;
          break;
        case AlertTriggerField _:
          fieldID = ((AlertTriggerField) sender).FieldID;
          description = alertTriggerField1.Description;
          break;
        case string _:
          fieldID = (string) sender;
          break;
      }
      this.populateFeeDetails(fieldID, description);
    }

    protected void setControls(bool enable)
    {
      if (!enable)
      {
        this.cboReason.SelectedIndexChanged -= new EventHandler(this.cboReason_SelectedIndexChanged);
        this.txtDescription.TextChanged -= new EventHandler(this.feeField_TextChanged);
        this.txtComments.TextChanged -= new EventHandler(this.feeField_TextChanged);
        this.dateChangedCircumstance.ValueChanged -= new EventHandler(this.dateField_ValueChanged);
        this.cboReasonOther.TextChanged -= new EventHandler(this.feeField_TextChanged);
      }
      this.dateChangedCircumstance.Visible = this.txtDescription.Visible = this.txtComments.Visible = this.cboReason.Visible = this.btnSelect.Visible = this.cboReasonOther.Visible = this.labelRevisedDueDate.Visible = this.label2.Visible = this.label4.Visible = this.label5.Visible = this.label6.Visible = this.dateRevisedDate.Visible = enable;
      if (!enable)
      {
        this.dateChangedCircumstance.Text = this.txtDescription.Text = this.txtComments.Text = this.cboReasonOther.Text = this.dateRevisedDate.Text = "";
        this.cboReason.SelectedIndex = -1;
        this.cboReason.SelectedIndexChanged += new EventHandler(this.cboReason_SelectedIndexChanged);
        this.txtDescription.TextChanged += new EventHandler(this.feeField_TextChanged);
        this.txtComments.TextChanged += new EventHandler(this.feeField_TextChanged);
        this.dateChangedCircumstance.ValueChanged += new EventHandler(this.dateField_ValueChanged);
        this.cboReasonOther.TextChanged += new EventHandler(this.feeField_TextChanged);
      }
      this.panelFeeDetail.Visible = enable;
    }

    private void populateFeeDetails(string fieldID, string description)
    {
      if (description != null)
      {
        if (description.Contains("Cannot Decrease") || description.Contains("Cannot Increase") || description.Contains("10% Variance"))
        {
          this.setControls(false);
          this.selectedAlertField = "";
          this.selectedDescription = description;
          return;
        }
        this.setControls(true);
      }
      else
        this.setControls(true);
      Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
      if ((fieldID ?? "") != "")
      {
        this.selectedRecordIndex = this.loan.GetGoodFaithChangeOfCircumstanceRecordIndex(fieldID);
        this.selectedAlertField = fieldID;
        this.selectedDescription = description;
      }
      else
      {
        this.selectedRecordIndex = -1;
        this.selectedAlertField = (string) null;
        this.selectedDescription = description;
      }
      this.cboReason.SelectedIndexChanged -= new EventHandler(this.cboReason_SelectedIndexChanged);
      this.txtDescription.TextChanged -= new EventHandler(this.feeField_TextChanged);
      this.txtComments.TextChanged -= new EventHandler(this.feeField_TextChanged);
      this.labelRevisedDueDate.Text = "Revised " + (this.forCD ? "CD" : "LE") + " Due Date";
      if (this.selectedRecordIndex > 0)
      {
        string str = "XCOC" + this.selectedRecordIndex.ToString("00");
        DateTime dateTime = Utils.ParseDate((object) this.loan.GetField(str + "03"));
        if (dateTime == DateTime.MinValue)
          dateTime = DateTime.Today;
        this.dateChangedCircumstance.Value = dateTime;
        if (Utils.ParseDate((object) this.loan.GetField(str + "04")) == DateTime.MinValue)
          this.loan.TriggerCalculation(str + "03", this.loan.GetField(str + "03"));
        this.dateRevisedDate.Value = Utils.ParseDate((object) this.loan.GetField(str + "04"));
        this.txtDescription.Text = this.loan.GetField(str + "05");
        this.txtComments.Text = this.loan.GetField(str + "06");
        this.loan.GetField(str + "06");
        this.cboReason.SelectedItem = (object) this.loan.GetField(str + "07");
        this.cboReasonOther.Text = this.loan.GetField(str + "08");
      }
      else
      {
        this.dateChangedCircumstance.Value = DateTime.MinValue;
        this.dateRevisedDate.Value = DateTime.MinValue;
        this.txtDescription.Text = "";
        this.txtComments.Text = "";
        this.cboReason.SelectedItem = (object) null;
      }
      if (this.selectedRecordIndex < 0)
      {
        this.dateChangedCircumstance.ReadOnly = this.dateRevisedDate.ReadOnly = this.txtComments.ReadOnly = dictionary == null;
        this.cboReason.Enabled = dictionary != null;
        this.setToolTips((string) null);
      }
      else
      {
        this.dateChangedCircumstance.ReadOnly = this.dateRevisedDate.ReadOnly = this.txtComments.ReadOnly = false;
        this.cboReason.Enabled = true;
        this.cboReasonOther.Visible = (string) this.cboReason.SelectedItem == "Other";
        this.setToolTips("XCOC" + this.selectedRecordIndex.ToString("00"));
      }
      this.txtDescription.TextChanged += new EventHandler(this.feeField_TextChanged);
      this.txtComments.TextChanged += new EventHandler(this.feeField_TextChanged);
      this.cboReason.SelectedIndexChanged += new EventHandler(this.cboReason_SelectedIndexChanged);
    }

    private void setToolTips(string id)
    {
      this.dateChangedCircumstance.Tag = id != null ? (object) (id + "03") : (object) (string) null;
      this.dateRevisedDate.Tag = id != null ? (object) (id + "04") : (object) (string) null;
      this.txtDescription.Tag = id != null ? (object) (id + "05") : (object) (string) null;
      this.txtComments.Tag = id != null ? (object) (id + "06") : (object) (string) null;
      this.cboReason.Tag = id != null ? (object) (id + "07") : (object) (string) null;
      this.dateChangedCircumstance.ToolTip = this.dateChangedCircumstance.Tag != null ? this.dateChangedCircumstance.Tag.ToString() : "";
      this.dateRevisedDate.ToolTip = this.dateRevisedDate.Tag != null ? this.dateRevisedDate.Tag.ToString() : "";
      this.toolTip1.SetToolTip((Control) this.txtDescription, this.txtDescription.Tag != null ? this.txtDescription.Tag.ToString() : "");
      this.toolTip1.SetToolTip((Control) this.txtComments, this.txtComments.Tag != null ? this.txtComments.Tag.ToString() : "");
      this.toolTip1.SetToolTip((Control) this.cboReason, this.cboReason.Tag != null ? this.cboReason.Tag.ToString() : "");
    }

    private void cboReason_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((string) this.cboReason.SelectedItem == "Other")
      {
        this.cboReasonOther.Visible = true;
        this.setField("08", this.cboReasonOther.Text);
      }
      else
      {
        this.cboReasonOther.Visible = false;
        this.cboReasonOther.Text = "";
        this.setField("08", "");
      }
      this.setField("07", this.cboReason.SelectedItem != null ? this.cboReason.SelectedItem.ToString() : "");
      this.loan.Locked_COC_Fields.Clear();
      this.mapToCDLEPage1();
    }

    private void dateField_ValueChanged(object sender, EventArgs e)
    {
      DatePicker datePicker = (DatePicker) sender;
      if (datePicker == null)
        return;
      if (datePicker.Name == "dateChangedCircumstance" && datePicker.Value > DateTime.MinValue)
      {
        this.setField("03", datePicker.Value > DateTime.MinValue ? datePicker.Value.ToString("MM/dd/yyyy") : "");
        this.dateRevisedDate.Value = Utils.ParseDate((object) this.loan.GetField("XCOC" + this.selectedRecordIndex.ToString("00") + "04"));
      }
      else if (datePicker.Name == "dateRevisedDate" && datePicker.Value > DateTime.MinValue)
        this.setField("04", datePicker.Value > DateTime.MinValue ? datePicker.Value.ToString("MM/dd/yyyy") : "");
      this.loan.Locked_COC_Fields.Clear();
      this.mapToCDLEPage1();
    }

    private void mapToCDLEPage1() => this.loan.Calculator.CopyAlertCoCToLECDPage1(this.forCD);

    private void feeField_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return;
      if (textBox.Name == "txtDescription")
        this.setField("05", textBox.Text.Trim());
      else if (textBox.Name == "txtComments")
        this.setField("06", textBox.Text.Trim());
      else if (textBox.Name == "cboReasonOther")
        this.setField("08", textBox.Text.Trim());
      this.loan.Locked_COC_Fields.Clear();
      this.mapToCDLEPage1();
    }

    private void setField(string id, string val)
    {
      if (this.selectedAlertField == "")
        return;
      if ((this.selectedAlertField ?? "") != "" && this.loan.GetGoodFaithChangeOfCircumstanceRecordIndex(this.selectedAlertField) == -1)
        this.selectedRecordIndex = this.loan.NewGoodFaithChangeOfCircumstance(this.selectedAlertField) + 1;
      if (this.selectedRecordIndex == -1)
        return;
      if (!id.StartsWith("XCOC"))
        id = "XCOC" + this.selectedRecordIndex.ToString("00") + id;
      this.loan.SetField(id, val.Trim());
    }

    private void field_Enter(object sender, EventArgs e)
    {
      if (this.statusDisplay == null)
        this.statusDisplay = Session.Application.GetService<IStatusDisplay>();
      if (this.statusDisplay == null)
        return;
      Control control = (Control) sender;
      this.statusDisplay.DisplayFieldID(control.Tag != null ? control.Tag.ToString() : "");
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (ChangeCircumstanceSelector circumstanceSelector = new ChangeCircumstanceSelector(false, this.applyLECDCombo.SelectedItem.ToString(), this.loan.GetField("4461") == "Y"))
      {
        if (circumstanceSelector.ShowDialog() != DialogResult.OK)
          return;
        this.cboReason.SelectedIndexChanged -= new EventHandler(this.cboReason_SelectedIndexChanged);
        this.txtDescription.TextChanged -= new EventHandler(this.feeField_TextChanged);
        this.txtComments.TextChanged -= new EventHandler(this.feeField_TextChanged);
        try
        {
          this.txtDescription.Text = circumstanceSelector.OptionValue;
          this.setField("05", this.txtDescription.Text);
          this.txtComments.Text = circumstanceSelector.OptionComment;
          this.setField("06", this.txtComments.Text);
          this.cboReason.SelectedIndex = 0;
          int int16 = (int) Convert.ToInt16(circumstanceSelector.OptionReason);
          if (int16 >= 0 && int16 <= this.Reasons.Length)
            this.cboReason.SelectedItem = (object) this.Reasons[int16 - 1];
          this.setField("07", this.cboReason.SelectedItem != null ? this.cboReason.SelectedItem.ToString() : "");
          if ((string) this.cboReason.SelectedItem != "Other")
          {
            if (this.cboReasonOther.Text != "")
              this.setField("08", "");
            this.cboReasonOther.Text = "";
            this.cboReasonOther.Visible = false;
          }
          else
          {
            this.cboReasonOther.Text = "";
            this.cboReasonOther.Visible = true;
          }
        }
        catch
        {
        }
        this.cboReason.SelectedIndexChanged += new EventHandler(this.cboReason_SelectedIndexChanged);
        this.txtDescription.TextChanged += new EventHandler(this.feeField_TextChanged);
        this.txtComments.TextChanged += new EventHandler(this.feeField_TextChanged);
        this.loan.Locked_COC_Fields.Clear();
        this.mapToCDLEPage1();
      }
    }

    private void clearInvalidReasons()
    {
      int changeOfCircumstance = this.loan.GetNumberOfGoodFaithChangeOfCircumstance();
      for (int index = 0; index <= changeOfCircumstance; ++index)
      {
        string field = this.loan.GetField("XCOC" + index.ToString("00") + "07");
        if (field != "" && Array.IndexOf<string>(this.forCD ? this.cdCocReason : this.leCocReason, field) <= -1)
        {
          this.loan.SetField("XCOC" + index.ToString("00") + "05", "");
          this.loan.SetField("XCOC" + index.ToString("00") + "06", "");
          this.loan.SetField("XCOC" + index.ToString("00") + "07", "");
          this.loan.SetField("XCOC" + index.ToString("00") + "08", "");
        }
      }
    }

    private void applyLECDCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((string) this.applyLECDCombo.SelectedItem == "CD")
      {
        if (DialogResult.OK != Utils.Dialog((IWin32Window) this, "Data for any reasons that are only associated with an LE will be cleared. Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand))
        {
          this.applyLECDCombo.SelectedIndexChanged -= new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
          this.applyLECDCombo.SelectedItem = (object) "LE";
          this.applyLECDCombo.SelectedIndexChanged += new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
          return;
        }
        this.forCD = true;
        this.loan.SetField("4462", "CD");
        this.dateRevisedDate.Text = this.loan.GetField("CD1.X63");
        this.labelRevisedDueDate.Text = "Revised " + (this.forCD ? "CD" : "LE") + " Due Date";
      }
      else
      {
        if (DialogResult.OK != Utils.Dialog((IWin32Window) this, "Data for any reasons that are only associated with a CD will be cleared. Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand))
        {
          this.applyLECDCombo.SelectedIndexChanged -= new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
          this.applyLECDCombo.SelectedItem = (object) "CD";
          this.applyLECDCombo.SelectedIndexChanged += new EventHandler(this.applyLECDCombo_SelectedIndexChanged);
          return;
        }
        this.forCD = false;
        this.loan.SetField("4462", "LE");
        this.dateRevisedDate.Text = this.loan.GetField("3167");
        this.labelRevisedDueDate.Text = "Revised " + (this.forCD ? "CD" : "LE") + " Due Date";
      }
      this.cboReason.Items.Clear();
      this.cboReason.Items.Add((object) "");
      this.cboReason.Items.AddRange(this.forCD ? (object[]) this.cdCocReason : (object[]) this.leCocReason);
      this.clearInvalidReasons();
      this.populateFeeDetails(this.selectedAlertField, this.selectedDescription);
      this.loan.Locked_COC_Fields.Clear();
      this.mapToCDLEPage1();
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
      this.panelFeeDetail = new Panel();
      this.groupContainerFee = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.applyLECDCombo = new ComboBox();
      this.applyToLabel = new Label();
      this.cboReasonOther = new TextBox();
      this.btnSelect = new StandardIconButton();
      this.cboReason = new ComboBox();
      this.label6 = new Label();
      this.label2 = new Label();
      this.txtComments = new TextBox();
      this.labelRevisedDueDate = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.dateRevisedDate = new DatePicker();
      this.txtDescription = new TextBox();
      this.dateChangedCircumstance = new DatePicker();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelStandartAlertUI = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.panelFeeDetail.SuspendLayout();
      this.groupContainerFee.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.SuspendLayout();
      this.panelFeeDetail.Controls.Add((Control) this.groupContainerFee);
      this.panelFeeDetail.Dock = DockStyle.Bottom;
      this.panelFeeDetail.Location = new Point(0, 282);
      this.panelFeeDetail.Name = "panelFeeDetail";
      this.panelFeeDetail.Size = new Size(1182, 279);
      this.panelFeeDetail.TabIndex = 0;
      this.groupContainerFee.Controls.Add((Control) this.flowLayoutPanel2);
      this.groupContainerFee.Controls.Add((Control) this.cboReasonOther);
      this.groupContainerFee.Controls.Add((Control) this.btnSelect);
      this.groupContainerFee.Controls.Add((Control) this.cboReason);
      this.groupContainerFee.Controls.Add((Control) this.label6);
      this.groupContainerFee.Controls.Add((Control) this.label2);
      this.groupContainerFee.Controls.Add((Control) this.txtComments);
      this.groupContainerFee.Controls.Add((Control) this.labelRevisedDueDate);
      this.groupContainerFee.Controls.Add((Control) this.label5);
      this.groupContainerFee.Controls.Add((Control) this.label4);
      this.groupContainerFee.Controls.Add((Control) this.dateRevisedDate);
      this.groupContainerFee.Controls.Add((Control) this.txtDescription);
      this.groupContainerFee.Controls.Add((Control) this.dateChangedCircumstance);
      this.groupContainerFee.Dock = DockStyle.Fill;
      this.groupContainerFee.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerFee.Location = new Point(0, 0);
      this.groupContainerFee.Name = "groupContainerFee";
      this.groupContainerFee.Size = new Size(1182, 279);
      this.groupContainerFee.TabIndex = 12;
      this.groupContainerFee.Text = "Fee Details";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.applyLECDCombo);
      this.flowLayoutPanel2.Controls.Add((Control) this.applyToLabel);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(588, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(594, 26);
      this.flowLayoutPanel2.TabIndex = 42;
      this.applyLECDCombo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.applyLECDCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.applyLECDCombo.FormattingEnabled = true;
      this.applyLECDCombo.Items.AddRange(new object[2]
      {
        (object) "LE",
        (object) "CD"
      });
      this.applyLECDCombo.Location = new Point(541, 3);
      this.applyLECDCombo.Name = "applyLECDCombo";
      this.applyLECDCombo.Size = new Size(50, 21);
      this.applyLECDCombo.TabIndex = 40;
      this.applyToLabel.Anchor = AnchorStyles.None;
      this.applyToLabel.AutoSize = true;
      this.applyToLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.applyToLabel.Location = new Point(478, 7);
      this.applyToLabel.Name = "applyToLabel";
      this.applyToLabel.Size = new Size(57, 13);
      this.applyToLabel.TabIndex = 41;
      this.applyToLabel.Text = "Applies To";
      this.cboReasonOther.Location = new Point(168, 214);
      this.cboReasonOther.Name = "cboReasonOther";
      this.cboReasonOther.Size = new Size(462, 20);
      this.cboReasonOther.TabIndex = 39;
      this.cboReasonOther.Visible = false;
      this.cboReasonOther.TextChanged += new EventHandler(this.feeField_TextChanged);
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(636, 86);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 38;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.cboReason.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboReason.FormattingEnabled = true;
      this.cboReason.Location = new Point(168, 187);
      this.cboReason.Name = "cboReason";
      this.cboReason.Size = new Size(462, 21);
      this.cboReason.TabIndex = 6;
      this.cboReason.Tag = (object) "07";
      this.cboReason.Enter += new EventHandler(this.field_Enter);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(14, 190);
      this.label6.Name = "label6";
      this.label6.Size = new Size(44, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Reason";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(14, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(124, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Changes Received Date";
      this.txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComments.Location = new Point(723, 86);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(437, 95);
      this.txtComments.TabIndex = 5;
      this.txtComments.Tag = (object) "06";
      this.txtComments.Enter += new EventHandler(this.field_Enter);
      this.labelRevisedDueDate.AutoSize = true;
      this.labelRevisedDueDate.Location = new Point(14, 61);
      this.labelRevisedDueDate.Name = "labelRevisedDueDate";
      this.labelRevisedDueDate.Size = new Size(111, 13);
      this.labelRevisedDueDate.TabIndex = 3;
      this.labelRevisedDueDate.Text = "Revised LE Due Date";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(661, 89);
      this.label5.Name = "label5";
      this.label5.Size = new Size(56, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Comments";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(14, 89);
      this.label4.Name = "label4";
      this.label4.Size = new Size(60, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Description";
      this.dateRevisedDate.BackColor = SystemColors.Window;
      this.dateRevisedDate.Enabled = false;
      this.dateRevisedDate.Location = new Point(168, 61);
      this.dateRevisedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dateRevisedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dateRevisedDate.Name = "dateRevisedDate";
      this.dateRevisedDate.Size = new Size(106, 21);
      this.dateRevisedDate.TabIndex = 3;
      this.dateRevisedDate.Tag = (object) "04";
      this.dateRevisedDate.ToolTip = "";
      this.dateRevisedDate.Value = new DateTime(0L);
      this.dateRevisedDate.ValueChanged += new EventHandler(this.dateField_ValueChanged);
      this.dateRevisedDate.Enter += new EventHandler(this.field_Enter);
      this.txtDescription.Location = new Point(168, 86);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(462, 95);
      this.txtDescription.TabIndex = 4;
      this.txtDescription.Tag = (object) "05";
      this.txtDescription.Enter += new EventHandler(this.field_Enter);
      this.dateChangedCircumstance.BackColor = SystemColors.Window;
      this.dateChangedCircumstance.Location = new Point(168, 38);
      this.dateChangedCircumstance.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dateChangedCircumstance.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dateChangedCircumstance.Name = "dateChangedCircumstance";
      this.dateChangedCircumstance.Size = new Size(106, 21);
      this.dateChangedCircumstance.TabIndex = 2;
      this.dateChangedCircumstance.Tag = (object) "03";
      this.dateChangedCircumstance.ToolTip = "";
      this.dateChangedCircumstance.Value = new DateTime(0L);
      this.dateChangedCircumstance.ValueChanged += new EventHandler(this.dateField_ValueChanged);
      this.dateChangedCircumstance.Enter += new EventHandler(this.field_Enter);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelFeeDetail;
      this.collapsibleSplitter1.Cursor = Cursors.SizeNS;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 279);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 1;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelStandartAlertUI.Dock = DockStyle.Fill;
      this.panelStandartAlertUI.Location = new Point(0, 0);
      this.panelStandartAlertUI.Name = "panelStandartAlertUI";
      this.panelStandartAlertUI.Size = new Size(1182, 279);
      this.panelStandartAlertUI.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelStandartAlertUI);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelFeeDetail);
      this.Name = nameof (GoodFaithFeeVarianceViolationAlertFeeDetailPanel);
      this.Size = new Size(1182, 561);
      this.panelFeeDetail.ResumeLayout(false);
      this.groupContainerFee.ResumeLayout(false);
      this.groupContainerFee.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.ResumeLayout(false);
    }
  }
}
