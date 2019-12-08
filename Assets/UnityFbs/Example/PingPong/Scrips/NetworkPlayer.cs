using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using PingPong;

public class NetworkPlayer : MonoBehaviour {
    // Start is called before the first frame update
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    private HttpClient client = new HttpClient();
    private string host = "localhost:5000";
    private MoveDirection currentMove = MoveDirection.None;

    public Transform ball;
    public Transform player;
    public PingPongGame game;

    public void Connect(string host) {
        this.host = host;
    }
    private void Start() {
        game.onGameStart += OnGameStart;
    }
    public void OnGameStart() {
        StopAllCoroutines();
        StartCoroutine(StartNetworkExchange());
    }

    void Update() {
        var newY = player.position.y + (currentMove == MoveDirection.Up ? 1 : -1) * game.playerSpeed * Time.deltaTime;
        if (currentMove != MoveDirection.None && Mathf.Abs(newY) < game.playerLimitY) {
            player.position = new Vector2(
                player.position.x,
                newY
            );
        }
    }

    IEnumerator StartNetworkExchange() {
        while (game.isGameRunning) {
            using (WWW www = new WWW($"http://{host}/next_action/", GetPostData())) {
                yield return www;
                HandleMessage(www.bytes);
            }
        }
    }

    private void HandleMessage(byte[] responseBody) {
        var action = FbsSerializer.GetPlayerAction(responseBody);
        Debug.Log($"Server responded on next_action with {action.MoveDirection}");
        currentMove = action.MoveDirection;
    }

    private byte[] GetPostData() {
        var rigidbody = ball.GetComponent<Rigidbody>();
        return FbsSerializer.SerializeBallCoordinates(rigidbody.position - player.position, rigidbody.velocity, rigidbody.velocity.magnitude);
    }
}
