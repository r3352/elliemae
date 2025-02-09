// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.OverNightRateProtection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class OverNightRateProtection
  {
    public static double GetOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      DateTime onrpStartDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select AccruedAmount from OnrpPeriodAccruedAmount ");
      dbQueryBuilder.AppendLine("where EntityChannel = '" + ((int) channel).ToString() + "' ");
      switch (channel)
      {
        case LoanChannel.BankedRetail:
          dbQueryBuilder.AppendLine("and BranchOrgId = " + entityId + " ");
          break;
        case LoanChannel.BankedWholesale:
          dbQueryBuilder.AppendLine("and ExternalCompanyId = '" + entityId + "' ");
          break;
        case LoanChannel.Correspondent:
          dbQueryBuilder.AppendLine("and ExternalCompanyId = '" + entityId + "' ");
          break;
      }
      dbQueryBuilder.AppendLine("and OnrpStartDate = '" + (object) onrpStartDate.Date + "' ");
      return SQL.DecodeDouble(dbQueryBuilder.ExecuteScalar(), 0.0);
    }

    public static void UpdateOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      DateTime onrpStartDate,
      double loanAmount)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("WHERE EntityChannel = '" + ((int) channel).ToString() + "' ");
      switch (channel)
      {
        case LoanChannel.BankedRetail:
          dbQueryBuilder1.AppendLine("AND BranchOrgId = " + entityId + " ");
          break;
        case LoanChannel.BankedWholesale:
          dbQueryBuilder1.AppendLine("AND ExternalCompanyId = '" + entityId + "' ");
          break;
        case LoanChannel.Correspondent:
          dbQueryBuilder1.AppendLine("AND ExternalCompanyId = '" + entityId + "' ");
          break;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("IF EXISTS(SELECT 1 FROM OnrpPeriodAccruedAmount ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine("and OnrpStartDate = '" + (object) onrpStartDate.Date + "' ");
      dbQueryBuilder2.AppendLine(") ");
      dbQueryBuilder2.AppendLine("BEGIN ");
      dbQueryBuilder2.AppendLine("    UPDATE OnrpPeriodAccruedAmount ");
      dbQueryBuilder2.AppendLine("    SET AccruedAmount = AccruedAmount + " + (object) loanAmount + " ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine("    and OnrpStartDate = '" + (object) onrpStartDate.Date + "' ");
      dbQueryBuilder2.AppendLine("END ");
      dbQueryBuilder2.AppendLine("ELSE");
      dbQueryBuilder2.AppendLine("    IF EXISTS(SELECT 1 FROM OnrpPeriodAccruedAmount ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine("    ) ");
      dbQueryBuilder2.AppendLine("    BEGIN ");
      dbQueryBuilder2.AppendLine("        UPDATE OnrpPeriodAccruedAmount ");
      dbQueryBuilder2.AppendLine("        SET AccruedAmount = " + (object) loanAmount + ", ");
      dbQueryBuilder2.AppendLine("        OnrpStartDate = '" + (object) onrpStartDate.Date + "' ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.AppendLine("    END ");
      dbQueryBuilder2.AppendLine("ELSE");
      dbQueryBuilder2.AppendLine("BEGIN");
      dbQueryBuilder2.AppendLine("    INSERT INTO OnrpPeriodAccruedAmount");
      dbQueryBuilder2.AppendLine("    VALUES('" + ((int) channel).ToString() + "', ");
      switch (channel - 1)
      {
        case LoanChannel.None:
          dbQueryBuilder2.AppendLine("null, " + entityId + ", ");
          break;
        case LoanChannel.BankedRetail:
          dbQueryBuilder2.AppendLine("'" + entityId + "', null, ");
          break;
        case LoanChannel.Brokered:
          dbQueryBuilder2.AppendLine("'" + entityId + "', null, ");
          break;
      }
      dbQueryBuilder2.AppendLine("'" + (object) onrpStartDate.Date + "', ");
      dbQueryBuilder2.AppendLine(loanAmount.ToString() + ")");
      dbQueryBuilder2.AppendLine("END ");
      dbQueryBuilder2.Execute();
    }

    public static void DeleteOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      bool checkUseGlobal)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (channel != LoanChannel.None)
      {
        dbQueryBuilder1.AppendLine("WHERE EntityChannel = '" + ((int) channel).ToString() + "' ");
        if (!string.IsNullOrEmpty(entityId))
        {
          switch (channel)
          {
            case LoanChannel.BankedRetail:
              dbQueryBuilder1.AppendLine("AND BranchOrgId = " + entityId + " ");
              break;
            case LoanChannel.BankedWholesale:
              dbQueryBuilder1.AppendLine("AND ExternalCompanyId = '" + entityId + "' ");
              break;
            case LoanChannel.Correspondent:
              dbQueryBuilder1.AppendLine("AND ExternalCompanyId = '" + entityId + "' ");
              break;
          }
        }
        else if (checkUseGlobal)
        {
          switch (channel)
          {
            case LoanChannel.BankedRetail:
              dbQueryBuilder1.AppendLine("AND BranchOrgId in (SELECT oid FROM org_chart WHERE onrp_use_channel_default = 1) ");
              break;
            case LoanChannel.BankedWholesale:
              dbQueryBuilder1.AppendLine("AND ExternalCompanyId in (SELECT c.ExternalId FROM ExternalOriginatorManagement c INNER JOIN ExternalOrgONRP o ON c.oid = o.externalorgid WHERE BrokerUseChannelDefault = 1) ");
              break;
            case LoanChannel.Correspondent:
              dbQueryBuilder1.AppendLine("AND ExternalCompanyId in (SELECT c.ExternalId FROM ExternalOriginatorManagement c INNER JOIN ExternalOrgONRP o ON c.oid = o.externalorgid WHERE CorrespondentUseChannelDefault = 1) ");
              break;
          }
        }
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("DELETE FROM OnrpPeriodAccruedAmount ");
      dbQueryBuilder2.Append(dbQueryBuilder1.ToString());
      dbQueryBuilder2.Execute();
    }
  }
}
