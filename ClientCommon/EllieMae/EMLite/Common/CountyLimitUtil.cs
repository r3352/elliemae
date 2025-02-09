// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CountyLimitUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CountyLimitUtil
  {
    private const string className = "CountyLimitUtil";
    private static readonly string sw = Tracing.SwCommon;

    public static CountyLimit[] GetCountyLimitFromUrl(string url)
    {
      int num = 0;
      List<CountyLimit> countyLimitList = new List<CountyLimit>();
      try
      {
        string str1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "hecm-limits.txt");
        if (System.IO.File.Exists(str1))
          System.IO.File.Delete(str1);
        new WebClient().DownloadFile(url, str1);
        using (StreamReader streamReader = new StreamReader(str1))
        {
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            ++num;
            if (str2.Length >= 163)
            {
              CountyLimit countyLimit = new CountyLimit();
              countyLimit.MsaCode = Utils.ParseInt((object) str2.Substring(0, 5).Trim());
              countyLimit.MetropolitanDivisionCode = Utils.ParseInt((object) str2.Substring(5, 5).Trim());
              countyLimit.MsaName = str2.Substring(10, 50).Trim();
              countyLimit.SoaCode = str2.Substring(60, 5).Trim();
              countyLimit.LimitType = str2.Substring(65, 1).Trim();
              countyLimit.MedianPrice = Utils.ParseInt((object) str2.Substring(66, 7).Trim());
              countyLimit.LimitFor1Unit = Utils.ParseInt((object) str2.Substring(73, 7).Trim());
              countyLimit.LimitFor2Units = Utils.ParseInt((object) str2.Substring(80, 7).Trim());
              countyLimit.LimitFor3Units = Utils.ParseInt((object) str2.Substring(87, 7).Trim());
              countyLimit.LimitFor4Units = Utils.ParseInt((object) str2.Substring(94, 7).Trim());
              countyLimit.StateAbbreviation = str2.Substring(101, 2).Trim();
              countyLimit.CountyCode = Utils.ParseInt((object) str2.Substring(103, 3).Trim());
              countyLimit.StateName = str2.Substring(106, 26).Trim();
              countyLimit.CountyName = str2.Substring(132, 15).Trim();
              countyLimit.CountyTransactionDate = str2.Substring(147, 8).Trim();
              countyLimit.LimitTransactionDate = str2.Substring(155, 8).Trim();
              countyLimit.LastModifiedDateTime = DateTime.Now;
              countyLimit.Customized = false;
              if (countyLimit.CountyName != "" && countyLimit.StateName != "")
                countyLimitList.Add(countyLimit);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CountyLimitUtil.sw, TraceLevel.Error, nameof (CountyLimitUtil), "Failed to retrieve county limit from URL: " + url + " in the line " + (object) num + ", Error Message:" + ex.ToString());
        countyLimitList.Clear();
      }
      return countyLimitList.ToArray();
    }
  }
}
