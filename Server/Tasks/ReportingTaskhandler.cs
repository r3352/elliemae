// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.ReportingTaskhandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  internal class ReportingTaskhandler : ITaskHandler
  {
    public void ProcessTask(ServerTask taskInfo)
    {
      foreach (SettingsRptJobInfo settingsRptJob in SettingsReportAccessor.GetSettingsRptJobs(0.ToString()))
      {
        int int16_1 = (int) Convert.ToInt16(settingsRptJob.ReportID);
        string fileGuid = settingsRptJob.FileGuid;
        settingsRptJob.reportFilters = SettingsReportAccessor.getReportFilters(int16_1);
        settingsRptJob.reportParameters = SettingsReportAccessor.getReportParameters(int16_1);
        try
        {
          if (settingsRptJob.Type == SettingsRptJobInfo.jobType.Organization)
          {
            if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
            {
              SettingsReportAccessor.UpdateReportStatus(1, int16_1, "");
              int int16_2 = (int) Convert.ToInt16(settingsRptJob.reportFilters[0]);
              StringBuilder stringBuilder = new StringBuilder("<SettingsData>");
              string str1 = settingsRptJob.reportParameters.ContainsKey("OrganizationDetails") ? settingsRptJob.reportParameters["OrganizationDetails"] : "False";
              string str2 = settingsRptJob.reportParameters.ContainsKey("OrganizationLicenses") ? settingsRptJob.reportParameters["OrganizationLicenses"] : "False";
              string str3 = settingsRptJob.reportParameters.ContainsKey("UserDetailsAll") ? settingsRptJob.reportParameters["UserDetailsAll"] : "False";
              string str4 = settingsRptJob.reportParameters.ContainsKey("UserDetailsByOrg") ? settingsRptJob.reportParameters["UserDetailsByOrg"] : "False";
              string str5 = settingsRptJob.reportParameters.ContainsKey("UserLicenses") ? settingsRptJob.reportParameters["UserLicenses"] : "False";
              if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
              {
                if (str1.Equals("True") || str2.Equals("True") || str4.Equals("True"))
                {
                  string orgXml = SettingsRptXmlHelper.createOrgXML(int16_2, settingsRptJob.reportParameters, int16_1.ToString());
                  if (!orgXml.Equals("CANCELLED"))
                    stringBuilder.Append(orgXml);
                  else
                    continue;
                }
                if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
                {
                  if (str3.Equals("True") || str4.Equals("True") || str5.Equals("True"))
                  {
                    string usersXml = SettingsRptXmlHelper.createUsersXml(int16_2, settingsRptJob.reportParameters, int16_1.ToString());
                    if (!usersXml.Equals("CANCELLED"))
                      stringBuilder.Append(usersXml);
                    else
                      continue;
                  }
                  stringBuilder.Append("</SettingsData>");
                  if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
                  {
                    SettingsReportAccessor.UpdateJobWithXML(stringBuilder.ToString(), fileGuid);
                    SettingsReportAccessor.UpdateReportStatus(2, int16_1, "Successfull");
                  }
                }
              }
            }
          }
          else if (settingsRptJob.Type == SettingsRptJobInfo.jobType.Persona)
          {
            if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
            {
              SettingsReportAccessor.UpdateReportStatus(1, int16_1, "");
              string personasXml = SettingsRptXmlHelper.createPersonasXML(settingsRptJob.reportFilters, settingsRptJob.reportParameters, int16_1.ToString());
              if (!personasXml.Equals("CANCELLED"))
              {
                SettingsReportAccessor.UpdateJobWithXML(personasXml.ToString(), fileGuid);
                SettingsReportAccessor.UpdateReportStatus(2, int16_1, "Successfull");
              }
            }
          }
          else if (settingsRptJob.Type == SettingsRptJobInfo.jobType.UserGroups)
          {
            if (!SettingsReportAccessor.CancelJob(int16_1.ToString()))
            {
              SettingsReportAccessor.UpdateReportStatus(1, int16_1, "");
              string userGroupXml = SettingsRptXmlHelper.createUserGroupXML(settingsRptJob.reportFilters, settingsRptJob.reportParameters, int16_1.ToString());
              if (!userGroupXml.Equals("CANCELLED"))
              {
                SettingsReportAccessor.UpdateJobWithXML(userGroupXml.ToString(), fileGuid);
                SettingsReportAccessor.UpdateReportStatus(2, int16_1, "Successfull");
              }
            }
          }
        }
        catch (Exception ex)
        {
          string message = "Failed to generate report for " + (object) int16_1 + ": " + ex.Message;
          SettingsReportAccessor.UpdateReportStatus(3, int16_1, message);
          if (EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog != null)
            EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog.Write(message, "ReportingTaskHandler");
          if (EllieMae.EMLite.Server.ServerGlobals.TraceLog != null)
            EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteError("ReportingTaskHandler", message);
        }
      }
    }
  }
}
