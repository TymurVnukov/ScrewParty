using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }
    public void StartAnim()
    {
        anim = this.gameObject.GetComponent<Animator>();
        StartCoroutine(AnimBox());
    }
    private IEnumerator AnimBox()
    {
        float targetY = -5f;
        anim.SetTrigger("BoxBreakTrigger");
        yield return new WaitForSeconds(1);
        while (transform.position.y > targetY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), 0.05f);
            yield return null;
        }
        yield return new WaitForSeconds(5);
        while (transform.position.y < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.05f);
            yield return null;
        }
    }
}
