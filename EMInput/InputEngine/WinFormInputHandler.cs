// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.WinFormInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class WinFormInputHandler
  {
    public static readonly Color FocusHighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204);
    public static readonly Color NoFocusColor = Color.White;
    private System.Windows.Forms.Control formControl;
    private ToolTip toolTip;
    private Hashtable fieldAccessRights;
    private IHtmlInput inputData;
    private Hashtable fieldDropdownRules;
    private bool isSuperAdmin;
    private bool isTemplate;
    private ILoanValidator validator;
    private bool autoCommit = true;
    private PiggybackSynchronization piggySyncTool;
    private Dictionary<string, FieldDefinition> fieldDefs;

    public event DataBindEventHandler DataBind;

    public event DataCommitEventHandler DataCommit;

    public WinFormInputHandler(LoanDataMgr loanMgr)
      : this((IHtmlInput) loanMgr.LoanData)
    {
      this.fieldAccessRights = loanMgr.GetFieldAccessList();
      this.fieldDropdownRules = loanMgr.GetDropdownFieldRuleList();
      this.isTemplate = loanMgr.LoanData.IsTemplate;
      this.validator = loanMgr.LoanData.Validator;
      if (loanMgr.LinkedLoan == null)
        return;
      this.piggySyncTool = new PiggybackSynchronization(loanMgr.LoanData);
    }

    public WinFormInputHandler(IHtmlInput inputData)
    {
      this.inputData = inputData;
      this.isTemplate = true;
      this.isSuperAdmin = Session.UserInfo.IsSuperAdministrator();
    }

    public bool AutoCommit
    {
      get => this.autoCommit;
      set => this.autoCommit = value;
    }

    protected IHtmlInput InputData => this.inputData;

    public virtual string GetFieldID(System.Windows.Forms.Control ctrl) => string.Concat(ctrl.Tag);

    public virtual void SetFieldID(System.Windows.Forms.Control ctrl, string fieldID)
    {
      ctrl.Tag = (object) fieldID;
    }

    public void Attach(System.Windows.Forms.Control formControl)
    {
      this.Attach(formControl, (ToolTip) null);
    }

    public virtual void Attach(System.Windows.Forms.Control formControl, ToolTip toolTip)
    {
      this.formControl = formControl;
      this.toolTip = toolTip;
      this.InitializeControls(formControl);
    }

    protected virtual void InitializeControls(System.Windows.Forms.Control parentControl)
    {
      this.fieldDefs = new Dictionary<string, FieldDefinition>();
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) parentControl.Controls)
      {
        if (this.IsFieldControl(control))
          this.InitializeFieldControl(control);
        else if (control is CalendarButton)
          this.InitializeCalendarButton(control as CalendarButton);
        else if (control.Controls.Count > 0)
          this.InitializeControls(control);
        this.ApplyBusinessRules(control);
      }
    }

    protected virtual void InitializeCalendarButton(CalendarButton btn)
    {
      btn.DateSelected += new CalendarPopupEventHandler(this.OnCalendarButtonDateSelected);
    }

    protected virtual void OnCalendarButtonDateSelected(object sender, CalendarPopupEventArgs e)
    {
      System.Windows.Forms.Control dateControl = ((CalendarButton) sender).DateControl;
      if (dateControl == null || !this.IsFieldControl(dateControl))
        return;
      this.ValidateAndAutoCommit(dateControl);
    }

    protected virtual void InitializeFieldControl(System.Windows.Forms.Control ctrl)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition(ctrl);
      this.ApplyFieldDefinitionToControl(ctrl, fieldDefinition);
      if (this.toolTip != null)
        this.SetToolTip(ctrl, this.toolTip);
      this.PopulateFieldValue(ctrl);
      ctrl.Enter += new EventHandler(this.OnFieldControlEnter);
      ctrl.Leave += new EventHandler(this.OnFieldControlLeave);
      ctrl.EnabledChanged += new EventHandler(this.OnFieldControlEnabledChanged);
    }

    protected virtual void OnFieldControlEnabledChanged(object sender, EventArgs e)
    {
      System.Windows.Forms.Control ctrl = (System.Windows.Forms.Control) sender;
      if (!WinFormInputHandler.IsTextControl(ctrl))
        return;
      this.SetTextControlBackground(ctrl, ctrl.Focused);
    }

    protected virtual void ApplyFieldDefinitionToControl(System.Windows.Forms.Control ctrl, FieldDefinition fieldDef)
    {
      if (ctrl is ComboBox)
        this.InitializeComboBox(ctrl as ComboBox, fieldDef);
      if (!(ctrl is System.Windows.Forms.TextBox) || fieldDef == null || fieldDef.Format == FieldFormat.STRING)
        return;
      ctrl.KeyUp += new KeyEventHandler(this.OnFieldControlKeyUp);
    }

    protected void InitializeComboBox(ComboBox cbo, FieldDefinition fieldDef)
    {
      if (fieldDef != null && fieldDef.Options.Count > 0)
      {
        cbo.DropDownStyle = !fieldDef.Options.RequireValueFromList ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
        this.PopulateDropdownList(cbo, fieldDef);
      }
      cbo.SelectedIndexChanged += new EventHandler(this.OnDropdownValueChanged);
    }

    protected virtual void OnDropdownValueChanged(object sender, EventArgs e)
    {
      this.ValidateAndAutoCommit((System.Windows.Forms.Control) sender);
    }

    protected virtual bool ValidateAndAutoCommit(System.Windows.Forms.Control c)
    {
      if (!this.RuleValidate(c) || !this.AutoCommit)
        return false;
      this.CommitValue(c);
      return true;
    }

    protected virtual void SetToolTip(System.Windows.Forms.Control ctrl, ToolTip toolTip)
    {
      string fieldId = this.GetFieldID(ctrl);
      toolTip.SetToolTip(ctrl, FieldHelp.GetText(fieldId) == "" ? fieldId : fieldId + ": " + FieldHelp.GetText(fieldId));
    }

    protected virtual void OnFieldControlEnter(object sender, EventArgs e)
    {
      System.Windows.Forms.Control ctrl = (System.Windows.Forms.Control) sender;
      if (WinFormInputHandler.IsTextControl(ctrl))
        this.SetTextControlBackground(ctrl, true);
      if (this.isTemplate)
        return;
      this.DisplayFieldID(this.GetFieldID(ctrl));
    }

    protected virtual void OnFieldControlLeave(object sender, EventArgs e)
    {
      System.Windows.Forms.Control control = (System.Windows.Forms.Control) sender;
      string fieldId = this.GetFieldID(control);
      if (this.inputData != null && !string.IsNullOrEmpty(fieldId) && control is ComboBox && this.inputData is LoanData && control.Text == this.inputData.GetSimpleField(fieldId))
        return;
      this.ValidateAndAutoCommit(control);
      if (!WinFormInputHandler.IsTextControl(control))
        return;
      this.SetTextControlBackground(control, false);
    }

    protected virtual void OnFieldControlKeyUp(object sender, KeyEventArgs e)
    {
      System.Windows.Forms.Control ctrl = (System.Windows.Forms.Control) sender;
      string fieldId = this.GetFieldID(ctrl);
      if (!this.fieldDefs.ContainsKey(fieldId))
        return;
      FieldFormat format = this.fieldDefs[fieldId].Format;
      if (format == FieldFormat.DATE && e.Control && e.KeyCode == Keys.D)
      {
        if (!ctrl.Enabled)
          return;
        ctrl.Text = DateTime.Today.ToString("MM/dd/yyyy");
      }
      else
      {
        if (!(ctrl is System.Windows.Forms.TextBox))
          return;
        this.ApplyFormatting((System.Windows.Forms.TextBox) ctrl, format);
      }
    }

    public void SetTextControlBackground(System.Windows.Forms.Control ctrl, bool hasFocus)
    {
      if (!ctrl.Enabled)
        ctrl.BackColor = Color.Empty;
      else if (ctrl is System.Windows.Forms.TextBox && ((TextBoxBase) ctrl).ReadOnly)
        ctrl.BackColor = Color.Empty;
      else if (hasFocus)
        ctrl.BackColor = WinFormInputHandler.FocusHighlightColor;
      else
        ctrl.BackColor = WinFormInputHandler.NoFocusColor;
    }

    public bool ApplyFormatting(System.Windows.Forms.TextBox ctrl)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition((System.Windows.Forms.Control) ctrl);
      return fieldDefinition != null && this.ApplyFormatting(ctrl, fieldDefinition.Format);
    }

    public bool ApplyFormatting(System.Windows.Forms.TextBox ctrl, FieldFormat format)
    {
      int newCursorPos = -1;
      bool needsUpdate = false;
      string str = Utils.FormatInput(ctrl.Text, format, ref needsUpdate, ctrl.SelectionStart, ref newCursorPos);
      if (needsUpdate)
      {
        ctrl.Text = str;
        ctrl.SelectionStart = newCursorPos;
      }
      return needsUpdate;
    }

    public bool PopulateDropdownList(ComboBox cbo)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition((System.Windows.Forms.Control) cbo);
      if (fieldDefinition == null)
        return false;
      this.PopulateDropdownList(cbo, fieldDefinition);
      return true;
    }

    protected virtual void PopulateDropdownList(ComboBox cbo, FieldDefinition fieldDef)
    {
      cbo.Items.Clear();
      bool flag = false;
      foreach (FieldOption option in fieldDef.Options)
      {
        if (option.Value == "")
          flag = true;
        cbo.Items.Add((object) option);
      }
      if (flag)
        return;
      cbo.Items.Insert(0, (object) FieldOption.Empty);
    }

    public FieldDefinition GetFieldDefinition(System.Windows.Forms.Control ctrl)
    {
      string fieldId = this.GetFieldID(ctrl);
      return fieldId == "" ? (FieldDefinition) null : this.GetFieldDefinition(fieldId);
    }

    public FieldDefinition GetFieldDefinition(string fieldID)
    {
      if (this.fieldDefs.ContainsKey(fieldID))
        return this.fieldDefs[fieldID];
      FieldDefinition fieldDefinition = !(this.inputData is LoanData) ? (FieldDefinition) StandardFields.GetField(fieldID) : EncompassFields.GetField(fieldID, ((LoanData) this.inputData).Settings.FieldSettings);
      this.fieldDefs[fieldID] = fieldDefinition;
      return fieldDefinition;
    }

    public virtual void PopulateControls(System.Windows.Forms.Control parentControl)
    {
      if (this.IsFieldControl(parentControl))
        this.PopulateFieldValue(parentControl);
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) parentControl.Controls)
      {
        if (this.IsFieldControl(control))
          this.PopulateFieldValue(control);
      }
    }

    public virtual bool PopulateFieldValue(System.Windows.Forms.Control ctrl)
    {
      string fieldId = this.GetFieldID(ctrl);
      string str = "";
      if (fieldId != "")
        str = this.inputData.GetField(fieldId);
      DataBindEventArgs e = new DataBindEventArgs(str);
      this.OnDataBind(ctrl, e);
      if (e.Cancel)
        return false;
      string val = e.Value;
      this.SetControlValue(ctrl, val);
      return true;
    }

    public virtual void SetControlValue(System.Windows.Forms.Control ctrl, string val)
    {
      switch (ctrl)
      {
        case System.Windows.Forms.CheckBox _:
          if (val == "Y")
          {
            ((System.Windows.Forms.CheckBox) ctrl).Checked = true;
            break;
          }
          ((System.Windows.Forms.CheckBox) ctrl).Checked = false;
          break;
        case DatePicker _:
          ((DatePicker) ctrl).Value = Utils.ParseDate((object) val);
          break;
        case ComboBox _:
          this.setDropdownValue(ctrl as ComboBox, val);
          break;
        default:
          ctrl.Text = val;
          break;
      }
    }

    private void setDropdownValue(ComboBox cbo, string val)
    {
      bool flag = false;
      for (int index = 0; index < cbo.Items.Count; ++index)
      {
        if (cbo.Items[index] is FieldOption)
        {
          if (((FieldOption) cbo.Items[index]).Value == val)
          {
            cbo.SelectedItem = cbo.Items[index];
            if (cbo.DropDownStyle == ComboBoxStyle.DropDown)
              cbo.Text = val;
            flag = true;
            break;
          }
        }
        else if (string.Concat(cbo.Items[index]) == val)
        {
          cbo.SelectedItem = cbo.Items[index];
          flag = true;
          break;
        }
      }
      if (flag || cbo.DropDownStyle != ComboBoxStyle.DropDown)
        return;
      cbo.Text = val;
    }

    public virtual bool IsFieldControl(System.Windows.Forms.Control c)
    {
      if (c == null || this.GetFieldID(c) == "")
        return false;
      switch (c)
      {
        case System.Windows.Forms.TextBox _:
          return true;
        case ComboBox _:
          return true;
        case System.Windows.Forms.CheckBox _:
          return true;
        case DatePicker _:
          return true;
        case SizableTextBox _:
          return true;
        default:
          return false;
      }
    }

    public static bool IsWinFormFieldControlType(System.Windows.Forms.Control c)
    {
      switch (c)
      {
        case System.Windows.Forms.TextBox _:
          return true;
        case ComboBox _:
          return true;
        case System.Windows.Forms.CheckBox _:
          return true;
        case DatePicker _:
          return true;
        case SizableTextBox _:
          return true;
        default:
          return false;
      }
    }

    protected static bool IsTextControl(System.Windows.Forms.Control ctrl)
    {
      switch (ctrl)
      {
        case System.Windows.Forms.TextBox _:
          return true;
        case SizableTextBox _:
          return true;
        case ComboBox _:
          return ((ComboBox) ctrl).DropDownStyle == ComboBoxStyle.DropDown;
        default:
          return false;
      }
    }

    public virtual void CommitValues() => this.commitControlValues(this.formControl);

    private void commitControlValues(System.Windows.Forms.Control parentControl)
    {
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) parentControl.Controls)
      {
        if (this.IsFieldControl(control))
          this.CommitValue(control);
        else if (control.Controls.Count > 0)
          this.commitControlValues(control);
      }
    }

    public virtual bool CommitValue(System.Windows.Forms.Control ctrl)
    {
      DataCommitEventArgs e = new DataCommitEventArgs(this.GetControlValue(ctrl));
      this.OnDataCommit(ctrl, e);
      if (e.Cancel)
        return false;
      string id = string.Concat(ctrl.Tag);
      string val = e.Value;
      if (id != "")
      {
        if (val == this.inputData.GetField(id))
          return false;
        if (this.piggySyncTool != null)
          this.piggySyncTool.SyncPiggyBackField(id, FieldSource.LinkedLoan, val, (BorrowerPair) null);
        this.inputData.SetField(id, e.Value);
        try
        {
          if (this.inputData is LoanData)
          {
            if (!((LoanData) this.inputData).IsTemplate)
              Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
          }
        }
        catch (Exception ex)
        {
        }
      }
      return true;
    }

    protected virtual void OnDataCommit(System.Windows.Forms.Control ctrl, DataCommitEventArgs e)
    {
      if (this.DataCommit == null)
        return;
      this.DataCommit((object) ctrl, e);
    }

    protected virtual void OnDataBind(System.Windows.Forms.Control ctrl, DataBindEventArgs e)
    {
      if (this.DataBind == null)
        return;
      this.DataBind((object) ctrl, e);
    }

    public void RefreshContents() => this.RefreshContents(false);

    public virtual void RefreshContents(bool force)
    {
      foreach (System.Windows.Forms.Control allFieldControl in this.GetAllFieldControls())
      {
        string fieldId = this.GetFieldID(allFieldControl);
        if (force || this.inputData.IsDirty(fieldId))
          this.PopulateFieldValue(allFieldControl);
      }
    }

    public System.Windows.Forms.Control[] GetAllFieldControls()
    {
      return this.GetChildFieldControls(this.formControl);
    }

    public System.Windows.Forms.Control[] GetChildFieldControls(System.Windows.Forms.Control parentControl)
    {
      List<System.Windows.Forms.Control> controlList = new List<System.Windows.Forms.Control>();
      foreach (System.Windows.Forms.Control control in (ArrangedElementCollection) parentControl.Controls)
      {
        if (this.IsFieldControl(control))
          controlList.Add(control);
        else if (control.Controls.Count > 0)
          controlList.AddRange((IEnumerable<System.Windows.Forms.Control>) this.GetChildFieldControls(control));
      }
      return controlList.ToArray();
    }

    public virtual string GetControlValue(System.Windows.Forms.Control ctrl)
    {
      switch (ctrl)
      {
        case System.Windows.Forms.CheckBox _:
          return !((System.Windows.Forms.CheckBox) ctrl).Checked ? "" : "Y";
        case ComboBox _:
          ComboBox comboBox = ctrl as ComboBox;
          return comboBox.SelectedItem is FieldOption ? ((FieldOption) comboBox.SelectedItem).Value : comboBox.Text;
        default:
          return ctrl.Text;
      }
    }

    public virtual bool RuleValidate(System.Windows.Forms.Control ctrl)
    {
      if (this.isSuperAdmin || this.validator == null)
        return true;
      string fieldId = this.GetFieldID(ctrl);
      if (fieldId == "")
        return true;
      string controlValue = this.GetControlValue(ctrl);
      try
      {
        this.validator.Validate(fieldId, controlValue);
      }
      catch (MissingPrerequisiteException ex)
      {
        StandardField field = StandardFields.GetField(ex.FieldID);
        if (field != null)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) Session.Application, "The field '" + field.Description + "' (" + field.FieldID + ") is required in order to complete this field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) Session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        if (ctrl is ComboBox)
          ((ComboBox) ctrl).SelectedIndexChanged -= new EventHandler(this.OnDropdownValueChanged);
        this.PopulateFieldValue(ctrl);
        if (ctrl is ComboBox)
          ((ComboBox) ctrl).SelectedIndexChanged += new EventHandler(this.OnDropdownValueChanged);
        return false;
      }
      catch (ValidationException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.PopulateFieldValue(ctrl);
        return false;
      }
      return true;
    }

    public virtual void ApplyBusinessRules(System.Windows.Forms.Control ctrl)
    {
      if (ctrl is System.Windows.Forms.Button)
      {
        this.ApplyButtonRule((System.Windows.Forms.Button) ctrl);
      }
      else
      {
        if (!this.IsFieldControl(ctrl))
          return;
        this.ApplyFieldRules(ctrl);
      }
    }

    public virtual void ApplyButtonRule(System.Windows.Forms.Button button)
    {
      string actionID = "BUTTON_" + button.Text.Trim().Replace("&", "");
      this.ApplyButtonRule(button, actionID);
    }

    public virtual void ApplyFieldRules(System.Windows.Forms.Control ctrl)
    {
      if (this.isSuperAdmin || this.fieldAccessRights == null)
        return;
      this.applyFieldDropdownRules(ctrl);
      this.ApplyFieldAccessRules(ctrl);
    }

    private void applyFieldDropdownRules(System.Windows.Forms.Control ctrl)
    {
      string fieldId = this.GetFieldID(ctrl);
      if (this.fieldDropdownRules == null || !this.fieldDropdownRules.ContainsKey((object) fieldId) || !(ctrl is System.Windows.Forms.TextBox))
        return;
      this.CreatePickList(ctrl as System.Windows.Forms.TextBox);
    }

    protected void CreatePickList(System.Windows.Forms.TextBox box)
    {
      if (box.Width > 19)
        box.Width -= 19;
      System.Windows.Forms.Button button = new System.Windows.Forms.Button();
      button.Name = box.Name + "_C";
      button.Top = box.Top + 1;
      button.Left = box.Left + box.Width + 2;
      button.Width = 17;
      button.Height = 17;
      button.FlatAppearance.BorderSize = 0;
      button.FlatStyle = FlatStyle.Flat;
      button.BackgroundImage = (System.Drawing.Image) Resources.dropdown;
      button.Click += new EventHandler(this.OnPickListClick);
      button.Tag = (object) box;
      box.Parent.Controls.Add((System.Windows.Forms.Control) button);
      FRList fieldDropdownRule = (FRList) this.fieldDropdownRules[(object) this.GetFieldID((System.Windows.Forms.Control) box)];
      box.ReadOnly = fieldDropdownRule.IsLock;
      box.TabStop = !fieldDropdownRule.IsLock;
    }

    protected virtual void OnPickListClick(object sender, EventArgs e)
    {
      System.Windows.Forms.Button button = (System.Windows.Forms.Button) sender;
      System.Windows.Forms.Control tag = (System.Windows.Forms.Control) button.Tag;
      string fieldId = this.GetFieldID(tag);
      FRList fieldDropdownRule = (FRList) this.fieldDropdownRules[(object) fieldId];
      List<DropdownOption> dropdownOptionList = new List<DropdownOption>();
      foreach (string text in fieldDropdownRule.List)
        dropdownOptionList.Add(new DropdownOption(text));
      Point screen = button.PointToScreen(new Point(0, button.Bottom + 1));
      using (FieldRuleDropdownDialog ruleDropdownDialog = new FieldRuleDropdownDialog(dropdownOptionList.ToArray(), "Field " + fieldId))
      {
        if (ruleDropdownDialog.Height + screen.Y > Screen.PrimaryScreen.Bounds.Height)
          screen.Y = Screen.PrimaryScreen.Bounds.Height - ruleDropdownDialog.Height - 2;
        ruleDropdownDialog.SetListBoxWidth(Math.Max(tag.Width, 20));
        ruleDropdownDialog.Location = screen;
        if (ruleDropdownDialog.ShowDialog((IWin32Window) Session.Application) != DialogResult.OK)
          return;
        this.SetControlValue(tag, ruleDropdownDialog.SelectedItem.Text);
        if (!this.AutoCommit)
          return;
        this.CommitValue(tag);
      }
    }

    protected void ApplyFieldAccessRules(System.Windows.Forms.Control ctrl)
    {
      string fieldId = this.GetFieldID(ctrl);
      if (this.fieldAccessRights == null || !this.fieldAccessRights.ContainsKey((object) fieldId))
        return;
      BizRule.FieldAccessRight fieldAccessRights = Session.LoanDataMgr.GetFieldAccessRights(fieldId);
      switch (fieldAccessRights)
      {
        case BizRule.FieldAccessRight.Edit:
          break;
        case BizRule.FieldAccessRight.DoesNotApply:
          break;
        default:
          this.ApplyFieldAccessRules(ctrl, fieldAccessRights);
          break;
      }
    }

    protected virtual void ApplyFieldAccessRules(System.Windows.Forms.Control ctrl, BizRule.FieldAccessRight rights)
    {
      string fieldId = this.GetFieldID(ctrl);
      System.Windows.Forms.TextBox textBox1 = (System.Windows.Forms.TextBox) null;
      if (rights == BizRule.FieldAccessRight.Hide)
      {
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = (System.Drawing.Image) Resources.asterisk;
        pictureBox.Location = new Point(0, 0);
        pictureBox.Name = "pictureBox" + fieldId;
        pictureBox.TabStop = false;
        if (ctrl is System.Windows.Forms.TextBox)
        {
          System.Windows.Forms.TextBox textBox2 = (System.Windows.Forms.TextBox) ctrl;
          pictureBox.BackColor = textBox2.BackColor;
          pictureBox.Size = new Size(textBox2.Width, textBox2.Height);
          textBox2.Controls.Add((System.Windows.Forms.Control) pictureBox);
        }
        else if (ctrl is System.Windows.Forms.CheckBox)
        {
          System.Windows.Forms.CheckBox checkBox = (System.Windows.Forms.CheckBox) ctrl;
          pictureBox.BackColor = checkBox.BackColor;
          pictureBox.Location = new Point(2, 3);
          pictureBox.Size = new Size(10, 10);
          checkBox.Controls.Add((System.Windows.Forms.Control) pictureBox);
        }
        else if (ctrl is ComboBox)
        {
          ComboBox box = (ComboBox) ctrl;
          box.Visible = false;
          textBox1 = this.createTextBox(false, (System.Windows.Forms.Control) box, pictureBox);
          box.Parent.Controls.Add((System.Windows.Forms.Control) textBox1);
        }
        else if (ctrl is DatePicker)
        {
          DatePicker box = (DatePicker) ctrl;
          box.Visible = false;
          textBox1 = this.createTextBox(false, (System.Windows.Forms.Control) box, pictureBox);
          box.Parent.Controls.Add((System.Windows.Forms.Control) textBox1);
        }
      }
      if (ctrl is System.Windows.Forms.TextBox)
      {
        System.Windows.Forms.TextBox textBox3 = (System.Windows.Forms.TextBox) ctrl;
        textBox3.ReadOnly = true;
        textBox3.TabStop = false;
      }
      else if (ctrl is System.Windows.Forms.CheckBox)
      {
        System.Windows.Forms.CheckBox checkBox = (System.Windows.Forms.CheckBox) ctrl;
        checkBox.Enabled = false;
        checkBox.TabStop = false;
      }
      else if (ctrl is ComboBox)
      {
        ComboBox box = (ComboBox) ctrl;
        box.Visible = false;
        box.Enabled = false;
        box.TabStop = false;
        if (textBox1 != null)
          return;
        System.Windows.Forms.TextBox textBox4 = this.createTextBox(true, (System.Windows.Forms.Control) box, (PictureBox) null);
        textBox4.Text = box.Text;
        box.Parent.Controls.Add((System.Windows.Forms.Control) textBox4);
      }
      else
      {
        if (!(ctrl is DatePicker))
          return;
        DatePicker box = (DatePicker) ctrl;
        box.Visible = false;
        box.Enabled = false;
        box.TabStop = false;
        if (textBox1 != null)
          return;
        System.Windows.Forms.TextBox textBox5 = this.createTextBox(true, (System.Windows.Forms.Control) box, (PictureBox) null);
        textBox5.Text = box.Text;
        box.Parent.Controls.Add((System.Windows.Forms.Control) textBox5);
      }
    }

    public virtual void ApplyButtonRule(System.Windows.Forms.Button button, string actionID)
    {
      if (this.isSuperAdmin || this.fieldAccessRights == null)
        return;
      if (!actionID.ToLower().StartsWith("button_"))
        actionID = "Button_" + actionID;
      if (this.fieldAccessRights == null || !this.fieldAccessRights.ContainsKey((object) actionID))
        return;
      switch (Session.LoanDataMgr.GetFieldAccessRights(actionID))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    private System.Windows.Forms.TextBox createTextBox(
      bool readOnly,
      System.Windows.Forms.Control box,
      PictureBox pictureBox)
    {
      System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
      textBox.Size = new Size(box.Width, box.Height);
      textBox.ReadOnly = true;
      textBox.TabStop = false;
      textBox.Visible = true;
      textBox.Location = new Point(box.Left, box.Top);
      textBox.Tag = box.Tag;
      textBox.Enter += new EventHandler(this.OnFieldControlEnter);
      if (!readOnly)
      {
        pictureBox.Location = new Point(2, 2);
        pictureBox.Size = new Size(textBox.Width, textBox.Height);
        pictureBox.BackColor = textBox.BackColor;
        textBox.Controls.Add((System.Windows.Forms.Control) pictureBox);
      }
      return textBox;
    }

    protected void DisplayFieldID(string fieldID)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(fieldID);
    }

    public static WinFormInputHandler Create(LoanData loan)
    {
      return loan.DataMgr != null ? new WinFormInputHandler((LoanDataMgr) loan.DataMgr) : new WinFormInputHandler((IHtmlInput) loan);
    }
  }
}
