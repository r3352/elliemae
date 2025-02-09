// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.SmartClient.SmartClientInitializer
// Assembly: EllieMae.Encompass.SmartClient, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 741C20F1-D2E4-4F74-A191-33694A9D6E88
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EllieMae.Encompass.SmartClient.dll

using EllieMae.Encompass.AsmResolver;

#nullable disable
namespace EllieMae.Encompass.SmartClient
{
  public class SmartClientInitializer
  {
    public static void Initialize(bool displayMOTDMessage)
    {
      if (!(AssemblyResolver.IsSmartClient & displayMOTDMessage))
        return;
      AssemblyResolver.DisplayMOTDMessage();
    }
  }
}
