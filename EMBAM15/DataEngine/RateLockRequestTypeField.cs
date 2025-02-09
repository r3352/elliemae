// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockRequestTypeField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockRequestTypeField : BankerEditionVirtualField
  {
    public const string Extended = "Extended";
    public const string Lock = "Lock";
    public const string Cancellation = "Cancellation";

    public RateLockRequestTypeField()
      : base("LOCKRATE.REQUESTTYPE", "Request Type", FieldFormat.STRING)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      LockRequestLog currentLockOrRequest = loan.GetLogList().GetCurrentLockOrRequest();
      if (currentLockOrRequest != null && currentLockOrRequest.IsLockExtension)
        return "Extended";
      return currentLockOrRequest != null && currentLockOrRequest.IsLockCancellation ? "Cancellation" : "Lock";
    }
  }
}
