// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public static class DocEngineUtils
  {
    public static void ChangeEncompassDocEngine(IHtmlInput dataObj, string docEngine)
    {
      Plan.ClearPlanMetadata(dataObj, DocumentOrderType.Opening, false);
      if (docEngine == "New_Encompass_Docs_Solution")
      {
        Plan.ClearPlanMetadata(dataObj, DocumentOrderType.Closing);
        EncompassDocs.SetDocEngine(dataObj, "New_Encompass_Docs_Solution");
      }
      else
      {
        if (!EncompassDocs.IsUsingAnyEncompassDocsProvider(dataObj))
          return;
        Plan.ClearPlanMetadata(dataObj, DocumentOrderType.Closing);
        EncompassDocs.SetDocEngine(dataObj, "Old_ODI_Encompass_Docs");
        EncompassDocs.ClearDocProvider(dataObj);
      }
    }

    public static bool IsActiveStatus(DocEngineServiceStatus status)
    {
      return status == DocEngineServiceStatus.Active || status == DocEngineServiceStatus.Demo;
    }
  }
}
