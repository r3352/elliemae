// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AddEditLoanFieldDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AddEditLoanFieldDialog : Form
  {
    private bool displaySummaryFields;
    private bool allowDatabaseFieldsOnly;
    private LoanReportFieldDefs fieldDefs;
    private LoanReportFieldDef currentField;
    private ReportingDatabaseColumnType columnTypeFilter;
    private List<ColumnInfo> columnInfoList;
    private FindLoanFieldDialog dlgFindField;
    private bool nonNumberEntered;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox cboSortOrder;
    private Label lblSortOrder;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblFieldId;
    private TextBox txtFieldId;
    private PictureBox picFindField;
    private ImageList imageList;
    private Panel pnlSummaryFields;
    private ComboBox cboSummaryType;
    private TextBox txtDecimalPlaces;
    private Label lblDecimalPlaces;
    private Label lblSummaryType;
    private ToolTip toolTip1;

    public bool AllowDatabaseFieldsOnly
    {
      get => this.allowDatabaseFieldsOnly;
      set => this.allowDatabaseFieldsOnly = value;
    }

    public ReportingDatabaseColumnType ColumnTypeFilter
    {
      get => this.columnTypeFilter;
      set => this.columnTypeFilter = value;
    }

    public List<ColumnInfo> ColumnInfoList
    {
      set => this.columnInfoList = value != null ? value : new List<ColumnInfo>();
    }

    public AddEditLoanFieldDialog(LoanReportFieldDefs fieldDefs)
      : this(fieldDefs, (List<ColumnInfo>) null)
    {
    }

    public AddEditLoanFieldDialog(LoanReportFieldDefs fieldDefs, List<ColumnInfo> columnInfoList)
    {
      this.fieldDefs = fieldDefs;
      this.ColumnInfoList = columnInfoList;
      this.InitializeComponent();
      this.initializeDialog();
      if (this.displaySummaryFields)
        return;
      this.pnlSummaryFields.Visible = false;
      this.Height -= this.pnlSummaryFields.Height;
    }

    public ColumnInfo GetColumnInfo()
    {
      return new ColumnInfo(this.txtFieldId.Text.Trim(), this.txtDescription.Text.Trim(), (ColumnSortOrder) Enum.ToObject(typeof (ColumnSortOrder), this.cboSortOrder.SelectedValue), (ColumnSummaryType) Enum.ToObject(typeof (ColumnSummaryType), this.cboSummaryType.SelectedValue), string.Empty == this.txtDecimalPlaces.Text ? 0 : Convert.ToInt32(this.txtDecimalPlaces.Text))
      {
        CriterionName = this.currentField == null ? string.Empty : this.currentField.CriterionFieldName
      };
    }

    public void ClearColumnInfo() => this.initializeDialog();

    public void SetColumnInfo(ColumnInfo columnInfo)
    {
      this.currentField = this.fieldDefs == null ? (LoanReportFieldDef) null : this.fieldDefs.GetFieldByCriterionName(columnInfo.CriterionName);
      this.txtDescription.Enabled = true;
      this.cboSortOrder.Enabled = true;
      this.cboSummaryType.Enabled = true;
      bool flag = this.currentField != null && EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric == this.currentField.FieldType;
      this.cboSummaryType.DataSource = flag ? LoanFieldSelection.SummaryTypeOptions2.Clone() : LoanFieldSelection.SummaryTypeOptions3.Clone();
      this.cboSummaryType.DisplayMember = "Name";
      this.cboSummaryType.ValueMember = "Id";
      this.txtDecimalPlaces.Enabled = flag;
      this.txtFieldId.Text = columnInfo.FieldID;
      this.txtDescription.Text = columnInfo.Description;
      this.cboSortOrder.SelectedValue = (object) (int) columnInfo.SortOrder;
      this.cboSummaryType.SelectedValue = (object) (int) columnInfo.SummaryType;
      this.txtDecimalPlaces.Text = columnInfo.DecimalPlaces == 0 ? string.Empty : columnInfo.DecimalPlaces.ToString();
      this.btnOK.Enabled = false;
    }

    private void initializeDialog()
    {
      this.currentField = (LoanReportFieldDef) null;
      this.txtFieldId.Text = string.Empty;
      this.txtDescription.Text = string.Empty;
      this.txtDecimalPlaces.Text = string.Empty;
      this.txtDescription.Enabled = false;
      this.txtDecimalPlaces.Enabled = false;
      this.cboSortOrder.DataSource = LoanFieldSelection.SortOrderOptions.Clone();
      this.cboSortOrder.DisplayMember = "Name";
      this.cboSortOrder.ValueMember = "Id";
      this.cboSortOrder.SelectedIndex = -1;
      this.cboSortOrder.Enabled = false;
      this.cboSummaryType.DataSource = LoanFieldSelection.SummaryTypeOptions2.Clone();
      this.cboSummaryType.DisplayMember = "Name";
      this.cboSummaryType.ValueMember = "Id";
      this.cboSummaryType.SelectedIndex = -1;
      this.cboSummaryType.Enabled = false;
    }

    private void loadField(LoanReportFieldDef selectedField)
    {
      this.currentField = selectedField;
      this.txtFieldId.Text = selectedField.FieldID;
      this.txtDescription.Text = selectedField.Description;
      this.cboSortOrder.SelectedValue = (object) 0;
      this.cboSummaryType.DataSource = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric == selectedField.FieldType ? LoanFieldSelection.SummaryTypeOptions2.Clone() : LoanFieldSelection.SummaryTypeOptions3.Clone();
      this.cboSummaryType.DisplayMember = "Name";
      this.cboSummaryType.ValueMember = "Id";
      this.cboSummaryType.SelectedValue = (object) 0;
      this.txtDescription.Enabled = true;
      this.cboSortOrder.Enabled = true;
      this.cboSummaryType.Enabled = true;
      this.txtDecimalPlaces.Enabled = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric == selectedField.FieldType;
      this.btnOK.Enabled = true;
      this.cboSortOrder.Focus();
    }

    private void picFindField_MouseEnter(object sender, EventArgs e)
    {
      this.picFindField.Image = this.imageList.Images["searchOver"];
    }

    private void picFindField_MouseLeave(object sender, EventArgs e)
    {
      this.picFindField.Image = this.imageList.Images["search"];
    }

    private void picFindField_Click(object sender, EventArgs e)
    {
      this.dlgFindField = new FindLoanFieldDialog(this.fieldDefs);
      this.dlgFindField.ColumnTypeFilter = this.columnTypeFilter;
      if (DialogResult.OK != this.dlgFindField.ShowDialog((IWin32Window) this))
        return;
      this.loadField(this.dlgFindField.GetSelectedField());
    }

    private void txtDescription_TextChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = 0 < this.txtDescription.Text.Length;
    }

    private void cboSortOrder_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnOK.Enabled = 0 < this.txtDescription.Text.Length;
    }

    private void cboSummaryType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnOK.Enabled = 0 < this.txtDescription.Text.Length;
    }

    private void txtDecimalPlaces_KeyDown(object sender, KeyEventArgs e)
    {
      this.nonNumberEntered = false;
      if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 || e.KeyCode == Keys.Back)
        return;
      this.nonNumberEntered = true;
    }

    private void txtDecimalPlaces_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.nonNumberEntered)
        e.Handled = true;
      this.btnOK.Enabled = 0 < this.txtDescription.Text.Length;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (string.Empty == this.txtFieldId.Text.Trim())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a field id.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtFieldId.Focus();
      }
      else if (string.Empty == this.txtDescription.Text.Trim())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a field description.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtDescription.Focus();
      }
      else
      {
        foreach (ColumnInfo columnInfo in this.columnInfoList)
        {
          if (string.Equals(columnInfo.Description, this.txtDescription.Text.Trim(), StringComparison.OrdinalIgnoreCase))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "There is already a field with this description. Please enter a different field description.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.txtDescription.Focus();
            return;
          }
        }
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditLoanFieldDialog));
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.cboSortOrder = new ComboBox();
      this.lblSortOrder = new Label();
      this.lblDescription = new Label();
      this.txtDescription = new TextBox();
      this.lblFieldId = new Label();
      this.txtFieldId = new TextBox();
      this.picFindField = new PictureBox();
      this.imageList = new ImageList(this.components);
      this.pnlSummaryFields = new Panel();
      this.cboSummaryType = new ComboBox();
      this.txtDecimalPlaces = new TextBox();
      this.lblDecimalPlaces = new Label();
      this.lblSummaryType = new Label();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picFindField).BeginInit();
      this.pnlSummaryFields.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(307, 144);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(226, 144);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.cboSortOrder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSortOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSortOrder.Enabled = false;
      this.cboSortOrder.Location = new Point(101, 64);
      this.cboSortOrder.Name = "cboSortOrder";
      this.cboSortOrder.Size = new Size(281, 21);
      this.cboSortOrder.TabIndex = 49;
      this.cboSortOrder.SelectionChangeCommitted += new EventHandler(this.cboSortOrder_SelectionChangeCommitted);
      this.lblSortOrder.AutoSize = true;
      this.lblSortOrder.Location = new Point(12, 68);
      this.lblSortOrder.Name = "lblSortOrder";
      this.lblSortOrder.Size = new Size(55, 13);
      this.lblSortOrder.TabIndex = 55;
      this.lblSortOrder.Text = "Sort Order";
      this.lblSortOrder.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(12, 42);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 54;
      this.lblDescription.Text = "Description";
      this.lblDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Enabled = false;
      this.txtDescription.Location = new Point(101, 38);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(281, 20);
      this.txtDescription.TabIndex = 47;
      this.txtDescription.TextChanged += new EventHandler(this.txtDescription_TextChanged);
      this.lblFieldId.AutoSize = true;
      this.lblFieldId.Location = new Point(12, 16);
      this.lblFieldId.Name = "lblFieldId";
      this.lblFieldId.Size = new Size(43, 13);
      this.lblFieldId.TabIndex = 53;
      this.lblFieldId.Text = "Field ID";
      this.lblFieldId.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFieldId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFieldId.Location = new Point(101, 12);
      this.txtFieldId.MaxLength = 100;
      this.txtFieldId.Name = "txtFieldId";
      this.txtFieldId.ReadOnly = true;
      this.txtFieldId.Size = new Size(281, 20);
      this.txtFieldId.TabIndex = 48;
      this.picFindField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picFindField.Image = (Image) componentResourceManager.GetObject("picFindField.Image");
      this.picFindField.Location = new Point(388, 14);
      this.picFindField.Name = "picFindField";
      this.picFindField.Size = new Size(16, 16);
      this.picFindField.TabIndex = 50;
      this.picFindField.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.picFindField, "Lookup");
      this.picFindField.MouseLeave += new EventHandler(this.picFindField_MouseLeave);
      this.picFindField.Click += new EventHandler(this.picFindField_Click);
      this.picFindField.MouseEnter += new EventHandler(this.picFindField_MouseEnter);
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "search");
      this.imageList.Images.SetKeyName(1, "searchOver");
      this.pnlSummaryFields.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlSummaryFields.Controls.Add((Control) this.cboSummaryType);
      this.pnlSummaryFields.Controls.Add((Control) this.txtDecimalPlaces);
      this.pnlSummaryFields.Controls.Add((Control) this.lblDecimalPlaces);
      this.pnlSummaryFields.Controls.Add((Control) this.lblSummaryType);
      this.pnlSummaryFields.Location = new Point(0, 85);
      this.pnlSummaryFields.Name = "pnlSummaryFields";
      this.pnlSummaryFields.Size = new Size(418, 53);
      this.pnlSummaryFields.TabIndex = 58;
      this.cboSummaryType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSummaryType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSummaryType.Enabled = false;
      this.cboSummaryType.Location = new Point(101, 6);
      this.cboSummaryType.Name = "cboSummaryType";
      this.cboSummaryType.Size = new Size(281, 21);
      this.cboSummaryType.TabIndex = 58;
      this.cboSummaryType.SelectionChangeCommitted += new EventHandler(this.cboSummaryType_SelectionChangeCommitted);
      this.txtDecimalPlaces.Enabled = false;
      this.txtDecimalPlaces.Location = new Point(101, 33);
      this.txtDecimalPlaces.MaxLength = 1;
      this.txtDecimalPlaces.Name = "txtDecimalPlaces";
      this.txtDecimalPlaces.Size = new Size(52, 20);
      this.txtDecimalPlaces.TabIndex = 59;
      this.txtDecimalPlaces.KeyDown += new KeyEventHandler(this.txtDecimalPlaces_KeyDown);
      this.lblDecimalPlaces.AutoSize = true;
      this.lblDecimalPlaces.Location = new Point(12, 37);
      this.lblDecimalPlaces.Name = "lblDecimalPlaces";
      this.lblDecimalPlaces.Size = new Size(80, 13);
      this.lblDecimalPlaces.TabIndex = 61;
      this.lblDecimalPlaces.Text = "Decimal Places";
      this.lblDecimalPlaces.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSummaryType.AutoSize = true;
      this.lblSummaryType.Location = new Point(12, 10);
      this.lblSummaryType.Name = "lblSummaryType";
      this.lblSummaryType.Size = new Size(77, 13);
      this.lblSummaryType.TabIndex = 60;
      this.lblSummaryType.Text = "Summary Type";
      this.lblSummaryType.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(416, 180);
      this.Controls.Add((Control) this.pnlSummaryFields);
      this.Controls.Add((Control) this.cboSortOrder);
      this.Controls.Add((Control) this.lblSortOrder);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.lblFieldId);
      this.Controls.Add((Control) this.txtFieldId);
      this.Controls.Add((Control) this.picFindField);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditLoanFieldDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Loan Field";
      ((ISupportInitialize) this.picFindField).EndInit();
      this.pnlSummaryFields.ResumeLayout(false);
      this.pnlSummaryFields.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
