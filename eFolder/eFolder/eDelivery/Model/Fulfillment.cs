// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Model.Fulfillment
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.Model
{
  public class Fulfillment
  {
    public string scheduledDate { get; set; }

    public User from { get; set; }

    public User to { get; set; }
  }
}
