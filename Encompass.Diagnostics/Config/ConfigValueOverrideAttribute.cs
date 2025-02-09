// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.ConfigValueOverrideAttribute
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class ConfigValueOverrideAttribute : HandlerAttribute
  {
    private readonly ICallHandler _callHandler;

    public ConfigValueOverrideAttribute(Type handlerType, params object[] initializationParameters)
    {
      if (handlerType == (Type) null)
        throw new ArgumentNullException(nameof (handlerType));
      this._callHandler = typeof (ConfigOverrideHandler).IsAssignableFrom(handlerType) ? Activator.CreateInstance(handlerType, initializationParameters) as ICallHandler : throw new ArgumentException("handlerType should extend " + typeof (ConfigOverrideHandler).Name, nameof (handlerType));
    }

    public override ICallHandler CreateHandler(IUnityContainer container) => this._callHandler;
  }
}
