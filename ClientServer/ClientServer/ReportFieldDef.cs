// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ReportFieldDef
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public abstract class ReportFieldDef
  {
    protected string category;
    protected string name;
    protected string description;
    protected EllieMae.EMLite.ClientServer.Reporting.FieldTypes fieldType;
    protected string dbName;
    protected bool isVolatile;
    protected FieldDisplayType displayType;
    protected string[] relatedFields = new string[0];
    protected IQueryTerm sortTerm;
    protected FieldDefinition fieldDefinition;
    protected bool forceDataConversion;
    protected bool selectable = true;

    public ReportFieldDef(
      string category,
      string fieldId,
      string name,
      string description,
      FieldFormat format,
      string dbname)
      : this(category, (FieldDefinition) new ReportingFieldDefinition(fieldId, description, format))
    {
      this.name = name;
      this.dbName = dbname;
    }

    public ReportFieldDef(string category, FieldDefinition fieldDef)
    {
      this.category = category;
      this.name = fieldDef.Description;
      this.description = fieldDef.Description;
      this.dbName = "";
      this.fieldDefinition = fieldDef;
      this.fieldType = FieldTypeUtilities.FieldFormatToFieldType(fieldDef.Format);
      if (this.fieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString || !fieldDef.Options.RequireValueFromList)
        return;
      this.fieldType = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList;
    }

    public ReportFieldDef(string category, FieldDefinition fieldDef, string dbname)
      : this(category, fieldDef)
    {
      this.dbName = dbname;
    }

    public ReportFieldDef(
      string category,
      FieldDefinition fieldDef,
      string dbname,
      FieldDisplayType displayType,
      string[] relatedFields)
      : this(category, fieldDef)
    {
      this.dbName = dbname;
      this.displayType = displayType;
      this.relatedFields = relatedFields;
    }

    protected ReportFieldDef(string category, XmlElement fieldElement)
    {
      this.category = category;
      string fieldId = fieldElement.GetAttribute("id") ?? "";
      this.name = fieldElement.GetAttribute(nameof (name)) ?? "";
      this.description = fieldElement.GetAttribute("desc") ?? "";
      this.dbName = fieldElement.GetAttribute("dbname") ?? "";
      this.isVolatile = (fieldElement.GetAttribute("volatile") ?? "") == "1";
      string str = fieldElement.GetAttribute(nameof (displayType)) ?? "";
      if (string.Empty != str)
        this.displayType = (FieldDisplayType) Enum.Parse(typeof (FieldDisplayType), str);
      if (string.Empty == this.name)
        this.name = this.description;
      this.fieldDefinition = this.GetFieldDefinition(fieldId, fieldElement);
      this.fieldType = this.fieldDefinition != null ? FieldTypeUtilities.FieldFormatToFieldType(this.fieldDefinition.Format) : throw new Exception("FieldDefinition for ReportFieldDef cannot be NULL");
      if (this.fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString && this.fieldDefinition.Options.RequireValueFromList)
        this.fieldType = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList;
      List<string> stringList = new List<string>();
      foreach (XmlElement selectNode in fieldElement.SelectNodes("RelatedField"))
        stringList.Add(selectNode.GetAttribute("dbname"));
      this.relatedFields = stringList.ToArray();
      string expr = fieldElement.GetAttribute("sortBy") ?? "";
      if (expr != "")
        this.sortTerm = QueryTerm.Parse(expr);
      this.selectable = !((fieldElement.GetAttribute(nameof (selectable)) ?? "").ToLower() == "false");
    }

    [CLSCompliant(false)]
    public string Category
    {
      get => this.category;
      set => this.category = value;
    }

    public virtual string FieldID
    {
      get => this.fieldDefinition.FieldID;
      set => this.fieldDefinition.FieldID = value;
    }

    [CLSCompliant(false)]
    public FieldDefinition FieldDefinition => this.fieldDefinition;

    public abstract FilterDataSource DataSource { get; }

    [CLSCompliant(false)]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [CLSCompliant(false)]
    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    [CLSCompliant(false)]
    public EllieMae.EMLite.ClientServer.Reporting.FieldTypes FieldType
    {
      get => this.fieldType;
      set => this.fieldType = value;
    }

    [CLSCompliant(false)]
    public FieldDisplayType DisplayType
    {
      get => this.displayType;
      set => this.displayType = value;
    }

    [CLSCompliant(false)]
    public bool Selectable => this.selectable;

    [CLSCompliant(false)]
    public string[] RelatedFields => this.relatedFields;

    public DataConversion DataConversion
    {
      get
      {
        if (!this.forceDataConversion)
          return DataConversion.None;
        switch (this.fieldType)
        {
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric:
            return DataConversion.Numeric;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
            return DataConversion.DateTime;
          default:
            return DataConversion.None;
        }
      }
    }

    [CLSCompliant(false)]
    public bool ForceDataConversion => this.forceDataConversion;

    public string CriterionFieldName => this.dbName;

    [CLSCompliant(false)]
    public IQueryTerm SortTerm
    {
      get
      {
        if (this.sortTerm == null)
          this.sortTerm = (IQueryTerm) new DataField(this.dbName);
        return this.sortTerm;
      }
    }

    public bool IsDatabaseField => this.dbName != "";

    [CLSCompliant(false)]
    public bool IsVolatile => this.isVolatile;

    public string ToDisplayValue(string value)
    {
      if (this.FieldDefinition != null)
        return this.FieldDefinition.ToDisplayValue(value);
      return this.fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || this.fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime ? Utils.FormatDateValue(value) : value;
    }

    protected virtual FieldDefinition GetFieldDefinition(string fieldId, XmlElement fieldElement)
    {
      return (FieldDefinition) new ReportingFieldDefinition(fieldElement.GetAttribute("id"), fieldElement);
    }
  }
}
