// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationUtility
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.Configuration;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public static class CalculationUtility
  {
    private const string className = "CalculationUtility";
    private static string rootEntityType = string.Empty;
    private static string rootRelationship = string.Empty;
    private static string objectModelRoot = string.Empty;
    private static string objectModelNamespace = string.Empty;
    private static List<EntityDescriptor> entityList = new List<EntityDescriptor>();
    public static string DEFAULT_ROOT_TYPE = "DefaultRootType";
    public static string DEFAULT_ROOT_RELATIONSHIP = "DefaultRelationship";
    public static string ALL_ENTITIES = "*";
    public static string SETTINGS = "Settings";
    public static ConcurrentDictionary<Type, Dictionary<int, string>> EnumStringLookupDictionary = new ConcurrentDictionary<Type, Dictionary<int, string>>();
    public static ConcurrentDictionary<int, IEnumerable<object>> _queryDictionary = new ConcurrentDictionary<int, IEnumerable<object>>();
    private static Dictionary<string, FieldDescriptor> modelHash = new Dictionary<string, FieldDescriptor>();
    private static XElement root;
    private static List<string> duplicates = new List<string>();

    public static object ConvertValueType(object value, Elli.CalculationEngine.Common.ValueType type)
    {
      object obj = value;
      try
      {
        switch (type)
        {
          case Elli.CalculationEngine.Common.ValueType.Integer:
            obj = (object) Convert.ToInt32(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.Decimal:
            obj = (object) Convert.ToDecimal(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.String:
            obj = (object) Convert.ToString(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.Date:
          case Elli.CalculationEngine.Common.ValueType.DateTime:
            obj = (object) Convert.ToDateTime(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.Boolean:
            obj = (object) Convert.ToBoolean(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.NullableBoolean:
            if (value != null)
            {
              obj = (object) Convert.ToBoolean(value);
              break;
            }
            break;
          case Elli.CalculationEngine.Common.ValueType.NullableDecimal:
            if (value != null)
            {
              obj = (object) Convert.ToDecimal(value);
              break;
            }
            break;
          case Elli.CalculationEngine.Common.ValueType.NullableInteger:
            if (value != null)
            {
              obj = (object) Convert.ToInt32(value);
              break;
            }
            break;
          case Elli.CalculationEngine.Common.ValueType.NullableDate:
          case Elli.CalculationEngine.Common.ValueType.NullableDateTime:
            if (value != null)
            {
              obj = (object) Convert.ToDateTime(value);
              break;
            }
            break;
          case Elli.CalculationEngine.Common.ValueType.Short:
            obj = (object) Convert.ToInt16(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.Byte:
            obj = (object) Convert.ToByte(value);
            break;
          case Elli.CalculationEngine.Common.ValueType.NullableShort:
            if (value != null)
            {
              obj = (object) Convert.ToInt16(value);
              break;
            }
            break;
          default:
            obj = value;
            break;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error converting value \"{0}\" to type {1})", value, (object) type.ToString()), ex.InnerException);
      }
      return obj;
    }

    public static string BuildFullyQualifiedName(string relationship, string fieldId)
    {
      if (!string.IsNullOrEmpty(relationship) && relationship.EndsWith("]"))
        relationship = relationship.Substring(0, relationship.IndexOf(".["));
      return CalculationUtility.BuildFullyQualifiedName(relationship, fieldId, string.Empty, string.Empty);
    }

    public static string BuildFullyQualifiedName(
      string relationship,
      string fieldId,
      string parentRelationship,
      string parentEntityType)
    {
      if (string.IsNullOrEmpty(relationship))
      {
        if (!string.IsNullOrEmpty(parentEntityType))
          parentRelationship = string.Format("{0}:{1}", (object) parentRelationship, (object) parentEntityType);
        relationship = string.Format("{{{0}}}", (object) parentRelationship);
      }
      return string.Format("{0}.[{1}]", (object) relationship, (object) fieldId);
    }

    public static CalculationSetElement CreateCalculationSetElement(
      CalculationSetElement calculationElement,
      Expression expression,
      bool transient,
      Guid parentId,
      bool multiLine = false,
      string parentEntityType = "")
    {
      switch (calculationElement.Identity.Type)
      {
        case LibraryElementType.FieldExpressionCalculation:
          return (CalculationSetElement) new FieldExpressionCalculation(calculationElement.Identity.Id, calculationElement.Name, calculationElement.DescriptiveName, calculationElement.Identity.Description, transient, expression.Text, parentId, calculationElement.Enabled, parentEntityType, expression.ReturnType, multiLine, calculationElement.CalculationTests);
        case LibraryElementType.CalculationTemplate:
          return (CalculationSetElement) new CalculationTemplate(calculationElement.Identity.Id, calculationElement.Name, calculationElement.DescriptiveName, calculationElement.Identity.Description, expression.Text, parentId, calculationElement.Enabled, expression.ReturnType, calculationElement.CalculationTests);
        case LibraryElementType.Function:
          return (CalculationSetElement) new Function(calculationElement.Identity.Id, calculationElement.DescriptiveName, calculationElement.Identity.Description, expression.Text, parentId, calculationElement.Enabled, expression.ReturnType, calculationElement.CalculationTests);
        case LibraryElementType.TransientDataObject:
          return (CalculationSetElement) new TransientDataObject(calculationElement.Identity.Id, calculationElement.DescriptiveName, calculationElement.Identity.Description, expression.Text, parentId, calculationElement.Enabled, expression.ReturnType, calculationElement.CalculationTests);
        default:
          return new CalculationSetElement();
      }
    }

    public static Expression GetExpression(CalculationSetElement calculationElement)
    {
      switch (calculationElement.Identity.Type)
      {
        case LibraryElementType.FieldExpressionCalculation:
          return (Expression) ((FieldExpressionCalculation) calculationElement).Expression;
        case LibraryElementType.CalculationTemplate:
          return (Expression) ((CalculationTemplate) calculationElement).Expression;
        case LibraryElementType.Function:
          return (Expression) ((Function) calculationElement).Expression;
        case LibraryElementType.TransientDataObject:
          return (Expression) ((TransientDataObject) calculationElement).Expression;
        default:
          return new Expression();
      }
    }

    public static string GetFunctionName(string expressionText)
    {
      return FunctionReplacementRegex.FunctionName(expressionText);
    }

    public static Elli.CalculationEngine.Common.ValueType GetFunctionReturnType(
      string expressionText)
    {
      try
      {
        return (Elli.CalculationEngine.Common.ValueType) Enum.Parse(typeof (Elli.CalculationEngine.Common.ValueType), FunctionReplacementRegex.ReturnType(expressionText));
      }
      catch
      {
        string message = string.Format("'{0}' is an invalid Value Type. Please contact administration if this is a needed Value Type.", (object) FunctionReplacementRegex.ReturnType(expressionText));
        Tracing.Log(TraceLevel.Error, nameof (CalculationUtility), string.Format("VerifyFunctionsEnabled: {0}", (object) message));
        throw new Exception(message);
      }
    }

    public static string GetTransientDataObjectName(string expressionText)
    {
      return TransientDataObjectReplacementRegex.TransientDataObjectName(expressionText);
    }

    public static string GetRootEntityType()
    {
      return !string.IsNullOrEmpty(CalculationUtility.rootEntityType) ? CalculationUtility.rootEntityType : CalculationUtility.DEFAULT_ROOT_TYPE;
    }

    public static string GetRootRelationship()
    {
      return !string.IsNullOrEmpty(CalculationUtility.rootRelationship) ? CalculationUtility.rootRelationship : CalculationUtility.DEFAULT_ROOT_RELATIONSHIP;
    }

    public static string GetObjectModelRoot() => CalculationUtility.objectModelRoot;

    public static string GetObjectModelNamespace() => CalculationUtility.objectModelNamespace;

    public static bool IsMultiLineCalculation(CalculationSetElement calculationElement)
    {
      return calculationElement.Identity.Type == LibraryElementType.FieldExpressionCalculation && ((FieldExpressionCalculation) calculationElement).IsMultiLineCalculation;
    }

    public static bool IsTransient(CalculationSetElement calculationElement)
    {
      return calculationElement.Identity.Type == LibraryElementType.FieldExpressionCalculation && ((FieldExpressionCalculation) calculationElement).IsTransient;
    }

    public static void SetRootEntityType(string entityType)
    {
      CalculationUtility.rootEntityType = entityType;
    }

    public static void SetRootRelationship(string relationship)
    {
      CalculationUtility.rootRelationship = relationship;
    }

    public static void SetObjectModelRoot(string root) => CalculationUtility.objectModelRoot = root;

    public static void SetObjectModelNamespace(string namespaceName)
    {
      CalculationUtility.objectModelNamespace = namespaceName;
    }

    public static void SetEntityTypeList(List<EntityDescriptor> list)
    {
      CalculationUtility.entityList = list;
    }

    public static List<EntityDescriptor> GetEntityTypeList() => CalculationUtility.entityList;

    public static void Compress(string filePath)
    {
      FileInfo fileInfo = new FileInfo(filePath);
      using (FileStream fileStream1 = fileInfo.OpenRead())
      {
        if (!((File.GetAttributes(fileInfo.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileInfo.Extension != ".gz"))
          return;
        using (FileStream fileStream2 = File.Create(fileInfo.FullName + ".gz"))
        {
          using (GZipStream destination = new GZipStream((Stream) fileStream2, CompressionMode.Compress))
            fileStream1.CopyTo((Stream) destination);
        }
      }
    }

    public static void Decompress(string filePath)
    {
      FileInfo fileInfo = new FileInfo(filePath);
      using (FileStream fileStream = fileInfo.OpenRead())
      {
        string fullName = fileInfo.FullName;
        using (FileStream destination = File.Create(fullName.Remove(fullName.Length - fileInfo.Extension.Length)))
        {
          using (GZipStream gzipStream = new GZipStream((Stream) fileStream, CompressionMode.Decompress))
            gzipStream.CopyTo((Stream) destination);
        }
      }
    }

    public static object UniqueOrDefault(this IEnumerable<DataEntityWrapper> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? (object) source.SingleOrDefault<DataEntityWrapper>() : (object) source.FirstOrDefault<DataEntityWrapper>();
    }

    public static object UniqueOrDefault(this IEnumerable<object> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<object>() : source.FirstOrDefault<object>();
    }

    public static DateTime UniqueOrDefault(this IEnumerable<DateTime> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<DateTime>() : source.FirstOrDefault<DateTime>();
    }

    public static DateTime? UniqueOrDefault(this IEnumerable<DateTime?> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<DateTime?>() : source.FirstOrDefault<DateTime?>();
    }

    public static Decimal UniqueOrDefault(this IEnumerable<Decimal?> source)
    {
      Decimal? nullable1 = new Decimal?(0M);
      Decimal? nullable2 = CalculationEngineConfiguration.CurrentConfiguration == null || CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting != UniqueOrDefaultSettingType.SingleOrDefault ? source.FirstOrDefault<Decimal?>() : source.SingleOrDefault<Decimal?>();
      return nullable2.HasValue ? Convert.ToDecimal((object) nullable2) : 0M;
    }

    public static Decimal? UniqueOrNull(this IEnumerable<Decimal?> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<Decimal?>() : source.FirstOrDefault<Decimal?>();
    }

    public static Decimal UniqueOrDefault(this IEnumerable<Decimal> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<Decimal>() : source.FirstOrDefault<Decimal>();
    }

    public static string UniqueOrDefault(this IEnumerable<string> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<string>() : source.FirstOrDefault<string>();
    }

    public static bool? UniqueOrDefault(this IEnumerable<bool?> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<bool?>() : source.FirstOrDefault<bool?>();
    }

    public static short UniqueOrDefault(this IEnumerable<short> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<short>() : source.FirstOrDefault<short>();
    }

    public static short UniqueOrDefault(this IEnumerable<short?> source)
    {
      short? nullable1 = new short?((short) 0);
      short? nullable2 = CalculationEngineConfiguration.CurrentConfiguration == null || CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting != UniqueOrDefaultSettingType.SingleOrDefault ? source.FirstOrDefault<short?>() : source.SingleOrDefault<short?>();
      return nullable2.HasValue ? Convert.ToInt16((object) nullable2) : (short) 0;
    }

    public static int UniqueOrDefault(this IEnumerable<int> source)
    {
      return CalculationEngineConfiguration.CurrentConfiguration != null && CalculationEngineConfiguration.CurrentConfiguration.UniqueOrDefaultSetting == UniqueOrDefaultSettingType.SingleOrDefault ? source.SingleOrDefault<int>() : source.FirstOrDefault<int>();
    }

    private static string ConvertEnumToString(Type enumType, int value)
    {
      string empty = string.Empty;
      Dictionary<int, string> dictionary1 = (Dictionary<int, string>) null;
      if (!CalculationUtility.EnumStringLookupDictionary.TryGetValue(enumType, out dictionary1))
      {
        Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
        foreach (object enumValue in enumType.GetEnumValues())
          dictionary2.Add(Convert.ToInt32(enumValue), enumValue.ToString());
        dictionary1 = CalculationUtility.EnumStringLookupDictionary.GetOrAdd(enumType, dictionary2);
      }
      dictionary1?.TryGetValue(value, out empty);
      return empty;
    }

    public static string FastToString(this Enum value)
    {
      return CalculationUtility.ConvertEnumToString(value.GetType(), Convert.ToInt32((object) value));
    }

    public static IEnumerable<TSource> GetCachedQuery<TSource>(
      this IEnumerable<TSource> source,
      int key,
      Func<TSource, bool> predicate)
    {
      IEnumerable<TSource> source1;
      if (CalculationUtility._queryDictionary.ContainsKey(key))
      {
        source1 = CalculationUtility._queryDictionary[key].Cast<TSource>();
      }
      else
      {
        source1 = source.Where<TSource>(predicate);
        CalculationUtility._queryDictionary.TryAdd(key, source1.Cast<object>());
      }
      return source1;
    }

    public static void InitEncompassFieldsLookup(string encompassFieldsPath)
    {
      CalculationUtility.root = XElement.Load(encompassFieldsPath);
      foreach (XElement element in CalculationUtility.root.Elements((XName) "Field"))
      {
        XElement modelElement = element.Element((XName) "ModelPath");
        if (modelElement != null && !string.IsNullOrWhiteSpace(modelElement.Value))
        {
          if (!CalculationUtility.modelHash.ContainsKey(modelElement.Value))
          {
            CalculationUtility.modelHash.Add(modelElement.Value, CalculationUtility.ConvertEncompassFieldModelPathToFieldDescriptor(modelElement.Value));
          }
          else
          {
            IEnumerable<XElement> xelements = CalculationUtility.root.Elements((XName) "Field").Where<XElement>((Func<XElement, bool>) (el => el.Element((XName) "ModelPath") != null && el.Element((XName) "ModelPath").Value.Contains(modelElement.Value)));
            string empty = string.Empty;
            foreach (XElement xelement in xelements)
            {
              if (string.IsNullOrEmpty(empty))
                empty = xelement.Element((XName) "XPath").Value;
              if (xelement.Element((XName) "XPath").Value != empty)
                CalculationUtility.duplicates.Add(modelElement.Value);
            }
          }
        }
      }
    }

    public static FieldDescriptor ConvertEncompassFieldModelPathToFieldDescriptor(string modelPath)
    {
      if (string.IsNullOrWhiteSpace(modelPath))
        return (FieldDescriptor) null;
      string[] source1 = modelPath.Trim().Split('.');
      string fieldId = ((IEnumerable<string>) source1).Last<string>();
      List<EntityParameter> parameters = (List<EntityParameter>) null;
      string str1 = ((IEnumerable<string>) source1).ElementAt<string>(((IEnumerable<string>) source1).Count<string>() - 2);
      string type;
      if (str1.Contains("["))
      {
        string[] source2 = str1.Replace("[%]", "").Split('[');
        type = ((IEnumerable<string>) source2).First<string>().TrimEnd('s');
        char[] chArray = new char[4]{ '[', ']', '(', ')' };
        string s = ((IEnumerable<string>) source2).Last<string>().Trim(chArray);
        if (!int.TryParse(s, out int _))
        {
          string[] strArray = s.Split(new string[1]
          {
            " && "
          }, StringSplitOptions.None);
          parameters = new List<EntityParameter>();
          foreach (string str2 in strArray)
          {
            string[] separator = new string[1]{ " == " };
            string[] source3 = str2.Split(separator, StringSplitOptions.None);
            parameters.Add(new EntityParameter(((IEnumerable<string>) source3).First<string>(), ((IEnumerable<string>) source3).Last<string>().Trim('\'')));
          }
        }
      }
      else
        type = str1;
      if (type == "CurrentApplication")
        type = "Application";
      if (type == "LOCompensation")
        type = "ElliLOCompensation";
      if (type == "Eem")
        type = "EnergyEfficientMortgage";
      if (type == "EnergyEfficientMortgageItem")
        type = "EnergyEfficientMortgage";
      if (type == "ReoPropertie")
        type = "ReoProperty";
      if (type == "Borrower" && fieldId == "NoCoApplicantIndicator")
        type = "CoBorrower";
      return FieldDescriptor.Create(EntityDescriptor.Create(type, (IEnumerable<EntityParameter>) parameters), fieldId);
    }

    public static List<string> GetFieldID(EntityDescriptor parent, string calcName)
    {
      string fieldId = calcName;
      if (fieldId == "LoCompensationLenderTotalPaidOriginatorAmountForGFE" || fieldId == "LoCompensationLenderTotalPaidOriginatorAmountForLOTool")
        fieldId = "LoCompensationLenderTotalPaidOriginatorAmount";
      return CalculationUtility.GetFieldID(FieldDescriptor.Create(parent, fieldId));
    }

    public static List<string> GetXPath(FieldDescriptor modelPath)
    {
      List<string> xpath = new List<string>();
      if (modelPath != (FieldDescriptor) null)
      {
        foreach (XElement matchingElement in CalculationUtility.GetMatchingElements(modelPath))
        {
          string str = string.Empty;
          if (matchingElement.Element((XName) "XPath") != null)
            str = str + matchingElement.Element((XName) "XPath").Value + "\t";
          xpath.Add(str);
        }
      }
      return xpath;
    }

    public static List<string> GetFieldID(FieldDescriptor modelPath)
    {
      List<string> fieldId = new List<string>();
      if (modelPath != (FieldDescriptor) null)
      {
        foreach (XElement matchingElement in CalculationUtility.GetMatchingElements(modelPath))
        {
          string str = string.Empty;
          if (matchingElement.Attribute((XName) "ID") != null)
            str = str + matchingElement.Attribute((XName) "ID").Value + "\t";
          fieldId.Add(str);
        }
      }
      return fieldId;
    }

    private static List<XElement> GetMatchingElements(FieldDescriptor modelPath)
    {
      List<XElement> matchingElements = new List<XElement>();
      IEnumerable<KeyValuePair<string, FieldDescriptor>> source1;
      if (modelPath.ParentEntityType.IsBaseType())
      {
        source1 = CalculationUtility.modelHash.Where<KeyValuePair<string, FieldDescriptor>>((Func<KeyValuePair<string, FieldDescriptor>, bool>) (x => x.Value.ParentEntityType.IsA(modelPath.ParentEntityType) && x.Value.FieldId == modelPath.FieldId));
        if (source1 == null || !source1.Any<KeyValuePair<string, FieldDescriptor>>())
          return matchingElements;
      }
      else
      {
        source1 = CalculationUtility.modelHash.Where<KeyValuePair<string, FieldDescriptor>>((Func<KeyValuePair<string, FieldDescriptor>, bool>) (x => x.Value == modelPath));
        if (source1 == null || !source1.Any<KeyValuePair<string, FieldDescriptor>>())
        {
          source1 = CalculationUtility.modelHash.Where<KeyValuePair<string, FieldDescriptor>>((Func<KeyValuePair<string, FieldDescriptor>, bool>) (x => x.Value.ParentEntityType.IsA(modelPath.ParentEntityType.GetBaseDescriptor()) && x.Value.FieldId == modelPath.FieldId));
          if (source1 != null && source1.Any<KeyValuePair<string, FieldDescriptor>>())
          {
            foreach (EntityParameter entityParameter in modelPath.ParentEntityType.EntityParameters)
            {
              EntityParameter parameter = entityParameter;
              IEnumerable<KeyValuePair<string, FieldDescriptor>> source2 = source1.Where<KeyValuePair<string, FieldDescriptor>>((Func<KeyValuePair<string, FieldDescriptor>, bool>) (p => p.Value.ParentEntityType.EntityParameters.Contains(parameter)));
              if (source2 != null)
              {
                if (source2.Any<KeyValuePair<string, FieldDescriptor>>())
                  source1 = source2;
                else
                  break;
              }
              else
                break;
            }
          }
          if (source1 == null || !source1.Any<KeyValuePair<string, FieldDescriptor>>())
            return matchingElements;
        }
      }
      foreach (KeyValuePair<string, FieldDescriptor> keyValuePair in source1)
      {
        KeyValuePair<string, FieldDescriptor> searchVal = keyValuePair;
        matchingElements.AddRange(CalculationUtility.root.Elements((XName) "Field").Where<XElement>((Func<XElement, bool>) (el => el.Element((XName) "ModelPath") != null && el.Element((XName) "ModelPath").Value != null && el.Element((XName) "ModelPath").Value.Contains(searchVal.Key))));
      }
      return matchingElements;
    }
  }
}
