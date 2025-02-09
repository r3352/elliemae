// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.SmartClient.SmartClientInitializer
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.AsmResolver;

#nullable disable
namespace EllieMae.Encompass.SmartClient
{
  public class SmartClientInitializer
  {
    public static void Init() => SmartClientInitializer.Init(false);

    public static void Init(bool displayMOTDMessage)
    {
      if (!(AssemblyResolver.IsSmartClient & displayMOTDMessage))
        return;
      AssemblyResolver.DisplayMOTDMessage();
    }
  }
}
