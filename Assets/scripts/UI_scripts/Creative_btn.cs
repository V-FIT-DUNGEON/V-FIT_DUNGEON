using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This scrip make hit box of botton follow the picture exlude transparent part
public class Creative_btn : MonoBehaviour
{
    [SerializeField] float _alphaHitTestMinimumThreshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = _alphaHitTestMinimumThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("Button Clicked");
    }
}
