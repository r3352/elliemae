// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.FieldDefinitionObj
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class FieldDefinitionObj
  {
    private FieldDefinition field;
    private string display = "";
    private FieldInstanceSpecifierType type;
    private bool multiInstance;
    private string fieldID = "";

    public FieldDefinitionObj(FieldDefinition field) => this.field = field;

    public FieldDefinitionObj(
      string display,
      FieldInstanceSpecifierType type,
      bool multiInstance,
      string fieldID)
    {
      this.display = display;
      this.type = type;
      this.multiInstance = multiInstance;
      this.fieldID = fieldID;
    }

    public override string ToString() => this.field != null ? this.field.Description : this.display;

    public FieldInstanceSpecifierType Type
    {
      get => this.field != null ? this.field.InstanceSpecifierType : this.type;
    }

    public string GetInstanceFieldID(string instanceSpecifier)
    {
      return this.field.CreateInstance((object) instanceSpecifier).FieldID;
    }

    public bool MultiInstance => this.field != null ? this.field.MultiInstance : this.multiInstance;

    public string FieldID => this.field != null ? this.field.FieldID : this.fieldID;
  }
}
