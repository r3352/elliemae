// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.ObjectLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class ObjectLock
  {
    private Thread thread;
    private string lockName;
    private object lockObject;
    private StackTrace stackTrace;
    private DateTime lockTime;
    private int nestCount = 1;

    public ObjectLock(string lockName, object lockObject)
    {
      this.thread = Thread.CurrentThread;
      this.lockName = lockName;
      this.lockObject = lockObject;
      this.lockTime = DateTime.Now;
      this.stackTrace = new StackTrace(3);
    }

    public Thread Thread => this.thread;

    public string LockName => this.lockName;

    public object LockObject => this.lockObject;

    public DateTime LockTime => this.lockTime;

    public StackTrace StackTrace => this.stackTrace;

    public int NestCount
    {
      get => this.nestCount;
      set => this.nestCount = value;
    }

    public override string ToString()
    {
      return "Object lock details -->" + Environment.NewLine + " + Object ID:    " + this.lockName + Environment.NewLine + " + Thread:       " + this.thread.Name + " (" + (object) this.thread.GetHashCode() + ")" + Environment.NewLine + " + Acquired At:  " + (object) this.lockTime + Environment.NewLine + " + Duration:     " + (object) (DateTime.Now - this.lockTime).TotalSeconds + "s" + Environment.NewLine + " + Thread State: " + (object) this.thread.ThreadState + Environment.NewLine + " + Stack Trace:  " + Environment.NewLine + (object) this.StackTrace;
    }
  }
}
