// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.PgBarThreadStart
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using AppUpdtr;
using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Threading;

#nullable disable
namespace RestoreAppLauncher
{
  public class PgBarThreadStart : IDisposable
  {
    private Thread pbThread;
    private PgBarForm progressBar;
    private string fileToUpdate;
    private int minimum;
    private int maximum;
    private int value;
    private int step;
    private int sleep;
    private bool neverClose;

    public PgBarThreadStart(
      string fileToUpdate,
      int minimum,
      int maximum,
      int value,
      int step,
      int sleep,
      bool neverClose = false)
    {
      this.fileToUpdate = fileToUpdate;
      if (string.Compare(this.fileToUpdate, "EllieMae.Encompass.AsmResolver.dll", true) == 0)
        this.fileToUpdate = "AsmResolver.dll";
      this.minimum = minimum;
      this.maximum = maximum;
      this.value = value;
      this.step = step;
      this.sleep = sleep;
      this.neverClose = neverClose;
      this.pbThread = new Thread(new ThreadStart(this.progressBarThreadStart));
      this.pbThread.IsBackground = true;
      this.pbThread.Start();
    }

    private void progressBarThreadStart()
    {
      this.progressBar = new PgBarForm(Consts.EncompassSmartClient, this.fileToUpdate);
      this.progressBar.Minimum = this.minimum;
      this.progressBar.Maximum = this.maximum;
      this.progressBar.Value = this.value;
      this.progressBar.Step = this.step;
      this.progressBar.ShowProgressBar();
      while (this.progressBar.Value < this.progressBar.Maximum)
      {
        this.progressBar.PerformStep();
        Thread.Sleep(this.sleep);
        if (this.neverClose && this.progressBar.Value >= this.progressBar.Maximum)
          this.progressBar.Value = this.progressBar.Maximum - 4;
      }
      this.progressBar.CloseProgressBar();
    }

    public void Dispose()
    {
      try
      {
        if (this.pbThread != null)
        {
          this.pbThread.Abort();
          this.pbThread = (Thread) null;
        }
      }
      catch
      {
      }
      try
      {
        if (this.progressBar == null)
          return;
        this.progressBar.CloseProgressBar();
        this.progressBar = (PgBarForm) null;
      }
      catch
      {
      }
    }
  }
}
