// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.TitleFeesAccessor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Server;
using ePASS.Title;
using ePASS.Title.WebServices;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class TitleFeesAccessor : ClientSession, ITitleFeesAccessor, IClientSession
  {
    private const string className = "TitleFeesAccessor";
    public static readonly SessionManager Sessions = new SessionManager((IClientContext) null);
    private string instanceName = "";
    private string loanGUID;

    public TitleFeesAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback callback)
      : base(new LoginParameters(orderUID, credentials, new ServerIdentity(instanceName), (string) null, "", false), Guid.NewGuid().ToString(), callback)
    {
      this.instanceName = instanceName;
      this.loanGUID = !loanGUID.StartsWith("{") ? "{" + loanGUID + "}" : loanGUID;
      TitleFeesAccessor.Sessions.AddSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
    }

    public override string UserID => "sysadmin";

    public OrderFeeSnapshotItem[] GetTitleFeeSnapshot()
    {
      this.onApiCalled(nameof (GetTitleFeeSnapshot));
      try
      {
        using (ClientContext.Open(this.instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          using (Loan loan = LoanStore.CheckOut(this.loanGUID))
          {
            try
            {
              if (loan.LoanData == null)
                Err.Raise(nameof (TitleFeesAccessor), new ServerException("Cannot open loan data for loan '" + this.loanGUID + "'"));
            }
            catch (Exception ex)
            {
              Err.Raise(nameof (TitleFeesAccessor), new ServerException("Cannot get LoanData for loan '" + this.loanGUID + "'"));
            }
            return FeeSnapshotUtil.BuildFeeSnapshot((IBam) new IBamWrapper(loan.LoanData));
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TitleFeesAccessor), ex);
      }
      return (OrderFeeSnapshotItem[]) null;
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
      TitleFeesAccessor.Sessions.RemoveSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
      base.Disconnect();
    }

    private void onApiCalled(string apiName, params object[] parms)
    {
      try
      {
        TraceLog.WriteApi(nameof (TitleFeesAccessor), apiName, parms);
      }
      catch
      {
      }
    }
  }
}
