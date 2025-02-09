// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.ObjectNotFoundException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class ObjectNotFoundException : ServerException
  {
    private const uint innerHResult = 2147754243;
    private ObjectType type;
    private object objectId;

    public ObjectNotFoundException(string message, ObjectType type, object objectId)
      : base(message)
    {
      this.type = type;
      this.objectId = objectId;
      this.HResult = this.HRESULT(2147754243U);
    }

    public ObjectNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.type = (ObjectType) info.GetValue(nameof (type), typeof (ObjectType));
      this.objectId = info.GetValue(nameof (objectId), typeof (object));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("type", (object) this.type);
      info.AddValue("objectId", this.objectId);
    }

    public ObjectType ObjectType => this.type;

    public object ObjectID => this.objectId;
  }
}
