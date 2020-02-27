using UnityEngine;

public class ImportantDoNotTouch : MonoBehaviour
{
    [SerializeField] private Shop shop = null;

    private void Awake()
    {
        fileUtility._shop = shop;
    }
}
