// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.InvestorServiceAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class InvestorServiceAclInfo
  {
    private string providerCompanyCode = "";
    private int featureID;
    private int personaID;
    private string userID;
    private AclResourceAccess personaAccess = AclResourceAccess.None;
    private AclResourceAccess customAccess = AclResourceAccess.None;
    private string investorCategory;

    public InvestorServiceAclInfo(
      string providerCompanyCode,
      int featureID,
      string investorCategory)
    {
      this.providerCompanyCode = providerCompanyCode;
      this.featureID = featureID;
      this.investorCategory = investorCategory;
    }

    public string UserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public int PersonaID
    {
      get => this.personaID;
      set => this.personaID = value;
    }

    public string ProviderCompanyCode
    {
      get => this.providerCompanyCode;
      set => this.providerCompanyCode = value;
    }

    public string InvestorCategory
    {
      get => this.investorCategory;
      set => this.investorCategory = value;
    }

    public int FeatureID => this.featureID;

    public bool Access
    {
      get
      {
        return this.customAccess != AclResourceAccess.None ? this.customAccess != AclResourceAccess.ReadOnly : this.personaAccess == AclResourceAccess.None || this.personaAccess != AclResourceAccess.ReadOnly;
      }
    }

    public AclResourceAccess PersonaAccess
    {
      get => this.personaAccess;
      set => this.personaAccess = value;
    }

    public AclResourceAccess CustomAccess
    {
      get => this.customAccess;
      set => this.customAccess = value;
    }

    public static List<InvestorServiceAclInfo> GetInvestorServicesList(
      string[] providerCompanyCodes,
      int featureID,
      string investorCategory)
    {
      return ((IEnumerable<string>) providerCompanyCodes).Select<string, InvestorServiceAclInfo>((Func<string, InvestorServiceAclInfo>) (pcCode => new InvestorServiceAclInfo(pcCode, featureID, investorCategory))).ToList<InvestorServiceAclInfo>();
    }

    public enum InvestorServicesDefaultSetting
    {
      None,
      Custom,
      All,
      NotSpecified,
    }
  }
}
