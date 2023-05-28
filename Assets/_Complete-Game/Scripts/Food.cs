using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public int Value => _value;

    [SerializeField] private int _value = 10;
    [SerializeField] private Text _valueText;

    public void Destroy() => StartCoroutine(Destroying());

    private IEnumerator Destroying()
    {
        _valueText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        _valueText.text = $"+{_value}";
        _valueText.enabled = true;

        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;

        float dist = Screen.height / 20f;
        Vector3 targetPosition = _valueText.transform.position + Vector3.up * dist;
        while (_valueText.transform.position != targetPosition)
        {
            _valueText.transform.position = Vector3.MoveTowards(_valueText.transform.position, targetPosition, Time.deltaTime * dist);
            float progress = (targetPosition - _valueText.transform.position).magnitude / dist;
            _valueText.color = new Color(_valueText.color.r, _valueText.color.g, _valueText.color.b, progress);
            yield return null;
        }
        Destroy(gameObject);
    }
}
