using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("攻撃力")]
    private int _attackPower;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private Vector2 _moveDirection;
    private bool _bFloor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _moveDirection = Vector2.left;
        _bFloor = true;

    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _ChangeMoveDirection();
        _LookMoveDirection();
        _HitFloor();
    }
    private void _Move()
    {
        if (_bFloor == false) return; // 以降の移動処理を実行せず、メソッドを抜ける
        _rigid.linearVelocity = new Vector2(_moveDirection.x * _moveSpeed, _rigid.linearVelocity.y);
    }
    private void _ChangeMoveDirection()
    {
       Vector2 halfSize = transform.lossyScale / 2.0f;
       int layerMask = LayerMask.GetMask("Floor");
       RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
       if (ray.transform == null) return;
       if (ray.transform.tag == "Floor")
       {
        _moveDirection = -_moveDirection;
       }
    }
    private void _LookMoveDirection()
    {
        if (_moveDirection.x < 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_moveDirection.x > 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }
    private void _HitFloor()
    {
        // 【判定用の設定】
        // フロアのレイヤー番号を取得
        LayerMask layerMask = LayerMask.GetMask("Floor");
        // 敵の足元の座標
        Vector2 rayPos = transform.position + new Vector3(0.0f, -0.5f);
        // プレイヤーの横幅よりやや短く、高さがない四角形サイズ
        Vector2 raySize = new Vector2(0.9f, 0.1f);
    
        // 【当たり判定の実行】
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0, Vector2.zero, 0, layerMask);

        // 【結果に基づく処理】
        if (rayHit.transform == null) // Raycastが何もオブジェクトに当たっていない（空中）の場合
        {
            _bFloor = false; // b_FloorをFalseに設定
            _anim.SetBool("Isidle", true); // アニメーションをIdleに切り替え
        }
        else // RaycastがFloorオブジェクトに当たっている（着地している）場合
        {
            // 落下中に着地した場合
            if (_bFloor == false)
            {
                _bFloor = true; // b_FloorをTrueに戻す
                _anim.SetBool("Isidle", false); // アニメーションをWalkに切り替え
            }
    }
}
    
    public void PlayerDamage(Player player)
    {
        player.Damage(_attackPower);
    }
}
