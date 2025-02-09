// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IEDisclosureStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("7F8A31BB-662A-4f7c-B0D1-416EE22AB601")]
  public interface IEDisclosureStatus
  {
    object BorrowerViewMessageDate { get; }

    object CoBorrowerViewMessageDate { get; }

    object BorrowerViewConsentDate { get; }

    object CoBorrowerViewConsentDate { get; }

    object BorrowerAcceptConsentDate { get; }

    object CoBorrowerAcceptConsentDate { get; }

    object BorrowerRejectConsentDate { get; }

    object CoBorrowerRejectConsentDate { get; }

    object BorrowereSignedDate { get; }

    object CoBorrowereSignedDate { get; }

    object BorrowerWetSignedDate { get; }

    object CoBorrowerWebSignedDate { get; }

    object PackageCreatedDate { get; }

    string FulfillmentOrderedBy { get; }

    object FullfillmentProcessedDate { get; }

    bool IsWetSigned { get; }

    bool Refresh();
  }
}
