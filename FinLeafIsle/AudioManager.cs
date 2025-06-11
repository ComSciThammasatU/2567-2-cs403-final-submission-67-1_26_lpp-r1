using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Autofac;

namespace FinLeafIsle
{
    public class AudioManager
    {
        public float Volume = 1f;
        private ContentManager _content;
        public SoundEffect ChargeSound { get; private set; }
        public SoundEffectInstance _chargeS { get; private set; }
        public SoundEffect CastingSound { get; private set; }
        public SoundEffectInstance _castS { get; private set; }
        public SoundEffect StepingSound { get; private set; }
        public SoundEffectInstance _stepingS { get; private set; }
        public SoundEffect HookedSound { get; private set; }
        public SoundEffectInstance _hookedS { get; private set; }
        public SoundEffect ReelInSound { get; private set; }
        public SoundEffectInstance _reelInS { get; private set; }
        public SoundEffect ReelOutSound { get; private set; }
        public SoundEffectInstance _reelOutS { get; private set; }
        public SoundEffect CatchSound { get; private set; }
        public SoundEffectInstance _catchS { get; private set; }
        public SoundEffect PressSound { get; private set; }
        public SoundEffectInstance _pressS { get; private set; }
        public SoundEffect ProgessSound { get; private set; }
        public SoundEffectInstance _progessS { get; private set; }
        public AudioManager(ContentManager content)
        {
            _content = content;

            ChargeSound = _content.Load<SoundEffect>("SFX/Charge");
            _chargeS = ChargeSound.CreateInstance();
            CastingSound = _content.Load<SoundEffect>("SFX/Casting");
            _castS = CastingSound.CreateInstance();
            StepingSound = _content.Load<SoundEffect>("SFX/Step");
            _stepingS = StepingSound.CreateInstance();
            HookedSound = _content.Load<SoundEffect>("SFX/Hooked");
            _hookedS = HookedSound.CreateInstance();
            ReelInSound = _content.Load<SoundEffect>("SFX/ReelIn");
            _reelInS = ReelInSound.CreateInstance();
            ReelOutSound = _content.Load<SoundEffect>("SFX/ReelOut");
            _reelOutS = ReelOutSound.CreateInstance();
            CatchSound = _content.Load<SoundEffect>("SFX/Catch");
            _catchS = CatchSound.CreateInstance();
            PressSound = _content.Load<SoundEffect>("SFX/PressB");
            _pressS = PressSound.CreateInstance();
            ProgessSound = _content.Load<SoundEffect>("SFX/Progess");
            _progessS = ProgessSound.CreateInstance();
        }

        public void IncVolume()
        {
            if (Volume + 0.2f <= 1f)
            {
                Volume += 0.2f;

                _chargeS.Volume = Volume;
                _catchS.Volume = Volume;
                _castS.Volume = Volume;
                _stepingS.Volume = Volume;
                _hookedS.Volume = Volume;
                _reelInS.Volume = Volume;
                _reelOutS.Volume = Volume;
                _pressS.Volume = Volume;
                _progessS.Volume = Volume;
            }

        }

        public void DecVolume()
        {
            if (Volume - 0.2f >= 0f)
            {
                Volume -= 0.2f;

                _chargeS.Volume = Volume;
                _catchS.Volume = Volume;
                _castS.Volume = Volume;
                _stepingS.Volume = Volume;
                _hookedS.Volume = Volume;
                _reelInS.Volume = Volume;
                _reelOutS.Volume = Volume;
                _pressS.Volume = Volume;
                _progessS.Volume = Volume;
            }

        }
        public float GetVolume()
        {
            return Volume;
        }
    }
}
