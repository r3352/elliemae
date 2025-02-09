// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG6_SPANISHCLASS
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
  public class _1003PG6_SPANISHCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1109", JS.GetStr(loan, "1109"));
      nameValueCollection.Add("1045", JS.GetStr(loan, "1045"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("142", JS.GetStr(loan, "142"));
      nameValueCollection.Add("1107", JS.GetStr(loan, "1107"));
      nameValueCollection.Add("171_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "171"), "Y", false) == 0, "X"));
      nameValueCollection.Add("171_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "171"), "N", false) == 0, "X"));
      nameValueCollection.Add("177_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "177"), "Y", false) == 0, "X"));
      nameValueCollection.Add("177_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "177"), "N", false) == 0, "X"));
      nameValueCollection.Add("965_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "Y", false) == 0, "X"));
      nameValueCollection.Add("965_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "N", false) == 0, "X"));
      nameValueCollection.Add("985_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "Y", false) == 0, "X"));
      nameValueCollection.Add("985_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "N", false) == 0, "X"));
      nameValueCollection.Add("466_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "Y", false) == 0, "X"));
      nameValueCollection.Add("466_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "N", false) == 0, "X"));
      nameValueCollection.Add("467_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "Y", false) == 0, "X"));
      nameValueCollection.Add("467_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "N", false) == 0, "X"));
      nameValueCollection.Add("418_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "418"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("418_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "418"), "No", false) == 0, "X"));
      nameValueCollection.Add("1343_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1343"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("1343_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1343"), "No", false) == 0, "X"));
      nameValueCollection.Add("403_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "403"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("403_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "403"), "No", false) == 0, "X"));
      nameValueCollection.Add("1108_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1108"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("1108_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1108"), "No", false) == 0, "X"));
      nameValueCollection.Add("981", Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "PrimaryResidence", false) == 0, "PR") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "Investment", false) == 0, "IP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "SecondaryResidence", false) == 0, "SH"));
      nameValueCollection.Add("1015", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "PrimaryResidence", false) == 0, "PR") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "Investment", false) == 0, "IP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "SecondaryResidence", false) == 0, "SH"));
      nameValueCollection.Add("1069", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "Sole", false) == 0, "S") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "JointWithSpouse", false) == 0, "SP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "JointWithOtherThanSpouse", false) == 0, "O"));
      nameValueCollection.Add("1070", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "Sole", false) == 0, "S") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "JointWithSpouse", false) == 0, "SP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "JointWithOtherThanSpouse", false) == 0, "O"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      return nameValueCollection;
    }
  }
}
