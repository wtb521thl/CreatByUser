using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class DataManager : SingleMono<DataManager>
{
    JsonData data;
    public string GetDataFromFile(string _path,string _key,string _startValue="" )
    {
        data = new JsonData();
        if (!File.Exists(_path))
        {
            data[_key] = _startValue;
            SaveDataToFile(_key,_startValue, _path);
            return _startValue;
        }
        data = JsonMapper.ToObject(File.ReadAllText(_path));
        try
        {
            return data[_key].ToJson();
        }
        catch
        {
            data[_key] = _startValue;
            SaveDataToFile(_key, _startValue, _path);
            return _startValue;
        }

    }

    public void SaveDataToFile(string _key,string _value,string _path)
    {
        string dirStr = _path.Replace(_path.Split('/')[_path.Split('/').Length - 1], "");
        if (!Directory.Exists(dirStr)) 
        {
            Directory.CreateDirectory(dirStr);
        }
        if (File.Exists(_path))
        {
            data = JsonMapper.ToObject(File.ReadAllText(_path));
            data[_key] = _value;
        }
        else
        {
            data = new JsonData();
            data[_key] = _value;
        }
        File.WriteAllText(_path, data.ToJson());
    }
}
