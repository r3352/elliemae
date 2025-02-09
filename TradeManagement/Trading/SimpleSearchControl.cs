// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SimpleSearchControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SimpleSearchControl : UserControl
  {
    private int borderPadding = 5;
    private bool modified;
    private bool loading = true;
    private bool readOnly;
    private IContainer components;
    private Label label2;
    private TextBox txtMinNoteRate;
    private TextBox txtMaxNoteRate;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txtMaxTerm;
    private TextBox txtMinTerm;
    private Label label7;
    private Label label8;
    private Label label9;
    private TextBox txtMaxLTV;
    private TextBox txtMinLTV;
    private Label label10;
    private Label label11;
    private CheckBox chkOccPrimary;
    private CheckBox chkOccSecondary;
    private CheckBox chkOccInvestment;
    private CheckBox chkInvFlow;
    private CheckBox chkInvBulk;
    private CheckBox chkInvUnassigned;
    private Label label12;
    private Panel pnlFields;
    private Panel pnlCheckFields;
    private Label label15;
    private Label label16;
    private Label label17;
    private Label label14;
    private Label label13;
    private Label label1;
    private GroupContainer grpLPs;
    private StandardIconButton btnRemoveLP;
    private StandardIconButton btnAddLP;
    private GroupContainer grpMilestones;
    private GroupContainer grpDetails;
    private GridView gvMilestones;
    private GridView gvLPs;
    private Panel pnlBody;
    private GroupContainer grpHeader;
    private FlowLayoutPanel flpHeader;
    private ToolTip toolTip1;
    private PanelEx panelEx1;
    private TableLayoutPanel tableLayoutPanel1;

    public event EventHandler DataChange;

    public SimpleSearchControl()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinNoteRate, TextBoxContentRule.NonNegativeDecimal, "0.000");
      TextBoxFormatter.Attach(this.txtMaxNoteRate, TextBoxContentRule.NonNegativeDecimal, "0.000");
      TextBoxFormatter.Attach(this.txtMinTerm, TextBoxContentRule.NonNegativeInteger, "0");
      TextBoxFormatter.Attach(this.txtMaxTerm, TextBoxContentRule.NonNegativeInteger, "0");
      TextBoxFormatter.Attach(this.txtMinLTV, TextBoxContentRule.NonNegativeDecimal, "0.000");
      TextBoxFormatter.Attach(this.txtMaxLTV, TextBoxContentRule.NonNegativeDecimal, "0.000");
    }

    [Category("Appearance")]
    [DefaultValue(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right)]
    public AnchorStyles Borders
    {
      get => this.grpHeader.Borders;
      set => this.grpHeader.Borders = value;
    }

    [Category("Appearance")]
    [DefaultValue("")]
    public string Title
    {
      get => this.grpHeader.Text;
      set => this.grpHeader.Text = value;
    }

    [Browsable(false)]
    public bool DataModified
    {
      get => this.modified;
      set => this.modified = value;
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public void AddControlToHeader(Control c)
    {
      this.flpHeader.Controls.Add(c);
      c.BringToFront();
    }

    private void setReadOnly()
    {
      this.txtMinNoteRate.ReadOnly = this.txtMaxNoteRate.ReadOnly = this.readOnly;
      this.txtMinLTV.ReadOnly = this.txtMaxLTV.ReadOnly = this.readOnly;
      this.txtMinTerm.ReadOnly = this.txtMaxTerm.ReadOnly = this.readOnly;
      this.chkOccPrimary.Enabled = this.chkOccSecondary.Enabled = this.chkOccInvestment.Enabled = !this.readOnly;
      this.chkInvUnassigned.Enabled = this.chkInvBulk.Enabled = this.chkInvFlow.Enabled = !this.readOnly;
      this.btnAddLP.Visible = this.btnRemoveLP.Visible = !this.readOnly;
      this.gvMilestones.Enabled = !this.readOnly;
    }

    private void loadMilestones()
    {
      if (this.gvMilestones.Items.Count > 0)
        return;
      List<EllieMae.EMLite.Workflow.Milestone> milestones = Session.StartupInfo.Milestones;
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestones)
      {
        if (milestone.Archived)
        {
          milestoneList.Add(milestone);
        }
        else
        {
          MilestoneLabel milestoneLabel = new MilestoneLabel(milestone);
          if (milestone.Name != "Completion")
            this.gvMilestones.Items.Add(new GVItem((object) milestoneLabel, (object) milestone));
        }
      }
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneList)
        this.gvMilestones.Items.Add(new GVItem((object) new MilestoneLabel(milestone), (object) milestone));
    }

    private void btnAddLP_Click(object sender, EventArgs e)
    {
      using (LoanProgramSelect loanProgramSelect = new LoanProgramSelect((LoanData) null, LoanProgramSelect.SelectTypes.ForTrade, Session.DefaultInstance))
      {
        if (loanProgramSelect.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        for (int index = 0; index < loanProgramSelect.SelectedEntries.Length; ++index)
        {
          string name = loanProgramSelect.SelectedEntries[index].Name;
          if (!this.gvLPs.Items.Contains((object) name))
            this.gvLPs.Items.Add(name);
        }
        this.onDataChange();
      }
    }

    private void btnRemoveLP_Click(object sender, EventArgs e)
    {
      while (this.gvLPs.SelectedItems.Count > 0)
        this.gvLPs.Items.Remove(this.gvLPs.SelectedItems[0]);
      this.onDataChange();
    }

    public void SetCurrentFilter(SimpleTradeFilter filter)
    {
      this.loadMilestones();
      this.loadFilterData(filter);
    }

    public void ClearFilter() => this.loadFilterData(new SimpleTradeFilter());

    public SimpleTradeFilter GetCurrentFilter(bool addInvestorStatus = true)
    {
      SimpleTradeFilter currentFilter = new SimpleTradeFilter(addInvestorStatus);
      currentFilter.LoanPrograms.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLPs.Items)
        currentFilter.LoanPrograms.Add(gvItem.Text);
      currentFilter.Milestones.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestones.Items)
      {
        if (gvItem.Checked)
        {
          EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) gvItem.Tag;
          currentFilter.Milestones.Add(tag.Name);
        }
      }
      currentFilter.OccupancyStatuses.Clear();
      if (this.chkOccPrimary.Checked)
        currentFilter.OccupancyStatuses.Add("PrimaryResidence");
      if (this.chkOccSecondary.Checked)
        currentFilter.OccupancyStatuses.Add("SecondHome");
      if (this.chkOccInvestment.Checked)
        currentFilter.OccupancyStatuses.Add("Investor");
      currentFilter.InvestorStatuses.Clear();
      if (this.chkInvUnassigned.Checked)
      {
        currentFilter.InvestorStatuses.Add("");
        currentFilter.InvestorStatuses.Add("Rejected");
      }
      if (this.chkInvBulk.Checked)
        currentFilter.InvestorStatuses.Add("AssignedBulk");
      if (this.chkInvFlow.Checked)
        currentFilter.InvestorStatuses.Add("AssignedFlow");
      currentFilter.NoteRateRange = Range<Decimal>.Parse(this.txtMinNoteRate.Text, this.txtMaxNoteRate.Text, Decimal.MinValue, Decimal.MaxValue);
      currentFilter.TermRange = Range<int>.Parse(this.txtMinTerm.Text, this.txtMaxTerm.Text, int.MinValue, int.MaxValue);
      currentFilter.LTVRange = Range<Decimal>.Parse(this.txtMinLTV.Text, this.txtMaxLTV.Text, Decimal.MinValue, Decimal.MaxValue);
      return currentFilter;
    }

    private void loadFilterData(SimpleTradeFilter filter)
    {
      this.gvLPs.Items.Clear();
      foreach (string loanProgram in (List<string>) filter.LoanPrograms)
        this.gvLPs.Items.Add(loanProgram);
      for (int nItemIndex = 0; nItemIndex < this.gvMilestones.Items.Count; ++nItemIndex)
      {
        EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) this.gvMilestones.Items[nItemIndex].Tag;
        this.gvMilestones.Items[nItemIndex].Checked = filter.Milestones.Contains(tag.Name);
      }
      this.chkOccPrimary.Checked = filter.OccupancyStatuses.Contains("PrimaryResidence");
      this.chkOccSecondary.Checked = filter.OccupancyStatuses.Contains("SecondHome");
      this.chkOccInvestment.Checked = filter.OccupancyStatuses.Contains("Investor");
      this.chkInvUnassigned.Checked = filter.InvestorStatuses.Contains("");
      this.chkInvBulk.Checked = filter.InvestorStatuses.Contains("AssignedBulk");
      this.chkInvFlow.Checked = filter.InvestorStatuses.Contains("AssignedFlow");
      this.txtMinNoteRate.Text = this.txtMaxNoteRate.Text = "";
      Decimal num1;
      if (filter.NoteRateRange != null)
      {
        TextBox txtMinNoteRate = this.txtMinNoteRate;
        string str1;
        if (!(filter.NoteRateRange.Minimum >= 0M))
        {
          str1 = "";
        }
        else
        {
          num1 = filter.NoteRateRange.Minimum;
          str1 = num1.ToString("0.000");
        }
        txtMinNoteRate.Text = str1;
        TextBox txtMaxNoteRate = this.txtMaxNoteRate;
        string str2;
        if (!(filter.NoteRateRange.Maximum < Decimal.MaxValue))
        {
          str2 = "";
        }
        else
        {
          num1 = filter.NoteRateRange.Maximum;
          str2 = num1.ToString("0.000");
        }
        txtMaxNoteRate.Text = str2;
      }
      this.txtMinTerm.Text = this.txtMaxTerm.Text = "";
      if (filter.TermRange != null)
      {
        TextBox txtMinTerm = this.txtMinTerm;
        int num2;
        string str3;
        if (filter.TermRange.Minimum <= 0)
        {
          str3 = "";
        }
        else
        {
          num2 = filter.TermRange.Minimum;
          str3 = num2.ToString();
        }
        txtMinTerm.Text = str3;
        TextBox txtMaxTerm = this.txtMaxTerm;
        string str4;
        if (filter.TermRange.Maximum >= int.MaxValue || filter.TermRange.Maximum == 0)
        {
          str4 = "";
        }
        else
        {
          num2 = filter.TermRange.Maximum;
          str4 = num2.ToString();
        }
        txtMaxTerm.Text = str4;
      }
      this.txtMinLTV.Text = this.txtMaxLTV.Text = "";
      if (filter.LTVRange != null)
      {
        TextBox txtMinLtv = this.txtMinLTV;
        string str5;
        if (!(filter.LTVRange.Minimum >= 0M))
        {
          str5 = "";
        }
        else
        {
          num1 = filter.LTVRange.Minimum;
          str5 = num1.ToString("0.000");
        }
        txtMinLtv.Text = str5;
        TextBox txtMaxLtv = this.txtMaxLTV;
        string str6;
        if (!(filter.LTVRange.Maximum < Decimal.MaxValue))
        {
          str6 = "";
        }
        else
        {
          num1 = filter.LTVRange.Maximum;
          str6 = num1.ToString("0.000");
        }
        txtMaxLtv.Text = str6;
      }
      this.modified = false;
    }

    public void validateNoteRateRange(object sender, CancelEventArgs e)
    {
      if (this.txtMaxNoteRate.Text == "" || this.txtMinNoteRate.Text == "" || (double) Utils.ParseSingle((object) this.txtMaxNoteRate.Text) >= (double) Utils.ParseSingle((object) this.txtMinNoteRate.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum note rate must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void validateTermRange(object sender, CancelEventArgs e)
    {
      if (this.txtMaxTerm.Text == "" || this.txtMinTerm.Text == "" || Utils.ParseInt((object) this.txtMaxTerm.Text) >= Utils.ParseInt((object) this.txtMinTerm.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum term must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void validateLTVRange(object sender, CancelEventArgs e)
    {
      if (this.txtMaxLTV.Text == "" || this.txtMinLTV.Text == "" || (double) Utils.ParseSingle((object) this.txtMaxLTV.Text) >= (double) Utils.ParseSingle((object) this.txtMinLTV.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum LTV must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void arrangeControlsExpanded()
    {
      int num1 = this.pnlBody.Width - 4 * this.borderPadding;
      this.grpLPs.Top = this.grpMilestones.Top = this.grpDetails.Top = this.borderPadding;
      GroupContainer grpLps = this.grpLPs;
      GroupContainer grpMilestones = this.grpMilestones;
      int num2;
      this.grpDetails.Height = num2 = Math.Max(0, this.pnlBody.Height - 2 * this.borderPadding) > 210 ? Math.Max(0, this.pnlBody.Height - 2 * this.borderPadding) : 210;
      int num3;
      int num4 = num3 = num2;
      grpMilestones.Height = num3;
      int num5 = num4;
      grpLps.Height = num5;
      this.grpDetails.Width = Math.Max(num1 / 3, this.grpDetails.MinimumSize.Width);
      if (this.grpDetails.Width < 269)
        this.grpDetails.Width = 269;
      int num6 = Math.Max(num1 - this.grpDetails.Width, 0);
      this.grpLPs.Width = num6 / 2;
      this.grpMilestones.Width = Math.Max(num6 - this.grpLPs.Width, 0);
      if (this.grpLPs.Width < 194)
        this.grpLPs.Width = this.grpMilestones.Width = 194;
      this.grpLPs.Left = this.borderPadding;
      this.grpMilestones.Left = this.grpLPs.Right + this.borderPadding;
      this.grpDetails.Left = this.grpMilestones.Right + this.borderPadding;
      if (this.grpMilestones.Right <= this.pnlBody.Right)
        return;
      this.pnlBody.HorizontalScroll.Visible = true;
    }

    private void SimpleSearchControl_Resize(object sender, EventArgs e)
    {
      this.grpLPs.Width = this.grpLPs.Parent.Width;
      this.grpMilestones.Width = this.grpMilestones.Parent.Width;
      if (!this.grpDetails.Visible)
        return;
      this.grpDetails.Width = this.grpDetails.Parent.Width;
    }

    private void onDataChange()
    {
      if (this.loading)
        return;
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private void onControlValueChanges(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.onDataChange();
    }

    private void lvwMilestones_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (this.readOnly)
        return;
      this.onDataChange();
    }

    private void SimpleSearchControl_Load(object sender, EventArgs e)
    {
      if (!this.DesignMode)
        this.loadMilestones();
      this.loading = false;
    }

    private void gvLPs_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemoveLP.Enabled = this.gvLPs.SelectedItems.Count > 0 && !this.readOnly;
    }

    private void lvwMilestones_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.NewValue = e.CurrentValue;
    }

    private void gvMilestones_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.onDataChange();
    }

    public void HideAdditionalDetails()
    {
      --this.tableLayoutPanel1.ColumnCount;
      this.grpDetails.Visible = false;
    }

    public string NoteRateMin
    {
      set => this.txtMinNoteRate.Text = value;
      get => this.txtMinNoteRate.Text;
    }

    public string NoteRateMax
    {
      set => this.txtMaxNoteRate.Text = value;
      get => this.txtMaxNoteRate.Text;
    }

    public string MaxTerm
    {
      set => this.txtMaxTerm.Text = value;
      get => this.txtMaxTerm.Text;
    }

    public string MinTerm
    {
      set => this.txtMinTerm.Text = value;
      get => this.txtMinTerm.Text;
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
      this.toolTip1 = new ToolTip(this.components);
      this.btnRemoveLP = new StandardIconButton();
      this.btnAddLP = new StandardIconButton();
      this.grpHeader = new GroupContainer();
      this.pnlBody = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.grpDetails = new GroupContainer();
      this.panelEx1 = new PanelEx();
      this.pnlCheckFields = new Panel();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label1 = new Label();
      this.label11 = new Label();
      this.chkOccPrimary = new CheckBox();
      this.chkInvFlow = new CheckBox();
      this.chkOccSecondary = new CheckBox();
      this.chkOccInvestment = new CheckBox();
      this.chkInvBulk = new CheckBox();
      this.label12 = new Label();
      this.chkInvUnassigned = new CheckBox();
      this.pnlFields = new Panel();
      this.label2 = new Label();
      this.txtMinNoteRate = new TextBox();
      this.txtMaxNoteRate = new TextBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label7 = new Label();
      this.txtMinTerm = new TextBox();
      this.txtMaxTerm = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label8 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.txtMinLTV = new TextBox();
      this.txtMaxLTV = new TextBox();
      this.grpLPs = new GroupContainer();
      this.gvLPs = new GridView();
      this.grpMilestones = new GroupContainer();
      this.gvMilestones = new GridView();
      this.flpHeader = new FlowLayoutPanel();
      ((ISupportInitialize) this.btnRemoveLP).BeginInit();
      ((ISupportInitialize) this.btnAddLP).BeginInit();
      this.grpHeader.SuspendLayout();
      this.pnlBody.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.panelEx1.SuspendLayout();
      this.pnlCheckFields.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.grpLPs.SuspendLayout();
      this.grpMilestones.SuspendLayout();
      this.SuspendLayout();
      this.btnRemoveLP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveLP.BackColor = Color.Transparent;
      this.btnRemoveLP.Enabled = false;
      this.btnRemoveLP.Location = new Point(286, 5);
      this.btnRemoveLP.MouseDownImage = (Image) null;
      this.btnRemoveLP.Name = "btnRemoveLP";
      this.btnRemoveLP.Size = new Size(16, 16);
      this.btnRemoveLP.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveLP.TabIndex = 2;
      this.btnRemoveLP.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveLP, "Remove Loan Program");
      this.btnRemoveLP.Click += new EventHandler(this.btnRemoveLP_Click);
      this.btnAddLP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddLP.BackColor = Color.Transparent;
      this.btnAddLP.Location = new Point(266, 5);
      this.btnAddLP.MouseDownImage = (Image) null;
      this.btnAddLP.Name = "btnAddLP";
      this.btnAddLP.Size = new Size(16, 16);
      this.btnAddLP.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddLP.TabIndex = 1;
      this.btnAddLP.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddLP, "Add Loan Program");
      this.btnAddLP.Click += new EventHandler(this.btnAddLP_Click);
      this.grpHeader.Controls.Add((Control) this.pnlBody);
      this.grpHeader.Controls.Add((Control) this.flpHeader);
      this.grpHeader.Dock = DockStyle.Fill;
      this.grpHeader.HeaderForeColor = SystemColors.ControlText;
      this.grpHeader.Location = new Point(0, 0);
      this.grpHeader.Name = "grpHeader";
      this.grpHeader.Size = new Size(945, 550);
      this.grpHeader.TabIndex = 10;
      this.pnlBody.AutoScroll = true;
      this.pnlBody.AutoScrollMinSize = new Size(677, 210);
      this.pnlBody.Controls.Add((Control) this.tableLayoutPanel1);
      this.pnlBody.Dock = DockStyle.Fill;
      this.pnlBody.Location = new Point(1, 26);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(943, 523);
      this.pnlBody.TabIndex = 9;
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
      this.tableLayoutPanel1.Controls.Add((Control) this.grpDetails, 2, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.grpLPs, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.grpMilestones, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(943, 523);
      this.tableLayoutPanel1.TabIndex = 1;
      this.grpDetails.AutoScroll = true;
      this.grpDetails.Controls.Add((Control) this.panelEx1);
      this.grpDetails.Dock = DockStyle.Fill;
      this.grpDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpDetails.Location = new Point(631, 3);
      this.grpDetails.MinimumSize = new Size(271, 0);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(309, 517);
      this.grpDetails.TabIndex = 7;
      this.grpDetails.Text = "Additional Details";
      this.panelEx1.AutoScroll = true;
      this.panelEx1.BackColor = Color.Transparent;
      this.panelEx1.Controls.Add((Control) this.pnlCheckFields);
      this.panelEx1.Controls.Add((Control) this.pnlFields);
      this.panelEx1.Dock = DockStyle.Fill;
      this.panelEx1.Location = new Point(1, 26);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(307, 490);
      this.panelEx1.TabIndex = 5;
      this.pnlCheckFields.BackColor = Color.Transparent;
      this.pnlCheckFields.Controls.Add((Control) this.label15);
      this.pnlCheckFields.Controls.Add((Control) this.label16);
      this.pnlCheckFields.Controls.Add((Control) this.label17);
      this.pnlCheckFields.Controls.Add((Control) this.label14);
      this.pnlCheckFields.Controls.Add((Control) this.label13);
      this.pnlCheckFields.Controls.Add((Control) this.label1);
      this.pnlCheckFields.Controls.Add((Control) this.label11);
      this.pnlCheckFields.Controls.Add((Control) this.chkOccPrimary);
      this.pnlCheckFields.Controls.Add((Control) this.chkInvFlow);
      this.pnlCheckFields.Controls.Add((Control) this.chkOccSecondary);
      this.pnlCheckFields.Controls.Add((Control) this.chkOccInvestment);
      this.pnlCheckFields.Controls.Add((Control) this.chkInvBulk);
      this.pnlCheckFields.Controls.Add((Control) this.label12);
      this.pnlCheckFields.Controls.Add((Control) this.chkInvUnassigned);
      this.pnlCheckFields.Dock = DockStyle.Fill;
      this.pnlCheckFields.Location = new Point(0, 77);
      this.pnlCheckFields.Name = "pnlCheckFields";
      this.pnlCheckFields.Size = new Size(307, 413);
      this.pnlCheckFields.TabIndex = 4;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(164, 45);
      this.label15.Name = "label15";
      this.label15.Size = new Size(83, 14);
      this.label15.TabIndex = 28;
      this.label15.Text = "Assigned - Bulk";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(164, 67);
      this.label16.Name = "label16";
      this.label16.Size = new Size(87, 14);
      this.label16.TabIndex = 27;
      this.label16.Text = "Assigned - Flow";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(164, 24);
      this.label17.Name = "label17";
      this.label17.Size = new Size(64, 14);
      this.label17.TabIndex = 26;
      this.label17.Text = "Unassigned";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(26, 45);
      this.label14.Name = "label14";
      this.label14.Size = new Size(60, 14);
      this.label14.TabIndex = 25;
      this.label14.Text = "Secondary";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(26, 67);
      this.label13.Name = "label13";
      this.label13.Size = new Size(59, 14);
      this.label13.TabIndex = 24;
      this.label13.Text = "Investment";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(26, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 14);
      this.label1.TabIndex = 20;
      this.label1.Text = "Primary";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 5);
      this.label11.Name = "label11";
      this.label11.Size = new Size(97, 14);
      this.label11.TabIndex = 19;
      this.label11.Text = "Occupancy Status";
      this.chkOccPrimary.AutoSize = true;
      this.chkOccPrimary.Location = new Point(10, 24);
      this.chkOccPrimary.Name = "chkOccPrimary";
      this.chkOccPrimary.Size = new Size(15, 14);
      this.chkOccPrimary.TabIndex = 1;
      this.chkOccPrimary.UseVisualStyleBackColor = true;
      this.chkOccPrimary.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.chkInvFlow.AutoSize = true;
      this.chkInvFlow.Location = new Point(149, 66);
      this.chkInvFlow.Name = "chkInvFlow";
      this.chkInvFlow.Size = new Size(15, 14);
      this.chkInvFlow.TabIndex = 6;
      this.chkInvFlow.UseVisualStyleBackColor = true;
      this.chkInvFlow.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.chkOccSecondary.AutoSize = true;
      this.chkOccSecondary.Location = new Point(10, 45);
      this.chkOccSecondary.Name = "chkOccSecondary";
      this.chkOccSecondary.Size = new Size(15, 14);
      this.chkOccSecondary.TabIndex = 2;
      this.chkOccSecondary.UseVisualStyleBackColor = true;
      this.chkOccSecondary.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.chkOccInvestment.AutoSize = true;
      this.chkOccInvestment.Location = new Point(10, 66);
      this.chkOccInvestment.Name = "chkOccInvestment";
      this.chkOccInvestment.Size = new Size(15, 14);
      this.chkOccInvestment.TabIndex = 3;
      this.chkOccInvestment.UseVisualStyleBackColor = true;
      this.chkOccInvestment.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.chkInvBulk.AutoSize = true;
      this.chkInvBulk.Location = new Point(149, 45);
      this.chkInvBulk.Name = "chkInvBulk";
      this.chkInvBulk.Size = new Size(15, 14);
      this.chkInvBulk.TabIndex = 5;
      this.chkInvBulk.UseVisualStyleBackColor = true;
      this.chkInvBulk.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(146, 5);
      this.label12.Name = "label12";
      this.label12.Size = new Size(80, 14);
      this.label12.TabIndex = 23;
      this.label12.Text = "Investor Status";
      this.chkInvUnassigned.AutoSize = true;
      this.chkInvUnassigned.Location = new Point(149, 24);
      this.chkInvUnassigned.Name = "chkInvUnassigned";
      this.chkInvUnassigned.Size = new Size(15, 14);
      this.chkInvUnassigned.TabIndex = 4;
      this.chkInvUnassigned.UseVisualStyleBackColor = true;
      this.chkInvUnassigned.CheckedChanged += new EventHandler(this.onControlValueChanges);
      this.pnlFields.BackColor = Color.Transparent;
      this.pnlFields.Controls.Add((Control) this.label2);
      this.pnlFields.Controls.Add((Control) this.txtMinNoteRate);
      this.pnlFields.Controls.Add((Control) this.txtMaxNoteRate);
      this.pnlFields.Controls.Add((Control) this.label3);
      this.pnlFields.Controls.Add((Control) this.label4);
      this.pnlFields.Controls.Add((Control) this.label7);
      this.pnlFields.Controls.Add((Control) this.txtMinTerm);
      this.pnlFields.Controls.Add((Control) this.txtMaxTerm);
      this.pnlFields.Controls.Add((Control) this.label6);
      this.pnlFields.Controls.Add((Control) this.label5);
      this.pnlFields.Controls.Add((Control) this.label8);
      this.pnlFields.Controls.Add((Control) this.label10);
      this.pnlFields.Controls.Add((Control) this.label9);
      this.pnlFields.Controls.Add((Control) this.txtMinLTV);
      this.pnlFields.Controls.Add((Control) this.txtMaxLTV);
      this.pnlFields.Dock = DockStyle.Top;
      this.pnlFields.Location = new Point(0, 0);
      this.pnlFields.Name = "pnlFields";
      this.pnlFields.Size = new Size(307, 77);
      this.pnlFields.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Note Rate";
      this.txtMinNoteRate.Location = new Point(68, 6);
      this.txtMinNoteRate.MaxLength = 6;
      this.txtMinNoteRate.Name = "txtMinNoteRate";
      this.txtMinNoteRate.Size = new Size(72, 20);
      this.txtMinNoteRate.TabIndex = 1;
      this.txtMinNoteRate.TextAlign = HorizontalAlignment.Right;
      this.txtMinNoteRate.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinNoteRate.Validating += new CancelEventHandler(this.validateNoteRateRange);
      this.txtMaxNoteRate.Location = new Point(154, 6);
      this.txtMaxNoteRate.MaxLength = 6;
      this.txtMaxNoteRate.Name = "txtMaxNoteRate";
      this.txtMaxNoteRate.Size = new Size(72, 20);
      this.txtMaxNoteRate.TabIndex = 2;
      this.txtMaxNoteRate.TextAlign = HorizontalAlignment.Right;
      this.txtMaxNoteRate.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxNoteRate.Validating += new CancelEventHandler(this.validateNoteRateRange);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(142, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(11, 14);
      this.label3.TabIndex = 7;
      this.label3.Text = "-";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(228, 10);
      this.label4.Name = "label4";
      this.label4.Size = new Size(17, 14);
      this.label4.TabIndex = 8;
      this.label4.Text = "%";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 32);
      this.label7.Name = "label7";
      this.label7.Size = new Size(30, 14);
      this.label7.TabIndex = 9;
      this.label7.Text = "Term";
      this.txtMinTerm.Location = new Point(68, 28);
      this.txtMinTerm.MaxLength = 4;
      this.txtMinTerm.Name = "txtMinTerm";
      this.txtMinTerm.Size = new Size(72, 20);
      this.txtMinTerm.TabIndex = 3;
      this.txtMinTerm.TextAlign = HorizontalAlignment.Right;
      this.txtMinTerm.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinTerm.Validating += new CancelEventHandler(this.validateTermRange);
      this.txtMaxTerm.Location = new Point(154, 28);
      this.txtMaxTerm.MaxLength = 4;
      this.txtMaxTerm.Name = "txtMaxTerm";
      this.txtMaxTerm.Size = new Size(72, 20);
      this.txtMaxTerm.TabIndex = 4;
      this.txtMaxTerm.TextAlign = HorizontalAlignment.Right;
      this.txtMaxTerm.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxTerm.Validating += new CancelEventHandler(this.validateTermRange);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(142, 33);
      this.label6.Name = "label6";
      this.label6.Size = new Size(11, 14);
      this.label6.TabIndex = 12;
      this.label6.Text = "-";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(228, 33);
      this.label5.Name = "label5";
      this.label5.Size = new Size(30, 14);
      this.label5.TabIndex = 13;
      this.label5.Text = "mths";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(228, 54);
      this.label8.Name = "label8";
      this.label8.Size = new Size(17, 14);
      this.label8.TabIndex = 18;
      this.label8.Text = "%";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(7, 54);
      this.label10.Name = "label10";
      this.label10.Size = new Size(26, 14);
      this.label10.TabIndex = 14;
      this.label10.Text = "LTV";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(142, 54);
      this.label9.Name = "label9";
      this.label9.Size = new Size(11, 14);
      this.label9.TabIndex = 17;
      this.label9.Text = "-";
      this.txtMinLTV.Location = new Point(68, 50);
      this.txtMinLTV.MaxLength = 6;
      this.txtMinLTV.Name = "txtMinLTV";
      this.txtMinLTV.Size = new Size(72, 20);
      this.txtMinLTV.TabIndex = 5;
      this.txtMinLTV.TextAlign = HorizontalAlignment.Right;
      this.txtMinLTV.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinLTV.Validating += new CancelEventHandler(this.validateLTVRange);
      this.txtMaxLTV.Location = new Point(154, 50);
      this.txtMaxLTV.MaxLength = 6;
      this.txtMaxLTV.Name = "txtMaxLTV";
      this.txtMaxLTV.Size = new Size(72, 20);
      this.txtMaxLTV.TabIndex = 6;
      this.txtMaxLTV.TextAlign = HorizontalAlignment.Right;
      this.txtMaxLTV.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxLTV.Validating += new CancelEventHandler(this.validateLTVRange);
      this.grpLPs.Controls.Add((Control) this.gvLPs);
      this.grpLPs.Controls.Add((Control) this.btnRemoveLP);
      this.grpLPs.Controls.Add((Control) this.btnAddLP);
      this.grpLPs.Dock = DockStyle.Fill;
      this.grpLPs.HeaderForeColor = SystemColors.ControlText;
      this.grpLPs.Location = new Point(3, 3);
      this.grpLPs.Name = "grpLPs";
      this.grpLPs.Size = new Size(308, 517);
      this.grpLPs.TabIndex = 5;
      this.grpLPs.Text = "Select Loan Programs";
      this.gvLPs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Milestone";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Column";
      gvColumn1.Width = 306;
      this.gvLPs.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvLPs.Dock = DockStyle.Fill;
      this.gvLPs.HeaderHeight = 0;
      this.gvLPs.HeaderVisible = false;
      this.gvLPs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLPs.Location = new Point(1, 26);
      this.gvLPs.Name = "gvLPs";
      this.gvLPs.Size = new Size(306, 490);
      this.gvLPs.TabIndex = 3;
      this.gvLPs.SelectedIndexChanged += new EventHandler(this.gvLPs_SelectedIndexChanged);
      this.grpMilestones.Controls.Add((Control) this.gvMilestones);
      this.grpMilestones.Dock = DockStyle.Fill;
      this.grpMilestones.HeaderForeColor = SystemColors.ControlText;
      this.grpMilestones.Location = new Point(317, 3);
      this.grpMilestones.Name = "grpMilestones";
      this.grpMilestones.Size = new Size(308, 517);
      this.grpMilestones.TabIndex = 6;
      this.grpMilestones.Text = "Select Last Finished Milestone";
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Milestone";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Column";
      gvColumn2.Width = 306;
      this.gvMilestones.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.HeaderHeight = 0;
      this.gvMilestones.HeaderVisible = false;
      this.gvMilestones.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvMilestones.Location = new Point(1, 26);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Selectable = false;
      this.gvMilestones.Size = new Size(306, 490);
      this.gvMilestones.TabIndex = 0;
      this.gvMilestones.SubItemCheck += new GVSubItemEventHandler(this.gvMilestones_SubItemCheck);
      this.flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpHeader.BackColor = Color.Transparent;
      this.flpHeader.FlowDirection = FlowDirection.RightToLeft;
      this.flpHeader.Location = new Point(87, 2);
      this.flpHeader.Name = "flpHeader";
      this.flpHeader.Size = new Size(852, 22);
      this.flpHeader.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.Controls.Add((Control) this.grpHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SimpleSearchControl);
      this.Size = new Size(945, 550);
      this.Load += new EventHandler(this.SimpleSearchControl_Load);
      this.Resize += new EventHandler(this.SimpleSearchControl_Resize);
      ((ISupportInitialize) this.btnRemoveLP).EndInit();
      ((ISupportInitialize) this.btnAddLP).EndInit();
      this.grpHeader.ResumeLayout(false);
      this.pnlBody.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.grpDetails.ResumeLayout(false);
      this.panelEx1.ResumeLayout(false);
      this.pnlCheckFields.ResumeLayout(false);
      this.pnlCheckFields.PerformLayout();
      this.pnlFields.ResumeLayout(false);
      this.pnlFields.PerformLayout();
      this.grpLPs.ResumeLayout(false);
      this.grpMilestones.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
