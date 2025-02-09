// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.RequestProcessingContext
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;

#nullable disable
namespace Elli.Service.Common
{
  public class RequestProcessingContext
  {
    public Request Request { get; private set; }

    public Response Response { get; private set; }

    public RequestProcessingContext(Request request) => this.Request = request;

    public void MarkAsProcessed(Response response)
    {
      this.Response = response != null ? response : throw new ArgumentNullException(nameof (response));
      this.IsProcessed = true;
    }

    public bool IsProcessed { get; private set; }
  }
}
