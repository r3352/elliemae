// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ConfigurationManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.DirectoryServices.Proxies;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.BidTapeManagement;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerCommon;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Server.ServerObjects.StatusOnline;
using EllieMae.EMLite.Server.Tasks;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ConfigurationManager : SessionBoundObject, IConfigurationManager
  {
    private const string className = "ConfigurationManager";
    private const string COMPANY_STATUS_ONLINE_SECTION = "CompanyStatusOnline";
    private const string PERSONAL_TRIGGER_TYPE = "PersonalTriggerType";
    private EncompassSystemInfo encompassSystemInfo;

    public ConfigurationManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (ConfigurationManager).ToLower());
      return this;
    }

    public virtual string GetSsoTokenExpirationTimeForEdm()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSsoTokenExpirationTimeForEdm), Array.Empty<object>());
      try
      {
        return "60";
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 5.ToString();
      }
    }

    public virtual EncompassSystemInfo GetEncompassSystemInfo()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEncompassSystemInfo), Array.Empty<object>());
      try
      {
        return this.getSystemInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EncompassSystemInfo) null;
      }
    }

    public virtual void SetSecondaryFields(ArrayList list, SecondaryFieldTypes type)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetSecondaryFields), new object[2]
      {
        (object) list,
        (object) type
      });
      try
      {
        SystemConfiguration.SetSecondaryFields(list, type);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Save secondary field information"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual object[] GetAllSecondaryFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllSecondaryFields), Array.Empty<object>());
      try
      {
        object[] allSecondaryFields = SystemConfiguration.GetAllSecondaryFields();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved all secondary field information"));
        return allSecondaryFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (object[]) null;
      }
    }

    public virtual ArrayList GetSecondaryFields(SecondaryFieldTypes type)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSecondaryFields), new object[1]
      {
        (object) type
      });
      try
      {
        ArrayList secondaryFields = SystemConfiguration.GetSecondaryFields(type);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved secondary field information"));
        return secondaryFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    public virtual void SetSecondarySecurityTypes(DataTable table)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetSecondarySecurityTypes), new object[1]
      {
        (object) table
      });
      try
      {
        SystemConfiguration.SetSecondarySecurityTypes(table);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Save secondary security types."));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DataTable GetSecondarySecurityTypes()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSecondarySecurityTypes), Array.Empty<object>());
      try
      {
        DataTable secondarySecurityTypes = SystemConfiguration.GetSecondarySecurityTypes();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved secondary security types."));
        return secondarySecurityTypes;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DataTable) null;
      }
    }

    public virtual void SetFannieMaeProductNames(DataTable table)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetFannieMaeProductNames), new object[1]
      {
        (object) table
      });
      try
      {
        SystemConfiguration.SetFannieMaeProductNames(table);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Save fannie mae product names."));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DataTable GetFannieMaeProductNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFannieMaeProductNames), Array.Empty<object>());
      try
      {
        DataTable fannieMaeProductNames = SystemConfiguration.GetFannieMaeProductNames();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved fannie mae product names."));
        return fannieMaeProductNames;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DataTable) null;
      }
    }

    public virtual DataTable GetLoanSynchronizationFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanSynchronizationFields), Array.Empty<object>());
      try
      {
        DataTable synchronizationFields = SystemConfiguration.GetLoanSynchronizationFields();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("get loan syncronization field setting."));
        return synchronizationFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DataTable) null;
      }
    }

    public virtual void SetLoanSynchronizationFields(DataTable table)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetLoanSynchronizationFields), new object[1]
      {
        (object) table
      });
      try
      {
        SystemConfiguration.SetLoanSynchronizationFields(table);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Save loan syncronization field setting."));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetAllPublicCustomLetterFileEntries(
      CustomLetterType contactType)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllCustomLetterFileEntries", new object[1]
      {
        (object) contactType
      });
      try
      {
        return CustomLetterStore.GetAllSystemEntries(contactType, (string) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetAllPublicTemplateSettingsSystemEntries(
      TemplateSettingsType type,
      bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllPublicTemplateSettingsFileEntries", new object[2]
      {
        (object) type,
        (object) includeProperties
      });
      try
      {
        return TemplateSettingsStore.GetAllPublicSystemEntries(type, includeProperties);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetAllPublicFormGroupDirEntries(bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllPublicFormGroupDirEntries), new object[1]
      {
        (object) includeProperties
      });
      try
      {
        return FormGroups.GetAllPublicSystemEntries(includeProperties);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual CompanyInfo GetCompanyInfo()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyInfo), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Call Company.GetCompanyInfo()."));
        CompanyInfo companyInfo = Company.GetCompanyInfo(this.Session.Context);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved master company info"));
        return companyInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (CompanyInfo) null;
      }
    }

    public virtual void UpdateCompanyInfo(CompanyInfo info, AclFeature feature)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateCompanyInfo), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("CompanyInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        try
        {
          this.Security.DemandRootAdministrator();
        }
        catch (Exception ex)
        {
          this.Security.DemandFeatureAccess(feature);
        }
        Company.UpdateCompanyInfo(info);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Master company information updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetCompanySetting(string section, string key, string str)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetCompanySetting), new object[3]
      {
        (object) section,
        (object) key,
        (object) str
      });
      if ((section ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Section cannot be blank or null", nameof (section), this.Session.SessionInfo));
      if ((key ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Key cannot be blank or null", nameof (key), this.Session.SessionInfo));
      try
      {
        Company.SetCompanySetting(section, key, str);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Company setting \"" + section + "/" + key + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetCompanySetting(string section, string key)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanySetting), new object[2]
      {
        (object) section,
        (object) key
      });
      if ((section ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Section cannot be blank or null", nameof (section), this.Session.SessionInfo));
      if ((key ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Key cannot be blank or null", nameof (key), this.Session.SessionInfo));
      try
      {
        return Company.GetCompanySetting(section, key);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual Hashtable GetCompanySettings(string section)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanySettings), new object[1]
      {
        (object) section
      });
      if ((section ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Section cannot be blank or null", nameof (section), this.Session.SessionInfo));
      try
      {
        return Company.GetCompanySettings(section);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual HMDAInformation GetHMDAInformation()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHMDAInformation), Array.Empty<object>());
      try
      {
        HMDAInformation hmdaInformation = Company.GetHMDAInformation();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved HMDA information"));
        return hmdaInformation;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HMDAInformation) null;
      }
    }

    public virtual void UpdateHMDAInformation(HMDAInformation hmdaInformation)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateHMDAInformation), new object[1]
      {
        (object) hmdaInformation
      });
      try
      {
        Company.UpdateHMDAInformation(hmdaInformation);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<HMDAProfile> GetHMDAProfile()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHMDAProfile), Array.Empty<object>());
      try
      {
        List<HMDAProfile> hmdaProfiles = HMDAProfileDbAccessor.GetHMDAProfiles();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved HMDA profiles"));
        return hmdaProfiles;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<HMDAProfile>) null;
      }
    }

    public virtual HMDAProfile GetHMDAProfileById(int profileId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHMDAProfileById), Array.Empty<object>());
      try
      {
        HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(profileId);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved HMDA profile By Id"));
        return hmdaProfileById;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HMDAProfile) null;
      }
    }

    public virtual void UpdateHMDAProfile(HMDAProfile hmdaProfile)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateHMDAProfile), Array.Empty<object>());
      try
      {
        HMDAProfileDbAccessor.UpdateHMDAProfile(hmdaProfile);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new HMDAAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, hmdaProfile.HMDAProfileID == 0 ? EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDACreated : EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDAModified, DateTime.Now, hmdaProfile.HMDAProfileID, AuditObjectType.HMDA_Profile, hmdaProfile.HMDAProfileName));
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Update HMDA profiles"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool DoesProfileNameExist(string profileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DoesProfileNameExist), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Check HMDA profile name"));
        return HMDAProfileDbAccessor.DoesProfileNameExist(profileName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual void DeleteProfileName(string profileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteProfileName), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Delete profile name"));
        HMDAProfileDbAccessor.DeleteHMDAProfile(profileName);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new HMDAAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.HMDADeleted, DateTime.Now, 0, AuditObjectType.HMDA_Profile, profileName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool IsAssociateToOrg(int profileID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsAssociateToOrg), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Check if a HMDA profile is associate to a orgnization"));
        return HMDAProfileDbAccessor.IsAssociateToOrg(profileID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual string GetOrgNameByHMDAProfile(int profileID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOrgNameByHMDAProfile), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Get a Org name from HMDA profile"));
        return HMDAProfileDbAccessor.GetOrgNameByHMDAProfile(profileID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual List<IRS4506TTemplate> GetIRS4506TTemplates()
    {
      return this.GetIRS4506TTemplates(false);
    }

    public virtual (List<IRS4506TTemplate> Templates, int TotalCount) GetPagedIRS4506TTemplates(
      string[] formVersions = null,
      int start = 0,
      int limit = 0)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPagedIRS4506TTemplates), new object[3]
      {
        (object) formVersions,
        (object) start,
        (object) limit
      });
      try
      {
        (List<IRS4506TTemplate> Templates, int TotalCount) irS4506Ttemplates = IRS4506TTemplateDbAccessor.GetPagedIRS4506TTemplates(formVersions, start, limit);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved tax transcript request templates"));
        return (irS4506Ttemplates.Templates, irS4506Ttemplates.TotalCount);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return ((List<IRS4506TTemplate>) null, 0);
      }
    }

    public virtual List<IRS4506TTemplate> GetIRS4506TTemplates(bool listOnly)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetIRS4506TTemplates), new object[1]
      {
        (object) listOnly
      });
      try
      {
        List<IRS4506TTemplate> irS4506Ttemplates = IRS4506TTemplateDbAccessor.GetIRS4506TTemplates(listOnly);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved tax transcript request templates"));
        return irS4506Ttemplates;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<IRS4506TTemplate>) null;
      }
    }

    public virtual IRS4506TTemplate GetIRS4506TTemplate(int templateID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetIRS4506TTemplate), new object[1]
      {
        (object) templateID
      });
      try
      {
        IRS4506TTemplate irS4506Ttemplate = IRS4506TTemplateDbAccessor.GetIRS4506TTemplate(templateID);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved the tax transcript request template by a template ID"));
        return irS4506Ttemplate;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IRS4506TTemplate) null;
      }
    }

    public virtual IRS4506TTemplate GetIRS4506TTemplate(string templateName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetIRS4506TTemplate), new object[1]
      {
        (object) templateName
      });
      try
      {
        IRS4506TTemplate irS4506Ttemplate = IRS4506TTemplateDbAccessor.GetIRS4506TTemplate(templateName);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved the tax transcript request template by a template name"));
        return irS4506Ttemplate;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IRS4506TTemplate) null;
      }
    }

    public virtual int CreateIRS4506TTemplate(IRS4506TTemplate template)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateIRS4506TTemplate), Array.Empty<object>());
      int irS4506Ttemplate = -1;
      try
      {
        irS4506Ttemplate = IRS4506TTemplateDbAccessor.CreateIRS4506TTemplate(template);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Create tax transcript request template"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, template.TemplateName, "DBTable-TaxTranscriptRequestTemplate"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return irS4506Ttemplate;
    }

    public virtual void UpdateIRS4506TTemplate(IRS4506TTemplate template)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateIRS4506TTemplate), Array.Empty<object>());
      try
      {
        IRS4506TTemplateDbAccessor.UpdateIRS4506TTemplate(template);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Update tax transcript request template"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified, DateTime.Now, template.TemplateName, "DBTable-TaxTranscriptRequestTemplate"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteIRS4506TTemplate(string templateName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteIRS4506TTemplate), Array.Empty<object>());
      try
      {
        IRS4506TTemplateDbAccessor.DeleteIRS4506TTemplate(templateName);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Delete tax transcript request template by name"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, templateName, "DBTable-TaxTranscriptRequestTemplate"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LicenseInfo GetServerLicense()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetServerLicense), Array.Empty<object>());
      try
      {
        LicenseInfo serverLicense = Company.GetServerLicense();
        if (serverLicense == null)
          throw new LicenseException((LicenseInfo) null, LicenseExceptionType.InvalidLicense, "Invalid or missing License information");
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved server license information"));
        return serverLicense;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LicenseInfo) null;
      }
    }

    public virtual void UpdateServerLicense(LicenseInfo license)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateServerLicense), new object[1]
      {
        (object) license
      });
      try
      {
        Company.UpdateServerLicense(license);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual UserLicenseInfo GetUserLicense(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUserLicense), new object[2]
      {
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        this.Security.DemandRootAdministrator();
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ObjectNotFoundException("User does not exist", ObjectType.User, (object) userId));
          return latestVersion.GetUserLicense();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (UserLicenseInfo) null;
      }
    }

    public virtual void UpdateUserLicense(UserLicenseInfo license)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateUserLicense), new object[1]
      {
        (object) license
      });
      try
      {
        this.Security.DemandRootAdministrator();
        using (User latestVersion = UserStore.GetLatestVersion(license.UserID))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ObjectNotFoundException("User does not exist", ObjectType.User, (object) license.UserID));
          latestVersion.UpdateUserLicense(license);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DefaultFieldsInfo GetDefaultFields(string root)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDefaultFields), new object[1]
      {
        (object) root
      });
      if (!((IEnumerable<string>) new string[3]
      {
        "RESPAFieldList",
        "PrivacyPolicyFieldList",
        "FHAConsumerChoiceFieldList"
      }).Contains<string>(root))
      {
        TraceLog.WriteWarning(nameof (ConfigurationManager), this.formatMsg("Invalid document name " + root));
        return (DefaultFieldsInfo) null;
      }
      try
      {
        DefaultFieldsInfo defaultFields = SystemConfiguration.GetDefaultFields(root);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved default field information"));
        return defaultFields;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DefaultFieldsInfo) null;
      }
    }

    public virtual void UpdateDefaultFields(DefaultFieldsInfo fieldInfo, string root)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateDefaultFields), new object[2]
      {
        (object) fieldInfo,
        (object) root
      });
      if (fieldInfo == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("DefaultFieldsInfo cannot be null", nameof (fieldInfo), this.Session.SessionInfo));
      try
      {
        SystemConfiguration.SaveDefaultFields(fieldInfo, root);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Default field information updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void MigrateLoanCustomFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), "MigrateCustomFields", (object[]) null);
      try
      {
        SystemConfiguration.MigrateLoanCustomFields(this.Session.UserID);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Migrate loan custom fields"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CustomFieldsInfo GetLoanCustomFields(bool forceToPrimaryDb = false)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetCustomFields", new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Settings)
      });
      try
      {
        using (this.Context.MakeCurrent(forcePrimaryDB: new bool?(forceToPrimaryDb)))
        {
          CustomFieldsInfo loanCustomFields = SystemConfiguration.GetLoanCustomFields();
          TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Get custom fields information"));
          return loanCustomFields;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (CustomFieldsInfo) null;
      }
    }

    public virtual CustomFieldInfo GetLoanCustomField(string fieldID)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetCustomField", new object[1]
      {
        (object) fieldID
      });
      if (fieldID == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("fieldID cannot be null", nameof (fieldID), this.Session.SessionInfo));
      try
      {
        CustomFieldInfo loanCustomField = SystemConfiguration.GetLoanCustomField(fieldID);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Get custom field information"));
        return loanCustomField;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (CustomFieldInfo) null;
      }
    }

    public virtual void UpdateLoanCustomField(CustomFieldInfo fieldInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLoanCustomField), new object[1]
      {
        (object) fieldInfo
      });
      if (fieldInfo == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("CustomFieldInfo cannot be null", nameof (fieldInfo), this.Session.SessionInfo));
      try
      {
        SystemConfiguration.UpdateLoanCustomField(fieldInfo, this.Session.UserID);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom field information updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateLoanCustomFields(
      CustomFieldsInfo fieldsInfo,
      bool cacheCustomFieldsInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLoanCustomFields), new object[2]
      {
        (object) fieldsInfo,
        (object) this.Session.UserID
      });
      if (fieldsInfo == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("CustomFieldsInfo cannot be null", nameof (fieldsInfo), this.Session.SessionInfo));
      try
      {
        SystemConfiguration.UpdateLoanCustomFields(fieldsInfo, this.Session.UserID);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom fields information updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteLoanCustomField(string fieldID)
    {
      this.DeleteLoanCustomFields(new string[1]{ fieldID });
    }

    public virtual void DeleteLoanCustomFields(string[] fieldIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteLoanCustomFields), (object[]) fieldIDs);
      if (fieldIDs == null)
        return;
      try
      {
        SystemConfiguration.DeleteLoanCustomFields(fieldIDs);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom fields deleted"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual EVerifyLoginInfoItem GetEVerifyLoginInfo(string userid, string vendorName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEVerifyLoginInfo), new object[1]
      {
        (object) userid
      });
      try
      {
        return EVerifyLoginInfoStore.Get(userid, vendorName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EVerifyLoginInfoItem) null;
      }
    }

    public virtual void SetEVerifyLoginInfo(string userid, EVerifyLoginInfoItem loginInfoItem)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetEVerifyLoginInfo), new object[1]
      {
        (object) userid
      });
      try
      {
        EVerifyLoginInfoStore.Set(userid, loginInfoItem);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanNumberingInfo GetLoanNumberingInfo()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanNumberingInfo), Array.Empty<object>());
      try
      {
        LoanNumberingInfo loanNumberingInfo = LoanNumberGenerator.GetLoanNumberingInfo();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved loan numbering information"));
        return loanNumberingInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanNumberingInfo) null;
      }
    }

    public virtual void UpdateLoanNumberingInfo(LoanNumberingInfo info)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLoanNumberingInfo), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("LoanNumberingInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        LoanNumberGenerator.SetLoanNumberingInfo(info);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Loan numbering settings updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MersNumberingInfo GetMersNumberingInfo()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMersNumberingInfo), Array.Empty<object>());
      try
      {
        MersNumberingInfo mersNumberingInfo = MersNumberGenerator.GetMersNumberingInfo();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved MERS numbering information"));
        return mersNumberingInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (MersNumberingInfo) null;
      }
    }

    public virtual void UpdateMersNumberingInfo(MersNumberingInfo info)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMersNumberingInfo), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("MersNumberingInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        MersNumberGenerator.SetMersNumberingInfo(info);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("MERS numbering settings updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BranchLoanNumberingInfo GetBranchLoanNumberingInfo(string orgCode)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBranchLoanNumberingInfo), new object[1]
      {
        (object) orgCode
      });
      try
      {
        return LoanNumberGenerator.GetBranchLoanNumberingInfo(orgCode);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchLoanNumberingInfo) null;
      }
    }

    public virtual BranchLoanNumberingInfo[] GetAllBranchLoanNumberingInfo(bool onlyInUse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllBranchLoanNumberingInfo), Array.Empty<object>());
      try
      {
        return LoanNumberGenerator.GetAllBranchLoanNumberingInfo(onlyInUse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchLoanNumberingInfo[]) null;
      }
    }

    public virtual BranchMERSMINNumberingInfo GetBranchMERSNumberingInfo(string mersminCode)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBranchMERSNumberingInfo), new object[1]
      {
        (object) mersminCode
      });
      try
      {
        return MersNumberGenerator.GetBranchMERSNumberingInfo(mersminCode);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchMERSMINNumberingInfo) null;
      }
    }

    public virtual BranchMERSMINNumberingInfo[] GetAllBranchMERSNumberingInfo(bool onlyInUse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllBranchMERSNumberingInfo), Array.Empty<object>());
      try
      {
        return MersNumberGenerator.GetAllBranchMERSMinNumberingInfo(onlyInUse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchMERSMINNumberingInfo[]) null;
      }
    }

    public virtual void SaveBranchMERSNumberingInfo(BranchMERSMINNumberingInfo info)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveBranchMERSNumberingInfo), new object[1]
      {
        (object) info
      });
      try
      {
        MersNumberGenerator.SaveBranchMERSNumberingInfo(info);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveBranchLoanNumberingInfo(BranchLoanNumberingInfo info)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveBranchLoanNumberingInfo), new object[1]
      {
        (object) info
      });
      try
      {
        LoanNumberGenerator.SaveBranchLoanNumberingInfo(info);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<LoanDuplicateChecker> GetLoanDuplicateInfo(
      string guid,
      List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo,
      EllieMae.EMLite.ClientServer.Address address,
      string loanFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanDuplicateInfo), new object[4]
      {
        (object) guid,
        (object) borrowerInfo,
        (object) address,
        (object) loanFolder
      });
      try
      {
        List<LoanDuplicateChecker> source = new List<LoanDuplicateChecker>();
        foreach (Dictionary<LoanDuplicateChecker.CheckField, string> dictionary in borrowerInfo.ToArray())
        {
          foreach (LoanDuplicateChecker duplicateChecker in LoanDuplicateAccessor.GetLoanDuplicateInfo(guid, dictionary[LoanDuplicateChecker.CheckField.FirstName], dictionary[LoanDuplicateChecker.CheckField.LastName], dictionary[LoanDuplicateChecker.CheckField.SSN], dictionary[LoanDuplicateChecker.CheckField.HomePhone], dictionary[LoanDuplicateChecker.CheckField.MobilePhone], dictionary[LoanDuplicateChecker.CheckField.Email], dictionary[LoanDuplicateChecker.CheckField.WorkEmail], address, loanFolder))
          {
            LoanDuplicateChecker check = duplicateChecker;
            if (source.FirstOrDefault<LoanDuplicateChecker>((System.Func<LoanDuplicateChecker, bool>) (item => item.GUID == check.GUID)) == null)
              source.Add(check);
          }
        }
        return source;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<LoanDuplicateChecker>) null;
      }
    }

    public virtual void SaveDuplicate(string loanGuid, string duplicateGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), "saveDuplicate", new object[2]
      {
        (object) loanGuid,
        (object) duplicateGuid
      });
      try
      {
        LoanDuplicateAccessor.saveDuplicate(loanGuid, duplicateGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetDuplicates(string loanGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDuplicates), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return LoanDuplicateAccessor.GetDuplicates(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual DuplicateScreenSetting GetDuplicateScreenSetting(
      string loanFolder,
      string loanName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDuplicateScreenSetting), Array.Empty<object>());
      try
      {
        return LoanDuplicateAccessor.GetDuplicateScreenSetting(this.Session.UserID, loanFolder, loanName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DuplicateScreenSetting) null;
      }
    }

    public virtual void SaveDuplicateScreenSetting(
      DuplicateScreenSetting setting,
      string loanFolder,
      string loanName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveDuplicateScreenSetting), new object[1]
      {
        (object) setting
      });
      try
      {
        LoanDuplicateAccessor.SaveDuplicateScreenSetting(this.Session.UserID, loanFolder, loanName, setting);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetSystemSettingsNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSystemSettingsNames), Array.Empty<object>());
      try
      {
        return SystemConfiguration.GetSystemSettingsNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual Dictionary<string, BinaryObject> GetMultipleSystemSettings(List<string> names)
    {
      Dictionary<string, BinaryObject> multipleSystemSettings = new Dictionary<string, BinaryObject>();
      this.onApiCalled(nameof (ConfigurationManager), "GetSystemSettings", new object[1]
      {
        (object) names
      });
      foreach (string name in names)
        multipleSystemSettings[name] = this.GetSystemSettingsInternal(name);
      return multipleSystemSettings;
    }

    private BinaryObject GetSystemSettingsInternal(string name)
    {
      if (string.IsNullOrEmpty(name))
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Settings name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      try
      {
        BinaryObject o = !(name == "VerifDays") ? SystemConfigurationAccessor.GetSystemSettings(name) : this.GetVerifDaysSettings();
        if (name == "PiggybackFields" && o == null)
          return (BinaryObject) null;
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved system settings \"" + name + "\""));
        return BinaryObject.Marshal(o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetSystemSettings(string name)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSystemSettings), new object[1]
      {
        (object) name
      });
      return this.GetSystemSettingsInternal(name);
    }

    public virtual BinaryObject GetVerifDaysSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetVerifDaysSettings), Array.Empty<object>());
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("Select * from VerifDays");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        StringBuilder stringBuilder = new StringBuilder();
        BinaryObject o;
        if (dataTable.Rows.Count == 0)
        {
          o = SystemConfiguration.GetSystemSettings("VerifDays");
          if (o == null)
          {
            stringBuilder.AppendLine("<objdata><element name=\"root\">");
            stringBuilder.AppendLine("</element></objdata>");
            o = new BinaryObject(stringBuilder.ToString(), Encoding.UTF8);
          }
          this.SaveVerifDaysSettings(o);
        }
        else
        {
          stringBuilder.AppendLine("<objdata><element name=\"root\">");
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            stringBuilder.AppendLine("<element name=\"" + row["Name"] + "\" >" + row["Days"] + "</element>");
          stringBuilder.AppendLine("</element></objdata>");
          o = new BinaryObject(stringBuilder.ToString(), Encoding.UTF8);
        }
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved Verif Days settings."));
        return BinaryObject.Marshal(o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveVerifDaysSettings(BinaryObject obj)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveVerifDaysSettings), Array.Empty<object>());
      try
      {
        SystemConfiguration.SaveVerifDaysSettings(obj);
        obj.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("VerifDays settings updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Hashtable GetSystemSettings(string[] names)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSystemSettings), (object[]) names);
      if (names == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Settings names cannot be blank or null", "name", this.Session.SessionInfo));
      try
      {
        Hashtable systemSettings = new Hashtable();
        for (int key = 0; key < names.Length; ++key)
        {
          systemSettings[(object) key] = (object) SystemConfiguration.GetSystemSettings(names[key]);
          TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved system settings \"" + names[key] + "\""));
        }
        return systemSettings;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SaveSystemSettings(string name, BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveSystemSettings), new object[2]
      {
        (object) name,
        (object) data
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Settings name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      if (name != "VerifDays")
        data.Download();
      try
      {
        if (name != "VerifDays")
          SystemConfigurationAccessor.SaveSystemSettings(name, data);
        else
          this.SaveVerifDaysSettings(data);
        data?.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("System settings \"" + name + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetCustomDataObjectNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomDataObjectNames), Array.Empty<object>());
      try
      {
        return SystemConfiguration.GetCustomDataObjectNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual BinaryObject GetCustomDataObject(string name)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomDataObject), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      try
      {
        BinaryObject customDataObject = SystemConfiguration.GetCustomDataObject(name);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved custom data object \"" + name + "\""));
        return BinaryObject.Marshal(customDataObject);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveCustomDataObject(string name, BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveCustomDataObject), new object[2]
      {
        (object) name,
        (object) data
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      data?.Download();
      try
      {
        SystemConfiguration.SaveCustomDataObject(name, data);
        data?.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom data file \"" + name + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AppendToCustomDataObject(string name, BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AppendToCustomDataObject), new object[2]
      {
        (object) name,
        (object) data
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Data value cannot be null", nameof (data), this.Session.SessionInfo));
      data.Download();
      try
      {
        SystemConfiguration.AppendToCustomDataObject(name, data);
        data.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom data file \"" + name + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DocumentTrackingSetup GetDocumentTrackingSetup()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocumentTrackingSetup), Array.Empty<object>());
      try
      {
        return DocumentTrackingConfiguration.GetDocumentTrackingSetup();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DocumentTrackingSetup) null;
      }
    }

    public virtual void SaveDocumentTrackingSetup(DocumentTrackingSetup setup)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveDocumentTrackingSetup), new object[1]
      {
        (object) setup
      });
      try
      {
        DocumentTrackingConfiguration.SaveDocumentTrackingSetup(setup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpsertDocumentTrackingTemplate(DocumentTrackingSetup setup, string guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpsertDocumentTrackingTemplate), new object[1]
      {
        (object) setup
      });
      try
      {
        DocumentTrackingConfiguration.UpsertDocumentTrackingTemplate(setup, guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ConditionTrackingSetup GetConditionTrackingSetup(ConditionType conditionType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetConditionTrackingSetup), new object[1]
      {
        (object) conditionType
      });
      try
      {
        switch (conditionType)
        {
          case ConditionType.Underwriting:
            return UnderwritingConditionListAccessor.GetUnderwritingConditionList();
          case ConditionType.PostClosing:
            return PostClosingConditionListAccessor.GetPostClosingConditionList();
          case ConditionType.Sell:
            return (ConditionTrackingSetup) UnderwritingConditionListAccessor.GetSellConditionList();
          default:
            return ConditionConfiguration.GetConditionTrackingSetup(conditionType);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ConditionTrackingSetup) null;
      }
    }

    public virtual void MigrateUnderwritingAndPostClosingConditionsFromXmlToDb()
    {
      this.onApiCalled(nameof (ConfigurationManager), "MigrateUnderwritingAndPostClosingConditionsToDb", Array.Empty<object>());
      try
      {
        PostClosingConditionListAccessor.MigratePostClosingConditionsToDb();
        UnderwritingConditionListAccessor.MigrateUnderwritingConditionsToDb();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddConditionTrackingSetup(ConditionTemplate conditionTemplate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddConditionTrackingSetup), new object[1]
      {
        (object) conditionTemplate
      });
      try
      {
        switch (conditionTemplate.ConditionType)
        {
          case ConditionType.Underwriting:
            UnderwritingConditionListAccessor.AddUnderwritingCondition((UnderwritingConditionTemplate) conditionTemplate, this.Session.SessionInfo.UserID);
            break;
          case ConditionType.PostClosing:
            PostClosingConditionListAccessor.AddPostClosingCondition((PostClosingConditionTemplate) conditionTemplate, this.Session.SessionInfo.UserID);
            break;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateConditionTrackingSetup(ConditionTemplate conditionTemplate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateConditionTrackingSetup), new object[1]
      {
        (object) conditionTemplate
      });
      try
      {
        switch (conditionTemplate.ConditionType)
        {
          case ConditionType.Underwriting:
            UnderwritingConditionListAccessor.UpdateUnderwritingCondition((UnderwritingConditionTemplate) conditionTemplate, this.Session.SessionInfo.UserID);
            break;
          case ConditionType.PostClosing:
            PostClosingConditionListAccessor.UpdatePostClosingCondition((PostClosingConditionTemplate) conditionTemplate, this.Session.SessionInfo.UserID);
            break;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteConditionTrackingSetup(List<ConditionTemplate> conditionTemplates)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteConditionTrackingSetup), new object[1]
      {
        (object) conditionTemplates
      });
      try
      {
        IEnumerable<Guid> source1 = conditionTemplates.Where<ConditionTemplate>((System.Func<ConditionTemplate, bool>) (cond => cond.ConditionType == ConditionType.PostClosing)).Select<ConditionTemplate, Guid>((System.Func<ConditionTemplate, Guid>) (cond => Guid.Parse(cond.Guid)));
        IEnumerable<Guid> source2 = conditionTemplates.Where<ConditionTemplate>((System.Func<ConditionTemplate, bool>) (cond => cond.ConditionType == ConditionType.Underwriting)).Select<ConditionTemplate, Guid>((System.Func<ConditionTemplate, Guid>) (cond => Guid.Parse(cond.Guid)));
        if (source1.Count<Guid>() > 0)
          PostClosingConditionListAccessor.DeletePostClosingCondition(source1.ToArray<Guid>());
        if (source2.Count<Guid>() <= 0)
          return;
        UnderwritingConditionListAccessor.DeleteUnderwritingCondition(source2.ToArray<Guid>());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DocumentGroupSetup GetDocumentGroupSetup()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocumentGroupSetup), Array.Empty<object>());
      try
      {
        return SystemConfigurationAccessor.GetDocumentGroupSetup();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DocumentGroupSetup) null;
      }
    }

    public virtual void SaveDocumentGroupSetup(DocumentGroupSetup setup)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveDocumentGroupSetup), new object[1]
      {
        (object) setup
      });
      try
      {
        SystemConfigurationAccessor.SaveDocumentGroupSetup(setup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual EDisclosureSetup GetEDisclosureSetup()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEDisclosureSetup), Array.Empty<object>());
      try
      {
        return EDisclosureConfigurationAccessor.GetEDisclosurePackageSetup();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EDisclosureSetup) null;
      }
    }

    public virtual void SaveEDisclosureSetup(EDisclosureSetup setup)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveEDisclosureSetup), new object[1]
      {
        (object) setup
      });
      try
      {
        EDisclosureConfigurationAccessor.SaveEDisclosurePackageSetup(setup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual EDisclosureSignOrderSetup GetEDisclosureSignOrderSetup()
    {
      try
      {
        return EDisclosureSignOrderConfiguration.GetEDisclosureSignOrderSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EDisclosureSignOrderSetup) null;
      }
    }

    public virtual void SaveEDisclosureSignOrderSetup(EDisclosureSignOrderSetup setup)
    {
      try
      {
        EDisclosureSignOrderConfiguration.SaveEDisclosureSignOrderSettings(setup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DataTable GetAllEdisclosureElementAttributes()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllEdisclosureElementAttributes), Array.Empty<object>());
      try
      {
        return EDisclosureConfigurationAccessor.GetAllEdisclosureElementAttributes();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DataTable) null;
      }
    }

    public virtual string GetEDisclosureElementAttributeId(
      int loanChannelType,
      int edisclosurePackageType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEDisclosureElementAttributeId), Array.Empty<object>());
      try
      {
        return EDisclosureConfigurationAccessor.GetEDisclosureElementAttributeId(loanChannelType, edisclosurePackageType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual string[] GetMilestoneTemplatesByChannelId(int channelId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMilestoneTemplatesByChannelId), Array.Empty<object>());
      try
      {
        return MilestoneRulesBpmDbAccessor.GetMilestoneTemplatesByChannelId(channelId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string[] GetMilestoneNamesByChannelId(int channelId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMilestoneNamesByChannelId), Array.Empty<object>());
      try
      {
        return MilestoneRulesBpmDbAccessor.GetMilestoneNamesByChannelId(channelId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual ImageAttachmentSettings GetImageAttachmentSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetImageAttachmentSettings), Array.Empty<object>());
      try
      {
        return ImageAttachmentSettingsStore.GetImageAttachmentSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ImageAttachmentSettings) null;
      }
    }

    public virtual void SaveImageAttachmentSettings(ImageAttachmentSettings settings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveImageAttachmentSettings), new object[1]
      {
        (object) settings
      });
      try
      {
        ImageAttachmentSettingsStore.SaveImageAttachmentSettings(settings);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual WebCenterSettings GetWebCenterSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetWebCenterSettings), Array.Empty<object>());
      try
      {
        return WebCenterSettingsStore.GetWebCenterSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (WebCenterSettings) null;
      }
    }

    public virtual void SaveWebCenterSettings(WebCenterSettings settings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveWebCenterSettings), new object[1]
      {
        (object) settings
      });
      try
      {
        WebCenterSettingsStore.SaveWebCenterSettings(settings);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetFormConfigFile(FormConfigFile fileType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFormConfigFile), new object[1]
      {
        (object) fileType
      });
      try
      {
        return FormsConfiguration.GetFormConfigurationFile(fileType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual FileSystemEntry[] GetHelocTableDirEntries()
    {
      return this.GetHelocTableDirEntries(false);
    }

    public virtual FileSystemEntry[] GetHelocTableDirEntries(bool useNewHELOCHistoricTable)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHelocTableDirEntries), Array.Empty<object>());
      try
      {
        string str = useNewHELOCHistoricTable ? "" : Path.Combine(this.Session.Context.Settings.AppDataDir, "HelocTables");
        if (!useNewHELOCHistoricTable && !Directory.Exists(str))
          Directory.CreateDirectory(str);
        return SystemConfigurationAccessor.GetHelocTableList(str, useNewHELOCHistoricTable);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual BinaryObject GetHelocTable(string name)
    {
      return this.GetHelocTable(name, true) ?? this.GetHelocTable(name, false);
    }

    public virtual BinaryObject GetHelocTable(string name, bool useNewHELOCHistoricTable)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHelocTable), new object[2]
      {
        (object) name,
        (object) useNewHELOCHistoricTable
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("HELOC Table name cannot be blank or null", nameof (name), this.Session.SessionInfo));
      try
      {
        BinaryObject helocTableDetails = SystemConfigurationAccessor.GetHelocTableDetails(name, useNewHELOCHistoricTable);
        name = FileSystem.EncodeFilename(name, true);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved HELOC Table \"" + name + "\""));
        return BinaryObject.Marshal(helocTableDetails);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual bool SaveHelocTable(string name, BinaryObject data)
    {
      return this.SaveHelocTable(name, data, false);
    }

    public virtual bool SaveHelocTable(
      string name,
      BinaryObject data,
      bool useNewHELOCHistoricTable)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveHelocTable), new object[2]
      {
        (object) name,
        (object) data
      });
      if ((name ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("HELOC Table name cannot be null", nameof (name), this.Session.SessionInfo));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("HELOC Table data object cannot be null", nameof (data), this.Session.SessionInfo));
      if (!useNewHELOCHistoricTable)
        data.Download();
      try
      {
        int num = SystemConfigurationAccessor.SaveHelocTable(name, data, useNewHELOCHistoricTable) ? 1 : 0;
        data.DisposeDeserialized();
        return num != 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool HelocTableObjectExists(string name)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (HelocTableObjectExists), new object[1]
      {
        (object) name
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("HELOC Table name cannot be null", nameof (name), this.Session.SessionInfo));
      try
      {
        return SystemConfigurationAccessor.ExistsHelocTable(name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteHelocTable(string name) => this.DeleteHelocTable(name, false);

    public virtual bool DeleteHelocTable(string name, bool useNewHELOCHistoricTable)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteHelocTable), new object[2]
      {
        (object) name,
        (object) useNewHELOCHistoricTable
      });
      if ((name ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("HELOC Table name cannot be null", nameof (name), this.Session.SessionInfo));
      try
      {
        return SystemConfigurationAccessor.DeleteHelocTable(name, useNewHELOCHistoricTable);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual DateTime GetOutputFormFileLastWriteTime(string fileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOutputFormFileLastWriteTime), new object[1]
      {
        (object) fileName
      });
      try
      {
        return File.GetLastWriteTime(Path.Combine(this.Session.Context.Settings.ApplicationDir, "PdfForms\\" + fileName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual BinaryObject GetOutputFormFile(string fileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOutputFormFile), new object[1]
      {
        (object) fileName
      });
      try
      {
        using (DataFile latestVersion = FileStore.GetLatestVersion(Path.Combine(this.Session.Context.Settings.ApplicationDir, "PdfForms\\" + fileName)))
          return !latestVersion.Exists ? (BinaryObject) null : BinaryObject.Marshal(latestVersion.GetData());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetCustomLetter(CustomLetterType type, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomLetter), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        using (DataFile latestVersion = CustomLetterStore.GetLatestVersion(type, entry))
        {
          if (!latestVersion.Exists)
            return (BinaryObject) null;
          TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved custom letter \"" + (object) entry + "\""));
          return CustomLetterStore.GetCustomLetterData(latestVersion, entry);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveCustomLetter(
      CustomLetterType type,
      FileSystemEntry entry,
      BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveCustomLetter), new object[3]
      {
        (object) type,
        (object) entry,
        (object) data
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Custom letter data object cannot be null", nameof (data), this.Session.SessionInfo));
      data.Download();
      try
      {
        using (DataFile dataFile = CustomLetterStore.CheckOut(type, entry))
        {
          string md5CheckSum = FileStore.GetMD5CheckSum(data);
          string filePath = entry.ToString();
          if (dataFile.Exists)
          {
            dataFile.CheckIn(data);
            if (md5CheckSum != null)
              FileInfoDbAccessor.UpdateFileInfo(data.Length, md5CheckSum, filePath, 7);
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
          }
          else
          {
            dataFile.CreateNew(data);
            if (md5CheckSum != null)
              FileInfoDbAccessor.InsertFileInfo(data.Length, md5CheckSum, filePath, 7);
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
          }
        }
        data.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom letter \"" + (object) entry + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool CustomLetterObjectExists(CustomLetterType type, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CustomLetterObjectExists), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return CustomLetterStore.Exists(type, entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CustomLetterObjectExistsOfAnyType(
      CustomLetterType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), "CustomLetterObjectExists", new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return CustomLetterStore.ExistsOfAnyType(type, entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void CreateCustomLetterFolder(CustomLetterType type, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), "CreateCustomFormFolder", new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        CustomLetterStore.CreateFolder(type, entry);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom letter \"" + (object) entry + "\" deleted"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void MoveCustomLetterObject(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveCustomLetterObject), new object[3]
      {
        (object) type,
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        CustomLetterStore.Move(type, source, target);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom letter \"" + (object) source + "\" moved to " + (object) target));
        if (source.ParentFolder.Path == target.ParentFolder.Path)
        {
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified, DateTime.Now, source.Name, source.Path));
        }
        else
        {
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, source.Name, source.Path));
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void NewCustomLetter(
      CustomLetterType type,
      FileSystemEntry entry,
      BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (NewCustomLetter), new object[3]
      {
        (object) type,
        (object) entry,
        (object) data
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("File system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      data.Download();
      try
      {
        CustomLetterStore.CreateNew(type, entry, data);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Custom letter \"" + (object) entry + "\" created"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
        data.DisposeDeserialized();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CopyCustomLetterObject(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CopyCustomLetterObject), new object[3]
      {
        (object) type,
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        CustomLetterStore.Copy(type, source, target);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteCustomLetterObject(CustomLetterType type, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteCustomLetterObject), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Custom letter object cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        CustomLetterStore.Delete(type, entry);
        FileInfoDbAccessor.DeleteFileInfo(entry.ToString(), 7);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetCustomLettersRecursively(
      CustomLetterType type,
      FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomLettersRecursively), new object[2]
      {
        (object) type,
        (object) parentFolder
      });
      try
      {
        return CustomLetterStore.GetCustomLettersRecursively(type, parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetAllCustomLetterFileEntries(
      CustomLetterType contactType,
      string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllCustomLetterFileEntries), new object[2]
      {
        (object) contactType,
        (object) userId
      });
      try
      {
        return CustomLetterStore.GetAllFileEntries(contactType, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetCustomLetterDirEntries(
      CustomLetterType contactType,
      FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomLetterDirEntries), new object[2]
      {
        (object) contactType,
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return CustomLetterStore.GetDirectoryEntries(contactType, parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredCustomLetterDirEntries(
      CustomLetterType contactType,
      FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFilteredCustomLetterDirEntries), new object[2]
      {
        (object) contactType,
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return CustomLetterStore.GetDirectoryEntries(this.Session.GetUserInfo(), contactType, parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual CustomFormDetail[] GetCustomFormDetails()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomFormDetails), Array.Empty<object>());
      try
      {
        return CustomFormDetailsAccessor.GetCustomFormDetails();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new CustomFormDetail[0];
      }
    }

    public virtual void SaveCustomFormDetail(CustomFormDetail formDetail)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveCustomFormDetail), new object[1]
      {
        (object) formDetail
      });
      try
      {
        CustomFormDetailsAccessor.SaveCustomFormDetail(formDetail);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RenameCustomFormDetailSource(string currentSource, string newSource)
    {
      this.onApiCalled(nameof (ConfigurationManager), "RenameCustomFormSource", new object[1]
      {
        (object) currentSource
      });
      try
      {
        CustomFormDetailsAccessor.RenameSource(currentSource, newSource);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CopyCustomFormDetail(string currentSource, string targetSource)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CopyCustomFormDetail), new object[1]
      {
        (object) currentSource
      });
      try
      {
        CustomFormDetailsAccessor.CopyCustomFormDetail(currentSource, targetSource);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteCustomFormDetail(string source)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteCustomFormDetail), new object[1]
      {
        (object) source
      });
      try
      {
        CustomFormDetailsAccessor.DeleteCustomFormDetail(source);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetTemplateDirEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateDirEntries), new object[2]
      {
        (object) type,
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Parent folder entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return TemplateSettingsStore.GetDirectoryEntries(type, parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredTemplateDirEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFilteredTemplateDirEntries), new object[2]
      {
        (object) type,
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Parent folder entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        FileSystemEntry[] templateDirEntries;
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            templateDirEntries = SystemConfigurationAccessor.GetDocumentStackingTemplatesList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.PurchaseAdvice:
            templateDirEntries = SystemConfigurationAccessor.GetPurchaseAdviceTemplatesList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.FundingTemplate:
            templateDirEntries = SystemConfigurationAccessor.GetFundingTemplateList(true);
            break;
          case TemplateSettingsType.DashboardTemplate:
            templateDirEntries = DashboardAccessor.GetDashboardSnapshotTemplateList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.DashboardViewTemplate:
            templateDirEntries = DashboardAccessor.GetDashboardViewTemplateList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.LoanDuplicationTemplate:
            templateDirEntries = SystemConfigurationAccessor.GetLoanDuplicationTemplatesList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.DocumentExportTemplate:
            templateDirEntries = SystemConfigurationAccessor.GetDocumentExportTemplatesList(parentFolder, this.Session);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            templateDirEntries = SystemConfigurationAccessor.GetAffiliatedBusinessArrangementsTemplatesList(parentFolder, this.Session);
            break;
          default:
            templateDirEntries = TemplateSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), type, parentFolder, FileSystemEntry.Types.All, false, true);
            break;
        }
        return templateDirEntries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table1,
      string table2,
      string table1PrimaryKeyColumn,
      string table2ForeignKeyColumn,
      string whereClausColumn,
      string[] templates)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomFieldsByTemplates), (object[]) templates);
      try
      {
        return SystemConfiguration.GetCustomFieldsByTemplates(customFieldColumn, table1, table2, table1PrimaryKeyColumn, table2ForeignKeyColumn, whereClausColumn, templates);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table,
      string whereClausColumn,
      string[] templates)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomFieldsByTemplates), (object[]) templates);
      try
      {
        return SystemConfiguration.GetCustomFieldsByTemplates(customFieldColumn, table, whereClausColumn, templates);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredTemplateFileEntries(
      TemplateSettingsType type,
      bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllFilteredTemplateDirEntries", new object[2]
      {
        (object) type,
        (object) includeProperties
      });
      FileSystemEntry parentEntry = new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null);
      try
      {
        return TemplateSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), type, parentEntry, FileSystemEntry.Types.File, true, includeProperties);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredTemplateFileEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      bool recurse,
      bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFilteredTemplateFileEntries), new object[4]
      {
        (object) type,
        (object) parentFolder,
        (object) recurse,
        (object) includeProperties
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Parent folder entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return TemplateSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), type, parentFolder, FileSystemEntry.Types.File, recurse, includeProperties);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry GetTemplatePropertiesAndRights(
      TemplateSettingsType type,
      FileSystemEntry fsEntry,
      bool includeProperties,
      bool includeRights)
    {
      FileSystemEntry[] propertiesAndRights = this.GetTemplatePropertiesAndRights(type, new FileSystemEntry[1]
      {
        fsEntry
      }, (includeProperties ? 1 : 0) != 0, (includeRights ? 1 : 0) != 0);
      return propertiesAndRights.Length != 0 ? propertiesAndRights[0] : (FileSystemEntry) null;
    }

    public virtual FileSystemEntry[] GetTemplatePropertiesAndRights(
      TemplateSettingsType type,
      FileSystemEntry[] fsEntries,
      bool includeProperties,
      bool includeRights)
    {
      this.onApiCalled(nameof (ConfigurationManager), "ResolveTemplateFileSystemEntries", new object[4]
      {
        (object) type,
        (object) fsEntries,
        (object) includeProperties,
        (object) includeRights
      });
      if (fsEntries == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("FileSystem entry list cannot be null", "parentFolder", this.Session.SessionInfo));
      try
      {
        return TemplateSettingsStore.GetTemplatePropertiesAndRights(this.Session.GetUserInfo(), type, fsEntries, includeProperties, includeRights);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      this.CopyTemplateSettingsObject(type, source, target, true);
    }

    public virtual void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      bool forNewRESPA)
    {
      this.CopyTemplateSettingsObject(type, source, target, forNewRESPA ? RespaVersions.Respa2015 : RespaVersions.Respa2010);
    }

    public virtual void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CopyTemplateSettingsObject), new object[4]
      {
        (object) type,
        (object) source,
        (object) target,
        (object) respaVersion
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Source cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        TemplateSettingsStore.Copy(type, source, target, respaVersion);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("The template \"" + (object) source + "\" moved to " + (object) target));
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            SystemConfigurationAccessor.DuplicateDocumentStackingTemplate(source.Name, target.Name, target.ToString());
            break;
          case TemplateSettingsType.PurchaseAdvice:
            SystemConfigurationAccessor.DuplicatePurchaseAdviceTemplate(source.Name, target.Name);
            break;
          case TemplateSettingsType.FundingTemplate:
            SystemConfigurationAccessor.DuplicateFundingTemplate(source.Name, target.Name);
            break;
          case TemplateSettingsType.DashboardTemplate:
            DashboardAccessor.DuplicateDashboardSnapshotTemplate(target, this.Session);
            break;
          case TemplateSettingsType.LoanDuplicationTemplate:
            SystemConfigurationAccessor.DuplicateLoanDuplicationTemplate(source.Name, target.Name);
            break;
          case TemplateSettingsType.DocumentExportTemplate:
            SystemConfigurationAccessor.DuplicateDocumentExportTemplate(source.Name, target.Name);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            SystemConfigurationAccessor.DuplicateAffiliatedBusinessArrangementsTemplate(source, target);
            break;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void MoveTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveTemplateSettingsObject), new object[3]
      {
        (object) type,
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException(type.ToString() + " template folder/file cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        TemplateSettingsStore.Move(type, source, target);
        switch (type)
        {
          case TemplateSettingsType.DashboardTemplate:
            DashboardAccessor.RenameDashboardSnapshotTemplate(source, target);
            break;
          case TemplateSettingsType.DashboardViewTemplate:
            DashboardAccessor.RenameDashboardViewTemplate(source, target);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            SystemConfigurationAccessor.RenameAffiliatedBusinessArrangementsTemplate(source, target);
            break;
        }
        if (source.ParentFolder.Path == target.ParentFolder.Path)
        {
          switch (type)
          {
            case TemplateSettingsType.StackingOrder:
              SystemConfigurationAccessor.RenameDocumentStackingTemplate(source.Name, target.Name, target.ToString());
              break;
            case TemplateSettingsType.PurchaseAdvice:
              SystemConfigurationAccessor.RenamePurchaseAdviceTemplate(source.Name, target.Name);
              break;
            case TemplateSettingsType.FundingTemplate:
              SystemConfigurationAccessor.RenameFundingTemplate(source.Name, target.Name);
              break;
            case TemplateSettingsType.LoanDuplicationTemplate:
              SystemConfigurationAccessor.RenameLoanDuplicationTemplate(source.Name, target.Name);
              break;
            case TemplateSettingsType.DocumentExportTemplate:
              SystemConfigurationAccessor.RenameDocumentExportTemplate(source.Name, target.Name);
              break;
          }
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified, DateTime.Now, source.Name, source.Path));
        }
        else
        {
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, source.Name, source.Path));
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
        }
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Template \"" + (object) source + "\" moved to " + (object) target));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetAllTemplateSettingsFileEntries(
      TemplateSettingsType type,
      string userID)
    {
      return this.GetAllTemplateSettingsFileEntries(type, userID, true);
    }

    public virtual FileSystemEntry[] GetAllTemplateSettingsFileEntries(
      TemplateSettingsType type,
      string userID,
      bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllTemplateSettingsFileEntries), new object[3]
      {
        (object) type,
        (object) userID,
        (object) includeProperties
      });
      try
      {
        return TemplateSettingsStore.GetAllFileEntries(type, userID, includeProperties);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetAllPublicTemplateSettingsFileEntries(
      TemplateSettingsType type,
      bool includeProperties)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllPublicTemplateSettingsFileEntries), new object[2]
      {
        (object) type,
        (object) includeProperties
      });
      try
      {
        FileSystemEntry[] settingsFileEntries;
        switch (type)
        {
          case TemplateSettingsType.TradePriceAdjustment:
            settingsFileEntries = SystemConfigurationAccessor.GetAdjustmentTemplateList(includeProperties);
            break;
          case TemplateSettingsType.SRPTable:
            settingsFileEntries = SystemConfigurationAccessor.GetSRPTemplateList(includeProperties);
            break;
          case TemplateSettingsType.Investor:
            settingsFileEntries = SystemConfigurationAccessor.GetAllPublicFileEntriesForInvestorTemplates(includeProperties);
            break;
          case TemplateSettingsType.FundingTemplate:
            settingsFileEntries = SystemConfigurationAccessor.GetFundingTemplateList(includeProperties);
            break;
          default:
            settingsFileEntries = TemplateSettingsStore.GetAllPublicFileEntries(type, includeProperties);
            break;
        }
        return settingsFileEntries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] FindTemplateFileEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string propertyName,
      object propertyValue,
      bool recurse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (FindTemplateFileEntries), new object[5]
      {
        (object) type,
        (object) parentFolder,
        (object) propertyName,
        propertyValue,
        (object) recurse
      });
      try
      {
        return TemplateSettingsStore.FindFileEntries(type, parentFolder, propertyName, propertyValue, recurse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry FindTemplateByGuid(TemplateSettingsType type, string guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (FindTemplateByGuid), new object[2]
      {
        (object) type,
        (object) guid
      });
      try
      {
        return TemplateSettingsStore.FindEntryByGuid(type, FileSystemEntry.PublicRoot, guid, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry) null;
      }
    }

    public virtual FileSystemEntry FindTemplateByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string guid,
      bool recurse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (FindTemplateByGuid), new object[4]
      {
        (object) type,
        (object) parentFolder,
        (object) guid,
        (object) recurse
      });
      try
      {
        return TemplateSettingsStore.FindEntryByGuid(type, parentFolder, guid, recurse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry) null;
      }
    }

    public virtual BinaryObject GetTemplateSettings(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateSettings), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        BinaryObject o = (BinaryObject) null;
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            o = SystemConfigurationAccessor.GetDocumentStackingTemplateSettings(entry);
            break;
          case TemplateSettingsType.TradePriceAdjustment:
            o = SystemConfigurationAccessor.GetAdjustmentTemplateSettings(entry);
            break;
          case TemplateSettingsType.SRPTable:
            o = SystemConfigurationAccessor.GetSRPTemplateSettings(entry);
            break;
          case TemplateSettingsType.Investor:
            o = SystemConfigurationAccessor.GetInvestorTemplateSettings(entry);
            break;
          case TemplateSettingsType.PurchaseAdvice:
            o = SystemConfigurationAccessor.GetPurchaseAdviceTemplateSettings(entry);
            break;
          case TemplateSettingsType.FundingTemplate:
            o = SystemConfigurationAccessor.GetFundingTemplateSettings(entry);
            break;
          case TemplateSettingsType.DashboardTemplate:
            o = DashboardAccessor.GetDashboardSnapshotTemplateSettings(entry);
            break;
          case TemplateSettingsType.LoanDuplicationTemplate:
            o = SystemConfigurationAccessor.GetLoanDuplicationTemplateSettings(entry);
            break;
          case TemplateSettingsType.DocumentExportTemplate:
            o = SystemConfigurationAccessor.GetDocumentExportTemplateSettings(entry);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            o = SystemConfigurationAccessor.GetAffiliatedBusinessArrangementsTemplateSettings(entry);
            break;
          default:
            using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entry))
            {
              if (!latestVersion.Exists)
                return (BinaryObject) null;
              o = latestVersion.Data;
              break;
            }
        }
        return BinaryObject.Marshal(o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual Dictionary<FileSystemEntry, BinaryObject> GetTemplateSettings(
      TemplateSettingsType type,
      FileSystemEntry[] entryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateSettings), new object[2]
      {
        (object) type,
        (object) entryList
      });
      if (entryList == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entryList), this.Session.SessionInfo));
      try
      {
        Dictionary<FileSystemEntry, BinaryObject> templateSettings = new Dictionary<FileSystemEntry, BinaryObject>();
        foreach (FileSystemEntry entry in entryList)
        {
          using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entry))
            templateSettings[entry] = latestVersion.Exists ? BinaryObject.Marshal(latestVersion.Data) : (BinaryObject) null;
        }
        return templateSettings;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new Dictionary<FileSystemEntry, BinaryObject>();
      }
    }

    public virtual BinaryObject GetTemplateSettings(
      TemplateSettingsType templateSettingsType,
      int templateID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateSettings), new object[2]
      {
        (object) templateSettingsType,
        (object) templateID
      });
      if (templateID <= 0)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified template ID cannot be less than or eqaul to zero", nameof (templateID), this.Session.SessionInfo));
      try
      {
        BinaryObject o = (BinaryObject) null;
        if (templateSettingsType == TemplateSettingsType.StackingOrder)
          o = SystemConfigurationAccessor.GetDocumentStackingTemplateSettingsByID(templateID);
        return BinaryObject.Marshal(o);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetTemplateByGuid(TemplateSettingsType type, string guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateByGuid), new object[2]
      {
        (object) type,
        (object) guid
      });
      try
      {
        return BinaryObject.Marshal(TemplateSettingsStore.GetTemplateByGuid(type, FileSystemEntry.PublicRoot, guid, true));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual BinaryObject GetTemplateByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string guid,
      bool recurse)
    {
      this.onApiCalled(nameof (ConfigurationManager), "FindTemplateByGuid", new object[4]
      {
        (object) type,
        (object) parentFolder,
        (object) guid,
        (object) recurse
      });
      try
      {
        return BinaryObject.Marshal(TemplateSettingsStore.GetTemplateByGuid(type, parentFolder, guid, recurse));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual Hashtable GetLoanTemplateComponents(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanTemplateComponents), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return TemplateSettingsStore.GetLoanTemplateComponents(entry, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SaveTemplateSettings(
      TemplateSettingsType type,
      FileSystemEntry entry,
      BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveTemplateSettings), new object[3]
      {
        (object) type,
        (object) entry,
        (object) data
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      data.Download();
      try
      {
        bool isUpdate = true;
        using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(type, entry))
        {
          if (!templateSettings.Exists)
            isUpdate = false;
          templateSettings.CheckIn(data);
        }
        if (isUpdate)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        else
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            SystemConfigurationAccessor.SaveDocumentStackingTemplateSettings(entry.ToString(), data, isUpdate);
            break;
          case TemplateSettingsType.UnderwritingConditionSet:
            ConditionConfiguration.ResetCacheForConditionTrackingSetup(ConditionType.Underwriting);
            break;
          case TemplateSettingsType.PostClosingConditionSet:
            ConditionConfiguration.ResetCacheForConditionTrackingSetup(ConditionType.PostClosing);
            break;
          case TemplateSettingsType.TradePriceAdjustment:
            SystemConfigurationAccessor.SaveAdjustmentTemplateSettings(entry, data);
            break;
          case TemplateSettingsType.SRPTable:
            SystemConfigurationAccessor.SaveSRPTemplateSettings(entry, data);
            break;
          case TemplateSettingsType.Investor:
            SystemConfigurationAccessor.SaveInvestorTemplatesSettings(data);
            break;
          case TemplateSettingsType.PurchaseAdvice:
            SystemConfigurationAccessor.SavePurchaseAdviceTemplateSettings(data);
            break;
          case TemplateSettingsType.FundingTemplate:
            SystemConfigurationAccessor.SaveFundingTemplateSettings(data);
            break;
          case TemplateSettingsType.DashboardTemplate:
            DashboardAccessor.SaveDashboardSnapshotTemplateSettings(entry, data, isUpdate);
            break;
          case TemplateSettingsType.PipelineView:
            ClientContext.GetCurrent().Cache.Remove(string.Format("{0}_{1}", (object) "UserCustomPipeline", (object) this.Session.UserID));
            break;
          case TemplateSettingsType.LoanDuplicationTemplate:
            SystemConfigurationAccessor.SaveLoanDuplicationTemplateSettings(data);
            break;
          case TemplateSettingsType.DocumentExportTemplate:
            SystemConfigurationAccessor.SaveDocumentExportTemplateSettings(data, isUpdate);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            SystemConfigurationAccessor.SaveAffiliatedBusinessArrangementsTemplatesSettings(entry, data, isUpdate);
            break;
        }
      }
      catch (PathTooLongException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool TemplateSettingsObjectExists(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (TemplateSettingsObjectExists), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            return SystemConfigurationAccessor.ExistsDocumentStackingTemplateSettings(entry, false);
          case TemplateSettingsType.TradePriceAdjustment:
            return SystemConfigurationAccessor.ExistsAdjustmentTemplateSettings(entry);
          case TemplateSettingsType.SRPTable:
            return SystemConfigurationAccessor.ExistsSRPTemplateSettings(entry);
          case TemplateSettingsType.Investor:
            return SystemConfigurationAccessor.ExistsInvestorTemplateSettings(entry, false);
          case TemplateSettingsType.PurchaseAdvice:
            return SystemConfigurationAccessor.ExistsPurchaseAdviceTemplateSettings(entry, false);
          case TemplateSettingsType.FundingTemplate:
            return SystemConfigurationAccessor.ExistsFundingTemplateSettings(entry);
          case TemplateSettingsType.DashboardTemplate:
            return DashboardAccessor.ExistsDashboardSnapshotTemplateSettings(entry, false);
          case TemplateSettingsType.LoanDuplicationTemplate:
            return SystemConfigurationAccessor.ExistsLoanDuplicationTemplateSettings(entry, false);
          case TemplateSettingsType.DocumentExportTemplate:
            return SystemConfigurationAccessor.ExistsDocumentExportTemplateSettings(entry, false);
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            return SystemConfigurationAccessor.ExistsAffiliatedBusinessArrangementsTemplateSettings(entry, false);
          default:
            return TemplateSettingsStore.Exists(type, entry);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool TemplateSettingsObjectExistsOfAnyType(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (TemplateSettingsObjectExistsOfAnyType), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        switch (type)
        {
          case TemplateSettingsType.StackingOrder:
            return SystemConfigurationAccessor.ExistsDocumentStackingTemplateSettings(entry, true);
          case TemplateSettingsType.PurchaseAdvice:
            return SystemConfigurationAccessor.ExistsPurchaseAdviceTemplateSettings(entry, true);
          case TemplateSettingsType.FundingTemplate:
            return SystemConfigurationAccessor.ExistsFundingTemplateSettings(entry);
          case TemplateSettingsType.DashboardTemplate:
            return DashboardAccessor.ExistsDashboardSnapshotTemplateSettings(entry, true);
          case TemplateSettingsType.LoanDuplicationTemplate:
            return SystemConfigurationAccessor.ExistsLoanDuplicationTemplateSettings(entry, true);
          case TemplateSettingsType.DocumentExportTemplate:
            return SystemConfigurationAccessor.ExistsDocumentExportTemplateSettings(entry, true);
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            return SystemConfigurationAccessor.ExistsAffiliatedBusinessArrangementsTemplateSettings(entry, true);
          default:
            return TemplateSettingsStore.ExistsOfAnyType(type, entry);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void DeleteTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteTemplateSettingsObject), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        if (type == TemplateSettingsType.StackingOrder)
        {
          StackingOrderSetTemplate templateSettings1 = (StackingOrderSetTemplate) this.GetTemplateSettings(type, entry);
          if (templateSettings1 != null)
          {
            FileSystemEntry[] exportTemplatesList = SystemConfigurationAccessor.GetDocumentExportTemplatesList(new FileSystemEntry("\\", "", FileSystemEntry.Types.File, (string) null), this.Session);
            if (exportTemplatesList != null && exportTemplatesList.Length != 0)
            {
              foreach (FileSystemEntry entry1 in exportTemplatesList)
              {
                DocumentExportTemplate templateSettings2 = (DocumentExportTemplate) this.GetTemplateSettings(TemplateSettingsType.DocumentExportTemplate, entry1);
                if (templateSettings2 != null && templateSettings2.DocumentStackingTemplateID.Equals(templateSettings1.DocumentStackingTemplateID))
                {
                  templateSettings2.StackingOrderName = (string) null;
                  templateSettings2.DocumentStackingTemplateID = 0;
                  this.SaveTemplateSettings(TemplateSettingsType.DocumentExportTemplate, entry1, new BinaryObject((IXmlSerializable) templateSettings2));
                }
              }
            }
          }
        }
        TemplateSettingsStore.Delete(type, entry);
        switch (type - 7)
        {
          case TemplateSettingsType.CustomLetter:
            SystemConfigurationAccessor.DeleteDocumentStackingTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.LoanProgram:
          case TemplateSettingsType.ClosingCost:
          case TemplateSettingsType.MiscData:
          case TemplateSettingsType.LoanTemplate:
          case TemplateSettingsType.SRPTable:
          case TemplateSettingsType.TradeFilter:
          case TemplateSettingsType.Investor:
          case TemplateSettingsType.PurchaseAdvice:
label_25:
            TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("The template \"" + (object) entry + "\" deleted"));
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, entry.Name, entry.Path));
            break;
          case TemplateSettingsType.FormList:
            SystemConfigurationAccessor.DeleteAdjustmentTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.DocumentSet:
            SystemConfigurationAccessor.DeleteSRPTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.StackingOrder:
            SystemConfigurationAccessor.DeleteInvestorTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.UnderwritingConditionSet:
            SystemConfigurationAccessor.DeletePurchaseAdviceTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.PostClosingConditionSet:
            SystemConfigurationAccessor.DeleteFundingTemplate(entry.Name);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.Campaign:
            DashboardAccessor.DeleteDashboardSnapshotTemplate(entry);
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.TradePriceAdjustment:
            ClientContext.GetCurrent().Cache.Remove(string.Format("{0}_{1}", (object) "UserCustomPipeline", (object) this.Session.UserID));
            goto case TemplateSettingsType.LoanProgram;
          case TemplateSettingsType.FundingTemplate:
            DashboardAccessor.DeleteDashboardViewTemplate(entry);
            goto case TemplateSettingsType.LoanProgram;
          default:
            switch (type - 36)
            {
              case TemplateSettingsType.CustomLetter:
                SystemConfigurationAccessor.DeleteLoanDuplicationTemplate(entry.Name);
                goto label_25;
              case TemplateSettingsType.LoanProgram:
                SystemConfigurationAccessor.DeleteDocumentExportTemplate(entry.Name);
                goto label_25;
              case TemplateSettingsType.MiscData:
                SystemConfigurationAccessor.DeleteAffiliatedBusinessArrangementsTemplate(entry);
                goto label_25;
              default:
                goto label_25;
            }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CreateTemplateSettingsFolder(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateTemplateSettingsFolder), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        TemplateSettingsStore.CreateFolder(type, entry);
        switch (type)
        {
          case TemplateSettingsType.DashboardTemplate:
            DashboardAccessor.SaveDashboardSnapshotTemplateSettings(entry, (BinaryObject) null, false);
            break;
          case TemplateSettingsType.DashboardViewTemplate:
            DashboardAccessor.SaveDashboardViewTemplateFolder(entry);
            break;
          case TemplateSettingsType.AffiliatedBusinessArrangements:
            SystemConfigurationAccessor.SaveAffiliatedBusinessArrangementsTemplatesSettings(entry, (BinaryObject) null, false);
            break;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetTemplateSettingsXRefs(
      TemplateSettingsType type,
      FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTemplateSettingsXRefs), new object[2]
      {
        (object) type,
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entry))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ObjectNotFoundException("Template not found", ObjectType.Template, (object) entry));
          return latestVersion.GetTemplateXRefs();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFormGroupDirEntries(FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFormGroupDirEntries), new object[1]
      {
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return FormGroups.GetDirectoryEntries(parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredFormGroupDirEntries(FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFilteredFormGroupDirEntries), new object[1]
      {
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return FormGroups.GetDirectoryEntries(this.Session.GetUserInfo(), parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FormInfo[] GetForms(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetForms), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        using (FormGroup formGroup = FormGroups.CheckOut(entry))
          return !formGroup.Exists ? (FormInfo[]) null : formGroup.Forms;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FormInfo[]) null;
      }
    }

    public virtual void SaveForms(FileSystemEntry entry, FormInfo[] forms)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveForms), new object[2]
      {
        (object) entry,
        (object) forms
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      if (forms == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Form group forms cannot be null", nameof (forms), this.Session.SessionInfo));
      try
      {
        FormGroups.SaveForms(entry, forms);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form Group \"" + (object) entry + "\" updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool FormGroupObjectExists(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (FormGroupObjectExists), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return FormGroups.Exists(entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool FormGroupObjectExistsOfAnyType(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (FormGroupObjectExistsOfAnyType), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return FormGroups.ExistsOfAnyType(entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void CreateFormGroupFolder(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateFormGroupFolder), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        FormGroups.CreateFolder(entry);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form group \"" + (object) entry + "\" created"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void MoveFormGroupObject(FileSystemEntry source, FileSystemEntry target)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveFormGroupObject), new object[2]
      {
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Source object cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target object cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        FormGroups.Move(source, target);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form group \"" + (object) source + "\" moved to " + (object) target));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, source.Name, source.Path));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void NewFormGroup(FileSystemEntry entry, FormInfo[] forms)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (NewFormGroup), new object[2]
      {
        (object) entry,
        (object) forms
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      if (forms == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Forms list cannot be null", nameof (forms), this.Session.SessionInfo));
      try
      {
        FormGroups.CreateNew(entry, forms);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form group \"" + (object) entry + "\" created"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CopyFormGroupObject(FileSystemEntry source, FileSystemEntry target)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CopyFormGroupObject), new object[2]
      {
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Source cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        FormGroups.Copy(source, target);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form group \"" + (object) source + "\" moved to " + (object) target));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, source.Name, source.Path));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteFormGroupObject(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteFormGroupObject), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        FormGroups.Delete(entry);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Form group \"" + (object) entry + "\" deleted"));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.TemplateDeleted, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetAllFormGroupFileEntries(string userID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllFormGroupFileEntries), new object[1]
      {
        (object) userID
      });
      try
      {
        return FormGroups.GetAllFileEntries(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFormGroupXRefs(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFormGroupXRefs), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        using (FormGroup formGroup = FormGroups.CheckOut(entry))
        {
          if (!formGroup.Exists)
            Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ObjectNotFoundException("Form Group not found", ObjectType.FormGroup, (object) entry));
          return formGroup.GetCustomLetterXRefs();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual string[] GetNotePhrases()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetNotePhrases), Array.Empty<object>());
      try
      {
        string[] allPhrases = NotePhrases.GetAllPhrases();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved all note phrases"));
        return allPhrases;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void AddNotePhrase(string phrase)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddNotePhrase), new object[1]
      {
        (object) phrase
      });
      if ((phrase ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Note phrase cannot be blank or null", nameof (phrase), this.Session.SessionInfo));
      try
      {
        NotePhrases.Add(phrase);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Added note phrase \"" + phrase + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddNotePhrases(string[] phrases)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddNotePhrases), (object[]) phrases);
      if (phrases == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Phrase list cannot be blank or null", nameof (phrases), this.Session.SessionInfo));
      try
      {
        NotePhrases.Add(phrases);
        for (int index = 0; index < phrases.Length; ++index)
          TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Added note phrase \"" + phrases[index] + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveNotePhrase(string phrase)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveNotePhrase), new object[1]
      {
        (object) phrase
      });
      if ((phrase ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Note phrase cannot be blank or null", nameof (phrase), this.Session.SessionInfo));
      try
      {
        NotePhrases.Remove(phrase);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Removed note phrase \"" + phrase + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveNotePhrases(string[] phrases)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveNotePhrases), (object[]) phrases);
      if (phrases == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Phrase list cannot be blank or null", nameof (phrases), this.Session.SessionInfo));
      try
      {
        NotePhrases.Remove(phrases);
        for (int index = 0; index < phrases.Length; ++index)
          TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Removed note phrase \"" + phrases[index] + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject GetPluginAssembly(string assemblyName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPluginAssembly), new object[1]
      {
        (object) assemblyName
      });
      if ((assemblyName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Assembly name cannot be blank or null", nameof (assemblyName), this.Session.SessionInfo));
      try
      {
        return BinaryObject.Marshal(Plugins.GetPluginAssembly(assemblyName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual Version GetPluginVersion(string assemblyName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPluginVersion), new object[1]
      {
        (object) assemblyName
      });
      if ((assemblyName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Assembly name cannot be blank or null", nameof (assemblyName), this.Session.SessionInfo));
      try
      {
        return Plugins.GetPluginVersion(assemblyName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Version) null;
      }
    }

    public virtual string[] GetPluginAssemblyNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPluginAssemblyNames), Array.Empty<object>());
      try
      {
        return Plugins.GetPluginAssemblyNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void InstallPlugin(string assemblyName, BinaryObject pluginAssembly)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InstallPlugin), new object[1]
      {
        (object) assemblyName
      });
      pluginAssembly.Download();
      try
      {
        this.Security.DemandSuperAdministrator();
        Plugins.InstallPlugin(assemblyName, pluginAssembly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UninstallPlugin(string assemblyName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UninstallPlugin), new object[1]
      {
        (object) assemblyName
      });
      try
      {
        this.Security.DemandSuperAdministrator();
        Plugins.UninstallPlugin(assemblyName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual HtmlEmailTemplate[] GetHtmlEmailTemplates(
      string ownerID,
      HtmlEmailTemplateType type)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHtmlEmailTemplates), new object[2]
      {
        (object) ownerID,
        (object) type
      });
      try
      {
        return SystemConfigurationAccessor.GetHtmlEmailTemplates(ownerID, type);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HtmlEmailTemplate[]) null;
      }
    }

    public virtual HtmlEmailTemplate GetHtmlEmailTemplate(string ownerID, string guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHtmlEmailTemplate), new object[2]
      {
        (object) ownerID,
        (object) guid
      });
      try
      {
        return SystemConfigurationAccessor.GetHtmlEmailTemplate(ownerID, guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HtmlEmailTemplate) null;
      }
    }

    public virtual void SaveHtmlEmailTemplate(HtmlEmailTemplate template)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveHtmlEmailTemplate), new object[1]
      {
        (object) template
      });
      try
      {
        SystemConfigurationAccessor.SaveTemplate(template);
        FieldSearchDbAccessor.UpdateFieldSearchInfo(new FieldSearchRule(template));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteHtmlEmailTemplate(HtmlEmailTemplate template)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteHtmlEmailTemplate), new object[1]
      {
        (object) template
      });
      try
      {
        SystemConfigurationAccessor.DeleteTemplate(template);
        FieldSearchDbAccessor.DeleteFieldSearchInfo(template.Guid, FieldSearchRuleType.HtmlEmailTemplate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool DataServicesOptOut()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DataServicesOptOut), Array.Empty<object>());
      try
      {
        return this.getSystemInfo().DataServicesOptOut;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateCompanyDataServicesOpt(string key)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateCompanyDataServicesOpt), new object[1]
      {
        (object) key
      });
      try
      {
        if (key.Length > 256)
          key = key.Substring(0, 256);
        EncompassSystemDbAccessor.UpdateCompanyDataServicesOpt(key);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateUserDataServicesOpt(string userID, string key)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateUserDataServicesOpt), new object[2]
      {
        (object) userID,
        (object) key
      });
      try
      {
        if (key.Length > 256)
          key = key.Substring(0, 256);
        using (User user = UserStore.CheckOut(userID))
        {
          user.UserInfo.DataServicesOptOutKey = key;
          user.CheckIn(this.Session.UserID, false);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    private EncompassSystemInfo getSystemInfo()
    {
      if (this.encompassSystemInfo == null)
        this.encompassSystemInfo = EncompassSystemDbAccessor.GetEncompassSystemInfo();
      return this.encompassSystemInfo;
    }

    public virtual Hashtable GetServicingMortgageStatements()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetServicingMortgageStatements), Array.Empty<object>());
      try
      {
        Hashtable mortgageStatements = SystemConfiguration.GetServicingMortgageStatements();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved Servicing Mortgage Statements information"));
        return mortgageStatements;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void UpdateServicingMortgageStatements(Hashtable formNames)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateServicingMortgageStatements), new object[1]
      {
        (object) formNames
      });
      try
      {
        SystemConfiguration.UpdateServicingMortgageStatements(formNames);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Servicing Mortgage Statement settings updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Hashtable GetServicingLateCharge(string stateAbbr)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetServicingLateCharge), new object[1]
      {
        (object) stateAbbr
      });
      try
      {
        Hashtable servicingLateCharge = SystemConfiguration.GetServicingLateCharge(stateAbbr);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved Servicing Late Charge minimum/maximum information"));
        return servicingLateCharge;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void UpdateServicingLateCharge(Hashtable info)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateServicingLateCharge), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Servicing Late Charge minimum/maximum cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        SystemConfiguration.UpdateServicingLateCharge(info);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Servicing Late Charge minimum/maximum settings updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual CountyLimit[] GetCountyLimits()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCountyLimits), Array.Empty<object>());
      try
      {
        return CountyLimitStore.GetCountyLimits();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (CountyLimit[]) null;
    }

    public virtual void UpdateCountyLimits(CountyLimit[] countyLimits)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateCountyLimits), Array.Empty<object>());
      try
      {
        CountyLimitStore.UpdateCountyLimits(countyLimits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ResetCountyLimits(CountyLimit[] countyLimits)
    {
      this.onApiCalled(nameof (ConfigurationManager), "AddAllCountyLimits", Array.Empty<object>());
      try
      {
        CountyLimitStore.ResetCountyLimits(countyLimits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int GetCountyLimit(string stateAbb, string countyName, int numOfUnits)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCountyLimit), new object[3]
      {
        (object) stateAbb,
        (object) countyName,
        (object) numOfUnits
      });
      try
      {
        return CountyLimitStore.GetCountyLimit(stateAbb, countyName, numOfUnits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return -999;
    }

    public virtual int GetCountyLimitByFips(string stateAbb, string fips, int numOfUnits)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCountyLimitByFips), new object[3]
      {
        (object) stateAbb,
        (object) fips,
        (object) numOfUnits
      });
      try
      {
        return CountyLimitStore.GetCountyLimitByFips(stateAbb, fips, numOfUnits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return -999;
    }

    public virtual ConventionalCountyLimit[] GetConventionalCountyLimits()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetConventionalCountyLimits), Array.Empty<object>());
      try
      {
        return ConventionalCountyLimitStore.GetConventionalCountyLimits();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ConventionalCountyLimit[]) null;
    }

    public virtual ConventionalCountyLimit[] GetConventionalCountyLimits(string state, int year)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetConventionalCountyLimits), Array.Empty<object>());
      try
      {
        return ConventionalCountyLimitStore.GetConventionalCountyLimits(state, year);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ConventionalCountyLimit[]) null;
    }

    public virtual ConventionalCountyLimit[] GetConventionalCountyLimits(string state)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetConventionalCountyLimits), Array.Empty<object>());
      try
      {
        return ConventionalCountyLimitStore.GetConventionalCountyLimits(state);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ConventionalCountyLimit[]) null;
    }

    public virtual void ResetConventionalCountyLimits(ConventionalCountyLimit[] countyLimits)
    {
      this.onApiCalled(nameof (ConfigurationManager), "AddAllConventionalCountyLimits", Array.Empty<object>());
      try
      {
        ConventionalCountyLimitStore.ResetConventionalCountyLimits(countyLimits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FedTresholdAdjustment[] GetFedTresholdAdjustments(int year)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetFedTresholdAdjustments(int year)", Array.Empty<object>());
      try
      {
        return FedTresholdAdjustmentStore.GetFedTresholdAdjustments(year);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (FedTresholdAdjustment[]) null;
    }

    public virtual FedTresholdAdjustment[] GetFedTresholdAdjustments()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetFedTresholdAdjustments()", Array.Empty<object>());
      try
      {
        return FedTresholdAdjustmentStore.GetFedTresholdAdjustments();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (FedTresholdAdjustment[]) null;
    }

    public virtual void ResetFedTresholdAdjustments(FedTresholdAdjustment[] fedAdjustments)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetFedTresholdAdjustments), Array.Empty<object>());
      try
      {
        FedTresholdAdjustmentStore.ResetFedTresholdAdjustments(fedAdjustments);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual AMILimit[] GetAMILimits(int year)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAMILmits(int year)", Array.Empty<object>());
      try
      {
        return AMILimitStore.GetAMILimits(year);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (AMILimit[]) null;
    }

    public virtual AMILimit[] GetAMILimits(string fipsCode)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAMILmits(string fipsCode)", Array.Empty<object>());
      try
      {
        return AMILimitStore.GetAMILimits(fipsCode);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (AMILimit[]) null;
    }

    public virtual AMILimit[] GetAMILimits()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAMILimits()", Array.Empty<object>());
      try
      {
        return AMILimitStore.GetAMILimits();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (AMILimit[]) null;
    }

    public virtual void ResetAMILimits(AMILimit[] limits)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetAMILimits), Array.Empty<object>());
      try
      {
        AMILimitStore.ResetAMILimits(limits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MFILimit[] GetMFILimits()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetMFILimits()", Array.Empty<object>());
      try
      {
        return MFILimitStore.GetMFILimits();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MFILimit[]) null;
    }

    public virtual void ResetMFILimits(MFILimit[] limits)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetMFILimits), Array.Empty<object>());
      try
      {
        MFILimitStore.ResetMFILimits(limits);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MFILimit[] GetMFILimits(string msaCode, string subjectState)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetMFILimits(string msaCode, string subjectState)", Array.Empty<object>());
      try
      {
        return MFILimitStore.GetMFILimits(msaCode, subjectState);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MFILimit[]) null;
    }

    public virtual int AddMIRecord(
      MIRecord miRecord,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddMIRecord), new object[4]
      {
        (object) miRecord,
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        return MIQueryStore.AddMIRecord(miRecord, miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return -1;
    }

    public virtual bool HasDuplicateMIRecord(
      int id,
      string scenarioKey,
      LoanTypeEnum miType,
      string tabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (HasDuplicateMIRecord), new object[4]
      {
        (object) id,
        (object) scenarioKey,
        (object) miType,
        (object) tabName
      });
      try
      {
        return MIQueryStore.HasDuplicateMIRecord(id, scenarioKey, miType, tabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual void UpdateMIOrder(
      List<int[]> ids,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMIOrder), new object[4]
      {
        (object) ids,
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        MIQueryStore.UpdateMIOrder(ids, miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteMIRecord(
      int id,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteMIRecord), new object[4]
      {
        (object) id,
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        MIQueryStore.DeleteMIRecord(id, miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MIRecord UpdateMIRecord(
      MIRecord miRecord,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMIRecord), new object[4]
      {
        (object) miRecord,
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        MIQueryStore.UpdateMIRecord(miRecord, miType, tabName, isForDownload);
        foreach (MIRecord miRecord1 in MIQueryStore.GetMIRecords(miType, tabName, isForDownload))
        {
          if (miRecord1.Id == miRecord.Id)
            return miRecord1;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MIRecord) null;
    }

    public virtual void UpdateMITable(
      MIRecord[] miRecords,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMITable), new object[4]
      {
        (object) miRecords,
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        MIQueryStore.UpdateMITable(miRecords, miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual MIRecord[] GetMIRecordsDetail(
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = ConfigurationManager.GetTableId(miType, isForDownload);
      if (string.IsNullOrEmpty(tableId))
        return (MIRecord[]) null;
      EllieMae.EMLite.DataAccess.DbQueryBuilder dbQueryBuilder = (EllieMae.EMLite.DataAccess.DbQueryBuilder) new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "Select queryTable.*, scenarioTable.* from " + tableId + " queryTable join " + tableId + "Scenarios scenarioTable on queryTable.id = scenarioTable.id";
      if (miType == LoanTypeEnum.Conventional && tabName != string.Empty)
        str = !string.Equals(tabName, "general", StringComparison.InvariantCultureIgnoreCase) ? str + " WHERE queryTable.TabName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) tabName) : str + " WHERE (queryTable.TabName = '' Or queryTable.TabName is NULL)";
      string text = str + " order by queryTable.[id], queryTable.[GroupOrder], scenarioTable.[id], scenarioTable.[ScenarioOrder] ASC";
      dbQueryBuilder.AppendLine(text);
      MIRecord miRecord = (MIRecord) null;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      try
      {
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        int num1 = -1;
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row = dataTable.Rows[index];
          int num2 = Utils.ParseInt((object) row["id"].ToString());
          if (num1 != num2)
          {
            if (miRecord != null)
            {
              miRecord.Scenarios = (FieldFilter[]) arrayList2.ToArray(typeof (FieldFilter));
              arrayList1.Add((object) miRecord);
            }
            miRecord = new MIRecord(row);
            arrayList2 = new ArrayList();
            num1 = num2;
          }
          arrayList2.Add((object) new FieldFilter(row));
        }
        if (arrayList2.Count > 0)
          miRecord.Scenarios = (FieldFilter[]) arrayList2.ToArray(typeof (FieldFilter));
        if (miRecord != null)
          arrayList1.Add((object) miRecord);
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot read records from MI table '" + tableId + "'.\r\n" + ex.Message);
      }
      return arrayList1.Count == 0 ? (MIRecord[]) null : (MIRecord[]) arrayList1.ToArray(typeof (MIRecord));
    }

    public virtual MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMIRecords), new object[2]
      {
        (object) miType,
        (object) tabName
      });
      try
      {
        return MIQueryStore.GetMIRecords(miType, tabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MIRecord[]) null;
    }

    public virtual MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName, bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMIRecords), new object[3]
      {
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        return MIQueryStore.GetMIRecords(miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MIRecord[]) null;
    }

    public virtual MIRecordXML ExportMIRecord(
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ExportMIRecord), new object[3]
      {
        (object) miType,
        (object) tabName,
        (object) isForDownload
      });
      try
      {
        return MIQueryStore.ExportMIRecord(miType, tabName, isForDownload);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (MIRecordXML) null;
    }

    public virtual void AddMITab(string tabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddMITab), new object[1]
      {
        (object) tabName
      });
      try
      {
        MIQueryStore.AddMITab(tabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteMITab(string tabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteMITab), new object[1]
      {
        (object) tabName
      });
      try
      {
        MIQueryStore.DeleteMITab(tabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateMITab(string oldTabName, string newTabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMITab), new object[2]
      {
        (object) oldTabName,
        (object) newTabName
      });
      try
      {
        MIQueryStore.UpdateMITab(oldTabName, newTabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool HadDuplicateMITab(string tabName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (HadDuplicateMITab), new object[1]
      {
        (object) tabName
      });
      try
      {
        return MIQueryStore.HadDuplicateMITab(tabName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual string[] GetMITabNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMITabNames), Array.Empty<object>());
      try
      {
        return MIQueryStore.GetMITabNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (string[]) null;
    }

    private static string GetTableId(LoanTypeEnum miType, bool isForDownload)
    {
      switch (miType)
      {
        case LoanTypeEnum.Conventional:
          return "MIConv";
        case LoanTypeEnum.USDA:
          return isForDownload ? "MIUSDADownload" : "MIUSDA";
        case LoanTypeEnum.FHA:
          return isForDownload ? "MIFHADownload" : "MIFHA";
        case LoanTypeEnum.VA:
          return isForDownload ? "MIVADownload" : "MIVA";
        case LoanTypeEnum.Other:
          return "MIOther";
        default:
          return (string) null;
      }
    }

    public virtual int AddTrusteeRecord(TrusteeRecord trusteeRecord)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddTrusteeRecord), new object[1]
      {
        (object) trusteeRecord
      });
      try
      {
        return TrusteeQueryStore.AddTrusteeRecord(trusteeRecord);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return -1;
    }

    public virtual bool HasDuplicateTrusteeRecord(int id, string contactName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (HasDuplicateTrusteeRecord), new object[2]
      {
        (object) id,
        (object) contactName
      });
      try
      {
        return TrusteeQueryStore.HasDuplicateTrusteeRecord(id, contactName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual void DeleteTrusteeRecord(int[] ids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteTrusteeRecord), new object[1]
      {
        (object) ids
      });
      try
      {
        TrusteeQueryStore.DeleteTrusteeRecord(ids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTrusteeRecord(TrusteeRecord trusteeRecord)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateTrusteeRecord), new object[1]
      {
        (object) trusteeRecord
      });
      try
      {
        TrusteeQueryStore.UpdateTrusteeRecord(trusteeRecord);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual TrusteeRecord[] GetTrusteeRecords()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTrusteeRecords), Array.Empty<object>());
      try
      {
        return TrusteeQueryStore.GetTrusteeRecords();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (TrusteeRecord[]) null;
    }

    public virtual FileSystemEntry[] GetUserAccessibleViews(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUserAccessibleViews), new object[1]
      {
        (object) userId
      });
      try
      {
        return TemplateSettingsStore.GetDirectoryEntries(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(userId));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual ServerTaskScheduleInfo[] GetTaskSchedules(
      bool activeOnly,
      bool includeSystemTasks)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTaskSchedules), new object[2]
      {
        (object) activeOnly,
        (object) includeSystemTasks
      });
      try
      {
        return TaskScheduleAccessor.GetTaskSchedules(activeOnly, includeSystemTasks);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ServerTaskScheduleInfo[]) null;
      }
    }

    public virtual ServerTaskScheduleInfo[] GetUserTaskSchedules(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUserTaskSchedules), new object[1]
      {
        (object) userId
      });
      try
      {
        return TaskScheduleAccessor.GetUserTaskSchedules(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ServerTaskScheduleInfo[]) null;
      }
    }

    public virtual ServerTaskScheduleInfo GetTaskSchedule(int scheduleId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTaskSchedule), new object[1]
      {
        (object) scheduleId
      });
      try
      {
        return TaskScheduleAccessor.GetTaskSchedule(scheduleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ServerTaskScheduleInfo) null;
      }
    }

    public virtual ServerTaskScheduleInfo CreateTaskSchedule(ServerTaskScheduleInfo schedule)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateTaskSchedule), new object[1]
      {
        (object) schedule
      });
      try
      {
        return TaskScheduleAccessor.CreateTaskSchedule(schedule);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ServerTaskScheduleInfo) null;
      }
    }

    public virtual void UpdateTaskSchedule(ServerTaskScheduleInfo schedule)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateTaskSchedule), new object[1]
      {
        (object) schedule
      });
      try
      {
        TaskScheduleAccessor.UpdateTaskSchedule(schedule);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteTaskSchedule(int scheduleId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteTaskSchedule), new object[1]
      {
        (object) scheduleId
      });
      try
      {
        TaskScheduleAccessor.DeleteTaskSchedule(scheduleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ServerTaskHistoryInfo[] GetTaskScheduleHistory(
      int scheduleId,
      DateTime startTime)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTaskScheduleHistory), new object[1]
      {
        (object) scheduleId
      });
      try
      {
        return TaskScheduleAccessor.GetTaskScheduleHistory(scheduleId, startTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ServerTaskHistoryInfo[]) null;
      }
    }

    public virtual EPassMessageInfo GetEPassMessage(string messageId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessage), new object[1]
      {
        (object) messageId
      });
      try
      {
        return EPassMessages.GetEPassMessage(messageId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo) null;
      }
    }

    public virtual EPassMessageInfo[] GetEPassMessages(string[] messageTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessages), (object[]) messageTypes);
      try
      {
        return EPassMessages.GetEPassMessages(messageTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual int GetEPassMessageCount(string[] messageTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessageCount), (object[]) messageTypes);
      try
      {
        return EPassMessages.GetEPassMessageCount(messageTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessagesForLoan), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return EPassMessages.GetEPassMessagesForLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid, string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessagesForLoan), new object[2]
      {
        (object) loanGuid,
        (object) userId
      });
      try
      {
        return EPassMessages.GetEPassMessagesForLoan(loanGuid, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual EPassMessageInfo[] GetEPassMessagesForUser(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessagesForUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return EPassMessages.GetEPassMessagesForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual EPassMessageInfo[] GetEPassMessagesForUser(string userId, string[] messageTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessagesForUser), new object[2]
      {
        (object) userId,
        (object) messageTypes
      });
      try
      {
        return EPassMessages.GetEPassMessagesForUser(userId, messageTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual int GetEPassMessageCountForUser(string userId, string[] messageTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPassMessageCountForUser), new object[2]
      {
        (object) userId,
        (object) messageTypes
      });
      try
      {
        return EPassMessages.GetEPassMessageCountForUser(userId, messageTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual EPassMessageInfo[] GetLoanMailboxMessagesForUser(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanMailboxMessagesForUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return EPassMessages.GetLoanMailboxMessagesForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EPassMessageInfo[]) null;
      }
    }

    public virtual int GetLoanMailboxMessageCountForUser(string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanMailboxMessageCountForUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return EPassMessages.GetLoanMailboxMessageCountForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual void UpsertEPassMessage(EPassMessageInfo ePassMessage)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpsertEPassMessage), new object[1]
      {
        (object) ePassMessage
      });
      try
      {
        EPassMessages.UpsertEPassMessage(ePassMessage);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteEPassMessages(string[] messageIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteEPassMessages), (object[]) messageIds);
      try
      {
        EPassMessages.DeleteMessages(messageIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ResetEPassMessages()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetEPassMessages), Array.Empty<object>());
      try
      {
        this.Session.SecurityManager.DemandSuperAdministrator();
        EPassMessages.ResetMessages();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string AddMilestoneTask(MilestoneTaskDefinition milestoneTask)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddMilestoneTask), new object[1]
      {
        (object) milestoneTask
      });
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Add a new milestone Tasks"));
        return MilestoneTaskAccessor.AddMilestoneTask(milestoneTask);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return string.Empty;
    }

    public virtual Hashtable GetMilestoneTasks()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMilestoneTasks), Array.Empty<object>());
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved all milestone Tasks and put them in a hash table"));
        return MilestoneTaskAccessor.GetMilestoneTasks();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual MilestoneTaskDefinition[] GetMilestoneTasks(string[] taskGUIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMilestoneTasks), (object[]) taskGUIDs);
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved all milestone Tasks"));
        return MilestoneTaskAccessor.GetMilestoneTasks(taskGUIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (MilestoneTaskDefinition[]) null;
      }
    }

    public virtual void UpdateMilestoneTask(MilestoneTaskDefinition milestoneTask)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateMilestoneTask), new object[1]
      {
        (object) milestoneTask
      });
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Update milestone task."));
        MilestoneTaskAccessor.UpdateMilestoneTask(milestoneTask);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteMilestoneTasks(string[] taskGUIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteMilestoneTasks), (object[]) taskGUIDs);
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Delete selected milestone Tasks"));
        MilestoneTaskAccessor.DeleteMilestoneTasks(taskGUIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool IsDuplicatedMilestoneTasks(string newTaskName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsDuplicatedMilestoneTasks), new object[1]
      {
        (object) newTaskName
      });
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Check duplicated task name"));
        return MilestoneTaskAccessor.IsDuplicatedMilestoneTasks(newTaskName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateLRAdditionalFields(LRAdditionalFields fields)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLRAdditionalFields), new object[1]
      {
        (object) fields
      });
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Update Lock Request and Loan Snapshot Additional Fields"));
        SecondaryRegistrationAccessor.UpdateLRAdditionalFields(fields);
        FieldSearchRule fieldSearchRule = new FieldSearchRule(fields);
        if (fieldSearchRule.FieldSearchFields.Count > 0)
          FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
        else
          FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.LockRequestAdditionalFields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LRAdditionalFields GetLRAdditionalFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLRAdditionalFields), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Get Lock Request and Loan Snapshot Additional Fields"));
        return SecondaryRegistrationAccessor.GetLRAdditionalFields();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LRAdditionalFields) null;
      }
    }

    public virtual bool IsReferencedAsDefaultViewTemplatePath(FileSystemEntry source)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsReferencedAsDefaultViewTemplatePath), new object[1]
      {
        (object) source
      });
      try
      {
        return Dashboard.IsReferencedAsDefaultViewTemplatePath(source);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void RemoveAllDefaultViewReference(FileSystemEntry source)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveAllDefaultViewReference), new object[1]
      {
        (object) source
      });
      try
      {
        Dashboard.RemoveAllDefaultViewReference(source);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int CreateBusinessCalendar(
      BusinessCalendar calendar,
      HolidaySchedule defaultHolidays)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateBusinessCalendar), new object[2]
      {
        (object) calendar,
        (object) defaultHolidays
      });
      try
      {
        return BusinessCalendarAccessor.CreateBusinessCalendar(calendar, defaultHolidays);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual List<BusinessCalendar> GetAllBusinessCalendars()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllBusinessCalendar", new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return BusinessCalendarAccessor.GetAllBusinessCalendars();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<BusinessCalendar>) null;
      }
    }

    public virtual BusinessCalendar GetBusinessCalendar(CalendarType calendarType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBusinessCalendar), new object[1]
      {
        (object) calendarType
      });
      try
      {
        return BusinessCalendarAccessor.GetBusinessCalendar(calendarType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BusinessCalendar) null;
      }
    }

    public virtual BusinessCalendar GetBusinessCalendar(
      CalendarType calendarType,
      DateTime startDate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBusinessCalendar), new object[2]
      {
        (object) calendarType,
        (object) startDate
      });
      try
      {
        return BusinessCalendarAccessor.GetBusinessCalendar(calendarType, startDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BusinessCalendar) null;
      }
    }

    public virtual BusinessCalendar GetBusinessCalendar(
      CalendarType calendarType,
      DateTime startDate,
      DateTime endDate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBusinessCalendar), new object[4]
      {
        (object) calendarType,
        (object) startDate,
        (object) endDate,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return BusinessCalendarAccessor.GetBusinessCalendar(calendarType, startDate, endDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BusinessCalendar) null;
      }
    }

    public virtual BusinessCalendar GetFullCalendar(
      CalendarType calendarType,
      DateTime startDate,
      DateTime endDate)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetBusinessCalendar", new object[3]
      {
        (object) calendarType,
        (object) startDate,
        (object) endDate
      });
      try
      {
        return BusinessCalendarAccessor.GetFullCalendar(calendarType, startDate, endDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BusinessCalendar) null;
      }
    }

    public virtual void SaveBusinessCalendar(BusinessCalendar calendar)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveBusinessCalendar), new object[1]
      {
        (object) calendar
      });
      try
      {
        BusinessCalendarAccessor.SaveBusinessCalendar(calendar);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DateTime AddBusinessDays(
      CalendarType calendarType,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddBusinessDays), new object[4]
      {
        (object) calendarType,
        (object) date,
        (object) daysToAdd,
        (object) startFromClosestBusinessDay
      });
      try
      {
        return BusinessCalendarAccessor.AddBusinessDays(calendarType, date, daysToAdd, startFromClosestBusinessDay);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual DateTime GetClosestBusinessDay(CalendarType calendarType, DateTime date)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetClosestBusinessDay), new object[2]
      {
        (object) calendarType,
        (object) date
      });
      try
      {
        return BusinessCalendarAccessor.GetClosestBusinessDay(calendarType, date);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual Dictionary<string, DisclosureTrackingFormItem.FormType> GetAllDisclosureFroms()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllDisclosureTrackingFroms", Array.Empty<object>());
      try
      {
        return DisclosureFormAccessor.GetDisclosureFroms();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      }
    }

    public virtual void UpdateDisclosureTrackingFroms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateDisclosureTrackingFroms), new object[1]
      {
        (object) formList
      });
      try
      {
        DisclosureFormAccessor.UpdateDisclosureForms(formList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<string, DisclosureTrackingFormItem.FormType> GetAllDisclosure2015Forms()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllDisclosure2015Forms), Array.Empty<object>());
      try
      {
        return DisclosureFormAccessor.GetDisclosure2015Forms();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      }
    }

    public virtual void UpdateDisclosureTracking2015Forms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList)
    {
      this.onApiCalled(nameof (ConfigurationManager), "UpdateDisclosure2015Forms", new object[1]
      {
        (object) formList
      });
      try
      {
        DisclosureFormAccessor.UpdateDisclosure2015Forms(formList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateFeeManagement(FeeManagementSetting feeManagementSetting)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateFeeManagement), new object[1]
      {
        (object) feeManagementSetting
      });
      if (feeManagementSetting == null)
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Fee Management Setting cannot be null", "info", this.Session.SessionInfo));
      try
      {
        SystemConfiguration.UpdateFeeManagement(feeManagementSetting);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Fee Management Setting updated"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FeeManagementSetting GetFeeManagement()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFeeManagement), Array.Empty<object>());
      try
      {
        return SystemConfiguration.GetFeeManagement();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (FeeManagementSetting) null;
    }

    public virtual List<ProductPricingSetting> GetProductPricingSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetProductPricingSettings), Array.Empty<object>());
      try
      {
        return ProductPricingSettingsAccessor.GetProductPricingSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ProductPricingSetting>();
      }
    }

    public virtual List<ProductPricingSetting> UpdateProductPricingSettings(
      List<ProductPricingSetting> settings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateProductPricingSettings), new object[1]
      {
        (object) settings
      });
      try
      {
        return ProductPricingSettingsAccessor.UpdateProductPricingSettings(settings);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ProductPricingSetting>();
      }
    }

    public virtual void ProductPricingExportUser(string providerId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ProductPricingExportUser), new object[1]
      {
        (object) providerId
      });
      try
      {
        ProductPricingSettingsAccessor.ProductPricingExportUser(providerId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ProductPricingSetting GetActiveProductPricingPartner()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetActiveProductPricingPartner), Array.Empty<object>());
      try
      {
        return ProductPricingSettingsAccessor.GetActiveProductPricingPartner();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ProductPricingSetting) null;
      }
    }

    public virtual ProductPricingSetting GetProductPricingPartner(string partnerID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetProductPricingPartner), new object[1]
      {
        (object) partnerID
      });
      try
      {
        return ProductPricingSettingsAccessor.GetProductPricingPartner(partnerID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ProductPricingSetting) null;
      }
    }

    public virtual bool IsProductAndPricingExpired(DateTime pricingDate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsProductAndPricingExpired), new object[1]
      {
        (object) pricingDate
      });
      try
      {
        int serverSetting = (int) this.Session.Context.Settings.GetServerSetting("Policies.Pricing_ElapsedTime");
        return pricingDate.AddMinutes(double.Parse(string.Concat((object) serverSetting))) < DateTime.Now;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual ePassCredentialSetting UpdateePassCredentialSetting(
      ePassCredentialSetting newSetting)
    {
      this.onApiCalled(nameof (ConfigurationManager), "UpdateCredentialSetting", new object[1]
      {
        (object) newSetting
      });
      try
      {
        return ePassPwdMmntAccessor.UpdateePassCredentialSetting(newSetting);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ePassCredentialSetting) null;
      }
    }

    public virtual List<ePassCredentialSetting> GetAllePassCredentialSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllePassCredentialSettings), Array.Empty<object>());
      try
      {
        return ePassPwdMmntAccessor.GetAllePassCredentialSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ePassCredentialSetting>();
      }
    }

    public virtual void DeleteePassCredentialSetting(int credentialID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteePassCredentialSetting), new object[1]
      {
        (object) credentialID
      });
      try
      {
        ePassPwdMmntAccessor.DeleteePassCredentialSetting(credentialID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<string> GetUserIDListByePassCredentialID(int credentialID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUserIDListByePassCredentialID), new object[1]
      {
        (object) credentialID
      });
      try
      {
        return ePassPwdMmntAccessor.GetUserIDList(credentialID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<string>();
      }
    }

    public virtual List<ePassCredentialSetting> GetUserePassCredentialSettings(string userID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUserePassCredentialSettings), new object[1]
      {
        (object) userID
      });
      try
      {
        return ePassPwdMmntAccessor.GetUserSettings(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ePassCredentialSetting>();
      }
    }

    public virtual void UpdateePassCredentialUserList(
      int credentialID,
      string providerName,
      string[] userList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateePassCredentialUserList), new object[3]
      {
        (object) credentialID,
        (object) providerName,
        (object) userList
      });
      try
      {
        ePassPwdMmntAccessor.UpdateUserList(credentialID, providerName, userList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<string> GetDuplicateUsers(
      int currentCredentialID,
      string providerName,
      string[] userList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDuplicateUsers), new object[3]
      {
        (object) currentCredentialID,
        (object) providerName,
        (object) userList
      });
      try
      {
        return userList == null || userList.Length == 0 ? new List<string>() : ePassPwdMmntAccessor.GetDuplicateUsers(currentCredentialID, providerName, userList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<string>();
      }
    }

    public virtual string[] GetLoanProgramAdditionalFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanProgramAdditionalFields), Array.Empty<object>());
      try
      {
        return LoanProgramConfiguration.GetAdditionalFields();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void SetLoanProgramAdditionalFields(string[] fieldIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetLoanProgramAdditionalFields), (object[]) fieldIds);
      try
      {
        LoanProgramConfiguration.SetAdditionalFields(fieldIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanProgramFieldSettings GetLoanProgramFieldSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanProgramFieldSettings), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        LoanProgramFieldSettings programFieldSettings = new LoanProgramFieldSettings();
        programFieldSettings.CustomFields = SystemConfiguration.GetLoanCustomFields();
        programFieldSettings.LoanProgramAdditionalFields = LoanProgramConfiguration.GetAdditionalFields();
        programFieldSettings.LockRequestAdditionalFields = new LRAdditionalFields();
        return programFieldSettings;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanProgramFieldSettings) null;
      }
    }

    public virtual bool IsZipcodeUserDefinedDuplicated(
      string zipcode,
      string zipcodeExt,
      string state,
      string city)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsZipcodeUserDefinedDuplicated), new object[4]
      {
        (object) zipcode,
        (object) zipcodeExt,
        (object) state,
        (object) city
      });
      try
      {
        return SystemConfiguration.IsZipcodeUserDefinedDuplicated(zipcode, zipcodeExt, state, city);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual ZipcodeUserDefinedList GetZipcodeUserDefinedList()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetZipcodeUserDefinedList), Array.Empty<object>());
      try
      {
        return SystemConfiguration.GetZipcodeUserDefinedList();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ZipcodeUserDefinedList) null;
    }

    public virtual ZipcodeInfoUserDefined[] GetZipcodeUserDefined(string zipcode)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetZipcodeUserDefined), new object[1]
      {
        (object) zipcode
      });
      try
      {
        return SystemConfiguration.GetZipcodeUserDefined(zipcode);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ZipcodeInfoUserDefined[]) null;
    }

    public virtual void DeleteZipcodeUserDefineds(ZipcodeInfoUserDefined[] zipcodeInfoUserDefineds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteZipcodeUserDefineds), (object[]) zipcodeInfoUserDefineds);
      try
      {
        SystemConfigurationAccessor.DeleteZipcodeUserDefineds(zipcodeInfoUserDefineds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateZipcodeUserDefined(
      ZipcodeInfoUserDefined newZipcodeInfoUserDefined,
      ZipcodeInfoUserDefined oldZipcodeInfoUserDefined)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateZipcodeUserDefined), new object[2]
      {
        (object) newZipcodeInfoUserDefined,
        (object) oldZipcodeInfoUserDefined
      });
      try
      {
        SystemConfigurationAccessor.UpdateZipcodeUserDefined(newZipcodeInfoUserDefined, oldZipcodeInfoUserDefined);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual IPRange[] GetAllowedIPRanges()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllowedIPRanges), Array.Empty<object>());
      IPRange[] allowedIpRanges = (IPRange[]) null;
      try
      {
        allowedIpRanges = ClientIPManager.GetAllowedIPRanges(this.Session.Context);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return allowedIpRanges;
    }

    public virtual void DeleteAllowedIPRange(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteAllowedIPRange), new object[1]
      {
        (object) oid
      });
      try
      {
        ClientIPManager.DeleteAllowedIPRange(this.Session.Context, oid);
        this.Session.Context.Cache.Remove(ClientIPManager.TableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteAllowedIPRanges(int[] oids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteAllowedIPRanges), new object[1]
      {
        (object) oids
      });
      try
      {
        ClientIPManager.DeleteAllowedIPRanges(this.Session.Context, oids);
        this.Session.Context.Cache.Remove(ClientIPManager.TableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddAllowedIPRange(IPRange ipRange)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddAllowedIPRange), new object[3]
      {
        (object) (ipRange.Userid ?? ""),
        (object) ipRange.StartIP,
        (object) ipRange.EndIP
      });
      try
      {
        ClientIPManager.AddAllowedIPRange(this.Session.Context, ipRange);
        this.Session.Context.Cache.Remove(ClientIPManager.TableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateAllowedIPRange(IPRange ipRange)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateAllowedIPRange), new object[4]
      {
        (object) ipRange.OID,
        (object) (ipRange.Userid ?? ""),
        (object) ipRange.StartIP,
        (object) ipRange.EndIP
      });
      try
      {
        ClientIPManager.UpdateAllowedIPRange(this.Session.Context, ipRange);
        this.Session.Context.Cache.Remove(ClientIPManager.TableName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LockExtensionPriceAdjustment[] GetLockExtensionPriceAdjustments()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLockExtensionPriceAdjustments), Array.Empty<object>());
      try
      {
        return LockExtensionPriceAdjustmentcessor.GetLockExtensionPriceAdjustments();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new LockExtensionPriceAdjustment[0];
      }
    }

    public virtual bool UpdateLockExtensionPriceAdjustments(
      LockExtensionPriceAdjustment[] newAdjustments)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLockExtensionPriceAdjustments), (object[]) newAdjustments);
      try
      {
        return LockExtensionPriceAdjustmentcessor.UpdateLockExtensionPriceAdjustment(newAdjustments);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual LockExtensionPriceAdjustment[] GetLockExtPriceAdjustPerOccurrence()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLockExtPriceAdjustPerOccurrence), Array.Empty<object>());
      try
      {
        return LockExtPriceAdjustPerOccurrenceAccessor.GetLockExtensionPriceAdjustments();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new LockExtensionPriceAdjustment[0];
      }
    }

    public virtual bool UpdateLockExtPriceAdjustPerOccurrence(
      LockExtensionPriceAdjustment[] newAdjustments)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLockExtPriceAdjustPerOccurrence), (object[]) newAdjustments);
      try
      {
        return LockExtPriceAdjustPerOccurrenceAccessor.UpdateLockExtensionPriceAdjustment(newAdjustments);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual StatusOnlineSetup GetStatusOnlineSetup(string ownerID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetStatusOnlineSetup), new object[1]
      {
        (object) ownerID
      });
      try
      {
        StatusOnlineSetup statusOnlineSetup = StatusOnlineSetupAccessor.GetStatusOnlineSetup(ownerID);
        if (statusOnlineSetup != null)
        {
          string companySetting = this.GetCompanySetting("CompanyStatusOnline", "PersonalTriggerType");
          if (!string.IsNullOrEmpty(companySetting))
            statusOnlineSetup.PersonalTriggersType = (ApplyPersonalTriggersType) Enum.Parse(typeof (ApplyPersonalTriggersType), companySetting);
        }
        return statusOnlineSetup;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (StatusOnlineSetup) null;
      }
    }

    public virtual string[] GetMilestonesByStatusOnlineTriggerGUIDs(string[] selectedGUIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetMilestonesByStatusOnlineTriggerGuids", (object[]) selectedGUIDs);
      try
      {
        return StatusOnlineSetupAccessor.GetMilestonesByStatusOnlineTriggerGUIDs(selectedGUIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void SaveStatusOnlineSetup(string ownerID, StatusOnlineSetup setup)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveStatusOnlineSetup), new object[2]
      {
        (object) ownerID,
        (object) setup
      });
      try
      {
        StatusOnlineSetupAccessor.SaveStatusOnlineSetup(ownerID, setup);
        this.SetCompanySetting("CompanyStatusOnline", "PersonalTriggerType", ((int) setup.PersonalTriggersType).ToString());
        string[] identifiersToKeep = new string[setup.Triggers.Count];
        for (int index = 0; index < setup.Triggers.Count; ++index)
        {
          StatusOnlineTrigger trigger = setup.Triggers[index];
          FieldSearchRule fieldSearchRule = new FieldSearchRule(trigger);
          if (fieldSearchRule.FieldSearchFields.Count > 0)
            FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
          else
            FieldSearchDbAccessor.DeleteFieldSearchInfo(trigger.Guid, FieldSearchRuleType.CompanyStatusOnline);
          identifiersToKeep[index] = trigger.Guid;
        }
        FieldSearchDbAccessor.DeleteFieldSearchInfo(identifiersToKeep, FieldSearchRuleType.CompanyStatusOnline);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual PlanCodeInfo[] GetCompanyPlanCodes(DocumentOrderType orderType)
    {
      return this.GetCompanyPlanCodes(orderType, false);
    }

    public virtual PlanCodeInfo[] GetCompanyPlanCodes(DocumentOrderType orderType, bool activeOnly)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 6549, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\ConfigurationManager.cs");
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyPlanCodes), new object[2]
      {
        (object) orderType,
        (object) activeOnly
      });
      PlanCodeInfo[] companyPlanCodes;
      try
      {
        companyPlanCodes = DocEngineDataAccessor.GetCompanyPlanCodes(orderType, activeOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        companyPlanCodes = (PlanCodeInfo[]) null;
      }
      PerformanceMeter.Current.AddCheckpoint("END", 6566, nameof (GetCompanyPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\ConfigurationManager.cs");
      return companyPlanCodes;
    }

    public virtual void AddCompanyPlanCodes(DocumentOrderType orderType, PlanCodeInfo[] newCodes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddCompanyPlanCodes), new object[2]
      {
        (object) orderType,
        (object) newCodes
      });
      try
      {
        DocEngineDataAccessor.AddCompanyPlanCodes(orderType, newCodes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveCompanyPlanCodes(DocumentOrderType orderType, string[] codesToRemove)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveCompanyPlanCodes), new object[2]
      {
        (object) orderType,
        (object) codesToRemove
      });
      try
      {
        DocEngineDataAccessor.RemoveCompanyPlanCodes(orderType, codesToRemove);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<CustomPlanCode> GetCompanyCustomPlanCodes(DocumentOrderType orderType)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 6609, nameof (GetCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\ConfigurationManager.cs");
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyCustomPlanCodes), new object[1]
      {
        (object) orderType
      });
      List<CustomPlanCode> companyCustomPlanCodes;
      try
      {
        companyCustomPlanCodes = DocEngineDataAccessor.GetCompanyCustomPlanCodes(orderType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        companyCustomPlanCodes = new List<CustomPlanCode>();
      }
      PerformanceMeter.Current.AddCheckpoint("END", 6624, nameof (GetCompanyCustomPlanCodes), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\ConfigurationManager.cs");
      return companyCustomPlanCodes;
    }

    public virtual CustomPlanCode GetCompanyCustomPlanCode(
      DocumentOrderType orderType,
      string planCode)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyCustomPlanCode), new object[2]
      {
        (object) orderType,
        (object) planCode
      });
      try
      {
        foreach (CustomPlanCode companyCustomPlanCode in DocEngineDataAccessor.GetCompanyCustomPlanCodes(orderType).ToArray())
        {
          if (companyCustomPlanCode.PlanCode == planCode)
            return companyCustomPlanCode;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (CustomPlanCode) null;
    }

    public virtual void AddCompanyCustomPlanCode(CustomPlanCode newPlanCode)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddCompanyCustomPlanCode), new object[1]
      {
        (object) newPlanCode
      });
      try
      {
        DocEngineDataAccessor.AddCompanyCustomPlanCodes(newPlanCode);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveCompanyCustomPlanCodes(
      DocumentOrderType orderType,
      List<CustomPlanCode> planCodeList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveCompanyCustomPlanCodes), new object[2]
      {
        (object) orderType,
        (object) planCodeList
      });
      try
      {
        DocEngineDataAccessor.RemoveCompanyCustomPlanCodes(planCodeList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual DocEngineStackingOrderInfo[] GetDocEngineStackingOrders(
      DocumentOrderType orderType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocEngineStackingOrders), new object[1]
      {
        (object) orderType
      });
      try
      {
        return DocEngineDataAccessor.GetDocEngineStackingOrders(orderType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DocEngineStackingOrderInfo[]) null;
      }
    }

    public virtual DocEngineStackingOrder GetDocEngineStackingOrder(string orderID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocEngineStackingOrder), new object[1]
      {
        (object) orderID
      });
      try
      {
        return DocEngineDataAccessor.GetDocEngineStackingOrder(orderID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DocEngineStackingOrder) null;
      }
    }

    public virtual DocEngineStackingOrder GetDocEngineStackingOrder(
      DocumentOrderType orderType,
      string name)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocEngineStackingOrder), new object[2]
      {
        (object) orderType,
        (object) name
      });
      try
      {
        return DocEngineDataAccessor.GetDocEngineStackingOrder(orderType, name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DocEngineStackingOrder) null;
      }
    }

    public virtual void SetDefaultDocEngineStackingOrder(string orderID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetDefaultDocEngineStackingOrder), new object[1]
      {
        (object) orderID
      });
      try
      {
        DocEngineDataAccessor.SetDefaultDocEngineStackingOrder(orderID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveDocEngineStackingOrder(DocEngineStackingOrder stackingOrder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveDocEngineStackingOrder), new object[1]
      {
        (object) stackingOrder
      });
      try
      {
        DocEngineDataAccessor.SaveDocEngineStackingOrder(stackingOrder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteDocEngineStackingOrder(string orderID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteDocEngineStackingOrder), new object[1]
      {
        (object) orderID
      });
      try
      {
        DocEngineDataAccessor.DeleteDocEngineStackingOrder(orderID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddKnownDocEngineStackingElements(
      DocumentOrderType orderType,
      StackingElement[] elements)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddKnownDocEngineStackingElements), new object[2]
      {
        (object) orderType,
        (object) elements
      });
      try
      {
        DocEngineDataAccessor.AddKnownDocEngineStackingElements(orderType, elements);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual StackingElement[] GetKnownDocEngineStackingElements(DocumentOrderType orderType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetKnownDocEngineStackingElements), new object[1]
      {
        (object) orderType
      });
      try
      {
        return DocEngineDataAccessor.GetKnownDocEngineStackingElements(orderType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (StackingElement[]) null;
      }
    }

    public virtual List<string> GetHighCostStates()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHighCostStates), Array.Empty<object>());
      try
      {
        return DocEngineDataAccessor.GetHighCostStates();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<string>();
      }
    }

    public virtual void UpdateHighCostStates(List<string> stateList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateHighCostStates), new object[1]
      {
        (object) stateList
      });
      try
      {
        DocEngineDataAccessor.UpdateHighCostStates(stateList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<string[]> GetChangeCircumstanceOptions()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetChangeCircumstanceOptions), Array.Empty<object>());
      try
      {
        List<string[]> circumstanceOptions = SystemConfigurationAccessor.GetChangeCircumstanceOptions();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved Change in Circumstance options"));
        return circumstanceOptions;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string[]>) null;
      }
    }

    public virtual void SetChangeCircumstanceOptions(List<string[]> options)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetChangeCircumstanceOptions), Array.Empty<object>());
      try
      {
        SystemConfigurationAccessor.SetChangeCircumstanceOptions(options);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Set Change in Circumstance options"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ChangeCircumstanceSettings> GetAllChangeCircumstanceSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllChangeCircumstanceSettings), Array.Empty<object>());
      try
      {
        List<ChangeCircumstanceSettings> circumstanceSettings = SystemConfigurationAccessor.GetAllChangeCircumstanceSettings();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved Change in Circumstance settings"));
        return circumstanceSettings;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ChangeCircumstanceSettings>) null;
      }
    }

    public virtual bool UpdateChangeCircumstance(List<ChangeCircumstanceSettings> changeCoCSettings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateChangeCircumstance), Array.Empty<object>());
      try
      {
        int num = SystemConfigurationAccessor.UpdateChangeCircumstance(changeCoCSettings) ? 1 : 0;
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Set Change in Circumstance settings"));
        return num != 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void DeleteChangeCircumstanceSetting(List<ChangeCircumstanceSettings> cocSettings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteChangeCircumstanceSetting), Array.Empty<object>());
      try
      {
        SystemConfigurationAccessor.DeleteChangeCircumstanceSetting(cocSettings);
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Delete Circumstance settings"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanCompDefaultPlan GetDefaultLoanCompPlan()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDefaultLoanCompPlan), Array.Empty<object>());
      try
      {
        return LOCompAccessor.GetDefaultLoanCompPlan();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new LoanCompDefaultPlan();
      }
    }

    public virtual void SetLoanCompDefaultPlan(LoanCompDefaultPlan loanCompDefaultPlan)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetLoanCompDefaultPlan), new object[1]
      {
        (object) loanCompDefaultPlan
      });
      try
      {
        LOCompAccessor.SetLoanCompDefaultPlan(loanCompDefaultPlan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<LoanCompPlan> GetAllCompPlans(bool activatedOnly, int compType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllCompPlans), new object[2]
      {
        (object) activatedOnly,
        (object) compType
      });
      try
      {
        return LOCompAccessor.GetAllCompPlans(activatedOnly, compType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<LoanCompPlan>) null;
      }
    }

    public virtual int AddCompPlan(LoanCompPlan newLoanCompPlan)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddCompPlan), new object[1]
      {
        (object) newLoanCompPlan
      });
      try
      {
        return LOCompAccessor.AddCompPlan(newLoanCompPlan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int UpdateCompPlan(LoanCompPlan loanCompPlan)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateCompPlan), new object[1]
      {
        (object) loanCompPlan
      });
      try
      {
        return LOCompAccessor.UpdateCompPlan(loanCompPlan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual List<int> RemoveCompPlans(int[] IDplansToRemove)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveCompPlans), new object[1]
      {
        (object) IDplansToRemove
      });
      try
      {
        return LOCompAccessor.RemoveCompPlans(IDplansToRemove);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual List<OrgInfo> GetOrganizationsUsingCompPlan(int loCompID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOrganizationsUsingCompPlan), new object[1]
      {
        (object) loCompID
      });
      try
      {
        return LOCompAccessor.GetOrganizationsUsingCompPlan(loCompID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (List<OrgInfo>) null;
    }

    public virtual CCSiteInfo createCCSiteInfo(int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (createCCSiteInfo), new object[1]
      {
        (object) orgId
      });
      try
      {
        return CCSiteInfoAccessor.getCCSiteInfo(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (CCSiteInfo) null;
    }

    public virtual List<object[]> GetUsersUsingCompPlan(int loCompID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUsersUsingCompPlan), new object[1]
      {
        (object) loCompID
      });
      try
      {
        return LOCompAccessor.GetUsersUsingCompPlan(loCompID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (List<object[]>) null;
    }

    public virtual LoanCompHistoryList GetComPlanHistoryforOrg(int orgID, bool forExternal)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetComPlanHistoryforOrg), new object[2]
      {
        (object) orgID,
        (object) forExternal
      });
      try
      {
        return LOCompAccessor.GetComPlanHistoryforOrg(orgID, forExternal);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompHistoryList) null;
      }
    }

    public virtual LoanCompHistoryList GetComPlanHistoryforUser(string userID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetComPlanHistoryforUser), new object[1]
      {
        (object) userID
      });
      try
      {
        return LOCompAccessor.GetComPlanHistoryforUser(userID, false, false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompHistoryList) null;
      }
    }

    public virtual List<LoanCompHistoryList> GetComPlanHistoryforAllOriginators()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetComPlanHistoryforAllOriginators), Array.Empty<object>());
      try
      {
        return LOCompAccessor.GetComPlanHistoryforAllOriginators();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<LoanCompHistoryList>) null;
      }
    }

    public virtual LoanCompPlan GetCurrentComPlanforUser(string userID, DateTime triggerDateTime)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCurrentComPlanforUser), new object[2]
      {
        (object) userID,
        (object) triggerDateTime
      });
      try
      {
        return LOCompAccessor.GetCurrentComPlanforUser(userID, triggerDateTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompPlan) null;
      }
    }

    public virtual LoanCompPlan GetLoanCompPlanByID(int id)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanCompPlanByID), new object[1]
      {
        (object) id
      });
      try
      {
        return LOCompAccessor.GetLoanCompPlanByID(id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompPlan) null;
      }
    }

    public virtual void CreateHistoryCompPlansForUser(
      LoanCompHistoryList loanCompHistoryList,
      string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateHistoryCompPlansForUser), new object[2]
      {
        (object) loanCompHistoryList,
        (object) userid
      });
      try
      {
        LOCompAccessor.CreateHistoryCompPlansForUser(loanCompHistoryList, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalContacts), new object[2]
      {
        (object) forLender,
        (object) newExternalContact
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalContacts(forLender, newExternalContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int ImportExternalContact(
      ExternalOriginatorManagementData newExternalContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ImportExternalContact), new object[1]
      {
        (object) newExternalContact
      });
      try
      {
        return ExternalOrgManagementAccessor.ImportExternalContact(newExternalContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalContacts), new object[3]
      {
        (object) forLender,
        (object) newExternalContact,
        (object) loanCompHistoryList
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalContacts(forLender, newExternalContact, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int AddBusinessContact(
      bool forLender,
      int businessID,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      int parent,
      int depth,
      string hierarchyPath)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddBusinessContact), new object[11]
      {
        (object) forLender,
        (object) businessID,
        (object) companyDBAName,
        (object) companyLegalName,
        (object) address,
        (object) city,
        (object) state,
        (object) zip,
        (object) parent,
        (object) depth,
        (object) hierarchyPath
      });
      try
      {
        return ExternalOrgManagementAccessor.AddBusinessContact(forLender, businessID, companyDBAName, companyLegalName, address, city, state, zip, parent, depth, hierarchyPath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int AddManualContact(
      bool forLender,
      ExternalOriginatorEntityType entityType,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      bool UseParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      ExternalOriginatorManagementData newExternalContact = new ExternalOriginatorManagementData();
      newExternalContact.entityType = entityType;
      newExternalContact.CompanyDBAName = companyDBAName;
      newExternalContact.CompanyLegalName = companyLegalName;
      newExternalContact.OrganizationName = companyLegalName;
      newExternalContact.Address = address;
      newExternalContact.City = city;
      newExternalContact.State = state;
      newExternalContact.Zip = zip;
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddManualContact), new object[7]
      {
        (object) forLender,
        (object) newExternalContact,
        (object) UseParentInfo,
        (object) parent,
        (object) depth,
        (object) hierarchyPath,
        (object) loanCompHistoryList
      });
      try
      {
        return ExternalOrgManagementAccessor.AddManualContact(forLender, newExternalContact, UseParentInfo, parent, depth, hierarchyPath, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int AddManualContact(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      bool UseParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddManualContact), new object[7]
      {
        (object) forLender,
        (object) newExternalContact,
        (object) UseParentInfo,
        (object) parent,
        (object) depth,
        (object) hierarchyPath,
        (object) loanCompHistoryList
      });
      try
      {
        return ExternalOrgManagementAccessor.AddManualContact(forLender, newExternalContact, UseParentInfo, parent, depth, hierarchyPath, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int AddTPOContact(
      bool forLender,
      string id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      ExternalOriginatorEntityType entityType,
      string address,
      string city,
      string state,
      string zip,
      int parent,
      int depth,
      string hierarchyPath)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddTPOContact), new object[13]
      {
        (object) forLender,
        (object) id,
        (object) organizationName,
        (object) companyDBAName,
        (object) companyLegalName,
        (object) entityType,
        (object) address,
        (object) city,
        (object) state,
        (object) zip,
        (object) parent,
        (object) depth,
        (object) hierarchyPath
      });
      try
      {
        return ExternalOrgManagementAccessor.AddTPOContact(forLender, id, organizationName, companyDBAName, companyLegalName, entityType, address, city, state, zip, parent, depth, hierarchyPath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateExternalContact(
      bool forLender,
      ExternalOriginatorManagementData externalContact,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalContact), new object[3]
      {
        (object) forLender,
        (object) externalContact,
        (object) loanCompHistoryList
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalContact(forLender, externalContact, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalContact(
      bool forLender,
      ExternalOriginatorManagementData externalContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalContact), new object[2]
      {
        (object) forLender,
        (object) externalContact
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalContact(forLender, externalContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalOrgLOCompPlans(
      bool forLender,
      int oid,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgLOCompPlans), new object[3]
      {
        (object) forLender,
        (object) oid,
        (object) loanCompHistoryList
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgLOCompPlans(forLender, oid, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateLOCompUseParentInfo(bool useParentInfo, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLOCompUseParentInfo), new object[2]
      {
        (object) useParentInfo,
        (object) oid
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateLOCompUseParentInfo(useParentInfo, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTPOContact(
      bool forLender,
      string id,
      ExternalOriginatorManagementData externalContact,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateTPOContact), new object[8]
      {
        (object) forLender,
        (object) id,
        (object) externalContact,
        (object) useParentInfo,
        (object) parent,
        (object) depth,
        (object) hierarchyPath,
        (object) loanCompHistoryList
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateTPOContact(forLender, id, externalContact, useParentInfo, parent, depth, hierarchyPath, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalOrgIsTestAccount(int oid, bool isTestAccount)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgIsTestAccount), new object[2]
      {
        (object) oid,
        (object) isTestAccount
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgIsTestAccount(oid, isTestAccount);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalOrgManager(int oid, string manager)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgManager), new object[2]
      {
        (object) oid,
        (object) manager
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgManager(oid, manager);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void OverwriteTPOContact(
      bool forLender,
      int oid,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      int parent,
      int depth,
      string hierarchyPath)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (OverwriteTPOContact), new object[13]
      {
        (object) forLender,
        (object) oid,
        (object) organizationName,
        (object) companyDBAName,
        (object) companyLegalName,
        (object) address,
        (object) city,
        (object) state,
        (object) zip,
        (object) entityType,
        (object) parent,
        (object) depth,
        (object) hierarchyPath
      });
      try
      {
        ExternalOrgManagementAccessor.OverwriteTPOContact(forLender, oid, organizationName, companyDBAName, companyLegalName, address, city, state, zip, entityType, parent, depth, hierarchyPath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBusinessContact(
      bool forLender,
      int id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateBusinessContact), new object[15]
      {
        (object) forLender,
        (object) id,
        (object) organizationName,
        (object) companyDBAName,
        (object) companyLegalName,
        (object) address,
        (object) city,
        (object) state,
        (object) zip,
        (object) entityType,
        (object) useParentInfo,
        (object) parent,
        (object) depth,
        (object) hierarchyPath,
        (object) loanCompHistoryList
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateBusinessContact(forLender, id, organizationName, companyDBAName, companyLegalName, address, city, state, zip, entityType, useParentInfo, parent, depth, hierarchyPath, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateManualContact(
      bool forLender,
      int id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateManualContact), new object[15]
      {
        (object) forLender,
        (object) id,
        (object) organizationName,
        (object) companyDBAName,
        (object) companyLegalName,
        (object) address,
        (object) city,
        (object) state,
        (object) zip,
        (object) entityType,
        (object) useParentInfo,
        (object) parent,
        (object) depth,
        (object) hierarchyPath,
        (object) loanCompHistoryList
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateManualContact(forLender, id, organizationName, companyDBAName, companyLegalName, address, city, state, zip, entityType, useParentInfo, parent, depth, hierarchyPath, loanCompHistoryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteExternalContact(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalContact), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalContact(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteExternalCompPlans(bool forLender, int oid, int CompPlanId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalCompPlans), new object[3]
      {
        (object) forLender,
        (object) oid,
        (object) CompPlanId
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalCompPlans(forLender, oid, CompPlanId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteExternalCompPlans(bool forLender, int oid, int[] CompPlanId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalCompPlans), new object[3]
      {
        (object) forLender,
        (object) oid,
        (object) CompPlanId
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalCompPlans(forLender, oid, CompPlanId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalOriginatorManagementData>[] GetAllExternalOrganizations()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalOrganizations), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalOrganizations();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>[]) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationBranches(
      bool forLender,
      int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationBranches), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationBranches(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationBranchesBySite(
      bool forLender,
      int oid,
      string siteID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationBranchesBySite), new object[3]
      {
        (object) forLender,
        (object) oid,
        (object) siteID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationBranchesBySite(forLender, oid, siteID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetChildExternalOrganizationByType(
      int oid,
      List<int> orgType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetChildExternalOrganizationByType), new object[2]
      {
        (object) oid,
        (object) orgType
      });
      try
      {
        return ExternalOrgManagementAccessor.GetChildExternalOrganizationByType(oid, orgType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetAllExternalOrganizations(bool forLender)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalOrganizations), new object[1]
      {
        (object) forLender
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalOrganizations(forLender);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetAllExternalParentOrganizations(
      bool forLender)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalParentOrganizations), new object[1]
      {
        (object) forLender
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalParentOrganizations(forLender);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual ExternalOriginatorManagementData GetExternalOrganization(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganization), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganization(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetOldTPOExternalOrganizations()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOldTPOExternalOrganizations), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetOldTPOExternalOrganizations();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual ExternalOriginatorManagementData GetOldExternalOrganizationByDBAName(
      string dbaName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOldExternalOrganizationByDBAName), new object[1]
      {
        (object) dbaName
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOldExternalOrganizationByDBAName(dbaName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual ExternalOriginatorManagementData GetOldExternalOrganizationByTPOID(string tpoId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOldExternalOrganizationByTPOID), new object[1]
      {
        (object) tpoId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOldExternalOrganizationByTPOID(tpoId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationByTPOID(
      string tpoId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationByTPOID), new object[1]
      {
        (object) tpoId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationByTPOID(tpoId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual int GetExternalRootOrgIdFromOrgChart()
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalRootOrgFromOrgChart", Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalRootOrgIdFromOrgChart();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual ExternalOriginatorManagementData[] QueryExternalOrganizations(
      QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (QueryExternalOrganizations), (object[]) criteria);
      try
      {
        return ExternalOrgManagementAccessor.QueryExternalOrganizations(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData[]) null;
      }
    }

    public virtual ExternalOriginatorManagementData GetRootOrganisation(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetRootOrganisation), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetRootOrganisation(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      List<int> oids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizations), new object[2]
      {
        (object) forLender,
        (object) oids
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizations(forLender, oids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      ExternalOriginatorEntityType entityType,
      string legalName,
      string dbaName,
      string cityName,
      string stateName,
      bool exactMatch,
      string sortedColumnName,
      bool sortedDescending,
      string currentUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizations), new object[10]
      {
        (object) forLender,
        (object) entityType,
        (object) legalName,
        (object) dbaName,
        (object) cityName,
        (object) stateName,
        (object) exactMatch,
        (object) sortedColumnName,
        (object) sortedDescending,
        (object) currentUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizations(forLender, entityType, legalName, dbaName, cityName, stateName, exactMatch, sortedColumnName, sortedDescending, currentUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<int> GetExternalOrganizationDesendents(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationDesendents), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationDesendents(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual List<string> GetExternalOrganizationDesendentsTPOID(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationDesendentsTPOID), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationDesendentsTPOID(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual List<int> GetExternalOrganizationParents(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationParents), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationParents(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationDesendentEntities(
      int oid,
      bool recursive)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationDesendentEntities), new object[1]
      {
        (object) oid
      });
      try
      {
        List<int> ids = new List<int>();
        if (recursive)
          ids = ExternalOrgManagementAccessor.GetExternalOrganizationDesendents(oid);
        else
          ExternalOrgManagementAccessor.GetHierarchy(false, oid).ForEach((Action<HierarchySummary>) (x => ids.Add(x.oid)));
        return ExternalOrgManagementAccessor.GetExternalOrganizations(false, ids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ExternalOriginatorManagementData>();
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetCompanyOrganizations(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyOrganizations), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCompanyOrganizations(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ExternalOriginatorManagementData>();
      }
    }

    public virtual List<string> GetAEAccessibleExternalUser(string[] userids)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAEAccessibleExternalUsers", (object[]) userids);
      try
      {
        return ExternalOrgManagementAccessor.GetAEAccessibleExternalUsers(userids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<string>();
      }
    }

    public virtual Dictionary<string, List<ExternalOriginatorManagementData>> GetCompanyAncestors(
      int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyAncestors), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCompanyAncestors(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, List<ExternalOriginatorManagementData>>();
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetCompanyOrganizations(
      string externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompanyOrganizations), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        ExternalOriginatorManagementData externalCompanyByTpoid = ExternalOrgManagementAccessor.GetExternalCompanyByTPOID(false, externalOrgID);
        return externalCompanyByTpoid == null ? new List<ExternalOriginatorManagementData>() : ExternalOrgManagementAccessor.GetCompanyOrganizations(externalCompanyByTpoid.oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ExternalOriginatorManagementData>();
      }
    }

    public virtual void UpdateOrgTypeAndTpoID(List<int> children, int orgType, string TpoID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateOrgTypeAndTpoID), new object[3]
      {
        (object) children,
        (object) orgType,
        (object) TpoID
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateOrgTypeAndTpoID(children, orgType, TpoID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalLicence(BranchExtLicensing license, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalLicence), new object[2]
      {
        (object) license,
        (object) oid
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalLicence(license, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BranchExtLicensing GetExtLicenseDetails(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExtLicenseDetails), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExtLicenseDetails(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchExtLicensing) null;
      }
    }

    public virtual BranchExtLicensing GetExportLicensesDetails(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExportLicensesDetails), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExportLicensesDetails(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchExtLicensing) null;
      }
    }

    public virtual BranchExtLicensing GetExtLicenseDetails(string externalUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExtLicenseDetails), new object[1]
      {
        (object) externalUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExtLicenseDetails(externalUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BranchExtLicensing) null;
      }
    }

    public virtual string GetExtUserNotes(string externalUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExtUserNotes), new object[1]
      {
        (object) externalUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExtUserNotes(externalUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return "";
      }
    }

    public virtual List<object> GetExternalAdditionalDetails(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAdditionalDetails), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAdditionalDetails(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<object>) null;
      }
    }

    public virtual List<object> GetExternalAdditionalDetails(string externalUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAdditionalDetails), new object[1]
      {
        (object) externalUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAdditionalDetails(externalUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<object>) null;
      }
    }

    public virtual Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> GetExternalAdditionalDetails(
      string externalUserID,
      List<ExternalUserInfo.ExternalUserInfoSetting> requested)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAdditionalDetails), new object[2]
      {
        (object) externalUserID,
        (object) requested
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAdditionalDetails(externalUserID, requested);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object>) null;
      }
    }

    public virtual Dictionary<ExternalOriginatorOrgSetting, object> GetExternalAdditionalDetails(
      int oid,
      List<ExternalOriginatorOrgSetting> requested)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAdditionalDetails), new object[2]
      {
        (object) oid,
        (object) requested
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAdditionalDetails(oid, requested);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<ExternalOriginatorOrgSetting, object>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationsUsingCompPlan(
      int loCompID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationsUsingCompPlan), new object[1]
      {
        (object) loCompID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationsUsingCompPlan(loCompID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual ExternalOriginatorManagementData GetByoid(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetByoid), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetByoid(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual LoanCompHistoryList GetCompPlansByoid(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCompPlansByoid), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCompPlansByoid(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompHistoryList) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetContactByName(
      bool forLender,
      string ContactName,
      bool searchLegalName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetContactByName), new object[3]
      {
        (object) forLender,
        (object) ContactName,
        (object) searchLegalName
      });
      try
      {
        return ExternalOrgManagementAccessor.GetContactByName(forLender, ContactName, searchLegalName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual bool CheckIfOrgNameExists(bool forLender, string orgName, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfOrgNameExists), new object[2]
      {
        (object) forLender,
        (object) orgName
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfOrgNameExists(forLender, orgName, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckIfOrgExists(bool forLender, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfOrgExists), new object[2]
      {
        (object) forLender,
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfOrgExists(forLender, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckIfOrgExistsWithTpoId(bool forLender, string TPOId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfOrgExistsWithTpoId), new object[2]
      {
        (object) forLender,
        (object) TPOId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfOrgExistsWithTpoId(forLender, TPOId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public bool CheckIfOrgExistsWithTpoIdAndSalesRep(string TPOId, string userId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfOrgExistsWithTpoIdAndSalesRep), new object[2]
      {
        (object) TPOId,
        (object) userId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfOrgExistsWithTpoIdAndSalesRep(TPOId, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual ExternalOriginatorManagementData GetExternalCompanyByTPOID(
      bool forLender,
      string TPOId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalCompanyByTPOID), new object[2]
      {
        (object) forLender,
        (object) TPOId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalCompanyByTPOID(forLender, TPOId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOriginatorManagementData) null;
      }
    }

    public virtual List<long> GetAllTpoID()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllTpoID), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllTpoID();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<long>) null;
      }
    }

    public virtual List<string> GetAllLoginEmailID(string contactID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllLoginEmailID), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllLoginEmailID(contactID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetAllContactsByLoginEmailId(
      string loginEmailID,
      string externalUserId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllContactsByLoginEmailId), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllContactsByLoginEmailId(loginEmailID, externalUserId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual List<ExternalUserInfoURL> GetExternalUserInfoUrlsByContactIds(
      string externalUserIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoUrlsByContactIds), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoUrlsByContactIds(externalUserIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfoURL>) null;
      }
    }

    public virtual HashSet<string> GetMultipleExternalUserInfoURLs(string externalUserIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetMultipleExternalUserInfoURLs), new object[1]
      {
        (object) externalUserIds
      });
      try
      {
        return ExternalOrgManagementAccessor.GetMultipleExternalUserInfoURLs(externalUserIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HashSet<string>) null;
      }
    }

    public virtual void DisableContactsLogin(string externalUserIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DisableContactsLogin), Array.Empty<object>());
      try
      {
        ExternalOrgManagementAccessor.DisableContactsLogin(externalUserIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<long> GetAllContactID()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllContactID), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllContactID();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<long>) null;
      }
    }

    public virtual int GetOidByTPOId(bool forLender, string ExternalID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOidByTPOId), new object[2]
      {
        (object) forLender,
        (object) ExternalID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOidByTPOId(forLender, ExternalID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int GetOidByBusinessId(bool forLender, int Id)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOidByBusinessId), new object[2]
      {
        (object) forLender,
        (object) Id
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOidByBusinessId(forLender, Id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual List<int> GetOidByParentId(bool forLender, int parentID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOidByParentId), new object[2]
      {
        (object) forLender,
        (object) parentID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOidByParentId(forLender, parentID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual List<HierarchySummary>[] GetAllHierarchy()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllHierarchy), Array.Empty<object>());
      try
      {
        return new List<HierarchySummary>[3]
        {
          ExternalOrgManagementAccessor.GetHierarchy(true),
          ExternalOrgManagementAccessor.GetHierarchy(false),
          ExternalOrgManagementAccessor.GetBankHierarchy()
        };
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<HierarchySummary>[]) null;
      }
    }

    public virtual List<List<HierarchySummary>> SearchOrganization(string type, string keyword)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SearchOrganization), new object[2]
      {
        (object) type,
        (object) keyword
      });
      try
      {
        return ExternalOrgManagementAccessor.SearchOrganization(type, keyword);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<List<HierarchySummary>>) null;
      }
    }

    public virtual List<HierarchySummary> GetHierarchy(bool forLender)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHierarchy), new object[1]
      {
        (object) forLender
      });
      try
      {
        return ExternalOrgManagementAccessor.GetHierarchy(forLender);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<HierarchySummary>) null;
      }
    }

    public virtual HierarchySummary GetOrgDetails(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOrgDetails), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOrgDetails(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HierarchySummary) null;
      }
    }

    public virtual List<HierarchySummary> GetHierarchy(bool forLender, int parentId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHierarchy), new object[2]
      {
        (object) forLender,
        (object) parentId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetHierarchy(forLender, parentId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<HierarchySummary>) null;
      }
    }

    public virtual bool GetUseParentInfoValue(bool forLender, int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUseParentInfoValue), new object[2]
      {
        (object) forLender,
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetUseParentInfoValue(forLender, oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void MoveExternalCompany(bool forLender, HierarchySummary summary)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveExternalCompany), new object[2]
      {
        (object) forLender,
        (object) summary
      });
      try
      {
        ExternalOrgManagementAccessor.MoveExternalCompany(forLender, summary);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanCompPlan GetCurrentComPlanforBrokerByName(
      bool forLender,
      string brokerName,
      DateTime triggerDateTime)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCurrentComPlanforBrokerByName), new object[3]
      {
        (object) forLender,
        (object) brokerName,
        (object) triggerDateTime
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByName(forLender, brokerName, triggerDateTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompPlan) null;
      }
    }

    public virtual LoanCompPlan GetCurrentComPlanforBrokerByID(
      bool forLender,
      string brokerID,
      DateTime triggerDateTime)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCurrentComPlanforBrokerByID), new object[3]
      {
        (object) forLender,
        (object) brokerID,
        (object) triggerDateTime
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByID(forLender, brokerID, triggerDateTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompPlan) null;
      }
    }

    public virtual LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(
      string TPOWebID,
      DateTime triggerDateTime)
    {
      return this.GetCurrentComPlanforBrokerByTPOWebID(false, TPOWebID, triggerDateTime);
    }

    public virtual LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(
      bool forLender,
      string TPOWebID,
      DateTime triggerDateTime)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCurrentComPlanforBrokerByTPOWebID), new object[3]
      {
        (object) forLender,
        (object) TPOWebID,
        (object) triggerDateTime
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCurrentComPlanforBrokerByTPOWebID(forLender, TPOWebID, triggerDateTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (LoanCompPlan) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationsWithoutExtension(
      string currentUserID,
      string externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationsWithoutExtension), new object[2]
      {
        (object) currentUserID,
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationsWithoutExtension(currentUserID, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual Hashtable GetExternalOrganizationsAvailableCommitment(int[] oids)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalOrganizationsCommitmentDetails", new object[1]
      {
        (object) oids
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationsAvailableCommitment(oids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual List<object> GetTPOInformationToolSettings(
      string companyExteralID,
      string companyName,
      string branchExteralID,
      string BranchName,
      string siteID,
      string currentUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTPOInformationToolSettings), new object[6]
      {
        (object) companyExteralID,
        (object) companyName,
        (object) branchExteralID,
        (object) BranchName,
        (object) siteID,
        (object) currentUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetTPOInformationToolSettings(companyExteralID, companyName, branchExteralID, BranchName, siteID, currentUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<object>) null;
      }
    }

    public virtual List<UserInfo[]> GetAllSalesRepUsers(string currentUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllSalesRepUsers), new object[1]
      {
        (object) currentUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllSalesRepUsers(currentUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<UserInfo[]>) null;
      }
    }

    public virtual string BackupTPOData()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (BackupTPOData), Array.Empty<object>());
      try
      {
        XmlDocument xmlDocument = ExternalOrgManagementAccessor.BackupTPOData();
        if (xmlDocument != null)
          return xmlDocument.OuterXml;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return "";
    }

    public virtual bool RestoreTPOData(string restoreData)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RestoreTPOData), new object[1]
      {
        (object) restoreData
      });
      try
      {
        new XmlDocument().LoadXml(restoreData);
        return ExternalOrgManagementAccessor.RestoreTPOData(restoreData);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void RebuildExternalOrgs()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RebuildExternalOrgs), Array.Empty<object>());
      try
      {
        ExternalOrgManagementAccessor.RebuildExternalOrgs();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalOrgNotes GetExternalOrganizationNotes(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationNotes), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationNotes(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgNotes) null;
      }
    }

    public virtual int AddExternalOrganizationNotes(ExternalOrgNote newExtOrgNote)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrganizationNotes), new object[1]
      {
        (object) newExtOrgNote
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrganizationNotes(newExtOrgNote);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual bool DeleteExternalOrganizationNotes(
      int oid,
      ExternalOrgNotes deletedExtOrgNotes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrganizationNotes), new object[2]
      {
        (object) oid,
        (object) deletedExtOrgNotes
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrganizationNotes(oid, deletedExtOrgNotes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteExternalOrganizationNotes(int oid, List<int> notesID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrganizationNotes), new object[2]
      {
        (object) oid,
        (object) notesID
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrganizationNotes(oid, notesID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual ExternalOrgLoanTypes GetExternalOrganizationLoanTypes(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationLoanTypes), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationLoanTypes(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgLoanTypes) null;
      }
    }

    public virtual bool UpdateExternalOrganizationLoanTypes(int oid, ExternalOrgLoanTypes loanTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrganizationLoanTypes), new object[2]
      {
        (object) oid,
        (object) loanTypes
      });
      try
      {
        return ExternalOrgManagementAccessor.UpdateExternalOrganizationLoanTypes(oid, loanTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual FieldRuleInfo GetExternalUnderwritingConditions(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUnderwritingConditions), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUnderwritingConditions(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (FieldRuleInfo) null;
      }
    }

    public virtual List<ExternalOrgAttachments> GetExternalAttachmentsByOid(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAttachmentsByOid), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAttachmentsByOid(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgAttachments>) null;
      }
    }

    public virtual List<string> GetExternalAttachmentFileNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAttachmentFileNames), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAttachmentFileNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual bool GetExternalAttachmentIsExpired(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalAttachmentIsExpired), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAttachmentIsExpired(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void InsertExternalAttachment(ExternalOrgAttachments attachment)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InsertExternalAttachment), new object[1]
      {
        (object) attachment
      });
      try
      {
        ExternalOrgManagementAccessor.InsertExternalAttachment(attachment);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalAttachment(ExternalOrgAttachments attachment)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalAttachment), new object[1]
      {
        (object) attachment
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalAttachment(attachment);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteExternalAttachment(Guid guid, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalAttachment), new object[2]
      {
        (object) guid,
        (object) entry
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalAttachment(guid, entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddAttachment(FileSystemEntry entry, BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddAttachment), new object[2]
      {
        (object) entry,
        (object) data
      });
      try
      {
        ExternalOrgManagementAccessor.AddAttachment(entry, data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject ReadAttachment(string fileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ReadAttachment), new object[1]
      {
        (object) fileName
      });
      try
      {
        return BinaryObject.Marshal(ExternalOrgManagementAccessor.ReadAttachment(fileName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual bool ReassignSalesRep(
      List<string> extUserId,
      List<int> extOrgId,
      string salesRepId,
      string currSalesRepId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ReassignSalesRep), new object[1]
      {
        (object) salesRepId
      });
      try
      {
        return ExternalOrgManagementAccessor.ReassignSalesRep(extUserId, extOrgId, salesRepId, currSalesRepId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCurrentOrg(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSalesRepsForCurrentOrg), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCurrentOrg(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgSalesRep>) null;
      }
    }

    public virtual List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCompany(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSalesRepsForCompany), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSalesRepsForCompany(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgSalesRep>) null;
      }
    }

    public virtual List<ExternalOrgLenderContact> GetExternalOrgSalesRepContactsForCompany(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalOrgSalesRepsForCompany", new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSalesRepContactsForCompany(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgLenderContact>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRep(
      string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationBySalesRep), new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationBySalesRep(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRepWithPrimarySalesRep(
      string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationBySalesRepWithPrimarySalesRep), new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationBySalesRepWithPrimarySalesRep(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual int GetExternalOrganizationCountForManagerID(string managerID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationCountForManagerID), new object[1]
      {
        (object) managerID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationCountForManagerID(managerID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual Dictionary<string, int> GetExternalOrganizationCountForManagerIDs(
      string[] managerIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationCountForManagerIDs), (object[]) managerIDs);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationCountForManagerIDs(managerIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, int>) null;
      }
    }

    public virtual List<int> GetExternalOrganizationsForManagerID(string managerID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationsForManagerID), new object[1]
      {
        (object) managerID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationsForManagerID(managerID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<int>();
      }
    }

    public virtual bool AddExternalOrganizationSalesReps(ExternalOrgSalesRep[] newReps)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrganizationSalesReps), (object[]) newReps);
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrganizationSalesReps(newReps);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool AddExternalOrganizationSalesReps(int oid, string[] userIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrganizationSalesReps), new object[2]
      {
        (object) oid,
        (object) userIds
      });
      try
      {
        ExternalOrgSalesRep[] newReps = new ExternalOrgSalesRep[userIds.Length];
        for (int index = 0; index < userIds.Length; ++index)
          newReps[index] = new ExternalOrgSalesRep(0, oid, userIds[index], "", "", "", "", "", "");
        return ExternalOrgManagementAccessor.AddExternalOrganizationSalesReps(newReps);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool UpdateExternalOrganizationSalesRep(ExternalOrgSalesRep salesRep)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrganizationSalesRep), new object[1]
      {
        (object) salesRep
      });
      try
      {
        return ExternalOrgManagementAccessor.UpdateExternalOrganizationSalesRep(salesRep);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteExternalOrganizationSalesReps(int oid, string[] userIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrganizationSalesReps), new object[2]
      {
        (object) oid,
        (object) userIDs
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrganizationSalesReps(oid, userIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void SetSalesRepAsPrimary(
      string userId,
      int externalOrgId,
      DateTime primarySalesRepAssignedDate = default (DateTime))
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetSalesRepAsPrimary), new object[2]
      {
        (object) userId,
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.SetSalesRepAsPrimary(userId, externalOrgId, primarySalesRepAssignedDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetPrimarySalesRep(int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPrimarySalesRep), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetPrimarySalesRep(externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual int GetOrgIdForExternalOrgID(int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOrgIdForExternalOrgID), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetOrgIdForExternalOrgID(externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual bool CheckIfSalesRepHasAnyContacts(string userId, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfSalesRepHasAnyContacts), new object[2]
      {
        (object) userId,
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfSalesRepHasAnyContacts(userId, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckIfSalesRepIsPrimary(string userId, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfSalesRepIsPrimary), new object[2]
      {
        (object) userId,
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfSalesRepIsPrimary(userId, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void ChangeSalesRepForContacts(
      string existingSalesRepUserId,
      string newSalesRepUserId,
      int externalOrgId,
      DateTime primaryAeSaleRepAssignedDate = default (DateTime))
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ChangeSalesRepForContacts), new object[3]
      {
        (object) existingSalesRepUserId,
        (object) newSalesRepUserId,
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.ChangeSalesRepForContacts(existingSalesRepUserId, newSalesRepUserId, externalOrgId, primaryAeSaleRepAssignedDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalSettingType> GetExternalOrgSettingTypes()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSettingTypes), (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSettingTypes();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalSettingType>) null;
      }
    }

    public virtual List<ExternalSettingValue> GetExternalOrgSettingsByType(int settingTypeId)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalSettingsByType", (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSettingsByType(settingTypeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalSettingValue>) null;
      }
    }

    public virtual List<ExternalSettingValue> GetExternalOrgSettingsByName(string settingName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSettingsByName), (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSettingsByName(settingName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalSettingValue>) null;
      }
    }

    public virtual ExternalSettingValue GetExternalOrgSettingsByID(int settingId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSettingsByID), (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSettingsByID(settingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalSettingValue) null;
      }
    }

    public virtual Dictionary<string, List<ExternalSettingValue>> GetExternalOrgSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgSettings), (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, List<ExternalSettingValue>>) null;
      }
    }

    public virtual int AddExternalOrgSettingValue(ExternalSettingValue externalSettingValue)
    {
      this.onApiCalled(nameof (ConfigurationManager), "AddExternalSettingValue", (object[]) null);
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrgSettingValue(externalSettingValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual bool DeleteExternalOrgSettingValues(string settingIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrgSettingValues), new object[1]
      {
        (object) settingIds
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrgSettingValues(settingIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateExternalOrgSettingValue(ExternalSettingValue externalSettingValue)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgSettingValue), new object[1]
      {
        (object) externalSettingValue
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgSettingValue(externalSettingValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ChangeSettingSortId(
      ExternalSettingValue oldSetting,
      ExternalSettingValue newSetting)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ChangeSettingSortId), new object[2]
      {
        (object) oldSetting,
        (object) newSetting
      });
      try
      {
        ExternalOrgManagementAccessor.ChangeSettingSortId(oldSetting, newSetting);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalOriginatorManagementData> GetExternalOrganizationsBySettingId(
      int settingId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationsBySettingId), new object[1]
      {
        (object) settingId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationsBySettingId(settingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOriginatorManagementData>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetExternalContactsBySettingId(int settingId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalContactsBySettingId), new object[1]
      {
        (object) settingId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalContactsBySettingId(settingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual bool CheckIfAttachmentWithCategoryExists(int settingId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfAttachmentWithCategoryExists), new object[1]
      {
        (object) settingId
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfAttachmentWithCategoryExists(settingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void AssignNewSettingValueToAttachments(int settingId)
    {
      this.onApiCalled(nameof (ConfigurationManager), "AssignNewSettingValueToAttachment", new object[1]
      {
        (object) settingId
      });
      try
      {
        ExternalOrgManagementAccessor.AssignNewSettingValueToAttachments(settingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactCustomFieldInfoCollection GetCustomFieldInfo()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomFieldInfo), Array.Empty<object>());
      try
      {
        ContactCustomFieldInfoCollection customFieldInfo = ExternalOrgManagementAccessor.GetCustomFieldInfo();
        TraceLog.WriteInfo(nameof (ConfigurationManager), this.formatMsg("Retrieved custom fields for TPO"));
        return customFieldInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ContactCustomFieldInfoCollection) null;
      }
    }

    public virtual void UpdateCustomFieldInfo(
      ContactCustomFieldInfoCollection customFields,
      ArrayList invalidFieldIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateCustomFieldInfo), new object[2]
      {
        (object) customFields,
        (object) invalidFieldIds
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateCustomFieldInfo(customFields, invalidFieldIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateCustomFieldValues(int orgID, ContactCustomField[] fields)
    {
      this.onApiCalled(nameof (ConfigurationManager), "updateCustomFieldValues", new object[2]
      {
        (object) fields,
        (object) orgID
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateCustomFieldValues(orgID, fields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ContactCustomField[] GetCustomFieldValues(int orgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCustomFieldValues), new object[1]
      {
        (object) orgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCustomFieldValues(orgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ContactCustomField[]) null;
      }
    }

    public virtual void AssignNewSettingValueToExternalOrg(
      int settingId,
      int settingTypeId,
      string oids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignNewSettingValueToExternalOrg), new object[3]
      {
        (object) settingId,
        (object) settingTypeId,
        (object) oids
      });
      try
      {
        ExternalOrgManagementAccessor.AssignNewSettingValueToExternalOrg(settingId, settingTypeId, oids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignNewSettingValueToContact(int settingId, string contactIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignNewSettingValueToContact), new object[2]
      {
        (object) settingId,
        (object) contactIds
      });
      try
      {
        ExternalOrgManagementAccessor.AssignNewSettingValueToContact(settingId, contactIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalOrgURL GetExternalOrganizationURLbySiteID(string siteID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationURLbySiteID), new object[1]
      {
        (object) siteID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationURLbySiteID(siteID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgURL) null;
      }
    }

    public virtual ExternalOrgURL[] GetExternalOrganizationURLs()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationURLs), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationURLs();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgURL[]) null;
      }
    }

    public virtual List<ExternalOrgURL> GetSelectedOrgUrls(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSelectedOrgUrls), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetSelectedOrgUrls(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgURL>) null;
      }
    }

    public virtual void UpdateExternalOrganizationSelectedURLs(
      int oid,
      List<ExternalOrgURL> orgUrl,
      int root)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrganizationSelectedURLs), new object[3]
      {
        (object) oid,
        (object) orgUrl,
        (object) root
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrganizationSelectedURLs(oid, orgUrl, root);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalOrganizationURL(ExternalOrgURL orgUrl)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrganizationURL), new object[1]
      {
        (object) orgUrl
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrganizationURL(orgUrl);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateExternalOrganizationURLTPOAccessStatus(ExternalOrgURL orgUrl)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrganizationURLTPOAccessStatus), new object[1]
      {
        (object) orgUrl
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrganizationURLTPOAccessStatus(orgUrl);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalOrgURL AssociateExternalOrganisationUrl(
      int oid,
      string siteId,
      int entityType)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssociateExternalOrganisationUrl), new object[3]
      {
        (object) oid,
        (object) siteId,
        (object) entityType
      });
      try
      {
        return ExternalOrgManagementAccessor.AssociateExternalOrganisationUrl(oid, siteId, entityType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgURL) null;
      }
    }

    public virtual void DeleteExternalOrgSelectedUrl(int oid, int urlid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrgSelectedUrl), new object[2]
      {
        (object) oid,
        (object) urlid
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalOrgSelectedUrl(oid, urlid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateInheritWebCenterSetupFlag(int oid, bool inheritWebCenterSetup)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateInheritWebCenterSetupFlag), new object[2]
      {
        (object) oid,
        (object) inheritWebCenterSetup
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateInheritWebCenterSetupFlag(oid, inheritWebCenterSetup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateInheritCustomFieldsFlag(int oid, bool inheritCustomFields)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateInheritCustomFieldsFlag), new object[2]
      {
        (object) oid,
        (object) inheritCustomFields
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateInheritCustomFieldsFlag(oid, inheritCustomFields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalOrgURL AddExternalOrganizationURL(string siteId, string url)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrganizationURL), new object[2]
      {
        (object) siteId,
        (object) url
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrganizationURL(siteId, url);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ExternalOrgURL) null;
    }

    public virtual bool DeleteExternalOrganizationURL(string siteId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrganizationURL), new object[1]
      {
        (object) siteId
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrganizationURL(siteId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual ExternalUserInfo[] QueryExternalUsers(QueryCriterion[] criteria)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (QueryExternalUsers), (object[]) criteria);
      try
      {
        return ExternalOrgManagementAccessor.QueryExternalUsers(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalUserInfo[]) null;
      }
    }

    public virtual ExternalUserInfo[] GetExternalUserInfos(int externalOrgID, bool isKeyContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfos), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfos(externalOrgID, isKeyContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ExternalUserInfo[0];
      }
    }

    public virtual ExternalUserInfo[] GetExternalUserInfosSummary(
      int externalOrgID,
      bool isKeyContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfosSummary), new object[2]
      {
        (object) externalOrgID,
        (object) isKeyContact
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfosSummary(externalOrgID, isKeyContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ExternalUserInfo[0];
      }
    }

    public virtual ExternalUserInfo[] GetExternalUserInfosSummary(
      int externalOrgID,
      bool isKeyContact,
      bool getPersona)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfosSummary), new object[3]
      {
        (object) externalOrgID,
        (object) isKeyContact,
        (object) getPersona
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfosSummary(externalOrgID, isKeyContact, getPersona);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ExternalUserInfo[0];
      }
    }

    public virtual ExternalUserInfo[] GetAllExternalUserInfos(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalUserInfos), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalUserInfos(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ExternalUserInfo[0];
      }
    }

    public virtual ExternalUserInfo[] GetAllExternalUserInfos(string tpoID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalUserInfos), new object[1]
      {
        (object) tpoID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalUserInfos(tpoID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ExternalUserInfo[0];
      }
    }

    public virtual ExternalUserInfo GetExternalUserInfo(string externalUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfo), new object[1]
      {
        (object) externalUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfo(externalUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalUserInfo) null;
      }
    }

    public virtual List<ExternalUserInfo> GetExternalUserInfoList(List<string> externalUserIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoList), new object[1]
      {
        (object) externalUserIDs
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoList(externalUserIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetExternalUserInfoListByContactId(List<string> contactIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoListByContactId), new object[1]
      {
        (object) contactIDs
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoListByContactId(contactIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetExternalUserInfoList(
      List<string> contactIDs,
      int urlID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoList), new object[2]
      {
        (object) contactIDs,
        (object) urlID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoList(contactIDs, urlID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual Persona[] GetExternalUserInfoUserPersonas(string contactID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoUserPersonas), new object[1]
      {
        (object) contactID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoUserPersonas(contactID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Persona[]) null;
      }
    }

    public virtual ExternalUserInfo GetExternalUserInfoByContactId(string contactId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoByContactId), new object[1]
      {
        (object) contactId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoByContactId(contactId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalUserInfo) null;
      }
    }

    public virtual List<ExternalUserInfo> GetExternalUserInfoFromEmailandURLID(
      string loginEmail,
      string URLID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoFromEmailandURLID), new object[2]
      {
        (object) loginEmail,
        (object) URLID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoFromEmailandURLID(loginEmail, URLID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual ExternalUserInfo SaveExternalUserInfo(ExternalUserInfo newExternalUserInfo)
    {
      return this.SaveExternalUserInfo(newExternalUserInfo, false, (int[]) null);
    }

    public virtual ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      int[] Urls = null)
    {
      return this.SaveExternalUserInfo(newExternalUserInfo, false, false, Urls);
    }

    public virtual ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      bool setDefaultURL,
      int[] Urls = null)
    {
      string validationMessage = "";
      this.onApiCalled(nameof (ConfigurationManager), "AddExternalUserInfo", new object[3]
      {
        (object) newExternalUserInfo,
        (object) cleanUpStateLicenseBeforeUpdateLicense,
        (object) setDefaultURL
      });
      try
      {
        if (newExternalUserInfo.UpdatedByExternalAdmin != "")
        {
          newExternalUserInfo.UpdatedBy = newExternalUserInfo.UpdatedByExternalAdmin;
          newExternalUserInfo.UpdatedByExternal = true;
        }
        else
        {
          newExternalUserInfo.UpdatedBy = this.Session.UserID;
          newExternalUserInfo.UpdatedByExternal = false;
        }
        if (!ServerUtil.ValidateExternalUserInfoData(newExternalUserInfo, out validationMessage))
          throw new ServerException(validationMessage);
        return ExternalOrgManagementAccessor.SaveExternalUserInfo(newExternalUserInfo, cleanUpStateLicenseBeforeUpdateLicense, setDefaultURL, Urls);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (ExternalUserInfo) null;
    }

    public virtual List<ExternalUserInfo> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool isCreate)
    {
      List<ExternalUserInfo> updatedExtUsers;
      this.UpsertExternalUserInfos(extUserInfos, true, out updatedExtUsers, isCreate);
      return updatedExtUsers;
    }

    public virtual List<string> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool returnUpdatedResult,
      out List<ExternalUserInfo> updatedExtUsers,
      bool isCreate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpsertExternalUserInfos), new object[1]
      {
        (object) extUserInfos
      });
      updatedExtUsers = (List<ExternalUserInfo>) null;
      try
      {
        foreach (Tuple<ExternalUserInfo, int[]> extUserInfo in extUserInfos)
        {
          ExternalUserInfo externalUserInfo = extUserInfo.Item1;
          if (externalUserInfo.UpdatedByExternalAdmin != "")
          {
            externalUserInfo.UpdatedBy = externalUserInfo.UpdatedByExternalAdmin;
            externalUserInfo.UpdatedByExternal = true;
          }
          else
          {
            externalUserInfo.UpdatedBy = this.Session.UserID;
            externalUserInfo.UpdatedByExternal = false;
          }
          string validationMessage;
          if (!ServerUtil.ValidateExternalUserInfoData(externalUserInfo, out validationMessage))
            throw new ServerException(validationMessage);
        }
        return ExternalOrgManagementAccessor.UpsertExternalUserInfos(extUserInfos, returnUpdatedResult, out updatedExtUsers, isCreate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual bool DeleteExternalUserInfo(
      int externalOrgID,
      ExternalUserInfo deletedExternalUserInfo,
      UserInfo userInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalUserInfo), new object[2]
      {
        (object) externalOrgID,
        (object) deletedExternalUserInfo
      });
      try
      {
        string userDataFolderPath = ClientContext.GetCurrent().Settings.GetUserDataFolderPath(deletedExternalUserInfo.ExternalUserID, false);
        DirectoryInfo directoryInfo = userDataFolderPath != null ? new DirectoryInfo(userDataFolderPath) : (DirectoryInfo) null;
        if (!ExternalOrgManagementAccessor.DeleteExternalUserInfo(externalOrgID, deletedExternalUserInfo, userInfo))
          return false;
        if (directoryInfo != null)
        {
          if (directoryInfo.Exists)
            directoryInfo.Delete(true);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
      return true;
    }

    public virtual bool DeleteExternalUserInfos(
      int externalOrgID,
      List<ExternalUserInfo> extUsers,
      UserInfo userinfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalUserInfos), new object[2]
      {
        (object) externalOrgID,
        (object) extUsers
      });
      try
      {
        List<DirectoryInfo> directoryInfoList = new List<DirectoryInfo>();
        foreach (ExternalUserInfo extUser in extUsers)
        {
          string userDataFolderPath = ClientContext.GetCurrent().Settings.GetUserDataFolderPath(extUser.ExternalUserID, false);
          if (userDataFolderPath != null)
            directoryInfoList.Add(new DirectoryInfo(userDataFolderPath));
        }
        if (!ExternalOrgManagementAccessor.DeleteExternalUserInfos(externalOrgID, extUsers, userinfo))
          return false;
        foreach (DirectoryInfo directoryInfo in directoryInfoList)
        {
          if (directoryInfo.Exists)
            directoryInfo.Delete(true);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
      return true;
    }

    public virtual bool UpdateExternalUserLastLogin(ExternalUserInfo newExternalUserInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalUserLastLogin), new object[2]
      {
        (object) newExternalUserInfo.ExternalUserID,
        (object) newExternalUserInfo.LastLogin
      });
      try
      {
        return ExternalOrgManagementAccessor.UpdateExternalUserLastLogin(newExternalUserInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual ExternalUserInfo ResetExternalUserInfoPassword(
      string externaluserID,
      string newPassword,
      DateTime date,
      bool requirePasswordChange)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetExternalUserInfoPassword), new object[4]
      {
        (object) externaluserID,
        (object) newPassword,
        (object) date,
        (object) requirePasswordChange
      });
      try
      {
        return ExternalOrgManagementAccessor.ResetExternalUserInfoPassword(externaluserID, newPassword, date, this.Session.UserID, requirePasswordChange);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalUserInfo) null;
      }
    }

    public virtual void SendWelcomeEmailUserInfo(
      string externaluserID,
      DateTime date,
      string userName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SendWelcomeEmailUserInfo), new object[3]
      {
        (object) externaluserID,
        (object) date,
        (object) userName
      });
      try
      {
        ExternalOrgManagementAccessor.SendWelcomeEmailUserInfo(externaluserID, date, userName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalUserInfo ResetExternalUserInfoUpdatedDate(
      string externaluserID,
      DateTime date,
      string updatedBy,
      bool updatedByExternal)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetExternalUserInfoUpdatedDate), new object[4]
      {
        (object) externaluserID,
        (object) date,
        (object) updatedBy,
        (object) updatedByExternal
      });
      try
      {
        return ExternalOrgManagementAccessor.ResetExternalUserInfoUpdatedDate(externaluserID, date, updatedBy, updatedByExternal);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalUserInfo) null;
      }
    }

    public virtual ExternalUserURL[] GetExternalUserInfoURLs(string externalUserID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoURLs), new object[1]
      {
        (object) externalUserID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoURLs(externalUserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return new ExternalUserURL[0];
    }

    public virtual List<ExternalUserURL> GetExternalUserURLs(
      string loginEmail,
      List<string> siteIds,
      string excludeExtUserID = null,
      bool activeUsersOnly = true)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserURLs), new object[1]
      {
        (object) loginEmail
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserURLs(loginEmail, siteIds, excludeExtUserID, activeUsersOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return new List<ExternalUserURL>();
    }

    public virtual string GetUrlLink(int urlID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetUrlLink), new object[1]
      {
        (object) urlID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetUrlLink(urlID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return "";
    }

    public virtual List<ExternalUserInfo> GetExternalUserInfoBySalesRep(string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalUserInfoBySalesRep), new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoBySalesRep(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return new List<ExternalUserInfo>();
    }

    public virtual List<ExternalUserInfo> GetAllCompanyManagers(int extID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllCompanyManagers), new object[1]
      {
        (object) extID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllCompanyManagers(extID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return new List<ExternalUserInfo>();
    }

    public virtual List<object> GetAllExternalOrganizationNames()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllExternalOrganizationNames), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllExternalOrganizationNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (List<object>) null;
    }

    public virtual bool CheckIfAnyTPOSiteExists()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfAnyTPOSiteExists), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.CheckIfAnyTPOSiteExists();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual HashSet<string> CheckIfTPOUsersHaveLoansAssigned(List<string> contactIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfTPOUsersHaveLoansAssigned), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.CheckIfTPOUsersHaveLoansAssigned(contactIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new HashSet<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      }
    }

    public virtual bool CheckIfTPOWebCenterProvisioned()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfTPOWebCenterProvisioned), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.CheckIfTPOWebCenterProvisioned();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckIfNewTPOSiteExists(string siteID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CheckIfNewTPOSiteExists), new object[1]
      {
        (object) siteID
      });
      try
      {
        return ExternalOrgManagementAccessor.CheckIfNewTPOSiteExists(siteID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<object> GetTPOForClosingVendorInformation(string tpoOrgID, string tpoLOID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTPOForClosingVendorInformation), new object[2]
      {
        (object) tpoOrgID,
        (object) tpoLOID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetTPOForClosingVendorInformation(tpoOrgID, tpoLOID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (List<object>) null;
    }

    public virtual void SaveExternalUserInfoURLs(string externalUserID, int[] urlIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveExternalUserInfoURLs), new object[2]
      {
        (object) externalUserID,
        (object) urlIDs
      });
      try
      {
        ExternalOrgManagementAccessor.SaveExternalUserInfoURLs(externalUserID, urlIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveExternalUserInfoURLs(string externalUserID, string[] urls)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveExternalUserInfoURLs), new object[2]
      {
        (object) externalUserID,
        (object) urls
      });
      try
      {
        if (!((UserInfo) this.GetExternalUserInfo(externalUserID) == (UserInfo) null))
          return;
        ExternalOrgURL[] organizationUrLs = this.GetExternalOrganizationURLs();
        List<int> urlIDs = new List<int>();
        ((IEnumerable<ExternalOrgURL>) organizationUrLs).ToList<ExternalOrgURL>().ForEach((Action<ExternalOrgURL>) (x =>
        {
          if (!((IEnumerable<string>) urls).Contains<string>(x.URL))
            return;
          urlIDs.Add(x.URLID);
        }));
        ExternalOrgManagementAccessor.SaveExternalUserInfoURLs(externalUserID, urlIDs.ToArray());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalUserInfo> ValidateExternalUserPasswordBySiteID(
      string loginEmail,
      string password,
      string urlID)
    {
      this.onApiCalled(nameof (ConfigurationManager), "ValidateExternalUserPassword", new object[3]
      {
        (object) loginEmail,
        (object) password,
        (object) urlID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserInfoFromEmailandURLID(loginEmail, urlID, password);
      }
      catch (Exception ex)
      {
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual bool DuplicateExternalUserLoginEmail(int orgID, string loginEmail)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DuplicateExternalUserLoginEmail), new object[2]
      {
        (object) orgID,
        (object) loginEmail
      });
      try
      {
        return ExternalOrgManagementAccessor.DuplicateExternalUserLoginEmail(orgID, loginEmail);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DoesTpoContactExists(
      ExternalUserInfo user,
      int destOid,
      List<int> urlsTobeAdded)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DoesTpoContactExists), new object[3]
      {
        (object) user,
        (object) destOid,
        (object) urlsTobeAdded
      });
      try
      {
        return ExternalOrgManagementAccessor.DoesTpoContactExists(user, destOid, urlsTobeAdded);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void MoveExternalUser(List<ExternalUserInfo> extUserList, int oId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveExternalUser), new object[1]
      {
        (object) extUserList
      });
      try
      {
        foreach (ExternalUserInfo extUser in extUserList)
        {
          if (extUser.UpdatedByExternalAdmin != "")
          {
            extUser.UpdatedBy = extUser.UpdatedByExternalAdmin;
            extUser.UpdatedByExternal = true;
          }
          else
          {
            extUser.UpdatedBy = this.Session.UserID;
            extUser.UpdatedByExternal = false;
          }
        }
        ExternalOrgManagementAccessor.MoveExternalUser(extUserList, oId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalUserInfo> GetAccessibleExternalUserInfos(string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAccessibleOrgContacts", new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAccessibleExternalUserInfos(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ExternalUserInfo>();
      }
    }

    public virtual List<object> GetAccessibleExternalUserInfoList(string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAccessibleOrgContacts", new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAccessibleExternalUserInfoList(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<object>();
      }
    }

    public virtual LoanImportRequirement GetLoanImportRequirements()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLoanImportRequirements), Array.Empty<object>());
      try
      {
        return SystemConfiguration.GetLoanImportRequirements();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return new LoanImportRequirement();
    }

    public virtual void SetLoanImportRequirements(
      LoanImportRequirement.LoanImportRequirementType fannieMaeImportRequirementType,
      string templateForFannieMaeImport,
      LoanImportRequirement.LoanImportRequirementType webCenterImportRequirementType,
      string templateForWebCenterImport)
    {
      this.SetLoanImportRequirements(new LoanImportRequirement(fannieMaeImportRequirementType, templateForFannieMaeImport, webCenterImportRequirementType, templateForWebCenterImport));
    }

    public virtual void SetLoanImportRequirements(LoanImportRequirement loanImportRequirement)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetLoanImportRequirements), new object[1]
      {
        (object) loanImportRequirement
      });
      try
      {
        SystemConfiguration.SetLoanImportRequirements(loanImportRequirement);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalOrgContact> GetExternalOrgContacts(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgContacts), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgContacts(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgContact>) null;
      }
    }

    public virtual int AddExternalOrgManualContact(ExternalOrgContact obj)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrgManualContact), new object[1]
      {
        (object) obj
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrgManualContact(obj);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual bool UpdateExternalOrgManualContact(ExternalOrgContact obj)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgManualContact), new object[1]
      {
        (object) obj
      });
      try
      {
        return ExternalOrgManagementAccessor.UpdateExternalOrgManualContact(obj);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateExternalOrgHeirarchyPath(int oid, string heirarchyPath)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgHeirarchyPath), new object[2]
      {
        (object) oid,
        (object) heirarchyPath
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgHeirarchyPath(oid, heirarchyPath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool DeleteExternalOrgContact(List<ExternalOrgContact> externalOrgContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrgContact), new object[1]
      {
        (object) externalOrgContact
      });
      try
      {
        return ExternalOrgManagementAccessor.DeleteExternalOrgContact(externalOrgContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool AddTpoUserToExtOrgContact(string[] external_userid, int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddTpoUserToExtOrgContact), new object[2]
      {
        (object) external_userid,
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.AddTpoUserToExtOrgContact(external_userid, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Dictionary<string, string> IsTpoOnWatchList(
      string companyId,
      string branchId,
      string lnProcessorId,
      string lnOfficerId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsTpoOnWatchList), new object[4]
      {
        (object) companyId,
        (object) branchId,
        (object) lnProcessorId,
        (object) lnOfficerId
      });
      try
      {
        return ExternalOrgManagementAccessor.IsTpoOnWatchList(companyId, branchId, lnProcessorId, lnOfficerId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, string>) null;
      }
    }

    public virtual ArrayList GetExternalUserAndOrgBySalesRep(string userid)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalUserIdBySalesRep", new object[1]
      {
        (object) userid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalUserAndOrgBySalesRep(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    public virtual ArrayList GetExternalAndInternalUserAndOrgBySalesRep(string userid, int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetExternalUserIdBySalesRep", new object[2]
      {
        (object) userid,
        (object) orgId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalAndInternalUserAndOrgBySalesRep(userid, orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ArrayList) null;
      }
    }

    public virtual ArrayList GetTPOWCAEView(string userid, int orgId, int urlID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTPOWCAEView), new object[3]
      {
        (object) userid,
        (object) orgId,
        (object) urlID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetTPOWCAEView(userid, orgId, urlID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new ArrayList();
      }
    }

    public virtual List<ExternalUserInfo> GetAllLOLPUsers()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllLOLPUsers), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllLOLPUsers();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetAllTPOLOLPUsers(List<int> personaIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllTPOLOLPUsers), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetAllTPOLOLPUsers(personaIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual List<ExternalUserInfo> GetAllAuthorizedDealers(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllAuthorizedDealers), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllAuthorizedDealers(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalUserInfo>) null;
      }
    }

    public virtual bool ReassignTPOLoans(
      ExternalUserInfo oldUser,
      ExternalUserInfo newUser,
      bool isTPOMVP = false)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ReassignTPOLoans), new object[2]
      {
        (object) oldUser,
        (object) newUser
      });
      try
      {
        return ExternalOrgManagementAccessor.ReassignTPOLoans(oldUser, newUser, this.Session.GetUserInfo(), isTPOMVP);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool ReassignTPOLoanSalesRep(List<int> orgList, List<string> userList)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ReassignTPOLoanSalesRep), new object[2]
      {
        (object) orgList,
        (object) userList
      });
      try
      {
        return ExternalOrgManagementAccessor.ReassignTPOLoanSalesRep(orgList, userList, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<ExternalBank> GetExternalBankByName(string bankName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalBankByName), new object[1]
      {
        (object) bankName
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalBankByName(bankName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalBank>) null;
      }
    }

    public virtual List<ExternalBank> GetExternalBanks()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalBanks), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalBanks();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalBank>) null;
      }
    }

    public List<ExternalBank> GetPaginatedExternalBanks(int start, int limit, out int totalRecords)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetPaginatedExternalBanks), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetPaginatedExternalBanks(start, limit, out totalRecords);
      }
      catch (Exception ex)
      {
        totalRecords = -1;
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalBank>) null;
      }
    }

    public virtual List<ExternalBank> GetSelectedExternalBanks(int[] bankIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetSelectedExternalBanks), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetSelectedExternalBanks(bankIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalBank>) null;
      }
    }

    public virtual int AddExternalBank(ExternalBank bank)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalBank), new object[1]
      {
        (object) bank
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalBank(bank);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual void UpdateExternalBank(int id, ExternalBank bank)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalBank), new object[2]
      {
        (object) id,
        (object) bank
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalBank(id, bank);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalBank GetExternalBankById(int id)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalBankById), new object[1]
      {
        (object) id
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalBankById(id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalBank) null;
      }
    }

    public virtual void DeleteExternalBank(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalBank), new object[1]
      {
        (object) oid
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalBank(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool AnyWarehousesUsingThisBank(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AnyWarehousesUsingThisBank), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.AnyWarehousesUsingThisBank(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual List<HierarchySummary> GetBankHierarchy()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBankHierarchy), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetBankHierarchy();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<HierarchySummary>) null;
      }
    }

    public virtual ExternalOrgWarehouse AddExternalOrgWarehouse(ExternalOrgWarehouse warehouse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddExternalOrgWarehouse), new object[1]
      {
        (object) warehouse
      });
      try
      {
        return ExternalOrgManagementAccessor.AddExternalOrgWarehouse(warehouse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgWarehouse) null;
      }
    }

    public virtual void UpdateExternalOrgWarehouse(int id, ExternalOrgWarehouse warehouse)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateExternalOrgWarehouse), new object[2]
      {
        (object) id,
        (object) warehouse
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateExternalOrgWarehouse(id, warehouse);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteExternalOrgWarehouse(int id, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteExternalOrgWarehouse), new object[2]
      {
        (object) id,
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteExternalOrgWarehouse(id, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalOrgWarehouse> GetExternalOrgWarehouses(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgWarehouses), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgWarehouses(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgWarehouse>) null;
      }
    }

    public virtual List<ExternalOrgWarehouse> GetExternalOrgWarehousesbyCompanies(int oid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgWarehousesbyCompanies), new object[1]
      {
        (object) oid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgWarehousesbyCompanies(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgWarehouse>) null;
      }
    }

    public virtual bool GetInheritWarehouses(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetInheritWarehouses), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetInheritWarehouses(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void UpdateInheritWarehouses(int externalOrgID, bool useParentInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateInheritWarehouses), new object[2]
      {
        (object) externalOrgID,
        (object) useParentInfo
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateInheritWarehouses(externalOrgID, useParentInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetStackingOrderTemplateNamesByExportTemplates(
      string[] documentExportTemplateNames)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetStackingOrderTemplateNamesByExportTemplates), (object[]) documentExportTemplateNames);
      try
      {
        return SystemConfigurationAccessor.GetStackingOrderTemplateNamesByExportTemplates(documentExportTemplateNames);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string GetDrysdaleHostForInstance(string instanceName)
    {
      return new DirectoryServiceClient().GetEntry(instanceName, "Host", "Drysdale").Value.ToString();
    }

    public virtual bool UpdateHomeCounselingCodes(
      List<KeyValuePair<string, string>> services,
      List<KeyValuePair<string, string>> languages)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateHomeCounselingCodes), new object[2]
      {
        (object) services,
        (object) languages
      });
      try
      {
        return HomeCounselingAccessor.UpdateHomeCounselingCodes(services, languages);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<KeyValuePair<string, string>>[] GetHomeCounselingServiceLanguageSupported()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHomeCounselingServiceLanguageSupported), Array.Empty<object>());
      try
      {
        return HomeCounselingAccessor.GetHomeCounselingServiceLanguageSupported();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<KeyValuePair<string, string>>[]) null;
      }
    }

    public virtual List<KeyValuePair<string, string>> GetHomeCounselingServiceSupported()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHomeCounselingServiceSupported), Array.Empty<object>());
      try
      {
        return HomeCounselingAccessor.GetHomeCounselingServiceSupported();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<KeyValuePair<string, string>>) null;
      }
    }

    public virtual List<KeyValuePair<string, string>> GetHomeCounselingLanguageSupported()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetHomeCounselingLanguageSupported), Array.Empty<object>());
      try
      {
        return HomeCounselingAccessor.GetHomeCounselingLanguageSupported();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<KeyValuePair<string, string>>) null;
      }
    }

    public virtual List<ExternalFeeManagement> GetFeeManagement(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFeeManagement), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetFeeManagement(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalFeeManagement>) null;
      }
    }

    public virtual List<ExternalFeeManagement> GetFeeManagementListByChannel(
      ExternalOriginatorEntityType channel)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFeeManagementListByChannel), new object[1]
      {
        (object) channel
      });
      try
      {
        return ExternalOrgManagementAccessor.GetFeeManagementListByChannel(channel);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalFeeManagement>) null;
      }
    }

    public virtual ExternalLateFeeSettings GetGlobalLateFeeSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetGlobalLateFeeSettings), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetGlobalLateFeeSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalLateFeeSettings) null;
      }
    }

    public virtual ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      string externalTPOCompanyID,
      bool getGlobalIfNotFound)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgLateFeeSettings), new object[2]
      {
        (object) externalTPOCompanyID,
        (object) getGlobalIfNotFound
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgLateFeeSettings(externalTPOCompanyID, getGlobalIfNotFound);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalLateFeeSettings) null;
      }
    }

    public virtual ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      int externalOrgID,
      bool getGlobalIfNotFound)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgLateFeeSettings), new object[2]
      {
        (object) externalOrgID,
        (object) getGlobalIfNotFound
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgLateFeeSettings(externalOrgID, getGlobalIfNotFound);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalLateFeeSettings) null;
      }
    }

    public virtual int GetGlobalOrSpecificTPOSetting(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetGlobalOrSpecificTPOSetting), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetGlobalOrSpecificTPOSetting(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual void InsertFeeManagementSettings(
      ExternalFeeManagement feeManagement,
      int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InsertFeeManagementSettings), new object[2]
      {
        (object) feeManagement,
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.InsertFeeManagementSettings(feeManagement, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateFeeManagementSettings(ExternalFeeManagement feeManagement)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateFeeManagementSettings), new object[1]
      {
        (object) feeManagement
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateFeeManagementSettings(feeManagement);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetDefaultFeeManagementListByChannel(
      int externalOrgID,
      ExternalOriginatorEntityType channel)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetDefaultFeeManagementListByChannel), new object[2]
      {
        (object) externalOrgID,
        (object) channel
      });
      try
      {
        ExternalOrgManagementAccessor.SetDefaultFeeManagementListByChannel(externalOrgID, channel);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetSelectedFeeManagementList(
      int externalOrgID,
      List<ExternalFeeManagement> fees)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetSelectedFeeManagementList), new object[2]
      {
        (object) externalOrgID,
        (object) fees
      });
      try
      {
        ExternalOrgManagementAccessor.SetSelectedFeeManagementList(externalOrgID, fees);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteFeeManagementSettings(List<int> feeManagementIDs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteFeeManagementSettings), new object[1]
      {
        (object) feeManagementIDs
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteFeeManagementSettings(feeManagementIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteTPOFeeManagementSettings(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteTPOFeeManagementSettings), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteTPOFeeManagementSettings(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void InsertLateFeeSettings(
      ExternalLateFeeSettings lateFeeSettings,
      int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InsertLateFeeSettings), new object[2]
      {
        (object) lateFeeSettings,
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.InsertLateFeeSettings(lateFeeSettings, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateOrgLateFeeSettings(
      ExternalLateFeeSettings lateFeeSettings,
      int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateOrgLateFeeSettings), new object[2]
      {
        (object) lateFeeSettings,
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateOrgLateFeeSettings(lateFeeSettings, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateGlobalLateFeeSettings(ExternalLateFeeSettings lateFeeSettings)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateGlobalLateFeeSettings), new object[1]
      {
        (object) lateFeeSettings
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateGlobalLateFeeSettings(lateFeeSettings);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteLateFeeSettings(int lateFeeSettingID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteLateFeeSettings), new object[1]
      {
        (object) lateFeeSettingID
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteLateFeeSettings(lateFeeSettingID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateGlobalOrSpecificTPOSetting(int externalOrgID, int globalOrSpecificTPO)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateGlobalOrSpecificTPOSetting), new object[2]
      {
        (object) externalOrgID,
        (object) globalOrSpecificTPO
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateGlobalOrSpecificTPOSetting(externalOrgID, globalOrSpecificTPO);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<ExternalOrgDBAName> GetDBANames(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDBANames), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetDBANames(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<ExternalOrgDBAName>) null;
      }
    }

    public virtual ExternalOrgDBAName GetDefaultDBAName(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDefaultDBAName), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetDefaultDBAName(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgDBAName) null;
      }
    }

    public virtual bool GetInheritDBANameSetting(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetInheritDBANameSetting), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetInheritDBANameSetting(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual int InsertDBANames(ExternalOrgDBAName name, int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InsertDBANames), new object[2]
      {
        (object) name,
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.InsertDBANames(name, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual void SetDBANameAsDefault(int externalOrgID, int DBAID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetDBANameAsDefault), new object[2]
      {
        (object) externalOrgID,
        (object) DBAID
      });
      try
      {
        ExternalOrgManagementAccessor.SetDBANameAsDefault(externalOrgID, DBAID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetDBANamesSortIndex(Dictionary<int, int> dbas, int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SetDBANamesSortIndex), new object[2]
      {
        (object) dbas,
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.SetDBANamesSortIndex(dbas, externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteDBANames(List<ExternalOrgDBAName> names)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteDBANames), new object[1]
      {
        (object) names
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteDBANames(names);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateDBANames(ExternalOrgDBAName name)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateDBANames), new object[1]
      {
        (object) name
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateDBANames(name);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateInheritDBANameSetting(int externalOrgID, bool useParentInfo)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateInheritDBANameSetting), new object[2]
      {
        (object) externalOrgID,
        (object) useParentInfo
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateInheritDBANameSetting(externalOrgID, useParentInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateAllChildrenDBANameSetting(int parent)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateAllChildrenDBANameSetting), new object[1]
      {
        (object) parent
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateAllChildrenDBANameSetting(parent);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<DocumentSettingInfo> GetExternalDocuments(
      int externalOrgID,
      int channel,
      int status)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalDocuments), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalDocuments(externalOrgID, channel, status);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<DocumentSettingInfo>) null;
      }
    }

    public virtual Dictionary<int, List<DocumentSettingInfo>> GetExternalOrgDocuments(
      int externalOrgID,
      int channel,
      int status,
      bool disableGlobalDocs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgDocuments), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgDocuments(externalOrgID, channel, status, disableGlobalDocs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, List<DocumentSettingInfo>>) null;
      }
    }

    public virtual List<DocumentSettingInfo> GetExternalDocumentsForOrgAssignment(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalDocumentsForOrgAssignment), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalDocumentsForOrgAssignment(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<DocumentSettingInfo>) null;
      }
    }

    public virtual List<DocumentSettingInfo> GetAllArchiveDocuments(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetAllDocumentSetting", new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetAllArchiveDocuments(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<DocumentSettingInfo>) null;
      }
    }

    public virtual void UnArchiveDocuments(int externalOrgID, List<string> guids)
    {
      this.onApiCalled(nameof (ConfigurationManager), "ArchiveDocuments", new object[2]
      {
        (object) externalOrgID,
        (object) guids
      });
      try
      {
        ExternalOrgManagementAccessor.UnArchiveDocuments(externalOrgID, guids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ArchiveDocuments(int externalOrgID, List<string> guids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ArchiveDocuments), new object[2]
      {
        (object) externalOrgID,
        (object) guids
      });
      try
      {
        ExternalOrgManagementAccessor.ArchiveDocuments(externalOrgID, guids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteDocument(int externalOrgID, Guid guid, FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteDocument), new object[3]
      {
        (object) externalOrgID,
        (object) guid,
        (object) entry
      });
      try
      {
        ExternalOrgManagementAccessor.DeleteDocument(externalOrgID, guid, entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddDocument(
      int externalOrgID,
      DocumentSettingInfo document,
      bool isTopOfCategory)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddDocument), new object[3]
      {
        (object) externalOrgID,
        (object) document,
        (object) isTopOfCategory
      });
      try
      {
        ExternalOrgManagementAccessor.AddDocument(externalOrgID, document, isTopOfCategory);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateDocument(int externalOrgID, DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateDocument), new object[2]
      {
        (object) externalOrgID,
        (object) document
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateDocument(externalOrgID, document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateActiveStatus(
      int externalOrgID,
      bool activeChecked,
      bool isDefault,
      Guid guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateActiveStatus), new object[4]
      {
        (object) externalOrgID,
        (object) activeChecked,
        (object) isDefault,
        (object) guid
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateActiveStatus(externalOrgID, activeChecked, isDefault, guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateActiveStatusAllDocsInCategory(
      int externalOrgID,
      int category,
      bool activeChecked)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateActiveStatusAllDocsInCategory), new object[3]
      {
        (object) externalOrgID,
        (object) category,
        (object) activeChecked
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateActiveStatusAllDocsInCategory(externalOrgID, category, activeChecked);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignDefaultDocumentToAll(DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignDefaultDocumentToAll), new object[1]
      {
        (object) document
      });
      try
      {
        ExternalOrgManagementAccessor.AssignDefaultDocumentToAll(document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignAllDefaultDocumentsToTOPOrg(int externalOrgID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignAllDefaultDocumentsToTOPOrg), new object[1]
      {
        (object) externalOrgID
      });
      try
      {
        ExternalOrgManagementAccessor.AssignAllDefaultDocumentsToTOPOrg(externalOrgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveDefaultDocumentFromAll(DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), "RemoveDefaultDocumentToAll", new object[1]
      {
        (object) document
      });
      try
      {
        ExternalOrgManagementAccessor.RemoveDefaultDocumentFromAll(document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveAssignedDocFromTPO(string guid, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveAssignedDocFromTPO), new object[2]
      {
        (object) guid,
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.RemoveAssignedDocFromTPO(guid, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveAssignedDocFromTPOs(string guid, List<int> externalOrgIds)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveAssignedDocFromTPOs), new object[2]
      {
        (object) guid,
        (object) externalOrgIds
      });
      try
      {
        ExternalOrgManagementAccessor.RemoveAssignedDocFromTPOs(guid, externalOrgIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AssignDocumentToOrg(DocumentSettingInfo document, bool isTopOfCategory)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignDocumentToOrg), new object[2]
      {
        (object) document,
        (object) isTopOfCategory
      });
      try
      {
        ExternalOrgManagementAccessor.AssignDocumentToOrg(document, isTopOfCategory);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateDocumentCategory(int oldCategory, int newCategory)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateDocumentCategory), new object[2]
      {
        (object) oldCategory,
        (object) newCategory
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateDocumentCategory(oldCategory, newCategory);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SwapDocumentSortIds(
      int externalOrgID,
      DocumentSettingInfo firstDoc,
      DocumentSettingInfo secondDoc)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SwapDocumentSortIds), new object[3]
      {
        (object) externalOrgID,
        (object) firstDoc,
        (object) secondDoc
      });
      try
      {
        ExternalOrgManagementAccessor.SwapDocumentSortIds(externalOrgID, firstDoc, secondDoc);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CreateDocumentInDataFolder(FileSystemEntry entry, BinaryObject data)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (CreateDocumentInDataFolder), new object[2]
      {
        (object) entry,
        (object) data
      });
      try
      {
        ExternalOrgManagementAccessor.CreateDocumentInDataFolder(entry, data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject ReadDocumentFromDataFolder(string fileName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ReadDocumentFromDataFolder), new object[1]
      {
        (object) fileName
      });
      try
      {
        return BinaryObject.Marshal(ExternalOrgManagementAccessor.ReadDocumentFromDataFolder(fileName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void MoveDocumentSortOrder(
      int externalOrgId,
      DocumentSettingInfo document,
      int startSortId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (MoveDocumentSortOrder), new object[3]
      {
        (object) externalOrgId,
        (object) document,
        (object) startSortId
      });
      try
      {
        ExternalOrgManagementAccessor.MoveDocumentSortOrder(externalOrgId, document, startSortId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int GetDocumentSortId(int externalOrgId, DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocumentSortId), new object[2]
      {
        (object) externalOrgId,
        (object) document
      });
      try
      {
        return ExternalOrgManagementAccessor.GetDocumentSortId(externalOrgId, document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void AssignDocumentsToTposByRelatedDoc(
      List<int> externalOrgIds,
      DocumentSettingInfo document,
      DocumentSettingInfo relatedDocument,
      bool IsTopOfCategoryorDoc)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignDocumentsToTposByRelatedDoc), new object[4]
      {
        (object) externalOrgIds,
        (object) document,
        (object) relatedDocument,
        (object) IsTopOfCategoryorDoc
      });
      try
      {
        ExternalOrgManagementAccessor.AssignDocumentsToTposByRelatedDoc(externalOrgIds, document, relatedDocument, IsTopOfCategoryorDoc);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int GetDocumentMaxSortId(int externalOrgid, DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocumentMaxSortId), new object[2]
      {
        (object) externalOrgid,
        (object) document
      });
      try
      {
        return ExternalOrgManagementAccessor.GetDocumentMaxSortId(externalOrgid, document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void AssignDocumentToTpos(List<int> externalOrgIds, DocumentSettingInfo document)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AssignDocumentToTpos), new object[2]
      {
        (object) externalOrgIds,
        (object) document
      });
      try
      {
        ExternalOrgManagementAccessor.AssignDocumentToTpos(externalOrgIds, document);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<int> GetExternalOrgsByDocumentGuid(Guid guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgsByDocumentGuid), new object[1]
      {
        (object) guid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgsByDocumentGuid(guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual Dictionary<string, bool> GetDocAssignedTPOs(Guid guid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetDocAssignedTPOs), new object[1]
      {
        (object) guid
      });
      try
      {
        return ExternalOrgManagementAccessor.GetDocAssignedTPOs(guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, bool>) null;
      }
    }

    public virtual Dictionary<CorrespondentMasterDeliveryType, Decimal> GetNonAllocatedOutstandingCommitments(
      string externalID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetNonAllocatedOutstandingCommitments), new object[1]
      {
        (object) externalID
      });
      try
      {
        return ExternalOrgManagementAccessor.GetNonAllocatedOutstandingCommitments(externalID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<CorrespondentMasterDeliveryType, Decimal>) null;
      }
    }

    public virtual bool IsInOutstandingCommitments(string loanGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsInOutstandingCommitments), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return ExternalOrgManagementAccessor.IsInOutstandingCommitments(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Dictionary<CorrespondentTradeCommitmentType, Decimal> GetCommitmentAvailableAmounts(
      ExternalOriginatorManagementData orgData)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCommitmentAvailableAmounts), new object[1]
      {
        (object) orgData
      });
      try
      {
        CorrespondentTradeManager correspondentTradeManager = InterceptionUtils.NewInstance<CorrespondentTradeManager>().Initialize(this.Session);
        return this.GetCommitmentAvailableAmounts(orgData, (ICorrespondentTradeManager) correspondentTradeManager);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<CorrespondentTradeCommitmentType, Decimal>) null;
      }
    }

    public virtual Dictionary<CorrespondentTradeCommitmentType, Decimal> GetCommitmentAvailableAmounts(
      ExternalOriginatorManagementData orgData,
      ICorrespondentTradeManager correspondentTradeManager)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetCommitmentAvailableAmounts), new object[1]
      {
        (object) orgData
      });
      try
      {
        return ExternalOrgManagementAccessor.GetCommitmentAvailableAmounts(orgData, correspondentTradeManager);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<CorrespondentTradeCommitmentType, Decimal>) null;
      }
    }

    public virtual ExternalOrgOnrpSettings GetExternalOrgOnrpSettings(int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgOnrpSettings), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgOnrpSettings(externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgOnrpSettings) null;
      }
    }

    public virtual ExternalOrgOnrpSettings GetExternalOrgOnrpSettingsByTPOId(string tpoId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrgOnrpSettingsByTPOId), new object[1]
      {
        (object) tpoId
      });
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrgOnrpSettingsByTPOId(tpoId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgOnrpSettings) null;
      }
    }

    public virtual void InsertOnrpSettings(ExternalOrgOnrpSettings onrpSettings, int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (InsertOnrpSettings), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.InsertOnrpSettings(onrpSettings, externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateOnrpSettings(
      ExternalOrgOnrpSettings onrpSettings,
      int externalOrgId,
      ExternalOrgOnrpSettings onrpSettingsOld)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateOnrpSettings), new object[1]
      {
        (object) externalOrgId
      });
      try
      {
        ExternalOrgManagementAccessor.UpdateOnrpSettings(onrpSettings, externalOrgId, onrpSettingsOld);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ONRPEntitySettings GetOnrpRetailSettingsByOrgId(int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetONRPRetailSettingsByOrgId", new object[1]
      {
        (object) orgId
      });
      try
      {
        return Organization.GetOrgOnrpInfo(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ONRPEntitySettings) null;
      }
    }

    public virtual bool IsRetailBranchExist(int oId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsRetailBranchExist), new object[1]
      {
        (object) oId
      });
      try
      {
        return Organization.IsRetailBranchExist(oId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<SyncTemplate> GetAllSyncTemplates()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAllSyncTemplates), Array.Empty<object>());
      try
      {
        return SyncTemplateAccessor.GetAllSyncTemplates();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<SyncTemplate>) null;
      }
    }

    public virtual List<int> RemoveSyncTemplates(List<int> ids)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (RemoveSyncTemplates), new object[1]
      {
        (object) ids
      });
      try
      {
        return SyncTemplateAccessor.RemoveSyncTemplates(ids);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<int>) null;
      }
    }

    public virtual int UpdateSyncTemplate(SyncTemplate syncTemplate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateSyncTemplate), new object[1]
      {
        (object) syncTemplate
      });
      try
      {
        return SyncTemplateAccessor.UpdateSyncTemplate(syncTemplate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual bool SyncTemplateNameExist(string templateName)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SyncTemplateNameExist), new object[1]
      {
        (object) templateName
      });
      try
      {
        return SyncTemplateAccessor.SyncTemplateNameExist(templateName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<EPPSLoanProgram> GetEPPSLoanProgramsSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEPPSLoanProgramsSettings), Array.Empty<object>());
      try
      {
        return EPPSLoanProgramsSettingsAccessor.GetEPPSLoanProgramsSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<EPPSLoanProgram>) null;
      }
    }

    public virtual void SaveEPPSLoanProgramsSettings(List<EPPSLoanProgram> programs)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveEPPSLoanProgramsSettings), Array.Empty<object>());
      try
      {
        EPPSLoanProgramsSettingsAccessor.SaveEPPSLoanProgramsSettings(programs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string getOAPIURL() => System.Configuration.ConfigurationManager.AppSettings["oAuth.Url"];

    public virtual PurchaseConditionCategory GetCategory(string categoryName)
    {
      try
      {
        return PurchaseConditionOptionsAccessor.GetCategory(categoryName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (PurchaseConditionCategory) null;
      }
    }

    public virtual List<PurchaseConditionCategory> GetAllCategories()
    {
      try
      {
        return PurchaseConditionOptionsAccessor.GetAllCategories();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<PurchaseConditionCategory>) null;
      }
    }

    public virtual List<PurchaseConditionCategory> GetSubCategories(int parentCategoryId)
    {
      try
      {
        return PurchaseConditionOptionsAccessor.GetSubCategories(parentCategoryId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<PurchaseConditionCategory>) null;
      }
    }

    public virtual PurchaseConditionStatus GetStatus(string statusName)
    {
      try
      {
        return PurchaseConditionOptionsAccessor.GetStatus(statusName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (PurchaseConditionStatus) null;
      }
    }

    public virtual List<PurchaseConditionStatus> GetAllStatuses()
    {
      try
      {
        return PurchaseConditionOptionsAccessor.GetAllStatuses();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<PurchaseConditionStatus>) null;
      }
    }

    public virtual int CreatePurchaseConditionCategory(
      PurchaseConditionCategory purchaseConditionCategory)
    {
      try
      {
        return PurchaseConditionOptionsAccessor.CreatePurchaseConditionCategory(purchaseConditionCategory);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual string GetFieldsUsedInRules(FieldSearchRuleType[] ruleTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFieldsUsedInRules), new object[1]
      {
        (object) ruleTypes
      });
      try
      {
        return FieldSearchDbAccessor.GetFields(ruleTypes)?.ToUpper();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (string) null;
    }

    public virtual DDMAffectedFieldsandDataTableNames GetFieldsAndDataTableNamesUsedInRules(
      FieldSearchRuleType[] ruleTypes)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetFieldsAndDataTableNamesUsedInRules), new object[1]
      {
        (object) ruleTypes
      });
      try
      {
        DDMAffectedFieldsandDataTableNames andDataTableNames = FieldSearchDbAccessor.GetFieldsAndDataTableNames(ruleTypes);
        andDataTableNames.fields = andDataTableNames.fields != null ? andDataTableNames.fields.ToUpper() : (string) null;
        return andDataTableNames;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
      return (DDMAffectedFieldsandDataTableNames) null;
    }

    public virtual bool ExistsConcurrentUserLogin(string userID, string appName, string sessionID = "")
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ExistsConcurrentUserLogin), new object[3]
      {
        (object) userID,
        (object) appName,
        (object) sessionID
      });
      try
      {
        return LoginManager.ExistsConcurrentUserLogin(userID, appName, sessionID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void SaveTitleFeeCredentials(
      string orderUID,
      string loanGUID,
      string credentials)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (SaveTitleFeeCredentials), new object[3]
      {
        (object) orderUID,
        (object) loanGUID,
        (object) credentials
      });
      try
      {
        TitleFeeCredentialsAccessor.SaveTitleFeeCredentials(orderUID, loanGUID, credentials);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetTitleFeeCredentials(string[] orderUIDs, string loanGUID)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTitleFeeCredentials), new object[2]
      {
        (object) orderUIDs,
        (object) loanGUID
      });
      try
      {
        if (orderUIDs == null)
          return (string[]) null;
        return orderUIDs.Length == 0 ? new string[0] : TitleFeeCredentialsAccessor.GetTitleFeeCredentials(orderUIDs, loanGUID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual IList<ExternalOrgLenderContact> GetGlobalLenderContacts()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetGlobalLenderContacts), Array.Empty<object>());
      try
      {
        return TPOCompanyContactsAccessor.GetGlobalLenderContacts();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<ExternalOrgLenderContact>) null;
      }
    }

    public virtual IList<ExternalOrgLenderContact> GetGlobalLenderContacts(int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetGlobalLenderContacts), Array.Empty<object>());
      try
      {
        return TPOCompanyContactsAccessor.GetGlobalLenderContacts(new int?(externalOrgId));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<ExternalOrgLenderContact>) null;
      }
    }

    public virtual IList<ExternalOrgLenderContact> GetLenderContacts(int externalOrgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLenderContacts), Array.Empty<object>());
      try
      {
        return TPOCompanyContactsAccessor.GetLenderContacts(externalOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<ExternalOrgLenderContact>) null;
      }
    }

    public virtual int AddLenderContact(ExternalOrgLenderContact newContact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddLenderContact), Array.Empty<object>());
      try
      {
        return TPOCompanyContactsAccessor.AddLenderContact(newContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual bool UpdateLenderContact(ExternalOrgLenderContact contact)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLenderContact), Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.UpdateLenderContact(contact);
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool UpdateLenderContacts(params ExternalOrgLenderContact[] contacts)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateLenderContacts), Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.UpdateLenderContacts(contacts);
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteLenderContact(int contactId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (DeleteLenderContact), Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.DeleteLenderContact(contactId);
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual IList<ExternalOrgLenderContact> GetTPOCompanyLenderContacts(int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTPOCompanyLenderContacts), Array.Empty<object>());
      try
      {
        return (IList<ExternalOrgLenderContact>) TPOCompanyContactsAccessor.GetTPOCompanyLenderContacts(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<ExternalOrgLenderContact>) null;
      }
    }

    public virtual void AddTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (AddTPOCompanyLenderContact), Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.AddTPOCompanyLenderContact(externalOrgId, contactID, source, hide, displayOrder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateTPOCompanyLenderContact), Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.UpdateTPOCompanyLenderContact(externalOrgId, contactID, source, hide, displayOrder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateTPOCompanyLenderContacts(
      int externalOrgId,
      params ExternalOrgLenderContact[] contacts)
    {
      this.onApiCalled(nameof (ConfigurationManager), "UpdateTPOCompanyLenderContact", Array.Empty<object>());
      try
      {
        TPOCompanyContactsAccessor.UpdateTPOCompanyLenderContacts(externalOrgId, (IList<ExternalOrgLenderContact>) contacts);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetThinClientURL() => System.Configuration.ConfigurationManager.AppSettings["ThinClient.Url"];

    public virtual string GetBidTapeThinClientURL()
    {
      return System.Configuration.ConfigurationManager.AppSettings["ThinClientBidTape.Url"];
    }

    public virtual string GetServicesManagementThinClientURL(string url)
    {
      return System.Configuration.ConfigurationManager.AppSettings[url];
    }

    public virtual IList<BidTapeField> GetBidTapeFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBidTapeFields), Array.Empty<object>());
      try
      {
        return BidTapeTemplateAccessor.GetBidTapeFields();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<BidTapeField>) null;
      }
    }

    public virtual void UpdateBidTapeFields(IList<BidTapeField> fields)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateBidTapeFields), Array.Empty<object>());
      try
      {
        BidTapeTemplateAccessor.UpdateBidTapeFields(fields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<string, string> GetTpoLoanOfficer(string orgId, int realWorldRoleId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTpoLoanOfficer), Array.Empty<object>());
      try
      {
        return TPOCompanyContactsAccessor.GetTpoLoanOfficer(orgId, realWorldRoleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, string>) null;
      }
    }

    public virtual void UpdateBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateBestEffortDailyLimit), Array.Empty<object>());
      try
      {
        ExternalOrgManagementAccessor.UpdateBestEffortDailyLimit(entityId, lockDate, loanAmount);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount,
      string loanGuid,
      string rateSheetId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateBestEffortDailyLimit), Array.Empty<object>());
      try
      {
        ExternalOrgManagementAccessor.UpdateBestEffortDailyLimit(entityId, lockDate, loanAmount, loanGuid, rateSheetId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual double GetBestEffortDailyLimit(string entityId, DateTime lockDate)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBestEffortDailyLimit), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetBestEffortDailyLimit(entityId, lockDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0.0;
      }
    }

    public virtual double GetBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      string rateSheetId,
      string loanGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetBestEffortDailyLimit), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetBestEffortDailyLimit(entityId, lockDate, rateSheetId, loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return 0.0;
      }
    }

    public virtual void ResetBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount,
      string loanGuid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (ResetBestEffortDailyLimit), Array.Empty<object>());
      try
      {
        ExternalOrgManagementAccessor.ResetBestEffortDailyLimit(entityId, lockDate, loanAmount, loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExternalOrgCustomFields GetTpoCustomFields(int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetTpoCustomFields), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetTpoCustomFields(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgCustomFields) null;
      }
    }

    public virtual ExternalOrgManagementDataCount GetExternalOrganizationDataWithCountByOid(
      int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetExternalOrganizationDataWithCountByOid), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetExternalOrganizationDataWithCountByOid(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (ExternalOrgManagementDataCount) null;
      }
    }

    public virtual Dictionary<string, string> GetOrgTree(int orgId)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOrgTree), Array.Empty<object>());
      try
      {
        return ExternalOrgManagementAccessor.GetOrgTree(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, string>) null;
      }
    }

    public virtual void ChangeTradeStatus(bool isAllowPublishChecked)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (isAllowPublishChecked)
          dbQueryBuilder.Append("Update t\r\n                                     Set t.Status = 8\r\n                                     From Trades t inner join CorrespondentTrades as ct on t.TradeID = ct.TradeID\r\n                                     Where  (ct.LastPublishedDateTime IS NULL OR \r\n                                            LTRIM(RTRIM(ct.LastPublishedDateTime)) = '')");
        else
          dbQueryBuilder.Append("Update t\r\n                                     Set Status = 2 \r\n                                     From Trades t inner join CorrespondentTrades as ct on t.TradeID = ct.TradeID\r\n                                     Where Status = 8");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ChangeTradeStatus), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateEnhancedConditionSet(EnhancedConditionSet condset)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (UpdateEnhancedConditionSet), Array.Empty<object>());
      try
      {
        EnhancedConditionSetAccessor.UpdateConditionSets(condset);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<EnhancedConditionSet> GetEnhancedConditionSets()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEnhancedConditionSets), Array.Empty<object>());
      try
      {
        return EnhancedConditionSetAccessor.GetConditionSets();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<EnhancedConditionSet>) null;
      }
    }

    public virtual DataSet GetEnhanceConditionTemplateNameAndType(Guid setid)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEnhanceConditionTemplateNameAndType), Array.Empty<object>());
      try
      {
        return EnhancedConditionTemplateAccessor.GetTemplateNameAndType(setid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (DataSet) null;
      }
    }

    public virtual HashSet<string> GetOptionsFromConditionType(string attribute)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetOptionsFromConditionType), Array.Empty<object>());
      try
      {
        return EnhancedConditionTypeAccessor.GetOptionsFromConditionType(attribute);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (HashSet<string>) null;
      }
    }

    public virtual List<string> GetTrackingDefinitionsOfConditionType(string type)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetEnhancedConditionType", Array.Empty<object>());
      try
      {
        return EnhancedConditionTypeAccessor.GetTrackingDefinitionsOfConditionType(type);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual List<string> GetEnhancedConditionTypeList(bool active)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEnhancedConditionTypeList), Array.Empty<object>());
      try
      {
        return EnhancedConditionTypeAccessor.GetEnhancedConditionTypeList(active);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual EnhancedConditionSet GetEnhancedConditionSetDetail(
      Guid setid,
      bool active,
      bool detailincluded)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetEnhancedConditionSetDetail), Array.Empty<object>());
      try
      {
        return EnhancedConditionSetAccessor.GetEnhancedConditionSetDetail(setid, active, detailincluded);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (EnhancedConditionSet) null;
      }
    }

    public virtual bool IsUniqueSetName(string setname, Guid id)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (IsUniqueSetName), Array.Empty<object>());
      try
      {
        return EnhancedConditionSetAccessor.IsUniqueSetName(setname, id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual List<ePassCredential> GetePassCredential(string category)
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetePassCredential), Array.Empty<object>());
      try
      {
        return ePassPwdMmntAccessor.GetePassCredential(category);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<ePassCredential>();
      }
    }

    public virtual SkyDriveUrl GetSkyDriveUrl(string attachmentId)
    {
      this.onApiCalled(nameof (ConfigurationManager), "GetSkyDriveUrlForGet", new object[1]
      {
        (object) attachmentId
      });
      if ((attachmentId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (ConfigurationManager), (ServerException) new ServerArgumentException("Attachment ID cannot be blank or null", nameof (attachmentId), this.Session.SessionInfo));
      try
      {
        return SkyDriveViewer.GetSkyDriveViewerURL(attachmentId, this.Session);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (SkyDriveUrl) null;
      }
    }

    public virtual List<AutoRetrieveSettings> GetAutoRetrieveSettings()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetAutoRetrieveSettings), Array.Empty<object>());
      try
      {
        return EDisclosureConfigurationAccessor.GetAutoRetrieveSettings();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return new List<AutoRetrieveSettings>();
      }
    }

    public virtual IList<LockComparisonField> GetLockComparisonFields()
    {
      this.onApiCalled(nameof (ConfigurationManager), nameof (GetLockComparisonFields), Array.Empty<object>());
      try
      {
        return LockComparisonFields.GetLockComparisonFields();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ConfigurationManager), ex, this.Session.SessionInfo);
        return (IList<LockComparisonField>) null;
      }
    }
  }
}
