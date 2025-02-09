// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.UserProfileInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class UserProfileInfo : ICloneable, IComparable
  {
    private string userid;
    private string lastName;
    private string firstName;
    private string middleName;
    private string suffixName;
    private string jobTitle;
    private string email;
    private int phone1Type;
    private string phone1;
    private int phone2Type;
    private string phone2;
    private string link1;
    private string link2;
    private string link3;
    private string profileDesc;
    private bool lastName_IsDefault;
    private bool firstName_IsDefault;
    private bool middleName_IsDefault;
    private bool suffixName_IsDefault;
    private bool phone1_IsDefault;
    private bool email_IsDefault;
    private bool phone2_IsDefault;
    private bool enable_profile;
    public Dictionary<UserProfileInfo.ContextDataId, object> ContextData = new Dictionary<UserProfileInfo.ContextDataId, object>();

    public object Clone() => (object) new UserProfileInfo(this);

    public int CompareTo(object obj) => this.FullName.CompareTo(((UserProfileInfo) obj).FullName);

    public UserProfileInfo(
      string userid,
      string lastName,
      string suffixName,
      string firstName,
      string middleName,
      string jobTitle,
      int phone1Type,
      string phone1,
      int phone2Type,
      string phone2,
      string email,
      string link1,
      string link2,
      string link3,
      string profileDesc,
      bool firstname_default,
      bool lastname_default,
      bool middlename_default,
      bool suffix_default,
      bool phone1_default,
      bool email_default,
      bool phone2_default,
      bool enable_profile)
    {
      this.userid = (userid ?? "").ToLower().Trim();
      this.lastName = lastName;
      this.suffixName = suffixName;
      this.firstName = firstName;
      this.middleName = middleName;
      this.jobTitle = jobTitle;
      this.phone1Type = phone1Type;
      this.phone1 = phone1;
      this.phone2Type = phone2Type;
      this.phone2 = phone2;
      this.email = email;
      this.link1 = link1;
      this.link2 = link2;
      this.link3 = link3;
      this.profileDesc = profileDesc;
      this.firstName_IsDefault = firstname_default;
      this.lastName_IsDefault = lastname_default;
      this.middleName_IsDefault = middlename_default;
      this.suffixName_IsDefault = suffix_default;
      this.phone1_IsDefault = phone1_default;
      this.email_IsDefault = email_default;
      this.phone2_IsDefault = phone2_default;
      this.enable_profile = enable_profile;
    }

    public UserProfileInfo(UserProfileInfo source)
    {
      this.userid = (this.userid ?? "").ToLower().Trim();
      this.lastName = source.LastName;
      this.suffixName = source.SuffixName;
      this.firstName = source.FirstName;
      this.middleName = source.MiddleName;
      this.jobTitle = source.JobTitle;
      this.phone1Type = source.Phone1Type;
      this.phone1 = source.Phone1;
      this.phone2Type = source.Phone2Type;
      this.phone2 = source.Phone2;
      this.email = source.Email;
      this.link1 = source.Link1;
      this.link2 = source.Link2;
      this.link3 = source.Link3;
      this.profileDesc = source.ProfileDesc;
      this.firstName_IsDefault = source.FirstName_IsDefault;
      this.lastName_IsDefault = source.LastName_IsDefault;
      this.middleName_IsDefault = source.MiddleName_IsDefault;
      this.suffixName_IsDefault = source.SuffixName_IsDefault;
      this.phone1_IsDefault = source.Phone1_IsDefault;
      this.email_IsDefault = source.Email_IsDefault;
      this.phone2_IsDefault = source.Phone2_IsDefault;
      this.enable_profile = source.Enable_Profile;
    }

    public string UserId => this.userid;

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value ?? "";
    }

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value ?? "";
    }

    public string MiddleName
    {
      get => this.middleName;
      set => this.middleName = value ?? "";
    }

    public string SuffixName
    {
      get => this.suffixName;
      set => this.suffixName = value ?? "";
    }

    public string FullName
    {
      get
      {
        return this.firstName + ((this.middleName ?? "") != string.Empty ? " " + this.middleName : "") + " " + this.lastName + ((this.suffixName ?? "") != string.Empty ? " " + this.suffixName : "");
      }
    }

    public string JobTitle
    {
      get => this.jobTitle;
      set => this.jobTitle = value ?? "";
    }

    public string Phone1
    {
      get => this.phone1;
      set => this.phone1 = value ?? "";
    }

    public int Phone1Type
    {
      get => this.phone1Type;
      set => this.phone1Type = value;
    }

    public string Phone2
    {
      get => this.phone2;
      set => this.phone2 = value ?? "";
    }

    public int Phone2Type
    {
      get => this.phone2Type;
      set => this.phone2Type = value;
    }

    public string Email
    {
      get => this.email;
      set => this.email = value ?? "";
    }

    public string Link1
    {
      get => this.link1;
      set => this.link1 = value ?? "";
    }

    public string Link2
    {
      get => this.link2;
      set => this.link2 = value ?? "";
    }

    public string Link3
    {
      get => this.link3;
      set => this.link3 = value ?? "";
    }

    public string ProfileDesc
    {
      get => this.profileDesc;
      set => this.profileDesc = value ?? "";
    }

    public bool LastName_IsDefault
    {
      get => this.lastName_IsDefault;
      set => this.lastName_IsDefault = value;
    }

    public bool FirstName_IsDefault
    {
      get => this.firstName_IsDefault;
      set => this.firstName_IsDefault = value;
    }

    public bool MiddleName_IsDefault
    {
      get => this.middleName_IsDefault;
      set => this.middleName_IsDefault = value;
    }

    public bool SuffixName_IsDefault
    {
      get => this.suffixName_IsDefault;
      set => this.suffixName_IsDefault = value;
    }

    public bool Phone1_IsDefault
    {
      get => this.phone1_IsDefault;
      set => this.phone1_IsDefault = value;
    }

    public bool Email_IsDefault
    {
      get => this.email_IsDefault;
      set => this.email_IsDefault = value;
    }

    public bool Phone2_IsDefault
    {
      get => this.phone2_IsDefault;
      set => this.phone2_IsDefault = value;
    }

    public bool Enable_Profile
    {
      get => this.enable_profile;
      set => this.enable_profile = value;
    }

    public enum ContextDataId
    {
    }
  }
}
