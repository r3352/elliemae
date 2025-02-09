// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.AmortizationType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Defines the possible amortization types for a loan.</summary>
  [Guid("B858057B-4AEA-4b29-9A72-3B90A0C8E35F")]
  public enum AmortizationType
  {
    /// <summary>The amortization type is unknown or has not been specified.</summary>
    Unspecified,
    /// <summary>The associated loan is at a fixed rate.</summary>
    FixedRate,
    /// <summary>The associated loan is a graduate payment mortgage.</summary>
    GPM,
    /// <summary>The associated loan is at an adjustable rate.</summary>
    ARM,
    /// <summary>The amortization is of another type.</summary>
    Other,
  }
}
