using System.Collections.Generic;
using UnityEngine;

public class GuestComment : MonoBehaviour
{
    private List<string> orderComments = new List<string>();
    private List<string> removeComments = new List<string>();
    private List<string> addComments = new List<string>();

    private List<string> goodComments = new List<string>();
    private List<string> commonComments = new List<string>();
    private List<string> badComments = new List<string>();

    public void SetAllComments()
    {
        if (orderComments.Count == 0)
        {
            SetOrderComments();
            SetRemoveComments();
            SetPlusComments();
            SetGoodComments();
            SetBadComments();
            SetCommonComments();
        }
    }

    private void SetOrderComments()
    {
        orderComments.Add("{0}이 땡기는 날이네요.");
        orderComments.Add("{0}은 감칠맛이 좋더라구요.");
        orderComments.Add("{0} 있나요?");
        orderComments.Add("세상 가장 위대한 {0}!");
        orderComments.Add("최고의 라면은 {0}이 아닐까요?");
        orderComments.Add("{0}.");
        orderComments.Add("{0} 줘요.");
        orderComments.Add("{0} 주시라고요.");
        orderComments.Add("{0} 달라니까요?");
        orderComments.Add("하하! {0}!");
        orderComments.Add("{0} 줘!!!!!!");
    }
    private void SetRemoveComments()
    {
        removeComments.Add(" {0} 빼주세요~");
        removeComments.Add(" {0} 말고요!!");
        removeComments.Add(" 그치만 {0} 싫어해요");
        removeComments.Add(" 저 {0} 싫어해요.");
        removeComments.Add(" 근데 {0}... 별로던데요");
        removeComments.Add(" 하지만 {0} 편식해요!");
    }
    private void SetPlusComments()
    {
        addComments.Add("도 추가요~");
        addComments.Add("추가요~");
        addComments.Add("도 넣어주실래요?");
        addComments.Add("도 넣어줘!!!!");
        addComments.Add("도 주세요!!!!");
        addComments.Add("도 줘!!!!");
        addComments.Add("도 주셔야하는 거 알죠?");
        addComments.Add("도 주세요~");
        addComments.Add("또한 필요합니다.");
        addComments.Add("도 필요해요!");
    }
    private void SetGoodComments()
    {
        goodComments.Add("너무 좋아요!!!");
        goodComments.Add("정말 완벽한 라면이야!");
        goodComments.Add("이 라면은 제 처음이자 마지막 라면이에요.");
        goodComments.Add("진.짜 맛.있.습.니.다");
        goodComments.Add("음~~ 맛있는데요?");
        goodComments.Add("결혼해줘요");
        goodComments.Add("제 전속 라면 요리사가 되어주세요.");
        goodComments.Add("쏘 딜리셔스~~");
        goodComments.Add("어메이징... 너무 맛있잖아요!!");
        goodComments.Add("맛있어!!!!!!");
    }
    private void SetBadComments()
    {
        badComments.Add("이게 뭐죠...?");
        badComments.Add("이 집!! 신고할거야!!!");
        badComments.Add("진짜 화나게 하지 마세요.");
        badComments.Add("이 집은 3일만에 접는다에 백만원 겁니다.");
        badComments.Add("에잇!! 이게 뭐야!!");
        badComments.Add("세살배기 애기가 더 잘 끓이겠네요.");
        badComments.Add("왜 장사하시는 건가요. 발닦고 주무세요.");
        badComments.Add("라면 요리는... 재능이 없으신가봐요...");
        badComments.Add("다신 안 와!!!");
        badComments.Add("으으으!!!!!!!!");
        badComments.Add("세상이 내일 멸망한다면, 저는 이 집을 신고하겠어요.");
    }
    private void SetCommonComments()
    {
        commonComments.Add("그냥 그러네용...");
        commonComments.Add("그냥 그래요.");
        commonComments.Add("뭐... 노력은 하셨군요.");
        commonComments.Add("하하... 파이팅...!");
        commonComments.Add("일주일 굶고 먹으면 맛있겠네요.");
        commonComments.Add("무난해요, 그냥.");
        commonComments.Add("심한 말은 하지 않는 편이지만... 이건...");
        commonComments.Add("다음엔 옆 집 갈래요!!");
        commonComments.Add("......");
    }

    public string GetOrderComments()
    {
        SetAllComments();
        return orderComments[Random.Range(0, orderComments.Count)];
    }
    public string GetAddComments()
    {
        SetAllComments();
        return addComments[Random.Range(0, addComments.Count)];
    }
    public string GetRemoveComments()
    {
        SetAllComments();
        return removeComments[Random.Range(0, removeComments.Count)];
    }
    public string GetGoodComments()
    {
        SetAllComments();
        return goodComments[Random.Range(0, goodComments.Count)];
    }
    public string GetBadComments()
    {
        SetAllComments();
        return badComments[Random.Range(0, badComments.Count)];
    }
    public string GetCommonComments()
    {
        SetAllComments();
        return commonComments[Random.Range(0, commonComments.Count)];
    }
}
