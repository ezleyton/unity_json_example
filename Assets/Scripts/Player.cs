using System;
using System.IO;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum SerializationMode
    {
        Class,
        Structure
    }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private SerializationMode serializationMode = SerializationMode.Class;
    private string _json;

    // Start is called before the first frame update
    void Start()
    {
        LoadFile();
        if (_json != null)
        {
            SerializeBack();
            //JsonUtility.FromJsonOverwrite(_json, transform); //engineTypes cannot be overwritten from json
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (serializationMode == SerializationMode.Class)
            {
                SaveData data = new SaveData(transform.position, transform.rotation);
                _json = JsonUtility.ToJson(data);
                SaveFile();
                Debug.Log(_json);
            }
            else
            {
                PlayerData playerData = new PlayerData(transform.position, transform.rotation);
                _json = JsonUtility.ToJson(playerData);
                SaveFile();
                Debug.Log(_json);
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            SerializeBack();
        }
    }

    private void SerializeBack()
    {
        if (serializationMode == SerializationMode.Class)
        {
            SaveData data = JsonUtility.FromJson<SaveData>(_json);
            transform.position = data.GetPosition();
            transform.rotation = data.rotation;
        }
        else
        {
            PlayerData data = JsonUtility.FromJson<PlayerData>(_json);
            transform.position = data.Position;
            transform.rotation = data.Rotation;
                
        }
    }

    private void SaveFile()
    {
        File.WriteAllText($"{serializationMode.ToString()}", _json, Encoding.UTF8);
    }

    private void LoadFile()
    {
        Debug.Log("Checking if file exists");
        if (File.Exists($"{serializationMode.ToString()}"))
        {
            Debug.Log("File found, loading data");
            _json = File.ReadAllText($"{serializationMode.ToString()}", Encoding.UTF8);
            Debug.Log($"Data loaded: {_json}");
        }
    }


    [Serializable] //parece no tener efecto dejarlo o sacarlo, a pesar de que la doc dice que es necesario
    private struct PlayerData
    {
        [SerializeField]
        private Vector3 position; //si o si es necesario para private, con [Serializable] o no a nivel estructura

        [SerializeField] private Quaternion rotation;


        public PlayerData(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 Position => position;
        public Quaternion Rotation => rotation;
    }

    public class SaveData
    {
        //Fields that have unsupported types, as well as private fields or fields marked with the NonSerialized attribute, will be ignored
        [SerializeField] private Vector3 position;
        public Quaternion rotation;
        protected string protectedString = "THIS_IS_A_PROTECTED_VALUE";
        [SerializeField] protected string protectedStringSerialized = "THIS_IS_A_PROTECTED_SERIALIZED_VALUE";
        private string privateString = "THIS_IS_A_PRIVATE_STRING_SHOULDNT_BE_SERIALIZED";

        public SaveData(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 GetPosition()
        {
            return position;
        }
    }
}