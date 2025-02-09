// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.SmartClient.SmartClientInitializer
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.AsmResolver;

#nullable disable
namespace EllieMae.Encompass.SmartClient
{
  /// <summary>
  /// Provides internal functionality to support the Encompass SmartClient deployment model.
  /// </summary>
  /// <remarks>This class is for internal use only.</remarks>
  /// <exclude />
  public class SmartClientInitializer
  {
    /// <summary>Init</summary>
    public static void Init() => SmartClientInitializer.Init(false);

    /// <summary>Init</summary>
    /// <param name="displayMOTDMessage"></param>
    public static void Init(bool displayMOTDMessage)
    {
      if (!(AssemblyResolver.IsSmartClient & displayMOTDMessage))
        return;
      AssemblyResolver.DisplayMOTDMessage();
    }
  }
}
