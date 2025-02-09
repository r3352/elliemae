// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._PRIVACYPOLICYP1CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _PRIVACYPOLICYP1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("NOTICES_X52", JS.GetStr(loan, "NOTICES.X52"));
      nameValueCollection.Add("NOTICES_X53", JS.GetStr(loan, "NOTICES.X53"));
      nameValueCollection.Add("NOTICES_X54", JS.GetStr(loan, "NOTICES.X54"));
      nameValueCollection.Add("NOTICES_X55", JS.GetStr(loan, "NOTICES.X55"));
      nameValueCollection.Add("NOTICES_X56", JS.GetStr(loan, "NOTICES.X56"));
      nameValueCollection.Add("NOTICES_X57", JS.GetStr(loan, "NOTICES.X57"));
      nameValueCollection.Add("NOTICES_X57a", JS.GetStr(loan, "NOTICES.X57"));
      nameValueCollection.Add("315a", JS.GetStr(loan, "315"));
      nameValueCollection.Add("315b", JS.GetStr(loan, "315"));
      nameValueCollection.Add("NOTICES_X58", JS.GetStr(loan, "NOTICES.X58"));
      nameValueCollection.Add("NOTICES_X59", JS.GetStr(loan, "NOTICES.X59"));
      nameValueCollection.Add("NOTICES_X60", JS.GetStr(loan, "NOTICES.X60"));
      nameValueCollection.Add("NOTICES_X61", JS.GetStr(loan, "NOTICES.X61"));
      nameValueCollection.Add("NOTICES_X62", JS.GetStr(loan, "NOTICES.X62"));
      nameValueCollection.Add("NOTICES_X63", JS.GetStr(loan, "NOTICES.X63"));
      nameValueCollection.Add("NOTICES_X64", JS.GetStr(loan, "NOTICES.X64"));
      nameValueCollection.Add("NOTICES_X65", JS.GetStr(loan, "NOTICES.X65"));
      nameValueCollection.Add("NOTICES_X66", JS.GetStr(loan, "NOTICES.X66"));
      nameValueCollection.Add("NOTICES_X67", JS.GetStr(loan, "NOTICES.X67"));
      nameValueCollection.Add("NOTICES_X68", JS.GetStr(loan, "NOTICES.X68"));
      nameValueCollection.Add("NOTICES_X69", JS.GetStr(loan, "NOTICES.X69"));
      nameValueCollection.Add("NOTICES_X70", JS.GetStr(loan, "NOTICES.X70"));
      nameValueCollection.Add("NOTICES_X71", JS.GetStr(loan, "NOTICES.X71"));
      nameValueCollection.Add("NOTICES_X89", JS.GetStr(loan, "NOTICES.X89"));
      nameValueCollection.Add("NOTICES_X90", JS.GetStr(loan, "NOTICES.X90"));
      nameValueCollection.Add("NOTICES_X91", JS.GetStr(loan, "NOTICES.X91"));
      nameValueCollection.Add("NOTICES_X92", JS.GetStr(loan, "NOTICES.X92"));
      nameValueCollection.Add("NOTICES_X93", JS.GetStr(loan, "NOTICES.X93"));
      return nameValueCollection;
    }
  }
}
