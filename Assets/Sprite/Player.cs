using System.Net.Http.Headers;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour

{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private int _hp;
    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private bool _bJump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _bJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        //Unity上にhpを表示する。敵に当たってhpが減ることの確認用
        Debug.Log(_hp);
    }
    private void _Move()
    {
        _rigid.linearVelocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.linearVelocity.y);
        _anim.SetBool("Walk", _inputDirection.x != 0.0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _bJump = false;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
        }
    }

    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;

        // Playerの足の下の座標がEnemyの頭の上の座標より高い場合（-0.1fは足が頭に少しめり込んだ時を想定して補正している）Enemyを消滅
        if (transform.position.y - (halfScaleY - 0.01f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.01f))
        {
            Destroy(enemy);
            // 敵を踏んだ時に跳ねる処理を追加
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        else
        {
            //enemyオブジェクトのEnemyスクリプトの中のplayerDamageメソッドが呼び出されて、引数の中は自分（Playerクラス）が入っていてPlayerクラスのhpが呼び出され当たった敵の攻撃力分減る。
            enemy.GetComponent<Enemy>().PlayerDamage(this);
        }
    }
    private void _Dead()
    {
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void _OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void _OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || _bJump) return;
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        _bJump = true;
    }
    public void Damage(int damage)
    {
        //hp-damageと0を比較して大きい方をhpに代入する。
        _hp = Mathf.Max(_hp - damage, 0);
        _Dead();
    }
    public int GetHP()
    {
        return _hp;
    }

}