// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ILoanAccessRules
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public interface ILoanAccessRules
  {
    UserInfo GetUserInfo();

    bool IsLogEntryAccessible(LogRecordBase logEntry);

    bool IsLogEntryDeletable(LogRecordBase logEntry);

    bool IsLogEntryEditable(LogRecordBase logEntry);

    bool IsLogEntryProtected(LogRecordBase logEntry);

    bool IsAllowedToClearCondition(ConditionLog logEntry);

    bool CanUpdateEnhancedConditionTrackingStatus(StatusTrackingDefinition trackDef);

    int[] GetDefaultRoleAccessForDocument(DocumentLog logEntry);

    bool IsAlertApplicableToUser(LogAlert alert);

    bool IsGFEEditable();

    bool IsUserInEffectiveRole(int roleId);

    void Refresh();
  }
}
