using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;

    public bool isPlayer1;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool jump;
        float horiz;
        if (isPlayer1)
        {
            jump = playerControls.Player1.Jump.WasPressedThisFrame();
            horiz = playerControls.Player1.Horizontal.ReadValue<Vector2>().x;
        }
        else
        {
            jump = playerControls.Player2.Jump.WasPressedThisFrame();
            horiz = playerControls.Player2.Horizontal.ReadValue<Vector2>().x;
        }

        if(jump)
        {
            rb.velocity += new Vector2(0, 7);
        }
        if (horiz != 0)
        {
            transform.position += new Vector3(horiz * Time.deltaTime * 5, 0, 0);
        }
    }
}
