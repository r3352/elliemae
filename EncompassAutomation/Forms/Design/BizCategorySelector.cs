// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.BizCategorySelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides a type editor the allows selection of a Business Category.
  /// </summary>
  /// <exclude />
  public class BizCategorySelector : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
      ListBox pListBox)
    {
      pListBox.Items.Clear();
      pListBox.Items.Add((object) "");
      foreach (BizCategory bizCategory in (EnumBase) EncompassApplication.Session.Contacts.BizCategories)
        pListBox.Items.Add((object) bizCategory);
      pListBox.Sorted = true;
    }

    /// <summary>
    /// Performs translation on the selected value back to ensure the empty value is converted to null.
    /// </summary>
    /// <param name="value">The value to be translated</param>
    /// <returns></returns>
    protected override object TranslateValue(object value)
    {
      return value is string ? (object) null : value;
    }
  }
}
