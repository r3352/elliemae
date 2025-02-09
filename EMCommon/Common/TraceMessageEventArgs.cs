// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TraceMessageEventArgs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [CLSCompliant(true)]
  public class TraceMessageEventArgs : EventArgs
  {
    public readonly Encompass.Diagnostics.Logging.LogLevel Level;
    public readonly string Category;
    public readonly string Message;
    public bool Cancel;

    public TraceMessageEventArgs(Encompass.Diagnostics.Logging.LogLevel level, string cat, string msg)
    {
      this.Level = level;
      this.Category = cat;
      this.Message = msg;
    }
  }
}
