using System;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバーUI")]
    private GameObject _gameOverUI;
    [SerializeField, Header("ゲームクリアUI")]
    private GameObject _gameClearUI;
    private GameObject _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Obsolete]
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _ShowGameOverUI();
    }
    private void _ShowGameOverUI()
    {
        if (_player != null) return;
        _gameOverUI.SetActive(true);
    }
    public void _ShowGameClearUI()
    {
        _gameClearUI.SetActive(true);
    }
}
