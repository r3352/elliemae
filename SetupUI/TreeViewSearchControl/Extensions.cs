// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.Extensions
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchControl
{
  public static class Extensions
  {
    public static void BindEnumToComboBox<T>(this ComboBox comboBox, T defaultSelection)
    {
      List<\u003C\u003Ef__AnonymousType16<string, T>> list = Enum.GetValues(typeof (T)).Cast<T>().Select(value => new
      {
        Description = (Attribute.GetCustomAttribute((MemberInfo) value.GetType().GetField(value.ToString()), typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute ? customAttribute.Description : (string) null) ?? value.ToString(),
        Value = value
      }).ToList();
      comboBox.DataSource = (object) list;
      comboBox.DisplayMember = "Description";
      comboBox.ValueMember = "Value";
      foreach (var data in list)
      {
        if (data.Value.ToString() == defaultSelection.ToString())
          comboBox.SelectedItem = (object) data;
      }
    }
  }
}
