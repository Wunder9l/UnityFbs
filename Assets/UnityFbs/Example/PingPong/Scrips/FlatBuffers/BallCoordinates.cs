// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace PingPong
{

using global::System;
using global::FlatBuffers;

public struct Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<PingPong.Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<PingPong.Vec3>(builder.Offset);
  }
};

public struct BallCoordinates : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_11_1(); }
  public static BallCoordinates GetRootAsBallCoordinates(ByteBuffer _bb) { return GetRootAsBallCoordinates(_bb, new BallCoordinates()); }
  public static BallCoordinates GetRootAsBallCoordinates(ByteBuffer _bb, BallCoordinates obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public BallCoordinates __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public PingPong.Vec3? Position { get { int o = __p.__offset(4); return o != 0 ? (PingPong.Vec3?)(new PingPong.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public PingPong.Vec3? Direction { get { int o = __p.__offset(6); return o != 0 ? (PingPong.Vec3?)(new PingPong.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public float Speed { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static void StartBallCoordinates(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddPosition(FlatBufferBuilder builder, Offset<PingPong.Vec3> positionOffset) { builder.AddStruct(0, positionOffset.Value, 0); }
  public static void AddDirection(FlatBufferBuilder builder, Offset<PingPong.Vec3> directionOffset) { builder.AddStruct(1, directionOffset.Value, 0); }
  public static void AddSpeed(FlatBufferBuilder builder, float speed) { builder.AddFloat(2, speed, 0.0f); }
  public static Offset<PingPong.BallCoordinates> EndBallCoordinates(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PingPong.BallCoordinates>(o);
  }
  public static void FinishBallCoordinatesBuffer(FlatBufferBuilder builder, Offset<PingPong.BallCoordinates> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedBallCoordinatesBuffer(FlatBufferBuilder builder, Offset<PingPong.BallCoordinates> offset) { builder.FinishSizePrefixed(offset.Value); }
};


}
