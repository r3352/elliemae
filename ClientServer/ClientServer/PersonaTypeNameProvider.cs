// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PersonaTypeNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class PersonaTypeNameProvider : CustomEnumNameProvider
  {
    private static Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    static PersonaTypeNameProvider()
    {
      PersonaTypeNameProvider.nameMap.Add((object) PersonaType.Internal, (object) "Internal");
      PersonaTypeNameProvider.nameMap.Add((object) PersonaType.External, (object) "External");
      PersonaTypeNameProvider.nameMap.Add((object) PersonaType.BothInternalExternal, (object) "Both Internal and External");
    }

    public PersonaTypeNameProvider()
      : base(typeof (PersonaType), PersonaTypeNameProvider.nameMap)
    {
    }

    public PersonaType GetPersonaTypeFromDescription(string personaTypeDescription)
    {
      return PersonaTypeNameProvider.nameMap.Keys.OfType<PersonaType>().FirstOrDefault<PersonaType>((Func<PersonaType, bool>) (s => PersonaTypeNameProvider.nameMap[(object) s].Equals((object) personaTypeDescription)));
    }

    public static string GetDescriptionFromPersonaType(PersonaType personaType)
    {
      return PersonaTypeNameProvider.nameMap[(object) personaType].ToString();
    }

    public static PersonaType ResolvePersonaType(bool isInternal, bool isExternal)
    {
      if (isInternal && !isExternal)
        return PersonaType.Internal;
      return !isInternal & isExternal ? PersonaType.External : PersonaType.BothInternalExternal;
    }
  }
}
