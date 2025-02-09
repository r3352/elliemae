// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.ToolsAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Serializable]
  public class ToolsAclInfo
  {
    private int featureID;
    private string milestoneID;
    private int roleID;
    private int access;
    private bool customAccess;
    private string roleName = "";

    public ToolsAclInfo(
      int featureID,
      int roleID,
      int access,
      string roleName,
      string milestoneID)
    {
      this.featureID = featureID;
      this.roleID = roleID;
      this.access = access;
      this.roleName = roleName;
      this.milestoneID = milestoneID;
    }

    public int FeatureID
    {
      get => this.featureID;
      set => this.featureID = value;
    }

    public int RoleID
    {
      get => this.roleID;
      set => this.roleID = value;
    }

    public int Access
    {
      get => this.access;
      set => this.access = value;
    }

    public bool CustomAccess
    {
      get => this.customAccess;
      set => this.customAccess = value;
    }

    public string RoleName
    {
      get => this.roleName;
      set => this.roleName = value;
    }

    public string MilestoneID
    {
      get => this.milestoneID;
      set => this.milestoneID = value;
    }
  }
}
