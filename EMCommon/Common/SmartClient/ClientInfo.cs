// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SmartClient.ClientInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;

#nullable disable
namespace EllieMae.EMLite.Common.SmartClient
{
  public class ClientInfo
  {
    public RuntimeEnvironment RuntimeEnvironment { get; set; }

    public string ClientID { get; set; }

    public string Password { get; set; }

    public string EncompassSystemID { get; set; }

    public string SqlDbID { get; set; }

    public EncompassEdition EncompassEdition { get; set; }

    public bool IsLegacySettingAutoUpdate { get; set; }

    public NameValuePair[] SessionInfoValuePair { get; set; }

    public string UserId { get; set; }
  }
}
