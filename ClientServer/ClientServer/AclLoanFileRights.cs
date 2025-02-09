// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclLoanFileRights
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public enum AclLoanFileRights
  {
    ViewOnly = 0,
    DocTracking = 1,
    ConvLog = 2,
    ConvLogDocTracking = 3,
    GeneralLog = 4,
    GeneralLogDocTracking = 5,
    GeneralLogConvLog = 6,
    GeneralLogConvLogDocTracking = 7,
    EditAll = 15, // 0x0000000F
  }
}
