using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // インスペクターで設定する敵のプレハブ
    [SerializeField, Header("敵オブジェクト")]
    private GameObject _enemy;
    // ゲーム内で自動的に取得するプレイヤーオブジェクト
    private Player _player;
    private GameObject _enemyObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        // シーン内のPlayerスクリプトを持つオブジェクトを検索
        _player = FindObjectOfType<Player>();
        // 敵の生成フラグを初期化
        _enemyObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        // 敵の生成ロジックを毎フレーム実行
        _SpawnEnemy();
    }
    private void _SpawnEnemy()
    {
        // プレイヤーが存在しない場合は処理を中断
        if (_player == null) return;
        // プレイヤーの位置を取得
        Vector3 playerPos = _player.transform.position;

        // カメラの画面右上のワールド座標を取得
        Vector3 cameraMaxPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        // 敵プレハブのスケール（大きさ）を取得
        Vector3 scale = _enemy.transform.localScale;

        // EnemySpawnerとプレイヤーの水平距離を計算
        float distance = Vector2.Distance(transform.position, new Vector2(playerPos.x, transform.position.y));

        // プレイヤーと画面右端の水平距離を計算（敵のサイズ分を考慮）
        float spawnDis = Vector2.Distance(new Vector2(playerPos.x, playerPos.y), new Vector2(cameraMaxPos.x + scale.x / 2, playerPos.y));

        // 敵の生成条件：
        // 1. EnemySpawnerとプレイヤーの距離が、プレイヤーと画面端の距離より短い
        // 2. まだ敵が生成されていない
        if (distance < spawnDis && _enemyObj == null)
        {
            _enemyObj = Instantiate(_enemy);
            _enemyObj.transform.position = transform.position;
            transform.parent = _enemyObj.transform;
        }
    }


}
