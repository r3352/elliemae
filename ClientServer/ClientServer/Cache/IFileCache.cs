// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.IFileCache
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public interface IFileCache : IDisposable
  {
    string GetFilePath(string fileName);

    bool Exists(string fileName);

    BinaryObject Get(string fileName);

    void Put(string fileName, BinaryObject data);

    void Put(string fileName, BinaryObject data, DateTime lastModificationTime);

    void Delete(string fileName);

    DateTime GetLastModificationDate(string fileName);

    Version GetFileVersion(string fileName);

    void CopyOut(string fileName, string targetFile);
  }
}
