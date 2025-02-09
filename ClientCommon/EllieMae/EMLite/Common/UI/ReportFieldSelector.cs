// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ReportFieldSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ReportFieldSelector : Form
  {
    private ReportFieldDef selectedField;
    private string lastSearch = "";
    private ReportFieldDefs fieldDefs;
    private Sessions.Session session;
    private IContainer components;
    private GridView gvFields;
    private Button btnFind;
    private TextBox txtFind;
    private DialogButtons dlgButtons;
    private ToolTip toolTip1;

    public ReportFieldSelector(ReportFieldDefs fieldDefs, Sessions.Session session)
      : this(fieldDefs, false, session)
    {
    }

    public ReportFieldSelector(ReportFieldDefs fieldDefs, EllieMae.EMLite.ClientServer.Reporting.FieldTypes[] allowedTypes)
      : this(fieldDefs, allowedTypes, false, true, Session.DefaultInstance)
    {
    }

    public ReportFieldSelector(ReportFieldDefs fieldDefs, bool dbFieldsOnly)
      : this(fieldDefs, dbFieldsOnly, true, Session.DefaultInstance)
    {
    }

    public ReportFieldSelector(
      ReportFieldDefs fieldDefs,
      bool dbFieldsOnly,
      Sessions.Session session)
      : this(fieldDefs, dbFieldsOnly, true, session)
    {
    }

    public ReportFieldSelector(
      ReportFieldDefs fieldDefs,
      bool dbFieldsOnly,
      bool allowVirtualFields)
      : this(fieldDefs, (EllieMae.EMLite.ClientServer.Reporting.FieldTypes[]) null, dbFieldsOnly, allowVirtualFields, Session.DefaultInstance)
    {
    }

    public ReportFieldSelector(
      ReportFieldDefs fieldDefs,
      bool dbFieldsOnly,
      bool allowVirtualFields,
      Sessions.Session session)
      : this(fieldDefs, (EllieMae.EMLite.ClientServer.Reporting.FieldTypes[]) null, dbFieldsOnly, allowVirtualFields, session)
    {
    }

    public ReportFieldSelector(
      ReportFieldDefs fieldDefs,
      EllieMae.EMLite.ClientServer.Reporting.FieldTypes[] allowedTypes,
      bool dbFieldsOnly,
      bool allowVirtualFields)
      : this(fieldDefs, allowedTypes, dbFieldsOnly, allowVirtualFields, Session.DefaultInstance)
    {
    }

    public ReportFieldSelector(
      ReportFieldDefs fieldDefs,
      EllieMae.EMLite.ClientServer.Reporting.FieldTypes[] allowedTypes,
      bool dbFieldsOnly,
      bool allowVirtualFields,
      Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.fieldDefs = fieldDefs;
      this.dlgButtons.OKButton.Enabled = false;
      if (!(fieldDefs is LoanReportFieldDefs))
        this.gvFields.Columns.Remove(this.gvFields.Columns.GetColumnByName("colPair"));
      this.loadFieldList(fieldDefs, allowedTypes, dbFieldsOnly, allowVirtualFields);
      this.gvFields.Sort(new GVColumnSort[2]
      {
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(1, SortOrder.Ascending)
      });
    }

    public ReportFieldDef SelectedField => this.selectedField;

    private void loadFieldList(
      ReportFieldDefs fieldDefs,
      EllieMae.EMLite.ClientServer.Reporting.FieldTypes[] allowedTypes,
      bool dbFieldsOnly,
      bool allowVirtualFields)
    {
      this.gvFields.Items.Clear();
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
      {
        if (!(!fieldDef.IsDatabaseField & dbFieldsOnly) && (allowVirtualFields || !(fieldDef.FieldDefinition is VirtualField)) && (allowedTypes == null || Array.IndexOf<EllieMae.EMLite.ClientServer.Reporting.FieldTypes>(allowedTypes, fieldDef.FieldType) >= 0) && fieldDef.Selectable)
        {
          GVItem gvItem = new GVItem(fieldDef.Category);
          gvItem.SubItems[1].Text = fieldDef.Description;
          gvItem.SubItems[2].Value = (object) new FieldIDElement(fieldDef.FieldID);
          if (fieldDef is LoanReportFieldDef)
          {
            int borrowerPair = ((LoanReportFieldDef) fieldDef).BorrowerPair;
            if (borrowerPair > 0)
              gvItem.SubItems[3].Value = (object) FieldPairInfo.GetPairDescription(borrowerPair);
            else if (borrowerPair == -1 && (fieldDef.FieldDefinition.Category == FieldCategory.Borrower || fieldDef.FieldDefinition.Category == FieldCategory.Coborrower))
            {
              gvItem.SubItems[3].Value = (object) "1st";
              gvItem.SubItems[1].Text += " - 1st";
            }
          }
          gvItem.Tag = (object) fieldDef;
          this.gvFields.Items.Add(gvItem);
        }
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Select the field to be included in the filter.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.selectedField = this.selectField(this.gvFields.SelectedItems[0].Tag as ReportFieldDef);
        if (this.selectedField == null)
          return;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void gvFields_DoubleClick(object sender, EventArgs e)
    {
      GVHitTestInfo gvHitTestInfo = this.gvFields.HitTest(this.gvFields.PointToClient(Cursor.Position));
      if (gvHitTestInfo.RowIndex < 0)
        return;
      this.selectedField = this.selectField(this.gvFields.Items[gvHitTestInfo.RowIndex].Tag as ReportFieldDef);
      if (this.selectedField == null)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private ReportFieldDef selectField(ReportFieldDef fieldDef)
    {
      if (fieldDef.FieldID == LoanReportFieldDef.RateLockFieldSelector().FieldID)
      {
        using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.RateLockFieldSelector().FieldID))
          return virtualFieldSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK ? (ReportFieldDef) null : (ReportFieldDef) virtualFieldSelectForm.SelectedField;
      }
      else if (fieldDef.FieldID == LoanReportFieldDef.InterimFieldSelector().FieldID)
      {
        using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.InterimFieldSelector().FieldID))
          return virtualFieldSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK ? (ReportFieldDef) null : (ReportFieldDef) virtualFieldSelectForm.SelectedField;
      }
      else
      {
        if (fieldDef.FieldID == LoanReportFieldDef.GFEDisclosedFieldSelector().FieldID)
        {
          using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.GFEDisclosedFieldSelector().FieldID))
          {
            if (virtualFieldSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return (ReportFieldDef) null;
            fieldDef = (ReportFieldDef) virtualFieldSelectForm.SelectedField;
          }
        }
        else if (fieldDef.FieldID == LoanReportFieldDef.AuditTrailFieldSelector().FieldID)
        {
          using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.AuditTrailFieldSelector().FieldID))
          {
            if (virtualFieldSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return (ReportFieldDef) null;
            fieldDef = (ReportFieldDef) virtualFieldSelectForm.SelectedField;
          }
        }
        if (fieldDef.FieldDefinition.InstanceSpecifierType == FieldInstanceSpecifierType.None || fieldDef.FieldID.IndexOf("00") == -1)
          return fieldDef;
        using (InstanceSelectorDialog instanceSelectorDialog = new InstanceSelectorDialog(fieldDef.FieldDefinition.InstanceSpecifierType))
        {
          if (instanceSelectorDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
            return this.fieldDefs.CreateReportFieldDef(this.session, fieldDef.FieldDefinition.CreateInstance((object) instanceSelectorDialog.SelectedInstance));
        }
        return (ReportFieldDef) null;
      }
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      string searchText = this.txtFind.Text.Trim();
      if (searchText == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Enter the text to find in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.searchForText(searchText);
    }

    private void searchForText(string searchText)
    {
      int num1 = 0;
      if (this.gvFields.SelectedItems.Count > 0 && searchText == this.lastSearch)
        num1 = this.gvFields.SelectedItems[0].Index + 1;
      this.lastSearch = searchText;
      if (num1 == 0)
      {
        for (int index = 0; index < this.gvFields.Items.Count; ++index)
        {
          if (string.Compare(this.gvFields.Items[index].SubItems[2].Text, searchText, true) == 0)
          {
            this.gvFields.Items[index].Selected = true;
            this.gvFields.EnsureVisible(index);
            this.gvFields.Focus();
            return;
          }
        }
      }
      if (this.gvFields.Items.Count > 0)
      {
        for (int index = 0; index <= this.gvFields.Items.Count; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.gvFields.Columns.Count; ++nItemIndex)
          {
            int num2 = (num1 + index) % this.gvFields.Items.Count;
            if (this.gvFields.Items[num2].SubItems[nItemIndex].Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
              this.gvFields.Items[num2].Selected = true;
              this.gvFields.EnsureVisible(num2);
              this.gvFields.Focus();
              return;
            }
          }
        }
      }
      int num3 = (int) Utils.Dialog((IWin32Window) this, "No match was found for '" + searchText + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void txtFind_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFind;
    }

    private void gvFields_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.dlgButtons;
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count > 0)
        this.dlgButtons.OKButton.Enabled = true;
      else
        this.dlgButtons.OKButton.Enabled = false;
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
      GVColumn gvColumn4 = new GVColumn();
      this.gvFields = new GridView();
      this.btnFind = new Button();
      this.txtFind = new TextBox();
      this.dlgButtons = new DialogButtons();
      this.toolTip1 = new ToolTip(this.components);
      this.SuspendLayout();
      this.gvFields.AllowMultiselect = false;
      this.gvFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Category";
      gvColumn1.Width = 116;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 185;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Custom;
      gvColumn3.Text = "Field ID";
      gvColumn3.Width = 94;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colPair";
      gvColumn4.Text = "Borrower Pair";
      gvColumn4.Width = 90;
      this.gvFields.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvFields.HoverToolTip = this.toolTip1;
      this.gvFields.Location = new Point(10, 34);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(487, 296);
      this.gvFields.TabIndex = 2;
      this.gvFields.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.gvFields.DoubleClick += new EventHandler(this.gvFields_DoubleClick);
      this.gvFields.Enter += new EventHandler(this.gvFields_Enter);
      this.btnFind.Location = new Point(10, 7);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(62, 22);
      this.btnFind.TabIndex = 5;
      this.btnFind.Text = "&Find";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.txtFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFind.Location = new Point(76, 8);
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(421, 20);
      this.txtFind.TabIndex = 1;
      this.txtFind.Enter += new EventHandler(this.txtFind_Enter);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 335);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(508, 39);
      this.dlgButtons.TabIndex = 6;
      this.dlgButtons.OK += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(508, 374);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.txtFind);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.gvFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ReportFieldSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Field";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
