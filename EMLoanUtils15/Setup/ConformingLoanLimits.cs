// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConformingLoanLimits
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public static class ConformingLoanLimits
  {
    private const string className = "ConformingLoanLimits�";
    private static string sw = Tracing.SwDataEngine;
    private static XmlDocument limitsXml = (XmlDocument) null;
    private static Dictionary<StateCounty, string> countyNameMap = (Dictionary<StateCounty, string>) null;
    private static ConventionalCountyLimit[] counties = (ConventionalCountyLimit[]) null;
    private static string state = (string) null;
    private static int year = -1;

    static ConformingLoanLimits()
    {
      ConformingLoanLimits.loadLoanLimitsXml();
      ConformingLoanLimits.loadCountyNameMap();
    }

    private static void loadLoanLimitsXml()
    {
      string str = Path.Combine(SystemSettings.EpassDataDir, "LoanLimits.dat");
      if (File.Exists(str))
      {
        try
        {
          ConformingLoanLimits.limitsXml = ConformingLoanLimits.parseDataFile(str);
        }
        catch (Exception ex)
        {
          Tracing.Log(ConformingLoanLimits.sw, nameof (ConformingLoanLimits), TraceLevel.Error, "Background downloaded loan limits file could not be passed. Local install file will be used instead.");
        }
      }
      if (ConformingLoanLimits.limitsXml != null)
        return;
      ConformingLoanLimits.limitsXml = ConformingLoanLimits.parseDataFile(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.LocalAppDir, "documents\\LoanLimits.dat") : AssemblyResolver.GetResourceFileFullPath("documents\\LoanLimits.dat"));
    }

    private static void loadCountyNameMap()
    {
      ConformingLoanLimits.countyNameMap = ConformingLoanLimits.parseCountyNameMap(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.LocalAppDir, "documents\\CountyNameMap.xml") : AssemblyResolver.GetResourceFileFullPath("documents\\CountyNameMap.xml"));
    }

    public static Decimal GetCountyLimit(
      SessionObjects sessionObjects,
      string stateAbbr,
      string countyName,
      int numUnits)
    {
      DateTime today = DateTime.Today;
      if (today.Year <= 2022)
        return ConformingLoanLimits.getCountyLimit(ConformingLoanLimits.getCountyXml(ConformingLoanLimits.getStateXml(ConformingLoanLimits.getLoanPeriodXml(today), stateAbbr), countyName), numUnits);
      Decimal conventionalCountyLimit = ConformingLoanLimits.GetConventionalCountyLimit(sessionObjects, stateAbbr, countyName, numUnits, today);
      return conventionalCountyLimit == 0M ? ConformingLoanLimits.GetCountyLimit(sessionObjects, stateAbbr, countyName, numUnits, today) : conventionalCountyLimit;
    }

    public static Decimal GetCountyLimit(
      SessionObjects sessionObjects,
      string stateAbbr,
      string countyName,
      int numUnits,
      DateTime date)
    {
      return ConformingLoanLimits.getCountyLimit(ConformingLoanLimits.getCountyXml(ConformingLoanLimits.getStateXml(ConformingLoanLimits.getLoanPeriodXml(date), stateAbbr), countyName), numUnits);
    }

    public static Decimal GetCountyLimit(LoanData loan, SessionObjects sessionObjects)
    {
      string simpleField1 = loan.GetSimpleField("14");
      string simpleField2 = loan.GetSimpleField("13");
      int numUnits = Utils.ParseInt((object) loan.GetSimpleField("16"), 1);
      string simpleField3 = loan.GetSimpleField("745");
      DateTime date = !(simpleField3 != "//") || !(simpleField3 != "") ? DateTime.Today : Utils.ParseDate((object) simpleField3);
      if (Utils.ParseDate((object) simpleField3).Year <= 2022)
        return ConformingLoanLimits.GetCountyLimit(sessionObjects, simpleField1, simpleField2, numUnits, date);
      Decimal conventionalCountyLimit = ConformingLoanLimits.GetConventionalCountyLimit(sessionObjects, simpleField1, simpleField2, numUnits, date);
      return conventionalCountyLimit == 0M ? ConformingLoanLimits.GetCountyLimit(sessionObjects, simpleField1, simpleField2, numUnits, date) : conventionalCountyLimit;
    }

    public static Decimal GetConventionalCountyLimit(
      SessionObjects sessionObjects,
      string stateAbbr,
      string countyName,
      int numUnits,
      DateTime date)
    {
      Decimal conventionalCountyLimit = 0M;
      if (string.IsNullOrEmpty(stateAbbr) || string.IsNullOrEmpty(countyName) || date == DateTime.MinValue)
        throw new ArgumentException("Not enough data to calculate conforming loan limit" + countyName);
      if (ConformingLoanLimits.counties == null || stateAbbr != ConformingLoanLimits.state || date.Year != ConformingLoanLimits.year)
      {
        ConformingLoanLimits.counties = sessionObjects?.ConfigurationManager.GetConventionalCountyLimits(stateAbbr, Utils.ParseDate((object) date).Year);
        ConformingLoanLimits.state = stateAbbr;
        ConformingLoanLimits.year = date.Year;
      }
      if (ConformingLoanLimits.counties == null || ((IEnumerable<ConventionalCountyLimit>) ConformingLoanLimits.counties).Count<ConventionalCountyLimit>() == 0)
        return conventionalCountyLimit;
      string fips = ZipCodeUtils.GetFIPS(stateAbbr, countyName);
      ConventionalCountyLimit limit = ConformingLoanLimits.findLimit(ConformingLoanLimits.counties, countyName, (StateCounty) null, fips);
      if (limit == null)
      {
        StateCounty stateCounty = new StateCounty(stateAbbr, countyName);
        if (ConformingLoanLimits.countyNameMap != null && ConformingLoanLimits.countyNameMap.ContainsKey(stateCounty))
          countyName = ConformingLoanLimits.countyNameMap[stateCounty];
        limit = ConformingLoanLimits.findLimit(ConformingLoanLimits.counties, countyName, stateCounty, (string) null);
      }
      if (limit == null)
        throw new ArgumentException("Conforming Loan Limit data is not available for the give data" + countyName);
      if (numUnits <= 0 || numUnits == 1)
        conventionalCountyLimit = (Decimal) limit.LimitFor1Unit;
      else if (numUnits == 2)
        conventionalCountyLimit = (Decimal) limit.LimitFor2Units;
      else if (numUnits == 3)
        conventionalCountyLimit = (Decimal) limit.LimitFor3Units;
      else if (numUnits >= 4)
        conventionalCountyLimit = (Decimal) limit.LimitFor4Units;
      return conventionalCountyLimit;
    }

    private static bool findCountyLimitsUsingAdjustedName(
      string cn,
      string countyName,
      StateCounty sc)
    {
      if (string.Compare(countyName, cn, true) == 0 || string.Compare(countyName, cn.Replace("'", ""), true) == 0 || string.Compare(countyName, cn.Replace("-", " "), true) == 0 || string.Compare(countyName, cn.Replace(".", ""), true) == 0 || string.Compare(countyName, cn.Replace(",", ""), true) == 0 || string.Compare(countyName, cn.Replace("ST.", "SAINT"), true) == 0 || string.Compare(countyName, "SKAGWAY-HOONAH", true) == 0 && string.Compare(cn, "SKAGWAY", true) == 0 || string.Compare(countyName, "WRANGELL-PETERS", true) == 0 && string.Compare(cn, "WRANGELL CITY", true) == 0)
        return true;
      if (sc != null)
      {
        if (sc.State == "AK")
        {
          if (string.Compare(countyName, "Prince Of Wales Hyder", true) == 0 && string.Compare(cn, "PRINCE OF WALES-HYDER CENSUS AREA", true) == 0)
            return true;
        }
        else if (sc.State == "AL" || sc.State == "MN")
        {
          if (string.Compare(countyName, "DEKALB", true) == 0 && string.Compare(cn, "DE KALB", true) == 0)
            return true;
        }
        else if (sc.State == "VA")
        {
          if ((string.Compare(countyName, "NEWPORT NEWS CI", true) == 0 || string.Compare(countyName, "NEWPORT NEWS CITY", true) == 0) && string.Compare(cn, "NEWPORT NEWS", true) == 0 || (string.Compare(countyName, "MANASSAS PARK C", true) == 0 || string.Compare(countyName, "MANASSAS PARK CITY", true) == 0) && string.Compare(cn, "MANASSAS PARK", true) == 0 || (string.Compare(countyName, "BUENA VISTA CIT", true) == 0 || string.Compare(countyName, "BUENA VISTA CITY", true) == 0) && string.Compare(cn, "BUENA VISTA", true) == 0 || (string.Compare(countyName, "FALLS CHURCH CI", true) == 0 || string.Compare(countyName, "FALLS CHURCH CITY", true) == 0) && string.Compare(cn, "FALLS CHURCH", true) == 0 || (string.Compare(countyName, "HARRISONBURG CI", true) == 0 || string.Compare(countyName, "HARRISONBURG CITY", true) == 0) && string.Compare(cn, "HARRISONBURG", true) == 0 || string.Compare(countyName, "BRISTOL CITY", true) == 0 && string.Compare(cn, "BRISTOL", true) == 0 || (string.Compare(countyName, "MARTINSVILLE CI", true) == 0 || string.Compare(countyName, "MARTINSVILLE CITY", true) == 0) && string.Compare(cn, "MARTINSVILLE", true) == 0 || string.Compare(countyName, "EMPORIA CITY", true) == 0 && string.Compare(cn, "EMPORIA", true) == 0 || string.Compare(countyName, "GALAX CITY", true) == 0 && string.Compare(cn, "GALAX", true) == 0 || string.Compare(countyName, "LEXINGTON CITY", true) == 0 && string.Compare(cn, "LEXINGTON", true) == 0 || string.Compare(countyName, "SALEM CITY", true) == 0 && string.Compare(cn, "SALEM", true) == 0 || string.Compare(countyName, "RADFORD CITY", true) == 0 && string.Compare(cn, "RADFORD", true) == 0)
            return true;
        }
        else if (sc.State == "ND")
        {
          if (string.Compare(countyName, "LAMOURE", true) == 0 && string.Compare(cn, "LA MOURE", true) == 0)
            return true;
        }
        else if (sc.State == "VI")
        {
          if (string.Compare(countyName, "ST. THOMAS ISLA", true) == 0 && string.Compare(cn, "ST. THOMAS", true) == 0 || string.Compare(countyName, "ST. CROIX ISLAN", true) == 0 && string.Compare(cn, "ST. CROIX", true) == 0 || string.Compare(countyName, "ST. JOHN ISLAND", true) == 0 && string.Compare(cn, "ST. JOHN,VI", true) == 0)
            return true;
        }
        else if (sc.State == "MO")
        {
          if (string.Compare(countyName, "DEKALB", true) == 0 && string.Compare(cn, "DE KALB", true) == 0)
            return true;
        }
        else if (sc.State == "FL")
        {
          if (string.Compare(countyName, "DESOTO", true) == 0 && string.Compare(cn, "DE SOTO", true) == 0)
            return true;
        }
        else if (sc.State == "IN")
        {
          if (string.Compare(countyName, "DEKALB", true) == 0 && string.Compare(cn, "DE KALB", true) == 0 || string.Compare(countyName, "LAPORTE", true) == 0 && string.Compare(cn, "LA PORTE", true) == 0)
            return true;
        }
        else if (sc.State == "IL" && string.Compare(countyName, "LASALLE", true) == 0 && string.Compare(cn, "LA SALLE", true) == 0)
          return true;
      }
      return false;
    }

    private static ConventionalCountyLimit findLimit(
      ConventionalCountyLimit[] counties,
      string countyName,
      StateCounty sc,
      string fips)
    {
      if (!string.IsNullOrEmpty(fips))
      {
        foreach (ConventionalCountyLimit county in counties)
        {
          if (county.FIPSStateCode + county.FIPSCountyCode == fips)
            return county;
        }
      }
      foreach (ConventionalCountyLimit county in counties)
      {
        if (ConformingLoanLimits.findCountyLimitsUsingAdjustedName(county.CountyName, countyName, sc))
          return county;
      }
      return (ConventionalCountyLimit) null;
    }

    public static bool IsConforming(LoanData loan, SessionObjects sessionObjects)
    {
      Decimal num;
      try
      {
        num = Utils.ParseDecimal((object) loan.GetSimpleField("2"), true);
      }
      catch
      {
        throw new ArgumentException("The loan file does not contain a valid loan amount");
      }
      Decimal countyLimit = ConformingLoanLimits.GetCountyLimit(loan, sessionObjects);
      return num <= countyLimit;
    }

    private static Decimal getCountyLimit(XmlElement countyXml, int numUnits)
    {
      string name = (string) null;
      if (numUnits <= 0)
        name = "N1Unit";
      else if (numUnits >= 1 && numUnits <= 4)
        name = "N" + (object) numUnits + "Unit";
      else if (numUnits > 4)
        name = "N4Unit";
      return Utils.ParseDecimal((object) countyXml.GetAttribute(name));
    }

    private static XmlElement getCountyXml(
      XmlElement periodXml,
      string stateAbbr,
      string countyName)
    {
      return ConformingLoanLimits.getCountyXml(ConformingLoanLimits.getStateXml(periodXml, stateAbbr), countyName);
    }

    private static XmlElement getCountyXml(XmlElement stateXml, string countyName)
    {
      StateCounty stateCounty = new StateCounty(stateXml.GetAttribute("Abbr"), countyName);
      if (ConformingLoanLimits.countyNameMap != null && ConformingLoanLimits.countyNameMap.ContainsKey(stateCounty))
        countyName = ConformingLoanLimits.countyNameMap[stateCounty];
      foreach (XmlElement selectNode in stateXml.SelectNodes(".//County"))
      {
        if (ConformingLoanLimits.findCountyLimitsUsingAdjustedName(selectNode.GetAttribute("Name"), countyName, stateCounty))
          return selectNode;
      }
      throw new ArgumentException("Invalid or unknown county: " + countyName);
    }

    private static XmlElement getStateXml(XmlElement periodXml, string stateAbbr)
    {
      return (XmlElement) periodXml.SelectSingleNode("State[@Abbr='" + ConformingLoanLimits.xmlEncode(stateAbbr.ToUpper()) + "']") ?? throw new ArgumentException("Invalid or unknown state abbreviation: " + stateAbbr);
    }

    private static XmlElement getLoanPeriodXml(DateTime date)
    {
      foreach (XmlElement selectNode in ConformingLoanLimits.limitsXml.SelectNodes("//LoanPeriod"))
      {
        DateTime date1 = Utils.ParseDate((object) selectNode.GetAttribute("StartDate"), DateTime.MinValue);
        DateTime date2 = Utils.ParseDate((object) selectNode.GetAttribute("EndDate"), DateTime.MaxValue);
        if (date.Date >= date1 && date.Date <= date2)
          return selectNode;
      }
      throw new ArgumentException("Conforming Loan Limit data is not available for the date " + date.ToShortDateString());
    }

    private static string xmlEncode(string text)
    {
      text = text.Replace("&", "&amp;");
      text = text.Replace("'", "&apos;");
      return text;
    }

    private static XmlDocument parseDataFile(string filePath)
    {
      try
      {
        byte[] numArray = (byte[]) null;
        using (FileStream fileStream = File.OpenRead(filePath))
        {
          int length = (int) fileStream.Length;
          numArray = new byte[length];
          fileStream.Read(numArray, 0, length);
        }
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] += (byte) 5;
        string xml = Encoding.ASCII.GetString(numArray);
        XmlDocument dataFile = new XmlDocument();
        dataFile.LoadXml(xml);
        return dataFile;
      }
      catch (Exception ex)
      {
        Tracing.Log(ConformingLoanLimits.sw, nameof (ConformingLoanLimits), TraceLevel.Error, "Error parsing loan limit file '" + filePath + "': " + (object) ex);
        throw new Exception("Error reading LoanLimits.xml file", ex);
      }
    }

    private static Dictionary<StateCounty, string> parseCountyNameMap(string filePath)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath);
        Dictionary<StateCounty, string> countyNameMap = new Dictionary<StateCounty, string>();
        foreach (XmlElement selectNode in xmlDocument.SelectNodes("//Mapping"))
        {
          StateCounty key = new StateCounty(selectNode.GetAttribute("StateCode"), selectNode.GetAttribute("Zipcode"));
          countyNameMap[key] = selectNode.GetAttribute("CountyLimit");
        }
        return countyNameMap;
      }
      catch (Exception ex)
      {
        Tracing.Log(ConformingLoanLimits.sw, nameof (ConformingLoanLimits), TraceLevel.Error, "Error parsing county name map file '" + filePath + "': " + (object) ex);
        throw new Exception("Error reading " + filePath + " file", ex);
      }
    }
  }
}
