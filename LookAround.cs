using UnityEngine;
using UnityEngine.EventSystems;

public class LookAround : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 touchStartPos;
    private Vector2 touchDelta;
    private bool isTouching = false;

    public float sensitivity = 2.0f;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private void Update()
    {
        if (isTouching)
        {
            Horizontal = touchDelta.x * sensitivity * Time.deltaTime;
            Vertical = touchDelta.y * sensitivity * Time.deltaTime;

            // Apply the horizontalDelta and verticalDelta to player's look
            // Example:
            // transform.Rotate(Vector3.up * horizontalDelta);
            // transform.Rotate(Vector3.right * -verticalDelta);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchStartPos = eventData.position;
        isTouching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchDelta = eventData.position - touchStartPos;
        touchStartPos = eventData.position;
    }
}
