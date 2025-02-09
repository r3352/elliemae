// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.PlanCodeSelectionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
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
  public class PlanCodeSelectionDialog : Form
  {
    private Plan selectedPlan;
    private Investor selectedInvestor;
    private PlanCodeSelectionDialog.SelectionMode selectionMode;
    private DocumentOrderType orderType;
    private int planCount = -1;
    private bool accessToOpeningDoc;
    private bool accessToClosingDoc;
    private IContainer components;
    private GroupContainer grpPlanCodes;
    private DialogButtons dlgButtons;
    private PlanCodeListControl lstPlanCodes;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnAddPlanCode;
    private Panel pnlInvestor;
    private ComboBox cboInvestor;
    private Label label1;
    private StandardIconButton btnRemovePlanCode;
    private TabControl tabsPlanCodes;
    private TabPage tpPlanCodes;
    private TabPage tpCustom;
    private GroupContainer grpCustom;
    private GridView gvCustom;
    private FlowLayoutPanel flowLayoutPanel2;
    private Label label2;
    private RadioButton radClosing;
    private RadioButton radOpening;
    private Panel panel1;
    private GradientPanel pnlPlanType;
    private ToolTip toolTip1;
    private StandardIconButton btnViewPlanCode;

    public PlanCodeSelectionDialog(
      PlanCodeSelectionDialog.SelectionMode selectionMode,
      DocumentOrderType orderType)
    {
      this.selectionMode = selectionMode;
      this.orderType = orderType;
      this.InitializeComponent();
      this.lstPlanCodes.AssignSession(Session.DefaultInstance);
      this.lstPlanCodes.ShowHiddenInvestorNames = this.selectionMode == PlanCodeSelectionDialog.SelectionMode.Template;
      this.lstPlanCodes.ShowPlanCodeTypeColumn = this.selectionMode == PlanCodeSelectionDialog.SelectionMode.Template;
      this.pnlPlanType.Visible = selectionMode == PlanCodeSelectionDialog.SelectionMode.Template && orderType == DocumentOrderType.Both;
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      this.accessToClosingDoc = service.IsEncompassDocServiceAvailable(DocumentOrderType.Closing);
      this.accessToOpeningDoc = service.IsEncompassDocServiceAvailable(DocumentOrderType.Opening);
      if (!this.accessToClosingDoc && !this.accessToOpeningDoc)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not licensed to use the Encompass Docs Solution. Contact your Encompass System Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      if (orderType == DocumentOrderType.Closing)
        this.radClosing.Checked = true;
      else
        this.radOpening.Checked = true;
      this.btnAddPlanCode.Enabled = Session.ACL.IsAuthorizedForFeature(AclFeature.LoanTab_Other_PlanCode);
      this.btnAddPlanCode.Visible = this.btnAddPlanCode.Enabled;
      this.btnRemovePlanCode.Visible = this.btnAddPlanCode.Enabled;
    }

    public PlanCodeSelectionDialog(
      PlanCodeSelectionDialog.SelectionMode selectionMode,
      DocumentOrderType orderType,
      IHtmlInput filter)
      : this(selectionMode, orderType)
    {
      this.lstPlanCodes.ApplyFilter(filter);
      this.refreshPlanCodeCount();
      if (this.lstPlanCodes.PlanCodeCount != 0)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "There are no available plan codes that match the criteria from your loan. All available plan codes will be shown.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.lstPlanCodes.ClearFilters();
      this.refreshPlanCodeCount();
    }

    public Plan SelectedPlan => this.selectedPlan;

    public Investor SelectedInvestor => this.selectedInvestor;

    public DocumentOrderType SelectedOrderType => this.orderType;

    public bool AllowAddToCompanyList
    {
      get => this.btnAddPlanCode.Visible;
      set => this.btnAddPlanCode.Visible = value;
    }

    private void refreshPlanCodes()
    {
      using (CursorActivator.Wait())
      {
        this.lstPlanCodes.ClearPlans();
        this.cboInvestor.Items.Clear();
        this.gvCustom.Items.Clear();
        this.cboInvestor.Items.Add((object) "");
        List<Plan> planList1 = new List<Plan>();
        List<Plan> planList2 = new List<Plan>();
        foreach (Plan companyPlan in Plans.GetCompanyPlans(Session.SessionObjects, this.orderType, true))
        {
          if ((this.orderType != DocumentOrderType.Opening || this.accessToOpeningDoc) && (this.orderType != DocumentOrderType.Closing || this.accessToClosingDoc))
          {
            if (companyPlan.PlanType == PlanType.Custom)
              planList2.Add(companyPlan);
            else
              planList1.Add(companyPlan);
          }
        }
        this.planCount = planList1.Count + planList2.Count;
        this.lstPlanCodes.LoadPlans(planList1.ToArray());
        this.loadCustomPlans(planList2.ToArray());
        foreach (Plan plan in planList1)
        {
          Investor investor = plan.GetInvestor();
          if (!plan.HideInvestorName && !investor.IsGeneric && !this.cboInvestor.Items.Contains((object) investor))
            this.cboInvestor.Items.Add((object) investor);
        }
        this.refreshPlanCodeCount();
        if (planList2.Count == 0 && this.tabsPlanCodes.TabPages.Contains(this.tpCustom))
          this.tabsPlanCodes.TabPages.Remove(this.tpCustom);
        else if (planList2.Count > 0 && !this.tabsPlanCodes.TabPages.Contains(this.tpCustom))
          this.tabsPlanCodes.TabPages.Add(this.tpCustom);
        if (planList1.Count != 0 || planList2.Count <= 0)
          return;
        this.tabsPlanCodes.SelectedTab = this.tpCustom;
      }
    }

    private void loadCustomPlans(Plan[] customPlans)
    {
      this.gvCustom.Items.Clear();
      foreach (Plan customPlan in customPlans)
        this.gvCustom.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = customPlan.InvestorPlanCode
            },
            [1] = {
              Text = customPlan.Description
            },
            [2] = {
              Text = customPlan.HideInvestorName ? "" : customPlan.InvestorName
            }
          },
          Tag = (object) customPlan
        });
      this.grpCustom.Text = "Custom Plan Codes (" + (object) this.gvCustom.Items.Count + ")";
    }

    private void refreshPlanCodeCount()
    {
      this.grpPlanCodes.Text = "Plan Codes (" + (object) this.lstPlanCodes.PlanCodeCount + ")";
    }

    private void lstPlanCodes_PlanCodeDoubleClick(object sender, PlanCodeEventArgs eventArgs)
    {
      this.selectPlan(eventArgs.Plan);
    }

    private void selectPlan(Plan plan)
    {
      if (this.selectionMode == PlanCodeSelectionDialog.SelectionMode.Template && plan.PlanType != PlanType.Custom && !this.confirmTemplateSelection())
        return;
      if (this.pnlInvestor.Visible && this.cboInvestor.SelectedIndex > 0)
        this.selectedInvestor = (Investor) this.cboInvestor.SelectedItem;
      this.selectedPlan = plan;
      this.DialogResult = DialogResult.OK;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.tabsPlanCodes.SelectedTab == this.tpPlanCodes && this.lstPlanCodes.SelectedPlan == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select a Plan Code from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.tabsPlanCodes.SelectedTab == this.tpCustom && this.gvCustom.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must first select a Plan Code from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.tabsPlanCodes.SelectedTab == this.tpPlanCodes)
        this.selectPlan(this.lstPlanCodes.SelectedPlan);
      else
        this.selectPlan(this.gvCustom.SelectedItems[0].Tag as Plan);
    }

    private bool confirmTemplateSelection()
    {
      return Utils.Dialog((IWin32Window) this, "By selecting this plan code, all values in the Basic Fields section will be overwritten with data from the selected plan. You will not be able to modify these fields unless you remove the Plan Code from the template.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK;
    }

    private void btnAddPlanCode_Click(object sender, EventArgs e)
    {
      if (this.orderType == DocumentOrderType.Closing && !this.accessToClosingDoc || this.orderType == DocumentOrderType.Opening && !this.accessToOpeningDoc)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to " + (this.orderType == DocumentOrderType.Opening ? "Opening Doc." : "Closing Doc."), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PlanCodeSelectListAddDialog selectListAddDialog = new PlanCodeSelectListAddDialog(this.orderType, this.selectionMode == PlanCodeSelectionDialog.SelectionMode.Template, Session.DefaultInstance))
        {
          if (selectListAddDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          string[] selectedPlanIds = selectListAddDialog.GetSelectedPlanIDs();
          this.refreshPlanCodes();
          if (selectedPlanIds.Length == 0)
            return;
          this.markPlanAsSelected(selectedPlanIds[0]);
        }
      }
    }

    private void btnRemovePlanCode_Click(object sender, EventArgs e)
    {
      Plan[] selectedPlans = this.lstPlanCodes.GetSelectedPlans();
      if (selectedPlans.Length == 0)
        return;
      Plan plan = selectedPlans[0];
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the plan '" + plan.Description + "' from your company's short list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      Session.ConfigurationManager.RemoveCompanyPlanCodes(this.orderType, new string[1]
      {
        plan.PlanID
      });
      this.lstPlanCodes.RemoveSelectedPlans();
      this.refreshPlanCodeCount();
    }

    private void markPlanAsSelected(string planId)
    {
      this.lstPlanCodes.ClearSelections();
      this.lstPlanCodes.SelectPlan(planId);
    }

    private void lstPlanCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstPlanCodes.SelectedPlan == null)
        this.pnlInvestor.Visible = false;
      else
        this.pnlInvestor.Visible = this.lstPlanCodes.SelectedPlan.GetInvestor().IsGeneric;
      this.btnRemovePlanCode.Enabled = this.lstPlanCodes.SelectedPlan is StandardPlan;
      this.btnViewPlanCode.Enabled = this.lstPlanCodes.SelectedPlanCodeCount == 1;
    }

    private void PlanCodeSelectionDialog_Shown(object sender, EventArgs e)
    {
      if (this.planCount == 0 && this.btnAddPlanCode.Enabled)
      {
        if (Utils.Dialog((IWin32Window) this, "You currently have no Plan Codes in your short list. Would you like to add one or more Plan Codes now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.btnAddPlanCode_Click((object) null, (EventArgs) null);
      }
      else
      {
        if (this.planCount != 0)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no Plan Codes in your company's short list. Your System Administrator must add one or more plan codes to this list in order for you to proceed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void lstPlanCodes_FilterChanged(object sender, EventArgs e)
    {
      this.refreshPlanCodeCount();
    }

    private void gvCustom_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.selectPlan((Plan) e.Item.Tag);
    }

    private void radOpening_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radOpening.Checked)
        return;
      this.orderType = DocumentOrderType.Opening;
      this.refreshPlanCodes();
    }

    private void radClosing_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radClosing.Checked)
        return;
      this.orderType = DocumentOrderType.Closing;
      this.refreshPlanCodes();
    }

    private void btnViewPlanCode_Click(object sender, EventArgs e)
    {
      Plan selectedPlan = this.lstPlanCodes.SelectedPlan;
      if (selectedPlan == null)
        return;
      using (PlanCodeDetailsDialog codeDetailsDialog = new PlanCodeDetailsDialog(selectedPlan, Session.DefaultInstance))
      {
        int num = (int) codeDetailsDialog.ShowDialog((IWin32Window) this);
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.grpPlanCodes = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemovePlanCode = new StandardIconButton();
      this.btnViewPlanCode = new StandardIconButton();
      this.btnAddPlanCode = new StandardIconButton();
      this.lstPlanCodes = new PlanCodeListControl();
      this.dlgButtons = new DialogButtons();
      this.pnlInvestor = new Panel();
      this.cboInvestor = new ComboBox();
      this.label1 = new Label();
      this.tabsPlanCodes = new TabControl();
      this.tpPlanCodes = new TabPage();
      this.tpCustom = new TabPage();
      this.grpCustom = new GroupContainer();
      this.gvCustom = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.label2 = new Label();
      this.radClosing = new RadioButton();
      this.radOpening = new RadioButton();
      this.panel1 = new Panel();
      this.pnlPlanType = new GradientPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.grpPlanCodes.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemovePlanCode).BeginInit();
      ((ISupportInitialize) this.btnViewPlanCode).BeginInit();
      ((ISupportInitialize) this.btnAddPlanCode).BeginInit();
      this.pnlInvestor.SuspendLayout();
      this.tabsPlanCodes.SuspendLayout();
      this.tpPlanCodes.SuspendLayout();
      this.tpCustom.SuspendLayout();
      this.grpCustom.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlPlanType.SuspendLayout();
      this.SuspendLayout();
      this.grpPlanCodes.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpPlanCodes.Controls.Add((Control) this.lstPlanCodes);
      this.grpPlanCodes.Dock = DockStyle.Fill;
      this.grpPlanCodes.HeaderForeColor = SystemColors.ControlText;
      this.grpPlanCodes.Location = new Point(3, 3);
      this.grpPlanCodes.Name = "grpPlanCodes";
      this.grpPlanCodes.Size = new Size(673, 258);
      this.grpPlanCodes.TabIndex = 0;
      this.grpPlanCodes.Text = "Plan Codes";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemovePlanCode);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnViewPlanCode);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddPlanCode);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(602, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(65, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.btnRemovePlanCode.BackColor = Color.Transparent;
      this.btnRemovePlanCode.Enabled = false;
      this.btnRemovePlanCode.Location = new Point(46, 3);
      this.btnRemovePlanCode.MouseDownImage = (Image) null;
      this.btnRemovePlanCode.Name = "btnRemovePlanCode";
      this.btnRemovePlanCode.Size = new Size(16, 16);
      this.btnRemovePlanCode.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePlanCode.TabIndex = 3;
      this.btnRemovePlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemovePlanCode, "Remove Plan Code from Company List");
      this.btnRemovePlanCode.Click += new EventHandler(this.btnRemovePlanCode_Click);
      this.btnViewPlanCode.BackColor = Color.Transparent;
      this.btnViewPlanCode.Location = new Point(25, 3);
      this.btnViewPlanCode.Margin = new Padding(3, 3, 2, 3);
      this.btnViewPlanCode.MouseDownImage = (Image) null;
      this.btnViewPlanCode.Name = "btnViewPlanCode";
      this.btnViewPlanCode.Size = new Size(16, 16);
      this.btnViewPlanCode.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewPlanCode.TabIndex = 4;
      this.btnViewPlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnViewPlanCode, "View Plan Code Details");
      this.btnViewPlanCode.Click += new EventHandler(this.btnViewPlanCode_Click);
      this.btnAddPlanCode.BackColor = Color.Transparent;
      this.btnAddPlanCode.Location = new Point(3, 3);
      this.btnAddPlanCode.MouseDownImage = (Image) null;
      this.btnAddPlanCode.Name = "btnAddPlanCode";
      this.btnAddPlanCode.Size = new Size(16, 16);
      this.btnAddPlanCode.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPlanCode.TabIndex = 0;
      this.btnAddPlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddPlanCode, "Add Plan Code to Company List");
      this.btnAddPlanCode.Click += new EventHandler(this.btnAddPlanCode_Click);
      this.lstPlanCodes.Dock = DockStyle.Fill;
      this.lstPlanCodes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lstPlanCodes.Location = new Point(1, 26);
      this.lstPlanCodes.Name = "lstPlanCodes";
      this.lstPlanCodes.SelectedPlan = (Plan) null;
      this.lstPlanCodes.Size = new Size(671, 231);
      this.lstPlanCodes.TabIndex = 0;
      this.lstPlanCodes.PlanCodeDoubleClick += new PlanCodeEventHandler(this.lstPlanCodes_PlanCodeDoubleClick);
      this.lstPlanCodes.FilterChanged += new EventHandler(this.lstPlanCodes_FilterChanged);
      this.lstPlanCodes.SelectedIndexChanged += new EventHandler(this.lstPlanCodes_SelectedIndexChanged);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 366);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Select";
      this.dlgButtons.Size = new Size(707, 39);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.pnlInvestor.Controls.Add((Control) this.cboInvestor);
      this.pnlInvestor.Controls.Add((Control) this.label1);
      this.pnlInvestor.Dock = DockStyle.Bottom;
      this.pnlInvestor.Location = new Point(3, 261);
      this.pnlInvestor.Name = "pnlInvestor";
      this.pnlInvestor.Size = new Size(673, 30);
      this.pnlInvestor.TabIndex = 2;
      this.pnlInvestor.Visible = false;
      this.cboInvestor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboInvestor.FormattingEnabled = true;
      this.cboInvestor.Location = new Point(100, 7);
      this.cboInvestor.Name = "cboInvestor";
      this.cboInvestor.Size = new Size(203, 22);
      this.cboInvestor.Sorted = true;
      this.cboInvestor.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(-3, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(98, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Apply Investor Info";
      this.tabsPlanCodes.Controls.Add((Control) this.tpPlanCodes);
      this.tabsPlanCodes.Controls.Add((Control) this.tpCustom);
      this.tabsPlanCodes.Dock = DockStyle.Fill;
      this.tabsPlanCodes.ItemSize = new Size(57, 23);
      this.tabsPlanCodes.Location = new Point(10, 10);
      this.tabsPlanCodes.Name = "tabsPlanCodes";
      this.tabsPlanCodes.SelectedIndex = 0;
      this.tabsPlanCodes.Size = new Size(687, 325);
      this.tabsPlanCodes.TabIndex = 3;
      this.tpPlanCodes.Controls.Add((Control) this.grpPlanCodes);
      this.tpPlanCodes.Controls.Add((Control) this.pnlInvestor);
      this.tpPlanCodes.Location = new Point(4, 27);
      this.tpPlanCodes.Name = "tpPlanCodes";
      this.tpPlanCodes.Padding = new Padding(3);
      this.tpPlanCodes.Size = new Size(679, 294);
      this.tpPlanCodes.TabIndex = 0;
      this.tpPlanCodes.Text = "Plan Codes";
      this.tpPlanCodes.UseVisualStyleBackColor = true;
      this.tpCustom.Controls.Add((Control) this.grpCustom);
      this.tpCustom.Location = new Point(4, 27);
      this.tpCustom.Name = "tpCustom";
      this.tpCustom.Padding = new Padding(3);
      this.tpCustom.Size = new Size(679, 294);
      this.tpCustom.TabIndex = 1;
      this.tpCustom.Text = "Custom Plan Codes";
      this.tpCustom.UseVisualStyleBackColor = true;
      this.grpCustom.Controls.Add((Control) this.gvCustom);
      this.grpCustom.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpCustom.Dock = DockStyle.Fill;
      this.grpCustom.HeaderForeColor = SystemColors.ControlText;
      this.grpCustom.Location = new Point(3, 3);
      this.grpCustom.Name = "grpCustom";
      this.grpCustom.Size = new Size(673, 288);
      this.grpCustom.TabIndex = 1;
      this.grpCustom.Text = "Custom Plan Codes";
      this.gvCustom.AllowMultiselect = false;
      this.gvCustom.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Plan Code";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Investor";
      gvColumn3.Width = 150;
      this.gvCustom.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvCustom.Dock = DockStyle.Fill;
      this.gvCustom.Location = new Point(1, 26);
      this.gvCustom.Name = "gvCustom";
      this.gvCustom.Size = new Size(671, 261);
      this.gvCustom.TabIndex = 2;
      this.gvCustom.ItemDoubleClick += new GVItemEventHandler(this.gvCustom_ItemDoubleClick);
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(667, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(0, 0);
      this.flowLayoutPanel2.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Plan Type:";
      this.radClosing.AutoSize = true;
      this.radClosing.BackColor = Color.Transparent;
      this.radClosing.Location = new Point(170, 7);
      this.radClosing.Name = "radClosing";
      this.radClosing.Size = new Size(88, 18);
      this.radClosing.TabIndex = 1;
      this.radClosing.TabStop = true;
      this.radClosing.Text = "Closing Docs";
      this.radClosing.UseVisualStyleBackColor = false;
      this.radClosing.CheckedChanged += new EventHandler(this.radClosing_CheckedChanged);
      this.radOpening.AutoSize = true;
      this.radOpening.BackColor = Color.Transparent;
      this.radOpening.Location = new Point(77, 7);
      this.radOpening.Name = "radOpening";
      this.radOpening.Size = new Size(88, 18);
      this.radOpening.TabIndex = 0;
      this.radOpening.TabStop = true;
      this.radOpening.Text = "eDisclosures";
      this.radOpening.UseVisualStyleBackColor = false;
      this.radOpening.CheckedChanged += new EventHandler(this.radOpening_CheckedChanged);
      this.panel1.Controls.Add((Control) this.tabsPlanCodes);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 31);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(10, 10, 10, 0);
      this.panel1.Size = new Size(707, 335);
      this.panel1.TabIndex = 5;
      this.pnlPlanType.Borders = AnchorStyles.Bottom;
      this.pnlPlanType.Controls.Add((Control) this.label2);
      this.pnlPlanType.Controls.Add((Control) this.radClosing);
      this.pnlPlanType.Controls.Add((Control) this.radOpening);
      this.pnlPlanType.Dock = DockStyle.Top;
      this.pnlPlanType.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlPlanType.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlPlanType.Location = new Point(0, 0);
      this.pnlPlanType.Name = "pnlPlanType";
      this.pnlPlanType.Size = new Size(707, 31);
      this.pnlPlanType.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlPlanType.TabIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(707, 405);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.pnlPlanType);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (PlanCodeSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Plan Code";
      this.Shown += new EventHandler(this.PlanCodeSelectionDialog_Shown);
      this.grpPlanCodes.ResumeLayout(false);
      this.grpPlanCodes.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemovePlanCode).EndInit();
      ((ISupportInitialize) this.btnViewPlanCode).EndInit();
      ((ISupportInitialize) this.btnAddPlanCode).EndInit();
      this.pnlInvestor.ResumeLayout(false);
      this.pnlInvestor.PerformLayout();
      this.tabsPlanCodes.ResumeLayout(false);
      this.tpPlanCodes.ResumeLayout(false);
      this.tpCustom.ResumeLayout(false);
      this.grpCustom.ResumeLayout(false);
      this.grpCustom.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.pnlPlanType.ResumeLayout(false);
      this.pnlPlanType.PerformLayout();
      this.ResumeLayout(false);
    }

    public enum SelectionMode
    {
      Template = 1,
      Loan = 2,
    }
  }
}
