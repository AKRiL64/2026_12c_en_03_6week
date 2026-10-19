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
        public uint Index { get; set; }
        public uint Exists { get; set; }
        public uint HasRb { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector2 LinearSpeed { get; set; }
        public float AngularSpeed { get; set; }
    }
    
    private static readonly string SaveDirectory = "Saves";
    private const string DefaultFilename = "burmalda";
    private static ObjectManager _objectManager;
    private static PlayerInventory _playerInventory;
    
    private static ulong _checksum = 0,
        _objectsMeta = 0;
    private static char _levelCheckpoint;
    private static uint _score, _coins, _diamonds, _collectibles, _objectsNumber, _objectStatus, _level, _checkpoint;
    private static GObject[] _objects;

    public static string SaveFilename = DefaultFilename;
    

    public static void SetPlayerInventory(PlayerInventory playerInventory)
    {
        _playerInventory = playerInventory;
    }
    
    public static void UpdateObjectManager(ObjectManager objectManager)
    {
        _objectManager = ObjectManager.Instance;
    }

    public static bool Save(int checkpoint, string name = DefaultFilename) // checkpoint number
    {
        _score = (uint)_playerInventory.Score;
        _coins = (uint)_playerInventory.CollectiblesCount[CoinType.Coin];
        _diamonds = (uint)_playerInventory.CollectiblesCount[CoinType.Diamond];
        
        char level = (char)(SceneManager.GetActiveScene().buildIndex);
        _levelCheckpoint = (char)((level << 4) | (char)checkpoint);
        Debug.Log($"Level to load: {level}");
        
        _objectsMeta = 0;

        Directory.CreateDirectory(SaveDirectory);
        string filepath = name == null ? SaveDirectory + "/" + DateTime.Now.Date : SaveDirectory + "/" + name;
        
        try
        {
            using (BinaryWriter writer =
                   new BinaryWriter(File.Create(filepath)))
            {
                WritePlayerStatus(writer);
                writer.Write(_levelCheckpoint);

                _objectsNumber = (uint)_objectManager.objects.Length;
                writer.Write(_objectsNumber);

                for (int i = 0; i < _objectsNumber; i++)
                    WriteObject(i, writer);

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
        if (obj == null)
        {
            _objectStatus |= (uint)index;
            _objectsMeta += (uint)_objectStatus;
            writer.Write(_objectStatus);
            return;
        }

        _objectStatus = (uint)GetObjectStatus(obj, index);
        writer.Write(_objectStatus);
        
        _objectsMeta += (uint)_objectStatus;

        WriteTransform(obj, writer);

        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            WriteSpeed(rb, writer);
    }

    private static void WritePlayerStatus(BinaryWriter writer)
    {
        _score = (uint)_playerInventory.Score;
        _coins = (uint)_playerInventory.CollectiblesCount[CoinType.Coin];
        _diamonds = (uint)_playerInventory.CollectiblesCount[CoinType.Diamond];
        
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
        
        AddFloatToMeta(objTransform.position.x); 
        AddFloatToMeta(objTransform.position.y); 
        AddFloatToMeta(objTransform.position.z);
        AddFloatToMeta(objTransform.rotation.eulerAngles.x); 
        AddFloatToMeta(objTransform.rotation.eulerAngles.y); 
        AddFloatToMeta(objTransform.rotation.eulerAngles.z);
    }

    private static void WriteSpeed(Rigidbody2D rb, BinaryWriter writer)
    {
        writer.Write(rb.linearVelocityX);
        writer.Write(rb.linearVelocityY);
        writer.Write(rb.angularVelocity);
        
        AddFloatToMeta(rb.linearVelocityX); AddFloatToMeta(rb.linearVelocityY);
        AddFloatToMeta(rb.angularVelocity);
    }

    private static uint GetObjectStatus(GameObject obj, int index)
    {
        _objectStatus = 0;
        _objectStatus = (uint)(_objectStatus | (1 << 31));
        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            _objectStatus |= 1 << 30;
        _objectStatus |= (uint)index;
        return _objectStatus;
    }
    
    public static bool Load(string name = DefaultFilename)
    {
        _checksum = 0;
        _objectsMeta = 0;
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(SaveDirectory + "/" + name, FileMode.Open)))
            {
                LoadPlayerStatus(reader);
                LoadLevel(reader);
                
                _objectsNumber = (uint)reader.ReadInt32();
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
        CheckpointManager.WipeCheckpoints();
        
        StatusPersister.SaveScore((int)_score);
        StatusPersister.SaveCollectibles((int)_coins, (int)_diamonds);
        
        AsyncOperation sceneLoaded = SceneManager.LoadSceneAsync((int)_level);
        sceneLoaded.completed += OnSceneLoaded;
        
        Debug.Log("Load successful");
        return true;
    }

    private static void LoadObject(BinaryReader reader)
    {
        _objectStatus = (uint)reader.ReadInt32();
        uint exists = _objectStatus >> 31,
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
        
        uint hasRigidbody = (_objectStatus << 1) >> 31;
        _objects[index] = new GObject()
        {
            Index = index, Exists = exists, HasRb = hasRigidbody,
            Position = new Vector3(posX, posY, posZ),  Rotation = new Vector3(rotX, rotY, rotZ)
        };
        AddFloatToMeta(posX); AddFloatToMeta(posY); AddFloatToMeta(posZ);
        AddFloatToMeta(rotX); AddFloatToMeta(rotY); AddFloatToMeta(rotZ);
        
        if (hasRigidbody == 1)
        {
            float linearSpeedX = reader.ReadSingle(), 
                  linearSpeedY = reader.ReadSingle(), 
                  angularSpeed = reader.ReadSingle();
            _objects[index].LinearSpeed = new Vector2(linearSpeedX, linearSpeedY);
            _objects[index].AngularSpeed = angularSpeed;
            AddFloatToMeta(linearSpeedX); AddFloatToMeta(linearSpeedY); 
            AddFloatToMeta(angularSpeed);
        }
    }

    private static void LoadLevel(BinaryReader reader)
    {
        Checkpoint.ResetCheckpoint();
        _levelCheckpoint = reader.ReadChar();
        _level = (uint)(_levelCheckpoint >> 4);
        Debug.Log($"Level to load: {_level}");
        _checkpoint = (uint)(_levelCheckpoint & 0xF);
    }

    private static void LoadPlayerStatus(BinaryReader reader)
    {
        _score = (uint)reader.ReadInt32();
        _collectibles = (uint)reader.ReadInt32();
        _coins = _collectibles & 0xFFFFFF;
        _diamonds = _collectibles >> 24;
    }
    
    private static void OnSceneLoaded(AsyncOperation op)
    {
        Debug.Log("Loaded: " + SceneManager.GetActiveScene().name);
        UpdateObjectManager(ObjectManager.Instance);
        
        SetupPlayer();
        CheckpointManager.PutPlayerOnCheckpoint((int)_checkpoint);
        SetupGameObjects();
    }

    private static void SetupPlayer()
    {
//        _playerInventory.UpdateScore((int)_score);
        _playerInventory.UpdateCoinbar();
        _playerInventory.UpdateCollectibles(CoinType.Coin, (int)_coins);
        _playerInventory.UpdateCollectibles(CoinType.Diamond, (int)_diamonds);
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
                _objectManager.DestroyObjectN((int)savedObj.Index);
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
    
    private static void AddFloatToMeta(float value) => 
        _objectsMeta += unchecked((uint)BitConverter.SingleToInt32Bits(value));
}
