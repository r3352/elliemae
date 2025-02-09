// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Acl.AclCategoryAttribute
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Acl
{
  public class AclCategoryAttribute : Attribute
  {
    public string ContractName { get; private set; }

    public AclFeatureTypes Category { get; private set; }

    public AclFeature? ParentFeature { get; private set; }

    public AclCategoryAttribute(AclFeatureTypes category, string contractName)
    {
      this.ContractName = contractName;
      this.Category = category;
      this.ParentFeature = new AclFeature?();
    }

    public AclCategoryAttribute(
      AclFeatureTypes category,
      string contractName,
      AclFeature parentFeature)
    {
      this.ContractName = contractName;
      this.Category = category;
      this.ParentFeature = new AclFeature?(parentFeature);
    }
  }
}
