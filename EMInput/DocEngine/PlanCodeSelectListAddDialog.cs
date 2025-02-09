// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.PlanCodeSelectListAddDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
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
  public class PlanCodeSelectListAddDialog : Form
  {
    private DocumentOrderType orderType;
    private Dictionary<string, PlanCodeInfo> companyPlanCodeLookup = new Dictionary<string, PlanCodeInfo>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private List<string> selectedPlanIDs;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private ComboBox cboInvestor;
    private GroupContainer grpPlanCodes;
    private PlanCodeListControl lstPlanCodes;
    private FlowLayoutPanel flowLayoutPanel1;
    private ToolTip toolTip1;
    private Button btnClose;
    private Button btnAddPlans;
    private Button btnAddAlias;
    private Button btnAddCustom;
    private StandardIconButton btnViewPlanCode;
    private CheckBox chkShowAdded;

    public PlanCodeSelectListAddDialog(DocumentOrderType orderType, bool allowCreateCustom)
      : this(orderType, allowCreateCustom, Session.DefaultInstance)
    {
    }

    public PlanCodeSelectListAddDialog(
      DocumentOrderType orderType,
      bool allowCreateCustom,
      Sessions.Session session)
    {
      this.orderType = orderType;
      this.session = session;
      this.InitializeComponent();
      this.lstPlanCodes.AssignSession(this.session);
      this.loadCompanyPlanCodes();
      this.loadInvestors();
      if (allowCreateCustom)
        return;
      this.btnAddAlias.Visible = this.btnAddCustom.Visible = false;
      this.btnAddPlans.Left = this.btnAddAlias.Left;
    }

    public string[] GetSelectedPlanIDs()
    {
      return this.selectedPlanIDs != null ? this.selectedPlanIDs.ToArray() : new string[0];
    }

    private void loadCompanyPlanCodes()
    {
      foreach (PlanCodeInfo companyPlanCode in this.session.ConfigurationManager.GetCompanyPlanCodes(this.orderType))
      {
        if (!companyPlanCode.IsCustom)
          this.companyPlanCodeLookup[companyPlanCode.PlanID] = companyPlanCode;
      }
    }

    private void loadInvestors()
    {
      using (DocEngineService docEngineService = new DocEngineService(this.session.SessionObjects))
      {
        foreach (object investor in docEngineService.GetInvestors(this.orderType, true))
          this.cboInvestor.Items.Add(investor);
      }
    }

    private void refreshPlanCodeCount()
    {
      this.grpPlanCodes.Text = "ICE Mortgage Technology Plan Codes (" + (object) this.lstPlanCodes.PlanCodeCount + ")";
    }

    private void cboInvestor_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboInvestor.SelectedIndex < 0)
        return;
      this.loadPlans((Investor) this.cboInvestor.SelectedItem);
    }

    private void loadPlans(Investor inv)
    {
      using (CursorActivator.Wait())
      {
        using (DocEngineService docEngineService = new DocEngineService(this.session.SessionObjects))
        {
          List<Plan> planList1 = new List<Plan>((IEnumerable<Plan>) docEngineService.GetPlansForInvestor(inv.InvestorCode));
          List<Plan> planList2 = new List<Plan>();
          foreach (Plan plan in planList1)
          {
            if (!this.isApplicablePlanType(plan.OrderType))
              planList2.Add(plan);
            else if (!this.chkShowAdded.Checked)
            {
              foreach (PlanCodeInfo planCodeInfo in this.companyPlanCodeLookup.Values)
              {
                if (plan.ToPlanCodeInfo().Equals((object) planCodeInfo))
                {
                  planList2.Add(plan);
                  break;
                }
              }
            }
          }
          foreach (Plan plan in planList2)
            planList1.Remove(plan);
          this.lstPlanCodes.LoadPlans(planList1.ToArray());
          if (this.chkShowAdded.Checked)
          {
            foreach (PlanCodeInfo planInfo in this.companyPlanCodeLookup.Values)
              this.lstPlanCodes.HighlightPlan(planInfo);
          }
          this.refreshPlanCodeCount();
        }
      }
    }

    private bool isApplicablePlanType(DocumentOrderType planType)
    {
      return (planType & this.orderType) == this.orderType;
    }

    private void btnAddPlan_Click(object sender, EventArgs e)
    {
      Plan[] selectedPlans = this.lstPlanCodes.GetSelectedPlans();
      if (selectedPlans.Length == 1 && this.companyPlanCodeLookup.ContainsKey(selectedPlans[0].PlanID))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected Plan Code is already part of your company list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Add the " + (object) selectedPlans.Length + " selected Plan Code(s) to your company list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        List<PlanCodeInfo> planCodeInfoList = new List<PlanCodeInfo>();
        this.selectedPlanIDs = new List<string>();
        foreach (Plan plan in selectedPlans)
        {
          planCodeInfoList.Add(plan.ToPlanCodeInfo());
          this.selectedPlanIDs.Add(plan.PlanID);
        }
        this.session.ConfigurationManager.AddCompanyPlanCodes(this.orderType, planCodeInfoList.ToArray());
        this.DialogResult = DialogResult.OK;
      }
    }

    private void lstPlanCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = false;
      foreach (Plan selectedPlan in this.lstPlanCodes.GetSelectedPlans())
      {
        if (!this.companyPlanCodeLookup.ContainsKey(selectedPlan.PlanID))
        {
          flag = true;
          break;
        }
      }
      this.btnAddAlias.Enabled = this.lstPlanCodes.SelectedPlanCodeCount == 1;
      this.btnViewPlanCode.Enabled = this.lstPlanCodes.SelectedPlanCodeCount == 1;
      this.btnAddPlans.Enabled = flag;
    }

    private void lstPlanCodes_PlanCodeDoubleClick(object sender, PlanCodeEventArgs eventArgs)
    {
      if (this.companyPlanCodeLookup.ContainsKey(eventArgs.Plan.PlanID))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected Plan Code is already part of your company list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        List<PlanCodeInfo> planCodeInfoList = new List<PlanCodeInfo>();
        planCodeInfoList.Add(eventArgs.Plan.ToPlanCodeInfo());
        this.selectedPlanIDs = new List<string>();
        this.selectedPlanIDs.Add(eventArgs.Plan.PlanID);
        this.session.ConfigurationManager.AddCompanyPlanCodes(this.orderType, planCodeInfoList.ToArray());
        this.DialogResult = DialogResult.OK;
      }
    }

    private void lstPlanCodes_FilterChanged(object sender, EventArgs e)
    {
      this.refreshPlanCodeCount();
    }

    private void btnAddAlias_Click(object sender, EventArgs e)
    {
      if (this.lstPlanCodes.SelectedPlan == null || new CustomPlanCodeDialog(this.lstPlanCodes.SelectedPlan, this.orderType, this.session).ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.btnClose.PerformClick();
      this.DialogResult = DialogResult.OK;
    }

    private void btnAddCustom_Click(object sender, EventArgs e)
    {
      this.Visible = false;
      if (new CustomPlanCodeDialog((Plan) null, this.orderType, this.session).ShowDialog((IWin32Window) this) == DialogResult.OK)
      {
        this.btnClose.PerformClick();
        this.DialogResult = DialogResult.OK;
      }
      else
        this.Visible = true;
    }

    private void PlanCodeSelectListAddDialog_Shown(object sender, EventArgs e)
    {
      this.cboInvestor.DroppedDown = true;
    }

    private void btnViewPlanCode_Click(object sender, EventArgs e)
    {
      Plan selectedPlan = this.lstPlanCodes.SelectedPlan;
      if (selectedPlan == null)
        return;
      using (PlanCodeDetailsDialog codeDetailsDialog = new PlanCodeDetailsDialog(selectedPlan, this.session))
      {
        int num = (int) codeDetailsDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void chkShowAdded_CheckedChanged(object sender, EventArgs e)
    {
      this.cboInvestor_SelectedIndexChanged((object) null, (EventArgs) null);
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
      this.label1 = new Label();
      this.cboInvestor = new ComboBox();
      this.toolTip1 = new ToolTip(this.components);
      this.btnViewPlanCode = new StandardIconButton();
      this.btnClose = new Button();
      this.btnAddPlans = new Button();
      this.btnAddAlias = new Button();
      this.btnAddCustom = new Button();
      this.grpPlanCodes = new GroupContainer();
      this.chkShowAdded = new CheckBox();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lstPlanCodes = new PlanCodeListControl();
      ((ISupportInitialize) this.btnViewPlanCode).BeginInit();
      this.grpPlanCodes.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select an Investor";
      this.cboInvestor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboInvestor.FormattingEnabled = true;
      this.cboInvestor.Location = new Point(107, 10);
      this.cboInvestor.Name = "cboInvestor";
      this.cboInvestor.Size = new Size(267, 22);
      this.cboInvestor.Sorted = true;
      this.cboInvestor.TabIndex = 1;
      this.cboInvestor.SelectedIndexChanged += new EventHandler(this.cboInvestor_SelectedIndexChanged);
      this.btnViewPlanCode.BackColor = Color.Transparent;
      this.btnViewPlanCode.Enabled = false;
      this.btnViewPlanCode.Location = new Point(3, 3);
      this.btnViewPlanCode.Margin = new Padding(3, 3, 2, 3);
      this.btnViewPlanCode.MouseDownImage = (Image) null;
      this.btnViewPlanCode.Name = "btnViewPlanCode";
      this.btnViewPlanCode.Size = new Size(16, 16);
      this.btnViewPlanCode.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewPlanCode.TabIndex = 6;
      this.btnViewPlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnViewPlanCode, "View Plan Code Details");
      this.btnViewPlanCode.Click += new EventHandler(this.btnViewPlanCode_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(632, 383);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "&Cancel";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnAddPlans.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddPlans.Enabled = false;
      this.btnAddPlans.Location = new Point(468, 383);
      this.btnAddPlans.Name = "btnAddPlans";
      this.btnAddPlans.Size = new Size(79, 23);
      this.btnAddPlans.TabIndex = 4;
      this.btnAddPlans.Text = "&Add";
      this.btnAddPlans.UseVisualStyleBackColor = true;
      this.btnAddPlans.Click += new EventHandler(this.btnAddPlan_Click);
      this.btnAddAlias.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddAlias.Enabled = false;
      this.btnAddAlias.Location = new Point(550, 383);
      this.btnAddAlias.Name = "btnAddAlias";
      this.btnAddAlias.Size = new Size(79, 23);
      this.btnAddAlias.TabIndex = 5;
      this.btnAddAlias.Text = "Add as Alias";
      this.btnAddAlias.UseVisualStyleBackColor = true;
      this.btnAddAlias.Click += new EventHandler(this.btnAddAlias_Click);
      this.btnAddCustom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnAddCustom.Location = new Point(10, 383);
      this.btnAddCustom.Name = "btnAddCustom";
      this.btnAddCustom.Size = new Size(91, 23);
      this.btnAddCustom.TabIndex = 6;
      this.btnAddCustom.Text = "Create Custom";
      this.btnAddCustom.UseVisualStyleBackColor = true;
      this.btnAddCustom.Click += new EventHandler(this.btnAddCustom_Click);
      this.grpPlanCodes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpPlanCodes.Controls.Add((Control) this.chkShowAdded);
      this.grpPlanCodes.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpPlanCodes.Controls.Add((Control) this.lstPlanCodes);
      this.grpPlanCodes.HeaderForeColor = SystemColors.ControlText;
      this.grpPlanCodes.Location = new Point(10, 41);
      this.grpPlanCodes.Name = "grpPlanCodes";
      this.grpPlanCodes.Size = new Size(697, 330);
      this.grpPlanCodes.TabIndex = 2;
      this.grpPlanCodes.Text = "ICE Mortgage Technology Plan Codes";
      this.chkShowAdded.AutoSize = true;
      this.chkShowAdded.BackColor = Color.Transparent;
      this.chkShowAdded.Location = new Point(262, 5);
      this.chkShowAdded.Name = "chkShowAdded";
      this.chkShowAdded.Size = new Size(183, 18);
      this.chkShowAdded.TabIndex = 2;
      this.chkShowAdded.Text = "Show already added plan codes";
      this.chkShowAdded.UseVisualStyleBackColor = false;
      this.chkShowAdded.CheckedChanged += new EventHandler(this.chkShowAdded_CheckedChanged);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnViewPlanCode);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(670, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(21, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.lstPlanCodes.AllowMultiselect = true;
      this.lstPlanCodes.Dock = DockStyle.Fill;
      this.lstPlanCodes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lstPlanCodes.Location = new Point(1, 26);
      this.lstPlanCodes.Name = "lstPlanCodes";
      this.lstPlanCodes.SelectedPlan = (Plan) null;
      this.lstPlanCodes.Size = new Size(695, 303);
      this.lstPlanCodes.TabIndex = 0;
      this.lstPlanCodes.PlanCodeDoubleClick += new PlanCodeEventHandler(this.lstPlanCodes_PlanCodeDoubleClick);
      this.lstPlanCodes.FilterChanged += new EventHandler(this.lstPlanCodes_FilterChanged);
      this.lstPlanCodes.SelectedIndexChanged += new EventHandler(this.lstPlanCodes_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(717, 416);
      this.Controls.Add((Control) this.btnAddCustom);
      this.Controls.Add((Control) this.btnAddAlias);
      this.Controls.Add((Control) this.btnAddPlans);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.grpPlanCodes);
      this.Controls.Add((Control) this.cboInvestor);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (PlanCodeSelectListAddDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Plan Code";
      this.Shown += new EventHandler(this.PlanCodeSelectListAddDialog_Shown);
      ((ISupportInitialize) this.btnViewPlanCode).EndInit();
      this.grpPlanCodes.ResumeLayout(false);
      this.grpPlanCodes.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
