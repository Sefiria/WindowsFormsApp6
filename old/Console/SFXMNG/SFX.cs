using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFXMNG
{
    public class SFX
    {
        static public void Play(string sample)
        {
            Note[] notes = Note.ParseSample(sample);
            foreach (Note note in notes)
                Console.Beep((int)note.tone * note.octave, (int)note.duration);
        }

        public enum Tone
        {
            GbelowC = 98,
            A = 110,
            Asharp = 116,
            B = 123,
            C = 131,
            Csharp = 138,
            D = 147,
            Dsharp = 155,
            E = 165,
            F = 174,
            Fsharp = 185,
            G = 196,
            Gsharp = 207
        }
        public enum Duration
        {
            A = 50,
            B = 100,
            C = 250,
            D = 500,
            E = 1000,
            F = 2000,
            G = 5000
        }
        public class Note
        {
            public Tone tone;
            public int octave;
            public Duration duration;
            public Note(Tone tone, int octave, Duration duration)
            {
                this.tone = tone;
                this.octave = octave;
                this.duration = duration;
            }
            static public Note ParseNote(string note)
            {
                int ofs = 0; 
                string n = $"{note[0]}";
                Tone tone;

                if (string.Concat(note.Take(2)) == "G@")
                {
                    tone = Tone.GbelowC;
                    ofs++;
                }
                else
                {
                    if (note[1] == '#')
                    {
                        if (!new List<char>() { 'B', 'E' }.Contains(note[0]))
                        {
                            n += "sharp";
                            ofs++;
                        }
                        else
                        {
                            note = note.Remove(1, 1);
                        }
                    }

                    tone = (Tone)Enum.Parse(typeof(Tone), n);
                }

                int octave = int.Parse(note[1 + ofs].ToString());
                Duration duration = (Duration)Enum.Parse(typeof(Duration), note[2 + ofs].ToString());

                return new Note(tone, octave, duration);
            }
            static public Note[] ParseSample(string sample)
            {
                string[] infos = sample.Split(' ');
                List<Note> notes = new List<Note>();
                
                foreach(string info in infos)
                {
                    notes.Add(ParseNote(info));
                }

                return notes.ToArray();
            }
        }


        public static void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++)
                {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }
            tert
            mStrm.Seek(0, SeekOrigin.Begin);
            new System.Media.SoundPlayer(mStrm).Play();
            writer.Close();
            mStrm.Close();

            Thread.Sleep(msDuration);
        }
    }
}
