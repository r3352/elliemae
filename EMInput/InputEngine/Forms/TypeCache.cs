// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.TypeCache
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Compiler;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class TypeCache
  {
    private static Hashtable cache = new Hashtable();

    public static void Put(FormDescriptor des, TypeIdentifier typeId)
    {
      TypeCache.cache[(object) des] = (object) typeId;
    }

    public static TypeIdentifier Get(FormDescriptor des)
    {
      return (TypeIdentifier) TypeCache.cache[(object) des];
    }
  }
}
