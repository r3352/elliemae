// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Threading.ReaderWriterTracker
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common.Threading
{
  public class ReaderWriterTracker : IDisposable
  {
    private ReaderWriterLock rwLock;
    private ReaderWriterLockSlim rwLockSlim;
    private bool writer;

    public ReaderWriterTracker(ReaderWriterLock rwLock, bool writer)
    {
      this.rwLock = rwLock;
      this.writer = writer;
    }

    public ReaderWriterTracker(ReaderWriterLockSlim rwLockSlim, bool writer)
    {
      this.rwLockSlim = rwLockSlim;
      this.writer = writer;
    }

    public void Dispose()
    {
      if (this.rwLock != null)
      {
        if (this.writer)
          this.rwLock.ReleaseWriterLock();
        else
          this.rwLock.ReleaseReaderLock();
        this.rwLock = (ReaderWriterLock) null;
      }
      if (this.rwLockSlim == null)
        return;
      if (this.writer)
      {
        if (this.rwLockSlim.IsWriteLockHeld)
          this.rwLockSlim.ExitWriteLock();
      }
      else if (this.rwLockSlim.IsReadLockHeld)
        this.rwLockSlim.ExitReadLock();
      this.rwLockSlim = (ReaderWriterLockSlim) null;
    }
  }
}
