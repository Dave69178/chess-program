using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotion : MonoBehaviour
{
    static GameObject whitePromotionMenu;
    static GameObject blackPromotionMenu;
    static GameObject promotionStore;

    private void Awake()
    {
        whitePromotionMenu = Resources.Load<GameObject>("Prefabs/PromotionMenu/WhitePromotion");
        blackPromotionMenu = Resources.Load<GameObject>("Prefabs/PromotionMenu/BlackPromotion");
        promotionStore = new GameObject("PromotionStore");
    }

    public static void ActivatePromotionMenu (int square)
    {
        (float x, float y) = PositionHelper.ArrayPositionToWorldPosition(square);
        if (square / 8 == 0)
        {
            GameObject promotionSprite = Instantiate(whitePromotionMenu, new Vector3(x, y - 3f), Quaternion.identity);
            promotionSprite.transform.SetParent(promotionStore.transform);
        }
        else
        {
            GameObject promotionSprite = Instantiate(blackPromotionMenu, new Vector3(x, y), Quaternion.identity);
            promotionSprite.transform.SetParent(promotionStore.transform);
        }
    }

    public static void DestroyPromotionMenu ()
    {
        foreach (Transform child in promotionStore.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
