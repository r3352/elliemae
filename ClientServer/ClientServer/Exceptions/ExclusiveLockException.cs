// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.ExclusiveLockException
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
  public class ExclusiveLockException : LockException
  {
    private LockInfo[] locks;

    public ExclusiveLockException(string message, LockInfo[] locks)
      : base(message)
    {
      this.setLocks(locks);
    }

    public ExclusiveLockException(LockInfo[] locks)
      : base("The loan is currently locked by another user.")
    {
      this.setLocks(locks);
    }

    protected ExclusiveLockException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.locks = (LockInfo[]) info.GetValue(nameof (locks), typeof (LockInfo[]));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("locks", (object) this.locks);
    }

    private void setLocks(LockInfo[] locks)
    {
      this.locks = locks;
      if (this.locks == null || this.locks.Length == 0)
        return;
      this.lockInfo = this.locks[0];
    }

    public LockInfo[] Locks => this.locks;
  }
}
