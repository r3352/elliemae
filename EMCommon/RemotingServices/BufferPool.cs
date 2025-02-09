// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BufferPool
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class BufferPool
  {
    private const string className = "BufferPool";
    private static readonly string sw = Tracing.SwCommon;
    private const int MinBufferTimeout = 30;
    private const int MinBufferPurgeInterval = 30;
    private Stack<BufferPool.PoolEntry> bufferStack = new Stack<BufferPool.PoolEntry>();
    private TimeSpan bufferTimeout = TimeSpan.FromMinutes(10.0);
    private TimeSpan bufferTimerFrequency = TimeSpan.FromMinutes(1.0);
    private int minSize = 5;
    private int maxSize = 100;
    private Timer timeoutTimer;
    private byte[] zeroBytes;
    private long buffersAllocated;
    private long buffersAcquired;
    private long buffersReturned;
    private long buffersExpired;
    private long lastBuffersAllocated;
    private long lastBuffersAcquired;
    private long lastBuffersReturned;
    private long lastBuffersExpired;
    private DateTime lastDataCollectionTime = DateTime.Now;

    public BufferPool(string poolName, int bufferSize, int minSize, int maxSize)
    {
      this.Name = poolName;
      this.BufferSize = bufferSize;
      this.minSize = minSize;
      this.maxSize = maxSize;
      this.zeroBytes = new byte[bufferSize];
      this.configurePoolSettings();
      this.timeoutTimer = new Timer(new TimerCallback(this.timeoutBuffers), (object) null, this.bufferTimerFrequency, TimeSpan.FromMilliseconds(-1.0));
    }

    public TimeSpan BufferTimeout
    {
      get
      {
        lock (this)
          return this.bufferTimeout;
      }
      set
      {
        lock (this)
          this.bufferTimeout = value;
      }
    }

    public int BufferSize { get; private set; }

    public string Name { get; private set; }

    public byte[] TakeBuffer()
    {
      lock (this.bufferStack)
      {
        while (this.bufferStack.Count > 0)
        {
          byte[] buffer = this.bufferStack.Pop().Acquire();
          if (buffer != null)
          {
            ++this.buffersAcquired;
            return buffer;
          }
        }
        ++this.buffersAllocated;
      }
      return this.allocateBuffer();
    }

    public void ReturnBuffer(byte[] buffer)
    {
      Buffer.BlockCopy((Array) this.zeroBytes, 0, (Array) buffer, 0, buffer.Length);
      lock (this.bufferStack)
      {
        if (this.bufferStack.Count >= this.maxSize || buffer.Length != this.BufferSize)
          return;
        ++this.buffersReturned;
        this.bufferStack.Push(new BufferPool.PoolEntry(buffer));
      }
    }

    private byte[] allocateBuffer() => new byte[this.BufferSize];

    private void timeoutBuffers(object notUsed)
    {
      try
      {
        long num1 = 0;
        long num2 = 0;
        long num3 = 0;
        BufferPool.PoolEntry[] source = (BufferPool.PoolEntry[]) null;
        lock (this.bufferStack)
        {
          num1 = this.buffersAllocated;
          num2 = this.buffersAcquired;
          num3 = this.buffersReturned;
          source = this.bufferStack.ToArray();
        }
        DateTime now = DateTime.Now;
        int num4 = ((IEnumerable<BufferPool.PoolEntry>) source).Count<BufferPool.PoolEntry>((Func<BufferPool.PoolEntry, bool>) (entry => entry.IsAvailable));
        foreach (BufferPool.PoolEntry poolEntry in source)
        {
          if (num4 > this.minSize)
          {
            if (poolEntry.Timeout(this.bufferTimeout))
            {
              ++this.buffersExpired;
              --num4;
            }
          }
          else
            break;
        }
        double totalSeconds = (now - this.lastDataCollectionTime).TotalSeconds;
        string str1 = "N/A";
        string str2 = "N/A";
        string str3 = "N/A";
        string str4 = "N/A";
        double num5 = (double) (this.BufferSize * num4) / 1000000.0;
        string str5 = num5.ToString("0.0") + " MB";
        if (num1 > 0L || num2 > 0L)
        {
          num5 = (double) num3 * 100.0 / (double) (num1 + num2);
          str1 = num5.ToString("0.0") + "%";
        }
        if (num1 > 0L || num2 > 0L)
        {
          num5 = (double) num2 * 100.0 / (double) (num1 + num2);
          str2 = num5.ToString("0.0") + "%";
        }
        if (totalSeconds > 0.0)
        {
          num5 = (double) ((num1 - this.lastBuffersAllocated) * (long) this.BufferSize) / totalSeconds / 1000.0;
          str3 = num5.ToString("0.0") + " KB/sec";
        }
        if (totalSeconds > 0.0)
        {
          num5 = (double) (num1 + num2 - this.lastBuffersAllocated - this.lastBuffersAcquired) / totalSeconds;
          str4 = num5.ToString("0.0") + " hits/sec";
        }
        double num6 = (double) (num1 - this.lastBuffersAllocated) / totalSeconds;
        double num7 = (double) (num2 - this.lastBuffersAcquired) / totalSeconds;
        double num8 = (double) (num3 - this.lastBuffersReturned) / totalSeconds;
        double num9 = (double) (this.buffersExpired - this.lastBuffersExpired) / totalSeconds;
        string msg = "BufferPool stats for pool '" + this.Name + "': Buffer Size = " + (object) (this.BufferSize / 1000) + " KB, Buffers Available = " + (object) num4 + ", Total Size = " + str5 + ", Hit Rate = " + str4 + ", Alloc Rate = " + str3 + ", Efficiency = " + str2 + ", Recovery = " + str1;
        if (Tracing.Debug)
          Tracing.Log(BufferPool.sw, nameof (BufferPool), TraceLevel.Verbose, msg + Environment.NewLine + string.Format("   {0,-15}{1,15}{2,15}{3,10}", (object) "", (object) "Total", (object) "New", (object) "Rate/sec") + Environment.NewLine + string.Format("   {0,-15}{1,15}{2,15}{3,10:0.0}", (object) "Allocated", (object) num1, (object) (num1 - this.lastBuffersAllocated), (object) num6) + Environment.NewLine + string.Format("   {0,-15}{1,15}{2,15}{3,10:0.0}", (object) "Acquired", (object) num2, (object) (num2 - this.lastBuffersAcquired), (object) num7) + Environment.NewLine + string.Format("   {0,-15}{1,15}{2,15}{3,10:0.0}", (object) "Returned", (object) num3, (object) (num3 - this.lastBuffersReturned), (object) num8) + Environment.NewLine + string.Format("   {0,-15}{1,15}{2,15}{3,10:0.0}", (object) "Expired", (object) this.buffersExpired, (object) (this.buffersExpired - this.lastBuffersExpired), (object) num9));
        else
          Tracing.Log(BufferPool.sw, nameof (BufferPool), TraceLevel.Info, msg);
        this.lastBuffersAcquired = num2;
        this.lastBuffersAllocated = num1;
        this.lastBuffersExpired = this.buffersExpired;
        this.lastBuffersReturned = num3;
        this.lastDataCollectionTime = now;
        this.configurePoolSettings();
      }
      catch (Exception ex)
      {
        Tracing.Log(BufferPool.sw, nameof (BufferPool), TraceLevel.Error, "Error attempting to evaluate buffer pool timeout: " + (object) ex);
      }
      finally
      {
        if (this.timeoutTimer != null)
          this.timeoutTimer.Dispose();
        this.timeoutTimer = new Timer(new TimerCallback(this.timeoutBuffers), (object) null, this.bufferTimerFrequency, TimeSpan.FromMilliseconds(-1.0));
      }
    }

    private void configurePoolSettings()
    {
      int result1;
      if (int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolTimeout"]), out result1))
      {
        if (result1 >= 30)
        {
          this.bufferTimeout = TimeSpan.FromSeconds((double) result1);
        }
        else
        {
          Tracing.Log(BufferPool.sw, nameof (BufferPool), TraceLevel.Warning, "BufferPoolTimeout value cannot be set below " + (object) 30 + " seconds");
          this.bufferTimeout = TimeSpan.FromSeconds(30.0);
        }
      }
      int result2;
      if (!int.TryParse(string.Concat(EnConfigurationSettings.GlobalSettings["BufferPoolPurgeInterval"]), out result2))
        return;
      if (result2 >= 30)
      {
        this.bufferTimerFrequency = TimeSpan.FromSeconds((double) result2);
      }
      else
      {
        Tracing.Log(BufferPool.sw, nameof (BufferPool), TraceLevel.Warning, "BufferPoolPurgeInterval value cannot be set below " + (object) 30 + " seconds");
        this.bufferTimerFrequency = TimeSpan.FromSeconds(30.0);
      }
    }

    private class PoolEntry
    {
      private byte[] buffer;
      private DateTime lastUsed;

      public PoolEntry(byte[] byteBuffer)
      {
        this.buffer = byteBuffer;
        this.lastUsed = DateTime.Now;
      }

      public byte[] Acquire()
      {
        lock (this)
        {
          if (this.buffer != null)
            this.lastUsed = DateTime.Now;
          return this.buffer;
        }
      }

      public void Release()
      {
        lock (this)
          this.buffer = (byte[]) null;
      }

      public bool IsAvailable
      {
        get
        {
          lock (this)
            return this.buffer != null;
        }
      }

      public bool Timeout(TimeSpan timeoutPeriod)
      {
        lock (this)
        {
          if (this.buffer == null || !(DateTime.Now - this.lastUsed > timeoutPeriod))
            return false;
          this.buffer = (byte[]) null;
          return true;
        }
      }
    }
  }
}
