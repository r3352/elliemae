// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.PersonaAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class PersonaAccessor
  {
    private PersonaAccessor()
    {
    }

    public static Hashtable GetPersonaLookup()
    {
      return PersonaAccessor.PersonasCacheEntry.Get().PersonasById;
    }

    public static Persona[] GetAllPersonas() => PersonaAccessor.PersonasCacheEntry.Get().Personas;

    public static Persona GetPersonaByName(string name)
    {
      return (Persona) PersonaAccessor.PersonasCacheEntry.Get().PersonasByName[(object) name];
    }

    public static Persona GetPersona(int id)
    {
      return (Persona) PersonaAccessor.GetPersonaLookup()[(object) id];
    }

    public static string GetPersonaName(int id)
    {
      Persona persona = PersonaAccessor.GetPersona(id);
      return persona == (Persona) null ? (string) null : persona.Name;
    }

    public static bool GetPersonaAclFeaturesDefault(int id)
    {
      if (id == Persona.SuperAdministrator.ID)
        return true;
      Persona persona = PersonaAccessor.GetPersona(id);
      return !(persona == (Persona) null) ? persona.AclFeaturesDefault : throw new Exception("Cannot find persona");
    }

    public static bool GetPersonaAclFeaturesDefault(int[] ids)
    {
      foreach (int id in ids)
      {
        if (PersonaAccessor.GetPersonaAclFeaturesDefault(id))
          return true;
      }
      return false;
    }

    public static Persona CreatePersona(
      string name,
      bool aclFeaturesDefault,
      bool isInternal,
      bool isExternal)
    {
      ClientContext current = ClientContext.GetCurrent();
      Persona newPersona = (Persona) null;
      current.Cache.Put<PersonaAccessor.PersonasCacheEntry>("PersonasCacheEntry", (Action) (() =>
      {
        name = (name ?? "").Trim();
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select personaID from Personas where personaName = " + SQL.Encode((object) name));
        DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
        if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
          throw new Exception(name + ": persona already exists");
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("insert into Personas (personaName, aclFeaturesDefault, isInternal, isExternal)");
        dbQueryBuilder.AppendLine(" values (" + SQL.Encode((object) name) + ", " + (object) (aclFeaturesDefault ? 1 : 0) + ", " + SQL.EncodeFlag(isInternal) + ", " + SQL.EncodeFlag(isExternal) + ")");
        dbQueryBuilder.AppendLine("select personaID, personaName, aclFeaturesDefault, isInternal, isExternal, displayOrder from Personas where personaName = " + SQL.Encode((object) name));
        DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
        newPersona = dataRowCollection2.Count != 0 ? PersonaAccessor.DataRowToPersona(dataRowCollection2[0]) : throw new Exception("Persona not created in database");
        if (!aclFeaturesDefault)
          return;
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("insert into Acl_FeeManagementAccess (personaID, overwrite700, overwrite800, overwrite900, overwrite1000, overwrite1100, overwrite1200, overwrite1300, overwritePC) values (" + (object) newPersona.ID + ",1,1,1,1,1,1,1,1)");
        dbQueryBuilder.ExecuteNonQuery();
      }), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
      return newPersona;
    }

    public static void DeletePersona(int personaID)
    {
      ClientContext current = ClientContext.GetCurrent();
      current.Cache.Put<PersonaAccessor.PersonasCacheEntry>("PersonasCacheEntry", (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("delete from Personas where personaID = " + (object) personaID);
        dbQueryBuilder.ExecuteNonQuery();
        WorkflowBpmDbAccessor.InvalidateRoleCache();
      }), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
      ThreadPool.QueueUserWorkItem(new WaitCallback(PersonaAccessor.flushUserStoreCache), (object) current);
    }

    private static void flushUserStoreCache(object contextObj)
    {
      using (((ClientContext) contextObj).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        UserStore.RefreshCache();
    }

    public static void RenamePersona(int personaID, string newName)
    {
      ClientContext.GetCurrent().Cache.Put<PersonaAccessor.PersonasCacheEntry>("PersonasCacheEntry", (Action) (() =>
      {
        newName = (newName ?? "").Trim();
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("update Personas set personaName = " + SQL.Encode((object) newName));
        dbQueryBuilder.AppendLine("\twhere personaID = " + (object) personaID);
        dbQueryBuilder.ExecuteNonQuery();
      }), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
    }

    public static void UpdatePersonaDisplayOrder(Persona[] personaList)
    {
      ClientContext.GetCurrent().Cache.Put<PersonaAccessor.PersonasCacheEntry>("PersonasCacheEntry", (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        foreach (Persona persona in personaList)
          dbQueryBuilder.AppendLine("update Personas set displayOrder = " + (object) persona.DisplayOrder + " where personaID = " + (object) persona.ID);
        dbQueryBuilder.ExecuteNonQuery();
      }), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
    }

    public static void UpdatePersonaType(int personaID, bool isInternal, bool isExternal)
    {
      ClientContext.GetCurrent().Cache.Put<PersonaAccessor.PersonasCacheEntry>("PersonasCacheEntry", (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("UPDATE Personas SET IsInternal = " + SQL.EncodeFlag(isInternal));
        dbQueryBuilder.AppendLine(", IsExternal = " + SQL.EncodeFlag(isExternal));
        dbQueryBuilder.AppendLine(" WHERE personaID = " + (object) personaID);
        dbQueryBuilder.ExecuteNonQuery();
      }), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
    }

    public static int GetAssociatedUsersCount(int personaID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT COUNT(*) FROM [UserPersona] UP ");
      dbQueryBuilder.AppendLine("INNER JOIN users U ON UP.userid = U.userid ");
      dbQueryBuilder.AppendLine("WHERE U.user_type = 'External' and UP.personaID = " + (object) personaID);
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar(DbTransactionType.None));
    }

    [PgReady]
    public static Persona[] GetPersonaListForUserFromDB(string userId)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("SELECT P.personaID, P.personaName, P.aclFeaturesDefault, P.isInternal, P.isExternal, P.displayOrder");
        pgDbQueryBuilder.AppendLine("FROM Personas P INNER JOIN UserPersona UP ON UP.personaID = P.personaID AND up.userid = @userid ORDER BY displayOrder");
        DbCommandParameter[] parameters = new DbCommandParameter[1]
        {
          new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.String)
        };
        return pgDbQueryBuilder.Execute(DbTransactionType.None, parameters).OfType<DataRow>().Select<DataRow, Persona>((System.Func<DataRow, Persona>) (dr => PersonaAccessor.DataRowToPersona(dr))).ToArray<Persona>();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine(string.Format("SELECT P.personaID, P.personaName, P.aclFeaturesDefault, P.isInternal, P.isExternal, P.displayOrder FROM Personas P INNER JOIN UserPersona UP ON UP.personaID = P.personaID AND up.userid = {0}  order by displayOrder", (object) SQL.Encode((object) userId)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      Persona[] listForUserFromDb = new Persona[dataRowCollection.Count];
      int num = 0;
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        listForUserFromDb[num++] = PersonaAccessor.DataRowToPersona(r);
      return listForUserFromDb;
    }

    [PgReady]
    public static Persona[] GetPersonaListFromDb()
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select personaID, personaName, aclFeaturesDefault, isInternal, isExternal, displayOrder from Personas order by displayOrder");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None);
        Persona[] personaListFromDb = new Persona[dataRowCollection.Count];
        int num = 0;
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          personaListFromDb[num++] = PersonaAccessor.DataRowToPersona(r);
        return personaListFromDb;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select personaID, personaName, aclFeaturesDefault, isInternal, isExternal, displayOrder from Personas order by displayOrder");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(DbTransactionType.None);
      Persona[] personaListFromDb1 = new Persona[dataRowCollection1.Count];
      int num1 = 0;
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection1)
        personaListFromDb1[num1++] = PersonaAccessor.DataRowToPersona(r);
      return personaListFromDb1;
    }

    public static Persona[] GetPersonasOfTypes(PersonaType[] personaTypes)
    {
      return ((IEnumerable<Persona>) PersonaAccessor.GetAllPersonas()).Where<Persona>((System.Func<Persona, bool>) (p => ((IEnumerable<PersonaType>) personaTypes).Any<PersonaType>((System.Func<PersonaType, bool>) (pt => PersonaAccessor.MatchesPersonaType(p, pt))))).ToArray<Persona>();
    }

    public static Persona[] GetExternalPersonas(int[] ids)
    {
      if (ids == null || ids.Length == 0)
        return new Persona[0];
      return ((IEnumerable<Persona>) PersonaAccessor.GetPersonasOfTypes(new PersonaType[2]
      {
        PersonaType.External,
        PersonaType.BothInternalExternal
      })).Where<Persona>((System.Func<Persona, bool>) (p => ((IEnumerable<int>) ids).Contains<int>(p.ID))).ToArray<Persona>();
    }

    private static bool MatchesPersonaType(Persona p, PersonaType pt)
    {
      switch (pt)
      {
        case PersonaType.Internal:
          return p.IsInternal && !p.IsExternal;
        case PersonaType.External:
          return !p.IsInternal && p.IsExternal;
        case PersonaType.BothInternalExternal:
          return p.IsInternal && p.IsExternal;
        default:
          return false;
      }
    }

    public static Persona[] GetAllCustomPersonas()
    {
      return ((IEnumerable<Persona>) PersonaAccessor.GetAllPersonas()).Where<Persona>((System.Func<Persona, bool>) (p => p.ID > 1)).ToArray<Persona>();
    }

    private static Persona DataRowToPersona(DataRow r)
    {
      return new Persona((int) r["personaID"], (string) r["personaName"], Convert.ToByte(r["aclFeaturesDefault"]) == (byte) 1, (int) r["displayOrder"], SQL.DecodeBoolean(r["isInternal"]), SQL.DecodeBoolean(r["isExternal"]));
    }

    [Serializable]
    private class PersonasCacheEntry
    {
      public const string CacheName = "PersonasCacheEntry�";

      public Persona[] Personas { get; set; }

      public Hashtable PersonasById { get; set; }

      public Hashtable PersonasByName { get; set; }

      public static PersonaAccessor.PersonasCacheEntry Get()
      {
        return ClientContext.GetCurrent().Cache.Get<PersonaAccessor.PersonasCacheEntry>(nameof (PersonasCacheEntry), new Func<PersonaAccessor.PersonasCacheEntry>(PersonaAccessor.PersonasCacheEntry.GetNew), CacheSetting.Low);
      }

      public static PersonaAccessor.PersonasCacheEntry GetNew()
      {
        Persona[] personaListFromDb = PersonaAccessor.GetPersonaListFromDb();
        if (personaListFromDb == null)
          return (PersonaAccessor.PersonasCacheEntry) null;
        Hashtable hashtable = new Hashtable();
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (Persona persona in personaListFromDb)
        {
          hashtable[(object) persona.ID] = (object) persona;
          insensitiveHashtable[(object) persona.Name] = (object) persona;
        }
        return new PersonaAccessor.PersonasCacheEntry()
        {
          Personas = personaListFromDb,
          PersonasById = hashtable,
          PersonasByName = insensitiveHashtable
        };
      }
    }
  }
}
