// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.FieldValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public abstract class FieldValueCriterion : QueryCriterion
  {
    private IQueryTerm field;
    private bool forceConversion;

    protected FieldValueCriterion(string fieldName)
      : this(fieldName, false)
    {
    }

    protected FieldValueCriterion(string fieldName, bool forceConversion)
      : this((IQueryTerm) new DataField(fieldName), forceConversion)
    {
    }

    protected FieldValueCriterion(IQueryTerm field)
      : this(field, false)
    {
    }

    protected FieldValueCriterion(IQueryTerm field, bool forceConversion)
    {
      this.field = field;
      this.forceConversion = forceConversion;
    }

    public string FieldName => this.field.FieldName;

    public IQueryTerm Term => this.field;

    public bool ForceDataTypeConversion
    {
      get => this.forceConversion;
      set => this.forceConversion = value;
    }

    public override string[] GetTables(ICriterionTranslator fieldTranslator)
    {
      return this.field.GetTableNames(fieldTranslator);
    }

    public override string[] GetFields(ICriterionTranslator fieldTranslator)
    {
      return this.field.GetFieldNames(fieldTranslator);
    }
  }
}
