// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDocumentOrders
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogDocumentOrders : LoanLogEntryCollection, IEnumerable
  {
    internal LogDocumentOrders(Loan loan)
      : base(loan, typeof (DocumentOrderLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" /> from the collection based on its index.
    /// </summary>
    public DocumentOrder this[int index] => (DocumentOrder) this.LogEntries[index];

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" /> that represents the most recent
    /// order of the given type.
    /// </summary>
    /// <param name="orderType">The type of order that was placed.</param>
    /// <returns>The most recent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" /> of the given
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrderType" /> is returned. If not orders exist of the given
    /// type, <c>null</c> is returned.</returns>
    public DocumentOrder GetMostRecentOrder(DocumentOrderType orderType)
    {
      DocumentOrder mostRecentOrder = (DocumentOrder) null;
      foreach (DocumentOrder documentOrder in (LoanLogEntryCollection) this)
      {
        if ((documentOrder.OrderType & orderType) != DocumentOrderType.None && (mostRecentOrder == null || (DateTime) documentOrder.Date > (DateTime) mostRecentOrder.Date))
          mostRecentOrder = documentOrder;
      }
      return mostRecentOrder;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new DocumentOrder(this.Loan, (DocumentOrderLog) logRecord);
    }
  }
}
