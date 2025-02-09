// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.LoanFieldTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class LoanFieldTranslator : EllieMae.EMLite.ReportingDbUtils.Query.LoanFieldTranslator
  {
    private AlertConfig[] alertConfigs;

    public LoanFieldTranslator()
      : this(false)
    {
    }

    public LoanFieldTranslator(bool useERDB)
      : base(LoanXDBStore.GetLoanXDBTableList(useERDB))
    {
    }

    public LoanFieldTranslator(LoanXDBTableList xdbConfig)
      : base(xdbConfig)
    {
    }

    public override CriterionName TranslateName(string fieldName)
    {
      CriterionName criName = CriterionName.Parse(fieldName, this.CriterionNameFormatter);
      if (string.Compare(criName.FieldSource, "Alert", true) == 0)
        return this.translateAlertCriterionName(criName);
      try
      {
        return base.TranslateName(fieldName);
      }
      catch (Exception ex)
      {
        if (this.TranslateNotFoundFieldAsNull)
          return (CriterionName) null;
        throw;
      }
    }

    private CriterionName translateAlertCriterionName(CriterionName criName)
    {
      if (this.alertConfigs == null)
        this.alertConfigs = AlertConfigAccessor.GetAlertConfigList();
      AlertConfig forCriterionName = Alerts.GetAlertForCriterionName(criName.ToString(), this.alertConfigs);
      return forCriterionName == null ? criName : new CriterionName(criName.FieldSource + "_" + (object) forCriterionName.AlertID, "AlertCount", this.CriterionNameFormatter);
    }

    public bool TranslateNotFoundFieldAsNull { get; set; }
  }
}
