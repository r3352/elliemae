// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PlanCodeManagementControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PlanCodeManagementControl : SettingsUserControl
  {
    private const string className = "PlanCodeManagementControl";
    private static string sw = Tracing.SwEpass;
    private Sessions.Session session;
    private DocumentOrderType orderType;
    private IContainer components;
    private GroupContainer grpPlanCodes;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnCreateLoanPrograms;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnRemovePlanCodes;
    private ToolTip toolTip1;
    private StandardIconButton btnAddPlanCodes;
    private PlanCodeListControl ctlPlanCodes;
    private StandardIconButton siBtnEditCustomPlanCode;
    private StandardIconButton btnViewPlanCode;

    public PlanCodeManagementControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      DocumentOrderType orderType)
      : this(setupContainer, session, orderType, false)
    {
    }

    public PlanCodeManagementControl(
      SetUpContainer container,
      Sessions.Session session,
      DocumentOrderType orderType,
      bool allowMultiSelect)
      : base(container)
    {
      this.session = session;
      this.orderType = orderType;
      this.InitializeComponent();
      this.ctlPlanCodes.AssignSession(this.session);
      this.ctlPlanCodes.AllowMultiselect = allowMultiSelect;
      this.refreshPlanCodes();
      this.Reset();
    }

    public override void Reset() => base.Reset();

    private void refreshPlanCodes()
    {
      using (CursorActivator.Wait())
      {
        this.ctlPlanCodes.LoadPlans(Plans.GetCompanyPlans(this.session.SessionObjects, this.orderType));
        this.refreshPlanCodeCount();
      }
    }

    private void refreshPlanCodeCount()
    {
      this.grpPlanCodes.Text = "Company Plan Codes (" + (object) this.ctlPlanCodes.PlanCodeCount + ")";
    }

    public override void Save() => base.Save();

    private void ctlPlanCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemovePlanCodes.Enabled = this.ctlPlanCodes.SelectedPlanCodeCount > 0;
      this.btnCreateLoanPrograms.Enabled = this.ctlPlanCodes.SelectedPlanCodeCount > 0;
      this.btnViewPlanCode.Enabled = this.ctlPlanCodes.SelectedPlanCodeCount == 1 && !(this.ctlPlanCodes.SelectedPlan is CustomPlan);
      if (this.ctlPlanCodes.SelectedPlanCodeCount != 1)
        this.siBtnEditCustomPlanCode.Enabled = false;
      else if (this.ctlPlanCodes.SelectedPlan.PlanType == PlanType.Standard)
        this.siBtnEditCustomPlanCode.Enabled = false;
      else
        this.siBtnEditCustomPlanCode.Enabled = true;
    }

    private void btnAddPlanCodes_Click(object sender, EventArgs e)
    {
      PlanCodeSelectListAddDialog selectListAddDialog = (PlanCodeSelectListAddDialog) null;
      using (CursorActivator.Wait())
        selectListAddDialog = new PlanCodeSelectListAddDialog(this.orderType, true, this.session);
      try
      {
        if (selectListAddDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.refreshPlanCodes();
      }
      finally
      {
        selectListAddDialog.Dispose();
      }
    }

    private void btnCreateLoanPrograms_Click(object sender, EventArgs e)
    {
      IFileSystem fileSystem = (IFileSystem) TemplateFileSystem.Create(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, this.session);
      FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
      using (FolderSelectionDialog folderSelectionDialog = new FolderSelectionDialog(fileSystem, true, this.session))
      {
        if (folderSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        fileSystemEntry = folderSelectionDialog.SelectedFolder;
      }
      Plan[] selectedPlans = this.ctlPlanCodes.GetSelectedPlans();
      using (ProgressDialog progressDialog = new ProgressDialog("Creating Loan Programs", new AsynchronousProcess(this.generateLoanPrograms), (object) new object[2]
      {
        (object) fileSystemEntry,
        (object) selectedPlans
      }, true))
      {
        if (progressDialog.ShowDialog((IWin32Window) this) == DialogResult.Abort)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, progressDialog.Value.ToString() + " Loan Programs were created in the '" + fileSystemEntry.ToDisplayString() + "' folder.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private DialogResult generateLoanPrograms(object state, IProgressFeedback feedback)
    {
      try
      {
        object[] objArray = (object[]) state;
        FileSystemEntry parentFolder = (FileSystemEntry) objArray[0];
        Plan[] planArray = (Plan[]) objArray[1];
        feedback.ResetCounter(planArray.Length);
        feedback.SetFeedback("Generating Loan Programs...", "", 0);
        foreach (Plan plan in planArray)
        {
          feedback.Details = "Generating '" + plan.Description + "'...";
          FileSystemEntry uniqueFileEntry = this.generateUniqueFileEntry(parentFolder, plan.Description);
          LoanProgram loanProgram = new LoanProgram();
          loanProgram.TemplateName = uniqueFileEntry.Name;
          loanProgram.Description = plan.Description;
          plan.Apply(loanProgram);
          this.session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, uniqueFileEntry, (BinaryObject) (BinaryConvertibleObject) loanProgram);
          if (!feedback.Increment(1))
            return DialogResult.Cancel;
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(PlanCodeManagementControl.sw, nameof (PlanCodeManagementControl), TraceLevel.Error, "Error generating loan programs: " + (object) ex);
        int num = (int) feedback.MsgBox("An error has occurred while generating the loan programs: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
    }

    private FileSystemEntry generateUniqueFileEntry(FileSystemEntry parentFolder, string baseName)
    {
      baseName = FileSystem.StripIllegalCharacters(baseName);
      if (baseName.Length == 0)
        baseName = "Plan";
      int num = 1;
      FileSystemEntry fileEntry;
      while (true)
      {
        string str;
        if (num != 1)
          str = baseName + " (" + (object) num + ")";
        else
          str = baseName;
        string path = str;
        fileEntry = parentFolder.Combine(path);
        if (this.session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileEntry))
          ++num;
        else
          break;
      }
      return fileEntry;
    }

    private void btnRemovePlanCodes_Click(object sender, EventArgs e)
    {
      Plan[] selectedPlans = this.ctlPlanCodes.GetSelectedPlans();
      if (Utils.Dialog((IWin32Window) this, "Remove the selected plan code(s) from your Company Plan Code list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (DocumentTemplate documentTemplate in documentTrackingSetup)
      {
        if (documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria.PlanCodeValues != null && documentTemplate.ClosingCriteria.PlanCodeValues.Length != 0)
        {
          foreach (string planCodeValue in documentTemplate.ClosingCriteria.PlanCodeValues)
          {
            if (!stringList1.Contains(planCodeValue))
              stringList1.Add(planCodeValue);
          }
        }
        if (documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria.PlanCodeValues != null && documentTemplate.OpeningCriteria.PlanCodeValues.Length != 0)
        {
          foreach (string planCodeValue in documentTemplate.OpeningCriteria.PlanCodeValues)
          {
            if (!stringList2.Contains(planCodeValue))
              stringList2.Add(planCodeValue);
          }
        }
      }
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      List<string> stringList5 = new List<string>();
      List<CustomPlanCode> planCodeList = new List<CustomPlanCode>();
      foreach (Plan plan in selectedPlans)
      {
        if (plan.PlanType == PlanType.Standard)
          stringList5.Add(plan.PlanID);
        else
          planCodeList.Add(new CustomPlanCode()
          {
            PlanCode = plan.ToPlanCodeInfo().PlanCode,
            OrderType = plan.OrderType
          });
        if (plan.OrderType == DocumentOrderType.Opening)
        {
          if (stringList2.Contains(plan.ToPlanCodeInfo().PlanCode) && Utils.Dialog((IWin32Window) this, "Do you want to remove '" + plan.ToPlanCodeInfo().PlanCode + "' from the document trigger criteria", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            stringList3.Add(plan.ToPlanCodeInfo().PlanCode);
        }
        else if (stringList1.Contains(plan.ToPlanCodeInfo().PlanCode) && Utils.Dialog((IWin32Window) this, "Do you want to remove '" + plan.ToPlanCodeInfo().PlanCode + "' from the document trigger criteria", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          stringList4.Add(plan.ToPlanCodeInfo().PlanCode);
      }
      if (stringList5.Count > 0)
        this.session.ConfigurationManager.RemoveCompanyPlanCodes(this.orderType, stringList5.ToArray());
      if (planCodeList.Count > 0)
        this.session.ConfigurationManager.RemoveCompanyCustomPlanCodes(this.orderType, planCodeList);
      foreach (DocumentTemplate documentTemplate in documentTrackingSetup)
      {
        if (documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria != null && documentTemplate.ClosingCriteria.PlanCodeValues != null && documentTemplate.ClosingCriteria.PlanCodeValues.Length != 0)
        {
          List<string> stringList6 = new List<string>((IEnumerable<string>) documentTemplate.ClosingCriteria.PlanCodeValues);
          foreach (string str in stringList4.ToArray())
          {
            if (stringList6.Contains(str))
              stringList6.Remove(str);
          }
          documentTemplate.ClosingCriteria.PlanCodeValues = stringList6.ToArray();
        }
        if (documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria != null && documentTemplate.OpeningCriteria.PlanCodeValues != null && documentTemplate.OpeningCriteria.PlanCodeValues.Length != 0)
        {
          List<string> stringList7 = new List<string>((IEnumerable<string>) documentTemplate.OpeningCriteria.PlanCodeValues);
          foreach (string str in stringList3.ToArray())
          {
            if (stringList7.Contains(str))
              stringList7.Remove(str);
          }
          documentTemplate.OpeningCriteria.PlanCodeValues = stringList7.ToArray();
        }
      }
      this.session.ConfigurationManager.SaveDocumentTrackingSetup(documentTrackingSetup);
      this.ctlPlanCodes.RemoveSelectedPlans();
      this.refreshPlanCodeCount();
    }

    private void ctlPlanCodes_FilterChanged(object sender, EventArgs e)
    {
      this.refreshPlanCodeCount();
    }

    private void btnResetSettings_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the Compliance Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.Reset();
    }

    private void btnSaveSettings_Click(object sender, EventArgs e) => this.Save();

    private void siBtnEditCustomPlanCode_Click(object sender, EventArgs e)
    {
      if (this.ctlPlanCodes.SelectedPlan == null || this.ctlPlanCodes.SelectedPlan.PlanType == PlanType.Standard)
        return;
      CustomPlanCode companyCustomPlanCode = this.session.ConfigurationManager.GetCompanyCustomPlanCode(this.ctlPlanCodes.SelectedPlan.OrderType, this.ctlPlanCodes.SelectedPlan.ToPlanCodeInfo().PlanCode);
      if (companyCustomPlanCode == null || new CustomPlanCodeDialog(companyCustomPlanCode, this.session).ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.refreshPlanCodes();
    }

    private void ctlPlanCodes_PlanCodeDoubleClick(object sender, PlanCodeEventArgs eventArgs)
    {
      if (!this.siBtnEditCustomPlanCode.Enabled)
        return;
      this.siBtnEditCustomPlanCode_Click((object) null, (EventArgs) null);
    }

    private void btnViewPlanCode_Click(object sender, EventArgs e)
    {
      Plan selectedPlan = this.ctlPlanCodes.SelectedPlan;
      if (selectedPlan == null)
        return;
      using (PlanCodeDetailsDialog codeDetailsDialog = new PlanCodeDetailsDialog(selectedPlan, this.session))
      {
        int num = (int) codeDetailsDialog.ShowDialog((IWin32Window) this);
      }
    }

    public List<string> SelectedCompanyCustomPlanCodes
    {
      get
      {
        Plan[] selectedPlans = this.ctlPlanCodes.GetSelectedPlans();
        if (selectedPlans == null)
          return (List<string>) null;
        List<string> companyCustomPlanCodes = new List<string>();
        foreach (Plan plan in selectedPlans)
        {
          if (plan.PlanType == PlanType.Alias || plan.PlanType == PlanType.Custom)
            companyCustomPlanCodes.Add(plan.InvestorPlanCode + "_" + ((int) plan.OrderType).ToString());
        }
        return companyCustomPlanCodes;
      }
    }

    public void SetSelectedCompanyPlanCodes(List<string> selectedPlanCode)
    {
      for (int index = 0; index < selectedPlanCode.Count; ++index)
        this.ctlPlanCodes.SelectComputedPlan(selectedPlanCode[index].ToString());
    }

    public void SetSelectPlanByPlanCode(List<string> selectedPlanCode)
    {
      for (int index = 0; index < selectedPlanCode.Count; ++index)
        this.ctlPlanCodes.SelectPlanByPlanCode(selectedPlanCode[index].Split('_')[0]);
    }

    public List<string> SelectedCompanyPlanCodes(DocumentOrderType sourceOrderType)
    {
      Plan[] selectedPlans = this.ctlPlanCodes.GetSelectedPlans();
      if (selectedPlans == null)
        return (List<string>) null;
      List<string> stringList = new List<string>();
      foreach (Plan plan in selectedPlans)
      {
        if (plan.PlanType == PlanType.Standard)
        {
          DocumentOrderType documentOrderType = (plan.OrderType & sourceOrderType) != sourceOrderType ? plan.OrderType : sourceOrderType;
          stringList.Add(string.Format("{0}_{1}_{2}", (object) (int) documentOrderType, string.IsNullOrEmpty(plan.PlanID) ? (object) "0" : (object) plan.PlanID, string.IsNullOrEmpty(plan.InvestorPlanCode) ? (object) "0" : (object) plan.InvestorPlanCode));
        }
      }
      return stringList;
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
      this.grpPlanCodes = new GroupContainer();
      this.ctlPlanCodes = new PlanCodeListControl();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnCreateLoanPrograms = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnRemovePlanCodes = new StandardIconButton();
      this.btnViewPlanCode = new StandardIconButton();
      this.siBtnEditCustomPlanCode = new StandardIconButton();
      this.btnAddPlanCodes = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.grpPlanCodes.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemovePlanCodes).BeginInit();
      ((ISupportInitialize) this.btnViewPlanCode).BeginInit();
      ((ISupportInitialize) this.siBtnEditCustomPlanCode).BeginInit();
      ((ISupportInitialize) this.btnAddPlanCodes).BeginInit();
      this.SuspendLayout();
      this.grpPlanCodes.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpPlanCodes.Controls.Add((Control) this.ctlPlanCodes);
      this.grpPlanCodes.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpPlanCodes.Dock = DockStyle.Fill;
      this.grpPlanCodes.HeaderForeColor = SystemColors.ControlText;
      this.grpPlanCodes.Location = new Point(0, 0);
      this.grpPlanCodes.Name = "grpPlanCodes";
      this.grpPlanCodes.Size = new Size(636, 504);
      this.grpPlanCodes.TabIndex = 0;
      this.grpPlanCodes.Text = "Company Plan Codes";
      this.ctlPlanCodes.AllowMultiselect = true;
      this.ctlPlanCodes.Dock = DockStyle.Fill;
      this.ctlPlanCodes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlPlanCodes.Location = new Point(1, 26);
      this.ctlPlanCodes.Name = "ctlPlanCodes";
      this.ctlPlanCodes.SelectedPlan = (Plan) null;
      this.ctlPlanCodes.ShowHiddenInvestorNames = true;
      this.ctlPlanCodes.ShowPlanCodeTypeColumn = true;
      this.ctlPlanCodes.Size = new Size(634, 478);
      this.ctlPlanCodes.TabIndex = 1;
      this.ctlPlanCodes.PlanCodeDoubleClick += new PlanCodeEventHandler(this.ctlPlanCodes_PlanCodeDoubleClick);
      this.ctlPlanCodes.FilterChanged += new EventHandler(this.ctlPlanCodes_FilterChanged);
      this.ctlPlanCodes.SelectedIndexChanged += new EventHandler(this.ctlPlanCodes_SelectedIndexChanged);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCreateLoanPrograms);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemovePlanCodes);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnViewPlanCode);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnEditCustomPlanCode);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddPlanCodes);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(318, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(318, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.btnCreateLoanPrograms.Enabled = false;
      this.btnCreateLoanPrograms.Location = new Point(185, 0);
      this.btnCreateLoanPrograms.Margin = new Padding(0);
      this.btnCreateLoanPrograms.Name = "btnCreateLoanPrograms";
      this.btnCreateLoanPrograms.Size = new Size(133, 22);
      this.btnCreateLoanPrograms.TabIndex = 2;
      this.btnCreateLoanPrograms.Text = "Create Loan Programs";
      this.btnCreateLoanPrograms.UseVisualStyleBackColor = true;
      this.btnCreateLoanPrograms.Click += new EventHandler(this.btnCreateLoanPrograms_Click);
      this.verticalSeparator1.Location = new Point(180, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 1;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnRemovePlanCodes.BackColor = Color.Transparent;
      this.btnRemovePlanCodes.Enabled = false;
      this.btnRemovePlanCodes.Location = new Point(159, 3);
      this.btnRemovePlanCodes.Margin = new Padding(3, 3, 2, 3);
      this.btnRemovePlanCodes.MouseDownImage = (Image) null;
      this.btnRemovePlanCodes.Name = "btnRemovePlanCodes";
      this.btnRemovePlanCodes.Size = new Size(16, 16);
      this.btnRemovePlanCodes.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePlanCodes.TabIndex = 2;
      this.btnRemovePlanCodes.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemovePlanCodes, "Remove Plan Codes");
      this.btnRemovePlanCodes.Click += new EventHandler(this.btnRemovePlanCodes_Click);
      this.btnViewPlanCode.BackColor = Color.Transparent;
      this.btnViewPlanCode.Location = new Point(138, 3);
      this.btnViewPlanCode.Margin = new Padding(3, 3, 2, 3);
      this.btnViewPlanCode.MouseDownImage = (Image) null;
      this.btnViewPlanCode.Name = "btnViewPlanCode";
      this.btnViewPlanCode.Size = new Size(16, 16);
      this.btnViewPlanCode.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewPlanCode.TabIndex = 6;
      this.btnViewPlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnViewPlanCode, "View Plan Code Details");
      this.btnViewPlanCode.Click += new EventHandler(this.btnViewPlanCode_Click);
      this.siBtnEditCustomPlanCode.BackColor = Color.Transparent;
      this.siBtnEditCustomPlanCode.Location = new Point(117, 3);
      this.siBtnEditCustomPlanCode.Margin = new Padding(3, 3, 2, 3);
      this.siBtnEditCustomPlanCode.MouseDownImage = (Image) null;
      this.siBtnEditCustomPlanCode.Name = "siBtnEditCustomPlanCode";
      this.siBtnEditCustomPlanCode.Size = new Size(16, 16);
      this.siBtnEditCustomPlanCode.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.siBtnEditCustomPlanCode.TabIndex = 5;
      this.siBtnEditCustomPlanCode.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnEditCustomPlanCode, "Edit Plan Code");
      this.siBtnEditCustomPlanCode.Click += new EventHandler(this.siBtnEditCustomPlanCode_Click);
      this.btnAddPlanCodes.BackColor = Color.Transparent;
      this.btnAddPlanCodes.Location = new Point(96, 3);
      this.btnAddPlanCodes.Margin = new Padding(3, 3, 2, 3);
      this.btnAddPlanCodes.MouseDownImage = (Image) null;
      this.btnAddPlanCodes.Name = "btnAddPlanCodes";
      this.btnAddPlanCodes.Size = new Size(16, 16);
      this.btnAddPlanCodes.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPlanCodes.TabIndex = 4;
      this.btnAddPlanCodes.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddPlanCodes, "Add Plan Codes");
      this.btnAddPlanCodes.Click += new EventHandler(this.btnAddPlanCodes_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpPlanCodes);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PlanCodeManagementControl);
      this.Size = new Size(636, 504);
      this.grpPlanCodes.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemovePlanCodes).EndInit();
      ((ISupportInitialize) this.btnViewPlanCode).EndInit();
      ((ISupportInitialize) this.siBtnEditCustomPlanCode).EndInit();
      ((ISupportInitialize) this.btnAddPlanCodes).EndInit();
      this.ResumeLayout(false);
    }
  }
}
