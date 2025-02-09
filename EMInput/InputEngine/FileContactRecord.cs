// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FileContactRecord
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FileContactRecord
  {
    private Sessions.Session session;
    private LoanData loan;

    public FileContactRecord(Sessions.Session session, LoanData loan)
    {
      this.session = session;
      this.loan = loan;
    }

    public FileContactRecord.ContactFields GetBorrower(bool isCoborrower)
    {
      FileContactRecord.ContactFields borrower = new FileContactRecord.ContactFields();
      if (isCoborrower)
      {
        borrower.Category = "Co-Borrower";
        borrower.FirstName = this.getFieldValue("68");
        borrower.LastName = this.getFieldValue("69");
        borrower.Email = this.getFieldValue("1268");
        borrower.BizPhone = this.getFieldValue("FE0217");
        borrower.ContactType = FileContactRecord.ContactTypes.Coborrower;
        borrower.FocusID = "68";
      }
      else
      {
        borrower.Category = "Borrower";
        borrower.FirstName = this.getFieldValue("36");
        borrower.LastName = this.getFieldValue("37");
        borrower.Email = this.getFieldValue("1240");
        borrower.BizPhone = this.getFieldValue("FE0117");
        borrower.ContactType = FileContactRecord.ContactTypes.Borrower;
        borrower.FocusID = "36";
      }
      return borrower;
    }

    public FileContactRecord.ContactFields GetNonBorrowingOwner(int i)
    {
      FileContactRecord.ContactFields nonBorrowingOwner = new FileContactRecord.ContactFields();
      string str1 = "NBOC";
      nonBorrowingOwner.Category = "Non-Borrowing Owner";
      string str2 = this.getFieldValue(str1 + i.ToString("00") + "01") + " " + this.getFieldValue(str1 + i.ToString("00") + "02");
      nonBorrowingOwner.FirstName = str2.Trim();
      string str3 = this.getFieldValue(str1 + i.ToString("00") + "03") + " " + this.getFieldValue(str1 + i.ToString("00") + "04");
      nonBorrowingOwner.LastName = str3.Trim();
      nonBorrowingOwner.Email = this.getFieldValue(str1 + i.ToString("00") + "11");
      nonBorrowingOwner.BizPhone = this.getFieldValue(str1 + i.ToString("00") + "13");
      nonBorrowingOwner.ContactType = FileContactRecord.ContactTypes.NonBorrowingOwnerContact;
      nonBorrowingOwner.FocusID = str1 + i.ToString("00") + "01";
      nonBorrowingOwner.NBOCIndex = i;
      return nonBorrowingOwner;
    }

    public FileContactRecord.ContactFields GetVendors(FileContactRecord.ContactTypes vendorType)
    {
      FileContactRecord.ContactFields vendors = new FileContactRecord.ContactFields();
      vendors.ContactType = vendorType;
      switch (vendorType)
      {
        case FileContactRecord.ContactTypes.Borrower:
          vendors.FullName = this.getFieldValue("4000") + " " + this.getFieldValue("4002");
          vendors.Email = this.getFieldValue("1240");
          vendors.BizPhone = this.getFieldValue("FE0117");
          vendors.FocusID = "4000";
          break;
        case FileContactRecord.ContactTypes.Coborrower:
          vendors.FullName = this.getFieldValue("4004") + " " + this.getFieldValue("4006");
          vendors.Email = this.getFieldValue("1268");
          vendors.BizPhone = this.getFieldValue("FE0217");
          vendors.FocusID = "4004";
          break;
        case FileContactRecord.ContactTypes.Lender:
          vendors.Category = "Lender";
          vendors.Company = this.getFieldValue("1264");
          vendors.FullName = this.getFieldValue("1256");
          vendors.Email = this.getFieldValue("95");
          vendors.BizPhone = this.getFieldValue("1262");
          vendors.FocusID = "1264";
          break;
        case FileContactRecord.ContactTypes.Appraiser:
          vendors.Category = "Appraiser";
          vendors.Company = this.getFieldValue("617");
          vendors.FullName = this.getFieldValue("618");
          vendors.Email = this.getFieldValue("89");
          vendors.BizPhone = this.getFieldValue("622");
          vendors.FocusID = "617";
          vendors.DiscToCD = this.getFieldValue("VEND.X976");
          break;
        case FileContactRecord.ContactTypes.Escrow:
          vendors.Category = "Escrow Company";
          vendors.Company = this.getFieldValue("610");
          vendors.FullName = this.getFieldValue("611");
          vendors.Email = this.getFieldValue("87");
          vendors.BizPhone = this.getFieldValue("615");
          vendors.FocusID = "610";
          break;
        case FileContactRecord.ContactTypes.Title:
          vendors.Category = "Title Insurance Company";
          vendors.Company = this.getFieldValue("411");
          vendors.FullName = this.getFieldValue("416");
          vendors.Email = this.getFieldValue("88");
          vendors.BizPhone = this.getFieldValue("417");
          vendors.FocusID = "411";
          break;
        case FileContactRecord.ContactTypes.BuyerAttorney:
          vendors.Category = "Buyer's Attorney";
          vendors.Company = this.getFieldValue("56");
          vendors.FullName = this.getFieldValue("VEND.X117");
          vendors.Email = this.getFieldValue("VEND.X119");
          vendors.BizPhone = this.getFieldValue("VEND.X118");
          vendors.FocusID = "56";
          break;
        case FileContactRecord.ContactTypes.SellerAttorney:
          vendors.Category = "Seller's Attorney";
          vendors.Company = this.getFieldValue("VEND.X122");
          vendors.FullName = this.getFieldValue("VEND.X128");
          vendors.Email = this.getFieldValue("VEND.X130");
          vendors.BizPhone = this.getFieldValue("VEND.X129");
          vendors.FocusID = "VEND.X122";
          vendors.DiscToCD = this.getFieldValue("VEND.X935");
          break;
        case FileContactRecord.ContactTypes.BuyerAgent:
          vendors.Category = "Buyer's Agent";
          vendors.Company = this.getFieldValue("VEND.X133");
          vendors.FullName = this.getFieldValue("VEND.X139");
          vendors.Email = this.getFieldValue("VEND.X141");
          vendors.BizPhone = this.getFieldValue("VEND.X140");
          vendors.DiscToCD = this.getFieldValue("VEND.X993");
          vendors.FocusID = "VEND.X133";
          break;
        case FileContactRecord.ContactTypes.SellerAgent:
          vendors.Category = "Seller's Agent";
          vendors.Company = this.getFieldValue("VEND.X144");
          vendors.FullName = this.getFieldValue("VEND.X150");
          vendors.Email = this.getFieldValue("VEND.X152");
          vendors.BizPhone = this.getFieldValue("VEND.X151");
          vendors.DiscToCD = this.getFieldValue("VEND.X994");
          vendors.FocusID = "VEND.X144";
          break;
        case FileContactRecord.ContactTypes.Seller:
          vendors.Category = "Seller 1";
          vendors.Company = "";
          vendors.FullName = this.getFieldValue("638");
          vendors.Email = this.getFieldValue("92");
          vendors.BizPhone = this.getFieldValue("VEND.X220");
          vendors.FocusID = "638";
          break;
        case FileContactRecord.ContactTypes.Seller2:
          vendors.Category = "Seller 2";
          vendors.Company = "";
          vendors.FullName = this.getFieldValue("VEND.X412");
          vendors.Email = this.getFieldValue("VEND.X419");
          vendors.BizPhone = this.getFieldValue("VEND.X421");
          vendors.FocusID = "638";
          break;
        case FileContactRecord.ContactTypes.Notary:
          vendors.Category = "Notary";
          vendors.Company = this.getFieldValue("VEND.X424");
          vendors.FullName = this.getFieldValue("VEND.X429");
          vendors.Email = this.getFieldValue("VEND.X432");
          vendors.BizPhone = this.getFieldValue("VEND.X430");
          vendors.FocusID = "VEND.X424";
          vendors.DiscToCD = this.getFieldValue("VEND.X939");
          break;
        case FileContactRecord.ContactTypes.Builder:
          vendors.Category = "Builder";
          vendors.Company = this.getFieldValue("713");
          vendors.FullName = this.getFieldValue("714");
          vendors.Email = this.getFieldValue("94");
          vendors.BizPhone = this.getFieldValue("718");
          vendors.FocusID = "713";
          vendors.DiscToCD = this.getFieldValue("VEND.X941");
          break;
        case FileContactRecord.ContactTypes.HazardInsurance:
          vendors.Category = "Hazard Insurance";
          vendors.Company = this.getFieldValue("L252");
          vendors.FullName = this.getFieldValue("VEND.X162");
          vendors.Email = this.getFieldValue("VEND.X164");
          vendors.BizPhone = this.getFieldValue("VEND.X163");
          vendors.FocusID = "L252";
          vendors.DiscToCD = this.getFieldValue("VEND.X943");
          break;
        case FileContactRecord.ContactTypes.MortgageInsurance:
          vendors.Category = "Mortgage Insurance";
          vendors.Company = this.getFieldValue("L248");
          vendors.FullName = this.getFieldValue("707");
          vendors.Email = this.getFieldValue("93");
          vendors.BizPhone = this.getFieldValue("711");
          vendors.FocusID = "L248";
          vendors.DiscToCD = this.getFieldValue("VEND.X945");
          break;
        case FileContactRecord.ContactTypes.Surveyor:
          vendors.Category = "Surveyor";
          vendors.Company = this.getFieldValue("VEND.X34");
          vendors.FullName = this.getFieldValue("VEND.X35");
          vendors.Email = this.getFieldValue("VEND.X43");
          vendors.BizPhone = this.getFieldValue("VEND.X41");
          vendors.FocusID = "VEND.X34";
          vendors.DiscToCD = this.getFieldValue("VEND.X947");
          break;
        case FileContactRecord.ContactTypes.FloodInsurance:
          vendors.Category = "Flood Insurance";
          vendors.Company = this.getFieldValue("1500");
          vendors.FullName = this.getFieldValue("VEND.X13");
          vendors.Email = this.getFieldValue("VEND.X21");
          vendors.BizPhone = this.getFieldValue("VEND.X19");
          vendors.FocusID = "1500";
          vendors.DiscToCD = this.getFieldValue("VEND.X949");
          break;
        case FileContactRecord.ContactTypes.CreditCompany:
          vendors.Category = "Credit Company";
          vendors.Company = this.getFieldValue("624");
          vendors.FullName = this.getFieldValue("625");
          vendors.Email = this.getFieldValue("90");
          vendors.BizPhone = this.getFieldValue("629");
          vendors.FocusID = "624";
          vendors.DiscToCD = this.getFieldValue("VEND.X951");
          break;
        case FileContactRecord.ContactTypes.Underwriter:
          vendors.Category = "Underwriter";
          vendors.Company = this.getFieldValue("REGZGFE.X8");
          vendors.FullName = this.getFieldValue("984");
          vendors.Email = this.getFieldValue("1411");
          vendors.BizPhone = this.getFieldValue("1410");
          vendors.FocusID = "REGZGFE.X8";
          vendors.DiscToCD = this.getFieldValue("VEND.X953");
          break;
        case FileContactRecord.ContactTypes.Servicing:
          vendors.Category = "Servicing";
          vendors.Company = this.getFieldValue("VEND.X178");
          vendors.FullName = this.getFieldValue("VEND.X184");
          vendors.Email = this.getFieldValue("VEND.X186");
          vendors.BizPhone = this.getFieldValue("VEND.X185");
          vendors.FocusID = "VEND.X178";
          vendors.DiscToCD = this.getFieldValue("VEND.X955");
          break;
        case FileContactRecord.ContactTypes.DocSigning:
          vendors.Category = "Doc Signing";
          vendors.Company = this.getFieldValue("395");
          vendors.FullName = this.getFieldValue("VEND.X195");
          vendors.Email = this.getFieldValue("VEND.X197");
          vendors.BizPhone = this.getFieldValue("VEND.X196");
          vendors.FocusID = "395";
          vendors.DiscToCD = this.getFieldValue("VEND.X957");
          break;
        case FileContactRecord.ContactTypes.Warehouse:
          vendors.Category = "Warehouse";
          vendors.Company = this.getFieldValue("VEND.X200");
          vendors.FullName = this.getFieldValue("VEND.X206");
          vendors.Email = this.getFieldValue("VEND.X208");
          vendors.BizPhone = this.getFieldValue("VEND.X207");
          vendors.FocusID = "VEND.X200";
          vendors.DiscToCD = this.getFieldValue("VEND.X959");
          break;
        case FileContactRecord.ContactTypes.FinancialPlanner:
          vendors.Category = "Financial Planner";
          vendors.Company = this.getFieldValue("VEND.X44");
          vendors.FullName = this.getFieldValue("VEND.X45");
          vendors.Email = this.getFieldValue("VEND.X53");
          vendors.BizPhone = this.getFieldValue("VEND.X51");
          vendors.FocusID = "VEND.X44";
          vendors.DiscToCD = this.getFieldValue("VEND.X961");
          break;
        case FileContactRecord.ContactTypes.Investor:
          if (!this.session.UserInfo.IsSuperAdministrator() && !((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_ShowInvestorContact))
            return (FileContactRecord.ContactFields) null;
          vendors.Category = "Investor";
          vendors.Company = this.getFieldValue("VEND.X263");
          vendors.FullName = this.getFieldValue("VEND.X271");
          vendors.Email = this.getFieldValue("VEND.X273");
          vendors.BizPhone = this.getFieldValue("VEND.X272");
          vendors.FocusID = "VEND.X263";
          vendors.DiscToCD = this.getFieldValue("VEND.X963");
          break;
        case FileContactRecord.ContactTypes.AssignTo:
          vendors.Category = "Assign To";
          vendors.Company = this.getFieldValue("VEND.X278");
          vendors.FullName = this.getFieldValue("VEND.X286");
          vendors.Email = this.getFieldValue("VEND.X288");
          vendors.BizPhone = this.getFieldValue("VEND.X287");
          vendors.FocusID = "VEND.X278";
          vendors.DiscToCD = this.getFieldValue("VEND.X965");
          break;
        case FileContactRecord.ContactTypes.Broker:
          vendors.Category = "Broker";
          vendors.Company = this.getFieldValue("VEND.X293");
          vendors.FullName = this.getFieldValue("VEND.X302");
          vendors.Email = this.getFieldValue("VEND.X305");
          vendors.BizPhone = this.getFieldValue("VEND.X303");
          vendors.FocusID = "VEND.X293";
          break;
        case FileContactRecord.ContactTypes.DocsPrepared:
          vendors.Category = "Docs Prepared By";
          vendors.Company = this.getFieldValue("VEND.X310");
          vendors.FullName = this.getFieldValue("VEND.X317");
          vendors.Email = this.getFieldValue("VEND.X319");
          vendors.BizPhone = this.getFieldValue("VEND.X318");
          vendors.FocusID = "VEND.X310";
          vendors.DiscToCD = this.getFieldValue("VEND.X967");
          break;
        case FileContactRecord.ContactTypes.Custom1:
          vendors.Category = this.getFieldValue("VEND.X84");
          if (vendors.Category == "")
            vendors.Category = "Custom Category #1";
          vendors.Company = this.getFieldValue("VEND.X54");
          vendors.FullName = this.getFieldValue("VEND.X55");
          vendors.Email = this.getFieldValue("VEND.X63");
          vendors.BizPhone = this.getFieldValue("VEND.X61");
          vendors.FocusID = "VEND.X84";
          vendors.DiscToCD = this.getFieldValue("VEND.X969");
          break;
        case FileContactRecord.ContactTypes.Custom2:
          vendors.Category = this.getFieldValue("VEND.X85");
          if (vendors.Category == "")
            vendors.Category = "Custom Category #2";
          vendors.Company = this.getFieldValue("VEND.X64");
          vendors.FullName = this.getFieldValue("VEND.X65");
          vendors.Email = this.getFieldValue("VEND.X73");
          vendors.BizPhone = this.getFieldValue("VEND.X71");
          vendors.FocusID = "VEND.X85";
          vendors.DiscToCD = this.getFieldValue("VEND.X971");
          break;
        case FileContactRecord.ContactTypes.Custom3:
          vendors.Category = this.getFieldValue("VEND.X86");
          if (vendors.Category == "")
            vendors.Category = "Custom Category #3";
          vendors.Company = this.getFieldValue("VEND.X74");
          vendors.FullName = this.getFieldValue("VEND.X75");
          vendors.Email = this.getFieldValue("VEND.X83");
          vendors.BizPhone = this.getFieldValue("VEND.X81");
          vendors.FocusID = "VEND.X86";
          vendors.DiscToCD = this.getFieldValue("VEND.X973");
          break;
        case FileContactRecord.ContactTypes.Custom4:
          vendors.Category = this.getFieldValue("VEND.X11");
          if (vendors.Category == "")
            vendors.Category = "Custom Category #4";
          vendors.Company = this.getFieldValue("VEND.X1");
          vendors.FullName = this.getFieldValue("VEND.X2");
          vendors.Email = this.getFieldValue("VEND.X10");
          vendors.BizPhone = this.getFieldValue("VEND.X8");
          vendors.FocusID = "VEND.X11";
          vendors.DiscToCD = this.getFieldValue("VEND.X975");
          break;
        case FileContactRecord.ContactTypes.Seller3:
          vendors.Category = "Seller 3";
          vendors.Company = "";
          vendors.FullName = this.getFieldValue("Seller3.Name");
          vendors.Email = this.getFieldValue("Seller3.Email");
          vendors.BizPhone = this.getFieldValue("Seller3.BusPh");
          vendors.FocusID = "Seller3.Name";
          break;
        case FileContactRecord.ContactTypes.Seller4:
          vendors.Category = "Seller 4";
          vendors.Company = "";
          vendors.FullName = this.getFieldValue("Seller4.Name");
          vendors.Email = this.getFieldValue("Seller4.Email");
          vendors.BizPhone = this.getFieldValue("Seller4.BusPh");
          vendors.FocusID = "Seller4.Name";
          break;
        case FileContactRecord.ContactTypes.SettlementAgent:
          vendors.Category = "Settlement Agent";
          vendors.Company = this.getFieldValue("VEND.X655");
          vendors.FullName = this.getFieldValue("VEND.X668");
          vendors.Email = this.getFieldValue("VEND.X670");
          vendors.BizPhone = this.getFieldValue("VEND.X669");
          vendors.DiscToCD = this.getFieldValue("VEND.X654");
          vendors.FocusID = "VEND.X655";
          break;
        case FileContactRecord.ContactTypes.SellerCorporationOfficer:
          vendors.Category = "Seller Corporation Officers";
          vendors.Company = this.getFieldValue("1863");
          vendors.FullName = this.getFieldValue("Vesting.SelOfcr1Nm");
          vendors.Email = this.getFieldValue("4886");
          vendors.BizPhone = this.getFieldValue("4885");
          vendors.FocusID = "1863";
          break;
      }
      return vendors;
    }

    private FileContactRecord.ContactFields GetVendorsName(FileContactRecord.ContactTypes vendorType)
    {
      FileContactRecord.ContactFields vendorsName = new FileContactRecord.ContactFields();
      vendorsName.ContactType = vendorType;
      switch (vendorType)
      {
        case FileContactRecord.ContactTypes.Borrower:
          vendorsName.FullName = "Borrower";
          vendorsName.FocusID = "4000";
          break;
        case FileContactRecord.ContactTypes.Coborrower:
          vendorsName.FullName = "Coborrower";
          vendorsName.FocusID = "4004";
          break;
        case FileContactRecord.ContactTypes.Lender:
          vendorsName.Category = "Lender";
          vendorsName.FocusID = "1264";
          break;
        case FileContactRecord.ContactTypes.Appraiser:
          vendorsName.Category = "Appraiser";
          vendorsName.FocusID = "617";
          break;
        case FileContactRecord.ContactTypes.Escrow:
          vendorsName.Category = "Escrow Company";
          vendorsName.FocusID = "610";
          break;
        case FileContactRecord.ContactTypes.Title:
          vendorsName.Category = "Title Insurance Company";
          vendorsName.FocusID = "411";
          break;
        case FileContactRecord.ContactTypes.BuyerAttorney:
          vendorsName.Category = "Buyer's Attorney";
          vendorsName.FocusID = "56";
          break;
        case FileContactRecord.ContactTypes.SellerAttorney:
          vendorsName.Category = "Seller's Attorney";
          vendorsName.FocusID = "VEND.X122";
          break;
        case FileContactRecord.ContactTypes.BuyerAgent:
          vendorsName.Category = "Buyer's Agent";
          vendorsName.FocusID = "VEND.X133";
          break;
        case FileContactRecord.ContactTypes.SellerAgent:
          vendorsName.Category = "Seller's Agent";
          vendorsName.FocusID = "VEND.X144";
          break;
        case FileContactRecord.ContactTypes.Seller:
          vendorsName.Category = "Seller 1";
          vendorsName.FocusID = "638";
          break;
        case FileContactRecord.ContactTypes.Seller2:
          vendorsName.Category = "Seller 2";
          vendorsName.FocusID = "638";
          break;
        case FileContactRecord.ContactTypes.Notary:
          vendorsName.Category = "Notary";
          vendorsName.FocusID = "VEND.X424";
          break;
        case FileContactRecord.ContactTypes.Builder:
          vendorsName.Category = "Builder";
          vendorsName.FocusID = "713";
          break;
        case FileContactRecord.ContactTypes.HazardInsurance:
          vendorsName.Category = "Hazard Insurance";
          vendorsName.FocusID = "L252";
          break;
        case FileContactRecord.ContactTypes.MortgageInsurance:
          vendorsName.Category = "Mortgage Insurance";
          vendorsName.FocusID = "L248";
          break;
        case FileContactRecord.ContactTypes.Surveyor:
          vendorsName.Category = "Surveyor";
          vendorsName.FocusID = "VEND.X34";
          break;
        case FileContactRecord.ContactTypes.FloodInsurance:
          vendorsName.Category = "Flood Insurance";
          vendorsName.FocusID = "1500";
          break;
        case FileContactRecord.ContactTypes.CreditCompany:
          vendorsName.Category = "Credit Company";
          vendorsName.FocusID = "624";
          break;
        case FileContactRecord.ContactTypes.Underwriter:
          vendorsName.Category = "Underwriter";
          vendorsName.FocusID = "REGZGFE.X8";
          break;
        case FileContactRecord.ContactTypes.Servicing:
          vendorsName.Category = "Servicing";
          vendorsName.FocusID = "VEND.X178";
          break;
        case FileContactRecord.ContactTypes.DocSigning:
          vendorsName.Category = "Doc Signing";
          vendorsName.FocusID = "395";
          break;
        case FileContactRecord.ContactTypes.Warehouse:
          vendorsName.Category = "Warehouse";
          vendorsName.FocusID = "VEND.X200";
          break;
        case FileContactRecord.ContactTypes.FinancialPlanner:
          vendorsName.Category = "Financial Planner";
          vendorsName.FocusID = "VEND.X44";
          break;
        case FileContactRecord.ContactTypes.Investor:
          vendorsName.Category = "Investor";
          vendorsName.FocusID = "VEND.X263";
          break;
        case FileContactRecord.ContactTypes.AssignTo:
          vendorsName.Category = "Assign To";
          vendorsName.FocusID = "VEND.X278";
          break;
        case FileContactRecord.ContactTypes.Broker:
          vendorsName.Category = "Broker";
          vendorsName.FocusID = "VEND.X293";
          break;
        case FileContactRecord.ContactTypes.DocsPrepared:
          vendorsName.Category = "Docs Prepared By";
          vendorsName.FocusID = "VEND.X310";
          break;
        case FileContactRecord.ContactTypes.Custom1:
          vendorsName.Category = "Custom Category #1";
          vendorsName.FocusID = "VEND.X84";
          break;
        case FileContactRecord.ContactTypes.Custom2:
          vendorsName.Category = "Custom Category #2";
          vendorsName.FocusID = "VEND.X85";
          break;
        case FileContactRecord.ContactTypes.Custom3:
          vendorsName.Category = "Custom Category #3";
          vendorsName.FocusID = "VEND.X86";
          break;
        case FileContactRecord.ContactTypes.Custom4:
          vendorsName.Category = "Custom Category #4";
          vendorsName.FocusID = "VEND.X11";
          break;
        case FileContactRecord.ContactTypes.Seller3:
          vendorsName.Category = "Seller 3";
          vendorsName.FocusID = "Seller3.Name";
          break;
        case FileContactRecord.ContactTypes.Seller4:
          vendorsName.Category = "Seller 4";
          vendorsName.FocusID = "Seller4.Name";
          break;
        case FileContactRecord.ContactTypes.SellerCorporationOfficer:
          vendorsName.FullName = "Seller Corporation Officers";
          vendorsName.FocusID = "1863";
          break;
      }
      return vendorsName;
    }

    public FileContactRecord.ContactFields[] GetRolesContactRecords()
    {
      List<FileContactRecord.ContactFields> contactFieldsList = new List<FileContactRecord.ContactFields>();
      Hashtable hashtable1 = new Hashtable();
      RoleInfo[] allRoles = this.session.LoanDataMgr.SystemConfiguration.AllRoles;
      for (int index = 0; index < allRoles.Length; ++index)
      {
        if (!hashtable1.ContainsKey((object) allRoles[index].RoleID))
          hashtable1.Add((object) allRoles[index].RoleID, (object) allRoles[index]);
      }
      LogList logList = this.loan.GetLogList();
      if (logList != null)
      {
        Hashtable hashtable2 = new Hashtable();
        foreach (MilestoneLog allMilestone in logList.GetAllMilestones())
        {
          if (allMilestone.RoleID != -1 || !((allMilestone.RoleName ?? "") == ""))
          {
            FileContactRecord.ContactFields contactFields = new FileContactRecord.ContactFields();
            hashtable2[(object) allMilestone.RoleID] = (object) allMilestone.RoleName;
            if (string.Compare(allMilestone.Stage, "Started", true) == 0)
              contactFields.Category = "Role - File Starter";
            else if (hashtable1.ContainsKey((object) allMilestone.RoleID))
              contactFields.Category = "Role - " + ((RoleSummaryInfo) hashtable1[(object) allMilestone.RoleID]).RoleName + " (" + allMilestone.Stage + ")";
            else
              contactFields.Category = "Role - " + allMilestone.RoleName + " (" + allMilestone.Stage + ")";
            contactFields.FullName = allMilestone.LoanAssociateName;
            contactFields.BizPhone = allMilestone.LoanAssociatePhone;
            contactFields.Email = allMilestone.LoanAssociateEmail;
            contactFieldsList.Add(contactFields);
          }
        }
        ArrayList arrayList = new ArrayList();
        foreach (RoleInfo roleInfo in (IEnumerable) hashtable1.Values)
        {
          if (roleInfo.RoleID != RoleInfo.FileStarter.RoleID && !hashtable2.Contains((object) roleInfo.RoleID))
            arrayList.Add((object) roleInfo);
        }
        arrayList.Sort();
        foreach (RoleInfo roleInfo in arrayList)
        {
          FileContactRecord.ContactFields contactFields = new FileContactRecord.ContactFields();
          contactFields.Category = "Role - " + roleInfo.RoleName;
          LoanAssociateLog loanAssociateLog = (LoanAssociateLog) null;
          LoanAssociateLog[] assignedAssociates = logList.GetAssignedAssociates(roleInfo.RoleID);
          if (assignedAssociates != null && assignedAssociates.Length != 0)
          {
            loanAssociateLog = assignedAssociates[0];
          }
          else
          {
            LoanAssociateLog[] allLoanAssociates = logList.GetAllLoanAssociates();
            if (allLoanAssociates != null)
            {
              for (int index = 0; index < allLoanAssociates.Length; ++index)
              {
                if (allLoanAssociates[index].RoleID == roleInfo.RoleID)
                {
                  loanAssociateLog = allLoanAssociates[index];
                  break;
                }
              }
            }
          }
          if (loanAssociateLog == null)
          {
            MilestoneFreeRoleLog rec = new MilestoneFreeRoleLog();
            rec.RoleID = roleInfo.RoleID;
            rec.RoleName = roleInfo.RoleName;
            rec.MarkAsClean();
            logList.AddRecord((LogRecordBase) rec, false);
            loanAssociateLog = (LoanAssociateLog) rec;
          }
          contactFields.FullName = loanAssociateLog.LoanAssociateName;
          contactFields.BizPhone = loanAssociateLog.LoanAssociatePhone;
          contactFields.Email = loanAssociateLog.LoanAssociateEmail;
          contactFieldsList.Add(contactFields);
        }
      }
      return contactFieldsList.ToArray();
    }

    public FileContactRecord.ContactFields[] GetContactRecords()
    {
      List<FileContactRecord.ContactFields> contactFieldsList = new List<FileContactRecord.ContactFields>();
      FileContactRecord.ContactFields contactFields = (FileContactRecord.ContactFields) null;
      int num = 0;
      for (int index = 1; index <= 36; ++index)
      {
        if (this.loan == null || !this.loan.IsTemplate || index >= 4)
        {
          switch (index)
          {
            case 3:
              continue;
            case 4:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Lender) : this.GetVendorsName(FileContactRecord.ContactTypes.Lender);
              break;
            case 5:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Appraiser) : this.GetVendorsName(FileContactRecord.ContactTypes.Appraiser);
              break;
            case 6:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Escrow) : this.GetVendorsName(FileContactRecord.ContactTypes.Escrow);
              break;
            case 7:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Title) : this.GetVendorsName(FileContactRecord.ContactTypes.Title);
              break;
            case 8:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.BuyerAttorney) : this.GetVendorsName(FileContactRecord.ContactTypes.BuyerAttorney);
              break;
            case 9:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.SellerAttorney) : this.GetVendorsName(FileContactRecord.ContactTypes.SellerAttorney);
              break;
            case 10:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.BuyerAgent) : this.GetVendorsName(FileContactRecord.ContactTypes.BuyerAgent);
              break;
            case 11:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.SellerAgent) : this.GetVendorsName(FileContactRecord.ContactTypes.SellerAgent);
              break;
            case 12:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Seller) : this.GetVendorsName(FileContactRecord.ContactTypes.Seller);
              break;
            case 13:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Seller2) : this.GetVendorsName(FileContactRecord.ContactTypes.Seller2);
              break;
            case 14:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Seller3) : this.GetVendorsName(FileContactRecord.ContactTypes.Seller3);
              break;
            case 15:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Seller4) : this.GetVendorsName(FileContactRecord.ContactTypes.Seller4);
              break;
            case 16:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Notary) : this.GetVendorsName(FileContactRecord.ContactTypes.Notary);
              break;
            case 17:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Builder) : this.GetVendorsName(FileContactRecord.ContactTypes.Builder);
              break;
            case 18:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.HazardInsurance) : this.GetVendorsName(FileContactRecord.ContactTypes.HazardInsurance);
              break;
            case 19:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.MortgageInsurance) : this.GetVendorsName(FileContactRecord.ContactTypes.MortgageInsurance);
              break;
            case 20:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Surveyor) : this.GetVendorsName(FileContactRecord.ContactTypes.Surveyor);
              break;
            case 21:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.FloodInsurance) : this.GetVendorsName(FileContactRecord.ContactTypes.FloodInsurance);
              break;
            case 22:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.CreditCompany) : this.GetVendorsName(FileContactRecord.ContactTypes.CreditCompany);
              break;
            case 23:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Underwriter) : this.GetVendorsName(FileContactRecord.ContactTypes.Underwriter);
              break;
            case 24:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Servicing) : this.GetVendorsName(FileContactRecord.ContactTypes.Servicing);
              break;
            case 25:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.DocSigning) : this.GetVendorsName(FileContactRecord.ContactTypes.DocSigning);
              break;
            case 26:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Warehouse) : this.GetVendorsName(FileContactRecord.ContactTypes.Warehouse);
              break;
            case 27:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.FinancialPlanner) : this.GetVendorsName(FileContactRecord.ContactTypes.FinancialPlanner);
              break;
            case 28:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Investor) : this.GetVendorsName(FileContactRecord.ContactTypes.Investor);
              break;
            case 29:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.AssignTo) : this.GetVendorsName(FileContactRecord.ContactTypes.AssignTo);
              break;
            case 30:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Broker) : this.GetVendorsName(FileContactRecord.ContactTypes.Broker);
              break;
            case 31:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.DocsPrepared) : this.GetVendorsName(FileContactRecord.ContactTypes.DocsPrepared);
              break;
            case 32:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Custom1) : this.GetVendorsName(FileContactRecord.ContactTypes.Custom1);
              break;
            case 33:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Custom2) : this.GetVendorsName(FileContactRecord.ContactTypes.Custom2);
              break;
            case 34:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Custom3) : this.GetVendorsName(FileContactRecord.ContactTypes.Custom3);
              break;
            case 35:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.Custom4) : this.GetVendorsName(FileContactRecord.ContactTypes.Custom4);
              break;
            case 36:
              contactFields = this.loan != null ? this.GetVendors(FileContactRecord.ContactTypes.SellerCorporationOfficer) : this.GetVendorsName(FileContactRecord.ContactTypes.SellerCorporationOfficer);
              break;
          }
          ++num;
          if (contactFields != null)
            contactFieldsList.Add(contactFields);
        }
      }
      return contactFieldsList.ToArray();
    }

    private string getFieldValue(string id)
    {
      return !this.session.UserInfo.IsSuperAdministrator() && this.session.LoanDataMgr.GetFieldAccessRights(id) == BizRule.FieldAccessRight.Hide ? "*" : this.loan.GetSimpleField(id);
    }

    public enum ContactTypes
    {
      Borrower = 0,
      Coborrower = 1,
      Lender = 2,
      Appraiser = 3,
      Escrow = 4,
      Title = 5,
      BuyerAttorney = 6,
      SellerAttorney = 7,
      BuyerAgent = 8,
      SellerAgent = 9,
      Seller = 10, // 0x0000000A
      Seller2 = 11, // 0x0000000B
      Notary = 12, // 0x0000000C
      Builder = 13, // 0x0000000D
      HazardInsurance = 14, // 0x0000000E
      MortgageInsurance = 15, // 0x0000000F
      Surveyor = 16, // 0x00000010
      FloodInsurance = 17, // 0x00000011
      CreditCompany = 18, // 0x00000012
      Underwriter = 19, // 0x00000013
      Servicing = 20, // 0x00000014
      DocSigning = 21, // 0x00000015
      Warehouse = 22, // 0x00000016
      FinancialPlanner = 23, // 0x00000017
      Investor = 24, // 0x00000018
      AssignTo = 25, // 0x00000019
      Broker = 26, // 0x0000001A
      DocsPrepared = 27, // 0x0000001B
      Custom1 = 28, // 0x0000001C
      Custom2 = 29, // 0x0000001D
      Custom3 = 30, // 0x0000001E
      Custom4 = 32, // 0x00000020
      Seller3 = 35, // 0x00000023
      Seller4 = 36, // 0x00000024
      SettlementAgent = 37, // 0x00000025
      NonBorrowingOwnerContact = 38, // 0x00000026
      SellerCorporationOfficer = 39, // 0x00000027
    }

    public class ContactFields
    {
      private FileContactRecord.ContactTypes contactType;
      private string focusID = "";
      private string category = "";
      private string discToCD = "";
      private string fullName = "";
      private string company = "";
      private string firstName = "";
      private string lastName = "";
      private string address = "";
      private string city = "";
      private string state = "";
      private string zip = "";
      private string homePhone = "";
      private string email = "";
      private string bizPhone = "";
      private string cellPhone = "";
      private string fax = "";
      private int nbocIndex = -1;

      public FileContactRecord.ContactTypes ContactType
      {
        set => this.contactType = value;
        get => this.contactType;
      }

      public string FocusID
      {
        set => this.focusID = value;
        get => this.focusID;
      }

      public string Category
      {
        set => this.category = value;
        get => this.category;
      }

      public string DiscToCD
      {
        set
        {
          switch (value)
          {
            case "Y":
              this.discToCD = "Yes";
              break;
            case "N":
              this.discToCD = "No";
              break;
          }
        }
        get => this.discToCD;
      }

      public string FullName
      {
        set => this.fullName = value;
        get
        {
          return this.fullName == string.Empty ? (this.firstName.Trim() + " " + this.lastName.Trim()).Trim() : this.fullName;
        }
      }

      public string Company
      {
        set => this.company = value;
        get => this.company;
      }

      public string FirstName
      {
        set => this.firstName = value;
        get => this.firstName;
      }

      public string LastName
      {
        set => this.lastName = value;
        get => this.lastName;
      }

      public string Address
      {
        set => this.address = value;
        get => this.address;
      }

      public string City
      {
        set => this.city = value;
        get => this.city;
      }

      public string State
      {
        set => this.state = value;
        get => this.state;
      }

      public string Zip
      {
        set => this.zip = value;
        get => this.zip;
      }

      public string HomePhone
      {
        set => this.homePhone = value;
        get => this.homePhone;
      }

      public string Email
      {
        set => this.email = value;
        get => this.email;
      }

      public string BizPhone
      {
        set => this.bizPhone = value;
        get => this.bizPhone;
      }

      public string CellPhone
      {
        set => this.cellPhone = value;
        get => this.cellPhone;
      }

      public string Fax
      {
        set => this.fax = value;
        get => this.fax;
      }

      public int NBOCIndex
      {
        set => this.nbocIndex = value;
        get => this.nbocIndex;
      }
    }
  }
}
