// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.Utils.SearchFilterUtil
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Exceptions;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.Utils
{
  public class SearchFilterUtil
  {
    public static string Pattern = "(?<!\\\\)',";
    public static char[] Operators = new char[3]
    {
      '=',
      ':',
      '<'
    };
    public const string FILTER_NOT_SUPPORTED = "'{0}' filter not supported.�";
    public const string VALUE_LESS_THAN_2_CHAR = "'{0}' filter value must be minimum 2 characters.�";
    public const string MALFORMED_QUERY = "'{0}' filter is either malformed or value not provided.�";
    public const string FILTER_DOESNT_SUPPORT_TYPE = "'{0}' filter does not support specified operator.�";
    public const string INVALID_VALUES = "'{0}' filter provided with invalid values. Allowed values are {1}.�";
    public const string NUMERIC_VALUES_ONLY = "'{0}' filter allows only numeric values.�";
    public const string DATETIME_VALUES_ONLY = "'{0}' filter allows only DateTime values in UTC format.�";
    public const string UNUPPORTED_OPERATOR = "{0} operator not supported for filter '{1}'.�";
    public const string UNEXPECTED_ERROR = "Unexpected error occured while parsing filter {0}.�";
    public const string DUPLICATE_FILTER_PROPERTY = "A property cannot be used more than once in the filter. Property is '{0}'�";

    public static string GetStringFragmentValue(
      string fragmentName,
      string filterQuery,
      ref int charIndex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      char minValue = char.MinValue;
      int num = filterQuery.Length - 1;
      if (charIndex == num)
        throw new SearchFilterParseException(string.Format("'{0}' filter is either malformed or value not provided.", (object) fragmentName));
      ++charIndex;
      SearchFilterUtil.SkipSpaces(filterQuery, num + 1, ref charIndex);
      if (charIndex == num || filterQuery[charIndex] != '\'')
        throw new SearchFilterParseException(string.Format("'{0}' filter is either malformed or value not provided.", (object) fragmentName));
      ++charIndex;
      while (charIndex <= num)
      {
        char ch = minValue;
        minValue = filterQuery[charIndex];
        if (minValue == '\'' && ch != '\\')
          return stringBuilder.ToString();
        if (minValue != '\\')
          stringBuilder.Append(minValue);
        ++charIndex;
      }
      throw new SearchFilterParseException(string.Format("'{0}' filter is either malformed or value not provided.", (object) fragmentName));
    }

    public static string GetNumberFragmentValue(
      string fragmentName,
      string filterQuery,
      ref int charIndex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = filterQuery.Length - 1;
      if (charIndex == num)
        throw new SearchFilterParseException(string.Format("'{0}' filter is either malformed or value not provided.", (object) fragmentName));
      do
      {
        ++charIndex;
        char ch = filterQuery[charIndex];
        if (ch == ',')
        {
          --charIndex;
          return SearchFilterUtil.RemoveSingleQuotes(stringBuilder.ToString().Trim());
        }
        stringBuilder.Append(ch);
      }
      while (charIndex != num);
      return SearchFilterUtil.RemoveSingleQuotes(stringBuilder.ToString().Trim());
    }

    private static string RemoveSingleQuotes(string value)
    {
      if (value[value.Length - 1] == '\'' && value[0] == '\'')
      {
        value = value.Remove(value.Length - 1);
        value = value.Remove(0, 1);
      }
      return value;
    }

    public static void SkipSpaces(string filterQuery, int queryLength, ref int charIndex)
    {
      while (charIndex < queryLength && filterQuery[charIndex] == ' ')
        ++charIndex;
    }
  }
}
