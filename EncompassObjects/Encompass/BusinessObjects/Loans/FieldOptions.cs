// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldOptions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for FieldOptionCollection.</summary>
  public class FieldOptions : IFieldOptions, IEnumerable
  {
    private FieldOptionCollection baseOptions;
    private ArrayList optionsList = new ArrayList();

    internal FieldOptions()
    {
    }

    internal FieldOptions(FieldDefinition fieldDef)
    {
      this.baseOptions = fieldDef.Options;
      foreach (EllieMae.EMLite.DataEngine.FieldOption baseOption in this.baseOptions)
        this.optionsList.Add((object) new FieldOption(baseOption.Text, baseOption.Value));
    }

    /// <summary>
    /// Indicates if the field must take on one of the specified values
    /// </summary>
    public bool RequireValueFromList => this.baseOptions.RequireValueFromList;

    /// <summary>Returns the number of fields in the collection</summary>
    public int Count => this.optionsList.Count;

    /// <summary>Retrieves by index</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FieldOption this[int index] => (FieldOption) this.optionsList[index];

    /// <summary>Checks if a value is one of the ones permitted</summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool ContainsValue(string value) => this.baseOptions.ContainsValue(value);

    /// <summary>Checks if a value is one of the ones permitted</summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsValueAllowed(string value) => this.baseOptions.IsValueAllowed(value);

    /// <summary>Returns the value list</summary>
    /// <returns></returns>
    public StringList GetValues() => new StringList((IList) this.baseOptions.GetValues());

    /// <summary>Provides an enumerator</summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.optionsList.GetEnumerator();
  }
}
