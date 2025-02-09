// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ZipCodeInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class ZipCodeInfo
  {
    private string city;
    private string county;
    private string state;
    private string latitude;
    private string longitude;
    private string fips;
    private string msacode;

    public string City => this.city;

    public string County => this.county;

    public string State => this.state;

    public string Latitude => this.latitude;

    public string Longitude => this.longitude;

    public string Fips => this.fips;

    public string MSACode => this.msacode;

    public ZipCodeInfo(string state, string city, string county)
      : this(state, city, county, "", "", "", "")
    {
    }

    public ZipCodeInfo(
      string state,
      string city,
      string county,
      string latitude,
      string longitude,
      string fips,
      string msacode)
    {
      this.city = city;
      this.county = county;
      this.state = state;
      this.latitude = latitude;
      this.longitude = longitude;
      this.fips = fips;
      this.msacode = msacode;
    }
  }
}
