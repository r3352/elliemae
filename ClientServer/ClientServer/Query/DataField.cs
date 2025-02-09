// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.DataField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class DataField : IQueryTerm
  {
    private string name;

    public DataField(string name)
    {
      this.name = name;
      this.UseNull = false;
    }

    public string FieldName => this.name;

    public bool UseNull { get; set; }

    public string ToString(ICriterionTranslator translator)
    {
      CriterionName criterionName = DataField.TranslateName(this.name, translator);
      return criterionName == null ? "" : criterionName.ToSqlColumnName();
    }

    public string ToString(ICriterionTranslator translator, bool withTableName)
    {
      CriterionName criterionName = DataField.TranslateName(this.name, translator);
      if (criterionName == null)
        return "";
      return !withTableName ? criterionName.ToSqlColumnNameWithoutTable() : criterionName.ToSqlColumnName();
    }

    public string[] GetTableNames(ICriterionTranslator translator)
    {
      return new string[1]
      {
        DataField.GetTableName(this.name, translator)
      };
    }

    public string[] GetFieldNames(ICriterionTranslator translator)
    {
      return new string[1]
      {
        DataField.TranslateName(this.name, translator).ToString()
      };
    }

    public static CriterionName TranslateName(string fieldName, ICriterionTranslator translator)
    {
      return translator == null ? CriterionName.Parse(fieldName) : translator.TranslateName(fieldName);
    }

    public static string GetTableName(string fieldName, ICriterionTranslator fieldTranslator)
    {
      CriterionName criterionName = DataField.TranslateName(fieldName, fieldTranslator);
      return criterionName == null ? "" : criterionName.FieldSource;
    }

    public static DataField[] CreateFields(string[] fieldNames)
    {
      if (fieldNames == null)
        return (DataField[]) null;
      List<DataField> dataFieldList = new List<DataField>();
      foreach (string fieldName in fieldNames)
        dataFieldList.Add(new DataField(fieldName));
      return dataFieldList.ToArray();
    }

    public static DataField CreateField(string fieldName, bool useNull)
    {
      if (fieldName == null)
        return (DataField) null;
      return new DataField(fieldName) { UseNull = useNull };
    }

    public override bool Equals(object obj)
    {
      return obj is DataField dataField && string.Compare(dataField.FieldName, this.FieldName, true) == 0;
    }

    public override int GetHashCode() => this.FieldName.ToLower().GetHashCode();

    public override string ToString() => this.FieldName;
  }
}
