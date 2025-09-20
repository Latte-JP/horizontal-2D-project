using System.Net.Http.Headers;
using Unity.Burst.CompilerServices;
using System.Collections;
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
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private bool _bJump;
    // プレイヤーのスプライトレンダラー
    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _bJump = false;
        //スプライトレンダラーコンポーネントを取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
    private void _LookMoveDirec()
    {
        if (_inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _bJump = false;
            _anim.SetBool("Jump", _bJump);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
            //プレイヤーのレイヤーを「PlayerDamage」に変更
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
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
    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for (int i = 0; i < _damageTime; i++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }
        _spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
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
        _anim.SetBool("Jump", _bJump);
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