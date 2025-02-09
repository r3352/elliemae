// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ZipcodeUserDefinedList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ZipcodeUserDefinedList
  {
    private List<ZipcodeInfoUserDefined> userDefinedList;
    private Hashtable userDefinedLookupTable;

    public ZipcodeUserDefinedList() => this.Clear();

    public void Clear()
    {
      this.userDefinedList = new List<ZipcodeInfoUserDefined>();
      this.userDefinedLookupTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
    }

    public void Add(ZipcodeInfoUserDefined zipcodeRecord)
    {
      this.userDefinedList.Add(zipcodeRecord);
      if (!this.userDefinedLookupTable.ContainsKey((object) zipcodeRecord.BuildKey()))
        this.userDefinedLookupTable.Add((object) zipcodeRecord.BuildKey(), (object) null);
      this.userDefinedLookupTable[(object) zipcodeRecord.BuildKey()] = (object) zipcodeRecord;
    }

    public int Count => this.userDefinedList.Count;

    public ZipcodeInfoUserDefined GetItemAt(int i)
    {
      return i >= this.userDefinedList.Count ? (ZipcodeInfoUserDefined) null : this.userDefinedList[i];
    }

    public ZipCodeInfo[] GetZipcodeInfo(string zipcode)
    {
      ZipcodeInfoUserDefined[] zipcodeUserDefineds = this.GetZipcodeUserDefineds(zipcode);
      List<ZipCodeInfo> zipCodeInfoList = new List<ZipCodeInfo>();
      if (zipcodeUserDefineds == null || zipcodeUserDefineds.Length == 0)
        return (ZipCodeInfo[]) null;
      for (int index = 0; index < zipcodeUserDefineds.Length; ++index)
        zipCodeInfoList.Add(zipcodeUserDefineds[index].ZipInfo);
      return zipCodeInfoList.ToArray();
    }

    public ZipcodeInfoUserDefined[] GetZipcodeUserDefineds(string zipcode)
    {
      string empty = string.Empty;
      if ((zipcode ?? "") != string.Empty && zipcode.IndexOf("-") > -1)
      {
        string[] strArray = zipcode.Split('-');
        zipcode = strArray[0];
        empty = strArray[1];
      }
      return this.GetZipcodeUserDefineds(zipcode, empty);
    }

    public ZipcodeInfoUserDefined[] GetZipcodeUserDefineds(string zipcode, string zipcodeExtension)
    {
      if ((zipcode ?? "") == string.Empty)
        return this.userDefinedList.ToArray();
      List<ZipcodeInfoUserDefined> zipcodeInfoUserDefinedList = new List<ZipcodeInfoUserDefined>();
      for (int index = 0; index < this.userDefinedList.Count; ++index)
      {
        if (zipcodeExtension != string.Empty && zipcodeExtension == this.userDefinedList[index].ZipcodeExtension && zipcode == this.userDefinedList[index].Zipcode)
          zipcodeInfoUserDefinedList.Add(this.userDefinedList[index]);
        else if (this.userDefinedList[index].Zipcode == zipcode)
          zipcodeInfoUserDefinedList.Add(this.userDefinedList[index]);
      }
      return zipcodeInfoUserDefinedList.ToArray();
    }

    public bool IsZipcodeUserDefinedDuplicated(
      string zipcode,
      string zipcodeExt,
      string state,
      string city)
    {
      return this.userDefinedLookupTable.ContainsKey((object) new ZipcodeInfoUserDefined(zipcode, zipcodeExt, city, state, string.Empty).BuildKey());
    }
  }
}
