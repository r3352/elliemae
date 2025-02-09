// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BpmCacheMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BpmCacheMessage : CacheControlMessage
  {
    public readonly BizRule.Condition Condition;
    public readonly int ConditionState;
    public readonly string MilestoneID;
    public readonly string ConditionState2;

    public BpmCacheMessage(
      ClientSessionCacheID cacheID,
      BizRule.Condition condition,
      int conditionState,
      string milestoneID,
      string conditionState2)
      : base(cacheID)
    {
      this.Condition = condition;
      this.ConditionState = conditionState;
      this.MilestoneID = milestoneID;
      this.ConditionState2 = conditionState2;
    }
  }
}
