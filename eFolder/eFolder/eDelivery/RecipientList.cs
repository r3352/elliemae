// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.RecipientList
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class RecipientList
  {
    public string id { get; set; }

    public string fullName { get; set; }

    public string email { get; set; }

    public int routingOrder { get; set; }

    public string role { get; set; }

    public DocumentCollection[] documents { get; set; }

    public Taskstatus1[] taskStatuses { get; set; }

    public object[] consents { get; set; }
  }
}
