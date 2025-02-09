// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class InvestorTemplate : EnumItem, IInvestorTemplate
  {
    private InvestorTemplate investorTemplate;
    private InvestorContacts investorContacts;
    private FileSystemEntry entry;
    private IConfigurationManager configMngr;

    internal InvestorTemplate(int id, FileSystemEntry entry, Session session)
      : base(id, entry.Name)
    {
      this.entry = entry;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    private void LoadInvestor()
    {
      if (this.investorTemplate != null)
        return;
      this.investorTemplate = InvestorTemplate.op_Explicit(this.configMngr.GetTemplateSettings((TemplateSettingsType) 14, this.entry));
      this.investorContacts = new InvestorContacts(this.investorTemplate.CompanyInformation);
    }

    public bool BulkSale
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.BulkSale;
      }
    }

    public int DeliveryTimeFrame
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.CompanyInformation.DeliveryTimeFrame;
      }
    }

    public string TypeOfPurchaser
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.CompanyInformation.TypeOfPurchaser;
      }
    }

    public InvestorContacts Contacts
    {
      get
      {
        this.LoadInvestor();
        return this.investorContacts;
      }
    }

    internal InvestorTemplate Unwrap()
    {
      this.LoadInvestor();
      return this.investorTemplate;
    }
  }
}
