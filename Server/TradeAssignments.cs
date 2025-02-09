// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TradeAssignments
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal static class TradeAssignments
  {
    internal static string BuildSelectSql(
      string selectClause,
      string innerJoinClause,
      string whereClause,
      List<TradeType> tradeTypes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("select " + selectClause + " ");
      stringBuilder.AppendLine("from TradeAssignment ");
      stringBuilder.AppendLine(" inner join Trades on Trades.TradeID = TradeAssignment.TradeID ");
      if (innerJoinClause != null)
        stringBuilder.AppendLine(innerJoinClause);
      stringBuilder.AppendLine(" where Trades.TradeType in (" + string.Join(", ", tradeTypes.Select<TradeType, string>((Func<TradeType, string>) (t => ((int) t).ToString())).ToArray<string>()) + ") ");
      if (whereClause != null)
        stringBuilder.AppendLine(whereClause);
      return stringBuilder.ToString();
    }
  }
}
