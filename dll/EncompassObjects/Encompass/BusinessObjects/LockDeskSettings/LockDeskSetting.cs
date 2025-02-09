// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.LockDeskSettings.LockDeskSetting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.LockDeskSettings
{
  public class LockDeskSetting
  {
    private string isEncompassLockDeskHoursEnabled;
    private string isLockDeskShutdown;
    private string lockDeskHoursMessage;
    private string lockDeskShutdownMessage;
    private string lockDeskStartTime;
    private string lockDeskEndTime;
    private IDictionary property;

    public LockDeskSetting(IDictionary property) => this.property = property;

    public string IsEncompassLockDeskHoursEnabled
    {
      get
      {
        this.isEncompassLockDeskHoursEnabled = this.property[(object) "POLICIES.EnableLockDeskSCHEDULE"].ToString();
        return this.isEncompassLockDeskHoursEnabled;
      }
    }

    public string IsLockDeskShutdown
    {
      get
      {
        this.isLockDeskShutdown = this.property[(object) "POLICIES.ShutDownLockSubmit"].ToString();
        return this.isLockDeskShutdown;
      }
    }

    public string LockDeskHoursMessage
    {
      get
      {
        this.lockDeskHoursMessage = this.property[(object) "POLICIES.LOCKDESKHRMSG"].ToString();
        return this.lockDeskHoursMessage;
      }
    }

    public string LockDeskShutdownMessage
    {
      get
      {
        this.lockDeskShutdownMessage = this.property[(object) "POLICIES.LOCKDESKSHUTDOWNMSG"].ToString();
        return this.lockDeskShutdownMessage;
      }
    }

    public string LockDeskStartTime
    {
      get
      {
        this.lockDeskStartTime = this.property[(object) "POLICIES.LOCKDESKSTRTIME"].ToString();
        return this.lockDeskStartTime;
      }
    }

    public string LockDeskEndTime
    {
      get
      {
        this.lockDeskEndTime = this.property[(object) "POLICIES.LOCKDESKENDTIME"].ToString();
        return this.lockDeskEndTime;
      }
    }
  }
}
