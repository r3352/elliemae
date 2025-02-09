// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.EncompassSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Provides access to basic configuration information for the current Encompass installation.
  /// </summary>
  public class EncompassSettings : IEncompassSettings
  {
    /// <summary>Gets the path of the local EncompassData folder.</summary>
    public string EncompassDataDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassDataDirectory;
    }

    /// <summary>
    /// Gets the path of the local Encompass application folder.
    /// </summary>
    public string EncompassProgramDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
    }
  }
}
