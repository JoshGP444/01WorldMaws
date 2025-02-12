using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    public float movespeed;
    public bool ismoving;
    public Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask interActableLayer;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (ismoving != true)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Debug.Log("This is input.x" + input.x);
            //Debug.Log("This is input.y" + input.y);

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("movex", input.x);
                animator.SetFloat("movey", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))

                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("ismoving", ismoving);

        if (Input.GetKeyDown(KeyCode.Z))
            InterAct();
    }
    void InterAct()
    {
        var facingDir = new Vector3(animator.GetFloat("movex"), animator.GetFloat("movey"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interActableLayer);
        if(collider != null)
        {
            collider.GetComponent<interact>()?.Interact();
        }
    }
    IEnumerator Move(Vector3 targetPos)
    {
        ismoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movespeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        ismoving = false;
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interActableLayer) != null)
        {
            return false;
        }
        return true;
    }
}
