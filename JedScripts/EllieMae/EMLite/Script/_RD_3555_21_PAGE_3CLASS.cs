// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RD_3555_21_PAGE_3CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScript;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _RD_3555_21_PAGE_3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("USDA_X48", JS.GetStr(loan, "USDA.X48"));
      nameValueCollection.Add("USDA_X49", JS.GetStr(loan, "USDA.X49"));
      nameValueCollection.Add("USDA_X50", JS.GetStr(loan, "USDA.X50"));
      nameValueCollection.Add("USDA_X51", JS.GetStr(loan, "USDA.X50"));
      nameValueCollection.Add("USDA_X52", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X52"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X52"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X48"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X53", JS.GetStr(loan, "USDA.X53"));
      nameValueCollection.Add("USDA_X56", JS.GetStr(loan, "USDA.X56"));
      nameValueCollection.Add("USDA_X57", JS.GetStr(loan, "USDA.X57"));
      nameValueCollection.Add("USDA_X58", JS.GetStr(loan, "USDA.X58"));
      nameValueCollection.Add("USDA_X59", JS.GetStr(loan, "USDA.X59"));
      nameValueCollection.Add("USDA_X60", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X60"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X60"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X56"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X61", JS.GetStr(loan, "USDA.X61"));
      nameValueCollection.Add("USDA_X64", JS.GetStr(loan, "USDA.X64"));
      nameValueCollection.Add("USDA_X65", JS.GetStr(loan, "USDA.X65"));
      nameValueCollection.Add("USDA_X66", JS.GetStr(loan, "USDA.X66"));
      nameValueCollection.Add("USDA_X67", JS.GetStr(loan, "USDA.X67"));
      nameValueCollection.Add("USDA_X68", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X68"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X68"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X64"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X69", JS.GetStr(loan, "USDA.X69"));
      nameValueCollection.Add("USDA_X72", JS.GetStr(loan, "USDA.X72"));
      nameValueCollection.Add("USDA_X73", JS.GetStr(loan, "USDA.X73"));
      nameValueCollection.Add("USDA_X74", JS.GetStr(loan, "USDA.X74"));
      nameValueCollection.Add("USDA_X75", JS.GetStr(loan, "USDA.X75"));
      nameValueCollection.Add("USDA_X76", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X76"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X76"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X72"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X77", JS.GetStr(loan, "USDA.X77"));
      nameValueCollection.Add("USDA_X80", JS.GetStr(loan, "USDA.X80"));
      nameValueCollection.Add("USDA_X81", JS.GetStr(loan, "USDA.X81"));
      nameValueCollection.Add("USDA_X82", JS.GetStr(loan, "USDA.X82"));
      nameValueCollection.Add("USDA_X83", JS.GetStr(loan, "USDA.X83"));
      nameValueCollection.Add("USDA_X84", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X84"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X84"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X80"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X85", JS.GetStr(loan, "USDA.X85"));
      nameValueCollection.Add("USDA_X88", JS.GetStr(loan, "USDA.X88"));
      nameValueCollection.Add("USDA_X89", JS.GetStr(loan, "USDA.X89"));
      nameValueCollection.Add("USDA_X90", JS.GetStr(loan, "USDA.X90"));
      nameValueCollection.Add("USDA_X91", JS.GetStr(loan, "USDA.X91"));
      nameValueCollection.Add("USDA_X92", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X92"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X92"), "0.00", false) != 0, "Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X88"), "", false) != 0, "N", "")));
      nameValueCollection.Add("USDA_X93", JS.GetStr(loan, "USDA.X93"));
      nameValueCollection.Add("USDA_X164", JS.GetStr(loan, "USDA.X164"));
      nameValueCollection.Add("USDA_X165", JS.GetStr(loan, "USDA.X165"));
      nameValueCollection.Add("USDA_X168", JS.GetStr(loan, "USDA.X168"));
      nameValueCollection.Add("USDA_X167", JS.GetStr(loan, "USDA.X167"));
      nameValueCollection.Add("USDA_X170", JS.GetStr(loan, "USDA.X170"));
      nameValueCollection.Add("USDA_X16", JS.GetStr(loan, "USDA.X16"));
      nameValueCollection.Add("USDA_X209", JS.GetStr(loan, "USDA.X209"));
      nameValueCollection.Add("USDA_X210", JS.GetStr(loan, "USDA.X210"));
      nameValueCollection.Add("USDA_X211", JS.GetStr(loan, "USDA.X211"));
      nameValueCollection.Add("USDA_X212", JS.GetStr(loan, "USDA.X212"));
      nameValueCollection.Add("USDA_X213", JS.GetStr(loan, "USDA.X213"));
      return nameValueCollection;
    }
  }
}
