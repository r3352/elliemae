// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.StandardField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class StandardField : PersistentField
  {
    private FieldCategory category = FieldCategory.Unknown;
    private FieldFormat format = FieldFormat.UNDEFINED;
    private bool allowInReportingDatabase;
    private bool allowEdit;
    private int dbFieldLength = -1;
    private string xpath;
    private string description;
    private string dbField;
    private FieldOptionCollection options;
    private EncompassEdition applicableEdition = ~EncompassEdition.None;
    private bool exclusive;
    private string rolodex;
    private bool fieldLockIcon;
    private string instanceDescription;

    protected StandardField(string id, XmlElement e, FieldInstanceSpecifierType specifierType)
      : base(id, specifierType)
    {
      this.allowEdit = e.GetAttribute(nameof (AllowEdit)) == "1";
      this.allowInReportingDatabase = e.GetAttribute("AllowReporting") == "1";
      this.rolodex = e.GetAttribute(nameof (Rolodex));
      this.fieldLockIcon = e.GetAttribute(nameof (FieldLockIcon)) == "1";
      FieldFormat result1;
      if (!Enum.TryParse<FieldFormat>(e.GetAttribute(nameof (Format)), true, out result1) || result1 == FieldFormat.NONE)
        result1 = FieldFormat.STRING;
      this.format = result1;
      switch (e.GetAttribute("Edition"))
      {
        case "Banker":
          this.applicableEdition = EncompassEdition.Banker;
          break;
        case "Broker":
          this.applicableEdition = EncompassEdition.Broker;
          break;
        default:
          this.applicableEdition = EncompassEdition.None;
          break;
      }
      string attribute = e.GetAttribute("MaxLength");
      int result2;
      if (string.IsNullOrEmpty(attribute) || !int.TryParse(attribute, out result2))
        result2 = 0;
      this.dbFieldLength = result2;
      this.dbField = e.GetAttribute("DbField") ?? "";
      this.description = this.getSubelementValue(e, nameof (Description));
      if (this.description == "")
        this.description = "Field " + this.FieldID;
      this.options = new FieldOptionCollection((FieldDefinition) this, (XmlElement) e.SelectSingleNode(nameof (Options)));
      this.xpath = this.getSubelementValue(e, nameof (XPath));
      this.instanceDescription = this.getSubelementValue(e, "InstanceDescription");
      this.category = this.parseCategory(e);
    }

    public StandardField(XmlElement e)
      : this(e.GetAttribute("ID"), e, e.GetAttribute("MultiInstance") == "1" ? FieldInstanceSpecifierType.Index : FieldInstanceSpecifierType.None)
    {
      this.exclusive = e.GetAttribute("Exclusive") == "1";
    }

    public StandardField(string id, XmlElement e)
      : this(id, e, FieldInstanceSpecifierType.None)
    {
    }

    protected StandardField(StandardField source, int instanceIndex)
      : base((FieldDefinition) source, (object) instanceIndex)
    {
      this.category = source.Category;
      this.format = source.Format;
      this.allowInReportingDatabase = source.AllowInReportingDatabase;
      this.allowEdit = source.AllowEdit;
      this.dbFieldLength = source.ReportingDatabaseColumnSize;
      this.xpath = source.XPath.Replace("%", string.Concat((object) instanceIndex));
      this.description = source.GetInstanceDescription(source.instanceDescription, instanceIndex);
      this.options = source.Options;
      this.rolodex = source.Rolodex;
      this.fieldLockIcon = source.FieldLockIcon;
      this.applicableEdition = source.applicableEdition;
      this.dbField = source.dbField;
      this.instanceDescription = source.instanceDescription;
    }

    public override FieldFormat Format
    {
      get => this.format;
      set => this.format = value;
    }

    public override bool AllowInReportingDatabase => this.allowInReportingDatabase;

    public override bool AllowEdit => this.allowEdit;

    public override bool RequiresExclusiveLock => this.exclusive;

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return this.applicableEdition == EncompassEdition.None || this.applicableEdition == edition;
    }

    public override int ReportingDatabaseColumnSize
    {
      get
      {
        if (this.MaxLength > 0)
          return this.MaxLength;
        return this.dbFieldLength > 0 ? this.dbFieldLength : base.ReportingDatabaseColumnSize;
      }
    }

    public string DatabaseFieldName => this.dbField;

    public override string Description => this.description;

    public override FieldOptionCollection Options => this.options;

    internal override string XPath => this.xpath;

    public override FieldCategory Category => this.category;

    public override string Rolodex
    {
      get => this.rolodex;
      set => this.rolodex = value;
    }

    public override bool FieldLockIcon => this.fieldLockIcon;

    public override string GetValue(LoanData loan) => this.GetValue(loan, this.FieldID);

    public override string GetValue(LoanData loan, string id)
    {
      if (this.MultiInstance)
        throw new InvalidOperationException("The current field is multi-instance");
      return loan.GetField(this.FieldID);
    }

    public override void SetValue(LoanData loan, string value)
    {
      if (this.MultiInstance)
        throw new InvalidOperationException("The current field is multi-instance");
      loan.SetField(this.FieldID, value);
    }

    public override string GetInstanceID(object instanceSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      int num = Convert.ToInt32(instanceSpecifier);
      string str1 = this.FieldID.Substring(0, 2);
      string str2;
      string upper;
      if (str1 == "TQ" && this.FieldID.StartsWith("TQLGSE"))
      {
        str2 = "TQLGSE";
        upper = this.FieldID.Substring(8).ToUpper();
      }
      else if (str1 == "NB" && this.FieldID.StartsWith("NBOC"))
      {
        str2 = "NBOC";
        upper = this.FieldID.Substring(6).ToUpper();
      }
      else if (str1 == "XC" && this.FieldID.StartsWith("XCOC"))
      {
        str2 = "XCOC";
        upper = this.FieldID.Substring(6).ToUpper();
      }
      else if (str1 == "FL" || str1 == "DD" || str1 == "FM" || str1 == "LP" || str1 == "SP" || str1 == "HC" || str1 == "AB")
      {
        str2 = str1;
        upper = this.FieldID.Substring(4).ToUpper();
      }
      else if (str1 == "IR" && this.FieldID.Length == 8)
      {
        str2 = str1;
        upper = this.FieldID.Substring(this.FieldID.Length - 4).ToUpper();
      }
      else if (str1 == "VA" && this.FieldID.StartsWith("VAL") && this.FieldID.Length == 9)
      {
        str2 = "VAL";
        upper = this.FieldID.Substring(this.FieldID.Length - 4).ToUpper();
      }
      else
      {
        str2 = this.FieldID.Substring(0, this.FieldID.Length - 4).ToUpper();
        upper = this.FieldID.Substring(this.FieldID.Length - 2).ToUpper();
      }
      if (str2 == "FE" && !this.FieldID.StartsWith("FEMA"))
      {
        str2 = num % 2 == 1 ? "FBE" : "FCE";
        num = (num - 1) / 2;
      }
      return str2 + num.ToString("00") + upper;
    }

    public string GetInstanceDescription(string desc, int instanceIndex)
    {
      return desc != "" ? desc.Replace("%", string.Concat((object) instanceIndex)) : this.Description + " #" + (object) instanceIndex;
    }

    public override FieldDefinition CreateInstance(object instanceSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      int int32;
      try
      {
        int32 = Convert.ToInt32(instanceSpecifier);
      }
      catch
      {
        throw new ArgumentException("Instance specifier for a standard field must be integral");
      }
      return int32 >= 1 && int32 <= 999 ? (FieldDefinition) new StandardField(this, int32) : throw new ArgumentException("Instance specifier must be between 1 and 999");
    }

    public override FieldDefinition CreateInstanceWithID(string fieldId)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      if (fieldId.Length < 5)
        return (FieldDefinition) null;
      StandardField.MultiInstanceFieldElements multiInstanceFieldId = StandardField.parseMultiInstanceFieldID(fieldId);
      if (multiInstanceFieldId == null || string.Compare(multiInstanceFieldId.MasterFieldID, this.FieldID, true) != 0)
        throw new Exception("Specified field ID is not an instance of the current field");
      return this.CreateInstance((object) multiInstanceFieldId.RecordIndex);
    }

    private string getSubelementValue(XmlElement e, string name)
    {
      return !(e.SelectSingleNode(name) is XmlElement xmlElement) ? "" : xmlElement.InnerText;
    }

    private FieldCategory parseCategory(XmlElement e)
    {
      switch (e.GetAttribute("Category"))
      {
        case "Borrower":
          return FieldCategory.Borrower;
        case "Coborrower":
          return FieldCategory.Coborrower;
        default:
          return FieldCategory.Common;
      }
    }

    internal static string GetMultiInstanceParentID(string fieldId)
    {
      return StandardField.parseMultiInstanceFieldID(fieldId)?.MasterFieldID;
    }

    public static StandardField.MultiInstanceFieldElements parseMultiInstanceFieldID(string fieldId)
    {
      if (fieldId.Length < 5)
        return (StandardField.MultiInstanceFieldElements) null;
      string str1 = fieldId.Substring(0, 2);
      string prefix;
      string str2;
      string postfix;
      if (str1 == "TQ" && fieldId.StartsWith("TQLGSE"))
      {
        if (fieldId.Length < 10)
          return (StandardField.MultiInstanceFieldElements) null;
        prefix = "TQLGSE";
        str2 = fieldId.Substring(6, fieldId.Length > 10 ? 3 : 2);
        postfix = fieldId.Substring(fieldId.Length > 10 ? 9 : 8);
      }
      else if (str1 == "NB" && fieldId.StartsWith("NBOC"))
      {
        if (fieldId.Length < 8)
          return (StandardField.MultiInstanceFieldElements) null;
        prefix = "NBOC";
        str2 = fieldId.Substring(4, fieldId.Length > 8 ? 3 : 2);
        postfix = fieldId.Substring(fieldId.Length > 8 ? 7 : 6);
      }
      else if (str1 == "XC" && fieldId.StartsWith("XCOC"))
      {
        if (fieldId.Length < 8)
          return (StandardField.MultiInstanceFieldElements) null;
        prefix = "XCOC";
        str2 = fieldId.Substring(4, fieldId.Length > 8 ? 3 : 2);
        postfix = fieldId.Substring(fieldId.Length > 8 ? 7 : 6);
      }
      else if (str1 == "LP")
      {
        prefix = str1;
        str2 = fieldId.Substring(2, fieldId.Length > 6 ? 3 : 2);
        postfix = fieldId.Substring(fieldId.Length > 6 ? 5 : 4);
      }
      else if (str1 == "FL" || str1 == "DD" || str1 == "FM" || str1 == "SP" || str1 == "HC" || str1 == "AB")
      {
        prefix = str1;
        str2 = fieldId.Substring(2, fieldId.Length - 4);
        postfix = fieldId.Substring(fieldId.Length - 2);
      }
      else if (str1 == "IR" && fieldId.Length == 8)
      {
        prefix = str1;
        str2 = fieldId.Substring(2, 2);
        postfix = fieldId.Substring(4);
      }
      else if (str1 == "VA" && fieldId.StartsWith("VAL") && fieldId.Length == 9)
      {
        prefix = "VAL";
        str2 = fieldId.Substring(3, 2);
        postfix = fieldId.Substring(5);
      }
      else
      {
        prefix = fieldId.Substring(0, fieldId.Length - 4);
        str2 = fieldId.Substring(fieldId.Length - 4, 2);
        postfix = fieldId.Substring(fieldId.Length - 2);
      }
      int index = Utils.ParseInt((object) str2, -1);
      return index <= 0 ? (StandardField.MultiInstanceFieldElements) null : new StandardField.MultiInstanceFieldElements(prefix, index, postfix);
    }

    public static bool IsValueFieldDefinition(XmlElement e)
    {
      return !((e.GetAttribute("Format") ?? "") == "");
    }

    public class MultiInstanceFieldElements
    {
      public string Prefix;
      public int RecordIndex;
      public string Postfix;

      public MultiInstanceFieldElements(string prefix, int index, string postfix)
      {
        this.Prefix = prefix;
        this.RecordIndex = index;
        this.Postfix = postfix;
      }

      public string MasterFieldID
      {
        get
        {
          string str = this.Prefix;
          if (str == "FBE" || str == "FCE")
            str = "FE";
          return str + "00" + this.Postfix;
        }
      }

      public string FieldID => this.Prefix + this.RecordIndex.ToString("00") + this.Postfix;
    }
  }
}
