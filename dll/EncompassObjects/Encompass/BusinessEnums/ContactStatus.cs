// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ContactStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class ContactStatus : EnumItem, IContactStatus
  {
    internal ContactStatus(BorrowerStatusItem item)
      : base(item.index, item.name)
    {
    }
  }
}
