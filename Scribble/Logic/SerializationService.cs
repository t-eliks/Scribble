namespace Scribble.Logic
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows;

    public class SerializationService<T>
    {
        public static void SerializeObject(T obj, string directory, string filename)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(String.Concat(directory, "\\", filename), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public static T DeserializeObject(string path)
        {
            if (File.Exists(path))
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return (T)formatter.Deserialize(stream);
                }

                //try
                //{
                    
                //}
                //catch
                //{
                //    MessageBox.Show("Deserialization error.", "Oops", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
            return default(T);
        }

        public static ObservableCollection<T> DeserializeDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                string[] files = Directory.GetFiles(directory);

                ObservableCollection<T> deserializedfiles = new ObservableCollection<T>();

                foreach (var file in files)
                {
                    deserializedfiles.Add((T)DeserializeObject(file));
                }

                return deserializedfiles;
            }

            return null;
        }
    }
}
