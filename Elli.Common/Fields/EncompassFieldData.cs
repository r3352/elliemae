// Decompiled with JetBrains decompiler
// Type: Elli.Common.Fields.EncompassFieldData
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.Common.Fields
{
  public sealed class EncompassFieldData
  {
    private const string StandardCustomFieldPrefix = "CUST�";
    private const string ExtendedCustomFieldPrefix = "CX.�";
    private const string LockRequestAdditionalFieldPrefix = "LR.�";
    private const string CustomFieldModelPathFormat = "Loan.CustomFields[(FieldName == '{0}')].StringValue�";
    private static readonly Regex TemplateFieldRegex = new Regex("^([^\\d]+)((\\d\\d\\d)|(\\d\\d))(.{4}|.{2})", RegexOptions.Compiled);
    private static readonly Regex Tql4506TFieldRegex = new Regex("^(TQL4506T)(\\d\\d)(.+)$", RegexOptions.Compiled);
    private static readonly Regex ContainCharacterRegex = new Regex("^[a-zA-Z]+$", RegexOptions.Compiled);
    private static readonly Dictionary<string, EncompassField> Fields = new Dictionary<string, EncompassField>(15000, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static List<EncompassField> _fieldsWithModelPath;
    private static List<EncompassField> _NonVirtualNonMultiinstanceFieldsWithModelPath;
    private static readonly ConcurrentDictionary<string, List<List<string>>> ModelPathKeyValueCache = new ConcurrentDictionary<string, List<List<string>>>();
    private static IList<string> _rateLockFormatFieldsModelPath = (IList<string>) null;
    private readonly Dictionary<string, EncompassFieldData.RepeatableField> _repeatableFieldMap = new Dictionary<string, EncompassFieldData.RepeatableField>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static readonly EncompassFieldData instance = new EncompassFieldData();

    private EncompassFieldData()
    {
      EncompassFieldReader.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream("Elli.Common.Fields.EncompassFields.xml"), (IDictionary<string, EncompassField>) EncompassFieldData.Fields);
      EncompassFieldData._fieldsWithModelPath = EncompassFieldData.Fields.Values.Where<EncompassField>((Func<EncompassField, bool>) (y => !string.IsNullOrEmpty(y.ModelPath))).ToList<EncompassField>();
      EncompassFieldData._NonVirtualNonMultiinstanceFieldsWithModelPath = EncompassFieldData._fieldsWithModelPath.Where<EncompassField>((Func<EncompassField, bool>) (y => !(y.Category.ToLower() == "none") && !y.MultiInstance)).ToList<EncompassField>();
    }

    public static EncompassFieldData Instance => EncompassFieldData.instance;

    public EncompassField Find(string fieldId)
    {
      EncompassField encompassField = (EncompassField) null;
      return !EncompassFieldData.Fields.TryGetValue(fieldId, out encompassField) ? (EncompassField) null : encompassField;
    }

    public EncompassField Get(string fieldId)
    {
      try
      {
        if (fieldId.Contains("#"))
          fieldId = fieldId.Split('#')[0];
        return EncompassFieldData.Fields[fieldId];
      }
      catch (KeyNotFoundException ex)
      {
        throw new KeyNotFoundException("The Field does not exist for FieldID=" + fieldId);
      }
    }

    public static string ValidateFieldOption(
      string fieldId,
      string optionValue,
      StringComparison stringComparison = StringComparison.InvariantCulture)
    {
      string str = string.Empty;
      try
      {
        EncompassFieldOption encompassFieldOption = EncompassFieldData.Fields[fieldId].Options.FirstOrDefault<EncompassFieldOption>((Func<EncompassFieldOption, bool>) (f => f.Value.Equals(optionValue, stringComparison) || f.Description.Equals(optionValue, stringComparison)));
        if (encompassFieldOption != null)
          str = string.IsNullOrEmpty(encompassFieldOption.Value) ? optionValue : encompassFieldOption.Value;
        else if (fieldId == "AUS.X999")
        {
          if (!(optionValue == "DU"))
          {
            if (!(optionValue == "LP"))
              goto label_8;
          }
          str = optionValue;
        }
      }
      catch (KeyNotFoundException ex)
      {
      }
label_8:
      return str;
    }

    public static IList<EncompassField> GetFieldsWithOptions()
    {
      return (IList<EncompassField>) EncompassFieldData.Fields.Values.Where<EncompassField>((Func<EncompassField, bool>) (p => p.Options.Count > 0)).ToList<EncompassField>();
    }

    public static IList<EncompassField> GetAllFields()
    {
      return (IList<EncompassField>) EncompassFieldData.Fields.Values.ToList<EncompassField>();
    }

    public static IList<EncompassField> GetAllFieldsWithModelPath()
    {
      return (IList<EncompassField>) EncompassFieldData._fieldsWithModelPath;
    }

    public static IList<EncompassField> GetAllNonVirtualNonMultiinstanceFieldsWithModelPath()
    {
      return (IList<EncompassField>) EncompassFieldData._NonVirtualNonMultiinstanceFieldsWithModelPath;
    }

    public IList<string> GetRateLockDecimalFormatFieldsModelPath()
    {
      if (EncompassFieldData._rateLockFormatFieldsModelPath == null)
      {
        EncompassFieldData._rateLockFormatFieldsModelPath = (IList<string>) new List<string>();
        foreach (string rateLockFormatField in LockRequestUtils.RateLockFormatFields)
        {
          string fullModelPath = this.GetFullModelPath(rateLockFormatField);
          if (!string.IsNullOrEmpty(fullModelPath))
            EncompassFieldData._rateLockFormatFieldsModelPath.Add(fullModelPath);
        }
      }
      return EncompassFieldData._rateLockFormatFieldsModelPath;
    }

    public static IList<string> GetFieldsByModelPath(string modelPath)
    {
      return (IList<string>) EncompassFieldData.GetAllFields().Select<EncompassField, string>((Func<EncompassField, string>) (p => p.ModelPath)).Where<string>((Func<string, bool>) (y => !string.IsNullOrEmpty(y) && y.Contains(modelPath))).ToList<string>();
    }

    public static List<List<string>> GetFieldsWithKeyValueByModelPath(string modelPath)
    {
      List<List<string>> valueByModelPath;
      if (EncompassFieldData.ModelPathKeyValueCache.TryGetValue(modelPath, out valueByModelPath))
        return valueByModelPath;
      List<string> list1 = EncompassFieldData.GetAllFields().Select<EncompassField, string>((Func<EncompassField, string>) (p => p.ModelPath)).Where<string>((Func<string, bool>) (y => !string.IsNullOrEmpty(y) && y.Contains(modelPath))).ToList<string>().Select<string, string>((Func<string, string>) (x => Regex.Match(x, "\\(([^)]*)\\)").ToString().Replace("(", "").Replace(")", ""))).ToList<string>();
      HashSet<string> source = new HashSet<string>();
      foreach (string str in list1)
        source.Add(str);
      List<List<string>> list2 = source.Select<string, List<string>>((Func<string, List<string>>) (x => ((IEnumerable<string>) Regex.Split(x, "&&")).SelectMany<string, string>((Func<string, IEnumerable<string>>) (y => (IEnumerable<string>) ((IEnumerable<string>) Regex.Split(y.Replace("'", ""), "==")).Select<string, string>((Func<string, string>) (p => p.Trim())).ToList<string>())).ToList<string>())).ToList<List<string>>();
      EncompassFieldData.ModelPathKeyValueCache.TryAdd(modelPath, list2);
      return list2;
    }

    public string GetFullModelPath(string fieldId, int? numOfLoanApplications = null)
    {
      using (PerformanceMeter.Current.BeginOperation("FieldData.GetFullModelPath"))
      {
        if (string.IsNullOrWhiteSpace(fieldId))
          throw new ArgumentNullException(nameof (fieldId));
        int result = 0;
        if (fieldId.Contains("#"))
        {
          string str = fieldId;
          string[] strArray = fieldId.Split('#');
          if (strArray.Length == 2)
            int.TryParse(strArray[1], out result);
          fieldId = strArray[0];
          if (numOfLoanApplications.HasValue)
          {
            int num = result;
            int? nullable = numOfLoanApplications;
            int valueOrDefault = nullable.GetValueOrDefault();
            if (num > valueOrDefault & nullable.HasValue)
              throw new ArgumentOutOfRangeException("applicationIndex", string.Format("The index {0} referred in fieldId {1} is out of range .", (object) result, (object) str));
          }
        }
        EncompassField encompassField1 = this.Find(fieldId);
        if (encompassField1 != null && encompassField1.HasValidCategory)
        {
          if (result <= 0 || result >= 100)
            return string.IsNullOrEmpty(encompassField1.ModelPath) ? (string) null : encompassField1.ModelPath;
          string newValue = string.Format("Loan.Applications[{0}]", (object) (result - 1));
          return encompassField1.ModelPath.Replace("Loan.CurrentApplication", newValue);
        }
        string fieldIdUpper;
        if (EncompassFieldData.IsCustomField(fieldId, out fieldIdUpper))
          return string.Format("Loan.CustomFields[(FieldName == '{0}')].StringValue", (object) fieldIdUpper);
        EncompassFieldData.RepeatableField repeatableField = this.FindRepeatableField(fieldId);
        if (repeatableField != null)
        {
          EncompassField encompassField2 = this.Find(repeatableField.TemplateFieldId);
          if (encompassField2 != null)
          {
            string str = encompassField2.GetRepeatableFieldModelPath(repeatableField.Index, repeatableField.FieldId);
            if (!string.IsNullOrEmpty(str) && result > 1 && result < 100)
            {
              string newValue = string.Format("Loan.Applications[{0}]", (object) (result - 1));
              str = str.Replace("Loan.CurrentApplication", newValue);
            }
            return string.IsNullOrEmpty(str) ? (string) null : str;
          }
        }
        return (string) null;
      }
    }

    public static bool ContainsCustomField(string[] fieldIds, out List<string> customFieldIds)
    {
      customFieldIds = new List<string>();
      for (int index = 0; index < fieldIds.Length; ++index)
      {
        string fieldIdUpper;
        if (EncompassFieldData.IsCustomField(fieldIds[index], out fieldIdUpper))
          customFieldIds.Add(fieldIdUpper);
      }
      return customFieldIds.Any<string>();
    }

    public List<string> GetValidFieldIds(
      string[] fieldIds,
      out List<string> invalidFieldIds,
      out HashSet<string> fieldIdsWithInvalidAppIndex)
    {
      int result = 0;
      invalidFieldIds = new List<string>();
      fieldIdsWithInvalidAppIndex = new HashSet<string>();
      List<string> validFieldIds = new List<string>();
      for (int index = 0; index < fieldIds.Length; ++index)
      {
        string fieldId = fieldIds[index];
        string str = EncompassFieldData.Instance.GetFullModelPath(fieldId);
        if (fieldId.Contains("#"))
        {
          string[] strArray = fieldId.Split('#');
          Match match = EncompassFieldData.ContainCharacterRegex.Match(strArray[1]);
          if (strArray.Length > 2 || match.Success || string.IsNullOrEmpty(strArray[1]))
            str = (string) null;
          int.TryParse(strArray[1], out result);
          if (result > 6 || match.Success)
            fieldIdsWithInvalidAppIndex.Add(fieldId);
        }
        if (!string.IsNullOrWhiteSpace(str))
          validFieldIds.Add(fieldId);
        else
          invalidFieldIds.Add(fieldId);
      }
      return validFieldIds;
    }

    public string GetBaseFieldId(string fieldId) => this.GetBaseField(fieldId)?.ID;

    public EncompassField GetBaseField(string fieldId)
    {
      using (PerformanceMeter.Current.BeginOperation("FieldData.GetBaseField"))
      {
        EncompassField baseField1 = !string.IsNullOrWhiteSpace(fieldId) ? this.Find(fieldId) : throw new ArgumentNullException(nameof (fieldId));
        if (baseField1 != null && baseField1.HasValidCategory)
          return baseField1;
        EncompassFieldData.RepeatableField repeatableField = this.FindRepeatableField(fieldId);
        if (repeatableField != null)
        {
          EncompassField baseField2 = this.Find(repeatableField.TemplateFieldId);
          if (baseField2 != null)
            return baseField2;
        }
        return (EncompassField) null;
      }
    }

    public bool IsFieldExist(string fieldId)
    {
      using (PerformanceMeter.Current.BeginOperation("FieldData.IsFieldExist"))
      {
        EncompassField encompassField = !string.IsNullOrWhiteSpace(fieldId) ? this.Find(fieldId) : throw new ArgumentNullException(nameof (fieldId));
        return encompassField != null && encompassField.HasValidCategory;
      }
    }

    private EncompassFieldData.RepeatableField FindRepeatableField(string fieldId)
    {
      using (PerformanceMeter.Current.BeginOperation("FieldData.FindRepeatableField"))
      {
        EncompassFieldData.RepeatableField repeatableField = (EncompassFieldData.RepeatableField) null;
        lock (this._repeatableFieldMap)
        {
          if (this._repeatableFieldMap.TryGetValue(fieldId, out repeatableField))
            return repeatableField;
        }
        Regex regex = EncompassFieldData.TemplateFieldRegex;
        string str = string.Empty;
        if (fieldId.Length >= 2)
          str = fieldId.Substring(0, 2);
        if (fieldId.StartsWith("TQL4506T", StringComparison.OrdinalIgnoreCase))
          regex = EncompassFieldData.Tql4506TFieldRegex;
        Match match = regex.Match(fieldId);
        if (match.Success)
        {
          int result;
          int.TryParse(match.Groups[2].Value, out result);
          int num = str == "FL" || str == "DD" || str == "FM" || str == "SP" || str == "HC" || str == "AB" ? 999 : 99;
          if (result >= 1 && result <= num)
          {
            string fieldId1 = string.Format("{0}{1}{2}", (object) match.Groups[1].Value, (object) "00", (object) match.Groups[5].Value);
            if (this.Find(fieldId1) != null)
              repeatableField = new EncompassFieldData.RepeatableField()
              {
                TemplateFieldId = fieldId1,
                FieldId = fieldId,
                Index = result
              };
          }
        }
        lock (this._repeatableFieldMap)
          this._repeatableFieldMap[fieldId] = repeatableField;
        return repeatableField;
      }
    }

    private static bool IsCustomField(string fieldId, out string fieldIdUpper)
    {
      fieldIdUpper = fieldId.ToUpper();
      return fieldIdUpper.StartsWith("CUST") || fieldIdUpper.StartsWith("CX.") || fieldIdUpper.StartsWith("LR.");
    }

    private class RepeatableField
    {
      public string TemplateFieldId { get; set; }

      public string FieldId { get; set; }

      public int Index { get; set; }
    }
  }
}
