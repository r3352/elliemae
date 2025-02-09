// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.UserGroupDetails
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Serializable]
  public class UserGroupDetails
  {
    public AclGroup GroupInfo { get; set; }

    public OrgInGroup[] MemberOrganizations { get; set; }

    public UserGroupMemberUser[] MemberUsers { get; set; }

    public OrgInGroupLoan[] LoanOrganizations { get; set; }

    public UserGroupLoanUser[] LoanUsers { get; set; }

    public List<string> AccessibleFolders { get; set; }

    public UserGroupLoanTemplates LoanTemplates { get; set; }
  }
}
