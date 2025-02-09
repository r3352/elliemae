// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BatchJobPipelineFilter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BatchJobPipelineFilter
  {
    public int[] TradeIds { get; set; }

    public string TradeName { get; set; }

    public string TradeType { get; set; }

    public DateTime? CreatedDateStart { get; set; }

    public DateTime? CreatedDateEnd { get; set; }

    public string CreatedBy { get; set; }

    public string[] Status { get; set; }
  }
}
