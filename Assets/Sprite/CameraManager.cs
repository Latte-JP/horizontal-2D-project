using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Serialize Field: Unityエディタから値を設定できるようにする
    [SerializeField, Header("揺れ時間")]
    private float _shakeTime;
    [SerializeField, Header("揺れの大きさ")]
    private float _shakeMagnitude;
    // private変数: スクリプト内で使用される
    private Player _player; // プレイヤーオブジェクトへの参照
    // カメラの初期位置を保持する変数
    private Vector3 initPos;
    private float _shakeCount;// 揺れ時間のカウント用
    private int _currentplayerHP;// 現在のプレイヤーのHPを格納
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [System.Obsolete]
    void Start()
    {
        // シーン内のPlayer型のコンポーネントを検索して取得
        _player = FindObjectOfType<Player>();
        // プレイヤーの現在のHPを初期値として格納
        _currentplayerHP = _player.GetHP();
        // ゲーム開始時にカメラの現在位置を初期位置として記録
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCheck(); // カメラ揺れチェックメソッドを呼び出す
        _FollowPlayer();
    }
    // カメラ揺れチェックメソッド
    private void ShakeCheck()
    {
        // 現在のHPがプレイヤーの実際のHPと異なる場合（ダメージを受けた場合）
        if (_currentplayerHP != _player.GetHP())
        {
            // 現在のHPを更新して、次のフレームで再度コルーチンが開始されないようにする
            _currentplayerHP = _player.GetHP();
            // 揺れカウントをリセット
            _shakeCount = 0.0f;
            // カメラを揺らすコルーチンを開始
            StartCoroutine(_Shake());


        }
    }
    // カメラを揺らすコルーチン
    IEnumerator _Shake()
    {
        // カメラの初期位置を保存
        Vector3 initPos = transform.position;

        // シェイクカウントが指定されたシェイクタイムに達するまでループ
        while (_shakeCount < _shakeTime)
        {
            // カメラのX座標とY座標をランダムにずらす
            float x = initPos.x + Random.Range(-_shakeMagnitude, _shakeMagnitude);
            float y = initPos.y + Random.Range(-_shakeMagnitude, _shakeMagnitude);

            // カメラの新しい位置を設定
            transform.position = new Vector3(x, y, initPos.z);

            // シェイクカウントを時間に基づいて更新
            _shakeCount += Time.deltaTime;

            // 1フレーム待機（次のフレームで処理を再開）
            yield return null;
        }

        // 揺れが終わったらカメラの位置を初期位置に戻す
        transform.position = initPos;
    }
        private void _FollowPlayer()
    {
        if (_player == null) return;
        // プレイヤーのX座標を取得
        float x = _player.transform.position.x;

        // Mathf.ClampでカメラのX座標に制限を設ける
        // 第1引数: 変更したい値 (プレイヤーのX座標)
        // 第2引数: 最小値 (カメラの初期X座標)
        // 第3引数: 最大値 (無限大 - 右方向には制限なし)
        x = Mathf.Clamp(x, initPos.x, Mathf.Infinity);

        // カメラの新しい位置を設定
        // X座標はプレイヤーに追従し、制限を適用
        // Y座標とZ座標は初期位置を保持
        transform.position = new Vector3(x, initPos.y, initPos.z);
    }
}
