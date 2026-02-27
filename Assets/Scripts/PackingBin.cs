using UnityEngine;

public class PackingBin : MonoBehaviour
{
    public PackingManager packingManager;

    private GameObject currentDisposable;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Disposable"))
            return;

        currentDisposable = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentDisposable == other.gameObject)
            currentDisposable = null;
    }

    public void TryDispose()
    {
        if (currentDisposable == null)
            return;

        Debug.Log(currentDisposable);

        packingManager.HandleDisposal(currentDisposable);

        currentDisposable = null;
    }

    void OnMouseDown()
    {
        packingManager.DisposeWholeBouquet();
    }
}
