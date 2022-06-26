using System;
using System.IO;
using System.Collections.Generic;
namespace CustomConfig
{
    public class CConfig
    {

        Dictionary<string, ValuePair> _BaseConfigData;

        /// <summary>
        /// Create a blank Config File
        /// </summary>
        public CConfig()
        {
            _BaseConfigData = new Dictionary<string, ValuePair>();
        }

        /// <summary>
        /// Load from a pre-built file
        /// </summary>
        /// <param name="filepath"></param>
        public CConfig(string filepath)
        {
            _BaseConfigData = new Dictionary<string, ValuePair>();
            DecodeDataFromFile(filepath);
        }

        /// <summary>
        /// Save modified data in this instance to a file
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveData(string filepath,bool overwrite=true)
        {
            if (File.Exists(filepath) && overwrite == false) { return; }
            
            // make path ready
            if(Directory.Exists(Path.GetDirectoryName(filepath)) ==false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }
           
            var encodeddata = EncodeDataToArray();
            File.WriteAllLines(filepath, encodeddata);
        }

        /// <summary>
        /// Set a Config varriable. 
        /// Don't put any space in key name
        /// </summary>
        /// <param name="key">No Space</param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        public void SetVar(string key,string value)
        {
            var refinedkey = key;
            if (refinedkey.StartsWith("#") == false) { refinedkey = "#" + refinedkey; }
            if (_BaseConfigData.ContainsKey(refinedkey))
            {
                _BaseConfigData[refinedkey].Value = value;
            }
            else
            {
                _BaseConfigData[refinedkey] = new ValuePair(value, "");
            }
            
        }

        /// <summary>
        /// Set a Config variable. comment provided
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        public void SetVar(string key, string value,string comment)
        {
            var refinedkey = key;
            if (refinedkey.StartsWith("#") == false) { refinedkey = "#" + refinedkey; }
            if (_BaseConfigData.ContainsKey(refinedkey))
            {
                _BaseConfigData[refinedkey].Value = value;
                _BaseConfigData[refinedkey].Comment = comment;
            }
            else
            {
                _BaseConfigData[refinedkey] = new ValuePair(value, comment);
            }
        }

        /// <summary>
        /// Read a config variable.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadVar(string key)
        {
            var refinedkey = key;
            if (refinedkey.StartsWith("#") == false) { refinedkey = "#" + refinedkey; }
            if (_BaseConfigData.ContainsKey(refinedkey))
            {
                return _BaseConfigData[refinedkey].Value;
            }
            return null;
        }

        /// <summary>
        /// Read a config variable comment.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadComment(string key)
        {
            var refinedkey = key;
            if (refinedkey.StartsWith("#") == false) { refinedkey = "#" + refinedkey; }
            if (_BaseConfigData.ContainsKey(refinedkey))
            {
                return _BaseConfigData[refinedkey].Comment;
            }
            return null;
        }


        /// <summary>
        /// Load data from file and save them into _BaseConfigData
        /// </summary>
        /// <param name="filepath"></param>
        private void DecodeDataFromFile(string filepath)
        {
            var filecontentarray = File.ReadAllLines(filepath);
            for (int i = 0; i < filecontentarray.Length; i++)
            {
                if(filecontentarray[i].StartsWith("#") && filecontentarray.Length>(i+1))
                {
                    var Extractedvalues = ReadUntilSpace(filecontentarray[i]);
                    var key = Extractedvalues[0].Trim();
                    _BaseConfigData[key] = new ValuePair(filecontentarray[i + 1], Extractedvalues[1]);
                    i++;
                }
            }
        }

        private string[] EncodeDataToArray()
        {
            var listdata = new List<string>();

            foreach (var item in _BaseConfigData)
            {
                listdata.Add(item.Key + " " + item.Value.Comment);
                listdata.Add(item.Value.Value);
            }

            return listdata.ToArray();
        }

        private string[] ReadUntilSpace(string text)
        {
            var spacepos = text.Trim().IndexOf(" ");
            var firstpart = text.Trim().Substring(0,spacepos);
            var secondpart = text.Trim().Substring(spacepos).Trim();

            return new string[] { firstpart, secondpart };
        }
    }

    internal class ValuePair
    {
        public string Value { get; set; }
        public string Comment { get; set; }

        public ValuePair(string value,string comment)
        {
            this.Value = value;
            this.Comment = comment;
        }
    }
}
