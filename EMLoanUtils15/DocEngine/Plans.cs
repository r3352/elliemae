// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.Plans
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public static class Plans
  {
    private static Dictionary<string, List<PlanCodeInfo>> _plansCache = new Dictionary<string, List<PlanCodeInfo>>();
    private static DateTime _plansLastLoadedUtc = DateTime.MinValue;
    private static Dictionary<string, string> _customPlansCache = new Dictionary<string, string>();
    private static DateTime _customPlansLastLoadedUtc = DateTime.MinValue;
    private static object _lockObject = new object();
    public const string EDS_SETTINGS_SECTION = "EDS�";
    public const string EDS_SETTINGS_KEY_COMPANYPLANSLASTMOD = "CompanyPlansLastModUTC�";
    public const string EDS_SETTINGS_KEY_CUSTOMPLANSLASTMOD = "CustomPlansLastModUTC�";

    public static DateTime GetOptionalCompanySetting(
      SessionObjects sessionObjects,
      string section,
      string key,
      DateTime defaultValue)
    {
      DateTime result = DateTime.MinValue;
      if (!DateTime.TryParse(sessionObjects.ConfigurationManager.GetCompanySetting(section, key), out result))
        result = defaultValue;
      return result;
    }

    public static Plan[] GetCompanyPlans(SessionObjects sessionObjects, DocumentOrderType orderType)
    {
      return Plans.GetCompanyPlans(sessionObjects, orderType, false);
    }

    public static Plan[] GetCompanyPlans(
      SessionObjects sessionObjects,
      DocumentOrderType orderType,
      bool activeOnly)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 70, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      List<Plan> planList = new List<Plan>();
      DateTime optionalCompanySetting1 = Plans.GetOptionalCompanySetting(sessionObjects, "EDS", "CompanyPlansLastModUTC", DateTime.MinValue);
      DateTime optionalCompanySetting2 = Plans.GetOptionalCompanySetting(sessionObjects, "EDS", "CustomPlansLastModUTC", DateTime.MinValue);
      DateTime dateTime = optionalCompanySetting1 > optionalCompanySetting2 ? optionalCompanySetting1 : optionalCompanySetting2;
      bool flag1 = true;
      string key1 = orderType.ToString() + activeOnly.ToString();
      lock (Plans._lockObject)
      {
        if (dateTime > DateTime.MinValue)
        {
          if (Plans._plansLastLoadedUtc > dateTime)
          {
            if (Plans._plansCache != null)
            {
              if (Plans._plansCache.ContainsKey(key1))
              {
                if (Plans._plansCache[key1] != null)
                {
                  if (Plans._plansCache[key1].Count > 0)
                    flag1 = false;
                }
              }
            }
          }
        }
      }
      PlanCodeInfo[] collection1 = (PlanCodeInfo[]) null;
      if (flag1)
      {
        collection1 = sessionObjects.ConfigurationManager.GetCompanyPlanCodes(orderType, activeOnly);
        lock (Plans._lockObject)
        {
          Plans._plansLastLoadedUtc = DateTime.UtcNow;
          if (Plans._plansCache == null)
            Plans._plansCache = new Dictionary<string, List<PlanCodeInfo>>();
          List<PlanCodeInfo> planCodeInfoList = new List<PlanCodeInfo>();
          planCodeInfoList.AddRange((IEnumerable<PlanCodeInfo>) collection1);
          Plans._plansCache[key1] = planCodeInfoList;
        }
        PerformanceMeter.Current.AddCheckpoint("Company plans loaded from Enc DB, Cache refreshed", 104, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      }
      else
      {
        lock (Plans._lockObject)
          collection1 = Plans._plansCache[key1].ToArray();
        PerformanceMeter.Current.AddCheckpoint("Company plans unchanged, returned from Cache", 112, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      }
      List<string> stringList = new List<string>();
      foreach (PlanCodeInfo planCodeInfo in collection1)
      {
        if (planCodeInfo.PlanID != "")
          stringList.Add(planCodeInfo.PlanID);
      }
      StandardPlan[] standardPlanArray = (StandardPlan[]) null;
      if (stringList.Count > 0)
      {
        using (DocEngineService docEngineService = new DocEngineService(sessionObjects))
        {
          stringList.Sort();
          standardPlanArray = docEngineService.GetPlans(stringList.ToArray());
        }
      }
      Dictionary<string, StandardPlan> dictionary = new Dictionary<string, StandardPlan>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      if (standardPlanArray != null)
      {
        foreach (StandardPlan standardPlan in standardPlanArray)
          dictionary[standardPlan.PlanID] = standardPlan;
      }
      bool flag2 = true;
      string key2 = orderType.ToString();
      lock (Plans._lockObject)
      {
        if (dateTime > DateTime.MinValue)
        {
          if (Plans._customPlansLastLoadedUtc > dateTime)
          {
            if (Plans._customPlansCache != null)
            {
              if (Plans._customPlansCache.ContainsKey(key2))
              {
                if (Plans._customPlansCache[key2] != null)
                  flag2 = false;
              }
            }
          }
        }
      }
      XmlSerializer xmlSerializer = new XmlSerializer();
      List<CustomPlanCode> customPlanCodeList = (List<CustomPlanCode>) null;
      if (flag2)
      {
        customPlanCodeList = sessionObjects.ConfigurationManager.GetCompanyCustomPlanCodes(orderType);
        string str = xmlSerializer.Serialize((object) customPlanCodeList.ToArray());
        lock (Plans._lockObject)
        {
          if (Plans._customPlansCache == null)
            Plans._customPlansCache = new Dictionary<string, string>();
          Plans._customPlansCache[key2] = str;
          Plans._customPlansLastLoadedUtc = DateTime.UtcNow;
        }
        PerformanceMeter.Current.AddCheckpoint("Custom plans loaded from Enc DB, Cache refreshed", 175, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      }
      else
      {
        CustomPlanCode[] customPlanCodeArray = new CustomPlanCode[1];
        lock (Plans._lockObject)
        {
          CustomPlanCode[] collection2 = (CustomPlanCode[]) xmlSerializer.Deserialize(Plans._customPlansCache[key2], customPlanCodeArray.GetType());
          customPlanCodeList = new List<CustomPlanCode>();
          customPlanCodeList.AddRange((IEnumerable<CustomPlanCode>) collection2);
        }
        PerformanceMeter.Current.AddCheckpoint("Custom plans unchanged, returned from Cache", 186, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      }
      foreach (PlanCodeInfo planCodeInfo in collection1)
      {
        if (!planCodeInfo.IsCustom && dictionary.ContainsKey(planCodeInfo.PlanID))
          planList.Add((Plan) dictionary[planCodeInfo.PlanID]);
      }
      foreach (CustomPlanCode customCode in customPlanCodeList)
      {
        if (customCode.IsActive || !activeOnly)
        {
          if (customCode.IsEMAlias)
          {
            if (dictionary.ContainsKey(customCode.PlanCodeID))
              planList.Add((Plan) new AliasedPlan(dictionary[customCode.PlanCodeID], customCode));
          }
          else
            planList.Add((Plan) new CustomPlan(customCode));
        }
      }
      PerformanceMeter.Current.AddCheckpoint("END", 213, nameof (GetCompanyPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\Plans.cs");
      return planList.ToArray();
    }

    public static Plan GetPlan(SessionObjects sessionObjects, PlanCodeInfo planInfo)
    {
      CustomPlanCode customCode = (CustomPlanCode) null;
      if (planInfo.IsCustom)
      {
        customCode = sessionObjects.ConfigurationManager.GetCompanyCustomPlanCode(planInfo.OrderType, planInfo.PlanCode);
        if (customCode == null)
          return (Plan) null;
        if (!customCode.IsEMAlias)
          return (Plan) new CustomPlan(customCode);
      }
      if (string.IsNullOrEmpty(planInfo.PlanID))
        return (Plan) null;
      StandardPlan basePlan = (StandardPlan) null;
      using (DocEngineService docEngineService = new DocEngineService(sessionObjects))
      {
        StandardPlan[] plans = docEngineService.GetPlans(new string[1]
        {
          planInfo.PlanID
        });
        if (plans.Length == 0)
          return (Plan) null;
        basePlan = plans[0];
      }
      if (customCode == null)
        return (Plan) basePlan;
      return (Plan) new AliasedPlan(basePlan, customCode);
    }
  }
}
