// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Persona
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class Persona : IWhiteListRemoteMethodParam
  {
    public const string LoanOfficerPersonaName = "Loan Officer";
    public const string LoanProcessorPersonaName = "Loan Processor";
    public static readonly Persona SuperAdministrator = new Persona(0, "Super Administrator", true, -1);
    public static readonly Persona Administrator = new Persona(1, nameof (Administrator), true, -1);
    private int personaID;
    private string personaName;
    private bool aclFeaturesDefault;
    private int displayOrder;
    private bool isInternal;
    private bool isExternal;

    public Persona(
      int personaID,
      string personaName,
      bool aclFeaturesDefault,
      int displayOrder,
      bool isInternal = false,
      bool isExternal = false)
    {
      this.personaID = personaID;
      this.personaName = personaName;
      this.aclFeaturesDefault = aclFeaturesDefault;
      this.displayOrder = displayOrder;
      this.isInternal = isInternal;
      this.isExternal = isExternal;
    }

    public int ID => this.personaID;

    public string Name => this.personaName;

    public bool AclFeaturesDefault => this.aclFeaturesDefault;

    public int DisplayOrder
    {
      get => this.displayOrder;
      set => this.displayOrder = value;
    }

    public bool IsInternal => this.isInternal;

    public bool IsExternal => this.isExternal;

    public override string ToString() => this.personaName;

    public override int GetHashCode() => this.personaID;

    public override bool Equals(object obj)
    {
      Persona persona = obj as Persona;
      return (object) persona != null && this.personaID == persona.ID;
    }

    public static bool operator ==(Persona persona1, Persona persona2)
    {
      return object.Equals((object) persona1, (object) persona2);
    }

    public static bool operator !=(Persona persona1, Persona persona2) => !(persona1 == persona2);

    public static string ToString(Persona[] personas, string seperator)
    {
      string[] strArray = new string[personas.Length];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = personas[index].Name;
      return string.Join(seperator, strArray);
    }

    public static string ToString(Persona[] personas) => Persona.ToString(personas, " + ");

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "name",
          (JToken) this.personaName
        }
      });
    }
  }
}
