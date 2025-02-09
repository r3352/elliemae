// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFolders
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanFolders : SessionBoundObject, ILoanFolders, IEnumerable
  {
    private ILoanManager mngr;
    private Hashtable folders;

    internal LoanFolders(Session session)
      : base(session)
    {
      this.mngr = (ILoanManager) session.GetObject("LoanManager");
      this.Refresh();
    }

    public int Count => this.folders.Count;

    public LoanFolder this[string name] => (LoanFolder) this.folders[(object) (name ?? "")];

    public LoanFolder Add(string name)
    {
      if (this.folders.ContainsKey((object) (name ?? "")))
        throw new InvalidOperationException("A folder with this name already exists");
      this.mngr.CreateLoanFolder(name ?? "");
      LoanFolder loanFolder = new LoanFolder(this.Session, name);
      this.folders.Add((object) name, (object) loanFolder);
      return loanFolder;
    }

    public void Remove(LoanFolder folder)
    {
      this.mngr.DeleteLoanFolder(folder.Name, false);
      this.folders.Remove((object) folder.Name);
    }

    public void RebuildAll()
    {
      this.mngr.RebuildPipeline((IServerProgressFeedback) null, (DatabaseToRebuild) 2);
    }

    public void Refresh()
    {
      this.folders = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      string[] allLoanFolderNames = this.mngr.GetAllLoanFolderNames(false);
      for (int index = 0; index < allLoanFolderNames.Length; ++index)
        this.folders.Add((object) allLoanFolderNames[index], (object) new LoanFolder(this.Session, allLoanFolderNames[index]));
    }

    public IEnumerator GetEnumerator() => this.folders.Values.GetEnumerator();
  }
}
