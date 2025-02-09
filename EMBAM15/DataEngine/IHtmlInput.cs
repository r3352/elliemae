// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.IHtmlInput
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public interface IHtmlInput
  {
    FieldFormat GetFormat(string id);

    FieldDefinition GetFieldDefinition(string id);

    string GetField(string id);

    string GetSimpleField(string id);

    string GetOrgField(string id);

    void SetField(string id, string val);

    void SetField(string id, string val, bool isUserModified);

    bool IsLocked(string id);

    void RemoveLock(string id);

    void AddLock(string id);

    void SetCurrentField(string id, string val);

    bool IsDirty(string id);

    void ClearDirtyTable();

    void CleanField(string id);
  }
}
