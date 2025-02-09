// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.IPipelineCacheStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public interface IPipelineCacheStore
  {
    void CreateSnapshotCursor(
      string cursorId,
      int maxCursors,
      int cursorIdleSeconds,
      int chunkSize);

    void CreateSnapshotCursorMetadata(
      string cursorId,
      string userIdentity,
      int totalCount,
      int entryTTLMinutes,
      int cursorIdleSeconds,
      object filter = null);

    SnapshotCursorMetadata GetSnapshotCursorMetadata(string cursorId);

    void StoreSnapshotCursorDataItems<T>(
      string cursorId,
      List<T> dataItems,
      int chunkSize,
      int entryTTLMinutes,
      int cursorIdleSeconds);

    List<T> RetrieveSnapshotCursorDataItems<T>(
      string cursorId,
      int startAt,
      int requestItems,
      int totalCount,
      int chunkSize);

    string[] GetAllCursors();

    void DeleteCursor(string cursorId);
  }
}
