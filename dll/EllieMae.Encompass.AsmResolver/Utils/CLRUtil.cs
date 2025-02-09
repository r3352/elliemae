// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.CLRUtil
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class CLRUtil
  {
    public static bool HasAssemblyExt(string filePath)
    {
      string lower = (Path.GetExtension(filePath ?? "") ?? "").Trim().ToLower();
      return lower == ".dll" || lower == ".exe";
    }
  }
}
