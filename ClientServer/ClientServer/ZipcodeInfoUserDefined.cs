// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ZipcodeInfoUserDefined
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ZipcodeInfoUserDefined
  {
    private string zipcode = string.Empty;
    private string zipcodeExtension = string.Empty;
    private ZipCodeInfo zipInfo;

    public ZipcodeInfoUserDefined(
      string zipcode,
      string zipcodeExtension,
      string city,
      string state,
      string county)
      : this(zipcode, zipcodeExtension, new ZipCodeInfo(state, city, county))
    {
    }

    public ZipcodeInfoUserDefined(string zipcode, string zipcodeExtension, ZipCodeInfo zipInfo)
    {
      this.zipcode = zipcode;
      this.zipcodeExtension = zipcodeExtension;
      this.zipInfo = zipInfo;
    }

    public string Zipcode => this.zipcode;

    public string ZipcodeExtension => this.zipcodeExtension;

    public ZipCodeInfo ZipInfo => this.zipInfo;

    public string BuildKey()
    {
      return this.zipcode + "-" + this.zipcodeExtension + "-" + this.zipInfo.State.ToUpper() + "-" + this.zipInfo.City.ToUpper();
    }
  }
}
