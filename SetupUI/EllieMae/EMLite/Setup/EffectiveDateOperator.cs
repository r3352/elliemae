// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EffectiveDateOperator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EffectiveDateOperator
  {
    public const string OPER_PLEASE_SELECT = "Please select";
    public const string OPER_BETWEEN = "Between";
    public const string OPER_BLANK = "Blank";
    public const string OPER_BLANK_ON_OR_AFTER = "Blank>=";
    public const string OPER_EQUALS = "=";
    public const string OPER_AFTER = ">";
    public const string OPER_ON_OR_AFTER = ">=";
    public const string OPER_LTE = "<=";
    public const string OPER_LT = "<";

    public string Operator { get; set; }

    public EffectiveDateOperator(string oper) => this.Operator = oper;

    public override string ToString()
    {
      switch (this.Operator)
      {
        case "<":
          return "< Less than";
        case "<=":
          return "<= Less than Equal to";
        case "=":
          return "= Equals";
        case ">":
          return "> After";
        case ">=":
          return ">= On Or After";
        case "Between":
          return "Between";
        case "Blank":
          return "Blank";
        case "Blank>=":
          return "Blank / On or After";
        case "Please select":
          return "Please select";
        default:
          return "n/a";
      }
    }

    public static string GetDisplayString(string oper)
    {
      switch (oper)
      {
        case "<":
        case "<=":
        case "=":
        case ">":
        case ">=":
          return oper;
        case "Between":
          return "Between";
        case "Blank":
          return "Blank";
        case "Blank>=":
          return "Blank / On or After";
        case "Please select":
          return "Please select";
        default:
          return "n/a";
      }
    }
  }
}
