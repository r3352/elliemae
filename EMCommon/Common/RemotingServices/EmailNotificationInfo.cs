// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RemotingServices.EmailNotificationInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.RemotingServices
{
  public class EmailNotificationInfo
  {
    public List<SimpleUserInfo> from;
    public List<SimpleUserInfo> to;
    public string body;
    public string subject;
    public List<SimpleUserInfo> replyTo;
    public string EmailNotificationType;
  }
}
