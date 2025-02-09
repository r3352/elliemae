// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WebhookEventContract
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class WebhookEventContract
  {
    public string UserId { get; set; }

    public string InstanceId { get; set; }

    public string EventId => Guid.NewGuid().ToString();

    public DateTime Time => DateTime.UtcNow;

    public string Source { get; set; }

    public string ResourceType { get; set; }

    public string Type
    {
      get
      {
        return string.Format("{0}:{1}:{2}", (object) this.Source, (object) this.ResourceType, (object) this.EventType);
      }
    }

    public string EventType { get; set; }

    public string ResourceId { get; set; }

    public string ResourceRef { get; set; }

    public object payload { get; set; }

    public void AddExpoObject(string propertyName, object propertyValue)
    {
      // ISSUE: reference to a compiler-generated field
      if (WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebhookEventContract), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (WebhookEventContract), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) WebhookEventContract.\u003C\u003Eo__38.\u003C\u003Ep__0, this.payload, (object) null);
      if (target((CallSite) p1, obj))
        this.payload = (object) new ExpandoObject();
      IDictionary<string, object> payload = this.payload as IDictionary<string, object>;
      if (payload.ContainsKey(propertyName))
        payload[propertyName] = propertyValue;
      else
        payload.Add(propertyName, propertyValue);
    }
  }
}
