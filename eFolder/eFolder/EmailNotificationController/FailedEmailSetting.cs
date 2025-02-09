// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailNotificationController.FailedEmailSetting
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.EmailNotificationController
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "FailedEmailSetting", Namespace = "http://schemas.datacontract.org/2004/07/Elli.EmailNotification.Service")]
  [Serializable]
  public class FailedEmailSetting : EmailSetting
  {
    [OptionalField]
    private string ReasonField;

    [DataMember]
    public string Reason
    {
      get => this.ReasonField;
      set
      {
        if ((object) this.ReasonField == (object) value)
          return;
        this.ReasonField = value;
        this.RaisePropertyChanged(nameof (Reason));
      }
    }
  }
}
