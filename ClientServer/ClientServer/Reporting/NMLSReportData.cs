// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.NMLSReportData
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class NMLSReportData
  {
    public string StateCode;
    public int ReportYear;
    public int ReportQuarter;
    public string ApplicationSource;
    public Decimal LoanAmount;
    public int LoanCount;

    public NMLSReportData(
      string stateCode,
      int reportYear,
      int reportQuarter,
      string appSource,
      Decimal loanAmount,
      int loanCount)
    {
      this.StateCode = stateCode;
      this.ReportYear = reportYear;
      this.ReportQuarter = reportQuarter;
      this.ApplicationSource = appSource;
      this.LoanAmount = loanAmount;
      this.LoanCount = loanCount;
    }
  }
}
