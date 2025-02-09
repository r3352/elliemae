// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.AsyncTargetQueueDefaults
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class AsyncTargetQueueDefaults
  {
    public const int DefaultPayloadSize = 20;
    public const int SingleEntryPayload = 1;
    public const int MaxQueueSize = 500;
    public const int DefaultWriteInterval = 30000;
    public const int InfiniteQueueSize = -1;
    public const int NoWaitWrite = -1;
  }
}
