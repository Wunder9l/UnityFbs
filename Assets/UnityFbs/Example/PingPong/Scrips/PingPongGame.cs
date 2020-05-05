using System;
using UnityEngine;
using UnityEngine.UI;

public class PingPongGame : MonoBehaviour {
    public Transform player;
    public Transform ball;
    public int startBallSpeed = 100;
    public float playerSpeed = 5f;
    public bool isGameRunning = false;
    public float playerLimitY = 2f;
    public delegate void OnGameStartCallback();
    public OnGameStartCallback onGameStart;
    public NetworkPlayer computerPlayer;
    private Text textBox;

    private void Awake() {
        textBox = GameObject.Find("TextBox").GetComponent<Text>();
    }
    public void StartGame() {
        PrintMessage("");
        if (computerPlayer?.IsReady() != true) {
            PrintMessage("Computer player is not ready. Have you started server?");
            return;
        }
        Reset(0);
        isGameRunning = true;
        Run();
        onGameStart?.Invoke();
    }

    private void PrintMessage(string message) {
        textBox.text = message;
    }

    void Run() {
        ball.GetComponent<Rigidbody>().WakeUp();
        Vector2 direction = new Vector2(1, UnityEngine.Random.Range(1.5f, -1.5f));
        if (UnityEngine.Random.Range(0, 2) == 1) direction.x *= -1;
        ball.GetComponent<Rigidbody>().velocity = direction * 3.5f; // .AddForce(direction * startBallSpeed);
    }
    void Update() {
        if (Input.GetAxis("Vertical") > 0 && player.position.y < playerLimitY) {
            player.Translate(Vector2.up * playerSpeed * Time.deltaTime);
        } else if (Input.GetAxis("Vertical") < 0 && player.position.y > -playerLimitY) {
            player.Translate(-Vector2.up * playerSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(ball.position.x) > 10f) {
            Reset(0);
        }
    }

    public void Reset(float x) {
        isGameRunning = false;
        ball.GetComponent<Rigidbody>().Sleep();
        player.position = new Vector2(player.position.x, 0);
        ball.position = new Vector3(-0.8f, 0, 0);
    }
}
