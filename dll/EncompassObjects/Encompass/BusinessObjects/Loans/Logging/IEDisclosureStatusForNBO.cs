// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IEDisclosureStatusForNBO
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("6342E27F-37D2-44D5-952A-7FD591A8EF4A")]
  public interface IEDisclosureStatusForNBO
  {
    DateTime eDisclosureNBOCAuthenticatedDate { get; }

    string eDisclosureNBOCAuthenticatedIP { get; }

    DateTime eDisclosureNBOCViewMessageDate { get; }

    DateTime eDisclosureNBOCRejectConsentDate { get; }

    string eDisclosureNBOCRejectConsentIP { get; }

    DateTime eDisclosureNBOCSignedDate { get; }

    string eDisclosureNBOCeSignedIP { get; }

    string eDisclosureNBOLoanLevelConsent { get; }

    DateTime eDisclosureNBOAcceptConsentDate { get; }

    string eDisclosureNBOAcceptConsentIP { get; }

    DateTime eDisclosureNBODocumentViewedDate { get; }
  }
}
