// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.FormDimension
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  internal class FormDimension
  {
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
  }
}
