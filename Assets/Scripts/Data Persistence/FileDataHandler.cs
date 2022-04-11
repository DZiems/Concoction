using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{

    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //use path.combine to account for different OSs having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    dataToLoad = reader.ReadToEnd();
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from file: {fullPath}\n{e}");
            }

        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //use path.combine to account for different OSs having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //create the directory if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize c# gamedata object to json
            string dataToStore = JsonUtility.ToJson(data, true);

            //write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to save data to file: {fullPath}\n{e}");

        }
    }
}
