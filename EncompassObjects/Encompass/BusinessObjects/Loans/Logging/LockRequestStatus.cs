// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Enumeration of the possible status values for a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" />.
  /// </summary>
  public enum LockRequestStatus
  {
    /// <summary>Request has not yet been sent.</summary>
    None = 0,
    /// <summary>Request has been sent but not locked.</summary>
    Pending = 1,
    /// <summary>Buy- or sell-side expiration date has passed prior to a lock being obtained.</summary>
    RateExpired = 2,
    /// <summary>Buy- or sell-side expiration date has passed after a lock was obtained.</summary>
    LockExpired = 3,
    /// <summary>The rate requested has been locked and confirmed.</summary>
    RateLocked = 4,
    /// <summary>Request represents an old rate lock request which has been superseced by a more recent request.</summary>
    Inactive = 5,
    /// <summary>Request has been denied.</summary>
    Denied = 6,
    /// <summary>Lock has been cancelled.</summary>
    Cancelled = 10, // 0x0000000A
  }
}
