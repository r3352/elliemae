// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.AdminClient
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Admin;
using Confluent.Kafka.Impl;
using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Implements an Apache Kafka admin client.</summary>
  internal class AdminClient : IAdminClient, IClient, IDisposable
  {
    private int cancellationDelayMaxMs;
    private Task callbackTask;
    private CancellationTokenSource callbackCts;
    private IntPtr resultQueue = IntPtr.Zero;
    internal static Dictionary<Librdkafka.EventType, Type> adminClientResultTypes = new Dictionary<Librdkafka.EventType, Type>()
    {
      {
        Librdkafka.EventType.CreateTopics_Result,
        typeof (TaskCompletionSource<List<CreateTopicReport>>)
      },
      {
        Librdkafka.EventType.DeleteTopics_Result,
        typeof (TaskCompletionSource<List<DeleteTopicReport>>)
      },
      {
        Librdkafka.EventType.DescribeConfigs_Result,
        typeof (TaskCompletionSource<List<DescribeConfigsResult>>)
      },
      {
        Librdkafka.EventType.AlterConfigs_Result,
        typeof (TaskCompletionSource<List<AlterConfigsReport>>)
      },
      {
        Librdkafka.EventType.CreatePartitions_Result,
        typeof (TaskCompletionSource<List<CreatePartitionsReport>>)
      }
    };
    private IClient ownedClient;
    private Handle handle;

    private List<CreateTopicReport> extractTopicResults(
      IntPtr topicResultsPtr,
      int topicResultsCount)
    {
      IntPtr[] numArray = new IntPtr[topicResultsCount];
      System.Runtime.InteropServices.Marshal.Copy(topicResultsPtr, numArray, 0, topicResultsCount);
      return ((IEnumerable<IntPtr>) numArray).Select<IntPtr, CreateTopicReport>((Func<IntPtr, CreateTopicReport>) (topicResultPtr => new CreateTopicReport()
      {
        Topic = Util.Marshal.PtrToStringUTF8(Librdkafka.topic_result_name(topicResultPtr)),
        Error = new Error(Librdkafka.topic_result_error(topicResultPtr), Util.Marshal.PtrToStringUTF8(Librdkafka.topic_result_error_string(topicResultPtr)))
      })).ToList<CreateTopicReport>();
    }

    private ConfigEntryResult extractConfigEntry(IntPtr configEntryPtr)
    {
      List<ConfigSynonym> configSynonymList = new List<ConfigSynonym>();
      UIntPtr cntp;
      IntPtr source = Librdkafka.ConfigEntry_synonyms(configEntryPtr, out cntp);
      if (source != IntPtr.Zero)
      {
        IntPtr[] numArray = new IntPtr[(int) (uint) cntp];
        System.Runtime.InteropServices.Marshal.Copy(source, numArray, 0, (int) (uint) cntp);
        configSynonymList = ((IEnumerable<IntPtr>) numArray).Select<IntPtr, ConfigEntryResult>((Func<IntPtr, ConfigEntryResult>) (synonymPtr => this.extractConfigEntry(synonymPtr))).Select<ConfigEntryResult, ConfigSynonym>((Func<ConfigEntryResult, ConfigSynonym>) (e => new ConfigSynonym()
        {
          Name = e.Name,
          Value = e.Value,
          Source = e.Source
        })).ToList<ConfigSynonym>();
      }
      return new ConfigEntryResult()
      {
        Name = Util.Marshal.PtrToStringUTF8(Librdkafka.ConfigEntry_name(configEntryPtr)),
        Value = Util.Marshal.PtrToStringUTF8(Librdkafka.ConfigEntry_value(configEntryPtr)),
        IsDefault = (int) Librdkafka.ConfigEntry_is_default(configEntryPtr) == 1,
        IsSensitive = (int) Librdkafka.ConfigEntry_is_sensitive(configEntryPtr) == 1,
        IsReadOnly = (int) Librdkafka.ConfigEntry_is_read_only(configEntryPtr) == 1,
        Source = Librdkafka.ConfigEntry_source(configEntryPtr),
        Synonyms = configSynonymList
      };
    }

    private List<DescribeConfigsReport> extractResultConfigs(
      IntPtr configResourcesPtr,
      int configResourceCount)
    {
      List<DescribeConfigsReport> resultConfigs = new List<DescribeConfigsReport>();
      IntPtr[] destination = new IntPtr[configResourceCount];
      System.Runtime.InteropServices.Marshal.Copy(configResourcesPtr, destination, 0, configResourceCount);
      foreach (IntPtr config in destination)
      {
        string stringUtF8_1 = Util.Marshal.PtrToStringUTF8(Librdkafka.ConfigResource_name(config));
        ErrorCode code = Librdkafka.ConfigResource_error(config);
        string stringUtF8_2 = Util.Marshal.PtrToStringUTF8(Librdkafka.ConfigResource_error_string(config));
        ResourceType resourceType = Librdkafka.ConfigResource_type(config);
        UIntPtr cntp;
        IntPtr source = Librdkafka.ConfigResource_configs(config, out cntp);
        IntPtr[] numArray = new IntPtr[(int) (uint) cntp];
        if ((int) (uint) cntp > 0)
          System.Runtime.InteropServices.Marshal.Copy(source, numArray, 0, (int) (uint) cntp);
        Dictionary<string, ConfigEntryResult> dictionary = ((IEnumerable<IntPtr>) numArray).Select<IntPtr, ConfigEntryResult>((Func<IntPtr, ConfigEntryResult>) (configEntryPtr => this.extractConfigEntry(configEntryPtr))).ToDictionary<ConfigEntryResult, string>((Func<ConfigEntryResult, string>) (e => e.Name));
        resultConfigs.Add(new DescribeConfigsReport()
        {
          ConfigResource = new ConfigResource()
          {
            Name = stringUtF8_1,
            Type = resourceType
          },
          Entries = dictionary,
          Error = new Error(code, stringUtF8_2)
        });
      }
      return resultConfigs;
    }

    private Task StartPollTask(CancellationToken ct)
    {
      return Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          while (true)
          {
            ct.ThrowIfCancellationRequested();
            try
            {
              IntPtr num = this.kafkaHandle.QueuePoll(this.resultQueue, this.cancellationDelayMaxMs);
              if (!(num == IntPtr.Zero))
              {
                Librdkafka.EventType key = Librdkafka.event_type(num);
                GCHandle gcHandle = GCHandle.FromIntPtr(Librdkafka.event_opaque(num));
                object adminClientResult = gcHandle.Target;
                gcHandle.Free();
                Type type;
                if (!AdminClient.adminClientResultTypes.TryGetValue(key, out type))
                  throw new InvalidOperationException(string.Format("Unknown result type: {0}", (object) key));
                if (adminClientResult.GetType() != type)
                  throw new InvalidOperationException(string.Format("Completion source type mismatch. Expected {0}, got {1}", (object) type.Name, (object) key));
                ErrorCode errorCode = Librdkafka.event_error(num);
                string errorStr = Librdkafka.event_error_string(num);
                switch (key)
                {
                  case Librdkafka.EventType.CreateTopics_Result:
                    if (errorCode != ErrorCode.NoError)
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreateTopicReport>>) adminClientResult).TrySetException((Exception) new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(errorCode, errorStr)))));
                      return;
                    }
                    UIntPtr cntp1;
                    List<CreateTopicReport> result1 = this.extractTopicResults(Librdkafka.CreateTopics_result_topics(num, out cntp1), (int) (uint) cntp1);
                    if (result1.Any<CreateTopicReport>((Func<CreateTopicReport, bool>) (r => r.Error.IsError)))
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreateTopicReport>>) adminClientResult).TrySetException((Exception) new CreateTopicsException(result1))));
                      continue;
                    }
                    Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreateTopicReport>>) adminClientResult).TrySetResult(result1)));
                    continue;
                  case Librdkafka.EventType.DeleteTopics_Result:
                    if (errorCode != ErrorCode.NoError)
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DeleteTopicReport>>) adminClientResult).TrySetException((Exception) new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(errorCode, errorStr)))));
                      return;
                    }
                    UIntPtr cntp2;
                    List<DeleteTopicReport> result2 = this.extractTopicResults(Librdkafka.DeleteTopics_result_topics(num, out cntp2), (int) (uint) cntp2).Select<CreateTopicReport, DeleteTopicReport>((Func<CreateTopicReport, DeleteTopicReport>) (r => new DeleteTopicReport()
                    {
                      Topic = r.Topic,
                      Error = r.Error
                    })).ToList<DeleteTopicReport>();
                    if (result2.Any<DeleteTopicReport>((Func<DeleteTopicReport, bool>) (r => r.Error.IsError)))
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DeleteTopicReport>>) adminClientResult).TrySetException((Exception) new DeleteTopicsException(result2))));
                      continue;
                    }
                    Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DeleteTopicReport>>) adminClientResult).TrySetResult(result2)));
                    continue;
                  case Librdkafka.EventType.CreatePartitions_Result:
                    if (errorCode != ErrorCode.NoError)
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreatePartitionsReport>>) adminClientResult).TrySetException((Exception) new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(errorCode, errorStr)))));
                      return;
                    }
                    UIntPtr cntp3;
                    List<CreatePartitionsReport> result3 = this.extractTopicResults(Librdkafka.CreatePartitions_result_topics(num, out cntp3), (int) (uint) cntp3).Select<CreateTopicReport, CreatePartitionsReport>((Func<CreateTopicReport, CreatePartitionsReport>) (r => new CreatePartitionsReport()
                    {
                      Topic = r.Topic,
                      Error = r.Error
                    })).ToList<CreatePartitionsReport>();
                    if (result3.Any<CreatePartitionsReport>((Func<CreatePartitionsReport, bool>) (r => r.Error.IsError)))
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreatePartitionsReport>>) adminClientResult).TrySetException((Exception) new CreatePartitionsException(result3))));
                      continue;
                    }
                    Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<CreatePartitionsReport>>) adminClientResult).TrySetResult(result3)));
                    continue;
                  case Librdkafka.EventType.AlterConfigs_Result:
                    if (errorCode != ErrorCode.NoError)
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<AlterConfigsReport>>) adminClientResult).TrySetException((Exception) new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(errorCode, errorStr)))));
                      return;
                    }
                    UIntPtr cntp4;
                    List<AlterConfigsReport> result4 = this.extractResultConfigs(Librdkafka.AlterConfigs_result_resources(num, out cntp4), (int) (uint) cntp4).Select<DescribeConfigsReport, AlterConfigsReport>((Func<DescribeConfigsReport, AlterConfigsReport>) (r => new AlterConfigsReport()
                    {
                      ConfigResource = r.ConfigResource,
                      Error = r.Error
                    })).ToList<AlterConfigsReport>();
                    if (result4.Any<AlterConfigsReport>((Func<AlterConfigsReport, bool>) (r => r.Error.IsError)))
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<AlterConfigsReport>>) adminClientResult).TrySetException((Exception) new AlterConfigsException(result4))));
                      continue;
                    }
                    Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<AlterConfigsReport>>) adminClientResult).TrySetResult(result4)));
                    continue;
                  case Librdkafka.EventType.DescribeConfigs_Result:
                    if (errorCode != ErrorCode.NoError)
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DescribeConfigsResult>>) adminClientResult).TrySetException((Exception) new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(errorCode, errorStr)))));
                      return;
                    }
                    UIntPtr cntp5;
                    List<DescribeConfigsReport> result5 = this.extractResultConfigs(Librdkafka.DescribeConfigs_result_resources(num, out cntp5), (int) (uint) cntp5);
                    if (result5.Any<DescribeConfigsReport>((Func<DescribeConfigsReport, bool>) (r => r.Error.IsError)))
                    {
                      Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DescribeConfigsResult>>) adminClientResult).TrySetException((Exception) new DescribeConfigsException(result5))));
                      continue;
                    }
                    List<DescribeConfigsResult> nr = result.Select<DescribeConfigsReport, DescribeConfigsResult>((Func<DescribeConfigsReport, DescribeConfigsResult>) (a => new DescribeConfigsResult()
                    {
                      ConfigResource = a.ConfigResource,
                      Entries = a.Entries
                    })).ToList<DescribeConfigsResult>();
                    Task.Run<bool>((Func<bool>) (() => ((TaskCompletionSource<List<DescribeConfigsResult>>) adminClientResult).TrySetResult(nr)));
                    continue;
                  default:
                    throw new InvalidOperationException(string.Format("Unknown result type: {0}", (object) key));
                }
              }
            }
            catch
            {
              this.DisposeResources();
              break;
            }
          }
        }
        catch (OperationCanceledException ex)
        {
        }
      }), ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.DescribeConfigsAsync(System.Collections.Generic.IEnumerable{Confluent.Kafka.Admin.ConfigResource},Confluent.Kafka.Admin.DescribeConfigsOptions)" />
    /// </summary>
    public Task<List<DescribeConfigsResult>> DescribeConfigsAsync(
      IEnumerable<ConfigResource> resources,
      DescribeConfigsOptions options = null)
    {
      TaskCompletionSource<List<DescribeConfigsResult>> completionSource = new TaskCompletionSource<List<DescribeConfigsResult>>();
      GCHandle gcHandle = GCHandle.Alloc((object) completionSource);
      this.Handle.LibrdkafkaHandle.DescribeConfigs(resources, options, this.resultQueue, GCHandle.ToIntPtr(gcHandle));
      return completionSource.Task;
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.AlterConfigsAsync(System.Collections.Generic.Dictionary{Confluent.Kafka.Admin.ConfigResource,System.Collections.Generic.List{Confluent.Kafka.Admin.ConfigEntry}},Confluent.Kafka.Admin.AlterConfigsOptions)" />
    /// </summary>
    public Task AlterConfigsAsync(
      Dictionary<ConfigResource, List<ConfigEntry>> configs,
      AlterConfigsOptions options = null)
    {
      TaskCompletionSource<List<AlterConfigsReport>> completionSource = new TaskCompletionSource<List<AlterConfigsReport>>();
      GCHandle gcHandle = GCHandle.Alloc((object) completionSource);
      this.Handle.LibrdkafkaHandle.AlterConfigs((IDictionary<ConfigResource, List<ConfigEntry>>) configs, options, this.resultQueue, GCHandle.ToIntPtr(gcHandle));
      return (Task) completionSource.Task;
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.CreateTopicsAsync(System.Collections.Generic.IEnumerable{Confluent.Kafka.Admin.TopicSpecification},Confluent.Kafka.Admin.CreateTopicsOptions)" />
    /// </summary>
    public Task CreateTopicsAsync(
      IEnumerable<TopicSpecification> topics,
      CreateTopicsOptions options = null)
    {
      TaskCompletionSource<List<CreateTopicReport>> completionSource = new TaskCompletionSource<List<CreateTopicReport>>();
      GCHandle gcHandle = GCHandle.Alloc((object) completionSource);
      this.Handle.LibrdkafkaHandle.CreateTopics(topics, options, this.resultQueue, GCHandle.ToIntPtr(gcHandle));
      return (Task) completionSource.Task;
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.DeleteTopicsAsync(System.Collections.Generic.IEnumerable{System.String},Confluent.Kafka.Admin.DeleteTopicsOptions)" />
    /// </summary>
    public Task DeleteTopicsAsync(IEnumerable<string> topics, DeleteTopicsOptions options = null)
    {
      TaskCompletionSource<List<DeleteTopicReport>> completionSource = new TaskCompletionSource<List<DeleteTopicReport>>();
      GCHandle gcHandle = GCHandle.Alloc((object) completionSource);
      this.Handle.LibrdkafkaHandle.DeleteTopics(topics, options, this.resultQueue, GCHandle.ToIntPtr(gcHandle));
      return (Task) completionSource.Task;
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.CreatePartitionsAsync(System.Collections.Generic.IEnumerable{Confluent.Kafka.Admin.PartitionsSpecification},Confluent.Kafka.Admin.CreatePartitionsOptions)" />
    /// </summary>
    public Task CreatePartitionsAsync(
      IEnumerable<PartitionsSpecification> partitionsSpecifications,
      CreatePartitionsOptions options = null)
    {
      TaskCompletionSource<List<CreatePartitionsReport>> completionSource = new TaskCompletionSource<List<CreatePartitionsReport>>();
      GCHandle gcHandle = GCHandle.Alloc((object) completionSource);
      this.Handle.LibrdkafkaHandle.CreatePartitions(partitionsSpecifications, options, this.resultQueue, GCHandle.ToIntPtr(gcHandle));
      return (Task) completionSource.Task;
    }

    private SafeKafkaHandle kafkaHandle => this.handle.LibrdkafkaHandle;

    /// <summary>Initialize a new AdminClient instance.</summary>
    /// <param name="handle">
    ///     An underlying librdkafka client handle that the AdminClient will use to
    ///     make broker requests. It is valid to provide either a Consumer, Producer
    ///     or AdminClient handle.
    /// </param>
    internal AdminClient(Handle handle)
    {
      this.ownedClient = (IClient) null;
      this.handle = handle;
      this.Init();
    }

    internal AdminClient(AdminClientBuilder builder)
    {
      AdminClient adminClient = this;
      IEnumerable<KeyValuePair<string, string>> cancellationDelayMaxMs = Config.ExtractCancellationDelayMaxMs(builder.Config, out this.cancellationDelayMaxMs);
      if (cancellationDelayMaxMs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key.StartsWith("dotnet.producer."))).Count<KeyValuePair<string, string>>() > 0 || cancellationDelayMaxMs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key.StartsWith("dotnet.consumer."))).Count<KeyValuePair<string, string>>() > 0)
        throw new ArgumentException("AdminClient configuration must not include producer or consumer specific configuration properties.");
      ProducerBuilder<Null, Null> producerBuilder = new ProducerBuilder<Null, Null>(cancellationDelayMaxMs);
      if (builder.LogHandler != null)
        producerBuilder.SetLogHandler((Action<IProducer<Null, Null>, LogMessage>) ((_, logMessage) => builder.LogHandler((IAdminClient) adminClient, logMessage)));
      if (builder.ErrorHandler != null)
        producerBuilder.SetErrorHandler((Action<IProducer<Null, Null>, Error>) ((_, error) => builder.ErrorHandler((IAdminClient) adminClient, error)));
      if (builder.StatisticsHandler != null)
        producerBuilder.SetStatisticsHandler((Action<IProducer<Null, Null>, string>) ((_, stats) => builder.StatisticsHandler((IAdminClient) adminClient, stats)));
      this.ownedClient = (IClient) producerBuilder.Build();
      this.handle = new Handle()
      {
        Owner = (IClient) this,
        LibrdkafkaHandle = this.ownedClient.Handle.LibrdkafkaHandle
      };
      this.Init();
    }

    private void Init()
    {
      this.resultQueue = this.kafkaHandle.CreateQueue();
      this.callbackCts = new CancellationTokenSource();
      this.callbackTask = this.StartPollTask(this.callbackCts.Token);
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.ListGroups(System.TimeSpan)" />
    /// </summary>
    public List<GroupInfo> ListGroups(TimeSpan timeout)
    {
      return this.kafkaHandle.ListGroups(timeout.TotalMillisecondsAsInt());
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.ListGroup(System.String,System.TimeSpan)" />
    /// </summary>
    public GroupInfo ListGroup(string group, TimeSpan timeout)
    {
      return this.kafkaHandle.ListGroup(group, timeout.TotalMillisecondsAsInt());
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.GetMetadata(System.TimeSpan)" />
    /// </summary>
    public Metadata GetMetadata(TimeSpan timeout)
    {
      return this.kafkaHandle.GetMetadata(true, (SafeTopicHandle) null, timeout.TotalMillisecondsAsInt());
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IAdminClient.GetMetadata(System.String,System.TimeSpan)" />
    /// </summary>
    public Metadata GetMetadata(string topic, TimeSpan timeout)
    {
      return this.kafkaHandle.GetMetadata(false, this.kafkaHandle.getKafkaTopicHandle(topic), timeout.TotalMillisecondsAsInt());
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IClient.AddBrokers(System.String)" />
    /// </summary>
    public int AddBrokers(string brokers) => this.kafkaHandle.AddBrokers(brokers);

    /// <summary>
    ///     Refer to <see cref="P:Confluent.Kafka.IClient.Name" />
    /// </summary>
    public string Name => this.kafkaHandle.Name;

    /// <summary>
    ///     An opaque reference to the underlying librdkafka
    ///     client instance.
    /// </summary>
    public Handle Handle => this.handle;

    /// <summary>
    ///     Releases all resources used by this AdminClient. In the current
    ///     implementation, this method may block for up to 100ms. This
    ///     will be replaced with a non-blocking version in the future.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the
    ///     <see cref="T:Confluent.Kafka.AdminClient" />
    ///     and optionally disposes the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources;
    ///     false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.callbackCts.Cancel();
      try
      {
        this.callbackTask.Wait();
      }
      catch (AggregateException ex)
      {
        if (ex.InnerException.GetType() != typeof (TaskCanceledException))
          throw ex.InnerException;
      }
      finally
      {
        this.callbackCts.Dispose();
      }
      this.DisposeResources();
    }

    private void DisposeResources()
    {
      this.kafkaHandle.DestroyQueue(this.resultQueue);
      if (this.handle.Owner != this)
        return;
      this.ownedClient.Dispose();
    }
  }
}
