// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogDocumentOrders
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogDocumentOrders : LoanLogEntryCollection, IEnumerable
  {
    internal LogDocumentOrders(Loan loan)
      : base(loan, typeof (DocumentOrderLog))
    {
    }

    public DocumentOrder this[int index] => (DocumentOrder) this.LogEntries[index];

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

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new DocumentOrder(this.Loan, (DocumentOrderLog) logRecord);
    }
  }
}
