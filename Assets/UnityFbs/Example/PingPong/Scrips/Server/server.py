from flask import Flask
from flask import request

from FbsCompilled.Vec3 import Vec3
from FbsCompilled.BallCoordinates import BallCoordinates
from FbsCompilled.MoveDirection import MoveDirection
from FbsCompilled.PlayerAction import *


class ComputerPlayer:
    def move(self, ball):
        t = ball.Position().X() / ball.Direction().X()
        target = ball.Position().Y() + t * ball.Direction().Y()
        print (f"Position ({ball.Position().X(), ball.Position().Y()}), velocity ({ball.Direction().X(), ball.Direction().Y()}), time={t}, target={target}")
        return MoveDirection.Up if target > 0 else (MoveDirection.Down if target < 0 else MoveDirection.None_)

    def handle(self, data):
        ball = BallCoordinates.GetRootAsBallCoordinates(data, 0)
        
        builder = flatbuffers.Builder(1)
        PlayerActionStart(builder)
        PlayerActionAddMoveDirection(builder, self.move(ball))
        end = PlayerActionEnd(builder)
        builder.Finish(end)
        return builder.Output()


app = Flask(__name__)
player = ComputerPlayer()


@app.route('/')
def get_ready():
    return 'Ok'

@app.route('/next_action/', methods=['POST'])
def next_action():
    return player.handle(request.get_data())
