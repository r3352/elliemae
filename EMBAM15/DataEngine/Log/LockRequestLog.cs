// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockRequestLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LockRequestLog : LogRecordBase
  {
    public static readonly string XmlType = "LockRequest";
    public static List<string> SnapshotFields = new List<string>();
    public static List<string> CurrentLockFields = new List<string>();
    public static List<string> LoanInfoSnapshotFields;
    public static List<string> BuySideFields = new List<string>();
    public static List<string> SellSideFields = new List<string>();
    public static List<string> RequestFields = new List<string>();
    public static List<string> LockExtensionFields = new List<string>();
    public static List<string> CompSideFields = new List<string>();
    public static List<KeyValuePair<string, string>> RequestFieldMap;
    private string requestedBy = string.Empty;
    private string requestedFullName = string.Empty;
    private string requestedStatus = string.Empty;
    private string requestedOldStatus = string.Empty;
    private DateTime buySideExpirationDate = DateTime.MinValue;
    private DateTime sellSideExpirationDate = DateTime.MinValue;
    private DateTime sellSideDeliveryDate = DateTime.MinValue;
    private bool alertLoanOfficer;
    private bool hideLog;
    private bool isFakeRequest;
    private bool isLockExtension;
    private bool isLockCancellation;
    private bool isRelock;
    private int numDayLocked;
    private string sellSideDeliveredBy = string.Empty;
    private string parentLockGuid = string.Empty;
    private bool isVoided;
    private int buySideNumDayExtended;
    private DateTime buySideNewLockExtensionDate = DateTime.MinValue;
    private int buySideNumDayLocked;
    private int cumulatedDaystoExtend;
    private string investorName = string.Empty;
    private int sellSideNumDayExtended;
    private DateTime sellSideNewLockExtensionDate = DateTime.MinValue;
    private Hashtable lockRequestSnapshot;
    public string lockRequestSnapshotString;
    private int reLockSequenceNumberForInactiveLock;
    private string rateLockAction = string.Empty;
    private string reviseAction = string.Empty;
    private string concessionIndicator = string.Empty;
    private string extensionIndicator = string.Empty;
    private string concessionRequestStatus = string.Empty;

    static LockRequestLog()
    {
      for (int index = 2034; index <= 2087; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      for (int index = 2516; index <= 2549; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      for (int index = 2690; index <= 2732; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      for (int index = 3434; index <= 3453; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      for (int index = 4416; index <= 4435; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      for (int index = 4436; index <= 4455; ++index)
        LockRequestLog.CurrentLockFields.Add(index.ToString());
      LockRequestLog.CurrentLockFields.AddRange((IEnumerable<string>) new string[4]
      {
        "2145",
        "2146",
        "2147",
        "2592"
      });
      LockRequestLog.CurrentLockFields.Add("3255");
      LockRequestLog.CurrentLockFields.Add("4787");
      LockRequestLog.SnapshotFields.AddRange((IEnumerable<string>) new string[63]
      {
        "1401",
        "2861",
        "136",
        "MORNET.X67",
        "67",
        "1450",
        "1414",
        "60",
        "1452",
        "5029",
        "1415",
        "1484",
        "VASUMM.X23",
        "2853",
        "1502",
        "11",
        "12",
        "14",
        "15",
        "13",
        "1821",
        "356",
        "1041",
        "1811",
        "19",
        "1172",
        "608",
        "420",
        "4",
        "325",
        "675",
        "2",
        "140",
        "142",
        "763",
        "353",
        "976",
        "740",
        "742",
        "2293",
        "2294",
        "2216",
        "2217",
        "1130",
        "FE0115",
        "FE0215",
        "934",
        "16",
        "3515",
        "4115",
        "4116",
        "4254",
        "Constr.Refi",
        "5015",
        "1825",
        "URLA.X74",
        "URLA.X75",
        "URLA.X76",
        "URLA.X73",
        "URLA.X205",
        "URLA.X206",
        "URLA.X207",
        "URLA.X208"
      });
      LockRequestLog.LoanInfoSnapshotFields = new List<string>((IEnumerable<string>) LockRequestLog.SnapshotFields.ToArray());
      LockRequestLog.SnapshotFields.Add("2848");
      for (int index = 2866; index <= 2967; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
      }
      for (int index = 3035; index <= 3038; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
      }
      for (int index = 3043; index <= 3047; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
      }
      for (int index = 3516; index <= 3527; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
      }
      LockRequestLog.RequestFields.Add("3049");
      LockRequestLog.LoanInfoSnapshotFields.Add("3049");
      LockRequestLog.SnapshotFields.Add("3049");
      LockRequestLog.RequestFields.Add("3041");
      LockRequestLog.LoanInfoSnapshotFields.Add("3041");
      LockRequestLog.SnapshotFields.Add("3041");
      LockRequestLog.RequestFields.Add("3056");
      LockRequestLog.LoanInfoSnapshotFields.Add("3056");
      LockRequestLog.SnapshotFields.Add("3056");
      LockRequestLog.RequestFields.AddRange((IEnumerable<string>) new string[2]
      {
        "3241",
        "3242"
      });
      LockRequestLog.LoanInfoSnapshotFields.AddRange((IEnumerable<string>) new string[2]
      {
        "3241",
        "3242"
      });
      LockRequestLog.SnapshotFields.AddRange((IEnumerable<string>) new string[2]
      {
        "3241",
        "3242"
      });
      string[] collection = new string[21]
      {
        "3528",
        "3529",
        "3530",
        "3628",
        "3360",
        "3361",
        "3362",
        "3369",
        "3847",
        "3845",
        "3846",
        "3844",
        "3872",
        "3874",
        "3892",
        "4459",
        "4463",
        "4515",
        "4516",
        "4517",
        "4518"
      };
      LockRequestLog.RequestFields.AddRange((IEnumerable<string>) collection);
      LockRequestLog.LoanInfoSnapshotFields.AddRange((IEnumerable<string>) collection);
      LockRequestLog.SnapshotFields.AddRange((IEnumerable<string>) collection);
      LockRequestLog.RequestFields.Add("4060");
      LockRequestLog.RequestFields.Add("4061");
      LockRequestLog.RequestFields.Add("4069");
      LockRequestLog.RequestFields.Add("4115");
      LockRequestLog.RequestFields.Add("4116");
      LockRequestLog.RequestFields.Add("1964");
      LockRequestLog.RequestFields.Add("4255");
      LockRequestLog.RequestFields.Add("5038");
      LockRequestLog.RequestFields.Add("4201");
      LockRequestLog.SnapshotFields.Add("4201");
      LockRequestLog.RequestFields.Add("4209");
      LockRequestLog.SnapshotFields.Add("4209");
      LockRequestLog.SnapshotFields.Add("3433");
      LockRequestLog.LockExtensionFields.Add("3433");
      for (int index = 2088; index <= 2144; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      for (int index = 2414; index <= 2447; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      for (int index = 2647; index <= 2689; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      LockRequestLog.SnapshotFields.Add("3841");
      for (int index = 3454; index <= 3473; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      for (int index = 4256; index <= 4275; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      for (int index = 4336; index <= 4355; ++index)
        LockRequestLog.SnapshotFields.Add(index.ToString());
      LockRequestLog.SnapshotFields.Add("3254");
      LockRequestLog.SnapshotFields.Add("4187");
      LockRequestLog.SnapshotFields.Add("3965");
      LockRequestLog.SnapshotFields.Add("4060");
      LockRequestLog.SnapshotFields.Add("4061");
      LockRequestLog.SnapshotFields.Add("4069");
      LockRequestLog.SnapshotFields.Add("4514");
      LockRequestLog.RequestFields.Add("4514");
      LockRequestLog.LoanInfoSnapshotFields.Add("4514");
      LockRequestLog.SnapshotFields.Add("4510");
      LockRequestLog.RequestFields.Add("4510");
      LockRequestLog.LoanInfoSnapshotFields.Add("4510");
      LockRequestLog.SnapshotFields.Add("4519");
      LockRequestLog.RequestFields.Add("4519");
      LockRequestLog.LoanInfoSnapshotFields.Add("4519");
      LockRequestLog.SnapshotFields.Add("4511");
      LockRequestLog.RequestFields.Add("4511");
      LockRequestLog.LoanInfoSnapshotFields.Add("4511");
      LockRequestLog.SnapshotFields.Add("4513");
      LockRequestLog.RequestFields.Add("4513");
      LockRequestLog.LoanInfoSnapshotFields.Add("4513");
      LockRequestLog.SnapshotFields.Add("4512");
      LockRequestLog.RequestFields.Add("4512");
      LockRequestLog.LoanInfoSnapshotFields.Add("4512");
      LockRequestLog.SnapshotFields.Add("4711");
      LockRequestLog.RequestFields.Add("4711");
      LockRequestLog.LoanInfoSnapshotFields.Add("4711");
      LockRequestLog.SnapshotFields.Add("4712");
      LockRequestLog.RequestFields.Add("4712");
      LockRequestLog.LoanInfoSnapshotFields.Add("4712");
      LockRequestLog.SnapshotFields.Add("4713");
      LockRequestLog.RequestFields.Add("4713");
      LockRequestLog.LoanInfoSnapshotFields.Add("4713");
      LockRequestLog.SnapshotFields.Add("4714");
      LockRequestLog.RequestFields.Add("4714");
      LockRequestLog.LoanInfoSnapshotFields.Add("4714");
      for (int index = 4535; index <= 4546; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
      }
      for (int index = 1269; index <= 1274; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
      }
      for (int index = 1613; index <= 1618; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
      }
      for (int index = 4631; index <= 4645; ++index)
      {
        LockRequestLog.RequestFields.Add(index.ToString());
        LockRequestLog.SnapshotFields.Add(index.ToString());
        LockRequestLog.LoanInfoSnapshotFields.Add(index.ToString());
      }
      LockRequestLog.SnapshotFields.Add("CASASRN.X141");
      LockRequestLog.RequestFields.Add("CASASRN.X141");
      LockRequestLog.LoanInfoSnapshotFields.Add("CASASRN.X141");
      LockRequestLog.RequestFields.Add("4787");
      LockRequestLog.SnapshotFields.Add("4787");
      LockRequestLog.LoanInfoSnapshotFields.Add("4787");
      LockRequestLog.SnapshotFields.Add("4789");
      LockRequestLog.RequestFields.Add("4789");
      LockRequestLog.LoanInfoSnapshotFields.Add("4789");
      LockRequestLog.SnapshotFields.Add("4790");
      LockRequestLog.RequestFields.Add("4790");
      LockRequestLog.LoanInfoSnapshotFields.Add("4790");
      LockRequestLog.SnapshotFields.Add("4791");
      LockRequestLog.RequestFields.Add("4791");
      LockRequestLog.LoanInfoSnapshotFields.Add("4791");
      for (int index = 2148; index <= 2205; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 2448; index <= 2481; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 2733; index <= 2775; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 3474; index <= 3493; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 4276; index <= 4295; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 4356; index <= 4375; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 3380; index <= 3420; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      for (int index = 3371; index <= 3379; ++index)
        LockRequestLog.BuySideFields.Add(index.ToString());
      LockRequestLog.BuySideFields.AddRange((IEnumerable<string>) new string[8]
      {
        "2215",
        "2218",
        "2029",
        "3256",
        "3364",
        "3848",
        "3873",
        "3875"
      });
      LockRequestLog.BuySideFields.AddRange((IEnumerable<string>) new string[10]
      {
        "3902",
        "3903",
        "3904",
        "3905",
        "3906",
        "3909",
        "3910",
        "3911",
        "3912",
        "3913"
      });
      LockRequestLog.BuySideFields.Add("4058");
      LockRequestLog.BuySideFields.Add("4059");
      LockRequestLog.BuySideFields.Add("4070");
      LockRequestLog.BuySideFields.Add("4751");
      LockRequestLog.BuySideFields.AddRange((IEnumerable<string>) new string[42]
      {
        "3371",
        "3372",
        "3373",
        "3374",
        "3375",
        "3376",
        "3377",
        "3378",
        "4753",
        "4754",
        "4755",
        "4756",
        "4757",
        "4758",
        "4759",
        "4760",
        "4761",
        "4762",
        "4763",
        "4764",
        "4765",
        "4766",
        "4767",
        "4768",
        "4769",
        "4770",
        "4771",
        "4772",
        "4773",
        "4774",
        "4775",
        "4776",
        "4777",
        "4778",
        "4779",
        "4780",
        "4781",
        "4782",
        "4783",
        "4784",
        "4857",
        "4858"
      });
      for (int index = 2219; index <= 2292; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 2482; index <= 2515; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 2295; index <= 2297; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 2776; index <= 2818; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 3494; index <= 3513; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 4296; index <= 4315; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      for (int index = 4376; index <= 4395; ++index)
        LockRequestLog.SellSideFields.Add(index.ToString());
      LockRequestLog.SellSideFields.AddRange((IEnumerable<string>) new string[6]
      {
        "2028",
        "2030",
        "2206",
        "2841",
        "2842",
        "3257"
      });
      LockRequestLog.SellSideFields.Add("3055");
      LockRequestLog.SellSideFields.Add("3123");
      LockRequestLog.SellSideFields.Add("3263");
      LockRequestLog.SellSideFields.AddRange((IEnumerable<string>) new string[6]
      {
        "3534",
        "3535",
        "3888",
        "3889",
        "3890",
        "4019"
      });
      LockRequestLog.SellSideFields.Add("4016");
      LockRequestLog.SellSideFields.Add("4093");
      LockRequestLog.SellSideFields.Add("4094");
      LockRequestLog.SellSideFields.Add("4118");
      LockRequestLog.SellSideFields.Add("4182");
      foreach (string buySideField in LockRequestLog.BuySideFields)
        LockRequestLog.SnapshotFields.Add(buySideField);
      foreach (string sellSideField in LockRequestLog.SellSideFields)
        LockRequestLog.SnapshotFields.Add(sellSideField);
      LockRequestLog.SnapshotFields.AddRange((IEnumerable<string>) new string[6]
      {
        "2216",
        "2217",
        "2293",
        "2294",
        "2592",
        "3431"
      });
      for (int index = 3358; index <= 3369; ++index)
        LockRequestLog.LockExtensionFields.Add(index.ToString());
      foreach (string lockExtensionField in LockRequestLog.LockExtensionFields)
        LockRequestLog.SnapshotFields.Add(lockExtensionField);
      for (int index = 3660; index <= 3837; ++index)
      {
        if (index != 3667 && index != 3668 && index != 3669 && index != 3670)
          LockRequestLog.CompSideFields.Add(index.ToString());
      }
      for (int index = 4316; index <= 4335; ++index)
        LockRequestLog.CompSideFields.Add(index.ToString());
      for (int index = 4396; index <= 4415; ++index)
        LockRequestLog.CompSideFields.Add(index.ToString());
      foreach (string compSideField in LockRequestLog.CompSideFields)
        LockRequestLog.SnapshotFields.Add(compSideField);
      LockRequestLog.RequestFieldMap = new List<KeyValuePair<string, string>>();
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2866", "1401"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2867", "MORNET.X67"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2940", "1484"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2941", "1502"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2853", "VASUMM.X23"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2942", "11"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2943", "12"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2945", "14"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2946", "15"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2944", "13"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2947", "1041"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2948", "1821"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2949", "356"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2950", "1811"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2951", "19"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2952", "1172"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2953", "608"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2954", "1267"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2955", "1266"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2956", "995"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2957", "994"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2958", "420"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2959", "4"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2960", "325"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2961", "2293"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2962", "2294"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2963", "675"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2964", "2217"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2965", "2"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2966", "763"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2967", "2861"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3035", "427"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3036", "428"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3038", "136"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3043", "1109"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3044", "1107"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3045", "1826"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3046", "1765"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3049", "562"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3056", "SYS.X11"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3047", "1760"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3241", "353"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3242", "976"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3041", "1130"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3528", "934"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3529", "16"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3530", "3515"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3535", "VEND.X178"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3628", "3533"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("2092", "3"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3844", "3000"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3845", "MAX23K.X43"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3846", "CASASRN.X167"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("3892", "3891"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4115", "3315"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4116", "3316"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4254", "1964"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4255", "Constr.Refi"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4463", "LCP.X1"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4514", "1540"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4510", "1888"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4519", "CASASRN.X168"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4511", "1482"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4513", "688"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4512", "1959"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4515", "URLA.X76"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4516", "URLA.X73"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4517", "URLA.X74"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4518", "URLA.X75"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4631", "CASASRN.X141"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4632", "4645"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4711", "URLA.X205"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4712", "URLA.X206"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4713", "URLA.X207"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("4714", "URLA.X208"));
      LockRequestLog.RequestFieldMap.Add(new KeyValuePair<string, string>("5038", "5015"));
    }

    public LockRequestLog(LogList log)
      : base(log)
    {
    }

    public LockRequestLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      string s = attributeReader.GetString("Date");
      DateTime result1;
      if (s.EndsWith("Z") && DateTime.TryParse(s, out result1))
        this.date = System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(result1, "Pacific Standard Time");
      string strB = attributeReader.GetString(nameof (TimeRequested));
      DateTime result2;
      if (string.Compare(this.TimeRequested, strB, true) != 0 && DateTime.TryParse(this.Date.ToString("MM/dd/yyyy") + " " + strB, out result2))
        this.date = result2;
      this.requestedBy = attributeReader.GetString(nameof (RequestedBy));
      this.requestedFullName = attributeReader.GetString("RequestedName");
      this.requestedStatus = attributeReader.GetString(nameof (RequestedStatus));
      this.requestedOldStatus = attributeReader.GetString(nameof (RequestedOldStatus));
      this.buySideExpirationDate = attributeReader.GetDate(nameof (BuySideExpirationDate));
      this.sellSideExpirationDate = attributeReader.GetDate(nameof (SellSideExpirationDate));
      this.sellSideDeliveryDate = attributeReader.GetDate(nameof (SellSideDeliveryDate));
      this.hideLog = attributeReader.GetBoolean("HideLog");
      this.isFakeRequest = attributeReader.GetBoolean(nameof (IsFakeRequest));
      this.isLockExtension = attributeReader.GetBoolean(nameof (IsLockExtension), false);
      this.isLockCancellation = attributeReader.GetBoolean(nameof (IsLockCancellation), false);
      this.isRelock = attributeReader.GetBoolean(nameof (IsRelock), false);
      this.numDayLocked = attributeReader.GetInteger("NumDayLocked", 0);
      this.sellSideDeliveredBy = attributeReader.GetString(nameof (SellSideDeliveredBy));
      this.buySideNumDayExtended = attributeReader.GetInteger(nameof (BuySideNumDayExtended), 0);
      this.buySideNewLockExtensionDate = attributeReader.GetDate(nameof (BuySideNewLockExtensionDate));
      this.buySideNumDayLocked = attributeReader.GetInteger(nameof (BuySideNumDayLocked), 0);
      this.cumulatedDaystoExtend = attributeReader.GetInteger(nameof (CumulatedDaystoExtend), 0);
      this.investorName = attributeReader.GetString(nameof (InvestorName));
      this.sellSideNumDayExtended = attributeReader.GetInteger(nameof (SellSideNumDayExtended), 0);
      this.sellSideNewLockExtensionDate = attributeReader.GetDate(nameof (SellSideNewLockExtensionDate));
      this.parentLockGuid = attributeReader.GetString(nameof (ParentLockGuid));
      this.reLockSequenceNumberForInactiveLock = attributeReader.GetInteger(nameof (ReLockSequenceNumberForInactiveLock));
      this.rateLockAction = attributeReader.GetString(nameof (RateLockAction));
      this.reviseAction = attributeReader.GetString(nameof (ReviseAction));
      this.isVoided = attributeReader.GetBoolean(nameof (Voided));
      this.concessionIndicator = attributeReader.GetString(nameof (PriceConcessionIndicator));
      this.extensionIndicator = attributeReader.GetString(nameof (LockExtensionIndicator));
      this.concessionRequestStatus = attributeReader.GetString(nameof (PriceConcessionRequestStatus));
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockRequestLog.XmlType);
      attributeWriter.Write("TimeRequested", (object) this.TimeRequested);
      attributeWriter.Write("RequestedBy", (object) this.requestedBy);
      attributeWriter.Write("RequestedName", (object) this.requestedFullName);
      attributeWriter.Write("RequestedStatus", (object) this.requestedStatus);
      attributeWriter.Write("RequestedOldStatus", (object) this.requestedOldStatus);
      attributeWriter.Write("BuySideExpirationDate", (object) this.buySideExpirationDate);
      attributeWriter.Write("SellSideExpirationDate", (object) this.sellSideExpirationDate);
      attributeWriter.Write("SellSideDeliveryDate", (object) this.sellSideDeliveryDate);
      attributeWriter.Write("HideLog", (object) this.hideLog);
      attributeWriter.Write("IsFakeRequest", (object) this.isFakeRequest);
      attributeWriter.Write("IsLockExtension", (object) this.isLockExtension);
      attributeWriter.Write("IsLockCancellation", (object) this.isLockCancellation);
      attributeWriter.Write("IsRelock", (object) this.isRelock);
      attributeWriter.Write("NumDayLocked", (object) this.numDayLocked);
      attributeWriter.Write("SellSideDeliveredBy", (object) this.sellSideDeliveredBy);
      attributeWriter.Write("BuySideNumDayExtended", (object) this.buySideNumDayExtended);
      attributeWriter.Write("BuySideNewLockExtensionDate", (object) this.buySideNewLockExtensionDate);
      attributeWriter.Write("BuySideNumDayLocked", (object) this.buySideNumDayLocked);
      attributeWriter.Write("CumulatedDaystoExtend", (object) this.cumulatedDaystoExtend);
      attributeWriter.Write("InvestorName", (object) this.investorName);
      attributeWriter.Write("SellSideNumDayExtended", (object) this.sellSideNumDayExtended);
      attributeWriter.Write("SellSideNewLockExtensionDate", (object) this.sellSideNewLockExtensionDate);
      attributeWriter.Write("ParentLockGuid", (object) this.parentLockGuid);
      attributeWriter.Write("ReLockSequenceNumberForInactiveLock", (object) this.reLockSequenceNumberForInactiveLock);
      attributeWriter.Write("RateLockAction", (object) this.rateLockAction);
      attributeWriter.Write("ReviseAction", (object) this.reviseAction);
      attributeWriter.Write("Voided", (object) this.isVoided);
      attributeWriter.Write("PriceConcessionIndicator", (object) this.concessionIndicator);
      attributeWriter.Write("LockExtensionIndicator", (object) this.extensionIndicator);
      attributeWriter.Write("PriceConcessionRequestStatus", (object) this.concessionRequestStatus);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      if (log.Loan.Calculator != null)
        log.Loan.Calculator.CalculateInvestorStatus();
      this.MarkAsDirty("LOCKRATE.REQUESTSTATUS");
      this.MarkAsDirty("LOCKRATE.LASTACTIONTIME");
      this.MarkAsDirty("LOCKRATE.REQUESTCOUNT");
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (this.LockRequestStatus != RateLockRequestStatus.Requested)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        !this.isLockCancellation ? new PipelineInfo.Alert(18, "Lock requested by " + this.requestedFullName, "", this.Date, "", this.Guid) : new PipelineInfo.Alert(33, "Lock cancellation requested by " + this.requestedFullName, "", this.Date, "", this.Guid)
      };
    }

    public string TimeRequested => this.Date.ToString("h:mm:ss tt");

    public DateTime DateTimeRequested => this.Date;

    public bool Voided
    {
      set
      {
        this.isVoided = value;
        this.MarkAsDirty();
      }
      get => this.isVoided;
    }

    public bool IsVoidable { get; set; }

    public bool IsCancellable { get; set; }

    public bool IsExtendable { get; set; }

    public void SetRequestingUser(string userId, string fullName)
    {
      this.requestedBy = userId;
      this.requestedFullName = fullName;
      this.MarkAsDirty();
    }

    public void Copy(ref LockRequestLog lockRequestLogToCopy)
    {
      lockRequestLogToCopy.BuySideExpirationDate = this.BuySideExpirationDate;
      lockRequestLogToCopy.BuySideNewLockExtensionDate = this.BuySideNewLockExtensionDate;
      lockRequestLogToCopy.BuySideNumDayExtended = this.BuySideNumDayExtended;
      lockRequestLogToCopy.BuySideNumDayLocked = this.BuySideNumDayLocked;
      lockRequestLogToCopy.Comments = this.Comments;
      lockRequestLogToCopy.CumulatedDaystoExtend = this.CumulatedDaystoExtend;
      lockRequestLogToCopy.DisplayInLog = this.DisplayInLog;
      lockRequestLogToCopy.InvestorName = this.InvestorName;
      lockRequestLogToCopy.IsFakeRequest = this.IsFakeRequest;
      lockRequestLogToCopy.IsRelock = this.IsRelock;
      lockRequestLogToCopy.IsLockCancellation = this.IsLockCancellation;
      lockRequestLogToCopy.IsLockExtension = this.IsLockExtension;
      lockRequestLogToCopy.LockRequestSnapshotString = this.LockRequestSnapshotString;
      lockRequestLogToCopy.LockRequestStatus = this.LockRequestStatus;
      lockRequestLogToCopy.NumOfDaysLocked = this.NumOfDaysLocked;
      lockRequestLogToCopy.ParentLockGuid = this.ParentLockGuid;
      lockRequestLogToCopy.RateLockAction = this.RateLockAction;
      lockRequestLogToCopy.ReviseAction = this.ReviseAction;
      lockRequestLogToCopy.ReLockSequenceNumberForInactiveLock = this.ReLockSequenceNumberForInactiveLock;
      lockRequestLogToCopy.RequestedStatus = this.RequestedStatus;
      lockRequestLogToCopy.RequestedOldStatus = this.RequestedOldStatus;
      lockRequestLogToCopy.SellSideDeliveredBy = this.SellSideDeliveredBy;
      lockRequestLogToCopy.SellSideDeliveryDate = this.SellSideDeliveryDate;
      lockRequestLogToCopy.SellSideExpirationDate = this.SellSideExpirationDate;
      lockRequestLogToCopy.SellSideNewLockExtensionDate = this.SellSideNewLockExtensionDate;
      lockRequestLogToCopy.SellSideNumDayExtended = this.SellSideNumDayExtended;
      lockRequestLogToCopy.AlertLoanOfficer = this.alertLoanOfficer;
      lockRequestLogToCopy.SetRequestingUser(this.RequestedBy, this.RequestedFullName);
      lockRequestLogToCopy.PriceConcessionIndicator = this.PriceConcessionIndicator;
      lockRequestLogToCopy.LockExtensionIndicator = this.LockExtensionIndicator;
      lockRequestLogToCopy.PriceConcessionRequestStatus = this.PriceConcessionRequestStatus;
    }

    public string RequestedBy => this.requestedBy;

    public string RequestedFullName => this.requestedFullName;

    public string RequestedStatus
    {
      get => this.requestedStatus;
      set
      {
        this.requestedStatus = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateInvestorStatus();
        this.MarkAsDirty("LOCKRATE.REQUESTSTATUS");
      }
    }

    public string RequestedOldStatus
    {
      get => this.requestedOldStatus;
      set => this.requestedOldStatus = value;
    }

    public string RequestType
    {
      get
      {
        if (this.RateLockAction == RateLockAction.TradeExtension || this.ReviseAction == RateLockAction.TradeExtension)
          return "Trade Extension";
        if (this.isLockExtension)
          return "Extension";
        return this.isLockCancellation ? "Cancellation" : "Lock";
      }
    }

    public bool IsLockExtension
    {
      get => this.isLockExtension;
      set => this.isLockExtension = value;
    }

    public bool IsLockCancellation
    {
      get => this.isLockCancellation;
      set => this.isLockCancellation = value;
    }

    public bool IsRelock
    {
      get => this.isRelock;
      set => this.isRelock = value;
    }

    public int NumOfDaysLocked
    {
      get => this.numDayLocked;
      set => this.numDayLocked = value;
    }

    public int BuySideNumDayExtended
    {
      get => this.buySideNumDayExtended;
      set
      {
        this.buySideNumDayExtended = value;
        this.MarkAsDirty();
      }
    }

    public int SellSideNumDayExtended
    {
      get => this.sellSideNumDayExtended;
      set
      {
        this.sellSideNumDayExtended = value;
        this.MarkAsDirty();
      }
    }

    public int BuySideNumDayLocked
    {
      get => this.buySideNumDayLocked;
      set
      {
        this.buySideNumDayLocked = value;
        this.MarkAsDirty();
      }
    }

    public int CumulatedDaystoExtend
    {
      get => this.cumulatedDaystoExtend;
      set
      {
        this.cumulatedDaystoExtend = value;
        this.MarkAsDirty();
      }
    }

    public string InvestorName
    {
      get => this.investorName;
      set
      {
        this.investorName = value;
        this.MarkAsDirty();
      }
    }

    public string SellSideDeliveredBy
    {
      get => this.sellSideDeliveredBy;
      set => this.sellSideDeliveredBy = value;
    }

    public RateLockRequestStatus LockRequestStatus
    {
      get => RateLockEnum.GetRateLockEnum(this.requestedStatus);
      set => this.RequestedStatus = RateLockEnum.GetRateLockStatusString(value);
    }

    public RateLockRequestStatus LockRequestOldStatus
    {
      get => RateLockEnum.GetRateLockEnum(this.requestedOldStatus);
      set => this.RequestedOldStatus = RateLockEnum.GetRateLockStatusString(value);
    }

    public DateTime BuySideExpirationDate
    {
      get => this.buySideExpirationDate;
      set
      {
        this.buySideExpirationDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime BuySideNewLockExtensionDate
    {
      get => this.buySideNewLockExtensionDate;
      set
      {
        this.buySideNewLockExtensionDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime SellSideNewLockExtensionDate
    {
      get => this.sellSideNewLockExtensionDate;
      set
      {
        this.sellSideNewLockExtensionDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime SellSideExpirationDate
    {
      get => this.sellSideExpirationDate;
      set
      {
        this.sellSideExpirationDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime SellSideDeliveryDate
    {
      get => this.sellSideDeliveryDate;
      set
      {
        this.sellSideDeliveryDate = value;
        this.MarkAsDirty();
      }
    }

    public bool AlertLoanOfficer
    {
      get => this.alertLoanOfficer;
      set
      {
        this.alertLoanOfficer = value;
        this.MarkAsDirty();
      }
    }

    public override bool DisplayInLog
    {
      get => !this.hideLog;
      set
      {
        this.hideLog = !value;
        this.MarkAsDirty();
      }
    }

    public bool IsFakeRequest
    {
      get => this.isFakeRequest;
      set
      {
        this.isFakeRequest = value;
        this.MarkAsDirty();
      }
    }

    public string ParentLockGuid
    {
      get => this.parentLockGuid;
      set
      {
        this.parentLockGuid = value;
        this.MarkAsDirty();
      }
    }

    public int ReLockSequenceNumberForInactiveLock
    {
      get => this.reLockSequenceNumberForInactiveLock;
      set
      {
        this.reLockSequenceNumberForInactiveLock = value;
        this.MarkAsDirty();
      }
    }

    public RateLockAction RateLockAction
    {
      get
      {
        try
        {
          return (RateLockAction) Enum.Parse(typeof (RateLockAction), this.rateLockAction);
        }
        catch
        {
          return RateLockAction.UnKnown;
        }
      }
      set
      {
        this.rateLockAction = value.ToString();
        this.MarkAsDirty();
      }
    }

    public string PriceConcessionIndicator
    {
      get => this.concessionIndicator;
      set
      {
        this.concessionIndicator = value;
        this.MarkAsDirty();
      }
    }

    public string LockExtensionIndicator
    {
      get => this.extensionIndicator;
      set
      {
        this.extensionIndicator = value;
        this.MarkAsDirty();
      }
    }

    public string PriceConcessionRequestStatus
    {
      get => this.concessionRequestStatus;
      set
      {
        this.concessionRequestStatus = value;
        this.MarkAsDirty();
      }
    }

    public RateLockAction ReviseAction
    {
      get
      {
        try
        {
          return (RateLockAction) Enum.Parse(typeof (RateLockAction), this.reviseAction);
        }
        catch
        {
          return RateLockAction.UnKnown;
        }
      }
      set
      {
        this.reviseAction = value.ToString();
        this.MarkAsDirty();
      }
    }

    public bool IsLockRequestSnapshotDirty { get; set; }

    public bool ResetLockRequestSnapshot()
    {
      bool flag = this.lockRequestSnapshot != null;
      this.lockRequestSnapshot = (Hashtable) null;
      return flag;
    }

    public Hashtable GetLockRequestSnapshot()
    {
      if (this.lockRequestSnapshot == null)
      {
        this.lockRequestSnapshot = this.Log.Loan.GetLockRequestSnapshot(this.Guid);
        if (this.lockRequestSnapshot == null || this.lockRequestSnapshot.Count == 0)
        {
          if (this.Log.Loan.SnapshotProvider != null)
          {
            Dictionary<string, string> loanSnapshot = this.Log.Loan.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.LockRequest, new System.Guid(this.Guid), false);
            if (loanSnapshot != null)
              this.lockRequestSnapshot = new Hashtable((IDictionary) loanSnapshot);
          }
        }
        else
          this.IsLockRequestSnapshotDirty = true;
      }
      return this.lockRequestSnapshot == null ? (Hashtable) null : new Hashtable((IDictionary) this.lockRequestSnapshot);
    }

    public void AddLockRequestSnapshot(Hashtable lockRequestSnapshot)
    {
      if (lockRequestSnapshot == null)
        return;
      Hashtable hashtable = new Hashtable();
      foreach (object key in (IEnumerable) lockRequestSnapshot.Keys)
        hashtable.Add((object) key.ToString(), (object) lockRequestSnapshot[key].ToString());
      this.lockRequestSnapshot = hashtable;
      this.IsLockRequestSnapshotDirty = true;
      this.Log.Loan.AddLockRequestSnapshot(this.Guid, this.lockRequestSnapshot);
    }

    public void ClearSnapshot()
    {
      if (this.LockRequestStatus == RateLockRequestStatus.RateLocked || this.LockRequestStatus == RateLockRequestStatus.Requested || this.LockRequestStatus == RateLockRequestStatus.ExtensionRequested || this.LockRequestStatus == RateLockRequestStatus.Cancelled || this.LockRequestStatus == RateLockRequestStatus.Voided || this.LockRequestStatus == RateLockRequestStatus.RequestDenied || this.LockRequestStatus == RateLockRequestStatus.OldLock)
        return;
      this.Log.Loan.ClearLockRequestSnapshot(this.Guid);
    }

    [CLSCompliant(false)]
    public string LockRequestSnapshotString
    {
      get
      {
        if (this.lockRequestSnapshot == null || this.lockRequestSnapshot.Count == 0)
          return string.Empty;
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement element = xmlDocument.CreateElement("Log");
        element.SetAttribute("GUID", this.Guid);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        foreach (DictionaryEntry dictionaryEntry in this.lockRequestSnapshot)
        {
          string str1 = dictionaryEntry.Key.ToString();
          string str2 = dictionaryEntry.Value.ToString();
          if (!(str2 == string.Empty))
          {
            XmlElement xmlElement = (XmlElement) element.AppendChild((XmlNode) xmlDocument.CreateElement("FIELD"));
            xmlElement.SetAttribute("id", str1);
            xmlElement.SetAttribute("val", str2);
          }
        }
        this.lockRequestSnapshotString = element.OuterXml;
        return this.lockRequestSnapshotString;
      }
      set => this.lockRequestSnapshotString = value;
    }
  }
}
