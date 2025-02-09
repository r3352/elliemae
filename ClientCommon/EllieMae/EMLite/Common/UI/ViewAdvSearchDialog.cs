// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ViewAdvSearchDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ViewAdvSearchDialog : Form
  {
    private IContainer components;
    private DialogButtons dialogButtons1;
    private AdvancedSearchControl advSearch;

    public ViewAdvSearchDialog(ReportFieldDefs fieldDefs, string title)
    {
      this.InitializeComponent();
      this.Text = title;
      this.advSearch.FieldDefs = fieldDefs;
      this.advSearch.AllowDynamicOperators = true;
      this.advSearch.AllowDatabaseFieldsOnly = true;
    }

    public ViewAdvSearchDialog(
      ReportFieldDefs fieldDefs,
      FieldFilterList currentFilter,
      string title)
      : this(fieldDefs, title)
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
      this.dialogButtons1.Location = new Point(0, 315);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Apply";
      this.dialogButtons1.Size = new Size(685, 44);
      this.dialogButtons1.TabIndex = 2;
      this.advSearch.AllowDatabaseFieldsOnly = false;
      this.advSearch.Dock = DockStyle.Fill;
      this.advSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.advSearch.Location = new Point(0, 0);
      this.advSearch.Name = "advSearch";
      this.advSearch.Padding = new Padding(8, 8, 8, 0);
      this.advSearch.Size = new Size(685, 315);
      this.advSearch.TabIndex = 3;
      this.advSearch.Title = "Filters";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(685, 359);
      this.Controls.Add((Control) this.advSearch);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewAdvSearchDialog);
      this.Text = "View Advanced Search Dialog";
      this.ResumeLayout(false);
    }
  }
}
