// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.FieldSearchField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  [Serializable]
  public class FieldSearchField
  {
    public const string FIELD_ID = "FieldId�";
    public const string RELATIONSHIP_TYPE = "RelationshipType�";
    public const string IS_SYSTEM_FIELD = "IsSystemField�";
    public const string DESCRIPTION = "Description�";
    public const string TYPE = "Type�";

    public FieldSearchField(string fieldId, RelationshipType relationshipType)
    {
      this.FieldId = fieldId;
      this.RelationshipType = relationshipType;
      this.IsSystemField = true;
    }

    public FieldSearchField(
      string fieldId,
      RelationshipType relationshipType,
      string description,
      string type)
    {
      this.FieldId = fieldId;
      this.RelationshipType = relationshipType;
      this.IsSystemField = description == null || type == null;
      if (this.IsSystemField)
        return;
      this.Description = description;
      this.Type = type;
    }

    public FieldSearchField(DataRow r)
    {
      this.FieldId = Convert.ToString(r[nameof (FieldId)]);
      this.RelationshipType = (RelationshipType) Convert.ToInt16(r[nameof (RelationshipType)]);
      this.IsSystemField = Convert.ToBoolean(r[nameof (IsSystemField)]);
      if (this.IsSystemField)
        return;
      this.Description = Convert.ToString(r[nameof (Description)]);
      this.Type = Convert.ToString(r[nameof (Type)]);
    }

    public string FieldId { get; set; }

    public RelationshipType RelationshipType { get; set; }

    [CLSCompliant(false)]
    public string Type { get; set; }

    [CLSCompliant(false)]
    public string Description { get; set; }

    public bool IsSystemField { get; set; }
  }
}
