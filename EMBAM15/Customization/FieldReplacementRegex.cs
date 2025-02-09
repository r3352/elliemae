// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.FieldReplacementRegex
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class FieldReplacementRegex
  {
    internal static readonly Regex fieldRegex = new Regex("\\[(?<field>(?<prefix>#|\\+|-|@)?(?<id>[^\\]]+?))\\]");

    public static string Replace(string sourceCode)
    {
      return FieldReplacementRegex.fieldRegex.Replace(sourceCode, "Fields(\"${field}\")");
    }

    public static string ReplaceLiteral(string text, FieldMatchEvaluator evaluator)
    {
      return FieldReplacementRegex.fieldRegex.Replace(text, new MatchEvaluator(new FieldReplacementRegex.FieldMatchEvaluatorProxy(evaluator).ReplaceField));
    }

    public static string ReplaceLiteral(string text, IFieldSource fieldSource)
    {
      return FieldReplacementRegex.ReplaceLiteral(text, new FieldMatchEvaluator(new LoanFieldMatchEvaluator(fieldSource).ReplaceField));
    }

    public static string ReplaceLiteral(string text, LoanData loanData)
    {
      return FieldReplacementRegex.ReplaceLiteral(text, (IFieldSource) new LoanDataFieldSource(loanData, (UserInfo) null, true));
    }

    public static string[] ParseDependentFields(string sourceCode)
    {
      Match match = FieldReplacementRegex.fieldRegex.Match(sourceCode);
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (; match.Success; match = match.NextMatch())
      {
        string key = match.Groups["id"].Value;
        if (!dictionary.ContainsKey(key))
          dictionary[key] = key;
      }
      string[] array = new string[dictionary.Count];
      if (dictionary.Count > 0)
        dictionary.Keys.CopyTo(array, 0);
      return array;
    }

    private class FieldMatchEvaluatorProxy
    {
      private FieldMatchEvaluator eval;

      public FieldMatchEvaluatorProxy(FieldMatchEvaluator eval) => this.eval = eval;

      public string ReplaceField(Match m) => this.eval(new FieldMatch(m));
    }
  }
}
