// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContactType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Defines the the type of contact a BorrowerContact object represents.
  /// </summary>
  [Guid("545B004A-27DB-43e5-9466-A36D8C6ABFDE")]
  public enum BorrowerContactType
  {
    /// <summary>The borrower contact type has not been specified for this contact.</summary>
    Unspecified,
    /// <summary>The contact represents a business prospect.</summary>
    Propspect,
    /// <summary>Contact represents a past or present borrower.</summary>
    Client,
    /// <summary>Contact represents a lead.</summary>
    Lead,
  }
}
