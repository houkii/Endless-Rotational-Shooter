using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public float LockInputTime = 2.0f;
    public UnityEvent OnScreenClick;

    private bool InputLocked = false;

    private void Start()
    {
        PlayerController.Instance.OnPlayerDead += () => StartCoroutine(LockInput(LockInputTime));
    }

    private void Update()
    {
        if (InputLocked) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            if (GameController.Instance.GameRunning)
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
