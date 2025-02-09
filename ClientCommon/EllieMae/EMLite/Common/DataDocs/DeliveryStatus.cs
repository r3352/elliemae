// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.DeliveryStatus
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public enum DeliveryStatus
  {
    [Description("None")] None,
    [Description("In Progress")] InProgress,
    [Description("Completed")] Completed,
    [Description("Error")] Error,
    [Description("Cancelled")] Cancelled,
    [Description("Submitted")] Submitted,
    [Description("Accepted")] Accepted,
    [Description("Rejected")] Rejected,
    [Description("Delivered")] Delivered,
  }
}
