// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.ReaderLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class ReaderLock : IDisposable
  {
    private string name;
    private ReaderWriterLock innerLock;
    private ReaderWriterLockSlim innerLockSlim;

    public ReaderLock(string name, ReaderWriterLock innerLock, TimeSpan timeout)
    {
      this.name = name;
      this.innerLock = innerLock;
      EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(innerLock, timeout);
    }

    public ReaderLock(string name, ReaderWriterLockSlim innerLockSlim, TimeSpan timeout)
    {
      this.name = name;
      this.innerLockSlim = innerLockSlim;
      EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(innerLockSlim, timeout);
    }

    ~ReaderLock()
    {
      if (this.innerLock == null)
        return;
      string message = "Unreleased reader lock for object '" + this.name + "' detected!";
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (ReaderLock), message);
      if (ServerGlobals.TraceLog == null)
        return;
      ServerGlobals.TraceLog.WriteErrorI(nameof (ReaderLock), message);
    }

    public void Dispose()
    {
      if (this.innerLock != null)
      {
        try
        {
          if (this.innerLock.IsReaderLockHeld)
            EllieMae.EMLite.DataAccess.Threading.ReleaseReaderLock(this.innerLock);
        }
        catch (Exception ex)
        {
          if (ServerGlobals.TraceLog != null)
            ServerGlobals.TraceLog.WriteErrorI(nameof (ReaderLock), "Error releasing reader lock: " + ex.Message);
        }
        this.innerLock = (ReaderWriterLock) null;
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
