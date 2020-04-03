using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Chunker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new StreamReader(args[0]).ReadToEnd();
            var dataChunks = JsonConvert.DeserializeObject<List<DataChunk>>(data);

            foreach (var d in dataChunks)
            {
                CompileChunkList(d);
            }
        }

        private static void CompileChunkList(DataChunk dataChunk)
        {
            // We want to scrape the files from a particular folder, manipulate them slightly and then compile them into an xml document.
            var chunkList = XmlWriter.Create(dataChunk.Destination);
            chunkList.WriteStartDocument();

            chunkList.WriteStartElement("Chunks");

            // Create a qualified path for a particular chunk.
            foreach (var file in Directory.GetFiles(Path.GetFullPath(dataChunk.Folder)))
            {
                // Parse filename. It will always be in the form name000.oel
                var rawFileName = Path.GetFileNameWithoutExtension(file);
                if (rawFileName == null)
                {
                    throw new InvalidOperationException("rawFileName cannot be null");
                }

                var chunkType = rawFileName.Substring(0, rawFileName.Length - 3);
                
                chunkList.WriteStartElement("Chunk");
                chunkList.WriteAttributeString("type", chunkType);

                // Read the file then do some stuff to it.
                var doc = XDocument.Load(file);
                var levelNode = doc.Descendants("level").First();
                foreach (var n in levelNode.Descendants("Layout"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Geometry"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Background"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Foreground"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("UpDoodad"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("DownDoodad"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Doodads"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Platforms"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Props"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Triggers"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Items"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Hazards"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                foreach (var n in levelNode.Descendants("Monsters"))
                {
                    chunkList.WriteRaw(n.ToString());
                }

                chunkList.WriteEndElement();
            }

            chunkList.WriteEndElement();

            chunkList.Close();
        }
    }
}
