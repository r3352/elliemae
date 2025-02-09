// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CustomFieldEditor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CustomFieldEditor : Form
  {
    private CustomFieldMappingCollection customFieldMappingCollection;
    private BizCategory[] allCategories;
    private CustomFieldsInfo customFields;
    private CustomFieldInfo currentField;
    private bool calculationValidated;
    private Label label2;
    private TextBox txtFieldID;
    private Label label4;
    private Label label5;
    private Panel pnlStringProperties;
    private Label label6;
    private Panel pnlNumberProperties;
    private TextBox txtMaxLength;
    private TextBox txtCalc;
    private Label label10;
    private Button btnValidateCalc;
    private System.ComponentModel.Container components;
    private ComboBox cboFormat;
    private TextBox txtDescription;
    private Label lblFieldIDPrefix;
    private Label label12;
    private Label label13;
    private RadioButton radAuditUserID;
    private RadioButton radAuditUserName;
    private RadioButton radAuditTimestamp;
    private Panel pnlAuditProperties;
    private TextBox txtAuditFieldID;
    private Panel pnlListProperties;
    private ListView lvwOptions;
    private Button btnDeleteOption;
    private Button btnDownOption;
    private Button btnUpOption;
    private Button btnAddOption;
    private Label label9;
    private ColumnHeader columnHeader1;
    private GroupBox grpSeparator;
    private Label lblFieldID;
    private DialogButtons dlgButtons;
    private Panel pnlCalc;
    private Hashtable selectedFields;
    private const int maxOptionLength = 256;
    private Sessions.Session session;

    public CustomFieldEditor(
      Sessions.Session session,
      CustomFieldsInfo customFields,
      CustomFieldInfo field)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.session.EncompassEdition == EncompassEdition.Broker)
      {
        this.pnlCalc.Visible = false;
        this.grpSeparator.Width -= this.pnlCalc.Width;
        this.Width -= this.pnlCalc.Width;
      }
      this.loadFieldFormatList();
      this.customFieldMappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(this.session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower | CustomFieldsType.BizPartner | CustomFieldsType.BizCategoryCustom, false));
      this.customFields = customFields;
      if (field == null)
        field = new CustomFieldInfo("CX.");
      this.setCurrentField(field);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlStringProperties = new Panel();
      this.txtMaxLength = new TextBox();
      this.label6 = new Label();
      this.cboFormat = new ComboBox();
      this.label5 = new Label();
      this.txtDescription = new TextBox();
      this.label4 = new Label();
      this.txtFieldID = new TextBox();
      this.lblFieldIDPrefix = new Label();
      this.label2 = new Label();
      this.pnlListProperties = new Panel();
      this.lvwOptions = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.btnDeleteOption = new Button();
      this.btnDownOption = new Button();
      this.btnUpOption = new Button();
      this.btnAddOption = new Button();
      this.label9 = new Label();
      this.pnlNumberProperties = new Panel();
      this.pnlAuditProperties = new Panel();
      this.txtAuditFieldID = new TextBox();
      this.radAuditTimestamp = new RadioButton();
      this.radAuditUserName = new RadioButton();
      this.radAuditUserID = new RadioButton();
      this.label12 = new Label();
      this.label13 = new Label();
      this.btnValidateCalc = new Button();
      this.label10 = new Label();
      this.txtCalc = new TextBox();
      this.grpSeparator = new GroupBox();
      this.lblFieldID = new Label();
      this.pnlCalc = new Panel();
      this.dlgButtons = new DialogButtons();
      this.pnlStringProperties.SuspendLayout();
      this.pnlListProperties.SuspendLayout();
      this.pnlAuditProperties.SuspendLayout();
      this.pnlCalc.SuspendLayout();
      this.SuspendLayout();
      this.pnlStringProperties.Controls.Add((Control) this.txtMaxLength);
      this.pnlStringProperties.Controls.Add((Control) this.label6);
      this.pnlStringProperties.Location = new Point(0, 77);
      this.pnlStringProperties.Name = "pnlStringProperties";
      this.pnlStringProperties.Size = new Size(312, 21);
      this.pnlStringProperties.TabIndex = 8;
      this.txtMaxLength.Location = new Point(102, 1);
      this.txtMaxLength.MaxLength = 5;
      this.txtMaxLength.Name = "txtMaxLength";
      this.txtMaxLength.Size = new Size(74, 20);
      this.txtMaxLength.TabIndex = 8;
      this.txtMaxLength.TextChanged += new EventHandler(this.txtMaxLength_TextChanged);
      this.txtMaxLength.KeyPress += new KeyPressEventHandler(this.requireIntHandler);
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(18, 4);
      this.label6.Name = "label6";
      this.label6.Size = new Size(80, 15);
      this.label6.TabIndex = 7;
      this.label6.Text = "Max Length";
      this.cboFormat.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFormat.Location = new Point(102, 54);
      this.cboFormat.Name = "cboFormat";
      this.cboFormat.Size = new Size(188, 22);
      this.cboFormat.Sorted = true;
      this.cboFormat.TabIndex = 7;
      this.cboFormat.SelectedIndexChanged += new EventHandler(this.cboFormat_SelectedIndexChanged);
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(18, 58);
      this.label5.Name = "label5";
      this.label5.Size = new Size(80, 15);
      this.label5.TabIndex = 6;
      this.label5.Text = "Format";
      this.txtDescription.Location = new Point(102, 32);
      this.txtDescription.MaxLength = 250;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(188, 20);
      this.txtDescription.TabIndex = 5;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(18, 35);
      this.label4.Name = "label4";
      this.label4.Size = new Size(80, 15);
      this.label4.TabIndex = 4;
      this.label4.Text = "Description";
      this.txtFieldID.CharacterCasing = CharacterCasing.Upper;
      this.txtFieldID.Location = new Point(123, 10);
      this.txtFieldID.MaxLength = 25;
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(167, 20);
      this.txtFieldID.TabIndex = 2;
      this.txtFieldID.TextChanged += new EventHandler(this.txtFieldID_TextChanged);
      this.txtFieldID.KeyPress += new KeyPressEventHandler(this.fieldIdCharFilter);
      this.lblFieldIDPrefix.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFieldIDPrefix.Location = new Point(100, 13);
      this.lblFieldIDPrefix.Name = "lblFieldIDPrefix";
      this.lblFieldIDPrefix.Size = new Size(26, 15);
      this.lblFieldIDPrefix.TabIndex = 3;
      this.lblFieldIDPrefix.Text = "CX.";
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(18, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Field ID";
      this.pnlListProperties.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlListProperties.Controls.Add((Control) this.lvwOptions);
      this.pnlListProperties.Controls.Add((Control) this.btnDeleteOption);
      this.pnlListProperties.Controls.Add((Control) this.btnDownOption);
      this.pnlListProperties.Controls.Add((Control) this.btnUpOption);
      this.pnlListProperties.Controls.Add((Control) this.btnAddOption);
      this.pnlListProperties.Controls.Add((Control) this.label9);
      this.pnlListProperties.Location = new Point(0, 98);
      this.pnlListProperties.Name = "pnlListProperties";
      this.pnlListProperties.Size = new Size(310, 145);
      this.pnlListProperties.TabIndex = 11;
      this.lvwOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwOptions.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lvwOptions.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwOptions.HideSelection = false;
      this.lvwOptions.LabelEdit = true;
      this.lvwOptions.LabelWrap = false;
      this.lvwOptions.Location = new Point(18, 23);
      this.lvwOptions.MultiSelect = false;
      this.lvwOptions.Name = "lvwOptions";
      this.lvwOptions.Size = new Size(194, 118);
      this.lvwOptions.TabIndex = 12;
      this.lvwOptions.UseCompatibleStateImageBehavior = false;
      this.lvwOptions.View = View.Details;
      this.columnHeader1.Width = 154;
      this.btnDeleteOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteOption.Location = new Point(216, 89);
      this.btnDeleteOption.Name = "btnDeleteOption";
      this.btnDeleteOption.Size = new Size(74, 22);
      this.btnDeleteOption.TabIndex = 11;
      this.btnDeleteOption.Text = "D&elete";
      this.btnDeleteOption.Click += new EventHandler(this.btnDeleteOption_Click);
      this.btnDownOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownOption.Location = new Point(216, 67);
      this.btnDownOption.Name = "btnDownOption";
      this.btnDownOption.Size = new Size(74, 22);
      this.btnDownOption.TabIndex = 10;
      this.btnDownOption.Text = "Move &Down";
      this.btnDownOption.Click += new EventHandler(this.btnDownOption_Click);
      this.btnUpOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpOption.Location = new Point(216, 45);
      this.btnUpOption.Name = "btnUpOption";
      this.btnUpOption.Size = new Size(74, 22);
      this.btnUpOption.TabIndex = 9;
      this.btnUpOption.Text = "Move &Up";
      this.btnUpOption.Click += new EventHandler(this.btnUpOption_Click);
      this.btnAddOption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddOption.Location = new Point(216, 23);
      this.btnAddOption.Name = "btnAddOption";
      this.btnAddOption.Size = new Size(74, 22);
      this.btnAddOption.TabIndex = 8;
      this.btnAddOption.Text = "&Add";
      this.btnAddOption.Click += new EventHandler(this.btnAddOption_Click);
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(18, 5);
      this.label9.Name = "label9";
      this.label9.Size = new Size(252, 16);
      this.label9.TabIndex = 7;
      this.label9.Text = "Options";
      this.pnlNumberProperties.Location = new Point(0, 78);
      this.pnlNumberProperties.Name = "pnlNumberProperties";
      this.pnlNumberProperties.Size = new Size(312, 168);
      this.pnlNumberProperties.TabIndex = 9;
      this.pnlAuditProperties.Controls.Add((Control) this.txtAuditFieldID);
      this.pnlAuditProperties.Controls.Add((Control) this.radAuditTimestamp);
      this.pnlAuditProperties.Controls.Add((Control) this.radAuditUserName);
      this.pnlAuditProperties.Controls.Add((Control) this.radAuditUserID);
      this.pnlAuditProperties.Controls.Add((Control) this.label12);
      this.pnlAuditProperties.Controls.Add((Control) this.label13);
      this.pnlAuditProperties.Location = new Point(0, 77);
      this.pnlAuditProperties.Name = "pnlAuditProperties";
      this.pnlAuditProperties.Size = new Size(312, 102);
      this.pnlAuditProperties.TabIndex = 10;
      this.txtAuditFieldID.Location = new Point(102, 1);
      this.txtAuditFieldID.MaxLength = 40;
      this.txtAuditFieldID.Name = "txtAuditFieldID";
      this.txtAuditFieldID.Size = new Size(188, 20);
      this.txtAuditFieldID.TabIndex = 13;
      this.radAuditTimestamp.Location = new Point(102, 68);
      this.radAuditTimestamp.Name = "radAuditTimestamp";
      this.radAuditTimestamp.Size = new Size(200, 18);
      this.radAuditTimestamp.TabIndex = 14;
      this.radAuditTimestamp.Text = "Date && time field changed";
      this.radAuditUserName.Location = new Point(102, 48);
      this.radAuditUserName.Name = "radAuditUserName";
      this.radAuditUserName.Size = new Size(200, 18);
      this.radAuditUserName.TabIndex = 14;
      this.radAuditUserName.Text = "Name of user who changed field";
      this.radAuditUserID.Location = new Point(102, 28);
      this.radAuditUserID.Name = "radAuditUserID";
      this.radAuditUserID.Size = new Size(200, 18);
      this.radAuditUserID.TabIndex = 14;
      this.radAuditUserID.Text = "Login ID of user who changed field";
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(18, 29);
      this.label12.Name = "label12";
      this.label12.Size = new Size(80, 15);
      this.label12.TabIndex = 9;
      this.label12.Text = "Audit Data";
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(18, 4);
      this.label13.Name = "label13";
      this.label13.Size = new Size(80, 15);
      this.label13.TabIndex = 7;
      this.label13.Text = "Audit Field";
      this.btnValidateCalc.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnValidateCalc.Location = new Point(231, 220);
      this.btnValidateCalc.Name = "btnValidateCalc";
      this.btnValidateCalc.Size = new Size(64, 22);
      this.btnValidateCalc.TabIndex = 3;
      this.btnValidateCalc.Text = "&Validate";
      this.btnValidateCalc.Click += new EventHandler(this.btnValidate_Click);
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(11, 13);
      this.label10.Name = "label10";
      this.label10.Size = new Size(75, 15);
      this.label10.TabIndex = 1;
      this.label10.Text = "Calculation";
      this.txtCalc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCalc.Location = new Point(14, 51);
      this.txtCalc.Multiline = true;
      this.txtCalc.Name = "txtCalc";
      this.txtCalc.Size = new Size(280, 160);
      this.txtCalc.TabIndex = 0;
      this.txtCalc.TextChanged += new EventHandler(this.txtCalc_TextChanged);
      this.grpSeparator.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpSeparator.Location = new Point(11, 251);
      this.grpSeparator.Name = "grpSeparator";
      this.grpSeparator.Size = new Size(599, 10);
      this.grpSeparator.TabIndex = 14;
      this.grpSeparator.TabStop = false;
      this.lblFieldID.AutoSize = true;
      this.lblFieldID.Location = new Point(12, 33);
      this.lblFieldID.Name = "lblFieldID";
      this.lblFieldID.Size = new Size(53, 14);
      this.lblFieldID.TabIndex = 15;
      this.lblFieldID.Text = "<Field ID>";
      this.pnlCalc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCalc.Controls.Add((Control) this.lblFieldID);
      this.pnlCalc.Controls.Add((Control) this.label10);
      this.pnlCalc.Controls.Add((Control) this.txtCalc);
      this.pnlCalc.Controls.Add((Control) this.btnValidateCalc);
      this.pnlCalc.Location = new Point(313, 0);
      this.pnlCalc.Name = "pnlCalc";
      this.pnlCalc.Size = new Size(307, 248);
      this.pnlCalc.TabIndex = 16;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 264);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(621, 36);
      this.dlgButtons.TabIndex = 17;
      this.dlgButtons.OK += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.ClientSize = new Size(621, 300);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.grpSeparator);
      this.Controls.Add((Control) this.pnlStringProperties);
      this.Controls.Add((Control) this.cboFormat);
      this.Controls.Add((Control) this.txtFieldID);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.lblFieldIDPrefix);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.pnlCalc);
      this.Controls.Add((Control) this.pnlListProperties);
      this.Controls.Add((Control) this.pnlAuditProperties);
      this.Controls.Add((Control) this.pnlNumberProperties);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(637, 338);
      this.Name = nameof (CustomFieldEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Custom Field Editor";
      this.pnlStringProperties.ResumeLayout(false);
      this.pnlStringProperties.PerformLayout();
      this.pnlListProperties.ResumeLayout(false);
      this.pnlAuditProperties.ResumeLayout(false);
      this.pnlAuditProperties.PerformLayout();
      this.pnlCalc.ResumeLayout(false);
      this.pnlCalc.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public CustomFieldsInfo CustomFields => this.customFields;

    public CustomFieldInfo CurrentField => this.currentField;

    private void loadFieldFormatList()
    {
      this.cboFormat.Items.Clear();
      this.cboFormat.Items.AddRange(FieldFormatEnumUtil.GetDisplayNames(true));
    }

    private void requireIntHandler(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void requireNumberHandler(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.')
        return;
      e.Handled = true;
    }

    private void fieldIdCharFilter(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '.')
        return;
      e.Handled = true;
    }

    private void setCurrentField(CustomFieldInfo field)
    {
      if (field == null)
        return;
      this.currentField = field;
      if (field.IsExtendedField())
      {
        this.txtFieldID.Width = this.txtDescription.Width - this.lblFieldIDPrefix.Width;
        this.txtFieldID.Left = this.lblFieldIDPrefix.Left + this.lblFieldIDPrefix.Width;
        this.txtFieldID.Text = field.FieldID.Substring("CX.".Length);
        this.txtFieldID.ReadOnly = false;
      }
      else
      {
        this.txtFieldID.Width = this.txtDescription.Width;
        this.txtFieldID.Left = this.txtDescription.Left;
        this.txtFieldID.Text = field.FieldID;
        this.txtFieldID.ReadOnly = true;
      }
      this.txtDescription.Text = field.Description;
      this.cboFormat.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(field.Format);
      this.txtMaxLength.Text = field.MaxLength > 0 ? field.MaxLength.ToString() : "";
      this.txtAuditFieldID.Text = field.AuditSettings == null ? "" : field.AuditSettings.FieldID;
      this.radAuditUserID.Checked = field.AuditSettings == null || field.AuditSettings.AuditData == AuditData.UserID;
      this.radAuditUserName.Checked = field.AuditSettings != null && field.AuditSettings.AuditData == AuditData.UserName;
      this.radAuditTimestamp.Checked = field.AuditSettings != null && field.AuditSettings.AuditData == AuditData.Timestamp;
      this.lvwOptions.Items.Clear();
      if (field.Options != null)
      {
        foreach (string option in field.Options)
          this.lvwOptions.Items.Add(option);
      }
      this.txtCalc.Text = field.Calculation;
      this.calculationValidated = true;
      this.btnValidateCalc.Enabled = false;
      if (field.IsCalculationAllowed())
        this.txtCalc.Enabled = true;
      else
        this.txtCalc.Enabled = false;
    }

    private bool saveCurrentField()
    {
      FieldFormat currentformat = FieldFormatEnumUtil.NameToValue(this.cboFormat.SelectedItem.ToString());
      if (this.currentField.Format != FieldFormat.NONE && this.currentField.Format != currentformat && this.verifyCustomFieldisAddedToRDB(this.currentField.FieldID) && !this.validateFieldFormatCategory(currentformat))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You can not change the format of this field from " + (object) this.currentField.Format + " to " + currentformat.ToString() + ", since this field is already in use in the Reporting Database. Please create a new custom field instead of changing the format of this existing field. Alternatively, you may remove this field from the Reporting Database, update the Reporting Database, and then update this field's format", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!this.txtFieldID.ReadOnly)
      {
        if (this.txtFieldID.Text == "")
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "The changes you have made to this custom field cannot be saved because you have not provided a valid Field ID.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (this.cboFormat.SelectedItem.ToString() == "")
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Please select a format for this custom field.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (this.txtDescription.Text.Trim() == "" && this.cboFormat.SelectedItem.ToString() != "")
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Please provide description for this custom field.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        this.txtFieldID.Text = this.txtFieldID.Text.Replace(" ", "");
      }
      if (string.Concat(this.cboFormat.SelectedItem) == FieldFormatEnumUtil.ValueToName(FieldFormat.AUDIT))
      {
        this.txtAuditFieldID.Text = this.txtAuditFieldID.Text.Replace(" ", "");
        if (this.txtAuditFieldID.Text == "")
        {
          MessageBox.Show((IWin32Window) this, "The changes you have made to this custom field cannot be saved because you have not provided the Field ID of the field to be audited.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (this.txtAuditFieldID.Text == "CX." + this.txtFieldID.Text)
        {
          MessageBox.Show((IWin32Window) this, "An audit field cannot be used to audit itself.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (EncompassFields.GetField(this.txtAuditFieldID.Text) == null && this.customFields.GetField(this.txtAuditFieldID.Text) == null)
        {
          MessageBox.Show((IWin32Window) this, "The field ID '" + this.txtAuditFieldID.Text + "' is invalid.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      else if (string.Concat(this.cboFormat.SelectedItem) == FieldFormatEnumUtil.ValueToName(FieldFormat.STRING))
      {
        bool flag = false;
        try
        {
          if (int.Parse(this.txtMaxLength.Text) <= 0)
            flag = true;
        }
        catch
        {
          flag = true;
        }
        if (flag)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Maximum length must be larger than 0.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      if (string.Concat(this.cboFormat.SelectedItem) == FieldFormatEnumUtil.ValueToName(FieldFormat.DROPDOWN) || string.Concat(this.cboFormat.SelectedItem) == FieldFormatEnumUtil.ValueToName(FieldFormat.DROPDOWNLIST))
      {
        int num1 = this.parseInt(this.txtMaxLength.Text, 0);
        if (num1 <= 0 || num1 > 256)
          num1 = 256;
        foreach (ListViewItem listViewItem in this.lvwOptions.Items)
        {
          if (listViewItem.Text.Length > num1)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The option '" + listViewItem.Text + "' exceeds the maximum number of allowed characters for the field (" + (object) num1 + "). You must modify this option or the field length to resolve this issue.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
        }
      }
      CustomFieldMapping customFieldMapping = this.customFieldMappingCollection.Find(this.currentField.FieldID);
      if (customFieldMapping != null)
      {
        StringBuilder stringBuilder = new StringBuilder("This field is mapped to: '");
        if (CustomFieldsType.Borrower == customFieldMapping.CustomFieldsType)
          stringBuilder.Append("Borrower");
        else
          stringBuilder.Append(this.getCategoryName(customFieldMapping.CategoryId));
        stringBuilder.Append(" - Custom Field " + customFieldMapping.FieldNumber.ToString() + "'.\n");
        string str = this.currentField.IsExtendedField() ? "CX." + this.txtFieldID.Text : this.txtFieldID.Text;
        if (customFieldMapping.LoanFieldId != str)
        {
          stringBuilder.Append("This mapping entry must be removed before proceeding.");
          Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtFieldID.Text = this.currentField.IsExtendedField() ? this.currentField.FieldID.Substring("CX.".Length) : this.currentField.FieldID;
          return false;
        }
        if (!this.verifyFieldFormats(this.cboFormat.SelectedIndex < 0 ? FieldFormat.STRING : FieldFormatEnumUtil.NameToValue(this.cboFormat.SelectedItem.ToString()), customFieldMapping.FieldFormat))
        {
          stringBuilder.Append("This mapping entry must be removed before proceeding.");
          Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.cboFormat.SelectedItem = (object) FieldFormatEnumUtil.ValueToName(this.currentField.Format);
          return false;
        }
      }
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.currentField.Format = this.cboFormat.SelectedIndex >= 0 ? FieldFormatEnumUtil.NameToValue(this.cboFormat.SelectedItem.ToString()) : FieldFormat.NONE;
        if (this.currentField.IsCalculationAllowed() && this.txtCalc.Text.Trim() != "" && !this.calculationValidated && !this.validateCalculation())
        {
          this.txtCalc.Focus();
          return false;
        }
        if (this.currentField.IsExtendedField())
        {
          string str = "CX." + this.txtFieldID.Text;
          if (this.currentField.FieldID != str)
          {
            if (this.customFields.GetField(str) != null)
            {
              MessageBox.Show((IWin32Window) this, "The changes you have made to this custom field cannot be saved because a field with the specified Field ID already exists.", "Custom Field Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            this.customFields.Remove(this.currentField);
            this.currentField = new CustomFieldInfo(str, this.currentField.Format);
            this.customFields.Add(this.currentField);
          }
        }
        this.currentField.Description = this.txtDescription.Text;
        if (this.currentField.IsTextValued())
          this.currentField.MaxLength = this.parseInt(this.txtMaxLength.Text, 0);
        this.currentField.Options = (string[]) null;
        if (this.currentField.IsListValued())
        {
          string[] strArray = new string[this.lvwOptions.Items.Count];
          for (int index = 0; index < this.lvwOptions.Items.Count; ++index)
            strArray[index] = this.lvwOptions.Items[index].Text;
          this.currentField.Options = strArray;
        }
        if (this.currentField.IsAuditField())
          this.currentField.AuditSettings = new FieldAuditSettings(this.txtAuditFieldID.Text.Trim(), this.getSelectedAuditDataType());
        if (this.currentField.IsCalculationAllowed())
          this.currentField.Calculation = this.txtCalc.Text.Trim();
        this.session.ConfigurationManager.UpdateLoanCustomField(this.currentField);
        return true;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool validateFieldFormatCategory(FieldFormat currentformat)
    {
      return FieldFormatEnumUtil.GetFieldFormatCategory(this.currentField.Format) == FieldFormatEnumUtil.GetFieldFormatCategory(currentformat);
    }

    private bool verifyFieldFormats(FieldFormat loanFieldFormat, FieldFormat customFieldFormat)
    {
      return loanFieldFormat == customFieldFormat || FieldFormat.STRING == loanFieldFormat && (FieldFormat.YN == customFieldFormat || FieldFormat.X == customFieldFormat || FieldFormat.ZIPCODE == customFieldFormat || FieldFormat.STATE == customFieldFormat || FieldFormat.PHONE == customFieldFormat || FieldFormat.SSN == customFieldFormat || FieldFormat.RA_STRING == customFieldFormat || FieldFormat.RA_INTEGER == customFieldFormat || FieldFormat.RA_DECIMAL_2 == customFieldFormat || FieldFormat.RA_DECIMAL_3 == customFieldFormat || FieldFormat.DROPDOWN == customFieldFormat || FieldFormat.DROPDOWNLIST == customFieldFormat) || FieldFormat.DECIMAL == loanFieldFormat && (FieldFormat.DECIMAL_1 == customFieldFormat || FieldFormat.DECIMAL_2 == customFieldFormat || FieldFormat.DECIMAL_3 == customFieldFormat || FieldFormat.DECIMAL_4 == customFieldFormat || FieldFormat.DECIMAL_5 == customFieldFormat || FieldFormat.DECIMAL_6 == customFieldFormat || FieldFormat.DECIMAL_7 == customFieldFormat || FieldFormat.DECIMAL_10 == customFieldFormat);
    }

    private string getCategoryName(int categoryId)
    {
      if (this.allCategories == null)
        this.allCategories = this.session.ContactManager.GetBizCategories();
      foreach (BizCategory allCategory in this.allCategories)
      {
        if (allCategory.CategoryID == categoryId)
          return allCategory.Name;
      }
      return "Unknown Category";
    }

    private AuditData getSelectedAuditDataType()
    {
      if (this.radAuditUserName.Checked)
        return AuditData.UserName;
      return this.radAuditTimestamp.Checked ? AuditData.Timestamp : AuditData.UserID;
    }

    private int parseInt(string value, int defaultValue)
    {
      try
      {
        return int.Parse(value, NumberStyles.Any, (IFormatProvider) null);
      }
      catch
      {
        return defaultValue;
      }
    }

    private Decimal parseDecimal(string value, Decimal defaultValue)
    {
      try
      {
        return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null);
      }
      catch
      {
        return defaultValue;
      }
    }

    private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (this.cboFormat.SelectedIndex < 0)
          return;
        FieldFormat format = FieldFormatEnumUtil.NameToValue(this.cboFormat.SelectedItem.ToString());
        CustomFieldInfo customFieldInfo = new CustomFieldInfo(this.currentField.FieldID, format);
        this.pnlStringProperties.Visible = customFieldInfo.IsTextValued() && !customFieldInfo.IsAuditField();
        this.pnlListProperties.Visible = customFieldInfo.IsListValued() && !customFieldInfo.IsAuditField();
        this.pnlNumberProperties.Visible = customFieldInfo.IsNumericValued() && !customFieldInfo.IsAuditField();
        this.pnlAuditProperties.Visible = customFieldInfo.IsAuditField();
        if (customFieldInfo.IsCalculationAllowed())
          this.txtCalc.Enabled = true;
        else
          this.txtCalc.Enabled = false;
        if (this.currentField.Format == FieldFormat.AUDIT)
        {
          if (customFieldInfo.Format != FieldFormat.AUDIT)
            this.txtCalc.Text = "";
          else
            this.txtCalc.Text = this.currentField.Calculation;
        }
        if (format == FieldFormat.DROPDOWNLIST || format == FieldFormat.DROPDOWN)
          this.txtMaxLength.Text = string.Concat((object) 256);
        else
          this.txtMaxLength.Text = "0";
      }
      catch
      {
      }
    }

    private void btnValidate_Click(object sender, EventArgs e)
    {
      if (!this.validateCalculation())
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, "The calculation is valid.", "Calculation Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.calculationValidated = true;
      this.btnValidateCalc.Enabled = false;
    }

    private bool validateCalculation()
    {
      using (RuntimeContext context = RuntimeContext.Create())
      {
        try
        {
          CustomFieldInfo customFieldInfo = new CustomFieldInfo("CX.TEST", FieldFormat.DECIMAL);
          customFieldInfo.Calculation = this.txtCalc.Text.Trim();
          if (new CustomFieldsInfo(new BinaryObject(new CustomFieldsInfo(false)
          {
            customFieldInfo
          }.ToString(), Encoding.Default).ToString(Encoding.Default)).GetField("CX.TEST").Calculation != customFieldInfo.Calculation)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, "Validation failed: the calculation contains one or more invalid characters.", "Calculation Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          new CalculationBuilder().CreateImplementation(new CustomCalculation(customFieldInfo.Calculation), context);
          return true;
        }
        catch (CompileException ex)
        {
          if (EnConfigurationSettings.GlobalSettings.Debug)
          {
            ErrorDialog.Display((Exception) ex);
            return false;
          }
          int num = (int) MessageBox.Show((IWin32Window) this, "Validation failed: the calculation contains errors or is not a valid formula.", "Calculation Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        catch (Exception ex)
        {
          ErrorDialog.Display(ex);
          return false;
        }
      }
    }

    private void txtCalc_TextChanged(object sender, EventArgs e)
    {
      this.calculationValidated = false;
      this.btnValidateCalc.Enabled = true;
    }

    private void btnAddOption_Click(object sender, EventArgs e)
    {
      this.lvwOptions.Items.Add("<New>").BeginEdit();
    }

    private void btnUpOption_Click(object sender, EventArgs e)
    {
      if (this.lvwOptions.SelectedItems.Count == 0)
        return;
      ListViewItem selectedItem = this.lvwOptions.SelectedItems[0];
      if (selectedItem.Index <= 0)
        return;
      ListViewItem listViewItem = this.lvwOptions.Items[selectedItem.Index - 1];
      string text = listViewItem.Text;
      listViewItem.Text = selectedItem.Text;
      selectedItem.Text = text;
      listViewItem.Selected = true;
    }

    private void btnDownOption_Click(object sender, EventArgs e)
    {
      if (this.lvwOptions.SelectedItems.Count == 0)
        return;
      ListViewItem selectedItem = this.lvwOptions.SelectedItems[0];
      if (selectedItem.Index >= this.lvwOptions.Items.Count - 1)
        return;
      ListViewItem listViewItem = this.lvwOptions.Items[selectedItem.Index + 1];
      string text = listViewItem.Text;
      listViewItem.Text = selectedItem.Text;
      selectedItem.Text = text;
      listViewItem.Selected = true;
    }

    private void btnDeleteOption_Click(object sender, EventArgs e)
    {
      if (this.lvwOptions.SelectedItems.Count == 0)
        return;
      this.lvwOptions.Items.Remove(this.lvwOptions.SelectedItems[0]);
    }

    private void lvwOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnUpOption.Enabled = this.lvwOptions.SelectedItems.Count > 0;
      this.btnDownOption.Enabled = this.lvwOptions.SelectedItems.Count > 0;
      this.btnDeleteOption.Enabled = this.lvwOptions.SelectedItems.Count > 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.saveCurrentField())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void txtFieldID_TextChanged(object sender, EventArgs e)
    {
      this.lblFieldID.Text = this.currentField.IsExtendedField() ? "CX." + this.txtFieldID.Text : this.txtFieldID.Text;
      this.lblFieldID.Text += " =";
    }

    private void txtMaxLength_TextChanged(object sender, EventArgs e)
    {
      switch (FieldFormatEnumUtil.NameToValue(this.cboFormat.SelectedItem.ToString()))
      {
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
          int num;
          try
          {
            num = Convert.ToInt32(this.txtMaxLength.Text);
          }
          catch
          {
            num = 256;
          }
          if (num > 0 && num <= 256)
            break;
          this.txtMaxLength.Text = string.Concat((object) 256);
          break;
      }
    }

    private bool verifyCustomFieldisAddedToRDB(string fieldID)
    {
      bool rdb = false;
      try
      {
        if (this.selectedFields == null)
          this.GetSelectedFields();
        if ((string) this.selectedFields[(object) fieldID] != null)
          rdb = true;
      }
      catch
      {
      }
      return rdb;
    }

    private void GetSelectedFields()
    {
      this.selectedFields = new Hashtable();
      LoanXDBTableList loanXdbTableList = this.session.LoanManager.GetLoanXDBTableList(false);
      if (loanXdbTableList == null)
        return;
      for (int i1 = 0; i1 < loanXdbTableList.TableCount; ++i1)
      {
        LoanXDBTable tableAt = loanXdbTableList.GetTableAt(i1);
        if (tableAt != null)
        {
          for (int i2 = 0; i2 < tableAt.FieldCount; ++i2)
          {
            LoanXDBField fieldAt = tableAt.GetFieldAt(i2);
            if (fieldAt != null && !this.selectedFields.ContainsKey((object) fieldAt.FieldID))
              this.selectedFields.Add((object) fieldAt.FieldID, (object) "0");
          }
        }
      }
    }
  }
}
