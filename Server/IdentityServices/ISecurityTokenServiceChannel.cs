// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.ISecurityTokenServiceChannel
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface ISecurityTokenServiceChannel : 
    ISecurityTokenService,
    IClientChannel,
    IContextChannel,
    IChannel,
    ICommunicationObject,
    IExtensibleObject<IContextChannel>,
    IDisposable
  {
  }
}
