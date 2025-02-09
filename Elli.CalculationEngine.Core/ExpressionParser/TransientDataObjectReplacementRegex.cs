// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.TransientDataObjectReplacementRegex
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  public class TransientDataObjectReplacementRegex
  {
    internal static readonly Regex transientDataObjectRegex = new Regex("Public Class\\s+(?<name>\\w+)");

    public static string TransientDataObjectName(string sourceCode)
    {
      sourceCode = sourceCode == null ? string.Empty : sourceCode;
      Match match = TransientDataObjectReplacementRegex.transientDataObjectRegex.Match(sourceCode);
      string empty = string.Empty;
      if (match.Success)
        empty = match.Groups["name"].Value;
      return empty;
    }
  }
}
