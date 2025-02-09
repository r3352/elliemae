// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingPaymentMethod
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Enumeration for the support interim servicing payment methods.
  /// </summary>
  [Guid("294499DD-284A-4ae6-89E7-08A4F136049D")]
  public enum ServicingPaymentMethod
  {
    /// <summary>No method specified.</summary>
    None,
    /// <summary>Payment made by check.</summary>
    Check,
    /// <summary>Electronic payment made through the ACH.</summary>
    AutomatedClearingHouse,
    /// <summary>Payment taken from the lockbox.</summary>
    LockBox,
    /// <summary>Payment made via wire transfer.</summary>
    Wire,
  }
}
