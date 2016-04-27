using UnityEngine;
using System.Collections;
using System;

public class SignalListener : MonoBehaviour {

	void Start () {
        SignalSender.TestSignal.AddListener(OnSignal);
        SignalSender.TestSignal.AddListener(OnSignal);
    }
    
    private void OnSignal(string text, int number) {
        Debug.Log(ToString() + " GOT NOTIFICATION -> text: " + text + "   number: " + number);
    }

    void cannor() { }

}
