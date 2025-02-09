// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.CodeLanguageEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides a TypeEditor for the CodeLanguage property on the <see cref="T:EllieMae.Encompass.Forms.Form" /> control.
  /// </summary>
  /// <exclude />
  public class CodeLanguageEditor : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
      ListBox pListBox)
    {
      pListBox.Items.Add((object) "VB.NET");
      pListBox.Items.Add((object) "C#");
    }

    /// <summary>
    /// Translates a text value to the ScriptLanguage enumeration.
    /// </summary>
    /// <param name="value">The text value to translate.</param>
    /// <returns>Returns the corresponding ScriptLanguage enume value.</returns>
    protected override object TranslateValue(object value)
    {
      return string.Concat(value) == "C#" ? (object) ScriptLanguage.CSharp : (object) ScriptLanguage.VB;
    }
  }
}
