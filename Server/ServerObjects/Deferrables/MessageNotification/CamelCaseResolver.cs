// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.MessageNotification.CamelCaseResolver
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.MessageNotification
{
  public class CamelCaseResolver : DefaultContractResolver
  {
    private static Regex UnderscoreRegex = new Regex("_[a-zA-Z0-9]?", RegexOptions.CultureInvariant);
    private static MatchEvaluator UnderscoreEvaluator = (MatchEvaluator) (match =>
    {
      if (match.Value.Length <= 1)
        return "";
      string str = match.Index == 0 ? char.ToLower(match.Value[1], CultureInfo.InvariantCulture).ToString((IFormatProvider) CultureInfo.InvariantCulture) : char.ToUpper(match.Value[1], CultureInfo.InvariantCulture).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (match.Value.Length > 2)
        str += match.Value.Substring(2);
      return str;
    });

    protected override string ResolvePropertyName(string propertyName)
    {
      return this.ResolvePropertyName(propertyName, int.MaxValue);
    }

    protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
    {
      JsonDictionaryContract dictionaryContract = base.CreateDictionaryContract(objectType);
      dictionaryContract.DictionaryKeyResolver = (Func<string, string>) (propertyName => propertyName);
      return dictionaryContract;
    }

    internal string ResolvePropertyName(string propertyName, int version)
    {
      if (string.IsNullOrEmpty(propertyName))
        return propertyName;
      string input = propertyName;
      if (char.IsUpper(propertyName[0]))
      {
        input = char.ToLower(propertyName[0], CultureInfo.InvariantCulture).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        if (propertyName.Length > 1)
          input += propertyName.Substring(1);
      }
      if (input.Contains("_"))
        input = CamelCaseResolver.UnderscoreRegex.Replace(input, CamelCaseResolver.UnderscoreEvaluator);
      return input;
    }
  }
}
