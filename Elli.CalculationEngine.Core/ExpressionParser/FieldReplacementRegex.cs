// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.FieldReplacementRegex
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  public class FieldReplacementRegex
  {
    internal static readonly Regex unbracketedFieldRegex = new Regex("(?<prefix>#|\\+|-|@)?(?<id>[^\\]]+)");
    internal static readonly Regex fieldRegex = new Regex("[\\[<](?<field>(?<prefix>#|\\+|-|@)?(?<id>[^\\]>\\s]+?))[\\]>]");
    internal static readonly Regex transientFieldRegex = new Regex("[\\[<](?<field>(?<prefix>#|\\+|-|@)?(?<id>\\^[^\\]>\\s]+?))[\\]>]");
    internal static readonly Regex entityNoTypeRegex = new Regex("(?<!\\{)\\{(?<entity>(?<name>[^\\{\\}:]+))\\}");
    internal static readonly Regex entityBaseTypeOnlyRegex = new Regex("(?<!\\{)\\{(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*))\\}");
    internal static readonly Regex entityRegex = new Regex("(?<!\\{)\\{(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*))\\}");
    internal static readonly Regex singleEntityRegex = new Regex("[\\s]\\{(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*))\\}");
    internal static readonly Regex firstEntityRegex = new Regex("(?<prefix>[=\\s])(?<!\\{)\\{(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*))\\}");
    internal static readonly Regex firstInLineEntityRegex = new Regex("^(?<!\\{)\\{(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*))\\}");
    internal static readonly Regex weakReferenceRegex = new Regex("<(?<weakReference>(?<id>[^\\s>]+?))>");
    internal static readonly Regex collectionNoTypeRegex = new Regex("\\{{2}(?<collection>(?<name>[^\\{\\}:]+))\\}{2}");
    internal static readonly Regex collectionRegex = new Regex("\\{{2}(?<collection>(?<name>[^\\{\\}:]+):?(?<type>(?<basetype>[^(\\}]*)[^\\}]*))\\}{2}");
    internal static readonly Regex collectionNoParametersRegex = new Regex("\\{{2}(?<collection>(?<name>[^\\{\\}:]+):?(?<type>[^(\\}]*))\\}{2}");
    internal static readonly Regex elementRegex = new Regex("(?<!\\{)\\{(?<element>(?<entity>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*)))\\}|\\{{2}(?<element>(?<collection>(?<name>[^\\{\\}:]+):?(?<type>[^\\}]*)))\\}{2}|[\\[<](?<element>(?<field>(?<prefix>#|\\+|-|@)?(?<name>[^\\]\\s]+?)))[\\]>]");
    internal static readonly Regex linqObjectParseRegex = new Regex("(?<linqObject>(?<linqVar>\\{{1}%{1}.+\\}{1})\\s(?i)in\\s(?<collection>\\{{1,2}[^\\]\\s]+\\]?\\}?))");
    internal static readonly Regex linqObjectValueParseRegex = new Regex("(?<linqObject>\\{%(?<linqValue>[^\\s.]+)\\})");
    internal static readonly Regex referenceParseRegex = new Regex("(?<complexId>\\{{1,2}[^\\]\\s%]+\\]?\\}?)|\\[(?<prefix>#|\\+|-|@)?(?<id>[^\\]]+)\\]|<(?<prefix>#|\\+|-|@)?(?<weakId>[^>\\s]+)>|(?<linqObject>\\{{1}%{1}[^\\}]+\\}{1}\\s(?i)in\\s\\{{1,2}[^\\]\\s]+\\]?\\}?)|(?<linqId>\\{{1}%{1}[^\\]\\s]+\\]?\\}?)");
    internal static readonly Regex propertyRegex = new Regex("(?<prefix>[^\\.])\\[(?<prop>[^\\^][^\\]\\s]+)\\]");
    internal static readonly Regex weakPropertyRegex = new Regex("(?<prefix>[^\\.])\\<(?<prop>[^\\^][^\\>\\s]+)\\>");
    internal static readonly Regex propertyDecimalRegex = new Regex("As Decimal =\\s*\\[(?<prop>[^\\^][^\\]]+)\\]", RegexOptions.IgnoreCase);
    internal static readonly Regex propertyIntegerRegex = new Regex("As Integer =\\s*\\[(?<prop>[^\\^][^\\]]+)\\]", RegexOptions.IgnoreCase);
    internal static readonly Regex propertyShortRegex = new Regex("As Short =\\s*\\[(?<prop>[^\\^][^\\]]+)\\]", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathDecimalRegex = new Regex("As Decimal =\\s*(?<path>\\{[^\\^\\]]+\\])", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathIntegerRegex = new Regex("As Integer =\\s*(?<path>\\{[^\\^][^\\]]+\\])", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathShortRegex = new Regex("As Short =\\s*(?<path>\\{[^\\^][^\\]]+\\])", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathBooleanRegex = new Regex("As Boolean =\\s*(?<path>\\{[^\\^][^\\]]+\\])", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathDecimalNotZeroCheckRegex = new Regex("\\s(?<path>\\{[^\\^\\]\\{]+\\])\\s*\\<\\>\\s*0d", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathBooleanTrueCheckRegex = new Regex("\\s(?<path>\\{[^\\^\\]\\{]+\\])\\s*=\\s*True", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathBooleanNotTrueCheckRegex = new Regex("\\s(?<path>\\{[^\\^\\]\\{]+\\])\\s*\\<\\>\\s*True", RegexOptions.IgnoreCase);
    internal static readonly Regex propertyEqRegex = new Regex("=\\s*\\[(?<prop>[^\\^][^\\]\\s+\\\"+\\%+]+)\\]", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathEqRegex = new Regex("=\\s*(?<path>\\{[^\\^][^\\]\\s+\\\"+\\%+]+\\])", RegexOptions.IgnoreCase);
    internal static readonly Regex fullPathFirstInLineRegex = new Regex("^(?<path>\\{[^\\^][^\\]\\s+\\\"+\\%+]+\\])");
    internal static readonly Regex fullPathLineRegex = new Regex("(?<prefix>[=\\(\\s*])(?<path>\\{[^\\^][^\\]\\s+\\\"+\\%+]+\\])");
    internal static readonly Regex fullPathLineEqualsRegex = new Regex("=(?<path>\\{[^\\^][^\\]\\s+\\\"+\\%+]+\\])");
    internal static readonly Regex propertyFirstInLineRegex = new Regex("^\\[(?<prop>[^\\^][^\\]]+)\\]");
    internal static readonly Regex objPropertyRegex = new Regex("\\.\\[(?<prop>[^\\^][^\\]]+)\\]");
    internal static readonly Regex getFieldRegex = new Regex("GetFieldValue\\(\"(?<prop>[^\\^][^\\)]+)\\)");
    internal static readonly Regex dimVariableNameRegex = new Regex("(?i)Dim\\s+(?<varName>[^\\s\\(\\?]*)");
    internal static readonly Regex dimVariableNameTypeRegex = new Regex("(?i)Dim\\s+(?<varName>[^\\s\\(]*)\\s+As\\s+(?<varType>[^\\s\\(]*)");
    internal static readonly Regex removeCommentRegex = new Regex("(?mn)^(?<line>[^\\r\\n\"R']*((\"[^\"]*\"|(?!REM)R)[^\\r\\n\"R']*)*)(REM|')[^\\r\\n]*");
    internal static readonly Regex removeBlankLineRegex = new Regex("^\\s+$[\\r\\n]*", RegexOptions.Multiline);
    internal static readonly Regex getFieldValRegex = new Regex("\\.GetFieldValue\\(\"(?<prop>[^\\^][^\\)]+)\"\\)");
    internal static readonly Regex getFieldValSpaceRegex = new Regex("\\sGetFieldValue\\(\"(?<prop>[^\\^][^\\)]+)\"\\)");
    internal static readonly Regex redundantXDec = new Regex("XDec\\(XDec\\(", RegexOptions.IgnoreCase);
    internal static readonly Regex redundantXString = new Regex("XString\\(XString\\(", RegexOptions.IgnoreCase);
    internal static Assembly _objectModelAssembly;
    internal static readonly string Operator = nameof (Operator);
    internal static readonly string OperatorLine = "Line";
    internal static readonly string OperatorLineRange = "LineRange";
    internal static readonly string OperatorRange = "Range";
    internal static int _cachedQueryIndex = 0;
    public static readonly string FieldPrefix = "__";
    private const bool _useSumCaching = false;
    private const bool _useCollectionCasting = true;
    private const bool _useCachedParameterizedQueries = false;
    private static List<string> _nullableBooleanCheckLineList = new List<string>();
    private static List<string> _nullableDecimalCheckLineList = new List<string>();
    private static List<string> _nullableShortCheckLineList = new List<string>();
    private static Dictionary<string, string> _linqObjectDictionary = new Dictionary<string, string>();
    private static List<string> _linqInStatementlist = new List<string>();
    private static HashSet<string> _linqUseWrapperHash = new HashSet<string>();

    public static EntityDescriptor ParseEntityDescriptor(string fullEntityType)
    {
      string type = fullEntityType;
      List<EntityParameter> parameters = (List<EntityParameter>) null;
      try
      {
        int length = fullEntityType.IndexOf('(');
        if (length > 0)
        {
          type = fullEntityType.Substring(0, length);
          parameters = new List<EntityParameter>();
          string str = fullEntityType.Substring(length + 1, fullEntityType.Length - length - 2);
          char[] chArray = new char[1]{ ',' };
          foreach (string fullParameter in str.Split(chArray))
          {
            EntityParameter entityParameter = new EntityParameter(fullParameter);
            parameters.Add(entityParameter);
          }
        }
        return new EntityDescriptor(type, (IEnumerable<EntityParameter>) parameters);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error parsing entity \"{0}\"", (object) fullEntityType));
      }
    }

    public static void ParseFieldId(string id, out string formatSpecifier, out string fieldId)
    {
      formatSpecifier = FieldReplacementRegex.unbracketedFieldRegex.Match(id).Groups["prefix"].Value;
      fieldId = FieldReplacementRegex.unbracketedFieldRegex.Match(id).Groups[nameof (id)].Value;
    }

    public static string ParseFieldIdWithBrackets(string field)
    {
      return FieldReplacementRegex.fieldRegex.Match(field).Groups["id"].Value;
    }

    public static ReferencedElement ParseFullyQualifiedFieldName(
      string qualifiedName,
      EntityDescriptor parentType,
      EntityDescriptor rootEntityType,
      string rootRelationship,
      out EntityDescriptor outParentType,
      out string fieldId,
      out string relationship)
    {
      outParentType = (EntityDescriptor) null;
      fieldId = string.Empty;
      relationship = string.Empty;
      ReferencedElement parentElement = new ReferencedElement(string.Empty, parentType, DataElementType.DataEntityType, (ReferencedElement) null, false);
      List<ReferencedElement> elementList = new List<ReferencedElement>();
      List<string> stringList = new List<string>();
      FieldReplacementRegex.ParseComplexId(qualifiedName, parentElement, elementList, rootEntityType, rootRelationship);
      if (parentElement.ReferencedElements.Count == 0)
        return parentElement;
      ReferencedElement referencedElement = parentElement;
      while (referencedElement != null && referencedElement.DataElementType != DataElementType.DataFieldType)
      {
        if (referencedElement.ReferencedElements.Count > 0)
        {
          referencedElement = referencedElement.ReferencedElements.First<ReferencedElement>();
          if (referencedElement.DataElementType == DataElementType.DataFieldType)
          {
            fieldId = referencedElement.Name;
            outParentType = referencedElement.ParentElement != null ? referencedElement.ParentElement.EntityType : (EntityDescriptor) null;
          }
          else
          {
            string str1 = string.Format("{{{0}}}", (object) referencedElement.Name);
            if (referencedElement.Name == CalculationUtility.ALL_ENTITIES || referencedElement.DataElementType == DataElementType.DataEntityCollectionType)
              str1 = string.Format("{{{{{0}:{1}}}}}", (object) referencedElement.Name, (object) referencedElement.EntityType);
            string str2 = string.IsNullOrEmpty(relationship) ? str1 : string.Format(".{0}", (object) str1);
            relationship += str2;
            stringList.Add(referencedElement.Name);
          }
        }
        else
          referencedElement = (ReferencedElement) null;
      }
      string str = string.Format("{{{0}}}", (object) rootRelationship);
      if (relationship.Contains(str))
      {
        int startIndex = relationship.IndexOf(str);
        if (startIndex > 0)
          relationship = relationship.Substring(startIndex);
      }
      return parentElement;
    }

    private static string GetSubStrings(string input, int start, out int nextstart)
    {
      int startIndex = input.IndexOf("SumAnyEnumerable", start);
      nextstart = startIndex;
      if (startIndex == -1)
      {
        nextstart = startIndex;
        return "";
      }
      string str = input.Substring(startIndex);
      string subStrings = "";
      int num = 0;
      foreach (char ch in str)
      {
        switch (ch)
        {
          case '(':
            ++num;
            break;
          case ')':
            --num;
            if (num == 0)
            {
              subStrings += ch.ToString();
              goto label_9;
            }
            else
              break;
        }
        subStrings += ch.ToString();
      }
label_9:
      return subStrings;
    }

    private static ReferencedElement ParseField(
      string fieldId,
      ReferencedElement parentElement,
      bool isWeak)
    {
      ReferencedElement field = (ReferencedElement) null;
      if (!fieldId.StartsWith("$"))
        field = new ReferencedElement(fieldId, (EntityDescriptor) null, DataElementType.DataFieldType, parentElement, isWeak);
      return field;
    }

    public static ReferencedElement ParseComplexId(
      string complexId,
      ReferencedElement parentElement,
      List<ReferencedElement> elementList,
      EntityDescriptor rootEntityType,
      string rootRelationship,
      bool ignoreCollections = true)
    {
      bool isWeak = false;
      ReferencedElement complexId1 = parentElement;
      ReferencedElement parentElement1 = complexId1;
      if (ignoreCollections && complexId.IndexOf('[') < 0 && complexId.IndexOf('<') < 0)
        return (ReferencedElement) null;
      string str1 = complexId;
      string[] separator = new string[1]{ "}." };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.None))
      {
        DataElementType dataElementType = DataElementType.Unknown;
        int startIndex1 = 0;
        int num1 = 0;
        int startIndex2 = 0;
        int num2 = 0;
        if (str2[0] == '[')
        {
          dataElementType = DataElementType.DataFieldType;
          num1 = str2.IndexOf(']');
          startIndex1 = 1;
        }
        else if (str2[0] == '<')
        {
          isWeak = true;
          dataElementType = DataElementType.DataFieldType;
          num1 = str2.IndexOf('>');
          startIndex1 = 1;
        }
        else if (str2[0] == '{')
        {
          if (str2[1] == '{')
          {
            dataElementType = DataElementType.DataEntityCollectionType;
            startIndex1 = 2;
            num2 = str2.IndexOf('}');
          }
          else
          {
            dataElementType = DataElementType.DataEntityType;
            startIndex1 = 1;
            num2 = str2.Length;
            if (str2.IndexOf('}') > 0)
              num2 = str2.IndexOf('}');
          }
          startIndex2 = str2.IndexOf(':');
          if (startIndex2 > 0)
          {
            num1 = startIndex2;
            ++startIndex2;
          }
          else
            num1 = num2;
        }
        string name = str2.Substring(startIndex1, num1 - startIndex1);
        string str3 = startIndex2 > 0 ? str2.Substring(startIndex2, num2 - startIndex2) : string.Empty;
        string fullEntityType = string.IsNullOrEmpty(str3) ? (name == rootRelationship ? rootEntityType.ToString() : name) : str3;
        EntityDescriptor descriptor = (EntityDescriptor) null;
        if (dataElementType != DataElementType.DataFieldType)
          descriptor = FieldReplacementRegex.ParseEntityDescriptor(fullEntityType);
        if (!(name == parentElement1.Name) || !(descriptor == parentElement1.EntityType))
        {
          ReferencedElement element = new ReferencedElement(name, descriptor, dataElementType, parentElement1, isWeak);
          if (complexId1 == null)
          {
            if (elementList != null && elementList.Exists((Predicate<ReferencedElement>) (p => p.Name == name)))
              element = elementList.Find((Predicate<ReferencedElement>) (p => p.Name == name));
            parentElement1 = complexId1 = element;
          }
          else
          {
            if (!parentElement1.ReferencedElements.Exists((Predicate<ReferencedElement>) (p => p.Name == name && p.EntityType == descriptor)))
              parentElement1.AddReferencedElement(element);
            parentElement1 = parentElement1.ReferencedElements.Find((Predicate<ReferencedElement>) (p => p.Name == name && p.EntityType == descriptor));
          }
        }
        if (name == rootRelationship || fullEntityType == rootEntityType.ToString())
        {
          complexId1 = parentElement1;
          complexId1.ParentElement = (ReferencedElement) null;
        }
      }
      return complexId1;
    }

    private static List<ReferencedElement> ParseLinq(
      List<string> linqObjectList,
      List<string> linqIdList,
      ReferencedElement parentElement,
      List<ReferencedElement> elementList,
      EntityDescriptor rootEntityType,
      string rootRelationship,
      out string linqErrors)
    {
      linqErrors = string.Empty;
      Dictionary<string, string> source = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (string linqObject in linqObjectList)
      {
        for (Match match = FieldReplacementRegex.linqObjectParseRegex.Match(linqObject); match.Success; match = match.NextMatch())
        {
          string key = match.Groups["linqVar"].Value;
          string str1 = match.Groups["collection"].Value;
          string str2;
          if (source.TryGetValue(key, out str2))
          {
            int startIndex = str1.LastIndexOf(":");
            if (startIndex > 0 && str1.Substring(startIndex) != str2.Substring(str2.LastIndexOf(":")))
            {
              string message = string.Format("Error adding linq variable {0} = {1}. Variable is already set to {2}", (object) key, (object) str1, (object) str2);
              linqErrors += string.Format("{0}\r\n", (object) message);
              Tracing.Log(TraceLevel.Warning, nameof (FieldReplacementRegex), message);
            }
          }
          else
            source[key] = str1;
        }
      }
      foreach (KeyValuePair<string, string> keyValuePair in source.Reverse<KeyValuePair<string, string>>())
      {
        if (keyValuePair.Value.Contains("{%"))
        {
          for (Match match = FieldReplacementRegex.linqObjectValueParseRegex.Match(keyValuePair.Value); match.Success; match = match.NextMatch())
          {
            string str = match.Groups["linqObject"].Value;
            source[keyValuePair.Key] = keyValuePair.Value.Replace(str, source.ContainsKey(str) ? source[str] : str);
          }
        }
      }
      foreach (string linqId in linqIdList)
      {
        for (Match match = FieldReplacementRegex.linqObjectValueParseRegex.Match(linqId); match.Success; match = match.NextMatch())
        {
          string str = match.Groups["linqObject"].Value;
          ReferencedElement complexElement = FieldReplacementRegex.ParseComplexId(linqId.Replace(str, source.ContainsKey(str) ? source[str] : str), parentElement, elementList, rootEntityType, rootRelationship);
          if (complexElement != null && !elementList.Exists((Predicate<ReferencedElement>) (p => p.Name == complexElement.Name && p.EntityType == complexElement.EntityType)))
            elementList.Add(complexElement);
        }
      }
      return elementList;
    }

    public static ReferencedElement ParseReferencedElements(
      string sourceCode,
      string parentEntityType,
      bool ignoreCollections = true)
    {
      EntityDescriptor entityDescriptor1 = FieldReplacementRegex.ParseEntityDescriptor(parentEntityType);
      EntityDescriptor entityDescriptor2 = FieldReplacementRegex.ParseEntityDescriptor(CalculationUtility.GetRootEntityType());
      return FieldReplacementRegex.ParseReferencedElements(sourceCode, entityDescriptor1, string.Empty, entityDescriptor2, CalculationUtility.GetRootRelationship(), ignoreCollections);
    }

    public static ReferencedElement ParseReferencedElements(
      string sourceCode,
      EntityDescriptor parentEntityType,
      string parentEntityName,
      EntityDescriptor rootEntityType,
      string rootRelationship,
      bool ignoreCollections = true)
    {
      string pattern = "'(.*?)\r?\n";
      sourceCode = Regex.Replace(sourceCode, pattern, "");
      ReferencedElement parentElement = EntityDescriptor.IsNullOrEmpty(parentEntityType) || parentEntityType == rootEntityType ? new ReferencedElement(rootRelationship, rootEntityType, DataElementType.DataEntityType, (ReferencedElement) null, false) : (EntityDescriptor.IsNullOrEmpty(parentEntityType) ? (ReferencedElement) null : new ReferencedElement(parentEntityName, parentEntityType, DataElementType.DataEntityType, (ReferencedElement) null, false));
      List<ReferencedElement> elementList = new List<ReferencedElement>();
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FieldReplacementRegex.referenceParseRegex.Match(sourceCode);
      List<string> linqObjectList = new List<string>();
      List<string> linqIdList = new List<string>();
      for (; match.Success; match = match.NextMatch())
      {
        string fieldId = match.Groups["id"].Value;
        bool isWeak = false;
        if (string.IsNullOrEmpty(fieldId))
        {
          fieldId = match.Groups["weakId"].Value;
          isWeak = true;
        }
        if (!string.IsNullOrEmpty(fieldId) && !elementList.Exists((Predicate<ReferencedElement>) (p => p.Name == fieldId)))
        {
          ReferencedElement field = FieldReplacementRegex.ParseField(fieldId, parentElement, isWeak);
          if (field != null)
            parentElement.AddReferencedElement(field);
        }
        string complexId = match.Groups["complexId"].Value;
        if (!string.IsNullOrEmpty(complexId))
        {
          ReferencedElement complexElement = FieldReplacementRegex.ParseComplexId(complexId, parentElement, elementList, rootEntityType, rootRelationship, ignoreCollections);
          if (complexElement != null && !elementList.Exists((Predicate<ReferencedElement>) (p => p.Name == complexElement.Name)))
            elementList.Add(complexElement);
        }
        string str1 = match.Groups["linqObject"].Value;
        string str2 = match.Groups["linqId"].Value;
        if (!string.IsNullOrEmpty(str1))
          linqObjectList.Add(str1);
        if (!string.IsNullOrEmpty(str2))
          linqIdList.Add(str2);
      }
      string linqErrors = string.Empty;
      FieldReplacementRegex.ParseLinq(linqObjectList, linqIdList, parentElement, elementList, rootEntityType, rootRelationship, out linqErrors);
      return parentElement;
    }

    public static ReferencedElement ParseLinqReferencedElements(
      string sourceCode,
      string parentEntityType,
      out string linqErrors)
    {
      linqErrors = string.Empty;
      EntityDescriptor entityDescriptor1 = FieldReplacementRegex.ParseEntityDescriptor(parentEntityType);
      EntityDescriptor entityDescriptor2 = FieldReplacementRegex.ParseEntityDescriptor(CalculationUtility.GetRootEntityType());
      return FieldReplacementRegex.ParseLinqReferencedElements(sourceCode, entityDescriptor1, string.Empty, entityDescriptor2, CalculationUtility.GetRootRelationship(), out linqErrors);
    }

    public static ReferencedElement ParseLinqReferencedElements(
      string sourceCode,
      EntityDescriptor parentEntityType,
      string parentEntityName,
      EntityDescriptor rootEntityType,
      string rootRelationship,
      out string linqErrors)
    {
      string pattern = "'(.*?)\r?\n";
      sourceCode = Regex.Replace(sourceCode, pattern, "");
      ReferencedElement parentElement = EntityDescriptor.IsNullOrEmpty(parentEntityType) || parentEntityType == rootEntityType ? new ReferencedElement(rootRelationship, rootEntityType, DataElementType.DataEntityType, (ReferencedElement) null, false) : (EntityDescriptor.IsNullOrEmpty(parentEntityType) ? (ReferencedElement) null : new ReferencedElement(parentEntityName, parentEntityType, DataElementType.DataEntityType, (ReferencedElement) null, false));
      List<ReferencedElement> elementList = new List<ReferencedElement>();
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FieldReplacementRegex.referenceParseRegex.Match(sourceCode);
      List<string> linqObjectList = new List<string>();
      List<string> linqIdList = new List<string>();
      for (; match.Success; match = match.NextMatch())
      {
        string str1 = match.Groups["linqObject"].Value;
        string str2 = match.Groups["linqId"].Value;
        if (!string.IsNullOrEmpty(str1))
          linqObjectList.Add(str1);
        if (!string.IsNullOrEmpty(str2))
          linqIdList.Add(str2);
      }
      FieldReplacementRegex.ParseLinq(linqObjectList, linqIdList, parentElement, elementList, rootEntityType, rootRelationship, out linqErrors);
      return parentElement;
    }

    public static List<string> ParseTransientFields(string sourceCode)
    {
      string pattern = "'(.*?)\r?\n";
      sourceCode = Regex.Replace(sourceCode, pattern, "");
      sourceCode = string.IsNullOrWhiteSpace(sourceCode) ? string.Empty : sourceCode;
      Match match = FieldReplacementRegex.fieldRegex.Match(sourceCode);
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (; match.Success; match = match.NextMatch())
      {
        string key = match.Groups["id"].Value;
        if (!dictionary.ContainsKey(key) && key.StartsWith("^T."))
          dictionary[key] = key;
      }
      string[] strArray = new string[dictionary.Count];
      if (dictionary.Count > 0)
        dictionary.Keys.CopyTo(strArray, 0);
      return new List<string>((IEnumerable<string>) strArray);
    }

    public static string ReplaceAt(string str, int index, int length, string replace)
    {
      return str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);
    }

    public static string FormatSourceCode(
      string calculationSourceCode,
      string calculationFieldId,
      string calculationParentEntityType,
      Assembly objectModelAssembly)
    {
      FieldReplacementRegex._objectModelAssembly = objectModelAssembly;
      if (calculationFieldId.ToUpper().StartsWith("^T.PERF_"))
        FieldReplacementRegex._objectModelAssembly = (Assembly) null;
      string input1 = calculationSourceCode;
      EntityDescriptor entityDescriptor = FieldReplacementRegex.ParseEntityDescriptor(calculationParentEntityType);
      Type parentInfo = FieldReplacementRegex.GetParentInfo(entityDescriptor.EntityType);
      string input2 = FieldReplacementRegex.removeCommentRegex.Replace(input1, "${line}\r\n");
      string singleLine = FieldReplacementRegex.MoveToSingleLine(FieldReplacementRegex.removeBlankLineRegex.Replace(input2, "").Replace("Nullable (Of Boolean)", "Boolean?"));
      FieldReplacementRegex.BuildTypeCheckLineLists(singleLine);
      FieldReplacementRegex.BuildLinqDictionaries(singleLine);
      string input3 = Regex.Replace(FieldReplacementRegex.fieldRegex.Replace(singleLine, (MatchEvaluator) (match => match.Value.Replace('.', '_'))), "(?<openBracket>[\\[<])(?<field>[^\\]>\\s]+)(?<closeBracket>[\\]>])", "[${field}]");
      string input4 = FieldReplacementRegex.getFieldValRegex.Replace(input3, ".[${prop}]");
      string soureCode = Regex.Replace(FieldReplacementRegex.getFieldValSpaceRegex.Replace(input4, " [${prop}]").Replace(".ToUpper", ".ToString().ToUpper"), "(?<openBracket>[\\[<])(?<field>\\d+[^\\]>\\s]+)(?<closeBracket>[\\]>])", string.Format("{0}{1}{2}{3}", (object) "${openBracket}", (object) FieldReplacementRegex.FieldPrefix, (object) "${field}", (object) "${closeBracket}"));
      string contextRoot = string.Format("{0}ContextRoot", (object) entityDescriptor.EntityType);
      if (entityDescriptor.EntityType != "FeeVariance")
      {
        string str = soureCode;
        foreach (KeyValuePair<string, string> linqObject in FieldReplacementRegex._linqObjectDictionary)
        {
          string objType = EntityDescriptor.Create(linqObject.Value).EntityType;
          if ((Assembly) null != FieldReplacementRegex._objectModelAssembly && ((IEnumerable<Type>) FieldReplacementRegex._objectModelAssembly.GetTypes()).Any<Type>((Func<Type, bool>) (t => t.Name == objType)))
          {
            string oldValue = string.Format("For Each {0} As Object", (object) linqObject.Key);
            string newValue = string.Format("For Each {0} As {1}.{2}", (object) linqObject.Key, (object) CalculationUtility.GetObjectModelNamespace(), (object) objType);
            str = str.Replace(oldValue, newValue);
          }
        }
        soureCode = str;
      }
      string input5 = FieldReplacementRegex.FormatObjectModelFieldReference(FieldReplacementRegex.FormatLinqSelectDatasources(soureCode, entityDescriptor, contextRoot, calculationFieldId, parentInfo == (Type) null), entityDescriptor, contextRoot, calculationFieldId, parentInfo == (Type) null);
      string str1 = FieldReplacementRegex.ReplaceEnumTypeComparisons(FieldReplacementRegex.RenameVariablesWithFieldPrefix(FieldReplacementRegex.linqObjectValueParseRegex.Replace(input5, "obj${linqValue}"))).Replace(" GetFieldValue(", " context.GetFieldValue(").Replace("(GetFieldValue(", "(context.GetFieldValue(").Replace(" GetRelatedWrappedEntity(", " context.GetRelatedWrappedEntity(").Replace("(GetRelatedWrappedEntity(", "(context.GetRelatedWrappedEntity(");
      Regex regex1 = new Regex("(?<prefix>[^\\.+])GetField\\(");
      Regex regex2 = new Regex("(?<prefix>[^\\.+])GetFieldValue\\(");
      Regex regex3 = new Regex("(?<prefix>[^\\.+])GetRelatedWrappedEntity\\(");
      Regex regex4 = new Regex("(?<prefix>[^\\.+])GetRelatedWrappedEntities\\(");
      string input6 = str1;
      string input7 = regex1.Replace(input6, "${prefix}context.GetField(");
      string input8 = regex2.Replace(input7, "${prefix}context.GetFieldValue(");
      string input9 = regex3.Replace(input8, "${prefix}context.GetRelatedWrappedEntity(");
      string str2 = regex4.Replace(input9, "${prefix}context.GetRelatedWrappedEntities(");
      Regex regex5 = new Regex("^GetField\\(");
      Regex regex6 = new Regex("^GetFieldValue\\(");
      Regex regex7 = new Regex("^GetRelatedWrappedEntity\\(");
      string input10 = str2;
      string input11 = regex5.Replace(input10, "context.GetField(");
      string input12 = regex6.Replace(input11, "context.GetFieldValue(");
      return new Regex("^InvokeMethod\\(").Replace(new Regex("\\sLock\\(").Replace(regex7.Replace(input12, "context.GetRelatedWrappedEntity(").Replace("IsModified(", " context.IsModified(").Replace(" IsLocked(", " context.IsLocked(").Replace("Unlock(", "context.Unlock(").Replace("UnLock(", "context.Unlock("), " context.Lock(").Replace(" InvokeMethod(", " context.InvokeMethod("), "context.InvokeMethod(");
    }

    private static Type GetParentType(ReferencedElement refElement, string parentEntityType)
    {
      Type parentType = (Type) null;
      ReferencedElement fieldElement = refElement == null ? (ReferencedElement) null : refElement.FieldElement;
      if (fieldElement != null)
      {
        string fieldParentType = parentEntityType;
        ReferencedElement parentElement = fieldElement.ParentElement;
        if (parentElement != null)
        {
          if (parentElement.Name.StartsWith("obj"))
          {
            string key = "{%" + parentElement.Name.Substring(3) + "}";
            if (FieldReplacementRegex._linqObjectDictionary.ContainsKey(key))
              fieldParentType = FieldReplacementRegex.ParseEntityDescriptor(FieldReplacementRegex._linqObjectDictionary[key]).EntityType;
          }
          else
            fieldParentType = parentElement.EntityType.EntityType;
        }
        parentType = FieldReplacementRegex.GetParentInfo(fieldParentType);
      }
      return parentType;
    }

    private static Type GetParentInfo(string fieldParentType)
    {
      Type parentInfo = (Type) null;
      if (FieldReplacementRegex._objectModelAssembly != (Assembly) null)
        parentInfo = FieldReplacementRegex._objectModelAssembly.GetType(string.Format("{0}.{1}", (object) CalculationUtility.GetObjectModelNamespace(), (object) fieldParentType));
      return parentInfo;
    }

    private static string MoveToSingleLine(string sourceCode)
    {
      string singleLine = "";
      StringReader stringReader = new StringReader(sourceCode);
      while (true)
      {
        string str1 = stringReader.ReadLine();
        if (str1 != null)
        {
          string str2;
          string str3;
          for (str2 = str1.TrimEnd(); str2.EndsWith("_") || str2.EndsWith("=") || str2.EndsWith("AndAlso") || str2.EndsWith("OrElse"); str2 = (str3 + stringReader.ReadLine().TrimStart()).TrimEnd())
          {
            if (str2.EndsWith("_"))
              str3 = str2.TrimEnd('_');
            else
              str3 = str2 + " ";
          }
          singleLine = singleLine + str2 + Environment.NewLine;
        }
        else
          break;
      }
      sourceCode = singleLine;
      return singleLine;
    }

    private static void BuildLinqDictionaries(string sourceCode)
    {
      List<ReferencedElement> referencedElementList = new List<ReferencedElement>();
      Match match1 = FieldReplacementRegex.referenceParseRegex.Match(sourceCode);
      List<string> stringList = new List<string>();
      FieldReplacementRegex._linqObjectDictionary.Clear();
      FieldReplacementRegex._linqInStatementlist.Clear();
      FieldReplacementRegex._linqUseWrapperHash.Clear();
      List<Tuple<string, string, string, string>> tupleList = new List<Tuple<string, string, string, string>>();
      for (; match1.Success; match1 = match1.NextMatch())
      {
        string input1 = match1.Groups["linqObject"].Value;
        string str1 = match1.Groups["linqId"].Value;
        if (!string.IsNullOrEmpty(input1))
        {
          Match match2 = FieldReplacementRegex.linqObjectParseRegex.Match(input1);
          if (match2.Success)
          {
            string key = match2.Groups["linqVar"].Value;
            FieldReplacementRegex._linqInStatementlist.Add(input1);
            string input2 = match2.Groups["collection"].Value;
            Match match3 = FieldReplacementRegex.collectionRegex.Match(input2);
            string str2 = match3.Groups["type"].Value;
            string name = match3.Groups["name"].Value;
            if (string.IsNullOrEmpty(str2))
              str2 = name;
            string str3 = key;
            string str4 = string.Empty;
            string str5 = name;
            string str6 = str2;
            if (input2.Contains("%"))
            {
              string str7 = input2.Substring(input2.IndexOf("{%"));
              str4 = str7.Substring(0, str7.IndexOf("."));
            }
            tupleList.Add(new Tuple<string, string, string, string>(str3, str4, str5, str6));
            int length = input2.LastIndexOf('.');
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            if (length > 0)
            {
              string str8 = input2.Substring(0, length);
              string input3 = str8;
              int num = str8.LastIndexOf('.');
              if (num > 0)
                input3 = str8.Substring(num + 1);
              if (!input3.Contains("%"))
              {
                string empty3 = string.Empty;
                Match match4 = FieldReplacementRegex.entityRegex.Match(input3);
                if (match4.Groups["type"].Success)
                  empty3 = match4.Groups["type"].Value;
                if (string.IsNullOrEmpty(empty3))
                  empty3 = match4.Groups["name"].Value;
                EntityDescriptor entityDescriptor = EntityDescriptor.Create(empty3);
                bool flag = false;
                Type parentInfo = FieldReplacementRegex.GetParentInfo(entityDescriptor.EntityType);
                if (parentInfo != (Type) null && parentInfo.GetProperty(name) != (PropertyInfo) null)
                  flag = true;
                if (!flag && !FieldReplacementRegex._linqUseWrapperHash.Contains(key))
                  FieldReplacementRegex._linqUseWrapperHash.Add(key);
              }
            }
            if (FieldReplacementRegex._linqObjectDictionary.ContainsKey(key))
            {
              if (str2 != FieldReplacementRegex._linqObjectDictionary[key])
                throw new Exception(string.Format("Duplicate linq object variable {0} already defined for type {1}", (object) key, (object) str2));
            }
            else
              FieldReplacementRegex._linqObjectDictionary.Add(key, str2);
            if (FieldReplacementRegex.linqObjectValueParseRegex.IsMatch(match2.Groups["collection"].Value))
              stringList.Add(match2.Groups["collection"].Value);
          }
        }
        if (!string.IsNullOrEmpty(str1))
          stringList.Add(str1);
      }
      foreach (KeyValuePair<string, string> linqObject in FieldReplacementRegex._linqObjectDictionary)
      {
        string entityType = FieldReplacementRegex.ParseEntityDescriptor(linqObject.Value).EntityType;
        if ((Type) null == FieldReplacementRegex.GetParentInfo(entityType) || FieldReplacementRegex.ContainsIntegrationKeyword(entityType))
        {
          FieldReplacementRegex._linqUseWrapperHash.Add(linqObject.Key);
        }
        else
        {
          foreach (string input in stringList)
          {
            if (input.Contains(linqObject.Key) && FieldReplacementRegex.ContainsIntegrationKeyword(input))
              FieldReplacementRegex._linqUseWrapperHash.Add(linqObject.Key);
          }
        }
      }
      foreach (KeyValuePair<string, string> keyValuePair1 in FieldReplacementRegex._linqObjectDictionary.Reverse<KeyValuePair<string, string>>())
      {
        if (!FieldReplacementRegex._linqUseWrapperHash.Contains(keyValuePair1.Key))
        {
          foreach (string input in stringList)
          {
            if (input.Contains(keyValuePair1.Key) && input.Contains("{{"))
            {
              string itemType = FieldReplacementRegex.collectionRegex.Match(input).Groups["basetype"].Value;
              foreach (KeyValuePair<string, string> keyValuePair2 in FieldReplacementRegex._linqObjectDictionary.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (p => p.Value == itemType)))
              {
                if (!FieldReplacementRegex._linqUseWrapperHash.Contains(keyValuePair2.Key))
                  FieldReplacementRegex._linqUseWrapperHash.Add(keyValuePair1.Key);
              }
            }
          }
        }
      }
      foreach (Tuple<string, string, string, string> tuple in tupleList)
      {
        if (FieldReplacementRegex._linqUseWrapperHash.Contains(tuple.Item1) && !FieldReplacementRegex._linqUseWrapperHash.Contains(tuple.Item2))
          FieldReplacementRegex._linqUseWrapperHash.Add(tuple.Item2);
      }
      for (int index = tupleList.Count - 1; index >= 0; --index)
      {
        if (FieldReplacementRegex._linqUseWrapperHash.Contains(tupleList[index].Item2) && !FieldReplacementRegex._linqUseWrapperHash.Contains(tupleList[index].Item1))
          FieldReplacementRegex._linqUseWrapperHash.Add(tupleList[index].Item1);
      }
    }

    private static Dictionary<string, string> BuildVariableTypeLookupDictionary(string sourceCode)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      for (Match match = FieldReplacementRegex.dimVariableNameTypeRegex.Match(sourceCode); match.Success; match = match.NextMatch())
      {
        string key = match.Groups["varName"].Value;
        string str = match.Groups["varType"].Value;
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, str);
      }
      return dictionary;
    }

    private static void BuildTypeCheckLineLists(string output)
    {
      Dictionary<string, string> dictionary = FieldReplacementRegex.BuildVariableTypeLookupDictionary(output);
      FieldReplacementRegex._nullableBooleanCheckLineList.Clear();
      FieldReplacementRegex._nullableDecimalCheckLineList.Clear();
      FieldReplacementRegex._nullableShortCheckLineList.Clear();
      StringReader stringReader = new StringReader(output);
label_1:
      string input = stringReader.ReadLine();
      if (input == null)
        return;
      using (Dictionary<string, string>.Enumerator enumerator = dictionary.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          string str1 = current.Value;
          string key = current.Key;
          string str2 = Regex.Replace(input, "\\s", "");
          if (str1.Equals("Boolean?", StringComparison.OrdinalIgnoreCase) && (str2.IndexOf(key + "=", StringComparison.OrdinalIgnoreCase) >= 0 || str2.IndexOf(key + "AsBoolean?=", StringComparison.OrdinalIgnoreCase) >= 0))
            FieldReplacementRegex._nullableBooleanCheckLineList.Add(input);
          if (str1.Equals("Decimal?", StringComparison.OrdinalIgnoreCase) && (str2.IndexOf(key + "=", StringComparison.OrdinalIgnoreCase) >= 0 || str2.IndexOf(key + "AsDecimal?=", StringComparison.OrdinalIgnoreCase) >= 0))
            FieldReplacementRegex._nullableDecimalCheckLineList.Add(input);
          if (str1.Equals("Short?", StringComparison.OrdinalIgnoreCase) && (str2.IndexOf(key + "=", StringComparison.OrdinalIgnoreCase) >= 0 || str2.IndexOf(key + "AsShort?=", StringComparison.OrdinalIgnoreCase) >= 0))
            FieldReplacementRegex._nullableShortCheckLineList.Add(input);
          if (str1.Equals("Object", StringComparison.OrdinalIgnoreCase) && (str2.IndexOf(key + "=", StringComparison.OrdinalIgnoreCase) >= 0 || str2.IndexOf(key + "AsObject=", StringComparison.OrdinalIgnoreCase) >= 0))
            FieldReplacementRegex._nullableShortCheckLineList.Add(input);
        }
        goto label_1;
      }
    }

    private static string FormatLinqSelectDatasources(
      string soureCode,
      EntityDescriptor parentDesc,
      string contextRoot,
      string calcFieldId,
      bool useWrappersForAll)
    {
      foreach (string oldValue in FieldReplacementRegex._linqInStatementlist)
      {
        string str1 = oldValue;
        int num = str1.IndexOf(" In ", StringComparison.OrdinalIgnoreCase);
        string str2 = str1.Substring(0, num + 4);
        string fullPathString = str1.Substring(num + 4);
        ReferencedElement referencedElements = FieldReplacementRegex.ParseReferencedElements(fullPathString.Replace("%", "obj"), parentDesc.EntityType, false);
        string key = FieldReplacementRegex.linqObjectValueParseRegex.Match(str1).Groups["linqObject"].Value;
        Type parentInfo = FieldReplacementRegex.GetParentInfo(FieldReplacementRegex.ParseEntityDescriptor(FieldReplacementRegex._linqObjectDictionary[key]).EntityType);
        bool flag = FieldReplacementRegex.UseDataEntityWrapperFormat(str1, parentInfo);
        if (parentDesc.EntityType == "FeeVariance")
          flag = true;
        string empty = string.Empty;
        string inputString;
        if (flag | useWrappersForAll)
        {
          inputString = FieldReplacementRegex.FormatDataEntityWrapperFieldReference(fullPathString, parentDesc.EntityType);
        }
        else
        {
          inputString = FieldReplacementRegex.FormatFieldReferencePaths(referencedElements, contextRoot);
          if (referencedElements.FieldElement != null)
            inputString = FieldReplacementRegex.FormatFieldTypes(inputString, parentInfo, referencedElements.FieldElement.Name, calcFieldId);
        }
        soureCode = soureCode.Replace(oldValue, str2 + inputString);
      }
      return soureCode;
    }

    private static string FormatFieldReferencePaths(
      ReferencedElement refElement,
      string contextRoot)
    {
      string str = string.Empty;
      for (ReferencedElement referencedElement = refElement; referencedElement != null; referencedElement = referencedElement.ReferencedElements.FirstOrDefault<ReferencedElement>())
      {
        if (!string.IsNullOrEmpty(referencedElement.Name))
        {
          if (referencedElement.Name == CalculationUtility.GetRootRelationship())
          {
            str = CalculationUtility.GetObjectModelRoot() + ".";
          }
          else
          {
            if (string.IsNullOrEmpty(str) && !referencedElement.Name.StartsWith("obj"))
              str = contextRoot + ".";
            str = !referencedElement.Name.StartsWith("obj") ? str + referencedElement.Name + "." : referencedElement.Name + ".";
            if (referencedElement.DataElementType == DataElementType.DataEntityCollectionType)
              str = str.TrimEnd('.') + string.Format(".Cast(Of {0}.{1})", (object) CalculationUtility.GetObjectModelNamespace(), (object) referencedElement.EntityType.EntityType);
            if (referencedElement.EntityType != (EntityDescriptor) null && referencedElement.EntityType.EntityParameters.Any<EntityParameter>())
              str += FieldReplacementRegex.GetParameterizedWhereClause(referencedElement.EntityType);
          }
        }
      }
      return str.TrimEnd('.');
    }

    private static string FormatObjectModelFieldReference(
      string sourceCode,
      EntityDescriptor parentDesc,
      string contextRoot,
      string calcFieldId,
      bool useWrappersForAll)
    {
      Regex regex = new Regex("[\\{\\[][^\\]\\s]+[\\]\\}\\.]");
      string input = sourceCode;
      for (Match match = regex.Match(input); match.Success; match = match.NextMatch())
      {
        string str1 = match.Groups[0].Value;
        ReferencedElement refElement = (ReferencedElement) null;
        Type parentTypeInfo = (Type) null;
        if (!str1.EndsWith("."))
        {
          refElement = FieldReplacementRegex.ParseReferencedElements(str1.Replace("%", "obj"), parentDesc.EntityType, false);
          parentTypeInfo = FieldReplacementRegex.GetParentType(refElement, parentDesc.EntityType);
        }
        bool flag = FieldReplacementRegex.UseDataEntityWrapperFormat(str1, parentTypeInfo);
        if (parentDesc.EntityType == "FeeVariance")
          flag = true;
        string empty = string.Empty;
        string str2;
        if (flag | useWrappersForAll)
        {
          str2 = FieldReplacementRegex.FormatDataEntityWrapperFieldReference(str1, parentDesc.EntityType);
        }
        else
        {
          str2 = FieldReplacementRegex.FormatFieldReferencePaths(refElement, contextRoot);
          if (refElement.FieldElement != null)
            str2 = FieldReplacementRegex.FormatFieldTypes(str2, parentTypeInfo, refElement.FieldElement.Name, calcFieldId);
        }
        input = regex.Replace(input, str2, 1);
      }
      return input;
    }

    private static string FormatDataEntityWrapperFieldReference(
      string fullPathString,
      string parentBaseType)
    {
      string input1 = fullPathString.Replace("^T_", "^T.");
      string input2 = FieldReplacementRegex.linqObjectValueParseRegex.Replace(input1, "obj${linqValue}");
      string input3 = FieldReplacementRegex.propertyRegex.Replace(input2, "${prefix}" + parentBaseType + "Entity.${prop}");
      string input4 = FieldReplacementRegex.weakPropertyRegex.Replace(input3, "${prefix}" + parentBaseType + "Entity.${prop}");
      string input5 = FieldReplacementRegex.propertyFirstInLineRegex.Replace(input4, parentBaseType + "Entity.${prop}");
      string input6 = FieldReplacementRegex.transientFieldRegex.Replace(input5, "GetFieldValue(\"${field}\")");
      string input7 = FieldReplacementRegex.fieldRegex.Replace(input6, "${field}");
      string input8 = FieldReplacementRegex.entityNoTypeRegex.Replace(input7, "GetRelatedWrappedEntity(Of ${name}EntityWrapper)(\"${name}\")");
      string input9 = FieldReplacementRegex.entityRegex.Replace(input8, "GetRelatedWrappedEntity(Of ${type}EntityWrapper)(\"${name}\")");
      string input10 = FieldReplacementRegex.collectionNoTypeRegex.Replace(input9, "GetRelatedWrappedEntities(Of ${name}EntityWrapper)(\"${name}\", \"${name}\")");
      return FieldReplacementRegex.collectionRegex.Replace(input10, "GetRelatedWrappedEntities(Of ${basetype}EntityWrapper)(\"${name}\", \"${type}\")");
    }

    private static bool IsPropertyTypeNullable(PropertyInfo propertyInfo)
    {
      bool flag = false;
      if (propertyInfo != (PropertyInfo) null)
      {
        Type propertyType = propertyInfo.PropertyType;
        flag = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
      }
      return flag;
    }

    private static bool ZeroIsNotNull(PropertyInfo propertyInfo)
    {
      bool flag = false;
      if (propertyInfo != (PropertyInfo) null)
      {
        foreach (CustomAttributeData customAttributeData in (IEnumerable<CustomAttributeData>) propertyInfo.GetCustomAttributesData())
        {
          if (customAttributeData.AttributeType.Name == "ZeroIsNotNullAttribute")
            flag = true;
        }
      }
      return flag;
    }

    private static string FormatFieldTypes(
      string inputString,
      Type parentTypeInfo,
      string fieldName,
      string calcFieldId)
    {
      string str1 = inputString;
      if (parentTypeInfo != (Type) null)
      {
        PropertyInfo property = parentTypeInfo.GetProperty(fieldName);
        if (property != (PropertyInfo) null)
        {
          Type propertyType = property.PropertyType;
          if (FieldReplacementRegex.IsPropertyTypeNullable(property))
          {
            if (!FieldReplacementRegex.ZeroIsNotNull(property))
            {
              switch (propertyType.GetGenericArguments()[0].Name)
              {
                case "Boolean":
                  if (!FieldReplacementRegex._nullableBooleanCheckLineList.Any<string>((Func<string, bool>) (p => p.Contains("[" + fieldName + "]"))))
                  {
                    str1 = "CType(" + str1 + ", Object)";
                    break;
                  }
                  break;
                case "Byte":
                  if (!FieldReplacementRegex._nullableShortCheckLineList.Any<string>((Func<string, bool>) (p => p.Contains("[" + fieldName + "]"))))
                  {
                    str1 = "XByte(" + str1 + ")";
                    break;
                  }
                  break;
                case "DateTime":
                  str1 = "CType(" + str1 + ", Object)";
                  break;
                case "Decimal":
                  if (!FieldReplacementRegex._nullableDecimalCheckLineList.Any<string>((Func<string, bool>) (p => p.Contains("[" + fieldName + "]"))))
                  {
                    str1 = "XDec(" + str1 + ")";
                    break;
                  }
                  break;
                case "Int16":
                case "Int32":
                  if (!FieldReplacementRegex._nullableShortCheckLineList.Any<string>((Func<string, bool>) (p => p.Contains("[" + fieldName + "]"))))
                  {
                    str1 = "XInt(" + str1 + ")";
                    break;
                  }
                  break;
                case "Short":
                  if (!FieldReplacementRegex._nullableShortCheckLineList.Any<string>((Func<string, bool>) (p => p.Contains("[" + fieldName + "]"))))
                  {
                    str1 = "XShort(" + str1 + ")";
                    break;
                  }
                  break;
              }
            }
          }
          else if (propertyType.Name == "String")
            str1 = "XString(" + str1 + ")";
          else if (propertyType.Name == "NA`1")
          {
            string str2 = str1.Substring(str1.IndexOf(".") + 1);
            if (calcFieldId == str2)
              str1 = "GetFieldValue(\"" + str2 + "\")";
          }
          else if (propertyType.BaseType.Name == "Enum")
            str1 += ".ToString()";
        }
      }
      return str1;
    }

    private static bool ContainsIntegrationKeyword(string input)
    {
      return input.Contains("^") || input.Contains("IsLocked(") || input.Contains("GetField(") || input.Contains("IsNull(") || input.Contains("InvokeMethod");
    }

    private static bool UseDataEntityWrapperFormat(string fullPathSource, Type parentTypeInfo)
    {
      bool flag1 = true;
      if (parentTypeInfo != (Type) null)
      {
        int num1 = FieldReplacementRegex.ContainsIntegrationKeyword(fullPathSource) ? 1 : 0;
        string str = FieldReplacementRegex.linqObjectValueParseRegex.Match(fullPathSource).Groups["linqObject"].Value;
        bool flag2 = FieldReplacementRegex._linqUseWrapperHash.Contains(str);
        bool flag3 = fullPathSource.EndsWith(".");
        int num2 = flag2 ? 1 : 0;
        flag1 = (num1 | num2 | (flag3 ? 1 : 0)) != 0;
      }
      return flag1;
    }

    private static string GetParameterizedWhereClause(EntityDescriptor entityType)
    {
      string parameterizedWhereClause = string.Empty;
      try
      {
        parameterizedWhereClause = ".Where(Function(p) ";
        bool flag = true;
        List<EntityParameter>.Enumerator enumerator = entityType.EntityParameters.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if (flag)
            flag = false;
          else
            parameterizedWhereClause += " AndAlso ";
          EntityParameter current1 = enumerator.Current;
          if (current1.ParameterName.Equals(FieldReplacementRegex.Operator) && current1.ParameterValue.Equals(FieldReplacementRegex.OperatorRange))
          {
            enumerator.MoveNext();
            Decimal num1 = Convert.ToDecimal(enumerator.Current.ParameterValue);
            enumerator.MoveNext();
            EntityParameter current2 = enumerator.Current;
            Decimal num2 = Convert.ToDecimal(current2.ParameterValue);
            parameterizedWhereClause += string.Format("p.{0} >= {1} AndAlso p.{0} <= {2}", (object) current2.ParameterName, (object) num1, (object) num2);
            break;
          }
          if (current1.ParameterName.Equals(FieldReplacementRegex.Operator) && current1.ParameterValue.Equals(FieldReplacementRegex.OperatorLineRange))
          {
            enumerator.MoveNext();
            Decimal num3 = Convert.ToDecimal(enumerator.Current.ParameterValue);
            enumerator.MoveNext();
            EntityParameter current3 = enumerator.Current;
            Decimal num4 = Convert.ToDecimal(current3.ParameterValue);
            parameterizedWhereClause += string.Format("XInt(p.{0}.ToString().Replace(\"Line\",\"\")) >= {1} AndAlso XInt(p.{0}.ToString().Replace(\"Line\",\"\")) <= {2}", (object) current3.ParameterName, (object) num3, (object) num4);
            break;
          }
          Type parentInfo = FieldReplacementRegex.GetParentInfo(entityType.EntityType);
          string parameterName = current1.ParameterName;
          string str1 = "p." + current1.ParameterName;
          string str2 = current1.ParameterValue;
          if (parentInfo != (Type) null)
          {
            PropertyInfo property = parentInfo.GetProperty(parameterName);
            if (property != (PropertyInfo) null)
            {
              Type propertyType = property.PropertyType;
              if (FieldReplacementRegex.IsPropertyTypeNullable(property) && !FieldReplacementRegex.ZeroIsNotNull(property))
              {
                if (propertyType.GetGenericArguments()[0].Name == "Decimal")
                  str1 = "XDec(" + str1 + ")";
              }
              else if (propertyType.Name == "String")
              {
                str1 = "XString(" + str1 + ")";
                str2 = "\"" + current1.ParameterValue + "\"";
              }
              else if (propertyType.BaseType.Name == "Enum")
                str2 = propertyType.FullName + "." + current1.ParameterValue;
            }
          }
          parameterizedWhereClause += string.Format("{0} = {1}", (object) str1, (object) str2);
        }
        parameterizedWhereClause += ")";
      }
      catch (Exception ex)
      {
        string message = ex.Message;
      }
      return parameterizedWhereClause;
    }

    private static string RenameVariablesWithFieldPrefix(string sourceCode)
    {
      string input = sourceCode;
      for (Match match = FieldReplacementRegex.dimVariableNameRegex.Match(input); match.Success; match = match.NextMatch())
      {
        string varName = match.Groups["varName"].Value;
        if (!string.IsNullOrEmpty(varName))
          input = new Regex(string.Format("\\\"[^\\\"]*\\\"|((?i)(?<![a-zA-Z0-9\\.\\\"_]){0}\\b)", (object) varName)).Replace(input, (MatchEvaluator) (m1 => m1.Groups[1].Value == "" ? m1.Value : FieldReplacementRegex.FieldPrefix + varName));
      }
      return input;
    }

    private static string ReplaceEnumTypeComparisons(string aLine)
    {
      string input = aLine;
      if (FieldReplacementRegex._objectModelAssembly != (Assembly) null)
      {
        input = Regex.Replace(input, "\\s+=\\s+", "=");
        try
        {
          foreach (Type enumType in ((IEnumerable<Type>) FieldReplacementRegex._objectModelAssembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => t.IsEnum && t.IsPublic && t.Namespace == CalculationUtility.GetObjectModelNamespace())))
          {
            foreach (object obj in Enum.GetValues(enumType))
            {
              string oldValue = string.Format("{0}.ToString()=\"{1}\"", (object) enumType.Name, obj);
              string newValue = string.Format("{0}={1}.{0}.{2}", (object) enumType.Name, (object) CalculationUtility.GetObjectModelNamespace(), obj);
              input = input.Replace(oldValue, newValue);
            }
          }
        }
        catch (Exception ex)
        {
          string message = ex.Message;
          if (ex is ReflectionTypeLoadException)
          {
            foreach (Exception loaderException in (ex as ReflectionTypeLoadException).LoaderExceptions)
              message += loaderException.Message;
          }
          throw new Exception(message, ex.InnerException);
        }
      }
      if (FieldReplacementRegex._objectModelAssembly != (Assembly) null)
      {
        input = Regex.Replace(input, "\\s+=\\s+", "=");
        foreach (Type type in ((IEnumerable<Type>) FieldReplacementRegex._objectModelAssembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => t.IsEnum && t.IsPublic && t.Namespace == CalculationUtility.GetObjectModelNamespace())))
        {
          string oldValue = string.Format("{0}.ToString()", (object) type.Name);
          string newValue = string.Format("{0}.FastToString()", (object) type.Name);
          input = input.Replace(oldValue, newValue);
        }
      }
      return input;
    }
  }
}
