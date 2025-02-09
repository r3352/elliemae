// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SystemConfigurationAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Server.ServerObjects.HtmlEmail;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class SystemConfigurationAccessor
  {
    private const string className = "SystemConfigurationAccessor�";
    private const string ESCROW_FEE = "EscrowFee�";
    private const string ESCROW_FEE_RATE_LIST = "EscrowFeeRateList�";
    private const string TITLE_FEE_TABLE = "TitleFee�";
    private const string TITLE_FEE_RATE_LIST_TABLE = "TitleFeeRateList�";
    private const string HELOC_TABLE = "HelocTable�";
    private const string HELOC_TABLE_DETAILS = "HelocTableDetails�";
    private const string CITY_TAX_TABLE = "CityTaxFee�";
    private const string STATE_TAX_TABLE = "StateTaxFee�";
    private const string FEE_USER_LIST = "UserDefinedFee�";
    private const string LOAN_DUPLICATION_TEMPLATES = "LoanDuplicationTemplates�";
    private const string LOAN_DUPLICATION_ADDITIONAL_FIELDS = "LoanDuplicationAdditionalFields�";
    private const string DOCUMENT_STACKING_TEMPLATE = "DocumentStackingTemplate�";
    private const string DOCUMENT_STACKING_TEMPLATE_ELEMENTS = "DocumentStackingTemplateElements�";
    private const string PIGGYBACK_FIELDS = "PiggybackFields�";
    private const string fixDocumentStackingTemplatesCacheName = "CachedFixDocumentStackinsgTemplatesFlag�";
    private const string changeCircumstanceCacheName = "CachedChangeCircumstanceOptions�";
    private const string zipcodeSettingCacheName = "CachedZipcodeUserDefined�";
    private const string DOCUMENT_EXPORT_TEMPLATE = "DocumentExportTemplates�";
    private const string DOCUMENT_GROUP = "DocumentGroup�";
    private const string DOCUMENT_GROUP_LIST = "DocumentGroupList�";
    private const string HTML_EMAIL_TEMPLATE = "HtmlEmailTemplates�";
    private const string ADJUSTMENT_TEMPLATE = "AdjustmentTemplate�";
    private const string ADJUSTMENT_TEMPLATE_PRICE_ADJUSTMENT = "AdjustmentTemplatePriceAdjustment�";
    private const string ADJUSTMENT_TEMPLATE_CRITERION = "AdjustmentTemplateCriterion�";
    private const string SRP_TEMPLATE = "SRPTemplates�";
    private const string SRP_TEMPLATE_PRICING_ITEM = "SRPTemplatePricingItems�";
    private const string SRP_TEMPLATE_STATE_ADJUSTMENT = "SRPTemplateStateAdjustments�";
    private const string FUNDING_TEMPLATE = "FundingTemplates�";
    private const string FUNDING_TEMPLATE_FIELD = "FundingTemplateFields�";
    private const string INVESTOR_TEMPLATES = "InvestorTemplates�";
    private const string INVESTOR_CONTACT_INFORMATION = "InvestorContactInformation�";
    private const string PURCHASE_ADVICE_TEMPLATE = "PurchaseAdviceTemplate�";
    private const string PURCHASE_ADVICE_TEMPLATE_FIELD = "PurchaseAdviceTemplateField�";
    private const string AFFILIATED_BUSINESS_ARRANGEMENTS_TEMPLATES = "AffiliatedBusinessArrangementsTemplates�";
    private const string AFFILIATED_FIELDS = "AffiliatedFields�";
    private const string CUSTOM_TOOLS = "CustomTools�";
    private const string LOConnect_CustomServices = "[Acl_LoConnectCustomServices]�";
    private const string LOConnect_CustomServices_User = "[Acl_LoConnectCustomServices_User]�";
    private const string DocumentGroupConfigurationCacheName = "DocumentGroupConfiguration�";

    private SystemConfigurationAccessor()
    {
    }

    public static BinaryObject GetSystemSettings(string name)
    {
      if (name.Equals(typeof (TblEscrowPurList).Name))
      {
        TableFeeListBase tableFeeListBase = (TableFeeListBase) new TblEscrowPurList();
        return SystemConfigurationAccessor.GetEscrowTableFeeDetails(name, TableFeeType.EscrowPurchase, tableFeeListBase);
      }
      if (name.Equals(typeof (TblEscrowRefiList).Name))
      {
        TableFeeListBase tableFeeListBase = (TableFeeListBase) new TblEscrowRefiList();
        return SystemConfigurationAccessor.GetEscrowTableFeeDetails(name, TableFeeType.EscrowRefinance, tableFeeListBase);
      }
      if (name.Equals(typeof (TblTitlePurList).Name))
      {
        TableFeeListBase tableFeeListBase = (TableFeeListBase) new TblTitlePurList();
        return SystemConfigurationAccessor.GetTableFeeDetailsForTitle(name, TableFeeType.TitlePurchase, tableFeeListBase);
      }
      if (name.Equals(typeof (TblTitleRefiList).Name))
      {
        TableFeeListBase tableFeeListBase = (TableFeeListBase) new TblTitleRefiList();
        return SystemConfigurationAccessor.GetTableFeeDetailsForTitle(name, TableFeeType.TitleRefinance, tableFeeListBase);
      }
      if (name.Equals(typeof (FeeCityList).Name))
        return SystemConfigurationAccessor.GetCityTaxFee(name);
      if (name.Equals(typeof (FeeStateList).Name))
        return SystemConfigurationAccessor.GetStateTaxFee(name);
      if (name.Equals(typeof (FeeUserList).Name))
        return SystemConfigurationAccessor.GetFeeUserList(name);
      return name.Equals(typeof (PiggybackFields).Name) ? SystemConfigurationAccessor.GetPiggybackFields(name) : SystemConfiguration.GetSystemSettings(name);
    }

    private static BinaryObject GetFeeUserList(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT * FROM {0}", (object) "UserDefinedFee"));
      BinaryObject objBinaryObject = SystemConfigurationAccessor.GetFeeUserListFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (objBinaryObject == null)
      {
        objBinaryObject = SystemConfiguration.GetSystemSettings(name);
        if (objBinaryObject != null && ((FeeListBase) objBinaryObject).Count > 0)
        {
          SystemConfigurationAccessor.InsertFeeUserList(objBinaryObject);
          objBinaryObject = SystemConfigurationAccessor.GetFeeUserList(name);
        }
      }
      return objBinaryObject;
    }

    private static void InsertFeeUserList(BinaryObject objBinaryObject)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      FeeListBase feeListBase = (FeeListBase) objBinaryObject;
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0}", (object) "UserDefinedFee"));
      for (int i = 0; i < feeListBase.Count; ++i)
      {
        FeeListBase.FeeTable tableAt = feeListBase.GetTableAt(i);
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([FeeName], [CalcBasedOn], [Rate], [Additional])", (object) "UserDefinedFee"));
        dbQueryBuilder.AppendLine(string.Format("VALUES({0}, {1}, {2}, {3})", (object) SQL.EncodeString(tableAt.FeeName), (object) SQL.EncodeString(tableAt.CalcBasedOn), (object) SQL.EncodeString(tableAt.Rate), (object) SQL.EncodeString(tableAt.Additional)));
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static BinaryObject GetFeeUserListFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (BinaryObject) null;
      FeeUserList listFromDataTable = new FeeUserList();
      foreach (DataRow row in (InternalDataCollectionBase) source.Rows)
        listFromDataTable.InsertFee(SQL.DecodeString(row["FeeName"]), SQL.DecodeString(row["CalcBasedOn"]), SQL.DecodeString(row["Rate"]), SQL.DecodeString(row["Additional"]));
      return (BinaryObject) (BinaryConvertibleObject) listFromDataTable;
    }

    public static void SaveSystemSettings(string name, BinaryObject data)
    {
      if (name.Equals(typeof (TblEscrowPurList).Name))
      {
        TableFeeListBase tableFeeListBase = data.ToObject<TableFeeListBase>();
        SystemConfigurationAccessor.SaveTableFeeForEscrow(ref tableFeeListBase, TableFeeType.EscrowPurchase);
      }
      else if (name.Equals(typeof (TblEscrowRefiList).Name))
      {
        TableFeeListBase tableFeeListBase = data.ToObject<TableFeeListBase>();
        SystemConfigurationAccessor.SaveTableFeeForEscrow(ref tableFeeListBase, TableFeeType.EscrowRefinance);
      }
      if (name.Equals(typeof (TblTitlePurList).Name))
      {
        TableFeeListBase tableFeeListBase = data.ToObject<TableFeeListBase>();
        SystemConfigurationAccessor.SaveTableFeeForTitle(ref tableFeeListBase, TableFeeType.TitlePurchase);
      }
      else if (name.Equals(typeof (TblTitleRefiList).Name))
      {
        TableFeeListBase tableFeeListBase = data.ToObject<TableFeeListBase>();
        SystemConfigurationAccessor.SaveTableFeeForTitle(ref tableFeeListBase, TableFeeType.TitleRefinance);
      }
      else if (name.Equals(typeof (FeeCityList).Name))
        SystemConfigurationAccessor.SaveCityTaxFee(data.ToObject<FeeCityList>());
      else if (name.Equals(typeof (FeeUserList).Name))
        SystemConfigurationAccessor.InsertFeeUserList(data);
      else if (name.Equals(typeof (FeeStateList).Name))
        SystemConfigurationAccessor.SaveStateTaxFee(data.ToObject<FeeStateList>());
      else if (name.Equals(typeof (PiggybackFields).Name))
        SystemConfigurationAccessor.SavePiggybackFields(data);
      SystemConfiguration.SaveSystemSettings(name, data);
    }

    private static BinaryObject GetEscrowTableFeeDetails(
      string name,
      TableFeeType tableFeeType,
      TableFeeListBase tableFeeListBase)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select *, frl.EscrowFeeID as frlEscrowFeeID, f.EscrowFeeID as fEscrowFeeID");
      dbQueryBuilder.AppendLine("from EscrowFeeRateList frl");
      dbQueryBuilder.AppendLine("    right outer join EscrowFee f on f.EscrowFeeID = frl.EscrowFeeID");
      dbQueryBuilder.AppendLine("    where (f.EscrowType = '" + (object) (int) tableFeeType + "')");
      dbQueryBuilder.AppendLine("    order by f.EscrowFeeID");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
      {
        BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name);
        if (systemSettings == null)
          return (BinaryObject) null;
        SystemConfigurationAccessor.SaveSystemSettings(name, systemSettings);
        if (systemSettings.ToObject<TableFeeListBase>().FeeTables.Count == 0)
          return new BinaryObject((IXmlSerializable) tableFeeListBase);
        dataRowCollection = dbQueryBuilder.Execute();
      }
      Dictionary<string, List<DataRow>> dictionary = new Dictionary<string, List<DataRow>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = SQL.DecodeString(dataRow["fEscrowFeeID"]);
        List<DataRow> dataRowList;
        if (!dictionary.ContainsKey(key))
        {
          dataRowList = new List<DataRow>();
          dictionary.Add(key, dataRowList);
        }
        else
          dataRowList = dictionary[key];
        dataRowList.Add(dataRow);
      }
      foreach (List<DataRow> escrowFeeRateRows in dictionary.Values)
        SystemConfigurationAccessor.GetEscrowFeeRateList(tableFeeListBase, escrowFeeRateRows);
      return new BinaryObject((IXmlSerializable) tableFeeListBase);
    }

    private static void GetEscrowFeeRateList(
      TableFeeListBase tableFeeListBase,
      List<DataRow> escrowFeeRateRows)
    {
      string rateList = string.Empty;
      DataRow escrowFeeRateRow1 = escrowFeeRateRows[0];
      string name = SQL.DecodeString(escrowFeeRateRow1["EscrowTableName"]);
      bool useThis = SQL.DecodeBoolean(escrowFeeRateRow1["IsDefault"]);
      string calcBasedOn = SQL.DecodeString(escrowFeeRateRow1["CalcBasedOn"]);
      string rounding = SQL.DecodeString(escrowFeeRateRow1["Rounding"]);
      string nearest = SQL.DecodeString(escrowFeeRateRow1["Nearest"]);
      string offset = SQL.DecodeString(escrowFeeRateRow1["Offset"]);
      string feeType = SQL.DecodeString(escrowFeeRateRow1["FeeType"]);
      if (!string.IsNullOrEmpty(SQL.DecodeString(escrowFeeRateRow1["frlEscrowFeeID"])))
      {
        rateList = string.Empty;
        string empty = string.Empty;
        foreach (DataRow escrowFeeRateRow2 in escrowFeeRateRows)
        {
          string str = string.Format("{0}:{1}:{2}", (object) SQL.DecodeString(escrowFeeRateRow2["RateUpTo"]), (object) SQL.DecodeString(escrowFeeRateRow2["BaseRate"]), (object) SQL.DecodeString(escrowFeeRateRow2["Factor"]));
          rateList = !(rateList == string.Empty) ? rateList + "|" + str : str;
        }
      }
      tableFeeListBase.InsertFee(name, useThis, calcBasedOn, rounding, nearest, offset, rateList, feeType);
    }

    private static void SaveTableFeeForEscrow(
      ref TableFeeListBase tableFeeListBase,
      TableFeeType tableFeeType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("EscrowFee");
      dbQueryBuilder.Declare("@EscrowFeeID", "int");
      dbQueryBuilder.AppendLine("-- DELETE EscrowRateList rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE R FROM {0} R ", (object) "EscrowFeeRateList"));
      dbQueryBuilder.AppendLine(string.Format("INNER JOIN {0} as E ", (object) "EscrowFee"));
      dbQueryBuilder.AppendLine(string.Format("ON R.EscrowFeeID = E.EscrowFeeID WHERE e.EscrowType = {0}", (object) (int) tableFeeType));
      dbQueryBuilder.AppendLine("-- DELETE EscrowFee rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE EscrowType = {1}", (object) "EscrowFee", (object) (int) tableFeeType));
      foreach (DictionaryEntry feeTable1 in tableFeeListBase.FeeTables)
      {
        TableFeeListBase.FeeTable feeTable2 = (TableFeeListBase.FeeTable) feeTable1.Value;
        dbQueryBuilder.AppendLine("SET @EscrowFeeID = 0");
        DbValueList valueListExcrowFee = SystemConfigurationAccessor.CreateDbValueListExcrowFee(feeTable2, tableFeeType);
        dbQueryBuilder.InsertInto(table, valueListExcrowFee, true, false);
        dbQueryBuilder.SelectIdentity("@EscrowFeeID");
        if (!string.IsNullOrEmpty(feeTable2.RateList))
        {
          string[] strArray1 = feeTable2.RateList.Split('|');
          string empty = string.Empty;
          for (int index1 = 0; index1 < strArray1.Length; ++index1)
          {
            string[] strArray2 = strArray1[index1].Split(':');
            for (int index2 = 0; index2 < strArray2.Length; ++index2)
            {
              if (strArray2[index2] == "")
                strArray2[index2] = "0";
            }
            if (strArray2.Length > 1)
              dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} (EscrowFeeID, RateUpTo, BaseRate, Factor) VALUES (@EscrowFeeID, {1}, {2}, {3})", (object) "EscrowFeeRateList", SQL.EncodeDecimal(strArray2[0]), SQL.EncodeDecimal(strArray2[1]), SQL.EncodeDecimal(strArray2[2])));
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static DbValueList CreateDbValueListExcrowFee(
      TableFeeListBase.FeeTable feeTable,
      TableFeeType tableFeeType)
    {
      return new DbValueList()
      {
        {
          "EscrowType",
          (object) (int) tableFeeType
        },
        {
          "EscrowTableName",
          (object) feeTable.TableName
        },
        {
          "IsDefault",
          (object) (feeTable.UseThis ? 1 : 0)
        },
        {
          "CalcBasedOn",
          (object) feeTable.CalcBasedOn
        },
        {
          "Rounding",
          (object) feeTable.Rounding
        },
        {
          "Nearest",
          SQL.EncodeDecimal(feeTable.Nearest)
        },
        {
          "Offset",
          SQL.EncodeDecimal(feeTable.Offset)
        },
        {
          "FeeType",
          (object) feeTable.FeeType
        }
      };
    }

    private static BinaryObject GetTableFeeDetailsForTitle(
      string name,
      TableFeeType tableFeeType,
      TableFeeListBase tableFeeListBase)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select *, frl.TitleFeeID as frlTitleFeeID, f.TitleFeeID as fTitleFeeID");
      dbQueryBuilder.AppendLine("from TitleFeeRateList frl");
      dbQueryBuilder.AppendLine("    right outer join TitleFee f on f.TitleFeeID = frl.TitleFeeID");
      dbQueryBuilder.AppendLine("    where (f.TitleType = '" + (object) (int) tableFeeType + "')");
      dbQueryBuilder.AppendLine("    order by f.TitleFeeID");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
      {
        BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name);
        if (systemSettings == null)
          return (BinaryObject) null;
        SystemConfigurationAccessor.SaveSystemSettings(name, systemSettings);
        if (systemSettings.ToObject<TableFeeListBase>().FeeTables.Count == 0)
          return new BinaryObject((IXmlSerializable) tableFeeListBase);
        dataRowCollection = dbQueryBuilder.Execute();
      }
      Dictionary<string, List<DataRow>> dictionary = new Dictionary<string, List<DataRow>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = SQL.DecodeString(dataRow["fTitleFeeID"]);
        List<DataRow> dataRowList;
        if (!dictionary.ContainsKey(key))
        {
          dataRowList = new List<DataRow>();
          dictionary.Add(key, dataRowList);
        }
        else
          dataRowList = dictionary[key];
        dataRowList.Add(dataRow);
      }
      foreach (List<DataRow> titleFeeRateRows in dictionary.Values)
        SystemConfigurationAccessor.GetTitleFeeRateList(tableFeeListBase, titleFeeRateRows);
      return new BinaryObject((IXmlSerializable) tableFeeListBase);
    }

    private static void GetTitleFeeRateList(
      TableFeeListBase tableFeeListBase,
      List<DataRow> titleFeeRateRows)
    {
      string rateList = string.Empty;
      DataRow titleFeeRateRow1 = titleFeeRateRows[0];
      string name = SQL.DecodeString(titleFeeRateRow1["TitleTableName"]);
      bool useThis = SQL.DecodeBoolean(titleFeeRateRow1["IsDefault"]);
      string calcBasedOn = SQL.DecodeString(titleFeeRateRow1["CalcBasedOn"]);
      string rounding = SQL.DecodeString(titleFeeRateRow1["Rounding"]);
      string nearest = SQL.DecodeString(titleFeeRateRow1["Nearest"]);
      string offset = SQL.DecodeString(titleFeeRateRow1["Offset"]);
      string feeType = SQL.DecodeString(titleFeeRateRow1["FeeType"]);
      if (!string.IsNullOrEmpty(SQL.DecodeString(titleFeeRateRow1["frlTitleFeeID"])))
      {
        rateList = string.Empty;
        string empty = string.Empty;
        foreach (DataRow titleFeeRateRow2 in titleFeeRateRows)
        {
          string str = string.Format("{0}:{1}:{2}", (object) SQL.DecodeString(titleFeeRateRow2["RateUpTo"]), (object) SQL.DecodeString(titleFeeRateRow2["BaseRate"]), (object) SQL.DecodeString(titleFeeRateRow2["Factor"]));
          rateList = !(rateList == string.Empty) ? rateList + "|" + str : str;
        }
      }
      tableFeeListBase.InsertFee(name, useThis, calcBasedOn, rounding, nearest, offset, rateList, feeType);
    }

    private static void SaveTableFeeForTitle(
      ref TableFeeListBase tableFeeListBase,
      TableFeeType tableFeeType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TitleFee");
      dbQueryBuilder.Declare("@TitleFeeID", "int");
      dbQueryBuilder.AppendLine("-- DELETE TitleRateList rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE R FROM {0} R ", (object) "TitleFeeRateList"));
      dbQueryBuilder.AppendLine(string.Format("INNER JOIN {0} as T ", (object) "TitleFee"));
      dbQueryBuilder.AppendLine(string.Format("ON R.TitleFeeID = T.TitleFeeID WHERE T.TitleType = {0}", (object) (int) tableFeeType));
      dbQueryBuilder.AppendLine("-- DELETE TitleFee rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE TitleType = {1}", (object) "TitleFee", (object) (int) tableFeeType));
      foreach (DictionaryEntry feeTable1 in tableFeeListBase.FeeTables)
      {
        TableFeeListBase.FeeTable feeTable2 = (TableFeeListBase.FeeTable) feeTable1.Value;
        dbQueryBuilder.AppendLine("SET @TitleFeeID = 0");
        DbValueList valueListTitleFee = SystemConfigurationAccessor.CreateDbValueListTitleFee(feeTable2, tableFeeType);
        dbQueryBuilder.InsertInto(table, valueListTitleFee, true, false);
        dbQueryBuilder.SelectIdentity("@TitleFeeID");
        if (!string.IsNullOrEmpty(feeTable2.RateList))
        {
          string[] strArray1 = feeTable2.RateList.Split('|');
          string empty = string.Empty;
          for (int index = 0; index < strArray1.Length; ++index)
          {
            string[] strArray2 = strArray1[index].Split(':');
            if (strArray2.Length > 1)
              dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} (TitleFeeID, RateUpTo, BaseRate, Factor) VALUES (@TitleFeeID, {1}, {2}, {3})", (object) "TitleFeeRateList", SQL.EncodeDecimal(strArray2[0]), SQL.EncodeDecimal(strArray2[1]), SQL.EncodeDecimal(strArray2[2])));
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static DbValueList CreateDbValueListTitleFee(
      TableFeeListBase.FeeTable feeTable,
      TableFeeType tableFeeType)
    {
      return new DbValueList()
      {
        {
          "TitleType",
          (object) (int) tableFeeType
        },
        {
          "TitleTableName",
          (object) feeTable.TableName
        },
        {
          "IsDefault",
          (object) (feeTable.UseThis ? 1 : 0)
        },
        {
          "CalcBasedOn",
          (object) feeTable.CalcBasedOn
        },
        {
          "Rounding",
          (object) feeTable.Rounding
        },
        {
          "Nearest",
          SQL.EncodeDecimal(feeTable.Nearest)
        },
        {
          "Offset",
          SQL.EncodeDecimal(feeTable.Offset)
        },
        {
          "FeeType",
          (object) feeTable.FeeType
        }
      };
    }

    private static BinaryObject GetCityTaxFee(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("CityTaxFee");
      dbQueryBuilder.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
      {
        BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name);
        if (systemSettings == null)
          return (BinaryObject) null;
        if (systemSettings.ToObject<FeeCityList>().Count <= 0)
          return systemSettings;
        SystemConfigurationAccessor.SaveSystemSettings(name, systemSettings);
        return SystemConfigurationAccessor.GetSystemSettings(name);
      }
      FeeCityList serializableObject = new FeeCityList();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        serializableObject.InsertFee(new FeeListBase.FeeTable(SQL.DecodeString(dataRow["FeeName"]), SQL.DecodeString(dataRow["CalcBasedOn"]), SQL.DecodeString(dataRow["FeeRate"]), SQL.DecodeString(dataRow["AdditionalAmount"])));
      return new BinaryObject((IXmlSerializable) serializableObject);
    }

    private static void SaveCityTaxFee(FeeCityList feeCityList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("CityTaxFee");
      dbQueryBuilder.AppendLine("-- DELETE CityTaxFee rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0}", (object) "CityTaxFee"));
      foreach (FeeListBase.FeeTable fee in feeCityList.FeeList)
      {
        DbValueList listForTaxFeeList = SystemConfigurationAccessor.CreateDbValueListForTaxFeeList(fee);
        dbQueryBuilder.InsertInto(table, listForTaxFeeList, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static DbValueList CreateDbValueListForTaxFeeList(FeeListBase.FeeTable feeListBase)
    {
      return new DbValueList()
      {
        {
          "FeeName",
          (object) feeListBase.FeeName
        },
        {
          "CalcBasedOn",
          (object) feeListBase.CalcBasedOn
        },
        {
          "FeeRate",
          (object) feeListBase.Rate
        },
        {
          "AdditionalAmount",
          (object) feeListBase.Additional
        }
      };
    }

    private static BinaryObject GetStateTaxFee(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("StateTaxFee");
      dbQueryBuilder.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
      {
        BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name);
        if (systemSettings == null)
          return (BinaryObject) null;
        if (systemSettings.ToObject<FeeStateList>().Count <= 0)
          return systemSettings;
        SystemConfigurationAccessor.SaveSystemSettings(name, systemSettings);
        return SystemConfigurationAccessor.GetSystemSettings(name);
      }
      FeeStateList serializableObject = new FeeStateList();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        serializableObject.InsertFee(new FeeListBase.FeeTable(SQL.DecodeString(dataRow["FeeName"]), SQL.DecodeString(dataRow["CalcBasedOn"]), SQL.DecodeString(dataRow["FeeRate"]), SQL.DecodeString(dataRow["AdditionalAmount"])));
      return new BinaryObject((IXmlSerializable) serializableObject);
    }

    private static void SaveStateTaxFee(FeeStateList feeStateList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("StateTaxFee");
      dbQueryBuilder.AppendLine("-- DELETE StateTaxFee rows");
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0}", (object) "StateTaxFee"));
      foreach (FeeListBase.FeeTable fee in feeStateList.FeeList)
      {
        DbValueList listForTaxFeeList = SystemConfigurationAccessor.CreateDbValueListForTaxFeeList(fee);
        dbQueryBuilder.InsertInto(table, listForTaxFeeList, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static BinaryObject GetPiggybackFields(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("PiggybackFields");
      dbQueryBuilder.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
      {
        BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name);
        if (systemSettings == null)
          return (BinaryObject) null;
        if (systemSettings.ToObject<PiggybackFields>().Count <= 0)
          return systemSettings;
        SystemConfigurationAccessor.SaveSystemSettings(name, systemSettings);
        return SystemConfigurationAccessor.GetSystemSettings(name);
      }
      PiggybackFields serializableObject = new PiggybackFields();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        serializableObject.AddField(SQL.DecodeString(dataRow["FieldID"]));
      return new BinaryObject((IXmlSerializable) serializableObject);
    }

    private static void SavePiggybackFields(BinaryObject binaryObject)
    {
      PiggybackFields ruleFields = binaryObject.ToObject<PiggybackFields>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("PiggybackFields");
      dbQueryBuilder.AppendLine(string.Format("-- DELETE {0} rows", (object) "PiggybackFields"));
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0}", (object) "PiggybackFields"));
      string[] syncFields = ruleFields.GetSyncFields();
      DbValueList values = new DbValueList();
      foreach (string str in syncFields)
      {
        values.Clear();
        DbValue dbValue = new DbValue("FieldID", (object) str);
        values.Add(dbValue);
        dbQueryBuilder.InsertInto(table, values, true, false);
      }
      FieldSearchRule fieldSearchRule = new FieldSearchRule(ruleFields);
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
    }

    public static FileSystemEntry[] GetHelocTableList(string fullName)
    {
      return SystemConfigurationAccessor.GetHelocTableList(fullName, false);
    }

    public static FileSystemEntry[] GetHelocTableList(
      string fullName,
      bool useNewHELOCHistoricTable)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM HelocTable WHERE [IsNewHELOC] " + (useNewHELOCHistoricTable ? "= '1'" : "<> '1'"));
      FileSystemEntry[] entries = SystemConfigurationAccessor.getHelocTableFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (useNewHELOCHistoricTable || entries != null && entries.Length != 0)
        return entries;
      entries = FileStore.GetDirectoryEntries(fullName, FileSystemEntry.Types.File, (string) null, "\\", false, true, false);
      if (entries != null && entries.Length != 0)
      {
        SystemConfigurationAccessor.populateHelocDatabase(entries);
        entries = SystemConfigurationAccessor.GetHelocTableList(fullName);
      }
      return entries;
    }

    public static BinaryObject GetHelocTableDetails(string name)
    {
      return SystemConfigurationAccessor.GetHelocTableDetails(name, false);
    }

    public static BinaryObject GetHelocTableDetails(string name, bool useNewHELOCHistoricTable)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT ht.*, htd.* FROM {0} ht INNER JOIN {1} htd ON ht.HELOCID = htd.HELOCID WHERE ht.[TableName] = {2} ", (object) "HelocTable", (object) "HelocTableDetails", (object) SQL.EncodeString(name)));
      if (useNewHELOCHistoricTable)
        dbQueryBuilder.Append("and ht.[IsNewHELOC] = '1'");
      else
        dbQueryBuilder.Append("and ht.[IsNewHELOC] <> '1'");
      BinaryObject helocTableDetails = SystemConfigurationAccessor.getHelocTableDetailsFromDataTable(dbQueryBuilder.ExecuteTableQuery(), useNewHELOCHistoricTable);
      if (!useNewHELOCHistoricTable && helocTableDetails == null)
      {
        name = FileSystem.EncodeFilename(name, true);
        helocTableDetails = SystemConfiguration.GetHelocRateTable(name);
      }
      return helocTableDetails;
    }

    public static bool SaveHelocTable(string name, BinaryObject data)
    {
      return SystemConfigurationAccessor.SaveHelocTable(name, data, false);
    }

    public static bool SaveHelocTable(
      string name,
      BinaryObject data,
      bool useNewHELOCHistoricTable)
    {
      bool flag = SystemConfigurationAccessor.insertHelocTable(name, data);
      if (useNewHELOCHistoricTable)
        return flag;
      name = FileSystem.EncodeFilename(name, true);
      return SystemConfiguration.SaveHelocRateTable(name, data);
    }

    public static bool ExistsHelocTable(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE TableName = {1}", (object) "HelocTable", (object) SQL.EncodeString(name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
      {
        name = FileSystem.EncodeFilename(name, true);
        flag = SystemConfiguration.HelocTableObjectExists(name);
      }
      return flag;
    }

    public static bool DeleteHelocTable(string name)
    {
      return SystemConfigurationAccessor.DeleteHelocTable(name, false);
    }

    public static bool DeleteHelocTable(string name, bool useNewHELOCHistoricTable)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("DELETE FROM {0} WHERE [TableName] = {1}", (object) "HelocTable", (object) SQL.EncodeString(name)));
      dbQueryBuilder.Append(" AND [IsNewHELOC] " + (useNewHELOCHistoricTable ? "= '1'" : "<> '1'"));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      if (!useNewHELOCHistoricTable)
      {
        name = FileSystem.EncodeFilename(name, true);
        SystemConfiguration.DeleteHelocRateTable(name);
      }
      return true;
    }

    private static FileSystemEntry[] getHelocTableFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] tableFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
        tableFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["TableName"]), FileSystemEntry.Types.File, (string) null);
      return tableFromDataTable;
    }

    private static BinaryObject getHelocTableDetailsFromDataTable(
      DataTable source,
      bool useNewHELOCHistoricTable)
    {
      if (source == null || source.Rows.Count == 0)
        return (BinaryObject) null;
      HelocRateTable detailsFromDataTable = new HelocRateTable();
      if (useNewHELOCHistoricTable)
      {
        detailsFromDataTable.IsNewHELOC = true;
        IEnumerator enumerator = source.Rows.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            detailsFromDataTable.IndexDay = Utils.ParseInt((object) SQL.DecodeString(current["IndexDay"]), 0);
            detailsFromDataTable.IndexMonth = Utils.ParseInt((object) SQL.DecodeString(current["IndexMonth"]), 0);
            detailsFromDataTable.IndexName = SQL.DecodeString(current["IndexName"]);
            detailsFromDataTable.UseAlternateSchedule = Utils.ParseBoolean(current["UseAlternateSchedule"]);
            detailsFromDataTable.DefaultHistoricMargin = Utils.ParseDecimal((object) SQL.DecodeString(current["DefaultMargin"]), 0M);
            detailsFromDataTable.DecimalsUseForIndex = SQL.DecodeString(current["DecimalsUseForIndex"]);
            if (string.IsNullOrEmpty(detailsFromDataTable.DecimalsUseForIndex))
              detailsFromDataTable.DecimalsUseForIndex = "ThreeDecimals";
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      else
      {
        IEnumerator enumerator = source.Rows.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            detailsFromDataTable.DecimalsUseForIndex = SQL.DecodeString(current["DecimalsUseForIndex"]);
            if (string.IsNullOrEmpty(detailsFromDataTable.DecimalsUseForIndex))
              detailsFromDataTable.DecimalsUseForIndex = "ThreeDecimals";
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      foreach (DataRow row in (InternalDataCollectionBase) source.Rows)
        detailsFromDataTable.InsertYearRecord(SQL.DecodeString(row["Year"]), SQL.DecodeString(row["PeriodType"]), SQL.DecodeString(row["IndexRate"]), SQL.DecodeString(row["MarginRate"]), SQL.DecodeString(row["APR"]), SQL.DecodeString(row["MinimumMonthlyPayment"]));
      return (BinaryObject) (BinaryConvertibleObject) detailsFromDataTable;
    }

    private static bool insertHelocTable(string name, BinaryObject data)
    {
      HelocRateTable helocRateTable = (HelocRateTable) data;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("HelocTable");
      DbValueList keys = new DbValueList();
      keys.Add("TableName", (object) name);
      keys.Add("IsNewHELOC", helocRateTable.IsNewHELOC ? (object) "1" : (object) "0");
      dbQueryBuilder.Declare("@HELOCID", "INT");
      dbQueryBuilder.DeleteFrom(table, keys);
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "TableName",
          (object) name
        },
        {
          "IndexDay",
          (object) helocRateTable.IndexDay
        },
        {
          "IndexMonth",
          (object) helocRateTable.IndexMonth
        },
        {
          "IndexName",
          (object) helocRateTable.IndexName
        },
        {
          "IsNewHELOC",
          helocRateTable.IsNewHELOC ? (object) "1" : (object) "0"
        },
        {
          "UseAlternateSchedule",
          helocRateTable.UseAlternateSchedule ? (object) "1" : (object) "0"
        },
        {
          "DefaultMargin",
          (object) helocRateTable.DefaultHistoricMargin
        },
        {
          "DecimalsUseForIndex",
          string.IsNullOrEmpty(helocRateTable.DecimalsUseForIndex) ? (object) "ThreeDecimals" : (object) helocRateTable.DecimalsUseForIndex
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity("@HELOCID");
      for (int i = 0; i < helocRateTable.Count; ++i)
      {
        HelocRateTable.YearRecord yearRecordAt = helocRateTable.GetYearRecordAt(i);
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([HELOCID], [Year], [PeriodType], [IndexRate], [MarginRate], [APR], [MinimumMonthlyPayment] )", (object) "HelocTableDetails"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@HELOCID, {1}, {2}, {3}, {4}, {5}, {6})", (object) "HelocTableDetails", (object) yearRecordAt.Year, (object) SQL.EncodeString(helocRateTable.IsNewHELOC ? "" : yearRecordAt.PeriodType), SQL.EncodeDecimal(yearRecordAt.IndexRate), SQL.EncodeDecimal(helocRateTable.IsNewHELOC ? "0" : yearRecordAt.MarginRate), SQL.EncodeDecimal(helocRateTable.IsNewHELOC ? "0" : yearRecordAt.APR), SQL.EncodeDecimal(helocRateTable.IsNewHELOC ? "0" : yearRecordAt.MinimumPayment)));
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    private static void populateHelocDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        BinaryObject helocRateTable = SystemConfiguration.GetHelocRateTable(entries[index].Name);
        SystemConfigurationAccessor.insertHelocTable(entries[index].Name, helocRateTable);
      }
    }

    public static FileSystemEntry[] GetLoanDuplicationTemplatesList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return SystemConfigurationAccessor.GetLoanDuplicationTemplatesList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetLoanDuplicationTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM LoanDuplicationTemplates");
      FileSystemEntry[] entries = SystemConfigurationAccessor.getLoanDuplicationTemplatesFromDataTable(dbQueryBuilder.ExecuteTableQuery(), parentFolder, userInfo);
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.LoanDuplicationTemplate, parentFolder, FileSystemEntry.Types.All, false, true);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateLoanDuplicationTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetLoanDuplicationTemplatesList(parentFolder, userInfo);
        }
      }
      return entries;
    }

    public static BinaryObject GetLoanDuplicationTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [name] = {1}", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(entry.Name)));
      DataTable dtLoanDuplicationTemplates = dbQueryBuilder.ExecuteTableQuery();
      DataTable dtLoanDuplicationAdditionalFields = (DataTable) null;
      if (dtLoanDuplicationTemplates != null && dtLoanDuplicationTemplates.Rows.Count > 0)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [LoanDuplicationTemplateID] = {1} ORDER BY [Sequence] ASC", (object) "LoanDuplicationAdditionalFields", dtLoanDuplicationTemplates.Rows[0]["LoanDuplicationTemplateID"]));
        dtLoanDuplicationAdditionalFields = dbQueryBuilder.ExecuteTableQuery();
      }
      return SystemConfigurationAccessor.getLoanDuplicationTemplateSettingsFromDataTable(dtLoanDuplicationTemplates, dtLoanDuplicationAdditionalFields);
    }

    public static bool ExistsLoanDuplicationTemplateSettings(FileSystemEntry entry, bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE name = {1}", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = SystemConfigurationAccessor.existsTemplate(ofAnyType, entry, TemplateSettingsType.LoanDuplicationTemplate);
      return flag;
    }

    public static bool SaveLoanDuplicationTemplateSettings(BinaryObject data)
    {
      SystemConfigurationAccessor.insertLoanDuplicationTemplates(data);
      return true;
    }

    public static void RenameLoanDuplicationTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [name] = {2} WHERE [name] = {1}", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(sourceName), (object) SQL.EncodeString(targetName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicateLoanDuplicationTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@LoanDuplicationTemplateID", "INT");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([name], [desc], [ignorebr], [BorrowerInformation], [BorrowerEmployerInformation], [BorrowerPresentAddress], [Co-BorrowerInformation], [Co-BorrowerEmployerInformation], [Co-BorrowerPresentAddress], [Property], [BorrowerAddressTo], [CoBorrowerAddressTo], [PropertyAddressTo])", (object) "LoanDuplicationTemplates"));
      dbQueryBuilder.AppendLine(string.Format("SELECT {2}, [desc], [ignorebr], [BorrowerInformation], [BorrowerEmployerInformation], [BorrowerPresentAddress], [Co-BorrowerInformation], [Co-BorrowerEmployerInformation], [Co-BorrowerPresentAddress], [Property], [BorrowerAddressTo], [CoBorrowerAddressTo], [PropertyAddressTo] FROM {0} WHERE [name] = {1}", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(sourceName), (object) SQL.EncodeString(targetName)));
      dbQueryBuilder.SelectIdentity("@LoanDuplicationTemplateID");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([LoanDuplicationTemplateID], [FieldID], [Sequence])", (object) "LoanDuplicationAdditionalFields"));
      dbQueryBuilder.AppendLine(string.Format("SELECT @LoanDuplicationTemplateID, [FieldID], [Sequence] FROM {0} WHERE [LoanDuplicationTemplateID] = (SELECT [LoanDuplicationTemplateID] FROM {1} WHERE [name] = {2})", (object) "LoanDuplicationAdditionalFields", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DeleteLoanDuplicationTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [name] = {1}", (object) "LoanDuplicationTemplates", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] getLoanDuplicationTemplatesFromDataTable(
      DataTable source,
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] fsEntries = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        fsEntries[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["name"]), FileSystemEntry.Types.File, (string) null);
        fsEntries[index].Properties.Add((object) "Name", (object) SQL.DecodeString(source.Rows[index]["name"]));
        fsEntries[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["desc"]));
        SystemConfigurationAccessor.createTemplateFile(fsEntries[index], TemplateSettingsType.LoanDuplicationTemplate, new SystemConfigurationAccessor.methodCallback(SystemConfigurationAccessor.GetLoanDuplicationTemplateSettings));
      }
      return AclGroupFileAccessor.ApplyUserAccessRights(userInfo, fsEntries, AclFileType.None);
    }

    private static BinaryObject getLoanDuplicationTemplateSettingsFromDataTable(
      DataTable dtLoanDuplicationTemplates,
      DataTable dtLoanDuplicationAdditionalFields)
    {
      if (dtLoanDuplicationTemplates == null || dtLoanDuplicationTemplates.Rows.Count == 0)
        return (BinaryObject) null;
      LoanDuplicationTemplate settingsFromDataTable = new LoanDuplicationTemplate();
      DataRow row1 = dtLoanDuplicationTemplates.Rows[0];
      settingsFromDataTable.TemplateName = SQL.DecodeString(row1["name"]);
      settingsFromDataTable.Description = SQL.DecodeString(row1["desc"]);
      settingsFromDataTable.IgnoreBusinessRules = SQL.DecodeBoolean(row1["ignorebr"]);
      settingsFromDataTable.SetCurrentField("BorrowerInformation", SQL.DecodeBoolean(row1["BorrowerInformation"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("BorrowerEmployerInformation", SQL.DecodeBoolean(row1["BorrowerEmployerInformation"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("BorrowerPresentAddress", SQL.DecodeBoolean(row1["BorrowerPresentAddress"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("Co-BorrowerInformation", SQL.DecodeBoolean(row1["Co-BorrowerInformation"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("Co-BorrowerEmployerInformation", SQL.DecodeBoolean(row1["Co-BorrowerEmployerInformation"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("Co-BorrowerPresentAddress", SQL.DecodeBoolean(row1["Co-BorrowerPresentAddress"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("Property", SQL.DecodeBoolean(row1["Property"]) ? "1" : "");
      settingsFromDataTable.SetCurrentField("BorrowerAddressTo", SQL.DecodeString(row1["BorrowerAddressTo"]));
      settingsFromDataTable.SetCurrentField("CoBorrowerAddressTo", SQL.DecodeString(row1["CoBorrowerAddressTo"]));
      settingsFromDataTable.SetCurrentField("PropertyAddressTo", SQL.DecodeString(row1["PropertyAddressTo"]));
      if (dtLoanDuplicationAdditionalFields != null && dtLoanDuplicationAdditionalFields.Rows.Count > 0)
      {
        List<string> stringList = new List<string>();
        foreach (DataRow row2 in (InternalDataCollectionBase) dtLoanDuplicationAdditionalFields.Rows)
          stringList.Add(SQL.DecodeString(row2["FieldID"]));
        settingsFromDataTable.SetAdditionalFields(stringList.ToArray());
      }
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataTable;
    }

    private static void populateLoanDuplicationTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.LoanDuplicationTemplate, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertLoanDuplicationTemplates(latestVersion.Data);
        }
      }
    }

    private static bool insertLoanDuplicationTemplates(BinaryObject data)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanDuplicationTemplates");
      DbValueList dbValueList = new DbValueList();
      LoanDuplicationTemplate duplicationTemplate = (LoanDuplicationTemplate) data;
      dbValueList.Add("name", (object) duplicationTemplate.TemplateName);
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("desc", (object) duplicationTemplate.Description);
      dbValueList.Add("ignorebr", (object) duplicationTemplate.IgnoreBusinessRules);
      dbValueList.Add("BorrowerInformation", (object) SQL.EncodeFlag(duplicationTemplate.GetField("BorrowerInformation") == "1"));
      dbValueList.Add("BorrowerEmployerInformation", (object) SQL.EncodeFlag(duplicationTemplate.GetField("BorrowerEmployerInformation") == "1"));
      dbValueList.Add("BorrowerPresentAddress", (object) SQL.EncodeFlag(duplicationTemplate.GetField("BorrowerPresentAddress") == "1"));
      dbValueList.Add("Co-BorrowerInformation", (object) SQL.EncodeFlag(duplicationTemplate.GetField("Co-BorrowerInformation") == "1"));
      dbValueList.Add("Co-BorrowerEmployerInformation", (object) SQL.EncodeFlag(duplicationTemplate.GetField("Co-BorrowerEmployerInformation") == "1"));
      dbValueList.Add("Co-BorrowerPresentAddress", (object) SQL.EncodeFlag(duplicationTemplate.GetField("Co-BorrowerPresentAddress") == "1"));
      dbValueList.Add("Property", (object) SQL.EncodeFlag(duplicationTemplate.GetField("Property") == "1"));
      dbValueList.Add("BorrowerAddressTo", (object) duplicationTemplate.GetField("BorrowerAddressTo"));
      dbValueList.Add("CoBorrowerAddressTo", (object) duplicationTemplate.GetField("CoBorrowerAddressTo"));
      dbValueList.Add("PropertyAddressTo", (object) duplicationTemplate.GetField("PropertyAddressTo"));
      dbQueryBuilder.Declare("@LoanDuplicationTemplateID", "INT");
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@LoanDuplicationTemplateID");
      string[] additionalFields = duplicationTemplate.GetAdditionalFields();
      if (additionalFields != null)
      {
        for (int index = 0; index < additionalFields.Length; ++index)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([LoanDuplicationTemplateID], [FieldID], [Sequence])", (object) "LoanDuplicationAdditionalFields"));
          dbQueryBuilder.AppendLine(string.Format("VALUES(@LoanDuplicationTemplateID, {0}, {1})", (object) SQL.EncodeString(additionalFields[index]), (object) (index + 1)));
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static List<ChangeCircumstanceSettings> GetAllChangeCircumstanceSettings()
    {
      return ClientContext.GetCurrent().Cache.Get<List<ChangeCircumstanceSettings>>("CachedChangeCircumstanceOptions", new Func<List<ChangeCircumstanceSettings>>(SystemConfigurationAccessor.GetAllChangeCircumstanceSettingsFromDB), CacheSetting.Low);
    }

    private static List<ChangeCircumstanceSettings> GetAllChangeCircumstanceSettingsFromDB()
    {
      try
      {
        List<ChangeCircumstanceSettings> circumstanceSettingsFromDb = new List<ChangeCircumstanceSettings>();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select optionID,LineNumber from [ChangeCircumstanceLineItems]");
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
        for (int index = 0; index < dataRowCollection1.Count; ++index)
        {
          DataRow dataRow = dataRowCollection1[index];
          if (dictionary.ContainsKey((int) dataRow["optionId"]))
            dictionary[SQL.DecodeInt(dataRow["optionId"])].Add(SQL.DecodeString(dataRow["LineNumber"]));
          else
            dictionary.Add((int) dataRow["optionId"], new List<string>()
            {
              (string) dataRow["LineNumber"]
            });
        }
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("select optionID,optionCode, optionValue, optionComment,Reason,CocType,optionOrder from [ChangeCircumstanceOptions] Order by optionOrder");
        DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
        for (int index = 0; index < dataRowCollection2.Count; ++index)
        {
          ChangeCircumstanceSettings circumstanceSettings = new ChangeCircumstanceSettings();
          DataRow dataRow = dataRowCollection2[index];
          circumstanceSettings.optionId = SQL.DecodeInt(dataRow["optionId"]);
          circumstanceSettings.Code = SQL.DecodeString(dataRow["optionCode"]);
          circumstanceSettings.Comment = SQL.DecodeString(dataRow["optionComment"]);
          circumstanceSettings.Description = SQL.DecodeString(dataRow["optionValue"]);
          circumstanceSettings.Reason = dataRow["Reason"] is DBNull || dataRow["Reason"] == null ? -1 : SQL.DecodeInt(dataRow["Reason"]);
          circumstanceSettings.CocType = SQL.DecodeString(dataRow["CocType"]);
          circumstanceSettings.optionOrder = SQL.DecodeInt(dataRow["optionOrder"]);
          circumstanceSettingsFromDb.Add(circumstanceSettings);
        }
        TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Retrieved Change Circumstance Settings");
        return circumstanceSettingsFromDb;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        return (List<ChangeCircumstanceSettings>) null;
      }
    }

    public static bool UpdateChangeCircumstance(List<ChangeCircumstanceSettings> changeCoCSetting)
    {
      ClientContext.GetCurrent().Cache.Put<List<ChangeCircumstanceSettings>>("CachedChangeCircumstanceOptions", (Action) (() =>
      {
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          int num = 1;
          foreach (ChangeCircumstanceSettings circumstanceSettings in changeCoCSetting)
          {
            if (circumstanceSettings.optionId == -1)
            {
              dbQueryBuilder.Reset();
              DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ChangeCircumstanceOptions");
              dbQueryBuilder.InsertInto(table, new DbValueList()
              {
                {
                  "optionCode",
                  (object) circumstanceSettings.Code
                },
                {
                  "optionvalue",
                  (object) circumstanceSettings.Description
                },
                {
                  "optionComment",
                  (object) circumstanceSettings.Comment
                },
                {
                  "optionOrder",
                  (object) num
                },
                {
                  "reason",
                  (object) circumstanceSettings.Reason
                },
                {
                  "CocType",
                  (object) circumstanceSettings.CocType
                }
              }, true, false);
              dbQueryBuilder.AppendLine("SELECT SCOPE_IDENTITY()");
              object obj = dbQueryBuilder.ExecuteScalar();
              circumstanceSettings.optionId = Convert.ToInt32(obj);
            }
            else
            {
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append("Update ChangeCircumstanceOptions set optionComment=" + SQL.EncodeString(circumstanceSettings.Comment) + ", optionvalue = " + SQL.EncodeString(circumstanceSettings.Description) + ", reason = " + SQL.Encode((object) circumstanceSettings.Reason) + ", CocType =" + SQL.EncodeString(circumstanceSettings.CocType) + ", optionOrder =" + SQL.Encode((object) num) + " where optionId =" + SQL.Encode((object) circumstanceSettings.optionId));
              dbQueryBuilder.ExecuteNonQuery();
            }
            ++num;
          }
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Change in Circumstance Option cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        }
      }), new Func<List<ChangeCircumstanceSettings>>(SystemConfigurationAccessor.GetAllChangeCircumstanceSettingsFromDB), CacheSetting.Low);
      return true;
    }

    public static void AddChangeCircumstanceLines(ChangeCircumstanceSettings coc)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete from ChangeCircumstanceLineItems where optionId =" + (object) coc.optionId);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ChangeCircumstanceLineItems");
      DbValueList dbValueList = new DbValueList();
      foreach (string lineNumber in coc.LineNumbers)
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "OptionId",
            (object) coc.optionId
          },
          {
            "LineNumber",
            (object) lineNumber
          }
        }, true, false);
      if (coc.LineNumbers.Count <= 0)
        return;
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<string[]> GetChangeCircumstanceOptions()
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        List<string[]> circumstanceOptions = (List<string[]>) current.Cache.Get("CachedChangeCircumstanceOptions");
        if (circumstanceOptions != null)
        {
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Retrieved Change Circumstance Options from Cache successfully.");
          return circumstanceOptions;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Cannot load Change Circumstance Options from Cache. Error: " + ex.Message);
      }
      try
      {
        using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
        {
          List<string[]> o = new List<string[]>();
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("select optionCode, optionValue, optionComment, Reason from [ChangeCircumstanceOptions] Order by optionOrder");
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection.Count == 0)
          {
            TraceLog.WriteWarning(nameof (SystemConfigurationAccessor), "No Change Circumstance Options found in database.");
            o.Add(new string[4]
            {
              "ChangeLoanAmt",
              "Change in loan amount",
              "Change in loan amount",
              "-1"
            });
            o.Add(new string[4]
            {
              "LoanTypeProgram",
              "Loan type or loan program has changed",
              "Loan type or loan program has changed",
              "-1"
            });
            o.Add(new string[4]
            {
              "IncomeNotVeri",
              "Borrower income could not verified or was verified at different amount",
              "Borrower income could not verified or was verified at different amount",
              "-1"
            });
            o.Add(new string[4]
            {
              "ApprasValDiff",
              "Appraised value is different than estimated value",
              "Appraised value is different than estimated value",
              "-1"
            });
            o.Add(new string[4]
            {
              "AddiService",
              "Additional service (such as survey) is necessary based on title report",
              "Additional service (such as survey) is necessary based on title report",
              "-1"
            });
            o.Add(new string[4]
            {
              "RecordingFee",
              "Recording fees are increased based on need to record additional unanticipated documents such as release of prior lien",
              "Recording fees are increased based on need to record additional unanticipated documents such as release of prior lien",
              "-1"
            });
            o.Add(new string[4]
            {
              "PropertyTitle",
              "Borrower taking title to the property has changed",
              "Borrower taking title to the property has changed",
              "-1"
            });
            o.Add(new string[4]
            {
              "AddiBor",
              "Additional borrower has been added to the loan or borrower has been dropped from the loan",
              "Additional borrower has been added to the loan or borrower has been dropped from the loan",
              "-1"
            });
            o.Add(new string[4]
            {
              "Other",
              "Other",
              "Other",
              "-1"
            });
            return o;
          }
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          string empty3 = string.Empty;
          string empty4 = string.Empty;
          for (int index = 0; index < dataRowCollection.Count; ++index)
          {
            DataRow dataRow = dataRowCollection[index];
            string str1 = (string) dataRow["optionCode"];
            string str2 = (string) dataRow["optionValue"];
            string str3 = (string) dataRow["optionComment"];
            string str4 = ((int) dataRow["Reason"]).ToString();
            o.Add(new string[4]{ str1, str2, str3, str4 });
          }
          current.Cache.Put("CachedChangeCircumstanceOptions", (object) o, CacheSetting.Low);
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Retrieved Change Circumstance Options");
          return o;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        return (List<string[]>) null;
      }
    }

    public static void SetChangeCircumstanceOptions(List<string[]> options)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
      {
        current.Cache.Remove("CachedChangeCircumstanceOptions");
        TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Deleting Change in Circumstance Option Setting from SQL...");
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.Append("Delete from [ChangeCircumstanceOptions]");
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Change in Circumstance Option Setting records cannot be deleted. Error: " + ex.Message);
        }
        if (options != null)
        {
          if (options.Count != 0)
          {
            try
            {
              TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Saving Change in Circumstance Option Setting records...");
              EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
              DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ChangeCircumstanceOptions");
              for (int index = 0; index < options.Count; ++index)
                dbQueryBuilder.InsertInto(table, new DbValueList()
                {
                  {
                    "optionCode",
                    (object) options[index][0]
                  },
                  {
                    "optionValue",
                    (object) options[index][1]
                  },
                  {
                    "optionComment",
                    (object) options[index][2]
                  },
                  {
                    "optionOrder",
                    (object) (index + 1)
                  }
                }, true, false);
              dbQueryBuilder.ExecuteNonQuery();
              current.Cache.Put("CachedChangeCircumstanceOptions", (object) options, CacheSetting.Low);
              return;
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Change in Circumstance Option cannot be updated. Error: " + ex.Message);
              Err.Reraise(nameof (SystemConfigurationAccessor), ex);
              return;
            }
          }
        }
        current.Cache.Remove("CachedChangeCircumstanceOptions");
      }
    }

    public static void DeleteChangeCircumstanceSetting(List<ChangeCircumstanceSettings> cocSettings)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
      {
        try
        {
          string str = string.Join<int>(",", (IEnumerable<int>) cocSettings.Select<ChangeCircumstanceSettings, int>((System.Func<ChangeCircumstanceSettings, int>) (a => a.optionId)).ToArray<int>());
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("Delete from ChangeCircumstanceLineItems where optionId in (" + str + ")");
          dbQueryBuilder.AppendLine("Delete from ChangeCircumstanceOptions where optionId in (" + str + ")");
          dbQueryBuilder.ExecuteNonQuery();
          current.Cache.Remove("CachedChangeCircumstanceOptions");
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Change circumstance cannot be deleted. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        }
      }
    }

    public static ChangeCircumstanceSettings GetChangedCircumstanceOption(
      int changeCircumstanceOptionId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ChangeCircumstanceSettings circumstanceOption = new ChangeCircumstanceSettings();
      dbQueryBuilder.AppendLine("SELECT optionID, optionCode, optionValue, optionComment, Reason, CocType, optionOrder FROM [ChangeCircumstanceOptions] WHERE optionID = " + (object) changeCircumstanceOptionId + " ORDER BY optionOrder");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return (ChangeCircumstanceSettings) null;
        circumstanceOption.optionId = SQL.DecodeInt(dataRowCollection[0]["optionId"]);
        circumstanceOption.Code = SQL.DecodeString(dataRowCollection[0]["optionCode"]);
        circumstanceOption.Comment = SQL.DecodeString(dataRowCollection[0]["optionComment"]);
        circumstanceOption.Description = SQL.DecodeString(dataRowCollection[0]["optionValue"]);
        circumstanceOption.Reason = dataRowCollection[0]["Reason"] is DBNull || dataRowCollection[0]["Reason"] == null ? -1 : SQL.DecodeInt(dataRowCollection[0]["Reason"]);
        circumstanceOption.CocType = SQL.DecodeString(dataRowCollection[0]["CocType"]);
        circumstanceOption.optionOrder = SQL.DecodeInt(dataRowCollection[0]["optionOrder"]);
        return circumstanceOption;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        return (ChangeCircumstanceSettings) null;
      }
    }

    public static int AddChangedCircumstanceOption(
      ChangeCircumstanceSettings changeCircumstanceOption)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        int num1 = 0;
        try
        {
          int num2 = 0;
          dbQueryBuilder.AppendLine("SELECT MAX(optionOrder) AS optionOrder FROM ChangeCircumstanceOptions");
          object obj = dbQueryBuilder.ExecuteScalar();
          if (obj != null)
            num2 = Convert.ToInt32(obj);
          dbQueryBuilder.Reset();
          DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ChangeCircumstanceOptions");
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "optionCode",
              (object) changeCircumstanceOption.Code
            },
            {
              "optionValue",
              (object) changeCircumstanceOption.Description
            },
            {
              "optionComment",
              (object) changeCircumstanceOption.Comment
            },
            {
              "optionOrder",
              (object) (num2 + 1)
            },
            {
              "Reason",
              (object) changeCircumstanceOption.Reason
            },
            {
              "CocType",
              (object) changeCircumstanceOption.CocType
            }
          }, true, false);
          dbQueryBuilder.SelectIdentity();
          num1 = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
          current.Cache.Remove("CachedChangeCircumstanceOptions");
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Changed circumstance option cannot be added. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        }
        return num1;
      }
    }

    public static bool UpdateChangedCircumstanceOption(
      int changeCircumstanceOptionId,
      ChangeCircumstanceSettings changeCircumstanceOption)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        try
        {
          dbQueryBuilder.Append("UPDATE ChangeCircumstanceOptions SET optionComment=" + SQL.EncodeString(changeCircumstanceOption.Comment) + ", optionValue = " + SQL.EncodeString(changeCircumstanceOption.Description) + ", Reason = " + SQL.Encode((object) changeCircumstanceOption.Reason) + ", CocType =" + SQL.EncodeString(changeCircumstanceOption.CocType) + " WHERE optionID =" + SQL.Encode((object) changeCircumstanceOptionId));
          dbQueryBuilder.ExecuteNonQuery();
          current.Cache.Remove("CachedChangeCircumstanceOptions");
          return true;
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Changed circumstance option cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
          return false;
        }
      }
    }

    public static bool DeleteChangedCircumstanceOption(int changeCircumstanceOptionId)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("CachedChangeCircumstanceOptions"))
      {
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("DELETE FROM ChangeCircumstanceOptions WHERE optionID = " + (object) changeCircumstanceOptionId);
          dbQueryBuilder.ExecuteNonQuery();
          current.Cache.Remove("CachedChangeCircumstanceOptions");
          return true;
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Changed circumstance option cannot be deleted. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
          return false;
        }
      }
    }

    public static void DeleteZipcodeUserDefineds(ZipcodeInfoUserDefined[] zipcodeInfoUserDefineds)
    {
      ClientContext.GetCurrent().Cache.Put<ZipcodeUserDefinedList>("CachedZipcodeUserDefined", (Action) (() =>
      {
        TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Deleting Zipcode Database Setting from SQL...");
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          for (int index = 0; index < zipcodeInfoUserDefineds.Length; ++index)
            dbQueryBuilder.AppendLine("Delete from [ZipcodeUserDefinedList] WHERE [Zipcode] = '" + zipcodeInfoUserDefineds[index].Zipcode + "' AND [ZipcodeExt] = '" + zipcodeInfoUserDefineds[index].ZipcodeExtension + "' AND [StateName] = '" + zipcodeInfoUserDefineds[index].ZipInfo.State + "' AND [CityName] = " + SQL.Encode((object) zipcodeInfoUserDefineds[index].ZipInfo.City));
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Zipcode Database Setting records cannot be deleted. Error: " + ex.Message);
        }
      }), new Func<ZipcodeUserDefinedList>(SystemConfiguration.GetZipcodeUserDefinedListFromDB), CacheSetting.Low);
    }

    public static void UpdateZipcodeUserDefined(
      ZipcodeInfoUserDefined newZipcodeInfoUserDefined,
      ZipcodeInfoUserDefined oldZipcodeInfoUserDefined)
    {
      ClientContext.GetCurrent().Cache.Put<ZipcodeUserDefinedList>("CachedZipcodeUserDefined", (Action) (() =>
      {
        if (oldZipcodeInfoUserDefined != null)
          SystemConfigurationAccessor.DeleteZipcodeUserDefineds(new ZipcodeInfoUserDefined[1]
          {
            oldZipcodeInfoUserDefined
          });
        try
        {
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Zipcode Database Setting records...");
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ZipcodeUserDefinedList");
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "Zipcode",
              (object) newZipcodeInfoUserDefined.Zipcode
            },
            {
              "ZipcodeExt",
              (object) newZipcodeInfoUserDefined.ZipcodeExtension
            },
            {
              "CityName",
              (object) newZipcodeInfoUserDefined.ZipInfo.City
            },
            {
              "StateName",
              (object) newZipcodeInfoUserDefined.ZipInfo.State.ToUpper()
            },
            {
              "CountyName",
              (object) newZipcodeInfoUserDefined.ZipInfo.County
            }
          }, true, false);
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Zipcode Database Setting cannot be updated. Error: " + ex.Message);
          Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        }
      }), new Func<ZipcodeUserDefinedList>(SystemConfiguration.GetZipcodeUserDefinedListFromDB), CacheSetting.Low);
    }

    public static FileSystemEntry[] GetDocumentStackingTemplatesList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return SystemConfigurationAccessor.GetDocumentStackingTemplatesList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetDocumentStackingTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      return SystemConfigurationAccessor.GetDocumentStackingTemplatesList(parentFolder, userInfo, out int _);
    }

    public static FileSystemEntry[] GetDocumentStackingTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo userInfo,
      out int totalCount,
      int start = 0,
      int limit = 0)
    {
      totalCount = 0;
      ClientContext current = ClientContext.GetCurrent();
      string str = "-1";
      try
      {
        str = (string) current.Cache.Get("CachedFixDocumentStackinsgTemplatesFlag");
        if (str != null)
          TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Retrieved Fix Document Stacking Templates flag from Cache.");
      }
      catch (Exception ex)
      {
        TraceLog.WriteInfo(nameof (SystemConfigurationAccessor), "Cannot load Fix Document Stacking Templates flag from Cache. Error: " + ex.Message);
      }
      if (str != "1")
        str = Company.GetCompanySetting("MIGRATION", "DocumentStackingTemplatesFixed") ?? "";
      if (str != "1")
      {
        TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Fixing Document Stacking Templates...");
        SystemConfigurationAccessor.fixDocumentStackingTemplates(parentFolder, userInfo);
        using (current.Cache.Lock("CachedFixDocumentStackinsgTemplatesFlag"))
        {
          Company.SetCompanySetting("MIGRATION", "DocumentStackingTemplatesFixed", "1");
          current.Cache.Put("CachedFixDocumentStackinsgTemplatesFlag", (object) "1", CacheSetting.Low);
        }
      }
      else
        TraceLog.WriteVerbose(nameof (SystemConfigurationAccessor), "Document Stacking Templates already fixed.");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DataTable source = (DataTable) null;
      dbQueryBuilder.AppendLine("SELECT * FROM DocumentStackingTemplate");
      if (start >= 0 && limit > 0)
      {
        DataTable paginatedRecords = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), start + 1, start + limit, (List<SortColumn>) null);
        if (paginatedRecords != null && paginatedRecords.Rows != null && paginatedRecords.Rows.Count > 0)
        {
          source = paginatedRecords;
          totalCount = SQL.DecodeInt(paginatedRecords.Rows[0]["TotalRowCount"]);
        }
      }
      else
        source = dbQueryBuilder.ExecuteTableQuery();
      FileSystemEntry[] entries = SystemConfigurationAccessor.getDocumentStackingTemplatesFromDataTable(source, parentFolder, userInfo);
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.StackingOrder, parentFolder, FileSystemEntry.Types.All, false, true);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateDocumentStackingTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetDocumentStackingTemplatesList(parentFolder, userInfo);
        }
      }
      return entries;
    }

    public static BinaryObject GetDocumentStackingTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DTNAME] = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(entry.Name)));
      DataTable dtDocumentStackingTemplates = dbQueryBuilder.ExecuteTableQuery();
      DataTable dtDocumentStackingTemplateElements = (DataTable) null;
      if (dtDocumentStackingTemplates != null && dtDocumentStackingTemplates.Rows.Count > 0)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DocumentStackingTemplateID] = {1} ORDER BY [ElementType] ASC, [Sequence] ASC", (object) "DocumentStackingTemplateElements", dtDocumentStackingTemplates.Rows[0]["DocumentStackingTemplateID"]));
        dtDocumentStackingTemplateElements = dbQueryBuilder.ExecuteTableQuery();
      }
      return SystemConfigurationAccessor.getDocumentStackingTemplateSettingsFromDataTable(dtDocumentStackingTemplates, dtDocumentStackingTemplateElements);
    }

    public static bool ExistsDocumentStackingTemplateSettings(FileSystemEntry entry, bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE DTNAME = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = SystemConfigurationAccessor.existsTemplate(ofAnyType, entry, TemplateSettingsType.StackingOrder);
      return flag;
    }

    public static bool SaveDocumentStackingTemplateSettings(
      string templatePath,
      BinaryObject data,
      bool isUpdate)
    {
      SystemConfigurationAccessor.insertDocumentStackingTemplates(templatePath, data, isUpdate);
      return true;
    }

    public static void RenameDocumentStackingTemplate(
      string sourceName,
      string targetName,
      string targetPath)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [DTNAME] = {2}, [TemplatePath] = {3} WHERE [DTNAME] = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(sourceName), (object) SQL.EncodeString(targetName), (object) SQL.EncodeString(targetPath)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicateDocumentStackingTemplate(
      string sourceName,
      string targetName,
      string targetPath)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@DocumentStackingTemplateID", "INT");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DTNAME], [DTDESC], [DEFAULT], [AUTOSELECTDOCUMENTS], [FILTERDOCUMENTS], [TemplatePath])", (object) "DocumentStackingTemplate"));
      dbQueryBuilder.AppendLine(string.Format("SELECT {2}, [DTDESC], [DEFAULT], [AUTOSELECTDOCUMENTS], [FILTERDOCUMENTS], {3} FROM {0} WHERE [DTNAME] = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(sourceName), (object) SQL.EncodeString(targetName), (object) SQL.EncodeString(targetPath)));
      dbQueryBuilder.SelectIdentity("@DocumentStackingTemplateID");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DocumentStackingTemplateID], [DocumentName], [ElementType], [Sequence])", (object) "DocumentStackingTemplateElements"));
      dbQueryBuilder.AppendLine(string.Format("SELECT @DocumentStackingTemplateID, [DocumentName], [ElementType], [Sequence] FROM {0} WHERE [DocumentStackingTemplateID] = (SELECT [DocumentStackingTemplateID] FROM {1} WHERE [DTNAME] = {2})", (object) "DocumentStackingTemplateElements", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DeleteDocumentStackingTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [DTNAME] = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static void fixDocumentStackingTemplates(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.StackingOrder, parentFolder, FileSystemEntry.Types.All, false, true);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.StackingOrder, directoryEntries[index]))
        {
          if (latestVersion.Exists)
          {
            foreach (XElement element in XDocument.Load(latestVersion.Identity.PhysicalPath).Root.Elements().Elements<XElement>())
            {
              if (element.FirstAttribute.Value == "1")
              {
                SystemConfigurationAccessor.insertDocumentStackingTemplates(directoryEntries[index].ToString(), latestVersion.Data, false);
                break;
              }
            }
          }
        }
      }
    }

    private static FileSystemEntry[] getDocumentStackingTemplatesFromDataTable(
      DataTable source,
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] fsEntries = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        fsEntries[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["DTNAME"]), FileSystemEntry.Types.File, (string) null);
        fsEntries[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["DTDESC"]));
        SystemConfigurationAccessor.createTemplateFile(fsEntries[index], TemplateSettingsType.StackingOrder, new SystemConfigurationAccessor.methodCallback(SystemConfigurationAccessor.GetDocumentStackingTemplateSettings));
      }
      return AclGroupFileAccessor.ApplyUserAccessRights(userInfo, fsEntries, AclFileType.None);
    }

    private static BinaryObject getDocumentStackingTemplateSettingsFromDataTable(
      DataTable dtDocumentStackingTemplates,
      DataTable dtDocumentStackingTemplateElements)
    {
      if (dtDocumentStackingTemplates == null || dtDocumentStackingTemplates.Rows.Count == 0)
        return (BinaryObject) null;
      StackingOrderSetTemplate ds = new StackingOrderSetTemplate();
      DataRow row1 = dtDocumentStackingTemplates.Rows[0];
      ds.DocumentStackingTemplateID = SQL.DecodeInt(row1["DocumentStackingTemplateID"]);
      ds.TemplateName = SQL.DecodeString(row1["DTNAME"]);
      ds.Description = SQL.DecodeString(row1["DTDESC"]);
      ds.IsDefault = SQL.DecodeBoolean(row1["DEFAULT"]);
      ds.AutoSelectDocuments = SQL.DecodeBoolean(row1["AUTOSELECTDOCUMENTS"]);
      ds.FilterDocuments = SQL.DecodeBoolean(row1["FILTERDOCUMENTS"]);
      if (dtDocumentStackingTemplateElements != null && dtDocumentStackingTemplateElements.Rows.Count > 0)
      {
        bool flag = false;
        foreach (DataRow row2 in (InternalDataCollectionBase) dtDocumentStackingTemplateElements.Rows)
        {
          if (SQL.DecodeInt(row2["ElementType"]) == 1)
          {
            ds.DocNames.Add((object) SQL.DecodeString(row2["DocumentName"]));
            flag = true;
          }
          else if (SQL.DecodeInt(row2["ElementType"]) == 2)
            ds.RequiredDocs.Add((object) SQL.DecodeString(row2["DocumentName"]));
          else if (SQL.DecodeInt(row2["ElementType"]) == 3)
            ds.DocNames.Add((object) SQL.DecodeString(row2["DocumentName"]));
          else if (SQL.DecodeInt(row2["ElementType"]) == 4)
            ds.NDEDocGroups.Add((object) SQL.DecodeString(row2["DocumentName"]));
        }
        if (flag)
          SystemConfigurationAccessor.resolveStackingOrderSetXRefs(ds, XRefKeyType.CustomMilestoneGuid);
      }
      return (BinaryObject) (BinaryConvertibleObject) ds;
    }

    private static void populateDocumentStackingTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.StackingOrder, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertDocumentStackingTemplates(entries[index].ToString(), latestVersion.Data, false);
        }
      }
    }

    private static bool insertDocumentStackingTemplates(
      string templatePath,
      BinaryObject data,
      bool isUpdate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DocumentStackingTemplate");
      DbValueList values = new DbValueList();
      StackingOrderSetTemplate orderSetTemplate = (StackingOrderSetTemplate) data;
      DbValue dbValue = new DbValue("TemplatePath", (object) templatePath);
      values.Add(dbValue);
      values.Add("DTNAME", (object) orderSetTemplate.TemplateName);
      values.Add("DTDESC", (object) orderSetTemplate.Description);
      values.Add("DEFAULT", (object) SQL.EncodeFlag(orderSetTemplate.IsDefault));
      values.Add("AUTOSELECTDOCUMENTS", (object) SQL.EncodeFlag(orderSetTemplate.AutoSelectDocuments));
      values.Add("FILTERDOCUMENTS", (object) SQL.EncodeFlag(orderSetTemplate.FilterDocuments));
      dbQueryBuilder.Declare("@DocumentStackingTemplateID", "INT");
      dbQueryBuilder.IfExists(table, dbValue);
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine(string.Format("SELECT @DocumentStackingTemplateID = DocumentStackingTemplateID FROM [{0}] WHERE TemplatePath = {1}", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(templatePath)));
      dbQueryBuilder.Update(table, values, dbValue);
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.SelectIdentity("@DocumentStackingTemplateID");
      dbQueryBuilder.End();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM [{0}] WHERE DocumentStackingTemplateID = @DocumentStackingTemplateID", (object) "DocumentStackingTemplateElements"));
      for (int index = 0; index < orderSetTemplate.RequiredDocs.Count; ++index)
      {
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DocumentStackingTemplateID], [DocumentName], [ElementType], [Sequence])", (object) "DocumentStackingTemplateElements"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@DocumentStackingTemplateID, {0}, 2, {1})", (object) SQL.Encode(orderSetTemplate.RequiredDocs[index]), (object) (index + 1)));
      }
      for (int index = 0; index < orderSetTemplate.DocNames.Count; ++index)
      {
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DocumentStackingTemplateID], [DocumentName], [ElementType], [Sequence])", (object) "DocumentStackingTemplateElements"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@DocumentStackingTemplateID, {0}, 3, {1})", (object) SQL.Encode(orderSetTemplate.DocNames[index]), (object) (index + 1)));
      }
      for (int index = 0; index < orderSetTemplate.NDEDocGroups.Count; ++index)
      {
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DocumentStackingTemplateID], [DocumentName], [ElementType], [Sequence])", (object) "DocumentStackingTemplateElements"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@DocumentStackingTemplateID, {0}, 4, {1})", (object) SQL.Encode(orderSetTemplate.NDEDocGroups[index]), (object) (index + 1)));
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    private static void resolveStackingOrderSetXRefs(
      StackingOrderSetTemplate ds,
      XRefKeyType keyTpye)
    {
      if (ds.DocNames.Count == 0)
        return;
      Hashtable documentXrefMap = DocumentTrackingConfiguration.GetDocumentXRefMap(keyTpye);
      SystemConfigurationAccessor.translateStackingOrderSetXRefs(ds, documentXrefMap);
    }

    private static void translateStackingOrderSetXRefs(
      StackingOrderSetTemplate ds,
      Hashtable docXRefs)
    {
      if (docXRefs.Contains((object) "VOD"))
        docXRefs.Remove((object) "VOD");
      if (docXRefs.Contains((object) "VOE"))
        docXRefs.Remove((object) "VOE");
      if (docXRefs.Contains((object) "VOL"))
        docXRefs.Remove((object) "VOL");
      if (docXRefs.Contains((object) "VOM"))
        docXRefs.Remove((object) "VOM");
      if (docXRefs.Contains((object) "VOR"))
        docXRefs.Remove((object) "VOR");
      docXRefs.Add((object) "VOD", (object) "VOD");
      docXRefs.Add((object) "VOE", (object) "VOE");
      docXRefs.Add((object) "VOL", (object) "VOL");
      docXRefs.Add((object) "VOM", (object) "VOM");
      docXRefs.Add((object) "VOR", (object) "VOR");
      for (int index = ds.DocNames.Count - 1; index >= 0; --index)
      {
        string docXref = (string) docXRefs[ds.DocNames[index]];
        if (docXref == null)
          ds.DocNames.RemoveAt(index);
        else
          ds.DocNames[index] = (object) docXref;
      }
    }

    public static FileSystemEntry[] GetDocumentExportTemplatesList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return SystemConfigurationAccessor.GetDocumentExportTemplatesList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetDocumentExportTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo user)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM DocumentExportTemplates");
      FileSystemEntry[] entries = SystemConfigurationAccessor.populateDocumentExportTemplatesFileSystemEntry(dbQueryBuilder.ExecuteTableQuery().Rows, parentFolder, user);
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetDirectoryEntries(user, TemplateSettingsType.DocumentExportTemplate, parentFolder, FileSystemEntry.Types.All, false, true);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateDocumentExportTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetDocumentExportTemplatesList(parentFolder, user);
        }
      }
      return entries;
    }

    public static BinaryObject GetDocumentExportTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT EX.*, ST.DTName as StackingOrderName  FROM [{0}] EX LEFT OUTER JOIN [{1}] ST ON EX.DocumentStackingTemplateID = ST.DocumentStackingTemplateID WHERE EX.[TemplateName] = {2}", (object) "DocumentExportTemplates", (object) "DocumentStackingTemplate", (object) SQL.EncodeString(entry.Name)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count > 0 ? SystemConfigurationAccessor.getDocumentExportTemplateSettingsFromDataTable(dataRowCollection[0]) : (BinaryObject) null;
    }

    public static BinaryObject GetDocumentStackingTemplateSettingsByID(int templateID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DocumentStackingTemplateID] = {1}", (object) "DocumentStackingTemplate", (object) templateID));
      DataTable dtDocumentStackingTemplates = dbQueryBuilder.ExecuteTableQuery();
      DataTable dtDocumentStackingTemplateElements = (DataTable) null;
      if (dtDocumentStackingTemplates != null && dtDocumentStackingTemplates.Rows.Count > 0)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DocumentStackingTemplateID] = {1} ORDER BY [ElementType] ASC, [Sequence] ASC", (object) "DocumentStackingTemplateElements", (object) templateID));
        dtDocumentStackingTemplateElements = dbQueryBuilder.ExecuteTableQuery();
      }
      return SystemConfigurationAccessor.getDocumentStackingTemplateSettingsFromDataTable(dtDocumentStackingTemplates, dtDocumentStackingTemplateElements);
    }

    public static bool ExistsDocumentExportTemplateSettings(FileSystemEntry entry, bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(1) FROM {0} WHERE [TemplateName] = {1}", (object) "DocumentExportTemplates", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = SystemConfigurationAccessor.existsTemplate(ofAnyType, entry, TemplateSettingsType.DocumentExportTemplate);
      return flag;
    }

    public static bool SaveDocumentExportTemplateSettings(BinaryObject data, bool isUpdate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(SystemConfigurationAccessor.saveDocumentExportTemplatesIntoDatabase(data, isUpdate));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static void RenameDocumentExportTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [TemplateName] = {1} WHERE [TemplateName] = {2}", (object) "DocumentExportTemplates", (object) SQL.EncodeString(targetName), (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicateDocumentExportTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "[IsDefault],[IsExportAsZip],[AnnotationExportType],[IsPasswordProtected],[IsEncrypted],[IsExportLocationSet],[ExportLocation],[Password],[FileNameField1],[FileNameText1],[FileNameField2],[FileNameText2],[FileNameField3],[FileNameText3],[DocumentStackingTemplateID]";
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([TemplateName], {1})", (object) "DocumentExportTemplates", (object) str));
      dbQueryBuilder.AppendLine(string.Format("SELECT {0}, {1} FROM {2} WHERE [TemplateName] = {3}", (object) SQL.EncodeString(targetName), (object) str, (object) "DocumentExportTemplates", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DeleteDocumentExportTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [TemplateName] = {1}", (object) "DocumentExportTemplates", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static string[] GetStackingOrderTemplateNamesByExportTemplates(
      string[] documentExportTemplateNames)
    {
      if (documentExportTemplateNames == null)
        return (string[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT DISTINCT s.DTName FROM [{0}] s", (object) "DocumentStackingTemplate"));
      dbQueryBuilder.AppendLine(string.Format(" INNER JOIN [{0}] e ", (object) "DocumentExportTemplates"));
      dbQueryBuilder.AppendLine(" On s.DocumentStackingTemplateID = e.DocumentStackingTemplateID ");
      dbQueryBuilder.AppendLine(string.Format(" WHERE e.TemplateName in ({0}) ", (object) string.Join(",", Array.ConvertAll<string, string>(documentExportTemplateNames, (Converter<string, string>) (x => SQL.EncodeString(x.ToString()))))));
      DataRowCollection source = dbQueryBuilder.Execute();
      return source.Count > 0 ? source.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => row["DTName"].ToString())).ToArray<string>() : (string[]) null;
    }

    private static FileSystemEntry[] populateDocumentExportTemplatesFileSystemEntry(
      DataRowCollection dataRows,
      FileSystemEntry parentFolder,
      UserInfo user)
    {
      if (dataRows == null || dataRows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] fsEntries = new FileSystemEntry[dataRows.Count];
      for (int index = 0; index < dataRows.Count; ++index)
      {
        fsEntries[index] = new FileSystemEntry("\\", SQL.DecodeString(dataRows[index]["TemplateName"]), FileSystemEntry.Types.File, (string) null);
        fsEntries[index].Properties.Add((object) "Description", (object) string.Empty);
        SystemConfigurationAccessor.createTemplateFile(fsEntries[index], TemplateSettingsType.DocumentExportTemplate, new SystemConfigurationAccessor.methodCallback(SystemConfigurationAccessor.GetDocumentExportTemplateSettings));
      }
      return AclGroupFileAccessor.ApplyUserAccessRights(user, fsEntries, AclFileType.None);
    }

    private static BinaryObject getDocumentExportTemplateSettingsFromDataTable(
      DataRow documentStackingTemplatesRow)
    {
      if (documentStackingTemplatesRow == null)
        return (BinaryObject) null;
      return (BinaryObject) (BinaryConvertibleObject) new DocumentExportTemplate()
      {
        TemplateName = SQL.DecodeString(documentStackingTemplatesRow["TemplateName"]),
        IsDefault = SQL.DecodeBoolean(documentStackingTemplatesRow["IsDefault"]),
        ExportAsZip = SQL.DecodeBoolean(documentStackingTemplatesRow["IsExportAsZip"]),
        AnnotationExportType = SQL.DecodeEnum<AnnotationExportType>(documentStackingTemplatesRow["AnnotationExportType"], AnnotationExportType.All),
        PasswordProtect = SQL.DecodeBoolean(documentStackingTemplatesRow["IsPasswordProtected"]),
        IsEncrypted = SQL.DecodeBoolean(documentStackingTemplatesRow["IsEncrypted"]),
        ExportLocationSet = SQL.DecodeBoolean(documentStackingTemplatesRow["IsExportLocationSet"]),
        ExportLocation = SQL.DecodeString(documentStackingTemplatesRow["ExportLocation"]),
        ShouldEncryptPassword = false,
        Password = SQL.DecodeString(documentStackingTemplatesRow["Password"]),
        FileNameField1 = SQL.DecodeEnum<ExportFileNameFieldType>(documentStackingTemplatesRow["FileNameField1"], ExportFileNameFieldType.None),
        FileNameText1 = SQL.DecodeString(documentStackingTemplatesRow["FileNameText1"]),
        FileNameField2 = SQL.DecodeEnum<ExportFileNameFieldType>(documentStackingTemplatesRow["FileNameField2"], ExportFileNameFieldType.None),
        FileNameText2 = SQL.DecodeString(documentStackingTemplatesRow["FileNameText2"]),
        FileNameField3 = SQL.DecodeEnum<ExportFileNameFieldType>(documentStackingTemplatesRow["FileNameField3"], ExportFileNameFieldType.None),
        FileNameText3 = SQL.DecodeString(documentStackingTemplatesRow["FileNameText3"]),
        DocumentStackingTemplateID = SQL.DecodeInt(documentStackingTemplatesRow["DocumentStackingTemplateID"]),
        StackingOrderName = SQL.DecodeString(documentStackingTemplatesRow["StackingOrderName"])
      };
    }

    private static void populateDocumentExportTemplatesDatabase(FileSystemEntry[] entries)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.DocumentExportTemplate, entries[index]))
        {
          if (latestVersion.Exists)
            dbQueryBuilder.AppendLine(SystemConfigurationAccessor.saveDocumentExportTemplatesIntoDatabase(latestVersion.Data, false));
        }
      }
      if (string.IsNullOrEmpty(dbQueryBuilder.ToString()))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static string saveDocumentExportTemplatesIntoDatabase(BinaryObject data, bool isUpdate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DocumentExportTemplates");
      DbValueList values = new DbValueList();
      DocumentExportTemplate documentExportTemplate = (DocumentExportTemplate) data;
      DbValue dbValue = new DbValue("TemplateName", (object) documentExportTemplate.TemplateName);
      values.Add(dbValue);
      values.Add("IsDefault", (object) SQL.EncodeFlag(documentExportTemplate.IsDefault));
      values.Add("IsExportAsZip", (object) SQL.EncodeFlag(documentExportTemplate.ExportAsZip));
      values.Add("AnnotationExportType", (object) (int) documentExportTemplate.AnnotationExportType);
      values.Add("IsPasswordProtected", (object) SQL.EncodeFlag(documentExportTemplate.PasswordProtect));
      values.Add("IsEncrypted", (object) SQL.EncodeFlag(documentExportTemplate.IsEncrypted));
      values.Add("IsExportLocationSet", (object) SQL.EncodeFlag(documentExportTemplate.ExportLocationSet));
      values.Add("ExportLocation", (object) documentExportTemplate.ExportLocation);
      values.Add("Password", documentExportTemplate.IsEncrypted ? (object) documentExportTemplate.EntrytedPassword : (object) documentExportTemplate.Password);
      values.Add("FileNameField1", (object) (int) documentExportTemplate.FileNameField1);
      values.Add("FileNameText1", (object) documentExportTemplate.FileNameText1);
      values.Add("FileNameField2", (object) (int) documentExportTemplate.FileNameField2);
      values.Add("FileNameText2", (object) documentExportTemplate.FileNameText2);
      values.Add("FileNameField3", (object) (int) documentExportTemplate.FileNameField3);
      values.Add("FileNameText3", (object) documentExportTemplate.FileNameText3);
      if (documentExportTemplate.DocumentStackingTemplateID > 0)
        values.Add("DocumentStackingTemplateID", (object) documentExportTemplate.DocumentStackingTemplateID);
      else if (!string.IsNullOrEmpty(documentExportTemplate.StackingOrderName))
      {
        StackingOrderSetTemplate templateSettings = (StackingOrderSetTemplate) SystemConfigurationAccessor.GetDocumentStackingTemplateSettings(new FileSystemEntry("\\", documentExportTemplate.StackingOrderName, FileSystemEntry.Types.File, (string) null));
        if (templateSettings != null)
        {
          if (templateSettings.DocumentStackingTemplateID <= 0)
            values.Add("DocumentStackingTemplateID", (object) null);
          else
            values.Add("DocumentStackingTemplateID", (object) templateSettings.DocumentStackingTemplateID);
        }
      }
      dbQueryBuilder.IfExists(table, dbValue);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table, values, dbValue);
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.End();
      return dbQueryBuilder.ToString();
    }

    public static HtmlEmailTemplate[] GetHtmlEmailTemplates(
      string ownerID,
      HtmlEmailTemplateType type)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM HtmlEmailTemplates");
      if (string.IsNullOrEmpty(ownerID))
        dbQueryBuilder.AppendLine(" WHERE OwnerID IS NULL ");
      else
        dbQueryBuilder.AppendLine(" WHERE OwnerID = " + SQL.EncodeString(ownerID));
      HtmlEmailTemplate[] templates = SystemConfigurationAccessor.getHtmlEmailTemplatesCollectionFromDataTable(dbQueryBuilder.ExecuteTableQuery()).GetByType(type);
      if (templates == null || templates.Length == 0)
      {
        templates = HtmlEmailTemplateStore.GetTemplates(ownerID, type);
        if (templates != null && templates.Length != 0)
        {
          SystemConfigurationAccessor.populateHtmlEmailTemplatesToDatabase(templates);
          templates = SystemConfigurationAccessor.GetHtmlEmailTemplates(ownerID, type);
        }
      }
      return templates;
    }

    public static HtmlEmailTemplate GetHtmlEmailTemplate(string ownerID, string guid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [guid] = {1}", (object) "HtmlEmailTemplates", (object) SQL.EncodeString(guid)));
      HtmlEmailTemplateCollection collectionFromDataTable = SystemConfigurationAccessor.getHtmlEmailTemplatesCollectionFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      return collectionFromDataTable == null || collectionFromDataTable.Count == 0 ? HtmlEmailTemplateStore.GetTemplate(ownerID, guid) : collectionFromDataTable.GetByID(guid);
    }

    private static HtmlEmailTemplateCollection getHtmlEmailTemplatesCollectionFromDataTable(
      DataTable source)
    {
      HtmlEmailTemplateCollection collectionFromDataTable = new HtmlEmailTemplateCollection();
      if (source == null || source.Rows.Count == 0)
        return collectionFromDataTable;
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        string ownerID = SQL.DecodeString(source.Rows[index]["OwnerId"]) == string.Empty ? (string) null : SQL.DecodeString(source.Rows[index]["OwnerId"]);
        collectionFromDataTable.Add(new HtmlEmailTemplate(SQL.DecodeString(source.Rows[index]["Guid"]) == string.Empty ? (string) null : SQL.DecodeString(source.Rows[index]["Guid"]), ownerID)
        {
          Type = (HtmlEmailTemplateType) SQL.DecodeInt(source.Rows[index]["TypeId"]),
          Subject = SQL.DecodeString(source.Rows[index]["Subject"]),
          Html = SQL.DecodeString(source.Rows[index]["Html"]),
          Migrated = SQL.DecodeBoolean(source.Rows[index]["Migrated"])
        });
      }
      return collectionFromDataTable;
    }

    private static void populateHtmlEmailTemplatesToDatabase(HtmlEmailTemplate[] templates)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string empty = string.Empty;
      for (int index = 0; index < templates.Length; ++index)
        empty += SystemConfigurationAccessor.generateHtmlEmailTemplateDatabaseScript(templates[index]);
      dbQueryBuilder.Append(empty);
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static string generateHtmlEmailTemplateDatabaseScript(HtmlEmailTemplate template)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("HtmlEmailTemplates");
      DbValueList values = new DbValueList();
      DbValue dbValue = new DbValue("guid", (object) template.Guid);
      values.Add("guid", (object) template.Guid);
      values.Add("OwnerId", (object) template.OwnerID);
      values.Add("TypeId", (object) (uint) template.Type);
      values.Add("Subject", (object) template.Subject);
      values.Add("Html", (object) template.Html);
      values.Add("Migrated", template.Migrated ? (object) "1" : (object) "0");
      dbQueryBuilder.IfExists(table, dbValue);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table, values, dbValue);
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.End();
      return dbQueryBuilder.ToString();
    }

    public static void SaveTemplate(HtmlEmailTemplate template)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string empty = string.Empty;
      string templateDatabaseScript = SystemConfigurationAccessor.generateHtmlEmailTemplateDatabaseScript(template);
      dbQueryBuilder.Append(templateDatabaseScript);
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      HtmlEmailTemplateStore.SaveTemplate(template);
    }

    public static void DeleteTemplate(HtmlEmailTemplate template)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue dbValue = new DbValue("guid", (object) template.Guid);
      dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("HtmlEmailTemplates"), new DbValue("guid", (object) template.Guid));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      HtmlEmailTemplateStore.DeleteTemplate(template);
    }

    public static DocumentGroupSetup GetDocumentGroupSetup()
    {
      return ClientContext.GetCurrent().Cache.Get<DocumentGroupSetup>("DocumentGroupConfiguration", new Func<DocumentGroupSetup>(SystemConfigurationAccessor.GetDocumentGroupSetupFromDB), CacheSetting.Low);
    }

    private static DocumentGroupSetup GetDocumentGroupSetupFromDB()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM DocumentGroup");
      dbQueryBuilder.AppendLine("SELECT * FROM DocumentGroupList");
      DocumentGroupSetup documentGroups = SystemConfigurationAccessor.getDocumentGroupsFromDataTable(dbQueryBuilder.ExecuteSetQuery());
      if (documentGroups == null || documentGroups.Count == 0)
      {
        documentGroups = DocumentGroupConfiguration.GetDocumentGroupSetupFromXml();
        if (documentGroups != null && documentGroups.Count > 0)
        {
          SystemConfigurationAccessor.insertDocumentGroups(documentGroups);
          documentGroups = SystemConfigurationAccessor.GetDocumentGroupSetup();
        }
      }
      return documentGroups;
    }

    public static void SaveDocumentGroupSetup(DocumentGroupSetup documentGroups)
    {
      ClientContext.GetCurrent().Cache.Put<DocumentGroupSetup>("DocumentGroupConfiguration", (Action) (() =>
      {
        SystemConfigurationAccessor.insertDocumentGroups(documentGroups);
        DocumentGroupConfiguration.SaveDocumentGroupSetupToXml(documentGroups);
      }), new Func<DocumentGroupSetup>(SystemConfigurationAccessor.GetDocumentGroupSetupFromDB), CacheSetting.Low);
    }

    private static DocumentGroupSetup getDocumentGroupsFromDataTable(DataSet source)
    {
      if (source == null || source.Tables.Count == 0 || source.Tables[0].Rows.Count == 0)
        return (DocumentGroupSetup) null;
      DocumentGroupSetup groupsFromDataTable = new DocumentGroupSetup();
      DataRelation relation = source.Relations.Add("GroupDocuments", source.Tables[0].Columns["DocumentGroupID"], source.Tables[1].Columns["DocumentGroupID"]);
      foreach (DataRow row in (InternalDataCollectionBase) source.Tables[0].Rows)
      {
        List<string> stringList = new List<string>();
        foreach (DataRow childRow in row.GetChildRows(relation))
          stringList.Add(SQL.DecodeString(childRow["Document"]));
        groupsFromDataTable.Add(new DocumentGroup(SQL.DecodeString(row["Name"]), SQL.DecodeString(row["Guid"]), stringList.Count > 0 ? stringList.ToArray() : (string[]) null));
      }
      return groupsFromDataTable;
    }

    private static bool insertDocumentGroups(DocumentGroupSetup documentGroups)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0}", (object) "DocumentGroup"));
      dbQueryBuilder.Declare("@DocumentGroupID", "INT");
      for (int index = 0; index < documentGroups.Count; ++index)
      {
        DocumentGroup documentGroup = documentGroups[index];
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([Guid], [Name])", (object) "DocumentGroup"));
        dbQueryBuilder.AppendLine(string.Format("VALUES({0}, {1})", (object) SQL.EncodeString(documentGroup.Guid), (object) SQL.EncodeString(documentGroup.Name)));
        dbQueryBuilder.SelectIdentity("@DocumentGroupID");
        if (documentGroup.DocList != null && documentGroup.DocList.Length != 0)
        {
          foreach (string doc in documentGroup.DocList)
          {
            dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([DocumentGroupID], [Document])", (object) "DocumentGroupList"));
            dbQueryBuilder.AppendLine(string.Format("VALUES(@DocumentGroupID, {0})", (object) SQL.EncodeString(doc)));
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static FileSystemEntry[] GetAdjustmentTemplateList(bool includeProperties)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM AdjustmentTemplate ORDER BY [NAME] ASC");
      FileSystemEntry[] entries = SystemConfigurationAccessor.getAdjustmentTemplatesFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.TradePriceAdjustment, false);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateAdjustmentTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetAdjustmentTemplateList(includeProperties);
        }
      }
      return entries;
    }

    public static BinaryObject GetAdjustmentTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [Name] = {1}", (object) "AdjustmentTemplate", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT atpa.* FROM {0} AS atpa INNER JOIN {1} AS at ON at.[AdjustmentTemplateID] = atpa.[AdjustmentTemplateID] AND at.[Name] = {2}", (object) "AdjustmentTemplatePriceAdjustment", (object) "AdjustmentTemplate", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT atc.* FROM {0} AS atc INNER JOIN {1} AS atpa ON atc.[AdjustmentTemplatePriceAdjustmentID] = atpa.[AdjustmentTemplatePriceAdjustmentID] INNER JOIN {2} AS at ON at.[AdjustmentTemplateID] = atpa.[AdjustmentTemplateID] AND at.[Name] = {3}", (object) "AdjustmentTemplateCriterion", (object) "AdjustmentTemplatePriceAdjustment", (object) "AdjustmentTemplate", (object) SQL.EncodeString(entry.Name)));
      return SystemConfigurationAccessor.getAdjustmentTemplateSettingsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
    }

    public static bool ExistsAdjustmentTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE [Name] = {1}", (object) "AdjustmentTemplate", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = TemplateSettingsStore.Exists(TemplateSettingsType.TradePriceAdjustment, entry);
      return flag;
    }

    public static bool SaveAdjustmentTemplateSettings(FileSystemEntry entry, BinaryObject data)
    {
      SystemConfigurationAccessor.insertAdjustmentTemplates(entry, data, true);
      return true;
    }

    public static void DeleteAdjustmentTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [Name] = {1}", (object) "AdjustmentTemplate", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] getAdjustmentTemplatesFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] templatesFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        templatesFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["Name"]), FileSystemEntry.Types.File, (string) null);
        templatesFromDataTable[index].Properties.Add((object) "Guid", (object) SQL.DecodeString(source.Rows[index]["Guid"]));
        templatesFromDataTable[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["Description"]));
      }
      return templatesFromDataTable;
    }

    private static BinaryObject getAdjustmentTemplateSettingsFromDataSet(
      DataSet dsAdjustmentTemplate)
    {
      if (dsAdjustmentTemplate.Tables.Count == 0 || dsAdjustmentTemplate.Tables[0].Rows == null)
        return (BinaryObject) null;
      PriceAdjustmentTemplate settingsFromDataSet = (PriceAdjustmentTemplate) null;
      DataRelation relation1 = dsAdjustmentTemplate.Relations.Add("AdjustmentTemplatePriceAdjustment", dsAdjustmentTemplate.Tables[0].Columns["AdjustmentTemplateID"], dsAdjustmentTemplate.Tables[1].Columns["AdjustmentTemplateID"]);
      DataRelation relation2 = dsAdjustmentTemplate.Relations.Add("AdjustmentTemplateCriterion", dsAdjustmentTemplate.Tables[1].Columns["AdjustmentTemplatePriceAdjustmentID"], dsAdjustmentTemplate.Tables[2].Columns["AdjustmentTemplatePriceAdjustmentID"]);
      foreach (DataRow row in (InternalDataCollectionBase) dsAdjustmentTemplate.Tables[0].Rows)
      {
        settingsFromDataSet = new PriceAdjustmentTemplate(SQL.DecodeString(row["Guid"]), SQL.DecodeString(row["Name"]), SQL.DecodeString(row["Description"]));
        foreach (DataRow childRow1 in row.GetChildRows(relation1))
        {
          TradePriceAdjustment tradePriceAdjustment = new TradePriceAdjustment();
          tradePriceAdjustment.PriceAdjustment = SQL.DecodeDecimal(childRow1["PriceAdjustment"]);
          foreach (DataRow childRow2 in childRow1.GetChildRows(relation2))
            tradePriceAdjustment.CriterionList.Add(new FieldFilter()
            {
              FieldType = SQL.DecodeEnum<EllieMae.EMLite.ClientServer.Reporting.FieldTypes>(childRow2["FieldType"]),
              FieldID = SQL.DecodeString(childRow2["FieldID"]),
              CriterionName = SQL.DecodeString(childRow2["CriterionName"]),
              FieldDescription = SQL.DecodeString(childRow2["FieldDescription"]),
              OperatorType = SQL.DecodeEnum<OperatorTypes>(childRow2["OpType"]),
              ValueFrom = SQL.DecodeString(childRow2["ValueFrom"]),
              ValueTo = SQL.DecodeString(childRow2["ValueTo"]),
              JointToken = SQL.DecodeEnum<JointTokens>(childRow2["JointToken"]),
              LeftParentheses = SQL.DecodeInt(childRow2["LeftParentheses"]),
              RightParentheses = SQL.DecodeInt(childRow2["RightParentheses"]),
              ValueDescription = SQL.DecodeString(childRow2["ValueDescription"]),
              IsVolatile = SQL.DecodeBoolean(childRow2["IsVolatile"]),
              ForceDataConversion = SQL.DecodeBoolean(childRow2["ForceDataConversion"]),
              DataSource = SQL.DecodeEnum<FilterDataSource>(childRow2["DataSource"])
            });
          settingsFromDataSet.PriceAdjustments.Add(tradePriceAdjustment);
        }
      }
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataSet;
    }

    private static void populateAdjustmentTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.TradePriceAdjustment, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertAdjustmentTemplates(entries[index], latestVersion.Data, false);
        }
      }
    }

    private static bool insertAdjustmentTemplates(
      FileSystemEntry entry,
      BinaryObject data,
      bool generateGUID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("AdjustmentTemplate");
      DbValueList dbValueList = new DbValueList();
      PriceAdjustmentTemplate adjustmentTemplate = (PriceAdjustmentTemplate) data;
      dbValueList.Add("Name", (object) adjustmentTemplate.TemplateName);
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("Guid", generateGUID ? (object) SystemConfigurationAccessor.regenerateGUID(entry, TemplateSettingsType.TradePriceAdjustment) : (object) adjustmentTemplate.Guid);
      dbValueList.Add("Description", (object) adjustmentTemplate.Description);
      dbQueryBuilder.Declare("@AdjustmentTemplateID", "INT");
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@AdjustmentTemplateID");
      dbQueryBuilder.Declare("@AdjustmentTemplatePriceAdjustmentID", "INT");
      foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) adjustmentTemplate.PriceAdjustments)
      {
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([AdjustmentTemplateID],\t[PriceAdjustment])", (object) "AdjustmentTemplatePriceAdjustment"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@AdjustmentTemplateID, {0})", (object) priceAdjustment.PriceAdjustment));
        dbQueryBuilder.SelectIdentity("@AdjustmentTemplatePriceAdjustmentID");
        foreach (FieldFilter criterion in (List<FieldFilter>) priceAdjustment.CriterionList)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([AdjustmentTemplatePriceAdjustmentID],\t[FieldType],\t[FieldID],\t[CriterionName],\t[FieldDescription],\t[OpType],\t[ValueFrom],\t[ValueTo],\t[JointToken],\t[LeftParentheses],\t[RightParentheses],\t[ValueDescription],\t[IsVolatile],\t[ForceDataConversion],\t[DataSource])", (object) "AdjustmentTemplateCriterion"));
          dbQueryBuilder.AppendLine(string.Format("VALUES(@AdjustmentTemplatePriceAdjustmentID, {0},\t{1},\t{2},\t{3},\t{4},\t{5},\t{6},\t{7},\t{8},\t{9},\t{10},\t{11},\t{12},\t{13})", (object) SQL.EncodeString(criterion.FieldType.ToString()), (object) SQL.EncodeString(criterion.FieldID), (object) SQL.EncodeString(criterion.CriterionName), (object) SQL.EncodeString(criterion.FieldDescription), (object) SQL.EncodeString(criterion.OperatorType.ToString()), (object) SQL.EncodeString(criterion.ValueFrom), (object) SQL.EncodeString(criterion.ValueTo), (object) SQL.EncodeString(criterion.JointToken.ToString()), (object) criterion.LeftParentheses, (object) criterion.RightParentheses, (object) SQL.EncodeString(criterion.ValueDescription), (object) SQL.EncodeFlag(criterion.IsVolatile), (object) SQL.EncodeFlag(criterion.ForceDataConversion), (object) SQL.EncodeString(criterion.DataSource.ToString())));
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static FileSystemEntry[] GetSRPTemplateList(bool includeProperties)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM SRPTemplates ORDER BY [NAME] ASC");
      FileSystemEntry[] entries = SystemConfigurationAccessor.getSRPTemplatesFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.SRPTable, false);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateSRPTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetSRPTemplateList(includeProperties);
        }
      }
      return entries;
    }

    public static BinaryObject GetSRPTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [Name] = {1}", (object) "SRPTemplates", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT stpi.* FROM {0} AS stpi INNER JOIN {1} AS st ON st.[SRPTemplateId] = stpi.[SRPTemplateId] AND st.[Name] = {2}", (object) "SRPTemplatePricingItems", (object) "SRPTemplates", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT stsa.* FROM {0} AS stsa INNER JOIN {1} AS stpi ON stsa.[PricingItemId] = stpi.[PricingItemId] INNER JOIN {2} AS st ON st.[SRPTemplateId] = stpi.[SRPTemplateId] AND st.[Name] = {3}", (object) "SRPTemplateStateAdjustments", (object) "SRPTemplatePricingItems", (object) "SRPTemplates", (object) SQL.EncodeString(entry.Name)));
      return SystemConfigurationAccessor.getSRPTemplateSettingsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
    }

    public static bool ExistsSRPTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE [Name] = {1}", (object) "SRPTemplates", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = TemplateSettingsStore.Exists(TemplateSettingsType.SRPTable, entry);
      return flag;
    }

    public static bool SaveSRPTemplateSettings(FileSystemEntry entry, BinaryObject data)
    {
      SystemConfigurationAccessor.insertSRPTemplates(entry, data, true);
      return true;
    }

    public static void DeleteSRPTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [Name] = {1}", (object) "SRPTemplates", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] getSRPTemplatesFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] templatesFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        templatesFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["Name"]), FileSystemEntry.Types.File, (string) null);
        templatesFromDataTable[index].Properties.Add((object) "Guid", (object) SQL.DecodeString(source.Rows[index]["Guid"]));
        templatesFromDataTable[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["Description"]));
      }
      return templatesFromDataTable;
    }

    private static BinaryObject getSRPTemplateSettingsFromDataSet(DataSet dsSRPTemplate)
    {
      if (dsSRPTemplate.Tables.Count == 0 || dsSRPTemplate.Tables[0].Rows == null)
        return (BinaryObject) null;
      SRPTableTemplate settingsFromDataSet = (SRPTableTemplate) null;
      DataRelation relation1 = dsSRPTemplate.Relations.Add("SRPTemplatePricingItem", dsSRPTemplate.Tables[0].Columns["SRPTemplateId"], dsSRPTemplate.Tables[1].Columns["SRPTemplateId"]);
      DataRelation relation2 = dsSRPTemplate.Relations.Add("SRPTemplateStateAdjustment", dsSRPTemplate.Tables[1].Columns["PricingItemId"], dsSRPTemplate.Tables[2].Columns["PricingItemId"]);
      foreach (DataRow row in (InternalDataCollectionBase) dsSRPTemplate.Tables[0].Rows)
      {
        settingsFromDataSet = new SRPTableTemplate(SQL.DecodeString(row["Guid"]), SQL.DecodeString(row["Name"]), SQL.DecodeString(row["Description"]));
        foreach (DataRow childRow1 in row.GetChildRows(relation1))
        {
          Range<Decimal> range = new Range<Decimal>();
          Decimal maxValue = childRow1["MaximumRange"] == DBNull.Value ? range.ValueTypeMaximum : SQL.DecodeDecimal(childRow1["MaximumRange"]);
          SRPTable.PricingItem pricingItem = new SRPTable.PricingItem(new Range<Decimal>(SQL.DecodeDecimal(childRow1["MinimumRange"]), maxValue), SQL.DecodeDecimal(childRow1["BaseAdjustment"]), SQL.DecodeDecimal(childRow1["ImpoundsAdjustment"]));
          foreach (DataRow childRow2 in childRow1.GetChildRows(relation2))
          {
            SRPTable.StateAdjustment adj = new SRPTable.StateAdjustment(SQL.DecodeString(childRow2["State"]), SQL.DecodeDecimal(childRow2["Adjustment"]), SQL.DecodeDecimal(childRow2["ImpoundAdjustment"]));
            pricingItem.StateAdjustments.Add(adj);
          }
          settingsFromDataSet.SRPTable.PricingItems.Add(pricingItem);
        }
      }
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataSet;
    }

    private static void populateSRPTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.SRPTable, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertSRPTemplates(entries[index], latestVersion.Data, false);
        }
      }
    }

    private static bool insertSRPTemplates(
      FileSystemEntry entry,
      BinaryObject data,
      bool genrateGUID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SRPTemplates");
      EllieMae.EMLite.Server.DbAccessManager.GetTable("SRPTemplatePricingItems");
      DbValueList dbValueList = new DbValueList();
      SRPTableTemplate srpTableTemplate = (SRPTableTemplate) data;
      dbValueList.Add("Name", (object) srpTableTemplate.TemplateName);
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("Guid", genrateGUID ? (object) SystemConfigurationAccessor.regenerateGUID(entry, TemplateSettingsType.SRPTable) : (object) srpTableTemplate.Guid);
      dbValueList.Add("Description", (object) srpTableTemplate.Description);
      dbQueryBuilder.Declare("@SRPTemplateId", "INT");
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@SRPTemplateId");
      dbQueryBuilder.Declare("@PricingItemId", "INT");
      foreach (SRPTable.PricingItem pricingItem in srpTableTemplate.SRPTable.PricingItems)
      {
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([SRPTemplateId],\t[MinimumRange], [MaximumRange], [BaseAdjustment], [ImpoundsAdjustment])", (object) "SRPTemplatePricingItems"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@SRPTemplateId, {0}, {1}, {2}, {3})", (object) pricingItem.LoanAmount.Minimum, pricingItem.LoanAmount.Maximum == pricingItem.LoanAmount.ValueTypeMaximum ? (object) "NULL" : (object) pricingItem.LoanAmount.Maximum.ToString(), (object) pricingItem.BaseAdjustment, (object) pricingItem.ImpoundsAdjustment));
        dbQueryBuilder.SelectIdentity("@PricingItemId");
        foreach (SRPTable.StateAdjustment stateAdjustment in pricingItem.StateAdjustments)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([PricingItemId], [State], [Adjustment], [ImpoundAdjustment])", (object) "SRPTemplateStateAdjustments"));
          dbQueryBuilder.AppendLine(string.Format("VALUES(@PricingItemId, {0},\t{1},\t{2})", (object) SQL.EncodeString(stateAdjustment.State), (object) stateAdjustment.Adjustment, (object) stateAdjustment.ImpoundAdjustment));
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static FileSystemEntry[] GetFundingTemplateList(bool includeProperties)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM FundingTemplates ORDER BY [NAME] ASC");
      FileSystemEntry[] entries = SystemConfigurationAccessor.getFundingTemplatesFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.FundingTemplate, includeProperties);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateFundingTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetFundingTemplateList(includeProperties);
        }
      }
      return entries;
    }

    public static BinaryObject GetFundingTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [Name] = {1}", (object) "FundingTemplates", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT ftf.* FROM {0} AS ftf INNER JOIN {1} AS ft ON ft.[FundingTemplateId] = ftf.[FundingTemplateId] AND ft.[Name] = {2}", (object) "FundingTemplateFields", (object) "FundingTemplates", (object) SQL.EncodeString(entry.Name)));
      return SystemConfigurationAccessor.getFundingTemplateSettingsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
    }

    public static bool ExistsFundingTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE [Name] = {1}", (object) "FundingTemplates", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = TemplateSettingsStore.Exists(TemplateSettingsType.FundingTemplate, entry);
      return flag;
    }

    public static bool SaveFundingTemplateSettings(BinaryObject data)
    {
      SystemConfigurationAccessor.insertFundingTemplates(data);
      return true;
    }

    public static void DeleteFundingTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [Name] = {1}", (object) "FundingTemplates", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicateFundingTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "[Description], [IgnoreBusinessRules], [Lock], [For2010GFE], [RESPAVersion]";
      dbQueryBuilder.Declare("@FundingTemplateId", "INT");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([Name], {1})", (object) "FundingTemplates", (object) str));
      dbQueryBuilder.AppendLine(string.Format("SELECT {0}, {1} FROM {2} WHERE [Name] = {3}", (object) SQL.EncodeString(targetName), (object) str, (object) "FundingTemplates", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.SelectIdentity("@FundingTemplateId");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([FundingTemplateId], [FieldKey])", (object) "FundingTemplateFields"));
      dbQueryBuilder.AppendLine(string.Format("SELECT @FundingTemplateId, [FieldKey] FROM {0} WHERE [FundingTemplateId] = (SELECT [FundingTemplateId] FROM {1} WHERE [Name] = {2})", (object) "FundingTemplateFields", (object) "FundingTemplates", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void RenameFundingTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [Name] = {1} WHERE [Name] = {2}", (object) "FundingTemplates", (object) SQL.EncodeString(targetName), (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] getFundingTemplatesFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] templatesFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        templatesFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["Name"]), FileSystemEntry.Types.File, (string) null);
        templatesFromDataTable[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["Description"]));
        templatesFromDataTable[index].Properties.Add((object) "For2010GFE", SQL.DecodeBoolean(source.Rows[index]["For2010GFE"]) ? (object) "Yes" : (object) "No");
        templatesFromDataTable[index].Properties.Add((object) "RESPAVERSION", (object) SQL.DecodeString(source.Rows[index]["RESPAVersion"]));
      }
      return templatesFromDataTable;
    }

    private static BinaryObject getFundingTemplateSettingsFromDataSet(DataSet dsFundingTemplate)
    {
      if (dsFundingTemplate.Tables.Count == 0 || dsFundingTemplate.Tables[0].Rows == null)
        return (BinaryObject) null;
      FundingTemplate settingsFromDataSet = (FundingTemplate) null;
      dsFundingTemplate.Relations.Add("FundingTemplateFields", dsFundingTemplate.Tables[0].Columns["FundingTemplateId"], dsFundingTemplate.Tables[1].Columns["FundingTemplateId"]);
      foreach (DataRow row in (InternalDataCollectionBase) dsFundingTemplate.Tables[0].Rows)
        settingsFromDataSet = new FundingTemplate(SQL.DecodeString(row["Name"]), SQL.DecodeString(row["Description"]), SQL.DecodeBoolean(row["For2010GFE"]), SQL.DecodeString(row["RESPAVersion"]), dsFundingTemplate.Tables[1].Select("FundingTemplateId = " + Convert.ToString(row["FundingTemplateId"])));
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataSet;
    }

    private static void populateFundingTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.FundingTemplate, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertFundingTemplates(latestVersion.Data);
        }
      }
    }

    private static bool insertFundingTemplates(BinaryObject data)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("FundingTemplates");
      DbValueList dbValueList = new DbValueList();
      FundingTemplate fundingTemplate = (FundingTemplate) data;
      dbValueList.Add("Name", (object) fundingTemplate.TemplateName);
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("Description", (object) fundingTemplate.Description);
      dbValueList.Add("IgnoreBusinessRules", fundingTemplate.IgnoreBusinessRules ? (object) "1" : (object) "0");
      dbValueList.Add("For2010GFE", fundingTemplate.For2010GFE ? (object) "1" : (object) "0");
      dbValueList.Add("RESPAVersion", (fundingTemplate.RESPAVersion ?? "") != "" ? (object) fundingTemplate.RESPAVersion : (object) (string) null);
      dbQueryBuilder.Declare("@FundingTemplateId", "INT");
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@FundingTemplateId");
      string[] assignedFieldIds = fundingTemplate.GetAssignedFieldIDs();
      if (assignedFieldIds != null && assignedFieldIds.Length != 0)
      {
        for (int index = 0; index < assignedFieldIds.Length; ++index)
        {
          if (fundingTemplate.GetField(assignedFieldIds[index]).ToLower() == "y")
          {
            dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([FundingTemplateId], [FieldKey])", (object) "FundingTemplateFields"));
            dbQueryBuilder.AppendLine(string.Format("VALUES(@FundingTemplateId, {0})", (object) SQL.EncodeString(assignedFieldIds[index])));
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    public static FileSystemEntry[] GetAllPublicFileEntriesForInvestorTemplates(
      bool includeProperties)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM InvestorTemplates ORDER BY [TEMPLATENAME] ASC");
      FileSystemEntry[] entries = SystemConfigurationAccessor.populateInvestorTemplatesFileSystemEntry(dbQueryBuilder.ExecuteTableQuery().Rows, includeProperties);
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.Investor, includeProperties);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populateInvestorTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetAllPublicFileEntriesForInvestorTemplates(includeProperties);
        }
      }
      return entries;
    }

    public static BinaryObject GetInvestorTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM [{0}] WHERE [TemplateName] = {1}", (object) "InvestorTemplates", (object) SQL.EncodeString(entry.Name)));
      dbQueryBuilder.AppendLine(string.Format("SELECT CE.* FROM {0} AS CE INNER JOIN {1} AS INV ON INV.[InvestorTemplateGuid] = CE.[InvestorTemplateGuid] AND INV.[TemplateName] = {2}", (object) "InvestorContactInformation", (object) "InvestorTemplates", (object) SQL.EncodeString(entry.Name)));
      return SystemConfigurationAccessor.getInvestorTemplateSettingsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
    }

    public static bool ExistsInvestorTemplateSettings(FileSystemEntry entry, bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(1) FROM {0} WHERE [TemplateName] = {1}", (object) "InvestorTemplates", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = !ofAnyType ? TemplateSettingsStore.Exists(TemplateSettingsType.Investor, entry) : TemplateSettingsStore.ExistsOfAnyType(TemplateSettingsType.Investor, entry);
      return flag;
    }

    public static void DeleteInvestorTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [TemplateName] = {1}", (object) "InvestorTemplates", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] populateInvestorTemplatesFileSystemEntry(
      DataRowCollection dataRows,
      bool includeProperties)
    {
      if (dataRows == null || dataRows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] fileSystemEntryArray = new FileSystemEntry[dataRows.Count];
      for (int index = 0; index < dataRows.Count; ++index)
      {
        fileSystemEntryArray[index] = new FileSystemEntry("\\", SQL.DecodeString(dataRows[index]["TemplateName"]), FileSystemEntry.Types.File, (string) null);
        fileSystemEntryArray[index].Properties.Add((object) "Description", (object) string.Empty);
        if (includeProperties)
        {
          Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
          insensitiveHashtable.Add((object) "Guid", (object) dataRows[index]["InvestorTemplateGuid"].ToString());
          insensitiveHashtable.Add((object) "BulkSale", SQL.DecodeBoolean(dataRows[index]["IsBulkSale"]) ? (object) "Yes" : (object) "No");
          insensitiveHashtable.Add((object) "DeliveryTimeFrame", (object) SQL.DecodeInt(dataRows[index]["DeliveryTimeFrame"]));
          insensitiveHashtable.Add((object) "PairOffFee", (object) SQL.DecodeInt(dataRows[index]["PairOffFee"]));
          insensitiveHashtable.Add((object) "TypeOfPurchaser", (object) SQL.DecodeString(dataRows[index]["TypeOfPurchaser"]));
          fileSystemEntryArray[index].Properties = insensitiveHashtable;
        }
      }
      return fileSystemEntryArray;
    }

    private static void populateInvestorTemplatesDatabase(FileSystemEntry[] entries)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@InvestorTemplateGuid", "VARCHAR(38)");
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.Investor, entries[index]))
        {
          if (latestVersion.Exists)
            dbQueryBuilder.AppendLine(SystemConfigurationAccessor.saveInvestorTemplatesIntoDatabase(latestVersion.Data));
        }
      }
      if (string.IsNullOrEmpty(dbQueryBuilder.ToString()))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static BinaryObject getInvestorTemplateSettingsFromDataSet(DataSet dtInvestorTemplates)
    {
      if (dtInvestorTemplates.Tables.Count == 0 || dtInvestorTemplates.Tables[0].Rows == null)
        return (BinaryObject) null;
      InvestorTemplate investorTemplate = (InvestorTemplate) null;
      dtInvestorTemplates.Relations.Add("InvestorTemplateGuid", dtInvestorTemplates.Tables[0].Columns["InvestorTemplateGuid"], dtInvestorTemplates.Tables[1].Columns["InvestorTemplateGuid"]);
      foreach (DataRow row in (InternalDataCollectionBase) dtInvestorTemplates.Tables[0].Rows)
      {
        investorTemplate = new InvestorTemplate(SQL.DecodeString(row["InvestorTemplateGuid"]));
        investorTemplate.TemplateName = SQL.DecodeString(row["TemplateName"]);
        investorTemplate.CompanyInformation.DeliveryTimeFrame = SQL.DecodeInt(row["DeliveryTimeFrame"]);
        investorTemplate.CompanyInformation.PairOffFee = (Decimal) SQL.DecodeInt(row["PairOffFee"]);
        investorTemplate.CompanyInformation.TypeOfPurchaser = SQL.DecodeString(row["TypeOfPurchaser"]);
        investorTemplate.BulkSale = SQL.DecodeBoolean(row["IsBulkSale"]);
        SystemConfigurationAccessor.populateContactInformation(investorTemplate, dtInvestorTemplates.Tables[1]);
      }
      return (BinaryObject) (BinaryConvertibleObject) investorTemplate;
    }

    private static void populateContactInformation(
      InvestorTemplate investorTemplate,
      DataTable dtInvestorContactInfo)
    {
      DataRowCollection rows = dtInvestorContactInfo.Rows;
      if (rows.Count <= 0)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
        investorTemplate.CompanyInformation.AddContactInformation(new ContactInformation()
        {
          EntityName = SQL.DecodeString(dataRow["EntityName"]),
          ContactName = SQL.DecodeString(dataRow["ContactName"]),
          Address = new EllieMae.EMLite.ClientServer.Address(SQL.DecodeString(dataRow["StreetAddress"]), (string) null, SQL.DecodeString(dataRow["City"]), SQL.DecodeString(dataRow["State"]), SQL.DecodeString(dataRow["ZipCode"])),
          PhoneNumber = SQL.DecodeString(dataRow["PhoneNumber"]),
          FaxNumber = SQL.DecodeString(dataRow["FaxNumber"]),
          EmailAddress = SQL.DecodeString(dataRow["EmailAddress"]),
          WebSite = SQL.DecodeString(dataRow["WebSite"])
        }, SQL.DecodeString(dataRow["InvestorContactType"]));
    }

    public static void SaveInvestorTemplatesSettings(BinaryObject data)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@InvestorTemplateGuid", "VARCHAR(38)");
      dbQueryBuilder.AppendLine(SystemConfigurationAccessor.saveInvestorTemplatesIntoDatabase(data));
      if (string.IsNullOrEmpty(dbQueryBuilder.ToString()))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static string saveInvestorTemplatesIntoDatabase(BinaryObject data)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("InvestorTemplates");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("InvestorContactInformation");
      DbValueList values = new DbValueList();
      InvestorTemplate investorTemplate = (InvestorTemplate) data;
      DbValue dbValue = new DbValue("TemplateName", (object) investorTemplate.TemplateName);
      DbValue key = new DbValue("InvestorTemplateGuid", (object) investorTemplate.Guid);
      values.Add(dbValue);
      values.Add(key);
      values.Add("IsBulkSale", (object) SQL.EncodeFlag(investorTemplate.BulkSale));
      if (investorTemplate.CompanyInformation != null)
      {
        values.Add("DeliveryTimeFrame", (object) investorTemplate.CompanyInformation.DeliveryTimeFrame);
        values.Add("PairOffFee", (object) investorTemplate.CompanyInformation.PairOffFee.ToString());
        values.Add("TypeOfPurchaser", (object) investorTemplate.CompanyInformation.TypeOfPurchaser);
      }
      dbQueryBuilder.AppendLine(string.Format("SET @InvestorTemplateGuid = {0}", (object) SQL.EncodeString(investorTemplate.Guid)));
      dbQueryBuilder.IfExists(table1, dbValue);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(table1, values, dbValue);
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table1, values, true, false);
      dbQueryBuilder.End();
      dbQueryBuilder.DeleteFrom(table2, key);
      foreach (string name in Enum.GetNames(typeof (InvestorContactType)))
      {
        ContactInformation contactInformation = investorTemplate.CompanyInformation.GetContactInformation(name);
        if (contactInformation != null)
          dbQueryBuilder.AppendLine(SystemConfigurationAccessor.saveInvestorContactInformation(contactInformation, name));
      }
      return dbQueryBuilder.ToString();
    }

    private static string saveInvestorContactInformation(
      ContactInformation contactInformation,
      string investorContactType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO [{0}]", (object) "InvestorContactInformation"));
      dbQueryBuilder.AppendLine("([InvestorTemplateGuid],[InvestorContactType],[EntityName],[ContactName],[StreetAddress],[City],[State],[ZipCode],[PhoneNumber],[FaxNumber],[EmailAddress],[Website])");
      dbQueryBuilder.AppendLine(string.Format(" VALUES (@InvestorTemplateGuid, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", (object) SQL.EncodeString(investorContactType.ToString()), (object) SQL.EncodeString(contactInformation.EntityName), (object) SQL.EncodeString(contactInformation.ContactName), (object) SQL.EncodeString(contactInformation.Address != null ? contactInformation.Address.Street1 : string.Empty), (object) SQL.EncodeString(contactInformation.Address != null ? contactInformation.Address.City : string.Empty), (object) SQL.EncodeString(contactInformation.Address != null ? contactInformation.Address.State : string.Empty), (object) SQL.EncodeString(contactInformation.Address != null ? contactInformation.Address.Zip : string.Empty), (object) SQL.EncodeString(contactInformation.PhoneNumber), (object) SQL.EncodeString(contactInformation.FaxNumber), (object) SQL.EncodeString(contactInformation.EmailAddress), (object) SQL.EncodeString(contactInformation.WebSite)));
      return dbQueryBuilder.ToString();
    }

    public static FileSystemEntry[] GetAffiliatedBusinessArrangementsTemplatesList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return SystemConfigurationAccessor.GetAffiliatedBusinessArrangementsTemplatesList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetAffiliatedBusinessArrangementsTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.AffiliatedBusinessArrangements, parentFolder, FileSystemEntry.Types.All, false, false);
      SystemConfigurationAccessor.populateAffiliatedBusinessArrangementsTemplatesDatabase(directoryEntries);
      return directoryEntries;
    }

    public static void DuplicateAffiliatedBusinessArrangementsTemplate(
      FileSystemEntry source,
      FileSystemEntry target)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@AffiliateTemplateID", "INT");
      string str = "[Description],[IsFolder],[IgnoreBusinessRules]";
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([TemplateName], [TemplatePath], {1})", (object) "AffiliatedBusinessArrangementsTemplates", (object) str));
      dbQueryBuilder.AppendLine(string.Format("SELECT {0}, {1}, {2} FROM {3} WHERE [TemplatePath] = {4}", (object) SQL.EncodeString(target.Name), (object) SQL.EncodeString(target.ToString()), (object) str, (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(source.ToString())));
      dbQueryBuilder.SelectIdentity("@AffiliateTemplateID");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([AffiliateTemplateID],[FieldID],[FieldValue])", (object) "AffiliatedFields"));
      dbQueryBuilder.AppendLine(string.Format("SELECT @AffiliateTemplateID, [FieldID], [FieldValue] FROM {0} AF ", (object) "AffiliatedFields"));
      dbQueryBuilder.AppendLine(string.Format("INNER JOIN {0} AFT ON AF.[AffiliateTemplateID] = AFT.[AffiliateTemplateID]", (object) "AffiliatedBusinessArrangementsTemplates"));
      dbQueryBuilder.AppendLine(string.Format("WHERE [TemplatePath] = {0}", (object) SQL.EncodeString(source.ToString())));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void RenameAffiliatedBusinessArrangementsTemplate(
      FileSystemEntry source,
      FileSystemEntry target)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [TemplateName] = {1}, [TemplatePath] = REPLACE([TemplatePath], {2}, {3}) WHERE [TemplatePath] = {2}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(target.Name), (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString())));
      if (source.Type == FileSystemEntry.Types.Folder)
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET TemplatePath = REPLACE(TemplatePath, {1}, {2})  WHERE TemplatePath  like {3}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(source.ToString() + "%")));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static BinaryObject GetAffiliatedBusinessArrangementsTemplateSettings(
      FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM [{0}] WHERE [TemplatePath] = {1}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.AppendLine(string.Format("SELECT AF.* FROM {0} AS ABT INNER JOIN {1} AS AF ON ABT.[AffiliateTemplateID] = AF.[AffiliateTemplateID] AND ABT.[TemplatePath] = {2}", (object) "AffiliatedBusinessArrangementsTemplates", (object) "AffiliatedFields", (object) SQL.EncodeString(entry.ToString())));
      return SystemConfigurationAccessor.getAffiliatedBusinessArrangementsTemplateSettingsFromDataSet(dbQueryBuilder.ExecuteSetQuery());
    }

    public static void SaveAffiliatedBusinessArrangementsTemplatesSettings(
      FileSystemEntry entry,
      BinaryObject data,
      bool isUpdate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@AffiliateTemplateID", "int");
      if (entry.Type == FileSystemEntry.Types.Folder && data == null)
      {
        AffiliateTemplate serializableObject = new AffiliateTemplate();
        serializableObject.TemplateName = entry.Name;
        data = new BinaryObject((IXmlSerializable) serializableObject);
      }
      dbQueryBuilder.AppendLine(SystemConfigurationAccessor.saveAffiliatedBusinessArrangementsTemplatesIntoDatabase(entry, data, isUpdate));
      if (string.IsNullOrEmpty(dbQueryBuilder.ToString()))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static bool ExistsAffiliatedBusinessArrangementsTemplateSettings(
      FileSystemEntry entry,
      bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(1) FROM {0} WHERE [TemplatePath] = {1}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(entry.ToString())));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = !ofAnyType ? TemplateSettingsStore.Exists(TemplateSettingsType.AffiliatedBusinessArrangements, entry) : TemplateSettingsStore.ExistsOfAnyType(TemplateSettingsType.AffiliatedBusinessArrangements, entry);
      return flag;
    }

    public static void DeleteAffiliatedBusinessArrangementsTemplate(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM [{0}] WHERE [TemplatePath] = {1}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static void populateAffiliatedBusinessArrangementsTemplatesDatabase(
      FileSystemEntry[] entries)
    {
      if (entries == null || entries.Length == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.Declare("@AffiliateTemplateID", "INT");
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("AffiliatedBusinessArrangementsTemplates");
      bool flag = false;
      foreach (FileSystemEntry entry in entries)
      {
        DbValue key = new DbValue("TemplatePath", (object) entry.ToString());
        dbQueryBuilder2.SelectFrom(table, key);
        DataRowCollection dataRowCollection = dbQueryBuilder2.Execute();
        dbQueryBuilder2.Reset();
        if (dataRowCollection == null || dataRowCollection.Count == 0)
        {
          flag = true;
          if (entry.Type == FileSystemEntry.Types.File)
          {
            using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.AffiliatedBusinessArrangements, entry))
            {
              if (latestVersion.Exists)
              {
                dbQueryBuilder1.AppendLine(SystemConfigurationAccessor.saveAffiliatedBusinessArrangementsTemplatesIntoDatabase(entry, latestVersion.Data, false));
                entry.Properties = latestVersion.Properties;
              }
            }
          }
          else if (entry.Type == FileSystemEntry.Types.Folder)
          {
            AffiliateTemplate serializableObject = new AffiliateTemplate();
            serializableObject.TemplateName = entry.Name;
            dbQueryBuilder1.AppendLine(SystemConfigurationAccessor.saveAffiliatedBusinessArrangementsTemplatesIntoDatabase(entry, new BinaryObject((IXmlSerializable) serializableObject), false));
          }
        }
        else
        {
          entry.Properties.Add((object) "Description", (object) SQL.DecodeString(dataRowCollection[0]["Description"]));
          entry.Properties.Add((object) "Name", (object) SQL.DecodeString(dataRowCollection[0]["TemplateName"]));
        }
      }
      if (!flag)
        return;
      dbQueryBuilder1.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static string saveAffiliatedBusinessArrangementsTemplatesIntoDatabase(
      FileSystemEntry entry,
      BinaryObject data,
      bool isUpdate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("AffiliatedBusinessArrangementsTemplates");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("AffiliatedFields");
      DbValueList values = new DbValueList();
      DbValue key1 = new DbValue("AffiliateTemplateID", (object) "@AffiliateTemplateID", (IDbEncoder) DbEncoding.None);
      AffiliateTemplate affiliateTemplate = (AffiliateTemplate) data;
      DbValue key2 = new DbValue("TemplatePath", (object) entry.ToString());
      values.Add(key2);
      values.Add("TemplateName", (object) affiliateTemplate.TemplateName);
      values.Add("Description", (object) affiliateTemplate.Description);
      values.Add("IsFolder", (object) SQL.EncodeFlag(entry.Type == FileSystemEntry.Types.Folder));
      values.Add("IgnoreBusinessRules", (object) SQL.EncodeFlag(affiliateTemplate.IgnoreBusinessRules));
      if (isUpdate)
      {
        dbQueryBuilder.AppendLine(string.Format("SELECT @AffiliateTemplateID = AffiliateTemplateID FROM [{0}] WHERE TemplatePath = {1}", (object) "AffiliatedBusinessArrangementsTemplates", (object) SQL.EncodeString(entry.ToString())));
        dbQueryBuilder.Update(table1, values, key2);
      }
      else
      {
        dbQueryBuilder.InsertInto(table1, values, true, false);
        dbQueryBuilder.SelectIdentity("@AffiliateTemplateID");
      }
      dbQueryBuilder.DeleteFrom(table2, key1);
      string[] assignedFieldIds = affiliateTemplate.GetAssignedFieldIDs();
      if (assignedFieldIds != null && assignedFieldIds.Length != 0)
      {
        foreach (string id in assignedFieldIds)
        {
          values.Clear();
          values.Add(key1);
          values.Add(new DbValue("FieldID", (object) id));
          values.Add(new DbValue("FieldValue", (object) affiliateTemplate.GetField(id)));
          dbQueryBuilder.InsertInto(table2, values, true, false);
        }
      }
      return dbQueryBuilder.ToString();
    }

    private static BinaryObject getAffiliatedBusinessArrangementsTemplateSettingsFromDataSet(
      DataSet dsAffiliteTemplates)
    {
      if (dsAffiliteTemplates.Tables.Count == 0 || dsAffiliteTemplates.Tables[0].Rows == null)
        return (BinaryObject) null;
      AffiliateTemplate affiliateTemplate = (AffiliateTemplate) null;
      dsAffiliteTemplates.Relations.Add("AffiliateTemplateID", dsAffiliteTemplates.Tables[0].Columns["AffiliateTemplateID"], dsAffiliteTemplates.Tables[1].Columns["AffiliateTemplateID"]);
      if (dsAffiliteTemplates.Tables[0].Rows.Count > 0)
      {
        DataRow row = dsAffiliteTemplates.Tables[0].Rows[0];
        affiliateTemplate = new AffiliateTemplate();
        affiliateTemplate.TemplateName = SQL.DecodeString(row["TemplateName"]);
        affiliateTemplate.Description = SQL.DecodeString(row["Description"]);
        affiliateTemplate.IgnoreBusinessRules = SQL.DecodeBoolean(row["IgnoreBusinessRules"]);
        SystemConfigurationAccessor.populateAffiliatedFieldsInformation(affiliateTemplate, dsAffiliteTemplates.Tables[1]);
      }
      return (BinaryObject) (BinaryConvertibleObject) affiliateTemplate;
    }

    private static void populateAffiliatedFieldsInformation(
      AffiliateTemplate affiliateTemplate,
      DataTable dtAffilitedFields)
    {
      DataRowCollection rows = dtAffilitedFields.Rows;
      if (rows.Count <= 0)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
        affiliateTemplate.SetField(SQL.DecodeString(dataRow["FieldID"]), SQL.DecodeString(dataRow["FieldValue"]));
    }

    public static FileSystemEntry[] GetPurchaseAdviceTemplatesList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return SystemConfigurationAccessor.GetPurchaseAdviceTemplatesList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetPurchaseAdviceTemplatesList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM PurchaseAdviceTemplate");
      FileSystemEntry[] entries = SystemConfigurationAccessor.getPurchaseAdviceTemplatesFromDataTable(dbQueryBuilder.ExecuteTableQuery());
      if (entries == null || entries.Length == 0)
      {
        entries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.PurchaseAdvice, parentFolder, FileSystemEntry.Types.All, false, true);
        if (entries != null && entries.Length != 0)
        {
          SystemConfigurationAccessor.populatePurchaseAdviceTemplatesDatabase(entries);
          entries = SystemConfigurationAccessor.GetPurchaseAdviceTemplatesList(parentFolder, userInfo);
        }
      }
      return entries;
    }

    public static BinaryObject GetPurchaseAdviceTemplateSettings(FileSystemEntry entry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [Name] = {1}", (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(entry.Name)));
      DataTable dtPurchaseAdviceTemplate = dbQueryBuilder.ExecuteTableQuery();
      DataTable dtPurchaseAdviceTemplateFields = (DataTable) null;
      if (dtPurchaseAdviceTemplate != null && dtPurchaseAdviceTemplate.Rows.Count > 0)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [PurchaseAdviceTemplateID] = {1}", (object) "PurchaseAdviceTemplateField", dtPurchaseAdviceTemplate.Rows[0]["PurchaseAdviceTemplateID"]));
        dtPurchaseAdviceTemplateFields = dbQueryBuilder.ExecuteTableQuery();
      }
      return SystemConfigurationAccessor.getPurchaseAdviceTemplateSettingsFromDataTable(dtPurchaseAdviceTemplate, dtPurchaseAdviceTemplateFields);
    }

    public static bool ExistsPurchaseAdviceTemplateSettings(FileSystemEntry entry, bool ofAnyType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE [Name] = {1}", (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(entry.Name)));
      if (Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0)
        flag = true;
      if (!flag)
        flag = SystemConfigurationAccessor.existsTemplate(ofAnyType, entry, TemplateSettingsType.PurchaseAdvice);
      return flag;
    }

    public static bool SavePurchaseAdviceTemplateSettings(BinaryObject data)
    {
      SystemConfigurationAccessor.insertPurchaseAdviceTemplates(data);
      return true;
    }

    public static void RenamePurchaseAdviceTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [Name] = {1} WHERE [Name] = {2}", (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(targetName), (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicatePurchaseAdviceTemplate(string sourceName, string targetName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@PurchaseAdviceTemplateID", "INT");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([Name], [Description], [IgnoreBR])", (object) "PurchaseAdviceTemplate"));
      dbQueryBuilder.AppendLine(string.Format("SELECT {0}, [Description], [IgnoreBR] FROM {1} WHERE [Name] = {2}", (object) SQL.EncodeString(targetName), (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.SelectIdentity("@PurchaseAdviceTemplateID");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([PurchaseAdviceTemplateID], [FieldKey], [FieldValue])", (object) "PurchaseAdviceTemplateField"));
      dbQueryBuilder.AppendLine(string.Format("SELECT @PurchaseAdviceTemplateID, [FieldKey], [FieldValue] FROM {0} WHERE [PurchaseAdviceTemplateID] = (SELECT [PurchaseAdviceTemplateID] FROM {1} WHERE [Name] = {2})", (object) "PurchaseAdviceTemplateField", (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(sourceName)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DeletePurchaseAdviceTemplate(string name)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [Name] = {1}", (object) "PurchaseAdviceTemplate", (object) SQL.EncodeString(name)));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static FileSystemEntry[] getPurchaseAdviceTemplatesFromDataTable(DataTable source)
    {
      if (source == null || source.Rows.Count == 0)
        return (FileSystemEntry[]) null;
      FileSystemEntry[] templatesFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        templatesFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["Name"]), FileSystemEntry.Types.File, (string) null);
        templatesFromDataTable[index].Properties.Add((object) "Name", (object) SQL.DecodeString(source.Rows[index]["Name"]));
        templatesFromDataTable[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["Description"]));
        SystemConfigurationAccessor.createTemplateFile(templatesFromDataTable[index], TemplateSettingsType.PurchaseAdvice, new SystemConfigurationAccessor.methodCallback(SystemConfigurationAccessor.GetPurchaseAdviceTemplateSettings));
      }
      return templatesFromDataTable;
    }

    private static BinaryObject getPurchaseAdviceTemplateSettingsFromDataTable(
      DataTable dtPurchaseAdviceTemplate,
      DataTable dtPurchaseAdviceTemplateFields)
    {
      if (dtPurchaseAdviceTemplate == null || dtPurchaseAdviceTemplate.Rows.Count == 0)
        return (BinaryObject) null;
      PurchaseAdviceTemplate settingsFromDataTable = new PurchaseAdviceTemplate();
      DataRow row1 = dtPurchaseAdviceTemplate.Rows[0];
      settingsFromDataTable.TemplateName = SQL.DecodeString(row1["Name"]);
      settingsFromDataTable.Description = SQL.DecodeString(row1["Description"]);
      settingsFromDataTable.IgnoreBusinessRules = SQL.DecodeBoolean(row1["IgnoreBR"]);
      if (dtPurchaseAdviceTemplateFields != null && dtPurchaseAdviceTemplateFields.Rows.Count > 0)
      {
        foreach (DataRow row2 in (InternalDataCollectionBase) dtPurchaseAdviceTemplateFields.Rows)
          settingsFromDataTable.SetField(SQL.DecodeString(row2["FieldKey"]), SQL.DecodeString(row2["FieldValue"]));
      }
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataTable;
    }

    private static void populatePurchaseAdviceTemplatesDatabase(FileSystemEntry[] entries)
    {
      for (int index = 0; index < entries.Length; ++index)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.PurchaseAdvice, entries[index]))
        {
          if (latestVersion.Exists)
            SystemConfigurationAccessor.insertPurchaseAdviceTemplates(latestVersion.Data);
        }
      }
    }

    private static bool insertPurchaseAdviceTemplates(BinaryObject data)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("PurchaseAdviceTemplate");
      DbValueList dbValueList = new DbValueList();
      PurchaseAdviceTemplate purchaseAdviceTemplate = (PurchaseAdviceTemplate) data;
      dbValueList.Add("Name", (object) purchaseAdviceTemplate.TemplateName);
      dbQueryBuilder.DeleteFrom(table, dbValueList);
      dbValueList.Add("Description", (object) purchaseAdviceTemplate.Description);
      dbValueList.Add("IgnoreBR", (object) SQL.EncodeFlag(purchaseAdviceTemplate.IgnoreBusinessRules));
      dbQueryBuilder.Declare("@PurchaseAdviceTemplateID", "INT");
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@PurchaseAdviceTemplateID");
      foreach (string assignedFieldId in purchaseAdviceTemplate.GetAssignedFieldIDs())
      {
        string field = purchaseAdviceTemplate.GetField(assignedFieldId);
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO {0} ([PurchaseAdviceTemplateID], [FieldKey], [FieldValue])", (object) "PurchaseAdviceTemplateField"));
        dbQueryBuilder.AppendLine(string.Format("VALUES(@PurchaseAdviceTemplateID, {0}, {1})", (object) SQL.Encode((object) assignedFieldId), (object) SQL.Encode((object) field)));
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    private static bool existsTemplate(
      bool ofAnyType,
      FileSystemEntry entry,
      TemplateSettingsType templateSettingsType)
    {
      return ofAnyType ? TemplateSettingsStore.ExistsOfAnyType(templateSettingsType, entry) : TemplateSettingsStore.Exists(templateSettingsType, entry);
    }

    private static void createTemplateFile(
      FileSystemEntry entry,
      TemplateSettingsType templateSettingsType,
      SystemConfigurationAccessor.methodCallback callback)
    {
      if (SystemConfigurationAccessor.existsTemplate(false, entry, templateSettingsType))
        return;
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(templateSettingsType, entry))
      {
        if (templateSettings.Exists)
          return;
        BinaryObject data = callback(entry);
        if (data == null)
          return;
        templateSettings.CheckIn(data);
      }
    }

    private static string regenerateGUID(FileSystemEntry entry, TemplateSettingsType templateType)
    {
      string str = Guid.NewGuid().ToString();
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(templateType, entry))
      {
        if (templateSettings.Exists)
        {
          BinaryObject data1 = (BinaryObject) null;
          switch (templateType)
          {
            case TemplateSettingsType.TradePriceAdjustment:
              PriceAdjustmentTemplate data2 = (PriceAdjustmentTemplate) templateSettings.Data;
              data2.Guid = str;
              data1 = (BinaryObject) (BinaryConvertibleObject) data2;
              break;
            case TemplateSettingsType.SRPTable:
              SRPTableTemplate data3 = (SRPTableTemplate) templateSettings.Data;
              data3.Guid = str;
              data1 = (BinaryObject) (BinaryConvertibleObject) data3;
              break;
          }
          templateSettings.CheckIn(data1);
        }
      }
      return str;
    }

    public static List<CustomTool> GetCustomTools(
      string name,
      int? device,
      bool? isGlobal,
      bool? isActive)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<CustomTool> customTools = (List<CustomTool>) null;
      DbValueList keys = new DbValueList();
      string[] columnNames = new string[13]
      {
        "CustomToolId",
        "Name",
        "Description",
        "URL",
        "Device",
        "IsActive",
        "ModifiedBy",
        "ModifiedDate",
        "IsGlobal",
        "IconUrl",
        "CustomAttributes",
        "VanityKey",
        "Tags"
      };
      if (device.HasValue)
        keys.Add(new DbValueList()
        {
          (DbValue) new DbFilterValue(nameof (device), (object) device)
        });
      if (isGlobal.HasValue)
        keys.Add(new DbValueList()
        {
          (DbValue) new DbFilterValue(nameof (isGlobal), (object) (isGlobal.GetValueOrDefault() ? 1 : 0))
        });
      if (isActive.HasValue)
        keys.Add(new DbValueList()
        {
          (DbValue) new DbFilterValue(nameof (isActive), (object) (isActive.GetValueOrDefault() ? 1 : 0))
        });
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("CustomTools"), columnNames, keys);
      if (name != null)
      {
        if (keys.Count == 0)
          dbQueryBuilder.AppendLine(" where name like '%" + SQL.Escape(name) + "%'");
        else
          dbQueryBuilder.AppendLine(" and name like '%" + SQL.Escape(name) + "%'");
      }
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            customTools = new List<CustomTool>();
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              customTools.Add(new CustomTool(SQL.DecodeString(dataRow["CustomToolId"]), SQL.DecodeString(dataRow["Name"]), SQL.DecodeString(dataRow["Description"]), SQL.DecodeString(dataRow["URL"]), SQL.DecodeInt(dataRow["Device"]), SQL.DecodeBoolean(dataRow["IsActive"]), SQL.DecodeString(dataRow["ModifiedBy"]), SQL.DecodeDateTime(dataRow["ModifiedDate"]), SQL.DecodeBoolean(dataRow["IsGlobal"]), (string) SQL.Decode(dataRow["IconUrl"], (object) null), (string) SQL.Decode(dataRow["CustomAttributes"], (object) null), (string) SQL.Decode(dataRow["VanityKey"], (object) null), (string) SQL.Decode(dataRow["Tags"], (object) null)));
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SystemConfigurationAccessor), ex);
        throw new Exception("Cannot read records from CustomTools table.\r\n" + ex.Message);
      }
      return customTools;
    }

    public static List<CustomTool> GetCustomtools(string customToolId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<CustomTool> customtools = (List<CustomTool>) null;
      string text = "SELECT * FROM CustomTools";
      if (customToolId != null)
        text = text + " WHERE CustomToolId = " + SQL.Encode((object) customToolId);
      dbQueryBuilder.AppendLine(text);
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
          {
            customtools = new List<CustomTool>();
            foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
              customtools.Add(new CustomTool(SQL.DecodeString(dataRow["CustomToolId"]), SQL.DecodeString(dataRow["Name"]), SQL.DecodeString(dataRow["Description"]), SQL.DecodeString(dataRow["URL"]), SQL.DecodeInt(dataRow["Device"]), SQL.DecodeBoolean(dataRow["IsActive"]), SQL.DecodeString(dataRow["ModifiedBy"]), SQL.DecodeDateTime(dataRow["ModifiedDate"]), SQL.DecodeBoolean(dataRow["IsGlobal"]), (string) SQL.Decode(dataRow["IconUrl"], (object) null), (string) SQL.Decode(dataRow["CustomAttributes"], (object) null), (string) SQL.Decode(dataRow["VanityKey"], (object) null), (string) SQL.Decode(dataRow["Tags"], (object) null)));
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SystemConfigurationAccessor), ex);
        throw new Exception("Cannot read records from CustomTools table.\r\n" + ex.Message);
      }
      return customtools;
    }

    public static CustomTool AddCustomtools(CustomTool customTool)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        customTool.CustomToolId = Guid.NewGuid();
        customTool.ModifiedDate = DateTime.Now;
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("CustomTools");
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "CustomToolId",
            (object) customTool.CustomToolId.ToString()
          },
          {
            "Name",
            (object) customTool.Name.Trim()
          },
          {
            "Description",
            (object) customTool.Description
          },
          {
            "URL",
            (object) customTool.Url.Trim()
          },
          {
            "Device",
            (object) customTool.Device
          },
          {
            "IsActive",
            (object) SQL.Encode((object) (customTool.IsActive ? 1 : 0))
          },
          {
            "ModifiedBy",
            (object) customTool.ModifiedBy
          },
          {
            "ModifiedDate",
            (object) customTool.ModifiedDate
          },
          {
            "IsGlobal",
            (object) SQL.Encode((object) (customTool.IsGlobal ? 1 : 0))
          },
          {
            "IconUrl",
            (object) customTool.IconUrl
          },
          {
            "CustomAttributes",
            (object) customTool.customAttributes
          },
          {
            "VanityKey",
            (object) customTool.VanityKey
          },
          {
            "Tags",
            (object) customTool.Tags
          }
        }, true, false);
        dbQueryBuilder.ExecuteNonQuery();
        return customTool;
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot add a new custom tool to CustomTools table Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static bool IsDuplicatedCustomTool(string name, bool isGlobal)
    {
      string text = "SELECT count(CustomToolId) FROM CustomTools WHERE [Name] = " + SQL.EncodeString(name.Trim()) + " AND [IsGlobal] = " + (object) (isGlobal ? 1 : 0);
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine(text);
        return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar(), 0) > 0;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static bool IsDuplicatedVanityKey(string vanityKey)
    {
      string text = "SELECT count(CustomToolId) FROM CustomTools WHERE [VanityKey] = " + SQL.EncodeString(vanityKey.Trim());
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine(text);
        return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar(), 0) > 0;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static bool DeleteCustomTool(string customToolId)
    {
      try
      {
        if (!string.IsNullOrEmpty(customToolId))
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE CustomEntityId = {1} AND ServiceType <> {2};", (object) "[Acl_LoConnectCustomServices]", (object) SQL.EncodeString(customToolId), (object) 1));
          dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE CustomEntityId = {1} AND ServiceType <> {2};", (object) "[Acl_LoConnectCustomServices_User]", (object) SQL.EncodeString(customToolId), (object) 1));
          dbQueryBuilder.AppendLine("DELETE FROM CustomTools WHERE CustomToolId = " + SQL.EncodeString(customToolId));
          dbQueryBuilder.ExecuteNonQuery();
          return true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Custom Tool cannot be deleted. Error: " + ex.Message);
        Err.Reraise(nameof (SystemConfigurationAccessor), ex);
      }
      return false;
    }

    public static bool UpdateCustomTool(CustomTool customTool)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string text = "UPDATE CustomTools SET [Name] = " + SQL.Encode((object) customTool.Name) + ",[Description] = " + SQL.Encode((object) customTool.Description) + ",[URL] = " + SQL.Encode((object) customTool.Url) + ",[Device] = " + SQL.Encode((object) customTool.Device) + ",[IsActive] = " + SQL.Encode((object) (customTool.IsActive ? 1 : 0)) + ",[ModifiedBy] = " + SQL.Encode((object) customTool.ModifiedBy) + ",[ModifiedDate] = " + SQL.Encode((object) DateTime.Now) + ",[IsGlobal] = " + SQL.Encode((object) (customTool.IsGlobal ? 1 : 0)) + ",[IconUrl] = " + SQL.Encode((object) customTool.IconUrl) + ",[CustomAttributes] = " + SQL.Encode((object) customTool.customAttributes) + ",[VanityKey] = " + SQL.Encode((object) customTool.VanityKey) + ",[Tags] = " + SQL.Encode((object) customTool.Tags) + " WHERE CustomToolId = " + SQL.EncodeString(customTool.CustomToolId.ToString());
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
        LoConnectCustomServicesAclDbAccessor.UpdateServiceType(customTool.CustomToolId.ToString(), customTool.IsGlobal ? LoServiceType.GlobalTool : LoServiceType.Tool);
        return true;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SystemConfigurationAccessor), "Changed custom tool cannot be updated. Error: " + ex.Message);
        Err.Reraise(nameof (SystemConfigurationAccessor), ex);
        return false;
      }
    }

    private delegate BinaryObject methodCallback(FileSystemEntry entry);
  }
}
