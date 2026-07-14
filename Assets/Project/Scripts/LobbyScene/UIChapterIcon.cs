using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject lockedIcon;
    [SerializeField]
    private Image imageChapter;
    [SerializeField]
    private TextMeshProUGUI textChapterName;
    [SerializeField]
    private TextMeshProUGUI textStage;

    public void Setup(int index, ChapterData chapterData)
    {
        lockedIcon.SetActive(!Database.DBItem.chapters[index].isUnlock);

        imageChapter.sprite = chapterData.ChapterDataTable.spriteChapter;
        // Debug.. 챕터 이미지가 모두 있을 땐 색상 정보는 필요 없음
        imageChapter.color = chapterData.ChapterDataTable.colorChapter;
        textChapterName.text = $"#{index+1:D2} {chapterData.ChapterDataTable.chapterName}";
        textStage.text = $"스테이지 {Database.DBItem.chapters[index].bestStage}/{chapterData.StageDataTable.maxStage}";
    }
}
