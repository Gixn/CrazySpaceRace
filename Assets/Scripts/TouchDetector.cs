using UnityEngine;


public interface ITouchDetectorDelegate {
    void OnSwipeUp();
    void OnTouchLeftHalf();
    void OnTouchRightHalf();
}

public class TouchDetector : MonoBehaviour {
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease;

    public ITouchDetectorDelegate TouchDelegate;

    public float SWIPE_THRESHOLD = 20f;
    
    void Start(){
        Input.simulateMouseWithTouches = true;
    }

    // Update is called once per frame
    void Update() {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                fingerUp = touch.position;
                fingerDown = touch.position;
                
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved) {
                if (!detectSwipeOnlyAfterRelease) {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended) {
                fingerDown = touch.position;
                checkSwipe();
            }
        }

    }

    void checkSwipe() {
        
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove()) { //Check if Vertical swipe
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        } else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove()) { //Check if Horizontal swipe
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        } else { // Simple touch
            OnTouch();

            if (fingerUp.x < Screen.width/2) {
                OnTouchLeftHalf();
            } else {
                OnTouchRightHalf();
            }
        }
        
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////    
    void OnSwipeUp() {
        //Debug.Log("Swipe Up");
        TouchDelegate.OnSwipeUp();
    }

    void OnSwipeDown() {
        //Debug.Log("Swipe Down");
    }

    void OnSwipeLeft() {
        // Debug.Log("Swipe Left");
        TouchDelegate.OnTouchLeftHalf();
    }

    void OnSwipeRight() {
        //Debug.Log("Swipe Right");
        TouchDelegate.OnTouchRightHalf();
    }

    void OnTouch() {
        //Debug.Log("Simple Touch");
    }
    
    void OnTouchLeftHalf() {
        //Debug.Log("Touch Left Side");
        TouchDelegate.OnTouchLeftHalf();
    }
    
    void OnTouchRightHalf() {
        //Debug.Log("Touch Right Side");
        TouchDelegate.OnTouchRightHalf();
    }
}