// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizCategoryUtil
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizCategoryUtil
  {
    private SessionObjects sessionObjects;
    private IContactManager contactMngr;
    private Hashtable categoryNameToIdMap;
    private Hashtable categoryIdToNameMap;

    public BizCategoryUtil(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.contactMngr = sessionObjects.ContactManager;
    }

    public Hashtable GetCategoryNameToIdTable()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (BizCategory bizCategory in this.contactMngr.GetBizCategories())
        insensitiveHashtable.Add((object) bizCategory.Name, (object) bizCategory.CategoryID);
      return insensitiveHashtable;
    }

    public Hashtable GetCategoryIdToNameTable()
    {
      Hashtable categoryIdToNameTable = new Hashtable();
      foreach (BizCategory bizCategory in this.contactMngr.GetBizCategories())
        categoryIdToNameTable.Add((object) bizCategory.CategoryID, (object) bizCategory.Name);
      return categoryIdToNameTable;
    }

    public int GetNoCategoryId() => this.CategoryNameToId("No Category");

    public int CategoryNameToId(string name)
    {
      if (this.categoryNameToIdMap == null)
        this.categoryNameToIdMap = this.GetCategoryNameToIdTable();
      return this.categoryNameToIdMap.Contains((object) name) ? (int) this.categoryNameToIdMap[(object) name] : -1;
    }

    public string CategoryIdToName(int id)
    {
      if (this.categoryIdToNameMap == null)
        this.categoryIdToNameMap = this.GetCategoryIdToNameTable();
      return this.categoryIdToNameMap.Contains((object) id) ? (string) this.categoryIdToNameMap[(object) id] : string.Empty;
    }
  }
}
