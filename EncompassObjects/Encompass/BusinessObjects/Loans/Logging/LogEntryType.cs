// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// An enumeration of the possible types of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry" /> objects.
  /// </summary>
  [Flags]
  [Guid("122EDFED-D29F-3B8D-BBA1-28068A780BA2")]
  public enum LogEntryType
  {
    /// <summary>A Milestone-related task, which generally must be completed prior to completing a milestone.</summary>
    MilestoneTask = 1,
    /// <summary>A Milestone-related event, such as the start or processing or the funding of the loan.</summary>
    MilestoneEvent = 2,
    /// <summary>Tracking information for a single document of the loan package.</summary>
    TrackedDocument = 4,
    /// <summary>An entry representing a conversation related to a loan, whether it be with
    /// the customer or with a partner/vendor.</summary>
    Conversation = 8,
    /// <summary>An entry representing a received Download.</summary>
    ReceivedDownload = 32, // 0x00000020
    /// <summary>An entry representing an Electronic Document Management-related transaction.</summary>
    EDMTransaction = 64, // 0x00000040
    /// <summary>An entry representing an update to the Status Online information for the loan.</summary>
    StatusOnlineUpdate = 128, // 0x00000080
    /// <summary>Tracking information for an undewriting condition.</summary>
    UnderwritingCondition = 256, // 0x00000100
    /// <summary>Tracking information for a post-closing or shipping condition.</summary>
    PostClosingCondition = 512, // 0x00000200
    /// <summary>Represents a lock request on a loan.</summary>
    LockRequest = 1024, // 0x00000400
    /// <summary>An entry representing a lock request confirmation on a loan.</summary>
    LockConfirmation = 2048, // 0x00000800
    /// <summary>An entry representing a lock request denial on a loan.</summary>
    LockDenial = 4096, // 0x00001000
    /// <summary>An entry representing a print event on a loan.</summary>
    PrintEvent = 8192, // 0x00002000
    /// <summary>Tracking information for a preliminary condition.</summary>
    PreliminaryCondition = 16384, // 0x00004000
    /// <summary>An entry representing the registration of the loan with an investor.</summary>
    InvestorRegistration = 32768, // 0x00008000
    /// <summary>Tracking information for a disclosure made to the borrower.</summary>
    Disclosure = 65536, // 0x00010000
    /// <summary>An entry representing an html email message.</summary>
    HtmlEmailMessage = 131072, // 0x00020000
    /// <summary>A record of a document order placed with the Encompass360 Doc Services.</summary>
    DocumentOrder = 262144, // 0x00040000
    /// <summary>An entry representing a request to cancel a rate lock.</summary>
    LockCancellationRequest = 524288, // 0x00080000
    /// <summary>An entry representing a rate lock cancellation.</summary>
    LockCancellation = 1048576, // 0x00100000
    /// <summary>An entry representing a 2015 disclosure made to a borrower.</summary>
    Disclosure2015 = 2097152, // 0x00200000
    /// <summary>A user-defined, custom event. Use this event type for reminders, alerts, etc.</summary>
    All = 16777215, // 0x00FFFFFF
  }
}
