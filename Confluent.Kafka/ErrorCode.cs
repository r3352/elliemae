// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ErrorCode
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Enumeration of local and broker generated error codes.
  /// </summary>
  /// <remarks>
  ///     Error codes that relate to locally produced errors in
  ///     librdkafka are prefixed with Local_
  /// </remarks>
  public enum ErrorCode
  {
    /// <summary>Received message is incorrect</summary>
    Local_BadMsg = -199, // 0xFFFFFF39
    /// <summary>Bad/unknown compression</summary>
    Local_BadCompression = -198, // 0xFFFFFF3A
    /// <summary>Broker is going away</summary>
    Local_Destroy = -197, // 0xFFFFFF3B
    /// <summary>Generic failure</summary>
    Local_Fail = -196, // 0xFFFFFF3C
    /// <summary>Broker transport failure</summary>
    Local_Transport = -195, // 0xFFFFFF3D
    /// <summary>Critical system resource</summary>
    Local_CritSysResource = -194, // 0xFFFFFF3E
    /// <summary>Failed to resolve broker</summary>
    Local_Resolve = -193, // 0xFFFFFF3F
    /// <summary>Produced message timed out</summary>
    Local_MsgTimedOut = -192, // 0xFFFFFF40
    /// <summary>
    ///     Reached the end of the topic+partition queue on the broker. Not really an error.
    /// </summary>
    Local_PartitionEOF = -191, // 0xFFFFFF41
    /// <summary>Permanent: Partition does not exist in cluster.</summary>
    Local_UnknownPartition = -190, // 0xFFFFFF42
    /// <summary>File or filesystem error</summary>
    Local_FS = -189, // 0xFFFFFF43
    /// <summary>Permanent: Topic does not exist in cluster.</summary>
    Local_UnknownTopic = -188, // 0xFFFFFF44
    /// <summary>All broker connections are down.</summary>
    Local_AllBrokersDown = -187, // 0xFFFFFF45
    /// <summary>Invalid argument, or invalid configuration</summary>
    Local_InvalidArg = -186, // 0xFFFFFF46
    /// <summary>Operation timed out</summary>
    Local_TimedOut = -185, // 0xFFFFFF47
    /// <summary>Queue is full</summary>
    Local_QueueFull = -184, // 0xFFFFFF48
    /// <summary>ISR count &lt; required.acks</summary>
    Local_IsrInsuff = -183, // 0xFFFFFF49
    /// <summary>Broker node update</summary>
    Local_NodeUpdate = -182, // 0xFFFFFF4A
    /// <summary>SSL error</summary>
    Local_Ssl = -181, // 0xFFFFFF4B
    /// <summary>Waiting for coordinator to become available.</summary>
    Local_WaitCoord = -180, // 0xFFFFFF4C
    /// <summary>Unknown client group</summary>
    Local_UnknownGroup = -179, // 0xFFFFFF4D
    /// <summary>Operation in progress</summary>
    Local_InProgress = -178, // 0xFFFFFF4E
    /// <summary>
    ///     Previous operation in progress, wait for it to finish.
    /// </summary>
    Local_PrevInProgress = -177, // 0xFFFFFF4F
    /// <summary>
    ///     This operation would interfere with an existing subscription
    /// </summary>
    Local_ExistingSubscription = -176, // 0xFFFFFF50
    /// <summary>Assigned partitions (rebalance_cb)</summary>
    Local_AssignPartitions = -175, // 0xFFFFFF51
    /// <summary>Revoked partitions (rebalance_cb)</summary>
    Local_RevokePartitions = -174, // 0xFFFFFF52
    /// <summary>Conflicting use</summary>
    Local_Conflict = -173, // 0xFFFFFF53
    /// <summary>Wrong state</summary>
    Local_State = -172, // 0xFFFFFF54
    /// <summary>Unknown protocol</summary>
    Local_UnknownProtocol = -171, // 0xFFFFFF55
    /// <summary>Not implemented</summary>
    Local_NotImplemented = -170, // 0xFFFFFF56
    /// <summary>Authentication failure</summary>
    Local_Authentication = -169, // 0xFFFFFF57
    /// <summary>No stored offset</summary>
    Local_NoOffset = -168, // 0xFFFFFF58
    /// <summary>Outdated</summary>
    Local_Outdated = -167, // 0xFFFFFF59
    /// <summary>Timed out in queue</summary>
    Local_TimedOutQueue = -166, // 0xFFFFFF5A
    /// <summary>Feature not supported by broker</summary>
    Local_UnsupportedFeature = -165, // 0xFFFFFF5B
    /// <summary>Awaiting cache update</summary>
    Local_WaitCache = -164, // 0xFFFFFF5C
    /// <summary>Operation interrupted</summary>
    Local_Intr = -163, // 0xFFFFFF5D
    /// <summary>Key serialization error</summary>
    Local_KeySerialization = -162, // 0xFFFFFF5E
    /// <summary>Value serialization error</summary>
    Local_ValueSerialization = -161, // 0xFFFFFF5F
    /// <summary>Key deserialization error</summary>
    Local_KeyDeserialization = -160, // 0xFFFFFF60
    /// <summary>Value deserialization error</summary>
    Local_ValueDeserialization = -159, // 0xFFFFFF61
    /// <summary>Partial response</summary>
    Local_Partial = -158, // 0xFFFFFF62
    /// <summary>Modification attempted on read-only object</summary>
    Local_ReadOnly = -157, // 0xFFFFFF63
    /// <summary>No such entry / item not found</summary>
    Local_NoEnt = -156, // 0xFFFFFF64
    /// <summary>Read underflow</summary>
    Local_Underflow = -155, // 0xFFFFFF65
    /// <summary>Invalid type</summary>
    Local_InvalidType = -154, // 0xFFFFFF66
    /// <summary>Retry operation.</summary>
    Local_Retry = -153, // 0xFFFFFF67
    /// <summary>Purged in queue</summary>
    Local_PurgeQueue = -152, // 0xFFFFFF68
    /// <summary>Purged in flight</summary>
    Local_PurgeInflight = -151, // 0xFFFFFF69
    /// <summary>Fatal error: see rd_kafka_fatal_error()</summary>
    Local_Fatal = -150, // 0xFFFFFF6A
    /// <summary>Inconsistent state</summary>
    Local_Inconsistent = -149, // 0xFFFFFF6B
    /// <summary>
    ///     Gap-less ordering would not be guaranteed if proceeding
    /// </summary>
    Local_GaplessGuarantee = -148, // 0xFFFFFF6C
    /// <summary>Maximum poll interval exceeded</summary>
    Local_MaxPollExceeded = -147, // 0xFFFFFF6D
    /// <summary>Unknown broker</summary>
    Local_UnknownBroker = -146, // 0xFFFFFF6E
    /// <summary>Functionality not configured</summary>
    Local_NotConfigured = -145, // 0xFFFFFF6F
    /// <summary>Instance has been fenced</summary>
    Local_Fenced = -144, // 0xFFFFFF70
    /// <summary>Application generated exception.</summary>
    Local_Application = -143, // 0xFFFFFF71
    /// <summary>Unknown broker error</summary>
    Unknown = -1, // 0xFFFFFFFF
    /// <summary>Success</summary>
    NoError = 0,
    /// <summary>Offset out of range</summary>
    OffsetOutOfRange = 1,
    /// <summary>Invalid message</summary>
    InvalidMsg = 2,
    /// <summary>Unknown topic or partition</summary>
    UnknownTopicOrPart = 3,
    /// <summary>Invalid message size</summary>
    InvalidMsgSize = 4,
    /// <summary>Leader not available</summary>
    LeaderNotAvailable = 5,
    /// <summary>Not leader for partition</summary>
    NotLeaderForPartition = 6,
    /// <summary>Request timed out</summary>
    RequestTimedOut = 7,
    /// <summary>Broker not available</summary>
    BrokerNotAvailable = 8,
    /// <summary>Replica not available</summary>
    ReplicaNotAvailable = 9,
    /// <summary>Message size too large</summary>
    MsgSizeTooLarge = 10, // 0x0000000A
    /// <summary>StaleControllerEpochCode</summary>
    StaleCtrlEpoch = 11, // 0x0000000B
    /// <summary>Offset metadata string too large</summary>
    OffsetMetadataTooLarge = 12, // 0x0000000C
    /// <summary>Broker disconnected before response received</summary>
    NetworkException = 13, // 0x0000000D
    /// <summary>Group coordinator load in progress</summary>
    GroupLoadInProress = 14, // 0x0000000E
    /// <summary>Group coordinator not available</summary>
    GroupCoordinatorNotAvailable = 15, // 0x0000000F
    /// <summary>Not coordinator for group</summary>
    NotCoordinatorForGroup = 16, // 0x00000010
    /// <summary>Invalid topic</summary>
    TopicException = 17, // 0x00000011
    /// <summary>
    ///     Message batch larger than configured server segment size
    /// </summary>
    RecordListTooLarge = 18, // 0x00000012
    /// <summary>Not enough in-sync replicas</summary>
    NotEnoughReplicas = 19, // 0x00000013
    /// <summary>
    ///     Message(s) written to insufficient number of in-sync replicas
    /// </summary>
    NotEnoughReplicasAfterAppend = 20, // 0x00000014
    /// <summary>Invalid required acks value</summary>
    InvalidRequiredAcks = 21, // 0x00000015
    /// <summary>Specified group generation id is not valid</summary>
    IllegalGeneration = 22, // 0x00000016
    /// <summary>Inconsistent group protocol</summary>
    InconsistentGroupProtocol = 23, // 0x00000017
    /// <summary>Invalid group.id</summary>
    InvalidGroupId = 24, // 0x00000018
    /// <summary>Unknown member</summary>
    UnknownMemberId = 25, // 0x00000019
    /// <summary>Invalid session timeout</summary>
    InvalidSessionTimeout = 26, // 0x0000001A
    /// <summary>Group rebalance in progress</summary>
    RebalanceInProgress = 27, // 0x0000001B
    /// <summary>Commit offset data size is not valid</summary>
    InvalidCommitOffsetSize = 28, // 0x0000001C
    /// <summary>Topic authorization failed</summary>
    TopicAuthorizationFailed = 29, // 0x0000001D
    /// <summary>Group authorization failed</summary>
    GroupAuthorizationFailed = 30, // 0x0000001E
    /// <summary>Cluster authorization failed</summary>
    ClusterAuthorizationFailed = 31, // 0x0000001F
    /// <summary>Invalid timestamp</summary>
    InvalidTimestamp = 32, // 0x00000020
    /// <summary>Unsupported SASL mechanism</summary>
    UnsupportedSaslMechanism = 33, // 0x00000021
    /// <summary>Illegal SASL state</summary>
    IllegalSaslState = 34, // 0x00000022
    /// <summary>Unsupported version</summary>
    UnsupportedVersion = 35, // 0x00000023
    /// <summary>Topic already exists</summary>
    TopicAlreadyExists = 36, // 0x00000024
    /// <summary>Invalid number of partitions</summary>
    InvalidPartitions = 37, // 0x00000025
    /// <summary>Invalid replication factor</summary>
    InvalidReplicationFactor = 38, // 0x00000026
    /// <summary>Invalid replica assignment</summary>
    InvalidReplicaAssignment = 39, // 0x00000027
    /// <summary>Invalid config</summary>
    InvalidConfig = 40, // 0x00000028
    /// <summary>Not controller for cluster</summary>
    NotController = 41, // 0x00000029
    /// <summary>Invalid request</summary>
    InvalidRequest = 42, // 0x0000002A
    /// <summary>Message format on broker does not support request</summary>
    UnsupportedForMessageFormat = 43, // 0x0000002B
    /// <summary>Isolation policy violation</summary>
    PolicyViolation = 44, // 0x0000002C
    /// <summary>Broker received an out of order sequence number</summary>
    OutOfOrderSequenceNumber = 45, // 0x0000002D
    /// <summary>Broker received a duplicate sequence number</summary>
    DuplicateSequenceNumber = 46, // 0x0000002E
    /// <summary>Producer attempted an operation with an old epoch</summary>
    InvalidProducerEpoch = 47, // 0x0000002F
    /// <summary>
    ///     Producer attempted a transactional operation in an invalid state
    /// </summary>
    InvalidTxnState = 48, // 0x00000030
    /// <summary>
    ///     Producer attempted to use a producer id which is not currently assigned to its transactional id
    /// </summary>
    InvalidProducerIdMapping = 49, // 0x00000031
    /// <summary>
    ///     Transaction timeout is larger than the maximum value allowed by the broker's max.transaction.timeout.ms
    /// </summary>
    InvalidTransactionTimeout = 50, // 0x00000032
    /// <summary>
    ///     Producer attempted to update a transaction while another concurrent operation on the same transaction was ongoing
    /// </summary>
    ConcurrentTransactions = 51, // 0x00000033
    /// <summary>
    ///     Indicates that the transaction coordinator sending a WriteTxnMarker is no longer the current coordinator for a given producer
    /// </summary>
    TransactionCoordinatorFenced = 52, // 0x00000034
    /// <summary>Transactional Id authorization failed</summary>
    TransactionalIdAuthorizationFailed = 53, // 0x00000035
    /// <summary>Security features are disabled</summary>
    SecurityDisabled = 54, // 0x00000036
    /// <summary>Operation not attempted</summary>
    OperationNotAttempted = 55, // 0x00000037
    /// <summary>
    ///     Disk error when trying to access log file on the disk.
    /// </summary>
    KafkaStorageError = 56, // 0x00000038
    /// <summary>
    ///     The user-specified log directory is not found in the broker config.
    /// </summary>
    LogDirNotFound = 57, // 0x00000039
    /// <summary>SASL Authentication failed.</summary>
    SaslAuthenticationFailed = 58, // 0x0000003A
    /// <summary>Unknown Producer Id.</summary>
    UnknownProducerId = 59, // 0x0000003B
    /// <summary>Partition reassignment is in progress.</summary>
    ReassignmentInProgress = 60, // 0x0000003C
    /// <summary>Delegation Token feature is not enabled.</summary>
    DelegationTokenAuthDisabled = 61, // 0x0000003D
    /// <summary>Delegation Token is not found on server.</summary>
    DelegationTokenNotFound = 62, // 0x0000003E
    /// <summary>Specified Principal is not valid Owner/Renewer.</summary>
    DelegationTokenOwnerMismatch = 63, // 0x0000003F
    /// <summary>
    ///     Delegation Token requests are not allowed on this connection.
    /// </summary>
    DelegationTokenRequestNotAllowed = 64, // 0x00000040
    /// <summary>Delegation Token authorization failed.</summary>
    DelegationTokenAuthorizationFailed = 65, // 0x00000041
    /// <summary>Delegation Token is expired.</summary>
    DelegationTokenExpired = 66, // 0x00000042
    /// <summary>Supplied principalType is not supported.</summary>
    InvalidPrincipalType = 67, // 0x00000043
    /// <summary>The group is not empty.</summary>
    NonEmptyGroup = 68, // 0x00000044
    /// <summary>The group id does not exist.</summary>
    GroupIdNotFound = 69, // 0x00000045
    /// <summary>The fetch session ID was not found.</summary>
    FetchSessionIdNotFound = 70, // 0x00000046
    /// <summary>The fetch session epoch is invalid.</summary>
    InvalidFetchSessionEpoch = 71, // 0x00000047
    /// <summary>No matching listener.</summary>
    ListenerNotFound = 72, // 0x00000048
    /// <summary>Topic deletion is disabled.</summary>
    TopicDeletionDisabled = 73, // 0x00000049
    /// <summary>Unsupported compression type.</summary>
    UnsupportedCompressionType = 74, // 0x0000004A
  }
}
