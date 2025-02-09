// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IPageImageAnnotation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for PageImageAnnotation class.</summary>
  /// <exclude />
  [Guid("ff5aeb99-a283-4faa-b98c-7fe878cf693a")]
  public interface IPageImageAnnotation
  {
    DateTime Date { get; }

    string AddedBy { get; }

    string Text { get; }

    int Left { get; }

    int Top { get; }

    int Width { get; }

    int Height { get; }
  }
}
