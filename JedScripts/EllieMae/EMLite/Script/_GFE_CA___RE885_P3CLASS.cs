// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFE_CA___RE885_P3CLASS
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
  public class _GFE_CA___RE885_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("RE88395X155_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X128"), "", false) != 0, "", "X"));
      nameValueCollection.Add("RE88395X155_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X128"), "", false) != 0, "X"));
      nameValueCollection.Add("RE88395X128", JS.GetStr(loan, "RE88395.X128"));
      nameValueCollection.Add("RE88395X131", JS.GetStr(loan, "RE88395.X131"));
      nameValueCollection.Add("RE88395X134", JS.GetStr(loan, "RE88395.X134"));
      nameValueCollection.Add("RE88395X129", JS.GetStr(loan, "RE88395.X129"));
      nameValueCollection.Add("RE88395X132", JS.GetStr(loan, "RE88395.X132"));
      nameValueCollection.Add("RE88395X135", JS.GetStr(loan, "RE88395.X135"));
      nameValueCollection.Add("RE88395X130", JS.GetStr(loan, "RE88395.X130"));
      nameValueCollection.Add("RE88395X133", JS.GetStr(loan, "RE88395.X133"));
      nameValueCollection.Add("RE88395X136", JS.GetStr(loan, "RE88395.X136"));
      nameValueCollection.Add("RE88395X137", JS.GetStr(loan, "RE88395.X137"));
      nameValueCollection.Add("RE88395X140", JS.GetStr(loan, "RE88395.X140"));
      nameValueCollection.Add("RE88395X143", JS.GetStr(loan, "RE88395.X143"));
      nameValueCollection.Add("RE88395X138", JS.GetStr(loan, "RE88395.X138"));
      nameValueCollection.Add("RE88395X141", JS.GetStr(loan, "RE88395.X141"));
      nameValueCollection.Add("RE88395X144", JS.GetStr(loan, "RE88395.X144"));
      nameValueCollection.Add("RE88395X139", JS.GetStr(loan, "RE88395.X139"));
      nameValueCollection.Add("RE88395X142", JS.GetStr(loan, "RE88395.X142"));
      nameValueCollection.Add("RE88395X145", JS.GetStr(loan, "RE88395.X145"));
      nameValueCollection.Add("RE88395X149_May", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "May", false) == 0, "X"));
      nameValueCollection.Add("RE88395X149_Will", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "Will", false) == 0, "X"));
      nameValueCollection.Add("RE88395X149_Willnot", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "WillNot", false) == 0, "X"));
      nameValueCollection.Add("MORNETX67_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "FullDocumentation", false) == 0, "X"));
      nameValueCollection.Add("MORNETX67_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "FullDocumentation", false) != 0, "X"));
      return nameValueCollection;
    }
  }
}
