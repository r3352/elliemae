// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.StandardPlan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class StandardPlan : Plan
  {
    private const string className = "StandardPlan�";
    private static readonly string sw = Tracing.SwDataEngine;
    private DocEngineDataEntity dataEntity;

    internal StandardPlan(DocEngineDataEntity dataEntity)
      : base(dataEntity.GetAttribute("PlanCode"), dataEntity.GetAttribute("PlanDescription"))
    {
      this.dataEntity = dataEntity;
    }

    public override PlanType PlanType => PlanType.Standard;

    public override FieldDefinition[] GetFieldDefinitions()
    {
      List<FieldDefinition> fieldDefinitionList = new List<FieldDefinition>();
      foreach (DocEngineFieldDescriptor engineFieldDescriptor in this.dataEntity.FieldMetadata)
      {
        FieldDefinition field = (FieldDefinition) StandardFields.GetField(engineFieldDescriptor.EncompassFieldID);
        if (field != null)
          fieldDefinitionList.Add(field);
      }
      return fieldDefinitionList.ToArray();
    }

    public override bool ContainsField(string encFieldId)
    {
      return this.dataEntity.ContainsField(encFieldId);
    }

    public override string GetField(string encFieldId)
    {
      return encFieldId == "PlanCode.PlanType" ? "EM" : this.dataEntity.GetField(encFieldId);
    }

    internal static StandardPlan[] ExtractCollection(DocEngineResponse response)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 84, nameof (ExtractCollection), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\StandardPlan.cs");
        DocEngineMetadata fieldMetadata = DocEngineMetadata.Extract(response);
        if (fieldMetadata == null)
          throw new Exception("DocEngineResponse does not contain metadata element");
        PerformanceMeter.Current.AddCheckpoint("Estract MetaData", 89, nameof (ExtractCollection), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\StandardPlan.cs");
        List<StandardPlan> standardPlanList = new List<StandardPlan>();
        foreach (XmlElement selectNode in response.ResponseXml.SelectNodes("//Plan"))
          standardPlanList.Add(new StandardPlan(new DocEngineDataEntity(selectNode, fieldMetadata)));
        PerformanceMeter.Current.AddCheckpoint("Extract Plans", 98, nameof (ExtractCollection), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\StandardPlan.cs");
        return standardPlanList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 104, nameof (ExtractCollection), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\StandardPlan.cs");
      }
    }

    internal static Plan Extract(DocEngineResponse response)
    {
      DocEngineMetadata fieldMetadata = DocEngineMetadata.Extract(response);
      if (fieldMetadata == null)
        throw new Exception("DocEngineResponse does not contain metadata element");
      XmlElement xml = (XmlElement) response.ResponseXml.SelectSingleNode("//Plan");
      return xml == null ? (Plan) null : (Plan) new StandardPlan(new DocEngineDataEntity(xml, fieldMetadata));
    }
  }
}
