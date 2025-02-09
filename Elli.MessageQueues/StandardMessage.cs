// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.StandardMessage
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;

#nullable disable
namespace Elli.MessageQueues
{
  [Serializable]
  public class StandardMessage : IMessage
  {
    public const string DefaultApplicationId = "";
    public const string DefaultServiceId = "";
    public const string NullInstanceId = null;
    public const string NullSiteId = null;

    public string Source { get; private set; }

    public string Type { get; private set; }

    public string ApplicationId { get; private set; }

    public string ServiceId { get; private set; }

    public string InstanceId { get; private set; }

    public string SiteId { get; private set; }

    public string EventId { get; private set; }

    public string UserId { get; private set; }

    public DateTime CreateAt { get; set; }

    public string CorrelationId { get; set; }

    public string AuditUserId { get; private set; }

    public StandardMessage(
      string applicationId,
      string serviceId,
      string instanceId,
      string siteId,
      string eventId,
      string userId,
      string auditUserId = null)
    {
      this.ApplicationId = applicationId;
      this.ServiceId = serviceId;
      this.InstanceId = instanceId;
      this.SiteId = siteId;
      this.EventId = eventId;
      this.UserId = userId;
      this.CreateAt = DateTime.UtcNow;
      this.CorrelationId = Guid.NewGuid().ToString();
      this.AuditUserId = auditUserId;
      this.Source = StandardMessage.ComposeSourceUrn(applicationId, serviceId, instanceId, siteId);
      this.Type = StandardMessage.ComposeTypeUrn(applicationId, serviceId, eventId);
    }

    public StandardMessage(
      string applicationId,
      string serviceId,
      string instanceId,
      string eventId,
      string userId,
      string auditUserId = null)
      : this(applicationId, serviceId, instanceId, (string) null, eventId, userId, auditUserId)
    {
    }

    public static string ComposeSourceUrn(
      string applicationId,
      string serviceId,
      string instanceId,
      string siteId)
    {
      return string.Format("urn:{0}:{1}:{2}:{3}", (object) applicationId, (object) serviceId, (object) instanceId, (object) siteId);
    }

    public static string ComposeTypeUrn(string applicationId, string serviceId, string eventId)
    {
      return string.Format("urn:{0}:{1}:{2}", (object) applicationId, (object) serviceId, (object) eventId);
    }

    public string TryGetSourceUrnPart(StandardMessage.SourceUrnPart part, string defaultValue = "")
    {
      switch (part)
      {
        case StandardMessage.SourceUrnPart.ApplicationId:
          return this.ApplicationId;
        case StandardMessage.SourceUrnPart.ServiceId:
          return this.ServiceId;
        case StandardMessage.SourceUrnPart.InstanceId:
          return this.InstanceId;
        case StandardMessage.SourceUrnPart.SiteId:
          return this.SiteId;
        default:
          return defaultValue;
      }
    }

    public string TryGetTypeUrnPart(StandardMessage.TypeUrnPart part, string defaultValue = "")
    {
      switch (part)
      {
        case StandardMessage.TypeUrnPart.ApplicationId:
          return this.ApplicationId;
        case StandardMessage.TypeUrnPart.ServiceId:
          return this.ServiceId;
        case StandardMessage.TypeUrnPart.EventId:
          return this.EventId;
        default:
          return defaultValue;
      }
    }

    public enum SourceUrnPart
    {
      ApplicationId,
      ServiceId,
      InstanceId,
      SiteId,
    }

    public enum TypeUrnPart
    {
      ApplicationId,
      ServiceId,
      EventId,
    }
  }
}
