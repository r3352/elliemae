// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogHtmlEmailMessages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for HtmlEmailMessages class.</summary>
  /// <exclude />
  [Guid("67BE8741-3E33-49ce-9D5D-8AC0D041DA60")]
  public interface ILogHtmlEmailMessages
  {
    int Count { get; }

    HtmlEmailMessage this[int index] { get; }

    void Remove(HtmlEmailMessage message);

    IEnumerator GetEnumerator();
  }
}
