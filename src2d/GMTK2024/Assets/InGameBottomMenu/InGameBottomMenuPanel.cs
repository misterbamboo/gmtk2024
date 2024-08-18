using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameBottomMenuPanel : MonoBehaviour
{
    [SerializeField] Image[] thumbnails;
    [SerializeField]
    [Range(0, 1)]
    float thumbnailsFadeZoneInPercentage = 0.25f;

    [SerializeField] float menuSmallWidth = 334f;
    [SerializeField] float menuBigWidth = 869f;

    [SerializeField]
    [Range(0, 1)]
    float progress = 0f;
    float targetProgress = 0f;

    [SerializeField] float progressAnimDuration = 0.5f;

    [SerializeField] bool connectProgressToOpenState = true;
    [SerializeField] bool open = false;

    [SerializeField] RectTransform menuContainer;
    [SerializeField] RectTransform maskContainer;

    [SerializeField] LadderOptions ladderOptions;

    private List<LadderOption> items = new List<LadderOption>();

    private void Start()
    {
        progress = open ? 1 : 0;
        RefillItems(0);
    }

    private void OnDrawGizmos()
    {
        UpdateProgress(0);
        UpdateContainerWidth();
        UpdateMaskContainerWidth();
        UpdateItemThumbnails();
    }

    private void Update()
    {
        UpdateProgress(progressAnimDuration);
        UpdateContainerWidth();
        UpdateMaskContainerWidth();
        UpdateItemThumbnails();
    }

    private void UpdateProgress(float duration)
    {
        if (connectProgressToOpenState)
        {
            if (duration <= 0)
            {
                progress = open ? 1 : 0;
            }
            else
            {
                targetProgress = open ? 1 : 0;
                if (targetProgress != progress)
                {
                    progress += progress < targetProgress ? Time.deltaTime / duration : -Time.deltaTime / duration;
                    progress = Mathf.Clamp01(progress);
                }
            }
        }
    }

    private void UpdateContainerWidth()
    {
        if (menuContainer)
        {
            var rect = menuContainer.rect;
            rect.width = Mathf.Lerp(menuSmallWidth, menuBigWidth, progress);
            menuContainer.sizeDelta = new Vector2(rect.width, 0);
        }
    }

    private void UpdateMaskContainerWidth()
    {
        if (maskContainer)
        {
            var rect = maskContainer.rect;
            rect.width = Mathf.Lerp(menuSmallWidth, menuBigWidth, progress);
            maskContainer.sizeDelta = new Vector2(rect.width, rect.height);
        }
    }

    private void UpdateItemThumbnails()
    {
        var multiplicator = 1f / Mathf.Clamp(thumbnailsFadeZoneInPercentage, 0.001f, 1f);

        var t = 1f - Mathf.Clamp01((1f - progress) * multiplicator);
        var phaseTDuration = t / thumbnails.Length;
        var phaseSize = 1f / thumbnails.Length;
        for (float i = 0; i < thumbnails.Length; i++)
        {
            var beginPhase = i * phaseTDuration;
            var endPhase = (i + 1) * phaseTDuration;
            var thumblainT = (Mathf.Clamp(t, beginPhase, endPhase) - beginPhase) / phaseSize;
            var color = thumbnails[(int)i].color;
            color.a = thumblainT;
            thumbnails[(int)i].color = color;
        }
    }

    private void RefillItems(int index)
    {
        while (items.Count < thumbnails.Length)
        {
            var randIndex = UnityEngine.Random.Range(0, ladderOptions.ladderOptions.Length);
            var option = ladderOptions.ladderOptions[randIndex];
            items.Insert(index, option);
            RefreshThumbnails();
        }
    }

    private void RefreshThumbnails()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (i < thumbnails.Length)
            {
                thumbnails[i].sprite = items[i].spriteRenderer;
            }
        }
    }

    public void Player_ClickReset()
    {
        var ladders = allBuiltLadders.ToArray();
        foreach (var ladder in ladders)
        {
            Destroy(ladder);
        }
        allBuiltLadders.Clear();
    }

    public void Player_ClickBuild()
    {
        this.open = true;
        GameEvents.Raise(GameEvents.OnBuildModeActiveChanged, this.open);
    }

    public void Player_ClickPlay()
    {
        this.open = false;
        GameEvents.Raise(GameEvents.OnBuildModeActiveChanged, this.open);
    }

    List<GameObject> allBuiltLadders = new List<GameObject>();
    public void Player_ClickItem(int index)
    {
        bool canBuild = open && progress > 0.95f;
        if (!canBuild) return;

        var item = items[index];
        var pos = Camera.main.transform.position;
        pos.z = 0;
        var ladder = Instantiate(item.prefab, pos, Quaternion.identity, null);
        allBuiltLadders.Add(ladder);

        items.RemoveAt(index);
        RefillItems(index);
    }
}
