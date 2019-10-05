using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;

    public static GameManager Instance { get; set; }

    //UI fields    //ep18/19       
    public TextMeshProUGUI ScoreText, CoinText, ModifierScoreText;
    public TextMeshProUGUI FinalScoreTxt, FinalCoinScoreTxt, HighScoreText;

    public Animator DeathMenuAnim, GameCanvas, MenuAnim, CoinAnim;

    public bool IsDead { get; set; }

    private CameraController _cameraController;
    private PlayerController _playerController;
    private GlacierSpawner _glacierSpawner;
    [HideInInspector]
    public bool _hasGameStarted = false;
    private float _score, _coins, _modifierScore;
    private int _lastScore;
    private string _highScoreKey = "HighScore";

    private void Awake()
    {
        Instance = this;
        _modifierScore = 1f;

        _playerController = FindObjectOfType<PlayerController>();
        _glacierSpawner = FindObjectOfType<GlacierSpawner>();
        _cameraController = FindObjectOfType<CameraController>();

        ScoreText.text = _score.ToString();
        CoinText.text = _coins.ToString();

        HighScoreText.text = PlayerPrefs.GetInt(_highScoreKey).ToString();
        //   ModifierScoreText.text = "X" + _modifierScore.ToString("0.0");
    }

    private void Update()
    {
        CheckIfGameHasStarted();
        _playerController.SetSwipeBools();
    }

    private void CheckIfGameHasStarted()
    {
        bool touchedScreen = (MobileInputs.Instance.Tap);
        bool gameHasNotStarted = (!_hasGameStarted);
        if (touchedScreen && gameHasNotStarted)
        {
            _hasGameStarted = true;
            _playerController.StartRunning();
            _glacierSpawner.IsScrolling = true;
            _cameraController.IsMoving = true;
            GameCanvas.SetTrigger("Show");
            MenuAnim.SetTrigger("Hide");

        }

        if (_hasGameStarted && !IsDead)
        {
            //Bump up the score
            _score += (Time.deltaTime * _modifierScore);
            if (_lastScore != (int)_score)
            {
                _lastScore = (int)_score;
                ScoreText.text = _score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        CoinAnim.SetTrigger("Collect");
        _coins++;
        CoinText.text = _coins.ToString("0");
        _score += COIN_SCORE_AMOUNT;
        CoinText.text = _coins.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        _modifierScore = 1 + modifierAmount;
        //   ModifierScoreText.text = "X" + _modifierScore.ToString("0.0");
    }

    public void OnPlayBtn()
    {
        SceneManager.LoadScene("Main_Game");
    }

    public void OnDeath()
    {
        IsDead = true;
        _glacierSpawner.IsScrolling = false;
        FinalScoreTxt.text = _score.ToString("0");
        FinalCoinScoreTxt.text = _coins.ToString("0");
        DeathMenuAnim.SetTrigger("Death");
        GameCanvas.SetTrigger("Hide");
        HighScore();
    }

    private void HighScore()
    {
        if (_score > PlayerPrefs.GetInt(_highScoreKey))
        {
            PlayerPrefs.SetInt(_highScoreKey, (int)_score);
        }
    }
}
