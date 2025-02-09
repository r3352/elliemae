// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ClientConfig
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Configuration common to all clients</summary>
  public class ClientConfig : Config
  {
    /// <summary>
    ///     Initialize a new empty <see cref="T:Confluent.Kafka.ClientConfig" /> instance.
    /// </summary>
    public ClientConfig()
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ClientConfig" /> instance wrapping
    ///     an existing <see cref="T:Confluent.Kafka.ClientConfig" /> instance.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ClientConfig(ClientConfig config)
      : base((Config) config)
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ClientConfig" /> instance wrapping
    ///     an existing key/value pair collection.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ClientConfig(IDictionary<string, string> config)
      : base(config)
    {
    }

    /// <summary>
    ///     SASL mechanism to use for authentication. Supported: GSSAPI, PLAIN, SCRAM-SHA-256, SCRAM-SHA-512. **NOTE**: Despite the name, you may not configure more than one mechanism.
    /// </summary>
    public Confluent.Kafka.SaslMechanism? SaslMechanism
    {
      get
      {
        string str = this.Get("sasl.mechanism");
        switch (str)
        {
          case null:
            return new Confluent.Kafka.SaslMechanism?();
          case "GSSAPI":
            return new Confluent.Kafka.SaslMechanism?(Confluent.Kafka.SaslMechanism.Gssapi);
          case "PLAIN":
            return new Confluent.Kafka.SaslMechanism?(Confluent.Kafka.SaslMechanism.Plain);
          case "SCRAM-SHA-256":
            return new Confluent.Kafka.SaslMechanism?(Confluent.Kafka.SaslMechanism.ScramSha256);
          case "SCRAM-SHA-512":
            return new Confluent.Kafka.SaslMechanism?(Confluent.Kafka.SaslMechanism.ScramSha512);
          default:
            throw new ArgumentException("Unknown sasl.mechanism value " + str);
        }
      }
      set
      {
        if (!value.HasValue)
        {
          this.properties.Remove("sasl.mechanism");
        }
        else
        {
          Confluent.Kafka.SaslMechanism? nullable = value;
          Confluent.Kafka.SaslMechanism saslMechanism1 = Confluent.Kafka.SaslMechanism.Gssapi;
          if (nullable.GetValueOrDefault() == saslMechanism1 & nullable.HasValue)
          {
            this.properties["sasl.mechanism"] = "GSSAPI";
          }
          else
          {
            nullable = value;
            Confluent.Kafka.SaslMechanism saslMechanism2 = Confluent.Kafka.SaslMechanism.Plain;
            if (nullable.GetValueOrDefault() == saslMechanism2 & nullable.HasValue)
            {
              this.properties["sasl.mechanism"] = "PLAIN";
            }
            else
            {
              nullable = value;
              Confluent.Kafka.SaslMechanism saslMechanism3 = Confluent.Kafka.SaslMechanism.ScramSha256;
              if (nullable.GetValueOrDefault() == saslMechanism3 & nullable.HasValue)
              {
                this.properties["sasl.mechanism"] = "SCRAM-SHA-256";
              }
              else
              {
                nullable = value;
                Confluent.Kafka.SaslMechanism saslMechanism4 = Confluent.Kafka.SaslMechanism.ScramSha512;
                if (!(nullable.GetValueOrDefault() == saslMechanism4 & nullable.HasValue))
                  throw new ArgumentException(string.Format("Unknown sasl.mechanism value {0}", (object) value));
                this.properties["sasl.mechanism"] = "SCRAM-SHA-512";
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///     This field indicates the number of acknowledgements the leader broker must receive from ISR brokers
    ///     before responding to the request: Zero=Broker does not send any response/ack to client, One=The
    ///     leader will write the record to its local log but will respond without awaiting full acknowledgement
    ///     from all followers. All=Broker will block until message is committed by all in sync replicas (ISRs).
    ///     If there are less than min.insync.replicas (broker configuration) in the ISR set the produce request
    ///     will fail.
    /// </summary>
    public Confluent.Kafka.Acks? Acks
    {
      get
      {
        string s = this.Get("acks");
        switch (s)
        {
          case null:
            return new Confluent.Kafka.Acks?();
          case "0":
            return new Confluent.Kafka.Acks?(Confluent.Kafka.Acks.None);
          case "1":
            return new Confluent.Kafka.Acks?(Confluent.Kafka.Acks.Leader);
          case "-1":
          case "all":
            return new Confluent.Kafka.Acks?(Confluent.Kafka.Acks.All);
          default:
            return new Confluent.Kafka.Acks?((Confluent.Kafka.Acks) int.Parse(s));
        }
      }
      set
      {
        if (!value.HasValue)
        {
          this.properties.Remove("acks");
        }
        else
        {
          Confluent.Kafka.Acks? nullable = value;
          Confluent.Kafka.Acks acks1 = Confluent.Kafka.Acks.None;
          if (nullable.GetValueOrDefault() == acks1 & nullable.HasValue)
          {
            this.properties["acks"] = "0";
          }
          else
          {
            nullable = value;
            Confluent.Kafka.Acks acks2 = Confluent.Kafka.Acks.Leader;
            if (nullable.GetValueOrDefault() == acks2 & nullable.HasValue)
            {
              this.properties["acks"] = "1";
            }
            else
            {
              nullable = value;
              Confluent.Kafka.Acks acks3 = Confluent.Kafka.Acks.All;
              if (nullable.GetValueOrDefault() == acks3 & nullable.HasValue)
                this.properties["acks"] = "-1";
              else
                this.properties["acks"] = ((int) value.Value).ToString();
            }
          }
        }
      }
    }

    /// <summary>
    ///     Client identifier.
    /// 
    ///     default: rdkafka
    ///     importance: low
    /// </summary>
    public string ClientId
    {
      get => this.Get("client.id");
      set => this.SetObject("client.id", (object) value);
    }

    /// <summary>
    ///     Initial list of brokers as a CSV list of broker host or host:port. The application may also use `rd_kafka_brokers_add()` to add brokers during runtime.
    /// 
    ///     default: ''
    ///     importance: high
    /// </summary>
    public string BootstrapServers
    {
      get => this.Get("bootstrap.servers");
      set => this.SetObject("bootstrap.servers", (object) value);
    }

    /// <summary>
    ///     Maximum Kafka protocol request message size. Due to differing framing overhead between protocol versions the producer is unable to reliably enforce a strict max message limit at produce time and may exceed the maximum size by one message in protocol ProduceRequests, the broker will enforce the the topic's `max.message.bytes` limit (see Apache Kafka documentation).
    /// 
    ///     default: 1000000
    ///     importance: medium
    /// </summary>
    public int? MessageMaxBytes
    {
      get => this.GetInt("message.max.bytes");
      set => this.SetObject("message.max.bytes", (object) value);
    }

    /// <summary>
    ///     Maximum size for message to be copied to buffer. Messages larger than this will be passed by reference (zero-copy) at the expense of larger iovecs.
    /// 
    ///     default: 65535
    ///     importance: low
    /// </summary>
    public int? MessageCopyMaxBytes
    {
      get => this.GetInt("message.copy.max.bytes");
      set => this.SetObject("message.copy.max.bytes", (object) value);
    }

    /// <summary>
    ///     Maximum Kafka protocol response message size. This serves as a safety precaution to avoid memory exhaustion in case of protocol hickups. This value must be at least `fetch.max.bytes`  + 512 to allow for protocol overhead; the value is adjusted automatically unless the configuration property is explicitly set.
    /// 
    ///     default: 100000000
    ///     importance: medium
    /// </summary>
    public int? ReceiveMessageMaxBytes
    {
      get => this.GetInt("receive.message.max.bytes");
      set => this.SetObject("receive.message.max.bytes", (object) value);
    }

    /// <summary>
    ///     Maximum number of in-flight requests per broker connection. This is a generic property applied to all broker communication, however it is primarily relevant to produce requests. In particular, note that other mechanisms limit the number of outstanding consumer fetch request per broker to one.
    /// 
    ///     default: 1000000
    ///     importance: low
    /// </summary>
    public int? MaxInFlight
    {
      get => this.GetInt("max.in.flight");
      set => this.SetObject("max.in.flight", (object) value);
    }

    /// <summary>
    ///     Non-topic request timeout in milliseconds. This is for metadata requests, etc.
    /// 
    ///     default: 60000
    ///     importance: low
    /// </summary>
    public int? MetadataRequestTimeoutMs
    {
      get => this.GetInt("metadata.request.timeout.ms");
      set => this.SetObject("metadata.request.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Period of time in milliseconds at which topic and broker metadata is refreshed in order to proactively discover any new brokers, topics, partitions or partition leader changes. Use -1 to disable the intervalled refresh (not recommended). If there are no locally referenced topics (no topic objects created, no messages produced, no subscription or no assignment) then only the broker list will be refreshed every interval but no more often than every 10s.
    /// 
    ///     default: 300000
    ///     importance: low
    /// </summary>
    public int? TopicMetadataRefreshIntervalMs
    {
      get => this.GetInt("topic.metadata.refresh.interval.ms");
      set => this.SetObject("topic.metadata.refresh.interval.ms", (object) value);
    }

    /// <summary>
    ///     Metadata cache max age. Defaults to topic.metadata.refresh.interval.ms * 3
    /// 
    ///     default: 900000
    ///     importance: low
    /// </summary>
    public int? MetadataMaxAgeMs
    {
      get => this.GetInt("metadata.max.age.ms");
      set => this.SetObject("metadata.max.age.ms", (object) value);
    }

    /// <summary>
    ///     When a topic loses its leader a new metadata request will be enqueued with this initial interval, exponentially increasing until the topic metadata has been refreshed. This is used to recover quickly from transitioning leader brokers.
    /// 
    ///     default: 250
    ///     importance: low
    /// </summary>
    public int? TopicMetadataRefreshFastIntervalMs
    {
      get => this.GetInt("topic.metadata.refresh.fast.interval.ms");
      set => this.SetObject("topic.metadata.refresh.fast.interval.ms", (object) value);
    }

    /// <summary>
    ///     Sparse metadata requests (consumes less network bandwidth)
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? TopicMetadataRefreshSparse
    {
      get => this.GetBool("topic.metadata.refresh.sparse");
      set => this.SetObject("topic.metadata.refresh.sparse", (object) value);
    }

    /// <summary>
    ///     Topic blacklist, a comma-separated list of regular expressions for matching topic names that should be ignored in broker metadata information as if the topics did not exist.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string TopicBlacklist
    {
      get => this.Get("topic.blacklist");
      set => this.SetObject("topic.blacklist", (object) value);
    }

    /// <summary>
    ///     A comma-separated list of debug contexts to enable. Detailed Producer debugging: broker,topic,msg. Consumer: consumer,cgrp,topic,fetch
    /// 
    ///     default: ''
    ///     importance: medium
    /// </summary>
    public string Debug
    {
      get => this.Get("debug");
      set => this.SetObject("debug", (object) value);
    }

    /// <summary>
    ///     Default timeout for network requests. Producer: ProduceRequests will use the lesser value of `socket.timeout.ms` and remaining `message.timeout.ms` for the first message in the batch. Consumer: FetchRequests will use `fetch.wait.max.ms` + `socket.timeout.ms`. Admin: Admin requests will use `socket.timeout.ms` or explicitly set `rd_kafka_AdminOptions_set_operation_timeout()` value.
    /// 
    ///     default: 60000
    ///     importance: low
    /// </summary>
    public int? SocketTimeoutMs
    {
      get => this.GetInt("socket.timeout.ms");
      set => this.SetObject("socket.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Broker socket send buffer size. System default is used if 0.
    /// 
    ///     default: 0
    ///     importance: low
    /// </summary>
    public int? SocketSendBufferBytes
    {
      get => this.GetInt("socket.send.buffer.bytes");
      set => this.SetObject("socket.send.buffer.bytes", (object) value);
    }

    /// <summary>
    ///     Broker socket receive buffer size. System default is used if 0.
    /// 
    ///     default: 0
    ///     importance: low
    /// </summary>
    public int? SocketReceiveBufferBytes
    {
      get => this.GetInt("socket.receive.buffer.bytes");
      set => this.SetObject("socket.receive.buffer.bytes", (object) value);
    }

    /// <summary>
    ///     Enable TCP keep-alives (SO_KEEPALIVE) on broker sockets
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? SocketKeepaliveEnable
    {
      get => this.GetBool("socket.keepalive.enable");
      set => this.SetObject("socket.keepalive.enable", (object) value);
    }

    /// <summary>
    ///     Disable the Nagle algorithm (TCP_NODELAY) on broker sockets.
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? SocketNagleDisable
    {
      get => this.GetBool("socket.nagle.disable");
      set => this.SetObject("socket.nagle.disable", (object) value);
    }

    /// <summary>
    ///     Disconnect from broker when this number of send failures (e.g., timed out requests) is reached. Disable with 0. WARNING: It is highly recommended to leave this setting at its default value of 1 to avoid the client and broker to become desynchronized in case of request timeouts. NOTE: The connection is automatically re-established.
    /// 
    ///     default: 1
    ///     importance: low
    /// </summary>
    public int? SocketMaxFails
    {
      get => this.GetInt("socket.max.fails");
      set => this.SetObject("socket.max.fails", (object) value);
    }

    /// <summary>
    ///     How long to cache the broker address resolving results (milliseconds).
    /// 
    ///     default: 1000
    ///     importance: low
    /// </summary>
    public int? BrokerAddressTtl
    {
      get => this.GetInt("broker.address.ttl");
      set => this.SetObject("broker.address.ttl", (object) value);
    }

    /// <summary>
    ///     Allowed broker IP address families: any, v4, v6
    /// 
    ///     default: any
    ///     importance: low
    /// </summary>
    public Confluent.Kafka.BrokerAddressFamily? BrokerAddressFamily
    {
      get
      {
        return (Confluent.Kafka.BrokerAddressFamily?) this.GetEnum(typeof (Confluent.Kafka.BrokerAddressFamily), "broker.address.family");
      }
      set => this.SetObject("broker.address.family", (object) value);
    }

    /// <summary>
    ///     The initial time to wait before reconnecting to a broker after the connection has been closed. The time is increased exponentially until `reconnect.backoff.max.ms` is reached. -25% to +50% jitter is applied to each reconnect backoff. A value of 0 disables the backoff and reconnects immediately.
    /// 
    ///     default: 100
    ///     importance: medium
    /// </summary>
    public int? ReconnectBackoffMs
    {
      get => this.GetInt("reconnect.backoff.ms");
      set => this.SetObject("reconnect.backoff.ms", (object) value);
    }

    /// <summary>
    ///     The maximum time to wait before reconnecting to a broker after the connection has been closed.
    /// 
    ///     default: 10000
    ///     importance: medium
    /// </summary>
    public int? ReconnectBackoffMaxMs
    {
      get => this.GetInt("reconnect.backoff.max.ms");
      set => this.SetObject("reconnect.backoff.max.ms", (object) value);
    }

    /// <summary>
    ///     librdkafka statistics emit interval. The application also needs to register a stats callback using `rd_kafka_conf_set_stats_cb()`. The granularity is 1000ms. A value of 0 disables statistics.
    /// 
    ///     default: 0
    ///     importance: high
    /// </summary>
    public int? StatisticsIntervalMs
    {
      get => this.GetInt("statistics.interval.ms");
      set => this.SetObject("statistics.interval.ms", (object) value);
    }

    /// <summary>
    ///     Disable spontaneous log_cb from internal librdkafka threads, instead enqueue log messages on queue set with `rd_kafka_set_log_queue()` and serve log callbacks or events through the standard poll APIs. **NOTE**: Log messages will linger in a temporary queue until the log queue has been set.
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? LogQueue
    {
      get => this.GetBool("log.queue");
      set => this.SetObject("log.queue", (object) value);
    }

    /// <summary>
    ///     Print internal thread name in log messages (useful for debugging librdkafka internals)
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? LogThreadName
    {
      get => this.GetBool("log.thread.name");
      set => this.SetObject("log.thread.name", (object) value);
    }

    /// <summary>
    ///     Log broker disconnects. It might be useful to turn this off when interacting with 0.9 brokers with an aggressive `connection.max.idle.ms` value.
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? LogConnectionClose
    {
      get => this.GetBool("log.connection.close");
      set => this.SetObject("log.connection.close", (object) value);
    }

    /// <summary>
    ///     Signal that librdkafka will use to quickly terminate on rd_kafka_destroy(). If this signal is not set then there will be a delay before rd_kafka_wait_destroyed() returns true as internal threads are timing out their system calls. If this signal is set however the delay will be minimal. The application should mask this signal as an internal signal handler is installed.
    /// 
    ///     default: 0
    ///     importance: low
    /// </summary>
    public int? InternalTerminationSignal
    {
      get => this.GetInt("internal.termination.signal");
      set => this.SetObject("internal.termination.signal", (object) value);
    }

    /// <summary>
    ///     Request broker's supported API versions to adjust functionality to available protocol features. If set to false, or the ApiVersionRequest fails, the fallback version `broker.version.fallback` will be used. **NOTE**: Depends on broker version &gt;=0.10.0. If the request is not supported by (an older) broker the `broker.version.fallback` fallback is used.
    /// 
    ///     default: true
    ///     importance: high
    /// </summary>
    public bool? ApiVersionRequest
    {
      get => this.GetBool("api.version.request");
      set => this.SetObject("api.version.request", (object) value);
    }

    /// <summary>
    ///     Timeout for broker API version requests.
    /// 
    ///     default: 10000
    ///     importance: low
    /// </summary>
    public int? ApiVersionRequestTimeoutMs
    {
      get => this.GetInt("api.version.request.timeout.ms");
      set => this.SetObject("api.version.request.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Dictates how long the `broker.version.fallback` fallback is used in the case the ApiVersionRequest fails. **NOTE**: The ApiVersionRequest is only issued when a new connection to the broker is made (such as after an upgrade).
    /// 
    ///     default: 0
    ///     importance: medium
    /// </summary>
    public int? ApiVersionFallbackMs
    {
      get => this.GetInt("api.version.fallback.ms");
      set => this.SetObject("api.version.fallback.ms", (object) value);
    }

    /// <summary>
    ///     Older broker versions (before 0.10.0) provide no way for a client to query for supported protocol features (ApiVersionRequest, see `api.version.request`) making it impossible for the client to know what features it may use. As a workaround a user may set this property to the expected broker version and the client will automatically adjust its feature set accordingly if the ApiVersionRequest fails (or is disabled). The fallback broker version will be used for `api.version.fallback.ms`. Valid values are: 0.9.0, 0.8.2, 0.8.1, 0.8.0. Any other value &gt;= 0.10, such as 0.10.2.1, enables ApiVersionRequests.
    /// 
    ///     default: 0.10.0
    ///     importance: medium
    /// </summary>
    public string BrokerVersionFallback
    {
      get => this.Get("broker.version.fallback");
      set => this.SetObject("broker.version.fallback", (object) value);
    }

    /// <summary>
    ///     Protocol used to communicate with brokers.
    /// 
    ///     default: plaintext
    ///     importance: high
    /// </summary>
    public Confluent.Kafka.SecurityProtocol? SecurityProtocol
    {
      get => (Confluent.Kafka.SecurityProtocol?) this.GetEnum(typeof (Confluent.Kafka.SecurityProtocol), "security.protocol");
      set => this.SetObject("security.protocol", (object) value);
    }

    /// <summary>
    ///     A cipher suite is a named combination of authentication, encryption, MAC and key exchange algorithm used to negotiate the security settings for a network connection using TLS or SSL network protocol. See manual page for `ciphers(1)` and `SSL_CTX_set_cipher_list(3).
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCipherSuites
    {
      get => this.Get("ssl.cipher.suites");
      set => this.SetObject("ssl.cipher.suites", (object) value);
    }

    /// <summary>
    ///     The supported-curves extension in the TLS ClientHello message specifies the curves (standard/named, or 'explicit' GF(2^k) or GF(p)) the client is willing to have the server use. See manual page for `SSL_CTX_set1_curves_list(3)`. OpenSSL &gt;= 1.0.2 required.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCurvesList
    {
      get => this.Get("ssl.curves.list");
      set => this.SetObject("ssl.curves.list", (object) value);
    }

    /// <summary>
    ///     The client uses the TLS ClientHello signature_algorithms extension to indicate to the server which signature/hash algorithm pairs may be used in digital signatures. See manual page for `SSL_CTX_set1_sigalgs_list(3)`. OpenSSL &gt;= 1.0.2 required.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslSigalgsList
    {
      get => this.Get("ssl.sigalgs.list");
      set => this.SetObject("ssl.sigalgs.list", (object) value);
    }

    /// <summary>
    ///     Path to client's private key (PEM) used for authentication.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslKeyLocation
    {
      get => this.Get("ssl.key.location");
      set => this.SetObject("ssl.key.location", (object) value);
    }

    /// <summary>
    ///     Private key passphrase (for use with `ssl.key.location` and `set_ssl_cert()`)
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslKeyPassword
    {
      get => this.Get("ssl.key.password");
      set => this.SetObject("ssl.key.password", (object) value);
    }

    /// <summary>
    ///     Client's private key string (PEM format) used for authentication.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslKeyPem
    {
      get => this.Get("ssl.key.pem");
      set => this.SetObject("ssl.key.pem", (object) value);
    }

    /// <summary>
    ///     Path to client's public key (PEM) used for authentication.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCertificateLocation
    {
      get => this.Get("ssl.certificate.location");
      set => this.SetObject("ssl.certificate.location", (object) value);
    }

    /// <summary>
    ///     Client's public key string (PEM format) used for authentication.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCertificatePem
    {
      get => this.Get("ssl.certificate.pem");
      set => this.SetObject("ssl.certificate.pem", (object) value);
    }

    /// <summary>
    ///     File or directory path to CA certificate(s) for verifying the broker's key.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCaLocation
    {
      get => this.Get("ssl.ca.location");
      set => this.SetObject("ssl.ca.location", (object) value);
    }

    /// <summary>
    ///     Path to CRL for verifying broker's certificate validity.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslCrlLocation
    {
      get => this.Get("ssl.crl.location");
      set => this.SetObject("ssl.crl.location", (object) value);
    }

    /// <summary>
    ///     Path to client's keystore (PKCS#12) used for authentication.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslKeystoreLocation
    {
      get => this.Get("ssl.keystore.location");
      set => this.SetObject("ssl.keystore.location", (object) value);
    }

    /// <summary>
    ///     Client's keystore (PKCS#12) password.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SslKeystorePassword
    {
      get => this.Get("ssl.keystore.password");
      set => this.SetObject("ssl.keystore.password", (object) value);
    }

    /// <summary>
    ///     Enable OpenSSL's builtin broker (server) certificate verification. This verification can be extended by the application by implementing a certificate_verify_cb.
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? EnableSslCertificateVerification
    {
      get => this.GetBool("enable.ssl.certificate.verification");
      set => this.SetObject("enable.ssl.certificate.verification", (object) value);
    }

    /// <summary>
    ///     Endpoint identification algorithm to validate broker hostname using broker certificate. https - Server (broker) hostname verification as specified in RFC2818. none - No endpoint verification. OpenSSL &gt;= 1.0.2 required.
    /// 
    ///     default: none
    ///     importance: low
    /// </summary>
    public Confluent.Kafka.SslEndpointIdentificationAlgorithm? SslEndpointIdentificationAlgorithm
    {
      get
      {
        return (Confluent.Kafka.SslEndpointIdentificationAlgorithm?) this.GetEnum(typeof (Confluent.Kafka.SslEndpointIdentificationAlgorithm), "ssl.endpoint.identification.algorithm");
      }
      set => this.SetObject("ssl.endpoint.identification.algorithm", (object) value);
    }

    /// <summary>
    ///     Kerberos principal name that Kafka runs as, not including /hostname@REALM
    /// 
    ///     default: kafka
    ///     importance: low
    /// </summary>
    public string SaslKerberosServiceName
    {
      get => this.Get("sasl.kerberos.service.name");
      set => this.SetObject("sasl.kerberos.service.name", (object) value);
    }

    /// <summary>
    ///     This client's Kerberos principal name. (Not supported on Windows, will use the logon user's principal).
    /// 
    ///     default: kafkaclient
    ///     importance: low
    /// </summary>
    public string SaslKerberosPrincipal
    {
      get => this.Get("sasl.kerberos.principal");
      set => this.SetObject("sasl.kerberos.principal", (object) value);
    }

    /// <summary>
    ///     Shell command to refresh or acquire the client's Kerberos ticket. This command is executed on client creation and every sasl.kerberos.min.time.before.relogin (0=disable). %{config.prop.name} is replaced by corresponding config object value.
    /// 
    ///     default: kinit -R -t "%{sasl.kerberos.keytab}" -k %{sasl.kerberos.principal} || kinit -t "%{sasl.kerberos.keytab}" -k %{sasl.kerberos.principal}
    ///     importance: low
    /// </summary>
    public string SaslKerberosKinitCmd
    {
      get => this.Get("sasl.kerberos.kinit.cmd");
      set => this.SetObject("sasl.kerberos.kinit.cmd", (object) value);
    }

    /// <summary>
    ///     Path to Kerberos keytab file. This configuration property is only used as a variable in `sasl.kerberos.kinit.cmd` as ` ... -t "%{sasl.kerberos.keytab}"`.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SaslKerberosKeytab
    {
      get => this.Get("sasl.kerberos.keytab");
      set => this.SetObject("sasl.kerberos.keytab", (object) value);
    }

    /// <summary>
    ///     Minimum time in milliseconds between key refresh attempts. Disable automatic key refresh by setting this property to 0.
    /// 
    ///     default: 60000
    ///     importance: low
    /// </summary>
    public int? SaslKerberosMinTimeBeforeRelogin
    {
      get => this.GetInt("sasl.kerberos.min.time.before.relogin");
      set => this.SetObject("sasl.kerberos.min.time.before.relogin", (object) value);
    }

    /// <summary>
    ///     SASL username for use with the PLAIN and SASL-SCRAM-.. mechanisms
    /// 
    ///     default: ''
    ///     importance: high
    /// </summary>
    public string SaslUsername
    {
      get => this.Get("sasl.username");
      set => this.SetObject("sasl.username", (object) value);
    }

    /// <summary>
    ///     SASL password for use with the PLAIN and SASL-SCRAM-.. mechanism
    /// 
    ///     default: ''
    ///     importance: high
    /// </summary>
    public string SaslPassword
    {
      get => this.Get("sasl.password");
      set => this.SetObject("sasl.password", (object) value);
    }

    /// <summary>
    ///     SASL/OAUTHBEARER configuration. The format is implementation-dependent and must be parsed accordingly. The default unsecured token implementation (see https://tools.ietf.org/html/rfc7515#appendix-A.5) recognizes space-separated name=value pairs with valid names including principalClaimName, principal, scopeClaimName, scope, and lifeSeconds. The default value for principalClaimName is "sub", the default value for scopeClaimName is "scope", and the default value for lifeSeconds is 3600. The scope value is CSV format with the default value being no/empty scope. For example: `principalClaimName=azp principal=admin scopeClaimName=roles scope=role1,role2 lifeSeconds=600`. In addition, SASL extensions can be communicated to the broker via `extension_NAME=value`. For example: `principal=admin extension_traceId=123`
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string SaslOauthbearerConfig
    {
      get => this.Get("sasl.oauthbearer.config");
      set => this.SetObject("sasl.oauthbearer.config", (object) value);
    }

    /// <summary>
    ///     Enable the builtin unsecure JWT OAUTHBEARER token handler if no oauthbearer_refresh_cb has been set. This builtin handler should only be used for development or testing, and not in production.
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? EnableSaslOauthbearerUnsecureJwt
    {
      get => this.GetBool("enable.sasl.oauthbearer.unsecure.jwt");
      set => this.SetObject("enable.sasl.oauthbearer.unsecure.jwt", (object) value);
    }

    /// <summary>
    ///     List of plugin libraries to load (; separated). The library search path is platform dependent (see dlopen(3) for Unix and LoadLibrary() for Windows). If no filename extension is specified the platform-specific extension (such as .dll or .so) will be appended automatically.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string PluginLibraryPaths
    {
      get => this.Get("plugin.library.paths");
      set => this.SetObject("plugin.library.paths", (object) value);
    }

    /// <summary>
    ///     A rack identifier for this client. This can be any string value which indicates where this client is physically located. It corresponds with the broker config `broker.rack`.
    /// 
    ///     default: ''
    ///     importance: low
    /// </summary>
    public string ClientRack
    {
      get => this.Get("client.rack");
      set => this.SetObject("client.rack", (object) value);
    }
  }
}
