// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IStringList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for StringList class.</summary>
  /// <exclude />
  [Guid("AD9D0687-7CF9-4938-A05B-557FE57F9997")]
  public interface IStringList
  {
    string this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(string value);

    bool Contains(string value);

    int IndexOf(string value);

    void Insert(int index, string value);

    void Remove(string value);

    string[] ToArray();

    IEnumerator GetEnumerator();
  }
}
