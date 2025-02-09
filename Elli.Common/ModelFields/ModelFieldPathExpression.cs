// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelFields.ModelFieldPathExpression
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.Common.ModelFields
{
  public class ModelFieldPathExpression
  {
    private static Regex propertyRegex = new Regex("^(?<propertyName>\\w+)$", RegexOptions.Compiled);
    private static Regex collectionRegex = new Regex("^(?<collectionName>\\w+)(\\[\\((?<qualifiers>[^\\)\\]]+)\\)\\])?(?<index>\\[(\\d+|\\%)\\])?$", RegexOptions.Compiled);
    private static readonly Dictionary<string, ModelFieldPathExpression> expressionCache = new Dictionary<string, ModelFieldPathExpression>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private readonly List<ModelFieldPathQualifier> _qualifiers = new List<ModelFieldPathQualifier>();

    public static ModelFieldPathExpression Parse(string pathExpression)
    {
      ModelFieldPathExpression fieldPathExpression1 = (ModelFieldPathExpression) null;
      lock (ModelFieldPathExpression.expressionCache)
      {
        if (ModelFieldPathExpression.expressionCache.TryGetValue(pathExpression, out fieldPathExpression1))
          return fieldPathExpression1;
      }
      ModelFieldPathExpression fieldPathExpression2 = new ModelFieldPathExpression(pathExpression);
      lock (ModelFieldPathExpression.expressionCache)
        ModelFieldPathExpression.expressionCache[pathExpression] = fieldPathExpression2;
      return fieldPathExpression2;
    }

    private ModelFieldPathExpression(string pathExpression)
    {
      using (PerformanceMeter.Current.BeginOperation("ModelFieldPathExpression.Constructor"))
      {
        this.Path = pathExpression.Replace("&amp;&amp;", "&&");
        if (this.MatchPropertyExpression(this.Path))
        {
          this.IsProperty = true;
        }
        else
        {
          if (!this.MatchCollectionExpression(this.Path))
            return;
          this.IsCollection = true;
        }
      }
    }

    private bool MatchPropertyExpression(string pathExpression)
    {
      Match match = ModelFieldPathExpression.propertyRegex.Match(pathExpression);
      if (match.Success)
        this.PropertyName = match.Groups["propertyName"].Value;
      return match.Success;
    }

    private bool MatchCollectionExpression(string pathExpression)
    {
      Match match = ModelFieldPathExpression.collectionRegex.Match(pathExpression);
      this._qualifiers.Clear();
      if (match.Success)
      {
        if (match.Groups["collectionName"].Success)
          this.CollectionName = match.Groups["collectionName"].Value;
        if (match.Groups["qualifiers"].Success)
        {
          string str1 = match.Groups["qualifiers"].Value;
          string[] separator = new string[1]{ "&&" };
          foreach (string str2 in str1.Split(separator, StringSplitOptions.None))
          {
            bool isValueQuoted = false;
            string[] strArray = str2.Trim().Split(new string[1]
            {
              "=="
            }, StringSplitOptions.None);
            string name = strArray[0].Trim();
            string str3 = strArray[1].Trim(' ');
            if (str3.StartsWith("'") && str3.EndsWith("'"))
            {
              isValueQuoted = true;
              str3 = strArray[1].Trim(' ', '\'');
            }
            this._qualifiers.Add(new ModelFieldPathQualifier(name, str3, isValueQuoted));
          }
        }
        if (match.Groups["index"].Success)
        {
          string s = match.Groups["index"].Value.Replace("[", string.Empty).Replace("]", string.Empty);
          if (s == "%")
            this.IsRepeatableCollection = true;
          else
            this.Index = int.Parse(s);
        }
      }
      return match.Success;
    }

    public bool IsProperty { get; private set; }

    public bool IsRepeatableCollection { get; private set; }

    public bool IsCollection { get; private set; }

    public string PropertyName { get; private set; }

    public string CollectionName { get; private set; }

    public string Condition1Name { get; private set; }

    public string Condition1Value { get; private set; }

    public string Condition2Name { get; private set; }

    public string Condition2Value { get; private set; }

    public bool IsAndedConditioned { get; private set; }

    public bool HasQualifier => this._qualifiers.Count > 0;

    public int Index { get; private set; }

    public bool IsIndexed => this.Index > 0;

    public string Path { get; private set; }

    public IList<ModelFieldPathQualifier> Qualifiers
    {
      get => (IList<ModelFieldPathQualifier>) this._qualifiers;
    }
  }
}
