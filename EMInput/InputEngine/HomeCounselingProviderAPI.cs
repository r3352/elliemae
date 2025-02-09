// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HomeCounselingProviderAPI
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public static class HomeCounselingProviderAPI
  {
    private const string className = "HomeCounselingProviderAPI";
    private static readonly string sw = Tracing.SwOutsideLoan;
    internal static string HudWebURL = "https://data.hud.gov/Housing_Counselor/";

    public static bool updateLanguageServiceCodes(
      Sessions.Session session,
      ref List<KeyValuePair<string, string>> serviceList,
      ref List<KeyValuePair<string, string>> languageList)
    {
      DateTime date1 = Utils.ParseDate((object) session.ConfigurationManager.GetCompanySetting("HomeCounseling", "LastUpdateDate"));
      if ((session.ServerTime.Date - date1.Date).TotalDays <= 2.0)
      {
        if (serviceList != null && serviceList.Count > 0 && languageList != null && languageList.Count > 0)
          return true;
        List<KeyValuePair<string, string>>[] languageSupported = session.ConfigurationManager.GetHomeCounselingServiceLanguageSupported();
        if (languageSupported != null && languageSupported.Length == 2)
        {
          serviceList = languageSupported[0];
          languageList = languageSupported[1];
        }
        if (serviceList != null && serviceList.Count > 0 && languageList != null && languageList.Count > 0)
          return true;
      }
      DateTime date2 = Utils.ParseDate((object) session.ConfigurationManager.GetCompanySetting("HomeCounseling", "UpdateLocked"));
      if ((session.ServerTime - date2).TotalMinutes < 3.0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass is updating the Service and Language tables. Please try it later.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      Cursor.Current = Cursors.WaitCursor;
      session.ConfigurationManager.SetCompanySetting("HomeCounseling", "UpdateLocked", session.ServerTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
      bool flag = false;
      if (languageList == null || languageList.Count == 0)
      {
        flag = true;
        List<List<string[]>> strArrayListList = (List<List<string[]>>) null;
        try
        {
          string homeCounseling = session.SessionObjects.GetHomeCounseling(HomeCounselingProviderAPI.HudWebURL + "getLanguages", (IWin32Window) session.MainForm);
          strArrayListList = session.SessionObjects.ParseHomeCounselingResults(homeCounseling);
        }
        catch (Exception ex)
        {
          Tracing.Log(HomeCounselingProviderAPI.sw, TraceLevel.Error, nameof (HomeCounselingProviderAPI), "Cannot get latest language supported list from URL '" + HomeCounselingProviderAPI.HudWebURL + "getLanguages'. Error: " + ex.Message);
        }
        if (strArrayListList != null && strArrayListList.Count > 0)
        {
          languageList = new List<KeyValuePair<string, string>>();
          string key = (string) null;
          string str = (string) null;
          foreach (List<string[]> strArrayList in strArrayListList)
          {
            foreach (string[] strArray in strArrayList)
            {
              if (string.Compare(strArray[0], "key", true) == 0)
                key = strArray[1];
              else if (string.Compare(strArray[0], "value", true) == 0)
                str = strArray[1];
            }
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(str))
              languageList.Add(new KeyValuePair<string, string>(key, str));
          }
        }
      }
      if (serviceList == null || serviceList.Count == 0)
      {
        flag = true;
        List<List<string[]>> strArrayListList = (List<List<string[]>>) null;
        try
        {
          string homeCounseling = session.SessionObjects.GetHomeCounseling(HomeCounselingProviderAPI.HudWebURL + "getServices", (IWin32Window) session.MainForm);
          strArrayListList = session.SessionObjects.ParseHomeCounselingResults(homeCounseling);
        }
        catch (Exception ex)
        {
          Tracing.Log(HomeCounselingProviderAPI.sw, TraceLevel.Error, nameof (HomeCounselingProviderAPI), "Cannot get latest service supported list from URL '" + HomeCounselingProviderAPI.HudWebURL + "getServices'. Error: " + ex.Message);
        }
        if (strArrayListList != null && strArrayListList.Count > 0)
        {
          serviceList = new List<KeyValuePair<string, string>>();
          string key = (string) null;
          string str = (string) null;
          foreach (List<string[]> strArrayList in strArrayListList)
          {
            foreach (string[] strArray in strArrayList)
            {
              if (string.Compare(strArray[0], "key", true) == 0)
                key = strArray[1];
              else if (string.Compare(strArray[0], "value", true) == 0)
                str = strArray[1];
            }
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(str))
              serviceList.Add(new KeyValuePair<string, string>(key, str));
          }
        }
      }
      try
      {
        if (flag)
          session.ConfigurationManager.UpdateHomeCounselingCodes(serviceList, languageList);
        IConfigurationManager configurationManager = session.ConfigurationManager;
        DateTime dateTime = session.ServerTime;
        dateTime = dateTime.Date;
        string str = dateTime.ToString("MM/dd/yyyy");
        configurationManager.SetCompanySetting("HomeCounseling", "LastUpdateDate", str);
        session.ConfigurationManager.SetCompanySetting("HomeCounseling", "UpdateLocked", "");
      }
      catch (Exception ex)
      {
        Tracing.Log(HomeCounselingProviderAPI.sw, TraceLevel.Error, nameof (HomeCounselingProviderAPI), "Cannot update Home Counseling Language/Service Supported. Error: " + ex.Message);
      }
      Cursor.Current = Cursors.Default;
      return true;
    }
  }
}
