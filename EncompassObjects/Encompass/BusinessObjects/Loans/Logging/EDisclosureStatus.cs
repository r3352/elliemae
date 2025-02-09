// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDisclosureStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Rerepresents the status of an eDisclosure sent to a borrower.
  /// </summary>
  public class EDisclosureStatus : IEDisclosureStatus
  {
    private Loan loan;
    private DisclosureTrackingBase discItem;

    internal EDisclosureStatus(Loan loan, DisclosureTrackingBase discItem)
    {
      this.loan = loan;
      this.discItem = discItem;
    }

    /// <summary>
    /// Gets the date the borrower viewed the eDisclosure message.
    /// </summary>
    public object BorrowerViewMessageDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerViewMessageDate);
    }

    /// <summary>
    /// Gets the date the co-borrower viewed the eDisclosure message.
    /// </summary>
    public object CoBorrowerViewMessageDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerViewMessageDate);
    }

    /// <summary>
    /// Gets the date the borrower viewed the eDisclosure consent agreement.
    /// </summary>
    public object BorrowerViewConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerViewConsentDate);
    }

    /// <summary>
    /// Gets the date the co-borrower viewed the eDisclosure consent agreement.
    /// </summary>
    public object CoBorrowerViewConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerViewConsentDate);
    }

    /// <summary>
    /// Gets the date the borrower accepted the eDisclosure consent agreement.
    /// </summary>
    public object BorrowerAcceptConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerAcceptConsentDate);
    }

    /// <summary>
    /// Gets the date the co-borrower accepted the eDisclosure consent agreement.
    /// </summary>
    public object CoBorrowerAcceptConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerAcceptConsentDate);
    }

    /// <summary>
    /// Gets the date the borrower rejected the eDisclosure consent agreement.
    /// </summary>
    public object BorrowerRejectConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerRejectConsentDate);
    }

    /// <summary>
    /// Gets the date the co-borrower rejected the eDisclosure consent agreement.
    /// </summary>
    public object CoBorrowerRejectConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerRejectConsentDate);
    }

    /// <summary>
    /// Gets the date the borrower signed the eDisclosure package.
    /// </summary>
    public object BorrowereSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowereSignedDate);
    }

    /// <summary>
    /// Gets the date the co-borrower signed the eDisclosure package.
    /// </summary>
    public object CoBorrowereSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowereSignedDate);
    }

    /// <summary>
    /// Gets the date the borrower wet signed the disclosure package.
    /// </summary>
    public object BorrowerWetSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerWetSignedDate);
    }

    /// <summary>
    /// Gets the date the co-borrower wet signed the disclosure package.
    /// </summary>
    public object CoBorrowerWebSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerWebSignedDate);
    }

    /// <summary>Gets the date the eDisclosure packaged was generated.</summary>
    public object PackageCreatedDate => this.toAPITime(this.discItem.eDisclosurePackageCreatedDate);

    /// <summary>
    /// Gets the User ID of the user who ordered fulfillment of the eDisclosure package.
    /// </summary>
    public string FulfillmentOrderedBy => this.discItem.FulfillmentOrderedBy;

    /// <summary>
    /// Gets the date the eDisclosure package was processed for fulfillment by mail.
    /// </summary>
    public object FullfillmentProcessedDate
    {
      get => this.toAPITime(this.discItem.FullfillmentProcessedDate);
    }

    /// <summary>
    /// Indicates whether the disclosure package was wet signed.
    /// </summary>
    public bool IsWetSigned => this.discItem.IsWetSigned;

    /// <summary>
    /// Refreshes the status of the eDisclosure package, if there is one
    /// </summary>
    /// <returns>Returns <c>true</c> if the package information was updated,
    /// <c>false</c> otherwise.</returns>
    public bool Refresh()
    {
      if (this.discItem is DisclosureTrackingLog)
        return this.loan.Unwrap().SyncEDisclosurePackageStatus(new List<DisclosureTrackingLog>((IEnumerable<DisclosureTrackingLog>) new DisclosureTrackingLog[1]
        {
          (DisclosureTrackingLog) this.discItem
        }), true);
      if (!(this.discItem is DisclosureTracking2015Log))
        return false;
      return this.loan.Unwrap().SyncEDisclosurePackageStatus2015(new List<DisclosureTracking2015Log>((IEnumerable<DisclosureTracking2015Log>) new DisclosureTracking2015Log[1]
      {
        (DisclosureTracking2015Log) this.discItem
      }), true);
    }

    private object toAPITime(DateTime dt) => dt == DateTime.MinValue ? (object) null : (object) dt;
  }
}
