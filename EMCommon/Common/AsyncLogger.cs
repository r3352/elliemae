// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.AsyncLogger
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class AsyncLogger
  {
    private static ConcurrentDictionary<string, List<IApplicationLog>> logWriters = new ConcurrentDictionary<string, List<IApplicationLog>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Queue<AsyncLogger.IQueuedMessage> msgQueue = new Queue<AsyncLogger.IQueuedMessage>();
    private static Thread writerThread = (Thread) null;
    private static bool writeSourceName = false;

    public static event WriteErrorEventHandler WriteError;

    private AsyncLogger()
    {
    }

    static AsyncLogger()
    {
      AsyncLogger.writerThread = new Thread(new ThreadStart(AsyncLogger.logWriterProc));
      AsyncLogger.writerThread.Priority = ThreadPriority.Lowest;
      AsyncLogger.writerThread.IsBackground = true;
      AsyncLogger.writerThread.Start();
    }

    public static bool WriteSourceName
    {
      get
      {
        lock (typeof (AsyncLogger))
          return AsyncLogger.writeSourceName;
      }
      set
      {
        lock (typeof (AsyncLogger))
          AsyncLogger.writeSourceName = value;
      }
    }

    public static List<IApplicationLog> GetLogForSource(string sourceName)
    {
      List<IApplicationLog> applicationLogList = new List<IApplicationLog>();
      return AsyncLogger.logWriters.TryGetValue(sourceName, out applicationLogList) ? applicationLogList : (List<IApplicationLog>) null;
    }

    public static void RegisterSource(string sourceName, IApplicationLog log)
    {
      AsyncLogger.logWriters.AddOrUpdate(sourceName, (Func<string, List<IApplicationLog>>) (key => new List<IApplicationLog>()
      {
        log
      }), (Func<string, List<IApplicationLog>, List<IApplicationLog>>) ((key, loggers) =>
      {
        loggers.Add(log);
        return loggers;
      }));
    }

    public static void UnregisterSource(string sourceName)
    {
      List<IApplicationLog> applicationLogList = (List<IApplicationLog>) null;
      if (!AsyncLogger.logWriters.TryRemove(sourceName, out applicationLogList) || applicationLogList == null || applicationLogList.Count <= 0)
        return;
      applicationLogList.ForEach((Action<IApplicationLog>) (l => l.Close()));
    }

    public static void Write(string sourceName, string message)
    {
      lock (AsyncLogger.msgQueue)
      {
        AsyncLogger.msgQueue.Enqueue((AsyncLogger.IQueuedMessage) new AsyncLogger.DefaultQueuedMessage(sourceName, message));
        Monitor.Pulse((object) AsyncLogger.msgQueue);
      }
    }

    private static void logWriterProc()
    {
label_0:
      lock (AsyncLogger.msgQueue)
      {
        while (AsyncLogger.msgQueue.Count == 0)
          Monitor.Wait((object) AsyncLogger.msgQueue);
      }
      Thread.Sleep(100);
      bool writeSource = AsyncLogger.WriteSourceName;
      Queue<AsyncLogger.IQueuedMessage> queuedMessageQueue = (Queue<AsyncLogger.IQueuedMessage>) null;
      lock (AsyncLogger.msgQueue)
      {
        queuedMessageQueue = new Queue<AsyncLogger.IQueuedMessage>((IEnumerable<AsyncLogger.IQueuedMessage>) AsyncLogger.msgQueue);
        AsyncLogger.msgQueue.Clear();
      }
      Dictionary<string, List<IApplicationLog>> dictionary = new Dictionary<string, List<IApplicationLog>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      while (queuedMessageQueue.Count > 0)
      {
        AsyncLogger.IQueuedMessage msg = queuedMessageQueue.Dequeue();
        try
        {
          List<IApplicationLog> applicationLogList = new List<IApplicationLog>();
          if (AsyncLogger.logWriters.TryGetValue(msg.SourceName, out applicationLogList))
          {
            if (applicationLogList != null)
            {
              if (applicationLogList.Count > 0)
              {
                applicationLogList.ForEach((Action<IApplicationLog>) (l => l.WriteLine(AsyncLogger.formatMessage(msg, writeSource))));
                dictionary[msg.SourceName] = applicationLogList;
              }
            }
          }
        }
        catch (Exception ex)
        {
          AsyncLogger.onWriteError(msg.SourceName, ex);
        }
      }
      using (Dictionary<string, List<IApplicationLog>>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          try
          {
            dictionary[current].ForEach((Action<IApplicationLog>) (lf => lf.Flush()));
          }
          catch (Exception ex)
          {
            AsyncLogger.onWriteError(current, ex);
          }
        }
        goto label_0;
      }
    }

    private static void onWriteError(string instanceName, Exception ex)
    {
      try
      {
        if (AsyncLogger.WriteError == null)
          return;
        AsyncLogger.WriteError(new WriteErrorEventArgs(instanceName, ex));
      }
      catch
      {
      }
    }

    private static string formatMessage(AsyncLogger.IQueuedMessage msg, bool writeSource)
    {
      string str1 = Tracing.MaskPII(msg.Message);
      if (!writeSource)
        return str1;
      string str2 = string.IsNullOrEmpty(msg.SourceName) ? "Default" : msg.SourceName;
      return str2 + " -> " + str1.Replace(Environment.NewLine, Environment.NewLine + str2 + " -> ");
    }

    private interface IQueuedMessage
    {
      string SourceName { get; }

      string Message { get; }
    }

    private class DefaultQueuedMessage : AsyncLogger.IQueuedMessage
    {
      public string SourceName { get; private set; }

      public string Message { get; private set; }

      public DefaultQueuedMessage(string sourceName, string message)
      {
        this.SourceName = sourceName;
        this.Message = message;
      }
    }
  }
}
