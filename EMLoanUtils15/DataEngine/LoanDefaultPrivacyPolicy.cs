// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDefaultPrivacyPolicy
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDefaultPrivacyPolicy : FormDataBase
  {
    public static readonly string[] LoanFields;
    private DefaultFieldsInfo formFields;

    static LoanDefaultPrivacyPolicy()
    {
      List<string> stringList = new List<string>();
      for (int index = 51; index <= 99; ++index)
      {
        if (index != 95 && index != 96 && index != 97)
          stringList.Add("NOTICES.X" + (object) index);
      }
      LoanDefaultPrivacyPolicy.LoanFields = stringList.ToArray();
    }

    public LoanDefaultPrivacyPolicy(SessionObjects sessionObjects)
    {
      this.formFields = sessionObjects.ConfigurationManager.GetDefaultFields("PrivacyPolicyFieldList");
      this.Reset();
    }

    public LoanDefaultPrivacyPolicy(DefaultFieldsInfo formFields)
    {
      this.formFields = formFields;
      this.Reset();
    }

    public void Reset()
    {
      foreach (string loanField in LoanDefaultPrivacyPolicy.LoanFields)
      {
        string field = this.formFields.GetField(loanField);
        this.SetCurrentField(loanField, field);
      }
      if (string.IsNullOrEmpty(this.GetSimpleField("NOTICES.X98")))
        this.SetCurrentField("NOTICES.X98", "1");
      if (!string.IsNullOrEmpty(this.GetSimpleField("NOTICES.X99")))
        return;
      this.SetCurrentField("NOTICES.X99", "11");
    }

    public DefaultFieldsInfo CommitChanges()
    {
      foreach (string loanField in LoanDefaultPrivacyPolicy.LoanFields)
      {
        string simpleField = this.GetSimpleField(loanField);
        this.formFields.SetField(loanField, simpleField);
      }
      return this.formFields;
    }

    public DefaultFieldsInfo CommitChanges(SessionObjects sessionObjects)
    {
      sessionObjects.ConfigurationManager.UpdateDefaultFields(this.CommitChanges(), "PrivacyPolicyFieldList");
      return this.formFields;
    }
  }
}
