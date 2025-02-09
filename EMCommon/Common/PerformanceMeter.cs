// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.PerformanceMeter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class PerformanceMeter : IDisposable
  {
    private const string className = "PerformanceMeter";
    public static PerformanceMeter Null = new PerformanceMeter();
    private static PerformanceMeterMode meterMode = PerformanceMeterMode.Disabled;
    private static Dictionary<string, StreamWriter> perfWriters = new Dictionary<string, StreamWriter>();
    [ThreadStatic]
    private static PerformanceMeter currentMeter = (PerformanceMeter) null;
    [ThreadStatic]
    public static string FilePath = (string) null;
    private DateTime startTime = DateTime.MinValue;
    private DateTime stopTime = DateTime.MaxValue;
    private TimeSpan customizationTime = TimeSpan.Zero;
    private DateTime customizationStartTime = DateTime.MinValue;
    private string name;
    private string description;
    private bool publish;
    private bool aborted;
    private bool abortOnDispose;
    private bool combinedPublication;
    private bool sslPublication;
    private ArrayList checkpoints = new ArrayList();
    private ArrayList notes = new ArrayList();
    private Hashtable operations = new Hashtable();
    private Hashtable variables = new Hashtable();
    private bool stopped;
    private PerformanceMeter childMeter;
    private PerformanceMeter parentMeter;

    public static event PerformanceMeterEventHandler MeterStarted;

    public static event PerformanceMeterEventHandler MeterStopped;

    static PerformanceMeter()
    {
      string str = EnConfigurationSettings.GetApplicationArgumentValue("-perf");
      if (string.IsNullOrEmpty(str))
        str = string.Concat(EnConfigurationSettings.GlobalSettings["Performance"]).ToLower();
      switch (str)
      {
        case "1":
        case "overwrite":
          PerformanceMeter.meterMode = PerformanceMeterMode.Overwrite;
          break;
        case "2":
        case "append":
          PerformanceMeter.meterMode = PerformanceMeterMode.Append;
          break;
        case "3":
        case "preserve":
          PerformanceMeter.meterMode = PerformanceMeterMode.Preserve;
          break;
      }
      EnConfigurationSettings.GlobalSettings.SettingsChanged += new EventHandler(PerformanceMeter.GlobalSettings_SettingsChanged);
    }

    private static void GlobalSettings_SettingsChanged(object sender, EventArgs e)
    {
      lock (typeof (PerformanceMeter))
      {
        string lower = string.Concat(EnConfigurationSettings.GlobalSettings["Performance"]).ToLower();
        if (lower == "0" || lower == "disabled")
          PerformanceMeter.meterMode = PerformanceMeterMode.Disabled;
        if (lower == "1" || lower == "overwrite")
          PerformanceMeter.meterMode = PerformanceMeterMode.Overwrite;
        else if (lower == "2" || lower == "append")
          PerformanceMeter.meterMode = PerformanceMeterMode.Append;
        else if (lower == "3" || lower == "preserve")
          PerformanceMeter.meterMode = PerformanceMeterMode.Preserve;
        if (PerformanceMeter.Enabled)
          return;
        foreach (StreamWriter streamWriter in PerformanceMeter.perfWriters.Values)
        {
          lock (streamWriter)
            streamWriter.Dispose();
        }
        PerformanceMeter.perfWriters.Clear();
      }
    }

    private PerformanceMeter()
    {
      this.name = (string) null;
      this.description = (string) null;
    }

    public bool IsNullMeter => this.name == null;

    public PerformanceMeter(
      string name,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
      : this(name, "", creatorLineNumber, creatorMemberName, creatorFilePath)
    {
    }

    public PerformanceMeter(
      string name,
      string description,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
      : this(name, description, false, creatorLineNumber, creatorMemberName, creatorFilePath)
    {
    }

    public PerformanceMeter(
      string name,
      bool publish,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
      : this(name, "", publish, creatorLineNumber, creatorMemberName, creatorFilePath)
    {
    }

    public PerformanceMeter(
      string name,
      string description,
      bool publish,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
      : this(name, description, publish, false, false, creatorLineNumber, creatorMemberName, creatorFilePath)
    {
    }

    public PerformanceMeter(
      string name,
      string description,
      bool publish,
      bool abortOnDispose,
      bool combinedPublication,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      this.name = !((name ?? "") == "") ? name : throw new ArgumentNullException(nameof (name));
      this.description = description;
      this.publish = publish;
      this.abortOnDispose = abortOnDispose;
      this.combinedPublication = combinedPublication;
      if (PerformanceMeter.FilePath == null)
        PerformanceMeter.FilePath = Path.Combine(EnConfigurationSettings.GlobalSettings.AppLogDirectory, Environment.UserName);
      lock (typeof (PerformanceMeter))
      {
        if (PerformanceMeter.Enabled)
        {
          if (PerformanceMeter.meterMode != PerformanceMeterMode.NoLog)
          {
            if (!PerformanceMeter.perfWriters.ContainsKey(PerformanceMeter.FilePath))
              PerformanceMeter.openMeterFile();
          }
        }
      }
      this.AddVariable("CreatorMemberName", (object) creatorMemberName);
      this.AddVariable("CreatorFilePath", (object) creatorFilePath);
      this.AddVariable("CreatorLineNumber", (object) creatorLineNumber.ToString());
    }

    public bool IsDocsMeter
    {
      get
      {
        bool isDocsMeter = false;
        if (!string.IsNullOrWhiteSpace(this.Description))
          isDocsMeter = this.Description.ToUpper().StartsWith("DOCS");
        return isDocsMeter;
      }
    }

    public bool Publish
    {
      get => this.publish;
      set => this.publish = value;
    }

    public bool SslPublication
    {
      get => this.sslPublication;
      set => this.sslPublication = value;
    }

    public bool CombinedPublication
    {
      get => this.combinedPublication;
      set => this.combinedPublication = value;
    }

    public string Name => this.name;

    public string Description => this.description;

    public bool Active
    {
      get
      {
        return !this.aborted && this.startTime != DateTime.MinValue && this.stopTime == DateTime.MaxValue;
      }
    }

    public bool Aborted => this.aborted;

    public TimeSpan Duration
    {
      get => this.Active ? DateTime.Now - this.startTime : this.stopTime - this.startTime;
    }

    public Hashtable Variables => this.variables;

    public void Start()
    {
      if (!PerformanceMeter.Enabled || this.name == null)
        return;
      this.startTime = DateTime.Now;
      if (PerformanceMeter.currentMeter != null)
      {
        PerformanceMeter.currentMeter.AddCheckpoint("#SUBMETER START: " + this.name, 278, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\Common\\PerformanceMeter.cs");
        this.parentMeter = PerformanceMeter.currentMeter;
        PerformanceMeter.currentMeter.childMeter = this;
      }
      PerformanceMeter.currentMeter = this;
      try
      {
        if (PerformanceMeter.MeterStarted == null)
          return;
        PerformanceMeter.MeterStarted(this);
      }
      catch
      {
      }
    }

    public void StartCustomization(string description)
    {
      this.AddCheckpoint(description, 292, nameof (StartCustomization), "D:\\ws\\24.3.0.0\\EmLite\\Common\\PerformanceMeter.cs");
      this.customizationStartTime = DateTime.Now;
    }

    public void StopCustomization(string description)
    {
      this.AddCheckpoint(description, 298, nameof (StopCustomization), "D:\\ws\\24.3.0.0\\EmLite\\Common\\PerformanceMeter.cs");
      if (this.customizationStartTime != DateTime.MinValue)
        this.customizationTime += DateTime.Now.Subtract(this.customizationStartTime);
      this.customizationStartTime = DateTime.MinValue;
    }

    public bool Stop() => this.stopMeter(false);

    public bool Abort() => this.stopMeter(true);

    private bool stopMeter(bool abort)
    {
      this.stopped = true;
      if (!PerformanceMeter.Enabled || this.name == null || !this.Active)
        return false;
      this.aborted = abort;
      lock (this)
      {
        if (this.startTime == DateTime.MinValue || this.stopTime != DateTime.MaxValue)
          return false;
        this.stopTime = DateTime.Now;
      }
      PerformanceMeter performanceMeter = this;
      while (performanceMeter.childMeter != null)
        performanceMeter = performanceMeter.childMeter;
      for (; performanceMeter != this; performanceMeter = performanceMeter.parentMeter)
      {
        if (performanceMeter.childMeter != null)
          performanceMeter.childMeter.stopMeter(abort);
      }
      if (PerformanceMeter.currentMeter == this)
        PerformanceMeter.currentMeter = this.parentMeter;
      try
      {
        if (PerformanceMeter.MeterStopped != null)
          PerformanceMeter.MeterStopped(this);
      }
      catch
      {
      }
      if (this.parentMeter != null)
      {
        this.parentMeter.AddCheckpoint("#SUBMETER " + (abort ? "ABORTED" : "STOPPED") + ": " + this.name, 362, nameof (stopMeter), "D:\\ws\\24.3.0.0\\EmLite\\Common\\PerformanceMeter.cs");
        this.parentMeter.childMeter = (PerformanceMeter) null;
        this.parentMeter = (PerformanceMeter) null;
      }
      if (!abort && PerformanceMeter.meterMode != PerformanceMeterMode.NoLog)
        this.writeToLog();
      return true;
    }

    public void AddCheckpoint(
      string description,
      [CallerLineNumber] int callerLineNumber = 0,
      [CallerMemberName] string callerMemberName = "",
      [CallerFilePath] string callerFilePath = "")
    {
      if (this.stopped || !PerformanceMeter.Enabled)
        return;
      if (this.name == null)
      {
        Tracing.Log(true, "INFO", nameof (PerformanceMeter), "AddCheckpoint Null Meter Call From " + callerMemberName + ", Line " + (object) callerLineNumber + ", " + callerFilePath);
      }
      else
      {
        lock (this)
          this.checkpoints.Add((object) new PerformanceMeter.Checkpoint(description, DateTime.Now, callerLineNumber, callerMemberName, callerFilePath));
      }
    }

    public void AddNote(string note)
    {
      if (this.stopped || !PerformanceMeter.Enabled || this.name == null)
        return;
      lock (this)
        this.notes.Add((object) note);
    }

    public void AddVariable(string name, object value)
    {
      if (this.stopped || !PerformanceMeter.Enabled || this.name == null)
        return;
      lock (this)
        this.variables[(object) name] = value;
    }

    public IDisposable BeginOperation(string name)
    {
      if (!PerformanceMeter.Enabled)
        return (IDisposable) null;
      lock (this.operations)
      {
        PerformanceMeter.OperationData opData = (PerformanceMeter.OperationData) this.operations[(object) name];
        if (opData == null)
        {
          opData = new PerformanceMeter.OperationData(name);
          this.operations.Add((object) name, (object) opData);
        }
        return (IDisposable) new PerformanceMeter.Operation(opData);
      }
    }

    private XmlDocument getBaseXML()
    {
      XmlDocument baseXml = new XmlDocument();
      baseXml.LoadXml("<Data />");
      if (!string.IsNullOrWhiteSpace(this.Description))
        baseXml.DocumentElement.AppendChild((XmlNode) baseXml.CreateElement("Desc")).InnerText = this.Description;
      foreach (string key in (IEnumerable) this.variables.Keys)
      {
        XmlElement xmlElement = (XmlElement) baseXml.DocumentElement.AppendChild((XmlNode) baseXml.CreateElement("Var"));
        xmlElement.SetAttribute("Name", key);
        xmlElement.InnerText = string.Concat(this.variables[(object) key]);
      }
      foreach (string note in this.notes)
        baseXml.DocumentElement.AppendChild((XmlNode) baseXml.CreateElement("Note")).InnerText = note;
      return baseXml;
    }

    public string GetXmlData()
    {
      return this.variables.Count == 0 ? (string) null : this.getBaseXML().OuterXml;
    }

    public string GetFullXmlData()
    {
      XmlDocument baseXml = this.getBaseXML();
      XmlElement xmlElement1 = (XmlElement) baseXml.DocumentElement.AppendChild((XmlNode) baseXml.CreateElement("Files"));
      Dictionary<string, string> allFilePaths = new Dictionary<string, string>();
      XmlElement element = baseXml.CreateElement("PM");
      baseXml.DocumentElement.AppendChild((XmlNode) element);
      element.SetAttribute("Name", this.name);
      element.SetAttribute("Start", this.startTime.ToString());
      element.SetAttribute("Stop", this.stopTime.ToString());
      element.SetAttribute("ET", (this.stopTime - this.startTime).TotalMilliseconds.ToString());
      element.SetAttribute("CustomizationTime", this.customizationTime.TotalMilliseconds.ToString());
      PerformanceMeter.Checkpoint checkpoint1 = (PerformanceMeter.Checkpoint) null;
      foreach (PerformanceMeter.Checkpoint checkpoint2 in this.checkpoints)
      {
        string shortFileName = this.GetShortFileName(allFilePaths, checkpoint2.CallerFilePath);
        TimeSpan timeSpan = checkpoint2.ElapsedTime(this.startTime);
        this.setCheckPointElement(baseXml, element, checkpoint2.Description, timeSpan.TotalMilliseconds, checkpoint1 == null ? 0.0 : checkpoint2.ElapsedTime(checkpoint1.Timestamp).TotalMilliseconds, checkpoint2.CallerLineNumber, checkpoint2.CallerMemberName, shortFileName);
        checkpoint1 = checkpoint2;
      }
      if (this.operations.Count > 0)
      {
        foreach (PerformanceMeter.OperationData op in (IEnumerable) this.operations.Values)
          this.setOperationElement(baseXml, element, op);
      }
      foreach (string key in allFilePaths.Keys)
      {
        XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) baseXml.CreateElement("File"));
        xmlElement2.SetAttribute("Name", allFilePaths[key]);
        xmlElement2.SetAttribute("Path", key);
      }
      return baseXml.OuterXml;
    }

    private string GetShortFileName(Dictionary<string, string> allFilePaths, string filePath)
    {
      string shortFileName = (string) null;
      if (!string.IsNullOrWhiteSpace(filePath))
      {
        if (allFilePaths.ContainsKey(filePath))
        {
          shortFileName = allFilePaths[filePath];
        }
        else
        {
          shortFileName = Path.GetFileName(filePath);
          if (allFilePaths.ContainsValue(shortFileName))
          {
            string[] strArray = filePath.Split(Path.PathSeparator);
            int index = strArray.Length - 1;
            do
            {
              --index;
              shortFileName = strArray[index] + Path.PathSeparator.ToString() + shortFileName;
            }
            while (allFilePaths.ContainsValue(shortFileName));
          }
          allFilePaths[filePath] = shortFileName;
        }
      }
      return shortFileName;
    }

    private void setCheckPointElement(
      XmlDocument doc,
      XmlElement parentElement,
      string name,
      double elapsed,
      double diff,
      int lineNum,
      string methodName,
      string shortFileName)
    {
      XmlElement element = doc.CreateElement("CP");
      parentElement.AppendChild((XmlNode) element);
      element.SetAttribute("Tag", name);
      element.SetAttribute("ET", elapsed.ToString());
      element.SetAttribute("DT", diff.ToString());
      if (!string.IsNullOrWhiteSpace(methodName))
        element.SetAttribute("ME", methodName);
      if (!string.IsNullOrWhiteSpace(shortFileName))
        element.SetAttribute("FL", shortFileName);
      if (lineNum <= 0)
        return;
      element.SetAttribute("LN", lineNum.ToString());
    }

    private void setOperationElement(
      XmlDocument doc,
      XmlElement parentElement,
      PerformanceMeter.OperationData op)
    {
      XmlElement element = doc.CreateElement("OP");
      parentElement.AppendChild((XmlNode) element);
      element.SetAttribute("Name", op.Name);
      element.SetAttribute("ET", op.TotalMilliseconds.ToString());
      element.SetAttribute("AV", op.AverageTime.ToString());
      element.SetAttribute("CT", op.Count.ToString());
    }

    private void writeToLog()
    {
      StreamWriter streamWriter = (StreamWriter) null;
      lock (typeof (PerformanceMeter))
      {
        if (PerformanceMeter.FilePath != null)
        {
          if (PerformanceMeter.perfWriters.ContainsKey(PerformanceMeter.FilePath))
            streamWriter = PerformanceMeter.perfWriters[PerformanceMeter.FilePath];
        }
      }
      if (streamWriter == null)
        return;
      lock (streamWriter)
      {
        try
        {
          streamWriter.WriteLine();
          streamWriter.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
          streamWriter.WriteLine("<<< PerformanceMeter: " + this.name);
          streamWriter.WriteLine("<<< Start Time:        " + this.startTime.ToString());
          streamWriter.WriteLine("<<< Stop Time:         " + this.stopTime.ToString());
          streamWriter.WriteLine("<<< Elapsed Time:      " + (object) (this.stopTime - this.startTime).TotalMilliseconds + " ms");
          streamWriter.WriteLine("<<< Customization Time:      " + (object) this.customizationTime.TotalMilliseconds + " ms");
          if ((this.description ?? "") != "")
            streamWriter.WriteLine("<<< Description:      " + this.description);
          streamWriter.WriteLine();
          streamWriter.WriteLine("<<< {0,-53} {1,12} {2,10}", (object) "Checkpoints", (object) "Elapsed (ms)", (object) "Diff (ms)");
          streamWriter.WriteLine("    + {0,-50} {1,12:0} {2,10:0}", (object) "#START", (object) 0, (object) 0);
          PerformanceMeter.Checkpoint checkpoint1 = (PerformanceMeter.Checkpoint) null;
          foreach (PerformanceMeter.Checkpoint checkpoint2 in this.checkpoints)
          {
            TimeSpan timeSpan = checkpoint2.ElapsedTime(this.startTime);
            streamWriter.WriteLine("    + {0,-50} {1,12:0} {2,10:0} {3}", (object) checkpoint2.Description, (object) timeSpan.TotalMilliseconds, (object) (checkpoint1 == null ? 0.0 : checkpoint2.ElapsedTime(checkpoint1.Timestamp).TotalMilliseconds), (object) ("- LINE: " + (object) checkpoint2.CallerLineNumber + ",  MEMBER: " + checkpoint2.CallerMemberName + ", FILE: " + checkpoint2.CallerFilePath));
            checkpoint1 = checkpoint2;
          }
          TimeSpan timeSpan1 = this.stopTime - this.startTime;
          streamWriter.WriteLine("    + {0,-50} {1,12:0} {2,10:0}", (object) "#STOP", (object) timeSpan1.TotalMilliseconds, (object) (checkpoint1 == null ? 0.0 : (this.stopTime - checkpoint1.Timestamp).TotalMilliseconds));
          streamWriter.WriteLine();
          if (this.operations.Count > 0)
          {
            streamWriter.WriteLine("<<< {0,-52} {1,12} {2,12} {3,10}", (object) "Operations", (object) "Count", (object) "Total (ms)", (object) "Avg (ms)");
            foreach (PerformanceMeter.OperationData operationData in (IEnumerable) this.operations.Values)
              streamWriter.WriteLine("    + {0,-50} {1,12} {2,12:0} {3,10:0}", (object) operationData.Name, (object) operationData.Count, (object) operationData.TotalMilliseconds, (object) operationData.AverageTime);
            streamWriter.WriteLine();
          }
          if (this.notes.Count > 0)
          {
            streamWriter.WriteLine("<<< Notes");
            for (int index = 0; index < this.notes.Count; ++index)
              streamWriter.WriteLine("    + " + PerformanceMeter.wrapText(this.notes[index].ToString(), 74, "      "));
            streamWriter.WriteLine();
          }
          if (this.variables.Count > 0)
          {
            streamWriter.WriteLine("<<< Variables");
            foreach (string key in (IEnumerable) this.variables.Keys)
              streamWriter.WriteLine("    + " + key + " = " + this.variables[(object) key]);
            streamWriter.WriteLine();
          }
          streamWriter.Flush();
        }
        catch (ObjectDisposedException ex)
        {
        }
      }
    }

    public void Dispose()
    {
      try
      {
        if (this.abortOnDispose)
          this.Abort();
        else
          this.Stop();
      }
      catch
      {
      }
    }

    public static bool Enabled => PerformanceMeter.meterMode != 0;

    public static PerformanceMeterMode MeterMode
    {
      get => PerformanceMeter.meterMode;
      set
      {
        lock (typeof (PerformanceMeter))
          PerformanceMeter.meterMode = value;
      }
    }

    public static PerformanceMeter StartNew(
      string name,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      return PerformanceMeter.StartNew(name, "", creatorLineNumber, creatorMemberName, creatorFilePath);
    }

    public static PerformanceMeter StartNew(
      string name,
      string description,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      return PerformanceMeter.StartNew(name, description, false, creatorLineNumber, creatorMemberName, creatorFilePath);
    }

    public static PerformanceMeter StartNew(
      string name,
      string description,
      bool publish,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      return PerformanceMeter.StartNew(name, description, publish, false, creatorLineNumber, creatorMemberName, creatorFilePath);
    }

    public static PerformanceMeter StartNew(
      string name,
      string description,
      bool publish,
      bool abortOnDispose,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      return PerformanceMeter.StartNew(name, description, publish, abortOnDispose, false, creatorLineNumber, creatorMemberName, creatorFilePath);
    }

    public static PerformanceMeter StartNew(
      string name,
      string description,
      bool publish,
      bool abortOnDispose,
      bool combinedPublication,
      [CallerLineNumber] int creatorLineNumber = 0,
      [CallerMemberName] string creatorMemberName = "",
      [CallerFilePath] string creatorFilePath = "")
    {
      PerformanceMeter performanceMeter = new PerformanceMeter(name, description, publish, abortOnDispose, combinedPublication, creatorLineNumber, creatorMemberName, creatorFilePath);
      performanceMeter.Start();
      return performanceMeter;
    }

    public static PerformanceMeter Current
    {
      get
      {
        return PerformanceMeter.currentMeter != null ? PerformanceMeter.currentMeter : PerformanceMeter.Null;
      }
    }

    public static void SetCurrent(PerformanceMeter meter) => PerformanceMeter.currentMeter = meter;

    public static PerformanceMeter Get(string name)
    {
      for (PerformanceMeter performanceMeter = PerformanceMeter.currentMeter; performanceMeter != null; performanceMeter = performanceMeter.parentMeter)
      {
        if (string.Compare(name, performanceMeter.name, true) == 0)
          return performanceMeter;
      }
      return PerformanceMeter.Null;
    }

    public static void AbortAll()
    {
      PerformanceMeter parentMeter;
      for (PerformanceMeter performanceMeter = PerformanceMeter.currentMeter; performanceMeter != null; performanceMeter = parentMeter)
      {
        parentMeter = performanceMeter.parentMeter;
        performanceMeter.Abort();
      }
    }

    public static void Abort(string name)
    {
      PerformanceMeter parentMeter;
      for (PerformanceMeter performanceMeter = PerformanceMeter.currentMeter; performanceMeter != null; performanceMeter = parentMeter)
      {
        parentMeter = performanceMeter.parentMeter;
        if (string.Compare(name, performanceMeter.name, true) == 0)
          performanceMeter.Abort();
      }
    }

    private static void openMeterFile()
    {
      try
      {
        string filePath = PerformanceMeter.FilePath;
        Tracing.Log(true, "INFO", nameof (PerformanceMeter), "Using PerfMeter folder at " + filePath);
        Directory.CreateDirectory(filePath);
        string path = Path.Combine(filePath, "perf.log");
        bool append = PerformanceMeter.meterMode == PerformanceMeterMode.Append;
        int num = 0;
        StreamWriter streamWriter1 = (StreamWriter) null;
        while (streamWriter1 == null)
        {
          if (PerformanceMeter.meterMode == PerformanceMeterMode.Preserve)
          {
            while (File.Exists(path))
              path = Path.Combine(filePath, "perf." + (object) ++num + ".log");
          }
          try
          {
            StreamWriter streamWriter2 = new StreamWriter(path, append, Encoding.ASCII);
            Tracing.Log(true, "INFO", nameof (PerformanceMeter), "Using PerfMeter file at " + path);
            streamWriter2.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            streamWriter2.WriteLine(">>> Performance Metering Started at " + DateTime.Now.ToString());
            streamWriter2.WriteLine(">>>");
            streamWriter2.Flush();
            streamWriter1 = streamWriter2;
          }
          catch (IOException ex)
          {
            path = ++num <= 100 ? Path.Combine(filePath, "perf" + (object) num + ".log") : throw new Exception("Exceeded maximum perf file count");
          }
        }
        PerformanceMeter.perfWriters[PerformanceMeter.FilePath] = streamWriter1;
      }
      catch (Exception ex)
      {
        throw new Exception("Error opening perf.log file: " + ex.Message, ex);
      }
    }

    private static string wrapText(string text, int linelength, string linePrefix)
    {
      text = text.Trim();
      string str = "";
      int num1 = 0;
      while (text.Length > linelength)
      {
        int num2 = linelength;
        while (num2 >= 1 && text[num2] != ' ')
          --num2;
        if (num2 > 0)
        {
          if (num1 > 0)
            str = str + Environment.NewLine + linePrefix;
          str += text.Substring(0, num2).Trim();
          text = text.Substring(num2).Trim();
          ++num1;
        }
        else
          break;
      }
      if (num1 > 0)
        str = str + Environment.NewLine + linePrefix;
      return str + text;
    }

    public static bool StartNewIfNoCurrentMeter(string newMeterName, string newMeterDescription)
    {
      bool isNullMeter = PerformanceMeter.Current.IsNullMeter;
      if (isNullMeter)
        PerformanceMeter.StartNew(newMeterName, newMeterDescription, 873, nameof (StartNewIfNoCurrentMeter), "D:\\ws\\24.3.0.0\\EmLite\\Common\\PerformanceMeter.cs");
      return isNullMeter;
    }

    private class Checkpoint
    {
      public string Description;
      public DateTime Timestamp;
      public string CallerMemberName;
      public string CallerFilePath;
      public int CallerLineNumber;

      public Checkpoint(
        string description,
        DateTime timestamp,
        int callerLineNumber = 0,
        string callerMemberName = "",
        string callerFilePath = "")
      {
        this.Description = description;
        this.Timestamp = timestamp;
        this.CallerMemberName = callerMemberName;
        this.CallerFilePath = callerFilePath;
        this.CallerLineNumber = callerLineNumber;
      }

      public TimeSpan ElapsedTime(DateTime startTime) => this.Timestamp - startTime;
    }

    private class OperationData
    {
      public string Name;
      public int Count;
      public double TotalMilliseconds;

      public OperationData(string name) => this.Name = name;

      public double AverageTime
      {
        get => this.Count != 0 ? this.TotalMilliseconds / (double) this.Count : 0.0;
      }

      public void AddOperation(double timeTakenMs)
      {
        lock (this)
        {
          ++this.Count;
          this.TotalMilliseconds += timeTakenMs;
        }
      }
    }

    private class Operation : IDisposable
    {
      private PerformanceMeter.OperationData opData;
      private DateTime startTime = DateTime.Now;

      public Operation(PerformanceMeter.OperationData opData) => this.opData = opData;

      public void Dispose()
      {
        this.opData.AddOperation((DateTime.Now - this.startTime).TotalMilliseconds);
      }
    }
  }
}
