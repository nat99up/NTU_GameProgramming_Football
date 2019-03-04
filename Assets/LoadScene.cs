using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {

  public void LoadGameScene(int level)
  { 
        SceneManager.LoadScene(level);
  }
}
