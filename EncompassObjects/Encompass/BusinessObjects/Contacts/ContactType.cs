// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Enumeration of the types of contacts which reside in the Encompass Contacts database.
  /// </summary>
  [Guid("6C2C4E1F-66FD-3662-B8A0-B234B259E810")]
  public enum ContactType
  {
    /// <summary>A current, previous or potential borrower.</summary>
    Borrower,
    /// <summary>A business partner or contact.</summary>
    Biz,
  }
}
