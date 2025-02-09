// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.SpecialFeatureCodeDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  [Serializable]
  public class SpecialFeatureCodeDefinition
  {
    public SpecialFeatureCodeDefinition(
      string code,
      string source,
      string description = "�",
      string comment = "�")
    {
      this.ID = Guid.NewGuid().ToString();
      this.Code = code;
      this.Source = source;
      this.Description = description;
      this.Comment = comment;
      this.Status = SpecialFeatureCodeDefinitionStatus.None;
    }

    public SpecialFeatureCodeDefinition Clone()
    {
      return new SpecialFeatureCodeDefinition(this.Code, this.Source, this.Description, this.Comment);
    }

    public string ID { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string Comment { get; set; }

    public string Source { get; set; }

    public SpecialFeatureCodeDefinitionStatus Status { get; set; }

    public bool IsActive => this.Status.HasFlag((Enum) SpecialFeatureCodeDefinitionStatus.Active);
  }
}
