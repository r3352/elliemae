// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.FieldNotInDBException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class FieldNotInDBException : ServerException
  {
    private string fieldID = "";

    public FieldNotInDBException(string fieldID, string message = "�")
      : base(message)
    {
      this.fieldID = fieldID;
    }

    protected FieldNotInDBException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.fieldID = info.GetString(nameof (fieldID));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("fieldID", (object) this.fieldID);
    }

    public string FieldID => this.fieldID;

    public bool IsMilestoneField => this.fieldID.ToLower().Contains(".ms.");

    public string MilestoneName
    {
      get
      {
        try
        {
          return ((IEnumerable<string>) this.fieldID.Split('.')).Last<string>();
        }
        catch
        {
          return "";
        }
      }
    }
  }
}
