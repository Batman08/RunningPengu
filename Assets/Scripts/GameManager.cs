using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;

    public static GameManager Instance { get; set; }

    //UI fields
    public TextMeshProUGUI ScoreText, CoinText, ModifierScoreText;

    private PlayerController _playerController;
    [HideInInspector]
    public bool _hasGameStarted = false;
    private float _score, _coins, _modifierScore;
    private int _lastScore;

    private void Awake()
    {
        Instance = this;
        _modifierScore = 1f;
        _playerController = FindObjectOfType<PlayerController>();

        ScoreText.text = _score.ToString();
        CoinText.text = _coins.ToString();
        ModifierScoreText.text = "X" + _modifierScore.ToString("0.0");
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

        }

        if (_hasGameStarted)
        {
            //Bumpo up the score
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
        _coins++;
        CoinText.text = _coins.ToString("0");
        _score += COIN_SCORE_AMOUNT;
        CoinText.text = _coins.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        _modifierScore = 1 + modifierAmount;
        ModifierScoreText.text = "X" + _modifierScore.ToString("0.0");
    }
}
