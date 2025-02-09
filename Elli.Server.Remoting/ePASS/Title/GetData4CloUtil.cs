// Decompiled with JetBrains decompiler
// Type: ePASS.Title.GetData4CloUtil
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;

#nullable disable
namespace ePASS.Title
{
  public class GetData4CloUtil
  {
    private static readonly string[] summaryOfTransactionsIDs = new string[133]
    {
      "CD3.X41",
      "L726",
      "L79",
      "CD3.X1",
      "L84",
      "L85",
      "L88",
      "L89",
      "CD3.X2",
      "CD3.X3",
      "CD3.X4",
      "CD3.X5",
      "L92",
      "L93",
      "L94",
      "L98",
      "L99",
      "L100",
      "L104",
      "L105",
      "L106",
      "L110",
      "L111",
      "L114",
      "L115",
      "L118",
      "L119",
      "L122",
      "L123",
      "CD3.X6",
      "CD3.X7",
      "CD3.X42",
      "L128",
      "2",
      "L132",
      "L134",
      "L135",
      "CD3.X9",
      "CD3.X10",
      "CD3.X11",
      "CD3.X12",
      "CD3.X13",
      "CD3.X14",
      "CD3.X15",
      "CD3.X16",
      "CD3.X17",
      "CD3.X18",
      "CD3.X19",
      "CD3.X20",
      "L156",
      "L157",
      "L158",
      "L162",
      "L163",
      "L164",
      "L168",
      "L169",
      "L170",
      "L174",
      "L175",
      "L178",
      "L179",
      "L182",
      "L183",
      "CD3.X43",
      "L80",
      "L81",
      "L82",
      "L86",
      "L87",
      "L90",
      "L91",
      "CD3.X24",
      "CD3.X25",
      "CD3.X26",
      "CD3.X27",
      "CD3.X28",
      "CD3.X29",
      "L95",
      "L96",
      "L97",
      "L101",
      "L102",
      "L103",
      "L107",
      "L108",
      "L109",
      "L112",
      "L113",
      "L116",
      "L117",
      "L120",
      "L121",
      "L124",
      "L125",
      "CD3.X30",
      "CD3.X31",
      "CD3.X44",
      "L129",
      "CD3.X46",
      "L133",
      "L136",
      "L139",
      "L142",
      "L143",
      "L146",
      "L147",
      "CD3.X108",
      "L150",
      "L151",
      "L154",
      "L155",
      "CD3.X32",
      "CD3.X33",
      "CD3.X34",
      "CD3.X35",
      "CD3.X36",
      "CD3.X37",
      "L159",
      "L160",
      "L161",
      "L165",
      "L166",
      "L167",
      "L171",
      "L172",
      "L173",
      "L176",
      "L177",
      "L180",
      "L181",
      "L184",
      "L185"
    };

    private static string getFieldValue(IBam bam, string fieldID)
    {
      return string.IsNullOrWhiteSpace(fieldID) ? "" : bam.GetSimpleField(fieldID);
    }

    public static Dictionary<string, string> GetSummaryOfTransactions(IBam bam)
    {
      Dictionary<string, string> summaryOfTransactions = new Dictionary<string, string>();
      foreach (string ofTransactionsId in GetData4CloUtil.summaryOfTransactionsIDs)
      {
        string fieldValue = GetData4CloUtil.getFieldValue(bam, ofTransactionsId);
        if (!summaryOfTransactions.ContainsKey(ofTransactionsId) && !string.IsNullOrWhiteSpace(fieldValue))
          summaryOfTransactions.Add(ofTransactionsId, fieldValue);
      }
      return summaryOfTransactions;
    }
  }
}
