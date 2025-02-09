// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomFieldInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class CustomFieldInfo : IFieldSearchable
  {
    public const string StandardCustomFieldPrefix = "CUST";
    public const string ExtendedCustomFieldPrefix = "CX.";
    private string fieldId = "";
    private string description = "";
    private FieldFormat format;
    private string[] options;
    private string calculation = "";
    private int maxLength;
    private FieldAuditSettings auditSettings;

    public CustomFieldInfo(string fieldId) => this.fieldId = fieldId.ToUpper();

    public CustomFieldInfo(string fieldId, FieldFormat format)
    {
      this.fieldId = fieldId.ToUpper();
      this.Format = format;
    }

    public CustomFieldInfo(string fieldID, string desc, FieldFormat fieldFormat, string[] options)
      : this(fieldID, desc, fieldFormat, options, 0, (FieldAuditSettings) null, "")
    {
    }

    public CustomFieldInfo(
      string fieldID,
      string desc,
      FieldFormat fieldFormat,
      int maxLength,
      string calculation)
      : this(fieldID, desc, fieldFormat, (string[]) null, maxLength, (FieldAuditSettings) null, calculation)
    {
    }

    public CustomFieldInfo(
      string fieldID,
      string desc,
      FieldFormat fieldFormat,
      string[] options,
      int maxLength,
      FieldAuditSettings auditSettings,
      string calculation)
    {
      this.fieldId = fieldID.ToUpper();
      this.description = desc;
      this.format = fieldFormat;
      this.options = options;
      this.calculation = calculation;
      this.maxLength = maxLength;
      this.auditSettings = auditSettings;
    }

    public CustomFieldInfo(BinaryReader br)
    {
      try
      {
        this.fieldId = br.ReadString();
        this.description = br.ReadString();
        this.format = (FieldFormat) br.ReadInt32();
        this.calculation = br.ReadString();
        this.maxLength = br.ReadInt32();
        int num1 = br.ReadInt32();
        string fieldId = br.ReadString();
        this.auditSettings = num1 == -1 || fieldId == null ? (FieldAuditSettings) null : new FieldAuditSettings(fieldId, (AuditData) num1);
        int num2 = br.ReadInt32();
        if (num2 > 0)
        {
          List<string> stringList = new List<string>();
          for (; num2 > 0; --num2)
            stringList.Add(br.ReadString());
          this.options = stringList.ToArray();
        }
        else
          this.options = (string[]) null;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string FieldID => this.fieldId;

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public FieldFormat Format
    {
      get => this.format;
      set => this.format = value;
    }

    public string[] Options
    {
      get => this.options;
      set => this.options = value;
    }

    public string Calculation
    {
      get
      {
        if (this.Format == FieldFormat.AUDIT)
          return this.generateAuditCalculation();
        return !this.IsCalculationAllowed() ? "" : this.calculation;
      }
      set
      {
        if (!this.IsCalculationAllowed())
          throw new ArgumentException("A calculation cannot be specified for this field");
        this.calculation = value;
      }
    }

    public int MaxLength
    {
      get
      {
        return this.format != FieldFormat.STRING && this.format != FieldFormat.DROPDOWN ? 0 : this.maxLength;
      }
      set => this.maxLength = value;
    }

    public FieldAuditSettings AuditSettings
    {
      get => this.IsAuditField() ? this.auditSettings : (FieldAuditSettings) null;
      set
      {
        if (!this.IsAuditField())
          throw new InvalidOperationException("Audit settings can only be saved to an Audit field");
        this.auditSettings = value;
      }
    }

    public void Clear()
    {
      this.description = "";
      this.format = FieldFormat.NONE;
      this.options = (string[]) null;
      this.calculation = "";
      this.maxLength = 0;
      this.auditSettings = (FieldAuditSettings) null;
    }

    public bool IsExtendedField() => this.fieldId.StartsWith("CX.");

    public bool IsAuditField() => this.Format == FieldFormat.AUDIT;

    public bool IsNumericValued()
    {
      switch (this.Format)
      {
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return true;
        default:
          return false;
      }
    }

    public bool IsDateValued()
    {
      switch (this.Format)
      {
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
          return true;
        case FieldFormat.AUDIT:
          return this.auditSettings != null && this.auditSettings.AuditData == AuditData.Timestamp;
        default:
          return false;
      }
    }

    public bool IsTextValued()
    {
      switch (this.Format)
      {
        case FieldFormat.NONE:
        case FieldFormat.STRING:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
        case FieldFormat.DROPDOWN:
          return true;
        case FieldFormat.AUDIT:
          return this.auditSettings != null && this.auditSettings.AuditData != AuditData.Timestamp;
        default:
          return false;
      }
    }

    public bool IsListValued()
    {
      switch (this.Format)
      {
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
          return true;
        default:
          return false;
      }
    }

    public bool IsCalculationAllowed() => !this.IsAuditField();

    public bool IsEmpty()
    {
      return this.Description == "" && this.Format == FieldFormat.NONE && this.Calculation == "";
    }

    public override string ToString() => this.FieldID;

    public override bool Equals(object obj)
    {
      return obj is CustomFieldInfo customFieldInfo && customFieldInfo.FieldID == this.FieldID;
    }

    public override int GetHashCode() => this.FieldID.GetHashCode();

    public static bool IsCustomFieldID(string fieldId)
    {
      return fieldId.StartsWith("CUST") || fieldId.StartsWith("CX.");
    }

    private string generateAuditCalculation()
    {
      if (this.auditSettings == null)
        return "";
      return "Audit([" + this.auditSettings.FieldID + "], " + (object) (int) this.auditSettings.AuditData + ")";
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, this.FieldID);
      if (this.Calculation != null)
      {
        string[] strArray = FieldReplacementRegex.ParseDependentFields(this.Calculation);
        for (int index = 0; index < strArray.Length; ++index)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index]);
        strArray = (string[]) null;
      }
    }

    public void WriteBytes(BinaryWriter bw)
    {
      bw.Write(this.fieldId);
      bw.Write(this.description);
      bw.Write((int) this.format);
      bw.Write(this.calculation);
      bw.Write(this.maxLength);
      if (this.auditSettings == null)
      {
        bw.Write(-1);
        bw.Write((string) null);
      }
      else
      {
        bw.Write((int) this.auditSettings.AuditData);
        bw.Write(this.auditSettings.FieldID);
      }
      if (this.options != null)
      {
        bw.Write(this.options.Length);
        foreach (string option in this.options)
          bw.Write(option);
      }
      else
        bw.Write(0);
    }
  }
}
