// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.FileMutex
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class FileMutex : IDisposable
  {
    private Mutex mutex;

    public FileMutex(string filePath)
    {
      bool createdNew;
      this.mutex = new Mutex(true, Path.GetFileName(filePath), out createdNew);
      if (createdNew)
        return;
      this.mutex.WaitOne();
    }

    public void Release()
    {
      if (this.mutex == null)
        return;
      this.mutex.ReleaseMutex();
    }

    public void Dispose() => this.Release();
  }
}
