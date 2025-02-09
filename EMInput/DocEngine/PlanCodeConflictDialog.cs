// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.PlanCodeConflictDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class PlanCodeConflictDialog : Form
  {
    private static PlanCodeConflictDialog currentInstance;
    private PlanCodeComparisonMode compareMode = PlanCodeComparisonMode.Conflict;
    private IHtmlInput inputData;
    private Plan planToCompare;
    private DocumentOrderType orderType;
    private IContainer components;
    private Label lblHeader;
    private FormBrowser fbBrowser;
    private GradientPanel gradientPanel1;
    private BorderPanel pnlForm;
    private GradientPanel gradientPanel2;
    private GradientPanel gradientPanel3;
    private Label lblPlanCode;
    private Label label3;
    private Button btnImport;
    private Button btnClose;
    private Button btnSkipImport;
    private Button btnImportAndClose;

    public PlanCodeConflictDialog(
      IHtmlInput input,
      Plan planToCompare,
      DocumentOrderType orderType,
      PlanCodeComparisonMode compareMode)
    {
      this.InitializeComponent();
      this.inputData = input;
      this.planToCompare = planToCompare;
      this.compareMode = compareMode;
      this.orderType = orderType;
      this.fbBrowser.DataSource = input;
      this.lblPlanCode.Text = "Plan Code " + planToCompare.GetField("1881");
      this.btnImport.Visible = compareMode == PlanCodeComparisonMode.Conflict;
      this.btnImportAndClose.Visible = compareMode == PlanCodeComparisonMode.Compare;
      this.btnSkipImport.Visible = compareMode == PlanCodeComparisonMode.Compare;
      this.btnClose.Text = compareMode == PlanCodeComparisonMode.Compare ? "&Cancel" : "&Close";
      if (compareMode == PlanCodeComparisonMode.Compare)
        this.lblHeader.Text = this.lblHeader.Text.Replace("Import All Plan Data", "Import Plan Data");
      InputFormInfo formInfo = new InputFormInfo("PlanCodeConflicts", "Plan Code Conflicts", InputFormType.Standard);
      this.fbBrowser.FormLoaded += new EventHandler(this.onFormLoaded);
      this.fbBrowser.OpenForm(formInfo);
      PlanCodeConflictDialog.currentInstance = this;
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
    }

    public PlanCodeConflictDialog(IHtmlInput input, DocumentOrderType orderType)
      : this(input, PlanCodeConflictDialog.getPlan(input, orderType), orderType, PlanCodeComparisonMode.Conflict)
    {
    }

    public static PlanCodeConflictDialog CurrentInstance => PlanCodeConflictDialog.currentInstance;

    private void onFormLoaded(object sender, EventArgs e)
    {
      PLANCODECONFLICTSInputHandler inputHandler = (PLANCODECONFLICTSInputHandler) this.fbBrowser.GetInputHandler();
      inputHandler.Plan = this.planToCompare;
      inputHandler.AllowEdit = false;
      inputHandler.RefreshContents();
    }

    private void onFormClosed(object sender, FormClosedEventArgs e)
    {
      if (PlanCodeConflictDialog.currentInstance != this)
        return;
      PlanCodeConflictDialog.currentInstance = (PlanCodeConflictDialog) null;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to overwrite the loan data with the plan code settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      using (CursorActivator.Wait())
      {
        this.planToCompare.Apply(this.inputData, this.orderType);
        this.fbBrowser.GetInputHandler().RefreshContents();
      }
      this.DialogResult = DialogResult.OK;
    }

    private static Plan getPlan(IHtmlInput inputData, DocumentOrderType orderType)
    {
      PlanCodeInfo planInfo = PlanCodeInfo.FromDataObject(inputData, orderType);
      return (planInfo != null ? Plans.GetPlan(Session.SessionObjects, planInfo) : throw new ArgumentException("The data object does not have a Plan Code specified")) ?? throw new InvalidPlanCodeException(planInfo.PlanCode);
    }

    private void btnSkipImport_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to apply the Plan Code without importing the plan code data into the loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.planToCompare.ApplyPlanMetadata((IHtmlInput) Session.LoanData, this.orderType);
      this.DialogResult = DialogResult.OK;
    }

    private void btnImportAndClose_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to overwrite the loan data with the plan code settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      using (CursorActivator.Wait())
      {
        this.planToCompare.Apply(this.inputData, this.orderType);
        this.DialogResult = DialogResult.OK;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlanCodeConflictDialog));
      this.lblHeader = new Label();
      this.btnClose = new Button();
      this.btnSkipImport = new Button();
      this.btnImportAndClose = new Button();
      this.gradientPanel3 = new GradientPanel();
      this.btnImport = new Button();
      this.label3 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.lblPlanCode = new Label();
      this.pnlForm = new BorderPanel();
      this.fbBrowser = new FormBrowser();
      this.gradientPanel1 = new GradientPanel();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.pnlForm.SuspendLayout();
      this.SuspendLayout();
      this.lblHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHeader.Location = new Point(7, 7);
      this.lblHeader.Margin = new Padding(2, 0, 2, 0);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(692, 35);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = componentResourceManager.GetString("lblHeader.Text");
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(625, 485);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 7;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnSkipImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSkipImport.Location = new Point(547, 485);
      this.btnSkipImport.Name = "btnSkipImport";
      this.btnSkipImport.Size = new Size(75, 22);
      this.btnSkipImport.TabIndex = 8;
      this.btnSkipImport.Text = "&Skip Import";
      this.btnSkipImport.UseVisualStyleBackColor = true;
      this.btnSkipImport.Click += new EventHandler(this.btnSkipImport_Click);
      this.btnImportAndClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnImportAndClose.Location = new Point(443, 485);
      this.btnImportAndClose.Name = "btnImportAndClose";
      this.btnImportAndClose.Size = new Size(101, 22);
      this.btnImportAndClose.TabIndex = 9;
      this.btnImportAndClose.Text = "&Import Plan Data";
      this.btnImportAndClose.UseVisualStyleBackColor = true;
      this.btnImportAndClose.Click += new EventHandler(this.btnImportAndClose_Click);
      this.gradientPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.btnImport);
      this.gradientPanel3.Controls.Add((Control) this.label3);
      this.gradientPanel3.Location = new Point(449, 45);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(249, 25);
      this.gradientPanel3.TabIndex = 6;
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.Location = new Point(131, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(113, 22);
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = "&Import All Plan Data";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(6, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(106, 14);
      this.label3.TabIndex = 1;
      this.label3.Text = "Existing Loan Data";
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblPlanCode);
      this.gradientPanel2.Location = new Point(240, 45);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(209, 25);
      this.gradientPanel2.TabIndex = 5;
      this.lblPlanCode.AutoSize = true;
      this.lblPlanCode.BackColor = Color.Transparent;
      this.lblPlanCode.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPlanCode.Location = new Point(7, 6);
      this.lblPlanCode.Name = "lblPlanCode";
      this.lblPlanCode.Size = new Size(83, 14);
      this.lblPlanCode.TabIndex = 0;
      this.lblPlanCode.Text = "Plan Code <1>";
      this.pnlForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlForm.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlForm.Controls.Add((Control) this.fbBrowser);
      this.pnlForm.Location = new Point(10, 70);
      this.pnlForm.Name = "pnlForm";
      this.pnlForm.Size = new Size(688, 407);
      this.pnlForm.TabIndex = 4;
      this.fbBrowser.DataSource = (IHtmlInput) null;
      this.fbBrowser.Dock = DockStyle.Fill;
      this.fbBrowser.Location = new Point(1, 0);
      this.fbBrowser.Name = "fbBrowser";
      this.fbBrowser.Size = new Size(686, 406);
      this.fbBrowser.TabIndex = 1;
      this.gradientPanel1.Location = new Point(10, 45);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(230, 25);
      this.gradientPanel1.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(709, 516);
      this.Controls.Add((Control) this.btnImportAndClose);
      this.Controls.Add((Control) this.btnSkipImport);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.gradientPanel3);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.pnlForm);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.lblHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Margin = new Padding(2, 3, 2, 3);
      this.MinimizeBox = false;
      this.Name = nameof (PlanCodeConflictDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Plan Code Conflict";
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.pnlForm.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
