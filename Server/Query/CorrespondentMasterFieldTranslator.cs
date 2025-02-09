// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.CorrespondentMasterFieldTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class CorrespondentMasterFieldTranslator : ICriterionTranslator
  {
    private Dictionary<string, int> fieldIdMap = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, FieldFormat> fieldLabelToType = new Dictionary<string, FieldFormat>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private LoanFieldTranslator loanTranslator;

    public CorrespondentMasterFieldTranslator() => this.loanTranslator = new LoanFieldTranslator();

    public ICriterionNameFormatter CriterionNameFormatter { get; set; }

    public CriterionName TranslateName(string fieldName)
    {
      CriterionName criterionName = CriterionName.Parse(fieldName);
      if (criterionName.FieldSource == "")
        criterionName = new CriterionName("CorrespondentMaster", criterionName.FieldName);
      return this.loanTranslator.TranslateName(criterionName.ToString());
    }

    public QueryCriterion TranslateCriterion(QueryCriterion cri)
    {
      return this.loanTranslator.TranslateCriterion(cri);
    }
  }
}
