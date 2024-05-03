using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScript : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Renderer _renderer;
    // Start is called before the first frame update
    private Color _defaultColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
    }
    void Start()
    {
        _player.OnPlayerHit += (sender, args) =>
        {
            UpdatePlayerColor();
        };

        _player.OnPlayerNotHit += (sender, args) =>
        {
            ResetlayerColor();
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdatePlayerColor()
    {
        _renderer.material.color = new Color(255f, 0f, 0f);
    }

    private void ResetlayerColor()
    {
        _renderer.material.color = _defaultColor;
    }
}
