// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldValidationException
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldValidationException : Exception, ISerializable
  {
    private string fieldId;
    private string invalidValue;

    public FieldValidationException(string fieldId, string invalidValue, string description)
      : base(description)
    {
      this.fieldId = fieldId;
      this.invalidValue = invalidValue;
    }

    private FieldValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.fieldId = info.GetString(nameof (fieldId));
      this.invalidValue = info.GetString(nameof (invalidValue));
    }

    public string FieldID => this.fieldId;

    public string InvalidValue => this.invalidValue;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("fieldId", (object) this.fieldId);
      info.AddValue("invalidValue", (object) this.invalidValue);
    }
  }
}
