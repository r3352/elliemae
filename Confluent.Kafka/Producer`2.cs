// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Producer`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Impl;
using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     A high level producer with serialization capability.
  /// </summary>
  internal class Producer<TKey, TValue> : IProducer<TKey, TValue>, IClient, IDisposable
  {
    private ISerializer<TKey> keySerializer;
    private ISerializer<TValue> valueSerializer;
    private IAsyncSerializer<TKey> asyncKeySerializer;
    private IAsyncSerializer<TValue> asyncValueSerializer;
    private static readonly Dictionary<Type, object> defaultSerializers = new Dictionary<Type, object>()
    {
      {
        typeof (Null),
        (object) Serializers.Null
      },
      {
        typeof (int),
        (object) Serializers.Int32
      },
      {
        typeof (long),
        (object) Serializers.Int64
      },
      {
        typeof (string),
        (object) Serializers.Utf8
      },
      {
        typeof (float),
        (object) Serializers.Single
      },
      {
        typeof (double),
        (object) Serializers.Double
      },
      {
        typeof (byte[]),
        (object) Serializers.ByteArray
      }
    };
    private int cancellationDelayMaxMs;
    private bool disposeHasBeenCalled;
    private object disposeHasBeenCalledLockObj = new object();
    private bool manualPoll;
    private bool enableDeliveryReports = true;
    private bool enableDeliveryReportKey = true;
    private bool enableDeliveryReportValue = true;
    private bool enableDeliveryReportTimestamp = true;
    private bool enableDeliveryReportHeaders = true;
    private bool enableDeliveryReportPersistedStatus = true;
    private SafeKafkaHandle ownedKafkaHandle;
    private Handle borrowedHandle;
    private Task callbackTask;
    private CancellationTokenSource callbackCts;
    private int eventsServedCount;
    private object pollSyncObj = new object();
    private Exception handlerException;
    private Action<Error> errorHandler;
    private Librdkafka.ErrorDelegate errorCallbackDelegate;
    private Action<string> statisticsHandler;
    private Librdkafka.StatsDelegate statisticsCallbackDelegate;
    private Action<LogMessage> logHandler;
    private object loggerLockObj = new object();
    private Librdkafka.LogDelegate logCallbackDelegate;
    private Librdkafka.DeliveryReportDelegate DeliveryReportCallback;

    private SafeKafkaHandle KafkaHandle
    {
      get
      {
        return this.ownedKafkaHandle == null ? this.borrowedHandle.LibrdkafkaHandle : this.ownedKafkaHandle;
      }
    }

    private Task StartPollTask(CancellationToken ct)
    {
      return Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          while (true)
          {
            int num;
            do
            {
              ct.ThrowIfCancellationRequested();
              num = this.ownedKafkaHandle.Poll((IntPtr) this.cancellationDelayMaxMs);
              if (this.handlerException != null)
              {
                Action<Error> errorHandler = this.errorHandler;
                if (errorHandler != null)
                  errorHandler(new Error(ErrorCode.Local_Application, this.handlerException.ToString()));
                this.handlerException = (Exception) null;
              }
            }
            while (num <= 0);
            lock (this.pollSyncObj)
            {
              this.eventsServedCount += num;
              Monitor.Pulse(this.pollSyncObj);
            }
          }
        }
        catch (OperationCanceledException ex)
        {
        }
      }), ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void ErrorCallback(IntPtr rk, ErrorCode err, string reason, IntPtr opaque)
    {
      if (this.ownedKafkaHandle.IsClosed)
        return;
      try
      {
        Action<Error> errorHandler = this.errorHandler;
        if (errorHandler == null)
          return;
        errorHandler(this.KafkaHandle.CreatePossiblyFatalError(err, reason));
      }
      catch (Exception ex)
      {
      }
    }

    private int StatisticsCallback(IntPtr rk, IntPtr json, UIntPtr json_len, IntPtr opaque)
    {
      if (this.ownedKafkaHandle.IsClosed)
        return 0;
      try
      {
        Action<string> statisticsHandler = this.statisticsHandler;
        if (statisticsHandler != null)
          statisticsHandler(Util.Marshal.PtrToStringUTF8(json));
      }
      catch (Exception ex)
      {
        this.handlerException = ex;
      }
      return 0;
    }

    private void LogCallback(IntPtr rk, SyslogLevel level, string fac, string buf)
    {
      if (this.ownedKafkaHandle != null && this.ownedKafkaHandle.IsClosed)
        return;
      try
      {
        Action<LogMessage> logHandler = this.logHandler;
        if (logHandler == null)
          return;
        logHandler(new LogMessage(Util.Marshal.PtrToStringUTF8(Librdkafka.name(rk)), level, fac, buf));
      }
      catch (Exception ex)
      {
      }
    }

    private void DeliveryReportCallbackImpl(IntPtr rk, IntPtr rkmessage, IntPtr opaque)
    {
      if (this.ownedKafkaHandle.IsClosed)
        return;
      try
      {
        rd_kafka_message structure = Util.Marshal.PtrToStructure<rd_kafka_message>(rkmessage);
        if (structure._private == IntPtr.Zero)
          return;
        GCHandle gcHandle = GCHandle.FromIntPtr(structure._private);
        IDeliveryHandler target = (IDeliveryHandler) gcHandle.Target;
        gcHandle.Free();
        Headers headers = (Headers) null;
        if (this.enableDeliveryReportHeaders)
        {
          headers = new Headers();
          IntPtr hdrs;
          int num = (int) Librdkafka.message_headers(rkmessage, out hdrs);
          if (hdrs != IntPtr.Zero)
          {
            IntPtr namep;
            IntPtr valuep;
            IntPtr sizep;
            for (int idx = 0; Librdkafka.header_get_all(hdrs, (IntPtr) idx, out namep, out valuep, out sizep) == ErrorCode.NoError; ++idx)
            {
              string stringUtF8 = Util.Marshal.PtrToStringUTF8(namep);
              byte[] numArray = (byte[]) null;
              if (valuep != IntPtr.Zero)
              {
                numArray = new byte[(int) sizep];
                System.Runtime.InteropServices.Marshal.Copy(valuep, numArray, 0, (int) sizep);
              }
              headers.Add(stringUtF8, numArray);
            }
          }
        }
        IntPtr tstype = (IntPtr) 0;
        long unixTimestampMs = 0;
        if (this.enableDeliveryReportTimestamp)
          unixTimestampMs = Librdkafka.message_timestamp(rkmessage, out tstype);
        PersistenceStatus persistenceStatus = PersistenceStatus.PossiblyPersisted;
        if (this.enableDeliveryReportPersistedStatus)
          persistenceStatus = Librdkafka.message_status(rkmessage);
        IDeliveryHandler deliveryHandler = target;
        DeliveryReport<Null, Null> deliveryReport = new DeliveryReport<Null, Null>();
        deliveryReport.Partition = (Partition) structure.partition;
        deliveryReport.Offset = (Offset) structure.offset;
        deliveryReport.Error = this.KafkaHandle.CreatePossiblyFatalError(structure.err, (string) null);
        deliveryReport.Status = persistenceStatus;
        Message<Null, Null> message = new Message<Null, Null>();
        message.Timestamp = new Timestamp(unixTimestampMs, (TimestampType) (int) tstype);
        message.Headers = headers;
        deliveryReport.Message = message;
        deliveryHandler.HandleDeliveryReport(deliveryReport);
      }
      catch (Exception ex)
      {
        this.handlerException = ex;
      }
    }

    private void ProduceImpl(
      string topic,
      byte[] val,
      int valOffset,
      int valLength,
      byte[] key,
      int keyOffset,
      int keyLength,
      Timestamp timestamp,
      Partition partition,
      IEnumerable<IHeader> headers,
      IDeliveryHandler deliveryHandler)
    {
      if (timestamp.Type != TimestampType.CreateTime && timestamp != Timestamp.Default)
        throw new ArgumentException("Timestamp must be either Timestamp.Default, or Timestamp.CreateTime.");
      ErrorCode err;
      if (this.enableDeliveryReports && deliveryHandler != null)
      {
        GCHandle gcHandle = GCHandle.Alloc((object) deliveryHandler);
        IntPtr intPtr = GCHandle.ToIntPtr(gcHandle);
        err = this.KafkaHandle.Produce(topic, val, valOffset, valLength, key, keyOffset, keyLength, partition.Value, timestamp.UnixTimestampMs, headers, intPtr);
        if (err != ErrorCode.NoError)
          gcHandle.Free();
      }
      else
        err = this.KafkaHandle.Produce(topic, val, valOffset, valLength, key, keyOffset, keyLength, partition.Value, timestamp.UnixTimestampMs, headers, IntPtr.Zero);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.KafkaHandle.CreatePossiblyFatalError(err, (string) null));
    }

    /// <inheritdoc />
    public int Poll(TimeSpan timeout)
    {
      if (this.manualPoll)
        return this.KafkaHandle.Poll((IntPtr) timeout.TotalMillisecondsAsInt());
      lock (this.pollSyncObj)
      {
        if (this.eventsServedCount == 0)
          Monitor.Wait(this.pollSyncObj, timeout);
        int eventsServedCount = this.eventsServedCount;
        this.eventsServedCount = 0;
        return eventsServedCount;
      }
    }

    /// <inheritdoc />
    public int Flush(TimeSpan timeout)
    {
      int num = this.KafkaHandle.Flush(timeout.TotalMillisecondsAsInt());
      if (this.handlerException == null)
        return num;
      Action<Error> errorHandler = this.errorHandler;
      if (errorHandler != null)
        errorHandler(new Error(ErrorCode.Local_Application, this.handlerException.ToString()));
      Exception handlerException = this.handlerException;
      this.handlerException = (Exception) null;
      return num;
    }

    /// <inheritdoc />
    public void Flush(CancellationToken cancellationToken)
    {
      do
      {
        int num = this.KafkaHandle.Flush(100);
        if (this.handlerException != null)
        {
          Action<Error> errorHandler = this.errorHandler;
          if (errorHandler != null)
            errorHandler(new Error(ErrorCode.Local_Application, this.handlerException.ToString()));
          Exception handlerException = this.handlerException;
          this.handlerException = (Exception) null;
        }
        if (num == 0)
          return;
      }
      while (!cancellationToken.IsCancellationRequested);
      throw new OperationCanceledException();
    }

    /// <inheritdoc />
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the
    ///     <see cref="T:Confluent.Kafka.Producer`2" />
    ///     and optionally disposes the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources;
    ///     false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      lock (this.disposeHasBeenCalledLockObj)
      {
        if (this.disposeHasBeenCalled)
          return;
        this.disposeHasBeenCalled = true;
      }
      if (this.ownedKafkaHandle == null || !disposing)
        return;
      if (!this.manualPoll)
      {
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
      }
      this.ownedKafkaHandle.Dispose();
    }

    /// <inheritdoc />
    public string Name => this.KafkaHandle.Name;

    /// <inheritdoc />
    public int AddBrokers(string brokers) => this.KafkaHandle.AddBrokers(brokers);

    /// <inheritdoc />
    public Handle Handle
    {
      get
      {
        if (this.ownedKafkaHandle == null)
          return this.borrowedHandle;
        return new Handle()
        {
          Owner = (IClient) this,
          LibrdkafkaHandle = this.ownedKafkaHandle
        };
      }
    }

    private void InitializeSerializers(
      ISerializer<TKey> keySerializer,
      ISerializer<TValue> valueSerializer,
      IAsyncSerializer<TKey> asyncKeySerializer,
      IAsyncSerializer<TValue> asyncValueSerializer)
    {
      if (keySerializer == null && asyncKeySerializer == null)
      {
        object obj;
        if (!Producer<TKey, TValue>.defaultSerializers.TryGetValue(typeof (TKey), out obj))
          throw new ArgumentNullException("Key serializer not specified and there is no default serializer defined for type " + typeof (TKey).Name + ".");
        this.keySerializer = (ISerializer<TKey>) obj;
      }
      else if (keySerializer == null && asyncKeySerializer != null)
        this.asyncKeySerializer = asyncKeySerializer;
      else
        this.keySerializer = keySerializer != null && asyncKeySerializer == null ? keySerializer : throw new InvalidOperationException("FATAL: Both async and sync key serializers were set.");
      if (valueSerializer == null && asyncValueSerializer == null)
      {
        object obj;
        if (!Producer<TKey, TValue>.defaultSerializers.TryGetValue(typeof (TValue), out obj))
          throw new ArgumentNullException("Value serializer not specified and there is no default serializer defined for type " + typeof (TValue).Name + ".");
        this.valueSerializer = (ISerializer<TValue>) obj;
      }
      else if (valueSerializer == null && asyncValueSerializer != null)
        this.asyncValueSerializer = asyncValueSerializer;
      else
        this.valueSerializer = valueSerializer != null && asyncValueSerializer == null ? valueSerializer : throw new InvalidOperationException("FATAL: Both async and sync value serializers were set.");
    }

    internal Producer(DependentProducerBuilder<TKey, TValue> builder)
    {
      this.borrowedHandle = builder.Handle;
      if (!this.borrowedHandle.Owner.GetType().Name.Contains(nameof (Producer<TKey, TValue>)))
        throw new Exception("A Producer instance may only be constructed using the handle of another Producer instance.");
      this.InitializeSerializers(builder.KeySerializer, builder.ValueSerializer, builder.AsyncKeySerializer, builder.AsyncValueSerializer);
    }

    internal Producer(ProducerBuilder<TKey, TValue> builder)
    {
      Producer<TKey, TValue>.Config config = builder.ConstructBaseConfig(this);
      this.statisticsHandler = config.statisticsHandler;
      this.logHandler = config.logHandler;
      this.errorHandler = config.errorHandler;
      IEnumerable<KeyValuePair<string, string>> cancellationDelayMaxMs = Confluent.Kafka.Config.ExtractCancellationDelayMaxMs(config.config, out this.cancellationDelayMaxMs);
      this.DeliveryReportCallback = new Librdkafka.DeliveryReportDelegate(this.DeliveryReportCallbackImpl);
      Librdkafka.Initialize((string) null);
      List<KeyValuePair<string, string>> list = Library.NameAndVersionConfig.Concat<KeyValuePair<string, string>>(cancellationDelayMaxMs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key != "dotnet.producer.enable.background.poll" && prop.Key != "dotnet.producer.enable.delivery.reports" && prop.Key != "dotnet.producer.delivery.report.fields"))).ToList<KeyValuePair<string, string>>();
      if (list.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (obj => obj.Key == "delivery.report.only.error")).Count<KeyValuePair<string, string>>() > 0)
        throw new ArgumentException("The 'delivery.report.only.error' property is not supported by this client");
      KeyValuePair<string, string> keyValuePair = cancellationDelayMaxMs.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key == "dotnet.producer.enable.background.poll"));
      string str1 = keyValuePair.Value;
      if (str1 != null)
        this.manualPoll = !bool.Parse(str1);
      keyValuePair = cancellationDelayMaxMs.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key == "dotnet.producer.enable.delivery.reports"));
      string str2 = keyValuePair.Value;
      if (str2 != null)
        this.enableDeliveryReports = bool.Parse(str2);
      keyValuePair = cancellationDelayMaxMs.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key == "dotnet.producer.delivery.report.fields"));
      string str3 = keyValuePair.Value;
      if (str3 != null)
      {
        string str4 = str3.Replace(" ", "");
        if (str4 != "all")
        {
          this.enableDeliveryReportKey = false;
          this.enableDeliveryReportValue = false;
          this.enableDeliveryReportHeaders = false;
          this.enableDeliveryReportTimestamp = false;
          this.enableDeliveryReportPersistedStatus = false;
          if (str4 != "none")
          {
            string str5 = str4;
            char[] chArray = new char[1]{ ',' };
            foreach (string str6 in str5.Split(chArray))
            {
              switch (str6)
              {
                case "key":
                  this.enableDeliveryReportKey = true;
                  break;
                case "value":
                  this.enableDeliveryReportValue = true;
                  break;
                case "timestamp":
                  this.enableDeliveryReportTimestamp = true;
                  break;
                case "headers":
                  this.enableDeliveryReportHeaders = true;
                  break;
                case "status":
                  this.enableDeliveryReportPersistedStatus = true;
                  break;
                default:
                  throw new ArgumentException("Unknown delivery report field name '" + str6 + "' in config value 'dotnet.producer.delivery.report.fields'.");
              }
            }
          }
        }
      }
      SafeConfigHandle configHandle = SafeConfigHandle.Create();
      list.ForEach((Action<KeyValuePair<string, string>>) (kvp =>
      {
        if (kvp.Value == null)
          throw new ArgumentNullException("'" + kvp.Key + "' configuration parameter must not be null.");
        configHandle.Set(kvp.Key, kvp.Value);
      }));
      IntPtr handle = configHandle.DangerousGetHandle();
      if (this.enableDeliveryReports)
        Librdkafka.conf_set_dr_msg_cb(handle, this.DeliveryReportCallback);
      this.errorCallbackDelegate = new Librdkafka.ErrorDelegate(this.ErrorCallback);
      this.logCallbackDelegate = new Librdkafka.LogDelegate(this.LogCallback);
      this.statisticsCallbackDelegate = new Librdkafka.StatsDelegate(this.StatisticsCallback);
      if (this.errorHandler != null)
        Librdkafka.conf_set_error_cb(handle, this.errorCallbackDelegate);
      if (this.logHandler != null)
        Librdkafka.conf_set_log_cb(handle, this.logCallbackDelegate);
      if (this.statisticsHandler != null)
        Librdkafka.conf_set_stats_cb(handle, this.statisticsCallbackDelegate);
      this.ownedKafkaHandle = SafeKafkaHandle.Create(RdKafkaType.Producer, handle, (IClient) this);
      configHandle.SetHandleAsInvalid();
      if (!this.manualPoll)
      {
        this.callbackCts = new CancellationTokenSource();
        this.callbackTask = this.StartPollTask(this.callbackCts.Token);
      }
      this.InitializeSerializers(builder.KeySerializer, builder.ValueSerializer, builder.AsyncKeySerializer, builder.AsyncValueSerializer);
    }

    /// <inheritdoc />
    public async Task<DeliveryResult<TKey, TValue>> ProduceAsync(
      TopicPartition topicPartition,
      Message<TKey, TValue> message,
      CancellationToken cancellationToken)
    {
      Headers headers = message.Headers ?? new Headers();
      byte[] keyBytes;
      ConfiguredTaskAwaitable<byte[]> configuredTaskAwaitable;
      try
      {
        byte[] numArray;
        if (this.keySerializer != null)
        {
          numArray = this.keySerializer.Serialize(message.Key, new SerializationContext(MessageComponentType.Key, topicPartition.Topic, headers));
        }
        else
        {
          configuredTaskAwaitable = this.asyncKeySerializer.SerializeAsync(message.Key, new SerializationContext(MessageComponentType.Key, topicPartition.Topic, headers)).ConfigureAwait(false);
          numArray = await configuredTaskAwaitable;
        }
        keyBytes = numArray;
      }
      catch (Exception ex)
      {
        Error error = new Error(ErrorCode.Local_KeySerialization);
        DeliveryResult<TKey, TValue> deliveryResult = new DeliveryResult<TKey, TValue>();
        deliveryResult.Message = message;
        deliveryResult.TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset);
        Exception innerException = ex;
        throw new ProduceException<TKey, TValue>(error, deliveryResult, innerException);
      }
      byte[] val;
      try
      {
        byte[] numArray;
        if (this.valueSerializer != null)
        {
          numArray = this.valueSerializer.Serialize(message.Value, new SerializationContext(MessageComponentType.Value, topicPartition.Topic, headers));
        }
        else
        {
          configuredTaskAwaitable = this.asyncValueSerializer.SerializeAsync(message.Value, new SerializationContext(MessageComponentType.Value, topicPartition.Topic, headers)).ConfigureAwait(false);
          numArray = await configuredTaskAwaitable;
        }
        val = numArray;
      }
      catch (Exception ex)
      {
        Error error = new Error(ErrorCode.Local_ValueSerialization);
        DeliveryResult<TKey, TValue> deliveryResult = new DeliveryResult<TKey, TValue>();
        deliveryResult.Message = message;
        deliveryResult.TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset);
        Exception innerException = ex;
        throw new ProduceException<TKey, TValue>(error, deliveryResult, innerException);
      }
      try
      {
        if (this.enableDeliveryReports)
        {
          Producer<TKey, TValue>.TypedTaskDeliveryHandlerShim handler = new Producer<TKey, TValue>.TypedTaskDeliveryHandlerShim(topicPartition.Topic, this.enableDeliveryReportKey ? message.Key : default (TKey), this.enableDeliveryReportValue ? message.Value : default (TValue));
          if (cancellationToken.CanBeCanceled)
            handler.CancellationTokenRegistration = cancellationToken.Register((Action) (() => handler.TrySetCanceled()));
          this.ProduceImpl(topicPartition.Topic, val, 0, val == null ? 0 : val.Length, keyBytes, 0, keyBytes == null ? 0 : keyBytes.Length, message.Timestamp, topicPartition.Partition, (IEnumerable<IHeader>) headers, (IDeliveryHandler) handler);
          return await handler.Task.ConfigureAwait(false);
        }
        this.ProduceImpl(topicPartition.Topic, val, 0, val == null ? 0 : val.Length, keyBytes, 0, keyBytes == null ? 0 : keyBytes.Length, message.Timestamp, topicPartition.Partition, (IEnumerable<IHeader>) headers, (IDeliveryHandler) null);
        return new DeliveryResult<TKey, TValue>()
        {
          TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset),
          Message = message
        };
      }
      catch (KafkaException ex)
      {
        throw new ProduceException<TKey, TValue>(ex.Error, new DeliveryResult<TKey, TValue>()
        {
          Message = message,
          TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset)
        });
      }
    }

    /// <inheritdoc />
    public Task<DeliveryResult<TKey, TValue>> ProduceAsync(
      string topic,
      Message<TKey, TValue> message,
      CancellationToken cancellationToken)
    {
      return this.ProduceAsync(new TopicPartition(topic, Partition.Any), message, cancellationToken);
    }

    /// <inheritdoc />
    public void Produce(
      string topic,
      Message<TKey, TValue> message,
      Action<DeliveryReport<TKey, TValue>> deliveryHandler = null)
    {
      this.Produce(new TopicPartition(topic, Partition.Any), message, deliveryHandler);
    }

    /// <inheritdoc />
    public void Produce(
      TopicPartition topicPartition,
      Message<TKey, TValue> message,
      Action<DeliveryReport<TKey, TValue>> deliveryHandler = null)
    {
      if (deliveryHandler != null && !this.enableDeliveryReports)
        throw new InvalidOperationException("A delivery handler was specified, but delivery reports are disabled.");
      Headers headers = message.Headers ?? new Headers();
      byte[] key;
      try
      {
        if (this.keySerializer == null)
          throw new InvalidOperationException("Produce called with an IAsyncSerializer key serializer configured but an ISerializer is required.");
        key = this.keySerializer.Serialize(message.Key, new SerializationContext(MessageComponentType.Key, topicPartition.Topic, headers));
      }
      catch (Exception ex)
      {
        throw new ProduceException<TKey, TValue>(new Error(ErrorCode.Local_KeySerialization, ex.ToString()), new DeliveryResult<TKey, TValue>()
        {
          Message = message,
          TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset)
        });
      }
      byte[] val;
      try
      {
        if (this.valueSerializer == null)
          throw new InvalidOperationException("Produce called with an IAsyncSerializer value serializer configured but an ISerializer is required.");
        val = this.valueSerializer.Serialize(message.Value, new SerializationContext(MessageComponentType.Value, topicPartition.Topic, headers));
      }
      catch (Exception ex)
      {
        throw new ProduceException<TKey, TValue>(new Error(ErrorCode.Local_ValueSerialization, ex.ToString()), new DeliveryResult<TKey, TValue>()
        {
          Message = message,
          TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset)
        });
      }
      try
      {
        this.ProduceImpl(topicPartition.Topic, val, 0, val == null ? 0 : val.Length, key, 0, key == null ? 0 : key.Length, message.Timestamp, topicPartition.Partition, (IEnumerable<IHeader>) headers, (IDeliveryHandler) new Producer<TKey, TValue>.TypedDeliveryHandlerShim_Action(topicPartition.Topic, this.enableDeliveryReportKey ? message.Key : default (TKey), this.enableDeliveryReportValue ? message.Value : default (TValue), deliveryHandler));
      }
      catch (KafkaException ex)
      {
        Error error = ex.Error;
        DeliveryReport<TKey, TValue> deliveryReport = new DeliveryReport<TKey, TValue>();
        deliveryReport.Message = message;
        deliveryReport.TopicPartitionOffset = new TopicPartitionOffset(topicPartition, Offset.Unset);
        throw new ProduceException<TKey, TValue>(error, (DeliveryResult<TKey, TValue>) deliveryReport);
      }
    }

    /// <inheritdoc />
    public void InitTransactions(TimeSpan timeout)
    {
      this.KafkaHandle.InitTransactions(timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public void BeginTransaction() => this.KafkaHandle.BeginTransaction();

    /// <inheritdoc />
    public void CommitTransaction(TimeSpan timeout)
    {
      this.KafkaHandle.CommitTransaction(timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public void AbortTransaction(TimeSpan timeout)
    {
      this.KafkaHandle.AbortTransaction(timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public void SendOffsetsToTransaction(
      IEnumerable<TopicPartitionOffset> offsets,
      IConsumerGroupMetadata groupMetadata,
      TimeSpan timeout)
    {
      this.KafkaHandle.SendOffsetsToTransaction(offsets, groupMetadata, timeout.TotalMillisecondsAsInt());
    }

    internal class Config
    {
      public IEnumerable<KeyValuePair<string, string>> config;
      public Action<Error> errorHandler;
      public Action<LogMessage> logHandler;
      public Action<string> statisticsHandler;
    }

    private class TypedTaskDeliveryHandlerShim : 
      TaskCompletionSource<DeliveryResult<TKey, TValue>>,
      IDeliveryHandler
    {
      public CancellationTokenRegistration CancellationTokenRegistration;
      public string Topic;
      public TKey Key;
      public TValue Value;

      public TypedTaskDeliveryHandlerShim(string topic, TKey key, TValue val)
      {
        this.Topic = topic;
        this.Key = key;
        this.Value = val;
      }

      public void HandleDeliveryReport(DeliveryReport<Null, Null> deliveryReport)
      {
        CancellationTokenRegistration tokenRegistration = this.CancellationTokenRegistration;
        this.CancellationTokenRegistration.Dispose();
        if (deliveryReport == null)
        {
          Task.Run<bool>((Func<bool>) (() => this.TrySetResult((DeliveryResult<TKey, TValue>) null)));
        }
        else
        {
          DeliveryResult<TKey, TValue> deliveryResult = new DeliveryResult<TKey, TValue>();
          deliveryResult.TopicPartitionOffset = deliveryReport.TopicPartitionOffset;
          deliveryResult.Status = deliveryReport.Status;
          Message<TKey, TValue> message = new Message<TKey, TValue>();
          message.Key = this.Key;
          message.Value = this.Value;
          message.Timestamp = deliveryReport.Message.Timestamp;
          message.Headers = deliveryReport.Message.Headers;
          deliveryResult.Message = message;
          DeliveryResult<TKey, TValue> dr = deliveryResult;
          dr.Topic = this.Topic;
          if (deliveryReport.Error.IsError)
            Task.Run((Action) (() => this.SetException((Exception) new ProduceException<TKey, TValue>(deliveryReport.Error, dr))));
          else
            Task.Run<bool>((Func<bool>) (() => this.TrySetResult(dr)));
        }
      }
    }

    private class TypedDeliveryHandlerShim_Action : IDeliveryHandler
    {
      public string Topic;
      public TKey Key;
      public TValue Value;
      public Action<DeliveryReport<TKey, TValue>> Handler;

      public TypedDeliveryHandlerShim_Action(
        string topic,
        TKey key,
        TValue val,
        Action<DeliveryReport<TKey, TValue>> handler)
      {
        this.Topic = topic;
        this.Key = key;
        this.Value = val;
        this.Handler = handler;
      }

      public void HandleDeliveryReport(DeliveryReport<Null, Null> deliveryReport)
      {
        if (deliveryReport == null)
          return;
        DeliveryReport<TKey, TValue> deliveryReport1 = new DeliveryReport<TKey, TValue>();
        deliveryReport1.TopicPartitionOffsetError = deliveryReport.TopicPartitionOffsetError;
        deliveryReport1.Status = deliveryReport.Status;
        Message<TKey, TValue> message = new Message<TKey, TValue>();
        message.Key = this.Key;
        message.Value = this.Value;
        message.Timestamp = deliveryReport.Message == null ? new Timestamp(0L, TimestampType.NotAvailable) : deliveryReport.Message.Timestamp;
        message.Headers = deliveryReport.Message?.Headers;
        deliveryReport1.Message = message;
        DeliveryReport<TKey, TValue> deliveryReport2 = deliveryReport1;
        deliveryReport2.Topic = this.Topic;
        if (this.Handler == null)
          return;
        this.Handler(deliveryReport2);
      }
    }
  }
}
