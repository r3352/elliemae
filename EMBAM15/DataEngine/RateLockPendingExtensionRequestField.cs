// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockPendingExtensionRequestField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockPendingExtensionRequestField : BankerEditionVirtualField
  {
    public RateLockPendingExtensionRequestField()
      : base("LOCKRATE.EXTENSIONREQUESTPENDING", "Pending Rate Lock Extension Request", FieldFormat.YN)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      LockRequestLog confirmLockRequest = loan.GetLogList().GetLastNotConfirmLockRequest();
      return confirmLockRequest != null && confirmLockRequest.IsLockExtension ? "Y" : "N";
    }
  }
}
