// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EventLogID
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class EventLogID
  {
    public const int ServerStart = 900;
    public const int ServerStop = 901;
    public const int ServerError = 902;
    public const int LogWriteFailure = 1100;
    public const int LogDisconnect = 1101;
    public const int LogReconnect = 1102;
    public const int PipelineRebuildAll = 2000;
    public const int PipelineRebuildFolder = 2001;
    public const int PipelineRebuildLoan = 2002;
    public const int PipelineReindex = 2003;
    public const int PipelineRebuildRDB = 2004;
  }
}
