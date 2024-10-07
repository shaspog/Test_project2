using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesClosing : MonoBehaviour
{
    public RectTransform UpperEyelid;      // The top UI Image's RectTransform
    public RectTransform LowerEyelid;   // The bottom UI Image's RectTransform
    public float closingSpeed = 2f;      // Speed of eyelid closing
    public float openingSpeed = 2f;      // Speed of eyelid opening

    private Vector2 topOpenPos;
    private Vector2 topClosedPos;
    private Vector2 bottomOpenPos;
    private Vector2 bottomClosedPos;
    private bool isClosing = false;
    private bool isOpening = false;
    public bool isClosed = false;
    public bool isOpen = true;

    void Start()
    {
        // Store the starting and target positions
        topOpenPos = UpperEyelid.anchoredPosition;
        bottomOpenPos = LowerEyelid.anchoredPosition;

        // Assuming the target positions when eyelids are fully closed (centered)
        topClosedPos = new Vector2(topOpenPos.x, 90);
        bottomClosedPos = new Vector2(bottomOpenPos.x, -90);
    }

    void Update()
    {
        // Animate the eyelids closing
        if (isClosing)
        {
            UpperEyelid.anchoredPosition = Vector2.Lerp(UpperEyelid.anchoredPosition, topClosedPos, closingSpeed * Time.deltaTime);
            LowerEyelid.anchoredPosition = Vector2.Lerp(LowerEyelid.anchoredPosition, bottomClosedPos, closingSpeed * Time.deltaTime);

            if (Vector2.Distance(UpperEyelid.anchoredPosition, topClosedPos) < 0.1f)
            {
                isClosing = false; // Stop when the eyelids are fully closed
                isClosed = true;
                isOpen = false;
            }
        }

        // Animate the eyelids opening
        if (isOpening)
        {
            UpperEyelid.anchoredPosition = Vector2.Lerp(UpperEyelid.anchoredPosition, topOpenPos, openingSpeed * Time.deltaTime);
            LowerEyelid.anchoredPosition = Vector2.Lerp(LowerEyelid.anchoredPosition, bottomOpenPos, openingSpeed * Time.deltaTime);

            if (Vector2.Distance(UpperEyelid.anchoredPosition, topOpenPos) < 0.1f)
            {
                isOpening = false; // Stop when the eyelids are fully open
                isOpen= true;
                isClosed= false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (isClosed)
            {
                OpenEyes();
                print("eyes opened");
            }

            else
            {
                CloseEyes();
                print("eyes closed");
            }

        }
    }

    // Call this method to trigger the closing of the eyelids
    public void CloseEyes()
    {
        isClosing = true;
        isOpening = false;
    }

    // Call this method to trigger the opening of the eyelids
    public void OpenEyes()
    {
        isOpening = true;
        isClosing = false;
    }
}