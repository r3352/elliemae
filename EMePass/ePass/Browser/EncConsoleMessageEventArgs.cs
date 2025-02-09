// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncConsoleMessageEventArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncConsoleMessageEventArgs : EventArgs
  {
    private int lineNumber;
    private MessageLevel level;
    private string message;
    private string source;

    public EncConsoleMessageEventArgs(int level, int lineNumber, string message, string source)
    {
      this.level = (MessageLevel) level;
      this.lineNumber = lineNumber;
      this.message = message;
      this.source = source;
    }

    public MessageLevel Level => this.level;

    public int LineNumber => this.lineNumber;

    public string Message => this.message;

    public string Source => this.source;
  }
}
