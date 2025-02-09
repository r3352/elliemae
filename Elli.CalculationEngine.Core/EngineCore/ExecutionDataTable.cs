// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.ExecutionDataTable
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class ExecutionDataTable
  {
    private readonly FieldExecutionData[] lookupTable;
    private readonly List<FieldDescriptor> masterPlan;
    public short[] CalculatedFields;

    public Dictionary<FieldDescriptor, short> FieldIndexDictionary { get; set; }

    public ExecutionDataTable(
      Dictionary<FieldDescriptor, short> fieldIndexDictionary,
      FieldExecutionData[] lookupTable)
    {
      this.FieldIndexDictionary = fieldIndexDictionary;
      this.lookupTable = lookupTable;
      List<short> shortList = new List<short>();
      for (short index = 0; (int) index < lookupTable.Length; ++index)
      {
        if (lookupTable[(int) index].HasCalculation)
          shortList.Add(index);
      }
      this.CalculatedFields = shortList.ToArray();
      this.masterPlan = this.FieldIndexDictionary.OrderBy<KeyValuePair<FieldDescriptor, short>, short>((Func<KeyValuePair<FieldDescriptor, short>, short>) (pair => pair.Value)).Select<KeyValuePair<FieldDescriptor, short>, FieldDescriptor>((Func<KeyValuePair<FieldDescriptor, short>, FieldDescriptor>) (pair => pair.Key)).ToList<FieldDescriptor>();
    }

    public FieldExecutionData GetFieldExecutionData(short index)
    {
      return index >= (short) 0 ? this.lookupTable[(int) index] : (FieldExecutionData) null;
    }

    public List<FieldDescriptor> GetExecutionPlan(FieldDescriptor fieldDescriptor)
    {
      List<FieldDescriptor> executionPlan = new List<FieldDescriptor>();
      List<short> shortList = new List<short>();
      foreach (short num in !(fieldDescriptor == (FieldDescriptor) null) ? ((IEnumerable<short>) this.GetFieldExecutionData(fieldDescriptor).ExecutionPlan).ToList<short>() : this.FieldIndexDictionary.Values.ToList<short>())
      {
        short planItem = num;
        KeyValuePair<FieldDescriptor, short> keyValuePair = this.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) planItem));
        executionPlan.Add(keyValuePair.Key);
      }
      return executionPlan;
    }

    public FieldExecutionData GetFieldExecutionData(FieldDescriptor descriptor)
    {
      short descriptorIndex = this.GetDescriptorIndex(descriptor);
      return descriptorIndex >= (short) 0 ? this.lookupTable[(int) descriptorIndex] : (FieldExecutionData) null;
    }

    public List<FieldDescriptor> GetExecutionPlan(
      FieldDescriptor fieldDescriptor,
      PlanType planType)
    {
      List<FieldDescriptor> executionPlan = new List<FieldDescriptor>();
      List<short> shortList = new List<short>();
      if (fieldDescriptor == (FieldDescriptor) null)
      {
        shortList = this.FieldIndexDictionary.Values.ToList<short>();
      }
      else
      {
        FieldExecutionData fieldExecutionData = this.GetFieldExecutionData(fieldDescriptor);
        if (fieldExecutionData != null)
        {
          switch (planType)
          {
            case PlanType.SingleFieldExecutionPlan:
            case PlanType.MergedExecutionPlan:
              shortList = ((IEnumerable<short>) fieldExecutionData.ExecutionPlan).ToList<short>();
              break;
            case PlanType.SingleFieldReferencePlan:
              shortList = ((IEnumerable<short>) fieldExecutionData.ReferencePlan).ToList<short>();
              break;
            case PlanType.SingleFieldInitializationPlan:
            case PlanType.MergedInitializationPlan:
            case PlanType.FullInitializationPlan:
              shortList = ((IEnumerable<short>) fieldExecutionData.InitializationPlan).ToList<short>();
              break;
            case PlanType.DirectlyReferencedCalculations:
              shortList = fieldExecutionData.ReferencedCalculations.ToList<short>();
              break;
            case PlanType.DirectlyDependentCalculations:
              shortList = fieldExecutionData.DependentCalculations.ToList<short>();
              break;
          }
        }
      }
      foreach (short num in shortList)
      {
        short planItem = num;
        KeyValuePair<FieldDescriptor, short> keyValuePair = this.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) planItem));
        executionPlan.Add(keyValuePair.Key);
      }
      return executionPlan;
    }

    public FieldExecutionData[] GetAllExecutionPlans() => this.lookupTable;

    public List<FieldDescriptor> GetMasterPlan() => this.masterPlan;

    public short GetDescriptorIndex(FieldDescriptor descriptor)
    {
      short descriptorIndex = -1;
      if (!this.FieldIndexDictionary.TryGetValue(descriptor, out descriptorIndex) && !this.FieldIndexDictionary.TryGetValue(descriptor.GetBaseDescriptor(), out descriptorIndex))
        descriptorIndex = (short) -1;
      return descriptorIndex;
    }

    public List<short> GetAllMemberFieldIndexes(EntityDescriptor parent)
    {
      List<short> memberFieldIndexes = new List<short>();
      foreach (KeyValuePair<FieldDescriptor, short> fieldIndex in this.FieldIndexDictionary)
      {
        FieldDescriptor key = fieldIndex.Key;
        if (key.ParentEntityType.IsA(parent) && !EngineUtility.IsTransientField(key.FieldId))
          memberFieldIndexes.Add(fieldIndex.Value);
      }
      return memberFieldIndexes;
    }

    public static void Serialize(ExecutionDataTable ept, string filename)
    {
      using (BinaryWriter bw = new BinaryWriter((Stream) File.OpenWrite(filename)))
      {
        bw.Write(ept.FieldIndexDictionary.Count<KeyValuePair<FieldDescriptor, short>>());
        foreach (KeyValuePair<FieldDescriptor, short> fieldIndex in ept.FieldIndexDictionary)
        {
          bw.Write(fieldIndex.Key.ToString());
          bw.Write(fieldIndex.Value);
        }
        bw.Write(((IEnumerable<FieldExecutionData>) ept.lookupTable).Count<FieldExecutionData>());
        foreach (FieldExecutionData fep in ept.lookupTable)
          FieldExecutionData.Serialize(fep, bw);
        if (ept.CalculatedFields == null)
        {
          bw.Write(0);
        }
        else
        {
          bw.Write(ept.CalculatedFields.Length);
          foreach (short calculatedField in ept.CalculatedFields)
            bw.Write(calculatedField);
        }
        if (ept.CalculatedFields == null)
        {
          bw.Write(0);
        }
        else
        {
          bw.Write(ept.CalculatedFields.Length);
          foreach (short calculatedField in ept.CalculatedFields)
            bw.Write(calculatedField);
        }
        bw.Flush();
        bw.Close();
      }
    }

    public static ExecutionDataTable Deserialize(Stream stream)
    {
      Dictionary<FieldDescriptor, short> fieldIndexDictionary = new Dictionary<FieldDescriptor, short>();
      List<FieldExecutionData> fieldExecutionDataList = new List<FieldExecutionData>();
      List<short> shortList = new List<short>();
      using (BinaryReader br = new BinaryReader(stream))
      {
        int num1 = br.ReadInt32();
        for (int index = 1; index <= num1; ++index)
        {
          FieldDescriptor key = FieldDescriptor.Create(br.ReadString());
          short num2 = br.ReadInt16();
          fieldIndexDictionary.Add(key, num2);
        }
        int num3 = br.ReadInt32();
        for (int index = 1; index <= num3; ++index)
        {
          FieldExecutionData fieldExecutionData = FieldExecutionData.Deserialize(br);
          fieldExecutionDataList.Add(fieldExecutionData);
        }
        int num4 = br.ReadInt32();
        for (int index = 1; index <= num4; ++index)
          shortList.Add(br.ReadInt16());
      }
      return new ExecutionDataTable(fieldIndexDictionary, fieldExecutionDataList.ToArray())
      {
        CalculatedFields = shortList.ToArray()
      };
    }
  }
}
