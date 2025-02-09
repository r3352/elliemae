// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.QueryResultCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Server;
using System.Collections;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class QueryResultCursor : CursorBase
  {
    public QueryResultCursor Initialize(ISession session, QueryResult res)
    {
      this.InitializeInternal(session);
      TraceLog.WriteInfo(nameof (QueryResultCursor), "Cursor created for data set with " + (object) res.RecordCount + " rows and " + (object) res.Columns.Count + " columns");
      if (res.RecordCount > 0)
        this.Items.AddRange((ICollection) res.GetRows(0, res.RecordCount));
      return this;
    }
  }
}
