// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanFolderAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanFolderAclInfo
  {
    private int featureID;
    private int personaID;
    private string folderName;
    private int moveFromAccess;
    private bool customMoveFrom;
    private bool customMoveTo;
    private int moveToAccess;

    public LoanFolderAclInfo(
      int featureID,
      int personaID,
      string folderName,
      int moveFromAccess,
      int moveToAccess)
    {
      this.featureID = featureID;
      this.personaID = personaID;
      this.folderName = folderName;
      this.moveFromAccess = moveFromAccess;
      this.moveToAccess = moveToAccess;
    }

    public int FeatureID
    {
      get => this.featureID;
      set => this.featureID = value;
    }

    public int PersonaID
    {
      get => this.personaID;
      set => this.personaID = value;
    }

    public string FolderName
    {
      get => this.folderName;
      set => this.folderName = value;
    }

    public int MoveFromAccess
    {
      get => this.moveFromAccess;
      set => this.moveFromAccess = value;
    }

    public int MoveToAccess
    {
      get => this.moveToAccess;
      set => this.moveToAccess = value;
    }

    public bool CustomMoveFrom
    {
      get => this.customMoveFrom;
      set => this.customMoveFrom = value;
    }

    public bool CustomMoveTo
    {
      get => this.customMoveTo;
      set => this.customMoveTo = value;
    }
  }
}
