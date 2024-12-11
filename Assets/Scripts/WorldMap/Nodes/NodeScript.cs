using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Node;

namespace Node {
    public class NodeScript : MonoBehaviour
    {
        [SerializeField] string nodeName;
        private SpriteRenderer spriteRenderer;
        public Color highlightColor = Color.yellow; // Color for hover
        private Color originalColor;

        void Start()
        {
            // Cache the SpriteRenderer and original color
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

        void OnMouseEnter()
        {
            // Highlight the sprite when mouse hovers
            spriteRenderer.color = highlightColor;
        }

        void OnMouseExit()
        {
            // Reset to the original color
            spriteRenderer.color = originalColor;
        }

        void OnMouseDown()
        {
            // Interaction logic when the GameObject is clicked
            SceneManager.LoadScene(nodeName);
        }
    }
}