// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailNotificationController.IEmailNotificationControllerChannel
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace EllieMae.EMLite.eFolder.EmailNotificationController
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface IEmailNotificationControllerChannel : 
    IEmailNotificationController,
    IClientChannel,
    IContextChannel,
    IChannel,
    ICommunicationObject,
    IExtensibleObject<IContextChannel>,
    IDisposable
  {
  }
}
