// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi.WebookEventContract
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi
{
  public class WebookEventContract
  {
    public WebookEventContract()
    {
    }

    public WebookEventContract(string userID, string eventyType, string resourceId)
    {
      this.UserId = userID;
      this.EventType = eventyType;
      this.ResourceId = resourceId;
    }

    public string UserId { get; set; }

    public string InstanceId
    {
      get
      {
        return Session.DefaultInstance.ServerIdentity == null ? "LOCALHOST" : Session.DefaultInstance.ServerIdentity.InstanceName;
      }
    }

    public string EventId => Guid.NewGuid().ToString();

    public DateTime Time => DateTime.UtcNow;

    public string Source => "urn:elli:encompass:" + this.InstanceId;

    public string Type
    {
      get
      {
        return string.Format("{0}{1}:{2}", (object) "urn:elli:encompass:", (object) "externalOrganization", (object) this.EventType);
      }
    }

    public string EventType { get; set; }

    public string ResourceId { get; set; }

    public string ResourceType => "externalOrganization";

    public string ResourceRef => "/platform/v1/events/externalorganization/" + this.ResourceId;

    public object payload { get; set; }

    public void AddExpoObject(string propertyName, object propertyValue)
    {
      // ISSUE: reference to a compiler-generated field
      if (WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (WebookEventContract), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (WebookEventContract), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__0.Target((CallSite) WebookEventContract.\u003C\u003Eo__32.\u003C\u003Ep__0, this.payload, (object) null);
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
