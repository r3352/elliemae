// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Bam
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.ePass.BamEnums;
using EllieMae.EMLite.ePass.BamObjects;
using EllieMae.EMLite.ePass.Browser;
using EllieMae.EMLite.ePass.eFolder;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.WebServices;
using EllieMae.EMLite.Workflow;
using EllieMae.EMLite.Xml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [Guid("32F6BF6F-42E1-3433-AF9F-CE6F14C7CE3B")]
  public class Bam : IBam
  {
    private static List<FileAttachment> _attachmentList;
    private static List<PendingAttachment> _pendingList;
    private static CompanyInfo _companyInfo;
    private const string className = "Bam";
    private string sw = Tracing.SwEpass;
    private bool debug = EnConfigurationSettings.GlobalSettings.Debug;
    private IBrowser browser;
    private LoanDataMgr loanDataMgr;
    private LoanData loanData;
    private XmlDocument xml;
    private string xmlBase;
    private EDisclosureSetup edisclosureSetup;
    private UserInfo[] userList;
    private OrgInfo[] orgList;
    private FieldSettings fieldSettings;
    public static List<EPPSLoanProgram> EppsLoanPrograms;

    public LoanDataMgr LoanDataMgr => this.loanDataMgr;

    public LoanData LoanData => this.loanData;

    public Bam()
      : this((IBrowser) null, Session.LoanDataMgr, Session.LoanData)
    {
    }

    public Bam(IBrowser browser)
      : this(browser, Session.LoanDataMgr, Session.LoanData)
    {
    }

    public Bam(IBrowser browser, LoanDataMgr loanDataMgr, LoanData loanData)
    {
      this.browser = browser;
      this.loanDataMgr = loanDataMgr;
      this.loanData = loanData;
      Bam._attachmentList = new List<FileAttachment>();
      Bam._pendingList = new List<PendingAttachment>();
      this.fieldSettings = Session.LoanManager.GetFieldSettings();
      if (Bam._companyInfo != null)
        return;
      Bam._companyInfo = Session.CompanyInfo;
      OrgInfo branchInfo = Session.StartupInfo.BranchInfo;
      if (branchInfo == null)
        return;
      Bam._companyInfo = new CompanyInfo(Bam._companyInfo.ClientID, branchInfo.CompanyName, branchInfo.CompanyAddress.Street1, branchInfo.CompanyAddress.City, branchInfo.CompanyAddress.State, branchInfo.CompanyAddress.Zip, branchInfo.CompanyPhone, branchInfo.CompanyFax, Bam._companyInfo.Password, new string[4]
      {
        branchInfo.DBAName1,
        branchInfo.DBAName2,
        branchInfo.DBAName3,
        branchInfo.DBAName4
      }, (BranchExtLicensing) branchInfo.OrgBranchLicensing.Clone());
    }

    public IEncompassBrowser CreateBrowser() => (IEncompassBrowser) EncompassWebBrowser.Create();

    public IEncompassBrowser CreateBrowser(IBrowserParams parameters)
    {
      return (IEncompassBrowser) EncompassWebBrowser.Create(parameters);
    }

    public string GetServiceUrl(string urlKey)
    {
      string empty = string.Empty;
      Session.SessionObjects?.StartupInfo?.ServiceUrls?.ToDictionary().TryGetValue(urlKey, out empty);
      return empty;
    }

    public Dictionary<string, string> GetServiceUrls()
    {
      return Session.SessionObjects?.StartupInfo?.ServiceUrls?.ToDictionary();
    }

    public string GetLoginID()
    {
      this.writeMethodCall(nameof (GetLoginID));
      return EpassLogin.LoginID;
    }

    public string GetUserID()
    {
      this.writeMethodCall(nameof (GetUserID));
      return Session.UserInfo.Userid;
    }

    public string GetUserFirstName()
    {
      this.writeMethodCall(nameof (GetUserFirstName));
      return Session.UserInfo.FirstName;
    }

    public string GetUserLastName()
    {
      this.writeMethodCall(nameof (GetUserLastName));
      return Session.UserInfo.LastName;
    }

    public string GetUserFullName()
    {
      this.writeMethodCall(nameof (GetUserFullName));
      return Session.UserInfo.FullName;
    }

    public string GetUserPassword()
    {
      this.writeMethodCall(nameof (GetUserPassword));
      if (!string.IsNullOrWhiteSpace(Session.Password))
        return Session.Password;
      ICache simpleCache = CacheManager.GetSimpleCache("SsoTokenCache");
      string key = Session.DefaultInstance.SessionID + "_Elli.Emn";
      string userPassword = simpleCache.Get(key) as string;
      if (!string.IsNullOrWhiteSpace(userPassword))
        return userPassword;
      string ssoToken = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
      {
        "Elli.Emn"
      }, 15);
      simpleCache.Put(key, new CacheItem((object) ssoToken));
      return ssoToken;
    }

    public string GetUserPersona()
    {
      this.writeMethodCall(nameof (GetUserPersona));
      return Session.GetEPassPersonaDescriptor();
    }

    public int[] GetUserPersonaList()
    {
      this.writeMethodCall(nameof (GetUserPersonaList));
      ArrayList arrayList = new ArrayList();
      foreach (Persona userPersona in Session.UserInfo.UserPersonas)
        arrayList.Add((object) userPersona.ID);
      return (int[]) arrayList.ToArray(typeof (int));
    }

    public string GetUserPhone()
    {
      this.writeMethodCall(nameof (GetUserPhone));
      return Session.UserInfo.Phone;
    }

    public string GetUserFax()
    {
      this.writeMethodCall(nameof (GetUserFax));
      return Session.UserInfo.Fax;
    }

    public string GetUserEmail()
    {
      this.writeMethodCall(nameof (GetUserEmail));
      return Session.UserInfo.Email;
    }

    public bool GetUserMyEpassCustomRight()
    {
      this.writeMethodCall(nameof (GetUserMyEpassCustomRight));
      throw new NotSupportedException();
    }

    public bool GetUserMyEpassCustomRight(string service)
    {
      this.writeMethodCall(nameof (GetUserMyEpassCustomRight), (object) service);
      ServicesAclManager aclManager = (ServicesAclManager) Session.ACL.GetAclManager(AclCategory.Services);
      switch (aclManager.GetServicesDefaultSetting())
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          return false;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          return true;
        default:
          return aclManager.HasServicePermission(service);
      }
    }

    public bool GetUserPlanCodeRight()
    {
      this.writeMethodCall(nameof (GetUserPlanCodeRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_Other_PlanCode);
    }

    public bool GetUserAppraisalMgtRight()
    {
      this.writeMethodCall(nameof (GetUserAppraisalMgtRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_AppraisalOrderManagement);
    }

    public bool GetUserAltLenderRight()
    {
      this.writeMethodCall(nameof (GetUserAltLenderRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_Other_AltLender);
    }

    public bool GetUserDeselectDisclosureRight()
    {
      this.writeMethodCall(nameof (GetUserDeselectDisclosureRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.eFolder_Other_eDisclosure_DeselectDocs);
    }

    public bool GetUserRequestEllieMaeNetworkServicesRight()
    {
      this.writeMethodCall(nameof (GetUserRequestEllieMaeNetworkServicesRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.eFolder_Other_RequestEllieMaeNetworkServices);
    }

    public bool GetUserOrderEncompassClosingDocsRight()
    {
      this.writeMethodCall(nameof (GetUserOrderEncompassClosingDocsRight));
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_EMClosingDocs_OrderDocs);
    }

    public string GetUserPaymentToken(string settingType)
    {
      this.writeMethodCall(nameof (GetUserPaymentToken), (object) settingType);
      return Session.Application.GetService<ILoanServices>().GetUserPaymentToken(settingType);
    }

    public string GetUserPaymentType(string settingType)
    {
      this.writeMethodCall(nameof (GetUserPaymentType), (object) settingType);
      return Session.Application.GetService<ILoanServices>().GetUserPaymentType(settingType);
    }

    public string GetCompanyClientID()
    {
      this.writeMethodCall(nameof (GetCompanyClientID));
      return Bam._companyInfo.ClientID;
    }

    public string GetCompanyName()
    {
      this.writeMethodCall(nameof (GetCompanyName));
      return Bam._companyInfo.Name;
    }

    public string GetCompanyAddressLine1()
    {
      this.writeMethodCall(nameof (GetCompanyAddressLine1));
      return Bam._companyInfo.Address;
    }

    public string GetCompanyAddressCity()
    {
      this.writeMethodCall(nameof (GetCompanyAddressCity));
      return Bam._companyInfo.City;
    }

    public string GetCompanyAddressState()
    {
      this.writeMethodCall(nameof (GetCompanyAddressState));
      return Bam._companyInfo.State;
    }

    public string GetCompanyAddressZip()
    {
      this.writeMethodCall(nameof (GetCompanyAddressZip));
      return Bam._companyInfo.Zip;
    }

    public string GetCompanyPhone()
    {
      this.writeMethodCall(nameof (GetCompanyPhone));
      return Bam._companyInfo.Phone;
    }

    public string GetCompanyFax()
    {
      this.writeMethodCall(nameof (GetCompanyFax));
      return Bam._companyInfo.Fax;
    }

    public int[] GetPersonaList()
    {
      this.writeMethodCall(nameof (GetPersonaList));
      ArrayList arrayList = new ArrayList();
      foreach (Persona allPersona in Session.PersonaManager.GetAllPersonas())
        arrayList.Add((object) allPersona.ID);
      return (int[]) arrayList.ToArray(typeof (int));
    }

    public string GetPersonaName(int personaID)
    {
      this.writeMethodCall("GetPersonaList");
      foreach (Persona allPersona in Session.PersonaManager.GetAllPersonas())
      {
        if (allPersona.ID == personaID)
          return allPersona.Name;
      }
      return (string) null;
    }

    public string GetTpoNumber(string userId)
    {
      this.writeMethodCall(nameof (GetTpoNumber), (object) userId);
      List<ePassCredentialSetting> credentialSettings = Session.ConfigurationManager.GetUserePassCredentialSettings("'" + userId + "'");
      if (credentialSettings == null)
        return string.Empty;
      foreach (ePassCredentialSetting credentialSetting in credentialSettings)
      {
        if (credentialSetting.PartnerSection == "LP2WAYUW")
          return credentialSetting.TPOFieldValue;
      }
      return string.Empty;
    }

    public string GetUserSetting(string section, string key)
    {
      this.writeMethodCall(nameof (GetUserSetting), (object) section, (object) key);
      return Session.GetPrivateProfileString(section, key);
    }

    public void SetUserSetting(string section, string key, string str)
    {
      this.writeMethodCall(nameof (SetUserSetting), (object) section, (object) key, (object) "<hidden>");
      Session.WritePrivateProfileString(section, key, str);
    }

    public string GetCompanySetting(string section, string key)
    {
      this.writeMethodCall(nameof (GetCompanySetting), (object) section, (object) key);
      if (!(key.ToUpper().Trim() == "USE.LOCK.REQUEST.FIELDS") || !(section.ToUpper().Trim() == "POLICIES"))
        return Session.ConfigurationManager.GetCompanySetting(section, key);
      ProductPricingUtils.SynchronizeProductPricingSettingsWithServer();
      return Session.StartupInfo.ProductPricingPartner == null || !Session.StartupInfo.ProductPricingPartner.ImportToLoan ? "y" : "n";
    }

    public void SetCompanySetting(string section, string key, string str)
    {
      this.writeMethodCall(nameof (SetCompanySetting), (object) section, (object) key, (object) "<hidden>");
      Session.ConfigurationManager.SetCompanySetting(section, key, str);
    }

    public string GetCustomFieldSettings()
    {
      if (this.loanDataMgr != null)
        return this.loanDataMgr.SystemConfiguration.CustomFields.ToXML();
      string empty = string.Empty;
      try
      {
        return Session.ConfigurationManager.GetLoanCustomFields().ToXML();
      }
      catch
      {
        return string.Empty;
      }
    }

    public string GetTempFolder()
    {
      this.writeMethodCall(nameof (GetTempFolder));
      return SystemSettings.TempFolderRoot;
    }

    public string[] GetGlobalFiles()
    {
      this.writeMethodCall(nameof (GetGlobalFiles));
      return Session.ConfigurationManager.GetSystemSettingsNames();
    }

    public string LoadGlobalFile(string filename)
    {
      this.writeMethodCall(nameof (LoadGlobalFile), (object) filename);
      return Session.ConfigurationManager.GetSystemSettings(filename)?.ToString(Encoding.ASCII);
    }

    public void SaveGlobalFile(string filename, string fileContent)
    {
      this.writeMethodCall(nameof (SaveGlobalFile), (object) filename, (object) fileContent);
      BinaryObject data = new BinaryObject(fileContent, Encoding.ASCII);
      Session.ConfigurationManager.SaveSystemSettings(filename, data);
    }

    public string[] GetUserFiles()
    {
      this.writeMethodCall(nameof (GetUserFiles));
      return Session.User.GetUserSettingsNames();
    }

    public string LoadUserFile(string filename)
    {
      this.writeMethodCall(nameof (LoadUserFile), (object) filename);
      return Session.User.GetUserSettings(filename)?.ToString(Encoding.ASCII);
    }

    public void SaveUserFile(string filename, string fileContent)
    {
      this.writeMethodCall(nameof (SaveUserFile), (object) filename, (object) fileContent);
      BinaryObject data = new BinaryObject(fileContent, Encoding.ASCII);
      Session.User.SaveUserSettings(filename, data);
    }

    public string[] GetLoanFolders()
    {
      this.writeMethodCall(nameof (GetLoanFolders));
      return Session.LoanManager.GetAllLoanFolderNames(false);
    }

    public string[] GetLoanNames(string loanFolder)
    {
      this.writeMethodCall(nameof (GetLoanNames), (object) loanFolder);
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(loanFolder, new string[1]
      {
        "LoanName"
      }, PipelineData.Fields, false);
      string[] loanNames = new string[pipeline.Length];
      for (int index = 0; index < pipeline.Length; ++index)
        loanNames[index] = pipeline[index].LoanName;
      return loanNames;
    }

    public static string QueryLoansForXmlTest(
      SessionObjects objs,
      string fieldsToRetrieveXml,
      string queryXml)
    {
      XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) new StringReader(fieldsToRetrieveXml)).CreateNavigator().Select("/fieldsToRetrieve/field/fieldId");
      List<string> stringList = new List<string>();
      while (xpathNodeIterator.MoveNext())
        stringList.Add(xpathNodeIterator.Current.Value);
      string[] array = stringList.ToArray();
      ReportSettings reportSettings = new ReportSettings("HMDA fields", fieldsToRetrieveXml);
      QueryCriterion filter = (QueryCriterion) null;
      if ((queryXml ?? "") != "")
        filter = FieldFilterList.Parse(queryXml).CreateEvaluator().ToQueryCriterion();
      DataQuery query = new DataQuery((IEnumerable) array, filter);
      QueryResult queryResult = objs.LoanManager.QueryPipeline(query, false);
      return queryResult.RecordCount == 0 ? (string) null : Bam.convertDataTableToXml(queryResult.ToDataTable());
    }

    public static object[][] QueryLoansTest(
      SessionObjects objs,
      string[] fieldsToRetrieve,
      string queryXml)
    {
      QueryCriterion filter = (QueryCriterion) null;
      if ((queryXml ?? "") != "")
        filter = FieldFilterList.Parse(queryXml).CreateEvaluator().ToQueryCriterion();
      DataQuery query = new DataQuery((IEnumerable) fieldsToRetrieve, filter);
      QueryResult queryResult = objs.LoanManager.QueryPipeline(query, false);
      return queryResult.RecordCount == 0 ? new object[0][] : queryResult.GetRows(0, queryResult.RecordCount);
    }

    public static string FieldsNotInRDB(SessionObjects objs, string fieldsXml)
    {
      XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) new StringReader(fieldsXml)).CreateNavigator().Select("/fieldsToRetrieve/field/fieldId");
      List<string> stringList1 = new List<string>();
      HashSet<string> stringSet = new HashSet<string>();
      List<string> stringList2 = new List<string>();
      while (xpathNodeIterator.MoveNext())
        stringList1.Add(xpathNodeIterator.Current.Value);
      LoanXDBTableList loanXdbTableList = objs.LoanManager.GetLoanXDBTableList(false);
      for (int i1 = 0; i1 < loanXdbTableList.TableCount; ++i1)
      {
        LoanXDBTable tableAt = loanXdbTableList.GetTableAt(i1);
        if (tableAt != null)
        {
          for (int i2 = 0; i2 < tableAt.FieldCount; ++i2)
          {
            LoanXDBField fieldAt = tableAt.GetFieldAt(i2);
            if (fieldAt != null)
              stringSet.Add(fieldAt.FieldID);
          }
        }
      }
      foreach (string str in stringList1)
      {
        if (!stringSet.Contains(str))
          stringList2.Add(str);
      }
      if (stringList2.Count <= 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder("<Root>");
      foreach (string str in stringList2)
        stringBuilder.Append("<field>" + str + "</field>");
      stringBuilder.Append("</Root>");
      return stringBuilder.ToString();
    }

    public object[][] QueryLoans(string[] fieldsToRetrieve, string queryXml)
    {
      this.writeMethodCall(nameof (QueryLoans), (object) fieldsToRetrieve, (object) queryXml);
      QueryCriterion filter = (QueryCriterion) null;
      if ((queryXml ?? "") != "")
        filter = FieldFilterList.Parse(queryXml).CreateEvaluator().ToQueryCriterion();
      QueryResult queryResult = Session.LoanManager.QueryPipeline(new DataQuery((IEnumerable) fieldsToRetrieve, filter), false);
      return queryResult.RecordCount == 0 ? new object[0][] : queryResult.GetRows(0, queryResult.RecordCount);
    }

    public bool RDBUpdatePendingStatus()
    {
      return Session.LoanManager.GetLoanXDBStatus(false).UpdatesPending;
    }

    public string FieldsNotInRDB(string fieldsXml)
    {
      XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) new StringReader(fieldsXml)).CreateNavigator().Select("/fieldsToRetrieve/field/fieldId");
      List<string> stringList1 = new List<string>();
      HashSet<string> stringSet = new HashSet<string>();
      List<string> stringList2 = new List<string>();
      while (xpathNodeIterator.MoveNext())
        stringList1.Add(xpathNodeIterator.Current.Value.Replace("Fields.", ""));
      LoanXDBTableList loanXdbTableList = Session.LoanManager.GetLoanXDBTableList(false);
      for (int i1 = 0; i1 < loanXdbTableList.TableCount; ++i1)
      {
        LoanXDBTable tableAt = loanXdbTableList.GetTableAt(i1);
        if (tableAt != null)
        {
          for (int i2 = 0; i2 < tableAt.FieldCount; ++i2)
          {
            LoanXDBField fieldAt = tableAt.GetFieldAt(i2);
            if (fieldAt != null)
            {
              if (fieldAt.ComortgagorPair > 1)
                stringSet.Add(fieldAt.FieldID + "#" + (object) fieldAt.ComortgagorPair);
              else
                stringSet.Add(fieldAt.FieldID);
            }
          }
        }
      }
      foreach (string str in stringList1)
      {
        if (!stringSet.Contains(str))
          stringList2.Add(str);
      }
      if (stringList2.Count <= 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder("<Root>");
      foreach (string str in stringList2)
        stringBuilder.Append("<field>" + str + "</field>");
      stringBuilder.Append("</Root>");
      return stringBuilder.ToString();
    }

    private static string getFieldID(LoanXDBField dbField)
    {
      string fieldId = "";
      string description = dbField.Description;
      FieldSettings fieldSettings = Session.LoanManager.GetFieldSettings();
      FieldDefinition field = EncompassFields.GetField(dbField.FieldID, fieldSettings);
      if (field != null)
        fieldId = field.FieldID;
      return fieldId;
    }

    public string QueryLoansForXml(string fieldsToRetrieveXml, string queryXml)
    {
      this.writeMethodCall(nameof (QueryLoansForXml), (object) fieldsToRetrieveXml, (object) queryXml);
      XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) new StringReader(fieldsToRetrieveXml)).CreateNavigator().Select("/fieldsToRetrieve/field/fieldId");
      List<string> stringList = new List<string>();
      while (xpathNodeIterator.MoveNext())
        stringList.Add(xpathNodeIterator.Current.Value);
      string[] array = stringList.ToArray();
      ReportSettings reportSettings = new ReportSettings("HMDA fields", fieldsToRetrieveXml);
      QueryCriterion filter = (QueryCriterion) null;
      if ((queryXml ?? "") != "")
        filter = FieldFilterList.Parse(queryXml).CreateEvaluator().ToQueryCriterion();
      QueryResult queryResult = Session.LoanManager.QueryPipeline(new DataQuery((IEnumerable) array, filter), false);
      return queryResult.RecordCount == 0 ? (string) null : Bam.convertDataTableToXml(queryResult.ToDataTable());
    }

    public static string convertDataTableToXml(DataTable dt)
    {
      StringWriter writer = new StringWriter();
      dt.TableName = "Loan";
      dt.WriteXml((TextWriter) writer, true);
      return writer.ToString();
    }

    public string[] QueryLoanGuids(string queryXml)
    {
      this.writeMethodCall(nameof (QueryLoanGuids), (object) queryXml);
      object[][] objArray = this.QueryLoans(new string[1]
      {
        "Loan.Guid"
      }, queryXml);
      string[] strArray = new string[objArray.GetLength(0)];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = string.Concat(objArray[index][0]);
      return strArray;
    }

    public string[] QueryLoanNames(string queryXml)
    {
      this.writeMethodCall(nameof (QueryLoanNames), (object) queryXml);
      object[][] objArray = this.QueryLoans(new string[1]
      {
        "Loan.LoanName"
      }, queryXml);
      string[] strArray = new string[objArray.GetLength(0)];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = string.Concat(objArray[index][0]);
      return strArray;
    }

    public void CloseLoan()
    {
      this.writeMethodCall(nameof (CloseLoan));
      if (this.loanDataMgr != null)
      {
        string str = (string) null;
        if (Session.LoanData != null)
          str = Session.LoanData.GUID;
        if (this.loanData.GUID != str)
          this.loanDataMgr.Close();
      }
      this.loanDataMgr = Session.LoanDataMgr;
      this.loanData = Session.LoanData;
      Bam._attachmentList.Clear();
      Bam._pendingList.Clear();
    }

    public void CreateLoan(string loanName)
    {
      this.writeMethodCall(nameof (CreateLoan), (object) loanName);
      this.CloseLoan();
      this.loanDataMgr = LoanDataMgr.NewLoan(Session.SessionObjects, Session.UserInfo.WorkingFolder, loanName);
      this.loanData = this.loanDataMgr.LoanData;
    }

    public bool CreateLoanFromDUFile(string filepath)
    {
      this.writeMethodCall(nameof (CreateLoanFromDUFile), (object) filepath);
      this.CloseLoan();
      using (ImportFannie2 importFannie2 = new ImportFannie2(filepath))
      {
        if (importFannie2.ShowDialog() == DialogResult.Cancel)
          return false;
        this.loanDataMgr = importFannie2.LoanDataMgr;
        this.loanData = this.loanDataMgr.LoanData;
      }
      return true;
    }

    public void ExportLoan(string zipFile)
    {
      this.writeMethodCall(nameof (ExportLoan), (object) zipFile);
      this.loanDataMgr.ExportLoan(zipFile);
    }

    public string GetLoanFolder()
    {
      this.writeMethodCall(nameof (GetLoanFolder));
      return this.loanDataMgr.LoanFolder;
    }

    public string GetLoanName()
    {
      this.writeMethodCall(nameof (GetLoanName));
      return this.loanDataMgr.LoanName;
    }

    public void ImportLoan(string zipFile)
    {
      this.writeMethodCall(nameof (ImportLoan), (object) zipFile);
      this.loanDataMgr.ImportLoan(zipFile, false);
      this.loanData = this.loanDataMgr.LoanData;
      string str = (string) null;
      if (Session.LoanData != null)
        str = Session.LoanData.GUID;
      if (!(this.loanData.GUID == str))
        return;
      Session.Application.GetService<ILoanEditor>().RefreshLoan();
    }

    public bool LoanExists(string guid)
    {
      this.writeMethodCall(nameof (LoanExists), (object) guid);
      return Session.LoanManager.LoanExists(guid);
    }

    public bool LoadLoan(string guid)
    {
      this.writeMethodCall(nameof (LoadLoan), (object) guid);
      try
      {
        this.LoadLoan(guid, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to load loan: " + (object) ex);
        return false;
      }
    }

    public void LoadLoan(string guid, bool readOnly)
    {
      this.writeMethodCall(nameof (LoadLoan), (object) guid, (object) readOnly);
      this.CloseLoan();
      string str = (string) null;
      if (Session.LoanData != null)
        str = Session.LoanData.GUID;
      if (str != guid)
      {
        this.loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, guid, false);
        this.loanData = this.loanDataMgr.LoanData;
      }
      if (readOnly || this.loanDataMgr.Writable)
        return;
      this.loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
    }

    public bool LoadLoan(string loanFolder, string loanName)
    {
      this.writeMethodCall(nameof (LoadLoan), (object) loanFolder, (object) loanName);
      try
      {
        this.LoadLoan(loanFolder, loanName, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to load loan: " + (object) ex);
        return false;
      }
    }

    public void LoadLoan(string loanFolder, string loanName, bool readOnly)
    {
      this.writeMethodCall(nameof (LoadLoan), (object) loanFolder, (object) loanName, (object) readOnly);
      this.CloseLoan();
      string str1 = (string) null;
      string str2 = (string) null;
      if (Session.LoanDataMgr != null)
      {
        str1 = Session.LoanDataMgr.LoanFolder;
        str2 = Session.LoanDataMgr.LoanName;
      }
      if (str1 != loanFolder || str2 != loanName)
      {
        this.loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, loanFolder, loanName, false);
        this.loanData = this.loanDataMgr.LoanData;
      }
      if (readOnly || this.loanDataMgr.Writable)
        return;
      this.loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
    }

    public bool LockLoan()
    {
      this.writeMethodCall(nameof (LockLoan));
      try
      {
        if (!this.loanDataMgr.Writable)
          this.loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unabled to lock the loan: " + (object) ex);
        return false;
      }
    }

    public void SaveLoan()
    {
      this.writeMethodCall(nameof (SaveLoan));
      Bam.LinkAttachments(this.loanDataMgr, (DocumentLog) null);
      this.loanDataMgr.Save(false);
    }

    public void refreshLoanFromServer()
    {
      this.writeMethodCall(nameof (refreshLoanFromServer));
      this.loanDataMgr.refreshLoanFromServer(true);
      if (this.loanData.LinkedData != null)
      {
        this.LoanDataMgr.LinkedLoan.refreshLoanFromServer();
        bool skipLockRequestSync1 = this.loanData.Calculator.SkipLockRequestSync;
        bool skipLockRequestSync2 = this.LoanDataMgr.LinkedLoan.Calculator.SkipLockRequestSync;
        this.loanData.Calculator.SkipLockRequestSync = true;
        this.LoanDataMgr.LinkedLoan.Calculator.SkipLockRequestSync = true;
        this.loanData.SyncPiggyBackFiles(this.loanDataMgr.SystemConfiguration.PiggybackSyncFields.GetSyncFields(), true, true, (string) null, (string) null, false);
        this.loanData.Calculator.SkipLockRequestSync = skipLockRequestSync1;
        this.loanDataMgr.LinkedLoan.Save(true);
        this.LoanDataMgr.LinkedLoan.Calculator.SkipLockRequestSync = skipLockRequestSync2;
      }
      this.loanDataMgr.HandleEPCIntegrationClose();
    }

    private string liabilityFileNew() => "liability-" + this.loanData.PairId + ".xml";

    private string liabilityFileOld() => "liability-" + this.loanData.PairId.Substring(1) + ".xml";

    private string reportFileNew(string vendor, Bam.FileType fileType)
    {
      switch (fileType)
      {
        case Bam.FileType.INFILE_PDF:
          return vendor + "-credit-" + this.loanData.PairId + ".pdf";
        case Bam.FileType.INFILE_HTML:
          return vendor + "-credit-" + this.loanData.PairId + ".htm";
        case Bam.FileType.FLOOD_PDF:
          return vendor + "-flood.pdf";
        case Bam.FileType.FLOOD_HTML:
          return vendor + "-flood.htm";
        case Bam.FileType.RMCR_PDF:
          return vendor + "-rmcr-" + this.loanData.PairId + ".pdf";
        case Bam.FileType.RMCR_HTML:
          return vendor + "-rmcr-" + this.loanData.PairId + ".htm";
        default:
          return vendor + "-unknown.type";
      }
    }

    private string reportFileOld(string vendor, Bam.FileType fileType)
    {
      switch (fileType)
      {
        case Bam.FileType.INFILE_PDF:
          return vendor + "-credit-" + this.loanData.PairId.Substring(1) + ".pdf";
        case Bam.FileType.INFILE_HTML:
          return vendor + "-credit-" + this.loanData.PairId.Substring(1) + ".htm";
        case Bam.FileType.FLOOD_PDF:
          return vendor + "-flood.pdf";
        case Bam.FileType.FLOOD_HTML:
          return vendor + "-flood.htm";
        case Bam.FileType.RMCR_PDF:
          return vendor + "-rmcr-" + this.loanData.PairId.Substring(1) + ".pdf";
        case Bam.FileType.RMCR_HTML:
          return vendor + "-rmcr-" + this.loanData.PairId.Substring(1) + ".htm";
        default:
          return vendor + "-unknown.type";
      }
    }

    private void saveAttachment(string filename, string title, BinaryObject data)
    {
      string lower = new FileInfo(filename).Extension.ToLower();
      if (!(lower == ".pdf") && !(lower == ".doc") && !(lower == ".txt") && !(lower == ".htm") && !(lower == ".findingshtml") && !(lower == ".creditprintfile"))
        return;
      try
      {
        data = new BinaryObject(Convert.FromBase64String(data.ToString()));
      }
      catch
      {
      }
      string tempFileName = SystemSettings.GetTempFileName(filename);
      data.Write(tempFileName);
      this.AddAttachment(tempFileName, title);
    }

    public string GetReportFileTypes(string vendor)
    {
      this.writeMethodCall(nameof (GetReportFileTypes), (object) vendor);
      string reportFileTypes = string.Empty;
      for (int index = 0; index < 6; ++index)
      {
        bool flag = this.loanDataMgr.SupportingDataExists(this.reportFileNew(vendor, (Bam.FileType) index));
        if (!flag)
          flag = this.loanDataMgr.SupportingDataExists(this.reportFileOld(vendor, (Bam.FileType) index));
        if (flag)
          reportFileTypes = !(reportFileTypes != string.Empty) ? index.ToString() : reportFileTypes + "," + index.ToString();
      }
      return reportFileTypes;
    }

    public byte[] LoadBinary(string filename)
    {
      this.writeMethodCall(nameof (LoadBinary), (object) filename);
      if (this.loanDataMgr.FileAttachments[filename] is ImageAttachment fileAttachment)
        return File.ReadAllBytes(this.loanDataMgr.FileAttachments.CreatePdf(fileAttachment));
      using (BinaryObject supportingData = this.loanDataMgr.GetSupportingData(filename))
        return supportingData?.GetBytes();
    }

    public string LoadFile(string filename)
    {
      this.writeMethodCall(nameof (LoadFile), (object) filename);
      return this.loanDataMgr.GetSupportingData(filename)?.ToString();
    }

    public string LoadDo()
    {
      this.writeMethodCall(nameof (LoadDo));
      return this.LoadFile("fannie_mae-do.xml");
    }

    public string LoadFannieLiability(string vendor)
    {
      this.writeMethodCall(nameof (LoadFannieLiability), (object) vendor);
      return this.LoadFile(vendor + "-fannie_lib.txt");
    }

    public string LoadLiability()
    {
      this.writeMethodCall(nameof (LoadLiability));
      return this.LoadFile(this.liabilityFileNew()) ?? this.LoadFile(this.liabilityFileOld());
    }

    public string LoadMbaLiability(string vendor)
    {
      this.writeMethodCall(nameof (LoadMbaLiability), (object) vendor);
      return this.LoadFile(vendor + "-mba_lib.txt");
    }

    public string LoadReport(string vendor, Bam.FileType fileType)
    {
      this.writeMethodCall(nameof (LoadReport), (object) vendor, (object) fileType);
      return this.LoadFile(this.reportFileNew(vendor, fileType)) ?? this.LoadFile(this.reportFileOld(vendor, fileType));
    }

    public string LoadXdt(string vendor)
    {
      this.writeMethodCall(nameof (LoadXdt), (object) vendor);
      return this.LoadFile(vendor + "-xdt.xml");
    }

    public void SaveBinary(string filename, byte[] content)
    {
      this.writeMethodCall(nameof (SaveBinary), (object) filename, (object) content);
      this.SaveBinary(filename, "Untitled", content);
    }

    public void SaveBinary(string filename, string title, byte[] content)
    {
      this.writeMethodCall(nameof (SaveBinary), (object) filename, (object) title, (object) content);
      BinaryObject data = (BinaryObject) null;
      if (content != null)
        data = new BinaryObject(content);
      this.loanDataMgr.SaveSupportingData(filename, data);
      if (data == null)
        return;
      this.saveAttachment(filename, title, data);
    }

    public void SaveFile(string filename, string fileContent)
    {
      this.writeMethodCall(nameof (SaveFile), (object) filename, (object) fileContent);
      this.SaveFile(filename, "Untitled", fileContent);
    }

    public void SaveFile(string filename, string title, string fileContent)
    {
      this.writeMethodCall(nameof (SaveFile), (object) filename, (object) title, (object) fileContent);
      BinaryObject data = (BinaryObject) null;
      if (fileContent != null)
        data = new BinaryObject(fileContent, Encoding.ASCII);
      this.loanDataMgr.SaveSupportingData(filename, data);
      if (data == null || data.Length == 0L)
        return;
      this.saveAttachment(filename, title, data);
    }

    public void SaveDo(string xmlStr)
    {
      this.writeMethodCall(nameof (SaveDo), (object) xmlStr);
      this.SaveFile("fannie_mae-do.xml", xmlStr);
    }

    public void SaveFannieLiability(string vendor, string xmlStr)
    {
      this.writeMethodCall(nameof (SaveFannieLiability), (object) vendor, (object) xmlStr);
      this.SaveFile(vendor + "-fannie_lib.txt", xmlStr);
    }

    public void SaveLiability(string xmlStr)
    {
      this.writeMethodCall(nameof (SaveLiability), (object) xmlStr);
      this.SaveFile(this.liabilityFileNew(), xmlStr);
    }

    public void SaveMbaLiability(string vendor, string xmlStr)
    {
      this.writeMethodCall(nameof (SaveMbaLiability), (object) vendor, (object) xmlStr);
      this.SaveFile(vendor + "-mba_lib.txt", xmlStr);
    }

    public void SaveReport(string vendor, Bam.FileType fileType, string fileContent)
    {
      this.writeMethodCall(nameof (SaveReport), (object) vendor, (object) fileType, (object) fileContent);
      this.SaveFile(this.reportFileNew(vendor, fileType), fileContent);
    }

    public void SaveXdt(string vendor, string xmlStr)
    {
      this.writeMethodCall(nameof (SaveXdt), (object) xmlStr);
      this.SaveFile(vendor + "-xdt.xml", xmlStr);
    }

    public int GetNumberOfDeposits()
    {
      this.writeMethodCall(nameof (GetNumberOfDeposits));
      return this.loanData.GetNumberOfDeposits();
    }

    public int GetNumberOfEmployer(bool borrower)
    {
      this.writeMethodCall(nameof (GetNumberOfEmployer), (object) borrower);
      return this.loanData.GetNumberOfEmployer(borrower);
    }

    public int GetNumberOfLiabilitesExlcudingAlimonyJobExp()
    {
      this.writeMethodCall(nameof (GetNumberOfLiabilitesExlcudingAlimonyJobExp));
      return this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
    }

    public int GetNumberOfMortgages()
    {
      this.writeMethodCall(nameof (GetNumberOfMortgages));
      return this.loanData.GetNumberOfMortgages();
    }

    public int GetNumberOfResidence(bool borrower)
    {
      this.writeMethodCall(nameof (GetNumberOfResidence), (object) borrower);
      return this.loanData.GetNumberOfResidence(borrower);
    }

    public int GetNumberOf4506()
    {
      this.writeMethodCall(nameof (GetNumberOf4506));
      return this.loanData.GetNumberOfTAX4506Ts(true);
    }

    public int GetNumberOf4506T()
    {
      this.writeMethodCall(nameof (GetNumberOf4506T));
      return this.loanData.GetNumberOfTAX4506Ts(false);
    }

    public int GetNumberOfBorrowerPairs()
    {
      this.writeMethodCall(nameof (GetNumberOfBorrowerPairs));
      return this.loanData.GetNumberOfBorrowerPairs();
    }

    public int GetNumberOfSettlementServiceProviders()
    {
      this.writeMethodCall(nameof (GetNumberOfSettlementServiceProviders));
      return this.loanData.GetNumberOfSettlementServiceProviders();
    }

    public string GetPairID()
    {
      this.writeMethodCall(nameof (GetPairID));
      return this.loanData.PairId;
    }

    public string GetSimpleField(string id)
    {
      this.writeMethodCall(nameof (GetSimpleField), (object) id);
      return this.loanData == null ? "" : this.loanData.GetSimpleField(id);
    }

    public string GetSimpleField(string id, int pairIndex)
    {
      this.writeMethodCall(nameof (GetSimpleField), (object) id, (object) pairIndex);
      return this.loanData.GetSimpleField(id, pairIndex);
    }

    public int NewLiability()
    {
      this.writeMethodCall(nameof (NewLiability));
      return this.loanData.NewLiability();
    }

    public void RemoveLiabilityAt(int block)
    {
      this.writeMethodCall(nameof (RemoveLiabilityAt), (object) block);
      this.loanData.RemoveLiabilityAt(block);
    }

    public void SetBorrowerPair(int pairIndex)
    {
      this.writeMethodCall(nameof (SetBorrowerPair), (object) pairIndex);
      this.loanData.SetBorrowerPair(pairIndex);
    }

    public int GetBorrowerPair()
    {
      this.writeMethodCall(nameof (GetBorrowerPair));
      return this.loanData.GetPairIndex(this.loanData.PairId);
    }

    public void SetCurrentField(string id, string val)
    {
      this.writeMethodCall(nameof (SetCurrentField), (object) id, (object) val);
      if (Session.LoanDataMgr != null)
        Session.LoanDataMgr.ValidationsEnabled = false;
      try
      {
        this.loanData.SetCurrentField(id, val);
      }
      finally
      {
        if (Session.LoanDataMgr != null)
          Session.LoanDataMgr.ValidationsEnabled = true;
      }
    }

    public void SetField(string id, string val)
    {
      this.writeMethodCall(nameof (SetField), (object) id, (object) val);
      if (Session.LoanDataMgr != null)
        Session.LoanDataMgr.ValidationsEnabled = false;
      try
      {
        if (id == "2942" && Utils.CheckIfURLA2020(this.loanData.GetField("1825")))
        {
          if (string.Compare(this.loanData.GetField(id), val, true) == 0)
            return;
          this.loanData.SetField(id, val);
          this.loanData.SetField("4516", val);
          this.loanData.SetField("4517", "");
          this.loanData.SetField("4518", "");
        }
        else
          this.loanData.SetField(id, val);
      }
      finally
      {
        if (Session.LoanDataMgr != null)
          Session.LoanDataMgr.ValidationsEnabled = true;
      }
    }

    public string GetLoanDataXML()
    {
      this.writeMethodCall(nameof (GetLoanDataXML));
      return this.loanData.ToXml();
    }

    public string GetLoanPaymentSchedule()
    {
      this.writeMethodCall(nameof (GetLoanPaymentSchedule));
      PaymentSchedule[] monthlyPayments;
      try
      {
        monthlyPayments = this.loanData.Calculator.GetPaymentSchedule(false).MonthlyPayments;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to load payment schedule: " + (object) ex);
        return (string) null;
      }
      this.xml = new XmlDocument();
      this.xml.LoadXml("<PaymentSchedule/>");
      int num1 = 0;
      foreach (PaymentSchedule paymentSchedule in monthlyPayments)
      {
        if (paymentSchedule != null)
        {
          ++num1;
          this.xmlBase = "PaymentSchedule/Payment[" + num1.ToString() + "]/";
          this.setValueText("@PaymentDate", paymentSchedule.PayDate);
          double num2 = paymentSchedule.CurrentRate;
          this.setValueText("@CurrentRate", num2.ToString());
          num2 = paymentSchedule.Principal;
          this.setValueText("@Principal", num2.ToString());
          num2 = paymentSchedule.Interest;
          this.setValueText("@Interest", num2.ToString());
          num2 = paymentSchedule.MortgageInsurance;
          this.setValueText("@MortgageInsurance", num2.ToString());
          num2 = paymentSchedule.Payment;
          this.setValueText("@Payment", num2.ToString());
          num2 = paymentSchedule.Balance;
          this.setValueText("@Balance", num2.ToString());
          num2 = paymentSchedule.BuydownSubsidyAmount;
          this.setValueText("@BuydownSubsidyAmount", num2.ToString());
          num2 = paymentSchedule.OriginalNoteRate;
          this.setValueText("@PaymentOriginalNoteRateDate", num2.ToString());
          num2 = paymentSchedule.BalanceForMI;
          this.setValueText("@BalanceForMI", num2.ToString());
          num2 = paymentSchedule.OriginalNoteRate;
          this.setValueText("@OriginalNoteRate", num2.ToString());
        }
      }
      return this.xml.OuterXml;
    }

    public void ExecuteCalculation(string calcName)
    {
      this.writeMethodCall(nameof (ExecuteCalculation), (object) calcName);
      this.loanDataMgr.ValidationsEnabled = false;
      try
      {
        this.loanData.Calculator.FormCalculation(calcName, (string) null, (string) null);
      }
      finally
      {
        this.loanDataMgr.ValidationsEnabled = true;
      }
    }

    public string ExportData(string format)
    {
      this.writeMethodCall(nameof (ExportData), (object) format);
      try
      {
        EllieMae.EMLite.Export.ExportData exportData = new EllieMae.EMLite.Export.ExportData(Session.LoanDataMgr, this.loanData);
        if (!format.Equals("Closing231", StringComparison.OrdinalIgnoreCase) && !format.Equals("Closing231.LockForm", StringComparison.OrdinalIgnoreCase))
          return exportData.Export(format);
        bool flag = false;
        try
        {
          if (string.Compare(this.loanData.GetField("2947"), "MHAdvantage", true) == 0)
          {
            flag = true;
            this.loanData.SetField("2947", "ManufacturedHousing");
          }
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(exportData.Export(format));
          XmlNode xmlNode = xmlDocument.SelectSingleNode("LOAN/_APPLICATION");
          XmlNode lastChild = xmlDocument.SelectSingleNode("LOAN/_APPLICATION").LastChild;
          string[] fields = Session.ConfigurationManager.GetLRAdditionalFields().GetFields(true);
          XmlElement element1 = xmlDocument.CreateElement("LockRequestAdditionalFields");
          FieldSettings fieldSettings = this.loanData.Settings.FieldSettings;
          for (int index1 = 0; index1 < fields.Length; ++index1)
          {
            FieldDefinition field = EncompassFields.GetField(fields[index1], fieldSettings);
            XmlElement element2 = xmlDocument.CreateElement("Field");
            XmlElement element3 = xmlDocument.CreateElement("FieldId");
            element3.InnerText = "LR." + fields[index1];
            element2.AppendChild((XmlNode) element3);
            XmlElement element4 = xmlDocument.CreateElement("DataType");
            element4.InnerText = field.Format.ToString();
            element2.AppendChild((XmlNode) element4);
            XmlElement element5 = xmlDocument.CreateElement("Value");
            if (this.loanData != null)
              element5.InnerText = this.loanData.GetField("LR." + fields[index1]);
            element2.AppendChild((XmlNode) element5);
            XmlElement element6 = xmlDocument.CreateElement("Description");
            element6.InnerText = field.Description;
            element2.AppendChild((XmlNode) element6);
            XmlElement element7 = xmlDocument.CreateElement("Options");
            string[] values = field.Options.GetValues();
            for (int index2 = 0; index2 < values.Length; ++index2)
            {
              XmlElement element8 = xmlDocument.CreateElement("Option");
              XmlAttribute attribute = xmlDocument.CreateAttribute("value");
              attribute.Value = values[index2];
              element8.Attributes.Append(attribute);
              element8.InnerText = field.Options[index2].Text;
              element7.AppendChild((XmlNode) element8);
            }
            element2.AppendChild((XmlNode) element7);
            element1.AppendChild((XmlNode) element2);
          }
          xmlNode.InsertAfter((XmlNode) element1, lastChild);
          return xmlDocument.OuterXml;
        }
        catch (Exception ex)
        {
          Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "ExportData: format = " + format + " ----- " + ex.Message);
          return string.Empty;
        }
        finally
        {
          if (flag)
            this.loanData.SetField("2947", "MHAdvantage");
        }
      }
      catch (ArgumentException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, ex.Message + ". Please modify this field and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return string.Empty;
      }
    }

    public bool ValidateData(string format, bool allowContinue)
    {
      this.writeMethodCall(nameof (ValidateData), (object) format, (object) allowContinue);
      return new EllieMae.EMLite.Export.ExportData(Session.LoanDataMgr, this.loanData).Validate(format, allowContinue);
    }

    private DocumentLog getDocumentLog(string docID)
    {
      LogRecordBase recordById = this.loanData.GetLogList().GetRecordByID(docID);
      return recordById is DocumentLog ? (DocumentLog) recordById : (DocumentLog) null;
    }

    public static void LinkAttachments(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      foreach (PendingAttachment pending in Bam._pendingList)
      {
        string title = pending.Title;
        if (title == "Untitled" && doc != null)
          title = doc.Title;
        loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Services, pending.Filepath, title, doc);
      }
      Bam._pendingList.Clear();
      if (doc != null)
      {
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(loanDataMgr, (LogRecordBase) doc);
        bool flag = true;
        bool isActive = true;
        if (folderAccessRights.RetrieveServiceMethod == RetrieveDocumentMethod.AssignNotCurrent)
          isActive = false;
        else if (folderAccessRights.RetrieveServiceMethod == RetrieveDocumentMethod.Unassigned)
          flag = false;
        foreach (FileAttachment attachment in Bam._attachmentList)
        {
          if (attachment.Title == "Untitled")
            attachment.Title = doc.Title;
          if (flag)
            doc.Files.Add(attachment.ID, Session.UserID, isActive);
        }
      }
      Bam._attachmentList.Clear();
    }

    public void AddComplianceLog(
      string company,
      string riskIndicator,
      string riskExplanation,
      bool freeReport)
    {
      this.writeMethodCall(nameof (AddComplianceLog), (object) company, (object) riskIndicator, (object) riskExplanation, (object) freeReport);
      throw new NotSupportedException();
    }

    public void AddCondition(
      string pairID,
      string docName,
      string condSource,
      string condInfo,
      bool eFolder)
    {
      this.writeMethodCall(nameof (AddCondition), (object) pairID, (object) docName, (object) condSource, (object) condInfo, (object) eFolder);
      throw new NotSupportedException();
    }

    public void AddEDMLog(string description, string action, string[] docList, string comments)
    {
      this.writeMethodCall(nameof (AddEDMLog), (object) description, (object) action, (object) docList, (object) comments);
      EDMLog edmLog = new EDMLog(Session.UserInfo.FullName + " (" + Session.UserID + ")");
      List<string> stringList = new List<string>();
      foreach (string doc in docList)
      {
        DocumentLog documentLog = this.getDocumentLog(doc);
        if (documentLog != null)
        {
          stringList.Add(documentLog.Title);
          this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) documentLog, "Doc sent to lender", (LogRecordBase) edmLog);
        }
      }
      edmLog.Description = description;
      edmLog.Comments = comments;
      edmLog.Documents = stringList.ToArray();
      this.loanData.GetLogList().AddRecord((LogRecordBase) edmLog);
      this.SaveLoan();
    }

    public void AddePASSLog(
      string title,
      string company,
      string orderDateStr,
      string receiveDateStr,
      string ePASSSignature,
      string comments)
    {
      this.writeMethodCall(nameof (AddePASSLog), (object) title, (object) company, (object) orderDateStr, (object) receiveDateStr, (object) ePASSSignature, (object) comments);
      this.AddDocumentLog(title, company, orderDateStr, string.Empty, receiveDateStr, comments, ePASSSignature);
    }

    public string AddDocumentLog(
      string title,
      string company,
      string dateRequested,
      string dateExpected,
      string dateReceived,
      string comments,
      string epassSignature)
    {
      this.writeMethodCall(nameof (AddDocumentLog), (object) title, (object) company, (object) dateRequested, (object) dateExpected, (object) dateReceived, (object) comments, (object) epassSignature);
      LogList logList = this.loanData.GetLogList();
      DocumentLog epassDoc = (DocumentLog) null;
      foreach (DocumentLog allDocument in logList.GetAllDocuments())
      {
        if (allDocument.Title == title && allDocument.PairId == this.loanData.PairId && allDocument.RequestedFrom == string.Empty)
          epassDoc = allDocument;
      }
      if (epassDoc == null)
      {
        DocumentTemplate byName = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(title);
        epassDoc = byName == null ? new DocumentLog(Session.UserID, this.loanData.PairId) : byName.CreateLogEntry(Session.UserID, this.loanData.PairId);
        epassDoc.Title = title;
        epassDoc.Stage = logList.NextStage;
        logList.AddRecord((LogRecordBase) epassDoc);
      }
      epassDoc.RequestedFrom = company;
      epassDoc.IsePASS = true;
      epassDoc.EPASSSignature = epassSignature;
      DateTime date1 = Utils.ParseDate((object) dateRequested);
      DateTime date2 = date1.Date;
      DateTime dateTime = DateTime.MinValue;
      DateTime date3 = dateTime.Date;
      if (date2 != date3)
        epassDoc.MarkAsRequested(date1, Session.UserID);
      DateTime date4 = Utils.ParseDate((object) dateExpected);
      DateTime date5 = date4.Date;
      dateTime = DateTime.MinValue;
      DateTime date6 = dateTime.Date;
      if (date5 != date6)
      {
        DatetimeUtils datetimeUtils = new DatetimeUtils(date1, this.loanData.DocumentDateTimeType);
        epassDoc.DaysDue = datetimeUtils.NumberOfDaysFrom(date4);
      }
      if (new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) epassDoc).RetrieveServiceMethod == RetrieveDocumentMethod.AssignCurrent)
      {
        DateTime date7 = Utils.ParseDate((object) dateReceived);
        DateTime date8 = date7.Date;
        dateTime = DateTime.MinValue;
        DateTime date9 = dateTime.Date;
        if (date8 != date9)
          epassDoc.MarkAsReceived(date7, Session.UserID);
      }
      if (comments != string.Empty)
        epassDoc.Comments.Add(comments, Session.UserID, Session.UserInfo.FullName);
      Bam.LinkAttachments(this.loanDataMgr, epassDoc);
      this.SaveLoan();
      if (string.IsNullOrEmpty(dateReceived))
      {
        dateTime = epassDoc.DateReceived;
        dateReceived = dateTime.ToString();
      }
      new Thread((ThreadStart) (() => this.AddAggregatorLog(title, company, dateRequested, dateExpected, dateReceived, comments, epassSignature, epassDoc)))
      {
        IsBackground = true
      }.Start();
      return epassDoc.Guid;
    }

    private void AddAggregatorLog(
      string title,
      string company,
      string dateRequested,
      string dateExpected,
      string dateReceived,
      string comments,
      string epassSignature,
      DocumentLog epassDoc)
    {
      try
      {
        new OrchestratorRestAPIHelper(this.LoanData.GUID).PostLog(this.ConstructPostData(title, company, dateRequested, dateExpected, dateReceived, comments, epassSignature, epassDoc));
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Orchestrator service call to create Aggregator log failed with the following exception:  " + (object) ex);
      }
    }

    private string ConstructPostData(
      string title,
      string Company,
      string DateRequested,
      string DateExpected,
      string DateReceived,
      string Comments,
      string EpassSignature,
      DocumentLog epassDoc)
    {
      string instanceName = Session.DefaultInstance.ServerIdentity.InstanceName;
      string guid = this.loanData.GUID;
      string id = this.loanData.CurrentBorrowerPair.Id;
      return new JObject()
      {
        ["serviceSetupId"] = ((JToken) this.LoanData.GUID),
        ["serviceCategory"] = ((JToken) title),
        ["vendorPlatform"] = ((JToken) "EMN"),
        ["entityRef"] = ((JToken) JObject.FromObject((object) new
        {
          entityId = string.Format("urn:elli:encompass:{0}:loan:{1}", (object) instanceName, (object) guid),
          entityType = "urn:elli:encompass:loan"
        })),
        ["scope"] = ((JToken) JObject.FromObject((object) new
        {
          entityRef = JObject.FromObject((object) new
          {
            entityId = string.Format("urn:elli:encompass:{0}:loan:{1}:application:{2}", (object) instanceName, (object) guid, (object) id),
            entityType = "urn:elli:encompass:loan:application"
          }),
          orderServiceFor = "APPLICATION"
        })),
        ["type"] = ((JToken) "MANUAL"),
        ["reason"] = ((JToken) string.Format("EMN Transaction from BAM, Encompass {0}", (object) this.GetDisplayVersion())),
        ["request"] = ((JToken) JObject.FromObject((object) new
        {
          loan = JObject.FromObject((object) new
          {
            instanceId = instanceName,
            userId = Session.DefaultInstance.UserInfo.Userid,
            loanId = guid,
            applicationId = id,
            documentId = epassDoc.Guid,
            documentPairId = epassDoc.PairId,
            company = Company,
            dateRequested = DateRequested,
            dateReceived = DateReceived,
            comments = Comments,
            epassSignature = EpassSignature
          })
        }))
      }.ToString();
    }

    public void AddLifeInsuranceLog(
      string title,
      string status,
      string followUpDate,
      bool alertLO,
      bool alertLP,
      string comments)
    {
      this.writeMethodCall(nameof (AddLifeInsuranceLog), (object) title, (object) status, (object) followUpDate, (object) alertLO, (object) alertLP, (object) comments);
      throw new NotSupportedException();
    }

    public void AddPreliminaryConditionLog(
      string source,
      string title,
      string description,
      string details,
      string pairID,
      string category,
      string priorTo)
    {
      this.writeMethodCall(nameof (AddPreliminaryConditionLog), (object) source, (object) title, (object) description, (object) details, (object) pairID, (object) category, (object) priorTo);
      PreliminaryConditionLog rec = new PreliminaryConditionLog(Session.UserID, pairID);
      rec.Source = source;
      rec.Title = title;
      rec.Description = description;
      rec.Details = details;
      rec.Category = category;
      rec.PriorTo = priorTo;
      this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
    }

    public void AddLockRequest()
    {
      this.writeMethodCall(nameof (AddLockRequest));
      this.loanDataMgr.CreateRateLockRequest();
    }

    public string[] GetDocumentList()
    {
      this.writeMethodCall(nameof (GetDocumentList));
      List<string> stringList = new List<string>();
      foreach (DocumentLog allDocument in this.loanData.GetLogList().GetAllDocuments())
        stringList.Add(allDocument.Guid);
      return stringList.ToArray();
    }

    public string GetDocumentListXml()
    {
      this.writeMethodCall(nameof (GetDocumentListXml));
      DocumentLog[] allDocuments = this.loanData.GetLogList().GetAllDocuments();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<DOCUMENTLIST></DOCUMENTLIST>");
      XmlElement documentElement = xmlDocument.DocumentElement;
      this.loanData.GetNumberOfBorrowerPairs();
      foreach (DocumentLog documentLog in allDocuments)
      {
        XmlElement element = xmlDocument.CreateElement("DOCUMENT");
        AttributeWriter attributeWriter = new AttributeWriter(element);
        attributeWriter.Write("GUID", (object) documentLog.Guid);
        attributeWriter.Write("IsePASS", (object) documentLog.IsePASS);
        attributeWriter.Write("EPASSSignature", (object) documentLog.EPASSSignature);
        attributeWriter.Write("eDisclosure", (object) documentLog.OpeningDocument);
        attributeWriter.Write("ArchiveDate", (object) documentLog.DateArchived);
        attributeWriter.Write("Title", (object) documentLog.Title);
        attributeWriter.Write("Company", (object) documentLog.RequestedFrom);
        attributeWriter.Write("OrderDate", (object) documentLog.DateRequested);
        attributeWriter.Write("ReceiveDate", (object) documentLog.DateReceived);
        attributeWriter.Write("ReorderDate", (object) documentLog.DateRerequested);
        attributeWriter.Write("Stage", (object) documentLog.Stage);
        attributeWriter.Write("PairId", (object) documentLog.PairId);
        attributeWriter.Write("DaysDue", (object) documentLog.DaysDue);
        attributeWriter.Write("DaysTillExpire", (object) documentLog.DaysTillExpire);
        attributeWriter.Write("DateAdded", (object) documentLog.DateAdded);
        attributeWriter.Write("AddedBy", (object) documentLog.AddedBy);
        attributeWriter.Write("RequestedBy", (object) documentLog.RerequestedBy);
        attributeWriter.Write("RerequestedBy", (object) documentLog.RerequestedBy);
        attributeWriter.Write("ReceivedBy", (object) documentLog.ReceivedBy);
        attributeWriter.Write("ArchivedBy", (object) documentLog.ArchivedBy);
        attributeWriter.Write("IsExternal", (object) documentLog.IsThirdPartyDoc);
        attributeWriter.Write("Status", (object) documentLog.Status);
        attributeWriter.Write("Date", (object) documentLog.Date);
        if (documentLog.PairId != string.Empty)
        {
          int pairIndex = this.loanData.GetPairIndex(documentLog.PairId);
          if (pairIndex >= 0)
          {
            attributeWriter.Write("PairIndex", (object) pairIndex.ToString());
            try
            {
              attributeWriter.Write("BorFName", (object) this.loanData.GetSimpleField("36", pairIndex));
              attributeWriter.Write("BorLName", (object) this.loanData.GetSimpleField("37", pairIndex));
              attributeWriter.Write("CoBorFName", (object) this.loanData.GetSimpleField("68", pairIndex));
              attributeWriter.Write("CoBorLName", (object) this.loanData.GetSimpleField("69", pairIndex));
            }
            catch (Exception ex)
            {
              attributeWriter.Write("Error", (object) (ex.Message + ex.Source + ex.StackTrace));
            }
          }
        }
        documentElement.AppendChild((XmlNode) element);
      }
      return documentElement.OuterXml;
    }

    public string GetDocumentTitle(string docID)
    {
      this.writeMethodCall(nameof (GetDocumentTitle), (object) docID);
      return this.getDocumentLog(docID)?.Title;
    }

    public string GetDocumentCompany(string docID)
    {
      this.writeMethodCall(nameof (GetDocumentCompany), (object) docID);
      return this.getDocumentLog(docID)?.RequestedFrom;
    }

    public string GetDocumentPairID(string docID)
    {
      this.writeMethodCall(nameof (GetDocumentPairID), (object) docID);
      return this.getDocumentLog(docID)?.PairId;
    }

    public void UpdateVerifLog(
      string verifID,
      bool ordered,
      bool received,
      string ePASSSignature,
      string comments)
    {
      this.writeMethodCall(nameof (UpdateVerifLog), (object) verifID, (object) ordered, (object) received, (object) ePASSSignature, (object) comments);
      VerifLog verif = this.loanData.GetLogList().GetVerif(verifID);
      if (verif == null)
        return;
      verif.IsePASS = true;
      verif.EPASSSignature = ePASSSignature;
      if (ordered)
        verif.MarkAsRequested(DateTime.Now, Session.UserID);
      if (received)
        verif.MarkAsReceived(DateTime.Now, Session.UserID);
      if (comments != string.Empty)
        verif.Comments.Add(comments, Session.UserID, Session.UserInfo.FullName);
      Bam.LinkAttachments(this.loanDataMgr, (DocumentLog) verif);
    }

    public void GoToField(string fieldID)
    {
      this.writeMethodCall(nameof (GoToField), (object) fieldID);
      this.GoToField(fieldID, false);
    }

    public void GoToField(string fieldID, bool findNext)
    {
      this.writeMethodCall(nameof (GoToField), (object) fieldID, (object) findNext);
      Session.Application.GetService<ILoanEditor>().BAMGoToField(fieldID, findNext);
    }

    public bool IsDocumentCollector()
    {
      this.writeMethodCall(nameof (IsDocumentCollector));
      return this.browser is BrowserControl;
    }

    public void Navigate(string url)
    {
      this.writeMethodCall(nameof (Navigate), (object) url);
      if (this.browser != null)
        this.browser.Navigate(url);
      else
        Session.Application.GetService<IEPass>().Navigate(url);
    }

    public void ProcessURL(string url)
    {
      this.writeMethodCall(nameof (ProcessURL), (object) url);
      if (this.browser != null)
        this.browser.ProcessURL(url);
      else
        Session.Application.GetService<IEPass>().ProcessURL(url);
    }

    public bool SendMessage(string msgFile)
    {
      this.writeMethodCall(nameof (SendMessage), (object) msgFile);
      return this.browser != null ? this.browser.SendMessage(msgFile) : Session.Application.GetService<IEPass>().SendMessage(msgFile);
    }

    public void ShowHelp(string helpPageName)
    {
      this.writeMethodCall(nameof (ShowHelp), (object) helpPageName);
      JedHelp.ShowHelp(helpPageName);
    }

    public void ShowPdfFile(string fileName)
    {
      this.writeMethodCall(nameof (ShowPdfFile), (object) fileName);
      throw new NotSupportedException();
    }

    public void ShowWebPage(string title, string url, int width, int height)
    {
      this.writeMethodCall(nameof (ShowWebPage), (object) title, (object) url);
      Session.MainScreen.OpenURL(url, title, width, height);
    }

    public void CreateLead(object properties)
    {
      this.writeMethodCall(nameof (CreateLead), properties);
      Hashtable hashtable = (Hashtable) properties;
      BorrowerInfo info = new BorrowerInfo(Session.UserID);
      Opportunity opportunity = new Opportunity();
      foreach (string key in (IEnumerable) hashtable.Keys)
      {
        try
        {
          info[key] = hashtable[(object) key];
        }
        catch (ArgumentException ex)
        {
          opportunity[key] = hashtable[(object) key];
        }
      }
      int borrower = Session.ContactManager.CreateBorrower(info, DateTime.Now, ContactSource.LeadCenter);
      opportunity.ContactID = borrower;
      Session.ContactManager.CreateBorrowerOpportunity(opportunity);
    }

    public bool LeadExists(string firstName, string lastName, string leadSource, string txnId)
    {
      this.writeMethodCall(nameof (LeadExists), (object) firstName, (object) lastName, (object) leadSource, (object) txnId);
      StringValueCriterion stringValueCriterion = new StringValueCriterion("Contact.FirstName", firstName, StringMatchType.CaseInsensitive);
      StringValueCriterion criterion1 = new StringValueCriterion("Contact.LastName", lastName, StringMatchType.CaseInsensitive);
      StringValueCriterion criterion2 = new StringValueCriterion("Contact.LeadSource", leadSource, StringMatchType.CaseInsensitive);
      StringValueCriterion criterion3 = new StringValueCriterion("Contact.LeadTxnID", txnId, StringMatchType.Exact);
      return Session.ContactManager.QueryBorrowers(new QueryCriterion[1]
      {
        stringValueCriterion.And((QueryCriterion) criterion1).And((QueryCriterion) criterion2).And((QueryCriterion) criterion3)
      }, RelatedLoanMatchType.None).Length != 0;
    }

    public string ShowRolodex(string category, bool multiSelect)
    {
      this.writeMethodCall(nameof (ShowRolodex), (object) category, (object) multiSelect);
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact(category, multiSelect, false))
        return rxBusinessContact.ShowDialog() == DialogResult.OK ? rxBusinessContact.SelectedXML : string.Empty;
    }

    public string AddAttachment(string filepath)
    {
      this.writeMethodCall(nameof (AddAttachment), (object) filepath);
      return this.AddAttachment(filepath, "Untitled");
    }

    public string AddAttachment(string filepath, string title)
    {
      this.writeMethodCall(nameof (AddAttachment), (object) filepath, (object) title);
      if (!this.loanDataMgr.UseSkyDrive && this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments)
      {
        string destFileName = SystemSettings.GetTempFileName(filepath);
        try
        {
          File.Copy(filepath, destFileName);
        }
        catch (Exception ex)
        {
          Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "File.Copy failed: " + (object) ex);
          destFileName = filepath;
        }
        PendingAttachment pendingAttachment = new PendingAttachment();
        pendingAttachment.ID = Guid.NewGuid().ToString();
        pendingAttachment.Title = title;
        pendingAttachment.Filepath = destFileName;
        Bam._pendingList.Add(pendingAttachment);
        return pendingAttachment.ID;
      }
      FileAttachment fileAttachment = this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Services, filepath, title, (DocumentLog) null);
      Bam._attachmentList.Add(fileAttachment);
      return fileAttachment.ID;
    }

    public string AddAttachment(string filepath, string title, string docID)
    {
      this.writeMethodCall(nameof (AddAttachment), (object) filepath, (object) title, (object) docID);
      DocumentLog documentLog = this.getDocumentLog(docID);
      return this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Services, filepath, title, documentLog).ID;
    }

    public bool CanAddAttachment(string docID)
    {
      this.writeMethodCall(nameof (CanAddAttachment), (object) docID);
      return new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) this.getDocumentLog(docID)).CanBrowseAttach;
    }

    public string ExportDocument(string docID)
    {
      this.writeMethodCall(nameof (ExportDocument), (object) docID);
      DocumentLog documentLog = this.getDocumentLog(docID);
      return documentLog == null ? (string) null : Session.Application.GetService<IEFolder>().Export(this.loanDataMgr, documentLog);
    }

    public void PrintFaxCover(string orderNo, string faxNumber, string trackingNo, string misc2)
    {
      this.writeMethodCall(nameof (PrintFaxCover), (object) orderNo, (object) faxNumber, (object) trackingNo, (object) misc2);
      throw new NotSupportedException();
    }

    public string[] SelectDocuments(bool multiSelect)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2914, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
        this.writeMethodCall(nameof (SelectDocuments), (object) multiSelect);
        List<string> stringList = new List<string>();
        if (multiSelect)
        {
          IEFolder service = Session.Application.GetService<IEFolder>();
          DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
          if (allDocuments == null)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT allDocList == null", 2929, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            return (string[]) null;
          }
          List<DocumentLog> documentLogList = new List<DocumentLog>();
          foreach (DocumentLog documentLog in allDocuments)
          {
            if (documentLog.IsThirdPartyDoc)
              documentLogList.Add(documentLog);
          }
          DocumentLog[] documentLogArray = service.SelectDocuments(this.loanDataMgr, documentLogList.ToArray());
          if (documentLogArray == null)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT docList == null", 2943, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            return (string[]) null;
          }
          foreach (DocumentLog documentLog in documentLogArray)
            stringList.Add(documentLog.Guid);
        }
        else
        {
          using (SelectDocumentDialog selectDocumentDialog = new SelectDocumentDialog(this.loanDataMgr))
          {
            PerformanceMeter.Current.AddCheckpoint("new SelectDocumentDialog", 2956, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE SelectDocumentDialog ShowDialog", 2958, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            DialogResult dialogResult = selectDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER SelectDocumentDialog ShowDialog - " + dialogResult.ToString(), 2960, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            if (dialogResult == DialogResult.Cancel)
              return (string[]) null;
            stringList.Add(selectDocumentDialog.Document.Guid);
          }
        }
        return stringList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2974, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
      }
    }

    public string[] SelectDocuments2(bool multiSelect, bool allowContinue)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2982, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
        this.writeMethodCall(nameof (SelectDocuments2), (object) multiSelect, (object) allowContinue);
        List<string> stringList = new List<string>();
        if (multiSelect)
        {
          IEFolder service = Session.Application.GetService<IEFolder>();
          DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
          if (allDocuments == null)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT allDocList == null", 2998, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            return (string[]) null;
          }
          List<DocumentLog> documentLogList = new List<DocumentLog>();
          foreach (DocumentLog documentLog in allDocuments)
          {
            if (documentLog.IsThirdPartyDoc)
              documentLogList.Add(documentLog);
          }
          DocumentLog[] documentLogArray = service.SelectDocuments(this.loanDataMgr, documentLogList.ToArray(), allowContinue);
          if (documentLogArray == null)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT docList == null", 3012, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            return (string[]) null;
          }
          foreach (DocumentLog documentLog in documentLogArray)
            stringList.Add(documentLog.Guid);
        }
        else
        {
          using (SelectDocumentDialog selectDocumentDialog = new SelectDocumentDialog(this.loanDataMgr))
          {
            PerformanceMeter.Current.AddCheckpoint("new SelectDocumentDialog", 3025, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE SelectDocumentDialog ShowDialog", 3027, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            DialogResult dialogResult = selectDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER SelectDocumentDialog ShowDialog - " + dialogResult.ToString(), 3029, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
            if (dialogResult == DialogResult.Cancel)
              return (string[]) null;
            stringList.Add(selectDocumentDialog.Document.Guid);
          }
        }
        return stringList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3043, nameof (SelectDocuments2), "D:\\ws\\24.3.0.0\\EmLite\\ePass\\Bam.cs");
      }
    }

    public string SelectEVaultDocument()
    {
      this.writeMethodCall(nameof (SelectEVaultDocument));
      return Session.Application.GetService<IEFolder>().SelectEVaultDocument(this.loanDataMgr);
    }

    public bool SendPDF(string pdfFile)
    {
      this.writeMethodCall(nameof (SendPDF), (object) pdfFile);
      string[] docList = new string[0];
      return this.SendPDF(pdfFile, docList);
    }

    public bool SendPDF(string pdfFile, string[] docList)
    {
      this.writeMethodCall(nameof (SendPDF), (object) pdfFile, (object) docList);
      return Session.Application.GetService<IEFolder>().SendForms(this.loanDataMgr, docList, pdfFile);
    }

    public string[] SelectEncompassForms(bool selectBorrowerPair)
    {
      this.writeMethodCall(nameof (SelectEncompassForms), (object) selectBorrowerPair);
      using (FormSelectionDialog formSelectionDialog = new FormSelectionDialog(selectBorrowerPair))
        return formSelectionDialog.ShowDialog() == DialogResult.Cancel ? (string[]) null : formSelectionDialog.DocumentList;
    }

    public string GetEncompassFormTitle(string formID)
    {
      this.writeMethodCall(nameof (GetEncompassFormTitle), (object) formID);
      return formID;
    }

    public string GetEncompassFormType(string formID)
    {
      this.writeMethodCall(nameof (GetEncompassFormType), (object) formID);
      return Session.ConfigurationManager.GetDocumentTrackingSetup().GetByName(formID)?.SourceType;
    }

    public string GetEncompassSignatureType(string formID)
    {
      this.writeMethodCall(nameof (GetEncompassSignatureType), (object) formID);
      return Session.ConfigurationManager.GetDocumentTrackingSetup().GetByName(formID)?.SignatureType;
    }

    public string ExportEncompassForm(string formID)
    {
      this.writeMethodCall(nameof (ExportEncompassForm), (object) formID);
      DocumentTemplate byName = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(formID);
      if (byName == null)
        return (string) null;
      string[] fileList = new FormExport(this.loanDataMgr).ExportForms(byName, true);
      if (fileList == null)
        return (string) null;
      using (PdfEditor pdfEditor = new PdfEditor(new PdfCreator().MergeFiles(fileList)))
        return pdfEditor.SetAttributes(byName.SignatureType, ForBorrowerType.All, (string) null);
    }

    public string ExportFaxCoversheet()
    {
      this.writeMethodCall(nameof (ExportFaxCoversheet));
      return new FormExport(this.loanDataMgr).ExportFaxCoversheet();
    }

    public string[] GetDocumentAttachmentList(string docID)
    {
      this.writeMethodCall(nameof (GetDocumentAttachmentList), (object) docID);
      DocumentLog documentLog = this.getDocumentLog(docID);
      List<string> stringList = new List<string>();
      if (documentLog != null)
      {
        foreach (FileAttachmentReference attachmentReference in documentLog.Files.ToArray())
          stringList.Add(attachmentReference.AttachmentID);
      }
      return stringList.ToArray();
    }

    public string GetAttachment(string attachmentID)
    {
      this.writeMethodCall(nameof (GetAttachment), (object) attachmentID);
      FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[attachmentID];
      switch (fileAttachment)
      {
        case NativeAttachment _:
          return this.loanDataMgr.FileAttachments.DownloadAttachment((NativeAttachment) fileAttachment);
        case ImageAttachment _:
          return this.loanDataMgr.FileAttachments.CreatePdf((ImageAttachment) fileAttachment);
        case CloudAttachment _:
          return this.loanDataMgr.FileAttachments.CreatePdf(new CloudAttachment[1]
          {
            (CloudAttachment) fileAttachment
          }, AnnotationExportType.Public);
        default:
          return (string) null;
      }
    }

    public int GetAttachmentRotation(string attachmentID)
    {
      this.writeMethodCall(nameof (GetAttachmentRotation), (object) attachmentID);
      FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[attachmentID];
      return fileAttachment is NativeAttachment ? ((NativeAttachment) fileAttachment).Rotation : 0;
    }

    public string GetAttachmentTitle(string attachmentID)
    {
      this.writeMethodCall(nameof (GetAttachmentTitle), (object) attachmentID);
      return this.loanDataMgr.FileAttachments[attachmentID]?.Title;
    }

    public string ExportAttachment(string attachmentID)
    {
      this.writeMethodCall(nameof (ExportAttachment), (object) attachmentID);
      FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[attachmentID];
      return fileAttachment == null ? (string) null : Session.Application.GetService<IEFolder>().Export(this.loanDataMgr, fileAttachment);
    }

    public string[] GetEncompassCloserForms()
    {
      this.writeMethodCall(nameof (GetEncompassCloserForms), (object[]) null);
      throw new NotSupportedException();
    }

    public bool PreviewClosing(string[] titleList, string[] pdfList)
    {
      this.writeMethodCall(nameof (PreviewClosing), (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public bool PrintClosing(string[] titleList, string[] pdfList)
    {
      this.writeMethodCall(nameof (PrintClosing), (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public bool SendClosing(string[] titleList, string[] pdfList)
    {
      this.writeMethodCall(nameof (SendClosing), (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public string AddDisclosureDocument(string docTitle)
    {
      this.writeMethodCall(nameof (AddDisclosureDocument), (object) docTitle);
      throw new NotSupportedException();
    }

    public void DeliverDisclosures(string zipFile, bool premiumDisclosures)
    {
      this.writeMethodCall(nameof (DeliverDisclosures), (object) zipFile, (object) premiumDisclosures);
      throw new NotSupportedException();
    }

    public bool ShowDisclosureEntities()
    {
      this.writeMethodCall(nameof (ShowDisclosureEntities));
      return false;
    }

    public bool ShowDisclosurePackages()
    {
      this.writeMethodCall(nameof (ShowDisclosurePackages));
      return this.getEDisclosureSetup().GetChannel(this.loanData).GetControlOption(this.loanData) == ControlOptionType.User;
    }

    public string[] GetCompanyDisclosureEntities()
    {
      this.writeMethodCall("GetDisclosureEntities");
      EDisclosureChannel channel = this.getEDisclosureSetup().GetChannel(this.loanData);
      List<string> stringList = new List<string>();
      if (channel.IsBroker)
        stringList.Add("Broker");
      if (channel.IsLender)
        stringList.Add("Lender");
      if (stringList.Count != 0)
        return stringList.ToArray();
      int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no types of disclosures available to send to this borrower pair at this time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return (string[]) null;
    }

    public string[] GetCompanyDisclosurePackages()
    {
      this.writeMethodCall(nameof (GetCompanyDisclosurePackages));
      EDisclosureSetup edisclosureSetup = this.getEDisclosureSetup();
      EDisclosureChannel channel = edisclosureSetup.GetChannel(this.loanData);
      MilestoneTemplate milestoneTemplate = this.loanData.GetLogList().MilestoneTemplate;
      if (milestoneTemplate != null && milestoneTemplate.TemplateID != null)
        milestoneTemplate = this.loanDataMgr.SessionObjects.BpmManager.GetMilestoneTemplate(milestoneTemplate.TemplateID);
      EDisclosurePackage[] collection = edisclosureSetup.GetChannelPackages(this.loanData, milestoneTemplate);
      if (collection.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no disclosure packages available to send to this borrower pair at this time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (string[]) null;
      }
      if (channel.GetControlOption(this.loanData) == ControlOptionType.Conditional)
      {
        List<EDisclosurePackage> edisclosurePackageList = new List<EDisclosurePackage>((IEnumerable<EDisclosurePackage>) collection);
        foreach (DisclosureTrackingLog disclosureTrackingLog in this.loanData.GetLogList().GetAllDisclosureTrackingLog(true))
        {
          if (!(disclosureTrackingLog.BorrowerPairID != this.loanData.PairId))
          {
            if (disclosureTrackingLog.eDisclosureApplicationPackage)
              edisclosurePackageList.Remove(channel.ConditionalApplication);
            if (disclosureTrackingLog.eDisclosureThreeDayPackage)
              edisclosurePackageList.Remove(channel.ConditionalThreeDay);
            if (disclosureTrackingLog.eDisclosureLockPackage)
              edisclosurePackageList.Remove(channel.ConditionalLock);
            if (disclosureTrackingLog.eDisclosureApprovalPackage)
              edisclosurePackageList.Remove(channel.ConditionalApproval);
          }
        }
        if (edisclosurePackageList.Count > 0)
          collection = edisclosurePackageList.ToArray();
        else if (Utils.Dialog((IWin32Window) Form.ActiveForm, "This borrower pair has already received all of the available disclosures. Would you like to send them again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return (string[]) null;
      }
      List<string> stringList = new List<string>();
      foreach (EDisclosurePackage edisclosurePackage in collection)
      {
        if (edisclosurePackage.AtApplication && !stringList.Contains("At Application"))
          stringList.Add("At Application");
        if (edisclosurePackage.ThreeDay && !stringList.Contains("Three-Day"))
          stringList.Add("Three-Day");
        if (edisclosurePackage.AtLock && !stringList.Contains("At Lock"))
          stringList.Add("At Lock");
        if (edisclosurePackage.Approval && !stringList.Contains("Approval"))
          stringList.Add("Approval");
        if (edisclosurePackage.IncludeGFE && !stringList.Contains("Include GFE"))
          stringList.Add("Include GFE");
        if (edisclosurePackage.IncludeTIL && !stringList.Contains("Include TIL"))
          stringList.Add("Include TIL");
      }
      if (stringList.Count != 0)
        return stringList.ToArray();
      int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no disclosures available to send to this borrower pair at this time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return (string[]) null;
    }

    public bool AreInformationalOnlyDisclosures()
    {
      this.writeMethodCall(nameof (AreInformationalOnlyDisclosures));
      return this.getEDisclosureSetup().GetChannel(this.loanData).IsInformationalOnly;
    }

    public bool DoNotUseEMeDisclosures()
    {
      this.writeMethodCall(nameof (DoNotUseEMeDisclosures));
      return false;
    }

    public string[] GetEncompassDisclosureForms()
    {
      this.writeMethodCall(nameof (GetEncompassDisclosureForms));
      throw new NotSupportedException();
    }

    public string[] GetEncompassDisclosureForms(string[] entityList, string[] packageList)
    {
      this.writeMethodCall(nameof (GetEncompassDisclosureForms), (object) entityList, (object) packageList);
      throw new NotSupportedException();
    }

    public bool PreviewDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList)
    {
      this.writeMethodCall(nameof (PreviewDisclosures), (object) coversheetFile, (object) entityList, (object) packageList, (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public bool PrintDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList)
    {
      this.writeMethodCall(nameof (PrintDisclosures), (object) coversheetFile, (object) entityList, (object) packageList, (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public bool SendDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList)
    {
      this.writeMethodCall(nameof (SendDisclosures), (object) coversheetFile, (object) entityList, (object) packageList, (object) titleList, (object) pdfList);
      throw new NotSupportedException();
    }

    public string[] GetOffers(string companyID) => new string[0];

    public string GetOfferCategory(string offerID) => (string) null;

    public string GetOfferCompanyID(string offerID) => (string) null;

    public string GetOfferCompanyName(string offerID) => (string) null;

    public string GetOfferCompanyProgram(string offerID) => (string) null;

    public string GetOfferDescription(string offerID) => (string) null;

    public void LogVerbose(string source, string message)
    {
      Tracing.Log(this.sw, source, TraceLevel.Verbose, message);
    }

    public void LogInfo(string source, string message)
    {
      Tracing.Log(this.sw, source, TraceLevel.Info, message);
    }

    public void LogWarning(string source, string message)
    {
      Tracing.Log(this.sw, source, TraceLevel.Warning, message);
    }

    public void LogError(string source, string message)
    {
      Tracing.Log(this.sw, source, TraceLevel.Error, message);
    }

    private void writeMethodCall(string methodName, params object[] parms)
    {
      try
      {
        if (!this.debug)
          return;
        Tracing.Log(true, "DEBUG", nameof (Bam), "BAM/" + (object) Thread.CurrentThread.GetHashCode() + ": " + methodName + "(" + Bam.paramsToString(parms) + ")");
      }
      catch
      {
      }
    }

    private static string paramsToString(object[] parms)
    {
      if (parms == null || parms.Length == 0)
        return "";
      string str1 = "";
      for (int index = 0; index < parms.Length; ++index)
      {
        object parm = parms[index];
        string str2;
        switch (parm)
        {
          case null:
            str2 = str1 + "null";
            break;
          case string _:
            str2 = str1 + "\"" + parm + "\"";
            break;
          case Enum _:
            str2 = str1 + parm.GetType().Name + "." + parm;
            break;
          case System.ValueType _:
            str2 = str1 + parm.ToString();
            break;
          default:
            if ((object) (parm as LoanIdentity) != null)
            {
              str2 = str1 + "<LoanIdentity(" + parm.ToString() + ")>";
              break;
            }
            if (parm is Array)
            {
              Array array = (Array) parm;
              System.Type elementType = array.GetType().GetElementType();
              str2 = str1 + "<" + elementType.Name + "[" + (object) array.Length + "]>";
              break;
            }
            str2 = str1 + "<" + parm.GetType().Name + ">";
            break;
        }
        str1 = str2 + (index < parms.Length - 1 ? ", " : "");
      }
      return str1;
    }

    public string GetEdition()
    {
      this.writeMethodCall(nameof (GetEdition));
      string edition = string.Empty;
      switch (Session.EncompassEdition)
      {
        case EncompassEdition.Broker:
          edition = "Broker";
          break;
        case EncompassEdition.Banker:
          edition = "Banker";
          break;
      }
      if (Session.StartupInfo.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted)
        edition = "Anywhere";
      return edition;
    }

    public string GetVersion()
    {
      this.writeMethodCall(nameof (GetVersion));
      return VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition);
    }

    public void SetTab(string tabName)
    {
      this.writeMethodCall(nameof (SetTab), (object) tabName);
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.setTabAsync), (object) tabName);
    }

    private void setTabAsync(object tabName)
    {
      if (Session.MainScreen.InvokeRequired)
      {
        Session.MainScreen.Invoke((Delegate) new WaitCallback(this.setTabAsync), new object[1]
        {
          tabName
        });
      }
      else
      {
        Session.Application.GetService<IEncompassApplication>().SetCurrentScreen(tabName.ToString());
        if (!(tabName.ToString().ToLower() == "loans"))
          return;
        Session.Application.GetService<ILoanEditor>().RefreshContents();
      }
    }

    public string SelectAnEmailAddress()
    {
      this.writeMethodCall(nameof (SelectAnEmailAddress));
      throw new NotSupportedException();
    }

    public void ViewDocument(string docID)
    {
      DocumentLog documentLog = this.getDocumentLog(docID);
      if (documentLog == null)
        return;
      Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, documentLog);
    }

    public void RetrieveDocument(string docID) => this.RetrieveDocument2(docID, true);

    public int[] GetOrganizationList()
    {
      this.writeMethodCall(nameof (GetOrganizationList));
      this.orgList = Session.OrganizationManager.GetAllOrganizations();
      List<int> intList = new List<int>();
      foreach (OrgInfo org in this.orgList)
        intList.Add(org.Oid);
      return intList.ToArray();
    }

    public string GetOrganizationName(int orgID)
    {
      this.writeMethodCall(nameof (GetOrganizationName), (object) orgID);
      return this.getOrganizationInfo(orgID)?.OrgName;
    }

    public string GetOrganizationCode(int orgID)
    {
      this.writeMethodCall(nameof (GetOrganizationCode), (object) orgID);
      string organizationCode = string.Empty;
      OrgInfo organizationInfo = this.getOrganizationInfo(orgID);
      if (organizationInfo != null)
        organizationCode = organizationInfo.OrgCode;
      if (organizationCode == string.Empty)
      {
        OrgInfo avaliableOrganization = Session.OrganizationManager.GetFirstAvaliableOrganization(orgID);
        if (avaliableOrganization == null)
          return (string) null;
        organizationCode = avaliableOrganization.OrgCode;
      }
      return organizationCode;
    }

    private OrgInfo getOrganizationInfo(int orgID)
    {
      if (this.orgList == null)
        this.orgList = Session.OrganizationManager.GetAllOrganizations();
      foreach (OrgInfo org in this.orgList)
      {
        if (org.Oid == orgID)
          return org;
      }
      return (OrgInfo) null;
    }

    public string[] GetUserList(bool accessibleOnly)
    {
      this.writeMethodCall(nameof (GetUserList), (object) accessibleOnly);
      this.userList = !accessibleOnly ? Session.OrganizationManager.GetAllUsers() : Session.OrganizationManager.GetAllAccessibleUsers();
      List<string> stringList = new List<string>();
      foreach (UserInfo user in this.userList)
        stringList.Add(user.Userid);
      return stringList.ToArray();
    }

    public string GetUserFirstName(string userID)
    {
      this.writeMethodCall(nameof (GetUserFirstName), (object) userID);
      UserInfo userInfo = this.getUserInfo(userID);
      return userInfo != (UserInfo) null ? userInfo.FirstName : (string) null;
    }

    public string GetUserLastName(string userID)
    {
      this.writeMethodCall(nameof (GetUserLastName), (object) userID);
      UserInfo userInfo = this.getUserInfo(userID);
      return userInfo != (UserInfo) null ? userInfo.LastName : (string) null;
    }

    public string GetUserEmail(string userID)
    {
      this.writeMethodCall(nameof (GetUserEmail), (object) userID);
      UserInfo userInfo = this.getUserInfo(userID);
      return userInfo != (UserInfo) null ? userInfo.Email : (string) null;
    }

    public int[] GetUserPersonaList(string userID)
    {
      this.writeMethodCall(nameof (GetUserPersonaList), (object) userID);
      UserInfo userInfo = this.getUserInfo(userID);
      if (userInfo == (UserInfo) null)
        return (int[]) null;
      List<int> intList = new List<int>();
      foreach (Persona userPersona in userInfo.UserPersonas)
        intList.Add(userPersona.ID);
      return intList.ToArray();
    }

    public int GetUserOrganization(string userID)
    {
      this.writeMethodCall(nameof (GetUserOrganization), (object) userID);
      UserInfo userInfo = this.getUserInfo(userID);
      return userInfo != (UserInfo) null ? userInfo.OrgId : -1;
    }

    public string[] GetPersonaUsers(int personaID)
    {
      this.writeMethodCall(nameof (GetPersonaUsers), (object) personaID);
      List<string> stringList = new List<string>();
      foreach (UserInfo userInfo in Session.OrganizationManager.GetUsersWithPersona(personaID, false))
        stringList.Add(userInfo.Userid);
      return stringList.ToArray();
    }

    public int[] GetUserGroupList()
    {
      this.writeMethodCall(nameof (GetUserGroupList));
      List<int> intList = new List<int>();
      foreach (AclGroup allGroup in Session.AclGroupManager.GetAllGroups())
        intList.Add(allGroup.ID);
      return intList.ToArray();
    }

    public string GetUserGroupName(int groupID)
    {
      this.writeMethodCall(nameof (GetUserGroupName));
      foreach (AclGroup allGroup in Session.AclGroupManager.GetAllGroups())
      {
        if (groupID == allGroup.ID)
          return allGroup.Name;
      }
      return string.Empty;
    }

    public string[] GetUserGroupUsers(int groupID)
    {
      this.writeMethodCall(nameof (GetUserGroupUsers), (object) groupID);
      return Session.AclGroupManager.GetUsersInGroup(groupID, true);
    }

    private UserInfo getUserInfo(string userID)
    {
      if (this.userList == null)
        this.userList = Session.OrganizationManager.GetAllUsers();
      foreach (UserInfo user in this.userList)
      {
        if (string.Equals(user.Userid, userID, StringComparison.CurrentCultureIgnoreCase))
          return user;
      }
      return (UserInfo) null;
    }

    private void setValueText(string node, string val)
    {
      if (val == string.Empty || val == null)
        return;
      this.xPath(this.xmlBase + node, val.Trim());
    }

    private void xPath(string xmlPath, string newValue)
    {
      if (xmlPath.EndsWith("/"))
        xmlPath = xmlPath.Substring(0, xmlPath.Length - 1);
      string[] strArray = xmlPath.Split('/');
      XmlNode xmlNode1 = (XmlNode) this.xml;
      try
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str1 = strArray[index];
          if (str1.Substring(0, 1) == "@")
          {
            string name = str1.Substring(1);
            ((XmlElement) xmlNode1).SetAttribute(name, newValue);
            return;
          }
          XmlNode xmlNode2;
          if (str1.IndexOf("[@") > 0)
          {
            string str2 = str1.Substring(str1.IndexOf("[@") + 2);
            string str3 = str2.Substring(0, str2.LastIndexOf("]"));
            string str4 = str3.Substring(str3.IndexOf("=") + 1);
            string name1 = str3.Substring(0, str3.LastIndexOf("="));
            string name2 = str1.Substring(0, str1.LastIndexOf("[@"));
            string xpath = name2 + "[@" + name1 + " = \"" + str4 + "\"]";
            xmlNode2 = xmlNode1.SelectSingleNode(xpath);
            if (xmlNode2 == null)
            {
              XmlNode element = (XmlNode) this.xml.CreateElement(name2);
              xmlNode2 = xmlNode1.AppendChild(element);
              ((XmlElement) xmlNode2).SetAttribute(name1, str4);
            }
          }
          else if (str1.IndexOf("[") != -1)
          {
            int num = this.intValue(str1.Substring(str1.LastIndexOf("[") + 1));
            string str5 = str1.Substring(0, str1.LastIndexOf("["));
            XmlNodeList xmlNodeList;
            for (xmlNodeList = xmlNode1.SelectNodes(str5); num > xmlNodeList.Count; xmlNodeList = xmlNode1.SelectNodes(str5))
            {
              XmlNode element = (XmlNode) this.xml.CreateElement(str5);
              xmlNode1.AppendChild(element);
            }
            xmlNode2 = xmlNodeList.Item(num - 1);
          }
          else
          {
            xmlNode2 = index != 0 ? xmlNode1.SelectSingleNode(str1) : xmlNode1.SelectSingleNode(str1);
            if (xmlNode2 == null)
            {
              XmlNode element = (XmlNode) this.xml.CreateElement(str1);
              xmlNode2 = xmlNode1.AppendChild(element);
            }
          }
          xmlNode1 = xmlNode2;
        }
        xmlNode1.InnerText = newValue;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Error writing to XML data: " + (object) ex);
      }
    }

    public bool RemoveSettlementServiceProviderAt(int i)
    {
      return this.loanData.RemoveSettlementServiceProviderAt(i);
    }

    public int NewSettlementServiceProvider() => this.loanData.NewSettlementServiceProvider();

    private int intValue(string val)
    {
      if (val == string.Empty)
        return 0;
      try
      {
        return int.Parse(val.Replace(",", string.Empty).Replace("]", string.Empty));
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    public string ProductPricingPartnerSetting(string partnerID, string property)
    {
      this.writeMethodCall("ProductPricingPartner");
      ProductPricingSetting productPricingPartner = Session.ConfigurationManager.GetProductPricingPartner(partnerID);
      if (productPricingPartner == null)
        return "";
      switch (property)
      {
        case "AdminURL":
          return productPricingPartner.AdminURL;
        case "AllowImportToLoan":
          return !productPricingPartner.ImportToLoan ? "n" : "y";
        case "AllowLockAndConfirm":
          return !productPricingPartner.PartnerLockConfirm ? "n" : "y";
        case "AllowRequestLock":
          return !productPricingPartner.PartnerRequestLock ? "n" : "y";
        case "EnableAutoLockRelock":
          return !Session.LoanDataMgr.AllowAutoLock(false, true, false, false, true) ? "n" : "y";
        case "EnableAutoLockRequest":
          return !Session.LoanDataMgr.AllowAutoLock(false, false, false, false, false) ? "n" : "y";
        case "IsCustomizeInvestorName":
          return !productPricingPartner.IsCustomizeInvestorName ? "n" : "y";
        case "MoreInfoURL":
          return productPricingPartner.MoreInfoURL;
        case "PartnerName":
          return productPricingPartner.PartnerName;
        case "SettingsSection":
          return productPricingPartner.SettingsSection;
        case "ShowSellSide":
          return !productPricingPartner.ShowSellSide ? "n" : "y";
        case "UseCustomizedInvestorName":
          return !productPricingPartner.UseCustomizedInvestorName ? "n" : "y";
        case "UseInvestorAndLenderName":
          return !productPricingPartner.UseInvestorAndLenderName ? "n" : "y";
        case "UseOnlyInvestorName":
          return !productPricingPartner.UseOnlyInvestorName ? "n" : "y";
        case "UseOnlyLenderName":
          return !productPricingPartner.UseOnlyLenderName ? "n" : "y";
        default:
          return "";
      }
    }

    public string GetDefaultProductPricingPartnerID()
    {
      this.writeMethodCall(nameof (GetDefaultProductPricingPartnerID));
      ProductPricingSetting productPricingPartner = Session.ConfigurationManager.GetActiveProductPricingPartner();
      return productPricingPartner == null ? "" : productPricingPartner.PartnerID;
    }

    public void ProductPricingCreateLockRequest()
    {
      this.writeMethodCall(nameof (ProductPricingCreateLockRequest));
      InputHandlerUtil inputHandlerUtil = new InputHandlerUtil(Session.DefaultInstance);
      try
      {
        inputHandlerUtil.SendLockRequest(true);
      }
      catch (OverDailyLimitRateLockRejectedException ex)
      {
        throw new LockDeskClosedException(ex.Message, (Exception) ex);
      }
    }

    public void ProductPricingCreateLockRequestWithAction(string productAndPricingData)
    {
      this.writeMethodCall(nameof (ProductPricingCreateLockRequestWithAction));
      string requestGUID = new InputHandlerUtil(Session.DefaultInstance).SendLockRequest(true);
      LockRequestLog lockRequest = Session.LoanData.GetLogList().GetLockRequest(requestGUID);
      Hashtable lockRequestSnapshot = lockRequest.GetLockRequestSnapshot();
      ProductPricingParser productPricingParser = new ProductPricingParser(productAndPricingData, lockRequestSnapshot);
      Hashtable mergedData = productPricingParser.MergedData;
      new LockRequestCalculator(Session.SessionObjects, this.LoanData).PerformSnapshotCalculations(mergedData);
      if (productPricingParser.ExternalActionRequested == ExternalAction.DenyLock)
      {
        Session.LoanDataMgr.DenyRateRequest(lockRequest, mergedData, Session.UserInfo, "Denied from Product and Pricing partner site.");
      }
      else
      {
        if (productPricingParser.ExternalActionRequested != ExternalAction.LockAndConfirm)
          return;
        Session.LoanDataMgr.LockRateRequest(lockRequest, mergedData, Session.UserInfo, true);
      }
    }

    public bool ProductPricingIsHistoricalPricingEnabled()
    {
      return ProductPricingUtils.IsHistoricalPricingEnabled;
    }

    public void SelectPlanCode()
    {
      this.writeMethodCall(nameof (SelectPlanCode));
      Session.Application.GetService<ILoanServices>().SelectPlanCode();
    }

    public void SelectAltLender()
    {
      this.writeMethodCall(nameof (SelectAltLender));
      Session.Application.GetService<ILoanServices>().SelectAltLender();
    }

    public void AuditClosingDocs()
    {
      this.writeMethodCall(nameof (AuditClosingDocs));
      Session.Application.GetService<ILoanServices>().AuditClosingDocs();
    }

    public void OrderClosingDocs()
    {
      this.writeMethodCall(nameof (OrderClosingDocs));
      Session.Application.GetService<ILoanServices>().OrderClosingDocs();
    }

    public void AuditDisclosures() => this.writeMethodCall(nameof (AuditDisclosures));

    public void OrderDisclosures()
    {
      this.writeMethodCall(nameof (OrderDisclosures));
      Session.Application.GetService<ILoanServices>().OrderDisclosures();
    }

    public void ViewClosingDocs()
    {
      this.writeMethodCall(nameof (ViewClosingDocs));
      DocumentOrderLog[] documentOrdersByType = this.LoanData.GetLogList().GetDocumentOrdersByType(DocumentOrderType.Closing);
      if (documentOrdersByType.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "There are no closing document sets saved with this loan.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        Session.Application.GetService<ILoanServices>().ViewClosingDocs(documentOrdersByType[documentOrdersByType.Length - 1].Guid);
    }

    public bool IsEncompassClosingDocsSolutionAvailable()
    {
      using (DocEngineService docEngineService = new DocEngineService(Session.SessionObjects))
        return DocEngineUtils.IsActiveStatus(docEngineService.GetClientAccountStatus().ClosingDocsStatus);
    }

    public bool IsEncompassOpeningDocsSolutionAvailable()
    {
      using (DocEngineService docEngineService = new DocEngineService(Session.SessionObjects))
        return DocEngineUtils.IsActiveStatus(docEngineService.GetClientAccountStatus().OpeningDocsStatus);
    }

    private EDisclosureSetup getEDisclosureSetup()
    {
      if (this.edisclosureSetup == null)
        this.edisclosureSetup = this.loanDataMgr.SessionObjects.ConfigurationManager.GetEDisclosureSetup();
      return this.edisclosureSetup;
    }

    public void ShowLockRequestProcessDialog()
    {
      using (LockRequestProcessDialog requestProcessDialog = new LockRequestProcessDialog())
      {
        if (requestProcessDialog.ShowDialog((IWin32Window) Session.Application) != DialogResult.OK || !requestProcessDialog.CloseLoanFile)
          return;
        Session.Application.GetService<ILoanConsole>().CloseLoan(false);
      }
    }

    public void LogTransaction(string vendorUrl, string transID, string miscData)
    {
      this.writeMethodCall(nameof (LogTransaction), (object) vendorUrl, (object) transID, (object) miscData);
      EpassTransactionService transactionService = new EpassTransactionService(this.loanDataMgr);
      if (this.loanDataMgr != null)
        transactionService.SendTransactionLog(vendorUrl, transID, miscData);
      else
        transactionService.SendTransactionLog(Session.CompanyInfo.ClientID, Session.UserInfo.Userid, vendorUrl, transID, miscData);
    }

    public Form GetEncompassApplicationWindow() => Session.MainForm;

    public string AddPartnerDocumentLog(
      string title,
      string company,
      string dateRequested,
      string dateExpected,
      string comments,
      string epassSignature)
    {
      if (this.GetDocumentID(title, company) != null)
        throw new ArgumentException("A document with the specified title and company already exists");
      return this.AddDocumentLog(title, company, dateRequested, dateExpected, "", comments, epassSignature);
    }

    public string GetDocumentID(string title, string company)
    {
      this.writeMethodCall(nameof (GetDocumentID), (object) title, (object) company);
      LogList logList = this.loanData.GetLogList();
      string documentId = (string) null;
      foreach (DocumentLog allDocument in logList.GetAllDocuments())
      {
        if (string.Compare(allDocument.Title, title, true) == 0 && string.Compare(allDocument.RequestedFrom, company, true) == 0)
          documentId = allDocument.Guid;
      }
      return documentId;
    }

    public bool DoesDocumentLogExist(string docID)
    {
      this.writeMethodCall(nameof (DoesDocumentLogExist), (object) docID);
      return this.getDocumentLog(docID) != null;
    }

    public bool DoesAttachmentExist(string attachmentID)
    {
      this.writeMethodCall(nameof (DoesAttachmentExist), (object) attachmentID);
      return this.loanDataMgr.FileAttachments[attachmentID] != null;
    }

    public string GetAttachmentID(string docID, string title)
    {
      this.writeMethodCall(nameof (GetAttachmentID), (object) docID, (object) title);
      DocumentLog documentLog = this.getDocumentLog(docID);
      if (documentLog == null)
        return (string) null;
      foreach (FileAttachment attachment in this.loanDataMgr.FileAttachments.GetAttachments(documentLog))
      {
        if (string.Compare(attachment.Title, title, true) == 0)
          return attachment.ID;
      }
      return (string) null;
    }

    public bool MarkDocumentLogAsReceived(string docID, string dateReceivedStr, string comments)
    {
      this.writeMethodCall(nameof (MarkDocumentLogAsReceived), (object) docID, (object) dateReceivedStr, (object) comments);
      DocumentLog documentLog = this.getDocumentLog(docID);
      if (documentLog == null)
        return false;
      if (!string.IsNullOrEmpty(dateReceivedStr))
        documentLog.MarkAsReceived(Utils.ParseDate((object) dateReceivedStr), this.GetUserID());
      if (!string.IsNullOrEmpty(comments))
        documentLog.Comments.Add(comments, this.GetUserID(), this.GetUserFullName());
      return true;
    }

    public string[] GetCompanyDocumentList(string company)
    {
      this.writeMethodCall("GetCompanyDocuments", (object) company);
      List<string> stringList = new List<string>();
      foreach (DocumentLog allDocument in this.loanData.GetLogList().GetAllDocuments())
      {
        if (string.Compare(allDocument.RequestedFrom, company, true) == 0)
          stringList.Add(allDocument.Guid);
      }
      return stringList.ToArray();
    }

    public DateTime GetDocumentDateRequested(string docID)
    {
      this.writeMethodCall(nameof (GetDocumentDateRequested), (object) docID);
      DocumentLog documentLog = this.getDocumentLog(docID);
      return documentLog != null ? documentLog.DateRequested : DateTime.MinValue;
    }

    public string GetDisplayVersion()
    {
      this.writeMethodCall(nameof (GetDisplayVersion));
      return VersionInformation.CurrentVersion.DisplayVersion;
    }

    public string GetLicenseEdition()
    {
      this.writeMethodCall(nameof (GetLicenseEdition));
      return Session.EncompassEdition.ToString();
    }

    public void SaveCustomBinary(string key, byte[] data)
    {
      this.writeMethodCall(nameof (SaveCustomBinary), (object) key, (object) data.Length);
      this.loanDataMgr.SaveSupportingData(key, new BinaryObject(data));
    }

    public void AppendCustomBinary(string key, byte[] data)
    {
      this.writeMethodCall(nameof (AppendCustomBinary), (object) key, (object) data.Length);
      this.loanDataMgr.AppendSupportingData(key, new BinaryObject(data));
    }

    public int GetPairIndex()
    {
      this.writeMethodCall(nameof (GetPairIndex));
      return this.loanData.GetPairIndex(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
    }

    public void SetBorrowerPair(string pairId)
    {
      this.writeMethodCall(nameof (SetBorrowerPair), (object) pairId);
      this.loanData.SetBorrowerPair(this.loanData.GetBorrowerPair(pairId) ?? throw new ArgumentException("The specified PairID is invalid in the loan"));
    }

    public object GetNativeField(string id)
    {
      this.writeMethodCall(nameof (GetNativeField), (object) id);
      string simpleField = this.loanData.GetSimpleField(id);
      FieldFormat format = this.loanData.GetFormat(id);
      try
      {
        return Utils.ConvertToNativeValue(simpleField, format, true);
      }
      catch
      {
        return (object) null;
      }
    }

    public object GetNativeField(string id, int pairIndex)
    {
      this.writeMethodCall(nameof (GetNativeField), (object) id, (object) pairIndex);
      string simpleField = this.loanData.GetSimpleField(id, pairIndex);
      FieldFormat format = this.loanData.GetFormat(id);
      try
      {
        return Utils.ConvertToNativeValue(simpleField, format, true);
      }
      catch
      {
        return (object) null;
      }
    }

    public void SetNativeField(string id, object val)
    {
      this.writeMethodCall(nameof (SetNativeField), (object) id, (object) string.Concat(val));
      if (Session.LoanDataMgr != null)
        Session.LoanDataMgr.ValidationsEnabled = false;
      try
      {
        string val1 = (this.loanData.GetFieldDefinition(id) ?? throw new ArgumentException("The value '" + id + "' is not a recognized Field ID")).ValidateInput(string.Concat(val));
        this.loanData.SetField(id, val1);
      }
      finally
      {
        if (Session.LoanDataMgr != null)
          Session.LoanDataMgr.ValidationsEnabled = true;
      }
    }

    public bool IsExportServiceAccessible(string exportName)
    {
      this.writeMethodCall(nameof (IsExportServiceAccessible), (object) exportName);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      return service.IsExportServiceAccessible(this.loanDataMgr, ServicesMapping.GetServiceSettingFromFileName(exportName) ?? throw new Exception("Failed to find any Export service with file name: " + exportName));
    }

    public bool ExportServiceProcessLoans(string exportName, string[] loanGuids)
    {
      this.writeMethodCall(nameof (ExportServiceProcessLoans), (object) exportName, (object) loanGuids);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      return service.ExportServiceProcessLoans(this.loanDataMgr, ServicesMapping.GetServiceSettingFromFileName(exportName) ?? throw new Exception("Failed to find any Export service with file name: " + exportName), loanGuids);
    }

    public bool ExportServiceValidateLoan(string exportName, string loanGuid)
    {
      this.writeMethodCall(nameof (ExportServiceValidateLoan), (object) exportName, (object) loanGuid);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      return service.ExportServiceValidateLoan(this.loanDataMgr, ServicesMapping.GetServiceSettingFromFileName(exportName) ?? throw new Exception("Failed to find any Export service with file name: " + exportName), loanGuid);
    }

    public void ExportServiceProcessLoan(string exportName, string loanGuid)
    {
      this.writeMethodCall(nameof (ExportServiceProcessLoan), (object) exportName, (object) loanGuid);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      service.ExportServiceProcessLoan(this.loanDataMgr, ServicesMapping.GetServiceSettingFromFileName(exportName) ?? throw new Exception("Failed to find any Export service with file name: " + exportName), loanGuid);
    }

    public bool ExportServiceExportData(string exportName, string[] loanGuids)
    {
      this.writeMethodCall(nameof (ExportServiceExportData), (object) exportName, (object) loanGuids);
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      return service.ExportServiceExportData(this.loanDataMgr, ServicesMapping.GetServiceSettingFromFileName(exportName) ?? throw new Exception("Failed to find any Export service with file name: " + exportName), loanGuids);
    }

    public void Recalculate()
    {
      this.writeMethodCall(nameof (Recalculate));
      Session.LoanDataMgr.Calculator.CalculateAll();
    }

    public void ExecCalculation(string calcName)
    {
      this.writeMethodCall(nameof (ExecCalculation));
      Session.LoanDataMgr.Calculator.FormCalculation(calcName, (string) null, (string) null);
    }

    public byte[] GetCompanyCustomDataObject(string name)
    {
      this.writeMethodCall(nameof (GetCompanyCustomDataObject), (object) name);
      using (BinaryObject customDataObject = Session.ConfigurationManager.GetCustomDataObject(name))
        return customDataObject?.GetBytes();
    }

    public void SetCompanyCustomDataObject(string name, byte[] data)
    {
      this.writeMethodCall(nameof (SetCompanyCustomDataObject), (object) name);
      Session.ConfigurationManager.SaveCustomDataObject(name, new BinaryObject(data));
    }

    public void AppendToCompanyCustomDataObject(string name, byte[] data)
    {
      this.writeMethodCall(nameof (AppendToCompanyCustomDataObject), (object) name);
      Session.ConfigurationManager.AppendToCustomDataObject(name, new BinaryObject(data));
    }

    public void DeleteCompanyCustomDataObject(string name)
    {
      this.writeMethodCall(nameof (DeleteCompanyCustomDataObject), (object) name);
      Session.ConfigurationManager.SaveCustomDataObject(name, (BinaryObject) null);
    }

    public string ImportAUSTrackingHistory(
      string xmlString,
      string submissionDate,
      bool copyDefaultLoanDataToLog,
      bool forDU)
    {
      string str = this.loanData.ImportAUSTrackingHistory(xmlString, submissionDate, copyDefaultLoanDataToLog, forDU);
      if (!string.IsNullOrEmpty(str))
        Session.Application.GetService<ILoanEditor>()?.RefreshContents();
      return str;
    }

    public string[] GetAUSTrackingHistoryGUIDs(bool includeManualEntry)
    {
      return this.loanData.GetAUSTrackingHistoryGUIDs(includeManualEntry);
    }

    public string GetAUSTrackingHistory(string historyGUID)
    {
      return this.loanData.GetAUSTrackingHistory(historyGUID);
    }

    public string GetSsoToken(IEnumerable<string> services, int expirationInMinutes)
    {
      return Session.IdentityManager.GetSsoToken(services, expirationInMinutes);
    }

    public string GetAccessToken(string scope)
    {
      try
      {
        return new OAuth2(Session.DefaultInstance.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects), CacheItemRetentionPolicy.NoRetention).GetAccessToken(Session.DefaultInstance.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, scope).TypeAndToken;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Verbose, nameof (Bam), "GetAccessToken: Error: " + ex.Message);
        return (string) null;
      }
    }

    public string GetOAPIGatewayBaseUri() => Session.StartupInfo.OAPIGatewayBaseUri;

    public string GetEpc2HostAdapterUrl() => Session.StartupInfo.Epc2HostAdapterUrl;

    public bool IsHosted() => Session.StartupInfo.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted;

    public bool CanSendConsent()
    {
      this.writeMethodCall(nameof (CanSendConsent));
      return new eFolderAccessRights(this.loanDataMgr).CanSendConsent;
    }

    public string GetFullVersion()
    {
      this.writeMethodCall(nameof (GetFullVersion));
      return VersionInformation.CurrentVersion.GetExtendedVersionWithHotfix(Session.EncompassEdition);
    }

    public List<KeyValuePair<string, string>> GetPreliminaryConditionLogs()
    {
      this.writeMethodCall(nameof (GetPreliminaryConditionLogs));
      List<KeyValuePair<string, string>> preliminaryConditionLogs = new List<KeyValuePair<string, string>>();
      try
      {
        if (this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary) is PreliminaryConditionLog[] allConditions)
          preliminaryConditionLogs = ((IEnumerable<PreliminaryConditionLog>) allConditions).Select<PreliminaryConditionLog, KeyValuePair<string, string>>((System.Func<PreliminaryConditionLog, KeyValuePair<string, string>>) (conditions => new KeyValuePair<string, string>(conditions.Title, conditions.Description))).ToList<KeyValuePair<string, string>>();
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to get Preliminary Conditions: " + (object) ex);
      }
      return preliminaryConditionLogs;
    }

    public void SaveLoan(bool triggerCalcAll)
    {
      this.writeMethodCall(nameof (SaveLoan), (object) triggerCalcAll);
      Bam.LinkAttachments(this.loanDataMgr, (DocumentLog) null);
      this.loanDataMgr.Save(false, triggerCalcAll);
    }

    public void SaveLoan(bool triggerCalcAll, bool throwException)
    {
      this.writeMethodCall(nameof (SaveLoan), (object) triggerCalcAll, (object) throwException);
      Bam.LinkAttachments(this.loanDataMgr, (DocumentLog) null);
      this.loanDataMgr.Save(false, triggerCalcAll, false, throwException);
    }

    public DisclosureTrackingRecord2015[] GetDisclosureTracking2015Records()
    {
      this.writeMethodCall(nameof (GetDisclosureTracking2015Records));
      this.loanData.GetSnapshotDataForAllDisclosureTracking2015LogsForLoan();
      DisclosureTracking2015Log[] disclosureTracking2015Log = this.loanData.GetLogList().GetAllDisclosureTracking2015Log(true);
      List<DisclosureTrackingRecord2015> trackingRecord2015List = new List<DisclosureTrackingRecord2015>();
      foreach (DisclosureTracking2015Log log in disclosureTracking2015Log)
        trackingRecord2015List.Add(this.TransformToBam2015Log(log));
      return trackingRecord2015List.ToArray();
    }

    private DisclosedMethodEnum TranformToBam2015Method(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      DisclosedMethodEnum bam2015Method = DisclosedMethodEnum.ByMail;
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          bam2015Method = DisclosedMethodEnum.None;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          bam2015Method = DisclosedMethodEnum.eDisclosure;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          bam2015Method = DisclosedMethodEnum.Fax;
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          bam2015Method = DisclosedMethodEnum.InPerson;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          bam2015Method = DisclosedMethodEnum.Other;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          bam2015Method = DisclosedMethodEnum.Email;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          bam2015Method = DisclosedMethodEnum.Phone;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          bam2015Method = DisclosedMethodEnum.Signature;
          break;
      }
      return bam2015Method;
    }

    private DisclosureTrackingRecord2015 TransformToBam2015Log(DisclosureTracking2015Log log)
    {
      DisclosureTrackingRecord2015 bam2015Log = new DisclosureTrackingRecord2015();
      bam2015Log.LEDisclosedByBroker = log.LEDisclosedByBroker;
      bam2015Log.DateAdded = log.DateAdded;
      bam2015Log.DisclosureTypeName = log.DisclosureTypeName;
      bam2015Log.LockedDisclosedDateField = log.LockedDisclosedDateField;
      bam2015Log.OriginalDisclosedDate = log.OriginalDisclosedDate;
      bam2015Log.DisclosedDate = log.DisclosedDate;
      bam2015Log.ProviderListSent = log.ProviderListSent;
      bam2015Log.DisclosedBy = log.DisclosedBy;
      bam2015Log.DisclosedByFullName = log.DisclosedByFullName;
      bam2015Log.LockedDisclosedByField = log.LockedDisclosedByField;
      bam2015Log.DisclosureMethod = this.TranformToBam2015Method(log.DisclosureMethod);
      bam2015Log.DisclosedMethodOther = log.DisclosedMethodOther;
      bam2015Log.BorrowerPairID = log.BorrowerPairID;
      bam2015Log.DisclosedMethodName = log.DisclosedMethodName;
      bam2015Log.ReceivedDate = log.ReceivedDate;
      bam2015Log.Received = log.Received;
      bam2015Log.IsDisclosedReceivedDateLocked = log.IsDisclosedReceivedDateLocked;
      bam2015Log.DisclosedAPR = log.DisclosedAPR;
      bam2015Log.FinanceCharge = log.FinanceCharge;
      bam2015Log.DisclosedDailyInterest = log.DisclosedDailyInterest;
      bam2015Log.ApplicationDate = log.ApplicationDate;
      bam2015Log.BorrowerName = log.BorrowerName;
      bam2015Log.CoBorrowerName = log.CoBorrowerName;
      bam2015Log.IsDisclosed = log.IsDisclosed;
      bam2015Log.IsLocked = log.IsLocked;
      bam2015Log.PropertyAddress = log.PropertyAddress;
      bam2015Log.PropertyCity = log.PropertyCity;
      bam2015Log.PropertyState = log.PropertyState;
      bam2015Log.PropertyZip = log.PropertyZip;
      bam2015Log.LoanProgram = log.LoanProgram;
      bam2015Log.LoanAmount = log.LoanAmount;
      bam2015Log.IsManuallyCreated = log.IsManuallyCreated;
      bam2015Log.IsDisclosedByLocked = log.IsDisclosedByLocked;
      bam2015Log.DisclosedForCD = log.DisclosedForCD;
      bam2015Log.DisclosedForLE = log.DisclosedForLE;
      bam2015Log.DisclosedForSafeHarbor = log.DisclosedForSafeHarbor;
      bam2015Log.IsDisclosedAPRLocked = log.IsDisclosedAPRLocked;
      bam2015Log.IsDisclosedFinanceChargeLocked = log.IsDisclosedFinanceChargeLocked;
      bam2015Log.IsDisclosedDailyInterestLocked = log.IsDisclosedDailyInterestLocked;
      bam2015Log.eDisclosureManualFulfillmentDate = log.eDisclosureManualFulfillmentDate;
      bam2015Log.eDisclosureManualFulfillmentMethod = this.TranformToBam2015Method(log.eDisclosureManualFulfillmentMethod);
      bam2015Log.LEReasonIsChangedCircumstanceSettlementCharges = log.LEReasonIsChangedCircumstanceSettlementCharges;
      bam2015Log.LEReasonIsChangedCircumstanceEligibility = log.LEReasonIsChangedCircumstanceEligibility;
      bam2015Log.LEReasonIsRevisionsRequestedByConsumer = log.LEReasonIsRevisionsRequestedByConsumer;
      bam2015Log.LEReasonIsInterestRateDependentCharges = log.LEReasonIsInterestRateDependentCharges;
      bam2015Log.LEReasonIsExpiration = log.LEReasonIsExpiration;
      bam2015Log.LEReasonIsDelayedSettlementOnConstructionLoans = log.LEReasonIsDelayedSettlementOnConstructionLoans;
      bam2015Log.LEReasonIsOther = log.LEReasonIsOther;
      bam2015Log.CDReasonIsChangeInAPR = log.CDReasonIsChangeInAPR;
      bam2015Log.CDReasonIsChangeInLoanProduct = log.CDReasonIsChangeInLoanProduct;
      bam2015Log.CDReasonIsPrepaymentPenaltyAdded = log.CDReasonIsPrepaymentPenaltyAdded;
      bam2015Log.CDReasonIsChangeInSettlementCharges = log.CDReasonIsChangeInSettlementCharges;
      bam2015Log.CDReasonIs24HourAdvancePreview = log.CDReasonIs24HourAdvancePreview;
      bam2015Log.CDReasonIsToleranceCure = log.CDReasonIsToleranceCure;
      bam2015Log.CDReasonIsClericalErrorCorrection = log.CDReasonIsClericalErrorCorrection;
      bam2015Log.CDReasonIsChangedCircumstanceEligibility = log.CDReasonIsChangedCircumstanceEligibility;
      bam2015Log.CDReasonIsRevisionsRequestedByConsumer = log.CDReasonIsRevisionsRequestedByConsumer;
      bam2015Log.CDReasonIsInterestRateDependentCharges = log.CDReasonIsInterestRateDependentCharges;
      bam2015Log.CDReasonIsOther = log.CDReasonIsOther;
      bam2015Log.LEReasonOther = log.LEReasonOther;
      bam2015Log.CDReasonOther = log.CDReasonOther;
      bam2015Log.ChangeInCircumstance = log.ChangeInCircumstance;
      bam2015Log.ChangeInCircumstanceComments = log.ChangeInCircumstanceComments;
      bam2015Log.IntentToProceed = log.IntentToProceed;
      bam2015Log.IntentToProceedDate = log.IntentToProceedDate;
      bam2015Log.IntentToProceedReceivedBy = log.IntentToProceedReceivedBy;
      bam2015Log.LockedIntentReceivedByField = log.LockedIntentReceivedByField;
      bam2015Log.IntentToProceedReceivedMethod = this.TranformToBam2015Method(log.IntentToProceedReceivedMethod);
      bam2015Log.IntentToProceedReceivedMethodOther = log.IntentToProceedReceivedMethodOther;
      bam2015Log.IntentToProceedComments = log.IntentToProceedComments;
      bam2015Log.IsIntentReceivedByLocked = log.IsIntentReceivedByLocked;
      bam2015Log.BorrowerDisclosedMethod = this.TranformToBam2015Method(log.BorrowerDisclosedMethod);
      bam2015Log.BorrowerDisclosedMethodOther = log.BorrowerDisclosedMethodOther;
      bam2015Log.IsBorrowerPresumedDateLocked = log.IsBorrowerPresumedDateLocked;
      bam2015Log.LockedBorrowerPresumedReceivedDate = log.LockedBorrowerPresumedReceivedDate;
      bam2015Log.BorrowerPresumedReceivedDate = log.BorrowerPresumedReceivedDate;
      bam2015Log.BorrowerActualReceivedDate = log.BorrowerActualReceivedDate;
      bam2015Log.BorrowerType = log.BorrowerType;
      bam2015Log.CoBorrowerDisclosedMethod = this.TranformToBam2015Method(log.CoBorrowerDisclosedMethod);
      bam2015Log.CoBorrowerDisclosedMethodOther = log.CoBorrowerDisclosedMethodOther;
      bam2015Log.IsCoBorrowerPresumedDateLocked = log.IsCoBorrowerPresumedDateLocked;
      bam2015Log.LockedCoBorrowerPresumedReceivedDate = log.LockedCoBorrowerPresumedReceivedDate;
      bam2015Log.CoBorrowerPresumedReceivedDate = log.CoBorrowerPresumedReceivedDate;
      bam2015Log.CoBorrowerActualReceivedDate = log.CoBorrowerActualReceivedDate;
      bam2015Log.CoBorrowerType = log.CoBorrowerType;
      bam2015Log.UCD = log.UCD;
      bam2015Log.UseForUCDExport = log.UseForUCDExport;
      bam2015Log.DisclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem disclosedLoanItem in log.DisclosedData)
      {
        if (!bam2015Log.DisclosedFields.ContainsKey(disclosedLoanItem.FieldID))
          bam2015Log.DisclosedFields.Add(disclosedLoanItem.FieldID, disclosedLoanItem.FieldValue);
      }
      if (log.UCD != string.Empty)
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(log.UCD);
        foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
        {
          if (!bam2015Log.DisclosedFields.ContainsKey(keyValuePair.Key))
            bam2015Log.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      return bam2015Log;
    }

    public List<KeyValuePair<string, string>> GetPostClosingConditionLogs()
    {
      this.writeMethodCall(nameof (GetPostClosingConditionLogs));
      List<KeyValuePair<string, string>> closingConditionLogs = new List<KeyValuePair<string, string>>();
      try
      {
        if (this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.PostClosing) is PostClosingConditionLog[] allConditions)
          closingConditionLogs = ((IEnumerable<PostClosingConditionLog>) allConditions).Select<PostClosingConditionLog, KeyValuePair<string, string>>((System.Func<PostClosingConditionLog, KeyValuePair<string, string>>) (conditions => new KeyValuePair<string, string>(conditions.Title, conditions.Description))).ToList<KeyValuePair<string, string>>();
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to get Post-Closing Conditions: " + (object) ex);
      }
      return closingConditionLogs;
    }

    public void AddPostClosingConditionLog(
      string source,
      string title,
      string description,
      string details,
      string pairID,
      string recipient)
    {
      this.writeMethodCall(nameof (AddPostClosingConditionLog), (object) source, (object) title, (object) description, (object) details, (object) pairID, (object) recipient);
      try
      {
        PostClosingConditionLog closingConditionLog = new PostClosingConditionLog(Session.UserID, pairID);
        closingConditionLog.Source = source;
        closingConditionLog.Title = title;
        closingConditionLog.Description = description;
        closingConditionLog.Details = details;
        closingConditionLog.Recipient = recipient;
        PostClosingConditionLog rec = closingConditionLog;
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to add Post-Closing Conditions: " + (object) ex);
      }
    }

    public FeeManagementSetting GetFeeManagementSetting()
    {
      return Session.ConfigurationManager.GetFeeManagement();
    }

    public string GetCurrentFormOrTool()
    {
      return this.loanDataMgr != null ? this.loanDataMgr.CurrentFormOrTool : (string) null;
    }

    public void SaveEPPSClientLoanPrograms(string xmlResponse)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlResponse);
      XmlElement documentElement = xmlDocument.DocumentElement;
      List<EPPSLoanProgram> eppsLoanProgramList = new List<EPPSLoanProgram>();
      foreach (XmlNode childNode in documentElement.ChildNodes)
      {
        string programId = childNode.Attributes["ProgramId"].Value;
        string programName = childNode.Attributes["Program"].Value;
        eppsLoanProgramList.Add(new EPPSLoanProgram(programId, programName));
      }
      if (eppsLoanProgramList == null || eppsLoanProgramList.Count <= 0)
        return;
      Bam.EppsLoanPrograms = eppsLoanProgramList;
    }

    public List<KeyValuePair<string, string>> GetEpassCredentials(
      string userId,
      string partnerSection)
    {
      this.writeMethodCall(nameof (GetEpassCredentials), (object) userId, (object) partnerSection);
      List<KeyValuePair<string, string>> epassCredentials = new List<KeyValuePair<string, string>>();
      try
      {
        List<ePassCredentialSetting> credentialSettings = Session.ConfigurationManager.GetUserePassCredentialSettings("'" + userId + "'");
        if (credentialSettings == null)
          return epassCredentials;
        ePassCredentialSetting credentialSetting = credentialSettings.FirstOrDefault<ePassCredentialSetting>((System.Func<ePassCredentialSetting, bool>) (setting => setting.PartnerSection == partnerSection));
        if (credentialSetting == null)
          return epassCredentials;
        epassCredentials.Add(new KeyValuePair<string, string>("UserId", credentialSetting.UIDValue ?? string.Empty));
        epassCredentials.Add(new KeyValuePair<string, string>("AuthPassword", credentialSetting.PasswordValue ?? string.Empty));
        epassCredentials.Add(new KeyValuePair<string, string>("LpId", credentialSetting.Auth1Value ?? string.Empty));
        epassCredentials.Add(new KeyValuePair<string, string>("TpoNumber", credentialSetting.TPOFieldValue ?? string.Empty));
        epassCredentials.Add(new KeyValuePair<string, string>("LpPassword", credentialSetting.Auth2Value ?? string.Empty));
        return epassCredentials;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to get LPA credentials: " + ex.Message);
        return epassCredentials;
      }
    }

    public List<KeyValuePair<string, string>> GetEpassCredentialsForAll(
      string userId,
      string partnerSection)
    {
      this.writeMethodCall(nameof (GetEpassCredentialsForAll), (object) userId, (object) partnerSection);
      List<KeyValuePair<string, string>> credentialsForAll = new List<KeyValuePair<string, string>>();
      try
      {
        List<ePassCredentialSetting> credentialSettings = Session.ConfigurationManager.GetUserePassCredentialSettings("'" + userId + "'");
        if (credentialSettings == null)
          return credentialsForAll;
        ePassCredentialSetting credentialSetting = credentialSettings.FirstOrDefault<ePassCredentialSetting>((System.Func<ePassCredentialSetting, bool>) (setting => setting.PartnerSection == partnerSection));
        if (credentialSetting == null)
          return credentialsForAll;
        credentialsForAll.Add(new KeyValuePair<string, string>("UIDValue", credentialSetting.UIDValue ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("Auth1Value", credentialSetting.Auth1Value ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("Auth2Value", credentialSetting.Auth2Value ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("Category", credentialSetting.Category ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("Description", credentialSetting.Description ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("PasswordValue", credentialSetting.PasswordValue ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("SaveLoginValue", credentialSetting.SaveLoginValue ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("Title", credentialSetting.Title ?? string.Empty));
        credentialsForAll.Add(new KeyValuePair<string, string>("TPOFieldValue", credentialSetting.TPOFieldValue ?? string.Empty));
        return credentialsForAll;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, nameof (Bam), TraceLevel.Error, "Unable to get ePass credentials: " + ex.Message);
        return credentialsForAll;
      }
    }

    public string EncompassServerUrl => Session.RemoteServer;

    public void SaveTitleFeeCredentials(string orderID, string loanGUID, string credentials)
    {
      Session.ConfigurationManager.SaveTitleFeeCredentials(orderID, loanGUID, credentials);
    }

    public string[] GetTitleFeeCredentials(string[] orderIDs, string loanGUID)
    {
      return Session.ConfigurationManager.GetTitleFeeCredentials(orderIDs, loanGUID);
    }

    public void RefreshContents()
    {
      Session.Application.GetService<ILoanEditor>().RefreshContents();
    }

    private string getBorrowerValue(string pairID)
    {
      if (pairID == BorrowerPair.All.Id)
        return BorrowerPair.All.ToString();
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        if (borrowerPair.Id == pairID)
          return borrowerPair.ToString();
      }
      return (string) null;
    }

    public Dictionary<DocumentLogInfo, FileAttachment[]> GetDocumentAttachmentsForCLO()
    {
      Dictionary<DocumentLogInfo, FileAttachment[]> attachmentsForClo = new Dictionary<DocumentLogInfo, FileAttachment[]>();
      foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
      {
        if (allDocument.IsThirdPartyDoc && this.loanDataMgr.FileAttachments.ContainsAttachment(allDocument))
        {
          FileAttachment[] attachments = this.loanDataMgr.FileAttachments.GetAttachments(new DocumentLog[1]
          {
            allDocument
          });
          if (attachments != null && attachments.Length != 0)
            attachmentsForClo.Add(new DocumentLogInfo(allDocument.Guid, allDocument.Title, allDocument.RequestedFrom, this.getBorrowerValue(allDocument.PairId)), attachments);
        }
      }
      return attachmentsForClo;
    }

    public string ExportDocumentFile(string docID, FileAttachment attachment)
    {
      this.writeMethodCall(nameof (ExportDocumentFile), (object) docID, (object) attachment.Title);
      return this.getDocumentLog(docID) == null ? (string) null : Session.Application.GetService<IEFolder>().Export(this.loanDataMgr, attachment);
    }

    public BizPartnerInfo ShowRolodex(string category)
    {
      this.writeMethodCall(nameof (ShowRolodex), (object) category);
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact(category, false, false))
        return rxBusinessContact.ShowDialog() == DialogResult.OK ? rxBusinessContact.SelectedPartnerInfo : (BizPartnerInfo) null;
    }

    public RoleInfo GetRealWorldRoleMapping(RealWorldRoleID realWorldRoleID)
    {
      WorkflowManager bpmManager = (WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow);
      RolesMappingInfo roleMappingInfo = bpmManager.GetRoleMappingInfo(realWorldRoleID);
      return roleMappingInfo != null && roleMappingInfo.RoleIDList != null && roleMappingInfo.RoleIDList.Length != 0 ? bpmManager.GetRoleFunction(roleMappingInfo.RoleIDList[0]) : (RoleInfo) null;
    }

    public UserInfo GetRealWorldRoleContact(RealWorldRoleID realWorldRoleID)
    {
      UserInfo worldRoleContact = (UserInfo) null;
      if (this.GetRealWorldRoleMapping(realWorldRoleID) != null)
      {
        string userID = string.Empty;
        switch (realWorldRoleID)
        {
          case RealWorldRoleID.LoanOfficer:
            userID = this.loanDataMgr.GetCurrentLOInLoanData();
            break;
          case RealWorldRoleID.LoanProcessor:
            userID = this.loanDataMgr.GetCurrentLPInLoanData();
            break;
          case RealWorldRoleID.LoanCloser:
            userID = this.loanDataMgr.GetCurrentCLInLoanData();
            break;
          case RealWorldRoleID.Underwriter:
            userID = this.loanDataMgr.GetCurrentUWInLoanData();
            break;
        }
        worldRoleContact = this.getUserInfo(userID);
      }
      return worldRoleContact;
    }

    public void AddFieldLock(string fieldID)
    {
      this.writeMethodCall(nameof (AddFieldLock), (object) fieldID);
      this.loanData.AddLock(fieldID);
    }

    public void RemoveFieldLock(string fieldID)
    {
      this.writeMethodCall(nameof (RemoveFieldLock), (object) fieldID);
      this.loanData.RemoveLock(fieldID);
    }

    public bool IsFieldLocked(string fieldID)
    {
      this.writeMethodCall(nameof (IsFieldLocked), (object) fieldID);
      return this.loanData.IsLocked(fieldID);
    }

    public int AddGSERepWarrantTracker()
    {
      this.writeMethodCall(nameof (AddGSERepWarrantTracker));
      return this.loanData.NewGSERepWarrantTracker();
    }

    public int GetNumberOfGSERepWarrentTrackers()
    {
      this.writeMethodCall(nameof (GetNumberOfGSERepWarrentTrackers));
      return this.loanData.GetNumberOfGSERepWarrantTrackers();
    }

    public bool RemoveGSERepWarrentTrackers()
    {
      this.writeMethodCall(nameof (RemoveGSERepWarrentTrackers));
      return this.loanData.RemoveGSERepWarrantTrackers();
    }

    public int RemoveGSERepWarrentBlock(int blockID)
    {
      this.writeMethodCall(nameof (RemoveGSERepWarrentBlock), (object) blockID);
      return this.loanData.RemoveGSERepWarrantTrackerAt(blockID);
    }

    public void RetrieveDocument2(string docID, bool showDocumentDetailsDialog)
    {
      DocumentLog documentLog = this.getDocumentLog(docID);
      if (documentLog == null)
        return;
      Session.Application.GetService<IEPass>().Retrieve(documentLog, showDocumentDetailsDialog);
    }

    public int GetNumberOfAdditionalLoans()
    {
      this.writeMethodCall(nameof (GetNumberOfAdditionalLoans));
      return this.loanData.GetNumberOfAdditionalLoans();
    }

    public int GetNumberOfGiftsAndGrants()
    {
      this.writeMethodCall(nameof (GetNumberOfGiftsAndGrants));
      return this.loanData.GetNumberOfGiftsAndGrants();
    }

    public int GetNumberOfOtherIncomeSources()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherIncomeSources));
      return this.loanData.GetNumberOfOtherIncomeSources();
    }

    public int GetNumberOfOtherLiabilities()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherLiabilities));
      return this.loanData.GetNumberOfOtherLiability();
    }

    public int GetNumberOfOtherAssets()
    {
      this.writeMethodCall(nameof (GetNumberOfOtherAssets));
      return this.loanData.GetNumberOfOtherAssets();
    }

    public int GetNumberOfAlternateNames(bool borrower)
    {
      this.writeMethodCall(nameof (GetNumberOfAlternateNames), (object) borrower);
      return this.loanData.GetNumberOfURLAAlternateNames(borrower);
    }

    public void Recalculate(bool skipLockRequestSync)
    {
      this.writeMethodCall(nameof (Recalculate), (object) skipLockRequestSync);
      bool skipLockRequestSync1 = Session.LoanDataMgr.Calculator.SkipLockRequestSync;
      Session.LoanDataMgr.Calculator.SkipLockRequestSync = skipLockRequestSync;
      Session.LoanDataMgr.Calculator.CalculateAll();
      Session.LoanDataMgr.Calculator.SkipLockRequestSync = skipLockRequestSync1;
    }

    public EnhancedDisclosureTrackingRecord2015[] GetDisclosureTracking2015EnhancedRecords()
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = Session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      List<EnhancedDisclosureTrackingRecord2015> trackingRecord2015List = new List<EnhancedDisclosureTrackingRecord2015>();
      foreach (IDisclosureTracking2015Log log in idisclosureTracking2015Log)
        trackingRecord2015List.Add(this.transformToBam2015Enhanced(log));
      return trackingRecord2015List.ToArray();
    }

    public EnhancedDisclosureTrackingRecord2015 transformToBam2015Enhanced(
      IDisclosureTracking2015Log log)
    {
      EnhancedDisclosureTrackingRecord2015 bam2015Enhanced = new EnhancedDisclosureTrackingRecord2015();
      bam2015Enhanced.LEDisclosedByBroker = log.LEDisclosedByBroker;
      bam2015Enhanced.DisclosureTypeName = log.DisclosureTypeName;
      bam2015Enhanced.LockedDisclosedDateField = log.LockedDisclosedDateField;
      bam2015Enhanced.OriginalDisclosedDate = log.OriginalDisclosedDate;
      bam2015Enhanced.DisclosedDate = log.DisclosedDate;
      bam2015Enhanced.ProviderListSent = log.ProviderListSent;
      bam2015Enhanced.DisclosedBy = log.DisclosedBy;
      bam2015Enhanced.DisclosedByFullName = log.DisclosedByFullName;
      bam2015Enhanced.LockedDisclosedByField = log.LockedDisclosedByField;
      bam2015Enhanced.DisclosureMethod = this.tranformToBam2015Method(log.DisclosureMethod);
      bam2015Enhanced.DisclosedMethodOther = log.DisclosedMethodOther;
      bam2015Enhanced.BorrowerPairID = log.BorrowerPairID;
      bam2015Enhanced.DisclosedMethodName = log.DisclosedMethodName;
      bam2015Enhanced.ReceivedDate = log.ReceivedDate;
      bam2015Enhanced.Received = log.Received;
      bam2015Enhanced.IsDisclosedReceivedDateLocked = log.IsDisclosedReceivedDateLocked;
      bam2015Enhanced.DisclosedAPR = log.DisclosedAPR;
      bam2015Enhanced.FinanceCharge = log.FinanceCharge;
      bam2015Enhanced.DisclosedDailyInterest = log.DisclosedDailyInterest;
      bam2015Enhanced.IsDisclosed = log.IsDisclosed;
      bam2015Enhanced.IsLocked = log.IsLocked;
      bam2015Enhanced.IsDisclosedByLocked = log.IsDisclosedByLocked;
      bam2015Enhanced.IsDisclosedAPRLocked = log.IsDisclosedAPRLocked;
      bam2015Enhanced.DisclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem disclosedLoanItem in log.DisclosedData)
      {
        if (!bam2015Enhanced.DisclosedFields.ContainsKey(disclosedLoanItem.FieldID))
          bam2015Enhanced.DisclosedFields.Add(disclosedLoanItem.FieldID, disclosedLoanItem.FieldValue);
      }
      bam2015Enhanced.UseForUCDExport = log.UseForUCDExport;
      if (log is EnhancedDisclosureTracking2015Log)
      {
        EnhancedDisclosureTracking2015Log disclosureTracking2015Log = (EnhancedDisclosureTracking2015Log) log;
        bam2015Enhanced.Status = disclosureTracking2015Log.Status.ToString();
        bam2015Enhanced.Provider = disclosureTracking2015Log.Provider;
        bam2015Enhanced.LoanAmount = disclosureTracking2015Log.LoanAmount.ToString();
        bam2015Enhanced.LoanProgram = disclosureTracking2015Log.LoanProgram;
        bam2015Enhanced.Contents = disclosureTracking2015Log.Contents.Select<EnhancedDisclosureTracking2015Log.DisclosureContentType, EllieMae.EMLite.ePass.BamObjects.DisclosureContentType>((System.Func<EnhancedDisclosureTracking2015Log.DisclosureContentType, EllieMae.EMLite.ePass.BamObjects.DisclosureContentType>) (c => (EllieMae.EMLite.ePass.BamObjects.DisclosureContentType) c)).ToList<EllieMae.EMLite.ePass.BamObjects.DisclosureContentType>();
        bam2015Enhanced.ApplicationDate = disclosureTracking2015Log.ApplicationDate;
        bam2015Enhanced.IsDisclosedFinanceChargeLocked = disclosureTracking2015Log.DisclosedFinanceCharge.UseUserValue;
        bam2015Enhanced.IsDisclosedDailyInterestLocked = disclosureTracking2015Log.DisclosedDailyInterest.UseUserValue;
        bam2015Enhanced.IsManuallyCreated = disclosureTracking2015Log.Provider == "Manual";
        bam2015Enhanced.DisclosureRecipients = new List<EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient>();
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient1 in (IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureRecipient>) disclosureTracking2015Log.DisclosureRecipients)
        {
          EnhancedDisclosureTracking2015Log.DisclosureRecipientType role1 = disclosureRecipient1.Role;
          DisclosureTrackingBase.DisclosedMethod disclosedMethod1;
          DateTimeWithZone dateTimeWithZone;
          switch (role1)
          {
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
              EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient1 = (EnhancedDisclosureTracking2015Log.BorrowerRecipient) disclosureRecipient1;
              List<EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient> disclosureRecipients1 = bam2015Enhanced.DisclosureRecipients;
              string id1 = borrowerRecipient1.Id;
              string borrowerPairId1 = borrowerRecipient1.BorrowerPairId;
              string name1 = borrowerRecipient1.Name;
              string email1 = borrowerRecipient1.Email;
              role1 = borrowerRecipient1.Role;
              string role2 = role1.ToString();
              string roleDescription1 = borrowerRecipient1.RoleDescription;
              disclosedMethod1 = borrowerRecipient1.DisclosedMethod;
              string disclosedMethod2 = disclosedMethod1.ToString();
              string methodDescription1 = borrowerRecipient1.DisclosedMethodDescription;
              string borrowerType1 = borrowerRecipient1.BorrowerType.UseUserValue ? borrowerRecipient1.BorrowerType.UserValue : borrowerRecipient1.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate1 = borrowerRecipient1.PresumedReceivedDate.UseUserValue ? borrowerRecipient1.PresumedReceivedDate.UserValue : borrowerRecipient1.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate1 = borrowerRecipient1.ActualReceivedDate;
              dateTimeWithZone = borrowerRecipient1.Tracking.AcceptConsentDate;
              DateTime dateTime1 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ESignedDate;
              DateTime dateTime2 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.WetSignedDate;
              DateTime dateTime3 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.RejectConsentDate;
              DateTime dateTime4 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewConsentDate;
              DateTime dateTime5 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewMessageDate;
              DateTime dateTime6 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.AuthenticatedDate;
              DateTime dateTime7 = dateTimeWithZone.DateTime;
              string authenticatedIp1 = borrowerRecipient1.Tracking.AuthenticatedIP;
              string acceptConsentIp1 = borrowerRecipient1.Tracking.AcceptConsentIP;
              string rejectConsentIp1 = borrowerRecipient1.Tracking.RejectConsentIP;
              string esignedIp1 = borrowerRecipient1.Tracking.ESignedIP;
              string loanLevelConsent1 = borrowerRecipient1.Tracking.LoanLevelConsent;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewESignedDate;
              DateTime dateTime8 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.ViewWetSignedDate;
              DateTime dateTime9 = dateTimeWithZone.DateTime;
              dateTimeWithZone = borrowerRecipient1.Tracking.InformationalViewedDate;
              DateTime dateTime10 = dateTimeWithZone.DateTime;
              string informationalViewedIp1 = borrowerRecipient1.Tracking.InformationalViewedIP;
              dateTimeWithZone = borrowerRecipient1.Tracking.InformationalCompletedDate;
              DateTime dateTime11 = dateTimeWithZone.DateTime;
              string informationalCompletedIp1 = borrowerRecipient1.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking tracking1 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dateTime1, dateTime2, dateTime3, dateTime4, dateTime5, dateTime6, dateTime7, authenticatedIp1, acceptConsentIp1, rejectConsentIp1, esignedIp1, loanLevelConsent1, dateTime8, dateTime9, dateTime10, informationalViewedIp1, dateTime11, informationalCompletedIp1);
              EllieMae.EMLite.ePass.BamObjects.BorrowerRecipient borrowerRecipient2 = new EllieMae.EMLite.ePass.BamObjects.BorrowerRecipient(id1, borrowerPairId1, name1, email1, role2, roleDescription1, disclosedMethod2, methodDescription1, borrowerType1, presumedReceivedDate1, actualReceivedDate1, tracking1);
              disclosureRecipients1.Add((EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient) borrowerRecipient2);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
              EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = (EnhancedDisclosureTracking2015Log.CoborrowerRecipient) disclosureRecipient1;
              List<EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient> disclosureRecipients2 = bam2015Enhanced.DisclosureRecipients;
              string id2 = coborrowerRecipient.Id;
              string borrowerPairId2 = coborrowerRecipient.BorrowerPairId;
              string name2 = coborrowerRecipient.Name;
              string email2 = coborrowerRecipient.Email;
              role1 = coborrowerRecipient.Role;
              string role3 = role1.ToString();
              string roleDescription2 = coborrowerRecipient.RoleDescription;
              disclosedMethod1 = coborrowerRecipient.DisclosedMethod;
              string disclosedMethod3 = disclosedMethod1.ToString();
              string methodDescription2 = coborrowerRecipient.DisclosedMethodDescription;
              string borrowerType2 = coborrowerRecipient.BorrowerType.UseUserValue ? coborrowerRecipient.BorrowerType.UserValue : coborrowerRecipient.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate2 = coborrowerRecipient.PresumedReceivedDate.UseUserValue ? coborrowerRecipient.PresumedReceivedDate.UserValue : coborrowerRecipient.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate2 = coborrowerRecipient.ActualReceivedDate;
              dateTimeWithZone = coborrowerRecipient.Tracking.AcceptConsentDate;
              DateTime dateTime12 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ESignedDate;
              DateTime dateTime13 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.WetSignedDate;
              DateTime dateTime14 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.RejectConsentDate;
              DateTime dateTime15 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewConsentDate;
              DateTime dateTime16 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewMessageDate;
              DateTime dateTime17 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.AuthenticatedDate;
              DateTime dateTime18 = dateTimeWithZone.DateTime;
              string authenticatedIp2 = coborrowerRecipient.Tracking.AuthenticatedIP;
              string acceptConsentIp2 = coborrowerRecipient.Tracking.AcceptConsentIP;
              string rejectConsentIp2 = coborrowerRecipient.Tracking.RejectConsentIP;
              string esignedIp2 = coborrowerRecipient.Tracking.ESignedIP;
              string loanLevelConsent2 = coborrowerRecipient.Tracking.LoanLevelConsent;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewESignedDate;
              DateTime dateTime19 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.ViewWetSignedDate;
              DateTime dateTime20 = dateTimeWithZone.DateTime;
              dateTimeWithZone = coborrowerRecipient.Tracking.InformationalViewedDate;
              DateTime dateTime21 = dateTimeWithZone.DateTime;
              string informationalViewedIp2 = coborrowerRecipient.Tracking.InformationalViewedIP;
              dateTimeWithZone = coborrowerRecipient.Tracking.InformationalCompletedDate;
              DateTime dateTime22 = dateTimeWithZone.DateTime;
              string informationalCompletedIp2 = coborrowerRecipient.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking tracking2 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dateTime12, dateTime13, dateTime14, dateTime15, dateTime16, dateTime17, dateTime18, authenticatedIp2, acceptConsentIp2, rejectConsentIp2, esignedIp2, loanLevelConsent2, dateTime19, dateTime20, dateTime21, informationalViewedIp2, dateTime22, informationalCompletedIp2);
              CoBorrowerRecipient borrowerRecipient3 = new CoBorrowerRecipient(id2, borrowerPairId2, name2, email2, role3, roleDescription2, disclosedMethod3, methodDescription2, borrowerType2, presumedReceivedDate2, actualReceivedDate2, tracking2);
              disclosureRecipients2.Add((EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient) borrowerRecipient3);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate:
              EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient2 = disclosureRecipient1;
              List<EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient> disclosureRecipients3 = bam2015Enhanced.DisclosureRecipients;
              string id3 = disclosureRecipient2.Id;
              string name3 = disclosureRecipient2.Name;
              string email3 = disclosureRecipient2.Email;
              role1 = disclosureRecipient2.Role;
              string role4 = role1.ToString();
              string roleDescription3 = disclosureRecipient2.RoleDescription;
              disclosedMethod1 = disclosureRecipient2.DisclosedMethod;
              string disclosedMethod4 = disclosedMethod1.ToString();
              string methodDescription3 = disclosureRecipient2.DisclosedMethodDescription;
              string borrowerType3 = disclosureRecipient2.BorrowerType.UseUserValue ? disclosureRecipient2.BorrowerType.UserValue : disclosureRecipient2.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate3 = disclosureRecipient2.PresumedReceivedDate.UseUserValue ? disclosureRecipient2.PresumedReceivedDate.UserValue : disclosureRecipient2.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate3 = disclosureRecipient2.ActualReceivedDate;
              dateTimeWithZone = disclosureRecipient2.Tracking.AcceptConsentDate;
              DateTime dateTime23 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ESignedDate;
              DateTime dateTime24 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.WetSignedDate;
              DateTime dateTime25 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.RejectConsentDate;
              DateTime dateTime26 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewConsentDate;
              DateTime dateTime27 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewMessageDate;
              DateTime dateTime28 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.AuthenticatedDate;
              DateTime dateTime29 = dateTimeWithZone.DateTime;
              string authenticatedIp3 = disclosureRecipient2.Tracking.AuthenticatedIP;
              string acceptConsentIp3 = disclosureRecipient2.Tracking.AcceptConsentIP;
              string rejectConsentIp3 = disclosureRecipient2.Tracking.RejectConsentIP;
              string esignedIp3 = disclosureRecipient2.Tracking.ESignedIP;
              string loanLevelConsent3 = disclosureRecipient2.Tracking.LoanLevelConsent;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewESignedDate;
              DateTime dateTime30 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.ViewWetSignedDate;
              DateTime dateTime31 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient2.Tracking.InformationalViewedDate;
              DateTime dateTime32 = dateTimeWithZone.DateTime;
              string informationalViewedIp3 = disclosureRecipient2.Tracking.InformationalViewedIP;
              dateTimeWithZone = disclosureRecipient2.Tracking.InformationalCompletedDate;
              DateTime dateTime33 = dateTimeWithZone.DateTime;
              string informationalCompletedIp3 = disclosureRecipient2.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking tracking3 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dateTime23, dateTime24, dateTime25, dateTime26, dateTime27, dateTime28, dateTime29, authenticatedIp3, acceptConsentIp3, rejectConsentIp3, esignedIp3, loanLevelConsent3, dateTime30, dateTime31, dateTime32, informationalViewedIp3, dateTime33, informationalCompletedIp3);
              string userId = disclosureRecipient2.UserId;
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient disclosureRecipient3 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient(id3, name3, email3, role4, roleDescription3, disclosedMethod4, methodDescription3, borrowerType3, presumedReceivedDate3, actualReceivedDate3, tracking3, userId);
              disclosureRecipients3.Add(disclosureRecipient3);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Other:
              EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient4 = disclosureRecipient1;
              List<EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient> disclosureRecipients4 = bam2015Enhanced.DisclosureRecipients;
              string id4 = disclosureRecipient4.Id;
              string name4 = disclosureRecipient4.Name;
              string email4 = disclosureRecipient4.Email;
              role1 = disclosureRecipient4.Role;
              string role5 = role1.ToString();
              string roleDescription4 = disclosureRecipient4.RoleDescription;
              disclosedMethod1 = disclosureRecipient4.DisclosedMethod;
              string disclosedMethod5 = disclosedMethod1.ToString();
              string methodDescription4 = disclosureRecipient4.DisclosedMethodDescription;
              string borrowerType4 = disclosureRecipient4.BorrowerType.UseUserValue ? disclosureRecipient4.BorrowerType.UserValue : disclosureRecipient4.BorrowerType.ComputedValue;
              DateTime presumedReceivedDate4 = disclosureRecipient4.PresumedReceivedDate.UseUserValue ? disclosureRecipient4.PresumedReceivedDate.UserValue : disclosureRecipient4.PresumedReceivedDate.ComputedValue;
              DateTime actualReceivedDate4 = disclosureRecipient4.ActualReceivedDate;
              dateTimeWithZone = disclosureRecipient4.Tracking.AcceptConsentDate;
              DateTime dateTime34 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ESignedDate;
              DateTime dateTime35 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.WetSignedDate;
              DateTime dateTime36 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.RejectConsentDate;
              DateTime dateTime37 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewConsentDate;
              DateTime dateTime38 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewMessageDate;
              DateTime dateTime39 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.AuthenticatedDate;
              DateTime dateTime40 = dateTimeWithZone.DateTime;
              string authenticatedIp4 = disclosureRecipient4.Tracking.AuthenticatedIP;
              string acceptConsentIp4 = disclosureRecipient4.Tracking.AcceptConsentIP;
              string rejectConsentIp4 = disclosureRecipient4.Tracking.RejectConsentIP;
              string esignedIp4 = disclosureRecipient4.Tracking.ESignedIP;
              string loanLevelConsent4 = disclosureRecipient4.Tracking.LoanLevelConsent;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewESignedDate;
              DateTime dateTime41 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.ViewWetSignedDate;
              DateTime dateTime42 = dateTimeWithZone.DateTime;
              dateTimeWithZone = disclosureRecipient4.Tracking.InformationalViewedDate;
              DateTime dateTime43 = dateTimeWithZone.DateTime;
              string informationalViewedIp4 = disclosureRecipient4.Tracking.InformationalViewedIP;
              dateTimeWithZone = disclosureRecipient4.Tracking.InformationalCompletedDate;
              DateTime dateTime44 = dateTimeWithZone.DateTime;
              string informationalCompletedIp4 = disclosureRecipient4.Tracking.InformationalCompletedIP;
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking tracking4 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dateTime34, dateTime35, dateTime36, dateTime37, dateTime38, dateTime39, dateTime40, authenticatedIp4, acceptConsentIp4, rejectConsentIp4, esignedIp4, loanLevelConsent4, dateTime41, dateTime42, dateTime43, informationalViewedIp4, dateTime44, informationalCompletedIp4);
              EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient disclosureRecipient5 = new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient(id4, name4, email4, role5, roleDescription4, disclosedMethod5, methodDescription4, borrowerType4, presumedReceivedDate4, actualReceivedDate4, tracking4);
              disclosureRecipients4.Add(disclosureRecipient5);
              continue;
            default:
              continue;
          }
        }
        bam2015Enhanced.Fulfillments = new List<EllieMae.EMLite.ePass.BamObjects.FulfillmentFields>();
        foreach (EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillment in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentFields>) disclosureTracking2015Log.Fulfillments)
        {
          List<EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient> recipients = new List<EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient>();
          foreach (EnhancedDisclosureTracking2015Log.FulfillmentRecipient recipient in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>) fulfillment.Recipients)
          {
            List<EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient> fulfillmentRecipientList = recipients;
            string id = recipient.Id;
            DateTimeWithZone dateTimeWithZone = recipient.PresumedDate;
            DateTime dateTime45 = dateTimeWithZone.DateTime;
            dateTimeWithZone = recipient.ActualDate;
            DateTime dateTime46 = dateTimeWithZone.DateTime;
            string comments = recipient.Comments;
            EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient fulfillmentRecipient = new EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient(id, dateTime45, dateTime46, comments);
            fulfillmentRecipientList.Add(fulfillmentRecipient);
          }
          bam2015Enhanced.Fulfillments.Add(new EllieMae.EMLite.ePass.BamObjects.FulfillmentFields(fulfillment.IsManual, fulfillment.Id, this.tranformToBam2015Method(fulfillment.DisclosedMethod), recipients, fulfillment.OrderedBy, fulfillment.ProcessedDate.DateTime, fulfillment.TrackingNumber));
        }
        bam2015Enhanced.LoanEstimate = new EllieMae.EMLite.ePass.BamObjects.LoanEstimateFields(disclosureTracking2015Log.LoanEstimate.IsDisclosedByBroker, disclosureTracking2015Log.LoanEstimate.IsChangedCircumstanceSettlementCharges, disclosureTracking2015Log.LoanEstimate.IsChangedCircumstanceEligibility, disclosureTracking2015Log.LoanEstimate.IsRevisionsRequestedByConsumer, disclosureTracking2015Log.LoanEstimate.IsInterestRateDependentCharges, disclosureTracking2015Log.LoanEstimate.IsExpiration, disclosureTracking2015Log.LoanEstimate.IsDelayedSettlementOnConstructionLoans, disclosureTracking2015Log.LoanEstimate.IsOther, disclosureTracking2015Log.LoanEstimate.OtherDescription, disclosureTracking2015Log.LoanEstimate.ChangesReceivedDate, disclosureTracking2015Log.LoanEstimate.RevisedDueDate);
        bam2015Enhanced.ClosingDisclosure = new EllieMae.EMLite.ePass.BamObjects.ClosingDisclosureFields(disclosureTracking2015Log.ClosingDisclosure.IsChangeInAPR, disclosureTracking2015Log.ClosingDisclosure.IsChangeInLoanProduct, disclosureTracking2015Log.ClosingDisclosure.IsPrepaymentPenaltyAdded, disclosureTracking2015Log.ClosingDisclosure.IsChangeInSettlementCharges, disclosureTracking2015Log.ClosingDisclosure.Is24HourAdvancePreview, disclosureTracking2015Log.ClosingDisclosure.IsToleranceCure, disclosureTracking2015Log.ClosingDisclosure.IsClericalErrorCorrection, disclosureTracking2015Log.ClosingDisclosure.IsChangedCircumstanceEligibility, disclosureTracking2015Log.ClosingDisclosure.IsInterestRateDependentCharges, disclosureTracking2015Log.ClosingDisclosure.IsRevisionsRequestedByConsumer, disclosureTracking2015Log.ClosingDisclosure.IsOther, disclosureTracking2015Log.ClosingDisclosure.OtherDescription, disclosureTracking2015Log.ClosingDisclosure.ChangesReceivedDate, disclosureTracking2015Log.ClosingDisclosure.RevisedDueDate);
        bam2015Enhanced.IntentToProceed = new EllieMae.EMLite.ePass.BamObjects.IntentToProceedFields(disclosureTracking2015Log.IntentToProceed.Intent, disclosureTracking2015Log.IntentToProceed.Date, disclosureTracking2015Log.IntentToProceed.ReceivedBy.UseUserValue ? disclosureTracking2015Log.IntentToProceed.ReceivedBy.UserValue : disclosureTracking2015Log.IntentToProceed.ReceivedBy.ComputedValue, this.tranformToBam2015Method(disclosureTracking2015Log.IntentToProceed.ReceivedMethod), disclosureTracking2015Log.IntentToProceed.ReceivedMethodOther, disclosureTracking2015Log.IntentToProceed.Comments);
        bam2015Enhanced.PropertyAddress = new EllieMae.EMLite.ePass.BamObjects.Address(disclosureTracking2015Log.PropertyAddress.City, disclosureTracking2015Log.PropertyAddress.State, disclosureTracking2015Log.PropertyAddress.Street1, disclosureTracking2015Log.PropertyAddress.Street2, disclosureTracking2015Log.PropertyAddress.Zip);
        bam2015Enhanced.ChangeInCircumstance = disclosureTracking2015Log.ChangeInCircumstance;
        bam2015Enhanced.ChangeInCircumstanceComments = disclosureTracking2015Log.ChangeInCircumstanceComments;
        bam2015Enhanced.UCD = disclosureTracking2015Log.UCD;
        bam2015Enhanced.Tracking = new EllieMae.EMLite.ePass.BamObjects.TrackingFields(this.transformToBAM2015TrackingIndicators(disclosureTracking2015Log.Tracking.Indicators), log.eDisclosureDisclosedMessage, log.eDisclosurePackageCreatedDate, log.eDisclosurePackageID, "");
        if (disclosureTracking2015Log.UCD != string.Empty)
        {
          XmlDocument doc = new XmlDocument();
          doc.LoadXml(disclosureTracking2015Log.UCD);
          foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
          {
            if (!bam2015Enhanced.DisclosedFields.ContainsKey(keyValuePair.Key))
              bam2015Enhanced.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      else
      {
        DisclosureTracking2015Log dt2015Log = (DisclosureTracking2015Log) log;
        bam2015Enhanced.Status = "Active";
        bam2015Enhanced.Provider = "Encompass";
        bam2015Enhanced.DisclosedDate = dt2015Log.DateAdded;
        bam2015Enhanced.LoanAmount = dt2015Log.LoanAmount.ToString();
        bam2015Enhanced.LoanProgram = dt2015Log.LoanProgram;
        bam2015Enhanced.ApplicationDate = dt2015Log.ApplicationDate;
        bam2015Enhanced.Contents = this.getDisclosureContentTypeforDT2015Log(dt2015Log);
        bam2015Enhanced.IsManuallyCreated = dt2015Log.IsManuallyCreated;
        string id5 = dt2015Log.Guid + "_borrower";
        string name5 = string.IsNullOrWhiteSpace(dt2015Log.eDisclosureBorrowerName) ? dt2015Log.BorrowerName : dt2015Log.eDisclosureBorrowerName;
        bam2015Enhanced.DisclosureRecipients.Add((EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient) new EllieMae.EMLite.ePass.BamObjects.BorrowerRecipient(id5, dt2015Log.BorrowerPairID, name5, dt2015Log.eDisclosureBorrowerEmail, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower.ToString(), string.Empty, dt2015Log.BorrowerDisclosedMethod.ToString(), dt2015Log.BorrowerDisclosedMethodOther, dt2015Log.IsBorrowerTypeLocked ? dt2015Log.LockedBorrowerType : dt2015Log.BorrowerType, dt2015Log.IsBorrowerPresumedDateLocked ? dt2015Log.BorrowerPresumedLockedReceivedDate : dt2015Log.BorrowerPresumedReceivedDate, dt2015Log.BorrowerActualReceivedDate, new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dt2015Log.eDisclosureBorrowerAcceptConsentDate, dt2015Log.eDisclosureBorrowereSignedDate, dt2015Log.eDisclosureBorrowerWetSignedDate, dt2015Log.eDisclosureBorrowerRejectConsentDate, dt2015Log.eDisclosureBorrowerViewConsentDate, dt2015Log.eDisclosureBorrowerViewMessageDate, dt2015Log.eDisclosureBorrowerAuthenticatedDate, dt2015Log.eDisclosureBorrowerAuthenticatedIP, dt2015Log.eDisclosureBorrowerAcceptConsentIP, dt2015Log.eDisclosureBorrowerRejectConsentIP, dt2015Log.eDisclosureBorrowereSignedIP, dt2015Log.EDisclosureBorrowerLoanLevelConsent, DateTime.MinValue, DateTime.MinValue)));
        string id6 = dt2015Log.Guid + "_coBorrower";
        string name6 = string.IsNullOrWhiteSpace(dt2015Log.eDisclosureCoBorrowerName) ? dt2015Log.CoBorrowerName : dt2015Log.eDisclosureCoBorrowerName;
        if (!string.IsNullOrWhiteSpace(name6))
          bam2015Enhanced.DisclosureRecipients.Add((EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient) new CoBorrowerRecipient(id6, dt2015Log.BorrowerPairID, name6, dt2015Log.eDisclosureCoBorrowerEmail, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower.ToString(), string.Empty, dt2015Log.CoBorrowerDisclosedMethod.ToString(), dt2015Log.CoBorrowerDisclosedMethodOther, dt2015Log.IsCoBorrowerTypeLocked ? dt2015Log.LockedCoBorrowerType : dt2015Log.CoBorrowerType, dt2015Log.IsCoBorrowerPresumedDateLocked ? dt2015Log.CoBorrowerPresumedLockedReceivedDate : dt2015Log.CoBorrowerPresumedReceivedDate, dt2015Log.CoBorrowerActualReceivedDate, new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(dt2015Log.eDisclosureCoBorrowerAcceptConsentDate, dt2015Log.eDisclosureCoBorrowereSignedDate, DateTime.MinValue, dt2015Log.eDisclosureCoBorrowerRejectConsentDate, dt2015Log.eDisclosureCoBorrowerViewConsentDate, dt2015Log.eDisclosureCoBorrowerViewMessageDate, dt2015Log.eDisclosureCoBorrowerAuthenticatedDate, dt2015Log.eDisclosureCoBorrowerAuthenticatedIP, dt2015Log.eDisclosureCoBorrowerAcceptConsentIP, dt2015Log.eDisclosureCoBorrowerRejectConsentIP, dt2015Log.eDisclosureCoBorrowereSignedIP, dt2015Log.EDisclosureCoBorrowerLoanLevelConsent, DateTime.MinValue, DateTime.MinValue)));
        if (!string.IsNullOrWhiteSpace(dt2015Log.eDisclosureLOName))
        {
          string id7 = dt2015Log.Guid + "_LoanOriginator";
          string disclosureLoName = dt2015Log.eDisclosureLOName;
          bam2015Enhanced.DisclosureRecipients.Add(new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipient(id7, disclosureLoName, "", EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate.ToString(), "Originator", string.Empty, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, new EllieMae.EMLite.ePass.BamObjects.DisclosureRecipientTracking(DateTime.MinValue, dt2015Log.eDisclosureLOeSignedDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, dt2015Log.eDisclosureLOViewMessageDate, DateTime.MinValue, string.Empty, string.Empty, string.Empty, dt2015Log.eDisclosureLOeSignedIP, string.Empty, DateTime.MinValue, DateTime.MinValue), dt2015Log.eDisclosureLOUserId));
        }
        List<EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient> recipients = new List<EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient>();
        recipients.Add(new EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient(id5, dt2015Log.PresumedFulfillmentDate, dt2015Log.ActualFulfillmentDate, dt2015Log.eDisclosureManualFulfillmentComment));
        if (!string.IsNullOrWhiteSpace(name6))
        {
          EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient fulfillmentRecipient = new EllieMae.EMLite.ePass.BamObjects.FulfillmentRecipient(id5, dt2015Log.PresumedFulfillmentDate, dt2015Log.ActualFulfillmentDate, dt2015Log.eDisclosureManualFulfillmentComment);
          recipients.Add(fulfillmentRecipient);
        }
        Guid guid;
        if (dt2015Log.eDisclosureManualFulfillmentDate == DateTime.MinValue)
        {
          List<EllieMae.EMLite.ePass.BamObjects.FulfillmentFields> fulfillments = bam2015Enhanced.Fulfillments;
          guid = new Guid();
          EllieMae.EMLite.ePass.BamObjects.FulfillmentFields fulfillmentFields = new EllieMae.EMLite.ePass.BamObjects.FulfillmentFields(false, guid.ToString(), DisclosedMethodEnum.None, recipients, dt2015Log.FulfillmentOrderedBy, dt2015Log.FullfillmentProcessedDate, dt2015Log.FulfillmentTrackingNumber);
          fulfillments.Add(fulfillmentFields);
        }
        else
        {
          List<EllieMae.EMLite.ePass.BamObjects.FulfillmentFields> fulfillments = bam2015Enhanced.Fulfillments;
          guid = new Guid();
          EllieMae.EMLite.ePass.BamObjects.FulfillmentFields fulfillmentFields = new EllieMae.EMLite.ePass.BamObjects.FulfillmentFields(false, guid.ToString(), this.tranformToBam2015Method(dt2015Log.eDisclosureManualFulfillmentMethod), recipients, dt2015Log.FulfillmentOrderedBy, dt2015Log.FullfillmentProcessedDate, dt2015Log.FulfillmentTrackingNumber);
          fulfillments.Add(fulfillmentFields);
        }
        bam2015Enhanced.LoanEstimate = new EllieMae.EMLite.ePass.BamObjects.LoanEstimateFields(dt2015Log.LEDisclosedByBroker, dt2015Log.LEReasonIsChangedCircumstanceSettlementCharges, dt2015Log.LEReasonIsChangedCircumstanceEligibility, dt2015Log.LEReasonIsRevisionsRequestedByConsumer, dt2015Log.LEReasonIsInterestRateDependentCharges, dt2015Log.LEReasonIsExpiration, dt2015Log.LEReasonIsDelayedSettlementOnConstructionLoans, dt2015Log.LEReasonIsOther, dt2015Log.LEReasonOther, dt2015Log.ChangesReceivedDate, dt2015Log.RevisedDueDate);
        bam2015Enhanced.ClosingDisclosure = new EllieMae.EMLite.ePass.BamObjects.ClosingDisclosureFields(dt2015Log.CDReasonIsChangeInAPR, dt2015Log.CDReasonIsChangeInLoanProduct, dt2015Log.CDReasonIsPrepaymentPenaltyAdded, dt2015Log.CDReasonIsChangeInSettlementCharges, dt2015Log.CDReasonIs24HourAdvancePreview, dt2015Log.CDReasonIsToleranceCure, dt2015Log.CDReasonIsClericalErrorCorrection, dt2015Log.CDReasonIsChangedCircumstanceEligibility, dt2015Log.CDReasonIsInterestRateDependentCharges, dt2015Log.CDReasonIsRevisionsRequestedByConsumer, dt2015Log.CDReasonIsOther, dt2015Log.CDReasonOther, dt2015Log.ChangesReceivedDate, dt2015Log.RevisedDueDate);
        bam2015Enhanced.IntentToProceed = new EllieMae.EMLite.ePass.BamObjects.IntentToProceedFields(dt2015Log.IntentToProceed, dt2015Log.IntentToProceedDate, dt2015Log.IsIntentReceivedByLocked ? dt2015Log.LockedIntentReceivedByField : dt2015Log.IntentToProceedReceivedBy, this.tranformToBam2015Method(dt2015Log.IntentToProceedReceivedMethod), dt2015Log.IntentToProceedReceivedMethodOther, dt2015Log.IntentToProceedComments);
        bam2015Enhanced.Tracking = new EllieMae.EMLite.ePass.BamObjects.TrackingFields(this.getTrackingIndicatorsforDT2015Log(dt2015Log), dt2015Log.eDisclosureDisclosedMessage, dt2015Log.eDisclosurePackageCreatedDate, dt2015Log.eDisclosurePackageID, dt2015Log.EDSRequestGuid);
        bam2015Enhanced.PropertyAddress = new EllieMae.EMLite.ePass.BamObjects.Address(dt2015Log.PropertyCity, dt2015Log.PropertyState, dt2015Log.PropertyAddress, string.Empty, dt2015Log.PropertyZip);
        bam2015Enhanced.ChangeInCircumstance = dt2015Log.ChangeInCircumstance;
        bam2015Enhanced.ChangeInCircumstanceComments = dt2015Log.ChangeInCircumstanceComments;
        bam2015Enhanced.UCD = dt2015Log.UCD;
        if (dt2015Log.UCD != string.Empty)
        {
          XmlDocument doc = new XmlDocument();
          doc.LoadXml(dt2015Log.UCD);
          foreach (KeyValuePair<string, string> keyValuePair in new UCDXmlParser(doc).ParseXml())
          {
            if (!bam2015Enhanced.DisclosedFields.ContainsKey(keyValuePair.Key))
              bam2015Enhanced.DisclosedFields.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      return bam2015Enhanced;
    }

    private List<EllieMae.EMLite.ePass.BamObjects.DisclosureContentType> getDisclosureContentTypeforDT2015Log(
      DisclosureTracking2015Log dt2015Log)
    {
      List<EllieMae.EMLite.ePass.BamObjects.DisclosureContentType> typeforDt2015Log = new List<EllieMae.EMLite.ePass.BamObjects.DisclosureContentType>();
      if (dt2015Log.DisclosedForLE)
        typeforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DisclosureContentType.LE);
      if (dt2015Log.DisclosedForCD)
        typeforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DisclosureContentType.CD);
      if (dt2015Log.DisclosedForSafeHarbor)
        typeforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DisclosureContentType.SafeHarbor);
      if (dt2015Log.ProviderListSent)
        typeforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DisclosureContentType.ServiceProviderList);
      if (dt2015Log.ProviderListNoFeeSent)
        typeforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DisclosureContentType.ServiceProviderListNoFee);
      return typeforDt2015Log;
    }

    private DisclosedMethodEnum tranformToBam2015Method(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      DisclosedMethodEnum bam2015Method = DisclosedMethodEnum.ByMail;
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.None:
          bam2015Method = DisclosedMethodEnum.None;
          break;
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          bam2015Method = DisclosedMethodEnum.eDisclosure;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          bam2015Method = DisclosedMethodEnum.Fax;
          break;
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          bam2015Method = DisclosedMethodEnum.InPerson;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Other:
          bam2015Method = DisclosedMethodEnum.Other;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Email:
          bam2015Method = DisclosedMethodEnum.Email;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          bam2015Method = DisclosedMethodEnum.Phone;
          break;
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          bam2015Method = DisclosedMethodEnum.Signature;
          break;
      }
      return bam2015Method;
    }

    private List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators> transformToBAM2015TrackingIndicators(
      IList<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators> indicators)
    {
      List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators> trackingIndicators = new List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators>();
      if (indicators == null)
        return trackingIndicators;
      foreach (int indicator in (IEnumerable<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) indicators)
      {
        switch (indicator)
        {
          case 0:
            trackingIndicators.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ApplicationPackage);
            continue;
          case 1:
            trackingIndicators.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ApprovalPackage);
            continue;
          case 2:
            trackingIndicators.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.LockPackage);
            continue;
          case 3:
            trackingIndicators.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ThreeDayPackage);
            continue;
          default:
            continue;
        }
      }
      return trackingIndicators;
    }

    private List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators> getTrackingIndicatorsforDT2015Log(
      DisclosureTracking2015Log dt2015Log)
    {
      List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators> indicatorsforDt2015Log = new List<EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators>();
      if (dt2015Log.eDisclosureApplicationPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ApplicationPackage);
      if (dt2015Log.eDisclosureThreeDayPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ThreeDayPackage);
      if (dt2015Log.eDisclosureApprovalPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.ApprovalPackage);
      if (dt2015Log.eDisclosureLockPackage)
        indicatorsforDt2015Log.Add(EllieMae.EMLite.ePass.BamObjects.DT2015TrackingIndicators.LockPackage);
      return indicatorsforDt2015Log;
    }

    [Guid("379C7AB3-4A15-333C-8E6B-EA69E212FCF2")]
    public enum FileType
    {
      INFILE_PDF,
      INFILE_HTML,
      FLOOD_PDF,
      FLOOD_HTML,
      RMCR_PDF,
      RMCR_HTML,
      UNKNOWN,
    }
  }
}
