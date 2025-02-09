// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._UNDERWRITERSUMMARYP3CLASS
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
  public class _UNDERWRITERSUMMARYP3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("2324", JS.GetStr(loan, "2324"));
      nameValueCollection.Add("2325", JS.GetStr(loan, "2325"));
      nameValueCollection.Add("2326", JS.GetStr(loan, "2326"));
      nameValueCollection.Add("2327", JS.GetStr(loan, "2327"));
      nameValueCollection.Add("2328", JS.GetStr(loan, "2328"));
      nameValueCollection.Add("2329", JS.GetStr(loan, "2329"));
      nameValueCollection.Add("2330", JS.GetStr(loan, "2330"));
      nameValueCollection.Add("2331", JS.GetStr(loan, "2331"));
      nameValueCollection.Add("2332", JS.GetStr(loan, "2332"));
      nameValueCollection.Add("2333", JS.GetStr(loan, "2333"));
      nameValueCollection.Add("2334", JS.GetStr(loan, "2334"));
      nameValueCollection.Add("2335", JS.GetStr(loan, "2335"));
      nameValueCollection.Add("2555", JS.GetStr(loan, "2555"));
      nameValueCollection.Add("2556", JS.GetStr(loan, "2556"));
      nameValueCollection.Add("2557", JS.GetStr(loan, "2557"));
      nameValueCollection.Add("2562", JS.GetStr(loan, "2562"));
      nameValueCollection.Add("2558", JS.GetStr(loan, "2558"));
      nameValueCollection.Add("2559", JS.GetStr(loan, "2559"));
      nameValueCollection.Add("2560", JS.GetStr(loan, "2560"));
      nameValueCollection.Add("2561", JS.GetStr(loan, "2561"));
      nameValueCollection.Add("2563_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2563"), "Y", false) == 0, "X"));
      nameValueCollection.Add("2563_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2563"), "Y", false) != 0, "X"));
      nameValueCollection.Add("2566_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2566"), "Y", false) == 0, "X"));
      nameValueCollection.Add("2566_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2566"), "Y", false) != 0, "X"));
      nameValueCollection.Add("2564", JS.GetStr(loan, "2564"));
      nameValueCollection.Add("2565", JS.GetStr(loan, "2565"));
      nameValueCollection.Add("2567", JS.GetStr(loan, "2567"));
      nameValueCollection.Add("2336", JS.GetStr(loan, "2336"));
      nameValueCollection.Add("2568", JS.GetStr(loan, "2568"));
      nameValueCollection.Add("2340", JS.GetStr(loan, "2340"));
      nameValueCollection.Add("2569", JS.GetStr(loan, "2569"));
      nameValueCollection.Add("2570", JS.GetStr(loan, "2570"));
      nameValueCollection.Add("2339_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2339"), "Y", false) == 0, "X"));
      nameValueCollection.Add("2339_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2339"), "Y", false) != 0, "X"));
      nameValueCollection.Add("2341", JS.GetStr(loan, "2341"));
      nameValueCollection.Add("2571", JS.GetStr(loan, "2571"));
      nameValueCollection.Add("2572", JS.GetStr(loan, "2572"));
      nameValueCollection.Add("2342", JS.GetStr(loan, "2342"));
      nameValueCollection.Add("2343", JS.GetStr(loan, "2343"));
      nameValueCollection.Add("2344", JS.GetStr(loan, "2344"));
      nameValueCollection.Add("2345", JS.GetStr(loan, "2345"));
      nameValueCollection.Add("2346", JS.GetStr(loan, "2346"));
      nameValueCollection.Add("2347", JS.GetStr(loan, "2347"));
      nameValueCollection.Add("2348", JS.GetStr(loan, "2348"));
      nameValueCollection.Add("2349", JS.GetStr(loan, "2349"));
      nameValueCollection.Add("2350", JS.GetStr(loan, "2350"));
      nameValueCollection.Add("2356", JS.GetStr(loan, "2356"));
      nameValueCollection.Add("2351", JS.GetStr(loan, "2351"));
      nameValueCollection.Add("2352", JS.GetStr(loan, "2352"));
      nameValueCollection.Add("2355", JS.GetStr(loan, "2355"));
      nameValueCollection.Add("2353", JS.GetStr(loan, "2353"));
      nameValueCollection.Add("2354", JS.GetStr(loan, "2354"));
      nameValueCollection.Add("2357", JS.GetStr(loan, "2357"));
      nameValueCollection.Add("2361", JS.GetStr(loan, "2361"));
      nameValueCollection.Add("2358", JS.GetStr(loan, "2358"));
      nameValueCollection.Add("2359", JS.GetStr(loan, "2359"));
      nameValueCollection.Add("2360", JS.GetStr(loan, "2360"));
      nameValueCollection.Add("1500", JS.GetStr(loan, "1500"));
      nameValueCollection.Add("2363", JS.GetStr(loan, "2363"));
      nameValueCollection.Add("2364", JS.GetStr(loan, "2364"));
      nameValueCollection.Add("2365", JS.GetStr(loan, "2365"));
      nameValueCollection.Add("2366_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2366"), "Y", false) == 0, "X"));
      nameValueCollection.Add("2366_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "2366"), "Y", false) != 0, "X"));
      nameValueCollection.Add("2367", JS.GetStr(loan, "2367"));
      nameValueCollection.Add("2368", JS.GetStr(loan, "2368"));
      nameValueCollection.Add("18", JS.GetStr(loan, "18"));
      nameValueCollection.Add("16", JS.GetStr(loan, "16"));
      nameValueCollection.Add("2369", JS.GetStr(loan, "2369"));
      return nameValueCollection;
    }
  }
}
