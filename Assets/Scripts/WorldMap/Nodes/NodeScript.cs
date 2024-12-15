using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Node;

namespace Node {
    public class NodeScript : MonoBehaviour {
        [SerializeField] string nodeName;
        private SpriteRenderer spriteRenderer;
        [Header("Sprites")]
        public Sprite defaultSprite;     // The normal/default sprite
        public Sprite highlightSprite;   // The sprite for when hovering


        void Start() {
            // Cache the SpriteRenderer and original color
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Set the default sprite at the start
            if (defaultSprite != null)
                spriteRenderer.sprite = defaultSprite;
        }

        void OnMouseEnter() {
            // Change to the highlight sprite on hover
            if (highlightSprite != null)
                spriteRenderer.sprite = highlightSprite;
        }

        void OnMouseExit() {
            // Reset to the default sprite when no longer hovering
            if (defaultSprite != null)
                spriteRenderer.sprite = defaultSprite;
        }

        void OnMouseDown() {
            // Interaction logic when the GameObject is clicked
            GameObject.Find("MapManager").GetComponent<MapManager>().MakeMapInactive();
            SceneManager.LoadScene(nodeName);
        }
    }
}