// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IEncERDBRegMgr
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IEncERDBRegMgr
  {
    Dictionary<string, string> CheckConnections(string erdbServer, int erdbServerPort);

    Dictionary<string, object> GetERDBRegistrationInfo();

    string Register(
      string saPwd,
      string erdbAppServer,
      int erdbAppServerPort,
      string erdbServer,
      string erdbName,
      string erdbLogin,
      string erdbPwd,
      string encDataDir,
      string smtpServer,
      int smtpPort,
      string smtpUserName,
      string smtpPwd,
      bool smtpUseSSL,
      string fromEmail,
      string toEmail,
      int emailDeliveryInterval);

    void UpdateNotificationSettings(
      string smtpServer,
      int smtpServerPort,
      string smtpUserName,
      string smtpPassword,
      bool smtpUseSSL,
      string fromEmail,
      string toEmail,
      int emailDeliveryInterval);
  }
}
