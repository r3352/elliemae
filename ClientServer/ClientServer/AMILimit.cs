// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AMILimit
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AMILimit
  {
    public AMILimit(
      int id,
      int limitYear,
      string fIPSCode,
      string stateName,
      string countyName,
      string amiLimit100,
      string amiLimit80,
      string amiLimit50,
      DateTime lastModifiedDTTM)
    {
      this.ID = id;
      this.LimitYear = limitYear;
      this.FIPSCode = fIPSCode;
      this.StateName = stateName;
      this.CountyName = countyName;
      this.AmiLimit100 = amiLimit100;
      this.AmiLimit80 = amiLimit80;
      this.AmiLimit50 = amiLimit50;
      this.LastModifiedDateTime = lastModifiedDTTM;
    }

    public AMILimit()
    {
    }

    public int ID { get; set; }

    public int LimitYear { get; set; }

    public string FIPSCode { get; set; }

    public string StateName { get; set; }

    public string CountyName { get; set; }

    public string AmiLimit100 { get; set; }

    public string AmiLimit80 { get; set; }

    public string AmiLimit50 { get; set; }

    public DateTime LastModifiedDateTime { get; set; }
  }
}
