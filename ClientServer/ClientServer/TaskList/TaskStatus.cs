// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskList.TaskStatus
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.TaskList
{
  public enum TaskStatus
  {
    Invalid = -1, // 0xFFFFFFFF
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    WaitingOnSomeoneElse = 3,
    Deferred = 4,
  }
}
