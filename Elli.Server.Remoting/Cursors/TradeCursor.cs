// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.TradeCursor
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
  public class TradeCursor : CursorBase
  {
    private const string className = "TradeCursor";
    private bool summaryOnly;
    private string[] fields;
    private UserInfo userInfo;

    public TradeCursor Initialize(
      ISession session,
      int[] contractIds,
      string[] fields,
      bool summaryOnly)
    {
      this.InitializeInternal(session);
      this.summaryOnly = summaryOnly;
      this.fields = fields;
      this.userInfo = session.GetUserInfo();
      this.insertPrimaryKey();
      if (contractIds == null)
        return this;
      for (int index = 0; index < contractIds.Length; ++index)
        this.Items.Add((object) contractIds[index]);
      return this;
    }

    public TradeCursor Initialize(
      ISession session,
      int[] contractIds,
      string[] fields,
      bool summaryOnly,
      UserInfo userInfo)
    {
      this.InitializeInternal(session);
      this.summaryOnly = summaryOnly;
      this.fields = fields;
      this.userInfo = userInfo;
      this.insertPrimaryKey();
      if (contractIds == null)
        return this;
      for (int index = 0; index < contractIds.Length; ++index)
        this.Items.Add((object) contractIds[index]);
      return this;
    }

    private void insertPrimaryKey()
    {
      bool flag = false;
      if (this.fields != null)
      {
        foreach (string field in this.fields)
        {
          if (field == "LoanTradeDetails.TradeID")
          {
            flag = true;
            break;
          }
        }
      }
      List<string> stringList = this.fields == null ? new List<string>() : new List<string>((IEnumerable<string>) this.fields);
      if (!flag)
        stringList.Add("LoanTradeDetails.TradeID");
      this.fields = stringList.ToArray();
    }

    public override object[] GetItems(int startIndex, int count)
    {
      TraceLog.WriteApi(nameof (TradeCursor), nameof (GetItems), (object) startIndex, (object) count);
      int[] array = (int[]) this.Items.GetRange(startIndex, count).ToArray(typeof (int));
      return this.summaryOnly ? (object[]) new List<LoanTradeViewModel>((IEnumerable<LoanTradeViewModel>) LoanTrades.GetTradeViews(array).Values).ToArray() : (object[]) LoanTrades.GetTrades(array);
    }

    public override object GetItem(int index) => this.GetItems(index, 1)[0];
  }
}
