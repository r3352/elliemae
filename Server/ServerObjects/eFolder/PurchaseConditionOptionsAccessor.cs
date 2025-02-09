// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.PurchaseConditionOptionsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class PurchaseConditionOptionsAccessor
  {
    private const string sCategorytableName = "PurchaseConditionCategories�";
    private const string sStatustableName = "PurchaseConditionStatuses�";

    private PurchaseConditionOptionsAccessor()
    {
    }

    public static PurchaseConditionCategory GetCategory(string categoryName)
    {
      PurchaseConditionCategory category = (PurchaseConditionCategory) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from PurchaseConditionCategories where [CategoryName] = " + SQL.EncodeString(categoryName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        category = new PurchaseConditionCategory()
        {
          CategoryId = Convert.ToInt32(dataRowCollection[0]["CategoryId"]),
          CategoryName = Convert.ToString(dataRowCollection[0]["CategoryName"])
        };
        category.SubCategories = PurchaseConditionOptionsAccessor.GetSubCategories(category.CategoryId);
      }
      return category;
    }

    public static List<PurchaseConditionCategory> GetAllCategories()
    {
      List<PurchaseConditionCategory> allCategories = new List<PurchaseConditionCategory>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from PurchaseConditionCategories where ParentID = " + (object) 0);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          PurchaseConditionCategory conditionCategory = new PurchaseConditionCategory();
          List<string> stringList = new List<string>();
          conditionCategory.CategoryId = Convert.ToInt32(dataRow["CategoryId"]);
          conditionCategory.CategoryName = Convert.ToString(dataRow["CategoryName"]);
          conditionCategory.SubCategories = PurchaseConditionOptionsAccessor.GetSubCategories(conditionCategory.CategoryId);
          allCategories.Add(conditionCategory);
        }
      }
      return allCategories;
    }

    public static List<PurchaseConditionCategory> GetSubCategories(int parentCategoryId)
    {
      List<PurchaseConditionCategory> subCategories = new List<PurchaseConditionCategory>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from PurchaseConditionCategories where ParentID = " + (object) parentCategoryId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          subCategories.Add(new PurchaseConditionCategory()
          {
            CategoryId = Convert.ToInt32(dataRow["CategoryId"]),
            CategoryName = Convert.ToString(dataRow["CategoryName"])
          });
      }
      return subCategories;
    }

    public static PurchaseConditionStatus GetStatus(string statusName)
    {
      PurchaseConditionStatus status = (PurchaseConditionStatus) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from PurchaseConditionStatuses where [StatusName] = " + SQL.EncodeString(statusName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        status = new PurchaseConditionStatus();
        status.StatusName = Convert.ToString(dataRowCollection[0]["StatusName"]);
        status.StatusInterpretation = Convert.ToString(dataRowCollection[0]["StatusInterpretation"]);
        status.StatusOrder = Convert.ToInt32(dataRowCollection[0]["DisplayOrder"]);
        status.CanBeDeleted = !dataRowCollection[0]["CanBeDeleted"].Equals((object) DBNull.Value) && Convert.ToBoolean(dataRowCollection[0]["CanBeDeleted"]);
      }
      return status;
    }

    public static List<PurchaseConditionStatus> GetAllStatuses()
    {
      List<PurchaseConditionStatus> allStatuses = new List<PurchaseConditionStatus>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from PurchaseConditionStatuses");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          allStatuses.Add(new PurchaseConditionStatus()
          {
            StatusName = Convert.ToString(dataRow["StatusName"]),
            StatusInterpretation = Convert.ToString(dataRow["StatusInterpretation"]),
            StatusOrder = Convert.ToInt32(dataRow["DisplayOrder"]),
            CanBeDeleted = !dataRow["CanBeDeleted"].Equals((object) DBNull.Value) && Convert.ToBoolean(dataRow["CanBeDeleted"])
          });
      }
      return allStatuses;
    }

    public static int CreatePurchaseConditionCategory(
      PurchaseConditionCategory purchaseConditionCategory)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      StringBuilder stringBuilder = new StringBuilder();
      string categoryName = purchaseConditionCategory.CategoryName;
      if (string.IsNullOrWhiteSpace(categoryName))
        throw new Exception("Category name cannot be blank. Category was not added.");
      stringBuilder.AppendLine("DECLARE @newId int");
      stringBuilder.AppendLine("IF EXISTS(SELECT 1 FROM PurchaseConditionCategories WHERE CategoryName = '" + categoryName + "')");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine(" SELECT -1");
      stringBuilder.AppendLine("END");
      stringBuilder.AppendLine("ELSE");
      stringBuilder.AppendLine("BEGIN");
      stringBuilder.AppendLine(" INSERT INTO PurchaseConditionCategories (ParentID, CategoryName) VALUES ( 0, '" + categoryName + "')");
      if (purchaseConditionCategory.SubCategories.Count > 0)
      {
        stringBuilder.AppendLine(" SET @newId = @@IDENTITY");
        foreach (PurchaseConditionCategory subCategory in purchaseConditionCategory.SubCategories)
          stringBuilder.AppendLine(" INSERT INTO PurchaseConditionCategories (ParentID, CategoryName) VALUES ( @newId, '" + subCategory.CategoryName + "')");
      }
      stringBuilder.AppendLine("END");
      stringBuilder.AppendLine("SELECT @newId");
      dbQueryBuilder.Append(stringBuilder.ToString());
      int num = -1;
      object obj = dbQueryBuilder.ExecuteScalar();
      if (obj != null && obj is int)
        num = Utils.ParseInt(obj);
      return num != -1 ? num : throw new Exception(string.Format("Category {0} is already in the database. Category was not added.", (object) categoryName));
    }
  }
}
