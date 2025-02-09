// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TZI
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  internal struct TZI
  {
    public int Bias;
    public int StandardBias;
    public int DaylightBias;
    public SystemTime StandardDate;
    public SystemTime DaylightDate;
  }
}
