// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.SearchFilterMappings
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine
{
  public class SearchFilterMappings
  {
    private const string _usersTableAbbreviation = "u�";
    private const string _personaTableAbbreviation = "p�";
    private static StringComparer _comparer = StringComparer.OrdinalIgnoreCase;
    public static Dictionary<string, FilterDef> UserSearchFilterMapping;
    public static Dictionary<string, FilterDef> PersonaSearchFilterMapping;

    static SearchFilterMappings()
    {
      Dictionary<string, FilterDef> dictionary = new Dictionary<string, FilterDef>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      dictionary.Add("FirstName", (FilterDef) new StringDef("first_name", "u", FilterDataType.String));
      dictionary.Add("LastName", (FilterDef) new StringDef("last_name", "u", FilterDataType.String));
      dictionary.Add("MiddleName", (FilterDef) new StringDef("middle_name", "u", FilterDataType.String));
      dictionary.Add("Suffix", (FilterDef) new StringDef("suffix_name", "u", FilterDataType.String));
      dictionary.Add("FullName", (FilterDef) new StringDef("FirstLastName", "u", FilterDataType.String));
      dictionary.Add("UserId", (FilterDef) new StringDef("userid", "u", FilterDataType.String));
      dictionary.Add("EmailId", (FilterDef) new StringDef("email", "u", FilterDataType.String));
      EnumDef enumDef = new EnumDef("status", "u", FilterDataType.Number);
      enumDef.DataMappings = new Dictionary<string, string>((IEqualityComparer<string>) SearchFilterMappings._comparer)
      {
        {
          "true",
          "0"
        },
        {
          "false",
          "1"
        }
      };
      dictionary.Add("AccountEnabled", (FilterDef) enumDef);
      SearchFilterMappings.UserSearchFilterMapping = dictionary;
      SearchFilterMappings.PersonaSearchFilterMapping = new Dictionary<string, FilterDef>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
      {
        {
          "Name",
          (FilterDef) new StringDef("name", "p", FilterDataType.String)
        }
      };
    }
  }
}
