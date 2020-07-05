using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceBullet : MonoBehaviour
{
    bool canBePressed, isPressed;
    float speed; // per second
    public KeyCode keyToPress;
    float timeWhenEnter;
    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        keyToPress = KeyCode.J;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        if (canBePressed && !isPressed)
        {
            if (Input.GetKeyDown(keyToPress))
            {
                float delta = Time.time - timeWhenEnter;
                PaceManager.PaceMatchScore score = PaceManager.Judge(delta);
                transform.parent.GetComponent<PaceManager>().DecBullet(score);
                isPressed = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Entering Sprites.");

        if (collision.tag == "Target")
        {
            canBePressed = true;
            isPressed = false;
            timeWhenEnter = Time.time;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            if (!isPressed)
            {
                transform.parent.GetComponent<PaceManager>().DecBullet(PaceManager.PaceMatchScore.MISS);   
            }
            canBePressed = false;
            gameObject.SetActive(false);
        }
    }
}
