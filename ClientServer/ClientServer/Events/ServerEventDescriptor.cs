// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.ServerEventDescriptor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  public class ServerEventDescriptor
  {
    private static ServerEventDescriptor[] events = (ServerEventDescriptor[]) new ArrayList()
    {
      (object) new ServerEventDescriptor(typeof (ConnectionEvent), "Connection", "Event that occurs when a connection is established or broken with the server"),
      (object) new ServerEventDescriptor(typeof (ExceptionEvent), "Exception", "Event that occurs when an exception is raised on the server"),
      (object) new ServerEventDescriptor(typeof (LicenseEvent), "License", "Event raised when a licensed is granted or denied"),
      (object) new ServerEventDescriptor(typeof (LoanEvent), "Loan", "Event that signifies a change in state of a loan"),
      (object) new ServerEventDescriptor(typeof (SessionEvent), "Session", "Event that signifies a change in state of a session")
    }.ToArray(typeof (ServerEventDescriptor));
    private Type eventType;
    private string name;
    private string description;

    public static ServerEventDescriptor[] AllEvents => ServerEventDescriptor.events;

    private ServerEventDescriptor(Type eventType, string name, string description)
    {
      this.eventType = eventType;
      this.name = name;
      this.description = description;
    }

    public Type EventType => this.eventType;

    public string Name => this.name;

    public string Description => this.description;
  }
}
