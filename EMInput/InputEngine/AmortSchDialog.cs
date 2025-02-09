// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AmortSchDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AmortSchDialog : Form
  {
    private IContainer components;
    private Button okBtn;
    private RadioButton yearlyRadio;
    private RadioButton completeRadio;
    private PaymentSchedule[] paySchedule;
    private PaymentSchedule[] payFHASchedule;
    private EMHelpLink emHelpLink1;
    private LoanData loan;
    private Label label1;
    private GridView amortListview;
    private bool debugMode;
    private bool useWorstCase;
    private int selected;
    private bool forUSDA;
    private GVColumn column11;
    private RadioButton rdoBestCase;
    private RadioButton rdoWorstCase;
    private Panel panelWorstBest;
    private GVColumn column12;
    private PaymentScheduleSnapshot bestPaySnapshot;
    private PaymentSchedule[] bestPaySchedule;
    private PaymentSchedule[] worstPaySchedule;
    private Button btnExport;
    private ToolTip toolTip1;
    private Panel panelTerminationDate;
    private Label label2;
    private FieldLockButton fieldLockButton1;
    private Label label3;
    private TextBox textBox2;
    private TextBox textBox3;
    private Label label4;
    private DatePicker datePicker1;
    private GenericFormInputHandler formInputHandler;

    public event EventHandler OnExportClicked;

    public AmortSchDialog(LoanData loan)
      : this(loan, (PaymentScheduleSnapshot) null, false)
    {
    }

    public AmortSchDialog(LoanData loan, PaymentScheduleSnapshot paySnapshot, bool useWorstCase)
      : this(loan, paySnapshot, useWorstCase, (PaymentScheduleSnapshot) null)
    {
    }

    public AmortSchDialog(
      LoanData loan,
      PaymentScheduleSnapshot paySnapshot,
      bool useWorstCase,
      PaymentScheduleSnapshot bestPaySnapshot)
    {
      this.loan = loan;
      this.useWorstCase = useWorstCase;
      this.bestPaySnapshot = bestPaySnapshot;
      bool flag = false;
      if (this.loan != null && this.loan.GetField("CALCREQUIRED") == "Y")
        flag = true;
      this.InitializeComponent();
      if (paySnapshot == null)
        paySnapshot = ((LoanCalculator) loan.Calculator).GetPaymentSchedule(false);
      else if (this.bestPaySnapshot != null)
      {
        this.Text += " - Worst/Best Case Scenario";
        this.bestPaySchedule = this.bestPaySnapshot.MonthlyPayments;
      }
      else
        this.Text += " - Worst Case Scenario";
      this.formInputHandler = new GenericFormInputHandler((IHtmlInput) this.loan, this.Controls, Session.DefaultInstance);
      this.formInputHandler.SetFieldValuesToForm();
      this.formInputHandler.SetBusinessRules(new ResourceManager(typeof (AmortSchDialog)));
      this.formInputHandler.SetLockIconStatus();
      this.formInputHandler.SetFieldTip(this.toolTip1);
      for (int index = 0; index < this.formInputHandler.FieldControls.Count; ++index)
        this.formInputHandler.SetFieldEvents(this.formInputHandler.FieldControls[index]);
      this.formInputHandler.OnLockClicked += new EventHandler(this.lockButton_Clicked);
      if (!this.useWorstCase || this.bestPaySnapshot == null)
      {
        this.panelWorstBest.Visible = false;
        this.panelTerminationDate.Visible = true;
      }
      else
      {
        this.amortListview.Top -= 51;
        this.amortListview.Size = new Size(641, 437);
        this.panelTerminationDate.Visible = false;
      }
      if (this.bestPaySnapshot != null)
        this.worstPaySchedule = paySnapshot.MonthlyPayments;
      if (paySnapshot != null)
        this.paySchedule = paySnapshot.MonthlyPayments;
      if (this.loan.GetField("1172") == "FHA" && this.loan.GetField("1678") != "Y")
        this.debugMode = true;
      else if (this.loan.GetField("1172") == "FarmersHomeAdministration")
        this.forUSDA = true;
      if ((!this.debugMode || this.useWorstCase) && !this.forUSDA)
      {
        this.amortListview.Columns.Remove(this.amortListview.Columns[12]);
        this.amortListview.Columns.Remove(this.amortListview.Columns[11]);
        this.amortListview.Columns.Remove(this.amortListview.Columns[10]);
        this.amortListview.Columns.Remove(this.amortListview.Columns[9]);
      }
      else
      {
        if (this.debugMode && !this.useWorstCase)
          this.amortListview.Columns.Remove(this.amortListview.Columns[8]);
        this.payFHASchedule = ((LoanCalculator) loan.Calculator).GetFHAPaymentSchedule();
      }
      if (this.forUSDA)
      {
        this.amortListview.Columns[9].Text = "Ave. Annual UPB";
        this.amortListview.Columns[10].Text = "Annual Fee";
        this.amortListview.Columns[11].Text = "Monthly Amount";
        this.amortListview.Columns[12].Text = "Monthly Payment";
        this.Width = this.Width + this.amortListview.Columns[9].Width + this.amortListview.Columns[10].Width + this.amortListview.Columns[11].Width + this.amortListview.Columns[12].Width;
        this.column11 = this.amortListview.Columns[11];
        this.column12 = this.amortListview.Columns[12];
      }
      this.buildCompleteList();
      if (!flag)
        return;
      Session.Application.GetService<ILoanEditor>().RefreshContents();
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
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.okBtn = new Button();
      this.yearlyRadio = new RadioButton();
      this.completeRadio = new RadioButton();
      this.label1 = new Label();
      this.amortListview = new GridView();
      this.rdoBestCase = new RadioButton();
      this.rdoWorstCase = new RadioButton();
      this.panelWorstBest = new Panel();
      this.btnExport = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.toolTip1 = new ToolTip(this.components);
      this.panelTerminationDate = new Panel();
      this.datePicker1 = new DatePicker();
      this.label4 = new Label();
      this.textBox3 = new TextBox();
      this.textBox2 = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.fieldLockButton1 = new FieldLockButton();
      this.panelWorstBest.SuspendLayout();
      this.panelTerminationDate.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(577, 483);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 35;
      this.okBtn.Text = "&Close";
      this.okBtn.Click += new EventHandler(this.btn_Close_Click);
      this.yearlyRadio.Location = new Point(226, 8);
      this.yearlyRadio.Name = "yearlyRadio";
      this.yearlyRadio.Size = new Size(110, 21);
      this.yearlyRadio.TabIndex = 40;
      this.yearlyRadio.Text = "Yearly Schedule";
      this.yearlyRadio.Click += new EventHandler(this.ReportTypeClick);
      this.completeRadio.Checked = true;
      this.completeRadio.Location = new Point(92, 8);
      this.completeRadio.Name = "completeRadio";
      this.completeRadio.Size = new Size(138, 21);
      this.completeRadio.TabIndex = 39;
      this.completeRadio.TabStop = true;
      this.completeRadio.Text = "Complete Schedule";
      this.completeRadio.Click += new EventHandler(this.ReportTypeClick);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 13);
      this.label1.TabIndex = 41;
      this.label1.Text = "Type of Report:";
      this.amortListview.AllowMultiselect = false;
      this.amortListview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pmt#";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pmt Date";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Rate";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 55;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Payment";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 85;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Principal";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 90;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Interest";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 85;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "MI";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 70;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Balance";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "gvRemainingLTV";
      gvColumn13.Text = "Threshold / LTV%";
      gvColumn13.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn13.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "gvFHAPayment";
      gvColumn9.Text = "FHA Payment";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "gvFHAPrincipal";
      gvColumn10.Text = "FHA Principal";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "gvFHABalance";
      gvColumn11.Text = "FHA Balance";
      gvColumn11.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "gvThreshold";
      gvColumn12.Text = "Threshold / LTV%";
      gvColumn12.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn12.Width = 100;
      this.amortListview.Columns.AddRange(new GVColumn[13]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn13,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12
      });
      this.amortListview.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.amortListview.Location = new Point(8, 83);
      this.amortListview.Name = "amortListview";
      this.amortListview.Size = new Size(641, 386);
      this.amortListview.SortIconVisible = false;
      this.amortListview.SortOption = GVSortOption.None;
      this.amortListview.TabIndex = 42;
      this.rdoBestCase.Location = new Point(83, 0);
      this.rdoBestCase.Name = "rdoBestCase";
      this.rdoBestCase.Size = new Size(79, 21);
      this.rdoBestCase.TabIndex = 44;
      this.rdoBestCase.Text = "Best Case";
      this.rdoBestCase.CheckedChanged += new EventHandler(this.rdoBestCase_CheckedChanged);
      this.rdoWorstCase.Checked = true;
      this.rdoWorstCase.Location = new Point(0, 0);
      this.rdoWorstCase.Name = "rdoWorstCase";
      this.rdoWorstCase.Size = new Size(87, 21);
      this.rdoWorstCase.TabIndex = 43;
      this.rdoWorstCase.TabStop = true;
      this.rdoWorstCase.Text = "Worst Case";
      this.rdoWorstCase.Click += new EventHandler(this.rdoWorstCase_Click);
      this.panelWorstBest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.panelWorstBest.Controls.Add((Control) this.rdoBestCase);
      this.panelWorstBest.Controls.Add((Control) this.rdoWorstCase);
      this.panelWorstBest.Location = new Point(498, 8);
      this.panelWorstBest.Name = "panelWorstBest";
      this.panelWorstBest.Size = new Size(160, 21);
      this.panelWorstBest.TabIndex = 45;
      this.btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnExport.Location = new Point(496, 483);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 46;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Visible = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Amortization Schedule";
      this.emHelpLink1.Location = new Point(11, 486);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 40;
      this.panelTerminationDate.Controls.Add((Control) this.datePicker1);
      this.panelTerminationDate.Controls.Add((Control) this.label4);
      this.panelTerminationDate.Controls.Add((Control) this.textBox3);
      this.panelTerminationDate.Controls.Add((Control) this.textBox2);
      this.panelTerminationDate.Controls.Add((Control) this.label3);
      this.panelTerminationDate.Controls.Add((Control) this.label2);
      this.panelTerminationDate.Controls.Add((Control) this.fieldLockButton1);
      this.panelTerminationDate.Location = new Point(4, 29);
      this.panelTerminationDate.Name = "panelTerminationDate";
      this.panelTerminationDate.Size = new Size(641, 51);
      this.panelTerminationDate.TabIndex = 57;
      this.datePicker1.BackColor = SystemColors.Window;
      this.datePicker1.Location = new Point(465, 5);
      this.datePicker1.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datePicker1.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePicker1.Name = "datePicker1";
      this.datePicker1.Size = new Size(101, 21);
      this.datePicker1.TabIndex = 63;
      this.datePicker1.Tag = (object) "CORRESPONDENT.X475";
      this.datePicker1.ToolTip = "";
      this.datePicker1.Value = new DateTime(0L);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 31);
      this.label4.Name = "label4";
      this.label4.Size = new Size(132, 13);
      this.label4.TabIndex = 62;
      this.label4.Text = "MI Termination Date (80%)";
      this.textBox3.Location = new Point(137, 28);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(91, 20);
      this.textBox3.TabIndex = 61;
      this.textBox3.Tag = (object) "109";
      this.textBox2.Location = new Point(137, 5);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new Size(91, 20);
      this.textBox2.TabIndex = 60;
      this.textBox2.Tag = (object) "118";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 7);
      this.label3.Name = "label3";
      this.label3.Size = new Size(132, 13);
      this.label3.TabIndex = 59;
      this.label3.Text = "MI Termination Date (78%)";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(242, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(203, 13);
      this.label2.TabIndex = 58;
      this.label2.Text = "Date of First Payment Without Monthly MI";
      this.fieldLockButton1.Location = new Point(446, 7);
      this.fieldLockButton1.MaximumSize = new Size(16, 16);
      this.fieldLockButton1.MinimumSize = new Size(16, 16);
      this.fieldLockButton1.Name = "fieldLockButton1";
      this.fieldLockButton1.Size = new Size(16, 16);
      this.fieldLockButton1.TabIndex = 57;
      this.fieldLockButton1.Tag = (object) "CORRESPONDENT.X475";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.okBtn;
      this.ClientSize = new Size(658, 514);
      this.Controls.Add((Control) this.panelTerminationDate);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.panelWorstBest);
      this.Controls.Add((Control) this.amortListview);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.yearlyRadio);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.completeRadio);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (AmortSchDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Amortization Schedule";
      this.VisibleChanged += new EventHandler(this.AmortSchDialog_VisibleChanged);
      this.KeyUp += new KeyEventHandler(this.AmortSchDialog_KeyUp);
      this.panelWorstBest.ResumeLayout(false);
      this.panelTerminationDate.ResumeLayout(false);
      this.panelTerminationDate.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void buildCompleteList()
    {
      int num1 = 12;
      if (this.loan.GetField("423") == "Biweekly")
        num1 = 26;
      this.amortListview.Columns[0].Text = "Pmt#";
      if (this.amortListview.Columns.Count <= 11 && this.forUSDA)
      {
        this.amortListview.Columns.Add(this.column11);
        this.amortListview.Columns.Add(this.column12);
      }
      this.amortListview.Items.Clear();
      this.amortListview.BeginUpdate();
      double num2 = 0.0;
      this.selected = 0;
      bool flag = false;
      if (this.loan.GetField("608") == "AdjustableRate")
        flag = true;
      int num3 = this.paySchedule.Length - 1;
      int num4 = Utils.ParseInt((object) this.loan.GetField("1177"));
      int num5 = Utils.ParseInt((object) this.loan.GetField("325"));
      int num6 = Utils.ParseInt((object) this.loan.GetField("4"));
      for (int index = 0; index < num3; ++index)
      {
        int num7 = index + 1;
        if (num7 % num1 == 1 && num7 != 1)
          this.amortListview.Items.Add(new GVItem(string.Empty)
          {
            SubItems = {
              (object) string.Empty
            }
          });
        GVItem gvItem = new GVItem(num7.ToString());
        gvItem.SubItems.Add((object) this.paySchedule[index].PayDate);
        double num8 = this.paySchedule[index].CurrentRate;
        string str1 = num8.ToString("N3");
        gvItem.SubItems.Add((object) str1);
        num8 = this.paySchedule[index].Payment;
        string str2 = num8.ToString("#,0.00");
        gvItem.SubItems.Add((object) str2);
        num8 = this.paySchedule[index].Principal;
        string str3 = num8.ToString("N2");
        gvItem.SubItems.Add((object) str3);
        num8 = this.paySchedule[index].Interest;
        string str4 = num8.ToString("N2");
        gvItem.SubItems.Add((object) str4);
        num8 = this.paySchedule[index].MortgageInsurance;
        string str5 = num8.ToString("#,0.00");
        gvItem.SubItems.Add((object) str5);
        num8 = this.paySchedule[index].Balance;
        string str6 = num8.ToString("#,0.00");
        gvItem.SubItems.Add((object) str6);
        if (!this.debugMode || this.useWorstCase)
        {
          num8 = this.paySchedule[index].RemainingLTV;
          string str7 = num8.ToString("N3");
          gvItem.SubItems.Add((object) str7);
        }
        if (this.forUSDA)
        {
          if (num7 % num1 == 1 && this.paySchedule[index] != null)
          {
            GVSubItemCollection subItems1 = gvItem.SubItems;
            num8 = this.paySchedule[index].USDAAnnualUPB;
            string str8 = num8.ToString("#,0.00");
            subItems1.Add((object) str8);
            GVSubItemCollection subItems2 = gvItem.SubItems;
            num8 = this.paySchedule[index].USDAAnnualFee;
            string str9 = num8.ToString("#,0.00");
            subItems2.Add((object) str9);
            GVSubItemCollection subItems3 = gvItem.SubItems;
            num8 = this.paySchedule[index].USDAMonthlyFee;
            string str10 = num8.ToString("#,0.00");
            subItems3.Add((object) str10);
            double num9 = this.paySchedule[index].Principal + this.paySchedule[index].Interest + this.paySchedule[index].USDAMonthlyFee;
            gvItem.SubItems.Add((object) num9.ToString("#,0.00"));
          }
          else
          {
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
          }
        }
        else if (this.debugMode && !this.useWorstCase)
        {
          string str11;
          if (this.payFHASchedule[index] == null)
          {
            str11 = "";
          }
          else
          {
            num8 = this.payFHASchedule[index].Payment;
            str11 = num8.ToString("#,0.00");
          }
          string str12 = str11;
          gvItem.SubItems.Add((object) str12);
          string str13;
          if (this.payFHASchedule[index] == null)
          {
            str13 = "";
          }
          else
          {
            num8 = this.payFHASchedule[index].Principal;
            str13 = num8.ToString("N2");
          }
          string str14 = str13;
          gvItem.SubItems.Add((object) str14);
          string str15;
          if (this.payFHASchedule[index] == null)
          {
            str15 = "";
          }
          else
          {
            num8 = this.payFHASchedule[index].Balance;
            str15 = num8.ToString("#,0.00");
          }
          string str16 = str15;
          gvItem.SubItems.Add((object) str16);
          string str17;
          if (this.payFHASchedule[index] == null)
          {
            str17 = "";
          }
          else
          {
            num8 = this.payFHASchedule[index].RemainingLTV;
            str17 = num8.ToString("N3");
          }
          string str18 = str17;
          gvItem.SubItems.Add((object) str18);
        }
        this.amortListview.Items.Add(gvItem);
        if (this.useWorstCase && index < this.paySchedule.Length - 2)
        {
          if (flag)
          {
            if (num2 < this.paySchedule[index].CurrentRate)
            {
              num2 = this.paySchedule[index].CurrentRate;
              this.selected = this.amortListview.Items.Count - 1;
            }
          }
          else if (num2 < Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2))
          {
            num2 = Utils.ArithmeticRounding(this.paySchedule[index].Principal + this.paySchedule[index].Interest, 2);
            this.selected = this.amortListview.Items.Count - 1;
          }
        }
      }
      if (num4 > 0 && (num4 >= num5 && num5 > 0 || num4 >= num6 && num6 > 0))
        this.selected = this.amortListview.Items.Count - 1;
      this.amortListview.EndUpdate();
    }

    private void lockButton_Clicked(object sender, EventArgs e)
    {
      FieldLockButton fieldLockButton = (FieldLockButton) sender;
      if (fieldLockButton.Tag == null)
        return;
      string id = fieldLockButton.Tag.ToString();
      if (id == string.Empty || fieldLockButton.Locked)
        return;
      this.datePicker1.Text = this.loan.GetFieldFromCal(id);
    }

    private void buildYearlyList()
    {
      string empty = string.Empty;
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double num8 = 0.0;
      this.amortListview.Columns[0].Text = "Year";
      int num9 = 12;
      if (this.loan.GetField("423") == "Biweekly")
        num9 = 26;
      int num10 = 0;
      int num11 = this.paySchedule.Length - 1;
      Utils.ParseInt((object) this.loan.GetField("1177"));
      Utils.ParseInt((object) this.loan.GetField("325"));
      Utils.ParseInt((object) this.loan.GetField("4"));
      this.amortListview.BeginUpdate();
      this.amortListview.Items.Clear();
      for (int index = 0; index < num11; ++index)
      {
        int num12 = index + 1;
        if (num12 % num9 == 1)
        {
          num1 = this.paySchedule[index].Payment;
          num2 = this.paySchedule[index].Principal;
          num3 = this.paySchedule[index].Interest;
          num4 = this.paySchedule[index].MortgageInsurance;
          if (this.debugMode && !this.useWorstCase)
          {
            num5 = this.payFHASchedule[index].Payment;
            num6 = this.payFHASchedule[index].Principal;
          }
          if (this.forUSDA)
          {
            num7 = this.paySchedule[index].USDAAnnualFee;
            num8 = this.paySchedule[index].Principal + this.paySchedule[index].Interest + this.paySchedule[index].USDAAnnualFee;
          }
        }
        else
        {
          num1 += this.paySchedule[index].Payment;
          num2 += this.paySchedule[index].Principal;
          num3 += this.paySchedule[index].Interest;
          num4 += this.paySchedule[index].MortgageInsurance;
          if (this.debugMode && !this.useWorstCase)
          {
            num5 += this.payFHASchedule[index].Payment;
            num6 += this.payFHASchedule[index].Principal;
          }
          if (this.forUSDA)
            num8 += this.paySchedule[index].Principal + this.paySchedule[index].Interest;
        }
        if (num12 % num9 == 0 || index == num11 - 1)
        {
          ++num10;
          GVItem gvItem = new GVItem(num10.ToString());
          gvItem.SubItems.Add((object) this.paySchedule[index].PayDate);
          double num13 = this.paySchedule[index].CurrentRate;
          string str1 = num13.ToString("N3");
          gvItem.SubItems.Add((object) str1);
          string str2 = num1.ToString("#,0.00");
          gvItem.SubItems.Add((object) str2);
          string str3 = num2.ToString("N2");
          gvItem.SubItems.Add((object) str3);
          string str4 = num3.ToString("N2");
          gvItem.SubItems.Add((object) str4);
          string str5 = num4.ToString("#,0.00");
          gvItem.SubItems.Add((object) str5);
          num13 = this.paySchedule[index].Balance;
          string str6 = num13.ToString("#,0.00");
          gvItem.SubItems.Add((object) str6);
          if (!this.debugMode || this.useWorstCase)
          {
            num13 = this.paySchedule[index].RemainingLTV;
            string str7 = num13.ToString("N3");
            gvItem.SubItems.Add((object) str7);
          }
          if (this.forUSDA)
          {
            GVSubItemCollection subItems1 = gvItem.SubItems;
            string str8;
            if (index < 11 || this.paySchedule[index - 11] == null)
            {
              str8 = "";
            }
            else
            {
              num13 = this.paySchedule[index - 11].USDAAnnualUPB;
              str8 = num13.ToString("#,0.00");
            }
            subItems1.Add((object) str8);
            GVSubItemCollection subItems2 = gvItem.SubItems;
            string str9;
            if (index < 11 || this.paySchedule[index - 11] == null)
            {
              str9 = "";
            }
            else
            {
              num13 = this.paySchedule[index - 11].USDAAnnualFee;
              str9 = num13.ToString("#,0.00");
            }
            subItems2.Add((object) str9);
            gvItem.SubItems.Add((object) num7.ToString("#,0.00"));
            gvItem.SubItems.Add((object) num8.ToString("#,0.00"));
          }
          else if (this.debugMode && !this.useWorstCase)
          {
            string str10 = num5.ToString("#,0.00");
            gvItem.SubItems.Add((object) str10);
            string str11 = num6.ToString("N2");
            gvItem.SubItems.Add((object) str11);
            num13 = this.payFHASchedule[index].Balance;
            string str12 = num13.ToString("#,0.00");
            gvItem.SubItems.Add((object) str12);
            num13 = this.payFHASchedule[index].RemainingLTV;
            string str13 = num13.ToString("N3");
            gvItem.SubItems.Add((object) str13);
          }
          this.amortListview.Items.Add(gvItem);
        }
      }
      if (this.forUSDA)
      {
        if (this.amortListview.Columns.Count > 11)
          this.amortListview.Columns.RemoveAt(11);
        if (this.amortListview.Columns.Count > 11)
          this.amortListview.Columns.RemoveAt(11);
      }
      this.amortListview.EndUpdate();
    }

    private void ReportTypeClick(object sender, EventArgs e)
    {
      if (this.yearlyRadio.Checked)
        this.buildYearlyList();
      else
        this.buildCompleteList();
    }

    private void AmortSchDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Amortization Schedule");
    }

    private void AmortSchDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.amortListview.Items.Count == 0 || !this.useWorstCase || this.selected >= this.amortListview.Items.Count)
        return;
      this.amortListview.BeginUpdate();
      this.amortListview.MoveToTop(this.selected);
      this.amortListview.Items[this.selected].Selected = true;
      this.amortListview.EnsureVisible(this.selected);
      this.amortListview.EndUpdate();
    }

    private void rdoBestCase_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoBestCase.Checked)
        this.paySchedule = this.bestPaySchedule;
      if (this.yearlyRadio.Checked)
        this.buildYearlyList();
      else
        this.buildCompleteList();
    }

    private void rdoWorstCase_Click(object sender, EventArgs e)
    {
      if (this.rdoWorstCase.Checked)
        this.paySchedule = this.worstPaySchedule;
      if (this.yearlyRadio.Checked)
        this.buildYearlyList();
      else
        this.buildCompleteList();
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (this.OnExportClicked == null)
        return;
      this.OnExportClicked((object) this.amortListview, e);
    }

    private void btn_Close_Click(object sender, EventArgs e)
    {
      if (this.fieldLockButton1.Locked)
        this.loan.AddLock("CORRESPONDENT.X475");
      this.formInputHandler.SetFieldValuesToLoan();
      this.DialogResult = DialogResult.OK;
    }
  }
}
