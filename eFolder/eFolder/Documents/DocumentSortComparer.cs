// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentSortComparer
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentSortComparer : IComparer<GVItem>
  {
    private StackingOrderComparer comparer;

    public DocumentSortComparer(LoanData loanData, StackingOrderSetTemplate template)
    {
      this.comparer = new StackingOrderComparer(loanData, template);
    }

    public int Compare(GVItem x, GVItem y)
    {
      return this.comparer.Compare((object) (x.Tag as DocumentLog), (object) (y.Tag as DocumentLog));
    }
  }
}
