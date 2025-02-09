// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.IDiagConfigChangeHandler`1
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public interface IDiagConfigChangeHandler<TConfig> : IDisposable
  {
    bool PerformsGlobalCleanup { get; }

    void GlobalCleanup(TConfig config);

    bool PerformsCleanup { get; }

    void Cleanup(TConfig config);

    bool PerformsGlobalSetup { get; }

    void GlobalSetup(TConfig config, bool refresh);

    bool PerformsSetup { get; }

    void Setup(TConfig config, bool refresh);
  }
}
