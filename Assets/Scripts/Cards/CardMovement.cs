using UnityEngine;
using UnityEngine.EventSystems;
using EnemyAndTowers;
using System.Collections;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool DEBUG_MODE;
    private RectTransform rectTransform;
    private Canvas canvas;
    private int currState = 0;
    public Quaternion origCardRotation;
    public Vector3 origCardPosition;
    public Vector3 origCardScale;
    public CardPlaystyle playstyle;

    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private Vector3 playPosition;
    // a highlight effect
    [SerializeField] private GameObject hoverHighlight;
    // the arrow that indicates where the card's effect will be placed
    [SerializeField] private GameObject playArrow;

    [SerializeField] private Vector2 cardPlayZone;
    // Linear interpolation amount
    [SerializeField] private float lerpTime = 0.1f;
    [SerializeField] private ErrorManager errorManager;


    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        errorManager = FindObjectOfType<ErrorManager>();
        // store the original transform of the card
        origCardScale = rectTransform.localScale;
        origCardPosition = rectTransform.localPosition;
        origCardRotation = rectTransform.localRotation;
        hoverHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        TowerSelector towerSelector = FindObjectOfType<TowerSelector>();
        // state -1 = Game Stop
        // state 0 = Not-hovering
        // state 1 = Hovering
        // state 2 = Dragging
        // state 3 = Playing
        // state 4 = Rotating
    switch (currState)
    {
        case 1:
            HandleHoverState();
            break;
        case 2:
            towerSelector.highlightTileMode = true;
            HandleDragState();
            if (!Input.GetMouseButton(0) && playstyle == CardPlaystyle.Dragging) {
                GoToState(0);
            }
            break;
        case 3:
            towerSelector.highlightTileMode = true;
            HandlePlayState();
            break;
        case 4:
            towerSelector.highlightTileMode = false;
            HandleRotationState();
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
        else if (desiredState == 4) {
            currState = 4;
            playArrow.SetActive(true);
        } else if (desiredState == -1) {
            currState = -1;
            rectTransform.localPosition = origCardPosition;
            rectTransform.localRotation = origCardRotation;
            rectTransform.localScale = origCardScale;
            hoverHighlight.SetActive(false);
            playArrow.SetActive(false);
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
            if (playstyle == CardPlaystyle.Dragging) {
                GoToState(2);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), 
                    eventData.position, eventData.pressEventCamera, out Vector2 adjustedCursorPosition);
                rectTransform.localPosition = adjustedCursorPosition;
            } else {
                StartCoroutine(ClickDelayCoroutine());
            }
        }
    }

    private IEnumerator ClickDelayCoroutine() {
        yield return new WaitForSeconds(0.1f);
        GoToState(3);
    }

    public void OnDrag(PointerEventData eventData) {
        if (currState == 2 && playstyle == CardPlaystyle.Dragging) {
            // I got no clue why I have to subtract 700 but otherwise its weirdly offset
            rectTransform.localPosition = Input.mousePosition / canvas.scaleFactor - new Vector3(700,0,0);
            if (rectTransform.localPosition.y > cardPlayZone.y) {
            GoToState(3);
            }
        }
    }

    private void HandleHoverState() {
        // make it glow and enlarge
        hoverHighlight.SetActive(true);
        rectTransform.localScale = origCardScale * hoverScale;
        // rectTransform.localPosition.Set(rectTransform.localPosition.x, rectTransform.localPosition.y, rectTransform.localPosition.z + 1);
    }

    private void HandleDragState() {
        // remove card's rotation
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState() {
        
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;
        if (playstyle == CardPlaystyle.Dragging) {
            // mouse released
            if (!Input.GetMouseButton(0))
            {
                PlayCard();
            }
            if (Input.mousePosition.y / canvas.scaleFactor < cardPlayZone.y)
            {
                GoToState(2);
                playArrow.SetActive(false);
            }
        } else {
            if (Input.GetMouseButton(0)) {
                PlayCard();
            } else if (Input.GetMouseButton(1)) {
                GoToState(0);
                playArrow.SetActive(false);
            }
        }
    }

    private void PlayCard() {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        NectarManager nectarManager = FindObjectOfType<NectarManager>();

        if (cardDisplay.cardData.cost <= nectarManager.GetNectar())
        {
            TowerSelector towerSelector = FindObjectOfType<TowerSelector>();

            if (cardDisplay.cardData.cardType == Card.CardType.Tower)
            {
                GameObject to = ((TowerCard)cardDisplay.cardData).prefab;
                if (DEBUG_MODE) Debug.Log("Its about to look at a tower");
                // ensures tower is playable at mouse location
                if (towerSelector.spawnTower(to)) {
                    nectarManager.SetNectar(nectarManager.GetNectar() - cardDisplay.cardData.cost);
                    if (to.GetComponent<Tower>().IsRotatable()) {
                        if (DEBUG_MODE) Debug.Log("It is a rotatable tower");
                        // Transition to the rotation state
                        GoToState(4); 
                        //playArrow.SetActive(false);
                        return;
                    }
                } else {
                    errorManager.SetErrorMsg("Invalid tile!");
                    GoToState(0); // 2
                    playArrow.SetActive(false);
                    return;
                }
            }
            else
            {
                // Cast spell
                towerSelector.CastSpell(((SpellCard)cardDisplay.cardData).prefab);
                nectarManager.SetNectar(nectarManager.GetNectar() - cardDisplay.cardData.cost);

            }
            towerSelector.highlightTileMode = false;
            destroyCard();
        }
        else
        {
            errorManager.SetErrorMsg("Not enough nectar!");
            GoToState(0); // 2
            playArrow.SetActive(false);
        }
    }

    private void HandleRotationState()
    {
        Tower beamerTower = FindObjectOfType<Tower>();
        beamerTower.active = false;
        if (beamerTower == null)
            return;

        // Get mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate the angle between the tower and the mouse
        Vector3 direction = mousePosition - beamerTower.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;

        // Snap to the nearest hexagonal direction
        float snappedAngle = Mathf.Round(angle / 60f) * 60f;

        // Calculate the angle pointing toward the map's center (assuming center is at (0, 0))
        Vector3 centerDirection = beamerTower.transform.position;
        float centerAngle = Mathf.Atan2(centerDirection.y, centerDirection.x) * Mathf.Rad2Deg;

        // Check if the snapped angle points toward the center
        bool isBlocked = Mathf.Abs(Mathf.DeltaAngle(snappedAngle, centerAngle)) < 1f;

        // Update the tower's rotation
        beamerTower.transform.rotation = Quaternion.Euler(0, 0, snappedAngle);

        // Update visual feedback
        SpriteRenderer sprite = beamerTower.GetComponentInChildren<SpriteRenderer>();
        if (isBlocked)
        {
            sprite.color = Color.red;
            //beamerTower.active = false; // Tower is inactive if pointed at the center
        }
        else
        {
            sprite.color = Color.white;
            //beamerTower.active = true; // Tower is active otherwise
        }

        // Confirm rotation on click if not blocked
        if (Input.GetMouseButtonDown(0) && !isBlocked)
        {
            if (DEBUG_MODE) {
                Debug.Log($"Beamer Tower rotation set to {snappedAngle} degrees.");
            }
            beamerTower.active = true;
            destroyCard();
            GoToState(0); // Return to default state
        }
    }


    private void destroyCard()
    {
        CardDisplay cardDisplay = GetComponent<CardDisplay>();
        HandManager handManager = FindObjectOfType<HandManager>();
        handManager.cardsInHand.Remove(gameObject);
        handManager.DiscardCard(cardDisplay.cardData);
        Destroy(gameObject);
    }

    public void Reset() {
    GoToState(-1);
    }
}
