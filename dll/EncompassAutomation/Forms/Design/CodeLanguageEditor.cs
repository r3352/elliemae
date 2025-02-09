// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.CodeLanguageEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class CodeLanguageEditor : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      ListBox pListBox)
    {
      pListBox.Items.Add((object) "VB.NET");
      pListBox.Items.Add((object) "C#");
    }

    protected override object TranslateValue(object value)
    {
      return string.Concat(value) == "C#" ? (object) ScriptLanguage.CSharp : (object) ScriptLanguage.VB;
    }
  }
}
