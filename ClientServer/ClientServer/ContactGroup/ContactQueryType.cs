// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactGroup.ContactQueryType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ContactGroup
{
  [Flags]
  public enum ContactQueryType
  {
    CampaignPredefinedQuery = 1,
    CampaignAdvancedQuery = 2,
    CampaignAddQuery = 4,
    CampaignDeleteQuery = 8,
    CampaignSavedQuery = 16, // 0x00000010
  }
}
