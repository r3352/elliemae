// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.IFileSystemTranslator
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public interface IFileSystemTranslator
  {
    string GetBasePath();

    string GetCurrentPath();

    bool Synchronized(
      IFileSystemTranslator source,
      Dictionary<string, ConflictResolveMethod> conflictResolves);

    List<string> CheckConflict(IFileSystemTranslator source);

    List<string> GetFileSystemEntries();
  }
}
