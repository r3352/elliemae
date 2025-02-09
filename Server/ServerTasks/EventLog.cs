// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.EventLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  public class EventLog
  {
    public int Id { get; set; }

    public string FilePath { get; set; }

    public string FileOwner { get; set; }

    public EventLogStatus Status { get; set; }

    public DateTime LastModified { get; set; }

    public long Position { get; set; }

    public bool IsComplete => this.Status == EventLogStatus.Complete;

    public bool IsDeleted => this.Status == EventLogStatus.Deleted;

    public static EventLogStatus UpdateStatus(string filePath)
    {
      string[] strArray = Path.GetFileNameWithoutExtension(filePath).Split('.');
      if (strArray.Length < 5)
        return EventLogStatus.Complete;
      DateTime now = DateTime.Now;
      if (now.ToString("yyyy-MM-dd-HH") == strArray[4])
        return EventLogStatus.InProgress;
      now = DateTime.Now;
      return now.Minute > 5 ? EventLogStatus.Complete : EventLogStatus.InProgress;
    }
  }
}
