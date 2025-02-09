// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.PreauthorizedModule
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [Serializable]
  public class PreauthorizedModule
  {
    private string authKey;
    private PreauthorizedModuleType moduleType;
    private string description;

    public PreauthorizedModule(
      string authKey,
      PreauthorizedModuleType moduleType,
      string description)
    {
      this.authKey = authKey;
      this.moduleType = moduleType;
      this.description = description;
    }

    public string AuthorizationKey => this.authKey;

    public PreauthorizedModuleType ModuleType => this.moduleType;

    public string Description => this.description;

    public override string ToString() => this.description;
  }
}
