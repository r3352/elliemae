// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PersistentField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public abstract class PersistentField : FieldDefinition
  {
    protected PersistentField(string fieldId)
      : this(fieldId, FieldInstanceSpecifierType.None)
    {
    }

    protected PersistentField(string fieldId, FieldInstanceSpecifierType instanceSpecifierType)
      : base(fieldId, instanceSpecifierType)
    {
    }

    protected PersistentField(FieldDefinition source, object instanceSpecifier)
      : base(source, instanceSpecifier)
    {
    }

    internal abstract string XPath { get; }
  }
}
