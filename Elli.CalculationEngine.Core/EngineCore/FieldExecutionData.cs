// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.FieldExecutionData
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class FieldExecutionData
  {
    public FieldDescriptor Descriptor;
    public short[] ExecutionPlan;
    public short[] InitializationPlan;
    public short[] ReferencePlan;
    public HashSet<short> DependentCalculations;
    public HashSet<short> ReferencedCalculations;
    public short[] WeakReferences;
    public short[] WeakDependencies;
    public short[] ParameterizedCalculations;
    public short BaseCalculation = -1;
    public bool HasCalculation;

    public static void Serialize(FieldExecutionData fep, BinaryWriter bw)
    {
      bw.Write(fep.Descriptor.ToString());
      bw.Write(fep.HasCalculation);
      bw.Write(fep.HasCalculation);
      if (fep.ExecutionPlan == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.ExecutionPlan.Length);
        foreach (short num in fep.ExecutionPlan)
          bw.Write(num);
      }
      if (fep.InitializationPlan == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.InitializationPlan.Length);
        foreach (short num in fep.InitializationPlan)
          bw.Write(num);
      }
      if (fep.ReferencePlan == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.ReferencePlan.Length);
        foreach (short num in fep.ReferencePlan)
          bw.Write(num);
      }
      if (fep.DependentCalculations == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.DependentCalculations.Count);
        foreach (short dependentCalculation in fep.DependentCalculations)
          bw.Write(dependentCalculation);
      }
      if (fep.ReferencedCalculations == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.ReferencedCalculations.Count);
        foreach (short referencedCalculation in fep.ReferencedCalculations)
          bw.Write(referencedCalculation);
      }
      if (fep.WeakReferences == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.WeakReferences.Length);
        foreach (short weakReference in fep.WeakReferences)
          bw.Write(weakReference);
      }
      if (fep.WeakDependencies == null)
      {
        bw.Write(0);
      }
      else
      {
        bw.Write(fep.WeakDependencies.Length);
        foreach (short weakDependency in fep.WeakDependencies)
          bw.Write(weakDependency);
      }
    }

    public static FieldExecutionData Deserialize(BinaryReader br)
    {
      FieldExecutionData fieldExecutionData = new FieldExecutionData();
      string descriptorString = br.ReadString();
      fieldExecutionData.Descriptor = FieldDescriptor.Create(descriptorString);
      fieldExecutionData.HasCalculation = br.ReadBoolean();
      fieldExecutionData.HasCalculation = br.ReadBoolean();
      int num1 = br.ReadInt32();
      List<short> shortList1 = new List<short>();
      for (int index = 1; index <= num1; ++index)
        shortList1.Add(br.ReadInt16());
      fieldExecutionData.ExecutionPlan = shortList1.ToArray();
      int num2 = br.ReadInt32();
      List<short> shortList2 = new List<short>();
      for (int index = 1; index <= num2; ++index)
        shortList2.Add(br.ReadInt16());
      fieldExecutionData.InitializationPlan = shortList2.ToArray();
      int num3 = br.ReadInt32();
      List<short> shortList3 = new List<short>();
      for (int index = 1; index <= num3; ++index)
        shortList3.Add(br.ReadInt16());
      fieldExecutionData.ReferencePlan = shortList3.ToArray();
      int num4 = br.ReadInt32();
      List<short> shortList4 = new List<short>();
      for (int index = 1; index <= num4; ++index)
        shortList4.Add(br.ReadInt16());
      fieldExecutionData.DependentCalculations = new HashSet<short>((IEnumerable<short>) shortList4.ToArray());
      int num5 = br.ReadInt32();
      List<short> shortList5 = new List<short>();
      for (int index = 1; index <= num5; ++index)
        shortList5.Add(br.ReadInt16());
      fieldExecutionData.ReferencedCalculations = new HashSet<short>((IEnumerable<short>) shortList5.ToArray());
      int num6 = br.ReadInt32();
      List<short> shortList6 = new List<short>();
      for (int index = 1; index <= num6; ++index)
        shortList6.Add(br.ReadInt16());
      fieldExecutionData.WeakReferences = shortList6.ToArray();
      int num7 = br.ReadInt32();
      List<short> shortList7 = new List<short>();
      for (int index = 1; index <= num7; ++index)
        shortList7.Add(br.ReadInt16());
      fieldExecutionData.WeakDependencies = shortList7.ToArray();
      return fieldExecutionData;
    }
  }
}
