// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IPersonaManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IPersonaManager
  {
    Persona GetPersonaByName(string personaName);

    Persona GetPersonaByID(int personaID);

    bool PersonaNameExists(string personaName);

    Persona[] GetAllPersonas();

    Persona[] GetAllPersonas(PersonaType[] personaType);

    Persona[] GetAllCustomPersonas();

    Persona CreatePersona(string name, bool aclFeaturesDefault, bool isInternal, bool isExternal);

    void DeletePersona(int personaID);

    void DeletePersona(Persona persona);

    void RenamePersona(int personaID, string newName);

    void RenamePersona(Persona persona, string newName);

    void UpdatePersonaOrder(Persona[] personaList);

    void UpdatePersonaType(int personaID, bool isInternal, bool isExternal);

    int GetAssociatedUsersCount(int personaID);
  }
}
