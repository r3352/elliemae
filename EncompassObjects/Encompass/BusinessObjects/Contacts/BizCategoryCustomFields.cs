// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BizCategoryCustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessEnums;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Provides access to the business category-specific custom fields for
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" />.
  /// </summary>
  public class BizCategoryCustomFields : IBizCategoryCustomFields
  {
    private Contact contact;
    private Hashtable categoryCustomFields = new Hashtable();

    internal BizCategoryCustomFields(Contact contact) => this.contact = contact;

    /// <summary>
    /// Gets the custom fields collection for a specific business category.
    /// </summary>
    public ContactCustomFields this[string categoryName]
    {
      get
      {
        if ((categoryName ?? "") == "")
          throw new ArgumentNullException("category", "The category cannot be blank or null");
        BizCategory itemByName = this.contact.Session.Contacts.BizCategories.GetItemByName(categoryName);
        if ((EnumItem) itemByName == (EnumItem) null)
          throw new ArgumentException("The category name '" + categoryName + "' is not a defined Business Category");
        if (!this.categoryCustomFields.Contains((object) itemByName))
          this.categoryCustomFields[(object) itemByName] = (object) new ContactCustomFields(itemByName, this.contact);
        return (ContactCustomFields) this.categoryCustomFields[(object) itemByName];
      }
    }

    internal void Commit()
    {
      foreach (BizCategory key in (IEnumerable) this.categoryCustomFields.Keys)
        ((ContactCustomFields) this.categoryCustomFields[(object) key]).Commit();
    }

    internal void Refresh() => this.categoryCustomFields = new Hashtable();
  }
}
