// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.SecondarySettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>Provides access to Secondary-related system settings</summary>
  public class SecondarySettings : SessionBoundObject
  {
    private LockExtensionSettings lockExtensions;
    private InvestorTemplates investorTemplates;
    private TradePriceAdjustmentTemplates tradePriceAdjustmentTemplates;
    private SRPTableTemplates sRPTableTemplates;
    private Session session;

    internal SecondarySettings(Session session)
      : base(session)
    {
      this.session = session;
      this.lockExtensions = new LockExtensionSettings(session);
    }

    /// <summary>
    /// Gets the accessor for lock extension-related settings in Encompass
    /// </summary>
    public LockExtensionSettings LockExtensions => this.lockExtensions;

    /// <summary>
    /// Gets access to all the Investor Templates configured in Encompass settings.
    /// </summary>
    public InvestorTemplates InvestorTemplates
    {
      get
      {
        if (this.investorTemplates == null)
          this.investorTemplates = new InvestorTemplates(this.session);
        return this.investorTemplates;
      }
    }

    /// <summary>
    /// Gets access to all the Trade Price Adjustment Templates configured in Encompass settings.
    /// </summary>
    public TradePriceAdjustmentTemplates TradePriceAdjustmentTemplates
    {
      get
      {
        if (this.tradePriceAdjustmentTemplates == null)
          this.tradePriceAdjustmentTemplates = new TradePriceAdjustmentTemplates(this.session);
        return this.tradePriceAdjustmentTemplates;
      }
    }

    /// <summary>
    /// Gets access to all the SRP Table Templates configured in Encompass settings.
    /// </summary>
    public SRPTableTemplates SRPTableTemplates
    {
      get
      {
        if (this.sRPTableTemplates == null)
          this.sRPTableTemplates = new SRPTableTemplates(this.session);
        return this.sRPTableTemplates;
      }
    }
  }
}
