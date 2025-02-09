// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003_PG4_ASSETS_LIABILITIES__2006_CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _1003_PG4_ASSETS_LIABILITIES__2006_CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("305", JS.GetStr(loan, "305"));
      nameValueCollection.Add("VOD_1", "");
      nameValueCollection.Add("VOD_2", "");
      nameValueCollection.Add("VOD_3", "");
      nameValueCollection.Add("VOD_4", "");
      nameValueCollection.Add("VOD_5", "");
      nameValueCollection.Add("VOD_6", "");
      nameValueCollection.Add("VOD_7", "");
      nameValueCollection.Add("VOD_8", "");
      nameValueCollection.Add("VOD_9", "");
      nameValueCollection.Add("VOD_10", "");
      nameValueCollection.Add("VOD_11", "");
      nameValueCollection.Add("VOD_12", "");
      nameValueCollection.Add("VOD_13", "");
      nameValueCollection.Add("VOD_14", "");
      nameValueCollection.Add("VOD_15", "");
      nameValueCollection.Add("VOD_16", "");
      nameValueCollection.Add("VOD_17", "");
      nameValueCollection.Add("VOD_18", "");
      nameValueCollection.Add("VOD_19", "");
      nameValueCollection.Add("VOD_20", "");
      nameValueCollection.Add("VOD_21", "");
      nameValueCollection.Add("VOD_22", "");
      nameValueCollection.Add("VOD_23", "");
      nameValueCollection.Add("VOD_24", "");
      nameValueCollection.Add("VOD_25", "");
      nameValueCollection.Add("VOD_26", "");
      nameValueCollection.Add("VOD_27", "");
      nameValueCollection.Add("VOD_28", "");
      nameValueCollection.Add("VOD_29", "");
      nameValueCollection.Add("VOD_30", "");
      nameValueCollection.Add("VOD_31", "");
      nameValueCollection.Add("VOD_32", "");
      nameValueCollection.Add("VOD_33", "");
      nameValueCollection.Add("VOD_34", "");
      nameValueCollection.Add("VOD_35", "");
      nameValueCollection.Add("VOD_36", "");
      nameValueCollection.Add("VOD_37", "");
      nameValueCollection.Add("VOD_38", "");
      nameValueCollection.Add("VOD_39", "");
      nameValueCollection.Add("VOD_40", "");
      nameValueCollection.Add("VOD_41", "");
      nameValueCollection.Add("VOD_42", "");
      nameValueCollection.Add("VOD_43", "");
      nameValueCollection.Add("VOD_44", "");
      nameValueCollection.Add("VOD_45", "");
      nameValueCollection.Add("VOD_46", "");
      nameValueCollection.Add("VOD_47", "");
      nameValueCollection.Add("VOD_48", "");
      nameValueCollection.Add("VOD_49", "");
      nameValueCollection.Add("VOD_50", "");
      nameValueCollection.Add("VOD_51", "");
      nameValueCollection.Add("VOD_52", "");
      nameValueCollection.Add("VOD_53", "");
      nameValueCollection.Add("VOD_54", "");
      nameValueCollection.Add("VOD_55", "");
      nameValueCollection.Add("VOD_56", "");
      nameValueCollection.Add("VOD_57", "");
      nameValueCollection.Add("VOD_58", "");
      nameValueCollection.Add("VOD_59", "");
      nameValueCollection.Add("VOD_60", "");
      nameValueCollection.Add("VOL_1", "");
      nameValueCollection.Add("VOL_2", "");
      nameValueCollection.Add("VOL_3", "");
      nameValueCollection.Add("VOL_4", "");
      nameValueCollection.Add("VOL_5", "");
      nameValueCollection.Add("VOL_6", "");
      nameValueCollection.Add("VOL_7", "");
      nameValueCollection.Add("VOL_8", "");
      nameValueCollection.Add("VOL_9", "");
      nameValueCollection.Add("VOL_10", "");
      nameValueCollection.Add("VOL_11", "");
      nameValueCollection.Add("VOL_12", "");
      nameValueCollection.Add("VOL_13", "");
      nameValueCollection.Add("VOL_14", "");
      nameValueCollection.Add("VOL_15", "");
      nameValueCollection.Add("VOL_16", "");
      nameValueCollection.Add("VOL_17", "");
      nameValueCollection.Add("VOL_18", "");
      nameValueCollection.Add("VOL_19", "");
      nameValueCollection.Add("VOL_20", "");
      nameValueCollection.Add("VOL_21", "");
      nameValueCollection.Add("VOL_22", "");
      nameValueCollection.Add("VOL_23", "");
      nameValueCollection.Add("VOL_24", "");
      nameValueCollection.Add("VOL_25", "");
      nameValueCollection.Add("VOL_26", "");
      nameValueCollection.Add("VOL_27", "");
      nameValueCollection.Add("VOL_28", "");
      nameValueCollection.Add("VOL_29", "");
      nameValueCollection.Add("VOL_30", "");
      nameValueCollection.Add("VOL_31", "");
      nameValueCollection.Add("VOL_32", "");
      nameValueCollection.Add("VOL_33", "");
      nameValueCollection.Add("VOL_34", "");
      nameValueCollection.Add("VOL_35", "");
      nameValueCollection.Add("VOL_36", "");
      nameValueCollection.Add("VOL_37", "");
      nameValueCollection.Add("VOL_38", "");
      nameValueCollection.Add("VOL_39", "");
      nameValueCollection.Add("VOL_40", "");
      nameValueCollection.Add("VOL_41", "");
      nameValueCollection.Add("VOL_42", "");
      nameValueCollection.Add("VOL_43", "");
      nameValueCollection.Add("VOL_44", "");
      nameValueCollection.Add("VOL_45", "");
      nameValueCollection.Add("VOL_46", "");
      nameValueCollection.Add("VOL_47", "");
      nameValueCollection.Add("VOL_48", "");
      nameValueCollection.Add("VOL_49", "");
      nameValueCollection.Add("VOL_50", "");
      nameValueCollection.Add("VOL_51", "");
      nameValueCollection.Add("VOL_52", "");
      nameValueCollection.Add("VOL_53", "");
      nameValueCollection.Add("VOL_54", "");
      nameValueCollection.Add("VOL_55", "");
      nameValueCollection.Add("VOL_56", "");
      nameValueCollection.Add("VOL_57", "");
      nameValueCollection.Add("VOL_58", "");
      nameValueCollection.Add("VOL_59", "");
      nameValueCollection.Add("VOL_60", "");
      nameValueCollection.Add("VOL_61", "");
      nameValueCollection.Add("VOL_62", "");
      nameValueCollection.Add("VOL_63", "");
      nameValueCollection.Add("VOL_64", "");
      nameValueCollection.Add("VOL_65", "");
      nameValueCollection.Add("VOL_66", "");
      nameValueCollection.Add("VOL_67", "");
      nameValueCollection.Add("VOL_68", "");
      nameValueCollection.Add("VOL_69", "");
      nameValueCollection.Add("VOL_70", "");
      nameValueCollection.Add("VOL_71", "");
      nameValueCollection.Add("VOL_72", "");
      nameValueCollection.Add("VOL_73", "");
      nameValueCollection.Add("VOL_74", "");
      nameValueCollection.Add("VOL_75", "");
      nameValueCollection.Add("VOL_76", "");
      nameValueCollection.Add("VOL_77", "");
      nameValueCollection.Add("VOL_78", "");
      nameValueCollection.Add("VOL_79", "");
      nameValueCollection.Add("VOL_80", "");
      return nameValueCollection;
    }
  }
}
