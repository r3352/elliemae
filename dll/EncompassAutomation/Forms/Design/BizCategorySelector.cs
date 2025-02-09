// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.BizCategorySelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class BizCategorySelector : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      ListBox pListBox)
    {
      pListBox.Items.Clear();
      pListBox.Items.Add((object) "");
      foreach (BizCategory bizCategory in (EnumBase) EncompassApplication.Session.Contacts.BizCategories)
        pListBox.Items.Add((object) bizCategory);
      pListBox.Sorted = true;
    }

    protected override object TranslateValue(object value)
    {
      return value is string ? (object) null : value;
    }
  }
}
