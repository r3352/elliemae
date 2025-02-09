// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IReportingFieldDescriptorList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Reporting;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for ReportingFieldDescriptorList class.</summary>
  /// <exclude />
  [Guid("F946E8C9-839E-41f8-AFAE-9B3610B4BBCA")]
  public interface IReportingFieldDescriptorList
  {
    ReportingFieldDescriptor this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ReportingFieldDescriptor value);

    bool Contains(ReportingFieldDescriptor value);

    int IndexOf(ReportingFieldDescriptor value);

    void Insert(int index, ReportingFieldDescriptor value);

    void Remove(ReportingFieldDescriptor value);

    ReportingFieldDescriptor[] ToArray();

    IEnumerator GetEnumerator();
  }
}
