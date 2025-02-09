// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputEventHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InputEventHelper
  {
    private List<object[]> targetFields;
    private bool zipChanged;

    public InputEventHelper() => this.targetFields = new List<object[]>();

    public void AddZipcodeFieldHelper(TextBox zipcodeField, TextBox cityField, ComboBox stateField)
    {
      zipcodeField.TextChanged += new EventHandler(this.zipcodeField_TextChanged);
      zipcodeField.Leave += new EventHandler(this.zipcodeField_Leave);
      zipcodeField.KeyUp += new KeyEventHandler(this.zipcodeField_KeyUp);
      this.targetFields.Add(new object[3]
      {
        (object) zipcodeField,
        (object) cityField,
        (object) stateField
      });
    }

    private void zipcodeField_TextChanged(object sender, EventArgs e) => this.zipChanged = true;

    private void zipcodeField_Leave(object sender, EventArgs e)
    {
      TextBox textBox1 = (TextBox) sender;
      if (textBox1 == null || textBox1.Text.Trim().Length < 5 || !this.zipChanged)
        return;
      this.zipChanged = false;
      for (int index = 0; index < this.targetFields.Count; ++index)
      {
        if (textBox1 == this.targetFields[index][0])
        {
          TextBox textBox2 = (TextBox) this.targetFields[index][1];
          ComboBox comboBox = (ComboBox) this.targetFields[index][2];
          ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(textBox1.Text.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(textBox1.Text.Trim().Substring(0, 5)));
          if (zipCodeInfo == null)
            break;
          textBox2.Text = zipCodeInfo.City;
          comboBox.Text = zipCodeInfo.State.ToUpper();
          break;
        }
      }
    }

    private void zipcodeField_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.ZIPCODE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    public void AddPhoneFieldHelper(TextBox phoneField)
    {
      phoneField.KeyUp += new KeyEventHandler(this.phoneField_KeyUp);
    }

    private void phoneField_KeyUp(object sender, KeyEventArgs e)
    {
      this.setFieldFormat(FieldFormat.PHONE, sender, e);
    }

    private void setFieldFormat(FieldFormat format, object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        return;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    public void AddNumericFieldHelper(TextBox numericField)
    {
      numericField.KeyPress += new KeyPressEventHandler(this.numeric_KeyUp);
      numericField.TextChanged += new EventHandler(this.field_TextChanged);
    }

    private void numeric_KeyUp(object sender, KeyPressEventArgs e)
    {
      NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
      string decimalSeparator = numberFormat.NumberDecimalSeparator;
      string numberGroupSeparator = numberFormat.NumberGroupSeparator;
      string negativeSign = numberFormat.NegativeSign;
      string str = e.KeyChar.ToString();
      if (char.IsDigit(e.KeyChar) || str.Equals(numberGroupSeparator) || str.Equals(negativeSign) || e.KeyChar == '\b')
        return;
      e.Handled = true;
    }

    private void field_TextChanged(object sender, EventArgs e)
    {
      if (Decimal.TryParse(((Control) sender).Text, out Decimal _))
        return;
      ((TextBoxBase) sender).Clear();
    }

    public void AddSSNFieldHelper(TextBox ssnField)
    {
      ssnField.KeyUp += new KeyEventHandler(this.ssnField_KeyUp);
    }

    private void ssnField_KeyUp(object sender, KeyEventArgs e)
    {
      this.setFieldFormat(FieldFormat.SSN, sender, e);
    }

    public void RemoveSSNFieldHelper(TextBox ssnField)
    {
      ssnField.KeyUp -= new KeyEventHandler(this.ssnField_KeyUp);
    }

    public void ReformatField(FieldFormat format, TextBox textField)
    {
      bool needsUpdate = false;
      string str = Utils.FormatInput(textField.Text.Trim(), format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textField.Text = str;
    }
  }
}
