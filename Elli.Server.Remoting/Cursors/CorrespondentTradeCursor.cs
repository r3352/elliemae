// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.CorrespondentTradeCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class CorrespondentTradeCursor : CursorBase
  {
    private const string className = "CorrespondentTradeCursor";
    private UserInfo userInfo;

    public CorrespondentTradeCursor Initialize(ISession session, int[] tradeIds)
    {
      this.InitializeInternal(session);
      this.userInfo = session.GetUserInfo();
      if (tradeIds == null)
        return this;
      for (int index = 0; index < tradeIds.Length; ++index)
        this.Items.Add((object) tradeIds[index]);
      return this;
    }

    public CorrespondentTradeCursor Initialize(ISession session, int[] tradeIds, UserInfo userInfo)
    {
      this.InitializeInternal(session);
      this.userInfo = userInfo;
      if (tradeIds == null)
        return this;
      for (int index = 0; index < tradeIds.Length; ++index)
        this.Items.Add((object) tradeIds[index]);
      return this;
    }

    public override object[] GetItems(int startIndex, int count)
    {
      TraceLog.WriteApi(nameof (CorrespondentTradeCursor), nameof (GetItems), (object) startIndex, (object) count);
      return (object[]) new List<CorrespondentTradeViewModel>((IEnumerable<CorrespondentTradeViewModel>) CorrespondentTrades.GetCorrespondentTradeViews((int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int))).Values).ToArray();
    }

    public override object GetItem(int index) => this.GetItems(index, 1)[0];
  }
}
