// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.PipelineAdvSearchDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class PipelineAdvSearchDialog : Form
  {
    private IContainer components;
    private AdvancedSearchControl advSearch;
    private DialogButtons dialogButtons1;

    public PipelineAdvSearchDialog(LoanReportFieldDefs fieldDefs)
    {
      this.InitializeComponent();
      this.advSearch.FieldDefs = (ReportFieldDefs) fieldDefs;
      this.advSearch.AllowDynamicOperators = true;
      this.advSearch.AllowDatabaseFieldsOnly = true;
    }

    public PipelineAdvSearchDialog(LoanReportFieldDefs fieldDefs, FieldFilterList currentFilter)
      : this(fieldDefs)
    {
      if (currentFilter == null)
        return;
      this.advSearch.AddFilters(currentFilter);
    }

    public FieldFilterList GetSelectedFilter() => this.advSearch.GetCurrentFilter();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.advSearch = new AdvancedSearchControl();
      this.SuspendLayout();
      this.dialogButtons1.DialogResult = DialogResult.OK;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 324);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Apply";
      this.dialogButtons1.Size = new Size(687, 44);
      this.dialogButtons1.TabIndex = 1;
      this.advSearch.AllowDatabaseFieldsOnly = false;
      this.advSearch.Dock = DockStyle.Fill;
      this.advSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.advSearch.Location = new Point(0, 0);
      this.advSearch.Name = "advSearch";
      this.advSearch.Padding = new Padding(8, 8, 8, 0);
      this.advSearch.Size = new Size(687, 324);
      this.advSearch.TabIndex = 0;
      this.advSearch.Title = "Filters";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(687, 368);
      this.Controls.Add((Control) this.advSearch);
      this.Controls.Add((Control) this.dialogButtons1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PipelineAdvSearchDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Advanced Search";
      this.ResumeLayout(false);
    }
  }
}
