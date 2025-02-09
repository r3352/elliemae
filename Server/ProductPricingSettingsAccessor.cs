// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ProductPricingSettingsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class ProductPricingSettingsAccessor
  {
    private static string tableName = "ProductPricingSetting";

    public static List<ProductPricingSetting> UpdateProductPricingSettings(
      List<ProductPricingSetting> settings)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete " + ProductPricingSettingsAccessor.tableName);
      foreach (ProductPricingSetting setting in settings)
        dbQueryBuilder.AppendLine("Insert into " + ProductPricingSettingsAccessor.tableName + " (ProviderID, PartnerID, VendorPlatform, SettingsSection, PartnerName, AdminURL, MoreInfoURL, SupportsImportToLoan, SupportsPartnerRequestLock, SupportsPartnerLockConfirm, ShowSellSide, ImportToLoan, PartnerRequestLock, PartnerLockConfirm, Active, RequestLockOnlyWOCurrentLock, IsCustomizeInvestorName, UseCustomizedInvestorName, UseOnlyInvestorName, UseOnlyLenderName, UseInvestorAndLenderName, EnableAutoLockRequest, EnableAutoLockExtensionRequest, EnableAutoCancelLockRequest, SupportEnableAutoLockRequest, GetPricingRelock, EnableAutoLockRelock, IsExportUserFinished, ExcludeAutoLockLoanType, ExcludeAutoLockPropertyState, ExcludeAutoLockChannel, ExcludeAutoLockLoanPurpose, ExcludeAutoLockAmortizationType, ExcludeAutoLockPropertyWillBe, ExcludeAutoLockLienPosition, ExcludeAutoLockLoanProgram, ExcludeAutoLockPlanCode) Values (" + SQL.Encode((object) setting.ProviderID) + ", " + SQL.Encode((object) setting.PartnerID) + ", " + SQL.Encode((object) setting.VendorPlatform.ToString()) + ", " + SQL.Encode((object) setting.SettingsSection) + ", " + SQL.Encode((object) setting.PartnerName) + ", " + SQL.Encode((object) setting.AdminURL) + ", " + SQL.Encode((object) setting.MoreInfoURL) + ", " + (setting.SupportImportToLoan ? "1" : "0") + ", " + (setting.SupportPartnerRequestLock ? "1" : "0") + ", " + (setting.SupportPartnerLockConfirm ? "1" : "0") + ", " + (setting.ShowSellSide ? "1" : "0") + ", " + (setting.ImportToLoan ? "1" : "0") + ", " + (setting.PartnerRequestLock ? "1" : "0") + ", " + (setting.PartnerLockConfirm ? "1" : "0") + ", " + (setting.Active ? "1" : "0") + ", " + (setting.PartnerRequestLockWhenNoCurrentLock ? "1" : "0") + ", " + (setting.IsCustomizeInvestorName ? "1" : "0") + ", " + (setting.UseCustomizedInvestorName ? "1" : "0") + ", " + (setting.UseOnlyInvestorName ? "1" : "0") + ", " + (setting.UseOnlyLenderName ? "1" : "0") + ", " + (setting.UseInvestorAndLenderName ? "1" : "0") + ", " + (setting.EnableAutoLockRequest ? "1" : "0") + ", " + (setting.EnableAutoLockExtensionRequest ? "1" : "0") + ", " + (setting.EnableAutoCancelRequest ? "1" : "0") + ", " + (setting.SupportEnableAutoLockRequest ? "1" : "0") + "," + (setting.GetPricingRelock ? "1" : "0") + " , " + (setting.EnableAutoLockRelock ? "1" : "0") + " , " + (setting.IsExportUserFinished ? "1" : "0") + " , " + SQL.Encode((object) setting.ExcludeAutoLockLoanType) + " , " + SQL.Encode((object) setting.ExcludeAutoLockPropertyState) + " , " + SQL.Encode((object) setting.ExcludeAutoLockChannel) + " , " + SQL.Encode((object) setting.ExcludeAutoLockLoanPurpose) + " , " + SQL.Encode((object) setting.ExcludeAutoLockAmortizationType) + " , " + SQL.Encode((object) setting.ExcludeAutoLockPropertyWillBe) + " , " + SQL.Encode((object) setting.ExcludeAutoLockLienPosition) + " , " + SQL.Encode((object) setting.ExcludeAutoLockLoanProgram) + " , " + SQL.Encode((object) setting.ExcludeAutoLockPlanCode) + ")");
      dbQueryBuilder.ExecuteNonQuery();
      return ProductPricingSettingsAccessor.GetProductPricingSettings();
    }

    public static void ProductPricingExportUser(string providerId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update ProductPricingSetting set IsExportUserFinished = 1 where ProviderID = '" + providerId + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    private static DataTable GetTable(string whereClause = null)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * FROM " + ProductPricingSettingsAccessor.tableName);
        if (!string.IsNullOrEmpty(whereClause))
          pgDbQueryBuilder.AppendLine(" WHERE " + whereClause);
        return pgDbQueryBuilder.ExecuteTableQuery();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + ProductPricingSettingsAccessor.tableName);
      if (!string.IsNullOrEmpty(whereClause))
        dbQueryBuilder.AppendLine("WHERE " + whereClause);
      return dbQueryBuilder.ExecuteTableQuery();
    }

    [PgReady]
    public static List<ProductPricingSetting> GetProductPricingSettings()
    {
      IClientContext current = ClientContext.GetCurrent(false);
      string companySetting = Company.GetCompanySetting("POLICIES", "EPPSPartnerID");
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        DataTable table = ProductPricingSettingsAccessor.GetTable();
        List<ProductPricingSetting> productPricingSettings = new List<ProductPricingSetting>();
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          string str = string.Concat(row["VendorPlatform"]);
          VendorPlatform vendorPlatform = string.IsNullOrEmpty(str) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str, true);
          productPricingSettings.Add(new ProductPricingSetting(string.Concat(row["ProviderID"]), string.Concat(row["PartnerID"]), vendorPlatform, string.Concat(row["SettingsSection"]), string.Concat(row["PartnerName"]), string.Concat(row["AdminURL"]), string.Concat(row["MoreInfoURL"]), string.Concat(row["SupportsImportToLoan"]) == "True", string.Concat(row["SupportsPartnerRequestLock"]) == "True", string.Concat(row["SupportsPartnerLockConfirm"]) == "True", string.Concat(row["ShowSellSide"]) == "True", string.Concat(row["ImportToLoan"]) == "True", string.Concat(row["PartnerRequestLock"]) == "True", string.Concat(row["PartnerLockConfirm"]) == "True", string.Concat(row["Active"]) == "True", string.Concat(row["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(row["IsCustomizeInvestorName"]) == "True", string.Concat(row["UseCustomizedInvestorName"]) == "True", string.Concat(row["UseOnlyInvestorName"]) == "True", string.Concat(row["UseOnlyLenderName"]) == "True", string.Concat(row["UseInvestorAndLenderName"]) == "True", string.Concat(row["EnableAutoLockRequest"]) == "True", string.Concat(row["EnableAutoLockExtensionRequest"]) == "True", string.Concat(row["EnableAutoCancelLockRequest"]) == "True", string.Concat(row["SupportEnableAutoLockRequest"]) == "True", string.Concat(row["GetPricingRelock"]) == "True", string.Concat(row["EnableAutoLockRelock"]) == "True", string.Concat(row["IsExportUserFinished"]) == "False", string.Concat(row["ExcludeAutoLockLoanType"]), string.Concat(row["ExcludeAutoLockPropertyState"]), string.Concat(row["ExcludeAutoLockChannel"]), string.Concat(row["ExcludeAutoLockLoanPurpose"]), string.Concat(row["ExcludeAutoLockAmortizationType"]), string.Concat(row["ExcludeAutoLockPropertyWillBe"]), string.Concat(row["ExcludeAutoLockLienPosition"]), string.Concat(row["ExcludeAutoLockLoanProgram"]), string.Concat(row["ExcludeAutoLockPlanCode"]))
          {
            EppsPartnerId = vendorPlatform == VendorPlatform.EPC2 ? companySetting : string.Empty
          });
        }
        return productPricingSettings;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + ProductPricingSettingsAccessor.tableName);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<ProductPricingSetting> productPricingSettings1 = new List<ProductPricingSetting>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str = string.Concat(row["VendorPlatform"]);
        VendorPlatform vendorPlatform = string.IsNullOrEmpty(str) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str, true);
        productPricingSettings1.Add(new ProductPricingSetting(string.Concat(row["ProviderID"]), string.Concat(row["PartnerID"]), vendorPlatform, string.Concat(row["SettingsSection"]), string.Concat(row["PartnerName"]), string.Concat(row["AdminURL"]), string.Concat(row["MoreInfoURL"]), string.Concat(row["SupportsImportToLoan"]) == "True", string.Concat(row["SupportsPartnerRequestLock"]) == "True", string.Concat(row["SupportsPartnerLockConfirm"]) == "True", string.Concat(row["ShowSellSide"]) == "True", string.Concat(row["ImportToLoan"]) == "True", string.Concat(row["PartnerRequestLock"]) == "True", string.Concat(row["PartnerLockConfirm"]) == "True", string.Concat(row["Active"]) == "True", string.Concat(row["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(row["IsCustomizeInvestorName"]) == "True", string.Concat(row["UseCustomizedInvestorName"]) == "True", string.Concat(row["UseOnlyInvestorName"]) == "True", string.Concat(row["UseOnlyLenderName"]) == "True", string.Concat(row["UseInvestorAndLenderName"]) == "True", string.Concat(row["EnableAutoLockRequest"]) == "True", string.Concat(row["EnableAutoLockExtensionRequest"]) == "True", string.Concat(row["EnableAutoCancelLockRequest"]) == "True", string.Concat(row["SupportEnableAutoLockRequest"]) == "True", string.Concat(row["GetPricingRelock"]) == "True", string.Concat(row["EnableAutoLockRelock"]) == "True", string.Concat(row["IsExportUserFinished"]) == "True", string.Concat(row["ExcludeAutoLockLoanType"]), string.Concat(row["ExcludeAutoLockPropertyState"]), string.Concat(row["ExcludeAutoLockChannel"]), string.Concat(row["ExcludeAutoLockLoanPurpose"]), string.Concat(row["ExcludeAutoLockAmortizationType"]), string.Concat(row["ExcludeAutoLockPropertyWillBe"]), string.Concat(row["ExcludeAutoLockLienPosition"]), string.Concat(row["ExcludeAutoLockLoanProgram"]), string.Concat(row["ExcludeAutoLockPlanCode"]))
        {
          EppsPartnerId = vendorPlatform == VendorPlatform.EPC2 ? companySetting : string.Empty
        });
      }
      return productPricingSettings1;
    }

    [PgReady]
    public static ProductPricingSetting GetActiveProductPricingPartner()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        DataTable table = ProductPricingSettingsAccessor.GetTable("Active = 1");
        if (table.Rows.Count <= 0)
          return (ProductPricingSetting) null;
        string str = string.Concat(table.Rows[0]["VendorPlatform"]);
        VendorPlatform vendorPlatform = string.IsNullOrEmpty(str) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str, true);
        return new ProductPricingSetting(string.Concat(table.Rows[0]["ProviderID"]), string.Concat(table.Rows[0]["PartnerID"]), vendorPlatform, string.Concat(table.Rows[0]["SettingsSection"]), string.Concat(table.Rows[0]["PartnerName"]), string.Concat(table.Rows[0]["AdminURL"]), string.Concat(table.Rows[0]["MoreInfoURL"]), string.Concat(table.Rows[0]["SupportsImportToLoan"]) == "True", string.Concat(table.Rows[0]["SupportsPartnerRequestLock"]) == "True", string.Concat(table.Rows[0]["SupportsPartnerLockConfirm"]) == "True", string.Concat(table.Rows[0]["ShowSellSide"]) == "True", string.Concat(table.Rows[0]["ImportToLoan"]) == "True", string.Concat(table.Rows[0]["PartnerRequestLock"]) == "True", string.Concat(table.Rows[0]["PartnerLockConfirm"]) == "True", string.Concat(table.Rows[0]["Active"]) == "True", string.Concat(table.Rows[0]["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(table.Rows[0]["IsCustomizeInvestorName"]) == "True", string.Concat(table.Rows[0]["UseCustomizedInvestorName"]) == "True", string.Concat(table.Rows[0]["UseOnlyInvestorName"]) == "True", string.Concat(table.Rows[0]["UseOnlyLenderName"]) == "True", string.Concat(table.Rows[0]["UseInvestorAndLenderName"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockRequest"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockExtensionRequest"]) == "True", string.Concat(table.Rows[0]["EnableAutoCancelLockRequest"]) == "True", string.Concat(table.Rows[0]["SupportEnableAutoLockRequest"]) == "True", string.Concat(table.Rows[0]["GetPricingRelock"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockRelock"]) == "True", string.Concat(table.Rows[0]["IsExportUserFinished"]) == "False", string.Concat(table.Rows[0]["ExcludeAutoLockLoanType"]), string.Concat(table.Rows[0]["ExcludeAutoLockPropertyState"]), string.Concat(table.Rows[0]["ExcludeAutoLockChannel"]), string.Concat(table.Rows[0]["ExcludeAutoLockLoanPurpose"]), string.Concat(table.Rows[0]["ExcludeAutoLockAmortizationType"]), string.Concat(table.Rows[0]["ExcludeAutoLockPropertyWillBe"]), string.Concat(table.Rows[0]["ExcludeAutoLockLienPosition"]), string.Concat(table.Rows[0]["ExcludeAutoLockLoanProgram"]), string.Concat(table.Rows[0]["ExcludeAutoLockPlanCode"]))
        {
          EppsPartnerId = vendorPlatform == VendorPlatform.EPC2 ? Company.GetCompanySetting("POLICIES", "EPPSPartnerID") : string.Empty
        };
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + ProductPricingSettingsAccessor.tableName + " where Active = 1");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable.Rows.Count <= 0)
        return (ProductPricingSetting) null;
      string str1 = string.Concat(dataTable.Rows[0]["VendorPlatform"]);
      VendorPlatform vendorPlatform1 = string.IsNullOrEmpty(str1) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str1, true);
      return new ProductPricingSetting(string.Concat(dataTable.Rows[0]["ProviderID"]), string.Concat(dataTable.Rows[0]["PartnerID"]), vendorPlatform1, string.Concat(dataTable.Rows[0]["SettingsSection"]), string.Concat(dataTable.Rows[0]["PartnerName"]), string.Concat(dataTable.Rows[0]["AdminURL"]), string.Concat(dataTable.Rows[0]["MoreInfoURL"]), string.Concat(dataTable.Rows[0]["SupportsImportToLoan"]) == "True", string.Concat(dataTable.Rows[0]["SupportsPartnerRequestLock"]) == "True", string.Concat(dataTable.Rows[0]["SupportsPartnerLockConfirm"]) == "True", string.Concat(dataTable.Rows[0]["ShowSellSide"]) == "True", string.Concat(dataTable.Rows[0]["ImportToLoan"]) == "True", string.Concat(dataTable.Rows[0]["PartnerRequestLock"]) == "True", string.Concat(dataTable.Rows[0]["PartnerLockConfirm"]) == "True", string.Concat(dataTable.Rows[0]["Active"]) == "True", string.Concat(dataTable.Rows[0]["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(dataTable.Rows[0]["IsCustomizeInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseCustomizedInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseOnlyInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseOnlyLenderName"]) == "True", string.Concat(dataTable.Rows[0]["UseInvestorAndLenderName"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockExtensionRequest"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoCancelLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["SupportEnableAutoLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["GetPricingRelock"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockRelock"]) == "True", string.Concat(dataTable.Rows[0]["IsExportUserFinished"]) == "True", string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanType"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPropertyState"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockChannel"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanPurpose"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockAmortizationType"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPropertyWillBe"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLienPosition"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanProgram"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPlanCode"]))
      {
        EppsPartnerId = vendorPlatform1 == VendorPlatform.EPC2 ? Company.GetCompanySetting("POLICIES", "EPPSPartnerID") : string.Empty
      };
    }

    [PgReady]
    public static ProductPricingSetting GetProductPricingPartner(string partnerID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        DataTable table = ProductPricingSettingsAccessor.GetTable("PartnerID = " + SQL.Encode((object) partnerID));
        if (table.Rows.Count <= 0)
          return (ProductPricingSetting) null;
        string str = string.Concat(table.Rows[0]["VendorPlatform"]);
        VendorPlatform vendorPlatform = string.IsNullOrEmpty(str) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str, true);
        return new ProductPricingSetting(string.Concat(table.Rows[0]["ProviderID"]), string.Concat(table.Rows[0]["PartnerID"]), vendorPlatform, string.Concat(table.Rows[0]["SettingsSection"]), string.Concat(table.Rows[0]["PartnerName"]), string.Concat(table.Rows[0]["AdminURL"]), string.Concat(table.Rows[0]["MoreInfoURL"]), string.Concat(table.Rows[0]["SupportsImportToLoan"]) == "True", string.Concat(table.Rows[0]["SupportsPartnerRequestLock"]) == "True", string.Concat(table.Rows[0]["SupportsPartnerLockConfirm"]) == "True", string.Concat(table.Rows[0]["ShowSellSide"]) == "True", string.Concat(table.Rows[0]["ImportToLoan"]) == "True", string.Concat(table.Rows[0]["PartnerRequestLock"]) == "True", string.Concat(table.Rows[0]["PartnerLockConfirm"]) == "True", string.Concat(table.Rows[0]["Active"]) == "True", string.Concat(table.Rows[0]["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(table.Rows[0]["IsCustomizeInvestorName"]) == "True", string.Concat(table.Rows[0]["UseCustomizedInvestorName"]) == "True", string.Concat(table.Rows[0]["UseOnlyInvestorName"]) == "True", string.Concat(table.Rows[0]["UseOnlyLenderName"]) == "True", string.Concat(table.Rows[0]["UseInvestorAndLenderName"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockRequest"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockExtensionRequest"]) == "True", string.Concat(table.Rows[0]["EnableAutoCancelLockRequest"]) == "True", string.Concat(table.Rows[0]["SupportEnableAutoLockRequest"]) == "True", string.Concat(table.Rows[0]["GetPricingRelock"]) == "True", string.Concat(table.Rows[0]["EnableAutoLockRelock"]) == "True", string.Concat(table.Rows[0]["IsExportUserFinished"]) == "False", string.Concat(table.Rows[0]["ExcludeAutoLockLoanType"]), string.Concat(table.Rows[0]["ExcludeAutoLockPropertyState"]), string.Concat(table.Rows[0]["ExcludeAutoLockChannel"]), string.Concat(table.Rows[0]["ExcludeAutoLockLoanPurpose"]), string.Concat(table.Rows[0]["ExcludeAutoLockAmortizationType"]), string.Concat(table.Rows[0]["ExcludeAutoLockPropertyWillBe"]), string.Concat(table.Rows[0]["ExcludeAutoLockLienPosition"]), string.Concat(table.Rows[0]["ExcludeAutoLockLoanProgram"]), string.Concat(table.Rows[0]["ExcludeAutoLockPlanCode"]))
        {
          EppsPartnerId = vendorPlatform == VendorPlatform.EPC2 ? Company.GetCompanySetting("POLICIES", "EPPSPartnerID") : string.Empty
        };
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from " + ProductPricingSettingsAccessor.tableName + " where PartnerID = " + SQL.Encode((object) partnerID));
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable.Rows.Count <= 0)
        return (ProductPricingSetting) null;
      string str1 = string.Concat(dataTable.Rows[0]["VendorPlatform"]);
      VendorPlatform vendorPlatform1 = string.IsNullOrEmpty(str1) ? VendorPlatform.EMN : (VendorPlatform) Enum.Parse(typeof (VendorPlatform), str1, true);
      return new ProductPricingSetting(string.Concat(dataTable.Rows[0]["ProviderID"]), string.Concat(dataTable.Rows[0]["PartnerID"]), vendorPlatform1, string.Concat(dataTable.Rows[0]["SettingsSection"]), string.Concat(dataTable.Rows[0]["PartnerName"]), string.Concat(dataTable.Rows[0]["AdminURL"]), string.Concat(dataTable.Rows[0]["MoreInfoURL"]), string.Concat(dataTable.Rows[0]["SupportsImportToLoan"]) == "True", string.Concat(dataTable.Rows[0]["SupportsPartnerRequestLock"]) == "True", string.Concat(dataTable.Rows[0]["SupportsPartnerLockConfirm"]) == "True", string.Concat(dataTable.Rows[0]["ShowSellSide"]) == "True", string.Concat(dataTable.Rows[0]["ImportToLoan"]) == "True", string.Concat(dataTable.Rows[0]["PartnerRequestLock"]) == "True", string.Concat(dataTable.Rows[0]["PartnerLockConfirm"]) == "True", string.Concat(dataTable.Rows[0]["Active"]) == "True", string.Concat(dataTable.Rows[0]["RequestLockOnlyWOCurrentLock"]) == "True", string.Concat(dataTable.Rows[0]["IsCustomizeInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseCustomizedInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseOnlyInvestorName"]) == "True", string.Concat(dataTable.Rows[0]["UseOnlyLenderName"]) == "True", string.Concat(dataTable.Rows[0]["UseInvestorAndLenderName"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockExtensionRequest"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoCancelLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["SupportEnableAutoLockRequest"]) == "True", string.Concat(dataTable.Rows[0]["GetPricingRelock"]) == "True", string.Concat(dataTable.Rows[0]["EnableAutoLockRelock"]) == "True", string.Concat(dataTable.Rows[0]["IsExportUserFinished"]) == "True", string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanType"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPropertyState"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockChannel"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanPurpose"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockAmortizationType"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPropertyWillBe"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLienPosition"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockLoanProgram"]), string.Concat(dataTable.Rows[0]["ExcludeAutoLockPlanCode"]))
      {
        EppsPartnerId = vendorPlatform1 == VendorPlatform.EPC2 ? Company.GetCompanySetting("POLICIES", "EPPSPartnerID") : string.Empty
      };
    }
  }
}
