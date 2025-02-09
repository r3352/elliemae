// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.JSonObjects.JSonUtil
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.JSonObjects
{
  public static class JSonUtil
  {
    private const string className = "JSonUtil�";
    private static string sw = Tracing.SwOutsideLoan;

    public static string getConsentPDFFromJSON(string JSONstring)
    {
      try
      {
        if (!string.IsNullOrEmpty(JSONstring))
        {
          ConsentPdf consentPdf = JsonConvert.DeserializeObject<ConsentPdf>(JSONstring);
          if (consentPdf != null)
            return consentPdf.PDF;
        }
        return (string) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(JSonUtil.sw, TraceLevel.Verbose, nameof (JSonUtil), "getConsentPDFFromJSON Error: " + ex.Message);
        return (string) null;
      }
    }

    public static ConsentTrackingDetails getConsentTrackingFromJSON(string JSONstring)
    {
      try
      {
        return !string.IsNullOrEmpty(JSONstring) ? JsonConvert.DeserializeObject<ConsentTrackingDetails>(JSONstring) : (ConsentTrackingDetails) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(JSonUtil.sw, TraceLevel.Verbose, nameof (JSonUtil), "getConsentTrackingFromJSON Error: " + ex.Message);
        return (ConsentTrackingDetails) null;
      }
    }

    public static JAIQIncome getAIQIncomeDataFromJSON(string JSONstring)
    {
      try
      {
        return !string.IsNullOrEmpty(JSONstring) ? JsonConvert.DeserializeObject<JAIQIncome>(JSONstring) : (JAIQIncome) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(JSonUtil.sw, TraceLevel.Verbose, nameof (JSonUtil), "getAIQIncomeDataFromJSON Error: " + ex.Message);
        return (JAIQIncome) null;
      }
    }
  }
}
