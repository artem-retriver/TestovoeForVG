using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject wallPrefab;

    public void ShowOffWall()
    {
        wallPrefab.transform.DOMoveY(-5.5f, 3);
    }
}
