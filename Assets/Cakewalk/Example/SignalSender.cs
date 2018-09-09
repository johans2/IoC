using UnityEngine;
using Cakewalk.Signal;
using System.Collections;

public class SignalSender : MonoBehaviour {

    public static Signal<string, int> TestSignal = new Signal<string, int>();

    public float eventDelayTime = 1f;
    public string text = "Hello from SignalSender!";
    public int number = 1;
    
    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(1);
            TestSignal.Dispatch(text, number);
            number++;
        }
    }
}
