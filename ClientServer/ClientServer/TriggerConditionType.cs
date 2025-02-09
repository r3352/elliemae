// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerConditionType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public enum TriggerConditionType
  {
    None = 0,
    ValueChange = 1,
    FixedValue = 2,
    Range = 3,
    ValueList = 4,
    NonEmptyValue = 5,
    MilestoneCompleted = 6,
    LockRequested = 7,
    LockConfirmed = 8,
    LockDenied = 9,
    RegisterLoan = 12, // 0x0000000C
    ImportAdditionalData = 13, // 0x0000000D
    OrderReissueCredit = 14, // 0x0000000E
    Disclosures = 16, // 0x00000010
    SubmitLoan = 17, // 0x00000011
    ChangedCircumstance = 18, // 0x00000012
    LockRequest = 19, // 0x00000013
    RunDUUnderwriting = 22, // 0x00000016
    ReSubmitLoan = 23, // 0x00000017
    ViewPurchaseAdvice = 25, // 0x00000019
    LockExtension = 27, // 0x0000001B
    RunLPUnderwriting = 28, // 0x0000001C
    SubmitPurchase = 29, // 0x0000001D
    FloatLock = 30, // 0x0000001E
    CancelLock = 31, // 0x0000001F
    RePriceLock = 32, // 0x00000020
    ReLockLock = 33, // 0x00000021
    ChangeRequestOB = 34, // 0x00000022
    Withdrawal = 35, // 0x00000023
    Cancel = 36, // 0x00000024
    RequestLoanEstimate = 37, // 0x00000025
    RequestTitleFees = 38, // 0x00000026
    GenerateLoanEstimateDisclosure = 39, // 0x00000027
    OrderAppraisalRequest = 40, // 0x00000028
    OrderAUS = 41, // 0x00000029
    SaveLoan = 42, // 0x0000002A
  }
}
