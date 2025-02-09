// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ContactGroup
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public static class ContactGroup
  {
    private static Hashtable contactTable = CollectionsUtil.CreateCaseInsensitiveHashtable();

    static ContactGroup()
    {
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Appraiser, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Appraiser, "Appraiser", "617", "619", "620", "1244", "621", "VEND.X212", "622", "VEND.X214", "618", "89", "1246"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.AssignTo, (object) new ContactFieldRef(FileContactRecord.ContactTypes.AssignTo, "AssignTo", "VEND.X278", "VEND.X279", "VEND.X280", "VEND.X281", "VEND.X282", "VEND.X284", "VEND.X287", "VEND.X285", "VEND.X286", "VEND.X288", "VEND.X289"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Broker, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Broker, "Broker", "VEND.X293", "VEND.X294", "VEND.X295", "VEND.X296", "VEND.X297", "VEND.X298", "VEND.X303", "VEND.X301", "VEND.X302", "VEND.X305", "VEND.X306"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Builder, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Builder, "Builder", "713", "715", "716", "1253", "717", "VEND.X243", "718", "VEND.X244", "714", "94", "1255"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.BuyerAgent, (object) new ContactFieldRef(FileContactRecord.ContactTypes.BuyerAgent, "Buyer's Agent", "VEND.X133", "VEND.X134", "VEND.X135", "VEND.X136", "VEND.X137", "VEND.X237", "VEND.X140", "VEND.X238", "VEND.X139", "VEND.X141", "VEND.X142"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.BuyerAttorney, (object) new ContactFieldRef(FileContactRecord.ContactTypes.BuyerAttorney, "Buyer's Attorney", "56", "VEND.X112", "VEND.X113", "VEND.X114", "VEND.X115", "VEND.X233", "VEND.X118", "VEND.X234", "VEND.X117", "VEND.X119", "VEND.X120"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.CreditCompany, (object) new ContactFieldRef(FileContactRecord.ContactTypes.CreditCompany, "Credit Company", "624", "626", "627", "1245", "628", "VEND.X227", "629", "VEND.X228", "625", "90", "1247"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.DocSigning, (object) new ContactFieldRef(FileContactRecord.ContactTypes.DocSigning, "Doc Signing", "395", "VEND.X190", "VEND.X191", "VEND.X192", "VEND.X193", "VEND.X249", "VEND.X196", "VEND.X250", "VEND.X195", "VEND.X197", "VEND.X198"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.DocsPrepared, (object) new ContactFieldRef(FileContactRecord.ContactTypes.DocsPrepared, "Docs Prepared", "VEND.X310", "VEND.X311", "VEND.X312", "VEND.X313", "VEND.X314", "VEND.X315", "VEND.X318", "VEND.X316", "VEND.X317", "VEND.X319", "VEND.X320"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Escrow, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Escrow, "Escrow Company", "610", "612", "613", "1175", "614", "VEND.X216", "615", "VEND.X218", "611", "87", "1011"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.FinancialPlanner, (object) new ContactFieldRef(FileContactRecord.ContactTypes.FinancialPlanner, "Financial Planner", "VEND.X44", "VEND.X46", "VEND.X47", "VEND.X48", "VEND.X49", "VEND.X253", "VEND.X51", "VEND.X254", "VEND.X45", "VEND.X53", "VEND.X52"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.FloodInsurance, (object) new ContactFieldRef(FileContactRecord.ContactTypes.FloodInsurance, "Flood Insurance", "1500", "VEND.X14", "VEND.X15", "VEND.X16", "VEND.X17", "VEND.X225", "VEND.X19", "VEND.X226", "VEND.X13", "VEND.X21", "VEND.X20"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.HazardInsurance, (object) new ContactFieldRef(FileContactRecord.ContactTypes.HazardInsurance, "Hazard Insurance", "L252", "VEND.X157", "VEND.X158", "VEND.X159", "VEND.X160", "VEND.X221", "VEND.X163", "VEND.X222", "VEND.X162", "VEND.X164", "VEND.X165"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Investor, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Investor, "Investor", "VEND.X263", "VEND.X264", "VEND.X265", "VEND.X266", "VEND.X267", "VEND.X269", "VEND.X272", "VEND.X270", "VEND.X271", "VEND.X273", "VEND.X274"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Lender, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Lender, "Lender", "1264", "1257", "1258", "1259", "1260", "VEND.X231", "1262", "VEND.X232", "1256", "95", "1263"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.MortgageInsurance, (object) new ContactFieldRef(FileContactRecord.ContactTypes.MortgageInsurance, "Mortgage Insurance", "L248", "708", "709", "1252", "710", "VEND.X223", "711", "VEND.X224", "707", "93", "1254"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Notary, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Notary, "Notary", "VEND.X424", "VEND.X425", "VEND.X426", "VEND.X427", "VEND.X428", "VEND.X435", "VEND.X430", "VEND.X436", "VEND.X429", "VEND.X432", "VEND.X431"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Seller, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Seller, "Seller", "638", "701", "702", "1249", "703", "VEND.X241", "704", "VEND.X242", "638", "92", "1251"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Seller2, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Seller2, "Seller", "VEND.X412", "VEND.X413", "VEND.X414", "VEND.X415", "VEND.X416", "VEND.X422", "VEND.X417", "VEND.X423", "VEND.X412", "VEND.X419", "VEND.X418"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Seller3, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Seller3, "Seller", "Seller3.Name", "Seller3.Addr", "Seller3.City", "Seller3.State", "Seller3.Zip", "Seller3.Rel", "Seller3.Phone", "Seller3.LineNum", "Seller3.Name", "Seller3.Email", "Seller3.Fax"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Seller4, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Seller4, "Seller", "Seller4.Name", "Seller4.Addr", "Seller4.City", "Seller4.State", "Seller4.Zip", "Seller4.Rel", "Seller4.Phone", "Seller4.LineNum", "Seller4.Name", "Seller4.Email", "Seller4.Fax"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.SellerAgent, (object) new ContactFieldRef(FileContactRecord.ContactTypes.SellerAgent, "Seller's Agent", "VEND.X144", "VEND.X145", "VEND.X146", "VEND.X147", "VEND.X148", "VEND.X239", "VEND.X151", "VEND.X240", "VEND.X150", "VEND.X152", "VEND.X153"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.SellerAttorney, (object) new ContactFieldRef(FileContactRecord.ContactTypes.SellerAttorney, "Seller's Attorney", "VEND.X122", "VEND.X123", "VEND.X124", "VEND.X125", "VEND.X126", "VEND.X235", "VEND.X129", "VEND.X236", "VEND.X128", "VEND.X130", "VEND.X131"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Servicing, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Servicing, "Servicing", "VEND.X178", "VEND.X179", "VEND.X180", "VEND.X181", "VEND.X182", "VEND.X247", "VEND.X185", "VEND.X248", "VEND.X184", "VEND.X186", "VEND.X187"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Surveyor, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Surveyor, "Surveyor", "VEND.X34", "VEND.X36", "VEND.X37", "VEND.X38", "VEND.X39", "VEND.X245", "VEND.X41", "VEND.X246", "VEND.X35", "VEND.X43", "VEND.X42"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Title, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Title, "Title Insurance", "411", "412", "413", "1174", "414", "VEND.X155", "417", "VEND.X156", "416", "88", "1243"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Underwriter, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Underwriter, "Underwriter", "REGZGFE.X8", "VEND.X171", "VEND.X172", "VEND.X173", "VEND.X174", "VEND.X229", "1410", "VEND.X230", "984", "1411", "VEND.X176"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Warehouse, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Warehouse, "Warehouse", "VEND.X200", "VEND.X201", "VEND.X202", "VEND.X203", "VEND.X204", "VEND.X251", "VEND.X207", "VEND.X252", "VEND.X206", "VEND.X208", "VEND.X209"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.SettlementAgent, (object) new ContactFieldRef(FileContactRecord.ContactTypes.SettlementAgent, "Settlement Agent", "VEND.X655", "VEND.X656", "VEND.X657", "VEND.X658", "VEND.X659", "VEND.X660", "VEND.X669", "VEND.X661", "VEND.X668", "VEND.X670", "VEND.X671"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.SellerCorporationOfficer, (object) new ContactFieldRef(FileContactRecord.ContactTypes.SellerCorporationOfficer, "Seller Corporation Officers", "1863", "4881", "4882", "4883", "4884", "VEND.X241", "4885", "VEND.X242", "Vesting.SelOfcr1Nm", "4886", "4887"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Custom1, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Custom1, "VEND.X84", "VEND.X54", "VEND.X56", "VEND.X57", "VEND.X58", "VEND.X59", "VEND.X255", "VEND.X61", "VEND.X256", "VEND.X55", "VEND.X63", "VEND.X62"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Custom2, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Custom2, "VEND.X85", "VEND.X64", "VEND.X66", "VEND.X67", "VEND.X68", "VEND.X69", "VEND.X257", "VEND.X71", "VEND.X258", "VEND.X65", "VEND.X73", "VEND.X72"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Custom3, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Custom3, "VEND.X86", "VEND.X74", "VEND.X76", "VEND.X77", "VEND.X78", "VEND.X79", "VEND.X259", "VEND.X81", "VEND.X260", "VEND.X75", "VEND.X83", "VEND.X82"));
      ContactGroup.contactTable.Add((object) FileContactRecord.ContactTypes.Custom4, (object) new ContactFieldRef(FileContactRecord.ContactTypes.Custom4, "VEND.X11", "VEND.X1", "VEND.X3", "VEND.X4", "VEND.X5", "VEND.X6", "VEND.X261", "VEND.X8", "VEND.X262", "VEND.X2", "VEND.X10", "VEND.X9"));
    }

    public static ContactFieldRef GetContactFields(FileContactRecord.ContactTypes contactType)
    {
      return !ContactGroup.contactTable.ContainsKey((object) contactType) ? (ContactFieldRef) null : (ContactFieldRef) ContactGroup.contactTable[(object) contactType];
    }
  }
}
