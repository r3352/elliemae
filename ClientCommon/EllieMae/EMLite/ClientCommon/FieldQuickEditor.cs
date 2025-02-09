// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.FieldQuickEditor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class FieldQuickEditor : UserControl
  {
    public static Color ExistingFieldColor = Color.FromArgb(200, 227, 254);
    public static Color SelectedFieldColor = Color.FromArgb(254, 232, 208);
    private LoanData loan;
    private string[] requiredFields;
    private FieldQuickEditorMode editorMode = FieldQuickEditorMode.Other;
    private Hashtable dataTable;
    private bool readOnly;
    private Control lastField;
    private FieldSettings fieldSettings;
    private Sessions.Session session;
    private bool showGoToField = true;
    private IContainer components;
    private Panel panelFields;
    private ToolTip toolTipField;

    public FieldQuickEditor(Sessions.Session session)
      : this(session, (LoanData) null, FieldQuickEditorMode.Other, false)
    {
    }

    public FieldQuickEditor(
      Sessions.Session session,
      Hashtable dataTable,
      FieldQuickEditorMode editorMode)
      : this(session, (LoanData) null, editorMode, true)
    {
      this.dataTable = dataTable;
    }

    public FieldQuickEditor(
      Sessions.Session session,
      LoanData loan,
      FieldQuickEditorMode editorMode,
      bool readOnly)
    {
      this.session = session;
      this.loan = loan;
      this.editorMode = editorMode;
      this.readOnly = readOnly;
      this.fieldSettings = this.loan == null ? this.session.LoanManager.GetFieldSettings() : loan.Settings.FieldSettings;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
    }

    public void RefreshFieldList(string[] requiredFields, bool displayEmptyFieldOnly)
    {
      this.showGoToField = true;
      this.lastField = (Control) null;
      this.panelFields.Controls.Clear();
      this.requiredFields = requiredFields;
      if (this.requiredFields == null || this.requiredFields.Length == 0)
      {
        this.showGoToField = false;
      }
      else
      {
        int width1 = (int) ((double) this.panelFields.Width * 0.4);
        int x = width1 + 5;
        int width2 = this.Width - 7 - width1 - 5 - 10;
        if (width2 <= 0)
          width2 = 30;
        string empty1 = string.Empty;
        int num1 = 0;
        string empty2 = string.Empty;
        int num2 = 0;
        bool flag1 = true;
        for (int index = 0; index < this.requiredFields.Length; ++index)
        {
          string upper = this.requiredFields[index].ToUpper();
          FieldDefinition field1 = EncompassFields.GetField(upper, this.fieldSettings);
          if (field1 == null)
          {
            ++num2;
          }
          else
          {
            QuickFieldDef quickDef = new QuickFieldDef(this.requiredFields[index], field1);
            string field2 = this.getField(quickDef.OriginalID);
            if (!displayEmptyFieldOnly || !(field2 != string.Empty) || !(field2 != "//"))
            {
              if (!VirtualFields.Contains(upper))
                flag1 = false;
              ++num1;
              int y1 = this.lastField != null ? this.lastField.Top + this.lastField.Height + 2 : 10;
              bool flag2 = this.isFieldEditable(quickDef);
              if ((field1.Format == FieldFormat.YN && (field1.Options == null || field1.Options.Count == 0) || field1.Format == FieldFormat.X) && this.loan != null && !this.loan.IsInFindFieldForm)
              {
                CheckBox checkBox = new CheckBox();
                checkBox.Location = new Point(x, y1);
                checkBox.Name = "c_" + quickDef.OriginalID;
                checkBox.Size = new Size(20, 20);
                checkBox.TabIndex = num1 - 1;
                checkBox.Text = "";
                checkBox.Tag = (object) quickDef;
                string field3 = this.getField(quickDef.OriginalID);
                checkBox.Checked = field3 == "Y" || field3 == "X";
                if (this.loan.IsInFindFieldForm)
                {
                  checkBox.MouseDown += new MouseEventHandler(this.field_MouseDown);
                  checkBox.ContextMenu = new ContextMenu();
                }
                else
                  checkBox.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
                checkBox.Enter += new EventHandler(this.field_Enter);
                checkBox.Enabled = flag2;
                if (!checkBox.Enabled)
                  checkBox.BackColor = Color.WhiteSmoke;
                this.panelFields.Controls.Add((Control) checkBox);
                this.toolTipField.SetToolTip((Control) checkBox, this.getUIFieldID(quickDef.OriginalID));
                this.lastField = (Control) checkBox;
              }
              else if (field1.Options != null && field1.Options.Count > 0 && this.loan != null && !this.loan.IsInFindFieldForm)
              {
                ComboBox comboBox = new ComboBox();
                comboBox.FormattingEnabled = true;
                comboBox.Location = new Point(x, y1);
                comboBox.Name = "l_" + quickDef.OriginalID;
                comboBox.Size = new Size(width2, 22);
                comboBox.TabIndex = num1 - 1;
                comboBox.Items.Add((object) FieldOption.Empty);
                comboBox.DropDownStyle = field1.Format != FieldFormat.DROPDOWN ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
                if (quickDef.OriginalID == "2626")
                {
                  string settingFromCache = this.session.SessionObjects.GetCompanySettingFromCache("CHANNELOPTION", "DISPLAY");
                  if ((settingFromCache ?? "") != string.Empty)
                  {
                    int num3 = 0;
                    foreach (FieldOption option in field1.Options)
                    {
                      if (settingFromCache.ToLower().IndexOf(num3.ToString()) > -1)
                        comboBox.Items.Add((object) option);
                      ++num3;
                    }
                  }
                  else
                  {
                    foreach (FieldOption option in field1.Options)
                      comboBox.Items.Add((object) option);
                  }
                }
                else
                {
                  foreach (FieldOption option in field1.Options)
                    comboBox.Items.Add((object) option);
                }
                comboBox.Text = field1.ToDisplayValue(this.getField(quickDef.OriginalID));
                comboBox.Enabled = flag2;
                comboBox.MouseDown += new MouseEventHandler(this.field_MouseDown);
                if (this.loan.IsInFindFieldForm)
                  comboBox.ContextMenu = new ContextMenu();
                else
                  comboBox.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
                comboBox.Enter += new EventHandler(this.field_Enter);
                comboBox.Tag = (object) quickDef;
                this.panelFields.Controls.Add((Control) comboBox);
                this.toolTipField.SetToolTip((Control) comboBox, this.getUIFieldID(quickDef.OriginalID));
                this.lastField = (Control) comboBox;
              }
              else
              {
                TextBox box = new TextBox();
                box.Location = new Point(x, y1);
                box.Name = "b_" + quickDef.OriginalID;
                if ((field1.Format == FieldFormat.YN || field1.Format == FieldFormat.X) && this.loan != null && this.loan.IsInFindFieldForm)
                  box.Size = new Size(20, 20);
                else
                  box.Size = new Size(width2, 20);
                box.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
                box.TabIndex = num1 - 1;
                box.Text = this.getField(quickDef.OriginalID);
                box.ReadOnly = !flag2 || this.loan.IsInFindFieldForm;
                if (box.ReadOnly)
                  box.BackColor = Color.WhiteSmoke;
                if (upper.StartsWith("CX.") || upper.StartsWith("CUST") && upper.EndsWith("FV"))
                  box.MaxLength = field1.MaxLength;
                if (this.loan != null && this.loan.IsInFindFieldForm)
                {
                  box.MouseDown += new MouseEventHandler(this.field_MouseDown);
                  box.ContextMenu = new ContextMenu();
                }
                else
                {
                  if (field1.IsNumeric())
                  {
                    box.TextAlign = HorizontalAlignment.Right;
                    box.KeyPress += new KeyPressEventHandler(this.numericField_KeyPress);
                  }
                  else if (field1.IsDateValued())
                    box.KeyDown += new KeyEventHandler(this.dateField_KeyDown);
                  box.KeyUp += new KeyEventHandler(this.field_KeyUp);
                  box.Leave += new EventHandler(this.field_Leave);
                }
                box.Enter += new EventHandler(this.field_Enter);
                box.Tag = (object) quickDef;
                if ((quickDef.OriginalID == "1401" || quickDef.OriginalID == "LR.1401" || quickDef.OriginalID == "1785" || quickDef.OriginalID == "LR.1785") && this.loan != null && !this.loan.IsFieldReadOnly(quickDef.OriginalID))
                {
                  box.Width -= 16;
                  this.panelFields.Controls.Add((Control) this.createSearchButton(box, quickDef.OriginalID));
                }
                this.panelFields.Controls.Add((Control) box);
                this.toolTipField.SetToolTip((Control) box, this.getUIFieldID(quickDef.OriginalID));
                this.lastField = (Control) box;
              }
              if (this.loan != null && this.loan.IsInFindFieldForm && this.loan.SelectedFieldType(this.getUIFieldID(quickDef.OriginalID)) == LoanData.FindFieldTypes.Existing)
                this.lastField.BackColor = FieldQuickEditor.ExistingFieldColor;
              int y2 = this.lastField.Top + (this.lastField.Height - 13) / 2;
              Label label = new Label();
              label.AutoSize = false;
              label.BackColor = Color.Transparent;
              label.ForeColor = Color.Black;
              label.Location = new Point(10, y2);
              label.Name = "l_" + quickDef.OriginalID;
              label.Size = new Size(width1, 13);
              label.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
              label.Text = quickDef.FieldDef.Description;
              label.Tag = (object) quickDef;
              this.panelFields.Controls.Add((Control) label);
              this.toolTipField.SetToolTip((Control) label, field1.Description);
            }
          }
        }
        if (num1 == 0 && this.requiredFields != null && this.requiredFields.Length != 0 && num2 != this.requiredFields.Length)
        {
          Label label = new Label();
          label.AutoSize = true;
          label.BackColor = Color.Transparent;
          label.ForeColor = Color.Black;
          label.Location = new Point(3, 5);
          label.Name = "l_noAnyMissingField";
          label.Text = "All required fields have been completed!";
          this.panelFields.Controls.Add((Control) label);
        }
        if (!flag1 && (this.requiredFields == null || this.requiredFields.Length == 0 || num2 != this.requiredFields.Length))
          return;
        this.showGoToField = false;
      }
    }

    public bool ShowGoToField => this.showGoToField;

    private void field_MouseDown(object sender, MouseEventArgs e)
    {
      if (!this.loan.IsInFindFieldForm && sender is ComboBox)
      {
        this.Paint -= new PaintEventHandler(this.FieldQuickEditor_Paint);
      }
      else
      {
        if (e.Button != MouseButtons.Right)
          return;
        Control control = (Control) sender;
        if (control.BackColor == FieldQuickEditor.ExistingFieldColor)
          return;
        string customFieldId = LockRequestCustomField.GenerateCustomFieldID(((QuickFieldDef) control.Tag).OriginalID);
        switch (this.loan.SelectedFieldType(customFieldId))
        {
          case LoanData.FindFieldTypes.None:
            this.loan.AddSelectedField(customFieldId);
            control.BackColor = FieldQuickEditor.SelectedFieldColor;
            break;
          case LoanData.FindFieldTypes.NewSelect:
            this.loan.RemoveSelectedField(customFieldId);
            control.BackColor = Color.WhiteSmoke;
            break;
        }
      }
    }

    private StandardIconButton createSearchButton(TextBox box, string originalFieldID)
    {
      StandardIconButton searchButton = new StandardIconButton();
      searchButton.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      searchButton.Name = box.Name + "_search";
      switch (originalFieldID)
      {
        case "1401":
        case "LR.1401":
          searchButton.Click += new EventHandler(this.loanProgramSearch_Click);
          break;
        case "1785":
          searchButton.Click += new EventHandler(this.closingCostSearch_Click);
          break;
      }
      searchButton.Location = new Point(box.Left + box.Width + 3, box.Top + 2);
      searchButton.Size = new Size(16, 16);
      return searchButton;
    }

    public int MaxHeight
    {
      get => this.lastField != null ? this.lastField.Top + this.lastField.Height + 3 : this.Height;
    }

    private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      QuickFieldDef tag = (QuickFieldDef) comboBox.Tag;
      this.setField(tag.OriginalID, tag.FieldDef.Options.TextToValue(comboBox.Text.Trim()));
      comboBox.Text = tag.FieldDef.ToDisplayValue(this.getField(tag.OriginalID));
      this.Paint += new PaintEventHandler(this.FieldQuickEditor_Paint);
    }

    private void panelFields_Resize(object sender, EventArgs e)
    {
      int num1 = (int) ((double) this.panelFields.Width * 0.4);
      int num2 = (int) ((double) this.panelFields.Width * 0.55);
      int num3 = num1 + 3 + 2;
      foreach (Control control in (ArrangedElementCollection) this.panelFields.Controls)
      {
        switch (control)
        {
          case Label _:
            control.Width = num1;
            continue;
          case TextBox _:
          case ComboBox _:
            control.Left = num3;
            QuickFieldDef tag = (QuickFieldDef) control.Tag;
            if (tag.FieldDef.Format != FieldFormat.YN && tag.FieldDef.Format != FieldFormat.X || this.loan == null || !this.loan.IsInFindFieldForm)
            {
              control.Width = num2;
              continue;
            }
            continue;
          case CheckBox _:
            control.Left = num3;
            continue;
          case StandardIconButton _:
            control.Left = num3 + num2 + 2;
            continue;
          default:
            continue;
        }
      }
    }

    private void field_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      QuickFieldDef tag = (QuickFieldDef) textBox.Tag;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, tag.FieldDef.Format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void field_Enter(object sender, EventArgs e)
    {
      QuickFieldDef quickFieldDef = (QuickFieldDef) null;
      switch (sender)
      {
        case TextBox _:
          quickFieldDef = (QuickFieldDef) ((Control) sender).Tag;
          break;
        case CheckBox _:
          quickFieldDef = (QuickFieldDef) ((Control) sender).Tag;
          break;
        case ComboBox _:
          quickFieldDef = (QuickFieldDef) ((Control) sender).Tag;
          break;
      }
      if (quickFieldDef == null)
        return;
      this.displayFieldId(quickFieldDef.OriginalID);
    }

    private void field_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.ReadOnly)
        return;
      QuickFieldDef tag = (QuickFieldDef) textBox.Tag;
      this.setField(tag.OriginalID, textBox.Text.Trim());
      textBox.Text = this.getField(tag.OriginalID);
    }

    private void dateField_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.D)
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox.ReadOnly)
        return;
      textBox.Text = DateTime.Today.ToString("MM/dd/yyyy");
      QuickFieldDef tag = (QuickFieldDef) textBox.Tag;
      this.setField(tag.OriginalID, textBox.Text.Trim());
      textBox.Text = this.getField(tag.OriginalID);
    }

    private void numericField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void checkBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      if (!checkBox.Enabled)
        return;
      QuickFieldDef tag = (QuickFieldDef) checkBox.Tag;
      string empty = string.Empty;
      string val = tag.FieldDef.Format != FieldFormat.X ? (checkBox.Checked ? "Y" : "N") : (checkBox.Checked ? "X" : "");
      this.setField(tag.OriginalID, val);
    }

    private void setField(string id, string val)
    {
      if (this.loan == null)
        return;
      id = this.getRealFieldID(id);
      if (this.session.LoanDataMgr != null && this.session.LoanDataMgr.GetFieldAccessRights(id) == BizRule.FieldAccessRight.Hide)
        return;
      try
      {
        this.loan.SetFieldFromCal(id, val);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private string getField(string id)
    {
      id = this.getRealFieldID(id);
      if (this.loan != null && !this.loan.IsInFindFieldForm)
        return this.loan.GetField(id);
      return this.dataTable != null && this.dataTable.ContainsKey((object) id) ? this.dataTable[(object) id].ToString() : string.Empty;
    }

    private string getRealFieldID(string id)
    {
      return this.editorMode == FieldQuickEditorMode.ForRequest || this.editorMode == FieldQuickEditorMode.ForRequestSnapshot ? LockRequestCustomField.GenerateCustomFieldID(id) : id;
    }

    private string getUIFieldID(string id)
    {
      if (this.editorMode == FieldQuickEditorMode.ForRequest)
        return LockRequestCustomField.GenerateCustomFieldID(id);
      if (this.editorMode == FieldQuickEditorMode.ForLoanSnapshot)
        return RateLockField.GenerateRateLockFieldID(id, RateLockField.RateLockOrder.MostRecent);
      return this.editorMode == FieldQuickEditorMode.ForRequestSnapshot ? LockRequestCustomField.GenerateCustomFieldID(id) : id;
    }

    public bool VerifyRequiredFields()
    {
      if (this.requiredFields == null || this.requiredFields.Length == 0)
        return true;
      string empty = string.Empty;
      for (int index = 0; index < this.requiredFields.Length; ++index)
      {
        string field = this.getField(this.requiredFields[index]);
        if (field == string.Empty || field == "//")
          return false;
      }
      return true;
    }

    private bool isFieldEditable(QuickFieldDef quickDef)
    {
      if (this.readOnly || this.loan == null)
        return false;
      string originalId = quickDef.OriginalID;
      if (originalId.StartsWith("CX.") || originalId.StartsWith("CUST") && originalId.EndsWith("FV"))
      {
        CustomFieldInfo field = this.fieldSettings.CustomFields.GetField(originalId);
        if (field.IsAuditField() || field.Calculation != string.Empty)
          return false;
      }
      if (!quickDef.FieldDef.AllowEdit || this.session.LoanDataMgr != null && this.session.LoanDataMgr.GetFieldAccessRights(originalId) == BizRule.FieldAccessRight.Hide)
        return false;
      string uiFieldId = this.getUIFieldID(originalId);
      return !this.loan.IsFieldReadOnly(uiFieldId) && (this.session.LoanDataMgr == null || this.session.LoanDataMgr.GetFieldAccessRights(uiFieldId) != BizRule.FieldAccessRight.Hide);
    }

    public void RefreshFieldValues(Hashtable dataTable, FieldQuickEditorMode editorMode)
    {
      this.dataTable = dataTable;
      this.editorMode = editorMode;
      this.FieldQuickEditor_Paint((object) null, (PaintEventArgs) null);
    }

    private void FieldQuickEditor_Paint(object sender, PaintEventArgs e)
    {
      foreach (Control control in (ArrangedElementCollection) this.panelFields.Controls)
      {
        QuickFieldDef tag = (QuickFieldDef) control.Tag;
        if (tag != null)
        {
          string str = this.getField(tag.OriginalID);
          if (this.session.LoanDataMgr != null && this.session.LoanDataMgr.GetFieldAccessRights(this.getUIFieldID(tag.OriginalID)) == BizRule.FieldAccessRight.Hide)
            str = "*";
          switch (control)
          {
            case CheckBox _:
              CheckBox checkBox = (CheckBox) control;
              checkBox.CheckedChanged -= new EventHandler(this.checkBox_CheckedChanged);
              checkBox.Checked = str == "Y" || str == "X";
              checkBox.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
              continue;
            case ComboBox _:
              ComboBox comboBox = (ComboBox) control;
              comboBox.SelectedIndexChanged -= new EventHandler(this.comboBox_SelectedIndexChanged);
              comboBox.Text = tag.FieldDef.ToDisplayValue(str);
              comboBox.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
              continue;
            case TextBox _:
              control.Text = str;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void displayFieldId(string id)
    {
      id = this.getUIFieldID(id);
      this.session.Application.GetService<IStatusDisplay>()?.DisplayFieldID(id);
    }

    private void loanProgramSearch_Click(object sender, EventArgs e)
    {
      if (!Session.Application.GetService<ILoanEditor>().GetInputEngineService(this.loan, this.editorMode == FieldQuickEditorMode.ForRequest ? InputEngineServiceType.ShowLPSelectorForLockRequest : InputEngineServiceType.ShowLPSelectorForLoan))
        return;
      this.FieldQuickEditor_Paint((object) null, (PaintEventArgs) null);
    }

    private void closingCostSearch_Click(object sender, EventArgs e)
    {
      if (!Session.Application.GetService<ILoanEditor>().GetInputEngineService(this.loan, InputEngineServiceType.ShowCCSelectorForLoan))
        return;
      this.FieldQuickEditor_Paint((object) null, (PaintEventArgs) null);
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
      this.panelFields = new Panel();
      this.toolTipField = new ToolTip(this.components);
      this.SuspendLayout();
      this.panelFields.AutoScroll = true;
      this.panelFields.Dock = DockStyle.Fill;
      this.panelFields.Location = new Point(0, 0);
      this.panelFields.Name = "panelFields";
      this.panelFields.Size = new Size(467, 492);
      this.panelFields.TabIndex = 0;
      this.panelFields.Resize += new EventHandler(this.panelFields_Resize);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.panelFields);
      this.Name = nameof (FieldQuickEditor);
      this.Size = new Size(467, 492);
      this.Paint += new PaintEventHandler(this.FieldQuickEditor_Paint);
      this.ResumeLayout(false);
    }
  }
}
