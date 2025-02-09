// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.EmploymentStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Defines the possible employment statuses for a borrower.
  /// </summary>
  [Guid("0F38D55D-B38D-45c3-9BCD-F337575DC8E2")]
  public enum EmploymentStatus
  {
    /// <summary>The borrower's employment status is unknown or has not been specified.</summary>
    Unspecified,
    /// <summary>The borrower is currently employed.</summary>
    Employed,
    /// <summary>The borrower is currently self-emplyed.</summary>
    SelfEmployed,
    /// <summary>The borrower is currently unemployed.</summary>
    Unemployed,
  }
}
