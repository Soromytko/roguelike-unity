using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text _foodText;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.FoodCountChanged += OnFoodCountChanged;
    }

    private void OnDisable()
    {
        _player.FoodCountChanged -= OnFoodCountChanged;
    }

    private void OnFoodCountChanged(int value) =>_foodText.text = $"Food: {value}";


}
