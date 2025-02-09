// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The InvestorTemplate class represents an investor template as configured in Encompass' Secondary Setup Settings
  /// </summary>
  public class InvestorTemplate : EnumItem, IInvestorTemplate
  {
    private EllieMae.EMLite.ClientServer.InvestorTemplate investorTemplate;
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
      this.investorTemplate = (EllieMae.EMLite.ClientServer.InvestorTemplate) this.configMngr.GetTemplateSettings(TemplateSettingsType.Investor, this.entry);
      this.investorContacts = new InvestorContacts(this.investorTemplate.CompanyInformation);
    }

    /// <summary>Gets if the investor is for bulk sale.</summary>
    public bool BulkSale
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.BulkSale;
      }
    }

    /// <summary>
    /// Gets the delivery time frame for the investor as number of days.
    /// </summary>
    public int DeliveryTimeFrame
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.CompanyInformation.DeliveryTimeFrame;
      }
    }

    /// <summary>Gets the type of HMDA purchaser.</summary>
    public string TypeOfPurchaser
    {
      get
      {
        this.LoadInvestor();
        return this.investorTemplate.CompanyInformation.TypeOfPurchaser;
      }
    }

    /// <summary>
    /// Gets the collection of all <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContacts">InvestorContact</see>.
    /// </summary>
    public InvestorContacts Contacts
    {
      get
      {
        this.LoadInvestor();
        return this.investorContacts;
      }
    }

    internal EllieMae.EMLite.ClientServer.InvestorTemplate Unwrap()
    {
      this.LoadInvestor();
      return this.investorTemplate;
    }
  }
}
