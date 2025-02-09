// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003_PG5_PREVIOUS_ADDRESS__LETTER_CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _1003_PG5_PREVIOUS_ADDRESS__LETTER_CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("305", JS.GetStr(loan, "305"));
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("VOR_97", JS.GetStr(loan, "VOR_97"));
      nameValueCollection.Add("VOR_96", JS.GetStr(loan, "VOR_96"));
      nameValueCollection.Add("VOR_95", JS.GetStr(loan, "VOR_95"));
      nameValueCollection.Add("VOR_94", JS.GetStr(loan, "VOR_94"));
      nameValueCollection.Add("VOR_9", JS.GetStr(loan, "VOR_9"));
      nameValueCollection.Add("VOR_8", JS.GetStr(loan, "VOR_8"));
      nameValueCollection.Add("VOR_7", JS.GetStr(loan, "VOR_7"));
      nameValueCollection.Add("VOR_6", JS.GetStr(loan, "VOR_6"));
      nameValueCollection.Add("VOR_5", JS.GetStr(loan, "VOR_5"));
      nameValueCollection.Add("VOR_4", JS.GetStr(loan, "VOR_4"));
      nameValueCollection.Add("VOR_3", JS.GetStr(loan, "VOR_3"));
      nameValueCollection.Add("VOR_2", JS.GetStr(loan, "VOR_2"));
      nameValueCollection.Add("VOR_1", JS.GetStr(loan, "VOR_1"));
      nameValueCollection.Add("VOR_29", JS.GetStr(loan, "VOR_29"));
      nameValueCollection.Add("VOR_28", JS.GetStr(loan, "VOR_28"));
      nameValueCollection.Add("VOR_27", JS.GetStr(loan, "VOR_27"));
      nameValueCollection.Add("VOR_26", JS.GetStr(loan, "VOR_26"));
      nameValueCollection.Add("VOR_25", JS.GetStr(loan, "VOR_25"));
      nameValueCollection.Add("VOR_24", JS.GetStr(loan, "VOR_24"));
      nameValueCollection.Add("VOR_23", JS.GetStr(loan, "VOR_23"));
      nameValueCollection.Add("VOR_22", JS.GetStr(loan, "VOR_22"));
      nameValueCollection.Add("VOR_21", JS.GetStr(loan, "VOR_21"));
      nameValueCollection.Add("VOR_20", JS.GetStr(loan, "VOR_20"));
      nameValueCollection.Add("VOR_88", JS.GetStr(loan, "VOR_88"));
      nameValueCollection.Add("VOR_87", JS.GetStr(loan, "VOR_87"));
      nameValueCollection.Add("VOR_86", JS.GetStr(loan, "VOR_86"));
      nameValueCollection.Add("VOR_85", JS.GetStr(loan, "VOR_85"));
      nameValueCollection.Add("VOR_84", JS.GetStr(loan, "VOR_84"));
      nameValueCollection.Add("VOR_83", JS.GetStr(loan, "VOR_83"));
      nameValueCollection.Add("VOR_82", JS.GetStr(loan, "VOR_82"));
      nameValueCollection.Add("VOR_81", JS.GetStr(loan, "VOR_81"));
      nameValueCollection.Add("VOR_80", JS.GetStr(loan, "VOR_80"));
      nameValueCollection.Add("VOR_19", JS.GetStr(loan, "VOR_19"));
      nameValueCollection.Add("VOR_18", JS.GetStr(loan, "VOR_18"));
      nameValueCollection.Add("VOR_17", JS.GetStr(loan, "VOR_17"));
      nameValueCollection.Add("VOR_16", JS.GetStr(loan, "VOR_16"));
      nameValueCollection.Add("VOR_15", JS.GetStr(loan, "VOR_15"));
      nameValueCollection.Add("VOR_14", JS.GetStr(loan, "VOR_14"));
      nameValueCollection.Add("VOR_13", JS.GetStr(loan, "VOR_13"));
      nameValueCollection.Add("VOR_12", JS.GetStr(loan, "VOR_12"));
      nameValueCollection.Add("VOR_11", JS.GetStr(loan, "VOR_11"));
      nameValueCollection.Add("VOR_10", JS.GetStr(loan, "VOR_10"));
      nameValueCollection.Add("VOR_79", JS.GetStr(loan, "VOR_79"));
      nameValueCollection.Add("VOR_78", JS.GetStr(loan, "VOR_78"));
      nameValueCollection.Add("VOR_77", JS.GetStr(loan, "VOR_77"));
      nameValueCollection.Add("VOR_76", JS.GetStr(loan, "VOR_76"));
      nameValueCollection.Add("VOR_75", JS.GetStr(loan, "VOR_75"));
      nameValueCollection.Add("VOR_74", JS.GetStr(loan, "VOR_74"));
      nameValueCollection.Add("VOR_73", JS.GetStr(loan, "VOR_73"));
      nameValueCollection.Add("VOR_72", JS.GetStr(loan, "VOR_72"));
      nameValueCollection.Add("VOR_71", JS.GetStr(loan, "VOR_71"));
      nameValueCollection.Add("VOR_70", JS.GetStr(loan, "VOR_70"));
      nameValueCollection.Add("VOR_118", JS.GetStr(loan, "VOR_118"));
      nameValueCollection.Add("VOR_110", JS.GetStr(loan, "VOR_110"));
      nameValueCollection.Add("VOR_69", JS.GetStr(loan, "VOR_69"));
      nameValueCollection.Add("VOR_68", JS.GetStr(loan, "VOR_68"));
      nameValueCollection.Add("VOR_67", JS.GetStr(loan, "VOR_67"));
      nameValueCollection.Add("VOR_66", JS.GetStr(loan, "VOR_66"));
      nameValueCollection.Add("VOR_65", JS.GetStr(loan, "VOR_65"));
      nameValueCollection.Add("VOR_64", JS.GetStr(loan, "VOR_64"));
      nameValueCollection.Add("VOR_63", JS.GetStr(loan, "VOR_63"));
      nameValueCollection.Add("VOR_62", JS.GetStr(loan, "VOR_62"));
      nameValueCollection.Add("VOR_61", JS.GetStr(loan, "VOR_61"));
      nameValueCollection.Add("VOR_60", JS.GetStr(loan, "VOR_60"));
      nameValueCollection.Add("VOR_109", JS.GetStr(loan, "VOR_109"));
      nameValueCollection.Add("VOR_108", JS.GetStr(loan, "VOR_108"));
      nameValueCollection.Add("VOR_100", JS.GetStr(loan, "VOR_100"));
      nameValueCollection.Add("VOR_59", JS.GetStr(loan, "VOR_59"));
      nameValueCollection.Add("VOR_58", JS.GetStr(loan, "VOR_58"));
      nameValueCollection.Add("VOR_57", JS.GetStr(loan, "VOR_57"));
      nameValueCollection.Add("VOR_56", JS.GetStr(loan, "VOR_56"));
      nameValueCollection.Add("VOR_55", JS.GetStr(loan, "VOR_55"));
      nameValueCollection.Add("VOR_54", JS.GetStr(loan, "VOR_54"));
      nameValueCollection.Add("VOR_53", JS.GetStr(loan, "VOR_53"));
      nameValueCollection.Add("VOR_52", JS.GetStr(loan, "VOR_52"));
      nameValueCollection.Add("VOR_51", JS.GetStr(loan, "VOR_51"));
      nameValueCollection.Add("VOR_50", JS.GetStr(loan, "VOR_50"));
      nameValueCollection.Add("VOR_120", JS.GetStr(loan, "VOR_120"));
      nameValueCollection.Add("VOR_93", JS.GetStr(loan, "VOR_93"));
      nameValueCollection.Add("VOR_92", JS.GetStr(loan, "VOR_92"));
      nameValueCollection.Add("VOR_91", JS.GetStr(loan, "VOR_91"));
      nameValueCollection.Add("VOR_90", JS.GetStr(loan, "VOR_90"));
      nameValueCollection.Add("VOR_49", JS.GetStr(loan, "VOR_49"));
      nameValueCollection.Add("VOR_48", JS.GetStr(loan, "VOR_48"));
      nameValueCollection.Add("VOR_47", JS.GetStr(loan, "VOR_47"));
      nameValueCollection.Add("VOR_46", JS.GetStr(loan, "VOR_46"));
      nameValueCollection.Add("VOR_45", JS.GetStr(loan, "VOR_45"));
      nameValueCollection.Add("VOR_44", JS.GetStr(loan, "VOR_44"));
      nameValueCollection.Add("VOR_43", JS.GetStr(loan, "VOR_43"));
      nameValueCollection.Add("VOR_42", JS.GetStr(loan, "VOR_42"));
      nameValueCollection.Add("VOR_41", JS.GetStr(loan, "VOR_41"));
      nameValueCollection.Add("VOR_40", JS.GetStr(loan, "VOR_40"));
      nameValueCollection.Add("VOR_117", JS.GetStr(loan, "VOR_117"));
      nameValueCollection.Add("VOR_116", JS.GetStr(loan, "VOR_116"));
      nameValueCollection.Add("VOR_115", JS.GetStr(loan, "VOR_115"));
      nameValueCollection.Add("VOR_114", JS.GetStr(loan, "VOR_114"));
      nameValueCollection.Add("VOR_113", JS.GetStr(loan, "VOR_113"));
      nameValueCollection.Add("VOR_112", JS.GetStr(loan, "VOR_112"));
      nameValueCollection.Add("VOR_111", JS.GetStr(loan, "VOR_111"));
      nameValueCollection.Add("VOR_89", JS.GetStr(loan, "VOR_89"));
      nameValueCollection.Add("VOR_119", JS.GetStr(loan, "VOR_119"));
      nameValueCollection.Add("VOR_39", JS.GetStr(loan, "VOR_39"));
      nameValueCollection.Add("VOR_38", JS.GetStr(loan, "VOR_38"));
      nameValueCollection.Add("VOR_37", JS.GetStr(loan, "VOR_37"));
      nameValueCollection.Add("VOR_36", JS.GetStr(loan, "VOR_36"));
      nameValueCollection.Add("VOR_35", JS.GetStr(loan, "VOR_35"));
      nameValueCollection.Add("VOR_34", JS.GetStr(loan, "VOR_34"));
      nameValueCollection.Add("VOR_33", JS.GetStr(loan, "VOR_33"));
      nameValueCollection.Add("VOR_32", JS.GetStr(loan, "VOR_32"));
      nameValueCollection.Add("VOR_31", JS.GetStr(loan, "VOR_31"));
      nameValueCollection.Add("VOR_30", JS.GetStr(loan, "VOR_30"));
      nameValueCollection.Add("VOR_107", JS.GetStr(loan, "VOR_107"));
      nameValueCollection.Add("VOR_106", JS.GetStr(loan, "VOR_106"));
      nameValueCollection.Add("VOR_105", JS.GetStr(loan, "VOR_105"));
      nameValueCollection.Add("VOR_104", JS.GetStr(loan, "VOR_104"));
      nameValueCollection.Add("VOR_103", JS.GetStr(loan, "VOR_103"));
      nameValueCollection.Add("VOR_102", JS.GetStr(loan, "VOR_102"));
      nameValueCollection.Add("VOR_101", JS.GetStr(loan, "VOR_101"));
      nameValueCollection.Add("VOR_99", JS.GetStr(loan, "VOR_99"));
      nameValueCollection.Add("VOR_98", JS.GetStr(loan, "VOR_98"));
      nameValueCollection.Add("VOR_121", JS.GetStr(loan, "VOR_121"));
      nameValueCollection.Add("VOR_122", JS.GetStr(loan, "VOR_122"));
      nameValueCollection.Add("VOR_123", JS.GetStr(loan, "VOR_123"));
      nameValueCollection.Add("VOR_124", JS.GetStr(loan, "VOR_124"));
      nameValueCollection.Add("VOR_125", JS.GetStr(loan, "VOR_125"));
      nameValueCollection.Add("VOR_126", JS.GetStr(loan, "VOR_126"));
      nameValueCollection.Add("VOR_127", JS.GetStr(loan, "VOR_127"));
      nameValueCollection.Add("VOR_128", JS.GetStr(loan, "VOR_128"));
      nameValueCollection.Add("VOR_129", JS.GetStr(loan, "VOR_129"));
      nameValueCollection.Add("VOR_130", JS.GetStr(loan, "VOR_130"));
      return nameValueCollection;
    }
  }
}
