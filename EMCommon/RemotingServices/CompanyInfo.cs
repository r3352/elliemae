// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CompanyInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class CompanyInfo
  {
    private string clientID;
    private string name;
    private string address;
    private string city;
    private string state;
    private string zip;
    private string phone;
    private string fax;
    private string password;
    private string dbaName1 = string.Empty;
    private string dbaName2 = string.Empty;
    private string dbaName3 = string.Empty;
    private string dbaName4 = string.Empty;
    private BranchExtLicensing stateBranchLicensing;

    public string ClientID => this.clientID;

    public string Name => this.name;

    public string Address => this.address;

    public string City => this.city;

    public string State => this.state;

    public string Zip => this.zip;

    public string Phone => this.phone;

    public string Fax => this.fax;

    public string Password => this.password;

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

    public BranchExtLicensing StateBranchLicensing => this.stateBranchLicensing;

    public CompanyInfo(
      string clientID,
      string name,
      string address,
      string city,
      string state,
      string zip,
      string phone,
      string fax,
      string password,
      string[] dbaNames,
      BranchLicensing stateBranchLicensing)
    {
      this.clientID = clientID;
      this.name = name;
      this.address = address;
      this.city = city;
      this.state = state;
      this.zip = zip;
      this.phone = phone;
      this.fax = fax;
      this.password = password;
      this.stateBranchLicensing = stateBranchLicensing != null ? new BranchExtLicensing(stateBranchLicensing) : new BranchExtLicensing();
      this.dbaName1 = dbaNames == null || dbaNames.Length == 0 ? "" : dbaNames[0];
      this.dbaName2 = dbaNames == null || dbaNames.Length <= 1 ? "" : dbaNames[1];
      this.dbaName3 = dbaNames == null || dbaNames.Length <= 2 ? "" : dbaNames[2];
      this.dbaName4 = dbaNames == null || dbaNames.Length <= 3 ? "" : dbaNames[3];
    }

    public CompanyInfo(
      string clientID,
      string name,
      string address,
      string city,
      string state,
      string zip,
      string phone,
      string fax,
      string password,
      string[] dbaNames,
      BranchExtLicensing stateBranchLicensing)
    {
      this.clientID = clientID;
      this.name = name;
      this.address = address;
      this.city = city;
      this.state = state;
      this.zip = zip;
      this.phone = phone;
      this.fax = fax;
      this.password = password;
      this.stateBranchLicensing = stateBranchLicensing != null ? stateBranchLicensing : new BranchExtLicensing();
      this.dbaName1 = dbaNames == null || dbaNames.Length == 0 ? "" : dbaNames[0];
      this.dbaName2 = dbaNames == null || dbaNames.Length <= 1 ? "" : dbaNames[1];
      this.dbaName3 = dbaNames == null || dbaNames.Length <= 2 ? "" : dbaNames[2];
      this.dbaName4 = dbaNames == null || dbaNames.Length <= 3 ? "" : dbaNames[3];
    }

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
  }
}
