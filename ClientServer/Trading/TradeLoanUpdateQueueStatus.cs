// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateQueueStatus
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public enum TradeLoanUpdateQueueStatus
  {
    None,
    [Description("Submitted")] InQueue,
    [Description("In Progress")] InProgress,
    [Description("Cancelled")] Cancelled,
    [Description("Completed")] Completed,
    [Description("Failed")] Failed,
    [Description("Cleared")] Cleared,
  }
}
