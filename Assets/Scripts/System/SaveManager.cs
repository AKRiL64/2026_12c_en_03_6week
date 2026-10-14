using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    private static readonly string SaveDirectory = "Saves";
    private static readonly int LevelZero = 1;
    private static ObjectManager _objectManager;
    private static PlayerInventory _playerInventory;
    
    private static ulong _checksum = 0,
        _objectMeta = 0;
    

    private static void SetPlayerInventory(PlayerInventory playerInventory)
    {
        _playerInventory = playerInventory;
    }
    
    public static void UpdateObjectManager(ObjectManager objectManager)
    {
        _objectManager = ObjectManager.Instance;
    }

    public static bool Save(int checkpoint) // checkpoint number
    {
        int score = _playerInventory.Score, 
            coins = _playerInventory.CollectiblesCount[CoinType.Coin],
            diamonds = _playerInventory.CollectiblesCount[CoinType.Diamond];
        
        int level = (char)(SceneManager.GetActiveScene().buildIndex - LevelZero);
        char levelCheckpoint = (char)((level << 4) | checkpoint);
        
        _checksum = 0;

        try
        {
            using (BinaryWriter writer =
                   new BinaryWriter(File.Open(SaveDirectory + DateTime.Now.Date, FileMode.OpenOrCreate)))
            {
                WritePlayerStatus(writer);
                writer.Write(levelCheckpoint);

                int objectsNumber = _objectManager.objects.Length;
                writer.Write(objectsNumber);

                for (int i = 0; i < objectsNumber; i++)
                    if (_objectManager.objects[i] != null)
                        WriteObject(i, writer);
                    else
                        writer.Write(0);

                _checksum = _checksum * (ulong)(levelCheckpoint + objectsNumber + 1) + (ulong)(coins * (diamonds + 1) + score);
                writer.Write(_checksum);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    private static void WriteObject(int index, BinaryWriter writer)
    {
        int objectStatus = 0;
        GameObject obj = _objectManager.objects[index];

        objectStatus = GetObjectStatus(obj, index);
        writer.Write(objectStatus);

        WriteTransform(obj, writer);

        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            WriteSpeed(rb, writer);
    }

    private static void WritePlayerStatus(BinaryWriter writer)
    {
        int score = _playerInventory.Score, 
            coins = _playerInventory.CollectiblesCount[CoinType.Coin],
            diamonds = _playerInventory.CollectiblesCount[CoinType.Diamond];
        
        int collectibles = (coins << 16) | diamonds;
        writer.Write(score);
        writer.Write(collectibles);
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

        _checksum += (ulong)(objTransform.position.x + objTransform.position.y + objTransform.position.z +
                             objTransform.rotation.eulerAngles.x + objTransform.rotation.eulerAngles.y + 
                             objTransform.rotation.eulerAngles.z);
    }

    private static void WriteSpeed(Rigidbody2D rb, BinaryWriter writer)
    {
        writer.Write(rb.linearVelocityX);
        writer.Write(rb.linearVelocityY);
        writer.Write(rb.angularVelocity);
        
        _checksum += (ulong)(rb.linearVelocityX + rb.linearVelocityY + rb.angularVelocity);
    }

    private static int GetObjectStatus(GameObject obj, int index)
    {
        int objectStatus = 0;
        objectStatus |= 1 << 31;
        if (obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            objectStatus |= 1 << 30;
            objectStatus |= index;
        }
        return objectStatus;
    }
    
    public static bool Load(string filepath)
    {
        _checksum = 0;
        _objectMeta = 0;
        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open)))
            {
                LoadPlayerStatus(reader); // NOTE: player must be present on the scene(add the player to the main menu?)
                LoadLevel(reader);
                UpdateObjectManager(ObjectManager.Instance);
                
                int objectsNumber = reader.ReadInt32();
                for (int i = 0; i < objectsNumber; i++)
                    LoadObject(reader);
                
                ulong checksum = reader.ReadUInt64();
                if (checksum != _checksum)
                {
                    SceneManager.LoadScene(0);
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            SceneManager.LoadScene(0);
            return false;
        }
        return true;
    }

    private static void LoadObject(BinaryReader reader)
    {
        int objectStatus = reader.ReadInt32(),
            exists = objectStatus >> 31,
            index = objectStatus & 0x3FFFFFFF;
        _objectMeta += (ulong)objectStatus;
        
        GameObject obj = _objectManager.objects[index];
        if (exists == 0)
        {
            obj.SetActive(false);
            return;
        }
        
        float posX = reader.ReadSingle(), posY = reader.ReadSingle(), posZ = reader.ReadSingle(),
            rotX = reader.ReadSingle(), rotY = reader.ReadSingle(), rotZ = reader.ReadSingle();

        obj.transform.position = new Vector3(posX, posY, posZ);
        obj.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
        
        int hasRigidbody = (objectStatus << 1) >> 31;
        if (hasRigidbody == 1)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            float linearSpeedX = reader.ReadSingle(), linearSpeedY = reader.ReadSingle(), 
                angularSpeed = reader.ReadSingle();
            rb.linearVelocity = new Vector2(linearSpeedX, linearSpeedY);
            rb.angularVelocity = angularSpeed;
            
            _checksum += (ulong)(posX + posY + posZ + rotX + rotY + rotZ + linearSpeedX + linearSpeedY + angularSpeed);
        }
        else
            _checksum += (ulong)(posX + posY + posZ + rotX + rotY + rotZ);
    }

    private static void LoadLevel(BinaryReader reader)
    {
        char levelCheckpoint = reader.ReadChar();
        int level = levelCheckpoint >> 4;
        int checkpoint = levelCheckpoint & 0xF;

        _checksum += (ulong)(levelCheckpoint + 1);
        
        SceneManager.LoadScene(level);
        // call some checkpoint manager to move the player
    }

    private static void LoadPlayerStatus(BinaryReader reader)
    {
        _playerInventory.UpdateScore(reader.ReadInt32());
        int collectibles = reader.ReadInt32();
        int coins = collectibles & 0xFFFFFF;
        int diamonds = collectibles >> 16;
        _checksum = (ulong)(coins * (diamonds + 1));
        _playerInventory.UpdateCollectibles(CoinType.Coin, coins);
        _playerInventory.UpdateCollectibles(CoinType.Diamond, diamonds);
    }
}
