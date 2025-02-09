// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaeProductGrid
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class FannieMaeProductGrid
  {
    public DataTable ConvertSelectedProductName(GVItemCollection gvItems)
    {
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add("ProductName", typeof (string));
      dataTable.Columns.Add("DisplayName", typeof (string));
      dataTable.Columns.Add("Description", typeof (string));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gvItems)
      {
        if (gvItem.Selected)
        {
          DataRow row = dataTable.NewRow();
          row["ProductName"] = (object) (string) gvItem.SubItems[0].Value;
          row["DisplayName"] = (object) (string) gvItem.SubItems[1].Value;
          row["Description"] = (object) (string) gvItem.SubItems[2].Value;
          dataTable.Rows.Add(row);
        }
      }
      return dataTable;
    }

    public DataTable ConvertProductName(GVItemCollection gvItems)
    {
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add("ProductName", typeof (string));
      dataTable.Columns.Add("DisplayName", typeof (string));
      dataTable.Columns.Add("Description", typeof (string));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gvItems)
      {
        DataRow row = dataTable.NewRow();
        row["ProductName"] = (object) (string) gvItem.SubItems[0].Value;
        row["DisplayName"] = (object) (string) gvItem.SubItems[1].Value;
        row["Description"] = (object) (string) gvItem.SubItems[2].Value;
        dataTable.Rows.Add(row);
      }
      return dataTable;
    }

    public GVItem[] ConvertProductName(DataTable fannieMaeProductNamesTable)
    {
      int num = 0;
      GVItem[] gvItemArray = new GVItem[fannieMaeProductNamesTable.Rows.Count];
      foreach (DataRow row in (InternalDataCollectionBase) fannieMaeProductNamesTable.Rows)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add(new GVSubItem(row["ProductName"]));
        gvItem.SubItems.Add(new GVSubItem(row["DisplayName"]));
        gvItem.SubItems.Add(new GVSubItem(row["Description"]));
        FannieMaeProduct fannieMaeProduct = new FannieMaeProduct()
        {
          ProductName = row["ProductName"].ToString(),
          DisplayName = row["DisplayName"].ToString(),
          ProductDescription = row["Description"].ToString()
        };
        gvItem.Tag = (object) fannieMaeProduct;
        gvItemArray[num++] = gvItem;
      }
      return gvItemArray;
    }

    public GVItem[] ConvertProductName(FannieMaeProducts products)
    {
      int num = 0;
      GVItem[] gvItemArray = new GVItem[products.Count];
      foreach (FannieMaeProduct product in products)
        gvItemArray[num++] = new GVItem()
        {
          SubItems = {
            new GVSubItem((object) product.ProductName),
            new GVSubItem((object) product.DisplayName),
            new GVSubItem((object) product.ProductDescription)
          }
        };
      return gvItemArray;
    }
  }
}
