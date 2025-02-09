// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ServerMessageEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Defines the arguments passed into the MessageArrived event on the <see cref="T:EllieMae.Encompass.Client.Session">Session</see>.
  /// </summary>
  public class ServerMessageEventArgs : EventArgs, IServerMessageEventArgs
  {
    private Message msg;

    internal ServerMessageEventArgs(Message msg) => this.msg = msg;

    /// <summary>Gets the User ID of the user who sent the message.</summary>
    public string Source => this.msg.Source == null ? "" : this.msg.Source.UserID;

    /// <summary>Gets the text of the message.</summary>
    public string Text => this.msg.Text;

    /// <summary>
    /// Provides a string representation of the source and text of the message.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return this.Source == "" ? this.msg.Text : "[Message from " + this.msg.Source.UserID + "] " + this.msg.Text;
    }
  }
}
