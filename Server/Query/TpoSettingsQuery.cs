// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.TpoSettingsQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class TpoSettingsQuery(UserInfo currentUser) : QueryEngine(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new TpoSettingsFieldTranslator())
  {
    public override string PrimaryKeyIdentifier => "TpoSettingsDetails.oid";

    public override string UserAccessQueryKeyColumnName => throw new NotImplementedException();

    public override string PrimaryKeyTableIdentifier => "TpoSettingsDetails";

    public override List<string> ParentTables => new List<string>();

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization) => "";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "commitments":
          return new FieldSource("commitments", " left join FN_GetTpoCommitments() commitments on tposettingsdetails.oid = commitments.oid ");
        case "companysettings":
          return new FieldSource("companysettings", "  left join (SELECT settingCode + ' - ' + settingValue as EPPSPricingGroup, SettingId FROM [ExternalOrgSettingValues]) CompanySettings on TpoSettingsDetails.EPPSPriceGroup = CompanySettings.settingId");
        case "eppspricegroupbroker":
          return new FieldSource("companysettingsbroker", "  left join (SELECT settingCode + ' - ' + settingValue as EPPSPriceGroupBroker, SettingId FROM [ExternalOrgSettingValues]) CompanySettingsBroker on TpoSettingsDetails.EPPSPriceGroupBroker = CompanySettingsBroker.settingId");
        case "eppspricegroupdel":
          return new FieldSource("companysettingsdel", "  left join (SELECT settingCode + ' - ' + settingValue as EPPSPriceGroupDel, SettingId FROM [ExternalOrgSettingValues]) CompanySettingsDel on TpoSettingsDetails.EPPSPriceGroupDel = CompanySettingsDel.settingId");
        case "eppspricegroupnondel":
          return new FieldSource("companysettingsnondel", "  left join (SELECT settingCode + ' - ' + settingValue as EPPSPriceGroupNonDel, SettingId FROM [ExternalOrgSettingValues]) CompanySettingsNonDel on TpoSettingsDetails.EPPSPriceGroupNonDel = CompanySettingsNonDel.settingId");
        case "persona":
        case "users":
          return new FieldSource("persona", " left join (\r\n\t\t\t\t\t\t\t\t\tSELECT\r\n\t\t                            userid,\r\n\t\t                            STUFF\r\n\t\t                            ((select DISTINCT '+' + P.personaName from users U inner join UserPersona UP on U.userid = up.userid\r\n\t\t\t                            inner join Personas P on up.personaID = p.personaID where USERS.userid = U.userid for xml Path('')),1,1,'')  AS Persona\r\n\t\t                            FROM users AS USERS\r\n\t\t                            GROUP BY userid \r\n\t\t                            ) PERSONA on TPOsettingsdetails.PrimarySalesRepUserId = PERSONA.userid LEFT JOIN users USERS on TPOsettingsdetails.PrimarySalesRepUserId = USERS.userid");
        case "tpomanager":
          return new FieldSource("tpomanager", "  left join (select (First_name + ' ' + Last_name) as ManagerName,external_userid from ExternalUsers) TPOManager on TpoSettingsDetails.ManagerUserID = TPOManager.external_userid");
        default:
          return (FieldSource) null;
      }
    }

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      throw new NotImplementedException();
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      throw new NotImplementedException();
    }
  }
}
