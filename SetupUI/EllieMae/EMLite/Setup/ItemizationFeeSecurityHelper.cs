// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ItemizationFeeSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ItemizationFeeSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode FeeDescription;
    protected TreeNode FeeAmounts;
    protected TreeNode BorrowerAmountOnly;
    protected TreeNode SellerAmount;
    protected TreeNode BrokerAmount;
    protected TreeNode FeeOptions;
    protected TreeNode BorrowerCanOptionFor;
    protected TreeNode BorrowerDidShopFor;
    protected TreeNode ImpactsAPR;
    protected TreeNode Escrowed;
    protected TreeNode PropertyTaxes907_912;
    protected TreeNode PropertyTaxes1007_1009;
    protected TreeNode Optional1310_1315;
    protected TreeNode PaidToType;
    protected TreeNode PaidToName;

    public ItemizationFeeSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.ItemizationFeeFeatures);
    }

    public ItemizationFeeSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.ItemizationFeeFeatures);
    }

    public Hashtable SpecialDepTreeNodes
    {
      get => this.securityHelper.getSpecialDepTreeNodes();
      set => this.securityHelper.setSpecialDepTreeNodes(value);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.FeeDescription = new TreeNode("Fee Description");
      this.FeeAmounts = new TreeNode("Fee Amounts");
      this.BorrowerAmountOnly = new TreeNode("Borrower amount only");
      this.SellerAmount = new TreeNode("Seller amount, Seller Credit, Seller Obligated");
      this.BrokerAmount = new TreeNode("Broker, Lender, Other amounts");
      this.FeeOptions = new TreeNode("Fee Options");
      this.BorrowerCanOptionFor = new TreeNode("Borrower can shop for");
      this.BorrowerDidShopFor = new TreeNode("Borrower did shop for");
      this.ImpactsAPR = new TreeNode("Impacts APR");
      this.Escrowed = new TreeNode("Escrowed");
      this.PropertyTaxes907_912 = new TreeNode("Property Taxes,Homeowner's Insurance, Other (907-912)");
      this.PropertyTaxes1007_1009 = new TreeNode("Property Taxes,Homeowner's Insurance, Other (1007-1009)");
      this.Optional1310_1315 = new TreeNode("Optional (1310-1315)");
      this.PaidToType = new TreeNode("Paid To Type");
      this.PaidToName = new TreeNode("Paid To Name");
      this.FeeAmounts.Nodes.AddRange(new TreeNode[3]
      {
        this.BorrowerAmountOnly,
        this.SellerAmount,
        this.BrokerAmount
      });
      this.FeeOptions.Nodes.AddRange(new TreeNode[7]
      {
        this.BorrowerCanOptionFor,
        this.BorrowerDidShopFor,
        this.ImpactsAPR,
        this.Escrowed,
        this.PropertyTaxes907_912,
        this.PropertyTaxes1007_1009,
        this.Optional1310_1315
      });
      treeView.Nodes.AddRange(new TreeNode[5]
      {
        this.FeeDescription,
        this.FeeAmounts,
        this.FeeOptions,
        this.PaidToType,
        this.PaidToName
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.FeeDescription, (object) AclFeature.LoanTab_ItemizationFee_FeeDescription);
      this.nodeToFeature.Add((object) this.FeeAmounts, (object) AclFeature.LoanTab_ItemizationFee_FeeAmount);
      this.nodeToFeature.Add((object) this.BorrowerAmountOnly, (object) AclFeature.LoanTab_ItemizationFee_BorrowerAmountOnly);
      this.nodeToFeature.Add((object) this.SellerAmount, (object) AclFeature.LoanTab_ItemizationFee_SellerAmount);
      this.nodeToFeature.Add((object) this.BrokerAmount, (object) AclFeature.LoanTab_ItemizationFee_BrokerAmount);
      this.nodeToFeature.Add((object) this.FeeOptions, (object) AclFeature.LoanTab_ItemizationFee_FeeOptions);
      this.nodeToFeature.Add((object) this.BorrowerCanOptionFor, (object) AclFeature.LoanTab_ItemizationFee_BorrwerCanShopFor);
      this.nodeToFeature.Add((object) this.BorrowerDidShopFor, (object) AclFeature.LoanTab_ItemizationFee_BorrwerDidShopFor);
      this.nodeToFeature.Add((object) this.ImpactsAPR, (object) AclFeature.LoanTab_ItemizationFee_ImpactAPR);
      this.nodeToFeature.Add((object) this.Escrowed, (object) AclFeature.LoanTab_ItemizationFee_Escrowed);
      this.nodeToFeature.Add((object) this.PropertyTaxes907_912, (object) AclFeature.LoanTab_ItemizationFee_PropertyTaxes907);
      this.nodeToFeature.Add((object) this.PropertyTaxes1007_1009, (object) AclFeature.LoanTab_ItemizationFee_PropertyTaxes1007);
      this.nodeToFeature.Add((object) this.Optional1310_1315, (object) AclFeature.LoanTab_ItemizationFee_Optional1310);
      this.nodeToFeature.Add((object) this.PaidToType, (object) AclFeature.LoanTab_ItemizationFee_PaidToType);
      this.nodeToFeature.Add((object) this.PaidToName, (object) AclFeature.LoanTab_ItemizationFee_PaidToName);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_FeeDescription, (object) this.FeeDescription);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_FeeAmount, (object) this.FeeAmounts);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_BorrowerAmountOnly, (object) this.BorrowerAmountOnly);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_SellerAmount, (object) this.SellerAmount);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_BrokerAmount, (object) this.BrokerAmount);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_FeeOptions, (object) this.FeeOptions);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_BorrwerCanShopFor, (object) this.BorrowerCanOptionFor);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_BorrwerDidShopFor, (object) this.BorrowerDidShopFor);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_ImpactAPR, (object) this.ImpactsAPR);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_Escrowed, (object) this.Escrowed);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_PropertyTaxes907, (object) this.PropertyTaxes907_912);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_PropertyTaxes1007, (object) this.PropertyTaxes1007_1009);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_Optional1310, (object) this.Optional1310_1315);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_PaidToType, (object) this.PaidToType);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ItemizationFee_PaidToName, (object) this.PaidToName);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.FeeDescription, (object) false);
      this.nodeToUpdateStatus.Add((object) this.FeeAmounts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.BorrowerAmountOnly, (object) false);
      this.nodeToUpdateStatus.Add((object) this.SellerAmount, (object) false);
      this.nodeToUpdateStatus.Add((object) this.BrokerAmount, (object) false);
      this.nodeToUpdateStatus.Add((object) this.FeeOptions, (object) false);
      this.nodeToUpdateStatus.Add((object) this.BorrowerCanOptionFor, (object) false);
      this.nodeToUpdateStatus.Add((object) this.BorrowerDidShopFor, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ImpactsAPR, (object) false);
      this.nodeToUpdateStatus.Add((object) this.Escrowed, (object) false);
      this.nodeToUpdateStatus.Add((object) this.PropertyTaxes907_912, (object) false);
      this.nodeToUpdateStatus.Add((object) this.PropertyTaxes1007_1009, (object) false);
      this.nodeToUpdateStatus.Add((object) this.Optional1310_1315, (object) false);
      this.nodeToUpdateStatus.Add((object) this.PaidToType, (object) false);
      this.nodeToUpdateStatus.Add((object) this.PaidToName, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
