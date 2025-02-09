// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ReportFieldCache
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ReportFieldCache
  {
    private static Dictionary<string, string> reportMapFields = new Dictionary<string, string>();
    private Dictionary<string, string> reportFields = new Dictionary<string, string>();
    private static Dictionary<string, string> fieldsDescription = (Dictionary<string, string>) null;

    static ReportFieldCache() => ReportFieldCache.getReportMapFields();

    public ReportFieldCache() => this.getReportFields();

    public string GetFieldId(string dbName)
    {
      if (dbName == null || string.Empty == dbName)
        return string.Empty;
      if (this.reportFields.ContainsKey(dbName))
        return this.reportFields[dbName];
      int startIndex = dbName.LastIndexOf('.') + 1;
      return dbName.Substring(startIndex);
    }

    public static string getFieldDescription(string field)
    {
      if (ReportFieldCache.fieldsDescription != null)
      {
        string fieldDescription;
        ReportFieldCache.fieldsDescription.TryGetValue(field, out fieldDescription);
        return fieldDescription;
      }
      ReportFieldCache.populateDescriptionMap();
      string fieldDescription1;
      ReportFieldCache.fieldsDescription.TryGetValue(field, out fieldDescription1);
      return fieldDescription1;
    }

    private static void populateDescriptionMap()
    {
      string str = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "documents\\ReportMap.xml");
      if (!File.Exists(str))
        return;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(str);
        ReportFieldCache.fieldsDescription = new Dictionary<string, string>();
        foreach (XmlNode selectNode1 in xmlDocument.SelectNodes("//Catagory"))
        {
          foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Field"))
          {
            string key1 = selectNode2.GetAttribute("dbname") ?? "";
            string key2 = selectNode2.GetAttribute("id") ?? "";
            string attribute = selectNode2.GetAttribute("desc");
            if (key1 != "" && !ReportFieldCache.fieldsDescription.ContainsKey(key1))
              ReportFieldCache.fieldsDescription.Add(key1, attribute);
            if (key2 != "" && !ReportFieldCache.fieldsDescription.ContainsKey(key2))
              ReportFieldCache.fieldsDescription.Add(key2, attribute);
          }
        }
      }
      catch
      {
      }
    }

    private static void getReportMapFields()
    {
      string str1 = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "documents\\ReportMap.xml");
      if (!File.Exists(str1))
        throw new Exception("ReportMap.xml file cannot be found");
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(str1);
        foreach (XmlNode selectNode1 in xmlDocument.SelectNodes("//Catagory"))
        {
          foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Field"))
          {
            string key = selectNode2.GetAttribute("dbname") ?? "";
            string str2 = selectNode2.GetAttribute("id") ?? "";
            if (string.Empty != str2 && string.Empty != key && !ReportFieldCache.reportMapFields.ContainsKey(key))
              ReportFieldCache.reportMapFields.Add(key, str2);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error parsing ReportMap.xml", ex);
      }
    }

    private void getReportFields()
    {
      foreach (string key in ReportFieldCache.reportMapFields.Keys)
        this.reportFields.Add(key, ReportFieldCache.reportMapFields[key]);
      foreach (LoanXDBField loanXdbField in (IEnumerable) LoanXDBStore.GetLoanXDBTableList().GetFields().Values)
      {
        if (loanXdbField.FieldID != null && string.Empty != loanXdbField.FieldID)
        {
          string key = "Fields." + loanXdbField.FieldID;
          if (!this.reportFields.ContainsKey(key))
            this.reportFields.Add(key, loanXdbField.FieldID);
        }
      }
    }
  }
}
