// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.IProgressBar
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public interface IProgressBar
  {
    string Title { get; set; }

    int Minimum { get; set; }

    int Maximum { get; set; }

    int Value { get; set; }

    void PerformStep();

    void ShowProgressBar();

    void CloseProgressBar();
  }
}
