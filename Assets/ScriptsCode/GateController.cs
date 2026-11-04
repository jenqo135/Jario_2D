using UnityEngine;

public class GateController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        if (animator != null)
        {
            animator.SetBool("Open", true);
        }
        Debug.Log("Ворота открыты! Победа!");
    }
}