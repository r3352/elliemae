// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.HMDAProfile
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class HMDAProfile
  {
    public int HMDAProfileID { get; set; }

    public string HMDAProfileName { get; set; }

    public string HMDAProfileOrgnization { get; set; }

    public string HMDAProfileRespondentID { get; set; }

    public string HMDAProfileLEI { get; set; }

    public string HMDAProfileCompanyName { get; set; }

    public string HMDAProfileAgency { get; set; }

    public string HMDAProfileLastModifiedBy { get; set; }

    public DateTime HMDAProfileLastModifiedDate { get; set; }

    public string HMDAProfileSetting { get; set; }
  }
}
