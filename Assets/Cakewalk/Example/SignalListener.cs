using UnityEngine;
public class SignalListener : MonoBehaviour {

    public int maxNum = 5;

	void Start () {
        Debug.Log(ToString() + "Adding listener..");
        SignalSender.TestSignal.AddListener(OnSignal);
    }
    
    private void OnSignal(string text, int number) {
        if(number > maxNum) {
            Debug.Log(ToString() + " Removing listener..");
            SignalSender.TestSignal.RemoveListener(OnSignal);
        }
        else {
            Debug.Log(ToString() + " GOT NOTIFICATION -> text: " + text + "   number: " + number);
        }
    }
}
