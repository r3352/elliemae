// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.JS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScript;
using System.Text.RegularExpressions;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class JS
  {
    private static readonly Regex newlineTailRegex = new Regex("(\\r\\n)+$");

    public static string GetStr(LoanData loan, string dataFieldName)
    {
      string field = loan.GetField(dataFieldName);
      return JS.newlineTailRegex.Replace(field, "");
    }

    public static double GetNum(LoanData loan, string dataFieldName)
    {
      string simpleField = loan.GetSimpleField(dataFieldName);
      return Jed.S2N(JS.newlineTailRegex.Replace(simpleField, ""));
    }

    public static string Dummy() => "";
  }
}
