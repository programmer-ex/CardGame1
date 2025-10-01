using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDirector : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabSpades;
    [SerializeField] List<GameObject> prefabClubs;
    [SerializeField] List<GameObject> prefabDiamonds;
    [SerializeField] List<GameObject> prefabHearts;
    [SerializeField] List<GameObject> prefabJokers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //シャッフルしたカードを返す
    public List<CardController> GetShuffleCards()
    {
        List<CardController> ret = new List<CardController>();

        ret.AddRange(createCards(SuitType.Spade));
        ret.AddRange(createCards(SuitType.Club));
        ret.AddRange(createCards(SuitType.Diamond));
        ret.AddRange(createCards(SuitType.Heart));

        ShuffleCards(ret);

        return ret;
    }
    //神経衰弱で使うカードを返す
    public List<CardController> GetMemoryCards()
    {
        List<CardController> ret = new List<CardController>();

        ret.AddRange(createCards(SuitType.Spade, 10));
        ret.AddRange(createCards(SuitType.Diamond, 10));

        ShuffleCards(ret);

        return ret;
    }    //シャッフル
    public void ShuffleCards(List<CardController> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rnd = Random.Range(0, cards.Count);
            CardController tmp = cards[i];

            cards[i] = cards[rnd];
            cards[rnd] = tmp;
        }
    }

    //カード作成
    List<CardController> createCards(SuitType suittype, int count = -1)
    {
        List<CardController> ret = new List<CardController>();

        //カードの種類（デフォルト）
        List<GameObject> prefabcards = prefabSpades;
        Color suitcolor = Color.black;

        if (SuitType.Club == suittype)
        {
            prefabcards = prefabClubs;
        }
        else if (SuitType.Diamond == suittype)
        {
            prefabcards = prefabDiamonds;
            suitcolor = Color.red;
        }
        else if (SuitType.Heart == suittype)
        {
            prefabcards = prefabHearts;
            suitcolor = Color.red;
        }
        else if (SuitType.Joker == suittype)
        {
            prefabcards = prefabJokers;
        }
        //枚数に指定がなければ全てのカードを作成する
        if (0 > count)
        {
            count = prefabcards.Count;
        }

        //カード生成
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabcards[i]);

            //当たり判定追加
            BoxCollider bc = obj.AddComponent<BoxCollider>();
            //当たり判定検知用
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            //カード同士の当たり判定と物理演算を使わない
            bc.isTrigger = true;
            rb.isKinematic = true;

            //カードにデータをセット
            CardController ctrl = obj.AddComponent<CardController>();

            ctrl.Suit = suittype;
            ctrl.SuitColor = suitcolor;
            ctrl.PlayerNo = -1;
            ctrl.No = i + 1;

            ret.Add(ctrl);
        }

        return ret;

    }

}
/*
List<CardController>
:CardControllerクラスで定義したものを使えるリスト
CardControllerクラスには、Suit（マーク）とかNo（番号）とかFlipCard()（めくる処理）とかが定義されてるよな？
List<CardController>っていうのは、**その全部の機能やデータを持った「カード1枚分のオブジェクト」**をまとめて管理するためのリスト。
だから、そのリストから取り出した1枚はこういう風に使える👇
cards[0].Suit = SuitType.Heart;   // 0番目のカードのマークをハートにする
cards[0].FlipCard(true);          // 0番目のカードを表にする

このスクリプトでやってること
このコードはまず createCards 関数で、指定されたマーク（SuitType）に応じて、対応するカードPrefabを複製して CardController 型のリスト ret を作り返す。
その後、このリストを ShuffleCards 関数に渡すことで、同じカードが重複しないよう順番をランダムに並べ替え


ShuffleCards関数に渡せてる？全部がうまいこと機能してるか疑問
*/
