// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILockDenial
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("E91DA56B-A6C1-4d8a-8135-22E39DE5030A")]
  public interface ILockDenial
  {
    string ID { get; }

    DateTime Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    bool AlertLO { get; set; }

    string DeniedBy { get; }

    LockRequest LockRequest { get; }
  }
}
