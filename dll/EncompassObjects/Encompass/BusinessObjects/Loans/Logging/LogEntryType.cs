// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Flags]
  [Guid("122EDFED-D29F-3B8D-BBA1-28068A780BA2")]
  public enum LogEntryType
  {
    MilestoneTask = 1,
    MilestoneEvent = 2,
    TrackedDocument = 4,
    Conversation = 8,
    ReceivedDownload = 32, // 0x00000020
    EDMTransaction = 64, // 0x00000040
    StatusOnlineUpdate = 128, // 0x00000080
    UnderwritingCondition = 256, // 0x00000100
    PostClosingCondition = 512, // 0x00000200
    LockRequest = 1024, // 0x00000400
    LockConfirmation = 2048, // 0x00000800
    LockDenial = 4096, // 0x00001000
    PrintEvent = 8192, // 0x00002000
    PreliminaryCondition = 16384, // 0x00004000
    InvestorRegistration = 32768, // 0x00008000
    Disclosure = 65536, // 0x00010000
    HtmlEmailMessage = 131072, // 0x00020000
    DocumentOrder = 262144, // 0x00040000
    LockCancellationRequest = 524288, // 0x00080000
    LockCancellation = 1048576, // 0x00100000
    Disclosure2015 = 2097152, // 0x00200000
    All = 16777215, // 0x00FFFFFF
  }
}
