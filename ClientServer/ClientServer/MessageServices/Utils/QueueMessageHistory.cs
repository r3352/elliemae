// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Utils.QueueMessageHistory
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Utils
{
  public class QueueMessageHistory
  {
    public string QueueName { get; set; }

    public string CorrelationId { get; set; }

    public string RefEntityId { get; set; }

    public string Category { get; set; }

    public string Source { get; set; }

    public string Type { get; set; }

    public string EventAction { get; set; }

    public string Message { get; set; }

    public string Reason { get; set; }

    public string Created { get; set; }
  }
}
