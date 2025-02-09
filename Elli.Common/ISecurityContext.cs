// Decompiled with JetBrains decompiler
// Type: Elli.Common.ISecurityContext
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common
{
  public interface ISecurityContext
  {
    string UserName { get; set; }

    DateTime? Created { get; set; }

    string SessionId { get; set; }

    string Realm { get; set; }

    string TokenData { get; set; }

    string TokenType { get; set; }

    string SiteId { get; set; }

    string CorrelationId { get; set; }

    bool IsVirtualUser { get; set; }

    string AuditUserId { get; set; }

    bool IsInternalSession { get; set; }

    bool SkipPersonaChecks { get; set; }
  }
}
