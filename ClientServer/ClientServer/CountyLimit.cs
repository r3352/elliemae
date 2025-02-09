// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CountyLimit
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class CountyLimit
  {
    private int id;
    private int msaCode;
    private int metropolitanDivisionCode;
    private string msaName;
    private string soaCode;
    private string limitType;
    private int medianPrice;
    private int limitFor1Unit;
    private int limitFor2Units;
    private int limitFor3Units;
    private int limitFor4Units;
    private string stateAbbreviation;
    private int countyCode;
    private string stateName;
    private string countyName;
    private string countyTransactionDate;
    private string limitTransactionDate;
    private DateTime lastModifiedDTTM;
    private bool customized;

    public CountyLimit(
      int id,
      int msaCode,
      int metropolitanDivisionCode,
      string msaName,
      string soaCode,
      string limitType,
      int medianPrice,
      int limitFor1Unit,
      int limitFor2Units,
      int limitFor3Units,
      int limitFor4Units,
      string stateAbbreviation,
      int countyCode,
      string stateName,
      string countyName,
      string countyTransactionDate,
      string limitTransactionDate,
      DateTime lastModifiedDTTM,
      bool customized)
    {
      this.ID = id;
      this.MsaCode = msaCode;
      this.MetropolitanDivisionCode = metropolitanDivisionCode;
      this.MsaName = msaName;
      this.SoaCode = soaCode;
      this.LimitType = limitType;
      this.MedianPrice = medianPrice;
      this.LimitFor1Unit = limitFor1Unit;
      this.LimitFor2Units = limitFor2Units;
      this.LimitFor3Units = limitFor3Units;
      this.LimitFor4Units = limitFor4Units;
      this.StateAbbreviation = stateAbbreviation;
      this.CountyCode = countyCode;
      this.StateName = stateName;
      this.CountyName = countyName;
      this.CountyTransactionDate = countyTransactionDate;
      this.LimitTransactionDate = limitTransactionDate;
      this.lastModifiedDTTM = lastModifiedDTTM;
      this.customized = customized;
    }

    public CountyLimit()
    {
    }

    public int ID
    {
      get => this.id;
      set => this.id = value;
    }

    public int MsaCode
    {
      get => this.msaCode;
      set => this.msaCode = value;
    }

    public int MetropolitanDivisionCode
    {
      get => this.metropolitanDivisionCode;
      set => this.metropolitanDivisionCode = value;
    }

    public string MsaName
    {
      get => this.msaName;
      set => this.msaName = value;
    }

    public string SoaCode
    {
      get => this.soaCode;
      set => this.soaCode = value;
    }

    public string LimitType
    {
      get => this.limitType;
      set => this.limitType = value;
    }

    public int MedianPrice
    {
      get => this.medianPrice;
      set => this.medianPrice = value;
    }

    public int LimitFor1Unit
    {
      get => this.limitFor1Unit;
      set => this.limitFor1Unit = value;
    }

    public int LimitFor2Units
    {
      get => this.limitFor2Units;
      set => this.limitFor2Units = value;
    }

    public int LimitFor3Units
    {
      get => this.limitFor3Units;
      set => this.limitFor3Units = value;
    }

    public int LimitFor4Units
    {
      get => this.limitFor4Units;
      set => this.limitFor4Units = value;
    }

    public string StateAbbreviation
    {
      get => this.stateAbbreviation;
      set => this.stateAbbreviation = value;
    }

    public int CountyCode
    {
      get => this.countyCode;
      set => this.countyCode = value;
    }

    public string StateName
    {
      get => this.stateName;
      set => this.stateName = value;
    }

    public string CountyName
    {
      get => this.countyName;
      set => this.countyName = value;
    }

    public string CountyTransactionDate
    {
      get => this.countyTransactionDate;
      set => this.countyTransactionDate = value;
    }

    public string LimitTransactionDate
    {
      get => this.limitTransactionDate;
      set => this.limitTransactionDate = value;
    }

    public DateTime LastModifiedDateTime
    {
      get => this.lastModifiedDTTM;
      set => this.lastModifiedDTTM = value;
    }

    public bool Customized
    {
      get => this.customized;
      set => this.customized = value;
    }
  }
}
