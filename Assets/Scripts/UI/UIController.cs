using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
  public static UIController instance;
    void Awake() {
      if (instance != null) {
        Destroy(gameObject);
      } else {
        instance = this;
      }

    }
}
