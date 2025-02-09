// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.LoanFileSnapshotUtils
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  public static class LoanFileSnapshotUtils
  {
    public static bool TakeSnapshot(
      string encompassDataDir,
      string currentLoanFilePath,
      string targetLoanFileName,
      string targetLoanFolderPath)
    {
      int num = 0;
      bool flag;
      for (flag = File.Exists(currentLoanFilePath); !flag && num < 5; flag = File.Exists(currentLoanFilePath))
      {
        Thread.Sleep(100);
        ++num;
      }
      if (!flag)
        throw new Exception(string.Format("Current loan file {0} cannot be found, already tried {1} times", (object) currentLoanFilePath, (object) num));
      DateTime startTime = DateTime.Now;
      string str = Path.Combine(encompassDataDir, targetLoanFolderPath);
      Directory.CreateDirectory(str);
      string targetLoanFilePath = Path.Combine(str, targetLoanFileName);
      return LoanFileSnapshotCircuitBreaker.Instance.Execute((Action) (() =>
      {
        File.Copy(currentLoanFilePath, targetLoanFilePath);
        TimeSpan timeSpan = DateTime.Now - startTime;
        if (timeSpan.TotalSeconds <= 3.0)
          return;
        TraceLog.WriteWarning(nameof (LoanFileSnapshotUtils), string.Format("Long duration file write occurred on thread {0} with elapsed time = {1}s. File Path = {2}.", (object) Thread.CurrentThread.GetHashCode(), (object) timeSpan.TotalSeconds.ToString("0.00"), (object) targetLoanFilePath));
      }), currentLoanFilePath);
    }
  }
}
