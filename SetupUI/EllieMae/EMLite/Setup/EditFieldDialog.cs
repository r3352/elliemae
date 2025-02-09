// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditFieldDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditFieldDialog : Form
  {
    private const string className = "EditFieldDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private Label label1;
    private TextBox textBoxFieldID;
    private Label label2;
    private TextBox textBoxDescription;
    private TabControl tabControl1;
    private Button okBtn;
    private Button cancelBtn;
    private TabPage tabRule;
    private TabPage tabRequired;
    private Button findBtn;
    private Label label3;
    private ComboBox comboBoxType;
    private Label label4;
    private Label label5;
    private Panel panelRange;
    private ListBox listBoxOptions;
    private Button newBtn;
    private Button upBtn;
    private Button downBtn;
    private Button deleteBtn;
    private TextBox textBoxCodes;
    private Panel panelDropdown;
    private Panel panelCodes;
    private ListView listViewFields;
    private ColumnHeader headerID;
    private ColumnHeader headerDescription;
    private Button addBtn;
    private Button findFieldBtn;
    private Button removeBtn;
    private TextBox textBoxUpper;
    private TextBox textBoxLower;
    private GroupBox groupBox1;
    private System.ComponentModel.Container components;
    private ArrayList existingFields;
    private Label label6;
    private Label label7;
    private Label label8;
    private bool useGeneralRule = true;
    private Button insertBtn;
    private Button resetBtn;
    private FieldSettings fieldSettings;
    private string ruleName;
    private string[] preConfiguredRuleNames = new string[1]
    {
      "Manner in Which Title will be Held"
    };
    private string fieldID = "";
    private string fieldDescription = "";
    private string[] requiredFields;
    private object ruleObject;
    private FieldFormat oldFieldFormat;
    private string oldFieldID = string.Empty;
    private string[] field33Options = new string[37]
    {
      "<Clear>",
      "Sole Ownership",
      "Life Estate",
      "Tenancy in Common",
      "Joint Tenancy with Right of Survivorship",
      "Tenancy by the Entirety",
      "All as Joint Tenants",
      "All as Tenants in Common",
      "As Community Property",
      "As Her Sole And Separate Property",
      "As His Sole And Separate Property",
      "As Joint Tenants",
      "As Tenants in Common",
      "As Tenancy by Entirety",
      "Each as to An Undivided One Half Interest",
      "Each as to An Undivided One Third Interest",
      "Each as to An Undivided One Fourth Interest",
      "Husband And Wife",
      "Husband And Wife as Joint Tenants",
      "Husband And Wife as Joint Tenants With Right of Survivorship",
      "Husband And Wife as Tenants in Common",
      "As Joint Tenants With Right of Survivorship",
      "Wife And Husband",
      "Both Unmarried",
      "Spouses Married to Each Other",
      "Community Property",
      "Joint Tenants",
      "Single Man",
      "Single Woman",
      "Married Man",
      "Married Woman",
      "Tenants in Common",
      "Tenancy by Entirety",
      "To be Decided in Escrow",
      "Unmarried Man",
      "Unmarried Woman",
      "Other"
    };

    public EditFieldDialog(
      Sessions.Session session,
      string fieldID,
      string desc,
      string[] requiredFields,
      object ruleObject,
      ArrayList existingFields,
      bool useGeneralRule,
      string ruleName = "")
    {
      this.existingFields = existingFields;
      this.useGeneralRule = useGeneralRule;
      this.InitializeComponent();
      this.session = session;
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      if (fieldID != string.Empty)
      {
        this.fieldID = fieldID;
        this.fieldDescription = desc;
        this.requiredFields = requiredFields;
        this.ruleObject = ruleObject;
      }
      else
      {
        this.fieldID = string.Empty;
        this.fieldDescription = string.Empty;
        this.requiredFields = (string[]) null;
        this.ruleObject = (object) null;
      }
      this.panelCodes.Location = this.panelRange.Location;
      this.panelDropdown.Location = this.panelRange.Location;
      this.ruleName = ruleName;
      this.initForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string FieldID => this.fieldID;

    public string FieldDescription => this.fieldDescription;

    public string[] RequiredFields => this.requiredFields;

    public object RuleObject => this.ruleObject;

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textBoxFieldID = new TextBox();
      this.label2 = new Label();
      this.textBoxDescription = new TextBox();
      this.tabControl1 = new TabControl();
      this.tabRule = new TabPage();
      this.panelDropdown = new Panel();
      this.insertBtn = new Button();
      this.label6 = new Label();
      this.deleteBtn = new Button();
      this.downBtn = new Button();
      this.upBtn = new Button();
      this.newBtn = new Button();
      this.listBoxOptions = new ListBox();
      this.panelCodes = new Panel();
      this.label7 = new Label();
      this.textBoxCodes = new TextBox();
      this.comboBoxType = new ComboBox();
      this.label3 = new Label();
      this.panelRange = new Panel();
      this.textBoxUpper = new TextBox();
      this.textBoxLower = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.tabRequired = new TabPage();
      this.label8 = new Label();
      this.removeBtn = new Button();
      this.findFieldBtn = new Button();
      this.addBtn = new Button();
      this.listViewFields = new ListView();
      this.headerID = new ColumnHeader();
      this.headerDescription = new ColumnHeader();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.findBtn = new Button();
      this.groupBox1 = new GroupBox();
      this.resetBtn = new Button();
      this.tabControl1.SuspendLayout();
      this.tabRule.SuspendLayout();
      this.panelDropdown.SuspendLayout();
      this.panelCodes.SuspendLayout();
      this.panelRange.SuspendLayout();
      this.tabRequired.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field ID";
      this.textBoxFieldID.BackColor = SystemColors.Window;
      this.textBoxFieldID.Location = new Point(84, 20);
      this.textBoxFieldID.Name = "textBoxFieldID";
      this.textBoxFieldID.Size = new Size(184, 20);
      this.textBoxFieldID.TabIndex = 1;
      this.textBoxFieldID.Leave += new EventHandler(this.textBoxFieldID_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description";
      this.textBoxDescription.Location = new Point(84, 48);
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.Size = new Size(184, 20);
      this.textBoxDescription.TabIndex = 1;
      this.textBoxDescription.TabStop = false;
      this.tabControl1.Controls.Add((Control) this.tabRule);
      this.tabControl1.Controls.Add((Control) this.tabRequired);
      this.tabControl1.Location = new Point(8, 88);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(456, 288);
      this.tabControl1.TabIndex = 4;
      this.tabRule.Controls.Add((Control) this.panelDropdown);
      this.tabRule.Controls.Add((Control) this.panelCodes);
      this.tabRule.Controls.Add((Control) this.comboBoxType);
      this.tabRule.Controls.Add((Control) this.label3);
      this.tabRule.Controls.Add((Control) this.panelRange);
      this.tabRule.Location = new Point(4, 22);
      this.tabRule.Name = "tabRule";
      this.tabRule.Size = new Size(448, 262);
      this.tabRule.TabIndex = 0;
      this.tabRule.Text = "Value Rule";
      this.panelDropdown.Controls.Add((Control) this.resetBtn);
      this.panelDropdown.Controls.Add((Control) this.insertBtn);
      this.panelDropdown.Controls.Add((Control) this.label6);
      this.panelDropdown.Controls.Add((Control) this.deleteBtn);
      this.panelDropdown.Controls.Add((Control) this.downBtn);
      this.panelDropdown.Controls.Add((Control) this.upBtn);
      this.panelDropdown.Controls.Add((Control) this.newBtn);
      this.panelDropdown.Controls.Add((Control) this.listBoxOptions);
      this.panelDropdown.Location = new Point(32, 55);
      this.panelDropdown.Name = "panelDropdown";
      this.panelDropdown.Size = new Size(428, 197);
      this.panelDropdown.TabIndex = 4;
      this.insertBtn.Location = new Point(272, 52);
      this.insertBtn.Name = "insertBtn";
      this.insertBtn.Size = new Size(56, 23);
      this.insertBtn.TabIndex = 21;
      this.insertBtn.Text = "Insert";
      this.insertBtn.Click += new EventHandler(this.insertBtn_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 8);
      this.label6.Name = "label6";
      this.label6.Size = new Size(151, 13);
      this.label6.TabIndex = 20;
      this.label6.Text = "Create options (100 maximum).";
      this.deleteBtn.Location = new Point(272, 133);
      this.deleteBtn.Name = "deleteBtn";
      this.deleteBtn.Size = new Size(56, 23);
      this.deleteBtn.TabIndex = 19;
      this.deleteBtn.Text = "Delete";
      this.deleteBtn.Click += new EventHandler(this.deleteBtn_Click);
      this.downBtn.Location = new Point(272, 106);
      this.downBtn.Name = "downBtn";
      this.downBtn.Size = new Size(56, 23);
      this.downBtn.TabIndex = 18;
      this.downBtn.Text = "Down";
      this.downBtn.Click += new EventHandler(this.downBtn_Click);
      this.upBtn.Location = new Point(272, 79);
      this.upBtn.Name = "upBtn";
      this.upBtn.Size = new Size(56, 23);
      this.upBtn.TabIndex = 17;
      this.upBtn.Text = "Up";
      this.upBtn.Click += new EventHandler(this.upBtn_Click);
      this.newBtn.Location = new Point(272, 25);
      this.newBtn.Name = "newBtn";
      this.newBtn.Size = new Size(56, 23);
      this.newBtn.TabIndex = 16;
      this.newBtn.Text = "New";
      this.newBtn.Click += new EventHandler(this.newBtn_Click);
      this.listBoxOptions.Location = new Point(8, 25);
      this.listBoxOptions.Name = "listBoxOptions";
      this.listBoxOptions.Size = new Size(252, 160);
      this.listBoxOptions.TabIndex = 5;
      this.panelCodes.Controls.Add((Control) this.label7);
      this.panelCodes.Controls.Add((Control) this.textBoxCodes);
      this.panelCodes.Location = new Point(57, 55);
      this.panelCodes.Name = "panelCodes";
      this.panelCodes.Size = new Size(428, 197);
      this.panelCodes.TabIndex = 5;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 8);
      this.label7.Name = "label7";
      this.label7.Size = new Size(224, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Write your own code using Visual Basic .NET.";
      this.textBoxCodes.AcceptsReturn = true;
      this.textBoxCodes.AcceptsTab = true;
      this.textBoxCodes.Location = new Point(8, 28);
      this.textBoxCodes.Multiline = true;
      this.textBoxCodes.Name = "textBoxCodes";
      this.textBoxCodes.ScrollBars = ScrollBars.Both;
      this.textBoxCodes.Size = new Size(400, 156);
      this.textBoxCodes.TabIndex = 6;
      this.comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxType.Items.AddRange(new object[3]
      {
        (object) "Range",
        (object) "Dropdown List",
        (object) "Editable Dropdown List"
      });
      this.comboBoxType.Location = new Point(20, 28);
      this.comboBoxType.Name = "comboBoxType";
      this.comboBoxType.Size = new Size(164, 21);
      this.comboBoxType.TabIndex = 2;
      this.comboBoxType.SelectedIndexChanged += new EventHandler(this.comboBoxType_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(16, 12);
      this.label3.Name = "label3";
      this.label3.Size = new Size(89, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Select a rule type";
      this.panelRange.Controls.Add((Control) this.textBoxUpper);
      this.panelRange.Controls.Add((Control) this.textBoxLower);
      this.panelRange.Controls.Add((Control) this.label5);
      this.panelRange.Controls.Add((Control) this.label4);
      this.panelRange.Location = new Point(12, 55);
      this.panelRange.Name = "panelRange";
      this.panelRange.Size = new Size(428, 197);
      this.panelRange.TabIndex = 3;
      this.textBoxUpper.Location = new Point(220, 28);
      this.textBoxUpper.Name = "textBoxUpper";
      this.textBoxUpper.Size = new Size(196, 20);
      this.textBoxUpper.TabIndex = 4;
      this.textBoxUpper.KeyUp += new KeyEventHandler(this.textFieldValue_KeyUp);
      this.textBoxLower.Location = new Point(12, 28);
      this.textBoxLower.Name = "textBoxLower";
      this.textBoxLower.Size = new Size(188, 20);
      this.textBoxLower.TabIndex = 3;
      this.textBoxLower.KeyUp += new KeyEventHandler(this.textFieldValue_KeyUp);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(216, 12);
      this.label5.Name = "label5";
      this.label5.Size = new Size(51, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Maximum";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 12);
      this.label4.Name = "label4";
      this.label4.Size = new Size(48, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Minimum";
      this.tabRequired.Controls.Add((Control) this.label8);
      this.tabRequired.Controls.Add((Control) this.removeBtn);
      this.tabRequired.Controls.Add((Control) this.findFieldBtn);
      this.tabRequired.Controls.Add((Control) this.addBtn);
      this.tabRequired.Controls.Add((Control) this.listViewFields);
      this.tabRequired.Location = new Point(4, 22);
      this.tabRequired.Name = "tabRequired";
      this.tabRequired.Size = new Size(448, 262);
      this.tabRequired.TabIndex = 1;
      this.tabRequired.Text = "Pre-Required Fields";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(11, 8);
      this.label8.Name = "label8";
      this.label8.Size = new Size(277, 13);
      this.label8.TabIndex = 35;
      this.label8.Text = "Add fields that must be completed prior to the field above.";
      this.removeBtn.Location = new Point(364, 85);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(75, 23);
      this.removeBtn.TabIndex = 34;
      this.removeBtn.Text = "Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.findFieldBtn.Location = new Point(364, 57);
      this.findFieldBtn.Name = "findFieldBtn";
      this.findFieldBtn.Size = new Size(75, 23);
      this.findFieldBtn.TabIndex = 33;
      this.findFieldBtn.Text = "Find";
      this.findFieldBtn.Click += new EventHandler(this.findFieldBtn_Click);
      this.addBtn.Location = new Point(364, 29);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 23);
      this.addBtn.TabIndex = 32;
      this.addBtn.Text = "Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.listViewFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listViewFields.Columns.AddRange(new ColumnHeader[2]
      {
        this.headerID,
        this.headerDescription
      });
      this.listViewFields.FullRowSelect = true;
      this.listViewFields.GridLines = true;
      this.listViewFields.HideSelection = false;
      this.listViewFields.Location = new Point(8, 28);
      this.listViewFields.MultiSelect = false;
      this.listViewFields.Name = "listViewFields";
      this.listViewFields.Size = new Size(348, 228);
      this.listViewFields.TabIndex = 31;
      this.listViewFields.UseCompatibleStateImageBehavior = false;
      this.listViewFields.View = View.Details;
      this.headerID.Text = "ID";
      this.headerID.Width = 113;
      this.headerDescription.Text = "Description";
      this.headerDescription.Width = 230;
      this.okBtn.Location = new Point(328, 408);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 7;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(408, 408);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "Cancel";
      this.findBtn.Location = new Point(276, 20);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(56, 21);
      this.findBtn.TabIndex = 15;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.textBoxDescription);
      this.groupBox1.Controls.Add((Control) this.findBtn);
      this.groupBox1.Controls.Add((Control) this.tabControl1);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.textBoxFieldID);
      this.groupBox1.Location = new Point(10, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(474, 384);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.resetBtn.Location = new Point(272, 161);
      this.resetBtn.Name = "resetBtn";
      this.resetBtn.Size = new Size(56, 23);
      this.resetBtn.TabIndex = 22;
      this.resetBtn.Text = "Reset";
      this.resetBtn.Click += new EventHandler(this.resetBtn_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(494, 444);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Field Rule";
      this.KeyDown += new KeyEventHandler(this.EditFieldDialog_KeyDown);
      this.tabControl1.ResumeLayout(false);
      this.tabRule.ResumeLayout(false);
      this.tabRule.PerformLayout();
      this.panelDropdown.ResumeLayout(false);
      this.panelDropdown.PerformLayout();
      this.panelCodes.ResumeLayout(false);
      this.panelCodes.PerformLayout();
      this.panelRange.ResumeLayout(false);
      this.panelRange.PerformLayout();
      this.tabRequired.ResumeLayout(false);
      this.tabRequired.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      if (this.useGeneralRule)
        this.comboBoxType.Items.Add((object) "Advanced Coding");
      this.listBoxOptions.Items.Clear();
      this.listViewFields.Items.Clear();
      this.textBoxFieldID.Text = this.fieldID;
      this.textBoxDescription.Text = this.fieldDescription;
      if (this.fieldID != string.Empty)
      {
        this.oldFieldID = this.fieldID;
        if (StandardFields.All.Contains(this.oldFieldID))
          this.oldFieldFormat = StandardFields.All[this.oldFieldID].Format;
        else if (this.fieldID.StartsWith("CX.") || this.fieldID.StartsWith("CUST") && this.fieldID.EndsWith("FV"))
        {
          CustomFieldInfo field = this.fieldSettings.CustomFields.GetField(this.fieldID);
          if (field != null)
          {
            this.oldFieldFormat = field.Format;
            if (field.Format == FieldFormat.AUDIT)
              this.oldFieldFormat = field.AuditSettings.AuditData != AuditData.Timestamp ? FieldFormat.STRING : FieldFormat.DATE;
            this.textBoxDescription.Text = field.Description;
          }
          else
            this.oldFieldFormat = FieldFormat.STRING;
        }
        else
          this.oldFieldFormat = FieldFormat.STRING;
        if (this.fieldID.StartsWith("BUTTON_"))
        {
          this.comboBoxType.Enabled = false;
          this.textBoxLower.Enabled = false;
          this.textBoxUpper.Enabled = false;
          this.comboBoxType.SelectedIndex = 0;
          this.textBoxDescription.Text = "Button";
        }
      }
      if (this.ruleObject == null)
      {
        this.comboBoxType.SelectedIndex = 0;
        this.textBoxUpper.Text = string.Empty;
        this.textBoxLower.Text = string.Empty;
        this.textBoxCodes.Text = string.Empty;
      }
      if (this.requiredFields != null)
      {
        for (int index = 0; index < this.requiredFields.Length; ++index)
          this.listViewFields.Items.Add(new ListViewItem(this.requiredFields[index])
          {
            SubItems = {
              this.getFieldDescription(this.requiredFields[index])
            }
          });
      }
      if (this.ruleObject != null)
      {
        if (this.ruleObject is FRRange)
        {
          FRRange ruleObject = (FRRange) this.ruleObject;
          this.textBoxUpper.Text = ruleObject.UpperBound;
          this.textBoxLower.Text = ruleObject.LowerBound;
          this.comboBoxType.SelectedIndex = 0;
        }
        else if (this.ruleObject is FRList)
        {
          FRList ruleObject = (FRList) this.ruleObject;
          for (int index = 0; index < ruleObject.List.Length; ++index)
          {
            if (ruleObject.List[index] == "")
              this.listBoxOptions.Items.Add((object) "<Clear>");
            else
              this.listBoxOptions.Items.Add((object) ruleObject.List[index]);
          }
          if (ruleObject.IsLock)
            this.comboBoxType.SelectedIndex = 1;
          else
            this.comboBoxType.SelectedIndex = 2;
        }
        else if (this.ruleObject is string)
        {
          this.textBoxCodes.Text = this.ruleObject.ToString();
          if (this.comboBoxType.Items.Count == 4)
            this.comboBoxType.SelectedIndex = 3;
          else
            this.comboBoxType.SelectedIndex = 0;
        }
        else
          this.comboBoxType.SelectedIndex = 0;
      }
      if (((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(this.ruleName) && this.fieldID == "33")
      {
        this.resetBtn.Visible = true;
        this.textBoxFieldID.ReadOnly = true;
      }
      else
      {
        this.resetBtn.Visible = false;
        this.textBoxFieldID.ReadOnly = false;
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, (string) null, AddFieldOptions.AllowAnyField))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void addFields(string[] ids)
    {
      this.listViewFields.BeginUpdate();
      for (int index1 = 0; index1 < ids.Length; ++index1)
      {
        if (string.Compare(ids[index1], this.textBoxFieldID.Text.Trim(), true) == 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The required field '" + ids[index1] + "' will not be added to field list because this is current field for this rule.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          bool flag = false;
          for (int index2 = 0; index2 < this.listViewFields.Items.Count; ++index2)
          {
            if (string.Compare(this.listViewFields.Items[index2].Text, ids[index1], true) == 0)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The field list already contains field '" + ids[index1] + "'. This field will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            ListViewItem listViewItem = new ListViewItem(ids[index1]);
            listViewItem.SubItems.Add(this.getFieldDescription(ids[index1]));
            listViewItem.Selected = true;
            listViewItem.EnsureVisible();
            this.listViewFields.Items.Add(listViewItem);
          }
        }
      }
      this.listViewFields.EndUpdate();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.listViewFields.SelectedItems[0].Index;
        this.listViewFields.Items.Remove(this.listViewFields.SelectedItems[0]);
        if (this.listViewFields.Items.Count == 0)
          return;
        if (index > this.listViewFields.Items.Count - 1)
          this.listViewFields.Items[this.listViewFields.Items.Count - 1].Selected = true;
        else
          this.listViewFields.Items[index].Selected = true;
      }
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) null, true, string.Empty, true, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int length = ruleFindFieldDialog.SelectedRequiredFields.Length;
        if (length <= 0)
          return;
        this.textBoxFieldID.Text = ruleFindFieldDialog.SelectedRequiredFields[length - 1];
        this.textBoxFieldID_Leave((object) null, (EventArgs) null);
        this.textBoxFieldID.Focus();
      }
    }

    private void findFieldBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.listViewFields.Items.Count; ++index)
        arrayList.Add((object) this.listViewFields.Items[index].Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) arrayList.ToArray(typeof (string)), true, string.Empty, false, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.listViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]))
            this.listViewFields.Items.Add(new ListViewItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index])
              }
            });
        }
        this.listViewFields.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string upper = this.textBoxFieldID.Text.Trim().ToUpper();
      if (upper == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a field ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxFieldID.Focus();
      }
      else if (upper != this.fieldID && this.existingFields.Contains((object) upper))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Field ID '" + upper + "' is already in list. Please use a different field ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxFieldID.Focus();
      }
      else if (!upper.StartsWith("BUTTON_") && !EncompassFields.IsReportable(upper, this.fieldSettings))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Field ID '" + this.textBoxFieldID.Text + "' is not a standard field in Encompass. Please enter a valid field ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxFieldID.Focus();
      }
      else
      {
        CustomFieldInfo customFieldInfo = (CustomFieldInfo) null;
        if (upper.StartsWith("CX.") || upper.StartsWith("CUST") && upper.EndsWith("FV"))
        {
          customFieldInfo = this.fieldSettings.CustomFields.GetField(upper);
          if (customFieldInfo == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The custom field '" + upper + "' is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.textBoxFieldID.Focus();
            return;
          }
        }
        switch (this.comboBoxType.SelectedIndex)
        {
          case 0:
            if (customFieldInfo != null)
            {
              FieldFormat fieldFormat = customFieldInfo.Format;
              if (customFieldInfo.Format == FieldFormat.AUDIT)
                fieldFormat = customFieldInfo.AuditSettings.AuditData != AuditData.Timestamp ? FieldFormat.STRING : FieldFormat.DATE;
              if (!customFieldInfo.IsNumericValued() && fieldFormat != FieldFormat.DATE && (this.listViewFields.Items.Count == 0 || this.listViewFields.Items.Count > 0 && (this.textBoxLower.Text.Trim() != string.Empty || this.textBoxUpper.Text.Trim() != string.Empty)))
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The field range rule is only available for numeric- and date-valued field types.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.textBoxFieldID.Focus();
                return;
              }
            }
            else if (upper != "1663" && upper != "1665" && !EncompassFields.IsNumeric(upper) && EncompassFields.GetFormat(upper) != FieldFormat.DATE && (this.listViewFields.Items.Count == 0 || this.listViewFields.Items.Count > 0 && (this.textBoxLower.Text.Trim() != string.Empty || this.textBoxUpper.Text.Trim() != string.Empty)))
            {
              if (upper.StartsWith("BUTTON_") && this.listViewFields.Items.Count == 0)
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "The pre-required field cannot be blank for a button.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              else
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "The field range rule is only available for numeric- and date-valued field types.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              this.textBoxFieldID.Focus();
              return;
            }
            if (customFieldInfo != null && (customFieldInfo.Format == FieldFormat.DATE || customFieldInfo.Format == FieldFormat.AUDIT && customFieldInfo.AuditSettings.AuditData == AuditData.Timestamp) || EncompassFields.GetFormat(upper) == FieldFormat.DATE)
            {
              DateTime date1 = Utils.ParseDate((object) this.textBoxLower.Text.Trim());
              if (date1 == DateTime.MinValue && this.listViewFields.Items.Count == 0)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The minimum date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.textBoxLower.Focus();
                return;
              }
              this.textBoxLower.Text = Utils.DateFormat(date1);
              DateTime date2 = Utils.ParseDate((object) this.textBoxUpper.Text.Trim());
              if (date2 == DateTime.MinValue && this.listViewFields.Items.Count == 0)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The maximum date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.textBoxUpper.Focus();
                return;
              }
              this.textBoxUpper.Text = Utils.DateFormat(date2);
              if (date1 > date2)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The date range is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.textBoxLower.Focus();
                return;
              }
            }
            else
            {
              Decimal num3 = Utils.ParseDecimal((object) this.textBoxLower.Text.Trim());
              Decimal num4 = Utils.ParseDecimal((object) this.textBoxUpper.Text.Trim());
              if (num3 > num4)
              {
                int num5 = (int) Utils.Dialog((IWin32Window) this, "The field range is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.textBoxUpper.Focus();
                return;
              }
              if (this.listViewFields.Items.Count == 0 && num3 == 0M && num4 == 0M)
              {
                int num6 = (int) Utils.Dialog((IWin32Window) this, "The field range is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            this.ruleObject = (object) new FRRange(this.textBoxLower.Text.Trim(), this.textBoxUpper.Text.Trim());
            break;
          case 1:
          case 2:
            if (customFieldInfo == null && StandardFields.All.Contains(upper) && StandardFields.All[upper].Options != null)
            {
              FieldFormat format = EncompassFields.GetFormat(upper);
              switch (format)
              {
                case FieldFormat.YN:
                case FieldFormat.X:
                  int num7 = (int) Utils.Dialog((IWin32Window) this, "The field " + upper + " is a checkbox field. You can't override it.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.textBoxFieldID.Focus();
                  return;
                default:
                  if (StandardFields.All[upper].Options.Count > 0 && format != FieldFormat.STRING)
                  {
                    int num8 = (int) Utils.Dialog((IWin32Window) this, "The field " + upper + " is a dropdown field or checkbox. You can't override it.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.textBoxFieldID.Focus();
                    return;
                  }
                  break;
              }
            }
            if (this.listBoxOptions.Items.Count == 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The dropdown options cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            string[] list = new string[this.listBoxOptions.Items.Count];
            for (int index = 0; index < this.listBoxOptions.Items.Count; ++index)
              list[index] = !(this.listBoxOptions.Items[index].ToString().Trim() == "<Clear>") ? this.listBoxOptions.Items[index].ToString().Trim() : "";
            if (this.comboBoxType.SelectedIndex == 1)
            {
              this.ruleObject = (object) new FRList(list, BizRule.FieldRuleType.ListLock);
              break;
            }
            if (this.comboBoxType.SelectedIndex == 2)
            {
              this.ruleObject = (object) new FRList(list, BizRule.FieldRuleType.ListUnlock);
              break;
            }
            break;
          case 3:
            if (!this.validateAdvancedCode())
              return;
            this.ruleObject = (object) this.textBoxCodes.Text.Trim();
            break;
        }
        this.requiredFields = new string[this.listViewFields.Items.Count];
        for (int index = 0; index < this.listViewFields.Items.Count; ++index)
          this.requiredFields[index] = this.listViewFields.Items[index].Text.Trim();
        this.fieldID = this.textBoxFieldID.Text.Trim();
        this.fieldDescription = this.textBoxDescription.Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.listBoxOptions.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an option.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int selectedIndex = this.listBoxOptions.SelectedIndex;
        this.listBoxOptions.Items.RemoveAt(selectedIndex);
        if (this.listBoxOptions.Items.Count == 0)
          return;
        if (selectedIndex > this.listBoxOptions.Items.Count - 1)
          this.listBoxOptions.SelectedIndex = this.listBoxOptions.Items.Count - 1;
        else
          this.listBoxOptions.SelectedIndex = selectedIndex;
      }
    }

    private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.comboBoxType.SelectedIndex)
      {
        case 1:
        case 2:
          this.panelRange.Visible = false;
          this.panelCodes.Visible = false;
          this.panelDropdown.Visible = true;
          if (((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(this.ruleName) && this.textBoxFieldID.Text.Trim() == "33")
            this.resetBtn.Visible = true;
          else
            this.resetBtn.Visible = false;
          this.newBtn.Focus();
          break;
        case 3:
          this.panelDropdown.Visible = false;
          this.panelRange.Visible = false;
          this.panelCodes.Visible = true;
          this.textBoxCodes.Focus();
          break;
        default:
          this.panelDropdown.Visible = false;
          this.panelCodes.Visible = false;
          this.panelRange.Visible = true;
          this.textBoxLower.Focus();
          break;
      }
    }

    private void textBoxFieldID_Leave(object sender, EventArgs e)
    {
      CustomFieldInfo customFieldInfo = (CustomFieldInfo) null;
      string upper = this.textBoxFieldID.Text.ToUpper();
      if (upper.StartsWith("CX.") || upper.StartsWith("CUST") && upper.EndsWith("FV"))
      {
        customFieldInfo = this.fieldSettings.CustomFields.GetField(upper);
        if (customFieldInfo == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Field " + upper + " is not a valid field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.textBoxFieldID.Text = upper;
      this.textBoxDescription.Text = this.getFieldDescription(upper);
      this.comboBoxType.Enabled = true;
      this.textBoxLower.Enabled = true;
      this.textBoxUpper.Enabled = true;
      bool flag = false;
      FieldFormat fieldFormat = FieldFormat.STRING;
      if (StandardFields.All.Contains(upper) || customFieldInfo != null)
      {
        if (customFieldInfo != null)
        {
          fieldFormat = customFieldInfo.Format;
          if (customFieldInfo.Format == FieldFormat.AUDIT)
            fieldFormat = customFieldInfo.AuditSettings.AuditData != AuditData.Timestamp ? FieldFormat.STRING : FieldFormat.DATE;
        }
        else
          fieldFormat = StandardFields.All[upper].Format;
        if (this.oldFieldFormat == FieldFormat.NONE)
        {
          this.oldFieldID = upper;
          this.oldFieldFormat = fieldFormat;
          return;
        }
        if (upper == this.oldFieldID)
          return;
        if (fieldFormat == FieldFormat.ZIPCODE && this.oldFieldFormat != FieldFormat.ZIPCODE)
          flag = true;
        if (fieldFormat == FieldFormat.YN && this.oldFieldFormat != FieldFormat.YN)
          flag = true;
        if (fieldFormat == FieldFormat.X && this.oldFieldFormat != FieldFormat.X)
          flag = true;
        if (fieldFormat == FieldFormat.STATE && this.oldFieldFormat != FieldFormat.STATE)
          flag = true;
        if (fieldFormat == FieldFormat.SSN && this.oldFieldFormat != FieldFormat.SSN)
          flag = true;
        if (fieldFormat == FieldFormat.PHONE && this.oldFieldFormat != FieldFormat.PHONE)
          flag = true;
        if (fieldFormat == FieldFormat.MONTHDAY && this.oldFieldFormat != FieldFormat.MONTHDAY)
          flag = true;
        if (fieldFormat == FieldFormat.DATE && this.oldFieldFormat != FieldFormat.DATE)
          flag = true;
        if (fieldFormat == FieldFormat.AUDIT && this.oldFieldFormat != FieldFormat.AUDIT)
          flag = true;
        if (this.oldFieldID != "")
        {
          if (customFieldInfo != null)
          {
            if (this.oldFieldID.StartsWith("CX.") || this.oldFieldID.StartsWith("CUST") && this.oldFieldID.EndsWith("FV"))
            {
              CustomFieldInfo field = this.fieldSettings.CustomFields.GetField(this.oldFieldID);
              if (field != null && customFieldInfo.IsNumericValued() && !field.IsNumericValued())
                flag = true;
            }
            else if (customFieldInfo.IsNumericValued() && !StandardFields.All[this.oldFieldID].IsNumeric())
              flag = true;
          }
          else if (this.oldFieldID.StartsWith("CX.") || this.oldFieldID.StartsWith("CUST") && this.oldFieldID.EndsWith("FV"))
          {
            CustomFieldInfo field = this.fieldSettings.CustomFields.GetField(this.oldFieldID);
            if (field != null && StandardFields.All[upper].IsNumeric() && !field.IsNumericValued())
              flag = true;
          }
          else if (StandardFields.All.Contains(this.oldFieldID))
          {
            if (StandardFields.All[upper].IsNumeric() && !StandardFields.All[this.oldFieldID].IsNumeric())
              flag = true;
          }
          else
            flag = true;
        }
        if (flag && (this.comboBoxType.Text.IndexOf("Dropdown List") > -1 && this.listBoxOptions.Items.Count > 0 || this.comboBoxType.Text == "Range" && (this.textBoxLower.Text != "" || this.textBoxUpper.Text != "")))
        {
          if (Utils.Dialog((IWin32Window) this, "You have changed field format. All options will be cleared. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          {
            this.textBoxFieldID.Text = this.oldFieldID;
            return;
          }
          this.listBoxOptions.Items.Clear();
          this.textBoxLower.Text = "";
          this.textBoxUpper.Text = "";
        }
      }
      else if (upper.StartsWith("BUTTON_"))
      {
        this.comboBoxType.Enabled = false;
        this.textBoxLower.Enabled = false;
        this.textBoxUpper.Enabled = false;
        this.textBoxUpper.Text = this.textBoxLower.Text = string.Empty;
        this.comboBoxType.SelectedIndex = 0;
      }
      this.oldFieldFormat = fieldFormat;
      this.oldFieldID = upper;
      if (((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(this.ruleName) && this.fieldID == "33")
        this.resetBtn.Visible = true;
      else
        this.resetBtn.Visible = false;
    }

    private void textFieldValue_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, this.oldFieldFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void textFieldValue_Leave(object sender, EventArgs e)
    {
      if (this.oldFieldFormat != FieldFormat.DATE || !(sender is TextBox textBox))
        return;
      DateTime date = Utils.ParseDate((object) textBox.Text, false, DateTime.MinValue);
      if (!(date != DateTime.MinValue))
        return;
      textBox.Text = Utils.DateFormat(date);
    }

    private void upBtn_Click(object sender, EventArgs e)
    {
      if (this.listBoxOptions.SelectedIndices.Count == 0)
        return;
      int selectedIndex = this.listBoxOptions.SelectedIndex;
      int index = selectedIndex - 1;
      if (index < 0)
        return;
      string str = this.listBoxOptions.Items[selectedIndex].ToString();
      this.listBoxOptions.Items.RemoveAt(selectedIndex);
      this.listBoxOptions.Items.Insert(index, (object) str);
      this.listBoxOptions.SelectedIndex = index;
    }

    private void downBtn_Click(object sender, EventArgs e)
    {
      if (this.listBoxOptions.SelectedIndices.Count == 0)
        return;
      int selectedIndex = this.listBoxOptions.SelectedIndex;
      int index = selectedIndex + 1;
      if (index >= this.listBoxOptions.Items.Count)
        return;
      string str = this.listBoxOptions.Items[selectedIndex].ToString();
      this.listBoxOptions.Items.RemoveAt(selectedIndex);
      this.listBoxOptions.Items.Insert(index, (object) str);
      this.listBoxOptions.SelectedIndex = index;
    }

    private string getFieldDescription(string fieldID)
    {
      if (fieldID.StartsWith("CX.") || fieldID.StartsWith("CUST") && fieldID.EndsWith("FV"))
      {
        if (this.fieldSettings != null)
        {
          CustomFieldInfo field = this.fieldSettings.CustomFields.GetField(fieldID);
          if (field != null && field.Description != "")
            return field.Description;
        }
      }
      else if (fieldID.StartsWith("BUTTON_"))
        return "Button";
      return EncompassFields.GetDescription(fieldID);
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
    }

    private void insertBtn_Click(object sender, EventArgs e)
    {
      if (this.listBoxOptions.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an option before insert.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.addNewOption(this.listBoxOptions.SelectedIndex);
    }

    private void newBtn_Click(object sender, EventArgs e) => this.addNewOption(-1);

    private void addNewOption(int insertPos)
    {
      if (this.listBoxOptions.Items.Count >= 100)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You already have reached maximum options.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string fieldId = this.textBoxFieldID.Text.Trim();
        if (fieldId == "")
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to setup field ID for business rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          FieldFormat oldFieldFormat = this.oldFieldFormat;
          string[] values = new string[this.listBoxOptions.Items.Count];
          for (int index = 0; index < this.listBoxOptions.Items.Count; ++index)
            values[index] = this.listBoxOptions.Items[index].ToString();
          using (EditDropdownDialog editDropdownDialog = new EditDropdownDialog(values, oldFieldFormat))
          {
            if (editDropdownDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            string str = editDropdownDialog.NewOption.Trim();
            if (str == "<Clear>")
            {
              this.listBoxOptions.Items.Insert(0, (object) str);
            }
            else
            {
              if (oldFieldFormat != FieldFormat.STRING)
              {
                str = Utils.ApplyFieldFormatting(str, oldFieldFormat);
                if (EncompassFields.IsNumeric(fieldId))
                  str = str.Replace(",", "");
              }
              if (insertPos == -1)
              {
                this.listBoxOptions.Items.Add((object) str);
                this.listBoxOptions.SelectedIndex = this.listBoxOptions.Items.Count - 1;
              }
              else
              {
                this.listBoxOptions.Items.Insert(insertPos + 1, (object) str);
                this.listBoxOptions.SelectedIndex = insertPos + 1;
              }
            }
          }
        }
      }
    }

    private bool validateAdvancedCode()
    {
      if (this.textBoxCodes.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Advanced Coding field cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxCodes.Focus();
        return false;
      }
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeFieldRule("", "", (RuleCondition) PredefinedCondition.Empty, this.textBoxCodes.Text, new string[0], FieldFormat.STRING).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        CompilerError error = ex.Errors[0];
        if (error.LineIndexOfRegion >= 0)
          Utils.HighlightLine(this.textBoxCodes, error.LineIndexOfRegion, false);
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: " + error.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void EditFieldDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.cancelBtn.PerformClick();
    }

    private void resetBtn_Click(object sender, EventArgs e)
    {
      if (this.listBoxOptions == null)
        return;
      this.listBoxOptions.Items.Clear();
      foreach (object field33Option in this.field33Options)
        this.listBoxOptions.Items.Add(field33Option);
    }
  }
}
