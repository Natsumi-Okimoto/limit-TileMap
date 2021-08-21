using UnityEngine;


public class SearchArea : MonoBehaviour
{
    public bool isSearching;
    public GameObject player;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSearching = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSearching = false;
            player = null;
        }
    }
}
