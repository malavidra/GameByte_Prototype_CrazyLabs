using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileMovement : MonoBehaviour
{
    public int _preSpawnedTiles;
    [Range(0, 5)] public int _chancesToSpawnPU, _chancesToSpawnOBS;
    [Range(0f, 2f)]public float _maxSpeed;
    public List<Transform> _tileTransform;
    [Range(0f, 2f)]public float _tileSpeed;
    [Range(0f, 0.001f)] public float _acceleration;
    public GameObject _tilePrefab, _powerUpPrefab, _obstaclePrefab, _obstaclePrefab1, _finalTilePrefab;
    private int _zMultiplier = 0;
    private float _tileLenght = 20, _diff;
    private bool _start, _gameEnded = false;
    public bool _paused = false;
    public GameObject _pauseMenu1, _pauseMenu2, _buttonNext;
    private GameObject _finalObstacle;
    private Vector3 _finalPosition;
    public List<GameObject> _tileList = new List<GameObject>(), _powerUpList = new List<GameObject>(),
        _obstacleList = new List<GameObject>();

    private Transform _pSystemFan;
    private int _randomi, _randomi0;

    private void Start()
    {
        if (!_paused)
        {
            _gameEnded = false;
            _start = true;
            for (int j = 0; j <= _preSpawnedTiles; j++)
            {
                SpawnTiles();
                if (j == 3)
                { 
                    _start = false;
                }
            }
        }
    }

    private void Update()
    {
        if (!_paused)
        {
            if (_tileSpeed == _maxSpeed)
            {
                EndGame();
            }
        
            if (_tileTransform.Count != 0)
            {
                for (int i = 0; i < _tileTransform.Count; i++)
                {
                    _tileTransform[i].position = new Vector3(_tileTransform[i].position.x, _tileTransform[i].position.y, _tileTransform[i].position.z - _tileSpeed);
                }
            }
            
            if (_tileTransform[0].position.z <= -_tileLenght && !_gameEnded)
            {
                _diff = _tileLenght - _tileTransform[0].position.z;
                SpawnTiles();
                DestroyTiles();
            }
        
            //          Speeding Up

            if (!_gameEnded)
            {
                _tileSpeed = Mathf.Min(_tileSpeed + _acceleration,_maxSpeed);
            }
        }
    }

    private void EndGame()
    {
        if (!_gameEnded)
        {
            _gameEnded = true;
            SpawnTiles();
        }
    }
    private void SpawnTiles()
    {
        if (!_gameEnded)
        {
            GameObject newTile = Instantiate(_tilePrefab, new Vector3(0f, -1.4f, _tileLenght * _zMultiplier - _diff),
                Quaternion.identity);

            if (!_start)
            {

                //      Spawn Power UP

                if (Random.Range(_chancesToSpawnPU + 95, 101) == 100)
                {
                    GameObject powerUp = Instantiate(_powerUpPrefab,
                        new Vector3(Random.Range(-1.2f, 1.2f), -0.11f, _tileLenght * _zMultiplier - _diff),
                        Quaternion.identity);
                    powerUp.transform.parent = newTile.transform;
                    _powerUpList.Add(powerUp);
                }

                //      Spawn Obstacles

                if (Random.Range(_chancesToSpawnOBS + 95, 101) == 100)
                {
                    _randomi0 = Random.Range(0, 2);
                    
                    if (_randomi0 == 0)
                    {
                        _finalObstacle = _obstaclePrefab;
                        
                        _finalPosition = new Vector3(Random.Range(-1, 2) * 1.5f, -1,
                            newTile.transform.position.z);
                    }
                    else
                    {
                        
                        _finalObstacle = _obstaclePrefab1;

                        _pSystemFan = _finalObstacle.transform.GetChild(0);
                        
                        _randomi = Random.Range(0,2);
                        if (_randomi == 0)
                        {
                            _randomi = -1;
                            _pSystemFan.eulerAngles = new Vector3(180f, -180f, 0f);
                        }
                        else
                        {
                            _randomi = 1;
                            _pSystemFan.eulerAngles = new Vector3(0f, 0f, 0f);
                        }
                        _finalPosition = new Vector3(5.9f * (float) _randomi * -1f, -3.6f, newTile.transform.position.z);
                    }
                    GameObject obstacle = Instantiate(_finalObstacle, _finalPosition,
                                Quaternion.identity);

                    obstacle.transform.parent = newTile.transform;
                    if (_randomi0 == 1)
                    {
                        obstacle.transform.localScale = new Vector3(
                            _finalObstacle.transform.localScale.x * (_randomi * -1),
                            _finalObstacle.transform.localScale.y,
                            _finalObstacle.transform.localScale.z);
                    }
                    _obstacleList.Add(obstacle);
                }

            }

            _tileList.Add(newTile);
            _tileTransform.Add(newTile.transform);

            _zMultiplier = Mathf.Min(_zMultiplier + 1, 9);
        }
        else
        {
            _diff = 10f - _tileTransform[0].position.z;
            GameObject finalTile = Instantiate(_finalTilePrefab, new Vector3(0f, -1.4f,
                    _tileTransform[_tileTransform.Count-1].position.x + _tileLenght * _zMultiplier - _diff + _tileLenght*1.5f),
                Quaternion.identity);
            _tileList.Add(finalTile);
            _tileTransform.Add(finalTile.transform);
        }
    }

    public void DestroyTiles()
    {
        Destroy(_tileList[0]);
        _tileList.RemoveAt(0);
        _tileTransform.RemoveAt(0);
    }

    public void SlideSpeed(float newSpeed)
    {
        _tileSpeed = newSpeed;
    }

    public void SlideAcceleration(float newAcceleration)
    {
        _acceleration = newAcceleration;
    }

    public void SlideMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
    }

    public void SlideOBSF(float slideOBS)
    {
        _chancesToSpawnOBS = (int)slideOBS;
    }
    
    public void SlidePUF(float slidePU)
    {
        _chancesToSpawnPU = (int)slidePU;
    }

    public void Pause()
    {
        if (_paused)
        {
            _paused = false;
            _pauseMenu1.SetActive(false);
            _pauseMenu2.SetActive(false);
            _buttonNext.SetActive(false);
        }
        else
        {
            _paused = true;
            _pauseMenu1.SetActive(true);
            _buttonNext.SetActive(true);
        }
    }
}
