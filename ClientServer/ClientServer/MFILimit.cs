// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MFILimit
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MFILimit
  {
    public MFILimit(
      int id,
      int sourceFileYear,
      string msamdCode,
      string msamdName,
      int actualMFIYear,
      string actualMFIAmount,
      int estimatedMFIYear,
      string estimatedMFIAmount,
      DateTime lastModifiedDTTM)
    {
      this.ID = id;
      this.SourceFileYear = sourceFileYear;
      this.MSAMDCode = msamdCode;
      this.MSAMDName = msamdName;
      this.ActualMFIYear = actualMFIYear;
      this.ActualMFIAmount = actualMFIAmount;
      this.EstimatedMFIYear = estimatedMFIYear;
      this.EstimatedMFIAmount = estimatedMFIAmount;
      this.LastModifiedDateTime = lastModifiedDTTM;
    }

    public MFILimit()
    {
    }

    public int ID { get; set; }

    public int SourceFileYear { get; set; }

    public string MSAMDCode { get; set; }

    public string MSAMDName { get; set; }

    public int ActualMFIYear { get; set; }

    public string ActualMFIAmount { get; set; }

    public int EstimatedMFIYear { get; set; }

    public string EstimatedMFIAmount { get; set; }

    public DateTime LastModifiedDateTime { get; set; }
  }
}
