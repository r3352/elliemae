// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TrusteeRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TrusteeRecord
  {
    private int id;
    private string contactName = string.Empty;
    private string address = string.Empty;
    private string city = string.Empty;
    private string state = string.Empty;
    private string zipCode = string.Empty;
    private string county = string.Empty;
    private string phone = string.Empty;
    private DateTime trustDate = DateTime.MinValue;
    private string orgState = string.Empty;
    private string orgType = string.Empty;

    public TrusteeRecord(DataRow r)
    {
      this.id = Utils.ParseInt((object) r[nameof (id)].ToString());
      this.contactName = (string) r[nameof (ContactName)];
      this.address = (string) r[nameof (Address)];
      this.city = (string) r[nameof (City)];
      this.state = (string) r[nameof (State)];
      this.zipCode = (string) r[nameof (ZipCode)];
      this.county = r[nameof (County)] != DBNull.Value ? (string) r[nameof (County)] : "";
      this.phone = r["PhoneNumber"] == DBNull.Value ? string.Empty : (string) r["PhoneNumber"];
      this.trustDate = r[nameof (TrustDate)] == DBNull.Value ? DateTime.MinValue : Utils.ParseDate((object) r[nameof (TrustDate)].ToString());
      this.orgState = r[nameof (OrgState)] != DBNull.Value ? (string) r[nameof (OrgState)] : "";
      this.orgType = r[nameof (OrgType)] != DBNull.Value ? (string) r[nameof (OrgType)] : "";
    }

    public TrusteeRecord(
      string contactName,
      string address,
      string city,
      string state,
      string zipCode,
      string county,
      string phone,
      DateTime trustDate,
      string orgState,
      string orgType)
    {
      this.contactName = contactName;
      this.address = address;
      this.city = city;
      this.state = state;
      this.zipCode = zipCode;
      this.county = county;
      this.phone = phone;
      this.trustDate = trustDate;
      this.orgState = orgState;
      this.orgType = orgType;
    }

    public int Id
    {
      set => this.id = value;
      get => this.id;
    }

    public string ContactName
    {
      set => this.contactName = value;
      get => this.contactName;
    }

    public string Address
    {
      set => this.address = value;
      get => this.address;
    }

    public string City
    {
      set => this.city = value;
      get => this.city;
    }

    public string State
    {
      set => this.state = value;
      get => this.state;
    }

    public string ZipCode
    {
      set => this.zipCode = value;
      get => this.zipCode;
    }

    public string County
    {
      set => this.county = value;
      get => this.county;
    }

    public string Phone
    {
      set => this.phone = value;
      get => this.phone;
    }

    public DateTime TrustDate
    {
      set => this.trustDate = value;
      get => this.trustDate;
    }

    public string OrgState
    {
      set => this.orgState = value;
      get => this.orgState;
    }

    public string OrgType
    {
      set => this.orgType = value;
      get => this.orgType;
    }

    public string FullAddress
    {
      get
      {
        string str1 = this.address ?? "";
        if (str1 != string.Empty)
          str1 += ", ";
        string str2 = str1 + this.city;
        if (str2 != string.Empty)
          str2 += ", ";
        string str3 = str2 + this.state + " " + this.zipCode;
        if (string.IsNullOrEmpty(this.county))
          str3 = str3 + (!string.IsNullOrEmpty(str3) ? " " : "") + this.county;
        return str3.Trim();
      }
    }
  }
}
