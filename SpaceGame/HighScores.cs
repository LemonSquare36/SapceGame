using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace SpaceGame
{
    /// <summary>
    /// Holds high scores and names.
    /// </summary>
    public class ScoreBoard
    {
        string[] textHighScores1, textHighScores2, encryptedHighScores1,
            encryptedHighScores2, textNames1, textNames2,
            encryptedNames1, encryptedNames2;
        int numOneScore = 0, dummy;
        string fileName, rootElement;
        bool boolWorkingFileIO = true;
        XmlDocument scoresWrite, scoresRead;

        public string[] HighScores
        {
            get { return textHighScores1; }
        }

        public string[] TopNames
        {
            get { return textNames1; }
        }

        public int getNumOneScore() { return numOneScore; }

        /// <summary>
        /// Holds high score records.
        /// </summary>
        /// <param name="fileName">Name of file to be saved to minus the extension.</param>
        /// <param name="rootElement">Name of Root level Xml Element.</param>
        /// <param name="length">Number of entries long the score board is.</param>
        public ScoreBoard(string fileName, string rootElement, int length)
        {
            this.fileName = fileName;
            this.rootElement = rootElement;
            textHighScores1 = new string[length];
            textHighScores2 = new string[length];
            encryptedHighScores1 = new string[length];
            encryptedHighScores2 = new string[length];
            textNames1 = new string[length];
            textNames2 = new string[length];
            encryptedNames1 = new string[length];
            encryptedNames2 = new string[length];
        }

        /// <summary>
        /// Retrieves and decrypts high score records.
        /// </summary>
        public void retrieveScores()
        {
            boolWorkingFileIO = true;

            try
            {
                scoresRead = new XmlDocument();
                scoresRead.Load(fileName + ".xml");

                for (int i = 0; i < textHighScores1.Length; i++)
                {
                    encryptedHighScores1[i] = scoresRead.SelectSingleNode("/" + rootElement + "/Score" + (i + 1)).Attributes["Score"].Value;
                    encryptedNames1[i] = scoresRead.SelectSingleNode("/" + rootElement + "/Score" + (i + 1)).Attributes["Name"].Value;

                    if (encryptedHighScores1[i] != "")
                    {
                        textHighScores1[i] = StringCipher.Decrypt(encryptedHighScores1[i], "G5bM(1inE|`JT(@GX5?:O=<*t<_EgB");
                    }
                    else
                    {
                        textHighScores1[i] = "0";
                    }

                    if (encryptedNames1[i] != "")
                    {
                        textNames1[i] = StringCipher.Decrypt(encryptedNames1[i], "US1qeI3{s%XfLP911(zckW3T)-C70F");
                    }
                    else
                    {
                        textNames1[i] = "FDR";
                    }

                    if (textHighScores1[i] == "")
                    {
                        textHighScores1[i] = "0";
                    }
                }
                scoresRead.Save(fileName + ".xml");
                numOneScore = Convert.ToInt32(textHighScores1[0]);
            }
            catch
            {
                Console.WriteLine("No high score file was found");
                Console.WriteLine("Generating " + fileName + ".xml");
                boolWorkingFileIO = false;
                XmlDocument create = new XmlDocument();
                XmlNode rootNode = create.CreateElement(rootElement);
                create.AppendChild(rootNode);

                for (int i = 1; i < 11; i++)
                {
                    XmlNode userNode = create.CreateElement("Score" + i);
                    XmlAttribute attribute = create.CreateAttribute("Score");
                    attribute.Value = "";
                    userNode.Attributes.Append(attribute);
                    XmlAttribute nameAttribute = create.CreateAttribute("Name");
                    nameAttribute.Value = "";
                    userNode.Attributes.Append(nameAttribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save(fileName + ".xml");
            }
        }

        /// <summary>
        /// Encrypts and records high score records.
        /// </summary>
        /// <param name="score">New score to compare.</param>
        /// <param name="name">Name of player.</param>
        public void recordScore(int score, string name)
        {
            boolWorkingFileIO = true;

            retrieveScores();

            //If there are no file problems continue
            if (boolWorkingFileIO)
            {
                for (int i = 0, j = 0; i < textHighScores2.Length; i++, j++)
                {
                    if (score > Convert.ToInt32(textHighScores1[i]) && i == j)
                    {
                        textHighScores2[i] = score.ToString();
                        textNames2[i] = name.ToString();
                        i++;
                        if (i < textHighScores2.Length)
                        {
                            textHighScores2[i] = textHighScores1[j];
                            textNames2[i] = textNames1[j];
                        }
                    }
                    else
                    {
                        textHighScores2[i] = textHighScores1[j];
                        textNames2[i] = textNames1[j];
                    }
                }
            }

            //Write the new scores to the file
            try
            {
                dummy = 1;

                while (dummy == 1)
                {
                    scoresWrite = new XmlDocument();

                    scoresWrite.Load(fileName + ".xml");

                    for (int i = 0; i < textHighScores2.Length; i++)
                    {
                        encryptedHighScores2[i] = StringCipher.Encrypt(textHighScores2[i], "G5bM(1inE|`JT(@GX5?:O=<*t<_EgB");
                        encryptedNames2[i] = StringCipher.Encrypt(textNames2[i], "US1qeI3{s%XfLP911(zckW3T)-C70F");
                        scoresWrite.SelectSingleNode("/" + rootElement + "/Score" + (i + 1)).Attributes["Score"].Value = encryptedHighScores2[i];
                        scoresWrite.SelectSingleNode("/" + rootElement + "/Score" + (i + 1)).Attributes["Name"].Value = encryptedNames2[i];
                    }
                    scoresWrite.Save(fileName + ".xml");
                    dummy = 0;
                }
            }
            catch
            {
                Console.WriteLine("No high score file was found");
                Console.WriteLine("Generating " + fileName + ".xml");
                boolWorkingFileIO = false;
                XmlDocument create = new XmlDocument();
                XmlNode rootNode = create.CreateElement(rootElement);
                create.AppendChild(rootNode);

                for (int i = 1; i < 11; i++)
                {
                    XmlNode userNode = create.CreateElement("Score" + i);
                    XmlAttribute attribute = create.CreateAttribute("Score");
                    attribute.Value = "";
                    userNode.Attributes.Append(attribute);
                    XmlAttribute nameAttribute = create.CreateAttribute("Name");
                    nameAttribute.Value = "";
                    userNode.Attributes.Append(nameAttribute);
                    rootNode.AppendChild(userNode);
                }
                create.Save(fileName + ".xml");
            }
        }
    }
}