// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.TrendChartTooltipRenderer
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using Infragistics.UltraChart.Resources;
using System.Collections;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class TrendChartTooltipRenderer : IRenderLabel
  {
    private DataTable dataTable;
    private string formatString = ":#,##0.000";

    public void SetDataTable(DataTable dataTable) => this.dataTable = dataTable;

    public void SetFormatString(string formatString)
    {
      this.formatString = "{0:" + formatString + "}";
    }

    public string ToString(Hashtable context)
    {
      string str1 = (string) context[(object) "SERIES_LABEL"];
      double num = (double) context[(object) "DATA_VALUE"];
      if (this.dataTable == null)
        return str1 + ": " + string.Format(this.formatString, (object) num);
      int columnIndex = (int) context[(object) "DATA_COLUMN"];
      string str2 = string.Format("{0:0.0000}", (object) num);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DataRow row in (InternalDataCollectionBase) this.dataTable.Rows)
      {
        if (str2 == string.Format("{0:0.0000}", row[columnIndex]))
        {
          if (0 < stringBuilder.Length)
            stringBuilder.Append(", ");
          stringBuilder.Append(row[0].ToString());
        }
      }
      stringBuilder.Append(": " + string.Format(this.formatString, (object) num));
      return stringBuilder.ToString();
    }
  }
}
