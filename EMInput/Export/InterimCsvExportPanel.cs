// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.InterimCsvExportPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.DataEngine.InterimServicing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class InterimCsvExportPanel : WizardItemWithHeader
  {
    private Panel panel2;
    private Label label1;
    private TextBox txtFile;
    private Button btnBrowse;
    private IContainer components;
    private OpenFileDialog ofdBrowse;
    private ServicingTransactionBase[] transactions;
    private Hashtable transTable;
    private string[] csvExportColumns = new string[30]
    {
      "Transaction #",
      "Statement Date",
      "Date Due",
      "Date Late",
      "Date Received",
      "Date Deposited",
      "Total Amount Received",
      "Principal Paid",
      "Interest Paid",
      "Escrow Paid",
      "Late Fee Paid",
      "Misc. Fee Paid",
      "Additional Principal Paid",
      "Additional Escrow Paid",
      "Index Rate",
      "Interest Rate",
      "Payment Method",
      "Account Holder",
      "Institution",
      "Routing #",
      "Account #",
      "Amount",
      "Check #",
      "Reference #",
      "Date",
      "Transaction Entered by",
      "Date Transaction Entered",
      "Transaction Last Modified by",
      "Date Last Modified",
      "Comments"
    };

    public override string NextLabel => "&Export >";

    public InterimCsvExportPanel(ServicingTransactionBase[] transactions)
    {
      this.transactions = transactions;
      this.InitializeComponent();
      this.Subheader = "Select the location for the exported transactions and enter a new file name.";
      this.txtFile.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\interimtransactions.csv";
      this.txtFile.Select(0, 0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ofdBrowse = new OpenFileDialog();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.txtFile = new TextBox();
      this.btnBrowse = new Button();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.ofdBrowse.CheckFileExists = false;
      this.ofdBrowse.CheckPathExists = false;
      this.ofdBrowse.DefaultExt = "csv";
      this.ofdBrowse.Filter = "Comma-Separated Value Files|*.csv|All Files|*.*";
      this.ofdBrowse.Title = "File Location and Name";
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtFile);
      this.panel2.Controls.Add((Control) this.btnBrowse);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.label1.Location = new Point(38, 62);
      this.label1.Name = "label1";
      this.label1.Size = new Size(300, 18);
      this.label1.TabIndex = 3;
      this.label1.Text = "Export transaction data to the following location and file:";
      this.txtFile.Location = new Point(38, 80);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new Size(316, 20);
      this.txtFile.TabIndex = 4;
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Location = new Point(356, 78);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(75, 23);
      this.btnBrowse.TabIndex = 5;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = false;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.Controls.Add((Control) this.panel2);
      this.Header = "File Location and Name";
      this.Name = nameof (InterimCsvExportPanel);
      this.Subheader = "Select the location for the exported contacts and enter a new file name.";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }

    public static void ChangeDlgItemThreadProc()
    {
      int window;
      while ((window = Win32.FindWindow((string) null, "File Location and Name")) == 0)
        Thread.Sleep(10);
      Win32.SetDlgItemText(window, Win32.IDOK, "OK");
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(InterimCsvExportPanel.ChangeDlgItemThreadProc)).Start();
      if (this.ofdBrowse.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.txtFile.Text = this.ofdBrowse.FileName;
    }

    public override WizardItem Next()
    {
      if (this.txtFile.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid file name in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (File.Exists(this.txtFile.Text) && DialogResult.No == Utils.Dialog((IWin32Window) this, "The specified file already exits. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
        return (WizardItem) null;
      new ProgressDialog("Exporting Interim Servicing Transactions", new AsynchronousProcess(this.exportTransactions), (object) null, true).ShowDialog((IWin32Window) this.ParentForm);
      return WizardItem.Finished;
    }

    private void writeCsvHeader(StreamWriter sw)
    {
      for (int index = 0; index < this.csvExportColumns.Length - 1; ++index)
        sw.Write("\"" + this.csvExportColumns[index] + "\",");
      sw.Write("\"" + this.csvExportColumns[this.csvExportColumns.Length - 1] + "\"\r\n");
    }

    private void writeCsvRecord(StreamWriter sw, ServicingTransactionBase transaction, int transNo)
    {
      bool isReversal = false;
      ServicingTransactionBase originalTransaction = transaction;
      if (transaction is PaymentReversalLog)
      {
        PaymentReversalLog paymentReversalLog = (PaymentReversalLog) transaction;
        isReversal = true;
        if (this.transTable.ContainsKey((object) paymentReversalLog.PaymentGUID))
          transaction = (ServicingTransactionBase) this.transTable[(object) paymentReversalLog.PaymentGUID];
      }
      if (transaction == null)
        return;
      this.writeCsvTransaction(sw, transaction, originalTransaction, transNo, isReversal);
    }

    private void writeCsvTransaction(
      StreamWriter sw,
      ServicingTransactionBase transaction,
      ServicingTransactionBase originalTransaction,
      int transNo,
      bool isReversal)
    {
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < this.csvExportColumns.Length; ++index)
      {
        object valObj = (object) null;
        string str = "";
        if (this.csvExportColumns[index] == "Transaction #")
        {
          valObj = (object) transNo;
        }
        else
        {
          switch (transaction)
          {
            case PaymentTransactionLog _:
              valObj = this.mapPaymentField(this.csvExportColumns[index], (PaymentTransactionLog) transaction, originalTransaction, isReversal);
              break;
            case EscrowDisbursementLog _:
              valObj = this.mapEscrowDisbursementField(this.csvExportColumns[index], (EscrowDisbursementLog) transaction, originalTransaction, isReversal);
              break;
            case EscrowInterestLog _:
              valObj = this.mapEscrowInterestField(this.csvExportColumns[index], (EscrowInterestLog) transaction);
              break;
            case LoanPurchaseLog _:
              valObj = this.mapPurchaseAdviceFields(this.csvExportColumns[index], (LoanPurchaseLog) transaction);
              break;
            case PrincipalDisbursementLog _:
              valObj = this.mapPrincipalDisbursementField(this.csvExportColumns[index], (PrincipalDisbursementLog) transaction);
              break;
          }
        }
        if (valObj != null)
        {
          switch (this.csvExportColumns[index])
          {
            case "Additional Escrow Paid":
            case "Additional Principal Paid":
            case "Amount":
            case "Escrow Paid":
            case "Interest Paid":
            case "Late Fee Paid":
            case "Misc. Fee Paid":
            case "Principal Paid":
            case "Total Amount Received":
              str = this.formatCsvValue(valObj, FieldFormat.DECIMAL_2);
              break;
            case "Date":
            case "Date Deposited":
            case "Date Due":
            case "Date Last Modified":
            case "Date Late":
            case "Date Received":
            case "Date Transaction Entered":
            case "Statement Date":
              str = this.formatCsvValue(valObj, FieldFormat.DATE);
              break;
            case "Index Rate":
            case "Interest Rate":
              str = this.formatCsvValue(valObj, FieldFormat.DECIMAL_3);
              break;
            case "Transaction #":
              str = this.formatCsvValue(valObj, FieldFormat.INTEGER);
              break;
            default:
              str = this.formatCsvValue(valObj, FieldFormat.STRING);
              break;
          }
        }
        int startIndex1;
        for (int startIndex2 = 0; (startIndex1 = str.IndexOf('"', startIndex2)) != -1; startIndex2 = startIndex1 + 2)
          str = str.Insert(startIndex1, "\"");
        sw.Write("\"" + str + "\"");
        if (index != this.csvExportColumns.Length - 1)
          sw.Write(",");
        else
          sw.Write("\r\n");
      }
    }

    private object mapPaymentField(
      string fieldID,
      PaymentTransactionLog payLog,
      ServicingTransactionBase originalTransaction,
      bool isReversal)
    {
      object obj = (object) null;
      int num = 1;
      if (isReversal)
        num = -1;
      switch (fieldID)
      {
        case "Account #":
          obj = (object) payLog.AccountNumber;
          break;
        case "Account Holder":
          obj = (object) payLog.AccountHolder;
          break;
        case "Additional Escrow Paid":
          obj = (object) (payLog.AdditionalEscrow * (double) num);
          break;
        case "Additional Principal Paid":
          obj = (object) (payLog.AdditionalPrincipal * (double) num);
          break;
        case "Amount":
          obj = payLog.PaymentMethod != ServicingPaymentMethods.AutomatedClearingHouse ? (payLog.PaymentMethod != ServicingPaymentMethods.Check ? (payLog.PaymentMethod != ServicingPaymentMethods.LockBox ? (payLog.PaymentMethod != ServicingPaymentMethods.Wire ? (object) 0.0 : (object) (payLog.WireAmount * (double) num)) : (object) (payLog.LockBoxAmount * (double) num)) : (object) (payLog.CheckAmount * (double) num)) : (object) (payLog.TransactionAmount * (double) num);
          break;
        case "Check #":
          obj = (object) payLog.CheckNumber;
          break;
        case "Comments":
          obj = (object) payLog.Comments;
          break;
        case "Date":
          obj = payLog.PaymentMethod != ServicingPaymentMethods.AutomatedClearingHouse ? (payLog.PaymentMethod != ServicingPaymentMethods.Check ? (payLog.PaymentMethod != ServicingPaymentMethods.LockBox ? (payLog.PaymentMethod != ServicingPaymentMethods.Wire ? (object) DateTime.MinValue : (object) payLog.WireDate) : (object) payLog.CreditDate) : (object) payLog.CheckDate) : (object) payLog.TransactionDate;
          break;
        case "Date Deposited":
          obj = (object) payLog.PaymentDepositedDate;
          break;
        case "Date Due":
          obj = (object) payLog.PaymentDueDate;
          break;
        case "Date Last Modified":
          obj = !isReversal ? (object) payLog.ModifiedDateTime : (object) originalTransaction.ModifiedDateTime;
          break;
        case "Date Late":
          obj = (object) payLog.LatePaymentDate;
          break;
        case "Date Received":
          obj = (object) payLog.PaymentReceivedDate;
          break;
        case "Date Transaction Entered":
          obj = !isReversal ? (object) payLog.CreatedDateTime : (object) originalTransaction.CreatedDateTime;
          break;
        case "Escrow Paid":
          obj = (object) (payLog.Escrow * (double) num);
          break;
        case "Index Rate":
          obj = (object) payLog.IndexRate;
          break;
        case "Institution":
          obj = (object) payLog.InstitutionName;
          break;
        case "Interest Paid":
          obj = (object) (payLog.Interest * (double) num);
          break;
        case "Interest Rate":
          obj = (object) payLog.InterestRate;
          break;
        case "Late Fee Paid":
          obj = (object) (payLog.LateFee * (double) num);
          break;
        case "Misc. Fee Paid":
          obj = (object) (payLog.MiscFee * (double) num);
          break;
        case "Payment Method":
          obj = (object) ServicingEnum.ServicingPaymentMethodsToUI(payLog.PaymentMethod);
          break;
        case "Principal Paid":
          obj = (object) (payLog.Principal * (double) num);
          break;
        case "Reference #":
          obj = payLog.PaymentMethod == ServicingPaymentMethods.AutomatedClearingHouse || payLog.PaymentMethod == ServicingPaymentMethods.Wire ? (object) payLog.Reference : (object) string.Empty;
          break;
        case "Routing #":
          obj = payLog.PaymentMethod != ServicingPaymentMethods.LockBox ? (object) payLog.InstitutionRouting : (object) string.Empty;
          break;
        case "Statement Date":
          obj = (object) payLog.StatementDate;
          break;
        case "Total Amount Received":
          obj = (object) (payLog.TotalAmountReceived * (double) num);
          break;
        case "Transaction Entered by":
          obj = !isReversal ? (object) payLog.CreatedByName : (object) originalTransaction.CreatedByName;
          break;
        case "Transaction Last Modified by":
          obj = !isReversal ? (object) payLog.ModifiedByName : (object) originalTransaction.ModifiedByName;
          break;
      }
      return obj;
    }

    private object mapEscrowDisbursementField(
      string fieldID,
      EscrowDisbursementLog payLog,
      ServicingTransactionBase originalTransaction,
      bool isReversal)
    {
      object obj = (object) null;
      int num = -1;
      if (isReversal)
        num = 1;
      switch (fieldID)
      {
        case "Amount":
          obj = (object) (payLog.TransactionAmount * (double) num);
          break;
        case "Comments":
          obj = (object) payLog.Comments;
          break;
        case "Date":
          obj = (object) payLog.TransactionDate;
          break;
        case "Date Due":
          obj = (object) payLog.DisbursementDueDate;
          break;
        case "Date Last Modified":
          obj = !isReversal ? (object) payLog.ModifiedDateTime : (object) originalTransaction.ModifiedDateTime;
          break;
        case "Date Received":
          obj = (object) payLog.TransactionDate;
          break;
        case "Date Transaction Entered":
          obj = !isReversal ? (object) payLog.CreatedDateTime : (object) originalTransaction.CreatedDateTime;
          break;
        case "Escrow Paid":
          obj = (object) (payLog.TransactionAmount * (double) num);
          break;
        case "Institution":
          obj = (object) payLog.InstitutionName;
          break;
        case "Total Amount Received":
          obj = (object) (payLog.TransactionAmount * (double) num);
          break;
        case "Transaction Entered by":
          obj = !isReversal ? (object) payLog.CreatedByName : (object) originalTransaction.CreatedByName;
          break;
        case "Transaction Last Modified by":
          obj = !isReversal ? (object) payLog.ModifiedByName : (object) originalTransaction.ModifiedByName;
          break;
      }
      return obj;
    }

    private object mapEscrowInterestField(string fieldID, EscrowInterestLog payLog)
    {
      object obj = (object) null;
      switch (fieldID)
      {
        case "Amount":
          obj = (object) payLog.InterestAmount;
          break;
        case "Comments":
          obj = (object) payLog.Comments;
          break;
        case "Date":
          obj = (object) payLog.IncurredDate;
          break;
        case "Date Last Modified":
          obj = (object) payLog.ModifiedDateTime;
          break;
        case "Date Received":
          obj = (object) payLog.IncurredDate;
          break;
        case "Date Transaction Entered":
          obj = (object) payLog.CreatedDateTime;
          break;
        case "Total Amount Received":
          obj = (object) payLog.InterestAmount;
          break;
        case "Transaction Entered by":
          obj = (object) payLog.CreatedByName;
          break;
        case "Transaction Last Modified by":
          obj = (object) payLog.ModifiedByName;
          break;
      }
      return obj;
    }

    private object mapPurchaseAdviceFields(string fieldID, LoanPurchaseLog purchaseLog)
    {
      object obj = (object) null;
      switch (fieldID)
      {
        case "Total Amount Received":
        case "Principal Paid":
          obj = (object) purchaseLog.PurchaseAmount;
          break;
        case "Transaction Entered by":
          obj = (object) "Admin User";
          break;
        case "Date Transaction Entered":
          obj = (object) purchaseLog.PurchaseAdviceDate;
          break;
      }
      return obj;
    }

    private object mapPrincipalDisbursementField(
      string fieldID,
      PrincipalDisbursementLog principalDisburseLog)
    {
      object obj = (object) null;
      switch (fieldID)
      {
        case "Comments":
          obj = (object) principalDisburseLog.Comments;
          break;
        case "Date":
        case "Date Received":
          obj = (object) principalDisburseLog.TransactionDate;
          break;
        case "Date Transaction Entered":
          obj = (object) principalDisburseLog.CreatedDateTime;
          break;
        case "Institution":
          obj = (object) principalDisburseLog.InstitutionName;
          break;
        case "Total Amount Received":
          obj = (object) (principalDisburseLog.TransactionAmount * -1.0);
          break;
        case "Transaction Entered by":
          obj = (object) principalDisburseLog.CreatedByName;
          break;
      }
      return obj;
    }

    private string formatCsvValue(object valObj, FieldFormat format)
    {
      bool needsUpdate = false;
      string orgval = valObj.ToString();
      if (format == FieldFormat.DATE)
      {
        try
        {
          DateTime dateTime = (DateTime) valObj;
          if (dateTime == DateTime.MinValue)
            return string.Empty;
          orgval = dateTime.ToString("MM/dd/yyyy");
        }
        catch
        {
          orgval = valObj.ToString();
        }
      }
      return Utils.FormatInput(orgval, format, ref needsUpdate);
    }

    private DialogResult exportTransactions(object state, IProgressFeedback feedback)
    {
      int num1 = 0;
      int transNo = 0;
      StreamWriter sw = (StreamWriter) null;
      try
      {
        feedback.Status = "Preparing to export...";
        sw = new StreamWriter(this.txtFile.Text);
        feedback.ResetCounter(this.transactions.Length);
        this.writeCsvHeader(sw);
        feedback.Status = "Exporting transactions...";
        this.transTable = new Hashtable();
        for (int index = 0; index < this.transactions.Length; ++index)
          this.transTable.Add((object) this.transactions[index].TransactionGUID, (object) this.transactions[index]);
        for (int index = 0; index < this.transactions.Length; ++index)
        {
          feedback.Increment(1);
          feedback.Details = "Completed " + (object) num1 + " of " + (object) this.transactions.Length;
          if (feedback.Cancel)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Operation cancelled after exporting " + (object) num1 + " transactions.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return DialogResult.Cancel;
          }
          ++transNo;
          if (!(this.transactions[index] is OtherTransactionLog))
          {
            ++num1;
            this.writeCsvRecord(sw, this.transactions[index], transNo);
          }
        }
        int num3 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Successfully exported " + (object) num1 + " transactions.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "An error has occurred while exporting transactions: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
      finally
      {
        sw.Close();
      }
    }

    private void timerCallback(object feedbackAsObject)
    {
      ((IServerProgressFeedback) feedbackAsObject).Increment(1);
    }
  }
}
