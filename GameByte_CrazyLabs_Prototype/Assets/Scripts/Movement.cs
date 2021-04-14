using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    public TileMovement _movement;
    public Transform _transform;
    public GameObject _cam, _particles, _particles2, _hitParticle, _slider, _gameOverScreen, _pauseMenu;
    public float _scalerSpeed, _cubeXSpeed, _distance;
    private bool _gameOver, _slowDown;
    public Animator _camAnim;
    public Slider _localScale;
    public SoundManager sndmng;
    
    /// <summary>
    ///         Sorry if this code isn't that pleasing to look at. 7 days to make a game is not enough to
    ///         make a code look good :)
    /// </summary>
    
    [Range(0f, 1f)] public float _puGain, _puLoss;
    void FixedUpdate()
    {
        if (!_movement._paused)
        {
            if (Input.GetMouseButton(0)) // Easier for testing on PC
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = _distance;
            
                //  Limiting the X coordinate:
            
                _transform.position = new Vector3(Mathf.Clamp((Camera.main.ScreenToWorldPoint(mousePosition).x * _cubeXSpeed), -1.5f,
                    1.5f), _transform.position.y, mousePosition.z);
            }
        }
    }

    private void Update()
    {
        if (!_movement._paused)
        {
            if (_slowDown)
            {
                _slider.SetActive(false);
                _movement._tileSpeed = _transform.localScale.x - 0.2f;
                _transform.localScale = new Vector3(Mathf.Max(_transform.localScale.x - 0.00275f, 0.2f), Mathf.Max(_transform.localScale.y - 0.00275f, 0.2f),
                    Mathf.Max(_transform.localScale.z - 0.00275f, 0.2f));
                _particles.transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
            }
        
            //      Getting smaller:

            if (!_gameOver)
            {
                _transform.localScale = new Vector3(_transform.localScale.x - _scalerSpeed, _transform.localScale.y - _scalerSpeed,
                    _transform.localScale.z - _scalerSpeed);
                _localScale.value = _transform.localScale.x;
            }
        
            //      Game over if:           ( using only x because they are all the same anyway )

            if (_transform.localScale.x <= 0.2f && !_gameOver)
            {
                _gameOver = true;
                GameOver();
            }
        
            //      Particles scaling with cube

            if (!_slowDown)
            {
                _particles.transform.localScale = new Vector3(_transform.localScale.x, _particles.transform.localScale.y, _transform.localScale.z);
            }
            else
            {
                _particles.transform.localScale = new Vector3(_transform.localScale.x -0.2f, _particles.transform.localScale.y-0.2f, _transform.localScale.z-0.2f);
            }
        }
    }

    public void GameOver()
    {
        _gameOverScreen.SetActive(true);
        _pauseMenu.SetActive(false);
        _movement._tileSpeed = 0f;
        _movement._acceleration = 0f;
        sndmng.PlaySound("lost");
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            _transform.localScale = new Vector3(Mathf.Max(_transform.localScale.x - _puLoss, 0.2f), Mathf.Max(_transform.localScale.y - _puLoss, 0.2f),
                Mathf.Max(_transform.localScale.z - _puLoss, 0.2f));
            Instantiate(_hitParticle, _transform.position, Quaternion.identity);
            sndmng.PlaySound("obstacle");
        }
        else if(other.gameObject.tag == "StartSlow")
        {
            _particles2.SetActive(false);
            _slowDown = true;
            _cam.transform.eulerAngles = new Vector3(0f,0f,0f);
            _camAnim.SetTrigger("End");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            _transform.localScale = new Vector3(Mathf.Min(_transform.localScale.x + _puGain, 1f), Mathf.Min(_transform.localScale.y + _puGain, 1f),
                Mathf.Min(_transform.localScale.z + _puGain, 1f));
            Destroy(other.gameObject);
            sndmng.PlaySound("powerup");
        }
    }

    public void Damage(float dmg)
    {
        _puLoss = dmg;
    }
    
    public void Boost(float bst)
    {
        _puGain = bst;
    }

    public void Melting(float melt)
    {
        _scalerSpeed = melt;
    }

    public void Losing(float losing)
    {
        losing = _localScale.value;
    }

    public void IceCubeSpeed(float speed)
    {
        _cubeXSpeed = speed;
    }
}
