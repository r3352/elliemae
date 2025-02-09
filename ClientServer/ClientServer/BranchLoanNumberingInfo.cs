// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BranchLoanNumberingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BranchLoanNumberingInfo
  {
    private string orgCode;
    private bool enabled;
    private string nextNumber = "";

    public BranchLoanNumberingInfo(string orgCode, bool enabled, string nextNumber)
    {
      this.orgCode = orgCode;
      this.enabled = enabled;
      this.nextNumber = nextNumber;
    }

    public BranchLoanNumberingInfo(string orgCode) => this.orgCode = orgCode;

    public string OrgCode => this.orgCode;

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public string NextNumber
    {
      get => this.nextNumber;
      set => this.nextNumber = value;
    }
  }
}
