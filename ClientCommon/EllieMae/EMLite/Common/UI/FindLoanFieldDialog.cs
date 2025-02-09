// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FindLoanFieldDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FindLoanFieldDialog : Form
  {
    private ListViewSortManager sortManager;
    private LoanReportFieldDefs fieldDefs;
    private LoanReportFieldDef selectedField;
    private ReportingDatabaseColumnType columnTypeFilter;
    private string lastSearch = string.Empty;
    private IContainer components;
    private Button btnFind;
    private TextBox txtFind;
    private ListView lvwFields;
    private Button btnCancel;
    private Button btnSelect;
    private ColumnHeader chdCategory;
    private ColumnHeader chdDescription;
    private ColumnHeader chdFieldId;

    public ReportingDatabaseColumnType ColumnTypeFilter
    {
      get => this.columnTypeFilter;
      set
      {
        if (this.columnTypeFilter == value)
          return;
        this.columnTypeFilter = value;
        this.loadFieldList();
      }
    }

    public FindLoanFieldDialog(LoanReportFieldDefs fieldDefs)
      : this(fieldDefs, ReportingDatabaseColumnType.Unknown)
    {
    }

    public FindLoanFieldDialog(
      LoanReportFieldDefs fieldDefs,
      ReportingDatabaseColumnType columnTypeFilter)
    {
      this.fieldDefs = fieldDefs;
      this.columnTypeFilter = columnTypeFilter;
      this.InitializeComponent();
      this.loadFieldList();
      this.sortManager = new ListViewSortManager(this.lvwFields, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewFieldIDSort)
      }, true);
      this.sortManager.Sort(0);
    }

    public LoanReportFieldDef GetSelectedField() => this.selectedField;

    private void loadFieldList()
    {
      this.lvwFields.Items.Clear();
      if (this.fieldDefs == null || this.fieldDefs.Count == 0)
        return;
      this.lvwFields.SuspendLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (string.Compare("Dashboard", fieldDef.Category, true) != 0 && (this.columnTypeFilter == ReportingDatabaseColumnType.Unknown || fieldDef.ReportingDatabaseColumnType == this.columnTypeFilter))
          this.lvwFields.Items.Add(new ListViewItem(fieldDef.Category)
          {
            SubItems = {
              fieldDef.Description,
              fieldDef.FieldID
            },
            Tag = (object) fieldDef
          });
      }
      this.lvwFields.ResumeLayout();
      if (0 >= this.lvwFields.Items.Count)
        return;
      this.btnFind.Enabled = true;
    }

    private void searchForText(string searchText)
    {
      int num1 = 0;
      if (this.lvwFields.SelectedItems.Count > 0 && searchText == this.lastSearch)
        num1 = this.lvwFields.SelectedIndices[0] + 1;
      this.lastSearch = searchText;
      if (num1 == 0)
      {
        for (int index = 0; index < this.lvwFields.Items.Count; ++index)
        {
          if (string.Compare(this.lvwFields.Items[index].SubItems[this.chdFieldId.Index].Text, searchText, true) == 0)
          {
            this.lvwFields.Items[index].Selected = true;
            this.lvwFields.Items[index].EnsureVisible();
            this.lvwFields.Focus();
            return;
          }
        }
      }
      for (int index1 = 0; index1 <= this.lvwFields.Items.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.lvwFields.Columns.Count; ++index2)
        {
          int index3 = (num1 + index1) % this.lvwFields.Items.Count;
          if (this.lvwFields.Items[index3].SubItems[index2].Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
          {
            this.lvwFields.Items[index3].Selected = true;
            this.lvwFields.Items[index3].EnsureVisible();
            this.lvwFields.Focus();
            return;
          }
        }
      }
      int num2 = (int) Utils.Dialog((IWin32Window) this, "No match was found for '" + searchText + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      string searchText = this.txtFind.Text.Trim();
      if (string.Empty == searchText)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the search text in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.searchForText(searchText);
    }

    private void txtFind_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFind;
    }

    private void lvwFields_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnSelect;
    }

    private void lvwFields_DoubleClick(object sender, EventArgs e)
    {
      ListViewHitTestInfo listViewHitTestInfo = this.lvwFields.HitTest(this.lvwFields.PointToClient(Cursor.Position));
      if (listViewHitTestInfo == null || listViewHitTestInfo.Item == null || !listViewHitTestInfo.Item.Selected)
        return;
      this.selectedField = listViewHitTestInfo.Item.Tag as LoanReportFieldDef;
      this.DialogResult = DialogResult.OK;
    }

    private void lvwFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = 0 < this.lvwFields.SelectedItems.Count;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.lvwFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan field from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.selectedField = this.lvwFields.SelectedItems[0].Tag as LoanReportFieldDef;
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
      this.btnFind = new Button();
      this.txtFind = new TextBox();
      this.lvwFields = new ListView();
      this.chdCategory = new ColumnHeader();
      this.chdDescription = new ColumnHeader();
      this.chdFieldId = new ColumnHeader();
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.SuspendLayout();
      this.btnFind.Enabled = false;
      this.btnFind.Location = new Point(12, 12);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(75, 24);
      this.btnFind.TabIndex = 3;
      this.btnFind.Text = "&Find";
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.txtFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFind.Location = new Point(93, 14);
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(439, 20);
      this.txtFind.TabIndex = 2;
      this.txtFind.Enter += new EventHandler(this.txtFind_Enter);
      this.lvwFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwFields.Columns.AddRange(new ColumnHeader[3]
      {
        this.chdCategory,
        this.chdDescription,
        this.chdFieldId
      });
      this.lvwFields.FullRowSelect = true;
      this.lvwFields.GridLines = true;
      this.lvwFields.HideSelection = false;
      this.lvwFields.Location = new Point(12, 42);
      this.lvwFields.MultiSelect = false;
      this.lvwFields.Name = "lvwFields";
      this.lvwFields.Size = new Size(520, 306);
      this.lvwFields.TabIndex = 4;
      this.lvwFields.UseCompatibleStateImageBehavior = false;
      this.lvwFields.View = View.Details;
      this.lvwFields.SelectedIndexChanged += new EventHandler(this.lvwFields_SelectedIndexChanged);
      this.lvwFields.DoubleClick += new EventHandler(this.lvwFields_DoubleClick);
      this.lvwFields.Enter += new EventHandler(this.lvwFields_Enter);
      this.chdCategory.Text = "Category";
      this.chdCategory.Width = 165;
      this.chdDescription.Text = "Description";
      this.chdDescription.Width = 165;
      this.chdFieldId.Text = "Field ID";
      this.chdFieldId.Width = 165;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(457, 354);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(376, 354);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 24);
      this.btnSelect.TabIndex = 5;
      this.btnSelect.Text = "&Select";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(544, 389);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.lvwFields);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.txtFind);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FindLoanFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Loan Field";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
