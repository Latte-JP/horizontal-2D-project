using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // 列挙型（enum）の定義：オブジェクトの状態を表す
    private enum Mode
    {
        None,        // 初期状態（カメラに映っていない）
        Render,     // カメラに映っている
        RenderOut  // カメラに映っていたが、今は映っていない
    }
    // 現在のオブジェクトの状態を格納するMode型の変数
    private Mode _mode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 初期状態をNoneに設定
        _mode = Mode.None;
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトの消滅処理をチェック
        _Dead();
    }
    // OnWillRenderObjectメソッド：オブジェクトがカメラに描画される直前に呼ばれるUnityのイベントメソッド
    private void OnWillRenderObject()
    {
        // 現在のカメラが「Main Camera」である場合にのみ処理を実行
        if (Camera.current.name == "Main Camera")
        {
            // オブジェクトがメインカメラに映っている場合、モードをRenderに変更
            _mode = Mode.Render;
        }
    }
      // オブジェクトの消滅処理を行うメソッド
    private void _Dead()
    {
        // モードがRenderOutの場合（カメラから外れた状態）
        if (_mode == Mode.RenderOut)
        {
            // このゲームオブジェクトを消滅させる
            Destroy(gameObject);
        }
        // モードがRenderの場合（カメラに映っている状態）
        else if (_mode == Mode.Render)
        {
            // 次のフレームでカメラから外れたかをチェックするため、モードをRenderOutに変更
            _mode = Mode.RenderOut;
        }
    }
    

}
