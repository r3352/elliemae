// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECmtAssignmentDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class GSECmtAssignmentDialog : Form
  {
    private ITradeEditorBase tradeEditor;
    private GSEcmtAssignmentListScreen ctlTradeList;
    private FieldFilterList filters;
    private IContainer components;
    private Panel pnlTop;
    private Label lblHelp;
    private Panel pnlTradeList;
    private Panel pnlSearch;
    private GroupContainer grpHeader;
    private Panel pnlSearchCriteria;
    private FlowLayoutPanel flpHeader;
    private Button btnSearch;
    private Button btnClear;
    private Panel panel1;
    private CollapsibleSplitter collapsibleSplitter1;
    private TextBox txt_contractnumber;
    private TextBox txt_cmtID;
    private Label label_issuemonth;
    private Label label_cmtamount;
    private Label label_tradedesc;
    private Label label_Contract;
    private Label label_cmtID;
    private Label label1;
    private TextBox txt_cmtamount2;
    private TextBox txt_cmtamount1;
    private ComboBox cmb_tradedescription;
    private DatePicker dt_issuemonth;

    public GSECmtAssignmentDialog()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txt_cmtamount1, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txt_cmtamount2, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      this.LoadConfigurableFieldOptions();
      this.ctlTradeList = new GSEcmtAssignmentListScreen(this.tradeEditor, true);
      this.pnlTradeList.Controls.Clear();
      this.pnlTradeList.Controls.Add((Control) this.ctlTradeList);
      this.ctlTradeList.Dock = DockStyle.Fill;
      this.ctlTradeList.AssignedClicked += new EventHandler(this.ctlTradeList_AssignedClicked);
      this.InitForm();
    }

    public GSECmtAssignmentDialog(ITradeEditorBase tradeEditor)
      : this()
    {
      this.tradeEditor = tradeEditor;
      this.ctlTradeList = new GSEcmtAssignmentListScreen(this.tradeEditor, true);
      this.pnlTradeList.Controls.Clear();
      this.pnlTradeList.Controls.Add((Control) this.ctlTradeList);
      this.ctlTradeList.Dock = DockStyle.Fill;
      this.ctlTradeList.AssignedClicked += new EventHandler(this.ctlTradeList_AssignedClicked);
      this.InitForm();
    }

    private void InitForm()
    {
      if (!(this.tradeEditor is GseCommitmentEditor))
        return;
      this.Text = "Allocate Fannie Mae PE MBS Pools";
      this.lblHelp.Text = "You may allocate multiple Fannie Mae PE MBS pools to GSE Commitment";
    }

    public bool DataModified => this.ctlTradeList.DataModified;

    public TradeAssignmentByTradeBase[] GetCurrentAssignments()
    {
      return this.ctlTradeList.GetCurrentAssignments();
    }

    public TradeAssignmentByTradeBase[] GetEligibleAssignments()
    {
      return this.ctlTradeList.GetEligibleAssignments();
    }

    public void RefreshData(
      TradeAssignmentByTradeBase[] currentAssignments,
      TradeAssignmentByTradeBase[] eligibleAssignments)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.ctlTradeList.RefreshData(currentAssignments, eligibleAssignments, this.filters);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void appendCriteria()
    {
      this.filters = (FieldFilterList) null;
      this.appendCriteria(FieldTypes.IsString, "GseCommitmentDetails.Name", this.txt_cmtID.Text);
      this.appendCriteria(FieldTypes.IsString, "GseCommitmentDetails.ContractNumber", this.txt_contractnumber.Text);
      this.appendCriteria(FieldTypes.IsString, "GseCommitmentDetails.TradeDescription", this.cmb_tradedescription.Text);
      this.appendCriteria(FieldTypes.IsNumeric, "GseCommitmentDetails.CommitmentAmount", this.txt_cmtamount1.Text, this.txt_cmtamount2.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsDate, "GseCommitmentDetails.IssueMonth", this.dt_issuemonth.Text);
      if (!(this.tradeEditor is GseCommitmentEditor))
        return;
      this.appendCriteria(FieldTypes.IsNumeric, "MbsPoolDetails.PoolMortgageType", 4.ToString());
    }

    private void appendCriteria(
      FieldTypes fieldType,
      string fieldName,
      string valueFrom,
      string valueTo = null,
      OperatorTypes operatorType = OperatorTypes.IsExact)
    {
      if (string.IsNullOrEmpty(valueFrom))
        return;
      if (this.filters == null)
        this.filters = new FieldFilterList();
      FieldFilter fieldFilter = !string.IsNullOrEmpty(valueTo) ? new FieldFilter(fieldType, fieldName, fieldName, fieldName, operatorType, valueFrom, valueTo) : new FieldFilter(fieldType, fieldName, fieldName, fieldName, operatorType, valueFrom);
      bool flag = false;
      for (int index = 0; index < this.filters.Count; ++index)
      {
        if (this.filters[index].CriterionName == fieldName)
        {
          this.filters[index] = fieldFilter;
          flag = true;
        }
      }
      if (!flag)
        this.filters.Add(fieldFilter);
      for (int index = 0; index < this.filters.Count - 1; ++index)
      {
        if (this.filters[index].JointToken == JointTokens.Nothing)
          this.filters[index].JointToken = JointTokens.And;
      }
    }

    private void LoadConfigurableFieldOptions()
    {
      ArrayList secondaryFields = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields == null)
        return;
      foreach (string str in secondaryFields)
        this.cmb_tradedescription.Items.Add((object) str);
    }

    private void ctlTradeList_AssignedClicked(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.appendCriteria();
      this.RefreshData(this.ctlTradeList.GetCurrentAssignments(), this.ctlTradeList.GetEligibleAssignments());
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.filters = (FieldFilterList) null;
      foreach (object control in (ArrangedElementCollection) this.pnlSearchCriteria.Controls)
      {
        switch (control)
        {
          case TextBox _:
            ((Control) control).Text = "";
            continue;
          case DatePicker _:
            ((Control) control).Text = "";
            continue;
          case ComboBox _:
            ((Control) control).Text = "";
            continue;
          default:
            continue;
        }
      }
      this.RefreshData(this.ctlTradeList.GetCurrentAssignments(), this.ctlTradeList.GetEligibleAssignments());
    }

    private void txt_cmtamt_Leave(object sender, EventArgs e)
    {
      if (this.txt_cmtamount1.Text == "")
        this.txt_cmtamount1.Text = "0";
      if (this.txt_cmtamount1.Text == "" || this.txt_cmtamount2.Text == "" || Utils.ParseDouble((object) this.txt_cmtamount2.Text) >= Utils.ParseDouble((object) this.txt_cmtamount1.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum commitment amount must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTop = new Panel();
      this.lblHelp = new Label();
      this.pnlTradeList = new Panel();
      this.pnlSearch = new Panel();
      this.grpHeader = new GroupContainer();
      this.flpHeader = new FlowLayoutPanel();
      this.btnClear = new Button();
      this.btnSearch = new Button();
      this.pnlSearchCriteria = new Panel();
      this.dt_issuemonth = new DatePicker();
      this.label1 = new Label();
      this.txt_cmtamount2 = new TextBox();
      this.txt_cmtamount1 = new TextBox();
      this.cmb_tradedescription = new ComboBox();
      this.txt_contractnumber = new TextBox();
      this.txt_cmtID = new TextBox();
      this.label_issuemonth = new Label();
      this.label_cmtamount = new Label();
      this.label_tradedesc = new Label();
      this.label_Contract = new Label();
      this.label_cmtID = new Label();
      this.panel1 = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlTop.SuspendLayout();
      this.pnlSearch.SuspendLayout();
      this.grpHeader.SuspendLayout();
      this.flpHeader.SuspendLayout();
      this.pnlSearchCriteria.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlTop.Controls.Add((Control) this.lblHelp);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(684, 29);
      this.pnlTop.TabIndex = 0;
      this.lblHelp.AutoSize = true;
      this.lblHelp.Location = new Point(4, 7);
      this.lblHelp.Name = "lblHelp";
      this.lblHelp.Size = new Size(284, 13);
      this.lblHelp.TabIndex = 0;
      this.lblHelp.Text = "You may assign multiple security trades to pool commitment";
      this.pnlTradeList.Dock = DockStyle.Fill;
      this.pnlTradeList.Location = new Point(0, 177);
      this.pnlTradeList.Name = "pnlTradeList";
      this.pnlTradeList.Size = new Size(684, 249);
      this.pnlTradeList.TabIndex = 1;
      this.pnlSearch.Controls.Add((Control) this.grpHeader);
      this.pnlSearch.Dock = DockStyle.Top;
      this.pnlSearch.Location = new Point(0, 0);
      this.pnlSearch.Name = "pnlSearch";
      this.pnlSearch.Size = new Size(684, 170);
      this.pnlSearch.TabIndex = 2;
      this.grpHeader.Controls.Add((Control) this.flpHeader);
      this.grpHeader.Controls.Add((Control) this.pnlSearchCriteria);
      this.grpHeader.Dock = DockStyle.Fill;
      this.grpHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpHeader.HeaderForeColor = SystemColors.ControlText;
      this.grpHeader.Location = new Point(0, 0);
      this.grpHeader.Name = "grpHeader";
      this.grpHeader.Size = new Size(684, 170);
      this.grpHeader.TabIndex = 4;
      this.grpHeader.Text = "Filter";
      this.flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpHeader.BackColor = Color.Transparent;
      this.flpHeader.Controls.Add((Control) this.btnClear);
      this.flpHeader.Controls.Add((Control) this.btnSearch);
      this.flpHeader.FlowDirection = FlowDirection.RightToLeft;
      this.flpHeader.Location = new Point(206, 2);
      this.flpHeader.Name = "flpHeader";
      this.flpHeader.Size = new Size(475, 22);
      this.flpHeader.TabIndex = 1;
      this.btnClear.Location = new Point(400, 0);
      this.btnClear.Margin = new Padding(0);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 1;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.btnSearch.Location = new Point(325, 0);
      this.btnSearch.Margin = new Padding(0);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(75, 23);
      this.btnSearch.TabIndex = 0;
      this.btnSearch.Text = "Filter";
      this.btnSearch.TextAlign = ContentAlignment.TopCenter;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.pnlSearchCriteria.Controls.Add((Control) this.dt_issuemonth);
      this.pnlSearchCriteria.Controls.Add((Control) this.label1);
      this.pnlSearchCriteria.Controls.Add((Control) this.txt_cmtamount2);
      this.pnlSearchCriteria.Controls.Add((Control) this.txt_cmtamount1);
      this.pnlSearchCriteria.Controls.Add((Control) this.cmb_tradedescription);
      this.pnlSearchCriteria.Controls.Add((Control) this.txt_contractnumber);
      this.pnlSearchCriteria.Controls.Add((Control) this.txt_cmtID);
      this.pnlSearchCriteria.Controls.Add((Control) this.label_issuemonth);
      this.pnlSearchCriteria.Controls.Add((Control) this.label_cmtamount);
      this.pnlSearchCriteria.Controls.Add((Control) this.label_tradedesc);
      this.pnlSearchCriteria.Controls.Add((Control) this.label_Contract);
      this.pnlSearchCriteria.Controls.Add((Control) this.label_cmtID);
      this.pnlSearchCriteria.Dock = DockStyle.Fill;
      this.pnlSearchCriteria.Location = new Point(1, 26);
      this.pnlSearchCriteria.Name = "pnlSearchCriteria";
      this.pnlSearchCriteria.Size = new Size(682, 143);
      this.pnlSearchCriteria.TabIndex = 0;
      this.dt_issuemonth.BackColor = SystemColors.Window;
      this.dt_issuemonth.CustomFormat = "MM/yyyy";
      this.dt_issuemonth.Location = new Point(156, 100);
      this.dt_issuemonth.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dt_issuemonth.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dt_issuemonth.Name = "dt_issuemonth";
      this.dt_issuemonth.Size = new Size(104, 22);
      this.dt_issuemonth.TabIndex = 181;
      this.dt_issuemonth.ToolTip = "";
      this.dt_issuemonth.Value = new DateTime(0L);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(263, 78);
      this.label1.Name = "label1";
      this.label1.Size = new Size(11, 14);
      this.label1.TabIndex = 10;
      this.label1.Text = "-";
      this.txt_cmtamount2.Location = new Point(280, 78);
      this.txt_cmtamount2.Name = "txt_cmtamount2";
      this.txt_cmtamount2.Size = new Size(100, 20);
      this.txt_cmtamount2.TabIndex = 9;
      this.txt_cmtamount2.Leave += new EventHandler(this.txt_cmtamt_Leave);
      this.txt_cmtamount1.Location = new Point(156, 78);
      this.txt_cmtamount1.Name = "txt_cmtamount1";
      this.txt_cmtamount1.Size = new Size(100, 20);
      this.txt_cmtamount1.TabIndex = 8;
      this.txt_cmtamount1.Leave += new EventHandler(this.txt_cmtamt_Leave);
      this.cmb_tradedescription.FormattingEnabled = true;
      this.cmb_tradedescription.Location = new Point(156, 56);
      this.cmb_tradedescription.Name = "cmb_tradedescription";
      this.cmb_tradedescription.Size = new Size(184, 22);
      this.cmb_tradedescription.TabIndex = 7;
      this.txt_contractnumber.Location = new Point(156, 34);
      this.txt_contractnumber.Name = "txt_contractnumber";
      this.txt_contractnumber.Size = new Size(131, 20);
      this.txt_contractnumber.TabIndex = 6;
      this.txt_cmtID.Location = new Point(156, 12);
      this.txt_cmtID.Name = "txt_cmtID";
      this.txt_cmtID.Size = new Size(131, 20);
      this.txt_cmtID.TabIndex = 5;
      this.label_issuemonth.AutoSize = true;
      this.label_issuemonth.Location = new Point(32, 104);
      this.label_issuemonth.Name = "label_issuemonth";
      this.label_issuemonth.Size = new Size(65, 14);
      this.label_issuemonth.TabIndex = 4;
      this.label_issuemonth.Text = "Issue Month";
      this.label_cmtamount.AutoSize = true;
      this.label_cmtamount.Location = new Point(32, 82);
      this.label_cmtamount.Name = "label_cmtamount";
      this.label_cmtamount.Size = new Size(103, 14);
      this.label_cmtamount.TabIndex = 3;
      this.label_cmtamount.Text = "Commitment Amount";
      this.label_tradedesc.AutoSize = true;
      this.label_tradedesc.Location = new Point(32, 59);
      this.label_tradedesc.Name = "label_tradedesc";
      this.label_tradedesc.Size = new Size(95, 14);
      this.label_tradedesc.TabIndex = 2;
      this.label_tradedesc.Text = "Trade Description ";
      this.label_Contract.AutoSize = true;
      this.label_Contract.Location = new Point(32, 35);
      this.label_Contract.Name = "label_Contract";
      this.label_Contract.Size = new Size(57, 14);
      this.label_Contract.TabIndex = 1;
      this.label_Contract.Text = "Contract #";
      this.label_cmtID.AutoSize = true;
      this.label_cmtID.Location = new Point(32, 12);
      this.label_cmtID.Name = "label_cmtID";
      this.label_cmtID.Size = new Size(76, 14);
      this.label_cmtID.TabIndex = 0;
      this.label_cmtID.Text = "Commitment ID";
      this.panel1.Controls.Add((Control) this.pnlTradeList);
      this.panel1.Controls.Add((Control) this.collapsibleSplitter1);
      this.panel1.Controls.Add((Control) this.pnlSearch);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 29);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(684, 426);
      this.panel1.TabIndex = 4;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlSearch;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 170);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 4;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(684, 455);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlTop);
      this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
      this.Name = nameof (GSECmtAssignmentDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Allocate GSE Commitments";
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlSearch.ResumeLayout(false);
      this.grpHeader.ResumeLayout(false);
      this.flpHeader.ResumeLayout(false);
      this.pnlSearchCriteria.ResumeLayout(false);
      this.pnlSearchCriteria.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
