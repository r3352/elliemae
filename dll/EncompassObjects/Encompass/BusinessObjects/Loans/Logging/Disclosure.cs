// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class Disclosure : DisclosureBase, IDisclosure
  {
    internal Disclosure(Loan loan, DisclosureTrackingLog discItem)
      : base(loan, (DisclosureTrackingBase) discItem)
    {
    }

    public override LogEntryType EntryType => LogEntryType.Disclosure;

    public StandardDisclosureType DisclosureType
    {
      get
      {
        this.EnsureValid();
        StandardDisclosureType disclosureType = StandardDisclosureType.None;
        if (this.DiscItem.DisclosedForGFE)
          disclosureType |= StandardDisclosureType.GFE;
        if (this.DiscItem.DisclosedForTIL)
          disclosureType |= StandardDisclosureType.TIL;
        if (((DisclosureTrackingBase) this.DiscItem).DisclosedForSafeHarbor)
          disclosureType |= StandardDisclosureType.SAFEHARBOR;
        return disclosureType;
      }
    }

    public bool EnabledForCompliance
    {
      get
      {
        this.EnsureValid();
        return this.DiscItem.IsDisclosed;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.IsDisclosed = value;
      }
    }

    public DeliveryMethod DeliveryMethod
    {
      get
      {
        this.EnsureValid();
        return (DeliveryMethod) this.DiscItem.DisclosureMethod;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.DisclosureMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    public override object ReceivedDate
    {
      get
      {
        this.EnsureValid();
        return !this.discItem.Received ? (object) null : (object) this.discItem.ReceivedDate;
      }
      set
      {
        this.EnsureEditable();
        if (this.DeliveryMethod == DeliveryMethod.Fax || this.DeliveryMethod == DeliveryMethod.Mail || this.DeliveryMethod == DeliveryMethod.InPerson)
          throw new Exception("The received date cannot be changed and is set by regulation for the specified delivery method.");
        if (value == null)
        {
          this.discItem.ReceivedDate = DateTime.MinValue;
        }
        else
        {
          DateTime dateTime = value is DateTime ? Convert.ToDateTime(value) : throw new ArgumentException("The specified value must be a DateTime.");
          if (dateTime.Date < ((LogRecordBase) this.discItem).Date.Date)
            throw new ArgumentException("The specified date is prior to the disclosure date.");
          this.discItem.ReceivedDate = dateTime.Date;
        }
      }
    }

    internal override bool IseDiclosure => this.DeliveryMethod == DeliveryMethod.eDisclosure;

    private DisclosureTrackingLog DiscItem => (DisclosureTrackingLog) this.discItem;
  }
}
