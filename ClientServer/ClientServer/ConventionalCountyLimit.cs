// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ConventionalCountyLimit
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ConventionalCountyLimit
  {
    public ConventionalCountyLimit(
      int id,
      int limitYear,
      string fIPSStateCode,
      string fIPSCountyCode,
      string countyName,
      string stateCode,
      string cBSANumber,
      int limitFor1Unit,
      int limitFor2Units,
      int limitFor3Units,
      int limitFor4Units,
      DateTime lastModifiedDTTM)
    {
      this.ID = id;
      this.LimitYear = limitYear;
      this.FIPSStateCode = fIPSStateCode;
      this.FIPSCountyCode = fIPSCountyCode;
      this.CountyName = countyName;
      this.StateCode = stateCode;
      this.CBSANumber = cBSANumber;
      this.LimitFor1Unit = limitFor1Unit;
      this.LimitFor2Units = limitFor2Units;
      this.LimitFor3Units = limitFor3Units;
      this.LimitFor4Units = limitFor4Units;
      this.LastModifiedDateTime = lastModifiedDTTM;
    }

    public ConventionalCountyLimit()
    {
    }

    public int ID { get; set; }

    public int LimitYear { get; set; }

    public string FIPSStateCode { get; set; }

    public string FIPSCountyCode { get; set; }

    public string CountyName { get; set; }

    public string StateCode { get; set; }

    public string CBSANumber { get; set; }

    public int LimitFor1Unit { get; set; }

    public int LimitFor2Units { get; set; }

    public int LimitFor3Units { get; set; }

    public int LimitFor4Units { get; set; }

    public DateTime LastModifiedDateTime { get; set; }
  }
}
