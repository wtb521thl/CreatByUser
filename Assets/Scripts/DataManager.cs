using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class DataManager : SingleMono<DataManager>
{

    public string GetDataFromFile(string _path,string _itemKey, string _key,string _startValue="" )
    {
        JsonData data = new JsonData();
        if (!File.Exists(_path))
        {
            SaveDataToFile(_itemKey, _key,_startValue, _path);
            return _startValue;
        }
        data = JsonMapper.ToObject(File.ReadAllText(_path));
        try
        {
            return data[_itemKey][_key].ToJson();
        }
        catch
        {
            SaveDataToFile(_itemKey,_key, _startValue, _path);
            return _startValue;
        }

    }

    public void SaveDataToFile(string _itemKey, string _key,string _value,string _path)
    {
        JsonData data = new JsonData();
        string dirStr = _path.Replace(_path.Split('/')[_path.Split('/').Length - 1], "");
        if (!Directory.Exists(dirStr)) 
        {
            Directory.CreateDirectory(dirStr);
        }
        if (File.Exists(_path))
        {
            data = JsonMapper.ToObject(File.ReadAllText(_path));
            try
            {
                data[_itemKey][_key] = _value;
            }
            catch
            {
                JsonData tempItemData = new JsonData();
                tempItemData[_key] = _value;
                data[_itemKey] = tempItemData;
            }
        }
        else
        {
            JsonData tempItemData = new JsonData();
            tempItemData[_key] = _value;
            data[_itemKey] = tempItemData;
        }
        File.WriteAllText(_path, data.ToJson());
    }

    public JsonData GetDataFromFile(string _path)
    {
        JsonData data = new JsonData();
        if (!File.Exists(_path))
        {
            SaveDataToFile(data, _path);
        }
        data = JsonMapper.ToObject(File.ReadAllText(_path));
        return data;
    }
    public void SaveDataToFile(JsonData data, string _path)
    {
        string dirStr = _path.Replace(_path.Split('/')[_path.Split('/').Length - 1], "");
        if (!Directory.Exists(dirStr))
        {
            Directory.CreateDirectory(dirStr);
        }
        File.WriteAllText(_path, data.ToJson());
    }


}
