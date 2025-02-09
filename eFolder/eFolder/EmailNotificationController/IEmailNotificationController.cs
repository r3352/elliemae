// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailNotificationController.IEmailNotificationController
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

#nullable disable
namespace EllieMae.EMLite.eFolder.EmailNotificationController
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(Namespace = "http://www.elliemae.com/edm/platform", ConfigurationName = "EmailNotificationController.IEmailNotificationController")]
  public interface IEmailNotificationController
  {
    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IEmailNotificationController/EmailSave", ReplyAction = "http://www.elliemae.com/edm/platform/IEmailNotificationController/EmailSaveResponse")]
    EmailSaveResponse EmailSave(EmailSaveRequest request);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IEmailNotificationController/EmailGet", ReplyAction = "http://www.elliemae.com/edm/platform/IEmailNotificationController/EmailGetResponse")]
    EmailGetResponse EmailGet(EmailGetRequest request);
  }
}
