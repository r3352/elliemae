// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.EncompassDocs
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public static class EncompassDocs
  {
    public const string DocProviderValue = "Encompass Docs";
    public const string ODIDocEngine = "Old_ODI_Encompass_Docs";
    public const string NDEDocEngine = "New_Encompass_Docs_Solution";

    public static void SetDocEngine(IHtmlInput dataObj, string docEngine)
    {
      if (docEngine == "New_Encompass_Docs_Solution")
        dataObj.SetField("2399", "Encompass Docs");
      dataObj.SetField("Docs.Engine", docEngine);
    }

    public static void ClearDocProvider(IHtmlInput dataObj) => dataObj.SetField("2399", "");

    public static void ClearDocEngine(IHtmlInput dataObj) => dataObj.SetField("Docs.Engine", "");

    public static bool IsUsingAnyEncompassDocsProvider(IHtmlInput dataObj)
    {
      return dataObj.GetField("2399") == "Encompass Docs";
    }

    public static bool IsUsingEncompassDocsSolution(IHtmlInput dataObj)
    {
      if (dataObj.GetField("2399") != "Encompass Docs")
        return false;
      string field = dataObj.GetField("Docs.Engine");
      return dataObj.GetField("PlanCode.ID") != "" || field == "New_Encompass_Docs_Solution";
    }
  }
}
