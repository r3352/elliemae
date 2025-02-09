// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FullRightsAccessRules
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FullRightsAccessRules : ILoanAccessRules
  {
    public virtual UserInfo GetUserInfo() => (UserInfo) null;

    public virtual bool IsLogEntryAccessible(LogRecordBase logEntry) => true;

    public virtual bool IsLogEntryDeletable(LogRecordBase logEntry) => true;

    public virtual bool IsLogEntryEditable(LogRecordBase logEntry) => true;

    public virtual bool IsLogEntryProtected(LogRecordBase logEntry) => true;

    public virtual bool IsAllowedToClearCondition(ConditionLog logEntry) => true;

    public virtual bool CanUpdateEnhancedConditionTrackingStatus(StatusTrackingDefinition trackDef)
    {
      return true;
    }

    public virtual int[] GetDefaultRoleAccessForDocument(DocumentLog logEntry)
    {
      throw new NotSupportedException("This method cannot be invoked under the current set of rules");
    }

    public virtual bool IsAlertApplicableToUser(LogAlert logAlert) => true;

    public virtual bool IsGFEEditable() => true;

    public virtual bool IsUserInEffectiveRole(int roleId) => false;

    public virtual void Refresh()
    {
    }
  }
}
