using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // プレイヤーの手札
    [SerializeField] Transform playerHand;
    // プレイヤーのフィールド
    [SerializeField] Transform playerField;
    // 敵の手札
    [SerializeField] Transform enemyHand;
    // 敵のフィールド
    [SerializeField] Transform enemyField;
    // カードの取得
    [SerializeField] CardController cardPrefab;
    // 結果画面の取得
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;
    // Timerの取得
    [SerializeField] Text timeCountText;
    int timeCount;

    // プレイヤーターンの判定
    bool isPlayerTurn;
    // プレイヤーヒーロ
    int playerHeroHp;
    // 敵ヒーロ
    int enemyHeroHp;
    // プレイヤーヒーロのポジション
    [SerializeField] Transform playerHeroPosition;
    // プレイヤーヒーロHpテキスト
    [SerializeField] Text playerHeroText;
    // 敵ヒーロHpテキスト
    [SerializeField] Text enemyHeroText;
    // プレイヤーマナ
    public int playerManaCost;
    // 敵マナ
    public int enemyManaCost;
    // マナのトータル値
    int playerDefaultManaCost;
    int enemyDefaultManaCost;
    // プレイヤーマナテキスト
    [SerializeField] Text playerManaText;
    // 敵マナテキスト
    [SerializeField] Text enemyManaText;
    //List<int> playerDeck = new List<int>() {3,1,4,2,5,7,6,8};
    //List<int> enemyDeck = new List<int>() {2,1,3,5,4,6,7,8};
    List<int> playerDeck = new List<int>();
    List<int> enemyDeck = new List<int>();
    // シングルトン化 どこからでもアクセスできるように
    public static GameManager instance;
    int playerNum;
    int enemyNum;

    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        System.Random randomNum = new System.Random();
        // 適当なデッキを作成
        for (int i = 0; i < 8; i++)
        {
            playerNum = randomNum.Next(8) + 1;
            playerDeck.Add(playerNum);
            enemyNum = randomNum.Next(8) + 1;
            enemyDeck.Add(enemyNum);
        }
        StartGame();
        isPlayerTurn = true;
        TurnCalc();

    }

    void StartGame()
    {
        resultPanel.SetActive(false);
        playerHeroHp = 30;
        enemyHeroHp = 30;
        playerManaCost = 1;
        enemyManaCost = 1;
        playerDefaultManaCost = 1;
        enemyDefaultManaCost = 1;
        ShowHeroHp();
        ShowManaCost();
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();
    }

    void ShowManaCost()
    {
        playerManaText.text = playerManaCost.ToString();
        enemyManaText.text = enemyManaCost.ToString();
    }

    public void ReduceManaCost(int cost, bool isPlayer)
    {
        if(isPlayer)
        {
            playerManaCost -= cost;
        }
        else
        {
            enemyManaCost -= cost;
        }
        ShowManaCost();
    }


    public void Restart()
    {
        // プレイヤーの手札のリセット
        foreach (Transform card in playerHand)
        {
            Destroy(card.gameObject);
        } 
        // 敵の手札のリセット
        foreach (Transform card in enemyHand)
        {
            Destroy(card.gameObject);
        }
        // プレイヤーのフィールドののリセット
        foreach (Transform card in playerField)
        {
            Destroy(card.gameObject);
        }
        // 敵のフィールドのリセット
        foreach (Transform card in enemyField)
        {
            Destroy(card.gameObject);
        }
        // デッキのリセット
        playerDeck = new List<int>() { 3, 1, 2, 2, 4 };
        enemyDeck = new List<int>() { 2, 1, 3, 1, 4 };
        // ゲームのリスタート
        StartGame();
    }

    void SettingInitHand()
    {
        // カード生成処理の実施
        // プレイヤーにカードを3まいずつ配る
        for (int i = 0; i < 3; i++)
        {
            GiveCardToHand(playerDeck, playerHand, true);
            GiveCardToHand(enemyDeck, enemyHand, false);
        }
    }

    void GiveCardToHand(List<int> deck, Transform hand, bool player)
    {
        if (deck.Count == 0)
        {
            return;
        }

        int cardId = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardId, hand, player);
    }

    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 20;
        timeCountText.text = timeCount.ToString();
        while(timeCount > 0)
        {
            // 1秒たったらカウントを減らす
            yield return new WaitForSeconds(1);
            timeCount--;
            timeCountText.text = timeCount.ToString();
        }
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        // ターンが切り替わるタイミングで一度全てのカードを攻撃不可状態にする
        // 自分フィールドカード
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);
        // 敵フィールドカード
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);
        isPlayerTurn = !isPlayerTurn;
        if(isPlayerTurn)
        {
            // マナコストを1増やす
            playerDefaultManaCost++;
            playerManaCost = playerDefaultManaCost;
            // デッキからカードをドロー処理の実施
            GiveCardToHand(playerDeck, playerHand, isPlayerTurn);
        }
        else
        {
            // マナコストを1増やす
            enemyDefaultManaCost++;
            enemyManaCost = enemyDefaultManaCost;
            GiveCardToHand(enemyDeck, enemyHand, isPlayerTurn);

        }
        ShowManaCost();
        TurnCalc();
    }

    // カード生成処理
    void CreateCard(int cardId,Transform hand, bool isplayer)
    {
        // 敵のカードの向きを180°反転させる
        if (!isplayer)
        {
            cardPrefab.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180);
        }
        else
        {
            cardPrefab.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0);
        }

        CardController card = Instantiate(cardPrefab, hand, false);
        if (hand.name == "PlayerHand")
        {
            card.Init(cardId, true);
        }
        else
        {
            card.Init(cardId, false);
        }

    }
    void SettingCanAttackView(CardController[] fieldCardList, bool isCanAttack)
    {
        foreach (CardController card in fieldCardList)
        {
            // cardを攻撃可能にする
            card.SetCanAttack(isCanAttack);
        }
    }


    void PlayerTurn()
    {
        // フィールドのカードを攻撃可能にする
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();

        SettingCanAttackView(playerFieldCardList, true);
    }

    // 敵の処理
    IEnumerator EnemyTurn()
    {
        // フィールドのカードを攻撃可能にする
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);

        /* 場にカードをだす */
        // 手札のカードを取得
        CardController[] handCardList = enemyHand.GetComponentsInChildren<CardController>();
        // マナコスト以下のカードを召喚し続ける
        while(Array.Exists(handCardList, card => card.cardModel.cost <= enemyManaCost))
        {
            // 配列の中から条件に合うものを取得する
            CardController[] selectableHandCostCardList = Array.FindAll(handCardList, card => card.cardModel.cost <= enemyManaCost);
            // 場に出すカードを取得
            CardController enemyCard = selectableHandCostCardList[0];
            // カードを移動
            StartCoroutine(enemyCard.cardMovement.MoveToField(enemyField));
            enemyCard.OnField(false);
            handCardList = enemyHand.GetComponentsInChildren<CardController>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        /* Playerに攻撃する */
        // フィールドのカードリストを取得
        CardController[] fieldCardList = enemyField.GetComponentsInChildren<CardController>();
        // 攻撃カードがあれば可能な限り攻撃する
        while(Array.Exists(fieldCardList, card => card.cardModel.canAttack))
        {
            // 攻撃可能なカードの取得
            CardController[] enemyCanAttackCardList = Array.FindAll(fieldCardList, card => card.cardModel.canAttack);
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();

            // アタックカードの選択
            CardController attacker = enemyCanAttackCardList[0];
            if (playerFieldCardList.Length > 0)
            {
                // 守備カードを選択
                CardController defender = playerFieldCardList[0];
                // 戦いの実施
                StartCoroutine(attacker.cardMovement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.51f);
                CardsBattle(attacker, defender);
            }
            else
            {
                StartCoroutine(attacker.cardMovement.MoveToTarget(playerHeroPosition));
                yield return new WaitForSeconds(0.25f);
                AttackToHero(attacker, false);
                yield return new WaitForSeconds(0.25f);
                CheckHeroHp();
            }
            yield return new WaitForSeconds(1);
        }

        fieldCardList = enemyField.GetComponentsInChildren<CardController>();
        yield return new WaitForSeconds(1);

        ChangeTurn();
    }

    public void CardsBattle(CardController attacker, CardController defender)
    {
        // 攻撃の実施
        attacker.Attack(defender);
        defender.Attack(attacker);

        // キャラクタのHPチェック実施
        attacker.CheckAlive();
        defender.CheckAlive();
    }

    void ShowHeroHp()
    {
        playerHeroText.text = playerHeroHp.ToString();
        enemyHeroText.text = enemyHeroHp.ToString();
    }

    public void AttackToHero(CardController attacker, bool isPlayerCard)
    {
        if(isPlayerCard)
        {
            // 敵ヒーロを攻撃する
            enemyHeroHp -= attacker.cardModel.at;
        }
        else
        {
            // 敵にヒーロが攻撃される
            playerHeroHp -= attacker.cardModel.at;
        }
        attacker.SetCanAttack(false);
        ShowHeroHp();
    }

    public void CheckHeroHp()
    {
        if(playerHeroHp <= 0 || enemyHeroHp <= 0)
        {
            if (playerHeroHp <= 0)
            {
                resultText.text = "あなたの負け";
            }
            if (enemyHeroHp <= 0)
            {
                resultText.text = "あなたの勝ち";
            }
            ShowResultPanel();
        }
    }

    void ShowResultPanel()
    {
        StopAllCoroutines();
        resultPanel.SetActive(true);
    }
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }
    }
}
