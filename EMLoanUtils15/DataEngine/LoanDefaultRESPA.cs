// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDefaultRESPA
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDefaultRESPA : FormDataBase
  {
    public static readonly string[] LoanFields = new string[3]
    {
      "RESPA.X1",
      "RESPA.X6",
      "RESPA.X28"
    };
    private DefaultFieldsInfo respaFields;

    public LoanDefaultRESPA(SessionObjects sessionObjects)
    {
      this.respaFields = sessionObjects.ConfigurationManager.GetDefaultFields("RESPAFieldList");
      this.Reset();
    }

    public LoanDefaultRESPA(DefaultFieldsInfo respaFields)
    {
      this.respaFields = respaFields;
      this.Reset();
    }

    public void Reset()
    {
      foreach (string loanField in LoanDefaultRESPA.LoanFields)
      {
        string field = this.respaFields.GetField(loanField);
        this.SetCurrentField(loanField, field);
      }
    }

    public DefaultFieldsInfo CommitChanges()
    {
      foreach (string loanField in LoanDefaultRESPA.LoanFields)
      {
        string simpleField = this.GetSimpleField(loanField);
        this.respaFields.SetField(loanField, simpleField);
      }
      return this.respaFields;
    }

    public DefaultFieldsInfo CommitChanges(SessionObjects sessionObjects)
    {
      sessionObjects.ConfigurationManager.UpdateDefaultFields(this.CommitChanges(), "RESPAFieldList");
      return this.respaFields;
    }
  }
}
