// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DocumentDefaultAccessRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DocumentDefaultAccessRuleInfo
  {
    private int roleAddedBy;
    private IntegerList rolesAllowedAccess;

    public DocumentDefaultAccessRuleInfo(int roleAddedBy)
    {
      this.roleAddedBy = roleAddedBy;
      this.rolesAllowedAccess = new IntegerList(true);
    }

    public DocumentDefaultAccessRuleInfo(int roleAddedBy, int[] rolesAllowedAccess)
    {
      this.roleAddedBy = roleAddedBy;
      this.rolesAllowedAccess = new IntegerList(true, (IList) rolesAllowedAccess);
    }

    public int RoleAddedBy => this.roleAddedBy;

    public IntegerList RolesAllowedAccess => this.rolesAllowedAccess;

    public DocumentDefaultAccessRuleInfo Clone()
    {
      return new DocumentDefaultAccessRuleInfo(this.roleAddedBy, this.rolesAllowedAccess.ToArray());
    }
  }
}
