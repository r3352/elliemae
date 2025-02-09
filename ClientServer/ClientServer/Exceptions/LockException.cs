// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.LockException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class LockException : ServerException
  {
    private const uint innerHResult = 2147754245;
    protected LockInfo lockInfo;

    public LockException(string message)
      : this(message, (LockInfo) null)
    {
    }

    public LockException(string message, LockInfo lockInfo)
      : base(message)
    {
      this.lockInfo = lockInfo;
      this.HResult = this.HRESULT(2147754245U);
    }

    public LockException(LockInfo lockInfo)
      : this("Object is locked by user '" + lockInfo.LockedBy + "'", lockInfo)
    {
    }

    protected LockException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.lockInfo = (LockInfo) info.GetValue(nameof (lockInfo), typeof (LockInfo));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("lockInfo", (object) this.lockInfo);
    }

    [CLSCompliant(false)]
    public LockInfo LockInfo => this.lockInfo;
  }
}
