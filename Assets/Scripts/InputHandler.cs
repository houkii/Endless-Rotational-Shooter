using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour, IPointerClickHandler
{
    //PlayerController player;
    public float LockInputTime = 2.0f;
    public UnityEvent OnScreenClick;

    bool InputLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.OnPlayerDead += () => StartCoroutine(LockInput(LockInputTime));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InputLocked) return;

        if (GameController.Instance.GameRunning)
        {
            OnScreenClick?.Invoke();
        }
        else
        {
            GameController.Instance.RestartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            if(GameController.Instance.GameRunning)
            {
                OnScreenClick?.Invoke();
            }
            else
            {
                GameController.Instance.RestartGame();
            }
        }
    }

    private IEnumerator LockInput(float lockTime)
    {
        InputLocked = true;
        yield return new WaitForSeconds(lockTime);
        InputLocked = false;
    }
}
