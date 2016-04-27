using UnityEngine;
using CakewalkIoC.Signal;

public class SignalSender : MonoBehaviour {

    public static Signal<string, int> TestSignal = new Signal<string, int>();

    public float eventDelayTime = 1f;
    public string text = "Hello from EventBehaviour1!";
    public int number = 1;

    private float elapsedTime = 0f;
    private bool hasNotified = false;

	void Start () {
    	
	}
	
	void Update () {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > eventDelayTime && !hasNotified) {
            TestSignal.Dispatch(text, number);
            hasNotified = true;
        }
	}
}
