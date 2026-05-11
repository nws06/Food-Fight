using UnityEngine;

public class StrawberryManager : AEnemyManager, IPauseableUpdate, IPauseableFixedUpdate
{
    private float _randomXPosition;
    private float _randomYPosition;
    private Vector2 _randomPosition;



    // No further initialization needed
    public override void Initialize() { }



    /// <summary>
    /// Set the given enemy's position to a random vector that is, relative to the player's transform: <br />
    /// between 80 and 120 units away on the x axis <br />
    /// between 25 and 75 units away on the y axis 
    /// </summary>
    public override Vector2 RandomLocation()
    {
        // X position: 
        _randomXPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 30f);
        _randomXPosition = (_randomXPosition >= 0) ? _randomXPosition + 80 : _randomXPosition - 80;
        _randomXPosition += _playerTransform.position.x;

        // Y position: 
        _randomYPosition = (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, 25f);
        _randomYPosition = (_randomYPosition >= 0) ? _randomYPosition + 50 : _randomYPosition - 50;
        _randomYPosition += _playerTransform.position.y;

        _randomPosition = new Vector2(_randomXPosition, _randomYPosition);

        return _randomPosition;
    }



    void OnDisable()
    {
        UpdateManager.Instance.UnregisterFromUpdate(this);
        UpdateManager.Instance.UnregisterFromFixedUpdate(this);

        _pauseService.OnGamePause -= OnGamePause;
    }
}
