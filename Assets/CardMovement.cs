
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originLocalCursorPosition;
    private Vector3 originLocalPanelPosition;
    private int currState = 0;
    private Quaternion origCardRotation;
    private Vector3 origCardPosition;
    private Vector3 origCardScale;

    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private Vector3 playPosition;
    // a highlight effect
    [SerializeField] private GameObject hoverHighlight;
    // the arrow that indicates where the card's effect will be placed
    [SerializeField] private GameObject playArrow;

    [SerializeField] private Vector2 cardPlayZone;
    // Linear interpolation amount
    [SerializeField] private float lerpTime = 0.1f;


    void Awake() {
      rectTransform = GetComponent<RectTransform>();
      canvas = GetComponentInParent<Canvas>();
      // store the original transform of the card
      origCardScale = rectTransform.localScale;
      origCardPosition = rectTransform.localPosition;
      origCardRotation = rectTransform.localRotation;
    }
    // Start is called before the first frame update
    void Start() {
        hoverHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(Input.mousePosition.y);
        switch (currState) {
          case 1:
            HandleHoverState();
            break;
          case 2:
            HandleDragState();
            // if left click is released
            if (!Input.GetMouseButton(0)) {
              GoToState(0);
            }
            break;
          case 3:
            HandlePlayState();
            if (!Input.GetMouseButton(0)) {
              GoToState(0);
            }
            break;
        }
    }

    private void GoToState(int desiredState) {
      // default state
      if (desiredState == 0) {
        currState = 0;
        rectTransform.localPosition = origCardPosition;
        rectTransform.localRotation = origCardRotation;
        rectTransform.localScale = origCardScale;
        hoverHighlight.SetActive(false);
        playArrow.SetActive(false);
      } else if (desiredState == 1) {
        // necessary??
        origCardScale = rectTransform.localScale;
        origCardPosition = rectTransform.localPosition;
        origCardRotation = rectTransform.localRotation;
        currState = 1;
      } else if (desiredState == 2) {
        currState = 2;
      } else if (desiredState == 3) {
        currState = 3;
        playArrow.SetActive(true);
        rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpTime);
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      if (currState == 0) {
        GoToState(1);
      }
    }

    public void OnPointerExit(PointerEventData eventData) {
      if (currState == 1) {
        GoToState(0);
      }
    }

    public void OnPointerDown(PointerEventData eventData) {
      if (currState == 1) {
        GoToState(2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
            eventData.position, eventData.pressEventCamera, out originLocalCursorPosition);
        originLocalPanelPosition = rectTransform.localPosition;
      }
    }

    public void OnDrag(PointerEventData eventData) {
      if (currState == 2) {
        // ensures card follows mouse
        // Vector2 localCursorPosition;
        // if (rectTransform.localPosition.y > cardPlay.y) {
        //          GoToState(3);
        //       }
        // if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
        //     eventData.position, eventData.pressEventCamera, out localCursorPosition)) {
        //       localCursorPosition /= canvas.scaleFactor;
        //       Vector3 offsetToOriginal = localCursorPosition - originLocalCursorPosition;
        //       rectTransform.localPosition = originLocalPanelPosition + offsetToOriginal;
        //       if (rectTransform.localPosition.y > cardPlay.y) {
        //          GoToState(3);
        //       }
        //     }

        // I got no clue why I have to subtract 600 but otherwise its weirdly offset
        rectTransform.localPosition = Input.mousePosition / canvas.scaleFactor - new Vector3(600,0,0);
        if (rectTransform.localPosition.y > cardPlayZone.y) {
          GoToState(3);
        }
      }
    }

    private void HandleHoverState() {
      // make it glow and enlarge
      hoverHighlight.SetActive(true);
      rectTransform.localScale =  origCardScale * hoverScale;
      // rectTransform.localPosition.Set(rectTransform.localPosition.x, rectTransform.localPosition.y, rectTransform.localPosition.z + 1);
    }

    private void HandleDragState() {
      // remove card's rotation
      rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState() {
      rectTransform.localPosition = playPosition;
      rectTransform.localRotation = Quaternion.identity;
      if (Input.mousePosition.y / canvas.scaleFactor < cardPlayZone.y) {
        GoToState(2);
        playArrow.SetActive(false);
      }
    }
}
