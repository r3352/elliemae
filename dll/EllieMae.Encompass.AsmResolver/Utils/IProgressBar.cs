// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.IProgressBar
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

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
