// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.ResFileInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using EllieMae.Encompass.AsmResolver.XmlHelperClasses;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class ResFileInfo : AppFileInfo
  {
    private readonly XEfile xeFile;
    private readonly Dictionary<HashAlgorithm, HashInfo> hashDict;

    public override long Size
    {
      get => this.size;
      set
      {
        this.size = value;
        if (this.xeFile == null)
          return;
        this.xeFile.Size = this.size;
      }
    }

    internal ResFileInfo(XEfile xeFile)
      : this(xeFile.Path, xeFile.Size, xeFile.FileVersion, xeFile.Version, xeFile.Group, xeFile.OnDemandOnly, xeFile.HashList)
    {
      this.xeFile = xeFile;
    }

    private ResFileInfo(
      string filePath,
      long size,
      string fileVersion,
      Version version,
      string group,
      bool onDemandOnly,
      List<XEhash> hashList)
      : base(filePath, size, fileVersion, version, group, onDemandOnly)
    {
      if (this.IsCLRAssembly)
        return;
      this.hashDict = new Dictionary<HashAlgorithm, HashInfo>();
      foreach (XEhash hash in hashList)
      {
        HashInfo hashInfo = new HashInfo(hash.Algorithm, hash.DigestValue);
        this.hashDict.Add(hashInfo.Algorithm, hashInfo);
      }
    }

    public ResFileInfo(
      string filePath,
      long size,
      string fileVersion,
      Version version,
      bool onDemandOnly,
      HashInfo hashInfo)
      : base(filePath, size, fileVersion, version, (string) null, onDemandOnly)
    {
      if (this.IsCLRAssembly)
        return;
      this.hashDict = new Dictionary<HashAlgorithm, HashInfo>();
      this.hashDict.Add(hashInfo.Algorithm, hashInfo);
    }

    public ResFileInfo(string basePath, string filePath, HashAlgorithm hashAlgorithm)
      : base(basePath, filePath)
    {
      if (this.IsCLRAssembly)
        return;
      this.hashDict = new Dictionary<HashAlgorithm, HashInfo>();
      this.hashDict.Add(hashAlgorithm, this.getAndWriteHashInfo(hashAlgorithm, basePath, filePath));
    }

    public ResFileInfo(string basePath, string filePath, bool writeHashFile)
      : base(basePath, filePath)
    {
      if (this.IsCLRAssembly)
        return;
      this.hashDict = new Dictionary<HashAlgorithm, HashInfo>();
      this.hashDict.Add(HashAlgorithm.SHA1, this.getAndWriteHashInfo(HashAlgorithm.SHA1, basePath, filePath, writeHashFile));
      this.hashDict.Add(HashAlgorithm.MD5, this.getAndWriteHashInfo(HashAlgorithm.MD5, basePath, filePath, writeHashFile));
    }

    public void WriteHashToHashFile(
      string basePath,
      HashAlgorithm hashAlgorithm,
      bool dontWriteIfAssembly)
    {
      if (dontWriteIfAssembly && this.IsCLRAssembly)
        return;
      HashInfo hashInfo = this.hashDict[hashAlgorithm];
      if (hashInfo == null)
        return;
      string path2 = DeployUtils.RemoveDeployExtension(this.FilePath);
      string hashFileExt = this.getHashFileExt(hashAlgorithm);
      SystemUtils.MutexFileWrite(Path.Combine(basePath, path2) + hashFileExt, hashInfo.DigestValueB64);
    }

    public void WriteHashToHashFile(string basePath, bool dontWriteIfAssembly)
    {
      foreach (HashAlgorithm key in this.hashDict.Keys)
        this.WriteHashToHashFile(basePath, key, dontWriteIfAssembly);
    }

    private HashInfo getAndWriteHashInfo(
      HashAlgorithm hashAlgorithm,
      string basePath,
      string filePath)
    {
      return this.getAndWriteHashInfo(hashAlgorithm, basePath, filePath, true);
    }

    private HashInfo getAndWriteHashInfo(
      HashAlgorithm hashAlgorithm,
      string basePath,
      string filePath,
      bool writeHashFile)
    {
      string str1 = Path.Combine(basePath, filePath);
      if (!File.Exists(str1))
        return (HashInfo) null;
      string hashFileExt = this.getHashFileExt(hashAlgorithm);
      string str2;
      if (File.Exists(str1 + hashFileExt))
      {
        str2 = File.ReadAllText(str1 + hashFileExt);
      }
      else
      {
        str2 = HashUtil.ComputeHashB64(hashAlgorithm, str1);
        if (writeHashFile)
          SystemUtils.MutexFileWrite(str1 + hashFileExt, str2);
      }
      return new HashInfo(hashAlgorithm, str2);
    }

    private string getHashFileExt(HashAlgorithm hashAlgorithm)
    {
      string hashFileExt = ResolverConsts.Sha1HashFileExt;
      if (hashAlgorithm == HashAlgorithm.MD5)
        hashFileExt = ResolverConsts.Md5HashFileExt;
      return hashFileExt;
    }

    public HashInfo GetHashInfo(HashAlgorithm hashAlgorithm) => this.hashDict[hashAlgorithm];

    public HashInfo[] GetHashInfos()
    {
      List<HashInfo> hashInfoList = new List<HashInfo>();
      foreach (HashAlgorithm key in this.hashDict.Keys)
        hashInfoList.Add(this.hashDict[key]);
      return hashInfoList.ToArray();
    }

    public bool IsSameVersion(ResFileInfo resFileInfo, HashAlgorithm algorithm)
    {
      if (!this.IsCLRAssembly)
        return this.hasSameHash(algorithm, resFileInfo.GetHashInfos());
      return this.FileVersion == resFileInfo.FileVersion;
    }

    private bool hasSameHash(HashAlgorithm algorithm, HashInfo[] hashInfos)
    {
      HashInfo hashInfo1 = this.hashDict[algorithm];
      HashInfo hashInfo2 = (HashInfo) null;
      if (hashInfos != null)
      {
        foreach (HashInfo hashInfo3 in hashInfos)
        {
          if (hashInfo3.Algorithm == algorithm)
          {
            hashInfo2 = hashInfo3;
            break;
          }
        }
      }
      return hashInfo1 == null && hashInfo2 == null || hashInfo1 != null && hashInfo2 != null && hashInfo1.DigestValueB64 == hashInfo2.DigestValueB64;
    }

    public void AddDeployZipExtToResFilePath()
    {
      if (this.xeFile != null)
        this.xeFile.AddDeployZipExtToPath();
      this.FilePath = this.xeFile.Path;
    }

    public bool IsCLRAssembly
    {
      get => this.FileVersion != (Version) null && this.Version != (Version) null;
    }
  }
}
