// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.EntityContractBase
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Namespace = "http://www.elliemae.com/encompass/platform")]
  public abstract class EntityContractBase
  {
    private ISet<string> ModifiedProperties { get; set; }

    public void PropertyChanged(string property)
    {
      if (this.ModifiedProperties == null)
        this.ModifiedProperties = (ISet<string>) new HashSet<string>();
      this.ModifiedProperties.Add(property);
    }

    public bool Modified(string property)
    {
      return this.ModifiedProperties != null && this.ModifiedProperties.Contains(property);
    }

    public string EntityId { get; set; }

    public bool? IsEntityDeleted { get; set; }
  }
}
