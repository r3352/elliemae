// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.FieldDescriptor
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  [Serializable]
  public class FieldDescriptor
  {
    private string fieldName;
    private string fieldType;
    private MemberAccessLevel fieldAccess;

    public FieldDescriptor(FieldInfo info)
    {
      this.fieldName = info.Name;
      this.fieldType = info.FieldType.FullName;
      if (info.IsPublic)
        this.fieldAccess = MemberAccessLevel.Public;
      else if (info.IsPrivate)
        this.fieldAccess = MemberAccessLevel.Private;
      else if (info.IsFamilyAndAssembly)
        this.fieldAccess = MemberAccessLevel.ProtectedInternal;
      else if (info.IsFamily)
        this.fieldAccess = MemberAccessLevel.Protected;
      else
        this.fieldAccess = MemberAccessLevel.Internal;
    }

    public string Name => this.fieldName;

    public string Type => this.fieldType;

    public MemberAccessLevel AccessLevel => this.fieldAccess;
  }
}
