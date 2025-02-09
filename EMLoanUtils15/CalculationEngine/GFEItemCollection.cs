// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.GFEItemCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class GFEItemCollection
  {
    public static List<GFEItem> GFEItems = new List<GFEItem>();
    public static List<GFEItem> GFEItems2010 = new List<GFEItem>();
    private static Dictionary<int, GFEItem> GFEItems2010ByLnNum = new Dictionary<int, GFEItem>();
    private static Dictionary<string, GFEItem> GFEItems2010ByLnNumCpntId = new Dictionary<string, GFEItem>();
    public static HashSet<int> Excluded2015GFEItem = new HashSet<int>()
    {
      205,
      206,
      207,
      208,
      209,
      216,
      217,
      218,
      219,
      502,
      516,
      517,
      518,
      519
    };

    static GFEItemCollection()
    {
      GFEItemCollection.GFEItems.Add(new GFEItem(101, "", "136", "", "HUD1P1.X1", "", "", "SYS.X546", "", "Contract Sales Price", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(102, "", "L79", "", "HUD1P1.X2", "", "", "SYS.X547", "", "Personal Property", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1, "", "CD3.X1", "", "HUD1P1.X69", "", "", "NEWHUD2.X165", "", "Closing Costs Paid at Closing (J)", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(103, "", "L351", "", "HUD1P1.X3", "", "", "SYS.X548", "", "Settlement Charges to Borrower(from line 1400)", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(104, "", "L85", "", "HUD1P1.X4", "", "", "SYS.X549", "", "L84", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(105, "", "L89", "", "HUD1P1.X5", "", "", "SYS.X550", "", "L88", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(2, "", "CD3.X3", "", "HUD1P1.X70", "", "", "NEWHUD2.X166", "", "CD3.X2", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(3, "", "CD3.X5", "", "HUD1P1.X71", "", "", "NEWHUD2.X167", "", "CD3.X4", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(106, "", "L94", "", "HUD1P1.X6", "", "", "SYS.X551", "", "City Tax - Due From Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(107, "", "L100", "", "HUD1P1.X7", "", "", "SYS.X552", "", "County Tax - Due From Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(108, "", "L106", "", "HUD1P1.X8", "", "", "SYS.X553", "", "Assmt - Due From Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(109, "", "L111", "", "HUD1P1.X9", "", "", "SYS.X554", "", "L110", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(110, "", "L115", "", "HUD1P1.X10", "", "", "SYS.X555", "", "L114", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(111, "", "L119", "", "HUD1P1.X11", "", "", "SYS.X556", "", "L118", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(112, "", "L123", "", "HUD1P1.X12", "", "", "SYS.X557", "", "L122", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(4, "", "CD3.X7", "", "HUD1P1.X72", "", "", "NEWHUD2.X168", "", "CD3.X6", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(201, "", "L128", "", "HUD1P1.X14", "", "", "SYS.X558", "", "Deposit or Earnest", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(202, "", "2", "", "HUD1P1.X15", "", "", "SYS.X559", "", "Principal Amount", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(203, "", "L132", "", "HUD1P1.X16", "", "", "SYS.X560", "", "Existing Loans Taken", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(204, "", "L135", "", "HUD1P1.X17", "", "", "SYS.X561", "", "L134", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(5, "", "CD3.X108", "", "HUD1P1.X73", "", "", "NEWHUD2.X169", "", "Seller Credit", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(6, "", "CD3.X10", "", "HUD1P1.X74", "", "", "NEWHUD2.X170", "", "CD3.X9", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(7, "", "CD3.X12", "", "HUD1P1.X75", "", "", "NEWHUD2.X171", "", "CD3.X11", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(8, "", "CD3.X14", "", "HUD1P1.X76", "", "", "NEWHUD2.X172", "", "CD3.X13", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(9, "", "CD3.X16", "", "HUD1P1.X77", "", "", "NEWHUD2.X173", "", "CD3.X15", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(10, "", "CD3.X18", "", "HUD1P1.X78", "", "", "NEWHUD2.X174", "", "CD3.X17", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(11, "", "CD3.X20", "", "HUD1P1.X79", "", "", "NEWHUD2.X175", "", "CD3.X19", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(205, "", "L138", "", "HUD1P1.X18", "", "", "SYS.X562", "", "L137", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(206, "", "L141", "", "HUD1P1.X19", "", "", "SYS.X563", "", "L140", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(207, "", "L145", "", "HUD1P1.X20", "", "", "SYS.X564", "", "L144", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(208, "", "L149", "", "HUD1P1.X21", "", "", "SYS.X565", "", "L148", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(209, "", "L153", "", "HUD1P1.X22", "", "", "SYS.X566", "", "L152", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(210, "", "L158", "", "HUD1P1.X23", "", "", "SYS.X567", "", "City Tax - Paid by Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(211, "", "L164", "", "HUD1P1.X24", "", "", "SYS.X568", "", "County Tax - Paid by Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(212, "", "L170", "", "HUD1P1.X25", "", "", "SYS.X569", "", "Assmt - Paid by Borrower", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(213, "", "L175", "", "HUD1P1.X26", "", "", "SYS.X570", "", "L174", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(214, "", "L179", "", "HUD1P1.X27", "", "", "SYS.X571", "", "L178", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(215, "", "L183", "", "HUD1P1.X28", "", "", "SYS.X572", "", "L182", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(216, "", "L187", "", "HUD1P1.X29", "", "", "SYS.X573", "", "L186", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(217, "", "L191", "", "HUD1P1.X30", "", "", "SYS.X574", "", "L190", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(218, "", "L195", "", "HUD1P1.X31", "", "", "SYS.X575", "", "L194", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(219, "", "L199", "", "HUD1P1.X32", "", "", "SYS.X576", "", "L198", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(401, "", "136", "", "HUD1P1.X37", "", "", "SYS.X577", "", "Contract Sales Price", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(402, "", "L80", "", "HUD1P1.X38", "", "", "SYS.X578", "", "Personal Property", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(403, "", "L82", "", "HUD1P1.X39", "", "", "SYS.X579", "", "L81", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(404, "", "L87", "", "HUD1P1.X40", "", "", "SYS.X580", "", "L86", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(405, "", "L91", "", "HUD1P1.X41", "", "", "SYS.X581", "", "L90", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(12, "", "CD3.X25", "", "HUD1P1.X80", "", "", "NEWHUD2.X176", "", "CD3.X24", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(13, "", "CD3.X27", "", "HUD1P1.X81", "", "", "NEWHUD2.X177", "", "CD3.X26", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(14, "", "CD3.X29", "", "HUD1P1.X82", "", "", "NEWHUD2.X178", "", "CD3.X28", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(406, "", "L97", "", "HUD1P1.X42", "", "", "SYS.X582", "", "City Tax - Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(407, "", "L103", "", "HUD1P1.X43", "", "", "SYS.X583", "", "County Tax - Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(408, "", "L109", "", "HUD1P1.X44", "", "", "SYS.X584", "", "Assmt - Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(409, "", "L113", "", "HUD1P1.X45", "", "", "SYS.X585", "", "L112", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(410, "", "L117", "", "HUD1P1.X46", "", "", "SYS.X586", "", "L116", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(411, "", "L121", "", "HUD1P1.X47", "", "", "SYS.X587", "", "L120", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(412, "", "L125", "", "HUD1P1.X48", "", "", "SYS.X588", "", "L124", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(15, "", "CD3.X31", "", "HUD1P1.X83", "", "", "NEWHUD2.X179", "", "CD3.X30", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(501, "", "L129", "", "HUD1P1.X50", "", "", "SYS.X589", "", "Excess Deposit", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(16, "", "CD3.X46", "", "HUD1P1.X84", "", "", "NEWHUD2.X180", "", "Closing Costs Paid at Closing (J)", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(502, "", "L352", "", "HUD1P1.X51", "", "", "SYS.X590", "", "Settlement Charges (line 1400)", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(503, "", "L133", "", "HUD1P1.X52", "", "", "SYS.X591", "", "Existing Loans Taken", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(504, "", "L136", "", "HUD1P1.X53", "", "", "SYS.X592", "", "Payoff of 1st Mortgage", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(505, "", "L139", "", "HUD1P1.X54", "", "", "SYS.X593", "", "Payoff of 2nd Mortgage", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(506, "", "L143", "", "HUD1P1.X55", "", "", "SYS.X594", "", "L142", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(507, "", "L147", "", "HUD1P1.X56", "", "", "SYS.X595", "", "L146", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(17, "", "CD3.X108", "", "HUD1P1.X85", "", "", "NEWHUD2.X181", "", "Seller Credit", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(508, "", "L151", "", "HUD1P1.X57", "", "", "SYS.X596", "", "L150", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(509, "", "L155", "", "HUD1P1.X58", "", "", "SYS.X597", "", "L154", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(18, "", "CD3.X33", "", "HUD1P1.X86", "", "", "NEWHUD2.X182", "", "CD3.X32", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(19, "", "CD3.X35", "", "HUD1P1.X87", "", "", "NEWHUD2.X183", "", "CD3.X34", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(20, "", "CD3.X37", "", "HUD1P1.X88", "", "", "NEWHUD2.X184", "", "CD3.X36", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(510, "", "L161", "", "HUD1P1.X59", "", "", "SYS.X598", "", "City Tax - Reductions in Amount Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(511, "", "L167", "", "HUD1P1.X60", "", "", "SYS.X599", "", "County Tax - Reductions in Amount Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(512, "", "L173", "", "HUD1P1.X61", "", "", "SYS.X600", "", "Assmt - Reductions in Amount Due to Seller", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(513, "", "L177", "", "HUD1P1.X62", "", "", "SYS.X601", "", "L176", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(514, "", "L181", "", "HUD1P1.X63", "", "", "SYS.X602", "", "L180", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(515, "", "L185", "", "HUD1P1.X64", "", "", "SYS.X603", "", "L184", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(516, "", "L189", "", "HUD1P1.X65", "", "", "SYS.X604", "", "L188", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(517, "", "L193", "", "HUD1P1.X66", "", "", "SYS.X605", "", "L192", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(518, "", "L197", "", "HUD1P1.X67", "", "", "SYS.X606", "", "L196", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(519, "", "L201", "", "HUD1P1.X68", "", "", "SYS.X607", "", "L200", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(21, "", "CD3.X65", "", "HUD1P1.X89", "", "", "NEWHUD2.X185", "", "CD3.X50", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(22, "", "CD3.X66", "", "HUD1P1.X90", "", "", "NEWHUD2.X186", "", "CD3.X51", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(23, "", "CD3.X67", "", "HUD1P1.X91", "", "", "NEWHUD2.X187", "", "CD3.X52", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(24, "", "CD3.X68", "", "HUD1P1.X92", "", "", "NEWHUD2.X188", "", "CD3.X53", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(25, "", "CD3.X69", "", "HUD1P1.X93", "", "", "NEWHUD2.X189", "", "CD3.X54", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(26, "", "CD3.X70", "", "HUD1P1.X94", "", "", "NEWHUD2.X190", "", "CD3.X55", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(27, "", "CD3.X71", "", "HUD1P1.X95", "", "", "NEWHUD2.X191", "", "CD3.X56", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(28, "", "CD3.X72", "", "HUD1P1.X96", "", "", "NEWHUD2.X192", "", "CD3.X57", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(29, "", "CD3.X73", "", "HUD1P1.X97", "", "", "NEWHUD2.X193", "", "CD3.X58", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(30, "", "CD3.X74", "", "HUD1P1.X98", "", "", "NEWHUD2.X194", "", "CD3.X59", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(31, "", "CD3.X75", "", "HUD1P1.X99", "", "", "NEWHUD2.X195", "", "CD3.X60", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(32, "", "CD3.X76", "", "HUD1P1.X100", "", "", "NEWHUD2.X196", "", "CD3.X61", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(33, "", "CD3.X77", "", "HUD1P1.X101", "", "", "NEWHUD2.X197", "", "CD3.X62", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(34, "", "CD3.X78", "", "HUD1P1.X102", "", "", "NEWHUD2.X198", "", "CD3.X63", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(35, "", "CD3.X79", "", "HUD1P1.X103", "", "", "NEWHUD2.X199", "", "CD3.X64", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(36, "", "CD3.X1554", "", "HUD1P1.X104", "", "", "NEWHUD2.X4770", "", "CD3.X1544", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(37, "", "CD3.X1555", "", "HUD1P1.X105", "", "", "NEWHUD2.X4771", "", "CD3.X1545", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(38, "", "CD3.X1556", "", "HUD1P1.X106", "", "", "NEWHUD2.X4772", "", "CD3.X1546", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(39, "", "CD3.X1557", "", "HUD1P1.X107", "", "", "NEWHUD2.X4773", "", "CD3.X1547", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(40, "", "CD3.X1558", "", "HUD1P1.X108", "", "", "NEWHUD2.X4774", "", "CD3.X1548", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(41, "", "CD3.X1559", "", "HUD1P1.X109", "", "", "NEWHUD2.X4775", "", "CD3.X1549", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(42, "", "CD3.X1560", "", "HUD1P1.X110", "", "", "NEWHUD2.X4776", "", "CD3.X1550", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(43, "", "CD3.X1561", "", "HUD1P1.X111", "", "", "NEWHUD2.X4777", "", "CD3.X1551", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(44, "", "CD3.X1562", "", "HUD1P1.X112", "", "", "NEWHUD2.X4778", "", "CD3.X1552", "Y"));
      GFEItemCollection.GFEItems.Add(new GFEItem(45, "", "CD3.X1563", "", "HUD1P1.X113", "", "", "NEWHUD2.X4779", "", "CD3.X1553", "Y"));
      GFEItemCollection.GFEItems2010.AddRange((IEnumerable<GFEItem>) GFEItemCollection.GFEItems);
      GFEItemCollection.GFEItems.Add(new GFEItem(801, "REGZGFE.X5", "454", "559", "SYS.X251", "SYS.X136", "SYS.X252", "SYS.X409", "SYS.X477", "Loan Origination Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(802, "REGZGFE.X6", "1093", "560", "SYS.X253", "SYS.X137", "SYS.X254", "SYS.X410", "SYS.X478", "Loan Discount Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(803, "617", "641", "581", "SYS.X255", "SYS.X231", "SYS.X256", "SYS.X411", "SYS.X479", "Appraisal Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(804, "624", "640", "580", "SYS.X257", "SYS.X232", "SYS.X258", "SYS.X412", "SYS.X480", "Credit Report", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(805, "L704", "329", "557", "SYS.X259", "SYS.X138", "SYS.X260", "SYS.X413", "SYS.X481", "Lender's Inspection Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(806, "L227", "L228", "L229", "SYS.X261", "SYS.X139", "SYS.X262", "SYS.X414", "SYS.X482", "MI Application Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(807, "REGZGFE.X7", "L230", "L231", "SYS.X263", "SYS.X140", "SYS.X264", "SYS.X415", "SYS.X483", "Assumption Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(808, "REGZGFE.X14", "439", "572", "SYS.X265", "SYS.X147", "SYS.X266", "SYS.X416", "SYS.X484", "Mtg Broker Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(809, "L224", "336", "565", "SYS.X267", "SYS.X141", "SYS.X268", "SYS.X417", "SYS.X485", "Tax Servicing Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(810, "1812", "1621", "1622", "SYS.X269", "SYS.X233", "SYS.X270", "SYS.X418", "SYS.X486", "Processing Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(811, "REGZGFE.X8", "367", "569", "SYS.X271", "SYS.X142", "SYS.X272", "SYS.X419", "SYS.X487", "Underwriting Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(812, "1813", "1623", "1624", "SYS.X273", "SYS.X234", "SYS.X274", "SYS.X420", "SYS.X488", "Wire Transfer Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(813, "", "370", "574", "SYS.X275", "SYS.X149", "SYS.X276", "SYS.X421", "SYS.X489", "369", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(814, "", "372", "575", "SYS.X277", "SYS.X150", "SYS.X278", "SYS.X422", "SYS.X490", "371", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(815, "", "349", "96", "SYS.X279", "SYS.X151", "SYS.X280", "SYS.X423", "SYS.X491", "348", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(816, "", "932", "1345", "SYS.X281", "SYS.X152", "SYS.X282", "SYS.X424", "SYS.X492", "931", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(817, "", "1009", "6", "SYS.X283", "SYS.X153", "SYS.X284", "SYS.X425", "SYS.X493", "1390", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(818, "", "554", "678", "SYS.X285", "SYS.X154", "SYS.X286", "SYS.X426", "SYS.X494", "410", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(819, "", "81", "82", "SYS.X287", "SYS.X155", "SYS.X288", "SYS.X427", "SYS.X495", "1391", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(820, "", "155", "200", "SYS.X287", "SYS.X155", "SYS.X288", "SYS.X428", "SYS.X496", "154", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(821, "", "1625", "1626", "SYS.X291", "SYS.X243", "SYS.X292", "SYS.X429", "SYS.X497", "1627", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(822, "", "1839", "1840", "SYS.X296", "SYS.X293", "SYS.X297", "SYS.X430", "SYS.X498", "1838", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(823, "", "1842", "1843", "SYS.X301", "SYS.X298", "SYS.X302", "SYS.X431", "SYS.X499", "1841", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(824, "", "1663", "", "", "", "", "SYS.X608", "", "1662", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(825, "", "1665", "", "", "", "", "SYS.X609", "", "1664", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(901, "", "334", "561", "SYS.X303", "SYS.X157", "SYS.X304", "SYS.X432", "SYS.X500", "Interest", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(902, "L248", "337", "562", "SYS.X305", "SYS.X158", "SYS.X306", "SYS.X433", "SYS.X501", "Mortgage Insurance Premium", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(903, "L252", "642", "578", "SYS.X307", "SYS.X159", "SYS.X308", "SYS.X434", "SYS.X502", "Hazard Insurance", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(904, "1955", "1849", "1850", "SYS.X391", "SYS.X388", "SYS.X392", "SYS.X435", "SYS.X503", "County Property Taxes", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(905, "1956", "1050", "571", "SYS.X311", "SYS.X235", "SYS.X312", "SYS.X436", "SYS.X504", "VA Funding Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(906, "1500", "643", "579", "SYS.X309", "SYS.X160", "SYS.X310", "SYS.X437", "SYS.X505", "Flood Insurance", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(907, "", "L260", "L261", "SYS.X313", "SYS.X161", "SYS.X314", "SYS.X438", "SYS.X506", "L259", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(908, "", "1667", "1668", "SYS.X315", "SYS.X238", "SYS.X316", "SYS.X439", "SYS.X507", "1666", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1001, "", "656", "596", "SYS.X317", "SYS.X162", "SYS.X318", "SYS.X440", "SYS.X508", "Haz Ins. Reserve", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1002, "", "338", "563", "SYS.X319", "SYS.X163", "SYS.X320", "SYS.X441", "SYS.X509", "Mtg Ins Reserve", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1003, "", "L269", "L270", "SYS.X321", "SYS.X164", "SYS.X322", "SYS.X442", "SYS.X510", "City Property Tax", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1004, "", "655", "595", "SYS.X323", "SYS.X165", "SYS.X324", "SYS.X443", "SYS.X511", "Tax Reserve", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1006, "", "657", "597", "SYS.X325", "SYS.X167", "SYS.X326", "SYS.X444", "SYS.X512", "Flood Ins. Reserve", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1007, "", "1631", "1632", "SYS.X327", "SYS.X239", "SYS.X328", "SYS.X445", "SYS.X513", "1628", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1008, "", "658", "598", "SYS.X329", "SYS.X168", "SYS.X330", "SYS.X446", "SYS.X514", "660", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1009, "", "659", "599", "SYS.X331", "SYS.X169", "SYS.X332", "SYS.X447", "SYS.X515", "661", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1010, "", "558", "", "", "", "", "SYS.X545", "", "Aggregate Adjustment", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1101, "610", "387", "582", "SYS.X333", "SYS.X170", "SYS.X334", "SYS.X448", "SYS.X516", "Closing/Escrow Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1102, "L287", "L288", "L289", "SYS.X403", "SYS.X171", "SYS.X404", "SYS.X449", "SYS.X517", "Abst. or Title Search", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1103, "L290", "L291", "L292", "SYS.X405", "SYS.X172", "SYS.X406", "SYS.X450", "SYS.X518", "Title Examination", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1104, "L293", "L294", "L295", "SYS.X407", "SYS.X173", "SYS.X408", "SYS.X451", "SYS.X519", "Title Ins. Binder", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1105, "395", "396", "583", "SYS.X335", "SYS.X174", "SYS.X336", "SYS.X452", "SYS.X520", "Doc Prep Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1106, "391", "392", "584", "SYS.X337", "SYS.X175", "SYS.X338", "SYS.X453", "SYS.X521", "Notary Fees", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1107, "56", "978", "1049", "SYS.X339", "SYS.X176", "SYS.X340", "SYS.X454", "SYS.X522", "Attorney Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1108, "411", "385", "585", "SYS.X341", "SYS.X177", "SYS.X342", "SYS.X455", "SYS.X523", "Title Company Fee", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1109, "", "646", "592", "SYS.X343", "SYS.X181", "SYS.X344", "SYS.X456", "SYS.X524", "652", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1110, "", "1634", "1635", "SYS.X345", "SYS.X240", "SYS.X346", "SYS.X457", "SYS.X525", "1633", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1111, "", "1763", "1764", "SYS.X347", "SYS.X244", "SYS.X348", "SYS.X458", "SYS.X526", "1762", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1112, "", "1768", "1769", "SYS.X349", "SYS.X245", "SYS.X350", "SYS.X459", "SYS.X527", "1767", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1113, "", "1773", "1774", "SYS.X351", "SYS.X246", "SYS.X352", "SYS.X460", "SYS.X528", "1772", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1114, "", "1778", "1779", "SYS.X353", "SYS.X247", "SYS.X354", "SYS.X461", "SYS.X529", "1777", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1201, "1636", "390", "587", "SYS.X355", "SYS.X182", "SYS.X356", "SYS.X462", "SYS.X530", "Recording Fees", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1202, "1637", "647", "593", "SYS.X357", "SYS.X183", "SYS.X358", "SYS.X463", "SYS.X531", "Local Tax/Stamps", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1203, "1638", "648", "594", "SYS.X359", "SYS.X184", "SYS.X360", "SYS.X464", "SYS.X532", "State Tax/Stamps", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1204, "373", "374", "576", "SYS.X361", "SYS.X185", "SYS.X362", "SYS.X465", "SYS.X533", "User Defined", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1205, "1640", "1641", "1642", "SYS.X363", "SYS.X241", "SYS.X364", "SYS.X466", "SYS.X534", "User Defined", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1206, "1643", "1644", "1645", "SYS.X365", "SYS.X242", "SYS.X366", "SYS.X467", "SYS.X535", "User Defined", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1301, "375", "383", "577", "SYS.X370", "SYS.X186", "SYS.X371", "SYS.X468", "SYS.X536", "Survey to", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1302, "REGZGFE.X15", "339", "564", "SYS.X372", "SYS.X187", "SYS.X373", "SYS.X469", "SYS.X537", "Pest Inspection", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1303, "", "644", "590", "SYS.X374", "SYS.X190", "SYS.X375", "SYS.X470", "SYS.X538", "650", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1304, "", "645", "591", "SYS.X376", "SYS.X191", "SYS.X377", "SYS.X471", "SYS.X539", "651", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1305, "", "41", "42", "SYS.X378", "SYS.X192", "SYS.X379", "SYS.X472", "SYS.X540", "40", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1306, "", "44", "55", "SYS.X380", "SYS.X193", "SYS.X381", "SYS.X473", "SYS.X541", "43", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1307, "", "1783", "1784", "SYS.X382", "SYS.X248", "SYS.X383", "SYS.X474", "SYS.X542", "1782", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1308, "", "1788", "1789", "SYS.X384", "SYS.X249", "SYS.X385", "SYS.X475", "SYS.X543", "1787", ""));
      GFEItemCollection.GFEItems.Add(new GFEItem(1309, "", "1793", "1794", "SYS.X386", "SYS.X250", "SYS.X387", "SYS.X476", "SYS.X544", "1792", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(701, "", "L212", "NEWHUD2.X1", "NEWHUD2.X2", "NEWHUD2.X60", "", "", "NEWHUD2.X141", "NEWHUD2.X142", "Real Estate Commission", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(702, "", "L214", "NEWHUD2.X3", "NEWHUD2.X4", "NEWHUD2.X63", "", "", "NEWHUD2.X143", "NEWHUD2.X144", "Real Estate Commission", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(704, "", "NEWHUD2.X54", "L218", "L219", "NEWHUD2.X66", "", "", "NEWHUD2.X145", "NEWHUD2.X146", "L217", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "a", "NEWHUD2.X104", "454", "559", "SYS.X251", "SYS.X136", "SYS.X252", "SYS.X409", "SYS.X477", "Loan Origination Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "b", "NEWHUD2.X105", "L228", "L229", "SYS.X261", "SYS.X139", "SYS.X262", "SYS.X414", "SYS.X482", "Application Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "c", "NEWHUD2.X106", "1621", "1622", "SYS.X269", "SYS.X233", "SYS.X270", "SYS.X418", "SYS.X486", "Processing Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "d", "NEWHUD2.X107", "367", "569", "SYS.X271", "SYS.X142", "SYS.X272", "SYS.X419", "SYS.X487", "Underwriting Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "e", "NEWHUD2.X108", "439", "572", "SYS.X265", "SYS.X147", "SYS.X266", "SYS.X416", "SYS.X484", "Broker Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "f", "NEWHUD2.X109", "NEWHUD.X225", "NEWHUD.X226", "NEWHUD.X227", "NEWHUD.X228", "NEWHUD.X230", "NEWHUD.X672", "NEWHUD.X673", "Broker Compensation", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "g", "NEWHUD.X1045", "155", "200", "SYS.X289", "SYS.X156", "SYS.X290", "SYS.X428", "SYS.X496", "154", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "h", "NEWHUD.X1046", "1625", "1626", "SYS.X291", "SYS.X243", "SYS.X292", "SYS.X429", "SYS.X497", "1627", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "i", "NEWHUD.X1047", "1839", "1840", "SYS.X296", "SYS.X293", "SYS.X297", "SYS.X430", "SYS.X498", "1838", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "j", "NEWHUD.X1048", "1842", "1843", "SYS.X301", "SYS.X298", "SYS.X302", "SYS.X431", "SYS.X499", "1841", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "k", "NEWHUD.X1049", "NEWHUD.X733", "NEWHUD.X779", "NEWHUD.X748", "NEWHUD.X819", "NEWHUD.X690", "NEWHUD.X716", "NEWHUD.X789", "NEWHUD.X732", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "l", "NEWHUD.X1236", "NEWHUD.X1237", "NEWHUD.X1238", "NEWHUD.X1239", "NEWHUD.X1240", "NEWHUD.X1242", "NEWHUD.X1479", "NEWHUD.X1502", "NEWHUD.X1235", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "m", "NEWHUD.X1244", "NEWHUD.X1245", "NEWHUD.X1246", "NEWHUD.X1247", "NEWHUD.X1248", "NEWHUD.X1250", "NEWHUD.X1480", "NEWHUD.X1503", "NEWHUD.X1243", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "n", "NEWHUD.X1252", "NEWHUD.X1253", "NEWHUD.X1254", "NEWHUD.X1255", "NEWHUD.X1256", "NEWHUD.X1258", "NEWHUD.X1481", "NEWHUD.X1504", "NEWHUD.X1251", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "o", "NEWHUD.X1260", "NEWHUD.X1261", "NEWHUD.X1262", "NEWHUD.X1263", "NEWHUD.X1264", "NEWHUD.X1266", "NEWHUD.X1482", "NEWHUD.X1505", "NEWHUD.X1259", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "p", "NEWHUD.X1268", "NEWHUD.X1269", "NEWHUD.X1270", "NEWHUD.X1271", "NEWHUD.X1272", "NEWHUD.X1274", "NEWHUD.X1483", "NEWHUD.X1506", "NEWHUD.X1267", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "q", "NEWHUD.X1276", "NEWHUD.X1277", "NEWHUD.X1278", "NEWHUD.X1279", "NEWHUD.X1280", "NEWHUD.X1282", "NEWHUD.X1484", "NEWHUD.X1507", "NEWHUD.X1275", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(801, "r", "NEWHUD.X1284", "NEWHUD.X1285", "NEWHUD.X1286", "NEWHUD.X1287", "NEWHUD.X1288", "NEWHUD.X1290", "NEWHUD.X1485", "NEWHUD.X1508", "NEWHUD.X1283", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "", "NEWHUD.X687", "NEWHUD.X780", "NEWHUD.X749", "NEWHUD.X820", "NEWHUD.X627", "NEWHUD.X398", "NEWHUD.X790", "Your Credit or Points", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "a", "", "NEWHUD.X1142", "", "NEWHUD.X1167", "", "NEWHUD.X1168", "NEWHUD.X1231", "", "Lender Compensation Credit", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "b", "", "NEWHUD.X1144", "", "NEWHUD.X1169", "", "NEWHUD.X1170", "NEWHUD.X1232", "", "Origination Credit", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "c", "", "NEWHUD.X1146", "", "NEWHUD.X1171", "", "NEWHUD.X1172", "NEWHUD.X1233", "", "NEWHUD.X1145", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "d", "", "NEWHUD.X1148", "", "NEWHUD.X1173", "", "NEWHUD.X1174", "NEWHUD.X1234", "", "NEWHUD.X1147", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "e", "NEWHUD2.X111", "NEWHUD.X1151", "NEWHUD.X1152", "NEWHUD.X1175", "NEWHUD.X1176", "NEWHUD.X1178", "NEWHUD.X1217", "NEWHUD.X1218", "Origination Points", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "f", "NEWHUD2.X112", "NEWHUD.X1155", "NEWHUD.X1156", "NEWHUD.X1179", "NEWHUD.X1180", "NEWHUD.X1182", "NEWHUD.X1219", "NEWHUD.X1220", "NEWHUD.X1153", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "g", "NEWHUD2.X113", "NEWHUD.X1159", "NEWHUD.X1160", "NEWHUD.X1183", "NEWHUD.X1184", "NEWHUD.X1186", "NEWHUD.X1221", "NEWHUD.X1222", "NEWHUD.X1157", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(802, "h", "NEWHUD2.X114", "NEWHUD.X1163", "NEWHUD.X1164", "NEWHUD.X1187", "NEWHUD.X1188", "NEWHUD.X1190", "NEWHUD.X1223", "NEWHUD.X1224", "NEWHUD.X1161", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(803, "x", "NEWHUD2.X8", "NEWHUD2.X9", "NEWHUD2.X10", "NEWHUD2.X69", "", "", "NEWHUD2.X147", "NEWHUD2.X148", "NEWHUD2.X7", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(804, "617", "641", "581", "SYS.X255", "SYS.X231", "SYS.X256", "SYS.X411", "SYS.X479", "Appraisal Fee", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(805, "624", "640", "580", "SYS.X257", "SYS.X232", "SYS.X258", "SYS.X412", "SYS.X480", "Credit Report", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(806, "L224", "336", "565", "SYS.X267", "SYS.X141", "SYS.X268", "SYS.X417", "SYS.X485", "Tax Service", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(807, "NEWHUD.X399", "NEWHUD.X400", "NEWHUD.X781", "NEWHUD.X742", "NEWHUD.X751", "NEWHUD.X628", "NEWHUD.X674", "NEWHUD.X681", "Flood Certification", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(808, "NEWHUD.X1050", "NEWHUD.X136", "NEWHUD.X147", "NEWHUD.X157", "NEWHUD.X752", "NEWHUD.X189", "NEWHUD.X358", "NEWHUD.X378", "NEWHUD.X126", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(809, "NEWHUD.X1051", "NEWHUD.X137", "NEWHUD.X148", "NEWHUD.X158", "NEWHUD.X753", "NEWHUD.X190", "NEWHUD.X359", "NEWHUD.X379", "NEWHUD.X127", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(810, "NEWHUD.X1052", "NEWHUD.X138", "NEWHUD.X149", "NEWHUD.X159", "NEWHUD.X754", "NEWHUD.X191", "NEWHUD.X360", "NEWHUD.X380", "NEWHUD.X128", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(811, "NEWHUD.X1053", "NEWHUD.X139", "NEWHUD.X150", "NEWHUD.X160", "NEWHUD.X755", "NEWHUD.X192", "NEWHUD.X361", "NEWHUD.X381", "NEWHUD.X129", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(812, "NEWHUD.X1054", "NEWHUD.X140", "NEWHUD.X151", "NEWHUD.X161", "NEWHUD.X756", "NEWHUD.X193", "NEWHUD.X362", "NEWHUD.X382", "NEWHUD.X130", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(813, "NEWHUD.X1055", "370", "574", "SYS.X275", "SYS.X149", "SYS.X276", "SYS.X421", "SYS.X489", "369", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(814, "NEWHUD.X1056", "372", "575", "SYS.X277", "SYS.X150", "SYS.X278", "SYS.X422", "SYS.X490", "371", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(815, "NEWHUD.X1057", "349", "96", "SYS.X279", "SYS.X151", "SYS.X280", "SYS.X423", "SYS.X491", "348", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(816, "NEWHUD.X1058", "932", "1345", "SYS.X281", "SYS.X152", "SYS.X282", "SYS.X424", "SYS.X492", "931", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(817, "NEWHUD.X1059", "1009", "6", "SYS.X283", "SYS.X153", "SYS.X284", "SYS.X425", "SYS.X493", "1390", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(818, "NEWHUD.X1292", "NEWHUD.X1293", "NEWHUD.X1294", "NEWHUD.X1295", "NEWHUD.X1296", "NEWHUD.X1298", "NEWHUD.X1486", "NEWHUD.X1509", "NEWHUD.X1291", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(819, "NEWHUD.X1300", "NEWHUD.X1301", "NEWHUD.X1302", "NEWHUD.X1303", "NEWHUD.X1304", "NEWHUD.X1306", "NEWHUD.X1487", "NEWHUD.X1510", "NEWHUD.X1299", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(820, "NEWHUD.X1308", "NEWHUD.X1309", "NEWHUD.X1310", "NEWHUD.X1311", "NEWHUD.X1312", "NEWHUD.X1314", "NEWHUD.X1488", "NEWHUD.X1511", "NEWHUD.X1307", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(821, "NEWHUD.X1316", "NEWHUD.X1317", "NEWHUD.X1318", "NEWHUD.X1319", "NEWHUD.X1320", "NEWHUD.X1322", "NEWHUD.X1489", "NEWHUD.X1512", "NEWHUD.X1315", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(822, "NEWHUD.X1324", "NEWHUD.X1325", "NEWHUD.X1326", "NEWHUD.X1327", "NEWHUD.X1328", "NEWHUD.X1330", "NEWHUD.X1490", "NEWHUD.X1513", "NEWHUD.X1323", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(823, "NEWHUD.X1332", "NEWHUD.X1333", "NEWHUD.X1334", "NEWHUD.X1335", "NEWHUD.X1336", "NEWHUD.X1338", "NEWHUD.X1491", "NEWHUD.X1514", "NEWHUD.X1331", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(824, "NEWHUD.X1340", "NEWHUD.X1341", "NEWHUD.X1342", "NEWHUD.X1343", "NEWHUD.X1344", "NEWHUD.X1346", "NEWHUD.X1492", "NEWHUD.X1515", "NEWHUD.X1339", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(825, "NEWHUD.X1348", "NEWHUD.X1349", "NEWHUD.X1350", "NEWHUD.X1351", "NEWHUD.X1352", "NEWHUD.X1354", "NEWHUD.X1493", "NEWHUD.X1516", "NEWHUD.X1347", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(826, "NEWHUD.X1356", "NEWHUD.X1357", "NEWHUD.X1358", "NEWHUD.X1359", "NEWHUD.X1360", "NEWHUD.X1362", "NEWHUD.X1494", "NEWHUD.X1517", "NEWHUD.X1355", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(827, "NEWHUD.X1364", "NEWHUD.X1365", "NEWHUD.X1366", "NEWHUD.X1367", "NEWHUD.X1368", "NEWHUD.X1370", "NEWHUD.X1495", "NEWHUD.X1518", "NEWHUD.X1363", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(828, "NEWHUD.X1372", "NEWHUD.X1373", "NEWHUD.X1374", "NEWHUD.X1375", "NEWHUD.X1376", "NEWHUD.X1378", "NEWHUD.X1496", "NEWHUD.X1519", "NEWHUD.X1371", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(829, "NEWHUD.X1380", "NEWHUD.X1381", "NEWHUD.X1382", "NEWHUD.X1383", "NEWHUD.X1384", "NEWHUD.X1386", "NEWHUD.X1497", "NEWHUD.X1520", "NEWHUD.X1379", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(830, "NEWHUD.X1388", "NEWHUD.X1389", "NEWHUD.X1390", "NEWHUD.X1391", "NEWHUD.X1392", "NEWHUD.X1394", "NEWHUD.X1498", "NEWHUD.X1521", "NEWHUD.X1387", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(831, "NEWHUD.X1396", "NEWHUD.X1397", "NEWHUD.X1398", "NEWHUD.X1399", "NEWHUD.X1400", "NEWHUD.X1402", "NEWHUD.X1499", "NEWHUD.X1522", "NEWHUD.X1395", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(832, "NEWHUD.X1404", "NEWHUD.X1405", "NEWHUD.X1406", "NEWHUD.X1407", "NEWHUD.X1408", "NEWHUD.X1410", "NEWHUD.X1500", "NEWHUD.X1523", "NEWHUD.X1403", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(833, "NEWHUD.X1412", "NEWHUD.X1413", "NEWHUD.X1414", "NEWHUD.X1415", "NEWHUD.X1416", "NEWHUD.X1418", "NEWHUD.X1501", "NEWHUD.X1524", "NEWHUD.X1411", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(834, "NEWHUD.X1060", "554", "678", "SYS.X285", "SYS.X154", "SYS.X286", "SYS.X426", "SYS.X494", "410", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(835, "NEWHUD.X1061", "NEWHUD.X657", "NEWHUD.X658", "NEWHUD.X162", "NEWHUD.X757", "NEWHUD.X660", "SYS.X427", "SYS.X495", "NEWHUD.X656", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(901, "NEWHUD2.X115", "334", "561", "SYS.X303", "SYS.X157", "SYS.X304", "SYS.X432", "SYS.X500", "Daily Interest Charges", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(902, "L248", "337", "562", "SYS.X305", "SYS.X158", "SYS.X306", "SYS.X433", "SYS.X501", "Mortgage Insurance Premium", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(903, "L252", "642", "578", "SYS.X307", "SYS.X159", "SYS.X308", "SYS.X434", "SYS.X502", "Homeowner's Insurance", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(904, "NEWHUD.X1062", "NEWHUD.X591", "NEWHUD.X594", "NEWHUD.X163", "NEWHUD.X647", "NEWHUD.X630", "NEWHUD.X675", "NEWHUD.X682", "Property Taxes", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(905, "1956", "1050", "571", "SYS.X311", "SYS.X235", "SYS.X312", "SYS.X436", "SYS.X504", "VA Funding Fee", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(906, "1500", "643", "579", "SYS.X309", "SYS.X160", "SYS.X310", "SYS.X437", "SYS.X505", "Flood Insurance", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(907, "NEWHUD.X1063", "L260", "L261", "SYS.X313", "SYS.X161", "SYS.X314", "SYS.X438", "SYS.X506", "L259", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(908, "NEWHUD.X1064", "1667", "1668", "SYS.X315", "SYS.X238", "SYS.X316", "SYS.X439", "SYS.X507", "1666", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(909, "NEWHUD.X1065", "NEWHUD.X592", "NEWHUD.X595", "NEWHUD.X164", "NEWHUD.X648", "NEWHUD.X625", "NEWHUD.X676", "NEWHUD.X683", "NEWHUD.X583", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(910, "NEWHUD.X1066", "NEWHUD.X593", "NEWHUD.X596", "NEWHUD.X165", "NEWHUD.X649", "NEWHUD.X626", "NEWHUD.X677", "NEWHUD.X684", "NEWHUD.X584", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(911, "NEWHUD.X1587", "NEWHUD.X1588", "NEWHUD.X1589", "NEWHUD.X1590", "NEWHUD.X1591", "NEWHUD.X1593", "NEWHUD.X1684", "NEWHUD.X1694", "NEWHUD.X1586", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(912, "NEWHUD.X1595", "NEWHUD.X1596", "NEWHUD.X1597", "NEWHUD.X1598", "NEWHUD.X1599", "NEWHUD.X1601", "NEWHUD.X1685", "NEWHUD.X1695", "NEWHUD.X1594", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1002, "NEWHUD2.X116", "656", "596", "SYS.X317", "", "SYS.X318", "SYS.X440", "SYS.X508", "Homeowner's Ins.", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1003, "NEWHUD2.X117", "338", "563", "SYS.X319", "", "SYS.X320", "SYS.X441", "SYS.X509", "Mortgage Ins.", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1004, "NEWHUD2.X118", "655", "595", "SYS.X323", "", "SYS.X322", "SYS.X443", "SYS.X511", "Property Taxes", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1005, "NEWHUD2.X119", "L269", "L270", "SYS.X321", "", "SYS.X324", "SYS.X442", "SYS.X510", "City Property Tax", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1006, "NEWHUD2.X120", "657", "597", "SYS.X325", "", "SYS.X326", "SYS.X444", "SYS.X512", "Flood Ins. Reserve", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1007, "VEND.X341", "1631", "1632", "SYS.X327", "", "SYS.X328", "SYS.X445", "SYS.X513", "1628", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1008, "VEND.X350", "658", "598", "SYS.X329", "", "SYS.X330", "SYS.X446", "SYS.X514", "660", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1009, "VEND.X359", "659", "599", "SYS.X331", "", "SYS.X332", "SYS.X447", "SYS.X515", "661", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1010, "NEWHUD.X1705", "NEWHUD.X1708", "NEWHUD.X1714", "NEWHUD.X1710", "", "", "NEWHUD.X1712", "NEWHUD.X1715", "USDA Annual Fee", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1011, "", "558", "", "", "", "", "SYS.X545", "", "Aggregate Adjustment", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "x", "NEWHUD.X202", "NEWHUD.X571", "NEWHUD.X798", "", "", "", "", "", "Title Services and Lender's Title Insurance", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "a", "NEWHUD.X1070", "NEWHUD.X952", "NEWHUD.X953", "NEWHUD.X955", "NEWHUD.X956", "NEWHUD.X959", "NEWHUD.X1033", "NEWHUD.X1039", "NEWHUD.X951", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "b", "NEWHUD.X1071", "NEWHUD.X961", "NEWHUD.X962", "NEWHUD.X964", "NEWHUD.X965", "NEWHUD.X968", "NEWHUD.X1034", "NEWHUD.X1040", "NEWHUD.X960", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "c", "NEWHUD.X1072", "NEWHUD.X970", "NEWHUD.X971", "NEWHUD.X973", "NEWHUD.X974", "NEWHUD.X977", "NEWHUD.X1035", "NEWHUD.X1041", "NEWHUD.X969", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "d", "NEWHUD.X1073", "NEWHUD.X979", "NEWHUD.X980", "NEWHUD.X982", "NEWHUD.X983", "NEWHUD.X986", "NEWHUD.X1036", "NEWHUD.X1042", "NEWHUD.X978", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "e", "NEWHUD.X1074", "NEWHUD.X988", "NEWHUD.X989", "NEWHUD.X991", "NEWHUD.X992", "NEWHUD.X995", "NEWHUD.X1037", "NEWHUD.X1043", "NEWHUD.X987", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1101, "f", "NEWHUD.X1075", "NEWHUD.X997", "NEWHUD.X998", "NEWHUD.X1000", "NEWHUD.X1001", "NEWHUD.X1004", "NEWHUD.X1038", "NEWHUD.X1044", "NEWHUD.X996", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "NEWHUD.X203", "NEWHUD.X645", "NEWHUD.X782", "NEWHUD.X743", "NEWHUD.X758", "NEWHUD.X803", "NEWHUD.X369", "NEWHUD.X389", "Settlement or Closing Fee", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "a", "NEWHUD.X203", "NEWHUD2.X11", "NEWHUD2.X12", "NEWHUD2.X72", "", "", "NEWHUD2.X149", "NEWHUD2.X150", "Settlement Fee", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "b", "NEWHUD2.X13", "NEWHUD2.X14", "NEWHUD2.X15", "NEWHUD2.X75", "", "", "NEWHUD2.X151", "NEWHUD2.X152", "Closing Fee", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "c", "NEWHUD2.X16", "NEWHUD.X808", "NEWHUD2.X17", "NEWHUD2.X78", "", "", "NEWHUD2.X153", "NEWHUD2.X154", "Escrow Fee", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "d", "NEWHUD2.X55", "NEWHUD.X810", "NEWHUD2.X18", "NEWHUD2.X81", "", "", "NEWHUD2.X155", "NEWHUD2.X156", "NEWHUD.X809", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "e", "NEWHUD2.X56", "NEWHUD.X812", "NEWHUD2.X19", "NEWHUD2.X84", "", "", "NEWHUD2.X157", "NEWHUD2.X158", "NEWHUD.X811", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "f", "NEWHUD2.X57", "NEWHUD.X814", "NEWHUD2.X20", "NEWHUD2.X87", "", "", "NEWHUD2.X159", "NEWHUD2.X160", "NEWHUD.X813", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "g", "NEWHUD2.X58", "NEWHUD.X816", "NEWHUD2.X21", "NEWHUD2.X90", "", "", "NEWHUD2.X161", "NEWHUD2.X162", "NEWHUD.X815", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1102, "h", "NEWHUD2.X59", "NEWHUD.X818", "NEWHUD2.X22", "NEWHUD2.X93", "", "", "NEWHUD2.X163", "NEWHUD2.X164", "NEWHUD.X817", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1103, "NEWHUD.X204", "NEWHUD.X572", "NEWHUD.X783", "NEWHUD.X744", "NEWHUD.X759", "NEWHUD.X804", "NEWHUD.X370", "NEWHUD.X390", "Owner's Title Insurance", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1104, "NEWHUD.X205", "NEWHUD.X639", "NEWHUD.X784", "NEWHUD.X745", "NEWHUD.X760", "NEWHUD.X805", "NEWHUD.X371", "NEWHUD.X791", "Lender's Title Insurance", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1109, "NEWHUD.X1076", "NEWHUD.X215", "NEWHUD.X218", "NEWHUD.X221", "NEWHUD.X763", "NEWHUD.X244", "NEWHUD.X680", "NEWHUD.X685", "NEWHUD.X208", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1110, "NEWHUD.X1077", "NEWHUD.X216", "NEWHUD.X219", "NEWHUD.X222", "NEWHUD.X764", "NEWHUD.X245", "NEWHUD.X372", "NEWHUD.X392", "NEWHUD.X209", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1111, "NEWHUD.X1078", "1763", "1764", "SYS.X347", "SYS.X244", "SYS.X348", "SYS.X458", "SYS.X526", "1762", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1112, "NEWHUD.X1079", "1768", "1769", "SYS.X349", "SYS.X245", "SYS.X350", "SYS.X459", "SYS.X527", "1767", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1113, "NEWHUD.X1080", "1773", "1774", "SYS.X351", "SYS.X246", "SYS.X352", "SYS.X460", "SYS.X528", "1772", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1114, "NEWHUD.X1081", "1778", "1779", "SYS.X353", "SYS.X247", "SYS.X354", "SYS.X461", "SYS.X529", "1777", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1115, "NEWHUD.X1603", "NEWHUD.X1604", "NEWHUD.X1605", "NEWHUD.X1606", "NEWHUD.X1607", "NEWHUD.X1609", "NEWHUD.X1686", "NEWHUD.X1696", "NEWHUD.X1602", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1116, "NEWHUD.X1611", "NEWHUD.X1612", "NEWHUD.X1613", "NEWHUD.X1614", "NEWHUD.X1615", "NEWHUD.X1617", "NEWHUD.X1687", "NEWHUD.X1697", "NEWHUD.X1610", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1202, "1636", "390", "587", "SYS.X355", "SYS.X182", "", "SYS.X462", "SYS.X530", "Recording Fees", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1203, "NEWHUD.X947", "NEWHUD.X731", "NEWHUD.X787", "NEWHUD.X261", "NEWHUD.X765", "", "NEWHUD.X373", "NEWHUD.X393", "Transfer Taxes", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1204, "1637", "647", "593", "SYS.X357", "SYS.X183", "", "SYS.X463", "SYS.X531", "City/County Tax/Stamps", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1205, "1638", "648", "594", "SYS.X359", "SYS.X184", "", "SYS.X464", "SYS.X532", "State Tax/Stamps", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1206, "NEWHUD.X1082", "374", "576", "SYS.X361", "SYS.X185", "", "SYS.X465", "SYS.X533", "373", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1207, "NEWHUD.X1083", "1641", "1642", "SYS.X363", "SYS.X241", "", "SYS.X466", "SYS.X534", "1640", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1208, "NEWHUD.X1084", "1644", "1645", "SYS.X365", "SYS.X242", "", "SYS.X467", "SYS.X535", "1643", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1209, "NEWHUD.X1619", "NEWHUD.X1620", "NEWHUD.X1621", "NEWHUD.X1622", "NEWHUD.X1623", "", "NEWHUD.X1688", "NEWHUD.X1698", "NEWHUD.X1618", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1210, "NEWHUD.X1626", "NEWHUD.X1627", "NEWHUD.X1628", "NEWHUD.X1629", "NEWHUD.X1630", "", "NEWHUD.X1689", "NEWHUD.X1699", "NEWHUD.X1625", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1302, "NEWHUD.X1085", "NEWHUD.X254", "NEWHUD.X258", "NEWHUD.X262", "NEWHUD.X766", "NEWHUD.X274", "NEWHUD.X375", "NEWHUD.X395", "NEWHUD.X251", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1303, "NEWHUD.X1086", "644", "590", "SYS.X374", "SYS.X190", "SYS.X375", "SYS.X470", "SYS.X538", "650", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1304, "NEWHUD.X1087", "645", "591", "SYS.X376", "SYS.X191", "SYS.X377", "SYS.X471", "SYS.X539", "651", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1305, "NEWHUD.X1088", "41", "42", "SYS.X378", "SYS.X192", "SYS.X379", "SYS.X472", "SYS.X540", "40", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1306, "NEWHUD.X1089", "44", "55", "SYS.X380", "SYS.X193", "SYS.X381", "SYS.X473", "SYS.X541", "43", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1307, "NEWHUD.X1090", "1783", "1784", "SYS.X382", "SYS.X248", "SYS.X383", "SYS.X474", "SYS.X542", "1782", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1308, "NEWHUD.X1091", "1788", "1789", "SYS.X384", "SYS.X249", "SYS.X385", "SYS.X475", "SYS.X543", "1787", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1309, "NEWHUD.X1092", "1793", "1794", "SYS.X386", "SYS.X250", "SYS.X387", "SYS.X476", "SYS.X544", "1792", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1310, "NEWHUD.X1093", "NEWHUD.X255", "NEWHUD.X259", "NEWHUD.X263", "NEWHUD.X767", "NEWHUD.X275", "NEWHUD.X376", "NEWHUD.X396", "NEWHUD.X252", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1311, "NEWHUD.X1094", "NEWHUD.X256", "NEWHUD.X260", "NEWHUD.X264", "NEWHUD.X768", "NEWHUD.X276", "NEWHUD.X377", "NEWHUD.X397", "NEWHUD.X253", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1312, "NEWHUD.X1633", "NEWHUD.X1634", "NEWHUD.X1635", "NEWHUD.X1636", "NEWHUD.X1637", "NEWHUD.X1639", "NEWHUD.X1690", "NEWHUD.X1700", "NEWHUD.X1632", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1313, "NEWHUD.X1641", "NEWHUD.X1642", "NEWHUD.X1643", "NEWHUD.X1644", "NEWHUD.X1645", "NEWHUD.X1647", "NEWHUD.X1691", "NEWHUD.X1701", "NEWHUD.X1640", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1314, "NEWHUD.X1649", "NEWHUD.X1650", "NEWHUD.X1651", "NEWHUD.X1652", "NEWHUD.X1653", "NEWHUD.X1655", "NEWHUD.X1692", "NEWHUD.X1702", "NEWHUD.X1648", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1315, "NEWHUD.X1657", "NEWHUD.X1658", "NEWHUD.X1659", "NEWHUD.X1660", "NEWHUD.X1661", "NEWHUD.X1663", "NEWHUD.X1693", "NEWHUD.X1703", "NEWHUD.X1656", ""));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1316, "NEWHUD2.X4611", "NEWHUD2.X4612", "NEWHUD2.X4613", "NEWHUD2.X4614", "POPT2.X13", "NEWHUD2.X4616", "NEWHUD2.X4650", "NEWHUD2.X4651", "NEWHUD2.X4610", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1317, "NEWHUD2.X4618", "NEWHUD2.X4619", "NEWHUD2.X4620", "NEWHUD2.X4621", "POPT2.X14", "NEWHUD2.X4623", "NEWHUD2.X4652", "NEWHUD2.X4653", "NEWHUD2.X4617", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1318, "NEWHUD2.X4625", "NEWHUD2.X4626", "NEWHUD2.X4627", "NEWHUD2.X4628", "POPT2.X15", "NEWHUD2.X4630", "NEWHUD2.X4654", "NEWHUD2.X4655", "NEWHUD2.X4624", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1319, "NEWHUD2.X4632", "NEWHUD2.X4633", "NEWHUD2.X4634", "NEWHUD2.X4635", "POPT2.X16", "NEWHUD2.X4637", "NEWHUD2.X4656", "NEWHUD2.X4657", "NEWHUD2.X4631", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(1320, "NEWHUD2.X4639", "NEWHUD2.X4640", "NEWHUD2.X4641", "NEWHUD2.X4642", "POPT2.X17", "NEWHUD2.X4644", "NEWHUD2.X4658", "NEWHUD2.X4659", "NEWHUD2.X4638", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(2001, "NEWHUD2.X4661", "NEWHUD2.X4662", "NEWHUD2.X4663", "NEWHUD2.X4752", "POPT2.X18", "NEWHUD2.X4669", "", "", "NEWHUD2.X4660", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(2002, "NEWHUD2.X4684", "NEWHUD2.X4685", "NEWHUD2.X4686", "NEWHUD2.X4753", "POPT2.X19", "NEWHUD2.X4692", "", "", "NEWHUD2.X4683", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(2003, "NEWHUD2.X4707", "NEWHUD2.X4708", "NEWHUD2.X4709", "NEWHUD2.X4754", "POPT2.X20", "NEWHUD2.X4715", "", "", "NEWHUD2.X4706", "Y"));
      GFEItemCollection.GFEItems2010.Add(new GFEItem(2004, "NEWHUD2.X4730", "NEWHUD2.X4731", "NEWHUD2.X4732", "NEWHUD2.X4755", "POPT2.X21", "NEWHUD2.X4738", "", "", "NEWHUD2.X4729", "Y"));
      foreach (GFEItem gfeItem in GFEItemCollection.GFEItems2010)
      {
        if (!GFEItemCollection.GFEItems2010ByLnNum.ContainsKey(gfeItem.LineNumber))
          GFEItemCollection.GFEItems2010ByLnNum.Add(gfeItem.LineNumber, gfeItem);
        string key = gfeItem.LineNumber.ToString("0000") + "|" + gfeItem.ComponentID;
        if (!GFEItemCollection.GFEItems2010ByLnNumCpntId.ContainsKey(key))
          GFEItemCollection.GFEItems2010ByLnNumCpntId.Add(key, gfeItem);
      }
    }

    public static GFEItem FindGFEItem2010ByLineNumber(int lineNumber)
    {
      GFEItem gfeItem;
      return !GFEItemCollection.GFEItems2010ByLnNum.TryGetValue(lineNumber, out gfeItem) ? (GFEItem) null : gfeItem;
    }

    public static GFEItem FindGFEItem2010ByLineNumberAndComponentId(
      int lineNumber,
      string componentId)
    {
      return GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentIdInner(lineNumber.ToString("0000"), componentId);
    }

    public static GFEItem FindGFEItem2010ByLineNumberAndComponentId(
      string lineNumber,
      string componentId)
    {
      return GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentIdInner(lineNumber.PadLeft(4, '0'), componentId);
    }

    private static GFEItem FindGFEItem2010ByLineNumberAndComponentIdInner(
      string lineNumber,
      string componentId)
    {
      string key = lineNumber + "|" + (componentId ?? string.Empty);
      GFEItem gfeItem;
      return !GFEItemCollection.GFEItems2010ByLnNumCpntId.TryGetValue(key, out gfeItem) ? (GFEItem) null : gfeItem;
    }
  }
}
