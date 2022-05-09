using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareInputManager : MonoBehaviour
{
    public delegate void SquareClick(int i);
    public static event SquareClick OnSquareClick;
    public static event SquareClick OnPromotionSquareClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("square"))
                {
                    OnSquareClick?.Invoke(int.Parse(hit.collider.name));  //  Send message with index of square that was clicked
                }
            }
        }
    }
}
