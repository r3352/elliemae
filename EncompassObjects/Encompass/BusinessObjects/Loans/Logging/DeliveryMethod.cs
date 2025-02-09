// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DeliveryMethod
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>The enumeration of the different disclosure methods.</summary>
  [Guid("CBB439AB-ADCF-46cc-8E31-1E04071FF6E9")]
  public enum DeliveryMethod
  {
    /// <summary>Disclosure method is unknown</summary>
    Unknown,
    /// <summary>Disclosure was sent by mail</summary>
    Mail,
    /// <summary>Disclosure was made electronically</summary>
    eDisclosure,
    /// <summary>Disclosure was sent by fax</summary>
    Fax,
    /// <summary>Disclosure was delivered in person</summary>
    InPerson,
    /// <summary>An unspecified disclosure method was used</summary>
    Other,
  }
}
