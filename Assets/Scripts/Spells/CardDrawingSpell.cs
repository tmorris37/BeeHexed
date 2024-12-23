using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardDrawingSpell : MonoBehaviour {
    // flag to let other scripts know that this card is done drawing (i.e. so it is safe to discard without risk of drawing itself back)
    public bool finishedDrawing = false;
    public bool safeToDestroy = false;
    // draws numCards cards
    protected void DrawCards(int numCards) {
        DrawPileManager drawPileManager = FindObjectOfType<DrawPileManager>();
        HandManager handManager = FindObjectOfType<HandManager>();
        for (int card = 0; card < numCards; card++) {
            drawPileManager.DrawCard(handManager);
        }
        finishedDrawing = true;
    }

    protected void DestroyIfSafe() {
        if (safeToDestroy) {
            Destroy(gameObject);
        }
    }

    protected void Update() {
        DestroyIfSafe();
    }
}
