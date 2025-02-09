// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public abstract class FieldDefinition : IComparable
  {
    public static readonly FieldDefinition Empty = (FieldDefinition) new FieldDefinition.EmptyFieldDefinition();
    public const int DefaultReportingDatabaseTextColumnSize = 64;
    private string fieldId = "";
    private int fieldIdAsNumber = int.MinValue;
    private FieldDefinition parentField;
    private FieldInstanceSpecifierType instanceSpecifierType;
    private object instanceSpecifier;

    protected FieldDefinition(string fieldId)
      : this(fieldId, FieldInstanceSpecifierType.None)
    {
    }

    protected FieldDefinition(string fieldId, FieldInstanceSpecifierType instanceSpecifierType)
    {
      this.fieldId = !string.IsNullOrEmpty(fieldId) ? fieldId : throw new ArgumentException("FieldID cannot be blank or null", nameof (fieldId));
      this.instanceSpecifierType = instanceSpecifierType;
    }

    protected FieldDefinition(FieldDefinition source, object instanceSpecifier)
    {
      this.fieldId = source.GetInstanceID(instanceSpecifier);
      if (string.IsNullOrEmpty(this.fieldId))
        throw new ArgumentException("FieldID cannot be blank or null", nameof (fieldId));
      this.parentField = source;
      this.instanceSpecifier = instanceSpecifier;
      if (!this.fieldId.StartsWith("TQLGSE") && !this.fieldId.StartsWith("URLABAKA") && !this.fieldId.StartsWith("URLACAKA") && !this.fieldId.StartsWith("NBOC") && !this.fieldId.StartsWith("UNFL") && !this.fieldId.StartsWith("XCOC") && (!this.fieldId.StartsWith("TR") || this.fieldId.Length < 6))
        return;
      this.instanceSpecifierType = FieldInstanceSpecifierType.Index;
    }

    private FieldDefinition() => this.fieldId = "";

    public abstract FieldFormat Format { get; set; }

    public abstract string Description { get; }

    public abstract string Rolodex { get; set; }

    public virtual bool FieldLockIcon => false;

    public abstract string GetValue(LoanData loan);

    public abstract void SetValue(LoanData loan, string value);

    public virtual string GetValue(LoanData loan, string id) => "";

    public string FieldID
    {
      get => this.fieldId;
      set => this.fieldId = value;
    }

    public bool MultiInstance => this.instanceSpecifierType != 0;

    public FieldDefinition ParentField => this.parentField;

    public FieldInstanceSpecifierType InstanceSpecifierType => this.instanceSpecifierType;

    public object InstanceSpecifier => this.instanceSpecifier;

    public bool IsInstance => this.parentField != null;

    public virtual bool AllowInReportingDatabase => false;

    public virtual bool AllowEdit => false;

    public virtual int ReportingDatabaseColumnSize
    {
      get
      {
        switch (this.ReportingDatabaseColumnType)
        {
          case ReportingDatabaseColumnType.Numeric:
            return 13;
          case ReportingDatabaseColumnType.Date:
          case ReportingDatabaseColumnType.DateTime:
            return 4;
          default:
            return this.MaxLength > 0 ? this.MaxLength : 64;
        }
      }
    }

    public virtual FieldOptionCollection Options => FieldOptionCollection.Empty;

    public virtual FieldCategory Category => FieldCategory.Common;

    public virtual VirtualFieldType VirtualFieldType => VirtualFieldType.None;

    public bool RequiresBorrowerPredicate
    {
      get => this.Category == FieldCategory.Borrower || this.Category == FieldCategory.Coborrower;
    }

    public virtual bool AppliesToEdition(EncompassEdition edition) => true;

    public virtual int MaxLength
    {
      get
      {
        switch (this.Format)
        {
          case FieldFormat.YN:
            return 1;
          case FieldFormat.X:
            return 1;
          case FieldFormat.PHONE:
            return 17;
          case FieldFormat.SSN:
            return 11;
          case FieldFormat.INTEGER:
            return 11;
          case FieldFormat.DECIMAL_1:
            return 13;
          case FieldFormat.DECIMAL_2:
            return 14;
          case FieldFormat.DECIMAL_3:
            return 15;
          case FieldFormat.DECIMAL_4:
            return 16;
          case FieldFormat.DECIMAL_6:
            return 18;
          case FieldFormat.DECIMAL_5:
            return 17;
          case FieldFormat.DECIMAL_7:
            return 19;
          case FieldFormat.DECIMAL:
            return 16;
          case FieldFormat.DECIMAL_10:
            return 22;
          case FieldFormat.DATE:
            return 10;
          case FieldFormat.MONTHDAY:
            return 7;
          case FieldFormat.DROPDOWNLIST:
            return 0;
          case FieldFormat.AUDIT:
            return 0;
          default:
            return 0;
        }
      }
    }

    public bool EnforceMaxLengthDuringValidation
    {
      get => Utils.GetNativeValueType(this.Format) == typeof (string);
    }

    public virtual ReportingDatabaseColumnType ReportingDatabaseColumnType
    {
      get
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
            return ReportingDatabaseColumnType.Numeric;
          case FieldFormat.DATE:
          case FieldFormat.MONTHDAY:
            return ReportingDatabaseColumnType.Date;
          case FieldFormat.DATETIME:
            return ReportingDatabaseColumnType.DateTime;
          default:
            return ReportingDatabaseColumnType.Text;
        }
      }
    }

    public virtual string GetInstanceID(object instanceSpecifier)
    {
      throw new NotSupportedException("The specified field type does not support multi-instance fields");
    }

    public virtual FieldDefinition CreateInstance(object instanceSpecifier)
    {
      throw new NotSupportedException("The specified field type does not support multi-instance fields");
    }

    public virtual FieldDefinition CreateInstanceWithID(string fieldId)
    {
      throw new NotSupportedException("The specified field type does not support multi-instance fields");
    }

    public bool IsNumeric()
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

    public bool HasOptions() => this.Options != null && this.Options.Count > 0;

    public bool IsDateValued()
    {
      switch (this.Format)
      {
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
        case FieldFormat.DATETIME:
          return true;
        default:
          return false;
      }
    }

    public virtual bool RequiresExclusiveLock => false;

    public object ToNativeValue(string value, bool throwOnError)
    {
      return Utils.ConvertToNativeValue(value, this.Format, throwOnError);
    }

    public object ToNativeValue(string value, object defaultValue)
    {
      return Utils.ConvertToNativeValue(value, this.Format, defaultValue);
    }

    public string ToDisplayValue(string value)
    {
      string text = this.Options.ValueToText(value);
      return text != "" || this.Options.RequireValueFromList ? text : Utils.ApplyFieldFormatting(value, this.Format, false);
    }

    public bool CompareValues(string val1, string val2)
    {
      return object.Equals(this.ToNativeValue(val1, (object) null), this.ToNativeValue(val2, (object) null)) || this.Format == FieldFormat.YN && val1 + val2 == "N";
    }

    public string FormatValue(string value) => Utils.ApplyFieldFormatting(value, this.Format);

    public string UnformatValue(string value) => Utils.UnformatValue(value, this.Format);

    public string ValidateFormat(string value)
    {
      if (value == null)
        value = string.Empty;
      value = value.Trim();
      if (!this.Options.IsValueAllowed(value))
        throw new FormatException("Allowed values are: " + string.Join(",", this.Options.GetValues()));
      if (this.MaxLength > 0 && value.Length > this.MaxLength && this.EnforceMaxLengthDuringValidation)
        value = value.Substring(0, this.MaxLength).Trim();
      return Utils.ConvertToLoanInternalValue(value, this.Format);
    }

    public string ValidateInput(string value)
    {
      if (!this.AllowEdit)
        throw new InvalidOperationException("Field '" + this.fieldId + "' is read-only");
      return this.ValidateFormat(value);
    }

    public int CompareTo(object obj)
    {
      FieldDefinition fieldDefinition = (FieldDefinition) obj;
      if (this.fieldIdAsNumber == int.MinValue)
      {
        this.fieldIdAsNumber = -1;
        if (char.IsDigit(this.fieldId[0]))
        {
          try
          {
            this.fieldIdAsNumber = int.Parse(this.fieldId);
          }
          catch
          {
          }
        }
      }
      if (this.fieldIdAsNumber != -1 && fieldDefinition.fieldIdAsNumber == -1)
        return -1;
      if (this.fieldIdAsNumber == -1 && fieldDefinition.fieldIdAsNumber != -1)
        return 1;
      return this.fieldIdAsNumber != -1 ? this.fieldIdAsNumber - fieldDefinition.fieldIdAsNumber : string.Compare(this.fieldId, fieldDefinition.fieldId, true);
    }

    public override string ToString() => this.fieldId;

    public override bool Equals(object obj)
    {
      return obj is FieldDefinition fieldDefinition && string.Compare(fieldDefinition.FieldID, this.FieldID, true) == 0;
    }

    public override int GetHashCode() => this.FieldID.ToUpper().GetHashCode();

    private class EmptyFieldDefinition : FieldDefinition
    {
      private FieldFormat format;

      public override FieldFormat Format
      {
        get => this.format;
        set => this.format = FieldFormat.NONE;
      }

      public override string Description => "";

      public override string Rolodex
      {
        get => (string) null;
        set
        {
        }
      }

      public override string GetValue(LoanData loan) => "";

      public override string GetValue(LoanData loan, string id) => "";

      public override void SetValue(LoanData loan, string value)
      {
        throw new NotSupportedException("Cannot set value on Empty field");
      }
    }
  }
}
