// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.WebServices.LoanService
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using System;
using System.Web.Services;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.Server.WebServices
{
  [WebService(Namespace = "http://encompass.elliemae.com/EncompassServices/")]
  public class LoanService : EncompassService
  {
    [WebMethod]
    [Secure]
    [SoapHeader("Credentials")]
    public string OpenLoan(string guid, bool acquireLock)
    {
      using (ClientContext.Get("", true).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        using (Loan loan = LoanStore.CheckOut(guid))
          return !loan.Exists ? (string) null : loan.Identity.ToString();
      }
    }
  }
}
