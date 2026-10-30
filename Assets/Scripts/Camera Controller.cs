using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff;　//対象とカメラの維持するべき距離

    //public GameObject target;　//カメラの対象
    GameObject Player;

    public float followSpeed =5.0f;　//追従スピード


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //diff = target.transform.position - transform.position;　//最初に維持するべき距離を定義
        diff = Player.transform.position - transform.position;
    }

    private void LateUpdate() //プレイヤーが動いた(Update.FixedUpdate)後に実行される
    {
        //現地点A→目的地Bに向けて収束していく
        //現地点Aの位置はどんどん詰めていくのでやがて限りなくイコールに近い距離になってくる
        //最終的には限りなく近づき続けるだけでイコールにはならないが、描画上は完全に止まっているように表現される
        transform.position = Vector3.Lerp(
            transform.position, //現地点A
            Player.transform.position - diff, //目的地B
            Time.deltaTime * followSpeed //A→Bへの進行に対して、どれだけ進んだ位置にいるべきかの進歩率
            );

    }
    void Update()
    {
        
    }
}
