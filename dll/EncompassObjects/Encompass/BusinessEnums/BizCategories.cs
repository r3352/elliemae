// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.BizCategories
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class BizCategories : EnumBase, IBizCategories
  {
    internal BizCategories(IContactManager mngr)
    {
      foreach (BizCategory bizCategory in mngr.GetBizCategories())
        this.AddItem((EnumItem) new BizCategory(bizCategory));
    }

    public BizCategory this[int index] => (BizCategory) this.GetItem(index);

    public BizCategory GetItemByID(int itemId) => (BizCategory) base.GetItemByID(itemId);

    public BizCategory GetItemByName(string name) => (BizCategory) base.GetItemByName(name);
  }
}
