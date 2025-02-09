// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.TransferProgressEventArgs
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class TransferProgressEventArgs
  {
    private int percentCompleted;
    private bool cancel;

    public TransferProgressEventArgs(int percentCompleted)
    {
      this.percentCompleted = percentCompleted;
    }

    public TransferProgressEventArgs(DownloadProgressEventArgs args)
    {
      this.percentCompleted = Convert.ToInt32(args.PercentComplete * 100f);
    }

    public int PercentCompleted => this.percentCompleted;

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
