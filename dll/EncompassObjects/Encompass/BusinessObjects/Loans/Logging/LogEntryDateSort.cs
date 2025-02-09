// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryDateSort
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  internal class LogEntryDateSort : IComparer
  {
    public int Compare(object x, object y)
    {
      LogEntry logEntry1 = x as LogEntry;
      LogEntry logEntry2 = y as LogEntry;
      if (x == y)
        return 0;
      if (logEntry1 == null)
        return -1;
      if (logEntry2 == null)
        return 1;
      if (logEntry1.Date == null && logEntry2.Date == null)
        return 0;
      if (logEntry1.Date == null && logEntry2.Date != null)
        return 1;
      return logEntry1.Date != null && logEntry2.Date == null ? -1 : Convert.ToDateTime(logEntry1.Date).CompareTo(Convert.ToDateTime(logEntry2.Date));
    }
  }
}
