// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailNotificationController.EmailNotificationControllerClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace EllieMae.EMLite.eFolder.EmailNotificationController
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class EmailNotificationControllerClient : 
    ClientBase<IEmailNotificationController>,
    IEmailNotificationController
  {
    public EmailNotificationControllerClient()
    {
    }

    public EmailNotificationControllerClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public EmailNotificationControllerClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public EmailNotificationControllerClient(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public EmailNotificationControllerClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public EmailSaveResponse EmailSave(EmailSaveRequest request) => this.Channel.EmailSave(request);

    public EmailGetResponse EmailGet(EmailGetRequest request) => this.Channel.EmailGet(request);
  }
}
