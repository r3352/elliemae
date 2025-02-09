// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.OutputFormNameMap
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class OutputFormNameMap
  {
    private const string className = "OutputFormNameMap";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static Hashtable _formKeyToName = (Hashtable) null;
    private static Hashtable _formNameToKey = (Hashtable) null;
    private static IDictionary _formList = (IDictionary) null;
    private static Dictionary<string, MergeParamValues> _allEdsFormsAndParams = (Dictionary<string, MergeParamValues>) null;

    static OutputFormNameMap()
    {
      if (Session.DefaultInstance == null)
        return;
      OutputFormNameMap.LoadFormNameMap(Session.DefaultInstance);
    }

    public static string GetFormNameToKey(string formName, Sessions.Session session)
    {
      if (OutputFormNameMap._formNameToKey == null)
        OutputFormNameMap.LoadFormNameMap(session);
      return formName == string.Empty || !OutputFormNameMap._formNameToKey.ContainsKey((object) formName) ? formName : OutputFormNameMap._formNameToKey[(object) formName].ToString();
    }

    public static string GetFormKeyToName(string formKey, Sessions.Session session)
    {
      if (OutputFormNameMap._formKeyToName == null)
        OutputFormNameMap.LoadFormNameMap(session);
      return formKey == string.Empty || !OutputFormNameMap._formKeyToName.ContainsKey((object) formKey) ? formKey : OutputFormNameMap._formKeyToName[(object) formKey].ToString();
    }

    public static bool ContainStandardForm(string formName, Sessions.Session session)
    {
      if (OutputFormNameMap._formList == null)
        OutputFormNameMap.LoadFormNameMap(session);
      return OutputFormNameMap._formList.Contains((object) formName);
    }

    public static string GetOutputFormFileName(string formName, Sessions.Session session)
    {
      if (OutputFormNameMap._formList == null)
        OutputFormNameMap.LoadFormNameMap(session);
      return OutputFormNameMap.ContainStandardForm(formName, session) ? (string) OutputFormNameMap._formList[(object) formName] : string.Empty;
    }

    public static Dictionary<string, MergeParamValues> AllEdsFormsAndParams
    {
      get => OutputFormNameMap._allEdsFormsAndParams;
    }

    public static void LoadFormNameMap(Sessions.Session session)
    {
      OutputFormNameMap._formKeyToName = CollectionsUtil.CreateCaseInsensitiveHashtable();
      OutputFormNameMap._formNameToKey = CollectionsUtil.CreateCaseInsensitiveHashtable();
      OutputFormNameMap._formList = (IDictionary) CollectionsUtil.CreateCaseInsensitiveHashtable();
      OutputFormNameMap._allEdsFormsAndParams = new Dictionary<string, MergeParamValues>();
      try
      {
        foreach (PrintForm printForm in PrintFormList.Parse(session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping)))
        {
          OutputFormNameMap._formKeyToName.Add((object) printForm.FormID, (object) printForm.UIName);
          OutputFormNameMap._formNameToKey.Add((object) printForm.UIName, (object) printForm.FormID);
          OutputFormNameMap._formList.Add((object) printForm.FormID, (object) printForm.FileName);
          if (printForm.MergeLocation == PrintForm.MergeLocationValues.EDS)
            OutputFormNameMap._allEdsFormsAndParams.Add(printForm.UIName, printForm.MergeParams.Clone());
        }
        string key1 = "1003 - All Required Pages";
        if (!OutputFormNameMap._formNameToKey.Contains((object) key1))
          OutputFormNameMap._formNameToKey.Add((object) key1, (object) key1);
        string key2 = "1003 - All Required Pages (Letter)";
        if (OutputFormNameMap._formNameToKey.Contains((object) key2))
          return;
        OutputFormNameMap._formNameToKey.Add((object) key2, (object) key2);
      }
      catch (Exception ex)
      {
        throw new Exception("OutputFormNameMap/loadFormNameMap: Can't load OutFormAndFileMapping.", ex);
      }
    }
  }
}
