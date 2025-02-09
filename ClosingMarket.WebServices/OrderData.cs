// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.OrderData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("OrderData", "http://www.closingmarket.com")]
  public class OrderData : UserFieldDataArray
  {
    public int ClosingMarketTransactionID;
    public string OriginatorOrderID;
    public string ProviderOrderID;
    public string OrderStatus;
    public string RegionID;
    public string Branch;
    public string OpenDate;
    public string CloseDate;
    public string CommitDate;
    public TitleCompanyData TitleCompany;
    public string SalesPrice;
    public string OwnerPolicyLiabilityOverride;
    public string LoanPolicyLiabilityOverride;
    public string AmountDown;
    public string EarnestMoney;
    public string Underwriter;
    public ContactData CloseAgent;
    public AgentData Referral;
    public OrderService Service;
    public AgentData SellerRealtor;
    public AgentData BuyerRealtor;
    public LoanData[] LoanInformation;
    public ContactData SalesRep;
    public AgentData PlaceOfClosing;
    public AgentData Customer;
    public string OtherFileNum;
    public string CloseTime;
    public EscrowBankData EscrowBank;
    public AgentData PaperworkPreparedByLawOffice;
    public ContactData EscrowProcessor;
    public string BinderWithPolicy;
    public EscrowBankData EscrowUnit;
    public TitleCompanyData TitleUnit;
    public string FormerPolicyNum;
    public string FormerPolicyAmount;
    public string FormerPolicyDate;
    public string FormerPolicyCode;
    public AgentData Member;
    public string MemberFileNum;
    public string SourceOfBusiness;
    public BuySellData[] Buyers;
    public BuySellData[] Sellers;
    public PropertyData[] Properties;
    public DocumentData[] Documents;
    public NoteData[] Notes;
    public CommitmentData Commitment;
  }
}
