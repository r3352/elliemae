// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Packages.PackageSerializationInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Serialization;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Packages
{
  public class PackageSerializationInfo : XmlSerializationInfo
  {
    private SessionObjects sessionObjects;
    private Persona[] personas;
    private RoleInfo[] roles;
    private List<EllieMae.EMLite.Workflow.Milestone> msList;
    private DocumentTrackingSetup docSetup;

    public PackageSerializationInfo(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
    }

    public PackageSerializationInfo(SessionObjects sessionObjects, string xmldata)
      : base(xmldata)
    {
      this.sessionObjects = sessionObjects;
    }

    public string PersonaIDToName(int personaId)
    {
      this.loadPersonas();
      foreach (Persona persona in this.personas)
      {
        if (persona.ID == personaId)
          return persona.Name;
      }
      return (string) null;
    }

    public int NameToPersonaID(string name) => this.NameToPersonaID(name, true);

    public int NameToPersonaID(string name, bool throwOnMissing)
    {
      this.loadPersonas();
      foreach (Persona persona in this.personas)
      {
        if (string.Compare(persona.Name, name, true) == 0)
          return persona.ID;
      }
      if (throwOnMissing)
        throw new PackageObjectNotFoundException(ObjectType.Persona, name);
      return -1;
    }

    public string RoleIDToName(int roleId)
    {
      this.loadRoles();
      foreach (RoleInfo role in this.roles)
      {
        if (role.RoleID == roleId)
          return role.Name;
      }
      if (roleId == RoleInfo.Others.RoleID)
        return RoleInfo.Others.Name;
      if (roleId == RoleInfo.FileStarter.RoleID)
        return RoleInfo.FileStarter.Name;
      if (roleId == RoleInfo.All.RoleID)
        return RoleInfo.All.Name;
      throw new PackageObjectNotFoundException(ObjectType.Role, roleId.ToString());
    }

    public int NameToRoleID(string name) => this.NameToRoleID(name, true);

    public int NameToRoleID(string name, bool throwOnMissing)
    {
      this.loadRoles();
      foreach (RoleInfo role in this.roles)
      {
        if (string.Compare(role.Name, name, true) == 0)
          return role.RoleID;
      }
      if (name == RoleInfo.Others.Name)
        return RoleInfo.Others.RoleID;
      if (name == RoleInfo.FileStarter.Name)
        return RoleInfo.FileStarter.RoleID;
      if (name == RoleInfo.All.Name)
        return RoleInfo.All.RoleID;
      if (throwOnMissing)
        throw new PackageObjectNotFoundException(ObjectType.Role, name);
      return RoleInfo.Null.RoleID;
    }

    public string MilestoneIDToName(string msid)
    {
      this.loadMilestones();
      foreach (EllieMae.EMLite.Workflow.Milestone ms in this.msList)
      {
        if (string.Compare(ms.MilestoneID, msid, true) == 0)
          return ms.Name;
      }
      throw new PackageObjectNotFoundException(ObjectType.Milestone, msid);
    }

    public string NameToMilestoneID(string name) => this.NameToMilestoneID(name, true);

    public string NameToMilestoneID(string name, bool throwOnMissing)
    {
      this.loadMilestones();
      foreach (EllieMae.EMLite.Workflow.Milestone ms in this.msList)
      {
        if (string.Compare(ms.Name, name, true) == 0)
          return ms.MilestoneID;
      }
      if (throwOnMissing)
        throw new PackageObjectNotFoundException(ObjectType.Milestone, name);
      return (string) null;
    }

    public string DocumentIDToName(string docId)
    {
      this.loadDocuments();
      foreach (DocumentTemplate documentTemplate in this.docSetup)
      {
        if (string.Compare(documentTemplate.Guid, docId, true) == 0)
          return documentTemplate.Name;
      }
      throw new PackageObjectNotFoundException(ObjectType.Document, docId);
    }

    public string NameToDocumentID(string name) => this.NameToDocumentID(name, true);

    public string NameToDocumentID(string name, bool throwOnMissing)
    {
      this.loadDocuments();
      foreach (DocumentTemplate documentTemplate in this.docSetup)
      {
        if (string.Compare(documentTemplate.Name, name, true) == 0)
          return documentTemplate.Guid;
      }
      if (throwOnMissing)
        throw new PackageObjectNotFoundException(ObjectType.Document, name);
      return (string) null;
    }

    private void loadPersonas()
    {
      if (this.personas != null)
        return;
      this.personas = this.sessionObjects.PersonaManager.GetAllPersonas();
    }

    private void loadRoles()
    {
      if (this.roles != null)
        return;
      this.roles = this.sessionObjects.BpmManager.GetAllRoleFunctions();
    }

    private void loadMilestones()
    {
      if (this.msList != null)
        return;
      this.msList = this.sessionObjects.StartupInfo.Milestones;
    }

    private void loadDocuments()
    {
      if (this.docSetup != null)
        return;
      this.docSetup = this.sessionObjects.ConfigurationManager.GetDocumentTrackingSetup();
    }
  }
}
