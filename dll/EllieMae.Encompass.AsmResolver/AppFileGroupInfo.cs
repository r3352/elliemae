// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AppFileGroupInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class AppFileGroupInfo
  {
    public readonly string Name;
    public readonly string Codebase;
    public readonly long Size = -1;

    public AppFileGroupInfo(string name, string codebase, long size)
    {
      this.Name = name;
      this.Codebase = codebase;
      this.Size = size;
    }
  }
}
