// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.Model.SignatureField
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.Model
{
  public class SignatureField
  {
    public string Id { get; set; }

    public string Name { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public SignatureFieldType? Type { get; set; }

    public int? Page { get; set; }

    public int? Left { get; set; }

    public int? Right { get; set; }

    public int? Top { get; set; }

    public int? Bottom { get; set; }

    public Decimal? Scale { get; set; }

    public SignatureFieldItemType? ItemType { get; set; }

    public List<SignatureFieldItem> ItemFields { get; set; }

    public bool? Optional { get; set; }

    public ConditionalField ConditionalField { get; set; }

    public ValidationField Validation { get; set; }
  }
}
