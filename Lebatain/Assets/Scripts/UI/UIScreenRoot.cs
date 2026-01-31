using UnityEngine;

public class UIScreenRoot : MonoBehaviour
{
    [SerializeField] private UIScreenType type;


    public void Assign()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RegisterScreen(type, gameObject);
        }
    }

    private void OnDestroy()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UnregisterScreen(type, gameObject);
        }
    }
}
