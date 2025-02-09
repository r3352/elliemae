// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusCollectionStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusCollectionStore
  {
    public static LoanStatusCollection Get(string userID)
    {
      return (LoanStatusCollection) XmlDataStore.Deserialize(typeof (LoanStatusCollection), LoanStatusCollectionStore.getXmlFilePath(userID));
    }

    public static LoanStatusCollection Get(LoanIdentity loanid)
    {
      return (LoanStatusCollection) XmlDataStore.Deserialize(typeof (LoanStatusCollection), LoanStatusCollectionStore.getXmlFilePath(loanid));
    }

    private static string getXmlFilePath(string userID)
    {
      ClientContext current = ClientContext.GetCurrent();
      return string.IsNullOrEmpty(userID) ? current.Settings.GetDataFilePath("LoanStatus.xml") : current.Settings.GetUserDataFilePath(userID, "LoanStatus.xml");
    }

    private static string getXmlFilePath(LoanIdentity loanid)
    {
      return ClientContext.GetCurrent().Settings.GetLoanFilePath(loanid.LoanFolder, loanid.LoanName, "LoanStatus.xml", false);
    }
  }
}
