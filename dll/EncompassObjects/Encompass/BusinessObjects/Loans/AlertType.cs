// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AlertType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public enum AlertType
  {
    MilestoneCompletion = 0,
    Conversation = 1,
    Document = 2,
    EscrowDisbursement = 3,
    Reminder = 4,
    Milestone = 5,
    BorrowerPayment = 6,
    StatementDelivery = 7,
    PurchaseAdvice = 8,
    RateLockConfirmation = 9,
    RateLock = 10, // 0x0000000A
    InvestorShipping = 12, // 0x0000000C
    InvestorRegistration = 14, // 0x0000000E
    Condition = 15, // 0x0000000F
    DocumentExpiration = 17, // 0x00000011
    RateLockRequest = 18, // 0x00000012
    RateLockDenied = 19, // 0x00000013
    MilestoneTask = 20, // 0x00000014
    RediscloseTIL = 21, // 0x00000015
    InitialDisclosures = 22, // 0x00000016
    GFEExpires = 23, // 0x00000017
    RediscloseGFERateLocked = 24, // 0x00000018
    HUD1ToleranceViolation = 25, // 0x00000019
    ClosingDateViolation = 26, // 0x0000001A
    RediscloseGFEChangedCircumstances = 27, // 0x0000001B
    ComplianceReview = 28, // 0x0000001C
  }
}
