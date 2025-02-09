// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.ConsoleMessageEventArgs
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System;

#nullable disable
namespace Elli.Web.Host
{
  public class ConsoleMessageEventArgs : EventArgs
  {
    private int lineNumber;
    private MessageLevel level;
    private string message;
    private string source;

    public ConsoleMessageEventArgs(int level, int lineNumber, string message, string source)
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
