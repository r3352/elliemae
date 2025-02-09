// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PipelineData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Flags]
  public enum PipelineData
  {
    Fields = 0,
    Borrowers = 1,
    Alerts = 2,
    Lock = 4,
    AssignedRights = 8,
    AccessRights = 16, // 0x00000010
    Milestones = 32, // 0x00000020
    LoanAssociates = 64, // 0x00000040
    Trade = 256, // 0x00000100
    All = 1023, // 0x000003FF
    AlertSummary = 1024, // 0x00000400
    CurrentUserAccessRightsOnly = 2048, // 0x00000800
  }
}
