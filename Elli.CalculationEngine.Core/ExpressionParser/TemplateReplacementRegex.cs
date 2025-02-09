// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.TemplateReplacementRegex
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  public class TemplateReplacementRegex
  {
    internal static readonly Regex fieldRegex = new Regex("\\[(?<field>(?<prefix>#|\\+|-|@)?(?<id>[^\\]]+?))\\]");
    internal static readonly Regex templateSubstituitionRegex = new Regex("\\[\\$(?<param>[^\\]\\=]+?)\\]\\=(?<value>(?<bracket>\\[)?.*)");

    public static string GetTemplateName(string template)
    {
      int length = template.IndexOf("(");
      string templateName = string.Empty;
      if (length > -1)
        templateName = template.Substring(0, length);
      return templateName;
    }

    public static string Replace(string templateSubtitutionDefinition, string sourceCode)
    {
      foreach (TemplateSubstitution templateSubstitution in TemplateReplacementRegex.ParseTemplateSubstitutions(templateSubtitutionDefinition))
        sourceCode = !templateSubstitution.IsFieldName ? sourceCode.Replace(string.Format("[${0}]", (object) templateSubstitution.ParameterName), templateSubstitution.Value) : sourceCode.Replace(string.Format("${0}", (object) templateSubstitution.ParameterName), templateSubstitution.Value);
      return sourceCode;
    }

    public static string Verify(
      string templateName,
      string templateSubstitutionDefinition,
      string sourceCode)
    {
      string empty = string.Empty;
      List<string> templateParameters = TemplateReplacementRegex.ParseTemplateParameters(sourceCode);
      List<TemplateSubstitution> templateSubstitutions = TemplateReplacementRegex.ParseTemplateSubstitutions(templateSubstitutionDefinition);
      foreach (string str in templateParameters)
      {
        bool flag = false;
        foreach (TemplateSubstitution templateSubstitution in templateSubstitutions)
        {
          if (str == string.Format("${0}", (object) templateSubstitution.ParameterName))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          empty += string.Format("{0} does not exist in {1}.\r\n", (object) str, (object) templateSubstitutionDefinition);
      }
      foreach (TemplateSubstitution templateSubstitution in templateSubstitutions)
      {
        bool flag = false;
        foreach (string str in templateParameters)
        {
          if (str == string.Format("${0}", (object) templateSubstitution.ParameterName))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          empty += string.Format("{0} does not exist in {1}.\r\n", (object) string.Format("${0}", (object) templateSubstitution.ParameterName), (object) sourceCode);
      }
      if (templateSubstitutionDefinition.Count<char>((Func<char, bool>) (left => left == '(')) != templateSubstitutionDefinition.Count<char>((Func<char, bool>) (right => right == ')')))
        empty += string.Format("{0} is not formatted correctly.\r\n", (object) templateSubstitutionDefinition);
      char[] chArray = new char[1]{ ',' };
      List<string> stringList = new List<string>((IEnumerable<string>) templateSubstitutionDefinition.Trim().Substring(0, templateSubstitutionDefinition.Length - 1).Replace(templateName + "(", "").Split(chArray));
      stringList.Remove(templateName);
      if (stringList[stringList.Count - 1] == string.Empty)
        stringList.RemoveAt(stringList.Count - 1);
      foreach (string source1 in stringList)
      {
        if (!source1.Trim().Contains("="))
          empty += source1.Trim() == string.Empty ? string.Format("{0} is not formatted correctly.\r\n", (object) templateSubstitutionDefinition) : string.Format("{0} is not formatted correctly in {1}.\r\n", (object) source1, (object) templateSubstitutionDefinition);
        else if (source1.Count<char>((Func<char, bool>) (right => right == ']')) != source1.Count<char>((Func<char, bool>) (left => left == '[')) || source1.Count<char>((Func<char, bool>) (right => right == ')')) != source1.Count<char>((Func<char, bool>) (left => left == '(')) || source1.Count<char>((Func<char, bool>) (right => right == '}')) != source1.Count<char>((Func<char, bool>) (left => left == '{')) || source1.Count<char>((Func<char, bool>) (equal => equal == '=')) > 1 || source1.Count<char>((Func<char, bool>) (quote => quote == '"')) % 2 != 0)
        {
          empty += string.Format("{0} is not formatted correctly in {1}.\r\n", (object) source1, (object) templateSubstitutionDefinition);
        }
        else
        {
          char[] separator = new char[1]{ '=' };
          string[] strArray = source1.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length != 2)
          {
            empty += string.Format("{0} is not formatted correctly in {1}.\r\n", (object) source1, (object) templateSubstitutionDefinition);
          }
          else
          {
            foreach (string source2 in strArray)
            {
              if (source2.Count<char>((Func<char, bool>) (right => right == ']')) != source2.Count<char>((Func<char, bool>) (left => left == '[')) || source2.Count<char>((Func<char, bool>) (right => right == ')')) != source2.Count<char>((Func<char, bool>) (left => left == '(')) || source2.Count<char>((Func<char, bool>) (right => right == '}')) != source2.Count<char>((Func<char, bool>) (left => left == '{')) || source2.Count<char>((Func<char, bool>) (quote => quote == '"')) % 2 != 0)
                empty += string.Format("{0} is not formatted correctly in {1}.\r\n", (object) source1, (object) templateSubstitutionDefinition);
            }
          }
        }
      }
      return empty;
    }

    public static List<string> ParseTemplateParameters(string sourceCode)
    {
      sourceCode = sourceCode == null ? string.Empty : sourceCode;
      Match match = TemplateReplacementRegex.fieldRegex.Match(sourceCode);
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (; match.Success; match = match.NextMatch())
      {
        string key = match.Groups["id"].Value;
        if (!dictionary.ContainsKey(key) && key.StartsWith("$"))
          dictionary[key] = key;
      }
      string[] strArray = new string[dictionary.Count];
      if (dictionary.Count > 0)
        dictionary.Keys.CopyTo(strArray, 0);
      return new List<string>((IEnumerable<string>) strArray);
    }

    public static List<string> ParseTemplates(string sourceCode)
    {
      sourceCode = sourceCode == null ? string.Empty : sourceCode;
      List<string> templates = new List<string>();
      string str = sourceCode;
      int startIndex = str.IndexOf("Template");
      int num1 = -1;
      if (startIndex != -1)
        num1 = str.Substring(startIndex).IndexOf(")") + startIndex;
      while (startIndex != -1 && num1 != -1)
      {
        string source = str.Substring(startIndex, num1 - startIndex + 1);
        int num2 = source.Count<char>((Func<char, bool>) (right => right == ')'));
        int num3 = source.Count<char>((Func<char, bool>) (left => left == '('));
        if (num2 != num3)
        {
          int num4 = str.IndexOf(")", startIndex);
          int num5 = Math.Abs(num3 - num2);
          while (num5-- > 0 && num4 != -1)
            num4 = str.IndexOf(")", num4 + 1);
          num1 = num4;
          source = str.Substring(startIndex, num1 - startIndex + 1);
        }
        templates.Add(source);
        str = str.Substring(num1 + 1, str.Length - num1 - 1);
        startIndex = str.IndexOf("Template");
        if (startIndex != -1)
          num1 = str.Substring(startIndex).IndexOf(")") + startIndex;
      }
      return templates;
    }

    private static List<TemplateSubstitution> ParseTemplateSubstitutions(string template)
    {
      template = template == null ? string.Empty : template;
      string templateName = TemplateReplacementRegex.GetTemplateName(template);
      char[] chArray = new char[1]{ ',' };
      string[] strArray = template.Trim().Substring(0, template.Length - 1).Replace(templateName + "(", "").Split(chArray);
      List<TemplateSubstitution> templateSubstitutions = new List<TemplateSubstitution>();
      foreach (string input in strArray)
      {
        for (Match match = TemplateReplacementRegex.templateSubstituitionRegex.Match(input); match.Success; match = match.NextMatch())
        {
          string param = match.Groups["param"].Value;
          string str = match.Groups["value"].Value;
          bool isFieldName = match.Groups["bracket"].Value == "[";
          if (str.StartsWith("["))
            str = str.Replace("[", "").Replace("]", "");
          if (templateSubstitutions.Find((Predicate<TemplateSubstitution>) (sub => sub.ParameterName == param)) == null)
            templateSubstitutions.Add(new TemplateSubstitution(param, str, isFieldName));
        }
      }
      return templateSubstitutions;
    }
  }
}
