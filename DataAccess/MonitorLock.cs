// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.MonitorLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class MonitorLock : IDisposable
  {
    private string name;
    private object lockObject;

    public MonitorLock(string name, object lockObject, TimeSpan timeOut)
    {
      this.name = name;
      Threading.AcquireLock(name, lockObject, timeOut);
      this.lockObject = lockObject;
    }

    ~MonitorLock()
    {
      if (this.lockObject == null)
        return;
      ObjectLock currentLock = Threading.GetCurrentLock(this.lockObject);
      string str = "Unreleased lock for object '" + this.name + "' detected! ";
      string message = currentLock != null ? str + currentLock.ToString() : str + "No additional lock info found.";
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (MonitorLock), message);
      if (ServerGlobals.TraceLog == null)
        return;
      ServerGlobals.TraceLog.WriteErrorI(nameof (MonitorLock), message);
    }

    public void Dispose()
    {
      if (this.lockObject != null)
      {
        object lockObject = this.lockObject;
        this.lockObject = (object) null;
        Threading.ReleaseLock(lockObject);
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
