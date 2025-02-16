using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_test : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;
    void Start()
    {
        btn.onClick.AddListener(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        Debug.Log("Button Clicked");
    }
}
