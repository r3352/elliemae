// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.RolesMappingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  [Serializable]
  public class RolesMappingInfo
  {
    private int[] roleIDList;
    private RealWorldRoleID realWorldRoleID;

    public RolesMappingInfo(int[] roleIDList, RealWorldRoleID realWorldRoleID)
    {
      this.roleIDList = roleIDList;
      this.realWorldRoleID = realWorldRoleID;
    }

    public RolesMappingInfo(RealWorldRoleID realWorldRoleID)
      : this(new int[0], realWorldRoleID)
    {
    }

    public int[] RoleIDList
    {
      get => this.roleIDList;
      set => this.roleIDList = value;
    }

    public RealWorldRoleID RealWorldRoleID => this.realWorldRoleID;

    public void AddRoleID(int roleID)
    {
      int[] numArray = new int[this.roleIDList.Length + 1];
      if (this.roleIDList.Length != 0)
        this.roleIDList.CopyTo((Array) numArray, 0);
      numArray[this.roleIDList.Length] = roleID;
      this.roleIDList = numArray;
    }
  }
}
