// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExportServiceAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExportServiceAclInfo
  {
    private string exportGroup = "";
    private int featureID;
    private int personaID;
    private string userID;
    private AclResourceAccess personaAccess = AclResourceAccess.None;
    private AclResourceAccess customAccess = AclResourceAccess.None;

    public ExportServiceAclInfo(string exportGroup, int featureID)
    {
      this.exportGroup = exportGroup;
      this.featureID = featureID;
    }

    public string UserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public int PersonaID
    {
      get => this.personaID;
      set => this.personaID = value;
    }

    public string ExportGroup => this.exportGroup;

    public bool Access
    {
      get
      {
        return this.customAccess != AclResourceAccess.None ? this.customAccess != AclResourceAccess.ReadOnly : this.personaAccess == AclResourceAccess.None || this.personaAccess != AclResourceAccess.ReadOnly;
      }
    }

    public AclResourceAccess PersonaAccess
    {
      get => this.personaAccess;
      set => this.personaAccess = value;
    }

    public AclResourceAccess CustomAccess
    {
      get => this.customAccess;
      set => this.customAccess = value;
    }

    public static List<ExportServiceAclInfo> GetExportServicesList(
      string[] exportGroups,
      int featureID)
    {
      List<ExportServiceAclInfo> exportServicesList = new List<ExportServiceAclInfo>();
      foreach (string exportGroup in exportGroups)
        exportServicesList.Add(new ExportServiceAclInfo(exportGroup, featureID));
      return exportServicesList;
    }

    public enum ExportServicesDefaultSetting
    {
      None,
      Custom,
      All,
      NotSpecified,
    }
  }
}
