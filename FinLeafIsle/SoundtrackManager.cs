using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Autofac;

namespace FinLeafIsle
{
    public class SoundtrackManager
    {
        public float Volume = 1f;
        private ContentManager _content;
        public SoundEffect MysticGrove { get; private set; }
        public SoundEffectInstance _m1 { get; private set; }
       
        public SoundtrackManager(ContentManager content)
        {
            _content = content;

            MysticGrove = _content.Load<SoundEffect>("OST/MysticGrove");
            _m1 = MysticGrove.CreateInstance();
            
        }

        public void IncVolume()
        {
            if (Volume + 0.2f <= 1f)
            {
                Volume += 0.2f;

                _m1.Volume = Volume;
            }

        }

        public void DecVolume()
        {
            if (Volume - 0.2f >= 0f)
            {
                Volume -= 0.2f;

                _m1.Volume = Volume;
            }

        }
        public float GetVolume()
        {
            return Volume;
        }
    }
}
