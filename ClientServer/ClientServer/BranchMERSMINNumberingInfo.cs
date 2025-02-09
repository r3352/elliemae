// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BranchMERSMINNumberingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BranchMERSMINNumberingInfo
  {
    private string mersminCode;
    private bool enabled;
    private string nextNumber = "0000000001";

    public BranchMERSMINNumberingInfo(string mersminCode, bool enabled, string nextNumber)
    {
      this.mersminCode = mersminCode;
      this.enabled = enabled;
      this.nextNumber = nextNumber;
    }

    public BranchMERSMINNumberingInfo(string mersminCode) => this.mersminCode = mersminCode;

    public string MERSMINCode => this.mersminCode;

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
