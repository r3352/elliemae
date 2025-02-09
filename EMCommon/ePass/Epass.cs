// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Epass
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class Epass
  {
    public static Epass.EpassDoc Credit = new Epass.EpassDoc("Credit Report", "Credit Report", "_EPASS_SIGNATURE;EPASSAI;2;Credit+Report");
    public static Epass.EpassDoc Lender = new Epass.EpassDoc("Lender Submission", nameof (Lender), "_EPASS_SIGNATURE;EPASSAI;2;Lenders");
    public static Epass.EpassDoc PP = new Epass.EpassDoc("Product and Pricing", "Product and Pricing", "_EPASS_SIGNATURE;EPASSAI;2;Product+and+Pricing");
    public static Epass.EpassDoc PPLockRequest = new Epass.EpassDoc("Price Table - Lock Request", "Product and Pricing", "_EPASS_SIGNATURE;EPASSAI;;GetPricing_LO;SOURCE_FORM=GETPRICING_LO_LOCK;Product+and+Pricing");
    public static Epass.EpassDoc PPBuySide = new Epass.EpassDoc("Price Table - Buy Side", "Product and Pricing", "_EPASS_SIGNATURE;EPASSAI;;GetPricing_SEC;SOURCE_FORM=GETBUYSIDEPRICING_SEC;Product+and+Pricing");
    public static Epass.EpassDoc PPSellSide = new Epass.EpassDoc("Price Table - Sell Side", "Product and Pricing", "_EPASS_SIGNATURE;EPASSAI;GetSellSidePricing_SEC;SOURCE_FORM=GetSellSidePricing_SEC;Product+and+Pricing");
    public static Epass.EpassDoc AU = new Epass.EpassDoc("Underwriting", nameof (AU), "_EPASS_SIGNATURE;EPASSAI;2;Underwriting");
    public static Epass.EpassDoc Appraisal = new Epass.EpassDoc(nameof (Appraisal), nameof (Appraisal), "_EPASS_SIGNATURE;EPASSAI;2;Appraisal");
    public static Epass.EpassDoc Flood = new Epass.EpassDoc("Flood Certificate", nameof (Flood), "_EPASS_SIGNATURE;EPASSAI;2;Flood+Certification");
    public static Epass.EpassDoc Title = new Epass.EpassDoc("Title Report", "Title Report", "_EPASS_SIGNATURE;EPASSAI;2;Title+%26+Closing");
    public static Epass.EpassDoc Closing = new Epass.EpassDoc("Escrow/Closing", "Escrow/Closing", "_EPASS_SIGNATURE;EPASSAI;2;Title+%26+Closing");
    public static Epass.EpassDoc DocPrep = new Epass.EpassDoc("Document Preparation", "Doc's", "_EPASS_SIGNATURE;EPASSAI;2;Doc+Preparation");
    public static Epass.EpassDoc MERS = new Epass.EpassDoc(nameof (MERS), nameof (MERS), "_EPASS_SIGNATURE;MERS;2");
    public static Epass.EpassDoc AVM = new Epass.EpassDoc(nameof (AVM), nameof (AVM), "_EPASS_SIGNATURE;EPASSAI;2;AVM");
    public static Epass.EpassDoc MI = new Epass.EpassDoc("Mortgage Insurance", nameof (MI), "_EPASS_SIGNATURE;EPASSAI;2;Mortgage+Insurance");
    public static Epass.EpassDoc Fraud = new Epass.EpassDoc("Fraud/Audit Services", nameof (Fraud), "_EPASS_SIGNATURE;EPASSAI;2;Fraud%2FAudit+Services");
    public static Epass.EpassDoc TaxReturnService = new Epass.EpassDoc("4506T Settlement Service", "4506T", "");
    public static Epass.EpassDoc ComplianceReport = new Epass.EpassDoc("Compliance Report", "Compliance Report", "");
    public static Epass.EpassDoc FraudLoanService = new Epass.EpassDoc(nameof (Fraud), nameof (Fraud), "");
    public static Epass.EpassDoc[] AllDocs = new Epass.EpassDoc[19]
    {
      Epass.Credit,
      Epass.Lender,
      Epass.PP,
      Epass.AU,
      Epass.Appraisal,
      Epass.Flood,
      Epass.Title,
      Epass.Closing,
      Epass.DocPrep,
      Epass.MERS,
      Epass.AVM,
      Epass.MI,
      Epass.Fraud,
      Epass.TaxReturnService,
      Epass.ComplianceReport,
      Epass.FraudLoanService,
      Epass.PPLockRequest,
      Epass.PPBuySide,
      Epass.PPSellSide
    };

    public static Epass.EpassDoc GetEpassDoc(string title)
    {
      foreach (Epass.EpassDoc allDoc in Epass.AllDocs)
      {
        if (allDoc.FullName == title)
          return allDoc;
      }
      return (Epass.EpassDoc) null;
    }

    public static bool IsEpassDoc(string title)
    {
      foreach (Epass.EpassDoc allDoc in Epass.AllDocs)
      {
        if (allDoc.FullName == title)
          return true;
      }
      return false;
    }

    [ComVisible(false)]
    public class EpassDoc
    {
      private string fullname;
      private string shortname;
      private string url;

      public string FullName => this.fullname;

      public string ShortName => this.shortname;

      public string Url => this.url;

      internal EpassDoc(string fullN, string shortN, string url)
      {
        this.fullname = fullN;
        this.shortname = shortN;
        this.url = url;
      }
    }
  }
}
