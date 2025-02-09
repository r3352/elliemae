// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QueryBuilderForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QueryBuilderForm : Form
  {
    private bool viewOnly;
    private AdvancedSearchControl filterBuilderControl;
    private Sessions.Session session;
    private FieldFilter[] newFilters;
    private IContainer components;
    private Panel panelFilter;
    private Button btnOK;
    private Button btnCancel;

    public QueryBuilderForm(Sessions.Session session, FieldFilter[] newFilters)
      : this(session, newFilters, false)
    {
    }

    public QueryBuilderForm(Sessions.Session session, FieldFilter[] newFilters, bool viewOnly)
    {
      this.session = session;
      this.viewOnly = viewOnly;
      this.newFilters = newFilters;
      this.InitializeComponent();
      FieldFilterList filters = (FieldFilterList) null;
      if (newFilters != null)
      {
        filters = new FieldFilterList();
        for (int index = 0; index < newFilters.Length; ++index)
          filters.Add(newFilters[index]);
      }
      this.filterBuilderControl = new AdvancedSearchControl();
      this.filterBuilderControl.AllowDatabaseFieldsOnly = false;
      this.filterBuilderControl.Dock = DockStyle.Fill;
      this.filterBuilderControl.SetCurrentFilter(filters);
      this.filterBuilderControl.ReadOnly = viewOnly;
      this.filterBuilderControl.Load += new EventHandler(this.filterBuilderControl_Load);
      this.panelFilter.Controls.Add((Control) this.filterBuilderControl);
      if (!this.viewOnly)
        return;
      this.btnCancel.Text = "&Close";
      this.btnOK.Visible = false;
    }

    private void filterBuilderControl_Load(object sender, EventArgs e)
    {
      LoanReportFieldDefs loanReportFieldDefs = new LoanReportFieldDefs(this.session);
      LoanReportFieldDefs fieldDefs = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.BasicLoanDataFields);
      for (int index = 0; index < fieldDefs.Count; ++index)
      {
        if (fieldDefs[index].Category != "Document Tracking" && fieldDefs[index].Category != "Condition Tracking" && fieldDefs[index].Category != "Post-Closing Condition Tracking" && fieldDefs[index].Category != "Compliance Assessment" && fieldDefs[index].Category != "Milestone" && fieldDefs[index].Category != "Rate Lock" && fieldDefs[index].FieldID.ToLower() != "comortgagorcount" && fieldDefs[index].FieldID.ToLower() != "log.ms.lastcompleted" && fieldDefs[index].FieldID.ToLower() != "currentteammember" && fieldDefs[index].FieldID.ToLower() != "ms.loanduration" && fieldDefs[index].FieldID.ToLower() != "previousteammember" && fieldDefs[index].FieldID.ToLower() != "coremilestone" && !fieldDefs[index].FieldID.ToLower().StartsWith("loanteammember."))
          loanReportFieldDefs.Add((ReportFieldDef) fieldDefs[index]);
      }
      this.filterBuilderControl.FieldDefs = (ReportFieldDefs) loanReportFieldDefs;
    }

    public FieldFilter[] NewFilters => this.newFilters;

    public string ScenarioKey
    {
      get
      {
        return this.filterBuilderControl != null ? this.filterBuilderControl.GetFilterSummary() : string.Empty;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      FieldFilterList currentFilter = this.filterBuilderControl.GetCurrentFilter();
      if (currentFilter == null || currentFilter.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter at least one scenario.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.newFilters = new FieldFilter[currentFilter.Count];
        for (int index = 0; index < currentFilter.Count; ++index)
          this.newFilters[index] = currentFilter[index];
        this.DialogResult = DialogResult.OK;
      }
    }

    private void QueryBuilderForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelFilter = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.panelFilter.Location = new Point(12, 12);
      this.panelFilter.Name = "panelFilter";
      this.panelFilter.Size = new Size(674, 476);
      this.panelFilter.TabIndex = 0;
      this.btnOK.Location = new Point(530, 491);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(611, 491);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(698, 526);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panelFilter);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (QueryBuilderForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Scenario Builder";
      this.KeyDown += new KeyEventHandler(this.QueryBuilderForm_KeyDown);
      this.ResumeLayout(false);
    }
  }
}
