// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.WebServices.GetDisclosureTrackingDetailsCompletedEventArgs
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ContactUI.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  public class GetDisclosureTrackingDetailsCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal GetDisclosureTrackingDetailsCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public string Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (string) this.results[0];
      }
    }
  }
}
