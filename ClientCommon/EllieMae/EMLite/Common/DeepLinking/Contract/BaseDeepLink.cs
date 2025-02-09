// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.Contract.BaseDeepLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.DeepLinking.Activity.Contract;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking.Contract
{
  public abstract class BaseDeepLink : IDeepLink
  {
    protected DeepLinkType _deepLinkType;
    protected IPreDeepLinkActivity _preDeepLinkActivity;
    protected IDeepLinkContext _deepLinkApplicationContext;
    protected string _url;
    protected string _source;

    public DeepLinkType DeepLinkType => this._deepLinkType;

    public IPreDeepLinkActivity PreDeepLinkActivity => this._preDeepLinkActivity;

    public IDeepLinkContext DeepLinkApplicationContext => this._deepLinkApplicationContext;

    public abstract string URL { get; }

    public virtual string KPIName
    {
      get
      {
        return string.Format("EncompassDesktop.DeepLink.{0}.{1}", (object) this._deepLinkApplicationContext.Source, (object) this._deepLinkType.ToString());
      }
    }

    public abstract string KPIDescription { get; }

    public BaseDeepLink(
      DeepLinkType deepLinkType,
      IDeepLinkContext deepLinkContext,
      IPreDeepLinkActivity preDeepLinkActivity = null)
    {
      this._deepLinkType = deepLinkType;
      this._preDeepLinkActivity = preDeepLinkActivity;
      this._deepLinkApplicationContext = deepLinkContext;
    }

    public virtual string GetLog()
    {
      string log = this.KPIName + " - " + this.KPIDescription + " - DeepLink app called from " + this._deepLinkApplicationContext.Source + " Application Name: " + this._deepLinkType.ToString() + " UserID: " + Session.UserID;
      string additionalLog = this._deepLinkApplicationContext.AdditionalLog;
      if (!string.IsNullOrWhiteSpace(additionalLog))
        log = log + " Additional Info:- " + additionalLog;
      return log;
    }
  }
}
