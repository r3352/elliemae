// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.Data4CloAccessor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Server;
using ePASS.Title;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class Data4CloAccessor : ClientSession, IData4CloAccessor, IClientSession
  {
    private const string className = "Data4CloAccessor";
    public static readonly SessionManager Sessions = new SessionManager((IClientContext) null);
    private string instanceName = "";
    private string loanGUID;

    public Data4CloAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback callback)
      : base(new LoginParameters(orderUID, credentials, new ServerIdentity(instanceName), (string) null, "", false), Guid.NewGuid().ToString(), callback)
    {
      this.instanceName = instanceName;
      this.loanGUID = !loanGUID.StartsWith("{") ? "{" + loanGUID + "}" : loanGUID;
      Data4CloAccessor.Sessions.AddSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
    }

    public override string UserID => "sysadmin";

    public object GetData4Clo(string dataType)
    {
      this.onApiCalled(nameof (GetData4Clo));
      try
      {
        using (ClientContext.Open(this.instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          using (Loan loan = LoanStore.CheckOut(this.loanGUID))
          {
            try
            {
              if (loan.LoanData == null)
                Err.Raise(nameof (Data4CloAccessor), new ServerException("Cannot open loan data for loan '" + this.loanGUID + "'"));
            }
            catch (Exception ex)
            {
              Err.Raise(nameof (Data4CloAccessor), new ServerException("Cannot get LoanData for loan '" + this.loanGUID + "'"));
            }
            IBamWrapper ibamWrapper = new IBamWrapper(loan.LoanData);
            return dataType == "SummaryOfTransactions" ? (object) GetData4CloUtil.GetSummaryOfTransactions((IBam) ibamWrapper) : (object) null;
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Data4CloAccessor), ex);
      }
      return (object) null;
    }

    public override void Disconnect()
    {
      try
      {
        ClientContext.Open(this.instanceName, false).Sessions.Tracing.UnregisterListener((IClientSession) this);
      }
      catch
      {
      }
      Data4CloAccessor.Sessions.RemoveSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
      base.Disconnect();
    }

    private void onApiCalled(string apiName, params object[] parms)
    {
      try
      {
        TraceLog.WriteApi(nameof (Data4CloAccessor), apiName, parms);
      }
      catch
      {
      }
    }
  }
}
