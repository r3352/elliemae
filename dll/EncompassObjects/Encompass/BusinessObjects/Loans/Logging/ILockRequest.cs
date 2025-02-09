// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILockRequest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("FA383EA7-AA66-444c-842C-13A2A734F292")]
  public interface ILockRequest
  {
    string ID { get; }

    DateTime Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    bool AlertLO { get; set; }

    string RequestedBy { get; }

    LockRequestStatus Status { get; }

    object BuySideExpirationDate { get; set; }

    object SellSideExpirationDate { get; set; }

    object SellSideDeliveryDate { get; set; }

    LockConfirmation Confirmation { get; }

    void Lock();

    LockConfirmation Confirm(User confirmingUser);

    LockRequestFields Fields { get; }

    LockDenial Deny(User denyingUser);
  }
}
