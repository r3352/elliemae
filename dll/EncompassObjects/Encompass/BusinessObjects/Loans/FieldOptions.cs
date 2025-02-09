// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldOptions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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
      foreach (FieldOption baseOption in this.baseOptions)
        this.optionsList.Add((object) new FieldOption(baseOption.Text, baseOption.Value));
    }

    public bool RequireValueFromList => this.baseOptions.RequireValueFromList;

    public int Count => this.optionsList.Count;

    public FieldOption this[int index] => (FieldOption) this.optionsList[index];

    public bool ContainsValue(string value) => this.baseOptions.ContainsValue(value);

    public bool IsValueAllowed(string value) => this.baseOptions.IsValueAllowed(value);

    public StringList GetValues() => new StringList((IList) this.baseOptions.GetValues());

    public IEnumerator GetEnumerator() => this.optionsList.GetEnumerator();
  }
}
