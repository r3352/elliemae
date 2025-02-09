// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EDisclosureConfigurationAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EDisclosureConfigurationAccessor
  {
    private const string E_DISCLOSURE_CHANNELS = "eDisclosureChannels�";
    private const string E_DISCLOSURE_CHANNEL_ELEMENTS = "eDisclosureChannelElements�";
    private const string E_DISCLOSURE_ELEMENT_ATTRIBUTES = "eDisclosureElementAttributes�";
    private const string E_DISCLOSURE_REQUIRED_FIELDS = "eDisclosureRequiredFields�";
    private const string E_DISCLOSURE_MSTEMPLATE_SETTINGS = "eDisclosureMSTemplateSettings�";
    private const string E_DISCLOSURE_PACKAGE_TYPES = "eDisclosurePackageTypes�";
    private const string MILESTONES = "Milestones�";
    private const string cacheName = "EDisclosureConfiguration�";
    private const string AUTO_RETRIEVE_SETTINGS = "AutoRetrieveSettings�";
    private static int eDisclosureElementAttributeID;

    private EDisclosureConfigurationAccessor()
    {
    }

    [PgReady]
    private static EDisclosureSetup GetEDisclosurePackageSetupFromDB(bool resetSqlData = true)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool flag1 = false;
      bool flag2 = true;
      string s = Company.GetCompanySetting("MIGRATION", "EDiclosureXMLFileDate") ?? "";
      EDisclosureSetup eDisclosureSetup;
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        string dataFilePath = current.Settings.GetDataFilePath("LoanSettings\\eDisclosures.XML");
        if (string.IsNullOrEmpty(s))
        {
          flag1 = true;
        }
        else
        {
          TimeSpan timeSpan = File.GetLastWriteTimeUtc(dataFilePath) - DateTime.Parse(s);
          timeSpan = timeSpan.Duration();
          if (timeSpan.TotalSeconds >= 1.0)
          {
            flag1 = true;
            TraceLog.WriteVerbose(nameof (EDisclosureConfigurationAccessor), "eDisclosures file date different than stored date.");
          }
        }
        if (flag1)
        {
          if (File.Exists(dataFilePath))
          {
            foreach (XElement element in XDocument.Load(dataFilePath).Root.Elements().Elements<XElement>().Elements<XElement>().Elements<XElement>())
            {
              if (element.FirstAttribute.Value == "IncludeLE")
              {
                flag2 = false;
                break;
              }
            }
          }
        }
        else
          flag2 = false;
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureChannelElements");
        if (flag2 & resetSqlData)
        {
          TraceLog.WriteVerbose(nameof (EDisclosureConfigurationAccessor), "eDisclosures file is old format. Clearing table to allow restore from file...");
          pgDbQueryBuilder.DeleteFrom(table1);
        }
        pgDbQueryBuilder.SelectFrom(table1);
        DataRowCollection dataRowCollection1 = pgDbQueryBuilder.Execute();
        if (dataRowCollection1 == null || dataRowCollection1.Count == 0)
        {
          EDisclosureSetup setup = EDisclosureConfiguration.GetSetup();
          if (setup == null)
            return setup;
          EDisclosureConfigurationAccessor.SaveEDisclosurePackageSetupToDB(setup, resetSqlData);
          Company.SetCompanySetting("MIGRATION", "EDiclosureXMLFileDate", File.GetLastWriteTimeUtc(dataFilePath).ToString());
          return EDisclosureConfigurationAccessor.GetEDisclosurePackageSetupFromDB();
        }
        eDisclosureSetup = new EDisclosureSetup();
        DataRow dataRow1 = dataRowCollection1[0];
        eDisclosureSetup.DefaultChannel = SQL.DecodeEnum<LoanChannel>(dataRow1["DefaultChannel"]);
        eDisclosureSetup.AllowESigningConventional = SQL.DecodeBoolean(dataRow1["AllowESigningConventional"]);
        eDisclosureSetup.AllowESigningFHA = SQL.DecodeBoolean(dataRow1["AllowESigningFHA"]);
        eDisclosureSetup.AllowESigningVA = SQL.DecodeBoolean(dataRow1["AllowESigningVA"]);
        eDisclosureSetup.AllowESigningUSDA = SQL.DecodeBoolean(dataRow1["AllowESigningUSDA"]);
        eDisclosureSetup.AllowESigningOther = SQL.DecodeBoolean(dataRow1["AllowESigningOther"]);
        eDisclosureSetup.AllowESigningHELOC = SQL.DecodeBoolean(dataRow1["AllowESigningHELOC"]);
        eDisclosureSetup.ConsentModelType = "Loan level consent";
        if (dataRow1["ConsentModelType"].ToString() == "Package level consent")
          eDisclosureSetup.ConsentModelType = "Package level consent";
        eDisclosureSetup.UseBranchAddress = SQL.DecodeBoolean(dataRow1["UseBranchAddress"]);
        pgDbQueryBuilder.Reset();
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureChannels");
        pgDbQueryBuilder.SelectFrom(table2);
        DataRowCollection dataRowCollection2 = pgDbQueryBuilder.Execute();
        if (dataRowCollection2 == null || dataRowCollection2.Count == 0)
          return eDisclosureSetup;
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureRequiredFields");
        DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureElementAttributes");
        foreach (DataRow dataRow2 in (InternalDataCollectionBase) dataRowCollection2)
        {
          string str = dataRow2["ChannelID"].ToString();
          LoanChannel loanChannel = SQL.DecodeEnum<LoanChannel>((object) str);
          EDisclosureChannel eDisclosureChannel = new EDisclosureChannel();
          eDisclosureChannel.IsBroker = SQL.DecodeBoolean(dataRow2["IsBroker"]);
          eDisclosureChannel.IsLender = SQL.DecodeBoolean(dataRow2["IsLender"]);
          eDisclosureChannel.IsInformationalOnly = SQL.DecodeBoolean(dataRow2["IsInformationOnly"]);
          eDisclosureChannel.InitialControl = SQL.DecodeEnum<ControlOptionType>(dataRow2["InitialControl"]);
          eDisclosureChannel.RedisclosureControl = SQL.DecodeEnum<ControlOptionType>(dataRow2["RedisclosureControl"]);
          pgDbQueryBuilder.Reset();
          DbValue key1 = new DbValue("ChannelID", (object) str);
          pgDbQueryBuilder.SelectFrom(table4, key1);
          DataRowCollection dataRowCollection3 = pgDbQueryBuilder.Execute();
          if (dataRowCollection3 != null && dataRowCollection3.Count > 0)
          {
            foreach (DataRow dataRow3 in (InternalDataCollectionBase) dataRowCollection3)
            {
              EDisclosurePackage eDisclosurePackage = new EDisclosurePackage();
              eDisclosurePackage.PackageType = SQL.DecodeEnum<EdisclosurePackageType>(dataRow3["PackageTypeID"]);
              eDisclosurePackage.Enabled = SQL.DecodeBoolean(dataRow3["Enabled"]);
              eDisclosurePackage.AtApplication = SQL.DecodeBoolean(dataRow3["AtApplication"]);
              eDisclosurePackage.ThreeDay = SQL.DecodeBoolean(dataRow3["ThreeDay"]);
              eDisclosurePackage.AtLock = SQL.DecodeBoolean(dataRow3["AtLock"]);
              eDisclosurePackage.Approval = SQL.DecodeBoolean(dataRow3["Approval"]);
              eDisclosurePackage.IncludeGFE = SQL.DecodeBoolean(dataRow3["IncludeGFE"]);
              eDisclosurePackage.IncludeTIL = SQL.DecodeBoolean(dataRow3["IncludeTIL"]);
              eDisclosurePackage.IncludeLE = SQL.DecodeBoolean(dataRow3["IncludeLE"]);
              eDisclosurePackage.RequirementType = SQL.DecodeEnum<PackageRequirementType>(dataRow3["RequirementType"]);
              eDisclosurePackage.RequiredAlert = SQL.DecodeEnum<StandardAlertID>(dataRow3["RequiredAlert"]);
              eDisclosurePackage.RequiredMilestone = SQL.DecodeString(dataRow3["RequiredMilestone"]);
              pgDbQueryBuilder.Reset();
              eDisclosureSetup.AutoRetrieveSettings = EDisclosureConfigurationAccessor.GetAutoRetrieveSettings();
              pgDbQueryBuilder.Reset();
              DbValue key2 = new DbValue("eDisclosureElementAttributeID", (object) dataRow3["eDisclosureElementAttributeID"].ToString());
              pgDbQueryBuilder.SelectFrom(table3, key2);
              DataRowCollection dataRowCollection4 = pgDbQueryBuilder.Execute();
              if (dataRowCollection4 != null && dataRowCollection4.Count > 0)
              {
                List<string> stringList = new List<string>();
                foreach (DataRow dataRow4 in (InternalDataCollectionBase) dataRowCollection4)
                  stringList.Add(SQL.DecodeString(dataRow4["FieldValue"]));
                eDisclosurePackage.RequiredFields = stringList.ToArray();
              }
              EDisclosureConfigurationAccessor.SetEDisclosurePackageType(ref eDisclosureChannel, eDisclosurePackage);
            }
          }
          EDisclosureConfigurationAccessor.SetChannelToEdisclosureSetup(ref eDisclosureSetup, eDisclosureChannel, loanChannel);
        }
      }
      else
      {
        string dataFilePath = current.Settings.GetDataFilePath("LoanSettings\\eDisclosures.XML");
        if (string.IsNullOrEmpty(s))
        {
          flag1 = true;
        }
        else
        {
          TimeSpan timeSpan = File.GetLastWriteTimeUtc(dataFilePath) - DateTime.Parse(s);
          timeSpan = timeSpan.Duration();
          if (timeSpan.TotalSeconds >= 1.0)
          {
            flag1 = true;
            TraceLog.WriteVerbose(nameof (EDisclosureConfigurationAccessor), "eDisclosures file date different than stored date.");
          }
        }
        if (flag1)
        {
          if (File.Exists(dataFilePath))
          {
            foreach (XElement element in XDocument.Load(dataFilePath).Root.Elements().Elements<XElement>().Elements<XElement>().Elements<XElement>())
            {
              if (element.FirstAttribute.Value == "IncludeLE")
              {
                flag2 = false;
                break;
              }
            }
          }
        }
        else
          flag2 = false;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table5 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureChannelElements");
        if (flag2 & resetSqlData)
        {
          TraceLog.WriteVerbose(nameof (EDisclosureConfigurationAccessor), "eDisclosures file is old format. Clearing table to allow restore from file...");
          dbQueryBuilder1.DeleteFrom(table5);
        }
        dbQueryBuilder1.SelectFrom(table5);
        DataRowCollection dataRowCollection5 = dbQueryBuilder1.Execute();
        if (dataRowCollection5 == null || dataRowCollection5.Count == 0)
        {
          EDisclosureSetup setup = EDisclosureConfiguration.GetSetup();
          if (setup == null)
            return setup;
          EDisclosureConfigurationAccessor.SaveEDisclosurePackageSetupToDB(setup, resetSqlData);
          Company.SetCompanySetting("MIGRATION", "EDiclosureXMLFileDate", File.GetLastWriteTimeUtc(dataFilePath).ToString());
          return EDisclosureConfigurationAccessor.GetEDisclosurePackageSetupFromDB();
        }
        eDisclosureSetup = new EDisclosureSetup();
        DataRow dataRow5 = dataRowCollection5[0];
        eDisclosureSetup.DefaultChannel = SQL.DecodeEnum<LoanChannel>((object) dataRow5["DefaultChannel"].ToString());
        eDisclosureSetup.AllowESigningConventional = SQL.DecodeBoolean((object) dataRow5["AllowESigningConventional"].ToString());
        eDisclosureSetup.AllowESigningFHA = SQL.DecodeBoolean((object) dataRow5["AllowESigningFHA"].ToString());
        eDisclosureSetup.AllowESigningVA = SQL.DecodeBoolean((object) dataRow5["AllowESigningVA"].ToString());
        eDisclosureSetup.AllowESigningUSDA = SQL.DecodeBoolean((object) dataRow5["AllowESigningUSDA"].ToString());
        eDisclosureSetup.AllowESigningOther = SQL.DecodeBoolean((object) dataRow5["AllowESigningOther"].ToString());
        eDisclosureSetup.AllowESigningHELOC = SQL.DecodeBoolean((object) dataRow5["AllowESigningHELOC"].ToString());
        eDisclosureSetup.ConsentModelType = "Loan level consent";
        if (dataRow5["ConsentModelType"].ToString() == "Package level consent")
          eDisclosureSetup.ConsentModelType = "Package level consent";
        eDisclosureSetup.UseBranchAddress = SQL.DecodeBoolean((object) dataRow5["UseBranchAddress"].ToString());
        eDisclosureSetup.AutoRetrieveSettings = EDisclosureConfigurationAccessor.GetAutoRetrieveSettings();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table6 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureChannels");
        dbQueryBuilder2.SelectFrom(table6);
        DataRowCollection dataRowCollection6 = dbQueryBuilder2.Execute();
        if (dataRowCollection6 == null || dataRowCollection6.Count == 0)
          return eDisclosureSetup;
        DbTableInfo table7 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureRequiredFields");
        DbTableInfo table8 = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureElementAttributes");
        foreach (DataRow dataRow6 in (InternalDataCollectionBase) dataRowCollection6)
        {
          string str = dataRow6["ChannelID"].ToString();
          LoanChannel loanChannel = SQL.DecodeEnum<LoanChannel>((object) str);
          EDisclosureChannel eDisclosureChannel = new EDisclosureChannel();
          eDisclosureChannel.IsBroker = SQL.DecodeBoolean(dataRow6["IsBroker"]);
          eDisclosureChannel.IsLender = SQL.DecodeBoolean(dataRow6["IsLender"]);
          eDisclosureChannel.IsInformationalOnly = SQL.DecodeBoolean(dataRow6["IsInformationOnly"]);
          eDisclosureChannel.InitialControl = SQL.DecodeEnum<ControlOptionType>((object) dataRow6["InitialControl"].ToString());
          eDisclosureChannel.RedisclosureControl = SQL.DecodeEnum<ControlOptionType>((object) dataRow6["RedisclosureControl"].ToString());
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = new EllieMae.EMLite.Server.DbQueryBuilder();
          DbValue key3 = new DbValue("ChannelID", (object) str);
          dbQueryBuilder3.SelectFrom(table8, key3);
          DataRowCollection dataRowCollection7 = dbQueryBuilder3.Execute();
          if (dataRowCollection7 != null && dataRowCollection7.Count > 0)
          {
            foreach (DataRow dataRow7 in (InternalDataCollectionBase) dataRowCollection7)
            {
              EDisclosurePackage eDisclosurePackage = new EDisclosurePackage();
              eDisclosurePackage.PackageType = SQL.DecodeEnum<EdisclosurePackageType>((object) dataRow7["PackageTypeID"].ToString());
              eDisclosurePackage.Enabled = SQL.DecodeBoolean(dataRow7["Enabled"]);
              eDisclosurePackage.AtApplication = SQL.DecodeBoolean(dataRow7["AtApplication"]);
              eDisclosurePackage.ThreeDay = SQL.DecodeBoolean(dataRow7["ThreeDay"]);
              eDisclosurePackage.AtLock = SQL.DecodeBoolean(dataRow7["AtLock"]);
              eDisclosurePackage.Approval = SQL.DecodeBoolean(dataRow7["Approval"]);
              eDisclosurePackage.IncludeGFE = SQL.DecodeBoolean(dataRow7["IncludeGFE"]);
              eDisclosurePackage.IncludeTIL = SQL.DecodeBoolean(dataRow7["IncludeTIL"]);
              eDisclosurePackage.IncludeLE = SQL.DecodeBoolean(dataRow7["IncludeLE"]);
              eDisclosurePackage.RequirementType = SQL.DecodeEnum<PackageRequirementType>((object) dataRow7["RequirementType"].ToString());
              eDisclosurePackage.RequiredAlert = SQL.DecodeEnum<StandardAlertID>((object) dataRow7["RequiredAlert"].ToString());
              eDisclosurePackage.RequiredMilestone = SQL.DecodeString(dataRow7["RequiredMilestone"]);
              EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder4 = new EllieMae.EMLite.Server.DbQueryBuilder();
              DbValue key4 = new DbValue("eDisclosureElementAttributeID", (object) dataRow7["eDisclosureElementAttributeID"].ToString());
              dbQueryBuilder4.SelectFrom(table7, key4);
              DataRowCollection dataRowCollection8 = dbQueryBuilder4.Execute();
              if (dataRowCollection8 != null && dataRowCollection8.Count > 0)
              {
                List<string> stringList = new List<string>();
                foreach (DataRow dataRow8 in (InternalDataCollectionBase) dataRowCollection8)
                  stringList.Add(SQL.DecodeString(dataRow8["FieldValue"]));
                eDisclosurePackage.RequiredFields = stringList.ToArray();
              }
              EDisclosureConfigurationAccessor.SetEDisclosurePackageType(ref eDisclosureChannel, eDisclosurePackage);
            }
          }
          EDisclosureConfigurationAccessor.SetChannelToEdisclosureSetup(ref eDisclosureSetup, eDisclosureChannel, loanChannel);
        }
      }
      return eDisclosureSetup;
    }

    public static EDisclosureSetup GetEDisclosurePackageSetup()
    {
      return ClientContext.GetCurrent().Cache.Get<EDisclosureSetup>("EDisclosureConfiguration", (Func<EDisclosureSetup>) (() => EDisclosureConfigurationAccessor.GetEDisclosurePackageSetupFromDB()), CacheSetting.Low);
    }

    [PgReady]
    private static void SaveEDisclosurePackageSetupToDB(EDisclosureSetup setup, bool resetXML = false)
    {
      EDisclosureConfigurationAccessor.eDisclosureElementAttributeID = 0;
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder1 = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder1.AppendLine("DO $$");
        pgDbQueryBuilder1.AppendLine("DECLARE v_MilestoneID VARCHAR(38);");
        pgDbQueryBuilder1.AppendLine("BEGIN");
        pgDbQueryBuilder1.AppendLine("Delete from eDisclosureRequiredFields;");
        pgDbQueryBuilder1.AppendLine("Delete from eDisclosureElementAttributes;");
        pgDbQueryBuilder1.AppendLine("Delete from eDisclosureChannels;");
        pgDbQueryBuilder1.AppendLine("Delete from eDisclosureChannelElements;");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder2 = pgDbQueryBuilder1;
        string[] strArray = new string[19];
        strArray[0] = "Insert into eDisclosureChannelElements (DefaultChannel, AllowESigningConventional, AllowESigningFHA,AllowESigningVA, AllowESigningUSDA, AllowESigningOther, AllowESigningHELOC, ConsentModelType, UseBranchAddress) Values(";
        strArray[1] = SQL.Encode((object) (int) setup.DefaultChannel);
        strArray[2] = ", ";
        bool flag = setup.AllowESigningConventional;
        strArray[3] = SQL.Encode((object) flag.ToString());
        strArray[4] = ", ";
        flag = setup.AllowESigningFHA;
        strArray[5] = SQL.Encode((object) flag.ToString());
        strArray[6] = ", ";
        flag = setup.AllowESigningVA;
        strArray[7] = SQL.Encode((object) flag.ToString());
        strArray[8] = ", ";
        flag = setup.AllowESigningUSDA;
        strArray[9] = SQL.Encode((object) flag.ToString());
        strArray[10] = ", ";
        flag = setup.AllowESigningOther;
        strArray[11] = SQL.Encode((object) flag.ToString());
        strArray[12] = ", ";
        flag = setup.AllowESigningHELOC;
        strArray[13] = SQL.Encode((object) flag.ToString());
        strArray[14] = ", ";
        strArray[15] = SQL.Encode((object) setup.ConsentModelType.ToString());
        strArray[16] = ", ";
        flag = setup.UseBranchAddress;
        strArray[17] = SQL.Encode((object) flag.ToString());
        strArray[18] = ");";
        string text = string.Concat(strArray);
        pgDbQueryBuilder2.AppendLine(text);
        pgDbQueryBuilder1.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.RetailChannel, LoanChannel.BankedRetail));
        pgDbQueryBuilder1.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.BrokerChannel, LoanChannel.Brokered));
        pgDbQueryBuilder1.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.CorrespondentChannel, LoanChannel.Correspondent));
        pgDbQueryBuilder1.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.WholesaleChannel, LoanChannel.BankedWholesale));
        pgDbQueryBuilder1.AppendLine("If Exists (Select  1 from eDisclosureMSTemplateSettings where eDisclosureElementAttributeID IS NULL)");
        pgDbQueryBuilder1.AppendLine("THEN");
        pgDbQueryBuilder1.AppendLine("UPDATE t1");
        pgDbQueryBuilder1.AppendLine("SET t1.eDisclosureElementAttributeID = t4.eDisclosureElementAttributeID");
        pgDbQueryBuilder1.AppendLine("FROM eDisclosureMSTemplateSettings AS t1");
        pgDbQueryBuilder1.AppendLine("LEFT JOIN eDisclosureChannels AS t2 ON t1.Channel = t2.Name");
        pgDbQueryBuilder1.AppendLine("LEFT JOIN eDisclosurePackageTypes AS t3 ON t1.Category = t3.PackageTypeName");
        pgDbQueryBuilder1.AppendLine("LEFT JOIN eDisclosureElementAttributes AS t4 ON t4.ChannelID = t2.ChannelID and t4.PackageTypeID = t3.PackageTypeID;");
        pgDbQueryBuilder1.AppendLine("END IF;");
        pgDbQueryBuilder1.AppendLine("END $$;");
        if (setup.AutoRetrieveSettings != null && setup.AutoRetrieveSettings.Count > 0)
        {
          foreach (AutoRetrieveSettings autoRetrieveSetting in setup.AutoRetrieveSettings)
          {
            pgDbQueryBuilder1.AppendLine("If Exists (Select  1 from AutoRetrieveSettings)");
            pgDbQueryBuilder1.AppendLine("UPDATE AutoRetrieveSettings SET Access = " + SQL.EncodeFlag(autoRetrieveSetting.Access));
            pgDbQueryBuilder1.AppendLine(", MilestoneId = " + (!string.IsNullOrEmpty(autoRetrieveSetting.MilestoneId) ? SQL.Encode((object) autoRetrieveSetting.MilestoneId) : SQL.Encode((object) null)));
            pgDbQueryBuilder1.AppendLine(" WHERE AutoRetrieveSettingsID = " + (object) autoRetrieveSetting.AutoRetrieveSettingsID);
          }
        }
        pgDbQueryBuilder1.ExecuteNonQuery(DbTransactionType.Default);
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Declare("@MilestoneID", "VARCHAR(38)");
        dbQueryBuilder.AppendLine("Delete eDisclosureRequiredFields");
        dbQueryBuilder.AppendLine("Delete eDisclosureElementAttributes");
        dbQueryBuilder.AppendLine("Delete eDisclosureChannels");
        dbQueryBuilder.AppendLine("Delete eDisclosureChannelElements");
        dbQueryBuilder.AppendLine("Insert into eDisclosureChannelElements (DefaultChannel, AllowESigningConventional, AllowESigningFHA,AllowESigningVA, AllowESigningUSDA, AllowESigningOther, AllowESigningHELOC, ConsentModelType, UseBranchAddress) Values(" + SQL.Encode((object) (int) setup.DefaultChannel) + ", " + SQL.Encode((object) setup.AllowESigningConventional.ToString()) + ", " + SQL.Encode((object) setup.AllowESigningFHA.ToString()) + ", " + SQL.Encode((object) setup.AllowESigningVA.ToString()) + ", " + SQL.Encode((object) setup.AllowESigningUSDA.ToString()) + ", " + SQL.Encode((object) setup.AllowESigningOther.ToString()) + ", " + SQL.Encode((object) setup.AllowESigningHELOC.ToString()) + ", " + SQL.Encode((object) setup.ConsentModelType.ToString()) + ", " + SQL.Encode((object) setup.UseBranchAddress.ToString()) + ")");
        dbQueryBuilder.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.RetailChannel, LoanChannel.BankedRetail));
        dbQueryBuilder.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.BrokerChannel, LoanChannel.Brokered));
        dbQueryBuilder.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.CorrespondentChannel, LoanChannel.Correspondent));
        dbQueryBuilder.Append(EDisclosureConfigurationAccessor.GetSqlBuilderForChannel(setup.WholesaleChannel, LoanChannel.BankedWholesale));
        dbQueryBuilder.AppendLine("If Exists (Select  1 from eDisclosureMSTemplateSettings where eDisclosureElementAttributeID IS NULL)");
        dbQueryBuilder.AppendLine("BEGIN");
        dbQueryBuilder.AppendLine("UPDATE t1");
        dbQueryBuilder.AppendLine("SET t1.eDisclosureElementAttributeID = t4.eDisclosureElementAttributeID");
        dbQueryBuilder.AppendLine("FROM eDisclosureMSTemplateSettings AS t1");
        dbQueryBuilder.AppendLine("LEFT JOIN eDisclosureChannels AS t2 ON t1.Channel = t2.Name");
        dbQueryBuilder.AppendLine("LEFT JOIN eDisclosurePackageTypes AS t3 ON t1.Category = t3.PackageTypeName");
        dbQueryBuilder.AppendLine("LEFT JOIN eDisclosureElementAttributes AS t4 ON t4.ChannelID = t2.ChannelID and t4.PackageTypeID = t3.PackageTypeID");
        dbQueryBuilder.AppendLine("END");
        if (setup.AutoRetrieveSettings != null && setup.AutoRetrieveSettings.Count > 0)
        {
          foreach (AutoRetrieveSettings autoRetrieveSetting in setup.AutoRetrieveSettings)
          {
            dbQueryBuilder.AppendLine("If Exists (Select  1 from AutoRetrieveSettings)");
            dbQueryBuilder.AppendLine("UPDATE AutoRetrieveSettings SET Access = " + SQL.EncodeFlag(autoRetrieveSetting.Access));
            dbQueryBuilder.AppendLine(", MilestoneId = " + (!string.IsNullOrEmpty(autoRetrieveSetting.MilestoneId) ? SQL.Encode((object) autoRetrieveSetting.MilestoneId) : SQL.Encode((object) null)));
            dbQueryBuilder.AppendLine(" WHERE AutoRetrieveSettingsID = " + (object) autoRetrieveSetting.AutoRetrieveSettingsID);
          }
        }
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      if (resetXML)
        setup = EDisclosureConfigurationAccessor.GetEDisclosurePackageSetupFromDB(false);
      EDisclosureConfiguration.SaveSetup(setup);
    }

    public static void SaveEDisclosurePackageSetup(EDisclosureSetup setup)
    {
      ClientContext.GetCurrent().Cache.Put<EDisclosureSetup>("EDisclosureConfiguration", (Action) (() => EDisclosureConfigurationAccessor.SaveEDisclosurePackageSetupToDB(setup)), (Func<EDisclosureSetup>) (() => EDisclosureConfigurationAccessor.GetEDisclosurePackageSetupFromDB()), CacheSetting.Low);
    }

    public static string GetEDisclosureElementAttributeId(int channelId, int edisclosurePackageType)
    {
      string empty = string.Empty;
      string columnName = "eDisclosureElementAttributeID";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("eDisclosureElementAttributes");
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        columnName
      }, new DbValueList()
      {
        new DbValue("ChannelId", (object) channelId),
        new DbValue("PackageTypeID", (object) edisclosurePackageType)
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        empty = dataRowCollection[0][columnName].ToString();
      return empty;
    }

    private static string GetChannelName(LoanChannel loanChannel)
    {
      switch (loanChannel)
      {
        case LoanChannel.BankedRetail:
          return "RetailChannel";
        case LoanChannel.BankedWholesale:
          return "WholesaleChannel";
        case LoanChannel.Brokered:
          return "BrokerChannel";
        case LoanChannel.Correspondent:
          return "CorrespondentChannel";
        default:
          return "None";
      }
    }

    [PgReady]
    private static string GetSqlBuilderForChannel(
      EDisclosureChannel channel,
      LoanChannel loanPackageType)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        string channelName = EDisclosureConfigurationAccessor.GetChannelName(loanPackageType);
        pgDbQueryBuilder.AppendLine("Insert into eDisclosureChannels (ChannelID, Name, IsBroker, IsLender, IsInformationOnly, InitialControl, RedisclosureControl) Values( " + SQL.Encode((object) (int) loanPackageType) + ", " + SQL.Encode((object) channelName) + ", " + SQL.Encode((object) channel.IsBroker.ToString()) + ", " + SQL.Encode((object) channel.IsLender.ToString()) + ", " + SQL.Encode((object) channel.IsInformationalOnly.ToString()) + ", " + SQL.Encode((object) (int) channel.InitialControl) + ", " + SQL.Encode((object) (int) channel.RedisclosureControl) + ");");
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.AllLoans, EdisclosurePackageType.AllLoans, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalApplication, EdisclosurePackageType.ConditionalApplication, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalThreeDay, EdisclosurePackageType.ConditionalThreeDay, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalLock, EdisclosurePackageType.ConditionalLock, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalApproval, EdisclosurePackageType.ConditionalApproval, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedAPR, EdisclosurePackageType.ChangedAPR, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedCircumstance, EdisclosurePackageType.ChangedCircumstance, loanPackageType));
        pgDbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedLock, EdisclosurePackageType.ChangedLock, loanPackageType));
        return pgDbQueryBuilder.ToString();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string channelName1 = EDisclosureConfigurationAccessor.GetChannelName(loanPackageType);
      dbQueryBuilder.AppendLine("Insert into eDisclosureChannels (ChannelID, Name, IsBroker, IsLender, IsInformationOnly, InitialControl, RedisclosureControl) Values( " + SQL.Encode((object) (int) loanPackageType) + ", " + SQL.Encode((object) channelName1) + ", " + SQL.Encode((object) channel.IsBroker.ToString()) + ", " + SQL.Encode((object) channel.IsLender.ToString()) + ", " + SQL.Encode((object) channel.IsInformationalOnly.ToString()) + ", " + SQL.Encode((object) (int) channel.InitialControl) + ", " + SQL.Encode((object) (int) channel.RedisclosureControl) + ")");
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.AllLoans, EdisclosurePackageType.AllLoans, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalApplication, EdisclosurePackageType.ConditionalApplication, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalThreeDay, EdisclosurePackageType.ConditionalThreeDay, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalLock, EdisclosurePackageType.ConditionalLock, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ConditionalApproval, EdisclosurePackageType.ConditionalApproval, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedAPR, EdisclosurePackageType.ChangedAPR, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedCircumstance, EdisclosurePackageType.ChangedCircumstance, loanPackageType));
      dbQueryBuilder.AppendLine(EDisclosureConfigurationAccessor.GetSqlBuilderForEDisclosurePackage(channel.ChangedLock, EdisclosurePackageType.ChangedLock, loanPackageType));
      return dbQueryBuilder.ToString();
    }

    [PgReady]
    private static string GetSqlBuilderForEDisclosurePackage(
      EDisclosurePackage eDisclosurePackage,
      EdisclosurePackageType packageType,
      LoanChannel loanChannel)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder1 = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        if (eDisclosurePackage != null)
        {
          ++EDisclosureConfigurationAccessor.eDisclosureElementAttributeID;
          if (!string.IsNullOrEmpty(eDisclosurePackage.RequiredMilestone))
            pgDbQueryBuilder1.AppendLine(string.Format("SELECT MilestoneID into v_MilestoneID FROM [{0}] WHERE MilestoneID = {1};", (object) "Milestones", (object) SQL.EncodeString(eDisclosurePackage.RequiredMilestone)));
          else
            pgDbQueryBuilder1.AppendLine("select NULL into v_MilestoneID;");
          EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder2 = pgDbQueryBuilder1;
          object[] objArray = new object[27];
          objArray[0] = (object) "Insert into eDisclosureElementAttributes(eDisclosureElementAttributeID, ChannelID, PackageTypeID, Enabled, AtApplication, ThreeDay, AtLock, Approval, IncludeGFE, IncludeTIL, IncludeLE, RequirementType, RequiredAlert, RequiredMilestone) Values(";
          objArray[1] = (object) EDisclosureConfigurationAccessor.eDisclosureElementAttributeID;
          objArray[2] = (object) ",";
          objArray[3] = (object) Convert.ToInt32((object) loanChannel);
          objArray[4] = (object) ",";
          objArray[5] = (object) SQL.Encode((object) Convert.ToInt32((object) packageType));
          objArray[6] = (object) ", ";
          objArray[7] = (object) SQL.Encode((object) eDisclosurePackage.Enabled.ToString());
          objArray[8] = (object) ", ";
          objArray[9] = (object) SQL.Encode((object) eDisclosurePackage.AtApplication.ToString());
          objArray[10] = (object) ", ";
          bool flag = eDisclosurePackage.ThreeDay;
          objArray[11] = (object) SQL.Encode((object) flag.ToString());
          objArray[12] = (object) ", ";
          flag = eDisclosurePackage.AtLock;
          objArray[13] = (object) SQL.Encode((object) flag.ToString());
          objArray[14] = (object) ", ";
          flag = eDisclosurePackage.Approval;
          objArray[15] = (object) SQL.Encode((object) flag.ToString());
          objArray[16] = (object) ", ";
          flag = eDisclosurePackage.IncludeGFE;
          objArray[17] = (object) SQL.Encode((object) flag.ToString());
          objArray[18] = (object) ", ";
          flag = eDisclosurePackage.IncludeTIL;
          objArray[19] = (object) SQL.Encode((object) flag.ToString());
          objArray[20] = (object) ", ";
          flag = eDisclosurePackage.IncludeLE;
          objArray[21] = (object) SQL.Encode((object) flag.ToString());
          objArray[22] = (object) ", ";
          objArray[23] = (object) SQL.Encode((object) Convert.ToInt32((object) eDisclosurePackage.RequirementType));
          objArray[24] = (object) ", ";
          objArray[25] = (object) SQL.Encode((object) Convert.ToInt32((object) eDisclosurePackage.RequiredAlert));
          objArray[26] = (object) ", v_MilestoneID);";
          string text = string.Concat(objArray);
          pgDbQueryBuilder2.AppendLine(text);
          if (eDisclosurePackage.RequiredFields != null && eDisclosurePackage.RequiredFields.Length != 0)
          {
            foreach (string requiredField in eDisclosurePackage.RequiredFields)
              pgDbQueryBuilder1.AppendLine(string.Format("Insert into {0} (eDisclosureElementAttributeID, FieldValue) values ({1},{2});", (object) "eDisclosureRequiredFields", (object) EDisclosureConfigurationAccessor.eDisclosureElementAttributeID, (object) SQL.Encode((object) requiredField)));
          }
        }
        return pgDbQueryBuilder1.ToString();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (eDisclosurePackage != null)
      {
        ++EDisclosureConfigurationAccessor.eDisclosureElementAttributeID;
        dbQueryBuilder1.SelectVar("@MilestoneID", (object) DBNull.Value);
        if (!string.IsNullOrEmpty(eDisclosurePackage.RequiredMilestone))
          dbQueryBuilder1.AppendLine(string.Format("SELECT @MilestoneID = MilestoneID FROM [{0}] WHERE MilestoneID = {1}", (object) "Milestones", (object) SQL.EncodeString(eDisclosurePackage.RequiredMilestone)));
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
        object[] objArray = new object[27];
        objArray[0] = (object) "Insert into eDisclosureElementAttributes(eDisclosureElementAttributeID, ChannelID, PackageTypeID, Enabled, AtApplication, ThreeDay, AtLock, Approval, IncludeGFE, IncludeTIL, IncludeLE, RequirementType, RequiredAlert, RequiredMilestone) Values(";
        objArray[1] = (object) EDisclosureConfigurationAccessor.eDisclosureElementAttributeID;
        objArray[2] = (object) ",";
        objArray[3] = (object) (int) loanChannel;
        objArray[4] = (object) ",";
        objArray[5] = (object) SQL.Encode((object) (int) packageType);
        objArray[6] = (object) ", ";
        objArray[7] = (object) SQL.Encode((object) eDisclosurePackage.Enabled.ToString());
        objArray[8] = (object) ", ";
        objArray[9] = (object) SQL.Encode((object) eDisclosurePackage.AtApplication.ToString());
        objArray[10] = (object) ", ";
        bool flag = eDisclosurePackage.ThreeDay;
        objArray[11] = (object) SQL.Encode((object) flag.ToString());
        objArray[12] = (object) ", ";
        flag = eDisclosurePackage.AtLock;
        objArray[13] = (object) SQL.Encode((object) flag.ToString());
        objArray[14] = (object) ", ";
        flag = eDisclosurePackage.Approval;
        objArray[15] = (object) SQL.Encode((object) flag.ToString());
        objArray[16] = (object) ", ";
        flag = eDisclosurePackage.IncludeGFE;
        objArray[17] = (object) SQL.Encode((object) flag.ToString());
        objArray[18] = (object) ", ";
        flag = eDisclosurePackage.IncludeTIL;
        objArray[19] = (object) SQL.Encode((object) flag.ToString());
        objArray[20] = (object) ", ";
        flag = eDisclosurePackage.IncludeLE;
        objArray[21] = (object) SQL.Encode((object) flag.ToString());
        objArray[22] = (object) ", ";
        objArray[23] = (object) SQL.Encode((object) (int) eDisclosurePackage.RequirementType);
        objArray[24] = (object) ", ";
        objArray[25] = (object) SQL.Encode((object) (int) eDisclosurePackage.RequiredAlert);
        objArray[26] = (object) ", @MilestoneID)";
        string text = string.Concat(objArray);
        dbQueryBuilder2.AppendLine(text);
        if (eDisclosurePackage.RequiredFields != null && eDisclosurePackage.RequiredFields.Length != 0)
        {
          foreach (string requiredField in eDisclosurePackage.RequiredFields)
            dbQueryBuilder1.AppendLine(string.Format("Insert into {0} (eDisclosureElementAttributeID, FieldValue) values ({1},{2})", (object) "eDisclosureRequiredFields", (object) EDisclosureConfigurationAccessor.eDisclosureElementAttributeID, (object) SQL.Encode((object) requiredField)));
        }
      }
      return dbQueryBuilder1.ToString();
    }

    private static void SetEDisclosurePackageType(
      ref EDisclosureChannel eDisclosureChannel,
      EDisclosurePackage eDisclosurePackage)
    {
      switch (eDisclosurePackage.PackageType)
      {
        case EdisclosurePackageType.AllLoans:
          eDisclosureChannel.AllLoans = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ConditionalApplication:
          eDisclosureChannel.ConditionalApplication = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ConditionalThreeDay:
          eDisclosureChannel.ConditionalThreeDay = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ConditionalLock:
          eDisclosureChannel.ConditionalLock = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ConditionalApproval:
          eDisclosureChannel.ConditionalApproval = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ChangedAPR:
          eDisclosureChannel.ChangedAPR = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ChangedCircumstance:
          eDisclosureChannel.ChangedCircumstance = eDisclosurePackage;
          break;
        case EdisclosurePackageType.ChangedLock:
          eDisclosureChannel.ChangedLock = eDisclosurePackage;
          break;
      }
    }

    private static void SetChannelToEdisclosureSetup(
      ref EDisclosureSetup eDisclosureSetup,
      EDisclosureChannel eDisclosureChannel,
      LoanChannel loanChannel)
    {
      switch (loanChannel)
      {
        case LoanChannel.BankedRetail:
          eDisclosureSetup.RetailChannel = eDisclosureChannel;
          break;
        case LoanChannel.BankedWholesale:
          eDisclosureSetup.WholesaleChannel = eDisclosureChannel;
          break;
        case LoanChannel.Brokered:
          eDisclosureSetup.BrokerChannel = eDisclosureChannel;
          break;
        case LoanChannel.Correspondent:
          eDisclosureSetup.CorrespondentChannel = eDisclosureChannel;
          break;
      }
    }

    public static DataTable GetAllEdisclosureElementAttributes()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("select * from eDisclosureElementAttributes");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    public static List<AutoRetrieveSettings> GetAutoRetrieveSettings()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("AutoRetrieveSettings");
      dbQueryBuilder.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<AutoRetrieveSettings> retrieveSettings1 = new List<AutoRetrieveSettings>();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          AutoRetrieveSettings retrieveSettings2 = new AutoRetrieveSettings()
          {
            AutoRetrieveSettingsID = SQL.DecodeInt(dataRow["AutoRetrieveSettingsID"]),
            Access = SQL.DecodeBoolean(dataRow["Access"]),
            ParentRetrieveSettingsId = (int?) SQL.Decode(dataRow["ParentRetrieveSettingsId"], (object) null),
            ShortDescription = SQL.DecodeString(dataRow["ShortDescription"]),
            MilestoneId = SQL.DecodeString(dataRow["MilestoneId"])
          };
          retrieveSettings1.Add(retrieveSettings2);
        }
      }
      return retrieveSettings1;
    }
  }
}
