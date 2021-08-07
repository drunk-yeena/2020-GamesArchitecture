using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        int audioBuffer;
        int audioSource;
        bool audioSwitch = true;
        bool audioloop = false;

        public ComponentAudio(string audioName)
        {
            audioBuffer = ResourceManager.LoadAudio(audioName);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
        }

        public void AudioPosition(Vector3 audioPosition)
        {
            AL.Source(audioSource, ALSource3f.Position, ref audioPosition);
        }

        public void AudioVelocity(Vector3 audioVelocity)
        {
            AL.Source(audioSource, ALSource3f.Velocity, ref audioVelocity);
        }

        public void AudioLooping()
        {
            if (audioloop == false)
            {
                AL.Source(audioSource, ALSourceb.Looping, true);
                audioloop = true;
            }
            else
            {
                AL.Source(audioSource, ALSourceb.Looping, false);
                audioloop = false;
            }
        }

        public void PlayAudio()
        {
            AL.SourcePlay(audioSource);
        }

        public void AudioSwitch()
        {
            if (audioSwitch == false)
            {
                AL.SourcePlay(audioSource); // play the ausio source
                audioSwitch = true;
            }
            else
            {
                AL.SourcePause(audioSource);
                audioSwitch = false;
            }
        }

        public int Audio
        {
            get { return Audio; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}
