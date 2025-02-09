// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.OrgInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class OrgInfo : ICloneable, IComparable
  {
    public static int NoOrgId = -1;
    private int oid = -1;
    private string orgName = "";
    private string description = "";
    private string orgCode = "";
    private int parent = -1;
    private string companyName = "";
    private Address companyAddress = new Address();
    private string phone = "";
    private string fax = "";
    private int[] children = new int[0];
    private string nmlsCode = "";
    private string mersminCode = "";
    private BranchExtLicensing orgBranchLicensing;
    private LoanCompHistoryList loanCompHistoryList;
    private string dbaName1 = string.Empty;
    private string dbaName2 = string.Empty;
    private string dbaName3 = string.Empty;
    private string dbaName4 = string.Empty;
    private string emailSignature = string.Empty;
    private bool showOrgInLOSearch;
    private string loSearchOrgName = "";
    private int hmdaProfileId;
    private CCSiteInfo ccSiteSettings;
    private SSOInfo ssoSettings;
    private ONRPEntitySettings ONRPBranchSettings;

    public OrgInfo()
    {
    }

    public OrgInfo(int oid, string orgName, string description, int parent, int[] children)
      : this(oid, orgName, description, parent, "", "", (Address) null, "", "", children, "", "", (string[]) null, (BranchExtLicensing) null, (LoanCompHistoryList) null, (ONRPEntitySettings) null, (CCSiteInfo) null, (SSOInfo) null, loSearchOrgName: "")
    {
    }

    public OrgInfo(
      int oid,
      string orgName,
      string description,
      int parent,
      string orgCode,
      string companyName,
      Address companyAddress,
      string phone,
      string fax,
      int[] children,
      string nmlsCode,
      string mersminCode,
      string[] dbNames,
      BranchExtLicensing orgBranchLicensing,
      LoanCompHistoryList loanCompHistoryList,
      ONRPEntitySettings ONRPBranchSettings,
      CCSiteInfo ccSiteInfo,
      SSOInfo ssoSettings,
      bool showOrgInLOSearch = false,
      string loSearchOrgName = "�",
      int hmdaProfileId = 0)
    {
      this.oid = oid;
      this.orgName = orgName;
      this.description = description;
      this.parent = parent;
      this.orgCode = orgCode;
      this.companyName = companyName;
      this.companyAddress = companyAddress == null ? new Address() : companyAddress;
      this.phone = phone;
      this.fax = fax;
      this.children = children == null ? new int[0] : children;
      this.nmlsCode = nmlsCode;
      this.mersminCode = mersminCode;
      if (dbNames != null)
      {
        this.dbaName1 = dbNames[0];
        this.dbaName2 = dbNames[1];
        this.dbaName3 = dbNames[2];
        this.dbaName4 = dbNames[3];
      }
      this.orgBranchLicensing = orgBranchLicensing;
      this.loanCompHistoryList = loanCompHistoryList;
      this.ONRPBranchSettings = ONRPBranchSettings == null ? new ONRPEntitySettings() : ONRPBranchSettings;
      this.showOrgInLOSearch = showOrgInLOSearch;
      this.loSearchOrgName = loSearchOrgName;
      this.hmdaProfileId = hmdaProfileId;
      this.ccSiteSettings = ccSiteInfo == null ? new CCSiteInfo() : ccSiteInfo;
      this.ssoSettings = ssoSettings != null ? ssoSettings : new SSOInfo();
    }

    public OrgInfo(
      int oid,
      string orgName,
      string description,
      int parent,
      string orgCode,
      string companyName,
      Address companyAddress,
      string phone,
      string fax,
      int[] children,
      string nmlsCode,
      string mersminCode,
      string[] dbNames,
      BranchExtLicensing orgBranchLicensing,
      LoanCompHistoryList loanCompHistoryList,
      ONRPEntitySettings ONRPBranchSettings,
      bool showOrgInLOSearch = false,
      string loSearchOrgName = "�")
    {
      this.oid = oid;
      this.orgName = orgName;
      this.description = description;
      this.parent = parent;
      this.orgCode = orgCode;
      this.companyName = companyName;
      this.companyAddress = companyAddress == null ? new Address() : companyAddress;
      this.phone = phone;
      this.fax = fax;
      this.children = children == null ? new int[0] : children;
      this.nmlsCode = nmlsCode;
      this.mersminCode = mersminCode;
      if (dbNames != null)
      {
        this.dbaName1 = dbNames[0];
        this.dbaName2 = dbNames[1];
        this.dbaName3 = dbNames[2];
        this.dbaName4 = dbNames[3];
      }
      this.orgBranchLicensing = orgBranchLicensing;
      this.loanCompHistoryList = loanCompHistoryList;
      this.ONRPBranchSettings = ONRPBranchSettings == null ? new ONRPEntitySettings() : ONRPBranchSettings;
      this.showOrgInLOSearch = showOrgInLOSearch;
      this.loSearchOrgName = loSearchOrgName;
      this.ccSiteSettings = new CCSiteInfo();
      this.ssoSettings = new SSOInfo();
    }

    public OrgInfo(int oid, string orgName, string description)
      : this(oid, orgName, description, -1, (int[]) null)
    {
    }

    public OrgInfo(
      string orgName,
      string description,
      int parent,
      string orgCode,
      string companyName,
      Address companyAddress,
      string phone,
      string fax,
      string nmlsCode,
      string mersminCode,
      string[] dbNames,
      BranchExtLicensing orgBranchLicensing,
      LoanCompHistoryList loanCompHistoryList,
      ONRPEntitySettings ONRPBranchSettings,
      CCSiteInfo ccSiteInfo,
      SSOInfo ssoSettings,
      bool showOrgInLOSearch = false,
      string loSearchOrgName = "�",
      int hmdaProfileId = 0)
      : this(-1, orgName, description, parent, orgCode, companyName, companyAddress, phone, fax, (int[]) null, nmlsCode, mersminCode, dbNames, orgBranchLicensing, loanCompHistoryList, ONRPBranchSettings, ccSiteInfo, ssoSettings, showOrgInLOSearch, loSearchOrgName, hmdaProfileId)
    {
    }

    public OrgInfo(
      string orgName,
      string description,
      int parent,
      string orgCode,
      string companyName,
      Address companyAddress,
      string phone,
      string fax,
      string nmlsCode,
      string mersminCode,
      string[] dbNames,
      BranchExtLicensing orgBranchLicensing,
      LoanCompHistoryList loanCompHistoryList,
      ONRPEntitySettings ONRPBranchSettings,
      bool showOrgInLOSearch = false,
      string loSearchOrgName = "�")
      : this(-1, orgName, description, parent, orgCode, companyName, companyAddress, phone, fax, (int[]) null, nmlsCode, mersminCode, dbNames, orgBranchLicensing, loanCompHistoryList, ONRPBranchSettings, showOrgInLOSearch, loSearchOrgName)
    {
    }

    public OrgInfo(OrgInfo source)
    {
      this.oid = source.oid;
      this.orgName = source.orgName;
      this.description = source.description;
      this.parent = source.parent;
      this.orgCode = source.orgCode;
      this.companyName = source.companyName;
      this.companyAddress = (Address) source.companyAddress.Clone();
      this.CompanyPhone = source.CompanyPhone;
      this.CompanyFax = source.CompanyFax;
      this.children = (int[]) source.children.Clone();
      this.nmlsCode = source.nmlsCode;
      this.mersminCode = source.mersminCode;
      this.orgBranchLicensing = (BranchExtLicensing) source.OrgBranchLicensing.Clone();
      this.loanCompHistoryList = source.LOCompHistoryList != null ? (LoanCompHistoryList) source.LOCompHistoryList.Clone() : (LoanCompHistoryList) null;
      this.ONRPBranchSettings = source.ONRPBranchSettings != null ? source.ONRPBranchSettings : new ONRPEntitySettings();
      this.ccSiteSettings = source.ccSiteSettings != null ? source.ccSiteSettings : new CCSiteInfo();
      this.dbaName1 = source.DBAName1;
      this.dbaName2 = source.DBAName2;
      this.dbaName3 = source.DBAName3;
      this.dbaName4 = source.DBAName4;
      this.emailSignature = source.emailSignature;
      this.showOrgInLOSearch = source.showOrgInLOSearch;
      this.loSearchOrgName = source.loSearchOrgName;
      this.hmdaProfileId = source.hmdaProfileId;
      this.ssoSettings = source.ssoSettings != null ? source.ssoSettings : new SSOInfo();
    }

    public int Oid => this.oid;

    public string OrgName
    {
      get => this.orgName;
      set => this.orgName = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public string OrgCode
    {
      get => this.orgCode;
      set => this.orgCode = value;
    }

    public string NMLSCode
    {
      get => this.nmlsCode;
      set => this.nmlsCode = value;
    }

    public string MERSMINCode
    {
      get => this.mersminCode;
      set => this.mersminCode = value;
    }

    public string DBAName1
    {
      get => this.dbaName1;
      set => this.dbaName1 = value;
    }

    public string DBAName2
    {
      get => this.dbaName2;
      set => this.dbaName2 = value;
    }

    public string DBAName3
    {
      get => this.dbaName3;
      set => this.dbaName3 = value;
    }

    public string DBAName4
    {
      get => this.dbaName4;
      set => this.dbaName4 = value;
    }

    public BranchExtLicensing OrgBranchLicensing
    {
      get => this.orgBranchLicensing;
      set => this.orgBranchLicensing = value;
    }

    public LoanCompHistoryList LOCompHistoryList
    {
      get => this.loanCompHistoryList;
      set => this.loanCompHistoryList = value;
    }

    public CCSiteInfo CCSiteSettings
    {
      get => this.ccSiteSettings;
      set => this.ccSiteSettings = value;
    }

    public SSOInfo SSOSettings
    {
      get => this.ssoSettings;
      set => this.ssoSettings = value;
    }

    public string EmailSignature
    {
      get => this.emailSignature;
      set => this.emailSignature = value;
    }

    public bool ShowOrgInLOSearch
    {
      get => this.showOrgInLOSearch;
      set => this.showOrgInLOSearch = value;
    }

    public string LOSearchOrgName
    {
      get => this.loSearchOrgName;
      set => this.loSearchOrgName = value;
    }

    public int HMDAProfileId
    {
      get => this.hmdaProfileId;
      set => this.hmdaProfileId = value;
    }

    public int Parent => this.parent;

    public int[] Children => this.children;

    public string CompanyName
    {
      get => this.companyName;
      set => this.companyName = value;
    }

    public Address CompanyAddress
    {
      get => this.companyAddress;
      set => this.companyAddress = value == null ? new Address() : value;
    }

    public string CompanyPhone
    {
      get => this.phone;
      set => this.phone = value;
    }

    public string CompanyFax
    {
      get => this.fax;
      set => this.fax = value;
    }

    public object Clone() => (object) new OrgInfo(this);

    public int CompareTo(object obj) => this.orgName.CompareTo(((OrgInfo) obj).orgName);

    public void SetDBANames(string[] dbaNames)
    {
      this.dbaName1 = string.Empty;
      this.dbaName2 = string.Empty;
      this.dbaName3 = string.Empty;
      this.dbaName4 = string.Empty;
      int num = 1;
      for (int index = 0; index < dbaNames.Length; ++index)
      {
        if (!(dbaNames[index] == string.Empty))
        {
          switch (num)
          {
            case 1:
              this.dbaName1 = dbaNames[index];
              break;
            case 2:
              this.dbaName2 = dbaNames[index];
              break;
            case 3:
              this.dbaName3 = dbaNames[index];
              break;
            case 4:
              this.dbaName4 = dbaNames[index];
              break;
          }
          ++num;
        }
      }
    }

    public string[] GetDBANames()
    {
      List<string> stringList = new List<string>();
      if (this.dbaName1 != string.Empty)
        stringList.Add(this.dbaName1);
      if (this.dbaName2 != string.Empty)
        stringList.Add(this.dbaName2);
      if (this.dbaName3 != string.Empty)
        stringList.Add(this.dbaName3);
      if (this.dbaName4 != string.Empty)
        stringList.Add(this.dbaName4);
      return stringList.Count == 0 ? (string[]) null : stringList.ToArray();
    }

    public ONRPEntitySettings ONRPRetailBranchSettings
    {
      get => this.ONRPBranchSettings;
      set => this.ONRPBranchSettings = value;
    }
  }
}
