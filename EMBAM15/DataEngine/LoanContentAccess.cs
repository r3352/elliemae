// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanContentAccess
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Flags]
  public enum LoanContentAccess
  {
    None = 0,
    DocumentTracking = 1,
    ConversationLog = 2,
    Task = 4,
    ProfitManagement = 8,
    ConditionTracking = 16, // 0x00000010
    DocTrackingViewOnly = 32, // 0x00000020
    ConditionTrackingViewOnly = 64, // 0x00000040
    ConversationLogViewOnly = 128, // 0x00000080
    TaskViewOnly = 256, // 0x00000100
    ProfitMgmtViewOnly = 512, // 0x00000200
    LockRequest = 1024, // 0x00000400
    LockRequestViewOnly = 2048, // 0x00000800
    FormFields = 4096, // 0x00001000
    DisclosureTracking = 32768, // 0x00008000
    DisclosureTrackingViewOnly = 65536, // 0x00010000
    DocTrackingRequestRetrieveService = 131072, // 0x00020000
    DocTrackingRetrieveServiceCurrent = 262144, // 0x00040000
    DocTrackingRetrieveServiceNotCurrent = 524288, // 0x00080000
    DocTrackingRetrieveServiceUnassigned = 1048576, // 0x00100000
    DocTrackingRequestRetrieveBorrower = 2097152, // 0x00200000
    DocTrackingRetrieveBorrowerCurrent = 4194304, // 0x00400000
    DocTrackingRetrieveBorrowerNotCurrent = 8388608, // 0x00800000
    DocTrackingRetrieveBorrowerUnassigned = 16777216, // 0x01000000
    DocTrackingUnassignedFiles = 33554432, // 0x02000000
    DocTrackingUnprotectedDocs = 67108864, // 0x04000000
    DocTrackingProtectedDocs = 134217728, // 0x08000000
    DocTrackingCreateDocs = 268435456, // 0x10000000
    DocTrackingOrderDisclosures = 536870912, // 0x20000000
    DocTrackingPartial = 1073741824, // 0x40000000
    FullAccess = 16777215, // 0x00FFFFFF
  }
}
