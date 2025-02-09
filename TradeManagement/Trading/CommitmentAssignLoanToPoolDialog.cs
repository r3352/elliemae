// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CommitmentAssignLoanToPoolDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CommitmentAssignLoanToPoolDialog : Form
  {
    private List<string> loanNumbers;
    private TradeAssignmentByTradeBase[] commitmentsInPool;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private TextBox txtCommitmentContractNum;
    private Label lblCommitment;
    private Label label1;
    private ComboBox cmbProductName;
    private Label lblLoansSelected;

    public string CommitmentContractNumber { get; set; }

    public new string ProductName { get; set; }

    public CommitmentAssignLoanToPoolDialog(List<string> products)
    {
      this.InitializeComponent();
      this.cmbProductName.DataSource = (object) products;
    }

    public CommitmentAssignLoanToPoolDialog(
      List<string> loanNumbers,
      List<string> products,
      TradeAssignmentByTradeBase[] commitmentsInPool)
      : this(products)
    {
      this.lblLoansSelected.Text = string.Format("You've selected {0} {1}.", (object) loanNumbers.Count.ToString(), loanNumbers.Count > 1 ? (object) "loans" : (object) "loan");
      this.loanNumbers = loanNumbers;
      this.commitmentsInPool = commitmentsInPool;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.CommitmentContractNumber = this.txtCommitmentContractNum.Text;
      this.ProductName = this.cmbProductName.Text;
      if (!string.IsNullOrEmpty(this.txtCommitmentContractNum.Text))
      {
        AssignmentErrorCode assignmentErrorCode = this.Validate();
        if (assignmentErrorCode == AssignmentErrorCode.None)
        {
          this.DialogResult = DialogResult.OK;
        }
        else
        {
          switch (assignmentErrorCode - 1)
          {
            case AssignmentErrorCode.None:
              if (Utils.Dialog((IWin32Window) this, "The Commitment Contract number is invalid. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK)
              {
                this.CommitmentContractNumber = string.Empty;
                this.DialogResult = DialogResult.OK;
                break;
              }
              this.txtCommitmentContractNum.Focus();
              break;
            case AssignmentErrorCode.InvalidContractNumber:
              if (Utils.Dialog((IWin32Window) this, string.Format("{0} is an invalid Product Name of the GSE Commitment {1}. Do you want to continue?", (object) this.ProductName, (object) this.CommitmentContractNumber), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK)
              {
                this.ProductName = string.Empty;
                this.DialogResult = DialogResult.OK;
                break;
              }
              this.cmbProductName.Focus();
              break;
            case AssignmentErrorCode.InvalidProductName:
              string str = string.Join(", ", this.loanNumbers.ToArray());
              int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Loan {0} not assigned because the GSE Commitment {1} is not allocated to this Fannie Mae PE MBS Pool.", this.loanNumbers.Count <= 1 ? (object) (str + " was") : (object) (str + " were"), (object) this.CommitmentContractNumber), MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.txtCommitmentContractNum.Focus();
              break;
          }
        }
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public AssignmentErrorCode Validate()
    {
      string contractNumber = this.txtCommitmentContractNum.Text.Trim();
      string productName = this.cmbProductName.Text.Trim();
      if (contractNumber == string.Empty)
        return AssignmentErrorCode.None;
      List<GSECommitmentInfo> source = Session.GseCommitmentManager.ValidateContractNumbers(new List<string>()
      {
        contractNumber
      });
      if (source.Count == 0)
        return AssignmentErrorCode.InvalidContractNumber;
      GSECommitmentInfo theCommitment = new GSECommitmentInfo();
      if (contractNumber != null && contractNumber.Trim() != string.Empty && source != null)
        theCommitment = source.Where<GSECommitmentInfo>((Func<GSECommitmentInfo, bool>) (c => c.ContractNumber == contractNumber)).FirstOrDefault<GSECommitmentInfo>();
      if (contractNumber != string.Empty && (this.commitmentsInPool != null && !((IEnumerable<TradeAssignmentByTradeBase>) this.commitmentsInPool).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (c =>
      {
        int? tradeId1 = c.TradeID;
        int tradeId2 = theCommitment.TradeID;
        return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
      })) || this.commitmentsInPool == null))
        return AssignmentErrorCode.CommitmentNotAllocated;
      return productName != string.Empty && theCommitment != null && !theCommitment.ProductNames.Any<FannieMaeProduct>((Func<FannieMaeProduct, bool>) (pn => pn.ProductName == productName)) ? AssignmentErrorCode.InvalidProductName : AssignmentErrorCode.None;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.txtCommitmentContractNum = new TextBox();
      this.lblCommitment = new Label();
      this.label1 = new Label();
      this.cmbProductName = new ComboBox();
      this.lblLoansSelected = new Label();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(234, 114);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(93, 23);
      this.btnCancel.TabIndex = 27;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Location = new Point(133, 114);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(93, 23);
      this.btnOK.TabIndex = 25;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtCommitmentContractNum.Location = new Point(139, 52);
      this.txtCommitmentContractNum.Name = "txtCommitmentContractNum";
      this.txtCommitmentContractNum.Size = new Size(188, 20);
      this.txtCommitmentContractNum.TabIndex = 21;
      this.lblCommitment.AutoSize = true;
      this.lblCommitment.Location = new Point(9, 55);
      this.lblCommitment.Name = "lblCommitment";
      this.lblCommitment.Size = new Size(117, 13);
      this.lblCommitment.TabIndex = 16;
      this.lblCommitment.Text = "Commitment Contract #";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 83);
      this.label1.Name = "label1";
      this.label1.Size = new Size(75, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Product Name";
      this.cmbProductName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbProductName.FormattingEnabled = true;
      this.cmbProductName.Location = new Point(139, 79);
      this.cmbProductName.Name = "cmbProductName";
      this.cmbProductName.Size = new Size(188, 21);
      this.cmbProductName.TabIndex = 28;
      this.lblLoansSelected.AutoSize = true;
      this.lblLoansSelected.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLoansSelected.Location = new Point(9, 21);
      this.lblLoansSelected.Name = "lblLoansSelected";
      this.lblLoansSelected.Size = new Size(0, 13);
      this.lblLoansSelected.TabIndex = 29;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(339, 155);
      this.Controls.Add((Control) this.lblLoansSelected);
      this.Controls.Add((Control) this.cmbProductName);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtCommitmentContractNum);
      this.Controls.Add((Control) this.lblCommitment);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CommitmentAssignLoanToPoolDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Assign Loans to Pool";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
