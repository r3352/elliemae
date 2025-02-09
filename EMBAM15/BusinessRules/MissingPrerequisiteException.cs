// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.MissingPrerequisiteException
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public class MissingPrerequisiteException : ValidationException
  {
    private string fieldId;

    public MissingPrerequisiteException(string fieldId)
      : base("The required field '" + fieldId + "' is not populated")
    {
      this.fieldId = fieldId;
    }

    protected MissingPrerequisiteException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.fieldId = info.GetString(nameof (fieldId));
    }

    public string FieldID => this.fieldId;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("fieldId", (object) this.fieldId);
    }
  }
}
