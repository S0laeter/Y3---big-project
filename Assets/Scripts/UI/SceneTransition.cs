using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator animator;
    public float transitionTime;

    //load scene
    public void LoadSceneAfterTransition(string sceneName)
    {
        StartCoroutine(WaitForTransition(sceneName));
    }
    
    //play the transition animation, wait till its done, then switch scene
    IEnumerator WaitForTransition(string name)
    {
        animator.SetTrigger("TransitionStart");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(name);
    }

}
