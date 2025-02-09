// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSettingsManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalSettingsManager : SessionBoundObject, IExternalSettingsManager
  {
    private IConfigurationManager mngr;
    private ExternalFeesList fees;

    public ExternalSettingsManager(Session session)
      : base(session)
    {
      this.mngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    public ExternalFeesList GetAllGlobalFees()
    {
      if (this.fees == null)
        this.fees = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      return this.fees;
    }

    public ExternalFeesList GetGlobalFeesByChannel(ExternalOrganizationEntityType channel)
    {
      if (this.fees == null)
        this.fees = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      if (channel == ExternalOrganizationEntityType.Both)
        return this.fees;
      ExternalFeesList globalFeesByChannel = new ExternalFeesList();
      foreach (ExternalFees fee in (CollectionBase) this.fees)
      {
        if (fee.Channel == channel)
          globalFeesByChannel.Add(fee);
      }
      return globalFeesByChannel;
    }

    public ExternalFeesList GetGlobalFeesByStatus(ExternalOriginatorFeeStatus status)
    {
      if (this.fees == null)
        this.fees = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      ExternalFeesList globalFeesByStatus = new ExternalFeesList();
      foreach (ExternalFees fee in (CollectionBase) this.fees)
      {
        if (fee.Status == status)
          globalFeesByStatus.Add(fee);
      }
      return globalFeesByStatus;
    }

    public LateFeeSettings GetGlobalLateFeeSettings()
    {
      return new LateFeeSettings(this.mngr.GetGlobalLateFeeSettings());
    }
  }
}
