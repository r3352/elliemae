// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockAndRequestStatusField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockAndRequestStatusField : BankerEditionVirtualField
  {
    public const string NotLockedNoRequest = "NotLocked-NoRequest";
    public const string NotLockedWithRequest = "NotLocked-Request";
    public const string LockedNoRequest = "Locked-NoRequest";
    public const string LockedWithRequest = "Locked-Request";
    public const string LockedWithExtensionRequest = "Locked-Extension-Request";
    public const string ExpiredNoRequest = "Expired-NoRequest";
    public const string ExpiredWithRequest = "Expired-Request";
    public const string ExpiredWithExtensionRequest = "Expired-Extension-Request";
    public const string Cancelled = "Cancelled";
    public const string LockedWithCancellationRequest = "Locked-Cancellation-Request";
    public const string Voided = "Voided";
    public const string PendingRequest = "PendingRequest";
    private FieldOptionCollection options;

    public RateLockAndRequestStatusField()
      : base("LOCKRATE.RATEREQUESTSTATUS", "Rate Lock/Request Status", FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection(new List<FieldOption>()
      {
        new FieldOption("Not Locked", "NotLocked-NoRequest"),
        new FieldOption("Lock Requested", "NotLocked-Request"),
        new FieldOption("Locked", "Locked-NoRequest"),
        new FieldOption("Locked, New Lock Requested", "Locked-Request"),
        new FieldOption("Locked, Extension Requested", "Locked-Extension-Request"),
        new FieldOption("Expired", "Expired-NoRequest"),
        new FieldOption("Expired, New Lock Requested", "Expired-Request"),
        new FieldOption("Expired, Extension Requested", "Expired-Extension-Request"),
        new FieldOption("Lock Cancelled", nameof (Cancelled)),
        new FieldOption("Locked, Cancellation Requested", "Locked-Cancellation-Request"),
        new FieldOption("Lock Voided", nameof (Voided))
      }.ToArray(), true);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    public override FieldOptionCollection Options => this.options;

    public override bool AllowInReportingDatabase => false;

    protected override string Evaluate(LoanData loan)
    {
      string str = loan.GetField("LOCKRATE.RATESTATUS");
      string field1 = loan.GetField("LOCKRATE.REQUESTPENDING");
      string field2 = loan.GetField("LOCKRATE.REQUESTTYPE");
      string field3 = loan.GetField("LOCKRATE.EXTENSIONREQUESTPENDING");
      string field4 = loan.GetField("LOCKRATE.CANCELLATIONREQUESTPENDING");
      string field5 = loan.GetField("LOCKRATE.RELOCKREQUESTPENDING");
      if (field1 == "Y")
      {
        if (str == "Cancelled")
          str = "NotLocked";
        if (!(field5 == "Y"))
        {
          if (field3 == "Y" || field2 == "Extended")
            str += "-Extension";
          else if ((field4 == "Y" || field2 == "Cancellation") && str != "Expired")
            str += "-Cancellation";
        }
        str += "-Request";
      }
      else if (str != "Cancelled")
        str += "-NoRequest";
      return str;
    }
  }
}
