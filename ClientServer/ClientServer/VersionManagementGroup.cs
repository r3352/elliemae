// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.VersionManagementGroup
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Version;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class VersionManagementGroup
  {
    private int groupId;
    private string groupName;
    private bool isDefault;
    private ClientAppVersion authVersion;
    private string[] groupUserIds;

    public VersionManagementGroup(string groupName)
    {
      this.groupId = -1;
      this.isDefault = false;
      this.groupName = groupName;
      this.authVersion = (ClientAppVersion) null;
      this.groupUserIds = (string[]) null;
    }

    public VersionManagementGroup(
      int groupId,
      string groupName,
      bool isDefault,
      ClientAppVersion authVersion,
      string[] groupUserIds)
    {
      this.groupId = groupId;
      this.groupName = groupName;
      this.isDefault = isDefault;
      this.authVersion = authVersion;
      this.groupUserIds = groupUserIds;
    }

    public int GroupID => this.groupId;

    public bool IsDefaultGroup => this.isDefault;

    public string GroupName
    {
      get => this.groupName;
      set => this.groupName = value;
    }

    public ClientAppVersion AuthorizedVersion
    {
      get => this.authVersion;
      set => this.authVersion = value;
    }

    public string[] GroupUserIDs
    {
      get => this.groupUserIds;
      set => this.groupUserIds = value;
    }
  }
}
