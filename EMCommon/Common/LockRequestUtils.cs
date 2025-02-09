// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.LockRequestUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class LockRequestUtils
  {
    private static string[] lockRequestFormFormatFields = new string[49]
    {
      "2101",
      "2142",
      "2143",
      "2045",
      "2047",
      "2049",
      "2051",
      "2053",
      "2055",
      "2057",
      "2059",
      "2061",
      "2063",
      "2065",
      "2067",
      "2069",
      "2071",
      "2073",
      "2075",
      "2077",
      "2079",
      "2081",
      "2083",
      "2085",
      "2086",
      "2087",
      "2103",
      "2105",
      "2107",
      "2109",
      "2111",
      "2113",
      "2115",
      "2117",
      "2119",
      "2121",
      "2123",
      "2125",
      "2127",
      "2129",
      "2131",
      "2133",
      "2135",
      "2137",
      "2139",
      "2141",
      "2142",
      "2143",
      "4787"
    };
    private static string[] secondaryRegCurrentLockFormatFields = new string[9]
    {
      "2161",
      "2202",
      "2203",
      "2232",
      "2273",
      "2274",
      "2218",
      "2295",
      "2296"
    };
    private static string[] secondaryRegLockToolBuySideLockRequest = new string[24]
    {
      "2101",
      "2103",
      "2105",
      "2107",
      "2109",
      "2111",
      "2113",
      "2115",
      "2117",
      "2119",
      "2121",
      "2123",
      "2125",
      "2127",
      "2129",
      "2131",
      "2133",
      "2135",
      "2137",
      "2139",
      "2141",
      "2142",
      "2143",
      "4787"
    };
    private static string[] secondaryRegLockToolBuySidePricing = new string[81]
    {
      "2161",
      "2163",
      "2165",
      "2167",
      "2169",
      "2171",
      "2173",
      "2175",
      "2177",
      "2179",
      "2181",
      "2183",
      "2185",
      "2187",
      "2189",
      "2191",
      "2193",
      "2195",
      "2197",
      "2199",
      "2201",
      "2202",
      "2203",
      "3381",
      "3383",
      "3385",
      "3387",
      "3389",
      "3391",
      "3393",
      "3395",
      "3397",
      "3399",
      "3401",
      "3403",
      "3405",
      "3407",
      "3420",
      "3475",
      "3477",
      "3479",
      "3481",
      "3483",
      "3485",
      "3487",
      "3489",
      "3491",
      "3493",
      "4277",
      "4279",
      "4281",
      "4283",
      "4285",
      "4287",
      "4289",
      "4291",
      "4293",
      "4295",
      "4357",
      "4359",
      "4361",
      "4363",
      "4365",
      "4367",
      "4369",
      "4371",
      "4373",
      "4375",
      "3371",
      "4753",
      "4757",
      "4761",
      "4765",
      "3375",
      "4769",
      "4773",
      "4777",
      "4781",
      "2205",
      "4857",
      "4858"
    };
    private static string[] secondaryRegLockToolSellSidePricing = new string[54]
    {
      "2232",
      "2234",
      "2236",
      "2238",
      "2240",
      "2242",
      "2244",
      "2246",
      "2248",
      "2250",
      "2252",
      "2254",
      "2256",
      "2258",
      "2260",
      "2262",
      "2264",
      "2266",
      "2268",
      "2270",
      "2272",
      "2273",
      "2274",
      "2028",
      "3495",
      "3497",
      "3499",
      "3501",
      "3503",
      "3505",
      "3507",
      "3509",
      "3511",
      "3513",
      "4297",
      "4299",
      "4301",
      "4303",
      "4305",
      "4307",
      "4309",
      "4311",
      "4313",
      "4315",
      "4377",
      "4379",
      "4381",
      "4383",
      "4385",
      "4387",
      "4389",
      "4311",
      "4313",
      "4315"
    };
    private static string[] secondaryRegLockToolCompSidePricing = new string[56]
    {
      "3714",
      "3716",
      "3718",
      "3720",
      "3722",
      "3724",
      "3726",
      "3728",
      "3730",
      "3732",
      "3734",
      "3736",
      "3738",
      "3740",
      "3742",
      "3744",
      "3746",
      "3748",
      "3750",
      "3752",
      "3754",
      "3756",
      "3758",
      "3760",
      "3762",
      "3764",
      "3766",
      "3768",
      "3770",
      "3772",
      "3774",
      "4317",
      "4319",
      "4321",
      "4323",
      "4325",
      "4327",
      "4329",
      "4331",
      "4333",
      "4335",
      "4397",
      "4399",
      "4401",
      "4403",
      "4405",
      "4407",
      "4409",
      "4411",
      "4413",
      "4415",
      "3775",
      "3776",
      "3835",
      "3836",
      "3837"
    };
    private static string[] secondaryRegLockToolGainLose = new string[6]
    {
      "2203",
      "2218",
      "2274",
      "2295",
      "2296",
      "2276"
    };
    private static string[] purchaseAdviceFormFormatFields = new string[5]
    {
      "3573",
      "3575",
      "3577",
      "3578",
      "3938"
    };
    private static Dictionary<string, string> lockRequestFields = new Dictionary<string, string>();
    private static Dictionary<string, string> secondaryFields;
    private static IEnumerable<string> _rateLockFormatFields;

    static LockRequestUtils()
    {
      foreach (string requestFormFormatField in LockRequestUtils.lockRequestFormFormatFields)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(requestFormFormatField))
          LockRequestUtils.lockRequestFields.Add(requestFormFormatField, requestFormFormatField);
      }
      foreach (string currentLockFormatField in LockRequestUtils.secondaryRegCurrentLockFormatFields)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(currentLockFormatField))
          LockRequestUtils.lockRequestFields.Add(currentLockFormatField, currentLockFormatField);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolBuySideLockRequest)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(key))
          LockRequestUtils.lockRequestFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolBuySidePricing)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(key))
          LockRequestUtils.lockRequestFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolSellSidePricing)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(key))
          LockRequestUtils.lockRequestFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolGainLose)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(key))
          LockRequestUtils.lockRequestFields.Add(key, key);
      }
      foreach (string adviceFormFormatField in LockRequestUtils.purchaseAdviceFormFormatFields)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(adviceFormFormatField))
          LockRequestUtils.lockRequestFields.Add(adviceFormFormatField, adviceFormFormatField);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolCompSidePricing)
      {
        if (!LockRequestUtils.lockRequestFields.ContainsKey(key))
          LockRequestUtils.lockRequestFields.Add(key, key);
      }
      LockRequestUtils.secondaryFields = new Dictionary<string, string>();
      foreach (string key in LockRequestUtils.secondaryRegLockToolBuySideLockRequest)
      {
        if (!LockRequestUtils.secondaryFields.ContainsKey(key))
          LockRequestUtils.secondaryFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolBuySidePricing)
      {
        if (!LockRequestUtils.secondaryFields.ContainsKey(key))
          LockRequestUtils.secondaryFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolSellSidePricing)
      {
        if (!LockRequestUtils.secondaryFields.ContainsKey(key))
          LockRequestUtils.secondaryFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolCompSidePricing)
      {
        if (!LockRequestUtils.secondaryFields.ContainsKey(key))
          LockRequestUtils.secondaryFields.Add(key, key);
      }
      foreach (string key in LockRequestUtils.secondaryRegLockToolGainLose)
      {
        if (!LockRequestUtils.secondaryFields.ContainsKey(key))
          LockRequestUtils.secondaryFields.Add(key, key);
      }
      LockRequestUtils._rateLockFormatFields = ((IEnumerable<string>) LockRequestUtils.lockRequestFormFormatFields).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegCurrentLockFormatFields).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegLockToolBuySideLockRequest).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegLockToolBuySidePricing).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegLockToolSellSidePricing).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegLockToolCompSidePricing).Union<string>((IEnumerable<string>) LockRequestUtils.secondaryRegLockToolGainLose).Union<string>((IEnumerable<string>) LockRequestUtils.purchaseAdviceFormFormatFields);
    }

    public static IEnumerable<string> RateLockFormatFields
    {
      get => LockRequestUtils._rateLockFormatFields;
    }

    public static bool IsLockRequestSecondary10DigitFormatingFields(string fieldID)
    {
      return LockRequestUtils.lockRequestFields.ContainsKey(fieldID);
    }

    public static string[] LockRequestSecondary10DigitFormatingFields()
    {
      string[] array = new string[LockRequestUtils.lockRequestFields.Count];
      LockRequestUtils.lockRequestFields.Values.CopyTo(array, 0);
      return array;
    }

    public static string[] LockRequestAreaFields => LockRequestUtils.lockRequestFormFormatFields;

    public static bool IsLockRequestAreaFields(string fieldId)
    {
      return Array.IndexOf<string>(LockRequestUtils.LockRequestAreaFields, fieldId) > -1;
    }

    public static bool IsSecondaryRegistrationAreaFields(string fieldID)
    {
      return LockRequestUtils.secondaryFields.ContainsKey(fieldID);
    }

    public static string[] SecondaryRegistrationAreaFields()
    {
      string[] array = new string[LockRequestUtils.secondaryFields.Count];
      LockRequestUtils.secondaryFields.Values.CopyTo(array, 0);
      return array;
    }

    public static List<string> TradeAreaFieldDescriptions()
    {
      return new List<string>()
      {
        "Price values in the Base Price table",
        "Buy Up value in the Buy UP/Down section",
        "Buy Down value in the Buy Up/Down section",
        "Price Adjustment in the Price Adjustments section"
      };
    }
  }
}
