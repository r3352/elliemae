// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UpsertScAttributesCompletedEventArgs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [GeneratedCode("System.Web.Services", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  public class UpsertScAttributesCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal UpsertScAttributesCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public ScAttribute[] Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (ScAttribute[]) this.results[0];
      }
    }
  }
}
