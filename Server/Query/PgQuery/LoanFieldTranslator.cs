// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.PgQuery.LoanFieldTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.SQE;
using Elli.SQE.IO.PGSQL.QueryDsl;
using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Query.PgQuery
{
  internal class LoanFieldTranslator : EllieMae.EMLite.Server.Query.LoanFieldTranslator
  {
    public LoanFieldTranslator()
    {
      this.CriterionNameFormatter = (ICriterionNameFormatter) new Elli.SQE.IO.PGSQL.QueryDsl.CriterionNameFormatter();
    }

    public override CriterionName TranslateName(string fieldName)
    {
      try
      {
        CriterionName criterionName = CriterionName.Parse(fieldName, this.CriterionNameFormatter);
        if (string.Compare(criterionName.FieldSource, "Fields", StringComparison.OrdinalIgnoreCase) != 0)
          return base.TranslateName(fieldName);
        QueryableFieldSource queryableFieldSource = LoanFieldSources.Instance.Generate((IQueryTerm) new DataField(criterionName.FieldName));
        JQueryableFieldCriterionNameFormatter criterionNameFormatter = queryableFieldSource != null ? new JQueryableFieldCriterionNameFormatter(queryableFieldSource.Definition, "EntityData") : throw new NullReferenceException("No matching LoanFieldSource cannot be found for FieldName: " + fieldName);
        return new CriterionName(queryableFieldSource.Name, criterionName.FieldName, (ICriterionNameFormatter) criterionNameFormatter);
      }
      catch (Exception ex)
      {
        if (this.TranslateNotFoundFieldAsNull)
          return (CriterionName) null;
        throw;
      }
    }
  }
}
