// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDisclosureStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class EDisclosureStatus : IEDisclosureStatus
  {
    private Loan loan;
    private DisclosureTrackingBase discItem;

    internal EDisclosureStatus(Loan loan, DisclosureTrackingBase discItem)
    {
      this.loan = loan;
      this.discItem = discItem;
    }

    public object BorrowerViewMessageDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerViewMessageDate);
    }

    public object CoBorrowerViewMessageDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerViewMessageDate);
    }

    public object BorrowerViewConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerViewConsentDate);
    }

    public object CoBorrowerViewConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerViewConsentDate);
    }

    public object BorrowerAcceptConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerAcceptConsentDate);
    }

    public object CoBorrowerAcceptConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerAcceptConsentDate);
    }

    public object BorrowerRejectConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerRejectConsentDate);
    }

    public object CoBorrowerRejectConsentDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerRejectConsentDate);
    }

    public object BorrowereSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowereSignedDate);
    }

    public object CoBorrowereSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowereSignedDate);
    }

    public object BorrowerWetSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureBorrowerWetSignedDate);
    }

    public object CoBorrowerWebSignedDate
    {
      get => this.toAPITime(this.discItem.eDisclosureCoBorrowerWebSignedDate);
    }

    public object PackageCreatedDate => this.toAPITime(this.discItem.eDisclosurePackageCreatedDate);

    public string FulfillmentOrderedBy => this.discItem.FulfillmentOrderedBy;

    public object FullfillmentProcessedDate
    {
      get => this.toAPITime(this.discItem.FullfillmentProcessedDate);
    }

    public bool IsWetSigned => this.discItem.IsWetSigned;

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
