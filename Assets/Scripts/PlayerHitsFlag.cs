using UnityEngine;

public class PlayerHitsFlag : MonoBehaviour
{
    [SerializeField] private UIController _uiController = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerFlag"))
        {
            _uiController.CurrentScreen = UIController.Screen.End;
        }
    }
}
