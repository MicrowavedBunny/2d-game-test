using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

	public static void savePlayer(CharacterController2D player)
	{
		//allows for binary conversion
		BinaryFormatter formatter = new BinaryFormatter();

		//save location and file name
		string path = Application.persistentDataPath + "/player.sav";

		//Write file
		FileStream stream = new FileStream(path, FileMode.Create);

		//uses constructor to create object of class PlayerData
		PlayerData data = new PlayerData(player);

		//convert data to binary
		formatter.Serialize(stream, data);

		//close stream
		stream.Close();
	}

	public static PlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + "/player.sav";

		if (File.Exists(path))
		{
			//allows for binary conversion
			BinaryFormatter formatter = new BinaryFormatter();

			//read file
			FileStream stream = new FileStream(path, FileMode.Open);
			
			//convert data back from binary
			PlayerData data = formatter.Deserialize(stream) as PlayerData;

			//close stream
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}

}
