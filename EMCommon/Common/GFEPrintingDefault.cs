// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.GFEPrintingDefault
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Flags]
  public enum GFEPrintingDefault
  {
    None = 0,
    LenderEx = 1,
    Broker = 2,
    Lender = 4,
    Itemization = 8,
    BrokerEx = 16, // 0x00000010
  }
}
