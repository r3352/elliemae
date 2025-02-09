// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Disposable
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;

#nullable disable
namespace Elli.Service.Common
{
  public abstract class Disposable : IDisposable
  {
    private bool _isDisposed;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public bool IsDisposed => this._isDisposed;

    protected void Dispose(bool disposing)
    {
      if (this.IsDisposed)
        return;
      if (disposing)
        this.DisposeManagedResources();
      this.DisposeUnmanagedResources();
      this._isDisposed = true;
    }

    protected void ThrowExceptionIfDisposed()
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException(this.GetType().FullName);
    }

    protected abstract void DisposeManagedResources();

    protected virtual void DisposeUnmanagedResources()
    {
    }
  }
}
