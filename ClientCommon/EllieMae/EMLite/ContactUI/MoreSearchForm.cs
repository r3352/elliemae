// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MoreSearchForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using System;
using System.Collections;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MoreSearchForm : Form
  {
    private Panel panel1;
    private ComboBox cmbBoxValue2;
    private Label lblValue2;
    private Label lblAnd;
    private ComboBox cmbBoxValue1;
    private ComboBox cmbBoxCondition;
    protected TextBox txtBoxContactFieldName;
    private Label label15;
    private Label lblValue1;
    private Label lblCondition;
    private Button btnFields;
    private Button btnAddCriterion;
    private Button btnRemoveCreterion;
    private Label label14;
    private ListView listViewMoreCriteria;
    private ColumnHeader columnHeaderFieldName;
    private ColumnHeader columnHeaderCondition;
    private ColumnHeader columnHeaderValue;
    private System.ComponentModel.Container components;
    private bool initialLoad = true;
    private bool deleteBackKey;
    private ContextMenu fieldMenu;
    protected string _ContactLabel = string.Empty;
    protected string _DbColumnName = string.Empty;
    protected bool isFieldBool;
    protected FieldFormat __FieldFormat = FieldFormat.STRING;
    private ArrayList _MoreCriteria = new ArrayList();
    protected Hashtable _MenuItemToField = new Hashtable();

    protected FieldFormat _FieldFormat
    {
      get => this.__FieldFormat;
      set
      {
        this.__FieldFormat = value;
        this.setControlsForNonDropDownField();
      }
    }

    public ArrayList MoreCriteria
    {
      get => this._MoreCriteria;
      set
      {
        this._MoreCriteria = value;
        this.listViewMoreCriteria.Items.Clear();
        for (int index = 0; index < this._MoreCriteria.Count; ++index)
          this.addCriterionToListView((ContactQueryItem) this._MoreCriteria[index]);
      }
    }

    public MoreSearchForm()
    {
      this.InitializeComponent();
      this.init();
    }

    private void init()
    {
      this.initialLoad = true;
      this.listViewMoreCriteria.Items.Clear();
      this.txtBoxContactFieldName.Text = string.Empty;
      this.cmbBoxCondition.Text = string.Empty;
      this.cmbBoxCondition.Enabled = false;
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = false;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Enabled = false;
      this.initialLoad = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (MoreSearchForm));
      this.panel1 = new Panel();
      this.cmbBoxValue2 = new ComboBox();
      this.lblValue2 = new Label();
      this.lblAnd = new Label();
      this.cmbBoxValue1 = new ComboBox();
      this.cmbBoxCondition = new ComboBox();
      this.txtBoxContactFieldName = new TextBox();
      this.label15 = new Label();
      this.lblValue1 = new Label();
      this.lblCondition = new Label();
      this.btnFields = new Button();
      this.btnAddCriterion = new Button();
      this.btnRemoveCreterion = new Button();
      this.label14 = new Label();
      this.listViewMoreCriteria = new ListView();
      this.columnHeaderFieldName = new ColumnHeader();
      this.columnHeaderCondition = new ColumnHeader();
      this.columnHeaderValue = new ColumnHeader();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.cmbBoxValue2);
      this.panel1.Controls.Add((Control) this.lblValue2);
      this.panel1.Controls.Add((Control) this.lblAnd);
      this.panel1.Controls.Add((Control) this.cmbBoxValue1);
      this.panel1.Controls.Add((Control) this.cmbBoxCondition);
      this.panel1.Controls.Add((Control) this.txtBoxContactFieldName);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.lblValue1);
      this.panel1.Controls.Add((Control) this.lblCondition);
      this.panel1.Controls.Add((Control) this.btnFields);
      this.panel1.Controls.Add((Control) this.btnAddCriterion);
      this.panel1.Location = new Point(8, 144);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(440, (int) sbyte.MaxValue);
      this.panel1.TabIndex = 3;
      this.cmbBoxValue2.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxValue2.Location = new Point(332, 64);
      this.cmbBoxValue2.Name = "cmbBoxValue2";
      this.cmbBoxValue2.Size = new Size(96, 21);
      this.cmbBoxValue2.TabIndex = 9;
      this.lblValue2.Location = new Point(336, 40);
      this.lblValue2.Name = "lblValue2";
      this.lblValue2.Size = new Size(76, 16);
      this.lblValue2.TabIndex = 8;
      this.lblValue2.Text = "Value:";
      this.lblAnd.Location = new Point(304, 68);
      this.lblAnd.Name = "lblAnd";
      this.lblAnd.Size = new Size(24, 16);
      this.lblAnd.TabIndex = 7;
      this.lblAnd.Text = "and";
      this.cmbBoxValue1.Location = new Point(204, 64);
      this.cmbBoxValue1.Name = "cmbBoxValue1";
      this.cmbBoxValue1.Size = new Size(96, 21);
      this.cmbBoxValue1.TabIndex = 6;
      this.cmbBoxValue1.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.cmbBoxValue1.TextChanged += new EventHandler(this.controlChanged);
      this.cmbBoxCondition.Location = new Point(112, 64);
      this.cmbBoxCondition.Name = "cmbBoxCondition";
      this.cmbBoxCondition.Size = new Size(84, 21);
      this.cmbBoxCondition.TabIndex = 4;
      this.cmbBoxCondition.SelectedIndexChanged += new EventHandler(this.cmbBoxCondition_SelectedIndexChanged);
      this.txtBoxContactFieldName.Location = new Point(4, 64);
      this.txtBoxContactFieldName.Name = "txtBoxContactFieldName";
      this.txtBoxContactFieldName.ReadOnly = true;
      this.txtBoxContactFieldName.TabIndex = 2;
      this.txtBoxContactFieldName.Text = "";
      this.label15.Location = new Point(4, 8);
      this.label15.Name = "label15";
      this.label15.Size = new Size(292, 16);
      this.label15.TabIndex = 0;
      this.label15.Text = "Define search criterion here and add it to the list above:";
      this.lblValue1.Location = new Point(204, 40);
      this.lblValue1.Name = "lblValue1";
      this.lblValue1.Size = new Size(80, 16);
      this.lblValue1.TabIndex = 5;
      this.lblValue1.Text = "Value:";
      this.lblCondition.Location = new Point(112, 40);
      this.lblCondition.Name = "lblCondition";
      this.lblCondition.Size = new Size(72, 16);
      this.lblCondition.TabIndex = 3;
      this.lblCondition.Text = "Condition:";
      this.btnFields.Image = (Image) resourceManager.GetObject("btnFields.Image");
      this.btnFields.ImageAlign = ContentAlignment.MiddleRight;
      this.btnFields.Location = new Point(4, 32);
      this.btnFields.Name = "btnFields";
      this.btnFields.Size = new Size(100, 24);
      this.btnFields.TabIndex = 1;
      this.btnFields.Text = "Contact Fields     ";
      this.btnFields.TextAlign = ContentAlignment.MiddleLeft;
      this.btnFields.Click += new EventHandler(this.btnFields_Click);
      this.btnAddCriterion.Location = new Point(320, 92);
      this.btnAddCriterion.Name = "btnAddCriterion";
      this.btnAddCriterion.Size = new Size(112, 24);
      this.btnAddCriterion.TabIndex = 10;
      this.btnAddCriterion.Text = "Add To Search List";
      this.btnAddCriterion.Click += new EventHandler(this.btnAddCriterion_Click);
      this.btnRemoveCreterion.Location = new Point(388, 37);
      this.btnRemoveCreterion.Name = "btnRemoveCreterion";
      this.btnRemoveCreterion.Size = new Size(60, 24);
      this.btnRemoveCreterion.TabIndex = 2;
      this.btnRemoveCreterion.Text = "Remove";
      this.btnRemoveCreterion.Click += new EventHandler(this.btnRemoveCreterion_Click);
      this.label14.Location = new Point(12, 8);
      this.label14.Name = "label14";
      this.label14.Size = new Size(228, 16);
      this.label14.TabIndex = 0;
      this.label14.Text = "Find contacts that match these criteria:";
      this.listViewMoreCriteria.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeaderFieldName,
        this.columnHeaderCondition,
        this.columnHeaderValue
      });
      this.listViewMoreCriteria.FullRowSelect = true;
      this.listViewMoreCriteria.Location = new Point(8, 28);
      this.listViewMoreCriteria.Name = "listViewMoreCriteria";
      this.listViewMoreCriteria.Size = new Size(372, 111);
      this.listViewMoreCriteria.TabIndex = 1;
      this.listViewMoreCriteria.View = View.Details;
      this.columnHeaderFieldName.Text = "Field Name";
      this.columnHeaderFieldName.Width = 110;
      this.columnHeaderCondition.Text = "Condition";
      this.columnHeaderCondition.Width = 70;
      this.columnHeaderValue.Text = "Value";
      this.columnHeaderValue.Width = 185;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(456, 280);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnRemoveCreterion);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.listViewMoreCriteria);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (MoreSearchForm);
      this.Text = nameof (MoreSearchForm);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    protected void setContextMenu(ContextMenu ctxMenu) => this.fieldMenu = ctxMenu;

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        ComboBox comboBox = (ComboBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(comboBox.Text, this._FieldFormat, ref needsUpdate);
        if (!needsUpdate)
          return;
        comboBox.Text = str;
        comboBox.SelectionStart = str.Length;
      }
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
    }

    private string valueTypeToString(FieldFormat valueType)
    {
      switch (valueType)
      {
        case FieldFormat.YN:
          return "System.String";
        case FieldFormat.X:
          return this.isFieldBool ? "System.Boolean" : "EllieMae.EMLite.Common.FieldFormat.X";
        case FieldFormat.INTEGER:
          return "System.Int32";
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return "System.Decimal";
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
          return "System.DateTime";
        default:
          return "System.String";
      }
    }

    public void LoadQuery(QueryCriterion[] defaultCriteria, RelatedLoanMatchType defaultMatchType)
    {
    }

    public void Reset()
    {
      this._DbColumnName = string.Empty;
      this._FieldFormat = FieldFormat.STRING;
      this._MoreCriteria = new ArrayList();
      this.init();
    }

    public ListViewItem addCriterionToListView(ContactQueryItem criterionInfo)
    {
      string str = criterionInfo.Value1;
      if (criterionInfo.Value2 != string.Empty)
        str = str + " and " + criterionInfo.Value2;
      ListViewItem listView = new ListViewItem(new string[3]
      {
        criterionInfo.FieldDisplayName,
        criterionInfo.Condition,
        str
      });
      listView.Tag = (object) criterionInfo;
      this.listViewMoreCriteria.Items.Add(listView);
      return listView;
    }

    public bool ExistsNotAddedCriterion()
    {
      return !(this.txtBoxContactFieldName.Text.Trim() == string.Empty) && !(this.cmbBoxCondition.Text.Trim() == string.Empty) && (!(this.cmbBoxValue1.Text.Trim() == string.Empty) || this._FieldFormat == FieldFormat.X) && (!this.cmbBoxValue2.Enabled || !(this.cmbBoxValue2.Text.Trim() == string.Empty));
    }

    protected void setControlsForStringField()
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      if ("Status" != this.txtBoxContactFieldName.Text)
        this.cmbBoxCondition.Items.Remove((object) TextConditionEnumUtil.ValueToName(TextCondition.IsNot));
      this.cmbBoxCondition.SelectedIndex = 0;
      this.cmbBoxCondition.Enabled = true;
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForNumberField()
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Items.AddRange(NumberConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCondition.SelectedIndex = 0;
      this.cmbBoxCondition.Enabled = true;
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForYNField()
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Text = "Is";
      this.cmbBoxCondition.Enabled = false;
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.Items.AddRange((object[]) new string[2]
      {
        "Y",
        "N"
      });
      this.cmbBoxValue1.SelectedIndex = 0;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForXField()
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Text = "Is";
      this.cmbBoxCondition.Enabled = false;
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.Items.AddRange((object[]) new string[2]
      {
        "",
        "X"
      });
      this.cmbBoxValue1.SelectedIndex = 0;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForDropDownListField(object[] options)
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Text = "Is";
      this.cmbBoxCondition.Enabled = false;
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.Items.AddRange(options);
      this.cmbBoxValue1.SelectedIndex = 0;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForDropDownField(object[] options)
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCondition.SelectedIndex = 0;
      this.cmbBoxCondition.Enabled = true;
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.DropDown;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.Items.AddRange(options);
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForDateField()
    {
      this.cmbBoxCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCondition.SelectedIndex = 0;
      this.cmbBoxCondition.Enabled = true;
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = true;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.DropDownStyle = ComboBoxStyle.Simple;
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Enabled = false;
    }

    protected void setControlsForNonDropDownField()
    {
      if (this._FieldFormat == FieldFormat.DROPDOWN || this._FieldFormat == FieldFormat.DROPDOWNLIST)
        return;
      if (this._FieldFormat == FieldFormat.DATE || this._FieldFormat == FieldFormat.MONTHDAY)
        this.setControlsForDateField();
      else if (this._FieldFormat == FieldFormat.DECIMAL || this._FieldFormat == FieldFormat.DECIMAL_1 || this._FieldFormat == FieldFormat.DECIMAL_2 || this._FieldFormat == FieldFormat.DECIMAL_3 || this._FieldFormat == FieldFormat.DECIMAL_4 || this._FieldFormat == FieldFormat.DECIMAL_5 || this._FieldFormat == FieldFormat.DECIMAL_6 || this._FieldFormat == FieldFormat.DECIMAL_7 || this._FieldFormat == FieldFormat.DECIMAL_10 || this._FieldFormat == FieldFormat.INTEGER)
        this.setControlsForNumberField();
      else if (this._FieldFormat == FieldFormat.YN)
        this.setControlsForYNField();
      else if (this._FieldFormat == FieldFormat.X)
        this.setControlsForXField();
      else
        this.setControlsForStringField();
    }

    private void cmbBoxCondition_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxCondition.Text.ToLower() == "between")
      {
        this.cmbBoxValue2.Text = string.Empty;
        this.cmbBoxValue2.Items.Clear();
        this.cmbBoxValue2.Enabled = true;
      }
      else
      {
        this.cmbBoxValue2.Text = string.Empty;
        this.cmbBoxValue2.Items.Clear();
        this.cmbBoxValue2.Enabled = false;
      }
    }

    private bool validateCriterion()
    {
      if (this.txtBoxContactFieldName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please click the 'Contact Fields' button and select a field to search against from the context menu.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnFields.Focus();
        return false;
      }
      if (this.cmbBoxCondition.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a condition from the 'Condition' dropdown list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbBoxCondition.Focus();
        return false;
      }
      if (this.cmbBoxValue1.Text.Trim() == string.Empty && this._FieldFormat != FieldFormat.X)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the value you want to search for in the " + this.txtBoxContactFieldName.Text + " field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbBoxValue1.Focus();
        return false;
      }
      if (this.cmbBoxValue2.Enabled && this.cmbBoxValue2.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the value you want to search for in the " + this.txtBoxContactFieldName.Text + " field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbBoxValue2.Focus();
        return false;
      }
      if (!this.validateValue(this.cmbBoxValue1.Text.Trim()))
      {
        this.cmbBoxValue1.Focus();
        return false;
      }
      if (!this.cmbBoxValue2.Enabled || this.validateValue(this.cmbBoxValue2.Text.Trim()))
        return true;
      this.cmbBoxValue2.Focus();
      return false;
    }

    private bool validateValue(string value)
    {
      switch (this._FieldFormat)
      {
        case FieldFormat.INTEGER:
          if (!Utils.IsInt((object) value))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value " + value + " you entered is not a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          break;
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          if (!Utils.IsDecimal((object) value))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value " + value + " you entered is not a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          break;
        case FieldFormat.DATE:
          if (!Utils.IsDate((object) value))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value " + value + " you entered is not a valid date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          break;
        default:
          return true;
      }
      return true;
    }

    public bool AddCriterion()
    {
      if (!this.validateCriterion())
        return false;
      ContactQueryItem criterionInfo = new ContactQueryItem();
      criterionInfo.FieldDisplayName = this.txtBoxContactFieldName.Text;
      criterionInfo.FieldName = this._DbColumnName;
      criterionInfo.Condition = this.cmbBoxCondition.Text;
      criterionInfo.Value1 = this.cmbBoxValue1.Text;
      criterionInfo.Value2 = this.cmbBoxValue2.Text;
      criterionInfo.ValueType = this.valueTypeToString(this._FieldFormat);
      criterionInfo.GroupName = "MoreChoices";
      this._MoreCriteria.Add((object) criterionInfo);
      this.addCriterionToListView(criterionInfo);
      this.ClearSearchCriterionToAdd();
      return true;
    }

    public void ClearSearchCriterionToAdd()
    {
      this.txtBoxContactFieldName.Text = string.Empty;
      this.cmbBoxCondition.Items.Clear();
      this.cmbBoxCondition.Text = string.Empty;
      this.cmbBoxCondition.Enabled = false;
      this.cmbBoxValue1.Items.Clear();
      this.cmbBoxValue1.Text = string.Empty;
      this.cmbBoxValue1.Enabled = false;
      this.cmbBoxValue2.Items.Clear();
      this.cmbBoxValue2.Text = string.Empty;
      this.cmbBoxValue2.Enabled = false;
    }

    private void btnAddCriterion_Click(object sender, EventArgs e) => this.AddCriterion();

    private void btnRemoveCreterion_Click(object sender, EventArgs e)
    {
      if (this.listViewMoreCriteria.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select items in the list to remove.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        foreach (ListViewItem selectedItem in this.listViewMoreCriteria.SelectedItems)
        {
          this._MoreCriteria.Remove(selectedItem.Tag);
          this.listViewMoreCriteria.Items.Remove(selectedItem);
        }
      }
    }

    private void btnFields_Click(object sender, EventArgs e)
    {
      this.fieldMenu.Show((Control) sender, ((Control) sender).ClientRectangle.Location);
    }
  }
}
