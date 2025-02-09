// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryProperties
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TemplateEntryProperties : ITemplateEntryProperties
  {
    private Hashtable properties;

    internal TemplateEntryProperties(Hashtable properties)
    {
      if (properties == null)
        this.properties = new Hashtable();
      else
        this.properties = properties;
    }

    public object this[string propertyName] => this.properties[(object) propertyName];

    public StringList GetPropertyNames()
    {
      StringList propertyNames = new StringList();
      foreach (string key in (IEnumerable) this.properties.Keys)
        propertyNames.Add(key);
      return propertyNames;
    }
  }
}
