// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Packages.PackageObjectNotFoundException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Exceptions;

#nullable disable
namespace EllieMae.EMLite.Packages
{
  public class PackageObjectNotFoundException : PackageSerializationException
  {
    private ObjectType objType;
    private string objId;

    public PackageObjectNotFoundException(ObjectType objType, string objId)
    {
      this.objType = objType;
      this.objId = objId;
    }

    public ObjectType ObjectType => this.objType;

    public string ObjectID => this.objId;
  }
}
