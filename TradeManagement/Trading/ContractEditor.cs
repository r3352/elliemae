// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.ContractEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class ContractEditor : UserControl, IMenuProvider
  {
    private static TradeStatusEnumNameProvider tradeStatusNameProvider = new TradeStatusEnumNameProvider();
    private MasterContractInfo contractInfo;
    private bool modified;
    private string originalContractNumber;
    private Investor selectedInvestor;
    private IContainer components;
    private Label label1;
    private TextBox txtNumber;
    private TextBox txtInvestorNumber;
    private Label label2;
    private TextBox txtInvestor;
    private TextBox txtContractAmt;
    private Label label4;
    private TextBox txtTolerance;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private DatePicker dtStartDate;
    private DatePicker dtEndDate;
    private ComboBox cboTerm;
    private Panel pnlTrades;
    private GridView gvTrades;
    private Button btnCreateTrade;
    private ContractProfitControl ctlProfit;
    private Label label3;
    private TextBox txtTerm;
    private Label label11;
    private TableContainer grpTrades;
    private StandardIconButton btnInvestorTemplate;
    private GroupContainer grpEditor;
    private GroupContainer grpDetails;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnSave;
    private ToolTip toolTip1;
    private StandardIconButton btnReset;
    private Button btnCreateMbsPool;

    public event EventHandler ContractSaved;

    public ContractEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtContractAmt, TextBoxContentRule.NonNegativeInteger, "#,##0");
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeDecimal, "#,##0.00;;\"\"");
      this.gvTrades.Sort(0, SortOrder.Ascending);
    }

    public MasterContractInfo CurrentContract
    {
      get => this.contractInfo;
      set
      {
        this.contractInfo = value;
        if (value == null)
          return;
        this.loadContractData();
      }
    }

    public bool DataModified => this.modified;

    public void SetReadOnly()
    {
      this.btnCreateMbsPool.Enabled = false;
      this.btnCreateTrade.Enabled = false;
      this.btnInvestorTemplate.Enabled = false;
      this.txtNumber.ReadOnly = true;
      this.txtInvestorNumber.ReadOnly = true;
      this.txtInvestor.ReadOnly = true;
      this.btnInvestorTemplate.Enabled = false;
      this.txtContractAmt.ReadOnly = true;
      this.txtTolerance.ReadOnly = true;
      this.dtStartDate.ReadOnly = true;
      this.dtEndDate.ReadOnly = true;
      this.cboTerm.Enabled = false;
      this.txtTerm.ReadOnly = true;
      this.btnSave.Enabled = false;
      this.btnReset.Enabled = false;
    }

    private void loadContractData()
    {
      this.originalContractNumber = this.contractInfo.ContractNumber;
      this.txtNumber.Text = this.contractInfo.ContractNumber;
      this.txtInvestorNumber.Text = this.contractInfo.InvestorContractNumber;
      this.txtInvestor.Text = this.contractInfo.InvestorName;
      this.txtContractAmt.Text = this.contractInfo.ContractAmount.ToString("#,###");
      this.txtTolerance.Text = this.contractInfo.Tolerance == 0M ? "" : this.contractInfo.Tolerance.ToString("#,##0.00;;\"\"");
      this.dtStartDate.Value = this.contractInfo.StartDate;
      this.dtEndDate.Value = this.contractInfo.EndDate;
      this.cboTerm.SelectedIndex = (int) this.contractInfo.Term;
      this.txtTerm.Text = string.Concat(this.cboTerm.SelectedItem);
      this.loadTradeList();
      bool flag = this.contractInfo.Status == 1;
      this.txtNumber.ReadOnly = flag;
      this.txtInvestorNumber.ReadOnly = flag;
      this.txtInvestor.ReadOnly = flag;
      this.btnInvestorTemplate.Enabled = !flag;
      this.txtContractAmt.ReadOnly = flag;
      this.txtTolerance.ReadOnly = flag;
      this.dtStartDate.ReadOnly = flag;
      this.dtEndDate.ReadOnly = flag;
      this.cboTerm.Visible = !flag;
      this.txtTerm.Visible = flag;
      this.btnInvestorTemplate.Enabled = !flag;
      this.btnCreateTrade.Visible = !flag;
      this.btnCreateMbsPool.Visible = !flag;
      this.btnSave.Visible = !flag;
      this.btnReset.Visible = !flag;
      this.btnSave.Enabled = false;
      this.btnReset.Enabled = false;
      this.setEditorTitle();
      this.modified = false;
    }

    private void loadTradeList()
    {
      this.gvTrades.Items.Clear();
      LoanTradeViewModel[] loanTradeViewModelArray = new LoanTradeViewModel[0];
      if (this.contractInfo.ContractID > 0)
      {
        foreach (LoanTradeViewModel trade in Session.LoanTradeManager.GetTradeViewsByContractID(this.contractInfo.ContractID))
          this.gvTrades.Items.Add(this.createTradeListViewItem(trade));
        foreach (MbsPoolViewModel trade in Session.MbsPoolManager.GetTradeViewsByContractID(this.contractInfo.ContractID))
          this.gvTrades.Items.Add(this.createTradeListViewItem(trade));
      }
      this.ctlProfit.Calculate(this.gvTrades.Items);
    }

    private GVItem createTradeListViewItem(LoanTradeViewModel trade)
    {
      return new GVItem()
      {
        Text = trade.Name,
        SubItems = {
          (object) ContractEditor.tradeStatusNameProvider.GetName((object) trade.Status),
          trade.TargetDeliveryDate == DateTime.MinValue ? (object) "" : (object) trade.TargetDeliveryDate.ToString("MM/dd/yyyy"),
          (object) trade.TradeAmount.ToString("#,##0"),
          (object) trade.AssignedAmount.ToString("#,##0"),
          (object) trade.NetProfit.ToString("#,##0")
        },
        Tag = (object) trade
      };
    }

    private GVItem createTradeListViewItem(MbsPoolViewModel trade)
    {
      return new GVItem()
      {
        Text = trade.Name,
        SubItems = {
          (object) ContractEditor.tradeStatusNameProvider.GetName((object) trade.Status),
          trade.TargetDeliveryDate == DateTime.MinValue ? (object) "" : (object) trade.TargetDeliveryDate.ToString("MM/dd/yyyy"),
          (object) trade.TradeAmount.ToString("#,##0"),
          (object) trade.AssignedAmount.ToString("#,##0"),
          (object) trade.NetProfit.ToString("#,##0")
        },
        Tag = (object) trade
      };
    }

    private void commitChanges()
    {
      this.contractInfo.ContractNumber = this.txtNumber.Text;
      this.contractInfo.InvestorContractNumber = this.txtInvestorNumber.Text;
      if (this.selectedInvestor != null && this.txtInvestor.Text == this.selectedInvestor.Name)
        this.contractInfo.Investor = this.selectedInvestor;
      else if (this.txtInvestor.Text != this.contractInfo.Investor.Name)
        this.contractInfo.Investor = new Investor();
      this.contractInfo.Investor.Name = this.txtInvestor.Text;
      this.contractInfo.ContractAmount = Utils.ParseDecimal((object) this.txtContractAmt.Text);
      this.contractInfo.Tolerance = Utils.ParseDecimal((object) this.txtTolerance.Text);
      this.contractInfo.StartDate = this.dtStartDate.Value;
      this.contractInfo.EndDate = this.dtEndDate.Value;
      this.contractInfo.Term = (MasterContractTerm) this.cboTerm.SelectedIndex;
    }

    public bool SaveContract()
    {
      if (!this.btnSave.Visible)
        return true;
      if (!this.validateContractData())
        return false;
      this.commitChanges();
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        int contractId = this.contractInfo.ContractID;
        if (this.contractInfo.ContractID < 0)
          contractId = Session.MasterContractManager.CreateContract(this.contractInfo);
        else
          Session.MasterContractManager.UpdateContract(this.contractInfo);
        this.contractInfo = Session.MasterContractManager.GetContract(contractId);
        this.loadContractData();
        if (this.ContractSaved != null)
          this.ContractSaved((object) this, EventArgs.Empty);
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error has occurred while attempting to save this contract: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool validateContractData()
    {
      string contractNumber = this.txtNumber.Text.Trim();
      if (contractNumber == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A contract number must be provided for this contract.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (contractNumber != this.originalContractNumber && Session.MasterContractManager.GetContractByContractNumber(contractNumber) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A contract already exists with the number '" + contractNumber + "'. You must provide a unique name/number for this contract.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!(this.dtStartDate.Value != DateTime.MinValue) || !(this.dtEndDate.Value != DateTime.MinValue) || !(this.dtEndDate.Value < this.dtStartDate.Value))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The contract's End Date must occur on or after its Start Date.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void btnCreateTrade_Click(object sender, EventArgs e)
    {
      if (this.contractInfo == null || !this.QuerySaveContract(true))
        return;
      LoanTradeInfo trade = new LoanTradeInfo();
      trade.Investor.CopyFrom(this.contractInfo.Investor);
      trade.ContractID = this.contractInfo.ContractID;
      trade.InvestorCommitmentNumber = this.contractInfo.InvestorContractNumber;
      TradeManagementConsole.Instance.OpenTrade(trade, 0);
    }

    private void btnCreateMbsPool_Click(object sender, EventArgs e)
    {
      if (this.contractInfo == null || !this.QuerySaveContract(true))
        return;
      MbsPoolInfo trade = new MbsPoolInfo();
      trade.Investor.CopyFrom(this.contractInfo.Investor);
      trade.ContractID = this.contractInfo.ContractID;
      MbsPoolMortgageType poolMortgageType = MbsPoolMortgageType.None;
      using (MbsPoolMortgageTypeDialog mortgageTypeDialog = new MbsPoolMortgageTypeDialog())
      {
        if (mortgageTypeDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          poolMortgageType = mortgageTypeDialog.PoolMortgageType;
      }
      if (poolMortgageType == MbsPoolMortgageType.None)
        return;
      trade.PoolMortgageType = poolMortgageType;
      TradeManagementConsole.Instance.OpenMbsPool(trade);
    }

    public bool QuerySaveContract(bool required)
    {
      if (!this.modified)
        return true;
      return (!required ? Utils.Dialog((IWin32Window) this, "Save your changes to the current contract?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) : Utils.Dialog((IWin32Window) this, "The contract must be saved before performing this action. Save your changes now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.No ? !required : this.SaveContract();
    }

    private void btnSave_Click(object sender, EventArgs e) => this.SaveContract();

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      this.modified = this.btnSave.Enabled = this.btnReset.Enabled = true;
    }

    private void btnInvestorTemplate_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector(true))
      {
        if (templateSelector.ShowDialog() != DialogResult.OK)
          return;
        InvestorTemplate selectedTemplate = templateSelector.SelectedTemplate;
        this.txtInvestor.Text = selectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.selectedInvestor = selectedTemplate.CompanyInformation;
      }
    }

    private void txtNumber_Validated(object sender, EventArgs e) => this.setEditorTitle();

    private void setEditorTitle()
    {
      string str = "Master Contract Details";
      if (this.txtNumber.Text != "")
        str = str + " - " + this.txtNumber.Text;
      this.grpEditor.Text = str;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to discard you changes to the current Contract?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.loadContractData();
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "MC_Save":
          this.btnSave_Click((object) null, (EventArgs) null);
          break;
        case "MC_CreateTrade":
          this.btnCreateTrade_Click((object) null, (EventArgs) null);
          break;
        case "MC_CreateMBSPool":
          this.btnCreateMbsPool_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "MC_Save":
          stateControl = (Control) this.btnSave;
          break;
        case "MC_CreateTrade":
          stateControl = (Control) this.btnCreateTrade;
          break;
        case "MC_CreateMBSPool":
          stateControl = (Control) this.btnCreateMbsPool;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
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
      this.label1 = new Label();
      this.txtNumber = new TextBox();
      this.txtInvestorNumber = new TextBox();
      this.label2 = new Label();
      this.txtInvestor = new TextBox();
      this.txtContractAmt = new TextBox();
      this.label4 = new Label();
      this.txtTolerance = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.cboTerm = new ComboBox();
      this.btnInvestorTemplate = new StandardIconButton();
      this.label11 = new Label();
      this.label3 = new Label();
      this.txtTerm = new TextBox();
      this.dtStartDate = new DatePicker();
      this.dtEndDate = new DatePicker();
      this.pnlTrades = new Panel();
      this.grpTrades = new TableContainer();
      this.gvTrades = new GridView();
      this.ctlProfit = new ContractProfitControl();
      this.btnCreateTrade = new Button();
      this.grpEditor = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnCreateMbsPool = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.grpDetails = new GroupContainer();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.btnInvestorTemplate).BeginInit();
      this.pnlTrades.SuspendLayout();
      this.grpTrades.SuspendLayout();
      this.grpEditor.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(93, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Master Contract #";
      this.txtNumber.Location = new Point(115, 36);
      this.txtNumber.MaxLength = 64;
      this.txtNumber.Name = "txtNumber";
      this.txtNumber.Size = new Size(120, 20);
      this.txtNumber.TabIndex = 1;
      this.txtNumber.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtNumber.Validated += new EventHandler(this.txtNumber_Validated);
      this.txtInvestorNumber.Location = new Point(115, 58);
      this.txtInvestorNumber.MaxLength = 64;
      this.txtInvestorNumber.Name = "txtInvestorNumber";
      this.txtInvestorNumber.Size = new Size(120, 20);
      this.txtInvestorNumber.TabIndex = 2;
      this.txtInvestorNumber.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 62);
      this.label2.Name = "label2";
      this.label2.Size = new Size(73, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Investor MC #";
      this.txtInvestor.Location = new Point(115, 80);
      this.txtInvestor.MaxLength = 64;
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.Size = new Size(100, 20);
      this.txtInvestor.TabIndex = 4;
      this.txtInvestor.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtContractAmt.Location = new Point(115, 102);
      this.txtContractAmt.MaxLength = 12;
      this.txtContractAmt.Name = "txtContractAmt";
      this.txtContractAmt.Size = new Size(100, 20);
      this.txtContractAmt.TabIndex = 5;
      this.txtContractAmt.TextAlign = HorizontalAlignment.Right;
      this.txtContractAmt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 106);
      this.label4.Name = "label4";
      this.label4.Size = new Size(87, 14);
      this.label4.TabIndex = 6;
      this.label4.Text = "Contract Amount";
      this.txtTolerance.Location = new Point(115, 124);
      this.txtTolerance.MaxLength = 6;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(100, 20);
      this.txtTolerance.TabIndex = 6;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.txtTolerance.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 129);
      this.label5.Name = "label5";
      this.label5.Size = new Size(54, 14);
      this.label5.TabIndex = 8;
      this.label5.Text = "Tolerance";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 153);
      this.label6.Name = "label6";
      this.label6.Size = new Size(55, 14);
      this.label6.TabIndex = 10;
      this.label6.Text = "Start Date";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 178);
      this.label7.Name = "label7";
      this.label7.Size = new Size(50, 14);
      this.label7.TabIndex = 12;
      this.label7.Text = "End Date";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 200);
      this.label8.Name = "label8";
      this.label8.Size = new Size(30, 14);
      this.label8.TabIndex = 14;
      this.label8.Text = "Term";
      this.cboTerm.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTerm.FormattingEnabled = true;
      this.cboTerm.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Monthly",
        (object) "Quarterly",
        (object) "Annually"
      });
      this.cboTerm.Location = new Point(115, 194);
      this.cboTerm.Name = "cboTerm";
      this.cboTerm.Size = new Size(120, 22);
      this.cboTerm.TabIndex = 9;
      this.cboTerm.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.btnInvestorTemplate.BackColor = Color.Transparent;
      this.btnInvestorTemplate.Location = new Point(218, 81);
      this.btnInvestorTemplate.MouseDownImage = (Image) null;
      this.btnInvestorTemplate.Name = "btnInvestorTemplate";
      this.btnInvestorTemplate.Size = new Size(16, 16);
      this.btnInvestorTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnInvestorTemplate.TabIndex = 21;
      this.btnInvestorTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnInvestorTemplate, "Select Investor");
      this.btnInvestorTemplate.Click += new EventHandler(this.btnInvestorTemplate_Click);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(8, 86);
      this.label11.Name = "label11";
      this.label11.Size = new Size(46, 14);
      this.label11.TabIndex = 20;
      this.label11.Text = "Investor";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(216, 128);
      this.label3.Name = "label3";
      this.label3.Size = new Size(17, 14);
      this.label3.TabIndex = 16;
      this.label3.Text = "%";
      this.txtTerm.Location = new Point(115, 194);
      this.txtTerm.MaxLength = 64;
      this.txtTerm.Name = "txtTerm";
      this.txtTerm.ReadOnly = true;
      this.txtTerm.Size = new Size(115, 20);
      this.txtTerm.TabIndex = 19;
      this.dtStartDate.BackColor = SystemColors.Window;
      this.dtStartDate.Location = new Point(115, 146);
      this.dtStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtStartDate.Name = "dtStartDate";
      this.dtStartDate.Size = new Size(120, 22);
      this.dtStartDate.TabIndex = 7;
      this.dtStartDate.ToolTip = "";
      this.dtStartDate.Value = new DateTime(0L);
      this.dtStartDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.dtEndDate.BackColor = SystemColors.Window;
      this.dtEndDate.Location = new Point(115, 170);
      this.dtEndDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtEndDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtEndDate.Name = "dtEndDate";
      this.dtEndDate.Size = new Size(120, 22);
      this.dtEndDate.TabIndex = 8;
      this.dtEndDate.ToolTip = "";
      this.dtEndDate.Value = new DateTime(0L);
      this.dtEndDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.pnlTrades.Controls.Add((Control) this.grpTrades);
      this.pnlTrades.Dock = DockStyle.Fill;
      this.pnlTrades.Location = new Point(251, 31);
      this.pnlTrades.Name = "pnlTrades";
      this.pnlTrades.Padding = new Padding(5, 0, 0, 0);
      this.pnlTrades.Size = new Size(591, 367);
      this.pnlTrades.TabIndex = 2;
      this.grpTrades.Controls.Add((Control) this.gvTrades);
      this.grpTrades.Controls.Add((Control) this.ctlProfit);
      this.grpTrades.Dock = DockStyle.Fill;
      this.grpTrades.Location = new Point(5, 0);
      this.grpTrades.Name = "grpTrades";
      this.grpTrades.Size = new Size(586, 367);
      this.grpTrades.TabIndex = 3;
      this.grpTrades.Text = "Currently in Contract";
      this.gvTrades.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Trade/Pool ID";
      gvColumn1.Width = 98;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 85;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Target Delivery";
      gvColumn3.Width = 95;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Trade/Pool Amt";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 96;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Assigned Amt";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 88;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Net Profit";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 78;
      this.gvTrades.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvTrades.Dock = DockStyle.Fill;
      this.gvTrades.Location = new Point(1, 26);
      this.gvTrades.Name = "gvTrades";
      this.gvTrades.Size = new Size(584, 315);
      this.gvTrades.TabIndex = 0;
      this.gvTrades.TabStop = false;
      this.ctlProfit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlProfit.BackColor = Color.Transparent;
      this.ctlProfit.Location = new Point(6, 346);
      this.ctlProfit.Name = "ctlProfit";
      this.ctlProfit.Size = new Size(574, 19);
      this.ctlProfit.TabIndex = 2;
      this.btnCreateTrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateTrade.BackColor = SystemColors.Control;
      this.btnCreateTrade.Location = new Point(72, 0);
      this.btnCreateTrade.Margin = new Padding(0);
      this.btnCreateTrade.Name = "btnCreateTrade";
      this.btnCreateTrade.Size = new Size(93, 22);
      this.btnCreateTrade.TabIndex = 1;
      this.btnCreateTrade.Text = "&Create Trade";
      this.btnCreateTrade.UseVisualStyleBackColor = true;
      this.btnCreateTrade.Click += new EventHandler(this.btnCreateTrade_Click);
      this.grpEditor.Borders = AnchorStyles.Top;
      this.grpEditor.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpEditor.Controls.Add((Control) this.pnlTrades);
      this.grpEditor.Controls.Add((Control) this.grpDetails);
      this.grpEditor.Dock = DockStyle.Fill;
      this.grpEditor.HeaderForeColor = SystemColors.ControlText;
      this.grpEditor.Location = new Point(0, 0);
      this.grpEditor.Name = "grpEditor";
      this.grpEditor.Padding = new Padding(5);
      this.grpEditor.Size = new Size(847, 403);
      this.grpEditor.TabIndex = 3;
      this.grpEditor.Text = "Master Contract Details";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCreateMbsPool);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCreateTrade);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnReset);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(572, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(270, 22);
      this.flowLayoutPanel1.TabIndex = 3;
      this.btnCreateMbsPool.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreateMbsPool.BackColor = SystemColors.Control;
      this.btnCreateMbsPool.Location = new Point(165, 0);
      this.btnCreateMbsPool.Margin = new Padding(0);
      this.btnCreateMbsPool.Name = "btnCreateMbsPool";
      this.btnCreateMbsPool.Size = new Size(105, 22);
      this.btnCreateMbsPool.TabIndex = 5;
      this.btnCreateMbsPool.Text = "Create &MBS Pool";
      this.btnCreateMbsPool.UseVisualStyleBackColor = true;
      this.btnCreateMbsPool.Click += new EventHandler(this.btnCreateMbsPool_Click);
      this.verticalSeparator1.Location = new Point(67, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 2;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Enabled = false;
      this.btnReset.Location = new Point(46, 3);
      this.btnReset.Margin = new Padding(3, 3, 2, 3);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 4;
      this.btnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnReset, "Reset Contract");
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(25, 3);
      this.btnSave.Margin = new Padding(3, 3, 2, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 3;
      this.btnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSave, "Save Contract");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.grpDetails.Controls.Add((Control) this.btnInvestorTemplate);
      this.grpDetails.Controls.Add((Control) this.label1);
      this.grpDetails.Controls.Add((Control) this.label11);
      this.grpDetails.Controls.Add((Control) this.dtEndDate);
      this.grpDetails.Controls.Add((Control) this.txtTolerance);
      this.grpDetails.Controls.Add((Control) this.dtStartDate);
      this.grpDetails.Controls.Add((Control) this.txtContractAmt);
      this.grpDetails.Controls.Add((Control) this.label5);
      this.grpDetails.Controls.Add((Control) this.label3);
      this.grpDetails.Controls.Add((Control) this.label4);
      this.grpDetails.Controls.Add((Control) this.label6);
      this.grpDetails.Controls.Add((Control) this.cboTerm);
      this.grpDetails.Controls.Add((Control) this.txtInvestor);
      this.grpDetails.Controls.Add((Control) this.txtNumber);
      this.grpDetails.Controls.Add((Control) this.label7);
      this.grpDetails.Controls.Add((Control) this.label2);
      this.grpDetails.Controls.Add((Control) this.label8);
      this.grpDetails.Controls.Add((Control) this.txtInvestorNumber);
      this.grpDetails.Controls.Add((Control) this.txtTerm);
      this.grpDetails.Dock = DockStyle.Left;
      this.grpDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpDetails.Location = new Point(5, 31);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(246, 367);
      this.grpDetails.TabIndex = 0;
      this.grpDetails.Text = "Contract Info";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.grpEditor);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ContractEditor);
      this.Size = new Size(847, 403);
      ((ISupportInitialize) this.btnInvestorTemplate).EndInit();
      this.pnlTrades.ResumeLayout(false);
      this.grpTrades.ResumeLayout(false);
      this.grpEditor.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
