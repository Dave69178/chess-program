using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromotionInputManager : MonoBehaviour
{
    public delegate void SquareClick(int i);
    public static event SquareClick OnPromotionSquareClick;

    private void Start()
    {
        GetComponent<PromotionInputManager>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("promotionSquare"))
                {
                    OnPromotionSquareClick?.Invoke(int.Parse(hit.collider.name));
                }
            }
        }
    }
}
