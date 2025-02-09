// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSettingsManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>ExternalSettingsManager Class</summary>
  public class ExternalSettingsManager : SessionBoundObject, IExternalSettingsManager
  {
    private IConfigurationManager mngr;
    private ExternalFeesList fees;

    /// <summary>Internal Constructor</summary>
    /// <param name="session"></param>
    public ExternalSettingsManager(Session session)
      : base(session)
    {
      this.mngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    /// <summary>Returns a list of Global Fees</summary>
    /// <returns></returns>
    public ExternalFeesList GetAllGlobalFees()
    {
      if (this.fees == null)
        this.fees = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      return this.fees;
    }

    /// <summary>Returns a list of Global Fees By Channel</summary>
    /// <param name="channel"></param>
    /// <returns></returns>
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

    /// <summary>Returns a list of Global Fees By Status</summary>
    /// <param name="status"></param>
    /// <returns></returns>
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

    /// <summary>Returns the deetings for late fees</summary>
    /// <returns></returns>
    public LateFeeSettings GetGlobalLateFeeSettings()
    {
      return new LateFeeSettings(this.mngr.GetGlobalLateFeeSettings());
    }
  }
}
