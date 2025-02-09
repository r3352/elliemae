// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Mapping.DocumentLogMap
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson.Serialization;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Elli.Data.MongoDB.Mapping
{
  public class DocumentLogMap
  {
    public static void Register()
    {
      if (BsonClassMap.IsClassMapRegistered(typeof (DocumentLog)))
        return;
      BsonClassMap.RegisterClassMap<DocumentLog>((Action<BsonClassMap<DocumentLog>>) (cm =>
      {
        cm.AutoMap();
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Requested));
        cm.MapProperty<DateTime?>((Expression<Func<DocumentLog, DateTime?>>) (c => c.DateRequested));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Rerequested));
        cm.MapProperty<DateTime?>((Expression<Func<DocumentLog, DateTime?>>) (c => c.DateRerequested));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Received));
        cm.MapProperty<DateTime?>((Expression<Func<DocumentLog, DateTime?>>) (c => c.DateReceived));
        cm.MapProperty<DateTime?>((Expression<Func<DocumentLog, DateTime?>>) (c => c.DateExpected));
        cm.MapProperty<DateTime?>((Expression<Func<DocumentLog, DateTime?>>) (c => c.DateExpires));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Expires));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsExpired));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.ShippingReady));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.UnderwritingReady));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Reviewed));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.Expected));
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsPastDue));
        cm.MapProperty<string>((Expression<Func<DocumentLog, string>>) (c => c.Status));
        cm.MapField("conditionRefs").SetElementName("Conditions");
        cm.MapField("fileAttachments").SetElementName("FileAttachments");
        cm.UnmapProperty<string>((Expression<Func<DocumentLog, string>>) (p => p.AllowedRolesXml));
        cm.GetMemberMap<string>((Expression<Func<DocumentLog, string>>) (p => p.AllowedRoleDelimitedList)).SetElementName("AllowedRoles");
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsWebCenterIndicator)).SetIgnoreIfDefault(false);
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsTPOWebcenterPortalIndicator)).SetIgnoreIfDefault(false);
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsThirdPartyDocIndicator)).SetIgnoreIfDefault(false);
        cm.MapProperty<bool>((Expression<Func<DocumentLog, bool>>) (c => c.IsExternalIndicator)).SetIgnoreIfDefault(false);
      }));
      EntityReferenceMap.Register();
      LogAlertMap.Register();
      LogCommentMap.Register();
      DocumentLogAttachmentsMap.Register();
    }
  }
}
