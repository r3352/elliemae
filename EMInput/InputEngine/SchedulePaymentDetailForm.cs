// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SchedulePaymentDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SchedulePaymentDetailForm : Form
  {
    private IContainer components;
    private Label label3;
    private Label label31;
    private Label label5;
    private Label label28;
    private Label label7;
    private Label label27;
    private Label label8;
    private Label label26;
    private Label label25;
    private Label label24;
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label17;
    private Button btnCancel;
    private TextBox textBoxPaymentDueDate;
    private TextBox textBoxMiscFee;
    private TextBox textBoxReceivedDate;
    private TextBox textBoxIndexRate;
    private TextBox textBoxEscrow;
    private TextBox textBoxInterestRate;
    private TextBox textBoxInterest;
    private TextBox textBoxPrincipal;
    private TextBox textBoxLatePaymentDate;
    private Label label4;
    private Label label2;
    private Label label6;
    private Label label9;
    private Label label10;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label18;
    private TextBox boxMiscReceived;
    private TextBox boxEscrowReceived;
    private TextBox boxInterestReceived;
    private TextBox boxPrincipalReceived;
    private Label label19;
    private Label label20;
    private Label label21;
    private Label label22;
    private Label label23;
    private TextBox boxLateFeeReceived;
    private TextBox boxPastDueReceived;
    private Label label29;
    private Label label30;
    private Label label32;
    private Label label33;
    private TextBox boxAddEscrowReceived;
    private TextBox boxAddPrincipalReceived;
    private Label label1;
    private Label label34;
    private TextBox textBoxUnpaid;
    private Label label35;
    private Label label36;
    private TextBox textBoxUnpaidLateFee;
    private Label label37;
    private Label label38;
    private TextBox textBoxPastDue;
    private GroupContainer groupContainer1;
    private GridView listViewTransaction;

    public SchedulePaymentDetailForm(SchedulePaymentLog payLog, LoanData loan)
    {
      this.InitializeComponent();
      this.textBoxPaymentDueDate.Text = payLog.TransactionDate.ToString("MM/dd/yyyy");
      this.textBoxLatePaymentDate.Text = payLog.LatePaymentDate.ToString("MM/dd/yyyy");
      TextBox textBoxIndexRate = this.textBoxIndexRate;
      double num1;
      string str1;
      if (!(loan?.GetField("4912") == "FiveDecimals"))
      {
        num1 = payLog.IndexRate;
        str1 = num1.ToString("N3");
      }
      else
      {
        num1 = payLog.IndexRate;
        str1 = num1.ToString("N5");
      }
      textBoxIndexRate.Text = str1;
      TextBox textBoxInterestRate = this.textBoxInterestRate;
      num1 = payLog.InterestRate;
      string str2 = num1.ToString("N3");
      textBoxInterestRate.Text = str2;
      TextBox textBoxPrincipal = this.textBoxPrincipal;
      num1 = payLog.PrincipalDue;
      string str3 = num1.ToString("N2");
      textBoxPrincipal.Text = str3;
      TextBox textBoxInterest = this.textBoxInterest;
      num1 = payLog.InterestDue;
      string str4 = num1.ToString("N2");
      textBoxInterest.Text = str4;
      TextBox textBoxEscrow = this.textBoxEscrow;
      num1 = payLog.EscrowDue;
      string str5 = num1.ToString("N2");
      textBoxEscrow.Text = str5;
      TextBox textBoxMiscFee = this.textBoxMiscFee;
      num1 = payLog.MiscFeeDue;
      string str6 = num1.ToString("N2");
      textBoxMiscFee.Text = str6;
      TextBox boxUnpaidLateFee = this.textBoxUnpaidLateFee;
      num1 = payLog.UnpaidLateFeeDue;
      string str7 = num1.ToString("N2");
      boxUnpaidLateFee.Text = str7;
      TextBox textBoxPastDue = this.textBoxPastDue;
      num1 = payLog.TotalPastDue;
      string str8 = num1.ToString("N2");
      textBoxPastDue.Text = str8;
      ServicingTransactionBase[] servicingTransactions = loan.GetServicingTransactions(true);
      if (servicingTransactions == null || servicingTransactions.Length == 0)
        return;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = 0.0;
      double num7 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      this.listViewTransaction.BeginUpdate();
      Hashtable hashtable = new Hashtable();
      DateTime dateTime;
      for (int sno = 0; sno < servicingTransactions.Length; ++sno)
      {
        if (servicingTransactions[sno] is PaymentTransactionLog)
        {
          PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactions[sno];
          dateTime = paymentTransactionLog.PaymentIndexDate;
          DateTime date1 = dateTime.Date;
          dateTime = payLog.TransactionDate;
          DateTime date2 = dateTime.Date;
          if (!(date1 != date2))
          {
            if (payLog.PaymentReceivedDate == DateTime.MinValue)
              payLog.PaymentReceivedDate = paymentTransactionLog.PaymentReceivedDate;
            hashtable.Add((object) paymentTransactionLog.TransactionGUID, (object) paymentTransactionLog);
            this.listViewTransaction.Items.Add(this.createTransactionItem(servicingTransactions[sno], sno, false));
            num2 += paymentTransactionLog.Principal;
            num3 += paymentTransactionLog.Interest;
            num4 += paymentTransactionLog.Escrow;
            num5 += paymentTransactionLog.MiscFee;
            num7 += paymentTransactionLog.LateFee;
            num8 += paymentTransactionLog.AdditionalPrincipal;
            num9 += paymentTransactionLog.AdditionalEscrow;
          }
        }
      }
      for (int sno = 0; sno < servicingTransactions.Length; ++sno)
      {
        if (servicingTransactions[sno] is PaymentReversalLog)
        {
          PaymentReversalLog paymentReversalLog = (PaymentReversalLog) servicingTransactions[sno];
          if (hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
          {
            this.listViewTransaction.Items.Add(this.createTransactionItem(servicingTransactions[sno], sno, false));
            PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) hashtable[(object) paymentReversalLog.PaymentGUID];
            num2 -= paymentTransactionLog.Principal;
            num3 -= paymentTransactionLog.Interest;
            num4 -= paymentTransactionLog.Escrow;
            num5 -= paymentTransactionLog.MiscFee;
            num7 -= paymentTransactionLog.LateFee;
            num8 -= paymentTransactionLog.AdditionalPrincipal;
            num9 -= paymentTransactionLog.AdditionalEscrow;
          }
        }
      }
      for (int sno = 0; sno < servicingTransactions.Length; ++sno)
      {
        if (servicingTransactions[sno] is LoanPurchaseLog)
        {
          LoanPurchaseLog loanPurchaseLog = (LoanPurchaseLog) servicingTransactions[sno];
          dateTime = loanPurchaseLog.PurchaseAdviceDate;
          DateTime date3 = dateTime.Date;
          dateTime = payLog.TransactionDate;
          DateTime date4 = dateTime.Date;
          if (date3 >= date4)
          {
            dateTime = loanPurchaseLog.PurchaseAdviceDate;
            DateTime date5 = dateTime.Date;
            dateTime = payLog.LatePaymentDate;
            DateTime date6 = dateTime.Date;
            if (date5 < date6)
              num2 -= loanPurchaseLog.PurchaseAmount;
          }
          this.listViewTransaction.Items.Add(this.createTransactionItem(servicingTransactions[sno], sno, false));
        }
      }
      this.listViewTransaction.EndUpdate();
      if (payLog.PaymentReceivedDate != DateTime.MinValue)
      {
        TextBox textBoxReceivedDate = this.textBoxReceivedDate;
        dateTime = payLog.PaymentReceivedDate;
        string str9 = dateTime.ToString("MM/dd/yyyy");
        textBoxReceivedDate.Text = str9;
      }
      else
        this.textBoxReceivedDate.Text = "";
      this.boxAddEscrowReceived.Text = num9.ToString("N2");
      this.boxAddPrincipalReceived.Text = num8.ToString("N2");
      this.boxEscrowReceived.Text = num4.ToString("N2");
      this.boxInterestReceived.Text = num3.ToString("N2");
      this.boxLateFeeReceived.Text = num7.ToString("N2");
      this.boxMiscReceived.Text = num5.ToString("N2");
      this.boxPastDueReceived.Text = num6.ToString("N2");
      this.boxPrincipalReceived.Text = num2.ToString("N2");
      double num10 = payLog.TotalPaymentDue - num2 - num3 - num4 - num5;
      if (num10 > 0.0)
        this.textBoxUnpaid.Text = num10.ToString("N2");
      else
        this.textBoxUnpaid.Text = "";
      PaymentScheduleSnapshot scheduleSnapshot = loan.GetPaymentScheduleSnapshot();
      for (int index = 0; index < scheduleSnapshot.MonthlyPayments.Length; ++index)
      {
        if (Utils.ParseDate((object) scheduleSnapshot.MonthlyPayments[index].PayDate) == payLog.TransactionDate)
        {
          this.groupContainer1.Text = "Schedule Payment #" + (index + 1).ToString();
          break;
        }
      }
    }

    private GVItem createTransactionItem(ServicingTransactionBase trans, int sno, bool selected)
    {
      GVItem transactionItem = new GVItem(sno.ToString());
      transactionItem.SubItems.Add((object) ServicingEnum.TransactionTypesToUI(trans.TransactionType));
      if (trans is LoanPurchaseLog)
      {
        LoanPurchaseLog loanPurchaseLog = (LoanPurchaseLog) trans;
        transactionItem.SubItems.Add((object) loanPurchaseLog.PurchaseAdviceDate.ToString("MM/dd/yyyy"));
        transactionItem.SubItems.Add((object) loanPurchaseLog.PurchaseAmount.ToString("N2"));
      }
      else
      {
        transactionItem.SubItems.Add((object) trans.CreatedDateTime.ToString("MM/dd/yyyy"));
        transactionItem.SubItems.Add((object) trans.TransactionAmount.ToString("N2"));
      }
      transactionItem.Selected = selected;
      transactionItem.Tag = (object) trans;
      return transactionItem;
    }

    private void listViewTransaction_DoubleClick(object sender, EventArgs e)
    {
      if (this.listViewTransaction.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a transaction first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        ServicingTransactionBase tag = (ServicingTransactionBase) this.listViewTransaction.SelectedItems[0].Tag;
        if (tag is PaymentReversalLog)
          return;
        using (PaymentDetailForm paymentDetailForm = new PaymentDetailForm((PaymentTransactionLog) tag))
        {
          int num2 = (int) paymentDetailForm.ShowDialog((IWin32Window) this);
        }
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.label3 = new Label();
      this.label31 = new Label();
      this.label5 = new Label();
      this.label28 = new Label();
      this.label7 = new Label();
      this.label27 = new Label();
      this.label8 = new Label();
      this.label26 = new Label();
      this.label25 = new Label();
      this.label24 = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.label17 = new Label();
      this.btnCancel = new Button();
      this.textBoxPaymentDueDate = new TextBox();
      this.textBoxMiscFee = new TextBox();
      this.textBoxReceivedDate = new TextBox();
      this.textBoxIndexRate = new TextBox();
      this.textBoxEscrow = new TextBox();
      this.textBoxInterestRate = new TextBox();
      this.textBoxInterest = new TextBox();
      this.textBoxPrincipal = new TextBox();
      this.textBoxLatePaymentDate = new TextBox();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label6 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label18 = new Label();
      this.boxMiscReceived = new TextBox();
      this.boxEscrowReceived = new TextBox();
      this.boxInterestReceived = new TextBox();
      this.boxPrincipalReceived = new TextBox();
      this.label19 = new Label();
      this.label20 = new Label();
      this.label21 = new Label();
      this.label22 = new Label();
      this.label23 = new Label();
      this.boxLateFeeReceived = new TextBox();
      this.boxPastDueReceived = new TextBox();
      this.label29 = new Label();
      this.label30 = new Label();
      this.label32 = new Label();
      this.label33 = new Label();
      this.boxAddEscrowReceived = new TextBox();
      this.boxAddPrincipalReceived = new TextBox();
      this.label1 = new Label();
      this.label34 = new Label();
      this.textBoxUnpaid = new TextBox();
      this.label35 = new Label();
      this.label36 = new Label();
      this.textBoxUnpaidLateFee = new TextBox();
      this.label37 = new Label();
      this.label38 = new Label();
      this.textBoxPastDue = new TextBox();
      this.groupContainer1 = new GroupContainer();
      this.listViewTransaction = new GridView();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 40);
      this.label3.Name = "label3";
      this.label3.Size = new Size(100, 13);
      this.label3.TabIndex = 83;
      this.label3.Text = "Payment Due Date:";
      this.label31.AutoSize = true;
      this.label31.Location = new Point(431, 104);
      this.label31.Name = "label31";
      this.label31.Size = new Size(13, 13);
      this.label31.TabIndex = (int) sbyte.MaxValue;
      this.label31.Text = "$";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 82);
      this.label5.Name = "label5";
      this.label5.Size = new Size(126, 13);
      this.label5.TabIndex = 86;
      this.label5.Text = "Payment Received Date:";
      this.label28.AutoSize = true;
      this.label28.Location = new Point(431, 83);
      this.label28.Name = "label28";
      this.label28.Size = new Size(13, 13);
      this.label28.TabIndex = 125;
      this.label28.Text = "$";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 103);
      this.label7.Name = "label7";
      this.label7.Size = new Size(62, 13);
      this.label7.TabIndex = 90;
      this.label7.Text = "Index Rate:";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(431, 59);
      this.label27.Name = "label27";
      this.label27.Size = new Size(13, 13);
      this.label27.TabIndex = 124;
      this.label27.Text = "$";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 124);
      this.label8.Name = "label8";
      this.label8.Size = new Size(71, 13);
      this.label8.TabIndex = 92;
      this.label8.Text = "Interest Rate:";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(431, 38);
      this.label26.Name = "label26";
      this.label26.Size = new Size(13, 13);
      this.label26.TabIndex = 123;
      this.label26.Text = "$";
      this.label25.AutoSize = true;
      this.label25.Location = new Point(209, (int) sbyte.MaxValue);
      this.label25.Name = "label25";
      this.label25.Size = new Size(15, 13);
      this.label25.TabIndex = 122;
      this.label25.Text = "%";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(209, 107);
      this.label24.Name = "label24";
      this.label24.Size = new Size(15, 13);
      this.label24.TabIndex = 121;
      this.label24.Text = "%";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(316, 38);
      this.label11.Name = "label11";
      this.label11.Size = new Size(73, 13);
      this.label11.TabIndex = 98;
      this.label11.Text = "Principal Due:";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(316, 60);
      this.label12.Name = "label12";
      this.label12.Size = new Size(68, 13);
      this.label12.TabIndex = 100;
      this.label12.Text = "Interest Due:";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(316, 82);
      this.label13.Name = "label13";
      this.label13.Size = new Size(68, 13);
      this.label13.TabIndex = 101;
      this.label13.Text = "Escrow Due:";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(316, 104);
      this.label17.Name = "label17";
      this.label17.Size = new Size(79, 13);
      this.label17.TabIndex = 108;
      this.label17.Text = "Misc. Fee Due:";
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(493, 524);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 116;
      this.btnCancel.Text = "&OK";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.textBoxPaymentDueDate.BackColor = Color.WhiteSmoke;
      this.textBoxPaymentDueDate.Location = new Point(141, 36);
      this.textBoxPaymentDueDate.Name = "textBoxPaymentDueDate";
      this.textBoxPaymentDueDate.ReadOnly = true;
      this.textBoxPaymentDueDate.Size = new Size(133, 20);
      this.textBoxPaymentDueDate.TabIndex = 78;
      this.textBoxPaymentDueDate.Tag = (object) "PaymentDueDate";
      this.textBoxMiscFee.BackColor = Color.WhiteSmoke;
      this.textBoxMiscFee.Location = new Point(447, 101);
      this.textBoxMiscFee.Name = "textBoxMiscFee";
      this.textBoxMiscFee.ReadOnly = true;
      this.textBoxMiscFee.Size = new Size(133, 20);
      this.textBoxMiscFee.TabIndex = 104;
      this.textBoxMiscFee.Tag = (object) "MiscFee";
      this.textBoxMiscFee.TextAlign = HorizontalAlignment.Right;
      this.textBoxReceivedDate.BackColor = Color.WhiteSmoke;
      this.textBoxReceivedDate.Location = new Point(141, 80);
      this.textBoxReceivedDate.Name = "textBoxReceivedDate";
      this.textBoxReceivedDate.ReadOnly = true;
      this.textBoxReceivedDate.Size = new Size(133, 20);
      this.textBoxReceivedDate.TabIndex = 81;
      this.textBoxReceivedDate.Tag = (object) "PaymentReceivedDate";
      this.textBoxIndexRate.BackColor = Color.WhiteSmoke;
      this.textBoxIndexRate.Location = new Point(141, 102);
      this.textBoxIndexRate.Name = "textBoxIndexRate";
      this.textBoxIndexRate.ReadOnly = true;
      this.textBoxIndexRate.Size = new Size(64, 20);
      this.textBoxIndexRate.TabIndex = 85;
      this.textBoxIndexRate.Tag = (object) "IndexRate";
      this.textBoxIndexRate.TextAlign = HorizontalAlignment.Right;
      this.textBoxEscrow.BackColor = Color.WhiteSmoke;
      this.textBoxEscrow.Location = new Point(447, 79);
      this.textBoxEscrow.Name = "textBoxEscrow";
      this.textBoxEscrow.ReadOnly = true;
      this.textBoxEscrow.Size = new Size(133, 20);
      this.textBoxEscrow.TabIndex = 97;
      this.textBoxEscrow.Tag = (object) "Escrow";
      this.textBoxEscrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxInterestRate.BackColor = Color.WhiteSmoke;
      this.textBoxInterestRate.Location = new Point(141, 124);
      this.textBoxInterestRate.Name = "textBoxInterestRate";
      this.textBoxInterestRate.ReadOnly = true;
      this.textBoxInterestRate.Size = new Size(64, 20);
      this.textBoxInterestRate.TabIndex = 87;
      this.textBoxInterestRate.Tag = (object) "InterestRate";
      this.textBoxInterestRate.TextAlign = HorizontalAlignment.Right;
      this.textBoxInterest.BackColor = Color.WhiteSmoke;
      this.textBoxInterest.Location = new Point(447, 57);
      this.textBoxInterest.Name = "textBoxInterest";
      this.textBoxInterest.ReadOnly = true;
      this.textBoxInterest.Size = new Size(133, 20);
      this.textBoxInterest.TabIndex = 95;
      this.textBoxInterest.Tag = (object) "Interest";
      this.textBoxInterest.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrincipal.BackColor = Color.WhiteSmoke;
      this.textBoxPrincipal.Location = new Point(447, 35);
      this.textBoxPrincipal.Name = "textBoxPrincipal";
      this.textBoxPrincipal.ReadOnly = true;
      this.textBoxPrincipal.Size = new Size(133, 20);
      this.textBoxPrincipal.TabIndex = 93;
      this.textBoxPrincipal.Tag = (object) "Principal";
      this.textBoxPrincipal.TextAlign = HorizontalAlignment.Right;
      this.textBoxLatePaymentDate.BackColor = Color.WhiteSmoke;
      this.textBoxLatePaymentDate.Location = new Point(141, 58);
      this.textBoxLatePaymentDate.Name = "textBoxLatePaymentDate";
      this.textBoxLatePaymentDate.ReadOnly = true;
      this.textBoxLatePaymentDate.Size = new Size(133, 20);
      this.textBoxLatePaymentDate.TabIndex = 79;
      this.textBoxLatePaymentDate.Tag = (object) "LatePaymentDate";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 61);
      this.label4.Name = "label4";
      this.label4.Size = new Size(101, 13);
      this.label4.TabIndex = 84;
      this.label4.Text = "Late Payment Date:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point((int) sbyte.MaxValue, 467);
      this.label2.Name = "label2";
      this.label2.Size = new Size(13, 13);
      this.label2.TabIndex = 139;
      this.label2.Text = "$";
      this.label6.AutoSize = true;
      this.label6.Location = new Point((int) sbyte.MaxValue, 446);
      this.label6.Name = "label6";
      this.label6.Size = new Size(13, 13);
      this.label6.TabIndex = 138;
      this.label6.Text = "$";
      this.label9.AutoSize = true;
      this.label9.Location = new Point((int) sbyte.MaxValue, 422);
      this.label9.Name = "label9";
      this.label9.Size = new Size(13, 13);
      this.label9.TabIndex = 137;
      this.label9.Text = "$";
      this.label10.AutoSize = true;
      this.label10.Location = new Point((int) sbyte.MaxValue, 401);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 13);
      this.label10.TabIndex = 136;
      this.label10.Text = "$";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(12, 401);
      this.label14.Name = "label14";
      this.label14.Size = new Size(50, 13);
      this.label14.TabIndex = 131;
      this.label14.Text = "Principal:";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(12, 423);
      this.label15.Name = "label15";
      this.label15.Size = new Size(45, 13);
      this.label15.TabIndex = 132;
      this.label15.Text = "Interest:";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(12, 445);
      this.label16.Name = "label16";
      this.label16.Size = new Size(45, 13);
      this.label16.TabIndex = 133;
      this.label16.Text = "Escrow:";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(12, 467);
      this.label18.Name = "label18";
      this.label18.Size = new Size(56, 13);
      this.label18.TabIndex = 135;
      this.label18.Text = "Misc. Fee:";
      this.boxMiscReceived.BackColor = Color.WhiteSmoke;
      this.boxMiscReceived.Location = new Point(143, 464);
      this.boxMiscReceived.Name = "boxMiscReceived";
      this.boxMiscReceived.ReadOnly = true;
      this.boxMiscReceived.Size = new Size(133, 20);
      this.boxMiscReceived.TabIndex = 134;
      this.boxMiscReceived.Tag = (object) "MiscFee";
      this.boxMiscReceived.TextAlign = HorizontalAlignment.Right;
      this.boxEscrowReceived.BackColor = Color.WhiteSmoke;
      this.boxEscrowReceived.Location = new Point(143, 442);
      this.boxEscrowReceived.Name = "boxEscrowReceived";
      this.boxEscrowReceived.ReadOnly = true;
      this.boxEscrowReceived.Size = new Size(133, 20);
      this.boxEscrowReceived.TabIndex = 130;
      this.boxEscrowReceived.Tag = (object) "Escrow";
      this.boxEscrowReceived.TextAlign = HorizontalAlignment.Right;
      this.boxInterestReceived.BackColor = Color.WhiteSmoke;
      this.boxInterestReceived.Location = new Point(143, 420);
      this.boxInterestReceived.Name = "boxInterestReceived";
      this.boxInterestReceived.ReadOnly = true;
      this.boxInterestReceived.Size = new Size(133, 20);
      this.boxInterestReceived.TabIndex = 129;
      this.boxInterestReceived.Tag = (object) "Interest";
      this.boxInterestReceived.TextAlign = HorizontalAlignment.Right;
      this.boxPrincipalReceived.BackColor = Color.WhiteSmoke;
      this.boxPrincipalReceived.Location = new Point(143, 398);
      this.boxPrincipalReceived.Name = "boxPrincipalReceived";
      this.boxPrincipalReceived.ReadOnly = true;
      this.boxPrincipalReceived.Size = new Size(133, 20);
      this.boxPrincipalReceived.TabIndex = 128;
      this.boxPrincipalReceived.Tag = (object) "Principal";
      this.boxPrincipalReceived.TextAlign = HorizontalAlignment.Right;
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(11, 373);
      this.label19.Name = "label19";
      this.label19.Size = new Size(257, 13);
      this.label19.TabIndex = 140;
      this.label19.Text = "Total Received For This Payment Schedule:";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(419, 419);
      this.label20.Name = "label20";
      this.label20.Size = new Size(13, 13);
      this.label20.TabIndex = 147;
      this.label20.Text = "$";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(419, 398);
      this.label21.Name = "label21";
      this.label21.Size = new Size(13, 13);
      this.label21.TabIndex = 146;
      this.label21.Text = "$";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(304, 398);
      this.label22.Name = "label22";
      this.label22.Size = new Size(54, 13);
      this.label22.TabIndex = 143;
      this.label22.Text = "Past Due:";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(304, 420);
      this.label23.Name = "label23";
      this.label23.Size = new Size(52, 13);
      this.label23.TabIndex = 145;
      this.label23.Text = "Late Fee:";
      this.boxLateFeeReceived.BackColor = Color.WhiteSmoke;
      this.boxLateFeeReceived.Location = new Point(435, 416);
      this.boxLateFeeReceived.Name = "boxLateFeeReceived";
      this.boxLateFeeReceived.ReadOnly = true;
      this.boxLateFeeReceived.Size = new Size(133, 20);
      this.boxLateFeeReceived.TabIndex = 144;
      this.boxLateFeeReceived.Tag = (object) "MiscFee";
      this.boxLateFeeReceived.TextAlign = HorizontalAlignment.Right;
      this.boxPastDueReceived.BackColor = Color.WhiteSmoke;
      this.boxPastDueReceived.Location = new Point(435, 394);
      this.boxPastDueReceived.Name = "boxPastDueReceived";
      this.boxPastDueReceived.ReadOnly = true;
      this.boxPastDueReceived.Size = new Size(133, 20);
      this.boxPastDueReceived.TabIndex = 142;
      this.boxPastDueReceived.Tag = (object) "Escrow";
      this.boxPastDueReceived.TextAlign = HorizontalAlignment.Right;
      this.label29.AutoSize = true;
      this.label29.Location = new Point(419, 465);
      this.label29.Name = "label29";
      this.label29.Size = new Size(13, 13);
      this.label29.TabIndex = 153;
      this.label29.Text = "$";
      this.label30.AutoSize = true;
      this.label30.Location = new Point(419, 444);
      this.label30.Name = "label30";
      this.label30.Size = new Size(13, 13);
      this.label30.TabIndex = 152;
      this.label30.Text = "$";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(304, 442);
      this.label32.Name = "label32";
      this.label32.Size = new Size(99, 13);
      this.label32.TabIndex = 149;
      this.label32.Text = "Additional Principal:";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(304, 464);
      this.label33.Name = "label33";
      this.label33.Size = new Size(94, 13);
      this.label33.TabIndex = 151;
      this.label33.Text = "Additional Escrow:";
      this.boxAddEscrowReceived.BackColor = Color.WhiteSmoke;
      this.boxAddEscrowReceived.Location = new Point(435, 460);
      this.boxAddEscrowReceived.Name = "boxAddEscrowReceived";
      this.boxAddEscrowReceived.ReadOnly = true;
      this.boxAddEscrowReceived.Size = new Size(133, 20);
      this.boxAddEscrowReceived.TabIndex = 150;
      this.boxAddEscrowReceived.Tag = (object) "MiscFee";
      this.boxAddEscrowReceived.TextAlign = HorizontalAlignment.Right;
      this.boxAddPrincipalReceived.BackColor = Color.WhiteSmoke;
      this.boxAddPrincipalReceived.Location = new Point(435, 438);
      this.boxAddPrincipalReceived.Name = "boxAddPrincipalReceived";
      this.boxAddPrincipalReceived.ReadOnly = true;
      this.boxAddPrincipalReceived.Size = new Size(133, 20);
      this.boxAddPrincipalReceived.TabIndex = 148;
      this.boxAddPrincipalReceived.Tag = (object) "Escrow";
      this.boxAddPrincipalReceived.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point((int) sbyte.MaxValue, 491);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 156;
      this.label1.Text = "$";
      this.label34.AutoSize = true;
      this.label34.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label34.ForeColor = Color.Red;
      this.label34.Location = new Point(12, 489);
      this.label34.Name = "label34";
      this.label34.Size = new Size(101, 13);
      this.label34.TabIndex = 155;
      this.label34.Text = "Unpaid Balance:";
      this.textBoxUnpaid.BackColor = Color.WhiteSmoke;
      this.textBoxUnpaid.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.textBoxUnpaid.Location = new Point(143, 486);
      this.textBoxUnpaid.Name = "textBoxUnpaid";
      this.textBoxUnpaid.ReadOnly = true;
      this.textBoxUnpaid.Size = new Size(133, 20);
      this.textBoxUnpaid.TabIndex = 154;
      this.textBoxUnpaid.Tag = (object) "MiscFee";
      this.textBoxUnpaid.TextAlign = HorizontalAlignment.Right;
      this.label35.AutoSize = true;
      this.label35.Location = new Point(431, 150);
      this.label35.Name = "label35";
      this.label35.Size = new Size(13, 13);
      this.label35.TabIndex = 159;
      this.label35.Text = "$";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(316, 150);
      this.label36.Name = "label36";
      this.label36.Size = new Size(112, 13);
      this.label36.TabIndex = 158;
      this.label36.Text = "Unpaid Late Fee Due:";
      this.textBoxUnpaidLateFee.BackColor = Color.WhiteSmoke;
      this.textBoxUnpaidLateFee.Location = new Point(447, 147);
      this.textBoxUnpaidLateFee.Name = "textBoxUnpaidLateFee";
      this.textBoxUnpaidLateFee.ReadOnly = true;
      this.textBoxUnpaidLateFee.Size = new Size(133, 20);
      this.textBoxUnpaidLateFee.TabIndex = 157;
      this.textBoxUnpaidLateFee.Tag = (object) "MiscFee";
      this.textBoxUnpaidLateFee.TextAlign = HorizontalAlignment.Right;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(431, (int) sbyte.MaxValue);
      this.label37.Name = "label37";
      this.label37.Size = new Size(13, 13);
      this.label37.TabIndex = 162;
      this.label37.Text = "$";
      this.label38.AutoSize = true;
      this.label38.Location = new Point(316, (int) sbyte.MaxValue);
      this.label38.Name = "label38";
      this.label38.Size = new Size(93, 13);
      this.label38.TabIndex = 161;
      this.label38.Text = "Past Due Amount:";
      this.textBoxPastDue.BackColor = Color.WhiteSmoke;
      this.textBoxPastDue.Location = new Point(447, 124);
      this.textBoxPastDue.Name = "textBoxPastDue";
      this.textBoxPastDue.ReadOnly = true;
      this.textBoxPastDue.Size = new Size(133, 20);
      this.textBoxPastDue.TabIndex = 160;
      this.textBoxPastDue.Tag = (object) "MiscFee";
      this.textBoxPastDue.TextAlign = HorizontalAlignment.Right;
      this.groupContainer1.Controls.Add((Control) this.listViewTransaction);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label37);
      this.groupContainer1.Controls.Add((Control) this.textBoxPrincipal);
      this.groupContainer1.Controls.Add((Control) this.label38);
      this.groupContainer1.Controls.Add((Control) this.textBoxInterest);
      this.groupContainer1.Controls.Add((Control) this.textBoxPastDue);
      this.groupContainer1.Controls.Add((Control) this.textBoxInterestRate);
      this.groupContainer1.Controls.Add((Control) this.label35);
      this.groupContainer1.Controls.Add((Control) this.textBoxEscrow);
      this.groupContainer1.Controls.Add((Control) this.label36);
      this.groupContainer1.Controls.Add((Control) this.textBoxIndexRate);
      this.groupContainer1.Controls.Add((Control) this.textBoxUnpaidLateFee);
      this.groupContainer1.Controls.Add((Control) this.textBoxReceivedDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.textBoxMiscFee);
      this.groupContainer1.Controls.Add((Control) this.label34);
      this.groupContainer1.Controls.Add((Control) this.textBoxLatePaymentDate);
      this.groupContainer1.Controls.Add((Control) this.textBoxUnpaid);
      this.groupContainer1.Controls.Add((Control) this.textBoxPaymentDueDate);
      this.groupContainer1.Controls.Add((Control) this.label29);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.label30);
      this.groupContainer1.Controls.Add((Control) this.label17);
      this.groupContainer1.Controls.Add((Control) this.label32);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.label33);
      this.groupContainer1.Controls.Add((Control) this.label12);
      this.groupContainer1.Controls.Add((Control) this.boxAddEscrowReceived);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.boxAddPrincipalReceived);
      this.groupContainer1.Controls.Add((Control) this.label24);
      this.groupContainer1.Controls.Add((Control) this.label20);
      this.groupContainer1.Controls.Add((Control) this.label25);
      this.groupContainer1.Controls.Add((Control) this.label21);
      this.groupContainer1.Controls.Add((Control) this.label26);
      this.groupContainer1.Controls.Add((Control) this.label22);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.label23);
      this.groupContainer1.Controls.Add((Control) this.label27);
      this.groupContainer1.Controls.Add((Control) this.boxLateFeeReceived);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.boxPastDueReceived);
      this.groupContainer1.Controls.Add((Control) this.label28);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label19);
      this.groupContainer1.Controls.Add((Control) this.label31);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.boxPrincipalReceived);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.boxInterestReceived);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.boxEscrowReceived);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.boxMiscReceived);
      this.groupContainer1.Controls.Add((Control) this.label15);
      this.groupContainer1.Controls.Add((Control) this.label18);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(592, 556);
      this.groupContainer1.TabIndex = 163;
      this.groupContainer1.Text = "Schedule Payment #1";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 31;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Transaction Type";
      gvColumn2.Width = 168;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Transaction Date";
      gvColumn3.Width = 149;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Transaction Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 220;
      this.listViewTransaction.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewTransaction.Location = new Point(11, 176);
      this.listViewTransaction.Name = "listViewTransaction";
      this.listViewTransaction.Size = new Size(570, 186);
      this.listViewTransaction.TabIndex = 163;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(592, 556);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SchedulePaymentDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Payment Schedule";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
