// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ConfigurableWorkFlowDBAccess
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class ConfigurableWorkFlowDBAccess
  {
    public static ConfigurableWorkflowCategories GetConfigurableWorkflowByChannel(
      WorkflowChannel channel)
    {
      ConfigurableWorkflowCategories categories = new ConfigurableWorkflowCategories(channel);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from ConfigurableWorkflowType where WorkflowId = " + (object) (int) channel + "\n");
      dbQueryBuilder.Append("select * from ConfigurableWorkflowCategory where WorkflowId = " + (object) (int) channel + " order by CategorySeq\n");
      dbQueryBuilder.Append("select * from ConfigurableWorkflowCategory cwc inner join ConfigurableWorkflowSubCategory cwsc on cwc.guid = cwsc.CategoryGuid where cwc.WorkflowId = " + (object) (int) channel + " order by cwc.CategorySeq, cwsc.SubCategorySeq");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables.Count > 0)
        categories.Active = dataSet.Tables[0].Rows[0]["Active"].ToString().ToLower() == "true";
      if (dataSet.Tables.Count > 1)
        ConfigurableWorkFlowDBAccess.GetCategories(dataSet.Tables[1], categories);
      if (dataSet.Tables.Count > 2)
      {
        foreach (ConfigurableWorkflowCategory workflowCategory in categories.Item)
          ConfigurableWorkFlowDBAccess.GetSubCategories(dataSet.Tables[2], workflowCategory.SubCategories, workflowCategory.GUID);
      }
      return categories;
    }

    public static void UpdateConfigurableWorkflowByChannel(
      WorkflowChannel channel,
      ConfigurableWorkflowCategories categories)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      dbQueryBuilder2.Append("Delete from ConfigurableWorkflowSubCategory where CategoryGUID in (select guid from ConfigurableWorkflowCategory where WorkflowId = " + (object) (int) channel + ");\n");
      dbQueryBuilder2.Append("Delete from ConfigurableWorkflowCategory where WorkflowId = " + (object) (int) channel + ";\n");
      dbQueryBuilder2.Append("Update ConfigurableWorkflowType set active = '" + (categories.Active ? 1 : 0).ToString() + "' where WorkflowId = " + (object) (int) channel + ";\n");
      foreach (ConfigurableWorkflowCategory workflowCategory in categories.Item)
      {
        string str = Guid.NewGuid().ToString();
        dbQueryBuilder2.Append("Insert into ConfigurableWorkflowCategory(WorkFlowId, Guid, Categoryname, CategorySeq) values('" + (object) (int) channel + "' ,'" + str + "','" + SQL.EncodeString(workflowCategory.CategoryName, false) + "'," + (object) workflowCategory.SequenceNum + ");\n");
        foreach (ConfigurableWorkflowSubCategory subCategory in workflowCategory.SubCategories)
          dbQueryBuilder2.Append("Insert into ConfigurableWorkflowSubCategory(CategoryGUID, SubCategoryname, SubCategorySeq, FieldId, eFolder1Id, eFolder2Id) values('" + str + "', '" + SQL.EncodeString(subCategory.SubCategoryName, false) + "' ," + (object) subCategory.SequenceNum + ",'" + subCategory.FieldId + "','" + subCategory.EFolder1Id + "','" + subCategory.EFolder2Id + "');\n");
      }
      dbQueryBuilder2.ExecuteSetQuery();
    }

    private static void GetCategories(DataTable data, ConfigurableWorkflowCategories categories)
    {
      if (data.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        categories.Item.Add(new ConfigurableWorkflowCategory()
        {
          GUID = (string) row["guid"],
          CategoryName = SQL.DecodeString((object) row["CategoryName"].ToString()),
          SequenceNum = (int) row["CategorySeq"]
        });
    }

    private static void GetSubCategories(
      DataTable table,
      List<ConfigurableWorkflowSubCategory> subCatgories,
      string catGUID)
    {
      if (table.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        if (((string) row["GUID"]).ToLower() == catGUID.ToLower())
          subCatgories.Add(new ConfigurableWorkflowSubCategory()
          {
            FieldId = row["FieldId"].ToString(),
            SequenceNum = (int) row["SubCategorySeq"],
            SubCategoryName = SQL.DecodeString((object) row["SubCategoryName"].ToString()),
            EFolder1Id = row["eFolder1Id"].ToString(),
            EFolder2Id = row["eFolder2Id"].ToString()
          });
      }
    }

    private static void GetSubCategories1(
      DataTable table,
      List<ConfigurableWorkflowCategory> categories,
      List<ConfigurableWorkflowSubCategory> subCatgories,
      string catGUID)
    {
      if (table.Rows.Count <= 0)
        return;
      foreach (DataRow row1 in (InternalDataCollectionBase) table.Rows)
      {
        DataRow row = row1;
        ConfigurableWorkflowCategory workflowCategory = categories.Where<ConfigurableWorkflowCategory>((System.Func<ConfigurableWorkflowCategory, bool>) (x => x.GUID.ToLower() == ((string) row["GUID"]).ToLower())).FirstOrDefault<ConfigurableWorkflowCategory>();
        if (string.IsNullOrEmpty(workflowCategory.GUID))
        {
          workflowCategory.GUID = (string) row["guid"];
          workflowCategory.CategoryName = SQL.DecodeString((object) row["CategoryName"].ToString());
          workflowCategory.SequenceNum = (int) row["CategorySeq"];
          categories.Add(workflowCategory);
        }
        workflowCategory.SubCategories.Add(new ConfigurableWorkflowSubCategory()
        {
          FieldId = row["FieldId"].ToString(),
          SequenceNum = (int) row["SubCategorySeq"],
          SubCategoryName = SQL.DecodeString((object) row["SubCategoryName"].ToString()),
          EFolder1Id = row["eFolder1Id"].ToString(),
          EFolder2Id = row["eFolder2Id"].ToString()
        });
      }
    }
  }
}
