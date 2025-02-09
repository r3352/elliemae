// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.SecondarySettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
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

    public LockExtensionSettings LockExtensions => this.lockExtensions;

    public InvestorTemplates InvestorTemplates
    {
      get
      {
        if (this.investorTemplates == null)
          this.investorTemplates = new InvestorTemplates(this.session);
        return this.investorTemplates;
      }
    }

    public TradePriceAdjustmentTemplates TradePriceAdjustmentTemplates
    {
      get
      {
        if (this.tradePriceAdjustmentTemplates == null)
          this.tradePriceAdjustmentTemplates = new TradePriceAdjustmentTemplates(this.session);
        return this.tradePriceAdjustmentTemplates;
      }
    }

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
