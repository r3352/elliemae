// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactAccessLevel
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Defines the access levels for Borrower Contacts in Encompass.
  /// </summary>
  [Guid("9E9C3CED-A8A8-4d5d-B9D2-01AB48132F90")]
  public enum ContactAccessLevel
  {
    /// <summary>Contact is accessible only by the contact's owner.</summary>
    Private,
    /// <summary>Contact is publicly accessible by all other users.</summary>
    Public,
  }
}
