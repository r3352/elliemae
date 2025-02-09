// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.GenericFormInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class GenericFormInputHandler
  {
    public EventHandler OnKeyUp;
    public EventHandler OnLeave;
    public EventHandler OnLockClicked;
    public EventHandler OnEntered;
    public EventHandler FieldFormatChanged;
    private Sessions.Session session;
    private IHtmlInput inputData;
    private LoanData loan;
    private PopupBusinessRules popupRules;
    private FieldFormat currentFieldFormat = FieldFormat.DECIMAL_2;
    private IStatusDisplay statusDisplay;
    private List<object> fieldControls;
    private bool keepZero;

    public List<object> FieldControls => this.fieldControls;

    public bool KeepZero
    {
      set => this.keepZero = value;
    }

    public GenericFormInputHandler(
      IHtmlInput inputData,
      Control.ControlCollection controls,
      Sessions.Session session)
    {
      this.session = session;
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.statusDisplay = this.session.Application.GetService<IStatusDisplay>();
      this.fieldControls = new List<object>();
      this.collectFields(controls);
      this.SetLockIconStatus();
    }

    private void collectFields(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case System.Windows.Forms.LinkLabel _:
          case Label _:
            continue;
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
          case DatePicker _:
          case FieldLockButton _:
          case Label _:
            if (c is TextBox || c is ComboBox || c is CheckBox || c is DatePicker || c is FieldLockButton || c is Label)
            {
              this.fieldControls.Add((object) c);
              continue;
            }
            continue;
          default:
            this.collectFields(c.Controls);
            continue;
        }
      }
    }

    public void SetVeriFieldIDinTag(string existingPrefix, string newPrefix, int veriIndx)
    {
      if (this.fieldControls == null || this.fieldControls.Count == 0)
        return;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null)
        {
          string str1 = fieldControl.Tag.ToString();
          if (!(str1 == string.Empty) && str1.StartsWith(existingPrefix))
          {
            string str2 = newPrefix + veriIndx.ToString("00") + str1.Substring(existingPrefix.Length + 2);
            fieldControl.Tag = (object) str2;
          }
        }
      }
    }

    public void SetAIQValuesToForm(
      bool refreshEncompassDataOnly,
      string type,
      AIQEmploymentData aiqEmpData = null,
      AIQPropertyRentalData aiqProp = null,
      AIQOtherIncomeData aiqOtherIncomeData = null)
    {
      if (this.fieldControls == null || this.fieldControls.Count == 0)
        return;
      string empty = string.Empty;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null && !(fieldControl.Tag is AIQEmploymentData) && !(fieldControl.Tag is AIQPropertyRentalData) && !(fieldControl.Tag is AIQOtherIncomeData))
        {
          string str1 = fieldControl.Tag.ToString();
          if (!(str1 == string.Empty) && (!refreshEncompassDataOnly || !str1.StartsWith("AIQ")))
          {
            this.currentFieldFormat = str1.StartsWith("AIQ") ? EncompassFields.GetFormat("BE" + str1.Substring(5)) : EncompassFields.GetFormat(str1);
            string str2 = "";
            if (!str1.StartsWith("AIQ"))
            {
              str2 = this.inputData.GetField(str1);
            }
            else
            {
              switch (type)
              {
                case "INCOME":
                  if (aiqEmpData != null)
                  {
                    str2 = aiqEmpData.GetField(str1);
                    break;
                  }
                  break;
                case "PROPERTYRENTAL":
                  if (aiqProp != null)
                  {
                    str2 = aiqProp.GetField(str1);
                    break;
                  }
                  break;
                case "OTHERINCOME":
                  if (aiqOtherIncomeData != null)
                  {
                    str2 = aiqOtherIncomeData.GetField(str1);
                    break;
                  }
                  break;
              }
            }
            if (str1.StartsWith(""))
            {
              switch (fieldControl)
              {
                case TextBox _:
                  TextBox textBox = (TextBox) fieldControl;
                  string str3 = Utils.ApplyFieldFormatting(str2, this.currentFieldFormat);
                  textBox.Text = str3;
                  if (this.currentFieldFormat == FieldFormat.INTEGER)
                  {
                    textBox.Text = textBox.Text.Replace(",", "");
                    continue;
                  }
                  continue;
                case CheckBox _:
                  ((CheckBox) fieldControl).Checked = str2 == "Y";
                  continue;
                case ComboBox _:
                  fieldControl.Text = str2;
                  continue;
                case DatePicker _:
                  fieldControl.Text = str2;
                  continue;
                case Label _:
                  fieldControl.Text = str2;
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }

    public void SetFieldValuesToForm()
    {
      if (this.fieldControls == null || this.fieldControls.Count == 0)
        return;
      string empty = string.Empty;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null)
        {
          string str = fieldControl.Tag.ToString();
          if (!(str == string.Empty))
          {
            this.currentFieldFormat = EncompassFields.GetFormat(str);
            switch (fieldControl)
            {
              case TextBox _:
                TextBox textBox = (TextBox) fieldControl;
                textBox.Text = this.inputData.GetField(str);
                if (this.currentFieldFormat == FieldFormat.INTEGER)
                {
                  textBox.Text = textBox.Text.Replace(",", "");
                  continue;
                }
                continue;
              case CheckBox _:
                ((CheckBox) fieldControl).Checked = this.inputData.GetField(str) == "Y";
                continue;
              case ComboBox _:
                fieldControl.Text = this.inputData.GetField(str);
                continue;
              case DatePicker _:
                fieldControl.Text = this.inputData.GetField(str);
                continue;
              case Label _:
                fieldControl.Text = this.inputData.GetField(str);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    public void SetFieldValuesToLoan() => this.SetFieldValuesToLoan(true);

    public void SetFieldValuesToLoan(bool triggerCalculationInRealTime)
    {
      if (this.fieldControls == null || this.fieldControls.Count == 0)
        return;
      string empty = string.Empty;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null)
        {
          string id = fieldControl.Tag.ToString();
          if (!(id == string.Empty))
          {
            switch (fieldControl)
            {
              case TextBox _:
                TextBox textBox = (TextBox) fieldControl;
                if (triggerCalculationInRealTime)
                {
                  this.inputData.SetField(id, textBox.Text);
                  continue;
                }
                this.inputData.SetCurrentField(id, textBox.Text);
                continue;
              case ComboBox _:
                ComboBox comboBox = (ComboBox) fieldControl;
                if (triggerCalculationInRealTime)
                {
                  this.inputData.SetField(id, comboBox.Text);
                  continue;
                }
                this.inputData.SetCurrentField(id, comboBox.Text);
                continue;
              case DatePicker _:
                DatePicker datePicker = (DatePicker) fieldControl;
                if (triggerCalculationInRealTime)
                {
                  this.inputData.SetField(id, datePicker.Text);
                  continue;
                }
                this.inputData.SetCurrentField(id, datePicker.Text);
                continue;
              case FieldLockButton _:
                FieldLockButton fieldLockButton = (FieldLockButton) fieldControl;
                if (fieldLockButton.Locked && !this.inputData.IsLocked(id))
                {
                  this.inputData.AddLock(id);
                  continue;
                }
                if (!fieldLockButton.Locked && this.inputData.IsLocked(id))
                {
                  this.inputData.RemoveLock(id);
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    public void SetFieldTip(ToolTip toolTip)
    {
      string empty = string.Empty;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null)
        {
          string caption = fieldControl.Tag.ToString();
          if (!(caption == string.Empty))
            toolTip.SetToolTip(fieldControl, caption);
        }
      }
    }

    public void SetBusinessRules(ResourceManager resources)
    {
      if (this.loan == null)
        return;
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      string empty = string.Empty;
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl.Tag != null)
        {
          string fieldID = fieldControl.Tag.ToString();
          if (!(fieldID == string.Empty))
          {
            this.popupRules.SetBusinessRules((object) fieldControl, fieldID);
            if (fieldControl is TextBox)
            {
              TextBox textBox = (TextBox) fieldControl;
              if (textBox.ReadOnly)
                textBox.BackColor = SystemColors.Control;
            }
          }
        }
      }
    }

    public void SetLockIconStatus()
    {
      foreach (Control fieldControl in this.fieldControls)
      {
        if (fieldControl is FieldLockButton && fieldControl.Tag != null)
        {
          string id = fieldControl.Tag.ToString();
          if (!(id == string.Empty))
          {
            FieldLockButton button = (FieldLockButton) fieldControl;
            button.Locked = this.inputData.IsLocked(id);
            this.setFieldLockStatus(id, button);
          }
        }
      }
    }

    public void SetFieldEvents(object control)
    {
      switch (control)
      {
        case TextBox _:
          TextBox textBox = (TextBox) control;
          textBox.KeyUp += new KeyEventHandler(this.textField_Keyup);
          textBox.KeyPress += new KeyPressEventHandler(this.textField_KeyPress);
          textBox.Leave += new EventHandler(this.control_Leave);
          textBox.Enter += new EventHandler(this.control_Enter);
          break;
        case FieldLockButton _:
          ((Control) control).Click += new EventHandler(this.btnLock_Clicked);
          break;
        case DatePicker _:
          DatePicker datePicker = (DatePicker) control;
          datePicker.Leave += new EventHandler(this.control_Leave);
          datePicker.Enter += new EventHandler(this.control_Enter);
          break;
      }
    }

    private void control_Enter(object sender, EventArgs e)
    {
      if (this.FieldFormatChanged != null)
        this.FieldFormatChanged(sender, new EventArgs());
      Control control = (Control) sender;
      if (control.Tag == null)
        return;
      string str = control.Tag.ToString();
      if (str != "" && control is TextBox && !((TextBoxBase) sender).ReadOnly)
        control.BackColor = Color.LightGoldenrodYellow;
      this.currentFieldFormat = EncompassFields.GetFormat(str);
      this.statusDisplay.DisplayFieldID(str);
      if (this.OnEntered == null)
        return;
      this.OnEntered(sender, e);
    }

    private void textField_Keyup(object sender, KeyEventArgs e)
    {
      if (this.currentFieldFormat != FieldFormat.STRING)
      {
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, this.currentFieldFormat, ref needsUpdate);
        if (needsUpdate)
        {
          textBox.Text = str;
          textBox.SelectionStart = str.Length;
        }
      }
      if (this.OnKeyUp == null)
        return;
      this.OnKeyUp(sender, (EventArgs) e);
    }

    private void textField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.currentFieldFormat == FieldFormat.STRING)
        return;
      char keyChar = e.KeyChar;
      if (keyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          e.Handled = true;
          return;
        }
      }
      e.Handled = false;
    }

    private void control_Leave(object sender, EventArgs e)
    {
      if (sender != null && sender is TextBox)
      {
        Control ctrl = (Control) sender;
        if (ctrl.Tag == null)
          return;
        if (ctrl is TextBox && !((TextBoxBase) ctrl).ReadOnly)
        {
          if (!this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
            return;
          if (this.currentFieldFormat != FieldFormat.STRING)
          {
            if (Utils.ParseDouble((object) ctrl.Text) == 0.0 && !this.keepZero)
            {
              if (ctrl.Tag != null && ctrl.Tag.ToString() == "3927" && !string.IsNullOrEmpty(ctrl.Text))
                ctrl.Text = "0";
              else if (ctrl.Tag.ToString() == "4489")
              {
                if (!string.IsNullOrEmpty(ctrl.Text))
                  ctrl.Text = "0.00";
              }
              else
                ctrl.Text = "";
            }
            else if (this.currentFieldFormat != FieldFormat.INTEGER)
              ctrl.Text = Utils.ApplyFieldFormatting(ctrl.Text.Trim(), this.currentFieldFormat);
          }
          ctrl.BackColor = Color.White;
        }
      }
      if (this.OnLeave == null)
        return;
      this.OnLeave(sender, e);
    }

    private void btnLock_Clicked(object sender, EventArgs e)
    {
      if (!(sender is FieldLockButton))
        return;
      FieldLockButton button = (FieldLockButton) sender;
      if (button.Tag == null)
        return;
      string id = button.Tag.ToString();
      if (id == string.Empty)
        return;
      button.Locked = !button.Locked;
      this.setFieldLockStatus(id, button);
      if (this.OnLockClicked == null)
        return;
      this.OnLockClicked(sender, e);
    }

    private void setFieldLockStatus(string id, FieldLockButton button)
    {
      if (this.popupRules != null && (this.popupRules.GetFieldAccessRight(id) == BizRule.FieldAccessRight.Hide || this.popupRules.GetFieldAccessRight(id) == BizRule.FieldAccessRight.ViewOnly))
        return;
      foreach (Control fieldControl in this.fieldControls)
      {
        switch (fieldControl)
        {
          case TextBox _:
          case ComboBox _:
          case DatePicker _:
            if (fieldControl.Tag != null)
            {
              string str = fieldControl.Tag.ToString();
              if (!(str == string.Empty) && !(str != id))
              {
                fieldControl.TabStop = button.Locked;
                switch (fieldControl)
                {
                  case TextBox _:
                    ((TextBoxBase) fieldControl).ReadOnly = !button.Locked;
                    break;
                  case ComboBox _:
                    fieldControl.Enabled = !button.Locked;
                    break;
                  case DatePicker _:
                    ((DatePicker) fieldControl).ReadOnly = !button.Locked;
                    break;
                }
                if (button.Locked)
                {
                  if (fieldControl is TextBox)
                    fieldControl.BackColor = SystemColors.Window;
                  fieldControl.Focus();
                  continue;
                }
                if (fieldControl is TextBox)
                  fieldControl.BackColor = SystemColors.Control;
                fieldControl.Text = !this.inputData.IsLocked(id) ? this.inputData.GetField(id) : this.inputData.GetOrgField(id);
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }

    public FieldFormat CurrentFieldFormat
    {
      set => this.currentFieldFormat = value;
    }
  }
}
