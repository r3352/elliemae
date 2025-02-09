// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceProductDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class ServiceProductDbAccessor
  {
    private const string ServiceProductTable = "ServiceProducts�";
    private const string ProductColumnProductID = "SvcProductID�";
    private const string ProductColumnProductName = "ProductName�";
    private const string ProductColumnPartnerID = "PartnerID�";
    private const string ProductColumnVersion = "Version�";
    private const string ProductColumnCategory = "Category�";
    private const string ProductColumnName = "Name�";
    private const string ProductColumnDescription = "Description�";
    private const string ProductColumnLastModifiedByUserId = "LastModifiedByUserId�";
    private const string ProductColumnLastModified = "LastModified�";

    public List<ServiceProduct> GetServiceProducts(
      string productName = null,
      string partnerId = null,
      string version = null)
    {
      List<ServiceProduct> serviceProducts = new List<ServiceProduct>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT \r\n                                            SvcProductID,\r\n                                            ProductName,\r\n                                            PartnerID,\r\n                                            Version,\r\n                                            Category,\r\n                                            Name,\r\n                                            Description,\r\n                                            LastModifiedByUserId,\r\n                                            LastModified,\r\n                                            OrderType \r\n                                           FROM {0} ", (object) "ServiceProducts"));
      if (!string.IsNullOrEmpty(productName) || !string.IsNullOrEmpty(partnerId) || !string.IsNullOrEmpty(version))
      {
        bool flag = false;
        dbQueryBuilder.AppendLine("WHERE ");
        if (!string.IsNullOrEmpty(productName))
        {
          dbQueryBuilder.AppendLine("ProductName=" + SQL.Encode((object) productName));
          flag = true;
        }
        if (!string.IsNullOrEmpty(partnerId))
        {
          dbQueryBuilder.AppendLine(flag ? "AND PartnerID=" + SQL.Encode((object) partnerId) : "PartnerID=" + SQL.Encode((object) partnerId));
          flag = true;
        }
        if (!string.IsNullOrEmpty(version))
          dbQueryBuilder.AppendLine(flag ? "AND Version=" + SQL.Encode((object) version) : "Version=" + SQL.Encode((object) version));
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        return serviceProducts;
      DataTable table = dataSet.Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        serviceProducts.Add(this.ConvertDataRowToProduct(row));
      }
      return serviceProducts;
    }

    public ServiceProduct GetServiceProductById(Guid productID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProducts");
      DbValue key = new DbValue("SvcProductID", (object) productID.ToString());
      dbQueryBuilder.SelectFrom(table, key);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      return dataSet == null ? (ServiceProduct) null : this.ConvertDataRowToProduct(dataSet.Tables, 0) ?? (ServiceProduct) null;
    }

    private ServiceProduct ConvertDataRowToProduct(DataTableCollection tables, int index)
    {
      if (tables == null || tables.Count <= index)
        return (ServiceProduct) null;
      DataTable table = tables[index];
      return table.Rows == null || table.Rows.Count <= 0 ? (ServiceProduct) null : this.ConvertDataRowToProduct(table.Rows[0]);
    }

    private ServiceProduct ConvertDataRowToProduct(DataRow row)
    {
      if (row == null)
        return (ServiceProduct) null;
      ServiceProduct product = new ServiceProduct();
      product.SvcProductID = (Guid) row["SvcProductID"];
      product.ProductName = (string) row["ProductName"];
      product.PartnerID = (string) row["PartnerID"];
      if (row["Version"] != DBNull.Value)
        product.Version = (string) row["Version"];
      product.Category = (string) row["Category"];
      if (row["Name"] != DBNull.Value)
        product.Name = (string) row["Name"];
      product.Description = row["Description"] == DBNull.Value ? string.Empty : (string) row["Description"];
      product.LastModifiedByUserId = row["LastModifiedByUserId"] == DBNull.Value ? string.Empty : (string) row["LastModifiedByUserId"];
      product.LastModified = (DateTime) row["LastModified"];
      return product;
    }

    public void CreateServiceProduct(ServiceProduct serviceProduct)
    {
      if (serviceProduct.SvcProductID == Guid.Empty)
        serviceProduct.SvcProductID = Guid.NewGuid();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProducts");
      DbValueList dbValueList = this.GetDBValueList(serviceProduct, true);
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void UpdateServiceProduct(ServiceProduct serviceProduct)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProducts");
      DbValue key = new DbValue("SvcProductID", (object) serviceProduct.SvcProductID.ToString());
      DbValueList dbValueList = this.GetDBValueList(serviceProduct);
      dbQueryBuilder.Update(table, dbValueList, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void DeleteServiceProduct(Guid productID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProducts");
      DbValue key = new DbValue("SvcProductID", (object) productID.ToString());
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private DbValueList GetDBValueList(ServiceProduct product, bool forCreate = false)
    {
      DbValueList dbValueList = new DbValueList();
      if (forCreate)
      {
        dbValueList.Add("SvcProductID", (object) product.SvcProductID.ToString());
        dbValueList.Add("ProductName", (object) product.ProductName);
        dbValueList.Add("PartnerID", (object) product.PartnerID);
        dbValueList.Add("Version", (object) product.Version);
        dbValueList.Add("Category", (object) product.Category);
      }
      dbValueList.Add("Name", (object) product.Name);
      dbValueList.Add("Description", (object) product.Description);
      dbValueList.Add("LastModifiedByUserId", (object) product.LastModifiedByUserId);
      dbValueList.Add("LastModified", (object) product.LastModified);
      return dbValueList;
    }

    public Guid GetServiceProductIdForDuplicateCheck(
      string productName,
      string partnerId,
      string version)
    {
      if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(partnerId))
        return Guid.Empty;
      DbValueList keys = new DbValueList();
      keys.Add("ProductName", (object) productName);
      keys.Add("PartnerID", (object) partnerId);
      keys.Add("Version", (object) version);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProducts");
      string[] columnNames = new string[1]{ "SvcProductID" };
      dbQueryBuilder.SelectFrom(table, columnNames, keys);
      object obj = dbQueryBuilder.ExecuteScalar();
      return obj == null ? Guid.Empty : (Guid) obj;
    }
  }
}
