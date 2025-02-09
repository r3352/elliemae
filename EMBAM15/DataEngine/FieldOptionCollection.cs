// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldOptionCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldOptionCollection : IEnumerable
  {
    public static readonly FieldOptionCollection Empty = new FieldOptionCollection();
    private bool required;
    private ArrayList options = new ArrayList();

    public FieldOptionCollection()
    {
    }

    public FieldOptionCollection(FieldDefinition def, XmlElement e)
    {
      if (e != null)
      {
        this.required = e.GetAttribute("Required") == "1";
        foreach (XmlElement selectNode in e.SelectNodes("Option"))
          this.options.Add((object) new FieldOption(selectNode));
      }
      if (def.Format == FieldFormat.X)
      {
        this.required = true;
        if (!this.ContainsValue("X"))
          this.options.Add((object) new FieldOption("X", "X"));
        if (this.options.Count != 1)
          throw new Exception("Field definition error for field '" + def.FieldID + "': A Y/N field cannot have options other than 'Y' and 'N'");
      }
      else
      {
        if (def.Format != FieldFormat.YN)
          return;
        this.required = true;
        if (this.ContainsValue("Y") && this.ValueToText("Y") == "")
          this.options.Remove((object) new FieldOption("", "Y"));
        if (this.ContainsValue("N") && this.ValueToText("N") == "")
          this.options.Remove((object) new FieldOption("", "N"));
        if (!this.ContainsValue("Y"))
          this.options.Add((object) new FieldOption("Yes", "Y"));
        if (!this.ContainsValue("N"))
          this.options.Add((object) new FieldOption("No", "N"));
        if (this.options.Count != 2)
          throw new Exception("Field definition error for field '" + def.FieldID + "': A Y/N field cannot have options other than 'Y' and 'N'");
      }
    }

    internal FieldOptionCollection(CustomFieldInfo customField)
    {
      if (customField.Format == FieldFormat.X)
      {
        this.required = true;
        this.options.Add((object) new FieldOption("X", "X"));
      }
      else if (customField.Format == FieldFormat.YN)
      {
        this.required = true;
        this.options.Add((object) new FieldOption("Yes", "Y"));
        this.options.Add((object) new FieldOption("No", "N"));
      }
      else
      {
        if (customField.Options == null)
          return;
        this.required = customField.Format == FieldFormat.DROPDOWNLIST;
        foreach (string option in customField.Options)
          this.options.Add((object) new FieldOption(option, option));
      }
    }

    public FieldOptionCollection(FieldOption[] optionList, bool required)
    {
      this.options = new ArrayList((ICollection) optionList);
      this.required = required;
    }

    public FieldOptionCollection(string[] optionList, bool required)
    {
      this.options = new ArrayList();
      foreach (string option in optionList)
        this.options.Add((object) new FieldOption(option, option));
      this.required = required;
    }

    public FieldOption GetOptionByValue(string value)
    {
      foreach (FieldOption option in this.options)
      {
        if (option.Value == value)
          return option;
      }
      return (FieldOption) null;
    }

    public FieldOption GetOptionByText(string text)
    {
      foreach (FieldOption option in this.options)
      {
        if (option.Text == text)
          return option;
      }
      return (FieldOption) null;
    }

    public bool RequireValueFromList
    {
      get => this.required;
      set => this.required = value;
    }

    public int Count => this.options.Count;

    public FieldOption this[int index] => (FieldOption) this.options[index];

    public bool ContainsValue(string value)
    {
      foreach (FieldOption option in this.options)
      {
        if (option.Value == value)
          return true;
      }
      return false;
    }

    public string TextToValue(string text)
    {
      foreach (FieldOption option in this.options)
      {
        if (option.Text == text)
          return option.Value;
      }
      return "";
    }

    public string ValueToText(string value)
    {
      foreach (FieldOption option in this.options)
      {
        if (option.Value == value || option.ReportingDatabaseValue == value)
          return option.Text;
      }
      return "";
    }

    public void AddOption(string text, string value)
    {
      this.options.Add((object) new FieldOption(text, value));
    }

    public void RemoveOption(FieldOption fieldOption) => this.options.Remove((object) fieldOption);

    public void Clear()
    {
      this.options.Clear();
      this.required = false;
    }

    public bool IsValueAllowed(string value)
    {
      return !this.required || value == "" || this.ContainsValue(value);
    }

    public string[] GetValues()
    {
      ArrayList arrayList = new ArrayList();
      foreach (FieldOption option in this.options)
        arrayList.Add((object) option.Value);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public Dictionary<bool, string> GetBooleanValues()
    {
      Dictionary<bool, string> booleanValues = new Dictionary<bool, string>();
      foreach (FieldOption option in this.options)
      {
        bool? booleanValue = option.BooleanValue;
        if (booleanValue.HasValue)
        {
          Dictionary<bool, string> dictionary = booleanValues;
          booleanValue = option.BooleanValue;
          int num = booleanValue.Value ? 1 : 0;
          string str = option.Value;
          dictionary.Add(num != 0, str);
        }
      }
      return booleanValues;
    }

    public FieldOption[] ToArray() => (FieldOption[]) this.options.ToArray(typeof (FieldOption));

    public IEnumerator GetEnumerator() => this.options.GetEnumerator();
  }
}
