using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private FieldManager _fieldManager;

    private void Awake()
    {
        _fieldManager = FindObjectOfType<FieldManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player player))
            return;

        _fieldManager.LoadNextLevel();
    }
}
