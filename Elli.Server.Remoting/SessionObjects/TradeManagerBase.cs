// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.TradeManagerBase
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public abstract class TradeManagerBase : SessionBoundObject
  {
    private string className = nameof (TradeManagerBase);
    private TradeType tradeType;
    protected string[] defaultTradeFields;

    protected void Initialize(ISession session, string className, TradeType tradeType)
    {
      this.className = className;
      this.tradeType = tradeType;
      this.defaultTradeFields = new string[2]
      {
        "Trades.TradeID",
        "Trades.Name"
      };
      this.InitializeInternal(session, className.ToLower());
    }

    public virtual string[] DefaultTradeFields => this.defaultTradeFields;

    public virtual void SetTradeStatus(int tradeId, TradeStatus status)
    {
      if (status == TradeStatus.Archived)
        this.SetTradeStatus(new int[1]{ tradeId }, status, TradeHistoryAction.TradeArchived);
      else
        this.SetTradeStatus(new int[1]{ tradeId }, status, TradeHistoryAction.TradeActivated);
    }

    public virtual void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck = true)
    {
      this.onApiCalled(this.className, nameof (SetTradeStatus), new object[4]
      {
        (object) tradeIds,
        (object) status,
        (object) action,
        (object) needPendingCheck
      });
      try
      {
        throw new NotImplementedException("Error setting trade status.");
      }
      catch (Exception ex)
      {
        Err.Reraise(this.className, ex, this.Session.SessionInfo);
      }
    }
  }
}
