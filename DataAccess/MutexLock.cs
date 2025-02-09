// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.MutexLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class MutexLock : IDisposable
  {
    private string name;
    private IDisposable lockObject;

    public MutexLock(string name, IDisposable lockObject)
    {
      this.name = name;
      this.lockObject = lockObject;
    }

    ~MutexLock()
    {
      if (this.lockObject != null)
      {
        string message = "Unreleased mutex for object '" + this.name + "' detected!";
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (MutexLock), message);
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (MutexLock), message);
      }
      this.dispose(false);
    }

    public void Dispose() => this.dispose(true);

    protected void dispose(bool disposing)
    {
      if (this.lockObject != null)
      {
        this.lockObject.Dispose();
        this.lockObject = (IDisposable) null;
      }
      if (!disposing)
        return;
      GC.SuppressFinalize((object) this);
    }
  }
}
