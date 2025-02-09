// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.TpoSettingsFieldTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class TpoSettingsFieldTranslator : ICriterionTranslator
  {
    public ICriterionNameFormatter CriterionNameFormatter { get; set; }

    public CriterionName TranslateName(string fieldName)
    {
      CriterionName criterionName = CriterionName.Parse(fieldName);
      if (criterionName.FieldSource == "")
        criterionName = new CriterionName("", criterionName.FieldName);
      return criterionName;
    }

    public QueryCriterion TranslateCriterion(QueryCriterion criterion) => criterion;
  }
}
