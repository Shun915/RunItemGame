using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreeMove : MonoBehaviour
{
    CharacterController controller; //CharacterControllerを参照できる変数
    [SerializeField] private GameObject animatorBody; //Animatorコンポーネントをもっているオブジェクトの取得
    Animator animator; //Animatorを参照できる変数

    //Vector3 moveDirection = new Vector3(0, 0, 0);
    Vector3 moveDirection = Vector3.zero; //Moveメソッドに与える方向

    public float gravity; //重力値
    public float speedZ; //前に進む力
    public float speedJump; //ジャンプ力

    private InputAction moveAction;
    private InputAction jumpAction;

    void Start()
    {
        //Playerについているコンポーネントの取得
        controller = GetComponent<CharacterController>();
        animator = animatorBody.GetComponent<Animator>();

        //PlayerInputコンポーネントに設置されている現MAPは何かを取得＝(PlayerMap)
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        moveAction = actionMap.FindAction("Move");
        moveAction = actionMap.FindAction("Jump");
    }

    void Update()
    {
        //CharacterControllerコンポーネントが接地判断をもっている
        //入力検知は地面にいる時
        if (controller.isGrounded)
        {
            //上下キーによる前か後ろかの値の設定
            //if (Input.GetAxis("Vertical") != 0.0f)
            if(moveAction.ReadValue<Vector2>().y!=0.0f)
            {
                //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
                moveDirection.z = moveAction.ReadValue<Vector2>().y* speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            //横回転 Rotateメソッドで直接回転させる
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
            transform.Rotate(0,moveAction.ReadValue<Vector2>().x * 300*Time.deltaTime,0);

            //ジャンプボタン　押されている間
            //if (Input.GetButton("Jump"))
            if(jumpAction.IsInProgress())
            {
                moveDirection.y = speedJump; //y方向をセッティング
                //animator.SetTrigger("jump"); //ジャンプアニメへの切替
            }
        }

        //常に重力はかかっている
        moveDirection.y -= gravity;

        //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
        //※実際にPlayerが動く Time.deltaTimeでPC性能差によらないように動作安定
        //回転を含む前進だと、グローバルな座標指定からズレるので、グローバルな座標になるように変換してからMoveに与える
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //地面にいるのであればY成分の力を毎回0にリセット
        if (controller.isGrounded) moveDirection.y = 0;

        //アニメの制御
        //animator.SetBool("run", moveDirection.z != 0.0f);
    }
}
