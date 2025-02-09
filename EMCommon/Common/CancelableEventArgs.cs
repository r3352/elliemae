// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CancelableEventArgs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CancelableEventArgs : EventArgs
  {
    private bool cancel;
    private string message = "";

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }

    public string CancelMessage
    {
      get => this.message;
      set => this.message = value;
    }
  }
}
