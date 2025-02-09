// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.TypeDescriptor
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  public class TypeDescriptor : MarshalByRefObject
  {
    private TypeIdentifier typeId;
    private Type type;

    public TypeDescriptor(TypeIdentifier typeId, Type type)
    {
      this.typeId = typeId;
      this.type = type;
    }

    public TypeIdentifier TypeID => this.typeId;

    public FieldDescriptor GetFieldDescriptor(string fieldName)
    {
      FieldInfo field = this.type.GetField(fieldName);
      return field == (FieldInfo) null ? (FieldDescriptor) null : new FieldDescriptor(field);
    }
  }
}
