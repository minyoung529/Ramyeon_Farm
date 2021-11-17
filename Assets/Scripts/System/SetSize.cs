using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSize : MonoBehaviour
{
    enum PositionType
    {
        x,
        y,
        xy,
        sliceX
    }

    [SerializeField] PositionType type;

    void Start()
    {
        if (type == PositionType.x)
        {
            transform.localScale = new Vector2(GameManager.Instance.UIManager.DistanceX(), transform.localScale.y);
        }

        else if (type == PositionType.y)
        {
            transform.localScale = new Vector2(transform.localScale.x, GameManager.Instance.UIManager.DistanceY());
        }

        else if (type == PositionType.xy)
        {
            transform.localScale = new Vector2(GameManager.Instance.UIManager.DistanceX(), GameManager.Instance.UIManager.DistanceY());
        }

        else if (type == PositionType.sliceX)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.size = new Vector2(GameManager.Instance.UIManager.DistanceX(), spriteRenderer.size.y);
        }
    }
}
