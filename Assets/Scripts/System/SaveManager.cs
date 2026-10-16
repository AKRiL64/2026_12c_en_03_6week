using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public static class SaveManager
{
    private record GObject
    {
        public int Index { get; set; }
        public int Exists { get; set; }
        public int HasRb { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector2 LinearSpeed { get; set; }
        public float AngularSpeed { get; set; }
    }
    
    private static readonly string SaveDirectory = "Saves";
    private static ObjectManager _objectManager;
    private static PlayerInventory _playerInventory;
    
    private static ulong _checksum = 0,
        _objectsMeta = 0;
    private static char _levelCheckpoint;
    private static int _score, _coins, _diamonds, _collectibles, _objectsNumber, _objectStatus, _level, _checkpoint;
    private static GObject[] _objects;
    

    public static void SetPlayerInventory(PlayerInventory playerInventory)
    {
        _playerInventory = playerInventory;
    }
    
    public static void UpdateObjectManager(ObjectManager objectManager)
    {
        _objectManager = ObjectManager.Instance;
    }

    public static bool Save([CanBeNull] string name, int checkpoint) // checkpoint number
    {
        _score = _playerInventory.Score;
        _coins = _playerInventory.CollectiblesCount[CoinType.Coin];
        _diamonds = _playerInventory.CollectiblesCount[CoinType.Diamond];
        
        char level = (char)(SceneManager.GetActiveScene().buildIndex);
        _levelCheckpoint = (char)((level << 4) | (char)checkpoint);
        Debug.Log($"Level to load: {level}");
        
        _objectsMeta = 0;
        
        string filepath = name == null ? SaveDirectory + "/" + DateTime.Now.Date : SaveDirectory + "/" + name;

        try
        {
            using (BinaryWriter writer =
                   new BinaryWriter(File.Create(filepath)))
            {
                WritePlayerStatus(writer);
                writer.Write(_levelCheckpoint);

                _objectsNumber = _objectManager.objects.Length;
                writer.Write(_objectsNumber);

                for (int i = 0; i < _objectsNumber; i++)
                    if (_objectManager.objects[i] != null)
                        WriteObject(i, writer);
                    else
                        writer.Write(0);

                Debug.Log($"ObjectsMeta: {_objectsMeta}\tLevelCheckpoint:{(int)_levelCheckpoint}\tObjectsCount:{_objectsNumber}\t" +
                          $"Coins:{_coins}\tDiamonds:{_diamonds}\tScore:{_score}");
                
                _checksum = _objectsMeta + (ulong)(_levelCheckpoint + _objectsNumber) + (ulong)(_coins * (_diamonds + 1) + _score);
                writer.Write(_checksum);
                
                Debug.Log("Checksum Saved: " + _checksum);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    private static void WriteObject(int index, BinaryWriter writer)
    {
        _objectStatus = 0;
        GameObject obj = _objectManager.objects[index];

        _objectStatus = GetObjectStatus(obj, index);
        writer.Write(_objectStatus);
        _objectsMeta += (ulong)_objectStatus;

        WriteTransform(obj, writer);

        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            WriteSpeed(rb, writer);
    }

    private static void WritePlayerStatus(BinaryWriter writer)
    {
        _score = _playerInventory.Score;
        _coins = _playerInventory.CollectiblesCount[CoinType.Coin];
        _diamonds = _playerInventory.CollectiblesCount[CoinType.Diamond];
        
        _collectibles = (_coins) | (_diamonds << 24);
        writer.Write(_score);
        writer.Write(_collectibles);
    }

    private static void WriteTransform(GameObject obj, BinaryWriter writer)
    {
        Transform objTransform = obj.GetComponent<Transform>();
        writer.Write(objTransform.position.x);
        writer.Write(objTransform.position.y);
        writer.Write(objTransform.position.z);
                    
        writer.Write(objTransform.rotation.eulerAngles.x);
        writer.Write(objTransform.rotation.eulerAngles.y);
        writer.Write(objTransform.rotation.eulerAngles.z);

        _objectsMeta += (ulong)(objTransform.position.x + objTransform.position.y + objTransform.position.z +
                               objTransform.rotation.eulerAngles.x + objTransform.rotation.eulerAngles.y + 
                               objTransform.rotation.eulerAngles.z);
    }

    private static void WriteSpeed(Rigidbody2D rb, BinaryWriter writer)
    {
        writer.Write(rb.linearVelocityX);
        writer.Write(rb.linearVelocityY);
        writer.Write(rb.angularVelocity);
        
        _objectsMeta += (ulong)(rb.linearVelocityX + rb.linearVelocityY + rb.angularVelocity);
    }

    private static int GetObjectStatus(GameObject obj, int index)
    {
        _objectStatus = 0;
        _objectStatus |= 1 << 31;
        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            _objectStatus |= 1 << 30;
            _objectStatus |= index;
        }
        return _objectStatus;
    }
    
    public static bool Load(string filepath)
    {
        _checksum = 0;
        _objectsMeta = 0;
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(SaveDirectory + "/" + filepath, FileMode.Open)))
            {
                LoadPlayerStatus(reader);
                LoadLevel(reader);

                AsyncOperation sceneLoaded = SceneManager.LoadSceneAsync(_level);
                sceneLoaded.completed += OnSceneLoaded;
                
                _objectsNumber = reader.ReadInt32();
                _objects = new GObject[_objectsNumber];
                
                for (int i = 0; i < _objectsNumber; i++)
                    LoadObject(reader);
                
                Debug.Log($"ObjectsMeta: {_objectsMeta}\tLevelCheckpoint:{(int)_levelCheckpoint}\tObjectsCount:{_objectsNumber}\t" +
                          $"Coins:{_coins}\tDiamonds:{_diamonds}\tScore:{_score}");
                
                ulong checksum = reader.ReadUInt64();
                _checksum = _objectsMeta + (ulong)(_levelCheckpoint + _objectsNumber) + (ulong)(_coins * (_diamonds + 1) + _score);
                
                Debug.Log($"Checksum Calculated: {_checksum}");
                if (checksum != _checksum)
                {
                    SceneManager.LoadScene(0);
                    Debug.LogError("Checksum fail");
                    Debug.Log($"Saved: {checksum}\tCalculated: {_checksum}");
                    return false;
                }
                
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            SceneManager.LoadScene(0);
            Debug.LogError("Load fail");
            return false;
        }
        Debug.Log("Load successful");
        return true;
    }

    private static void LoadObject(BinaryReader reader)
    {
        _objectStatus = reader.ReadInt32();
        int exists = _objectStatus >> 31,
            index = _objectStatus & 0x3FFFFFFF;
        _objectsMeta += (ulong)_objectStatus;
        
        Debug.Log($"Exists: {exists}");
        if (exists == 0)
        {
            _objects[index] = new GObject() {Index = index, Exists = exists}; 
            return;
        }

        float posX = reader.ReadSingle(), posY = reader.ReadSingle(), posZ = reader.ReadSingle(),
            rotX = reader.ReadSingle(), rotY = reader.ReadSingle(), rotZ = reader.ReadSingle();
        
        int hasRigidbody = (_objectStatus << 1) >> 31;
        _objects[index] = new GObject()
        {
            Index = index, Exists = exists, HasRb = hasRigidbody,
            Position = new Vector3(posX, posY, posZ),  Rotation = new Vector3(rotX, rotY, rotZ)
        };
        
        if (hasRigidbody == 1)
        {
            float linearSpeedX = reader.ReadSingle(), 
                  linearSpeedY = reader.ReadSingle(), 
                  angularSpeed = reader.ReadSingle();
            _objects[index].LinearSpeed = new Vector2(linearSpeedX, linearSpeedY);
            _objects[index].AngularSpeed = angularSpeed;
            _objectsMeta += (ulong)(posX + posY + posZ + rotX + rotY + rotZ + linearSpeedX + linearSpeedY + angularSpeed);
        }
        else
            _objectsMeta += (ulong)(posX + posY + posZ + rotX + rotY + rotZ);
    }

    private static void LoadLevel(BinaryReader reader)
    {
        _levelCheckpoint = reader.ReadChar();
        _level = _levelCheckpoint >> 4;
        Debug.Log($"Level to load: {_level}");
        _checkpoint = _levelCheckpoint & 0xF;
    }

    private static void LoadPlayerStatus(BinaryReader reader)
    {
        _score = reader.ReadInt32();
        _collectibles = reader.ReadInt32();
        _coins = _collectibles & 0xFFFFFF;
        _diamonds = _collectibles >> 24;
    }
    
    private static void OnSceneLoaded(AsyncOperation op)
    {
        Debug.Log("Loaded: " + SceneManager.GetActiveScene().name);
        UpdateObjectManager(ObjectManager.Instance);
        
        SetupPlayer();
        // call some checkpoint manager to move the player
        SetupGameObjects();
    }

    private static void SetupPlayer()
    {
        _playerInventory.UpdateScore(_score);
        _playerInventory.UpdateCollectibles(CoinType.Coin, _coins);
        _playerInventory.UpdateCollectibles(CoinType.Diamond, _diamonds);
    }
    
    private static void SetupGameObjects()
    {
        for (int i = 0; i < _objectsNumber; i++)
        {
            GObject savedObj = _objects[i];
            Debug.Log(savedObj.Index);
            GameObject obj = _objectManager.objects[savedObj.Index];

            if (savedObj.Exists == 0)
            {
                _objectManager.DestroyObjectN(i);
                continue;
            }
            
            obj.transform.position = savedObj.Position;
            obj.transform.rotation = Quaternion.Euler(savedObj.Rotation);

            if (savedObj.HasRb == 1)
            {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.linearVelocity =  savedObj.LinearSpeed;
                rb.angularVelocity =  savedObj.AngularSpeed;
            }
        }
    }
}
