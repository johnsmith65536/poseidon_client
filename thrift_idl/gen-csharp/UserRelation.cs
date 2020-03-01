/**
 * Autogenerated by Thrift Compiler (0.13.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class UserRelation : TBase
{
  private long _Id;
  private long _UserIdSend;
  private long _UserIdRecv;
  private long _CreateTime;
  private int _Status;

  public long Id
  {
    get
    {
      return _Id;
    }
    set
    {
      __isset.Id = true;
      this._Id = value;
    }
  }

  public long UserIdSend
  {
    get
    {
      return _UserIdSend;
    }
    set
    {
      __isset.UserIdSend = true;
      this._UserIdSend = value;
    }
  }

  public long UserIdRecv
  {
    get
    {
      return _UserIdRecv;
    }
    set
    {
      __isset.UserIdRecv = true;
      this._UserIdRecv = value;
    }
  }

  public long CreateTime
  {
    get
    {
      return _CreateTime;
    }
    set
    {
      __isset.CreateTime = true;
      this._CreateTime = value;
    }
  }

  public int Status
  {
    get
    {
      return _Status;
    }
    set
    {
      __isset.Status = true;
      this._Status = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool Id;
    public bool UserIdSend;
    public bool UserIdRecv;
    public bool CreateTime;
    public bool Status;
  }

  public UserRelation() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I64) {
              Id = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              UserIdSend = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              UserIdRecv = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              CreateTime = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Status = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("UserRelation");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.Id) {
        field.Name = "Id";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Id);
        oprot.WriteFieldEnd();
      }
      if (__isset.UserIdSend) {
        field.Name = "UserIdSend";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(UserIdSend);
        oprot.WriteFieldEnd();
      }
      if (__isset.UserIdRecv) {
        field.Name = "UserIdRecv";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(UserIdRecv);
        oprot.WriteFieldEnd();
      }
      if (__isset.CreateTime) {
        field.Name = "CreateTime";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(CreateTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.Status) {
        field.Name = "Status";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Status);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("UserRelation(");
    bool __first = true;
    if (__isset.Id) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Id: ");
      __sb.Append(Id);
    }
    if (__isset.UserIdSend) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("UserIdSend: ");
      __sb.Append(UserIdSend);
    }
    if (__isset.UserIdRecv) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("UserIdRecv: ");
      __sb.Append(UserIdRecv);
    }
    if (__isset.CreateTime) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CreateTime: ");
      __sb.Append(CreateTime);
    }
    if (__isset.Status) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Status: ");
      __sb.Append(Status);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

