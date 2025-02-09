// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AlertType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// The different types of alerts possible on an item in the Pipeline
  /// </summary>
  public enum AlertType
  {
    /// <summary>An alert indicating that a milestone has been completed.</summary>
    MilestoneCompletion = 0,
    /// <summary>An follow-up alert related to a perviously held conversation</summary>
    Conversation = 1,
    /// <summary>An alert related to the expected arrival or expiration of a document</summary>
    Document = 2,
    /// <summary>Indicates that a servicing distribution from escrow is due.</summary>
    EscrowDisbursement = 3,
    /// <summary>An reminder alert related to a milestone task entry</summary>
    Reminder = 4,
    /// <summary>An alert caused by the expiration (or pending expiration) of a milestone</summary>
    Milestone = 5,
    /// <summary>Indicates that a borrower payment is past due when servicing a loan.</summary>
    BorrowerPayment = 6,
    /// <summary>Indicates that a statement is past due to be printed/mailed when servicing a loan.</summary>
    StatementDelivery = 7,
    /// <summary>Indicates that the purchase advice form is incomplete.</summary>
    PurchaseAdvice = 8,
    /// <summary>An alert related to the confirmation of a previously requested rate lock.</summary>
    RateLockConfirmation = 9,
    /// <summary>An alert related to the pending expiration of a rate lock</summary>
    RateLock = 10, // 0x0000000A
    /// <summary>An alert indicating that the loan is due to be shipped to the investor.</summary>
    InvestorShipping = 12, // 0x0000000C
    /// <summary>An alert indicating that a rate has been registered with an investor.</summary>
    InvestorRegistration = 14, // 0x0000000E
    /// <summary>An alert indicating a condition is expiring or past due.</summary>
    Condition = 15, // 0x0000000F
    /// <summary>An alert indicating that a document has expired.</summary>
    DocumentExpiration = 17, // 0x00000011
    /// <summary>An alert indicating a rate lock has been requested.</summary>
    RateLockRequest = 18, // 0x00000012
    /// <summary>Indicates that a rate lock request has been denied.</summary>
    RateLockDenied = 19, // 0x00000013
    /// <summary>Indicates that a milestone task is expected or past due.</summary>
    MilestoneTask = 20, // 0x00000014
    /// <summary>A redisclosure of the TIL is required for regulatory purposes.</summary>
    RediscloseTIL = 21, // 0x00000015
    /// <summary>You are required to make initial disclosures to the borrower.</summary>
    InitialDisclosures = 22, // 0x00000016
    /// <summary>The GFE is set to expire.</summary>
    GFEExpires = 23, // 0x00000017
    /// <summary>A redisclosure of the GFE is required after the rate is locked.</summary>
    RediscloseGFERateLocked = 24, // 0x00000018
    /// <summary>A variation between the HUD1 and GFE that is outside of allowed tolerances is detected.</summary>
    HUD1ToleranceViolation = 25, // 0x00000019
    /// <summary>The estimated closing date is earlier than the earliest closing date allowed for compliance.</summary>
    ClosingDateViolation = 26, // 0x0000001A
    /// <summary>A redisclosure of the GFE is required after changed circumstances.</summary>
    RediscloseGFEChangedCircumstances = 27, // 0x0000001B
    /// <summary>Indicates that the loan has failed a comprehensive compliance review.</summary>
    ComplianceReview = 28, // 0x0000001C
  }
}
