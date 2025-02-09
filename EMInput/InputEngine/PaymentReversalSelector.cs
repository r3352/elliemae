// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PaymentReversalSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PaymentReversalSelector : Form
  {
    private ServicingTransactionTypes transType;
    private PaymentReversalLog reversalLog;
    private IContainer components;
    private Label label1;
    private Label labelNo;
    private Button btnCancel;
    private Button btnOK;
    private Label labelModifiedBy;
    private Label labelCreatedBy;
    protected EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private GridView listViewTransaction;

    public PaymentReversalSelector(
      PaymentReversalLog reversalLog,
      GridView originalTransactionList,
      int transNo,
      ServicingTransactionTypes transType,
      bool viewOnly)
    {
      this.reversalLog = reversalLog;
      this.transType = transType;
      this.InitializeComponent();
      this.initForm(originalTransactionList, viewOnly);
      this.labelNo.Text = "T" + transNo.ToString("00");
      switch (transType)
      {
        case ServicingTransactionTypes.Payment:
          this.labelNo.Text += " Payment Reversal";
          this.emHelpLink1.Visible = true;
          this.emHelpLink1.HelpTag = "Interum Payment Reversal";
          break;
        case ServicingTransactionTypes.EscrowDisbursement:
          this.labelNo.Text += " Escrow Disbursement Reversal";
          this.emHelpLink1.Visible = true;
          this.emHelpLink1.HelpTag = "Interum Escrow Disbursement Reversal";
          this.label1.Text = "Select an escrow disbursement to reverse:";
          this.listViewTransaction.Columns[0].Text = "Disbursement #";
          this.listViewTransaction.Columns[1].Text = "Disbursement Received Date";
          this.listViewTransaction.Columns[2].Text = "Disbursement Amount";
          break;
      }
      if (this.reversalLog != null)
        this.labelCreatedBy.Text = "Created by " + this.reversalLog.CreatedByName + " on " + this.reversalLog.CreatedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      else
        this.labelCreatedBy.Text = "Created by " + Session.UserInfo.FullName + " on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
      if (this.reversalLog != null && this.reversalLog.ModifiedByName != "")
        this.labelModifiedBy.Text = "Last modified by " + this.reversalLog.ModifiedByName + " on " + this.reversalLog.ModifiedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      else
        this.labelModifiedBy.Text = "";
      if (!viewOnly)
        return;
      this.btnOK.Visible = false;
      this.btnCancel.Text = "OK";
      this.Text = "View Transaction";
    }

    private void initForm(GridView originalTransactionList, bool viewOnly)
    {
      this.listViewTransaction.Items.Clear();
      if (originalTransactionList.Items.Count == 0)
        return;
      Hashtable hashtable = new Hashtable();
      for (int nItemIndex = 0; nItemIndex < originalTransactionList.Items.Count; ++nItemIndex)
      {
        ServicingTransactionBase tag = (ServicingTransactionBase) originalTransactionList.Items[nItemIndex].Tag;
        if (tag is PaymentReversalLog)
        {
          PaymentReversalLog paymentReversalLog = (PaymentReversalLog) tag;
          if ((this.reversalLog == null || !(this.reversalLog.TransactionGUID == paymentReversalLog.TransactionGUID)) && !hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
            hashtable.Add((object) paymentReversalLog.PaymentGUID, (object) paymentReversalLog);
        }
      }
      int transNo = 0;
      this.listViewTransaction.BeginUpdate();
      for (int nItemIndex = 0; nItemIndex < originalTransactionList.Items.Count; ++nItemIndex)
      {
        ServicingTransactionBase tag = (ServicingTransactionBase) originalTransactionList.Items[nItemIndex].Tag;
        if (tag.TransactionType == this.transType)
        {
          ++transNo;
          if (!hashtable.ContainsKey((object) tag.TransactionGUID))
          {
            bool selected = false;
            if (this.reversalLog != null && this.reversalLog.PaymentGUID == tag.TransactionGUID)
              selected = true;
            if (!viewOnly || selected)
              this.listViewTransaction.Items.Add(this.createTransactionItem(transNo, tag, selected));
          }
        }
      }
      this.listViewTransaction.EndUpdate();
    }

    private GVItem createTransactionItem(
      int transNo,
      ServicingTransactionBase trans,
      bool selected)
    {
      GVItem transactionItem = new GVItem(transNo.ToString());
      GVSubItemCollection subItems = transactionItem.SubItems;
      DateTime dateTime = trans.CreatedDateTime;
      dateTime = dateTime.Date;
      string str = dateTime.ToString("MM/dd/yyyy");
      subItems.Add((object) str);
      transactionItem.SubItems.Add((object) trans.TransactionAmount.ToString("N2"));
      transactionItem.Selected = selected;
      transactionItem.Tag = (object) trans;
      return transactionItem;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.listViewTransaction.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a payment first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        ServicingTransactionBase tag = (ServicingTransactionBase) this.listViewTransaction.SelectedItems[0].Tag;
        if (this.reversalLog != null)
        {
          if (this.reversalLog.PaymentGUID != tag.TransactionGUID)
            this.reversalLog.PaymentGUID = tag.TransactionGUID;
          this.reversalLog.ModifiedByID = Session.UserInfo.Userid;
          this.reversalLog.ModifiedByName = Session.UserInfo.FullName;
          this.reversalLog.ModifiedDateTime = DateTime.Now;
        }
        else
        {
          this.reversalLog = new PaymentReversalLog();
          this.reversalLog.CreatedByID = Session.UserInfo.Userid;
          this.reversalLog.CreatedByName = Session.UserInfo.FullName;
          this.reversalLog.CreatedDateTime = DateTime.Now;
          this.reversalLog.PaymentGUID = tag.TransactionGUID;
          this.reversalLog.TransactionDate = DateTime.Today;
          this.reversalLog.ReversalType = this.transType != ServicingTransactionTypes.Payment ? ServicingTransactionTypes.EscrowDisbursementReversal : ServicingTransactionTypes.PaymentReversal;
        }
        this.reversalLog.TransactionAmount = tag.TransactionAmount * -1.0;
        this.DialogResult = DialogResult.OK;
      }
    }

    public PaymentReversalLog ReversalLog => this.reversalLog;

    public bool HasTransactionToReversal => this.listViewTransaction.Items.Count != 0;

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
      this.label1 = new Label();
      this.labelNo = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.labelModifiedBy = new Label();
      this.labelCreatedBy = new Label();
      this.groupContainer1 = new GroupContainer();
      this.listViewTransaction = new GridView();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 36);
      this.label1.Name = "label1";
      this.label1.Size = new Size(139, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Select a payment to reverse";
      this.labelNo.AutoSize = true;
      this.labelNo.BackColor = Color.Transparent;
      this.labelNo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelNo.ForeColor = SystemColors.Highlight;
      this.labelNo.Location = new Point(9, 7);
      this.labelNo.Name = "labelNo";
      this.labelNo.Size = new Size(135, 13);
      this.labelNo.TabIndex = 0;
      this.labelNo.Text = "T01 Payment Reversal";
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(359, 244);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(279, 244);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Location = new Point(10, 313);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 111;
      this.emHelpLink1.Visible = false;
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(10, 293);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 79;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(10, 273);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 78;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.labelNo);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.listViewTransaction);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.btnOK);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(445, 349);
      this.groupContainer1.TabIndex = 1;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvHeaderPayment";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Payment #";
      gvColumn1.Width = 88;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvHeaderDate";
      gvColumn2.SortMethod = GVSortMethod.Date;
      gvColumn2.Text = "Payment Received Date";
      gvColumn2.Width = 160;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvHeaderAmount";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Payment Amount";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 129;
      this.listViewTransaction.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewTransaction.Location = new Point(10, 53);
      this.listViewTransaction.Name = "listViewTransaction";
      this.listViewTransaction.Size = new Size(423, 185);
      this.listViewTransaction.TabIndex = 11;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(445, 349);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PaymentReversalSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
