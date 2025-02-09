// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.MessageQueueException
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Interface;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class MessageQueueException : BusinessException, IInsuppressible
  {
    public AsyncProcessMessage AsyncProcessMessage { get; set; }

    public MessageQueueException(string description)
      : base(description)
    {
    }

    public MessageQueueException(string description, Exception innerException)
      : base(description, innerException)
    {
    }
  }
}
