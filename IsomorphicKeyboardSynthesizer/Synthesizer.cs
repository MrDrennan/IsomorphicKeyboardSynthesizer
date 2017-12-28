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
    public partial class Synthesizer : Form
    {
        private const int SAMPLE_RATE = 44100;
        private const short BITS_PER_SAMPLE = 16;
        /// <summary>
        /// Max amplitude for 16 bit audio
        /// </summary>
        private const int AMPLITUDE = 32760;
        private const byte CHANNELS = 1;
        private const uint NUM_SAMPLES = SAMPLE_RATE * CHANNELS;
         
        public Synthesizer()
        {
            InitializeComponent();
        }

        private void Synthesizer_KeyDown(object sender, KeyEventArgs e)
        {
            short[] wave = new short[SAMPLE_RATE];

            float[] frequencies = new float[38]; // range of used frequencies
            byte startNote = 28; // the key number to start at on a piano
            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] = (float)Math.Pow((Math.Pow(2, 1.0 / 12)), (i + startNote) - 49) * 440;
            }

            float currFrequency;
            
            // notes based on 440hz default
            // indexes 1, 3, 34, 36 are not on keyboard so they are ignored 
            switch (e.KeyCode)
            {
                case Keys.D1:
                    currFrequency = frequencies[15]; //D#4
                    break;
                case Keys.D2:
                    currFrequency = frequencies[17]; //F4
                    break;
                case Keys.D3:
                    currFrequency = frequencies[19]; // G4
                    break;
                case Keys.D4:
                    currFrequency = frequencies[21]; // A4
                    break;
                case Keys.D5:
                    currFrequency = frequencies[23]; // B4
                    break;
                case Keys.D6:
                    currFrequency = frequencies[25]; // C#5
                    break;
                case Keys.D7:
                    currFrequency = frequencies[27]; // D#5
                    break;
                case Keys.D8:
                    currFrequency = frequencies[29]; // F5
                    break;
                case Keys.D9:
                    currFrequency = frequencies[31]; // G5
                    break;
                case Keys.D0:
                    currFrequency = frequencies[33]; // A5
                    break;
                case Keys.OemMinus:
                    currFrequency = frequencies[35]; // B5
                    break;
                case Keys.Oemplus:
                    currFrequency = frequencies[37]; // C#6
                    break;
                case Keys.Q:
                    currFrequency = frequencies[10]; // A#3
                    break;
                case Keys.W:
                    currFrequency = frequencies[12]; // C4
                    break;
                case Keys.E:
                    currFrequency = frequencies[14]; // D4
                    break;
                case Keys.R:
                    currFrequency = frequencies[16]; // E4
                    break;
                case Keys.T:
                    currFrequency = frequencies[18]; // F#4
                    break;
                case Keys.Y:
                    currFrequency = frequencies[20]; // G#4
                    break;
                case Keys.U:
                    currFrequency = frequencies[22]; // A#4
                    break;
                case Keys.I:
                    currFrequency = frequencies[24]; // C5
                    break;
                case Keys.O:
                    currFrequency = frequencies[26]; // D5
                    break;
                case Keys.P:
                    currFrequency = frequencies[28]; // E5
                    break;
                case Keys.OemOpenBrackets:
                    currFrequency = frequencies[30]; // F#5
                    break;
                case Keys.OemCloseBrackets:
                    currFrequency = frequencies[32]; // G#5
                    break;
                case Keys.A:
                    currFrequency = frequencies[5]; // F3
                    break;
                case Keys.S:
                    currFrequency = frequencies[7]; // G3
                    break;
                case Keys.D:
                    currFrequency = frequencies[9]; // A3
                    break;
                case Keys.F:
                    currFrequency = frequencies[11]; // B3
                    break;
                case Keys.G:
                    currFrequency = frequencies[13]; // C#4
                    break;
                case Keys.H:
                    currFrequency = frequencies[15]; // D#4
                    break;
                case Keys.J:
                    currFrequency = frequencies[17]; // F4
                    break;
                case Keys.K:
                    currFrequency = frequencies[19]; // G4
                    break;
                case Keys.L:
                    currFrequency = frequencies[21]; // A4
                    break;
                case Keys.OemSemicolon:
                    currFrequency = frequencies[23]; // B4
                    break;
                case Keys.OemQuotes:
                    currFrequency = frequencies[25]; // C#5
                    break;
                case Keys.Z:
                    currFrequency = frequencies[0]; // C3
                    break;
                case Keys.X:
                    currFrequency = frequencies[2]; // D3
                    break;
                case Keys.C:
                    currFrequency = frequencies[4]; // E3
                    break;
                case Keys.V:
                    currFrequency = frequencies[6]; // F#3
                    break;
                case Keys.B:
                    currFrequency = frequencies[8]; // G#3
                    break;
                case Keys.N:
                    currFrequency = frequencies[10]; // A#3
                    break;
                case Keys.M:
                    currFrequency = frequencies[12]; // C4
                    break;
                case Keys.Oemcomma:
                    currFrequency = frequencies[14]; // D4
                    break;
                case Keys.OemPeriod:
                    currFrequency = frequencies[16]; // E4
                    break;
                case Keys.OemQuestion:
                    currFrequency = frequencies[18]; // F#4
                    break;
                default:
                    return;
            }

            foreach (Oscillator oscillator in Controls.OfType<Oscillator>())
            {
                int samplesPerWavelength = Convert.ToInt32(SAMPLE_RATE / (currFrequency / CHANNELS));
                short ampStep = Convert.ToInt16((AMPLITUDE * 2) / samplesPerWavelength);

                switch (oscillator.WaveForm)
                {
                    case WaveForm.Sine:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            wave[i] = Convert.ToInt16(AMPLITUDE * Math.Sin((Math.PI * 2 * currFrequency) / SAMPLE_RATE * i));
                        }
                        break;
                    case WaveForm.Square:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            wave[i] = Convert.ToInt16(AMPLITUDE * Math.Sign(Math.Sin((Math.PI * 2 * currFrequency) / SAMPLE_RATE * i)));
                        }
                        break;
                    case WaveForm.Saw:
                        short tempSample;
                        for (int i = 0; i < NUM_SAMPLES; i++)
                        {
                            tempSample = (short)-AMPLITUDE;
                            for (uint j = 0; j < samplesPerWavelength && i < NUM_SAMPLES; j += CHANNELS)
                            {
                                tempSample += ampStep;
                                for (int channel = 0; channel < CHANNELS && i < NUM_SAMPLES; channel++)
                                {
                                    wave[i++] = Convert.ToInt16(tempSample);
                                }
                            }
                            i--;
                        }
                        break;
                    case WaveForm.Triangle:
                        tempSample = (short)-AMPLITUDE;
                        for (int i = 0; i < NUM_SAMPLES - CHANNELS; i += CHANNELS)
                        {
                            for (int channel = 0; channel < CHANNELS; channel++)
                            {
                                // Negate ampstep whenever it hits the amplitude boundary
                                if (Math.Abs(tempSample + ampStep) > AMPLITUDE)
                                {
                                    ampStep = (short)-ampStep;
                                }
                                tempSample += ampStep;
                                wave[i + channel] = Convert.ToInt16(tempSample);
                            }
                        }
                        break;
                    case WaveForm.Noise:
                        Random rnd = new Random();
                        for (int i = 0; i < NUM_SAMPLES; i++)
                        {
                            wave[i] = Convert.ToInt16(rnd.Next(-AMPLITUDE, AMPLITUDE));
                        }
                        break;
                }
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

    public enum WaveForm
    {
        Sine, Square, Saw, Triangle, Noise
    }
}
