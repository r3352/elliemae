// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.LockDeskSettings.LockDeskSetting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.LockDeskSettings
{
  /// <summary>Represents lock desk setting.</summary>
  public class LockDeskSetting
  {
    private string isEncompassLockDeskHoursEnabled;
    private string isLockDeskShutdown;
    private string lockDeskHoursMessage;
    private string lockDeskShutdownMessage;
    private string lockDeskStartTime;
    private string lockDeskEndTime;
    private IDictionary property;

    /// <summary>Constructor to initalize class variable</summary>
    /// <param name="property"></param>
    public LockDeskSetting(IDictionary property) => this.property = property;

    /// <summary>Gets Is Encompass Lock Desk Hours Enabled Setting</summary>
    public string IsEncompassLockDeskHoursEnabled
    {
      get
      {
        this.isEncompassLockDeskHoursEnabled = this.property[(object) "POLICIES.EnableLockDeskSCHEDULE"].ToString();
        return this.isEncompassLockDeskHoursEnabled;
      }
    }

    /// <summary>Gets Is Lock Desk Shut Down Setting</summary>
    public string IsLockDeskShutdown
    {
      get
      {
        this.isLockDeskShutdown = this.property[(object) "POLICIES.ShutDownLockSubmit"].ToString();
        return this.isLockDeskShutdown;
      }
    }

    /// <summary>Gets Lock Desk Hours Message</summary>
    public string LockDeskHoursMessage
    {
      get
      {
        this.lockDeskHoursMessage = this.property[(object) "POLICIES.LOCKDESKHRMSG"].ToString();
        return this.lockDeskHoursMessage;
      }
    }

    /// <summary>Gets Lock Desk Shut Down Setting</summary>
    public string LockDeskShutdownMessage
    {
      get
      {
        this.lockDeskShutdownMessage = this.property[(object) "POLICIES.LOCKDESKSHUTDOWNMSG"].ToString();
        return this.lockDeskShutdownMessage;
      }
    }

    /// <summary>Gets Lock Desk Start Time</summary>
    public string LockDeskStartTime
    {
      get
      {
        this.lockDeskStartTime = this.property[(object) "POLICIES.LOCKDESKSTRTIME"].ToString();
        return this.lockDeskStartTime;
      }
    }

    /// <summary>Gets Lock Desk End Time</summary>
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
