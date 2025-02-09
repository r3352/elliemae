// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactUtil
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactUtil
  {
    private static string className = nameof (ContactUtil);
    private static string sw = Tracing.SwContact;
    public static readonly Regex nameDelimiter = new Regex("[,\\s]+", RegexOptions.RightToLeft);
    private static readonly string _BorFirstNameFieldId = "4000";
    private static readonly string _BorMiddleNameFieldId = "4001";
    private static readonly string _BorLastNameFieldId = "4002";
    private static readonly string _BorSuffixNameFieldId = "4003";
    private static readonly string _BorSSNFieldId = "65";
    private static readonly string _CoBorFirstNameFieldId = "4004";
    private static readonly string _CoBorMiddleNameFieldId = "4005";
    private static readonly string _CoBorLastNameFieldId = "4006";
    private static readonly string _CoBorSuffixNameFieldId = "4007";
    private static readonly string _CoBorSSNFieldId = "97";
    private static readonly string _AppraisedValueFieldId = "356";
    private static readonly string _LoanAmountFieldId = "2";
    private static readonly string _InterestRateFieldId = "3";
    private static readonly string _TermFieldId = "4";
    private static readonly string _PurposeFieldId = "19";
    private static readonly string _DownPaymentFieldId = "1771";
    private static readonly string _LTVFieldId = "353";
    private static readonly string _AmortizationFieldId = "608";
    private static readonly string _LoanTypeFieldId = "1172";
    private static readonly string _LienPositionFieldId = "420";
    private SessionObjects sessionObjects;
    private static Hashtable _NameEmidToCompanyEmid;
    private BizCategoryUtil bizCatUtil;
    private bool fromLoanImport;
    private static readonly string[] _AppraiserContactNameEmid = new string[1]
    {
      "618"
    };
    private static readonly string[] _AttorneyContactNameEmid = new string[2]
    {
      "VEND.X117",
      "VEND.X128"
    };
    private static readonly string[] _CreditContactNameEmid = new string[1]
    {
      "625"
    };
    private static readonly string[] _DocSigningContactNameEmid = new string[1]
    {
      "VEND.X195"
    };
    private static readonly string[] _EscrowContactNameEmid = new string[1]
    {
      "611"
    };
    private static readonly string[] _FloodContactNameEmid = new string[1]
    {
      "VEND.X13"
    };
    private static readonly string[] _HazardContactNameEmid = new string[1]
    {
      "VEND.X162"
    };
    private static readonly string[] _LenderContactNameEmid = new string[1]
    {
      "1256"
    };
    private static readonly string[] _MortgageContactNameEmid = new string[1]
    {
      "707"
    };
    private static readonly string[] _RealEstateContactNameEmid = new string[2]
    {
      "VEND.X139",
      "VEND.X150"
    };
    private static readonly string[] _ServicingContactNameEmid = new string[1]
    {
      "VEND.X184"
    };
    private static readonly string[] _TitleContactNameEmid = new string[1]
    {
      "416"
    };
    private static readonly string[] _UnderwriterContactNameEmid = new string[1]
    {
      "984"
    };
    private static readonly string[] _SurveyorContactNameEmid = new string[1]
    {
      "VEND.X35"
    };
    private static readonly string[] _BuilderContactNameEmid = new string[1]
    {
      "714"
    };
    private static readonly string[] _WarehouseContactNameEmid = new string[1]
    {
      "VEND.X206"
    };
    private static readonly string[] _NoCategoryTypePlannerContactNameEmid = new string[1]
    {
      "VEND.X45"
    };
    private static readonly string[] _InvestorContactNameEmid = new string[1]
    {
      "VEND.X271"
    };
    private static readonly string[] _OrganizationTypeContactNameEmid = new string[3]
    {
      "VEND.X302",
      "VEND.X317",
      "VEND.X286"
    };
    private static readonly string[][] BizContactNameEmidsByCate = new string[20][]
    {
      ContactUtil._AppraiserContactNameEmid,
      ContactUtil._AttorneyContactNameEmid,
      ContactUtil._CreditContactNameEmid,
      ContactUtil._DocSigningContactNameEmid,
      ContactUtil._EscrowContactNameEmid,
      ContactUtil._FloodContactNameEmid,
      ContactUtil._HazardContactNameEmid,
      ContactUtil._LenderContactNameEmid,
      ContactUtil._MortgageContactNameEmid,
      ContactUtil._RealEstateContactNameEmid,
      ContactUtil._ServicingContactNameEmid,
      ContactUtil._TitleContactNameEmid,
      ContactUtil._UnderwriterContactNameEmid,
      ContactUtil._SurveyorContactNameEmid,
      ContactUtil._NoCategoryTypePlannerContactNameEmid,
      ContactUtil._OrganizationTypeContactNameEmid,
      new string[0],
      ContactUtil._InvestorContactNameEmid,
      ContactUtil._WarehouseContactNameEmid,
      ContactUtil._BuilderContactNameEmid
    };
    private static Hashtable cateAbbrToNameTbl = new Hashtable();

    static ContactUtil()
    {
      ContactUtil.cateAbbrToNameTbl.Add((object) "all", (object) "All");
      ContactUtil.cateAbbrToNameTbl.Add((object) "app", (object) "Appraiser");
      ContactUtil.cateAbbrToNameTbl.Add((object) "att", (object) "Attorney");
      ContactUtil.cateAbbrToNameTbl.Add((object) "cre", (object) "Credit Company");
      ContactUtil.cateAbbrToNameTbl.Add((object) "doc", (object) "Doc Signing");
      ContactUtil.cateAbbrToNameTbl.Add((object) "esc", (object) "Escrow Company");
      ContactUtil.cateAbbrToNameTbl.Add((object) "flo", (object) "Flood Insurance");
      ContactUtil.cateAbbrToNameTbl.Add((object) "haz", (object) "Hazard Insurance");
      ContactUtil.cateAbbrToNameTbl.Add((object) "len", (object) "Lender");
      ContactUtil.cateAbbrToNameTbl.Add((object) "mor", (object) "Mortgage Insurance");
      ContactUtil.cateAbbrToNameTbl.Add((object) "rea", (object) "Real Estate Agent");
      ContactUtil.cateAbbrToNameTbl.Add((object) "ser", (object) "Servicing");
      ContactUtil.cateAbbrToNameTbl.Add((object) "tit", (object) "Title Insurance");
      ContactUtil.cateAbbrToNameTbl.Add((object) "und", (object) "Underwriter");
      ContactUtil.cateAbbrToNameTbl.Add((object) "sur", (object) "Surveyor");
      ContactUtil.cateAbbrToNameTbl.Add((object) "noc", (object) "No Category");
      ContactUtil.cateAbbrToNameTbl.Add((object) "org", (object) "Organization");
      ContactUtil.cateAbbrToNameTbl.Add((object) "ver", (object) "Verification");
      ContactUtil.cateAbbrToNameTbl.Add((object) "inv", (object) "Investor");
      ContactUtil.cateAbbrToNameTbl.Add((object) "war", (object) "Warehouse Bank");
      ContactUtil._NameEmidToCompanyEmid = new Hashtable();
      ContactUtil._NameEmidToCompanyEmid.Add((object) "618", (object) "617");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X117", (object) "56");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X128", (object) "VEND.X122");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "625", (object) "624");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X195", (object) "395");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "611", (object) "610");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X13", (object) "1500");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X162", (object) "L252");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "1256", (object) "1264");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "707", (object) "L248");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X139", (object) "VEND.X133");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X150", (object) "VEND.X144");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X184", (object) "VEND.X178");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "416", (object) "411");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "984", (object) "REGZGFE.X8");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X35", (object) "VEND.X34");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "714", (object) "713");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X206", (object) "VEND.X200");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X45", (object) "VEND.X44");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X55", (object) "VEND.X84");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X65", (object) "VEND.X85");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X75", (object) "VEND.X86");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X2", (object) "VEND.X11");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X271", (object) "VEND.X263");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X286", (object) "VEND.X278");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X302", (object) "VEND.X293");
      ContactUtil._NameEmidToCompanyEmid.Add((object) "VEND.X317", (object) "VEND.X310");
    }

    public ContactUtil(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.bizCatUtil = new BizCategoryUtil(this.sessionObjects);
    }

    public static void InitContactUtil()
    {
    }

    public int GetCategoryID(int index)
    {
      int categoryId = -1;
      switch (index)
      {
        case 0:
          categoryId = this.bizCatUtil.CategoryNameToId("Appraiser");
          break;
        case 1:
          categoryId = this.bizCatUtil.CategoryNameToId("Attorney");
          break;
        case 2:
          categoryId = this.bizCatUtil.CategoryNameToId("Credit Company");
          break;
        case 3:
          categoryId = this.bizCatUtil.CategoryNameToId("Doc Signing");
          break;
        case 4:
          categoryId = this.bizCatUtil.CategoryNameToId("Escrow Company");
          break;
        case 5:
          categoryId = this.bizCatUtil.CategoryNameToId("Flood Insurance");
          break;
        case 6:
          categoryId = this.bizCatUtil.CategoryNameToId("Hazard Insurance");
          break;
        case 7:
          categoryId = this.bizCatUtil.CategoryNameToId("Lender");
          break;
        case 8:
          categoryId = this.bizCatUtil.CategoryNameToId("Mortgage Insurance");
          break;
        case 9:
          categoryId = this.bizCatUtil.CategoryNameToId("Real Estate Agent");
          break;
        case 10:
          categoryId = this.bizCatUtil.CategoryNameToId("Servicing");
          break;
        case 11:
          categoryId = this.bizCatUtil.CategoryNameToId("Title Insurance");
          break;
        case 12:
          categoryId = this.bizCatUtil.CategoryNameToId("Underwriter");
          break;
        case 13:
          categoryId = this.bizCatUtil.CategoryNameToId("Surveyor");
          break;
        case 14:
          categoryId = this.bizCatUtil.CategoryNameToId("No Category");
          break;
        case 15:
          categoryId = this.bizCatUtil.CategoryNameToId("Organization");
          break;
        case 16:
          categoryId = this.bizCatUtil.CategoryNameToId("Verification");
          break;
        case 17:
          categoryId = this.bizCatUtil.CategoryNameToId("Investor");
          break;
        case 18:
          categoryId = this.bizCatUtil.CategoryNameToId("Warehouse Bank");
          break;
        case 19:
          categoryId = this.bizCatUtil.CategoryNameToId("Builder");
          break;
      }
      return categoryId;
    }

    public static bool IsContactUpdateRequired(
      LoanData loanData,
      ILoanConfigurationInfo configInfo,
      string defaultMilestoneID)
    {
      if (loanData.GetField("2821") == "Y")
        return false;
      string simpleField1 = loanData.GetSimpleField("19");
      string simpleField2 = loanData.GetSimpleField("1811");
      if (simpleField1 != "Purchase" || simpleField2 != "PrimaryResidence")
        return false;
      string updateMilestoneId = configInfo.ContactHistoryUpdateMilestoneID;
      MilestoneLog milestoneById = loanData.GetLogList().GetMilestoneByID(updateMilestoneId);
      if (milestoneById != null)
        return milestoneById.Done;
      return defaultMilestoneID != "" ? loanData.GetLogList().GetMilestoneByID(defaultMilestoneID).Done : loanData.GetLogList().GetMilestoneByID("7").Done;
    }

    public void InsertLinkedContactHistoryOnLoanClosed(
      LoanData loan,
      bool interactive,
      bool fromLoanImport)
    {
      this.fromLoanImport = fromLoanImport;
      ContactLoanInfo contactLoan = this.insertLinkedLoanOnLoanClosed(loan);
      if (contactLoan == null)
      {
        Tracing.Log(ContactUtil.sw, TraceLevel.Error, ContactUtil.className, "Cannot insert loan history records because InsertLoanOnLoanClosed returns 0.");
      }
      else
      {
        CRMLog[] allCrmMapping = loan.GetLogList().GetAllCRMMapping();
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        foreach (CRMLog crmLog in allCrmMapping)
        {
          if (crmLog.MappingType == CRMLogType.BorrowerContact)
            stringList1.Add(crmLog.ContactGuid);
          else
            stringList2.Add(crmLog.ContactGuid);
        }
        if (stringList1.Count > 0)
          this.InsertLinkedBorrowerHistoryOnLoanClosed(contactLoan, loan, stringList1.ToArray(), interactive);
        if (stringList2.Count <= 0)
          return;
        this.InsertLinkedBizPartnerHistoryOnLoanClosed(contactLoan, loan, stringList2.ToArray(), interactive);
      }
    }

    public void InsertContactHistoryOnLoanClosed(
      LoanData loan,
      bool interactive,
      bool fromLoanImport)
    {
      this.fromLoanImport = fromLoanImport;
      int loanId = this.InsertLoanOnLoanClosed(loan);
      if (loanId == 0)
      {
        Tracing.Log(ContactUtil.sw, TraceLevel.Error, ContactUtil.className, "Cannot insert loan history records because InsertLoanOnLoanClosed returns 0.");
      }
      else
      {
        string loid = this.makeBestGuessOfContactOwnerFromLoan(loan, true);
        if (loid != string.Empty)
        {
          BorrowerPair[] borrowerPairs = loan.GetBorrowerPairs();
          bool isPrimaryPair = true;
          foreach (BorrowerPair pair in borrowerPairs)
          {
            this.InsertBorrowerPairHistoryOnLoanClosed(loanId, loan, pair, loid, isPrimaryPair, interactive);
            isPrimaryPair = false;
          }
        }
        for (int index1 = 0; index1 < ContactUtil.BizContactNameEmidsByCate.Length; ++index1)
        {
          string[] strArray = ContactUtil.BizContactNameEmidsByCate[index1];
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            string companyEmid = (string) ContactUtil._NameEmidToCompanyEmid[(object) strArray[index2]];
            this.DoBizContactInClosedLoan(loanId, loan, strArray[index2], companyEmid, this.GetCategoryID(index1), loid);
          }
        }
      }
    }

    public int GetBorIdFromLoan(LoanData loan, BorrowerPair pair)
    {
      return this.GetBorIdFromLoan(loan, false, pair);
    }

    public int GetBorIdFromLoan(LoanData loan, bool isCoBorrower, BorrowerPair pair)
    {
      return this.GetBorIdFromLoan(loan, isCoBorrower, pair, false);
    }

    public int GetBorIdFromLoan(LoanData loan)
    {
      return this.GetBorIdFromLoan(loan, false, (BorrowerPair) null);
    }

    public int GetBorIdFromLoan(
      LoanData loan,
      bool isCoBorrower,
      BorrowerPair pair,
      bool matchCurrentUserAsOwnerAlso)
    {
      string loid = this.makeBestGuessOfContactOwnerFromLoan(loan, matchCurrentUserAsOwnerAlso);
      if (loid == string.Empty)
        return 0;
      string stringLoanField1 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorLastNameFieldId : ContactUtil._BorLastNameFieldId, pair);
      string stringLoanField2 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorMiddleNameFieldId : ContactUtil._BorMiddleNameFieldId, pair);
      string stringLoanField3 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorFirstNameFieldId : ContactUtil._BorFirstNameFieldId, pair);
      string stringLoanField4 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorSuffixNameFieldId : ContactUtil._BorSuffixNameFieldId, pair);
      BorrowerInfo borrowerByName = this.findBorrowerByName(stringLoanField3, stringLoanField2, stringLoanField1, stringLoanField4, loid);
      if (borrowerByName != null)
        return borrowerByName.ContactID;
      Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Borrower contact not found. Borrower fist name is " + stringLoanField3 + ". Borrower middle name is " + stringLoanField2 + ". Borrower last name is " + stringLoanField1 + ". Loan info is not inserted into Loan table.");
      return 0;
    }

    private int GetLinkedBorIdFromLoan(LoanData loan)
    {
      CRMLog[] allCrmMapping = loan.GetLogList().GetAllCRMMapping();
      string contactGuid = "";
      foreach (CRMLog crmLog in allCrmMapping)
      {
        if (crmLog.MappingType != CRMLogType.BusinessContact && crmLog.MappingID == "_borrower1")
        {
          contactGuid = crmLog.ContactGuid;
          break;
        }
      }
      if (contactGuid != "")
      {
        BorrowerInfo borrower = this.ContactManager.GetBorrower(contactGuid);
        if (borrower != null)
          return borrower.ContactID;
      }
      return -1;
    }

    public int GetCoBorIdFromLoan(LoanData loan, BorrowerPair pair)
    {
      return this.GetBorIdFromLoan(loan, true, pair);
    }

    public int GetCoBorIdFromLoan(LoanData loan)
    {
      return this.GetBorIdFromLoan(loan, true, (BorrowerPair) null);
    }

    public void CreateBorrowersForLoan(LoanData loan, ContactSource contactSource)
    {
      string loid = this.makeBestGuessOfContactOwnerFromLoan(loan, true);
      if (loid == string.Empty)
        return;
      DateTime loanStartedDate = this.getLoanStartedDate(loan);
      BorrowerPair[] borrowerPairs = loan.GetBorrowerPairs();
      loan.SetBorrowerPair(borrowerPairs[0]);
      bool foundBySSN = true;
      bool isPrimaryPair = true;
      foreach (BorrowerPair pair in borrowerPairs)
      {
        BorrowerInfo[] borrowerContactForLoan1 = this.findBorrowerContactForLoan(ref foundBySSN, loan, false, pair, loid);
        if (borrowerContactForLoan1 == null || borrowerContactForLoan1.Length == 0)
        {
          BorrowerInfo borrowerInfoFromLoan = this.GetBorrowerInfoFromLoan(loan, false, pair, isPrimaryPair);
          isPrimaryPair = false;
          if (borrowerInfoFromLoan.FirstName.Trim() != string.Empty && borrowerInfoFromLoan.LastName.Trim() != string.Empty)
          {
            borrowerInfoFromLoan.OwnerID = loid;
            BorrowerInfo borrowerInfo = this.ContactManager.CreateBorrowerInfo(borrowerInfoFromLoan, loanStartedDate, contactSource);
            loan.GetLogList().AddCRMMapping(pair.Borrower.Id, CRMLogType.BorrowerContact, borrowerInfo.ContactGuid.ToString(), CRMRoleType.Borrower);
          }
        }
        BorrowerInfo[] borrowerContactForLoan2 = this.findBorrowerContactForLoan(ref foundBySSN, loan, true, pair, loid);
        if (borrowerContactForLoan2 == null || borrowerContactForLoan2.Length == 0)
        {
          BorrowerInfo borrowerInfoFromLoan = this.GetBorrowerInfoFromLoan(loan, true, pair, false);
          if (borrowerInfoFromLoan.FirstName.Trim() != string.Empty && borrowerInfoFromLoan.LastName.Trim() != string.Empty)
          {
            borrowerInfoFromLoan.OwnerID = loid;
            BorrowerInfo borrowerInfo = this.ContactManager.CreateBorrowerInfo(borrowerInfoFromLoan, loanStartedDate, contactSource);
            loan.GetLogList().AddCRMMapping(pair.CoBorrower.Id, CRMLogType.BorrowerContact, borrowerInfo.ContactGuid.ToString(), CRMRoleType.Coborrower);
          }
        }
      }
    }

    public static string CateAbbrToName(string cateAbbr)
    {
      if (cateAbbr == null || cateAbbr == string.Empty)
        cateAbbr = "all";
      return (string) ContactUtil.cateAbbrToNameTbl[(object) cateAbbr] ?? "All";
    }

    public static string[] GetFirstLastName(string name)
    {
      if (name == null)
        return new string[2]{ string.Empty, string.Empty };
      if (name == string.Empty)
        return new string[2]{ string.Empty, string.Empty };
      name = name.Trim();
      Match match = ContactUtil.nameDelimiter.Match(name);
      if (match.Success)
      {
        int length = name.LastIndexOf(match.Value);
        return new string[2]
        {
          name.Substring(0, length),
          name.Substring(length + match.Value.Length)
        };
      }
      return new string[2]{ name, string.Empty };
    }

    public int InsertLoanOnLoanClosed(LoanData loan)
    {
      ContactLoanInfo loanInfoFromLoan = this.GetContactLoanInfoFromLoan(loan);
      if (loanInfoFromLoan != null)
        return this.ContactManager.AddContactLoan(loanInfoFromLoan);
      Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Loan record is not inserted into the Loan table.");
      return 0;
    }

    private ContactLoanInfo insertLinkedLoanOnLoanClosed(LoanData loan)
    {
      ContactLoanInfo loanInfoFromLoan = this.GetLinkedContactLoanInfoFromLoan(loan);
      if (loanInfoFromLoan != null)
        return this.ContactManager.GetContactLoan(this.ContactManager.AddContactLoan(loanInfoFromLoan));
      Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Loan record is not inserted into the Loan table.");
      return (ContactLoanInfo) null;
    }

    private BorrowerInfo[] findBorrowerContactForLoan(
      ref bool foundBySSN,
      LoanData loan,
      bool isCoBorrower,
      BorrowerPair pair,
      string loid)
    {
      foundBySSN = false;
      BorrowerInfo[] borrowerInfoArray = new BorrowerInfo[0];
      BorrowerInfo[] contactsBySsn = this.findContactsBySSN(this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorSSNFieldId : ContactUtil._BorSSNFieldId, pair), loid);
      if (contactsBySsn.Length != 0)
      {
        foundBySSN = true;
        return contactsBySsn;
      }
      string stringLoanField1 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorLastNameFieldId : ContactUtil._BorLastNameFieldId, pair);
      string stringLoanField2 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorSuffixNameFieldId : ContactUtil._BorSuffixNameFieldId, pair);
      return this.findContactsByName(this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorFirstNameFieldId : ContactUtil._BorFirstNameFieldId, pair), this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorMiddleNameFieldId : ContactUtil._BorMiddleNameFieldId, pair), stringLoanField1, stringLoanField2, loid);
    }

    private void InsertBorrowerPairHistoryOnLoanClosed(
      int loanId,
      LoanData loan,
      BorrowerPair pair,
      string loid,
      bool isPrimaryPair,
      bool interactive)
    {
      bool foundBySSN1 = true;
      bool foundBySSN2 = true;
      bool hasCoborrower = true;
      string stringLoanField1 = this.GetStringLoanField(loan, ContactUtil._CoBorLastNameFieldId, pair);
      string stringLoanField2 = this.GetStringLoanField(loan, ContactUtil._CoBorFirstNameFieldId, pair);
      if (stringLoanField1.Trim() == string.Empty || stringLoanField2.Trim() == string.Empty)
        hasCoborrower = false;
      BorrowerInfo[] borrowerContactForLoan = this.findBorrowerContactForLoan(ref foundBySSN1, loan, false, pair, loid);
      BorrowerInfo[] coborrowers = new BorrowerInfo[0];
      if (hasCoborrower)
        coborrowers = this.findBorrowerContactForLoan(ref foundBySSN2, loan, true, pair, loid);
      bool allowAddressUpdate = false;
      bool flag1 = true;
      bool flag2 = true;
      if (isPrimaryPair && this.GetStringLoanField(loan, "19") == "Purchase" & this.GetStringLoanField(loan, "1811") == "PrimaryResidence")
      {
        if (this.fromLoanImport)
        {
          MilestoneLog milestoneLog = (MilestoneLog) null;
          foreach (MilestoneLog allMilestone in loan.GetLogList().GetAllMilestones())
          {
            if (allMilestone.Done)
              milestoneLog = allMilestone;
            else
              break;
          }
          if (milestoneLog != null)
          {
            if (borrowerContactForLoan != null && borrowerContactForLoan.Length != 0)
            {
              ContactLoanInfo[] borrowerLoans = this.ContactManager.GetBorrowerLoans(borrowerContactForLoan[0].ContactID);
              if (borrowerLoans != null)
              {
                bool flag3 = false;
                foreach (ContactLoanInfo contactLoanInfo in borrowerLoans)
                {
                  if (contactLoanInfo.Purpose == EllieMae.EMLite.Common.Contact.LoanPurpose.Purchase && contactLoanInfo.LoanStatus != "Started" && contactLoanInfo.DateCompleted > milestoneLog.Date)
                  {
                    flag3 = true;
                    break;
                  }
                }
                if (flag3)
                  flag1 = false;
              }
            }
            else
              flag1 = false;
            if (coborrowers != null && coborrowers.Length != 0)
            {
              ContactLoanInfo[] borrowerLoans = this.ContactManager.GetBorrowerLoans(coborrowers[0].ContactID);
              if (borrowerLoans != null)
              {
                bool flag4 = false;
                foreach (ContactLoanInfo contactLoanInfo in borrowerLoans)
                {
                  if (contactLoanInfo.Purpose == EllieMae.EMLite.Common.Contact.LoanPurpose.Purchase && contactLoanInfo.LoanStatus != "Started" && contactLoanInfo.DateCompleted > milestoneLog.Date)
                  {
                    flag4 = true;
                    break;
                  }
                }
                if (flag4)
                  flag2 = false;
              }
            }
            else
              flag2 = false;
            allowAddressUpdate = flag1 | flag2;
          }
          else
          {
            flag1 = false;
            flag2 = false;
            allowAddressUpdate = false;
          }
        }
        else
          allowAddressUpdate = true;
      }
      bool flag5 = true;
      bool showCreateNew = true;
      if (foundBySSN1 & foundBySSN2 & borrowerContactForLoan.Length == 1 & coborrowers.Length == 1)
      {
        flag5 = isPrimaryPair;
        showCreateNew = false;
      }
      if (!interactive)
        flag5 = false;
      bool allowUpdateOptOut = false;
      if (((IFeaturesAclManager) this.sessionObjects.Session.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.Cnt_Contacts_Update, this.sessionObjects.Session.UserID))
        allowUpdateOptOut = true;
      BorrowerInfo borrowerInfo1 = (BorrowerInfo) null;
      BorrowerInfo borrowerInfo2 = (BorrowerInfo) null;
      bool flag6 = false;
      if (flag5)
      {
        ContactSelectionDlg contactSelectionDlg = new ContactSelectionDlg(borrowerContactForLoan, coborrowers, allowAddressUpdate, showCreateNew, hasCoborrower, allowUpdateOptOut);
        if (DialogResult.Cancel == contactSelectionDlg.ShowDialog((IWin32Window) null))
          return;
        if (contactSelectionDlg.CreateBorrower)
        {
          BorrowerInfo borrowerInfoFromLoan = this.GetBorrowerInfoFromLoan(loan, false, pair, isPrimaryPair);
          borrowerInfoFromLoan.OwnerID = loid;
          DateTime loanStartedDate = this.getLoanStartedDate(loan);
          borrowerInfoFromLoan.ContactID = this.ContactManager.CreateBorrower(borrowerInfoFromLoan, loanStartedDate, ContactSource.NotAvailable);
          borrowerInfoFromLoan.ContactType = BorrowerType.Client;
          borrowerInfo1 = borrowerInfoFromLoan;
        }
        else
          borrowerInfo1 = contactSelectionDlg.SelectedBorrower;
        if (hasCoborrower)
        {
          if (contactSelectionDlg.CreateCoBorrower)
          {
            BorrowerInfo borrowerInfoFromLoan = this.GetBorrowerInfoFromLoan(loan, true, pair, isPrimaryPair);
            borrowerInfoFromLoan.OwnerID = loid;
            DateTime loanStartedDate = this.getLoanStartedDate(loan);
            borrowerInfoFromLoan.ContactID = this.ContactManager.CreateBorrower(borrowerInfoFromLoan, loanStartedDate, ContactSource.NotAvailable);
            borrowerInfoFromLoan.ContactType = BorrowerType.Client;
            borrowerInfo2 = borrowerInfoFromLoan;
          }
          else
            borrowerInfo2 = contactSelectionDlg.SelectedCoBorrower;
        }
        flag6 = contactSelectionDlg.UpdateAddress;
      }
      else if (!interactive)
      {
        if (borrowerContactForLoan.Length == 1)
          borrowerInfo1 = borrowerContactForLoan[0];
        if (hasCoborrower && coborrowers.Length == 1)
          borrowerInfo2 = coborrowers[0];
        flag6 = allowAddressUpdate;
      }
      else
      {
        if (borrowerContactForLoan.Length != 0)
          borrowerInfo1 = borrowerContactForLoan[0];
        if (hasCoborrower && coborrowers.Length != 0)
          borrowerInfo2 = coborrowers[0];
      }
      if (borrowerInfo1 == null & borrowerInfo2 == null)
        return;
      bool flag7 = flag6;
      bool flag8 = flag6;
      if (borrowerInfo1 != null && borrowerInfo1.ContactType != BorrowerType.Client)
      {
        borrowerInfo1.ContactType = BorrowerType.Client;
        flag7 = true;
      }
      if (borrowerInfo2 != null && borrowerInfo2.ContactType != BorrowerType.Client)
      {
        borrowerInfo2.ContactType = BorrowerType.Client;
        flag8 = true;
      }
      if (flag6)
      {
        EllieMae.EMLite.ClientServer.Address address = new EllieMae.EMLite.ClientServer.Address();
        address.Street1 = this.GetStringLoanField(loan, "11");
        address.Street2 = string.Empty;
        address.City = this.GetStringLoanField(loan, "12");
        address.State = this.GetStringLoanField(loan, "14");
        address.Zip = this.GetStringLoanField(loan, "15");
        if (borrowerInfo1 != null & flag1)
        {
          borrowerInfo1.HomeAddress.Street1 = address.Street1;
          borrowerInfo1.HomeAddress.Street2 = address.Street2;
          borrowerInfo1.HomeAddress.City = address.City;
          borrowerInfo1.HomeAddress.State = address.State;
          borrowerInfo1.HomeAddress.Zip = address.Zip;
        }
        if (borrowerInfo2 != null & flag2)
        {
          borrowerInfo2.HomeAddress.Street1 = address.Street1;
          borrowerInfo2.HomeAddress.Street2 = address.Street2;
          borrowerInfo2.HomeAddress.City = address.City;
          borrowerInfo2.HomeAddress.State = address.State;
          borrowerInfo2.HomeAddress.Zip = address.Zip;
        }
      }
      if (borrowerInfo1 != null)
      {
        if (flag7)
          this.ContactManager.UpdateBorrower(borrowerInfo1);
        this.updateContactCustomFields(borrowerInfo1, loan);
      }
      if (borrowerInfo2 != null)
      {
        if (flag8)
          this.ContactManager.UpdateBorrower(borrowerInfo2);
        this.updateContactCustomFields(borrowerInfo2, loan);
      }
      ContactLoanInfo contactLoan = this.ContactManager.GetContactLoan(loanId);
      if (contactLoan == null)
        return;
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem("Completed Loan", contactLoan.DateCompleted, loanId);
      if (borrowerInfo1 != null)
        this.ContactManager.AddHistoryItemForContact(borrowerInfo1.ContactID, ContactType.Borrower, contactHistoryItem);
      if (borrowerInfo2 == null)
        return;
      this.ContactManager.AddHistoryItemForContact(borrowerInfo2.ContactID, ContactType.Borrower, contactHistoryItem);
    }

    private void InsertLinkedBorrowerHistoryOnLoanClosed(
      ContactLoanInfo contactLoan,
      LoanData loan,
      string[] contactGuids,
      bool interactive)
    {
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem("Completed Loan", contactLoan.DateCompleted, contactLoan.LoanID);
      List<string> stringList = new List<string>();
      foreach (string contactGuid in contactGuids)
      {
        BorrowerInfo borrower = this.ContactManager.GetBorrower(contactGuid);
        if (borrower != null)
          stringList.Add(borrower.ContactGuid.ToString());
      }
      if (stringList.Count == 0)
        return;
      this.ContactManager.AddHistoryItemForContacts(stringList.ToArray(), ContactType.Borrower, contactHistoryItem);
    }

    private void InsertLinkedBizPartnerHistoryOnLoanClosed(
      ContactLoanInfo contactLoan,
      LoanData loan,
      string[] contactGuids,
      bool interactive)
    {
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem("Completed Loan", contactLoan.DateCompleted, contactLoan.LoanID);
      BizPartnerInfo[] bizPartners = this.ContactManager.GetBizPartners(contactGuids);
      List<string> stringList = new List<string>();
      foreach (BizPartnerInfo bizPartnerInfo in bizPartners)
        stringList.Add(bizPartnerInfo.ContactGuid.ToString());
      if (stringList.Count == 0)
        return;
      this.ContactManager.AddHistoryItemForContacts(stringList.ToArray(), ContactType.BizPartner, contactHistoryItem);
    }

    private DateTime getLoanClosedDate(LoanData data)
    {
      MilestoneLog milestone = data.GetLogList().GetMilestone("Completion");
      return milestone == null ? DateTime.Now : milestone.Date;
    }

    private BorrowerInfo[] findContactsByName(
      string firstName,
      string middleName,
      string lastName,
      string suffixName,
      string loid)
    {
      if (firstName == null)
      {
        if (lastName == null)
          return new BorrowerInfo[0];
        if (lastName.Trim() == string.Empty)
          return new BorrowerInfo[0];
      }
      if (firstName.Trim() == string.Empty)
      {
        if (lastName == null)
          return new BorrowerInfo[0];
        if (lastName.Trim() == string.Empty)
          return new BorrowerInfo[0];
      }
      return this.ContactManager.QueryAllBorrowers(new QueryCriterion[5]
      {
        (QueryCriterion) new StringValueCriterion("Contact.FirstName", firstName),
        (QueryCriterion) new StringValueCriterion("Contact.MiddleName", middleName),
        (QueryCriterion) new StringValueCriterion("Contact.LastName", lastName),
        (QueryCriterion) new StringValueCriterion("Contact.SuffixName", suffixName),
        (QueryCriterion) new StringValueCriterion("Contact.OwnerID", loid)
      }, RelatedLoanMatchType.None);
    }

    private BorrowerInfo[] findContactsBySSN(string ssn, string loid)
    {
      if (ssn == null)
        return new BorrowerInfo[0];
      if (ssn.Trim() == string.Empty)
        return new BorrowerInfo[0];
      return this.ContactManager.QueryAllBorrowers(new QueryCriterion[2]
      {
        (QueryCriterion) new StringValueCriterion("Contact.SSN", ssn),
        (QueryCriterion) new StringValueCriterion("Contact.OwnerID", loid)
      }, RelatedLoanMatchType.None);
    }

    private BorrowerInfo findBorrowerByName(
      string firstName,
      string middleName,
      string lastName,
      string suffixName,
      string loid)
    {
      BorrowerInfo[] borrowerInfoArray = this.ContactManager.QueryAllBorrowers(new QueryCriterion[5]
      {
        (QueryCriterion) new StringValueCriterion("Contact.FirstName", firstName),
        (QueryCriterion) new StringValueCriterion("Contact.MiddleName", middleName),
        (QueryCriterion) new StringValueCriterion("Contact.LastName", lastName),
        (QueryCriterion) new StringValueCriterion("Contact.SuffixName", suffixName),
        (QueryCriterion) new StringValueCriterion("Contact.OwnerID", loid)
      }, RelatedLoanMatchType.None);
      return borrowerInfoArray.Length == 0 ? (BorrowerInfo) null : borrowerInfoArray[0];
    }

    private DateTime getLoanStartedDate(LoanData data)
    {
      MilestoneLog milestone = data.GetLogList().GetMilestone("Started");
      return milestone == null || milestone.Date == DateTime.MaxValue ? DateTime.Now : milestone.Date;
    }

    private BorrowerInfo GetBorrowerInfoFromLoan(
      LoanData loan,
      bool isCoBorrower,
      BorrowerPair pair,
      bool isPrimaryPair)
    {
      BorrowerInfo borrowerInfoFromLoan = new BorrowerInfo();
      borrowerInfoFromLoan.PrimaryContact = !isCoBorrower && isPrimaryPair;
      borrowerInfoFromLoan.LastName = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorLastNameFieldId : ContactUtil._BorLastNameFieldId, pair);
      borrowerInfoFromLoan.SuffixName = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorSuffixNameFieldId : ContactUtil._BorSuffixNameFieldId, pair);
      borrowerInfoFromLoan.FirstName = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorFirstNameFieldId : ContactUtil._BorFirstNameFieldId, pair);
      borrowerInfoFromLoan.MiddleName = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._CoBorMiddleNameFieldId : ContactUtil._BorMiddleNameFieldId, pair);
      borrowerInfoFromLoan.HomeAddress.Street1 = this.GetStringLoanField(loan, isCoBorrower ? "FR0204" : "FR0104", pair);
      borrowerInfoFromLoan.HomeAddress.Street2 = string.Empty;
      borrowerInfoFromLoan.HomeAddress.City = this.GetStringLoanField(loan, isCoBorrower ? "FR0206" : "FR0106", pair);
      borrowerInfoFromLoan.HomeAddress.State = this.GetStringLoanField(loan, isCoBorrower ? "FR0207" : "FR0107", pair);
      borrowerInfoFromLoan.HomeAddress.Zip = this.GetStringLoanField(loan, isCoBorrower ? "FR0208" : "FR0108", pair);
      borrowerInfoFromLoan.BizAddress.Street1 = this.GetStringLoanField(loan, isCoBorrower ? "FE0204" : "FE0104", pair);
      borrowerInfoFromLoan.BizAddress.Street2 = string.Empty;
      borrowerInfoFromLoan.BizAddress.City = this.GetStringLoanField(loan, isCoBorrower ? "FE0205" : "FE0105", pair);
      borrowerInfoFromLoan.BizAddress.State = this.GetStringLoanField(loan, isCoBorrower ? "FE0206" : "FE0106", pair);
      borrowerInfoFromLoan.BizAddress.Zip = this.GetStringLoanField(loan, isCoBorrower ? "FE0207" : "FE0107", pair);
      borrowerInfoFromLoan.BizWebUrl = string.Empty;
      borrowerInfoFromLoan.EmployerName = this.GetStringLoanField(loan, isCoBorrower ? "FE0202" : "FE0102", pair);
      borrowerInfoFromLoan.JobTitle = this.GetStringLoanField(loan, isCoBorrower ? "FE0210" : "FE0110", pair);
      borrowerInfoFromLoan.WorkPhone = this.GetStringLoanField(loan, isCoBorrower ? "FE0217" : "FE0117", pair);
      borrowerInfoFromLoan.HomePhone = this.GetStringLoanField(loan, isCoBorrower ? "98" : "66", pair);
      borrowerInfoFromLoan.MobilePhone = this.GetStringLoanField(loan, isCoBorrower ? "1480" : "1490", pair);
      borrowerInfoFromLoan.FaxNumber = this.GetStringLoanField(loan, isCoBorrower ? "1241" : "1188", pair);
      borrowerInfoFromLoan.PersonalEmail = this.GetStringLoanField(loan, isCoBorrower ? "1268" : "1240", pair);
      borrowerInfoFromLoan.BizEmail = string.Empty;
      borrowerInfoFromLoan.SSN = this.GetStringLoanField(loan, isCoBorrower ? "97" : "65", pair);
      borrowerInfoFromLoan.Referral = this.GetStringLoanField(loan, "1822", pair);
      borrowerInfoFromLoan.Income = 12M * this.GetDecimalLoanField(loan, isCoBorrower ? "911" : "910");
      if (this.GetStringLoanField(loan, isCoBorrower ? "84" : "52", pair).Trim() == "Married")
      {
        borrowerInfoFromLoan.Married = true;
        string stringLoanField1 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._BorFirstNameFieldId : ContactUtil._CoBorFirstNameFieldId, pair);
        string stringLoanField2 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._BorMiddleNameFieldId : ContactUtil._CoBorMiddleNameFieldId, pair);
        string stringLoanField3 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._BorLastNameFieldId : ContactUtil._CoBorLastNameFieldId, pair);
        string stringLoanField4 = this.GetStringLoanField(loan, isCoBorrower ? ContactUtil._BorSuffixNameFieldId : ContactUtil._CoBorSuffixNameFieldId, pair);
        borrowerInfoFromLoan.SpouseName = stringLoanField1 + " " + stringLoanField2 + " " + stringLoanField3 + " " + stringLoanField4;
        borrowerInfoFromLoan.SpouseContactID = 0;
      }
      else
      {
        borrowerInfoFromLoan.Married = false;
        borrowerInfoFromLoan.SpouseContactID = 0;
        borrowerInfoFromLoan.SpouseName = string.Empty;
      }
      borrowerInfoFromLoan.Anniversary = DateTime.MinValue;
      try
      {
        string simpleField = loan.GetSimpleField(isCoBorrower ? "1403" : "1402", pair);
        DateTime dateTime = Convert.ToDateTime(simpleField);
        if (dateTime > Utils.DbMinSmallDate)
        {
          if (dateTime < Utils.DbMaxSmallDate)
          {
            if (simpleField.IndexOf(dateTime.Year.ToString()) == -1 && dateTime.Year >= DateTime.Today.Year && dateTime.Year - 100 < DateTime.Today.Year)
              dateTime = dateTime.AddYears(-100);
            borrowerInfoFromLoan.Birthdate = dateTime;
          }
        }
      }
      catch
      {
      }
      borrowerInfoFromLoan.OwnerID = this.sessionObjects.UserID;
      borrowerInfoFromLoan.AccessLevel = ContactAccess.Private;
      borrowerInfoFromLoan.CustField1 = string.Empty;
      borrowerInfoFromLoan.CustField2 = string.Empty;
      borrowerInfoFromLoan.CustField3 = string.Empty;
      borrowerInfoFromLoan.CustField4 = string.Empty;
      borrowerInfoFromLoan.PrimaryEmail = string.Empty;
      borrowerInfoFromLoan.PrimaryPhone = string.Empty;
      borrowerInfoFromLoan.NoSpam = false;
      borrowerInfoFromLoan.Salutation = string.Empty;
      return borrowerInfoFromLoan;
    }

    private string makeBestGuessOfContactOwnerFromLoan(
      LoanData loan,
      bool matchCurrentUserAsOwnerAlso)
    {
      string userId = loan.GetField("LOID").Trim();
      if (this.userExists(userId))
        return userId;
      string str = this.makeBestGuessOfContactOwnerByName(this.GetStringLoanField(loan, "317"));
      return (str ?? "") != string.Empty ? str : this.sessionObjects.UserInfo.Userid;
    }

    private bool userExists(string userId)
    {
      return userId != null && !(userId == string.Empty) && this.OrganizationManager.GetUser(userId) != (UserInfo) null;
    }

    private string makeBestGuessOfContactOwnerByName(string userName)
    {
      UserInfo[] matchedUsersFromName = this.getMatchedUsersFromName(userName);
      return matchedUsersFromName != null && matchedUsersFromName.Length != 0 ? matchedUsersFromName[0].Userid : (string) null;
    }

    private UserInfo[] getMatchedUsersFromName(string userName)
    {
      if (userName == null)
        return (UserInfo[]) null;
      if (userName == string.Empty)
        return (UserInfo[]) null;
      string[] firstLastName = ContactUtil.GetFirstLastName(userName);
      UserInfo[] matchedUsersFromName = (UserInfo[]) null;
      if ((firstLastName[0] ?? "") != "" && (firstLastName[1] ?? "") != "")
        matchedUsersFromName = this.OrganizationManager.GetUsersByName(firstLastName[0], firstLastName[1]);
      return matchedUsersFromName;
    }

    private ContactLoanInfo GetContactLoanInfoFromLoan(LoanData loan)
    {
      int borIdFromLoan = this.GetBorIdFromLoan(loan, false, (BorrowerPair) null, true);
      return this.GetContactLoanInfoFromLoan(loan, borIdFromLoan);
    }

    private ContactLoanInfo GetLinkedContactLoanInfoFromLoan(LoanData loan)
    {
      int linkedBorIdFromLoan = this.GetLinkedBorIdFromLoan(loan);
      return this.GetContactLoanInfoFromLoan(loan, linkedBorIdFromLoan);
    }

    private ContactLoanInfo GetContactLoanInfoFromLoan(LoanData loan, int borID)
    {
      ContactLoanInfo loanInfoFromLoan = new ContactLoanInfo();
      loanInfoFromLoan.BorrowerID = borID;
      loanInfoFromLoan.AppraisedValue = this.GetDecimalLoanField(loan, ContactUtil._AppraisedValueFieldId);
      loanInfoFromLoan.LoanAmount = this.GetDecimalLoanField(loan, ContactUtil._LoanAmountFieldId);
      loanInfoFromLoan.InterestRate = this.GetDecimalLoanField(loan, ContactUtil._InterestRateFieldId);
      loanInfoFromLoan.Term = this.GetIntLoanField(loan, ContactUtil._TermFieldId);
      loanInfoFromLoan.Purpose = LoanPurposeEnumUtil.NameInLoanToValue(this.GetStringLoanField(loan, ContactUtil._PurposeFieldId));
      loanInfoFromLoan.DownPayment = this.GetDecimalLoanField(loan, ContactUtil._DownPaymentFieldId);
      loanInfoFromLoan.LTV = this.GetDecimalLoanField(loan, ContactUtil._LTVFieldId);
      loanInfoFromLoan.Amortization = AmortizationTypeEnumUtil.NameInLoanToValue(this.GetStringLoanField(loan, ContactUtil._AmortizationFieldId));
      loanInfoFromLoan.LoanType = LoanTypeEnumUtil.NameInLoanToValue(this.GetStringLoanField(loan, ContactUtil._LoanTypeFieldId));
      loanInfoFromLoan.LienPosition = LienEnumUtil.NameInLoanToValue(this.GetStringLoanField(loan, ContactUtil._LienPositionFieldId));
      string str = string.Concat(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.ContactUpdateMilestone"]);
      MilestoneLog milestoneLog = (MilestoneLog) null;
      foreach (MilestoneLog allMilestone in loan.GetLogList().GetAllMilestones())
      {
        if (allMilestone.MilestoneID == str)
        {
          milestoneLog = allMilestone;
          break;
        }
      }
      if (milestoneLog != null)
      {
        loanInfoFromLoan.LoanStatus = milestoneLog.Stage;
        loanInfoFromLoan.DateCompleted = milestoneLog.Date;
      }
      return loanInfoFromLoan;
    }

    private void updateContactCustomFields(BorrowerInfo contact, LoanData loanData)
    {
      Tracing.Log(ContactUtil.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("Entering LoanUtils.ContactUI.ContactUtil.updateContactCustomFields(contact.FirstName='{0}', contact.LastName='{1}')", (object) contact.FirstName, (object) contact.LastName));
      CustomFieldsMappingInfo customFieldsMapping = this.ContactManager.GetCustomFieldsMapping(CustomFieldsType.Borrower, 0, true);
      if (customFieldsMapping == null || customFieldsMapping.CustomFieldMappings.Length == 0)
        return;
      List<ContactCustomField> contactCustomFieldList = new List<ContactCustomField>();
      foreach (CustomFieldMappingInfo customFieldMapping in customFieldsMapping.CustomFieldMappings)
      {
        string fieldValue = (string) null;
        try
        {
          fieldValue = loanData.GetField(customFieldMapping.LoanFieldId);
          Tracing.Log(ContactUtil.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("CustomFieldMapping: CategoryId='{0}', FieldNumber='{1}', FieldFormat='{2}', LoanFieldId='{3}', FieldValue='{4}'", (object) customFieldMapping.CategoryId, (object) customFieldMapping.FieldNumber, (object) customFieldMapping.FieldFormat, (object) customFieldMapping.LoanFieldId, (object) fieldValue));
        }
        catch (Exception ex)
        {
          Tracing.Log(ContactUtil.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{2}', Value '{1}' to Borrower Contact 'Custom Field {0}' failed.", (object) customFieldMapping.FieldNumber.ToString(), fieldValue == null ? (object) "UNKNOWN" : (object) fieldValue, (object) customFieldMapping.LoanFieldId));
          fieldValue = (string) null;
        }
        if (fieldValue != null)
        {
          ContactCustomField contactCustomField = new ContactCustomField(contact.ContactID, customFieldMapping.FieldNumber, contact.OwnerID, fieldValue);
          contactCustomFieldList.Add(contactCustomField);
        }
      }
      if (contactCustomFieldList.Count == 0)
        return;
      this.ContactManager.UpdateCustomFieldsForContact(contact.ContactID, ContactType.Borrower, contactCustomFieldList.ToArray());
    }

    private BizPartnerInfo findBizPartnerByNameAndCompany(
      string firstName,
      string lastName,
      string companyName,
      int categoryId,
      string loid)
    {
      bool flag = true;
      if (companyName == null || companyName.Trim() == string.Empty)
        flag = false;
      QueryCriterion[] criteria = !flag ? new QueryCriterion[3] : new QueryCriterion[4];
      criteria[0] = (QueryCriterion) new StringValueCriterion("Contact.FirstName", firstName);
      criteria[1] = (QueryCriterion) new StringValueCriterion("Contact.LastName", lastName);
      criteria[2] = (QueryCriterion) new OrdinalValueCriterion("Contact.CategoryID", (object) categoryId);
      if (flag)
        criteria[3] = (QueryCriterion) new StringValueCriterion("Contact.CompanyName", companyName.Trim());
      BizPartnerInfo[] bizPartnerInfoArray = this.ContactManager.QueryBizPartners(criteria, RelatedLoanMatchType.None);
      return bizPartnerInfoArray.Length == 0 ? (BizPartnerInfo) null : bizPartnerInfoArray[0];
    }

    private void DoBizContactInClosedLoan(
      int loanId,
      LoanData loan,
      string contactNameEmid,
      string companyEmid,
      int cateId,
      string loid)
    {
      string stringLoanField1 = this.GetStringLoanField(loan, contactNameEmid);
      string stringLoanField2 = this.GetStringLoanField(loan, companyEmid);
      string[] firstLastName = ContactUtil.GetFirstLastName(stringLoanField1);
      BizPartnerInfo byNameAndCompany = this.findBizPartnerByNameAndCompany(firstLastName[0], firstLastName[1], stringLoanField2, cateId, loid);
      if (byNameAndCompany == null)
        return;
      ContactLoanInfo contactLoan = this.ContactManager.GetContactLoan(loanId);
      if (contactLoan == null)
        return;
      ContactHistoryItem contactHistoryItem = new ContactHistoryItem("Completed Loan", contactLoan.DateCompleted, loanId);
      this.ContactManager.AddHistoryItemForContact(byNameAndCompany.ContactID, ContactType.BizPartner, contactHistoryItem);
    }

    private DateTime GetSmallDateLoanField(LoanData loan, string fieldId, BorrowerPair pair)
    {
      string simpleField = loan.GetSimpleField(fieldId, pair);
      DateTime dateTime = new DateTime(1900, 1, 1);
      DateTime minValue;
      try
      {
        minValue = DateTime.Parse(simpleField);
      }
      catch (Exception ex)
      {
        minValue = DateTime.MinValue;
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Field value from loan file is not a valid date. " + ex.ToString());
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Loan fieldId is " + fieldId);
      }
      if (minValue.Year < 1900 || minValue.Year > 2078)
        minValue = DateTime.MinValue;
      return minValue;
    }

    private string GetStringLoanField(LoanData loan, string fieldId, BorrowerPair pair)
    {
      return loan.GetSimpleField(fieldId, pair);
    }

    private string GetStringLoanField(LoanData loan, string fieldId)
    {
      return loan.GetSimpleField(fieldId);
    }

    private int GetIntLoanField(LoanData loan, string fieldId)
    {
      string simpleField = loan.GetSimpleField(fieldId);
      int intLoanField;
      try
      {
        intLoanField = Convert.ToInt32(simpleField);
      }
      catch (Exception ex)
      {
        intLoanField = 0;
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Field value from loan file is not valid. " + ex.ToString());
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Loan fieldId is " + fieldId);
      }
      return intLoanField;
    }

    private Decimal GetDecimalLoanField(LoanData loan, string fieldId)
    {
      string simpleField = loan.GetSimpleField(fieldId);
      Decimal decimalLoanField;
      try
      {
        decimalLoanField = Convert.ToDecimal(simpleField);
      }
      catch (Exception ex)
      {
        decimalLoanField = 0M;
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Field value from loan file is not valid. " + ex.ToString());
        Tracing.Log(ContactUtil.sw, TraceLevel.Warning, ContactUtil.className, "Loan fieldId is " + fieldId);
      }
      return decimalLoanField;
    }

    private IContactManager ContactManager => this.sessionObjects.ContactManager;

    private IConfigurationManager ConfigurationManager => this.sessionObjects.ConfigurationManager;

    private IServerManager ServerManager => this.sessionObjects.ServerManager;

    private IOrganizationManager OrganizationManager => this.sessionObjects.OrganizationManager;

    public bool SetContactFieldsInLoan(
      LoanData loanData,
      BorrowerInfo borrower,
      BorrowerInfo coBorrower)
    {
      if (borrower != null)
        this.setBorrowerFieldsInLoan(loanData, borrower);
      if (coBorrower != null)
        this.setCoborrowerFieldsInLoan(loanData, coBorrower);
      if ((borrower != null || coBorrower != null) && loanData.Calculator != null)
        loanData.Calculator.UpdateAccountName("4000");
      if (borrower != null)
        this.setOpportunityFieldsInLoan(loanData, borrower.ContactID);
      if (borrower != null)
        this.setBorrowerCustomFieldsInLoan(loanData, borrower);
      return this.setLOLicense(loanData);
    }

    private bool setLOLicense(LoanData loanData)
    {
      if (loanData.GetField("14") == "" || loanData.GetField("LOID") != this.sessionObjects.UserID)
        return true;
      LOLicenseInfo loLicense = this.sessionObjects.OrganizationManager.GetLOLicense(this.sessionObjects.UserID, loanData.GetField("14"));
      if (loLicense != null && loLicense.Enabled)
      {
        DateTime dateTime = DateTime.Today;
        dateTime = dateTime.Date;
        if (dateTime.CompareTo(loLicense.ExpirationDate) <= 0)
        {
          if ((loLicense.License ?? "") != string.Empty)
            loanData.SetCurrentField("2306", loLicense.License);
          else
            loanData.SetCurrentField("2306", "");
          return true;
        }
      }
      int num = (int) Utils.Dialog((IWin32Window) null, "You have selected a state in which the loan officer is not licensed to originate loans or the license expiration is expired.  Contact your system administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void setBorrowerFieldsInLoan(LoanData loanData, BorrowerInfo info)
    {
      loanData.SetField("4000", info.FirstName);
      loanData.SetField("4001", info.MiddleName);
      loanData.SetField("4002", info.LastName);
      loanData.SetField("4003", info.SuffixName);
      loanData.SetField("FR0104", info.HomeAddress.Street1 + " " + info.HomeAddress.Street2);
      loanData.SetField("FR0106", info.HomeAddress.City);
      loanData.SetField("FR0107", info.HomeAddress.State);
      loanData.SetField("FR0108", info.HomeAddress.Zip);
      loanData.SetField("66", info.HomePhone);
      loanData.SetField("1178", info.BizEmail);
      loanData.SetField("1240", info.PersonalEmail);
      loanData.SetField("1188", info.FaxNumber);
      loanData.SetField("1822", info.Referral);
      loanData.SetField("65", info.SSN);
      loanData.SetField("FE0104", info.BizAddress.Street1 + " " + info.BizAddress.Street2);
      loanData.SetField("FE0105", info.BizAddress.City);
      loanData.SetField("FE0106", info.BizAddress.State);
      loanData.SetField("FE0107", info.BizAddress.Zip);
      loanData.SetField("FE0102", info.EmployerName);
      loanData.SetField("FE0110", info.JobTitle);
      loanData.SetField("FE0117", info.WorkPhone);
      loanData.SetField("1402", info.Birthdate == DateTime.MinValue ? string.Empty : info.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
      loanData.SetField("1490", info.MobilePhone);
      if (info.Married)
      {
        loanData.SetField("52", "Married");
        string[] firstLastName = ContactUtil.GetFirstLastName(info.SpouseName);
        loanData.SetField("4004", firstLastName[0]);
        loanData.SetField("4006", firstLastName[1]);
      }
      else
        loanData.SetField("52", "");
    }

    private void setCoborrowerFieldsInLoan(LoanData loanData, BorrowerInfo info)
    {
      if (info == null)
        return;
      loanData.SetField("4004", info.FirstName);
      loanData.SetField("4005", info.MiddleName);
      loanData.SetField("4006", info.LastName);
      loanData.SetField("4007", info.SuffixName);
      loanData.SetField("FR0204", info.HomeAddress.Street1 + " " + info.HomeAddress.Street2);
      loanData.SetField("FR0206", info.HomeAddress.City);
      loanData.SetField("FR0207", info.HomeAddress.State);
      loanData.SetField("FR0208", info.HomeAddress.Zip);
      loanData.SetField("98", info.HomePhone);
      loanData.SetField("1179", info.BizEmail);
      loanData.SetField("1268", info.PersonalEmail);
      loanData.SetField("1241", info.FaxNumber);
      loanData.SetField("97", info.SSN);
      loanData.SetField("FE0204", info.BizAddress.Street1 + " " + info.BizAddress.Street2);
      loanData.SetField("FE0205", info.BizAddress.City);
      loanData.SetField("FE0206", info.BizAddress.State);
      loanData.SetField("FE0207", info.BizAddress.Zip);
      loanData.SetField("FE0202", info.EmployerName);
      loanData.SetField("FE0210", info.JobTitle);
      loanData.SetField("FE0217", info.WorkPhone);
      loanData.SetField("1403", info.Birthdate == DateTime.MinValue ? string.Empty : info.Birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
      loanData.SetField("1480", info.MobilePhone);
      if (info.Married)
        loanData.SetField("84", "Married");
      else
        loanData.SetField("84", "");
    }

    private void setOpportunityFieldsInLoan(LoanData loanData, int contactId)
    {
      Opportunity opportunityByBorrowerId = this.sessionObjects.ContactManager.GetOpportunityByBorrowerId(contactId);
      if (opportunityByBorrowerId == null)
        return;
      if (opportunityByBorrowerId.Purpose == EllieMae.EMLite.Common.Contact.LoanPurpose.Other)
        loanData.SetCurrentField("9", opportunityByBorrowerId.PurposeOther);
      loanData.SetCurrentField("4", opportunityByBorrowerId.Term < 0 ? "" : opportunityByBorrowerId.Term.ToString(), false);
      loanData.SetCurrentField("608", AmortizationTypeEnumUtil.ValueToNameInLoan(opportunityByBorrowerId.Amortization), false);
      loanData.SetCurrentField("11", opportunityByBorrowerId.PropertyAddress.Street1, false);
      loanData.SetCurrentField("12", opportunityByBorrowerId.PropertyAddress.City, false);
      loanData.SetCurrentField("14", opportunityByBorrowerId.PropertyAddress.State, false);
      loanData.SetCurrentField("15", opportunityByBorrowerId.PropertyAddress.Zip, false);
      ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(opportunityByBorrowerId.PropertyAddress.Zip);
      if (zipInfoAt != null && zipInfoAt.County != null)
        loanData.SetCurrentField("13", zipInfoAt.County, false);
      loanData.SetCurrentField("1811", PropertyUseEnumUtil.ValueToNameInLoan(opportunityByBorrowerId.PropUse), false);
      loanData.SetCurrentField("1041", PropertyTypeEnumUtil.ValueToNameInLoan(opportunityByBorrowerId.PropType), false);
      loanData.SetCurrentField("26", opportunityByBorrowerId.MortgageBalance == 0M ? "" : opportunityByBorrowerId.MortgageBalance.ToString("N0"));
      loanData.SetCurrentField("265", opportunityByBorrowerId.IsBankruptcy ? "Y" : "N");
      loanData.SetCurrentField("24", opportunityByBorrowerId.PurchaseDate == DateTime.MinValue ? string.Empty : opportunityByBorrowerId.PurchaseDate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo));
      loanData.SetCurrentField("19", LoanPurposeEnumUtil.ValueToNameInLoan(opportunityByBorrowerId.Purpose), false);
      loanData.SetCurrentField("1172", LoanTypeEnumUtil.ValueToNameInLoan(opportunityByBorrowerId.LoanType), false);
      if (opportunityByBorrowerId.LoanType == LoanTypeEnum.Other)
        loanData.SetCurrentField("1063", opportunityByBorrowerId.TypeOther, false);
      if (Utils.IsInt((object) opportunityByBorrowerId.CreditRating))
        loanData.SetCurrentField("VASUMM.X23", string.Concat((object) Utils.ParseInt((object) opportunityByBorrowerId.CreditRating)));
      loanData.SetField("1821", opportunityByBorrowerId.PropertyValue == 0M ? "" : opportunityByBorrowerId.PropertyValue.ToString("N0"));
      loanData.SetField("1335", opportunityByBorrowerId.DownPayment == 0M ? "" : opportunityByBorrowerId.DownPayment.ToString("N0"));
      loanData.SetField("1109", opportunityByBorrowerId.LoanAmount == 0M ? "" : opportunityByBorrowerId.LoanAmount.ToString("N0"));
      if (opportunityByBorrowerId.Employment == EmploymentStatus.SelfEmployed)
        loanData.SetCurrentField("FE0115", "Y");
      loanData.SetCurrentField("PREQUAL.X250", opportunityByBorrowerId.CashOut == 0M ? "" : opportunityByBorrowerId.CashOut.ToString("N0"));
    }

    private void setBorrowerCustomFieldsInLoan(LoanData loanData, BorrowerInfo borrowerInfo)
    {
      CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(this.sessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower, false));
      if (mappingCollection.Count == 0)
        return;
      ContactCustomField[] fieldsForContact = this.sessionObjects.ContactManager.GetCustomFieldsForContact(borrowerInfo.ContactID, ContactType.Borrower);
      if (fieldsForContact.Length == 0)
        return;
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      foreach (ContactCustomField contactCustomField in fieldsForContact)
        dictionary.Add(contactCustomField.FieldID, contactCustomField.FieldValue);
      foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
      {
        if (dictionary.ContainsKey(customFieldMapping.FieldNumber))
        {
          string val = dictionary[customFieldMapping.FieldNumber];
          try
          {
            loanData.SetField(customFieldMapping.LoanFieldId, val);
          }
          catch (Exception ex)
          {
            Tracing.Log(ContactUtil.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Business Contact 'Custom Field {0}', Value '{1}' to Loan Field ID '{2}' failed.", (object) customFieldMapping.FieldNumber.ToString(), (object) val, (object) customFieldMapping.LoanFieldId));
          }
        }
      }
    }

    public static CRMRoleType GetCRMRoleType(string loanFieldID)
    {
      CRMRoleType crmRoleType = CRMRoleType.NotFound;
      switch (loanFieldID.ToLower())
      {
        case "1011":
        case "1175":
        case "610":
        case "611":
        case "612":
        case "613":
        case "614":
        case "615":
        case "87":
        case "vend.x396":
        case "vend.x397":
          crmRoleType = CRMRoleType.EscrowCompany;
          break;
        case "1174":
        case "1243":
        case "411":
        case "412":
        case "413":
        case "414":
        case "416":
        case "417":
        case "88":
        case "vend.x398":
        case "vend.x399":
          crmRoleType = CRMRoleType.TitleInsuranceCompany;
          break;
        case "1244":
        case "1246":
        case "617":
        case "618":
        case "619":
        case "620":
        case "621":
        case "622":
        case "89":
        case "974":
          crmRoleType = CRMRoleType.Appraiser;
          break;
        case "1245":
        case "1247":
        case "624":
        case "625":
        case "626":
        case "627":
        case "628":
        case "629":
        case "90":
          crmRoleType = CRMRoleType.CreditCompany;
          break;
        case "1249":
        case "1251":
        case "638":
        case "701":
        case "702":
        case "703":
        case "704":
        case "92":
        case "vend.x220":
          crmRoleType = CRMRoleType.Seller1;
          break;
        case "1252":
        case "1254":
        case "707":
        case "708":
        case "709":
        case "710":
        case "711":
        case "93":
        case "l248":
          crmRoleType = CRMRoleType.MortgageInsurance;
          break;
        case "1253":
        case "1255":
        case "713":
        case "714":
        case "715":
        case "716":
        case "717":
        case "718":
        case "94":
          crmRoleType = CRMRoleType.Builder;
          break;
        case "1256":
        case "1257":
        case "1258":
        case "1259":
        case "1260":
        case "1262":
        case "1263":
        case "1264":
        case "95":
          crmRoleType = CRMRoleType.Lender;
          break;
        case "1410":
        case "1411":
        case "984":
        case "regzgfe.x8":
        case "vend.x171":
        case "vend.x172":
        case "vend.x173":
        case "vend.x174":
        case "vend.x176":
          crmRoleType = CRMRoleType.Underwriter;
          break;
        case "1500":
        case "vend.x13":
        case "vend.x14":
        case "vend.x15":
        case "vend.x16":
        case "vend.x17":
        case "vend.x19":
        case "vend.x20":
        case "vend.x21":
          crmRoleType = CRMRoleType.FloodInsurance;
          break;
        case "1822":
          crmRoleType = CRMRoleType.Referral;
          break;
        case "395":
        case "vend.x190":
        case "vend.x191":
        case "vend.x192":
        case "vend.x193":
        case "vend.x195":
        case "vend.x196":
        case "vend.x197":
        case "vend.x198":
          crmRoleType = CRMRoleType.DocSigning;
          break;
        case "56":
        case "vend.x112":
        case "vend.x113":
        case "vend.x114":
        case "vend.x115":
        case "vend.x117":
        case "vend.x118":
        case "vend.x119":
        case "vend.x120":
        case "vend.x400":
        case "vend.x401":
          crmRoleType = CRMRoleType.BuyerAttorney;
          break;
        case "l252":
        case "vend.x157":
        case "vend.x158":
        case "vend.x159":
        case "vend.x160":
        case "vend.x162":
        case "vend.x163":
        case "vend.x164":
        case "vend.x165":
          crmRoleType = CRMRoleType.HazardInsurance;
          break;
        case "vend.x1":
        case "vend.x10":
        case "vend.x11":
        case "vend.x2":
        case "vend.x3":
        case "vend.x4":
        case "vend.x410":
        case "vend.x411":
        case "vend.x5":
        case "vend.x6":
        case "vend.x8":
        case "vend.x9":
          crmRoleType = CRMRoleType.CustomCategory4;
          break;
        case "vend.x122":
        case "vend.x123":
        case "vend.x124":
        case "vend.x125":
        case "vend.x126":
        case "vend.x128":
        case "vend.x129":
        case "vend.x130":
        case "vend.x131":
        case "vend.x402":
        case "vend.x403":
          crmRoleType = CRMRoleType.SellerAttorney;
          break;
        case "vend.x133":
        case "vend.x134":
        case "vend.x135":
        case "vend.x136":
        case "vend.x137":
        case "vend.x139":
        case "vend.x140":
        case "vend.x141":
        case "vend.x142":
          crmRoleType = CRMRoleType.BuyerAgent;
          break;
        case "vend.x144":
        case "vend.x145":
        case "vend.x146":
        case "vend.x147":
        case "vend.x148":
        case "vend.x150":
        case "vend.x151":
        case "vend.x152":
        case "vend.x153":
          crmRoleType = CRMRoleType.SellerAgent;
          break;
        case "vend.x178":
        case "vend.x179":
        case "vend.x180":
        case "vend.x181":
        case "vend.x182":
        case "vend.x184":
        case "vend.x185":
        case "vend.x186":
        case "vend.x187":
          crmRoleType = CRMRoleType.Servicing;
          break;
        case "vend.x200":
        case "vend.x201":
        case "vend.x202":
        case "vend.x203":
        case "vend.x204":
        case "vend.x206":
        case "vend.x207":
        case "vend.x208":
        case "vend.x209":
          crmRoleType = CRMRoleType.Warehouse;
          break;
        case "vend.x263":
        case "vend.x264":
        case "vend.x265":
        case "vend.x266":
        case "vend.x267":
        case "vend.x271":
        case "vend.x272":
        case "vend.x273":
        case "vend.x274":
          crmRoleType = CRMRoleType.Investor;
          break;
        case "vend.x278":
        case "vend.x279":
        case "vend.x280":
        case "vend.x281":
        case "vend.x282":
        case "vend.x286":
        case "vend.x287":
        case "vend.x288":
        case "vend.x289":
          crmRoleType = CRMRoleType.AssignTo;
          break;
        case "vend.x293":
        case "vend.x294":
        case "vend.x295":
        case "vend.x296":
        case "vend.x297":
        case "vend.x300":
        case "vend.x302":
        case "vend.x303":
        case "vend.x304":
        case "vend.x305":
        case "vend.x306":
          crmRoleType = CRMRoleType.Broker;
          break;
        case "vend.x310":
        case "vend.x311":
        case "vend.x312":
        case "vend.x313":
        case "vend.x314":
        case "vend.x317":
        case "vend.x318":
        case "vend.x319":
        case "vend.x320":
          crmRoleType = CRMRoleType.DocsPreparedBy;
          break;
        case "vend.x34":
        case "vend.x35":
        case "vend.x36":
        case "vend.x37":
        case "vend.x38":
        case "vend.x39":
        case "vend.x41":
        case "vend.x42":
        case "vend.x43":
          crmRoleType = CRMRoleType.Surveyor;
          break;
        case "vend.x404":
        case "vend.x405":
        case "vend.x54":
        case "vend.x55":
        case "vend.x56":
        case "vend.x57":
        case "vend.x58":
        case "vend.x59":
        case "vend.x61":
        case "vend.x62":
        case "vend.x63":
        case "vend.x84":
          crmRoleType = CRMRoleType.CustomCategory1;
          break;
        case "vend.x406":
        case "vend.x407":
        case "vend.x64":
        case "vend.x65":
        case "vend.x66":
        case "vend.x67":
        case "vend.x68":
        case "vend.x69":
        case "vend.x71":
        case "vend.x72":
        case "vend.x73":
        case "vend.x85":
          crmRoleType = CRMRoleType.CustomCategory2;
          break;
        case "vend.x408":
        case "vend.x409":
        case "vend.x74":
        case "vend.x75":
        case "vend.x76":
        case "vend.x77":
        case "vend.x78":
        case "vend.x79":
        case "vend.x81":
        case "vend.x82":
        case "vend.x83":
        case "vend.x86":
          crmRoleType = CRMRoleType.CustomCategory3;
          break;
        case "vend.x412":
        case "vend.x413":
        case "vend.x414":
        case "vend.x415":
        case "vend.x416":
        case "vend.x417":
        case "vend.x418":
        case "vend.x419":
        case "vend.x421":
          crmRoleType = CRMRoleType.Seller2;
          break;
        case "vend.x424":
        case "vend.x425":
        case "vend.x426":
        case "vend.x427":
        case "vend.x428":
        case "vend.x429":
        case "vend.x430":
        case "vend.x431":
        case "vend.x432":
          crmRoleType = CRMRoleType.Notary;
          break;
        case "vend.x44":
        case "vend.x45":
        case "vend.x46":
        case "vend.x47":
        case "vend.x48":
        case "vend.x49":
        case "vend.x51":
        case "vend.x52":
        case "vend.x53":
          crmRoleType = CRMRoleType.FinancialPlanner;
          break;
        case "vend.x655":
          crmRoleType = CRMRoleType.SettlementAgent;
          break;
        default:
          if (loanFieldID.ToLower().StartsWith("seller3."))
            return CRMRoleType.Seller3;
          if (loanFieldID.ToLower().StartsWith("seller4."))
            return CRMRoleType.Seller4;
          if (loanFieldID.ToLower().StartsWith("fe") && !loanFieldID.ToLower().StartsWith("fema") || loanFieldID.ToLower().StartsWith("be") || loanFieldID.ToLower().StartsWith("ce"))
          {
            crmRoleType = CRMRoleType.Employer;
            break;
          }
          if (loanFieldID.ToLower().StartsWith("dd"))
          {
            crmRoleType = CRMRoleType.Depository;
            break;
          }
          if (loanFieldID.ToLower().StartsWith("fl"))
          {
            crmRoleType = CRMRoleType.Liability;
            break;
          }
          if (loanFieldID.ToLower().StartsWith("fr") || loanFieldID.ToLower().StartsWith("br") || loanFieldID.ToLower().StartsWith("cr"))
          {
            crmRoleType = CRMRoleType.Residence;
            break;
          }
          break;
      }
      return crmRoleType;
    }

    public static string getCRMMappingID(CRMRoleType roleType, string loanFieldID)
    {
      string crmMappingId;
      switch (roleType)
      {
        case CRMRoleType.Lender:
        case CRMRoleType.Appraiser:
        case CRMRoleType.EscrowCompany:
        case CRMRoleType.TitleInsuranceCompany:
        case CRMRoleType.BuyerAttorney:
        case CRMRoleType.SellerAttorney:
        case CRMRoleType.BuyerAgent:
        case CRMRoleType.SellerAgent:
        case CRMRoleType.Seller1:
        case CRMRoleType.Seller2:
        case CRMRoleType.Notary:
        case CRMRoleType.Builder:
        case CRMRoleType.HazardInsurance:
        case CRMRoleType.MortgageInsurance:
        case CRMRoleType.Surveyor:
        case CRMRoleType.FloodInsurance:
        case CRMRoleType.CreditCompany:
        case CRMRoleType.Underwriter:
        case CRMRoleType.Servicing:
        case CRMRoleType.DocSigning:
        case CRMRoleType.Warehouse:
        case CRMRoleType.FinancialPlanner:
        case CRMRoleType.Investor:
        case CRMRoleType.AssignTo:
        case CRMRoleType.Broker:
        case CRMRoleType.DocsPreparedBy:
        case CRMRoleType.CustomCategory1:
        case CRMRoleType.CustomCategory2:
        case CRMRoleType.CustomCategory3:
        case CRMRoleType.CustomCategory4:
        case CRMRoleType.Referral:
        case CRMRoleType.Seller3:
        case CRMRoleType.Seller4:
        case CRMRoleType.SellerCorporationOfficer:
          crmMappingId = string.Concat((object) (int) roleType);
          break;
        case CRMRoleType.Employer:
          crmMappingId = !loanFieldID.StartsWith("FE01") ? (!loanFieldID.StartsWith("FE02") ? loanFieldID.Substring(0, 4) : "CE01") : "BE01";
          break;
        case CRMRoleType.Depository:
          crmMappingId = loanFieldID.Substring(0, 4);
          break;
        case CRMRoleType.Liability:
          crmMappingId = loanFieldID.Substring(0, 4);
          break;
        case CRMRoleType.Residence:
          crmMappingId = !loanFieldID.StartsWith("FR01") ? (!loanFieldID.StartsWith("FR02") ? loanFieldID.Substring(0, 4) : "CR01") : "BR01";
          break;
        default:
          crmMappingId = "";
          break;
      }
      return crmMappingId;
    }
  }
}
