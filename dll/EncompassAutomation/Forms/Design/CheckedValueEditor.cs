// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.CheckedValueEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class CheckedValueEditor : BaseDropdownTypeEditor
  {
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext pContext)
    {
      return pContext == null || (pContext.Instance as FieldControl).Field.Options.RequireValueFromList ? UITypeEditorEditStyle.DropDown : UITypeEditorEditStyle.None;
    }

    internal override void FillInList(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      ListBox pListBox)
    {
      if (pContext.PropertyDescriptor.Name == "UncheckedValue" && pContext.Instance is EllieMae.Encompass.Forms.CheckBox && ((EllieMae.Encompass.Forms.CheckBox) pContext.Instance).BehaveAsRadio)
        return;
      FieldControl instance = pContext.Instance as FieldControl;
      pListBox.Items.Add((object) "");
      pListBox.Items.AddRange((object[]) instance.Field.Options.GetValues().ToArray());
    }
  }
}
