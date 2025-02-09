// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ContactStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The ContactStatus represents a single Business Category to which a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see>
  /// can be assigned. Every category has a unique numeric ID as well as a displayable name.
  /// </summary>
  public class ContactStatus : EnumItem, IContactStatus
  {
    internal ContactStatus(BorrowerStatusItem item)
      : base(item.index, item.name)
    {
    }
  }
}
