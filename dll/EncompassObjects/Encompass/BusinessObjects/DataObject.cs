// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.DataObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  public class DataObject : IDataObject, IDisposable
  {
    private BinaryObject data;
    private byte[] byteData;

    internal DataObject(BinaryObject data) => this.data = data;

    public DataObject() => this.data = new BinaryObject(new byte[0]);

    public DataObject(byte[] data) => this.data = new BinaryObject(data);

    public DataObject(string filePath) => this.data = new BinaryObject(filePath);

    public DataObject(Stream stream) => this.data = new BinaryObject(stream, true);

    public int Size => (int) this.data.Length;

    public byte[] Data
    {
      get
      {
        if (this.byteData == null)
          this.byteData = this.data.GetBytes();
        return this.byteData;
      }
    }

    public string ToString(Encoding encoding) => this.data.ToString(encoding);

    public Stream OpenStream() => this.data.AsStream();

    public void SaveToDisk(string filePath) => this.data.Write(filePath);

    public void Load(byte[] data) => this.data = new BinaryObject(data);

    public void LoadFile(string filePath) => this.data = new BinaryObject(filePath);

    internal BinaryObject Unwrap() => this.data;

    public void Dispose()
    {
      if (this.data == null)
        return;
      this.data.Dispose();
      this.data = (BinaryObject) null;
    }
  }
}
