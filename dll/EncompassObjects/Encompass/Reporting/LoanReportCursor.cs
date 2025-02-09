// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.LoanReportCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  public class LoanReportCursor : Cursor, ILoanReportCursor
  {
    private QueryResult.ColumnNameCollection columns;

    internal LoanReportCursor(Session session, QueryCursor cursor)
      : base(session, (ICursor) cursor)
    {
      this.columns = cursor.Columns;
    }

    public LoanReportData GetItem(int index) => (LoanReportData) base.GetItem(index);

    public LoanReportDataList GetItems(int startIndex, int count)
    {
      return (LoanReportDataList) base.GetItems(startIndex, count);
    }

    internal override object ConvertToItemType(object item)
    {
      return (object) new LoanReportData(this.columns, (object[]) item);
    }

    internal override ListBase ConvertToItemList(object[] items)
    {
      return (ListBase) LoanReportData.ToList(this.columns, items);
    }
  }
}
