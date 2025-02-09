// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ServicingDetailsWorksheet
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.InterimServicing;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ServicingDetailsWorksheet : CustomUserControl, IOnlineHelpTarget, IRefreshContents
  {
    private LoanData loan;
    private LoanScreen freeScreen;
    private bool debugMode;
    private FeaturesAclManager aclMgr;
    private bool canEdit = true;
    private bool canAdd = true;
    private PaymentScheduleSnapshot paySnapshot;
    private int daysAlertPrint = -1;
    private int daysAlertDue = -1;
    private int daysAlertEscrow = -1;
    private IContainer components;
    private Panel panelAll;
    private Panel panelTop;
    private ToolTip toolTip1;
    private Button btnStartServicing;
    private GradientPanel gradientPanelHeader;
    private BorderPanel borderPanelBody;
    private Label labelHeader;
    private BorderPanel borderPanelForBorder;
    private PictureBox pictureBox1;
    private BorderPanel borderPanelList;
    private GroupContainer groupContainerList;
    private StandardIconButton iconBtnExport;
    private VerticalSeparator verticalSeparator1;
    private GridView gridViewTransaction;
    private StandardIconButton iconBtnDelete;
    private StandardIconButton iconBtnEdit;
    private StandardIconButton iconBtnAdd;
    private Button btnPrintList;
    private Button btnPrintDetail;
    private GroupContainer groupContainer_Comments;
    private Panel panelComments;
    private RichTextBox richTextBox_Comments;
    private Button btnAddConmment;
    private Panel panelGridRightSection;
    private Panel panelCommentsRight;

    public ServicingDetailsWorksheet(LoanData loan)
    {
      if (EnConfigurationSettings.GlobalSettings.Debug)
        this.debugMode = true;
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_EditDeleteTransaction))
        this.canEdit = false;
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_EnterTransaction))
        this.canAdd = false;
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_CopyLoan))
        this.btnStartServicing.Enabled = false;
      else if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_RecopyLoan) && this.loan.GetServicingTransactions() != null)
        this.btnStartServicing.Enabled = false;
      AlertConfig[] alertConfigList = Session.AlertManager.GetAlertConfigList();
      for (int index = 0; index < alertConfigList.Length; ++index)
      {
        if (this.daysAlertPrint == -1 && alertConfigList[index].AlertID == 7)
          this.daysAlertPrint = alertConfigList[index].DaysBefore;
        if (this.daysAlertDue == -1 && alertConfigList[index].AlertID == 6)
          this.daysAlertDue = alertConfigList[index].DaysBefore;
        if (this.daysAlertEscrow == -1 && alertConfigList[index].AlertID == 3)
          this.daysAlertEscrow = alertConfigList[index].DaysBefore;
        if (this.daysAlertPrint != -1 && this.daysAlertDue != -1 && this.daysAlertEscrow != -1)
          break;
      }
      this.RefreshTransactionList();
      this.RefreshComments();
      if (this.loan.GetPaymentScheduleSnapshot() == null)
        this.ChangeButtonStatus(false);
      else
        this.ChangeButtonStatus(true);
      this.freeScreen = new LoanScreen(Session.DefaultInstance);
      string str = "SERVICINGDETAIL";
      this.freeScreen.LoadForm(new InputFormInfo(str, str));
      this.freeScreen.RemoveTitle();
      this.freeScreen.RemoveBorder();
      this.freeScreen.RemoveScrollBar();
      this.panelTop.Controls.Add((Control) this.freeScreen);
      this.freeScreen.Focus();
      if (this.loan.FieldForGoTo != string.Empty)
      {
        this.freeScreen.SetGoToFieldFocus(this.loan.FieldForGoTo, 1);
        this.loan.FieldForGoTo = string.Empty;
      }
      this.panelAll.Height = this.panelTop.Height + this.borderPanelList.Height;
      this.freeScreen_OnFieldChanged((object) null, (EventArgs) null);
      this.freeScreen.ButtonClicked += new EventHandler(this.freeScreen_ButtonClicked);
      this.freeScreen.OnFieldChanged += new EventHandler(this.freeScreen_OnFieldChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.freeScreen.ButtonClicked -= new EventHandler(this.freeScreen_ButtonClicked);
        this.freeScreen.OnFieldChanged -= new EventHandler(this.freeScreen_OnFieldChanged);
        this.components.Dispose();
        this.freeScreen.Dispose();
      }
      base.Dispose(disposing);
    }

    public void RefreshContents()
    {
      this.freeScreen.RefreshContents();
      this.freeScreen_OnFieldChanged((object) null, (EventArgs) null);
    }

    public void RefreshLoanContents()
    {
      this.freeScreen.RefreshLoanContents();
      this.freeScreen_OnFieldChanged((object) null, (EventArgs) null);
    }

    private void freeScreen_ButtonClicked(object sender, EventArgs e)
    {
      if (!(sender is string) || this.loan.GetPaymentScheduleSnapshot() == null)
        return;
      switch ((string) sender)
      {
        case "viewlastpayment":
          if (this.loan.GetField("SERVICE.X30") == string.Empty)
            break;
          if (this.gridViewTransaction.Items.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You don't have a last payment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          string field = this.loan.GetField("SERVICE.LASTGUID");
          for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
          {
            this.gridViewTransaction.Items[nItemIndex].Selected = false;
            ServicingTransactionBase tag = (ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag;
            if (tag.TransactionType == ServicingTransactionTypes.Payment && tag.TransactionGUID == field)
            {
              this.gridViewTransaction.Items[nItemIndex].Selected = true;
              this.btnView_Click((object) null, (EventArgs) null);
              return;
            }
          }
          for (int nItemIndex = this.gridViewTransaction.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
          {
            if ((ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag is PaymentTransactionLog)
            {
              this.gridViewTransaction.Items[nItemIndex].Selected = true;
              this.btnView_Click((object) null, (EventArgs) null);
              break;
            }
          }
          break;
        case "pastduecalculation":
          Cursor.Current = Cursors.WaitCursor;
          this.loan.Calculator.CalculateInterimServicing(false);
          this.freeScreen.RefreshContents();
          Cursor.Current = Cursors.Default;
          break;
        case "viewsummaryhistory":
          using (ServicingAnnualSummary servicingAnnualSummary = new ServicingAnnualSummary(this.loan.Calculator.GetInterimServicingAnnualSummary(-1)))
          {
            int num = (int) servicingAnnualSummary.ShowDialog((IWin32Window) this);
            break;
          }
      }
    }

    public void ChangeButtonStatus(bool enabled)
    {
      this.iconBtnDelete.Enabled = enabled && this.canEdit;
      this.iconBtnExport.Enabled = enabled;
      this.iconBtnAdd.Enabled = enabled && this.canAdd;
      this.btnPrintDetail.Enabled = enabled;
      this.btnPrintList.Enabled = enabled;
      this.iconBtnEdit.Enabled = enabled && this.canEdit;
      this.btnAddConmment.Enabled = enabled && this.canEdit;
      if (this.loan.GetPaymentScheduleSnapshot() == null)
        return;
      this.listViewTransaction_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public void RefreshTransactionList()
    {
      this.gridViewTransaction.Items.Clear();
      ServicingTransactionBase[] servicingTransactions = this.loan.GetServicingTransactions();
      if (servicingTransactions == null || servicingTransactions.Length == 0)
        return;
      if (!(this.loan.GetField("2370") != "//") || !(this.loan.GetField("2370") != "") || !(this.loan.GetField("3514") != "//") || !(this.loan.GetField("3514") != "") || !(this.loan.GetField("2211") != ""))
      {
        for (int index = 0; index < servicingTransactions.Length; ++index)
        {
          if (servicingTransactions[index] is LoanPurchaseLog)
            this.loan.RemoveServicingTransaction(servicingTransactions[index]);
        }
        servicingTransactions = this.loan.GetServicingTransactions();
      }
      this.gridViewTransaction.BeginUpdate();
      for (int index = 0; index < servicingTransactions.Length; ++index)
      {
        if (this.debugMode || !(servicingTransactions[index] is SchedulePaymentLog))
          this.gridViewTransaction.Items.Add(this.createTransactionItem(servicingTransactions[index], false));
      }
      this.gridViewTransaction.EndUpdate();
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return;
      ServicingTransactionTypes transactionAction = ServicingTransactionTypes.Payment;
      using (TransactionTypeSelector transactionTypeSelector = new TransactionTypeSelector())
      {
        if (transactionTypeSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        transactionAction = transactionTypeSelector.TransactionType;
      }
      switch (transactionAction)
      {
        case ServicingTransactionTypes.PaymentReversal:
          using (PaymentReversalSelector reversalSelector = new PaymentReversalSelector((PaymentReversalLog) null, this.gridViewTransaction, this.gridViewTransaction.Items.Count + 1, ServicingTransactionTypes.Payment, false))
          {
            if (!reversalSelector.HasTransactionToReversal)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "There is no transaction for reversal.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (reversalSelector.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase reversalLog = (ServicingTransactionBase) reversalSelector.ReversalLog;
              reversalLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(reversalLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(reversalLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        case ServicingTransactionTypes.EscrowDisbursement:
          using (EscrowDisbursementForm disbursementForm = new EscrowDisbursementForm((EscrowDisbursementLog) null, this.gridViewTransaction.Items.Count + 1, this.getNextNumber(ServicingTransactionTypes.EscrowDisbursement), this.loan, false))
          {
            if (disbursementForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase disbursementLog = (ServicingTransactionBase) disbursementForm.DisbursementLog;
              disbursementLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(disbursementLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(disbursementLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        case ServicingTransactionTypes.EscrowInterest:
          using (EscrowInterestForm escrowInterestForm = new EscrowInterestForm((EscrowInterestLog) null, this.gridViewTransaction.Items.Count + 1, false))
          {
            if (escrowInterestForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase interestLog = (ServicingTransactionBase) escrowInterestForm.InterestLog;
              interestLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(interestLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(interestLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        case ServicingTransactionTypes.EscrowDisbursementReversal:
          using (PaymentReversalSelector reversalSelector = new PaymentReversalSelector((PaymentReversalLog) null, this.gridViewTransaction, this.gridViewTransaction.Items.Count + 1, ServicingTransactionTypes.EscrowDisbursement, false))
          {
            if (!reversalSelector.HasTransactionToReversal)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "There is no transaction for reversal.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (reversalSelector.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase reversalLog = (ServicingTransactionBase) reversalSelector.ReversalLog;
              reversalLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(reversalLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(reversalLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        case ServicingTransactionTypes.Other:
          using (OtherDetailForm otherDetailForm = new OtherDetailForm((OtherTransactionLog) null, this.gridViewTransaction.Items.Count + 1, false))
          {
            if (otherDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase otherTransLog = (ServicingTransactionBase) otherDetailForm.OtherTransLog;
              otherTransLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(otherTransLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(otherTransLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        case ServicingTransactionTypes.PrincipalDisbursement:
          using (PrincipalDisbursementForm disbursementForm = new PrincipalDisbursementForm((PrincipalDisbursementLog) null, this.gridViewTransaction.Items.Count + 1, this.loan, false))
          {
            if (disbursementForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase disbursementLog = (ServicingTransactionBase) disbursementForm.DisbursementLog;
              disbursementLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(disbursementLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(disbursementLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
        default:
          int nextNumber = this.getNextNumber(ServicingTransactionTypes.Payment);
          PaymentTransactionLog paymentTransactionLog = new PaymentTransactionLog();
          Session.LoanDataMgr.PopulateNextServicingPaymentInformation(paymentTransactionLog);
          paymentTransactionLog.CreatedByID = Session.UserInfo.Userid;
          paymentTransactionLog.CreatedByName = Session.UserInfo.FullName;
          string s = this.loan.GetField("Service.X23");
          if (s == "")
            s = "0";
          paymentTransactionLog.SchedulePayLogMiscFee = double.Parse(s);
          using (PaymentDetailForm paymentDetailForm = new PaymentDetailForm(paymentTransactionLog, this.gridViewTransaction.Items.Count + 1, nextNumber, paymentTransactionLog.Principal, paymentTransactionLog.Interest, this.loan, true))
          {
            if (paymentDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.gridViewTransaction.SelectedItems.Clear();
              ServicingTransactionBase paymentLog = (ServicingTransactionBase) paymentDetailForm.PaymentLog;
              paymentLog.TransactionDate = DateTime.Now;
              this.loan.AddServicingTransaction(paymentLog);
              this.gridViewTransaction.Items.Add(this.createTransactionItem(paymentLog, true));
              this.updateSummary(transactionAction, false, false);
              break;
            }
            break;
          }
      }
      this.RefreshTransactionList();
      this.freeScreen.RefreshContents();
      this.listViewTransaction_SelectedIndexChanged((object) this, (EventArgs) null);
      this.panelAll.Height = this.panelTop.Height + this.borderPanelList.Height;
    }

    private GVItem createTransactionItem(ServicingTransactionBase trans, bool selected)
    {
      GVItem transactionItem = new GVItem((this.gridViewTransaction.Items.Count + 1).ToString());
      if (trans is PaymentReversalLog)
      {
        PaymentReversalLog paymentReversalLog = (PaymentReversalLog) trans;
        transactionItem.SubItems.Add((object) ServicingEnum.TransactionTypesToUI(paymentReversalLog.ReversalType));
      }
      else
        transactionItem.SubItems.Add((object) ServicingEnum.TransactionTypesToUI(trans.TransactionType));
      switch (trans)
      {
        case SchedulePaymentLog _:
          SchedulePaymentLog schedulePaymentLog = (SchedulePaymentLog) trans;
          transactionItem.SubItems.Add((object) schedulePaymentLog.TransactionDate.ToString("MM/dd/yyyy"));
          transactionItem.SubItems.Add((object) schedulePaymentLog.TotalPaymentDue.ToString("N2"));
          break;
        case LoanPurchaseLog _:
          LoanPurchaseLog loanPurchaseLog = (LoanPurchaseLog) trans;
          transactionItem.SubItems.Add((object) loanPurchaseLog.PurchaseAdviceDate.ToString("MM/dd/yyyy"));
          transactionItem.SubItems.Add((object) loanPurchaseLog.PurchaseAmount.ToString("N2"));
          break;
        default:
          transactionItem.SubItems.Add((object) trans.CreatedDateTime.Date.ToString("MM/dd/yyyy"));
          transactionItem.SubItems.Add((object) trans.TransactionAmount.ToString("N2"));
          break;
      }
      transactionItem.Selected = selected;
      transactionItem.Tag = (object) trans;
      return transactionItem;
    }

    private bool updateTransactionItem(ServicingTransactionBase trans, GVItem item, bool selected)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return false;
      this.loan.UpdateServicingTransaction(trans);
      ServicingTransactionTypes transactionType = trans.TransactionType;
      if (trans is PaymentReversalLog && ((PaymentReversalLog) trans).ReversalType == ServicingTransactionTypes.EscrowDisbursementReversal)
        transactionType = ServicingTransactionTypes.EscrowDisbursementReversal;
      item.SubItems[1].Text = ServicingEnum.TransactionTypesToUI(transactionType);
      GVSubItem subItem = item.SubItems[2];
      DateTime dateTime = trans.CreatedDateTime;
      dateTime = dateTime.Date;
      string str = dateTime.ToString("MM/dd/yyyy");
      subItem.Text = str;
      item.SubItems[3].Text = trans.TransactionAmount.ToString("N2");
      item.Tag = (object) trans;
      item.Selected = selected;
      return true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return;
      if (this.gridViewTransaction.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a transaction first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this transaction?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        ServicingTransactionBase tag1 = (ServicingTransactionBase) this.gridViewTransaction.SelectedItems[0].Tag;
        if (tag1 == null || (tag1.TransactionGUID ?? "") == string.Empty)
          return;
        int index = this.gridViewTransaction.SelectedItems[0].Index;
        GVItem selectedItem = this.gridViewTransaction.SelectedItems[0];
        if (tag1 is PaymentTransactionLog || tag1 is EscrowDisbursementLog)
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
          {
            ServicingTransactionBase tag2 = (ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag;
            if (tag2 is PaymentReversalLog)
            {
              PaymentReversalLog transactionLog = (PaymentReversalLog) tag2;
              if (transactionLog.PaymentGUID == tag1.TransactionGUID)
              {
                try
                {
                  this.loan.RemoveServicingTransaction((ServicingTransactionBase) transactionLog);
                  this.gridViewTransaction.Items.RemoveAt(nItemIndex);
                  break;
                }
                catch (Exception ex)
                {
                  int num2 = (int) Utils.Dialog((IWin32Window) this, "The Reversal Payment associated with this payment cannot be deleted. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
              }
            }
          }
        }
        if (this.loan.RemoveServicingTransaction(tag1))
        {
          this.gridViewTransaction.Items.Remove(selectedItem);
          this.updateSummary(tag1.TransactionType, true, false);
          if (this.gridViewTransaction.Items.Count == 0)
          {
            this.listViewTransaction_SelectedIndexChanged((object) this, (EventArgs) null);
            return;
          }
          if (index + 1 > this.gridViewTransaction.Items.Count)
            this.gridViewTransaction.Items[this.gridViewTransaction.Items.Count - 1].Selected = true;
          else
            this.gridViewTransaction.Items[index].Selected = true;
        }
        this.listViewTransaction_SelectedIndexChanged((object) this, (EventArgs) null);
      }
    }

    private bool updateSummary(
      ServicingTransactionTypes transactionAction,
      bool updateTransactionNo,
      bool updateNextPayment)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      int num1 = 0;
      int num2 = 0;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
      {
        if (updateTransactionNo)
          this.gridViewTransaction.Items[nItemIndex].Text = string.Concat((object) (nItemIndex + 1));
        ServicingTransactionBase tag1 = (ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag;
        if (transactionAction == ServicingTransactionTypes.EscrowDisbursement && tag1.TransactionType == ServicingTransactionTypes.EscrowDisbursement)
        {
          ++num1;
          EscrowDisbursementLog tag2 = (EscrowDisbursementLog) this.gridViewTransaction.Items[nItemIndex].Tag;
          if (tag2.DisbursementNo != num1)
          {
            tag2.DisbursementNo = num1;
            this.updateTransactionItem((ServicingTransactionBase) tag2, this.gridViewTransaction.Items[nItemIndex], false);
          }
        }
        if (transactionAction == ServicingTransactionTypes.Payment && tag1.TransactionType == ServicingTransactionTypes.Payment)
        {
          ++num2;
          PaymentTransactionLog tag3 = (PaymentTransactionLog) this.gridViewTransaction.Items[nItemIndex].Tag;
          if (tag3.PaymentNo != num2)
          {
            tag3.PaymentNo = num2;
            this.updateTransactionItem((ServicingTransactionBase) tag3, this.gridViewTransaction.Items[nItemIndex], false);
          }
        }
      }
      this.loan.Calculator.CalculateInterimServicing(true);
      this.freeScreen_OnFieldChanged((object) null, (EventArgs) null);
      this.freeScreen.RefreshContents();
      Cursor.Current = Cursors.Default;
      return true;
    }

    private int getNextNumber(ServicingTransactionTypes transType)
    {
      int num = 0;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
      {
        if (((ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag).TransactionType == transType)
          ++num;
      }
      return num + 1;
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.gridViewTransaction.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a transaction first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int displayIndex = this.gridViewTransaction.SelectedItems[0].DisplayIndex;
        this.RefreshTransactionList();
        this.gridViewTransaction.Items[displayIndex].Selected = true;
        ServicingTransactionBase tag = (ServicingTransactionBase) this.gridViewTransaction.SelectedItems[0].Tag;
        ServicingTransactionTypes transactionTypes = tag.TransactionType;
        if (tag is PaymentReversalLog && ((PaymentReversalLog) tag).ReversalType == ServicingTransactionTypes.EscrowDisbursementReversal)
          transactionTypes = ServicingTransactionTypes.EscrowDisbursementReversal;
        int transNo = this.gridViewTransaction.SelectedItems[0].Index + 1;
        switch (transactionTypes)
        {
          case ServicingTransactionTypes.Payment:
            if (this.canEdit)
            {
              double maxPrincipal = 0.0;
              double maxInterest = 0.0;
              this.findPaymentLimits((PaymentTransactionLog) tag, ref maxPrincipal, ref maxInterest);
              using (PaymentDetailForm paymentDetailForm = new PaymentDetailForm((PaymentTransactionLog) tag, transNo, -1, maxPrincipal, maxInterest, this.loan, false))
              {
                if (paymentDetailForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                  return;
                if (!this.updateTransactionItem((ServicingTransactionBase) paymentDetailForm.PaymentLog, this.gridViewTransaction.SelectedItems[0], true))
                  return;
                break;
              }
            }
            else
            {
              using (PaymentDetailForm paymentDetailForm = new PaymentDetailForm((PaymentTransactionLog) tag))
              {
                int num2 = (int) paymentDetailForm.ShowDialog((IWin32Window) this);
                return;
              }
            }
          case ServicingTransactionTypes.PaymentReversal:
            using (PaymentReversalSelector reversalSelector = new PaymentReversalSelector((PaymentReversalLog) tag, this.gridViewTransaction, transNo, ServicingTransactionTypes.Payment, !this.canEdit))
            {
              if (reversalSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) reversalSelector.ReversalLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
          case ServicingTransactionTypes.EscrowDisbursement:
            using (EscrowDisbursementForm disbursementForm = new EscrowDisbursementForm((EscrowDisbursementLog) tag, transNo, 1, this.loan, !this.canEdit))
            {
              if (disbursementForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) disbursementForm.DisbursementLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
          case ServicingTransactionTypes.EscrowInterest:
            using (EscrowInterestForm escrowInterestForm = new EscrowInterestForm((EscrowInterestLog) tag, transNo, !this.canEdit))
            {
              if (escrowInterestForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) escrowInterestForm.InterestLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
          case ServicingTransactionTypes.EscrowDisbursementReversal:
            using (PaymentReversalSelector reversalSelector = new PaymentReversalSelector((PaymentReversalLog) tag, this.gridViewTransaction, transNo, ServicingTransactionTypes.EscrowDisbursement, !this.canEdit))
            {
              if (reversalSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) reversalSelector.ReversalLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
          case ServicingTransactionTypes.Other:
            using (OtherDetailForm otherDetailForm = new OtherDetailForm((OtherTransactionLog) tag, transNo, !this.canEdit))
            {
              if (otherDetailForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) otherDetailForm.OtherTransLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
          case ServicingTransactionTypes.SchedulePayment:
            using (SchedulePaymentDetailForm paymentDetailForm = new SchedulePaymentDetailForm((SchedulePaymentLog) tag, this.loan))
            {
              int num3 = (int) paymentDetailForm.ShowDialog((IWin32Window) this);
              return;
            }
          case ServicingTransactionTypes.PurchaseAdvice:
            using (PurchaseAdviceDetailForm adviceDetailForm = new PurchaseAdviceDetailForm((LoanPurchaseLog) tag, this.loan))
            {
              int num4 = (int) adviceDetailForm.ShowDialog((IWin32Window) this);
              return;
            }
          case ServicingTransactionTypes.PrincipalDisbursement:
            using (PrincipalDisbursementForm disbursementForm = new PrincipalDisbursementForm((PrincipalDisbursementLog) tag, transNo, this.loan, !this.canEdit))
            {
              if (disbursementForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (!this.updateTransactionItem((ServicingTransactionBase) disbursementForm.DisbursementLog, this.gridViewTransaction.SelectedItems[0], true))
                return;
              break;
            }
        }
        this.updateSummary(tag.TransactionType, false, false);
      }
    }

    private void findPaymentLimits(
      PaymentTransactionLog selectedPayLog,
      ref double maxPrincipal,
      ref double maxInterest)
    {
      SchedulePaymentLog schedulePaymentLog = (SchedulePaymentLog) null;
      ServicingTransactionBase[] servicingTransactions = this.loan.GetServicingTransactions();
      if (servicingTransactions == null || servicingTransactions.Length == 0)
      {
        maxPrincipal = double.MaxValue;
        maxInterest = double.MaxValue;
      }
      else
      {
        Hashtable hashtable = new Hashtable();
        for (int index = 0; index < servicingTransactions.Length; ++index)
        {
          if (servicingTransactions[index] is PaymentReversalLog)
          {
            PaymentReversalLog paymentReversalLog = (PaymentReversalLog) servicingTransactions[index];
            if (!hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
              hashtable.Add((object) paymentReversalLog.PaymentGUID, (object) "");
          }
          else if (servicingTransactions[index] is SchedulePaymentLog && servicingTransactions[index].TransactionDate == selectedPayLog.PaymentIndexDate)
            schedulePaymentLog = (SchedulePaymentLog) servicingTransactions[index];
        }
        if (schedulePaymentLog == null)
        {
          maxPrincipal = double.MaxValue;
          maxInterest = double.MaxValue;
        }
        else
        {
          schedulePaymentLog.ClearReceivedAmount();
          for (int index = 0; index < servicingTransactions.Length; ++index)
          {
            if (servicingTransactions[index] is PaymentTransactionLog && !hashtable.ContainsKey((object) servicingTransactions[index].TransactionGUID))
            {
              PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactions[index];
              if (!(paymentTransactionLog.PaymentIndexDate != schedulePaymentLog.TransactionDate) && !(selectedPayLog.TransactionGUID == paymentTransactionLog.TransactionGUID))
              {
                schedulePaymentLog.Principal += paymentTransactionLog.Principal;
                schedulePaymentLog.Interest += paymentTransactionLog.Interest;
              }
            }
          }
          maxPrincipal = schedulePaymentLog.PrincipalDue - schedulePaymentLog.Principal;
          maxInterest = schedulePaymentLog.InterestDue - schedulePaymentLog.Interest;
          if (maxPrincipal < 0.0)
            maxPrincipal = 0.0;
          if (maxInterest >= 0.0)
            return;
          maxInterest = 0.0;
        }
      }
    }

    private void btnPrintDetail_Click(object sender, EventArgs e)
    {
      if (this.gridViewTransaction.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a transaction first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        Hashtable hashtable = new Hashtable();
        for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
        {
          ServicingTransactionBase tag = (ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag;
          switch (tag)
          {
            case PaymentTransactionLog _:
            case EscrowDisbursementLog _:
              if (!hashtable.ContainsKey((object) tag.TransactionGUID))
              {
                hashtable.Add((object) tag.TransactionGUID, (object) tag);
                break;
              }
              break;
          }
        }
        ServicingTransactionBase[] transactions = new ServicingTransactionBase[this.gridViewTransaction.SelectedItems.Count];
        ServicingTransactionTypes[] tranTypes = new ServicingTransactionTypes[this.gridViewTransaction.SelectedItems.Count];
        ServicingTransactionBase[] originalTransaction = new ServicingTransactionBase[this.gridViewTransaction.SelectedItems.Count];
        int[] transNo = new int[this.gridViewTransaction.SelectedItems.Count];
        int index = 0;
        for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
        {
          if (this.gridViewTransaction.Items[nItemIndex].Selected)
          {
            ServicingTransactionBase tag = (ServicingTransactionBase) this.gridViewTransaction.Items[nItemIndex].Tag;
            tranTypes[index] = tag.TransactionType;
            originalTransaction[index] = tag;
            if (tag is PaymentReversalLog)
            {
              PaymentReversalLog paymentReversalLog = (PaymentReversalLog) tag;
              if (hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
                tag = (ServicingTransactionBase) hashtable[(object) paymentReversalLog.PaymentGUID];
            }
            transactions[index] = tag;
            transNo[index] = nItemIndex + 1;
            ++index;
          }
        }
        new PdfFormFacade().ProcessInterimTransactionPrint(transactions, tranTypes, originalTransaction, transNo);
        Cursor.Current = Cursors.Default;
        this.Focus();
      }
    }

    private void btnPrintList_Click(object sender, EventArgs e)
    {
      if (this.gridViewTransaction.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You don't have any transaction to print.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        ServicingTransactionBase[] servicingTransactions = this.loan.GetServicingTransactions(true);
        if (servicingTransactions != null && servicingTransactions.Length != 0)
        {
          new PdfFormFacade().ProcessInterimTransactionPrint(servicingTransactions);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You don't have any payment transaction to print.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Cursor.Current = Cursors.Default;
        this.Focus();
      }
    }

    public FrmBrowserHandler BrowserHandler => this.freeScreen.BrowserHandler;

    private void listViewTransaction_DoubleClick(object sender, EventArgs e)
    {
      this.btnView_Click((object) null, (EventArgs) null);
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (this.gridViewTransaction.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You don't have any transaction to export.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        new InterimServicingExportWizard(this.loan.GetServicingTransactions(true)).ShowDialog((IWin32Window) this);
    }

    private void listViewTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = false;
      for (int nItemIndex = 0; nItemIndex < this.gridViewTransaction.Items.Count; ++nItemIndex)
      {
        if (this.gridViewTransaction.Items[nItemIndex].Selected && this.gridViewTransaction.Items[nItemIndex].Tag is LoanPurchaseLog)
          flag = true;
      }
      if (!flag)
      {
        this.iconBtnEdit.Enabled = this.gridViewTransaction.Items.Count > 0 && this.gridViewTransaction.SelectedItems.Count == 1 && this.canEdit;
        this.iconBtnDelete.Enabled = this.gridViewTransaction.Items.Count > 0 && this.gridViewTransaction.SelectedItems.Count == 1 && this.canEdit;
      }
      else
        this.iconBtnEdit.Enabled = this.iconBtnDelete.Enabled = false;
      this.iconBtnExport.Enabled = this.gridViewTransaction.Items.Count > 0 && this.gridViewTransaction.Items.Count > 0;
      this.btnPrintList.Enabled = this.gridViewTransaction.Items.Count > 0;
      this.btnPrintDetail.Enabled = this.gridViewTransaction.SelectedItems.Count == 1 && !flag || this.gridViewTransaction.SelectedItems.Count >= 2;
    }

    private void btnStartServicing_Click(object sender, EventArgs e)
    {
      if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
        return;
      string field1 = this.loan.GetField("SERVICE.X8");
      if (field1 == "Foreclosure" || field1 == "Servicing Released")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You can't start or restart servicing this loan because the loan has been foreclosure or released.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.loan.GetField("SERVICE.X8") != "" && Utils.Dialog((IWin32Window) this, "The account transaction will be cleared. Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        string field2 = this.loan.GetField("682");
        if (field2 == "" || field2 == "//")
        {
          if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "To use Interim Servicing, you have to setup the First Payment Date on Regz-TIL input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "To use Interim Servicing, you have to setup the First Payment Date on RegZ-LE input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        else
        {
          if (this.loan.GetField("2626") == "Correspondent")
          {
            string field3 = this.loan.GetField("3579");
            string field4 = this.loan.GetField("3567");
            string field5 = this.loan.GetField("2");
            if (field3 == "" || Utils.ToDouble(field3) <= 0.0 || field5 == "" || Utils.ToDouble(field5) <= 0.0 || field4 == "" || field4 == "//")
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "To Start Servicing on a correspondent loan there must be a Purchased Principal/Unpaid Principal Balance as a beginning balance. To populate this amount the Correspondent Purchase Advice must contain at least a Purchase Date, a Loan Amount and a Purchased Principal prior to Start Servicing.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          Cursor.Current = Cursors.WaitCursor;
          if (!this.loan.StartInterimServicing())
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "Interim Servicing cannot be started. Please make sure the monthly payment field is not blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            this.RefreshTransactionList();
            this.ChangeButtonStatus(true);
            this.freeScreen.RefreshContents();
            if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_IS_RecopyLoan))
              this.btnStartServicing.Enabled = false;
            this.freeScreen_OnFieldChanged((object) null, (EventArgs) null);
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    private void freeScreen_OnFieldChanged(object sender, EventArgs e)
    {
      this.labelHeader.Text = "Interim Servicing Worksheet";
      this.labelHeader.ForeColor = Color.Black;
      this.gradientPanelHeader.GradientColor1 = Color.FromArgb(233, 242, (int) byte.MaxValue);
      this.gradientPanelHeader.GradientColor2 = Color.FromArgb(207, 221, 243);
      if (this.loan == null || this.loan.IsTemplate || this.loan.IsInFindFieldForm)
        return;
      this.paySnapshot = this.loan.GetPaymentScheduleSnapshot();
      if (this.paySnapshot == null)
        return;
      this.freeScreen.SetTitleAlert(false, "");
      string empty = string.Empty;
      double num = Utils.ParseDouble((object) this.loan.GetField("SERVICE.X57"));
      DateTime dateTime = DateTime.Today.AddDays((double) this.daysAlertPrint);
      if (num > 0.0 && this.loan.GetField("SERVICE.X13") != string.Empty && this.loan.GetField("SERVICE.X14") != this.loan.GetField("SERVICE.X10") && Utils.ParseDate((object) this.loan.GetField("SERVICE.X13")).Date <= dateTime.Date)
        empty += "Statement Due! ";
      dateTime = this.loan.Calculator.GetISWCutoffDate().AddDays((double) this.daysAlertDue);
      if (num > 0.0 && this.loan.GetField("SERVICE.X15") != string.Empty && Utils.ParseDate((object) this.loan.GetField("SERVICE.X15")).Date <= dateTime.Date)
        empty += "Payment Past Due! ";
      bool flag = false;
      dateTime = DateTime.Today.AddDays((double) this.daysAlertEscrow);
      for (int index = 59; index <= 73; index += 2)
      {
        if (this.loan.GetField("SERVICE.X" + index.ToString()) != string.Empty && this.loan.GetField("SERVICE.X" + index.ToString()) != "//" && Utils.ParseDate((object) this.loan.GetField("SERVICE.X" + index.ToString())) <= dateTime)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        empty += "Disbursement Due!";
      if (!(empty != string.Empty))
        return;
      this.labelHeader.Text = "Interim Servicing Worksheet " + empty;
      this.labelHeader.ForeColor = Color.White;
      this.gradientPanelHeader.GradientColor1 = AppColors.AlertRed;
      this.gradientPanelHeader.GradientColor2 = AppColors.AlertRed;
    }

    public static bool HasLink(string formId) => formId.ToUpper() == "INTERIMSERVICING";

    private void borderPanelBody_SizeChanged(object sender, EventArgs e)
    {
      this.borderPanelBody.AutoScrollPosition = new Point(0, 0);
      this.panelAll.Location = new Point(1, 1);
    }

    public string GetHelpTargetName() => "Interim Servicing Worksheet";

    private void btnAddConmment_Click(object sender, EventArgs e)
    {
      CommentDlg commentDlg = new CommentDlg();
      int num = (int) commentDlg.ShowDialog();
      if (commentDlg.DialogResult != DialogResult.OK)
        return;
      this.RefreshComments();
    }

    private void RefreshComments()
    {
      List<Comment> toComments = CommentDlg.ParseToComments(Session.LoanData.GetField("SERVICE.Comments"));
      this.richTextBox_Comments.Clear();
      for (int index = toComments.Count - 1; index >= 0; --index)
      {
        string str = string.Format("{0} {1} >> ", (object) toComments[index].UserName, (object) toComments[index].LogDate.ToString("MM/dd/yyyy hh:mm tt"));
        string text = string.Format("{0}{1}\n\n", (object) str, (object) toComments[index].CommentText);
        int length1 = this.richTextBox_Comments.Text.Length;
        int length2 = str.Length;
        this.richTextBox_Comments.AppendText(text);
        this.richTextBox_Comments.SelectionStart = length1;
        this.richTextBox_Comments.SelectionLength = length2;
        this.richTextBox_Comments.SelectionFont = new Font(this.Font, FontStyle.Bold);
      }
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.iconBtnAdd = new StandardIconButton();
      this.iconBtnEdit = new StandardIconButton();
      this.iconBtnDelete = new StandardIconButton();
      this.iconBtnExport = new StandardIconButton();
      this.borderPanelForBorder = new BorderPanel();
      this.borderPanelBody = new BorderPanel();
      this.panelAll = new Panel();
      this.borderPanelList = new BorderPanel();
      this.groupContainerList = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gridViewTransaction = new GridView();
      this.btnPrintList = new Button();
      this.btnPrintDetail = new Button();
      this.panelGridRightSection = new Panel();
      this.panelComments = new Panel();
      this.panelCommentsRight = new Panel();
      this.groupContainer_Comments = new GroupContainer();
      this.richTextBox_Comments = new RichTextBox();
      this.btnAddConmment = new Button();
      this.panelTop = new Panel();
      this.pictureBox1 = new PictureBox();
      this.gradientPanelHeader = new GradientPanel();
      this.btnStartServicing = new Button();
      this.labelHeader = new Label();
      ((ISupportInitialize) this.iconBtnAdd).BeginInit();
      ((ISupportInitialize) this.iconBtnEdit).BeginInit();
      ((ISupportInitialize) this.iconBtnDelete).BeginInit();
      ((ISupportInitialize) this.iconBtnExport).BeginInit();
      this.borderPanelForBorder.SuspendLayout();
      this.borderPanelBody.SuspendLayout();
      this.panelAll.SuspendLayout();
      this.borderPanelList.SuspendLayout();
      this.groupContainerList.SuspendLayout();
      this.panelComments.SuspendLayout();
      this.groupContainer_Comments.SuspendLayout();
      this.panelTop.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.gradientPanelHeader.SuspendLayout();
      this.SuspendLayout();
      this.iconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnAdd.BackColor = Color.Transparent;
      this.iconBtnAdd.Location = new Point(434, 6);
      this.iconBtnAdd.MouseDownImage = (Image) null;
      this.iconBtnAdd.Name = "iconBtnAdd";
      this.iconBtnAdd.Size = new Size(16, 16);
      this.iconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.iconBtnAdd.TabIndex = 11;
      this.iconBtnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnAdd, "Add Transaction");
      this.iconBtnAdd.Click += new EventHandler(this.btnNew_Click);
      this.iconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnEdit.BackColor = Color.Transparent;
      this.iconBtnEdit.Location = new Point(454, 6);
      this.iconBtnEdit.MouseDownImage = (Image) null;
      this.iconBtnEdit.Name = "iconBtnEdit";
      this.iconBtnEdit.Size = new Size(16, 16);
      this.iconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.iconBtnEdit.TabIndex = 12;
      this.iconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnEdit, "Edit Transaction");
      this.iconBtnEdit.Click += new EventHandler(this.btnView_Click);
      this.iconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnDelete.BackColor = Color.Transparent;
      this.iconBtnDelete.Location = new Point(474, 6);
      this.iconBtnDelete.MouseDownImage = (Image) null;
      this.iconBtnDelete.Name = "iconBtnDelete";
      this.iconBtnDelete.Size = new Size(16, 16);
      this.iconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnDelete.TabIndex = 13;
      this.iconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnDelete, "Delete Transaction");
      this.iconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.iconBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnExport.BackColor = Color.Transparent;
      this.iconBtnExport.Location = new Point(494, 6);
      this.iconBtnExport.MouseDownImage = (Image) null;
      this.iconBtnExport.Name = "iconBtnExport";
      this.iconBtnExport.Size = new Size(16, 16);
      this.iconBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.iconBtnExport.TabIndex = 16;
      this.iconBtnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnExport, "Export Transaction");
      this.iconBtnExport.Click += new EventHandler(this.btnExport_Click);
      this.borderPanelForBorder.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanelForBorder.Controls.Add((Control) this.borderPanelBody);
      this.borderPanelForBorder.Dock = DockStyle.Fill;
      this.borderPanelForBorder.Location = new Point(0, 26);
      this.borderPanelForBorder.Name = "borderPanelForBorder";
      this.borderPanelForBorder.Size = new Size(706, 609);
      this.borderPanelForBorder.TabIndex = 33;
      this.borderPanelBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanelBody.AutoScroll = true;
      this.borderPanelBody.Borders = AnchorStyles.None;
      this.borderPanelBody.Controls.Add((Control) this.panelAll);
      this.borderPanelBody.Location = new Point(1, 0);
      this.borderPanelBody.Name = "borderPanelBody";
      this.borderPanelBody.Size = new Size(705, 596);
      this.borderPanelBody.TabIndex = 32;
      this.borderPanelBody.SizeChanged += new EventHandler(this.borderPanelBody_SizeChanged);
      this.panelAll.BackColor = Color.WhiteSmoke;
      this.panelAll.Controls.Add((Control) this.borderPanelList);
      this.panelAll.Controls.Add((Control) this.panelTop);
      this.panelAll.Location = new Point(0, 1);
      this.panelAll.Name = "panelAll";
      this.panelAll.Size = new Size(702, 1770);
      this.panelAll.TabIndex = 29;
      this.borderPanelList.BackColor = Color.WhiteSmoke;
      this.borderPanelList.Borders = AnchorStyles.None;
      this.borderPanelList.Controls.Add((Control) this.groupContainerList);
      this.borderPanelList.Controls.Add((Control) this.panelGridRightSection);
      this.borderPanelList.Controls.Add((Control) this.panelComments);
      this.borderPanelList.Dock = DockStyle.Fill;
      this.borderPanelList.Location = new Point(0, 1375);
      this.borderPanelList.Name = "borderPanelList";
      this.borderPanelList.Padding = new Padding(4, 0, 0, 0);
      this.borderPanelList.Size = new Size(702, 395);
      this.borderPanelList.TabIndex = 2;
      this.groupContainerList.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerList.Controls.Add((Control) this.iconBtnExport);
      this.groupContainerList.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainerList.Controls.Add((Control) this.gridViewTransaction);
      this.groupContainerList.Controls.Add((Control) this.iconBtnDelete);
      this.groupContainerList.Controls.Add((Control) this.iconBtnEdit);
      this.groupContainerList.Controls.Add((Control) this.iconBtnAdd);
      this.groupContainerList.Controls.Add((Control) this.btnPrintList);
      this.groupContainerList.Controls.Add((Control) this.btnPrintDetail);
      this.groupContainerList.Dock = DockStyle.Left;
      this.groupContainerList.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerList.Location = new Point(4, 0);
      this.groupContainerList.Name = "groupContainerList";
      this.groupContainerList.Size = new Size(675, 295);
      this.groupContainerList.TabIndex = 1;
      this.groupContainerList.Text = "Account Transaction";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(508, 6);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 15;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.gridViewTransaction.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 31;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Transaction Type";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Transaction Date";
      gvColumn3.Width = 180;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Transaction Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 160;
      this.gridViewTransaction.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridViewTransaction.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewTransaction.Location = new Point(3, 26);
      this.gridViewTransaction.Name = "gridViewTransaction";
      this.gridViewTransaction.Size = new Size(671, 264);
      this.gridViewTransaction.SortOption = GVSortOption.None;
      this.gridViewTransaction.TabIndex = 14;
      this.gridViewTransaction.SelectedIndexChanged += new EventHandler(this.listViewTransaction_SelectedIndexChanged);
      this.gridViewTransaction.DoubleClick += new EventHandler(this.listViewTransaction_DoubleClick);
      this.btnPrintList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrintList.Location = new Point(595, 2);
      this.btnPrintList.Name = "btnPrintList";
      this.btnPrintList.Size = new Size(75, 22);
      this.btnPrintList.TabIndex = 9;
      this.btnPrintList.Text = "Print List";
      this.btnPrintList.UseVisualStyleBackColor = true;
      this.btnPrintList.Click += new EventHandler(this.btnPrintList_Click);
      this.btnPrintDetail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrintDetail.Location = new Point(520, 2);
      this.btnPrintDetail.Name = "btnPrintDetail";
      this.btnPrintDetail.Size = new Size(75, 22);
      this.btnPrintDetail.TabIndex = 8;
      this.btnPrintDetail.Text = "Print Details";
      this.btnPrintDetail.UseVisualStyleBackColor = true;
      this.btnPrintDetail.Click += new EventHandler(this.btnPrintDetail_Click);
      this.panelGridRightSection.Dock = DockStyle.Right;
      this.panelGridRightSection.Location = new Point(679, 0);
      this.panelGridRightSection.Name = "panelGridRightSection";
      this.panelGridRightSection.Size = new Size(23, 295);
      this.panelGridRightSection.TabIndex = 17;
      this.panelComments.Controls.Add((Control) this.panelCommentsRight);
      this.panelComments.Controls.Add((Control) this.groupContainer_Comments);
      this.panelComments.Dock = DockStyle.Bottom;
      this.panelComments.Location = new Point(4, 295);
      this.panelComments.Name = "panelComments";
      this.panelComments.Size = new Size(698, 100);
      this.panelComments.TabIndex = 5;
      this.panelCommentsRight.Dock = DockStyle.Right;
      this.panelCommentsRight.Location = new Point(675, 0);
      this.panelCommentsRight.Name = "panelCommentsRight";
      this.panelCommentsRight.Size = new Size(23, 100);
      this.panelCommentsRight.TabIndex = 18;
      this.groupContainer_Comments.Controls.Add((Control) this.richTextBox_Comments);
      this.groupContainer_Comments.Controls.Add((Control) this.btnAddConmment);
      this.groupContainer_Comments.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer_Comments.Location = new Point(0, 0);
      this.groupContainer_Comments.Name = "groupContainer_Comments";
      this.groupContainer_Comments.Padding = new Padding(5, 0, 0, 0);
      this.groupContainer_Comments.Size = new Size(675, 100);
      this.groupContainer_Comments.TabIndex = 14;
      this.groupContainer_Comments.Text = "Comments";
      this.richTextBox_Comments.Location = new Point(4, 29);
      this.richTextBox_Comments.Name = "richTextBox_Comments";
      this.richTextBox_Comments.ReadOnly = true;
      this.richTextBox_Comments.Size = new Size(667, 66);
      this.richTextBox_Comments.TabIndex = 4;
      this.richTextBox_Comments.Text = "";
      this.btnAddConmment.Location = new Point(582, 3);
      this.btnAddConmment.Name = "btnAddConmment";
      this.btnAddConmment.Size = new Size(88, 22);
      this.btnAddConmment.TabIndex = 3;
      this.btnAddConmment.Text = "Add Comment";
      this.btnAddConmment.UseVisualStyleBackColor = true;
      this.btnAddConmment.Click += new EventHandler(this.btnAddConmment_Click);
      this.panelTop.BackColor = Color.WhiteSmoke;
      this.panelTop.Controls.Add((Control) this.pictureBox1);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(702, 1375);
      this.panelTop.TabIndex = 0;
      this.pictureBox1.BackColor = Color.WhiteSmoke;
      this.pictureBox1.Dock = DockStyle.Right;
      this.pictureBox1.Location = new Point(679, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(23, 1375);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.gradientPanelHeader.Controls.Add((Control) this.btnStartServicing);
      this.gradientPanelHeader.Controls.Add((Control) this.labelHeader);
      this.gradientPanelHeader.Dock = DockStyle.Top;
      this.gradientPanelHeader.Location = new Point(0, 0);
      this.gradientPanelHeader.Name = "gradientPanelHeader";
      this.gradientPanelHeader.Size = new Size(706, 26);
      this.gradientPanelHeader.TabIndex = 31;
      this.btnStartServicing.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStartServicing.BackColor = SystemColors.Control;
      this.btnStartServicing.Location = new Point(598, 2);
      this.btnStartServicing.Name = "btnStartServicing";
      this.btnStartServicing.Size = new Size(103, 22);
      this.btnStartServicing.TabIndex = 30;
      this.btnStartServicing.Text = "&Start Servicing";
      this.btnStartServicing.UseVisualStyleBackColor = true;
      this.btnStartServicing.Click += new EventHandler(this.btnStartServicing_Click);
      this.labelHeader.AutoSize = true;
      this.labelHeader.BackColor = Color.Transparent;
      this.labelHeader.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelHeader.Location = new Point(4, 6);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new Size(165, 14);
      this.labelHeader.TabIndex = 31;
      this.labelHeader.Text = "Interim Servicing Worksheet";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.borderPanelForBorder);
      this.Controls.Add((Control) this.gradientPanelHeader);
      this.Name = nameof (ServicingDetailsWorksheet);
      this.Size = new Size(706, 635);
      ((ISupportInitialize) this.iconBtnAdd).EndInit();
      ((ISupportInitialize) this.iconBtnEdit).EndInit();
      ((ISupportInitialize) this.iconBtnDelete).EndInit();
      ((ISupportInitialize) this.iconBtnExport).EndInit();
      this.borderPanelForBorder.ResumeLayout(false);
      this.borderPanelBody.ResumeLayout(false);
      this.panelAll.ResumeLayout(false);
      this.borderPanelList.ResumeLayout(false);
      this.groupContainerList.ResumeLayout(false);
      this.panelComments.ResumeLayout(false);
      this.groupContainer_Comments.ResumeLayout(false);
      this.panelTop.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.gradientPanelHeader.ResumeLayout(false);
      this.gradientPanelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
