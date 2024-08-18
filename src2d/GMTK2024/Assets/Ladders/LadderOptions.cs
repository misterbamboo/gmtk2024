using UnityEngine;

public class LadderOptions : MonoBehaviour
{
    [SerializeField] public LadderOption[] ladderOptions;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class LadderOption
{
    public Sprite spriteRenderer;
    public GameObject prefab;
}