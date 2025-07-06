using UnityEngine;

public class WeaponHover : MonoBehaviour
{
    // Reference to the player transform to position the weapon relative to the player
    public Transform player;
    
    // Distance to hover beside the player on the x-axis
    public float hoverDistance = 0.8f;

    // SpriteRenderer component for flipping the weapon sprite based on direction
    private SpriteRenderer sr;

    // Offset vector representing where the weapon hovers relative to the player
    private Vector3 targetOffset = Vector3.right;

    void Start()
    {
        // Initialize the hover offset to be hoverDistance units to the right of the player
        targetOffset = new Vector3(hoverDistance, 0, 0);
        
        // Get the SpriteRenderer component attached to this GameObject
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get horizontal input to determine player movement direction (-1 left, 1 right)
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            // Set target offset to hover on the side the player is facing
            targetOffset = new Vector3(Mathf.Sign(horizontal) * hoverDistance, 0, 0);
            
            // Flip the weapon sprite horizontally when player moves left
            sr.flipX = horizontal < 0;
        }

        // Update the weapon's position to hover beside the player at the target offset
        transform.position = player.position + targetOffset;
    }
}
