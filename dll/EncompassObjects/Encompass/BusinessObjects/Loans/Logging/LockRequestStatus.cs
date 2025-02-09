// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public enum LockRequestStatus
  {
    None = 0,
    Pending = 1,
    RateExpired = 2,
    LockExpired = 3,
    RateLocked = 4,
    Inactive = 5,
    Denied = 6,
    Cancelled = 10, // 0x0000000A
  }
}
