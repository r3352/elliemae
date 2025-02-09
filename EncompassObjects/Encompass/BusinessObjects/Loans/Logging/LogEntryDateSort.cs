// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryDateSort
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides a sorting comparison for LogEntry items based on date.
  /// </summary>
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
