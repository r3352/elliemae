// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.ITemplateSettingService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface ITemplateSettingService : IContextBoundObject
  {
    List<FileSystemEntry> GetAllEntries(
      UserInfo userInfo,
      FileSystemEntry folder,
      TemplateSettingsType templateSettingsType,
      FileSystemEntry.Types fileSystemEntryType);

    BinaryObject GetObject(
      string templatePath,
      string userId,
      TemplateSettingsType templateSettingsType);

    void SaveObject(TemplateSettingsType type, FileSystemEntry entry, BinaryObject data);

    void DeleteObject(TemplateSettingsType type, FileSystemEntry fileEntry);

    bool ObjectExistsOfAnyType(TemplateSettingsType type, FileSystemEntry fileEntry);
  }
}
