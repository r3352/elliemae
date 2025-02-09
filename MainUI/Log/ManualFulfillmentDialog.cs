// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ManualFulfillmentDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ManualFulfillmentDialog : Form
  {
    private IDisclosureTrackingLog _disclosureLog;
    private string eDisclosureManuallyFulfilledBy = "";
    private DateTime eDisclosureManualFulfillmentDate;
    private DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod;
    private string eDisclosureManualFulfillmentComment = "";
    private DateTime eDisclosurePresumedDate;
    private DateTime eDisclosureActualDate;
    private IContainer components;
    private Label label1;
    private TextBox txtFulfilledBy;
    private Label label2;
    private Label label3;
    private TextBox txtComments;
    private Label label4;
    private Button btnSubmit;
    private Button btnCancel;
    private ComboBox cboFulfillmentMethod;
    private DateTimePicker dtpDateTimeFulfilled;
    private ErrorProvider errorProvider1;
    private DatePicker dpActualFulfillmentDate;
    private Label label18;
    private Label label27;
    private DatePicker dpPresumedFulfillmentDate;
    private Panel panel2015;

    public ManualFulfillmentDialog(IDisclosureTrackingLog disclosureLog)
    {
      this.InitializeComponent();
      this._disclosureLog = disclosureLog;
      this.txtFulfilledBy.Text = Session.UserInfo.FullName;
      this.cboFulfillmentMethod.Items.Add((object) this.DisclosureMethodToString(DisclosureTrackingBase.DisclosedMethod.ByMail));
      this.cboFulfillmentMethod.Items.Add((object) this.DisclosureMethodToString(DisclosureTrackingBase.DisclosedMethod.InPerson));
      this.cboFulfillmentMethod.SelectedIndex = 0;
      switch (disclosureLog)
      {
        case DisclosureTrackingLog _:
          this.panel2015.Visible = false;
          this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - 52);
          this.dtpDateTimeFulfilled.Value = ((DisclosureTrackingBase) disclosureLog).ConvertToLoanTimeZone(DateTime.Now);
          break;
        case DisclosureTracking2015Log _:
          this.dtpDateTimeFulfilled.Value = ((DisclosureTrackingBase) disclosureLog).ConvertToLoanTimeZone(DateTime.Now);
          break;
        default:
          this.dtpDateTimeFulfilled.Value = DateTime.Now;
          break;
      }
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      if (this._disclosureLog is DisclosureTrackingLog)
      {
        this._disclosureLog.eDisclosureManuallyFulfilledBy = this.txtFulfilledBy.Text.Trim();
        this._disclosureLog.eDisclosureManualFulfillmentDate = this.dtpDateTimeFulfilled.Value;
        this._disclosureLog.eDisclosureManualFulfillmentMethod = this.StringToDisclosureMethod(this.cboFulfillmentMethod.Text);
        this._disclosureLog.eDisclosureManualFulfillmentComment = this.txtComments.Text.Trim();
      }
      else
      {
        this.eDisclosureManuallyFulfilledBy = this.txtFulfilledBy.Text.Trim();
        this.eDisclosureManualFulfillmentDate = this.dtpDateTimeFulfilled.Value;
        this.eDisclosureManualFulfillmentMethod = this.StringToDisclosureMethod(this.cboFulfillmentMethod.Text);
        this.eDisclosureManualFulfillmentComment = this.txtComments.Text.Trim();
        this.eDisclosurePresumedDate = this.dpPresumedFulfillmentDate.Value;
        this.eDisclosureActualDate = this.dpActualFulfillmentDate.Value;
      }
      this.Close();
    }

    private DisclosureTrackingBase.DisclosedMethod StringToDisclosureMethod(string methodString)
    {
      switch (methodString)
      {
        case "U.S. Mail":
          return DisclosureTrackingBase.DisclosedMethod.ByMail;
        case "In Person":
          return DisclosureTrackingBase.DisclosedMethod.InPerson;
        default:
          return DisclosureTrackingBase.DisclosedMethod.Other;
      }
    }

    private string DisclosureMethodToString(DisclosureTrackingBase.DisclosedMethod method)
    {
      if (method == DisclosureTrackingBase.DisclosedMethod.ByMail)
        return "U.S. Mail";
      return method == DisclosureTrackingBase.DisclosedMethod.InPerson ? "In Person" : "";
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
    }

    private void txtFulfilledBy_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.txtFulfilledBy.Text.Trim() == ""))
        return;
      this.errorProvider1.SetError((Control) this.txtFulfilledBy, "This is a required field.");
      e.Cancel = true;
    }

    private void txtFulfilledBy_Validated(object sender, EventArgs e)
    {
      this.errorProvider1.SetError((Control) this.txtFulfilledBy, "");
    }

    public string EDisclosureManuallyFulfilledBy => this.eDisclosureManuallyFulfilledBy;

    public DateTime EDisclosureManualFulfillmentDate => this.eDisclosureManualFulfillmentDate;

    public DisclosureTrackingBase.DisclosedMethod EDisclosureManualFulfillmentMethod
    {
      get => this.eDisclosureManualFulfillmentMethod;
    }

    public string EDisclosureManualFulfillmentComment => this.eDisclosureManualFulfillmentComment;

    public DateTime EDisclosurePresumedDate => this.eDisclosurePresumedDate;

    public DateTime EDisclosureActualDate => this.eDisclosureActualDate;

    private void dtpDateTimeFulfilled_ValueChanged(object sender, EventArgs e)
    {
      BusinessCalendar businessCalendar = Session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      if (!(this.dtpDateTimeFulfilled.Value != DateTime.MinValue))
        return;
      this.dpPresumedFulfillmentDate.Value = businessCalendar.AddBusinessDays(this.dtpDateTimeFulfilled.Value, 3, true);
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
      this.label1 = new Label();
      this.txtFulfilledBy = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtComments = new TextBox();
      this.label4 = new Label();
      this.btnSubmit = new Button();
      this.btnCancel = new Button();
      this.cboFulfillmentMethod = new ComboBox();
      this.dtpDateTimeFulfilled = new DateTimePicker();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.dpActualFulfillmentDate = new DatePicker();
      this.label18 = new Label();
      this.label27 = new Label();
      this.dpPresumedFulfillmentDate = new DatePicker();
      this.panel2015 = new Panel();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.panel2015.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(64, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "* Fulfilled By";
      this.errorProvider1.SetIconAlignment((Control) this.txtFulfilledBy, ErrorIconAlignment.MiddleLeft);
      this.txtFulfilledBy.Location = new Point(163, 12);
      this.txtFulfilledBy.Name = "txtFulfilledBy";
      this.txtFulfilledBy.Size = new Size(230, 20);
      this.txtFulfilledBy.TabIndex = 1;
      this.txtFulfilledBy.Validating += new CancelEventHandler(this.txtFulfilledBy_Validating);
      this.txtFulfilledBy.Validated += new EventHandler(this.txtFulfilledBy_Validated);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 43);
      this.label2.Name = "label2";
      this.label2.Size = new Size(103, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "* Date/Time Fulfilled";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 69);
      this.label3.Name = "label3";
      this.label3.Size = new Size(99, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "* Fulfillment Method";
      this.txtComments.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.txtComments.Location = new Point(163, 147);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(230, 59);
      this.txtComments.TabIndex = 7;
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(19, 150);
      this.label4.Name = "label4";
      this.label4.Size = new Size(56, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Comments";
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.DialogResult = DialogResult.OK;
      this.btnSubmit.Location = new Point(235, 221);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(75, 23);
      this.btnSubmit.TabIndex = 8;
      this.btnSubmit.Text = "Submit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(316, 221);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cboFulfillmentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFulfillmentMethod.FormattingEnabled = true;
      this.cboFulfillmentMethod.Location = new Point(163, 66);
      this.cboFulfillmentMethod.Name = "cboFulfillmentMethod";
      this.cboFulfillmentMethod.Size = new Size(230, 21);
      this.cboFulfillmentMethod.TabIndex = 10;
      this.dtpDateTimeFulfilled.CustomFormat = "M/d/yyyy hh:mm tt";
      this.dtpDateTimeFulfilled.Format = DateTimePickerFormat.Custom;
      this.dtpDateTimeFulfilled.Location = new Point(163, 39);
      this.dtpDateTimeFulfilled.Name = "dtpDateTimeFulfilled";
      this.dtpDateTimeFulfilled.Size = new Size(230, 20);
      this.dtpDateTimeFulfilled.TabIndex = 22;
      this.dtpDateTimeFulfilled.ValueChanged += new EventHandler(this.dtpDateTimeFulfilled_ValueChanged);
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      this.dpActualFulfillmentDate.BackColor = SystemColors.Window;
      this.dpActualFulfillmentDate.Location = new Point(147, 33);
      this.dpActualFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpActualFulfillmentDate.Name = "dpActualFulfillmentDate";
      this.dpActualFulfillmentDate.Size = new Size(230, 21);
      this.dpActualFulfillmentDate.TabIndex = 72;
      this.dpActualFulfillmentDate.Tag = (object) "763";
      this.dpActualFulfillmentDate.ToolTip = "";
      this.dpActualFulfillmentDate.Value = new DateTime(0L);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(3, 37);
      this.label18.Name = "label18";
      this.label18.Size = new Size(112, 13);
      this.label18.TabIndex = 74;
      this.label18.Text = "Actual Received Date";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(3, 10);
      this.label27.Name = "label27";
      this.label27.Size = new Size(129, 13);
      this.label27.TabIndex = 73;
      this.label27.Text = "Presumed Received Date";
      this.dpPresumedFulfillmentDate.BackColor = SystemColors.Window;
      this.dpPresumedFulfillmentDate.Location = new Point(147, 6);
      this.dpPresumedFulfillmentDate.MaxValue = new DateTime(2029, 12, 31, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.MinValue = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.dpPresumedFulfillmentDate.Name = "dpPresumedFulfillmentDate";
      this.dpPresumedFulfillmentDate.ReadOnly = true;
      this.dpPresumedFulfillmentDate.Size = new Size(230, 21);
      this.dpPresumedFulfillmentDate.TabIndex = 71;
      this.dpPresumedFulfillmentDate.Tag = (object) "763";
      this.dpPresumedFulfillmentDate.ToolTip = "";
      this.dpPresumedFulfillmentDate.Value = new DateTime(0L);
      this.panel2015.Controls.Add((Control) this.label27);
      this.panel2015.Controls.Add((Control) this.dpActualFulfillmentDate);
      this.panel2015.Controls.Add((Control) this.dpPresumedFulfillmentDate);
      this.panel2015.Controls.Add((Control) this.label18);
      this.panel2015.Location = new Point(15, 87);
      this.panel2015.Name = "panel2015";
      this.panel2015.Size = new Size(381, 56);
      this.panel2015.TabIndex = 75;
      this.AcceptButton = (IButtonControl) this.btnSubmit;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(404, 257);
      this.Controls.Add((Control) this.panel2015);
      this.Controls.Add((Control) this.dtpDateTimeFulfilled);
      this.Controls.Add((Control) this.cboFulfillmentMethod);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSubmit);
      this.Controls.Add((Control) this.txtComments);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtFulfilledBy);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ManualFulfillmentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Manually Fulfill";
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.panel2015.ResumeLayout(false);
      this.panel2015.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
