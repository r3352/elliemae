// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.BizCategories
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The BizCategories class represents the set of all Business Categories defined
  /// for the system. Business Categories are used to classify <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see>s
  /// into Appraisers, Lenders, etc.
  /// </summary>
  public class BizCategories : EnumBase, IBizCategories
  {
    internal BizCategories(IContactManager mngr)
    {
      foreach (EllieMae.EMLite.ClientServer.Contacts.BizCategory bizCategory in mngr.GetBizCategories())
        this.AddItem((EnumItem) new BizCategory(bizCategory));
    }

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see> with the specified index.</summary>
    public BizCategory this[int index] => (BizCategory) this.GetItem(index);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see> with the specified ID value.</summary>
    public BizCategory GetItemByID(int itemId) => (BizCategory) base.GetItemByID(itemId);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see> with the specified name.</summary>
    /// <param name="name">The name of the item being retrieved (case insensitive).</param>
    public BizCategory GetItemByName(string name) => (BizCategory) base.GetItemByName(name);
  }
}
