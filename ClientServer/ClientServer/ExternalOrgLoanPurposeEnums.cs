// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOrgLoanPurposeEnums
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Flags]
  public enum ExternalOrgLoanPurposeEnums
  {
    None = 0,
    Purchase = 1,
    NoCashOutRefi = 2,
    CashOutRefi = 4,
    Construction = 8,
    ConstructionPerm = 16, // 0x00000010
    Other = 32, // 0x00000020
  }
}
