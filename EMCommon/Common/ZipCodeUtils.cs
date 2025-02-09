// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ZipCodeUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Device.Location;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ZipCodeUtils
  {
    private static Hashtable tbl = new Hashtable();
    private static Hashtable tblState = new Hashtable();
    private static Hashtable tblFips = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static HashSet<string> county = new HashSet<string>();

    public static Hashtable ZipcodeTable => ZipCodeUtils.tbl;

    static ZipCodeUtils()
    {
      string empty1 = string.Empty;
      StreamReader streamReader = new StreamReader((Stream) File.OpenRead(!AssemblyResolver.IsSmartClient ? SystemSettings.ZipCodeFile : AssemblyResolver.GetResourceFileFullPath(SystemSettings.ZipCodeFileRelPath)));
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string str1;
      while ((str1 = streamReader.ReadLine()) != null)
      {
        string str2 = str1.Trim();
        if (str2.Length > 7)
        {
          string key = str2.Substring(0, 5);
          string str3 = str2.Substring(5, 2);
          List<ZipCodeInfo> zipCodeInfoList;
          if (!ZipCodeUtils.tbl.ContainsKey((object) key))
          {
            zipCodeInfoList = new List<ZipCodeInfo>();
            ZipCodeUtils.tbl.Add((object) key, (object) zipCodeInfoList);
          }
          else
            zipCodeInfoList = (List<ZipCodeInfo>) ZipCodeUtils.tbl[(object) key];
          List<StateInfo> stateInfoList;
          if (!ZipCodeUtils.tblState.ContainsKey((object) str3))
          {
            stateInfoList = new List<StateInfo>();
            ZipCodeUtils.tblState.Add((object) str3, (object) stateInfoList);
          }
          else
            stateInfoList = (List<StateInfo>) ZipCodeUtils.tblState[(object) str3];
          if (str2.Length > 7 && str2.Length < 30)
          {
            zipCodeInfoList.Add(new ZipCodeInfo(str3, str2.Substring(7), string.Empty));
            stateInfoList.Add(new StateInfo(str2.Substring(0, 5), str2.Substring(7), string.Empty));
          }
          else if (str2.Length > 30 && str2.Length < 69)
          {
            zipCodeInfoList.Add(new ZipCodeInfo(str3, str2.Substring(7, 22).Trim(), str2.Substring(29)));
            stateInfoList.Add(new StateInfo(str2.Substring(0, 5), str2.Substring(7, 22).Trim(), str2.Substring(29)));
          }
          else if (str2.Length > 68 && str2.Length < 74)
          {
            string county = str2.Substring(29, 25);
            zipCodeInfoList.Add(new ZipCodeInfo(str3, str2.Substring(7, 22).Trim(), county, str2.Substring(54, 7), str2.Substring(61), "", ""));
            stateInfoList.Add(new StateInfo(str2.Substring(0, 5), str2.Substring(7, 22).Trim(), county, str2.Substring(54, 7), str2.Substring(61), "", ""));
            if (!string.IsNullOrEmpty(county))
              ZipCodeUtils.county.Add(county.Trim());
          }
          else if (str2.Length > 74)
          {
            string county = str2.Substring(29, 25);
            string fips = str2.Substring(71, 5);
            string msacode = str2.Length > 76 ? str2.Substring(77) : string.Empty;
            zipCodeInfoList.Add(new ZipCodeInfo(str3, str2.Substring(7, 22).Trim(), county, str2.Substring(54, 7), str2.Substring(61, 9), fips, msacode));
            stateInfoList.Add(new StateInfo(str2.Substring(0, 5), str2.Substring(7, 22).Trim(), county, str2.Substring(54, 7), str2.Substring(61, 9), fips, msacode));
            if (!string.IsNullOrEmpty(county))
              ZipCodeUtils.county.Add(county.Trim());
            if (!ZipCodeUtils.tblFips.ContainsKey((object) (str3 + "|" + county.Trim())))
              ZipCodeUtils.tblFips.Add((object) (str3 + "|" + county.Trim()), (object) fips);
          }
        }
      }
      streamReader.Close();
    }

    public static string CapText(Match m)
    {
      string str = m.ToString();
      return char.IsLower(str[0]) ? char.ToUpper(str[0]).ToString() + str.Substring(1, str.Length - 1) : str;
    }

    public static bool IsValidCounty(string cnty) => ZipCodeUtils.county.Contains(cnty);

    public static ZipCodeInfo GetZipInfoAt(string zip)
    {
      if (zip.Length > 5)
        zip = zip.Substring(0, 5);
      if (!ZipCodeUtils.tbl.Contains((object) zip))
        return (ZipCodeInfo) null;
      List<ZipCodeInfo> zipCodeInfoList = (List<ZipCodeInfo>) ZipCodeUtils.tbl[(object) zip];
      string lower1 = zipCodeInfoList[0].City.Trim().ToLower();
      string lower2 = zipCodeInfoList[0].County.Trim().ToLower();
      string city = Regex.Replace(lower1, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
      string county = Regex.Replace(lower2, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
      return new ZipCodeInfo(zipCodeInfoList[0].State, city, county, zipCodeInfoList[0].Latitude, zipCodeInfoList[0].Longitude, zipCodeInfoList[0].Fips, zipCodeInfoList[0].MSACode);
    }

    public static string GetFIPS(string state, string county)
    {
      if (ZipCodeUtils.tblFips == null || ZipCodeUtils.tblFips.Count == 0)
        return (string) null;
      if (ZipCodeUtils.tblFips.ContainsKey((object) (state + "|" + county)))
        return ZipCodeUtils.tblFips[(object) (state + "|" + county)].ToString();
      if (!ZipCodeUtils.tblState.ContainsKey((object) state))
        return (string) null;
      List<StateInfo> stateInfoList = (List<StateInfo>) ZipCodeUtils.tblState[(object) state];
      if (stateInfoList == null || stateInfoList.Count == 0)
        return (string) null;
      for (int index = 0; index < stateInfoList.Count; ++index)
      {
        if (string.Compare(stateInfoList[index].County.Trim(), county, true) == 0)
          return stateInfoList[index].Fips;
      }
      return (string) null;
    }

    public static string GetMSACode(string state, string county)
    {
      if (!ZipCodeUtils.tblState.ContainsKey((object) state))
        return (string) null;
      List<StateInfo> stateInfoList = (List<StateInfo>) ZipCodeUtils.tblState[(object) state];
      if (stateInfoList == null || stateInfoList.Count == 0)
        return (string) null;
      for (int index = 0; index < stateInfoList.Count; ++index)
      {
        if (string.Compare(stateInfoList[index].County.Trim(), county, true) == 0)
          return stateInfoList[index].MSACode;
      }
      return (string) null;
    }

    public static StateInfo[] GetMultipleStateInfoAt(string state)
    {
      if (state.Length > 2 || !ZipCodeUtils.tblState.Contains((object) state))
        return (StateInfo[]) null;
      List<StateInfo> stateInfoList = (List<StateInfo>) ZipCodeUtils.tblState[(object) state];
      StateInfo[] multipleStateInfoAt = new StateInfo[stateInfoList.Count];
      for (int index = 0; index < stateInfoList.Count; ++index)
      {
        string lower1 = stateInfoList[index].City.Trim().ToLower();
        string lower2 = stateInfoList[index].County.Trim().ToLower();
        string city = Regex.Replace(lower1, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
        string county = Regex.Replace(lower2, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
        multipleStateInfoAt[index] = new StateInfo(stateInfoList[index].Zipcode, city, county, stateInfoList[index].Latitude, stateInfoList[index].Longitude, stateInfoList[index].Fips, stateInfoList[index].MSACode);
      }
      return multipleStateInfoAt;
    }

    public static string ConvertToFHACountyLimitName(string stateName, string countyName)
    {
      if (string.Compare(stateName, "AK", true) == 0)
      {
        if (string.Compare(countyName, "DILLINGHAM", true) == 0)
          return "DILLINGHAM CENS";
        if (string.Compare(countyName, "BETHEL", true) == 0)
          return "BETHEL CENSUS A";
        if (string.Compare(countyName, "KUSILVAK", true) == 0)
          return "KUSILVAK CENSUS";
        if (string.Compare(countyName, "YUKON KOYUKUK", true) == 0)
          return "YUKON-KOYUKUK C";
        if (string.Compare(countyName, "SKAGWAY", true) == 0)
          return "SKAGWAY MUNICIP";
        if (string.Compare(countyName, "PETERSBURG", true) == 0)
          return "PETERSBURG CENS";
        if (string.Compare(countyName, "WRANGELL", true) == 0)
          return "WRANGELL CITY A";
        if (string.Compare(countyName, "YAKUTAT", true) == 0)
          return "YAKUTAT CITY AN";
        if (string.Compare(countyName, "ANCHORAGE", true) == 0)
          return "ANCHORAGE MUNIC";
        if (string.Compare(countyName, "JUNEAU", true) == 0)
          return "JUNEAU CITY AND";
        if (string.Compare(countyName, "KODIAK ISLAND", true) == 0)
          return "KODIAK ISLAND B";
        if (string.Compare(countyName, "DENALI", true) == 0)
          return "DENALI BOROUGH";
        if (string.Compare(countyName, "WADE HAMPTON", true) == 0)
          return "WADE HAMPTON CE";
        if (string.Compare(countyName, "BRISTOL BAY", true) == 0)
          return "BRISTOL BAY BOR";
        if (string.Compare(countyName, "NOME", true) == 0)
          return "NOME CENSUS ARE";
        if (string.Compare(countyName, "NORTH SLOPE", true) == 0)
          return "NORTH SLOPE BOR";
        if (string.Compare(countyName, "HAINES", true) == 0)
          return "HAINES BOROUGH";
        if (string.Compare(countyName, "SITKA", true) == 0)
          return "SITKA CITY AND";
        if (string.Compare(countyName, "Copper River", true) == 0)
          return "COPPER RIVER CE";
        if (string.Compare(countyName, "Chugach", true) == 0)
          return "CHUGACH CENSUS";
      }
      else if (string.Compare(stateName, "VA", true) == 0)
      {
        if (string.Compare(countyName, "RADFORD", true) == 0)
          return "RADFORD CITY";
        if (string.Compare(countyName, "SALEM", true) == 0)
          return "SALEM CITY";
      }
      else if (string.Compare(stateName, "AS", true) == 0 && string.Compare(countyName, "AMERICAN SAMOA", true) == 0)
        return "EASTERN DISTRIC";
      if (countyName.Length > 15)
        countyName = countyName.Substring(0, 15).Trim();
      return countyName;
    }

    public static ZipCodeInfo[] GetMultipleZipInfoAt(string zip)
    {
      if (zip.Length > 5)
        zip = zip.Substring(0, 5);
      if (!ZipCodeUtils.tbl.Contains((object) zip))
        return (ZipCodeInfo[]) null;
      List<ZipCodeInfo> zipCodeInfoList1 = (List<ZipCodeInfo>) ZipCodeUtils.tbl[(object) zip];
      ZipCodeInfo[] collection = new ZipCodeInfo[zipCodeInfoList1.Count];
      ZipCodeInfo zipCodeInfo = (ZipCodeInfo) null;
      int index1 = -1;
      for (int index2 = 0; index2 < zipCodeInfoList1.Count; ++index2)
      {
        string lower1 = zipCodeInfoList1[index2].City.Trim().ToLower();
        string lower2 = zipCodeInfoList1[index2].County.Trim().ToLower();
        string city = Regex.Replace(lower1, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
        string str = Regex.Replace(lower2, "[(\\w)(-)]+", new MatchEvaluator(ZipCodeUtils.CapText));
        if (zip == "34266" && string.Compare(str, "De Soto", true) == 0)
          str = "DeSoto";
        collection[index2] = new ZipCodeInfo(zipCodeInfoList1[index2].State, city, str, zipCodeInfoList1[index2].Latitude, zipCodeInfoList1[index2].Longitude, zipCodeInfoList1[index2].Fips, zipCodeInfoList1[index2].MSACode);
        if (zip == "61373" && str == "La Salle" && city == "North Utica" && zipCodeInfo == null)
        {
          zipCodeInfo = new ZipCodeInfo(zipCodeInfoList1[index2].State, city, "LaSalle", zipCodeInfoList1[index2].Latitude, zipCodeInfoList1[index2].Longitude, zipCodeInfoList1[index2].Fips, zipCodeInfoList1[index2].MSACode);
          index1 = index2;
        }
      }
      if (zipCodeInfo == null)
        return collection;
      List<ZipCodeInfo> zipCodeInfoList2 = new List<ZipCodeInfo>((IEnumerable<ZipCodeInfo>) collection);
      zipCodeInfoList2.Insert(index1, zipCodeInfo);
      return zipCodeInfoList2.ToArray();
    }

    public static GeoCoordinate GetZipGeoCoordinate(
      string zip,
      string state,
      string city,
      string county)
    {
      if (string.IsNullOrEmpty(zip) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(city))
        return (GeoCoordinate) null;
      ZipCodeInfo[] multipleZipInfoAt = ZipCodeUtils.GetMultipleZipInfoAt(zip);
      if (multipleZipInfoAt == null || multipleZipInfoAt.Length == 0)
        return (GeoCoordinate) null;
      for (int index = 0; index < multipleZipInfoAt.Length; ++index)
      {
        if (string.Compare(multipleZipInfoAt[index].City, city, true) == 0 && string.Compare(multipleZipInfoAt[index].State, state, true) == 0 && (string.IsNullOrEmpty(county) || string.Compare(multipleZipInfoAt[index].County, county, true) == 0))
          return new GeoCoordinate(Utils.ParseDouble((object) multipleZipInfoAt[index].Latitude.Trim()), Utils.ParseDouble((object) multipleZipInfoAt[index].Longitude.Trim()));
      }
      return new GeoCoordinate(Utils.ParseDouble((object) multipleZipInfoAt[0].Latitude.Trim()), Utils.ParseDouble((object) multipleZipInfoAt[0].Longitude.Trim()));
    }

    public static bool IsValid(string zip)
    {
      Match match = new Regex("[0-9]{5}(-[0-9]{4})?").Match(zip);
      return match.Success && match.Length == zip.Length;
    }
  }
}
