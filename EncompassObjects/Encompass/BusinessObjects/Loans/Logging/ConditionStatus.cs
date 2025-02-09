// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ConditionStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumeration of the different possible statuses of a Condition.
  /// </summary>
  public enum ConditionStatus
  {
    /// <summary>The condition has been added but no action taken.</summary>
    Added,
    /// <summary>The condition is waiting to be received.</summary>
    Expected,
    /// <summary>The condition has been requested from the source.</summary>
    Requested,
    /// <summary>The condition has been re-requested from the source.</summary>
    Rerequested,
    /// <summary>The condition has been received.</summary>
    Received,
    /// <summary>The condition has been reviewed.</summary>
    Reviewed,
    /// <summary>The condition has been sent to the recipient.</summary>
    Sent,
    /// <summary>The condition has been cleared.</summary>
    Cleared,
    /// <summary>The condition has been waived.</summary>
    Waived,
    /// <summary>The condition has been expired prior to being cleared or waived.</summary>
    Expired,
    /// <summary>The condition has been fulfilled.</summary>
    Fulfilled,
    /// <summary>The condition is past due.</summary>
    PastDue,
    /// <summary>The condition has been rejected.</summary>
    Rejected,
  }
}
