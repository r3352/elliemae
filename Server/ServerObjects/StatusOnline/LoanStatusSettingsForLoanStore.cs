// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusSettingsForLoanStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusSettingsForLoanStore
  {
    public static LoanStatusSettingsForLoan Get(LoanIdentity loanid)
    {
      return (LoanStatusSettingsForLoan) XmlDataStore.Deserialize(typeof (LoanStatusSettingsForLoan), LoanStatusSettingsForLoanStore.getXmlFilePath(loanid));
    }

    private static string getXmlFilePath(LoanIdentity loanid)
    {
      return ClientContext.GetCurrent().Settings.GetLoanFilePath(loanid.LoanFolder, loanid.LoanName, "LoanStatusSettings.xml", false);
    }
  }
}
