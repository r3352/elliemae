// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.PlanCodeListControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class PlanCodeListControl : UserControl
  {
    private static readonly string[] PlanCodeFilterFields = new string[3]
    {
      "1172",
      "420",
      "608"
    };
    private GridViewFilterManager filterMngr;
    private ComboBox cboInvestor;
    private ComboBox cboPlanType;
    private bool showHiddenInvestorNames;
    private bool showPlanCodeTypeColumn;
    private Sessions.Session session;
    private static readonly TableLayout PlanCodeLayout = new TableLayout(new TableLayout.Column[65]
    {
      new TableLayout.Column("PlanCode.PlanType", "Type", HorizontalAlignment.Left, 80),
      new TableLayout.Column("PlanCode.ProgSpnsrNm", "Investor", HorizontalAlignment.Left, 0),
      new TableLayout.Column("PlanCode.Desc", "Description", HorizontalAlignment.Left, 300),
      new TableLayout.Column("1881", "Plan Code", HorizontalAlignment.Left, 100),
      new TableLayout.Column("PlanCode.ID", "ICE Mortgage Technology Plan ID", HorizontalAlignment.Left, 180),
      new TableLayout.Column("PlanCode.LoanProgTyp", "Order Type", HorizontalAlignment.Left, 0),
      new TableLayout.Column("PlanCode.ProgCd", "Program Code", HorizontalAlignment.Left, 100),
      new TableLayout.Column("PlanCode.InvCd", "Investor Code", HorizontalAlignment.Left, 100),
      new TableLayout.Column("995", "ARM Type", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1172", "Loan Type", HorizontalAlignment.Left, 0),
      new TableLayout.Column("Terms.USDAGovtType", "USDA - Govt Loan Type", HorizontalAlignment.Left, 0),
      new TableLayout.Column("420", "Lien Pos", HorizontalAlignment.Left, 0),
      new TableLayout.Column("608", "Amortization", HorizontalAlignment.Left, 0),
      new TableLayout.Column("4", "Term", HorizontalAlignment.Right, 0),
      new TableLayout.Column("325", "Due In", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1659", "Balloon", HorizontalAlignment.Left, 0),
      new TableLayout.Column("677", "Assumability", HorizontalAlignment.Left, 0),
      new TableLayout.Column("663", "Demand", HorizontalAlignment.Left, 0),
      new TableLayout.Column("423", "Biweekly", HorizontalAlignment.Left, 0),
      new TableLayout.Column("SYS.X2", "Days/Year", HorizontalAlignment.Right, 0),
      new TableLayout.Column("Terms.IntrOnly", "Interest Only", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1177", "Interest Only Months", HorizontalAlignment.Right, 0),
      new TableLayout.Column("697", "ARM - 1st Change High", HorizontalAlignment.Right, 0),
      new TableLayout.Column("696", "ARM - 1st Change", HorizontalAlignment.Right, 0),
      new TableLayout.Column("695", "ARM - Sub Cap High", HorizontalAlignment.Right, 0),
      new TableLayout.Column("694", "ARM - Sub Change", HorizontalAlignment.Right, 0),
      new TableLayout.Column("247", "ARM - Life Cap", HorizontalAlignment.Right, 0),
      new TableLayout.Column("ARM.ApplyLfCpLow", "ARM - Life Cap Low", HorizontalAlignment.Right, 0),
      new TableLayout.Column("ARM.FlrBasis", "ARM - Floor Basis", HorizontalAlignment.Right, 0),
      new TableLayout.Column("ARM.FlrVerbgTyp", "ARM - Floor Verbiage", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1700", "ARM - Rounding Factor", HorizontalAlignment.Right, 0),
      new TableLayout.Column("SYS.X1", "ARM - Rounding Type", HorizontalAlignment.Left, 0),
      new TableLayout.Column("ARM.IdxLkbckPrd", "ARM - Index Lookback", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1290", "Convertible", HorizontalAlignment.Left, 0),
      new TableLayout.Column("CnvrOpt.FeeAmt", "Cnvtbl - Fee Amt", HorizontalAlignment.Right, 0),
      new TableLayout.Column("CnvrOpt.FeePct", "Cnvtbl - Fee Pct", HorizontalAlignment.Right, 0),
      new TableLayout.Column("CnvrOpt.MaxRateAdj", "Cnvtbl - Max Rate Adj", HorizontalAlignment.Right, 0),
      new TableLayout.Column("CnvrOpt.MinRateAdj", "Cnvtbl - Min Rate Adj", HorizontalAlignment.Right, 0),
      new TableLayout.Column("675", "Prepymt Penalty", HorizontalAlignment.Left, 0),
      new TableLayout.Column("Terms.PrepyVrbgTyp", "Prepymt Penalty Verbiage", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1266", "GPM - Years", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1267", "GPM - Rate", HorizontalAlignment.Right, 0),
      new TableLayout.Column("Terms.GPMPmtIncr", "GPM - Pymt Increase", HorizontalAlignment.Right, 0),
      new TableLayout.Column("SYS.X6", "Constr - Est Int On", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1962", "Constr - Num Days", HorizontalAlignment.Right, 0),
      new TableLayout.Column("19", "Constr - Loan Purpose", HorizontalAlignment.Left, 0),
      new TableLayout.Column("1889", "HELOC - Draw Period", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1890", "HELOC - Repay Period", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1891", "HELOC - Annual Fee", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1893", "HELOC - Max Allowable APR", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1892", "HELOC - Min Adv Amt", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1483", "HELOC - Min Payment", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1413", "HELOC - Payment Factor", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.MinPmtUPB", "HELOC - Min Pymt UPB", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.OvrLmtChg", "HELOC - Over Limit Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.OvrLmtRtnChg", "HELOC - Over Limit Return Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.RlsRecgChg", "HELOC - Release Rec Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.RtdChkChgAmt", "HELOC - Rtn Check Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.RtdChkChgMax", "HELOC - Max Rtn Check Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.RtdChkChgMin", "HELOC - Min Rtn Check Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.RtdChkChgRat", "HELOC - Rtn Check Charge Rate", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.StopPmtChrg", "HELOC - Stop Pymt Charge", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1986", "HELOC - Termination Fee", HorizontalAlignment.Right, 0),
      new TableLayout.Column("1987", "HELOC - Termination Period", HorizontalAlignment.Right, 0),
      new TableLayout.Column("HELOC.WireFee", "HELOC - Wire Fee", HorizontalAlignment.Right, 0)
    });
    private IContainer components;
    private GridView gvPlanCodes;
    private GradientPanel gradientPanel1;
    private Label lblDescription;
    private Button btnClearFilter;

    public event PlanCodeEventHandler PlanCodeDoubleClick;

    public event EventHandler SelectedIndexChanged;

    public event EventHandler FilterChanged;

    public PlanCodeListControl()
    {
      this.InitializeComponent();
      this.gvPlanCodes.AllowMultiselect = false;
    }

    public void AssignSession(Sessions.Session session)
    {
      this.session = session;
      this.initializeColumns();
    }

    [Browsable(false)]
    public int PlanCodeCount => this.gvPlanCodes.VisibleItems.Count;

    [Browsable(false)]
    public int SelectedPlanCodeCount => this.gvPlanCodes.SelectedItems.Count;

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AllowMultiselect
    {
      get => this.gvPlanCodes.AllowMultiselect;
      set => this.gvPlanCodes.AllowMultiselect = value;
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ShowHiddenInvestorNames
    {
      get => this.showHiddenInvestorNames;
      set => this.showHiddenInvestorNames = value;
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ShowPlanCodeTypeColumn
    {
      get => this.showPlanCodeTypeColumn;
      set
      {
        this.showPlanCodeTypeColumn = value;
        this.initializeColumns();
      }
    }

    public void ClearFilters()
    {
      this.filterMngr.ClearColumnFilters();
      this.onFilterChanged((object) this, EventArgs.Empty);
    }

    private void initializeColumns()
    {
      if (this.session == null || !this.session.IsConnected)
        return;
      this.gvPlanCodes.Columns.Clear();
      if (this.filterMngr != null)
        this.filterMngr.Dispose();
      this.filterMngr = new GridViewFilterManager(this.session, this.gvPlanCodes, true);
      this.filterMngr.FilterChanged += new EventHandler(this.onFilterChanged);
      this.filterMngr.AllowTextMatchForListOptions = true;
      foreach (TableLayout.Column column in PlanCodeListControl.PlanCodeLayout)
      {
        if (this.showPlanCodeTypeColumn || column.ColumnID != "PlanCode.PlanType")
        {
          GVColumn newColumn = new GVColumn();
          newColumn.Text = column.Description;
          newColumn.TextAlign = column.Alignment;
          newColumn.Width = column.Width;
          int columnIndex = this.gvPlanCodes.Columns.Add(newColumn);
          FieldDefinition field = (FieldDefinition) StandardFields.GetField(column.ColumnID);
          if (field == null && column.ColumnID == "PlanCode.PlanType")
          {
            this.cboPlanType = (ComboBox) this.filterMngr.CreateColumnFilter(columnIndex, FieldFormat.DROPDOWNLIST);
            newColumn.Tag = (object) "PlanCode.PlanType";
          }
          else
          {
            if (field.FieldID == "PlanCode.ProgSpnsrNm")
              this.cboInvestor = (ComboBox) this.filterMngr.CreateColumnFilter(columnIndex, FieldFormat.DROPDOWNLIST);
            else
              this.filterMngr.CreateColumnFilter(columnIndex, field);
            newColumn.Tag = (object) field;
          }
        }
      }
    }

    private void onFilterChanged(object sender, EventArgs e)
    {
      this.refreshFilterDescription();
      if (this.FilterChanged == null)
        return;
      this.FilterChanged((object) this, e);
    }

    private void refreshFilterDescription()
    {
      FieldFilterList fieldFilterList = this.filterMngr.ToFieldFilterList();
      if (fieldFilterList.Count == 0)
        this.lblDescription.Text = "Filter: None";
      else
        this.lblDescription.Text = "Filter: " + fieldFilterList.ToString(true);
    }

    [Browsable(false)]
    public Plan SelectedPlan
    {
      get
      {
        return this.gvPlanCodes.SelectedItems.Count == 0 ? (Plan) null : (Plan) this.gvPlanCodes.SelectedItems[0].Tag;
      }
      set
      {
        this.gvPlanCodes.SelectedItems.Clear();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPlanCodes.Items)
        {
          if (((Plan) gvItem.Tag).Equals((object) value))
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
    }

    public void EnsureVisible(Plan plan)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPlanCodes.Items)
      {
        if (((Plan) gvItem.Tag).Equals((object) plan))
        {
          this.gvPlanCodes.EnsureVisible(gvItem.Index);
          break;
        }
      }
    }

    public void LoadPlans(Plan[] plans)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 277, nameof (LoadPlans), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
      this.gvPlanCodes.Items.Clear();
      this.cboInvestor.Items.Clear();
      this.cboInvestor.Items.Add((object) "");
      if (this.cboPlanType != null)
      {
        this.cboPlanType.Items.Clear();
        this.cboPlanType.Items.Add((object) "");
      }
      foreach (Plan plan in plans)
      {
        this.gvPlanCodes.Items.Add(this.createPlanCodeItem(plan));
        Investor investor = plan.GetInvestor();
        if ((this.ShowHiddenInvestorNames || !plan.HideInvestorName) && !this.cboInvestor.Items.Contains((object) investor.Name))
          this.cboInvestor.Items.Add((object) investor.Name);
        if (this.cboPlanType != null && !this.cboPlanType.Items.Contains((object) plan.GetField("PlanCode.PlanType")))
          this.cboPlanType.Items.Add((object) plan.GetField("PlanCode.PlanType"));
      }
      PerformanceMeter.Current.AddCheckpoint("after Plan loop", 305, nameof (LoadPlans), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
      this.cboInvestor.Sorted = true;
      if (this.cboPlanType != null)
        this.cboPlanType.Sorted = true;
      this.filterMngr.ApplyFilter();
      PerformanceMeter.Current.AddCheckpoint("END", 314, nameof (LoadPlans), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
    }

    public void HighlightPlan(PlanCodeInfo planInfo)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPlanCodes.Items)
      {
        if (((Plan) gvItem.Tag).ToPlanCodeInfo().Equals((object) planInfo))
        {
          gvItem.ForeColor = Color.DarkGray;
          break;
        }
      }
    }

    public void ApplyFilter(IHtmlInput filterSource)
    {
      bool flag = false;
      foreach (GVColumn column in this.gvPlanCodes.Columns)
      {
        try
        {
          FieldDefinition tag = (FieldDefinition) column.Tag;
          if (Array.IndexOf<string>(PlanCodeListControl.PlanCodeFilterFields, tag.FieldID) >= 0)
          {
            string field = filterSource.GetField(tag.FieldID);
            if ((field ?? "") != "")
            {
              this.filterMngr.SetFilterValue(column.FilterControl, tag.ToDisplayValue(field));
              flag = true;
            }
          }
        }
        catch
        {
        }
      }
      if (flag)
        this.filterMngr.ApplyFilter();
      this.refreshFilterDescription();
    }

    public void ClearPlans() => this.gvPlanCodes.Items.Clear();

    public void ClearSelections()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 384, nameof (ClearSelections), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
      this.gvPlanCodes.Items.Clear();
      PerformanceMeter.Current.AddCheckpoint("END", 386, nameof (ClearSelections), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
    }

    public void SelectPlan(string planId)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 394, nameof (SelectPlan), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPlanCodes.Items)
      {
        using (PerformanceMeter.Current.BeginOperation("item loop"))
        {
          if (((Plan) gvItem.Tag).PlanID == planId)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
      PerformanceMeter.Current.AddCheckpoint("END", 407, nameof (SelectPlan), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
    }

    public void SelectComputedPlan(string computedPlanCode)
    {
      if (computedPlanCode.IndexOf("_") < 0)
        return;
      this.SelectPlan(computedPlanCode.Split('_')[1]);
    }

    public void SelectPlanByPlanCode(string planCode)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 430, nameof (SelectPlanByPlanCode), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPlanCodes.Items)
      {
        using (PerformanceMeter.Current.BeginOperation("item loop"))
        {
          if (((Plan) gvItem.Tag).InvestorPlanCode == planCode)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
      PerformanceMeter.Current.AddCheckpoint("END", 444, nameof (SelectPlanByPlanCode), "D:\\ws\\24.3.0.0\\EmLite\\Input\\DocEngine\\PlanCodeListControl.cs");
    }

    public Plan GetPlan(int index) => (Plan) this.gvPlanCodes.Items[index].Tag;

    public Plan[] GetSelectedPlans()
    {
      List<Plan> planList = new List<Plan>();
      foreach (GVItem selectedItem in this.gvPlanCodes.SelectedItems)
        planList.Add((Plan) selectedItem.Tag);
      return planList.ToArray();
    }

    public void RemoveSelectedPlans()
    {
      for (int nItemIndex = this.gvPlanCodes.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvPlanCodes.Items[nItemIndex].Selected)
          this.gvPlanCodes.Items.RemoveAt(nItemIndex);
      }
    }

    private GVItem createPlanCodeItem(Plan plan)
    {
      GVItem planCodeItem = new GVItem();
      for (int index = 0; index < this.gvPlanCodes.Columns.Count; ++index)
      {
        FieldDefinition fieldDefinition = (FieldDefinition) null;
        string encFieldId;
        if (this.gvPlanCodes.Columns[index].Tag is FieldDefinition)
        {
          fieldDefinition = (FieldDefinition) this.gvPlanCodes.Columns[index].Tag;
          encFieldId = fieldDefinition.FieldID;
        }
        else
          encFieldId = string.Concat(this.gvPlanCodes.Columns[index].Tag);
        try
        {
          if (encFieldId == "PlanCode.ProgSpnsrNm" && !this.ShowHiddenInvestorNames && plan.HideInvestorName)
            planCodeItem.SubItems[index].Text = "";
          else if (fieldDefinition != null)
            planCodeItem.SubItems[index].Text = fieldDefinition.ToDisplayValue(plan.GetField(encFieldId));
          else if (encFieldId == "PlanCode.PlanType")
            planCodeItem.SubItems[index].Text = plan.GetField(encFieldId);
        }
        catch
        {
          planCodeItem.SubItems[index].Text = "<Invalid>";
        }
      }
      if (!plan.Active)
      {
        planCodeItem.BackColor = EncompassColors.Secondary2;
        planCodeItem.ForeColor = EncompassColors.Secondary5;
      }
      planCodeItem.Tag = (object) plan;
      return planCodeItem;
    }

    private Hyperlink createDetailsHyperlink(string text, Plan plan)
    {
      Hyperlink detailsHyperlink = new Hyperlink(text, new EventHandler(this.onPlanClick));
      detailsHyperlink.Tag = (object) plan;
      return detailsHyperlink;
    }

    private void onPlanClick(object sender, EventArgs e)
    {
      using (PlanCodeDetailsDialog codeDetailsDialog = new PlanCodeDetailsDialog((Plan) ((Element) sender).Tag))
      {
        int num = (int) codeDetailsDialog.ShowDialog((IWin32Window) this);
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      this.setDefaultColumnWidths();
      base.OnLoad(e);
    }

    private void setDefaultColumnWidths()
    {
      using (Graphics graphics = this.CreateGraphics())
      {
        foreach (GVColumn column in this.gvPlanCodes.Columns)
        {
          if (column.Width == 0)
          {
            FieldDefinition tag = (FieldDefinition) column.Tag;
            column.Width = GridViewLayoutHelper.GetDefaultColumnWidth(tag, graphics, this.gvPlanCodes.Font);
          }
        }
      }
    }

    private void gvPlanCodes_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      Plan tag = (Plan) e.Item.Tag;
      if (this.PlanCodeDoubleClick == null)
        return;
      this.PlanCodeDoubleClick((object) this, new PlanCodeEventArgs(tag));
    }

    private void gvPlanCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, EventArgs.Empty);
    }

    private void btnClearFilter_Click(object sender, EventArgs e) => this.ClearFilters();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gvPlanCodes = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.btnClearFilter = new Button();
      this.lblDescription = new Label();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.gvPlanCodes.AllowMultiselect = false;
      this.gvPlanCodes.BorderStyle = BorderStyle.None;
      this.gvPlanCodes.Dock = DockStyle.Fill;
      this.gvPlanCodes.FilterVisible = true;
      this.gvPlanCodes.Location = new Point(0, 31);
      this.gvPlanCodes.Name = "gvPlanCodes";
      this.gvPlanCodes.Size = new Size(699, 293);
      this.gvPlanCodes.TabIndex = 0;
      this.gvPlanCodes.SelectedIndexChanged += new EventHandler(this.gvPlanCodes_SelectedIndexChanged);
      this.gvPlanCodes.ItemDoubleClick += new GVItemEventHandler(this.gvPlanCodes_ItemDoubleClick);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.btnClearFilter);
      this.gradientPanel1.Controls.Add((Control) this.lblDescription);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(699, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 1;
      this.btnClearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearFilter.Location = new Point(619, 4);
      this.btnClearFilter.Name = "btnClearFilter";
      this.btnClearFilter.Size = new Size(75, 22);
      this.btnClearFilter.TabIndex = 1;
      this.btnClearFilter.Text = "Clear Filter";
      this.btnClearFilter.UseVisualStyleBackColor = true;
      this.btnClearFilter.Click += new EventHandler(this.btnClearFilter_Click);
      this.lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDescription.AutoEllipsis = true;
      this.lblDescription.BackColor = Color.Transparent;
      this.lblDescription.Location = new Point(6, 8);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(608, 15);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Filter:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gvPlanCodes);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PlanCodeListControl);
      this.Size = new Size(699, 324);
      this.gradientPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
