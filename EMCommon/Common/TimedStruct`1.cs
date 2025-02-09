// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TimedStruct`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public struct TimedStruct<T>
  {
    private readonly DateTime expiryTime;
    public readonly T data;

    public TimedStruct(T d, int expiryTimeInMinutes)
    {
      this.expiryTime = DateTime.Now.AddMinutes((double) expiryTimeInMinutes);
      this.data = d;
    }

    public bool StillExists() => DateTime.Now < this.expiryTime;
  }
}
