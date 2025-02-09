// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.CriterionName
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  public class CriterionName
  {
    private string fieldSource = "";
    private string fieldName = "";
    private ICriterionNameFormatter criterionNameFormatter;

    public CriterionName(
      string fieldSource,
      string fieldName,
      ICriterionNameFormatter criterionNameFormatter)
    {
      this.fieldSource = fieldSource;
      this.fieldName = fieldName;
      this.criterionNameFormatter = criterionNameFormatter;
    }

    public CriterionName(string fieldSource, string fieldName)
      : this(fieldSource, fieldName, (ICriterionNameFormatter) null)
    {
    }

    public string FieldSource => this.fieldSource;

    public string FieldName => this.fieldName;

    public override bool Equals(object obj)
    {
      CriterionName criterionName = obj as CriterionName;
      return obj != null && string.Compare(this.fieldSource, criterionName.fieldSource, true) == 0 && string.Compare(this.fieldName, criterionName.fieldName, true) == 0;
    }

    public override int GetHashCode()
    {
      return this.fieldSource.GetHashCode() ^ this.fieldName.GetHashCode();
    }

    public override string ToString()
    {
      return this.fieldSource != "" ? this.fieldSource + "." + this.fieldName : this.fieldName;
    }

    public string ToSqlColumnName()
    {
      if (this.criterionNameFormatter != null)
        return this.criterionNameFormatter.FormatSqlColumnName(this.fieldSource, this.fieldName);
      if (!(this.fieldSource != ""))
        return "[" + this.fieldName + "]";
      return "[" + this.fieldSource + "].[" + this.fieldName + "]";
    }

    public string ToSqlColumnNameWithoutTable() => "[" + this.fieldName + "]";

    public static CriterionName Parse(
      string criterionName,
      ICriterionNameFormatter criterionNameFormatter = null)
    {
      string[] strArray = criterionName.Split('.');
      if (strArray.Length == 0)
        throw new ArgumentException("Invalid criterion name");
      return strArray.Length == 1 ? new CriterionName("", strArray[0], criterionNameFormatter) : new CriterionName(strArray[0], string.Join(".", strArray, 1, strArray.Length - 1), criterionNameFormatter);
    }
  }
}
