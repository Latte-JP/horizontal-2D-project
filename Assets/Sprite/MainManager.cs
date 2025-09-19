using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバー")]
    private GameObject _gameOverUI;
    private GameObject _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _ShowGameOver();
    }
    private void _ShowGameOver()
    {
        if (_player != null) return;
        _gameOverUI.SetActive(true);
    }
}
