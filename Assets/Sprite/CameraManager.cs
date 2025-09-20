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
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCheck(); // カメラ揺れチェックメソッドを呼び出す
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
}
