// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.ModelPathParser
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class ModelPathParser
  {
    public const int MULTIPLIER = 17;
    public const int MULTIPLIER_SQUARE = 289;
    private const int MaxTokenLength = 100;

    private TData ParseInner<TData, TBuilder>(
      string path,
      IModelPathBuilder<TData, TBuilder> tokensBuilder)
      where TData : IModelPath
      where TBuilder : IModelPathBuilder<TData, TBuilder>
    {
      int num = 0;
      ModelPathParser.PositionCategory positionCategory = ModelPathParser.PositionCategory.Start;
      bool flag1 = false;
      bool flag2 = false;
      StringBuilder tokenValue = new StringBuilder();
      for (; num < path.Length; ++num)
      {
        char c1 = path[num];
        TBuilder builder;
        switch (positionCategory)
        {
          case ModelPathParser.PositionCategory.Start:
            if (c1 >= 'A' && c1 <= 'Z' || c1 >= 'a' && c1 <= 'z')
            {
              tokenValue.Append(c1);
              positionCategory = ModelPathParser.PositionCategory.Property;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.Property:
            if (c1 >= 'a' && c1 <= 'z' || c1 >= 'A' && c1 <= 'Z' || c1 >= '0' && c1 <= '9' || c1 == '_')
            {
              tokenValue.Append(c1);
              break;
            }
            switch (c1)
            {
              case ' ':
                tokensBuilder.StartFragment(tokenValue);
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.PropertyEnd;
                continue;
              case '.':
                tokensBuilder.StartFragment(tokenValue);
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.Dot;
                continue;
              case '[':
                if (flag1)
                  throw new ModelPathParseException(c1, num, path, "Two index qualifiers are not allowed. Index qualifiers should be added after property qualifiers.");
                tokensBuilder.StartFragment(tokenValue);
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.SquareStart;
                continue;
              default:
                throw new ModelPathParseException(c1, num, path);
            }
          case ModelPathParser.PositionCategory.PropertyEnd:
            switch (c1)
            {
              case ' ':
                continue;
              case '.':
                positionCategory = ModelPathParser.PositionCategory.Dot;
                continue;
              case '[':
                if (flag1)
                  throw new ModelPathParseException(c1, num, path, "Two index qualifiers are not allowed. Index qualifiers should be added after property qualifiers.");
                positionCategory = ModelPathParser.PositionCategory.SquareStart;
                continue;
              default:
                throw new ModelPathParseException(c1, num, path);
            }
          case ModelPathParser.PositionCategory.Dot:
            if (c1 >= 'A' && c1 <= 'Z' || c1 >= 'a' && c1 <= 'z')
            {
              flag1 = false;
              flag2 = false;
              tokenValue.Append(c1);
              positionCategory = ModelPathParser.PositionCategory.Property;
              tokensBuilder.AddFragment();
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.SquareStart:
            switch (c1)
            {
              case '%':
                positionCategory = ModelPathParser.PositionCategory.PlaceholderIndex;
                continue;
              case '(':
                if (flag2)
                  throw new ModelPathParseException(c1, num, path, "Two property qualifiers are not allowed.");
                positionCategory = ModelPathParser.PositionCategory.RoundStart;
                continue;
              default:
                if (c1 >= '0' && c1 <= '9')
                {
                  flag1 = true;
                  tokenValue.Append(c1);
                  positionCategory = ModelPathParser.PositionCategory.FixedIndex;
                  continue;
                }
                if (c1 != ' ')
                  throw new ModelPathParseException(c1, num, path);
                continue;
            }
          case ModelPathParser.PositionCategory.RoundStart:
          case ModelPathParser.PositionCategory.QualifierAndEnd:
            if (c1 >= 'A' && c1 <= 'Z' || c1 >= 'a' && c1 <= 'z')
            {
              tokenValue.Append(c1);
              positionCategory = ModelPathParser.PositionCategory.QualifierPropertyName;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.QualifierPropertyName:
            if (c1 >= 'a' && c1 <= 'z' || c1 >= 'A' && c1 <= 'Z' || c1 >= '0' && c1 <= '9' || c1 == '_')
            {
              tokenValue.Append(c1);
              break;
            }
            if (c1 == ' ')
            {
              tokensBuilder.StartQualifier(tokenValue);
              tokenValue.Clear();
              positionCategory = ModelPathParser.PositionCategory.QualifierPropertyNameEnd;
              break;
            }
            if (c1 != '=')
              throw new ModelPathParseException(c1, num, path);
            tokensBuilder.StartQualifier(tokenValue);
            tokenValue.Clear();
            positionCategory = ModelPathParser.PositionCategory.QualifierComparisonStart;
            break;
          case ModelPathParser.PositionCategory.QualifierPropertyNameEnd:
            if (c1 == '=')
            {
              positionCategory = ModelPathParser.PositionCategory.QualifierComparisonStart;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.QualifierComparisonStart:
            if (c1 != '=')
              throw new ModelPathParseException(c1, num, path);
            positionCategory = ModelPathParser.PositionCategory.QualifierComparisonEnd;
            break;
          case ModelPathParser.PositionCategory.QualifierComparisonEnd:
            if (c1 != ' ')
            {
              if (c1 >= '0' && c1 <= '9')
              {
                tokenValue.Append(c1);
                positionCategory = ModelPathParser.PositionCategory.QualifierNumericValue;
                break;
              }
              if (c1 == 't' || c1 == 'T' || c1 == 'f' || c1 == 'F')
              {
                tokenValue.Append(c1);
                string str1 = c1 == 't' || c1 == 'T' ? "rue" : "alse";
                string str2 = c1 == 't' || c1 == 'T' ? "RUE" : "ALSE";
                int index;
                for (index = 0; num + 1 < path.Length && index < str1.Length; ++index)
                {
                  ++num;
                  char c2 = path[num];
                  if ((int) c2 != (int) str1[index] && (int) c2 != (int) str2[index])
                    throw new ModelPathParseException(c2, num, path);
                  tokenValue.Append(c2);
                }
                if (index < str1.Length)
                  throw new IncompleteModelPathParseException(path);
                builder = tokensBuilder.SetQualifierValue(tokenValue, QualifierValueType.Bool);
                builder.AddQualifierToFragment();
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.QualifierBoolValue;
                break;
              }
              if (c1 != '\'')
                throw new ModelPathParseException(c1, num, path);
              positionCategory = ModelPathParser.PositionCategory.QualifierValueQuoteStart;
              break;
            }
            break;
          case ModelPathParser.PositionCategory.QualifierValueQuoteStart:
          case ModelPathParser.PositionCategory.QualifierStringValue:
            if (c1 == '\'')
            {
              builder = tokensBuilder.SetQualifierValue(tokenValue, QualifierValueType.String);
              builder.AddQualifierToFragment();
              tokenValue.Clear();
              positionCategory = ModelPathParser.PositionCategory.QualifierValueQuoteEnd;
              break;
            }
            tokenValue.Append(c1);
            positionCategory = ModelPathParser.PositionCategory.QualifierStringValue;
            break;
          case ModelPathParser.PositionCategory.QualifierValueQuoteEnd:
          case ModelPathParser.PositionCategory.QualifierBoolValue:
          case ModelPathParser.PositionCategory.QualifierNumericValueEnd:
            switch (c1)
            {
              case ' ':
                continue;
              case '&':
                positionCategory = ModelPathParser.PositionCategory.QualifierAndStart;
                if (path[num + 1] == 'a' && path[num + 2] == 'm' && path[num + 3] == 'p' && path[num + 4] == ';')
                {
                  num += 4;
                  continue;
                }
                continue;
              case ')':
                flag2 = true;
                positionCategory = ModelPathParser.PositionCategory.RoundEnd;
                continue;
              default:
                throw new ModelPathParseException(c1, num, path);
            }
          case ModelPathParser.PositionCategory.QualifierNumericValue:
            if (c1 >= '0' && c1 <= '9')
            {
              tokenValue.Append(c1);
              positionCategory = ModelPathParser.PositionCategory.QualifierNumericValue;
              break;
            }
            switch (c1)
            {
              case ' ':
                builder = tokensBuilder.SetQualifierValue(tokenValue, QualifierValueType.Numeric);
                builder.AddQualifierToFragment();
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.QualifierNumericValueEnd;
                continue;
              case '&':
                builder = tokensBuilder.SetQualifierValue(tokenValue, QualifierValueType.Numeric);
                builder.AddQualifierToFragment();
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.QualifierAndStart;
                if (path[num + 1] == 'a' && path[num + 2] == 'm' && path[num + 3] == 'p' && path[num + 4] == ';')
                {
                  num += 4;
                  continue;
                }
                continue;
              case ')':
                flag2 = true;
                builder = tokensBuilder.SetQualifierValue(tokenValue, QualifierValueType.Numeric);
                builder.AddQualifierToFragment();
                tokenValue.Clear();
                positionCategory = ModelPathParser.PositionCategory.RoundEnd;
                continue;
              default:
                throw new ModelPathParseException(c1, num, path);
            }
          case ModelPathParser.PositionCategory.QualifierAndStart:
            if (c1 != '&')
              throw new ModelPathParseException(c1, num, path);
            positionCategory = ModelPathParser.PositionCategory.QualifierAndEnd;
            if (path[num + 1] == 'a' && path[num + 2] == 'm' && path[num + 3] == 'p' && path[num + 4] == ';')
            {
              num += 4;
              break;
            }
            break;
          case ModelPathParser.PositionCategory.RoundEnd:
            if (c1 == ']')
            {
              positionCategory = ModelPathParser.PositionCategory.SquareEnd;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.SquareEnd:
            switch (c1)
            {
              case ' ':
                continue;
              case '.':
                positionCategory = ModelPathParser.PositionCategory.Dot;
                continue;
              case '[':
                if (flag1)
                  throw new ModelPathParseException(c1, num, path, "Two index qualifiers are not allowed. Index qualifiers should be added after property qualifiers.");
                positionCategory = ModelPathParser.PositionCategory.SquareStart;
                continue;
              default:
                throw new ModelPathParseException(c1, num, path);
            }
          case ModelPathParser.PositionCategory.FixedIndex:
            if (c1 >= '0' && c1 <= '9')
            {
              tokenValue.Append(c1);
              break;
            }
            if (c1 == ']')
            {
              tokensBuilder.SetFragmentIndex(tokenValue, FragmentIndexType.FixedOneBased);
              tokenValue.Clear();
              positionCategory = ModelPathParser.PositionCategory.SquareEnd;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            tokensBuilder.SetFragmentIndex(tokenValue, FragmentIndexType.FixedOneBased);
            tokenValue.Clear();
            positionCategory = ModelPathParser.PositionCategory.FixedIndexEnd;
            break;
          case ModelPathParser.PositionCategory.FixedIndexEnd:
            if (c1 == ']')
            {
              positionCategory = ModelPathParser.PositionCategory.SquareEnd;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
          case ModelPathParser.PositionCategory.PlaceholderIndex:
            if (c1 == ']')
            {
              tokensBuilder.SetFragmentIndex(FragmentIndexToken.PlaceholderIndexToken);
              positionCategory = ModelPathParser.PositionCategory.SquareEnd;
              break;
            }
            if (c1 != ' ')
              throw new ModelPathParseException(c1, num, path);
            break;
        }
      }
      if (positionCategory != ModelPathParser.PositionCategory.Property && positionCategory != ModelPathParser.PositionCategory.PropertyEnd)
        throw new IncompleteModelPathParseException(path);
      if (positionCategory == ModelPathParser.PositionCategory.Property)
      {
        tokensBuilder.StartFragment(tokenValue);
        tokenValue.Clear();
      }
      return tokensBuilder.Build();
    }

    public ModelPath Parse(string path)
    {
      return this.ParseInner<ModelPath, ModelPathBuilder>(path, (IModelPathBuilder<ModelPath, ModelPathBuilder>) new ModelPathBuilder());
    }

    public ModelPathLite ParseLite(string path)
    {
      return this.ParseInner<ModelPathLite, ModelPathLite>(path, (IModelPathBuilder<ModelPathLite, ModelPathLite>) new ModelPathLite());
    }

    private enum PositionCategory
    {
      Start,
      Property,
      PropertyEnd,
      Dot,
      SquareStart,
      RoundStart,
      QualifierPropertyName,
      QualifierPropertyNameEnd,
      QualifierComparisonStart,
      QualifierComparisonEnd,
      QualifierValueQuoteStart,
      QualifierStringValue,
      QualifierValueQuoteEnd,
      QualifierBoolValue,
      QualifierNumericValue,
      QualifierNumericValueEnd,
      QualifierAndStart,
      QualifierAndEnd,
      RoundEnd,
      SquareEnd,
      Space,
      FixedIndex,
      FixedIndexEnd,
      PlaceholderIndex,
    }
  }
}
