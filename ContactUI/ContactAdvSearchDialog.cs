// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAdvSearchDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAdvSearchDialog : Form
  {
    private IContainer components;
    private DialogButtons dialogButtons1;
    private ContactAdvancedSearchControl advSearch;

    public ContactAdvSearchDialog(ReportFieldDefs fieldDefs)
    {
      this.InitializeComponent();
      this.advSearch.FieldDefs = fieldDefs;
      this.advSearch.AllowDynamicOperators = true;
      this.advSearch.AllowDatabaseFieldsOnly = true;
    }

    public ContactAdvSearchDialog(ReportFieldDefs fieldDefs, FieldFilterList currentFilter)
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
      this.advSearch = new ContactAdvancedSearchControl();
      this.SuspendLayout();
      this.dialogButtons1.DialogResult = DialogResult.OK;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 315);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Apply";
      this.dialogButtons1.Size = new Size(684, 44);
      this.dialogButtons1.TabIndex = 2;
      this.advSearch.AllowDatabaseFieldsOnly = false;
      this.advSearch.Dock = DockStyle.Fill;
      this.advSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.advSearch.Location = new Point(0, 0);
      this.advSearch.Name = "advSearch";
      this.advSearch.Padding = new Padding(8, 8, 8, 0);
      this.advSearch.Size = new Size(684, 315);
      this.advSearch.TabIndex = 3;
      this.advSearch.Title = "Filters";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(684, 359);
      this.Controls.Add((Control) this.advSearch);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactAdvSearchDialog);
      this.Text = "Contact Advanced Search Dialog";
      this.ResumeLayout(false);
    }
  }
}
