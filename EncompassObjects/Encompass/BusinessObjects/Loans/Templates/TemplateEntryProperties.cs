// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryProperties
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the collection of properties on a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntry" /> object.
  /// </summary>
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

    /// <summary>Gets a property value using the property name.</summary>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The value of the specified property if present in the collection. If no property
    /// exists with the specified name, a <c>null</c> is returned.</returns>
    public object this[string propertyName] => this.properties[(object) propertyName];

    /// <summary>
    /// Returns the list of available properties on the object.
    /// </summary>
    /// <returns></returns>
    public StringList GetPropertyNames()
    {
      StringList propertyNames = new StringList();
      foreach (string key in (IEnumerable) this.properties.Keys)
        propertyNames.Add(key);
      return propertyNames;
    }
  }
}
