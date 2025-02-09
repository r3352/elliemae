// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.UserGroupCoreEntity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Flags]
  public enum UserGroupCoreEntity
  {
    Summary = 0,
    Members = 1,
    Loans = 2,
    LoanTemplates = 4,
    All = LoanTemplates | Loans | Members, // 0x00000007
  }
}
