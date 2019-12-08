using FlatBuffers;
using PingPong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbsSerializer {
    public static byte[] SerializeBallCoordinates(Vector3 position, Vector3 direction, float speed) {
        var builder = new FlatBufferBuilder(1);

        BallCoordinates.StartBallCoordinates(builder);
        BallCoordinates.AddPosition(builder, Vec3.CreateVec3(builder, position.x, position.y, position.z));
        BallCoordinates.AddDirection(builder, Vec3.CreateVec3(builder, direction.x, direction.y, direction.z));
        BallCoordinates.AddSpeed(builder, speed);
        var ball = BallCoordinates.EndBallCoordinates(builder);
        BallCoordinates.FinishBallCoordinatesBuffer(builder, ball);
        return builder.SizedByteArray();
    }

    public static PlayerAction GetPlayerAction(byte[] bytes) {
        return PlayerAction.GetRootAsPlayerAction(new ByteBuffer(bytes));
    }
}
