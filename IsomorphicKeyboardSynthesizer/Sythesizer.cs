using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace IsomorphicKeyboardSynthesizer
{
    public partial class Sythesizer : Form
    {
        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;
        /// <summary>
        /// Max amplitude for 16 bit audio
        /// </summary>
        private const int AMPLITUDE = 32760; 

        public Sythesizer()
        {
            InitializeComponent();
        }

        private void Sythesizer_KeyDown(object sender, KeyEventArgs e)
        {
            short[] wave = new short[SAMPLE_RATE];

            float frequency = 440f;

            for (int i = 0; i < SAMPLE_RATE; i++)
            {
                wave[i] = Convert.ToInt16(AMPLITUDE * Math.Sin((Math.PI * 2 * frequency) / SAMPLE_RATE * i));
            }

            using (MemoryStream memory = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(memory))
            {
                short blockAlign = BITS_PER_SAMPLE / 8;
                int subChunk2Size = SAMPLE_RATE * blockAlign;

                // WAVE format 
                // header
                binaryWriter.Write(new[] { 'R', 'I', 'F', 'F' }); //ChunkID
                binaryWriter.Write(36 + subChunk2Size); //ChunkSize
                binaryWriter.Write(new[] { 'W', 'A', 'V', 'E' }); // format type
                // format Subchunk
                binaryWriter.Write(new[] { 'f', 'm', 't', ' ' }); // Subchunk1ID
                binaryWriter.Write(16); // SubChunk1Size
                binaryWriter.Write((short)1); // Audio format 1 for PCM (2 bytes)
                binaryWriter.Write((short)1); // Channels (2 bytes)
                binaryWriter.Write(SAMPLE_RATE);
                binaryWriter.Write(SAMPLE_RATE * blockAlign); // ByteRate
                binaryWriter.Write(blockAlign);
                binaryWriter.Write(BITS_PER_SAMPLE);
                // data Subchunk
                binaryWriter.Write(new[] { 'd', 'a', 't', 'a' }); //Subchunk2ID
                binaryWriter.Write(subChunk2Size);
                foreach (short sample in wave)
                {
                    binaryWriter.Write(sample);
                }
                memory.Position = 0;
                new SoundPlayer(memory).Play();
            }
        }
    }
}
