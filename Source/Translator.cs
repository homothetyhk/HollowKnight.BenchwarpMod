using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;
using System.IO;

namespace Benchwarp
{
    public static class Translator
    {
        // Dictionaries for translation.
        private static Dictionary<string, string> TranslateDict = new Dictionary<string, string>();
        private static Dictionary<string, string> RevertDict = new Dictionary<string, string>();
        private static XmlDocument xml;
        private static bool DictActive;
        private static bool ReverseDictExists;

        /// <summary>
        /// Creates dictionary/ies for translation from file in saves folder.
        /// If file does not exist then dictionaries are not generated, and all routines default to returning their input parameter.
        /// </summary>
        public static void Initialize()
        {
            ReverseDictExists = false; // Change if you wish to reverse translation.

            DictActive = LoadXML();
            if (DictActive)
            {
                CreateDicts(ReverseDictExists);
            }
        }

        /// <summary>
        /// Opens the dictionary xml file located in the saves folder.
        /// </summary>
        /// <returns>true if file is located, false if file is not located.</returns>
        public static bool LoadXML()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, "TranslatorDictionary.xml")))
            {
                Benchwarp.instance.Log("XML not found. Please place TranslatorDictionary.xml into your saves folder.");
                return false;
            }
            Benchwarp.instance.Log("Translation XML loaded.");
            xml = new XmlDocument();
            xml.Load(Path.Combine(Application.persistentDataPath, "TranslatorDictionary.xml"));
            return true;
        }

        /// <summary>
        /// Creates the dictionaries used for translation.
        /// </summary>
        /// <param name="CreateInvertedDict">Whether to create an inverse dictionary for detranslation. Only set to true if your project requires it.</param>
        public static void CreateDicts(bool CreateInvertedDict)
        {
            foreach (XmlNode node in xml.DocumentElement.ChildNodes) // Iterate through each <entry> tag in dictionary.
            {
                TranslateDict.Add(node.SelectSingleNode("oldName").InnerText, node.SelectSingleNode("newName").InnerText);
                if (CreateInvertedDict == true)
                {
                    RevertDict.Add(node.SelectSingleNode("newName").InnerText, node.SelectSingleNode("oldName").InnerText);
                }
            }
        }

        /// <summary>
        /// Translates scene name to a more descriptive room name.
        /// </summary>
        /// <param name="oldName">Name of scene</param>
        /// <returns>A more descriptive room name if it exists in the dictionary, else oldName.</returns>
        public static string TranslateSceneName(string oldName)
        {
            string newName;
            if (TranslateDict.TryGetValue(oldName, out newName))
            {
                return newName;
            }
            return oldName;
        }

        /// <summary>
        /// Translates a Randomizer 3 transition name to a more descriptive name.
        /// </summary>
        /// <param name="oldName">Name of transition</param>
        /// <returns>More descriptive transition name if it exists in dictionary, else oldName.</returns>
        public static string TranslateTransitionName(string oldName)
        {
            if (!DictActive) { return oldName; }
            string[] transitionArray = oldName.Split('[', ']');
            return TranslateSceneName(transitionArray[0]) + '[' + transitionArray[1] + ']';
        }

        /// <summary>
        /// Reverses translation of a room name if ReverseDictExists was set to true.
        /// </summary>
        /// <param name="oldName">Translated room name.</param>
        /// <returns>Original scene name.</returns>
        public static string ReverseSceneTranslation(string oldName)
        {
            if (!ReverseDictExists || !DictActive) { return oldName; }

            string newName;
            if (TranslateDict.TryGetValue(oldName, out newName))
            {
                return newName;
            }
            return oldName;
        }

        /// <summary>
        /// Reverses translation of a transition name if ReverseDictExists was set to true.
        /// </summary>
        /// <param name="oldName">Translated transition name.</param>
        /// <returns>Original transition name.</returns>
        public static string ReverseTransitionTranslation(string oldName)
        {
            if (!DictActive || !ReverseDictExists) { return oldName; }
            string[] transitionArray = oldName.Split('[', ']');
            return TranslateSceneName(transitionArray[0]) + '[' + transitionArray[1] + ']';
        }
    }
}
