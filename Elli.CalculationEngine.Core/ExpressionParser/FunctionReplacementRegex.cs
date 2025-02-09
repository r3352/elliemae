// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.FunctionReplacementRegex
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  public class FunctionReplacementRegex
  {
    internal static readonly Regex functionRegex = new Regex("(?i)Function\\s+(?<name>\\w+)");
    internal static readonly Regex functionParameterRegex = new Regex("[(,\\s+](?<param>\\w+)\\s+(?i)As\\s+(?<type>\\w+)[,)]");
    internal static readonly Regex returnTypeRegex = new Regex("[)]\\s+(?i)As\\s+(?<returnType>\\w+)");

    public static string FunctionName(string sourceCode)
    {
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FunctionReplacementRegex.functionRegex.Match(sourceCode);
      string empty = string.Empty;
      if (match.Success)
        empty = match.Groups["name"].Value;
      return empty;
    }

    public static List<FunctionParameter> ParseFunctionParameters(string sourceCode)
    {
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FunctionReplacementRegex.functionParameterRegex.Match(sourceCode);
      List<FunctionParameter> functionParameters = new List<FunctionParameter>();
      TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
      for (; match.Success; match = match.NextMatch())
      {
        string param = match.Groups["param"].Value;
        string str = match.Groups["type"].Value;
        if (functionParameters.Find((Predicate<FunctionParameter>) (par => par.Name == param)) == null)
          functionParameters.Add(new FunctionParameter(param, textInfo.ToTitleCase(str.ToLower())));
      }
      return functionParameters;
    }

    public static string ReturnType(string sourceCode)
    {
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FunctionReplacementRegex.returnTypeRegex.Match(sourceCode);
      string empty = string.Empty;
      if (match.Success)
        empty = match.Groups["returnType"].Value;
      return empty;
    }
  }
}
