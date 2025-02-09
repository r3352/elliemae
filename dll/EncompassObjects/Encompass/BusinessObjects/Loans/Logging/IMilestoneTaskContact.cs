// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IMilestoneTaskContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("34B7DBD6-23C8-4daa-9720-F703AD4AE7E9")]
  public interface IMilestoneTaskContact
  {
    string Name { get; set; }

    BizCategory Category { get; set; }

    string PhoneNumber { get; set; }

    string Email { get; set; }

    string StreetAddress { get; set; }

    string City { get; set; }

    string State { get; set; }

    string ZipCode { get; set; }

    BizContact GetLinkedContact();

    void LinkContact(BizContact contactToLink);

    void UnlinkContact();
  }
}
