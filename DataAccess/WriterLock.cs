// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.WriterLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class WriterLock : IDisposable
  {
    private string name;
    private ReaderWriterLock innerLock;
    private ReaderWriterLockSlim innerLockSlim;

    public WriterLock(string name, ReaderWriterLock innerLock, TimeSpan timeout)
    {
      this.name = name;
      EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(innerLock, timeout);
      this.innerLock = innerLock;
    }

    public WriterLock(string name, ReaderWriterLockSlim innerLockSlim, TimeSpan timeout)
    {
      this.name = name;
      EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(innerLockSlim, timeout);
      this.innerLockSlim = innerLockSlim;
    }

    ~WriterLock()
    {
      if (this.innerLock == null && this.innerLockSlim == null)
        return;
      string message = "Unreleased writer lock for object '" + this.name + "' detected!";
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (WriterLock), message);
      if (ServerGlobals.TraceLog == null)
        return;
      ServerGlobals.TraceLog.WriteErrorI(nameof (WriterLock), message);
    }

    public void Dispose()
    {
      if (this.innerLock != null)
      {
        try
        {
          if (this.innerLock.IsWriterLockHeld)
            EllieMae.EMLite.DataAccess.Threading.ReleaseWriterLock(this.innerLock);
        }
        catch (Exception ex)
        {
          if (ServerGlobals.TraceLog != null)
            ServerGlobals.TraceLog.WriteErrorI(nameof (WriterLock), "Error releasing writer lock: " + ex.Message);
        }
        this.innerLock = (ReaderWriterLock) null;
      }
      if (this.innerLockSlim != null)
      {
        try
        {
          EllieMae.EMLite.DataAccess.Threading.ReleaseWriterLock(this.innerLockSlim);
        }
        catch (Exception ex)
        {
          if (ServerGlobals.TraceLog != null)
            ServerGlobals.TraceLog.WriteErrorI(nameof (WriterLock), "Error releasing writer lock: " + ex.Message);
        }
        this.innerLockSlim = (ReaderWriterLockSlim) null;
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
