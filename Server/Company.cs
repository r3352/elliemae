// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Company
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class Company
  {
    private const string className = "Company�";
    private const string CacheName = "COMPANYDATA�";

    public static string GetCacheKey(string section) => "COMPANYDATA/" + section.ToUpper();

    private static bool CacheEnabled(IClientContext context) => context.Cache.CacheSetting != 0;

    private Company()
    {
    }

    public static CompanyInfo GetCompanyInfo()
    {
      return Company.GetCompanyInfo((IClientContext) ClientContext.GetCurrent());
    }

    public static CompanyInfo GetCompanyInfo(IClientContext context)
    {
      return Company.getCompanyInfo(Company.GetCompanySettings(context, "CLIENT"));
    }

    [PgReady]
    public static void UpdateCompanyInfo(CompanyInfo info)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<Hashtable>(Company.GetCacheKey("CLIENT"), (Action) (() =>
      {
        if (context.Settings.DbServerType == DbServerType.Postgres)
        {
          PgDbQueryBuilder sql = new PgDbQueryBuilder((IClientContext) context);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTNAME", info.Name);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTADDRESS", info.Address);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTCITY", info.City);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTSTATE", info.State);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTZIP", info.Zip);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTPHONE", info.Phone);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTFAX", info.Fax);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME1", info.DBAName1);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME2", info.DBAName2);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME3", info.DBAName3);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME4", info.DBAName4);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLENDERTYPE", info.StateBranchLicensing.LenderType);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTHOMESTATE", info.StateBranchLicensing.HomeState);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTOPTOUT", info.StateBranchLicensing.OptOut);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "STATUTORYELECTIONINMARYLAND", info.StateBranchLicensing.StatutoryElectionInMaryland ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "STATUTORYELECTIONINKANSAS", info.StateBranchLicensing.StatutoryElectionInKansas ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "USECUSTOMLENDERPROFILE", info.StateBranchLicensing.UseCustomLenderProfile ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "ATRSMALLCREDITOR", ((int) info.StateBranchLicensing.ATRSmallCreditor).ToString());
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "ATREXEMPTCREDITOR", ((int) info.StateBranchLicensing.ATRExemptCreditor).ToString());
          string empty = string.Empty;
          Hashtable hashtable = new Hashtable();
          for (int index = 0; index < info.StateBranchLicensing.StateLicenseExtTypes.Count; ++index)
          {
            if (!hashtable.ContainsKey((object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation))
              hashtable.Add((object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation, (object) "");
            string str1 = hashtable[(object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation].ToString();
            if (str1 != string.Empty)
              str1 += "@";
            object[] objArray = new object[24];
            objArray[0] = (object) str1;
            objArray[1] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation;
            objArray[2] = (object) "|";
            objArray[3] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseType;
            objArray[4] = (object) "|";
            objArray[5] = info.StateBranchLicensing.StateLicenseExtTypes[index].Selected ? (object) "Y" : (object) "";
            objArray[6] = (object) "|";
            objArray[7] = info.StateBranchLicensing.StateLicenseExtTypes[index].Exempt ? (object) "Y" : (object) "";
            objArray[8] = (object) "|";
            objArray[9] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseNo;
            objArray[10] = (object) "|";
            DateTime dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].IssueDate;
            objArray[11] = (object) dateTime.ToString();
            objArray[12] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].StartDate;
            objArray[13] = (object) dateTime.ToString();
            objArray[14] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].EndDate;
            objArray[15] = (object) dateTime.ToString();
            objArray[16] = (object) "|";
            objArray[17] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseStatus;
            objArray[18] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].StatusDate;
            objArray[19] = (object) dateTime.ToString();
            objArray[20] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].LastChecked;
            objArray[21] = (object) dateTime.ToString();
            objArray[22] = (object) "|";
            objArray[23] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].SortIndex;
            string str2 = string.Concat(objArray);
            hashtable[(object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation] = (object) str2;
          }
          string[] states = Utils.GetStates();
          for (int index = 0; index < states.Length; ++index)
          {
            if (string.Compare(states[index], "GU", true) != 0 && string.Compare(states[index], "VI", true) != 0 && string.Compare(states[index], "PR", true) != 0)
            {
              if (!hashtable.ContainsKey((object) states[index]))
                Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLICENSING_" + states[index], "");
              else
                Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLICENSING_" + states[index], hashtable[(object) states[index]].ToString());
            }
          }
          sql.ExecuteNonQuery();
          context.TraceLog.WriteVerbose(nameof (Company), "Company info saved to database");
        }
        else
        {
          DbQueryBuilder sql = new DbQueryBuilder((IClientContext) context);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTNAME", info.Name);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTADDRESS", info.Address);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTCITY", info.City);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTSTATE", info.State);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTZIP", info.Zip);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTPHONE", info.Phone);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTFAX", info.Fax);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME1", info.DBAName1);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME2", info.DBAName2);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME3", info.DBAName3);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTDBANAME4", info.DBAName4);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLENDERTYPE", info.StateBranchLicensing.LenderType);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTHOMESTATE", info.StateBranchLicensing.HomeState);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTOPTOUT", info.StateBranchLicensing.OptOut);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "STATUTORYELECTIONINMARYLAND", info.StateBranchLicensing.StatutoryElectionInMaryland ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "STATUTORYELECTIONINMARYLAND2", info.StateBranchLicensing.StatutoryElectionInMaryland2);
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "STATUTORYELECTIONINKANSAS", info.StateBranchLicensing.StatutoryElectionInKansas ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "USECUSTOMLENDERPROFILE", info.StateBranchLicensing.UseCustomLenderProfile ? "Y" : "");
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "ATRSMALLCREDITOR", ((int) info.StateBranchLicensing.ATRSmallCreditor).ToString());
          Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "ATREXEMPTCREDITOR", ((int) info.StateBranchLicensing.ATRExemptCreditor).ToString());
          string empty = string.Empty;
          Hashtable hashtable = new Hashtable();
          for (int index = 0; index < info.StateBranchLicensing.StateLicenseExtTypes.Count; ++index)
          {
            if (!hashtable.ContainsKey((object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation))
              hashtable.Add((object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation, (object) "");
            string str3 = hashtable[(object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation].ToString();
            if (str3 != string.Empty)
              str3 += "@";
            object[] objArray = new object[24];
            objArray[0] = (object) str3;
            objArray[1] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation;
            objArray[2] = (object) "|";
            objArray[3] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseType;
            objArray[4] = (object) "|";
            objArray[5] = info.StateBranchLicensing.StateLicenseExtTypes[index].Selected ? (object) "Y" : (object) "";
            objArray[6] = (object) "|";
            objArray[7] = info.StateBranchLicensing.StateLicenseExtTypes[index].Exempt ? (object) "Y" : (object) "";
            objArray[8] = (object) "|";
            objArray[9] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseNo;
            objArray[10] = (object) "|";
            DateTime dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].IssueDate;
            objArray[11] = (object) dateTime.ToString();
            objArray[12] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].StartDate;
            objArray[13] = (object) dateTime.ToString();
            objArray[14] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].EndDate;
            objArray[15] = (object) dateTime.ToString();
            objArray[16] = (object) "|";
            objArray[17] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].LicenseStatus;
            objArray[18] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].StatusDate;
            objArray[19] = (object) dateTime.ToString();
            objArray[20] = (object) "|";
            dateTime = info.StateBranchLicensing.StateLicenseExtTypes[index].LastChecked;
            objArray[21] = (object) dateTime.ToString();
            objArray[22] = (object) "|";
            objArray[23] = (object) info.StateBranchLicensing.StateLicenseExtTypes[index].SortIndex;
            string str4 = string.Concat(objArray);
            hashtable[(object) info.StateBranchLicensing.StateLicenseExtTypes[index].StateAbbrevation] = (object) str4;
          }
          string[] states = Utils.GetStates();
          for (int index = 0; index < states.Length; ++index)
          {
            if (string.Compare(states[index], "GU", true) != 0 && string.Compare(states[index], "VI", true) != 0 && string.Compare(states[index], "PR", true) != 0)
            {
              if (!hashtable.ContainsKey((object) states[index]))
                Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLICENSING_" + states[index], "");
              else
                Company.appendUpdateStatement((IClientContext) context, (BaseDbQueryBuilder) sql, "CLIENT", "CLIENTLICENSING_" + states[index], hashtable[(object) states[index]].ToString());
            }
          }
          sql.ExecuteNonQuery();
          context.TraceLog.WriteVerbose(nameof (Company), "Company info saved to database");
        }
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB((IClientContext) context, "CLIENT")), CacheSetting.Low);
    }

    public static HMDAInformation GetHMDAInformation()
    {
      string informationFromDb = Company.GetHMDAInformationFromDB(ClientContext.GetCurrent());
      return string.IsNullOrEmpty(informationFromDb) ? (HMDAInformation) null : new HMDAInformation(informationFromDb);
    }

    private static string GetHMDAInformationFromDB(ClientContext context)
    {
      return Company.GetCompanySetting((IClientContext) context, "HMDA", "HMDASettings");
    }

    public static void UpdateHMDAInformation(HMDAInformation hmdaInformation)
    {
      Company.SetCompanySetting("HMDA", "HMDASettings", hmdaInformation?.ToXmlString());
    }

    public static EncompassEdition GetCurrentEdition()
    {
      return Company.GetEdition(ClientContext.GetCurrent());
    }

    public static EncompassEdition GetEdition(ClientContext context)
    {
      LicenseInfo serverLicense = Company.GetServerLicense((IClientContext) context);
      return serverLicense == null ? EncompassEdition.None : serverLicense.Edition;
    }

    public static LicenseInfo GetServerLicense()
    {
      return Company.GetServerLicense((IClientContext) ClientContext.GetCurrent());
    }

    public static LicenseInfo GetServerLicense(IClientContext context)
    {
      string companySetting = Company.GetCompanySetting(context, "CLIENT", "LICENSE");
      try
      {
        return LicenseInfo.Parse(companySetting);
      }
      catch (Exception ex)
      {
        context.TraceLog.WriteError(nameof (Company), "Error reading server license: " + (object) ex);
        return (LicenseInfo) null;
      }
    }

    public static void UpdateServerLicense(LicenseInfo license)
    {
      Company.UpdateServerLicense((IClientContext) ClientContext.GetCurrent(), license);
    }

    public static void UpdateServerLicense(IClientContext context, LicenseInfo license)
    {
      Company.SetCompanySetting(context, "CLIENT", "LICENSE", license.ToString());
    }

    public static void SetCompanySetting(string category, string attribute, string value)
    {
      Company.SetCompanySetting((IClientContext) ClientContext.GetCurrent(), category, attribute, value);
    }

    public static void SetCompanySetting(
      IClientContext context,
      string category,
      string attribute,
      string value)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 340, nameof (SetCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
      context.Cache.PutIf<Hashtable>(Company.GetCacheKey(category), (Func<bool>) (() =>
      {
        if (Company.CacheEnabled(context))
        {
          Hashtable companySettings = Company.GetCompanySettings(context, category);
          if (companySettings.Contains((object) category) && string.Compare(value, (string) companySettings[(object) category], true) == 0)
            return false;
        }
        return true;
      }), (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder(context);
        Company.appendUpdateStatement(context, (BaseDbQueryBuilder) sql, category, attribute, value);
        sql.ExecuteNonQuery();
        context.TraceLog.WriteVerbose(nameof (Company), "Company setting \"" + category + "\\" + attribute + "\" saved to database");
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
      PerformanceMeter.Current.AddCheckpoint("END", 372, nameof (SetCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
    }

    public static void SetCompanySettings(string category, Dictionary<string, string> settings)
    {
      Company.SetCompanySetting((IClientContext) ClientContext.GetCurrent(), category, settings);
    }

    public static void SetCompanySetting(
      IClientContext context,
      string category,
      Dictionary<string, string> settings)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 393, nameof (SetCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
      context.Cache.Put<Hashtable>(Company.GetCacheKey(category), (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder(context);
        foreach (KeyValuePair<string, string> setting in settings)
          Company.appendUpdateStatement(context, (BaseDbQueryBuilder) sql, category, setting.Key, setting.Value);
        sql.ExecuteNonQuery();
        context.TraceLog.WriteVerbose(nameof (Company), "Company settings in \"" + category + "\" saved to database");
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
      PerformanceMeter.Current.AddCheckpoint("END", 413, nameof (SetCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
    }

    public static void DeleteCompanySettings(string category, IEnumerable<string> attributes)
    {
      IClientContext context = (IClientContext) ClientContext.GetCurrent();
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 424, nameof (DeleteCompanySettings), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
      context.Cache.Put<Hashtable>(Company.GetCacheKey(category), (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder(context);
        foreach (string attribute in attributes)
          Company.appendDeleteStatement(context, (BaseDbQueryBuilder) sql, category, attribute);
        sql.ExecuteNonQuery();
        context.TraceLog.WriteVerbose(nameof (Company), "Company setting \"" + category + "\\" + (object) attributes + "\" deleted from database");
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
      PerformanceMeter.Current.AddCheckpoint("END", 440, nameof (DeleteCompanySettings), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
    }

    public static void DeleteCompanySetting(string category, string attribute)
    {
      IClientContext context = (IClientContext) ClientContext.GetCurrent();
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 452, nameof (DeleteCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
      context.Cache.Put<Hashtable>(Company.GetCacheKey(category), (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder(context);
        Company.appendDeleteStatement(context, (BaseDbQueryBuilder) sql, category, attribute);
        sql.ExecuteNonQuery();
        context.TraceLog.WriteVerbose(nameof (Company), "Company setting \"" + category + "\\" + attribute + "\" deleted from database");
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
      PerformanceMeter.Current.AddCheckpoint("END", 471, nameof (DeleteCompanySetting), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
    }

    public static void DeleteCompanySettings(string category)
    {
      IClientContext context = (IClientContext) ClientContext.GetCurrent();
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 483, nameof (DeleteCompanySettings), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
      context.Cache.Put<Hashtable>(Company.GetCacheKey(category), (Action) (() =>
      {
        DbQueryBuilder sql = new DbQueryBuilder(context);
        Company.appendDeleteStatement(context, (BaseDbQueryBuilder) sql, category);
        sql.ExecuteNonQuery();
        context.TraceLog.WriteVerbose(nameof (Company), "Company setting section: \"" + category + "\" deleted from database");
      }), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
      PerformanceMeter.Current.AddCheckpoint("END", 502, nameof (DeleteCompanySettings), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Company.cs");
    }

    public static string GetCompanySetting(string category, string attribute)
    {
      return Company.GetCompanySetting((IClientContext) ClientContext.GetCurrent(), category, attribute);
    }

    [PgReady]
    public static string GetCompanySetting(IClientContext context, string section, string key)
    {
      Hashtable companySettings = Company.GetCompanySettings(context, section);
      if (key.Length > 32)
        key = key.Substring(0, 32);
      return companySettings != null && companySettings.ContainsKey((object) key) ? companySettings[(object) key].ToString() : string.Empty;
    }

    public static string GetCompanySettingFromDB(string category, string attribute)
    {
      if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(attribute))
        return "";
      EllieMae.EMLite.DataAccess.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.DataAccess.DbQueryBuilder(ClientContext.GetCurrent().Settings.ConnectionString);
      dbQueryBuilder.Append("select * from [company_settings] where [category] = " + SQL.EncodeString(category) + " and [attribute] = " + SQL.EncodeString(attribute));
      DataRow dataRow = dbQueryBuilder.ExecuteRowQuery(DbTransactionType.None);
      string companySettingFromDb = "";
      if (dataRow != null)
        companySettingFromDb = dataRow["value"].ToString();
      return companySettingFromDb;
    }

    public static Hashtable GetCompanySettings(string category)
    {
      return Company.GetCompanySettings((IClientContext) ClientContext.GetCurrent(), category);
    }

    public static Dictionary<string, Hashtable> GetCompanySettings(string[] categories)
    {
      return Company.GetCompanySettings((IClientContext) ClientContext.GetCurrent(), categories);
    }

    public static Hashtable GetCompanySettings(IClientContext context, string category)
    {
      return string.IsNullOrWhiteSpace(category) ? new Hashtable() : context.Cache.Get<Hashtable>(Company.GetCacheKey(category), (Func<Hashtable>) (() => Company.GetCompanySettingsFromDB(context, category)), CacheSetting.Low);
    }

    public static Dictionary<string, Hashtable> GetCompanySettings(
      IClientContext context,
      string[] categories)
    {
      Dictionary<string, string> categoryKeyMap = ((IEnumerable<string>) categories).Select<string, string>((System.Func<string, string>) (category => category.Trim())).Where<string>((System.Func<string, bool>) (category => !string.IsNullOrEmpty(category))).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase).ToDictionary<string, string, string>((System.Func<string, string>) (category => category), (System.Func<string, string>) (category => Company.GetCacheKey(category)));
      Dictionary<string, string> keyCategoryMap = categoryKeyMap.ToDictionary<KeyValuePair<string, string>, string, string>((System.Func<KeyValuePair<string, string>, string>) (entry => entry.Value), (System.Func<KeyValuePair<string, string>, string>) (entry => entry.Key));
      return context.Cache.GetAll<Hashtable>(categoryKeyMap.Values.ToArray<string>(), (System.Func<string[], IDictionary<string, Hashtable>>) (cacheKeys => (IDictionary<string, Hashtable>) Company.GetCompanySettingsFromDB(context.Settings.ConnectionString, ((IEnumerable<string>) cacheKeys).Select<string, string>((System.Func<string, string>) (cacheKey => keyCategoryMap[cacheKey])).ToArray<string>()).ToDictionary<KeyValuePair<string, Hashtable>, string, Hashtable>((System.Func<KeyValuePair<string, Hashtable>, string>) (dbEntry => categoryKeyMap[dbEntry.Key]), (System.Func<KeyValuePair<string, Hashtable>, Hashtable>) (dbEntry => dbEntry.Value))), CacheSetting.Low).ToDictionary<KeyValuePair<string, Hashtable>, string, Hashtable>((System.Func<KeyValuePair<string, Hashtable>, string>) (cacheEntry => keyCategoryMap[cacheEntry.Key]), (System.Func<KeyValuePair<string, Hashtable>, Hashtable>) (cacheEntry => cacheEntry.Value), (IEqualityComparer<string>) Company.CompanySettingKeyComparer.Instance);
    }

    public static Hashtable GetCompanySettingsFromDB(IClientContext context, string category)
    {
      return Company.GetCompanySettingsFromDB(context.Settings.ConnectionString, new string[1]
      {
        category
      })[category];
    }

    public static Hashtable GetCompanySettingsFromDB(string connectionString, string category)
    {
      return Company.GetCompanySettingsFromDB(connectionString, new string[1]
      {
        category
      })[category];
    }

    private static Dictionary<string, Hashtable> GetCompanySettingsFromDB(
      string connectionString,
      string[] categories)
    {
      List<string> list = ((IEnumerable<string>) categories).Select<string, string>((System.Func<string, string>) (category => category.Trim())).Where<string>((System.Func<string, bool>) (category => !string.IsNullOrEmpty(category))).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase).ToList<string>();
      Dictionary<string, Hashtable> companySettingsFromDb = new Dictionary<string, Hashtable>((IEqualityComparer<string>) Company.CompanySettingKeyComparer.Instance);
      if (categories != null && categories.Length != 0)
      {
        string str1 = "in (" + SQL.EncodeArray((Array) list.ToArray()) + ")";
        foreach (string key in list)
          companySettingsFromDb[key] = new Hashtable((IEqualityComparer) Company.CompanySettingKeyComparer.Instance);
        EllieMae.EMLite.DataAccess.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.DataAccess.DbQueryBuilder(connectionString);
        dbQueryBuilder.Append("select * from [company_settings] where [category] " + str1);
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute(DbTransactionType.None))
        {
          string key1 = dataRow["category"].ToString().Trim();
          string key2 = dataRow["attribute"].ToString().Trim();
          string str2 = dataRow["value"].ToString();
          companySettingsFromDb[key1][(object) key2] = (object) str2;
        }
      }
      return companySettingsFromDb;
    }

    private static CompanyInfo getCompanyInfo(Hashtable clientData)
    {
      string clientID = "";
      string name = "";
      string address = "";
      string city = "";
      string state = "";
      string zip = "";
      string phone = "";
      string fax = "";
      string password = "";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      string empty7 = string.Empty;
      bool statutoryElectionInMaryland = false;
      string empty8 = string.Empty;
      bool statutoryElectionInKansas = false;
      bool useCustomLenderProfile = false;
      BranchLicensing.ATRSmallCreditors atrSmallCreditor = BranchLicensing.ATRSmallCreditors.None;
      BranchLicensing.ATRExemptCreditors atrExemptCreditor = BranchLicensing.ATRExemptCreditors.None;
      List<StateLicenseExtType> stateLicenseExtTypes = new List<StateLicenseExtType>();
      string empty9 = string.Empty;
      foreach (string key in (IEnumerable) clientData.Keys)
      {
        switch (key)
        {
          case "ATREXEMPTCREDITOR":
            atrExemptCreditor = BranchLicensing.ATRExemptCreditorToEnum(Utils.ParseInt((object) (string) clientData[(object) key]));
            continue;
          case "ATRSMALLCREDITOR":
            atrSmallCreditor = BranchLicensing.ATRSmallCreditorToEnum(Utils.ParseInt((object) (string) clientData[(object) key]));
            continue;
          case "CLIENTADDRESS":
            address = (string) clientData[(object) key];
            continue;
          case "CLIENTCITY":
            city = (string) clientData[(object) key];
            continue;
          case "CLIENTDBANAME1":
            empty1 = (string) clientData[(object) key];
            continue;
          case "CLIENTDBANAME2":
            empty2 = (string) clientData[(object) key];
            continue;
          case "CLIENTDBANAME3":
            empty3 = (string) clientData[(object) key];
            continue;
          case "CLIENTDBANAME4":
            empty4 = (string) clientData[(object) key];
            continue;
          case "CLIENTFAX":
            fax = (string) clientData[(object) key];
            continue;
          case "CLIENTHOMESTATE":
            empty6 = (string) clientData[(object) key];
            continue;
          case "CLIENTID":
            clientID = (string) clientData[(object) key];
            continue;
          case "CLIENTLENDERTYPE":
            empty5 = (string) clientData[(object) key];
            continue;
          case "CLIENTNAME":
            name = (string) clientData[(object) key];
            continue;
          case "CLIENTOPTOUT":
            empty7 = (string) clientData[(object) key];
            continue;
          case "CLIENTPASSWORD":
            password = (string) clientData[(object) key];
            continue;
          case "CLIENTPHONE":
            phone = (string) clientData[(object) key];
            continue;
          case "CLIENTSTATE":
            state = (string) clientData[(object) key];
            continue;
          case "CLIENTZIP":
            zip = (string) clientData[(object) key];
            continue;
          case "STATUTORYELECTIONINKANSAS":
            statutoryElectionInKansas = (string) clientData[(object) key] == "Y";
            continue;
          case "STATUTORYELECTIONINMARYLAND":
            statutoryElectionInMaryland = (string) clientData[(object) key] == "Y";
            continue;
          case "STATUTORYELECTIONINMARYLAND2":
            empty8 = (string) clientData[(object) key];
            continue;
          case "USECUSTOMLENDERPROFILE":
            useCustomLenderProfile = (string) clientData[(object) key] == "Y";
            continue;
          default:
            if (key.StartsWith("CLIENTLICENSING_"))
            {
              string[] strArray1 = ((string) clientData[(object) key]).Split('@');
              if (strArray1 != null && strArray1.Length != 0)
              {
                for (int index = 0; index < strArray1.Length; ++index)
                {
                  string[] strArray2 = strArray1[index].Split('|');
                  if (strArray2 != null && strArray2.Length > 10)
                    stateLicenseExtTypes.Add(new StateLicenseExtType(strArray2[0], strArray2[1], strArray2[4], strArray2[5] == "" ? DateTime.MinValue : Convert.ToDateTime(strArray2[5]), strArray2[6] == "" ? DateTime.MinValue : Convert.ToDateTime(strArray2[6]), strArray2[7] == "" ? DateTime.MinValue : Convert.ToDateTime(strArray2[7]), strArray2[8], strArray2[9] == "" ? DateTime.MinValue : Convert.ToDateTime(strArray2[9]), strArray2[2] == "Y", strArray2[3] == "Y", strArray2[10] == "" ? DateTime.MinValue : Convert.ToDateTime(strArray2[10]), strArray2[11] == "" ? 0 : Convert.ToInt32(strArray2[11])));
                  else if (strArray2 != null && strArray2.Length > 3)
                    stateLicenseExtTypes.Add(new StateLicenseExtType(strArray2[0], strArray2[1], "", DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "", DateTime.MinValue, strArray2[2] == "Y", strArray2[3] == "Y", DateTime.MinValue));
                }
                continue;
              }
              continue;
            }
            continue;
        }
      }
      BranchExtLicensing stateBranchLicensing = new BranchExtLicensing(false, 0, "", empty5, empty6, empty7, statutoryElectionInMaryland, empty8, statutoryElectionInKansas, stateLicenseExtTypes, useCustomLenderProfile, atrSmallCreditor, atrExemptCreditor);
      TraceLog.WriteVerbose(nameof (Company), "Company information read from database");
      return new CompanyInfo(clientID, name, address, city, state, zip, phone, fax, password, new string[4]
      {
        empty1,
        empty2,
        empty3,
        empty4
      }, stateBranchLicensing);
    }

    [PgReady]
    public static void appendUpdateStatement(
      IClientContext context,
      BaseDbQueryBuilder sql,
      string category,
      string attribute,
      string value)
    {
      if (string.IsNullOrEmpty(category))
        throw new ArgumentNullException(nameof (category));
      if (string.IsNullOrEmpty(attribute))
        throw new ArgumentNullException(nameof (attribute));
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add(nameof (category), (object) category.Trim());
      dbValueList.Add(nameof (attribute), (object) attribute.Trim());
      DbTableInfo table = DbAccessManager.GetTable(context, "company_settings");
      sql.DeleteFrom(table, dbValueList);
      dbValueList.Add(nameof (value), (object) value);
      sql.InsertInto(table, dbValueList);
    }

    public static void appendDeleteStatement(
      IClientContext context,
      BaseDbQueryBuilder sql,
      string category,
      string attribute)
    {
      if (string.IsNullOrEmpty(category))
        throw new ArgumentNullException(nameof (category));
      if (string.IsNullOrEmpty(attribute))
        throw new ArgumentNullException(nameof (attribute));
      DbValueList keys = new DbValueList();
      keys.Add(nameof (category), (object) category.Trim());
      keys.Add(nameof (attribute), (object) attribute.Trim());
      DbTableInfo table = DbAccessManager.GetTable(context, "company_settings");
      sql.DeleteFrom(table, keys);
    }

    public static void appendDeleteStatement(
      IClientContext context,
      BaseDbQueryBuilder sql,
      string category)
    {
      if (string.IsNullOrEmpty(category))
        throw new ArgumentNullException(nameof (category));
      DbValueList keys = new DbValueList();
      keys.Add(nameof (category), (object) category.Trim());
      DbTableInfo table = DbAccessManager.GetTable(context, "company_settings");
      sql.DeleteFrom(table, keys);
    }

    [Serializable]
    private class CompanySettingKeyComparer : IEqualityComparer<string>, IEqualityComparer
    {
      public static readonly Company.CompanySettingKeyComparer Instance = new Company.CompanySettingKeyComparer();

      public bool Equals(string x, string y)
      {
        if (x != null)
          x = x.Trim();
        if (y != null)
          y = y.Trim();
        return StringComparer.CurrentCultureIgnoreCase.Equals(x, y);
      }

      public bool Equals(object x, object y)
      {
        return x is string && y is string ? this.Equals(x as string, y as string) : object.Equals(x, y);
      }

      public int GetHashCode(string obj)
      {
        if (obj != null)
          obj = obj.Trim();
        return StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj);
      }

      public int GetHashCode(object obj)
      {
        switch (obj)
        {
          case null:
          case string _:
            return this.GetHashCode(obj as string);
          default:
            return obj.GetHashCode();
        }
      }
    }
  }
}
