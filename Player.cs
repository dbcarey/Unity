using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _powerupPrefab;
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _leftEngine,_rightEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
//    [SerializeField]
//    private AudioClip _explosionSoundClip;


    //Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0, 0 ,0) 
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        //create variable for direction
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
            
        }

        //constrain player movements

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.0)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.0)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {

        //if space key is hit spawn gameObject

        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);  //don't care about rotation
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        _audioSource.Play();

    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.gameObject.SetActive(false);
            return;
        }


        _lives--;
        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
        }
        if (_lives == 1)
        {
            _leftEngine.gameObject.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        
        // check if player dead, if so destroy us
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            //communicate with spawn manager to stop spawning
            Destroy(this.gameObject);
        }

    }


    public void TripleShotActive()
    {

        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.gameObject.SetActive(true);
        
        //StartCoroutine(ShieldsPowerDownRoutine());

    }
    public void AddScore(int points)
    {

        _score += points;
        _uiManager.UpdateScore(_score);

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_isSpeedBoostActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _isSpeedBoostActive = false;
        }
    }

}

