// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.LoanReportCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>
  /// Represents a Cursor used for fast retrieval and iteration over a set of
  /// <see cref="T:EllieMae.Encompass.Reporting.LoanReportData" /> objects.
  /// </summary>
  public class LoanReportCursor : Cursor, ILoanReportCursor
  {
    private QueryResult.ColumnNameCollection columns;

    internal LoanReportCursor(Session session, QueryCursor cursor)
      : base(session, (ICursor) cursor)
    {
      this.columns = cursor.Columns;
    }

    /// <summary>
    /// Retrieves the item from the cursor at the specified index.
    /// </summary>
    /// <param name="index">Index of the item to be retrieved (with 0 as the first
    /// index).</param>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.Reporting.LoanReportData" /> object.</returns>
    public LoanReportData GetItem(int index) => (LoanReportData) base.GetItem(index);

    /// <summary>
    /// Retrieves a subset of the cursor items starting at a specified index.
    /// </summary>
    /// <param name="startIndex">The index at which to start the subset.</param>
    /// <param name="count">The number of items to retrieve</param>
    /// <returns>Returns an array containing the <see cref="T:EllieMae.EMLite.DataEngine.PipelineData" /> objects
    /// within the specified range</returns>
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
