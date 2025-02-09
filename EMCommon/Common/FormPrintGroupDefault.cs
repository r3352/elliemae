// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FormPrintGroupDefault
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Flags]
  public enum FormPrintGroupDefault
  {
    None = 0,
    General = 2,
    Verif = 4,
    FHA = 8,
    VA = 16, // 0x00000010
    Tools = 32, // 0x00000020
    DocTracking = 64, // 0x00000040
    StateSpecific = 128, // 0x00000080
    All = StateSpecific | DocTracking | Tools | VA | FHA | Verif | General, // 0x000000FE
  }
}
