using UnityEngine;

public class AnimatedTexture : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private float _speedX = 0.1f;
    [SerializeField] private float _speedY = 0.1f;

    private float _curX;
    private float _curY;

    void Start()
    {
        _curX = _renderer.material.mainTextureOffset.x;
        _curY = _renderer.material.mainTextureOffset.y;
    }

    void Update()
    {
        _curX += _speedX * Time.deltaTime;
        _curY += _speedY * Time.deltaTime;

        _renderer.material.mainTextureOffset = new Vector2(_curX, _curY);
    }
}
