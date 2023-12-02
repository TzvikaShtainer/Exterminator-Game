using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform backgroundTrans;
    [SerializeField] private RectTransform thumbStickTrans;
    [SerializeField] private RectTransform centerTrans;

    private bool bWasDragging;
    
    public delegate void OnStickInputValueUpdated(Vector2 inputVal);
    public delegate void OnStickTaped();

    public event OnStickInputValueUpdated oStickValueUpdated; //make an event that fire with the vector2 for moving
    public event OnStickTaped onStickTaped;
    
    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"OnDrag{eventData.position}");
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = backgroundTrans.position;

        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, backgroundTrans.sizeDelta.x/2); //make the joystick limits

        thumbStickTrans.position = centerPos + localOffset;

        Vector2 inputVal = localOffset / (backgroundTrans.sizeDelta.x / 2); //dont get the math but we need to do that
        oStickValueUpdated?.Invoke(inputVal);

        bWasDragging = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTrans.position = eventData.position;
        thumbStickTrans.position = eventData.position;

        bWasDragging = false;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        backgroundTrans.position = centerTrans.position;
        thumbStickTrans.position = backgroundTrans.position;
        
        oStickValueUpdated?.Invoke(Vector2.zero);

        if (!bWasDragging)
        {
            onStickTaped?.Invoke();
        }

    }
}
