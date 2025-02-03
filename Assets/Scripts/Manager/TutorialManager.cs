using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Wall")]
    public GameObject wallPrefab;

    public void ShowOffWall()
    {
        wallPrefab.transform.DOMoveY(-5.5f, 3);
    }
}
