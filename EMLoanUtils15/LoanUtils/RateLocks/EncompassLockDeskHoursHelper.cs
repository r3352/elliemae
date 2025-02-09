// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.EncompassLockDeskHoursHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public class EncompassLockDeskHoursHelper : ILockDeskHoursHelper
  {
    private SessionObjects sessionObjs;
    private LoanDataMgr loanDataMgr;
    private IClientSession clientSession;
    private const string className = "EncompassLockDeskHoursHelper�";
    protected static string sw = Tracing.SwOutsideLoan;
    private static DateTime? _QAOverrideServerEasternTime = new DateTime?();
    private static DateTime _QAOverrideServerEasternTime_ExpireTime = DateTime.MinValue;

    public EncompassLockDeskHoursHelper(
      IClientSession clientSession,
      SessionObjects sessionObjs,
      LoanDataMgr loanDataMgr)
    {
      this.clientSession = clientSession;
      this.sessionObjs = sessionObjs;
      this.loanDataMgr = loanDataMgr;
    }

    public void Log(string sw, string className, TraceLevel l, string msg)
    {
      Tracing.Log(sw, className, l, msg);
    }

    public DateTime GetServerEasternTime()
    {
      if (EncompassLockDeskHoursHelper.QAOverrideServerEasternTime.HasValue)
        return EncompassLockDeskHoursHelper.QAOverrideServerEasternTime.Value;
      DateTime serverUtcTime = LockDeskHoursUtils.GetServerUtcTime(this.clientSession);
      System.TimeZoneInfo destinationTimeZone;
      try
      {
        destinationTimeZone = System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
      }
      catch (Exception ex)
      {
        destinationTimeZone = System.TimeZoneInfo.Local;
      }
      return System.TimeZoneInfo.ConvertTimeFromUtc(serverUtcTime, destinationTimeZone);
    }

    public LockDeskHoursInfo GetLockDeskHoursSettings(LoanChannel loanChannel)
    {
      IDictionary dictionary;
      try
      {
        dictionary = this.sessionObjs.ServerManager.GetServerSettings("Policies");
      }
      catch (NotImplementedException ex)
      {
        dictionary = this.sessionObjs.StartupInfo.PolicySettings;
      }
      LockDeskHoursInfo deskHoursSettings = (LockDeskHoursInfo) null;
      switch (loanChannel)
      {
        case LoanChannel.None:
          deskHoursSettings = new LockDeskHoursInfo();
          if ((bool) dictionary[(object) "Policies.ENABLEALLCHANNEL"])
          {
            deskHoursSettings.IsEncompassLockDeskHoursEnabled = (bool) dictionary[(object) "Policies.EnableLockDeskSCHEDULE"];
            deskHoursSettings.LockDeskHoursMessage = dictionary[(object) "Policies.RETLDHRMSG"].ToString();
            deskHoursSettings.IsLockDeskShutdown = (bool) dictionary[(object) "Policies.RETLDSHUTDOWN"];
            deskHoursSettings.AllowActiveRelockRequests = (bool) dictionary[(object) "Policies.RETLDALLOWACTIVERELOCK"];
            deskHoursSettings.LockDeskShutdownMessage = dictionary[(object) "Policies.RETLDSHUTDOWNMSG"].ToString();
            deskHoursSettings.LockDeskStartTime = Utils.ParseDate(dictionary[(object) "Policies.RETLDSTRTIME"], DateTime.MinValue).TimeOfDay;
            deskHoursSettings.LockDeskEndTime = Utils.ParseDate(dictionary[(object) "Policies.RETLDENDTIME"], DateTime.MinValue).TimeOfDay;
            deskHoursSettings.LockDeskSatHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDRETSAT"];
            deskHoursSettings.LockDeskStartTimeSat = Utils.ParseDate(dictionary[(object) "Policies.RETLDSATSTRTIME"], DateTime.MinValue).TimeOfDay;
            deskHoursSettings.LockDeskEndTimeSat = Utils.ParseDate(dictionary[(object) "Policies.RETLDSATENDTIME"], DateTime.MinValue).TimeOfDay;
            deskHoursSettings.LockDeskSunHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDRETSUN"];
            deskHoursSettings.LockDeskStartTimeSun = Utils.ParseDate(dictionary[(object) "Policies.RETLDSUNSTRTIME"], DateTime.MinValue).TimeOfDay;
            deskHoursSettings.LockDeskEndTimeSun = Utils.ParseDate(dictionary[(object) "Policies.RETLDSUNENDTIME"], DateTime.MinValue).TimeOfDay;
            break;
          }
          deskHoursSettings.IsEncompassLockDeskHoursEnabled = (bool) dictionary[(object) "Policies.EnableLockDeskSCHEDULE"];
          break;
        case LoanChannel.BankedRetail:
          deskHoursSettings = new LockDeskHoursInfo();
          deskHoursSettings.IsEncompassLockDeskHoursEnabled = (bool) dictionary[(object) "Policies.EnableLockDeskSCHEDULE"];
          deskHoursSettings.LockDeskHoursMessage = dictionary[(object) "Policies.RETLDHRMSG"].ToString();
          deskHoursSettings.IsLockDeskShutdown = (bool) dictionary[(object) "Policies.RETLDSHUTDOWN"];
          deskHoursSettings.AllowActiveRelockRequests = (bool) dictionary[(object) "Policies.RETLDALLOWACTIVERELOCK"];
          deskHoursSettings.LockDeskShutdownMessage = dictionary[(object) "Policies.RETLDSHUTDOWNMSG"].ToString();
          deskHoursSettings.LockDeskStartTime = Utils.ParseDate(dictionary[(object) "Policies.RETLDSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTime = Utils.ParseDate(dictionary[(object) "Policies.RETLDENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSatHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDRETSAT"];
          deskHoursSettings.LockDeskStartTimeSat = Utils.ParseDate(dictionary[(object) "Policies.RETLDSATSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSat = Utils.ParseDate(dictionary[(object) "Policies.RETLDSATENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSunHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDRETSUN"];
          deskHoursSettings.LockDeskStartTimeSun = Utils.ParseDate(dictionary[(object) "Policies.RETLDSUNSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSun = Utils.ParseDate(dictionary[(object) "Policies.RETLDSUNENDTIME"], DateTime.MinValue).TimeOfDay;
          break;
        case LoanChannel.BankedWholesale:
          deskHoursSettings = new LockDeskHoursInfo();
          deskHoursSettings.IsEncompassLockDeskHoursEnabled = (bool) dictionary[(object) "Policies.EnableLockDeskSCHEDULE"];
          deskHoursSettings.LockDeskHoursMessage = dictionary[(object) "Policies.BROKERLDHRMSG"].ToString();
          deskHoursSettings.IsLockDeskShutdown = (bool) dictionary[(object) "Policies.BROKERLDSHUTDOWN"];
          deskHoursSettings.AllowActiveRelockRequests = (bool) dictionary[(object) "Policies.BROKERLDALLOWACTIVERELOCK"];
          deskHoursSettings.LockDeskShutdownMessage = dictionary[(object) "Policies.BROKERLDSHUTDOWNMSG"].ToString();
          deskHoursSettings.LockDeskStartTime = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTime = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSatHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDBROKERSAT"];
          deskHoursSettings.LockDeskStartTimeSat = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDSATSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSat = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDSATENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSunHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDBROKERSUN"];
          deskHoursSettings.LockDeskStartTimeSun = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDSUNSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSun = Utils.ParseDate(dictionary[(object) "Policies.BROKERLDSUNENDTIME"], DateTime.MinValue).TimeOfDay;
          break;
        case LoanChannel.Brokered:
          deskHoursSettings = new LockDeskHoursInfo();
          deskHoursSettings.IsEncompassLockDeskHoursEnabled = false;
          break;
        case LoanChannel.Correspondent:
          deskHoursSettings = new LockDeskHoursInfo();
          deskHoursSettings.IsEncompassLockDeskHoursEnabled = (bool) dictionary[(object) "Policies.EnableLockDeskSCHEDULE"];
          deskHoursSettings.LockDeskHoursMessage = dictionary[(object) "Policies.CORLDHRMSG"].ToString();
          deskHoursSettings.IsLockDeskShutdown = (bool) dictionary[(object) "Policies.CORLDSHUTDOWN"];
          deskHoursSettings.AllowActiveRelockRequests = (bool) dictionary[(object) "Policies.CORLDALLOWACTIVERELOCK"];
          deskHoursSettings.LockDeskShutdownMessage = dictionary[(object) "Policies.CORLDSHUTDOWNMSG"].ToString();
          deskHoursSettings.LockDeskStartTime = Utils.ParseDate(dictionary[(object) "Policies.CORLDSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTime = Utils.ParseDate(dictionary[(object) "Policies.CORLDENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSatHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDCORSAT"];
          deskHoursSettings.LockDeskStartTimeSat = Utils.ParseDate(dictionary[(object) "Policies.CORLDSATSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSat = Utils.ParseDate(dictionary[(object) "Policies.CORLDSATENDTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskSunHoursEnabled = (bool) dictionary[(object) "Policies.ENABLELDCORSUN"];
          deskHoursSettings.LockDeskStartTimeSun = Utils.ParseDate(dictionary[(object) "Policies.CORLDSUNSTRTIME"], DateTime.MinValue).TimeOfDay;
          deskHoursSettings.LockDeskEndTimeSun = Utils.ParseDate(dictionary[(object) "Policies.CORLDSUNENDTIME"], DateTime.MinValue).TimeOfDay;
          break;
      }
      return deskHoursSettings;
    }

    public void GetONRPGlobalSettings(
      out LockDeskOnrpInfo globalRetailSettings,
      out LockDeskOnrpInfo globalWholesaleSettings,
      out LockDeskOnrpInfo globalCorrespondentSettings)
    {
      IDictionary dictionary;
      try
      {
        dictionary = this.sessionObjs.ServerManager.GetServerSettings("Policies");
      }
      catch (NotImplementedException ex)
      {
        dictionary = this.sessionObjs.StartupInfo.PolicySettings;
      }
      globalRetailSettings = new LockDeskOnrpInfo();
      globalRetailSettings.IsContinuousONRPCoverage = dictionary[(object) "Policies.ONRPRetCvrg"].ToString() != "Specify";
      globalRetailSettings.IsONRPEnabled = (bool) dictionary[(object) "Policies.EnableONRPRet"];
      globalRetailSettings.IsNoMaxLimit = (bool) dictionary[(object) "Policies.ONRPNoMaxLimitRet"];
      globalRetailSettings.ONRPLimitAmount = Utils.ToDouble(dictionary[(object) "Policies.ONRPRETDOLLIMIT"].ToString());
      globalRetailSettings.ONRPTolerance = Utils.ParseInt(dictionary[(object) "Policies.ONRPRETDOLTOL"], 0);
      globalRetailSettings.ONRPEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPRETENDTIME"], DateTime.MinValue).TimeOfDay;
      globalRetailSettings.IsWeekendHolidayCoverage = (bool) dictionary[(object) "Policies.ENABLEONRPWHRETCVRG"];
      globalRetailSettings.ONRPMessageAddendum = dictionary[(object) "Policies.ONRPOVERLIMITMSGADDENDUM"].ToString();
      globalRetailSettings.ONRPOverLimitMessage = "Overnight Rate Protection for Loan <Loan Number> exceeded Company limit by $<Dollar Amount>.";
      globalRetailSettings.IsONRPSatEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPRETSAT"];
      globalRetailSettings.IsONRPSunEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPRETSUN"];
      globalRetailSettings.ONRPSatEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPRETSATENDTIME"], DateTime.MinValue).TimeOfDay;
      globalRetailSettings.ONRPSunEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPRETSUNENDTIME"], DateTime.MinValue).TimeOfDay;
      globalWholesaleSettings = new LockDeskOnrpInfo();
      globalWholesaleSettings.IsContinuousONRPCoverage = dictionary[(object) "Policies.ONRPBROKERCVRG"].ToString() != "Specify";
      globalWholesaleSettings.IsONRPEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPBROKER"];
      globalWholesaleSettings.IsNoMaxLimit = (bool) dictionary[(object) "Policies.ONRPNOMAXLIMITBROKER"];
      globalWholesaleSettings.ONRPLimitAmount = Utils.ToDouble(dictionary[(object) "Policies.ONRPBROKERDOLLIMIT"].ToString());
      globalWholesaleSettings.ONRPTolerance = Utils.ParseInt(dictionary[(object) "Policies.ONRPBROKERDOLTOL"], 0);
      globalWholesaleSettings.ONRPEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPBROKERENDTIME"], DateTime.MinValue).TimeOfDay;
      globalWholesaleSettings.IsWeekendHolidayCoverage = (bool) dictionary[(object) "Policies.ENABLEONRPWHBROKERCVRG"];
      globalWholesaleSettings.ONRPMessageAddendum = dictionary[(object) "Policies.ONRPOVERLIMITMSGADDENDUM"].ToString();
      globalWholesaleSettings.ONRPOverLimitMessage = "Overnight Rate Protection for Loan <Loan Number> exceeded Company limit by $<Dollar Amount>.";
      globalWholesaleSettings.IsONRPSatEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPBROKERSAT"];
      globalWholesaleSettings.IsONRPSunEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPBROKERSUN"];
      globalWholesaleSettings.ONRPSatEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPBROKERSATENDTIME"], DateTime.MinValue).TimeOfDay;
      globalWholesaleSettings.ONRPSunEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPBROKERSUNENDTIME"], DateTime.MinValue).TimeOfDay;
      globalCorrespondentSettings = new LockDeskOnrpInfo();
      globalCorrespondentSettings.IsContinuousONRPCoverage = dictionary[(object) "Policies.ONRPCORCVRG"].ToString() != "Specify";
      globalCorrespondentSettings.IsONRPEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPCOR"];
      globalCorrespondentSettings.AllowONRPForCancelledExpiredLocks = (bool) dictionary[(object) "Policies.ONRPCancelledExpiredLocksCOR"];
      globalCorrespondentSettings.IsNoMaxLimit = (bool) dictionary[(object) "Policies.ONRPNOMAXLIMITCOR"];
      globalCorrespondentSettings.ONRPLimitAmount = Utils.ToDouble(dictionary[(object) "Policies.ONRPCORDOLLIMIT"].ToString());
      globalCorrespondentSettings.ONRPTolerance = Utils.ParseInt(dictionary[(object) "Policies.ONRPCORDOLTOL"], 0);
      globalCorrespondentSettings.ONRPEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPCORENDTIME"], DateTime.MinValue).TimeOfDay;
      globalCorrespondentSettings.IsWeekendHolidayCoverage = (bool) dictionary[(object) "Policies.ENABLEONRPWHCORCVRG"];
      globalCorrespondentSettings.ONRPMessageAddendum = dictionary[(object) "Policies.ONRPOVERLIMITMSGADDENDUM"].ToString();
      globalCorrespondentSettings.ONRPOverLimitMessage = "Overnight Rate Protection for Loan <Loan Number> exceeded Company limit by $<Dollar Amount>.";
      globalCorrespondentSettings.IsONRPSatEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPCORSAT"];
      globalCorrespondentSettings.IsONRPSunEnabled = (bool) dictionary[(object) "Policies.ENABLEONRPCORSUN"];
      globalCorrespondentSettings.ONRPSatEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPCORSATENDTIME"], DateTime.MinValue).TimeOfDay;
      globalCorrespondentSettings.ONRPSunEndTime = Utils.ParseDate(dictionary[(object) "Policies.ONRPCORSUNENDTIME"], DateTime.MinValue).TimeOfDay;
    }

    public void GetONRPTPOSettings(
      string tpoId,
      out LockDeskOnrpInfo brokerONRPSettings,
      out LockDeskOnrpInfo correspondentONRPSettings)
    {
      ExternalOrgOnrpSettings onrpSettingsByTpoId = this.sessionObjs.ConfigurationManager.GetExternalOrgOnrpSettingsByTPOId(tpoId);
      LockDeskOnrpInfo lockDeskOnrpInfo1 = (LockDeskOnrpInfo) null;
      LockDeskOnrpInfo lockDeskOnrpInfo2 = (LockDeskOnrpInfo) null;
      if (onrpSettingsByTpoId != null)
      {
        lockDeskOnrpInfo1 = new LockDeskOnrpInfo();
        lockDeskOnrpInfo1.Channel = LoanChannel.BankedWholesale;
        LockDeskOnrpInfo lockDeskOnrpInfo3 = lockDeskOnrpInfo1;
        DateTime dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Broker.ONRPEndTime);
        TimeSpan timeOfDay1 = dateTime.TimeOfDay;
        lockDeskOnrpInfo3.ONRPEndTime = timeOfDay1;
        lockDeskOnrpInfo1.IsONRPEnabled = onrpSettingsByTpoId.Broker.EnableONRP;
        lockDeskOnrpInfo1.IsUseChannelDefaults = onrpSettingsByTpoId.Broker.UseChannelDefault;
        lockDeskOnrpInfo1.IsNoMaxLimit = onrpSettingsByTpoId.Broker.MaximumLimit;
        lockDeskOnrpInfo1.IsWeekendHolidayCoverage = onrpSettingsByTpoId.Broker.WeekendHolidayCoverage;
        lockDeskOnrpInfo1.ONRPTolerance = onrpSettingsByTpoId.Broker.Tolerance;
        lockDeskOnrpInfo1.IsContinuousONRPCoverage = onrpSettingsByTpoId.Broker.ContinuousCoverage;
        lockDeskOnrpInfo1.ONRPLimitAmount = onrpSettingsByTpoId.Broker.DollarLimit;
        lockDeskOnrpInfo1.EntityId = tpoId;
        lockDeskOnrpInfo1.IsONRPSatEnabled = onrpSettingsByTpoId.Broker.EnableSatONRP;
        lockDeskOnrpInfo1.IsONRPSunEnabled = onrpSettingsByTpoId.Broker.EnableSunONRP;
        LockDeskOnrpInfo lockDeskOnrpInfo4 = lockDeskOnrpInfo1;
        dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Broker.ONRPSatEndTime);
        TimeSpan timeOfDay2 = dateTime.TimeOfDay;
        lockDeskOnrpInfo4.ONRPSatEndTime = timeOfDay2;
        LockDeskOnrpInfo lockDeskOnrpInfo5 = lockDeskOnrpInfo1;
        dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Broker.ONRPSunEndTime);
        TimeSpan timeOfDay3 = dateTime.TimeOfDay;
        lockDeskOnrpInfo5.ONRPSunEndTime = timeOfDay3;
        lockDeskOnrpInfo2 = new LockDeskOnrpInfo();
        lockDeskOnrpInfo2.Channel = LoanChannel.Correspondent;
        LockDeskOnrpInfo lockDeskOnrpInfo6 = lockDeskOnrpInfo2;
        dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Correspondent.ONRPEndTime);
        TimeSpan timeOfDay4 = dateTime.TimeOfDay;
        lockDeskOnrpInfo6.ONRPEndTime = timeOfDay4;
        lockDeskOnrpInfo2.IsONRPEnabled = onrpSettingsByTpoId.Correspondent.EnableONRP;
        lockDeskOnrpInfo2.AllowONRPForCancelledExpiredLocks = onrpSettingsByTpoId.Correspondent.AllowONRPForCancelledExpiredLocks;
        lockDeskOnrpInfo2.IsUseChannelDefaults = onrpSettingsByTpoId.Correspondent.UseChannelDefault;
        lockDeskOnrpInfo2.IsNoMaxLimit = onrpSettingsByTpoId.Correspondent.MaximumLimit;
        lockDeskOnrpInfo2.IsWeekendHolidayCoverage = onrpSettingsByTpoId.Correspondent.WeekendHolidayCoverage;
        lockDeskOnrpInfo2.ONRPTolerance = onrpSettingsByTpoId.Correspondent.Tolerance;
        lockDeskOnrpInfo2.IsContinuousONRPCoverage = onrpSettingsByTpoId.Correspondent.ContinuousCoverage;
        lockDeskOnrpInfo2.ONRPLimitAmount = onrpSettingsByTpoId.Correspondent.DollarLimit;
        lockDeskOnrpInfo2.EntityId = tpoId;
        lockDeskOnrpInfo2.IsONRPSatEnabled = onrpSettingsByTpoId.Correspondent.EnableSatONRP;
        lockDeskOnrpInfo2.IsONRPSunEnabled = onrpSettingsByTpoId.Correspondent.EnableSunONRP;
        LockDeskOnrpInfo lockDeskOnrpInfo7 = lockDeskOnrpInfo2;
        dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Correspondent.ONRPSatEndTime);
        TimeSpan timeOfDay5 = dateTime.TimeOfDay;
        lockDeskOnrpInfo7.ONRPSatEndTime = timeOfDay5;
        LockDeskOnrpInfo lockDeskOnrpInfo8 = lockDeskOnrpInfo2;
        dateTime = ONRPEntitySettings.ConverToDateTime(onrpSettingsByTpoId.Correspondent.ONRPSunEndTime);
        TimeSpan timeOfDay6 = dateTime.TimeOfDay;
        lockDeskOnrpInfo8.ONRPSunEndTime = timeOfDay6;
      }
      brokerONRPSettings = lockDeskOnrpInfo1;
      correspondentONRPSettings = lockDeskOnrpInfo2;
    }

    public void GetONRPRetailSettings(out LockDeskOnrpInfo branchSettings)
    {
      LockDeskOnrpInfo lockDeskOnrpInfo1 = new LockDeskOnrpInfo();
      lockDeskOnrpInfo1.Channel = LoanChannel.BankedRetail;
      MilestoneLog milestoneById = this.loanDataMgr.LoanData.GetLogList().GetMilestoneByID("1");
      ONRPEntitySettings onrpEntitySettings = new ONRPEntitySettings();
      bool flag = false;
      if (milestoneById != null)
      {
        string loanAssociateId = milestoneById.LoanAssociateID;
        if (loanAssociateId == string.Empty)
        {
          flag = true;
          this.Log(EncompassLockDeskHoursHelper.sw, nameof (EncompassLockDeskHoursHelper), TraceLevel.Verbose, "GetONRPRetailSettings(): UserId missing from File Creator field.");
        }
        else
        {
          UserInfo user = this.sessionObjs.OrganizationManager.GetUser(loanAssociateId);
          if (user == (UserInfo) null)
          {
            flag = true;
            this.Log(EncompassLockDeskHoursHelper.sw, nameof (EncompassLockDeskHoursHelper), TraceLevel.Verbose, string.Format("GetONRPRetailSettings(): User{0} not found.", (object) loanAssociateId));
          }
          else
          {
            OrgInfo organizationWithOnrp = this.sessionObjs.OrganizationManager.GetFirstOrganizationWithONRP(user.OrgId);
            if (organizationWithOnrp == null)
            {
              flag = true;
              this.Log(EncompassLockDeskHoursHelper.sw, nameof (EncompassLockDeskHoursHelper), TraceLevel.Verbose, string.Format("GetONRPRetailSettings(): Org not found for User{0}.", (object) loanAssociateId));
            }
            else
              onrpEntitySettings = organizationWithOnrp.ONRPRetailBranchSettings;
          }
        }
      }
      else
      {
        this.Log(EncompassLockDeskHoursHelper.sw, nameof (EncompassLockDeskHoursHelper), TraceLevel.Verbose, "GetONRPRetailSettings(): File Created Milestone Missing.");
        flag = true;
      }
      if (flag)
        onrpEntitySettings = this.loanDataMgr.SystemConfiguration.UserOrganiation.ONRPRetailBranchSettings;
      lockDeskOnrpInfo1.IsContinuousONRPCoverage = onrpEntitySettings.ContinuousCoverage;
      lockDeskOnrpInfo1.ONRPLimitAmount = onrpEntitySettings.DollarLimit;
      lockDeskOnrpInfo1.IsONRPEnabled = onrpEntitySettings.EnableONRP;
      lockDeskOnrpInfo1.AllowONRPForCancelledExpiredLocks = onrpEntitySettings.AllowONRPForCancelledExpiredLocks;
      lockDeskOnrpInfo1.ONRPEndTime = ONRPEntitySettings.ConverToDateTime(onrpEntitySettings.ONRPEndTime).TimeOfDay;
      lockDeskOnrpInfo1.ONRPTolerance = onrpEntitySettings.Tolerance;
      lockDeskOnrpInfo1.IsUseChannelDefaults = onrpEntitySettings.UseChannelDefault;
      lockDeskOnrpInfo1.IsWeekendHolidayCoverage = onrpEntitySettings.WeekendHolidayCoverage;
      lockDeskOnrpInfo1.IsNoMaxLimit = onrpEntitySettings.MaximumLimit;
      lockDeskOnrpInfo1.EntityId = this.loanDataMgr.SystemConfiguration.UserOrganiation.Oid.ToString();
      lockDeskOnrpInfo1.IsONRPSatEnabled = onrpEntitySettings.EnableSatONRP;
      lockDeskOnrpInfo1.IsONRPSunEnabled = onrpEntitySettings.EnableSunONRP;
      LockDeskOnrpInfo lockDeskOnrpInfo2 = lockDeskOnrpInfo1;
      DateTime dateTime = ONRPEntitySettings.ConverToDateTime(onrpEntitySettings.ONRPSatEndTime);
      TimeSpan timeOfDay1 = dateTime.TimeOfDay;
      lockDeskOnrpInfo2.ONRPSatEndTime = timeOfDay1;
      LockDeskOnrpInfo lockDeskOnrpInfo3 = lockDeskOnrpInfo1;
      dateTime = ONRPEntitySettings.ConverToDateTime(onrpEntitySettings.ONRPSunEndTime);
      TimeSpan timeOfDay2 = dateTime.TimeOfDay;
      lockDeskOnrpInfo3.ONRPSunEndTime = timeOfDay2;
      branchSettings = lockDeskOnrpInfo1;
    }

    public double GetOnrpAccruedDollarAmount(
      LoanChannel channel,
      string entityId,
      DateTime onrpStartDate)
    {
      return this.sessionObjs.OverNightRateProtectionManager.GetOnrpPeriodAccruedAmount(channel, entityId, onrpStartDate);
    }

    public static DateTime? QAOverrideServerEasternTime
    {
      get
      {
        if (!EncompassLockDeskHoursHelper._QAOverrideServerEasternTime.HasValue)
          return new DateTime?();
        if (DateTime.Now < EncompassLockDeskHoursHelper._QAOverrideServerEasternTime_ExpireTime)
          return EncompassLockDeskHoursHelper._QAOverrideServerEasternTime;
        EncompassLockDeskHoursHelper._QAOverrideServerEasternTime = new DateTime?();
        EncompassLockDeskHoursHelper._QAOverrideServerEasternTime_ExpireTime = DateTime.MinValue;
        return EncompassLockDeskHoursHelper._QAOverrideServerEasternTime;
      }
      set
      {
        EncompassLockDeskHoursHelper._QAOverrideServerEasternTime = value;
        EncompassLockDeskHoursHelper._QAOverrideServerEasternTime_ExpireTime = DateTime.Now.AddSeconds(5.0);
      }
    }

    public bool IsFirstLockRequest()
    {
      return ((IEnumerable<LockConfirmLog>) this.loanDataMgr.LoanData.GetLogList().GetAllConfirmLocks()).Count<LockConfirmLog>() == 0;
    }

    public LoanChannel GetLoanChannel()
    {
      string field = this.loanDataMgr.LoanData.GetField("2626");
      if (string.IsNullOrEmpty(field))
      {
        Hashtable companySettings = this.sessionObjs.ConfigurationManager.GetCompanySettings("NMLS");
        if (companySettings != null && companySettings.Contains((object) "ChannelMap/"))
          field = companySettings[(object) "ChannelMap/"].ToString();
      }
      return LockDeskHoursUtils.GetEnumChannel(field);
    }
  }
}
