// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SettingsRptXmlHelper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class SettingsRptXmlHelper
  {
    private static void genOrgInfoXML(XmlElement root, OrgInfo org_info)
    {
      ElementWriter elementWriter = new ElementWriter(root);
      root.SetAttribute("city", org_info.CompanyAddress.City);
      root.SetAttribute("state", org_info.CompanyAddress.State);
      root.SetAttribute("zip", org_info.CompanyAddress.Zip);
      root.SetAttribute("phone", org_info.CompanyPhone);
      root.SetAttribute("fax", org_info.CompanyFax);
      root.SetAttribute("license_dbaname1", org_info.DBAName1);
      root.SetAttribute("license_dbaname2", org_info.DBAName2);
      root.SetAttribute("license_dbaname3", org_info.DBAName3);
      root.SetAttribute("license_dbaname4", org_info.DBAName4);
      root.SetAttribute("org_code", org_info.OrgCode);
      elementWriter.Append("company_name", org_info.CompanyName);
      elementWriter.Append("address1", org_info.CompanyAddress.Street1);
      elementWriter.Append("address2", org_info.CompanyAddress.Street2);
    }

    private static void genOnrpInfoXML(
      XmlElement root,
      OrgInfo org_info,
      IDictionary settings,
      bool useChannelDefault)
    {
      if (useChannelDefault)
      {
        if (settings.Contains((object) "POLICIES.ONRPRETDOLLIMIT"))
        {
          root.SetAttribute("onrp_dollar_limit", settings[(object) "POLICIES.ONRPRETDOLLIMIT"].ToString());
        }
        else
        {
          XmlElement xmlElement = root;
          double dollarLimit = org_info.ONRPRetailBranchSettings.DollarLimit;
          string str;
          if (!dollarLimit.Equals((object) null))
          {
            dollarLimit = org_info.ONRPRetailBranchSettings.DollarLimit;
            str = dollarLimit.ToString();
          }
          else
            str = "";
          xmlElement.SetAttribute("onrp_dollar_limit", str);
        }
        if (settings.Contains((object) "POLICIES.ONRPRETDOLTOL"))
        {
          root.SetAttribute("onrp_tolerance", settings[(object) "POLICIES.ONRPRETDOLTOL"].ToString());
        }
        else
        {
          XmlElement xmlElement = root;
          int tolerance = org_info.ONRPRetailBranchSettings.Tolerance;
          string str;
          if (!tolerance.Equals((object) null))
          {
            tolerance = org_info.ONRPRetailBranchSettings.Tolerance;
            str = tolerance.ToString();
          }
          else
            str = "";
          xmlElement.SetAttribute("onrp_tolerance", str);
        }
      }
      else
      {
        XmlElement xmlElement1 = root;
        double dollarLimit = org_info.ONRPRetailBranchSettings.DollarLimit;
        string str1;
        if (!dollarLimit.Equals((object) null))
        {
          dollarLimit = org_info.ONRPRetailBranchSettings.DollarLimit;
          str1 = dollarLimit.ToString();
        }
        else
          str1 = "";
        xmlElement1.SetAttribute("onrp_dollar_limit", str1);
        XmlElement xmlElement2 = root;
        int tolerance = org_info.ONRPRetailBranchSettings.Tolerance;
        string str2;
        if (!tolerance.Equals((object) null))
        {
          tolerance = org_info.ONRPRetailBranchSettings.Tolerance;
          str2 = tolerance.ToString();
        }
        else
          str2 = "";
        xmlElement2.SetAttribute("onrp_tolerance", str2);
      }
      XmlElement xmlElement3 = root;
      bool flag = org_info.ONRPRetailBranchSettings.EnableONRP;
      string str3;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.EnableONRP;
        str3 = flag.ToString();
      }
      else
        str3 = "";
      xmlElement3.SetAttribute("onrp_enable", str3);
      XmlElement xmlElement4 = root;
      flag = org_info.ONRPRetailBranchSettings.UseChannelDefault;
      string str4;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.UseChannelDefault;
        str4 = flag.ToString();
      }
      else
        str4 = "";
      xmlElement4.SetAttribute("onrp_use_channel_default", str4);
      XmlElement xmlElement5 = root;
      flag = org_info.ONRPRetailBranchSettings.ContinuousCoverage;
      string str5;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.ContinuousCoverage;
        str5 = flag.ToString();
      }
      else
        str5 = "";
      xmlElement5.SetAttribute("onrp_continuous_coverage", str5);
      XmlElement xmlElement6 = root;
      flag = org_info.ONRPRetailBranchSettings.WeekendHolidayCoverage;
      string str6;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.WeekendHolidayCoverage;
        str6 = flag.ToString();
      }
      else
        str6 = "";
      xmlElement6.SetAttribute("onrp_weekend_holiday_coverage", str6);
      XmlElement xmlElement7 = root;
      flag = org_info.ONRPRetailBranchSettings.UseChannelDefault;
      string str7;
      if (!flag.Equals((object) null))
      {
        flag = !org_info.ONRPRetailBranchSettings.UseChannelDefault;
        str7 = flag.ToString();
      }
      else
        str7 = "";
      xmlElement7.SetAttribute("onrp_settings", str7);
      XmlElement xmlElement8 = root;
      flag = org_info.ONRPRetailBranchSettings.MaximumLimit;
      string str8;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.MaximumLimit;
        str8 = flag.ToString();
      }
      else
        str8 = "";
      xmlElement8.SetAttribute("onrp_maximum_limit", str8);
      root.SetAttribute("onrp_start_time", org_info.ONRPRetailBranchSettings.ONRPStartTime);
      root.SetAttribute("onrp_end_time", org_info.ONRPRetailBranchSettings.ONRPEndTime);
      XmlElement xmlElement9 = root;
      flag = org_info.ONRPRetailBranchSettings.UseParentInfo;
      string str9;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.UseParentInfo;
        str9 = flag.ToString();
      }
      else
        str9 = "";
      xmlElement9.SetAttribute("inheritParentonrp", str9);
      XmlElement xmlElement10 = root;
      flag = org_info.ONRPRetailBranchSettings.EnableSatONRP;
      string str10;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.EnableSatONRP;
        str10 = flag.ToString();
      }
      else
        str10 = "";
      xmlElement10.SetAttribute("onrp_sat_enable", str10);
      XmlElement xmlElement11 = root;
      flag = org_info.ONRPRetailBranchSettings.EnableSunONRP;
      string str11;
      if (!flag.Equals((object) null))
      {
        flag = org_info.ONRPRetailBranchSettings.EnableSunONRP;
        str11 = flag.ToString();
      }
      else
        str11 = "";
      xmlElement11.SetAttribute("onrp_sun_enable", str11);
      root.SetAttribute("onrp_sat_start_time", org_info.ONRPRetailBranchSettings.ONRPSatStartTime);
      root.SetAttribute("onrp_sat_end_time", org_info.ONRPRetailBranchSettings.ONRPSatEndTime);
      root.SetAttribute("onrp_sun_start_time", org_info.ONRPRetailBranchSettings.ONRPSunStartTime);
      root.SetAttribute("onrp_sun_end_time", org_info.ONRPRetailBranchSettings.ONRPSunEndTime);
    }

    private static void genCompPlansXML(XmlElement root, OrgInfo org_info)
    {
      XmlElement xmlElement1 = new ElementWriter(root).Append("CompPlans");
      xmlElement1.SetAttribute("use_parent_info", org_info.LOCompHistoryList.UseParentInfo.Equals((object) null) ? "" : org_info.LOCompHistoryList.UseParentInfo.ToString());
      foreach (LoanCompHistory currentAndFuturePlan in org_info.LOCompHistoryList.GetCurrentAndFuturePlans(DateTime.MinValue))
      {
        XmlElement element = xmlElement1.OwnerDocument.CreateElement("CompPlan");
        ElementWriter elementWriter = new ElementWriter(element);
        element.SetAttribute("compplanid", currentAndFuturePlan.Id);
        element.SetAttribute("name", currentAndFuturePlan.PlanName);
        element.SetAttribute("type", currentAndFuturePlan.Type);
        element.SetAttribute("status", currentAndFuturePlan.Status);
        XmlElement xmlElement2 = element;
        DateTime dateTime = currentAndFuturePlan.StartDate;
        string str1;
        if (!dateTime.Equals((object) null))
        {
          dateTime = currentAndFuturePlan.StartDate;
          str1 = dateTime.ToString();
        }
        else
          str1 = "";
        xmlElement2.SetAttribute("activationDate", str1);
        XmlElement xmlElement3 = element;
        dateTime = currentAndFuturePlan.EndDate;
        string str2;
        if (!dateTime.Equals((object) null))
        {
          dateTime = currentAndFuturePlan.EndDate;
          if (!dateTime.Equals(DateTime.MaxValue))
          {
            dateTime = currentAndFuturePlan.EndDate;
            str2 = dateTime.ToString();
            goto label_9;
          }
        }
        str2 = "";
label_9:
        xmlElement3.SetAttribute("enddate", str2);
        XmlElement xmlElement4 = element;
        int num1 = currentAndFuturePlan.MinTermDays;
        string str3;
        if (!num1.Equals((object) null))
        {
          num1 = currentAndFuturePlan.MinTermDays;
          str3 = num1.ToString();
        }
        else
          str3 = "";
        xmlElement4.SetAttribute("minTermDays", str3);
        XmlElement xmlElement5 = element;
        Decimal num2 = currentAndFuturePlan.PercentAmt;
        string str4;
        if (!num2.Equals((object) null))
        {
          num2 = currentAndFuturePlan.PercentAmt;
          str4 = num2.ToString();
        }
        else
          str4 = "";
        xmlElement5.SetAttribute("percentAmt", str4);
        XmlElement xmlElement6 = element;
        num1 = currentAndFuturePlan.PercentAmtBase;
        string str5;
        if (!num1.Equals((object) null))
        {
          num1 = currentAndFuturePlan.PercentAmtBase;
          str5 = num1.ToString();
        }
        else
          str5 = "";
        xmlElement6.SetAttribute("percentAmtBase", str5);
        XmlElement xmlElement7 = element;
        num1 = currentAndFuturePlan.RoundingMethod;
        string str6;
        if (!num1.Equals((object) null))
        {
          num1 = currentAndFuturePlan.RoundingMethod;
          str6 = num1.ToString();
        }
        else
          str6 = "";
        xmlElement7.SetAttribute("roundingMethod", str6);
        XmlElement xmlElement8 = element;
        num2 = currentAndFuturePlan.DollarAmount;
        string str7;
        if (!num2.Equals((object) null))
        {
          num2 = currentAndFuturePlan.DollarAmount;
          str7 = num2.ToString();
        }
        else
          str7 = "";
        xmlElement8.SetAttribute("dollarAmt", str7);
        XmlElement xmlElement9 = element;
        num2 = currentAndFuturePlan.MinDollarAmount;
        string str8;
        if (!num2.Equals((object) null))
        {
          num2 = currentAndFuturePlan.MinDollarAmount;
          str8 = num2.ToString();
        }
        else
          str8 = "";
        xmlElement9.SetAttribute("minDollarAmt", str8);
        XmlElement xmlElement10 = element;
        num2 = currentAndFuturePlan.MaxDollarAmount;
        string str9;
        if (!num2.Equals((object) null))
        {
          num2 = currentAndFuturePlan.MaxDollarAmount;
          str9 = num2.ToString();
        }
        else
          str9 = "";
        xmlElement10.SetAttribute("maxDollarAmt", str9);
        element.SetAttribute("triggerFieldID", currentAndFuturePlan.TriggerFieldID);
        elementWriter.Append("description", currentAndFuturePlan.Description);
        xmlElement1.AppendChild((XmlNode) element);
      }
    }

    private static void genStateLicensingXML(
      XmlElement root,
      OrgInfo org_info,
      string orgLicenses,
      string license_use_parent_info)
    {
      ElementWriter elementWriter = new ElementWriter(root);
      root.SetAttribute("atrSmallCreditor", org_info.OrgBranchLicensing.ATRSmallCreditorToString());
      root.SetAttribute("atrExemptCreditor", org_info.OrgBranchLicensing.ATRExemptCreditorToString());
      XmlElement xmlElement1 = elementWriter.Append("StateLicenses");
      xmlElement1.SetAttribute("use_parent_info", license_use_parent_info);
      xmlElement1.SetAttribute("license_lender_type", org_info.OrgBranchLicensing.LenderType);
      xmlElement1.SetAttribute("license_home_state", org_info.OrgBranchLicensing.HomeState);
      XmlElement xmlElement2 = xmlElement1;
      bool flag;
      string str1;
      if (!org_info.OrgBranchLicensing.StatutoryElectionInMaryland.Equals((object) null))
      {
        flag = org_info.OrgBranchLicensing.StatutoryElectionInMaryland;
        str1 = flag.ToString();
      }
      else
        str1 = "";
      xmlElement2.SetAttribute("license_statutory_maryland", str1);
      XmlElement xmlElement3 = xmlElement1;
      flag = org_info.OrgBranchLicensing.StatutoryElectionInKansas;
      string str2;
      if (!flag.Equals((object) null))
      {
        flag = org_info.OrgBranchLicensing.StatutoryElectionInKansas;
        str2 = flag.ToString();
      }
      else
        str2 = "";
      xmlElement3.SetAttribute("license_statutory_kansas", str2);
      XmlElement xmlElement4 = xmlElement1;
      flag = org_info.OrgBranchLicensing.UseCustomLenderProfile;
      string str3;
      if (!flag.Equals((object) null))
      {
        flag = org_info.OrgBranchLicensing.UseCustomLenderProfile;
        str3 = flag.ToString();
      }
      else
        str3 = "";
      xmlElement4.SetAttribute("use_custom_lender_profile", str3);
      if (!orgLicenses.Equals("True"))
        return;
      foreach (StateLicenseExtType stateLicenseExtType in org_info.OrgBranchLicensing.StateLicenseExtTypes)
      {
        XmlElement element = xmlElement1.OwnerDocument.CreateElement("license");
        element.SetAttribute("state", stateLicenseExtType.StateAbbrevation);
        element.SetAttribute("licenseType", stateLicenseExtType.LicenseType);
        XmlElement xmlElement5 = element;
        flag = stateLicenseExtType.Selected;
        string str4;
        if (!flag.Equals((object) null))
        {
          flag = stateLicenseExtType.Selected;
          str4 = flag.ToString();
        }
        else
          str4 = "";
        xmlElement5.SetAttribute("licenseSelected", str4);
        XmlElement xmlElement6 = element;
        flag = stateLicenseExtType.Exempt;
        string str5;
        if (!flag.Equals((object) null))
        {
          flag = stateLicenseExtType.Exempt;
          str5 = flag.ToString();
        }
        else
          str5 = "";
        xmlElement6.SetAttribute("licenseExempt", str5);
        element.SetAttribute("licenseNumber", stateLicenseExtType.LicenseNo);
        XmlElement xmlElement7 = element;
        DateTime dateTime = stateLicenseExtType.IssueDate;
        string str6;
        if (!dateTime.Equals((object) null))
        {
          dateTime = stateLicenseExtType.IssueDate;
          if (!dateTime.Equals(DateTime.MinValue))
          {
            dateTime = stateLicenseExtType.IssueDate;
            str6 = dateTime.ToString();
            goto label_22;
          }
        }
        str6 = "";
label_22:
        xmlElement7.SetAttribute("IssueDate", str6);
        XmlElement xmlElement8 = element;
        dateTime = stateLicenseExtType.StartDate;
        string str7;
        if (!dateTime.Equals((object) null))
        {
          dateTime = stateLicenseExtType.StartDate;
          if (!dateTime.Equals(DateTime.MinValue))
          {
            dateTime = stateLicenseExtType.StartDate;
            str7 = dateTime.ToString();
            goto label_26;
          }
        }
        str7 = "";
label_26:
        xmlElement8.SetAttribute("StartDate", str7);
        XmlElement xmlElement9 = element;
        dateTime = stateLicenseExtType.EndDate;
        string str8;
        if (!dateTime.Equals((object) null))
        {
          dateTime = stateLicenseExtType.EndDate;
          if (!dateTime.Equals(DateTime.MinValue))
          {
            dateTime = stateLicenseExtType.EndDate;
            str8 = dateTime.ToString();
            goto label_30;
          }
        }
        str8 = "";
label_30:
        xmlElement9.SetAttribute("EndDate", str8);
        element.SetAttribute("Status", stateLicenseExtType.LicenseStatus);
        XmlElement xmlElement10 = element;
        dateTime = stateLicenseExtType.StatusDate;
        string str9;
        if (!dateTime.Equals((object) null))
        {
          dateTime = stateLicenseExtType.StatusDate;
          if (!dateTime.Equals(DateTime.MinValue))
          {
            dateTime = stateLicenseExtType.StatusDate;
            str9 = dateTime.ToString();
            goto label_34;
          }
        }
        str9 = "";
label_34:
        xmlElement10.SetAttribute("StatusDate", str9);
        XmlElement xmlElement11 = element;
        dateTime = stateLicenseExtType.LastChecked;
        string str10;
        if (!dateTime.Equals((object) null))
        {
          dateTime = stateLicenseExtType.StatusDate;
          if (!dateTime.Equals(DateTime.MinValue))
          {
            dateTime = stateLicenseExtType.LastChecked;
            str10 = dateTime.ToString();
            goto label_38;
          }
        }
        str10 = "";
label_38:
        xmlElement11.SetAttribute("LastCheckedDate", str10);
        XmlElement xmlElement12 = element;
        int sortIndex = stateLicenseExtType.SortIndex;
        string str11;
        if (!sortIndex.Equals((object) null))
        {
          sortIndex = stateLicenseExtType.SortIndex;
          str11 = sortIndex.ToString();
        }
        else
          str11 = "";
        xmlElement12.SetAttribute("SortIndex", str11);
        xmlElement1.AppendChild((XmlNode) element);
      }
    }

    private static void genOrgSettingsData(
      XmlElement root,
      OrgInfo org_info,
      string orgDetails,
      string orgLicenses,
      IDictionary settings)
    {
      UserInfo[] usersInOrganization = User.GetUsersInOrganization(org_info.Oid);
      XmlElement element = root.OwnerDocument.CreateElement("Organization");
      ElementWriter elementWriter = new ElementWriter(element);
      string orgPath = Organization.GetOrgPath(org_info.Oid);
      element.SetAttribute("oid", org_info.Oid.ToString());
      element.SetAttribute("org_name", org_info.OrgName);
      element.SetAttribute("orgPath", orgPath.Replace("\\", "/").Replace(nameof (root), "RootOrganization"));
      element.SetAttribute("parent", org_info.Parent.ToString());
      elementWriter.Append("description", org_info.Description);
      element.SetAttribute("login_access_use_parent_info", org_info.SSOSettings.UseParentInfo.ToString());
      if (org_info.SSOSettings.UseParentInfo)
      {
        OrgInfo organizationForSso = OrganizationStore.GetFirstOrganizationForSSO(org_info.Oid);
        element.SetAttribute("login_access", organizationForSso.SSOSettings.LoginAccess.ToString());
      }
      else
        element.SetAttribute("login_access", org_info.SSOSettings.LoginAccess.ToString());
      OrgInfo withStateLicensing = OrganizationStore.GetFirstOrganizationWithStateLicensing(org_info.Oid);
      string license_use_parent_info = "";
      int num1 = orgPath.Length - orgPath.Replace("\\", "").Length;
      element.SetAttribute("depth", num1.ToString());
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      string str6 = "";
      OrgInfo avaliableOrganization = OrganizationStore.GetFirstAvaliableOrganization(org_info.Oid, true);
      OrgInfo organizationWithNmls = OrganizationStore.GetFirstOrganizationWithNMLS(org_info.Oid);
      OrgInfo organizationWithMersmin = OrganizationStore.GetFirstOrganizationWithMERSMIN(org_info.Oid);
      OrgInfo organizationWithOnrp = OrganizationStore.GetFirstOrganizationWithONRP(org_info.Oid);
      OrgInfo organizationWithLei = OrganizationStore.GetFirstOrganizationWithLEI(org_info.Oid);
      OrgInfo organizationWithLoSearch = OrganizationStore.GetFirstOrganizationWithLOSearch(org_info.Oid);
      if (avaliableOrganization != null)
        str1 = avaliableOrganization.Oid == org_info.Oid ? "False" : "True";
      if (organizationWithNmls != null)
        str2 = organizationWithNmls.Oid == org_info.Oid ? "False" : "True";
      else if (num1 > 0)
        str2 = "True";
      if (organizationWithMersmin != null)
        str3 = organizationWithMersmin.Oid == org_info.Oid ? "False" : "True";
      if (organizationWithOnrp != null)
        str4 = organizationWithOnrp.Oid == org_info.Oid ? "False" : "True";
      if (withStateLicensing != null)
        license_use_parent_info = withStateLicensing.Oid == org_info.Oid ? "False" : "True";
      if (organizationWithLei != null)
        str5 = organizationWithLei.Oid == org_info.Oid ? "False" : "True";
      if (organizationWithLoSearch != null)
        str6 = organizationWithLoSearch.Oid == org_info.Oid ? "False" : "True";
      element.SetAttribute("license_use_parent_info", license_use_parent_info);
      element.SetAttribute("org_use_parent_info", str1);
      element.SetAttribute("cclo_search_use_parent_info", str6);
      element.SetAttribute("nmls_use_parent_info", str2);
      element.SetAttribute("mersmin_use_parent_info", str3);
      element.SetAttribute("lei_use_parent_info", str5);
      if (str2 == "True" && organizationWithNmls != null)
        element.SetAttribute("nmls_code", organizationWithNmls.NMLSCode);
      else
        element.SetAttribute("nmls_code", org_info.NMLSCode);
      if (str3 == "True")
        element.SetAttribute("mersmin_code", organizationWithMersmin.MERSMINCode);
      else
        element.SetAttribute("mersmin_code", org_info.MERSMINCode);
      if (str6 == "True")
      {
        if (organizationWithLoSearch.ShowOrgInLOSearch && !string.IsNullOrEmpty(organizationWithLoSearch.LOSearchOrgName))
        {
          element.SetAttribute("cclo_show_org_in_search", "True");
          element.SetAttribute("cclo_org_name", organizationWithLoSearch.LOSearchOrgName);
        }
        else
        {
          element.SetAttribute("cclo_show_org_in_search", "False");
          element.SetAttribute("cclo_org_name", "");
        }
      }
      else if (org_info.ShowOrgInLOSearch && !string.IsNullOrEmpty(org_info.LOSearchOrgName))
      {
        element.SetAttribute("cclo_show_org_in_search", "True");
        element.SetAttribute("cclo_org_name", org_info.LOSearchOrgName);
      }
      else
      {
        element.SetAttribute("cclo_show_org_in_search", "False");
        element.SetAttribute("cclo_org_name", "");
      }
      if (str5 == "True")
      {
        HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(organizationWithLei.HMDAProfileId);
        element.SetAttribute("lei_hmda_profile", hmdaProfileById != null ? hmdaProfileById.HMDAProfileName : "");
        element.SetAttribute("lei_leicode", hmdaProfileById != null ? hmdaProfileById.HMDAProfileLEI : "");
      }
      else
      {
        HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(org_info.HMDAProfileId);
        element.SetAttribute("lei_hmda_profile", hmdaProfileById != null ? hmdaProfileById.HMDAProfileName : "");
        element.SetAttribute("lei_leicode", hmdaProfileById != null ? hmdaProfileById.HMDAProfileLEI : "");
      }
      element.SetAttribute("cc_site_url_use_parent_info", org_info.CCSiteSettings != null ? org_info.CCSiteSettings.UseParentInfo.ToString() : "");
      element.SetAttribute("cc_site_url", org_info.CCSiteSettings != null ? org_info.CCSiteSettings.Url : "");
      element.SetAttribute("cc_site_id", org_info.CCSiteSettings != null ? org_info.CCSiteSettings.SiteId : "");
      if (orgDetails.Equals("True"))
      {
        int num2 = 0;
        int num3 = 0;
        foreach (UserInfo userInfo in usersInOrganization)
        {
          if (userInfo.Status.ToString().Equals("Enabled"))
            ++num2;
          else
            ++num3;
        }
        element.SetAttribute("enabled_user_count", num2.ToString());
        element.SetAttribute("disabled_user_count", num3.ToString());
        if (str1 == "True")
          SettingsRptXmlHelper.genOrgInfoXML(element, avaliableOrganization);
        else
          SettingsRptXmlHelper.genOrgInfoXML(element, org_info);
        if (str4 == "True")
          SettingsRptXmlHelper.genOnrpInfoXML(element, organizationWithOnrp, settings, org_info.ONRPRetailBranchSettings.UseChannelDefault);
        else
          SettingsRptXmlHelper.genOnrpInfoXML(element, org_info, settings, org_info.ONRPRetailBranchSettings.UseChannelDefault);
        SettingsRptXmlHelper.genCompPlansXML(element, org_info);
      }
      if (license_use_parent_info == "True")
        SettingsRptXmlHelper.genStateLicensingXML(element, withStateLicensing, orgLicenses, license_use_parent_info);
      else
        SettingsRptXmlHelper.genStateLicensingXML(element, org_info, orgLicenses, license_use_parent_info);
      root.AppendChild((XmlNode) element);
    }

    public static string createOrgXML(
      int orgId,
      Dictionary<string, string> reportParameters,
      string reportID)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element = xmlDocument.CreateElement("SettingsOrgData");
      xmlDocument.AppendChild((XmlNode) element);
      IDictionary serverSettings = SettingsReportAccessor.getServerSettings("POLICIES");
      string orgDetails = reportParameters.ContainsKey("OrganizationDetails") ? reportParameters["OrganizationDetails"] : "False";
      string orgLicenses = reportParameters.ContainsKey("OrganizationLicenses") ? reportParameters["OrganizationLicenses"] : "False";
      OrgInfo org_info = Organization.LoadOrganization(orgId);
      SettingsRptXmlHelper.genOrgSettingsData(element, org_info, orgDetails, orgLicenses, serverSettings);
      if ((reportParameters.ContainsKey("IncludeSubOrganization") ? reportParameters["IncludeSubOrganization"] : "False").Equals("True"))
      {
        OrgInfo[] orgInfoArray = Organization.LoadOrganizations(OrganizationStore.GetDescendentsOfOrg(orgId));
        int num = 10;
        if (orgInfoArray.Length != 0)
        {
          for (int index = 0; index < orgInfoArray.Length; ++index)
          {
            if (index % num == 0 && SettingsReportAccessor.CancelJob(reportID))
              return "CANCELLED";
            SettingsRptXmlHelper.genOrgSettingsData(element, orgInfoArray[index], orgDetails, orgLicenses, serverSettings);
          }
        }
      }
      return element.OuterXml;
    }

    public static string createUsersXml(
      int orgId,
      Dictionary<string, string> reportParameters,
      string reportID)
    {
      string orgPath = Organization.GetOrgPath(orgId);
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement("SettingsUserData");
      xmlDocument.AppendChild((XmlNode) element1);
      string str1 = reportParameters.ContainsKey("IncludeSubOrganization") ? reportParameters["IncludeSubOrganization"] : "False";
      string str2 = reportParameters.ContainsKey("IncludeDisabledUser") ? reportParameters["IncludeDisabledUser"] : "False";
      UserInfo[] userInfoArray = !str1.Equals("True") ? User.GetUsersInOrganization(orgId) : User.GetUsersUnderOrganization(orgId);
      int num1 = 10;
      Dictionary<int, Tuple<string, string>> dictionary = new Dictionary<int, Tuple<string, string>>();
      for (int index1 = 0; index1 < userInfoArray.Length; ++index1)
      {
        if (index1 % num1 == 0 && SettingsReportAccessor.CancelJob(reportID))
          return "CANCELLED";
        UserInfo userInfo = userInfoArray[index1];
        if (!str2.Equals("False") || userInfo.Status != UserInfo.UserStatusEnum.Disabled)
        {
          LOLicenseInfo[] loLicenseInfoArray = (LOLicenseInfo[]) null;
          using (User latestVersion = UserStore.GetLatestVersion(userInfo.Userid))
            loLicenseInfoArray = latestVersion.GetLOLicenses();
          LoanCompHistoryList planHistoryforUser = LOCompAccessor.GetComPlanHistoryforUser(userInfo.Userid, false, false);
          string str3 = "";
          string str4 = "";
          if (!dictionary.ContainsKey(userInfo.OrgId))
          {
            using (Organization latestVersion = OrganizationStore.GetLatestVersion(userInfo.OrgId))
            {
              if (!latestVersion.Exists)
                Err.Raise(TraceLevel.Warning, nameof (SettingsRptXmlHelper), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
              str3 = latestVersion.GetOrganizationInfo().OrgName;
              str4 = orgPath.Replace("\\", "/").Replace("root", "RootOrganization");
              dictionary.Add(userInfo.OrgId, Tuple.Create<string, string>(str3, str4));
            }
          }
          else
          {
            Tuple<string, string> tuple = dictionary[userInfo.OrgId];
            str3 = tuple.Item1;
            str4 = tuple.Item2;
          }
          CCSiteInfo userCcSiteInfo = CCSiteInfoAccessor.getUserCCSiteInfo(userInfo.Userid);
          XmlElement element2 = element1.OwnerDocument.CreateElement("users");
          ElementWriter elementWriter1 = new ElementWriter(element2);
          XmlElement xmlElement1 = element2;
          int num2 = userInfo.OrgId;
          string str5 = num2.ToString();
          xmlElement1.SetAttribute("oid", str5);
          element2.SetAttribute("userid", userInfo.Userid);
          element2.SetAttribute("first_name", userInfo.FirstName);
          element2.SetAttribute("last_name", userInfo.LastName);
          element2.SetAttribute("org_name", str3);
          element2.SetAttribute("org_path", str4);
          string str6 = SecurityElement.Escape(userInfo.WorkingFolder);
          element2.SetAttribute("working_folder", str6);
          element2.SetAttribute("access_mode", userInfo.AccessMode.ToString());
          element2.SetAttribute("file_transfer_right", userInfo.file_transfer_right);
          element2.SetAttribute("tmp_table_right", userInfo.tmp_table_right);
          element2.SetAttribute("working_folder", userInfo.WorkingFolder);
          element2.SetAttribute("tracking_setup_right", userInfo.tracking_setup_right);
          element2.SetAttribute("reports_right", userInfo.reports_right);
          element2.SetAttribute("myepass_custom_right", userInfo.myepass_custom_right);
          element2.SetAttribute("offline_right", userInfo.offline_right);
          element2.SetAttribute("status", userInfo.Status.ToString());
          element2.SetAttribute("failed_login_attempts", userInfo.failed_login_attempts.ToString());
          element2.SetAttribute("contact_export_right", userInfo.contact_export_right);
          element2.SetAttribute("plan_code_right", userInfo.plan_code_right);
          element2.SetAttribute("alt_lender_right", userInfo.alt_lender_right);
          element2.SetAttribute("scope_lo", userInfo.scope_lo);
          element2.SetAttribute("scope_lp", userInfo.scope_lp);
          element2.SetAttribute("scope_closer", userInfo.scope_closer);
          element2.SetAttribute("pipelineView", userInfo.pipelineView);
          element2.SetAttribute("peerView", userInfo.PeerView.ToString());
          element2.SetAttribute("require_pwd_change", userInfo.RequirePasswordChange.ToString());
          element2.SetAttribute("locked", userInfo.Locked.ToString());
          element2.SetAttribute("sso_only", userInfo.SSOOnly.ToString());
          element2.SetAttribute("no_pwd_expiration", userInfo.PasswordNeverExpires.ToString());
          XmlElement xmlElement2 = element2;
          bool flag1 = userInfo.DataServicesOptOut;
          string str7 = flag1.ToString();
          xmlElement2.SetAttribute("data_services_opt", str7);
          XmlElement xmlElement3 = element2;
          flag1 = userInfo.LegacyDelegateTasksRight;
          string str8 = flag1.ToString();
          xmlElement3.SetAttribute("delegate_tasks_right", str8);
          element2.SetAttribute("chumid", userInfo.CHUMId.ToString());
          element2.SetAttribute("nmlsOriginatorID", userInfo.NMLSOriginatorID.ToString());
          element2.SetAttribute("user_cc_site", userCcSiteInfo != null ? userCcSiteInfo.Url : "");
          element2.SetAttribute("user_cc_siteId", userCcSiteInfo != null ? userCcSiteInfo.SiteId : "");
          element2.SetAttribute("enc_version", userInfo.enc_version.ToString());
          XmlElement xmlElement4 = element2;
          flag1 = userInfo.PersonalStatusOnline;
          string str9 = flag1.ToString();
          xmlElement4.SetAttribute("personalStatusOnline", str9);
          element2.SetAttribute("employee_id", userInfo.EmployeeID.ToString());
          XmlElement xmlElement5 = element2;
          flag1 = userInfo.InheritParentCompPlan;
          string str10 = flag1.ToString();
          xmlElement5.SetAttribute("inheritParentCompPlan", str10);
          element2.SetAttribute("middle_name", userInfo.MiddleName.ToString());
          element2.SetAttribute("suffix_name", userInfo.SuffixName.ToString());
          element2.SetAttribute("job_title", userInfo.JobTitle.ToString());
          element2.SetAttribute("user_Type", userInfo.UserType.ToString());
          element2.SetAttribute("FirstLastName", userInfo.firstLastName.ToString());
          element2.SetAttribute("UserName", userInfo.userName.ToString());
          elementWriter1.Append("password", userInfo.Password);
          XmlElement element3 = element2.OwnerDocument.CreateElement("UserPublicProfile");
          ElementWriter elementWriter2 = new ElementWriter(element3);
          bool flag2 = FeaturesAclDbAccessor.CheckPermission(AclFeature.SettingsTab_Personal_MyProfilePhoto, userInfo);
          UserProfileInfo userProfile = User.GetUserProfile(userInfo.Userid);
          elementWriter2.Append("public_profile", flag2.ToString());
          if (userProfile != null)
          {
            ElementWriter elementWriter3 = elementWriter2;
            flag1 = userProfile.Enable_Profile;
            string innerText = flag1.ToString();
            elementWriter3.Append("enable_profile", innerText);
            elementWriter2.Append("first_name", userProfile.FirstName);
            elementWriter2.Append("middle_name", userProfile.MiddleName);
            elementWriter2.Append("last_name", userProfile.LastName);
            elementWriter2.Append("suffix_name", userProfile.SuffixName);
            elementWriter2.Append("job_title", userProfile.JobTitle);
            XmlElement element4 = element3.OwnerDocument.CreateElement("phone1");
            XmlElement xmlElement6 = element4;
            num2 = userProfile.Phone1Type;
            string str11 = num2.ToString();
            xmlElement6.SetAttribute("phone_type", str11);
            element4.InnerText = userProfile.Phone1.ToString();
            element3.AppendChild((XmlNode) element4);
            XmlElement element5 = element3.OwnerDocument.CreateElement("phone2");
            XmlElement xmlElement7 = element5;
            num2 = userProfile.Phone2Type;
            string str12 = num2.ToString();
            xmlElement7.SetAttribute("phone_type", str12);
            element5.InnerText = userProfile.Phone2.ToString();
            element3.AppendChild((XmlNode) element5);
            elementWriter2.Append("email", userProfile.Email);
            elementWriter2.Append("nmls_lo_id", userInfo.NMLSOriginatorID);
            elementWriter2.Append("link1", userProfile.Link1);
            elementWriter2.Append("link2", userProfile.Link2);
            elementWriter2.Append("link3", userProfile.Link3);
            elementWriter2.Append("profile_desc", userProfile.ProfileDesc);
          }
          element2.AppendChild((XmlNode) element3);
          XmlElement element6 = element2.OwnerDocument.CreateElement("UserPersona");
          for (int index2 = 0; index2 < userInfo.UserPersonas.Length; ++index2)
          {
            Persona userPersona = userInfo.UserPersonas[index2];
            XmlElement element7 = element6.OwnerDocument.CreateElement("persona");
            XmlElement xmlElement8 = element7;
            num2 = userPersona.ID;
            string str13 = num2.ToString();
            xmlElement8.SetAttribute("id", str13);
            element7.InnerText = userPersona.Name;
            element6.AppendChild((XmlNode) element7);
          }
          element2.AppendChild((XmlNode) element6);
          elementWriter1.Append("email", userInfo.Email);
          elementWriter1.Append("phone", userInfo.Phone);
          XmlElement element8 = xmlDocument.CreateElement("password_changed");
          element8.SetAttribute("value", userInfo.PasswordChangedDate.ToString());
          element2.AppendChild((XmlNode) element8);
          elementWriter1.Append("lo_license", userInfo.lo_license);
          elementWriter1.Append("fax", userInfo.Fax);
          XmlElement element9 = xmlDocument.CreateElement("last_login");
          element9.SetAttribute("value", userInfo.LastLogin == DateTime.MinValue ? "" : userInfo.LastLogin.ToString());
          element2.AppendChild((XmlNode) element9);
          elementWriter1.Append("cell_phone", userInfo.CellPhone);
          XmlElement element10 = xmlDocument.CreateElement("nmlsExpirationDate");
          element10.SetAttribute("value", userInfo.NMLSExpirationDate == DateTime.MaxValue ? "" : userInfo.NMLSExpirationDate.ToString());
          element2.AppendChild((XmlNode) element10);
          string innerText1 = SecurityElement.Escape(userInfo.EmailSignature);
          elementWriter1.Append("emailSignature", innerText1);
          elementWriter1.Append("personaAccessComments", userInfo.PersonaAccessComments);
          bool flag3 = SettingsRptXmlHelper.loadModificationSetting(userInfo.Userid, userInfo.UserPersonas);
          elementWriter1.Append("userRights", flag3.ToString());
          AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userInfo.Userid);
          XmlElement element11 = element2.OwnerDocument.CreateElement("aclGroup");
          foreach (AclGroup aclGroup in groupsOfUser)
          {
            XmlElement element12 = element11.OwnerDocument.CreateElement("group");
            XmlElement xmlElement9 = element12;
            num2 = aclGroup.ID;
            string str14 = num2.ToString();
            xmlElement9.SetAttribute("id", str14);
            element12.InnerText = aclGroup.Name;
            element11.AppendChild((XmlNode) element12);
          }
          element2.AppendChild((XmlNode) element11);
          XmlElement element13 = element2.OwnerDocument.CreateElement("user_lo_licenses");
          for (int index3 = 0; index3 < loLicenseInfoArray.Length; ++index3)
          {
            LOLicenseInfo loLicenseInfo = loLicenseInfoArray[index3];
            XmlElement element14 = element13.OwnerDocument.CreateElement("license");
            element14.SetAttribute("userid", loLicenseInfo.UserID);
            element14.SetAttribute("state", loLicenseInfo.StateAbbr);
            XmlElement xmlElement10 = element14;
            flag1 = loLicenseInfo.Enabled;
            string str15 = flag1.ToString();
            xmlElement10.SetAttribute("enabled", str15);
            element14.SetAttribute("license", loLicenseInfo.License);
            element14.SetAttribute("expirationDate", loLicenseInfo.ExpirationDate == DateTime.MaxValue ? "" : loLicenseInfo.ExpirationDate.ToString());
            element14.SetAttribute("IssueDate", loLicenseInfo.IssueDate == DateTime.MinValue ? "" : loLicenseInfo.IssueDate.ToString());
            element14.SetAttribute("StartDate", loLicenseInfo.StartDate == DateTime.MinValue ? "" : loLicenseInfo.StartDate.ToString());
            element14.SetAttribute("Status", loLicenseInfo.LicenseStatus);
            element14.SetAttribute("StatusDate", loLicenseInfo.StatusDate == DateTime.MinValue ? "" : loLicenseInfo.StatusDate.ToString());
            element14.SetAttribute("LastCheckedDate", loLicenseInfo.LastChecked == DateTime.MinValue ? "" : loLicenseInfo.LastChecked.ToString());
            element13.AppendChild((XmlNode) element14);
          }
          element2.AppendChild((XmlNode) element13);
          XmlElement element15 = element2.OwnerDocument.CreateElement("CompPlans");
          for (int i = 0; i < planHistoryforUser.Count; ++i)
          {
            LoanCompHistory historyAt = planHistoryforUser.GetHistoryAt(i);
            XmlElement element16 = element15.OwnerDocument.CreateElement("CompPlan");
            element16.SetAttribute("type", historyAt.Type);
            element16.SetAttribute("status", historyAt.Status);
            element16.SetAttribute("activationDate", historyAt.StartDate == DateTime.MinValue ? "" : historyAt.StartDate.ToString());
            element16.SetAttribute("enddate", historyAt.EndDate == DateTime.MaxValue.Date ? "" : historyAt.EndDate.ToString());
            element16.SetAttribute("triggerFieldID", historyAt.TriggerFieldID);
            XmlElement xmlElement11 = element16;
            num2 = historyAt.MinTermDays;
            string str16 = num2.ToString();
            xmlElement11.SetAttribute("minTermDays", str16);
            element16.SetAttribute("name", historyAt.PlanName);
            XmlElement xmlElement12 = element16;
            flag1 = planHistoryforUser.UseParentInfo;
            string str17 = flag1.ToString();
            xmlElement12.SetAttribute("use_parent_info", str17);
            XmlElement xmlElement13 = element16;
            Decimal num3 = historyAt.PercentAmt;
            string str18 = num3.ToString();
            xmlElement13.SetAttribute("percentAmt", str18);
            XmlElement xmlElement14 = element16;
            num2 = historyAt.PercentAmtBase;
            string str19 = num2.ToString();
            xmlElement14.SetAttribute("percentAmtBase", str19);
            XmlElement xmlElement15 = element16;
            num2 = historyAt.RoundingMethod;
            string str20 = num2.ToString();
            xmlElement15.SetAttribute("roundingMethod", str20);
            XmlElement xmlElement16 = element16;
            num3 = historyAt.DollarAmount;
            string str21 = num3.ToString();
            xmlElement16.SetAttribute("dollarAmt", str21);
            XmlElement xmlElement17 = element16;
            num3 = historyAt.MinDollarAmount;
            string str22 = num3.ToString();
            xmlElement17.SetAttribute("minDollarAmt", str22);
            XmlElement xmlElement18 = element16;
            num3 = historyAt.MaxDollarAmount;
            string str23 = num3.ToString();
            xmlElement18.SetAttribute("maxDollarAmt", str23);
            XmlElement element17 = element16.OwnerDocument.CreateElement("description");
            element17.InnerText = historyAt.Description;
            element16.AppendChild((XmlNode) element17);
            element15.AppendChild((XmlNode) element16);
          }
          element2.AppendChild((XmlNode) element15);
          element1.AppendChild((XmlNode) element2);
        }
      }
      return element1.OuterXml;
    }

    public static string createUsersXml3(int orgId, Dictionary<string, string> reportParameters)
    {
      string orgPath = Organization.GetOrgPath(orgId);
      string str1 = reportParameters.ContainsKey("IncludeSubOrganization") ? reportParameters["IncludeSubOrganization"] : "False";
      string str2 = reportParameters.ContainsKey("IncludeDisabledUser") ? reportParameters["IncludeDisabledUser"] : "False";
      UserInfo[] userInfoArray = !str1.Equals("True") ? User.GetUsersInOrganization(orgId) : User.GetUsersUnderOrganization(orgId);
      StringBuilder stringBuilder1 = new StringBuilder("<SettingsUserData>");
      for (int index1 = 0; index1 < userInfoArray.Length; ++index1)
      {
        UserInfo userInfo = userInfoArray[index1];
        if (!str2.Equals("False") || userInfo.Status != UserInfo.UserStatusEnum.Disabled)
        {
          LOLicenseInfo[] loLicenseInfoArray = (LOLicenseInfo[]) null;
          using (User latestVersion = UserStore.GetLatestVersion(userInfo.Userid))
            loLicenseInfoArray = latestVersion.GetLOLicenses();
          LoanCompHistoryList planHistoryforUser = LOCompAccessor.GetComPlanHistoryforUser(userInfo.Userid, false, false);
          string str3 = "";
          string str4 = "";
          using (Organization latestVersion = OrganizationStore.GetLatestVersion(userInfo.OrgId))
          {
            if (!latestVersion.Exists)
              Err.Raise(TraceLevel.Warning, nameof (SettingsRptXmlHelper), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
            str3 = latestVersion.GetOrganizationInfo().OrgName;
            str4 = orgPath.Replace("\\", "/").Replace("root", "RootOrganization");
          }
          StringBuilder stringBuilder2 = stringBuilder1;
          object[] objArray1 = new object[83]
          {
            (object) "<users table =\"users\" userid =\"",
            (object) userInfo.Userid,
            (object) "\" first_name=\"",
            (object) userInfo.FirstName,
            (object) "\" last_name=\"",
            (object) userInfo.LastName,
            (object) "\" org_id =\"",
            (object) userInfo.OrgId,
            (object) "\" org_name=\"",
            (object) str3,
            (object) "\" org_path=\"",
            (object) str4,
            (object) "\" working_folder=\"",
            (object) userInfo.WorkingFolder,
            (object) "\" access_mode=\"",
            (object) userInfo.AccessMode,
            (object) "\" file_transfer_right=\"",
            (object) userInfo.file_transfer_right,
            (object) "\" tmp_table_right=\"",
            (object) userInfo.tmp_table_right,
            (object) "\" tracking_setup_right=\"",
            (object) userInfo.tracking_setup_right,
            (object) "\" reports_right=\"",
            (object) userInfo.reports_right,
            (object) "\" myepass_custom_right=\"",
            (object) userInfo.myepass_custom_right,
            (object) "\" offline_right=\"",
            (object) userInfo.offline_right,
            (object) "\" status=\"",
            (object) userInfo.Status,
            (object) "\" failed_login_attempts=\"",
            (object) userInfo.failed_login_attempts,
            (object) "\" contact_export_right=\"",
            (object) userInfo.contact_export_right,
            (object) "\" plan_code_right=\"",
            (object) userInfo.plan_code_right,
            (object) "\" alt_lender_right=\"",
            (object) userInfo.alt_lender_right,
            (object) "\" scope_lo=\"",
            (object) userInfo.scope_lo,
            (object) "\" scope_lp=\"",
            (object) userInfo.scope_lp,
            (object) "\" scope_closer=\"",
            (object) userInfo.scope_closer,
            (object) "\" pipelineView=\"",
            (object) userInfo.pipelineView,
            (object) "\" peerView=\"",
            (object) userInfo.PeerView,
            (object) "\" require_pwd_change=\"",
            (object) userInfo.RequirePasswordChange.ToString(),
            (object) "\" locked=\"",
            (object) userInfo.Locked.ToString(),
            (object) "\" no_pwd_expiration=\"",
            (object) userInfo.PasswordNeverExpires.ToString(),
            (object) "\" data_services_opt=\"",
            (object) userInfo.DataServicesOptOut.ToString(),
            (object) "\" delegate_tasks_right=\"",
            (object) userInfo.LegacyDelegateTasksRight.ToString(),
            (object) "\" chumid=\"",
            (object) userInfo.CHUMId,
            (object) "\" nmlsOriginatorID=\"",
            (object) userInfo.NMLSOriginatorID,
            (object) "\" enc_version=\"",
            (object) userInfo.enc_version,
            (object) "\" personalStatusOnline=\"",
            (object) userInfo.PersonalStatusOnline.ToString(),
            (object) "\" employee_id=\"",
            (object) userInfo.EmployeeID,
            (object) "\" inheritParentCompPlan=\"",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
          };
          bool flag1 = userInfo.InheritParentCompPlan;
          objArray1[69] = (object) flag1.ToString();
          objArray1[70] = (object) "\" middle_name=\"";
          objArray1[71] = (object) userInfo.MiddleName;
          objArray1[72] = (object) "\" suffix_name=\"";
          objArray1[73] = (object) userInfo.SuffixName;
          objArray1[74] = (object) "\" job_title=\"";
          objArray1[75] = (object) userInfo.JobTitle;
          objArray1[76] = (object) "\" user_Type=\"";
          objArray1[77] = (object) userInfo.UserType;
          objArray1[78] = (object) "\" FirstLastName=\"";
          objArray1[79] = (object) userInfo.firstLastName;
          objArray1[80] = (object) "\" UserName=\"";
          objArray1[81] = (object) userInfo.userName;
          objArray1[82] = (object) "\">";
          string str5 = string.Concat(objArray1);
          stringBuilder2.Append(str5);
          stringBuilder1.Append("<password>" + userInfo.Password + "</password>");
          stringBuilder1.Append("<UserPersona table =\"UserPersona\">");
          for (int index2 = 0; index2 < userInfo.UserPersonas.Length; ++index2)
          {
            Persona userPersona = userInfo.UserPersonas[index2];
            stringBuilder1.Append("<persona id=\"" + (object) userPersona.ID + "\">" + userPersona.Name + "</persona>");
          }
          stringBuilder1.Append("</UserPersona>");
          stringBuilder1.Append("<email>" + userInfo.Email + "</email>");
          stringBuilder1.Append("<phone>" + userInfo.Phone + "</phone>");
          stringBuilder1.Append("<password_changed value =\"" + (object) userInfo.PasswordChangedDate + "\" />");
          stringBuilder1.Append("<lo_license>" + userInfo.lo_license + "</lo_license>");
          stringBuilder1.Append("<fax>" + userInfo.Fax + "</fax>");
          StringBuilder stringBuilder3 = stringBuilder1;
          DateTime dateTime;
          string str6;
          if (!(userInfo.LastLogin == DateTime.MinValue))
          {
            dateTime = userInfo.LastLogin;
            str6 = dateTime.ToString();
          }
          else
            str6 = "";
          string str7 = "<last_login value =\"" + str6 + "\"/>";
          stringBuilder3.Append(str7);
          stringBuilder1.Append("<cell_phone>" + userInfo.CellPhone + "</cell_phone>");
          StringBuilder stringBuilder4 = stringBuilder1;
          string str8;
          if (!(userInfo.NMLSExpirationDate == DateTime.MaxValue))
          {
            dateTime = userInfo.NMLSExpirationDate;
            str8 = dateTime.ToString();
          }
          else
            str8 = "";
          string str9 = "<nmlsExpirationDate value =\"" + str8 + "\"/>";
          stringBuilder4.Append(str9);
          string str10 = SecurityElement.Escape(userInfo.EmailSignature);
          stringBuilder1.Append("<emailSignature>" + str10 + "</emailSignature>");
          stringBuilder1.Append("<personaAccessComments>" + userInfo.PersonaAccessComments + "</personaAccessComments>");
          bool flag2 = SettingsRptXmlHelper.loadModificationSetting(userInfo.Userid, userInfo.UserPersonas);
          stringBuilder1.Append("<userRights>" + flag2.ToString() + "</userRights>");
          AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userInfo.Userid);
          stringBuilder1.Append("<aclGroup>");
          foreach (AclGroup aclGroup in groupsOfUser)
            stringBuilder1.Append("<group id=\"" + (object) aclGroup.ID + "\">" + aclGroup.Name + "</group>");
          stringBuilder1.Append("</aclGroup>");
          stringBuilder1.Append("<user_lo_licenses>");
          for (int index3 = 0; index3 < loLicenseInfoArray.Length; ++index3)
          {
            LOLicenseInfo loLicenseInfo = loLicenseInfoArray[index3];
            StringBuilder stringBuilder5 = stringBuilder1;
            string[] strArray = new string[21];
            strArray[0] = "<license userid=\"";
            strArray[1] = loLicenseInfo.UserID;
            strArray[2] = "\" state=\"";
            strArray[3] = loLicenseInfo.StateAbbr;
            strArray[4] = "\" enabled=\"";
            flag1 = loLicenseInfo.Enabled;
            strArray[5] = flag1.ToString();
            strArray[6] = "\" license=\"";
            strArray[7] = loLicenseInfo.License;
            strArray[8] = "\" expirationDate=\"";
            string str11;
            if (!(loLicenseInfo.ExpirationDate == DateTime.MaxValue))
            {
              dateTime = loLicenseInfo.ExpirationDate;
              str11 = dateTime.ToString();
            }
            else
              str11 = "";
            strArray[9] = str11;
            strArray[10] = "\" IssueDate=\"";
            string str12;
            if (!(loLicenseInfo.IssueDate == DateTime.MinValue))
            {
              dateTime = loLicenseInfo.IssueDate;
              str12 = dateTime.ToString();
            }
            else
              str12 = "";
            strArray[11] = str12;
            strArray[12] = "\" StartDate=\"";
            string str13;
            if (!(loLicenseInfo.StartDate == DateTime.MinValue))
            {
              dateTime = loLicenseInfo.StartDate;
              str13 = dateTime.ToString();
            }
            else
              str13 = "";
            strArray[13] = str13;
            strArray[14] = "\" Status=\"";
            strArray[15] = loLicenseInfo.LicenseStatus;
            strArray[16] = "\" StatusDate=\"";
            string str14;
            if (!(loLicenseInfo.StatusDate == DateTime.MinValue))
            {
              dateTime = loLicenseInfo.StatusDate;
              str14 = dateTime.ToString();
            }
            else
              str14 = "";
            strArray[17] = str14;
            strArray[18] = "\" LastCheckedDate=\"";
            string str15;
            if (!(loLicenseInfo.LastChecked == DateTime.MinValue))
            {
              dateTime = loLicenseInfo.LastChecked;
              str15 = dateTime.ToString();
            }
            else
              str15 = "";
            strArray[19] = str15;
            strArray[20] = "\">";
            string str16 = string.Concat(strArray);
            stringBuilder5.Append(str16);
            stringBuilder1.Append("</license>");
          }
          stringBuilder1.Append("</user_lo_licenses>");
          stringBuilder1.Append("<CompPlans table=\"user_compPlan\">");
          for (int i = 0; i < planHistoryforUser.Count; ++i)
          {
            LoanCompHistory historyAt = planHistoryforUser.GetHistoryAt(i);
            StringBuilder stringBuilder6 = stringBuilder1;
            object[] objArray2 = new object[29];
            objArray2[0] = (object) "<CompPlan type=\"";
            objArray2[1] = (object) historyAt.Type;
            objArray2[2] = (object) "\" status=\"";
            objArray2[3] = (object) historyAt.Status;
            objArray2[4] = (object) "\" activationDate=\"";
            string str17;
            if (!(historyAt.StartDate == DateTime.MinValue))
            {
              dateTime = historyAt.StartDate;
              str17 = dateTime.ToString();
            }
            else
              str17 = "";
            objArray2[5] = (object) str17;
            objArray2[6] = (object) "\" enddate=\"";
            DateTime endDate = historyAt.EndDate;
            dateTime = DateTime.MaxValue;
            DateTime date = dateTime.Date;
            string str18;
            if (!(endDate == date))
            {
              dateTime = historyAt.EndDate;
              str18 = dateTime.ToString();
            }
            else
              str18 = "";
            objArray2[7] = (object) str18;
            objArray2[8] = (object) "\" triggerFieldID=\"";
            objArray2[9] = (object) historyAt.TriggerFieldID;
            objArray2[10] = (object) "\" minTermDays=\"";
            objArray2[11] = (object) historyAt.MinTermDays;
            objArray2[12] = (object) "\" name=\"";
            objArray2[13] = (object) historyAt.PlanName;
            objArray2[14] = (object) "\" use_parent_info=\"";
            flag1 = planHistoryforUser.UseParentInfo;
            objArray2[15] = (object) flag1.ToString();
            objArray2[16] = (object) "\" percentAmt=\"";
            objArray2[17] = (object) historyAt.PercentAmt;
            objArray2[18] = (object) "\" percentAmtBase=\"";
            objArray2[19] = (object) historyAt.PercentAmtBase;
            objArray2[20] = (object) "\" roundingMethod=\"";
            objArray2[21] = (object) historyAt.RoundingMethod;
            objArray2[22] = (object) "\" dollarAmt=\"";
            objArray2[23] = (object) historyAt.DollarAmount;
            objArray2[24] = (object) "\" minDollarAmt=\"";
            objArray2[25] = (object) historyAt.MinDollarAmount;
            objArray2[26] = (object) "\" maxDollarAmt=\"";
            objArray2[27] = (object) historyAt.MaxDollarAmount;
            objArray2[28] = (object) "\">";
            string str19 = string.Concat(objArray2);
            stringBuilder6.Append(str19);
            stringBuilder1.Append("<description>" + historyAt.Description + "</description>");
            stringBuilder1.Append("</CompPlan>");
          }
          stringBuilder1.Append("</CompPlans>");
          stringBuilder1.Append("</users>");
        }
      }
      stringBuilder1.Append("</SettingsUserData>");
      return stringBuilder1.ToString();
    }

    private static bool loadModificationSetting(string userid, Persona[] UserPersonas)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (SettingsRptXmlHelper.CheckPersonalFeatures(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.checkPlanCodeAltLender(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalInputForms(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalFieldAccess(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalMilestones(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalLoanFolders(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalTools(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalServices(userid, UserPersonas))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalFeatureConfigs(userid))
      {
        flag2 = true;
        flag1 = false;
      }
      if (flag1 && SettingsRptXmlHelper.CheckPersonalExportServices(userid, UserPersonas))
        flag2 = true;
      return flag2;
    }

    private static bool CheckPersonalFeatures(string userid)
    {
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) FeatureSets.BizContacts);
      arrayList.AddRange((ICollection) FeatureSets.BorContacts);
      arrayList.AddRange((ICollection) FeatureSets.Contacts);
      arrayList.AddRange((ICollection) FeatureSets.Features);
      arrayList.AddRange((ICollection) FeatureSets.PipelineGlobalTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanMgmtFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoansPrintFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabPersonalFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabCompanyFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ToolsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanOtherFeatures);
      arrayList.AddRange((ICollection) FeatureSets.DashboardFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ReportFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TradeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.HomeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.EMClosingDocsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ExternalSettingTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOAdministrationTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LOConnectStandardFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOSiteSettingsTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.AIQFeatures);
      return FeaturesAclDbAccessor.GetPermissions((AclFeature[]) arrayList.ToArray(typeof (AclFeature)), userid).Count > 0;
    }

    private static bool checkPlanCodeAltLender(string userid)
    {
      return FeaturesAclDbAccessor.GetPermissions(new AclFeature[2]
      {
        AclFeature.LoanTab_Other_PlanCode,
        AclFeature.LoanTab_Other_AltLender
      }, userid).Count > 0;
    }

    private static bool CheckPersonalInputForms(string userid)
    {
      Hashtable permissionsForAllForms = InputFormsAclDbAccessor.GetPermissionsForAllForms(userid);
      return permissionsForAllForms != null && permissionsForAllForms.Count > 0;
    }

    private static bool CheckPersonalFieldAccess(string userid)
    {
      bool flag = false;
      Dictionary<string, AclTriState> fieldsPermission = FieldAccessAclDbAccessor.GetFieldsPermission(userid);
      if (fieldsPermission != null && fieldsPermission.Count > 0)
        flag = true;
      return flag;
    }

    private static bool CheckPersonalMilestones(string userid)
    {
      bool flag = false;
      Hashtable personalPermission = MilestonesAclDbAccessor.GetPersonalPermission((AclMilestone[]) null, userid);
      if (personalPermission != null && personalPermission.Count > 0)
        flag = true;
      return flag;
    }

    private static bool CheckPersonalLoanFolders(string userid)
    {
      bool flag = false;
      LoanFolderAclInfo[] userLoanFolders1 = LoanFoldersAclDbAccessor.GetUserLoanFolders(AclFeature.LoanMgmt_Move, "", userid);
      if (userLoanFolders1 != null && userLoanFolders1.Length != 0)
        flag = true;
      if (flag)
        return flag;
      LoanFolderAclInfo[] userLoanFolders2 = LoanFoldersAclDbAccessor.GetUserLoanFolders(AclFeature.LoanMgmt_Import, "", userid);
      if (userLoanFolders2 != null && userLoanFolders2.Length != 0)
        flag = true;
      return flag;
    }

    private static bool CheckPersonalTools(string userid)
    {
      bool flag = false;
      ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetUserToolsConfiguration("", -1, userid);
      if (toolsConfiguration != null && toolsConfiguration.Length != 0)
        flag = true;
      return flag;
    }

    private static bool CheckPersonalFeatureConfigs(string userid)
    {
      bool flag = false;
      Dictionary<AclFeature, int> permissions = FeatureConfigsAclDbAccessor.GetPermissions(FeatureSets.AllConfigs, userid);
      if (permissions != null && permissions.Count > 0)
        flag = true;
      return flag;
    }

    private static bool CheckPersonalServices(string userid, Persona[] UserPersonas)
    {
      if (ServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, userid, AclUtils.GetPersonaIDs(UserPersonas)) != ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
        return true;
      ServiceAclInfo[] permissions = ServicesAclDbAccessor.GetPermissions(AclFeature.LoanTab_Other_ePASS, userid, AclUtils.GetPersonaIDs(UserPersonas));
      bool flag = false;
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (serviceAclInfo.CustomAccess != AclResourceAccess.None)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private static bool CheckPersonalExportServices(string userid, Persona[] UserPersonas)
    {
      if (ExportServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(AclFeature.LoanMgmt_MgmtPipelineServices, userid, AclUtils.GetPersonaIDs(UserPersonas)) != ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified)
        return true;
      ExportServiceAclInfo[] permissions = ExportServicesAclDbAccessor.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, userid, AclUtils.GetPersonaIDs(UserPersonas), ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray());
      bool flag = false;
      foreach (ExportServiceAclInfo exportServiceAclInfo in permissions)
      {
        if (exportServiceAclInfo.CustomAccess != AclResourceAccess.None)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public static AclFeature[] getFeatureList(Dictionary<string, string> reportParameters)
    {
      ArrayList arrayList = new ArrayList();
      string str1 = reportParameters.ContainsKey("LoansAccess") ? reportParameters["LoansAccess"] : "False";
      string str2 = reportParameters.ContainsKey("ToolsAccess") ? reportParameters["ToolsAccess"] : "False";
      string str3 = reportParameters.ContainsKey("FormsAccess") ? reportParameters["FormsAccess"] : "False";
      string str4 = reportParameters.ContainsKey("PipelineAccess") ? reportParameters["PipelineAccess"] : "False";
      string str5 = reportParameters.ContainsKey("HomePageAccess") ? reportParameters["HomePageAccess"] : "False";
      string str6 = reportParameters.ContainsKey("EFolderAccess") ? reportParameters["EFolderAccess"] : "False";
      string str7 = reportParameters.ContainsKey("TCDRAccess") ? reportParameters["TCDRAccess"] : "False";
      string str8 = reportParameters.ContainsKey("SettingsAccess") ? reportParameters["SettingsAccess"] : "False";
      string str9 = reportParameters.ContainsKey("ExternalSettingsAccess") ? reportParameters["ExternalSettingsAccess"] : "False";
      string str10 = reportParameters.ContainsKey("TPOADMINAccess") ? reportParameters["TPOADMINAccess"] : "False";
      string str11 = reportParameters.ContainsKey("ConsumerConnectAccess") ? reportParameters["ConsumerConnectAccess"] : "False";
      string str12 = reportParameters.ContainsKey("LOConnectAccess") ? reportParameters["LOConnectAccess"] : "False";
      string str13 = reportParameters.ContainsKey("EVaultAccess") ? reportParameters["EVaultAccess"] : "False";
      if ((reportParameters.ContainsKey("DevConnectAccess") ? reportParameters["DevConnectAccess"] : "False").Equals("True"))
      {
        arrayList.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks);
        arrayList.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange);
      }
      if (str1.Equals("True"))
      {
        arrayList.AddRange((ICollection) FeatureSets.LoansPrintFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ItemizationFeeFeatures);
        arrayList.AddRange((ICollection) FeatureSets.LoanOtherFeatures);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailures);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ViewDocData);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_OverrideDocData);
        arrayList.Add((object) AclFeature.LoanTab_Other_AltLender);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ConfigureClosingOptionsDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailuresDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocsDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocsDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocsDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ClosingPackageManagmentDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ApproveForSigningDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PackagePreviewDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PackageExpirationDigitalClosing);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_eNoteTab);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseRegistration);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Transfer);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Deactivate);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseDeactivation);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingComplianceDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingAdditionalDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingMoveDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingDeselectDocs);
        arrayList.Add((object) AclFeature.LoanTab_EMClosingDocs_PreCloseDocs);
      }
      if (str4.Equals("True"))
      {
        arrayList.AddRange((ICollection) FeatureSets.LoanMgmtFeatures);
        arrayList.AddRange((ICollection) FeatureSets.PipelineGlobalTabFeatures);
      }
      if (str5.Equals("True"))
        arrayList.Add((object) AclFeature.HomeTab_ManageHomePageAccount);
      if (str2.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.ToolsFeatures);
      if (str8.Equals("True"))
      {
        arrayList.AddRange((ICollection) FeatureSets.SettingsTabCompanyFeatures);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_AssignmentOfRights);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfileName);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfileEmail);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfilePhoto);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfilePhone);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfileCell);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_MyProfileFax);
        arrayList.Add((object) AclFeature.SettingsTab_Personal_DefaultFileContacts);
        arrayList.Add((object) AclFeature.SettingsTab_Company_CustomInputFormEditor);
        arrayList.Add((object) AclFeature.SettingsTab_Company_DiagnosticMode);
      }
      if (str7.Equals("True"))
      {
        arrayList.AddRange((ICollection) FeatureSets.TradeFeatures);
        arrayList.AddRange((ICollection) FeatureSets.BorContacts);
        arrayList.AddRange((ICollection) FeatureSets.BizContacts);
        arrayList.Add((object) AclFeature.Cnt_Campaign_Access);
        arrayList.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther);
        arrayList.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates);
        arrayList.Add((object) AclFeature.Cnt_Synchronization);
        arrayList.Add((object) AclFeature.Cnt_Contacts_Update);
        arrayList.AddRange((ICollection) FeatureSets.DashboardFeatures);
        arrayList.AddRange((ICollection) FeatureSets.ReportFeatures);
        arrayList.Add((object) AclFeature.ReportTab_ReportingDB);
      }
      if (str6.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
      if (str9.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.ExternalSettingTabFeatures);
      if (str10.Equals("True"))
      {
        arrayList.AddRange((ICollection) FeatureSets.TPOAdministrationTabFeatures);
        arrayList.AddRange((ICollection) FeatureSets.TPOSiteSettingsTabFeatures);
      }
      if (str11.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
      if (str13.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.eVaultFeatures);
      if (str12.Equals("True"))
        arrayList.AddRange((ICollection) FeatureSets.LOConnectStandardFeatures);
      return (AclFeature[]) arrayList.ToArray(typeof (AclFeature));
    }

    public static string createPersonasXML(
      List<string> reportFilters,
      Dictionary<string, string> reportParameters,
      string reportID)
    {
      int xRefCount = 1;
      string loansAccess = reportParameters.ContainsKey("LoansAccess") ? reportParameters["LoansAccess"] : "False";
      string str1 = reportParameters.ContainsKey("ToolsAccess") ? reportParameters["ToolsAccess"] : "False";
      string str2 = reportParameters.ContainsKey("FormsAccess") ? reportParameters["FormsAccess"] : "False";
      string pipelineAccess = reportParameters.ContainsKey("PipelineAccess") ? reportParameters["PipelineAccess"] : "False";
      string str3 = reportParameters.ContainsKey("HomePageAccess") ? reportParameters["HomePageAccess"] : "False";
      string str4 = reportParameters.ContainsKey("EFolderAccess") ? reportParameters["EFolderAccess"] : "False";
      string str5 = reportParameters.ContainsKey("TCDRAccess") ? reportParameters["TCDRAccess"] : "False";
      string str6 = reportParameters.ContainsKey("SettingsAccess") ? reportParameters["SettingsAccess"] : "False";
      string str7 = reportParameters.ContainsKey("ExternalSettingsAccess") ? reportParameters["ExternalSettingsAccess"] : "False";
      string str8 = reportParameters.ContainsKey("TPOADMINAccess") ? reportParameters["TPOADMINAccess"] : "False";
      string str9 = reportParameters.ContainsKey("ConsumerConnectAccess") ? reportParameters["ConsumerConnectAccess"] : "False";
      string loConnectAccess = reportParameters.ContainsKey("LOConnectAccess") ? reportParameters["LOConnectAccess"] : "False";
      string aiqAccess = reportParameters.ContainsKey("IceMortgageTechAiq") ? reportParameters["IceMortgageTechAiq"] : "False";
      string str10 = reportParameters.ContainsKey("DevConnectAccess") ? reportParameters["DevConnectAccess"] : "False";
      AclFeature[] featureList = SettingsRptXmlHelper.getFeatureList(reportParameters);
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement("SettingsData");
      xmlDocument.AppendChild((XmlNode) element1);
      foreach (string reportFilter in reportFilters)
      {
        if (SettingsReportAccessor.CancelJob(reportID))
          return "CANCELLED";
        XmlElement element2 = element1.OwnerDocument.CreateElement("Persona");
        Persona personaByName = PersonaAccessor.GetPersonaByName(reportFilter);
        element2.SetAttribute("Name", reportFilter);
        element2.SetAttribute("ID", personaByName.ID.ToString());
        SettingsRptXmlHelper.createPermissionsXML(element2, personaByName, featureList, pipelineAccess, loansAccess, loConnectAccess, aiqAccess);
        if (loansAccess.Equals("True"))
          SettingsRptXmlHelper.createMilestoneWorkflowAccessXML(element2, personaByName, ref xRefCount);
        if (str2.Equals("True"))
          SettingsRptXmlHelper.createPersonaFormsXml(element2, personaByName.ID, ref xRefCount);
        if (str1.Equals("True"))
          SettingsRptXmlHelper.createPersonaToolsXml(element2, personaByName.ID, ref xRefCount);
        if (str3.Equals("True"))
          SettingsRptXmlHelper.createHomePageXML(element2, personaByName);
        if (str4.Equals("True"))
          SettingsRptXmlHelper.updateeFolderAccess(element2, FeaturesAclDbAccessor.GetPermission(AclFeature.GlobalTab_Pipeline, personaByName.ID));
        if (pipelineAccess.Equals("True"))
        {
          if (string.Equals(Company.GetCompanySetting("POLICIES", "EnableLoanSoftArchival"), "true", StringComparison.OrdinalIgnoreCase))
          {
            element2.SelectSingleNode("./Permissions/Access[@Type='LoanMgmt_SearchArchiveFolders']").Attributes["Value"].Value = "False";
          }
          else
          {
            element2.SelectSingleNode("./Permissions/Access[@Type='LoanMgmt_AccessToArchiveLoans']").Attributes["Value"].Value = "False";
            element2.SelectSingleNode("./Permissions/Access[@Type='LoanMgmt_AccessToArchiveFolders']").Attributes["Value"].Value = "False";
          }
        }
        element1.AppendChild((XmlNode) element2);
      }
      return element1.OuterXml;
    }

    private static void updateeFolderAccess(XmlElement personaNode, bool isPipelineAccess)
    {
      bool flag = personaNode.SelectSingleNode("./Permissions/Access[@Type='eFolder_AccessToDocumentTab']").Attributes["Value"].Value.Equals("True");
      if (isPipelineAccess && flag)
        return;
      AclFeature[] featureList = SettingsRptXmlHelper.getFeatureList(new Dictionary<string, string>()
      {
        {
          "EFolderAccess",
          "True"
        }
      });
      AclFeature[] source = new AclFeature[28]
      {
        AclFeature.eFolder_Conditions_PreliminaryCondition,
        AclFeature.eFolder_Conditions_AddEditDeleteCondPreliminary,
        AclFeature.eFolder_Conditions_AddBusinessRulePreliminary,
        AclFeature.eFolder_Conditions_PreliminaryCondTab_ImportCond,
        AclFeature.eFolder_Conditions_UnderWritingCondTab,
        AclFeature.eFolder_Conditions_UW_NewEditImpDel,
        AclFeature.eFolder_Conditions_UW_EditUser,
        AclFeature.eFolder_Conditions_UW_EditDate,
        AclFeature.eFolder_Conditions_AddBusinessRuleUnderwriting,
        AclFeature.eFolder_Conditions_UW_PriorTo,
        AclFeature.eFolder_Conditions_UW_Status_Fulfilled,
        AclFeature.eFolder_Conditions_UW_Status_Received,
        AclFeature.eFolder_Conditions_UW_Status_Reviewed,
        AclFeature.eFolder_Conditions_UW_Status_Rejected,
        AclFeature.eFolder_Conditions_UW_Status_Cleared,
        AclFeature.eFolder_Conditions_UW_Status_Waived,
        AclFeature.eFolder_Conditions_UnderwritingCond_ImportCond,
        AclFeature.eFolder_Conditions_UW_EditComment,
        AclFeature.eFolder_Conditions_UW_AddSupportDoc,
        AclFeature.eFolder_Conditions_UW_RemoveSupportDoc,
        AclFeature.eFolder_Conditions_PostClosingCondTab,
        AclFeature.eFolder_Conditions_AddBusinessRulePostClosing,
        AclFeature.eFolder_Conditions_PostClosingCondition_ImportCond,
        AclFeature.eFolder_Conditions_PCCT_NewEditImpDel,
        AclFeature.eFolder_Conditions_SellCondTab,
        AclFeature.eFolder_Conditions_SellCond_AddEditDel,
        AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond,
        AclFeature.eFolder_Conditions_HistoryTab
      };
      List<AclFeature> aclFeatureList = new List<AclFeature>();
      foreach (AclFeature aclFeature in featureList)
      {
        if (!((IEnumerable<AclFeature>) source).Contains<AclFeature>(aclFeature))
          aclFeatureList.Add(aclFeature);
      }
      foreach (AclFeature aclFeature in aclFeatureList)
        personaNode.SelectSingleNode("./Permissions/Access[@Type='" + (object) aclFeature + "']").Attributes["Value"].Value = "False";
      if (isPipelineAccess)
        return;
      foreach (AclFeature aclFeature in source)
        personaNode.SelectSingleNode("./Permissions/Access[@Type='" + (object) aclFeature + "']").Attributes["Value"].Value = "False";
    }

    public static void createHomePageXML(XmlElement root, Persona psa)
    {
      ClientContext current = ClientContext.GetCurrent();
      int maxAllowed = 0;
      string appSetting1 = EnConfigurationSettings.AppSettings["DataServicesUrl"];
      string appSetting2 = EnConfigurationSettings.AppSettings["LoginServicesUrl"];
      string sso = HomePageService.getSSO(current.ClientID, appSetting2, "admin", Company.GetEdition(current).ToString());
      Hashtable moduleSettings = HomePageService.GetModuleSettings(current.ClientID, appSetting1, psa.Name, psa.ID, out maxAllowed, sso);
      XmlElement element1 = root.OwnerDocument.CreateElement("HomePage");
      XmlElement element2 = element1.OwnerDocument.CreateElement("Modules");
      foreach (string key in (IEnumerable) moduleSettings.Keys)
      {
        HomePageModuleSettings pageModuleSettings = (HomePageModuleSettings) moduleSettings[(object) key];
        XmlElement element3 = element2.OwnerDocument.CreateElement("Module");
        string str = SecurityElement.Escape(pageModuleSettings.Title);
        element3.SetAttribute("MustHave", pageModuleSettings.IsLocked ? "True" : "False");
        element3.SetAttribute("ShowbyDefault", pageModuleSettings.IsDefault ? "True" : "False");
        element3.SetAttribute("Accessible", pageModuleSettings.IsAccessible ? "True" : "False");
        element3.InnerText = str.Trim();
        element2.AppendChild((XmlNode) element3);
      }
      element1.AppendChild((XmlNode) element2);
      root.AppendChild((XmlNode) element1);
    }

    public static void createPermissionsXML(
      XmlElement root,
      Persona psa,
      AclFeature[] features,
      string pipelineAccess,
      string loansAccess,
      string loConnectAccess,
      string aiqAccess)
    {
      XmlElement newChild = new ElementWriter(root).Append("Permissions");
      foreach (DictionaryEntry permission in FeaturesAclDbAccessor.GetPermissions(features, psa.ID))
      {
        XmlElement element = newChild.OwnerDocument.CreateElement("Access");
        element.SetAttribute("Type", permission.Key.ToString());
        element.SetAttribute("Value", permission.Value.ToString());
        newChild.AppendChild((XmlNode) element);
      }
      if (loConnectAccess.Equals("True"))
      {
        foreach (StandardWebFormInfo standardWebFormInfo in StandardWebFormsAclDbAccessor.GetFormsByPersona(psa.ID))
        {
          XmlElement element = newChild.OwnerDocument.CreateElement("Access");
          element.SetAttribute("Type", standardWebFormInfo.FormName.ToString());
          element.SetAttribute("Value", standardWebFormInfo.Access.ToString());
          newChild.AppendChild((XmlNode) element);
        }
      }
      if (aiqAccess.Equals("True"))
      {
        foreach (KeyValuePair<AclFeature, int> permission in FeatureConfigsAclDbAccessor.GetPermissions(FeatureSets.AIQFeatures, psa.ID))
        {
          XmlElement element = newChild.OwnerDocument.CreateElement("Access");
          element.SetAttribute("Type", permission.Key.ToString());
          element.SetAttribute("Value", permission.Value.ToString());
          newChild.AppendChild((XmlNode) element);
        }
      }
      if (loansAccess.Equals("True"))
      {
        ServicesAclDbAccessor.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, psa.ID);
        ServiceAclInfo[] permissions = ServicesAclDbAccessor.GetPermissions(AclFeature.LoanTab_Other_ePASS, psa.ID);
        bool flag = false;
        foreach (ServiceAclInfo serviceAclInfo in permissions)
        {
          if (serviceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite)
          {
            flag = true;
            break;
          }
        }
        XmlElement element = newChild.OwnerDocument.CreateElement("Access");
        element.SetAttribute("Type", AclFeature.LoanTab_Other_ePASS.ToString());
        element.SetAttribute("Value", flag.ToString());
        newChild.AppendChild((XmlNode) element);
      }
      if (pipelineAccess.Equals("True"))
      {
        LoanFolderAclInfo[] personaLoanFolders1 = LoanFoldersAclDbAccessor.GetPersonaLoanFolders(AclFeature.LoanMgmt_Import, "", psa.ID.ToString());
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        foreach (LoanFolderAclInfo loanFolderAclInfo in personaLoanFolders1)
        {
          switch (loanFolderAclInfo.FolderName)
          {
            case "Calyx Point":
              if (loanFolderAclInfo.MoveFromAccess == 1)
              {
                flag1 = true;
                break;
              }
              break;
            case "Fannie Mae 3.x":
              if (loanFolderAclInfo.MoveFromAccess == 1)
              {
                flag2 = true;
                break;
              }
              break;
            case "ULAD":
              if (loanFolderAclInfo.MoveFromAccess == 1)
              {
                flag3 = true;
                break;
              }
              break;
          }
        }
        XmlElement element1 = newChild.OwnerDocument.CreateElement("Access");
        element1.SetAttribute("Type", "Import_Loans_Calyx_Point");
        element1.SetAttribute("Value", flag1.ToString());
        newChild.AppendChild((XmlNode) element1);
        XmlElement element2 = newChild.OwnerDocument.CreateElement("Access");
        element2.SetAttribute("Type", "Import_Loans_Fannie_Mae_3x");
        element2.SetAttribute("Value", flag2.ToString());
        newChild.AppendChild((XmlNode) element2);
        XmlElement element3 = newChild.OwnerDocument.CreateElement("Access");
        element3.SetAttribute("Type", "Import_Loans_ULAD");
        element3.SetAttribute("Value", flag3.ToString());
        newChild.AppendChild((XmlNode) element3);
        ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServicesAclDbAccessor.GetServicesDefaultSetting(AclFeature.LoanMgmt_MgmtPipelineServices, psa.ID);
        XmlElement element4 = newChild.OwnerDocument.CreateElement("Access");
        element4.SetAttribute("Type", "PipeLine_Services");
        element4.SetAttribute("Value", servicesDefaultSetting.ToString());
        newChild.AppendChild((XmlNode) element4);
        LoanFolderAclInfo[] personaLoanFolders2 = LoanFoldersAclDbAccessor.GetPersonaLoanFolders(AclFeature.LoanMgmt_Move, "", psa.ID.ToString());
        XmlElement element5 = newChild.OwnerDocument.CreateElement("Access");
        element5.SetAttribute("Type", AclFeature.LoanMgmt_Move.ToString());
        string str = "False";
        if (personaLoanFolders2 != null && personaLoanFolders2.Length != 0)
        {
          foreach (LoanFolderAclInfo loanFolderAclInfo in personaLoanFolders2)
          {
            if (loanFolderAclInfo.MoveFromAccess == 1 || loanFolderAclInfo.MoveToAccess == 1)
            {
              str = "True";
              break;
            }
          }
        }
        element5.SetAttribute("Value", str);
        newChild.AppendChild((XmlNode) element5);
        List<ExportServiceAclInfo> exportServicesList = ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229);
        foreach (ExportServiceAclInfo permission in ExportServicesAclDbAccessor.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, psa.ID, exportServicesList.ToArray()))
        {
          XmlElement element6 = newChild.OwnerDocument.CreateElement("Access");
          element6.SetAttribute("Type", permission.ExportGroup);
          element6.SetAttribute("Value", permission.Access.ToString());
          newChild.AppendChild((XmlNode) element6);
        }
      }
      root.AppendChild((XmlNode) newChild);
    }

    public static void createMilestoneWorkflowAccessXML(
      XmlElement root,
      Persona psa,
      ref int xRefCount)
    {
      XmlElement xmlElement = new ElementWriter(root).Append("Milestone_Workflow_Management");
      Dictionary<string, bool> forCustomMilestone1 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.AcceptFiles, psa.ID);
      Dictionary<string, bool> forCustomMilestone2 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.FinishMilestone, psa.ID);
      Dictionary<string, bool> forCustomMilestone3 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.ReturnFiles, psa.ID);
      Dictionary<string, bool> forCustomMilestone4 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.ChangeExpectedDate, psa.ID);
      Dictionary<string, bool> forCustomMilestone5 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.EditMilestoneComments, psa.ID);
      Dictionary<string, bool> forCustomMilestone6 = MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(AclMilestone.AssignLoanTeamMembers, psa.ID);
      XmlElement workFlowNode1 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Accept_Files", forCustomMilestone1, ref xRefCount);
      XmlElement workFlowNode2 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Finish_Milestone", forCustomMilestone2, ref xRefCount);
      XmlElement workFlowNode3 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Return_Files", forCustomMilestone3, ref xRefCount);
      XmlElement workFlowNode4 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Change_Expected_Dates", forCustomMilestone4, ref xRefCount);
      XmlElement workFlowNode5 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Edit_MS_Comments", forCustomMilestone5, ref xRefCount);
      XmlElement workFlowNode6 = SettingsRptXmlHelper.createWorkFlowNode(xmlElement, psa, "Assign_LT_Member", forCustomMilestone6, ref xRefCount);
      xmlElement.AppendChild((XmlNode) workFlowNode1);
      xmlElement.AppendChild((XmlNode) workFlowNode2);
      xmlElement.AppendChild((XmlNode) workFlowNode3);
      xmlElement.AppendChild((XmlNode) workFlowNode4);
      xmlElement.AppendChild((XmlNode) workFlowNode5);
      xmlElement.AppendChild((XmlNode) workFlowNode6);
      root.AppendChild((XmlNode) xmlElement);
    }

    public static XmlElement createWorkFlowNode(
      XmlElement root,
      Persona psa,
      string workFlowName,
      Dictionary<string, bool> permissions,
      ref int xRefCount)
    {
      XmlElement element1 = root.OwnerDocument.CreateElement(workFlowName);
      List<EllieMae.EMLite.Workflow.Milestone> milestones = WorkflowBpmDbAccessor.GetMilestones(false);
      string str1 = "";
      for (int index = 0; index < milestones.Count; ++index)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = milestones[index];
        if (permissions.ContainsKey(milestone.MilestoneID) && permissions[milestone.MilestoneID])
        {
          str1 = milestone.Name + ",";
          string str2 = milestone.Name + (milestone.Archived ? " (Archived)" : "");
          XmlElement element2 = element1.OwnerDocument.CreateElement("XRef");
          element2.SetAttribute("RefID", xRefCount.ToString());
          element2.SetAttribute("EntityUID", str2);
          element2.SetAttribute("EntityType", "Milestone");
          element2.SetAttribute("EntityID", milestone.MilestoneID);
          element1.AppendChild((XmlNode) element2);
          XmlElement element3 = root.OwnerDocument.CreateElement("DependsOn");
          element3.SetAttribute("RefID", xRefCount.ToString());
          root.AppendChild((XmlNode) element3);
          ++xRefCount;
        }
      }
      return element1;
    }

    private static void createPersonaFormsXml(
      XmlElement rootNode,
      int personaId,
      ref int xRefCount)
    {
      bool permission = FeaturesAclDbAccessor.GetPermission(AclFeature.GlobalTab_Pipeline, personaId);
      XmlElement xmlElement = new ElementWriter(rootNode).Append("Form_Access");
      Hashtable permissionsForAllForms = InputFormsAclDbAccessor.GetPermissionsForAllForms(personaId);
      InputFormInfo[] formInfos = InputForms.GetFormInfos(InputFormType.All, InputFormCategory.Form);
      List<XmlElement> dependsOnNodes = new List<XmlElement>();
      List<XmlElement> xrefNodes = new List<XmlElement>();
      foreach (InputFormInfo formInfo in formInfos)
      {
        if (!InputFormInfo.IsChildForm(formInfo.FormID))
        {
          ExternalEntityRef formsExternalEntity = SettingsRptXmlHelper.createFormsExternalEntity(formInfo, permissionsForAllForms, permission);
          formsExternalEntity.XRefId = xRefCount++;
          dependsOnNodes.Add(SettingsRptXmlHelper.writeDependsOnElement(xmlElement, formsExternalEntity));
          xrefNodes.Add(SettingsRptXmlHelper.writeXRefElement(xmlElement, formsExternalEntity));
        }
      }
      SettingsRptXmlHelper.writeToRootNode(rootNode, xmlElement, dependsOnNodes, xrefNodes);
    }

    private static XmlElement writeXRefElement(
      XmlElement element,
      ExternalEntityRef externalEntityRef)
    {
      XmlElement element1 = element.OwnerDocument.CreateElement("XRef");
      element1.SetAttribute("RefID", externalEntityRef.XRefId.ToString());
      element1.SetAttribute("EntityID", externalEntityRef.EntityId);
      element1.SetAttribute("EntityType", externalEntityRef.EntityType);
      element1.SetAttribute("EntityUID", externalEntityRef.EntityUid);
      element1.SetAttribute("Access", externalEntityRef.Access ? "True" : "False");
      return element1;
    }

    private static ExternalEntityRef createFormsExternalEntity(
      InputFormInfo formInfo,
      Hashtable personaFormPerms,
      bool isPipelineAccess)
    {
      if (formInfo == (InputFormInfo) null)
        return (ExternalEntityRef) null;
      return new ExternalEntityRef()
      {
        EntityId = formInfo.FormID.ToString(),
        EntityType = "Form",
        EntityUid = formInfo.Name,
        Access = isPipelineAccess && personaFormPerms.Contains((object) formInfo.FormID) && (bool) personaFormPerms[(object) formInfo.FormID]
      };
    }

    private static ExternalEntityRef createMilestoneExternalEntity(
      EllieMae.EMLite.Workflow.Milestone milestoneInfo,
      bool access)
    {
      if (milestoneInfo == null)
        return (ExternalEntityRef) null;
      return new ExternalEntityRef()
      {
        EntityId = milestoneInfo.MilestoneID.ToString(),
        EntityType = "Milestone",
        EntityUid = milestoneInfo.Name,
        Access = access
      };
    }

    private static ExternalEntityRef createRolesExternalEntity(RoleInfo roleInfo, bool access)
    {
      if (roleInfo == null)
        return (ExternalEntityRef) null;
      return new ExternalEntityRef()
      {
        EntityId = roleInfo.RoleID.ToString(),
        EntityType = "Role",
        EntityUid = roleInfo.RoleName,
        Access = access
      };
    }

    private static XmlElement writeDependsOnElement(
      XmlElement element,
      ExternalEntityRef externalEntityRefs)
    {
      XmlElement element1 = element.OwnerDocument.CreateElement("DependsOn");
      element1.SetAttribute("RefID", externalEntityRefs.XRefId.ToString());
      return element1;
    }

    private static void writeToRootNode(
      XmlElement rootNode,
      XmlElement childNode,
      List<XmlElement> dependsOnNodes,
      List<XmlElement> xrefNodes)
    {
      foreach (XmlElement dependsOnNode in dependsOnNodes)
        childNode.AppendChild((XmlNode) dependsOnNode);
      foreach (XmlElement xrefNode in xrefNodes)
        childNode.AppendChild((XmlNode) xrefNode);
      rootNode.AppendChild((XmlNode) childNode);
    }

    private static void createPersonaToolsXml(
      XmlElement rootNode,
      int personaId,
      ref int xRefCount)
    {
      XmlElement xmlElement = new ElementWriter(rootNode).Append("Grant_Write_Access");
      ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetPersonaToolsConfiguration("", -1, personaId.ToString());
      List<EllieMae.EMLite.Workflow.Milestone> milestones = WorkflowBpmDbAccessor.GetMilestones(false);
      RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
      List<XmlElement> dependsOnNodes = new List<XmlElement>();
      List<XmlElement> xrefNodes = new List<XmlElement>();
      if (toolsConfiguration != null && toolsConfiguration.Length != 0)
      {
        foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestones)
        {
          EllieMae.EMLite.Workflow.Milestone milestoneInfo = milestone;
          if (milestoneInfo.RoleID > 0 && WorkflowBpmDbAccessor.GetRoleFunction(milestoneInfo.RoleID) != null)
          {
            ToolsAclInfo toolsAclInfo = ((IEnumerable<ToolsAclInfo>) toolsConfiguration).Where<ToolsAclInfo>((Func<ToolsAclInfo, bool>) (tool => tool.MilestoneID == milestoneInfo.MilestoneID)).SingleOrDefault<ToolsAclInfo>();
            if (toolsAclInfo != null)
            {
              ExternalEntityRef milestoneExternalEntity = SettingsRptXmlHelper.createMilestoneExternalEntity(milestoneInfo, toolsAclInfo.Access == 1);
              milestoneExternalEntity.XRefId = xRefCount++;
              dependsOnNodes.Add(SettingsRptXmlHelper.writeDependsOnElement(xmlElement, milestoneExternalEntity));
              xrefNodes.Add(SettingsRptXmlHelper.writeXRefElement(xmlElement, milestoneExternalEntity));
            }
          }
        }
        foreach (ToolsAclInfo toolsAclInfo in toolsConfiguration)
        {
          ToolsAclInfo toolInfo = toolsAclInfo;
          if (toolInfo.MilestoneID == "-1")
          {
            ExternalEntityRef rolesExternalEntity = SettingsRptXmlHelper.createRolesExternalEntity(((IEnumerable<RoleInfo>) allRoleFunctions).Where<RoleInfo>((Func<RoleInfo, bool>) (role => role.RoleID == toolInfo.RoleID)).Single<RoleInfo>(), toolInfo.Access == 1);
            rolesExternalEntity.XRefId = xRefCount++;
            dependsOnNodes.Add(SettingsRptXmlHelper.writeDependsOnElement(xmlElement, rolesExternalEntity));
            xrefNodes.Add(SettingsRptXmlHelper.writeXRefElement(xmlElement, rolesExternalEntity));
          }
        }
      }
      SettingsRptXmlHelper.writeToRootNode(rootNode, xmlElement, dependsOnNodes, xrefNodes);
      if (((IEnumerable<ToolsAclInfo>) toolsConfiguration).Where<ToolsAclInfo>((Func<ToolsAclInfo, bool>) (tool => tool.Access == 1)).Count<ToolsAclInfo>() > 0)
        rootNode.SelectSingleNode("Permissions/Access[@Type='ToolsTab_GrantWriteAccess']").Attributes["Value"].Value = "True";
      else
        rootNode.SelectSingleNode("Permissions/Access[@Type='ToolsTab_GrantWriteAccess']").Attributes["Value"].Value = "False";
    }

    public static string createUserGroupXML(
      List<string> reportFilters,
      Dictionary<string, string> reportParameters,
      string reportID)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement("SettingsData");
      xmlDocument.AppendChild((XmlNode) element1);
      string str1 = reportParameters.ContainsKey("Members") ? reportParameters["Members"] : "False";
      string str2 = reportParameters.ContainsKey("LoanAccess") ? reportParameters["LoanAccess"] : "False";
      string str3 = reportParameters.ContainsKey("BorrowContacts") ? reportParameters["BorrowContacts"] : "False";
      string str4 = reportParameters.ContainsKey("LoanTemplates") ? reportParameters["LoanTemplates"] : "False";
      string str5 = reportParameters.ContainsKey("Resources") ? reportParameters["Resources"] : "False";
      string str6 = reportParameters.ContainsKey("RolesList") ? reportParameters["RolesList"] : "False";
      foreach (string reportFilter in reportFilters)
      {
        XmlElement element2 = xmlDocument.CreateElement("SettingsUserGroupData");
        element1.AppendChild((XmlNode) element2);
        AclGroup groupByName = AclGroupAccessor.GetGroupByName(reportFilter);
        element2.SetAttribute("name", groupByName.Name);
        element2.SetAttribute("id", groupByName.ID.ToString());
        XmlElement xmlElement1 = (XmlElement) null;
        XmlElement xmlElement2 = (XmlElement) null;
        XmlElement xmlElement3 = (XmlElement) null;
        if (str4.Equals("True") || str5.Equals("True"))
        {
          xmlElement1 = xmlDocument.CreateElement("SettingsResources");
          xmlElement2 = xmlDocument.CreateElement("Folders");
          xmlElement3 = xmlDocument.CreateElement("Files");
          xmlElement1.AppendChild((XmlNode) xmlElement2);
          xmlElement1.AppendChild((XmlNode) xmlElement3);
          element2.AppendChild((XmlNode) xmlElement1);
        }
        if (str1.Equals("True"))
          SettingsRptXmlHelper.generateUserGroupOrgSettingsData(element2, groupByName.ID);
        if (str2.Equals("True"))
        {
          XmlElement element3 = xmlDocument.CreateElement("SettingsLoanData");
          element2.AppendChild((XmlNode) element3);
          SettingsRptXmlHelper.generateUserGroupLoanMembersSettingsData(element3, groupByName.ID);
          SettingsRptXmlHelper.generateUserGroupLoanFoldersSettingsData(element3, groupByName.ID);
        }
        if (str3.Equals("True"))
          SettingsRptXmlHelper.generateUserGroupBorrowerContactsXml(element2, groupByName.ID);
        if (str4.Equals("True"))
          SettingsRptXmlHelper.generateUserGroupLoanTemplatesXml(xmlElement1, xmlElement2, xmlElement3, groupByName.ID);
        if (str5.Equals("True"))
          SettingsRptXmlHelper.generateUserGroupResourcesXml(xmlElement1, xmlElement2, xmlElement3, groupByName.ID);
        if (str6.Equals("True"))
          SettingsRptXmlHelper.generateUserGroupRolesSettingsData(element2, groupByName.ID);
      }
      return element1.OuterXml;
    }

    private static void generateUserGroupOrgSettingsData(XmlElement root, int aclGroupId)
    {
      Dictionary<string, object> membersInGroup = AclGroupAccessor.GetMembersInGroup(aclGroupId);
      OrgInGroup[] orgInGroupArray = !membersInGroup.ContainsKey("OrgList") || membersInGroup["OrgList"] == null ? new OrgInGroup[0] : (OrgInGroup[]) membersInGroup["OrgList"];
      UserInfo[] userInfoArray = !membersInGroup.ContainsKey("UserList") || membersInGroup["UserList"] == null ? new UserInfo[0] : (UserInfo[]) membersInGroup["UserList"];
      XmlElement element1 = root.OwnerDocument.CreateElement("SettingsMembersData");
      XmlElement element2 = root.OwnerDocument.CreateElement("Groups");
      for (int index = 0; index < orgInGroupArray.Length; ++index)
      {
        XmlElement element3 = element1.OwnerDocument.CreateElement("Group");
        OrgInGroup orgInGroup = orgInGroupArray[index];
        element3.SetAttribute("id", orgInGroup.OrgID.ToString());
        element3.SetAttribute("name", orgInGroup.OrgName);
        element3.SetAttribute("include_levels_below", orgInGroup.IsInclusive.ToString());
        element2.AppendChild((XmlNode) element3);
      }
      element1.AppendChild((XmlNode) element2);
      XmlElement element4 = root.OwnerDocument.CreateElement("Users");
      for (int index = 0; index < userInfoArray.Length; ++index)
      {
        UserInfo userInfo1 = userInfoArray[index];
        if (!(userInfo1 == (UserInfo) null))
        {
          XmlElement element5 = element1.OwnerDocument.CreateElement("User");
          UserInfo userInfo2 = userInfoArray[index];
          element5.SetAttribute("id", userInfo1.Userid);
          element5.SetAttribute("name", userInfo1.FullName + " (" + userInfo1.Userid + ")");
          element4.AppendChild((XmlNode) element5);
        }
      }
      element1.AppendChild((XmlNode) element4);
      root.AppendChild((XmlNode) element1);
    }

    private static void generateUserGroupLoanMembersSettingsData(XmlElement root, int aclGroupId)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      XmlElement element1 = root.OwnerDocument.CreateElement("AccessToOthersLoans");
      XmlElement element2 = root.OwnerDocument.CreateElement("Groups");
      AclGroupLoanMembers membersInGroupLoan = AclGroupLoanAccessor.GetMembersInGroupLoan(aclGroupId);
      for (int index = 0; index < membersInGroupLoan.OrgMembers.Length; ++index)
      {
        OrgInGroupLoan orgMember = membersInGroupLoan.OrgMembers[index];
        string orgName = orgMember.OrgName;
        bool isInclusive = orgMember.IsInclusive;
        string str1 = isInclusive.ToString();
        string str2 = orgName + "_" + str1;
        if (!stringList1.Contains(str2))
        {
          XmlElement element3 = element2.OwnerDocument.CreateElement("Group");
          element3.SetAttribute("id", orgMember.OrgID.ToString());
          element3.SetAttribute("name", orgMember.OrgName);
          XmlElement xmlElement = element3;
          isInclusive = orgMember.IsInclusive;
          string str3 = isInclusive.ToString();
          xmlElement.SetAttribute("include_levels_below", str3);
          element3.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(orgMember.Access));
          element2.AppendChild((XmlNode) element3);
          stringList1.Add(str2);
        }
      }
      element1.AppendChild((XmlNode) element2);
      XmlElement element4 = root.OwnerDocument.CreateElement("Users");
      if (membersInGroupLoan.UserMembers != null && membersInGroupLoan.UserMembers.Length != 0)
      {
        List<string> stringList3 = new List<string>();
        foreach (UserInGroupLoan userMember in membersInGroupLoan.UserMembers)
        {
          if (!stringList3.Contains(userMember.UserID))
            stringList3.Add(userMember.UserID);
        }
        UserInfo[] users = User.GetUsers(stringList3.ToArray());
        for (int index = 0; index < membersInGroupLoan.UserMembers.Length; ++index)
        {
          UserInGroupLoan user = membersInGroupLoan.UserMembers[index];
          UserInfo userInfo = ((IEnumerable<UserInfo>) users).Where<UserInfo>((Func<UserInfo, bool>) (a => a.Userid.Equals(user.UserID))).First<UserInfo>();
          if (!(userInfo == (UserInfo) null) && !stringList2.Contains(user.UserID))
          {
            XmlElement element5 = element2.OwnerDocument.CreateElement("User");
            element5.SetAttribute("id", userInfo.Userid);
            element5.SetAttribute("name", userInfo.FullName + " (" + userInfo.Userid + ")");
            element5.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(user.Access));
            element4.AppendChild((XmlNode) element5);
            stringList2.Add(user.UserID);
          }
        }
      }
      element1.AppendChild((XmlNode) element4);
      root.AppendChild((XmlNode) element1);
    }

    private static void generateUserGroupLoanFoldersSettingsData(XmlElement root, int aclGroupId)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      XmlElement element1 = root.OwnerDocument.CreateElement("AccessToLoanFolders");
      LoanFolderInGroup[] groupLoanFolders = AclGroupLoanAccessor.GetAclGroupLoanFolders(aclGroupId);
      Hashtable hashtable = new Hashtable(groupLoanFolders.Length);
      for (int index = 0; index < groupLoanFolders.Length; ++index)
        hashtable.Add((object) groupLoanFolders[index].FolderName, (object) groupLoanFolders[index]);
      LoanFolderInfo[] allLoanFolderInfos = LoanFolder.GetAllLoanFolderInfos(true);
      if (allLoanFolderInfos == null || allLoanFolderInfos.Length == 0)
        return;
      for (int index = 0; index < allLoanFolderInfos.Length; ++index)
      {
        XmlElement element2 = element1.OwnerDocument.CreateElement("LoanFolder");
        element2.InnerText = allLoanFolderInfos[index].DisplayName;
        if (hashtable.Contains((object) allLoanFolderInfos[index].Name))
        {
          LoanFolderInGroup loanFolderInGroup = (LoanFolderInGroup) hashtable[(object) allLoanFolderInfos[index].Name];
          element2.SetAttribute("access", loanFolderInGroup.Accessible.ToString());
        }
        else
          element2.SetAttribute("access", "False");
        element1.AppendChild((XmlNode) element2);
      }
      root.AppendChild((XmlNode) element1);
    }

    private static void generateUserGroupBorrowerContactsXml(XmlElement rootNode, int groupID)
    {
      XmlElement element1 = rootNode.OwnerDocument.CreateElement("SettingsBorrowerContactsData");
      AclGroup groupById = AclGroupAccessor.GetGroupById(groupID);
      element1.SetAttribute("are_public_viewablebysuperiors", groupById.ViewSubordinatesContacts.ToString().ToLower());
      element1.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(groupById.ContactAccess, groupById.ViewSubordinatesContacts));
      XmlElement element2 = rootNode.OwnerDocument.CreateElement("AccessToOtherUsers");
      AclGroupContactMembers membersInGroupContact = AclGroupContactAccessor.GetMembersInGroupContact(groupID);
      XmlElement element3 = rootNode.OwnerDocument.CreateElement("Groups");
      foreach (OrgInGroupContact orgMember in membersInGroupContact.OrgMembers)
      {
        if (element3.SelectNodes(string.Format("Group[@id='{0}']", (object) orgMember.OrgID)).Count <= 0)
        {
          XmlElement element4 = rootNode.OwnerDocument.CreateElement("Group");
          element4.SetAttribute("id", orgMember.OrgID.ToString());
          element4.SetAttribute("name", orgMember.OrgName);
          element4.SetAttribute("include_levels_below", orgMember.IsInclusive.ToString().ToLower());
          element4.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(orgMember.Access));
          element3.AppendChild((XmlNode) element4);
        }
      }
      element2.AppendChild((XmlNode) element3);
      XmlElement element5 = rootNode.OwnerDocument.CreateElement("Users");
      if (membersInGroupContact.UserMembers != null && membersInGroupContact.UserMembers.Length != 0)
      {
        List<string> stringList = new List<string>();
        foreach (UserInGroupContact userMember in membersInGroupContact.UserMembers)
        {
          if (!stringList.Contains(userMember.UserID))
            stringList.Add(userMember.UserID);
        }
        UserInfo[] users = User.GetUsers(stringList.ToArray());
        foreach (UserInGroupContact userMember1 in membersInGroupContact.UserMembers)
        {
          UserInGroupContact userMember = userMember1;
          XmlElement element6 = rootNode.OwnerDocument.CreateElement("User");
          UserInfo userInfo = ((IEnumerable<UserInfo>) users).Where<UserInfo>((Func<UserInfo, bool>) (u => string.Compare(u.Userid.Trim(), userMember.UserID.Trim(), StringComparison.OrdinalIgnoreCase) == 0)).FirstOrDefault<UserInfo>();
          element6.SetAttribute("name", userInfo.FullName);
          element6.SetAttribute("id", userMember.UserID);
          element6.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(userMember.Access));
          element5.AppendChild((XmlNode) element6);
        }
      }
      element2.AppendChild((XmlNode) element5);
      element1.AppendChild((XmlNode) element2);
      rootNode.AppendChild((XmlNode) element1);
    }

    private static string getAccessRightDescription(
      AclResourceAccess resourecAccess,
      bool ViewSubordinatesContacts = true)
    {
      if (!ViewSubordinatesContacts)
        return string.Empty;
      return resourecAccess != AclResourceAccess.ReadWrite ? "View Only" : "Edit";
    }

    private static void generateUserGroupLoanTemplatesXml(
      XmlElement settingsResourcesNode,
      XmlElement foldersNode,
      XmlElement filesNode,
      int groupID)
    {
      Hashtable filesTable = new Hashtable();
      foreach (AclFileType aclFileType in new List<AclFileType>((IEnumerable<AclFileType>) new AclFileType[9]
      {
        AclFileType.LoanProgram,
        AclFileType.ClosingCost,
        AclFileType.DocumentSet,
        AclFileType.FormList,
        AclFileType.MiscData,
        AclFileType.LoanTemplate,
        AclFileType.TaskSet,
        AclFileType.SettlementServiceProviders,
        AclFileType.AffiliatedBusinessArrangements
      }))
      {
        FileInGroup[] aclGroupFileRefs = AclGroupFileAccessor.GetAclGroupFileRefs(groupID, aclFileType);
        for (int index = 0; index < aclGroupFileRefs.Length; ++index)
          filesTable.Add((object) aclGroupFileRefs[index].FileID, (object) aclGroupFileRefs[index]);
        SettingsRptXmlHelper.generateFoldersFilesXmlNode(foldersNode, filesNode, aclFileType, filesTable);
        filesTable.Clear();
      }
    }

    private static void generateUserGroupResourcesXml(
      XmlElement settingsResourcesNode,
      XmlElement foldersNode,
      XmlElement filesNode,
      int groupID)
    {
      Hashtable filesTable = new Hashtable();
      AclFileType[] collection = new AclFileType[9]
      {
        AclFileType.CustomPrintForms,
        AclFileType.PrintGroups,
        AclFileType.BorrowerCustomLetters,
        AclFileType.BizCustomLetters,
        AclFileType.Reports,
        AclFileType.CampaignTemplate,
        AclFileType.DashboardTemplate,
        AclFileType.DashboardViewTemplate,
        AclFileType.ConditionalApprovalLetter
      };
      foreach (AclFileType aclFileType in new List<AclFileType>((IEnumerable<AclFileType>) collection))
      {
        FileInGroup[] aclGroupFileRefs = AclGroupFileAccessor.GetAclGroupFileRefs(groupID, aclFileType);
        for (int index = 0; index < aclGroupFileRefs.Length; ++index)
          filesTable.Add((object) aclGroupFileRefs[index].FileID, (object) aclGroupFileRefs[index]);
        SettingsRptXmlHelper.generateFoldersFilesXmlNode(foldersNode, filesNode, aclFileType, filesTable);
        filesTable.Clear();
      }
      foreach (string groupStdPrintForm in AclGroupStdPrintFormAccessor.GetAclGroupStdPrintForms(groupID))
      {
        XmlElement element = filesNode.OwnerDocument.CreateElement("File");
        element.SetAttribute("type", "StandardPrintForms");
        element.SetAttribute("name", groupStdPrintForm);
        filesNode.AppendChild((XmlNode) element);
      }
      BizGroupRef[] contactGroupRefs = AclGroupBizGroupAccessor.GetBizContactGroupRefs(groupID);
      Hashtable hashtable = (Hashtable) null;
      if (((IEnumerable<BizGroupRef>) contactGroupRefs).Count<BizGroupRef>() > 0)
        hashtable = SettingsRptXmlHelper.buildBizGroupIdToNameTable();
      foreach (BizGroupRef bizGroupRef in contactGroupRefs)
      {
        XmlElement element = filesNode.OwnerDocument.CreateElement("File");
        element.SetAttribute("type", "BizContacts");
        element.SetAttribute("name", hashtable[(object) bizGroupRef.BizGroupID].ToString());
        element.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(bizGroupRef.Access));
        filesNode.AppendChild((XmlNode) element);
      }
    }

    private static Hashtable buildBizGroupIdToNameTable()
    {
      Hashtable nameTable = new Hashtable();
      foreach (ContactGroupInfo publicBizContactGroup in ContactGroupProvider.GetPublicBizContactGroups())
        nameTable.Add((object) publicBizContactGroup.GroupId, (object) publicBizContactGroup.GroupName);
      return nameTable;
    }

    private static void generateFoldersFilesXmlNode(
      XmlElement foldersNode,
      XmlElement filesNode,
      AclFileType filetype,
      Hashtable filesTable)
    {
      int[] fileIds = new int[filesTable.Count];
      filesTable.Keys.CopyTo((Array) fileIds, 0);
      AclFileResource[] aclFileResources = AclGroupFileAccessor.GetAclFileResources(fileIds);
      for (int index = 0; index < aclFileResources.Length; ++index)
      {
        FileInGroup fileInGroup = (FileInGroup) filesTable[(object) aclFileResources[index].FileID];
        if (aclFileResources[index].IsFolder)
        {
          XmlElement element = foldersNode.OwnerDocument.CreateElement("Folder");
          element.SetAttribute("type", filetype.ToString());
          element.SetAttribute("name", aclFileResources[index].FilePath.Replace("Public:", string.Empty).ToString());
          element.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(fileInGroup.Access));
          element.SetAttribute("include_levels_below", fileInGroup.IsInclusive ? "True" : "False");
          foldersNode.AppendChild((XmlNode) element);
        }
        else
        {
          XmlElement element = filesNode.OwnerDocument.CreateElement("File");
          element.SetAttribute("type", filetype.ToString());
          element.SetAttribute("name", aclFileResources[index].FilePath.Replace("Public:", string.Empty).ToString());
          if (filetype != AclFileType.ConditionalApprovalLetter)
          {
            element.SetAttribute("access_right", SettingsRptXmlHelper.getAccessRightDescription(fileInGroup.Access));
            element.SetAttribute("include_levels_below", fileInGroup.IsInclusive ? "True" : "False");
          }
          filesNode.AppendChild((XmlNode) element);
        }
      }
    }

    private static void generateUserGroupRolesSettingsData(
      XmlElement userGroupParentNode,
      int aclGroupId)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      XmlElement element1 = userGroupParentNode.OwnerDocument.CreateElement("SettingsRoleListViewData");
      foreach (RoleInfo allRoleFunction in WorkflowBpmDbAccessor.GetAllRoleFunctions())
      {
        XmlElement element2 = element1.OwnerDocument.CreateElement("AccessToRoleList");
        element2.SetAttribute("group_can_view", allRoleFunction.RoleName);
        AclGroupRoleAccessLevel groupRoleAccessLevel = AclGroupRoleAccessor.GetAclGroupRoleAccessLevel(aclGroupId, allRoleFunction.RoleID);
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4;
        if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.All)
          flag1 = true;
        else if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.BelowInOrg)
        {
          flag2 = true;
        }
        else
        {
          flag3 = true;
          AclGroupRoleMembers membersInGroupRole = AclGroupRoleAccessor.GetMembersInGroupRole(aclGroupId, allRoleFunction.RoleID);
          XmlElement element3 = element1.OwnerDocument.CreateElement("Groups");
          for (int index = 0; index < membersInGroupRole.OrgMembers.Length; ++index)
          {
            OrgInGroupRole orgMember = membersInGroupRole.OrgMembers[index];
            string orgName = orgMember.OrgName;
            flag4 = orgMember.IsInclusive;
            string str1 = flag4.ToString();
            string str2 = orgName + "_" + str1;
            if (!stringList1.Contains(str2))
            {
              XmlElement element4 = element2.OwnerDocument.CreateElement("Group");
              element4.SetAttribute("id", orgMember.OrgID.ToString());
              element4.SetAttribute("name", orgMember.OrgName);
              XmlElement xmlElement = element4;
              flag4 = orgMember.IsInclusive;
              string str3 = flag4.ToString();
              xmlElement.SetAttribute("include_levels_below", str3);
              element3.AppendChild((XmlNode) element4);
              stringList1.Add(str2);
            }
          }
          element2.AppendChild((XmlNode) element3);
          XmlElement element5 = element2.OwnerDocument.CreateElement("Users");
          if (membersInGroupRole.UserMembers != null && membersInGroupRole.UserMembers.Length != 0)
          {
            List<string> stringList3 = new List<string>();
            foreach (UserInGroupRole userMember in membersInGroupRole.UserMembers)
            {
              if (!stringList3.Contains(userMember.UserID))
                stringList3.Add(userMember.UserID);
            }
            UserInfo[] users = User.GetUsers(stringList3.ToArray());
            foreach (UserInGroupRole userMember1 in membersInGroupRole.UserMembers)
            {
              UserInGroupRole userMember = userMember1;
              XmlElement element6 = element2.OwnerDocument.CreateElement("User");
              UserInfo userInfo = ((IEnumerable<UserInfo>) users).Where<UserInfo>((Func<UserInfo, bool>) (u => string.Compare(u.Userid.Trim(), userMember.UserID.Trim(), StringComparison.OrdinalIgnoreCase) == 0)).FirstOrDefault<UserInfo>();
              element6.SetAttribute("name", userInfo.FullName);
              element6.SetAttribute("id", userMember.UserID);
              element5.AppendChild((XmlNode) element6);
            }
          }
          element2.AppendChild((XmlNode) element5);
        }
        element2.SetAttribute("all_roles", flag1.ToString());
        element2.SetAttribute("roles_below_organization_hierarhy", flag2.ToString());
        element2.SetAttribute("some_roles", flag3.ToString());
        XmlElement xmlElement1 = element2;
        flag4 = groupRoleAccessLevel.HideDisabledAccount;
        string str = flag4.ToString();
        xmlElement1.SetAttribute("do_not_show_disabled_user_account", str);
        element1.AppendChild((XmlNode) element2);
      }
      userGroupParentNode.AppendChild((XmlNode) element1);
    }
  }
}
