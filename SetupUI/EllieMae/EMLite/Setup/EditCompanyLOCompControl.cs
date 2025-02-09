// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyLOCompControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyLOCompControl : UserControl
  {
    private LOCompHistoryControl loCompHistoryControl;
    private LOCompCurrentControl loCompCurrentControl;
    private Sessions.Session session;
    private bool forLender;
    private int parent;
    private int oid;
    private bool edit;
    private LoanCompHistoryList loanCompHistoryList;
    private IContainer components;
    private Panel panelLOCompCurrent;
    private Panel panelLOCompHistory;
    private GroupContainer grpAll;
    private Panel panelHeader;
    private Label label33;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;

    public event EventHandler SaveButton_Clicked;

    public LoanCompHistoryList LOCompHistoryList => this.loanCompHistoryList;

    public EditCompanyLOCompControl(
      Sessions.Session session,
      int oid,
      int parent,
      bool forLender,
      bool edit)
    {
      this.session = session;
      this.forLender = forLender;
      this.parent = parent;
      this.oid = oid;
      this.edit = edit;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.loCompHistoryControl = new LOCompHistoryControl(this.session, false, true, this.forLender);
      this.loCompCurrentControl = new LOCompCurrentControl(this.session, false, true, this.forLender);
      this.panelLOCompHistory.Controls.Add((Control) this.loCompHistoryControl);
      this.panelLOCompCurrent.Controls.Add((Control) this.loCompCurrentControl);
      this.initForm();
      this.loCompHistoryControl.HistorySelectedIndexChanged += new EventHandler(this.loCompHistoryControl_HistorySelectedIndexChanged);
      this.loCompHistoryControl.UseParentInfoClicked += new EventHandler(this.loCompHistoryControl_UseParentInfoClicked);
      this.loCompHistoryControl.AssignPlanButtonClicked += new EventHandler(this.loCompHistoryControl_AssignPlanButtonClicked);
      this.loCompHistoryControl.DeletePlanButtonClicked += new EventHandler(this.loCompHistoryControl_DeletePlanButtonClicked);
      this.loCompCurrentControl.StartDateChanged += new EventHandler(this.loCompCurrentControl_StartDateChanged);
      this.loCompHistoryControl.AutoSetGrid();
      if (!edit)
        return;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(oid, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void initForm()
    {
      if (this.edit)
      {
        this.loanCompHistoryList = this.session.ConfigurationManager.GetCompPlansByoid(this.forLender, this.oid);
        this.loanCompHistoryList.UseParentInfo = this.session.ConfigurationManager.GetUseParentInfoValue(this.forLender, this.oid);
      }
      else
        this.loanCompHistoryList = new LoanCompHistoryList(this.oid.ToString());
      this.loCompCurrentControl.RefreshDate(this.loanCompHistoryList, this.parent.ToString(), this.oid.ToString());
      this.loCompHistoryControl.RefreshData(this.loanCompHistoryList, this.parent.ToString(), this.oid.ToString());
      this.SetButtonStatus(false);
    }

    private void loCompHistoryControl_HistorySelectedIndexChanged(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompHistoryControl_UseParentInfoClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
      this.SetButtonStatus(true);
    }

    private void loCompHistoryControl_AssignPlanButtonClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
      this.SetButtonStatus(true);
    }

    private void loCompHistoryControl_DeletePlanButtonClicked(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void loCompCurrentControl_StartDateChanged(object sender, EventArgs e)
    {
      this.loCompHistoryControl.RefreshHistoryList((LoanCompHistory) sender);
      this.SetButtonStatus(true);
    }

    public void DisableButtons(bool value)
    {
      this.loCompHistoryControl.DisableButtons(value);
      if (!this.edit)
        return;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(this.oid, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public bool GetUseParentInfo() => this.loCompHistoryControl.GetUseParentInfo();

    public bool UncheckParentInfo() => this.loCompHistoryControl.UncheckParentInfo;

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.SaveButton_Clicked != null)
        this.SaveButton_Clicked(sender, e);
      this.edit = true;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.initForm();
    }

    public void SetButtonStatus(bool value) => this.btnSave.Enabled = this.btnReset.Enabled = value;

    public bool DataValidated()
    {
      return this.loCompHistoryControl == null || this.loCompHistoryControl.DataValidation(false);
    }

    public bool IsDirty => this.btnSave.Enabled;

    public void AssignOid(int oid)
    {
      if (this.oid == -1)
        this.oid = oid;
      this.initForm();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(oid, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public void DisableControls()
    {
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
      this.loCompHistoryControl.DisableControls();
      this.disableControl(this.Controls);
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
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
      this.panelLOCompCurrent = new Panel();
      this.panelLOCompHistory = new Panel();
      this.grpAll = new GroupContainer();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.panelLOCompCurrent.Location = new Point(4, 68);
      this.panelLOCompCurrent.Name = "panelLOCompCurrent";
      this.panelLOCompCurrent.Size = new Size(369, 311);
      this.panelLOCompCurrent.TabIndex = 20;
      this.panelLOCompHistory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelLOCompHistory.Location = new Point(378, 68);
      this.panelLOCompHistory.Name = "panelLOCompHistory";
      this.panelLOCompHistory.Size = new Size(484, 311);
      this.panelLOCompHistory.TabIndex = 21;
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Controls.Add((Control) this.panelLOCompCurrent);
      this.grpAll.Controls.Add((Control) this.panelLOCompHistory);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0, 0, 0, 0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 610);
      this.grpAll.TabIndex = 22;
      this.grpAll.Text = "LO Comp";
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(838, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 34;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(816, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 33;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 35);
      this.panelHeader.TabIndex = 1;
      this.label33.Location = new Point(-1, 4);
      this.label33.Name = "label33";
      this.label33.Padding = new Padding(4, 0, 0, 0);
      this.label33.Size = new Size(848, 35);
      this.label33.TabIndex = 36;
      this.label33.Text = "Assign and manage LO compensation plans for the company or branch. Before assigning a plan, use the Tables and Fees > LO Compensation setting to create LO compensation plans.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0, 0, 0, 0);
      this.Name = nameof (EditCompanyLOCompControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.grpAll.ResumeLayout(false);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
